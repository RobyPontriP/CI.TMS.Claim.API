using System;
using System.Collections.Generic;
using System.Text;

namespace CI.TMS.Claim.API.Helper.Notification
{
    public class NotificationModel
    {
        public class SendEmailResult
        {
            public string Action { get; set; }
            public string Redirect { get; set; }
            public string ModuleId { get; set; }
            public string Error { get; set; }
            public string Message { get; set; }
            public bool Status { get; set; }
        }

        public class ParamSendEmail { 
            public string Subject { get; set; }
            public int Priority { get; set; }
            public int Is_HTML { get; set; }
            public string Sender { get; set; }
            public string Recipient { get; set; }
            public string Recipient_CC { get; set; }
            public string Recipient_BCC { get; set; }
            public string Message { get; set; }
            public string App_Sender { get; set; }
            public DateTime Send_Time { get; set; }
        }
    }
}
