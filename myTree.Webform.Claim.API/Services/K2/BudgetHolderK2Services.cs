using System;
using System.Collections.Generic;
using System.Text;
using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using CI.TMS.General;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Serilog;
using System.Net.NetworkInformation;

namespace CI.TMS.Claim.API.Services.K2
{
    public class BudgetHolderK2Services : BaseService
    {
        #region Properties
        public BudgetHolderK2Services(
              ClaimContext _context,
              IHttpContextAccessor _httpContextAccessor,
              ILogger<BaseService> _log
              ) : base(_context, _httpContextAccessor, _log)
        {

        }

        List<K2ActivityUser> unApproveUsers { get; set; }
        #endregion

        public IEnumerable<Employee> GetInitiator(string id)
        {
            //return addDummyUser("Cifor-Sysdev47");

            var initiator = context.Employee.Where(x => x.EmpId == id).Select(x => x.EmpUserId).FirstOrDefault();

            Employee emp = new Employee();
            emp.EmpUserId = initiator;

            List<Employee> listEmp = new List<Employee>();
            listEmp.Add(emp);

            IEnumerable<Employee> listPi = listEmp;

            return listPi;

        }

        private string mapUser(int activityId)
        {
            string ret = string.Join(';', unApproveUsers.Where(x => x.ActivityID.Equals(activityId)).Select(x => x.Username).ToList());
            return ret;
        }

        #region Get WF User
        public async Task<List<K2BudgetHolderResponseDTO>> GetBudgetHolderUser(Guid id, Expression<Func<K2BudgetHolderResponseDTO, bool>>? predicate = null)
        {

            var BudgetHolderPerdiem = context.ClaimPerdiemChargeCode
                                      .SelectMany(perdiem => context.CostCenter.Where(costCenter => costCenter.Id == perdiem.CostCenterId).DefaultIfEmpty(), (perdiem, costCenter) => new { perdiem, costCenter })
                                      .SelectMany(item => context.Employee
                                      .Where(employee => employee.RowId == item.costCenter.BudgetHolderId)
                                      .DefaultIfEmpty(), (item, employee) => new K2BudgetHolderResponseDTO
                                      {
                                          UserId = employee.EmpUserId,
                                          ClaimId = item.perdiem.ClaimId,
                                          EmpStatus = employee.EmpStatus
                                      })
                                      .Where(result => result.ClaimId == id && result.EmpStatus == "CONFIRMED")
                                      .OrderBy(result => result.UserId)
                                      .AsNoTracking()
                                      .ToList();

            if (BudgetHolderPerdiem == null)
            {
                // Handle the case where BudgetHolderPerdiem is null or empty
            }

            var BudgetHolderExpense = context.ClaimExpenseChargeCode.SelectMany(expense => context.CostCenter.Where(costCenter => costCenter.Id == expense.CostCenterId).DefaultIfEmpty(), (expense, costCenter) => new { expense, costCenter })
                                     .SelectMany(item => context.Employee.Where(employee => employee.RowId == item.costCenter.BudgetHolderId).DefaultIfEmpty(),
                                     (item, employee) => new K2BudgetHolderResponseDTO
                                     {
                                         UserId = employee.EmpUserId,
                                         ClaimId = item.expense.ClaimId,
                                         EmpStatus = employee.EmpStatus
                                     })
                                    .Where(result => result.ClaimId == id && result.EmpStatus == "CONFIRMED")
                                    .OrderBy(result => result.UserId)
                                    .AsNoTracking()
                                    .ToList(); // Removed ToListAsync()

            var combinedResults = BudgetHolderPerdiem.Union(BudgetHolderExpense, new K2BudgetHolderResponseDTOComparer());

            return combinedResults.Distinct(new K2BudgetHolderResponseDTOComparer()).ToList();

        }
        public async Task<IEnumerable<K2BudgetHolderResponseDTO>> GetFinanceUser(Guid id)
        {
            //dummy data finance 
            return await (from cec in context.ClaimExpenseChargeCode
                          join cc in context.CostCenter on cec.CostCenterId equals cc.Id
                          join em in context.Employee on cc.BudgetHolderId equals em.RowId
                          select new
                          {
                              UserId = em.EmpUserId,
                              cec.ClaimId,
                              em.EmpStatus

                          }).Where(x => x.ClaimId == id && x.EmpStatus == "CONFIRMED").Project().To<K2BudgetHolderResponseDTO>().ToListAsync();
        }


