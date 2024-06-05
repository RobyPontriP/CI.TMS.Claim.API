using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwAgressoChartOfAccounts")]
    [Keyless]
    public class ChartOfAccounts
    {
        public string Account { get; set; }
        public string Rule { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
    }
}
