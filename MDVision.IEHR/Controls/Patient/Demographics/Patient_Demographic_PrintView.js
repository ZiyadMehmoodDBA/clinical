Patient_Demographic_PrintView = {
    bIsFirstLoad: true,
    params: [],
    pdf: '',
    Load: function (params) {
        Patient_Demographic_PrintView.params = params;
        Patient_Demographic_PrintView.pdf = "";

        if (Patient_Demographic_PrintView.params == null) {
            Patient_Demographic_PrintView.params = [];
        }
        if (Patient_Demographic_PrintView.params.PanelID != "pnlPatient_Demographic_PrintView") {
            Patient_Demographic_PrintView.params["PanelID"] = "pnlPatient_Demographic_PrintView";
        }

        if (Patient_Demographic_PrintView.bIsFirstLoad) {
            Patient_Demographic_PrintView.bIsFirstLoad = false;

            var PatientId = Patient_Demographic_PrintView.params.PatientId;
            var PatientInsuranceId = Patient_Demographic_PrintView.params.InsuranceId;
            var DemoInsuranceJSON = Patient_Demographic_PrintView.params.DemographicsJSON ? Patient_Demographic_PrintView.params.DemographicsJSON : Patient_Demographic_PrintView.params.InsuranceJSON;
            var PreviewPrintFor = Patient_Demographic_PrintView.params.PreviewPrintFor;

            Patient_Demographic_PrintView.DemoInsurancePrintPreview(DemoInsuranceJSON, true, PreviewPrintFor);
        }
    },

    DemoInsurancePrintPreview: function (DemoInsuranceJSON, IsPreview, PreviewPrintFor) {
        var dfd = new $.Deferred();
        var demoInsuranceInfo = Patient_Demographic_PrintView.storeDemoInsuranceData(JSON.parse(DemoInsuranceJSON), PreviewPrintFor);
        var demoInsNonEmptyData = {};
        var spanMarkup = '<span class="required">*</span>';
        //declare demographics and insurance Object arrays to match with JSON keys and once object is print, IsPrint will be equal to 1
        var demographicsObjectDataArr = [{ 'key': 'Account', 'IsPrint': '0' }, { 'key': 'Sexual Orientation', 'IsPrint': '0' }, { 'key': 'MRN', 'IsPrint': '0' }, { 'key': 'Birth Sex', 'IsPrint': '0' }, { 'key': 'SSN', 'IsPrint': '0' }, { 'key': 'Patient Status', 'IsPrint': '0' }, { 'key': 'Prefix', 'IsPrint': '0' }, { 'key': 'Care Giver', 'IsPrint': '0' }, { 'key': 'Last Name', 'IsPrint': '0' }, { 'key': 'Self Payer', 'IsPrint': '0' },
                                         { 'key': 'Suffix', 'IsPrint': '0' }, { 'key': 'Address 1', 'IsPrint': '0' }, { 'key': 'First Name', 'IsPrint': '0' }, { 'key': 'City', 'IsPrint': '0' }, { 'key': 'Middle/Initial Name', 'IsPrint': '0' }, { 'key': 'State', 'IsPrint': '0' }, { 'key': 'Previous Name', 'IsPrint': '0' }, { 'key': 'Zip', 'IsPrint': '0' }, { 'key': "Mother's Maiden", 'IsPrint': '0' }, { 'key': 'Country', 'IsPrint': '0' },
                                         { 'key': 'Date of Birth', 'IsPrint': '0' }, { 'key': 'Preferred Address', 'IsPrint': '0' }, { 'key': 'Age', 'IsPrint': '0' }, { 'key': 'Home Tel', 'IsPrint': '0' }, { 'key': 'Sex', 'IsPrint': '0' }, { 'key': 'Work Tel & Ext(If exists)', 'IsPrint': '0' }, { 'key': 'Marital Status', 'IsPrint': '0' }, { 'key': 'Cell', 'IsPrint': '0' }, { 'key': 'Ethnicity', 'IsPrint': '0' }, { 'key': 'Fax', 'IsPrint': '0' },
                                         { 'key': 'Race', 'IsPrint': '0' }, { 'key': 'Email', 'IsPrint': '0' }, { 'key': 'Preferred Language', 'IsPrint': '0' }, { 'key': 'Preferred Phone', 'IsPrint': '0' }, { 'key': 'Gender Identity', 'IsPrint': '0' }, { 'key': 'Bad Address', 'IsPrint': '0' }, { 'key': 'Last Seen Provider', 'IsPrint': '0' }, { 'key': 'Referring Provider', 'IsPrint': '0' }, { 'key': 'Last Seen Facility', 'IsPrint': '0' },
                                         { 'key': 'PCP', 'IsPrint': '0' }, { 'key': 'Practice', 'IsPrint': '0' }, { 'key': 'Guarantor', 'IsPrint': '0' }, { 'key': 'Comments', 'IsPrint': '0' }];
        var insuranceObjectDataArr = [{ 'key': 'Insurance', 'IsPrint': '0' }, { 'key': 'Insurance Unassigned', 'IsPrint': '0' }, { 'key': 'Insurance Plan', 'IsPrint': '0' }, { 'key': 'Assignment Accepted', 'IsPrint': '0' }, { 'key': 'Subscriber ID', 'IsPrint': '0' }, { 'key': 'PAR/NON PAR', 'IsPrint': '0' }, { 'key': 'Visit Copay', 'IsPrint': '0' }, { 'key': 'Specialist Copay', 'IsPrint': '0' }, { 'key': 'Relation', 'IsPrint': '0' },
                                      { 'key': 'Subscriber Group ID', 'IsPrint': '0' }, { 'key': 'Last Name', 'IsPrint': '0' }, { 'key': 'Plan Address', 'IsPrint': '0' }, { 'key': 'First Name', 'IsPrint': '0' }, { 'key': 'Telephone', 'IsPrint': '0' }, { 'key': 'M.I. / Name', 'IsPrint': '0' }, { 'key': 'Coverage Date From', 'IsPrint': '0' }, { 'key': 'Date of Birth', 'IsPrint': '0' }, { 'key': 'Coverage Date To', 'IsPrint': '0' },
                                      { 'key': 'Sex', 'IsPrint': '0' }, { 'key': 'Date Signed', 'IsPrint': '0' }, { 'key': 'SSN', 'IsPrint': '0' }, { 'key': 'Lawyer', 'IsPrint': '0' }, { 'key': 'Address 1', 'IsPrint': '0' }, { 'key': 'Employer', 'IsPrint': '0' }, { 'key': 'City', 'IsPrint': '0' }, { 'key': 'MSP Type', 'IsPrint': '0' }, { 'key': 'State', 'IsPrint': '0' }, { 'key': 'Eligibility Date', 'IsPrint': '0' },
                                      { 'key': 'Zip', 'IsPrint': '0' }, { 'key': 'Eligibility Status', 'IsPrint': '0' }, { 'key': 'Home Tel', 'IsPrint': '0' }, { 'key': 'Comments', 'IsPrint': '0' }];

        var dfd_html = new $.Deferred();

        if (IsPreview == true) {
            var $html = $("#" + Patient_Demographic_PrintView.params["PanelID"]);
            dfd_html.resolve($html);
        }
        else {
            var ajax_get = $.get("./Controls/Patient/Demographics/Patient_Demographic_PrintView.html", {
                cache: false
            }, function (content) {
                var $html = $(content);
                $("body").append($html);
                dfd_html.resolve($html);
            }, "html");
        }

        dfd_html.then(function ($html) {
            var tblPatientDemoInsurance = $html.find("#frmPatient_Demographic_PrintView #tblPatientDemoInsurance > tbody");

            //Heading Date
            $html.find("#frmPatient_Demographic_PrintView #HeaderCurrDate").html(utility.RemoveTimeFromDate(null, Date()));

            //filter demographics and insurance JSON data
            $.each(demoInsuranceInfo, function (key, value) {
                if (value) {
                    if (value.toLowerCase() != 'select' && value.toLowerCase() != '- select -' && value.toLowerCase() != '-- select --') {
                        demoInsNonEmptyData[key] = value;
                    }
                }
            });

            if (PreviewPrintFor == 'Demographics') {
                $html.find("#frmPatient_Demographic_PrintView #HeadingTitle").html('<b>Patient Demographics</b>');
                //print demographics non-empty data
                var markup = '<tr>';
                var count = 0;
                for (var i = 0; i < demographicsObjectDataArr.length; i++) {
                    $.each(demoInsNonEmptyData, function (key, value) {
                        if (demographicsObjectDataArr[i].key.split(" ").join('') == key && demographicsObjectDataArr[i].IsPrint == '0') {
                            if (key == 'LastName' || key == 'Address1' || key == 'FirstName' || key == 'City' || key == 'State' || key == 'Zip' || key == 'DateofBirth' || key == 'HomeTel' || key == 'Sex' || key == 'MaritalStatus' || key == 'Ethnicity' || key == 'Race' || key == 'PreferredLanguage' || key == 'Practice')
                                markup += '<td><strong>' + demographicsObjectDataArr[i].key + '' + spanMarkup + ':</strong></td><td>' + value + '</td>';
                            else
                                markup += '<td><strong>' + demographicsObjectDataArr[i].key + ':</strong></td><td>' + value + '</td>';
                            demographicsObjectDataArr[i].IsPrint = '1';
                            count++;
                        }
                        if (count == 2) {
                            $(tblPatientDemoInsurance).append(markup + '</tr>');
                            markup = '<tr>';
                            count = 0;
                        }
                        else if (((Object.keys(demoInsNonEmptyData).length % 2) == 1) && (i == demographicsObjectDataArr.length - 1 && count == 1)) {
                            $(tblPatientDemoInsurance).append(markup.split('</td>')[0] + '</td><td colspan="3">' + markup.split('</td>')[1].split('<td>')[1] + '</td></tr>');
                            markup = '<tr>';
                            count = 0;
                        }
                    });
                };
            }
            else {
                $html.find("#frmPatient_Demographic_PrintView #HeadingTitle").html('<b>Patient Insurance</b>');
                //print insurance non-empty data
                var markup = '<tr>';
                var count = 0;
                for (var i = 0; i < insuranceObjectDataArr.length; i++) {
                    $.each(demoInsNonEmptyData, function (key, value) {
                        if (insuranceObjectDataArr[i].key.split(" ").join('') == key && insuranceObjectDataArr[i].IsPrint == '0') {
                            if (key == 'InsurancePlan' || key == 'SubscriberID' || key == 'Relation' || key == 'LastName' || key == 'PlanAddress' || key == 'FirstName' || key == 'DateofBirth' || key == 'Sex')
                                markup += '<td><strong>' + insuranceObjectDataArr[i].key + '' + spanMarkup + ':</strong></td><td>' + value + '</td>';
                            else
                                markup += '<td><strong>' + insuranceObjectDataArr[i].key + ':</strong></td><td>' + value + '</td>';
                            insuranceObjectDataArr[i].IsPrint = '1';
                            count++;
                        }
                        if (count == 2) {
                            $(tblPatientDemoInsurance).append(markup + '</tr>');
                            markup = '<tr>';
                            count = 0;
                        }
                        else if (((Object.keys(demoInsNonEmptyData).length % 2) == 1) && (i == insuranceObjectDataArr.length - 1 && count == 1)) {
                            $(tblPatientDemoInsurance).append(markup.split('</td>')[0] + '</td><td colspan="3">' + markup.split('</td>')[1].split('<td>')[1] + '</td></tr>');
                            markup = '<tr>';
                            count = 0;
                        }
                    });
                };
            }

            BackgroundLoaderShow(true);
            Patient_Demographic_PrintView.getPrintPDF($html, IsPreview).done(function () {
                if (IsPreview == false)
                    $("body").find($html).remove();

                BackgroundLoaderShow(false);
                dfd.resolve();
            });
        });
        return dfd.promise();
    },

    storeDemoInsuranceData: function (demoInsuranceJSONData, PreviewPrintFor) {  
        var demoInsuranceObjectData = {};
        //store keys and values of demographics and insurance according to screen fields
        if (PreviewPrintFor == 'Demographics') {
            demoInsuranceObjectData["Account"] = demoInsuranceJSONData.AccountNo;
            demoInsuranceObjectData["SexualOrientation"] = demoInsuranceJSONData.SexualOrientationId_text;
            demoInsuranceObjectData["MRN"] = demoInsuranceJSONData.MRN;
            demoInsuranceObjectData["BirthSex"] = demoInsuranceJSONData.BirthSex_text;
            demoInsuranceObjectData["SSN"] = demoInsuranceJSONData.SSN;
            demoInsuranceObjectData["PatientStatus"] = demoInsuranceJSONData.Active_text;
            demoInsuranceObjectData["Prefix"] = demoInsuranceJSONData.Prefix_text;
            demoInsuranceObjectData["CareGiver"] = demoInsuranceJSONData.CareGiver;
            demoInsuranceObjectData["LastName"] = demoInsuranceJSONData.LastName;
            demoInsuranceObjectData["SelfPayer"] = demoInsuranceJSONData.SelfPay == false ? "No" : "Yes";
            demoInsuranceObjectData["Suffix"] = demoInsuranceJSONData.Suffix_text;
            demoInsuranceObjectData["Address1"] = demoInsuranceJSONData.Address1;
            demoInsuranceObjectData["FirstName"] = demoInsuranceJSONData.FirstName;
            demoInsuranceObjectData["City"] = demoInsuranceJSONData.City;
            demoInsuranceObjectData["Middle/InitialName"] = demoInsuranceJSONData.MiddleInitial;
            demoInsuranceObjectData["State"] = demoInsuranceJSONData.State;
            demoInsuranceObjectData["Zip"] = demoInsuranceJSONData.Zip;
            demoInsuranceObjectData["PreviousName"] = demoInsuranceJSONData.PreviousName;
            demoInsuranceObjectData["Mother'sMaiden"] = demoInsuranceJSONData.MotherMaidenName;
            demoInsuranceObjectData["Country"] = demoInsuranceJSONData.Country;
            demoInsuranceObjectData["DateofBirth"] = demoInsuranceJSONData.DOB;
            demoInsuranceObjectData["PreferredAddress"] = demoInsuranceJSONData.PreferredAddressID_text;
            demoInsuranceObjectData["Age"] = demoInsuranceJSONData.Age;
            demoInsuranceObjectData["HomeTel"] = demoInsuranceJSONData.HomeTel;
            demoInsuranceObjectData["Sex"] = demoInsuranceJSONData.Sex_text;
            demoInsuranceObjectData["WorkTel&Ext(Ifexists)"] = demoInsuranceJSONData.Ext ? demoInsuranceJSONData.WorkTel + ' ' + demoInsuranceJSONData.Ext : demoInsuranceJSONData.WorkTel;
            demoInsuranceObjectData["MaritalStatus"] = demoInsuranceJSONData.MaritalStatus_text;
            demoInsuranceObjectData["Cell"] = demoInsuranceJSONData.Cell;
            demoInsuranceObjectData["Ethnicity"] = demoInsuranceJSONData.Ethnicity_text;
            demoInsuranceObjectData["Fax"] = demoInsuranceJSONData.Fax;
            demoInsuranceObjectData["Race"] = demoInsuranceJSONData.PatientRaceIds_text;
            demoInsuranceObjectData["Email"] = demoInsuranceJSONData.Email;
            demoInsuranceObjectData["PreferredLanguage"] = demoInsuranceJSONData.PrefLanguage;
            demoInsuranceObjectData["PreferredPhone"] = demoInsuranceJSONData.PreferredPhoneID_text;
            demoInsuranceObjectData["GenderIdentity"] = demoInsuranceJSONData.GenderIdentityId_text;
            demoInsuranceObjectData["BadAddress"] = demoInsuranceJSONData.BadAddress == false ? "No" : "Yes";
            demoInsuranceObjectData["LastSeenProvider"] = demoInsuranceJSONData.Provider;
            demoInsuranceObjectData["ReferringProvider"] = demoInsuranceJSONData.RefProvider;
            demoInsuranceObjectData["LastSeenFacility"] = demoInsuranceJSONData.Facility;
            demoInsuranceObjectData["PCP"] = demoInsuranceJSONData.PCP;
            demoInsuranceObjectData["Practice"] = demoInsuranceJSONData.Practice;
            demoInsuranceObjectData["Guarantor"] = demoInsuranceJSONData.Guarantor;
            demoInsuranceObjectData["Comments"] = demoInsuranceJSONData.Comments;
        }
        else {
            demoInsuranceObjectData["Insurance"] = demoInsuranceJSONData.Active == true ? 'Active' : 'Inactive';
            demoInsuranceObjectData["InsuranceUnassigned"] = demoInsuranceJSONData.Unassigned == true ? 'Yes' : 'No';
            demoInsuranceObjectData["InsurancePlan"] = demoInsuranceJSONData.InsurancePlan;
            demoInsuranceObjectData["AssignmentAccepted"] = demoInsuranceJSONData.AssgBenefits == true ? 'Yes' : 'No' ;
            demoInsuranceObjectData["SubscriberID"] = demoInsuranceJSONData.SubscriberID;
            demoInsuranceObjectData["PAR/NONPAR"] = demoInsuranceJSONData.PariticpantParticipentStatus_text;
            demoInsuranceObjectData["VisitCopay"] = demoInsuranceJSONData.VisitCopayment;
            demoInsuranceObjectData["SpecialistCopay"] = demoInsuranceJSONData.SpecialistCopay;
            demoInsuranceObjectData["Relation"] = demoInsuranceJSONData.Relation_text;
            demoInsuranceObjectData["SubscriberGroupID"] = demoInsuranceJSONData.SubscriberGroupID;
            demoInsuranceObjectData["LastName"] = demoInsuranceJSONData.LastName;
            demoInsuranceObjectData["PlanAddress"] = demoInsuranceJSONData.PlanAddress_text;
            demoInsuranceObjectData["FirstName"] = demoInsuranceJSONData.FirstName;
            demoInsuranceObjectData["Telephone"] = demoInsuranceJSONData.Telephone;
            demoInsuranceObjectData["M.I./Name"] = demoInsuranceJSONData.MiddleInitial;
            demoInsuranceObjectData["CoverageDateFrom"] = demoInsuranceJSONData.CoverageDateFrom;
            demoInsuranceObjectData["DateofBirth"] = demoInsuranceJSONData.DOB;
            demoInsuranceObjectData["CoverageDateTo"] = demoInsuranceJSONData.CoverageDateTo;
            demoInsuranceObjectData["Sex"] = demoInsuranceJSONData.Sex_text;
            demoInsuranceObjectData["DateSigned"] = demoInsuranceJSONData.DateSigned;
            demoInsuranceObjectData["SSN"] = demoInsuranceJSONData.SSN;
            demoInsuranceObjectData["Lawyer"] = demoInsuranceJSONData.Lawyer;
            demoInsuranceObjectData["Address1"] = demoInsuranceJSONData.Address1;
            demoInsuranceObjectData["Employer"] = demoInsuranceJSONData.Employer;
            demoInsuranceObjectData["City"] = demoInsuranceJSONData.City;
            demoInsuranceObjectData["MSPType"] = demoInsuranceJSONData.MSPType_text;
            demoInsuranceObjectData["State"] = demoInsuranceJSONData.State;
            demoInsuranceObjectData["EligibilityDate"] = demoInsuranceJSONData.EligibilityDate;
            demoInsuranceObjectData["Zip"] = demoInsuranceJSONData.Zip;
            demoInsuranceObjectData["EligibilityStatus"] = demoInsuranceJSONData.EligibilityStatus_text;
            demoInsuranceObjectData["HomeTel"] = demoInsuranceJSONData.HomeTel;
            demoInsuranceObjectData["Comments"] = demoInsuranceJSONData.Comments;
        }
        return demoInsuranceObjectData;
    },

    getPrintPDF: function ($obj, isPreview) {
        var def = $.Deferred();
        setTimeout(function () {
            $obj.find("#printcall").show();
            $obj.find("#printcall").css("display", "inline");

            kendo.drawing.drawDOM($obj.find("#printcall"), {
                landscape: false,
                scale: 0.6,
                paperSize: "A4",
                margin: {
                    left: "10mm",
                    top: "10mm",
                    right: "10mm",
                    bottom: "30mm"
                },
                template: kendo.template($('#pnlPatient_Demographic_PrintView #page-templateDemoInsurance').html())
            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    Patient_Demographic_PrintView.pdf = params["PrintPDFDataURL"];

                    if (isPreview)
                        utility.PDFViewer(params["PrintPDFDataURL"], false, 'pnlPatient_Demographic_PrintView #PreviewPrintDemoInsurance', true);

                    def.resolve();
                    $obj.find("#printcall").hide();
                });
            });
        }, 500);

        return def.promise();
    },

    UnLoad: function () {
        Patient_Demographic_PrintView.pdf = "";
        UnloadActionPan(Patient_Demographic_PrintView.params["ParentCtrl"], "actionPanPatient_Demographic_PrintView");
    },

    PrintDemoInsurance: function () {
        var params = [];
        params["PrintPDFDataURL"] = Patient_Demographic_PrintView.pdf;
        params["PreviewPdf"] = true;
        utility.documentPrint(params["PrintPDFDataURL"]);
    },
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = Patient_Demographic_PrintView.pdf;
        params["ParentCtrl"] = 'Patient_Demographic_PrintView';
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        LoadActionPan("Batch_FaxSend", params);
    }
}