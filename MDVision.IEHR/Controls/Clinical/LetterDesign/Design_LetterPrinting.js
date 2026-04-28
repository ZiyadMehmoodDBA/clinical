designLetterPrinting = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        designLetterPrinting.params = params;

        var self = $('#designLetterPrinting');
        self.loadDropDowns(true);
        //CacheManager.BindDropDownsByID('#designLetterPrinting #ddlLetters', 'GetLetters', true, 19);

        designLetterPrinting.FindAllPrinters();
        designLetterPrinting.ValidatePrintLetter();
    },

    FindAllPrinters: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
               
                designLetterPrinting.FindPrinters().done(function (response) {
                    if (response.status != false) {
                        var printers = response.printersList;
                        //var PatientDetailJSON_detail = JSON.parse(PatientDetailJSON);
                        $("#frmLetterPrinting #lstPrinters").empty();
                        $("#frmLetterPrinting #lstPrinters").append($('<option/>', {
                            value: "",
                            html: "- SELECT -",
                        }));
                        $.each(printers, function (i, item) {
                            $("#frmLetterPrinting #lstPrinters").append(
                                $('<option />', {

                                    html: item,
                                    
                                })
                            );

                        });

                    }
                    else {
                        //utility.DisplayMessages(response.Message, 3);
                    }
                });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    LetterFieldsSearch: function (LetterId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
                if ($("#designLetterPrinting #pnlLetterFields_Result").css("display") == "none") {
                    $("#designLetterPrinting #pnlLetterFields_Result").show();
                }

                designLetterPrinting.SearchLetterFields(0, LetterId).done(function (response) {
                    if (response.status != false) {
                        designLetterPrinting.LetterFieldsGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    LetterFieldsGridLoad: function (response) {
        $("#dgvLetterFields").dataTable().fnDestroy();
        $("#pnlLetterFields_Result #dgvLetterFields tbody").find("tr").remove();
        if (response.LetterFieldsCount > 0) {
            var LetterFieldsLoadJSONData = JSON.parse(response.LetterFieldsLoad_JSON);
            var LetterJSONData = JSON.parse(response.Letters_JSON);
            $.each(LetterFieldsLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvAppointmentStatus_row" + item.LtrFieldId + "'))");
                $row.attr("id", "gvAppointmentStatus_row" + item.LtrFieldId);
                $row.attr("LtrFieldId", item.LtrFieldId);
               

                var htmlString = response.HtmlDocument;
               // var textarray = htmltext.split('<p>');
                var $jQueryObject = $($.parseHTML(htmlString));
                var InsertedFieldsList = $jQueryObject.find(".FieldInserted_PK");

                var selectedids = [];
                var selectedtext = [];
                for (var i = 0; i < InsertedFieldsList.length ; i++) {

                    selectedids.push($(InsertedFieldsList[i]).attr('id'));
                    selectedtext.push($(InsertedFieldsList[i]).val());
                }

                for (var i = 0; i < selectedids.length ; i++) {
                    if (selectedids[i] == item.LtrFieldId) {
                        $row.append('<td style="display:none;">' + item.LtrFieldId + '</td><td>' + item.FieldName + '</td><td><input class="ManualFields" type="text" name="text" id="' + item.LtrFieldId + '" value="' + item.Description + '"></td>');
                    }
                }
                $("#printHTMlDiv").html(htmlString); 
                $("#pnlLetterFields_Result #dgvLetterFields tbody").last().append($row);
            });
        }
        else {
            $('#dgvLetterFields').DataTable({
                "language": {
                    "emptyTable": "No Letter Fields Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvLetterFields'))
            ;
        else
            $("#pnlLetterFields_Result #dgvLetterFields").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ValidatePrintLetter: function () {
        $('#frmLetterPrinting')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               excluded: ':disabled',
               fields: {

                   Letters: {
                       group: '.col-sm-3',
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
            designLetterPrinting.PrintLetter();
        });
    },

    PrintLetter: function () {
        $("#pnlLetterFields_Result #dgvLetterFields tbody .ManualFields").each(function () {
            $("#printHTMlDiv #" + this.id).attr("value", $("#pnlLetterFields_Result #dgvLetterFields tbody #" + this.id).val());
        });

        $("#printHTMlDiv").printMe();
         
        },

    FindPrinters: function () {
        //var data = "letterData=" + letterData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService("", "DESIGN_LETTERPRINTING", "FIND_PRINTERS");
    },

    SetPrinter: function (PrinterName) {

        designLetterPrinting.SetDefaultPrinter(PrinterName);
    },

    SetDefaultPrinter: function (PrinterName) {
        var data = "PrinterName=" + PrinterName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "DESIGN_LETTERPRINTING", "SET_PRINTER");
    },

    SearchLetterFields: function (LtrFieldId, LetterId) {
        var data = "LtrFieldId=" + LtrFieldId + "&LetterId=" + LetterId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "DESIGN_LETTER", "SEARCH_LETTER_FIELDS");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmLetterPrinting', function () {

            UnloadActionPan(designLetterPrinting.params["ParentCtrl"], "designLetterPrinting");

        }, function () {

            UnloadActionPan(designLetterPrinting.params["ParentCtrl"], "designLetterPrinting");

        });
    },
    
}
