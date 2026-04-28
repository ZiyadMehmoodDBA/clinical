Clinical_SuperBillTemplate = {

    //Author: Abid Ali
    //Date: 20-07-2016
    //This file will handle load of super bill templates from admin section
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    CPTCodes: [],
    ICDCodes: [],
    isDataExists: false,

    Load: function (params) {

        Clinical_SuperBillTemplate.params = params;
        var self = $("#pnlClinicalSuperBillTemplate");
        var form = "#pnlClinicalSuperBillTemplate #frmSuperBillTemplate";

        //Set PanelId If changed
        if (Clinical_SuperBillTemplate.params.PanelID != 'pnlClinicalSuperBillTemplate') {
            Clinical_SuperBillTemplate.params.PanelID = Clinical_SuperBillTemplate.params.PanelID + ' #pnlClinicalSuperBillTemplate';
        } else {
            Clinical_SuperBillTemplate.params.PanelID = 'pnlClinicalSuperBillTemplate';
        }
        if (Clinical_SuperBillTemplate.params.ParentCtrl == 'schTabCalendar') {
            $("#pnlClinicalSuperBillTemplate #btnSaveTemplate").addClass('hidden');
        }
        //Begin Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
        CacheManager.BindDropDownsByID($(form + ' #ddlSuperBillTemplate'), 'GetSupperBill', false, 0).done(function () {
            self.find('#ddlSuperBillTemplate option').filter(function () {
                return $.trim($(this).val()) == globalAppdata.DefaultSuperBill
            }).attr('selected', true);

        });
        //End Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
        //$(form).data('serialize', $(form).serialize()); 
        //Begin 09-12-2016 Edit By Muhammad Zain ul abdin Bug# EMR-2182
        if (globalAppdata.DefaultSuperBill == "") {
            Clinical_SuperBillTemplate.loadSupperBill(-1);
        }
        else {
            Clinical_SuperBillTemplate.loadSupperBill(globalAppdata.DefaultSuperBill);
        }
        //End 09-12-2016 Edit By Muhammad Zain ul abdin Bug# EMR-2182
        // Clinical_SuperBillTemplate.params.PatientId = 332896;
        // Clinical_SuperBillTemplate.params.NoteId = 213;
    },

    appendCodes: function (obj) {
        var selectedValue = $(obj).val() == "" ? -1 : $(obj).val();
        //get codes by template id
        Clinical_SuperBillTemplate.loadSupperBill(selectedValue);
    },

    loadSupperBill: function (superbillId) {

        Clinical_SuperBillTemplate.fillSupperBill(superbillId).done(function (response) {
            if (response.status != false) {
                Clinical_SuperBillTemplate.supperBillGridLoad(response);
                if (Clinical_SuperBillTemplate.isDataExists) {
                    $("#pnlClinicalSuperBillTemplate").find('#btnSavePrint').show();
                    $("#pnlClinicalSuperBillTemplate").find('#girdCPTICD').parent().show();
                }
            }
            else {
                Clinical_SuperBillTemplate.clearGrids();
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    supperBillGridLoad: function (response) {

        var TitleLoadJSONData = JSON.parse(response.Title_JSON);
        var modifier_list = JSON.parse(response.Modifier_JSON);
        var Icd_list = JSON.parse(response.ICD_JSON);
        var cpt_list = JSON.parse(response.CPT_JSON);

        Clinical_SuperBillTemplate.clearGrids();
        //Title
        $.each(TitleLoadJSONData, function (i, item) {

            //Modifier
            if (item.TitleType == "Modifier") {
                //Add Title 
                // SupperBillDetail.AddTitle(item, "#pnlSB_Modifier_Result #dgvModifier_SB");
                //Add Modifier of that title.
                //  SupperBillDetail.AddModifierCode(item.SBTitleId, modifier_list);

            } //ICD
            else if (item.TitleType == "ICD") {
                //Add Title 
                Clinical_SuperBillTemplate.AddTitle(item, "#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes");
                //Add ICD of that title.
                Clinical_SuperBillTemplate.addICDCode(item.SBTitleId, Icd_list);


            } //CPT
            else if (item.TitleType == "CPT") {
                //Add Title 
                Clinical_SuperBillTemplate.AddTitle(item, "#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes");
                //Add CPT of that title.
                Clinical_SuperBillTemplate.addCPTCode(item.SBTitleId, cpt_list);
            }

        });
    },

    AddTitle: function (item, tableId) {
        var $row = $('<tr  Id="' + item.SBTitleId + '" class="r_title" />');
        $row.attr("onclick", "utility.SelectGridRow($(this))");
        $row.append('<td colspan="3" >' + item.Description + '</td>');
        $row.css('font-weight', 'bold');
        if (tableId.indexOf("pnlICDCodes") >= 0)
            $row.find("td").attr("colspan", "4");

        $(tableId + " tbody").last().append($row);

    },

    clearGrids: function () {
        //Clear  ICD Grid
        $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes tbody").find("tr").remove();
        $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes").find('th#ColumnMasterCheckbox  input').prop('checked', false);
        //Clear CPT Grid
        $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes tbody").find("tr").remove();
        $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes").find('th#ColumnMasterCheckbox input').prop('checked', false);
        //hide header units
        $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes").find('th#thCPTUnits').addClass('hidden');

        $("#pnlClinicalSuperBillTemplate").find('#btnSavePrint').hide();
        $("#pnlClinicalSuperBillTemplate").find('#girdCPTICD').parent().hide();
        Clinical_SuperBillTemplate.isDataExists = false;

    },

    addICDCode: function (TitleId, Codeitem) {

        $.each(Codeitem, function (i, item) {

            if (item.SBTitleId == TitleId) {

                if (!Clinical_SuperBillTemplate.checkICDIfAppend(item)) {

                    var $row = $('<tr sortId="' + item.SortId + '" titleId="' + item.SBTitleId + '" Id="' + item.SBICDId + '" />');
                    $row.attr("onclick", "utility.SelectGridRow($(this))");
                    $row.data("Code", item);
                    var onClick = "Clinical_SuperBillTemplate.pushCodes(this,false)";
                    var checkbox = '<input name="ICD" onclick=' + onClick + ' type="checkbox" id="checkbox_' + item.SBICDId + '"/>'
                    //$row.append('<td style="vertical-align: middle;" class="actions sorting_1" >' + checkbox + '</td><td style="vertical-align: middle;" class="code" >' + item.ICDCode + '</td><td style="vertical-align: middle;">' + item.ICD10 + '</td><td style="vertical-align: middle;">' + item.Description + '</td>');
                    $row.append('<td style="vertical-align: middle;" class="actions sorting_1" >' + checkbox + '</td><td style="vertical-align: middle;">' + item.ICD10 + '</td><td style="vertical-align: middle;">' + item.Description + '</td>');

                    $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes tbody").last().append($row);
                }

                Clinical_SuperBillTemplate.isDataExists = true;
            }

        });

    },

    checkICDIfAppend: function (Code, IsCPT) {
        var $tbody = $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes tbody tr");
        if (IsCPT) {

        }
        else {
            var isICDExists = false;
            $tbody.each(function () {
                var $this = $(this);
                var item = $this.data("Code");
                if (item != null && item.ICDCode == Code.ICDCode && item.Description == Code.Description && item.ICD10 == Code.ICD10) {
                    isICDExists = true;
                    return false;
                }
            });
            return isICDExists;
        }


    },

    addCPTCode: function (TitleId, Codeitem) {
        $.each(Codeitem, function (i, item) {

            if (item.SBTitleId == TitleId) {

                var $row = $('<tr sortId="' + item.SortId + '" titleId="' + item.SBTitleId + '" Id="' + item.SBCPTId + '" />');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                item["CPTUnit"] = "1";
                $row.data("Code", item);
                var onClick = "Clinical_SuperBillTemplate.pushCodes(this,true)";
                var checkbox = '<input name="CPT" onclick=' + onClick + ' type="checkbox" id="checkbox_' + item.SBCPTId + '"/>'
                var unitTextBox = '<input onkeydown="Clinical_SuperBillTemplate.allowDigitsOnly(event);" oninput="Clinical_SuperBillTemplate.checkMaxLength(this)" maxlength = "2" class="size40 hidden" onblur="Clinical_SuperBillTemplate.pushUnits(this);" name="CPTUnit" min="1" max="99" type="number" id="textUnit_' + item.SBCPTId + '" value="1"/>'
                $row.append('<td style="vertical-align: middle;" class="actions sorting_1" >' + checkbox + '</td><td style="vertical-align: middle;" class="code" >' + item.CPTCode + '</td><td style="vertical-align: middle;">' + item.Description + '</td><td style="vertical-align: middle;" class="hidden">' + unitTextBox + '</td>');

                $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes tbody").last().append($row);
                Clinical_SuperBillTemplate.isDataExists = true;
            }

        });
    },

    pushCodes: function (obj, isCPT) {
        var $tr = $(obj).parent().parent();
        var Code = $tr.data("Code");

        var isChecked = $(obj).prop('checked');

        if (isChecked) {
            //Push CPT Codes if not exists
            if (isCPT) {

                if (!Clinical_SuperBillTemplate.isCPTExists(Code)) {
                    Clinical_SuperBillTemplate.CPTCodes.push(Code);
                }
                Clinical_SuperBillTemplate.showHideCPTUnits();

            }
                //Push ICD Codes if not exists
            else {
                if (Clinical_SuperBillTemplate.ICDCodes.length < 12) {
                    if (!Clinical_SuperBillTemplate.isICDExists(Code)) {
                        Clinical_SuperBillTemplate.ICDCodes.push(Code);
                    }
                    //mark header check box to true/false
                    if (Clinical_SuperBillTemplate.allRowsMarked(false)) {
                        $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes").find('th#ColumnMasterCheckbox  input').prop('checked', true);
                    }
                }
                else {
                    utility.DisplayMessages("A maximum of 12 Diagnosis Codes can be selected", 3);
                    $(obj).prop('checked', false);
                }
            }
        }
        else {
            if (isCPT) {
                Clinical_SuperBillTemplate.removeFromCodes(Clinical_SuperBillTemplate.CPTCodes, Code, true);
                Clinical_SuperBillTemplate.showHideCPTUnits();
            }
            else {
                Clinical_SuperBillTemplate.removeFromCodes(Clinical_SuperBillTemplate.ICDCodes, Code, false);
                if (Clinical_SuperBillTemplate.ICDCodes.length == 0) {
                    $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes").find('th#ColumnMasterCheckbox  input').prop('checked', false);
                }
                if (!Clinical_SuperBillTemplate.allRowsMarked(false)) {
                    $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes").find('th#ColumnMasterCheckbox  input').prop('checked', false);
                }
            }
        }
    },

    isCPTExists: function (CPT) {
        var isCPTExists = false;
        $.each(Clinical_SuperBillTemplate.CPTCodes, function (index, item) {
            if (item.CPTCode == CPT.CPTCode && item.Description == CPT.Description) {
                isCPTExists = true;
                return false;
            }
        });
        return isCPTExists;
    },

    isICDExists: function (ICD) {
        var isICDExists = false;
        $.each(Clinical_SuperBillTemplate.ICDCodes, function (index, item) {
            if (item.ICDCode == ICD.ICDCode && item.Description == ICD.Description && item.ICD10 == ICD.ICD10) {
                isICDExists = true;
                return false;
            }
        });
        return isICDExists;
    },

    removeFromCodes: function (Codes, object, isCPT) {

        if (object != null) {
            $.each(Codes, function (index, item) {
                if (isCPT) {
                    if (item.CPTCode == object.CPTCode && item.Description == object.Description) {
                        Clinical_SuperBillTemplate.CPTCodes.splice(index, 1);
                        return false;
                    }
                }
                else {
                    if (item.ICDCode == object.ICDCode && item.Description == object.Description && item.ICD10 == object.ICD10) {
                        Clinical_SuperBillTemplate.ICDCodes.splice(index, 1);
                        return false;
                    }
                }
            });
        }
    },

    markAllCPTCodes: function (obj, isCheked) {

        var isChecked = $(obj).prop('checked') || (isChecked != null);
        var $table = $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes tbody");
        if (isChecked) {
            //show header units
            $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes").find('th#thCPTUnits').removeClass('hidden');
        }
        else {

            //hide header units
            $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes").find('th#thCPTUnits').addClass('hidden');
        }
        $table.find("tr").each(function (index, item) {
            var Code = $(this).data('Code');
            if (isNaN(parseInt($(item).find('td:first').attr("colspan")))) {
                if (isChecked) {
                    //Check and remove class from units td

                    $(item).find('td:first input').prop('checked', true);
                    $(item).find('td:last').removeClass('hidden');
                    $(item).find('td:last input').removeClass('hidden');

                    //Push Codes
                    if (!Clinical_SuperBillTemplate.isCPTExists(Code)) {
                        Clinical_SuperBillTemplate.CPTCodes.push(Code);
                    }

                }
                else {
                    //unCheck and add class to units td
                    $(item).find('td:first input').prop('checked', false);
                    $(item).find('td:last').addClass('hidden');
                    $(item).find('td:last input').addClass('hidden');
                    //Remove all codes
                    Clinical_SuperBillTemplate.CPTCodes = [];
                }
            }
        });
    },

    markAllICDCodes: function (obj) {
        var isChecked = $(obj).prop('checked');
        var $table = $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes tbody");

        $table.find("tr").each(function (index, item) {
            var Code = $(this).data('Code');

            if (isNaN(parseInt($(item).find('td:first').attr("colspan")))) {

                if (isChecked) {
                    if (!Clinical_SuperBillTemplate.isICDExists(Code)) {
                        if (Clinical_SuperBillTemplate.ICDCodes.length < 12) {
                            //Check the td's checkbox
                            $(item).find('td:first input').prop('checked', true);

                            Clinical_SuperBillTemplate.ICDCodes.push(Code);
                        }
                        else {
                            ///  utility.DisplayMessages("A maximum of 12 Diagnosis Codes can be selected", 3);
                        }
                    }
                }
                else {
                    //Uncheck the td's checkbox
                    $(item).find('td:first input').prop('checked', false);
                    Clinical_SuperBillTemplate.ICDCodes = [];
                }
            }
        });
    },

    getMarkCheckboxesLength: function (isCPT) {
        var counter = 0;
        var $table = $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes tbody");
        if (isCPT) {
            $table = $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes tbody");

        }
        $table.find("tr").each(function (index, item) {

            if ($(item).find("td:first input").prop('checked')) {
                counter++;
            }
        });
        return counter;
    },

    getTotalRows: function (isCPT) {
        var $table = $("#pnlClinicalSuperBillTemplate #pnlICDCodes #tblDiagnosisCodes tbody");
        if (isCPT) {
            $table = $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes tbody");
            return $table.find("tr").find(" td:first:not([colspan=3])").length;
        }
        else {
            var rowsLength = $table.find("tr").find(" td:first:not([colspan=4])").length;
            return rowsLength > 12 ? 12 : rowsLength; //Checks the capacity
        }

    },

    allRowsMarked: function (isCPT) {
        var markedCheckboxes = 0;
        var totalRows = 0;
        if (isCPT) {
            markedCheckboxes = Clinical_SuperBillTemplate.getMarkCheckboxesLength(true);
            totalRows = Clinical_SuperBillTemplate.getTotalRows(true);
        }
        else {
            markedCheckboxes = Clinical_SuperBillTemplate.getMarkCheckboxesLength(false);
            totalRows = Clinical_SuperBillTemplate.getTotalRows(false);
        }
        return markedCheckboxes == totalRows;
    },

    pushUnits: function (obj) {

        var $tr = $(obj).parent().parent();
        var value = $(obj).val();
        if (parseInt(value) == 0) {
            value = 1;
            $(obj).val(value);
        }
        var Code = $tr.data("Code");
        Code["CPTUnit"] = value;
        $tr.data("Code", Code);
    },

    checkMaxLength: function (object) {

        if (object.value.length > object.maxLength)
            object.value = object.value.slice(0, object.maxLength)
    },

    allowDigitsOnly: function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    },

    UnLoad: function () {

        Clinical_SuperBillTemplate.CPTCodes = [];
        Clinical_SuperBillTemplate.ICDCodes = [];
        Clinical_SuperBillTemplate.isDataExists = false;

        UnloadActionPan(Clinical_SuperBillTemplate.params["ParentCtrl"], "actionPanClinicalSuperBillTemplate");

    },

    showHideCPTUnits: function () {

        var $table = $("#pnlClinicalSuperBillTemplate #pnlCPTCodes #tblCPTCodes")
        var $tbody = $table.find("tbody");

        var isRowChecked = false; //Falg For any row checked
        var headerCheckbox = $table.find('th#ColumnMasterCheckbox input');

        $tbody.find("tr").each(function (index, item) {
            isRowChecked = $(item).find('td:first input').prop('checked');
            if (isRowChecked) {
                return false;
            }
        });
        if (isRowChecked) {

            $table.find('th#thCPTUnits').removeClass('hidden');

            //mark header check box to true/false

            headerCheckbox.prop('checked', Clinical_SuperBillTemplate.allRowsMarked(true));

            $tbody.find("tr").each(function (index, item) {
                isRowChecked = $(item).find('td:first input').prop('checked');

                if (isRowChecked) {
                    $(item).find('td:last').removeClass('hidden');
                    $(item).find('td:last input').removeClass('hidden');
                }
                else {
                    $(item).find('td:last').removeClass('hidden');
                    $(item).find('td:last input').addClass('hidden');
                }

            });
        }
        else {
            $table.find('th#thCPTUnits').addClass('hidden');
            headerCheckbox.prop('checked', false);
            $tbody.find("tr").each(function (index, item) {
                if (isNaN($(item).find('td:first').attr('colspan'))) {
                    $(item).find('td:last').addClass('hidden');
                    $(item).find('td:last input').addClass('hidden');
                }


            });
        }
    },

    paitentInfoHeader: function () {

        var dfd = new $.Deferred();
        var patientId = $('#PatientProfile #hfPatientId').val();

        if (Clinical_SuperBillTemplate.params["PatientId"] != null) {
            patientId = Clinical_SuperBillTemplate.params["PatientId"];
        }
        Clinical_SuperBillTemplate.FillDemographic(patientId).done(function (response) {
            if (response.status != false) {
                dfd.resolve(response);
                return dfd.promise();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },

    pushCodesToBillingInfo: function () {
        BillingInformation.pushCPTICDCodes(Clinical_SuperBillTemplate.CPTCodes, Clinical_SuperBillTemplate.ICDCodes);
        Clinical_SuperBillTemplate.UnLoad();
    },

    //Start//============ Print Section===========

    PrintReport: function () {

        var html = Clinical_SuperBillTemplate.reGenerateHtml();
        Clinical_SuperBillTemplate.paitentInfoHeader().done(function (patientInfo) {
            var PatientProfileInfo = JSON.parse(patientInfo.DemographicFill_JSON);
            if (Clinical_SuperBillTemplate.params.appid != null) {
                Clinical_SuperBillTemplate.FillAppointment(Clinical_SuperBillTemplate.params.appid).done(function (response) {
                    if (response.status != false) {
                        var appointment_detail = JSON.parse(response.AppointmentFill_JSON);
                        var formHeaderHtml = html;
                        if (response.ReportHeaderInfo != '' || response.ReportFooterInfo != '') {
                            formHeaderHtml = response.ReportHeaderInfo + html + response.ReportFooterInfo;
                            Clinical_SuperBillTemplate.getPrintSuperBillPDF(response, formHeaderHtml);
                        } else {
                            Clinical_SuperBillTemplate.PrintWithoutCustomHeader(response, formHeaderHtml, PatientProfileInfo, '');
                        }
                    }

                    else {
                        utility.DisplayMessages(response.Message, 3);
                        BackgroundLoaderShow(false);
                    }

                });
            } else
                if (Clinical_SuperBillTemplate.params.NoteId != null && parseInt(Clinical_SuperBillTemplate.params.NoteId) > 0) {
                    Clinical_SuperBillTemplate.NotesPreview(Clinical_SuperBillTemplate.params.NoteId, Clinical_SuperBillTemplate.params.PatientId, Clinical_SuperBillTemplate.params.ProviderId).done(function (response) {

                        var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                        var providerData = JSON.parse(response.NoteHeaderProviderData);
                        var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "");
                        var DOS = utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate);
                        PatientProfileInfo.Provider = providerName;
                        var formHeaderHtml = html;
                        if (response.ReportHeaderInfo != '' && response.ReportFooterInfo != '') {
                            formHeaderHtml = response.ReportHeaderInfo + html + response.ReportFooterInfo;
                            Clinical_SuperBillTemplate.getPrintSuperBillPDF(response, formHeaderHtml);
                        } else {
                            Clinical_SuperBillTemplate.PrintWithoutCustomHeader(response, formHeaderHtml, PatientProfileInfo, '<li id="DOS">DOS: ' + DOS + '</li>');
                        }

                    });
                } else if (Clinical_SuperBillTemplate.params.PatientId != null && Clinical_SuperBillTemplate.params.PatientId > 0) {
                    Clinical_SuperBillTemplate.ReportHeaderPrint_DbCall(-1, Clinical_SuperBillTemplate.params.PatientId, 'eSuperbill').done(function (response) {
                        var formHeaderHtml = html;
                        response = JSON.parse(response);
                        if (response.Header != '' || response.Footer != '') {
                            formHeaderHtml = response.Header + html + response.Footer;
                            Clinical_SuperBillTemplate.getPrintSuperBillPDF(response, formHeaderHtml);
                        } else {
                            Clinical_SuperBillTemplate.PrintWithoutCustomHeader(response, formHeaderHtml, PatientProfileInfo, '');
                        }

                    });

                }

                else {
                    var response = { Header: "" };
                    var formHeaderHtml = html;
                    Clinical_SuperBillTemplate.PrintWithoutCustomHeader(response, formHeaderHtml, PatientProfileInfo, '');
                }


        });
    },
    PrintWithoutCustomHeader: function (response, formHeaderHtml, PatientProfileInfo, DOS) {
        var patientAge = "";
        if (PatientProfileInfo.Age) {
            patientAge = PatientProfileInfo.Age.split(',');
            if (parseInt((PatientProfileInfo.Age.split(',')[0]).split(' ')[1]) > 0) {

                patientAge = PatientProfileInfo.Age.split(',')[0]; //age in years
            } else if (parseInt((PatientProfileInfo.Age.split(',')[1]).split(' ')[1]) > 0) {
                patientAge = PatientProfileInfo.Age.split(',')[1]; //age in months
            } else {
                patientAge = PatientProfileInfo.Age.split(',')[2]; //age in days

            }
        }
        var form = "#pnlClinicalSuperBillTemplate #frmSuperBillTemplate";
        var templateName = $(form + ' #ddlSuperBillTemplate option:selected').text();
        //var DOB = 'DOB: ' + PatientProfileInfo.DOB;
        var patientInfo = patientAge + " " + PatientProfileInfo.Sex;
        var patientAddress = PatientProfileInfo.Address1 + ', ' + PatientProfileInfo.City + ', ' + PatientProfileInfo.State + ', ' + PatientProfileInfo.Zip;
        var PatientEmail = '';
        var PatientHomePhone = '';
        var PatientCellPhone = '';

        if (PatientProfileInfo.Email != null && PatientProfileInfo.Email != "") {
            PatientEmail = '<li id="PatientEmail">Email: ' + PatientProfileInfo.Email + '</li>';
        }
        if (PatientProfileInfo.HomeTel != null && PatientProfileInfo.HomeTel != "") {
            PatientHomePhone = '<li id="PatientHomePhone">Home Phone: ' + PatientProfileInfo.HomeTel + '</li>';
        }
        if (PatientProfileInfo.Cell != null && PatientProfileInfo.Cell != "") {
            PatientCellPhone = '<li id="PatientCellPhone">Cell Phone: ' + PatientProfileInfo.Cell + '</li>';
        }
        

        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + ' #ProviderList').html(

                                   '<li id="DOS">DOS: ' + date_format + '</li>' +

                                   '<li id="PatientProvider">Provider: ' + PatientProfileInfo.Provider + '</li>'
                                    );
        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + ' #PatientList').html(
                               '<li id="PatientName">' + PatientProfileInfo.FullName + '</li>' +
                               '<li id="PatientAccount"></li>' + PatientProfileInfo.AccountNo + '</li>' +
                                '<li id="PatientAge">' + patientInfo + '</li>' +
                               '<li id="PatientAddress">' + patientAddress + '</li>' +
                               PatientHomePhone +
                               PatientCellPhone +
                               PatientEmail
                                );
        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + ' #PracticeList').html(
                           '<li id="TemplateName">Superbill (' + templateName + ')</li>'

                           );

        Clinical_SuperBillTemplate.getPrintSuperBillPDF(response, formHeaderHtml);
    },

    //Start 19-01-2017 Humaira Yousaf for Bug# EMR-2354
    getPrintSuperBillPDF: function (response, contents) {

        if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + ' #printcall:visible').length == 0) {
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + ' #printcall').css('display', '');
        }
        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ulContent").html(contents);
        var imgLength = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall img").length;
        var images = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall img");

        var Logo = '';
        if (response.Header != null) {
            var logoHTML = $(response.Header);
            Logo = logoHTML.find('img').prop('src');
        } else if (response.ReportHeaderInfo != null) {
            var logoHTML = $(response.ReportHeaderInfo);
            Logo = logoHTML.find('img').prop('src');
        }
        var HeaderLogo = '';
        if (Logo != null && Logo != '') {
            $.each(images, function (i, item) {
                if (item.src == Logo) {
                    HeaderLogo = item.src;
                }

            });
        } else {
            if (images != null && images.length > 0) {
                HeaderLogo = images[0].src;
            }
        }


        var FooterText = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall footer").text().split('Generated by: ').join('');

        if (HeaderLogo !== null && HeaderLogo !== "") {
            var CustomLogo = '<img id="ClinicalReportsHeaderLogo" src="' + HeaderLogo + '" class="img-responsive" style="max-width: 350px;max-height:140px;">';
            var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', HeaderLogo);
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);
        }
        else {
            var defaultLogo = 'content/images/SHS-nav-logo-small-100.png';
            var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', defaultLogo);
          
            $(PageTemp).find('#ClinicalReportsHeaderLogo').css('max-height', '140px');
            $(PageTemp).find('#ClinicalReportsHeaderLogo').css('max-width', '350px');

            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);
        }

        // ----- footer 
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooter').text(FooterText);
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);

        }
        else {
            var footerText = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooter').text(footerText);
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);

        }
        //PracticeDiv start
        var practiceHTML = '';
        if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList").length > 0) {

            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList > li").length > 7) {
                $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList > li:nth-child(7)").nextAll("li").remove();
            }
            practiceHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList").length == 1 ? $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList")[0].outerHTML : $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList")[1].outerHTML;
        }
        var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#PracticeDiv').html(practiceHTML.split('#').join('\\#'));
        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);

        //if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList").length > 0) {
        //    var practiceHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList").length == 1 ? $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList")[0].outerHTML : $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PracticeList")[1].outerHTML;
        //    var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
        //    var PageTemp = $(insideHTML);
        //    $(PageTemp).find('#PracticeDiv').html(practiceHTML.split('#').join('\\#'));
        //    $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);
        //}
        //PracticeDiv end
        //PatientList start
        var patientHTML = '';
        if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList").length > 0) {
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList > li").length > 7) {
                $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList > li:nth-child(7)").nextAll("li").remove();
            }

            patientHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList").length == 1 ? $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList")[0].outerHTML : $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList")[1].outerHTML;
        }

        var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#PatientDiv').html(patientHTML.split('#').join('\\#'));
        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);

        //if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList").length > 0) {
        //    var patientHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList").length == 1 ? $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList")[0].outerHTML : $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientList")[1].outerHTML;
        //    var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();
        //    var PageTemp = $(insideHTML);
        //    $(PageTemp).find('#PatientDiv').html(patientHTML.split('#').join('\\#'));
        //    $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);
        //}
        //PatientList end
        //ProviderDiv start


        var providerHTML = '';
        if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList").length > 0) {

            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList > li").each(function () {
                if ($(this).html().split(' ').join('') == '') {
                    $(this).remove();
                }
            });

            if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList > li").length > 7) {
                $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList > li:nth-child(7)").nextAll("li").remove();
            }

            providerHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList").length == 1 ? $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList")[0].outerHTML : $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList")[1].outerHTML;
        }
        var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();;
        var PageTemp = $(insideHTML);
        $(PageTemp).find('#ProviderDiv').html(providerHTML.split('#').join('\\#'));
        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);


        //if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList").length > 0) {
        //    var providerHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList").length == 1 ? $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList")[0].outerHTML : $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #ProviderList")[1].outerHTML;
        //    var insideHTML = $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html();;
        //    var PageTemp = $(insideHTML);
        //    $(PageTemp).find('#ProviderDiv').html(providerHTML.split('#').join('\\#'));
        //    $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html(PageTemp);
        //}

        //ProviderDiv end

        var img = new Image();
        img.src = $(PageTemp).find('#ClinicalReportsHeaderLogo').attr('src');
        var srcWidth = img.width;
        var srcHeight = img.height;
        var maxWidth = 350;
        var maxHeight = 140;

        var headerImgHeight = 0;
        
        var topMargin = 0;
        var height = 0;

        var pracHeight = $(PageTemp).find('#PracticeDiv ul li:not(:empty)').length * 4.68;
        var logoHeight = 0;
        if (srcHeight > maxHeight) {
            headerImgHeight = Clinical_SuperBillTemplate.calculateHeaderImageSize(srcWidth, srcHeight, maxWidth, maxHeight).height;
            logoHeight = headerImgHeight * 0.26;
        }
        else {
            logoHeight = 140 * 0.26;
        }
            
        if (pracHeight < logoHeight) {
            pracHeight = logoHeight;
        }
      
        var ProvHeight = $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length * 4.68;
        var patHeight = $(PageTemp).find('#PatientDiv ul li:not(:empty)').length * 4.68;
        if (patHeight > ProvHeight) {
            height += pracHeight + patHeight;
        } else {
            height += pracHeight + ProvHeight;
        }

        if ($(PageTemp).find('#ProviderDiv ul li').length == 7 || $(PageTemp).find('#PatientDiv ul li').length == 7) {
            height = height - 2;
        }
      
        topMargin = height;

        $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientInfo").hide();
        if ($('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template .blueBorderPrint").length >= 1) {
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall .form-group").hide();
            $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall footer").hide();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                top: topMargin + "mm",
                right: "10mm",
                bottom: "30mm"
            },
            template: $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #page-template").html()
        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, '#pnlClinicalSuperBillTemplate #PreviewReportPrint', false, true);
                $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall #PatientInfo").hide();
                $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall footer").hide();
                $('#' + Clinical_SuperBillTemplate.params["PanelID"] + " #printcall").hide();
            });
        });

        // ------------------------------------- End Download functionality--------------------------------------
    },


    calculateHeaderImageSize: function (srcWidth, srcHeight, maxWidth, maxHeight) {

        var ratio = Math.min(maxWidth / srcWidth, maxHeight / srcHeight);
        return { width: srcWidth * ratio, height: srcHeight * ratio };
    },

    //End 19-01-2017 Humaira Yousaf for Bug# EMR-2354

    ReportHeaderPrint_DbCall: function (ProviderId, PatientId, formName) {
        var objData = {};
        objData["ProviderId"] = ProviderId == null || ProviderId == '' ? -1 : ProviderId;
        objData["PatientId"] = PatientId == null || PatientId == '' ? -1 : PatientId;
        objData["commandType"] = "GET_REPORT_HEADER_TAGS_HTML";
        if (formName == null) {
            formName = "Clinical Reports";
        }
        objData["FormName"] = formName;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },

    FillAppointment: function (AppointmentID) {
        var patientID = Clinical_SuperBillTemplate.params.PatientId;
        var providerID = Clinical_SuperBillTemplate.params.ProviderId;
        var data = "AppointmentID=" + AppointmentID + "&patientID=" + patientID + "&providerID=" + providerID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_APPOINTMENT");
    },

    reGenerateHtml: function () {

        var form = "#pnlClinicalSuperBillTemplate";
        var modal = $(form).clone();

        var tblCPT = $(modal).find('#tblCPTCodes').html()
        var tblICD = $(modal).find('#tblDiagnosisCodes').html();
        tblCPT = Clinical_SuperBillTemplate.modifyTableHtml(modal, true);
        $(modal).find('#tblCPTCodes').find('tbody').html(tblCPT)
        tblICD = Clinical_SuperBillTemplate.modifyTableHtml(modal, false);
        $(modal).find('#tblDiagnosisCodes').find('tbody').html(tblICD);
        $(modal).find('#btnSavePrint').remove();
        //$(modal).find('div.panel-body').removeClass('panel-body panel-featured');

        return $(modal).find('#girdCPTICD').removeClass('height-max400').parent().html();
    },

    modifyTableHtml: function (modal, isCPT) {

        var $tbody = isCPT ? $($(modal).find('#tblCPTCodes').find("tbody")) : $($(modal).find('#tblDiagnosisCodes').find("tbody"));
        var $headerCheckbox = $($(modal).find('th#ColumnMasterCheckbox input'));
        var $headerUnit = $($(modal).find('th#thCPTUnits'));

        $headerCheckbox.prop('checked', false).addClass('disabled').attr('onclick', "return false;");
        $headerUnit.removeClass('hidden');

        $tbody.find('tr').each(function (index, item) {
            $(item).removeClass('active').attr('onclick', "return false;");
            $(item).find('td:first input').prop('checked', false).addClass('disabled').attr('onclick', "return false;");
            if (isCPT) {

                if ($(item).find('td:last').attr("colspan") != "3") {
                    var unit = $('<input class="size40"  disabled type="text" value=""/>');
                    unit.css("background-color", "white");
                    $(item).find('td:last').removeClass('hidden');
                    $(item).find('td:last').html("");
                    $(item).find('td:last').html(unit);
                }

            }
        });
        return $tbody.html();
    },

    NotesPreview: function (NotesId, PatientId, ProviderId) {
        var dfd = new $.Deferred();
        Clinical_SuperBillTemplate.PreviewNotes(NotesId, PatientId, ProviderId).done(function (response) {
            if (response.status != false) {
                dfd.resolve(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },

    //End//============ Print Section=============


    //Start//=============== DB Calls=============

    fillSupperBill: function (BillID) {

        var data = "BillID=" + BillID;
        return MDVisionService.defaultService(data, "ADMIN_SUPPER_BILL_DETAIL", "FILL_SUPPERBILL");

    },

    FillDemographic: function (PatientID) {

        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "fill_patient_demographic";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },

    PreviewNotes: function (NotesID, PatientId, ProviderId) {
        var data = "NotesID=" + NotesID + "&PatientId=" + PatientId + "&ProviderId=" + ProviderId + "&FormName=eSuperbill";

        return MDVisionService.defaultService(data, "DASHBOARD", "PREVIEW_NOTES");
    },

    //End//===============DB Calls===============

}