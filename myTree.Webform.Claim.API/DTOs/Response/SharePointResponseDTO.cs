using System.ComponentModel.DataAnnotations.Schema;

namespace CI.TMS.Claim.API.DTOs.Response
{
    public class SharePointResponseDTO
    {
        public class ErrorResponse
        {
            public string status_code { get; set; }
            public string message { get; set; }
        }

        public class GetFileUrlsResponse
        {
            public string status_code { get; set; }
            public List<SharePointFileList> fileList { get; set; }
            public string message { get; set; }

        }

        public class CopyFolderResponse
        {
            public string status_code { get; set; }
            public FileListAfterCopyResponse fileList { get; set; }
            public string message { get; set; }

        }

        public class AssignPermissionResponse
        {
            public string status_code { get; set; }
            public string message { get; set; }

        }

        public class UploadResponse
        {
            public string status_code { get; set; }
            public string module_id { get; set; }
            public List<SharePointFileList> fileList { get; set; }
            public string message { get; set; }

        }

        public class FileListAfterCopyResponse
        {
            public List<SharePointFileList> origin { get; set; }
            public List<SharePointFileList> destination { get; set; }

        }

        public class SharePointFileList
        {
            public string filename { get; set; }
            public string webUrl { get; set; }
        }
    }
}
