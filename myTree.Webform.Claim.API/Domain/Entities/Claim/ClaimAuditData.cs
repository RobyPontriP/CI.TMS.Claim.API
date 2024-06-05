using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("ClaimAuditData")]
    public class ClaimAuditData
    {
        [Key]
        public  Guid Id { get; set; }
        public  string ModuleId { get; set; }
        public string SubModule { get; set; }
        public string  RecId { get; set; }
        public string ChangeBy { get; set; }
        public DateTime ChangeTime { get; set; }
        public string ChangeType { get; set; }
        public string FieldName { get; set; }
        public string PreviousValue { get; set; }
        public string NewValue { get; set; }
        public string ReasonOfChange { get; set; }
        public int? ApprovalNo { get; set; }
        public int? SeqNo { get; set; }
    }
}
