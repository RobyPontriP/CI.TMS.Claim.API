using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using CI.TMS.Claim.API.Services.K2;
using CI.TMS.Claim.Web.Services;
using CI.TMS.General.Models.DTOs;
using CI.TMS.General.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq.Expressions;
using static CI.TMS.Claim.API.Helper.Variable;
using CI.TMS;
using System;
using System.Security.Claims;
using CI.TMS.General.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Serilog;
using OpenTelemetry.Trace;
using CI.TMS.Claim.API.Services.IntegratedPortal;
using CI.TMS.Claim.API.Domain.Entities.IntegratedPortal;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimSubmissionService : BaseService
    {
        ClaimService claimSvc;
        ClaimDataService claimDataSvc;
        ClaimPerdiemService claimperdiemSvc;
        ClaimPerdiemDetailService claimperdiemdtlSvc;
        ClaimExpenseService claimexpenseSvc;
        ClaimPerdiemChargeCodeService ClaimPerdiemChargeCodeSvc;
        ClaimExpenseChargeCodeService ClaimExpenseChargeCodeSvc;
        ClaimCommentService claimcommentSvc;
        ClaimSupportingDocumentService claimsupportingdocumentSvc;
        ClaimBoardingPassDocumentService claimboardingpassdocumentSvc;
        TravelAuthorizationService travelauthorizationSvc;
        TravelAuthorizationExtendedService travelauthorizationextendedSvc;
        NotificationServices notificationSvc;
        PerdiemRateService perdiemSvc;
        ClaimK2Service claimK2Svc;
        GeneralK2Service k2Svc;
        K2ApproveStateService approveStateSvc;
        ClaimJournalService claimJournalSvc;
        OCSMFLQueueService ocsMFLQueueSvc;
        K2ActivityUserListService k2ActivityUserListSvc;
        IConfiguration config;
        string? k2ProcessName = Configuration["K2Workflow:ProcessName"];

        public ClaimSubmissionService(
            ClaimContext _context,
            IHttpContextAccessor _httpContextAccessor,
            ILogger<BaseService> _log,
            ClaimService _claimSvc,
            ClaimPerdiemService _claimperdiemSvc,
            ClaimPerdiemDetailService _claimperdiemdetailSvc,
            ClaimExpenseService _claimexpenseSvc,
            ClaimPerdiemChargeCodeService _ClaimPerdiemChargeCodeSvc,
            ClaimExpenseChargeCodeService _ClaimExpenseChargeCodeSvc,
            ClaimCommentService _claimcommentSvc,
            ClaimSupportingDocumentService _claimsupportingdocumentSvc,
            ClaimBoardingPassDocumentService _claimboardingpassdocumentSvc,
            GeneralK2Service _k2Svc,
            TravelAuthorizationService _travelauthorizationSvc,
            TravelAuthorizationExtendedService _travelauthorizationextendedSvc,
            NotificationServices _notificationSvc,
            PerdiemRateService _perdiemSvc,
            ClaimK2Service _claimK2Svc,
            K2ApproveStateService _approveStateSvc,
            ClaimDataService _claimDataSvc,
            ClaimJournalService _claimJournalSvc,
            OCSMFLQueueService _ocsMFLQueueSvc,
            K2ActivityUserListService _k2ActivityUserListSvc,
        IConfiguration _config
            ) : base(_context, _httpContextAccessor, _log)
        {
            claimSvc = _claimSvc;
            claimperdiemSvc = _claimperdiemSvc;
            claimperdiemdtlSvc = _claimperdiemdetailSvc;
            claimexpenseSvc = _claimexpenseSvc;
            ClaimPerdiemChargeCodeSvc = _ClaimPerdiemChargeCodeSvc;
            ClaimExpenseChargeCodeSvc = _ClaimExpenseChargeCodeSvc;
            claimcommentSvc = _claimcommentSvc;
            claimsupportingdocumentSvc = _claimsupportingdocumentSvc;
            claimboardingpassdocumentSvc = _claimboardingpassdocumentSvc;
            notificationSvc = _notificationSvc;
            k2Svc = _k2Svc;
            claimK2Svc = _claimK2Svc;
            perdiemSvc = _perdiemSvc;
            approveStateSvc = _approveStateSvc;
            claimDataSvc = _claimDataSvc;
            claimJournalSvc = _claimJournalSvc;
            travelauthorizationSvc = _travelauthorizationSvc;
            travelauthorizationextendedSvc = _travelauthorizationextendedSvc;
            ocsMFLQueueSvc = _ocsMFLQueueSvc;
            config = _config;
            k2ActivityUserListSvc = _k2ActivityUserListSvc;
        }
        public async Task<SubmissionResponseDTO> SubmitClaim(SubmissionRequestDTO data, string userId, string accessToken)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                SubmissionResponseDTO ret = new SubmissionResponseDTO();
                string nextRequestId = "";

                //DECLARE
                var ClaimPerdiemId = Guid.Empty;
                var ClaimExpenseId = Guid.Empty;
                var ClaimId = Guid.Empty;
                var ClaimComment = Guid.Empty;
                string newSystemCode = string.Empty;

                ClaimRequestDTO claimRequest = new ClaimRequestDTO();
                //claimRequest.Id = Guid.NewGuid();
                claimRequest.MapFrom(data);


                var DataClaim = await claimSvc.GetById(claimRequest.Id);

                if (data.IsHaveClaim == true && (data.Action.ToLower() == "request for revision" || data.Action.ToLower() == "rejected" || data.Action.ToLower() == "approved" || data.Action.ToLower() == "draft" || data.Action.ToLower() == "saved" || data.Action.ToLower() == "submitted" || data.Action.ToLower() == "resubmitted" || data.Action.ToLower() == "redirect for budget holder(s) approval"))
                {
                    //convert amount to tbl claim
                    #region Updated Add to Claim Table
                    claimRequest.TotalPerdiemClaim = Convert.ToDecimal(data.TotalPerdiemClaim);
                    claimRequest.TotalExpenseClaim = Convert.ToDecimal(data.TotalExpenseClaim);
                    claimRequest.TotalTEC = Convert.ToDecimal(data.TotalTEC);
                    claimRequest.AdvanceAmount = Convert.ToDecimal(data.AdvanceAmount);

                    if (DataClaim.Count() > 0)
                    {
                        //claimRequest.MapFrom(claimRequest);
                        ClaimRequestDTO newClaimRequest = new ClaimRequestDTO();
                        newClaimRequest.MapFrom(claimRequest);
                        newClaimRequest.IsHaveClaim = data.IsHaveClaim;
                        newClaimRequest.TravelOfficeId = data.TravelOfficeId;
                        newClaimRequest.ClaimConditionId = data.ClaimConditionId == null ? Guid.Empty : data.ClaimConditionId;
                        newClaimRequest.SystemCode = DataClaim.Select(p => p.SystemCode)?.FirstOrDefault();
                        await claimSvc.Update(newClaimRequest);
                        ClaimId = newClaimRequest.Id;
                    }
                    else //for submit and save
                    {
                        claimRequest.Id = claimRequest.Id;
                        claimRequest.ClaimConditionId = Guid.Empty;
                        ClaimId = await claimSvc.Add(claimRequest);
                    }
                    #endregion Updated Add to Claim Table



                    int CountNotSameData = 0;
                    List<Guid> listPerdiemUpdated = new List<Guid>();

                    foreach (ClaimPerdiemRequestDTO obj in data.ClaimPerdiem)
                    {
                        ClaimPerdiemRequestDTO perdiemRequest = new ClaimPerdiemRequestDTO();
                        perdiemRequest.ClaimId = ClaimId;
                        perdiemRequest.MapFrom(obj);

                        ClaimPerdiemResponseDTO perdiemResponse = new ClaimPerdiemResponseDTO();
                        perdiemResponse.MapFrom(obj);



                        if ((data.Action.ToLower() == "submitted" || data.Action.ToLower() == "resubmitted" || data.Action.ToLower() == "draft" || (data.Action.ToLower() == "saved" && (data?.Page ?? "").ToLower() == "submission")))
                        {
                            var DataPerdiem2 = await claimperdiemSvc.GetById(obj.Id);

                            if (DataPerdiem2.Count > 0)
                            {
                                var datadb = DataPerdiem2.First();
                                perdiemResponse.ClaimId = ClaimId;
                                var datajs = perdiemResponse;

                                // Compare values directly
                                if ((datadb.B != datajs.B || datadb.L != datajs.L || datadb.D != datajs.D || datadb.I != datajs.I || datadb.F != datajs.F || datadb.CityId != datajs.CityId
                                    || datadb.CityName != datajs.CityName || datadb.CityOther != datajs.CityOther || datadb.CountryId != datajs.CountryId || datadb.CountryName != datajs.CountryName
                                    || datadb.TotalAmount != datajs.TotalAmount || datadb.TotalPerdiemRate != datajs.TotalPerdiemRate
                                    || datadb.DateFromText != datajs.DateFromText
                                    || datadb.DateToText != datajs.DateToText || datadb.IsActive != datajs.IsActive
                                    ))
                                {
                                    CountNotSameData = 1;
                                    listPerdiemUpdated.Add(obj.Id);
                                }
                            }
                            else
                            {
                                CountNotSameData = 1;
                                listPerdiemUpdated.Add(obj.Id);
                            }
                        }
                        else
                        {
                            var DataPerdiem = await claimperdiemSvc.GetById(obj.Id);
                            perdiemRequest.ClaimId = ClaimId;

                            if (DataPerdiem.Count > 0)
                            {
                                ClaimPerdiemId = await claimperdiemSvc.AddOrUpdate(perdiemRequest);
                            }
                            else
                            {
                                ClaimPerdiemId = await claimperdiemSvc.Add(perdiemRequest);
                            }
                        }
                    }

                    if ((data.Action.ToLower() == "submitted" || data.Action.ToLower() == "resubmitted" || data.Action.ToLower() == "draft" || (data.Action.ToLower() == "saved" && (data?.Page ?? "").ToLower() == "submission")))
                    {
                        if (CountNotSameData == 1)
                        {
                            var DataPerdiemClaimDetail = await claimperdiemdtlSvc.Get(x => (x.ClaimId == ClaimId && listPerdiemUpdated.Contains(x.ClaimPerdiemId)) || (x.ClaimId == ClaimId && x.IsFinance));
                            if (DataPerdiemClaimDetail.Count() > 0)
                            {
                                #region Delete Claim Perdiem Detail
                                foreach (var datadetail in DataPerdiemClaimDetail)
                                {
                                    //delete all
                                    var DataPerdiemClaimDetail2 = await claimperdiemdtlSvc.Get(x => x.Id == datadetail.Id);
                                    if (DataPerdiemClaimDetail2.Count() > 0)
                                    {
                                        ClaimPerdiemDetailRequestDTO objdetail = new ClaimPerdiemDetailRequestDTO();
                                        objdetail.Id = datadetail.Id;
                                        await claimperdiemdtlSvc.Delete(objdetail);
                                    }
                                }
                                #endregion

                            }
                            var DataPerdiemClaimDetailAfterDelete = await claimperdiemdtlSvc.Get(x => x.ClaimId == ClaimId && listPerdiemUpdated.Contains(x.ClaimPerdiemId));

                            if (DataPerdiemClaimDetailAfterDelete.Count() == 0)
                            {

                                List<ClaimPerdiemDetailRequestDTO> perdiemDetailRequests = new List<ClaimPerdiemDetailRequestDTO>();
                                foreach (ClaimPerdiemRequestDTO obj in data.ClaimPerdiem.Where(x => listPerdiemUpdated.Contains(x.Id)))
                                {


                                    ClaimPerdiemDetailRequestDTO perdiemDetailRequest = new ClaimPerdiemDetailRequestDTO();

                                    DateTime DateFrom = Convert.ToDateTime(obj.DateFrom);
                                    DateTime DateTo = Convert.ToDateTime(obj.DateTo);
                                    //int NumberOfDays = claimperdiemSvc.CalculateDaysDifference(DateFrom, DateTo);
                                    for (DateTime currentDate = DateFrom; currentDate <= DateTo; currentDate = currentDate.AddDays(1))
                                    {
                                        DateTime DatePerdiem = currentDate;
                                        string CountryId = obj.CountryId;
                                        string CityId = obj.CityId;


                                        var GetPerdiemRates = (await perdiemSvc.GetPerdiem(CountryId, CityId, DatePerdiem, DatePerdiem)).OrderByDescending(x => x.Year).FirstOrDefault();

                                        perdiemDetailRequest = new ClaimPerdiemDetailRequestDTO
                                        {
                                            Id = Guid.Empty,
                                            ClaimId = ClaimId,
                                            ClaimPerdiemId = obj.Id,
                                            Date = DatePerdiem,
                                            CountryId = obj.CountryId,
                                            CountryName = obj.CountryName,
                                            CityId = obj.CityId,
                                            CityName = obj.CityName,
                                            CityOther = obj.CityOther,
                                            PerdiemRate = Convert.ToDecimal(GetPerdiemRates?.FullPerdiem) == 0 ? 0 : Convert.ToDecimal(GetPerdiemRates?.FullPerdiem),
                                            B = obj.B,
                                            L = obj.L,
                                            D = obj.D,
                                            I = obj.I,
                                            F = obj.F,
                                            BFinance = obj.B,
                                            LFinance = obj.L,
                                            DFinance = obj.D,
                                            IFinance = obj.I,
                                            FFinance = obj.F,
                                            BAmount = Convert.ToBoolean(obj.B) ? GetPerdiemRates?.Breakfast ?? 0 : 0,
                                            LAmount = Convert.ToBoolean(obj.L) ? GetPerdiemRates?.Lunch ?? 0 : 0,
                                            DAmount = Convert.ToBoolean(obj.D) ? GetPerdiemRates?.Dinner ?? 0 : 0,
                                            IAmount = Convert.ToBoolean(obj.I) ? GetPerdiemRates?.Incidental ?? 0 : 0,
                                            Amount = Convert.ToBoolean(obj.F) ? GetPerdiemRates?.FullPerdiem ?? 0 : 0,
                                            BAmount0 = Convert.ToBoolean(obj.B) ? GetPerdiemRates?.BreakfastOriginal ?? 0 : 0,
                                            LAmount0 = Convert.ToBoolean(obj.L) ? GetPerdiemRates?.LunchOriginal ?? 0 : 0,
                                            DAmount0 = Convert.ToBoolean(obj.D) ? GetPerdiemRates?.DinnerOriginal ?? 0 : 0,
                                            IAmount0 = Convert.ToBoolean(obj.I) ? GetPerdiemRates?.IncidentalOriginal ?? 0 : 0,
                                            Amount0 = Convert.ToBoolean(obj.F) ? GetPerdiemRates?.FullPerdiemOriginal ?? 0 : 0,
                                            BFinanceAmount = Convert.ToBoolean(obj.B) ? GetPerdiemRates?.Breakfast ?? 0 : 0,
                                            LFinanceAmount = Convert.ToBoolean(obj.L) ? GetPerdiemRates?.Lunch ?? 0 : 0,
                                            DFinanceAmount = Convert.ToBoolean(obj.D) ? GetPerdiemRates?.Dinner ?? 0 : 0,
                                            IFinanceAmount = Convert.ToBoolean(obj.I) ? GetPerdiemRates?.Incidental ?? 0 : 0,
                                            AmountFinance = Convert.ToBoolean(obj.F) ? GetPerdiemRates?.FullPerdiem ?? 0 : 0,
                                            BFinanceAmount0 = Convert.ToBoolean(obj.B) ? GetPerdiemRates?.BreakfastOriginal ?? 0 : 0,
                                            LFinanceAmount0 = Convert.ToBoolean(obj.L) ? GetPerdiemRates?.LunchOriginal ?? 0 : 0,
                                            DFinanceAmount0 = Convert.ToBoolean(obj.D) ? GetPerdiemRates?.DinnerOriginal ?? 0 : 0,
                                            IFinanceAmount0 = Convert.ToBoolean(obj.I) ? GetPerdiemRates?.IncidentalOriginal ?? 0 : 0,
                                            AmountFinance0 = Convert.ToBoolean(obj.F) ? GetPerdiemRates?.FullPerdiemOriginal ?? 0 : 0,
                                            Currency = obj.Currency,

                                        };

                                        perdiemDetailRequests.Add(perdiemDetailRequest);
                                    }
                                }

                                #region Update, Add table Claim Perdiem 
                                foreach (ClaimPerdiemRequestDTO obj in data.ClaimPerdiem)
                                {
                                    var DataPerdiem = await claimperdiemSvc.GetById(obj.Id);
                                    ClaimPerdiemRequestDTO perdiemRequest = new ClaimPerdiemRequestDTO();
                                    perdiemRequest.MapFrom(obj);
                                    if (DataPerdiem.Count > 0)
                                    {
                                        perdiemRequest.ClaimId = ClaimId;
                                        ClaimPerdiemId = await claimperdiemSvc.AddOrUpdate(perdiemRequest);
                                    }
                                    else
                                    {
                                        perdiemRequest.ClaimId = ClaimId;
                                        ClaimPerdiemId = await claimperdiemSvc.Add(perdiemRequest);
                                    }

                                    #region Update, Add table Claim Perdiem Detail
                                    foreach (ClaimPerdiemDetailRequestDTO objdt in perdiemDetailRequests)
                                    {
                                        Guid newClaimPerdiemId = objdt.ClaimPerdiemId == Guid.Empty ? ClaimPerdiemId : objdt.ClaimPerdiemId;
                                        if (ClaimPerdiemId == newClaimPerdiemId)
                                        {
                                            ClaimPerdiemDetailRequestDTO perdiemDetailRequest = new ClaimPerdiemDetailRequestDTO();
                                            perdiemDetailRequest.MapFrom(objdt);
                                            perdiemDetailRequest.ClaimId = ClaimId;
                                            perdiemDetailRequest.ClaimPerdiemId = newClaimPerdiemId;
                                            perdiemDetailRequest.IsActive = obj.IsActive;
                                            await claimperdiemdtlSvc.AddOrUpdateOrDelete(perdiemDetailRequest);
                                        }
                                    }
                                    #endregion Update, Add table Claim Perdiem Detail
                                }
                                #endregion Update, Add table Claim Perdiem 


                            }



                        }
                    }


                    if ((data.Action.ToLower() != "submitted" && data.Action.ToLower() != "resubmitted" && data.Action.ToLower() != "draft" && (data.Action.ToLower() != "saved" || (data?.Page ?? "").ToLower() != "submission")))
                    {
                        foreach (ClaimPerdiemDetailRequestDTO objdt in data.ClaimPerdiemDetail)
                        {
                            ClaimPerdiemDetailRequestDTO perdiemDetailRequest = new ClaimPerdiemDetailRequestDTO();
                            perdiemDetailRequest.MapFrom(objdt);
                            var DataPerdiemDetail = await claimperdiemdtlSvc.GetById(perdiemDetailRequest.Id);
                            if (DataPerdiemDetail.Count > 0)
                            {
                                perdiemDetailRequest.ClaimId = ClaimId;
                                await claimperdiemdtlSvc.AddOrUpdateOrDelete(perdiemDetailRequest);
                            }
                            else
                            {
                                perdiemDetailRequest.ClaimId = ClaimId;
                                await claimperdiemdtlSvc.Add(perdiemDetailRequest);
                            }
                        }
                    }


                    //claim perdiem chargecode
                    foreach (ClaimPerdiemChargeCodeRequestDTO obj in data.ClaimPerdiemChargeCode)
                    {
                        ClaimPerdiemChargeCodeRequestDTO ccRequest = new ClaimPerdiemChargeCodeRequestDTO();
                        ccRequest.MapFrom(obj);
                        ccRequest.ClaimPerdiemId = ClaimPerdiemId;
                        ccRequest.ClaimId = ClaimId;
                        await ClaimPerdiemChargeCodeSvc.AddOrUpdateOrDelete(ccRequest);
                    }

                    //claim expanse
                    foreach (ClaimExpenseRequestDTO obj in data.ClaimExpense)
                    {
                        ClaimExpenseRequestDTO expenseRequest = new ClaimExpenseRequestDTO();
                        expenseRequest.MapFrom(obj);
                        var Data = await claimexpenseSvc.GetById(expenseRequest.Id);
                        if (Data.Count > 0)
                        {
                            expenseRequest.ClaimId = ClaimId;
                            expenseRequest.ClaimDocumentId = expenseRequest.ClaimDocumentId == null ? Guid.Empty : expenseRequest.ClaimDocumentId;
                            ClaimExpenseId = await claimexpenseSvc.AddOrUpdateOrDelete(expenseRequest);
                        }
                        else
                        {
                            expenseRequest.ClaimId = ClaimId;
                            expenseRequest.ClaimDocumentId = expenseRequest.ClaimDocumentId == null ? Guid.Empty : expenseRequest.ClaimDocumentId;
                            ClaimExpenseId = await claimexpenseSvc.Add(expenseRequest);
                        }
                    }

                    // claim expanse chargecode
                    foreach (ClaimExpenseChargeCodeRequestDTO objcc in data.ClaimExpenseChargeCode)
                    {
                        ClaimExpenseChargeCodeRequestDTO ccRequest = new ClaimExpenseChargeCodeRequestDTO();
                        ccRequest.MapFrom(objcc);
                        ccRequest.ClaimId = ClaimId;
                        //ccRequest.ClaimExpenseId = ClaimExpenseId;
                        bool DataExpenseIsActive = await ClaimExpenseChargeCodeSvc.CheckExpenseIsActive(ccRequest);
                        if (DataExpenseIsActive == false)
                            ccRequest.IsActive = DataExpenseIsActive;

                        await ClaimExpenseChargeCodeSvc.AddOrUpdateOrDelete(ccRequest);
                    }

                    //claim supporting document
                    foreach (ClaimSupportingDocumentRequestDTO objdt in data.ClaimSupportingDocument)
                    {
                        ClaimSupportingDocumentRequestDTO supdoc = new ClaimSupportingDocumentRequestDTO();
                        supdoc.MapFrom(objdt);
                        supdoc.ClaimId = ClaimId;
                        //var DataSupDoc = await claimsupportingdocumentSvc.GetById(supdoc.Id);
                        await claimsupportingdocumentSvc.AddOrUpdateOrDelete(supdoc);
                    }

                    //claim boarding pass document
                    foreach (ClaimBoardingPassDocumentRequestDTO objdt in data.ClaimBoardingPassDocument)
                    {
                        ClaimBoardingPassDocumentRequestDTO boardpassdoc = new ClaimBoardingPassDocumentRequestDTO();
                        boardpassdoc.MapFrom(objdt);
                        boardpassdoc.ClaimId = ClaimId;
                        //var DataSupDoc = await claimsupportingdocumentSvc.GetById(supdoc.Id);
                        await claimboardingpassdocumentSvc.AddOrUpdateOrDelete(boardpassdoc);
                    }

                    //claim journal
                    if (data.Page.ToUpper() != "APPROVAL")
                    {
                        await claimJournalSvc.Delete(Guid.Empty, x => x.ClaimId == claimRequest.Id);
                    }
                    foreach (ClaimJournalRequestDTO obj in data.ClaimJournal)
                    {
                        obj.ClaimId = ClaimId;
                        await claimJournalSvc.Add(obj);
                    }

                }
                else
                {
                    //convert amount to tbl claim
                    claimRequest.TotalPerdiemClaim = 0;
                    claimRequest.TotalExpenseClaim = 0;
                    claimRequest.TotalTEC = 0;
                    claimRequest.AdvanceAmount = Convert.ToDecimal(data.AdvanceAmount);


                    if (DataClaim.Count() > 0)
                    {
                        claimRequest.MapFrom(claimRequest);
                        claimRequest.StatusId = data.Action.ToLower() == "submitted" ? "APPROVED" : "DRAFT";
                        claimRequest.ClaimConditionId = data.ClaimConditionId;
                        await claimSvc.Update(claimRequest);
                        ClaimId = claimRequest.Id;
                    }
                    else //for submit and save
                    {
                        claimRequest.Id = claimRequest.Id;
                        claimRequest.StatusId = data.Action.ToLower() == "submitted" ? "APPROVED" : "DRAFT";
                        claimRequest.ClaimConditionId = Guid.Empty;
                        ClaimId = await claimSvc.Add(claimRequest);
                    }

                    var claimData = await claimSvc.Get(x => x.Id.Equals(ClaimId));

                    if (!string.IsNullOrEmpty(claimData.FirstOrDefault()?.SystemCode))
                    {
                        newSystemCode = await claimSvc.GetSystemCode(claimData.FirstOrDefault()?.SystemCode, x => x.IsActive == true && !string.IsNullOrEmpty(x.SystemCode));
                    }
                    else
                    {
                        newSystemCode = await claimSvc.GetSystemCode(newSystemCode, x => x.IsActive == true);
                    }

                    var claimResponseDtoSingle = claimData.FirstOrDefault();
                    ClaimRequestDTO mainReqDTO = new ClaimRequestDTO();
                    mainReqDTO.MapFrom(claimResponseDtoSingle);
                    mainReqDTO.SystemCode = newSystemCode;
                    await claimSvc.Update(mainReqDTO);

                    //claim perdiem set isactive = 0
                    #region Perdiem Claim
                    var DataPerdiemClaim = await claimperdiemSvc.GetByClaimId(ClaimId);
                    if (DataPerdiemClaim.Count() > 0)
                    {
                        foreach (var obj in DataPerdiemClaim)
                        {
                            await claimperdiemSvc.UpdateIsActive(obj.Id);
                        }
                    }
                    #endregion

                    //claim perdiem charge code set isactive = 0
                    #region Perdiem Charge Code
                    var DataPerdiemClaimCC = await ClaimPerdiemChargeCodeSvc.GetByClaimId(ClaimId);
                    if (DataPerdiemClaimCC.Count() > 0)
                    {
                        foreach (var obj in DataPerdiemClaimCC)
                        {
                            await ClaimPerdiemChargeCodeSvc.UpdateIsActive(obj.Id);
                        }
                    }
                    #endregion

                    //claim expense set isactive = 0
                    #region Expense
                    var DataExpense = await claimexpenseSvc.GetByClaimId(ClaimId);
                    if (DataExpense.Count() > 0)
                    {
                        foreach (var obj in DataExpense)
                        {
                            await claimexpenseSvc.UpdateIsActive(obj.Id);
                        }
                    }
                    #endregion

                    //claim expense charge code set isactive = 0
                    #region Expense CC
                    var DataExpenseCC = await ClaimExpenseChargeCodeSvc.GetByClaimId(ClaimId);
                    if (DataExpenseCC.Count() > 0)
                    {
                        foreach (var obj in DataExpenseCC)
                        {
                            await ClaimExpenseChargeCodeSvc.UpdateIsActive(obj.Id);
                        }
                    }
                    #endregion

                    //claim supporting document
                    foreach (ClaimSupportingDocumentRequestDTO objdt in data.ClaimSupportingDocument)
                    {
                        ClaimSupportingDocumentRequestDTO supdoc = new ClaimSupportingDocumentRequestDTO();
                        supdoc.MapFrom(objdt);
                        supdoc.ClaimId = ClaimId;
                        //var DataSupDoc = await claimsupportingdocumentSvc.GetById(supdoc.Id);
                        await claimsupportingdocumentSvc.AddOrUpdateOrDelete(supdoc);
                    }

                    //claim boarding pass document
                    foreach (ClaimBoardingPassDocumentRequestDTO objdt in data.ClaimBoardingPassDocument)
                    {
                        ClaimBoardingPassDocumentRequestDTO boardpassdoc = new ClaimBoardingPassDocumentRequestDTO();
                        boardpassdoc.MapFrom(objdt);
                        boardpassdoc.ClaimId = ClaimId;
                        //var DataSupDoc = await claimsupportingdocumentSvc.GetById(supdoc.Id);
                        await claimboardingpassdocumentSvc.AddOrUpdateOrDelete(boardpassdoc);
                    }

                    //claim recent comment
                    #region Recent Comment
                    var latestRecenCommentObj = await claimcommentSvc.Get(x => x.ClaimId.Equals(data.Id));
                    if (latestRecenCommentObj.Count() > 0)
                    {
                        var latestRecentComm = latestRecenCommentObj.OrderByDescending(x => x.Date).FirstOrDefault();
                        if (latestRecentComm.ActionTaken.ToUpper() == "DRAFT" && data.Action.ToUpper() == "DRAFT")
                        {
                            ClaimCommentRequestDTO claimcomment = new ClaimCommentRequestDTO();
                            claimcomment.Id = latestRecentComm.Id;
                            claimcomment.Name = data.UserId ?? "CIFOR-ICRAF SYSTEM";
                            claimcomment.ClaimId = ClaimId;
                            claimcomment.Date = DateTime.Now;
                            claimcomment.ActionTaken = "DRAFT";
                            claimcomment.Role = "";
                            claimcomment.DocumentId = Guid.Empty;
                            claimcomment.Comment = data.ClaimComment.Select(x => x.Comment).FirstOrDefault();
                            await claimcommentSvc.Update(claimcomment);
                        }
                        else
                        {
                            ClaimCommentRequestDTO claimcomment = new ClaimCommentRequestDTO();
                            claimcomment.Id = Guid.NewGuid();
                            claimcomment.Name = data.UserId ?? "CIFOR-ICRAF SYSTEM";
                            claimcomment.ClaimId = ClaimId;
                            claimcomment.Date = DateTime.Now;
                            claimcomment.ActionTaken = "APPROVED";
                            claimcomment.Role = "";
                            claimcomment.DocumentId = Guid.Empty;
                            claimcomment.Comment = data.ClaimComment.Select(x => x.Comment).FirstOrDefault();
                            await claimcommentSvc.Add(claimcomment);
                            if (data.Action.ToLower() == "submitted")
                            {
                                await notificationSvc.SendClaimNotification(data.TAId, data.Id, accessToken, data.Action, data.UserId, data.Role, data.Page, data.ClaimComment);
                                await travelauthorizationSvc.UpdateAlreadyHaveExpensePerdiemClaim(data.TAId, data.Action.ToUpper());
                            }
                        }
                    }
                    else
                    {
                        ClaimCommentRequestDTO claimcomment = new ClaimCommentRequestDTO();
                        claimcomment.Id = Guid.NewGuid();
                        claimcomment.Name = data.UserId ?? "CIFOR-ICRAF SYSTEM";
                        claimcomment.ClaimId = ClaimId;
                        claimcomment.Date = DateTime.Now;
                        claimcomment.ActionTaken = data.Action.ToLower() == "submitted" ? "APPROVED" : "DRAFT";
                        claimcomment.Role = "";
                        claimcomment.DocumentId = Guid.Empty;
                        claimcomment.Comment = data.ClaimComment.Select(x => x.Comment).FirstOrDefault();
                        await claimcommentSvc.Add(claimcomment);
                        if (data.Action.ToLower() == "submitted")
                        {
                            await notificationSvc.SendClaimNotification(data.TAId, data.Id, accessToken, data.Action, data.UserId, data.Role, data.Page, data.ClaimComment);
                            await travelauthorizationSvc.UpdateAlreadyHaveExpensePerdiemClaim(data.TAId, data.Action.ToUpper());
                        }
                    }

                    #endregion

                }
                ret.ClaimId = ClaimId;
                ret.TAId = claimRequest.TAId;
                ret.SystemCode = newSystemCode == "" ? claimRequest.SystemCode : newSystemCode;


                transaction.Commit();
                return ret;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<SubmissionResponseDTO> SubmitWithoutClaim(SubmissionRequestDTO data, string userId, string accessToken)
        {
            using var transaction = context.Database.BeginTransaction();

            try
            {
                SubmissionResponseDTO ret = new SubmissionResponseDTO();
                string nextRequestId = "";

                //DECLARE
                var ClaimPerdiemId = Guid.Empty;
                var ClaimExpenseId = Guid.Empty;
                var ClaimId = Guid.Empty;
                var ClaimComment = Guid.Empty;

                ClaimRequestDTO claimRequest = new ClaimRequestDTO();
                //claimRequest.Id = Guid.NewGuid();
                claimRequest.MapFrom(data);
                var DataClaim = await claimSvc.GetById(claimRequest.Id);

                if (data.Action.ToLower() == "request for revision" || data.Action.ToLower() == "rejected" || data.Action.ToLower() == "approved" || data.Action.ToLower() == "draft" || data.Action.ToLower() == "submitted" || data.Action.ToLower() == "resubmitted" || data.Action.ToLower() == "redirected")
                {
                    //convert amount to tbl claim
                    claimRequest.TotalPerdiemClaim = Convert.ToDecimal(data.TotalPerdiemClaim);
                    claimRequest.TotalExpenseClaim = Convert.ToDecimal(data.TotalExpenseClaim);
                    claimRequest.TotalTEC = Convert.ToDecimal(data.TotalTEC);
                    claimRequest.AdvanceAmount = Convert.ToDecimal(data.AdvanceAmount);

                    if (DataClaim.Count() > 0)
                    {
                        claimRequest.MapFrom(claimRequest);
                        claimRequest.StatusId = "APPROVED";
                        claimRequest.ClaimConditionId = data.ClaimConditionId;
                        await claimSvc.Update(claimRequest);
                        ClaimId = claimRequest.Id;
                    }
                    else //for submit and save
                    {
                        claimRequest.Id = claimRequest.Id;
                        claimRequest.ClaimConditionId = Guid.Empty;
                        ClaimId = await claimSvc.Add(claimRequest);
                    }

                    //claim supporting document
                    foreach (ClaimSupportingDocumentRequestDTO objdt in data.ClaimSupportingDocument)
                    {
                        ClaimSupportingDocumentRequestDTO supdoc = new ClaimSupportingDocumentRequestDTO();
                        supdoc.MapFrom(objdt);
                        supdoc.ClaimId = ClaimId;
                        //var DataSupDoc = await claimsupportingdocumentSvc.GetById(supdoc.Id);
                        await claimsupportingdocumentSvc.AddOrUpdateOrDelete(supdoc);
                    }

                    //claim supporting document
                    foreach (ClaimBoardingPassDocumentRequestDTO objdt in data.ClaimBoardingPassDocument)
                    {
                        ClaimBoardingPassDocumentRequestDTO boardpassdoc = new ClaimBoardingPassDocumentRequestDTO();
                        boardpassdoc.MapFrom(objdt);
                        boardpassdoc.ClaimId = ClaimId;
                        //var DataSupDoc = await claimsupportingdocumentSvc.GetById(supdoc.Id);
                        await claimboardingpassdocumentSvc.AddOrUpdateOrDelete(boardpassdoc);
                    }

                    //claim recent comment
                    #region Recent Comment
                    ClaimCommentRequestDTO claimcomment = new ClaimCommentRequestDTO();
                    claimcomment.Id = Guid.NewGuid();
                    claimcomment.Name = data.UserId ?? "CIFOR-ICRAF SYSTEM";
                    claimcomment.ClaimId = ClaimId;
                    claimcomment.Date = DateTime.Now;
                    claimcomment.ActionTaken = "APPROVED";
                    claimcomment.Role = "";
                    claimcomment.DocumentId = Guid.Empty;
                    await claimcommentSvc.Add(claimcomment);
                    #endregion

                }

                ret.ClaimId = ClaimId;
                ret.TAId = claimRequest.TAId;


                transaction.Commit();
                return ret;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<SubmissionResponseDTO> SubmitClaimWithWorkflow(SubmissionRequestDTO data, string userId, string accessToken)
        {
            try
            {
                SubmissionResponseDTO ret = new SubmissionResponseDTO();
                ret = await SubmitClaim(data, userId, accessToken);

                if (data.IsHaveClaim == true)
                {
                    bool isWfActive = false;

                    bool isWfExist = false;

                    //Check WF is exist for submission
                    if (data.ActionName.ToUpper().Equals(WorkflowCommand.Submit))
                    {
                        var wfExistValidatorReqDto = new WorkflowExistRequestDTO();
                        wfExistValidatorReqDto.RequestId = data.Id;
                        var wfistValidator = await k2Svc.WorkflowExistValidator(wfExistValidatorReqDto);
                        if (wfistValidator.IsWorkflowExist)
                        {

                            isWfExist = wfistValidator.IsWorkflowExist;
                        }

                        if (isWfExist)
                        {
                            throw new Exception("Claim with id: " + data.Id.ToString() + " has already submitted to workflow. Please re-check or choose other claim.");
                        }
                        log.LogInformation($"Result check workflow exist with Id:{wfExistValidatorReqDto.RequestId} and Result is {wfistValidator.IsWorkflowExist}");
                    }

                    //Check SN is Valid for resubmission and approval
                    if (!string.IsNullOrEmpty(data.ActId) || !string.IsNullOrEmpty(data.Sn))
                    {
                        //Skenario Resubmission dan Approval maka harus cek SN terlebih dahulu untuk melakukan action apapun
                        var snValidatorReq = new SnValidatorRequestDTO();
                        snValidatorReq.Id = data.Id.ToString();
                        snValidatorReq.Folder = Configuration["K2Workflow:Folder"];
                        snValidatorReq.ProcessName = Configuration["K2Workflow:ProcessName"];
                        snValidatorReq.UserId = data.UserId;
                        snValidatorReq.Sn = data.Sn;
                        var snValidator = await k2Svc.SnValidator(snValidatorReq);

                        if (snValidator != null)
                        {
                            if (!snValidator.IsSnValid)
                            {
                                throw new Exception("This task for request id: " + data.Id.ToString() + " has already taken. Please choose another task.");
                            }
                        }
                    }



                    if (Configuration["K2Workflow:IsWorkflowActive"] == "1")
                    {
                        isWfActive = true;
                    }

                    if (isWfActive)
                    {
                        ExecuteWorkflowRequestDTO dataWftoSend = new ExecuteWorkflowRequestDTO();
                        dataWftoSend.Command = data.ActionName;
                        dataWftoSend.Id = data.Id;
                        dataWftoSend.Username = data.UserId;
                        dataWftoSend.Sn = data.Sn;
                        dataWftoSend.ActivityId = data.ActId;
                        dataWftoSend.Status = data.StatusId;
                        dataWftoSend.ActionId = data.Action;
                        dataWftoSend.Comment = data.Comment;
                        dataWftoSend.Role = data.Role;

                        var wfdata = await ExecuteWorkflow(dataWftoSend);
                        //await k2Svc.K2Sync(data.RequestId.ToString(), Configuration["K2Workflow:ProcessName"], Configuration["K2Workflow:Folder"]);
                        //ret.SystemCode = wfdata.data.errorMsg + " (1);" + wfdata.additionalInfo + " ; " + wfdata.status + " ; " + wfdata.message;

                        //updated to TravelAuthorization
                        if (data.Action.ToUpper() == "SUBMITTED")
                        {
                            await travelauthorizationSvc.UpdateAlreadyHaveExpensePerdiemClaim(data.TAId, data.Action.ToUpper());
                        }

                        //send notification claim
                        if (data.Action.ToUpper() == "APPROVED")
                        {
                            if (data.StatusId.ToUpper() == "APPROVED")
                            {
                                await travelauthorizationextendedSvc.UpdateTravelAuthorizationExtended(data.TAId, data.Action.ToUpper());
                                await notificationSvc.SendClaimNotification(ret.TAId, ret.ClaimId, accessToken, data.Action, data.UserId, data.Role, data.Page, data.ClaimComment);
                                var claimData = await claimSvc.Get(x => x.Id.Equals(data.Id));
                                OCSMFLQueueRequestDTO ocsMFLRequest = new OCSMFLQueueRequestDTO();
                                ocsMFLRequest.Id = Guid.NewGuid();
                                ocsMFLRequest.SourceName = "Claim";
                                ocsMFLRequest.SourceId = data.Id.ToString();
                                ocsMFLRequest.UserId = data.UserId;
                                ocsMFLRequest.Time = DateTime.Now;
                                ocsMFLRequest.DueTime = DateTime.Now.AddMinutes(60);
                                ocsMFLRequest.Status = "Submitted";
                                ocsMFLRequest.Remark = string.Empty;
                                ocsMFLRequest.MyTreeReferenceNo = claimData.FirstOrDefault()?.SystemCode;

                                await ocsMFLQueueSvc.Add(ocsMFLRequest);
                            }
                        }
                        else if (data.Action.ToUpper() == "REJECTED")
                        {
                            await notificationSvc.SendClaimNotification(ret.TAId, ret.ClaimId, accessToken, data.Action, data.UserId, data.Role, data.Page, data.ClaimComment);
                            await travelauthorizationSvc.UpdateAlreadyHaveExpensePerdiemClaim(data.TAId, data.Action.ToUpper());
                        }
                    }
                }



                return ret;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> ExecuteWorkflow(ExecuteWorkflowRequestDTO execWfRequestDTO)
        {
            using var transaction = context.Database.BeginTransaction();

            var command = execWfRequestDTO.Command;
            Guid id = execWfRequestDTO.Id;
            var username = execWfRequestDTO.Username;
            var sn = execWfRequestDTO.Sn;
            var activityId = execWfRequestDTO.ActivityId;
            var statusid = execWfRequestDTO.Status;
            var actionid = execWfRequestDTO.ActionId;
            string newSystemCode = string.Empty;

            try
            {
                APIResponse<Response> ret = new APIResponse<Response>();
                var claimData = await claimSvc.Get(x => x.Id.Equals(id));
                if (!string.IsNullOrEmpty(claimData.FirstOrDefault()?.SystemCode))
                {
                    newSystemCode = await claimSvc.GetSystemCode(claimData.FirstOrDefault()?.SystemCode, x => x.IsActive == true && !string.IsNullOrEmpty(x.SystemCode));
                }
                else
                {
                    newSystemCode = await claimSvc.GetSystemCode(newSystemCode, x => x.IsActive == true);
                }


                var allDataClaim = await claimDataSvc.GetAllData("null", id, "");
                allDataClaim.SystemCode = newSystemCode;
                if (command.ToUpper() == (WorkflowCommand.Submit) && string.IsNullOrEmpty(activityId))
                {

                    #region Recent Comment Submitted
                    ClaimCommentRequestDTO claimcomment = new ClaimCommentRequestDTO();
                    claimcomment.Id = Guid.NewGuid();
                    claimcomment.ActivityId = 0;
                    claimcomment.Name = execWfRequestDTO.Username ?? "CIFOR-ICRAF SYSTEM";
                    claimcomment.ClaimId = id;
                    claimcomment.Date = DateTime.Now;
                    claimcomment.ActionTaken = actionid;
                    claimcomment.Role = execWfRequestDTO.Role;
                    claimcomment.Comment = execWfRequestDTO.Comment;
                    claimcomment.DocumentId = Guid.Empty;
                    await claimcommentSvc.Add(claimcomment);
                    #endregion

                    #region Update WF Status
                    //Update Status

                    if (claimData.Count() > 0)
                    {
                        var claimResponseDtoSingle = claimData.FirstOrDefault();
                        ClaimRequestDTO mainReqDTO = new ClaimRequestDTO();
                        mainReqDTO.MapFrom(claimResponseDtoSingle);
                        mainReqDTO.SystemCode = newSystemCode;
                        mainReqDTO.StatusId = statusid;

                        await claimSvc.Update(mainReqDTO);
                    }
                    #endregion

                    #region Submit Data and Hit K2 Submit

                    if (allDataClaim is not null)
                    {
                        //Save datalog list
                        await claimK2Svc.SaveClaimDataLogList(allDataClaim);

                        //Get participant and datafield
                        Dictionary<string, object> dataToSend = await claimK2Svc.GetK2Datafield(allDataClaim);
                        var dataRequest = new K2SubmitRequestDTO();
                        dataRequest.ID = id.ToString();
                        dataRequest.UserName = username;
                        dataRequest.Data = dataToSend;

                        //Send to K2
                        ret = await k2Svc.Submit(dataRequest);

                        //Save all participant to Integrated Portal
                        await claimK2Svc.SaveK2ActivityUserList(allDataClaim);
                        await claimK2Svc.SaveApproveState(id.ToString(), username, "0");
                    }
                    #endregion
                }
                else
                {
                    List<string> snListApvAction = new List<string> { sn };

                    var dataRequestApvAction = new K2ApprovalActionRequestDTO();
                    dataRequestApvAction.Username = username;
                    dataRequestApvAction.SN = snListApvAction;

                    List<string> listApprovalAction = new List<string> {
                        WorkflowCommand.Approve,
                        WorkflowCommand.Verify,
                        WorkflowCommand.Recommend,
                        WorkflowCommand.Upload,
                        WorkflowCommand.Resubmit,
                        WorkflowCommand.Save,
                        WorkflowCommand.Redirect,
                        WorkflowCommand.Revise,
                        WorkflowCommand.Reject,
                    };

                    #region Recent Comment and Update Status
                    //Save Recent Comment and Update Status
                    if (listApprovalAction.Contains(command.ToUpper()))
                    {
                        if (command.ToUpper() == WorkflowCommand.Save)
                        {
                            #region Recent Comment Save/Draft
                            var latestRecenCommentObj = await claimcommentSvc.Get(x => x.ClaimId.Equals(id));
                            if (latestRecenCommentObj.Count() > 0)
                            {
                                var latestRecentComm = latestRecenCommentObj.OrderByDescending(x => x.Date).FirstOrDefault();
                                if ((latestRecentComm.ActionTaken == WorkflowStatus.Draft && activityId == "") || (latestRecentComm.ActionTaken == WorkflowStatus.Saved && activityId != ""))
                                {
                                    ClaimCommentRequestDTO claimCommentRequest = new ClaimCommentRequestDTO();
                                    claimCommentRequest.MapFrom(latestRecentComm);
                                    if (activityId != "") claimCommentRequest.ActivityId = Convert.ToInt32(activityId);
                                    claimCommentRequest.Comment = execWfRequestDTO.Comment;
                                    claimCommentRequest.Role = execWfRequestDTO.Role;
                                    claimCommentRequest.ClaimId = id;
                                    claimCommentRequest.ActionTaken = (latestRecentComm.ActionTaken == WorkflowStatus.Draft && activityId == "") ? WorkflowStatus.Draft : WorkflowStatus.Saved;
                                    claimCommentRequest.Date = DateTime.Now;
                                    claimCommentRequest.Name = username;
                                    claimCommentRequest.DocumentId = Guid.Empty;
                                    var resCtrCommentequest = await claimcommentSvc.Update(claimCommentRequest);

                                    //Update Status

                                    if (claimData.Count() > 0)
                                    {
                                        var mainResponseDtoSingle = claimData.FirstOrDefault();
                                        ClaimRequestDTO mainReqDTO = new ClaimRequestDTO();
                                        mainReqDTO.MapFrom(mainResponseDtoSingle);
                                        mainReqDTO.SystemCode = newSystemCode;
                                        mainReqDTO.StatusId = (latestRecentComm.ActionTaken == WorkflowStatus.Draft && activityId == "") ? WorkflowStatus.Draft : statusid.ToUpper();
                                        await claimSvc.Update(mainReqDTO);
                                    }
                                }
                                else
                                {
                                    ClaimCommentRequestDTO claimCommentRequest = new ClaimCommentRequestDTO();
                                    if (activityId != "") claimCommentRequest.ActivityId = Convert.ToInt32(activityId);
                                    claimCommentRequest.Comment = execWfRequestDTO.Comment;
                                    claimCommentRequest.Role = execWfRequestDTO.Role;
                                    claimCommentRequest.ClaimId = id;
                                    claimCommentRequest.ActionTaken = (command.ToUpper() == WorkflowCommand.Save && activityId != "") ? WorkflowStatus.Saved : WorkflowStatus.Draft;
                                    claimCommentRequest.Date = DateTime.Now;
                                    claimCommentRequest.Name = username;
                                    claimCommentRequest.DocumentId = Guid.Empty;
                                    var resCtrCommentequest = await claimcommentSvc.Add(claimCommentRequest);

                                    if (claimData.Count() > 0)
                                    {
                                        var mainResponseDtoSingle = claimData.FirstOrDefault();
                                        ClaimRequestDTO mainReqDTO = new ClaimRequestDTO();
                                        mainReqDTO.MapFrom(mainResponseDtoSingle);
                                        mainReqDTO.SystemCode = newSystemCode;
                                        mainReqDTO.StatusId = statusid.ToUpper();
                                        await claimSvc.Update(mainReqDTO);
                                    }
                                }
                            }
                            else
                            {
                                ClaimCommentRequestDTO claimCommentRequest = new ClaimCommentRequestDTO();
                                if (activityId != "") claimCommentRequest.ActivityId = Convert.ToInt32(activityId);
                                claimCommentRequest.Comment = execWfRequestDTO.Comment;
                                claimCommentRequest.Role = execWfRequestDTO.Role;
                                claimCommentRequest.ClaimId = id;
                                claimCommentRequest.ActionTaken = actionid.ToUpper();//command.ToUpper() == WorkflowCommand.Save ? WorkflowStatus.Draft : command.ToUpper();
                                claimCommentRequest.Date = DateTime.Now;
                                claimCommentRequest.Name = username;
                                claimCommentRequest.DocumentId = Guid.Empty;
                                var resCtrCommentequest = await claimcommentSvc.Add(claimCommentRequest);

                                if (claimData.Count() > 0)
                                {
                                    var mainResponseDtoSingle = claimData.FirstOrDefault();
                                    ClaimRequestDTO mainReqDTO = new ClaimRequestDTO();
                                    mainReqDTO.MapFrom(mainResponseDtoSingle);
                                    mainReqDTO.SystemCode = newSystemCode;
                                    mainReqDTO.StatusId = WorkflowStatus.Draft;
                                    await claimSvc.Update(mainReqDTO);
                                }

                            }
                            #endregion

                        }
                        else
                        {

                            #region Recent Comment Approve, Resubmit, Redirect
                            //Save recent Comments
                            ClaimCommentRequestDTO claimCommentRequest = new ClaimCommentRequestDTO();
                            claimCommentRequest.Comment = execWfRequestDTO.Comment;
                            claimCommentRequest.ActivityId = Convert.ToInt32(activityId);
                            claimCommentRequest.Sn = sn;
                            claimCommentRequest.Role = execWfRequestDTO.Role;
                            claimCommentRequest.ClaimId = id;
                            claimCommentRequest.ActionTaken = actionid.ToUpper();//command.ToUpper() == WorkflowCommand.Save ? WorkflowStatus.Draft : command.ToUpper();
                            claimCommentRequest.Date = DateTime.Now;
                            claimCommentRequest.Name = username;
                            claimCommentRequest.DocumentId = Guid.Empty;
                            var resCtrCommentequest = await claimcommentSvc.Add(claimCommentRequest);
                            #endregion

                            #region Check WF Status

                            //check next activity to determine WF Status
                            var totalTEC = allDataClaim.TotalTEC;
                            var userListObj = await claimK2Svc.GetK2UserForIntegratedPortal(allDataClaim);
                            var involvingActId = userListObj.OrderBy(x => activityId).Select(x => x.ActivityID).ToList();
                            involvingActId = involvingActId.Distinct().ToList();
                            var nextActivity = involvingActId.Where(x => x > Convert.ToInt32(activityId)).OrderBy(x => x).FirstOrDefault();
                            var nextWfStatus = string.Empty;

                            List<int> listOfApprovalActivities = new List<int>() { 2, 3 };

                            if (Convert.ToInt32(activityId) == 2 && command.ToUpper() == "APPROVE")
                            {
                                nextWfStatus = WorkflowStatus.Approved;
                            }
                            else if (Convert.ToInt32(activityId) == 3 && command.ToUpper() == "APPROVE")
                            {
                                nextWfStatus = WorkflowStatus.WaitingForApproval;
                            }
                            else if (Convert.ToInt32(activityId) == 2 && command.ToUpper() == "REDIRECT")
                            {
                                nextWfStatus = WorkflowStatus.WaitingForApproval;
                            }
                            else if (listOfApprovalActivities.Contains(Convert.ToInt32(nextActivity)) && activityId != "1")
                            {
                                nextWfStatus = WorkflowStatus.WaitingForApproval;
                            }
                            else if (command.ToUpper() == "REVISE")
                            {
                                nextWfStatus = WorkflowStatus.Revised;
                            }
                            else if (command.ToUpper() == "REJECT")
                            {
                                nextWfStatus = WorkflowStatus.Rejected;
                            }
                            else
                                nextWfStatus = WorkflowStatus.Resubmitted;
                            #endregion

                            #region Update WF Status
                            //Update Status
                            var mainResponseDtoObj = await claimSvc.Get(x => x.Id.Equals(id));
                            if (mainResponseDtoObj.Count() > 0)
                            {
                                var mainResponseDtoSingle = mainResponseDtoObj.FirstOrDefault();
                                ClaimRequestDTO mainReqDTO = new ClaimRequestDTO();
                                mainReqDTO.MapFrom(mainResponseDtoSingle);
                                mainReqDTO.StatusId = nextWfStatus;
                                mainReqDTO.SystemCode = newSystemCode;
                                await claimSvc.Update(mainReqDTO);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region Hit K2

                    if (command.ToUpper() == (WorkflowCommand.Resubmit))
                    {
                        //var allDataClaim = await claimDataSvc.GetAllData("null", id, "");
                        if (allDataClaim is not null)
                        {
                            //Save datalog list
                            await claimK2Svc.SaveClaimDataLogList(allDataClaim);

                            //Perform CompareLog
                            await claimK2Svc.CompareLog(id.ToString(), false);

                            //Get participant and datafield
                            //SetApprover State 0
                            var dtApproveState = await approveStateSvc.Get(x => x.RelevantId.Equals(id.ToString()) && x.ActivityId == "2");
                            foreach (var obj in dtApproveState)
                            {
                                await approveStateSvc.Delete(obj.Id);
                                Log.Information("Update State 0");
                            }
                            var dtApproveState2 = await approveStateSvc.Get(x => x.RelevantId.Equals(id.ToString()) && x.ActivityId == "0");
                            foreach (var obj in dtApproveState2)
                            {
                                await approveStateSvc.UpdateState(obj.Id);
                            }
                            Dictionary<string, object> dataToSendResubmit = await claimK2Svc.GetK2Datafield(allDataClaim);
                            dataRequestApvAction.Data = dataToSendResubmit;
                            //Send to K2
                            ret = await k2Svc.Resubmit(dataRequestApvAction);

                            //Save all participant to Integrated Portal
                            await claimK2Svc.SaveK2ActivityUserList(allDataClaim);
                            await claimK2Svc.SaveApproveState(id.ToString(), username, "1");

                        }
                    }
                    else if (command.ToUpper() == (WorkflowCommand.Revise))
                    {
                        ret = await k2Svc.Revise(dataRequestApvAction);
                    }
                    else if (command.ToUpper() == (WorkflowCommand.Reject))
                    {
                        ret = await k2Svc.Reject(dataRequestApvAction);
                        await claimK2Svc.CompareLog(id.ToString(), true);
                    }
                    else if (command.ToUpper() == (WorkflowCommand.Approve))
                    {
                        if (activityId == "3")
                        {
                            ret = await k2Svc.UpdateProcessData(allDataClaim, execWfRequestDTO.Sn, execWfRequestDTO.ActivityId);


                            string configSpecialWorkorder = config["SpecialWorkorder"];
                            string configSpecialCostCenter = config["SpecialCostCenter"];
                            string configSpecialBudgetHolderUserId = config["SpecialBudgetHolderUserId"];
                            string configSpecialCostCenterUserId = config["SpecialCostCenterUserId"];
                            int configDelayWFAction = Convert.ToInt32(config["DelayWFAction"]);
                            bool ActiveSpecialBudgetHolder = Convert.ToBoolean(config["ActiveSpecialBudgetHolder"]);
                            bool isNotSpecial = false;
                            bool isSpecial = false;
                            /*Active untuk budget holder special case*/
                            if (ActiveSpecialBudgetHolder)
                            {
                                var dataPerdiemCC = await ClaimPerdiemChargeCodeSvc.Get(p => p.ClaimId == id && p.CostCenterId == configSpecialCostCenter);
                                var dataExpenseCC = await ClaimExpenseChargeCodeSvc.Get(p => p.ClaimId == id && p.CostCenterId == configSpecialCostCenter);
                                List<string> listSpecialBudgetHolderUserId = new List<string>();
                                List<string> listSpecialWorkorder = new List<string>();

                                if (!string.IsNullOrEmpty(configSpecialWorkorder))
                                {
                                    listSpecialWorkorder = configSpecialWorkorder.Split(";").ToList();
                                }
                                if (!string.IsNullOrEmpty(configSpecialBudgetHolderUserId))
                                {
                                    listSpecialBudgetHolderUserId = configSpecialBudgetHolderUserId.Split(";").ToList();
                                }

                                if (dataPerdiemCC.Count() > 0)
                                {
                                    foreach (var obj in dataPerdiemCC)
                                    {
                                        if (obj.CostCenterId == configSpecialCostCenter)
                                        {
                                            if (listSpecialWorkorder.Contains(obj.WorkOrderId))
                                            {
                                                isSpecial = true;
                                            }
                                            else
                                            {
                                                isNotSpecial = true;
                                            }
                                        }
                                    }
                                }

                                if (dataExpenseCC.Count() > 0)
                                {
                                    foreach (var obj in dataExpenseCC)
                                    {
                                        if (obj.CostCenterId == configSpecialCostCenter)
                                        {
                                            if (listSpecialWorkorder.Contains(obj.WorkOrderId))
                                            {
                                                isSpecial = true;
                                            }
                                            else
                                            {
                                                isNotSpecial = true;
                                            }
                                        }
                                    }
                                }

                                #region UserTakeAction BBIDDER, AWITZENBURG 
                                /*case 1 : (1 WO Special atau 3 WO (1 special, 1 not special, 1 cc selain 1104) take action by bbidder
                                  case 2 : 1 WO special atau (3 WO (1 special, 1 not special, 1 cc selain 1104) take action awitzenburg)*/
                                if ((isSpecial == true && isNotSpecial == true && username.ToUpper().Contains(configSpecialCostCenterUserId)) ||
                                    (isSpecial == true && isNotSpecial == false && username.ToUpper().Contains(configSpecialCostCenterUserId)))
                                {
                                    #region Approve Action API Additional Budget Holder (AWITZENBURG)
                                    foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                    {
                                        var CheckData = await claimK2Svc.GetApproverByRelevantIdAndProcessName(id.ToString(), k2ProcessName);
                                        var approver = CheckData.ToList();
                                        var dta = approver.Where(x => x.Username.ToUpper().Equals(additionalUserBH)).Count();

                                        var listK2User = await k2ActivityUserListSvc.Get(x => x.RelevantId == id.ToString() && x.ActivityId == Convert.ToInt32(activityId));
                                        bool isBudgetHolder = false;
                                        Log.Information("UserAlreadyApproved Status : " + dta);
                                        Log.Information("isBudgetHolder : " + isBudgetHolder + " " + "additionalUserBH " + additionalUserBH);
                                        if (listK2User.Count() > 0)
                                        {
                                            foreach (var obj in listK2User)
                                            {
                                                if (additionalUserBH.Contains(obj.ActivityUser))
                                                {
                                                    isBudgetHolder = true;
                                                }
                                            }
                                        }

                                        bool userAlreadyApproved = dta >= 1 ? true : false;
                                        if (!userAlreadyApproved && isBudgetHolder)
                                        {
                                            try
                                            {
                                                var dataRequestApvActionNew = new K2ApprovalActionRequestDTO();
                                                dataRequestApvActionNew.SN = snListApvAction;
                                                dataRequestApvActionNew.Username = additionalUserBH;
                                                dataRequestApvActionNew.ActionName = "approve";
                                                await k2Svc.Approve(dataRequestApvActionNew);
                                                Log.Information("Approve data additional berhasil " + additionalUserBH + "SN :" + snListApvAction);
                                            }
                                            catch
                                            {
                                                Log.Information("Approve data gagal" + additionalUserBH + "SN :" + snListApvAction);
                                            }
                                        }
                                    }
                                    #endregion
                                    Thread.Sleep(configDelayWFAction);
                                    #region Approve Action API BBIDDER
                                    var dataRequestApvAction1 = new K2ApprovalActionRequestDTO();
                                    dataRequestApvAction1.SN = snListApvAction;
                                    dataRequestApvAction1.Username = configSpecialCostCenterUserId;
                                    dataRequestApvAction1.ActionName = "approve";
                                    await k2Svc.Approve(dataRequestApvAction1);
                                    #endregion
                                }
                                else if ((isSpecial == true && isNotSpecial == false && (listSpecialBudgetHolderUserId.Contains(username.ToUpper()))))
                                {
                                    #region Approve Action API BBIDDER
                                    var dataRequestApvActionNew = new K2ApprovalActionRequestDTO();
                                    dataRequestApvActionNew.SN = snListApvAction;
                                    dataRequestApvActionNew.Username = configSpecialCostCenterUserId;
                                    dataRequestApvActionNew.ActionName = "approve";
                                    await k2Svc.Approve(dataRequestApvActionNew);
                                    #endregion

                                    Thread.Sleep(configDelayWFAction);
                                    #region Approve Action API Additional Budget Holder (AWITZENBURG)
                                    var dataRequestApvAction2 = new K2ApprovalActionRequestDTO();
                                    dataRequestApvAction2.SN = snListApvAction;
                                    dataRequestApvAction2.Username = username.ToUpper();
                                    dataRequestApvAction.ActionName = "approve";
                                    await k2Svc.Approve(dataRequestApvAction2);
                                    #endregion
                                    Log.Information("Approve data additional berhasil case 2 " + configSpecialCostCenterUserId + "SN :" + snListApvAction);
                                }
                                else
                                {
                                    await k2Svc.Approve(dataRequestApvAction);
                                    Log.Information("Approve data additional berhasil case 3" + dataRequestApvAction.Username + "SN :" + snListApvAction);
                                }
                                #endregion
                            }
                            else
                            {
                                ret = await k2Svc.Approve(dataRequestApvAction);
                            }
                        }
                        else
                        {
                            ret = await k2Svc.Approve(dataRequestApvAction);
                        }
                    }
                    else if (command.ToUpper() == (WorkflowCommand.Redirect))
                    {
                        Log.Information("WF Redirect");
                        var dtApproveState = await approveStateSvc.Get(x => x.RelevantId.Equals(id.ToString()) && x.ActivityId == "3");
                        foreach (var obj in dtApproveState)
                        {
                            await approveStateSvc.Delete(obj.Id);
                            Log.Information("Update State 0");
                        }

                        ret = await k2Svc.UpdateProcessData(allDataClaim, execWfRequestDTO.Sn, execWfRequestDTO.ActivityId);
                        ret = await k2Svc.Redirect(dataRequestApvAction);
                        Log.Information("Done Redirect");
                    }

                    #endregion

                    //Save Approve State for Positive Flow
                    List<string> listAPositiveFlow = new List<string> {
                        WorkflowCommand.Approve,
                        WorkflowCommand.Redirect,
                        WorkflowCommand.Verify,
                        WorkflowCommand.Recommend,
                        WorkflowCommand.Upload,
                        WorkflowCommand.Redirect
                    };

                    #region Save Approve State
                    if (listAPositiveFlow.Contains(command.ToUpper()))
                    {
                        await claimK2Svc.SaveApproveState(id.ToString(), username, activityId);
                    }
                    #endregion
                }

                transaction.Commit();

                return ret;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<General.Models.Response>> ExecuteBudgetHolder(Guid Id, string sn, string activityId)
        {
            try
            {
                var allDataClaim = await claimDataSvc.GetAllData("NULL", Id, "");
                var res = await k2Svc.UpdateProcessData(allDataClaim, sn, activityId);
                return res;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}