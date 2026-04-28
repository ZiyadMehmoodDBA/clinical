/*
Author: Muhammad Arshad
Date: 31/03/2016
Overview: This file is created to show clinical summary
*/

Clinical_ClinicalSummaryHTML = {
    bIsFirstLoad: true,
    params: [],
    arrComponentToLoad: [],
    Load: function (params) {

        Clinical_ClinicalSummaryHTML.params = params;

        if (Clinical_ClinicalSummaryHTML.params.PanelID != 'Clinical_ClinicalSummaryHTML') {
            Clinical_ClinicalSummaryHTML.params.PanelID = Clinical_ClinicalSummaryHTML.params.PanelID + ' #Clinical_ClinicalSummaryHTML';
        } else {
            Clinical_ClinicalSummaryHTML.params.PanelID = 'Clinical_ClinicalSummaryHTML';
        }
        if (Clinical_ClinicalSummaryHTML.bIsFirstLoad) {
            Clinical_ClinicalSummaryHTML.getHtmlFromXMLBase64();
            if (Clinical_ClinicalSummaryHTML.params.ParentCtrl == "Clinical_ReferralSummary") {
                $("#Clinical_ClinicalSummaryHTML .modal-title").text("2016 Clinical Referral: Consolidated CDA");
            }
        }
    },

    //Author:Farooq Ahmad
    //Date: 25/04/2016
    //Overview: This function is created to html from encrpted xml base64
    getHtmlFromXMLBase64: function () {
        var objData = new Object();
        objData["commandType"] = "HTML";
        if (Clinical_ClinicalSummaryHTML.params.ParentCtrl == "Clinical_ReferralSummary")
            objData["Template"] = "ReferralSummary";
        else
            objData["Template"] = "ClinicalSummary";
        objData["DataIsEncrypted"] = Clinical_ClinicalSummaryHTML.params.DataIsEncrypted;
        objData["XMLData"] = Clinical_ClinicalSummaryHTML.params.XMLData;
        objData["Password"] = Clinical_ClinicalSummaryHTML.params.Password;
        objData["NoteId"] = Clinical_ClinicalSummaryHTML.params.NoteId;
        objData["PatientId"] = Clinical_ClinicalSummaryHTML.params.PatientId;
        Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                var data = JSON.stringify(objData);
                MDVisionService.APIService(data, "ClinicalSummary", "ClinicalSummary").done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var self = $("#frmClinicalSummaryHTML");
                        try {

                            self.html(response.data.replace("2016 Clinical Summary: Consolidated CDA", "2016 Clinical Referral: Consolidated CDA"));
                            $("#Clinical_ClinicalSummaryHTML #hbase64HTML").val(window.btoa(response.data));
                       // var UncheckedComponents=   $("#pnlClinicalFaceSheet #Clinical_ClinicalSummary  #divDataElements input[type='checkbox']:not(:checked)")
                            $("#pnlClinicalFaceSheet" + " #" + Clinical_ClinicalSummaryHTML.params.ParentCtrl + " #divDataElements input[type='checkbox']:not(:checked)").each(function (index, item) {

                            var name = item.name.replace('DataElement', '').toUpperCase().trim();

                            if (name == "LABRESULT" || name == "LABRESULTS")
                                name = "RESULTS"
                            else if (name == "MEDICALEQUIPMENT")
                            {
                                name = "IMPLANTS";
                            }
                            else if (name == "SOCIALHISTORY") {
                                name = "SOCIAL HISTORY";

                            }
                            else if (name == "ALLERGIESANDADVERSEREACTIONS") {
                                name = "ALLERGIES";

                            }
                            else if (name == "COGNITIVES" || name == "COGNITIVE") {
                                name = "FUNCTIONAL STATUS";

                            }
                            else if (name == "MENTALSTATUS") {
                                name = "MENTAL STATUS";

                            }
                            
                            else if (name == "REFFERRAL") {
                                name = "REASON FOR REFERRAL";

                            }
                            else if (name == "PAYERSSECTION") {
                                name = "INSURANCE PAYERS";

                            }
                            
                            else if (name == "VISITREASON") {
                                if (Clinical_ClinicalSummaryHTML.params.ParentCtrl == "Clinical_ReferralSummary")
                                    name = "REASON FOR REFERRAL";
                                else if (Clinical_ClinicalSummaryHTML.params.ParentCtrl == "Clinical_ClinicalSummary")
                                    name = "REASON FOR VISIT/CHIEF COMPLAINT";
                                //else if (Clinical_ClinicalSummaryHTML.params.ParentCtrl == "Clinical_ReferralNote")
                                //    name = "REASON FOR VISIT/CHIEF COMPLAINT";
                                
                            }
                            else if (name == "HEALTHCONCERNS") {
                                name = "HEALTH CONCERNS SECTION";
                            }
                            else if (name == "VITALSIGN" || name == "VITALSIGNS") {
                                name = "VITAL SIGNS";
                            }
                            else if (name == "PLANOFCARE") {
                                name = "GOALS SECTION";
                            }
                            else if (name == "PLANOFTREATMENT") {
                                name = "PLAN OF TREATMENT";
                            }
                   
                            self.find('li:contains("' + name + '")').remove();
                            self.find('h3:contains("' + name + '")').next("div").remove();
                            self.find('h3:contains("' + name + '")').remove();

                            

                            })
                            if (Clinical_ClinicalSummaryHTML.params.ParentCtrl == "Clinical_ReferralNote") {
                                self.find('li:contains("INSURANCE PAYERS")').remove();
                                self.find('h3:contains("INSURANCE PAYERS")').next("div").remove();
                                self.find('h3:contains("INSURANCE PAYERS")').remove();
                            }
                            
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
    loadClinicalSummaryHTMLData: function () {
        Clinical_ClinicalSummaryHTML.ClinicalSummaryHTMLDataLoad().done(function (response) {
            if (response.status != false) {
                var responseDetail = JSON.parse(response);
                if (responseDetail != null && responseDetail != "" && responseDetail.status != false) {
                    var self = $("#frmClinicalSummaryHTML");
                    try {
                        self.html(responseDetail.data);
                        $("#Clinical_ClinicalSummaryHTML #hbase64HTML").val(window.btoa(responseDetail.data));
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

    DownloadCCDAHTML: function () {
        var base64Content = $("#Clinical_ClinicalSummaryHTML #hbase64HTML").val();
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
                Patient_Demographic.FillDemographic(Clinical_ClinicalSummaryHTML.params.PatientId).done(function (response) {
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
        if (Clinical_ClinicalSummaryHTML.params.PatientId != null) {
            objData["PatientId"] = Clinical_ClinicalSummaryHTML.params.PatientId;
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
    ClinicalSummaryHTMLDataLoad: function (params) {
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
            $(Clinical_ClinicalSummaryHTML.params.chkCheckedDataElements).each(function (index, item) {
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
            objData["NoteId"] = Clinical_ClinicalSummaryHTML.params.NoteId;
            objData["ProviderId"] = Clinical_ClinicalSummaryHTML.params.ProviderId;
            objData["PatientId"] = Clinical_ClinicalSummaryHTML.params.PatientId;

        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary");
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
                Clinical_ClinicalSummaryHTML.params.ProviderId = Clinical_Notes_detail.ProviderId;
                Clinical_ClinicalSummaryHTML.params.PatientId = Clinical_Notes_detail.PatientId;
                Clinical_ClinicalSummaryHTML.loadClinicalSummaryHTMLData();
                var myNoteText = $(Clinical_Notes_detail.NoteText);
                var chkCheckedDataElements = Clinical_ClinicalSummaryHTML.params["chkCheckedDataElements"];

                myNoteText.find("li.initialVisitBody").each(function (i, item) {
                    var onClickFunctionName = $(item).find("header a:first").attr("onclick");
                    onClickFunctionName = onClickFunctionName.substring(onClickFunctionName.indexOf("'") + 1);
                    onClickFunctionName = onClickFunctionName.substring(0, onClickFunctionName.indexOf("'"));
                    var componentId = Clinical_ClinicalSummaryHTML.getNumberPart($(item).find("section").attr("value"));

                    $(chkCheckedDataElements).each(function (i, item) {
                        var itemcomponentText = $(item).attr("class");
                        componentId = $(item).attr("value");
                        if (itemcomponentText != null && itemcomponentText != '') {
                            if (itemcomponentText.toLowerCase() == onClickFunctionName.toLowerCase() &&
                                !Clinical_ClinicalSummaryHTML.containsObject(itemcomponentText, Clinical_ClinicalSummaryHTML.arrComponentToLoad)) {

                                var obj = {
                                    componentId: componentId,
                                    componentName: itemcomponentText,
                                }
                                Clinical_ClinicalSummaryHTML.arrComponentToLoad.push(obj);
                            }

                        }
                    });

                    $("#" + Clinical_ClinicalSummaryHTML.params.PanelID + " #ulDataElements ." + onClickFunctionName).attr("checked", "checked");


                });


                //if (Clinical_ClinicalSummaryHTML.arrComponentToLoad.length > 0) {
                //    var objData = new Object();
                //    objData["Components"] = Clinical_ClinicalSummaryHTML.arrComponentToLoad;
                //    objData["commandType"] = "LOAD_CLINICAL_SUMMARY";
                //    objData["NoteId"] = Clinical_ClinicalSummaryHTML.params.NoteId;
                //    var data = JSON.stringify(objData);
                //    MDVisionService.APIService(data, "ClinicalSummary", "ClinicalSummary").done(function (response) {
                //        response = JSON.parse(response);
                //        if (response.status != false) {
                //            $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtHashValue").val(response.SHA1);
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
        UnloadActionPan(Clinical_ClinicalSummaryHTML.params.ParentCtrl, 'Clinical_ClinicalSummaryHTML');
        objDeffered.resolve();
        return objDeffered;
    },
}