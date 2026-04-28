Clinical_FaceSheetComponentSelection = {
    bIsFirstLoad: true,
    params: [],
    PrintPDFDataURL: '',
    Load: function (params) {
        Clinical_FaceSheetComponentSelection.params = params;
        if (Clinical_FaceSheetComponentSelection.params != null && Clinical_FaceSheetComponentSelection.params.PanelID != "Clinical_FaceSheetComponentSelection") {
            Clinical_FaceSheetComponentSelection.params["PanelID"] = Clinical_FaceSheetComponentSelection.params["PanelID"] + ' #Clinical_FaceSheetComponentSelection';
        }
        else {
            Clinical_FaceSheetComponentSelection.params = [];
            Clinical_FaceSheetComponentSelection.params["PanelID"] = "Clinical_FaceSheetComponentSelection"
        }

        if (Clinical_FaceSheetComponentSelection.bIsFirstLoad) {
            Clinical_FaceSheetComponentSelection.bIsFirstLoad = false
            Clinical_FaceSheetComponentSelection.SelectedComponent = [];
            if (Clinical_FaceSheetComponentSelection.params.CheckBoxes != null) {
                $('#' + Clinical_FaceSheetComponentSelection.params["PanelID"] + ' #chkBoxes').append('<div class="col-sm-12 col-md-12 col-lg-12 mb-md"><div class="checkbox-custom"><input type="checkbox" id="chkComponentAll" onclick="Clinical_FaceSheetComponentSelection.AddToListAll(this);" name="Component" checked><label class="control-label"> Select All</label></div>');
                for (var chk = 0; chk < Clinical_FaceSheetComponentSelection.params.CheckBoxes.length; chk++) {
                    $('#' + Clinical_FaceSheetComponentSelection.params["PanelID"] + ' #chkBoxes').append('<div class="col-sm-6 col-md-4 col-lg-3 mb-md"><div class="checkbox-custom"><input type="checkbox" id="chkComponent" onclick="Clinical_FaceSheetComponentSelection.AddToList(this);" name="Component" checked><label class="control-label">' + Clinical_FaceSheetComponentSelection.params.CheckBoxes[chk] + '</label></div>');
                    Clinical_FaceSheetComponentSelection.SelectedComponent.push(Clinical_FaceSheetComponentSelection.params.CheckBoxes[chk]);
                }
            }
        }
    },
    AddToListAll: function (obj) {
        Clinical_FaceSheetComponentSelection.SelectedComponent = [];
        if ($(obj).prop('checked')) {
            $('#Clinical_FaceSheetComponentSelection #chkComponent').prop('checked', true);
            $.each($('#Clinical_FaceSheetComponentSelection #chkComponent'), function (i, item) {
                Clinical_FaceSheetComponentSelection.SelectedComponent.push($(item).siblings().text());
            });
        } else {
            $('#Clinical_FaceSheetComponentSelection #chkComponent').prop('checked', false);

        }
    },
    AddToList: function (chk) {
        if ($('#Clinical_FaceSheetComponentSelection #chkComponent:checked').length == $('#Clinical_FaceSheetComponentSelection #chkComponent').length) {
            $('#Clinical_FaceSheetComponentSelection #chkComponentAll').prop('checked', true);
        } else {
            $('#Clinical_FaceSheetComponentSelection #chkComponentAll').prop('checked', false);
        }
        if (Clinical_FaceSheetComponentSelection.params["ParentCtrl"] == "Clinical_FaceSheetView") {
            var ComponentName = $(chk).parent().text();
            if ($(chk).prop("checked") == true) {
                if (Clinical_FaceSheetComponentSelection.SelectedComponent.indexOf(ComponentName) == -1) {
                    Clinical_FaceSheetComponentSelection.SelectedComponent.push(ComponentName);
                }
            }
            else {
                var charIndex = Clinical_FaceSheetComponentSelection.SelectedComponent.indexOf(ComponentName)
                if (charIndex > -1) {
                    Clinical_FaceSheetComponentSelection.SelectedComponent.splice(charIndex, 1);
                }
            }
        }
    },

    UnLoad: function () {       
        if (Clinical_FaceSheetComponentSelection.params != null && Clinical_FaceSheetComponentSelection.params.ParentCtrl) {
            UnloadActionPan(Clinical_FaceSheetComponentSelection.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }

        Clinical_FaceSheetComponentSelection.PrintComponent();      
    },

    PrintComponent: function () {        

        Clinical_FaceSheetComponentSelection.LoadPrintComponents().done(function (response) {
            response = JSON.parse(response);
            utility.PDFViewer(response.FaceSheetHTML, true, null, null, true);
            Clinical_FaceSheetComponentSelection.SelectedComponent = null;
        });
    },

    LoadPrintComponents: function () {

        var objData = {};
        objData["PatientId"] = Clinical_FaceSheetComponentSelection.params["PatientId"];
        objData["PrintComponents"] = Clinical_FaceSheetComponentSelection.SelectedComponent;
        objData["commandType"] = "getclinicalsummaryprintcomponents";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FaceSheet", "FaceSheet");        
    },
}
