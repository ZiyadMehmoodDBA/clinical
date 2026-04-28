Clinical_ROSCharatristicsDetail = {
    params: [],
    bIsFirstLoad: true,
    IstoExit: false,
    Load: function (params) {
        Clinical_ROSCharatristicsDetail.params = params;
        Clinical_ROSCharatristicsDetail.IstoExit = false;
        //if (Clinical_ROSCharatristicsDetail.params.PanelID != "pnlAdminPEObservationDetail")
        //    Clinical_ROSCharatristicsDetail.params.PanelID = Clinical_ROSCharatristicsDetail.params.PanelID + " #pnlAdminPEObservationDetail";
        //else
        //    Clinical_ROSCharatristicsDetail.params.PanelID = "pnlAdminPEObservationDetail";

        //Clinical_ROSCharatristicsDetail.bindsystemAutoComplete();
        Clinical_ROSCharatristicsDetail.loadROSCharacteristics();
    },


    validatePhysicalExamObservationDetail: function () {
        var $self = $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #frmROSCharatristicsDetail");
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
            Clinical_ROSCharatristicsDetail.saveROSCharatristic();
        });

    },

    loadROSCharacteristics: function () {

        if (Clinical_ROSCharatristicsDetail.params.mode == "Add") {

            //serialize data
            $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + ' #frmPEObservations').data('serialize', $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + ' #frmPEObservations').serialize());
            Clinical_ROSCharatristicsDetail.validatePhysicalExamObservationDetail();
        }
        else if (Clinical_ROSCharatristicsDetail.params.mode == "Edit") {

            Clinical_ROSCharatristicsDetail.loadROSCharacteristics_DBcall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var ObservationLoad_JSON = JSON.parse(response.ObservationLoad_JSON);
                        var $self = $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #frmROSCharatristicsDetail");
                        utility.bindMyJSONByName(true, ObservationLoad_JSON[0], false, $self).done(function () {
                            $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + ' #frmROSCharatristicsDetail').data('serialize', $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + ' #frmROSCharatristicsDetail').serialize());
                        });

                        Clinical_ROSCharatristicsDetail.validatePhysicalExamObservationDetail();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    loadROSCharacteristics_DBcall: function () {
        var objData = new Object();
        objData["ROSCharacteristicsId"] = Clinical_ROSCharatristicsDetail.params.ROSCharacteristicsId;
        objData["commandType"] = "fill_reviewofsystem_charatristics"; /*fill_reviewofsystem_charatristics*/
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    //bindsystemAutoComplete: function () {

    //    Clinical_ROSCharatristicsDetail.lookupPESystem_DBCall().done(function (response) {
    //        if (response != "") {
    //            response = JSON.parse(response);
    //            if (response.status != false) {
    //                var System_JSON = JSON.parse(response.System_JSON);

    //                $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #txtSystem").kendoAutoComplete({
    //                    dataSource: System_JSON,
    //                    filter: "contains",
    //                    dataTextField: "Name",
    //                    placeholder: "Select System...",
    //                    select: function (e) {
    //                        var dataItem = this.dataItem(e.item.index());
    //                        $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #hfPESystemId").val(dataItem.PESystemId);
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

    saveROSCharatristic: function () {

        var strMessage = "";
        var self = $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #frmROSCharatristicsDetail"); /* zia */
        var myJSON = self.getMyJSONByName();
        var name_ = self.find("#txtName").val();
        if (name_ != "") {

            if (Clinical_ROSCharatristicsDetail.params.mode == "Add") {
                Clinical_ROSCharatristicsDetail.saveROSCharatristic_DBCall(myJSON).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_ROSCharatristicsDetail.resetToAdd();
                            Clinical_ROSCharatristics.searchROSCharatristics();
                            if (Clinical_ROSCharatristicsDetail.IstoExit)
                                Clinical_ROSCharatristicsDetail.UnLoad('saveExit');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else if (Clinical_ROSCharatristicsDetail.params.mode == "Edit") {

                Clinical_ROSCharatristicsDetail.updateROSCharacteristicsId_DBCall(myJSON, Clinical_ROSCharatristicsDetail.params.ROSCharacteristicsId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_ROSCharatristics.searchROSCharatristics();
                            if (Clinical_ROSCharatristicsDetail.IstoExit)
                                Clinical_ROSCharatristicsDetail.UnLoad('saveExit');
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
        var $self = $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #frmROSCharatristicsDetail");
        $self.find("#txtName").val("");
        $self.find('[type=checkbox]').each(function () {
            this.checked = true;
        });
        $self.bootstrapValidator('revalidateField', 'Name');

    },

    SetforExit: function () {
        var $self = $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #frmROSCharatristicsDetail");
        if ($self.find("#txtName").val() != "")
            Clinical_ROSCharatristicsDetail.IstoExit = true;
    },

    saveROSCharatristic_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        //var obj = new Object();
        //obj.Name = objData['Name'];
        //obj.IsActive = objData['IsActive'];


        //return MDVisionService.APIServiceComplex(obj, "ReviewOfSystemRMP", "ROSCharatristics");


        objData["commandType"] = "insert_reviewofsystem_charatristics";/*insert_physicalexam_observation zia*/
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    updateROSCharacteristicsId_DBCall: function (data, ROSCharacteristicsId) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["ROSCharacteristicsId"] = ROSCharacteristicsId;
        objData["commandType"] = "update_reviewofsystem_charatristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");

    },

    UnLoad: function (caller) {

        if (Clinical_ROSCharatristicsDetail.params["FromAdmin"] == "0") {

            if (Clinical_ROSCharatristicsDetail.params != null && Clinical_ROSCharatristicsDetail.params.ParentCtrl != null) {
                if (caller == 'saveExit') {
                    UnloadActionPan(Clinical_ROSCharatristicsDetail.params.ParentCtrl, 'Clinical_ROSCharatristicsDetail', null, Clinical_ROSCharatristicsDetail.params.PanelID);
                }
                else {
                    utility.UnLoadDialog("frmPEObservations", function () {
                        UnloadActionPan(Clinical_ROSCharatristicsDetail.params.ParentCtrl, 'Clinical_ROSCharatristicsDetail', null, Clinical_ROSCharatristicsDetail.params.PanelID);
                    }, function () {
                        UnloadActionPan(Clinical_ROSCharatristicsDetail.params.ParentCtrl, 'Clinical_ROSCharatristicsDetail', null, Clinical_ROSCharatristicsDetail.params.PanelID);
                    });

                }
            }
            else
                UnloadActionPan(null, 'Clinical_ROSCharatristicsDetail');

        }
        else {
            if (caller == 'saveExit') {
                UnloadActionPan();

            }
            else {
               // utility.UnLoadDialog("frmPEObservations", function () {
                    UnloadActionPan(Clinical_ROSCharatristicsDetail.params.ParentCtrl, 'Clinical_ROSCharatristicsDetail', null, Clinical_ROSCharatristicsDetail.params.PanelID);
              //  }, function () {
                //    UnloadActionPan(Clinical_ROSCharatristicsDetail.params.ParentCtrl, 'Clinical_ROSCharatristicsDetail', null, Clinical_ROSCharatristicsDetail.params.PanelID);
              //  });



            }
        }


    },

}