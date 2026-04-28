//Author: Abid Ali
//Date: 01-04-2016
//This file will handle all actions performed to view PDF
Clinical_LabOrderView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",
    Load: function (params) {
        Clinical_LabOrderView.params = params;
        if (Clinical_LabOrderView.params != null && Clinical_LabOrderView.params.PanelID != "Clinical_LabOrderView") {
            Clinical_LabOrderView.params["PanelID"] = Clinical_LabOrderView.params["PanelID"] + ' #Clinical_LabOrderView';
        }
        else {
            Clinical_LabOrderView.params = [];
            Clinical_LabOrderView.params["PanelID"] = "Clinical_LabOrderView"
        }



        if (Clinical_LabOrderView.bIsFirstLoad) {
            Clinical_LabOrderView.bIsFirstLoad = false;
            var self = $('#' + Clinical_LabOrderView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Clinical_LabOrderView.LabOrderPreview(Clinical_LabOrderView.params.PatientId, Clinical_LabOrderView.params.UserId, Clinical_LabOrderView.params.LabOrderId);
            });
        }

    },
    //Function Name: LabOrderPreview
    //Author Name: Abid Ali
    //Created Date: 01-04-2016
    //Description: Creates PDF to view Radiology Order
    LabOrderPreview: function (patientID, userID, LabOrderId) {     
        Clinical_LabOrderView.previewLabOrder(patientID, userID, LabOrderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                Clinical_LabOrderView.pdf = response.LabOrderHTML;

                utility.documentPrint(response.LabOrderHTML);
            }
            else 
            {
                utility.DisplayMessages(response.Message, 2);
            }
           });
    },
    //Function Name: printLabOrder
    //Author Name: Abid Ali
    //Created Date: 01-04-2016
    //Description: Prints PDF
    printLabOrder: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            strMessage = "";
            if (strMessage == "") {                

                var raw = atob($('#helperPDF').val());

                var uint8Array = new Uint8Array(raw.length);
                for (var i = 0; i < raw.length; i++) {
                    uint8Array[i] = raw.charCodeAt(i);
                }
                var byteArray = new Uint8Array(uint8Array);

                var blob = new Blob([byteArray], { type: 'application/pdf' });
                var url = URL.createObjectURL(blob);
                //window.focus();
                var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
                var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
                var myWindow = window.open(url, "MsgWindow", 'width=' + width + ', height=' + height);              

                return true;
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
        if (LabOrderId.indexOf(',') > -1) {
            objData["LabOrderIDs"] = LabOrderId;
        } else {
            objData["LabOrderId"] = LabOrderId;
        }
        objData["commandType"] = "preview_LabOrder";
        if (Clinical_LabOrderView.params["BarCodeHtml"] != null) {
            objData["BarCodeHtml"] = Clinical_LabOrderView.params["BarCodeHtml"];
        }
        else
            objData["BarCodeHtml"] = true;
        var data = JSON.stringify(objData);
        
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },


    UnLoad: function () {


        if (Clinical_LabOrderView.params != null && Clinical_LabOrderView.params.ParentCtrl) {
            if (Clinical_LabOrderView.params.ParentCtrlPanelID == 'pnlClinicalProgressNote #pnlClinicalLabOrder') {
                UnloadActionPan(Clinical_LabOrderView.params.ParentCtrl, null, null, 'pnlClinicalProgressNote #pnlClinicalLabOrder');
            } else {
                UnloadActionPan(Clinical_LabOrderView.params.ParentCtrl);
            }

           
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
            Clinical_LabOrderView.loadLabTests_DBCall(LabTestsText.val()).done(function (response) {
                var response = JSON.parse(response);
                if (response.status != false) {
                    if (response.labTestCount > 0) {
                        var LabTests = JSON.parse(response.labTestList_JSON);

                        $.each(LabTests, function (i, item) {
                            allLabTests.push({ id: item.LOINC, value:item.LOINC+" "+ item.LOINCDescription,   });

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
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = "data:application/pdf;base64," + Clinical_LabOrderView.pdf;
        params["ParentCtrl"] = "Clinical_LabOrderView";
        LoadActionPan("Batch_FaxSend",params);
    }

    

}
