using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("airports")]
    public class Airports
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string? Name { get; set; }
        [Column("city")]
        public string? City { get; set; }
        [Column("country")]
        public string? Country { get; set; }
        [Column("iata")]
        public string? Iata { get; set; }
    }
}
