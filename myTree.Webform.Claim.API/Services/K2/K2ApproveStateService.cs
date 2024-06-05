using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using Serilog;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services.K2
{
    public class K2ApproveStateService : BaseService
    {
        public K2ApproveStateService(
            ClaimContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<K2ApproveStateService> log
        ) : base(context, httpContextAccessor, log)
        {
        }

        public async Task<IEnumerable<K2ApproveStateResponseDTO>> Get(Expression<Func<ApproveState, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != null && x.Id != new Guid();
                return await context.ApproveState
                    .Where(predicate)
                    .OrderBy(x => x.Module)
                    .ThenBy(x => x.RelevantId)
                    .ThenBy(x => x.ActivityId)
                    .Project().To<K2ApproveStateResponseDTO>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<Guid?> Add(K2ApproveStateRequestDTO data)
        {
            try
            {
                var model = new ApproveState();
                model.MapFrom(data);

                model.Id = Guid.NewGuid();

                await context.ApproveState.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<K2ApproveStateResponseDTO> Update(K2ApproveStateRequestDTO data)
        {
            try
            {
                var model = await context.ApproveState.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Approve state not found.");

                model.MapFrom(data);

                context.Update(model);
                await context.SaveChangesAsync();

                return (await this.Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task Delete(Guid? id)
        {
            try
            {
                var model = await context.ApproveState.FirstOrDefaultAsync(x => x.Id == id);
                if (model != null)
                {
                    model.State = 0;
                    context.Update(model);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task UpdateState(Guid? id)
        {
            try
            {
                var model = await context.ApproveState.FirstOrDefaultAsync(x => x.Id == id);
                if (model != null)
                {
                    model.State = 1;
                    context.Update(model);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task HardDelete(string relevantid)
        {
            try
            {
                var model = await context.ApproveState.Where(x => x.RelevantId == relevantid && x.ActivityId == "3").AsNoTracking().ToListAsync();
                if (model != null)
                {
                    foreach(var t in model)
                    {
                        await Delete(t.Id);
                        Log.Information("Hard Delete Apv State Else :" + t.Id);
                    }

                }
                else
                {
                    Log.Information("Hard Delete Apv State Else :");
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                Log.Information("Error Hard Delete:" + ex);
                throw;
            }
        }
    }
}
