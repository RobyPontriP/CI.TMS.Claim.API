using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class TravelAuthorizationDestinationResponseDTO
    {
        public string Id { get; set; }
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string OtherCityName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClassOfTravel { get; set; }
        public string PurposeOfTravel { get; set; }
        public string TAId { get; set; }
        public string TypeOfTravel { get; set; }
    }
}
