using CI.TMS.Claim.API.Helper;
using CI.TMS.Claim.API.Persistence;
using CI.TMS.Claim.API.Services;
using CI.TMS.Claim.API.Services.Master;
using CI.TMS.Claim.API.Services.K2;
using Microsoft.EntityFrameworkCore;
using CI.TMS.Claim.API.Helper.Interfaces;
using CI.TMS.Claim.API.Helper.Notification;
using CI.TMS.Claim.Web.Services;
using Serilog;
using CI.TMS.Claim.API.Services.IntegratedPortal;

namespace CI.TMS.Claim.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {

        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:dbCI_TMS"];
            services.AddDbContext<ClaimContext>(options =>
                options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(ClaimContext).Assembly.FullName)));

            var connectionStringIntegratedPortal = config["ConnectionStrings:dbIntegratedPortal"];
            services.AddDbContext<dbIntegratedPortalContext>(options =>
                options.UseSqlServer(
                connectionStringIntegratedPortal,
                b => b.MigrationsAssembly(typeof(dbIntegratedPortalContext).Assembly.FullName)));
        }

        public static void ConfigureAppServices(this IServiceCollection services)
        {
            //K2
            services.AddTransient<BudgetHolderK2Services, BudgetHolderK2Services>();
            services.AddTransient<ClaimK2Service, ClaimK2Service>();
            services.AddTransient<GeneralK2Service, GeneralK2Service>();
            services.AddTransient<K2ActivityUserListService, K2ActivityUserListService>();
            services.AddTransient<K2ApproveStateService, K2ApproveStateService>();
            services.AddTransient<K2DataLogService, K2DataLogService>();

            //integratedportal
            services.AddTransient<OCSMFLQueueService, OCSMFLQueueService>();

            //Document 
            services.AddTransient<CommonService, CommonService>();
            services.AddTransient<DataTableService, DataTableService>();

            //Sharepoint
            services.AddTransient<SharePointHelper, SharePointHelper>();
            services.AddTransient<INotificationHelper, NotificationHelper>();
            services.AddTransient<NotificationServices, NotificationServices>();

            //Claim
            services.AddTransient<ClaimPerdiemChargeCodeService, ClaimPerdiemChargeCodeService>();
            services.AddTransient<ClaimExpenseChargeCodeService, ClaimExpenseChargeCodeService>();
            services.AddTransient<ClaimCommentService, ClaimCommentService>();
            services.AddTransient<ClaimDataService, ClaimDataService>();
            services.AddTransient<ClaimExpenseService, ClaimExpenseService>();
            services.AddTransient<ClaimPerdiemDetailService, ClaimPerdiemDetailService>();
            services.AddTransient<ClaimPerdiemService, ClaimPerdiemService>();
            services.AddTransient<ClaimSubmissionService, ClaimSubmissionService>();
            services.AddTransient<ClaimService, ClaimService>();
            services.AddTransient<ClaimDocumentService, ClaimDocumentService>();
            services.AddTransient<ClaimSupportingDocumentService, ClaimSupportingDocumentService>();
            services.AddTransient<ClaimBoardingPassDocumentService, ClaimBoardingPassDocumentService>();
            services.AddTransient<ClaimJournalService, ClaimJournalService>();
            services.AddTransient<ClaimAuditDataService, ClaimAuditDataService>();

            //Master
            services.AddTransient<CityService, CityService>();
            services.AddTransient<CountryService, CountryService>();
            services.AddTransient<CurrencyService, CurrencyService>();
            services.AddTransient<PerdiemRateService, PerdiemRateService>();
            services.AddTransient<CostCenterService, CostCenterService>();
            services.AddTransient<WorkOrderService, WorkOrderService>();
            services.AddTransient<EntityService, EntityService>();
            services.AddTransient<EmployeeService, EmployeeService>();
            services.AddTransient<ExpenseTypeService, ExpenseTypeService>();
            services.AddTransient<FinanceOfficerService, FinanceOfficerService>();
            services.AddTransient<PartnerSupplierService, PartnerSupplierService>();
            services.AddTransient<PerdiemTypeService, PerdiemTypeService>();
            services.AddTransient<AccountingPeriodService, AccountingPeriodService>();
            services.AddTransient<TravelJournalService, TravelJournalService>();
            services.AddTransient<LegalEntityService, LegalEntityService>();
            services.AddTransient<FinanceOfficeService, FinanceOfficeService>();
            services.AddTransient<MasterIKICostCentersService, MasterIKICostCentersService>();
            services.AddTransient<ClaimConditionService, ClaimConditionService>();
            services.AddTransient<TravelJournalAccountDefaultService, TravelJournalAccountDefaultService>();
            services.AddTransient<CalendarService, CalendarService>();
            services.AddTransient<BankAccountService, BankAccountService>();
            services.AddTransient<FinanceRateService, FinanceRateService>();

            //TravelAuthorization
            services.AddTransient<TravelAuthorizationService, TravelAuthorizationService>();
            services.AddTransient<TravelAuthorizationExtendedService, TravelAuthorizationExtendedService>();
            services.AddTransient<TravelAuthorizationDestinationService, TravelAuthorizationDestinationService>();
            services.AddTransient<TravelAuthorizationItineraryService, TravelAuthorizationItineraryService>();
            services.AddTransient<TravelAuthorizationService, TravelAuthorizationService>();
            services.AddTransient<TravelAuthorizationSponsorshipService, TravelAuthorizationSponsorshipService>();
            services.AddTransient<TravelAuthorizationTravelerService, TravelAuthorizationTravelerService>();
            services.AddTransient<TravelAuthorizationCostCenterService, TravelAuthorizationCostCenterService>();
            services.AddTransient<TravelAuthorizationPopUpService, TravelAuthorizationPopUpService>();
            services.AddTransient<TravelAuthorizationJournalService, TravelAuthorizationJournalService>();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services, Type T, IConfiguration config)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Log.Logger);
            //services.AddAutoMapper(T);
        }
    }
}