        #endregion

        public class K2BudgetHolderResponseDTOComparer : IEqualityComparer<K2BudgetHolderResponseDTO>
        {
            public bool Equals(K2BudgetHolderResponseDTO x, K2BudgetHolderResponseDTO y)
            {
                if (x == null || y == null)
                    return false;

                return x.UserId == y.UserId && x.ClaimId == y.ClaimId && x.EmpStatus == y.EmpStatus;
            }
            public int GetHashCode(K2BudgetHolderResponseDTO obj)
            {
                return HashCode.Combine(obj.UserId, obj.ClaimId, obj.EmpStatus);
            }
        }

        //public async Task<ApprovalNotes> GetLoAPartnerApvNotes(int activityId, string username, Guid id, string accessManagementToken)
        //{
        //    Log.Information("LoA Partner Approval Notes for: " + id.ToString() + ". ActivityID: " + activityId.ToString() + ". Username: " + username.ToString());

        //    ApprovalNotes actObj = new ApprovalNotes();

        //    List<int> listActId = new List<int>();
        //    int maxActId = 0;
        //    List<string> listStringActivity = new List<string>();
        //    List<string> listStringRole = new List<string>();
        //    string _actDesc = "-";
        //    string _actName = "-";

        //    string action = "";
        //    string actionForAct3 = "";
        //    string actionForAct12 = "";
        //    bool isResubmit = false;

        //    ApproveState apvState = _applicationContext.ApproveStates.Where(y => y.Module == "LoAPartner" && y.Username.ToLower() == username.ToLower() && y.ActivityId == activityId.ToString() && y.State == 0 && y.RelevantId == id).FirstOrDefault();

        //    if (apvState != null)
        //    {
        //        if (apvState.Id != Guid.Empty || apvState.Id != null)
        //        {
        //            isResubmit = true;
        //            Log.Information("LoA Partner Approval Notes: Is ReAction. With ApvStateId = " + apvState.Id.ToString());
        //        }
        //    }

        //    if (isResubmit)
        //    {
        //        action = "re-";
        //        actionForAct3 = "Re-";
        //        actionForAct12 = "re-";
        //    }

        //    if (activityId == 3 || activityId == 13)
        //    {
        //        action += "verifying";
        //    }
        //    else if (activityId == 9 || activityId == 10 || activityId == 11)
        //    {
        //        action += "approving";
        //    }
        //    else if (activityId == 12)
        //    {
        //        action += "uploading";
        //    }
        //    else
        //    {
        //        action += "recommending";
        //    }

        //    Log.Information("LoA Partner Approval Notes: Action = " + action.ToString());

        //    if (activityId != 3)
        //    {
        //        if (activityId != 12)
        //        {
        //            _actDesc = "You are " + action + " for: ";
        //        }
        //        else
        //        {
        //            _actDesc = "You are " + actionForAct12 + "uploading the signed copy of the agreement. Click on the \"Upload signed document\" button in the main document section to replace it with the signed copy of the agreement.";
        //        }
        //    }
        //    else
        //    {
        //        _actDesc = "You are:";
        //    }

        //    Log.Information("LoA Partner Approval Notes: Activity Desc = " + _actDesc.ToString());
        //    _actDesc += "<ul>";

        //    List<K2ActivityUser> allActivityUser = await GetAllActivityUser(id, accessManagementToken);

        //    //TeamLeader mdObj = _applicationContext.TeamLeaders.Where(x => x.TeamId.ToUpper() == "ODGFMD".ToUpper()).FirstOrDefault(); //RNASI
        //    //string DG = "";
        //    //if (mdObj != null)
        //    //{
        //    //    DG = mdObj.TeamLeaderUserId; //RNASI
        //    //}

