Clinical_SchedulerView = {

    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_SchedulerView.params = params;

        if (Clinical_SchedulerView.params != null && Clinical_SchedulerView.params.PanelID != "pnlClinical_SchedulerView") {
            Clinical_SchedulerView.params["PanelID"] = Clinical_SchedulerView.params["PanelID"] + ' #pnlClinical_SchedulerView';
        }
        else {

            Clinical_SchedulerView.params["PanelID"] = "pnlClinical_SchedulerView"
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            //$("#pnlClinicalImmunization #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            Clinical_SchedulerView.params.patientID = $('#PatientProfile #hfPatientId').val();
        }
        if (Clinical_SchedulerView.bIsFirstLoad) {
            Clinical_SchedulerView.bIsFirstLoad = false;
            var self = $('#' + Clinical_SchedulerView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                Clinical_SchedulerView.SchedulerPreview();
                //Clinical_SchedulerView.KendoIntialization();
            });
        }
    },

    KendoIntialization: function () {
        Clinical_SchedulerView.CreateTreeMap();
        //$(document).bind("kendo:skinChange", createTreeMap);
        //$(".options").bind("change", refresh);
    },



    CreateTreeMap: function (Data) {
        var dataSource = new kendo.data.HierarchicalDataSource({
            data: Data
        });



        $("#treeMap").kendoTreeMap({
            dataSource: dataSource,
            textField: "name",
            colorField: "color",
            valueField: "value",
            classField: "class",
            type: "horizontal",
            itemCreated: function (e) {
                var DiveData = e.element.find("div").html();
                var ClassAndData = DiveData.split("_C_");
                if (ClassAndData.length == 3) {
                    e.element.find("div").html(ClassAndData[0]);
                    e.element.addClass(ClassAndData[1]);
                    if ($(e.element.find("div")).parent().css('backgroundColor') == "rgb(0, 128, 0)") {
                        e.element.find("div").css({ 'color': 'white' });
                    }
                    else {
                        e.element.find("div").css({ 'color': 'black' });
                    }

                    e.element.find("div").css('text-align', 'center');
                }
                else {
                    //e.element.find("div").css({ backgroundColor: 'white' });

                    if (ClassAndData.length == 2) {
                        if (ClassAndData[1] == "Bold") {
                            e.element.find("div").addClass("bold");
                        }
                        else {
                            e.element.find("div").css('text-align', 'center');
                        }
                        e.element.find("div").html(ClassAndData[0]);

                        if ($(e.element.find("div")).parent().css('backgroundColor') == "rgb(0, 128, 0)") {
                            e.element.find("div").css({ 'color': 'white' });
                        }
                        else {
                            e.element.find("div").css({ 'color': 'black' });
                        }



                    }
                    else {
                        if ($(e.element.find("div")).parent().css('backgroundColor') == "rgb(0, 128, 0)") {
                            e.element.find("div").css({ 'color': 'white' });
                        }
                        else {
                            e.element.find("div").css({ 'color': 'black' });
                        }

                    }

                }

            }
        });
    },

    SchedulerPreview: function () {
        Clinical_ReportHeader.ReportHeaderPrint_DbCall(-1, Clinical_SchedulerView.params.patientID, 'Immunization').done(function (response1) {
            response1 = JSON.parse(response1);
            if (response1.status != false) {
                //var contents = tinyMCE.activeEditor.getContent();
                //response1.Header + contents + response1.Footer;
                Clinical_SchedulerView.GetPreviewSchedulerData_DB_Call().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {


                        //var patientData = JSON.parse(response.SchedulerHeaderPatientData);
                        //var practiceData = JSON.parse(response.SchedulerPracticeData);

                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practicename").append(practiceData[0].ShortName);
                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practiceaddress").append(practiceData[0].Address + (practiceData[0].State != "" ? ',State ' + practiceData[0].State : '') + (practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : ""));
                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practicecity").append(practiceData[0].PhoneNo != "" ? ('Phone:' + practiceData[0].PhoneNo) : '');
                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practicefax").append(practiceData[0].Fax != "" ? "Fax:" + practiceData[0].Fax : "");

                        //var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
                        //var age = (patientData[0].DOB != "" ? "DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB) : "");

                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #PatientName").append(patientName);
                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #PatientAge").append(age);
                        //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #PatientMRN").append(patientData[0].MRNumber != "" ? "MRN:" + patientData[0].MRNumber : "");

                        $('#' + Clinical_SchedulerView.params.PanelID + " #SchedulerViewHeader").html(response1.Header);
                        $('#' + Clinical_SchedulerView.params.PanelID + " #SchedulerViewFooter").html(response1.Footer);

                        Clinical_SchedulerView.BuildJsonForTreeMap(response);


                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    BuildJsonForTreeMap: function (response) {
        var CategoryData = JSON.parse(response.CategoryData);
        var SchedulerData = JSON.parse(response.SchedulerData);

        var MainObject = new Object();
        var MainArray = [];
        var FinalArray = [];

        MainObject.name = "";
        MainObject.value = 1;

        var RowObjectForHeader = new Object();
        RowObjectForHeader.name = "";
        RowObjectForHeader.value = 1;

        var ColumnsArrayHeader = [];

        var HeaderData = ["Vaccine", "Birth", "1 month", "2 months", "4 months", "6 months", "9 months", "12 months", "15 months", "18 months", "19-23 months", "2-3 years", "4-6 years", "7-10 years", "11-12 years", "13-15 years", "16-18 years"];

        $.each(HeaderData, function (i, item) {
            if (i == 0) {
                var HeaderColumnObject = new Object();
                HeaderColumnObject.name = HeaderData[i] + "_C_Bold";
                HeaderColumnObject.value = 3;
                HeaderColumnObject.color = "white";
                ColumnsArrayHeader.push(HeaderColumnObject);
            }
            else {
                var HeaderColumnObject = new Object();
                HeaderColumnObject.name = HeaderData[i] + "_C_Bold";
                HeaderColumnObject.value = 1;
                HeaderColumnObject.color = "white";
                ColumnsArrayHeader.push(HeaderColumnObject);
            }

        });
        RowObjectForHeader.items = ColumnsArrayHeader;
        MainArray.push(RowObjectForHeader);

        $.each(CategoryData, function (i, item) {
            var RowObject = new Object();
            RowObject.name = "";
            RowObject.value = 1;
            var ColumnsArray = [];

            var VaccineColumnObject = new Object();
            VaccineColumnObject.name = item.ShortName + "_C_Bold";
            VaccineColumnObject.value = 3;
            VaccineColumnObject.color = "white";
            ColumnsArray.push(VaccineColumnObject);
            var DoesNumber = 1;
            for (var i = 0; i < 16; i++) {
                if (i == 0) {//Birth
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "Birth" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();
                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "green";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }

                }
                else if (i == 1) {//1 mo
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "1 - 2 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();
                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_TwoColumns" + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            var ColumnObject1 = new Object();
                            ColumnObject1.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject1.value = 1;
                            ColumnObject1.color = color;
                            ColumnsArray.push(ColumnObject1);
                            i = 2;
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 2) {//2 mos
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "2 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();
                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 3) {//4 mos
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "4 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 4) {//6 mos
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "6 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                        else if (item1.Schedule == "6 - 18 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_FiveColumns" + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);

                            var ColumnObject1 = new Object();
                            ColumnObject1.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject1.value = 1;
                            ColumnObject1.color = color;
                            ColumnsArray.push(ColumnObject1);

                            var ColumnObject2 = new Object();
                            ColumnObject2.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject2.value = 1;
                            ColumnObject2.color = color;
                            ColumnsArray.push(ColumnObject2);

                            var ColumnObject3 = new Object();
                            ColumnObject3.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject3.value = 1;
                            ColumnObject3.color = color;
                            ColumnsArray.push(ColumnObject3);

                            var ColumnObject4 = new Object();
                            ColumnObject4.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject4.value = 1;
                            ColumnObject5.color = color;
                            ColumnsArray.push(ColumnObject4);
                            i = 8;
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 5) {//9 mos
                    var ColumnObject = new Object();
                    ColumnObject.name = "";
                    ColumnObject.value = 1;
                    ColumnObject.color = "white";
                    ColumnsArray.push(ColumnObject);
                }
                else if (i == 6) {//12 mos
                    //12 - 15 Months  12 - 23 Months  12 - 18 Months
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "12-15 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_TwoColumns" + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);

                            var ColumnObject1 = new Object();
                            ColumnObject1.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject1.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject1.color = color;
                            ColumnsArray.push(ColumnObject1);
                            i = 7;
                            Isfind = true;
                            return;
                        }
                        else if (item1.Schedule == "12-18 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_ThreeColumns" + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);

                            var ColumnObject1 = new Object();
                            ColumnObject1.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject1.value = 1;
                            ColumnObject1.color = color;
                            ColumnsArray.push(ColumnObject1);

                            var ColumnObject2 = new Object();
                            ColumnObject2.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject2.value = 1;
                            ColumnObject2.color = color;
                            ColumnsArray.push(ColumnObject2);

                            i = 8;
                            Isfind = true;
                            return;
                        }
                        else if (item1.Schedule == "12-23 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_FourColumns" + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);

                            var ColumnObject1 = new Object();
                            ColumnObject1.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject1.value = 1;
                            ColumnObject1.color = color;
                            ColumnsArray.push(ColumnObject1);

                            var ColumnObject2 = new Object();
                            ColumnObject2.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject2.value = 1;
                            ColumnObject2.color = color;
                            ColumnsArray.push(ColumnObject2);

                            var ColumnObject3 = new Object();
                            ColumnObject3.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject3.value = 1;
                            ColumnObject3.color = color;
                            ColumnsArray.push(ColumnObject3);

                            i = 9;
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 7) {//15 mos
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "15-18 Months" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_TwoColumns" + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);

                            var ColumnObject1 = new Object();
                            ColumnObject1.name = "_C_ZeroColumn" + "_C_Center";
                            ColumnObject1.value = 1;
                            ColumnObject1.color = color;
                            ColumnsArray.push(ColumnObject1);
                            i = 8;
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 8) {//18 mos
                    var ColumnObject = new Object();
                    ColumnObject.name = "";
                    ColumnObject.value = 1;
                    ColumnObject.color = "white";
                    ColumnsArray.push(ColumnObject);

                }

                else if (i == 9) {//19-23 mos
                    var ColumnObject = new Object();
                    ColumnObject.name = "";
                    ColumnObject.value = 1;
                    ColumnObject.color = "white";
                    ColumnsArray.push(ColumnObject);

                }
                else if (i == 10) {//2-3 yrs
                    var ColumnObject = new Object();
                    ColumnObject.name = "";
                    ColumnObject.value = 1;
                    ColumnObject.color = "white";
                    ColumnsArray.push(ColumnObject);

                }
                else if (i == 11) {//4-6 yrs
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "4-6 Years" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 12) {//7-10 yrs

                    var ColumnObject = new Object();
                    ColumnObject.name = "";
                    ColumnObject.value = 1;
                    ColumnObject.color = "white";
                    ColumnsArray.push(ColumnObject);

                }
                else if (i == 13) {//11-12 yrs
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "11 Years" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();

                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                        else if (item1.Schedule == "11-12 Years" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();
                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
                else if (i == 14) {//13-15 yrs
                    var ColumnObject = new Object();
                    ColumnObject.name = "";
                    ColumnObject.value = 1;
                    ColumnObject.color = "white";
                    ColumnsArray.push(ColumnObject);
                }
                else if (i == 15) {//16-18 yrs
                    var Isfind = false;
                    $.grep(SchedulerData, function (item1, index) {
                        if (item1.Schedule == "16 Years" && item1.Category == item.VaccineGroupID) {
                            var ColumnObject = new Object();
                            ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center";
                            DoesNumber++;
                            ColumnObject.value = 1;
                            var color = "yellow";
                            if (item1.Type == "ADMINISTER") {
                                color = "green";
                            }
                            else if (item1.Type == "DOCUMENTHX") {
                                color = "yellow";
                            }
                            else if (item1.Type == "REFUSAL") {
                                color = "Silver";
                            }
                            ColumnObject.color = color;
                            ColumnsArray.push(ColumnObject);
                            Isfind = true;
                            return;
                        }
                    });
                    if (!Isfind) {
                        var ColumnObject = new Object();
                        ColumnObject.name = "";
                        ColumnObject.value = 1;
                        ColumnObject.color = "white";
                        ColumnsArray.push(ColumnObject);
                    }
                }
            }
            RowObject.items = ColumnsArray
            MainArray.push(RowObject);
        });
        MainObject.items = MainArray;
        FinalArray.push(MainObject);
        Clinical_SchedulerView.CreateTreeMap(FinalArray);
    },




    GetPatientAge: function (PatientAge) {
        var returnAge = "";
        PatientAge = PatientAge.split(",");
        if (PatientAge.length == 3) {
            if (parseInt(PatientAge[0]) > 0) {


                returnAge += parseInt(PatientAge[0]) + " Y ";

                //if (parseInt(PatientAge[0]) == 1) {
                //    returnAge += parseInt(PatientAge[0]) + " Y, ";
                //    return parseInt(PatientAge[0]) + " Year";

                //}
                //else {
                //    returnAge += parseInt(PatientAge[0]) + " Y, ";
                //    return parseInt(PatientAge[0]) + " Years";
                //}
            }
            if (parseInt(PatientAge[1]) > 0) {
                returnAge += parseInt(PatientAge[1]) + " M ";
                //if (parseInt(PatientAge[1]) == 1) {
                //    return parseInt(PatientAge[1]) + " Month";
                //}
                //else {
                //    return parseInt(PatientAge[1]) + " Months";
                //}
            }
            if (parseInt(PatientAge[2]) > 0) {
                returnAge += parseInt(PatientAge[2]) + " D";

                //if (parseInt(PatientAge[2]) == 1) {
                //    return parseInt(PatientAge[2]) + " Day";
                //}
                //else {
                //    return parseInt(PatientAge[2]) + " Days";
                //}
            }
            return returnAge;
        }

    },
    GetDoes: function (Dose) {
        if (Dose == 1) {
            return "1st dose";
        }
        else if (Dose == 2) {
            return "2nd dose";
        }
        else if (Dose == 3) {
            return "3rd dose";
        }
        else if (Dose == 4) {
            return "4th dose";
        }
        else if (Dose == 5) {
            return "5th dose";
        }
        else if (Dose == 6) {
            return "6th dose";
        }
        else if (Dose == 7) {
            return "7th dose";
        }
        else if (Dose == 8) {
            return "8th dose";
        }
        else if (Dose == 9) {
            return "9th dose";
        }
        else if (Dose == 7) {
            return "10th dose";
        }
        else if (Dose == 11) {
            return "11th dose";
        }
        else if (Dose == 12) {
            return "12th dose";
        }
        else if (Dose == 13) {
            return "13th dose";
        }
        else if (Dose == 14) {
            return "14th dose";
        }
        else if (Dose == 15) {
            return "15th dose";
        }
        else if (Dose == 16) {
            return "16th dose";
        }
    },
    GetPreviewSchedulerData_DB_Call: function () {
        var objData = {};

        objData["PatientId"] = Clinical_SchedulerView.params.PatientId;
        objData["commandType"] = "GetPreviewSchedulerData";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    //*..............Dont Change Comment Code Use In future For Add or Edit Schedule from this chart.......... *
    //BY:M Ahmad Imran

    //CreateTreeMap: function (Data) {
    //    var dfd = $.Deferred();
    //    var dataSource = new kendo.data.HierarchicalDataSource({
    //        data: Data
    //    });



    //    $("#treeMap").kendoTreeMap({
    //        dataSource: dataSource,
    //        textField: "name",
    //        colorField: "color",
    //        valueField: "value",
    //        classField: "class",
    //        type: "horizontal",
    //        itemCreated: function (e) {
    //            var DiveData = e.element.find("div").html();
    //            var OnclickFunction = DiveData.split("_OnClick_");
    //            if (OnclickFunction.length == 2) {
    //                if (OnclickFunction[1] != "") {
    //                    var clickFunction = OnclickFunction[1];
    //                    $(e.element).attr("onclick", clickFunction);
    //                }


    //                var ClassAndData = OnclickFunction[0].split("_C_");
    //                if (ClassAndData.length == 3) {
    //                    e.element.find("div").html(ClassAndData[0]);
    //                    e.element.addClass(ClassAndData[1]);
    //                    if ($(e.element.find("div")).parent().css('backgroundColor') == "rgb(0, 128, 0)") {
    //                        e.element.find("div").css({ 'color': 'white' });
    //                    }
    //                    else {
    //                        e.element.find("div").css({ 'color': 'black' });
    //                    }

    //                    e.element.find("div").css('text-align', 'center');
    //                }
    //                else {
    //                    //e.element.find("div").css({ backgroundColor: 'white' });

    //                    if (ClassAndData.length == 2) {
    //                        if (ClassAndData[1] == "Bold") {
    //                            e.element.find("div").addClass("bold");
    //                        }
    //                        else {
    //                            e.element.find("div").css('text-align', 'center');
    //                        }
    //                        e.element.find("div").html(ClassAndData[0]);

    //                        if ($(e.element.find("div")).parent().css('backgroundColor') == "rgb(0, 128, 0)") {
    //                            e.element.find("div").css({ 'color': 'white' });
    //                        }
    //                        else {
    //                            e.element.find("div").css({ 'color': 'black' });
    //                        }



    //                    }
    //                    else {
    //                        if ($(e.element.find("div")).parent().css('backgroundColor') == "rgb(0, 128, 0)") {
    //                            e.element.find("div").css({ 'color': 'white' });
    //                        }
    //                        else {
    //                            e.element.find("div").css({ 'color': 'black' });
    //                        }

    //                    }

    //                }
    //            }

    //        }
    //    });
    //    dfd.resolve();
    //    return dfd;
    //},

    //SchedulerPreview: function () {
    //    var dfd = $.Deferred();
    //    Clinical_ReportHeader.ReportHeaderPrint_DbCall(-1, Clinical_SchedulerView.params.patientID).done(function (response1) {
    //        response1 = JSON.parse(response1);
    //        if (response1.status != false) {
    //            //var contents = tinyMCE.activeEditor.getContent();
    //            //response1.Header + contents + response1.Footer;
    //            Clinical_SchedulerView.GetPreviewSchedulerData_DB_Call().done(function (response) {
    //                response = JSON.parse(response);
    //                if (response.status != false) {


    //                    //var patientData = JSON.parse(response.SchedulerHeaderPatientData);
    //                    //var practiceData = JSON.parse(response.SchedulerPracticeData);

    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practicename").append(practiceData[0].ShortName);
    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practiceaddress").append(practiceData[0].Address + (practiceData[0].State != "" ? ',State ' + practiceData[0].State : '') + (practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : ""));
    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practicecity").append(practiceData[0].PhoneNo != "" ? ('Phone:' + practiceData[0].PhoneNo) : '');
    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #Practicefax").append(practiceData[0].Fax != "" ? "Fax:" + practiceData[0].Fax : "");

    //                    //var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
    //                    //var age = (patientData[0].DOB != "" ? "DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB) : "");

    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #PatientName").append(patientName);
    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #PatientAge").append(age);
    //                    //$('#' + Clinical_SchedulerView.params.PanelID + " #frmClinical_SchedulerView #PatientMRN").append(patientData[0].MRNumber != "" ? "MRN:" + patientData[0].MRNumber : "");

    //                    $('#' + Clinical_SchedulerView.params.PanelID + " #SchedulerViewHeader").html(response1.Header);
    //                    $('#' + Clinical_SchedulerView.params.PanelID + " #SchedulerViewFooter").html(response1.Footer);

    //                    $.when(Clinical_SchedulerView.BuildJsonForTreeMap(response)).then(function () {

    //                        dfd.resolve();
    //                    });


    //                }
    //                else {
    //                    utility.DisplayMessages(response.Message, 3);
    //                }
    //            });
    //        }
    //    });

    //    return dfd;
    //},

    //BuildJsonForTreeMap: function (response) {
    //    var dfd = $.Deferred();
    //    var CategoryData = JSON.parse(response.CategoryData);
    //    var SchedulerData = JSON.parse(response.SchedulerData);

    //    var MainObject = new Object();
    //    var MainArray = [];
    //    var FinalArray = [];

    //    MainObject.name = "";
    //    MainObject.value = 1;

    //    var RowObjectForHeader = new Object();
    //    RowObjectForHeader.name = "";
    //    RowObjectForHeader.value = 1;

    //    var ColumnsArrayHeader = [];

    //    var HeaderData = ["Vaccine", "Birth", "1 month", "2 months", "4 months", "6 months", "9 months", "12 months", "15 months", "18 months", "19-23 months", "2-3 years", "4-6 years", "7-10 years", "11-12 years", "13-15 years", "16-18 years"];

    //    $.each(HeaderData, function (i, item) {
    //        if (i == 0) {
    //            var HeaderColumnObject = new Object();
    //            HeaderColumnObject.name = HeaderData[i] + "_C_Bold_OnClick_";
    //            HeaderColumnObject.value = 3;
    //            HeaderColumnObject.color = "white";
    //            ColumnsArrayHeader.push(HeaderColumnObject);
    //        }
    //        else {
    //            var HeaderColumnObject = new Object();
    //            HeaderColumnObject.name = HeaderData[i] + "_C_Bold_OnClick_";
    //            HeaderColumnObject.value = 1;
    //            HeaderColumnObject.color = "white";
    //            ColumnsArrayHeader.push(HeaderColumnObject);
    //        }

    //    });
    //    RowObjectForHeader.items = ColumnsArrayHeader;
    //    MainArray.push(RowObjectForHeader);

    //    var def = [];

    //    $.each(CategoryData, function (i, item) {
    //        var RowObject = new Object();
    //        RowObject.name = "";
    //        RowObject.value = 1;
    //        var ColumnsArray = [];

    //        var VaccineColumnObject = new Object();
    //        VaccineColumnObject.name = item.ShortName + "_C_Bold_OnClick_";
    //        VaccineColumnObject.value = 3;
    //        VaccineColumnObject.color = "white";
    //        ColumnsArray.push(VaccineColumnObject);
    //        var DoesNumber = 1;
    //        for (var i = 0; i < 16; i++) {
    //            if (i == 0) {//Birth
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "Birth" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();

    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";

    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "green";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;




    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                        $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "Birth")).then(function () {

    //                            var OnclickFunction = "";
    //                            if (result.response != 0) {
    //                                var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                                OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                            }

    //                            var ColumnObject = new Object();
    //                            ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                            ColumnObject.value = 1;
    //                            ColumnObject.color = "white";
    //                            ColumnsArray.push(ColumnObject);
    //                        })
    //                    )

    //                }

    //            }
    //            else if (i == 1) {//1 mo
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "1 - 2 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";

    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_TwoColumns" + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        var ColumnObject1 = new Object();
    //                        ColumnObject1.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject1.value = 1;
    //                        ColumnObject1.color = color;
    //                        ColumnsArray.push(ColumnObject1);
    //                        i = 2;
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "1 - 2 Months")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 2) {//2 mos
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "2 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";

    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "2 Months")).then(function () {
    //                        var ColumnObject = new Object();

    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 3) {//4 mos
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "4 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "4 Months")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 4) {//6 mos
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "6 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                    else if (item1.Schedule == "6 - 18 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_FiveColumns" + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);

    //                        var ColumnObject1 = new Object();
    //                        ColumnObject1.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject1.value = 1;
    //                        ColumnObject1.color = color;
    //                        ColumnsArray.push(ColumnObject1);

    //                        var ColumnObject2 = new Object();
    //                        ColumnObject2.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject2.value = 1;
    //                        ColumnObject2.color = color;
    //                        ColumnsArray.push(ColumnObject2);

    //                        var ColumnObject3 = new Object();
    //                        ColumnObject3.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject3.value = 1;
    //                        ColumnObject3.color = color;
    //                        ColumnsArray.push(ColumnObject3);

    //                        var ColumnObject4 = new Object();
    //                        ColumnObject4.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject4.value = 1;
    //                        ColumnObject4.color = color;
    //                        ColumnsArray.push(ColumnObject4);
    //                        i = 8;
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "6 Months")).then(function () {
    //                        var ColumnObject = new Object();

    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })

    //                    )
    //                }
    //            }
    //            else if (i == 5) {//9 mos
    //                def.push(
    //                $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "6-18 Months")).then(function () {
    //                    var ColumnObject = new Object();
    //                    var OnclickFunction = "";
    //                    if (result.response != 0) {
    //                        var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                        OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                    }
    //                    ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                    ColumnObject.value = 1;
    //                    ColumnObject.color = "white";
    //                    ColumnsArray.push(ColumnObject);
    //                })
    //                )
    //            }
    //            else if (i == 6) {//12 mos
    //                //12 - 15 Months  12 - 23 Months  12 - 18 Months
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "12-15 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_TwoColumns" + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);

    //                        var ColumnObject1 = new Object();
    //                        ColumnObject1.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject1.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject1.color = color;
    //                        ColumnsArray.push(ColumnObject1);
    //                        i = 7;
    //                        Isfind = true;
    //                        return;
    //                    }
    //                    else if (item1.Schedule == "12-18 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_ThreeColumns" + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);

    //                        var ColumnObject1 = new Object();
    //                        ColumnObject1.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject1.value = 1;
    //                        ColumnObject1.color = color;
    //                        ColumnsArray.push(ColumnObject1);

    //                        var ColumnObject2 = new Object();
    //                        ColumnObject2.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject2.value = 1;
    //                        ColumnObject2.color = color;
    //                        ColumnsArray.push(ColumnObject2);

    //                        i = 8;
    //                        Isfind = true;
    //                        return;
    //                    }
    //                    else if (item1.Schedule == "12-23 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_FourColumns" + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);

    //                        var ColumnObject1 = new Object();
    //                        ColumnObject1.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject1.value = 1;
    //                        ColumnObject1.color = color;
    //                        ColumnsArray.push(ColumnObject1);

    //                        var ColumnObject2 = new Object();
    //                        ColumnObject2.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject2.value = 1;
    //                        ColumnObject2.color = color;
    //                        ColumnsArray.push(ColumnObject2);

    //                        var ColumnObject3 = new Object();
    //                        ColumnObject3.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject3.value = 1;
    //                        ColumnObject3.color = color;
    //                        ColumnsArray.push(ColumnObject3);

    //                        i = 9;
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "12-18 Months")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 7) {//15 mos
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "15-18 Months" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_TwoColumns" + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);

    //                        var ColumnObject1 = new Object();
    //                        ColumnObject1.name = "_C_ZeroColumn" + "_C_Center" + "_OnClick_";
    //                        ColumnObject1.value = 1;
    //                        ColumnObject1.color = color;
    //                        ColumnsArray.push(ColumnObject1);
    //                        i = 8;
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "15-18 Months")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 8) {//18 mos
    //                def.push(
    //                $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "15-18 Months")).then(function () {
    //                    var ColumnObject = new Object();
    //                    var OnclickFunction = "";
    //                    if (result.response != 0) {
    //                        var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                        OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                    }
    //                    ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                    ColumnObject.value = 1;
    //                    ColumnObject.color = "white";
    //                    ColumnsArray.push(ColumnObject);
    //                })
    //                )

    //            }

    //            else if (i == 9) {//19-23 mos
    //                def.push(
    //                $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "12-23 Months")).then(function () {
    //                    var ColumnObject = new Object();
    //                    var OnclickFunction = "";
    //                    if (result.response != 0) {
    //                        var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                        OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                    }
    //                    ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                    ColumnObject.value = 1;
    //                    ColumnObject.color = "white";
    //                    ColumnsArray.push(ColumnObject);
    //                })
    //                )

    //            }
    //            else if (i == 10) {//2-3 yrs

    //                def.push(
    //                $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "2-3 Years")).then(function () {
    //                    var ColumnObject = new Object();
    //                    var OnclickFunction = "";
    //                    if (result.response != 0) {
    //                        var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                        OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                    }
    //                    ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                    ColumnObject.value = 1;
    //                    ColumnObject.color = "white";
    //                    ColumnsArray.push(ColumnObject);
    //                })
    //                )

    //            }
    //            else if (i == 11) {//4-6 yrs
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "4-6 Years" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {

    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "4-6 Years")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 12) {//7-10 yrs
    //                def.push(
    //                $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "7-10 Years")).then(function () {
    //                    var ColumnObject = new Object();
    //                    var OnclickFunction = "";
    //                    if (result.response != 0) {
    //                        var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                        OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                    }
    //                    ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                    ColumnObject.value = 1;
    //                    ColumnObject.color = "white";
    //                    ColumnsArray.push(ColumnObject);
    //                })
    //                )

    //            }
    //            else if (i == 13) {//11-12 yrs
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "11 Years" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                    else if (item1.Schedule == "11-12 Years" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "11-12 Years")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //            else if (i == 14) {//13-15 yrs

    //                def.push(
    //                $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "13-15 Years")).then(function () {
    //                    var ColumnObject = new Object();
    //                    var OnclickFunction = "";
    //                    if (result.response != 0) {
    //                        var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                        OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                    }
    //                    ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                    ColumnObject.value = 1;
    //                    ColumnObject.color = "white";
    //                    ColumnsArray.push(ColumnObject);
    //                })
    //                )
    //            }
    //            else if (i == 15) {//16-18 yrs
    //                var Isfind = false;
    //                $.grep(SchedulerData, function (item1, index) {
    //                    if (item1.Schedule == "16 Years" && item1.Category == item.VaccineGroupID) {
    //                        var ColumnObject = new Object();
    //                        var AddParameters = "'Edit','" + item1.Category + "'," + item1.VaccineScheduleId + ",'Chart'," + item1.VaccineHxId + ",'" + item1.Type + "'";
    //                        var OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        ColumnObject.name = Clinical_SchedulerView.GetDoes(DoesNumber) + (item1.AdministrationDate != "" ? "<br>" + item1.AdministrationDate + " " + item1.AdministrationTime : "") + (item1.GivenBy != "" ? "<br>" + item1.GivenBy : "") + (Clinical_SchedulerView.GetPatientAge(item1.PatientAge) != "" ? "<br>" + Clinical_SchedulerView.GetPatientAge(item1.PatientAge) : "") + "_C_Center" + "_OnClick_" + OnclickFunction;
    //                        DoesNumber++;
    //                        ColumnObject.value = 1;
    //                        var color = "yellow";
    //                        if (item1.Type == "ADMINISTER") {
    //                            color = "green";
    //                        }
    //                        else if (item1.Type == "DOCUMENTHX") {
    //                            color = "yellow";
    //                        }
    //                        else if (item1.Type == "REFUSAL") {
    //                            color = "Silver";
    //                        }
    //                        ColumnObject.color = color;
    //                        ColumnsArray.push(ColumnObject);
    //                        Isfind = true;
    //                        return;
    //                    }
    //                });
    //                if (!Isfind) {
    //                    def.push(
    //                    $.when(result = Clinical_SchedulerView.GetVaccineSchedulerId(item.VaccineGroupID, "16 Years")).then(function () {
    //                        var ColumnObject = new Object();
    //                        var OnclickFunction = "";
    //                        if (result.response != 0) {
    //                            var AddParameters = "'Add','" + item.VaccineGroupID + "'," + result.response + ",'Chart'";
    //                            OnclickFunction = "Clinical_SchedulerView.ClickOnAddButtonFromSchedulerChart(" + AddParameters + ")";
    //                        }
    //                        ColumnObject.name = "_C_" + "_OnClick_" + OnclickFunction;
    //                        ColumnObject.value = 1;
    //                        ColumnObject.color = "white";
    //                        ColumnsArray.push(ColumnObject);
    //                    })
    //                    )
    //                }
    //            }
    //        }
    //        RowObject.items = ColumnsArray
    //        MainArray.push(RowObject);
    //    });
    //    $.when.apply($, def).done(function ($n) {
    //        MainObject.items = MainArray;
    //        FinalArray.push(MainObject);
    //        $.when(Clinical_SchedulerView.CreateTreeMap(FinalArray)).then(function () {
    //            dfd.resolve();

    //        });
    //    });
    //    return dfd;
    //},
    //ClickOnAddButtonFromSchedulerChart: function (Mode, VaccineCategoryId, VaccineScheduleId, TabId, VaccineHxId, Type) {


    //    var MODE = "ADD";
    //    if (Mode == "Edit") {
    //        MODE = "EDIT";
    //    }
    //    var strMessage = "";
    //    AppPrivileges.GetFormPrivileges("Medical_Immunization", MODE, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //        if (strMessage == "") {
    //            var params = [];

    //            var PanelID = "";


    //            params["ParentCtrl"] = 'Clinical_SchedulerView';
    //            PanelID = Clinical_SchedulerView.params.PanelID;
    //            params["from"] = 'Clinical_SchedulerView';
    //            params["PanelID"] = Clinical_SchedulerView.params.PanelID;
    //            params["ParentPanelID"] = Clinical_SchedulerView.params.PanelID;


    //            params["FromAdmin"] = 0;
    //            params["VaccineScheduleId"] = VaccineScheduleId;
    //            params["CategoryId"] = VaccineCategoryId;

    //            if (Mode == "Edit") {
    //                params["VaccineHxId"] = VaccineHxId;
    //                params["Type"] = Type;
    //            }

    //            params["TabId"] = TabId;
    //            params["mode"] = Mode;
    //            params["patientID"] = Clinical_SchedulerView.params["patientID"];
    //            LoadActionPan('Clinical_ImmunizationDetail', params, PanelID);
    //        } else
    //            utility.DisplayMessages(strMessage, 2);
    //    });
    //},

    //GetVaccineSchedulerId: function (categoryId, scheduleShortName) {

    //    var dfd1 = $.Deferred();
    //    var response = "";
    //    Clinical_SchedulerView.GetVaccineSchedulerId_DB_CALL(categoryId, scheduleShortName).done(function (response) {
    //        response = JSON.parse(response);
    //        if (response.status != false) {
    //            dfd1.response = response.VaccineSchedulerId;
    //            dfd1.resolve();
    //        }
    //        else {
    //            dfd1.resolve();
    //            utility.DisplayMessages(response.message, 3);
    //        }
    //    });
    //    return dfd1;
    //},
    //GetVaccineSchedulerId_DB_CALL: function (categoryId, scheduleShortName) {
    //    var objData = new Object();
    //    objData["CategoryID"] = categoryId;
    //    objData["ScheduleShortName"] = scheduleShortName;
    //    objData["commandType"] = "Get_VaccineSchedulerId";
    //    var data = JSON.stringify(objData);
    //    return Clinical_SchedulerView.APIServiceCopy(data, "MEDICAL", "Immunization");
    //},
    ////beacuse of sync call
    //APIServiceCopy: function (inputData, ModuleName, SubModule) {

    //    return $.ajax({
    //        type: "POST",
    //        url: "api/" + ModuleName + "/" + SubModule,
    //        data: { data: inputData },
    //        //contentType: "application/json",
    //        dataType: "json",
    //        cache: "false",
    //        async: false,
    //        rp: 10,
    //        beforeSend: function () {

    //            BackgroundLoaderShow(true);
    //        },
    //        success: function (result) {
    //            sess_Reset(globalAppdata["SessionTimout"]);

    //            BackgroundLoaderShow(false);
    //            if (result.Redirect != undefined && result.Redirect != "undefined") {
    //            }



    //        },
    //    }).promise();

    //},

    //*..............Dont Change Comment Code Use In future For Add or Edit Schedule from this chart.......... *
    //BY:M Ahmad Imran

    UnLoad: function () {

        if (Clinical_SchedulerView.params != null && Clinical_SchedulerView.params.ParentCtrl) {

            if (Clinical_SchedulerView.params.ParentCtrl == 'clinicalTabImmunization') {
                UnloadActionPan(Clinical_SchedulerView.params["ParentCtrl"], "Clinical_SchedulerView");
            } else {
                Clinical_SchedulerView.params.PanelID = Clinical_SchedulerView.params.PanelID.replace(" #pnlClinical_SchedulerView", "");
                UnloadActionPan(Clinical_SchedulerView.params.ParentCtrl, 'Clinical_SchedulerView', null, Clinical_SchedulerView.params.PanelID);
            }

        }
        else {
            UnloadActionPan();
        }
    }

}