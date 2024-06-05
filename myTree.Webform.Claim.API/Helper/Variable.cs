namespace CI.TMS.Claim.API.Helper
{
    public class Variable
    {
        public static IConfiguration Configuration;

        public class WorkflowCommand
        {
            public const string Submit = "SUBMIT";
            public const string Resubmit = "RESUBMIT";
            public const string Revise = "REVISE";
            public const string Reject = "REJECT";
            public const string Approve = "APPROVE";
            public const string Recommend = "RECOMMEND";
            public const string Redirect = "REDIRECT";
            public const string Verify = "VERIFY";
            public const string Upload = "UPLOAD";
            public const string Save = "SAVE";
        }

        public class WorkflowStatus
        {
            public const string Submitted = "SUBMITTED";
            public const string Draft = "DRAFT";
            public const string Resubmitted = "RESUBMITTED";
            public const string Revised = "REVISED";
            public const string Rejected = "REJECTED";
            public const string Approved = "APPROVED";
            public const string Recommended = "RECOMMENDED";
            public const string Redirected = "REDIRECTED";
            public const string Verified = "VERIFIED";
            public const string Uploaded = "UPLOADED";
            public const string Saved = "SAVED";

            public const string WaitingForVerification = "WAITING FOR VERIFICATION";
            public const string WaitingForApproval = "WAITING FOR APPROVAL";
            public const string WaitingForRecommendation = "WAITING FOR RECOMMENDATION";
            public const string WaitingForUpload = "WAITING FOR UPLOAD";
            public const string WaitingForFinalization = "WAITING FOR FINALIZATION";
            public const string WaitingForAcceptance = "WAITING FOR ACCEPTANCE";
        }

    }
}
