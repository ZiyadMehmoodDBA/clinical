

Clinical_CaseReportsHTML = {
    bIsFirstLoad: true,
    params: [],
    arrComponentToLoad: [],
    Load: function (params) {

        Clinical_CaseReportsHTML.params = params;

        if (Clinical_CaseReportsHTML.params.PanelID != 'Clinical_CaseReportsHTML') {
            Clinical_CaseReportsHTML.params.PanelID = Clinical_CaseReportsHTML.params.PanelID + ' #Clinical_CaseReportsHTML';
        } else {
            Clinical_CaseReportsHTML.params.PanelID = 'Clinical_CaseReportsHTML';
        }
        if (Clinical_CaseReportsHTML.bIsFirstLoad) {
            //Clinical_CaseReportsHTML.getHtmlFromXMLBase64();
            Clinical_CaseReportsHTML.BindCaseReportHTML();
            if (Clinical_CaseReportsHTML.params.ParentCtrl == "Clinical_ReferralSummary") {
                $("#Clinical_CaseReportsHTML .modal-title").text("2016 Clinical Referral: Consolidated CDA");
            }
        }
    },

    BindCaseReportHTML: function (panel) {
        var PatientData = Clinical_CaseReportsHTML.params.PatientData[0];
        var AuthoringData = Clinical_CaseReportsHTML.params.AuthoringData[0];
        var EncounterData = Clinical_CaseReportsHTML.params.EncounterData[0];
        var NotesData = Clinical_CaseReportsHTML.params.NotesData[0];
        var HistoryOfCurrentIllnessData = Clinical_CaseReportsHTML.params.HistoryOfCurrentIllnessData;
        var ReasonForVisitData = Clinical_CaseReportsHTML.params.ReasonForVisitData;
        var ComponentData = Clinical_CaseReportsHTML.params.ComponentData;
        var GenerateDateTime = Clinical_CaseReportsHTML.params.DateTime;
        var PracticeData = JSON.parse(Clinical_CaseReportsHTML.params.PracticeData)[0];

        if (panel == null || panel == "") {
            panel = "Clinical_CaseReportsHTML";
        }
        $("#" + panel + " #frmCaseReportsHTML").attr('src', "./EMR/HTML/Clinical/ClinicalSummary/Clinical_CaseReport_Template.html");
        var iframe = $('#' + panel + ' #frmCaseReportsHTML')[0];
        iframe.addEventListener('load', function () {
            var iframeBody = $($('#' + panel + ' #frmCaseReportsHTML')[0].contentWindow.document).find("body");
            iframeBody.find("#TxtPatientName").text(PatientData["PatientName"]);
            iframeBody.find("#DivPatientName").text(PatientData["PatientName"]);
            iframeBody.find("#PatientIDs").text(PatientData["PatientIDs"]);
            iframeBody.find("#PatientSSN").text(PatientData["PatientSSN"]);

            // Patient Data Mapping
            if (Clinical_CaseReportsHTML.params.PatientData.length > 0) {
                $.each(iframeBody.find('#DivCaseReportTemplate #DivPatientData').find('div[name],span[name]'), function (i, item) {
                    if (item.hasAttribute('name') && (PatientData[item.attributes['name'].value]) != null) {
                        $(item).text((PatientData[item.attributes['name'].value]).replace("<br>", ""));
                    }
                });
            }

            if (Clinical_CaseReportsHTML.params.NotesData.length > 0) {
                $.each(iframeBody.find('#DivCaseReportTemplate #DivEncounterDetails').find('div[name],span[name]'), function (i, item) {
                    if (item.hasAttribute('name') && (NotesData[item.attributes['name'].value]) != null) {
                        if (item.attributes['name'].value == "ResponsiblePartyContactInfo") {

                        } else {
                            $(item).text((NotesData[item.attributes['name'].value]).replace("<br>", ""));
                        }
                    }
                });

                $.each(iframeBody.find('#DivCaseReportTemplate #DivDocumentMaintainedBy .DocumentMaintainedBy').find('div[name],span[name]'), function (i, item) {
                    if (item.hasAttribute('name') && (NotesData[item.attributes['name'].value]) != null) {
                        $(item).text((NotesData[item.attributes['name'].value]).replace("<br>", ""));
                    }
                });
            }

            if (Clinical_CaseReportsHTML.params.PracticeData.length > 0) {
                var d = new Date($('#userCurrentTime').text());

                if (d == "Invalid Date")
                    d = new Date(Date($('#userCurrentTime').text()))
                var t = d.toLocaleTimeString();
                $.each(iframeBody.find('#DivCaseReportTemplate #AuthorDetail').find('div[name],span[name]'), function (i, item) {
                    if (item.hasAttribute('name') && ((PracticeData[item.attributes['name'].value]) != null || (item.attributes['name'].value == "AuthorTime"))) {
                        if (item.attributes['name'].value == "AuthorTime") {
                            $(item).text(GenerateDateTime + " MD Vision");
                        } else {
                            $(item).text((PracticeData[item.attributes['name'].value]).replace("<br>", ""));
                        }
                    }
                });

                $.each(iframeBody.find('#DivCaseReportTemplate #DivDocumentMaintainedBy .PracticeAddress').find('div[name],span[name]'), function (i, item) {
                    if (item.hasAttribute('name') && (PracticeData[item.attributes['name'].value]) != null) {
                        $(item).text((PracticeData[item.attributes['name'].value]).replace("<br>", ""));
                    }
                });
            }

            if (Clinical_CaseReportsHTML.params.ReasonForVisitData.length > 0) {
                iframeBody.find('#DivCaseReportTemplate #DivReasonForVisit').find('span[name=ReasonforVisit]').text("");
                $.each(ReasonForVisitData, function (i, item) {
                    iframeBody.find('#DivCaseReportTemplate #DivReasonForVisit').find('span[name=ReasonforVisit]').last().append("<p>" + ReasonForVisitData[i] + "</p>");
                });
            }

            // Author Data Mapping
            if (Clinical_CaseReportsHTML.params.AuthoringData.length > 0) {
                $.each(iframeBody.find('#DivCaseReportTemplate #DivEncounterDetails .ProvAddress').find('div[name],span[name]'), function (i, item) {
                    if (item.hasAttribute('name') && (AuthoringData[item.attributes['name'].value]) != null) {
                        $(item).text((AuthoringData[item.attributes['name'].value]).replace("<br>", ""));
                    }
                });
            }

            for (i = 0; i < ComponentData.length ; i++) {
                if (ComponentData[i].ComponentName == "SOCIAL HISTORY") {
                    var SocialHistoryGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "PROBLEMS") {
                    var ProblemsGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "MEDICATIONS ADMINISTERED") {
                    var MedicationsGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "REASON FOR VISIT/CHIEF COMPLAINT") {
                    var ReasonForVisitGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "RESULTS") {
                    var ResultsGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "ENCOUNTER") {
                    var EncounterGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "Plan of Treatment") {
                    var PlanofTreatmentGrid = ComponentData[i].ComponentHTML;
                }
                if (ComponentData[i].ComponentName == "HISTORY OF PRESENT ILLNESS") {
                    var HPIElement = ComponentData[i].ComponentHTML;
                }

            }

            iframeBody.find('#DivCaseReportTemplate #tblSocialHistory').empty();
            iframeBody.find('#DivCaseReportTemplate #tblProblems').empty();
            iframeBody.find('#DivCaseReportTemplate #tblMedications').empty();
            iframeBody.find('#DivCaseReportTemplate #tblResults').empty();
            iframeBody.find('#DivCaseReportTemplate #tblEncounters').empty();
            iframeBody.find('#DivCaseReportTemplate #tblPlanofTreatment').empty();

            if (HPIElement != null) {
                iframeBody.find('#DivCaseReportTemplate #DivHistoryOfCurrentIllness').find('span[name=ComplaintDescription]').last().append((HPIElement).replace("<br>", ""));
            }

            if (SocialHistoryGrid != null) {
                var DivStart = "<div class='table-responsive'>";
                var DivEnd = "</div>";
                iframeBody.find('#DivCaseReportTemplate #tblSocialHistory').append(DivStart + SocialHistoryGrid + DivEnd);
                iframeBody.find('#DivCaseReportTemplate #tblSocialHistory').find("table").addClass("table table-striped table-hover");
                iframeBody.find('#DivCaseReportTemplate #tblSocialHistory').find("th,tr,td,thead,tbody").addClass("cda-render");
                iframeBody.find('#DivCaseReportTemplate #tblSocialHistory').find("table").removeAttr("border");
            }
            if (ProblemsGrid != null) {
                iframeBody.find('#DivCaseReportTemplate #tblProblems').append(DivStart + ProblemsGrid + DivEnd);
                iframeBody.find('#DivCaseReportTemplate #tblProblems').find("table").addClass("table table-striped table-hover");
                iframeBody.find('#DivCaseReportTemplate #tblProblems').find("th,tr,td,thead,tbody").addClass("cda-render");
                iframeBody.find('#DivCaseReportTemplate #tblProblems').find("table").removeAttr("border");
            }
            if (MedicationsGrid != null) {
                iframeBody.find('#DivCaseReportTemplate #tblMedications').append(DivStart + MedicationsGrid + DivEnd);
                iframeBody.find('#DivCaseReportTemplate #tblMedications').find("table").addClass("table table-striped table-hover");
                iframeBody.find('#DivCaseReportTemplate #tblMedications').find("th,tr,td,thead,tbody").addClass("cda-render");
                iframeBody.find('#DivCaseReportTemplate #tblMedications').find("table").removeAttr("border");
            }
            if (ResultsGrid != null) {
                iframeBody.find('#DivCaseReportTemplate #tblResults').append(DivStart + ResultsGrid + DivEnd);
                iframeBody.find('#DivCaseReportTemplate #tblResults').find("table").addClass("table table-striped table-hover");
                iframeBody.find('#DivCaseReportTemplate #tblResults').find("th,tr,td,thead,tbody").addClass("cda-render");
                iframeBody.find('#DivCaseReportTemplate #tblResults').find("table").removeAttr("border");
            }
            if (EncounterGrid != null) {
                iframeBody.find('#DivCaseReportTemplate #tblEncounters').append(DivStart + EncounterGrid + DivEnd);
                iframeBody.find('#DivCaseReportTemplate #tblEncounters').find("table").addClass("table table-striped table-hover");
                iframeBody.find('#DivCaseReportTemplate #tblEncounters').find("th,tr,td,thead,tbody").addClass("cda-render");
                iframeBody.find('#DivCaseReportTemplate #tblEncounters').find("table").removeAttr("border");
            }
            if (PlanofTreatmentGrid != null) {
                iframeBody.find('#DivCaseReportTemplate #tblPlanofTreatment').append(DivStart + PlanofTreatmentGrid + DivEnd);
                iframeBody.find('#DivCaseReportTemplate #tblPlanofTreatment').find("table").addClass("table table-striped table-hover");
                iframeBody.find('#DivCaseReportTemplate #tblPlanofTreatment').find("th,tr,td,thead,tbody").addClass("cda-render");
                iframeBody.find('#DivCaseReportTemplate #tblPlanofTreatment').find("table").removeAttr("border");
            }
            iframeBody.find('#DivCaseReportTemplate #DivDocumentInfo #dgvDocumentInfo tbody').empty();
            iframeBody.find('#DivCaseReportTemplate #DivDocumentInfo #dgvDocumentInfo tbody').append("<td>" + "OID: " + Clinical_CaseReportsHTML.GenerateGUID() + "</td><td>" + GenerateDateTime + "</td>")
            setTimeout(function () {
                if ($('#' + panel + ' #frmCaseReportsHTML')[0])
                    $("#" + panel + " #hbase64HTML").val(window.btoa(unescape(window.encodeURIComponent($($('#' + panel + ' #frmCaseReportsHTML')[0].contentWindow.document)[0].scrollingElement.outerHTML))));

            }, 100);
        });
    },

    GenerateGUID: function () {
        function _p8(s) {
            var p = (Math.random().toString(16) + "000000000").substr(2, 8);
            return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
        }
        return _p8() + _p8(true) + _p8(true) + _p8();
    },
    BindDivValuesByName: function (DivName, JSON) {
        $.each(iframeBody.find(DivName).find('div[name],span[name]'), function (i, item) {
            if (item.hasAttribute('name')) {
                $(item).text(JSON[item.attributes['name'].value]);
            }
        });
        //console.log(PatientData[0]);

        //$.get("./EMR/HTML/Clinical/ClinicalSummary/Clinical_CaseReport_Template.html", function (TemplateHTML) {
        //    $("#Clinical_CaseReportsHTML #frmCaseReportsHTML").append(TemplateHTML);

        //    $('#DivCaseReportTemplate #DivPatientData').find('div').each(function () {
        //        if (this.hasAttribute('name')) {
        //           this.value
        //        } else {
        //            this.defaultValue = this.value;
        //        }
        //    });

        //$("#Clinical_CaseReportsHTML #frmCaseReportsHTML #DivCaseReportTemplate #DivPatientName").text(PatientData[0].PatientName);
        //$("#Clinical_CaseReportsHTML #frmCaseReportsHTML #DivCaseReportTemplate #TxtPatientName").text(PatientData[0].PatientName);

        //-------------------------------------------------------------------------------------------------------------------------------------------

        ////$(this).children("div:first").html(data);
        //utility.bindMyJSON(true, PatientData, true, TemplateHTML).done(function () {
        //    $("#Clinical_CaseReportsHTML #frmCaseReportsHTML").append(TemplateHTML);
        //});
        ////console.log($("#DivCaseReportTemplate #DivPatientName").text());


    },
    //Author:Farooq Ahmad
    //Date: 25/04/2016
    //Overview: This function is created to html from encrpted xml base64
    getHtmlFromXMLBase64: function () {
        var objData = new Object();
        objData["commandType"] = "HTML";
        if (Clinical_CaseReportsHTML.params.ParentCtrl == "Clinical_ReferralSummary")
            objData["Template"] = "ReferralSummary";
        else
            objData["Template"] = "CaseReports";
        objData["DataIsEncrypted"] = Clinical_CaseReportsHTML.params.DataIsEncrypted;
        objData["XMLData"] = Clinical_CaseReportsHTML.params.XMLData;
        objData["Password"] = Clinical_CaseReportsHTML.params.Password;
        objData["NoteId"] = Clinical_CaseReportsHTML.params.NoteId;
        objData["PatientId"] = Clinical_CaseReportsHTML.params.PatientId;
        Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                var data = JSON.stringify(objData);
                MDVisionService.APIService(data, "CaseReports", "CaseReports").done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var self = $("#frmCaseReportsHTML");
                        try {
                            self.html(response.data.replace("2016 Clinical Summary: Consolidated CDA", "2016 Clinical Referral: Consolidated CDA"));
                            $("#Clinical_CaseReportsHTML #hbase64HTML").val(window.btoa(response.data));
                        }
                        catch (ex) {
                            console.log(ex);
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },


    //Author: Muhammad Arshad
    //Date: 06/04/2016
    //Overview: This function is created to Load Clinical Summary HTML
    loadCaseReportsHTMLData: function () {
        Clinical_CaseReportsHTML.CaseReportsHTMLDataLoad().done(function (response) {
            if (response.status != false) {
                var responseDetail = JSON.parse(response);
                if (responseDetail != null && responseDetail != "" && responseDetail.status != false) {
                    var self = $("#frmCaseReportsHTML");
                    try {
                        self.html(responseDetail.data);
                        $("#Clinical_CaseReportsHTML #hbase64HTML").val(window.btoa(responseDetail.data));
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                    $(self).find(".header_table td.td_header_role_value label").each(function () {
                        $(this).html($(this).text());
                    });
                }
                var mystr = "";
                var mystr2 = "";

            }
            else {
                utility.DisplayMessages(response.Message, 3);
                return dfd.promise();
            }
        });
    },

    DownloadCCDAHTML: function (panel) {
        if (panel == "" || panel == null) {
            panel = "Clinical_CaseReportsHTML";
        }
        var base64Content = $("#" + panel + " #hbase64HTML").val();
        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }
        guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
        download("data:text/HTML;base64," + base64Content, guid + ".html", "text/HTML");
    },


    //Author: Farooq Ahmad
    //Date: 04/04/2016
    //Overview: This function is created to LoadNotes
    loadPatientData: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Demographic.FillDemographic(Clinical_CaseReportsHTML.params.PatientId).done(function (response) {
                    if (response.status != false) {
                        var demographic_detail = JSON.parse(response.DemographicFill_JSON);
                        console.log(demographic_detail);


                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        return dfd.promise();
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
                return dfd.promise();
            }
        });
    },



    //Author: Farooq Ahmad
    //Date: 31/03/2016
    //Overview: This function is created to LoadNotes
    NotesLoad: function () {
        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        if (Clinical_CaseReportsHTML.params.PatientId != null) {
            objData["PatientId"] = Clinical_CaseReportsHTML.params.PatientId;
        }
        objData["commandType"] = "SEARCH_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    /*
        Author: Muhammad Arshad
        Date: 06/04/2016
        Overview: This function is service call
        */
    CaseReportsHTMLDataLoad: function (params) {
        TryParseInt = function (str, defaultValue) {
            var retValue = defaultValue;
            if (str !== null) {
                if (str.length > 0) {
                    if (!isNaN(str)) {
                        retValue = parseInt(str);
                    }
                }
            }
            return retValue;
        }
        var objData = new Object();
        //objData["Components"] = arrComponentToLoad;
        objData["commandType"] = "html";
        if (params != null) {
            objData["NoteId"] = params.NoteId;
            objData["ProviderId"] = params.ProviderId;
            objData["PatientId"] = params.PatientId;
            if (params.commandType != null)
                objData["commandType"] = params.commandType;
        }
        else {
            Components = [];
            var compId = -1;
            $(Clinical_CaseReportsHTML.params.chkCheckedDataElements).each(function (index, item) {
                if ($(item).attr('value') != null) {
                    var CompIds = $(item).attr('value').split(",");
                    for (var comId in CompIds) {
                        componentId = TryParseInt(CompIds[comId], 0);
                        if (componentId == 0) {
                            componentId = compId--;
                        }
                        var obj = {
                            componentId: componentId,
                            componentName: $(item).attr('class'),
                        };
                        Components.push(obj);
                    }
                    componentId = TryParseInt($(item).attr('value'), 0)
                }
                else {
                    componentId = compId--;
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);
                }



            });


            objData["Components"] = Components;
            objData["NoteId"] = Clinical_CaseReportsHTML.params.NoteId;
            objData["ProviderId"] = Clinical_CaseReportsHTML.params.ProviderId;
            objData["PatientId"] = Clinical_CaseReportsHTML.params.PatientId;

        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CaseReports", "CaseReports");
    },

    /*
     Author: Muhammad Arshad
     Date: 04/04/2016
     Overview: This function is load Number part
     */
    getNumberPart: function (obj) {
        var innernumericPart = 0;
        if (obj != null && obj != "") {
            innernumericPart = obj.replace(/[^\d]+/, '');
        }
        return innernumericPart;
    },

    containsObject: function (obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i].componentName === obj) {
                return true;
            }
        }

        return false;
    },

    /*
    Author: Muhammad Arshad
    Date: 31/03/2016
    Overview: This function is load Note Data
    */
    loadNotesData: function (NotesId) {

        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                Clinical_CaseReportsHTML.params.ProviderId = Clinical_Notes_detail.ProviderId;
                Clinical_CaseReportsHTML.params.PatientId = Clinical_Notes_detail.PatientId;
                Clinical_CaseReportsHTML.loadCaseReportsHTMLData();
                var myNoteText = $(Clinical_Notes_detail.NoteText);
                var chkCheckedDataElements = Clinical_CaseReportsHTML.params["chkCheckedDataElements"];

                myNoteText.find("li.initialVisitBody").each(function (i, item) {
                    var onClickFunctionName = $(item).find("header a:first").attr("onclick");
                    onClickFunctionName = onClickFunctionName.substring(onClickFunctionName.indexOf("'") + 1);
                    onClickFunctionName = onClickFunctionName.substring(0, onClickFunctionName.indexOf("'"));
                    var componentId = Clinical_CaseReportsHTML.getNumberPart($(item).find("section").attr("value"));

                    $(chkCheckedDataElements).each(function (i, item) {
                        var itemcomponentText = $(item).attr("class");
                        componentId = $(item).attr("value");
                        if (itemcomponentText != null && itemcomponentText != '') {
                            if (itemcomponentText.toLowerCase() == onClickFunctionName.toLowerCase() &&
                                !Clinical_CaseReportsHTML.containsObject(itemcomponentText, Clinical_CaseReportsHTML.arrComponentToLoad)) {

                                var obj = {
                                    componentId: componentId,
                                    componentName: itemcomponentText,
                                }
                                Clinical_CaseReportsHTML.arrComponentToLoad.push(obj);
                            }

                        }
                    });

                    $("#" + Clinical_CaseReportsHTML.params.PanelID + " #ulDataElements ." + onClickFunctionName).attr("checked", "checked");


                });


                //if (Clinical_CaseReportsHTML.arrComponentToLoad.length > 0) {
                //    var objData = new Object();
                //    objData["Components"] = Clinical_CaseReportsHTML.arrComponentToLoad;
                //    objData["commandType"] = "LOAD_CLINICAL_SUMMARY";
                //    objData["NoteId"] = Clinical_CaseReportsHTML.params.NoteId;
                //    var data = JSON.stringify(objData);
                //    MDVisionService.APIService(data, "CaseReports", "CaseReports").done(function (response) {
                //        response = JSON.parse(response);
                //        if (response.status != false) {
                //            $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtHashValue").val(response.SHA1);
                //        }
                //        else {
                //            utility.DisplayMessages(response.Message, 3);
                //        }
                //    });;
                //}


            }
            else {
                //  utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    /*
    Author: Muhammad Arshad
    Date: 31/03/2016
    Overview: This function is load Note Data
    */
    procedureOrderGridLoad: function (response) {
        $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.procedureOrderCount > 0) {
            var procedureLoadJSONData = JSON.parse(response.ProcedureLoad_JSON); //Parsing array to JSON
            $.each(procedureLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_ProcedureOrder.procedureOrderAddEdit('" + item.ProcedureOrderId + "',event);");
                $row.attr("id", "gvProcedure_row" + item.ProcedureOrderId);
                $row.attr("ProcedureId", item.ProcedureOrderId);
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
                //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                $row.append('<td style="display:none;">' + item.ProcedureOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_ProcedureOrder.procedureOrderDelete(\'' + item.ProcedureOrderId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_ProcedureOrder.procedureOrderAddEdit(\'' + item.ProcedureOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ProcedureOrder.printProcedureOrder(\'' + item.ProcedureOrderId + '\',\'' + item.Status + '\' );" title="View Result"> <i class="fa fa-credit-card blue"></i></a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Procedures + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssigneeName + '</td>');
                //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + ' #pnlProcedureOrder_Result #dgvProcedureOrder').DataTable({
                "language": {
                    "emptyTable": "No Procedure Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_ProcedureOrder.params.PanelID + ' #pnlProcedureOrder_Result #dgvProcedureOrder'))
            ;
        else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown          
        }
    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(Clinical_CaseReportsHTML.params.ParentCtrl, 'Clinical_CaseReportsHTML');
        objDeffered.resolve();
        return objDeffered;
    },
}