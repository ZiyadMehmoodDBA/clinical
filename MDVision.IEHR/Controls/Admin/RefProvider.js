refprovider = {
    Params: "",
    //Index of ProviderShortName in grid [RefProvider_SearchResult] datakeys
    ProviderShortNameIndex: 1,
    SelectedRefProIns: 0,
    Insaction: "",
    Load: function (params) {
        setZIndex('refprovider');
        setFocus();
        if (GetCurrentSelectedTab().TabID == "patTab_NewPatient") {
            $("#refprovider #fadeoutPatch1_refprovider").removeClass("fadeoutPatch1").addClass("fadeoutPatch00");
        } else {
            $("#refprovider #fadeoutPatch1_refprovider").removeClass("fadeoutPatch00").addClass("fadeoutPatch1");
        }
        $("#refprovider #RefProvider_Detail .Validator").hide();
        refprovider.Params = params;
        var self = $('#refprovider #RefProvider_Detail');
        self.loadDropDowns(true);
        $("#RefProvider_Detail #ddlQualifier").val("1G");
        //! Params.Sender is passed from Patient_AddReferringProvider to make
        //! this control behave differently {as required in caller.}

        //! if (refprovider.Params.Sender) added by zubair
        if (refprovider.Params) {

            
            if (refprovider.Params.Sender == "patient_demographic") {
                if (refprovider.Params.IsrefproExist) {
                    $("#refprovider #RefProvider_Detail").css("display", "");
                    $("#refprovider #RefProvider_Search").css("display", "none");
                    refprovider.Click_RefProvider_lnkDetail(refprovider.Params.Seq_Num);
                    $("#REFPRO_SELECTEDDATAKEYS").val(refprovider.Params.Seq_Num);
                }
            }
            else if (refprovider.Params.Sender == "dashboard_patient_checkin" || refprovider.Params.Sender == "dashboard_startprocess" || refprovider.Params.Sender == "patient_walkincheckin" || refprovider.Params.Sender == "dashboard_patient_checkout") {
                if (refprovider.Params.IsrefproExist) {
                    $("#refprovider #RefProvider_Detail").css("display", "");
                    $("#refprovider #RefProvider_Search").css("display", "none");
                    refprovider.Click_RefProvider_lnkDetail(refprovider.Params.Seq_Num);
                    $("#REFPRO_SELECTEDDATAKEYS").val(refprovider.Params.Seq_Num);
                }
            }
            else if (refprovider.Params.Sender == patient_referring_provider_detail.Name)
            {
                // refprovider.Click_RefProvider_lnkDetail(refprovider.Params.Seq_Num);
                //$("#REFPRO_SELECTEDDATAKEYS").val(refprovider.Params.Seq_Num)
                //do nothing
            }
            else if (refprovider.Params.Sender == patient_addreferringprovider.Name)
            {
                //do nothing
            }
        }
        else {
            //! will set pCaller property in .html
            $('#refprovider').bindMyJSON(true, { "parentcontrol": patient_addreferringprovider.Name });
        }
        //zubair
        //Might not be needful to add this line here but for safeside used it here as well
        //to cover the scenario where on Load you use Referring Provider Detail Window
        refprovider.ShowHideInsuranceList();

    },
    Click_RefProvider_lnkAddNew: function () {
        $("#refprovider #RefProvider_Detail .Validator").hide();
        $("#refprovider #RefProvider_Detail").css("display", "");
        $("#refprovider #RefProvider_Search").css("display", "none");
        $('#refprovider #RefProvider_lnkDelete').hide();
        $('#refprovider #RefProvider_Detail').find('input:text').val('');
        $("#refprovider #REFPRO_SELECTEDDATAKEYS").val('');

        //zubair
        $("#RefProvidersInsSearchGrid_emrgrid").html("");
        refprovider.ShowHideInsuranceList();
    },
    Click_RefProvider_lnkDetail: function (SEQ_NUM) {
        $("#refprovider #RefProvider_Detail .Validator").hide();
        var self = $('#refprovider #RefProvider_Detail');
        dataservice_patient.getSelectedRefproInfo(SEQ_NUM).done(function (json) {
            self.bindMyJSON(true, json);
            $("#refprovider #RefProvider_Detail").css("display", "");
            $("#refprovider #RefProvider_Search").css("display", "none");
            $('#refprovider #RefProvider_lnkDelete').show();
            refprovider.Click_lnkInsurance_GetRefProIns(SEQ_NUM);
            //zubair
            refprovider.ShowHideInsuranceList();
        })
    },
    Click_RefProvider_btnSearch: function () {
        $("#refprovider #RefProvider_SearchResult").css("display", "");
        var queryData = "test=test;txtSearch_UPIN=" + $('#txtSearch_UPIN').val() + ";txtSearch_Specialty=" + $('#RefProvider_Search #txtSearch_Specialty').val() + ";txtSearch_LastName=" + $('#RefProvider_Search #txtSearch_LastName').val() + ";txtSearch_City=" + $('#RefProvider_Search #txtSearch_City').val() + ";txtSearch_NPI=" + $('#RefProvider_Search #txtSearch_NPI').val() + ";txtSearch_FirstName=" + $('#RefProvider_Search #txtSearch_FirstName').val() + ";txtSearch_Zip=" + $('#RefProvider_Search #txtSearch_Zip').val() + ";txtSearch_TaxonomyCode=" + $('#RefProvider_Search #txtSearch_TaxonomyCode').val() + ";txt_RefProvider=" + $('#RefProvider_Search #txt_RefProvider').val();
        $("#refprovider #RefProvidersSearchGrid").emrgrid({
            url: "default.ashx?class=REFPROVIDER&action=SEARCH_REF_PROVIDER",
            dataType: 'json',
            colModel: [

                { display: 'Ref. Provider', name: 'SHORT_NAME', width: '15%', sortable: true, align: 'left', divhtml: 'REF_PRO_NAME_LINK', divfields: 'SHORT_NAME,SEQ_NUM' },
                { display: 'Name', name: 'FIRST_NAME', width: '15%', sortable: true, align: 'left', divhtml: 'REF_PRO_FLNAME_LINK', divfields: 'FIRST_NAME,LAST_NAME' },
                { display: 'Specialty', name: 'SPECIALITY_SHORT_NAME', width: '10%', sortable: true, align: 'left' },
                { display: 'UPIN', name: 'UPIN_NUM', width: '10%', sortable: true, align: 'left' },
                { display: 'Address', name: 'ADDRESS1', width: '10%', sortable: true, align: 'left' },
                { display: 'City', name: 'CITY', width: '10%', sortable: true, align: 'left' },
                { display: 'State', name: 'STATE', width: '10%', sortable: true, align: 'left' },
                { display: 'Zip', name: 'ZIPCODE', width: '10%', sortable: true, align: 'left' },
                { display: 'Taxonomy Code', name: 'TAXONOMY_CODE', width: '10%', sortable: true, align: 'left', replacevalues: 'M|Male,F|Female' },
                { display: 'NPI', name: 'NPI', width: '10%', sortable: true, align: 'left', replacevalues: 'Y|Yes,N|No' }

            ],
            sortname: "SHORT_NAME",
            sortorder: "desc",
            usepager: true,
            resizable: false,
            colResize: false,
            singleSelect: true,
            onSubmit: false,
            rp: 15,
            width: '100%',
            height: 258,
            cachedata: true,
            widthmode: 'auto',
            query: queryData,
            //? SHORT_NAME can have comma {i.e Zubair,A} inside. 
            //? if you add another datakey. put your key before SHORT_NAME and reset shortName [this.ProviderShortNameIndex] index accordingly
            datakeys: 'SEQ_NUM,SHORT_NAME',
            dataselectioncontrol: 'REFPRO_SELECTEDDATAKEYS'
        })
    },
    Click_RefProvider_btnSave: function () {
        var self = $('#refprovider #DivReffProDet');
        if (ControlValidator.IsUIValidated(self)) {
            var myJSON = self.getMyJSON();
            var SEQ_NUM;
            if ($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0]) {
                SEQ_NUM = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0];
            } else if (refprovider.Params.IsrefproExist) {
                SEQ_NUM = refprovider.Params.Seq_Num;
            }
            else {
                SEQ_NUM = "-1";
            }

            dataservice_patient.saveRefpro(SEQ_NUM, myJSON).done(function (json) {
                if (json["status"] == true) {
                    utility.DisplayMessage({
                        'message': json["Message"],
                        'errorType': '1'
                    });
                    if (refprovider.Params.IsrefproExist) {
                        //UnloadActionPan();
                        //zubair
                        //refprovider.ShowHideInsuranceList();
                        var value = json["SeqNum"] + "," + json["ShortName"];
                        $("#refprovider #REFPRO_SELECTEDDATAKEYS").val(value);
                        refprovider.Click_RefProvider_lnkDetail(json["SeqNum"]);
                    }
                    else {
                        //$("#refprovider #RefProvider_Detail").css("display", "none");
                        //$("#refprovider #RefProvider_Search").css("display", "");
                        //refprovider.Click_RefProvider_btnSearch();
                        //zubair
                        //refprovider.ShowHideInsuranceList();
                        var value = json["SeqNum"] + "," + json["ShortName"];
                        $("#refprovider #REFPRO_SELECTEDDATAKEYS").val(value);
                        refprovider.Click_RefProvider_lnkDetail(json["SeqNum"]);


                    }
                }
                else {
                    utility.DisplayMessage({
                        'message': json["Message"],
                        'errorType': '2'
                    });
                }
            })
        }
    },
    Click_RefProvider_btnReset: function () {
        $("#refprovider #RefProvider_Detail .Validator").hide();
        var str;
        var DataKeys = new Array();
        str = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val();
        DataKeys = (str.split(","));
        var self = $('#refprovider #RefProvider_Detail');
        dataservice_patient.getSelectedRefproInfo(DataKeys[0]).done(function (json) {
            self.bindMyJSON(true, json);
        })
    },
    Click_RefProvider_btnCancel: function () {
        refprovider.Click_RefProvider_btnSearch()
        if (refprovider.Params.IsrefproExist) {
            UnloadActionPan();
        }
        else {
            $("#refprovider #RefProvider_Detail").css("display", "none");
            $("#refprovider #RefProvider_Search").css("display", "");
            $('#refprovider #RefProvider_Detail').find('input:text').val('');
        }

    },
    Click_RefProvider_lnkSelect: function () {
        var result = $('#refprovider').getMyJSON();
        if (result)
            var json = JSON.parse(result);

        //if (json && json.parentcontrol == patient_addreferringprovider.Name) {
        //    var selectedRefProvAttribs = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',');
        //    if (selectedRefProvAttribs && selectedRefProvAttribs.length > 1) {

        //        var refProvSeqNum = selectedRefProvAttribs[0];
        //        var refProvShortName = selectedRefProvAttribs[this.ProviderShortNameIndex];

        //        patient_addreferringprovider.UpdatePatientRefProviderUI(refProvSeqNum, refProvShortName);
        //    }
        //    else {
        //        utility.DisplayMessage({
        //            'message': 'Please select provider.',
        //            'errorType': '1'
        //        });
        //        return;
        //    }
            //}




        if (refprovider.Params && refprovider.Params.Sender == patient_addreferringprovider.Name)
        {
                var selectedRefProvAttribs = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',');
                if (selectedRefProvAttribs && selectedRefProvAttribs.length > 1) {

                    var refProvSeqNum = selectedRefProvAttribs[0];
                    var refProvShortName = selectedRefProvAttribs[this.ProviderShortNameIndex];

                    patient_addreferringprovider.UpdatePatientRefProviderUI(refProvSeqNum, refProvShortName);
                }
                else {
                    utility.DisplayMessage({
                        'message': 'Please select provider.',
                        'errorType': '1'
                    });
                    return;
                }
        }

            //OLd check 
            //else if(refprovider.Params.Sender == "dashboard_patient_checkin" || refprovider.Params.Sender == "dashboard_startprocess") 
            //updated by zubair Ali.



        else if (refprovider.Params && (refprovider.Params.Sender == "dashboard_patient_checkin" || refprovider.Params.Sender == "dashboard_startprocess" || refprovider.Params.Sender == "patient_walkincheckin" || refprovider.Params.Sender == "dashboard_patient_checkout")) {
            if ($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0] != "") {
                $(refprovider.Params.Container + " #RefProviderID").val($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0]);
                $(refprovider.Params.Container + " #txtRefProvider").val($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[1]);
                $(refprovider.Params.Container + " #lnk_removerefprovider").show();
            } else {
                utility.DisplayMessage({
                    'message': 'Please select provider.',
                    'errorType': '2'
                });
                return;
            }
        }
        else if (refprovider.Params && refprovider.Params.Sender == patient_referring_provider_detail.Name) {
            var selectedRefProvAttribs = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',');

            if (selectedRefProvAttribs && selectedRefProvAttribs.length > 1) {

                var refProvSeqNum = selectedRefProvAttribs[0];
                var refProvShortName = selectedRefProvAttribs[this.ProviderShortNameIndex];

                patient_referring_provider_detail.UpdateSelectedProvider(refProvSeqNum);

                // $("#" + patient_referring_provider_detail.Name  + " #" +  patient_referring_provider_detail.ddlRefProviderID).val(

            }


        }
        else {
            if ($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0] != "") {
                $(refprovider.Params.Container + " #RefProviderID").val($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0]);
                $(refprovider.Params.Container + " #chkRefBy").prop('checked', true);
            } else {
                utility.DisplayMessage({
                    'message': 'Please select provider.',
                    'errorType': '1'
                });
                return;
            }
        }
        UnloadActionPan();
    },
    Click_RefProvider_lnkDelete: function () {
        utility.Confirm({
            'action': function () {
                var SEQ_NUM;
                if ($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0]) {
                    SEQ_NUM = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0];
                } else if (refprovider.Params.IsrefproExist) {
                    SEQ_NUM = refprovider.Params.Seq_Num;
                }
                dataservice_patient.deleteRefpro(SEQ_NUM).done(function (response) {
                    //var response = $.parseJSON(json);
                    if (response.status) {
                        utility.DisplayMessage({
                            'message': response.Message,
                            'errorType': '1'
                        });
                        if (refprovider.Params.IsrefproExist) {
                            $(refprovider.Params.Container + " #RefProviderID").val('');
                            $(refprovider.Params.Container + " #chkRefBy").prop('checked', false);
                            UnloadActionPan();
                        }
                        else {
                            $("#refprovider #RefProvider_Detail").css("display", "none");
                            $("#refprovider #RefProvider_Search").css("display", "");
                            refprovider.Click_RefProvider_btnSearch();
                        }
                    } else if (response.status == false) {
                        utility.DisplayMessage({
                            'message': response.Message,
                            'errorType': '2'
                        });
                    }
                })
            },
            'Cancel': function () {
            },
            'message': 'Are you sure you want to delete selected record ?',
            'YesText': 'Yes',
            'NoText': 'No'
        });
    },
    Click_lnkInsurance_GetRefProIns: function (refseq_num) {

        $("#refprovider #RefProvidersInsSearchGrid").emrgrid({
            url: "default.ashx?class=REFPROVIDER&action=GET_PROVIDER_INSURANCES&RefproSeqNum=" + refseq_num,
            dataType: 'json',
            colModel: [
                { display: 'Insurance', name: 'INSURANCE_SHORT_NAME', width: '10%', sortable: true, align: 'left' },
                { display: 'PIN', name: 'PIN_NUM', width: '10%', sortable: true, align: 'left' },
                { display: 'Qualifier', name: 'PIN_QUALIFIER', width: '10%', sortable: true, align: 'left' },
            ],
            sortname: "INSURANCE_SEQ_NUM",
            sortorder: "desc",
            usepager: true,
            resizable: false,
            colResize: false,
            singleSelect: true,
            onSubmit: false,
            rp: 15,
            width: '100%',
            height: 100,
            cachedata: false,
            widthmode: 'auto',
            datakeys: 'INSURANCE_SEQ_NUM,REF_PROV_SEQ_NUM',
            dataselectioncontrol: 'REFPROINS_SELECTEDDATAKEYS'
        })
    },
    Click_lnkInsurance_AddNew: function () {
       
        $("#refprovider #RefProvider_ProvInsurance .Validator").hide();
        if ($("#refprovider #REFPRO_SELECTEDDATAKEYS").val() != "") {
            refprovider.Insaction = "NEW";
            $("#refprovider #REFPROINS_SELECTEDDATAKEYS").val('')
            $("#refprovider #RefProvider_ProvInsurance").css("display", "");
            $('#refprovider #RefProvider_ProvInsurance').find('input:text').val('');
            $('#refprovider #RefProvider_ProvInsurance').find('select').val('');
            $("#RefProvider_Detail #ddlQualifier").val("1G");
        } else {
            utility.DisplayMessage({
                'message': 'Please save the referring proider first.',
                'errorType': '1'
            });
        }
    },
    Click_lnkInsurance_Edit: function () {
        $("#refprovider #RefProvider_ProvInsurance .Validator").hide();
        if ($("#refprovider #REFPROINS_SELECTEDDATAKEYS").val() != "") {
            refprovider.Insaction = "UPDATE";
            $("#refprovider #RefProvider_ProvInsurance").css("display", "");
            var self = $('#refprovider #RefProvider_ProvInsurance');
            var myJSON = self.getMyJSON();
            var RefproSeqNum, INSURANCE_SEQ_NUM;
            if ($("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[0]) {
                INSURANCE_SEQ_NUM = $("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[0];
                RefproSeqNum = $("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[1];
            }
            dataservice_patient.getSelectedRefproInsInfo(RefproSeqNum, INSURANCE_SEQ_NUM).done(function (json) {
                self.bindMyJSON(true, json);
            })
        } else {
            utility.DisplayMessage({
                'message': 'Please select a record first.',
                'errorType': '1'
            });
        }
    },
    Click_lnkInsurance_Delete: function () {
        if ($("#refprovider #REFPROINS_SELECTEDDATAKEYS").val() != "") {
            utility.Confirm({
                'action': function () {
                    var RefproSeqNum, INSURANCE_SEQ_NUM;

                    if ($("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[0]) {
                        INSURANCE_SEQ_NUM = $("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[0];
                        RefproSeqNum = $("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[1];
                    }
                    dataservice_patient.deleteRefproIns(RefproSeqNum, INSURANCE_SEQ_NUM).done(function (json) {
                        var response = $.parseJSON(json);
                        if (response.status == true) {
                            utility.DisplayMessage({
                                'message': response.Message,
                                'errorType': '1'
                            });
                            refprovider.Click_lnkInsurance_GetRefProIns($("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[1]);
                        } else if (response.status == false) {
                            utility.DisplayMessage({
                                'message': response.Message,
                                'errorType': '2'
                            });
                        }
                    })

                },
                'Cancel': function () {
                },
                'message': 'Are you sure you want to delete ?',
                'YesText': 'Yes',
                'NoText': 'No'
            });
        }
        else {
            utility.DisplayMessage({
                'message': 'Please select a record first.',
                'errorType': '1'
            });
        }

    },
    Click_RefProvider_btnInsurance_Save: function () {
        var self = $('#refprovider #RefProvider_ProvInsurance');
        if (ControlValidator.IsUIValidated(self)) {
            var myJSON = self.getMyJSON();
            var SEQ_NUM;
            if ($("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[1]) {
                SEQ_NUM = $("#refprovider #REFPROINS_SELECTEDDATAKEYS").val().split(',')[1];
            } else if (refprovider.Params.IsrefproExist) {
                SEQ_NUM = refprovider.Params.Seq_Num;
            } else {
                SEQ_NUM = $("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0]
            }
            dataservice_patient.saveRefproIns(SEQ_NUM, myJSON, refprovider.Insaction).done(function (json) {
                var response = $.parseJSON(json);
                if (response.status == true) {
                    utility.DisplayMessage({
                        'message': response.Message,
                        'errorType': '1'
                    });
                    if (refprovider.Params.IsrefproExist) {
                        UnloadActionPan();
                    }
                    else {
                        $("#refprovider #RefProvider_ProvInsurance").css("display", "none");
                        refprovider.Click_lnkInsurance_GetRefProIns($("#refprovider #REFPRO_SELECTEDDATAKEYS").val().split(',')[0]);
                    }
                } else if (response.status == false) {
                    utility.DisplayMessage({
                        'message': response.Message,
                        'errorType': '2'
                    });
                }
            })
        }
    },
    Click_RefProvider_btnInsurance_Reset: function () {
        refprovider.Click_lnkInsurance_Edit();
    },
    Click_RefProvider_btnInsurance_Cancel: function () {
        $("#refprovider #RefProvider_ProvInsurance").css("display", "none");
    },
    //zubair
    ShowHideInsuranceList: function ()
    {
        //debugger;
        if ($.trim($("#refprovider #txtShortName").val()).length > 0 && $.trim($("#refprovider #txtLastName").val()).length > 0 && $.trim($("#refprovider #txtFirstName").val()).length > 0)
        {
            $("#RefProvider_ProvInsuranceList").show();
        }
        else
            $("#RefProvider_ProvInsuranceList").hide();

        //RefProvider_ProvInsuranceList



    }

    
}

