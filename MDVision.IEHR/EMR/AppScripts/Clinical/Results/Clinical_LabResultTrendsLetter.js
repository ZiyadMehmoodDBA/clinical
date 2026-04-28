Clinical_LabResultTrendsLetter = {
    bIsFirstLoad: true,
    params: [],
    imageSize: 0,
    LetterContent: "",
    Load: function (params) {
        Clinical_LabResultTrendsLetter.params = params;
        $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter #btnAssignLetter').addClass("disabled");
        if (Clinical_LabResultTrendsLetter.params.PanelID != 'pnlClinical_LabResultTrendsLetter') {
            Clinical_LabResultTrendsLetter.params.PanelID = Clinical_LabResultTrendsLetter.params.PanelID + ' #pnlClinical_LabResultTrendsLetter';
        } else {
            Clinical_LabResultTrendsLetter.params.PanelID = 'pnlClinical_LabResultTrendsLetter';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_LabResultTrendsLetter.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        $("#" + Clinical_LabResultTrendsLetter.params.PanelID + " #LetterName").html(Clinical_LabResultTrendsLetter.params.TemplateLetterText);
        Clinical_LabResultTrendsLetter.TemplateContent = "";
        var self = $('#' + Clinical_LabResultTrendsLetter.params.PanelID);


        self.loadDropDowns(true).done(function () {


            
                $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter #btnDeletePatientLetter').addClass("disabled");
                $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter #btnSendLetter').addClass("disabled");
                $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter #btnAssignLetter').addClass("disabled");

                //intialize TinyMCE instance on textarea control
                EMRUtility.InitTinymceControl(false).done(function () {

                    Clinical_LabResultTrendsLetter.get_LabTemps().done(function (response) {
                        response = JSON.parse(response);
                        $('#ddlTemplateId').append('<option> - Select - </option>');
                        $.each(response.letterDetails, function(i, item) {
                            $('#ddlTemplateId').append('<option id="' + item.TemplateId + '"> ' + item.TemplateName + '</option>');
                        }
                    );
                });
                Clinical_LabResultTrendsLetter.TemplateContent = tinyMCE.activeEditor.getContent();
                Clinical_LabResultTrendsLetter.LoadTemplateLetter();

                $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter').data('serialize', $('#' + Add_Letter_Template.params.PanelID + ' #frmClinical_LabResultTrendsLetter').serialize());





                });
        
            //Clinical_LabResultTrendsLetter.ValidateTemplateLetter();

        });
        
    },
    get_LabTemps: function() {
        var param = {};
       
        param["commandType"] = "get_LabTemps";
        var data = JSON.stringify(param);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
   LoadTemplateLetter: function () {

        Clinical_LabResultTrendsLetter.FillLetter().done(function (response) {

            response = JSON.parse(response);

            if (response.status != false) {
                if (response.PatientLetterContentCount != 0) {
                    var strToReplaceByte = '';
                    var strToReplace = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="184" class="TagInserted" style="min-width: 10px; border: none; width: 240px;" readonly="readonly" type="text" value="{{ Primary Care Provider eSignature }}" />') > -1) {
                        strToReplace = '<input id="184" class="TagInserted" style="min-width: 10px; border: none; width: 240px;" readonly="readonly" type="text" value="{{ Primary Care Provider eSignature }}" />';
                    }

                    if (response.PatientLetterContent.indexOf('<input id="191" class="TagInserted" style="min-width: 10px; border: none; width: 128px;" readonly="readonly" type="text" value="9737397163STATE FARM INSURANCE" />') > -1) {
                        var str = response.PatientLetterContent.substring(response.PatientLetterContent.indexOf('<input id="191"'));
                        str = str.substring(0, str.indexOf('/>') + 2);
                        response.PatientLetterContent = response.PatientLetterContent.replace(str, $(str).css("width", "300px")[0].outerHTML);

                    }


                    if (response.PatientLetterContent.indexOf('<input id="192" class="TagInserted" style="min-width: 10px; border: none; width: 142px;" readonly="readonly" type="text" value="respiratory treatment and nebulizer" />') > -1) {
                        var str = response.PatientLetterContent.substring(response.PatientLetterContent.indexOf('<input id="192"'));
                        str = str.substring(0, str.indexOf('/>') + 2);
                        response.PatientLetterContent = response.PatientLetterContent.replace(str, $(str).css("width", "230px")[0].outerHTML);
                    }

                    var strToReplaceCurentProvider = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="185" class="TagInserted" style="min-width: 10px; border: none; width: 205px;" readonly="readonly" type="text" value="{{ Current Provider eSignature }}" />') > -1) {
                        strToReplaceCurentProvider = '<input id="185" class="TagInserted" style="min-width: 10px; border: none; width: 205px;" readonly="readonly" type="text" value="{{ Current Provider eSignature }}" />';
                    }

                    var strToReplaceMiscellaneous = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="186" class="TagInserted" style="min-width: 10px; border: none; width: 184px;" readonly="readonly" type="text" value="{{ Miscellaneous eSignature }}" />') > -1) {
                        strToReplaceMiscellaneous = '<input id="186" class="TagInserted" style="min-width: 10px; border: none; width: 184px;" readonly="readonly" type="text" value="{{ Miscellaneous eSignature }}" />';
                    }

                    if (response.eSignatureBase64 != "") {

                        var isIEBrowser = EMRUtility.GetIEVersion() > 0 ? true : false;
                        var base64Type = "image/png;";
                        if (isIEBrowser) {
                            base64Type = "image/gif";
                        }

                        strToReplaceByte = "<img id=\"imgProvidereSignature\" src=\"data:" + base64Type + ";base64," + response.eSignatureBase64 + "\" />";
                    }

                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplace, "gi"), strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplaceCurentProvider, "gi"), strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplaceMiscellaneous, "gi"), strToReplaceByte);
                    var isReadonly = false;
                    if (Clinical_LabResultTrendsLetter.params.Status == "Signed") {
                        isReadonly = true
                    }
                    EMRUtility.InitTinymceControl(isReadonly).done(function () {
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Miscellaneous eSignature }}/g, strToReplaceByte);
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Primary Care Provider eSignature }}/g, strToReplaceByte);
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Current Provider eSignature }}/g, strToReplaceByte);

                        tinyMCE.activeEditor.setContent(response.PatientLetterContent, { format: 'raw' });
                        Clinical_LabResultTrendsLetter.TemplateContent = tinyMCE.activeEditor.getContent();




                        // tinyMCE.activeEditor.insertContent(strToReplaceByte);//'<img alt="Smiley face" height="42" width="42" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEUAAAD///+l2Z/dAAAAM0lEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6P9/AFGGFyjOXZtQAAAAAElFTkSuQmCC"/>');
                        $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter').data('serialize', $('#' + Clinical_LabResultTrendsLetter.params.PanelID + ' #frmClinical_LabResultTrendsLetter').serialize());
                    });


                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    
    FillLetter: function () {
        var objData = {};
        if (Clinical_LabResultTrendsLetter.params.mode == "Edit") {
            objData["Patient_Letter_Id"] = Clinical_LabResultTrendsLetter.params.Patient_Letter_Id;
        }
        objData["TemplateLetterId"] = $('#ddlTemplateId option:selected').attr('id');
        objData["PatientId"] = Clinical_LabResultTrendsLetter.params.PatientId;
        objData["LabOrderResultId"] = ClinicalLabResultTrends.params.LabResultId;
        objData["LOINC"] = Clinical_LabResultTrendsLetter.params.LOINC;
        objData["Mode"] = "Add";
        objData["commandType"] = "Get_Content_Of_LETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
    deletePatientLetter: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Letter", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (typeof Clinical_LabResultTrendsLetter.params.Patient_Letter_Id != 'undefined' && Clinical_LabResultTrendsLetter.params.Patient_Letter_Id != null && Clinical_LabResultTrendsLetter.params.Patient_Letter_Id != '') {
                    Patient_Letter.PatientLetterDelete(Clinical_LabResultTrendsLetter.params.Patient_Letter_Id);
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    printPatientLetter: function () {
        var Base64 = "";
        $('#pnlClinical_LabResultTrendsLetter #PanelHead').replaceWith(Clinical_LabResultTrendsLetter.params.header);
        var contents = $(tinyMCE.activeEditor.getBody()).html();
        $('#pnlClinical_LabResultTrendsLetter #PanelHead').append("<hr>");
        $('#pnlClinical_LabResultTrendsLetter #printLetter #PanelBody').append(contents);
        if (Clinical_LabResultTrendsLetter.params.Type == "Trend") {
            $('#pnlClinical_LabResultTrendsLetter #printLetter #PanelBody').append($('#pnlClinicalLabResultTrends #TrendsPrint').clone());
        }
        else if (Clinical_LabResultTrendsLetter.params.Type == "Graph") {
            Base64 = Clinical_LabResultTrendsLetter.params.Base64;
        }
        else if (Clinical_LabResultTrendsLetter.params.Type == "Result") {
            Base64 = Clinical_LabResultTrendsLetter.params.Base64;
        }
        Clinical_LabResultTrendsLetter.getPrintnotePDF(Base64);
    },
    
    UnLoad: function () {
        if (Clinical_LabResultTrendsLetter.params != null && Clinical_LabResultTrendsLetter.params.ParentCtrl != null) {
            if (Clinical_LabResultTrendsLetter.params.ParentCtrl == 'clinicalTabLabOrder') {
                UnloadActionPan(Clinical_LabResultTrendsLetter.params["ParentCtrl"], "Clinical_LabResultTrendsLetter");
            } else {
                Clinical_LabResultTrendsLetter.params.PanelID = Clinical_LabResultTrendsLetter.params.PanelID.replace(" #Clinical_LabResultTrendsLetter", "");
                UnloadActionPan(Clinical_LabResultTrendsLetter.params.ParentCtrl);
            }
        }
        else
            UnloadActionPan(null, 'Clinical_LabResultTrendsLetter');

    },
    //Start//1/03/2016//M Ahmad Imran//Implimented SaveLetter function which Save Template letter.
    SavePatientLetter: function (status) {
        if (typeof status != 'undefined' && status != '' && status != null && (status).toLowerCase() == 'signed') {
            Clinical_LabResultTrendsLetter.SavePatientLetterWithStatus(status);
        }
        else {
            if (Clinical_LabResultTrendsLetter.TemplateContent != tinyMCE.activeEditor.getContent() || Clinical_LabResultTrendsLetter.TemplateContent != '') {
                Clinical_LabResultTrendsLetter.SavePatientLetterWithStatus(status);
            }
            else {
                utility.DisplayMessages("Please make any changes to save/update Patient Letter ", 3);
            }
        }
    },
    SavePatientLetterWithStatus: function (status) {
        if (Clinical_LabResultTrendsLetter.params.mode == "Add") {
            Clinical_LabResultTrendsLetter.convertHtmlToBase64().done(function (Base64String) {

                Clinical_LabResultTrendsLetter.PatientLetterSave(status, Base64String).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_LabResultTrendsLetter.UnLoadTab("Save").done(function () {
                            SelectLetter_Template.UnLoadTab();
                            Patient_Letter.patientTemplateLetterSearch();
                            if ((status).toLowerCase() == 'signed') {
                                utility.DisplayMessages("Successfully Signed", 1);
                            } else {
                                utility.DisplayMessages(response.Message, 1);
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            });
        }
        else if (Clinical_LabResultTrendsLetter.params.mode == "Edit") {
            Clinical_LabResultTrendsLetter.convertHtmlToBase64().done(function (Base64String) {
                Clinical_LabResultTrendsLetter.PatientLetterUpdate(status, Base64String).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_LabResultTrendsLetter.UnLoadTab("Save").done(function () {
                            UnloadActionPan("Patient_Letter", 'SelectLetter_Template');
                            Patient_Letter.patientTemplateLetterSearch();
                            if ((status).toLowerCase() == 'signed') {
                                utility.DisplayMessages("Successfully Signed", 1);
                            } else {
                                utility.DisplayMessages(response.Message, 1);
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            });
        }

    },
    //Start//2/3/2016//M Ahmad Imran//Implimented Call to Controller for Save Patient letter Detail
    PatientLetterSave: function (Status, Base64string) {


        var objData = {};
        var Sign = "";
        if (Status == "Signed") {
            Sign = Clinical_LabResultTrendsLetter.SignPatientLetter();
        }


        objData["Status"] = Status;
        objData["PatientLetterContent"] = tinyMCE.activeEditor.getContent() + Sign;
        objData["commandType"] = "SAVE_PATIENT_LETTER";
        objData["PatientId"] = Clinical_LabResultTrendsLetter.params.PatientId;
        objData["TemplateLetterId"] = Clinical_LabResultTrendsLetter.params.TemplateLetterId;
        objData["Base64String"] = Base64string;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");

    },
    //Start//2/3/2016//M Ahmad Imran//Implimented Call to Controller for Update Patient letter Detail
    PatientLetterUpdate: function (Status, Base64string) {
        var objData = {};
        var Sign = "";
        if (Status == "Signed") {
            Sign = Clinical_LabResultTrendsLetter.SignPatientLetter();
        }
        objData["Status"] = Status;
        objData["PatientLetterContent"] = tinyMCE.activeEditor.getContent() + Sign;
        objData["commandType"] = "UPDATE_PATIENT_LETTER";
        objData["PatientId"] = Clinical_LabResultTrendsLetter.params.PatientId;
        objData["Patient_Letter_Id"] = Clinical_LabResultTrendsLetter.params.Patient_Letter_Id;
        objData["Base64String"] = Base64string;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");

    },

    getPrintnotePDF: function (Base64) {
        var divId = "";
        var params = [];
        //  $('#pnlClinical_LabResultTrendsLetter #printLetter').hide();
        params["ParentCtrl"] = "Clinical_LabResultTrendsLetter";
        LoadActionPan('Clinical_LabResultTrendsView', params);
        $('#pnlClinical_LabResultTrendsLetter #printLetter').show();

        setTimeout(function () {
            // --------------------------------------------- start Download functionality--------------------------------------------------------------
            kendo.drawing.drawDOM('#pnlClinical_LabResultTrendsLetter #printLetter', {
                landscape: false,
                scale: 0.7,
                paperSize: "Legal",
                // margin: "2cm 3cm ",
                margin: {
                    left: "10mm",
                    top: "10mm",
                    right: "10mm",
                    bottom: "30mm"
                }

            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    $('#pnlClinical_LabResultTrendsLetter #printLetter').hide();
                    $('#pnlClinical_LabResultTrendsLetter #PanelHead').find("hr").remove();
                    var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                    if (Clinical_LabResultTrendsLetter.params.Type == "Result" || Clinical_LabResultTrendsLetter.params.Type == "Graph") {
                        Clinical_LabResultTrendsLetter.mergeBase64(PrintPDFDataURL, Base64);
                    }
                    else {
                        Clinical_LabResultTrendsView.faxPDF = PrintPDFDataURL;
                        Clinical_LabResultTrendsView.printScreen(dataURL);
                    }
                    $('#pnlClinical_LabResultTrendsLetter #printLetter #PanelBody').html("");
                });
            });

        }, 150);

        // ------------------------------------- End Download functionality--------------------------------------
    },

    mergeBase64: function (dataURL, Base64) {
        Clinical_LabResultTrendsLetter.mergeBase64_DBcall(dataURL, Base64).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_LabResultTrendsView.faxPDF = response.MergedContent;
                utility.PDFViewer(response.MergedContent, false, 'Clinical_LabResultTrendsView #PreviewLabResultForm', true);
            }
            else
                utility.DisplayMessages(response.message, 2);
        });
    },
    mergeBase64_DBcall: function (dataURL, Base64) {
        var objData = {};
        objData["LabLetterBase64"] = dataURL;
        objData["LabResultBase64"] = Base64;
        objData["commandType"] = "Merge_Base64_Content";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },
    PDFViewer: function (base64, IsnewTab, ObjectControlID, IsIframe, IsPrint) {
        if (utility.UserBrowser() == "IE") {
            utility.SaveOrOpenBlob(base64, IsnewTab, ObjectControlID, IsIframe, IsPrint);
        } else {
            utility.PDFViewer(base64, IsnewTab, ObjectControlID, IsIframe, IsPrint);
        }
    },



    calculateHeaderImageSize: function (srcWidth, srcHeight, maxWidth, maxHeight) {

        var ratio = Math.min(maxWidth / srcWidth, maxHeight / srcHeight);
        return { width: srcWidth * ratio, height: srcHeight * ratio };
    },
    
    SignPatientLetter: function () {
        var LetterContent = tinyMCE.activeEditor.getContent();
        var fullDate = new Date();
        var hours = fullDate.getHours();
        var hours = (hours + 24 - 2) % 24;
        var mid = 'AM';
        if (hours == 0) { //At 00 hours we need to show 12 am
            hours = 12;
        }
        else if (hours > 12) {
            hours = hours % 12;
            mid = 'PM';
        }

        var daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        var d = new Date();
        //    document.write("The current month is " + monthNames[d.getMonth()]);

        var CurrentDate = daysOfWeek[fullDate.getDay()] + ", " + monthNames[d.getMonth()] + " " + fullDate.getDate() + ", " + fullDate.getFullYear() + " at " + fullDate.getHours() + ":" + fullDate.getMinutes() + " " + mid;
        var SignedText = "electronically signed by: " + globalAppdata['AppUserName'] + " on " + CurrentDate;
        var SignedLength = globalAppdata['AppUserName'].length + 33; a
        var InsertFieldInput = '<br><input type="text" readonly value="' + SignedText + '" style="min-width: 10px; margin: 0 5px; margin-right:5px; border: none;padding: 0 0px;width:' + (SignedText.length + 4) * 7 + 'px;"/>';
        return InsertFieldInput;
    },
    SendPatientLetter: function () {
        var params = [];
        params["patientId"] = Clinical_LabResultTrendsLetter.params.PatientId;
        params["mode"] = "Add";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Clinical_LabResultTrendsLetter';
        params["FromLetter"] = "1";
        params["Patient_Letter_Id"] = Clinical_LabResultTrendsLetter.params.Patient_Letter_Id;
        LoadActionPan('Document_AssignedTo', params);

    },
    convertHtmlToBase64: function (callback) {
        var def = $.Deferred();
        $('#sendHtml').html(tinyMCE.activeEditor.getContent());


        kendo.drawing.drawDOM("#sendHtml", {
            landscape: false,
            scale: 0.6,
            paperSize: "Legal",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                top: "10" + "mm",
                right: "10mm",
                bottom: "10mm"
            },

        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                $('#sendHtml').hide();

                // Clinical_LabResultTrendsLetter.Base64string = dataURL;

                $('#pnlClinical_LabResultTrendsLetter #sendHtml').html('');
                $('#pnlClinical_LabResultTrendsLetter #sendHtml').show();
                def.resolve(dataURL);

            });
        });
        return def.promise();
    },
   

}