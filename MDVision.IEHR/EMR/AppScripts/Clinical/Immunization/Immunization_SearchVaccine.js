Immunization_SearchVaccine = {
    bIsFirstLoad: true,
    params: [],

    NoteId: 0,
    Load: function (params) {

        Immunization_SearchVaccine.params = params;
        if (Immunization_SearchVaccine.params.PanelID != 'pnlImmunization_SearchVaccine') {
            Immunization_SearchVaccine.params.PanelID = Immunization_SearchVaccine.params.PanelID + ' #pnlImmunization_SearchVaccine';
        } else {
            Immunization_SearchVaccine.params.PanelID = 'pnlImmunization_SearchVaccine';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Immunization_SearchVaccine.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        Immunization_SearchVaccine.VaccinesSearch();
    },
    VaccinesSearch: function (VaccineId, PageNo, rpp) {
        Immunization_SearchVaccine.SearchVaccine(VaccineId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Immunization_SearchVaccine.VaccineGridLoad(response);
                var TableControl = "pnlImmunization_SearchVaccine #dgvClinicalVaccine";
                var PagingPanelControlID = "pnlImmunization_SearchVaccine #divClinicalVaccinePaging";
                var ClassControlName = "Immunization_SearchVaccine";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.VaccineCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (VaccineId, PageNumber, ResultPerPage) {
                    Immunization_SearchVaccine.VaccinesSearch(VaccineId, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    //NotesSearch
    VaccineGridLoad: function (response) {
        $("#pnlImmunization_SearchVaccine #dgvClinicalVaccine").dataTable().fnDestroy();
        $("#pnlImmunization_SearchVaccine #pnlVaccine_Result #dgvClinicalVaccine tbody").find("tr").remove();
        if (response.VaccineCount > 0) {
            //EMR-602 fix
            $('#pnlImmunization_SearchVaccine #PreviousNotediv').removeClass('disableAll')
            var VaccineLoadJSONData = JSON.parse(response.VaccineLoad_JSON);
            $.each(VaccineLoadJSONData, function (i, item) {
                var ClassDisabled = item.VaccineStatus.toLowerCase() == "active" ? "" : "disabled";
                var selectMethod = "Immunization_SearchVaccine.SelectVaccine('" + item.VaccineId + "','" + item.FullDescription + "','" + item.CVXCode + "','" + item.CPTCode + "',event)"
                var $row = $('<tr/>');
                var selectVaccine = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                var selectAction = "";
                if (item.VaccineStatus.toLowerCase() == "active") {
                    $row.attr("onclick", selectMethod);
                    var link = $('<a class="btn btn-xs' + ClassDisabled + '" href="#" title="Select Record"><i class="fa fa-check black"></i></a>');
                    link.attr("onclick", selectMethod);
                    selectAction = link[0].outerHTML + '&nbsp;';
                }

                $row.attr("id", item.VaccineId);
                $row.attr("VaccineId", item.VaccineId);
                $row.append('<td style="display:none;">' + item.VaccineId + '</td><td>' + selectAction + '</td><td>' + item.FullDescription + '</td><td>' + item.CVXCode + '</td><td>' + item.CPTCode + '</td>');
                //<a class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesActiveInactive(' + item.NotesId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>
                $("#pnlVaccine_Result #dgvClinicalVaccine tbody").last().append($row);
            });
        }
        else {

            $('#pnlImmunization_SearchVaccine #dgvClinicalVaccine').DataTable({
                "language": {
                    "emptyTable": "No Vaccine is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        //if ($.fn.dataTable.isDataTable('#pnlImmunization_SearchVaccine #dgvClinicalVaccine'))
        //    ;
        //else {
        //    $("#pnlImmunization_SearchVaccine #pnlVaccine_Result #dgvClinicalVaccine").DataTable({ "bInfo": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //}
    },
    SelectVaccine: function (VaccineId, FullVaccineName, CVXCode, CPTCode, event) {
        if (event != null) {
            event.stopPropagation();
        }
        //$("#" + Immunization_SearchVaccine.params.ParentCtrl.params.PanelID + " #txtVaccine").val(CVXCode + "-" + Vaccine);
        //$("#" + Immunization_SearchVaccine.params.ParentCtrl.params.PanelID + " #txtVaccineId").val(VaccineId);
        //Immunization_SearchVaccine.params.ParentCtrl.SearchManufacturer(CVXCode, ManufacturerId);
        var RefCtrl = null;
        var RefCtrlHidden = null;
        var RefCVXCtrl = null;
        var RefCPTCtrl = null;
        var RefCtrlLink = null;
        var RefCtrlLabel = null;
        if (Immunization_SearchVaccine.params["RefCtrl"] != null)
            RefCtrl = "#" + Immunization_SearchVaccine.params["RefCtrl"];
        if (Immunization_SearchVaccine.params["RefHiddenCtrl"] != null)
            RefCtrlHidden = "#" + Immunization_SearchVaccine.params["RefHiddenCtrl"];
        if (Immunization_SearchVaccine.params["RefCtrlLabel"] != null)
            RefCtrlLabel = "#" + Immunization_SearchVaccine.params["RefCtrlLabel"];
        if (Immunization_SearchVaccine.params["RefCtrlLink"] != null)
            RefCtrlLink = "#" + Immunization_SearchVaccine.params["RefCtrlLink"];
        if (Immunization_SearchVaccine.params["RefCtrl"] != null)
            RefCtrl = "#" + Immunization_SearchVaccine.params["RefCtrl"];
        if (Immunization_SearchVaccine.params["RefCVXCtrl"] != null)
            RefCVXCtrl = "#" + Immunization_SearchVaccine.params["RefCVXCtrl"];
        if (Immunization_SearchVaccine.params["RefCPTCtrl"] != null)
            RefCPTCtrl = "#" + Immunization_SearchVaccine.params["RefCPTCtrl"];
        if (Immunization_SearchVaccine.params.ParentCtrl && Immunization_SearchVaccine.params.ParentCtrl == "Immunization_VaccineCrosswalk" || Immunization_SearchVaccine.params.ParentCtrl == "Immunization_LotNumberDetail") {
            if (RefCtrl)
                $(RefCtrl).val(FullVaccineName).focus();
            if (RefCtrlHidden)
                $(RefCtrlHidden).val(VaccineId);
            if (RefCVXCtrl)
                $(RefCVXCtrl).val(CVXCode);
            if (RefCPTCtrl)
                $(RefCPTCtrl).val(CPTCode);
            if (!$(RefCtrlLabel).hasClass("hidden")) {
                $(RefCtrlLabel).addClass('hidden');
            }
            if ($(RefCtrlLink).hasClass("hidden")) {
                $(RefCtrlLink).removeClass('hidden');
            }
        }
        if (Immunization_SearchVaccine.params["ParentCtrl"] != null && Immunization_SearchVaccine.params["ParentCtrl"] == "Immunization_LotNumberDetail") {
            $.when(Immunization_LotNumberDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown()).then(function () {
                Immunization_SearchVaccine.UnLoadTab();
            });
        }
        else {
            Immunization_SearchVaccine.UnLoadTab();
        }
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_SearchVaccine.params["FromAdmin"] == "0") {
            if (Immunization_SearchVaccine.params != null && Immunization_SearchVaccine.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_SearchVaccine.params.ParentCtrl, 'Immunization_SearchVaccine');
            }
            else
                UnloadActionPan(null, 'Immunization_SearchVaccine');
        }
        else {

            RemoveAdminTab();
        }
        return objDeffered;
    },
    SearchVaccine: function (VaccineId, PageNumber, RowsPerPage, VaccineGroupID) {
        var objData = new Object();
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        if (Immunization_SearchVaccine.params["VaccineGroupID"]) {
            VaccineGroupID = Immunization_SearchVaccine.params["VaccineGroupID"];
        }
        objData["VaccineGroupID"] = VaccineGroupID;
        if (Immunization_SearchVaccine.params["VaccineId"])
            objData["VaccineId"] = Immunization_SearchVaccine.params["VaccineId"];
        else
            objData["VaccineId"] = VaccineId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["SearchText"] = "";
        objData["commandType"] = "get_all_Vaccines";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Vaccines");
    },
}