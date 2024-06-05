using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using CI.TMS.Claim.API.Services.K2;
using CI.TMS.Claim.API.Services.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myTree.MicroService.Helper;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Xml.Linq;
using static Antlr4.Runtime.Atn.SemanticContext;

namespace CI.TMS.Claim.API.Services
{
    public class ClaimDataService : BaseService
    {
        TravelAuthorizationService travelauthorizationSvc;
        TravelAuthorizationDestinationService travelauthorizationdestinationSvc;
        TravelAuthorizationSponsorshipService travelauthorizationsponsorshipSvc;
        TravelAuthorizationItineraryService travelauthorizationitinerarySvc;
        TravelAuthorizationTravelerService travelauthorizationtravelerSvc;
        TravelAuthorizationCostCenterService travelauthorizationcostcenterSvc;
        TravelAuthorizationExtendedService travelauthorizationextendedSvc;
        ClaimPerdiemService claimPerdiemSvc;
        ClaimPerdiemDetailService claimPerdiemDetailSvc;
        ClaimExpenseService claimExpenseSvc;
        ClaimExpenseChargeCodeService claimExpenseChargeCodeSvc;
        ClaimPerdiemChargeCodeService claimPerdiemChargeCodeSvc;
        ClaimCommentService claimCommentSvc;
        ClaimSupportingDocumentService claimsupportingdocumentSvc;
        ClaimBoardingPassDocumentService claimboardingpassdocumentSvc;
        ClaimJournalService claimJournalSvc;
        ClaimService claimSvc;
        BudgetHolderK2Services k2Svc;
        string travelDestination = string.Empty;
        EmployeeService employeeSvc;
        AccountingPeriodService accountingperiodSvc;
        FinanceOfficeService financeOfficeSvc;
        TravelJournalService traveljournalSvc;
        CurrencyService currencySvc;
        PerdiemRateService perdiemSvc;
        CalendarService calendarSvc;
        EntityService entityService;

