using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class PerdiemRateResponseDTO
    {
        public Guid Id { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        [Precision(18, 2)]
        public decimal FullPerdiem { get; set; }
        [Precision(18, 2)]
        public decimal FullPerdiemOriginal { get; set; }
        public string CurrencyId { get; set; }
        [Precision(18, 2)]
        public decimal MealsPrice { get; set; }
        [Precision(18, 2)]
        public decimal MealsPriceOriginal { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int IsActive { get; set; }
        //[Precision(18, 2)]
        //public decimal BreakfastAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.2 * MealsPrice), 2) : Math.Round(((Decimal)0.2 * MealsPrice), 2); } set { } }
        //[Precision(18, 2)]
        //public decimal LunchAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.25 * MealsPrice), 2) : Math.Round(((Decimal)0.4 * MealsPrice), 2); } set { } }
        //[Precision(18, 2)]
        //public decimal DinnerAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.35 * MealsPrice), 2) : Math.Round(((Decimal)0.4 * MealsPrice), 2); } set { } }
        //[Precision(18, 2)]
        //public decimal IncidentalAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.2 * MealsPrice), 2) : 0; } set { } }
        //[Precision(18, 2)]
        //public decimal BreakfastOriginalAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.2 * MealsPrice), 2) : Math.Round(((Decimal)0.2 * MealsPriceOriginal), 2); } set { } }
        //[Precision(18, 2)]
        //public decimal LunchOriginalAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.25 * MealsPrice), 2) : Math.Round(((Decimal)0.4 * MealsPriceOriginal), 2); } set { } }
        //[Precision(18, 2)]
        //public decimal DinnerOriginalAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.35 * MealsPrice), 2) : Math.Round(((Decimal)0.4 * MealsPriceOriginal), 2); } set { } }
        //[Precision(18, 2)]
        //public decimal IncidentalOriginalAmount { get { return CurrencyId == "USD" ? Math.Round(((Decimal)0.2 * MealsPrice), 2) : (Decimal)0 * MealsPriceOriginal; } set { } }
        [Precision(18, 2)]
        public decimal Breakfast { get; set; }
        [Precision(18, 2)]
        public decimal Lunch { get; set; }
        [Precision(18, 2)]
        public decimal Dinner { get; set; }
        [Precision(18, 2)]
        public decimal Incidental { get; set; }
        [Precision(18, 2)]
        public decimal BreakfastOriginal { get; set; }
        [Precision(18, 2)]
        public decimal LunchOriginal { get; set; }
        [Precision(18, 2)]
        public decimal DinnerOriginal { get; set; }
        [Precision(18, 2)]
        public decimal IncidentalOriginal { get; set; }
        public string PerdiemType { get; set; }

    }

    public class PerdiemListResponse
    {
        public Guid Id { get; set; }
        public string CountryId { get; set; }
        public PerdiemRateResponseDTO PerdiemRates { get; set; }
        public List<CityResponseDTO> ListCity { get; set; }

    }
}
