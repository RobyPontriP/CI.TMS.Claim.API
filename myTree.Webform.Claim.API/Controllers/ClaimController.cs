using CI.TMS.Claim.API.Controllers;
using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.Domain.Entities.K2;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Helper;
using CI.TMS.Claim.API.Services;
using CI.TMS.Claim.API.Services.K2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myTree.MicroService.Helper;
using Serilog;
using static CI.TMS.Claim.API.Helper.Variable;

namespace myTree.Webform.Claim.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClaimController : Controller
    {

        private IConfiguration config;
        private ILogger<ClaimController> log;
        private IHttpContextAccessor httpContextAccessor;
        private ClaimDataService claimdataSvc;
        private ClaimSubmissionService claimSvc;
        private CommonService commonService;
        private SharePointHelper sharePointHelper;
        private ClaimService claimService;
        private ClaimDocumentService claimdocumentService;
        private ClaimSupportingDocumentService claimsupdocumentService;
        private ClaimBoardingPassDocumentService claimboardpassdocumentService;
        private ClaimExpenseService claimExpenseSvc;
        private ClaimK2Service claimK2Service;
        private ClaimJournalService claimJournalService;
        private ClaimAuditDataService claimAuditDataService;
        private GeneralK2Service genK2Svc;
        private string userId = string.Empty;
        public ClaimController(IConfiguration config,
             ILogger<ClaimController> log,
             ClaimDataService claimdataSvc,
             IHttpContextAccessor httpContextAccessor,
             ClaimSubmissionService claimSvc,
             CommonService commonService,
             ClaimService claimService,
             SharePointHelper sharePointHelper,
             ClaimDocumentService claimdocumentService,
             ClaimSupportingDocumentService claimsupdocumentService,
             ClaimBoardingPassDocumentService claimboardpassdocumentService,
             ClaimExpenseService claimExpenseSvc,
             ClaimJournalService claimJournalService,
             ClaimAuditDataService claimAuditDataService,
             ClaimK2Service claimK2Service,
             GeneralK2Service genK2Svc)
        {
            this.config = config;
            this.log = log;
            this.claimdataSvc = claimdataSvc;
            this.httpContextAccessor = httpContextAccessor;
            this.claimSvc = claimSvc;
            this.httpContextAccessor = httpContextAccessor;
            this.commonService = commonService;
            this.claimService = claimService;
            this.sharePointHelper = sharePointHelper;
            this.claimdocumentService = claimdocumentService;
            this.claimsupdocumentService = claimsupdocumentService;
            this.claimboardpassdocumentService = claimboardpassdocumentService;
            this.claimJournalService = claimJournalService;
            this.claimAuditDataService = claimAuditDataService;
            this.claimExpenseSvc = claimExpenseSvc;
            userId = (httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "name")?.Value ?? "").ToString();
            this.claimK2Service = claimK2Service;
            this.genK2Svc = genK2Svc;
        }

        [Route("{TAId?}/{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetAllData(string TAId, string id = "NULL")
        {
            try
            {
                Guid newId = Guid.Empty;
                if (id != "NULL")
                {
                    newId = new Guid(id);
                }

                var accessToken = await commonService.GetAccessToken(HttpContext);
                var data = await claimdataSvc.GetAllData(TAId, newId, accessToken);
                return Ok(new APIResponse<ClaimDataResponseDTO>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(SubmissionRequestDTO request)
        {
            try
            {
                var accessToken = await commonService.GetAccessToken(HttpContext);

                var data = await claimSvc.SubmitClaimWithWorkflow(request, userId, accessToken);
                return Ok(new APIResponse<SubmissionResponseDTO>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }

        private ObjectResult ErrorReturnHandler(Exception ex)
        {
            log.LogError(ex.Detail());

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = ex.Detail()
            }); ;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UploadDocumentRequestDTO request)
        {
            try
            {
                var accessToken = await commonService.GetAccessToken(HttpContext);
                //var accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjU2QThFRjgxQzM3RjNBMjg3QzNBNEVDMTE3NTMxQjJDIiwidHlwIjoiSldUIn0.eyJuYmYiOjE2OTk2NzcwNjYsImV4cCI6MTY5OTcwNTg2NiwiaXNzIjoiaHR0cHM6Ly9teXRyZWUtc3RhZ2luZy5jaWZvci1pY3JhZi5vcmcvaWRlbnRpdHkiLCJhdWQiOiJodHRwczovL215dHJlZS1zdGFnaW5nLmNpZm9yLWljcmFmLm9yZy9pZGVudGl0eS9yZXNvdXJjZXMiLCJjbGllbnRfaWQiOiJJbnRlZ3JhdGVkUG9ydGFsIiwic3ViIjoiNWU4YzkzNDgtNWIzZi00NTM5LTkyMjktMWZjOTA0NGQ0YmZmIiwiYXV0aF90aW1lIjoxNjk5Njc3MDY0LCJpZHAiOiJBenVyZUFEIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJHVDVaSUFRRlVPRUVQNU5PQlJXWlRCRk5HM0pCR0k2WCIsIm5hbWUiOiJNQVNUVVRJIiwib3JpZ2luYWxfbmFtZSI6IkNJRk9SLUlTLUNvbnN1bHQwNiIsInByZWZlcnJlZF91c2VybmFtZSI6IlBhMVpZbnAwN0djMXhhTzZLRHZpS1R1WHl3YjB1aDFYUjVsNjBwWERJSGsiLCJlbWFpbCI6IkNJRk9SLUlTLUNvbnN1bHRhbnQwNkBjaWZvci1pY3JhZi5vcmciLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImp0aSI6IkU3QkY2MjUzNkIxQ0NGNTYyMkFBMkM2RTVCNjM3ODFDIiwic2lkIjoiQzY2OEE0NEE5NTlBMjcwOTA4NzA4QTBDOEZBOUEzMjMiLCJpYXQiOjE2OTk2NzcwNjYsInNjb3BlIjpbImVtYWlsIiwicHJvZmlsZSIsImZpcmViYXNlIiwibXlzdWJtaXNzaW9uIiwiZ3JhbnRzIiwiUXVlcnlFbmdpbmUiLCJsZWF2ZXMiLCJvY3MiLCJlbXBsb3llZXMiLCJvcGVuaWQiLCJvY3NfbmV0Y29yZV9hcGkiLCJhY2Nlc3NfY29udHJvbCIsImludGVncmF0ZWRfcG9ydGFsX2FwaSIsInBtc19ncmFudF9hcGkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsiZXh0ZXJuYWwiXX0.Ae8PROrr6eRxNrycsVNFhouorqh_qR_leaz6wbBph06MF_G9cz2hKZj7DVLWDSp8djBpZgZQ446sM5gY0Uxww1URpzRp-iCMOpegluLnt-uEFtsHhphau_ouV-bIog0cDu_NmkRkkryeAa5oac_Zo72PzkN9vnv2nHve4PQ605mwIKbWzUceEsc5O4WdduOAbtBwHqSiu42Ole0a5JNNt0revxn_6_HVNVLZAeTji4j-yt97OdR8RnL865zaqbNfjW8jdYlEOm9B8ZLPp6vzuzU0vd61NGMluuQMbR2nRm43kbOkz7Wny6dzyLRkiNh94NFEyfslMDM64z0Q2B2o6Q";

                var data = new ClaimDocumentResponseDTO();
                var dataRequest = new ClaimDocumentRequestDTO();
                var uploadDocumentTask = new SharePointResponseDTO.UploadResponse();

                bool isSaveDocument = false;
                if (request.Document != null && request.Document.File != null)
                {
                    isSaveDocument = true;
                    dataRequest.FileName = request.Document.File.FileName;

                    var multiPartStream = new MultipartFormDataContent();
                    var ms = new MemoryStream();
                    await request.Document.File.CopyToAsync(ms);
                    multiPartStream.Add(new StringContent("{}"), "metadata");
                    multiPartStream.Add(request.Document.File.GetStreamContent());
                    try
                    {
                        uploadDocumentTask = await sharePointHelper.uploadFileAsync(multiPartStream, config["SystemName"], config["ModuleName"], request.ClaimId.ToString(), accessToken);
                    }
                    catch (Exception ex)
                    {
                        log.LogError(ex.Detail());
                        throw;
                    }

                    if (uploadDocumentTask.fileList != null && uploadDocumentTask.fileList.Count > 0)
                    {
                        foreach (var sd in uploadDocumentTask.fileList)
                        {
                            dataRequest.FileUrl = sd.webUrl;
                        }
                    }
                }

                var updatedData = new ClaimDocumentResponseDTO();
                if (isSaveDocument)
                {
                    //Add and update tblDocument
                    if (request.Id == Guid.Empty)
                    {
                        dataRequest.Id = Guid.NewGuid();
                        data = await claimdocumentService.Add(dataRequest, userId);
                    }
                    else
                    {
                        dataRequest.Id = request.Id;
                        data = await claimdocumentService.Update(dataRequest, userId);
                    }

                    //update table supporting document
                    if (request.TableName.ToLower() == "claimsupportingdocument")
                    {
                        var dataClaimSupportingDocument = new ClaimSupportingDocumentRequestDTO();
                        var dataclaimsupdoc = await claimsupdocumentService.Get(x => x.ClaimDocumentId.Equals(request.Id));
                        var dataclaimsupdocSingle = dataclaimsupdoc.FirstOrDefault();

                        if (dataclaimsupdocSingle != null)
                        {
                            dataClaimSupportingDocument.MapFrom(dataclaimsupdocSingle);

                            if (request.FieldName.ToLower() == "claimdocumentid")
                            {
                                if (request.Id == Guid.Empty)
                                {
                                    dataClaimSupportingDocument.ClaimDocumentId = dataRequest.Id;
                                }
                                else
                                {
                                    dataClaimSupportingDocument.ClaimDocumentId = request.Id;
                                }
                            }
                            await claimsupdocumentService.Update(dataClaimSupportingDocument);
                        }
                    }

                    //update table supporting document
                    if (request.TableName.ToLower() == "claimboardingpassdocument")
                    {
                        var dataClaimBoardingPassDocument = new ClaimBoardingPassDocumentRequestDTO();
                        var dataclaimboardpassdoc = await claimboardpassdocumentService.Get(x => x.ClaimDocumentId.Equals(request.Id));
                        var dataclaimboardpassdocSingle = dataclaimboardpassdoc.FirstOrDefault();

                        if (dataclaimboardpassdocSingle != null)
                        {
                            dataClaimBoardingPassDocument.MapFrom(dataclaimboardpassdocSingle);

                            if (request.FieldName.ToLower() == "claimdocumentid")
                            {
                                if (request.Id == Guid.Empty)
                                {
                                    dataClaimBoardingPassDocument.ClaimDocumentId = dataRequest.Id;
                                }
                                else
                                {
                                    dataClaimBoardingPassDocument.ClaimDocumentId = request.Id;
                                }
                            }
                            await claimboardpassdocumentService.Update(dataClaimBoardingPassDocument);
                        }
                    }

                    //update table expense detail
                    if (request.TableName.ToLower() == "claimexpensedocument")
                    {
                        var dataClaimExpenseDocument = new ClaimExpenseRequestDTO();
                        var dataclaimexpdoc = await claimExpenseSvc.Get(x => x.ClaimDocumentId.Equals(request.Id));
                        var dataclaimexpdocSingle = dataclaimexpdoc.FirstOrDefault();

                        if (dataclaimexpdocSingle != null)
                        {
                            dataClaimExpenseDocument.MapFrom(dataclaimexpdocSingle);

                            if (request.FieldName.ToLower() == "claimdocumentid")
                            {
                                if (request.Id == Guid.Empty)
                                {
                                    dataClaimExpenseDocument.ClaimDocumentId = dataRequest.Id;
                                }
                                else
                                {
                                    dataClaimExpenseDocument.ClaimDocumentId = request.Id;
                                }
                            }
                            await claimExpenseSvc.Update(dataClaimExpenseDocument);
                        }
                    }

                }


                //var ouput = await docSvc.Add(request);
                return Ok(new APIResponse<ClaimDocumentResponseDTO>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Getk2DataField(Guid ClaimId)
        {
            try
            {
                var accessToken = await commonService.GetAccessToken(HttpContext);
                var allDataClaim = await claimdataSvc.GetAllData("NULL", ClaimId, "");

                Dictionary<string, object> dataToSend = await claimK2Service.GetK2Datafield(allDataClaim);

                return Ok(new APIResponse<Dictionary<string, object>>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = dataToSend
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }

        [Route("{id?}/{username?}/{activityId?}")]
        [HttpGet]
        public async Task<IActionResult> GetApprovalNotes(Guid id, string username, string activityId)
        {
            try
            {
                var data = await claimK2Service.GetApprovalNotes(id, username, activityId);

                return Ok(new APIResponse<K2ApprovalNotes>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return ErrorReturnHandler(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RedirectBudgetHolder(Guid id, string sn, string activityId)
        {
            try
            {
                var data = await claimSvc.ExecuteBudgetHolder(id, sn, activityId);

                return StatusCode(data.status, data);
            }
            catch (Exception ex)
            {
                return ErrorReturnHandler(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AuditData(string ClaimId)
        {
            try
            {
              
                var accessToken = await commonService.GetAccessToken(HttpContext);
                var data = await claimAuditDataService.GetClaimAudit(accessToken, ClaimId); 
                return Ok(new APIResponse<List<ClaimAuditDataResponseDTO>>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AuditDataDetail(string RecId, DateTime ChangeTime, string SubModule)
        {
            try
            {

                var accessToken = await commonService.GetAccessToken(HttpContext);
                var data = await claimAuditDataService.GetClaimAuditDetail(accessToken,
                    x => x.RecId.Equals(RecId) && x.ChangeTime.Equals(ChangeTime) && x.SubModule.Equals(SubModule));
                return Ok(new APIResponse<List<dynamic>>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SnValidator(SnValidatorRequestDTO snValReqDto)
        {
            try
            {
                var data = await genK2Svc.SnValidator(snValReqDto);

                return Ok(new APIResponse<SnValidatorResponseDTO>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return ErrorReturnHandler(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> WorkflowExistValidator(WorkflowExistRequestDTO wfExistReqDto)
        {
            try
            {
                var data = await genK2Svc.WorkflowExistValidator(wfExistReqDto);
                return Ok(new APIResponse<WorkflowExistResponseDTO>()
                {
                    success = true,
                    status = StatusCodes.Status200OK,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return ErrorReturnHandler(ex);
            }
        }

    }
}
