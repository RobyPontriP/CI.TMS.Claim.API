using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.Domain.Entities
{
    [Table("vwCIF_EMPLOYEES")]
    public class Employee
    {
        [Column("ADDRESS")]
        public string? Address { get; set; }
        [Column("DEPT_ID")]
        public string? DeptId { get; set; }
        [Column("DEPT_NAME")]
        public string? DeptName { get; set; }
        [Column("DIRECT_SUP_EMP_ID")]
        public string? DirectSupEmpId { get; set; }
        [Column("DIRECT_SUP_NAME")]
        public string? DirectSupEmpName { get; set; }
        [Column("DIV_CODE")]
        public string? DivCode { get; set; }
        [Column("DIV_NAME")]
        public string? DivName { get; set; }
        [Column("DUTYPT_CODE")]
        public string? DutyptCode { get; set; }
        [Column("EMAIL")]
        public string? Email { get; set; }
        [Column("EMP_BIRTHDAY")]
        public DateTime? EmpBirthday { get; set; }
        [Column("EMP_CONFIRM_DATE")]
        public DateTime? EmpConfirmDate { get; set; }
        [Column("EMP_ID")]
        public string? EmpId  { get; set; }
        [Column("EMP_NAME")]
        public string? EmpName { get; set; }
        [Column("EMP_STATUS")]
        public string? EmpStatus { get; set; }
        [Key]
        [Column("EMP_USER_ID")]
        public string? EmpUserId { get; set; }
        [Column("GENDER")]
        public int? Gender { get; set; }
        [Column("ROW_ID")]
        public string? RowId { get; set; }
        [Column("LEGAL_ENTITY_ID")]
        public string? LegalEntityId { get; set; }
    }
}
