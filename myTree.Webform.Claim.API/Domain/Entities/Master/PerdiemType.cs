using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("PerdiemType")]
    [Keyless]
    public class PerdiemType
    {
        public string AccountCode { get; set; }
        public string AccountDescription { get; set; }
        public string TravelerType { get; set; }
        public string RuleId { get; set; }
        public bool IsBankAccount { get; set; }
    }
}
