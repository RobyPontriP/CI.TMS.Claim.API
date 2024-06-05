//using CI.TMS.Claim.API.Controllers;
//using CI.TMS.Claim.API.Domain.Entities;
//using CI.TMS.Claim.API.DTOs.Request;
//using CI.TMS.Claim.API.DTOs.Response;
//using CI.TMS.Claim.API.Helper;
//using CI.TMS.Claim.API.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using myTree.MicroService.Helper;
//using Serilog;

//namespace myTree.Webform.Claim.API.Controllers
//{
//    [ApiController]
//    [Route("[controller]/[action]")]
//    public class DocumentController : Controller
//    {

//        private IConfiguration config;
//        private ILogger<DocumentController> log;
//        private IHttpContextAccessor httpContextAccessor;
//        private CommonService commonService;
//        private SharePointHelper sharePointService;
//        private ClaimService claimService;
//        private ClaimDocumentService claimdocumentService;
//        private DocumentService documentService;
//        private string userId = string.Empty;
//        public DocumentController(IConfiguration config,
//            ILogger<DocumentController> log,
//             IHttpContextAccessor httpContextAccessor,
//             CommonService commonService,
//             ClaimService claimService,
//             DocumentService documentService,
//             SharePointHelper sharePointService,
//             ClaimDocumentService claimdocumentService
//            )
//        {
//            this.config = config;
//            this.log = log;
//            this.httpContextAccessor = httpContextAccessor;
//            this.commonService = commonService;
//            this.claimService = claimService;
//            this.sharePointService = sharePointService;
//            this.documentService = documentService;
//            this.claimdocumentService = claimdocumentService;
//            userId = (httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "name")?.Value ?? "").ToString();

//        }


//        //[HttpPost]
//        //[Produces("application/json")]
//        //[ProducesResponseType(typeof(APIResponse<DocumentResponseDTO>), StatusCodes.Status200OK)]
//        //public async Task<IActionResult> UploadFile([FromForm] ClaimDocumentRequestDTO data)
//        //{
//        //    try
//        //    {

//        //        if (!HttpContext.User.Identity.IsAuthenticated)
//        //            return Challenge();

//        //        var accessToken = await commonService.GetAccessToken(HttpContext);
//        //        //accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjU2QThFRjgxQzM3RjNBMjg3QzNBNEVDMTE3NTMxQjJDIiwidHlwIjoiSldUIn0.eyJuYmYiOjE2OTg3Mjg0MzMsImV4cCI6MTY5ODc1NzIzMywiaXNzIjoiaHR0cHM6Ly9teXRyZWUtc3RhZ2luZy5jaWZvci1pY3JhZi5vcmcvaWRlbnRpdHkiLCJhdWQiOiJodHRwczovL215dHJlZS1zdGFnaW5nLmNpZm9yLWljcmFmLm9yZy9pZGVudGl0eS9yZXNvdXJjZXMiLCJjbGllbnRfaWQiOiJJbnRlZ3JhdGVkUG9ydGFsIiwic3ViIjoiMzRjNjhkNzEtNzMwNS00MzMxLTlmZjctMDBkNDE4ZmNiNWFjIiwiYXV0aF90aW1lIjoxNjk4NzIyMjIyLCJpZHAiOiJBenVyZUFEIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI3TkpaVVRZRzVOT0NCMlhZSk80QUhEN1VGQ0hRVUlLUyIsIm5hbWUiOiJKTkVXQkVSWSIsIm9yaWdpbmFsX25hbWUiOiJDSUZPUi1JUy1Db25zdWx0MDciLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJtZUV0RGxCTzJMZENfbkdOME95M1FpVzhkd2pIeDR5MGtuY2F1Zk5pc1dBIiwiZW1haWwiOiJDSUZPUi1JUy1Db25zdWx0YW50MDdAY2lmb3ItaWNyYWYub3JnIiwiZW1haWxfdmVyaWZpZWQiOmZhbHNlLCJqdGkiOiIxNjg5QTA1QkVEMkEyRjIxQTVBMEUxRkY0NDYzMTZDNiIsInNpZCI6IjZGOEY2OUY5MDIxNDE4OUFFRURFNTRGRTY0OUJGMDhFIiwiaWF0IjoxNjk4NzI4NDMzLCJzY29wZSI6WyJlbWFpbCIsInByb2ZpbGUiLCJmaXJlYmFzZSIsIm15c3VibWlzc2lvbiIsImdyYW50cyIsIlF1ZXJ5RW5naW5lIiwibGVhdmVzIiwib2NzIiwiZW1wbG95ZWVzIiwib3BlbmlkIiwib2NzX25ldGNvcmVfYXBpIiwiYWNjZXNzX2NvbnRyb2wiLCJpbnRlZ3JhdGVkX3BvcnRhbF9hcGkiLCJwbXNfZ3JhbnRfYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbImV4dGVybmFsIl19.XDN2J8ZX1Pt6gKXM9DFdEB7lxEed-ySGaoY2LzFkm--w3zBXV066yXnO23yIl97r1qnCKB2kWlHxGu3mTK-pCp9LngarSwa9Tw1omxuHOylnIERjeBkkFAtqyJKTqtSd363nJOzHqwR9P_qRbdDy0Ha4Rmj80rsBz3gTadmmIMe7Q8AEaVX-qE_S_qd0qi0OKyx13-yYAwH5ZVoBy76JWK8JkP_z3dX_2qxlWG4zYYC3pEjIBj-2gM1X5NjjVga3LRp10x9iAuALu1y8H26z1tMiOwAsaRHnZhNWJ-e6TI9etcW-3N5rhXUtQm093VpvyPm7i7df3NuRk4Ii_2TyLg";
//        //        //var Claim = (await claimService.Get(x => x.Id == data.ClaimId)).FirstOrDefault();

