//Author: Abid Ali
//Date: 01-04-2016
//This file will handle all actions performed to view PDF
Clinical_LabOrderABNView = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_LabOrderABNView.params = params;
        if (Clinical_LabOrderABNView.params != null && Clinical_LabOrderABNView.params.PanelID != "Clinical_LabOrderABNView") {
            Clinical_LabOrderABNView.params["PanelID"] = Clinical_LabOrderABNView.params["PanelID"] + ' #Clinical_LabOrderABNView';
        }
        else {
            Clinical_LabOrderABNView.params = [];
            Clinical_LabOrderABNView.params["PanelID"] = "Clinical_LabOrderABNView"
        }



        if (Clinical_LabOrderABNView.bIsFirstLoad) {
            Clinical_LabOrderABNView.bIsFirstLoad = false;
            var self = $('#' + Clinical_LabOrderABNView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Clinical_LabOrderABNView.LabOrderPreview(Clinical_LabOrderABNView.params.PatientId, Clinical_LabOrderABNView.params.UserId, Clinical_LabOrderABNView.params.LabOrderId);
            });
        }
    },
    //Function Name: LabOrderPreview
    //Author Name: Abid Ali
    //Created Date: 01-04-2016
    //Description: Creates PDF to view Radiology Order
    LabOrderPreview: function (patientID, userID, LabOrderId) {
        Clinical_LabOrderABNView.previewLabOrder(patientID, userID, LabOrderId).done(function (response) {
            response = JSON.parse(response);
            utility.PDFViewer(response.LabOrderHTML, false, 'Clinical_LabOrderABNView #PreviewLabOrderForm', true);

        });
    },
    //Function Name: printLabOrder
    //Author Name: Abid Ali
    //Created Date: 01-04-2016
    //Description: Prints PDF
    printLabOrderABN: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#Clinical_LabOrderABNView #PreviewLabOrderForm")[0].contentWindow.focus();
                $("#Clinical_LabOrderABNView #PreviewLabOrderForm")[0].contentWindow.print();

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Function Name: previewLabOrder
    //Author Name: Abid Ali
    //Created Date: 01-04-2016
    //Description: DB call to view PDF
    previewLabOrder: function (patientID, userID, LabOrderId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["LabOrderId"] = LabOrderId;
        objData["commandType"] = "preview_LabOrderABN";
        objData["Tests"] = Clinical_LabOrderABNView.params["Tests"];
        if (Clinical_LabOrderABNView.params["BarCodeHtml"] != null) {
            objData["BarCodeHtml"] = Clinical_LabOrderABNView.params["BarCodeHtml"];
        }
        else
            objData["BarCodeHtml"] = '';
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },


    UnLoad: function () {

        if (Clinical_LabOrderABNView.params != null && Clinical_LabOrderABNView.params.ParentCtrl) {
            UnloadActionPan(Clinical_LabOrderABNView.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },

    // AutoComplete for Lab test
    // Author: Azhar | Date: Sep 01, 2016
    BindLabTestText: function (PanelID, control, hfField) {
        var LabTestsText = $(control);
        var allLabTests = [];
        if (LabTestsText.val().length > 2) {
            utility.Keyupdelay(function () {
                Clinical_LabOrderABNView.loadLabTests_DBCall(LabTestsText.val()).done(function (response) {
                    var response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.labTestCount > 0) {
                            var LabTests = JSON.parse(response.labTestList_JSON);

                            $.each(LabTests, function (i, item) {
                                allLabTests.push({ id: item.LOINC, value: item.LOINC + " " + item.LOINCDescription, });

                            });
                            LabTestsText.autocomplete({
                                autoFocus: true,
                                source: allLabTests,
                                select: function (event, ui) {
                                    setTimeout(function () {
                                        $('#' + PanelID + " #" + hfField).val(ui.item.id);
                                    }, 100);
                                }
                            });
                            LabTestsText.autocomplete("search");

                        } else {
                            $('#' + PanelID + " #" + hfField).val('');
                        }
                    } else {
                        $('#' + PanelID + " #" + hfField).val('');
                    }

                });
            });
        } else {
            $('#' + PanelID + " #" + hfField).val('');
        }

    },
    //BindLabTestText: function (PanelID, control, hfField) {
    //    var LabTestsText = $(control);
    //    LabTestsText.autocomplete({
    //        minLength: 1,
    //        source: function (request, response) {
    //            var objData = new Object();
    //            objData["Test"] = $(control).val();
    //            objData["commandType"] = "LOOKUP_LABTEST_REPORT";
    //            var datajson = JSON.stringify(objData);
    //            $.ajax({
    //                url: "api/LabOrder/LabOrder",
    //                data: datajson,
    //                dataType: "json",
    //                type: "POST",
    //                beforeSend: function () {

    //                    BackgroundLoaderShow(true);
    //                },
    //                success: function (response) {
    //                    BackgroundLoaderShow(false);
    //                    var response = JSON.parse(response);
    //                    response($.map(response.labTestList_JSON, function (obj) {
    //                        return {
    //                            label: obj.name,
    //                            value: obj.name,
    //                            description: obj.description,
    //                            id: obj.name // don't really need this unless you're using it elsewhere.
    //                        };
    //                    }));
    //                }

    //            });
    //        }
    //    }).data("autocomplete")._renderItem = function (ul, item) {
    //        // Inside of _renderItem you can use any property that exists on each item that we built
    //        // with $.map above */
    //        return $("<li></li>")
    //            .data("item.autocomplete", item)
    //            .append("<a>" + item.label + "<br>" + item.description + "</a>")
    //            .appendTo(ul);
    //    };
    //},
    loadLabTests_DBCall: function (LabTestsText) {
        var objData = new Object();
        objData["Test"] = LabTestsText;
        objData["commandType"] = "LOOKUP_LABTEST_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },
}