        public ClaimDataService(
            ClaimContext _context,
            IHttpContextAccessor _httpContextAccessor,
            ILogger<BaseService> _log,
            TravelAuthorizationDestinationService _travelauthorizationdestinationSvc,
            TravelAuthorizationSponsorshipService _travelauthorizationsponsorshipSvc,
            TravelAuthorizationItineraryService _travelauthorizationitinerarySvc,
            TravelAuthorizationService _travelauthorizationSvc,
            TravelAuthorizationTravelerService _travelauthorizationtravelerSvc,
            TravelAuthorizationCostCenterService _travelauthorizationcostcenterSvc,
            TravelAuthorizationExtendedService _travelauthorizationextendedSvc,
            ClaimPerdiemService _claimPerdiemSvc,
            ClaimPerdiemDetailService _claimPerdiemDetailSvc,
            ClaimExpenseService _claimExpenseSvc,
            ClaimExpenseChargeCodeService _claimExpenseChargeCodeSvc,
            ClaimPerdiemChargeCodeService _claimPerdiemChargeCodeSvc,
            ClaimCommentService _claimCommentSvc,
            ClaimSupportingDocumentService _claimsupportingdocumentSvc,
            ClaimBoardingPassDocumentService _claimboardingpassdocumentSvc,
            BudgetHolderK2Services _k2Svc,
            ClaimService _claimSvc,
            EmployeeService _employeeSvc,
            ClaimJournalService _claimJournalSvc,
            AccountingPeriodService _accountingperiodSvc,
            FinanceOfficeService _financeOfficeSvc,
            TravelJournalService _traveljournalSvc,
            CurrencyService _currencySvc,
            PerdiemRateService _perdiemSvc,
            CalendarService _calendarSvc,
            EntityService _entityService

            ) : base(_context, _httpContextAccessor, _log)
        {
            travelauthorizationdestinationSvc = _travelauthorizationdestinationSvc;
            travelauthorizationsponsorshipSvc = _travelauthorizationsponsorshipSvc;
            travelauthorizationitinerarySvc = _travelauthorizationitinerarySvc;
            travelauthorizationSvc = _travelauthorizationSvc;
            travelauthorizationtravelerSvc = _travelauthorizationtravelerSvc;
            travelauthorizationcostcenterSvc = _travelauthorizationcostcenterSvc;
            travelauthorizationextendedSvc = _travelauthorizationextendedSvc;
            claimPerdiemSvc = _claimPerdiemSvc;
            claimPerdiemDetailSvc = _claimPerdiemDetailSvc;
            claimExpenseSvc = _claimExpenseSvc;
            claimExpenseChargeCodeSvc = _claimExpenseChargeCodeSvc;
            claimPerdiemChargeCodeSvc = _claimPerdiemChargeCodeSvc;
            claimCommentSvc = _claimCommentSvc;
            claimsupportingdocumentSvc = _claimsupportingdocumentSvc;
            claimboardingpassdocumentSvc = _claimboardingpassdocumentSvc;
            claimJournalSvc = _claimJournalSvc;
            claimSvc = _claimSvc;
            k2Svc = _k2Svc;
            employeeSvc = _employeeSvc;
            accountingperiodSvc = _accountingperiodSvc;
            financeOfficeSvc = _financeOfficeSvc;
            traveljournalSvc = _traveljournalSvc;
            currencySvc = _currencySvc;
            perdiemSvc = _perdiemSvc;
            calendarSvc = _calendarSvc;
            entityService = _entityService;
        }
        public async Task<ClaimDataResponseDTO> GetAllData(string TAId, Guid id, string accessToken)
        {
            try
            {

                ClaimDataResponseDTO cl = new ClaimDataResponseDTO();
                string IdTA = "";
                string TravelOfficeId = string.Empty;
                bool IsAlreadyHaveTripReport = false;
                string TravelOfficeName = string.Empty;
                bool IsHaveClaim = true;
                string SystemCode = string.Empty;
                //if (TAId.ToUpper() == "NULL" && id != Guid.Empty && id is Guid)
                if (TAId.ToUpper() == "NULL" && id != Guid.Empty)
                {
                    var claim = await claimSvc.GetTAId(id);
                    IdTA = claim.Select(p => p.TAId).FirstOrDefault();
                    TravelOfficeId = claim.FirstOrDefault()?.TravelOfficeId;
                    TravelOfficeName = claim.FirstOrDefault()?.TravelOfficeName;
                    SystemCode = claim.FirstOrDefault()?.SystemCode;
                    var claimdata = await claimSvc.Get(x => x.Id == id);
                    IsHaveClaim = Convert.ToBoolean(claimdata.FirstOrDefault()?.IsHaveClaim);
                    cl.JournalNo = claimdata.FirstOrDefault()?.JournalNo;
                }
                else
                {
                    IdTA = TAId;
                    var TaData = await travelauthorizationSvc.Get(x => x.TAId == IdTA);
                    TravelOfficeId = TaData.FirstOrDefault()?.TravelOfficeId;
                    var financeOffice = await financeOfficeSvc.GetByOfficeId(x => x.Id == TravelOfficeId);
                    TravelOfficeName = financeOffice?.TravelOfficeName;
                }

                var DataTA = await travelauthorizationSvc.Get(x => x.TAId == IdTA);
                IsAlreadyHaveTripReport = Convert.ToBoolean(DataTA.FirstOrDefault()?.IsAlreadyHaveTripReport);
                //var testing = await k2Svc.GetBudgetHolderUser(id);
                var taDestination = await travelauthorizationdestinationSvc.Get(x => x.TAId == IdTA && IdTA != "");
                var taDestinationCountHR = await travelauthorizationdestinationSvc.GetCountHR(IdTA);
                var taDestinationCountPersonalTravel = await travelauthorizationdestinationSvc.GetCountPersonalTravel(IdTA);
                var taSponsorshipCount = await travelauthorizationsponsorshipSvc.GetCountSponsorship(IdTA);
                var taItinerary = await travelauthorizationitinerarySvc.Get(x => x.TAId == IdTA && IdTA != "");
                var taExtended = await travelauthorizationextendedSvc.Get(x => x.TAId == IdTA && IdTA != "");
                var taCostCenter = await travelauthorizationcostcenterSvc.Get(x => x.TAId == IdTA && IdTA != "");
                var taTraveler = await travelauthorizationtravelerSvc.GetByTAId(x => x.TAId == IdTA && IdTA != "");
                var taStartTripDate = await travelauthorizationdestinationSvc.GetStartDateTrip(x => x.TAId == IdTA && IdTA != "");
                var taEndTripDate = await travelauthorizationdestinationSvc.GetEndDateTrip(x => x.TAId == IdTA && IdTA != "");
                var taTotalAdvance = await travelauthorizationextendedSvc.Get(x => x.TAId == IdTA && IdTA != "");
                var perdiemClaim = await claimPerdiemSvc.GetPerdiemById(id);
                var perdiemClaimDetail = await claimPerdiemDetailSvc.GetPerdiemDetailById(id);
                var perdiemChargeCode = await claimPerdiemChargeCodeSvc.Get(x => x.ClaimId == id && x.IsActive == true);
                var expenseClaim = await claimExpenseSvc.Get(x => x.ClaimId == id && x.IsActive == true);
                var expenseClaimChargeCode = await claimExpenseChargeCodeSvc.Get(x => x.ClaimId == id && x.IsActive == true);
                var claimSupportingDocument = await claimsupportingdocumentSvc.GetByClaimId(id);
                var claimBoardingPassDocument = await claimboardingpassdocumentSvc.GetByClaimId(id);
                var totalClaim = await claimSvc.GetAllTotal(id);
                var claimcomment = await claimCommentSvc.GetById(id);
                var claimJournal = await claimJournalSvc.GetById(Guid.Empty, x => x.ClaimId == id);
                var period = await accountingperiodSvc.Get();
                var journalList = await traveljournalSvc.GetByTAId(IdTA);

                string destinationCountryName = "";
                string destinationCityName = "";
                string destinationCityNameOther = "";
                string startDateTrip = "";
                string endDateTrip = "";
                string countHRRelated = "";


                foreach (var dest in taDestination)
                {
                    destinationCountryName = dest.CountryName;
                    destinationCityName = dest.CityName;
                    destinationCityNameOther = dest.OtherCityName;
                    startDateTrip = dest.StartDate == null ? "" : dest.StartDate?.ToString("dd MMM yyyy");
                    endDateTrip = dest.EndDate == null ? "" : dest.EndDate?.ToString("dd MMM yyyy");
                    travelDestination += destinationCountryName + ", " + (string.IsNullOrEmpty(destinationCityNameOther) ? destinationCityName : destinationCityNameOther) + " (" + startDateTrip + " - " + endDateTrip + "); ";
                }

                cl.AllEntityList = new List<EntityResponseDTO>();
                foreach (var cc in taCostCenter)
                {
                    cl.AllEntityList.AddRange(await entityService.GetEntityByCostCenterId(cc.CostCenterId));
                }


                string travelRowId = taTraveler.Select(p => p.TravelerId).FirstOrDefault() == null ? "" : taTraveler.Select(p => p.TravelerId).FirstOrDefault();
                var empTravelerData = await employeeSvc.Get(x => x.RowId.Equals(travelRowId));

                string createdBy = totalClaim.Select(p => p.CreatedBy)?.FirstOrDefault() == null ? "" : totalClaim.Select(p => p.CreatedBy).FirstOrDefault();

                var empIntiatorData = await employeeSvc.Get(x => x.EmpUserId.Equals(createdBy));
                string createdByUserName = string.Empty;
                if (empIntiatorData.Count > 0)
                {
                    createdByUserName = empIntiatorData.FirstOrDefault() == null ? "" : empIntiatorData.FirstOrDefault().EmpName;
                }

                string traveluserId = string.Empty;
                if (empTravelerData.Count > 0)
                {
                    traveluserId = empTravelerData.FirstOrDefault() == null ? "" : empTravelerData.FirstOrDefault().EmpUserId;
                }

                var advanceAmount = 0.0;
                if (journalList.Count() > 0)
                {
                    foreach (var item in journalList)
                    {
                        var exchangeRate = currencySvc.Get(x => x.CurrencyCode == item.Currency).Result.Select(x => x.Rate).FirstOrDefault();
                        advanceAmount = advanceAmount + Convert.ToDouble(item.Amount * exchangeRate);
                    }
                }
                else
                {
                    if (taTotalAdvance.Select(p => p.IsHaveJournalProcessed).FirstOrDefault() == true)
                    {
                        advanceAmount = taTotalAdvance.Select(p => p.TotalAdvance).FirstOrDefault();
                    }
                }

                cl.Id = id;
                cl.TAId = IdTA;
                cl.TACode = taTotalAdvance.Select(p => p.TACode).FirstOrDefault();
                cl.TotalPerdiemClaim = Convert.ToDecimal(totalClaim.Select(p => p.TotalPerdiemClaim).FirstOrDefault());
                cl.TotalExpenseClaim = Convert.ToDecimal(totalClaim.Select(p => p.TotalExpenseClaim).FirstOrDefault());
                cl.TotalTEC = Convert.ToDecimal(totalClaim.Select(p => p.TotalTEC).FirstOrDefault());
                cl.ParticipantLetter = taTraveler.Select(p => p.ParticipantLetter).FirstOrDefault();
                cl.DetailTraveler = taTraveler.Select(p => p.DetailTraveler).FirstOrDefault();
                cl.Destination = taDestination;
                cl.ChargeCode = taCostCenter;
                cl.IsAlreadyHaveTripReport = IsAlreadyHaveTripReport;
                cl.TravelerType = taTraveler.Select(p => p.TravelerType).FirstOrDefault();
                cl.TravelerName = taTraveler.Select(p => p.TravelerName).FirstOrDefault();
                cl.TravelerGender = taTraveler.Select(p => p.Gender == 0 ? "Female" : "Male").FirstOrDefault();
                cl.TravelerDutyPost = taTraveler.Select(p => p.TravelerDutyPostName).FirstOrDefault();
                cl.TravelerWorkingLocation = taTraveler.Select(p => p.WorkingLocation).FirstOrDefault();
                cl.TaxSystem = taTraveler.Select(p => p.TaxSystem).FirstOrDefault();
                cl.AparId = taTraveler.Select(p => p.AparId).FirstOrDefault();
                cl.StartDate = taStartTripDate.Select(p => p.StartDate).FirstOrDefault();
                cl.EndDate = taEndTripDate.Select(p => p.EndDate).FirstOrDefault();
                cl.AdvanceAmount = advanceAmount;
                cl.SystemCode = totalClaim.Select(p => p.SystemCode).FirstOrDefault();
                cl.Status = totalClaim.Select(p => p.StatusId).FirstOrDefault();
                cl.CreatedBy = createdBy;
                cl.AmountChargeUsdPrice = Convert.ToDouble(DataTA.FirstOrDefault()?.AmountChargeUsdPrice);
                cl.ClaimConditionId = totalClaim.Select(p => p.ClaimConditionId).FirstOrDefault();
                cl.ClaimPerdiem = perdiemClaim.OrderByDescending(p => p.DateFrom.HasValue).ThenBy(p => p.DateFrom).ToList();
                cl.ClaimPerdiemDetail = perdiemClaimDetail.OrderByDescending(p => p.Date.HasValue).ThenBy(p => p.Date).ToList();
                cl.ClaimPerdiemChargeCode = perdiemChargeCode.ToList();
                cl.ClaimExpense = expenseClaim.OrderBy(x => x.ExpenseClaimDate).ToList();
                cl.ClaimExpenseChargeCode = expenseClaimChargeCode.ToList();
                cl.ClaimComment = claimcomment.ToList();
                cl.ClaimSupportingDocument = claimSupportingDocument.ToList();
                cl.ClaimBoardingPassDocument = claimBoardingPassDocument.ToList();
                cl.TravelOfficeId = TravelOfficeId;
                cl.TravelOfficeName = TravelOfficeName;
                cl.TravelDestination = travelDestination;
                cl.TravelerUserId = traveluserId;
                cl.SystemCode = SystemCode;
                cl.CreatedName = createdByUserName;
                cl.ClaimJournal = claimJournal.ToList();
                cl.IsHaveClaim = IsHaveClaim;
                cl.Period = totalClaim.Select(p => String.IsNullOrEmpty(p.Period) ? period.Select(p => p.Id).FirstOrDefault() : p.Period).FirstOrDefault();
                cl.TransactionDate = totalClaim.Select(p => p.TransactionDate.HasValue ? p.TransactionDate.Value : DateTime.Now).FirstOrDefault();
                if (totalClaim.Select(p => p.DueDate).FirstOrDefault() == null)
                {
                    cl.DueDate = await calendarSvc.GetDueDate(totalClaim.Select(p => p.TransactionDate.HasValue ? p.TransactionDate.Value : DateTime.Now).FirstOrDefault(), 10);
                }
                else
                {
                    cl.DueDate = totalClaim.Select(p => p.DueDate.Value).FirstOrDefault();
                }
                cl.TravelAuthorizationItinerary = taItinerary.OrderBy(p => p.Id).ToList();
                cl.TravelAuthorizationExtended = taExtended.ToList();
                cl.IsTicketRequired = Convert.ToBoolean(taExtended.Select(p => p.IsTicketRequired).FirstOrDefault());
                cl.CountHR = taDestinationCountHR.Select(p => p.CountResult).FirstOrDefault();
                cl.CountPersonalTravel = taDestinationCountPersonalTravel.Select(p => p.CountResult).FirstOrDefault();
                cl.CountSponsorship = taSponsorshipCount.Select(p => p.CountResult).FirstOrDefault();
                cl.PerdiemRatesList = await perdiemSvc.GetListPerdiem(perdiemClaim.OrderBy(p => p.DateFrom).ToList());
                cl.PerdiemDetailRatesList = await perdiemSvc.GetListPerdiemDetail(perdiemClaimDetail.OrderBy(p => p.Date).ToList());
                return cl;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}