//        //        var output = new APIResponse<DocumentResponseDTO>();
//        //        output.success = true;
//        //        output.status = 200;

//        //        var uploadDocumentTask = new SharePointResponseDTO.UploadResponse();
//        //        bool isSaveDocument = false;
//        //        if (data.Document != null && data.Document.File != null)
//        //        {
//        //            isSaveDocument = true;
//        //            //data.FileName = data.Document.File.FileName;
//        //            //validasi kalau tidak ada file tidak perlu upload 
//        //            var multiPartStream = new MultipartFormDataContent();
//        //            var ms = new MemoryStream();
//        //            await data.Document.File.CopyToAsync(ms);
//        //            multiPartStream.Add(new StringContent("{}"), "metadata");
//        //            multiPartStream.Add(new ByteArrayContent(ms.ToArray(), 0, Convert.ToInt32(ms.Length)), $"file", data.Document.File.FileName);
//        //            try
//        //            {
//        //                string SystemName = config["SystemName"].ToString();
//        //                string ModuleName = config["ModuleName"].ToString();
//        //                Guid ModuleId = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
//        //                uploadDocumentTask = await sharePointService.uploadFileAsync(multiPartStream,SystemName, ModuleName, ModuleId.ToString(), accessToken);
//        //            }
//        //            catch (Exception ex)
//        //            {
//        //                log.LogError(ex.Detail());
//        //                throw;
//        //            }

//        //            //if (uploadDocumentTask.fileList != null && uploadDocumentTask.fileList.Count > 0)
//        //            //{
//        //            //    foreach (var sd in uploadDocumentTask.fileList)
//        //            //    {
//        //            //        if (data.Document.FileName?.ToLower() == sd.filename.ToLower())
//        //            //        {
//        //            //            data.FileUrl = sd.webUrl;
//        //            //        }
//        //            //    }
//        //            //}
//        //        }

//        //        if (isSaveDocument)
//        //        {
                    
//        //            var pmDoc = new ClaimDocument();
//        //            pmDoc.FileName = uploadDocumentTask.fileList.Select(x => x.filename).FirstOrDefault();
//        //            pmDoc.FileUrl = uploadDocumentTask.fileList.Select(x => x.webUrl).FirstOrDefault();
//        //            pmDoc.Id = new Guid(uploadDocumentTask.module_id);
//        //            pmDoc.IsActive = true;

//        //            if (data.Id == Guid.Empty)
//        //            {
//        //                data.Id = Guid.NewGuid();
//        //                pmDoc.Id = data.Id;
//        //                await claimdocumentService.AddDocument(pmDoc);
//        //            }
//        //            else
//        //            {
//        //                pmDoc.Id = data.Id;
//        //                await documentService.UpdateDocument(pmDoc);
//        //            }
//        //        }

//        //        var updatedData = new DocumentResponseDTO();
//        //        if (data.Id == Guid.Empty)
//        //        {
//        //            updatedData = await documentService.Add(data, userId);
//        //        }
//        //        else
//        //        {
//        //            updatedData = await documentService.Update(data, userId);
//        //        }

//        //        output.data = updatedData;

//        //        //newAuditRec = updatedData;


//        //        //await documentService.CompareDocument(oldAuditRec, newAuditRec, projectME.ProjectId.ToString(), userId, changeTime, data.CategoryId);

//        //        //notificationService.NotificationOnInformationChanged((Guid)projectME.ProjectId, type.ToLower().Replace("melia", "MELIA"), accessToken);
//        //        return Ok(output);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        log.LogError(ex.Detail());

//        //        return StatusCode(500, new
//        //        {
//        //            success = false,
//        //            message = ex.Detail()
//        //        });
//        //    }
//        //}


//        private ObjectResult ErrorReturnHandler(Exception ex)
//        {
//            log.LogError(ex.Detail());

//            return StatusCode(StatusCodes.Status500InternalServerError, new
//            {
//                success = false,
//                message = ex.Detail()
//            }); ;
//        }



//    }
//}
