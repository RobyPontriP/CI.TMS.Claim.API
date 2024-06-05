using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CI.TMS.Claim.API.Domain;

namespace CI.TMS.Claim.API.Domain.Entities.K2
{
    [Table("tblDatalog")]
    public class DataLog
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("RelevantId")]
        public string? RelevantId { get; set; }

        [Column("Module")]
        public string? Module { get; set; }

        [Column("FieldName")]
        public string? FieldName { get; set; }

        [Column("Value")]
        public string? Value { get; set; }

        [Column("SeqNo")]
        public int? SeqNo { get; set; }
    }
}
