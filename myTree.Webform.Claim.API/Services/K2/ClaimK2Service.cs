using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.Services.Master;
using CI.TMS.Claim.API.Helper;
using static CI.TMS.Claim.API.Helper.Variable;
using CI.TMS.Claim.API.Persistence;
using CI.TMS.Claim.API.DTOs.Response;
using System.Security;
using System.Linq;
using CI.TMS.Claim.API.DTOs.Request;
using Newtonsoft.Json;
using myTree.MicroService.Helper;
using Serilog;
using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace CI.TMS.Claim.API.Services.K2
{
    public class ClaimK2Service : BaseService
    {
        List<K2ActivityUser> UnApproveUsers = new List<K2ActivityUser>();
        CostCenterService ccSvc;
        ClaimPerdiemChargeCodeService cPerdiemCCSvc;
        ClaimExpenseChargeCodeService cExpenseCCSvc;
        K2DataLogService k2DataLogSvc;
        EmployeeService empSvc;
        K2ActivityUserListService k2ActivityUserListSvc;
        FinanceOfficerService financeOfficerSvc;
        FinanceOfficeService financeOfficeSvc;
        K2ApproveStateService k2ApproveStateSvc;
        ClaimSubmissionService claimSubmissionSvc;
        ClaimDataService claimDataSvc;
        CountryService countrySvc;
        IConfiguration config;

        List<int> EscalableActId = new List<int>() { 2, 3 };
        string? k2Folder = Configuration["K2Workflow:Folder"];
        string? k2ProcessName = Configuration["K2Workflow:ProcessName"];
        string configSpecialWorkorder = Configuration["SpecialWorkorder"];
        string configSpecialCostCenter = Configuration["SpecialCostCenter"];
        string configSpecialBudgetHolderUserId = Configuration["SpecialBudgetHolderUserId"];
        string configSpecialCostCenterUserId = Configuration["SpecialCostCenterUserId"];
        bool ActiveSpecialBudgetHolder = Convert.ToBoolean(Configuration["ActiveSpecialBudgetHolder"]);
        string configTravelOfficeBogor = Configuration["TravelOfficeBogor"];
        string configTravelOfficeKenya = Configuration["TravelOfficeKenya"];
        string configRegionAsia = Configuration["RegionAsia"];
        string configRegionGermany = Configuration["RegionGermany"];


        Guid claimId = Guid.Empty;
        string? travellerUserId = string.Empty;
        string? travellerName = string.Empty;
        string? initiatorUserId = string.Empty;
        string? initiatorUserName = string.Empty;
        string? travelOfficeId = string.Empty;
        string? travelOfficeName = string.Empty;
        string? financeofficeId = string.Empty;
        string? claimSystemCode = string.Empty;
        string? destination = string.Empty;

        decimal? TotalPerdiemClaim = 0;
        decimal? TotalExpenseClaim = 0;
        decimal? TotalTEC = 0;
        decimal? AdvanceAmount = 0;
        bool? isBudgetHolder = false;
        bool isTesting = false;


        public ClaimK2Service(
            EmployeeService _empSvc,
            CostCenterService _ccSvc,
            ClaimPerdiemChargeCodeService _cPerdiemCCSvc,
            ClaimExpenseChargeCodeService _cExpenseCCSvc,
            ClaimContext _context,
            IHttpContextAccessor _httpContextAccessor,
            ILogger<BaseService> _log,
            IConfiguration _config,
            FinanceOfficerService _financeOfficerSvc,
            K2ApproveStateService _k2ApproveStateSvc,
            ClaimDataService _claimDataSvc,
            K2DataLogService _k2DataLogSvc,
            K2ActivityUserListService _k2ActivityUserListSvc,
            FinanceOfficeService _financeOfficeSvc,
            CountryService _countrySvc)
            : base(_context, _httpContextAccessor, _log)
        {
            config = _config;
            ccSvc = _ccSvc;
            empSvc = _empSvc;
            cPerdiemCCSvc = _cPerdiemCCSvc;
            cExpenseCCSvc = _cExpenseCCSvc;
            financeOfficerSvc = _financeOfficerSvc;
            k2ApproveStateSvc = _k2ApproveStateSvc;
            claimDataSvc = _claimDataSvc;
            k2DataLogSvc = _k2DataLogSvc;
            k2ActivityUserListSvc = _k2ActivityUserListSvc;
            financeOfficeSvc = _financeOfficeSvc;
            countrySvc = _countrySvc;       
        }


        public async Task<Dictionary<string, object>> GetK2Datafield(ClaimDataResponseDTO submissionResDTO = null)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            try
            {
                var allClaimDataObj = submissionResDTO;

                if (!isTesting)
                {
                    claimId = allClaimDataObj.Id;
                    TotalTEC = allClaimDataObj.TotalTEC;
                    TotalExpenseClaim = allClaimDataObj.TotalExpenseClaim;
                    TotalPerdiemClaim = allClaimDataObj.TotalPerdiemClaim;

                    var vCreatedBy = string.IsNullOrEmpty(allClaimDataObj.CreatedBy) ? "" : allClaimDataObj.CreatedBy;
                    var vCreatedName = string.IsNullOrEmpty(allClaimDataObj.CreatedName) ? "" : allClaimDataObj.CreatedName;
                    var vTravellerName = string.IsNullOrEmpty(allClaimDataObj.TravelerName) ? "" : allClaimDataObj.TravelerName;
                    var vTravellerUserId = string.IsNullOrEmpty(allClaimDataObj.TravelerUserId) ? "" : allClaimDataObj.TravelerUserId;
                    var vSystemCode = string.IsNullOrEmpty(allClaimDataObj.SystemCode) ? "" : allClaimDataObj.SystemCode;
                    var vTravelDestination = string.IsNullOrEmpty(allClaimDataObj.TravelDestination) ? "" : allClaimDataObj.TravelDestination;
                    var vTravelOfficeId = string.IsNullOrEmpty(allClaimDataObj.TravelOfficeId) ? "" : allClaimDataObj.TravelOfficeId;
                    var vTravelOfficeName = string.IsNullOrEmpty(allClaimDataObj.TravelOfficeName) ? "" : allClaimDataObj.TravelOfficeName;

                    initiatorUserId = vCreatedBy;
                    initiatorUserName = vCreatedName;
                    travellerUserId = vTravellerUserId;
                    travellerName = vTravellerName;
                    claimSystemCode = vSystemCode;
                    destination = vTravelDestination;
                    travelOfficeId = vTravelOfficeId;
                    travelOfficeName = vTravelOfficeName;
                }
                await SetUnApproveUser(claimId.ToString());

                res.Add("TotalTEC", TotalTEC);
                res.Add("InitiatorName", initiatorUserName);
                res.Add("TravellerName", travellerName);
                res.Add("TravellerUser", travellerUserId);
                res.Add("ClaimSystemCode", claimSystemCode);
                res.Add("Destination", destination);
                res.Add("TravelOffice", travelOfficeName);
                res.Add("InitiatorUser", MapUser(1));
                res.Add("FinanceUser", MapUser(2));
                res.Add("BudgetHolderUser", MapUser(3));
                Log.Information("FinanceUser : " + MapUser(2));
                Log.Information("InitiatorUser : " + MapUser(1));
                return res;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2ActivityUser>> GetK2UserForIntegratedPortal(ClaimDataResponseDTO submissionResDTO)
        {
            List<K2ActivityUser> res = new List<K2ActivityUser>();
            try
            {
                var allClaimDataObj = submissionResDTO;

                if (!isTesting)
                {
                    claimId = allClaimDataObj.Id;
                    TotalTEC = allClaimDataObj.TotalTEC;
                    TotalExpenseClaim = allClaimDataObj.TotalExpenseClaim;
                    TotalPerdiemClaim = allClaimDataObj.TotalPerdiemClaim;

                    var vCreatedBy = string.IsNullOrEmpty(allClaimDataObj.CreatedBy) ? "" : allClaimDataObj.CreatedBy;
                    var vCreatedName = string.IsNullOrEmpty(allClaimDataObj.CreatedName) ? "" : allClaimDataObj.CreatedName;
                    var vTravellerName = string.IsNullOrEmpty(allClaimDataObj.TravelerName) ? "" : allClaimDataObj.TravelerName;
                    var vTravellerUserId = string.IsNullOrEmpty(allClaimDataObj.TravelerUserId) ? "" : allClaimDataObj.TravelerUserId;
                    var vSystemCode = string.IsNullOrEmpty(allClaimDataObj.SystemCode) ? "" : allClaimDataObj.SystemCode;
                    var vTravelDestination = string.IsNullOrEmpty(allClaimDataObj.TravelDestination) ? "" : allClaimDataObj.TravelDestination;
                    var vTravelOfficeId = string.IsNullOrEmpty(allClaimDataObj.TravelOfficeId) ? "" : allClaimDataObj.TravelOfficeId;

                    initiatorUserId = vCreatedBy;
                    initiatorUserName = vCreatedName;
                    travellerUserId = vTravellerUserId;
                    travellerName = vTravellerName;
                    claimSystemCode = vSystemCode;
                    destination = vTravelDestination;
                    travelOfficeId = vTravelOfficeId;
                }

                return await GetEligibleActivityUser();
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task SetUnApproveUser(string Id)
        {
            try
            {
                UnApproveUsers = new List<K2ActivityUser>();

                var eligibleUser = await GetEligibleActivityUser();
                var approverObj = await GetApproverByRelevantIdAndProcessName(Id, k2ProcessName);
                var approver = approverObj.ToList();

                foreach (var user in eligibleUser)
                {
                    bool userAlreadyApproved = approver.Where(x => x.ActivityId.Equals(user.ActivityID.ToString())).Any();

                    if (!userAlreadyApproved)
                    {
                        log.LogInformation("Un-Approved ActivityID: " + user.ActivityID + ". Username: " + user.Username);
                        UnApproveUsers.Add(user);
                    }
                }
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2ActivityUser>> GetEligibleActivityUser()
        {
            List<K2ActivityUser> ret = new List<K2ActivityUser>();
            try
            {

                var financeUser = await GetFinanceUser(travelOfficeId, initiatorUserId, travellerUserId);
                Log.Information("GetEligibleActivityUser finance user : " + financeUser.FirstOrDefault() + " , Travel Office : " + travelOfficeId);
                var initiatorUser = new List<string>() { initiatorUserId, travellerUserId };

                string budgetHoldeUserId = "";
                bool isRequestBudgetHolder = false;
                K2ActivityUserListResponseDTO k2Actuser = new K2ActivityUserListResponseDTO();

                var listK2User = await k2ActivityUserListSvc.Get(x => x.RelevantId == claimId.ToString());
                if (listK2User.Count() > 0)
                {
                    Log.Information("Claim Get budget holder for ID: " + claimId.ToString());
                    try
                    {
                        k2Actuser = listK2User.FirstOrDefault(x => x.ActivityId == 3 && x.SeqNo == listK2User.Max(x => x.SeqNo));
                    }
                    catch (Exception ex)
                    {
                        k2Actuser = null;
                        Log.Error("Claim Error Get Budget holder. Detail: " + ex.InnerException);
                    }
                    Log.Information("Claim Get Budget holder for ID: " + claimId.ToString() + ". Result ILMG: " + k2Actuser?.ActivityUser);
                }

                if (!string.IsNullOrEmpty(k2Actuser?.ActivityUser))
                {
                    isRequestBudgetHolder = true;
                    budgetHoldeUserId = k2Actuser.ActivityUser;

                    Log.Information("Claim Get K2 User - Get budget holder - Result :" + budgetHoldeUserId);
                }
                else
                {
                    isRequestBudgetHolder = false;
                }

                if (isRequestBudgetHolder)
                {
                    var budgetHolderUser = await GetBudgetHolderUser(claimId);
                    ret.AddRange(budgetHolderUser
                        .Select(x => new K2ActivityUser() { Username = x, ActivityID = 3 })
                        );
                }

                #region participant
                //ret.AddRange(budgetHolderUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 3 }));
                ret.AddRange(financeUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 2 }));
                ret.AddRange(initiatorUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 1 }));
                ret.AddRange(initiatorUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 0 }));

                #endregion

                ret = await SetEscalationAndFiltering(ret);

                return ret;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2ActivityUser>> SetEscalationAndFiltering(List<K2ActivityUser> listOfEligible)
        {
            try
            {
                if (listOfEligible.Count() > 0)
                {
                    foreach (K2ActivityUser eligibleUser in listOfEligible.OrderByDescending(x => x.ActivityID).ToList())
                    {
                        var user = eligibleUser.Username;
                        var actId = eligibleUser.ActivityID ?? 0;
                        int[] requesterActId = { 0, 1 };
                        bool isUserInitiatorOrRequester = false;
                        if (user.ToUpper().Equals(travellerUserId.ToUpper()) || user.ToUpper().Equals(initiatorUserId.ToUpper()))
                        {
                            isUserInitiatorOrRequester = true;
                        }

                        if (!requesterActId.Contains(actId))
                        {
                            if (isUserInitiatorOrRequester)
                            {
                                if (EscalableActId.Contains(actId))
                                {
                                    var userCountPerActivityId = listOfEligible.Where(x => x.ActivityID.Equals(actId)).Count();

                                    if (userCountPerActivityId < 1)
                                    {
                                        if (actId.Equals(2)) //DCS
                                        {
                                            var spv = await GetDirectSupervisorByUserId(userId);
                                            eligibleUser.Substitute = spv.EmpUserId;
                                        }
                                        else if (actId.Equals(3))
                                        {
                                            //check and balance
                                            var spv = await GetDirectSupervisorByUserId(userId);
                                            eligibleUser.Substitute = spv.EmpUserId;
                                        }
                                    }
                                    else
                                    {
                                        listOfEligible.RemoveAll(x => x.Username.ToUpper().Equals(user) && x.ActivityID.Equals(actId));
                                    }
                                }
                                else
                                {
                                    listOfEligible.RemoveAll(x => x.Username.ToUpper().Equals(user) && x.ActivityID.Equals(actId));
                                }
                            }
                        }
                    }
                }

                return listOfEligible;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2ActivityUser>> GetAllActivityUser(Guid Id, ClaimDataResponseDTO claimData)
        {
            List<K2ActivityUser> ret = new List<K2ActivityUser>();
            try
            {
                var budgetHolderUser = await GetBudgetHolderUser(Id);
                var financeUser = await GetFinanceUser(claimData.TravelOfficeId, claimData.CreatedBy, claimData.TravelerUserId);
                var initiatorUser = new List<string>() { claimData.CreatedBy, claimData.TravelerUserId };

                ret.AddRange(budgetHolderUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 3 }));
                ret.AddRange(financeUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 2 }));
                ret.AddRange(initiatorUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 1 }));
                ret.AddRange(initiatorUser.Select(x => new K2ActivityUser() { Username = x, ActivityID = 0 }));

                ret = await SetEscalationAndFiltering(ret);

                return ret;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        //public async Task<List<string>> GetUnitLeaderByUnitId(string unitId = "unitId")
        //{
        //    try
        //    {
        //        List<string> listUser = new List<string>();
        //        var unitLeadfObj = await unitLeadSvc.Get(x => x.UnitId.Equals(unitId));
        //        if (unitLeadfObj.Count() > 0)
        //        {
        //            foreach (UnitLeaderResponseDTO obj in unitLeadfObj)
        //            {
        //                listUser.Add(obj.UnitLeaderUserId);
        //            }
        //        }

        //        listUser.RemoveAll(user => string.IsNullOrEmpty(user));
        //        listUser = listUser.Distinct().ToList();
        //        return listUser;
        //    }
        //    catch (Exception x)
        //    {
        //        ErrorServiceHandler(x);
        //        throw;
        //    }
        //}

        public async Task<List<string>> GetBudgetHolderUser(Guid claimId)
        {
            try
            {
                List<string> listUser = new List<string>();
                var cPerdiemObj = await cPerdiemCCSvc.Get(x => x.ClaimId.Equals(claimId) && x.IsActive == true);
                var cExpenseObj = await cExpenseCCSvc.Get(x => x.ClaimId.Equals(claimId) && x.IsActive == true);
                /*Active untuk budget holder special case*/
                if (ActiveSpecialBudgetHolder)
                {
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

                    var CheckData = await GetApproverByRelevantIdAndProcessName(claimId.ToString(), k2ProcessName);
                    var approver = CheckData.ToList();


                    if (cPerdiemObj.Count() > 0)
                    {
                        foreach (var obj in cPerdiemObj)
                        {
                            if (listSpecialWorkorder.Contains(obj.WorkOrderId) && obj.CostCenterId == configSpecialCostCenter)
                            {
                                foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                {
                                    var dta = approver.Where(x => x.Username.ToUpper().Equals(additionalUserBH)).Count();
                                    if (dta == 0)
                                    {
                                        listUser.Add(additionalUserBH);
                                        log.LogInformation("Count Dta = 0 " + additionalUserBH);
                                    }
                                }
                            }
                            var dta2 = approver.Where(x => x.Username.ToUpper().Equals(configSpecialCostCenterUserId)).Count();
                            if (dta2 > 0)
                            {
                                foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                {
                                    listUser.Add(additionalUserBH);
                                }
                            }
                            else
                            {
                                listUser.Add(obj.BudgetHolderUserId);
                            }

                        }
                    }

                    if (cExpenseObj.Count() > 0)
                    {
                        foreach (var obj in cExpenseObj)
                        {
                            if (listSpecialWorkorder.Contains(obj.WorkOrderId) && obj.CostCenterId == configSpecialCostCenter)
                            {
                                foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                {
                                    var dta = approver.Where(x => x.Username.ToUpper().Equals(additionalUserBH)).Count();
                                    if (dta == 0)
                                    {
                                        listUser.Add(additionalUserBH);
                                        log.LogInformation("Count Dta = 0 " + additionalUserBH);
                                    }
                                }
                            }
                            var dta2 = approver.Where(x => x.Username.ToUpper().Equals(configSpecialCostCenterUserId)).Count();
                            if (dta2 > 0)
                            {
                                foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                {
                                    listUser.Add(additionalUserBH);
                                }
                            }
                            else
                            {
                                listUser.Add(obj.BudgetHolderUserId);
                            }
                        }
                    }
                }
                else
                {
                    if (cPerdiemObj.Count() > 0)
                    {
                        foreach (var obj in cPerdiemObj)
                        {
                            listUser.Add(obj.BudgetHolderUserId);
                        }
                    }

                    if (cExpenseObj.Count() > 0)
                    {
                        foreach (var obj in cExpenseObj)
                        {
                            listUser.Add(obj.BudgetHolderUserId);
                        }
                    }
                }
                listUser.RemoveAll(x => string.IsNullOrEmpty(x));
                listUser = listUser.Distinct().ToList();
                Log.Information("List USer : " + listUser.ToList());
                return listUser;

            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<string>> GetFinanceUser(string travelOfficeId, string InitiatorUser, string Traveler)
        {
            try
            {
                List<string> listUser = new List<string>();

                var cFinanceObj = await financeOfficerSvc.Get(x => x.TravelOfficeId.Equals(travelOfficeId) && x.Status == true);
               

                if (cFinanceObj.Count() > 0)
                {
                    foreach (var obj in cFinanceObj.Distinct())
                    {
                        listUser.Add(obj.UserId);
                        Log.Information("Finance User :" + obj.UserId);
                    }
                    Log.Information("Count Finance Office User : " + cFinanceObj.Count);
                    if (cFinanceObj.Count <= 2)
                    {
                        #region Checking if finance office user is traveler or initiator and if finance office user just 1 user, Checking if traveler and initiator is Finance User 
                        if (((listUser.Contains(Traveler)) || (listUser.Contains(InitiatorUser)) && cFinanceObj.Count == 1) || (((listUser.Contains(Traveler)) && Traveler.ToLower() != InitiatorUser.ToLower()  && (listUser.Contains(InitiatorUser))) && cFinanceObj.Count == 2))
                        {
                            listUser.Clear();
                            Log.Information("Checking if finance office user is traveler or initiator and if finance office user just 1 user");
                            var Region = await countrySvc.GetRegion(travelOfficeId);
                            if ((Region.FirstOrDefault().RegionName == configRegionAsia) || (Region.FirstOrDefault().RegionName == configRegionGermany))
                            {
                                var FinanceOfficeBogor = await financeOfficerSvc.Get(x => x.TravelOfficeId.Equals(configTravelOfficeBogor) && x.Status == true);
                                foreach (var obj in FinanceOfficeBogor.Distinct())
                                {
                                    listUser.Add(obj.UserId);
                                    Log.Information("Finance Office Bogor User :" + obj.UserId);
                                }
                            }
                            else
                            {
                                var FinanceOfficeKenya = await financeOfficerSvc.Get(x => x.TravelOfficeId.Equals("KENYA") && x.Status == true);
                                foreach (var obj in FinanceOfficeKenya.Distinct())
                                {
                                    listUser.Add(obj.UserId);
                                    Log.Information("Finance Office Kenya User :" + obj.UserId);
                                }
                            }
                        }
                        #endregion
                    }
                }

                listUser.RemoveAll(x => string.IsNullOrEmpty(x));
                listUser = listUser.Distinct().ToList();
                return listUser;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<EmployeeResponseDTO>? GetDirectSupervisorByUserId(string userId = "userId")
        {
            try
            {
                EmployeeResponseDTO? spvObjSingle = new EmployeeResponseDTO();

                var empObj = await empSvc.Get(x => x.EmpUserId.Equals(userId.ToUpper()));
                var empObjSingle = empObj.FirstOrDefault();
                if (empObjSingle is not null)
                {
                    var spvEmpId = empObjSingle.DirectSupEmpId;
                    var spvObj = await empSvc.Get(x => x.RowId.Equals(spvEmpId.ToUpper()));
                    spvObjSingle = spvObj.FirstOrDefault();
                }

                return spvObjSingle;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        private string MapUser(int activityId)
        {
            try
            {
                string ret = string.Join(';', UnApproveUsers.Where(x => x.ActivityID.Equals(activityId)).Select(x => !string.IsNullOrEmpty(x.Substitute) ? x.Substitute : x.Username).ToList());
                return ret;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<bool> CompareLog(string Id, bool isReject = false)
        {
            bool returnVal = false;
            int differentCount = 0;
            Dictionary<string, bool> listOfDifferentCostC = new Dictionary<string, bool>();

            if (isReject == false)
            {
                log.LogInformation($"Start get older and current in function CompareLog function. Processname{k2ProcessName}");
                var oldGrantDataObj = await GetListOlderDataLog(Id, k2ProcessName);
                var oldGrantData = oldGrantDataObj;
                var newGrantDataObj = await GetListCurrentDataLog(Id, k2ProcessName);
                var newGrantData = newGrantDataObj;
                log.LogInformation($"End get older and current in function CompareLog function. Processname{k2ProcessName}");


                string[] fields = new string[] { "TotalTEC", "ClaimPerdiemChargeCode", "ClaimExpenseChargeCode", "ClaimPerdiem", "ClaimExpense" };

                foreach (var oldData in oldGrantData)
                {

                    if (fields.Contains(oldData.FieldName))
                    {
                        if (oldData.Value.ToLower() != newGrantData.FirstOrDefault(x => x.FieldName == oldData.FieldName).Value.ToLower())
                        {
                            if (oldData.FieldName == "ClaimPerdiemChargeCode")
                            {
                                var oldChargeCode = oldData.Value;
                                var resultOld = JArray.Parse(oldChargeCode);

                                var newChargeCode = newGrantData.FirstOrDefault(x => x.FieldName == oldData.FieldName).Value;
                                var resultNew = JArray.Parse(newChargeCode);


                                List<ClaimPerdiemChargeCodeResponseDTO> perdiemCCDTOOld = new List<ClaimPerdiemChargeCodeResponseDTO>();
                                List<ClaimPerdiemChargeCodeResponseDTO> perdiemCCDTONew = new List<ClaimPerdiemChargeCodeResponseDTO>();

                                //perdiemCCDTOOld.MapFrom(resultOld);
                                //perdiemCCDTONew.MapFrom(resultNew);

                                foreach (var subItem in resultOld)
                                {
                                    var ccvalObj = subItem.ToObject<ClaimPerdiemChargeCodeResponseDTO>();
                                    perdiemCCDTOOld.Add(ccvalObj);
                                }

                                foreach (var subItem in resultNew)
                                {
                                    var ccvalObj = subItem.ToObject<ClaimPerdiemChargeCodeResponseDTO>();
                                    perdiemCCDTONew.Add(ccvalObj);
                                }

                                if (perdiemCCDTOOld.Count != perdiemCCDTONew.Count)
                                {
                                    differentCount++;

                                }

                                foreach (var p in perdiemCCDTONew.Where(x => x.IsActive == true))
                                {
                                    var pCC = perdiemCCDTOOld.Where(x => x.CostCenterId == p.CostCenterId && x.WorkOrderId == p.WorkOrderId && x.EntityId == p.EntityId && x.LegalEntityId == p.LegalEntityId && p.IsActive == true).FirstOrDefault();

                                    if (pCC != null)
                                    {
                                        if (p.Amount != pCC.Amount)
                                        {
                                            differentCount++;
                                        }
                                    }
                                    else
                                    {
                                        differentCount++;

                                    }
                                }
                            }
                            else if (oldData.FieldName == "ClaimExpenseChargeCode")
                            {
                                var oldChargeCode = oldData.Value;
                                var resultOld = JArray.Parse(oldChargeCode);

                                var newChargeCode = newGrantData.FirstOrDefault(x => x.FieldName == oldData.FieldName).Value;
                                var resultNew = JArray.Parse(newChargeCode);


                                List<ClaimExpenseChargeCodeResponseDTO> expenseCCDTOOld = new List<ClaimExpenseChargeCodeResponseDTO>();
                                List<ClaimExpenseChargeCodeResponseDTO> expenseCCDTONew = new List<ClaimExpenseChargeCodeResponseDTO>();

                                //expenseCCDTOOld.MapFrom(resultOld);
                                //expenseCCDTONew.MapFrom(resultNew);
                                foreach (var subItem in resultOld)
                                {
                                    var ccvalObj = subItem.ToObject<ClaimExpenseChargeCodeResponseDTO>();
                                    expenseCCDTOOld.Add(ccvalObj);
                                }

                                foreach (var subItem in resultNew)
                                {
                                    var ccvalObj = subItem.ToObject<ClaimExpenseChargeCodeResponseDTO>();
                                    expenseCCDTOOld.Add(ccvalObj);
                                }


                                if (expenseCCDTOOld.Count != expenseCCDTONew.Count)
                                {
                                    differentCount++;

                                }

                                foreach (var p in expenseCCDTONew.Where(x => x.IsActive == true))
                                {
                                    var pCC = expenseCCDTOOld.Where(x => x.CostCenterId == p.CostCenterId && x.WorkOrderId == p.WorkOrderId && x.EntityId == p.EntityId && x.LegalEntityId == p.LegalEntityId && p.IsActive == true).FirstOrDefault();

                                    if (pCC != null)
                                    {
                                        if (p.Amount != pCC.Amount)
                                        {
                                            differentCount++;
                                        }
                                    }
                                    else
                                    {
                                        differentCount++;

                                    }
                                }

                            }
                            else if (oldData.FieldName == "TotalTEC")
                            {
                                differentCount++;
                            }
                            //else if (oldData.FieldName == "ClaimPerdiem")
                            //{
                            //    var oldPerdiem = oldData.Value;
                            //    var resultOld = JArray.Parse(oldPerdiem);

                            //    var newPerdiem = newGrantData.FirstOrDefault(x => x.FieldName == oldData.FieldName).Value;
                            //    var resultNew = JArray.Parse(newPerdiem);


                            //    List<ClaimPerdiemResponseDTO> perdiemDTOOld = new List<ClaimPerdiemResponseDTO>();
                            //    List<ClaimPerdiemResponseDTO> perdiemDTONew = new List<ClaimPerdiemResponseDTO>();

                            //    perdiemDTOOld.MapFrom(resultOld);
                            //    perdiemDTONew.MapFrom(resultNew);

                            //    if (perdiemDTOOld.Count != perdiemDTONew.Count)
                            //    {
                            //        differentCount++;

                            //    }

                            //    foreach (var p in perdiemDTONew.Where(x => x.IsActive == true))
                            //    {
                            //        var pCC = perdiemCCDTOOld.Where(x => x.CostCenterId == p.CostCenterId && x.WorkOrderId == p.WorkOrderId && x.EntityId == p.EntityId && x.LegalEntityId == p.LegalEntityId && p.IsActive == true).FirstOrDefault();

                            //        if (pCC != null)
                            //        {
                            //            if (p.Amount != pCC.Amount)
                            //            {
                            //                differentCount++;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            differentCount++;

                            //        }
                            //    }
                            //    differentCount++;
                            //}
                            //else if (oldData.FieldName == "ClaimExpense")
                            //{
                            //    differentCount++;
                            //}
                        }
                    }
                }
            }

            if (differentCount > 0)
            {
                var apvStatesObj = await GetApproverByRelevantIdAndProcessName(Id, k2ProcessName);
                var apvStates = apvStatesObj.ToList();
                foreach (K2ApproveStateResponseDTO apvResDto in apvStates)
                {
                    K2ApproveStateRequestDTO reqDto = new K2ApproveStateRequestDTO();
                    reqDto.MapFrom(apvResDto);
                    reqDto.Id = apvResDto.Id;
                    reqDto.State = 0;
                    await k2ApproveStateSvc.Update(reqDto);
                }

                returnVal = true;
            }

            return returnVal;

        }

        public async Task SaveClaimDataLogList(ClaimDataResponseDTO allDataClaim)
        {
            try
            {
                claimId = allDataClaim.Id;

                #region Prepare K2 Log Data
                List<K2DataLogRequestDTO> listRequestDataLogs = new List<K2DataLogRequestDTO>();
                string claimRequestIdId = allDataClaim.Id.ToString();

                K2DataLogRequestDTO totalAmount = new K2DataLogRequestDTO();
                totalAmount.RelevantId = claimRequestIdId;
                totalAmount.Module = k2ProcessName;
                totalAmount.FieldName = "TotalTEC";
                totalAmount.Value = allDataClaim.TotalTEC.ToString();

                K2DataLogRequestDTO claimPerdiem = new K2DataLogRequestDTO();
                claimPerdiem.RelevantId = claimRequestIdId;
                claimPerdiem.Module = k2ProcessName;
                claimPerdiem.FieldName = "ClaimPerdiem";
                claimPerdiem.Value = JsonConvert.SerializeObject(allDataClaim.ClaimPerdiem?.OrderBy(x => x.DateFrom));

                K2DataLogRequestDTO claimPerdiemChargeCodes = new K2DataLogRequestDTO();
                claimPerdiemChargeCodes.RelevantId = claimRequestIdId;
                claimPerdiemChargeCodes.Module = k2ProcessName;
                claimPerdiemChargeCodes.FieldName = "ClaimPerdiemChargeCode";
                claimPerdiemChargeCodes.Value = JsonConvert.SerializeObject(allDataClaim.ClaimPerdiemChargeCode?.OrderBy(x => x.SeqNo));

                K2DataLogRequestDTO claimExpense = new K2DataLogRequestDTO();
                claimExpense.RelevantId = claimRequestIdId;
                claimExpense.Module = k2ProcessName;
                claimExpense.FieldName = "ClaimExpense";
                claimExpense.Value = JsonConvert.SerializeObject(allDataClaim.ClaimExpense?.OrderBy(x => x.ExpenseClaimDate));

                K2DataLogRequestDTO claimExpenseChargeCodes = new K2DataLogRequestDTO();
                claimExpenseChargeCodes.RelevantId = claimRequestIdId;
                claimExpenseChargeCodes.Module = k2ProcessName;
                claimExpenseChargeCodes.FieldName = "ClaimExpenseChargeCode";
                claimExpenseChargeCodes.Value = JsonConvert.SerializeObject(allDataClaim.ClaimExpenseChargeCode?.OrderBy(x => x.SeqNo));

                listRequestDataLogs.Add(totalAmount);
                listRequestDataLogs.Add(claimPerdiemChargeCodes);
                listRequestDataLogs.Add(claimExpenseChargeCodes);

                #endregion
                listRequestDataLogs = await SetDataLogNextSeqNo(listRequestDataLogs);
                await SaveListOfDatalog(listRequestDataLogs);
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2DataLogRequestDTO>> SetDataLogNextSeqNo(List<K2DataLogRequestDTO> listDataLog)
        {
            try
            {
                int? nextSeqNo = 0;
                var datalogObj = await k2DataLogSvc.Get(x => x.RelevantId.Equals(claimId.ToString()));
                if (datalogObj.Count() > 0)
                {
                    datalogObj = datalogObj.OrderByDescending(x => x.SeqNo).ToList();
                    nextSeqNo = datalogObj.FirstOrDefault().SeqNo + 1;
                }
                else
                {
                    nextSeqNo = 1;
                }

                foreach (K2DataLogRequestDTO log in listDataLog)
                {
                    log.SeqNo = nextSeqNo;
                }

                return listDataLog;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task SaveListOfDatalog(List<K2DataLogRequestDTO> logObj)
        {
            try
            {
                foreach (K2DataLogRequestDTO obj in logObj)
                {
                    await k2DataLogSvc.Add(obj);
                }
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2DataLogResponseDTO>> GetListCurrentDataLog(string requestId, string module = "")
        {
            try
            {
                if (string.IsNullOrEmpty(module))
                {
                    module = Configuration["ProcessName"] ?? "Not Available";
                }

                int? maxSeqNo = 0;
                var datalogObj = await k2DataLogSvc.Get(x => x.RelevantId.Equals(requestId.ToString()));
                if (datalogObj.Count() > 0)
                {
                    datalogObj = datalogObj.OrderByDescending(x => x.SeqNo).ToList();
                    maxSeqNo = datalogObj.FirstOrDefault().SeqNo;

                    datalogObj = datalogObj.Where(x => x.SeqNo.Equals(maxSeqNo))
                        .OrderByDescending(x => x.SeqNo)
                        .ThenBy(x => x.FieldName)
                        .ToList();
                }

                return datalogObj.ToList();
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<K2DataLogResponseDTO>> GetListOlderDataLog(string Id, string module)
        {
            try
            {
                if (string.IsNullOrEmpty(module))
                {
                    module = Configuration["ProcessName"] ?? "Not Available";
                }

                int? maxSeqNo = 0;
                var datalogObj = await k2DataLogSvc.Get(x => x.RelevantId.Equals(Id.ToString()));
                if (datalogObj.Count() > 0)
                {
                    datalogObj = datalogObj.OrderByDescending(x => x.SeqNo).ToList();
                    maxSeqNo = datalogObj.FirstOrDefault().SeqNo;

                    datalogObj = datalogObj.Where(x => x.SeqNo.Equals(maxSeqNo - 1))
                        .OrderByDescending(x => x.SeqNo)
                        .ThenBy(x => x.FieldName)
                        .ToList();
                }

                return datalogObj.ToList();
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task SaveApproveState(string id, string username, string activityId)
        {
            try
            {
                K2ApproveStateRequestDTO apvStateReq = new K2ApproveStateRequestDTO();
                apvStateReq.State = 1;
                apvStateReq.Module = k2ProcessName;
                apvStateReq.RelevantId = id;
                apvStateReq.Username = username;
                apvStateReq.ActivityId = activityId;

                await k2ApproveStateSvc.Add(apvStateReq);
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<IEnumerable<K2ApproveStateResponseDTO>> GetApproverByRelevantIdAndProcessName(string relevantId, string ProcessName)
        {
            try
            {
                return await k2ApproveStateSvc.Get(x => x.RelevantId.Equals(relevantId) && x.State.Equals(1));
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task SaveK2ActivityUserList(ClaimDataResponseDTO submissionResDTO)
        {
            try
            {
                var userListObj = await GetK2UserForIntegratedPortal(submissionResDTO);

                if (userListObj.Count > 0)
                {
                    var itemToRemove = userListObj.Where(x => x.ActivityID == 0).ToList();
                    if (itemToRemove != null)
                    {
                        if (itemToRemove.Count() > 0)
                        {
                            foreach (K2ActivityUser item in itemToRemove)
                            {
                                userListObj.Remove(item);
                            }
                        }
                    }

                    int? currentMaxSeqNo = await GetK2ActivityUserListMaxSeqNo(claimId.ToString());
                    currentMaxSeqNo = currentMaxSeqNo ?? 0;
                    var nextSeqNo = currentMaxSeqNo + 1;

                    userListObj = userListObj.OrderBy(x => x.ActivityID).ToList();
                    foreach (K2ActivityUser obj in userListObj)
                    {
                        K2ActivityUserListRequestDTO k2ActReqDto = new K2ActivityUserListRequestDTO();
                        k2ActReqDto.RelevantId = claimId.ToString();
                        k2ActReqDto.Folder = k2Folder;
                        k2ActReqDto.ProcessName = k2ProcessName;
                        k2ActReqDto.ActivityName = string.Empty;
                        k2ActReqDto.ActivityId = obj.ActivityID;
                        k2ActReqDto.ActivityUser = !string.IsNullOrEmpty(obj.Substitute) ? obj.Substitute : obj.Username;
                        k2ActReqDto.SeqNo = nextSeqNo;

                        await k2ActivityUserListSvc.Add(k2ActReqDto);
                    }
                }
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<int?> GetK2ActivityUserListMaxSeqNo(string id)
        {
            try
            {
                int? ret = 0;

                var k2ActUserList = await k2ActivityUserListSvc.Get(x => x.RelevantId.Equals(id.ToString())
                                                                      && x.ProcessName.ToUpper().Equals(k2ProcessName.ToUpper())
                                                                      && x.Folder.ToUpper().Equals(k2Folder.ToUpper()));

                if (k2ActUserList.Count() > 0)
                {
                    k2ActUserList = k2ActUserList.OrderByDescending(x => x.SeqNo).ToList();
                    ret = k2ActUserList.FirstOrDefault().SeqNo;
                }

                return ret;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<K2ApprovalNotes> GetApprovalNotes(Guid id, string username, string activityId)
        {
            try
            {
                K2ApprovalNotes ret = new K2ApprovalNotes();

                List<int> listActId = new List<int>();
                List<string> listStringActivity = new List<string>();
                List<string> listStringRole = new List<string>();
                string trueBudgetHolder = "";
                string _actDesc = "-";
                string _actName = "-";
                string action = "";
                bool isReAction = false;
                var allDataCLaim = await claimDataSvc.GetAllData("NULL", id, "");

                IEnumerable<K2ApproveStateResponseDTO> aps = await k2ApproveStateSvc.Get(x => x.RelevantId.Equals(id.ToString())
                                                                                       && x.Module.Equals(k2ProcessName)
                                                                                       && x.ActivityId.Equals(activityId)
                                                                                       && x.Username.Equals(username)
                                                                                       && x.State.Equals(0));
                if (aps.Count() > 0)
                {
                    isReAction = true;
                }

                if (isReAction)
                {
                    action = "re-";
                }

                string[] actApprove = new string[] { "2", "3" };

                if (actApprove.Contains(activityId))
                {
                    action += "approving";
                }


                if (activityId == "1")
                {
                    _actDesc = "You are ";
                }
                else
                {
                    _actDesc = "You are " + action + " this request for ";
                }

                _actDesc += "<ul>";



                List<K2ActivityUser> allActivityUserMapped = await GetAllActivityUser(id, allDataCLaim);

                foreach (K2ActivityUser user in allActivityUserMapped)
                {
                    if (!string.IsNullOrEmpty(user.Substitute))
                    {
                        if (user.Substitute.ToLower() == username.ToLower())
                        {
                            listActId.Add(Convert.ToInt32(user.ActivityID));
                        }
                    }
                    else if (!string.IsNullOrEmpty(user.Username))
                    {
                        if (user.Username.ToLower() == username.ToLower())
                        {
                            listActId.Add(Convert.ToInt32(user.ActivityID));
                        }
                    }
                }

                if (listActId.Count > 0)
                {
                    listActId.RemoveAll(item => item == 0);
                    if (activityId != "1")
                    {
                        listActId.RemoveAll(item => item == 1);
                    }

                    listActId.RemoveAll(item => item > Convert.ToInt32(activityId));

                    listActId.Reverse();
                    listActId = listActId.Select(x => x).Distinct().ToList();

                    foreach (int actId in listActId)
                    {
                        log.LogInformation("Approval Notes involving act id: " + actId.ToString());
                    }

                    var budgetHolderWording = string.Empty;
                    var financeWording = string.Empty;
                    var FinanceOfficer = string.Empty;
                    var FinanceOfficeName = string.Empty;
                    FinanceOfficer = allDataCLaim.TravelOfficeId;
                    var Office = await financeOfficeSvc.GetByOfficeId(x => x.Id == FinanceOfficer);
                    FinanceOfficeName = Office.TravelOfficeName;


                    if (listActId.Contains(3))
                    {
                        trueBudgetHolder = username.ToLower();
                        if (!string.IsNullOrEmpty(allActivityUserMapped.FirstOrDefault().Substitute))
                        {
                            if (allActivityUserMapped.Where(c => c.ActivityID == 3 && c.Substitute.ToLower() == username.ToLower()).Select(c => c.Username).Any())
                            {
                                trueBudgetHolder = allActivityUserMapped.Where(c => c.ActivityID == 3 && c.Substitute.ToLower() == username.ToLower()).FirstOrDefault().Username.ToLower();
                            }
                        }

                        var cPerdiemObj = await cPerdiemCCSvc.Get(x => x.ClaimId.Equals(id) && x.IsActive == true);
                        var cExpenseObj = await cExpenseCCSvc.Get(x => x.ClaimId.Equals(id) && x.IsActive == true);
                        List<string> cAllccByUsername = new List<string>();
                        List<string> ccInvolveInContract = new List<string>();
                        List<string> listSpecialBudgetHolderUserId = new List<string>();
                        List<string> listSpecialWorkorder = new List<string>();
                        /*Active untuk budget holder special case*/
                        if (ActiveSpecialBudgetHolder)
                        {
                            if (!string.IsNullOrEmpty(configSpecialWorkorder))
                            {
                                listSpecialWorkorder = configSpecialWorkorder.Split(";").ToList();
                            }
                            if (!string.IsNullOrEmpty(configSpecialBudgetHolderUserId))
                            {
                                listSpecialBudgetHolderUserId = configSpecialBudgetHolderUserId.Split(";").ToList();
                            }
                            if (cPerdiemObj.Count() > 0)
                            {
                                List<ClaimPerdiemChargeCodeResponseDTO> perdiemCC = new List<ClaimPerdiemChargeCodeResponseDTO>();
                                //List<ClaimPerdiemChargeCodeResponseDTO> perdiemCCList = new List<ClaimPerdiemChargeCodeResponseDTO>();

                                foreach (ClaimPerdiemChargeCodeResponseDTO obj in cPerdiemObj)
                                {

                                    if (listSpecialWorkorder.Contains(obj.WorkOrderId) && obj.CostCenterId.Equals(configSpecialCostCenter) && obj.BudgetHolderUserId.ToUpper() == configSpecialCostCenterUserId)
                                    {
                                        ClaimPerdiemChargeCodeResponseDTO dt = new ClaimPerdiemChargeCodeResponseDTO();
                                        dt.CostCenterId = obj.CostCenterId;
                                        dt.WorkOrderId = obj.WorkOrderId;
                                        dt.EntityId = obj.EntityId;
                                        dt.BudgetHolderUserId = configSpecialCostCenterUserId;
                                        Log.Information("BudgetHolderUserId1 " + dt.BudgetHolderUserId);
                                        perdiemCC.Add(dt);
                                        foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                        {
                                            ClaimPerdiemChargeCodeResponseDTO dt2 = new ClaimPerdiemChargeCodeResponseDTO();
                                            dt2.CostCenterId = obj.CostCenterId;
                                            dt2.WorkOrderId = obj.WorkOrderId;
                                            dt2.EntityId = obj.EntityId;
                                            dt2.BudgetHolderUserId = additionalUserBH;
                                            Log.Information("BudgetHolderUserId2 " + dt2.BudgetHolderUserId);
                                            perdiemCC.Add(dt2);
                                        }
                                    }
                                    else
                                    {
                                        perdiemCC.Add(obj);
                                        Log.Information("BudgetHolderUserId3 " + obj.BudgetHolderUserId);
                                    }
                                }
                                foreach (ClaimPerdiemChargeCodeResponseDTO data in perdiemCC)
                                {
                                    Log.Information("Count perdiem cc : " + perdiemCC.Count());
                                    Log.Information("Budget Holder User Id : " + data.BudgetHolderUserId);
                                    Log.Information("True Budget Holder User Id : " + trueBudgetHolder.ToLower());
                                    if (data.BudgetHolderUserId.ToLower() == trueBudgetHolder.ToLower())
                                    {
                                        ccInvolveInContract.Add(data.CostCenterId + "." + data.WorkOrderId + "." + data.EntityId);
                                        Log.Information("Budget Holder Perdiem " + trueBudgetHolder + " " + data.CostCenterId + "." + data.WorkOrderId + "." + data.EntityId);
                                    }
                                    Log.Information("Budget Holder Perdiem " + trueBudgetHolder + " " + data.CostCenterId + "." + data.WorkOrderId + "." + data.EntityId);

                                }
                                ccInvolveInContract.RemoveAll(x => string.IsNullOrEmpty(x));
                            }
                            if (cExpenseObj.Count() > 0)
                            {
                                List<ClaimExpenseChargeCodeResponseDTO> expenseCC = new List<ClaimExpenseChargeCodeResponseDTO>();
                                //List<ClaimExpenseChargeCodeResponseDTO> expenseCCList = new List<ClaimExpenseChargeCodeResponseDTO>();
                                //bool userDoneExpenseCC = false;

                                foreach (ClaimExpenseChargeCodeResponseDTO obj in cExpenseObj)
                                {
                                    if (listSpecialWorkorder.Contains(obj.WorkOrderId) && obj.CostCenterId.Equals(configSpecialCostCenter) && obj.BudgetHolderUserId.ToUpper() == configSpecialCostCenterUserId)
                                    {
                                        obj.CostCenterId = obj.CostCenterId;
                                        obj.WorkOrderId = obj.WorkOrderId;
                                        obj.EntityId = obj.EntityId;
                                        obj.BudgetHolderUserId = configSpecialCostCenterUserId;
                                        expenseCC.Add(obj);

                                        foreach (var additionalUserBH in listSpecialBudgetHolderUserId)
                                        {
                                            obj.CostCenterId = obj.CostCenterId;
                                            obj.WorkOrderId = obj.WorkOrderId;
                                            obj.EntityId = obj.EntityId;
                                            obj.BudgetHolderUserId = additionalUserBH;
                                            expenseCC.Add(obj);
                                        }
                                    }
                                    else
                                    {
                                        expenseCC.Add(obj);
                                    }
                                }
                                foreach (ClaimExpenseChargeCodeResponseDTO obj in expenseCC)
                                {
                                    if (obj.BudgetHolderUserId.ToLower() == trueBudgetHolder.ToLower())
                                    {
                                        ccInvolveInContract.Add(obj.CostCenterId + "." + obj.WorkOrderId + "." + obj.EntityId);
                                        Log.Information("Budget Holder Expense : " + trueBudgetHolder + " " + obj.CostCenterId + "." + obj.WorkOrderId + "." + obj.EntityId);
                                    }
                                }
                                ccInvolveInContract.RemoveAll(x => string.IsNullOrEmpty(x));
                            }
                        }
                        else
                        {
                            if (cPerdiemObj.Count() > 0)
                            {
                                foreach (ClaimPerdiemChargeCodeResponseDTO obj in cPerdiemObj)
                                {
                                    if (obj.BudgetHolderUserId.ToLower() == trueBudgetHolder)
                                    {
                                        ccInvolveInContract.Add(obj.CostCenterId + "." + obj.WorkOrderId + "." + obj.EntityId);
                                    }
                                }
                            }
                            if (cExpenseObj.Count() > 0)
                            {
                                foreach (ClaimExpenseChargeCodeResponseDTO obj in cExpenseObj)
                                {
                                    if (obj.BudgetHolderUserId.ToLower() == trueBudgetHolder)
                                    {
                                        ccInvolveInContract.Add(obj.CostCenterId + "." + obj.WorkOrderId + "." + obj.EntityId);
                                    }
                                }
                            }
                        }
                        if (ccInvolveInContract != null)
                        {
                            if (ccInvolveInContract.Count > 0)
                            {
                                Log.Information("Budget Holder All : " + ccInvolveInContract.Distinct());
                                foreach (string a in ccInvolveInContract.Distinct())
                                {
                                    budgetHolderWording += "<li>";
                                    budgetHolderWording += "Budget holder of " + a;
                                    budgetHolderWording += "</li>";
                                }
                            }
                        }
                    }

                    Dictionary<int, string> dictOfActivity = new Dictionary<int, string>
                    {
                        {1, "Submission/Resubmission" },
                        {2, "Finance Office " + FinanceOfficeName },
                        {3, budgetHolderWording }
                    };

                    Dictionary<int, string> dictOfRole = new Dictionary<int, string>
                    {
                        {1, "Initiator" },
                        {2, "Finance Office " + FinanceOfficeName + " Approval" },
                        {3, "Budget Holder Approval" },

                    };

                    foreach (int i in listActId)
                    {
                        string act = dictOfActivity.Where(dActId => dActId.Key == i).Select(dActId => dActId.Value).FirstOrDefault();
                        string role = dictOfRole.Where(dRole => dRole.Key == i).Select(dRole => dRole.Value).FirstOrDefault();

                        if (!string.IsNullOrEmpty(act))
                        {
                            listStringActivity.Add(act);
                        }

                        if (!string.IsNullOrEmpty(role))
                        {
                            listStringRole.Add(role);
                        }
                    }

                    if (isReAction)
                    {

                    }

                    if (listStringActivity.Count > 0)
                    {
                        foreach (string act in listStringActivity)
                        {
                            if (!act.Contains("</li>"))
                            {
                                _actDesc += "<li>";
                                _actDesc += act;
                                _actDesc += "</li>";
                            }
                            else
                            {
                                _actDesc += act;
                            }
                        }
                    }

                    if (listStringRole.Count > 0)
                    {
                        _actName = string.Join("; ", listStringRole.ToArray());
                    }
                }

                _actDesc += "</ul>";

                ret.ApprovalNotesWording = _actDesc; //Masuk ke Approval Notes
                ret.Role = _actName; //Masuk ke Role di Comment

                return ret;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

        public async Task<List<string>> GetRegion(string travelOfficeId)
        {
            try
            {
                List<string> listUser = new List<string>();

                var cFinanceObj = await financeOfficerSvc.Get(x => x.TravelOfficeId.Equals(travelOfficeId) && x.Status == true);

                if (cFinanceObj.Count() > 0)
                {
                    foreach (var obj in cFinanceObj.Distinct())
                    {
                        listUser.Add(obj.UserId);
                        Log.Information("Finance User :" + obj.UserId);
                    }
                }

                listUser.RemoveAll(x => string.IsNullOrEmpty(x));
                listUser = listUser.Distinct().ToList();
                return listUser;
            }
            catch (Exception x)
            {
                ErrorServiceHandler(x);
                throw;
            }
        }

    }


}
