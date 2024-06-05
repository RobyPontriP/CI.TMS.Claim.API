using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class DocumentResponseDTO
    {
        public Guid Id { get; set; }

        public string? FileName { get; set; }

        public string? FileURL { get; set; }
        public bool? IsActive { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
    }
}
