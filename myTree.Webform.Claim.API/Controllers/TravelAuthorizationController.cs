using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myTree.MicroService.Helper;
using Serilog;
using System.Collections.Generic;

namespace CI.TMS.Claim.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TravelAuthorizationController : ControllerBase
    {
            private IConfiguration config;
            private ILogger<TravelAuthorizationController> log;
            private TravelAuthorizationService travelauthorizationSvc;
            private TravelAuthorizationDestinationService travelauthorizationdestinationSvc;
            private TravelAuthorizationTravelerService travelauthorizationtravelerSvc;
            private TravelAuthorizationCostCenterService travelauthorizationcostcenterSvc;
            private TravelAuthorizationPopUpService travelauthorizationpopupSvc;
            public TravelAuthorizationController(IConfiguration config,
                ILogger<TravelAuthorizationController> log,
                TravelAuthorizationService travelauthorizationSvc,
                TravelAuthorizationDestinationService travelauthorizationdestinationSvc,
                TravelAuthorizationTravelerService travelauthorizationtravelerSvc,
                TravelAuthorizationCostCenterService travelauthorizationcostcenterSvc,
                TravelAuthorizationPopUpService travelauthorizationpopupSvc
                ) {
                this.config = config;
                this.log = log;
                this.travelauthorizationSvc = travelauthorizationSvc;
                this.travelauthorizationdestinationSvc = travelauthorizationdestinationSvc;
                this.travelauthorizationtravelerSvc = travelauthorizationtravelerSvc;
                this.travelauthorizationcostcenterSvc = travelauthorizationcostcenterSvc;
                this.travelauthorizationpopupSvc = travelauthorizationpopupSvc;

            }

     

        [HttpGet("{TAId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<TravelAuthorizationDestinationResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTravelAuthorizationDestination(string TAId)
        {
            try
            {
                var data = await travelauthorizationdestinationSvc.Get(x => x.TAId.ToUpper() == (TAId ?? "").ToUpper());
                var output = new APIResponse<IEnumerable<TravelAuthorizationDestinationResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = data;
                return Ok(output);
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

        [HttpGet("{TAId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<TravelAuthorizationTravelerResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTravelAuthorizationTraveler(string TAId)
        {
            try
            {
                var output = new APIResponse<TravelAuthorizationTravelerResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = (await travelauthorizationtravelerSvc.Get(x => x.TAId == (TAId ?? ""))).FirstOrDefault();
                return Ok(output);
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

        [HttpGet("{TAId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<TravelAuthorizationCostCenterResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTravelAuthorizationCostCenter(string TAId)
        {
            try
            {
                var data = await travelauthorizationcostcenterSvc.Get(x => x.TAId.ToUpper() == (TAId ?? "").ToUpper());
                var output = new APIResponse<IEnumerable<TravelAuthorizationCostCenterResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = data;
                return Ok(output);
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

        [Route("{userid}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<TravelAuthorizationPopUpResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTravelAuthorization(string userid)
        {
            try
            {
                var output = new APIResponse<List<TravelAuthorizationPopUpResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await travelauthorizationpopupSvc.GetTravelAuthorizationPopUpByUserId(userid);
                return Ok(output);
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

        [HttpGet("{TAId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<TravelAuthorizationCarbonOffsetResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetTravelCarbonOffsetById(string TAId)
        {
            try
            {
                var data = await travelauthorizationSvc.GetTravelCarbonOffsetByTAId(TAId);
                var output = new APIResponse<List<TravelAuthorizationCarbonOffsetResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = data;
                return Ok(output);
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

    }
}
