using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Data;
using System.Linq.Expressions;
using CI.TMS.Claim.API.Domain;

namespace CI.TMS.Claim.API.Services.K2
{
    public class K2DataLogService : BaseService
    {
        public K2DataLogService(
            ClaimContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CurrencyService> log
            
            ) : base(context, httpContextAccessor, log)
        {
        }

        public async Task<IEnumerable<K2DataLogResponseDTO>> Get(Expression<Func<DataLog, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != null && x.Id != new Guid();
                return await context.DataLog.Where(predicate)
                    .OrderBy(x => x.Module)
                    .ThenBy(x => x.RelevantId)
                    .ThenByDescending(x => x.SeqNo)
                    .ThenBy(x => x.FieldName)
                    .Project().To<K2DataLogResponseDTO>()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<Guid?> Add(K2DataLogRequestDTO data)
        {
            try
            {
                var model = new DataLog();
                //model.MapFrom(data);

                model.Id = Guid.NewGuid();
                model.RelevantId = data.RelevantId;
                model.Module = data.Module;
                model.FieldName = data.FieldName;
                model.Value = data.Value;
                model.SeqNo = data.SeqNo;

                await context.DataLog.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<K2DataLogResponseDTO> Update(K2DataLogRequestDTO data)
        {
            try
            {
                var model = await context.DataLog.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Data log not found.");

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
                var model = await context.DataLog.FirstOrDefaultAsync(x => x.Id == id);
                if (model != null)
                {
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

    }
}
