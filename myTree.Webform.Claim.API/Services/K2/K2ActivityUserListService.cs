using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using myTree.MicroService.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CI.TMS.Claim.API.Domain.Entities.K2;

namespace CI.TMS.Claim.API.Services.K2
{
    public class K2ActivityUserListService : BaseService
    {
        dbIntegratedPortalContext contextIntegratedPortal;
        public K2ActivityUserListService(
            ClaimContext context,
            dbIntegratedPortalContext _contextIntegratedPortal,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrencyService> log
            ) : base(context, httpContextAccessor, log)
        {
            contextIntegratedPortal = _contextIntegratedPortal;
        }

        public async Task<IEnumerable<K2ActivityUserListResponseDTO>> Get(Expression<Func<K2ActivityUserList, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != null && x.Id != 0;
                return await contextIntegratedPortal.K2ActivityUserList
                    .Where(predicate)
                    .OrderBy(x => x.Folder)
                    .ThenBy(x => x.ProcessName)
                    .ThenBy(x => x.RelevantId)
                    .ThenBy(x => x.SeqNo)
                    .ThenBy(x => x.ActivityId)
                    .Project().To<K2ActivityUserListResponseDTO>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<long?> Add(K2ActivityUserListRequestDTO data)
        {
            try
            {
                var model = new K2ActivityUserList();
                model.MapFrom(data);

                await contextIntegratedPortal.K2ActivityUserList.AddAsync(model);
                await contextIntegratedPortal.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<K2ActivityUserListResponseDTO> Update(K2ActivityUserListRequestDTO data)
        {
            try
            {
                var model = await contextIntegratedPortal.K2ActivityUserList.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("K2ActivityUserList not found.");

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

        public async Task HardDelete(long? id)
        {
            try
            {
                var model = await contextIntegratedPortal.K2ActivityUserList.FirstOrDefaultAsync(x => x.Id == id);
                if (model != null)
                {
                    contextIntegratedPortal.Remove(model);
                    await contextIntegratedPortal.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}