        //    //TeamLeader edObj = _applicationContext.TeamLeaders.Where(x => x.TeamId.ToUpper() == "ODGFED".ToUpper()).FirstOrDefault(); //TSIMONS
        //    //string ED = "";
        //    //if (edObj != null)
        //    //{
        //    //    ED = edObj.TeamLeaderUserId; //TSIMONS
        //    //}

        //    //if (username.ToLower() == ED.ToLower() && activityId == 8)
        //    //{
        //    //    username = DG;
        //    //}

        //    foreach (K2ActivityUser user in allActivityUser)
        //    {
        //        if (user.Username.ToLower() == username.ToLower())
        //        {
        //            listActId.Add(user.ActivityID);
        //        }
        //    }

        //    Log.Information("LoA Partner Approval Notes : List Act ID Count = " + listActId.Count.ToString());

        //    if (listActId.Count > 0 || activityId == 8)
        //    {

        //        listActId.RemoveAll(item => item == 0);
        //        listActId.RemoveAll(item => item == 1);

        //        if (activityId == 13) //Jika di Activity Grant Officer, maka tidak perlu menyertakan Activity PMU
        //        {
        //            listActId.RemoveAll(item => item == 6);
        //            listActId.RemoveAll(item => item == 12);
        //        }

        //        if (activityId == 9 || activityId == 10 || activityId == 11) //Activity MD DG, DCS, Thematic Unit Lead tidak perlu memperlihatkan apv notes dari ILMG
        //        {
        //            listActId.RemoveAll(item => item == 8);

        //        }
        //        if (activityId == 9 || activityId == 7)
        //        {
        //            if (listActId.Contains(4))
        //            {
        //                listActId.RemoveAll(item => item == 4);
        //            }
        //        }

        //        if (activityId == 8) // Activity ILMG perlu cek ulang apakah sudah ada data ILMG atau blm di list ini
        //        {
        //            if (!listActId.Where(x => x == 8).Any())
        //            {
        //                listActId.Add(activityId);
        //            }
        //            listActId.RemoveAll(item => item < Convert.ToInt32(activityId));
        //        }

        //        if (activityId != 5)
        //        {
        //            //listActId.RemoveAll(item => item > Convert.ToInt32(activityId));

        //            if (activityId == 7) //activity thematic tidak perlu menampilkan activity DCS
        //            {
        //                if (listActId.Where(x => x == 5).Any())
        //                {
        //                    listActId.RemoveAll(item => item == 5);
        //                }
        //            }

        //            listActId.RemoveAll(item => item > Convert.ToInt32(activityId));
        //        }
        //        else //di activity id 5 (DCS recommendation) perlu cek ke activityId 7 ada participant atau tidak, kalau ada dimasukan
        //        {
        //            bool isThematicExist = false;
        //            if (listActId.Where(x => x == 7).Any())
        //            {
        //                isThematicExist = true;
        //            }

        //            listActId.RemoveAll(item => item > Convert.ToInt32(activityId));

        //            if (isThematicExist)
        //            {
        //                listActId.Add(7);
        //            }
        //        }

        //        if (!listActId.Where(x => x == activityId).Any())
        //        {
        //            listActId.Add(activityId);
        //        }

        //        listActId.Reverse();

        //        var query = (from g in _applicationContext.LoaPartners
        //                     join lt in _applicationContext.LoaPartnerTeams on g.Id equals lt.LoaPartnerId
        //                     join t in _applicationContext.Teams on lt.TeamId equals t.TeamId
        //                     where lt.RoleId.ToUpper() == "LEAD" && g.Id == id && lt.IsActive == true
        //                     select new
        //                     { t.Name, t.TeamId }).FirstOrDefault();

