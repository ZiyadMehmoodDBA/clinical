//Author: Humaira Yousaf
//Date: 23-03-2016
//This file will handle all actions performed to view PDF
Clinical_RadiologyOrderView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",

    Load: function (params) {
        Clinical_RadiologyOrderView.params = params;
        if (Clinical_RadiologyOrderView.params != null && Clinical_RadiologyOrderView.params.PanelID != "Clinical_RadiologyOrderView") {
            Clinical_RadiologyOrderView.params["PanelID"] = Clinical_RadiologyOrderView.params["PanelID"] + ' #Clinical_RadiologyOrderView';
        }
        else {
            Clinical_RadiologyOrderView.params = [];
            Clinical_RadiologyOrderView.params["PanelID"] = "Clinical_RadiologyOrderView"
        }

        if (Clinical_RadiologyOrderView.bIsFirstLoad) {
            Clinical_RadiologyOrderView.bIsFirstLoad = false;
            var self = $('#' + Clinical_RadiologyOrderView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Clinical_RadiologyOrderView.radiologyOrderPreview(Clinical_RadiologyOrderView.params.PatientId, Clinical_RadiologyOrderView.params.UserId, Clinical_RadiologyOrderView.params.RadiologyOrderId);
            });
        }
    },
    //Function Name: radiologyOrderPreview
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Creates PDF to view Radiology Order
    radiologyOrderPreview: function (patientID, userID, radiologyOrderId) {
        Clinical_RadiologyOrderView.previewRadiologyOrder(patientID, userID, radiologyOrderId).done(function (response) {
            response = JSON.parse(response);
            Clinical_RadiologyOrderView.pdf = response.RadiologyOrderHTML;
            //utility.PDFViewer(response.RadiologyOrderHTML, false, 'Clinical_RadiologyOrderView #PreviewRadiologyOrderForm', true);
            utility.documentPrint(response.RadiologyOrderHTML);
        });
    },
    //Function Name: printRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Prints PDF
    printRadiologyOrder: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //$("#" + Clinical_RadiologyOrderView.params["PanelID"] + " #PreviewRadiologyOrderForm")[0].contentWindow.focus();
                //$("#" + Clinical_RadiologyOrderView.params["PanelID"] + " #PreviewRadiologyOrderForm")[0].contentWindow.print();
                //$("#" + Clinical_RadiologyOrderView.params["PanelID"] + " #PreviewRadiologyOrderForm")[0].contentWindow.close();

        var raw = atob($('#helperPDF').val());

        var uint8Array = new Uint8Array(raw.length);
        for (var i = 0; i < raw.length; i++) {
            uint8Array[i] = raw.charCodeAt(i);
        }
        var byteArray = new Uint8Array(uint8Array);

        var blob = new Blob([byteArray], { type: 'application/pdf' });
        var url = URL.createObjectURL(blob);

        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        
        var myWindow = window.open(url, "MsgWindow", 'width=' + width + ', height=' + height);
        myWindow.focus();
        myWindow.print();
        return false;
        

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Function Name: previewRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: DB call to view PDF
    previewRadiologyOrder: function (patientID, userID, radiologyOrderId, mode, isIncludeComments) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["RadiologyOrderId"] = radiologyOrderId;
        objData["Mode"] = mode;
        objData["IsIncludeComments"] = isIncludeComments;
        objData["commandType"] = "preview_radiologyOrder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },


    UnLoad: function () {

        if (Clinical_RadiologyOrderView.params["FromAdmin"] == "0") {
            if (Clinical_RadiologyOrderView.params != null && Clinical_RadiologyOrderView.params.ParentCtrl != null) {
                if (Clinical_RadiologyOrderView.params.ParentCtrl == 'clinicalTabRadiologyOrder') {
                    UnloadActionPan(Clinical_RadiologyOrderView.params["ParentCtrl"], "Clinical_RadiologyOrderView");
                } else {
                    Clinical_RadiologyOrderView.params.PanelID = Clinical_RadiologyOrderView.params.PanelID.replace(" #Clinical_RadiologyOrderView", "");
                    UnloadActionPan(Clinical_RadiologyOrderView.params.ParentCtrl, 'Clinical_RadiologyOrderView', null, Clinical_RadiologyOrderView.params.PrPanelID);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_RadiologyOrderView');
        }
        else {
            RemoveAdminTab();
        }
    },

    BindRadiologyLabTestText: function (PanelID, control, hfField) {
        var RadiologyLabTestsText = $(control);
        var allRadiologyLabTests = [];
        if (RadiologyLabTestsText.val().length > 2) {
            utility.Keyupdelay(function () {
                Clinical_RadiologyOrderView.loadRadiologyLabTests_DBCall(RadiologyLabTestsText.val()).done(function (response) {
                    var response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.radiologyTestCount > 0) {
                            var RadiologyLabTests = JSON.parse(response.radiologyTestList_JSON);

                            $.each(RadiologyLabTests, function (i, item) {
                                allRadiologyLabTests.push({ id: item.LOINC, value: item.LOINC + " " + item.LOINCDescription, });

                            });
                            RadiologyLabTestsText.autocomplete({
                                autoFocus: true,
                                source: allRadiologyLabTests,
                                select: function (event, ui) {
                                    setTimeout(function () {
                                        $('#' + PanelID + " #" + hfField).val(ui.item.id);
                                    }, 100);
                                }
                            });
                            RadiologyLabTestsText.autocomplete("search");

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
    loadRadiologyLabTests_DBCall: function (RadiologyLabTestsText) {
        var objData = new Object();
        objData["Test"] = RadiologyLabTestsText;
        objData["commandType"] = "LOOKUP_RADIOLOGYLABTEST_REPORT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = "data:application/pdf;base64," + Clinical_RadiologyOrderView.pdf;
        params["ParentCtrl"] = "Clinical_RadiologyOrderView";
        LoadActionPan("Batch_FaxSend", params);
    }
}
