Encounter_NDCSelection = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Encounter_NDCSelection.params = params;
        Encounter_NDCSelection.params.DeleteProblem = [];
        if (Encounter_NDCSelection.params.PanelID != 'pnlEncounter_NDCSelection') {
            Encounter_NDCSelection.params.PanelID = Encounter_NDCSelection.params.PanelID + ' #pnlEncounter_NDCSelection';
        }
        else {
            Encounter_NDCSelection.params.PanelID = 'pnlEncounter_NDCSelection';
        }
        Encounter_NDCSelection.LoadNDC()
    },
    LoadNDC: function () {
        $.each(Encounter_NDCSelection.params.NDCDetail, function (i, item) {
            var row = "<tr>";
            row += '<td><input type="radio" name="NDC" value="' + item.CPTNdcId + '"></td>';
            row += "<td>"+item.NDCCode+"</td>";
            row += "<td>"+item.NDCDescription+"</td>";
            row += "<td>"+item.Unit+"</td>";
            row += "<td>"+item.UnitPrice+"</td>";
            row += "<td>" + item.NDCMeasurementText + "</td>";
            row += "</tr>";
            $("#" + Encounter_NDCSelection.params.PanelID + " #dgvNdcInfo tbody").append(row);
        });
    },
    bindNDC:function(){
        var NDCId=$("#" + Encounter_NDCSelection.params.PanelID + " #dgvNdcInfo tbody").find('input[name=NDC]:checked').val()
        var SelectedNDC = $.grep(Encounter_NDCSelection.params.NDCDetail, function (n, i) {
            return n.CPTNdcId == NDCId;
        });
        if (SelectedNDC.length > 0) {
            
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr#Child" + Encounter_NDCSelection.params.CtrlId).find('input[id*="txtNDC"]').val(SelectedNDC[0].NDCCode);
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr#Child" + Encounter_NDCSelection.params.CtrlId).find('input[id*="txtNDCDescription"]').val(SelectedNDC[0].NDCDescription);
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr#Child" + Encounter_NDCSelection.params.CtrlId).find('input[id*="txtNDCUnit"]').val(SelectedNDC[0].Unit);
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr#Child" + Encounter_NDCSelection.params.CtrlId).find('input[id*="txtNDCUnitPrice"]').val(SelectedNDC[0].UnitPrice);
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr#Child" + Encounter_NDCSelection.params.CtrlId).find('select[id*="ddlNDCMeasurement"]').val(SelectedNDC[0].NDCMeasurementId).attr("selected", "selected");
            Encounter_NDCSelection.UnLoad();
        }
    },
    UnLoad: function () {
        if (Encounter_NDCSelection.params.ParentCtrl && Encounter_NDCSelection.params.ParentCtrl != "")
            UnloadActionPan(Encounter_NDCSelection.params.ParentCtrl, 'Encounter_NDCSelection');
    }
}