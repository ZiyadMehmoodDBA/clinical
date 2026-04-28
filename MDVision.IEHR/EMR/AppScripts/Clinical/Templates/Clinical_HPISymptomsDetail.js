Clinical_HPISymptomsDetail = {
    params: [],
    FindingsList: [],
    bIsFirstLoad: true,
    bIsTextChanged: false,

    Load: function (params) {

        Clinical_HPISymptomsDetail.params = params;
        Clinical_HPISymptomsDetail.loadHPISymptoms();
        Clinical_HPISymptomsDetail.bindfindingAutoComplete();
        Clinical_HPISymptomsDetail.validateHPISymptomsDetail();

    },

    validateHPISymptomsDetail: function () {
        var $self = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #frmHPISymptomsDetail");
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
            Clinical_HPISymptomsDetail.saveHPISymptoms();
        });

    },

    loadHPISymptoms: function () {

        if (Clinical_HPISymptomsDetail.params.mode == "Add") {

            $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #frmHPISymptomsDetail #btnSaveAs").attr('disabled', true);
        }
        else if (Clinical_HPISymptomsDetail.params.mode == "Edit") {

            Clinical_HPISymptomsDetail.loadHPISymptoms_DBcall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var listSymptoms = response.listSymptoms;
                        var listFindings = response.listFindings;
                        var $self = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #frmHPISymptomsDetail");
                        Clinical_HPISymptomsDetail.bindFindings(listFindings, true);
                        utility.bindMyJSONByName(true, listSymptoms[0], false, $self).done(function () {
                            //$('#frmHPISymptomsDetail').data('serialize', $('#frmHPISymptomsDetail').serialize());
                        });
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }

            });

        }
    },

    loadHPISymptoms_DBcall: function () {
        var objData = new Object();
        objData["HPISymptomsId"] = Clinical_HPISymptomsDetail.params.HPISymptomsId;
        objData["commandType"] = "fill_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },

    bindFindingToHTML: function (item, HPISymptomFindingsId, IsFromLoad) {


        //1- set finding into grid
        $ul = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #ulFinding");

        if ($ul.find("#Finding_" + item.HPIFindingsId).length <= 0) {
            delete_ = '<a href="#" class="rightbtn"><span class="removeIconListHover" onclick="Clinical_HPISymptomsDetail.removeFinding(this,\'' + item.HPIFindingsId + '\', event);">'
                                        + '<i class="fa fa-times"></i></span> </a>'
            var findingOrder = 1;
            if (IsFromLoad) {
                findingOrder = item.FindingOrder;
            }
            else {               
                if ($ul.find("li").length > 0) {
                    var lastOrder = $ul.find("li").last().attr('findingOrder');
                    if (lastOrder != null) {
                        findingOrder = parseInt(lastOrder) + 1;
                    }
                    else {
                        findingOrder = 1;
                    }
                }
            }

            li_ = '<li findingOrder="' + findingOrder + '" HPIFindingsId="' + item.HPIFindingsId + '" id="Finding_' + item.HPIFindingsId + '" SymptomFindingsId="' + HPISymptomFindingsId + '" data-lab="' + item.Name + '"><span id="spnLabName">' + item.Name + '</span>' + delete_ + '</li>';

            $ul.append(li_);

            //2- set its hidden value 
            if (IsFromLoad != true) {
                var array = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val().split(',');
                var index_ = array.indexOf(item.HPIFindingsId);
                if (index_ < 0) {
                    array.push(item.HPIFindingsId);
                    $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val(array.join());
                }
            }
        }
        else
            utility.DisplayMessages("Finding already exists", 3);
    },

    bindFindings: function (dataItems, IsFromLoad) {

        $.each(dataItems, function (i, item) {

            var HPISymptomFindingsId = "";
            if (item.HPISymptomFindingsId)
                HPISymptomFindingsId = item.HPISymptomFindingsId;

            //add finding to html
            Clinical_HPISymptomsDetail.bindFindingToHTML(item, HPISymptomFindingsId, IsFromLoad);
        });
    },

    removeFindingFromHTML: function ($obj, HPIFindingsId) {

        $($obj).parent().parent().remove();
        var array = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val().split(',');
        var index_ = array.indexOf(HPIFindingsId);
        if (index_ >= 0) {
            array.splice(index_, 1);
            $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val(array.join());
        }

    },

    removeFinding: function ($obj, HPIFindingsId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var SymptomFindingsId = $($obj).parent().parent().attr('SymptomFindingsId');
        if (SymptomFindingsId) {
            utility.myConfirm("1", function () {
                Clinical_HPISymptomsDetail.deleteHPI_Finding_DBCall(SymptomFindingsId).done(function (response) {

                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            Clinical_HPISymptomsDetail.removeFindingFromHTML($obj, HPIFindingsId);
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    }

                });

            }, function () { });
        }
        else {
            Clinical_HPISymptomsDetail.removeFindingFromHTML($obj, HPIFindingsId);
        }


    },

    bindAddedFinding: function (dataItems) {

        if (dataItems && dataItems.length > 0) {
            $.each(dataItems, function (i, item) {
                $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #ulFinding")
                    .find("#Finding_" + item.HPIFindingsId).attr("SymptomFindingsId", item.HPISymptomFindingsId);
            });
        }
        $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val('');
        //Clinical_HPISymptomsDetail.bindFindings(dataItems, true);

    },

    saveHPISymptoms: function () {


        var strMessage = "";
        var self = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #frmHPISymptomsDetail");
        var myJSON = self.getMyJSONByName();
        var name_ = self.find("#txtName").val();
        if (name_ != "") {

            if (Clinical_HPISymptomsDetail.params.mode == "Add") {
                Clinical_HPISymptomsDetail.saveHPISymptoms_DBCall(myJSON).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            // Enable Save as 
                            $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #btnSaveAs").removeAttr('disabled');
                            Clinical_HPISymptomsDetail.bindAddedFinding(response.listFindings);

                            Clinical_HPISymptomsDetail.params.mode = "Edit";
                            Clinical_HPISymptomsDetail.params.HPISymptomsId = response.HPISymptomsId;

                            $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #ulFinding li").remove();
                            $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtFindings").val('');

                            Clinical_HPISymptomsDetail.loadHPISymptoms();
                            Clinical_HPISymptoms.searchHPISymptoms();

                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else if (Clinical_HPISymptomsDetail.params.mode == "Edit") {

                Clinical_HPISymptomsDetail.updateHPISymptoms_DBCall(myJSON, Clinical_HPISymptomsDetail.params.HPISymptomsId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_HPISymptomsDetail.bindAddedFinding(response.listFindings);

                            utility.DisplayMessages(response.Message, 1);
                            Clinical_HPISymptoms.searchHPISymptoms();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            Clinical_HPISymptomsDetail.bIsTextChanged = false;
        }
        else {
            utility.DisplayMessages('please enter symptom.', 3);
        }


    },

    bindfindingAutoComplete: function () {

        Clinical_HPISymptomsDetail.lookupHPIFinding_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtFindings").kendoAutoComplete({
                        dataSource: response.listFindings,
                        filter: "contains",
                        dataTextField: "Name",
                        placeholder: "Select Finding...",
                        select: function (e) {
                            var dataItem = this.dataItem(e.item.index());
                            Clinical_HPISymptomsDetail.bindFindingToHTML(dataItem, "", false);
                            setTimeout(function () {
                                $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtFindings").val('');
                            }, 100);
                        },
                    });
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    fillHPIFinding: function (id) {

        var dataItem = $.grep(Clinical_HPISymptomsDetail.FindingsList, function (item_) { return (item_.HPIFindingsId == id) });
        if (dataItem.length > 0) {
            if (dataItem[0].IsActive == "True") {
                $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtFindings").val('');
                //$('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtFindings").val(dataItem[0].Name);
                Clinical_HPISymptomsDetail.bindFindingToHTML(dataItem[0], "", false);
                Clinical_HPIFindings.UnLoadTab();
            }
            else
                utility.DisplayMessages('Inactive Finding.', 3);
        }
        else
            Clinical_HPIFindings.UnLoadTab();

    },

    TextChanged: function () {

        Clinical_HPISymptomsDetail.bIsTextChanged = true;

        //if ($(element).is(':checked')) {
        //    $("" + panelId + " #dvresion").fadeOut();
        //}
        //else
        //    $("" + panelId + " #dvresion").fadeIn();

    },
    saveAs: function (obj) {
        $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtName").val('');
        $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #txtFindings").val('');
        $('#' + Clinical_HPISymptomsDetail.params.PanelID + ' #frmHPISymptomsDetail').bootstrapValidator('revalidateField', 'Name');
        Clinical_HPISymptomsDetail.params["mode"] = "Add";
        $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #ulFinding li").each(function (i, item) {
            var Id = $(item).attr("HPIFindingsId");
            $(item).attr("symptomfindingsid", ''); // clear this Id so user can't remove association from pervious Symptom
            var isnormal = $(item).attr("data-lab").toLowerCase() == "is normal" ? true : false;
            if (Id && !isnormal) {
                var array = $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val().split(',');
                var index_ = array.indexOf(Id);
                if (index_ < 0) {
                    array.push(Id);
                    $('#' + Clinical_HPISymptomsDetail.params.PanelID + " #hfHPIFindingsIds").val(array.join());
                }
            }
        });
        Clinical_HPISymptomsDetail.bIsTextChanged = false;
    },

    OpenFinding: function () {

        Clinical_HPISymptomsDetail.FindingsList = [];
        var params = [];
        params["IsOptional"] = false;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_HPISymptomsDetail";
        LoadActionPan('Clinical_HPIFindings', params);

    },

    lookupHPIFinding_DBCall: function () {

        var objData = new Object();
        objData["IsActive"] = 1;
        objData["commandType"] = "lookup_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    saveHPISymptoms_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["commandType"] = "insert_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },

    updateHPISymptoms_DBCall: function (data, HPISymptomsId) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["HPISymptomsId"] = HPISymptomsId;
        objData["commandType"] = "update_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");

    },

    deleteHPI_Finding_DBCall: function (SymptomFindingsId) {

        var objData = new Object();
        objData["HPISymptomFindingsId"] = SymptomFindingsId;
        objData["commandType"] = "delete_hpi_symptom_finding";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    UnLoad: function () {
        if (Clinical_HPISymptomsDetail.params["FromAdmin"] == "0") {
            if (Clinical_HPISymptomsDetail.params != null && Clinical_HPISymptomsDetail.params.ParentCtrl != null) {
                if (Clinical_HPISymptomsDetail.bIsTextChanged) {
                    utility.UnLoadDialog("frmHPISymptomsDetail", function () {
                        UnloadActionPan(Clinical_HPISymptomsDetail.params.ParentCtrl, 'Clinical_HPISymptomsDetail', null, Clinical_HPISymptomsDetail.params.PanelID);
                    }, function () {
                        UnloadActionPan(Clinical_HPISymptomsDetail.params.ParentCtrl, 'Clinical_HPISymptomsDetail', null, Clinical_HPISymptomsDetail.params.PanelID);
                    });
                }
                else {
                    UnloadActionPan(null, 'Clinical_HPISymptomsDetail');

                }

            }
            else
                UnloadActionPan(null, 'Clinical_HPISymptomsDetail');
        }
        else {

            if (Clinical_HPISymptomsDetail.bIsTextChanged) {
                utility.UnLoadDialog("frmHPISymptomsDetail", function () {
                    UnloadActionPan(Clinical_HPISymptomsDetail.params.ParentCtrl, 'Clinical_HPISymptomsDetail', null, Clinical_HPISymptomsDetail.params.PanelID);
                }, function () {
                    UnloadActionPan(Clinical_HPISymptomsDetail.params.ParentCtrl, 'Clinical_HPISymptomsDetail', null, Clinical_HPISymptomsDetail.params.PanelID);
                });
            }
            else {
                UnloadActionPan(null, 'Clinical_HPISymptomsDetail');

            }
        }
        Clinical_HPISymptomsDetail.bIsTextChanged = false;
    },

}