Patient_CustomForm = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',

    Load: function (params) {
        Patient_CustomForm.params = params;
        //Patient_CustomForm.params["patientID"]
        if (Patient_CustomForm.params.PanelID != 'pnlPatientCustomForm') {
            Patient_CustomForm.params.PanelID = Patient_CustomForm.params.PanelID + ' #pnlPatientCustomForm';
        } else {
            Patient_CustomForm.params.PanelID = 'pnlPatientCustomForm';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Patient_CustomForm.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        var self = $('#' + Patient_CustomForm.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Patient_CustomForm.patientCustomFormSearch();
        });
    },


    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Patient_CustomForm.params["FromAdmin"] == "0") {
            if (Patient_CustomForm.params != null && Patient_CustomForm.params.ParentCtrl != null) {
                UnloadActionPan(Patient_CustomForm.params.ParentCtrl, 'Patient_CustomForm');
            }
            else
                UnloadActionPan(null, 'Patient_CustomForm');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    AddCustomForm: function () {
        AppPrivileges.GetFormPrivileges("CustomForm", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //params["mode"] = "Add";
                params["ParentCtrl"] = "Patient_CustomForm";
                params["FromAdmin"] = 0;
                params["mode"] = "Add";
                params["PatientId"] = Patient_CustomForm.params["patientID"];
                LoadActionPan("Select_CustomForm", params);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    patientCustomFormSearch: function (patientCustomFormId, PageNo, rpp) {
        var dfd = $.Deferred();
        if ($('#' + Patient_CustomForm.params.PanelID + " #pnlPatCustomForms_Result").css("display") == "none") {
            $('#' + Patient_CustomForm.params.PanelID + " #pnlPatCustomForms_Result").show();
        }
        Patient_CustomForm.searchPatientCustomForm(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Patient_CustomForm.patientCustomFormGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var TableControl = "pnlPatientCustomForm #dgvPatCustomForms";
                var PagingPanelControlID = "pnlPatientCustomForm #divPatCustomFormsPaging";
                var ClassControlName = "Patient_CustomForm";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.customFormCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Patient_CustomForm.patientCustomFormSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },

    searchPatientCustomForm: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        objData["FormName"] = $('#' + Patient_CustomForm.params.PanelID + " #txtCustomFormName").val();
        objData["PageNumber"] = PageNumber;
        Patient_CustomForm.Switch == 1 ? objData["Isactive"] = "1" : objData["Isactive"] = "0";
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PATIENT_CUSTOMFORMS";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val()
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    patientCustomFormGridLoad: function (response) {
        var dfd = $.Deferred();
        $("#pnlPatientCustomForm #pnlPatCustomForms_Result #dgvPatCustomForms").dataTable().fnDestroy();
        $("#pnlPatientCustomForm #pnlPatCustomForms_Result #dgvPatCustomForms tbody").find("tr").remove();

        if (response.customFormCount > 0) {
            var PatientCustomFormJSONData = response.listCustomForm;
            $.each(PatientCustomFormJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "utility.SelectGridRow($('#gvPatTL_row" + item.Patient_CustomForm_Id + "'))");
                $row.attr("patientCustomFormId", item.PatientCustomFormId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var CustomFormName = "'" + item.FormName + "'";

                if (item.IsSigned.toLowerCase() == "true") {
                    $row.attr("onclick", "Patient_CustomForm.OpenDocumentForPrint(" + item.PatDocID + ", event);");
                    $row.append('<td style="display:none;">' + item.PatientCustomFormId + '</td><td><a disabled="disabled" class="btn  btn-xs" href="#" onclick="Patient_CustomForm.customFormDelete(' + item.PatientCustomFormId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a disabled="disabled" class="btn btn-xs" href="javascript:void(0);" onclick="Patient_CustomForm.RowEdit(' + item.PatientCustomFormId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a disabled="disabled" class="btn  btn-xs" href="#" onclick="Patient_CustomForm.customFormActiveInactive(' + item.PatientCustomFormId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.FormName + '</td>' + '</td><td>' + item.ModifiedOn + " By " + item.ModifiedByName + '</td>');
                }
                else {
                    $row.attr("onclick", "Patient_CustomForm.RowEdit(" + item.PatientCustomFormId + ", event);");
                    $row.append('<td style="display:none;">' + item.PatientCustomFormId + '</td><td><a class="btn  btn-xs" href="#" onclick="Patient_CustomForm.customFormDelete(' + item.PatientCustomFormId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="javascript:void(0);" onclick="Patient_CustomForm.RowEdit(' + item.PatientCustomFormId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_CustomForm.customFormActiveInactive(' + item.PatientCustomFormId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.FormName + '</td>' + '</td><td>' + item.ModifiedOn + " By " + item.ModifiedByName + '</td>');
                }

                $("#pnlPatCustomForms_Result #dgvPatCustomForms tbody").last().append($row);
            });
        }
        else {
            $('#pnlPatientCustomForm #dgvPatCustomForms').DataTable({
                "language": {
                    "emptyTable": "No Patient Custom Form Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#' + Patient_CustomForm.params.PanelID + ' #dgvPatCustomForms'))
            ;
        else {
            $('#' + Patient_CustomForm.params.PanelID + " #dgvPatCustomForms").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

        }
        var isactive;
        var checked;
        if (Patient_CustomForm.Switch == 1) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "0";
            checked = '';
        }

        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                     '<input id="gridSwitchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Patient_CustomForm.activeInactiveCustomFormSearch(this);">' +
                      '</div><span class="pl-xs">Active</span>';
        $("#" + Patient_CustomForm.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
        dfd.resolve();
        return dfd;
    },
    OpenDocumentForPrint: function (PatDocID) {
        var params = [];
        params["PatientID"] = $('#PatientProfile #hfPatientId').val();
        params["PatDocID"] = PatDocID;
        params["FolderID"] = 0;
        params["mode"] = "Edit";
        params["ParentCtrl"] = "Patient_CustomForm";
        LoadActionPan('Document_Viewer', params);
    },
    RowEdit: function (patientCustomFormId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = 'Patient_CustomForm';
        PanelID = 'pnlPatientCustomForm';
        params["IsFromAdminOrNot"] = "0";
        params["IsAddToNote"] = false;
        params["Mode"] = "Edit";
        params["PatientCustomFormId"] = patientCustomFormId;
        LoadActionPan("Clinical_CustomFormsPreview", params, PanelID);
    },

    activeInactiveCustomFormSearch: function (obj) {

        var isactive = $(obj).attr('isactive');
        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Patient_CustomForm.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Patient_CustomForm.Switch = 1;
        }

        Patient_CustomForm.patientCustomFormSearch();
    },

    customFormDelete: function (formId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            if (formId > 0) {
                Patient_CustomForm.customFormDelete_DbCall(formId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Patient_CustomForm.patientCustomFormSearch();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });

    },
    customFormDelete_DbCall: function (formId) {
        var objData = {};
        objData["CustomFormId"] = formId;
        objData["commandType"] = "DELETE_PATIENT_CUSTOM_FORMS";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    customFormActiveInactive: function (formId, isActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            if (formId == "" || formId == "undefined") {
            }
            else {
                Patient_CustomForm.customFormActiveInactive_Dbcall(formId, isActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Patient_CustomForm.patientCustomFormSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
                        '3', null, null, null, isActive
                    );
    },
    customFormActiveInactive_Dbcall: function (formId, isActive) {
        var objData = {};
        objData["PatientCustomFormId"] = formId;
        objData["IsActive"] = isActive;

        objData["commandType"] = "PATIENT_CUSTOM_FORM_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    customFormFill_DBCall: function (formId) {
        var objData = {};
        objData["PatientCustomFormId"] = formId;
        objData["commandType"] = "FILL_PATIENT_CUSTOM_FORM";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    SaveCustomForm: function (IsSigned, NotUnload) {
        // If the form is invalid, submit it. The form won't actually submit;
        // this will just cause the browser to display the native HTML5 error messages.

        var dfd = $.Deferred();
        var $myForm = $('#frmPreview')
        if (!$myForm[0].checkValidity()) {
            $myForm.find(':submit').click()
            dfd.resolve();
            return;
        }
        var isValidProc = Clinical_CustomFormsPreview.validateProcedures();
        var isValidProbs = Clinical_CustomFormsPreview.validateProblems();
        var isValidMS = Clinical_CustomFormsPreview.validateMultiSelectValues();
        var isValidYesNo = Clinical_CustomFormsPreview.validateYesNo();
        var isValidTable = Clinical_CustomFormsPreview.validateTable();
        var isValidSS = Clinical_CustomFormsPreview.validateSingleSelect();
        if (isValidMS && isValidYesNo && isValidTable && isValidSS && isValidProc && isValidProbs) {
            //var customFormSentenceView = Clinical_CustomFormsPreview.getCustomFormSentenceView($('#pnlPatientCustomForm #customFormDetails'));
            
            $.each($('#pnlPatientCustomForm #customFormDetails').find("li"), function (i, item) {
                $(item).find("div:first").removeClass("showAllContent");
                $(item).find("div:first").css("width","");
                $(item).find("div:first").css("overflow", "");
                $(item).find("div:first").css("height", "");
            });
            $('#pnlPatientCustomForm #customFormDetails').find('input[id*=txtt_]').css({ 'border': '1px solid #CCC' });
            var customFormSentenceView = $('#pnlPatientCustomForm #customFormDetails').html();
            Patient_CustomForm.convertHtmlToBase64().done(function (Base64String) {
                if (Clinical_CustomFormsPreview.params.Mode == "Edit") {
                    Patient_CustomForm.UpdateCustomFormsDetails(customFormSentenceView, Base64String, IsSigned).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $.when(Patient_CustomForm.patientCustomFormSearch()).then(function () {
                                if (NotUnload) { }
                                else { Clinical_CustomFormsPreview.UnLoadTab(); }

                                dfd.resolve(response.CustomFormId);
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            dfd.resolve();
                        }
                    });
                } else {
                    Patient_CustomForm.SaveCustomFormsDetails(customFormSentenceView, Base64String, IsSigned).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $.when(Patient_CustomForm.patientCustomFormSearch()).then(function () {
                                if (NotUnload) { }
                                else { Clinical_CustomFormsPreview.UnLoadTab(); }
                                dfd.resolve(response.CustomFormId);
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            dfd.resolve();
                        }
                    });
                }
            });
        }
        else {
            dfd.resolve();
        }
        return dfd.then(function (CustomFormId) {
            return CustomFormId;
        });
    },

    getHeaderFooterInfo: function (patientId) {

        var objData = {};
        objData["PatientId"] = patientId;
        objData["commandType"] = "get_report_header_footer";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    printcustomForm: function (IsSigned) {
        var Domhtml = $('#pnlPatientCustomForm #containerCustomFormPreview').html();
        Patient_CustomForm.SaveCustomForm(IsSigned, true).done(function (CustomFormId) {
            var patientId = $('#PatientProfile #hfPatientId').val();
            if (IsSigned == '0') {
                Patient_CustomForm.getHeaderFooterInfo(patientId).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status == true) {
                        var html = Domhtml;
                        var HeaderLogo = response.HeaderLogo;
                        var FooterText = response.FooterText;

                        if (HeaderLogo != null && HeaderLogo !== "" && FooterText != null && FooterText !== "") {

                            var patientData = "";
                            if (response.PatientText != 'undefined' && response.PatientText != null)
                                patientData = a = response.PatientText.split('<br/>');
                            var newPatientText = '';
                            for (var i = 0; i < patientData.length; i++) {
                                if ($.trim(patientData[i]) != '') {
                                    newPatientText += '<li>' + patientData[i] + '</li>';
                                }
                            }

                            var providerData = "";
                            if (response.ProviderText != 'undefined' && response.ProviderText != null)
                                providerData = a = response.ProviderText.split('<br/>');
                            var newProviderText = '';
                            for (var i = 0; i < providerData.length; i++) {
                                if ($.trim(providerData[i]) != '') {
                                    newProviderText += '<li align="right">' + providerData[i] + '</li>';
                                }
                            }


                            var formHeaderHtml = '<div id="printcall">' +
                             '<div id="PatientInfo" class="col-xs-12 p-none">' +
                             '<div class="col-sm-4 col-lg-2 pull-left">' +
                               '<img src="' + HeaderLogo + '" class="img-responsive" height="100px" width="240px"></div>' +
                                  '<ul class="list-unstyled pull-right line-height-fix">' +
                                  '<li id="PatientPractice" class="text-right">' + response.PracticeText + '</li></ul>' +
                                   '<div class="clearfix"></div>' +
                                     '<div class="splitter m-none mt-xs">' +
                                     '<div class="spacer3"></div></div>' +
                                       '<div class="spacer3"></div>' +
                                  '<ul class="list-unstyled pull-left line-height-fix" >' +
                                      newPatientText + '</ul>' +
                                   '<ul class="list-unstyled pull-right line-height-fix">' +
                                  //'<li id="PatientProvider" align="right">' +
                                  newProviderText + '</ul>' +
                              '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                                       '<div class="spacer3"></div>' +
                              '<section class="">' +
                              '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                              '<div class="spacer10"></div><h4 id="templateHeader"></h4>';

                            var docType = '<!doctype html>';
                            var docCnt = formHeaderHtml;

                            var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                         + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                         + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                         + '</script>'
                         + '</head>';

                            var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                          + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                        + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                        + '<span id="ClinicalReportsFooter">' + FooterText + '</span>'
                         + '</div> </div>';

                            var html = docCnt;
                            var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
                            $("#CustomeformprintparentdivPatient").append(ProgressNoteSign);
                            $("#customformsign #txtTextField").each(function (i, e) {
                                var scrollHeight = e.scrollHeight;
                                if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                    $(e).height(scrollHeight + 50);
                                } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                    $(e).height(scrollHeight + 40);
                                } else {
                                    $(e).height(scrollHeight + 20);
                                }
                                $(e).parents('.toolcontroldiv').addClass('heightReset');
                            });
                            $("#customformsign #txtFreeText").each(function (i, e) {
                                var scrollHeight = e.scrollHeight;
                                if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                    $(e).height(scrollHeight + 130);
                                } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                    $(e).height(scrollHeight + 100);
                                } else {
                                    $(e).height(scrollHeight + 50);
                                }
                                $(e).parents('.toolcontroldiv').addClass('heightReset');
                            });
                            $('#customformsign span.required').remove();
                            $("#CustomeformprintparentdivPatient").removeClass('hidden');
                            //var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                            //var newWin = window.open("", "_blank", winAttr);
                            //writeDoc = newWin.document;
                            //writeDoc.open();
                            // writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                            ////writeDoc.close();

                            //////   Our changes End
                            //setTimeout(function () {
                            //    newWin.focus();
                            //    newWin.print();
                            //    newWin.close();
                            //}, 200);
                            $.each($("#customformsign").find("input[type=checkbox]"), function (i, item) {
                                if ($(item).parent().find("label.ellipses").length > 0) {
                                    $(item).parent().find("label.ellipses").removeClass("ellipses")
                                }
                            });
                            $.each($("#customformsign").find("ul#customFormDetails li"), function (i, item) {
                                $(item).find("div:first").addClass("showAllContent");
                            });
                            kendo.drawing.drawDOM("#customformsign", {
                                landscape: false,
                                scale: 0.6,
                                paperSize: "A4",
                                // margin: "2cm 3cm ",
                                margin: {
                                    left: "10mm",
                                    //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    top: "3mm",
                                    //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    right: "10mm",
                                    bottom: "15mm"
                                },
                                template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
                            }).then(function (group) {

                                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                                    $.each($('#pnlPatientCustomForm #customFormDetails').find("li"), function (i, item) {
                                        $(item).find("div:first").removeClass("showAllContent");
                                        $(item).find("div:first").css("width", "");
                                        $(item).find("div:first").css("overflow", "");
                                        $(item).find("div:first").css("height", "");
                                    });
                                    Patient_CustomForm.pdf = dataURL;
                                    var params = [];
                                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                                    params["PreviewPdf"] = true;
                                    utility.PDFViewer(params["PrintPDFDataURL"], true, 'PreviewReportPrint', true);
                                    //document.getElementById("PreviewReportPrint").contentWindow.print();
                                    $("#CustomeformprintparentdivPatient").addClass('hidden');
                                    $("#CustomeformprintparentdivPatient").html('');

                                });

                            });
                        }
                        else {
                            Patient_Demographic.FillDemographic(patientId).done(function (patientInfo) {

                                var PatientProfileInfo = JSON.parse(patientInfo.DemographicFill_JSON);
                                var demographic_detail = JSON.parse(patientInfo.DemographicFill_JSON);

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

                                //var form = "#pnlClinicalSuperBillTemplate #frmSuperBillTemplate";
                                //var templateName = $(form + ' #ddlSuperBillTemplate option:selected').text();
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

                                if (globalAppdata['DateFormat'])
                                    date_format = globalAppdata['DateFormat'];
                                var date = new Date();
                                date_format = date_format.replace("mm", date.getMonth() + 1);
                                date_format = date_format.replace("yyyy", date.getFullYear());
                                var day = "";
                                if (date.getDate().length < 2) {
                                    day = "0" + date.getDate();
                                }
                                else {
                                    day = date.getDate();
                                }
                                date_format = date_format.replace("dd", day);

                                //var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                                //      var providerData = JSON.parse(response.NoteHeaderProviderData);
                                //var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "");
                                //date_format = utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate);
                                //PatientProfileInfo.Provider = providerName;
                                setTimeout(function () {
                                    var html = Domhtml;
                                    var formHeaderHtml = '<div id="printcall">' +
                                            '<div id="PatientInfo">' +
                                            '<div class="col-sm-4 col-lg-2 pull-left">' +
                                            '<img src="content/images/SHS-nav-logo-small-100.png" class="img-responsive"></div>' +
                                                 '<ul class="list-unstyled pull-right line-height-fix">' +
                                                 '<li id="DOS" class="text-right">DOS: ' + date_format + '</li>' +
                                                 '<li id="PatientProvider" class="text-right"">Provider: ' + PatientProfileInfo.Provider + '</li></ul>' +
                                                  '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                                   '<div class="spacer3"></div>' +
                                                 '<ul class="list-unstyled pull-left line-height-fix">' +
                                                 '<li id="PatientName" >' + PatientProfileInfo.FullName + '</li>' +
                                                 '<li id="PatientAccount" ></li>' + PatientProfileInfo.AccountNo + '</li>' +
                                                  '<li id="PatientAge" >' + patientInfo + '</li>' +
                                                 '<li id="PatientAddress">' + patientAddress + '</li>' +
                                                 PatientHomePhone +
                                                 PatientCellPhone +
                                                 PatientEmail +
                                                 '</ul>' +

                                             '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                                       '<div class="spacer3"></div>' +
                                             '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                                             '<div class="spacer10"></div><h4 id="templateHeader"></h4>';

                                    var docType = '<!doctype html>';
                                    var docCnt = formHeaderHtml;

                                    //   var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                                    //+ '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                                    //+ '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                                    //+ '</script>'
                                    //+ '</head>';


                                    var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                                  + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                                + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                                 + 'Generated by:'
                                + '<span id="ClinicalReportsFooter"> MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page 1 of 1</span>'
                                 + '</div> </div>';
                                    var html = docCnt; //+ '</body></html>';
                                    var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
                                    $("#CustomeformprintparentdivPatient").append(ProgressNoteSign);
                                    $("#customformsign #txtTextField").each(function (i, e) {
                                        var scrollHeight = e.scrollHeight;
                                        if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                            $(e).height(scrollHeight + 50);
                                        } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                            $(e).height(scrollHeight + 40);
                                        } else {
                                            $(e).height(scrollHeight + 20);
                                        }
                                        $(e).parents('.toolcontroldiv').addClass('heightReset');
                                    });
                                    $("#customformsign #txtFreeText").each(function (i, e) {
                                        var scrollHeight = e.scrollHeight;
                                        if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                            $(e).height(scrollHeight + 130);
                                        } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                            $(e).height(scrollHeight + 100);
                                        } else {
                                            $(e).height(scrollHeight + 50);
                                        }
                                        $(e).parents('.toolcontroldiv').addClass('heightReset');
                                    });
                                    $('#customformsign span.required').remove();
                                    $("#CustomeformprintparentdivPatient").removeClass('hidden');
                                    $.each($("#CustomeformprintparentdivPatient #customformsign").find("input[type=checkbox]"), function (i, item) {
                                        if ($(item).parent().find("label.ellipses").length > 0) {
                                            $(item).parent().find("label.ellipses").removeClass("ellipses")
                                        }
                                    });
                                    $.each($("#CustomeformprintparentdivPatient #customformsign").find("ul#customFormDetails li"), function (i, item) {
                                        $(item).find("div:first").addClass("showAllContent");
                                    });
                                    kendo.drawing.drawDOM("#CustomeformprintparentdivPatient #customformsign", {
                                        landscape: false,
                                        scale: 0.6,
                                        paperSize: "A4",
                                        // margin: "2cm 3cm ",
                                        margin: {
                                            left: "10mm",
                                            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                            top: "3mm",
                                            //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                            right: "10mm",
                                            bottom: "15mm"
                                        },
                                        template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
                                    }).then(function (group) {

                                        kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                                            $.each($('#pnlPatientCustomForm #customFormDetails').find("li"), function (i, item) {
                                                $(item).find("div:first").removeClass("showAllContent");
                                                $(item).find("div:first").css("width", "");
                                                $(item).find("div:first").css("overflow", "");
                                                $(item).find("div:first").css("height", "");
                                            });
                                            Patient_CustomForm.pdf = dataURL;
                                            var params = [];
                                            params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                                            params["PreviewPdf"] = true;
                                            utility.PDFViewer(params["PrintPDFDataURL"], true, 'PreviewReportPrint', true);
                                            //document.getElementById("PreviewReportPrint").contentWindow.print();
                                            $("#CustomeformprintparentdivPatient").addClass('hidden');
                                            $("#CustomeformprintparentdivPatient").html('');
                                        });
                                    });
                                    //var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                                    //var newWin = window.open("", "_blank", winAttr);
                                    //writeDoc = newWin.document;
                                    //writeDoc.open();
                                    //writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                                    ////writeDoc.close();

                                    //////   Our changes End
                                    //setTimeout(function () {
                                    //    newWin.focus();
                                    //    newWin.print();
                                    //    newWin.close();
                                    //}, 200);
                                }, 100);


                            });
                        }
                        Clinical_CustomFormsPreview.UnLoadTab();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                if (CustomFormId > 0) {
                    Patient_CustomForm.GetPatientDocumentId(CustomFormId).done(function (PatientDocumentId) {
                        if (PatientDocumentId > 0) {
                            var documentCall = Document_Viewer.FillDocument(null, $('#PatientProfile #hfPatientId').val(), PatientDocumentId);
                            $.when(documentCall).done(function (response1) {
                                if (response1.status != false) {
                                    var document_details = JSON.parse(response1.DocumentLoad_JSON);
                                    utility.documentPrint(document_details.Base64FileStream);
                                }
                            });
                        }
                    });
                }
            }
        });
    },

    GetPatientDocumentId: function (CustomFormId) {

        var dfd = $.Deferred();
        Patient_CustomForm.GetPatientDocumentId_DB_CALL(CustomFormId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve(response.patDocId);
            }
            else {
                dfd.resolve(null);
                utility.DisplayMessages(response.Message, 3);
            }
        });

        return dfd.then(function (result) {
            return result;
        });
    },
    GetPatientDocumentId_DB_CALL: function (CustomFormId) {
        var objData = new Object();
        objData["CustomFormId"] = CustomFormId;
        objData["commandType"] = "get_patient_document_id";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    bindHeaderData: function () {
        debugger;
        var patientId = $('#PatientProfile #hfPatientId').val();
        Patient_CustomForm.getHeaderFooterInfo(patientId).done(function (response) {
            response = JSON.parse(response);

            if (response.status == true) {

                var HeaderLogo = response.HeaderLogo;
                var FooterText = response.FooterText;

                if (HeaderLogo != null && HeaderLogo != "" && FooterText != null && FooterText != "") {
                    //Start//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
                    if (FooterText != null && FooterText != '') {
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #printcall footer").remove();
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #printcall").append(FooterText);
                    }
                    var patientData = response.PatientText.split('<br/>');
                    var providerData = response.ProviderText.split('<br/>');
                    var practiceData = response.PracticeText.split('<br/>');
                    if (patientData.length > 0) {
                        var patientAccount = patientData[3] != "" ? patientData[3] : "";
                        var patientCell = patientData[4] != "" ? patientData[4] : "";
                        var patientName = patientData[0] != "" ? patientData[0] : "";
                        var age = patientData[1] != "" ? patientData[1] : "";
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PatientName").html(patientName);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PatientAge").html(age);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PatientAccount").html(patientAccount);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PatientPhone").html(patientCell);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PatientAddress").html(patientData[5]);
                    }
                    if (providerData.length > 0) {
                        var providerName = providerData[0] != "" ? providerData[0] : "";
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #ProviderName").html(providerName);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #Speciality").html(providerData[1].SpecialtyName);
                    }
                    if (practiceData.length > 0) {
                        var city = practiceData[0].City;
                        city += practiceData[0].State != "" ? ", " + practiceData[0].State : "";
                        city += practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : "";
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PracticeName").html(practiceData[0]);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PracticeAddress").html(practiceData[1]);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PracticeCity").html(practiceData[2]);
                        $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PracticePhone").html(practiceData[3]);
                    }
                } else {

                    $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #printcall > div:not(#PatientInfo)").remove()
                    $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #printcall header").remove();
                    $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #printcall").prepend(HeaderLogo);
                    $('#' + Clinical_CustomFormsPreview.params.PanelID + " #frmCustomFormPreview #PatientInfo").hide();
                }
            }
        });
    },

    getPrintnotePDF: function (isSignNote) {
        var def = $.Deferred();
        $('#printcall').html($('#pnlPatientCustomForm #customFormDetails'));

        var FooterText = $("#pnlClinicalCustomFormsPreview #printcall footer").text().split('Generated by: ').join('');
        $('#progressnotesign').removeClass("hidden");
        $('#progressnotesign').show();
        $("#pnlClinicalCustomFormsPreview #printcall").show();
        $("#pnlClinicalCustomFormsPreview #printcall").css("display", "inline");
        // ----- footer 
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(FooterText);
            $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html(PageTemp);
        }
        else {
            var footerText = $("#pnlClinicalCustomFormsPreview #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(footerText);
            $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html(PageTemp);

        }
        var insideHTML = $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html();
        var PageTemp = $(insideHTML);
        $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html(PageTemp);
        var height = 0;

        var topMargin = height <= 18 ? 18 : height + 3;

        if ($("#pnlClinicalCustomFormsPreview #page-templateLegacy .blueBorderPrint").length >= 1) {
            $("#pnlClinicalCustomFormsPreview #printcall .form-group").remove();
            $("#pnlClinicalCustomFormsPreview #printcall footer").remove();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM("#pnlClinicalCustomFormsPreview #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "3mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                Patient_CustomForm.pdf = dataURL;
                var params = [];
                params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                params["PreviewPdf"] = true;

                utility.PDFViewer(params["PrintPDFDataURL"], false, 'PreviewReportPrint', true);
                def.resolve('ok');

                $("#pnlClinicalCustomFormsPreview #printcall").hide();

            });

        });
        // ------------------------------------- End Download functionality--------------------------------------



        return def.promise();


    },

    SaveCustomFormsDetails: function (customFormHTML, Base64String, IsSigned) {
        var objData = {};
        objData["commandType"] = "SAVE_PATIENT_CUSTOM_FORM";
        objData["CustomFormId"] = Clinical_CustomFormsPreview.params.CustomFormId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["CustomFormHTML"] = customFormHTML;
        objData["CustomFormBase64"] = Base64String.split(',')[1];
        if (IsSigned) {
            objData["IsSigned"] = IsSigned;
        }
        else {
            objData["IsSigned"] = "0";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    UpdateCustomFormsDetails: function (customFormHTML, Base64String, IsSigned) {
        var objData = {};
        objData["commandType"] = "UPDATE_PATIENT_CUSTOM_FORM";
        objData["CustomFormId"] = $('#ddlCustomForm option:selected').val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["CustomFormHTML"] = customFormHTML;
        objData["CustomFormBase64"] = Base64String.split(',')[1];
        objData["PatientCustomFormId"] = Clinical_CustomFormsPreview.params.PatientCustomFormId;
        objData["IsActive"] = $('#pnlPatientCustomForm #gridSwitchActive').attr('isactive');
        if (IsSigned) {
            objData["IsSigned"] = IsSigned;
        }
        else {
            objData["IsSigned"] = "0";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    convertHtmlToBase64: function (callback) {
        var def = $.Deferred();


        var patientId = $('#PatientProfile #hfPatientId').val();

        Patient_CustomForm.getHeaderFooterInfo(patientId).done(function (response) {
            response = JSON.parse(response);
            var html = $('#pnlPatientCustomForm #containerCustomFormPreview').html();
            if (response.status == true) {
                var HeaderLogo = response.HeaderLogo;
                var FooterText = response.FooterText;

                if (HeaderLogo != null && HeaderLogo !== "" && FooterText != null && FooterText !== "") {

                    var patientData = "";
                    if (response.PatientText != 'undefined' && response.PatientText != null)
                        patientData = a = response.PatientText.split('<br/>');
                    var newPatientText = '';
                    for (var i = 0; i < patientData.length; i++) {
                        if ($.trim(patientData[i]) != '') {
                            newPatientText += '<li>' + patientData[i] + '</li>';
                        }
                    }
                    var providerData = "";
                    if (response.ProviderText != 'undefined' && response.ProviderText != null)
                        providerData = a = response.ProviderText.split('<br/>');
                    var newProviderText = '';
                    for (var i = 0; i < providerData.length; i++) {
                        if ($.trim(providerData[i]) != '') {
                            newProviderText += '<li align="right">' + providerData[i] + '</li>';
                        }
                    }

                    var formHeaderHtml = '<div id="printcall">' +
                     '<div id="PatientInfo" class="col-xs-12 p-none">' +
                     '<div class="col-sm-4 col-lg-2 pull-left">' +
                      '<img src="' + HeaderLogo + '" class="img-responsive" height="100px" width="240px"></div>' +
                          '<ul class="list-unstyled pull-right line-height-fix">' +
                          '<li id="PatientPractice" class="text-right">' + response.PracticeText + '</li></ul>' +
                           '<div class="clearfix"></div>' +
                             '<div class="splitter m-none mt-xs">' +
                             '<div class="spacer3"></div></div>' +
                               '<div class="spacer3"></div>' +
                          '<ul class="list-unstyled pull-left line-height-fix" >' +
                              newPatientText + '</ul>' +
                           '<ul class="list-unstyled pull-right line-height-fix">' +
                          //'<li id="PatientProvider" align="right">' +
                          newProviderText + '</ul>' +
                       '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                               '<div class="spacer3"></div>' +
                      '<section class="">' +
                      '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                      '<div class="spacer10"></div><h4 id="templateHeader"></h4>';

                    var docType = '<!doctype html>';
                    var docCnt = formHeaderHtml;

                    var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                 + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                 + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                 + '</script>'
                 + '</head>';

                    var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                  + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                + '<span id="ClinicalReportsFooter">' + FooterText + '</span>'
                 + '</div> </div>';

                    var html = docCnt;
                    var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
                    $("#CustomeformprintparentdivPatient").append(ProgressNoteSign);
                    $("#customformsign #txtTextField").each(function (i, e) {
                        var scrollHeight = e.scrollHeight;
                        if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                            $(e).height(scrollHeight + 50);
                        } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                            $(e).height(scrollHeight + 40);
                        } else {
                            $(e).height(scrollHeight + 20);
                        }
                        $(e).parents('.toolcontroldiv').addClass('heightReset');
                    });
                    $("#customformsign #txtFreeText").each(function (i, e) {
                        var scrollHeight = e.scrollHeight;
                        if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                            $(e).height(scrollHeight + 130);
                        } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                            $(e).height(scrollHeight + 100);
                        } else {
                            $(e).height(scrollHeight + 50);
                        }
                        $(e).parents('.toolcontroldiv').addClass('heightReset');
                    });
                    $('#customformsign span.required').remove();
                    $("#CustomeformprintparentdivPatient").removeClass('hidden');
                    //var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                    //var newWin = window.open("", "_blank", winAttr);
                    //writeDoc = newWin.document;
                    //writeDoc.open();
                    // writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                    ////writeDoc.close();

                    //////   Our changes End
                    //setTimeout(function () {
                    //    newWin.focus();
                    //    newWin.print();
                    //    newWin.close();
                    //}, 200);
                    $.each($("#customformsign").find("ul#customFormDetails li"), function (i, item) {
                        $(item).find("div:first").addClass("showAllContent");
                    });
                    kendo.drawing.drawDOM("#customformsign", {
                        landscape: false,
                        scale: 0.6,
                        paperSize: "A4",
                        // margin: "2cm 3cm ",
                        margin: {
                            left: "10mm",
                            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                            top: "3mm",
                            //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                            right: "10mm",
                            bottom: "15mm"
                        },
                        template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
                    }).then(function (group) {
                        
                        kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                            $.each($('#pnlPatientCustomForm #customFormDetails').find("li"), function (i, item) {
                                $(item).find("div:first").removeClass("showAllContent");
                                $(item).find("div:first").css("width", "");
                                $(item).find("div:first").css("overflow", "");
                                $(item).find("div:first").css("height", "");
                            });
                            Patient_CustomForm.pdf = dataURL;
                            var params = [];
                            params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                            params["PreviewPdf"] = true;
                            //utility.PDFViewer(params["PrintPDFDataURL"], true, 'PreviewReportPrint', true);
                            //document.getElementById("PreviewReportPrint").contentWindow.print();
                            def.resolve(dataURL);
                            $("#CustomeformprintparentdivPatient").addClass('hidden');
                            $("#CustomeformprintparentdivPatient").html('');

                        });

                    });
                    return def.promise();
                }
                else {
                    Patient_Demographic.FillDemographic(patientId).done(function (patientInfo) {

                        var PatientProfileInfo = JSON.parse(patientInfo.DemographicFill_JSON);
                        var demographic_detail = JSON.parse(patientInfo.DemographicFill_JSON);

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

                        //var form = "#pnlClinicalSuperBillTemplate #frmSuperBillTemplate";
                        //var templateName = $(form + ' #ddlSuperBillTemplate option:selected').text();
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

                        if (globalAppdata['DateFormat'])
                            date_format = globalAppdata['DateFormat'];
                        var date = new Date();
                        //date_format = date_format.replace("mm", date.getMonth() + 1);
                        //date_format = date_format.replace("yyyy", date.getFullYear());
                        var day = "";
                        if (date.getDate().length < 2) {
                            day = "0" + date.getDate();
                        }
                        else {
                            day = date.getDate();
                        }
                        //date_format = date_format.replace("dd", day);

                        //var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                        //      var providerData = JSON.parse(response.NoteHeaderProviderData);
                        //var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "");
                        //date_format = utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate);
                        //PatientProfileInfo.Provider = providerName;
                        setTimeout(function () {
                            var html = $('#pnlPatientCustomForm #containerCustomFormPreview').html();
                            var formHeaderHtml = '<div id="printcall">' +
                                        '<div id="PatientInfo">' +
                                        '<div class="col-sm-4 col-lg-2 pull-left">' +
                                        '<img src="content/images/SHS-nav-logo-small-100.png" class="img-responsive"></div>' +
                                             '<ul class="list-unstyled pull-right line-height-fix">' +
                                             '<li id="DOS" class="text-right">DOS: ' + date + '</li>' +
                                             '<li id="PatientProvider" class="text-right"">Provider: ' + PatientProfileInfo.Provider + '</li></ul>' +
                                              '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                               '<div class="spacer3"></div>' +
                                             '<ul class="list-unstyled pull-left line-height-fix">' +
                                             '<li id="PatientName" >' + PatientProfileInfo.FullName + '</li>' +
                                             '<li id="PatientAccount" ></li>' + PatientProfileInfo.AccountNo + '</li>' +
                                              '<li id="PatientAge" >' + patientInfo + '</li>' +
                                             '<li id="PatientAddress">' + patientAddress + '</li>' +
                                             PatientHomePhone +
                                             PatientCellPhone +
                                             PatientEmail +
                                             '</ul>' +

                                         '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                               '<div class="spacer3"></div>' +
                                         '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                                         '<div class="spacer10"></div><h4 id="templateHeader"></h4>';

                            var docType = '<!doctype html>';
                            var docCnt = formHeaderHtml;

                            //   var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                            //+ '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                            //+ '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                            //+ '</script>'
                            //+ '</head>';


                            var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                          + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                        + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                         + 'Generated by:'
                        + '<span id="ClinicalReportsFooter"> MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page 1 of 1</span>'
                         + '</div> </div>';
                            var html = docCnt; //+ '</body></html>';
                            var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
                            $("#CustomeformprintparentdivPatient").append(ProgressNoteSign);
                            $("#customformsign #txtTextField").each(function (i, e) {
                                var scrollHeight = e.scrollHeight;
                                if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                    $(e).height(scrollHeight + 50);
                                } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                    $(e).height(scrollHeight + 40);
                                } else {
                                    $(e).height(scrollHeight + 20);
                                }
                                $(e).parents('.toolcontroldiv').addClass('heightReset');
                            });
                            $("#customformsign #txtFreeText").each(function (i, e) {
                                var scrollHeight = e.scrollHeight;
                                if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                    $(e).height(scrollHeight + 130);
                                } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                    $(e).height(scrollHeight + 100);
                                } else {
                                    $(e).height(scrollHeight + 50);
                                }
                                $(e).parents('.toolcontroldiv').addClass('heightReset');
                            });
                            $('#customformsign span.required').remove();
                            $("#CustomeformprintparentdivPatient").removeClass('hidden');
                            $.each($("#CustomeformprintparentdivPatient #customformsign").find("ul#customFormDetails li"), function (i, item) {
                                $(item).find("div:first").addClass("showAllContent");
                            });
                            kendo.drawing.drawDOM("#CustomeformprintparentdivPatient #customformsign", {
                                landscape: false,
                                scale: 0.6,
                                paperSize: "A4",
                                // margin: "2cm 3cm ",
                                margin: {
                                    left: "10mm",
                                    //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    top: "3mm",
                                    //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    right: "10mm",
                                    bottom: "15mm"
                                },
                                template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
                            }).then(function (group) {

                                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                                    $.each($('#pnlPatientCustomForm #customFormDetails').find("li"), function (i, item) {
                                        $(item).find("div:first").removeClass("showAllContent");
                                        $(item).find("div:first").css("width", "");
                                        $(item).find("div:first").css("overflow", "");
                                        $(item).find("div:first").css("height", "");
                                    });
                                    Patient_CustomForm.pdf = dataURL;
                                    var params = [];
                                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                                    params["PreviewPdf"] = true;
                                    //utility.PDFViewer(params["PrintPDFDataURL"], true, 'PreviewReportPrint', true);
                                    //document.getElementById("PreviewReportPrint").contentWindow.print();
                                    def.resolve(dataURL);
                                    $("#CustomeformprintparentdivPatient").addClass('hidden');
                                    $("#CustomeformprintparentdivPatient").html('');

                                });

                            });
                            return def.promise();
                            //var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                            //var newWin = window.open("", "_blank", winAttr);
                            //writeDoc = newWin.document;
                            //writeDoc.open();
                            //writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                            ////writeDoc.close();

                            //////   Our changes End
                            //setTimeout(function () {
                            //    newWin.focus();
                            //    newWin.print();
                            //    newWin.close();
                            //}, 200);

                        }, 100);

                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return def.promise();
    },

}
