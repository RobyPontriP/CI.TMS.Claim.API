using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimJournalService : BaseService
    {
        public ClaimJournalService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimJournalService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<List<ClaimJournalResponseDTO>> Get(Expression<Func<ClaimJournal, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != Guid.Empty;

                return await context.ClaimJournal.Where(predicate).AsNoTracking().Project().To<ClaimJournalResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<Guid> Add(ClaimJournalRequestDTO data)
        {
            try
            {
                var model = new ClaimJournal();
                model.MapFrom(data);
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                await context.ClaimJournal.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimJournalResponseDTO> Update(ClaimJournalRequestDTO data)
        {
            try
            {
                var model = await context.ClaimJournal.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Claim journal not found.");

                model.MapFrom(data);
                context.Update(model);
                await context.SaveChangesAsync();
                return (await Get(predicate: (x => x.Id == model.Id))).FirstOrDefault();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task Delete(Guid id, Expression<Func<ClaimJournal, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;

               var model = await context.ClaimJournal.Where(predicate).Project().To<ClaimJournalResponseDTO>().ToListAsync();

                foreach (var item in model)
                {
                    var entity = new ClaimJournal();
                    entity.MapFrom(item);
                    context.ClaimJournal.Remove(entity);
                }
                
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

      
        public async Task<List<ClaimJournalResponseDTO>> GetById(Guid id, Expression<Func<ClaimJournal, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimJournal.Where(predicate).Project().To<ClaimJournalResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}