        //        string team = query.Name;
        //        string teamId = query.TeamId;
        //        string verifying = actionForAct3 != "" ? "verifying" : "Verifying";
        //        Dictionary<int, string> dictOfActivity = new Dictionary<int, string>
        //            {
        //                {1, "Submission/Resubmission" },
        //                {2, "Director, Innovation, Investment and Impact" },
        //                //{3, "<li>"+actionForAct3+"verifying some of the Partner LoA information</li><li>"+actionForAct3+"verifying some of the project information</li>" },
        //                {3, actionForAct3+ verifying +" some of the Partner LoA information" },
        //                {4, "Team Leader, Finance or Chief Financial Officer" },
        //                {5, "Director, Corporate Services" },
        //                {6, "Team Leader, Project Management Unit" },
        //                {7, "Team Leader, " + team + "" },
        //                {8, "<li>Integrated Leadership and Management Group</li><li>Due Diligence process by clicking the [Partner Due Diligence] to open the due diligence information</li>" },
        //                {9, "Team Leader, " + team + "" },
        //                {10, "Director, Corporate Services" },
        //                {11, "Director General" },
        //                //{12, "" },
        //                {13, "Project Management System Officer" },
        //            };

        //        Dictionary<int, string> dictOfRole = new Dictionary<int, string>
        //            {
        //                {1, "Initiator" },
        //                {2, "Director, Innovation, Investment and Impact Recommendation" },
        //                {3, "Finance Verification" },
        //                {4, "Team Leader Finance Recommendation" },
        //                {5, "Director, Corporate Services Recommendation" },
        //                {6, "Team Leader, Project Management Unit Recommendation" },
        //                {7, "Thematic Unit Leader or Country Coordinator Recommendation" },
        //                {8, "Integrated Leadership and Management Group Recommendation" },
        //                {9, "Thematic Unit Leader or Country Coordinator Approval" },
        //                {10, "Director, Corporate Services Approval" },
        //                {11, "Managing Director Approval" },
        //                {12, "Upload Signed Copy of Agreement" },
        //                {13, "Project Management System Officer Verification" },
        //            };

        //        foreach (int i in listActId)
        //        {
        //            string act = dictOfActivity.Where(dActId => dActId.Key == i).Select(dActId => dActId.Value).FirstOrDefault();
        //            string role = dictOfRole.Where(dRole => dRole.Key == i).Select(dRole => dRole.Value).FirstOrDefault();

        //            if (!string.IsNullOrEmpty(act))
        //            {
        //                listStringActivity.Add(act);
        //            }

        //            if (!string.IsNullOrEmpty(role))
        //            {
        //                listStringRole.Add(role);
        //            }

        //            act = string.IsNullOrEmpty(act) ? "" : act;
        //            role = string.IsNullOrEmpty(role) ? "" : role;

        //            Log.Information("LoA Partner Approval Notes: Activity Id= " + i.ToString() + ", Notes= " + act.ToString() + ", Role= " + role.ToString());
        //        }

        //        #region Cek adanya Perubahan Data yang menjadi parameter reset WF
        //        if (isResubmit)
        //        {
        //            #region Penambahan Wording Apv Notes Untuk Resubmission dan Jika ada perubahan data
        //            Dictionary<string, string> oldGrantData = _dataLogService.GetListOlderDataLog(id, "LoAPartner");
        //            Dictionary<string, string> newGrantData = _dataLogService.GetListCurrentDataLog(id, "LoAPartner");
        //            List<DifferentData> listDiff = new List<DifferentData>();

        //            foreach (var oldData in oldGrantData)
        //            {
        //                if (oldData.Value.ToLower() != newGrantData.FirstOrDefault(x => x.Key == oldData.Key).Value.ToLower())
        //                {
        //                    DifferentData difData = new DifferentData();
        //                    difData.FieldName = oldData.Key;
        //                    difData.OldValue = oldData.Value;
        //                    difData.NewValue = newGrantData.FirstOrDefault(x => x.Key == oldData.Key).Value;

        //                    listDiff.Add(difData);
        //                }
        //            }

