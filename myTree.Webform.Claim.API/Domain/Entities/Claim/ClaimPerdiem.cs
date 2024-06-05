using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ClaimPerdiem")]
    public class ClaimPerdiem : BaseEntity<Guid>
    {
        [Key]
        public override Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CityId { get; set; }
        public string? CityName { get; set; }
        public string? CityOther { get; set; }
        public decimal? TotalPerdiemRate { get; set; }
        public bool? B { get; set; }
        public bool? L { get; set; }
        public bool? D { get; set; }
        public bool? I { get; set; }
        public bool? F { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalAmount0 { get; set; }
        public decimal? TotalAmountFinance { get; set; }
        public decimal? TotalAmount0Finance { get; set; }
        public decimal? TotalPerdiemRate0 { get; set; }
        public string? Currency { get; set; }
        public bool IsFinance { get; set; }
        public override bool IsActive { get; set; }
        public override DateTime? CreatedAt { get; set; }
        public override string CreatedBy { get; set; }
        public override DateTime? UpdatedAt { get; set; }
        public override string UpdatedBy { get; set; }
    }
}
