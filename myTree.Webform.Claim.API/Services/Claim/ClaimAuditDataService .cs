using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimAuditDataService : BaseService
    {
        ClaimPerdiemDetailService claimperdiemdtlSvc;
        ClaimDocumentService claimDocumentSvc;
        ClaimConditionService claimConditionSvc;

        public ClaimAuditDataService(
            ClaimContext context,
            ClaimPerdiemDetailService _claimperdiemdetailSvc,
            ClaimDocumentService _claimDocumentSvc,
            ClaimConditionService _claimConditionSvc,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ClaimAuditDataService> log)
           : base(context, httpContextAccessor, log)
        {
            claimperdiemdtlSvc = _claimperdiemdetailSvc;
            claimDocumentSvc = _claimDocumentSvc;
            claimConditionSvc = _claimConditionSvc;
        }
        public async Task<List<ClaimAuditDataResponseDTO>> Get(string accessToken, Expression<Func<ClaimAuditData, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.Id != Guid.Empty;

                return await context.ClaimAuditData.Where(predicate).AsNoTracking().Project().To<ClaimAuditDataResponseDTO>().ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<ClaimAuditDataResponseDTO>> GetClaimAudit(string accessToken, string ClaimId)
        {
            try
            {

                List<ClaimAuditDataResponseDTO> Header = await context.ClaimAuditData
                   .Where(x => x.ModuleId.Equals(ClaimId) && x.SubModule.Equals("Claim"))
                    .SelectMany(data => context.Employee.Where(emp => data.ChangeBy == emp.EmpUserId && !string.IsNullOrEmpty(emp.EmpUserId) && !string.IsNullOrEmpty(data.ChangeBy)).DefaultIfEmpty(), (data, emp) => new { AuditData = data, Employee = emp })
                    .Select(select => new
                    {
                        ModuleId = select.AuditData.ModuleId,
                        RecId = select.AuditData.RecId,
                        SubModule = select.AuditData.SubModule,
                        ChangeBy = select.Employee.EmpName,
                        ChangeTime = select.AuditData.ChangeTime,
                        ChangeType = select.AuditData.ChangeType,
                        FieldName = select.AuditData.FieldName == "TotalPerdiemClaim" ? "Total perdiem claim" :
                                    select.AuditData.FieldName == "TotalExpenseClaim" ? "Total expense claim" :
                                    select.AuditData.FieldName == "TotalTEC" ? "Total TEC" :
                                    select.AuditData.FieldName == "AdvanceAmount" ? "Advance amount" :
                                    select.AuditData.FieldName == "JournalNo" ? "Journal No" :
                                    select.AuditData.FieldName == "StatusId" ? "Status" :
                                    select.AuditData.FieldName == "ClaimConditionId" ? "Condition" :
                                    select.AuditData.FieldName == "TravelOfficeId" ? "Travel office" :
                                    select.AuditData.FieldName == "IsHaveClaim" ? "Is have claim" :
                                    select.AuditData.FieldName == "Period" ? "Period" :
                                    select.AuditData.FieldName == "TransactionDate" ? "Transaction date" :
                                    select.AuditData.FieldName == "DueDate" ? "Due date" : ""
                        ,
                        PreviousValue = ((select.AuditData.FieldName != "TransactionDate" && select.AuditData.FieldName != "DueDate") ? select.AuditData.PreviousValue : (select.AuditData.PreviousValue == "" ? select.AuditData.PreviousValue : Convert.ToDateTime(select.AuditData.PreviousValue).ToString("dd MMM yyyy"))),
                        NewValue = ((select.AuditData.FieldName != "TransactionDate" && select.AuditData.FieldName != "DueDate") ? select.AuditData.NewValue : (select.AuditData.NewValue == "" ? select.AuditData.NewValue : Convert.ToDateTime(select.AuditData.NewValue).ToString("dd MMM yyyy"))),
                        ReasonOfChange = select.AuditData.ReasonOfChange,
                        ApprovalNo = select.AuditData.ApprovalNo,
                        SeqNo = select.AuditData.SeqNo
                    })
                   .AsNoTracking()
                   .Project()
                   .To<ClaimAuditDataResponseDTO>()
                   .ToListAsync();

                foreach (var item in Header)
                {
                    if (item.FieldName == "Condition")
                    {
                        Guid previousConditionId = new Guid(item.PreviousValue);
                        Guid newConditionId = new Guid(item.NewValue);

                        var modelPrevious = await context.ClaimCondition.FirstOrDefaultAsync(x => x.Id == previousConditionId);
                        if (modelPrevious != null)
                        {
                            item.PreviousValue = string.Format("{0}", modelPrevious.Description);
                        }
                        else
                        {
                            item.PreviousValue = "";
                        }

                        var modelNew = await context.ClaimCondition.FirstOrDefaultAsync(x => x.Id == newConditionId);
                        if (modelNew != null)
                        {
                            item.NewValue = string.Format("{0}", modelNew.Description);
                        }
                        else
                        {
                            item.NewValue = "";
                        }
                    }

                }

                List<ClaimAuditDataResponseDTO> Detail = await context.ClaimAuditData
                    .Where(x => x.ModuleId.Equals(ClaimId) && x.SubModule != "Claim")
                    .SelectMany(data => context.Employee.Where(emp => data.ChangeBy == emp.EmpUserId && !string.IsNullOrEmpty(emp.EmpUserId) && !string.IsNullOrEmpty(data.ChangeBy)).DefaultIfEmpty(), (data, emp) => new { AuditData = data, Employee = emp })
                    .Select(select => new
                    {
                        ModuleId = select.AuditData.ModuleId,
                        RecId = select.AuditData.RecId,
                        SubModule = select.AuditData.SubModule,
                        ChangeBy = select.Employee.EmpName,
                        ChangeTime = select.AuditData.ChangeTime,
                        ChangeType = select.AuditData.ChangeType,
                        FieldName = select.AuditData.SubModule == "ClaimPerdiem" ? "Claim perdiem" :
                                    select.AuditData.SubModule == "ClaimPerdiemDetail" ? "Claim perdiem detail" :
                                    select.AuditData.SubModule == "ClaimPerdiemChargeCode" ? "Claim perdiem charge code" :
                                    select.AuditData.SubModule == "ClaimExpense" ? "Claim expense" :
                                    select.AuditData.SubModule == "ClaimExpenseChargeCode" ? "Claim expense charge code" :
                                    select.AuditData.SubModule == "ClaimSupportingDocument" ? "Claim supporting document" :
                                    select.AuditData.SubModule == "ClaimBoardingPassDocument" ? "Claim boarding pass document" : "-"
                        ,
                        PreviousValue = "",
                        NewValue = "",
                        ReasonOfChange = "",
                        ApprovalNo = select.AuditData.ApprovalNo,
                        SeqNo = select.AuditData.SeqNo
                    })
                    .AsNoTracking()
                    .Project()
                    .To<ClaimAuditDataResponseDTO>()
                    .ToListAsync();

                List<string> auditDetailSubmoduleList = new List<string>
                {
                    "claim perdiem detail",
                    "claim perdiem charge code",
                    "claim perdiem",
                    "claim expense",
                    "claim expense charge code",
                    "claim supporting document",
                    "claim boarding pass document"
                };
                var groupDetail = Detail
                .GroupBy(item => new { item.RecId, item.SubModule, item.ChangeTime })
                .Select(group =>
                {
                    ClaimAuditDataResponseDTO selectedItem = group.Any(x => x.ChangeType.ToLower() == "deleted" && auditDetailSubmoduleList.Contains(x.FieldName.ToLower()))
                        ? group.First(x => x.ChangeType.ToLower() == "deleted" && auditDetailSubmoduleList.Contains(x.FieldName.ToLower()))
                        : group.First();

                    return selectedItem;
                })
                .ToList();
                var combinedResult = Header.Union(groupDetail).ToList();

                return combinedResult;


            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<dynamic>> GetClaimAuditDetail(string accessToken, Expression<Func<ClaimAuditData, bool>>? predicate = null)
        {
            try
            {


                List<ClaimAuditDataResponseDTO> dataAudit = await context.ClaimAuditData
                   .Where(predicate)
                    .Select(select => new
                    {
                        ModuleId = select.ModuleId,
                        RecId = select.RecId,
                        SubModule = select.SubModule,
                        ChangeBy = select.ChangeBy,
                        ChangeTime = select.ChangeTime,
                        ChangeType = select.ChangeType,
                        FieldName = select.FieldName,
                        PreviousValue = select.PreviousValue,
                        NewValue = select.NewValue
                    })
                   .AsNoTracking()
                   .Project()
                   .To<ClaimAuditDataResponseDTO>()
                   .ToListAsync();

                var submodule = dataAudit.FirstOrDefault().SubModule;
                Guid recId = new Guid(dataAudit.FirstOrDefault().RecId);

                List<dynamic> result = new List<dynamic>();

                #region peridem
                if (submodule == "ClaimPerdiem")
                {
                    var perdiem = await context.ClaimPerdiem
                                 .Where(x => x.Id == recId)
                                  .SelectMany(per =>
                                            context.Country.Where(coty => per.CountryId == coty.Id).DefaultIfEmpty(),
                                            (per, coty) => new { per = per, coty = coty })
                                         .SelectMany(per =>
                                            context.City.Where(cty => per.per.CityId == cty.Id).DefaultIfEmpty(),
                                            (per, cty) => new { per = per, cty = cty })
                                    .Select(select => new
                                    {
                                        RowValue = "",
                                        DateTo = Convert.ToDateTime(select.per.per.DateTo).ToString("dd MMM yyyy"),
                                        DateFrom = Convert.ToDateTime(select.per.per.DateFrom).ToString("dd MMM yyyy"),
                                        CountryName = select.per.coty.Name,
                                        CityName = select.cty.Name,
                                        CityOther = select.per.per.CityOther,
                                        Currency = select.per.per.Currency,
                                        B = select.per.per.B == null ? false : select.per.per.B,
                                        L = select.per.per.L == null ? false : select.per.per.L,
                                        D = select.per.per.D == null ? false : select.per.per.D,
                                        I = select.per.per.I == null ? false : select.per.per.I,
                                        F = select.per.per.F == null ? false : select.per.per.F,
                                        TotalAmount = select.per.per.TotalAmount

                                    })
                                 .AsNoTracking()
                                 .Project()
                                 .To<ClaimAuditPerdiemResponseDTO>()
                                 .FirstOrDefaultAsync();

                    #region previous value & new value
                    var itemPrevious = new ClaimAuditPerdiemResponseDTO();
                    itemPrevious.MapFrom(perdiem);

                    var itemNew = new ClaimAuditPerdiemResponseDTO();
                    itemNew.MapFrom(perdiem);

                    foreach (var itemAudit in dataAudit)
                    {

                        if (itemAudit.FieldName == "DateTo")
                        {
                            itemPrevious.DateTo = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? "" : Convert.ToDateTime(itemAudit.PreviousValue).ToString("dd MMM yyyy");
                            itemNew.DateTo = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? "" : Convert.ToDateTime(itemAudit.NewValue).ToString("dd MMM yyyy");
                        }
                        else if (itemAudit.FieldName == "DateFrom")
                        {
                            itemPrevious.DateFrom = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? "" : Convert.ToDateTime(itemAudit.PreviousValue).ToString("dd MMM yyyy");
                            itemNew.DateFrom = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? "" : Convert.ToDateTime(itemAudit.NewValue).ToString("dd MMM yyyy");
                        }
                        else if (itemAudit.FieldName == "CountryId")
                        {
                            var model = await context.Country.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.CountryName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.CountryName = "";
                            }

                            var modelNew = await context.Country.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.CountryName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.CountryName = "";
                            }
                        }

                        else if (itemAudit.FieldName == "CityId")
                        {
                            var model = await context.City.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.CityName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.CityName = "";
                            }

                            var modelNew = await context.City.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.CityName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.CityName = "";
                            }
                        }

                        else if (itemAudit.FieldName == "CityOther")
                        {
                            itemPrevious.CityOther = itemAudit.PreviousValue;
                            itemNew.CityOther = itemAudit.NewValue;
                        }

                        else if (itemAudit.FieldName == "Currency")
                        {
                            itemPrevious.Currency = itemAudit.PreviousValue;
                            itemNew.Currency = itemAudit.NewValue;
                        }

                        else if (itemAudit.FieldName == "B")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.B = true;
                            }
                            else
                            {
                                itemPrevious.B = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.B = true;
                            }
                            else
                            {
                                itemNew.B = false;
                            }

                        }
                        else if (itemAudit.FieldName == "L")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.L = true;
                            }
                            else
                            {
                                itemPrevious.L = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.L = true;
                            }
                            else
                            {
                                itemNew.L = false;
                            }

                        }
                        else if (itemAudit.FieldName == "D")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.D = true;
                            }
                            else
                            {
                                itemPrevious.D = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.D = true;
                            }
                            else
                            {
                                itemNew.D = false;
                            }
                        }
                        else if (itemAudit.FieldName == "I")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.I = true;
                            }
                            else
                            {
                                itemPrevious.I = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.I = true;
                            }
                            else
                            {
                                itemNew.I = false;
                            }
                        }
                        else if (itemAudit.FieldName == "F")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.F = true;
                            }
                            else
                            {
                                itemPrevious.F = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.F = true;
                            }
                            else
                            {
                                itemNew.F = false;
                            }
                        }

                        else if (itemAudit.FieldName == "TotalAmount")
                        {
                            itemPrevious.TotalAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.TotalAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion


                }
                #endregion

                #region perdiem detail
                if (submodule == "ClaimPerdiemDetail")
                {
                    var perdiemDetail = await context.ClaimPerdiemDetail
                                        .Where(x => x.Id == recId)
                                        .SelectMany(per =>
                                            context.Country.Where(coty => per.CountryId == coty.Id).DefaultIfEmpty(),
                                            (per, coty) => new { per = per, coty = coty })
                                         .SelectMany(per =>
                                            context.City.Where(cty => per.per.CityId == cty.Id).DefaultIfEmpty(),
                                            (per, cty) => new { per = per, cty = cty })
                                        .Select(select => new
                                        {
                                            RowValue = "",
                                            Date = Convert.ToDateTime(select.per.per.Date).ToString("dd MMM yyyy"),
                                            CountryName = select.per.coty.Name,
                                            CityName = select.cty.Name,
                                            CityOther = select.per.per.CityOther,
                                            Currency = select.per.per.Currency,
                                            B = select.per.per.B == null ? false : select.per.per.B,
                                            L = select.per.per.L == null ? false : select.per.per.L,
                                            D = select.per.per.D == null ? false : select.per.per.D,
                                            I = select.per.per.I == null ? false : select.per.per.I,
                                            F = select.per.per.F == null ? false : select.per.per.F,
                                            BAmount = select.per.per.BAmount,
                                            LAmount = select.per.per.LAmount,
                                            DAmount = select.per.per.DAmount,
                                            IAmount = select.per.per.IAmount,
                                            Amount = select.per.per.Amount,
                                            BFinance = select.per.per.BFinance == null ? false : select.per.per.BFinance,
                                            LFinance = select.per.per.LFinance == null ? false : select.per.per.LFinance,
                                            DFinance = select.per.per.DFinance == null ? false : select.per.per.DFinance,
                                            IFinance = select.per.per.IFinance == null ? false : select.per.per.IFinance,
                                            FFinance = select.per.per.FFinance == null ? false : select.per.per.FFinance,
                                            AmountFinance = select.per.per.AmountFinance,
                                            BFinanceAmount = select.per.per.BFinanceAmount,
                                            LFinanceAmount = select.per.per.LFinanceAmount,
                                            DFinanceAmount = select.per.per.DFinanceAmount,
                                            IFinanceAmount = select.per.per.IFinanceAmount,
                                        })
                                        .AsNoTracking()
                                        .Project()
                                        .To<ClaimAuditPerdiemDetailResponseDTO>()
                                        .FirstOrDefaultAsync();

                    #region previous value & new value
                    var itemPrevious = new ClaimAuditPerdiemDetailResponseDTO();
                    //itemPrevious.MapFrom(perdiemDetail);

                    var itemNew = new ClaimAuditPerdiemDetailResponseDTO();
                    //itemNew.MapFrom(perdiemDetail);

                    foreach (var itemAudit in dataAudit)
                    {
                        if (itemAudit.FieldName == "Date")
                        {
                            itemPrevious.Date = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? "" : Convert.ToDateTime(itemAudit.PreviousValue).ToString("dd MMM yyyy");
                            itemNew.Date = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? "" : Convert.ToDateTime(itemAudit.NewValue).ToString("dd MMM yyyy");
                        }
                        else if (itemAudit.FieldName == "CountryId")
                        {
                            var model = await context.Country.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.CountryName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.CountryName = "";
                            }

                            var modelNew = await context.Country.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.CountryName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.CountryName = "";
                            }
                        }

                        else if (itemAudit.FieldName == "CityId")
                        {
                            var model = await context.City.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.CityName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.CityName = "";
                            }

                            var modelNew = await context.City.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.CityName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.CityName = "";
                            }
                        }

                        else if (itemAudit.FieldName == "CityOther")
                        {
                            itemPrevious.CityOther = itemAudit.PreviousValue;
                            itemNew.CityOther = itemAudit.NewValue;
                        }

                        else if (itemAudit.FieldName == "Currency")
                        {
                            itemPrevious.Currency = itemAudit.PreviousValue;
                            itemNew.Currency = itemAudit.NewValue;
                        }

                        //submission
                        else if (itemAudit.FieldName == "B")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.B = true;
                            }
                            else
                            {
                                itemPrevious.B = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.B = true;
                            }
                            else
                            {
                                itemNew.B = false;
                            }

                        }
                        else if (itemAudit.FieldName == "L")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.L = true;
                            }
                            else
                            {
                                itemPrevious.L = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.L = true;
                            }
                            else
                            {
                                itemNew.L = false;
                            }

                        }
                        else if (itemAudit.FieldName == "D")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.D = true;
                            }
                            else
                            {
                                itemPrevious.D = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.D = true;
                            }
                            else
                            {
                                itemNew.D = false;
                            }
                        }
                        else if (itemAudit.FieldName == "I")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.I = true;
                            }
                            else
                            {
                                itemPrevious.I = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.I = true;
                            }
                            else
                            {
                                itemNew.I = false;
                            }
                        }
                        else if (itemAudit.FieldName == "F")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.F = true;
                            }
                            else
                            {
                                itemPrevious.F = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.F = true;
                            }
                            else
                            {
                                itemNew.F = false;
                            }
                        }
                        else if (itemAudit.FieldName == "BAmount")
                        {
                            itemPrevious.BAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.BAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "LAmount")
                        {
                            itemPrevious.LAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.LAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "DAmount")
                        {
                            itemPrevious.DAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.DAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "IAmount")
                        {
                            itemPrevious.IAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.IAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "Amount")
                        {
                            itemPrevious.Amount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.Amount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }

                        //Finance
                        else if (itemAudit.FieldName == "BFinance")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.BFinance = true;
                            }
                            else
                            {
                                itemPrevious.BFinance = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.BFinance = true;
                            }
                            else
                            {
                                itemNew.BFinance = false;
                            }
                        }
                        else if (itemAudit.FieldName == "LFinance")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.LFinance = true;
                            }
                            else
                            {
                                itemPrevious.LFinance = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.LFinance = true;
                            }
                            else
                            {
                                itemNew.LFinance = false;
                            }
                        }
                        else if (itemAudit.FieldName == "DFinance")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.DFinance = true;
                            }
                            else
                            {
                                itemPrevious.DFinance = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.DFinance = true;
                            }
                            else
                            {
                                itemNew.DFinance = false;
                            }
                        }
                        else if (itemAudit.FieldName == "IFinance")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.IFinance = true;
                            }
                            else
                            {
                                itemPrevious.IFinance = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.IFinance = true;
                            }
                            else
                            {
                                itemNew.IFinance = false;
                            }
                        }
                        else if (itemAudit.FieldName == "FFinance")
                        {
                            if (itemAudit.PreviousValue == "1")
                            {
                                itemPrevious.FFinance = true;
                            }
                            else
                            {
                                itemPrevious.FFinance = false;
                            }

                            if (itemAudit.NewValue == "1")
                            {
                                itemNew.FFinance = true;
                            }
                            else
                            {
                                itemNew.FFinance = false;
                            }
                        }
                        else if (itemAudit.FieldName == "BFinanceAmount")
                        {
                            itemPrevious.BFinanceAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.BFinanceAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "LFinanceAmount")
                        {
                            itemPrevious.LFinanceAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.LFinanceAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "DFinanceAmount")
                        {
                            itemPrevious.DFinanceAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.DFinanceAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "IFinanceAmount")
                        {
                            itemPrevious.IFinanceAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.IFinanceAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "AmountFinance")
                        {
                            itemPrevious.AmountFinance = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.AmountFinance = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion
                }
                #endregion

                #region peridem charge code
                if (submodule == "ClaimPerdiemChargeCode")
                {
                    var perdiemChargeCode = await context.ClaimPerdiemChargeCode
                               .Where(x => x.Id == recId)
                               .SelectMany(tat => context.CostCenter.Where(cc => cc.Id == tat.CostCenterId).DefaultIfEmpty(), (tat, cc) => new { Tat = tat, CC = cc })
                               .SelectMany(tat => context.WorkOrder.Where(wo => wo.CostCenterId == tat.Tat.CostCenterId && wo.Id == tat.Tat.WorkOrderId).DefaultIfEmpty(), (tat, wo) => new { Tat = tat, WO = wo })
                               .SelectMany(tat => context.Entity.Where(et => et.CostCenterId == tat.Tat.Tat.CostCenterId && et.Id == tat.Tat.Tat.EntityId).DefaultIfEmpty(), (tat, et) => new { Tat = tat, ET = et })
                               .Select(select => new
                               {
                                   RowValue = "",
                                   CostCenterName = select.Tat.Tat.CC.Id + " - " + select.Tat.Tat.CC.Name,
                                   WorkOrderName = select.Tat.WO.Name,
                                   EntityName = select.ET.Name,
                                   LegalEntityId = select.Tat.Tat.Tat.LegalEntityId,
                                   Percentage = select.Tat.Tat.Tat.Percentage,
                                   Amount = select.Tat.Tat.Tat.Amount,
                                   Remarks = select.Tat.Tat.Tat.Remarks,
                               })
                               .AsNoTracking()
                               .Project()
                               .To<ClaimAuditPerdiemChargeCodeResponseDTO>()
                               .FirstOrDefaultAsync();

                    #region Previous value & New value
                    var itemPrevious = new ClaimAuditPerdiemChargeCodeResponseDTO();
                    itemPrevious.MapFrom(perdiemChargeCode);
                    var itemNew = new ClaimAuditPerdiemChargeCodeResponseDTO();
                    itemNew.MapFrom(perdiemChargeCode);

                    //Previous value & New value
                    foreach (var itemAudit in dataAudit)
                    {
                        if (itemAudit.FieldName == "CostCenterId")
                        {
                            var model = await context.CostCenter.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.CostCenterName = string.Format("{0} - {1}", model.Id, model.Name);
                            }
                            else
                            {
                                itemPrevious.CostCenterName = "";
                            }

                            var modelNew = await context.CostCenter.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.CostCenterName = string.Format("{0} - {1}", modelNew.Id, modelNew.Name);
                            }
                            else
                            {
                                itemNew.CostCenterName = "";
                            }

                        }
                        else if (itemAudit.FieldName == "WorkOrderId")
                        {
                            var model = await context.WorkOrder.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.WorkOrderName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.WorkOrderName = "";
                            }

                            var modelNew = await context.WorkOrder.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.WorkOrderName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.WorkOrderName = "";
                            }

                        }
                        else if (itemAudit.FieldName == "EntityName")
                        {
                            var model = await context.Entity.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.EntityName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.EntityName = "";
                            }

                            var modelNew = await context.Entity.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.EntityName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.EntityName = "";
                            }
                        }
                        else if (itemAudit.FieldName == "LegalEntityId")
                        {
                            itemPrevious.LegalEntityId = itemAudit.PreviousValue;
                            itemNew.LegalEntityId = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "Percentage")
                        {
                            itemPrevious.Percentage = itemAudit.PreviousValue != "" ? Convert.ToDecimal(itemAudit.PreviousValue) : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.Percentage = itemAudit.NewValue != "" ? Convert.ToDecimal(itemAudit.NewValue) : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "Amount")
                        {
                            itemPrevious.Amount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.Amount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "Remarks")
                        {
                            itemPrevious.Remarks = itemAudit.PreviousValue;
                            itemNew.Remarks = itemAudit.NewValue;
                        }
                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion

                }
                #endregion

                #region Expense
                if (submodule == "ClaimExpense")
                {
                    var expense = await context.ClaimExpense
                                .Where(x => x.Id == recId)
                                .SelectMany(tat => context.ExpenseType.Where(tate => tat.ExpenseTypeId == tate.ExpenseTypeId).DefaultIfEmpty(), (tat, tate) => new { tat = tat, tate = tate })
                                .SelectMany(tat => context.ClaimDocument.Where(doc => tat.tat.ClaimDocumentId == doc.Id).DefaultIfEmpty(), (tat, doc) => new { tat = tat, doc = doc })
                                .SelectMany(tat => context.ClaimDocument.Where(disDoc => tat.tat.tat.DisagreeDocumentId == disDoc.Id).DefaultIfEmpty(), (tat, disDoc) => new { tat = tat, disDoc = disDoc })
                                .SelectMany(tat => context.ExpenseType.Where(tate2 => tat.tat.tat.tat.ExpenseTypeIdApproval == tate2.ExpenseTypeId).DefaultIfEmpty(), (tat, tate2) => new { tat = tat, tate2 = tate2 })
                                .Select(select => new
                                {
                                    RowValue = "",
                                    ExpenseClaimDate = Convert.ToDateTime(select.tat.tat.tat.tat.ExpenseClaimDate).ToString("dd MMM yyyy"),
                                    CountryName = select.tat.tat.tat.tat.CountryName,
                                    CityName = select.tat.tat.tat.tat.CityName,
                                    CityOther = select.tat.tat.tat.tat.OtherCityLocation,
                                    Expenditure = select.tat.tat.tat.tat.Expenditure,
                                    CurrencyName = select.tat.tat.tat.tat.CurrencyName,
                                    Amount = select.tat.tat.tat.tat.Amount,
                                    AmountApproval = select.tat.tat.tat.tat.AmountApproval,
                                    ExchangeRate = select.tat.tat.tat.tat.ExchangeRate,
                                    USDAmount = select.tat.tat.tat.tat.USDAmount,
                                    Remarks = select.tat.tat.tat.tat.Remarks,
                                    ClaimDocumentId = select.tat.tat.tat.tat.ClaimDocumentId,
                                    ExpenseTypeName = select.tat.tat.tat.tate.AccountDescription,
                                    AmountApprovalUsd = select.tat.tat.tat.tat.AmountApprovalUsd,
                                    ExchangeRateApproval = select.tat.tat.tat.tat.ExchangeRateApproval,
                                    CommentApproval = select.tat.tat.tat.tat.CommentApproval,
                                    ReasonDisagree = select.tat.tat.tat.tat.ReasonDisagree,
                                    DisagreeDocumentId = select.tat.tat.tat.tat.DisagreeDocumentId,
                                    CurrencyNameApproval = select.tat.tat.tat.tat.CurrencyNameApproval,
                                    ExpenseTypeNameApproval = select.tate2.AccountDescription,
                                    Status = select.tat.tat.tat.tat.Status,
                                    IsFinance = select.tat.tat.tat.tat.IsFinance,
                                    FileName = select.tat.tat.tat.tat.IsFinance.Value ? "" : select.tat.tat.doc.FileName,
                                    FileUrl = select.tat.tat.tat.tat.IsFinance.Value ? "" : select.tat.tat.doc.FileUrl,
                                    FileNameFinance = select.tat.tat.tat.tat.IsFinance.Value ? select.tat.tat.doc.FileName : "",
                                    FileUrlFinance = select.tat.tat.tat.tat.IsFinance.Value ? select.tat.tat.doc.FileUrl : "",
                                    FileNameDisagree = select.tat.tat.tat.tat.IsFinance.Value ? select.tat.tat.doc.FileName : "",
                                    FileUrlDisagree = select.tat.tat.tat.tat.IsFinance.Value ? select.tat.disDoc.FileUrl : ""
                                })
                             .AsNoTracking()
                             .Project()
                             .To<ClaimAuditExpenseResponseDTO>()
                             .FirstOrDefaultAsync();

                    #region Previous value & New value
                    var itemPrevious = new ClaimAuditExpenseResponseDTO();
                    itemPrevious.MapFrom(expense);
                    var itemNew = new ClaimAuditExpenseResponseDTO();
                    itemNew.MapFrom(expense);

                    foreach (var itemAudit in dataAudit)
                    {
                        if (itemAudit.FieldName == "ExpenseClaimDate")
                        {
                            itemPrevious.ExpenseClaimDate = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? "" : Convert.ToDateTime(itemAudit.PreviousValue).ToString("dd MMM yyyy");
                            itemNew.ExpenseClaimDate = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? "" : Convert.ToDateTime(itemAudit.NewValue).ToString("dd MMM yyyy");
                        }
                        else if (itemAudit.FieldName == "CountryName")
                        {
                            itemPrevious.CountryName = itemAudit.PreviousValue;
                            itemNew.CountryName = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "CityName")
                        {
                            itemPrevious.CityName = itemAudit.PreviousValue;
                            itemNew.CityName = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "OtherCityLocation")
                        {
                            itemPrevious.CityOther = itemAudit.PreviousValue;
                            itemNew.CityOther = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "Expenditure")
                        {
                            itemPrevious.Expenditure = itemAudit.PreviousValue;
                            itemNew.Expenditure = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "CurrencyName")
                        {
                            itemPrevious.CurrencyName = itemAudit.PreviousValue;
                            itemNew.CurrencyName = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "Amount")
                        {
                            itemPrevious.Amount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.Amount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "AmountApproval")
                        {
                            itemPrevious.AmountApproval = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.AmountApproval = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "ExchangeRate")
                        {
                            itemPrevious.ExchangeRate = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.ExchangeRate = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "USDAmount")
                        {
                            itemPrevious.USDAmount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.USDAmount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "Remarks")
                        {
                            itemPrevious.Remarks = itemAudit.PreviousValue;
                            itemNew.Remarks = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "ClaimDocumentId")
                        {
                            Guid _idPrevious = itemAudit.PreviousValue == "" ? Guid.Empty : new Guid(itemAudit.PreviousValue);
                            var model = await claimDocumentSvc.GetById(_idPrevious);
                            if (model != null)
                            {
                                if (itemPrevious.IsFinance.Value)
                                {
                                    itemPrevious.FileNameFinance = string.Format("{0}", model.FirstOrDefault()?.FileName);
                                    itemPrevious.FileUrlFinance = string.Format("{0}", model.FirstOrDefault()?.FileUrl);
                                }
                                else
                                {
                                    itemPrevious.FileName = string.Format("{0}", model.FirstOrDefault()?.FileName);
                                    itemPrevious.FileUrl = string.Format("{0}", model.FirstOrDefault()?.FileUrl);
                                }

                            }
                            else
                            {
                                itemPrevious.FileNameFinance = "";
                                itemPrevious.FileUrlFinance = "";
                                itemPrevious.FileName = "";
                                itemPrevious.FileUrl = "";
                            }

                            Guid _idNew = itemAudit.NewValue == "" ? Guid.Empty : new Guid(itemAudit.NewValue);
                            var modelNew = await claimDocumentSvc.GetById(_idNew);
                            if (modelNew != null)
                            {
                                if (itemNew.IsFinance.Value)
                                {
                                    itemNew.FileNameFinance = string.Format("{0}", modelNew.FirstOrDefault()?.FileName);
                                    itemNew.FileUrlFinance = string.Format("{0}", modelNew.FirstOrDefault()?.FileUrl);
                                }
                                else
                                {
                                    itemNew.FileName = string.Format("{0}", modelNew.FirstOrDefault()?.FileName);
                                    itemNew.FileUrl = string.Format("{0}", modelNew.FirstOrDefault()?.FileUrl);
                                }

                            }
                            else
                            {
                                itemNew.FileNameFinance = "";
                                itemNew.FileUrlFinance = "";
                                itemNew.FileName = "";
                                itemNew.FileUrl = "";
                            }

                        }
                        else if (itemAudit.FieldName == "ExpenseTypeId")
                        {
                            var model = await context.ExpenseType.FirstOrDefaultAsync(x => x.ExpenseTypeId == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.ExpenseTypeName = string.Format("{0}", model.AccountDescription);
                            }
                            else
                            {
                                itemPrevious.ExpenseTypeName = "";
                            }

                            var modelNew = await context.ExpenseType.FirstOrDefaultAsync(x => x.ExpenseTypeId == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.ExpenseTypeName = string.Format("{0}", modelNew.AccountDescription);
                            }
                            else
                            {
                                itemNew.ExpenseTypeName = "";
                            }
                        }
                        else if (itemAudit.FieldName == "AmountApprovalUsd")
                        {

                            itemPrevious.AmountApprovalUsd = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue); ;
                            itemNew.AmountApprovalUsd = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "ExchangeRateApproval")
                        {
                            itemPrevious.ExchangeRateApproval = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue); ;
                            itemNew.ExchangeRateApproval = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "CommentApproval")
                        {
                            itemPrevious.CommentApproval = itemAudit.PreviousValue;
                            itemNew.CommentApproval = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "ReasonDisagree")
                        {
                            itemPrevious.ReasonDisagree = itemAudit.PreviousValue;
                            itemNew.ReasonDisagree = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "DisagreeDocumentId")
                        {
                            Guid _idPrevious = itemAudit.PreviousValue == "" ? Guid.Empty : new Guid(itemAudit.PreviousValue);
                            var model = await claimDocumentSvc.GetById(_idPrevious);
                            if (model != null)
                            {

                                itemPrevious.FileNameDisagree = string.Format("{0}", model.FirstOrDefault()?.FileName);
                                itemPrevious.FileUrlDisagree = string.Format("{0}", model.FirstOrDefault()?.FileUrl);
                            }
                            else
                            {
                                itemPrevious.FileNameDisagree = "";
                                itemPrevious.FileUrlDisagree = "";
                            }

                            Guid _idNew = itemAudit.NewValue == "" ? Guid.Empty : new Guid(itemAudit.NewValue);
                            var modelNew = await claimDocumentSvc.GetById(_idNew);
                            if (modelNew != null)
                            {
                                itemNew.FileNameDisagree = string.Format("{0}", modelNew.FirstOrDefault()?.FileName);
                                itemNew.FileUrlDisagree = string.Format("{0}", modelNew.FirstOrDefault()?.FileUrl);
                            }
                            else
                            {
                                itemNew.FileNameDisagree = "";
                                itemNew.FileUrlDisagree = "";
                            }
                        }
                        else if (itemAudit.FieldName == "CurrencyNameApproval")
                        {
                            itemPrevious.CurrencyNameApproval = itemAudit.PreviousValue;
                            itemNew.CurrencyNameApproval = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "ExpenseTypeIdApproval")
                        {
                            var model = await context.ExpenseType.FirstOrDefaultAsync(x => x.ExpenseTypeId == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.ExpenseTypeNameApproval = string.Format("{0}", model.AccountDescription);
                            }
                            else
                            {
                                itemPrevious.ExpenseTypeNameApproval = "";
                            }

                            var modelNew = await context.ExpenseType.FirstOrDefaultAsync(x => x.ExpenseTypeId == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.ExpenseTypeNameApproval = string.Format("{0}", modelNew.AccountDescription);
                            }
                            else
                            {
                                itemNew.ExpenseTypeNameApproval = "";
                            }
                        }
                        else if (itemAudit.FieldName == "Status")
                        {
                            itemPrevious.Status = itemAudit.PreviousValue;
                            itemNew.Status = itemAudit.NewValue;
                        }

                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion
                }
                #endregion

                #region Expense charge code
                if (submodule == "ClaimExpenseChargeCode")
                {
                    var expenseChargeCode = await context.ClaimExpenseChargeCode
                             .Where(x => x.Id == recId)
                             .SelectMany(tat => context.CostCenter.Where(cc => cc.Id == tat.CostCenterId).DefaultIfEmpty(), (tat, cc) => new { Tat = tat, CC = cc })
                             .SelectMany(tat => context.WorkOrder.Where(wo => wo.CostCenterId == tat.Tat.CostCenterId && wo.Id == tat.Tat.WorkOrderId).DefaultIfEmpty(), (tat, wo) => new { Tat = tat, WO = wo })
                             .SelectMany(tat => context.Entity.Where(et => et.CostCenterId == tat.Tat.Tat.CostCenterId && et.Id == tat.Tat.Tat.EntityId).DefaultIfEmpty(), (tat, et) => new { Tat = tat, ET = et })
                             .Select(select => new
                             {
                                 RowValue = "",
                                 CostCenterName = string.Format("{0} - {1}", select.Tat.Tat.CC.Id, select.Tat.Tat.CC.Name),
                                 WorkOrderName = select.Tat.WO.Name,
                                 EntityName = select.ET.Name,
                                 LegalEntityId = select.Tat.Tat.Tat.LegalEntityId,
                                 Percentage = select.Tat.Tat.Tat.Percentage,
                                 Amount = select.Tat.Tat.Tat.Amount,
                                 Remarks = select.Tat.Tat.Tat.Remarks,
                             })
                             .AsNoTracking()
                             .Project()
                             .To<ClaimAuditExpenseChargeCodeResponseDTO>()
                             .FirstOrDefaultAsync();

                    #region Previous value & New value
                    var itemPrevious = new ClaimAuditExpenseChargeCodeResponseDTO();
                    itemPrevious.MapFrom(expenseChargeCode);
                    var itemNew = new ClaimAuditExpenseChargeCodeResponseDTO();
                    itemNew.MapFrom(expenseChargeCode);

                    //Previous value & New value
                    foreach (var itemAudit in dataAudit)
                    {
                        if (itemAudit.FieldName == "CostCenterId")
                        {
                            var model = await context.CostCenter.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.CostCenterName = string.Format("{0} - {1}", model.Id, model.Name);
                            }
                            else
                            {
                                itemPrevious.CostCenterName = "";
                            }

                            var modelNew = await context.CostCenter.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.CostCenterName = string.Format("{0} - {1}", modelNew.Id, modelNew.Name);
                            }
                            else
                            {
                                itemNew.CostCenterName = "";
                            }

                        }
                        else if (itemAudit.FieldName == "WorkOrderId")
                        {
                            var model = await context.WorkOrder.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.WorkOrderName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.WorkOrderName = "";
                            }

                            var modelNew = await context.WorkOrder.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.WorkOrderName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.WorkOrderName = "";
                            }

                        }
                        else if (itemAudit.FieldName == "EntityName")
                        {
                            var model = await context.Entity.FirstOrDefaultAsync(x => x.Id == itemAudit.PreviousValue);
                            if (model != null)
                            {
                                itemPrevious.EntityName = string.Format("{0}", model.Name);
                            }
                            else
                            {
                                itemPrevious.EntityName = "";
                            }

                            var modelNew = await context.Entity.FirstOrDefaultAsync(x => x.Id == itemAudit.NewValue);
                            if (modelNew != null)
                            {
                                itemNew.EntityName = string.Format("{0}", modelNew.Name);
                            }
                            else
                            {
                                itemNew.EntityName = "";
                            }
                        }
                        else if (itemAudit.FieldName == "LegalEntityId")
                        {
                            itemPrevious.LegalEntityId = itemAudit.PreviousValue;
                            itemNew.LegalEntityId = itemAudit.NewValue;
                        }
                        else if (itemAudit.FieldName == "Percentage")
                        {
                            itemPrevious.Percentage = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.Percentage = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "Amount")
                        {
                            itemPrevious.Amount = (itemAudit.PreviousValue == null || itemAudit.PreviousValue == "") ? 0 : Convert.ToDecimal(itemAudit.PreviousValue);
                            itemNew.Amount = (itemAudit.NewValue == null || itemAudit.NewValue == "") ? 0 : Convert.ToDecimal(itemAudit.NewValue);
                        }
                        else if (itemAudit.FieldName == "Remarks")
                        {
                            itemPrevious.Remarks = itemAudit.PreviousValue;
                            itemNew.Remarks = itemAudit.NewValue;
                        }
                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion
                }
                #endregion

                #region Supporting document
                if (submodule == "ClaimSupportingDocument")
                {

                    var supportingDocument = await context.ClaimSupportingDocument
                           .Where(x => x.Id == recId)
                           .SelectMany(tat => context.ClaimDocument.Where(doc => tat.ClaimDocumentId == doc.Id).DefaultIfEmpty(), (tat, doc) => new { tat = tat, doc = doc })
                           .Select(select => new
                           {
                               RowValue = "",
                               DocumentDate = Convert.ToDateTime(select.tat.DocumentDate).ToString("dd MMM yyyy"),
                               FileName = select.doc.FileName,
                               FileUrl = select.doc.FileUrl,
                               Description = select.tat.Description
                           })
                           .AsNoTracking()
                           .Project()
                           .To<ClaimAuditSupportingDocumentResponseDTO>()
                           .FirstOrDefaultAsync();



                    #region Previous value & New value
                    var itemPrevious = new ClaimAuditSupportingDocumentResponseDTO();
                    itemPrevious.MapFrom(supportingDocument);
                    var itemNew = new ClaimAuditSupportingDocumentResponseDTO();
                    itemNew.MapFrom(supportingDocument);

                    foreach (var itemAudit in dataAudit)
                    {
                        if (itemAudit.FieldName == "Description")
                        {
                            itemPrevious.Description = itemAudit.PreviousValue;
                            itemNew.Description = itemAudit.NewValue;

                        }
                        else if (itemAudit.FieldName == "DocumentDate")
                        {
                            itemPrevious.DocumentDate = Convert.ToDateTime(itemAudit.PreviousValue).ToString("dd MMM yyyy");
                            itemNew.DocumentDate = Convert.ToDateTime(itemAudit.NewValue).ToString("dd MMM yyyy");
                        }
                        else if (itemAudit.FieldName == "ClaimDocumentId")
                        {
                            Guid _idPrevious = itemAudit.PreviousValue == "" ? Guid.Empty : new Guid(itemAudit.PreviousValue);
                            var model = await claimDocumentSvc.GetById(_idPrevious);
                            if (model != null)
                            {
                                itemPrevious.FileName = string.Format("{0}", model.FirstOrDefault()?.FileName);
                                itemPrevious.FileUrl = string.Format("{0}", model.FirstOrDefault()?.FileUrl);
                            }
                            else
                            {
                                itemPrevious.FileName = "";
                                itemPrevious.FileUrl = "";
                            }

                            Guid _idNew = itemAudit.NewValue == "" ? Guid.Empty : new Guid(itemAudit.NewValue);
                            var modelNew = await claimDocumentSvc.GetById(_idNew);
                            if (modelNew != null)
                            {
                                itemNew.FileName = string.Format("{0}", modelNew.FirstOrDefault()?.FileName);
                                itemNew.FileUrl = string.Format("{0}", modelNew.FirstOrDefault()?.FileUrl);
                            }
                            else
                            {
                                itemNew.FileName = "";
                                itemNew.FileUrl = "";
                            }

                        }

                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion
                }
                #endregion

                #region Boarding pass document
                if (submodule == "ClaimBoardingPassDocument")
                {
                    var boardingDocumentDocument = await context.ClaimBoardingPassDocument
                            .Where(x => x.Id == recId)
                             .SelectMany(tat => context.ClaimDocument.Where(doc => tat.ClaimDocumentId == doc.Id).DefaultIfEmpty(), (tat, doc) => new { tat = tat, doc = doc })
                            .Select(select => new
                            {
                                RowValue = "",
                                DocumentDate = Convert.ToDateTime(select.tat.DocumentDate).ToString("dd MMM yyyy"),
                                FileName = select.doc.FileName,
                                FileUrl = select.doc.FileUrl,
                                Description = select.tat.Description
                            })
                            .AsNoTracking()
                            .Project()
                            .To<ClaimAuditBoardingPassDocumentResponseDTO>()
                            .FirstOrDefaultAsync();

                    #region Previous value & New value
                    var itemPrevious = new ClaimAuditBoardingPassDocumentResponseDTO();
                    itemPrevious.MapFrom(boardingDocumentDocument);
                    var itemNew = new ClaimAuditBoardingPassDocumentResponseDTO();
                    itemNew.MapFrom(boardingDocumentDocument);

                    foreach (var itemAudit in dataAudit)
                    {

                        if (itemAudit.FieldName == "Description")
                        {
                            itemPrevious.Description = itemAudit.PreviousValue;
                            itemNew.Description = itemAudit.NewValue;

                        }
                        else if (itemAudit.FieldName == "DocumentDate")
                        {
                            itemPrevious.DocumentDate = Convert.ToDateTime(itemAudit.PreviousValue).ToString("dd MMM yyyy");
                            itemNew.DocumentDate = Convert.ToDateTime(itemAudit.NewValue).ToString("dd MMM yyyy");
                        }
                        else if (itemAudit.FieldName == "ClaimDocumentId")
                        {
                            Guid _idPrevious = itemAudit.PreviousValue == "" ? Guid.Empty : new Guid(itemAudit.PreviousValue);
                            var model = await claimDocumentSvc.GetById(_idPrevious);
                            if (model != null)
                            {
                                itemPrevious.FileName = string.Format("{0}", model.FirstOrDefault()?.FileName);
                                itemPrevious.FileUrl = string.Format("{0}", model.FirstOrDefault()?.FileUrl);
                            }
                            else
                            {
                                itemPrevious.FileName = "";
                                itemPrevious.FileUrl = "";
                            }

                            Guid _idNew = itemAudit.NewValue == "" ? Guid.Empty : new Guid(itemAudit.NewValue);
                            var modelNew = await claimDocumentSvc.GetById(_idNew);
                            if (modelNew != null)
                            {
                                itemNew.FileName = string.Format("{0}", modelNew.FirstOrDefault()?.FileName);
                                itemNew.FileUrl = string.Format("{0}", modelNew.FirstOrDefault()?.FileUrl);
                            }
                            else
                            {
                                itemNew.FileName = "";
                                itemNew.FileUrl = "";
                            }

                        }

                    }

                    itemPrevious.RowValue = "Previous value";
                    itemNew.RowValue = "New value";
                    result.Add(itemPrevious);
                    result.Add(itemNew);
                    #endregion
                }
                #endregion

                return result;

            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}