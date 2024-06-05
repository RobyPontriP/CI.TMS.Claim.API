using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("tblTravel_Category")]
    public class TravelCategory
    {
        [Key]
        [Column("category_id")]
        public string Id { get; set; }
        [Column("category_name")]
        public string CategoryName { get; set; }
        [Column("category_value")]
        public string CategoryValue { get; set; }
        [Column("isflight")]
        public bool? IsFlight { get; set; }
    }
}
