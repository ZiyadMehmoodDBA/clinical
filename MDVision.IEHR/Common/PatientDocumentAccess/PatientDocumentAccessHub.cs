using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MDVision.Common.Utilities;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using System.Globalization;

namespace MDVision.IEHR.Common.PatientDocumentAccess
{


    public class PatientDocumentAccessHub : Hub
    {

        public static List<OnlineUser> OnlineUsers = new List<OnlineUser>();

        public override Task OnConnected()
        {
            var Username = Context.QueryString["Username"];
            var UserId = Context.QueryString["UserId"];
            var PatDocID = Context.QueryString["PatDocID"];           

            var existRecord = OnlineUsers.Where(x => x.UserId == UserId.ToLower()).FirstOrDefault();

            if (existRecord == null)
            {
                OnlineUser newUser = new OnlineUser()
                {
                    Username = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Username),
                    UserId = UserId,
                    PatDocID = PatDocID,
                    ConnectionID = Context.ConnectionId
                };
                OnlineUsers.Add(newUser);
                return base.OnConnected();
            }
            else
            {
                existRecord.ConnectionID = Context.ConnectionId;
                return base.OnConnected();
            }
        }

        public string ConcurrencyDocumentAccessAlert(string Username, string PatDocID,string UserID)
        {
            string resFunction =string.Empty;

            try
            {
                if (OnlineUsers.FirstOrDefault(p => p.PatDocID == PatDocID) != null)
                {
                    HubResponse response = new HubResponse();

                    var llistString = OnlineUsers.Where(p => p.PatDocID == PatDocID && p.UserId != UserID).FirstOrDefault();
                    if (llistString != null)
                    {
                       
                        response.UserName = llistString.Username;
                        response.status = true;
                        resFunction = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                    }
                    else {
                        response.Message = string.Empty;
                        response.status = false;
                        resFunction = Newtonsoft.Json.JsonConvert.SerializeObject(response); 
                    }

                }
            }
            catch (Exception ex)
            {
                resFunction = string.Empty;
            }

            return resFunction;
        }

        public string ConcurrencyAlertMessageToCurrentUser(string PatDocID, string UserID)
        {

            string resFunction = string.Empty;
            try
            {
                if (OnlineUsers.FirstOrDefault(p => p.PatDocID == PatDocID) != null)
                {
                    HubResponse response = new HubResponse();
                    string lConnectionID = string.Empty;

                    var llistString = OnlineUsers.Where(p => p.PatDocID == PatDocID && p.UserId != UserID).FirstOrDefault();
                    var getUserName = OnlineUsers.Where(p => p.PatDocID == PatDocID && p.UserId == UserID).FirstOrDefault();

                    if (llistString != null)
                    {

                        response.UserName = getUserName.Username;    
                        resFunction = Newtonsoft.Json.JsonConvert.SerializeObject(response);
                        Clients.Client(llistString.ConnectionID).otherUserAccessDocument(Newtonsoft.Json.JsonConvert.SerializeObject(response));                     
                        resFunction = getUserName.Username;


                    }

                }
            }
            catch (Exception ex)
            {
                resFunction = string.Empty;
            }

            return resFunction;
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                // We know that Stop() was called on the client,
                // and the connection shut down gracefully.
                var usr = OnlineUsers.Where(x => x.ConnectionID == Context.ConnectionId).FirstOrDefault();
                if (usr != null)
                {
                    OnlineUsers.Remove(usr);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

    }

    public partial class OnlineUser
    {

        public OnlineUser()
        {

        }
        public string ConnectionID { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string PatDocID { get; set; }

    }

    public partial class HubResponse
    {
        public string UserName { get; set; }     
        public string Message { get; set; }
        public bool status { get; set; }
        public string PatDocID { get; set; }

    }
}