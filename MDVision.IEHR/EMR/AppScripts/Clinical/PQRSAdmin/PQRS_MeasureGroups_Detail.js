PQRS_MeasureGroups_Detail = {
    bIsFirstLoad: true,
    params: [],
    measureTable: null,
    Load: function (params) {
        PQRS_MeasureGroups_Detail.params = params;
        PQRS_MeasureGroups.measureTable = null;
        if (PQRS_MeasureGroups_Detail.params.PanelID != 'pnlPQRS_MeasureGroups_Detail') {
            PQRS_MeasureGroups_Detail.params.PanelID = PQRS_MeasureGroups_Detail.params.PanelID + ' #pnlPQRS_MeasureGroups_Detail';
        } else {
            PQRS_MeasureGroups_Detail.params.PanelID = 'pnlPQRS_MeasureGroups_Detail';
        }
        if (PQRS_MeasureGroups_Detail.bIsFirstLoad) {
            PQRS_MeasureGroups_Detail.bIsFirstLoad = false;
            var self = $('#' + PQRS_MeasureGroups_Detail.params.PanelID);

            $('#' + PQRS_MeasureGroups_Detail.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            EMRUtility.CreateYearViewDatePicker(PQRS_MeasureGroups_Detail.params["PanelID"] + ' #dtpSubmissionYear',
               //on-change callback method 
               function (ev) {
                   if ($('#' + PQRS_MeasureGroups_Detail.params["PanelID"] + ' #frmPQRS_MeasureGroups_Detail').data("bootstrapValidator") != null) {
                       $('#' + PQRS_MeasureGroups_Detail.params["PanelID"] + ' #frmPQRS_MeasureGroups_Detail').bootstrapValidator('revalidateField', 'SubmissionYear');
                   }
               }, true);
        }

        PQRS_MeasureGroups_Detail.measuresSearch();
        self.loadDropDowns(true).done(function () {
            PQRS_MeasureGroups_Detail.resetMeasureGroup(true);
            if (PQRS_MeasureGroups_Detail.params["mode"] == 'Edit' && PQRS_MeasureGroups_Detail.params["MeasureGroupId"] != null) {
                PQRS_MeasureGroups_Detail.fillMeasureGroups(PQRS_MeasureGroups_Detail.params["MeasureGroupId"]);
                $('#' + PQRS_MeasureGroups_Detail.params["PanelID"] + ' #headerId').text('Edit Group');
            } else {
                $('#' + PQRS_MeasureGroups_Detail.params["PanelID"] + ' #headerId').text('Add Group');
            }
        });

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
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail').bootstrapValidator('destroy');
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  MeasureGroupsName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
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
                  ProviderIds: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  SpecialtyIds: {
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
              $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail select[multiple]').each(function () {
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
           $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail select[multiple]').each(function () {
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
               var ProvLenght = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail select#ddlProviderIds').val().length;
               if (ProvLenght > 1) {
                   PQRS_MeasureGroups_Detail.saveMeasureGroup();
               } else {
                   utility.DisplayMessages("Please select at least two Providers", 2);
               }

           }
       });
    },


    saveMeasureGroup: function () {

        var self = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' form');

        var checkedVals = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures input:checked").map(function () {
            return this.id;
        }).get();

        var firstIndex = checkedVals.indexOf('selectAllMeasures');
        if (firstIndex > -1) {
            checkedVals.splice(firstIndex, 1);
        }

        var CQMcheckedVals = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures input:checked").map(function () {
            return this.id;
        }).get();

        var CQMfirstIndex = CQMcheckedVals.indexOf('selectAllMeasures');
        if (CQMfirstIndex > -1) {
            CQMcheckedVals.splice(CQMfirstIndex, 1);
        }

        var MeasureIds = '';
        if (checkedVals.length > 0 && CQMcheckedVals.length > 0) {
            MeasureIds = checkedVals.join(",").concat(',' + CQMcheckedVals.join(","))
        }
        else if (CQMcheckedVals.length > 0) {
            MeasureIds = CQMcheckedVals.join(",");
        }
        else {
            MeasureIds = checkedVals.join(",");
        }

        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail #MeasureIds').val(MeasureIds);

        var myJSON = self.getMyJSONByName();
        if (PQRS_MeasureGroups_Detail.params.mode == "Add") {
            PQRS_MeasureGroups_Detail.saveMeasureGroup_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PQRS_MeasureGroups.measureGroupsSearch();
                    PQRS_MeasureGroups_Detail.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
        else if (PQRS_MeasureGroups_Detail.params.mode == "Edit") {

            var myJSON = self.getMyJSONByName();
            PQRS_MeasureGroups_Detail.saveMeasureGroup_DbCall(myJSON, PQRS_MeasureGroups_Detail.params.MeasureGroupId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PQRS_MeasureGroups.measureGroupsSearch();
                    PQRS_MeasureGroups_Detail.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        }
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures")) {
            $("#" + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
        }

        PQRS_MeasureGroups_Detail.measureTable = null;
        if (PQRS_MeasureGroups_Detail.params["FromAdmin"] == "0") {
            if (PQRS_MeasureGroups_Detail.params != null && PQRS_MeasureGroups_Detail.params.ParentCtrl != null) {
                UnloadActionPan(PQRS_MeasureGroups_Detail.params.ParentCtrl, 'PQRS_MeasureGroups_Detail');
            }
            else
                UnloadActionPan(null, 'PQRS_MeasureGroups_Detail');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    saveMeasureGroup_DbCall: function (MeasureGroupData, measureGroupsId) {
        var objData = JSON.parse(MeasureGroupData);
        //    var measuresSelected = $("input:checked", $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures").dataTable().fnGetNodes()).map(function () {
        //     return $(this).attr('id');
        //  }).get().join(',');
        //  objData["MeasureIds"] = measuresSelected;
        if (measureGroupsId == null) {
            objData["commandType"] = "SAVE_PQRS_MEASUREGROUPS";
        } else {
            objData["MeasureGroupId"] = measureGroupsId;
            objData["commandType"] = "UPDATE_PQRS_MEASUREGROUPS";
        }


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_MeasureGroup");
    },
    //******************utility support functions***************
    fillMeasureGroups: function (MeasureGroupId) {
        if (PQRS_MeasureGroups_Detail.params.mode == "Edit") {
            PQRS_MeasureGroups_Detail.fillMeasureGroups_DBCall(MeasureGroupId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var self = $("#" + PQRS_MeasureGroups_Detail.params.PanelID);
                    utility.bindMyJSONByName(true, JSON.parse(response.measureGroupList_JSON)[0], false, self);

                    if (JSON.parse(response.measureGroupList_JSON)[0]['IsActive'] == true)
                        $("#" + PQRS_MeasureGroups_Detail.params.PanelID + ' #Active').attr("checked", true);
                    else
                        $("#" + PQRS_MeasureGroups_Detail.params.PanelID + ' #Active').attr("checked", false);

                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect('clearSelection', false);
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect('updateButtonText');
                    // Set the value                
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #ddlSpecialtyIds").val(JSON.parse(response.measureGroupList_JSON)[0]['SpecialtyIds'].split(','));
                    // Then refresh
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect("refresh");

                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect('clearSelection', false);
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect('updateButtonText');
                    // Set the value
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #ddlProviderIds").val(JSON.parse(response.measureGroupList_JSON)[0]['ProviderIds'].split(','));
                    // Then refresh
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect("refresh");

                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect('clearSelection', false);
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect('updateButtonText');
                    // Set the value
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #ddlPractices").val(JSON.parse(response.measureGroupList_JSON)[0]['PracticeIds'].split(','));
                    // Then refresh
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect("refresh");
                    $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #tabPQRSGroupMeasures').trigger('onclick');
                    PQRS_MeasureGroups_Detail.selectMeasures(JSON.parse(response.measureGroupList_JSON)[0]['MeasureIds'], JSON.parse(response.measureGroupList_JSON)[0]['CQMMeasureIds']);
                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    setGrid: function (gridType) {
        if (gridType == 'PQRS') {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #CQMGroupMeasures').css("display", "none");
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #PQRSGroupMeasures').css("display", "");
        }
        else {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #CQMGroupMeasures').css("display", "");
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #PQRSGroupMeasures').css("display", "none");
        }

    },
    selectAllMeasures: function (cntrl, gridId) {
        if (gridId != null && gridId != '') {

        }
        else {
            gridId = 'dgvMeasures';
        }
        if ($(cntrl).is(':checked')) {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', true);

        } else {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #" + gridId + ' [name="SelectCheckBoxMeasure"]').prop('checked', false);

        }
    },
    selectMeasures: function (measureIds, CQMMeasureIds) {
        if (measureIds != null & measureIds != '') {
            $.each(measureIds.split(','), function (index, item) {
                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures" + ' #' + item + '').prop('checked', true);
            });
            
            PQRS_MeasureGroups_Detail.checkAllMeasureSelected('dgvMeasures');
        }
        if (CQMMeasureIds != null & CQMMeasureIds != '') {
            $.each(CQMMeasureIds.split(','), function (index, item) {
                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures" + ' #' + item + '').prop('checked', true);
            });
            
            PQRS_MeasureGroups_Detail.checkAllMeasureSelected('dgvCQMMeasures');
        }
        
    },
    checkAllMeasureSelected: function (gridId) {
        if (!(gridId != null && gridId != "")) {
            gridId = 'dgvMeasures';
            if ($('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #litabCQMGroupMeasures").hasClass('active'))
                gridId = 'dgvCQMMeasures';
        }
        
        var curPageChkbx = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #" + gridId + ' tbody input[type=checkbox]').length;
        var curPageChkdChkbx = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #" + gridId + ' tbody input[name="SelectCheckBoxMeasure"]:checked').length;
        if (curPageChkdChkbx == curPageChkbx && curPageChkbx > 0) {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', true);
        } else {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #' + gridId + ' #selectAllMeasures').prop('checked', false);
        }
    },
    resetMeasureGroup: function (firstLoad) {
        if (firstLoad) {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect("destroy");
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect("destroy");
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect("destroy");

            $('#' + PQRS_MeasureGroups_Detail.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
        }

        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect('clearSelection', false);
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect('updateButtonText');
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect('clearSelection', false);
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect('updateButtonText');
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect('clearSelection', false);
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect('updateButtonText');
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlSpecialtyIds').multiselect("refresh");
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlProviderIds').multiselect("refresh");
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #ddlPractices').multiselect("refresh");
        $("#" + PQRS_MeasureGroups_Detail.params.PanelID + ' #Active').attr("checked", true);
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures #selectAllMeasures').attr("checked", false);
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });

        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures #selectAllMeasures').attr("checked", false);
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures [name="SelectCheckBoxMeasure"]').prop('checked', false);
        $("input:checked", $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnGetNodes()).each(function () {
            $(this).prop('checked', false);
        });

        $("#" + PQRS_MeasureGroups_Detail.params.PanelID + ' #txtMeasureGroupsName').val("");
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val('');
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #CQMMeasureIds').val('');
        $("#" + PQRS_MeasureGroups_Detail.params.PanelID + ' #dtpSubmissionYear').datepicker('setDate', new Date())
        $("#" + PQRS_MeasureGroups_Detail.params.PanelID + ' #txtMeasureGroupsName').removeClass('')
        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #frmPQRS_MeasureGroups_Detail select[multiple]').each(function () {
            $(this).closest('div').find('label.control-label').removeClass('has-error');
            $(this).closest('div').find('label.control-label').css('color', '');
        });
        PQRS_MeasureGroups_Detail.ValidateMeasureGroup();


    },
    //*********************************end support functions

    ////****************************************************
    ////************ Measures*******************************
    ////****************************************************
    measuresSearch: function (measuresId) {


        PQRS_MeasureGroups_Detail.SearchMeasures_DBCall(measuresId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PQRS_MeasureGroups_Detail.MeasuresLoad(response);
                PQRS_MeasureGroups_Detail.CQMMeasuresLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    MeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures")) {
            $("#" + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures").dataTable().fnClearTable();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures").dataTable().fnDestroy();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvMeasures tbody").find("tr").remove();
        }

        if (response.PQRSmeasureCount > 0) {
            var MeasureJSONData = JSON.parse(response.PQRSmeasureList_JSON);
            $.each(MeasureJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvMeasures_row" + item.MeasureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureId);
                $row.attr("MeasureGroupsId", item.MeasureId);
                var onclick = 'onclick="PQRS_MeasureGroups_Detail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + '</td><td>' + item.NQSDomain + '</td><td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures').DataTable({
                "language": {
                    "emptyTable": "No Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures'))
            ;
        else {
            var headercount = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures  thead th').length - 1) > index) {
                        var title = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            PQRS_MeasureGroups_Detail.measureTable = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures').DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(PQRS_MeasureGroups_Detail.measureTable.table().container()).on('keyup', 'tfoot input', function () {
                    PQRS_MeasureGroups_Detail.measureTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    PQRS_MeasureGroups_Detail.checkAllMeasureSelected('dgvMeasures');
                });
                $(PQRS_MeasureGroups_Detail.measureTable.table().container).on('page.dt', function () {
                    if (PQRS_MeasureGroups_Detail.params.mode == "Edit") {
                        setTimeout(function () {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures #selectAllMeasures').prop('checked', false);
                            PQRS_MeasureGroups_Detail.selectMeasures($('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(), $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(PQRS_MeasureGroups_Detail.measureTable.table().container).on('change', '#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures input[type=checkbox]', function () {
                    var measureIds = $(this).attr('id');//$('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var array = new Array();
                    array = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val().split(',');
                    var ArrMeasureIds = [];
                    $.each(array, function (i) {
                        ArrMeasureIds.push(i);
                    });
                    var zeroIndex = ArrMeasureIds.indexOf(0);
                    if (zeroIndex >= 0) {
                        ArrMeasureIds.splice(zeroIndex, 1);
                    }
                    // var ArrMeasureIds = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val();//(measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }

                                }
                            });
                        } else {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                    } else if (measureIds != null & measureIds != '') {
                        if (ArrMeasureIds != null && this.id != null) {
                            var selectedId = this.id != null ? parseInt(this.id) : null;
                            var index = ArrMeasureIds.indexOf(selectedId);
                            if (isChecked) {
                                if (index < 0) {
                                    ArrMeasureIds.push(this.id);
                                }
                            } else {
                                if (index >= 0) {
                                    ArrMeasureIds.splice(index, 1);
                                }
                            }

                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            if (isChecked) {
                                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(this.id);
                            }
                        }

                    }
                    else {
                        if (isChecked) {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(this.id);
                        }
                    }
                    PQRS_MeasureGroups_Detail.checkAllMeasureSelected('dgvMeasures');

                });
            }
        }

    },

    CQMMeasuresLoad: function (response) {

        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures")) {
            $("#" + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnClearTable();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures").dataTable().fnDestroy();
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + " #dgvCQMMeasures tbody").find("tr").remove();
        }

        if (response.CQMmeasureCount > 0) {
            var MeasureJSONData = JSON.parse(response.CQMmeasureList_JSON);
            $.each(MeasureJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvCQMMeasures_row" + item.MeasureId + "'))");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureId);
                $row.attr("MeasureGroupsId", item.MeasureId);
                var onclick = 'onclick="PQRS_MeasureGroups_Detail.showMeasureDocument(' + item.MeasureId + ');"';
                var title = " title='" + item.DocumentName + "'";
                if (item.DocumentName == null || item.DocumentName == '') {
                    onclick = 'onclick="utility.DisplayMessages(\'No Document Found\', 2);"';
                    title = " title='No Document Found'";
                }
                SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" id="' + item.MeasureId + '" name="SelectCheckBoxMeasure"  class="input-block text-center"/></td>';
                $row.append(SelectionCheckBoxColumn +
                    '<td>' + item.MeasureNumber + '</td><td>' +
                   item.MeasureTitle + '</td><td>' + item.NQSDomain + '</td><td class="sorting_1 center"><a ' + title + onclick + ' href="javascript:void(0)"><i class="fa fa-info-circle"></i></a></td>');
                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures tbody').last().append($row);

            });
        }
        else {
            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures').DataTable({
                "language": {
                    "emptyTable": "No CQM Measure(s) Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures'))
            ;
        else {
            var headercount = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures  thead th').length
            // appending textbox controls
            if (response.measureCount > 0) {
                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures  tfoot th').each(function (index, element) {
                    if (index > 0 && ($('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures  thead th').length - 1) > index) {
                        var title = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures thead th').eq($(this).index()).text();
                        $(this).html('<input type="text" placeholder="Search ' + title + '" class="form-control" style="font-weight:100;min-width:105px;" data-index="' + index + '" />');
                    }
                });
            }
            PQRS_MeasureGroups_Detail.measureTable = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures').DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "pageLength": 20, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            if (response.measureCount > 0) {
                // DataTable binding filters
                $(PQRS_MeasureGroups_Detail.measureTable.table().container()).on('keyup', 'tfoot input', function () {
                    PQRS_MeasureGroups_Detail.measureTable
                        .column($(this).data('index'))
                        .search(this.value)
                        .draw();
                    PQRS_MeasureGroups_Detail.checkAllMeasureSelected('dgvCQMMeasures');
                });
                $(PQRS_MeasureGroups_Detail.measureTable.table().container).on('page.dt', function () {
                    if (PQRS_MeasureGroups_Detail.params.mode == "Edit") {
                        setTimeout(function () {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures #selectAllMeasures').prop('checked', false);
                            PQRS_MeasureGroups_Detail.selectMeasures($('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(), $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #CQMMeasureIds').val());
                        }, 10);
                    }
                });
                $(PQRS_MeasureGroups_Detail.measureTable.table().container).on('change', '#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]', function () {
                    var measureIds = $(this).attr('id');//$('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val();
                    var isChecked = $(this).is(':checked');
                    var array = new Array();
                    array = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val().split(',');
                    var ArrMeasureIds = [];
                    $.each(array, function (i) {
                        ArrMeasureIds.push(i);
                    });
                    var zeroIndex = ArrMeasureIds.indexOf(0);
                    if (zeroIndex >= 0) {
                        ArrMeasureIds.splice(zeroIndex, 1);
                    }
                    // var ArrMeasureIds = $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val();//(measureIds == "" || measureIds == null) ? [] : measureIds.split(',');
                    var elementId = this.id;
                    if (this.id == "selectAllMeasures") {
                        if (isChecked) {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]:checked').each(function (item) {
                                if (ArrMeasureIds != null && this.id != null && elementId != this.id) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index < 0) {
                                        ArrMeasureIds.push(this.id);
                                    }

                                }
                            });
                        } else {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #dgvCQMMeasures input[type=checkbox]').each(function (item) {

                                if (ArrMeasureIds != null && this.id != null) {
                                    var index = ArrMeasureIds.indexOf(this.id);
                                    if (index >= 0) {
                                        ArrMeasureIds.splice(index, 1);
                                    }
                                }
                            });
                        }
                        $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                    } else if (measureIds != null & measureIds != '') {
                        if (ArrMeasureIds != null && this.id != null) {
                            var selectedId = this.id != null ? parseInt(this.id) : null;
                            var index = ArrMeasureIds.indexOf(selectedId);
                            if (isChecked) {
                                if (index < 0) {
                                    ArrMeasureIds.push(this.id);
                                }
                            } else {
                                if (index >= 0) {
                                    ArrMeasureIds.splice(index, 1);
                                }
                            }

                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(ArrMeasureIds.join(','));
                        } else {
                            if (isChecked) {
                                $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(this.id);
                            }
                        }

                    }
                    else {
                        if (isChecked) {
                            $('#' + PQRS_MeasureGroups_Detail.params.PanelID + ' #MeasureIds').val(this.id);
                        }
                    }
                    PQRS_MeasureGroups_Detail.checkAllMeasureSelected('dgvCQMMeasures');
                });
            }
        }

    },
    fillMeasureGroups_DBCall: function (MeasureGroupId) {
        var objData = {};
        objData["MeasureGroupId"] = MeasureGroupId;
        objData["commandType"] = "FILL_PQRS_MEASUREGROUPS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_MeasureGroup");
    },
    SearchMeasures_DBCall: function (MeasuresId) {

        var objData = {};
        objData["MeasuresId"] = MeasuresId;
        objData["commandType"] = "SEARCH_PQRS_MEASURES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_Measure");
    },

    showMeasureDocument: function (MeasureId) {

        var params = [];
        params["MeasureId"] = MeasureId;
        params["viewMode"] = "Measures";
        params["ParentCtrl"] = "PQRS_MeasureGroups_Detail";
        params["FromAdmin"] = 0;
        LoadActionPan("PQRS_CMSView", params);
    },
}