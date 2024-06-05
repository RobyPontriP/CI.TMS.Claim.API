using CI.TMS.Claim.API.DTOs;
using CI.TMS.Claim.API.Helper;
using CI.TMS.Claim.API.Persistence;
using CI.TMS.General;
using CI.TMS.General.Models.DTOs;
using myTree.MicroService.Helper;
using Newtonsoft.Json;
using static CI.TMS.Claim.API.Helper.Variable;
using System.Collections.Specialized;
using CI.TMS.General.Models;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.DTOs.Request;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace CI.TMS.Claim.API.Services.K2
{
    public class GeneralK2Service: BaseService
    {
        private K2APIBaseDataDTO baseWfConfig;
        private K2APILibrary k2APIServices;
        ClaimK2Service claimK2Svc;
        K2ActivityUserListService k2ActivityUserListSvc;
        K2ApproveStateService approveStateSvc;
        private string k2Folder = Variable.Configuration["K2Workflow:Folder"];
        private string k2ProcessName = Variable.Configuration["K2Workflow:ProcessName"];
        private string k2Folio = Variable.Configuration["K2Workflow:Folio"];
        private string logPath = Variable.Configuration["K2Workflow:LogPath"];
        private string endPoint = Variable.Configuration["K2Workflow:Endpoint"];
        private string apiKey = Variable.Configuration["K2Workflow:APIKey"];

        public GeneralK2Service(
            ClaimContext _context,
            ClaimK2Service _hrK2Svc,
            K2ActivityUserListService _k2ActivityUserListSvc,
            K2ApproveStateService _approveStateSvc,
            IHttpContextAccessor _httpContextAccessor,
            ILogger<BaseService> _log)
            : base(_context, _httpContextAccessor, _log)
        {
            baseWfConfig = new K2APIBaseDataDTO();
            baseWfConfig.k2ApiEndPoint = endPoint;
            baseWfConfig.baseLogPath = logPath;
            baseWfConfig.k2ApiKey = apiKey;
            baseWfConfig.isUseLog = true;
            k2APIServices = new K2APILibrary(baseWfConfig);
            claimK2Svc = _hrK2Svc;
            k2ActivityUserListSvc = _k2ActivityUserListSvc;
            approveStateSvc = _approveStateSvc;
        }

        public async Task<APIResponse<Response>> Submit(K2SubmitRequestDTO objToSend)
        {
            try
            {
                objToSend.ProcessName = k2ProcessName;
                objToSend.Folder = k2Folder;
                objToSend.Folio = k2Folio + objToSend.ID;

                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.Submit(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Submit K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Submit K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> SubmitAsync(K2SubmitRequestDTO objToSend)
        {
            try
            {
                objToSend.ProcessName = k2ProcessName;
                objToSend.Folder = k2Folder;
                objToSend.Folio = k2Folio + objToSend.ID;

                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = await k2APIServices.SubmitAsync(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Submit K2 async success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Submit K2 async failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Resubmit(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Resubmit;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);
                log.LogInformation("Testing Resubmit " + sentdatalog);
                log.LogInformation("Resubmit Action API K2" + objToSend.ActionName + " SN: " + objToSend.SN + "Username k2 : " + objToSend.Username + " Data : "+ objToSend.Data);


                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Resubmit K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Resubmit K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Revise(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Revise;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Revise K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Revise K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Reject(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Reject;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Reject K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Reject K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Approve(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Approve;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                log.LogInformation("Testing Approve " + sentdatalog);
                log.LogInformation("Approve Action API K2" + objToSend.ActionName+ " SN: "+objToSend.SN+ "Usernamek2 : "+ objToSend.Username+ objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Approve K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Approve K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Verify(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Verify;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Verify K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Verify K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Recommend(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Recommend;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Recommend K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Recommend K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Upload(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Upload;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Upload K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Upload K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> Redirect(K2ApprovalActionRequestDTO objToSend)
        {
            try
            {
                objToSend.ActionName = WorkflowCommand.Redirect;
                var sentdatalog = JsonConvert.SerializeObject(objToSend);

                var k2Result = k2APIServices.ApprovalAction(objToSend);
                if (k2Result.statusCode == StatusCodes.Status200OK)
                {
                    return new APIResponse<Response>()
                    {
                        success = true,
                        status = k2Result.statusCode,
                        message = "API general Redirect K2 success",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
                else
                {
                    return new APIResponse<Response>()
                    {
                        success = false,
                        status = k2Result.statusCode,
                        message = "API general Redirect K2 failed",
                        additionalInfo = sentdatalog,
                        data = k2Result
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<APIResponse<Response>> UpdateProcessData(ClaimDataResponseDTO submissionResDTO, string Sn, string actId)
        {
            try
            {
                Dictionary<string, object> redirObj = new Dictionary<string, object>();
                string budgetHolderUserId = string.Empty;
                string financeOfficerUserId = string.Empty;
                var budgetHolderData = await claimK2Svc.GetBudgetHolderUser(submissionResDTO.Id);
                var financeOfficerData = await claimK2Svc.GetFinanceUser(submissionResDTO.TravelOfficeId, submissionResDTO.CreatedBy, submissionResDTO.TravelerUserId);
                if (actId == "3")
                {
                    if (financeOfficerData.Count > 0)
                    {
                        financeOfficerUserId = string.Join(";", financeOfficerData);
                    }
                    redirObj.Add("FinanceUser", financeOfficerUserId);
                    redirObj.Add("FinanceState", 0);
                } else if (actId == "2")
                {
                    if (budgetHolderData.Count > 0)
                    {
                        budgetHolderUserId = string.Join(";", budgetHolderData);
                    }
                    redirObj.Add("BudgetHolderUser", budgetHolderUserId);
                    redirObj.Add("BudgetHolderState", 0);
                }              
                

                string instanceId = "";
                instanceId = Sn.Substring(0, Sn.IndexOf("_"));
                K2UpdateProcessDataRequestDTO updateRequestDTO = new K2UpdateProcessDataRequestDTO();
                updateRequestDTO.data = redirObj;

                var param = new NameValueCollection()
                {
                    { "id", instanceId }
                };

                var sentdatalog = JsonConvert.SerializeObject(updateRequestDTO);

                var k2ResultUpdate = k2APIServices.UpdateProcessData(param, updateRequestDTO);
                if (k2ResultUpdate.statusCode == StatusCodes.Status200OK)
                {
                    List<int?> actIdToDelete = new List<int?>() { 3 };
                    var existingUserObj = await k2ActivityUserListSvc.Get(x => x.RelevantId.Equals(submissionResDTO.Id.ToString())
                                                                                    && x.Folder.Equals(Configuration["K2Workflow:Folder"])
                                                                                    && x.ProcessName.Equals(Configuration["K2Workflow:ProcessName"])
                                                                                    && actIdToDelete.Contains(x.ActivityId));
                    var listexistingUserObj = existingUserObj.OrderByDescending(ord => ord.SeqNo).ToList();
                    var latestSeqNo = listexistingUserObj.FirstOrDefault()?.SeqNo;

                    var currseqno = await k2ActivityUserListSvc.Get(x => x.RelevantId.Equals(submissionResDTO.Id.ToString())
                                                                                    && x.Folder.Equals(Configuration["K2Workflow:Folder"])
                                                                                    && x.ProcessName.Equals(Configuration["K2Workflow:ProcessName"])
                                                                                    );
                    var listCurrlatesSeqno = currseqno.OrderByDescending(ord => ord.SeqNo).ToList();
                    var currlatesSeqno = listCurrlatesSeqno.FirstOrDefault()?.SeqNo;
                    


                    if (listexistingUserObj.Count() > 0)
                    {
                        foreach (K2ActivityUserListResponseDTO obj in listexistingUserObj.Where(x => x.SeqNo.Equals(latestSeqNo)))
                        {
                            await k2ActivityUserListSvc.HardDelete(obj.Id);
                        }
                    }
                
                    foreach (string str in budgetHolderData)
                    {
                        var newCouUsertoAdd = new K2ActivityUserListRequestDTO();
                        newCouUsertoAdd.ActivityId = 3;
                        newCouUsertoAdd.ActivityUser = str;
                        newCouUsertoAdd.Folder = Configuration["K2Workflow:Folder"];
                        newCouUsertoAdd.ProcessName = Configuration["K2Workflow:ProcessName"];
                        newCouUsertoAdd.RelevantId = submissionResDTO.Id.ToString();
                        newCouUsertoAdd.SeqNo = currlatesSeqno;
                        newCouUsertoAdd.ActivityName = string.Empty;

                        await k2ActivityUserListSvc.Add(newCouUsertoAdd);
                    }

                    return new APIResponse<General.Models.Response>()
                    {
                        success = false,
                        status = k2ResultUpdate.statusCode,
                        message = "API Update data field is success",
                        additionalInfo = sentdatalog,
                        data = k2ResultUpdate
                    };
                }
                else 
                {
                    return new APIResponse<General.Models.Response>()
                    {
                        success = false,
                        status = k2ResultUpdate.statusCode,
                        message = "API Update data field is failed",
                        additionalInfo = sentdatalog,
                        data = k2ResultUpdate
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


        public async Task<SnValidatorResponseDTO> SnValidator(SnValidatorRequestDTO requestDTO)
        {
            try
            {
                var id = requestDTO.Id;
                var userid = requestDTO.UserId;
                var processName = string.IsNullOrEmpty(requestDTO.ProcessName) ? k2ProcessName : requestDTO.ProcessName;
                var folder = string.IsNullOrEmpty(requestDTO.Folder) ? k2Folder : requestDTO.Folder;
                var sn = requestDTO.Sn;
                string snFromUrl = sn;
                bool res = true;
                SnValidatorResponseDTO ret = new SnValidatorResponseDTO();
                userid = string.IsNullOrEmpty(userid) ? "--" : userid; // diberi logic ini karena jika mengirim string kosong maka get current activity tetap akan memberi hasil pencarian berdasar dengan id, prcessname dan folder

                var param = new NameValueCollection()
                {
                   { "id", id },
                   { "processName", processName },
                   { "folder", folder },
                   { "activityUser", userid }
                };

                Response resCurrentActivity = k2APIServices.GetCurrentActivity(param);

                if (resCurrentActivity.data != null)
                {
                    try
                    {
                        K2GetCurrentActivityResponseDTO responseK2 = (K2GetCurrentActivityResponseDTO)resCurrentActivity.data;
                        log.LogInformation("debug SnValidator id:" + id + ". After assign data to responseK2");

                        if (responseK2.Message != null)
                        {
                            log.LogInformation("debug SnValidator id:" + id + ". responseK2.Message is not null");
                            log.LogInformation("debug SnValidator id:" + id + ". responseK2.Message : " + JsonConvert.SerializeObject(responseK2.Message));
                            var jObj = JArray.Parse(JsonConvert.SerializeObject(responseK2.Message));
                            //List<K2GetCurrentActivityMessageDTO> listOfCurrentAct = responseK2.Message.Cast<K2GetCurrentActivityMessageDTO>().ToList();
                            log.LogInformation("debug SnValidator id:" + id + ". After assign responseK2.Message to jObj");

                            var snList = new List<string>();
                            if (jObj.Count == 0)
                            {
                                log.LogInformation("debug SnValidator id:" + id + ". jObj.Count == 0");
                                res = false;
                            }
                            else
                            {
                                log.LogInformation("debug SnValidator id:" + id + ". jObj.Count != 0");
                                snList = jObj.Children().Select(x => (string)x["SN"]).ToList();
                                if (!snList.Contains(snFromUrl))
                                {
                                    res = false;
                                }
                            }
                        }
                        else
                        {
                            log.LogInformation("debug SnValidator id:" + id + ". Error code 001. responseK2.Message is null");
                            throw new Exception("Endpoint for checking serial number of workflow seems doesn't respond properly. Please resubmit or if the issue persist please contact administrator.");
                        }
                    }
                    catch (Exception x)
                    {
                        log.LogInformation("debug SnValidator id:" + id + ". Error code 002. catch(). Error: " + x.Message + " | " + x.InnerException + " | " + x.StackTrace);
                        throw new Exception("Endpoint for checking serial number of workflow seems doesn't respond properly. Please resubmit or if the issue persist please contact administrator.");
                    }
                }
                else
                {
                    log.LogInformation("debug SnValidator id:" + id + ". Error code 003. resCurrentActivity.data is null");
                    throw new Exception("Endpoint for checking serial number of workflow seems doesn't respond properly. Please resubmit or if the issue persist please contact administrator.");
                }

                ret.IsSnValid = res;

                return ret;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<WorkflowExistResponseDTO> WorkflowExistValidator(WorkflowExistRequestDTO reqDto)
        {
            try
            {
                var folio = k2Folio + reqDto.RequestId.ToString();
                bool res = true;
                WorkflowExistResponseDTO ret = new WorkflowExistResponseDTO();

                var param = new NameValueCollection()
                {
                   { "folio", folio },
                };

                log.LogInformation("debug wfIsExist folio:" + folio);
                Response resFolioExist = k2APIServices.IsFolioExist(param);
                log.LogInformation("response: " + JsonConvert.SerializeObject(resFolioExist));

                //var text = File.ReadAllText("IsFolioExist.json");
                //Response resFolioExist = JsonConvert.DeserializeObject<Response>(text);
                //log.LogInformation("response: " + JsonConvert.SerializeObject(resFolioExist));

                K2IsFolioExistResponseDTO resp = new K2IsFolioExistResponseDTO();

                if (resFolioExist.data != null)
                {
                    log.LogInformation("debug wfIsExist folio:" + folio + ". response.data is not null.");
                    resp = (K2IsFolioExistResponseDTO)resFolioExist.data;

                    log.LogInformation("debug wfIsExist folio:" + folio + ". Get final reposponse from K2.");
                    if (resp != null)
                    {
                        bool responseMessage = (bool)resp.Message;
                        log.LogInformation("debug wfIsExist folio:" + folio + ". Final reposponse from K2 is not null.");
                        if (responseMessage != null)
                        {
                            res = responseMessage;
                        }
                    }
                }
                else
                {
                    throw new Exception("Endpoint for checking existing data of workflow seems doesn't respond properly. Please resubmit or if the issue persist please contact administrator.");
                }

                ret.IsWorkflowExist = res;

                return ret;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}
