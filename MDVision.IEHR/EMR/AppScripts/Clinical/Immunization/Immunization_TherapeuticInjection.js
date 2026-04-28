Immunization_TherapeuticInjection = {
    bIsFirstLoad: true,
    params: [],
    IsChangeTherapeuticInjection: false,
    FavListName: 'ClinicalTherapeuticInjection',
    Load: function (params) {
        Immunization_TherapeuticInjection.params = params;
        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#pnlClinicalImmunization #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            Immunization_TherapeuticInjection.params.PatientId = $('#PatientProfile #hfPatientId').val();
        }

        if (Immunization_TherapeuticInjection.params.PanelID != 'pnlClinicalTherapeuticInjection') {
            Immunization_TherapeuticInjection.params.PanelID = Immunization_TherapeuticInjection.params.PanelID + ' #pnlClinicalTherapeuticInjection';
        } else {
            Immunization_TherapeuticInjection.params.PanelID = 'pnlClinicalTherapeuticInjection';
        }

        Immunization_TherapeuticInjection.CreatingDatePickers();
        Immunization_TherapeuticInjection.CreatingTimePickers();
        var self = $('#' + Immunization_TherapeuticInjection.params.PanelID);
        if (Immunization_TherapeuticInjection.bIsFirstLoad == true) {
            Immunization_TherapeuticInjection.ValidateTherapeuticInjectionAdminister();
            Immunization_TherapeuticInjection.ValidateTherapeuticInjectionHistory();
            self.loadDropDowns(true).done(function () {
                EMRUtility.setFavoriteSectionStyle(Immunization_TherapeuticInjection.params.PanelID);
                //Immunization_TherapeuticInjection.LoadAllAutocomplete();
                $.when(Immunization_TherapeuticInjection.PopulateVisitDate()).then(function () {
                    $.when(Immunization_TherapeuticInjection.setPatientProvider()).then(function () {
                        if (Immunization_TherapeuticInjection.params.mode == "Edit") {
                            Immunization_TherapeuticInjection.ActiveInactiveTab(Immunization_TherapeuticInjection.params.Type);
                            $.when(Immunization_TherapeuticInjection.LoadTherapeuticInjection(Immunization_TherapeuticInjection.params.ImmTherInjectionId)).then(function () {
                                Immunization_TherapeuticInjection.LoadFavTherapuetic(true);
                            });
                            if (Immunization_TherapeuticInjection.params.Type == "Administered") {
                                if (!$('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjectionHistory").hasClass('disableAll')) {
                                    $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjectionHistory").addClass('disableAll');
                                }
                                if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").hasClass('disableAll')) {
                                    $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").removeClass('disableAll');
                                }
                            } else {
                                if (!$('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").hasClass('disableAll')) {
                                    $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").addClass('disableAll');
                                }
                                if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjectionHistory").hasClass('disableAll')) {
                                    $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjectionHistory").removeClass('disableAll');
                                }
                            }
                        }
                        else {
                            if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjectionHistory").hasClass('disableAll')) {
                                $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjectionHistory").removeClass('disableAll');
                            }
                            if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").hasClass('disableAll')) {
                                $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").removeClass('disableAll');
                            }
                            Immunization_TherapeuticInjection.LoadFavTherapuetic(true);
                        }
                    });
                });
            });
        }
        else {
            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").attr('disabled', false);
            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #listAdministerInjection").attr('disabled', false);

        }

        if (EMRUtility.getFavListStatus(Immunization_TherapeuticInjection.FavListName)) {
            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #favSectionDiv").addClass("toggledHor");
            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #favSectionDiv").removeClass("toggledHor");
            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #FormDiv").removeClass("toggleHorContainer");
        }
    },

    LoadFavTherapuetic: function (ComeFormLoadFuntion) {
        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ulFavVaccine li').remove();
        var Tab = Immunization_TherapeuticInjection.AdministerText;
        var CtrlId = "#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider";
        if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
            Tab = "listAdministerInjection";
            CtrlId = "#frmTherapeuticInjection #ddlProvider";
        }
        else if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjectionHistory") {
            Tab = "listAdministerInjectionHistory";
            CtrlId = "#frmTherapeuticInjectionHistory #ddlProviderHistory";
        }


        var dfd = $.Deferred();
        var self = $('#' + Immunization_TherapeuticInjection.params.PanelID);
        self.find('.Favorites > select').attr('ddlist', 'GetFavVaccine');
        var data = "IsActive=&StrID=" + $(CtrlId).val() + "&StrID2=" + Tab + "&StrID3=therapuetic&ID=-1";
        self.find('.Favorites').loadDropDowns(true, data).done(function () {
            if (typeof ComeFormLoadFuntion == typeof undefined && ComeFormLoadFuntion == null) {
                if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
                    $.when(Immunization_TherapeuticInjection.PopulateLotNumber($("#" + Immunization_TherapeuticInjection.params.PanelID + " #hfTherapeuticInjection").val(), ComeFormLoadFuntion)).then(function () {
                        //$.when(Immunization_TherapeuticInjection.SetLotManufanucture($("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val())).then(function () {
                        dfd.resolve();
                        //});
                    });
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                if (typeof ComeFormLoadFuntion != typeof undefined && ComeFormLoadFuntion != null && ComeFormLoadFuntion == true) {
                    Immunization_TherapeuticInjection.SetFavListVal($("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlFavorites"));
                }
                dfd.resolve();
            }
        });
        return dfd;
    },
    SetFavListVal: function ($ddl) {

        var FavOptionLength = $("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlFavorites option").length;

        if (FavOptionLength > 1) {

            EMRUtility.getFavListValue(Immunization_TherapeuticInjection.FavListName).done(function (response1) {

                response1 = JSON.parse(response1);

                if (response1.status != false) {

                    if (response1.favListVal != "") {

                        if ($("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlFavorites option[value='" + response1.favListVal + "']").length > 0) {
                            $ddl.val(response1.favListVal);
                            $ddl.trigger("onchange");
                        }
                        else {
                            if (FavOptionLength == 2) {
                                $ddl.val($("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlFavorites option:nth-child(2)").val());
                                $ddl.trigger("onchange");
                            }
                            else if (FavOptionLength > 2) {
                                $ddl.trigger("onchange");
                            }
                            else {
                                $ddl.trigger("onchange");
                            }
                        }
                    }
                    else {
                        if (FavOptionLength == 2) {
                            $ddl.val($("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlFavorites option:nth-child(2)").val());
                            $ddl.trigger("onchange");
                        }
                        else if (FavOptionLength > 2) {
                            $ddl.trigger("onchange");
                        }
                        else {
                            $ddl.trigger("onchange");
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    AutoSearchFavTherapuetic: function () {
        utility.Keyupdelay(function () {
            Immunization_TherapeuticInjection.FavTherapueticChange(null);
        });
    },

    FavTherapueticChange: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlFavorites');
        }
        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ulFavVaccine li').remove();
        if ($(obj).val() != "") {
            Immunization_TherapeuticInjection.LoadFavImmunization_Vaccine_DBCALL($(obj).val(), $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #FavSearchBox').val()).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.FavImmunizationCount > 0) {
                        $.each(response.FavImmunization_JSON, function (i, item) {
                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ulFavVaccine').append('<li id=' + item.InjectionId + ' onclick="Immunization_TherapeuticInjection.SelectFavTherapuetic(this)">' + item.VaccineName + '</li>');
                        });
                    }
                }
            });
        }
    },
    LoadFavImmunization_Vaccine_DBCALL: function (FavId, SearchData) {
        var Tab = Immunization_TherapeuticInjection.AdministerText;

        if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
            Tab = Immunization_TherapeuticInjection.AdministerText;
        }
        else if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjectionHistory") {
            Tab = Immunization_TherapeuticInjection.DocumentHxDoseText;
        }


        var objData = new Object();
        objData["commandType"] = "Load_Fav_Immunization_Vaccine_Detail";
        objData["FavoritiesListId"] = FavId;
        objData["Tab"] = Tab;
        objData["SearchData"] = SearchData;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },
    LoadTherapeuticInjection: function (ImmTherInjectionId) {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Immunization_TherapeuticInjection.searchImmunizationTherapeuticInjection(ImmTherInjectionId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        if (response.TherapeuticInjectionCount > 0) {
                            var TherInjectionDetails = JSON.parse(response.TherapeuticInjectionLoad_JSON)[0];

                            if (Immunization_TherapeuticInjection.params.Type == "Administered") {
                                var self = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection");
                                utility.bindMyJSONByName(true, TherInjectionDetails, false, self).done(function () {
                                    $.when(Immunization_TherapeuticInjection.PopulateLotNumber(TherInjectionDetails.TherapeuticInjectionId)).then(function () {


                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection #txtTherapeuticInjection").val((TherInjectionDetails.CPTCode != "" ? TherInjectionDetails.CPTCode + ' - ' : "") + TherInjectionDetails.TherapeuticInjection);

                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpAdministrationDate').val(TherInjectionDetails.AdministrationDate);
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpAdministrationDate').datepicker('setDate', $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpAdministrationDate').val());
                                        if (TherInjectionDetails.ExpiryDate != "") {
                                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').val(TherInjectionDetails.ExpiryDate)
                                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').datepicker('setDate', $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').val());
                                        }


                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTime').val(TherInjectionDetails.AdministrationTime);
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTime').timepicker('setTime', $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTime').val());
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection #ddlVisitDate").find('option').each(function () {
                                            if (typeof $(this).val() != 'undefined' && $(this).val() != null && $(this).val() != "") {
                                                if ($(this).text() == TherInjectionDetails.VisitDate) {
                                                    $(this).attr('selected', 'selected');
                                                    return;
                                                }
                                            }
                                        });
                                        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;
                                        if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber option[value='" + TherInjectionDetails.LotNumber + "']").length == 0) {
                                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlLotNumber').append('<option selected value="' + TherInjectionDetails.LotNumber + '">' + TherInjectionDetails.LotText + '</option>');
                                        }
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber").val(TherInjectionDetails.LotNumber);
                                        if (TherInjectionDetails.LinkedWithAnyNote == "True") {
                                            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").attr("disabled", true);
                                        }
                                        else {
                                            $('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").attr("disabled", false);
                                        }

                                        dfd.resolve();
                                    });
                                });

                            }
                            else if (Immunization_TherapeuticInjection.params.Type == "DocumentHx") {
                                var self = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjectionHistory");
                                utility.bindMyJSONByName(true, TherInjectionDetails, false, self).done(function () {
                                    $('#' + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjectionHistory #txtTherapeuticInjectionHistory").val(TherInjectionDetails.CPTCode + ' - ' + TherInjectionDetails.TherapeuticInjection);
                                    if (TherInjectionDetails.AdministrationDateHistory != "") {
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpAdministrationDateHistory').val(TherInjectionDetails.AdministrationDateHistory);
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpAdministrationDateHistory').datepicker('setDate', $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpAdministrationDateHistory').val());
                                    }


                                    if (TherInjectionDetails.AdministrationTimeHistory != "") {
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTimeHistory').val(TherInjectionDetails.AdministrationTimeHistory);
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTimeHistory').timepicker('setTime', $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTimeHistory').val());
                                    }

                                    Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;

                                    if (TherInjectionDetails.LinkedWithAnyNote == "True") {
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").attr("disabled", true);
                                    }
                                    else {
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").attr("disabled", false);
                                    }

                                    dfd.resolve();
                                });
                            }
                        }
                        else {
                            dfd.resolve();
                        }
                    } else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                dfd.resolve();
                utility.DisplayMessages(strMessage, 2);
            }
        });
        return dfd;
    },

    SelectFavTherapuetic: function (obj) {

        var controlTxtId = "#txtTherapeuticInjection";
        var controlhfId = "#hfTherapeuticInjection";
        var formId = "frmTherapeuticInjection";
        var controlName = "TherapeuticInjection";
        if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
            controlTxtId = "#txtTherapeuticInjection";
            controlhfId = "#hfTherapeuticInjection";
            formId = "frmTherapeuticInjection";
            controlName = "TherapeuticInjection";
        }
        else if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjectionHistory") {
            controlTxtId = "#txtTherapeuticInjectionHistory";
            controlhfId = "#hfTherapeuticInjectionHistory";
            formId = "frmTherapeuticInjectionHistory";
            controlName = "TherapeuticInjectionHistory";

        }

        $('#' + Immunization_TherapeuticInjection.params.PanelID + " " + controlTxtId).val($(obj).text());
        $('#' + Immunization_TherapeuticInjection.params.PanelID + " " + controlhfId).val($(obj).attr("id"));
        utility.SetAutoCompleteSource($('#' + Immunization_TherapeuticInjection.params.PanelID + " " + controlTxtId), $('#' + Immunization_TherapeuticInjection.params.PanelID + " " + controlhfId));
        $("#" + Immunization_TherapeuticInjection.params.PanelID + " #" + formId).bootstrapValidator('revalidateField', controlName);
        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = true;
        if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
            Immunization_TherapeuticInjection.TherapueticInjectionChange();
        }

    },


    searchImmunizationTherapeuticInjection: function (ImmTherInjectionId) {


        pageNumber = 1;
        rowsPerPage = 1;
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        //objData["IsActive"] = IsActive;
        objData["pageNumber"] = pageNumber;

        objData["ImmTherInjectionId"] = ImmTherInjectionId;
        objData["Type"] = Immunization_TherapeuticInjection.params.Type;
        objData["rowsPerPage"] = rowsPerPage;
        objData["commandType"] = "SEARCH_Immunization_Therapeutic_Injection";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");

    },
    PopulateVisitDate: function () {
        var dfd = $.Deferred();
        Immunization_TherapeuticInjection.NotesLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ClinicalNotesCount > 0) {

                    var NotesArray = JSON.parse(response.NotesLoad_JSON);
                    var ddlVisitDate = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlVisitDate');
                    $(ddlVisitDate).find('option').remove();
                    $(ddlVisitDate).append($('<option>', {
                        value: '',
                        text: '- Select -'
                    }));
                    for (var Note in NotesArray) {
                        var date = new Date(NotesArray[Note].VisitDate);
                        var displayValue = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                        displayValue = displayValue + ' ' + NotesArray[Note].VisitTime;


                        $(ddlVisitDate).append($('<option>', {
                            value: NotesArray[Note].NotesId,
                            text: displayValue
                        }))
                    }
                    if (Immunization_TherapeuticInjection.params.from == 'clinicalTabProgressNote') {
                        $(ddlVisitDate).val(Clinical_ProgressNote.params.NotesId);
                        $(ddlVisitDate).attr('disabled', true);
                    }
                    dfd.resolve();
                }
                else {
                    var ddlVisitDate = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlVisitDate');
                    $(ddlVisitDate).find('option').remove();
                    $(ddlVisitDate).append($('<option>', {
                        value: '',
                        text: '- Select -'
                    }));
                    dfd.resolve();
                }

            }
            else {
                utility.DisplayMessages(strMessage, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    NotesLoad: function () {
        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        if (Immunization_TherapeuticInjection.params.PatientId != null) {
            objData["PatientId"] = Immunization_TherapeuticInjection.params.PatientId;
        }
        objData["NoteStatus"] = "Draft";
        objData["commandType"] = "LOAD_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    CreatingDatePickers: function () {

        var datePickerArray = [" #dpAdministrationDate", " #dpExpiryDate", " #dpAdministrationDateHistory"];

        for (var i = 0; i < datePickerArray.length; i++) {
            if (datePickerArray[i] == " #dpExpiryDate") {
                utility.CreateDatePicker(Immunization_TherapeuticInjection.params.PanelID + datePickerArray[i], function () { }, false);
            } else {
                utility.CreateDatePicker(Immunization_TherapeuticInjection.params.PanelID + datePickerArray[i], function () { }, true);
            }

        }
    },

    //Author: Talha Tanweer
    CreatingTimePickers: function () {
        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTime').timepicker('setTime', new Date().toLocaleTimeString());
        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tpAdministrationTimeHistory').timepicker('setTime', new Date().toLocaleTimeString());
    },
    TherapueticInjectionChange: function () {

        if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").val() != "" && $('#' + Immunization_TherapeuticInjection.params.PanelID + " #hfTherapeuticInjection").val() != "") {
            //$('#' + Immunization_TherapeuticInjection.params.PanelID + ' #NoteTemplatTagName').prop('disabled', false);
            if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val() != "") {
                var self = $('#' + Immunization_TherapeuticInjection.params.PanelID);
                $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber").attr('ddlist', 'GetTherapeuticInjectionLotNumber');
                var data = "IsActive=&ID=" + $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #hfTherapeuticInjection').val() + "&ID2=" + $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val();
                self.find('.LotNumber').loadDropDowns(true, data, 'pnlClinicalTherapeuticInjection').done(function () {

                    if ($(" #pnlClinicalTherapeuticInjection #ddlLotNumber").find("option").length == 1) {
                        var checkEmpty = false;
                        if ($(" #pnlClinicalTherapeuticInjection #ddlLotNumber").find("option:first").text() == "- Select -") {
                            checkEmpty = true;
                        }
                        //if()
                        if (checkEmpty) {
                            $.when(Clinical_ImmunizationDetail.WhyLotIsNotPopulateInDropDown($('#' + Immunization_TherapeuticInjection.params.PanelID + ' #hfTherapeuticInjection').val(), $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val(), "Therapeutic")).then(function () {

                            });
                        }
                        else {
                            Immunization_TherapeuticInjection.LotNumberChange($(" #pnlClinicalTherapeuticInjection #ddlLotNumber").find("option:first").val());
                        }

                    }
                    else {

                    }

                });
            }
            else {

            }

        }
        else {
            var self = $('#' + Immunization_TherapeuticInjection.params.PanelID);
            self.find('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlLotNumber').attr('ddlist', '');
        }
    },

    ValidateTherapeuticInjectionAdminister: function () {
        $(' #pnlClinicalTherapeuticInjection  #frmTherapeuticInjection')
             .bootstrapValidator({
                 live: 'disabled',
                 message: 'This value is not valid',
                 feedbackIcons: {
                     valid: 'glyphicon glyphicon-ok',
                     invalid: 'glyphicon glyphicon-remove',
                     validating: 'glyphicon glyphicon-refresh'
                 },
                 fields: {

                     TherapeuticInjection: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },

                     ProviderId: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },

                     AdministrationDate: {
                         group: '.col-xs-6',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     AdministrationTime: {
                         group: '.col-xs-6',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },

                     Dose: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             },
                             numeric: {
                                 message: 'Dose value should be numeric',
                                 thousandsSeparator: '',
                                 decimalSeparator: '.'
                             }
                         }

                     },

                 }
             }).on('success.form.bv', function (e) {

                 e.preventDefault();
                 Immunization_TherapeuticInjection.TherapeuticInjectionSave();

             });




    },
    ValidateTherapeuticInjectionHistory: function () {
        $(' #pnlClinicalTherapeuticInjection  #frmTherapeuticInjectionHistory')
             .bootstrapValidator({
                 live: 'disabled',
                 message: 'This value is not valid',
                 feedbackIcons: {
                     valid: 'glyphicon glyphicon-ok',
                     invalid: 'glyphicon glyphicon-remove',
                     validating: 'glyphicon glyphicon-refresh'
                 },
                 fields: {

                     TherapeuticInjectionHistory: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     DoseHistory: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             },
                             numeric: {
                                 message: 'Dose value should be numeric',
                                 thousandsSeparator: '',
                                 decimalSeparator: '.'
                             }
                         }

                     },



                 }
             }).on('success.form.bv', function (e) {

                 e.preventDefault();
                 Immunization_TherapeuticInjection.TherapeuticInjectionHistorySave();

             });




    },
    TherapeuticInjectionSave: function () {
        var PreFavVal = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlFavorites').val();
        if (Immunization_TherapeuticInjection.params.mode == "Add") {
            var self = $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Medical_Immunization", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_TherapeuticInjection.SaveTherapeuticInjection(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Immunization_TherapeuticInjection.SaveFavToggelStatus(PreFavVal);
                            if (Immunization_TherapeuticInjection.params.from == 'clinicalTabProgressNote') {
                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.TherapeuticInjectionIdColumn + "thera", "Administer", Immunization_TherapeuticInjection.params.PatientId, false, false)).then(function () {
                                    $.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                        Clinical_Immunization.params.PanelID.replace("#pnlClinicalTherapeuticInjection", "").trim();
                                        $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                            Immunization_TherapeuticInjection.UnLoad();
                                        });
                                    });
                                });
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                                $.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                    Clinical_Immunization.params.PanelID.replace("#pnlClinicalTherapeuticInjection", "").trim();
                                    $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                        Immunization_TherapeuticInjection.UnLoad();
                                    });
                                });
                            }


                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
        else if (Immunization_TherapeuticInjection.params.mode == "Edit") {
            var self = $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Medical_Immunization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_TherapeuticInjection.SaveTherapeuticInjection(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Immunization_TherapeuticInjection.SaveFavToggelStatus(PreFavVal);
                            if (Immunization_TherapeuticInjection.IsChangeTherapeuticInjection) {
                                var ResponseData
                                if (Immunization_TherapeuticInjection.params.from == 'clinicalTabProgressNote') {
                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.TherapeuticInjectionIdColumn + "thera", "Administer", Immunization_TherapeuticInjection.params.PatientId, false, false)).then(function () {
                                        $.when(ResponseData = Immunization_TherapeuticInjection.GetProcedureIdAgainstTherapeuticInjectionId(response.TherapeuticInjectionIdColumn)).then(function () {
                                            if (ResponseData.response == 0) {
                                                utility.DisplayMessages("Procedure Not Found", 3);
                                            }
                                            else {
                                                $.when(Immunization_TherapeuticInjection.DeleteProcedureOfPrevInjection(ResponseData.response)).then(function () {
                                                    $.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                                        $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                                            Immunization_TherapeuticInjection.UnLoad();
                                                        });
                                                    });
                                                });

                                            }

                                        });
                                        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;
                                    });
                                }
                                else {
                                    $.when(ResponseData = Immunization_TherapeuticInjection.GetProcedureIdAgainstTherapeuticInjectionId(response.TherapeuticInjectionIdColumn)).then(function () {
                                        if (ResponseData.response == 0) {
                                            utility.DisplayMessages("Procedure Not Found", 3);
                                        }
                                        else {
                                            $.when(Immunization_TherapeuticInjection.DeleteProcedureOfPrevInjection(ResponseData.response)).then(function () {
                                                $.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                                    $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                                        Immunization_TherapeuticInjection.UnLoad();
                                                    });
                                                });
                                            });

                                        }

                                    });
                                    Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;
                                    utility.DisplayMessages(response.message, 1);
                                }


                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                                $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                    Immunization_TherapeuticInjection.UnLoad();
                                });
                            }



                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
    },
    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Immunization_TherapeuticInjection.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Immunization_TherapeuticInjection.FavListName, FavListVal);
        });
    },
    TherapeuticInjectionHistorySave: function () {
        var PreFavVal = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlFavorites').val();
        if (Immunization_TherapeuticInjection.params.mode == "Add") {
            var self = $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjectionHistory");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Medical_Immunization", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_TherapeuticInjection.SaveTherapeuticInjectionHistory(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Immunization_TherapeuticInjection.SaveFavToggelStatus(PreFavVal);
                            if (Immunization_TherapeuticInjection.params.from == 'clinicalTabProgressNote') {
                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.TherapeuticInjectionIdColumn + "thera", "Administer", Immunization_TherapeuticInjection.params.PatientId, false, false)).then(function () {
                                    //$.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                    Clinical_Immunization.params.PanelID.replace("#pnlClinicalTherapeuticInjection", "").trim();
                                    $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                        Immunization_TherapeuticInjection.UnLoad();
                                    });
                                    //});
                                });
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                                //$.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                Clinical_Immunization.params.PanelID.replace("#pnlClinicalTherapeuticInjection", "").trim();
                                $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                    Immunization_TherapeuticInjection.UnLoad();
                                });
                                //});
                            }


                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
        else if (Immunization_TherapeuticInjection.params.mode == "Edit") {
            var self = $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjectionHistory");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Medical_Immunization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_TherapeuticInjection.SaveTherapeuticInjectionHistory(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Immunization_TherapeuticInjection.SaveFavToggelStatus(PreFavVal);
                            if (Immunization_TherapeuticInjection.IsChangeTherapeuticInjection) {
                                var ResponseData
                                if (Immunization_TherapeuticInjection.params.from == 'clinicalTabProgressNote') {
                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.TherapeuticInjectionIdColumn + "thera", "Administer", Immunization_TherapeuticInjection.params.PatientId, false, false)).then(function () {
                                        //$.when(ResponseData = Immunization_TherapeuticInjection.GetProcedureIdAgainstTherapeuticInjectionId(response.TherapeuticInjectionIdColumn)).then(function () {
                                        //    if (ResponseData.response == 0) {
                                        //        utility.DisplayMessages("Procedure Not Found", 3);
                                        //    }
                                        //    else {
                                        //        $.when(Immunization_TherapeuticInjection.DeleteProcedureOfPrevInjection(ResponseData.response)).then(function () {
                                        //            $.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                        $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                            Immunization_TherapeuticInjection.UnLoad();
                                        });
                                        //            });
                                        //        });
                                        //    }

                                        // });
                                        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;
                                    });
                                }
                                else {
                                    //$.when(ResponseData = Immunization_TherapeuticInjection.GetProcedureIdAgainstTherapeuticInjectionId(response.TherapeuticInjectionIdColumn)).then(function () {
                                    //    if (ResponseData.response == 0) {
                                    //        utility.DisplayMessages("Procedure Not Found", 3);
                                    //    }
                                    //    else {
                                    //        $.when(Immunization_TherapeuticInjection.DeleteProcedureOfPrevInjection(ResponseData.response)).then(function () {
                                    //            $.when(Immunization_TherapeuticInjection.InsertTherapeuticInjectionInProcedure(myJSON, response.TherapeuticInjectionIdColumn)).then(function () {
                                    $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                        Immunization_TherapeuticInjection.UnLoad();
                                    });
                                    //            });
                                    //        });
                                    //    }

                                    //});
                                    Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = false;
                                    utility.DisplayMessages(response.message, 1);
                                }


                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                                $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch('', 1, 15, true)).then(function () {
                                    Immunization_TherapeuticInjection.UnLoad();
                                });
                            }
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
    },
    GetProcedureIdAgainstTherapeuticInjectionId: function (ImmTherInjectionId) {
        var dfd = $.Deferred();
        Immunization_TherapeuticInjection.GetProcedureIdAgainstTherapeuticInjectionId_DBCALL(ImmTherInjectionId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.response = response.ProcedureId;
                dfd.resolve();
            }
            else {
                dfd.response = 0;
                dfd.resolve();
            }
        });
        return dfd;
    },
    GetProcedureIdAgainstTherapeuticInjectionId_DBCALL: function (ImmTherInjectionId) {
        var objData = {};
        objData["commandType"] = "GET_PROCEDUREID_AGAINST_THERAPEUTIC_INJECTIONID";
        objData["ImmTherInjectionId"] = ImmTherInjectionId;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");

    },
    SaveTherapeuticInjection: function (TherapeuticInjectionData) {
        var objData = JSON.parse(TherapeuticInjectionData);
        objData["Type"] = "Administered";
        if (objData.PatientId == '' || typeof objData.PatientId == 'undefined') {
            objData.PatientId = Immunization_TherapeuticInjection.params.PatientId;
        }

        if (Immunization_TherapeuticInjection.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_THERAPEUTIC_INJECTION";
            objData["ImmTherInjectionId"] = Immunization_TherapeuticInjection.params.ImmTherInjectionId;
        }
        else {
            objData["commandType"] = "SAVE_THERAPEUTIC_INJECTION";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },

    SaveTherapeuticInjectionHistory: function (TherapeuticInjectionHistoryData) {
        var objData = JSON.parse(TherapeuticInjectionHistoryData);
        objData["ProviderId"] = objData["ProviderIdHistory"];
        objData["AdministrationDate"] = objData["AdministrationDateHistory"];
        objData["AdministrationTime"] = objData["AdministrationTimeHistory"];
        objData["Dose"] = objData["DoseHistory"];
        objData["Amount"] = objData["AmountHistory"];
        objData["RouteId"] = objData["RouteIdHistory"];
        objData["SiteId"] = objData["SiteIdHistory"];
        objData["Comments"] = objData["CommentsHistory"];
        objData["Type"] = "DocumentHx";
        objData["TherapeuticInjectionId"] = objData["TherapeuticInjectionIdHistory"];


        if (objData.PatientId == '' || typeof objData.PatientId == 'undefined') {
            objData.PatientId = Immunization_TherapeuticInjection.params.PatientId;
        }

        if (Immunization_TherapeuticInjection.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_THERAPEUTIC_INJECTION";
            objData["ImmTherInjectionId"] = Immunization_TherapeuticInjection.params.ImmTherInjectionId;
        }
        else {
            objData["commandType"] = "SAVE_THERAPEUTIC_INJECTION";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },
    InsertTherapeuticInjectionInProcedure: function (TherapeuticInjectionData, ImmTherInjectionId) {

        var dfd = $.Deferred();
        var TherapInjecData = JSON.parse(TherapeuticInjectionData);
        var CurrentTherapeuticInjectionId;
        if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
            CurrentTherapeuticInjectionId = TherapInjecData.TherapeuticInjectionId;
        }
        else if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjectionHistory") {
            CurrentTherapeuticInjectionId = TherapInjecData.TherapeuticInjectionIdHistory;
        }

        Immunization_TherapeuticInjection.GetCptCodeAndAdministeredCode(CurrentTherapeuticInjectionId).done(function (response1) {
            response1 = JSON.parse(response1);
            if (response1.status != false) {
                if (response1.VaccineCount != 0) {
                    var CptLoad_JSON = JSON.parse(response1.CptLoad_JSON);
                    var ProceduresDetail = [];
                    var CptCodeDesc = "";
                    if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
                        CptCodeDesc = TherapInjecData.TherapeuticInjection;
                    }
                    else if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjectionHistory") {
                        CptCodeDesc = TherapInjecData.TherapeuticInjectionHistory;
                    }

                    var InjectionDescription = "";
                    var CptCodeDescSplit = CptCodeDesc.split(' - ');
                    if (CptCodeDescSplit.length >= 2) {
                        for (loop = 1; loop < CptCodeDescSplit.length; loop++) {
                            InjectionDescription = InjectionDescription + CptCodeDescSplit[loop];
                        }

                    }
                    else if (CptCodeDescSplit.length == 1) {
                        InjectionDescription = CptCodeDescSplit[0];
                    }

                    $.each(CptLoad_JSON, function (index, item) {
                        var objDetail = {};
                        objDetail["CPTCode"] = item.Code;
                        objDetail["CPT_DESCRIPTION"] = InjectionDescription;
                        //objDetail["CPT_DESCRIPTION"] = item.Description;
                        objDetail["ImmTherInjectionId"] = ImmTherInjectionId;
                        if (Immunization_TherapeuticInjection.params.PatientId != '' || typeof Immunization_TherapeuticInjection.params.PatientId != 'undefined') {
                            objDetail.PatientId = Immunization_TherapeuticInjection.params.PatientId;
                        }
                        else {
                            objDetail.PatientId = $('#PatientProfile #hfPatientId').val();
                        }

                        if (Immunization_TherapeuticInjection.params.PanelID.indexOf("pnlClinicalProgressNote") > 0) {
                            objDetail["NotesId"] = $("#pnlClinicalProgressNote #hfNotesId").val();
                        }
                        else {
                            objDetail["NotesId"] = -1;
                        }
                        objDetail["Modifier"] = "";
                        objDetail["Unit"] = 1;
                        ProceduresDetail.push(objDetail);
                    });

                    if (ProceduresDetail.length > 0) {
                        Immunization_TherapeuticInjection.SaveProceduresForTherapeuticInjection(ProceduresDetail).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.ProcedureCount > 0) {
                                    if (Immunization_TherapeuticInjection.params.from == "clinicalTabProgressNote") {
                                        var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
                                        //$.each(ProceduresLoadJSONData, function (i, item) {
                                        //$('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_VisitDate')
                                        //$.when(Clinical_Procedures.getProceduresInfo(item.ProcedureId, true)).then(function () {
                                        dfd.resolve();
                                        //});
                                        //});

                                    }
                                    else {
                                        dfd.resolve();
                                    }
                                }
                                else {
                                    dfd.resolve();
                                }
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                                dfd.resolve();
                            }
                        });
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                utility.DisplayMessages(response1.Message, 3);
                dfd.resolve();
            }


        });

        return dfd;
    },

    GetCptCodeAndAdministeredCode: function (TherapeuticInjectionId) {
        var objData = new Object();
        objData["TherapeuticInjectionId"] = TherapeuticInjectionId;
        objData["commandType"] = "Get_Cpt_CodeAndAdministeredCode";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },
    DeleteProcedureOfPrevInjection: function (ProcedureIds) {
        var dfd = $.Deferred();
        var def = [];
        $.each(ProcedureIds.split(','), function (index, item) {
            def.push(Clinical_Procedures.DeleteProcedure(item).done(function (response) {
            }));
        });
        $.when.apply($, def).done(function ($n) {
            dfd.resolve();
        });
        return dfd;
    },
    SaveProceduresForTherapeuticInjection: function (ProceduresDetail) {
        var objData = {};
        objData["procedureDetailModel"] = ProceduresDetail;
        objData["commandType"] = "save_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    UpdateProceduresForTherapeuticInjection: function (ProceduresDetail) {
        var objData = {};
        objData["procedureDetailModel"] = ProceduresDetail;
        objData["commandType"] = "update_procedure";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetTherapeuticInjection', false).done(function (result) {
            $("#frmTherapeuticInjection #txtTherapeuticInjection").autocomplete({
                autoFocus: true,
                source: TherapeuticInjection, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlClinicalTherapeuticInjection #txtTherapeuticInjection").val(ui.item.value); // add the selected id
                        $("#pnlClinicalTherapeuticInjection #hfTherapeuticInjection").val(ui.item.id);
                        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = true;
                        Immunization_TherapeuticInjection.TherapueticInjectionChange();
                        // GetNotesTemplates($("#pnlClinicalNotes #txtProvider"), ui.item.id);
                        Immunization_ImmunizationAddImmInj.GetVaccineInformation('Therapeutic', ui.item.id).done(function (response) {
                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #txtDose').val(response.Dose);
                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlAmount').val(response.Amount);
                        });
                    }, 100);
                }
            });
        });
        CacheManager.BindCodes('GetTherapeuticInjection', false).done(function (result) {
            $("#frmTherapeuticInjectionHistory #txtTherapeuticInjectionHistory").autocomplete({
                autoFocus: true,
                source: TherapeuticInjection, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlClinicalTherapeuticInjection #txtTherapeuticInjectionHistory").val(ui.item.value); // add the selected id
                        $("#pnlClinicalTherapeuticInjection #hfTherapeuticInjectionHistory").val(ui.item.id);
                        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = true;
                        // GetNotesTemplates($("#pnlClinicalNotes #txtProvider"), ui.item.id);
                        Immunization_ImmunizationAddImmInj.GetVaccineInformation('Therapeutic', ui.item.id).done(function (response) {
                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #txtDoseHistory').val(response.Dose);
                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlAmountHistory').val(response.Amount);
                        });
                    }, 100);
                }
            });
        });
    },

    setPatientProvider: function () {
        var dfd = $.Deferred();
        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
            Immunization_TherapeuticInjection.getPatientProvider().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $("#frmTherapeuticInjection #ddlProvider").val(response.ProviderId);
                    $("#frmTherapeuticInjectionHistory #ddlProviderHistory").val(response.ProviderId);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {

            if (globalAppdata.DefaultProviderId != "" && globalAppdata.DefaultProviderId != null) {
                $("#frmTherapeuticInjection #ddlProvider").val(globalAppdata.DefaultProviderId);
                $("#frmTherapeuticInjectionHistory #ddlProviderHistory").val(globalAppdata.DefaultProviderId);
            }
            else {
                $("#frmTherapeuticInjection #ddlProvider").val("");
                $("#frmTherapeuticInjectionHistory #ddlProviderHistory").val("");
            }

            dfd.resolve();
        }
        return dfd;
    },
    getPatientProvider: function () {
        var objData = new Object();
        objData["NotesId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        objData["commandType"] = "GET_PROVIDER_ID";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    OpenLotNumber: function () {
        if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").val() != "" && $('#' + Immunization_TherapeuticInjection.params.PanelID + " #hfTherapeuticInjection").val() != "") {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
            params["ParentCtrl"] = 'Immunization_TherapeuticInjection';

            if (Immunization_TherapeuticInjection.getACtiveTabLidID() == "listAdministerInjection") {
                params["TherapeuticInjectionId"] = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #hfTherapeuticInjection").val();
                params["TherapeuticInjectionText"] = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #txtTherapeuticInjection").val();

            }
            if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val() != "") {
                params["ProviderId"] = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val();
                LoadActionPan('Immunization_LotNumber', params);
            }
            else {
                utility.DisplayMessages("Select Provider", 2);
            }

        }
        else {
            utility.DisplayMessages("Select Therapeutic Injection", 3);
        }

    },

    LotNumberChange: function (LotId) {
        var dfd = $.Deferred();
        if (LotId != "") {


            Immunization_TherapeuticInjection.get_LotDataForAutoPopulate(LotId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.LotNumberCount > 0) {
                        var LotNumberLoad_JSON = JSON.parse(response.LotNumberLoad_JSON)[0];
                        Immunization_TherapeuticInjection.SetExpiryDateAndRoute(LotNumberLoad_JSON.ExpiryDate, LotNumberLoad_JSON.RouteId);
                        if (LotNumberLoad_JSON.VacManufacturerId != "" && LotNumberLoad_JSON.ManufacturerName != "") {
                            $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection #hfManufacturerId").val(LotNumberLoad_JSON.VacManufacturerId);
                            $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection #txtManufacturerName").val(LotNumberLoad_JSON.ManufacturerName);
                            utility.SetAutoCompleteSource($("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection #txtManufacturerName"), $("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection #hfManufacturerId"));
                        }
                        dfd.resolve();
                    }
                    else {
                        if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber").length == 1 && $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber").val() != "") {

                        }
                        else {
                            $("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlRoute").val("");
                            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').val("");
                        }
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlRoute").val("");
            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').val("");
            dfd.resolve();
        }
        return dfd
    },
    get_LotDataForAutoPopulate: function (LotId) {

        var objData = new Object();
        PageNumber = 1;
        RowsPerPage = 15;
        objData["Checkprivilegas"] = "yes";
        objData["VaccineLotNoId"] = LotId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "get_lotnumber_by_id";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },

    SetExpiryDateAndRoute: function (ExpiryDate, RouteId) {
        if (RouteId != "" && RouteId != "0" && RouteId != -1) {
            $("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlRoute").val(RouteId);
        }
        else {
            $("#" + Immunization_TherapeuticInjection.params.PanelID + " #ddlRoute").val("");
        }
        if (ExpiryDate != "" && ExpiryDate != -1) {
            ExpiryDate = $.datepicker.formatDate('mm dd yy ', new Date(ExpiryDate));
            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').val(ExpiryDate);
            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').datepicker('setDate', $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #dpExpiryDate').val());
        }

    },
    getACtiveTabLidID: function () {
        var activeTabliId = $("#ulTherapeuticInjectionTabsItems ").find("li.active").attr('id');
        return activeTabliId;
    },
    ActiveInactiveTab: function (Tab) {
        var TabId = "";
        if (Tab == "Administered") {
            TabId = "AdministerInjection";
        }
        else if (Tab == "DocumentHx") {
            TabId = "AdministerInjectionHistory";
        }
        if (TabId != "") {
            $('#' + Immunization_TherapeuticInjection.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + Immunization_TherapeuticInjection.params.PanelID).find('[id*="tab"]').removeClass('active');
            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #list' + TabId).addClass('active');
            $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #tab' + TabId).addClass('active');
        }
    },
    PopulateLotNumber: function (vaccineId, ComeFormLoadFuntion) {
        var dfd = new $.Deferred();
        var self = $('#' + Immunization_TherapeuticInjection.params.PanelID);
        $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber").attr('ddlist', 'GetTherapeuticInjectionLotNumber');
        var data = "IsActive=&ID=" + vaccineId + "&ID2=" + $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val();
        self.find('.LotNumber').loadDropDowns(true, data, 'pnlClinicalTherapeuticInjection').done(function () {
            if (typeof ComeFormLoadFuntion == typeof undefined || ComeFormLoadFuntion == null) {
                if ($(" #pnlClinicalTherapeuticInjection #ddlLotNumber").find("option").length == 1) {
                    var checkEmpty = false;
                    if ($(" #pnlClinicalTherapeuticInjection #ddlLotNumber").find("option:first").text() == "- Select -") {
                        checkEmpty = true;
                    }
                    if (checkEmpty) {
                        $.when(Clinical_ImmunizationDetail.WhyLotIsNotPopulateInDropDown(vaccineId, $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlProvider").val(), "Therapeutic")).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }
        });
        return dfd;
    },
    RefreshLotDetail: function () {
        var dfd = $.Deferred();
        var oldLotId = $('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber").val();
        $.when(Immunization_TherapeuticInjection.PopulateLotNumber($('#' + Immunization_TherapeuticInjection.params.PanelID + " #hfTherapeuticInjection").val())).then(function () {

            $.when(Immunization_TherapeuticInjection.PopulateLotNumberFromLotNumberList($('#' + Immunization_TherapeuticInjection.params.PanelID + " #hfTherapeuticInjection").val(), oldLotId)).then(function () {
                if (oldLotId != "") {
                    $.when(Immunization_TherapeuticInjection.LotNumberChange(oldLotId)).then(function () {
                        dfd.resolve();
                    });
                }
                else {
                    dfd.resolve();
                }
            });

        });


        return dfd;
    },

    PopulateLotNumberFromLotNumberList: function (Vaccineid, TherapueticInjectionLotId) {
        var dfd = $.Deferred();
        if (TherapueticInjectionLotId != "") {
            if ($('#' + Immunization_TherapeuticInjection.params.PanelID + " #ddlLotNumber option[value='" + TherapueticInjectionLotId + "']").length > 0) {
                $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlLotNumber').val(TherapueticInjectionLotId);
                $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlLotNumber' + ' option[value=' + TherapueticInjectionLotId + ']').prop("selected", "selected");//
                //$("#" + Immunization_TherapeuticInjection.params.PanelID + " #frmTherapeuticInjection").bootstrapValidator('revalidateField', 'LotNumber');
                dfd.resolve();
            }
            else {
                dfd.resolve();
            }
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    BindAutocomplete: function () {
        var TherapeuticId = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #frmTherapeuticInjection #hfTherapeuticInjection').val();
        var ManufacturerName = $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #frmTherapeuticInjection #txtManufacturerName').val();
        if (TherapeuticId > 0) {
            utility.Keyupdelay(function () {
                Immunization_TherapeuticInjection.GetManufacturerArray(TherapeuticId, ManufacturerName, "Therapeutic").done(function (response) {
                    $("#frmTherapeuticInjection #txtManufacturerName").autocomplete({
                        autoFocus: true,
                        source: response,
                        select: function (event, ui) {
                            setTimeout(function () {
                                $("#frmTherapeuticInjection #hfManufacturerId").val(ui.item.id);
                                $("#frmTherapeuticInjection #txtManufacturerName").val(ui.item.value);
                            }, 100);
                        }
                    });
                    $("#frmTherapeuticInjection #txtManufacturerName").autocomplete("search");
                });
            });
        }
        else {
            utility.DisplayMessages("Select Therapeutic Injection", 2);
        }
    },
    GetManufacturerArray: function (TherapeuticId, ManufacturerName, Type) {
        var AllManufacturer = [];
        var IsValid = false;

        if (ManufacturerName != null && ManufacturerName.length > 2) {
            IsValid = true;
        }

        else {
            IsValid = false;
        }
        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Immunization_TherapeuticInjection.GetManufacturerArray_DBCALL(TherapeuticId, ManufacturerName, Type).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    if (responseData.ManufacturerCount > 0) {
                        var Manufacturer_JSON = responseData.Manufacturer_JSON;
                        $.each(Manufacturer_JSON, function (i, item) {
                            AllManufacturer.push({ id: item.ManufacturerId, value: item.ManufacturerName });
                        });
                    }
                }
                dfd.resolve(AllManufacturer);
            });
        }
        else {
            dfd.resolve(AllManufacturer);
        }

        return dfd.promise();

    },
    GetManufacturerArray_DBCALL: function (TherapeuticId, ManufacturerName, Type) {

        var objData = new Object();
        objData["TherapeuticId"] = TherapeuticId;
        objData["ManufacturerName"] = ManufacturerName;
        objData["commandType"] = "Get_Manufacturer_Array";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },
    UnLoad: function () {
        if (Immunization_TherapeuticInjection.params != null && Immunization_TherapeuticInjection.params.ParentCtrl) {
            if (Immunization_TherapeuticInjection.params.ParentCtrl == 'clinicalTabImmunization') {
                UnloadActionPan(Immunization_TherapeuticInjection.params["ParentCtrl"], "Immunization_TherapeuticInjection");
            } else {
                Clinical_Treatment.TherapeuticSearch('', 1, 15);
                Immunization_TherapeuticInjection.params.PanelID = Immunization_TherapeuticInjection.params.PanelID.replace(" #pnlClinicalTherapeuticInjection", "");
                UnloadActionPan(Immunization_TherapeuticInjection.params.ParentCtrl, 'Immunization_TherapeuticInjection', null, Immunization_TherapeuticInjection.params.PanelID);
            }
        }
        else {
            UnloadActionPan();
        }
    },
}