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
    public class ClaimExpenseService : BaseService
    {
        public ClaimExpenseService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimExpenseService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<IEnumerable<ClaimExpenseResponseDTO>> Get(Expression<Func<ClaimExpense, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "";
                return await context.ClaimExpense.Where(predicate)
                .SelectMany(tat => context.ExpenseType.Where(tate => tat.ExpenseTypeId == tate.ExpenseTypeId).DefaultIfEmpty(), (tat, tate) => new { tat = tat, tate = tate })
                .SelectMany(tat => context.ClaimDocument.Where(doc => tat.tat.ClaimDocumentId == doc.Id).DefaultIfEmpty(), (tat, doc) => new { tat = tat, doc = doc })
                .SelectMany(tat => context.ClaimDocument.Where(disDoc => tat.tat.tat.DisagreeDocumentId == disDoc.Id).DefaultIfEmpty(), (tat, disDoc) => new { tat = tat, disDoc = disDoc })
                .SelectMany(tat => context.ExpenseType.Where(tate2 => tat.tat.tat.tat.ExpenseTypeIdApproval == tate2.ExpenseTypeId).DefaultIfEmpty(), (tat, tate2) => new { tat = tat, tate2 = tate2 })
                .Select(select => new
                {
                    Id = select.tat.tat.tat.tat.Id,
                    ClaimId = select.tat.tat.tat.tat.ClaimId,
                    ExpenseClaimDate = select.tat.tat.tat.tat.ExpenseClaimDate,
                    CountryId = select.tat.tat.tat.tat.CountryId,
                    CountryName = select.tat.tat.tat.tat.CountryName,
                    CityId = select.tat.tat.tat.tat.CityId,
                    CityName = select.tat.tat.tat.tat.CityName,
                    OtherCityLocation = select.tat.tat.tat.tat.OtherCityLocation,
                    Expenditure = select.tat.tat.tat.tat.Expenditure,
                    ReceiptNo = select.tat.tat.tat.tat.ReceiptNo,
                    CurrencyId = select.tat.tat.tat.tat.CurrencyId,
                    CurrencyName = select.tat.tat.tat.tat.CurrencyName,
                    Amount = select.tat.tat.tat.tat.Amount,
                    AmountApproval = select.tat.tat.tat.tat.AmountApproval,
                    ExchangeRate = select.tat.tat.tat.tat.ExchangeRate,
                    USDAmount = select.tat.tat.tat.tat.USDAmount,
                    Remarks = select.tat.tat.tat.tat.Remarks,
                    Operator = select.tat.tat.tat.tat.Operator,
                    ClaimDocumentId = select.tat.tat.tat.tat.ClaimDocumentId,
                    ExpenseTypeId = select.tat.tat.tat.tat.ExpenseTypeId,
                    ExpenseTypeName = select.tat.tat.tat.tate.AccountDescription,
                    AmountApprovalUsd = select.tat.tat.tat.tat.AmountApprovalUsd,
                    ExchangeRateApproval = select.tat.tat.tat.tat.ExchangeRateApproval,
                    OperatorApproval = select.tat.tat.tat.tat.OperatorApproval,
                    CommentApproval = select.tat.tat.tat.tat.CommentApproval,
                    ReasonDisagree = select.tat.tat.tat.tat.ReasonDisagree,
                    StatusApproval = select.tat.tat.tat.tat.StatusApproval,
                    DisagreeDocumentId = select.tat.tat.tat.tat.DisagreeDocumentId,
                    CurrencyIdApproval = select.tat.tat.tat.tat.CurrencyIdApproval,
                    CurrencyNameApproval = select.tat.tat.tat.tat.CurrencyNameApproval,
                    ExpenseTypeIdApproval = select.tat.tat.tat.tat.ExpenseTypeIdApproval,
                    ExpenseTypeNameApproval = select.tate2.AccountDescription,
                    IsActive = select.tat.tat.tat.tat.IsActive,
                    IsFinance = select.tat.tat.tat.tat.IsFinance,
                    Status = select.tat.tat.tat.tat.Status,
                    ExpenseDocument = new ClaimDocumentResponseDTO { Id = select.tat.tat.doc.Id == null ? Guid.Empty : select.tat.tat.doc.Id, FileName = select.tat.tat.doc.FileName ?? "", FileUrl = select.tat.tat.doc.FileUrl ?? "" },
                    DisagreeDocument = new ClaimDocumentResponseDTO { Id = select.tat.disDoc.Id == null ? Guid.Empty : select.tat.disDoc.Id, FileName = select.tat.disDoc.FileName ?? "", FileUrl = select.tat.disDoc.FileUrl ?? "" }
                }).AsNoTracking().Project().To<ClaimExpenseResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimExpenseResponseDTO>> GetById(Guid id, Expression<Func<ClaimExpense, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.Id == id;
                return await context.ClaimExpense.Where(predicate).Project().To<ClaimExpenseResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<List<ClaimExpenseResponseDTO>> GetByClaimId(Guid id, Expression<Func<ClaimExpense, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id.ToString() != "" && x.ClaimId == id;
                return await context.ClaimExpense.Where(predicate).Project().To<ClaimExpenseResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


        public async Task<Guid> AddOrUpdateOrDelete(ClaimExpenseRequestDTO data)
        {
            var model = new ClaimExpense();
            try
            {
                if (data.Id == Guid.Empty)
                {
                    model.Id = await this.Add(data);
                }
                else if (data.Id == Guid.Empty && data.IsActive == false)
                {
                    await this.Delete(data.Id);
                    model.Id = data.Id;
                }
                else
                {
                    await this.Update(data);
                    model.Id = data.Id;
                }
                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
        public async Task<Guid> Add(ClaimExpenseRequestDTO data)
        {
            try
            {
                var model = new ClaimExpense();
                model.MapFrom(data);
                model.CreatedAt = DateTime.Now;
                model.ClaimDocumentId = data.ClaimDocumentId;
                model.CreatedBy = userId;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = true;

                await context.ClaimExpense.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimExpenseResponseDTO> Update(ClaimExpenseRequestDTO data)
        {
            try
            {
                var model = await context.ClaimExpense.AsNoTracking().FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Claim expense not found.");

                var newModel = new ClaimExpense();
                newModel.MapFrom(model);

                newModel.ExpenseClaimDate = Convert.ToDateTime(data.ExpenseClaimDate);
                newModel.CountryId = data.CountryId;
                newModel.CountryName = data.CountryName;
                newModel.CityId = data.CityId;
                newModel.CityName = data.CityName;
                newModel.OtherCityLocation = data.OtherCityLocation;
                newModel.Expenditure = data.Expenditure;
                newModel.ReceiptNo = data.ReceiptNo;
                newModel.CurrencyId = data.CurrencyId;
                newModel.CurrencyIdApproval = data.CurrencyIdApproval;
                newModel.CurrencyName = data.CurrencyName;
                newModel.CurrencyNameApproval = data.CurrencyNameApproval;
                newModel.Amount = data.Amount;
                newModel.AmountApproval = data.AmountApproval;
                newModel.ExchangeRate = data.ExchangeRate;
                newModel.ExchangeRateApproval = data.ExchangeRateApproval;
                newModel.USDAmount = data.USDAmount;
                newModel.AmountApprovalUsd = data.AmountApprovalUsd;
                newModel.Remarks = data.Remarks;
                newModel.Operator = data.Operator;
                newModel.OperatorApproval = data.OperatorApproval;
                newModel.IsActive = data.IsActive;
                newModel.IsFinance = data.IsFinance;
                newModel.CommentApproval = data.CommentApproval;
                newModel.ReasonDisagree = data.ReasonDisagree;
                newModel.Status = data.Status;
                newModel.StatusApproval = data.StatusApproval;
                newModel.DisagreeDocumentId = data.DisagreeDocumentId;
                newModel.ClaimDocumentId = data.ClaimDocumentId;
                newModel.UpdatedBy = userId;
                newModel.UpdatedAt = DateTime.Now;
                newModel.ExpenseTypeId = data.ExpenseTypeId;
                newModel.ExpenseTypeIdApproval = data.ExpenseTypeIdApproval;

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
        public async Task<ClaimExpenseResponseDTO> UpdateIsActive(Guid id)
        {
            try
            {
                var model = await context.ClaimExpense.FirstOrDefaultAsync(x => x.Id == id);
                if (model == null)
                    throw new Exception("Claim expense not found.");

                model.UpdatedBy = userId;
                model.IsActive = false;
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
                var model = await context.ClaimExpense.FirstOrDefaultAsync(x => x.Id == id);

                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = false;

                context.ClaimExpense.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }



    }
}