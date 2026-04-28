Encounter_CreateClaim = {
    bIsFirstLoad: true,
    params: [],
    patFullName: "",
    SelfPay: "False",
    RefProviderId: "",
    ReProviderName: "",
    PatientDOB: "",
    PatientGender: "",

    Load: function (params) {
        Encounter_CreateClaim.params = params;

        var ObjDeferred = new $.Deferred();

        if (Encounter_CreateClaim.bIsFirstLoad) {
            var self = null;
            if (Encounter_CreateClaim.params.PanelID.indexOf("pnlEncounterCreateClaim") < 0)
                self = $('#' + Encounter_CreateClaim.params.PanelID + " #pnlEncounterCreateClaim");
            else
                self = $('#' + Encounter_CreateClaim.params.PanelID);

            self.loadDropDowns(true).done(function () {
                self.find('#ddlClaimType option').filter(function () {
                    return $.trim($(this).text().toLowerCase()) == "professional medical";
                }).attr('selected', true);
                Encounter_CreateClaim.ValidateForm();

                $('#frmEncounterCreateClaim').data('serialize', $('#frmEncounterCreateClaim').serialize());
                ObjDeferred.resolve();

            });
        }
        else
            ObjDeferred.resolve();

        Encounter_CreateClaim.params["mode"] = "Add";

        $.when(ObjDeferred).then(function (event) {
            Encounter_CreateClaim.LoadAllAutocomplete();
            Encounter_CreateClaim.BindPatientAccount();
            Encounter_CreateClaim.enablePatientControls();
            Encounter_CreateClaim.LoadDefaultData();
            Encounter_CreateClaim.FillPatientInfoFromSearch(Encounter_CreateClaim.params["PatientId"], Encounter_CreateClaim.params["patFullName"], Encounter_CreateClaim.params["RefProviderId"], Encounter_CreateClaim.params["RefProviderName"], Encounter_CreateClaim.params["ProviderId"], Encounter_CreateClaim.params["ProviderName"], Encounter_CreateClaim.params["FacilityId"], Encounter_CreateClaim.params["FaciltyName"], Encounter_CreateClaim.params["SelfPay"], event, Encounter_CreateClaim.params["DOB"], Encounter_CreateClaim.params["Gender"]);
            $('#frmEncounterCreateClaim').data('serialize', $('#frmEncounterCreateClaim').serialize());
        });

        if (Encounter_CreateClaim.bIsFirstLoad) {
            Encounter_CreateClaim.bIsFirstLoad = false;
            utility.ValidateFromToDate(Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim', 'dtpDOSFrom', 'dtpDOSTo', true, function () {
                if ($('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                    $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'DOSFrom');
                }
                Encounter_CreateClaim.validateDosFromTo($('#' + Encounter_CreateClaim.params.PanelID + ' #dtpDOSFrom'), $('#' + Encounter_CreateClaim.params.PanelID + ' #dtpDOSTo'));
            }, function () {
                //to date callback
                if ($('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                    $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'DOSTo');
                }

            });
            Encounter_CreateClaim.FillDOS();
            Encounter_CreateClaim.validateDosFromTo($('#' + Encounter_CreateClaim.params.PanelID + ' #dtpDOSFrom'), $('#' + Encounter_CreateClaim.params.PanelID + ' #dtpDOSTo'));
        }
        self.find('#divAccount,#divPatientName').css("display", "");
        $('#frmEncounterCreateClaim').data('serialize', $('#frmEncounterCreateClaim').serialize());
    },
    validateDosFromTo: function (objDosFrom, objDosTo) {
        $(objDosFrom).datepicker('setEndDate', new Date());
        $(objDosTo).datepicker('setEndDate', new Date());
    },

    ClearFields: function () {
        if ($('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtPatientName').val() == "") {
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #hfPatientId').val('');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtPatientFullName').val('')
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtProvider').val('');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #hfProvider').val('');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtFacility').val('');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #hfFacility').val('');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtResourceProvider').val('');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #hfResourceProvider').val('');

            $bootstrapValidator = null;
            if ($('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                $bootstrapValidator = $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim').data('bootstrapValidator');
            }

            if ($bootstrapValidator) {
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'Provider');
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'Facility');
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'ResourceProvider');
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'PatientName');
            }


            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #lblAccount').show();
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #lnkAccountEdit').hide();

            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #lblPatientName').removeClass("hidden");
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #lnkPatientNameEdit').addClass('hidden');

            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtPatientFullName').trigger('onblur');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtProvider').trigger('onblur');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtFacility').trigger('onblur');
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #txtResourceProvider').trigger('onblur');

        }
        else {
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #lblAccount').hide();
            $('#' + Encounter_CreateClaim.params.PanelID + ' #frmEncounterCreateClaim #lnkAccountEdit').show();
        }
    },

    FillDOS: function () {
        $('#' + Encounter_CreateClaim.params.PanelID + ' #dtpDOSFrom').val($.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#' + Encounter_CreateClaim.params.PanelID + ' #dtpDOSTo').val($.datepicker.formatDate('mm/dd/yy', new Date()));
    },

    LoadDefaultData: function () {
        var self = null;
        if (Encounter_CreateClaim.params.PanelID.indexOf("pnlEncounterCreateClaim") < 0)
            self = $('#' + Encounter_CreateClaim.params.PanelID + ' #pnlEncounterCreateClaim');
        else
            self = $('#' + Encounter_CreateClaim.params.PanelID);
        // Set Patient Banner Info into Patient Account/hiddenField/Patient Name
        utility.GetPatientBannerInfo(self.find("#hfPatientId"), self.find("#txtPatientName"), self.find("#txtPatientFullName"), self.find("#hfRefProvider"), self.find("#txtRefProvider"), function () {
            Encounter_CreateClaim.enablePatientControls();
        }, true);
        //End PMS-2387
        if (globalAppdata['DefaultProviderName'] != "" && globalAppdata['DefaultProviderId'] != "") {
            self.find('#txtProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfProvider').val(globalAppdata['DefaultProviderId']);

            //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856; Set Provider value as Resource Provider.
            self.find('#txtResourceProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfResourceProvider').val(globalAppdata['DefaultProviderId']);

            var result = Providers.filter(function (obj) {
                return obj.id == globalAppdata['DefaultProviderId'];
            });
            if (result.length > 0 && result[0].SpecialityName) {
                self.find('#hfSpecialityName').val(result[0].SpecialityName);
            }
        }
        else {
            // Begin Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3140 Set the value from patient demographics
            if (Patient_Demographic.params.PatientProvider) {
                self.find('#txtProvider').val(Patient_Demographic.params.PatientProvider);
            }
            if (Patient_Demographic.params.PatientProviderId) {
                self.find('#hfProvider').val(Patient_Demographic.params.PatientProviderId);
            }
            // End Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3140 Set the value from patient demographics

            //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856; Set Provider value as Resource Provider.
            if (Patient_Demographic.params.PatientProvider) {
                self.find('#txtResourceProvider').val(Patient_Demographic.params.PatientProvider);
            }
            if (Patient_Demographic.params.PatientProviderId) {
                self.find('#hfResourceProvider').val(Patient_Demographic.params.PatientProviderId);
            }
            //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856
        }
        if (self.find('#txtProvider').val() != "") {
            self.find('#lnkProviderEdit').css("display", "inline");
            self.find('#lblProvider').css("display", "none");
        }
        if (self.find('#txtResourceProvider').val() != "") {
            self.find('#lnkResourceProviderEdit').css("display", "inline");
            self.find('#lblResourceProvider').css("display", "none");
        }
        if (self.find('#txtRefProvider') != "") {
            self.find('#lnkRefProviderEdit').css("display", "inline");
            self.find('#lblRefProvider').css("display", "none");
        }
        if (globalAppdata['DefaultFacilityName'] != "" && globalAppdata['DefaultFacilityId'] != "") {
            self.find('#txtFacility').val(globalAppdata['DefaultFacilityName']);
            self.find('#hfFacility').val(globalAppdata['DefaultFacilityId']);
        }
        else {
            // Begin Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3140 Set the value from patient demographics
            if (Patient_Demographic.params.PatientFacility) {
                self.find('#txtFacility').val(Patient_Demographic.params.PatientFacility);
                self.find('#hfFacility').val(Patient_Demographic.params.PatientFacilityId);
            }
            // End Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3140 Set the value from patient demographics
        }
        //Begin Edited by Azeem Raza Tayyab on 11-Feb-2016 to fix bug#:PMS-3920
        if (self.find('#txtFacility').val() != "") {
            self.find('#lnkFacilityEdit').css("display", "inline");
            self.find('#lblFacility').css("display", "none");
        }
    },
    /*
    LoadDefaultData: function () {
        var self = null;
        if (Encounter_CreateClaim.params.PanelID.indexOf("pnlEncounterCreateClaim") < 0)
            self = $('#' + Encounter_CreateClaim.params.PanelID + ' #pnlEncounterCreateClaim');
        else
            self = $('#' + Encounter_CreateClaim.params.PanelID);
        if (globalAppdata['DefaultProviderName'] != "" && globalAppdata['DefaultProviderId'] != "") {
            self.find('#txtProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfProvider').val(globalAppdata['DefaultProviderId']);
            self.find('#txtResourceProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfResourceProvider').val(globalAppdata['DefaultProviderId']);
        }
        else {
            self.find('#txtProvider').val(Patient_Demographic.params.PatientProvider);
            self.find('#hfProvider').val(Patient_Demographic.params.PatientProviderId);
            self.find('#txtResourceProvider').val(Patient_Demographic.params.PatientProvider);
            self.find('#hfResourceProvider').val(Patient_Demographic.params.PatientProviderId);

        }
        if (self.find('#txtProvider').val() != "") {
            self.find('#lnkProviderEdit').css("display", "inline");
            self.find('#lblProvider').css("display", "none");
        }
        if (self.find('#txtResourceProvider') != "") {
            self.find('#lnkResourceProviderEdit').css("display", "inline");
            self.find('#lblResourceProvider').css("display", "none");
        }
        if (globalAppdata['DefaultFacilityName'] != "" && globalAppdata['DefaultFacilityId'] != "") {
            self.find('#txtFacility').val(globalAppdata['DefaultFacilityName']);
            self.find('#hfFacility').val(globalAppdata['DefaultFacilityId']);
        }
        else {
            self.find('#txtFacility').val(Patient_Demographic.params.PatientFacility);
            self.find('#hfFacility').val(Patient_Demographic.params.PatientFacilityId);
        }
        if (self.find('#txtFacility').val() != "") {
            self.find('#lnkFacilityEdit').css("display", "inline");
            self.find('#lblFacility').css("display", "none");
        }
    },
    */
    LoadAllAutocomplete: function () {
        var dfd = new $.Deferred();
        var IsLoaded;
        var self = null;
        if (Encounter_CreateClaim.params.PanelID.indexOf("pnlEncounterCreateClaim") < 0)
            self = $('#' + Encounter_CreateClaim.params.PanelID + ' #pnlEncounterCreateClaim');
        else
            self = $('#' + Encounter_CreateClaim.params.PanelID);
        IsLoaded = CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = self.find("input#txtProvider");
            var hfCtrl = self.find("#hfProvider");
            var onSelect = function (e) { self.find("#hfSpecialityName").val(e.SpecialityName); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);

            var Ctrl = self.find("input#txtResourceProvider");
            var hfCtrl = self.find("#hfResourceProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });
        IsLoaded = CacheManager.BindCodes('GetBillingProviders', false).done(function (result) {
            var Ctrl = self.find("input#txtBillingProvider");
            var hfCtrl = self.find("#hfBillingProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", GetBillingProviders, null, hfCtrl);
        });
        IsLoaded = CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = self.find("input#txtFacility");
            var hfCtrl = self.find("#hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });


        dfd.resolve(IsLoaded);
        return dfd.promise();
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.TabID == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('Patient_Search', params);
    },

    PatientDemographics: function (patientid) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Encounter_CreateClaim";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, RefProviderId, RefProviderName, ProviderId, ProviderName, FacilityId, FaciltyName, SelfPay, event, DOB, Gender) {
        if (event != null) {
            event.stopPropagation();
        }
        Encounter_CreateClaim.patFullName = patFullName;
        Encounter_CreateClaim.SelfPay = SelfPay;
        Encounter_CreateClaim.RefProviderId = RefProviderId;
        Encounter_CreateClaim.ReProviderName = RefProviderName;
        if (DOB) {
            Encounter_CreateClaim.PatientDOB = DOB;
        }
        if (Gender)
            Encounter_CreateClaim.PatientGender = Gender;
        var self = null;
        if (Encounter_CreateClaim.params.PanelID.indexOf("pnlEncounterCreateClaim") < 0)
            self = $('#' + Encounter_CreateClaim.params.PanelID + ' #pnlEncounterCreateClaim');
        else
            self = $('#' + Encounter_CreateClaim.params.PanelID);

        self.find("#hfPatientId").val(PatientId);

        self.find("#txtPatientName").val(patFullName.split(" - ")[0]);
        self.find("#txtPatientFullName").val(patFullName.split(" - ")[1]);
        utility.SetKendoAutoCompleteSourceforValidate(self.find("#txtPatientName"), patFullName.split(" - ")[0], self.find("#hfPatientId"), PatientId, "AccountNumber");
        if ((globalAppdata['DefaultProviderName'] == "" || globalAppdata['DefaultProviderName'] == "- Select -") && globalAppdata['DefaultProviderId'] == "") {
            self.find('#hfProvider').val(ProviderId);
            self.find('#txtProvider').val(ProviderName);
            if ($("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'Provider');
            }
            if (self.find('#txtProvider').val() != "") {
                self.find('#lnkProviderEdit').css("display", "inline");
                self.find('#lblProvider').css("display", "none");
            }
        }
        if ((globalAppdata['DefaultProviderName'] == "" || globalAppdata['DefaultProviderName'] == "- Select -") && globalAppdata['DefaultProviderId'] == "") {
            self.find('#hfResourceProvider').val(ProviderId);
            self.find('#txtResourceProvider').val(ProviderName);
            if ($("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'ResourceProvider');
            }
            if (self.find('#txtResourceProvider') != "") {
                self.find('#lnkResourceProviderEdit').css("display", "inline");
                self.find('#lblResourceProvider').css("display", "none");
            }
        }
        if ((globalAppdata['DefaultFacilityName'] == "" || globalAppdata['DefaultFacilityName'] == "- Select -") && globalAppdata['DefaultFacilityId'] == "") {
            self.find('#hfFacility').val(FacilityId);
            self.find('#txtFacility').val(FaciltyName);
            if ($("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'Facility');
            }
            if (self.find('#txtFacility').val() != "") {
                self.find('#lnkFacilityEdit').css("display", "inline");
                self.find('#lblFacility').css("display", "none");
            }
        }

        if (self.find("#hfPatientId").val() != "") {
            self.find("#lblAccount").hide();
            self.find("#lnkPatientNameEdit").removeClass("hidden");
            self.find("#lblPatientName").addClass("hidden");
            self.find("#lnkAccountEdit").show();
        } else {
            self.find("#lnkAccountEdit").hide();
            self.find("#lblAccount").show();
            self.find("#lnkPatientNameEdit").addClass("hidden");
            self.find("#lblPatientName").removeClass("hidden");
        }

        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment') {
            UnloadActionPan('Encounter_CreateClaim');
            if ($("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'PatientName');
            }
        }
        if (globalAppdata['DefaultBillingProviderId'] != "" && globalAppdata['DefaultBillingProviderName'] != "- Select -") {
            self.find('#hfBillingProvider').val(globalAppdata['DefaultBillingProviderId']);
            self.find('#txtBillingProvider').val(globalAppdata['DefaultBillingProviderName']);
            if (self.find('#txtBillingProvider').val() != "") {
                self.find('#lnkBillingProviderEdit').css("display", "inline");
                self.find('#lblBillingProvider').css("display", "none");
            }
        }
        Encounter_CreateClaim.enablePatientControls();
        utility.InsertRecentPatient(PatientId);
    },

    enablePatientControls: function () {
        var self = null;
        if (Encounter_CreateClaim.params["PanelID"] != 'pnlEncounterCreateClaim')
            self = $("#" + Encounter_CreateClaim.params["PanelID"] + " #pnlEncounterCreateClaim");
        else
            self = $("#" + Encounter_CreateClaim.params["PanelID"]);

        AccountNo = self.find("#txtPatientName").val();
        if (AccountNo != null && AccountNo == "") {

            self.find("#divCaseNumber, #divPatientInsurancePlan").each(function () {
                $(this).addClass('disableAll');
            });

        }
        else if (AccountNo != null && AccountNo != "") {

            self.find("#divCaseNumber, #divPatientInsurancePlan").each(function () {
                $(this).removeClass('disableAll');
            });
        }
        if (self.find("#hfPatientId").val() != "") {
            self.find("#lnkAccountEdit").show();
            self.find("#lblAccount").hide();
            self.find("#lnkPatientNameEdit").removeClass("hidden");
            self.find("#lblPatientName").addClass("hidden");
        } else {
            self.find("#lnkAccountEdit").hide();
            self.find("#lblAccount").show();
            self.find("#lnkPatientNameEdit").addClass("hidden");
            self.find("#lblPatientName").removeClass("hidden");
        }

    },

    BindPatientAccount: function () {
        var valid = false;
        var self = null;
        if (Encounter_CreateClaim.params["PanelID"] != 'pnlEncounterCreateClaim')
            self = $("#" + Encounter_CreateClaim.params["PanelID"] + " #pnlEncounterCreateClaim");
        else
            self = $("#" + Encounter_CreateClaim.params["PanelID"]);
        var Ctrl = self.find("#txtPatientName");
        var hfCtrl = self.find("#hfPatientId");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.AccountNumber && obj.AccountNumber.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.AccountNumber;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
                self.find("#lnkPatientNameEdit").removeClass("hidden");
                self.find("#lblPatientName").addClass("hidden");
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
                self.find("#lnkPatientNameEdit").addClass("hidden");
                self.find("#lblPatientName").removeClass("hidden");
            }

            Encounter_CreateClaim.enablePatientControls();
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            self.find("#txtPatientName").val(dataItem.AccountNumber);
            self.find("#txtPatientFullName").val(dataItem.FullName);
            Encounter_CreateClaim.patFullName = dataItem.AccountNumber + ' - ' + dataItem.FullName;
            Encounter_CreateClaim.SelfPay = dataItem.SelfPay == "" ? "False" : dataItem.SelfPay;
            Encounter_CreateClaim.RefProviderId = dataItem.RefferingProviderId;
            Encounter_CreateClaim.ReProviderName = dataItem.RefferingProviderName;
            $bootstrapValidator = null;
            if (self.find('#frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof self.find('#frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                $bootstrapValidator = self.find('#frmEncounterCreateClaim').data('bootstrapValidator');
            }

            if ($bootstrapValidator) {
                $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'PatientName');
            }

            if (dataItem.Providername != "") {
                self.find('#txtProvider').val(dataItem.Providername);
                self.find('#hfProvider').val(dataItem.ProviderID);
                if ($bootstrapValidator) {
                    $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'Provider');
                }
                if (self.find('#txtProvider').val() != "") {
                    self.find('#lnkProviderEdit').css("display", "inline");
                    self.find('#lblProvider').css("display", "none");
                }

            } else {
                self.find('#txtProvider').val("");
                self.find('#hfProvider').val("");
            }
            if (dataItem.Providername != "") {
                self.find('#txtResourceProvider').val(dataItem.Providername);
                self.find('#hfResourceProvider').val(dataItem.ProviderID);
                if ($bootstrapValidator) {
                    $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'ResourceProvider');
                }
                if (self.find('#txtResourceProvider') != "") {
                    self.find('#lnkResourceProviderEdit').css("display", "inline");
                    self.find('#lblResourceProvider').css("display", "none");
                }

            } else {
                self.find('#txtResourceProvider').val("");
                self.find('#hfResourceProvider').val("");
            }
            if (dataItem.Facilityname != "") {
                self.find('#txtFacility').val(dataItem.Facilityname);
                self.find('#hfFacility').val(dataItem.FacilityID);
                if ($bootstrapValidator) {
                    $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').bootstrapValidator('revalidateField', 'Facility');
                }
                if (self.find('#txtFacility').val() != "") {
                    self.find('#lnkFacilityEdit').css("display", "inline");
                    self.find('#lblFacility').css("display", "none");
                }

            } else {
                self.find('#txtFacility').val("");
                self.find('#hfFacility').val("");
            }
        }

        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 4,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArray(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },

    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = Encounter_CreateClaim.params["TabID"];
        if (RefCtrl == 'frmEncounterCreateClaim #txtProvider' || RefCtrl == 'frmEncounterCreateClaim #txtResourceProvider')
            params["IsOptional"] = false;
        else
            params["IsOptional"] = true;
        params["RefForm"] = 'frmEncounterCreateClaim';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {
        var params = [];
        params["ProviderId"] = $('#' + Encounter_CreateClaim.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('providerDetail', params);
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = 'frmEncounterCreateClaim';
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function (HiddenCtrl) {
        var params = [];
        params["FacilityId"] = $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('facilityDetail', params);
    },

    ValidateForm: function () {

        $("#" + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim')
           .bootstrapValidator({
               live: 'disabled',
               excluded: [':disabled'],
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   PatientName: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   Provider: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   Facility: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   DOSFrom: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DOSTo: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ResourceProvider: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   ClaimType: {
                       group: '.col-lg-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               Encounter_CreateClaim.LoadChargeCapture();

           });
    },

    ValidateValues: function () {
        if (!utility.ValidateAutoComplete($('#' + Encounter_CreateClaim.params["PanelID"] + " #txtProvider"), Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim #hfProvider', false)) {
            return false;
        }
        if (!utility.ValidateAutoComplete($('#' + Encounter_CreateClaim.params["PanelID"] + " #txtFacility"), Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim #hfFacility', false)) {
            return false;
        }
        if (!utility.ValidateAutoComplete($('#' + Encounter_CreateClaim.params["PanelID"] + " #txtResourceProvider"), Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim #hfResourceProvider', false)) {
            return false;
        }
        else
            return true;
    },

    LoadChargeCapture: function () {
        var self = null;
        if (Encounter_CreateClaim.params.PanelID.indexOf("pnlEncounterCreateClaim") < 0)
            self = $('#' + Encounter_CreateClaim.params.PanelID + " #pnlEncounterCreateClaim");
        else
            self = $('#' + Encounter_CreateClaim.params.PanelID);
        var claimType = self.find('#ddlClaimType option:selected').text();
        if (claimType && claimType.toLowerCase() != "institutional") {
            //Start PRD-635
            var objData = {};
            objData['DOSFrom'] = self.find('#dtpDOSFrom').val();
            objData['DOSTo'] = self.find('#dtpDOSTo').val();
            objData['PatientId'] = self.find('#hfPatientId').val();
            objData["providerId"] = self.find('#hfProvider').val();
            objData["facilityId"] = self.find('#hfFacility').val();
            var myJson = JSON.stringify(objData);
            Encounter_CreateClaim.VerifyDuplicateClaim_DBCall(myJson).done(function (response) {
                if (response.status != false) {
                    var ClaimJSON = JSON.parse(response.VisitsLoad_JSON);
                    utility.myConfirmDetail("The Claim# " + Encounter_CreateClaim.BindClaimNumbersWFunction('Encounter_CreateClaim', ClaimJSON) + " already exist for this patient with same DOS, Facility & Provider. Do you want to create a new claim?", function () {
                        Encounter_CreateClaim.LoadClaimDetailScreen(self, claimType);
                    }, function () {
                        UnloadActionPan(Encounter_CreateClaim.params.ParentCtrl, "Encounter_CreateClaim");
                    }, 'Duplicate Claim Alert!');
                }
                else
                    Encounter_CreateClaim.LoadClaimDetailScreen(self, claimType);
            });
            //End PRD-635
        }
        else {
            self.find('#ddlClaimType').focus();
            utility.DisplayMessages("Please select 'Professional Medical' Or 'Professional Anesthesia ' Claim Type", 3);
        }
    },
    //Start PRD-635
    BindClaimNumbersWFunction: function (CtrlId, ClaimJSON, IsOkBtnOnly) {
        var ClaimJsonlength = ClaimJSON.length;
        var ClaimNumberHyperlink = '';
        $.each(ClaimJSON, function (i, item) {
            var VisitDetailFunc = '';
            if (CtrlId.toLowerCase() == 'encounter_createclaim')
                VisitDetailFunc = "Encounter_CreateClaim.LoadVisitDetail(" + item.VisitId + ", " + item.PatientId + ", event)";
            else if (CtrlId.toLowerCase() == 'encounterchargecapture')
                VisitDetailFunc = "EncounterChargeCapture.LoadVisitDetail(" + item.VisitId + ", " + item.PatientId + ", " + IsOkBtnOnly + ", event)";
            ClaimNumberHyperlink += "<a href='#' onclick='" + VisitDetailFunc + "' title='View Claim Detail'>" + item.ClaimNumber + "</a>, ";
        });
        return ClaimNumberHyperlink.substring(0, ClaimNumberHyperlink.length - 2);
    },

    VerifyDuplicateClaim_DBCall: function (ClaimData) {
        var data = "ClaimData=" + ClaimData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "VERIFY_DUPLICATE_CLAIM");
    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $('#modal-from-dom.modal').addClass('modal-backdrop');
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Encounter_CreateClaim';
                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params, Encounter_CreateClaim.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //End PRD-635
    LoadClaimDetailScreen: function(self, claimType){
        UnloadActionPan(Encounter_CreateClaim.params.ParentCtrl, "Encounter_CreateClaim");
        setTimeout(function () {
            var params = [];
            params["FromAdmin"] = 0;
            if (Encounter_CreateClaim.params.ParentCtrl == "Bill_ChargeSearch") {
                params["ParentCtrl"] = 'Bill_ChargeSearch';
                if (Bill_ChargeSearch.params["CaseId"]) {
                    params["CaseId"] = Encounter_CreateClaim.params["CaseId"];
                    params["CaseNo"] = Encounter_CreateClaim.params["CaseNo"];
                }
            }
            else if (Encounter_CreateClaim.params.ParentCtrl == "Patient_Case_Detail") {
                params["ParentCtrl"] = 'Patient_Case_Detail';
            }
            else if (Encounter_CreateClaim.params.ParentCtrl == "billTabUnClaimedAppointment") {
                params["ParentCtrl"] = 'billTabUnClaimedAppointment';
            }
            params['mode'] = "Add";
            params['DOSFrom'] = self.find('#dtpDOSFrom').val();
            params['DOSTo'] = self.find('#dtpDOSTo').val();
            params['PatientId'] = self.find('#hfPatientId').val();
            params['FromCreateClaim'] = true;
            params['BillingProvider'] = self.find("#hfBillingProvider").val();
            params['ClaimType'] = claimType.toLowerCase();
            params['SpecialityName'] = "";//self.find('#hfSpecialityName').val();

            var parmsFromCreateClaims = [];
            parmsFromCreateClaims["patientId"] = self.find('#hfPatientId').val();
            parmsFromCreateClaims["patFullName"] = Encounter_CreateClaim.patFullName;
            parmsFromCreateClaims["RefProviderId"] = Encounter_CreateClaim.RefProviderId;
            parmsFromCreateClaims["ReProviderName"] = Encounter_CreateClaim.ReProviderName;
            parmsFromCreateClaims["providerId"] = self.find('#hfProvider').val();
            parmsFromCreateClaims["providerName"] = self.find('#txtProvider').val();
            parmsFromCreateClaims["facilityId"] = self.find('#hfFacility').val();
            parmsFromCreateClaims["facilityName"] = self.find('#txtFacility').val();
            parmsFromCreateClaims["billingProviderId"] = self.find('#hfBillingProvider').val();
            parmsFromCreateClaims["selfPay"] = Encounter_CreateClaim.SelfPay;
            parmsFromCreateClaims["resourceProviderId"] = self.find('#hfResourceProvider').val();
            parmsFromCreateClaims["resourceProviderName"] = self.find('#txtResourceProvider').val();
            parmsFromCreateClaims["DOB"] = Encounter_CreateClaim.PatientDOB;
            parmsFromCreateClaims["Gender"] = Encounter_CreateClaim.PatientGender;
            params["parmsFromCreateClaims"] = parmsFromCreateClaims;
            LoadActionPan('EncounterChargeCapture', params);
        }, 700);
    },

    SaveVisit: function (VisitData, PatientID) {



        var data = "VisitData=" + VisitData + "&PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SAVE_VISIT");


    },

    UnLoad: function () {
        if (Encounter_CreateClaim.params != null && Encounter_CreateClaim.params.ParentCtrl != null) {
            if (Encounter_CreateClaim.params.ParentCtrl == "chargeBatchDetail") {
                var parentCtrl = Encounter_CreateClaim.params.ParentCtrl;
                if (Encounter_CreateClaim.params["hyperlink"] == true) {
                    UnloadActionPan(parentCtrl, 'Encounter_CreateClaim');
                }
                else {
                    if ($('#frmEncounterCreateClaim').serialize() != $('#frmEncounterCreateClaim').data('serialize')) {
                        utility.myConfirm('2', function () {
                            UnloadActionPan(parentCtrl, 'Encounter_CreateClaim', true);
                            Encounter_CreateClaim.params = null;
                            //UnloadActionPan(null, 'Encounter_CreateClaim', true);
                            ChargeBatch_Viewer.ActiveBtns(true);
                        }, function () {
                        },
                                '2'
                            );
                    }
                    else {
                        UnloadActionPan(parentCtrl, 'Encounter_CreateClaim', true);

                        Encounter_CreateClaim.params = null;
                        //UnloadActionPan(null, 'Encounter_CreateClaim', true);
                        ChargeBatch_Viewer.ActiveBtns(true);
                    }
                }


                //utility.myConfirm('2', function () {
                //    UnloadActionPan(parentCtrl, 'Encounter_CreateClaim', true);
                //    ChargeBatch_Viewer.ActiveBtns(true);
                //}, function () { },
                //    '2'
                //);

            }
            else if (Encounter_CreateClaim.params.ParentCtrl == "Bill_PaymentPosting") {
                // Existance of Encounter Charge Capture multiple times
                var occurance = $('body #' + Encounter_CreateClaim.params.PanelID).length;
                if (occurance > 1) {
                    var selectedTab = GetCurrentSelectedTab();
                    var parentPanelID = selectedTab.PanelID; //+ ' #' + Encounter_CreateClaim.params.PanelID;
                    UnloadActionPan(Encounter_CreateClaim.params.ParentCtrl, 'Encounter_CreateClaim', null, parentPanelID);

                }
                else
                    UnloadActionPan(Encounter_CreateClaim.params.ParentCtrl, 'Encounter_CreateClaim');


            }

            else if (Encounter_CreateClaim.params.ParentCtrl == "Bill_ChargeSearch" || Encounter_CreateClaim.params.TabID == 'Patient_Case_Detail' || Encounter_CreateClaim.params.ParentCtrl == "billTabUnClaimedAppointment" || Encounter_CreateClaim.params.ParentCtrl == "schTabCalendar" || Encounter_CreateClaim.params.TabID == 'batchTabEncounter' || Encounter_CreateClaim.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || Encounter_CreateClaim.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment') {
                var parentCtrl = Encounter_CreateClaim.params.ParentCtrl;


                if ($('#frmEncounterCreateClaim').serialize() != $('#frmEncounterCreateClaim').data('serialize')) {
                    utility.myConfirm('2', function () {
                        UnloadActionPan(parentCtrl, 'Encounter_CreateClaim');

                        Encounter_CreateClaim.params = null;
                        //UnloadActionPan(null, 'Encounter_CreateClaim');

                    }, function () {
                    },
                            '2'
                        );
                }
                else {
                    UnloadActionPan(parentCtrl, 'Encounter_CreateClaim');

                    Encounter_CreateClaim.params = null;
                    //UnloadActionPan(null, 'Encounter_CreateClaim');

                }

                //utility.myConfirm('2', function () {
                //    UnloadActionPan(parentCtrl, 'Encounter_CreateClaim');

                //}, function () { },
                //    '2'
                //);

            }
            else {
                UnloadActionPan(Encounter_CreateClaim.params.ParentCtrl, 'Encounter_CreateClaim');

                Encounter_CreateClaim.params = null;
            }
        }
        else {
            // adnan maqbool,PMS-3997 , 17-02-2016
            UnloadActionPan(null, 'Encounter_CreateClaim');
            if (Encounter_CreateClaim.params != null && Encounter_CreateClaim.params.ParentCtrl != null)

                //end
                Encounter_CreateClaim.params = null;
        }


    },

    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        $(CtrlToDate).attr('disabled', true);
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    //utility.ShowMessage(3, "To date is smaller than from date");
                }
            }
            $(CtrlToDate).attr('disabled', false);

            // $(CtrlToDate).datepicker('remove');
            if (!IsOptional) {
                if ($('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
                }

            }


            $(this).datepicker('hide');
            if (!IsOptional) {
                if ($('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
                }
            }


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }


            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            // startDate: inputDate,
            format: date_format,
            //todayBtn: 'linked',
        }).change(function (e) {

            $(this).datepicker('hide');
            if (!IsOptional) {
                if ($('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != null && typeof $('#' + Encounter_CreateClaim.params["PanelID"] + ' #frmEncounterCreateClaim').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
                }
            }

            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                            //utility.ShowMessage(3, "To date is smaller than from date")
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        });

        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {
                   if ($(CtrlFromDate).val() != '') {
                       utility.ValidateDate(CtrlFromDate);
                   }

               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {
                if ($(CtrlToDate).val() != '') {
                    utility.ValidateDate(CtrlToDate);
                }
            }, 100);
        });
    },

    OpenEncounterChargeCapture: function () {

    },
    OpenBillingProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = Encounter_CreateClaim.params["TabID"];
        if (RefCtrl == 'frmEncounterCreateClaim #txtProvider' || RefCtrl == 'frmEncounterCreateClaim #txtResourceProvider')
            params["IsOptional"] = false;
        else
            params["IsOptional"] = true;
        params["RefForm"] = 'frmEncounterCreateClaim';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('Admin_BillingProvider', params);
    },

    OpenBillingProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {
        var params = [];
        params["BillingProviderId"] = $('#' + Encounter_CreateClaim.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        if (Encounter_CreateClaim.params.TabID == 'Bill_ChargeSearch' || Encounter_CreateClaim.params.ParentCtrl == 'Patient_Search' || Encounter_CreateClaim.params.ParentCtrl == 'billTabUnClaimedAppointment')
            params["ParentCtrl"] = 'Encounter_CreateClaim';
        else
            params["ParentCtrl"] = Encounter_CreateClaim.params["TabID"];
        LoadActionPan('Admin_BillingProvider_Detail', params);
    },
}