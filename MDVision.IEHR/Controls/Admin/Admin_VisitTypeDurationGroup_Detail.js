VisitTypeDetail = {
    params: [],
    arrOfObj: [],
 
    //isempty: false,
    Load: function (params) {
        VisitTypeDetail.params = params;
        var VisitGroupId = VisitTypeDetail.params.VisitTypeDurationGroupId;
        $('#VisitTypeDetail #hfVisitId').val();
        var self = $('#pnlAdmin_VisitTypeDurationGroupDetail');
        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            $("#pnlAdmin_VisitTypeDurationGroupDetail #ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                $("#pnlAdmin_VisitTypeDurationGroupDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            VisitTypeDetail.VisitTypeLookup();
            VisitTypeDetail.LoadVisitTypeDurationGroup(VisitGroupId);
        });

    },

    LoadVisitTypeDurationGroup: function (VisitGroupId) {
        VisitTypeDetail.ValidateVisitTypeDurationGroup();
        if (VisitTypeDetail.params.mode == "Add") {
            $('#frmVisitTypeDurationGroupDetail').data('serialize', $('#frmVisitTypeDurationGroupDetail').serialize());
        }
        else if (VisitTypeDetail.params.mode == "Edit") {
            VisitTypeDetail.FillVisitTypeForm(VisitGroupId).done(function (response) {
                if (response.status == true) {
                    VisitTypeDetail.bindMyJson(response.VisitDurationGroup_JSON, VisitGroupId);
                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
     
    bindMyJson: function (VisitDurationGroup_JSON, VisitGroupId) {
        $("#pnlAdmin_VisitTypeDurationGroupDetail #txtName").val(VisitDurationGroup_JSON[0].Name);
        $("#pnlAdmin_VisitTypeDurationGroupDetail #ddlEntity").val(VisitDurationGroup_JSON[0].EntityId);
        $("#pnlAdmin_VisitTypeDurationGroupDetail #chkActive").attr("checked", VisitDurationGroup_JSON[0].IsActive);
        $("#pnlAdmin_VisitTypeDurationGroupDetail #dgvNewPatientRecords tbody tr").remove();
        $("#pnlAdmin_VisitTypeDurationGroupDetail #dgvEstablishedPatientRecords tbody tr").remove();

        for (i = 0; i < VisitDurationGroup_JSON.length; i++) {
            if (VisitDurationGroup_JSON[i].PatientTypeId == 1) {
                var $row = $('<tr id="' + VisitDurationGroup_JSON[i].VisitTypeId + '"/>');
                $row.attr("VisitTypeId", VisitDurationGroup_JSON[i].VisitTypeId);
                $row.attr("VisitDurationGroupId", VisitDurationGroup_JSON[i].Id);
                $row.attr("PatientTypeId", VisitDurationGroup_JSON[i].PatientTypeId);
                $row.append('<td style="display:none;">' + VisitDurationGroup_JSON[i].VisitTypeId + '</td><td>' + VisitDurationGroup_JSON[i].VisitTypeName + ' - New Patient</td><td><input class="form-control" type="text" visittypeid="' + VisitDurationGroup_JSON[i].VisitTypeId + '" id="' + VisitDurationGroup_JSON[i].VisitTypeId + '" value="' + VisitDurationGroup_JSON[i].Duration + '" maxlength="3" placeholder="999" name="Duration' + (i + 1) + '" onkeypress="return utility.ValidateZeroAndNumeric(event, this);"></td><td><div class="input-group color demo2 colorpickerdiv" data-plugin-colorpicker ><span class="input-group-addon" style="padding:1px 4px; padding-top:2px;"><i></i></span><input type="text" class="form-control" name="Color" value=' + VisitDurationGroup_JSON[i].Color + ' id="txtColor"></div></td>');
                $("#dgvNewPatientRecords tbody").last().append($row);
            }
            else if (VisitDurationGroup_JSON[i].PatientTypeId == 2) {
                var $row = $('<tr id="' + VisitDurationGroup_JSON[i].VisitTypeId + '"/>');
                $row.attr("VisitTypeId", VisitDurationGroup_JSON[i].VisitTypeId);
                $row.attr("VisitDurationGroupId", VisitDurationGroup_JSON[i].Id);
                $row.attr("PatientTypeId", VisitDurationGroup_JSON[i].PatientTypeId);
                $row.append('<td style="display:none;">' + VisitDurationGroup_JSON[i].VisitTypeId + '</td><td>' + VisitDurationGroup_JSON[i].VisitTypeName + ' - Established Patient</td><td><input class="form-control" type="text" visittypeid="' + VisitDurationGroup_JSON[i].VisitTypeId + '" id="' + VisitDurationGroup_JSON[i].VisitTypeId + '" value="' + VisitDurationGroup_JSON[i].Duration + '"maxlength="3" placeholder="999" name="Duration' + (i + 1) + '" onkeypress="return utility.ValidateZeroAndNumeric(event, this);"></td><td><div class="input-group color demo2 colorpickerdiv" data-plugin-colorpicker ><span class="input-group-addon" style="padding:1px 4px; padding-top:2px;"><i></i></span><input type="text" class="form-control" name="Color" value=' + VisitDurationGroup_JSON[i].Color + ' id="txtColor"></div></td>');
                $("#dgvEstablishedPatientRecords tbody").append($row);
            }
        }
        //$(".colorpicker").kendoColorPicker({
        //    value: "#08c",
        //    buttons: false
        //});
        $(".colorpickerdiv").colorpicker();
       

        $('.colorpickerdiv').colorpicker().on('changeColor.colorpicker', function (event) {
            //$('#frmFacilityDetail').bootstrapValidator('revalidateField', 'Color');
            //$('#frmFacilityDetail').bootstrapValidator('updateStatus', 'Color', 'NOT_VALIDATED');
        });
    },
   

    ValidateVisitTypeDurationGroup: function () {
        $('#frmVisitTypeDurationGroupDetail')
           .bootstrapValidator({
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Name: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Entity: {
                       group: '.col-xs-5',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            VisitTypeDetail.VisitDurationGroupSave();
        });
    },

    ValidateDurations: function () {
        var isempty = false;
        for (i = 0; i < $("#dgvNewPatientRecords tbody tr td input").length; i++) {
            if ($($("#dgvNewPatientRecords tbody tr td input")[i]).val() == "") {
                utility.DisplayMessages($($($("#dgvNewPatientRecords tbody tr")[i]).find('td')[1]).text() + " Field Empty", 3);
                isempty = true;
                break;
            }
        }

        for (i = 0; i < $("#dgvEstablishedPatientRecords tbody tr td input").length; i++) {
            if ($($("#dgvEstablishedPatientRecords tbody tr td input")[i]).val() == "") {
                utility.DisplayMessages($($($("#dgvEstablishedPatientRecords tbody tr")[i]).find('td')[1]).text() + " Field Empty", 3);
                isempty = true;
                break;
            }
        }
        return isempty;
    },

    VisitTypeLookup: function () {
        CacheManager.BindCodes('GetPatientVisitType', true).done(function (response) {
            var PatientVisitTypeData = JSON.parse(response.GetPatientVisitType);
            if (VisitTypeDetail.params.mode == "Add") {
                $.each(PatientVisitTypeData, function (i, item) {
                    if (item.RefValue == 1) {
                        var $row = $('<tr id="' + item.Value + '"/>');
                        $row.attr("VisitTypeId", item.Value);
                        $row.attr("PatientTypeId", item.RefValue);
                        if (item.Value == 4)
                            $row.append('<td style="display:none;">' + item.Value + '</td><td>' + item.Name + '</td><td><input class="form-control" id="' + item.Value + '"type="text"  maxlength="3" placeholder="999" name="Duration' + item.Value + '" value=30 onkeypress="return utility.ValidateZeroAndNumeric(event, this);"  /></td><td><div class="input-group color demo2 colorpickerdiv" data-plugin-colorpicker ><span class="input-group-addon" style="padding:1px 4px; padding-top:2px;"><i></i></span><input type="text" class="form-control" name="Color" value="#08c" id="txtColor"></div></td>');
                        else
                            $row.append('<td style="display:none;">' + item.Value + '</td><td>' + item.Name + '</td><td><input class="form-control" id="' + item.Value + '"type="text"  maxlength="3" placeholder="999" name="Duration' + item.Value + '" value=45 onkeypress="return utility.ValidateZeroAndNumeric(event, this);"  /></td><td><div class="input-group color demo2 colorpickerdiv" data-plugin-colorpicker ><span class="input-group-addon" style="padding:1px 4px; padding-top:2px;"><i></i></span><input type="text" class="form-control" name="Color" value="#08c" id="txtColor"></div></td>');
                        $("#pnlNewPatientsResult #dgvNewPatientRecords tbody").last().append($row);
                    }
                    else if (item.RefValue == 2) {
                        var $row = $('<tr id="' + item.Value + '"/>');
                        $row.attr("VisitTypeId", item.Value);
                        $row.attr("PatientTypeId", item.RefValue);
                        $row.append('<td style="display:none;">' + item.Value + '</td><td>' + item.Name + '</td><td><input class="form-control" id="' + item.Value + '" type="text" maxlength="3" placeholder="999" name="Duration' + item.Value + '" value=30 onkeypress="return utility.ValidateZeroAndNumeric(event, this);"  /></td><td><div class="input-group color demo2 colorpickerdiv" data-plugin-colorpicker ><span class="input-group-addon" style="padding:1px 4px; padding-top:2px;"><i></i></span><input type="text" class="form-control" name="Color" value="#08c" id="txtColor"></div></td>');
                        $("#pnlEstablishedPatientsResult #dgvEstablishedPatientRecords tbody").last().append($row);
                    }
                });
            }
            $(".colorpickerdiv").colorpicker();

        });
    },

    GetVisitTypeDurationArray: function () {
        if (VisitTypeDetail.params.mode == "Add") {
            for (i = 0; i < $("#dgvNewPatientRecords tbody tr").length ; i++) {
                VisitTypeDetail.arrOfObj.push({ "VisitTypeId": $($("#dgvNewPatientRecords tbody tr")[i]).attr('visittypeid'), "Duration": $($("#dgvNewPatientRecords tbody tr")[i]).find('input').eq(0).val(), "Color": $($("#dgvNewPatientRecords tbody tr")[i]).find('input').eq(1).val() });
            }
            for (i = 0; i < $("#dgvEstablishedPatientRecords tbody tr").length ; i++) {
                VisitTypeDetail.arrOfObj.push({ "VisitTypeId": $($("#dgvEstablishedPatientRecords tbody tr")[i]).attr('visittypeid'), "Duration": $($("#dgvEstablishedPatientRecords tbody tr")[i]).find('input').eq(0).val(), "Color": $($("#dgvEstablishedPatientRecords tbody tr")[i]).find('input').eq(1).val() });

            }
        }
        else if (VisitTypeDetail.params.mode == "Edit") {
            for (i = 0; i < $("#dgvNewPatientRecords tbody tr").length ; i++) {
                VisitTypeDetail.arrOfObj.push({ "VisitTypeId": $($("#dgvNewPatientRecords tbody tr")[i]).attr('visittypeid'), "Duration": $($("#dgvNewPatientRecords tbody tr")[i]).find('input').eq(0).val(), "VisitDurationGroupId": $($("#dgvNewPatientRecords tbody tr")[i]).attr('visitdurationgroupid'), "Color": $($("#dgvNewPatientRecords tbody tr")[i]).find('input').eq(1).val() });

            }
            for (i = 0; i < $("#dgvEstablishedPatientRecords tbody tr").length ; i++) {
                VisitTypeDetail.arrOfObj.push({ "VisitTypeId": $($("#dgvEstablishedPatientRecords tbody tr")[i]).attr('visittypeid'), "Duration": $($("#dgvEstablishedPatientRecords tbody tr")[i]).find('input').eq(0).val(), "VisitDurationGroupId": $($("#dgvEstablishedPatientRecords tbody tr")[i]).attr('visitdurationgroupid'), "Color": $($("#dgvEstablishedPatientRecords tbody tr")[i]).find('input').eq(1).val() });
            }
        }
    },

    VisitDurationGroupSave: function (VisitTypeDurationGroupId) {

        var strMessage = "";
        var self = $('#pnlAdmin_VisitTypeDurationGroupDetail');
        var myJson = {};

        if (VisitTypeDetail.params.mode == "Add") {
            var retVal = VisitTypeDetail.ValidateDurations();
            myJson["VisitDurationGroupName"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #txtName").val();
            myJson["Entity"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #ddlEntity").val();
            $("#pnlAdmin_VisitTypeDurationGroupDetail #chkActive").is(':checked') ? myJson["IsActive"] = true : myJson["IsActive"] = false;
            myJson["VisitGroupId"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #hfVisitId").val();
            var obj = JSON.stringify(myJson);

            AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    $("#pnlAdmin_VisitTypeDurationGroupDetail #SaveButton").removeAttr('disabled');
                    if (retVal == true) {
                    }
                    else {
                        VisitTypeDetail.SaveVisitGroupData(obj).done(function (response) {
                            if (response.status == true) {
                                VisitTypeDetail.VisitTypeDurationsDataSave(response.VisitTypeDurationGroupId);
                            }
                            else
                                utility.DisplayMessages(response.message, 3);
                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

        else if (VisitTypeDetail.params.mode == "Edit") {
            myJson["VisitDurationGroupName"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #txtName").val();
            myJson["Entity"] = $('#ddlEntity').val();
            $("#pnlAdmin_VisitTypeDurationGroupDetail #chkActive").is(':checked') ? myJson["IsActive"] = true : myJson["IsActive"] = false;
            myJson["VisitGroupId"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #hfVisitId").val();
            var obj = JSON.stringify(myJson);

            VisitTypeDetail.VisitTypeDurationsDataSave(VisitTypeDurationGroupId);
        }
    },

    VisitTypeDurationsDataSave: function (VisitTypeGroupId) {
        VisitTypeDetail.GetVisitTypeDurationArray();

        var strMessage = "";
        if (strMessage == "") {
            if (VisitTypeDetail.params.mode == "Add") {
                var myJson = {};
                myJson["VisitDurationGroupName"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #txtName").val();
                myJson["Entity"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #ddlEntity").val();
                myJson["VisitTypeDurations"] = VisitTypeDetail.arrOfObj;
                
                
                $("#pnlAdmin_VisitTypeDurationGroupDetail #chkActive").is(':checked') ? myJson["IsActive"] = true : myJson["IsActive"] = false;
                myJson["VisitDurationId"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #hfVisitId").val();
                myJson["VisitGroupId"] = VisitTypeGroupId;
                var obj = JSON.stringify(myJson);
                $("#pnlAdmin_VisitTypeDurationGroupDetail #SaveButton").removeAttr('disabled');
                VisitTypeDetail.SaveVisitTypeDurationsData(obj).done(function (response) {
                    if (response.status == true) {
                        VisitTypeDurationGroup.VisitTypeDurationGroupSearch();
                        utility.DisplayMessages(response.message, 1);
                        VisitTypeDetail.UnloadTab();
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }
            else if (VisitTypeDetail.params.mode == "Edit") {
                var retVal = VisitTypeDetail.ValidateDurations();
                var myJson = {};
                myJson["VisitDurationGroupName"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #txtName").val();
                myJson["VisitTypeDurations"] = VisitTypeDetail.arrOfObj;
               

                myJson["Entity"] = $("#pnlAdmin_VisitTypeDurationGroupDetail #ddlEntity").val();
                $("#pnlAdmin_VisitTypeDurationGroupDetail #chkActive").is(':checked') ? myJson["IsActive"] = true : myJson["IsActive"] = false;
                myJson["VisitGroupId"] = VisitTypeDetail.params.VisitTypeDurationGroupId;
                var obj = JSON.stringify(myJson);

                AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        $("#pnlAdmin_VisitTypeDurationGroupDetail #SaveButton").removeAttr('disabled');
                        if (retVal == true) {
                        }
                        else {
                            
                            VisitTypeDetail.UpdateVisitTypeData(obj, VisitTypeDetail.params.VisitTypeDurationGroupId).done(function (response) {
                                if (response.status == true) {
                                    VisitTypeDurationGroup.VisitTypeDurationGroupSearch(response.VisitTypeDurationGroupId);
                                    utility.DisplayMessages(response.message, 1);
                                    VisitTypeDetail.UnloadTab();
                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                                VisitTypeDurationGroup.VisitTypeDurationGroupSearch();
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }

        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    SaveVisitTypeDurationsData: function (VisitTypeDuartionsData) {
        var data = "VisitTypeDurationsData=" + VisitTypeDuartionsData;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP_DETAIL", "SAVE_VISIT_TYPE_DURATIONS");
    },

    SaveVisitGroupData: function (VisitTypeDurationGroupData) {
        var data = "VisitTypeDurationGroupData=" + VisitTypeDurationGroupData;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP_DETAIL", "SAVE_VISIT_TYPE_DURATION_GROUP");
    },

    UpdateVisitTypeData: function (VisitTypeDurationGroupData, VisitTypeDurationGroupId, IsActive) {
        var data = "VisitTypeDurationGroupData=" + VisitTypeDurationGroupData + "&VisitTypeDurationGroupId=" + VisitTypeDurationGroupId + "&IsActive" + IsActive;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP_DETAIL", "UPDATE_VISIT_TYPE_DURATION_GROUP");
    },

    FillVisitTypeForm: function (VisitTypeDurationGroupId) {
        var data = "VisitTypeDurationGroupId=" + VisitTypeDurationGroupId;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP_DETAIL", "FILL_VISIT_TYPE_DURATION_GROUP");
    },

    UnloadTab: function (caller) {
        VisitTypeDetail.arrOfObj = [];
       
        UnloadActionPan(VisitTypeDetail.params["ParentCtrl"], "VisitTypeDetail");
    },

    ShowHistory: function () {
        var PanelID = 'VisitTypeDetail';
        var ParentCtrl = 'VisitTypeDetail';
        var ProfileName = 'VisitTypeDurationGroup';
        var DBTableName = 'VisitTypeDurationGroup';
        var ColumnKeyId = VisitTypeDetail.params.VisitTypeDurationGroupId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}