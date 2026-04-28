EDIBatchDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        EDIBatchDetail.params = params;
        if (EDIBatchDetail.bIsFirstLoad) {
            EDIBatchDetail.bIsFirstLoad = false;
            EDIBatchDetail.LoadEDIBatchDetail();
        }


    },

    LoadEDIBatchDetail: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        $("#EDIBatchDetail #batch_details :not(textarea)").attr('disabled', true);
        //$("#EDIBatchDetail #batch_details :input[type=text]").attr('disabled', true);
        //$('#EDIBatchDetail #Attach_EDI_file').attr('disabled', true);
        EDIBatchDetail.FillEDIBatchDetail().done(function (response) {
            if (response.status != false) {
                var ediBatch_detail = JSON.parse(response.EDIBatchLoad_JSON);

                var self = $("#EDIBatchDetail");
                utility.bindMyJSON(true, ediBatch_detail, false, self).done(function () {

                    $('#frmEDIBatchDetail').data('serialize', $('#frmEDIBatchDetail').serialize());
                });
                EDIBatchDetail.BindEDIReports(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    BindEDIReports: function (response) {
        var Provider_Detail = JSON.parse(response.EDIReport_JSON);
        var rowSelect = 0;
        if ($("#EDIBatchDetail #pnlAttachmentBatch_Result").css("display") == "none") {
            $("#EDIBatchDetail #pnlAttachmentBatch_Result").show();
        }

        $("#dgvAttachments").dataTable().fnDestroy();
        $("#EDIBatchDetail #pnlAttachmentBatch_Result #dgvAttachments tbody").find("tr").remove();

        if (response.EDIReportCount > 0) {
            var EDIReportJSONData = JSON.parse(response.EDIReport_JSON);
            $.each(EDIReportJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td style="display:none;">' + item.EDIReportId + '</td><td><a class="btn  btn-xs" href="#" onclick="EDIBatchDetail.EDIReportDelete(' + item.EDIReportId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="EDIBatchDetail.EDIBatchReportView(' + item.EDIReportId + ');"  title="View Record"><i class="fa fa-edit black"></i></a></td><td>' + item.FileName + '</td><td>' + item.CreatedByName + '</td><td>' + item.EntryDate + '</td><td>' + item.Comments + '</td>');
                $("#EDIBatchDetail #dgvAttachments tbody").last().append($row);
            });
        }
        else {
            $('#dgvAttachments').DataTable({
                "language": {
                    "emptyTable": "No Report Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvAttachments'))
            ;
        else
            $("#pnlAttachmentBatch_Result #dgvAttachments").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    EDIReportDelete: function (EDIReportId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = EDIReportId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    EDIBatchDetail.DeleteEDIReport(selectedValue).done(function (response) {
                        if (response.status != false) {
                            EDIBatchDetail.LoadEDIBatchDetail();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                '1'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    View_Claims: function () {
        var params = [];
        params["_837BatchId"] = EDIBatchDetail.params._837BatchId;
        params["ParentCtrl"] = 'EDIBatchDetail';
        LoadActionPan('EDIClaimViewDetail', params);
    },

    ValidateEDIFileType: function (fName, input) {
        // Use a regular expression to trim everything before final dot
        var extension = fName.replace(/^.*\./, '');
        // Iff there is no dot anywhere in filename, we would have extension == filename,
        // so we account for this possibility now
        if (extension == fName) {
            extension = '';
        }
        else {
            // if there is an extension, we convert to lower case
            extension = extension.toLowerCase();
        }

        try {

            var exs = $(input).attr("allowfiles");
            var allowfiles = exs.split(",");
            if (allowfiles.indexOf(extension) >= 0)
                return true;
            else
                return false;

        } catch (ex) {
            console.log(ex);
            return false;
        }

    },

    Attach_EDIReport: function (input, IsBatch, ClearinghouseId) {
        var strMessage = "";
        if (input.files) {
            var fName = input.files[0];
            fName = fName.name;
            if (fName.length <= 100) {
                if (EDIBatchDetail.ValidateEDIFileType(fName, input)) {

                    var reader = new FileReader();
                    reader.onload = function (event) {
                        var data = reader.result;
                        EDIBatchDetail.SaveEDIFile(fName, data, IsBatch, ClearinghouseId).done(function (response) {
                            var extension = fName.replace(/^.*\./, '');
                            if (response.status != false) {

                                if (IsBatch == true)
                                    EDIBatchDetail.LoadEDIBatchDetail();
                                else {
                                    Bill_EDIReport.EDIReportSearch();
                                }
                                /*Begin Added by Azeem Raza Tayyab to Implement CR:"PMS-942" on 01-Nov-2016*/
                                if (extension == '835') {
                                    Bill_ERA.Download_ERA(response.EDIReportId);
                                }
                                /*End Added by Azeem Raza Tayyab to Implement CR:PMS-942 on 01-Nov-2016*/
                                utility.DisplayMessages(response.message, 1);
                                $(input).val(null);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    };
                    reader.onerror = function () {
                        utility.DisplayMessages('Unable to read file data', 3);
                    };
                    reader.readAsText(input.files[0]);
                }
                else {
                    utility.DisplayMessages("Only " + $(input).attr("allowfiles") + " files are allowed", 2);
                    $(input).val(null);
                }
            }
            else {
                utility.DisplayMessages("File name should be 20 characters long.", 2);
            }
        }


    },

    EDIBatchReportView: function (EDIReportId) {
        var params = [];
        params["EDIReportId"] = EDIReportId;
        params["ParentCtrl"] = 'EDIBatchDetail';
        params["EDIBatchNumber"] = $("#EDIBatchDetail #txtBatchNumber").val();
        LoadActionPan('EDIReviewReport', params);
    },

    Save_BatchReport: function () {

        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment Status", "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var self = $("#EDIBatchDetail #frmEDIBatchDetail");
            var myJSON = self.getMyJSON();

            EDIBatchDetail.SaveBatchReport(myJSON).done(function (response) {

                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });

        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });

    },


    FillEDIBatchDetail: function () {
        var data = "_837BatchId=" + EDIBatchDetail.params._837BatchId + "&BatchControlNo=" + EDIBatchDetail.params.BatchControlNo;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EDI_BATCH_DETAIL", "FILL_EDI_BATCH_DETAIL");
    },

    DeleteEDIReport: function (EDIReportId) {
        var data = "EDIReportId=" + EDIReportId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EDI_BATCH_DETAIL", "DELETE_EDI_REPORT");
    },

    SaveEDIFile: function (fileName, data, IsBatch, ClearinghouseId) {

        if (IsBatch == true)
            ClearinghouseId = $("#EDIBatchDetail #hfClearingHouseId").val();

        var data_ = utility.replaceSpecialCharacters(data);
        var data = "_837BatchNumber=" + EDIBatchDetail.params.BatchControlNo + "&fileName=" + fileName + "&_837BatchId=" + EDIBatchDetail.params._837BatchId + "&ClearingHouseId=" + ClearinghouseId + "&IsBatch=" + IsBatch + "&fileData=" + data_;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EDI_BATCH_DETAIL", "SAVE_EDI_FILE");
    },

    SaveBatchReport: function (data) {

        var data = "_837BatchId=" + EDIBatchDetail.params._837BatchId + "&_837BatchData=" + data;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EDI_BATCH_DETAIL", "UPDATE_EDI_REPORT");
    },


    UnLoad: function () {
        if (EDIBatchDetail.params.ParentCtrl == "EDIReviewReport") {
            UnloadActionPan(EDIBatchDetail.params.ParentCtrl,null );
        }
        else { UnloadActionPan(null, 'EDIBatchDetail'); }
    },

    View_EDIFile: function (_837BatchId) {

        var edi_string = $("#frmEDIBatchDetail #hfEDI837String").val();
        if (edi_string != "") {

            $("#EDIBatchDetail #actionPanEDIBatchDetail").prepend($("#viewEDIFileData").html());
            $("#EDIBatchDetail #actionPanEDIBatchDetail").modal({
                show: 'true',
                backdrop: 'static',
                keyboard: false

            });

            $("#EDIBatchDetail #txtTextView").val(edi_string);
        }
        else {
            utility.DisplayMessages('No data is found', 2);
        }
    },

    UnLoadEdiFileViewer: function () {

        $("#EDIBatchDetail #actionPanEDIBatchDetail").modal('hide');

        $("#EDIBatchDetail #actionPanEDIBatchDetail").find('div').first().hide('blind', 500, function () { $(this).remove(); });
    },

}