PQRS_IndividualReporting_Detail = {
    bIsFirstLoad: true,
    params: [],
    measureIndividualTable: null,
    Load: function (params) {
        PQRS_IndividualReporting_Detail.params = params;
        PQRS_IndividualReporting_Detail.measureIndividualTable = null;
        if (PQRS_IndividualReporting_Detail.params.PanelID != 'pnlPQRS_IndividualReporting_Detail') {
            PQRS_IndividualReporting_Detail.params.PanelID = PQRS_IndividualReporting_Detail.params.PanelID + ' #pnlPQRS_IndividualReporting_Detail';
        } else {
            PQRS_IndividualReporting_Detail.params.PanelID = 'pnlPQRS_IndividualReporting_Detail';
        }
        if (PQRS_IndividualReporting_Detail.bIsFirstLoad) {
            PQRS_IndividualReporting_Detail.bIsFirstLoad = false;
            var self = $('#' + PQRS_IndividualReporting_Detail.params.PanelID);

            $('#' + PQRS_IndividualReporting_Detail.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            EMRUtility.CreateYearViewDatePicker(PQRS_IndividualReporting_Detail.params["PanelID"] + ' #dtpSubmissionYear',
               //on-change callback method 
               function (ev) {
                   if ($('#' + PQRS_IndividualReporting_Detail.params["PanelID"] + ' #frmPQRS_IndividualReporting_Detail').data("bootstrapValidator") != null) {
                       $('#' + PQRS_IndividualReporting_Detail.params["PanelID"] + ' #frmPQRS_IndividualReporting_Detail').bootstrapValidator('revalidateField', 'SubmissionYear');
                   }
               }, true);
        }

        var self = $('#' + PQRS_IndividualReporting_Detail.params.PanelID);
        PQRS_IndividualReporting_Detail.measuresSearch();
        self.loadDropDowns(true).done(function () {
            PQRS_IndividualReporting_Detail.resetMeasureIndividual(true);
            if (PQRS_IndividualReporting_Detail.params["mode"] == 'Edit' && PQRS_IndividualReporting_Detail.params["MeasureIndividualId"] != null) {
                PQRS_IndividualReporting_Detail.fillMeasureIndividuals(PQRS_IndividualReporting_Detail.params["MeasureIndividualId"]);
                $('#' + PQRS_IndividualReporting_Detail.params["PanelID"] + ' #headerId').text('Edit Measure');
            } else {
                $('#' + PQRS_IndividualReporting_Detail.params["PanelID"] + ' #headerId').text('Add Measure');
            }
            //   PQRS_IndividualReporting_Detail.individualReportingSearch();
        });

    },
    fillMeasureIndividuals: function (MeasureIndividualId) {
        if (PQRS_IndividualReporting_Detail.params.mode == "Edit") {
            PQRS_IndividualReporting_Detail.fillMeasureIndividual_DBCall(MeasureIndividualId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var self = $("#" + PQRS_IndividualReporting_Detail.params.PanelID);
                    utility.bindMyJSONByName(true, JSON.parse(response.measureIndividualList_JSON)[0], false, self);

                    if (JSON.parse(response.measureIndividualList_JSON)[0]['IsActive'] == true)
                        $("#" + PQRS_IndividualReporting_Detail.params.PanelID + ' #Active').attr("checked", true);
                    else
                        $("#" + PQRS_IndividualReporting_Detail.params.PanelID + ' #Active').attr("checked", false);

                    $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect('clearSelection', false);
                    $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect('updateButtonText');
                    // Set the value
                    $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #ddlPractice").val(JSON.parse(response.measureIndividualList_JSON)[0]['PracticeIds'].split(','));
                    // Then refresh
                    $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect("refresh");
                    //litabPQRSIndividualMeasures
                    $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #tabPQRSIndividualMeasures').trigger('onclick');
                    PQRS_IndividualReporting_Detail.selectMeasures(JSON.parse(response.measureIndividualList_JSON)[0]['MeasureIds'], JSON.parse(response.measureIndividualList_JSON)[0]['CQMMeasureIds'], JSON.parse(response.measureIndividualList_JSON)[0]['VBPMeasureIds']);
                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    setGrid: function (gridType) {
        if (gridType == 'PQRS') {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #PQRSIndividualMeasures').css("display", "");
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMIndividualMeasures').css("display", "none");
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPIndividualMeasures').css("display", "none");
        }
        else if (gridType == 'CQM') {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMIndividualMeasures').css("display", "");
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #PQRSIndividualMeasures').css("display", "none");
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPIndividualMeasures').css("display", "none");
        }
        else if (gridType == 'VBP') {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPIndividualMeasures').css("display", "");
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #PQRSIndividualMeasures').css("display", "none");
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMIndividualMeasures').css("display", "none");
        }
    },
    resetMeasureIndividual: function (firstLoad) {
        if (firstLoad) {

            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect("destroy");

            $('#' + PQRS_IndividualReporting_Detail.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
        }

        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' select').each(function () { $(this).val('') });
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect('clearSelection', false);
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect('updateButtonText');
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #ddlPractice').multiselect("refresh");
        $("#" + PQRS_IndividualReporting_Detail.params.PanelID + ' #Active').attr("checked", true);
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures #selectAllMeasures').attr("checked", false);
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val('');
        $("#" + PQRS_IndividualReporting_Detail.params.PanelID + ' #dtpSubmissionYear').datepicker('setDate', new Date())

        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #frmPQRS_IndividualReporting_Detail select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });

        //CQM Individual Measures
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures #selectAllMeasures').attr("checked", false);
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val('');

        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #frmPQRS_IndividualReporting_Detail #CQMIndividualMeasures select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });

        PQRS_IndividualReporting_Detail.ValidateMeasureIndividual();


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
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #frmPQRS_IndividualReporting_Detail').bootstrapValidator('destroy');
        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #frmPQRS_IndividualReporting_Detail')
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
              $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #frmPQRS_IndividualReporting_Detail select[multiple]').each(function () {
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
           $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #frmPQRS_IndividualReporting_Detail select[multiple]').each(function () {
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
               PQRS_IndividualReporting_Detail.saveMeasureIndividual();
           }
       });
    },
    saveMeasureIndividual: function () {
        var PQRSMeasureIds = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val();
        var CQMMeasureIds = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val();
        if (PQRSMeasureIds != "" && CQMMeasureIds != "") {
            PQRSMeasureIds = PQRSMeasureIds.concat(',' + CQMMeasureIds)
        }
        else if (CQMMeasureIds != "") {
            PQRSMeasureIds = CQMMeasureIds
        }

        var VBPMeasureIds = $($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds')[0]).val();
        if (PQRSMeasureIds != "" && VBPMeasureIds != "") {
            PQRSMeasureIds = PQRSMeasureIds.concat(',' + VBPMeasureIds)
        }
        else if (VBPMeasureIds != "") {
            PQRSMeasureIds = VBPMeasureIds
        }

        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(PQRSMeasureIds);
        var self = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();

        if (PQRS_IndividualReporting_Detail.params.mode == "Add") {
            PQRS_IndividualReporting_Detail.saveMeasureIndividual_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PQRS_IndividualReporting.measureIndividualSearch();
                    PQRS_IndividualReporting_Detail.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
        else if (PQRS_IndividualReporting_Detail.params.mode == "Edit") {

            var myJSON = self.getMyJSONByName();
            PQRS_IndividualReporting_Detail.saveMeasureIndividual_DbCall(myJSON, PQRS_IndividualReporting_Detail.params.MeasureIndividualId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PQRS_IndividualReporting.measureIndividualSearch();
                    PQRS_IndividualReporting_Detail.UnLoadTab();
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
        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures")) {
            $("#" + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
        }

        PQRS_IndividualReporting_Detail.measureIndividualTable = null;
        if (PQRS_IndividualReporting_Detail.params["FromAdmin"] == "0") {
            if (PQRS_IndividualReporting_Detail.params != null && PQRS_IndividualReporting_Detail.params.ParentCtrl != null) {
                UnloadActionPan(PQRS_IndividualReporting_Detail.params.ParentCtrl, 'PQRS_IndividualReporting_Detail');
            }
            else
                UnloadActionPan(null, 'PQRS_IndividualReporting_Detail');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    selectMeasures: function (measureIds, CQMMeasureIds, VBPMeasureIds) {
        if (measureIds != null & measureIds != '') {
            var gridId = 'dgvMeasures';
            $.each(measureIds.split(','), function (index, item) {
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
            });
            PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvMeasures');
        }

        if (CQMMeasureIds != null & CQMMeasureIds != '') {
            var gridId = 'dgvCQMMeasures';
            $.each(CQMMeasureIds.split(','), function (index, item) {
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
            });
            PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvCQMMeasures');
        }

        if (VBPMeasureIds != null & VBPMeasureIds != '') {
            var gridId = 'dgvVBPMeasures';
            $.each(VBPMeasureIds.split(','), function (index, item) {
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' #' + item + '').prop('checked', true);
            });
            PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvVBPMeasures');
        }
        
    },
    checkAllMeasureSelected: function (gridId) {
        if (!(gridId != null && gridId != "")) {
            gridId = 'dgvMeasures';
            if ($('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #litabCQMIndividualMeasures").hasClass('active'))
                gridId = 'dgvCQMMeasures';
            else if ($('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #litabVBPIndividualMeasures").hasClass('active'))
                gridId = 'dgvVBPMeasures';
        }
        
        var curPageChkbx = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' tbody input[type=checkbox]').length;
        var curPageChkdChkbx = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' tbody input[name="SelectCheckBoxMeasure"]:checked').length;
        if (curPageChkdChkbx == curPageChkbx && curPageChkbx > 0) {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', true);
        } else {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', false);
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
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', true);

        } else {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', false);

        }
    },
    measuresSearch: function (measuresId) {
        PQRS_MeasureGroups_Detail.SearchMeasures_DBCall(measuresId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PQRS_IndividualReporting_Detail.MeasuresLoad(response);
                PQRS_IndividualReporting_Detail.CQMMeasuresLoad(response);
                PQRS_IndividualReporting_Detail.VBPMeasuresLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    MeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures")) {
            $("#" + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
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
                var onclick = 'onclick="PQRS_IndividualReporting_Detail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + '</td><td>' + item.NQSDomain + '</td><td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures').DataTable({
                "language": {
                    "emptyTable": "No Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures'))
            ;
        else {
            var headercount = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures  thead th').length - 1) > index) {
                        var title = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            PQRS_IndividualReporting_Detail.measureIndividualTable = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures').DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container()).on('keyup', 'tfoot input', function () {
                    PQRS_IndividualReporting_Detail.measureIndividualTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvMeasures');
                });
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container).on('page.dt', function () {
                    if (PQRS_IndividualReporting_Detail.params.mode == "Edit") {
                        setTimeout(function () {
                            PQRS_IndividualReporting_Detail.selectMeasures($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(), $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container).on('change', '#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
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
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(this.id);
                        }
                    }
                    PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvMeasures');
                });
            }
        }

    },

    CQMMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
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
                var onclick = 'onclick="PQRS_IndividualReporting_Detail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + '</td><td>' + item.NQSDomain + '</td><td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures').DataTable({
                "language": {
                    "emptyTable": "No CQM Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures'))
            ;
        else {
            var headercount = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures  thead th').length - 1) > index) {
                        var title = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            PQRS_IndividualReporting_Detail.measureIndividualTable = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures').DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container()).on('keyup', 'tfoot input', function () {
                    PQRS_IndividualReporting_Detail.measureIndividualTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvCQMMeasures');
                });
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container).on('page.dt', function () {
                    if (PQRS_IndividualReporting_Detail.params.mode == "Edit") {
                        setTimeout(function () {
                            PQRS_IndividualReporting_Detail.selectMeasures($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(), $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container).on('change', '#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val(ArrMeasureIds.join(','));
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
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #CQMMeasureIds').val(this.id);
                        }
                    }
                    PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvCQMMeasures');
                });
            }
        }

    },

    VBPMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvVBPMeasures")) {
            $("#" + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvVBPMeasures").dataTable().fnClearTable();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvVBPMeasures").dataTable().fnDestroy();
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + " #dgvVBPMeasures tbody").find("tr").remove();
        }

        if (response.VBPmeasureCount > 0) {
            var MeasureJSONData = JSON.parse(response.VBPmeasureList_JSON);
            $.each(MeasureJSONData, function (i, item) {
                //if (item.MeasureType != "VBP")
                //    return;
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.MeasureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureId);
                $row.attr("MeasureGroupsId", item.MeasureId);
                var onclick = 'onclick="PQRS_IndividualReporting_Detail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + '</td><td>' + item.NQSDomain + '</td><td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures').DataTable({
                "language": {
                    "emptyTable": "No VBP Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures'))
            ;
        else {
            var headercount = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures  thead th').length - 1) > index) {
                        var title = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            PQRS_IndividualReporting_Detail.measureIndividualTable = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures').DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container()).on('keyup', 'tfoot input', function () {
                    PQRS_IndividualReporting_Detail.measureIndividualTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvVBPMeasures');
                });
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container).on('page.dt', function () {
                    if (PQRS_IndividualReporting_Detail.params.mode == "Edit") {
                        setTimeout(function () {
                            PQRS_IndividualReporting_Detail.selectMeasures($('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #MeasureIds').val(), $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds').val());
                        }, 10);
                    }
                });
                $(PQRS_IndividualReporting_Detail.measureIndividualTable.table().container).on('change', '#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]', function () {
                    var measureIds = $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var ArrMeasureIds = (measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }
                                }
                            });
                        } else {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #dgvVBPMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds').val(ArrMeasureIds.join(','));
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
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds').val(this.id);
                        }

                    } else {
                        if (isChecked) {
                            $('#' + PQRS_IndividualReporting_Detail.params.PanelID + ' #VBPMeasureIds').val(this.id);
                        }
                    }
                    PQRS_IndividualReporting_Detail.checkAllMeasureSelected('dgvVBPMeasures');
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
        params["ParentCtrl"] = "PQRS_IndividualReporting_Detail";
        params["FromAdmin"] = 0;
        LoadActionPan("PQRS_CMSView", params);
    },
}