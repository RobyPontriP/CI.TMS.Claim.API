using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Persistence;
using Microsoft.EntityFrameworkCore;
using myTree.MicroService.Helper;
using System.Linq;
using System.Linq.Expressions;

namespace CI.TMS.Claim.API.Services
{
    public class CountryService: BaseService
    {
        public CountryService(ClaimContext context, IHttpContextAccessor httpContextAccessor, ILogger<CountryService> log)
            : base (context, httpContextAccessor, log) {
        }

        public async Task<List<CountryResponseDTO>> Get(Expression<Func<Country, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    predicate = x => !string.IsNullOrEmpty(x.Name) && x.Name != "ALL COUNTRIES";

              return await context.Country.Where(predicate).Select(p => new { Id = p.Id, Name = p.Name.ToUpper()}).OrderBy(p=>p.Name).AsNoTracking().Project().To<CountryResponseDTO>().ToListAsync();
              //return await
              //context.Country.Where(predicate)
              //.SelectMany(tat => context.PerdiemRate.Where(tate => tat.Id.ToUpper() == tate.CountryId.ToUpper()).Distinct(), (tate, tat) => new { tate = tate, Country = tat })
              //.Select(select => new
              //{ select.tate.Id, select.tate.CountryName }).AsNoTracking().Project().To<CountryResponseDTO>().ToListAsync();

            }
            catch(Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }

        public async Task<List<CountryResponseDTO>> GetRegion(string TravelOfficeId)
        {
            try
            {
                return await
               context.AgressoTravelOfficeDutyPost.Where(x => x.TravelOfficeId == TravelOfficeId)
               .SelectMany(tat => context.AgressoDutyPost.Where(dutypost => dutypost.Id == tat.DutyPostId).DefaultIfEmpty(), (tat, dutypost) => new { Tat = tat, CC = dutypost })
               .SelectMany(tat => context.AgressoCountry.Where(cou => cou.Id == tat.Tat.DutyPostId).DefaultIfEmpty(), (tat, Country) => new { Tat = tat, Country = Country })
               .Select(select => new
               {
                   RegionName = select.Country.RegionId == "OTHER_CONT" && select.Country.RegionId == "DE" ? select.Country.SubRegionId : select.Country.Id == "DE" ? "ASIA" : select.Country.RegionId
               }).AsNoTracking().Project().To<CountryResponseDTO>().ToListAsync();
            }


            catch (Exception ex)
            {
                ErrorServiceHandler(ex);
                throw;
            }
        }
    }
}
