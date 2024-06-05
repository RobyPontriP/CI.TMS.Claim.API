using CI.TMS.Claim.API.Services.Master;
using CI.TMS.Claim.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myTree.MicroService.Helper;
using Serilog;
using System.Collections.Generic;
using CI.TMS.Claim.API.Helper.Reports;
using Newtonsoft.Json;
using CI.TMS.Claim.API.DTOs.Request;
using System.Net;

namespace CI.TMS.Claim.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private IConfiguration config;
        private ILogger<ReportController> log;
        private CommonService commonService;

        public ReportController(IConfiguration config,
            ILogger<ReportController> log,
            CommonService commonService)
        {
            this.config = config;
            this.log = log;
            this.commonService = commonService;
        }

        [HttpPost("ExportToExcel/TEC")]
        public async Task<IActionResult> TECReport([FromForm] string formData)
        {
            try
            {
                ReportTECRequestDTO param = JsonConvert.DeserializeObject<ReportTECRequestDTO>(formData);
                byte[] fileBytes = ExportToExcel.TECReport(param);
                string fileName = "TEC - Export to excel";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


                return File(fileBytes, contentType, fileName);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, new
                {
                    success = false,
                    message = e.Detail()
                });
            }
        }

        //[HttpPost("ExportToExcel/TEC")]
        //public HttpResponseMessage DownloadExcelFile(ReportTECRequestDTO request)
        //{


        //        // Generate the Excel file using your method
        //        var excelData = ExportToExcel.TECReport(request);

        //        // Create a response message
        //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //        response.Content = new ByteArrayContent(excelData);
        //        response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = "example.xlsx" // Set the desired file name
        //        };
        //        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        //        return response;


        //}


    }
}
