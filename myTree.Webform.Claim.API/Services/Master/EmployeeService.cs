using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services.Master
{
    public class EmployeeService : BaseService
    {
        public EmployeeService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<CostCenterService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<EmployeeResponseDTO>> Get(Expression<Func<Employee, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.EmpStatus.ToUpper() != "RESIGNED" && x.EmpStatus.ToUpper() != "END OF CONTRACT" ;

                return await context.Employee.Where(predicate)
                    .Select(emp => new
                    {
                        emp.Address,
                        emp.DeptId,
                        emp.DeptName,
                        emp.DirectSupEmpId,
                        emp.DirectSupEmpName,
                        emp.DivCode,
                        emp.DivName,
                        emp.DutyptCode,
                        emp.Email,
                        emp.EmpBirthday,
                        emp.EmpConfirmDate,
                        emp.EmpId,
                        emp.EmpName,
                        emp.EmpStatus,
                        emp.EmpUserId,
                        Gender = emp.Gender == 0 ? "Female" : "Male",
                        emp.RowId,
                        emp.LegalEntityId
                    }).
                    Project().To<EmployeeResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<EmployeeResponseDTO> GetEmployeeByUsername(string username, Expression<Func<Employee, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.EmpStatus.ToUpper() != "RESIGNED" && x.EmpStatus.ToUpper() != "END OF CONTRACT";

                return await context.Employee.Where(predicate)
                    .Select(emp => new
                    {
                        emp.Address,
                        emp.DeptId,
                        emp.DeptName,
                        emp.DirectSupEmpId,
                        emp.DirectSupEmpName,
                        emp.DivCode,
                        emp.DivName,
                        emp.DutyptCode,
                        emp.Email,
                        emp.EmpBirthday,
                        emp.EmpConfirmDate,
                        emp.EmpId,
                        emp.EmpName,
                        emp.EmpStatus,
                        emp.EmpUserId,
                        Gender = emp.Gender == 0 ? "Female" : "Male",
                        emp.RowId,
                        emp.LegalEntityId
                    })
                    .Where(emp => emp.EmpUserId.ToUpper() == username.ToUpper()).Project().To<EmployeeResponseDTO>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<EmployeeResponseDTO> GetEmployeeByClaimId(Guid id, Expression<Func<Employee, bool>>? predicate = null)
        {
            try
            {
                var travelerId = await context.Claim.Where(c=> c.Id == id)
                    .SelectMany(tat => context.TravelAuthorizationTraveler.Where(p => p.TAId == tat.TAId))
                    .Select(select => select.TravelerId).FirstOrDefaultAsync();
                if (predicate == null)
                    predicate = x => x.EmpStatus.ToUpper() != "RESIGNED" && x.EmpStatus.ToUpper() != "END OF CONTRACT";

                return await context.Employee.Where(predicate)
                    .Select(emp => new
                    {
                        emp.Address,
                        emp.DeptId,
                        emp.DeptName,
                        emp.DirectSupEmpId,
                        emp.DirectSupEmpName,
                        emp.DivCode,
                        emp.DivName,
                        emp.DutyptCode,
                        emp.Email,
                        emp.EmpBirthday,
                        emp.EmpConfirmDate,
                        emp.EmpId,
                        emp.EmpName,
                        emp.EmpStatus,
                        emp.EmpUserId,
                        Gender = emp.Gender == 0 ? "Female" : "Male",
                        emp.RowId,
                        emp.LegalEntityId
                    })
                    .Where(emp => emp.RowId == travelerId).Project().To<EmployeeResponseDTO>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}

