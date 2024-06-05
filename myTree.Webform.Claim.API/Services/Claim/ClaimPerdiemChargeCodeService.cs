using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimPerdiemChargeCodeService : BaseService
    {
        public ClaimPerdiemChargeCodeService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimPerdiemChargeCodeService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<IEnumerable<ClaimPerdiemChargeCodeResponseDTO>> Get(Expression<Func<ClaimPerdiemChargeCode, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "";

                return await context.ClaimPerdiemChargeCode.Where(predicate)
                .SelectMany(tat => context.CostCenter.Where(cc => cc.Id == tat.CostCenterId).DefaultIfEmpty(), (tat, cc) => new { Tat = tat, CC = cc })
                .SelectMany(tat => context.WorkOrder.Where(wo => wo.CostCenterId == tat.Tat.CostCenterId && wo.Id == tat.Tat.WorkOrderId).DefaultIfEmpty(), (tat, wo) => new { Tat = tat, WO = wo })
                .SelectMany(tat => context.Entity.Where(et => et.CostCenterId == tat.Tat.Tat.CostCenterId && et.Id == tat.Tat.Tat.EntityId).DefaultIfEmpty(), (tat, et) => new { Tat = tat, ET = et })
                .Select(select => new
                {
                    Id = select.Tat.Tat.Tat.Id,
                    ClaimId = select.Tat.Tat.Tat.ClaimId,
                    ClaimPerdiemId = select.Tat.Tat.Tat.ClaimPerdiemId,
                    CostCenterId = select.Tat.Tat.CC.Id,
                    CostCenterName = select.Tat.Tat.CC.Id + " - " + select.Tat.Tat.CC.Name,
                    WorkOrderId = select.Tat.WO.Id,
                    WorkOrderName = select.Tat.WO.Name,
                    EntityId = select.ET.Id,
                    EntityName = select.ET.Name,
                    LegalEntityId = select.Tat.Tat.Tat.LegalEntityId,
                    Percentage = select.Tat.Tat.Tat.Percentage,
                    Amount = select.Tat.Tat.Tat.Amount,
                    Remarks = select.Tat.Tat.Tat.Remarks,
                    SeqNo = select.Tat.Tat.Tat.SeqNo,
                    IsActive = select.Tat.Tat.Tat.IsActive,
                    BudgetHolderId = select.Tat.Tat.CC.BudgetHolderId,
                    BudgetHolderUserId = select.Tat.Tat.CC.BudgetHolderUserId
                }).OrderBy(p => p.ClaimPerdiemId).OrderBy(p => p.SeqNo).Project().To<ClaimPerdiemChargeCodeResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task AddOrUpdateOrDelete(ClaimPerdiemChargeCodeRequestDTO data)
        {
            try
            {
                if (data.Id == Guid.Empty)
                {
                    await this.Add(data);
                }
                else
                {
                    await this.Update(data);
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<Guid> Add(ClaimPerdiemChargeCodeRequestDTO data)
        {
            try
            {
                var model = new ClaimPerdiemChargeCode();
                model.MapFrom(data);

                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = userId;
                model.IsActive = true;

                await context.ClaimPerdiemChargeCode.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimPerdiemChargeCodeResponseDTO> Update(ClaimPerdiemChargeCodeRequestDTO data)
        {
            try
            {
                var model = await context.ClaimPerdiemChargeCode.AsNoTracking().FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Claim charge code not found.");
                var newModel = new ClaimPerdiemChargeCode();
                newModel.MapFrom(model);

                newModel.ClaimPerdiemId = data.ClaimPerdiemId;
                newModel.CostCenterId = data.CostCenterId;
                newModel.WorkOrderId = data.WorkOrderId;
                newModel.EntityId = data.EntityId;
                newModel.LegalEntityId = data.LegalEntityId;
                newModel.Percentage = Convert.ToDecimal(data.Percentage);
                newModel.Amount = Convert.ToDecimal(data.Amount);
                newModel.Remarks = data.Remarks;
                newModel.SeqNo = data.SeqNo;
                newModel.IsActive = data.IsActive;
                newModel.UpdatedBy = userId;
                newModel.UpdatedAt = DateTime.Now;

                context.Update(newModel);
                await context.SaveChangesAsync();

                return (await this.Get(predicate: (x => x.Id == newModel.Id))).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimPerdiemChargeCodeResponseDTO> UpdateIsActive(Guid id)
        {
            try
            {
                var model = await context.ClaimPerdiemChargeCode.FirstOrDefaultAsync(x => x.Id == id);
                if (model == null)
                    throw new Exception("Claim charge code not found.");
                model.IsActive = false;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;

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

        public async Task Delete(Guid id)
        {
            try
            {
                var model = await context.ClaimPerdiemChargeCode.FirstOrDefaultAsync(x => x.Id == id);

                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = false;

                context.ClaimPerdiemChargeCode.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<List<ClaimPerdiemChargeCodeResponseDTO>> GetById(Guid id, Expression<Func<ClaimPerdiemChargeCode, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimPerdiemChargeCode.Where(predicate).Project().To<ClaimPerdiemChargeCodeResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimPerdiemChargeCodeResponseDTO>> GetByClaimId(Guid id, Expression<Func<ClaimPerdiemChargeCode, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.ClaimId == id;
                return await context.ClaimPerdiemChargeCode.Where(predicate).Project().To<ClaimPerdiemChargeCodeResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }



    }
}