        //            if (listDiff.Count > 0)
        //            {
        //                Log.Information("LoA Partner Approval Notes: Differences between new and old data detected.");
        //                foreach (DifferentData diff in listDiff)
        //                {
        //                    if (diff.FieldName.ToLower().Equals("PrincipalInvestigatorId".ToLower()))
        //                    {
        //                        string piNameOld = (from e in _applicationContext.Emmployees
        //                                            where e.Id == diff.OldValue
        //                                            select e.FullName).FirstOrDefault();
        //                        piNameOld = string.IsNullOrEmpty(piNameOld) ? "EmployeeDataNull" : piNameOld;

        //                        string piNameNew = (from e in _applicationContext.Emmployees
        //                                            where e.Id == diff.NewValue
        //                                            select e.FullName).FirstOrDefault();
        //                        piNameNew = string.IsNullOrEmpty(piNameNew) ? "EmployeeDataNull" : piNameNew;

        //                        Log.Information("LoA Partner Approval Notes: PrincipalInvestigatorId is Different. Old= " + piNameOld + " New= " + piNameNew);
        //                        listStringActivity.Add("Change of Principal investigator from " + piNameOld + " to " + piNameNew);
        //                    }
        //                    else if (diff.FieldName.ToLower().Equals("ThematicUnitId".ToLower()))
        //                    {
        //                        string oldTeam = (from t in _applicationContext.TeamLeaders
        //                                          where t.TeamId == diff.OldValue
        //                                          select t.Name).FirstOrDefault();
        //                        oldTeam = string.IsNullOrEmpty(oldTeam) ? "TeamDataNull" : oldTeam;

        //                        string newTeam = (from t in _applicationContext.TeamLeaders
        //                                          where t.TeamId == diff.NewValue
        //                                          select t.Name).FirstOrDefault();
        //                        newTeam = string.IsNullOrEmpty(newTeam) ? "TeamDataNull" : newTeam;

        //                        Log.Information("LoA Partner Approval Notes: ThematicUnitLeaderId is Different. Old= " + oldTeam + " New= " + newTeam);
        //                        listStringActivity.Add("Change of Thematic unit/Country office/Other unit from " + oldTeam + " to " + newTeam);
        //                    }
        //                    else if (diff.FieldName.ToLower().Equals("ApprovedBudgetInUsd".ToLower()))
        //                    {
        //                        try
        //                        {
        //                            #region Get Older Data
        //                            string oldApvBudgetUSD = string.Format("{0:N2}", Convert.ToDecimal(diff.OldValue));

        //                            string oldOriginalApvBudget = "-";
        //                            string x = oldGrantData.Where(x => x.Key.ToLower() == "OriginalApprovedBudget".ToLower()).Select(x => x.Value).FirstOrDefault();
        //                            if (!string.IsNullOrEmpty(x))
        //                            {
        //                                oldOriginalApvBudget = string.Format("{0:N2}", Convert.ToDecimal(x));
        //                            }

        //                            string oldExchangeRate = "-";
        //                            string y = oldGrantData.Where(x => x.Key.ToLower() == "ExchangeRate".ToLower()).Select(x => x.Value).FirstOrDefault();
        //                            if (!string.IsNullOrEmpty(y))
        //                            {
        //                                oldExchangeRate = y;
        //                            }

        //                            string oldCurrency = "-";
        //                            string z = oldGrantData.Where(x => x.Key.ToLower() == "Currency".ToLower()).Select(x => x.Value).FirstOrDefault();
        //                            if (!string.IsNullOrEmpty(z))
        //                            {
        //                                string currName = z;
        //                                oldCurrency = string.IsNullOrEmpty(currName) ? z : currName;
        //                            }
        //                            #endregion

        //                            #region Get Current Data
        //                            string newApvBudgetUSD = string.Format("{0:N2}", Convert.ToDecimal(diff.NewValue));

        //                            string newOriginalApvBudget = "-";
        //                            string xx = newGrantData.Where(x => x.Key.ToLower() == "OriginalApprovedBudget".ToLower()).Select(x => x.Value).FirstOrDefault();
        //                            if (!string.IsNullOrEmpty(xx))
        //                            {
        //                                newOriginalApvBudget = string.Format("{0:N2}", Convert.ToDecimal(xx));
        //                            }

