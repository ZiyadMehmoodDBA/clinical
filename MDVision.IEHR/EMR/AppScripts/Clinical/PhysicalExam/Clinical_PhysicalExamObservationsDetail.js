Clinical_PhysicalExamObservationsDetail = {
    params: [],
    bIsFirstLoad: true,
    IstoExit: false,
    Load: function (params) {
        Clinical_PhysicalExamObservationsDetail.params = params;
        Clinical_PhysicalExamObservationsDetail.IstoExit = false;
        //if (Clinical_PhysicalExamObservationsDetail.params.PanelID != "pnlAdminPEObservationDetail")
        //    Clinical_PhysicalExamObservationsDetail.params.PanelID = Clinical_PhysicalExamObservationsDetail.params.PanelID + " #pnlAdminPEObservationDetail";
        //else
        //    Clinical_PhysicalExamObservationsDetail.params.PanelID = "pnlAdminPEObservationDetail";

        //Clinical_PhysicalExamObservationsDetail.bindsystemAutoComplete();
        Clinical_PhysicalExamObservationsDetail.loadPEObservation();
    },


    validatePhysicalExamObservationDetail: function () {
        var $self = $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #frmPEObservations");
        $self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                Name: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
            }

        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_PhysicalExamObservationsDetail.savePEObservation();
        });

    },

    loadPEObservation: function () {

        if (Clinical_PhysicalExamObservationsDetail.params.mode == "Add") {

            //serialize data
            $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + ' #frmPEObservations').data('serialize', $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + ' #frmPEObservations').serialize());
            Clinical_PhysicalExamObservationsDetail.validatePhysicalExamObservationDetail();
        }
        else if (Clinical_PhysicalExamObservationsDetail.params.mode == "Edit") {

            Clinical_PhysicalExamObservationsDetail.loadPEObservation_DBcall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var ObservationLoad_JSON = JSON.parse(utility.decodeHtml(response.ObservationLoad_JSON));
                        var $self = $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #frmPEObservations");
                        utility.bindMyJSONByName(true, ObservationLoad_JSON[0], false, $self).done(function () {
                            $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + ' #frmPEObservations').data('serialize', $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + ' #frmPEObservations').serialize());
                        });

                        Clinical_PhysicalExamObservationsDetail.validatePhysicalExamObservationDetail();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    loadPEObservation_DBcall: function () {
        var objData = new Object();
        objData["PEObservationId"] = Clinical_PhysicalExamObservationsDetail.params.PEObservationId;
        objData["commandType"] = "fill_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    //bindsystemAutoComplete: function () {

    //    Clinical_PhysicalExamObservationsDetail.lookupPESystem_DBCall().done(function (response) {
    //        if (response != "") {
    //            response = JSON.parse(response);
    //            if (response.status != false) {
    //                var System_JSON = JSON.parse(response.System_JSON);

    //                $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #txtSystem").kendoAutoComplete({
    //                    dataSource: System_JSON,
    //                    filter: "contains",
    //                    dataTextField: "Name",
    //                    placeholder: "Select System...",
    //                    select: function (e) {
    //                        var dataItem = this.dataItem(e.item.index());
    //                        $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #hfPESystemId").val(dataItem.PESystemId);
    //                    },
    //                });
    //            }
    //            else
    //                utility.DisplayMessages(response.Message, 3);
    //        }

    //    });
    //},

    //lookupPESystem_DBCall: function () {

    //    var objData = new Object();
    //    objData["IsActive"] = 1;
    //    objData["commandType"] = "lookup_physicalexam_system";
    //    var data = JSON.stringify(objData);
    //    return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    //},

    savePEObservation: function () {

        var strMessage = "";
        var self = $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #frmPEObservations");
        var myJSON = self.getMyJSONByName();
        var name_ = self.find("#txtName").val();
        if (name_ != "") {

            if (Clinical_PhysicalExamObservationsDetail.params.mode == "Add") {
                Clinical_PhysicalExamObservationsDetail.savePEObservation_DBCall(myJSON).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_PhysicalExamObservationsDetail.resetToAdd();
                            // PRD-5 ,Dev By:MAhmad 
                            Clinical_PhysicalExamObservations.resetAllFields();
                            // PRD-5 ,Dev By:MAhmad 
                            Clinical_PhysicalExamObservations.searchPEObservations();
                            if (Clinical_PhysicalExamObservationsDetail.IstoExit)
                                Clinical_PhysicalExamObservationsDetail.UnLoad('saveExit');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else if (Clinical_PhysicalExamObservationsDetail.params.mode == "Edit") {

                Clinical_PhysicalExamObservationsDetail.updatePEObservation_DBCall(myJSON, Clinical_PhysicalExamObservationsDetail.params.PEObservationId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            // PRD-5 ,Dev By:MAhmad 
                            Clinical_PhysicalExamObservations.resetAllFields();
                            // PRD-5 ,Dev By:MAhmad 
                            Clinical_PhysicalExamObservations.searchPEObservations();
                            if (Clinical_PhysicalExamObservationsDetail.IstoExit)
                                Clinical_PhysicalExamObservationsDetail.UnLoad('saveExit');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }

        }
        else {
            utility.DisplayMessages('please enter observation.', 3);
        }
        

    },

    resetToAdd: function () {
        var $self = $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #frmPEObservations");
        $self.find("#txtName").val("");
        $self.find('[type=checkbox]').each(function () {
            this.checked = true;
        });
        $self.bootstrapValidator('revalidateField', 'Name');

    },

    SetforExit: function () {
        var $self = $('#' + Clinical_PhysicalExamObservationsDetail.params.PanelID + " #frmPEObservations");
        if ($self.find("#txtName").val() != "")
            Clinical_PhysicalExamObservationsDetail.IstoExit = true;
    },

    savePEObservation_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["commandType"] = "insert_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    updatePEObservation_DBCall: function (data, PEObservationId) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["PEObservationId"] = PEObservationId;
        objData["commandType"] = "update_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");

    },

    UnLoad: function (caller) {

        if (Clinical_PhysicalExamObservationsDetail.params["FromAdmin"] == "0") {

            if (Clinical_PhysicalExamObservationsDetail.params != null && Clinical_PhysicalExamObservationsDetail.params.ParentCtrl != null) {
                if (caller == 'saveExit') {
                    UnloadActionPan(Clinical_PhysicalExamObservationsDetail.params.ParentCtrl, 'Clinical_PhysicalExamObservationsDetail', null, Clinical_PhysicalExamObservationsDetail.params.PanelID);
                }
                else {
                    utility.UnLoadDialog("frmPEObservations", function () {
                        UnloadActionPan(Clinical_PhysicalExamObservationsDetail.params.ParentCtrl, 'Clinical_PhysicalExamObservationsDetail', null, Clinical_PhysicalExamObservationsDetail.params.PanelID);
                    }, function () {
                        UnloadActionPan(Clinical_PhysicalExamObservationsDetail.params.ParentCtrl, 'Clinical_PhysicalExamObservationsDetail', null, Clinical_PhysicalExamObservationsDetail.params.PanelID);
                    });

                }
            }
            else
                UnloadActionPan(null, 'Clinical_PhysicalExamObservationsDetail');

        }
        else {
            if (caller == 'saveExit') {
                UnloadActionPan();

            }
            else
            {
                utility.UnLoadDialog("frmPEObservations", function () {
                    UnloadActionPan(Clinical_PhysicalExamObservationsDetail.params.ParentCtrl, 'Clinical_PhysicalExamObservationsDetail', null, Clinical_PhysicalExamObservationsDetail.params.PanelID);
                }, function () {
                    UnloadActionPan(Clinical_PhysicalExamObservationsDetail.params.ParentCtrl, 'Clinical_PhysicalExamObservationsDetail', null, Clinical_PhysicalExamObservationsDetail.params.PanelID);
                });

               

            }
        }


    },
    
}