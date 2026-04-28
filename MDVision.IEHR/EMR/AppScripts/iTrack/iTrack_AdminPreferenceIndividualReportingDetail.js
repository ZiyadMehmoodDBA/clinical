iTrack_AdminPreferenceIndividualReportingDetail = {
    bIsFirstLoad: true,
    params: [],
    measureIndividualTable: null,
    Load: function (params) {
        iTrack_AdminPreferenceIndividualReportingDetail.params = params;
        iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr = [];
        iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr = [];
        iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable = null;
        if (iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID != 'pnlMIPSAdminPreferenceIndividualReportingDetail') {
            iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID = iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #pnlMIPSAdminPreferenceIndividualReportingDetail';
        } else {
            iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID = 'pnlMIPSAdminPreferenceIndividualReportingDetail';
        }
        if (iTrack_AdminPreferenceIndividualReportingDetail.bIsFirstLoad) {
            iTrack_AdminPreferenceIndividualReportingDetail.bIsFirstLoad = false;
            var self = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID);

            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            EMRUtility.CreateYearViewDatePicker(iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' #dtpSubmissionYear',
               //on-change callback method 
               function (ev) {
                   if ($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' #frmiTrackAdminPreferenceIndividualReportingDetail').data("bootstrapValidator") != null) {
                       $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' #frmiTrackAdminPreferenceIndividualReportingDetail').bootstrapValidator('revalidateField', 'SubmissionYear');
                   }
               }, true);
        }

        var self = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID);
        iTrack_AdminPreferenceIndividualReportingDetail.measuresSearch();
        self.loadDropDowns(true).done(function () {
            iTrack_AdminPreferenceIndividualReportingDetail.resetMeasureIndividual(true);
            if (iTrack_AdminPreferenceIndividualReportingDetail.params["mode"] == 'Edit' && iTrack_AdminPreferenceIndividualReportingDetail.params["MeasureIndividualId"] != null) {
                iTrack_AdminPreferenceIndividualReportingDetail.fillMeasureIndividuals(iTrack_AdminPreferenceIndividualReportingDetail.params["MeasureIndividualId"]);
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' #headerId').text('Edit Measure');
            } else {
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' #headerId').text('Add Measure');
            }
            //   iTrack_AdminPreferenceIndividualReportingDetail.individualReportingSearch();
        });

    },
    fillMeasureIndividuals: function (MeasureIndividualId) {
        if (iTrack_AdminPreferenceIndividualReportingDetail.params.mode == "Edit") {
            iTrack_AdminPreferenceIndividualReportingDetail.fillMeasureIndividual_DBCall(MeasureIndividualId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var self = $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID);
                    utility.bindMyJSONByName(true, JSON.parse(response.measureIndividualList_JSON)[0], false, self);

                    if (JSON.parse(response.measureIndividualList_JSON)[0]['IsActive'] == true)
                        $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #Active').attr("checked", true);
                    else
                        $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #Active').attr("checked", false);

                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect('clearSelection', false);
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect('updateButtonText');
                    // Set the value
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #ddlPractice").val(JSON.parse(response.measureIndividualList_JSON)[0]['PracticeIds'].split(','));
                    // Then refresh
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect("refresh");
                    //litabPQRSIndividualMeasures
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #tabCQMIndividualMeasures').trigger('onclick');
                    iTrack_AdminPreferenceIndividualReportingDetail.selectMeasures(JSON.parse(response.measureIndividualList_JSON)[0]['MeasureIds'], JSON.parse(response.measureIndividualList_JSON)[0]['CQMMeasureIds'], JSON.parse(response.measureIndividualList_JSON)[0]['IAMeasureIds']);
                    iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr = JSON.parse(response.measureIndividualList_JSON)[0]['CQMMeasureIds'].split(',');
                    iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr = JSON.parse(response.measureIndividualList_JSON)[0]['IAMeasureIds'].split(',');
                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    setGrid: function (gridType) {
         if (gridType == 'CQM') {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMIndividualMeasures').css("display", "");
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPIndividualMeasures').css("display", "none");
        }
        else if (gridType == 'VBP') {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPIndividualMeasures').css("display", "");
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMIndividualMeasures').css("display", "none");
        }
    },
    resetMeasureIndividual: function (firstLoad) {
        if (firstLoad) {

            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect("destroy");

            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
        }

        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' select').each(function () { $(this).val('') });
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect('clearSelection', false);
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect('updateButtonText');
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #ddlPractice').multiselect("refresh");
        $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #Active').attr("checked", true);
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures #selectAllMeasures').attr("checked", false);
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val('');
        $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dtpSubmissionYear').datepicker('setDate', new Date())

        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });

        //CQM Individual Measures
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures #selectAllMeasures').attr("checked", false);
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val('');

        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail #CQMIndividualMeasures select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });

        iTrack_AdminPreferenceIndividualReportingDetail.ValidateMeasureIndividual();


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
    ValidateMeasureIndividual: function () {
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail').bootstrapValidator('destroy');
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail')
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
              $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail select[multiple]').each(function () {
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
           $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #frmiTrackAdminPreferenceIndividualReportingDetail select[multiple]').each(function () {
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
               iTrack_AdminPreferenceIndividualReportingDetail.saveMeasureIndividual();
           }
       });
    },
    addInCheckBoxArr: function (obj) {
        var type= "cqm";
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #litabCQMIndividualMeasures').hasClass("active") ? type = "cqm" : type = "vbp";
        if ($(obj).is(':checked')) {
            if (type == "cqm") {
                if ($.inArray(obj.id, iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr) == -1) 
                    iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr.push(obj.id);
            }
            else {
                if ($.inArray(obj.id, iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr) == -1) 
                    iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr.push(obj.id);
            }
        } else {
            if (type == "cqm") {
                var index = iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr.indexOf(obj.id);
                if (index > -1) {
                    iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr.splice(index, 1);
                }
            }
            else {
                var index = iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr.indexOf(obj.id);
                if (index > -1) {
                    iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr.splice(index, 1);
                }
            }
        }
    },
    saveMeasureIndividual: function () {

        var IAIDs = iTrack_AdminPreferenceIndividualReportingDetail.params.vbpChkBxArr.join(',');
        var CQMIDs = iTrack_AdminPreferenceIndividualReportingDetail.params.cqmChkBxArr.join(',');




        var PQRSMeasureIds = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val();
        var CQMMeasureIds = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val();
        if (PQRSMeasureIds != "" && CQMIDs != "") {
            PQRSMeasureIds = PQRSMeasureIds.concat(',' + CQMIDs)
        }
        else if (CQMIDs != "") {
            PQRSMeasureIds = CQMIDs
        }

        var VBPMeasureIds = $($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds')[0]).val();
        if (PQRSMeasureIds != "" && IAIDs != "") {
            PQRSMeasureIds = PQRSMeasureIds.concat(',' + IAIDs)
        }
        else if (IAIDs != "") {
            PQRSMeasureIds = IAIDs
        }

        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(PQRSMeasureIds);
        var self = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();

        if (iTrack_AdminPreferenceIndividualReportingDetail.params.mode == "Add") {
            iTrack_AdminPreferenceIndividualReportingDetail.saveMeasureIndividual_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    iTrack_AdminPreferenceIndividualReporting.measureIndividualSearch();
                    iTrack_AdminPreferenceIndividualReportingDetail.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
        else if (iTrack_AdminPreferenceIndividualReportingDetail.params.mode == "Edit") {

            var myJSON = self.getMyJSONByName();
            iTrack_AdminPreferenceIndividualReportingDetail.saveMeasureIndividual_DbCall(myJSON, iTrack_AdminPreferenceIndividualReportingDetail.params.MeasureIndividualId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    iTrack_AdminPreferenceIndividualReporting.measureIndividualSearch();
                    iTrack_AdminPreferenceIndividualReportingDetail.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
    },
    saveMeasureIndividual_DbCall: function (MeasureIndividualData, MeasureIndividualId) {
        var objData = JSON.parse(MeasureIndividualData);
        if (MeasureIndividualId == null) {
            objData["commandType"] = "SAVE_PQRS_MEASUREINDIVIDUAL";
        } else {
            objData["MeasureIndividualId"] = MeasureIndividualId;
            objData["commandType"] = "UPDATE_PQRS_MEASUREINDIVIDUAL";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures")) {
            $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
        }

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
        }

        iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable = null;
        if (iTrack_AdminPreferenceIndividualReportingDetail.params["FromAdmin"] == "0") {
            if (iTrack_AdminPreferenceIndividualReportingDetail.params != null && iTrack_AdminPreferenceIndividualReportingDetail.params.ParentCtrl != null) {
                UnloadActionPan(iTrack_AdminPreferenceIndividualReportingDetail.params.ParentCtrl, 'iTrack_AdminPreferenceIndividualReportingDetail');
            }
            else
                UnloadActionPan(null, 'iTrack_AdminPreferenceIndividualReportingDetail');
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
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
                });

                iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvMeasures');
            }, 2000);
        }

        if (CQMMeasureIds != null & CQMMeasureIds != '') {
            
            setTimeout(function () {
                $.each(CQMMeasureIds.split(','), function (index, item) {
                    var gridId = 'dgvCQMMeasures';
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
                });

                iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvCQMMeasures');
            }, 2000);
        }

        if (IAIds != null & IAIds != '') {
            
            setTimeout(function () {
                $.each(IAIds.split(','), function (index, item) {
                    var gridId = 'dgvVBPMeasures';
                    $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
                });

                iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvVBPMeasures');
            }, 2000);
        }

    },
    checkAllMeasureSelected: function (gridId) {
        if (!(gridId != null && gridId != "")) {
            gridId = 'dgvMeasures';
            if ($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #litabCQMIndividualMeasures").hasClass('active'))
                gridId = 'dgvCQMMeasures';
            else if ($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #litabVBPIndividualMeasures").hasClass('active'))
                gridId = 'dgvVBPMeasures';
        }

        var curPageChkbx = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' tbody input[type=checkbox]').length;
        var curPageChkdChkbx = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' tbody input[name="SelectCheckBoxMeasure"]:checked').length;
        if (curPageChkdChkbx == curPageChkbx && curPageChkbx > 0) {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', true);
        } else {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', false);
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
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', true);

        } else {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', false);

        }
        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').each(function (index, item) {
            iTrack_AdminPreferenceIndividualReportingDetail.addInCheckBoxArr(item);
        });
    },
    measuresSearch: function (measuresId) {
        PQRS_MeasureGroups_Detail.SearchMeasures_DBCall(measuresId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //iTrack_AdminPreferenceIndividualReportingDetail.MeasuresLoad(response);
                iTrack_AdminPreferenceIndividualReportingDetail.CQMMeasuresLoad(response);
                iTrack_AdminPreferenceIndividualReportingDetail.VBPMeasuresLoad(response);
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
            iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures').DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container()).on('keyup', 'tfoot input', function () {
                    iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvMeasures');
                });
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container).on('page.dt', function () {
                    if (iTrack_AdminPreferenceIndividualReportingDetail.params.mode == "Edit") {
                        setTimeout(function () {
                            iTrack_AdminPreferenceIndividualReportingDetail.selectMeasures($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(), $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container).on('change', '#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
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
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(this.id);
                        }
                    }
                    iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvMeasures');
                });
            }
        }

    },

    CQMMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
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
                var onclick = 'onclick="iTrack_AdminPreferenceIndividualReportingDetail.showMeasureDocumentByMeasureNumber(\'' + item.MeasureNumber + '\');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                var highPriority = "No";
                if (item.HighPriority == "Yes") {
                    highPriority = "Yes";
                }
                var measureType = "Process";
                if (item.MeasureNumber == "CMS122v5" || item.MeasureNumber == "CMS165v6") {
                    measureType = "Intermediate Outcome";
                }


                var infoIcon = '<a data-toggle="tooltip" title="" data-original-title="Test tooltip" ' + onclick + ' href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs pull-right" style="border-radius: 0px;"></i></a>';
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox"  class="text-center" id="' + item.MeasureId + '" onchange="iTrack_AdminPreferenceIndividualReportingDetail.addInCheckBoxArr(this)" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' + item.MeasureTitle + infoIcon + '</td><td>' + item.NQSDomain + '</td><td>' + measureType + '</td><td>' + highPriority + '</td>');
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures').DataTable({
                "language": {
                    "emptyTable": "No CQM Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures'))
            ;
        else {
            var headercount = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures  thead th').length - 1) > index) {
                        var title = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures').DataTable({ "bInfo": true, "searching": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container()).on('keyup', 'tfoot input', function () {
                    iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvCQMMeasures');
                });
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container).on('page.dt', function () {
                    if (iTrack_AdminPreferenceIndividualReportingDetail.params.mode == "Edit") {
                        setTimeout(function () {
                            iTrack_AdminPreferenceIndividualReportingDetail.selectMeasures($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(), $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container).on('change', '#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val(ArrMeasureIds.join(','));
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
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #CQMMeasureIds').val(this.id);
                        }
                    }
                    iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvCQMMeasures');
                });
            }
        }

    },

    VBPMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvVBPMeasures")) {
            $("#" + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvVBPMeasures").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvVBPMeasures").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + " #dgvVBPMeasures tbody").find("tr").remove();
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
                var onclick = 'onclick="iTrack_AdminPreferenceIndividualReportingDetail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                //if (item.DocumentName == null || item.DocumentName == '') {
                //    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                //    title = " title='No Document Found'";
                //}
                var weightage = "10";
                if (item.NQSDomain == "High")
                    weightage = "20";
                var infoIcon = '<a data-toggle="tooltip" title="" data-original-title="Test tooltip" onclick="" href="javascript:void(0)" style=" margin-left: 10px;"><i class="fa fa-info btn btn-primary btn-xs" style="border-radius: 0px;"></i></a>';
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" onchange="iTrack_AdminPreferenceIndividualReportingDetail.addInCheckBoxArr(this);" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + infoIcon + '</td><td>' + item.NQSDomain + '</td><td>' + weightage + '</td>');//<td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures').DataTable({
                "language": {
                    "emptyTable": "No VBP Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures'))
            ;
        else {
            var headercount = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures  thead th').length - 1) > index) {
                        var title = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures').DataTable({ "bInfo": true, "searching": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container()).on('keyup', 'tfoot input', function () {
                    iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvVBPMeasures');
                });
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container).on('page.dt', function () {
                    if (iTrack_AdminPreferenceIndividualReportingDetail.params.mode == "Edit") {
                        setTimeout(function () {
                            iTrack_AdminPreferenceIndividualReportingDetail.selectMeasures($('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #MeasureIds').val(), $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds').val());
                        }, 10);
                    }
                });
                $(iTrack_AdminPreferenceIndividualReportingDetail.measureIndividualTable.table().container).on('change', '#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds').val(ArrMeasureIds.join(','));
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
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + iTrack_AdminPreferenceIndividualReportingDetail.params.PanelID + ' #VBPMeasureIds').val(this.id);
                        }
                    }
                    iTrack_AdminPreferenceIndividualReportingDetail.checkAllMeasureSelected('dgvVBPMeasures');
                });
            }
        }

    },
    fillMeasureIndividual_DBCall: function (MeasureIndividualId) {
        var objData = {};
        objData["MeasureIndividualId"] = MeasureIndividualId;
        objData["commandType"] = "FILL_PQRS_MEASUREINDIVIDUAL";
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
        params["ParentCtrl"] = "iTrack_AdminPreferenceIndividualReportingDetail";
        params["FromAdmin"] = 0;
        LoadActionPan("PQRS_CMSView", params);
    },
}