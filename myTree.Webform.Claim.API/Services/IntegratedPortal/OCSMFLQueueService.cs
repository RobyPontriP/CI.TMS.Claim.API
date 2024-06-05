using CI.TMS.Claim.API.Domain.Entities.IntegratedPortal;
using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using CI.TMS.Claim.API.Services.K2;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services.IntegratedPortal
{
    public class OCSMFLQueueService : BaseService
    {
        dbIntegratedPortalContext contextIntegratedPortal;
        public OCSMFLQueueService(
            ClaimContext context,
            dbIntegratedPortalContext _contextIntegratedPortal,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrencyService> log
            ) : base(context, httpContextAccessor, log)
        {
            contextIntegratedPortal = _contextIntegratedPortal;
        }

        public async Task<IEnumerable<OCSMFLQueueResponseDTO>> Get(Expression<Func<OCSMFLQueue, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty;

                return await contextIntegratedPortal.OCSMFLQueueList
                    .Where(predicate)
                    .OrderBy(x => x.DueTime)
                    .ThenBy(x => x.Time)
                    .Project().To<OCSMFLQueueResponseDTO>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<Guid> Add(OCSMFLQueueRequestDTO data)
        {
            try
            {
                var model = new OCSMFLQueue();
                model.MapFrom(data);

                await contextIntegratedPortal.OCSMFLQueueList.AddAsync(model);
                await contextIntegratedPortal.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<OCSMFLQueueResponseDTO> Update(OCSMFLQueueRequestDTO data)
        {
            try
            {
                var model = await contextIntegratedPortal.OCSMFLQueueList.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("OCSMFLQueue  not found.");

                model.MapFrom(data);

                contextIntegratedPortal.Update(model);
                await contextIntegratedPortal.SaveChangesAsync();

                return (await Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
