OrderSet_TherapeuticDetail = {
    bIsFirstLoad: true,
    params: [],
    Type: "",
    Load: function (params) {
        OrderSet_TherapeuticDetail.params = params;
        if (OrderSet_TherapeuticDetail.params.PanelID != 'pnlOrderSetTherapeuticInjection') {
            OrderSet_TherapeuticDetail.params.PanelID = OrderSet_TherapeuticDetail.params.PanelID + ' #pnlOrderSetTherapeuticInjection';
        } else {
            OrderSet_TherapeuticDetail.params.PanelID = 'pnlOrderSetTherapeuticInjection';
        }
        OrderSet_TherapeuticDetail.CreatingDatePickers();
        OrderSet_TherapeuticDetail.CreatingTimePickers();
        var self = $('#' + OrderSet_TherapeuticDetail.params.PanelID);
        if (OrderSet_TherapeuticDetail.bIsFirstLoad == true) {
            OrderSet_TherapeuticDetail.ValidateTherapeuticInjectionAdminister();
            OrderSet_TherapeuticDetail.ValidateTherapeuticInjectionHistory();
            self.loadDropDowns(true).done(function () {
                if (OrderSet_TherapeuticDetail.params.mode == "Edit") {


                    $.when(OrderSet_TherapeuticDetail.LoadTherapeuticInjection(OrderSet_TherapeuticDetail.params.OSImmTherInjectionId)).then(function () {
                        OrderSet_TherapeuticDetail.ActiveInactiveTab(OrderSet_TherapeuticDetail.params.Type);
                        OrderSet_TherapeuticDetail.CreatingDatePickers();
                        OrderSet_TherapeuticDetail.CreatingTimePickers();
                        
                        
                        if (OrderSet_TherapeuticDetail.params.Type == "Administered") {
                            utility.SetAutoCompleteSource($("#pnlOrderSetTherapeuticInjection #txtTherapeuticInjection"), $("#pnlOrderSetTherapeuticInjection #hfTherapeuticInjection"));
                            utility.SetAutoCompleteSource($("#pnlOrderSetTherapeuticInjection #txtManufacturerName"), $("#pnlOrderSetTherapeuticInjection #hfManufacturerId"));
                            if (!$('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjectionHistory").hasClass('disableAll')) {
                                $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjectionHistory").addClass('disableAll');
                            }
                            if ($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjection").hasClass('disableAll')) {
                                $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjection").removeClass('disableAll');
                            }
                        } else {
                            utility.SetAutoCompleteSource($("#pnlOrderSetTherapeuticInjection #txtTherapeuticInjectionHistory"), $("#pnlOrderSetTherapeuticInjection #hfTherapeuticInjectionHistory"));
                            if (!$('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjection").hasClass('disableAll')) {
                                $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjection").addClass('disableAll');
                            }
                            if ($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjectionHistory").hasClass('disableAll')) {
                                $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjectionHistory").removeClass('disableAll');
                            }
                        }

                    });

                }
                else {
                    if ($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjectionHistory").hasClass('disableAll')) {
                        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjectionHistory").removeClass('disableAll');
                    }
                    if ($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjection").hasClass('disableAll')) {
                        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #listAdministerInjection").removeClass('disableAll');
                    }
                }
            });
        }
    },
    LoadTherapeuticInjection: function (OsImmTherInjectionId) {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_OrderSetDetails.searchTherapeutic(1, 1, OsImmTherInjectionId, OrderSet_TherapeuticDetail.params.Type).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        if (response.TherapeuticInjectionCount > 0) {
                            var TherInjectionDetails = JSON.parse(response.TherapeuticInjectionLoad_JSON)[0];

                            if (OrderSet_TherapeuticDetail.params.Type == "Administered") {
                                
                                var self = $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection");
                                utility.bindMyJSONByName(true, TherInjectionDetails, false, self).done(function () {
                                    $.when(OrderSet_TherapeuticDetail.PopulateLotNumber(TherInjectionDetails.TherapeuticInjectionId)).then(function () {
                                        OrderSet_TherapeuticDetail.SetExpiryDateAndRoute(TherInjectionDetails.ExpiryDate, TherInjectionDetails.RouteId);
                                        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #txtTherapeuticInjection").val((TherInjectionDetails.CPTCode != "" ? TherInjectionDetails.CPTCode + ' - ' : "") + TherInjectionDetails.TherapeuticInjection);
                                        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #hfTherapeuticInjection").val(TherInjectionDetails.TherapeuticInjectionId);
                                        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #hfManufacturerId").val(TherInjectionDetails.ManufacturerId);

                                        //if (TherInjectionDetails.ExpiryDate != "") {
                                        //    $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').val(TherInjectionDetails.ExpiryDate)
                                        //    $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').datepicker('setDate', $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').val());
                                        //}


                                       
                                        if ($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber option[value='" + TherInjectionDetails.LotNumber + "']").length == 0 && TherInjectionDetails.LotNumber!="") {
                                            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #ddlLotNumber').append('<option selected value="' + TherInjectionDetails.LotNumber + '">' + TherInjectionDetails.LotText + '</option>');
                                        }
                                        else if (TherInjectionDetails.LotNumber == "") {
                                            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #ddlLotNumber').append('<option selected value="">-Select-</option>');
                                        }
                                        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").val(TherInjectionDetails.LotNumber);
                                        dfd.resolve();
                                    });
                                });

                            }
                            else if (OrderSet_TherapeuticDetail.params.Type == "DocumentHx") {
                                
                                var self = $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjectionHistory");
                                utility.bindMyJSONByName(true, TherInjectionDetails, false, self).done(function () {
                                    $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjectionHistory #txtTherapeuticInjectionHistory").val(TherInjectionDetails.CPTCode + ' - ' + TherInjectionDetails.TherapeuticInjection);
                                    $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #hfTherapeuticInjectionHistory").val(TherInjectionDetails.TherapeuticInjectionId);
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
    ValidateTherapeuticInjectionAdminister: function () {
        $(' #pnlOrderSetTherapeuticInjection  #frmTherapeuticInjection')
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
                     ddlAmount: {
                         group: '.col-xs-6',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },

                 }
             }).on('success.form.bv', function (e) {

                 e.preventDefault();
                 OrderSet_TherapeuticDetail.OS_TherapeuticInjectionSave();

             });




    },
    ValidateTherapeuticInjectionHistory: function () {
        $(' #pnlOrderSetTherapeuticInjection  #frmTherapeuticInjectionHistory')
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
                 OrderSet_TherapeuticDetail.TherapeuticInjectionHistorySave();

             });
    },
    CreatingDatePickers: function () {

        var datePickerArray = [" #dpAdministrationDate", " #dpExpiryDate", " #dpAdministrationDateHistory"];

        for (var i = 0; i < datePickerArray.length; i++) {
            if (datePickerArray[i] == " #dpExpiryDate") {
                utility.CreateDatePicker(OrderSet_TherapeuticDetail.params.PanelID + datePickerArray[i], function () { }, false);
            } else {
                utility.CreateDatePicker(OrderSet_TherapeuticDetail.params.PanelID + datePickerArray[i], function () { }, true);
            }

        }
    },

    //Author: Talha Tanweer
    CreatingTimePickers: function () {
        $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #tpAdministrationTime').timepicker('setTime', new Date().toLocaleTimeString());
        $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #tpAdministrationTimeHistory').timepicker('setTime', new Date().toLocaleTimeString());
    },

    ProviderChange: function (ComeFormLoadFuntion) {
        var dfd = $.Deferred();
        $.when(OrderSet_TherapeuticDetail.PopulateLotNumber($("#" + OrderSet_TherapeuticDetail.params.PanelID + " #hfTherapeuticInjection").val(), ComeFormLoadFuntion)).then(function () {
            //$.when(OrderSet_TherapeuticDetail.SetLotManufanucture($("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val())).then(function () {
            
            //});
        });
        return dfd;
    },
    PopulateLotNumber: function (vaccineId, ComeFormLoadFuntion) {
        var dfd = new $.Deferred();
        var self = $('#' + OrderSet_TherapeuticDetail.params.PanelID);
        $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").attr('ddlist', 'GetTherapeuticInjectionLotNumber');
        if (vaccineId > 0 && $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlProvider").val() > 0) {
            var data = "IsActive=&ID=" + vaccineId + "&ID2=" + $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlProvider").val();
            self.find('.LotNumber').loadDropDowns(true, data, 'pnlOrderSetTherapeuticInjection').done(function () {
                if (typeof ComeFormLoadFuntion == typeof undefined || ComeFormLoadFuntion == null) {
                    if ($("#pnlOrderSetTherapeuticInjection #ddlLotNumber").find("option").length == 1) {
                        var checkEmpty = false;
                        if ($("#pnlOrderSetTherapeuticInjection #ddlLotNumber").find("option:first").text() == "- Select -") {
                            checkEmpty = true;
                        }
                        if (checkEmpty) {
                            $.when(Clinical_ImmunizationDetail.WhyLotIsNotPopulateInDropDown(vaccineId, $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlProvider").val(), "Therapeutic")).then(function () {
                                dfd.resolve();
                            });
                        }
                        else {
                            $.when(OrderSet_TherapeuticDetail.LotNumberChange($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").val())).then(function () {
                                dfd.resolve();
                            });
                        }
                    }
                    else {
                        $.when(OrderSet_TherapeuticDetail.LotNumberChange($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").val())).then(function () {
                            dfd.resolve();
                        });
                    }
                }
                else {
                    $.when(OrderSet_TherapeuticDetail.LotNumberChange($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").val())).then(function () {
                        dfd.resolve();
                    });
                }
            });
        }
        else {
            $("#pnlOrderSetTherapeuticInjection #ddlLotNumber").html("<option>-Select-</option>");
            dfd.resolve();
        }
        
        return dfd;
    },

    BindAutocomplete: function (NotRefreashAutocomplete) {
        var TherapeuticId = $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #frmTherapeuticInjection #hfTherapeuticInjection').val();
        var ManufacturerName = $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #frmTherapeuticInjection #txtManufacturerName').val();
        if (TherapeuticId > 0) {
            utility.Keyupdelay(function () {
                OrderSet_TherapeuticDetail.GetManufacturerArray(TherapeuticId, ManufacturerName, "Therapeutic").done(function (response) {
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
            OrderSet_TherapeuticDetail.GetManufacturerArray_DBCALL(TherapeuticId, ManufacturerName, Type).done(function (responseData) {
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

    OS_TherapeuticInjectionSave: function () {
        if (OrderSet_TherapeuticDetail.params.mode == "Add") {
            var self = $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                OrderSet_TherapeuticDetail.SaveTherapeuticInjection(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.TherapeuticSearch()).then(function () {
                            OrderSet_TherapeuticDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }
            //});
        }
        else if (OrderSet_TherapeuticDetail.params.mode == "Edit") {
            var self = $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                OrderSet_TherapeuticDetail.SaveTherapeuticInjection(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.TherapeuticSearch()).then(function () {
                            OrderSet_TherapeuticDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }
        }
    },
    TherapeuticInjectionHistorySave: function () {

        if (OrderSet_TherapeuticDetail.params.mode == "Add") {
            var self = $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjectionHistory");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                OrderSet_TherapeuticDetail.SaveTherapeuticInjectionHistory(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.TherapeuticSearch()).then(function () {
                            OrderSet_TherapeuticDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }
            //});
        }
        else if (OrderSet_TherapeuticDetail.params.mode == "Edit") {
            var self = $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjectionHistory");
            var myJSON = self.getMyJSONByName();
            var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                OrderSet_TherapeuticDetail.SaveTherapeuticInjectionHistory(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.TherapeuticSearch()).then(function () {
                            OrderSet_TherapeuticDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }
        }
    },
    SaveTherapeuticInjection: function (TherapeuticInjectionData) {
        var objData = JSON.parse(TherapeuticInjectionData);
        objData["Type"] = "Administered";
        objData["OrderSetId"] = OrderSet_TherapeuticDetail.params.OrderSetId;
        if (OrderSet_TherapeuticDetail.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_THERAPEUTIC_INJECTION";
            objData["OSImmTherInjectionId"] = OrderSet_TherapeuticDetail.params.OSImmTherInjectionId;
        }
        else {
            objData["commandType"] = "SAVE_THERAPEUTIC_INJECTION";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "IMMUNIZATIONTHERAPEUTICINJECTION");
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
        objData["OrderSetId"] = OrderSet_TherapeuticDetail.params.OrderSetId;
        if (OrderSet_TherapeuticDetail.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_THERAPEUTIC_INJECTION";
            objData["OSImmTherInjectionId"] = OrderSet_TherapeuticDetail.params.OSImmTherInjectionId;
        }
        else {
            objData["commandType"] = "SAVE_THERAPEUTIC_INJECTION";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },
    LotNumberChange: function (LotId) {
        var dfd = $.Deferred();
        if (LotId != "") {


            OrderSet_TherapeuticDetail.get_LotDataForAutoPopulate(LotId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.LotNumberCount > 0) {
                        var LotNumberLoad_JSON = JSON.parse(response.LotNumberLoad_JSON)[0];
                        OrderSet_TherapeuticDetail.SetExpiryDateAndRoute(LotNumberLoad_JSON.ExpiryDate, LotNumberLoad_JSON.RouteId);
                        if (LotNumberLoad_JSON.VacManufacturerId != "" && LotNumberLoad_JSON.ManufacturerName != "") {
                            $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #hfManufacturerId").val(LotNumberLoad_JSON.VacManufacturerId);
                            $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #txtManufacturerName").val(LotNumberLoad_JSON.ManufacturerName);
                            utility.SetAutoCompleteSource($("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #txtManufacturerName"), $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #frmTherapeuticInjection #hfManufacturerId"));
                        }
                        dfd.resolve();
                    }
                    else {
                        if ($('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").length == 1 && $('#' + OrderSet_TherapeuticDetail.params.PanelID + " #ddlLotNumber").val() != "") {

                        }
                        else {
                            $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #ddlRoute").val("");
                            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').val("");
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
            $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #ddlRoute").val("");
            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').val("");
            dfd.resolve();
        }
        return dfd
    },
    SetExpiryDateAndRoute: function (ExpiryDate, RouteId) {
        if (RouteId != "" && RouteId != "0" && RouteId != -1) {
            $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #ddlRoute").val(RouteId);
        }
        else {
            $("#" + OrderSet_TherapeuticDetail.params.PanelID + " #ddlRoute").val("");
        }
        if (ExpiryDate != "" && ExpiryDate != -1) {
            ExpiryDate = $.datepicker.formatDate('mm dd yy ', new Date(ExpiryDate));
            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').val(ExpiryDate);
            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').datepicker('setDate', $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #dpExpiryDate').val());
        }

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
    ActiveInactiveTab: function (Tab) {
        var TabId = "";
        if (Tab == "Administered") {
            TabId = "AdministerInjection";
        }
        else if (Tab == "DocumentHx") {
            TabId = "AdministerInjectionHistory";
        }
        if (TabId != "") {
            $('#' + OrderSet_TherapeuticDetail.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + OrderSet_TherapeuticDetail.params.PanelID).find('[id*="tab"]').removeClass('active');
            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #list' + TabId).addClass('active');
            $('#' + OrderSet_TherapeuticDetail.params.PanelID + ' #tab' + TabId).addClass('active');
        }
    },
    UnLoad: function () {

        if (OrderSet_TherapeuticDetail.params != null && OrderSet_TherapeuticDetail.params.ParentCtrl && OrderSet_TherapeuticDetail.params.ParentCtrlPanelID) {
            UnloadActionPan(OrderSet_TherapeuticDetail.params.ParentCtrl, "OrderSet_TherapeuticDetail", null, OrderSet_TherapeuticDetail.params.ParentCtrlPanelID);
        }
        else if (OrderSet_TherapeuticDetail.params != null && OrderSet_TherapeuticDetail.params.ParentCtrl) {
            UnloadActionPan(OrderSet_TherapeuticDetail.params.ParentCtrl, "OrderSet_TherapeuticDetail");
        }
        else {
            UnloadActionPan(null, "OrderSet_TherapeuticDetail");
        }
    },
}