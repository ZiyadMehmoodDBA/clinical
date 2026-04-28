//Author: Ahmad Raza
//File:   PQRS_ICDCPTCodes
//Date:   17-08-2016

PQRS_ICDCPTCodes = {


    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    arrICDs: [],
    arrCPTs: [],
    arrAllCodes: [],

    Load: function (params) {
        BackgroundLoaderShow(true);
        //PQRS_ICDCPTCodes.params["TabID"] = 'PQRS_ICDCPTCodes';
        PQRS_ICDCPTCodes.params = params;

        if (PQRS_ICDCPTCodes.params.PanelID != 'pnlICDCPTCodes') {
            PQRS_ICDCPTCodes.params.PanelID = PQRS_ICDCPTCodes.params.PanelID + ' #pnlICDCPTCodes';
        } else {
            PQRS_ICDCPTCodes.params.PanelID = 'pnlICDCPTCodes';
        }

        if ((PQRS_ICDCPTCodes.params.MeasureId != "0421a" && PQRS_ICDCPTCodes.params.MeasureId != "0419" && PQRS_ICDCPTCodes.params.MeasureId != "0043" && PQRS_ICDCPTCodes.params.MeasureId != "0068" && PQRS_ICDCPTCodes.params.MeasureId != "PHQ2" && PQRS_ICDCPTCodes.params.MeasureId != "PHQ9") && PQRS_ICDCPTCodes.params.CodeType != null && PQRS_ICDCPTCodes.params.CodeType != null) {
            var arrCodeType = PQRS_ICDCPTCodes.params.CodeType.split(",");
            var arrCodeOperators = PQRS_ICDCPTCodes.params.GroupOperator.split(",");
            var arrCodes = PQRS_ICDCPTCodes.params.CodeValues.split("|");
            var initialCounter = 1;
            $.each(arrCodeType, function (i, CodeType) {
                if (CodeType.trim() != "") {

                    var currentCodeOperator = "OR";
                    if (arrCodeOperators[i + 1] != null && arrCodeOperators[i + 1] != "") {
                        currentCodeOperator = arrCodeOperators[i + 1].trim();
                    }

                    var divName = CodeType;
                    if ($("#frmICDCPTCodes div[id*='" + CodeType + "']").length > 0) {
                        divName = CodeType + initialCounter;
                        initialCounter += 1;
                    }

                    var divCurrentCode = $("#frmICDCPTCodes #divCodesTemplate").clone();
                    divCurrentCode.attr("id", "div" + divName);
                    divCurrentCode.removeClass("hidden");
                    divCurrentCode.find("#divSearch").removeClass("hidden").attr("id", "divSearchdiv" + divName);
                    divCurrentCode.find("#headingTemplate").attr("id", "heading" + CodeType).text(CodeType + " Codes");
                    divCurrentCode.find("#txtTemplate").attr("id", "txt" + CodeType).attr("oninput", "PQRS_ICDCPTCodes.ShowCurrentCodeRadioBtn(this,'" + CodeType + "','" + "div" + divName + "');");
                    //PQRS_ICDCPTCodes.ShowCurrentCodeRadioBtn(obj, CodeType, ParentDivName, ui.item.value);
                    divCurrentCode.find("#btnTemplate").attr("id", "btn" + CodeType);
                    divCurrentCode.find("#lstTemplate").attr("id", "lst" + CodeType).empty();
                    divCurrentCode.find("#divAndOR").removeClass("hidden").attr("id", "divAndORdiv" + divName);
                    divCurrentCode.find("#divAndORdiv" + divName).addClass("disableAll");
                    divCurrentCode.find("#rdoAndTemplate").attr("id", "rdoAnddiv" + divName);
                    divCurrentCode.find("#rdoOrTemplate").attr("id", "rdoOrdiv" + divName);
                    divCurrentCode.find("#rdoAnddiv" + divName).attr('name', "div" + divName);
                    divCurrentCode.find("#rdoOrdiv" + divName).attr('name', "div" + divName);

                    if (currentCodeOperator == "AND") {
                        divCurrentCode.find("#rdoAnddiv" + divName).attr('checked', true);
                        divCurrentCode.find("#rdoOrdiv" + divName).attr('checked', false);
                    }
                    else {
                        divCurrentCode.find("#rdoAnddiv" + divName).attr('checked', false);
                        divCurrentCode.find("#rdoOrdiv" + divName).attr('checked', true);
                    }

                    $("#frmICDCPTCodes").append(divCurrentCode);
                    var divCurrentCode = $("#frmICDCPTCodes #div" + divName)
                    var arrCodeValues = arrCodes[i].split(",");
                    PQRS_ICDCPTCodes.arrAllCodes["div" + divName] = arrCodes[i].split(",");
                    var ExistingCount = 0;
                    if (divCurrentCode.attr("ExistingCount") != null && parseInt(divCurrentCode.attr("ExistingCount")) > 0) {
                        ExistingCount = divCurrentCode.attr("ExistingCount");
                    }
                    else {
                        divCurrentCode.attr("ExistingCount", PQRS_ICDCPTCodes.arrAllCodes["div" + divName].length);
                    }

                    $.each(arrCodeValues, function (j, CodeValue) {
                        if (ExistingCount > 0) {
                            ExistingCount += 1;
                            j = ExistingCount;
                        }
                        var myclass = "";
                        if (j > 9) {
                            myclass = "hidden";
                        }
                        //divCurrentCode.find("#lst" + CodeType).append("<a id='" + j + "' onclick=\"PQRS_ICDCPTCodes.fillCode('" + CodeType + "','" + CodeValue + "');\" class=\"btn btn-xs btn-link " + myclass + "\" href=\"#\">" + CodeValue + ",</a>");
                        divCurrentCode.find("#lst" + CodeType).append("<span id='span" + j + "' class='" + myclass + "'><input type='radio' name='" + divName + "' id='" + j + "' onclick=\"PQRS_ICDCPTCodes.fillCode('" + CodeType + "','" + CodeValue + "','" + "div" + divName + "',this);\" class=\"btn btn-xs btn-link mb-xs\" >" + CodeValue + "</input></span>");
                        if (PQRS_ICDCPTCodes.params.arrAttrNameValue != null) {
                            var selectedCode = PQRS_ICDCPTCodes.params.arrAttrNameValue["div" + divName.toLowerCase()];
                            if (selectedCode != null && selectedCode != "") {
                                //divCurrentCode.find("span[text='" + selectedCode + "'] input[type='radio'][name *='" + divName + "']").attr("checked", true);
                                var currentRadioBtn = divCurrentCode.find("span").filter(function () { return $(this).text().trim() == selectedCode; }).find("input[type='radio'][name *='" + divName + "']");
                                $(currentRadioBtn).attr("checked", true);
                            }
                        }
                        if (j == 9) {
                            //divList.append("<a class=\"btn btn-xs btn-link\" href=\"#\">" + CodeValue + ",</a>");
                            divCurrentCode.find("#lst" + CodeType).append("<a id='" + j + "' onclick=\"PQRS_ICDCPTCodes.ShowAll('" + "lst" + CodeType + "',this);\" class=\"btn btn-xs btn-link\" href=\"#\">More...,</a>");
                        }
                    });


                }
            });
            var allAndORDivs = $("#frmICDCPTCodes div[id*='divAndOR']");
            $(allAndORDivs[allAndORDivs.length - 1]).addClass("hidden");
        }
        else if ((PQRS_ICDCPTCodes.params.MeasureId == "0421a" || PQRS_ICDCPTCodes.params.MeasureId == "0419" || PQRS_ICDCPTCodes.params.MeasureId == "0043" || PQRS_ICDCPTCodes.params.MeasureId == "0068") && PQRS_ICDCPTCodes.params.CodeType != null && PQRS_ICDCPTCodes.params.CodeType != null) {
            PQRS_ICDCPTCodes.ShowMeasureCodesfor421();
            PQRS_ICDCPTCodes.ShowCodeSelection();
        }
        else if (PQRS_ICDCPTCodes.params.MeasureId == "PHQ2" || PQRS_ICDCPTCodes.params.MeasureId == "PHQ9") {
            PQRS_ICDCPTCodes.ShowMeasureCodesforVBP();
        }
        else {
            var divICD = $(' #lstICDs').text().split(',');
            $.each(divICD, function (i, item) {
                PQRS_ICDCPTCodes.arrICDs.push({

                    Code: item.trim()
                });
            });

            var divCPT = $(' #lstCPTs').text().split(',');
            $.each(divCPT, function (i, item) {
                PQRS_ICDCPTCodes.arrCPTs.push({

                    Code: item.trim()
                });
            });
        }


        $(' #pnlICDCPTCodes #frmICDCPTCodes #rdoAnd').prop('checked', true);
    },
    ShowMeasureCodesfor421: function () {
        if (PQRS_ICDCPTCodes.params.CodeType != null && PQRS_ICDCPTCodes.params.CodeType != null) {
            //$("#frmICDCPTCodes #headingTemplate").text(PQRS_ICDCPTCodes.params.ReasonDescription);
            var arrCodeType = PQRS_ICDCPTCodes.params.CodeType.split(",");
            var arrCodeOperators = PQRS_ICDCPTCodes.params.GroupOperator.split(",");
            var arrCodes = PQRS_ICDCPTCodes.params.CodeValues.split("|");
            var arrCodeDescriptions = PQRS_ICDCPTCodes.params.CodeDescription != null ? PQRS_ICDCPTCodes.params.CodeDescription.split("|") : [];
            var initialCounter = 1;
            var firstCodeType = "";
            $.each(arrCodeType, function (i, CodeType) {
                if (CodeType.trim() != "") {
                    if (firstCodeType == "") {
                        firstCodeType = CodeType;
                    }
                    var currentCodeOperator = "OR";
                    if (arrCodeOperators[i + 1] != null && arrCodeOperators[i + 1] != "") {
                        currentCodeOperator = arrCodeOperators[i + 1].trim();
                    }
                    var divName = CodeType;
                    var divCurrentCode = null;
                    if (currentCodeOperator == "AND") {
                        if ($("#frmICDCPTCodes div[id*='" + CodeType + "']").length > 0) {
                            divName = CodeType + initialCounter;
                            initialCounter += 1;
                        }

                        divCurrentCode = $("#frmICDCPTCodes #divCodesTemplate").clone();
                    }
                    else {
                        if ($("#frmICDCPTCodes div[id='" + "div" + firstCodeType + "']").length > 0) {
                            divCurrentCode = $("#frmICDCPTCodes div[id='" + "div" + firstCodeType + "']");//.clone();
                            //divName = divCurrentCode.attr("id").replace("div", "");
                        }
                        else {
                            divCurrentCode = $("#frmICDCPTCodes #divCodesTemplate").clone();
                        }
                    }


                    divCurrentCode.attr("id", "div" + firstCodeType);
                    divCurrentCode.removeClass("hidden");
                    divCurrentCode.find("#divSearch").removeClass("hidden").attr("id", "divSearchdiv" + firstCodeType);
                    $("#pnlICDCPTCodes #headingTitle").text(PQRS_ICDCPTCodes.params.ReasonDescription);
                    divCurrentCode.find("#headingTemplate").attr("id", "heading" + firstCodeType).text(PQRS_ICDCPTCodes.params.ReasonDescription);
                    divCurrentCode.find("#headerTemplate").attr("id", "header" + firstCodeType)
                    divCurrentCode.find("#header" + firstCodeType).addClass("hidden");
                    divCurrentCode.find("#txtTemplate").attr("id", "txt" + firstCodeType).attr("oninput", "PQRS_ICDCPTCodes.ShowCurrentCodeRadioBtn(this,'" + firstCodeType + "','" + "div" + firstCodeType + "');");
                    //PQRS_ICDCPTCodes.ShowCurrentCodeRadioBtn(obj, CodeType, ParentDivName, ui.item.value);
                    divCurrentCode.find("#btnTemplate").attr("id", "btn" + firstCodeType);
                    divCurrentCode.find("#lstTemplate").attr("id", "lst" + firstCodeType).empty();
                    divCurrentCode.find("#divAndOR").removeClass("hidden").attr("id", "divAndORdiv" + firstCodeType);
                    divCurrentCode.find("#divAndORdiv" + firstCodeType).addClass("disableAll");
                    divCurrentCode.find("#rdoAndTemplate").attr("id", "rdoAnddiv" + firstCodeType);
                    divCurrentCode.find("#rdoOrTemplate").attr("id", "rdoOrdiv" + firstCodeType);
                    divCurrentCode.find("#rdoAnddiv" + firstCodeType).attr('name', "div" + firstCodeType);
                    divCurrentCode.find("#rdoOrdiv" + firstCodeType).attr('name', "div" + firstCodeType);

                    if (currentCodeOperator == "AND") {
                        divCurrentCode.find("#rdoAnddiv" + firstCodeType).attr('checked', true);
                        divCurrentCode.find("#rdoOrdiv" + firstCodeType).attr('checked', false);
                    }
                    else {
                        divCurrentCode.find("#rdoAnddiv" + firstCodeType).attr('checked', false);
                        divCurrentCode.find("#rdoOrdiv" + firstCodeType).attr('checked', true);
                    }
                    if (currentCodeOperator == "AND" || $("#frmICDCPTCodes div[id*='" + firstCodeType + "']").length < 1) {
                        $("#frmICDCPTCodes").append(divCurrentCode);
                    }
                    var divCurrentCode = $("#frmICDCPTCodes #div" + firstCodeType)
                    var arrCodeValues = arrCodes[i].split(",");
                    var arrCodeDescription = arrCodeDescriptions[i] != null ? arrCodeDescriptions[i].split(",,") : [];
                    PQRS_ICDCPTCodes.arrAllCodes["div" + CodeType] = arrCodes[i].split(",");
                    var ExistingCount = 0;
                    if (divCurrentCode.attr("ExistingCount") != null && parseInt(divCurrentCode.attr("ExistingCount")) > 0) {
                        ExistingCount = divCurrentCode.attr("ExistingCount");
                    }
                    else {
                        divCurrentCode.attr("ExistingCount", PQRS_ICDCPTCodes.arrAllCodes["div" + CodeType].length);
                    }

                    $.each(arrCodeValues, function (j, CodeValue) {
                        //if (ExistingCount > 0) {
                        //    ExistingCount += 1;
                        //    j = ExistingCount;
                        //}
                        var myclass = "";
                        //if (j > 9) {
                        //    myclass = "hidden";
                        //}
                        //divCurrentCode.find("#lst" + CodeType).append("<a id='" + j + "' onclick=\"PQRS_ICDCPTCodes.fillCode('" + CodeType + "','" + CodeValue + "');\" class=\"btn btn-xs btn-link " + myclass + "\" href=\"#\">" + CodeValue + ",</a>");
                        if (CodeType == "LOINC" && CodeValue == "39156-5") {
                            var TextBoxCtrl = PQRS_ICDCPTCodes.ShowTextBox(CodeType, CodeValue, PQRS_ICDCPTCodes.params.DescValue);
                            divCurrentCode.find("#lst" + firstCodeType).append(TextBoxCtrl);//divSearchdivLOINC
                            divCurrentCode.find("#divSearchdiv" + firstCodeType).addClass("hidden");
                            var BMITextBox = divCurrentCode.find("#txtBMI");
                            BMITextBox.focus();

                        }
                        else {
                            var CodeDescription = CodeValue;
                            if (arrCodeDescription[j] != null && arrCodeDescription[j] != "") {
                                CodeDescription = arrCodeDescription[j];
                            }
                            divCurrentCode.find("#lst" + firstCodeType).append("<span title='" + CodeType + ": " + CodeValue + (CodeDescription != CodeValue ? " " + CodeDescription : "") + "' id='span" + j + "' class='" + myclass + " mr-xlg col-sm-3 noWordBreak'><input type='radio' name='" + divName + "' id='" + j + "' onclick=\"PQRS_ICDCPTCodes.fillCode('" + CodeType + "','" + CodeValue + "','" + "div" + firstCodeType + "',this);\" class=\"btn btn-xs btn-link mb-xs ml-xs\" >" + (CodeDescription != CodeValue ? (CodeDescription.length > 47 ? CodeDescription.substr(0, 47) + "...." : CodeDescription) : CodeValue) + "</input></span>");
                            if (PQRS_ICDCPTCodes.params.arrAttrNameValue != null) {
                                var selectedCode = PQRS_ICDCPTCodes.params.arrAttrNameValue["div" + firstCodeType.toLowerCase()];
                                if (selectedCode != null && selectedCode != "") {
                                    //divCurrentCode.find("span[text='" + selectedCode + "'] input[type='radio'][name *='" + divName + "']").attr("checked", true);
                                    var currentRadioBtn = divCurrentCode.find("span").filter(function () { return $(this).text().trim() == selectedCode; }).find("input[type='radio'][name *='" + divName + "']");
                                    $(currentRadioBtn).attr("checked", true);
                                }
                            }
                        }

                        //if (j == 9) {
                        //    //divList.append("<a class=\"btn btn-xs btn-link\" href=\"#\">" + CodeValue + ",</a>");
                        //    divCurrentCode.find("#lst" + CodeType).append("<a id='" + j + "' onclick=\"PQRS_ICDCPTCodes.ShowAll('" + "lst" + CodeType + "',this);\" class=\"btn btn-xs btn-link\" href=\"#\">More...,</a>");
                        //}
                    });


                }
            });
            var allAndORDivs = $("#frmICDCPTCodes div[id*='divAndOR']");
            $(allAndORDivs[allAndORDivs.length - 1]).addClass("hidden");
        }
        else {
            var divICD = $(' #lstICDs').text().split(',');
            $.each(divICD, function (i, item) {
                PQRS_ICDCPTCodes.arrICDs.push({

                    Code: item.trim()
                });
            });

            var divCPT = $(' #lstCPTs').text().split(',');
            $.each(divCPT, function (i, item) {
                PQRS_ICDCPTCodes.arrCPTs.push({

                    Code: item.trim()
                });
            });
        }


        $(' #pnlICDCPTCodes #frmICDCPTCodes #rdoAnd').prop('checked', true);
    },
    ShowMeasureCodesforVBP: function () {
        var divVBPAnswer = $("#frmICDCPTCodes #divVBPAnswer");
        var lblQuestion = $("#frmICDCPTCodes #lblVBPQuestion").text(PQRS_ICDCPTCodes.params.ReasonDescription.replace(".", ":"));
        var objddlAnswer = $("#frmICDCPTCodes #ddlVBPAnswerId");
        divVBPAnswer.removeClass("hidden");//VBPQuestionAnswers
        $("#pnlICDCPTCodes #headingTitle").text(PQRS_ICDCPTCodes.params.ReasonDescription);
        var currentQuestionAnswers = $.grep(PQRS_ICDCPTCodes.params.VBPQuestionAnswers, function (a) {
            return a.Question == PQRS_ICDCPTCodes.params.ReasonDescription;
        });
        PQRS_ICDCPTCodes.LoadVBPAnswer(PQRS_ICDCPTCodes.params.MeasureId, objddlAnswer, currentQuestionAnswers);
    },
    LoadVBPAnswer: function (measureId, objddl, arrAnswers) {
        // Loads Entity Based Basic Fee Group
        objddl.empty();
        objddl.append($('<option/>', {
            value: "",
            html: "- SELECT -"
        }));
        $.each(arrAnswers, function (i, item) {
            objddl.append(
                $('<option/>', {
                    value: item.QuestionAnswersId,
                    html: item.Answer,
                    MeasureQuestionnaireId: item.MeasureQuestionnaireId
                })
            );
        });
        PQRS_ICDCPTCodes.ShowVBPAnswerSelection();
    },
    ShowVBPAnswerSelection: function () {
        if (PQRS_ICDCPTCodes.params.selectedValue != null && PQRS_ICDCPTCodes.params.selectedValue != "") {
            var arrSelectedValues = PQRS_ICDCPTCodes.params.selectedValue.split(",");
            $.each(arrSelectedValues, function (i, selectedValue) {
                var arrselectedValue = selectedValue.split(":");
                if (arrselectedValue.length > 2) {
                    var currentCtrl = $("#pnlICDCPTCodes #frmICDCPTCodes select#" + arrselectedValue[0]);
                    currentCtrl.val(arrselectedValue[2]);
                    currentCtrl.attr("PreviousValue", arrselectedValue[2]);
                }
            });
        }
    },
    ShowCodeSelection: function () {
        if (PQRS_ICDCPTCodes.params.selectedValue != null && PQRS_ICDCPTCodes.params.selectedValue != "") {
            var arrSelectedValues = PQRS_ICDCPTCodes.params.selectedValue.split(",");
            $.each(arrSelectedValues, function (i, selectedValue) {
                var arrselectedValue = selectedValue.split(":");
                if (arrselectedValue.length > 2) {
                    var currentCtrl = $("#pnlICDCPTCodes #frmICDCPTCodes input[id='" + arrselectedValue[0] + "'][name='" + arrselectedValue[1] + "']");
                    if (arrselectedValue[2].trim() == "checked") {
                        currentCtrl.prop('checked', true);
                    }
                    else {
                        currentCtrl.val(arrselectedValue[2].trim());
                        currentCtrl.attr("PreviousValue", arrselectedValue[0] + ":" + arrselectedValue[1] + ":" + arrselectedValue[2].trim());
                    }
                }
            });
        }
        var BMITextBox = $("#pnlICDCPTCodes #frmICDCPTCodes input[id='txtBMI'][name='txtBMI']");
        if (BMITextBox.length > 0 && BMITextBox.val().trim() == "") {
            PQRS_ICDCPTCodes.loadPatientBMI(PQRS_ICDCPTCodes.params["Patientid"], PQRS_ICDCPTCodes.params["NoteId"], BMITextBox);
        }
        else {
            setTimeout(function () {
                var BMITextBox = $("#pnlICDCPTCodes #frmICDCPTCodes input[id='txtBMI'][name='txtBMI']");
                BMITextBox.focus();
            }, 500);
        }

    },

    loadPatientBMI: function (PatientId, NoteId, TextBoxCtrl) {
        var objData = {};
        objData.PatientId = PatientId;
        objData.NoteId = NoteId;
        objData.ReportFromDate = PQRS_Patient_List.params.ReportFromDate;
        objData.ReportToDate = PQRS_Patient_List.params.ReportToDate;
        objData.commandType = "load_patient_bmi";

        PQRS_ICDCPTCodes.loadPatientBMI_DbCall(objData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                TextBoxCtrl.val(response.BMI);//.focus();;
                setTimeout(function () {
                    var BMITextBox = $("#pnlICDCPTCodes #frmICDCPTCodes input[id='txtBMI'][name='txtBMI']");
                    BMITextBox.focus();
                }, 500);
                //$(TextBoxCtrl).focus();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    loadPatientBMI_DbCall: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    ShowTextBox: function (CodeType, CodeValue, DescValue) {
        var TextBoxCtrl = "";
        var SpanText = "Enter BMI ";
        if (CodeType == "LOINC" && CodeValue == '39156-5') {
            var arrBMIValue = DescValue.trim().split("(");//(result >= 18.5 kg/m2)(result < 25 kg/m2)
            var strBMIValue = ""
            var minBMIValue = 0;
            var maxBMIValue = 0;
            var BMIValueLength = arrBMIValue.length;
            $.each(arrBMIValue, function (i, item) {
                strBMIValue = item.replace("kg/m2", "");
                strBMIValue = utility.getNumericPart(strBMIValue);
                if (item != "") {
                    if (BMIValueLength > 2) {
                        if (item.indexOf(">") > -1 || item.indexOf(">=") > -1) {
                            minBMIValue = strBMIValue.replace(")", "").trim();
                            if (item.indexOf(">=") > -1) {
                                SpanText += "greater than or equal to " + minBMIValue + "kg/m2";
                            }
                            else if (item.indexOf(">") > -1) {
                                SpanText += "greater than " + minBMIValue + "kg/m2";
                            }

                        }
                        else if (item.indexOf("<") > -1 || item.indexOf("<=") > -1) {
                            maxBMIValue = strBMIValue.replace(")", "").trim();
                            if (item.indexOf("<=") > -1) {
                                SpanText += (SpanText == "Enter BMI " ? " lesser than equal to " : " and lesser than equal to ") + maxBMIValue + "kg/m2";
                            }
                            else if (item.indexOf("<") > -1) {
                                SpanText += (SpanText == "Enter BMI " ? " lesser than " : " and lesser than ") + maxBMIValue + "kg/m2";
                            }
                        }
                    }
                    else {
                        if (item.indexOf(">") > -1 || item.indexOf(">=") > -1) {
                            minBMIValue = strBMIValue.replace(")", "").trim();
                            maxBMIValue = 0;
                            if (item.indexOf(">=") > -1) {
                                SpanText += "greater than or equal to " + minBMIValue + "kg/m2";
                            }
                            else if (item.indexOf(">") > -1) {
                                SpanText += "greater than " + minBMIValue + "kg/m2";
                            }
                        }
                        else if (item.indexOf("<") > -1 || item.indexOf("<=") > -1) {
                            maxBMIValue = strBMIValue.replace(")", "").trim();
                            minBMIValue = 0;
                            if (item.indexOf("<=") > -1) {
                                SpanText += (SpanText == "Enter BMI " ? " lesser than equal to " : " and lesser than equal to ") + maxBMIValue + "kg/m2";
                            }
                            else if (item.indexOf("<") > -1) {
                                SpanText += (SpanText == "Enter BMI " ? " lesser than " : " and lesser than ") + maxBMIValue + "kg/m2";
                            }
                        }
                    }
                }
            });

            var TextBoxName = "txtBMI";
            TextBoxCtrl = '<span>' + SpanText + ':' + '</span><br/>';
            TextBoxCtrl += '<input type="text" Ctrltype="CQM" title="' + DescValue + '" MeasureId="' + PQRS_ICDCPTCodes.params.MeasureId + '" PatientId="' + PQRS_ICDCPTCodes.params.Patientid + '" NoteId="' + PQRS_ICDCPTCodes.params.NoteId + '" LOINC="39156-5" id="' + TextBoxName + '" max="255" name="' + TextBoxName + '" data-plugin-keyboard-numpad maxlength="5" class="form-control" onblur="utility.ValidateValue(event, 2,\'' + minBMIValue + '\',\'' + maxBMIValue + '\');"><br/>';
            return TextBoxCtrl;
        }
    },
    ShowAll: function (lstId, obj) {
        if (lstId != null && lstId != "") {
            $(obj).addClass("hidden");
            var currentlst = $(obj).parent();//$("#frmICDCPTCodes #" + lstId);
            $.each(currentlst.find("span"), function (i, Anchor) {
                var currentId = $(Anchor).attr("id").replace("span", "");
                if (parseInt(currentId) > 9)
                    $(Anchor).removeClass("hidden");
                else
                    var myvar = "1";
            });
        }

    },

    fillCode: function (CodeType, Code, ParentDivName, obj) {
        //var fromCtrl = $("#" + PQRS_Patient_List.params.PanelID + " input[id='" + PQRS_ICDCPTCodes.params.FromCtrl + "'][name='" + PQRS_ICDCPTCodes.params.FromCtrlName + "']");
        if (PQRS_ICDCPTCodes.params.MeasureId != "0421a" && PQRS_ICDCPTCodes.params.MeasureId != "0419" && PQRS_ICDCPTCodes.params.MeasureId != "0043" && PQRS_ICDCPTCodes.params.MeasureId != "0068") {
            var fromCtrl = $("#" + PQRS_Patient_List.params.PanelID + " #dgvMeasureReasons input[id='" + PQRS_ICDCPTCodes.params.FromCtrl + "']");
            if (fromCtrl != null) {
                var checked = fromCtrl.attr('checked');
                if (!checked) {
                    fromCtrl.attr('checked', true);
                }
                var ExistingCode = fromCtrl.attr(CodeType);
                //ExistingCode = Code;
                if (ExistingCode != null && ExistingCode != "") {
                    ExistingCode = ExistingCode + "," + Code;
                }
                else {
                    ExistingCode = Code;
                }
                fromCtrl.attr(CodeType, ExistingCode);
                fromCtrl.attr(ParentDivName, Code);
            }
        }
        else {
            if (obj != null) {
                obj = $(obj);
                if (obj.attr('previousValue') == 'true') {
                    obj.prop('checked', false)
                } else {
                    obj.attr('previousValue', false);
                }
                obj.attr('previousValue', obj.prop('checked'));
            }
        }

    },
    //PQRS_Patient_List.params.PanelID
    UnLoad: function (caller) {
        if (caller!=null && caller.toLowerCase() == "save") {
            var FromCtrl = $("#anchorAssociateCode" + PQRS_ICDCPTCodes.params.FromCtrl);
            var currentRadioCtrl = $("#pnlICDCPTCodes input[type='radio']:checked");
            var selectedRadioLength = currentRadioCtrl.length;

            $.each(currentRadioCtrl, function (index, item) {
                var Id = $(item).attr("id");
                var Name = $(item).attr("name");
                if (selectedRadioLength != null && selectedRadioLength > 1) {
                    if (FromCtrl.attr("selectedValue") != null) {
                        if (FromCtrl.attr("selectedValue").indexOf(Id + ":" + Name) < 0) {
                            FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue") + "," + Id + ":" + Name + ":checked");
                        }
                        else {
                            FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue").replace(Id + ":" + Name + ":checked", Id + ":" + Name + ":checked"));
                        }
                    }
                    else {
                        FromCtrl.attr("selectedValue", Id + ":" + Name + ":checked");
                    }
                    var parent = $(item).parent();
                    var ActionResult = $(item).attr("ActionResult");
                    if (parent != null && parent.attr("title") != null) {
                        var title = parent.attr("title").replace(Name + ": ", "").trim();
                        title = title.split(" ")[0];
                        if (title != "") {
                            FromCtrl.attr(Name, title);
                            if (ActionResult != null && ActionResult != "") {
                                FromCtrl.attr("ActionResult", ActionResult);
                            }
                            FromCtrl.attr("hasValue", true);
                        }
                        else {
                            FromCtrl.attr("hasValue", false);
                        }

                    }
                }
                else {
                    FromCtrl.attr("hasValue", false);
                }

            });


            var currentTextCtrl = $("#pnlICDCPTCodes input[type='text'][Ctrltype='CQM']");
            $.each(currentTextCtrl, function (index, item) {
                var Id = $(item).attr("id");
                var Name = $(item).attr("name");
                var Value = $(item).val();
                var LOINC = $(item).attr("loinc");
                if (FromCtrl.attr("selectedValue") != null) {
                    if (FromCtrl.attr("selectedValue").indexOf(Id + ":" + Name) < 0) {
                        FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue") + "," + Id + ":" + Name + ":" + Value);
                    }
                    else {
                        var OldValue = $(item).attr("PreviousValue");
                        if (OldValue != null && OldValue != "") {
                            FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue").replace(OldValue, Id + ":" + Name + ":" + Value));
                        }
                        else {
                            FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue").replace(Id + ":" + Name + ":" + Value, Id + ":" + Name + ":" + Value));
                        }
                    }
                }
                else {
                    FromCtrl.attr("selectedValue", Id + ":" + Name + ":" + Value);
                }
                if (Name == "txtBMI") {
                    FromCtrl.attr("BMI", Value);
                    FromCtrl.attr("BMILOINC", LOINC);
                    if (Value != "") {

                        FromCtrl.attr("hasValue", true);
                    }
                    else {
                        FromCtrl.attr("hasValue", false);
                    }
                }
            });

            var currentTextCtrl = $("#pnlICDCPTCodes select[Values!='']");
            $.each(currentTextCtrl, function (index, item) {
                var Id = $(item).attr("id");
                var Name = $(item).attr("name");
                var MeasureQuestionnaireId = $(item).find("Option:selected").attr("MeasureQuestionnaireId");
                var Value = $(item).find("Option:selected").val();
                var LOINC = $(item).attr("loinc");
                if (FromCtrl.attr("selectedValue") != null) {
                    if (FromCtrl.attr("selectedValue").indexOf(Id + ":" + Name) < 0) {
                        FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue") + "," + Id + ":" + Name + ":" + Value);
                    }
                    else {
                        var OldValue = $(item).attr("PreviousValue");
                        if (OldValue != null && OldValue != "") {
                            FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue").replace(Id + ":" + Name + ":" + OldValue, Id + ":" + Name + ":" + Value));
                        }
                        else {
                            FromCtrl.attr("selectedValue", FromCtrl.attr("selectedValue").replace(Id + ":" + Name + ":" + Value, Id + ":" + Name + ":" + Value));
                        }
                    }
                }
                else {
                    FromCtrl.attr("selectedValue", Id + ":" + Name + ":" + Value);
                }
                if (Value != "") {
                    if (Name == "AnswerId") {
                        Name = "QuestionAnswersId";
                    }
                    FromCtrl.attr(Name, Value);
                    FromCtrl.attr("MeasureQuestionnaireId", MeasureQuestionnaireId);
                    FromCtrl.attr("hasValue", true);
                }
                else {
                    FromCtrl.attr("hasValue", false);
                }
            });

            var MainRadCtrls = $("#" + PQRS_Patient_List.params.PanelID + " #dgvMeasureReasons input[type='radio'][id*='MainRad']");
            $.each(MainRadCtrls, function (index, radio) {
                var parentTd = $(radio).parent();
                var assocAnchors = parentTd.find("a[id*='anchorAssociateCode']");
                var anchorLength = assocAnchors.length;
                if (PQRS_Patient_List.params.measureId == "0043") {
                    var hasValue = parentTd.find("a[id*='anchorAssociateCode'][hasValue='true']");
                    if (hasValue.length > 0) {
                        $(radio).prop("checked", true);
                        parentTd.addClass("bg-success");
                    }
                    else {
                        $(radio).prop("checked", false);
                        parentTd.removeClass("bg-success");
                    }
                }
                else if (PQRS_Patient_List.params.measureId == "PHQ2") {
                    var hasValue = parentTd.find("a[id*='anchorAssociateCode'][hasValue='true']");
                    if (hasValue.length > 1) {
                        $(radio).prop("checked", true);
                        parentTd.addClass("bg-success");
                    }
                    else {
                        $(radio).prop("checked", false);
                        parentTd.removeClass("bg-success");
                    }
                }
                else if (PQRS_Patient_List.params.measureId == "PHQ9") {
                    var hasValue = parentTd.find("a[id*='anchorAssociateCode'][hasValue='true']");
                    if (hasValue.length > 0) {
                        $(radio).prop("checked", true);
                        parentTd.addClass("bg-success");
                    }
                    else {
                        $(radio).prop("checked", false);
                        parentTd.removeClass("bg-success");
                    }
                }
                else {
                    $.each(assocAnchors, function (i, anchor) {
                        if (anchorLength > 1) {
                            var Mandatory = null;
                            var Optional = null;
                            var IndexToCompare = 0;
                            if (i == IndexToCompare) {
                                Mandatory = parentTd.find("a[id='" + $(anchor).attr("id") + "'][hasValue='true']");
                                Optional = parentTd.find("a[id!='" + $(anchor).attr("id") + "'][hasValue='true']");
                                if (Mandatory.length > 0 && Optional.length > 0) {
                                    $(radio).prop("checked", true);
                                    parentTd.addClass("bg-success");
                                }
                                else {
                                    $(radio).prop("checked", false);
                                    parentTd.removeClass("bg-success");
                                }
                            }
                        }
                        else {
                            if ($(anchor).attr("hasValue") != null) {
                                if ($(anchor).attr("hasValue") == "true") {
                                    $(radio).prop("checked", true);
                                    parentTd.addClass("bg-success");
                                }
                                else {
                                    $(radio).prop("checked", false);
                                    parentTd.removeClass("bg-success");
                                }

                            }
                        }

                    });
                }

            });
        }
        var mystr = 123;
        //return;
        if (PQRS_ICDCPTCodes.params != null && PQRS_ICDCPTCodes.params.ParentCtrl != null) {
            UnloadActionPan(PQRS_ICDCPTCodes.params.ParentCtrl, 'PQRS_ICDCPTCodes');
        }
        else
            UnloadActionPan(null, 'PQRS_ICDCPTCodes');
    },

    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "PQRS_ICDCPTCodes", null, false);
    },

    BindCode: function (obj, CodeType, ParentDivName) {
        utility.Keyupdelay(function () {
            $(obj).autocomplete({
                autoFocus: true,
                source: PQRS_ICDCPTCodes.arrAllCodes[ParentDivName],
                select: function (event, ui) {
                    setTimeout(function () {
                        //$("#" + ContainerCtrl + " #" + hfcontrolid).val(ui.item.id);
                        $(obj).val(ui.item.value);
                        PQRS_ICDCPTCodes.ShowCurrentCodeRadioBtn(obj, CodeType, ParentDivName, ui.item.value);
                    }, 100);
                }
            });
            //CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            //    $(obj).autocomplete({
            //        autoFocus: true,
            //        source: InsurancePlans,
            //        select: function (event, ui) {
            //            setTimeout(function () {
            //                //$("#" + ContainerCtrl + " #" + hfcontrolid).val(ui.item.id);
            //                $(obj).val(ui.item.value);
            //            }, 100);
            //        }
            //    });
            //});
        });
    },
    ShowCurrentCodeRadioBtn: function (obj, CodeType, ParentDivName, ValueToSearch) {
        ValueToSearch = $(obj).val();
        var prnt = $(obj).parent();
        var arrSpanToShow = $("#" + ParentDivName + " #lst" + CodeType).find("span:contains('" + ValueToSearch + "')");
        var RadioToShow = $("#" + ParentDivName + " #lst" + CodeType).find("span:contains('" + ValueToSearch + "')").find("input[type='radio']");
        $.each($("#" + ParentDivName + " #lst" + CodeType).find("span"), function (i, item) {
            $(item).addClass("hidden");
        });
        if (ValueToSearch != "") {
            $("#" + ParentDivName + " #lst" + CodeType).find("a").addClass("hidden");
            $.each(arrSpanToShow, function (j, SPanToShow) {
                $(SPanToShow).removeClass("hidden");
            });
        }
        else {
            $("#" + ParentDivName + " #lst" + CodeType).find("a").addClass("hidden");
            $.each($("#" + ParentDivName + " #lst" + CodeType).find("span"), function (i, item) {
                $(item).removeClass("hidden");

            });
        }


        var str = 1;
        var str = 2;
    },

    openSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = PQRS_ICDCPTCodes.params["TabID"];
        if (PQRS_ICDCPTCodes.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'PQRS_ICDCPTCodes';
        }
        else
            params["ParentCtrl"] = 'PQRS_ICDCPTCodes';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (PQRS_ICDCPTCodes.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    bindAutoComplete: function (element) {


        var hiddenCrtl = $('#' + PQRS_ICDCPTCodes.params.PanelID + ' #txtCPTCode');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "PQRS_ICDCPTCodes", null, true);

    },
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "PQRS_ICDCPTCodes";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = PQRS_ICDCPTCodes.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params);
    },


}