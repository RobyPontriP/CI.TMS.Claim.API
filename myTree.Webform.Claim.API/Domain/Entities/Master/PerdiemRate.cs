using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("PerDiemRates")]
    public class PerdiemRate
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("country_id")]
        public string CountryId { get; set; }
        [Column("country_name")]
        public string CountryName { get; set; }
        [Column("city_id")]
        public string CityId { get; set; }
        [Column("city_name")]
        public string CityName { get; set; }
        [Precision(18, 2)]
        [Column("full_perdiem")]
        public decimal FullPerdiem { get; set; }
        [Column("CurrencyOriginalId")]
        public string CurrencyId { get; set; }
        [Precision(18, 2)]
        [Column("meals_pice")]
        public decimal MealsPrice { get; set; }
        [Precision(18, 2)]
        [Column("breakfast")]
        public decimal Breakfast { get; set; }
        [Precision(18, 2)]
        [Column("lunch")]
        public decimal Lunch { get; set; }
        [Precision(18, 2)]
        [Column("dinner")]
        public decimal Dinner { get; set; }
        [Precision(18, 2)]
        [Column("incidentals")]
        public decimal Incidental { get; set; }
        [Column("periode_year")]
        public int Year { get; set; }
        [Column("periode_month_no")]
        public int Month { get; set; }
        [Column("is_active")]
        public int IsActive { get; set; }
        [Column("perdiemrate_type")]
        public string PerdiemType { get; set; }
        [Column("period_year_month")]
        public int Periode { get; set; }
        [Column("date_from")]
        public DateTime DateFrom { get; set; }
        [Column("date_to")]
        public DateTime DateTo { get; set; }
        [Precision(18, 2)]
        public decimal FullPerdiemOriginal { get; set; }
        [Precision(18, 2)]
        public decimal MealsPriceOriginal { get; set; }
        [Precision(18, 2)]
        public decimal BreakfastOriginal { get; set; }
        [Precision(18, 2)]
        public decimal LunchOriginal { get; set; }
        [Precision(18, 2)]
        public decimal DinnerOriginal { get; set; }
        [Precision(18, 2)]
        [Column("IncidentalsOriginal")]
        public decimal IncidentalOriginal { get; set; }
    }
}
