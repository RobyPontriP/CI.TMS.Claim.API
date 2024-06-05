using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;
using static CI.TMS.Claim.API.Helper.Variable;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimService : BaseService
    {
        public ClaimService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<ClaimService> log)
           : base(context, httpContextAccessor, log)
        {
        }
        public async Task<List<ClaimResponseDTO>> Get(Expression<Func<Domain.Entities.Claim, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty;
                return await context.Claim.Where(predicate).Project().To<ClaimResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<TravelAuthorizationResponseDTO>> GetTAId(Guid ClaimId, Expression<Func<Domain.Entities.Claim, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty && x.Id == ClaimId;

                return await
                       context.Claim.Where(predicate)
                       .SelectMany(claim => context.FinanceOffice.Where(fo => fo.Id == claim.TravelOfficeId).DefaultIfEmpty(), (fo, claim) => new { fo = fo, claim = claim })
                       .Select(select => new
                       {
                           TAId = select.fo.TAId,
                           TravelOfficeId = select.fo.TravelOfficeId,
                           TravelOfficeName = (select.claim.TravelOfficeName ?? "").Replace("Travel", "").Replace("Ticket Approvers", "").Trim(),
                           SystemCode = select.fo.SystemCode
                       }).AsNoTracking().Project().To<TravelAuthorizationResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimTotalResponseDTO>> GetAllTotal(Guid ClaimId, Expression<Func<Domain.Entities.Claim, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty && x.Id == ClaimId;


                return await (from p in context.Claim
                              select new
                              {
                                  Id = p.Id,
                                  TotalPerdiemClaim = Convert.ToDouble(p.TotalPerdiemClaim),
                                  TotalExpenseClaim = Convert.ToDouble(p.TotalExpenseClaim),
                                  TotalTEC = Convert.ToDouble(p.TotalTEC),
                                  SystemCode = p.SystemCode,
                                  StatusId = p.StatusId,
                                  ClaimConditionId = p.ClaimConditionId,
                                  CreatedBy = p.CreatedBy,
                                  Period = p.Period,
                                  TransactionDate = p.TransactionDate,
                                  DueDate = p.DueDate
                              }).Where(x => x.Id == ClaimId).AsNoTracking()
                              .Project().To<ClaimTotalResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task AddOrUpdate(ClaimRequestDTO data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.Id.ToString()))
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
        public async Task<Guid> Add(ClaimRequestDTO data)
        {
            try
            {
                var model = new Domain.Entities.Claim();
                model.MapFrom(data);

                model.CreatedAt = DateTime.Now;
                model.CreatedBy = userId;
                model.IsActive = true;

                await context.Claim.AddAsync(model);
                await context.SaveChangesAsync();

                return model.Id;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<ClaimResponseDTO> Update(ClaimRequestDTO data)
        {
            try
            {
                var model = await context.Claim.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (model == null)
                    throw new Exception("Claim not found.");

                //model.MapFrom(data);
                model.TAId = data.TAId;
                model.TotalPerdiemClaim = Convert.ToDecimal(data.TotalPerdiemClaim);
                model.TotalExpenseClaim = Convert.ToDecimal(data.TotalExpenseClaim);
                model.TotalTEC = Convert.ToDecimal(data.TotalTEC);
                model.AdvanceAmount = Convert.ToDecimal(data.AdvanceAmount);
                model.AmountChargeToPersonal = Convert.ToDecimal(data.AmountChargeToPersonal);
                model.TravelOfficeId = data.TravelOfficeId;
                model.ClaimConditionId = data.ClaimConditionId;
                model.StatusId = data.StatusId;
                model.SystemCode = data.SystemCode;
                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsHaveClaim = Convert.ToBoolean(data.IsHaveClaim);
                model.Period = data.Period;
                model.TransactionDate = ((Convert.ToDateTime(data.TransactionDate) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.TransactionDate == null)) ? null : (Convert.ToDateTime(data.TransactionDate)));
                model.DueDate = ((Convert.ToDateTime(data.DueDate) == Convert.ToDateTime("1/1/0001 12:00:00 AM") || (data.DueDate == null)) ? null : (Convert.ToDateTime(data.DueDate)));
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
                var model = await context.Claim.FirstOrDefaultAsync(x => x.Id == id);

                model.UpdatedBy = userId;
                model.UpdatedAt = DateTime.Now;
                model.IsActive = false;

                context.Claim.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimResponseDTO>> GetById(Guid id, Expression<Func<Domain.Entities.Claim, bool>>? predicate = null)
        {
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty && x.Id == id;
                return await context.Claim.Where(predicate).Project().To<ClaimResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<Boolean> DeleteAll(Guid id, Expression<Func<ClaimPerdiem, bool>>? predicate = null)
        {
            try
            {

                var claimPerdiemChargeCode = context.ClaimPerdiemChargeCode.Where(od => od.ClaimId == id);
                context.ClaimPerdiemChargeCode.RemoveRange(claimPerdiemChargeCode);
                await context.SaveChangesAsync();

                var claimperdiemdetail = context.ClaimPerdiemDetail.Where(od => od.ClaimId == id);
                context.ClaimPerdiemDetail.RemoveRange(claimperdiemdetail);
                await context.SaveChangesAsync();

                var claimperdiem = context.ClaimPerdiem.Where(od => od.ClaimId == id);
                context.ClaimPerdiem.RemoveRange(claimperdiem);
                await context.SaveChangesAsync();

                var claimExpenseChargeCode = context.ClaimExpenseChargeCode.Where(od => od.ClaimId == id);
                context.ClaimExpenseChargeCode.RemoveRange(claimExpenseChargeCode);
                await context.SaveChangesAsync();

                var claimExpense = context.ClaimExpense.Where(od => od.ClaimId == id);
                context.ClaimExpense.RemoveRange(claimExpense);
                await context.SaveChangesAsync();

                var claimComment = context.ClaimComment.Where(od => od.ClaimId == id);
                context.ClaimComment.RemoveRange(claimComment);
                await context.SaveChangesAsync();

                var claim = context.Claim.Find(id);
                context.Claim.RemoveRange(claim);
                await context.SaveChangesAsync();


                return true;

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<string> GetSystemCode(string SystemCode, Expression<Func<Domain.Entities.Claim, bool>>? predicate = null)
        {
            string newSystemCode = string.Empty;
            try
            {
                if (predicate is null)
                    predicate = x => x.Id != Guid.Empty && x.IsActive == false && x.SystemCode != null;

                string prefix = string.Empty;
                string year = string.Empty;
                string number = string.Empty;

                var lastClaim = context.Claim.Where(predicate).Where(x => x.SystemCode != null).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

                if (lastClaim != null)
                {
                    prefix = Configuration["PrefixSystemCode"];
                    year = DateAndTime.Today.Year.ToString();
                    if (lastClaim?.SystemCode.Substring(4, 4) == DateAndTime.Today.Year.ToString() && string.IsNullOrEmpty(SystemCode))
                    {
                        number = (int.Parse(lastClaim?.SystemCode.Substring(9)) + 1).ToString();
                    }
                    else
                    {
                        number = "1";
                    }

                    if (!string.IsNullOrEmpty(SystemCode))
                    {
                        newSystemCode = SystemCode;
                    }
                    else
                    {
                        newSystemCode = $"" + prefix + "/" + year + "/" + number;
                    }
                }
                else
                {
                    prefix = Configuration["PrefixSystemCode"];
                    year = DateAndTime.Today.Year.ToString();
                    number = "1";

                    if (!string.IsNullOrEmpty(SystemCode))
                    {
                        newSystemCode = SystemCode;
                    }
                    else
                    {
                        newSystemCode = $"" + prefix + "/" + year + "/" + number;
                    }
                }

                return newSystemCode;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}