        //                            string newExchangeRate = "-";
        //                            string yy = newGrantData.Where(x => x.Key.ToLower() == "ExchangeRate".ToLower()).Select(x => x.Value).FirstOrDefault();
        //                            if (!string.IsNullOrEmpty(yy))
        //                            {
        //                                newExchangeRate = yy; ;
        //                            }

        //                            string newCurrency = "-";
        //                            string zz = newGrantData.Where(x => x.Key.ToLower() == "Currency".ToLower()).Select(x => x.Value).FirstOrDefault();
        //                            if (!string.IsNullOrEmpty(zz))
        //                            {
        //                                string currName = zz;
        //                                newCurrency = string.IsNullOrEmpty(currName) ? zz : currName;
        //                            }
        //                            #endregion

        //                            Log.Information("LoA Partner Approval Notes: ApprovedBudgetInUsd is Different. Old= " + oldApvBudgetUSD + " New= " + newApvBudgetUSD);
        //                            listStringActivity.Add("<li>Partnership value:<ul><li>Change from " + oldCurrency.ToUpper() + " " + oldOriginalApvBudget + " with exchange rate " + oldExchangeRate + " equivalent to USD " + oldApvBudgetUSD + " to " + newCurrency.ToUpper() + " " + newOriginalApvBudget + " with exchange rate " + newExchangeRate + " equivalent to USD " + newApvBudgetUSD + "</li></ul></li>");
        //                        }
        //                        catch
        //                        {
        //                            listStringActivity.Add("Could Not Get/Format Approved Budget");
        //                        }
        //                    }
        //                    else if (diff.FieldName.ToLower().Equals("StartDate".ToLower()))
        //                    {
        //                        Log.Information("LoA Partner Approval Notes: StartDate is Different. Old= " + diff.OldValue + " New= " + diff.NewValue);

        //                        string strOldStartDate = diff.OldValue;
        //                        DateTime oldStartDate = DateTime.ParseExact(strOldStartDate, "dd/MM/yyyy", null);
        //                        string formatedOldStartDate = oldStartDate.ToString("dd MMMM yyyy");

        //                        string strNewStartDate = diff.NewValue;
        //                        DateTime newStartDate = DateTime.ParseExact(strNewStartDate, "dd/MM/yyyy", null);
        //                        string formatedNewStartDate = newStartDate.ToString("dd MMMM yyyy");

        //                        listStringActivity.Add("Change of Start Date from " + formatedOldStartDate + " to " + formatedNewStartDate);
        //                    }
        //                    else if (diff.FieldName.ToLower().Equals("EndDate".ToLower()))
        //                    {
        //                        Log.Information("LoA Partner Approval Notes: EndDate is Different. Old= " + diff.OldValue + " New= " + diff.NewValue);

        //                        string strOldEndDate = diff.OldValue;
        //                        DateTime oldEndDate = DateTime.ParseExact(strOldEndDate, "dd/MM/yyyy", null);
        //                        string formatedOldEndDate = oldEndDate.ToString("dd MMMM yyyy");

        //                        string strNewEndDate = diff.NewValue;
        //                        DateTime newEndDate = DateTime.ParseExact(strNewEndDate, "dd/MM/yyyy", null);
        //                        string formatedNewEndDate = newEndDate.ToString("dd MMMM yyyy");

        //                        listStringActivity.Add("Change of End Date from " + formatedOldEndDate + " to " + formatedNewEndDate);
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        #endregion

        //        #region Cek terkait First Payment di activity-activity tertentu
        //        List<int> listActForPaymentException = new List<int>();
        //        listActForPaymentException.Add(4);
        //        listActForPaymentException.Add(5);
        //        listActForPaymentException.Add(10);

