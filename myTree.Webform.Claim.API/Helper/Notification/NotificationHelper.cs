using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Serilog;
using CI.TMS.Claim.API.Helper.Notification;
using System.Net;
using Newtonsoft.Json;
using CI.TMS.Claim.API.Helper.Interfaces;

namespace CI.TMS.Claim.API.Helper.Notification
{
    public class NotificationHelper : INotificationHelper
    {
        private string EndPoint;
        public static IConfiguration config;

        public NotificationHelper(IConfiguration configuration)
        {
            config = configuration;
            EndPoint = config["NotificationEndpoint"].ToString();            
        }

        public NotificationModel.SendEmailResult SendEmail(NotificationModel.ParamSendEmail param, string AccessToken)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + AccessToken);
                    client.Headers.Add("Content-Type", "application/json");
                    var resultResponse = client.UploadString(EndPoint + "Send", JsonConvert.SerializeObject(param));

                    var apiResponse = JsonConvert.DeserializeObject<NotificationModel.SendEmailResult>(resultResponse);
                    client.Dispose();

                    if (!apiResponse.Status)
                        throw new Exception(apiResponse.Message.ToString());

                    return apiResponse;

                }
            }
            catch (Exception ex)
            {
                Log.Error("Error: {0} {1}", ex.Message, ((ex.InnerException != null) ? ex.InnerException.Message : ""));
                throw ex;
            }
        }
    }

    
}
