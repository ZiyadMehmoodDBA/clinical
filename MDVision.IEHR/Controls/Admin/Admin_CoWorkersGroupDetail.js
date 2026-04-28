Admin_CoWorkersGroupDetail = {
    bIsFirstLoad: true,
    Load: function (params) {
        Admin_CoWorkersGroupDetail.params = params;
        var self = null;
        if (Admin_CoWorkersGroupDetail.params.mode == "Edit") {
            $('#pnlAdmin_CoWorkersGroupDetail #headingTitle').text('Edit Co-Workers Group');
        } else {
            $('#pnlAdmin_CoWorkersGroupDetail #headingTitle').text('Add Co-Workers Group');
        }
        if (Admin_CoWorkersGroupDetail.params.PanelID == "pnlAdmin_CoWorkersGroupDetail")
            self = $('#pnlAdmin_CoWorkersGroupDetail');
        else
            self = $('#' + Admin_CoWorkersGroupDetail.params.PanelID + ' #pnlAdmin_CoWorkersGroupDetail');
        if (Admin_CoWorkersGroupDetail.bIsFirstLoad) {
            Admin_CoWorkersGroupDetail.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                $('#multiselect').multiselectlist();
                Admin_CoWorkersGroupDetail.LoadCoWorkersGroup();
                Admin_CoWorkersGroupDetail.validateAdminCoWorkersGroupName();
                if (Admin_CoWorkersGroupDetail.params.mode == "Add") {
                    $('#frmAdminCoWorkersGroupDetail').data('serialize', $('#frmAdminCoWorkersGroupDetail').serialize());
                }

            });
        }
    },
    LoadCoWorkersGroup: function () {
        if (Admin_CoWorkersGroupDetail.params.mode == "Edit") {
            Admin_CoWorkersGroupDetail.FillCoWorkerGroup(Admin_CoWorkersGroupDetail.params.CoWorkersGroupID).done(function (response) {
                if (response.status != false) {
                    var CoWorker_detail = JSON.parse(response.CoWorkerGroup_JSON);
                    var self = $("#pnlAdmin_CoWorkersGroupDetail");
                    $("#pnlAdmin_CoWorkersGroupDetail #txtName").val(CoWorker_detail.Name);
                    if (CoWorker_detail.chkIsActive == 'True')
                        $("#pnlAdmin_CoWorkersGroupDetail #chkIsActive").attr("checked", true);
                    else
                        $("#pnlAdmin_CoWorkersGroupDetail #chkIsActive").attr("checked", false);
                    var arrUserIds = CoWorker_detail.UserID.split(',');
                    var arrUserNames = CoWorker_detail.usernames.split('-');
                    $.each(arrUserIds, function (i, value) {
                        $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail #multiselect_to").append("<option value=\'" + arrUserIds[i] + "'\>" + arrUserNames[i] + "</option>");
                        $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail #multiselect option[value='" + arrUserIds[i] + "']").remove();
                    });
                    $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #hfCoWorkersGroupId").val(CoWorker_detail.CoWorkersGroupId);
                    //specialtyDetail.ValidateSpecialty();
                    $('#frmAdminCoWorkersGroupDetail').data('serialize', $('#frmAdminCoWorkersGroupDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    FillCoWorkerGroup: function (CoWorkersGroupID) {
        var data = "CoWorkersGroupID=" + CoWorkersGroupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_COWORKERGROUP", "FILL_COWORKERSGROUP");
    },
    save: function () {
        var strMessage = "";
        if ($("#" + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail #txtName").val() != "") {
            var Id = $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #hfCoWorkersGroupId").val() != "" ? $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #hfCoWorkersGroupId").val() : "-1";
            if (parseInt(Id) > 0) {
                Admin_CoWorkersGroupDetail.params.mode = "Edit";
            }
            else {
                Admin_CoWorkersGroupDetail.params.mode = "Add";
            }
            var self = $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail");
            var myJSON = self != null ? self.getMyJSON() : "{}";
            var objData = JSON.parse(myJSON);
            var userIds = self.find($('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail #multiselect_to option")).map(function () {
                return this.value;
            }).get().join(',');
            objData["UserIDs"] = userIds;
            myJSON = JSON.stringify(objData);
            if (Admin_CoWorkersGroupDetail.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Co-workers Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_CoWorkersGroupDetail.SaveCoWorkersGroup(myJSON).done(function (response) {
                            if (response.status != false) {
                                Admin_CoWorkersGroupDetail.params.mode = "Edit";
                                Admin_CoWorkersGroup.CoWorkersGroupSearch(response.CoWorkersGroupId);
                                utility.DisplayMessages(response.message, 1);
                                UnloadActionPan(Admin_CoWorkersGroupDetail.params["ParentCtrl"]);
                                //Admin_CoWorkersGroupDetail.LoadUser();
                                //Admin_User.UserSearch(userID);
                                //Admin_CoWorkersGroupDetail.UnLoad();
                            }
                            else {

                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(strMessage, 2);
                    }
                });
            }
            else if (Admin_CoWorkersGroupDetail.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Co-workers Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_CoWorkersGroupDetail.UpdateCoWorkersGroup(myJSON).done(function (response) {
                            if (response.status != false) {
                                $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #hfCoWorkersGroupId").val("");
                                UnloadActionPan(Admin_CoWorkersGroupDetail.params["ParentCtrl"]);
                                utility.DisplayMessages("Successfully Updated", 1);
                                //Clinical_CDS.CDSSearch(null, null, null, null, null, "loadCurrentCDS");
                                //Admin_CoWorkersGroupDetail.UnLoad('saveExit');
                                $('#frmAdminCoWorkersGroupDetail').data('serialize', $('#frmAdminCoWorkersGroupDetail').serialize());
                                Admin_CoWorkersGroup.CoWorkersGroupSearch(Id);
                                Admin_CoWorkersGroupDetail.UnLoad();
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else {
                         utility.DisplayMessages(strMessage, 2);
                    }
                });
            }
        }
    },
    validateAdminCoWorkersGroupName: function () {
        var self = '#' + Admin_CoWorkersGroupDetail.params.PanelID + ' #frmAdminCoWorkersGroupDetail';
        $(self).bootstrapValidator('destroy');
        $(self)
          .bootstrapValidator({
              live: 'disabled',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  grpName: {
                      group: '.col-sm-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           if (e.type == "success") {
               Admin_CoWorkersGroupDetail.save();
           }
       });
        //.on('error.form.bv', function(e) {
        //    e.preventDefault();
        //    $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail #multiselect_to").multiselect("clearSelection");
        //    $('#' + Admin_CoWorkersGroupDetail.params.PanelID + " #frmAdminCoWorkersGroupDetail #multiselect").multiselect("clearSelection");
        //})
    },
    SaveCoWorkersGroup: function (UserData) {
        var data = "Data=" + UserData;

        return MDVisionService.defaultService(data, "ADMIN_COWORKERGROUP_DETAIL", "SAVE_SAVECOWORKERSGROUP");
    },
    UpdateCoWorkersGroup: function (UserData) {
        var data = "Data=" + UserData;
        return MDVisionService.defaultService(data, "ADMIN_COWORKERGROUP_DETAIL", "UPDATE_COWORKERSGROUP");
    },
    UnLoad: function () {
        utility.UnLoadDialog("frmAdminCoWorkersGroupDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },
    ClearSelection: function (obj) {
        $("#pnlAdmin_CoWorkersGroupDetail " + obj).find('option').prop('selected', false);
    },
    ShowHistory: function () {
        var PanelID = 'pnlAdminCoWorkersGroup';
        var ParentCtrl = 'Admin_CoWorkersGroupDetail';
        var ProfileName = 'Co-workers Group';
        var DBTableName = 'group';
        var ColumnKeyId = Admin_CoWorkersGroupDetail.params.CoWorkersGroupID;
        var FromAdmin = "1";

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
        //    .done(function (response) {
        //});
        
    },
}