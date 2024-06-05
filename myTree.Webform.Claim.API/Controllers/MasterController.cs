using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Services;
using CI.TMS.Claim.API.Services.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myTree.MicroService.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CI.TMS.Claim.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MasterController : ControllerBase
    {
        private IConfiguration config;
        private ILogger<MasterController> log;
        private CityService citySvc;
        private PerdiemRateService perdiemRateSvc;
        private CountryService countrySvc;
        private CurrencyService currencySvc;
        private CostCenterService costcenterSvc;
        private WorkOrderService workorderSvc;
        private EntityService entitySvc;
        private EmployeeService employeeSvc;
        private ExpenseTypeService expensetypeSvc;
        private PartnerSupplierService partnersupplierSvc;
        private PerdiemTypeService perdiemtypeSvc;
        private TravelJournalService traveljournalSvc;
        private LegalEntityService legalentitySvc;
        private MasterIKICostCentersService masterikicostcentersSvc;
        private ClaimConditionService claimConditionSvc;
        private TravelJournalAccountDefaultService journalaccountdefaultSvc;
        private AccountingPeriodService accountingperiodSvc;
        private FinanceOfficerService financeofficerSvc;
        private BankAccountService bankaccountSvc;
        private FinanceRateService financerateSvc;
        public MasterController(IConfiguration config,
            ILogger<MasterController> log,
            CityService citySvc,
            CountryService countrySvc,
            CurrencyService currencySvc,
            CostCenterService costcenterSvc,
            WorkOrderService workorderSvc,
            EntityService entitySvc,
            PerdiemRateService perdiemRateSvc,
            EmployeeService employeeSvc,
            ExpenseTypeService expensetypeSvc,
            PartnerSupplierService partnersupplierSvc,
            PerdiemTypeService perdiemtypeSvc,
            TravelJournalService traveljournalSvc,
            LegalEntityService legalentitySvc,
            MasterIKICostCentersService masterikicostcentersSvc,
            ClaimConditionService claimConditionSvc,
            TravelJournalAccountDefaultService journalaccountdefaultSvc,
            AccountingPeriodService accountingperiodSvc,
            FinanceOfficerService financeofficerSvc,
            BankAccountService bankaccountSvc,
            FinanceRateService financerateSvc)
        {
            this.config = config;
            this.log = log;
            this.citySvc = citySvc;
            this.countrySvc = countrySvc;
            this.currencySvc = currencySvc;
            this.costcenterSvc = costcenterSvc;
            this.workorderSvc = workorderSvc;
            this.entitySvc = entitySvc;
            this.perdiemRateSvc = perdiemRateSvc;
            this.employeeSvc = employeeSvc;
            this.expensetypeSvc = expensetypeSvc;
            this.partnersupplierSvc = partnersupplierSvc;
            this.perdiemtypeSvc = perdiemtypeSvc;
            this.traveljournalSvc = traveljournalSvc;
            this.legalentitySvc = legalentitySvc;
            this.masterikicostcentersSvc = masterikicostcentersSvc;
            this.claimConditionSvc = claimConditionSvc; 
            this.journalaccountdefaultSvc = journalaccountdefaultSvc;
            this.accountingperiodSvc = accountingperiodSvc;
            this.financeofficerSvc = financeofficerSvc;
            this.bankaccountSvc = bankaccountSvc;
            this.financerateSvc = financerateSvc;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<CityResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllCity()
        {
            try
            {
                var output = new APIResponse<List<CityResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = (await citySvc.Get());
                return Ok(output);
            }
            catch(Exception ex)
            {
                log.LogError(ex.Detail());

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Detail()
                });
            }
        }
        [HttpGet("{id}/{datefrom}/{dateto}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<CityResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCityByPerdiem(string id, DateTime datefrom, DateTime dateto)
        {
            try
            {
                var output = new APIResponse<List<CityResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await citySvc.GetCityByPerdiem(id, datefrom, dateto);
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

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<CityResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCityByCountryId(string id)
        {
            try
            {
                var output = new APIResponse<List<CityResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await citySvc.GetCityByCountryId(id);
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<CountryResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllCountry()
        {
            try
            {
                var output = new APIResponse<List<CountryResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await countrySvc.Get();
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

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<CountryResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCountry(string id)
        {
            try
            {
                var output = new APIResponse<CountryResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = (await countrySvc.Get(x => x.Id.ToUpper() == (id ?? "").ToUpper())).FirstOrDefault();
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
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<CurrencyResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllCurrency()
        {
            try
            {
                var output = new APIResponse<List<CurrencyResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await currencySvc.Get();
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
        [Route("{CurrencyCode?}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<CurrencyResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCurrencyByCurrencyCode(string CurrencyCode)
        {
            try
            {
                var output = new APIResponse<CurrencyResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = (await currencySvc.Get(x => x.CurrencyCode == CurrencyCode)).FirstOrDefault();
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



        [Route("{countryid?}/{cityid?}/{datefrom?}/{dateto}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<PerdiemRateResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPerdiemRates(string countryid, string cityid, DateTime datefrom, DateTime dateto)
        {
            try
            {
                var output = new APIResponse<PerdiemRateResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = (await perdiemRateSvc.GetPerdiem(countryid, cityid, datefrom, dateto)).FirstOrDefault();
                //output.data = (await perdiemRateSvc.Get());
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

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<CostCenterResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllCostCenterTA(string id)
        {
            try
            {
                var output = new APIResponse<List<CostCenterResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await costcenterSvc.Get(id);
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

        [Route("{id?}/{costcenterid?}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<WorkOrderResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllWorkOrderTA(string id, string costcenterid)
        {
            try
            {
                var output = new APIResponse<List<WorkOrderResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await workorderSvc.Get(id, costcenterid);
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

        [Route("{id?}/{costcenterid?}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<EntityResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllEntityTA(string id, string costcenterid)
        {
            try
            {
                var output = new APIResponse<List<EntityResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await entitySvc.Get(id, costcenterid);
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

        [Route("{username}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<EmployeeResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmployeeByUsername(string username)
        {
            try
            {
                var output = new APIResponse<EmployeeResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = await employeeSvc.GetEmployeeByUsername(username);
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

        [Route("{id}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<EmployeeResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmployeeByClaimId(Guid id)
        {
            try
            {
                var output = new APIResponse<EmployeeResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = await employeeSvc.GetEmployeeByClaimId(id);
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


        [Route("{TAId?}")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetExpenseTypeByTAId(string TAId)
        {
            try
            {
                var output = new APIResponse<List<ExpenseTypeResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await expensetypeSvc.GetByTAId(TAId);
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

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetExpenseType()
        {
            try
            {
                var output = new APIResponse<List<ExpenseTypeResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await expensetypeSvc.Get();
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<PartnerSupplierResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllTaxSystem()
        {
            try
            {
                var output = new APIResponse<List<PartnerSupplierResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await partnersupplierSvc.GetAllTaxSystem();
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<PerdiemTypeResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllPerdiemType()
        {
            try
            {
                var output = new APIResponse<List<PerdiemTypeResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await perdiemtypeSvc.GetAllPerdiemType();
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

        [Route("{TAId?}")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetTravelJournalAdvanceByTAId(string TAId)
        {
            try
            {
                var output = new APIResponse<TravelJournalAdvanceResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = await traveljournalSvc.GetTravelJournalAdvanceByTAId(TAId);
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
        [Route("{AparId?}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<EmployeeResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmployeeByAparId(string AparId)
        {
            try
            {
                var output = new APIResponse<List<EmployeeResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await employeeSvc.Get(x=>x.RowId == AparId && x.EmpStatus.ToUpper() != "RESIGNED" && x.EmpStatus.ToUpper() != "END OF CONTRACT");
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<LegalEntityResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllLegalEntity()
        {
            try
            {
                var output = new APIResponse<List<LegalEntityResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await legalentitySvc.Get();
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

        [Route("{Resno?}")]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<PartnerSupplierResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetMasterSupplierByResno(string Resno)
        {
            try
            {
                var output = new APIResponse<List<PartnerSupplierResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await partnersupplierSvc.Get(x => !string.IsNullOrEmpty(x.Id) && x.Id == Resno);
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<AccountCodeResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetListAccountCode()
        {
            try
            {
                var output = new APIResponse<List<AccountCodeResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await expensetypeSvc.GetListAccountCode();
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

        [HttpGet("{ProjectId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<MasterIKICostCentersResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetMasterIKICostCenters(string ProjectId)
        {
            try
            {
                var output = new APIResponse<MasterIKICostCentersResponseDTO>();
                output.success = true;
                output.status = 200;
                output.data = (await masterikicostcentersSvc.Get(x => x.ProjectId.ToUpper() == (ProjectId ?? "").ToUpper())).FirstOrDefault();
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

        [HttpGet("{Search}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<PartnerSupplierDropdown>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAparIdByName(string Search)
        {
            try
            {
                var output = new APIResponse<List<PartnerSupplierDropdown>>();
                output.success = true;
                output.status = 200;
                output.data = await partnersupplierSvc.GetAparIdByName(x=> (x.Id.Contains(Search) || x.Name.Contains(Search)) && !string.IsNullOrEmpty(x.Id));
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<ClaimConditionResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetClaimCondition()
        {
            try
            {
                var output = new APIResponse<List<ClaimConditionResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = (await claimConditionSvc.Get());
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<TravelJournalAccountDefaultResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetListJournalAccountDefault()
        {
            try
            {
                var output = new APIResponse<List<TravelJournalAccountDefaultResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await journalaccountdefaultSvc.Get();
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<AccountingPeriodResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllPeriod()
        {
            try
            {
                var output = new APIResponse<List<AccountingPeriodResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await accountingperiodSvc.Get();
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

        [HttpGet("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<FinanceOfficerResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetFinanceByUserId(string userId)
        {
            try
            {
                var output = new APIResponse<List<FinanceOfficerResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await financeofficerSvc.GetFinanceByUserId(userId);
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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<BankAccountDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetListBankAccount()
        {
            try
            {
                var output = new APIResponse<BankAccountDTO>();
                output.success = true;
                output.status = 200;
                output.data = await bankaccountSvc.Get();
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

        [HttpGet("{transactionDate}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(APIResponse<List<FinanceRateResponseDTO>>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCurrencyByTransactionDate(string transactionDate)
        {
            try
            {
                var output = new APIResponse<List<FinanceRateResponseDTO>>();
                output.success = true;
                output.status = 200;
                output.data = await financerateSvc.Get(x=> Convert.ToDateTime(transactionDate) >= x.DateFrom && Convert.ToDateTime(transactionDate) <= x.DateTo);
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
