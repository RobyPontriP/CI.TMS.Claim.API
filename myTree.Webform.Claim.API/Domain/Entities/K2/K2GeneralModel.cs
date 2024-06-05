namespace CI.TMS.Claim.API.Domain.Entities.K2
{
    public class K2GeneralModel
    {
        public class ApiResponse
        {
            public bool? Status { get; set; }
            public object? Message { get; set; }
        }

        public class SubmitModel
        {
            public string Id { get; set; }
            public string Folder { get; set; }
            public string ProcessName { get; set; }
            public string Username { get; set; }
            public Dictionary<string, object>? Data { get; set; }
        }

        public class GoToActivityModel
        {
            public string Folio { get; set; }
            public string ActivityName { get; set; }
            public Dictionary<string, object>? Data { get; set; }
        }

        public class DelegateRedirectModel
        {
            public string Folio { get; set; }
            public string ActivityName { get; set; }
            public string SourceUser { get; set; }
            public string DestinationUser { get; set; }
        }

        public class ApprovalModel
        {
            public string Username { get; set; }
            public List<string>? SN { get; set; }
            public string ActionName { get; set; }
            public Dictionary<string, object>? Data { get; set; }
        }

        public class ApprovalModelAdmin
        {
            public string Action { get; set; }
            public string Folio { get; set; }
            public bool? RemoveLog { get; set; }
        }

        public class TasklistModel
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string TaskName { get; set; }
            public string Url { get; set; }
            public string UrlDetail { get; set; }
            public string ActivityName { get; set; }
            public string ActivityId { get; set; }
            public string SN { get; set; }
            public string DueDate { get; set; }
        }

        public class DifferentData
        {
            public string FieldName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public int? Index { get; set; }
            public int? SubIndex { get; set; }

        }
    }
}
