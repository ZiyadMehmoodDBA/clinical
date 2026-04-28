using MDVision.Common.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace MDVision.IEHR.Common.ProviderNoteAccess
{
    public class ProviderNoteAccessCache
    {
        public static string KeyPreFix = "NoteCompumentAccess_";
        public bool insertIntoCache(RevokeAccessModel model)
        {
            bool FnResponse = false;
            try
            {
                if (model.UserId > 0)
                {
                    if (HttpContext.Current.Cache.Get(KeyPreFix + model.UserId.ToString()) != null)
                    {
                        RevokeAccessModel model_ = (RevokeAccessModel)HttpContext.Current.Cache.Get(KeyPreFix + model.UserId.ToString());

                        if (model_.NoteComponents.FirstOrDefault(p => p.ComponentName == model.CurrentComponent) != null)
                        {
                            foreach (var item in model_.NoteComponents.Where(p => p.ComponentName == model.CurrentComponent))
                            {
                                item.AccessTime = DateTime.Now;
                                item.IsAvaliable = false;
                            }
                        }
                        else
                        {
                            NoteComponentAccessModel model_component = new NoteComponentAccessModel();
                            model_component.AccessTime = DateTime.Now;
                            model_component.ComponentName = model.CurrentComponent;
                            model_component.IsAvaliable = false;
                            model_.NoteComponents.Add(model_component);
                        }

                        model.NoteComponents = model_.NoteComponents;
                        HttpContext.Current.Cache.Remove(KeyPreFix + model.UserId.ToString());
                    }
                    else
                    {
                        NoteComponentAccessModel model_component = new NoteComponentAccessModel();
                        model_component.AccessTime = DateTime.Now;
                        model_component.ComponentName = model.CurrentComponent;
                        model_component.IsAvaliable = false;
                        model.NoteComponents.Add(model_component);
                    }

                    HttpContext.Current.Cache.Insert(KeyPreFix + model.UserId.ToString(), model);
                    FnResponse = true;
                }
            }
            catch (Exception ex)
            {
                FnResponse = false;
            }

            return FnResponse;

        }
        public bool removeUserNoteAccessFromCache(long UserId)
        {
            bool FnResponse = false;
            try
            {
                if (HttpContext.Current.Cache.Get(KeyPreFix + UserId.ToString()) != null)
                {
                    HttpContext.Current.Cache.Remove(KeyPreFix + UserId.ToString());
                    FnResponse = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return FnResponse;
        }
        public bool removeComponentFromCache(long UserId, string ComponentName)
        {
            bool FnResponse = false;
            try
            {
                if (HttpContext.Current.Cache.Get(KeyPreFix + UserId.ToString()) != null)
                {
                    RevokeAccessModel model_ = (RevokeAccessModel)HttpContext.Current.Cache.Get(KeyPreFix + UserId.ToString());
                    foreach (var item in model_.NoteComponents.Where(p => p.ComponentName == ComponentName))
                    { item.AccessTime = DateTime.Now; item.IsAvaliable = true; }


                    HttpContext.Current.Cache.Remove(KeyPreFix + UserId.ToString());
                    HttpContext.Current.Cache.Insert(KeyPreFix + UserId.ToString(), model_);
                    FnResponse = true;
                }
            }
            catch (Exception ex)
            {
                FnResponse = false;
            }

            return FnResponse;
        }
        public RevokeAccessResponseModel IsNoteComponentAvaliable(long UserId, long NoteId, string ComponentName, string NoteAccessTime)
        {
            DateTime now = DateTime.Now;
            RevokeAccessResponseModel response = new RevokeAccessResponseModel();
            response.IsComponentAvaliable = true;
            response.IsComponentUpdated = false;
            try
            {
                IDictionaryEnumerator Cache_ = HttpContext.Current.Cache.GetEnumerator();
                while (Cache_.MoveNext())
                {
                    if (Cache_.Value != null && Cache_.Key.ToString().Contains(KeyPreFix))
                    {

                        RevokeAccessModel model_ = Cache_.Value as RevokeAccessModel;

                        // check is this component is updated by some one else.
                        if (response.IsComponentUpdated == false
                            && !string.IsNullOrEmpty(NoteAccessTime)
                            && model_.NoteId == NoteId
                            && model_.NoteComponents.FirstOrDefault(p => p.ComponentName == ComponentName && p.IsAvaliable == true) != null
                            && model_.UserId != UserId)
                        {
                            DateTime note_access_time = MDVUtility.StringToDate(NoteAccessTime);
                            var obj = model_.NoteComponents.FirstOrDefault(p => p.ComponentName == ComponentName && p.IsAvaliable == true);
                            if (obj.AccessTime >= note_access_time && obj.AccessTime <= now)
                            {
                                response.IsComponentUpdated = true;
                            }


                        }

                        // check is this not is avaliable.
                        if (model_.NoteId == NoteId
                            && model_.NoteComponents.FirstOrDefault(p => p.ComponentName == ComponentName && p.IsAvaliable == false) != null
                            && model_.UserId != UserId)
                        {
                            response.IsComponentAvaliable = false;
                            response.PriorUserName = model_.UserName.ToLower();
                            response.PriorUserId = model_.UserId;
                            response.ComponentName = ComponentName;
                            break;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }
    }

    public class RevokeAccessModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long NoteId { get; set; }
        public string CurrentComponent { get; set; }
        public List<NoteComponentAccessModel> NoteComponents { get; set; }

        public RevokeAccessModel()
        {
            this.NoteComponents = new List<NoteComponentAccessModel>();
        }
    }
    public class NoteComponentAccessModel
    {
        public string ComponentName { get; set; }
        public bool IsAvaliable { get; set; }
        public DateTime AccessTime { get; set; }
    }


    public class RevokeAccessResponseModel
    {
        public string PriorUserName { get; set; }
        public long PriorUserId { get; set; }
        public bool IsComponentAvaliable { get; set; }
        public string ComponentName { get; set; }
        public bool IsComponentUpdated { get; set; }
    }
}