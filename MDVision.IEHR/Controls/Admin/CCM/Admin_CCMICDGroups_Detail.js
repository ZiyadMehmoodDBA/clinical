Admin_CCMICDGroups_Detail = {
    params: [], ICDArrays: null,
    bIsFirstLoad: true,

    Load: function (params) {
        Admin_CCMICDGroups_Detail.params = params;
        var self = $('#CCMICDGroupsDetail #frmCCMICDGroupsDetail');
        Admin_CCMICDGroups_Detail.ICDArrays = [];
        $("#hftxtICDGroupId").val(Admin_CCMICDGroups_Detail.params.CCMICDGroupID);
        
        $(function () {
            $('#CCMICDGroupsDetail #txtCCMProblems').keypress(function (e) {
                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                if (keycode == 13) {
                    e.preventDefault();
                }
            });
        });


        if (Admin_CCMICDGroups_Detail.bIsFirstLoad)
        {
            $("#txtCCMProblems").attr('disabled', true);
            Admin_CCMICDGroups_Detail.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                $('#frmCCMICDGroupsDetail').data('serialize', $('#frmCCMICDGroupsDetail').serialize());
                //facilityDetail.LoadFacility();
                Admin_CCMICDGroups_Detail.ValidateICD();
            });
        }
        else
        {
            $("#txtCCMProblems").attr('disabled', false);
        }

        if (Admin_CCMICDGroups_Detail.params.CCMICDGroupID != null)
        {
                Admin_CCMICDGroups_Detail.loadCCMICDGroupDetail(Admin_CCMICDGroups_Detail.params.CCMICDGroupID).done(function (response) {
                Admin_CCMICDGroups_Detail.ICDGroupEdit(response);
                $("#txtCCMProblems").attr('disabled', false);
            });
        }
    },

    ICDGroupEdit: function (response)
    {
        if (response.status != false)
        {

            if (response.Code) {
                var objectCCMICDGroupDetailFill_JSON = JSON.parse(response.CCMICDGroupDetailFill_JSON);
                Admin_CCMICDGroups_Detail.AddItemToGrid(objectCCMICDGroupDetailFill_JSON);
                $('#txtICDGroupName').attr("disabled", "disabled");
                $("#CCMICDGroupsDetail #txtICDGroupName").val(response.ICDGroupShortName);
                $("#CCMICDGroupsDetail #txtICDGroupDescription").val(response.ICDGroupDescription);
                

            }
            else {
                $('#txtICDGroupName').attr("disabled", "disabled");
                $("#CCMICDGroupsDetail #txtICDGroupName").val(response.ICDGroupShortName);
                $("#CCMICDGroupsDetail #txtICDGroupDescription").val(response.ICDGroupDescription);
                $("#txtCCMProblems").attr('disabled', false);
                $("#dgvCCMICDGroupsDetail tbody tr").remove();
                $("#dgvCCMICDGroupsDetail div").remove();
            }
            if (response.ICDGroupisActive == false) {
                $('#tblCCMICDGroupsDetail #chkActive').attr('checked', false);
            }
            else {
                $('#tblCCMICDGroupsDetail #chkActive').attr('checked', true);
              }
        }
        else {
            $('#txtICDGroupName').attr("disabled", "disabled");
            $("#CCMICDGroupsDetail #txtICDGroupName").val(response.ICDGroupShortName);
            $("#CCMICDGroupsDetail #txtICDGroupDescription").val(response.ICDGroupDescription);
            $("#txtCCMProblems").attr('disabled', false);
            $("#dgvCCMICDGroupsDetail tbody tr").remove();
            $("#dgvCCMICDGroupsDetail div").remove();
        }
    },

    loadCCMICDGroupDetail: function (CCMICDGroupID) {
        var data = "CCMICDGroupID=" + CCMICDGroupID;
        return MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "LOAD_CCMICDGROUPS_DETAIL");
    },

    ValidateICD: function () {
        $('#frmCCMICDGroupsDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Name: {
                       group: '.col-sm-2',
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
                Admin_CCMICDGroups_Detail.CCMICDGroupSave();
        });
    },

    CCMICDGroupSave: function () {
        var strMessage = "";
        var self = $("#CCMICDGroupsDetail #frmCCMICDGroupsDetail");
        var myJSON = self.getMyJSON();
        if (Admin_CCMICDGroups_Detail.params.mode == "Add") {
            //AppPrivileges.GetFormPrivileges("ICD", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Admin_CCMICDGroups_Detail.SaveCCMICDGroup(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        //ii++;
                        $("#hftxtICDGroupId").val(response.ICDGroupId);
                        Admin_CCMICDGroups_Detail.params.CCMICDGroupID = response.ICDGroupId;
                        $('#txtICDGroupName').attr("disabled", true);
                        $("#txtCCMProblems").attr('disabled', false);
                        Admin_CCMICDGroups.loadICDGroups();
                        //Admin_ICD.ICDSearch(response.ICDId);
                        //UnloadActionPan(Admin_CCMICDGroups_Detail.params["ParentCtrl"], "Admin_CCMICDGroups_Detail");

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            //});
        }
        else if (Admin_CCMICDGroups_Detail.params.mode == "Edit") {
            //AppPrivileges.GetFormPrivileges("ICD", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
            Admin_CCMICDGroups_Detail.UpdateCCMICDGroup(myJSON).done(function (response) {
                if (response.status != false)
                {
                    $("#hftxtICDGroupId").val(response.ICDGroupId);
                    utility.DisplayMessages("Successfully Updated", 1);
                    Admin_CCMICDGroups_Detail.params.CCMICDGroupID = response.ICDGroupId;
                    $('#txtICDGroupName').attr("disabled",true);
                    $("#txtCCMProblems").attr('disabled', false);
                    Admin_CCMICDGroups.loadICDGroups();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            }
            else
                utility.DisplayMessages(strMessage, 2);
            //});
        }
    },

    SaveCCMICDGroup: function (ICDData) {
        var data = "ICDData=" + ICDData;
        return MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "SAVE_CCMICDGROUPS");
    },

    UpdateCCMICDGroup: function (ICDData) {
        var data = "ICDData=" + ICDData;
        return MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "UPDATE_CCMICDGROUPS");
    },

    SaveCCMICDGroup_ICD: function (icd9code, icd9description, icd10code, icd10description, snomedcode, snomeddescription) {
        var data = "icd9code=" + icd9code + "&icd9description=" + icd9description + "&icd10code=" + icd10code + "&icd10description=" + icd10description
            + "&snomedcode=" + snomedcode + "&snomeddescription=" + snomeddescription + "&ICDGroupId=" + $("#hftxtICDGroupId").val();
        return MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "SAVE_CCMICD_ICDGROUPS");
    },

    BindICDNValues: function (self, ControlsArray) {
        Admin_CCMICDGroups_Detail.ICDArrays.push(ControlsArray);
        var ControlsArray1 = ControlsArray[0];
        var icd9 = "", icd9code = "", icd9description = "", icd10 = "", icd10string = "", icd10code = "", icd10description = "", snomedstring = "", snomedcode = "", snomeddescription = "";
        if (ControlsArray1) {
            icd9 = ControlsArray1.lastIndexOf('*');
            if (icd9) {
                icd9code = ControlsArray1.substring(0, icd9).split('$')[0];
                icd9description = ControlsArray1.substring(0, icd9).split('$')[1];
            }

            icd10 = ControlsArray1.split('*')[1];
            if (icd10) {
                icd10string = icd10.split('#')[0];
                if (icd10string) {
                    icd10code = icd10string.split('$')[0];
                    icd10description = icd10string.split('$')[1];
                }
            }
            if (icd10) {
                snomedstring = icd10.split('#')[1];
                if (snomedstring) {
                    snomedcode = snomedstring.split('$')[0];
                    snomeddescription = snomedstring.split('$')[1];
                }
            }

            var GridJsonObject = {
                Description: icd10description,
                Code: icd10code
            };

            var json = JSON.stringify(GridJsonObject);
            var object = JSON.parse("[" + json + "]");


            Admin_CCMICDGroups_Detail.SaveCCMICDGroup_ICD(icd9code, icd9description, icd10code, icd10description, snomedcode, snomeddescription).done(function (response) {
                if (response.status != false) 
                {
                    if (response.message == "ICD already exists")
                    {
                        utility.DisplayMessages(response.message, 1);
                        $("#txtCCMProblems").val('');
                    }
                    else
                    {
                        var icdID = response.ICDId;
                        Admin_CCMICDGroups_Detail.AddItemToGrid(object);
                        utility.DisplayMessages("Successfully Saved", 1);
                        Admin_CCMICDGroups_Detail.Load(Admin_CCMICDGroups_Detail.params);
                        $("#txtCCMProblems").val("");
                    }
                }
            });
        }
    },

    AddItemToGrid: function (data) {
        var data = new kendo.data.DataSource({
            data: data,
            pageSize: 15
        });

        $("#dgvCCMICDGroupsDetail").kendoGrid({
            dataSource: data,
            //pageSize: 15,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "There is no data on current page"
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 15,20, 50, 100],
                buttonCount: 5
            },
            columns: [
            { title: "Action", width: "20px", template: '#=Admin_CCMICDGroups_Detail.ActionCCMICDGroups(data)#' },
            { title: "Description", field: "Description", width: "170px" },
            { title: "Code", field: "Code", width: "30px" },
            ],
        });
    },

    ActionCCMICDGroups: function (data) {
        var deleteMethod = "Admin_CCMICDGroups_Detail.DeleteICD_ICDGroup('" + data.ICDGroupMapId + "');";
        return '<a class="btn  btn-xs" href="javascript:;" onclick="' + deleteMethod + '" title="Delete ICD from Group"><i class="fa fa-close red"></i></a>';
    },

    DeleteICD_ICDGroup: function (ICDGroupMapId)
    {
        utility.myConfirm("1", function () {
        var data = "ICDGroupMapId=" + ICDGroupMapId;
            MDVisionService.defaultService(data, "ADMIN_CCMICDGROUPS", "DELETE_CCMICD_ICDGROUPS")
            .done(function (response) {
                utility.DisplayMessages(response.Message, 3);
                Admin_CCMICDGroups_Detail.Load(Admin_CCMICDGroups_Detail.params);
            })
        }, function () { });

    },

    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, 100, true, -1, "ICD", true, "Admin_CCMICDGroups_Detail", null, false);
    },

    BindICD10AutoComplete: function (element) {

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, 100, true, -1, "ICD", false, "Admin_CCMICDGroups_Detail", null, false);
    },

    UnLoad: function () {

        //if (Admin_CCMICDGroups_Detail.params != null && Admin_CCMICDGroups_Detail.params.ParentCtrl != null && Admin_CCMICDGroups_Detail.params.PanelID != 'tblCCMICDGroupsDetail') {
        //    UnloadActionPan(Admin_CCMICDGroups_Detail.params.ParentCtrl, 'CCMICDGroupsDetail', null, Admin_CCMICDGroups_Detail.params.PanelID);
        //}
        //else {
        //    utility.UnLoadDialog("frmCCMICDGroupsDetail", function () {
        //        UnloadActionPan(Admin_CCMICDGroups_Detail.params["ParentCtrl"], "CCMICDGroupsDetail");
        //    }, function () {
        //        UnloadActionPan(Admin_CCMICDGroups_Detail.params["ParentCtrl"], "CCMICDGroupsDetail");
        //    });
        //}
        if (Admin_CCMICDGroups.params != null && Admin_CCMICDGroups.params.ParentCtrl) {
            UnloadActionPan(Admin_CCMICDGroups.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
        Admin_CCMICDGroups.loadICDGroups();
    },

}