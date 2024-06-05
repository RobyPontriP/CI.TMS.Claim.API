using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class EmployeeResponseDTO
    {
        public string? Address { get; set; }
        public string? DeptId { get; set; }
        public string? DeptName { get; set; }
        public string? DirectSupEmpId { get; set; }
        public string? DirectSupEmpName { get; set; }
        public string? DivCode { get; set; }
        public string? DivName { get; set; }
        public string? DutyptCode { get; set; }
        public string? Email { get; set; }
        public DateTime? EmpBirthday { get; set; }
        public DateTime? EmpConfirmDate { get; set; }
        public string? EmpId { get; set; }
        public string? EmpName { get; set; }
        public string? EmpStatus { get; set; }
        public string? EmpUserId { get; set; }
        public string? Gender { get; set; }
        public string? RowId { get; set; }
        public string? LegalEntityId { get; set; }
    }
}
