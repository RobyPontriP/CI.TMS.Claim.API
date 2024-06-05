using CI.TMS.Claim.API.Domain.Entities.K2;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CI.TMS.Claim.API.Persistence
{
    public class ClaimContext : DbContext 
    {
        private readonly IConfiguration config;

        public ClaimContext(IConfiguration config)
        {
            this.config = config;
        }

        public ClaimContext(DbContextOptions<ClaimContext> options, IConfiguration config)
            : base(options)
        {
            this.config = config;
        }
        #region Master Data
        //public DbSet<CI.TMS.Claim.API.Domain.Entities.Employee> Employee { get; set; }
        //public DbSet<CI.TMS.Claim.API.Domain.Entities.CostCenter> CostCenter { get; set; }

        //Claim
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimComment> ClaimComment { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimPerdiemChargeCode> ClaimPerdiemChargeCode { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimExpenseChargeCode> ClaimExpenseChargeCode { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimExpense> ClaimExpense { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimDocument> ClaimDocument { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimPerdiem> ClaimPerdiem { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Claim> Claim { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimPerdiemDetail> ClaimPerdiemDetail { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimSupportingDocument> ClaimSupportingDocument { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimBoardingPassDocument> ClaimBoardingPassDocument { get; set; }
        public DbSet<DataLog> DataLog { get; set; }
        public DbSet<ApproveState> ApproveState { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimJournal> ClaimJournal { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimAuditData> ClaimAuditData { get; set; }


        //Master
        public DbSet<CI.TMS.Claim.API.Domain.Entities.City> City { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.PerdiemRate> PerdiemRate { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.MasterIKICostCenters> MasterIKICostCenters { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Country> Country { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Currency> Currency { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.FinanceRate> FinanceRate { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.CostCenter> CostCenter { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.WorkOrder> WorkOrder { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Entity> Entity { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Employee> Employee { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.DutyPost> DutyPost { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ExpenseType> ExpenseType { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Master.FinanceOfficer> FiannceOfficer { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.PartnerSupplier> PartnerSupplier { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.PerdiemType> PerdiemType { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.AccountingPeriod> AccountingPeriod { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelJournal> TravelJournal { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.LegalEntity> LegalEntity { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Master.FinanceOffice> FinanceOffice { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Airports> Airports { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelCategory> TravelCategory { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ChartOfAccounts> ChartOfAccounts { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelJournalRule> TravelJournalRule { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.ClaimCondition> ClaimCondition { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelJournalAccountDefault> TravelJournalAccountDefault { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.Calendar> Calendar { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.HolidayCalendar> HolidayCalendar { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.BankAccount> BankAccount { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.AgressoCountry> AgressoCountry { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.AgressoDutyPost> AgressoDutyPost { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.AgressoTravelOfficeDutyPost> AgressoTravelOfficeDutyPost { get; set; }


        //TravelAuthorization
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorization> TravelAuthorization { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationExtended> TravelAuthorizationExtended { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationDestination> TravelAuthorizationDestination { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationItinerary> TravelAuthorizationItinerary { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationCostCenter> TravelAuthorizationCostCenter{ get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationTraveler> TravelAuthorizationTraveler { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationSponsorship> TravelAuthorizationSponsorship { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationTravelerExtended> TravelAuthorizationTravelerExtended { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationPopUp> TravelAuthorizationPopUp { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationJournal> TravelAuthorizationJournal { get; set; }
        public DbSet<CI.TMS.Claim.API.Domain.Entities.TravelAuthorizationCarbonOffset> TravelAuthorizationCarbonOffset { get; set; }

        #endregion

        //#region Claim
        ////public DbSet<CI.TMS.Claim.API.Domain.Entities.Claim> Claim { get; set; }
        //#region

        //#region Travel Authorization
        //#region
    }
}
