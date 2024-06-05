using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using CI.TMS.Claim.API.Domain.Entities.Master;
using myTree.MicroService.Helper;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace CI.TMS.Claim.API.Services.Master
{    
    public class FinanceOfficerService : BaseService
    {
        IConfiguration config;
        public FinanceOfficerService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log, IConfiguration _config)
            : base(context, httpContextAccessor, log)
        {
            config = _config;
        }

        public async Task<List<FinanceOfficerResponseDTO>> Get(Expression<Func<FinanceOfficer, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Status == true;

                return await context.FiannceOfficer.Where(predicate)
                    .SelectMany(fo => context.Employee.Where(em => em.EmpId == fo.EmployeeId).DefaultIfEmpty(), (fo, emp) => new { FO = fo, EMP = emp })
                    .Select(select => new
                    {
                        EmployeeId = select.FO.EmployeeId,
                        UserId =  select.EMP.EmpUserId,
                        PreferedName = select.FO.PreferedName,
                        TravelOfficeId = select.FO.TravelOfficeId,
                        FinanceTECId = select.FO.FinanceTECId ,
                        LastUpdatedAt = select.FO.LastUpdatedAt ,
                        status = select.FO.Status,
                        Email = select.EMP.Email
                    }).Project().To<FinanceOfficerResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<FinanceOfficerResponseDTO> GetFinanceByOfficeId(string travelOfficeId, Expression<Func<FinanceOfficer, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Status == true;

                return await context.FiannceOfficer.Where(predicate)
                    .Select(emp => new
                    {
                        emp.EmployeeId,
                        emp.PreferedName,
                        emp.TravelOfficeId,
                        emp.FinanceTECId,
                        emp.Status,
                        emp.LastUpdatedAt
                    })
                    .Where(emp => emp.TravelOfficeId.ToUpper() == travelOfficeId.ToUpper()).Project().To<FinanceOfficerResponseDTO>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<FinanceOfficerResponseDTO>> GetFinanceByUserId(string userId, Expression<Func<FinanceOfficer, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Status == true;

                return await context.FiannceOfficer.Where(predicate)
                    .SelectMany(fo => context.Employee.Where(em => em.EmpId == fo.EmployeeId && em.EmpStatus == "Confirmed").DefaultIfEmpty(), (fo, emp) => new { FO = fo, EMP = emp })
                    .Select(select => new
                    {
                        EmployeeId = select.FO.EmployeeId,
                        UserId = select.EMP.EmpUserId,
                        PreferedName = select.FO.PreferedName,
                        TravelOfficeId = select.FO.TravelOfficeId,
                        FinanceTECId = select.FO.FinanceTECId,
                        LastUpdatedAt = select.FO.LastUpdatedAt,
                        status = select.FO.Status,
                        Email = select.EMP.Email
                    }).Where(x=>x.UserId == userId).Project().To<FinanceOfficerResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
