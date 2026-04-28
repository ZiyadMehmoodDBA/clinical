Clinical_Copy_Note_Component_Selection = {
    Load: function (params) {
        Clinical_Copy_Note_Component_Selection.params = params;
        if (Clinical_Copy_Note_Component_Selection.params != null && Clinical_Copy_Note_Component_Selection.params.PanelID != "Clinical_NoteCopyComponentSelection") {
            Clinical_Copy_Note_Component_Selection.params["PanelID"] = Clinical_Copy_Note_Component_Selection.params["PanelID"] + ' #Clinical_NoteCopyComponentSelection';
        }
        else {
            Clinical_Copy_Note_Component_Selection.params = [];
            Clinical_Copy_Note_Component_Selection.params["PanelID"] = "Clinical_NoteCopyComponentSelection";
        }

        if (Clinical_Copy_Note_Component_Selection.bIsFirstLoad) {
            Clinical_Copy_Note_Component_Selection.bIsFirstLoad = false;
            Clinical_Copy_Note_Component_Selection.loadNoteComponentsName();
        }
    },
    UnLoad: function (refreshNoteScreen) {

        if (refreshNoteScreen) {
            SelectTab('clinicalTabNotes', 'false');
            ClinicalMenuClick(event, function () {
                ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
            }, null, null, 'clinicalTabNotes', 'button');
        }
        if (Clinical_Copy_Note_Component_Selection.params != null && Clinical_Copy_Note_Component_Selection.params.ParentCtrl) {
            UnloadActionPan(Clinical_Copy_Note_Component_Selection.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
    AddAllToList: function (obj) {
        if ($(obj).prop('checked')) {
            $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " .copyComponents").prop('checked', true);
            $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " #btnCopy").attr("disabled", false);
        }
        else {
            $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " .copyComponents").prop('checked', false);
            $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " #btnCopy").attr("disabled", true);
        }
    },

    
    loadNoteComponentsName: function () {
        
        if (Clinical_Copy_Note_Component_Selection.params.NoteComponents.length > 0) {
            if (Clinical_Copy_Note_Component_Selection.params.NoteComponents && Clinical_Copy_Note_Component_Selection.params.NoteComponents.length > 1) {
                    $('#' + Clinical_Copy_Note_Component_Selection.params["PanelID"] + ' #chkBoxes').append('<div class="col-sm-6 col-md-6 col-lg-4 mb-md"> <div class="checkbox-custom"><input type="checkbox" componentName="SelectAll" id="chkSelectAll" onclick="Clinical_Copy_Note_Component_Selection.AddAllToList(this);" name="Component" checked><label class="control-label">Select All</label> </div></div>'
                    )
                }
            $.each(Clinical_Copy_Note_Component_Selection.params.NoteComponents, function (i, item) {
                    $('#' + Clinical_Copy_Note_Component_Selection.params["PanelID"] + ' #chkBoxes').append('<div class="col-sm-6 col-md-6 col-lg-4 mb-md"> <div class="checkbox-custom"><input type="checkbox" class="copyComponents" componentName="' + item.ComponentName + ' " onclick="Clinical_Copy_Note_Component_Selection.AddToList(this);" name="Component" checked><label class="control-label">' + item.customComponentName + '</label></div></div>');
                })
                }
            
        
    },
    AddToList: function (obj) {
       // if ($(obj).prop('checked')) {
            Clinical_Copy_Note_Component_Selection.checkOrUncheckSelectAll();
        //}
        //else {
        //    $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " #chkSelectAll").prop('checked', false);
        //}
    },
    checkOrUncheckSelectAll: function () {
        var anyUncheck = false
        var copyBtn = false;
        $.each($('#' + Clinical_Copy_Note_Component_Selection.params.PanelID).find(".copyComponents"), function (i, item) {
            if (!$(item).prop('checked')) {
                anyUncheck = true;
            }
            else {
                copyBtn = true;
            }
        })
        if (anyUncheck) {
            $('#' + Clinical_Copy_Note_Component_Selection.params.PanelID + " #chkSelectAll").prop('checked', false);
        }
        else {
            $('#' + Clinical_Copy_Note_Component_Selection.params.PanelID + " #chkSelectAll").prop('checked', true);
        }
        if (copyBtn) {
            $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " #btnCopy").attr("disabled", false);
        }
        else {
            $("#" + Clinical_Copy_Note_Component_Selection.params["PanelID"] + " #btnCopy").attr("disabled", true);

        }

    },
    CopyNote: function () {
        Clinical_Copy_Note_Component_Selection.UnLoad();
        var NoteComponentList = []
        $.each($('#' + Clinical_Copy_Note_Component_Selection.params.PanelID).find(".copyComponents"), function (i, item) {
            if ($(item).prop('checked')) {
                NoteComponentList.push({ ComponentName: $(item).attr("componentName"), isCopy: 1})
            }
        })
        Clinical_Notes.params.NoteComponentList = NoteComponentList
        $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').submit();
    }
}