using CI.TMS.Claim.API.Helper.Notification;
using System.Collections.Generic;

namespace CI.TMS.Claim.API.Helper.Interfaces
{
    public interface INotificationHelper
    {
        public NotificationModel.SendEmailResult SendEmail(NotificationModel.ParamSendEmail param, string AccessToken);


    }
}