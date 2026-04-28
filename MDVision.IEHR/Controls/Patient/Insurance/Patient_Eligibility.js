Patient_Eligibility = {
    params: [],
    ActiveRowIndex: -1,
    GridPatientEligibility: "dgvEligibility",
    IsAnyEligibilityChecked: false,
    Load: function (params) {

        Patient_Eligibility.params = params;
        Patient_Eligibility.IsAnyEligibilityChecked = false;
       
        if (Patient_Eligibility.params["PanelID"] != 'pnlPatientEligibility')
            Patient_Eligibility.params["PanelID"] = Patient_Eligibility.params["PanelID"] + ' #pnlPatientEligibility';
      

        var Tab = GetTab(Patient_Eligibility.params["TabID"]);
        if (Tab["PanelID"] != "") {

            if (Tab["Container"] != "")
                Patient_Eligibility.params["PanelID"] = Tab["Container"] + ' #' + Tab["PanelID"] + ' #pnlPatientEligibility';;


            if (Tab["MasterTabID"] && Tab["MasterTabID"] == "mstrTabBatch")
                Patient_Eligibility.SetScreen(Tab);
            else {
                Patient_Eligibility.params["ActionPanContainer"] = "actionPanPatientEligibility";
                Patient_Eligibility.GridPatientEligibility = "dgvEligibility",
                $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection #parentdiv').removeClass('row').addClass('modal-body');

                //Set Patient ID.
                $('#' + Patient_Eligibility.params["PanelID"] + ' #hfPatientId').val(Patient_Eligibility.params.patientID);
                CacheManager.BindDropDownsByID("#" + Patient_Eligibility.params.PanelID + " #ddlPatientInsurancePlan", 'GetPatientInsurance', true, $('#' + Patient_Eligibility.params["PanelID"] + ' #hfPatientId').val());
                
                var self = $('#' + Patient_Eligibility.params.PanelID);
                self.loadDropDowns(true).done(function () {
                    Patient_Eligibility.LoadEligibilityDefaults();
                    Patient_Eligibility.ValidateEligibility();
                    Patient_Eligibility.LoadPatientEligibility($('#' + Patient_Eligibility.params["PanelID"] + ' #hfPatientId').val());
                });

                $("#" + Patient_Eligibility.params.PanelID + " #frmPatientEligibility #dosfield").text("Date of Service");
                $("#" + Patient_Eligibility.params.PanelID + " #dtpDOSTo").addClass("hidden");
                utility.CreateDatePicker(Patient_Eligibility.params.PanelID + ' #dtpDOSFrom', function () { }, true);
            }
        }
        Patient_Eligibility.BindProvider();
        Patient_Eligibility.BindAutocomplete();

        $('#' + Patient_Eligibility.params["PanelID"] + ' #chkSelectAll ').change(function () {
            if (this.checked) {
                $("#" + Patient_Eligibility.params["PanelID"] + " #dgvEligibilityBatch  [id^='cb_']").prop("checked", true);
            }
            else {
                $("#" + Patient_Eligibility.params["PanelID"] + " #dgvEligibilityBatch  [id^='cb_']").prop("checked", false);
            }
        });
    },
    ExportData: function (e) {
        if ($("#" + Patient_Eligibility.params.PanelID +  " #dgvEligibilityBatch  [id^='cb_']:checked ").length > 0) {
            var JSONData = [];
            $("#" + Patient_Eligibility.params.PanelID +  " #dgvEligibilityBatch tbody tr").each(function () {
                var chkbx = $(this).find("[id^='cb_']");
                if (chkbx.length > 0) {
                    chkbx = chkbx[0].checked;
                    if (chkbx) { 
                        var obj = {
                            PatientName: $(this).find("#PatientName").text().trim(),
                            AccountNumber: $(this).find("#AccountNumber").text().trim(),
                            DOB: utility.RemoveTimeFromDate(null, $(this).find("#DOB").text().trim()),
                            SubscriberID: $(this).find("#SubscriberID").text().trim(),
                            Status: $(this).find("#Status").text().trim(),
                            DOS: utility.RemoveTimeFromDate(null, $(this).find("#DOS").text().trim()),
                            InsurancePlan: $(this).find("#InsurancePlan").text().trim(),
                            EQSeviceName: $(this).find("#ServiceType").text().trim(),
                            PlanPriority: $(this).find("#Priority").text().trim(),
                            EligibilityDate: $(this).find("#EligibillityDate").text().trim(),
                            Copay: $(this).find("#Copay").text().trim(),
                            Deductible: $(this).find("#Deductible").text().trim(),
                            ProviderName: $(this).find("#provider").text().trim(),
                            User: $(this).find("#User").text().trim(),
                        }
                        JSONData.push(obj);
                    }
                }
            });
            Patient_Eligibility.ExportDataToExcel(JSONData);

        } else {

            var self = $("#" + Patient_Eligibility.params.PanelID + " #frmPatientEligibility");
            var myJSON = self.getMyJSON();
            Patient_Eligibility.Load_PatientEligibilityExport(myJSON).done(function (response) {
                if (response.status != false) {
                    var myJSON_Data = JSON.parse(response.PatientEligibilityLoad_JSON);
                    if (myJSON_Data.length > 0) {
                        var JSONData = [];
                        $.each(myJSON_Data, function (i, item) {

                            var obj = {
                                PatientName: item.PatientName,
                                AccountNumber: item.AccountNumber,
                                DOB: utility.RemoveTimeFromDate(null, item.DOB),
                                SubscriberID: item.SubscriberID,
                                Status: item.Status,
                                DOS: utility.RemoveTimeFromDate(null, item.DOS),
                                InsurancePlan: item.InsurancePlan,
                                EQSeviceName: item.EQSeviceName,
                                PlanPriority: item.PlanPriority,
                                EligibilityDate: item.DOS,
                                Copay: item.Copay,
                                Deductible: item.Deductible,
                                ProviderName: item.ProviderName,
                                User: item.CreatedBy,


                            }
                            JSONData.push(obj);


                        });
                        Patient_Eligibility.ExportDataToExcel(JSONData);
                    } else {
                        utility.DisplayMessages("No Record Found.", 3);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }


            });
        }
    },
    ExportDataToExcel: function(JSONData) {
   
        
        var ReportTitle = "Patient Eligibility";
        var ShowLabel = true;
        //If JSONData is not an object then JSON.parse will parse the JSON string in an Object

        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        //Set Report title in first row or line

        CSV += ReportTitle + '\r\n\n';

        //This condition will generate the Label/Header
        if (ShowLabel) {
            var row = "";

            //This loop will extract the label from 1st index of on array
            for (var index in arrData[0]) {

                //Now convert each value to string and comma-seprated
                row += index + ',';
            }

            row = row.slice(0, -1);

            //append Label row with line break
            CSV += row + '\r\n';
        }

        //1st loop is to extract each row
        for (var i = 0; i < arrData.length; i++) {
            var row = "";

            //2nd loop will extract each column and convert it in string comma-seprated
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
            }

            row.slice(0, row.length - 1);

            //add a line break after each row
            CSV += row + '\r\n';
        }

        if (CSV == '') {
            alert("Invalid data");
            return;
        }

        //Generate a file name
        var fileName = "";
        //this will remove the blank-spaces from the title and replace it with an underscore
        fileName += ReportTitle.replace(/ /g, "_");
        var csvData = new Blob([CSV], { type: 'text/csv' }); //new way
        var csvUrl = URL.createObjectURL(csvData);
        //$(this)
        //    .attr({
        //        'download': fileName,
        //        'href': csvUrl
        //    });

        //Initialize file format you want csv or xls
        //var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

        // Now the little tricky part.
        // you can use either>> window.open(uri);
        // but this will not work in some browsers
        // or you will not get the correct file extension 

        //this trick will generate a temp <a /> tag
        var link = document.createElement("a");
        link.href = csvUrl;

        //set the visibility hidden so it will not effect on your web-layout
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";

        //this part will append the anchor tag and remove it after automatic click
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    },
    BindProvider: function () {
        var Ctrl = $("#" + Patient_Eligibility.params.PanelID + " #frmPatientEligibility #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Eligibility.params.PanelID + " #frmPatientEligibility #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    LoadEligibilityDefaults: function () {

        //setValues
        $("#" + Patient_Eligibility.params.PanelID + " #txtAccount").val(Patient_Eligibility.params.patientAccount);
        $("#" + Patient_Eligibility.params.PanelID + " #txtLastName").val(Patient_Eligibility.params.patientLastName);
        $("#" + Patient_Eligibility.params.PanelID + " #txtFirstName").val(Patient_Eligibility.params.patientFirstName);
        $("#" + Patient_Eligibility.params.PanelID + " #ddlPatientInsurancePlan").val(Patient_Eligibility.params.patientInsurancePlanId);
        $("#" + Patient_Eligibility.params.PanelID + " #txtProvider").val(Patient_Eligibility.params.Provider);
        $("#" + Patient_Eligibility.params.PanelID + " #hfProvider").val(Patient_Eligibility.params.ProviderId);

        $Ctrl_p = $("#" + Patient_Eligibility.params.PanelID + " #txtProvider");
        $hfCtrl_p = $("#" + Patient_Eligibility.params.PanelID + " #hfProvider");
        //Provider
        if ($Ctrl_p.data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, Patient_Eligibility.params.Provider, $hfCtrl_p, Patient_Eligibility.params.ProviderId);

        $("#" + Patient_Eligibility.params.PanelID + " #ddlServiceTypeCode").val("30");
        Patient_Eligibility.GetInsuranceEligibility($("#" + Patient_Eligibility.params.PanelID + " #ddlPatientInsurancePlan"));
        if ($("#" + Patient_Eligibility.params.PanelID + " #ddlPatientInsurancePlan option").length == 2) {
            $($("#" + Patient_Eligibility.params.PanelID + " #ddlPatientInsurancePlan option")[1]).prop('selected', true);
        }
        else {
            $($("#" + Patient_Eligibility.params.PanelID + " #ddlPatientInsurancePlan option")[0]).prop('selected', true);
        }
    },

    ValidateEligibility: function () {
        $('#' + Patient_Eligibility.params.PanelID + ' #frmPatientEligibility')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  DateOfService: {
                      group: '.col-md-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                          date: {
                              format: date_format.toUpperCase(),
                              message: ' '
                          }
                      }
                  },
                  InsurancePlan: {
                      group: '.col-md-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Provider: {
                      group: '.col-md-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ServiceTypeCode: {
                      group: '.col-md-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          }).on('success.form.bv', function (e) {
              e.preventDefault();
              Patient_Eligibility.CheckPatientEligibility();
          });
    },

    CheckPatientEligibility: function () {
                var self = $("#" + Patient_Eligibility.params.PanelID + " #frmPatientEligibility");
                var myJSON = self.getMyJSON();
                Patient_Eligibility.Check_PatientEligibility(myJSON).done(function (response) {
                    if (response.status != false) {
                        //view Eligibility Detail
                        Patient_Eligibility.OpenEligibilityDetail(-1, response);
                        Patient_Eligibility.ActiveRowIndex = 0;
                        Patient_Eligibility.LoadPatientEligibility($('#' + Patient_Eligibility.params["PanelID"] + ' #hfPatientId').val());
                        Patient_Eligibility.IsAnyEligibilityChecked = true;
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
    },

    LoadPatientEligibility: function (PatientId, PageNo, rpp) {
      
            var self = $("#" + Patient_Eligibility.params.PanelID + " #frmPatientEligibility");
            var myJSON = self.getMyJSON();

            Patient_Eligibility.Load_PatientEligibility(myJSON, PatientId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    if ($("#" + Patient_Eligibility.params["PanelID"] + " #EligibilityhistoryDiv").css("display") == "none") {
                        $("#" + Patient_Eligibility.params["PanelID"] + " #EligibilityhistoryDiv").show();
                    }
                    Patient_Eligibility.LoadEligibilityGrid(response, PageNo, rpp);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
       
    },

    SearchEligibility: function ()
    {
        utility.ValidateSearchCriteria(Patient_Eligibility.params.PanelID + " #frmPatientEligibility", function () {
            Patient_Eligibility.LoadPatientEligibility(null,1,15);
        });
    },


    LoadEligibilityGrid: function (response, PageNo, rpp) {

        //Set Custom Paging
        if (response.PatientEligibilityCount > 0) {
            $("#" + Patient_Eligibility.params.PanelID + " #divEligibilityPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 25;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divEligibilityPaging", response.iTotalDisplayRecords, 5, "Patient_Eligibility", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Patient_Eligibility.params.PanelID + "  #divEligibilityPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Patient_Eligibility.params.PanelID + " #pnlEligibility_Result li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                } else
                    $(this).removeAttr("class");
            });
        } else {
            $("#" + Patient_Eligibility.params.PanelID + " #divEligibilityPaging").css("display", "none");
            var showingText = "Showing 0 Record(s)";
            $("#" + Patient_Eligibility.params.PanelID + " #divEligibilityPaging #divShowingEntries").text(showingText);
        }

        //Bind Data in Table
        $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.GridPatientEligibility).dataTable().fnDestroy();
        $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.GridPatientEligibility + " tbody").find("tr").remove();

        if (response.PatientEligibilityCount > 0) {
            var PatientEligibilityLoadJSONData = JSON.parse(response.PatientEligibilityLoad_JSON);
            $.each(PatientEligibilityLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                if (Patient_Eligibility.ActiveRowIndex == i)
                    $row.attr("class", "active");


                $row.append('<td><input  class="btn btn-xs" style="margin-left: 10%;" onclick="Patient_Eligibility.CheckAllSelected();" id=cb_' + item.ContactId + ' type="checkbox" name="To' + i + '"  name="contacts" id="' + item.ContactId + '"></td><td><a class="btn btn-xs" href="#" onclick="Patient_Eligibility.EDIEligibilityDelete(' + item.EDIEligibilityId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Eligibility.OpenEligibilityDetail(' + item.EDIEligibilityId + ');" title="Eligibility Detail"><i class="fa fa-eye black"></i></a><a class="btn  btn-xs" href="#" onclick="Patient_Eligibility.OpenEligibilityFile(' + item.EDIEligibilityId + ',270);" title="View 270 file"><i class="fa fa-file black"></i></a><a class="btn  btn-xs" href="#" onclick="Patient_Eligibility.OpenEligibilityFile(' + item.EDIEligibilityId + ',271);" title="View 271 file"><i class="fa fa-file blue"></i></a></td><td id="PatientName">' + item.PatientName + '</td><td id="AccountNumber">' + item.AccountNumber + '</td><td id="DOB">' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td id="SubscriberID">' + item.SubscriberID + '</td><td id="Status">' + item.Status + '</td><td id="DOS">' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td id="InsurancePlan">' + item.InsurancePlan + '</td><td id="ServiceType" data-toggle="tooltip" data-placement="right"  class="size-max120 ellipses" title=" ' + item.EQSeviceName + '">' + item.EQSeviceName + '</td><td id="Priority">' + item.PlanPriority + '</td><td id="EligibillityDate">' + item.CreatedOn + '</td><td id="Copay" class="text-right">' + globalAppdata.DefaultCurrency + Number(item.Copay).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td id ="Deductible" class="text-right">' + globalAppdata.DefaultCurrency + Number(item.Deductible).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td id="provider">' + item.ProviderName + '</td><td id="User">' + item.CreatedBy + '</td>');
                $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.GridPatientEligibility + " tbody").last().append($row);
            });
        }
        else {
            $('#' + Patient_Eligibility.params.PanelID + ' #' + Patient_Eligibility.GridPatientEligibility).DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0,1] }], "bPaginate": false, "bInfo": false
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Patient_Eligibility.params.PanelID + ' #' + Patient_Eligibility.GridPatientEligibility))
            ;
        else
            $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.GridPatientEligibility).DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0,1] }], "bPaginate": false, "bInfo": false }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },
    CheckAllSelected: function () {
        if ($("#" + Patient_Eligibility.params.PanelID + " #dgvEligibilityBatch  [id^='cb_']:checked ").length != $("#" + Patient_Eligibility.params.PanelID + " #dgvEligibilityBatch  [id^='cb_'] ").length)
        {
            $("#" + Patient_Eligibility.params.PanelID + " #chkSelectAll").prop('checked', false);
        }else
        {

            $("#" + Patient_Eligibility.params.PanelID + " #chkSelectAll").prop('checked', true);
        }

    },
    EDIEligibilityDelete: function (EDIEligibilityId) {
        utility.myConfirm('1', function () {
            var selectedValue = EDIEligibilityId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_Eligibility.DeleteEDIEligibility(selectedValue).done(function (response) {
                    if (response.status != false) {
                        var table1 = $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.GridPatientEligibility).DataTable();
                        table1.row('.active').remove().draw(false);
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '1');
    },

    GetInsuranceEligibility: function (obj) {
        var InsurancePlanId = $(obj).val();

        if (InsurancePlanId != "") {
            if (InsurancePlanId != "" && InsurancePlanId > 0) {
                Patient_Eligibility.Load_InsuranceEDIEligibility(InsurancePlanId, Patient_Eligibility.params.patientID).done(function (response) {

                    if (response.status != false) {
                        $("#" + Patient_Eligibility.params.PanelID + " #txtEligibilityInsurance").val(response.EDIEligibility);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else {
            $("#" + Patient_Eligibility.params.PanelID + " #txtEligibilityInsurance").val("");
        }
    },

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = null;
        params["RefForm"] = "frmPatientEligibility";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        if (!Patient_Eligibility.params["IsTab"])
            params["ParentCtrl"] = "Patient_Eligibility";
        LoadActionPan('Admin_Provider', params);
    },

    OpenEligibilityDetail: function (id, dataset) {

        var params = [];
        params["EDIEligibilityId"] = id;
        params["EDIEligibilityData"] = dataset;
        if (!Patient_Eligibility.params["IsTab"])
            params["ParentCtrl"] = "Patient_Eligibility";

        LoadActionPan('Patient_Eligibility_Detail', params);
    },
    OpenEligibilityDetailSch: function (id, dataset) {

        var params = [];
        params["EDIEligibilityId"] = id;
        params["EDIEligibilityData"] = dataset;

        params["ParentCtrl"] = "schTabCalendar";

        LoadActionPan('Patient_Eligibility_Detail', params);
    },

    OpenEligibilityFile: function (id, type) {

        var self = $("#" + Patient_Eligibility.params.PanelID + " #viewEligibilityFile #frmviewEligibilityFile");
        var myJSON = self.getMyJSON();

        $("#" + Patient_Eligibility.params.PanelID + " #viewEligibilityFile #lblFileHeading").html("View " + type + " File");
        $("#" + Patient_Eligibility.params.PanelID + " #viewEligibilityFile #txtEligibilityTextView").val('');
        Patient_Eligibility.LoadEligibilityReport(id, type).done(function (response) {

            if (response.status != false) {
                var fileData = JSON.parse(response.PatientEligibilityReport_JSON);
                if (fileData.txtEligibilityTextView != "") {

                    utility.bindMyJSON(true, fileData, false, self).done(function () {

                        $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.params.ActionPanContainer).prepend($("#viewEligibilityFile").html());
                        $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.params.ActionPanContainer).modal({
                            show: 'true',
                            backdrop: 'static',
                            keyboard: false

                        });
                    });

                }
                else {
                    utility.DisplayMessages('No data found', 2);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    //------ Batch specific  functions starts ------//

    SetScreen: function (Tab) {

        $("#" + Tab["Container"]).find("#actionPanPatientEligibility").attr('id', Tab['ActionPanContainer']);
        $("#" + Tab["Container"]).find("#pnlPatientEligibility").attr('id', Tab['PanelID']);
        Patient_Eligibility.params["PanelID"] = Tab['PanelID'];
        Patient_Eligibility.params["IsTab"] = true;

        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection .modal-header').hide();
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection .modal-content').removeClass('modal-content');
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection .modal-dialog-full').removeAttr('class');
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection #parentdiv').removeClass('modal-body').addClass('row');
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection #EligibilityhistoryDiv thead tr').removeClass('bg-gray');
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection #EligibilityhistoryDiv section').addClass('panel-body panel-featured').find('label:first').remove();
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection #EligibilityhistoryDiv section .toggle-content').css('display', 'block');
        $('#' + Patient_Eligibility.params.PanelID + ' #ScreenSection #EligibilityhistoryDiv section').find('div:first').removeClass("panel-body");

        $('#' + Patient_Eligibility.params["PanelID"] + " #formpanelheading").css('display', 'block');

        Patient_Eligibility.GridPatientEligibility = "dgvEligibilityBatch";
        $('#' + Patient_Eligibility.params["PanelID"]).find("#dgvEligibility").each(function () {
            var id_ = $(this).attr('id');
            $(this).attr('id', id_ + "Batch");
        });

        //enable fields
        $("#" + Patient_Eligibility.params.PanelID + " #lnkPatientAccount").removeAttr("disabled");
        $("#" + Patient_Eligibility.params.PanelID + " #txtAccount").removeAttr("disabled");
        $("#" + Patient_Eligibility.params.PanelID + " #txtLastName").removeAttr("disabled");
        $("#" + Patient_Eligibility.params.PanelID + " #txtFirstName").removeAttr("disabled");
        $("#" + Patient_Eligibility.params.PanelID + " #btnSearch").removeClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " #btnEligibility").addClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " span.required").addClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " #div_insurancePlans").removeClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " .only-dialog").addClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " .only-screen").removeClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " #dtpDOSFrom").datepicker("setDate", new Date());
        $("#" + Patient_Eligibility.params.PanelID + " #dtpDOSTo").datepicker("setDate", new Date());
        $("#" + Patient_Eligibility.params.PanelID + " #btnReset").removeClass("hidden");
        $("#" + Patient_Eligibility.params.PanelID + " #btnExport").removeClass("hidden");
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#" + Patient_Eligibility.params.PanelID + " #txtInsurancePlan");
            var hfCtrl = $("#" + Patient_Eligibility.params.PanelID + " #hfInsurancePlanId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
        });
        var self = $('#' + Patient_Eligibility.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Patient_Eligibility.LoadPatientEligibility();
        });

        utility.ValidateFromToDate(Patient_Eligibility.params.PanelID, 'dtpDOSFrom', 'dtpDOSTo', true);
        utility.ValidateFromToDate(Patient_Eligibility.params.PanelID, 'dtpEligibiltyFrom', 'dtpEligibiltyTo', true);
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_Eligibility.params["PanelID"] + ' #hfPatientId', false);
    },

    BindAutocomplete: function () {
        var Ctrl = $("#" + Patient_Eligibility.params["PanelID"] + " #txtAccount");
        var hfCtrl = $("#" + Patient_Eligibility.params["PanelID"] + " #hfPatientId");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var onSelect = function (e) {
            var fullname = e.FullName.split(',');
            $("#" + Patient_Eligibility.params["PanelID"] + " #txtLastName").val(fullname[0]);
            $("#" + Patient_Eligibility.params["PanelID"] + " #txtFirstName").val(fullname[1]);
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = Patient_Eligibility.params["TabID"];
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, FirstName, LastName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#' + Patient_Eligibility.params["PanelID"] + ' #hfPatientId').val(PatientId);
        $('#' + Patient_Eligibility.params["PanelID"] + ' #txtAccount').val(AccountNo);
        $("#" + Patient_Eligibility.params["PanelID"] + " #txtLastName").val(LastName);
        $("#" + Patient_Eligibility.params["PanelID"] + " #txtFirstName").val(FirstName);
        if ($('#' + Patient_Eligibility.params["PanelID"] + ' #txtAccount').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Patient_Eligibility.params["PanelID"] + ' #txtAccount'), AccountNo, $('#' + Patient_Eligibility.params["PanelID"] + ' #hfPatientId'), PatientId);
        UnloadActionPan(Patient_Eligibility.params["TabID"]);
        utility.InsertRecentPatient(PatientId);
    },

    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_InsurancePlan', params);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        if ($("#" + Patient_Eligibility.params["PanelID"] + " #txtInsurancePlan").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_Eligibility.params["PanelID"] + " #txtInsurancePlan"), InsurancePlanName, $("#" + Patient_Eligibility.params["PanelID"] + " #hfInsurancePlan"), InsurancePlanId);
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    },

    //------ Batch specific  functions end ------//

    //------ Custom Paging Overloaded Methods starts ------//

    SelectedPageClick: function (PageNo, objPage, frm, to, pagingDivId) {

        var selecter = "#" + Patient_Eligibility.params.PanelID + " #pnlEligibility_Result li";

        // Change Background Color to Black for selected page
        $(selecter).each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        Patient_Eligibility.LoadPatientEligibility(null, PageNo, 25);

    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var selecter = "#" + Patient_Eligibility.params.PanelID + " #pnlEligibility_Result li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Patient_Eligibility.LoadPatientEligibility(null, currentPageNo, 25);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {


        var selecter = "#" + Patient_Eligibility.params.PanelID + " #pnlEligibility_Result li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Patient_Eligibility.LoadPatientEligibility(null, currentPageNo, 25);
        }
    },

    //------ Custom Paging Overloaded Methods starts ------//

    //------ Service side calls starts ------//

    Load_InsuranceEDIEligibility: function (InsurancePlanId, PatientId) {

        var data = "InsurancePlanId=" + InsurancePlanId + "&PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY", "LOAD_INSURANCE_EDI_ELIGIBILITY");
    },

    Check_PatientEligibility: function (Eligibilitydata) {

        var data = "Eligibilitydata=" + Eligibilitydata;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY", "CHECK_PATIENT_ELIGIBILITY");
    },

    Load_PatientEligibility: function (Eligibilitydata, PatientId, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 25 : RowsPerPage;

        var data = "Eligibilitydata=" + Eligibilitydata + "&PatientId=" + PatientId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY", "LOAD_PATIENT_ELIGIBILITY");
    },
    Load_PatientEligibilityExport: function (Eligibilitydata, PatientId) {
        var data = "Eligibilitydata=" + Eligibilitydata + "&PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY", "LOAD_PATIENT_ELIGIBILITY_EXPORT");
    },
    DeleteEDIEligibility: function (EDIEligibilityId) {

        var data = "EDIEligibilityId=" + EDIEligibilityId;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY", "DELETE_EDI_ELIGIBILITY");
    },

    LoadEligibilityReport: function (EDIEligibilityId, EDIEligibilityType) {

        var data = "EDIEligibilityId=" + EDIEligibilityId + "&FileType=" + EDIEligibilityType;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY", "LOAD_INSURANCE_EDI_ELIGIBILITY_REPORT");
    },

    //------ Service side calls ends ------//

    UnLoad: function () {

        // implementation of story IMP-193 STARTS
        if (Patient_Eligibility.IsAnyEligibilityChecked) {
            if (Patient_Eligibility.params["ParentCtrl"] == "schTabCalendar" || Patient_Eligibility.params["ParentCtrl"] == "schTabMultipleView") {
                var $active_calenderbtn = $("#containerScheduleMode").find('button.active');
                if ($active_calenderbtn && $("#schTabCalendar").hasClass('active'))
                    $($active_calenderbtn).click();
                else if ($("#schTabMultipleView").hasClass('active'))
                    $("#pnlScheduleMuliView #frmSchedulingMuliView #btnsearch").click();
            }
            else if (Patient_Eligibility.params["ParentCtrl"] == "schTabMultipleView" && $("#schTabMultipleView").hasClass('active')) {
                $("#pnlScheduleMuliView #frmSchedulingMuliView #btnsearch").click();
            }
        }
        // implementation of story IMP-193 ENDS

        if (Patient_Eligibility.params != null && Patient_Eligibility.params.ParentCtrl) {
            //UnloadActionPan(Patient_Eligibility.params.ParentCtrl);
            if (Patient_Eligibility.params.ParentCtrl == 'schTabCalendar') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'pnlScheduleCalendar');
            }
            else if (Patient_Eligibility.params.ParentCtrl == 'patTabInsurance') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'pnlPatientInsurance');
                if (Patient_Eligibility.params.IsFromInsuranceSave && Patient_Eligibility.params.IsFromInsuranceSave == true) {
                    Patient_Insurance.ShowAppointmentUpdateAlert();
                }
            }
            else if (Patient_Eligibility.params.ParentCtrl == 'Patient_Insurance') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'pnlPatientInsurance');
                if (Patient_Eligibility.params.IsFromInsuranceSave && Patient_Eligibility.params.IsFromInsuranceSave == true) {
                    Patient_Insurance.ShowAppointmentUpdateAlert();
                }
            }
            else if (Patient_Eligibility.params.ParentCtrl == 'appointmentDetail') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'appointmentDetail ');
            }
            else if (Patient_Eligibility.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'pnlBillFollowUpPatientARDetail');
            }
            else if (Patient_Eligibility.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'pnlBillFollowUpInsuranceARDetail');
            }
            else if (Patient_Eligibility.params.ParentCtrl == 'schTabMultipleView') {
                UnloadActionPan(Patient_Eligibility.params.ParentCtrl, 'Patient_Eligibility', null, 'pnlScheduleMuliView');
            }
        }
        else if (Patient_Eligibility.params["IsTab"]) {
            RemoveAdminTab();
        }
        else
            UnloadActionPan();

        Patient_Eligibility.params = null;
    },

    UnLoadFileViewer: function () {

        $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.params.ActionPanContainer).modal('hide');
        setTimeout(function () {
            $("#" + Patient_Eligibility.params.PanelID + " #" + Patient_Eligibility.params.ActionPanContainer).find('div').first().remove();
        }, 300);

        //$("#" + Patient_Eligibility.params.PanelID + " #"+ Patient_Eligibility.params.ActionPanContainer).find('div').first().hide('blind', 500, function () { $(this).remove(); });
    },
    ClearEligibilitySearch: function () {

        $('#' + Patient_Eligibility.params["PanelID"] + ' #frmPatientEligibility').resetAllControls();
    },

}
