using CI.TMS.Claim.API.Domain.Entities;
using CI.TMS.Claim.API.DTOs.Request;
using CI.TMS.Claim.API.DTOs.Response;
using CI.TMS.Claim.API.Helper.Interfaces;
using CI.TMS.Claim.API.Helper.Notification;
using CI.TMS.Claim.API.Services;
using CI.TMS.Claim.API.Services.Master;
using myTree.MicroService.Helper;
using Serilog;
using System.Data;
using static Antlr4.Runtime.Atn.SemanticContext;

namespace CI.TMS.Claim.Web.Services
{
    public class NotificationServices
    {
        public static IConfiguration config;
        private Antlr3.ST.StringTemplateGroup group = new Antlr3.ST.StringTemplateGroup("EmailTemplate");
        private Antlr3.ST.StringTemplate template = new Antlr3.ST.StringTemplate();
        private ClaimDataService claimDataSvc;
        private EmployeeService employeeSvc;
        private FinanceOfficerService financeOfficerSvc;
        INotificationHelper notificationHelper;



        public NotificationServices(IConfiguration Config, ClaimDataService _claimDataSvc, EmployeeService _employeeSvc, FinanceOfficerService _financeOfficerSvc, INotificationHelper NotificationHelper)
        {
            config = Config;
            claimDataSvc = _claimDataSvc;
            employeeSvc = _employeeSvc;
            notificationHelper = NotificationHelper;
            financeOfficerSvc = _financeOfficerSvc;
        }

        private string ProcesTestModeTemplate(string MainRecipients, string Subject, string CCRecipients)
        {
            Antlr3.ST.StringTemplate template2 = group.GetInstanceOf("TestModeMain");
            template2.SetAttribute("TO", MainRecipients);
            template2.SetAttribute("CC", CCRecipients);
            template2.SetAttribute("SUBJECT", Subject);

            return template2.ToString();
        }
        private async Task<string> ProcessWithoutClaimNotification(ClaimDataResponseDTO claimDataResponseDTO, string MainRecipients, string RecipientCC, string Subject, bool IsNotificationTestMode, string accessToken, string action, string userId, string role, string page, IList<ClaimCommentRequestDTO> comment, string ListFinanceTeam)
        {
            group.RootDir = config["email_template_path"];
            template = group.GetInstanceOf("NotificationWithoutClaim");

            template.SetAttribute("TEST_MODE", IsNotificationTestMode);
            template.SetAttribute("PAGE_FINANCE", page.ToUpper() == "APPROVALFINANCE");
            template.SetAttribute("TO_CC", ProcesTestModeTemplate(MainRecipients, Subject, RecipientCC));
            template.SetAttribute("TRAVELER_TYPE", claimDataResponseDTO.TravelerType == "5");
            template.SetAttribute("TRAVELER_NAME", claimDataResponseDTO.TravelerName);
            template.SetAttribute("TRAVELER_GENDER", claimDataResponseDTO.TravelerGender);
            template.SetAttribute("TRAVELER_DUTY_POST", claimDataResponseDTO.TravelerDutyPost);
            template.SetAttribute("TRAVELER_WORKING_LOCATION", claimDataResponseDTO.TravelerWorkingLocation);
            template.SetAttribute("TEC_CODE", claimDataResponseDTO.SystemCode ?? "");
            template.SetAttribute("TA_CODE", claimDataResponseDTO.TACode ?? "");
            template.SetAttribute("CLAIM_PROFILE_URL", config["ClaimProfileUrl"].ToString() + claimDataResponseDTO.Id);
            template.SetAttribute("TA_PROFILE_URL", config["TAProfileUrl"].ToString() + claimDataResponseDTO.TAId);
            template.SetAttribute("COMMENT", comment.Select(x => x.Comment ?? "").FirstOrDefault());


            #region TravelDestination
            string Data = "";
            string Destination = "";
            int itd = 1;
            foreach (var data in claimDataResponseDTO.Destination)
            {
                if (itd % 2 != 0)
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TADestinationsNormal");
                    row.SetAttribute("destination", data.CityName + ", " + data.CountryName);
                    row.SetAttribute("purpose", data.PurposeOfTravel);
                    row.SetAttribute("date", data.StartDate.Value.ToString("dd MMMM yyyy") + " - " + data.EndDate.Value.ToString("dd MMMM yyyy"));
                    row.SetAttribute("typeoftravel", data.TypeOfTravel);
                    Destination += row.ToString();
                }
                else
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TADestinationsZebra");
                    row.SetAttribute("destination", data.CityName + ", " + data.CountryName);
                    row.SetAttribute("purpose", data.PurposeOfTravel);
                    row.SetAttribute("date", data.StartDate.Value.ToString("dd MMMM yyyy") + " - " + data.EndDate.Value.ToString("dd MMMM yyyy"));
                    row.SetAttribute("typeoftravel", data.TypeOfTravel);
                    Destination += row.ToString();
                }
                itd++;
            }


