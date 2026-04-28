Clinical_HPIFindingsDetail = {
    params: [],
    bIsFirstLoad: true,
    IstoExit: false,
    Load: function (params) {
        Clinical_HPIFindingsDetail.params = params;
        Clinical_HPIFindingsDetail.IstoExit = false;
        //if (Clinical_HPIFindingsDetail.params.PanelID != "pnlAdminHPIFindingsDetail")
        //    Clinical_HPIFindingsDetail.params.PanelID = Clinical_HPIFindingsDetail.params.PanelID + " #pnlAdminHPIFindingsDetail";
        //else
        //    Clinical_HPIFindingsDetail.params.PanelID = "pnlAdminHPIFindingsDetail";

        //Clinical_HPIFindingsDetail.bindsystemAutoComplete();
        Clinical_HPIFindingsDetail.loadHPIFindingsDetail();
    },


    validateHPIFindingsDetail: function () {
        var $self = $('#' + Clinical_HPIFindingsDetail.params.PanelID + " #frmHPIFindingsDetail");
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
            Clinical_HPIFindingsDetail.saveHPIFindings();
        });

    },

    loadHPIFindingsDetail: function () {

        if (Clinical_HPIFindingsDetail.params.mode == "Add") {

            //serialize data
            $('#' + Clinical_HPIFindingsDetail.params.PanelID + ' #frmHPIFindingsDetail').data('serialize', $('#' + Clinical_HPIFindingsDetail.params.PanelID + ' #frmHPIFindingsDetail').serialize());
            Clinical_HPIFindingsDetail.validateHPIFindingsDetail();
        }
        else if (Clinical_HPIFindingsDetail.params.mode == "Edit") {

            Clinical_HPIFindingsDetail.loadHPIFindingsDetail_DBcall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var $self = $('#' + Clinical_HPIFindingsDetail.params.PanelID + " #frmHPIFindingsDetail");
                        utility.bindMyJSONByName(true, response.listFindings[0], false, $self).done(function () {
                            $('#' + Clinical_HPIFindingsDetail.params.PanelID + ' #frmHPIFindingsDetail').data('serialize', $('#' + Clinical_HPIFindingsDetail.params.PanelID + ' #frmHPIFindingsDetail').serialize());
                        });

                        Clinical_HPIFindingsDetail.validateHPIFindingsDetail();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    loadHPIFindingsDetail_DBcall: function () {
        var objData = new Object();
        objData["HPIFindingsId"] = Clinical_HPIFindingsDetail.params.HPIFindingsId;
        objData["commandType"] = "fill_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    saveHPIFindings: function () {

        var strMessage = "";
        var self = $('#' + Clinical_HPIFindingsDetail.params.PanelID + " #frmHPIFindingsDetail");
        var myJSON = self.getMyJSONByName();
        var name_ = self.find("#txtName").val();
        if (name_ != "") {

            if (Clinical_HPIFindingsDetail.params.mode == "Add") {
                Clinical_HPIFindingsDetail.saveHPIFindings_DBCall(myJSON).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_HPIFindingsDetail.resetToAdd();
                            Clinical_HPIFindingsDetail.bindFindingToHTML(name_);
                            Clinical_HPIFindings.searchHPIFindings();
                            if (Clinical_HPIFindingsDetail.IstoExit)
                                Clinical_HPIFindingsDetail.UnLoad('saveExit');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else if (Clinical_HPIFindingsDetail.params.mode == "Edit") {

                Clinical_HPIFindingsDetail.updateHPIFindings_DBCall(myJSON, Clinical_HPIFindingsDetail.params.HPIFindingsId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_HPIFindings.searchHPIFindings();
                            if (Clinical_HPIFindingsDetail.IstoExit)
                                Clinical_HPIFindingsDetail.UnLoad('saveExit');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }

        }
        else {
            utility.DisplayMessages('please enter finding.', 3);
        }


    },

    bindFindingToHTML: function (item) {
        $ul = $('#' + Clinical_HPIFindingsDetail.params.PanelID + " #ulFinding");
        li_ = '<li data-lab="' + item + '"><span id="spnLabName">' + item + '</span></li>';
        $ul.append(li_);
    },

    resetToAdd: function () {
        var $self = $('#' + Clinical_HPIFindingsDetail.params.PanelID + " #frmHPIFindingsDetail");
        $self.find("#txtName").val("");
        $self.find('[type=checkbox]').each(function () {
            this.checked = true;
        });
        $self.bootstrapValidator('revalidateField', 'Name');

    },

    SetforExit: function () {
        var $self = $('#' + Clinical_HPIFindingsDetail.params.PanelID + " #frmHPIFindingsDetail");
        if ($self.find("#txtName").val() != "")
            Clinical_HPIFindingsDetail.IstoExit = true;
    },

    saveHPIFindings_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);
        objData["IsActive"] = true;
        objData["commandType"] = "insert_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    updateHPIFindings_DBCall: function (data, HPIFindingsId) {

        var objData = new Object();
        if (data)
            objData = JSON.parse(data);

        objData["HPIFindingsId"] = HPIFindingsId;
        objData["commandType"] = "update_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");

    },

    UnLoad: function (caller) {

        if (Clinical_HPIFindingsDetail.params["FromAdmin"] == "0") {

            if (Clinical_HPIFindingsDetail.params != null && Clinical_HPIFindingsDetail.params.ParentCtrl != null) {
                if (caller == 'saveExit') {
                    UnloadActionPan(Clinical_HPIFindingsDetail.params.ParentCtrl, 'Clinical_HPIFindingsDetail', null, Clinical_HPIFindingsDetail.params.PanelID);
                }
                else {
                    utility.UnLoadDialog("frmHPIFindingsDetail", function () {
                        UnloadActionPan(Clinical_HPIFindingsDetail.params.ParentCtrl, 'Clinical_HPIFindingsDetail', null, Clinical_HPIFindingsDetail.params.PanelID);
                    }, function () {
                        UnloadActionPan(Clinical_HPIFindingsDetail.params.ParentCtrl, 'Clinical_HPIFindingsDetail', null, Clinical_HPIFindingsDetail.params.PanelID);
                    });

                }
            }
            else
                UnloadActionPan(null, 'Clinical_HPIFindingsDetail');

        }
        else {
            if (caller == 'saveExit') {
                UnloadActionPan();

            }
            else {
                utility.UnLoadDialog("frmHPIFindingsDetail", function () {
                    UnloadActionPan(Clinical_HPIFindingsDetail.params.ParentCtrl, 'Clinical_HPIFindingsDetail', null, Clinical_HPIFindingsDetail.params.PanelID);
                }, function () {
                    UnloadActionPan(Clinical_HPIFindingsDetail.params.ParentCtrl, 'Clinical_HPIFindingsDetail', null, Clinical_HPIFindingsDetail.params.PanelID);
                });
            }
        }
    },

}