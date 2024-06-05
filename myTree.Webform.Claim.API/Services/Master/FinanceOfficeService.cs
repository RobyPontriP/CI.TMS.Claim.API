using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using CI.TMS.Claim.API.Domain.Entities.Master;

namespace CI.TMS.Claim.API.Services.Master
{    
    public class FinanceOfficeService : BaseService
    {
        IConfiguration config;
        public FinanceOfficeService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<BaseService> log, IConfiguration _config)
            : base(context, httpContextAccessor, log)
        {
            config = _config;
        }


        public async Task<FinanceOfficeResponseDTO> GetByOfficeId(Expression<Func<FinanceOffice, bool>>? predicate = null)
        {
            try
            {

                return await context.FinanceOffice.Where(predicate)
                    .Select(e => new
                    {
                        e.Id,
                        TravelOfficeName = e.TravelOfficeName.Replace("Travel","").Replace("Ticket Approvers","").Trim(),
                        e.Status,
                        e.LastUpdatedAt
                    })
                    .Project().To<FinanceOfficeResponseDTO>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
