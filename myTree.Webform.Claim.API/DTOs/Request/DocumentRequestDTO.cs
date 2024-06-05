using CI.TMS.Claim.API.DTOs.Response;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Request
{
    public class DocumentRequestDTO
    {
        public Guid Id { get; set; }

        public string? FileName { get; set; }

        public string? FileURL { get; set; }
        public bool? IsActive { get; set; }
    }
}
