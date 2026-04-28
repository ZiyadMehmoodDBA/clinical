using MDVision.Business.BCommon;
using MDVision.Common.Logging;
using MDVision.Common.Utilities;
using MDVision.DataAccess.DAL.Clinical;
using MDVision.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MDVision.Model.Clinical.Macros;
namespace MDVision.Business.BLL
{
    public class BLLMacro
    {
        #region " Constructors "
        public BLLMacro()
        {
            InitializeComponent();
        }

        private IContainer components;
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
        public MacroModel SaveMacroDetails(string MacroName, string Keyword, string Description, bool IsIndependent, string UserIds, string NoteComponentsIds)
        {
            try
            {
                return new DALMacro().SaveDetails(MacroName, Keyword, Description, IsIndependent, UserIds, NoteComponentsIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMacro::SaveMacroDetails", ex);
                return null;
            }
        }
        public List<MacroModel> SearchDetailsForNotes(long MacroId, string MacroName, string Keyword, long UserId, long NoteComponentId,string ComponentName)
        {
            try
            {
                return new DALMacro().SearchDetailsForNotes(MacroId, MacroName, Keyword, UserId, NoteComponentId, ComponentName);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMacro::SearchDetailsForNotes", ex);
                return null;
            }
        }
        public List<MacroModel> ShowMacroDetails(long Macroid = 0, string MacroName = null, string Keyword = null, string NoteComponentIds = null, string UsersIds = null, string DateFrom = null, string DateTo = null)
        {
            try
            {
                if (Macroid != 0)
                    return new DALMacro().ShowDetails(Macroid);
                else
                    return new DALMacro().ShowDetails(Macroid, MacroName, Keyword, NoteComponentIds, UsersIds, DateFrom, DateTo);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMacro::ShowMacroDetails", ex);
                return null;
            }
        }

        public bool DeleteMacroDetails(MacroModel Macro)
        {
            int[] ids = Macro.IdsToDelete.Split(',').Select(int.Parse).ToArray();
            try
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    if ((new DALMacro().DeleteDetails(ids[i])) == false)
                        return false;
                }
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMacro::DeleteMacroDetails", ex);
                return false;
            }
            return true;
        }
        public MacroModel UpdateMacroDetails(long MacroId, string MacroName, string Keyword, string Description, bool IsIndependent, string UserIds, string NoteComponentsIds)
        {
            try
            {
                return new DALMacro().UpdateDetails(MacroId, MacroName, Keyword, Description, IsIndependent, UserIds, NoteComponentsIds);
            }
            catch (Exception ex)
            {
                MDVLogger.BLLErrorLog("BLLMacro::UpdateMacroDetails", ex);
                return null;
            }
        }

    }

    
}
