Clinical_ROSSystemsDetail = {
    params: [],
    CharatristicsList: [],
    bIsFirstLoad: true,
    bIsTextChanged: false,
    IstoExit: false,

    Load: function (params) {

        Clinical_ROSSystemsDetail.params = params;
        Clinical_ROSSystemsDetail.loadROSSystems();
        Clinical_ROSSystemsDetail.bindcharatristicsAutoComplete();
        Clinical_ROSSystemsDetail.validatePhysicalExamSystemsDetail();
        Clinical_ROSSystemsDetail.IstoExit = false;

    },

    validatePhysicalExamSystemsDetail: function () {
        var $self = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #frmROSSystem");
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
            Clinical_ROSSystemsDetail.saveROSystem();
        });

    },

    loadROSSystems: function () {

        if (Clinical_ROSSystemsDetail.params.mode == "Add") {

            $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #frmROSSystem #btnSaveAs").attr('disabled', true);
        }
        else if (Clinical_ROSSystemsDetail.params.mode == "Edit") {

            Clinical_ROSSystemsDetail.loadROSSystems_DBcall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var SystemsDataJSON = JSON.parse(response.SystemLoad_JSON);
                        var ObservationLoad_JSON = JSON.parse(response.ObservationLoad_JSON);
                        var $self = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #frmROSSystem");
                        Clinical_ROSSystemsDetail.bindROSCharatristics(ObservationLoad_JSON, true);
                        utility.bindMyJSONByName(true, SystemsDataJSON[0], false, $self).done(function () {
                            //$('#frmROSSystem').data('serialize', $('#frmROSSystem').serialize());
                        });
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }

            });

        }
    },

    loadROSSystems_DBcall: function () {
        var objData = new Object();
        objData["ROSSystemId"] = Clinical_ROSSystemsDetail.params.ROSSystemId;
        objData["commandType"] = "fill_reviewofsystems_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    bindCharatristicsToHTML: function (item, ROSSystemCharatristicsId, IsFromLoad) {


        //1- set observation into grid
        $ul = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #ulCharatristics");

        if ($ul.find("#Charatristic_" + item.ROSCharacteristicsId).length <= 0) {
            delete_ = '<a href="#" class="rightbtn"><span class="removeIconListHover" onclick="Clinical_ROSSystemsDetail.removeObservation(this,\'' + item.ROSCharacteristicsId + '\', event);">'
                                        + '<i class="fa fa-times"></i></span> </a>'
            li_ = '<li ROSCharatristicsId="' + item.ROSCharacteristicsId + '" id="Charatristic_' + item.ROSCharacteristicsId + '" SystemCharatristicsId="' + ROSSystemCharatristicsId + '" data-lab="' + item.Name + '"><span id="spnLabName">' + item.Name + '</span>' + delete_ + '</li>';

            $ul.append(li_);

            //2- set its hidden value 
            if (IsFromLoad != true || Clinical_ROSSystemsDetail.params.mode == "Edit") {
                var array = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val().split(',');
                var index_ = array.indexOf(item.ROSCharacteristicsId);
                if (index_ < 0) {
                    array.push(item.ROSCharacteristicsId);
                    $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val(array.join());
                }
            }
        } else {           
            utility.DisplayMessages("A characteristic with same name already exists", 3);
        }
    },

    bindROSCharatristics: function (dataItems, IsFromLoad) {

        $.each(dataItems, function (i, item) {

            var ROSSystemCharatristicsId = "";
            if (item.ROSSystemCharatristicsId)
                ROSSystemCharatristicsId = item.ROSSystemCharatristicsId;

            //add observation to html
            Clinical_ROSSystemsDetail.bindCharatristicsToHTML(item, ROSSystemCharatristicsId, IsFromLoad);
        });
    },

    removeObservationFromHTML: function ($obj, PEObservationId) {

        $($obj).parent().parent().remove();
        var array = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val().split(',');
        var index_ = array.indexOf(PEObservationId);
        if (index_ >= 0) {
            array.splice(index_, 1);
            $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val(array.join());
        }

    },

    removeObservation: function ($obj, PEObservationId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var SystemObservationId = $($obj).parent().parent().attr('SystemObservationId');
        if (SystemObservationId) {
            utility.myConfirm("1", function () {
                Clinical_ROSSystemsDetail.deletePESystem_Observation_DBCall(SystemObservationId).done(function (response) {

                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            Clinical_ROSSystemsDetail.removeObservationFromHTML($obj, PEObservationId);
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    }

                });

            }, function () { });
        }
        else {
            Clinical_ROSSystemsDetail.removeObservationFromHTML($obj, PEObservationId);
        }


    },

    bindAddedObservations: function (dataItems, Isupdate) {
        var charIds = '';
        if (dataItems && dataItems.length > 0) {
            $.each(dataItems, function (i, item) {
                $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #ulCharatristics")
                    .find("#Charatristic_" + item.ROSCharacteristicsId).attr("ROSCharacteristicsId", item.ROSCharacteristicsId);
                acharIds = charIds + ',' + item.ROSCharacteristicsId;
            });
        }
        $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val('');
        if (Isupdate = true) {
            $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val(acharIds);
        }
        //Clinical_ROSSystemsDetail.bindROSCharatristics(dataItems, true);

    },

    saveROSystem: function () {


        var strMessage = "";
        var self = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #frmROSSystem");
        var myJSON = self.getMyJSONByName();
        var name_ = self.find("#txtName").val();
        if (name_ != "") {

            if (Clinical_ROSSystemsDetail.params.mode == "Add") {
                Clinical_ROSSystemsDetail.saveROSystem_DBCall(myJSON).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            // Enable Save as 
                            $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #btnSaveAs").removeAttr('disabled');
                            var PEObservation_JSON = JSON.parse(response.PEObservation_JSON);
                            if (PEObservation_JSON.length > 0) {
                                Clinical_ROSSystemsDetail.bindAddedObservations(PEObservation_JSON);

                                Clinical_ROSSystemsDetail.params.mode = "Edit";
                                Clinical_ROSSystemsDetail.params.ROSSystemId = response.ROSSystemId;

                                $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #ulCharatristics li").remove();
                                $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtCharatristics").val('');
                            }
                            Clinical_ROSSystemsDetail.loadROSSystems();
                            Clinical_ROSSystems.searchROSSystems();

                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else if (Clinical_ROSSystemsDetail.params.mode == "Edit") {

                Clinical_ROSSystemsDetail.updateROSSystem_DBCall(myJSON, Clinical_ROSSystemsDetail.params.ROSSystemId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            var PEObservation_JSON = JSON.parse(response.PEObservation_JSON);
                            Clinical_ROSSystemsDetail.bindAddedObservations(PEObservation_JSON, true);

                            utility.DisplayMessages(response.Message, 1);
                            Clinical_ROSSystems.searchROSSystems();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            Clinical_ROSSystemsDetail.bIsTextChanged = false;
            if (Clinical_ROSSystemsDetail.IstoExit) {
                Clinical_ROSSystemsDetail.UnLoad();
            }
        }
        else {
            utility.DisplayMessages('please enter system.', 3);
        }


    },

    bindcharatristicsAutoComplete: function () {

        Clinical_ROSSystemsDetail.lookupROSCharatristics_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Observation_JSON = JSON.parse(response.ObservationLoad_JSON);

                    $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtCharatristics").kendoAutoComplete({
                        dataSource: Observation_JSON,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Characteristics...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            Clinical_ROSSystemsDetail.bindCharatristicsToHTML(dataItem, "", false);
                            setTimeout(function () {
                                $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtCharatristics").val('');
                            }, 100);
                        },
                    });
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    fillROSCharatristics: function (id) {

        var dataItem = $.grep(Clinical_ROSSystemsDetail.CharatristicsList, function (item_) { return (item_.ROSCharacteristicsId == id) });
        if (dataItem.length > 0) {
            if (dataItem[0].IsActive == "True") {
                $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtCharatristics").val('');
                //$('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtCharatristics").val(dataItem[0].Name);
                Clinical_ROSSystemsDetail.bindCharatristicsToHTML(dataItem[0], "", false);
                Clinical_ROSCharatristics.UnLoadTab();
            }
            else
                utility.DisplayMessages('Inactive Observation.', 3);
        }
        else
            Clinical_ROSCharatristics.UnLoadTab();

    },

    TextChanged: function () {

        Clinical_ROSSystemsDetail.bIsTextChanged = true;

        //if ($(element).is(':checked')) {
        //    $("" + panelId + " #dvresion").fadeOut();
        //}
        //else
        //    $("" + panelId + " #dvresion").fadeIn();

    },
    saveAs: function (obj) {
        $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtName").val('');
        $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #txtCharatristics").val('');
        $('#' + Clinical_ROSSystemsDetail.params.PanelID + ' #frmROSSystem').bootstrapValidator('revalidateField', 'Name');
        Clinical_ROSSystemsDetail.params["mode"] = "Add";
        $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #ulCharatristics li").each(function (i, item) {
            var Id = $(item).attr("roscharatristicsid");
            $(item).attr("systemcharatristicsid", ''); // clear this Id so user can't remove association from pervious System
            var isnormal = $(item).attr("data-lab").toLowerCase() == "is normal" ? true : false;
            if (Id && !isnormal) {
                var array = $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val().split(',');
                var index_ = array.indexOf(Id);
                if (index_ < 0) {
                    array.push(Id);
                    $('#' + Clinical_ROSSystemsDetail.params.PanelID + " #hfROSCharatristicsIds").val(array.join());
                }
            }
        });
        Clinical_ROSSystemsDetail.bIsTextChanged = false;
    },

    OpenCharatristics: function () {

        Clinical_ROSSystemsDetail.CharatristicsList = [];
        var params = [];
        params["IsOptional"] = false;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ROSSystemsDetail";
        LoadActionPan('Clinical_ROSCharatristics', params);

    },

    lookupROSCharatristics_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_reviewofsystem_charatristics"; /* lookup_physicalexam_observation */
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    saveROSystem_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["commandType"] = "insert_reviewofsystem_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    updateROSSystem_DBCall: function (data, ROSSystemId) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["ROSSystemId"] = ROSSystemId;
        objData["commandType"] = "update_reviewofsystem_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");

    },

    deletePESystem_Observation_DBCall: function (SystemObservationId) {

        var objData = new Object();
        objData["PESystemObservationId"] = SystemObservationId;
        objData["commandType"] = "delete_physicalexam_system_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    SetforExit: function () {
        var $self = $('#' + Clinical_ROSCharatristicsDetail.params.PanelID + " #frmROSSystem");
        if ($self.find("#txtName").val() != "")
            Clinical_ROSSystemsDetail.IstoExit = true;
    },

    UnLoad: function () {
        if (Clinical_ROSSystemsDetail.params["FromAdmin"] == "0") {
            if (Clinical_ROSSystemsDetail.params != null && Clinical_ROSSystemsDetail.params.ParentCtrl != null) {
                if (Clinical_ROSSystemsDetail.bIsTextChanged) {
                    utility.UnLoadDialog("frmROSSystem", function () {
                        UnloadActionPan(Clinical_ROSSystemsDetail.params.ParentCtrl, 'Clinical_ROSSystemsDetail', null, Clinical_ROSSystemsDetail.params.PanelID);
                    }, function () {
                        UnloadActionPan(Clinical_ROSSystemsDetail.params.ParentCtrl, 'Clinical_ROSSystemsDetail', null, Clinical_ROSSystemsDetail.params.PanelID);
                    });
                }
                else {
                    UnloadActionPan(null, 'Clinical_ROSSystemsDetail');

                }

            }
            else
                UnloadActionPan(null, 'Clinical_ROSSystemsDetail');
        }
        else {

            if (Clinical_ROSSystemsDetail.bIsTextChanged) {
                utility.UnLoadDialog("frmROSSystem", function () {
                    UnloadActionPan(Clinical_ROSSystemsDetail.params.ParentCtrl, 'Clinical_ROSSystemsDetail', null, Clinical_ROSSystemsDetail.params.PanelID);
                }, function () {
                    UnloadActionPan(Clinical_ROSSystemsDetail.params.ParentCtrl, 'Clinical_ROSSystemsDetail', null, Clinical_ROSSystemsDetail.params.PanelID);
                });
            }
            else {
                UnloadActionPan(null, 'Clinical_ROSSystemsDetail');

            }
        }
        Clinical_ROSSystemsDetail.bIsTextChanged = false;
    },

}