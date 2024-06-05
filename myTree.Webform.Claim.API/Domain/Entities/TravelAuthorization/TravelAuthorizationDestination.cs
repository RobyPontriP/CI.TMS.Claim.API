using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("TravelAuthorizationDestination")]
    public class TravelAuthorizationDestination
    {
        [Key]
        public string Id { get; set; }
        [Column("destination_country_id")]
        public string CountryId { get; set; }
        [Column("destination_country_name")]
        public string CountryName { get; set; }
        [Column("destination_city_id")]
        public string CityId { get; set; }
        [Column("destination_city_name")]
        public string CityName { get; set; }
        [Column("destination_other_city_name")]
        public string OtherCityName { get; set; }
        [Column("start_date_of_trip")]
        public DateTime? StartDate { get; set; }
        [Column("end_date_of_trip")]
        public DateTime? EndDate { get; set; }
        [Column("class_of_travel")]
        public string ClassOfTravel { get; set; }
        [Column("purpose_of_travel")]
        public string PurposeOfTravel { get; set; }
        [Column("travel_authorization_id")]
        public string TAId { get; set; }
        [Column("type_of_travel")]
        public string TypeOfTravel { get; set; }
    }
}
