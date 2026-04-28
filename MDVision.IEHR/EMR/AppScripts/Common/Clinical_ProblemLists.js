Clinical_ProblemLists = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    icdsValues: {},
    cancerCodes: [],
    isCancerDisease: false,
    LastSctBaseSearch: '',
    FavListName: "ClinicalProblemList",

    Load: function (params) {

        Clinical_ProblemLists.params = params;
        if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
            $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        Clinical_ProblemLists.params.mode = "Add";

        if (Clinical_ProblemLists.params.PanelID != 'pnlClinicalProblemLists') {
            Clinical_ProblemLists.params.PanelID = Clinical_ProblemLists.params.PanelID + ' #pnlClinicalProblemLists';
        }
        else {
            Clinical_ProblemLists.params.PanelID = 'pnlClinicalProblemLists';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
            $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        if (Clinical_ProblemLists.bIsFirstLoad) {

            Clinical_ProblemLists.domReadyFunc();

            //for Favorites toggle
            EMRUtility.setFavoriteSectionStyle(Clinical_ProblemLists.params.PanelID);

            $.when(utility.ValidateFromToDate('frmClinicalProblemLists', 'dpStartDate', 'dpEndDate', true)).then(function () {
                $('#frmClinicalProblemLists #dpStartDate').datepicker('setStartDate', $("#banner_PatientDOB").html());
                $('#frmClinicalProblemLists #dpEndDate').datepicker('setStartDate', $("#banner_PatientDOB").html());
            });
            //  $('#frmClinicalProblemLists #dpStartDate').datepicker('setEndDate', $("#banner_PatientDOB").html());


            //Start Farooq Ahmad 25/3/2016 if from Order Detail then hide the detail section
            try {
                $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result").css('display', 'inline');
                $("#" + Clinical_ProblemLists.params.PanelID + " #formpanelheading").css('display', '');
                if (Clinical_ProblemLists.params.FromOrderDetail != null && Clinical_ProblemLists.params.FromOrderDetail == "1") {
                    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result").css('display', 'none');
                    $("#" + Clinical_ProblemLists.params.PanelID + " #formpanelheading").css('display', 'none');
                }
            } catch (ex) {
                console.log(ex);
            }
            //End Farooq Ahmad 25/3/2016 if from Order Detail then hide the detail section
        }

        var FromOrderDetail = false;
        try {
            FromOrderDetail = (Clinical_ProblemLists.params.FromOrderDetail != null && Clinical_ProblemLists.params.FromOrderDetail == "1");
        } catch (ex) {
            console.log(ex);
        }

        if (!FromOrderDetail)
            Clinical_ProblemLists.ProblemListsSearch();

        var self = $('#' + Clinical_ProblemLists.params.PanelID);

        if (Clinical_ProblemLists.bIsFirstLoad == true) {
            var data = "IsActive=1";
            // AST-357 load provider sepcfic problem from Note flow.
            if (Clinical_ProblemLists.params.CurrentNotesProviderId && Clinical_ProblemLists.params["IsFromNote"]) {
                data = data + "&StrID=" + Clinical_ProblemLists.params.CurrentNotesProviderId;
            }

            self.loadDropDowns(true).done(function () {
                // AST-545 load problems dropdown 
                self.find("#ddlFavProblems").attr("ddlist", "GetFavProblems");
                self.find("#favSectionDiv").loadDropDowns(true, data).done(function () {


                    Clinical_ProblemLists.loadAllCancerCodes();
                    Clinical_ProblemLists.ValidateProblemLists();
                    Clinical_ProblemLists.bIsFirstLoad = false;

                    //Serialization
                    $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').data('serialize', $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').serialize());

                    //Start 2-07-2016 M Ahmad Imran for favorite list setting for all favLists
                    if (EMRUtility.getFavListStatus(Clinical_ProblemLists.FavListName)) {
                        $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").addClass("toggledHor");
                        $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #FormDiv").addClass("toggleHorContainer");
                    }
                    else {
                        $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").removeClass("toggledHor");
                        $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #FormDiv").removeClass("toggleHorContainer");
                    }
                    //End 2-07-2016 M Ahmad Imran for favorite list setting for all favLists

                    Clinical_ProblemLists.SetFavListVal($('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems'));
                }); // end problm list download mathao
            });
        }
        else {
            if (EMRUtility.getFavListStatus(Clinical_ProblemLists.FavListName)) {

                $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").addClass("toggledHor");
                $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #FormDiv").addClass("toggleHorContainer");
            }
            else {
                $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").removeClass("toggledHor");
                $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #FormDiv").removeClass("toggleHorContainer");
            }

            Clinical_ProblemLists.SetFavListVal($('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems'));
        }

        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {

            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #chkChiefComplaints").attr('checked', false);
            $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #chkCC").removeClass('hidden');

            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_ProblemLists.params.PanelID, 'Medical', 'Problems', 'Clinical_ProblemLists.UnLoadTab();', 'frmClinicalProblemLists');
        }

        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ProblemLists.params.PanelID + " div#FaceSheetPager", Clinical_ProblemLists.params.FaceSheetComponents, 'problem list');
        } else if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalProblemLists' + " div#FaceSheetPager", Clinical_ProblemLists.params.FaceSheetComponents, 'problem list');
        }
        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });

        //Load MU3 Alerts
        var PatientId = $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val();
        utility.LoadMUAlerts(PatientId, true);
    },
    loadAllCancerCodes: function () {
        Clinical_ProblemLists.loadCancerCodes().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.each(response.CodesJSON, function (i, item) {

                    Clinical_ProblemLists.cancerCodes.push(item.PreferredAlternateCode);
                });

            }
        });
    },
    loadCancerCodes: function () {
        var objData = new Object();
        objData["commandType"] = "load_cancer_codes";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    SetFavListVal: function ($ddl) {

        var FavOptionLength = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ddlFavProblems option").length;

        if (FavOptionLength > 1) {

            EMRUtility.getFavListValue(Clinical_ProblemLists.FavListName).done(function (response1) {

                response1 = JSON.parse(response1);

                if (response1.status != false) {

                    if (response1.favListVal != "") {

                        if ($("#" + Clinical_ProblemLists.params.PanelID + " #ddlFavProblems option[value='" + response1.favListVal + "']").length > 0) {
                            $ddl.val(response1.favListVal);
                            $ddl.trigger("onchange");
                        }
                        else {
                            if (FavOptionLength == 2) {
                                $ddl.val($("#" + Clinical_ProblemLists.params.PanelID + " #ddlFavProblems option:nth-child(2)").val());
                                $ddl.trigger("onchange");
                            }
                            else if (FavOptionLength > 2) {
                                $ddl.trigger("onchange");
                            }
                            else {
                                $ddl.trigger("onchange");
                            }
                        }
                    }
                    else {
                        if (FavOptionLength == 2) {
                            $ddl.val($("#" + Clinical_ProblemLists.params.PanelID + " #ddlFavProblems option:nth-child(2)").val());
                            $ddl.trigger("onchange");
                        }
                        else if (FavOptionLength > 2) {
                            $ddl.trigger("onchange");
                        }
                        else {
                            $ddl.trigger("onchange");
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    // Author: Muhammad Arshad
    //Date : 11-04-2016
    //This will show ActivityLog for Selected Problem List
    ShowHistory: function (problemListId) {

        //adnan maqbool, EMR-745
        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet" || Clinical_ProblemLists.params.ParentCtrl == "Clinical_Treatment") {
            EMRUtility.showCurrentItemHistory(Clinical_ProblemLists.params.PanelID, null, problemListId, "ProblemList", Clinical_ProblemLists.params.patientID, "Clinical_ProblemLists", null);
        } else {
            //Begin  4/26/16  Edit By M Ahmad Imran Bug # EMR-813
            EMRUtility.showCurrentItemHistory(Clinical_ProblemLists.params.PanelID, null, problemListId, "ProblemList", Clinical_ProblemLists.params.patientID, Clinical_ProblemLists.params.ParentCtrl != "clinicalTabProgressNote" ? Clinical_ProblemLists.params.TabID : "Clinical_ProblemLists", null);
            //End  4/26/16  Edit By M Ahmad Imran Bug # EMR-813
        }
    },

    DownloadProblems: function () {
        var objData = [];
        objData["commandType"] = "download_data";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "downloadProblemList");
    },

    PostXmlData: function () {

        //var alergi = '<?xml version="1.0" encoding="UTF-8"?><RCExtRequest version = "2.19"> <Caller>  <VendorName>vendorsh1100</VendorName>  <VendorPassword>b2xddjpn</VendorPassword> </Caller> <SystemName>vendorsh1100</SystemName> <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername> <Request>  <Command>send_allergy</Command>  <AllergyList>     <!-- Allergy defined by drug NDC id -->   <Allergy>    <ExternalID>testax1000</ExternalID>    <RcopiaID></RcopiaID>    <Deleted>n</Deleted>    <Status><Active/></Status>     <Patient>     <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>     <ExternalID>' + $externalid.text() + '</ExternalID>    </Patient>    <Allergen>     <Name>aspirin</Name>     <Drug>      <RcopiaID></RcopiaID>      <NDCID>58487000101</NDCID>     </Drug>    </Allergen>    <Reaction>nausea</Reaction>    <OnsetDate>11/01/2015</OnsetDate>   </Allergy>      <!-- Allergy defined by allergen name, Rcopia will try to match -->   <Allergy>    <ExternalID>testax1001</ExternalID>    <RcopiaID></RcopiaID>        <Deleted>n</Deleted>    <Status><Active/></Status>     <Patient>     <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>     <ExternalID>' + $externalid.text() + '</ExternalID>    </Patient>    <Allergen>     <Name>Lisinopril</Name>    </Allergen>     <Reaction>rash</Reaction>    <OnsetDate></OnsetDate>   </Allergy>      <!-- Allergy defined by Rcopia allergy group id -->   <Allergy>    <ExternalID>testax1002</ExternalID>    <RcopiaID></RcopiaID>    <Deleted>n</Deleted>    <Patient>     <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>     <ExternalID>' + $externalid.text() + '</ExternalID>    </Patient>    <Allergen>     <Name>Glutamic Acid</Name>     <Group>      <RcopiaID>620</RcopiaID>     </Group>    </Allergen>    <Reaction>sneezing</Reaction>    <OnsetDate></OnsetDate>   </Allergy>  </AllergyList> </Request></RCExtRequest>        ';
        //var medicationData = "<?xml version='1.0' encoding='UTF-8'?><RCExtRequest version = '2.19'>    <Caller>        <VendorName>vendorsh1100</VendorName>        <VendorPassword>b2xddjpn</VendorPassword>    </Caller>    <SystemName>vendorsh1100</SystemName>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername>    <Request>        <Command>send_medication</Command>        <MedicationList>            <!-- Medication defined by NDC list, with structured sig -->            <Medication>                <Deleted>n</Deleted>                <RcopiaID></RcopiaID>                <ExternalID>testmed1000</ExternalID>                <Patient>                    <RcopiaID>" + $Recopiaid.text() + "</RcopiaID>                    <ExternalID>" + $externalid.text() + "</ExternalID>                </Patient>                <Provider>                    <Username>sprovider47</Username>                </Provider>                <Sig>                    <Drug>                        <NDCID>00071015694,52959076090,49999046790</NDCID>                        <BrandName>Lipitor</BrandName>                        <GenericName></GenericName>                        <Form>tablet</Form>                        <Strength>20 mg</Strength>                    </Drug>                    <Dose>1</Dose>                    <DoseUnit>tablet</DoseUnit>                    <DoseTiming>once per day</DoseTiming>                    <Duration></Duration>                    <Quantity>60</Quantity>                    <QuantityUnit>tablet</QuantityUnit>                    <Refills>2</Refills>                    <SubstitutionPermitted>y</SubstitutionPermitted>                    <OtherNotes></OtherNotes>                    <PatientNotes>take with a glass of water</PatientNotes>                </Sig>                <StartDate>11/01/2015</StartDate>               <StopDate></StopDate>                <FillDate>11/02/2015</FillDate>                <StopReason></StopReason>            </Medication>            <!-- Free text medication. Will not be used for interaction checking -->            <Medication>                <Deleted>n</Deleted>                <RcopiaID></RcopiaID>                <ExternalID>testmed1001</ExternalID>                <Patient>                    <RcopiaID></RcopiaID>                    <ExternalID>paltrowbruce</ExternalID>                </Patient>                <Provider>                    <Username>sprovider47</Username>                </Provider>                <Sig>                    <Drug>                        <NDCID></NDCID>                        <BrandName>Elphaba's Green Pill</BrandName>                        <GenericName></GenericName>                        <Strength></Strength>                    </Drug>                    <PatientNotes>One per day as needed for mild fatigue.</PatientNotes>                </Sig>                <StartDate>11/01/2015</StartDate>                <StopDate></StopDate>                <FillDate></FillDate>                <StopReason></StopReason>            </Medication>            <!-- This is a medication that is stopped -->            <Medication>                <Deleted>n</Deleted>                <RcopiaID></RcopiaID>                <ExternalID>testmed1002</ExternalID>                <Patient>                    <RcopiaID></RcopiaID>                    <ExternalID>paltrowbruce</ExternalID>                </Patient>                <Provider>                    <Username>sprovider47</Username>                </Provider>                <Sig>                    <Drug>                        <NDCID>00006022101,00006022131,51138047930</NDCID>                        <BrandName>Januvia</BrandName>                        <GenericName>Sitagliptin</GenericName>                        <Strength>25 mg</Strength>                    </Drug>                    <Dose>1</Dose>                    <DoseUnit>tablet</DoseUnit>                    <DoseTiming>once per day</DoseTiming>                    <Duration></Duration>                    <Quantity>60</Quantity>                    <QuantityUnit>tablet</QuantityUnit>                    <Refills>2</Refills>                    <SubstitutionPermitted>y</SubstitutionPermitted>                    <OtherNotes></OtherNotes>                    <PatientNotes>take with a glass of water</PatientNotes>                         </Sig>                <StartDate></StartDate>                <StopDate>11/01/2015</StopDate>                <FillDate></FillDate>                <StopReason></StopReason>            </Medication>  </MedicationList>    </Request></RCExtRequest>        ";
        //var problemListData = '<?xml version="1.0" encoding="UTF-8"?><RCExtRequest version = "2.19">    <Caller>        <VendorName>vendorsh1100</VendorName>        <VendorPassword>b2xddjpn</VendorPassword>    </Caller>    <SystemName>vendorsh1100</SystemName>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername>    <Request>                                <Command>send_problem</Command>                                <ProblemList>                                                <Problem>                                                                <Deleted>n</Deleted>                                                                <Status><Active/></Status>                                                                <OnsetDate>11/01/2015</OnsetDate>                                                                <RcopiaID></RcopiaID>                                                                <ExternalID>testdx1000</ExternalID>                                                                <Patient>                                                                    <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>                                                                     <ExternalID>' + $externalid.text() + '</ExternalID>                                                                </Patient>                                                                <ICD9>                                                                                <Code>250.00</Code>                                                                                <Description>DIABETES MELLITUS WITHOUT MENTION OF COMPLICATION</Description>                                                                </ICD9>                                                </Problem>                                                <Problem>                                                                <Deleted>n</Deleted>                                                                <Status><Active/></Status>                                                                <OnsetDate>11/01/2015</OnsetDate>                                                                <RcopiaID></RcopiaID>                                                                <ExternalID>testdx1001</ExternalID>                                                                <Patient>                                                                    <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>                                                                     <ExternalID>' + $externalid.text() + '</ExternalID>                                                                </Patient>                                                                <ICD10>                                                                                <Code>I11.0</Code>                                                                                <Description>HYPERTENSIVE HEART DISEASE WITH HEART FAILURE</Description>                                                                </ICD10>                                                </Problem>                                                                                       <Problem>                                                                <Deleted>n</Deleted>                                                                <Status><Active/></Status>                                                                <OnsetDate>11/01/2015</OnsetDate>                                                                <RcopiaID></RcopiaID>                                                                <ExternalID>testdx1002</ExternalID>                                                                <Patient>                                                                                <RcopiaID>' + $Recopiaid.text() + '</RcopiaID>                                                                                <ExternalID>' + $externalid.text() + '</ExternalID>                                                                </Patient>                                                                <SNOMED>                                                                                <ConceptID>111297002</ConceptID>                                                                                <Description>NONPARALYTIC STROKE</Description>                                                                </SNOMED>                                                </Problem>                                </ProblemList>                </Request></RCExtRequest>        ';
        //var getXML = "";
        //$.get('./resources/XmlFile1.xml', function (res) {


        //    var serializer = new XMLSerializer();
        //    getXML = serializer.serializeToString(res);
        //})
        var inputdata = '<?xml version="1.0" encoding="UTF-8"?><RCExtRequest>	<Caller>		<VendorName>vendorsh1100</VendorName>		<VendorPassword>b2xddjpn</VendorPassword>	</Caller>    <RcopiaPracticeUsername>lmsp-sh1100</RcopiaPracticeUsername>	<Request>		<Command>get_url</Command>	</Request></RCExtRequest>';
        //var invocation = new XMLHttpRequest();
        //var url = 'https://ans2.drfirst.com/getURL?xml=' + inputdata;


        //if (invocation) {
        //    invocation.open('GET', url, true);
        //    invocation.onreadystatechange = handler;
        //    invocation.onload = function (res) {
        //        var text = res.responseText;
        //        alert(text);
        //        var title = getTitle(text);

        //    };

        //    invocation.onerror = function (E) {
        //        console.log(e.message)
        //        alert('Woops, there was an error making the request. exception=' + e.message);
        //    };
        //    invocation.send();
        //}

        //$.ajax({
        //    type: 'GET',
        //    url: 'https://ans2.drfirst.com/getURL?xml=' + inputdata,
        //    dataType: 'jsonp',
        //    beforeSend: function () {
        //        BackgroundLoaderShow(true);
        //    },
        //    success: function (response) {
        //        alert(response.responsetext);
        //    },
        //    error: function (response) {
        //        alert(JSON.stringify(response));
        //    }
        //});
    },

    BindICD9AutoComplete: function (element) {

        if ($(element).val().length > 3) {

            if ($(element).val().substring(0, 3).toLowerCase() == "sct") {
                Clinical_ProblemLists.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            }
            else {
                Clinical_ProblemLists.LastSctBaseSearch = "";
            }
        }
        else {
            Clinical_ProblemLists.LastSctBaseSearch = "";
        }

        $('#pnlClinicalProblemLists #txtProblems').attr("data-popupunload", "false");

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_ProblemLists", null, false);
    },

    // Validate and save/edit functions
    ValidateProblemLists: function () {
        $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').unbind("success.form.bv");
        $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   ProblemName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_ProblemLists.ProblemListsSave();
        });
    },

    //openDrFirst: function () {
    //    BackgroundLoaderShow(true);
    //    var params = [];
    //    params["StartupScreen"] = "patient";
    //    params["PatientId"] = Clinical_ProblemLists.params.patientID;
    //    params["FromAdmin"] = 0;
    //    params["ParentCtrl"] = Clinical_ProblemLists.params.ParentCtrl != "clinicalTabProgressNote" ? Clinical_ProblemLists.params.TabID : "Clinical_ProblemLists";
    //    LoadActionPan("DRFirst", params);
    //},

    ProblemListsSave: function () {

        var PreFavVal = $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').val();
        var tPlist = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtProblems").val();
        if (tPlist != null && tPlist.indexOf(' - ') > -1) {
            var strArray = tPlist.split(' - ');
            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtProblems").val(strArray[1].trim());
        }

        //Start//13/01/2016//Ahmad Raza//fixed bug#EMR-212
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');

        if ($("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtProblems").val() != "" && $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtDiagnosis").val() != "") {
            var probDesc = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtProblems").val().trim();
            var des = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtDiagnosis").val()
            var diagDesc = des.substring(des.indexOf(" - ") + 2).trim();
            if (probDesc == diagDesc) {
                //End//13/01/2016//Ahmad Raza//fixed bug#EMR-212
                var strMessage = "";

                $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
                    $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
                }
                var self = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists");
                var myJSON = self.getMyJSONByName();
                var problem = $("#" + Clinical_ProblemLists.params.PanelID + " #txtDiagnosis").val();

                //Start 25-08-2016 Humaira Yousaf to prevent duplicate problem for referrals
                if (Clinical_ProblemLists.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail") {

                    var exists = false;
                    if ($("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ulProblemLists tbody tr").text().indexOf(problem) > -1) {
                        exists = true;
                    }
                    // var exists = Patient_Referrals_Outgoing_Detail.isProblemExists(problem);
                    if (exists == true) {
                        utility.DisplayMessages("This code already exists in the Referral.", 3);
                        return;
                    }
                }
                // Implement me properly after Demo: 15March2017
                var problemExists = false;
                var ICDCodeANDDescription = "";
                var icdCode = "";
                var icdDescription = "";
                var problemCode = "";
                if (problem) if (problem.indexOf('-') > 1) problemCode = problem.split('-')[0].trim();

                var ArrayProblems = new Array();
                var columnIndex = $(" #dgvProblemLists thead tr th:contains('ICD (Diagnosis)')").index() + 1;

                $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tbody tr td:nth-child(" + columnIndex + ")").each(function (i) {

                    if ($(this).text()) {
                        ICDCodeANDDescription = $(this).text();
                        if (ICDCodeANDDescription.indexOf('-') > 1) {
                            icdCode = ICDCodeANDDescription.split('-')[0].trim();
                            icdDescription = ICDCodeANDDescription.split('-')[1].trim();
                            ArrayProblems.push(icdCode);
                        }
                    }
                    else
                        if ($("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tbody tr").text().indexOf(problem) > -1)
                            problemExists = true;
                });
                if ((ArrayProblems.indexOf(problemCode)) != -1)
                    problemExists = true;
                if (problemExists == true) {
                    utility.DisplayMessages("Problem Already Exists.", 3);
                    return;
                }
                var IsThisActive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive') == null ? "1" : "0";

                //Clinical_ProblemLists.SearchProblemList(null, null, null, IsThisActive).done(function (response) {
                //    var qresponse = JSON.parse(response);
                //    if (qresponse.status != false) {
                //        if (qresponse.ProblemListLoad_JSON) {
                //            response = JSON.parse(qresponse.ProblemListLoad_JSON);
                //            $.each(response, function (i, item) {
                //                ArrayProblems.push(item.ICD10);
                //            });

                //            if ((ArrayProblems.indexOf(problemCode)) != -1)
                //                problemExists = true;

                //            if (problemExists == true) {
                //                utility.DisplayMessages("Problem Already Exists.", 3);
                //                return;
                //            }
                //        }
                //    }
                //End 25-08-2016 Humaira Yousaf to preventing duplicate problem for referrals
                //return false;

                if (Clinical_ProblemLists.params.mode == "Add") {

                    var objDeferredLoadProblems = $.Deferred();
                    //Start//21/12/2015//Ahmad Raza//Logic implemented for privileges
                    // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
                    var hfProblemText = $("#" + Clinical_ProblemLists.params.PanelID + " #hfIMOProblem").val();
                    var changesProblemText = $("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val();

                    // End 19/01/2016 Muhammad Irfan for bug # EMR-219
                    if ((hfProblemText.toString() == changesProblemText.toString()) || changesProblemText.toString() != "") {

                        //AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

                        //    if (strMessage == "") {

                        Clinical_ProblemLists.SaveProblemLists(myJSON, PreFavVal).done(function (response) {

                            response = JSON.parse(response);
                            if (response.status != false) {
                                var Diagnosis = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtDiagnosis").val();
                                var ICDInserted = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtDiagnosis").val().split('-')[0];
                                //------------------
                                if (Clinical_ProgressNote.params.isProgressNoteSelected) {
                                    Clinical_ProgressNote.params.newlyAddedProblemLists.push(response.ProblemListId);
                                }
                                //------------------

                                //Clinical_ProblemLists.SaveFavToggelStatus(PreFavVal);
                                Clinical_ProblemLists.LastSctBaseSearch = "";
                                $('#pnlClinicalProblemLists #ulFavCompliantDisease li').remove();

                                //Start Farooq Ahmad 25/3/2016 if from Order Detail then hide the detail section
                                try {
                                    if (Clinical_ProblemLists.params.FromOrderDetail != null && Clinical_ProblemLists.params.FromOrderDetail == "1") {
                                        if (Clinical_ProblemLists.params.ParentCtrl == "Patient_Referrals_Incoming_Detail") {
                                            $.when(Patient_Referrals_Incoming_Detail.loadProblemList()).then(function () {
                                                $.when(Patient_Referrals_Incoming_Detail.CheckedPreviousProbems()).then(function () {
                                                    UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                                                    objDeferredLoadProblems.resolve("ok");
                                                });
                                            });
                                        }
                                        else if (Clinical_ProblemLists.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail") {
                                            //EMR-6537 BY:MAHMAD
                                            Patient_Referrals_Outgoing_Detail.ReferralProblemsChange = true;
                                            //EMR-6537 BY:MAHMAD
                                            $.when(Patient_Referrals_Outgoing_Detail.loadProblemList()).then(function () {
                                                $.when(Patient_Referrals_Outgoing_Detail.CheckedPreviousProbems()).then(function () {
                                                    UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                                                    objDeferredLoadProblems.resolve("ok");
                                                });
                                            });
                                        }
                                        else if (Clinical_ProblemLists.params.ParentCtrl == "ClinicalProcedureOrderDetail") {
                                            $.when(ClinicalProcedureOrderDetail.loadProblemList()).then(function () {
                                                $.when(ClinicalProcedureOrderDetail.CheckedPreviousProbems()).then(function () {
                                                    UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                                                    objDeferredLoadProblems.resolve("ok");
                                                });
                                            });
                                        } else if (Clinical_ProblemLists.params.ParentCtrl == "ClinicalConsultationOrderDetail") {
                                            $.when(ClinicalConsultationOrderDetail.loadProblemList()).then(function () {
                                                $.when(ClinicalConsultationOrderDetail.CheckedPreviousProbems()).then(function () {
                                                    UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                                                    objDeferredLoadProblems.resolve("ok");
                                                });
                                            });
                                        }
                                        else {
                                            eval(Clinical_ProblemLists.params.ParentCtrl + ".loadProblemList(true);");
                                            UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                                            objDeferredLoadProblems.resolve("ok");
                                        }
                                    }
                                } catch (ex) {
                                    console.log(ex);
                                }
                                //End Farooq Ahmad 25/3/2016 if from Order Detail then hide the detail section

                                if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                                    Clinical_ProblemLists.getProblemListsInfo(response.ProblemListId, true).done(function () {
                                        //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-659
                                        $.when(Clinical_ProblemLists.ProblemListsSearch()).then(function () {
                                            objDeferredLoadProblems.resolve("ok");
                                        });
                                        //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-659
                                    });
                                }
                                else {
                                    var FromOrderDetail = false;
                                    try {
                                        FromOrderDetail = (Clinical_ProblemLists.params.FromOrderDetail != null && Clinical_ProblemLists.params.FromOrderDetail == "1");
                                    } catch (ex) {
                                        console.log(ex);
                                    }
                                    if (!FromOrderDetail) {
                                        $.when(Clinical_ProblemLists.ProblemListsSearch()).then(function () {
                                            objDeferredLoadProblems.resolve("ok");
                                        });
                                    }
                                }

                                utility.DisplayMessages(response.message, 1);
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', false);
                                var chkChiefComplaints = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #chkChiefComplaints").prop('checked');
                                $("#mainForm  li#CDSAlert").show();
                                if (Clinical_ProblemLists.params.ParentCtrl != "Clinical_FaceSheet") {

                                    $.when(setPatientBanner($('#PatientProfile #hfPatientId').val(), "1")).then(function () {
                                        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote")
                                            Clinical_ProgressNote.LoadCDSAlerts();
                                    });
                                }
                                //        $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
                                $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                                if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
                                    $("#" + Clinical_ProblemLists.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
                                }
                                // Start 27/11/2015 Muhammad Irfan Bug # 91,92
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                                $("#pnlClinicalProblemLists #frmClinicalProblemLists #dpStartDate").prop("disabled", true);
                                $("#" + Clinical_ProblemLists.params.PanelID + " #txtComments,#txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpEndDate").prop("disabled", true);
                                // End 27/11/2015 Muhammad Irfan Bug # 91,92
                                //issue fix, for having previously added Problem List information
                                $("#" + Clinical_ProblemLists.params.PanelID + " #ulProblemDisease").empty();
                                //if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                                //    Clinical_ProblemLists.getProblemListsInfo(response.ProblemListId);
                                //}
                                //Start//17/12/2015//Ahmad Raza//Serialization of form
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').data('serialize', $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').serialize());
                                //End//17/12/2015//Ahmad Raza//Serialization of form

                                //Clinical_ProblemLists.AddProblemOnDrFirst(response.ProblemListId);

                                if (Clinical_ProgressNote.params.NotesId != null && Clinical_ProgressNote.params.NotesId > 0 && Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote" && chkChiefComplaints) {
                                    Clinical_Complaints.params.ParentCtrl = "Clinical_ProblemLists";
                                    Clinical_Complaints.getLatestComplaintByPatientId();
                                }

                                $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').val(PreFavVal);
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').trigger("onchange");

                                // IMP-749
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlChronicityLevel').val('');
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlSeverity').val('');
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #dpStartDate').datepicker('setDate', null);
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #dpEndDate').datepicker('setDate', null);
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #txtComments').val('');

                                var bExists = false;
                                var paramsCancerDetail = [];
                                $.each(Clinical_ProblemLists.cancerCodes, function (i, item) {

                                    if (item.trim() == ICDInserted.trim()) {

                                        Clinical_ProblemLists.isCancerDisease = true;
                                        paramsCancerDetail = [];
                                        paramsCancerDetail["FromAdmin"] = "0";
                                        paramsCancerDetail["Diagnosis"] = Diagnosis;
                                        paramsCancerDetail["ProblemListsId"] = response.ProblemListId;
                                        if (!Clinical_ProblemLists.params.ParentCtrl || Clinical_ProblemLists.params.ParentCtrl == "undefined" || Clinical_ProblemLists.params.ParentCtrl == "" || Clinical_ProblemLists.params.ParentCtrl == "Clinical_Treatment")
                                            paramsCancerDetail["ParentCtrl"] = "Clinical_ProblemLists";
                                        else
                                            paramsCancerDetail["ParentCtrl"] = Clinical_ProblemLists.params.ParentCtrl;

                                        bExists = true;

                                    }
                                });
                                $.when(objDeferredLoadProblems).done(function () {
                                    if (bExists) {
                                        setTimeout(function () {
                                            LoadActionPan("Clinical_ProblemDetails", paramsCancerDetail);
                                        }, 500);
                                    }
                                });
                            }
                            else {
                                $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                        //    }
                        //    else
                        //        utility.DisplayMessages(strMessage, 2);
                        //});
                    } else {
                        utility.DisplayMessages("Please Enter Valid Problem", 3);
                        $("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val('');
                        $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').data('bootstrapValidator').enableFieldValidators('ProblemName', true);
                    }
                    //End//21/12/2015//Ahmad Raza//Logic implemented for privileges
                }
                else if (Clinical_ProblemLists.params.mode == "Edit") {

                    Clinical_ProblemLists.UpdateProblemLists(myJSON, Clinical_ProblemLists.params.ProblemListId).done(function (response) {

                        response = JSON.parse(response);

                        if (response.status != false) {

                            Clinical_ProblemLists.LastSctBaseSearch = "";
                            utility.DisplayMessages(response.message, 1);
                            $("#mainForm  li#CDSAlert").show();
                            $.when(setPatientBanner($('#PatientProfile #hfPatientId').val(), "1")).then(function () {
                                if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote")
                                    Clinical_ProgressNote.LoadCDSAlerts();
                            });
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                }
                //});

            }
            else {
                utility.DisplayMessages("Please Enter Valid Problem", 3);
            }
        }
        else {
            utility.DisplayMessages("Please Enter Valid Problem", 3);
        }
    },

    AddProblemOnDrFirst: function (ProblemListId) {
        Clinical_ProblemLists.AddProblemOnDrFirst_DB_Call(ProblemListId).done(function (response1) {
            response1 = JSON.parse(response1);
            if (response1.status == false) {
                utility.DisplayMessages(response1.Message, 3);
            }
        });
    },

    UpdateProblemOnDrFirst: function (Command) {
        Clinical_ProblemLists.UpdateProblemOnDrFirst_DB_Call(Command).done(function (response1) {
            response1 = JSON.parse(response1);
            if (response1.status == false) {
                utility.DisplayMessages(response1.message, 3);
            }
        });
    },

    UpdateProblemOnDrFirst_DB_Call: function (Command) {
        var objData = {};

        objData["commandType"] = Command;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    AddProblemOnDrFirst_DB_Call: function (ProblemListID) {
        var objData = {};
        objData.ProblemListId = ProblemListID;
        objData["commandType"] = "SAVE_PROBLEMLIST_ON_DRFIRST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    SaveProblemLists: function (ProblemListsData, PreFavVal) {

        var objData = JSON.parse(ProblemListsData);
        if (objData.PatientId == '' || typeof objData.PatientId == 'undefined') {
            objData.PatientId = $('#PatientProfile #hfPatientId').val();
        }

        if (Clinical_ProblemLists.icdsValues != null && typeof Clinical_ProblemLists.icdsValues["ICD9"] != 'undefined'
            && Clinical_ProblemLists.icdsValues["ICD9"] != null && Clinical_ProblemLists.icdsValues["ICD9"] != '') {

            objData["ICD9"] = Clinical_ProblemLists.icdsValues["ICD9"];
            objData["ICD10"] = Clinical_ProblemLists.icdsValues["ICD10"];
            objData["ICD9_Description"] = Clinical_ProblemLists.icdsValues["ICD9_Description"];
            objData["ICD10_Description"] = Clinical_ProblemLists.icdsValues["ICD10_Description"];
            objData["SNOMEDID"] = Clinical_ProblemLists.icdsValues["SNOMEDID"];
            objData["SNOMED_DESCRIPTION"] = Clinical_ProblemLists.icdsValues["SNOMED_DESCRIPTION"];
        }
        else {
            objData["ICD9"] = $("#pnlClinicalProblemLists #ulProblemDisease li").attr("icd9code");
            objData["ICD10"] = $("#pnlClinicalProblemLists #ulProblemDisease li").attr("icd10code");
            objData["ICD9_Description"] = $("#pnlClinicalProblemLists #ulProblemDisease li").attr("icd9desc");
            objData["ICD10_Description"] = $("#pnlClinicalProblemLists #ulProblemDisease li").attr("icd10desc");
            objData["SNOMEDID"] = $("#pnlClinicalProblemLists #ulProblemDisease li").attr("snomedcode");
            objData["SNOMED_DESCRIPTION"] = $("#pnlClinicalProblemLists #ulProblemDisease li").attr("snomeddesc");
        }

        //Start for wrong snomed code
        if (Clinical_ProblemLists.LastSctBaseSearch != "") {

            if (Clinical_ProblemLists.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "385093006" && objData["SNOMEDID"] != "385093006") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "385093006")
                }
                objData["SNOMEDID"] = "385093006";
                objData["SNOMED_DESCRIPTION"] = "Community Acquired Pneumonia";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "233604007" && objData["SNOMEDID"] != "233604007") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "233604007")
                }
                objData["SNOMEDID"] = "233604007";
                objData["SNOMED_DESCRIPTION"] = "Pneumonia";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "59621000" && objData["SNOMEDID"] != "59621000") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "59621000")
                }
                objData["SNOMEDID"] = "59621000";
                objData["SNOMED_DESCRIPTION"] = "Essential hypertension";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "95891005" && objData["SNOMEDID"] != "95891005") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "95891005")
                }
                objData["SNOMEDID"] = "95891005";
                objData["SNOMED_DESCRIPTION"] = "Flu-like symptoms";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "363746003" && objData["SNOMEDID"] != "363746003") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "363746003")
                }
                objData["SNOMEDID"] = "363746003";
                objData["SNOMED_DESCRIPTION"] = "Acute pharyngitis";
            }

            else if (Clinical_ProblemLists.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "5281000124103" && objData["SNOMEDID"] != "5281000124103") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "5281000124103")
                }
                objData["SNOMEDID"] = "5281000124103";
                objData["SNOMED_DESCRIPTION"] = "Persistent asthma";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "981000124106" && objData["SNOMEDID"] != "981000124106") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "981000124106")
                }
                objData["SNOMEDID"] = "981000124106";
                objData["SNOMED_DESCRIPTION"] = "Moderate left ventricular systolic dysfunction";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "426749004" && objData["SNOMEDID"] != "426749004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "426749004")
                }
                objData["SNOMEDID"] = "426749004";
                objData["SNOMED_DESCRIPTION"] = "Chronic atrial fibrillation";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "991000124109" && objData["SNOMEDID"] != "991000124109") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "991000124109")
                }
                objData["SNOMEDID"] = "991000124109";
                objData["SNOMED_DESCRIPTION"] = "Severe left ventricular systolic dysfunction";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "64109004" && objData["SNOMEDID"] != "64109004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "64109004")
                }
                objData["SNOMEDID"] = "64109004";
                objData["SNOMED_DESCRIPTION"] = "Costal Chondritis";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "195967001" && objData["SNOMEDID"] != "195967001") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "195967001")
                }
                objData["SNOMEDID"] = "195967001";
                objData["SNOMED_DESCRIPTION"] = "Asthma";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "53741008" && objData["SNOMEDID"] != "53741008") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "53741008")
                }
                objData["SNOMEDID"] = "53741008";
                objData["SNOMED_DESCRIPTION"] = "Coronary arteriosclerosis";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "266569009" && objData["SNOMEDID"] != "266569009") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "266569009")
                }
                objData["SNOMEDID"] = "266569009";
                objData["SNOMED_DESCRIPTION"] = "Benign prostatic hyperplasia";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "49436004" && objData["SNOMEDID"] != "49436004") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "49436004")
                }
                objData["SNOMEDID"] = "49436004";
                objData["SNOMED_DESCRIPTION"] = "Atrial fibrillation";
            }
            else if (Clinical_ProblemLists.LastSctBaseSearch == "230572002" && objData["SNOMEDID"] != "230572002") {
                if (objData["Description"].indexOf(objData["SNOMEDID"]) > 0) {
                    objData["Description"] = objData["Description"].replace(objData["SNOMEDID"], "230572002")
                }
                objData["SNOMEDID"] = "230572002";
                objData["SNOMED_DESCRIPTION"] = "Diabetic neuropathy";
            }
        }
        //End for wrong snomed code

        objData["ICD9_Description"] = Clinical_ProblemLists.RemoveDashSignFromStr(objData["ICD9_Description"]);
        objData["ICD10_Description"] = Clinical_ProblemLists.RemoveDashSignFromStr(objData["ICD10_Description"]);
        objData["IMOProblem"] = Clinical_ProblemLists.RemoveDashSignFromStr(objData["IMOProblem"]);
        objData["ProblemName"] = Clinical_ProblemLists.RemoveDashSignFromStr(objData["ProblemName"]);
        //--------------------------
        objData["commandType"] = "SAVE_PROBLEMLIST";
        objData["IsActiveGrid"] = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        objData["IsChiefComplaint"] = cc == true ? 1 : 0;

        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
            var cc = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #chkChiefComplaints").prop('checked');
            objData["IsChiefComplaint"] = cc == true ? 1 : 0;
            objData["NoteId"] = Clinical_ProblemLists.params.NotesId;
            objData["commandType"] = "SAVE_PROBLEMLIST";
        }
        objData["CheckProblemExists"] = "1";
        var isFavListOpened = $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").hasClass("toggledHor");
        objData['FavListNames'] = Clinical_ProblemLists.GetFavListStatusString(Clinical_ProblemLists.FavListName, isFavListOpened);
        objData['FavListName'] = Clinical_ProblemLists.FavListName;
        objData['FavListVal'] = PreFavVal;
        objData['UpdateFavValues'] = "1";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    GetFavListStatusString: function (favListName, isFavListOpened, FavListVal) {
        var commSeparatedFavListsStatus = "";
        if (isFavListOpened == true) {

            commSeparatedFavListsStatus = EMRUtility.setFavListStatus(favListName, true);
        }
        else {
            commSeparatedFavListsStatus = EMRUtility.setFavListStatus(favListName, false);
        }
        return commSeparatedFavListsStatus;
    },
    RemoveDashSignFromStr: function (str) {
        if (str != null && str.indexOf(' - ') > -1) {
            var strArray = str.split(' - ');
            return strArray[strArray.length - 1].trim();
        } else {
            return str;
        }
    },

    NoKnownProblem: function () {

        var strMessage = "";
        var self = $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists");
        var myJSON = '{}';

        Clinical_ProblemLists.SaveProblemLists(myJSON).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                $("#pnlProblemLists_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                Clinical_ProblemLists.ProblemListsSearch();
                //Clinical_ProblemLists.AddProblemOnDrFirst(response.ProblemListId);
                $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    callJs: function () {
        var objData = [];
        objData["commandType"] = "download_data";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "callJs");
    },

    UpdateProblemLists: function (ProblemListsData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListsData);
        if (objData.PatientId == '') {
            objData.PatientId = Clinical_ProblemLists.params.patientID;
        }

        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },
    // End Validate and save/edit functions

    // Search/Grid Load Functions
    //Adding Pagination on 04 Dec 2015 by Azhar
    ProblemListsSearch: function (ProblemListId, PageNo, rpp) {

        var strMessage = "";

        if ($("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result").css("display") == "none") {
            $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result").show();
        }

        //Start//21/12/2015//Ahmad Raza//Implimented Privileges for ProblemList Search
        //AppPrivileges.GetFormPrivileges("Medical_Problems List", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        Clinical_ProblemLists.SearchProblemList(ProblemListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Adding selection column of checkbox of Problem lists for Progress Notes on 04 Dec 2015 by Azhar
                if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                    if ($("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists thead tr #SelectRecord").length == 0) {
                        $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" id="chkHeaderProblemsList" onchange="Clinical_ProblemLists.checkUncheckAllProblemsList(this);"  class="input-block" coltype="checkbox"/> </th>');
                    }

                } else {
                    $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists th#SelectRecord").remove();
                }

                //Clinical_ProblemLists.ProblemListGridLoad(response);
                Clinical_ProblemLists.ProblemListGridLoadNew(response);
                //Adding Pagination on 04 Dec 2015 by Azhar
                //var TableControl = Clinical_ProblemLists.params.PanelID + " #dgvProblemLists";
                //var PagingPanelControlID = Clinical_ProblemLists.params.PanelID + " #dgvProblemLists_Paging";
                //var ClassControlName = "Clinical_ProblemLists";
                //var PagesToDisplay = 5;
                //var iTotalDisplayRecords = response.iTotalDisplayRecords;

                //setTimeout(CreatePagination(response.ProblemListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                //    Clinical_ProblemLists.ProblemListsSearch(PrimaryID, PageNumber, ResultPerPage);
                //}), 10);

                setTimeout(function () {
                    if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_ProblemLists.params.PanelID + "  #dgvProblemLists_Paging").html() == "") {
                        $("#" + Clinical_ProblemLists.params.PanelID + "  #dgvProblemLists_Paging").append('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_ProblemLists.addProblemListsToNotes();" id="btnAddProbListToNotes">Add on Note</button>');
                    }
                }, 11);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        //End//21/12/2015//Ahmad Raza//Implimented Privileges for ProblemList Search
    },

    //Start by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.
    checkUncheckAllProblemsList: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_ProblemLists.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", true);
        }
        else {
            $("#" + Clinical_ProblemLists.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", false);
        }

        $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tbody").find('input[type="checkbox"]').each(function () {
            Clinical_ProblemLists.enableAddProbList(this);
        });
    },
    //End by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.

    ProblemListGridLoad: function (response) {

        if (response.ProblemListCount > 0) {
            Clinical_ProblemLists.EditableGrid.datatable.clear().draw();

            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);

            $.each(ProblemListLoadJSONData, function (i, item) {

                var ProblemListId = item.ProblemListId;
                var CurrentRow = Clinical_ProblemLists.AddNewProblemListsRow(ProblemListId, "Edit", null);

                var self = $("#dgvProblemLists tr#" + ProblemListId);

                utility.bindMyJSONByName(true, item, false, self).done(function () { });

                var row = Clinical_ProblemLists.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();

                /********************************/

                row.child().loadDropDowns(true).done(function () {
                    utility.bindMyJSON(true, item, false, $(newChildRow));
                });
            });
        }
        else {
            $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").css("display", "none");
            $('#dgvProblemLists').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },

    SearchProblemList: function (ProblemListId, PageNumber, RowsPerPage, getInActiveProblems) {

        var IsCheckedIn = null;

        IsCheckedIn = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        // Patch for getting InActiveProblems // fixMe
        if (getInActiveProblems) {
            IsCheckedIn = getInActiveProblems;
        }

        //if (PageNumber == null) {
        //    PageNumber = 1;
        //}
        //if (RowsPerPage == null) {
        //    RowsPerPage = 15;
        //}

        var objData = new Object();

        objData["PatientId"] = Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = IsCheckedIn;
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    // Search/Grid Load Functions
    buildRowChild: function (obj, ParentRowId) {

        if (!ParentRowId) {
            ParentRowId = "";
        }

        var ChildHTML = $("<div></div>");
        var txtProblem = "<div class='col-xs-1'><label class='control-label'>Problem</label><input  class='form-control'type='text' id='txtProblem" + ParentRowId + "' name='Problem" + ParentRowId + "' disabled></div>";
        var txtDiagnosis = "<div class='col-xs-1'><label class='control-label'>ICD-9 ICD-10 Description</label><input class='form-control' id='Diagnosis" + ParentRowId + "' name='Diagnosis" + ParentRowId + "' type='text' /></div>";
        var txtChronicityLevel = "<div class='col-xs-1'><label class='control-label'>Chronicity Level</label><input class='form-control' id='txtChronicity" + ParentRowId + "' name='Chronicity" + ParentRowId + "' type='text' /></div>";
        var txtSeverity = "<div class='col-xs-1  size-min100'><label class='control-label'>Severity</label><input class='form-control' id='txtSeverity" + ParentRowId + "' name='Severity" + ParentRowId + "' type='text' /></div>";
        var ddlNDCMeasurement = "<div class='col-xs-2'><label class='control-label'>NDC Measurement Code</label><select id='ddlNDCMeasurement" + ParentRowId + "' name='ddlNDCMeasurement" + ParentRowId + "' class='form-control' ddlist='GetNDCMeasurementCode'></select></div>";
        var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea spellcheck='true' class='form-control' rows='1' id='txtComments" + ParentRowId + "' name='txtComments" + ParentRowId + "'></textarea></div>";
        var chkHold = "<div class='col-xs-1 pt-lg'><div class='checkbox-custom checkbox-default'><input type='checkbox' onclick=EncounterChargeCapture.validateIsHold(this,'divHoldDays" + ParentRowId + "') id='chkHold" + ParentRowId + "' value name='chkHold" + ParentRowId + "'/><label class='control-label'>Is Hold</label></div></div>";
        var HoldDays = "<div id='divHoldDays" + ParentRowId + "' style='display:none' class='col-xs-1'><label class='control-label'>Hold Days</label><input type='text' class='form-control' onfocusout=EncounterChargeCapture.validateHoldDays(this,'chkHold" + ParentRowId + "') id='txtHoldDays" + ParentRowId + "' data-mask='9?99' name='txtHoldDays" + ParentRowId + "'/></div>";
        var spacer = '<div class="spacer5"></div>';

        ChildHTML.append(txtProblem, txtDiagnosis, txtChronicityLevel, txtSeverity, ddlNDCMeasurement, LineNotes, chkHold, HoldDays, spacer);
        return ChildHTML;
    },

    //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
    AddNewProblemListsRow: function (RowId, mode, CurrRef, NotesId) {

        var CurrentRow = null;
        if (RowId && RowId > 0) {

            CurrentRow = Clinical_ProblemLists.EditableGrid.rowAdd(RowId, Clinical_ProblemLists.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = Clinical_ProblemLists.EditableGrid.rowAdd(TemplateRowId - 1, Clinical_ProblemLists.params.VitalSignsId, null, null, null, null, null);
            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        }

        var row = Clinical_ProblemLists.EditableGrid.datatable.row(CurrentRow);
        row.child(Clinical_ProblemLists.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        Clinical_ProblemLists.enableRemoveRow($(CurrentRow));
        return CurrentRow;
    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },

    // end editable grid functions
    OpenSearchPopup: function () {
        var controlToLoad = "";
        controlToLoad = "Admin_IMOICD";
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ProblemLists";
        params["RefCtrl"] = "txtProblems";

        $('#pnlClinicalProblemLists #txtProblems').attr('data-popupunload', 'true');

        params["Parent"] = 'pnlAdminIMOICD';
        HiddenCtrl = 'hfICD1-1,hfICDDescription1-1,hfICD101-1,hfICD10Description1-1,hfSNOMED1-1,hfSNOMEDDescription1-1';
        params["RefHiddenCtrl"] = HiddenCtrl;
        LoadActionPan(controlToLoad, params);
    },
    // editabele grid functions

    // Start 7/2/2016 Muhammad Ahmad Imran
    //Purpose Save/update favList Status
    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_ProblemLists.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_ProblemLists.FavListName, FavListVal);
        });
    },
    // End 7/2/2016 Muhammad Ahmad Imran

    rowSave: function ($row, obj) {

        //if (obj.rowValidate($row)) {

        var _self = obj,
        $actions,
        values = [];

        if ($row.hasClass('adding')) {
            $row.removeClass('adding');
        }

        values = $row.find('td').map(function () {

            var $this = $(this);

            if ($this.hasClass('expand')) {
                return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            }
            else if ($this.hasClass('actions')) {

                return _self.datatable.cell(this).data();
            }
            else if ($this.hasClass('ddl')) {
                return $.trim($this.find('select').val());

            } else {
                $obj_ = $this.find('input');

                if ($obj_.attr('type') == "checkbox") {
                    if ($obj_.prop('checked'))
                        return $.trim("True");
                    else
                        return $.trim("False");
                }
                else
                    return $.trim($obj_.val());
            }
        });

        var id = $row.attr("id");

        var myJSON = $row.getMyJSONByName();
        //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
        var NotesId = $row.attr("problemlistnotesid");
        //End//31/12/2015//Ahmad Raza//Bug#178 fixed


        var objData = JSON.parse(myJSON);


        $row.find("select[id*=ddlDiagnosis] option").each(function () {
            var opVal = $(this).val();
            var selVal = objData["Description"];
            if ($(this).val() == objData["Description"]) {
                objData["ICD9"] = $(this).attr("icd9code");
                objData["ICD10"] = $(this).attr("icd10code");
                objData["ICD9_Description"] = $(this).attr("icd9desc");
                objData["ICD10_Description"] = $(this).attr("icd10desc");
                objData["SNOMEDID"] = $(this).attr("snomedcode");
                objData["SNOMED_DESCRIPTION"] = $(this).attr("snomeddesc");
                return false;
            }

        });

        myJSON = JSON.stringify(objData);

        if (id && id > 0) {

            //Edit Record
            var strMessage = "";
            //Start//22/12/2015//Ahmad Raza//Logic implemented for Privileges
            AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_ProblemLists.UpdateProblemListsRow(myJSON, id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            //Serialization
                            //  $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').data('serialize', $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').serialize());

                            //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                            //Start//31/12/2015//Ahmad Raza//Logic to update against current Note only
                            if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote" && NotesId.indexOf(Clinical_ProblemLists.params.NotesId) > -1) {
                                //Ends//31/12/2015//Ahmad Raza//Logic to update against current Note only
                                Clinical_ProblemLists.getProblemListsInfo(id);

                            }
                                //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                            $("#mainForm  li#CDSAlert").show();
                            $.when(setPatientBanner($('#PatientProfile #hfPatientId').val(), "1")).then(function () {
                                if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                                    Clinical_ProgressNote.LoadCDSAlerts();
                                    Clinical_ProgressNote.AttachedNoteComponentIds.push(id);
                                }
                            });

                            Clinical_ProblemLists.ProblemListsSearch();

                            Clinical_ProblemLists.UpdateProblemOnDrFirst("UpdateProblemInDrFirstForGrid");
                            //Clinical_ProblemLists.rowDraw($row, _self, values);
                            //  Clinical_ProblemLists.ProblemListsSearch();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
            //End//22/12/2015//Ahmad Raza//Logic implemented for Privileges
        }

    },

    rowDetail: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            Clinical_ProblemLists.ShowHistory(currentProblemListId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProblemListId > 0) {
            Clinical_ProblemLists.ShowHistory(currentProblemListId);
        }
    },
    rowAdd: function () {
        //Start//21/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
                $("#mainForm  li#CDSAlert").show();
                $.when(setPatientBanner($('#PatientProfile #hfPatientId').val(), "1")).then(function () {
                    if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote")
                        Clinical_ProgressNote.LoadCDSAlerts();
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Privileges logic implemented
    },

    rowRemove: function ($row, obj) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var strMessage = "";
        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        //Start//15/1/2016//M Ahmad Imran//DrFirst Integration Changes
                        var description;
                        if ($row.find("td:nth-child(4)").html() != "") {
                            description = $row.find("td:nth-child(4)").html();
                        }
                        else {
                            description = $row.find("td:nth-child(3)").html();
                        }
                        //End//15/1/2016//M Ahmad Imran//DrFirst Integration Changes
                        Clinical_ProblemLists.DeleteProblemList(selectedValue, description, $row.find("td:nth-child(7)").html()).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                utility.DisplayMessages(response.Message, 1);
                                $("#mainForm  li#CDSAlert").show();
                                $.when(setPatientBanner($('#PatientProfile #hfPatientId').val(), "1")).then(function () {
                                    if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote")
                                        Clinical_ProgressNote.LoadCDSAlerts();
                                });
                                Clinical_ProblemLists.UpdateProblemOnDrFirst("DeleteProblemFromDrFirst");
                                // Begin Edited by Azeem Raza Tayyab on 15-Sep-2016 to Fix bug EMR-1358
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + selectedValue).length != 0) {
                                    Clinical_ProblemLists.detachProblemsListFromNotes_DBCall(selectedValue).done(function (response) {
                                        response = JSON.parse(response);
                                        if (response.status != false) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + selectedValue).remove();
                                            Clinical_ProgressNote.saveComponentSOAPText('Problems');
                                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                }
                                //End fixes related to EMR-1358
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                            //Start//28/12/2015//Ahmad Raza// No Known ProblemList hyperlink(link not visible when there is no problem list) issue fixed
                            Clinical_ProblemLists.ProblemListsSearch();
                            //End//28/12/2015//Ahmad Raza// No Known ProblemList hyperlink(link not visible when there is no problem list) issue fixed

                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented

    },

    rowInactive: function ($row, obj) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var strMessage = "";
        var id = $row.attr("id");

        var IsActive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        if (IsActive == "1") {
            IsActive = "0";
        } else {
            IsActive = "1"
        }

        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var IsActiveRecord = null;
                        IsActiveRecord = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
                        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_ProgressNote.AttachedNoteComponentIds.push(selectedValue);
                        }
                        if (IsActiveRecord == "1") {
                            var params = [];
                            var PanelID = "";

                            if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                                params["ParentCtrl"] = 'Clinical_ProblemLists';
                                PanelID = 'pnlClinicalProgressNote #pnlClinicalProblemLists';
                            }
                            else if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
                                params["ParentCtrl"] = 'Clinical_ProblemLists';
                                PanelID = 'pnlClinicalFaceSheet #pnlClinicalProblemLists';
                            }
                            else if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_Treatment") {
                                params["ParentCtrl"] = 'Clinical_ProblemLists';
                                PanelID = 'pnlClinicalTreatment #pnlClinicalProblemLists';
                            }
                            else {
                                params["ParentCtrl"] = 'clinicalTabProblemLists';
                                PanelID = 'pnlClinicalProblemLists';
                            }
                            params["ProblemListId"] = selectedValue;
                            //Start//Ahmad Raza//15/12/2015//InActive PopUp issue in ProgressNote Resolved
                            params["FromAdmin"] = "0";
                            //params["ParentCtrl"] = "Clinical_ProblemLists";
                            //End//Ahmad Raza//15/12/2015//InActive PopUp issue in ProgressNote Resolved
                            params["PatientId"] = Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                            LoadActionPan('Clinical_ProblemListInActive', params, PanelID);
                        } else {
                            IsActiveRecord = "0";
                            Clinical_ProblemLists.InActiveProblemList(selectedValue, null, null).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Clinical_ProblemLists.ProblemListsSearch();
                                    Clinical_ProblemLists.UpdateProblemOnDrFirst("inactive_problemlistFromDrfirst");

                                }
                                else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                            });
                        }
                    }
                }, function () { },
                    '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },

    InActiveProblemList: function (ProblemListId, comments, endDate) {

        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsActiveRecord == "1")
            IsActiveRecord = "0";
        else if (IsActiveRecord == "0")
            IsActiveRecord = "1";

        //     var ProblemListId = Clinical_ProblemLists.params.ProblemListId;
        var patientId = Clinical_ProblemLists.params.PatientId;

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["PatientId"] = patientId;
        objData["InActiveChkBoxValue"] = null;
        objData["InActiveReason"] = null;
        objData["EndDate"] = null;
        objData["IsActive"] = IsCheckedIn;
        objData["IsActiveRecord"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    rowCancel: function ($row, obj) {


        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);

        //Start//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes
        if (Clinical_ProblemLists.params.ActionPanContainer == "actionPanClinicalProgressNote") {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:nth-child(2) .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:nth-child(2) .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        else {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        //End//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes


    },

    ShowHideEditableGridRows: function (isShow) {

        var VitalsGridId = "#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Clinical_ProblemLists.EditableGrid.datatable.row(parentRow);

            if (isShow == true) {

                row.child.show();
                $(parentRow).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");

            }
            else {

                $(parentRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();

            }

        });

    },

    // end editable grid functions
    UpdateProblemListsRow: function (ProblemListData, ProblemListId) {

        var isactive = null;
        isactive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProblemListData);
        objData["ProblemListId"] = ProblemListId;
        objData["Comments"] = objData["hfComments"];
        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["NoteId"] = Clinical_ProblemLists.params.NotesId;
        }
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    //
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = Clinical_ProblemLists.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.ProblemId);

                currentChildRow.attr("parentvitalid", ParentRowId);
                currentChildRow.addClass("childRow-bg");
                $(currentChildRow).find("td:nth-child(1)").html("");
                $(currentChildRow).find("td:nth-child(2)").html("");
                $(currentChildRow).find("td:nth-child(3)").html("");
                //var currentChild = Clinical_Vitals.FillCurrentRow(currentChildRow, item, row, true);
                CurrentRowchilds = CurrentRowchilds.add(currentChildRow);
            });
            row.child(CurrentRowchilds).show();
            setTimeout(function () {
                row.child.hide();
            }, 100);

        }
        else {
            $(CurrentRow).find("td:nth-child(1)").html("");
        }

        return row.child();
    },
    DeleteProblemList: function (ProblemListId, Description, StartDate) {

        var objData = new Object();
        objData["ProblemListId"] = ProblemListId;
        objData["commandType"] = "DELETE_PROBLEMLIST";
        objData["PatientId"] = Clinical_ProblemLists.params.patientID;
        objData["Description"] = Description;
        objData["StartDate"] = StartDate;


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },
    problemDetail: function (problemListId, event) {
        event.stopPropagation();
        var params = [];
        params["FromAdmin"] = "0";
        params["ProblemListId"] = problemListId;
        params["IsUpdate"] = true;
        params["ParentCtrl"] = "Clinical_ProblemLists";

        LoadActionPan("Clinical_ProblemDetails", params);

    },
    // problem list grid load as per admin

    ProblemListGridLoadNew: function (response) {
        var isactive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');

        // get Actions
        var actions = "";
        $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result");
                }
            }
        });

        ////Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        //if ($.fn.dataTable.isDataTable("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnClearTable();
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists_filter").remove();
        //}

        //if ($("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody tr").length > 0) {
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnClearTable();
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
        //    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists_filter").remove();
        //}


        if ($.fn.dataTable.isDataTable("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
            $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnClearTable();
            $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
            $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == 'dgvProblemLists') {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
                    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").parent().parent().find('div.row').remove();
                }
            })
        }



        var totalProblems = 0;
        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        if (response.ProblemListCount > 0) {
            totalProblems = response.ProblemListCount;
            //$('#' + Clinical_ProblemLists.params.PanelID + ' div#divShowHistory').removeClass("hidden");
            var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
            var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

            if ($.fn.dataTable.isDataTable("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
                $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
            }
            //tem array to hold rows and childs
            var arraTemp = [];
            var originalActions = actions;


            $.each(ProblemListLoadJSONData, function (i, item) {
                actions = originalActions;
                if (item.IsCancerDisease == "True") {
                    actions += '<a id="cancerIcon" href="#" class="btn-xs mr-none btn" onclick=" Clinical_ProblemLists.problemDetail(' + item.ProblemListId + ',event) " title="Cancer Disease" ><i class="fa fa-star" aria-hidden="true"></i></a>';
                }
                var $infoButtonrow = "";
                if (item.Description != "") {
                    if (typeof item.Description !== 'undefined' && typeof item.Description.split(' - ')[1] !== 'undefined') {
                        var searchstr = item.Description.split('-')[0].trim();
                        if (item.Description.split(" - ")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf(" - ") + 2);
                        }
                        $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "Clinical_ProblemLists", 2);
                    }
                }
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ProblemListId);
                //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                $row.attr("ProblemListNotesId", item.NoteId);
                //End//31/12/2015//Ahmad Raza//Bug#178 fixed

                $row.attr("name", "Problems");
                var depressionlnk = "", diabeteslnk = "", tobacolnk = "";
                if (item.IsDepressionConfig == "1") {
                    depressionlnk = '<a href=\"#\" onclick=\"Clinical_ProblemLists.OpenDepressionScreen(' + item.ProviderId + ',' + item.PatientId + ');\"  title=\"View Depression Screening\"><b>Depression Screening</b></a><br/>';
                }
                if (item.IsDiabetesConfig == "1") {
                    diabeteslnk = "<a href='#' onclick='Clinical_ProblemLists.OpenDiabetesScreen(" + item.ProviderId + ',' + item.PatientId + ")'><b>Diabetes Screening</b></a><br/>";
                }
                if (item.IsTobaccoConfig == "1") {
                    tobacolnk = '<a href=\"#\" onclick=\"Clinical_ProblemLists.OpenTobaccoScreen(' + item.ProviderId + ',' + item.PatientId + ');\"  title=\"View Tobacco Screening\"><b>Tobacco Screening</b></a><br/>';
                }
                var color = "";

                // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold"'
                }
                if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold"'
                }
                if (item.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold"'
                }

                var comments = "";
                var commentsMethod = "Clinical_ProblemLists.AddComments('" + item.ProblemListId + "');";
                if (item.IsActive.toLowerCase() == "false") {
                    if (item.InActiveReason.length > 0) {
                        comments = '<a  href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></a>';
                    } else {
                        comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></a>';
                    }

                }
                else {
                    //if (item.Comments != "") {
                    // var commentsMethod = "Clinical_ProblemLists.AddComments('" + item.ProblemListId + "');";
                    //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                    comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></a>';
                    //}

                }
                actions += comments;
                var ProblemListId = item.ProblemListId;
                var ChildHistory_ProblemList = $.grep(ProblemListHistoryLoadJSONData, function (n, i) {
                    return n.ProblemId == ProblemListId;
                });
                if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote" && item.ProblemName == "No Known Problems") {
                    //$row.append('<td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                    var SelectionCheckBoxColumn = "";
                    var Checked = "";
                    if (item.IsNoteLinked == "True") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            Checked = " ";
                        } else {
                            Checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                    }

                    if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="text-center" onchange="Clinical_ProblemLists.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                    } else {
                        SelectionCheckBoxColumn = "";
                    }
                    if (ChildHistory_ProblemList.length > 0) {
                        $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td><td style="display:none">' + item.ProblemOrder + '</td>');
                    } else {
                        $row.append(SelectionCheckBoxColumn + '<td></td><td></td><td>' + item.ProblemName + $infoButtonrow + '</a></td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td><td style="display:none">' + item.ProblemOrder + '</td>');
                    }
                    //End//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row
                }
                else {


                    if (item.ProblemName.toLowerCase() == "No Known Problems".toLowerCase()) {
                        $row.append('<td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + item.ProblemName + $infoButtonrow + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center" style="display:none;"> ' + depressionlnk + tobacolnk + ' </td><td id="hfComments' + item.ProblemListId + '"  style="display:none"><input type="hidden" name="hfComments"  value="' + item.Comments + '"/></td><td style="display:none">' + item.ProblemOrder + '</td>');
                    }
                    else {
                        //adding checkboxes column and disabling that row, if problem list already binded with notes
                        var SelectionCheckBoxColumn = "";
                        var Checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                        }

                        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                            SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" onchange="Clinical_ProblemLists.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        } else {
                            SelectionCheckBoxColumn = "";
                        }
                        if (ChildHistory_ProblemList.length > 0) {
                            $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center" style="display:none;"> ' + depressionlnk + tobacolnk + ' </td><td id="hfComments' + item.ProblemListId + '"  style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td><td style="display:none">' + item.ProblemOrder + '</td>');
                        } else {
                            $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.ProblemListId + '" >' + actions + '</td><td>' + item.ProblemName + $infoButtonrow + '</td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center" style="display:none;"> ' + depressionlnk + tobacolnk + ' </td><td id="hfComments' + item.ProblemListId + '"  style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td><td style="display:none">' + item.ProblemOrder + '</td>');
                        }
                    }
                }
                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tbody").last().append($row);
                if (totalProblems > 1) {
                    $('#' + Clinical_ProblemLists.params.PanelID + ' #dgvProblemLists tbody tr').each(function () {
                        if ($(this).text().indexOf("No Known Problems") > -1) {
                            $(this).remove();
                        }
                    });
                }

                var CurrentRowchilds = $();

                if (ChildHistory_ProblemList.length > 0) {
                    $.each(ChildHistory_ProblemList, function (i, item) {
                        // if (item.ProblemId == ProblemListId) {
                        //arrProblemListHistory.push(item);
                        if (item.Description.split("-")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf("-") + 2);
                        }
                        comments = "";
                        if (item.IsActive.toLowerCase() == "false") {
                            if (item.InActiveReason.length > 0) {
                                comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                            } else {
                                comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                            }
                        }
                        else {
                            var commentsMethod = "Clinical_ProblemLists.AddComments('" + item.ProblemListId + "');";
                            //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }





                        var Title_Tooltip = "Inactive Reason: " + item.InActiveChkBoxValue + (item.EndDate != '' ? " <br/>End Date: " + utility.RemoveTimeFromDate(null, item.EndDate) : "") + (item.InActiveReason != '' ? " <br/>Comments: " + item.InActiveReason : "");
                        var IsActiveText = "";
                        if (item.IsActive == "True") {
                            IsActiveText = "<Label>[Active]</Label>";
                        } else {
                            IsActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + Title_Tooltip + "'>[Inactive]</Label>";
                        }

                        // Start 27/11/2015 Muhammad Irfan Change color of severity

                        var colorChild = "";

                        // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                        if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent") {
                            colorChild = 'style = "color:green;font-weight:bold"'
                        }
                        if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity") {
                            colorChild = 'style = "color:red;font-weight:bold"'
                        }
                        if (item.Severity == "Moderate Persistent") {
                            colorChild = 'style = "color:orange;font-weight:bold"'
                        }

                        // End 27/11/2015 Muhammad Irfan Change color of severity

                        //Start//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed
                        if (Clinical_ProblemLists.params.ActionPanContainer == "actionPanClinicalProgressNote") {
                            var currentHistory = '<tr class="childRow-bg"><td></td><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center" style="display:none;"> ' + depressionlnk + tobacolnk + ' </td><td id="hfComments' + item.ProblemListId + '"  style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td></tr>';

                        }
                        else {

                            var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item.ProblemListId + '" ></td><td>' + IsActiveText + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + colorChild + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center" style="display:none;"> ' + depressionlnk + tobacolnk + ' </td><td id="hfComments' + item.ProblemListId + '" style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td></tr>';
                        }
                        //End//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed

                        //arrProblemListHistory.push(currentHistory);
                        CurrentRowchilds = CurrentRowchilds.add(currentHistory);

                        // }
                    });
                }

                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });
                Clinical_ProblemLists.showHideUpDown($row, $row.prev());
            });

            //Inalize grid
            var PanelGrid = "#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result";
            var GridId = "#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists";

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (Clinical_ProblemLists.myGrid != null) {
                //  Clinical_ProblemLists.myGrid.$table.find("tbody tr").remove();
                // Clinical_ProblemLists.myGrid.$table.dataTable().fnClearTable()
                if ($.fn.dataTable.isDataTable(Clinical_ProblemLists.myGrid)) {
                    Clinical_ProblemLists.myGrid.$table.dataTable().fnDestroy();
                } else {
                    Clinical_ProblemLists.myGrid = null;
                }
                //  Clinical_ProblemLists.myGrid.datatable.clear().draw();
                if ($.fn.dataTable.isDataTable("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
                    $("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
                }
            }
            //End By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            //   if ($.fn.dataTable.isDataTable("#" + Clinical_ProblemLists.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists") == false) {
            Clinical_ProblemLists.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_ProblemLists, 0, false, true, false, true, false, null);
            //  $("#" + Clinical_ProblemLists.params.PanelID + " div.mystuff").attr('id', 'divSwitch');
            //   }

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (Clinical_ProblemLists.myGrid != null) {
                    var row = Clinical_ProblemLists.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Start//04//01//2015//Ahmad Raza//Sorting removed from first column of grid
            // $('#dgvProblemLists').dataTable().fnSettings().aoColumns[0].bSortable = false;
            if ($('#' + Clinical_ProblemLists.params.PanelID + ' #dgvProblemLists').length > 0) {
                $('#' + Clinical_ProblemLists.params.PanelID + ' #dgvProblemLists').dataTable().fnSettings().aoColumns[0].bSortable = false;
                if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                    $('#' + Clinical_ProblemLists.params.PanelID + ' #dgvProblemLists').dataTable().fnSort([[12, "desc"]]);
                } else {
                    $('#' + Clinical_ProblemLists.params.PanelID + ' #dgvProblemLists').dataTable().fnSort([[11, "desc"]]);
                }

            }
            //End//04//01//2015//Ahmad Raza//Sorting removed from first column of grid


            /* Start of Code for making No Known Problem List hyperlink inline with checkbox and search box.
             *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
             */
            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }

            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_ProblemLists.ActiveProblemSearch(this);">' +
                          '</div><span class="pl-xs">Active</span>' +
          '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + Clinical_ProblemLists.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            //  if ($('#pnlClinicalProblemLists #switchActive').is(':checked') || $("#pnlProblemLists_Result #btnNoKnownProblems").is(':visible')) {
            if ($('#' + Clinical_ProblemLists.params.PanelID + ' div#divShowHistory').hasClass("hidden") == false) {
                $('#' + Clinical_ProblemLists.params.PanelID + ' div#divShowHistory').addClass("hidden");
            }

            $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #dgvProblemLists').DataTable({
                "language": {
                    "emptyTable": "No Problem List Found."
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
            var checked = '';
            if (isactive == "0") {
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_ProblemLists.ActiveProblemSearch(this);">' +
                         '</div><span class="pl-xs">Active</span>' +
         ' <a id="btnNoKnownProblems" class="ml-md btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + Clinical_ProblemLists.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            if (response.ProblemListCount == 0 && $("#switchActive").attr('isactive') != "0") {
                $("#pnlProblemLists_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProblemLists_Result #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */

        }


        EMRUtility.SwicthWidgetInializatoin();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        // $('#dgvProblemLists thead tr th:first-child').removeClass('sorting_asc');
        $('#' + Clinical_ProblemLists.params.PanelID + ' #dgvProblemLists thead tr th:first-child').removeClass('sorting_asc');
        //End//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        //Editable Grid
        //var PanelGrid = "#pnlClinicalProblemLists #pnlProblemLists_Result";
        //var GridId = "#pnlClinicalProblemLists #dgvProblemLists";
        //EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_ProblemLists, 0);

    },

    //
    UnLoadTab: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        // Ast 357 
        if (Clinical_ProblemLists.params.CurrentNotesProviderId)
            Clinical_ProblemLists.params.CurrentNotesProviderId = "";
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
            utility.UnLoadDialog(Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists', function () {
                if (Clinical_ProblemLists.params["FromAdmin"] == "0") {
                    if (Clinical_ProblemLists.params != null && Clinical_ProblemLists.params.ParentCtrl != null) {
                        if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
                            parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                            Clinical_FaceSheet.params.ChildPanelID = null;
                            UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists', null, parentPanelId);
                        } else {
                            UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_ProblemLists');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_problems');
            }, function () {
                if (Clinical_ProblemLists.params["FromAdmin"] == "0") {
                    if (Clinical_ProblemLists.params != null && Clinical_ProblemLists.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_ProblemLists');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
                EMRUtility.scrollToPNcomponent('clinical_problems');
            });

            Clinical_FaceSheet.loadFaceSheet();

        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_ProblemLists.addProblemListsToNotes();
            UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
            EMRUtility.scrollToPNcomponent('clinical_problems');
        }
        else {
            if (Clinical_ProblemLists.params["FromAdmin"] == "0") {
                if (Clinical_ProblemLists.params != null && Clinical_ProblemLists.params.ParentCtrl != null) {
                    if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_Treatment") {
                        $("#" + Clinical_Treatment.params.PanelID + " #problemSection").addClass("active");
                        $("#" + Clinical_Treatment.params.PanelID + " #pnlProblemLists_ResultT").css("display", "block");
                        Treatment_ProblemListGrid.ProblemListsSearch();
                    }
                    UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
                }
                else
                    UnloadActionPan(null, 'Clinical_ProblemLists');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_problems');
        }
        return objDeffered;
    },

    //Comments Update

    AddComments: function (ProblemListId) {


        var params = [];
        var PanelID = "";
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'Clinical_ProblemLists';
            PanelID = 'pnlClinicalProgressNote #pnlClinicalProblemLists';
        }
            // Start By Babur on 2/15/2016 - Bug EMR-329 - FaceSheet in clinical module -> Problem list -> Edit comments
        else if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet") {
            params["ParentCtrl"] = 'Clinical_ProblemLists';
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalProblemLists';
        }
        else if (Clinical_ProblemLists.params.ParentCtrl == "Clinical_Treatment") {
            params["ParentCtrl"] = 'Clinical_ProblemLists';
            PanelID = 'pnlClinicalTreatment #pnlClinicalProblemLists';
        }
            // End By Babur on 2/15/2016 - Bug EMR-329 - FaceSheet in clinical module -> Problem list -> Edit comments
        else {
            params["ParentCtrl"] = 'clinicalTabProblemLists';
            PanelID = 'pnlClinicalProblemLists';
        }

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */

        params["ProblemListId"] = ProblemListId;
        //Start//Ahmad Raza//15/12/2015//Comments Popup issue in ProgressNote, Resolved
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "Clinical_ProblemLists";
        //End//Ahmad Raza//15/12/2015//Comments Popup issue in ProgressNote, Resolved
        params["PatientId"] = Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        LoadActionPan('Clinical_ProblemListsComments', params, PanelID);
    },

    //

    ActiveProblemSearch: function (objThis) {


        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        Clinical_ProblemLists.ProblemListsSearch();
    },

    // Start 27/11/2015 Muhammad Irfan Bug # 93,94
    ResetDiagnosis: function () {
        var PrevFavVal = "-1";
        if (Clinical_ProblemLists != null && typeof Clinical_ProblemLists != typeof undefined) {
            PrevFavVal = $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').val();
        }

        //if ($("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val() == "") {
        $('#' + Clinical_ProblemLists.params.PanelID + ' #frmClinicalProblemLists').resetAllControls(null);
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtDiagnosis").prop("disabled", true);
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ddlChronicityLevel").prop("disabled", true);
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ddlSeverity").prop("disabled", true);
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #dpStartDate").prop("disabled", true);
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #dpEndDate").prop("disabled", true);
        $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtComments").prop("disabled", true);
        if (PrevFavVal != "-1") {
            $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').val(PrevFavVal);
            $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').trigger("onchange");
        }

        //}
    },
    // End 27/11/2015 Muhammad Irfan Bug # 93,94

    //-----------------Progress Note-------------
    // added on Dec 04,2015 by Muhammad Azhar Shahzad
    //Call Back function to add component to Progress Note
    addProblemListsToNotes: function () {
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        var strMessage = "";
        //Start//22/12/2015//Ahmad Raza//Logic implemented for Privileges
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var SelectedProblemLists = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                //if (SelectedProblemLists != null && SelectedProblemLists != '') {
                //    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                //        var PLid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                //        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + PLid).length != 0) {
                //            var index = SelectedProblemLists.indexOf(PLid);
                //            if (index > -1) {
                //                SelectedProblemLists.splice(index, 1);
                //            }
                //        }
                //    }
                //}
                var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
                if (detachedvalues.join() != '') {

                    Clinical_ProblemLists.detachProblemsListFromNotes(detachedvalues).done(function () {
                        if (SelectedProblemLists.join() != null && SelectedProblemLists.join() != '') {
                            Clinical_ProblemLists.attachProblemsListFromNotes(SelectedProblemLists);
                        } else {
                            Clinical_ProgressNote.saveComponentSOAPText('Problems');
                        }
                    });
                }
                else if (SelectedProblemLists.join() != null && SelectedProblemLists.join() != '') {
                    Clinical_ProblemLists.attachProblemsListFromNotes(SelectedProblemLists);
                }
                //When User has attached Allergies with notes than add on note button should be disabled
                $("#" + Clinical_ProblemLists.params.PanelID + "  #dgvProblemLists_Paging #btnAddProbListToNotes").prop('disabled', true);
                Clinical_ProgressNote.EnableDisableCancerReportButton();
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
        if (Clinical_ProblemLists.params && Clinical_ProblemLists.params.ParentCtrl && Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {


            UnloadActionPan(Clinical_ProblemLists.params.ParentCtrl, 'Clinical_ProblemLists');
            EMRUtility.scrollToPNcomponent('clinical_problems');

        }
    },

    attachProblemsListFromNotes: function (SelectedProblemLists) {
        SelectedProblemLists = $.unique(SelectedProblemLists);
        Clinical_ProblemLists.getProblemListsInfo(SelectedProblemLists.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_ProblemLists.params != null && Clinical_ProblemLists.params.length > 0 && Clinical_ProblemLists.params.PanelID.indexOf('pnlClinicalProblemLists') != -1) {
                    Clinical_ProblemLists.ProblemListsSearch();
                }
            }, 5);
        });
    },

    detachProblemsListFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_ProblemLists.detachProblemsListFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var PLid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + PLid).remove();
                }


                Clinical_ProgressNote.HideShowBillingInfo();
                //Clinical_ProgressNote.saveComponentSOAPText("Problems");
                //setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                //if (Clinical_ProblemLists.params != null && Clinical_ProblemLists.params.PanelID.indexOf('pnlClinicalProblemLists') != -1) {
                //    Clinical_ProblemLists.ProblemListsSearch();
                //}
                //   utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    //this function will get Problem Lists Soap Text and attach that to Progress note
    getProblemListsInfo: function (ProblemListsId, hideAlertMessage) {
        if (ProblemListsId == null || ProblemListsId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_ProblemLists.get_ProblemLists_ForSOAP(ProblemListsId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.ProblemListSoapCount > 0) {

                        //Start//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                        Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ProblemListsId, hideAlertMessage);
                        //End//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed

                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    //This Function will check, if Problem Lists Soap is already attached in Progress note, if Problem Lists is not attached than it will create main divs to attach allergy
    checkProblemListExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #AssessmentNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ProblemsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_problems title="Problems"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Problems\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Problem">Problems</a> ' +
                        '<a onclick="Clinical_ProgressNote.openProblemLists();" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="OpenProblemLists" name=""><i class="fa fa-caret-down orange" aria-hidden="true"></i></a>' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Problems\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Problems\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_problems> </header></li>');
            $(CompnentSelector).find('li.ProblemsComponent').attr('IsCancerReportPrinted', false);

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });


        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

    createProblemListBodyHTMLFromNotes: function (Problems, Attached_Problems, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        Clinical_ProblemLists.checkProblemListExists();

        var $mainDivVital = $(document.createElement('div'));

        if (!Problems) {
            return;
        }

        if (Problems.length > 0) {
            $.each(Problems, function (index, element) {
                var color = "";
                var $infoButtonrow = "";
                var PLid = element.ProblemListId;
                var $SectionBodyVital = $(document.createElement('section'));
                var $DetailsDiv = $(document.createElement('div'));
                var $ListProblem = $(document.createElement('ul'));

                $ListProblem.attr('class', 'list-unstyled')
                $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('problemOrder', element.ProblemOrder);
                $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('IsCancerDisease', element.IsCancerDisease);

                $DetailsDiv.attr('id', "Cli_ProblemList_" + PLid);

                if (element.Description != "") {
                    var searchstr = element.Description.split('-')[0].trim();
                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "clinicalTabProgressNote", 2, "", "Clinical_ProblemLists");
                }
                // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                if (element.Severity == "Mild Intermittent" || element.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold" ';
                }
                if (element.Severity == "Severe Persistent" || element.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold" ';
                }
                if (element.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold" ';
                }

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProblemList_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Problems_Main" + PLid + '"  ><i class="fa fa-times"></i></a><a href="javascript:void(0);" class="btn-xs on-default up-row" title="Up Record" onclick="Clinical_ProgressNote.problemUp(\'' + PLid + '\')"><i class="fa fa-arrow-up black"></i></a>'
                    + '<a href="javascript:void(0);" class="btn-xs on-default down-row" title="Down Record" onclick="Clinical_ProgressNote.problemDown(\'' + PLid + '\')"><i class="fa fa-arrow-down black"></i></a></div> ');

                var StartDateEndDate = "";
                if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " and ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.StartDate == '' || element.StartDate == null) && (element.EndDate != '' && element.EndDate != null)) {
                    StartDateEndDate = (element.EndDate == '' ? "" : " ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.EndDate == '' || element.EndDate == null) && (element.StartDate != '' && element.StartDate != null)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                    StartDateEndDate = "";
                }
                else if (element.StartDate == element.EndDate) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                var inActiveText = "";
                if (element.IsActive != null && element.IsActive == "False") {
                    inActiveText = " : <span style = 'color:red;font-weight:bold'> (Inactive) </span>";
                }

                var CodeDescription = element.Description == '' ? "" : element.Description;
                $ListProblem.append("<li><strong> " + CodeDescription + inActiveText + " " + $infoButtonrow + " </strong>" + (element.ChronicityLevel == '' ? "" : " Chronicity: " + element.ChronicityLevel) + ((element.Severity && element.ChronicityLevel) ? ", " : "") +
                    (element.Severity == '' ? "" : "<span " + color + '>Severity: ' + element.Severity + "</span> ") +
                    StartDateEndDate + (element.ModifiedOn == '' ? "" : " Modified on " + utility.RemoveTimeFromDate(null, element.ModifiedOn) + ".")
                    );
                // MU3-32 Faizan AMeen

                $ListProblem.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
                if (element.IsCancerDisease == "True") {
                    if (element.DiagnosisConfirmation != "") {

                        var ProblemDetailcolor = 'style = "color:#444444;font-weight:bolder" ';
                        $ListProblem.append("<li id='CancerDetailList'><span " + ProblemDetailcolor + ">Diagnosis Confirmation- </span>   Status:" + (element.CancerIsActive == "True" ? "Active" : "Inactive") + ",Diagnosis:" + element.Description + ",Diagnosis Date:" + utility.RemoveTimeFromDate(null, element.CancerDiagnosisDate) + ", Diagnostic Confirmation:" + element.DiagnosisConfirmation + ","
                        + "Primary Site:" + element.PrimarySite + ", Laterality:" + element.Laterality + ", Histologic Type:" + element.HistologicType + ", Behavior:" + element.Behavior + ", Grade:" + element.Grade +
    "<br/><span " + ProblemDetailcolor + ">TNM Clinical Stage Information- </span>" + (element.NKOClinical == "True" ? "No Clinical Stage Information" : "Diagnosis Date:" + utility.RemoveTimeFromDate(null, element.CancerClinicalDiagnosisDate) + ", TNM Stage Group:" + element.ClinicalStageGroup + ", TNM Stage Descriptor:" + element.ClinicalStageDescriptor +
    ", Primary Clinical Tumor:" + element.PrimaryClinicalTumor + ", Regional Lymph Nodes Clinical:" + element.RLNC + ",Distance Mestastatases" + element.DistanceMestastatases + ", Stager Clinical Cancer :" + element.StagerClinicalCancer) +
    "<br/><span " + ProblemDetailcolor + ">TNM Pathologic Stage Information- </span>" + (element.NKOPathologic == "True" ? "No Pathologic Stage Information" : "Effective  Date:" + utility.RemoveTimeFromDate(null, element.CancerEffectiveDate) + ", TNM Pathologic Stage Group:" + element.PathologicStageGroup + ", TNM Pathologic Stage Descriptor:" + element.PathologicStageDescriptor +
    ", Primary Tumor Pathologic:" + element.PrimaryTumorPathologic + ", Regional Lymph Nodes Pathologic:" + element.RLNP + ",Distance Mestastatases Pathologic:" + element.DistanceMestastatasesPathologic + ", Stager Pathologic Cancer:" + element.StagerPathologicCancer) + "<br/>"
    )
                    }
                    else {

                        $SectionBodyVital.find('#Cli_Problems_Main' + PLid).removeAttr('IsCancerDisease');
                        $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('IsCancerDisease', 'False');
                    }

                }



                $DetailsDiv.append($ListProblem);
                $SectionBodyVital.append($DetailsDiv);

                if ($(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).length == 0) {
                    $mainDivVital.append($SectionBodyVital);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul').append(CommentHTML);;
                }

            });


            Clinical_ProgressNote.sort_problemsDesc();
            Clinical_ProgressNote.showHideUpDownProb();
            if ($mainDivVital.html() != '') {
                Clinical_ProblemLists.setProblemListHtml($mainDivVital.html(), NoteHTMLCtrl, Attached_Problems, bNotSaveCompt);
            } else {
                Clinical_ProblemLists.setProblemListHtml('', NoteHTMLCtrl, bNotSaveCompt, Attached_Problems);
                Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
            }
        }
    },

    setProblemListHtml: function (ProblemListHtml, NoteHTMLCtrl, Attached_Problems, bNotSaveCompt) {

        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('initialVisitBody');
        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('probList');

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();

        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' clinical_problems').parent().parent().append(ProblemListHtml);

            if (Attached_Problems.length >= 0) {
                if (!bNotSaveCompt)
                    Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage)
                Clinical_ProgressNote.HideShowBillingInfo();
            }
        }
    },

    updateProblemListHtmlFromNote: function (ProblemListHtml, ProblemListId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('initialVisitBody');
        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' clinical_problems').parent().parent().append(ProblemListHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createProblemListBodyHTML: function (response, NoteHTMLCtrl, ProblemListsId, hideAlertMessage, fromResult, fromEsuperBill, previousProblemsTextObj) {
        Clinical_ProblemLists.checkProblemListExists();
        var ProblemListSoap_JSON = JSON.parse(response.ProblemListSoap_JSON);
        var $mainDivVital = $(document.createElement('div'));
        if (ProblemListSoap_JSON == null || ProblemListSoap_JSON.length == 0) {
            Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
            return "";
        }
        var PListId = [];
        if (response.ProblemListSoapCount > 0) {
            var CancerDiseaceCount = 0;
            $.each(ProblemListSoap_JSON, function (index, element) {
                var color = "";
                var $infoButtonrow = "";
                if (element.Description != "") {
                    var searchstr = element.Description.split('-')[0].trim();
                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(searchstr, "clinicalTabProgressNote", 2, "", "Clinical_ProblemLists");
                }
                // Start 26/11/2015 Muhammad Irfan Bug # EMR-80
                if (element.Severity == "Mild Intermittent" || element.Severity == "Mild Persistent") {
                    color = 'style = "color:green;font-weight:bold" ';
                }
                if (element.Severity == "Severe Persistent" || element.Severity == "Unspecified Severity") {
                    color = 'style = "color:red;font-weight:bold" ';
                }
                if (element.Severity == "Moderate Persistent") {
                    color = 'style = "color:orange;font-weight:bold" ';
                }

                var PLid = element.ProblemListId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('problemOrder', element.ProblemOrder);


                $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('IsCancerDisease', element.IsCancerDisease);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ProblemList_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProblemList_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Problems_Main" + PLid + '"  ><i class="fa fa-times"></i></a><a href="javascript:void(0);" class="btn-xs on-default up-row" title="Up Record" onclick="Clinical_ProgressNote.problemUp(\'' + PLid + '\')"><i class="fa fa-arrow-up black"></i></a>'
                    + '<a href="javascript:void(0);" class="btn-xs on-default down-row" title="Down Record" onclick="Clinical_ProgressNote.problemDown(\'' + PLid + '\')"><i class="fa fa-arrow-down black"></i></a></div> ');

                var StartDateEndDate = "";
                if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " and ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.StartDate == '' || element.StartDate == null) && (element.EndDate != '' && element.EndDate != null)) {
                    StartDateEndDate = (element.EndDate == '' ? "" : " ended on " + utility.RemoveTimeFromDate(null, element.EndDate) + ".");
                }
                else if ((element.EndDate == '' || element.EndDate == null) && (element.StartDate != '' && element.StartDate != null)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " Started on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                    StartDateEndDate = "";
                }
                else if (element.StartDate == element.EndDate) {
                    StartDateEndDate = (element.StartDate == '' ? "" : " on " + utility.RemoveTimeFromDate(null, element.StartDate) + ".");
                }
                var inActiveText = "";
                if (element.IsActive != null && element.IsActive == "False") {
                    inActiveText = " : <span style = 'color:red;font-weight:bold'> (Inactive) </span>";
                }
                var CodeDescription = element.Description == '' ? "" : element.Description;
                $ListVital.append("<li><strong> " + CodeDescription + inActiveText + " " + $infoButtonrow + " </strong>" + (element.ChronicityLevel == '' ? "" : " Chronicity: " + element.ChronicityLevel) + ((element.Severity && element.ChronicityLevel) ? ", " : "") +
                    (element.Severity == '' ? "" : "<span " + color + '>Severity: ' + element.Severity + "</span> ") +
                    StartDateEndDate +
                    //------------------------------------
                    /*(element.StartDate == '' ? "" : ", Start on " + utility.RemoveTimeFromDate(null, element.StartDate)) +
                    (element.EndDate == '' ? "" : ", End on " + utility.RemoveTimeFromDate(null, element.EndDate)) +*/
                    //-----------------------------------
                    (element.ModifiedOn == '' ? "" : " Modified on " + utility.RemoveTimeFromDate(null, element.ModifiedOn) + ".")
                    );

                $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
                if (fromEsuperBill) {
                    if (previousProblemsTextObj.length > 0) {
                        var editorComments = $(previousProblemsTextObj).find('div[id=Cli_ProblemList_' + PLid + '] ul li[id=Comments_Cli_ProblemList_' + PLid + ']');
                        if (editorComments)
                            $ListVital.append(editorComments);
                    }
                }
                // MU3-32 Faizan Ameen

                $DetailsDiv.append($ListVital);


                if (element.IsCancerDisease == "True") {

                    if (element.DiagnosisConfirmation != "") {
                        var ProblemDetailcolor = 'style = "color:#444444;font-weight:bolder" ';
                        $DetailsDiv.append("<li id='CancerDetailList'><span " + ProblemDetailcolor + ">Diagnosis Confirmation- </span>   Status:" + (element.CancerIsActive == "True" ? "Active" : "Inactive") + ",Diagnosis:" + element.Description + ",Diagnosis Date:" + utility.RemoveTimeFromDate(null, element.CancerDiagnosisDate) + ", Diagnostic Confirmation:" + element.DiagnosisConfirmation + ","
                        + "Primary Site:" + element.PrimarySite + ", Laterality:" + element.Laterality + ", Histologic Type:" + element.HistologicType + ", Behavior:" + element.Behavior + ", Grade:" + element.Grade +
    "<br/><span " + ProblemDetailcolor + ">TNM Clinical Stage Information- </span>" + (element.NKOClinical == "True" ? "No Clinical Stage Information" : "Diagnosis Date:" + utility.RemoveTimeFromDate(null, element.CancerClinicalDiagnosisDate) + ", TNM Stage Group:" + element.ClinicalStageGroup + ", TNM Stage Descriptor:" + element.ClinicalStageDescriptor +
    ", Primary Clinical Tumor:" + element.PrimaryClinicalTumor + ", Regional Lymph Nodes Clinical:" + element.RLNC + ",Distance Mestastatases" + element.DistanceMestastatases + ", Stager Clinical Cancer :" + element.StagerClinicalCancer) +
    "<br/><span " + ProblemDetailcolor + ">TNM Pathologic Stage Information- </span>" + (element.NKOPathologic == "True" ? "No Pathologic Stage Information" : "Effective  Date:" + utility.RemoveTimeFromDate(null, element.CancerEffectiveDate) + ", TNM Pathologic Stage Group:" + element.PathologicStageGroup + ", TNM Pathologic Stage Descriptor:" + element.PathologicStageDescriptor +
    ", Primary Tumor Pathologic:" + element.PrimaryTumorPathologic + ", Regional Lymph Nodes Pathologic:" + element.RLNP + ",Distance Mestastatases Pathologic:" + element.DistanceMestastatasesPathologic + ", Stager Pathologic Cancer:" + element.StagerPathologicCancer) + "<br/>"
    )

                        CancerDiseaceCount++;
                        if (CancerDiseaceCount == 1) {
                            Clinical_ProblemLists.EnableDisableCancerReportButton();
                        }

                    }
                    else {
                        $SectionBodyVital.find('#Cli_Problems_Main' + PLid).removeAttr('IsCancerDisease');
                        $SectionBodyVital.attr('id', "Cli_Problems_Main" + PLid).attr('IsCancerDisease', 'False');

                    }
                }

                $SectionBodyVital.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_ProblemList').parent().parent().find('#Cli_Problems_Main' + PLid).length == 0) {
                if ($(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).length == 0) {
                    PListId.push(PLid);
                    $mainDivVital.append($SectionBodyVital);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_problems').parent().parent().find('#Cli_Problems_Main' + PLid + ' ul').append(CommentHTML);;
                }
            });
            //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if (PListId.join(",") != "") {
                ProblemListsId = PListId.join(",");
            }
            Clinical_ProgressNote.sort_problemsDesc();
            Clinical_ProgressNote.showHideUpDownProb();
            //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if ($mainDivVital.html() != '') {
                //Start//04//01//2015//Ahmad Raza//
                Clinical_ProblemLists.updateProblemListHtml($mainDivVital.html(), ProblemListsId, NoteHTMLCtrl, hideAlertMessage, null, fromResult);
            } else {
                Clinical_ProblemLists.updateProblemListHtml('', ProblemListsId, NoteHTMLCtrl, hideAlertMessage, null, fromResult);
                Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
            }
        } else {

            Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
        }
    },
    EnableDisableCancerReportButton: function () {
        //  $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').addClass("disabled");
        var VisitType = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ddlVisitType option:selected').text().trim();
        if (VisitType.toLowerCase().indexOf("cancer") >= 0)
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #btnCancerReport').removeClass("disabled");

    },
    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    updateProblemListHtml: function (ProblemListHtml, ProblemListId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt, fromResult) {
        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('initialVisitBody');
        $(NoteHTMLCtrl + ' clinical_problems').parent().parent().addClass('probList');
        if (ProblemListHtml != '') {
            $(NoteHTMLCtrl + ' clinical_problems').parent().parent().append(ProblemListHtml);
        }
        if (ProblemListId != null && typeof ProblemListId != "string") {
            ProblemListId = ProblemListId.toString();
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (fromResult) {
            Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage);
        }
        else if (ProblemListHtml != '') {
            Clinical_ProblemLists.attachProblemListignFromNotes(ProblemListId, hideAlertMessage, bNotSaveCompt);
        }

    },

    //This Functions ask for Detaching Vital sign from Progress Note for current Patient Selected
    detachProblemListFromNotes: function (ProblemsListId) {
        var dfd = new $.Deferred();
        var strMessage = "";
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Problems List", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    EMRUtility.scrollToPNcomponent('clinical_problems');
                    var selectedValue = ProblemsListId.replace('Cli_Problems_Main', '');
                    if (selectedValue == "" || selectedValue == "undefined") {
                        dfd.resolve('ok');
                    }
                    else {
                        Clinical_ProblemLists.detachProblemsListFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $('#' + ProblemsListId).remove();
                                $.when(Clinical_ProgressNote.saveComponentSOAPText('Problems')).then(function () {
                                    Clinical_ProgressNote.HideShowBillingInfo();
                                    dfd.resolve('ok');
                                    //  Clinical_ProgressNote.EnableDisableCancerReportButton();
                                });
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                //   utility.DisplayMessages(response.Message, 1);


                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                                dfd.resolve('ok');
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve('ok');
            }

        });

        return dfd.promise();
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    //This Function detach Problem list From progress note
    detach_ComponentsProblemList: function (ComponentName, IsUpdate, ProblemListComponentRemove) {

        var Clinical_ProblemListIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').parent().parent().find('section[id*="Cli_Problems_Main"]').map(function () {
            return this.id.replace("Cli_Problems_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ProblemsComponent').attr('NoteComponentId');
        if (ProblemListComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_problems').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Problems', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_problems').parent().parent().remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Problem Lists']").remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Problem Lists']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_problems').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Problems', true));
                }
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
                $.when.apply($, promise).done(function () {
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                });
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_problems').parent().parent().find('section[id*="Cli_Problems_Main"]').remove();
        }

        if (Clinical_ProblemListIds == "" || Clinical_ProblemListIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_problems').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Problems', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_problems').parent().parent().remove();
                Clinical_ProgressNote.HideShowBillingInfo();
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            Clinical_ProblemLists.detachProblemsListFromNotes_DBCall(Clinical_ProblemListIds).done(function (response) {

                response = JSON.parse(response);

                if (response.status != false) {
                    var detachedvalues = Clinical_ProblemListIds.split(',');
                    $.each(detachedvalues, function (index, value) {
                        var PLid = detachedvalues[index];
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Problems_Main' + PLid).remove();
                    });
                    if (IsUpdate) {

                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Problems', true)).then(function () {

                            Clinical_ProgressNote.HideShowBillingInfo();
                        });
                    }

                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //This Functions attached Vital sign to Progress Note for current Patient Selected
    attachProblemListignFromNotes: function (ProblemsListId, hideAlertMessage, bNotSaveCompt) {
        if (ProblemsListId == "" || ProblemsListId == "undefined") {
        }
        else {
            Clinical_ProblemLists.attachProblemsListWithNotes_DBCall(ProblemsListId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //var attachedVitals = JSON.parse(response.VitalsLoad_JSON);

                    //If Attached Vitals Made new inseration to Vitals Table than good ids should be attached to HTML
                    //  Clinical_ProgressNote.ChangeHTML(response);
                    if (!bNotSaveCompt)
                        Clinical_ProgressNote.saveComponentSOAPText('Problems', hideAlertMessage)
                    Clinical_ProgressNote.HideShowBillingInfo();

                    // Grid row was removing which was attaching to Note
                    //   $('#' + ProblemsListId).remove();

                    // utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //This Function enable/disable add to note button
    enableAddProbList: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id);
            } if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlProblemLists_Result #chkHeaderProblemsList').prop('checked', false);
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }
        var isactive = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if ((Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0)) {//&& isactive == "1") {
            $("#" + Clinical_ProblemLists.params.PanelID + "  #dgvProblemLists_Paging #btnAddProbListToNotes").prop('disabled', false);
        } else {
            $("#" + Clinical_ProblemLists.params.PanelID + "  #dgvProblemLists_Paging #btnAddProbListToNotes").prop('disabled', true);
        }

    },

    //If ProblemLists Component which is dropeed in Progress note has no ProblemList attached, than it will call for Latest ProblemList for this patient
    getLatestProblemListByPatientId: function (hideAlertMessage, droppedComponent) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            Clinical_ProblemLists.getLatestProblemListByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);



                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    getRcopiaInformaionForProblemListSoap: function () {
        Clinical_ProblemLists.getLatestProblemListByPatientId();
    },
    //-----Server calls of Notes----------
    attachProblemsListWithNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "attach_problemlists_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    detachProblemsListFromNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "detach_problemlists_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    get_ProblemLists_ForSOAP: function (ProblemListsId) {

        var objData = new Object();
        objData["ProblemListId"] = ProblemListsId;
        objData["commandType"] = "get_problemlists_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    getLatestProblemListByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];

        if (Clinical_ProgressNote.params.EnrollmentInfoId != null && Clinical_ProgressNote.params.EnrollmentInfoId != "")
            objData["commandType"] = "getlatest_chronicproblemlistby_patientid";
        else
            objData["commandType"] = "getlatest_problemlistby_patientid";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    getinfobuttondetials: function (searchstr) {
        var params = [];
        params["FromAdmin"] = "0";
        params["codeSystem"] = "2.16.840.1.113883.6.90";
        params["searchStr"] = searchstr;
        params["PatientId"] = Clinical_ProblemLists.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = Clinical_ProblemLists.params.TabID;
        params["UserName"] = globalAppdata["AppUserName"];
        LoadActionPan('Clinical_InfoButtonView', params);
    },

    BindFavProblems: function () {

        var FavoriteListId = $('#' + Clinical_ProblemLists.params.PanelID + ' #ddlFavProblems').val();
        var SerachData = $('#' + Clinical_ProblemLists.params.PanelID + ' #FavSearchBox').val();
        if (FavoriteListId != "") {

            Favorite_Problems.searchFavoriteList_ICD_DBCall(null, FavoriteListId, null, null, SerachData).done(function (response) {

                response = JSON.parse(response);

                if (response.status != false) {

                    if (response.FavoriteListICDCount > 0) {

                        $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                        var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                        var li = "";

                        $.each(FavoriteListJSON, function (i, item) {

                            if (typeof item.ICD9Code == 'undefined' || item.ICD9Code == null) { item.ICD9Code = ''; }
                            if (typeof item.ICD10Code == 'undefined' || item.ICD10Code == null) { item.ICD10Code = ''; }
                            if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) { item.SNOMEDID = ''; }
                            if (typeof item.ICD10CodeDescription == 'undefined' || item.ICD10CodeDescription == null) { item.ICD10CodeDescription = ''; }

                            var diagnosis = item.ICD10Code + " - " + item.ICD10CodeDescription;
                            var ICD9Code = "" + item.ICD9Code + "";
                            var ICD10Code = "" + item.ICD10Code + "";
                            var ICD9CodeDescription = "" + item.ICD9CodeDescription + "";
                            var ICD10CodeDescription = "" + item.ICD10CodeDescription + "";
                            var SNOMEDID = "" + item.SNOMEDID + "";
                            var SNOMEDDescription = "" + item.SNOMEDDescription + "";

                            li += "<li  id=" + item.FavoriteListICDId + " onclick='Clinical_ProblemLists.PopulateFields(this,\"" + diagnosis + "\",\"" + ICD9Code + "\",\"" + ICD10Code + "\",\"" + ICD9CodeDescription + "\",\"" + ICD10CodeDescription + "\",\"" + SNOMEDID + "\",\"" + SNOMEDDescription + "\");' ><a href='#' class='pr-sm'>" + item.ICD10Code + " - " + item.ICD10CodeDescription + "</a></li>";
                        });

                        $('#pnlClinicalProblemLists #ulFavCompliantDisease').append(li);

                        //Save
                    }
                    else {
                        $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                    }
                }
                else {
                    $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

        //on change, set controls to default state.
        $("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val('');
        $("#" + Clinical_ProblemLists.params.PanelID + " #txtDiagnosis").val('');

        if ($("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val() == "") {
            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtComments,#txtDiagnosis,#ddlChronicityLevel,#ddlSeverity").prop("disabled", true);
            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #dpStartDate,#dpEndDate").prop("disabled", true);
        }

        if ($('#pnlClinicalProblemLists #ddlFavProblems').val() == '' || $('#pnlClinicalProblemLists #ddlFavProblems').val() == '- Select -') {
            $('#pnlClinicalProblemLists #ulFavCompliantDisease li').remove();
        }
    },

    PopulateFields: function (cntrl, diagnosis, ICD9Code, ICD10Code, ICD9CodeDescription, ICD10CodeDescription, SNOMEDID, SNOMEDDescription) {

        $('#pnlClinicalProblemLists #txtProblems').val($(cntrl).text());
        $('#pnlClinicalProblemLists #txtDiagnosis').val(diagnosis);

        var lii = "<li icd9Code=\"" + ICD9Code + "\" icd9Desc=\"" + ICD9CodeDescription + "\" icd10Code=\"" + ICD10Code + "\" icd10Desc=\"" + ICD10CodeDescription + "\" snomedCode=\"" + SNOMEDID + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#' class='pr-sm'>" + ICD10CodeDescription + "</a></li>"
        $('#pnlClinicalProblemLists #ulProblemDisease').html(lii);

        if (typeof $("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val() != 'undefined'
            && $("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val() != null
            && $("#" + Clinical_ProblemLists.params.PanelID + " #txtProblems").val() != ""
            && typeof $("#" + Clinical_ProblemLists.params.PanelID + " #txtDiagnosis").val() != 'undefined'
            && $("#" + Clinical_ProblemLists.params.PanelID + " #txtDiagnosis").val() != null
            && $("#" + Clinical_ProblemLists.params.PanelID + " #txtDiagnosis").val() != "") {

            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #txtComments,#ddlChronicityLevel,#ddlSeverity").prop("disabled", false);
            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #dpStartDate,#dpEndDate").prop("disabled", false);
            $('#hfIMOProblem').val($(cntrl).text());

            $("#" + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');
        }

        Clinical_ProblemLists.ProblemListsSave();

        if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_ProblemLists.params.PanelID + "  #dgvProblemLists_Paging #btnAddProbListToNotes").length == 0) {
            Clinical_ProblemLists.addProblemListsToNotes();
        }
    },

    domReadyFunc: function () {

        $(document).ready(function () {

            $('#' + Clinical_ProblemLists.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });

            //for autocomplete zIndex fix
            $("#txtProblems").on("autocompleteopen", function (event, ui) {
                if ($(this).closest(".modal-dialog").length == 0)
                    $(this).autocomplete('widget').zIndex("1018");
            });
        });

    },

    logViewProblem: function (problemId) {


        var objData = new Object();
        objData["ProblemListId"] = problemId;
        objData["commandType"] = "logviewproblemlist";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    rowUp: function ($row, obj) {

        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {
                var from_Id = $row.attr("Id");
                var to_Id = "";

                if ($row.prev().attr("Id").match(/[a-z]/i)) {
                    to_Id = $row.prev().prev().prev().attr("Id");
                }
                else {
                    to_Id = $row.prev().attr("Id");
                }

                if (from_Id && to_Id && from_Id != undefined && to_Id != undefined) {
                    Clinical_ProblemLists.changeProblemsOrder(from_Id, to_Id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            /************* HIDING CHILD ROW *******************/
                            var row = obj.datatable.row($row);

                            if (row.child.isShown()) {

                                $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                                row.child.hide();
                            }
                            /*************************************************/
                            // Draw row on grid

                            var prevRow = "";

                            if ($row.prev().attr("Id").match(/[a-z]/i)) {
                                prevRow = $row.prev().prev().prev().get(0);
                            }
                            else {
                                prevRow = $row.prev().get(0);
                            }

                            EditableTable.rowSwap(prevRow, $row.get(0));
                            //need to review next line tommorow
                            Clinical_ProblemLists.showHideUpDown($row, $row.next());
                            if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                                var $from = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #Cli_Problems_Main' + from_Id);
                                var $to = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #Cli_Problems_Main' + to_Id);
                                if ($from && $to) {
                                    Clinical_ProgressNote.swapElements($from, $to);
                                    Clinical_ProgressNote.saveComponentSOAPText('Problems');
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowDown: function ($row, obj) {

        AppPrivileges.GetFormPrivileges("Medical_Problems List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var from_Id = $row.attr("Id");
                var to_Id = "";

                if ($row.next().attr("Id").match(/[a-z]/i)) {
                    to_Id = $row.next().next().next().attr("Id");
                }
                else {
                    to_Id = $row.next().attr("Id");
                }

                if (from_Id && to_Id && from_Id != undefined && to_Id != undefined) {
                    var data = "FromElementId=" + from_Id + "&ToElementId=" + to_Id;
                    Clinical_ProblemLists.changeProblemsOrder(from_Id, to_Id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            // Draw row on grid
                            /************* HIDING CHILD ROW *******************/
                            var row = obj.datatable.row($row);

                            if (row.child.isShown()) {

                                $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                                row.child.hide();

                            }
                            /*************************************************/

                            var NextRow = "";

                            if ($row.next().attr("Id").match(/[a-z]/i)) {
                                NextRow = $row.next().next().next().get(0);
                            }
                            else {
                                NextRow = $row.next().get(0);
                            }

                            EditableTable.rowSwap($row.get(0), NextRow);

                            Clinical_ProblemLists.showHideUpDown($row, $row.prev());
                            if (Clinical_ProblemLists.params.ParentCtrl == "clinicalTabProgressNote") {
                                var $from = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #Cli_Problems_Main' + from_Id);
                                var $to = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #Cli_Problems_Main' + to_Id);
                                if ($from && $to) {
                                    Clinical_ProgressNote.swapElements($from, $to);
                                    Clinical_ProgressNote.saveComponentSOAPText('Problems');
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                            }
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    showHideUpDown: function ($row, $RowSwapedWith) {
        var totalArrayLength = $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr").length - 2;
        var rowIndex = $.inArray($row[0], $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr")) - 1;
        var RowSwapIndex = $.inArray($RowSwapedWith[0], $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr")) - 1;

        if (rowIndex == 0) {
            $row.find("td.actions a.up-row").hide();
            $row.find("td.actions a.down-row").show();
        }
        if (rowIndex > 0) {
            $row.find("td.actions a.up-row").show();
            $row.find("td.actions a.down-row").show();
        }
        if (rowIndex == totalArrayLength) {
            $row.find("td.actions a.up-row").show();
            $row.find("td.actions a.down-row").hide();
        }
        if (RowSwapIndex == 0) {
            $RowSwapedWith.find("td.actions a.up-row").hide();
            $RowSwapedWith.find("td.actions a.down-row").show();
        }
        if (RowSwapIndex > 0) {
            $RowSwapedWith.find("td.actions a.up-row").show();
            $RowSwapedWith.find("td.actions a.down-row").show();
        }
        if (RowSwapIndex == totalArrayLength) {
            $RowSwapedWith.find("td.actions a.up-row").show();
            $RowSwapedWith.find("td.actions a.down-row").hide();
        }
        $("#" + Clinical_ProblemLists.params.PanelID + " #dgvProblemLists tr").last().find("td.actions a.down-row").hide();
    },
    enableUpAndDown: function (CurrentRow) {
        CurrentRow.find("td.actions a.up-row,a.down-row").each(function () {
            $(this).removeClass('hidden')
        });
    },
    changeProblemsOrder: function (fromId, toId) {
        var objData = {};
        objData["commandType"] = "UPDATE_PROBLEM_ORDER";
        objData["FromElementId"] = fromId;
        objData["ToElementId"] = toId;
        objData["NoteId"] = Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },
    OpenDepressionScreen: function (ProviderId, PatientId) {
        var params = [];
        //params["NoteId"] = NoteId;
        params["ParentCtrl"] = "Clinical_ProblemLists";// Clinical_ProblemLists.params.ParentCtrl;
        params["PatientIds"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["Depression"] = true;
        LoadActionPan('VBP_MissingDataAlert', params, Clinical_ProblemLists.params.PanelID);
    },
    OpenDiabetesScreen: function (ProviderId, PatientId) {
        var params = [];
        //params["NoteId"] = NoteId;
        params["ParentCtrl"] = "Clinical_ProblemLists";
        params["PatientIds"] = PatientId;
        params["ProviderId"] = ProviderId;
        LoadActionPan('IA_DiabetesScreening', params);
    },
    OpenTobaccoScreen: function (ProviderId, PatientId) {
        var params = [];
        //params["NoteId"] = NoteId;
        params["ParentCtrl"] = "Clinical_ProblemLists";
        params["PatientIds"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["ParentPanelID"] = Clinical_ProblemLists.params.PanelID;
        LoadActionPan('IA_TabacooScreening', params, Clinical_ProblemLists.params.PanelID);
    },

}