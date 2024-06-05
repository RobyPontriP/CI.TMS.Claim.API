using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq.Expressions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;


namespace CI.TMS.Claim.API.Services
{
    public class CityService: BaseService
    {
        IConfiguration config;
        public CityService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<CityService> log, IConfiguration Config)
            : base (context, httpContextAccessor, log) {
            config = Config;
        }

        public async Task<List<CityResponseDTO>> Get(Expression<Func<City, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Name);

                return await context.City.Where(predicate)
                    .GroupBy(p => new { p.Id, p.Name, p.CountryId }).Select(grp => new CityResponseDTO() { Id = grp.Key.Id, Name = grp.Key.Name, CountryId = grp.Key.CountryId }).OrderBy(p => p.Id).AsNoTracking().Project().To<CityResponseDTO>().ToListAsync();
            }
            catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<CityResponseDTO>> GetCityByPerdiem(string countryid,DateTime datefrom, DateTime dateto, Expression<Func<City, bool>>? predicate = null)
        {
            try
            {
                var tes = DateTime.Today;
                var tes2 = dateto;
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Name) && x.CountryId == countryid;

                string? gracePeriod = config["GracePeriod"];
                int GracePeriodInt = Convert.ToInt32(gracePeriod);




                //return await context.City.Where(predicate).AsNoTracking().Project().To<CityResponseDTO>().ToListAsync();
                return await context.City.Where(predicate)
                    .Where(p => ((p.DateFrom <= datefrom && p.DateTo.AddDays(GracePeriodInt) >= datefrom) || (p.DateFrom <= dateto && p.DateTo.AddDays(GracePeriodInt) >= dateto)))
                    .GroupBy(p => new { p.Id , p.Name, p.CountryId, p.SeqNo}).Select(grp => new CityResponseDTO() { Id = grp.Key.Id, Name = grp.Key.Name, CountryId = grp.Key.CountryId, SeqNo = grp.Key.SeqNo}).OrderBy(p => p.SeqNo).ThenBy(p => p.Name).AsNoTracking().Project().To<CityResponseDTO>().ToListAsync();
            
            
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<CityResponseDTO>> GetCityByCountryId(string countryid, Expression<Func<City, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Name) && x.CountryId == countryid;

                //return await context.City.Where(predicate).AsNoTracking().Project().To<CityResponseDTO>().ToListAsync();
                return await context.City.Where(predicate)
                   .GroupBy(p => new { p.Id, p.Name }).Select(grp => new CityResponseDTO() { Id = grp.Key.Id, Name = grp.Key.Name }).OrderBy(p => p.Id).AsNoTracking().Project().To<CityResponseDTO>().ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

    }
}
