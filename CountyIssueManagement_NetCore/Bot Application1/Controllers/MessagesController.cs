using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
//using WebApp_OpenIDConnect_DotNet_B2C.Models;
//using WebApp_OpenIDConnect_DotNet_B2C.DatabaseContext;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task <HttpResponseMessage> Post([FromBody] Activity message)
       {
            ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
            if (message.Type == "message")
            {
                string StockRateString;
                CountyLUIS StLUIS = await GetEntityFromLUIS(message.Text);
                if (StLUIS.intents.Count() > 0 && StLUIS.entities.Count() > 1)
                {
                    switch (StLUIS.intents[0].intent)
                    {
                        case "AddComplaints":
                            StockRateString = await AddComplaint(StLUIS.entities[0].entity, StLUIS.entities[1].entity, message.Text);
                            break;
                        case "GetComplaintsStatus":
                            StockRateString = await AddComplaint(StLUIS.entities[0].entity, "empty", message.Text);
                            break;
                        default:
                            StockRateString = "Sorry, I am not getting you. :(\nPlease try contacting our super friendly officer at  +1 425-882-8080";
                            break;
                    }
                }

                else if (StLUIS.intents.Count() > 0 && StLUIS.entities.Count() > 0)
                {
                    StateClient stateClient = message.GetStateClient();
                    BotData userData = await stateClient.BotState.GetUserDataAsync(message.ChannelId, message.From.Id);
                    switch (StLUIS.intents[0].intent)
                    {
                        case "AddComplaints":
                            
                            
                            userData.SetProperty<string>("entity1", StLUIS.entities[0].entity);
                            await stateClient.BotState.SetUserDataAsync(message.ChannelId, message.From.Id, userData);
                            
                            StockRateString = await AddComplaint(StLUIS.entities[0].entity);
                            break;
                        case "GetComplaintsStatus":
                            StockRateString = await AddComplaint(StLUIS.entities[0].entity, "empty", message.Text);
                            break;

                        case "EnterLocation":

                            string intent = userData.GetProperty<string>("entity1").ToString();
                            StockRateString = await AddComplaint(intent, StLUIS.entities[0].entity, message.Text);
                            break;

                        default:
                            StockRateString = "Sorry, I am not getting you. :(\nPlease try contacting our super friendly officer at  +1 425-882-8080";
                            break;
                    }
                }
                else
                {
                    StockRateString = "Sorry, I am not getting you. :(\nPlease try contacting our super friendly officer at  +1 425-882-8080";
                }

                // return our reply to the user  
                Activity m = message.CreateReply(StockRateString);
                await connector.Conversations.ReplyToActivityAsync(m);
                

            }
            else
            {
                HandleSystemMessage(message);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private static async Task<CountyLUIS> GetEntityFromLUIS(string Query)
        {
            Query = Uri.EscapeDataString(Query);
            CountyLUIS Data = new CountyLUIS();
            using (HttpClient client = new HttpClient())
            {
                string RequestURI = WebConfigurationManager.AppSettings["LUISAPIURI"] + Query;
                

                HttpResponseMessage msg = await client.GetAsync(RequestURI);

                if (msg.IsSuccessStatusCode)
                {
                    var JsonDataResponse = await msg.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<CountyLUIS>(JsonDataResponse);
                }
            }
            return Data;
        }
        //se
        private async Task<string> AddComplaint(string ComplaintType, string Locationx, string Message)
        {
            //string Description = "this is a description";
            //string Creator = "bot" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
            using (SqlConnection openCon = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString()))
            {




                if (ComplaintType == null)
                {
                    return string.Format("I see there is a \"{0}\" at \"{1}\"", ComplaintType, Locationx);
                }
                else
                {

                    string saveIssue = "INSERT into Issues (Description,Creator,IssueCategoryId, IssueStatusId, Location) VALUES (@Description,@Creator,@icid,@isid,@Location)";

                    using (SqlCommand querysaveIssue = new SqlCommand(saveIssue))
                    {
                        querysaveIssue.Connection = openCon;
                        querysaveIssue.Parameters.Add("@Description", SqlDbType.NVarChar, 30).Value = Message;
                        querysaveIssue.Parameters.Add("@Creator", SqlDbType.NVarChar, 30).Value = "bot" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
                        querysaveIssue.Parameters.Add("@icid", SqlDbType.Int, 30).Value = 1;
                        querysaveIssue.Parameters.Add("@isid", SqlDbType.Int, 30).Value = 1;
                        querysaveIssue.Parameters.Add("@Location", SqlDbType.NVarChar, 30).Value = Locationx;
                        try
                        {
                            openCon.Open();
                            int recordsAffected = querysaveIssue.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            return "error" + e;
                        }


                    }

                    return string.Format("I see there is a \"{0}\" at \"{1}\" we will have someone attend to this.", ComplaintType, Locationx);

                    

                    //use entity framework to log this
                }
            }
        }

        private async Task<string> AddComplaint(string ComplaintType)
        {

            if (ComplaintType == null)
            {
                return string.Format("I see there is a \"{0}\" at \"{1}\"", ComplaintType);
            }
            else
            {
                return string.Format("I see there is a \"{0}\" but where?", ComplaintType);

                //use entity framework to log this
            }
        }
    }
}