Clinical_PhysicalExamSystemsDetail = {
    params: [],
    ObservationsList: [],
    bIsFirstLoad: true,
    bIsTextChanged: false,

    Load: function (params) {

        Clinical_PhysicalExamSystemsDetail.params = params;
        Clinical_PhysicalExamSystemsDetail.loadPESystems();
        Clinical_PhysicalExamSystemsDetail.bindobservationAutoComplete();
        Clinical_PhysicalExamSystemsDetail.validatePhysicalExamSystemsDetail();

    },

    validatePhysicalExamSystemsDetail: function () {
        var $self = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #frmPESystem");
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
                    group: '.col-sm-9',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
            }

        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_PhysicalExamSystemsDetail.savePESystem();
        });

    },

    loadPESystems: function () {

        if (Clinical_PhysicalExamSystemsDetail.params.mode == "Add") {

            $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #frmPESystem #btnSaveAs").attr('disabled', true);
        }
        else if (Clinical_PhysicalExamSystemsDetail.params.mode == "Edit") {

            Clinical_PhysicalExamSystemsDetail.loadPESystems_DBcall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var SystemsDataJSON = JSON.parse(utility.decodeHtml(response.SystemLoad_JSON));
                        var ObservationLoad_JSON = JSON.parse(utility.decodeHtml(response.ObservationLoad_JSON));
                        var $self = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #frmPESystem");
                        Clinical_PhysicalExamSystemsDetail.bindObservations(ObservationLoad_JSON, true);
                        utility.bindMyJSONByName(true, SystemsDataJSON[0], false, $self).done(function () {
                            //$('#frmPESystem').data('serialize', $('#frmPESystem').serialize());
                        });
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }

            });

        }
    },

    loadPESystems_DBcall: function () {
        var objData = new Object();
        objData["PESystemId"] = Clinical_PhysicalExamSystemsDetail.params.PESystemId;
        objData["commandType"] = "fill_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },

    bindObservationToHTML: function (item, PESystemObservationId, IsFromLoad) {


        //1- set observation into grid
        $ul = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #ulObservation");

        if ($ul.find("#Observation_" + item.PEObservationId).length <= 0) {
            delete_ = '<a href="#" class="rightbtn"><span class="removeIconListHover" onclick="Clinical_PhysicalExamSystemsDetail.removeObservation(this,\'' + item.PEObservationId + '\', event);">'
                                        + '<i class="fa fa-times"></i></span> </a>'
            li_ = '<li PEObservationId="' + item.PEObservationId + '" id="Observation_' + item.PEObservationId + '" SystemObservationId="' + PESystemObservationId + '" data-lab="' + item.Name + '"><span id="spnLabName">' + item.Name + '</span>' + delete_ + '</li>';

            $ul.append(li_);

            //2- set its hidden value 
            if (IsFromLoad != true) {
                var array = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val().split(',');
                var index_ = array.indexOf(item.PEObservationId);
                if (index_ < 0) {
                    array.push(item.PEObservationId);
                    $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val(array.join());
                }
            }
        }
    },

    bindObservations: function (dataItems, IsFromLoad) {

        $.each(dataItems, function (i, item) {

            var PESystemObservationId = "";
            if (item.PESystemObservationId)
                PESystemObservationId = item.PESystemObservationId;

            //add observation to html
            Clinical_PhysicalExamSystemsDetail.bindObservationToHTML(item, PESystemObservationId, IsFromLoad);
        });
    },

    removeObservationFromHTML: function ($obj, PEObservationId) {

        $($obj).parent().parent().remove();
        var array = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val().split(',');
        var index_ = array.indexOf(PEObservationId);
        if (index_ >= 0) {
            array.splice(index_, 1);
            $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val(array.join());
        }

    },

    removeObservation: function ($obj, PEObservationId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var SystemObservationId = $($obj).parent().parent().attr('SystemObservationId');
        if (SystemObservationId) {
            utility.myConfirm("1", function () {
                Clinical_PhysicalExamSystemsDetail.deletePESystem_Observation_DBCall(SystemObservationId).done(function (response) {

                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            Clinical_PhysicalExamSystemsDetail.removeObservationFromHTML($obj, PEObservationId);
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    }

                });

            }, function () { });
        }
        else {
            Clinical_PhysicalExamSystemsDetail.removeObservationFromHTML($obj, PEObservationId);
        }


    },

    bindAddedObservations: function (dataItems) {

        if (dataItems && dataItems.length > 0) {
            $.each(dataItems, function (i, item) {
                $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #ulObservation")
                    .find("#Observation_" + item.PEObservationId).attr("SystemObservationId", item.PESystemObservationId);
            });
        }
        $('#' +Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val('');
        //Clinical_PhysicalExamSystemsDetail.bindObservations(dataItems, true);

    },

    savePESystem: function () {


        var strMessage = "";
        var self = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #frmPESystem");
        var myJSON = self.getMyJSONByName();
        var name_ = self.find("#txtName").val();
        if (name_ != "") {

            if (Clinical_PhysicalExamSystemsDetail.params.mode == "Add") {
                Clinical_PhysicalExamSystemsDetail.savePESystem_DBCall(myJSON).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            // Enable Save as 
                            $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #btnSaveAs").removeAttr('disabled');
                            var PEObservation_JSON = JSON.parse(response.PEObservation_JSON);
                            Clinical_PhysicalExamSystemsDetail.bindAddedObservations(PEObservation_JSON);

                            Clinical_PhysicalExamSystemsDetail.params.mode = "Edit";
                            Clinical_PhysicalExamSystemsDetail.params.PESystemId = response.PESystemId;

                            $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #ulObservation li").remove();
                            $('#' +Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtObservations").val('');

                            Clinical_PhysicalExamSystemsDetail.loadPESystems();
                            Clinical_PhysicalExamSystems.searchPESystems();

                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else if (Clinical_PhysicalExamSystemsDetail.params.mode == "Edit") {

                Clinical_PhysicalExamSystemsDetail.updatePESystem_DBCall(myJSON, Clinical_PhysicalExamSystemsDetail.params.PESystemId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            var PEObservation_JSON = JSON.parse(response.PEObservation_JSON);
                            Clinical_PhysicalExamSystemsDetail.bindAddedObservations(PEObservation_JSON);

                            utility.DisplayMessages(response.Message, 1);
                            Clinical_PhysicalExamSystems.searchPESystems();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            Clinical_PhysicalExamSystemsDetail.bIsTextChanged = false;
        }
        else {
            utility.DisplayMessages('please enter system.', 3);
        }
        

    },

    bindobservationAutoComplete: function () {

        Clinical_PhysicalExamSystemsDetail.lookupPEObservation_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    response.Observation_JSON = response.Observation_JSON.replace(/&quot;/g, '');
                    var Observation_JSON = JSON.parse(utility.decodeHtml(response.Observation_JSON));

                    $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtObservations").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Observation...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            Clinical_PhysicalExamSystemsDetail.bindObservationToHTML(dataItem, "", false);
                            setTimeout(function () {
                                $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtObservations").val('');
                            }, 100);
                        },
                    });
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    fillPEObservation: function (id) {

        var dataItem = $.grep(Clinical_PhysicalExamSystemsDetail.ObservationsList, function (item_) { return (item_.PEObservationId == id) });
        if (dataItem.length > 0) {
            if (dataItem[0].IsActive == "True") {
                $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtObservations").val('');
                //$('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtObservations").val(dataItem[0].Name);
                Clinical_PhysicalExamSystemsDetail.bindObservationToHTML(dataItem[0], "", false);
                Clinical_PhysicalExamObservations.UnLoadTab();
            }
            else
                utility.DisplayMessages('Inactive Observation.', 3);
        }
        else
            Clinical_PhysicalExamObservations.UnLoadTab();

    },

    TextChanged: function () {

        Clinical_PhysicalExamSystemsDetail.bIsTextChanged = true;

        //if ($(element).is(':checked')) {
        //    $("" + panelId + " #dvresion").fadeOut();
        //}
        //else
        //    $("" + panelId + " #dvresion").fadeIn();

    },
    saveAs: function (obj) {
        $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtName").val('');
        $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #txtObservations").val('');
        $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + ' #frmPESystem').bootstrapValidator('revalidateField', 'Name');
        Clinical_PhysicalExamSystemsDetail.params["mode"] = "Add";
        $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #ulObservation li").each(function (i, item) {
            var Id = $(item).attr("PEObservationId");
            $(item).attr("systemobservationid", ''); // clear this Id so user can't remove association from pervious System
            var isnormal = $(item).attr("data-lab").toLowerCase() == "is normal" ? true : false;
            if (Id && !isnormal) {
                var array = $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val().split(',');
                var index_ = array.indexOf(Id);
                if (index_ < 0) {
                    array.push(Id);
                    $('#' + Clinical_PhysicalExamSystemsDetail.params.PanelID + " #hfPEObservationIds").val(array.join());
                }
            }
        });
        Clinical_PhysicalExamSystemsDetail.bIsTextChanged = false;
    },

    OpenObservation: function () {

        Clinical_PhysicalExamSystemsDetail.ObservationsList = [];
        var params = [];
        params["IsOptional"] = false;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_PhysicalExamSystemsDetail";
        LoadActionPan('Clinical_PhysicalExamObservations', params);

    },

    lookupPEObservation_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    savePESystem_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["commandType"] = "insert_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },

    updatePESystem_DBCall: function (data, PESystemId) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["PESystemId"] = PESystemId;
        objData["commandType"] = "update_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");

    },

    deletePESystem_Observation_DBCall: function (SystemObservationId) {

        var objData = new Object();
        objData["PESystemObservationId"] = SystemObservationId;
        objData["commandType"] = "delete_physicalexam_system_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    UnLoad: function () {
        if (Clinical_PhysicalExamSystemsDetail.params["FromAdmin"] == "0") {
            if (Clinical_PhysicalExamSystemsDetail.params != null && Clinical_PhysicalExamSystemsDetail.params.ParentCtrl != null) {
                if (Clinical_PhysicalExamSystemsDetail.bIsTextChanged) {
                    utility.UnLoadDialog("frmPESystem", function () {
                        UnloadActionPan(Clinical_PhysicalExamSystemsDetail.params.ParentCtrl, 'Clinical_PhysicalExamSystemsDetail', null, Clinical_PhysicalExamSystemsDetail.params.PanelID);
                    }, function () {
                        UnloadActionPan(Clinical_PhysicalExamSystemsDetail.params.ParentCtrl, 'Clinical_PhysicalExamSystemsDetail', null, Clinical_PhysicalExamSystemsDetail.params.PanelID);
                    });
                }
                else {
                    UnloadActionPan(null, 'Clinical_PhysicalExamSystemsDetail');

                }
                
            }
            else
                UnloadActionPan(null, 'Clinical_PhysicalExamSystemsDetail');
        }
        else {

            if (Clinical_PhysicalExamSystemsDetail.bIsTextChanged) {
                utility.UnLoadDialog("frmPESystem", function () {
                    UnloadActionPan(Clinical_PhysicalExamSystemsDetail.params.ParentCtrl, 'Clinical_PhysicalExamSystemsDetail', null, Clinical_PhysicalExamSystemsDetail.params.PanelID);
                }, function () {
                    UnloadActionPan(Clinical_PhysicalExamSystemsDetail.params.ParentCtrl, 'Clinical_PhysicalExamSystemsDetail', null, Clinical_PhysicalExamSystemsDetail.params.PanelID);
                });
            }
            else {
                UnloadActionPan(null, 'Clinical_PhysicalExamSystemsDetail');

            }
        }
        Clinical_PhysicalExamSystemsDetail.bIsTextChanged = false;
    },
    
}