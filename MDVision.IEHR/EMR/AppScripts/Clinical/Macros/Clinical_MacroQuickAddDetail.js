Clinical_MacroQuickAddDetail = {
    params: [],
    Load: function (params) {
        Clinical_MacroQuickAddDetail.params = params;
        Clinical_MacroQuickAddDetail.validateMacro();
        if (Clinical_MacroQuickAddDetail.params["Mode"] == "Edit") {
            var item = params["MacroId"];
            Clinical_MacroQuickAddDetail.editMacroDetail(item);
            $('#pnlMacroQuickAddDetail #headingMode').html("Edit");
        }
        
           
       

        $(function () {
            $('#pnlMacroQuickAddDetail #txtKeyword').keydown(function (e) {
                if (e.keyCode == 32 || e.keyCode==188 || e.keyCode==190) // 32 is the ASCII value for a space
                { e.preventDefault(); }
            });
            

        });
        
    },
    validateMacro: function () {
        $('#pnlMacroQuickAddDetail #frmMacroQuickAddDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   MacroName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Keyword: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Description: {
                       group: '.col-sm-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();

            
           
                Clinical_MacroQuickAddDetail.Save();
            
            
        });
    },
    Save: function () {
        var self = $('#pnlMacroQuickAddDetail');
        var objData = {};
        objData["MacroName"] = $('#pnlMacroQuickAddDetail #txtMacroName').val();
        objData["Keyword"] = $('#pnlMacroQuickAddDetail #txtKeyword').val();
        objData["Description"] = $('#pnlMacroQuickAddDetail #txtDescription').val();
    //    objData["Description"] = objData["Description"].replace(/\r?\n/gi, "<br>");
        objData["IsIndependent"] = "true";
        objData["UsersIds"] = globalAppdata.AppUserId;
        if (TinymceEditor.params.ComponentName && TinymceEditor.params.ComponentName == "Complaints") {

            var componentid = $(TinymceEditor.params["Control"]).attr('notecomponentid');
            if (componentid)
            { objData["NoteComponentIds"] = componentid }
            else {
                objData["NoteComponentIds"] = $(TinymceEditor.params["Control"]).closest("li").attr("notecomponentid");
            }
        }
        else { objData["NoteComponentIds"] = $(TinymceEditor.params["Control"]).attr('notecomponentid'); }

        if (Clinical_MacroQuickAddDetail.params["Mode"] == "Edit") {
            objData["commandType"] = "Edit_Macro";
            objData["MacroID"] = Clinical_MacroQuickAddDetail.params["MacroId"];
            Clinical_MacroDetail.Update_DBCall(objData).done(function (response) {
                if (response) {
                    utility.DisplayMessages("Successfully updated.", 1);
                    TinymceEditor.LoadMacros();
                    Clinical_MacroQuickAddDetail.UnLoad();
                }
                else {

                }
            });
        }

        else {
            objData["commandType"] = "Save_Macro";
            Clinical_MacroDetail.Save_DBCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    utility.DisplayMessages(response.message, 1);
                    //if (objData["Description"]) {
                    //    objData["Description"] = objData["Description"].replace(/[']/g, "#quote#");
                    //    objData["Description"] = objData["Description"].replace(/["]/g, "#doublequote#");
                    //}
                    //$("#pnlTinymceEditor #ulMacroDetails").
                    //    prepend(TinymceEditor.addMacros(response.MacroId, objData["MacroName"], objData["Description"], true));
                    TinymceEditor.LoadMacros();
                    Clinical_MacroQuickAddDetail.UnLoad();
                }
                else {
                    utility.DisplayMessages(response.message, 2);
                }
            });
        }


        
    },
    UnLoad: function (ParentCtrl) {
        if (Clinical_MacroQuickAddDetail.params["ParentCtrl"] != null || Clinical_MacroQuickAddDetail.params["ParentCtrl"] != "") {
            UnloadActionPan(Clinical_MacroQuickAddDetail.params["ParentCtrl"], "Clinical_MacroQuickAddDetail");
        }
        else {
            UnloadActionPan();
        }
    },

    editMacroDetail: function (item) {
        var data;

        Clinical_MacroQuickAddDetail.LoadMacro(item).done(function (response) {
            response = JSON.parse(response);
            data = response.macros;
            $('#pnlMacroQuickAddDetail #txtMacroName').val(data[0].MacroName);
            $('#pnlMacroQuickAddDetail #txtKeyword').val(data[0].Keyword);
            $('#pnlMacroQuickAddDetail #txtDescription').val(data[0].Description);
            //$('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect('clearSelection', false);
            //$('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect('updateButtonText');
            //$("#pnlClinicalMacroDetail #ddlMacroDetailUsers").val(data[0].UsersIds.split(','));
            //$('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect("refresh");

            //$('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('clearSelection', false);
            //$('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('updateButtonText');
            //// load compnetent if Note Flow
            //$('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('clearSelection', false);
            //$('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('updateButtonText');
            //$("#pnlClinicalMacroDetail #ddlMacroDetailComponentNames").val(data[0].NoteComponentIds.split(','));
            //$('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect("refresh");

            //$('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect("refresh");
        });


    },
    LoadMacro: function (item) {

        var obj = new Object();
        obj["MacroId"] = item;
        obj["commandType"] = "Get_Macro";
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "Macro", "Macro");
    },
     
}