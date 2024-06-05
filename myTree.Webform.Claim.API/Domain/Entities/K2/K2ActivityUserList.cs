using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CI.TMS.Claim.API.Domain.Entities.K2
{
    [Table("K2_ACTIVITY_USER_LIST")]
    public class K2ActivityUserList
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("relevant_id")]
        public string? RelevantId { get; set; }

        [Column("folder")]
        public string? Folder { get; set; }

        [Column("process_name")]
        public string? ProcessName { get; set; }

        [Column("activity_name")]
        public string? ActivityName { get; set; }

        [Column("activity_id")]
        public int? ActivityId { get; set; }

        [Column("activity_user")]
        public string? ActivityUser { get; set; }

        [Column("seq_no")]
        public int? SeqNo { get; set; }
    }
}
