using MDVision.Datasets;
using MDVision.Business.BCommon;
using MDVision.IEHR.EMR.Model.PhysicalExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MDVision.Common.Utilities;
using MDVision.Common.Shared;
using MDVision.Business.BLL;
using System.IO;
using MDVision.IEHR.Common;
using System.Reflection;
using System.Xml.Serialization;
using MDVision.Model.Clinical.Macros;
namespace MDVision.IEHR.EMR.Helpers.Clinical.Templates
{
    public class MacroHelper
    {
        private BLLClinical BLLClinicalObj = null;
        private BLLMacro BLLMacroObj = null;
        public MacroHelper()
        {
            BLLClinicalObj = new BLLClinical();
            BLLMacroObj = new BLLMacro();
        }
        private static MacroHelper _instance = null;
        public static MacroHelper Instance()
        {
            if (_instance == null)
                _instance = new MacroHelper();
            return _instance;
        }

        public string SearchDetailsForNotes(MacroModel macro)
        {
            try
            {
                List<MacroModel> obj = BLLMacroObj.SearchDetailsForNotes(macro.MacroId, macro.MacroName, macro.Keyword, macro.UserId, macro.NoteComponentId, macro.NoteComponentsNames);
                if (obj != null)
                {
                    var response = new
                    {
                        status = true,
                        MacroDetails = obj
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Couldn't load data."
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string insertMacro(MacroModel macro)
        {
            try
            {
                MacroModel obj = BLLMacroObj.SaveMacroDetails(macro.MacroName, macro.Keyword, macro.Description, Convert.ToBoolean(macro.IsIndependent), macro.UsersIds, macro.NoteComponentIds);
                if (obj != null && (obj.ErrorMessage == "" || obj.ErrorMessage == null))
                {
                    var response = new
                    {
                        status = true,
                        message = "Saved Successfully!",
                        MacroId = obj.MacroId
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.ErrorMessage
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string showMacro(MacroModel model)
        {
            try
            {
                List<MacroModel> obj = BLLMacroObj.ShowMacroDetails(model.MacroId, model.MacroName, model.Keyword, model.NoteComponentIds, model.UsersIds, model.DateFrom, model.DateTo);
                if (obj != null)
                {
                    var response = new
                    {
                        status = true,
                        macros = obj
                        // message = "Saved Successfully!"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj[0].ErrorMessage
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }

        public string deleteMacro(MacroModel Macro)
        {
            try
            {

                bool check = BLLMacroObj.DeleteMacroDetails(Macro);
                if (check == true)
                {
                    var response = new
                    {
                        status = true,

                        message = "Deleted Successfully!"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = "Unable to Delete"
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
        public string updateMacro(MacroModel macro)
        {
            try
            {
                MacroModel obj = BLLMacroObj.UpdateMacroDetails(macro.MacroId, macro.MacroName, macro.Keyword, macro.Description, Convert.ToBoolean(macro.IsIndependent), macro.UsersIds, macro.NoteComponentIds);
                if (obj != null && obj.ErrorMessage == "")
                {
                    var response = new
                    {
                        status = true,
                        message = "Saved Successfully!"
                    };
                    return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
                }
                else
                {
                    var response = new
                    {
                        status = false,
                        Message = obj.ErrorMessage
                    };
                    return Newtonsoft.Json.JsonConvert.SerializeObject(response);
                }
            }
            catch (Exception ex)
            {
                var response = new
                {
                    status = false,
                    Message = MDVCustomException.HumanReadableMessage(ex.Message),
                };
                return (Newtonsoft.Json.JsonConvert.SerializeObject(response));
            }
        }
    }

}