            #endregion TravelDestination

            #region TravelItinerary
            string Itinerary = string.Empty;
            int itn = 1;
            foreach (var dr in claimDataResponseDTO.TravelAuthorizationItinerary)
            {
                string From = string.Empty;
                string To = string.Empty;

                if (dr.AirportIdFrom == 99999) From = dr.AirportOtherFrom;
                else
                {
                    if (dr.NameFrom != dr.CityFrom) From = dr.CityFrom + "-" + dr.NameFrom + dr.IataFrom + dr.CountryFrom;
                    else From = dr.NameFrom + dr.IataFrom + dr.CountryFrom;
                }

                if (dr.AirportIdTo == 99999) From = dr.AirportOtherTo;
                else
                {
                    if (dr.NameTo != dr.CityTo) To = dr.CityTo + "-" + dr.NameTo + dr.IataTo + dr.CountryTo;
                    else To = dr.NameTo + dr.IataTo + dr.CountryTo;
                }


                if (itn % 2 != 0)
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TAItineraryNormal");
                    row.SetAttribute("startpoint", From);
                    row.SetAttribute("endpoint", To);
                    row.SetAttribute("classoftravel", dr.ClassOfTravel);
                    row.SetAttribute("travel_date_label", Convert.ToDateTime(dr.TravelDate).ToString("dd MMM yyyy"));
                    row.SetAttribute("travel_time_label", dr.TravelTime.ToString());
                    row.SetAttribute("travel_type", dr.TravelType.ToString());
                    row.SetAttribute("remarks", dr.Remarks.ToString());
                    Itinerary += row.ToString();
                }
                else
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TAItineraryZebra");
                    row.SetAttribute("startpoint", From);
                    row.SetAttribute("endpoint", To);
                    row.SetAttribute("classoftravel", dr.ClassOfTravel);
                    row.SetAttribute("travel_date_label", Convert.ToDateTime(dr.TravelDate).ToString("dd MMM yyyy"));
                    row.SetAttribute("travel_time_label", dr.TravelTime.ToString());
                    row.SetAttribute("travel_type", dr.TravelType.ToString());
                    row.SetAttribute("remarks", dr.Remarks.ToString());
                    Itinerary += row.ToString();
                }
                itn++;
            }
            #endregion

            #region TravelCostCenter
            string Costcenter = string.Empty;
            bool isHasCostCenter = false;
            int icc = 0;
            foreach (var dr in claimDataResponseDTO.ChargeCode)
            {
                isHasCostCenter = true;
                if (icc % 2 != 0)
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TAChargeCodesZebra");
                    row.SetAttribute("costCenter", dr.CostCenterName);
                    row.SetAttribute("t4", dr.WorkOrderName);
                    row.SetAttribute("entity_code", dr.EntityName);
                    row.SetAttribute("percentage", Convert.ToDouble(dr.Percentage).ToString("N2"));
                    row.SetAttribute("value", Convert.ToDouble(dr.CostCenterAmounUSD).ToString("N2"));
                    row.SetAttribute("remark", dr.Remarks);
                    Costcenter += row.ToString();
                }
                else
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TAChargeCodesNormal");
                    row.SetAttribute("costCenter", dr.CostCenterName);
                    row.SetAttribute("t4", dr.WorkOrderName);
                    row.SetAttribute("entity_code", dr.EntityName);
                    row.SetAttribute("percentage", Convert.ToDouble(dr.Percentage).ToString("N2"));
                    row.SetAttribute("value", Convert.ToDouble(dr.CostCenterAmounUSD).ToString("N2"));
                    row.SetAttribute("remark", dr.Remarks);
                    Costcenter += row.ToString();
                }
                icc++;
            }
            #endregion TravelCostCenter

            #region TravelExtended
            string ActualAirfare = string.Empty;
            string IsTicketRequired = string.Empty;
            string IsHotelRequired = string.Empty;
            string IsTravelInsuranceRequired = string.Empty;
            string Preferences = string.Empty;
            string AdditionalInformation = string.Empty;
            bool TicketRequiredApproval = false;
            bool IsAdvanceRequired = false;
            bool IsSameCurrencyAdvance = false;
            bool IsAllAmount = false;
            string FlightTicket = string.Empty;
            string TypeOfBooking = string.Empty;
            string AmountAllAdvance = string.Empty;
            string TextAmountAdvance = string.Empty;
            string AdvanceDate = string.Empty;
            string AllAmountAdvance = string.Empty;
            decimal AdvancedRequiredOtherCurrencyPrice = 0;
            decimal AdvancedRequiredOtherCurrencyPriceSecond = 0;

            int iex = 0;
            foreach (var dr in claimDataResponseDTO.TravelAuthorizationExtended)
            {
                if (dr.ActualAirfareCur == "") ActualAirfare = "USD " + Convert.ToDecimal(dr.ActualAirfare).ToString("N2");
                else ActualAirfare = dr.ActualAirfareCur + " " + Convert.ToDecimal(dr.ActualAirfare).ToString("N2");

                if (dr.IsTicketRequired == 1)
                {
                    IsTicketRequired = "Yes";
                    TicketRequiredApproval = true;
                }
                else
                {
                    IsTicketRequired = "No";
                    TicketRequiredApproval = false;
                }
                if (dr.IsHotelRequired == 1) IsHotelRequired = "Yes"; else IsHotelRequired = "No";
                if (dr.IsTravelInsuranceRequired == 1) IsTravelInsuranceRequired = "Yes"; else IsTravelInsuranceRequired = "No";
                Preferences = dr.Preference.ToString();
                AdditionalInformation = dr.AdditionalInfo.ToString();
                if (dr.FlightTicket == 1)
                    FlightTicket = "One-way";
                else if (dr.FlightTicket == 2)
                    FlightTicket = "Round trip";
                else if (dr.FlightTicket == 3)
                    FlightTicket = "Multi cities";
                else
                    FlightTicket = "-";

                if (dr.FlexibleTravelDates == 0)
                    TypeOfBooking = "Fixed dates";
                else if (dr.FlexibleTravelDates == 1)
                    TypeOfBooking = "Flexible travel date";
                else if (dr.FlexibleTravelDates == 2)
                    TypeOfBooking = "Changeable ticket";
                else if (dr.FlexibleTravelDates == 3)
                    TypeOfBooking = "Changeable & refundable ticket";
                else
                    TypeOfBooking = "-";

                IsAdvanceRequired = dr.AdvanceRequired == 0 ? false : true;
                if (dr.AdvancedRequiredOtherCurrencyId == dr.AdvancedRequiredOtherCurrencyIdSecond)
                    IsSameCurrencyAdvance = true;
                if (IsAdvanceRequired == true && IsSameCurrencyAdvance == true)
                    IsAllAmount = true;
                else if (IsAdvanceRequired == true && IsSameCurrencyAdvance == false)
                    IsAllAmount = false;


                decimal AmountAdvanceAll = Convert.ToDecimal(dr.AdvancedRequiredOtherCurrencyPrice + dr.AdvancedRequiredOtherCurrencyPriceSecond);
                AllAmountAdvance = dr.AdvancedRequiredOtherCurrencyId + ' ' + AmountAdvanceAll.ToString("N2");

                AdvancedRequiredOtherCurrencyPrice = Convert.ToDecimal(dr.AdvancedRequiredOtherCurrencyPrice);
                AdvancedRequiredOtherCurrencyPriceSecond = Convert.ToDecimal(dr.AdvancedRequiredOtherCurrencyPriceSecond);

                if (Convert.ToDecimal(AdvancedRequiredOtherCurrencyPrice) > 0 && Convert.ToDecimal(AdvancedRequiredOtherCurrencyPriceSecond) > 0)
                    TextAmountAdvance = dr.AdvancedRequiredOtherCurrencyId + " " + AdvancedRequiredOtherCurrencyPrice.ToString("N2") + " and " + dr.AdvancedRequiredOtherCurrencyIdSecond + " " +AdvancedRequiredOtherCurrencyPriceSecond.ToString("N2");
                else if (Convert.ToDecimal(AdvancedRequiredOtherCurrencyPrice) > 0 && Convert.ToDecimal(AdvancedRequiredOtherCurrencyPriceSecond) <= 0)
                    TextAmountAdvance = dr.AdvancedRequiredOtherCurrencyId + " " + AdvancedRequiredOtherCurrencyPrice.ToString("N2");
                else
                    TextAmountAdvance = dr.AdvancedRequiredOtherCurrencyIdSecond + " " + AdvancedRequiredOtherCurrencyPriceSecond.ToString("N2");

                AdvanceDate = Convert.ToDateTime(dr.AdvanceRequiredDate).ToString("dd MMMM yyyy");

                iex++;
            }
            #endregion TravelExtended

            #region Set All Data to Set Attribute
            Antlr3.ST.StringTemplate tempItinerary = group.GetInstanceOf("TAItinerary");
            tempItinerary.SetAttribute("itinerary", Itinerary);
            template.SetAttribute("itinerary", tempItinerary.ToString());

            Antlr3.ST.StringTemplate tempCostcenter = group.GetInstanceOf("TAChargecodes");
            tempCostcenter.SetAttribute("datachargecodes", Costcenter);
            template.SetAttribute("datachargecodes", tempCostcenter.ToString());

            Antlr3.ST.StringTemplate tempDestination = group.GetInstanceOf("TADestinations");
            tempDestination.SetAttribute("destinations", Destination);
            template.SetAttribute("destinations", tempDestination.ToString());

            template.SetAttribute("ActualAirfare", ActualAirfare);
            template.SetAttribute("IsTicketRequired", IsTicketRequired);
            template.SetAttribute("IsHotelRequired", IsHotelRequired);
            template.SetAttribute("IsTravelInsuranceRequired", IsTravelInsuranceRequired);
            template.SetAttribute("Preferences", Preferences);
            template.SetAttribute("AdditionalInformation", AdditionalInformation);
            template.SetAttribute("TicketRequiredApproval", TicketRequiredApproval);
            template.SetAttribute("FlightTicket", FlightTicket);
            template.SetAttribute("TypeOfBooking", TypeOfBooking);
            template.SetAttribute("IsAdvanceRequired", IsAdvanceRequired);
            template.SetAttribute("IsAllAmount", IsAllAmount);
            template.SetAttribute("AllAmountAdvance", AllAmountAdvance);
            template.SetAttribute("TextAmountAdvance", TextAmountAdvance);
            template.SetAttribute("AdvanceDate", AdvanceDate);

            template.SetAttribute("hasCostCenter", isHasCostCenter);
            string listFinance = "";
            string emp = "";
            listFinance = ListFinanceTeam;

            template.SetAttribute("TEAMFINANCE", listFinance);
            template.SetAttribute("EMPLOYEE", emp);

            #endregion



            return template.ToString();
        }
        private async Task<string> ProcessClaimNotification(ClaimDataResponseDTO claimDataResponseDTO, string MainRecipients, string RecipientCC, string Subject, bool IsNotificationTestMode, string accessToken, string action, string userId, string role, string page, IList<ClaimCommentRequestDTO> comment, string ListFinanceTeam)
        {
            group.RootDir = config["email_template_path"];
            if (action.ToUpper() == "APPROVED")
            {
                template = group.GetInstanceOf("NotificationClaimApproved");
            }
            else if (action.ToUpper() == "REJECTED")
            {
                template = group.GetInstanceOf("NotificationClaimRejected");
            }
            else if (action.ToUpper() == "SUBMITTED")
            {
                template = group.GetInstanceOf("NotificationWithoutClaim");
            }


            template.SetAttribute("TEST_MODE", IsNotificationTestMode);
            template.SetAttribute("PAGE_FINANCE", page.ToUpper() == "APPROVALFINANCE");
            template.SetAttribute("TO_CC", ProcesTestModeTemplate(MainRecipients, Subject, RecipientCC));
            template.SetAttribute("TRAVELER_TYPE", claimDataResponseDTO.TravelerType == "5");
            template.SetAttribute("TRAVELER_NAME", claimDataResponseDTO.TravelerName);
            template.SetAttribute("TRAVELER_GENDER", claimDataResponseDTO.TravelerGender);
            template.SetAttribute("TRAVELER_DUTY_POST", claimDataResponseDTO.TravelerDutyPost);
            template.SetAttribute("TRAVELER_WORKING_LOCATION", claimDataResponseDTO.TravelerWorkingLocation);
            template.SetAttribute("TEC_CODE", claimDataResponseDTO.SystemCode ?? "");
            template.SetAttribute("TA_CODE", claimDataResponseDTO.TACode ?? "");
            template.SetAttribute("CLAIM_PROFILE_URL", config["ClaimProfileUrl"].ToString() + claimDataResponseDTO.Id);
            template.SetAttribute("TA_PROFILE_URL", config["TAProfileUrl"].ToString() + claimDataResponseDTO.TAId);
            template.SetAttribute("COMMENT", comment.Select(x => x.Comment ?? "").FirstOrDefault());


            #region TravelDestination
            string Data = "";
            string Destination = "";
            int itd = 1;
            foreach (var data in claimDataResponseDTO.Destination)
            {
                if (itd % 2 != 0)
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TADestinationsNormal");
                    row.SetAttribute("destination", data.CityName + ", " + data.CountryName);
                    row.SetAttribute("purpose", data.PurposeOfTravel);
                    row.SetAttribute("date", data.StartDate.Value.ToString("dd MMMM yyyy") + " - " + data.EndDate.Value.ToString("dd MMMM yyyy"));
                    row.SetAttribute("typeoftravel", data.TypeOfTravel);
                    Destination += row.ToString();
                }
                else
                {
                    Antlr3.ST.StringTemplate row = group.GetInstanceOf("TADestinationsZebra");
                    row.SetAttribute("destination", data.CityName + ", " + data.CountryName);
                    row.SetAttribute("purpose", data.PurposeOfTravel);
                    row.SetAttribute("date", data.StartDate.Value.ToString("dd MMMM yyyy") + " - " + data.EndDate.Value.ToString("dd MMMM yyyy"));
                    row.SetAttribute("typeoftravel", data.TypeOfTravel);
                    Destination += row.ToString();
                }
                itd++;
            }
            Antlr3.ST.StringTemplate tempDestination = group.GetInstanceOf("TADestinations");
            tempDestination.SetAttribute("destinations", Destination);
            template.SetAttribute("destinations", tempDestination.ToString());

            #endregion TravelDestination

            string listFinance = "";
            string emp = "";
            if ((action.ToUpper() == "APPROVED" || action.ToUpper() == "REJECTED"))
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var FinanceEmployeeByUsername = await employeeSvc.GetEmployeeByUsername(userId);
                    if (FinanceEmployeeByUsername != null)
                    {
                        if (!string.IsNullOrEmpty(FinanceEmployeeByUsername.Email))
                        {
                            listFinance += "<a href='mailto:" + FinanceEmployeeByUsername.Email + "' target='_blank' rel='noopener noreferrer' data-auth='NotApplicable' data-safelink='true' data-linkindex='4' class='email-footer'>" + FinanceEmployeeByUsername.Email + "</a>.";
                        }

                        if (!string.IsNullOrEmpty(FinanceEmployeeByUsername.EmpName))
                        {
                            emp = FinanceEmployeeByUsername.EmpName;
                        }
                    }

                }
            }
            else
            {
                listFinance = ListFinanceTeam;
            }

            template.SetAttribute("TEAMFINANCE", listFinance);
            template.SetAttribute("EMPLOYEE", emp);

            return template.ToString();
        }

        public async Task SendClaimNotification(string TAId, Guid Id, string accessToken, string action, string userId, string role, string page, IList<ClaimCommentRequestDTO> comment)
        {
            try
            {
                bool isNotificationTestMode = config["send_mail_test"].ToString() == "1" ? true : false;
                bool isSendEmail = config["send_mail"].ToString() == "1" ? true : false;

                ClaimDataResponseDTO claimDataResponseDTO = await claimDataSvc.GetAllData((action.ToUpper() == "APPROVED" || action.ToUpper() == "REJECTED") ? "null" : TAId, (action.ToUpper() == "APPROVED" || action.ToUpper() == "REJECTED") ? Id : Guid.Empty, accessToken);


                string subject = string.Empty;
                var financeTeam = new List<string> { };
                string ListFinanceTeam = string.Empty;
                string to = string.Empty;
                string cc = string.Empty;
                var toList = new List<string> { };
                var ccList = new List<string> { };
                string template = string.Empty;

                if (((claimDataResponseDTO.Id != Guid.Empty && (action.ToUpper() == "APPROVED" || action.ToUpper() == "REJECTED")) || action.ToUpper() == "SUBMITTED"))
                {
                    if (action.ToUpper() == "APPROVED")
                    {
                        subject = "Your claim is approved " + claimDataResponseDTO.TravelerName;
                    }
                    else if (action.ToUpper() == "REJECTED")
                    {
                        subject = "Your claim is rejected " + claimDataResponseDTO.TravelerName;
                    }
                    else
                    {
                        subject = "Travel authorization " + claimDataResponseDTO.TACode + " for " + claimDataResponseDTO.TravelerName + " has nothing to claim";
                    }

                    if ((action.ToUpper() == "APPROVED" || action.ToUpper() == "REJECTED"))
                    {
                        //Initiator
                        if (!string.IsNullOrEmpty(claimDataResponseDTO.CreatedBy))
                        {
                            var employeeByUsername = await employeeSvc.GetEmployeeByUsername(claimDataResponseDTO.CreatedBy);
                            if (employeeByUsername != null)
                            {
                                if (!string.IsNullOrEmpty(employeeByUsername.Email))
                                {
                                    toList.Add(employeeByUsername.Email);
                                }
                            }
                        }
                        //traveler
                        var employeeById = await employeeSvc.GetEmployeeByClaimId(claimDataResponseDTO.Id);
                        if (employeeById != null)
                        {
                            if (!string.IsNullOrEmpty(employeeById.Email))
                            {
                                toList.Add(employeeById.Email);
                            }
                        }
                    }
                    else  // Without Claim to All Finance officer from the spesific travel office
                    {
                        var cFinanceObj = await financeOfficerSvc.Get(x => x.TravelOfficeId.Equals(claimDataResponseDTO.TravelOfficeId) && x.Status == true);
                        if (cFinanceObj.Count() > 0)
                        {
                            foreach (var obj in cFinanceObj.Distinct())
                            {
                                if (!string.IsNullOrEmpty(obj.Email))
                                {
                                    toList.Add(obj.Email);
                                    financeTeam.Add("<a href='mailto:" + obj.Email + "' target='_blank' rel='noopener noreferrer' data-auth='NotApplicable' data-safelink='true' data-linkindex='4' class='email-footer'>" + obj.Email + "</a>");
                                }
                            }
                        }
                    }


                    to = string.Join(";", toList.Distinct());
                    ListFinanceTeam = string.Join(", ", financeTeam.Distinct());
                    if ((action.ToUpper() == "APPROVED" || action.ToUpper() == "REJECTED"))
                    {
                        cc = string.Join(";", ccList.Except(toList));
                        template = await ProcessClaimNotification(claimDataResponseDTO, to, cc, subject, isNotificationTestMode, accessToken, action, userId, role, page, comment, ListFinanceTeam);
                    }
                    else
                        template = await ProcessWithoutClaimNotification(claimDataResponseDTO, to, cc, subject, isNotificationTestMode, accessToken, action, userId, role, page, comment, ListFinanceTeam);


                    if (isNotificationTestMode)
                    {
                        to = config["mail_user"];
                        cc = config["mail_user_cc"];
                    }

                    if (isSendEmail)
                    {
                        NotificationModel.ParamSendEmail param = new NotificationModel.ParamSendEmail()
                        {
                            Subject = subject,
                            App_Sender = config["AppSender"],
                            Is_HTML = 1,
                            Priority = 0,
                            Sender = config["mail_sender"],
                            Recipient = to,
                            Recipient_CC = cc,
                            Recipient_BCC = config["bcc_email"],
                            Message = template,
                            Send_Time = DateTime.Now
                        };

                        var result = notificationHelper.SendEmail(param, accessToken);
                    }
                }
                else
                {
                    throw new Exception("No data available to be sent");
                }
            }
            catch (Exception ex)
            {
                Log.Information("Error Claim Notification with ID: " + Id.ToString() + ". Check error detail below!");
                Log.Error("Error: {0} {1} {2} {3}", ex.LineNumber(), ex.Detail(), ex.Message, ((ex.InnerException != null) ? ex.InnerException.Message : ""));
                throw;
            }
        }

    }
}
