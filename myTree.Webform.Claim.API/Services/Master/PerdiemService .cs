using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class PerdiemRateService : BaseService
    {
        IConfiguration config;
        CityService cityService;
        public PerdiemRateService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<PerdiemRateService> log, IConfiguration Config, CityService cityService)
            : base(context, httpContextAccessor, log)
        {
            config = Config;
            this.cityService = cityService;
        }

        public async Task<List<PerdiemRateResponseDTO>> Get(Expression<Func<PerdiemRate, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => x.MealsPrice > 0;

                return await context.PerdiemRate.Where(predicate).AsNoTracking().Skip(0).Project().To<PerdiemRateResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<PerdiemRateResponseDTO>> GetPerdiem(string countryid, string cityid, DateTime datefrom, DateTime dateto, Expression<Func<PerdiemRate, bool>>? predicate = null)
        {
            try
            {

                var perdiemList = new List<PerdiemRateResponseDTO>(); // Assuming PerdiemRateResponseDTO is your DTO class
                DateTime date = datefrom;
                var a = date.AddDays(1);
                for (date = datefrom; date <= dateto; date = date.AddDays(1))
                {
                    if (predicate == null)
                        predicate = x => x.MealsPrice > 0;

                    string? gracePeriod = config["GracePeriod"];
                    int GracePeriodInt = Convert.ToInt32(gracePeriod);

                    var dataperdiem = await context.PerdiemRate
                        .Where(predicate)
                        .Where(p => p.CityId == cityid && p.CountryId == countryid && p.IsActive == 1 &&
                                    ((p.DateFrom <= date && p.DateTo.AddDays(GracePeriodInt) >= date) /*|| (p.DateFrom <= dateto && p.DateTo.AddDays(GracePeriodInt) >= dateto)*/))
                        .Project().To<PerdiemRateResponseDTO>().OrderByDescending(p => p.Year).Take(1)
                        .ToListAsync();

                    perdiemList.AddRange(dataperdiem);
                }
                var groupedPerdiemList = perdiemList.GroupBy(p => new { p.CountryId, p.CityId }).Select(group => new PerdiemRateResponseDTO
                {
                    CountryId = group.Key.CountryId,
                    CountryName = group.Select(x => x.CountryName).FirstOrDefault(),
                    CityId = group.Key.CityId,
                    CityName = group.Select(x => x.CityName).FirstOrDefault(),
                    CurrencyId = group.Select(x => x.CurrencyId).FirstOrDefault(),
                    FullPerdiem = group.Sum(item => item.FullPerdiem),
                    FullPerdiemOriginal = group.Sum(item => item.FullPerdiemOriginal),
                    MealsPrice = group.Sum(item => item.MealsPrice),
                    MealsPriceOriginal = group.Sum(item => item.MealsPriceOriginal),
                    Year = group.Select(x => x.Year).FirstOrDefault(),
                    Month = group.Select(x => x.Month).FirstOrDefault(),
                    IsActive = group.Select(x => x.IsActive).FirstOrDefault(),
                    Breakfast = group.Sum(item => item.Breakfast),
                    Lunch = group.Sum(item => item.Lunch),
                    Dinner = group.Sum(item => item.Dinner),
                    Incidental = group.Sum(item => item.Incidental),
                    BreakfastOriginal = group.Sum(item => item.BreakfastOriginal),
                    LunchOriginal = group.Sum(item => item.LunchOriginal),
                    DinnerOriginal = group.Sum(item => item.DinnerOriginal),
                    IncidentalOriginal = group.Sum(item => item.IncidentalOriginal),
                    PerdiemType = group.Select(x => x.PerdiemType).FirstOrDefault()

                }).ToList();

                return groupedPerdiemList.ToList();
                //return perdiemList.GroupBy(p => new { p.CountryId, p.CityId }).Select(grp => new { CountryId = grp.Key.CountryId, City = grp.Key.CityId }).ToList();

                //if (predicate == null)
                //    predicate = x => x.MealsPrice > 0;

                //return await context.PerdiemRate.Where(predicate).Where(p => p.CityId == cityid && p.CountryId == countryid && p.IsActive == 1 
                //&& ((p.DateFrom <= datefrom && p.DateTo >= datefrom) || (p.DateFrom <= dateto && p.DateTo >= dateto))).OrderByDescending(p => p.Periode).AsNoTracking().Skip(0).Project().To<PerdiemRateResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }


        public async Task<List<PerdiemListResponse>> GetListPerdiem(List<ClaimPerdiemResponseDTO> perdiem, Expression<Func<PerdiemRate, bool>>? predicate = null)
        {
            try
            {
                var perdiemListResult = new List<PerdiemListResponse>();

                foreach (var item in perdiem)
                {
                    var perdiemResultData = new PerdiemListResponse();

                    //Get list Perdiem rates based on date range
                    DateTime date = Convert.ToDateTime(item.DateFrom);
                    var a = date.AddDays(1);
                    var perdiemRow = new List<PerdiemRateResponseDTO>();
                    for (date = Convert.ToDateTime(item.DateFrom); date <= Convert.ToDateTime(item.DateTo); date = date.AddDays(1))
                    {
                        if (predicate == null)
                            predicate = x => x.MealsPrice > 0;

                        string? gracePeriod = config["GracePeriod"];
                        int GracePeriodInt = Convert.ToInt32(gracePeriod);

                        var dataperdiem = await context.PerdiemRate
                            .Where(predicate)
                            .Where(p => p.CityId == item.CityId && p.CountryId == item.CountryId && p.IsActive == 1 &&
                                        (p.DateFrom <= date && p.DateTo.AddDays(GracePeriodInt) >= date))
                            .Project().To<PerdiemRateResponseDTO>().OrderByDescending(p => p.Year).Take(1)
                            .ToListAsync();

                        perdiemRow.AddRange(dataperdiem);
                    }


                    //Get list city based on country id and date range
                    perdiemResultData.CountryId = item.CountryId;
                    perdiemResultData.ListCity = await cityService.GetCityByPerdiem(item.CountryId, Convert.ToDateTime(item.DateFrom), Convert.ToDateTime(item.DateTo));


                    //grouping list perdiemRates
                    var groupedPerdiemList = new PerdiemRateResponseDTO();
                    groupedPerdiemList = perdiemRow.GroupBy(p => new { p.CountryId, p.CityId }).Select(group => new PerdiemRateResponseDTO
                    {
                        CountryId = group.Key.CountryId,
                        CountryName = group.Select(x => x.CountryName).FirstOrDefault(),
                        CityId = group.Key.CityId,
                        CityName = group.Select(x => x.CityName).FirstOrDefault(),
                        CurrencyId = group.Select(x => x.CurrencyId).FirstOrDefault(),
                        FullPerdiem = group.Sum(item => item.FullPerdiem),
                        FullPerdiemOriginal = group.Sum(item => item.FullPerdiemOriginal),
                        MealsPrice = group.Sum(item => item.MealsPrice),
                        MealsPriceOriginal = group.Sum(item => item.MealsPriceOriginal),
                        Year = group.Select(x => x.Year).FirstOrDefault(),
                        Month = group.Select(x => x.Month).FirstOrDefault(),
                        IsActive = group.Select(x => x.IsActive).FirstOrDefault(),
                        Breakfast = group.Sum(item => item.Breakfast),
                        Lunch = group.Sum(item => item.Lunch),
                        Dinner = group.Sum(item => item.Dinner),
                        Incidental = group.Sum(item => item.Incidental),
                        BreakfastOriginal = group.Sum(item => item.BreakfastOriginal),
                        LunchOriginal = group.Sum(item => item.LunchOriginal),
                        DinnerOriginal = group.Sum(item => item.DinnerOriginal),
                        IncidentalOriginal = group.Sum(item => item.IncidentalOriginal),
                        PerdiemType = group.Select(x => x.PerdiemType).FirstOrDefault(),

                    }).FirstOrDefault();
                    perdiemResultData.Id = item.Id;
                    perdiemResultData.PerdiemRates = groupedPerdiemList;

                    perdiemListResult.Add(perdiemResultData);
                }

                return perdiemListResult;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<PerdiemListResponse>> GetListPerdiemDetail(List<ClaimPerdiemDetailResponseDTO> perdiem, Expression<Func<PerdiemRate, bool>>? predicate = null)
        {
            try
            {
                var perdiemListResult = new List<PerdiemListResponse>();

                foreach (var item in perdiem)
                {
                    var perdiemResultData = new PerdiemListResponse();

                    //Get list Perdiem rates based on date range
                    DateTime date = Convert.ToDateTime(item.Date);
                    var a = date.AddDays(1);
                    var perdiemRow = new List<PerdiemRateResponseDTO>();
                    for (date = Convert.ToDateTime(item.Date); date <= item.Date; date = date.AddDays(1))
                    {
                        if (predicate == null)
                            predicate = x => x.MealsPrice > 0;

                        string? gracePeriod = config["GracePeriod"];
                        int GracePeriodInt = Convert.ToInt32(gracePeriod);

                        var dataperdiem = await context.PerdiemRate
                            .Where(predicate)
                            .Where(p => p.CityId == item.CityId && p.CountryId == item.CountryId && p.IsActive == 1 &&
                                        (p.DateFrom <= date && p.DateTo.AddDays(GracePeriodInt) >= date))
                            .Project().To<PerdiemRateResponseDTO>().OrderByDescending(p => p.Year).Take(1)
                            .ToListAsync();

                        perdiemRow.AddRange(dataperdiem);
                    }


                    //Get list city based on country id and date range
                    perdiemResultData.CountryId = item.CountryId;
                    perdiemResultData.ListCity = await cityService.GetCityByPerdiem(item.CountryId, Convert.ToDateTime(item.Date), Convert.ToDateTime(item.Date));


                    //grouping list perdiemRates
                    var groupedPerdiemList = new PerdiemRateResponseDTO();
                    groupedPerdiemList = perdiemRow.GroupBy(p => new { p.CountryId, p.CityId }).Select(group => new PerdiemRateResponseDTO
                    {
                        CountryId = group.Key.CountryId,
                        CountryName = group.Select(x => x.CountryName).FirstOrDefault(),
                        CityId = group.Key.CityId,
                        CityName = group.Select(x => x.CityName).FirstOrDefault(),
                        CurrencyId = group.Select(x => x.CurrencyId).FirstOrDefault(),
                        FullPerdiem = group.Sum(item => item.FullPerdiem),
                        FullPerdiemOriginal = group.Sum(item => item.FullPerdiemOriginal),
                        MealsPrice = group.Sum(item => item.MealsPrice),
                        MealsPriceOriginal = group.Sum(item => item.MealsPriceOriginal),
                        Year = group.Select(x => x.Year).FirstOrDefault(),
                        Month = group.Select(x => x.Month).FirstOrDefault(),
                        IsActive = group.Select(x => x.IsActive).FirstOrDefault(),
                        Breakfast = group.Sum(item => item.Breakfast),
                        Lunch = group.Sum(item => item.Lunch),
                        Dinner = group.Sum(item => item.Dinner),
                        Incidental = group.Sum(item => item.Incidental),
                        BreakfastOriginal = group.Sum(item => item.BreakfastOriginal),
                        LunchOriginal = group.Sum(item => item.LunchOriginal),
                        DinnerOriginal = group.Sum(item => item.DinnerOriginal),
                        IncidentalOriginal = group.Sum(item => item.IncidentalOriginal),
                        PerdiemType = group.Select(x => x.PerdiemType).FirstOrDefault(),

                    }).FirstOrDefault();
                    perdiemResultData.Id = item.Id;
                    perdiemResultData.PerdiemRates = groupedPerdiemList;

                    perdiemListResult.Add(perdiemResultData);
                }

                return perdiemListResult;
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
