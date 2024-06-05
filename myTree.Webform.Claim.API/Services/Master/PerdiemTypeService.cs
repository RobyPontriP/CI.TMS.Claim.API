using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CI.TMS.Claim.API.Services
{
    public class PerdiemTypeService : BaseService
    {
        public PerdiemTypeService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<PerdiemTypeService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<PerdiemTypeResponseDTO>> GetAllPerdiemType(Expression<Func<PerdiemType, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.AccountCode);

                return await context.PerdiemType.Where(predicate)
                    .Select(x => new PerdiemTypeResponseDTO { 
                        AccountCode = x.AccountCode,
                        AccountDescription = x.AccountDescription,
                        TravelerType = x.TravelerType
                    }).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