        //        if (listActForPaymentException.Contains(activityId))
        //        {
        //            Domain.Entities.LoaPartnerPaymentTerm payTerm = (from a in _applicationContext.LoaPartnerPaymentTerms
        //                                                             where a.LoaPartnerId == id && a.PaymentOrderId.ToUpper() == "PAYORDER00".ToUpper() && a.IsActive == true
        //                                                             select a).FirstOrDefault();

        //            if (payTerm != null)
        //            {
        //                if (payTerm.Percentage > 20)
        //                {
        //                    if (payTerm.AmountInUSD >= 10000 && payTerm.AmountInUSD <= 20000)
        //                    {
        //                        if (activityId == 4)
        //                        {
        //                            listStringActivity.Add("For first payment more than 20%");
        //                        }
        //                    }
        //                    else if (payTerm.AmountInUSD > 20000)
        //                    {
        //                        if (activityId == 5 || activityId == 10)
        //                        {
        //                            listStringActivity.Add("For first payment more than 20%");
        //                        }
        //                    }
        //                }

        //            }

        //            #region Old Rule
        //            //Domain.Entities.LoaPartner loaPartnerObj = (from g in _applicationContext.LoaPartners
        //            //                                            where g.Id == id
        //            //                                            select g).FirstOrDefault();

        //            //bool isFirstPaymentException = false;
        //            //bool isBelowEqual20K = false;
        //            //bool isBetween20KtoEqual50K = false;
        //            //bool isAbove50K = false;

        //            //if (loaPartnerObj.IsFirstPaymentException == true)
        //            //{
        //            //    isFirstPaymentException = true;
        //            //}

        //            //if (loaPartnerObj.ApprovedBudgetInUSD <= 20000)
        //            //{
        //            //    isBelowEqual20K = true;
        //            //}
        //            //else if (loaPartnerObj.ApprovedBudgetInUSD > 20000 && loaPartnerObj.ApprovedBudgetInUSD <= 50000)
        //            //{
        //            //    isBetween20KtoEqual50K = true;
        //            //}
        //            //else if (loaPartnerObj.ApprovedBudgetInUSD > 50000)
        //            //{
        //            //    isAbove50K = true;
        //            //}

        //            //if (isFirstPaymentException)
        //            //{
        //            //    if (isBelowEqual20K)
        //            //    {
        //            //        if (activityId == 4)
        //            //        {
        //            //            listStringActivity.Add("for first payment 20%");
        //            //        }
        //            //    }
        //            //    else if (isBetween20KtoEqual50K)
        //            //    {
        //            //        if (activityId == 4)
        //            //        {
        //            //            listStringActivity.Add("for first payment 20%");
        //            //        }
        //            //        else if (activityId == 10)
        //            //        {
        //            //            listStringActivity.Add("for first payment 20%");
        //            //        }
        //            //    }
        //            //    else if (isAbove50K)
        //            //    {
        //            //        if (activityId == 4)
        //            //        {
        //            //            listStringActivity.Add("for first payment 20%");
        //            //        }
        //            //        else if (activityId == 5)
        //            //        {
        //            //            listStringActivity.Add("for first payment 20%");
        //            //        }
        //            //    }
        //            //}
        //            #endregion

        //        }
        //        #endregion

        //        if (listStringActivity.Count > 0)
        //        {
        //            foreach (string act in listStringActivity)
        //            {
        //                if (!act.Contains("</li>"))
        //                {
        //                    _actDesc += "<li>";
        //                    _actDesc += act;
        //                    _actDesc += "</li>";
        //                }
        //                else
        //                {
        //                    _actDesc += act;
        //                }
        //            }
        //        }

        //        if (listStringRole.Count > 0)
        //        {
        //            _actName = string.Join("; ", listStringRole.ToList());
        //            Log.Information("LoA Partner Approval Notes: Role Output:" + _actName);
        //        }

        //        //}
        //    }

        //    _actDesc += "</ul>";

        //    Log.Information("LoA Partner Approval Notes: Notes Output:" + _actDesc);

        //    actObj.actDesc = _actDesc; //Masuk ke Approval Notes
        //    actObj.actName = _actName; //Masuk ke Role di Comment

        //    return actObj;
        //}

    }
}
