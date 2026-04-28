iTrack_AdminPreferenceGroupReportingDetail = {
    bIsFirstLoad: true,
    params: [],
    measureGroupTable: null,
    Load: function (params) {
        iTrack_AdminPreferenceGroupReportingDetail.params = params;
        iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable = null;
        if (iTrack_AdminPreferenceGroupReportingDetail.params.PanelID != 'pnlMIPSAdminPreferenceGroupReportingDetail') {
            iTrack_AdminPreferenceGroupReportingDetail.params.PanelID = iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #pnlMIPSAdminPreferenceGroupReportingDetail';
        } else {
            iTrack_AdminPreferenceGroupReportingDetail.params.PanelID = 'pnlMIPSAdminPreferenceGroupReportingDetail';
        }
        
       if( iTrack_AdminPreferenceGroupReportingDetail.params.DropDownMeasureGroupName !=null   )   iTrack_AdminPreferenceGroupReportingDetail.params.DropDownMeasureGroupName=iTrack_AdminPreferenceGroupReportingDetail.params.DropDownMeasureGroupName.replace('%20', ' ');



        if (iTrack_AdminPreferenceGroupReportingDetail.bIsFirstLoad) {
            iTrack_AdminPreferenceGroupReportingDetail.bIsFirstLoad = false;
            var self = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID);

            //$('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' select[multiple]').multiselect({
            //    includeSelectAllOption: true,
            //    enableFiltering: true,
            //    enableCaseInsensitiveFiltering: true
            //});
            EMRUtility.CreateYearViewDatePicker(iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #dtpPerformanceYear',
               //on-change callback method 
               function (ev) {
                   if ($('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #frmiTrackAdminPreferenceGroupReportingDetail').data("bootstrapValidator") != null) {
                       $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #frmiTrackAdminPreferenceGroupReportingDetail').bootstrapValidator('revalidateField', 'PerformanceYear');
                   }
               }, true);
        }

        var self = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID);
        iTrack_AdminPreferenceGroupReportingDetail.measuresSearch();
        self.loadDropDowns(true).done(function () {
         
            iTrack_AdminPreferenceGroupReportingDetail.LoadGroupLookup();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonTitle: function (options, select) {
                    var buttonTitle = "";
                    $.each(options, function (i, item) {
                        if (buttonTitle != "") {
                            buttonTitle += "," + $(item).attr("refvalue");
                        }
                        else {
                            buttonTitle += $(item).attr("refvalue");
                        }

                    });

                    return buttonTitle;
                }
            });
            iTrack_AdminPreferenceGroupReportingDetail.resetMeasureGroup(true);
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #txtTIN').val("");
            if (iTrack_AdminPreferenceGroupReportingDetail.params["mode"] == 'Edit' && iTrack_AdminPreferenceGroupReportingDetail.params["MeasureGroupId"] != null) {
                iTrack_AdminPreferenceGroupReportingDetail.fillMeasureGroups(iTrack_AdminPreferenceGroupReportingDetail.params["MeasureGroupId"]);
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #headerId').text('Edit Measure');
            } else {
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #headerId').text('Add Measure');
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #txtTIN').val("");
            }
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #EntityID').val(100);
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').val(1);
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').multiselect("refresh");
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #PracticeDivReports').css('pointer-events', 'none');
            //   iTrack_AdminPreferenceGroupReportingDetail.GroupReportingSearch();
        });


    },
    LoadGroupLookup: function () {
        iTrack_AdminIPPreference.LoadGroupLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $.each(list, function (i, item) {
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] +' #ddlGroupName').append('<option value=' + item.GroupId + ' >' + item.GroupName + '</option>')
                });




            }
            else {

            }

        });

    },
    loadGroupData: function () {

        iTrack_AdminPreferenceGroupReportingDetail.loadGroupData_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var dta = JSON.parse(response.IndividualProCountLoad_JSON)
                if (dta.Groups && dta.Groups.length > 0) {

                    var tin = dta.Groups[0].TIN;
                    var comments = dta.Groups[0].JoiningComments;
                    var joiningDate = dta.Groups[0].JoiningDate;
                    joiningDate = new Date(joiningDate);
                    var leavingDate = dta.Groups[0].LeavingDate;
                    leavingDate = new Date(leavingDate);
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #txtTIN').val(tin);
                   

                }
                var members = null;
                if (dta.GroupDetail && dta.GroupDetail.length > 0) {
                    $.each(dta.GroupDetail, function (i, item) {

                        members += "," + item.ProviderId;


                    });
                    var memberslist = members.split(',');
                    memberslist.splice($.inArray('null', memberslist), 1);
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').val(memberslist);
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').multiselect("refresh");

                }
                else {
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #txtTIN').val(tin);
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').val(memberslist);
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').multiselect("refresh");
                }
            }
          
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' .dropdown-menu li').each(function () {
                if ($(this).hasClass("active"))
                    $(this).show();
                else
                    $(this).hide();
            });

            if (iTrack_AdminPreferenceGroupReportingDetail.params.SelectedProvider) {
                var memberslist = iTrack_AdminPreferenceGroupReportingDetail.params.SelectedProvider.split(',');
                var members = "";
                $.each(iTrack_AdminPreferenceGroupReportingDetail.params.SelectedProvider, function (i, item) {

                    members += "," + item.ProviderId;
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').append($('<option>', {
                        refvalue: item.ProviderName,
                        value: item.ProviderId,
                        text: item.ProviderName
                    }));

                });
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').val(members.split(','));
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').multiselect("refresh");
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlMemberProvider').multiselect('rebuild');
                iTrack_AdminPreferenceGroupReportingDetail.params.SelectedProvider = null;
            }

        });
    },

    LoadGroupLookup_DBCall: function () {
        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["commandType"] = "loadgroupnamelookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },

    loadGroupData_DBCall: function () {
        var objData = new Object();
        objData["GroupName"] =  $('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' #ddlGroupName option:selected').text();
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ReportingMethod"] = "MD Vision EHR";
        
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;
        objData["commandType"] = "searchmimpsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    fillMeasureGroups: function (MeasureGroupId) {
        if (iTrack_AdminPreferenceGroupReportingDetail.params.mode == "Edit") {
            iTrack_AdminPreferenceGroupReportingDetail.fillMeasureGroup_DBCall(MeasureGroupId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var self = $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID);
                    utility.bindMyJSONByName(true, JSON.parse(response.measureIndividualList_JSON)[0], false, self);
                    var data = JSON.parse(response.measureIndividualList_JSON);
                    iTrack_AdminPreferenceGroupReportingDetail.params.SelectedProvider = data[0].ProviderId;
                    if (data[0].PracticeIds) {
                        var PracticeIds = data[0].PracticeIds.split(",");
                        $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').val(PracticeIds);
                  
                        
                        $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').attr('disabled', 'disabled');
                        $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').multiselect("refresh");
                    }

                    
                   
                    iTrack_AdminPreferenceGroupReportingDetail.loadGroupData();
                    $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #EntityID').val(100);
                    $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlGroupName').val($("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #ddlGroupName" + " option").filter(function () { return this.text == iTrack_AdminPreferenceGroupReportingDetail.params.DropDownMeasureGroupName }).val());
                    if (JSON.parse(response.measureIndividualList_JSON)[0]['IsActive'] == true)
                        $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #Active').attr("checked", true);
                    else
                        $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #Active').attr("checked", false);

                    //$('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect('clearSelection', false);
                    //$('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect('updateButtonText');
                    //// Set the value
                    //$('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #ddlPractice").val(JSON.parse(response.measureIndividualList_JSON)[0]['PracticeIds'].split(','));
                    //// Then refresh
                    //$('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect("refresh");
                    //litabPQRSIndividualMeasures
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #tabCQMIndividualMeasures').trigger('onclick');
                    iTrack_AdminPreferenceGroupReportingDetail.selectMeasures(JSON.parse(response.measureIndividualList_JSON)[0]['MeasureIds'], JSON.parse(response.measureIndividualList_JSON)[0]['CQMMeasureIds'], JSON.parse(response.measureIndividualList_JSON)[0]['IAMeasureIds']);
                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    setGrid: function (gridType) {
         if (gridType == 'CQM') {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMIndividualMeasures').css("display", "");
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPIndividualMeasures').css("display", "none");
        }
        else if (gridType == 'VBP') {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPIndividualMeasures').css("display", "");
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMIndividualMeasures').css("display", "none");
        }
    },
    resetMeasureGroup: function (firstLoad) {
        if (firstLoad) {

           // $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').multiselect("destroy");

            //$('#' + iTrack_AdminPreferenceGroupReportingDetail.params["PanelID"] + ' select[multiple]').multiselect({
            //    includeSelectAllOption: true,
            //    enableFiltering: true,
            //    enableCaseInsensitiveFiltering: true
            //});
        }
        //$("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlGroupName').val('');
        //iTrack_AdminPreferenceGroupReportingDetail.loadGroupData();


        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' select').each(function () { $(this).val('') });
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').multiselect('clearSelection', false);
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').multiselect('updateButtonText');
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #ddlPractice').multiselect("refresh");
        $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #Active').attr("checked", true);
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvMeasures #selectAllMeasures').attr("checked", false);
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val('');
        $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dtpSubmissionYear').datepicker('setDate', new Date())

        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceGroupReportingDetail select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });

        //CQM Group Measures
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures #selectAllMeasures').attr("checked", false);
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val('');

        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail #CQMIndividualMeasures select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });

        iTrack_AdminPreferenceGroupReportingDetail.ValidateMeasureGroup();


    },
    multiselect_Validator: function (cntrl) {
        if ($(cntrl).val() == null || $(cntrl).val() == "") {
            $(cntrl).closest('div').find('label.control-label').addClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '#cc2724');
        } else {
            $(cntrl).closest('div').find('label.control-label').removeClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '');
        }
    },

    //Binding Validation Functionk
    ValidateMeasureGroup: function () {
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail').bootstrapValidator('destroy');
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  SubmissionYear: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  PracticeIds: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ProviderId: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  SpecialityId: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }

              }
          }).on('error.form.bv', function (e) {
              // Prevent form submission
              e.preventDefault();
              $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail select[multiple]').each(function () {
                  if ($(this).val() == null || $(this).val() == "") {
                      $(this).closest('div').find('label.control-label').addClass('has-error');
                      $(this).closest('div').find('label.control-label').css('color', '#cc2724');

                  } else {
                      $(this).closest('div').find('label.control-label').removeClass('has-error');
                      $(this).closest('div').find('label.control-label').css('color', '');

                  }
              });
          })
       .on('success.form.bv', function (e) {
           var hasError = false;
           $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail select[multiple]').each(function () {
               if ($(this).val() == null || $(this).val() == "") {
                   $(this).closest('div').find('label.control-label').addClass('has-error');
                   $(this).closest('div').find('label.control-label').css('color', '#cc2724');
                   hasError = true;
               } else {
                   $(this).closest('div').find('label.control-label').removeClass('has-error');
                   $(this).closest('div').find('label.control-label').css('color', '');

               }
           });
           e.preventDefault();
           if (!hasError) {
               iTrack_AdminPreferenceGroupReportingDetail.saveMeasureIndividual();
           }
       });
    },
    saveMeasureIndividual: function () {



        var cqm = [];
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMIndividualMeasures input[type=checkbox]:checked').each(function (item) {
            if (this.id == "selectAllMeasures") {
                return;
            }
                var index = cqm.indexOf(this.id);
                if (index < 0) {
                    cqm.push(this.id);
                }
          
        });

        var ia = [];
        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPIndividualMeasures input[type=checkbox]:checked').each(function (item) {
            if (this.id == "selectAllMeasures") {
                return;
            }
                var index = ia.indexOf(this.id);
                if (index < 0) {
                    ia.push(this.id);
                }
         
        });

        var IAIDs = ia.join(',');
        var CQMIDs = cqm.join(',');




        var PQRSMeasureIds = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val();
        var CQMMeasureIds = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val();
        if (PQRSMeasureIds != "" && CQMIDs != "") {
            PQRSMeasureIds = PQRSMeasureIds.concat(',' + CQMIDs)
        }
        else if (CQMIDs != "") {
            PQRSMeasureIds = CQMIDs
        }

        var VBPMeasureIds = $($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds')[0]).val();
        if (PQRSMeasureIds != "" && IAIDs != "") {
            PQRSMeasureIds = PQRSMeasureIds.concat(',' + IAIDs)
        }
        else if (IAIDs != "") {
            PQRSMeasureIds = IAIDs
        }

        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(PQRSMeasureIds);
        var self = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();

        if (iTrack_AdminPreferenceGroupReportingDetail.params.mode == "Add") {
            iTrack_AdminPreferenceGroupReportingDetail.saveMeasureGroup_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    iTrack_AdminPreferenceGroupReporting.measureGroupSearch();
                    iTrack_AdminPreferenceGroupReportingDetail.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
        else if (iTrack_AdminPreferenceGroupReportingDetail.params.mode == "Edit") {

            var myJSON = self.getMyJSONByName();
            iTrack_AdminPreferenceGroupReportingDetail.saveMeasureGroup_DbCall(myJSON, iTrack_AdminPreferenceGroupReportingDetail.params.MeasureGroupId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    
                    iTrack_AdminPreferenceGroupReportingDetail.UnLoadTab();
                    iTrack_AdminPreferenceGroupReporting.measureGroupSearch();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
    },
    saveMeasureGroup_DbCall: function (MeasureGroupData, MeasureGroupId) {
        var objData = JSON.parse(MeasureGroupData);
        if (MeasureGroupId == null) {
            objData["commandType"] = "save_pqrs_measuregroup";
        } else {
            objData["MeasureGroupId"] = iTrack_AdminPreferenceGroupReportingDetail.params.MeasureGroupId;
            objData["commandType"] = "update_pqrs_measuregroup";

        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvMeasures")) {
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
        }

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
        }

        iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable = null;
        if (iTrack_AdminPreferenceGroupReportingDetail.params["FromAdmin"] == "0") {
            if (iTrack_AdminPreferenceGroupReportingDetail.params != null && iTrack_AdminPreferenceGroupReportingDetail.params.ParentCtrl != null) {
                UnloadActionPan(iTrack_AdminPreferenceGroupReportingDetail.params.ParentCtrl, 'iTrack_AdminPreferenceGroupReportingDetail');
            }
            else
                UnloadActionPan(null, 'iTrack_AdminPreferenceGroupReportingDetail');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    selectMeasures: function (measureIds, CQMMeasureIds, IAIds) {
        if (measureIds != null & measureIds != '') {
            
            setTimeout(function () {
                $.each(measureIds.split(','), function (index, item) {
                    var gridId = 'dgvMeasures';
                    $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
                });
            
                iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvMeasures');
            }, 2000);
        }

        if (CQMMeasureIds != null & CQMMeasureIds != '') {
            
            setTimeout(function () {
                $.each(CQMMeasureIds.split(','), function (index, item) {
                var gridId = 'dgvCQMMeasures';
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
            });
          
            iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvCQMMeasures');
            }, 2000);
        }

        if (IAIds != null & IAIds != '') {
            
            setTimeout(function () {
                $.each(IAIds.split(','), function (index, item) {
                var gridId = 'dgvVBPMeasures';
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
            });
           
            iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvVBPMeasures');
            }, 2000);
        }

    },
    checkAllMeasureSelected: function (gridId) {
        if (!(gridId != null && gridId != "")) {
            gridId = 'dgvMeasures';
            if ($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #litabCQMIndividualMeasures").hasClass('active'))
                gridId = 'dgvCQMMeasures';
            else if ($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #litabVBPIndividualMeasures").hasClass('active'))
                gridId = 'dgvVBPMeasures';
        }

        var curPageChkbx = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' tbody input[type=checkbox]').length;
        var curPageChkdChkbx = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' tbody input[name="SelectCheckBoxMeasure"]:checked').length;
        if (curPageChkdChkbx == curPageChkbx && curPageChkbx > 0) {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', true);
        } else {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', false);
        }
    },
    ////****************************************************
    ////************ Measures*******************************
    ////****************************************************
    selectAllMeasures: function (cntrl, gridId) {
        if (gridId != null && gridId != '') {

        }
        else {
            gridId = 'dgvMeasures';
        }
        if ($(cntrl).is(':checked')) {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', true);

        } else {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', false);

        }
    },
    measuresSearch: function (measuresId) {
        PQRS_MeasureGroups_Detail.SearchMeasures_DBCall(measuresId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //iTrack_AdminPreferenceIndividualReportingDetail.MeasuresLoad(response);
                iTrack_AdminPreferenceGroupReportingDetail.CQMMeasuresLoad(response);
                iTrack_AdminPreferenceGroupReportingDetail.VBPMeasuresLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    MeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures")) {
            $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
        }

        if (response.PQRSmeasureCount > 0) {
            var MeasureJSONData = JSON.parse(response.PQRSmeasureList_JSON);
            $.each(MeasureJSONData, function (i, item) {
                //if (item.MeasureType != "PQRS")
                //    return;
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.MeasureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureId);
                $row.attr("MeasureGroupsId", item.MeasureId);
                var onclick = 'onclick="iTrack_AdminPreferenceIndividualReportingDetail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + '</td><td>' + item.NQSDomain + '</td><td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures').DataTable({
                "language": {
                    "emptyTable": "No Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures'))
            ;
        else {
            var headercount = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures  thead th').length - 1) > index) {
                        var title = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            iTrack_AdminPreferenceIndividualReportingDetail.measureGroupTable = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvMeasures').DataTable({ "bInfo": true, "searching":false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container()).on('keyup', 'tfoot input', function () {
                    iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvMeasures');
                });
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container).on('page.dt', function () {
                    if (iTrack_AdminPreferenceGroupReportingDetail.params.mode == "Edit") {
                        setTimeout(function () {
                            iTrack_AdminPreferenceGroupReportingDetail.selectMeasures($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(), $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container).on('change', '#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                    } else if (measureIds != null & measureIds != '') {
                        if (ArrMeasureIds != null && this.id != null) {
                            var index = ArrMeasureIds.indexOf(this.id);
                            if (isChecked) {
                                if (index < 0) {
                                    ArrMeasureIds.push(this.id);
                                }
                            } else {
                                if (index >= 0) {
                                    ArrMeasureIds.splice(index, 1);
                                }
                            }
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(this.id);
                        }
                    }
                    iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvMeasures');
                });
            }
        }

    },

    CQMMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
        }

        if (response.CQMmeasureCount > 0) {
            var MeasureJSONData = JSON.parse(response.CQMmeasureList_JSON);
            $.each(MeasureJSONData, function (i, item) {
                //if (item.MeasureType != "CQM")
                //    return;
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.MeasureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureId);
                $row.attr("MeasureGroupsId", item.MeasureId);
                var onclick = 'onclick="iTrack_AdminPreferenceGroupReportingDetail.showMeasureDocumentByMeasureNumber(\'' + item.MeasureNumber + '\');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                var highPriority = "No";
                if (item.ActivityWeighting == "High") {
                    highPriority = "Yes";
                }
                var measureType = "Process";
                if (item.MeasureNumber == "CMS122v5" || item.MeasureNumber == "CMS165v6") {
                    measureType = "Intermediate Outcome";
                }
                var infoIcon = '<a data-toggle="tooltip" title="" data-original-title="Test tooltip" ' + onclick + ' href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs pull-right" style="border-radius: 0px;"></i></a>';
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' + item.MeasureTitle + infoIcon + '</td><td>' + item.NQSDomain + '</td><td>' + measureType + '</td><td>' + highPriority + '</td>');
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures').DataTable({
                "language": {
                    "emptyTable": "No CQM Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures'))
            ;
        else {
            var headercount = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures  thead th').length - 1) > index) {
                        var title = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures').DataTable({ "bInfo": true, "searching": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container()).on('keyup', 'tfoot input', function () {
                    iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvCQMMeasures');
                });
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container).on('page.dt', function () {
                    if (iTrack_AdminPreferenceGroupReportingDetail.params.mode == "Edit") {
                        setTimeout(function () {
                            iTrack_AdminPreferenceGroupReportingDetail.selectMeasures($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(), $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container).on('change', '#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val(ArrMeasureIds.join(','));
                    } else if (measureIds != null & measureIds != '') {
                        if (ArrMeasureIds != null && this.id != null) {
                            var index = ArrMeasureIds.indexOf(this.id);
                            if (isChecked) {
                                if (index < 0) {
                                    ArrMeasureIds.push(this.id);
                                }
                            } else {
                                if (index >= 0) {
                                    ArrMeasureIds.splice(index, 1);
                                }
                            }
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #CQMMeasureIds').val(this.id);
                        }
                    }
                    iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvCQMMeasures');
                });
            }
        }

    },

    VBPMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvVBPMeasures")) {
            $("#" + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvVBPMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvVBPMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + " #dgvVBPMeasures tbody").find("tr").remove();
        }

        if (response.VBPmeasureCount > 0) {
            var MeasureJSONData = JSON.parse(response.IAmeasureList_JSON);
            $.each(MeasureJSONData, function (i, item) {
                //if (item.MeasureType != "VBP")
                //    return;
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.MeasureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureId);
                $row.attr("MeasureGroupsId", item.MeasureId);
                var onclick = 'onclick="iTrack_AdminPreferenceGroupReportingDetail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                //if (item.DocumentName == null || item.DocumentName == '') {
                //    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                //    title = " title='No Document Found'";
                //}
                var weightage = "10";
                if (item.NQSDomain == "High")
                    weightage = "20";
                var infoIcon = '<a data-toggle="tooltip" title="" data-original-title="Test tooltip" onclick="" href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs" style="border-radius: 0px;"></i></a>';
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber +  '</td><td>' +
                   item.MeasureTitle + infoIcon + '</td><td>' + item.NQSDomain + '</td><td>' + weightage + '</td>');//<td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures').DataTable({
                "language": {
                    "emptyTable": "No VBP Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures'))
            ;
        else {
            var headercount = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures  thead th').length - 1) > index) {
                        var title = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures').DataTable({ "bInfo": true, "searching": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container()).on('keyup', 'tfoot input', function () {
                    iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvVBPMeasures');
                });
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container).on('page.dt', function () {
                    if (iTrack_AdminPreferenceGroupReportingDetail.params.mode == "Edit") {
                        setTimeout(function () {
                            iTrack_AdminPreferenceGroupReportingDetail.selectMeasures($('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #MeasureIds').val(), $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds').val());
                        }, 10);
                    }
                });
                $(iTrack_AdminPreferenceGroupReportingDetail.measureGroupTable.table().container).on('change', '#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds').val(ArrMeasureIds.join(','));
                    } else if (measureIds != null & measureIds != '') {
                        if (ArrMeasureIds != null && this.id != null) {
                            var index = ArrMeasureIds.indexOf(this.id);
                            if (isChecked) {
                                if (index < 0) {
                                    ArrMeasureIds.push(this.id);
                                }
                            } else {
                                if (index >= 0) {
                                    ArrMeasureIds.splice(index, 1);
                                }
                            }
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceGroupReportingDetail.params.PanelID + ' #VBPMeasureIds').val(this.id);
                        }
                    }
                    iTrack_AdminPreferenceGroupReportingDetail.checkAllMeasureSelected('dgvVBPMeasures');
                });
            }
        }

    },
    fillMeasureGroup_DBCall: function (MeasureGroupId) {
        var objData = {};
        objData["MeasureGroupId"] = MeasureGroupId;
        objData["commandType"] = "fill_pqrs_measuregroupdetails";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },


    showMeasureDocument: function (MeasureId) {

        var params = [];
        params["MeasureId"] = MeasureId;
        params["viewMode"] = "Measures";
        params["ParentCtrl"] = "iTrack_AdminPreferenceIndividualReportingDetail";
        params["FromAdmin"] = 0;
        LoadActionPan("PQRS_CMSView", params);
    },

    showMeasureDocumentByMeasureNumber: function (MeasureNumber) {

        var params = [];
        params["MeasureNumber"] = MeasureNumber + ".pdf";
        params["viewMode"] = "MeasuresByMeasureNumber";
        params["ParentCtrl"] = "iTrack_AdminPreferenceGroupReportingDetail";
        params["FromAdmin"] = 0;
        LoadActionPan("PQRS_CMSView", params);
    },
}