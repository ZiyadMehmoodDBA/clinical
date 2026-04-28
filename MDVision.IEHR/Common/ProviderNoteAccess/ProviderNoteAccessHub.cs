using MDVision.Common.Shared;
using MDVision.Common.Utilities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MDVision.IEHR.Common.ProviderNoteAccess
{
    public class ProviderNoteAccessHub : Hub
    {
        public static List<OnlineUser> OnlineUsers = new List<OnlineUser>();

        public override Task OnConnected()
        {
            var Username = Context.QueryString["Username"];
            var UserId = Context.QueryString["UserId"];
            var NotesId = Context.QueryString["NotesId"];
            var existRecord = OnlineUsers.Where(x => x.Username == Username.ToLower()).FirstOrDefault();
            if (existRecord == null)
            {
                OnlineUser newUser = new OnlineUser()
                {
                    Username = Username.ToLower(),
                    UserId = UserId,
                    NotesId = NotesId,
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

        public bool RevokeComponentAccess(string Username,string PriorUserName, string PriorUserId, string ComponentName)
        {
            bool resFunction = true;
            try
            {
                if (OnlineUsers.FirstOrDefault(p => p.Username == PriorUserName.ToLower() && p.UserId == PriorUserId) != null)
                {
                    HubResponse res = new HubResponse();
                    res.ComponentName = ComponentName;
                    res.UserName = Username.ToLower();
                    res.status = true;
                    res.Message = MDVUtility.ToTitleCase(Username) + "  has revoked your access to this component.";

                    Clients.Client(OnlineUsers
                                    .FirstOrDefault(p => p.Username == PriorUserName.ToLower() && p.UserId == PriorUserId).ConnectionID)
                                    .releaseNoteComponentAccess(Newtonsoft.Json.JsonConvert.SerializeObject(res));
                }
                else
                    resFunction = false;
            }
            catch (Exception ex)
            {
                resFunction = false;
            }

            return resFunction;
        }

        public bool revokeNoteSignAccess(string NotesId, string AppUserId, string appusername)
        {
            bool resFunction = true;
            try
            {
                if (OnlineUsers.FirstOrDefault(p => p.NotesId == NotesId) != null)
                {
                    HubResponse res = new HubResponse();
                    res.ComponentName = "SignNote";
                    res.status = true;
                    res.NotesId = NotesId;
                    res.UserName = appusername;
                    Clients.Clients(OnlineUsers.Where(p => p.NotesId == NotesId && p.UserId != AppUserId).ToList().Select(x=>x.ConnectionID).ToArray()).RefreshOtherUserAfterSignNote(Newtonsoft.Json.JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                resFunction = false;
            }

            return resFunction;
        }
        public bool DisableSignNoteBtn(string NotesId, string AppUserId)
        {
            bool resFunction = true;
            try
            {
                if (OnlineUsers.FirstOrDefault(p => p.NotesId == NotesId) != null)
                {
                    HubResponse res = new HubResponse();
                    res.ComponentName = "SignNote";
                    res.status = true;
                    res.NotesId = NotesId;
                    Clients.Clients(OnlineUsers.Where(p => p.NotesId == NotesId && p.UserId != AppUserId).ToList().Select(x => x.ConnectionID).ToArray()).DisableOtherUserSignBtn(Newtonsoft.Json.JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                resFunction = false;
            }

            return resFunction;
        }
        public bool EnableSignNoteBtn(string NotesId, string AppUserId)
        {
            bool resFunction = true;
            try
            {
                if (OnlineUsers.FirstOrDefault(p => p.NotesId == NotesId) != null)
                {
                    HubResponse res = new HubResponse();
                    res.ComponentName = "SignNote";
                    res.status = true;
                    res.NotesId = NotesId;
                    Clients.Clients(OnlineUsers.Where(p => p.NotesId == NotesId && p.UserId != AppUserId).ToList().Select(x => x.ConnectionID).ToArray()).EnableOtherUserSignBtn(Newtonsoft.Json.JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception ex)
            {
                resFunction = false;
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
        public string NotesId { get; set; }
    }

    public partial class HubResponse
    {
        public string UserName { get; set; }
        public string ComponentName { get; set; }
        public string Message { get; set; }
        public bool status { get; set; }
        public string NotesId { get; set; }
    }
}