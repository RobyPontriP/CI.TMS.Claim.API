using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;

namespace CI.TMS.Claim.API.Services
{
    public class PartnerSupplierService : BaseService
    {
        public PartnerSupplierService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<PartnerSupplierService> log)
            : base(context, httpContextAccessor, log)
        {
        }

        public async Task<List<PartnerSupplierResponseDTO>> Get(Expression<Func<PartnerSupplier, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);

                return await context.PartnerSupplier.Where(predicate).AsNoTracking().Project().To<PartnerSupplierResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<PartnerSupplierResponseDTO>> GetAllTaxSystem(Expression<Func<PartnerSupplier, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);

                return await context.PartnerSupplier.Where(predicate)
                    .GroupBy(p => new { p.TaxSystem, p.TaxSystemName }).Select(grp => new PartnerSupplierResponseDTO() { TaxSystem = grp.Key.TaxSystem, TaxSystemName = grp.Key.TaxSystemName})
                    .Where(x=>x.TaxSystem != "").OrderBy(x=>x.TaxSystem).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<PartnerSupplierDropdown>> GetAparIdByName(Expression<Func<PartnerSupplier, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Id);

                return await context.PartnerSupplier.Where(predicate)
                    .GroupBy(p => new { p.Id, p.Name }).Select(grp => new PartnerSupplierDropdown() { Id = grp.Key.Id, Text = grp.Key.Id + " - " + grp.Key.Name })
                    .OrderBy(x => x.Text).AsNoTracking().Take(20).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
