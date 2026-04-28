Clinical_Procedures = {
    bIsFirstLoad: true,
    params: [],
    Procedures: [],
    ProceduresDetail: [],

    IsUpdate: false,
    EditableGrid: null,
    EditableGridAdd: null,
    myGrid: null,
    FavListName: "ClinicalProcedureDetail",
    PreviousComments: '',
    Load: function (params) {
        Clinical_Procedures.params = params;
        Clinical_Procedures.params.mode = "Add";
        Clinical_Procedures.favoriteListSearch();
        if (Clinical_Procedures.params.PanelID != 'pnlClinicalProcedures') {
            Clinical_Procedures.params.PanelID = Clinical_Procedures.params.PanelID + ' #pnlClinicalProcedures';
        } else {
            Clinical_Procedures.params.PanelID = 'pnlClinicalProcedures';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_Procedures.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        var PanelProcedureGrid = "#" + Clinical_Procedures.params.PanelID + " #pnlProcedure_ResultAdd";
        var ProcedureGridId = "#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd";
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures thead th#CQMEncounterTypeId").show();
        }
        else {
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures thead th#CQMEncounterTypeId").hide();
        }
        $(ProcedureGridId + " tbody tr").remove();
        Clinical_Procedures.EditableGridAdd = utility.MakeEditableGrid(PanelProcedureGrid, ProcedureGridId, Clinical_Procedures, "0", false, false, false, false);
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Procedures.params.PanelID, 'Medical', 'Procedures', 'Clinical_Procedures.unLoadTab();', null, true);
        }
        if (!$("#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd_wrapper").hasClass('no-footer height-max150 overflowY')) {
            $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd_wrapper").addClass('no-footer height-max150 overflowY')
        }
        //utility.CreateDatePicker('frmClinicalProcedures #dpStartDate', function () {
        //    //on-change callback method
        //},true);
        //utility.CreateDatePicker('frmClinicalProcedures #dpEndDate', function () {
        //    //on-change callback method
        //},true);



        ///* Start 22/12/2015 Muhammad Irfan for bug # EMR-131 */
        //utility.ValidateFromToDate('frmClinicalProcedures', 'dpStartDate', 'dpEndDate', true);
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-131 */
        Clinical_Procedures.domReadyFunc();
        var self = $('#' + Clinical_Procedures.params.PanelID);
        self.find('.Diagnosis > select').attr('ddlist', 'LookupProblemLists');
        var data = "IsActive=&ID=" + $('#PatientProfile #hfPatientId').val();
        self.find('.Diagnosis').loadDropDowns(true, data).done(function () {
        });
        var self = $('#' + Clinical_Procedures.params.PanelID);
        if (Clinical_Procedures.bIsFirstLoad == true) {
            //for Favorites toggle
            EMRUtility.setFavoriteSectionStyle(Clinical_Procedures.params.PanelID);


            Clinical_Procedures.bIsFirstLoad = false;
            //Serialization
            $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').data('serialize', $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').serialize());
            Clinical_Procedures.domReadyFunction();

        }
        // imp 720
        // Faizan ameen.

        Clinical_Procedures.ProceduresSearch();
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Procedures.params.PanelID, 'Medical', 'Procedures', 'Clinical_Procedures.unLoadTab();', null, true);
        }
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Procedures.params.PanelID + " div#FaceSheetPager", Clinical_Procedures.params.FaceSheetComponents, 'problem list');
        }
        Clinical_Procedures.Procedures = [];
        Clinical_Procedures.ProceduresDetail = [];
        //$("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr").each(function () {
        //    Clinical_Procedures.EditableGridAdd.datatable.row($(this).get(0)).remove().draw();
        //});
        //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-771
        if ($.fn.dataTable.isDataTable("#" + Clinical_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd")) {
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd").dataTable().fnClearTable();
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd").dataTable().fnDestroy();
        }
        //  $("#" + Clinical_Allergies.params.PanelID + " #pnlAllergies_Result #dgvAllergies").dataTable().fnDestroy();
        $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedure_ResultAdd #dgvProcedureAdd tbody").find("tr").remove();
        //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-771


        if (EMRUtility.getFavListStatus(Clinical_Procedures.FavListName)) {
            $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #favSectionDiv").addClass("toggledHor");
            $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #favSectionDiv").removeClass("toggledHor");
            $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #FormDiv").removeClass("toggleHorContainer");
        }
        //End 2-07-2016 M Ahmad Imran for favorite list setting for all favLists

        //End 2-07-2016 M Ahmad Imran for favorite list setting for all favLists
    },

    //Start//03/22/2016//M Ahmad Imran//Implimented ready function which run at load time
    domReadyFunc: function () {

        $('#' + Clinical_Procedures.params.PanelID + ' #sectionProceduresDetails').on('keydown', '#UnitId', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });

    },

    //Author: Muhammad Ahmad Imran
    //Date :  03-31-2016
    //Reason: Function to load procedure Order Fav List
    favoriteListSearch: function () {
        Favorite_ProcedureOrder.searchFavoriteList_DBCall("Procedure", null, 1, 5000, "", true).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Clinical_Procedures.params.PanelID + ' #ddlFavoriteListProcedure');
                var favouriteProcedures = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                $ddl.append(
                         $('<option/>', {
                             value: "",
                             html: "- Select -",
                         })
                       );
                $.each(favouriteProcedures, function (i, item) {
                    if (item.Name != "") {
                        $ddl.append(
                          $('<option/>', {
                              id: item.FavoriteListId,
                              value: item.FavoriteListId,
                              html: item.Name,
                          })
                        );
                    }
                });
                if (favouriteProcedures.length > 0) {
                    EMRUtility.getFavListValue(Clinical_Procedures.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "") {
                                if ($("#" + Clinical_Procedures.params.PanelID + " #ddlFavoriteListProcedure option[value='" + response1.favListVal + "']").length > 0) {
                                    $ddl.val(response1.favListVal);
                                    $ddl.trigger("onchange");
                                }
                                else {
                                    if (favouriteProcedures.length == 1) {
                                        $ddl.val(favouriteProcedures[0].FavoriteListId);
                                        $ddl.trigger("onchange");
                                    }
                                    else if (favouriteProcedures.length > 1) {
                                        $ddl.trigger("onchange");
                                    }
                                }
                            }
                            else {
                                if (favouriteProcedures.length == 1) {
                                    $ddl.val(favouriteProcedures[0].FavoriteListId);
                                    $ddl.trigger("onchange");
                                }
                                else if (favouriteProcedures.length > 1) {
                                    $ddl.trigger("onchange");
                                }
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });
    },
    AutoSearchFavProcedure: function () {
        utility.Keyupdelay(function () {
            Clinical_Procedures.loadfavoriteListContent();
        });
    },
    //Author: Muhammad Ahmad Imran
    //Date :  03-31-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Clinical_Procedures.params.PanelID + ' #ddlFavoriteListProcedure');
        }
        var SearchData = $('#' + Clinical_Procedures.params.PanelID + ' #FavSearchBox').val();
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            if (selectedOption.val() != "") {
                Clinical_Procedures.favoriteList_CPTSearch(selectedOption.attr("id"), SearchData);
            }
            else {
                var $UL = $('#' + Clinical_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent');
                $UL.empty();
            }
        }
    },

    //Author: Muhammad Ahmad Imran
    //Date :  03-31-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + Clinical_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent li').each(function (i, item) {
            $(item).trigger("click");
        });


    },

    //Author: Muhammad Ahmad Imran
    //Date :  03-31-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        var $UL = $('#' + Clinical_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent');
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);
                $UL.empty();

                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'Clinical_Procedures.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + item.SNOMEDID + '\',\'' + String(item.SNOMED_DESCRIPTION) + '\',\'' + String(item.Unit) + '\',\'' + String(item.Modifier) + '\')';
                        $UL.append('<li id="' + item.CPTCode + '" onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + '</li>');
                    }
                });



            }
            else {
                $UL.empty();
            }
        });
    },
    DisableFavProcedure: function (ProcedureId) {

        $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures ul#ulFavoriteListProcedureContent li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == ProcedureId) {
                $(this).addClass("disableAll");
            }
        });
    },
    EnableFavProcedure: function (ProcedureDes) {
        //ProcedureDes1 = ProcedureDes.split(" ");
        //ProcedureDes = "";
        //for (var i = 1; i < ProcedureDes1.length; i++) {
        //    if (i != 1) {
        //        ProcedureDes = ProcedureDes + " " + ProcedureDes1[i];
        //    }
        //    else {
        //        ProcedureDes = ProcedureDes + ProcedureDes1[i];
        //    }

        //}
        ProcedureDes = ProcedureDes.split("(")[0].trim();
        $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures ul#ulFavoriteListProcedureContent li").each(function (i, item) {
            var text = $(this).html();
            if ($(this).html() == ProcedureDes) {
                $(this).removeClass("disableAll");
            }
        });
    },
    EnableAllFavProcedure: function () {

        $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures ul#ulFavoriteListProcedureContent li").each(function (i, item) {

            if ($(this).hasClass('disableAll')) {
                $(this).removeClass("disableAll");
            }
        });
    },
    //New functions added by Zeeshan



    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SnomedID, SnomedDescription, unit, modifier) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd tbody tr");
        var isTestAlreadySelected = false;
        Clinical_Procedures.DisableFavProcedure(cptCode);
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val().replace(cptCode, "").trim() : "";
            if (currentRowCPTDescription.indexOf("(SCT") > 0) {
                currentRowCPTDescription = currentRowCPTDescription.substring(0, currentRowCPTDescription.indexOf("(SCT")).trim();
            }
            //var currentRowCPTDescription = currentRowCPTDescription.split('(')[0].trim();
            if (cptDescription.toLowerCase().trim() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {

            Clinical_Procedures.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription, unit, modifier);
            setTimeout(function () {
                $("#" + Clinical_Procedures.params.PanelID + " #txtProcedureCPTCode").val('');
                $('input[id*=txtUnit]').on("input", function () {
                    var val = Math.abs(parseInt(this.value, 10) || 1);
                    this.value = val > 999 ? 999 : val;
                });
            }, 200);
        }
        else {

            // faizan ameen
            // dated: 25-oct-2016
            // EMR-1691
            utility.myConfirm('This code already exists, do you want to add this code again?', function () {
                Clinical_Procedures.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription, unit, modifier);
                setTimeout(function () {
                    $("#" + Clinical_Procedures.params.PanelID + " #txtProcedureCPTCode").val('');
                    $('input[id*=txtUnit]').on("input", function () {
                        var val = Math.abs(parseInt(this.value, 10) || 1);
                        this.value = val > 999 ? 999 : val;
                    });
                }, 200);

            },
            function () {
                $("#" + Clinical_Procedures.params.PanelID + " #txtProcedureCPTCode").val('');
                return false;
            },
            'Confirmation duplicate CPT');

            // utility.DisplayMessages("Procedure is already selected", 2);
        }
    },

    validateUnits: function (self) {

        var value = $(self).val();
        var val = Math.abs(parseInt(value, 10) || 1);
        value = val > 999 ? $(self).val(999) : $(self).val(val);
        //if ($(id).attr('id').indexOf('txtModifiertxtpUnits') > -1) {

        //}
        //else {
        //    $(id).val(value);
        //}


    },

    resetValues: function () {
        //$('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #sectionProceduresDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio]').each(function () {
        //    $(this).val('');
        //});
        //$('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #sectionProceduresDetails").find('select').each(function () {
        //    $(this).val('0');
        //});
        //$('#' + Clinical_Procedures.params.PanelID + ' #Diagnosis').val($('#' + Clinical_Procedures.params.PanelID + ' #Diagnosis option:first').val());
        $('#' + Clinical_Procedures.params.PanelID + ' #sectionProceduresDetails').resetAllControls(null);
        utility.CreateDatePicker('frmClinicalProcedures #dpStartDate', function () {
            //on-change callback method
        });
        utility.CreateDatePicker('frmClinicalProcedures #dpEndDate', function () {
            //on-change callback method
        });


    },


    bindAutoComplete: function (element) {


        var hiddenCrtl = $('#' + Clinical_Procedures.params.PanelID + ' #txtCPTCode');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_Procedures", null, true);

    },

    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_Procedures";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_Procedures.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_Procedures.params.PanelID);
    },

    validateProcedure: function () {
        //if ($('#' + Clinical_Procedures.params.PanelID + ' #txtCPTCode').val() == "") {
        //    return true;
        //}
        //else if ($('#' + Clinical_Procedures.params.PanelID + ' #hfCPTCode').val() + " - " + $('#' + Clinical_Procedures.params.PanelID + ' #hfCPTDescription').val() != $('#' + Clinical_Procedures.params.PanelID + ' #txtCPTCode').val()) {
        //    utility.DisplayMessages("Procedure not Valid", 2);
        //    $('#' + Clinical_Procedures.params.PanelID + ' #txtCPTCode').val('');
        //    return false;
        //}
        //else
        //    return true;

        var cptDescription = $('#' + Clinical_Procedures.params.PanelID + ' #hfCPTDescription').val();
        var cptCode = $('#' + Clinical_Procedures.params.PanelID + ' #hfCPTCode').val();
        var $txtCptCode = $('#' + Clinical_Procedures.params.PanelID + ' #txtCPTCode');
        if ($txtCptCode.val() == "") {
            return true;
        }
        //Start//2/02/2016//Abid Ali//added Logic to compare txtCPT values with IMO
        if (cptCode == "") {
            if (cptDescription != $txtCptCode.val()) {
                utility.DisplayMessages("Procedure not Valid", 2);
                $txtCptCode.val('');
                return false;
            }
        }
            //End//2/02/2016//Abid Ali//added Logic to compare txtCPT values with IMO
        else if (cptCode + " - " + cptDescription != $txtCptCode.val()) {
            utility.DisplayMessages("Procedure not Valid", 2);
            $txtCptCode.val('');
            return false;
        }
        else
            return true;

    },



    //End of New functions added by Zeeshan


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
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_Procedures", null, false);
    },

    // Validate and save/edit functions

    ValidateProcedures: function () {
        $('#frmClinicalProcedures')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   // Start 27/11/2015 Muhammad Irfan Bug # 91,92

                   ProblemName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   // End 27/11/2015 Muhammad Irfan Bug # 91,92

                   //Color: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_Procedures.ProceduresSave();
        });
    },


    ProceduresSave: function () {
        var objDeffered = $.Deferred();

        if ($("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr").length != 0) {
            if ($("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr td").length != 1) {


                var strMessage = "";
                $("#" + Clinical_Procedures.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
                var self = $("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures");
                if (Clinical_Procedures.params.mode == "Add") {
                    var hfProblemText = $("#" + Clinical_Procedures.params.PanelID + " #hfIMOProblem").val();
                    var changesProblemText = $("#" + Clinical_Procedures.params.PanelID + " #txtProblems").val();
                    //AppPrivileges.GetFormPrivileges("Medical_Procedures", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    //    if (strMessage == "") {
                    Clinical_Procedures.SaveProcedures().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_Procedures.SaveFavToggelStatus($('#' + Clinical_Procedures.params.PanelID + ' #ddlFavoriteListProcedure').val());
                            //Clinical_Procedures.SaveFavVal();

                            if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {

                                if (response.ProcedureCount > 0) {
                                    var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
                                    var ProcedureIds = $.map(ProceduresLoadJSONData, function (item) {
                                        return item.ProcedureId;
                                    });
                                    var procedurid = "";
                                    if (ProcedureIds.join(",") != "") {
                                        procedurid = ProcedureIds.join(",");
                                    }
                                    if (ProcedureIds.join() != null && ProcedureIds.join() != '') {
                                        //Clinical_Procedures.showVBPScoreScreen(procedurid, 'Clinical_Procedures');
                                        Clinical_Procedures.attachProceduresFromNotes(ProcedureIds, true, true);  // zia mehmood
                                        //To stop the pop up as per M Bilal
                                        //var objData = {};
                                        //objData["procedureDetailModel"] = Clinical_Procedures.GetAllNewProcedures();
                                        //var ScreeningProcedure = $.grep(objData["procedureDetailModel"], function (procedure, i) {
                                        //    return procedure.CPTCode == "96127";
                                        //});
                                        //if (ScreeningProcedure.length > 0) {
                                        //    var params = [];
                                        //    params["NotesId"] = Clinical_ProgressNote.params.NotesId;
                                        //    params["PatientId"] = Clinical_ProgressNote.params.patientID;
                                        //    params["FromAdmin"] = "0";
                                        //    params["ParentCtrl"] = "Clinical_Procedures";//"clinicalTabProgressNote";
                                        //    params["mode"] = "Add";
                                        //    params["VisitId"] = Clinical_ProgressNote.params.VisitId;
                                        //    params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
                                        //    params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
                                        //    params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                                        //    LoadActionPan("VBP_PHQQuestionnaire", params);
                                        //}
                                    }
                                }
                                //Clinical_Procedures.ProceduresSearch();
                            } else {
                                Clinical_Procedures.ProceduresSearch();
                            }
                            utility.DisplayMessages(response.Message, 1);
                            var PrevFavVal = $('#' + Clinical_Procedures.params.PanelID + ' #ddlFavoriteListProcedure').val();
                            $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').resetAllControls(null);
                            $("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr").each(function () {
                                Clinical_Procedures.EditableGridAdd.datatable.row($(this).get(0)).remove().draw();
                            });
                            $("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr").remove();
                            var $UL = $('#' + Clinical_Procedures.params.PanelID + ' #ulFavoriteListProcedureContent');
                            $UL.empty();
                            Clinical_Procedures.ProceduresDetail = [];
                            Clinical_Procedures.Procedures = [];
                            Clinical_Procedures.EnableAllFavProcedure();
                            $('#' + Clinical_Procedures.params.PanelID + ' #ddlFavoriteListProcedure').val(PrevFavVal);
                            $('#' + Clinical_Procedures.params.PanelID + ' #ddlFavoriteListProcedure').trigger("onchange");
                            objDeffered.resolve();

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                    //    }
                    //    else
                    //        utility.DisplayMessages(strMessage, 2);
                    //});

                }
                else if (Clinical_Procedures.params.mode == "Edit") {

                    Clinical_Procedures.UpdateProcedures(myJSON, Clinical_Procedures.params.ProcedureId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_Procedures.ProceduresSearch();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
            }
            else {
                utility.DisplayMessages("There is no Procedure to Add", 3);
            }
        }
        else {
            utility.DisplayMessages("There is no Procedure to Add", 3);
        }
        return objDeffered;
    },
    showVBPScoreScreen: function (SelectedProcedureIds, parntctl) {
        //var objData = {};
        //objData["procedureDetailModel"] = Clinical_Procedures.GetAllNewProcedures();
        //var ScreeningProcedure = $.grep(objData["procedureDetailModel"], function (procedure, i) {
        //    return procedure.CPTCode == "96127";
        //});

        Clinical_Procedures.isPHQProcedure(SelectedProcedureIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false && response.isPHQProcedure.toLowerCase() == "1") {
                Clinical_ProgressNote.showPHQScorePopUp(parntctl);
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    isPHQProcedure: function (SelectedProcedureIds) {
        var objData = {};
        //it will also check patient eligibility regarding Insurance.
        objData["ProcedureId"] = SelectedProcedureIds;
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "is_phq_procedure";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },
    SaveProcedures: function () {
        var objData = {};
        objData["procedureDetailModel"] = Clinical_Procedures.GetAllNewProcedures();
        objData["commandType"] = "save_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },
    GetAllNewProcedures: function () {
        $("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr:not([id*=Child])").each(function () {
            var myJSON = $(this).getMyJSONByName();
            var objDetail = JSON.parse(myJSON);
            var RowId = $(this).attr("id");
            objDetail["ProblemListId_text"] = objDetail["ProblemListId_text"] == "- Select -" ? "" : objDetail["ProblemListId_text"];
            var ChildData = $("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #pnlProcedure_ResultAdd tbody tr#Child" + RowId).getMyJSONByName();
            var parseChildData = JSON.parse(ChildData);
            objDetail["Comments"] = parseChildData["Comments"];
            $.grep(Clinical_Procedures.Procedures, function (item, index) {
                if (item.ProcedureId == RowId) {
                    objDetail["CPTCode"] = item.CPTCode;
                    if (item.CPTCode == '96127' && Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote")
                        Clinical_Procedures.params.ShowScreen = true;
                    objDetail["CPT_DESCRIPTION"] = item.CPT_DESCRIPTION;
                    objDetail["ProcedureId"] = item.ProcedureId;
                    objDetail["SNOMEDID"] = item.SNOMEDID;
                    objDetail["SNOMED_DESCRIPTION"] = item.SNOMED_DESCRIPTION;
                    return;
                }
            });
            objDetail["PatientId"] = Clinical_Procedures.params.patientID;
            if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
                objDetail["NotesId"] = Clinical_Procedures.params.NotesId;
            }
            else {
                objDetail["NotesId"] = -1;
            }
            Clinical_Procedures.ProceduresDetail.push(objDetail);
        });
        return Clinical_Procedures.ProceduresDetail;
    },
    NoKnownProblem: function () {
        var strMessage = "";
        var self = $("#" + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures");
        var myJSON = self.getMyJSONByName();
        Clinical_Procedures.SaveProcedures(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlProcedures_Result #btnNoKnownProblems").css("display", "none");
                utility.DisplayMessages(response.message, 1);
                Clinical_Procedures.ProceduresSearch();
                $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').resetAllControls(null);
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
    UpdateProcedures: function (ProceduresData, ProcedureId) {

        var isactive = null;
        isactive = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');

        var objData = JSON.parse(ProceduresData);
        if (objData.PatientId == '') {
            objData.PatientId = Clinical_Procedures.params.patientID;
        }
        objData["ProblemListId_text"] = objData["ProblemListId_text"] == "- Select -" ? "" : objData["ProblemListId_text"];
        objData["IsActive"] = isactive;
        objData["commandType"] = "UPDATE_VITALS";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    // End Validate and save/edit functions

    // Search/Grid Load Functions
    //Adding Pagination on 04 Dec 2015 by Azhar
    ProceduresSearch: function (ProcedureId, PageNo, rpp, IsEnabled) {

        //var PanelProblemListGrid = "#pnlClinicalProcedures #pnlProcedures_Result";
        //var ProblemListGridId = "#pnlClinicalProcedures #dgvProcedures";
        ////$(ChargeGridId).dataTable().fnDestroy();
        //$(ProblemListGridId + " tbody tr").remove();
        //Clinical_Procedures.EditableGrid = EMRUtility.MakeEditableGrid(PanelProblemListGrid, ProblemListGridId, Clinical_Procedures, "0", false, false, false, false);

        var strMessage = "";

        if ($("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result").css("display") == "none") {
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result").show();
        }
        //Start//21/12/2015//Ahmad Raza//Implimented Privileges for Procedures Search
        //AppPrivileges.GetFormPrivileges("Medical_Procedures", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        Clinical_Procedures.SearchProcedures(ProcedureId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Adding selection column of checkbox of Problem lists for Progress Notes on 04 Dec 2015 by Azhar
                if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
                    if ($("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures thead tr #SelectRecord").length == 0) {
                        $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures thead tr").prepend(' <th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderProcedures" onchange="Clinical_Procedures.checkUncheckAllProcedures(this);"   class="pull-left" coltype="checkbox"/> </th>');
                    }

                } else {
                    $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures th#SelectRecord").remove();
                }
                //Clinical_Procedures.ProceduresGridLoad(response);
                Clinical_Procedures.ProceduresGridLoadNew(response);
                //Adding Pagination on 04 Dec 2015 by Azhar
                var TableControl = Clinical_Procedures.params.PanelID + " #dgvProcedures";
                var PagingPanelControlID = Clinical_Procedures.params.PanelID + " #dgvProcedures_Paging";
                var ClassControlName = "Clinical_Procedures";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.ProcedureTotalCount;    // EMR - 6546
                setTimeout(CreatePagination(response.ProcedureCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_Procedures.ProceduresSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
                setTimeout(function () {
                    if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging #btnAddProceduresToNotes").length == 0) {
                        $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Procedures.addProceduresToNotes();" disabled id="btnAddProceduresToNotes">Add on Note</button>').insertAfter("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging .pagination")
                        // added by zia mehmood
                        if (IsEnabled) {
                            $("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging #btnAddProceduresToNotes").prop('disabled', false);
                        }
                    }
                }, 11);
                setTimeout(function () {
                    $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures th[controlname='LastUpdatedDate']").click();
                    $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures th[controlname='LastUpdatedDate']").click();
                }, 20);
                /*setTimeout(function () {
                    //$('#' + Clinical_Procedures.params.PanelID + ' #dgvProcedures').DataTable().destroy(); dataTable().fnDestroy();
                    if ($.fn.dataTable.isDataTable("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures")) {
                        $("#" + Clinical_Procedures.params.PanelID + " pnlProcedures_Result #dgvProcedures").dataTable().fnClearTable();
                        $("#" + Clinical_Procedures.params.PanelID + " pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
                    }
                    if (Clinical_Procedures.params.ParentCtrl != "clinicalTabProgressNote") {

                        $('#dgvProcedures').DataTable({
                            "order": [[7, "desc"]]
                        });
                    }
                    else {
                        $('#dgvProcedures').DataTable({
                            "order": [[8, "desc"]]
                        });
                    }
                }, 1);*/
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        //End//21/12/2015//Ahmad Raza//Implimented Privileges for Procedures Search
    },
    //Start by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.
    checkUncheckAllProcedures: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $("#" + Clinical_Procedures.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", true);
        } else {
            $("#" + Clinical_Procedures.params.PanelID + " [name='SelectCheckBoxProbList']").prop("checked", false);
        }
        $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody").find('input[type="checkbox"]').each(function () {
            Clinical_Procedures.enableAddProbList(this);
        });
    },
    //End by Khaleel Ur Rehman to check uncheck all problem list by a checkBox in header. Date: 22 Jan 2016.
    ProceduresGridLoad: function (response) {
        //$("#pnlClinicalProcedures #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
        //$("#pnlClinicalProcedures #pnlProcedures_Result #dgvProcedures tbody").find("tr").remove();
        if (response.ProcedureCount > 0) {
            Clinical_Procedures.EditableGrid.datatable.clear().draw();
            ////new
            //var actions = "";
            //$("#pnlClinicalProcedures #dgvProcedures tr th").each(function () {
            //    if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
            //        var arrActionType = [];
            //        if ($(this).attr("ActionType") != null) {
            //            arrActionType = $(this).attr("ActionType").split(',');
            //            actions = EditableGrid.GetActions(arrActionType);
            //        }
            //    }
            //});

            var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);

            $.each(ProceduresLoadJSONData, function (i, item) {
                //var charge_detail = JSON.parse(item);
                var ProcedureId = item.ProcedureId;
                var CurrentRow = Clinical_Procedures.AddNewProceduresRow(ProcedureId, "Edit", null);
                //var VisitChargesTable = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures");
                var self = $("#dgvProcedures tr#" + ProcedureId);
                //utility.bindMyJSON(true, item, false, VitalsLoadJSONData);
                utility.bindMyJSONByName(true, item, false, self).done(function () {
                    //$('#pnlClinicalProcedures #btnsave').text('Update');
                    //Clinical_Procedures.params["VitalSignsId"] = VitalSignsId;
                    //Clinical_Procedures.params["mode"] = "Edit";
                });

                var row = Clinical_Procedures.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();

                /********************************/


                row.child().loadDropDowns(true).done(function () {
                    utility.bindMyJSON(true, item, false, $(newChildRow));
                    //    //serialize data
                    //    // $('#frmClinicalProcedures').data('serialize', $('#frmClinicalProcedures').serialize());

                });
            });
        }
        else {
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").css("display", "none");
            if (Clinical_Procedures.params.ParentCtrl != "clinicalTabProgressNote") {
                $('#dgvProcedures').DataTable({
                    "language": {
                        "emptyTable": "No Procedure List Found."
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [7] }]
                });
            }
            else {
                $('#dgvProcedures').DataTable({
                    "language": {
                        "emptyTable": "No Procedure List Found."
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [8] }]
                });
            }
        }
    },

    SearchProcedures: function (ProcedureId, PageNumber, RowsPerPage) {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();

        var ProcedureData = [];
        //var objData1 = {};

        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["ProcedureId"] = ProcedureId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["NotesId"] = Clinical_Procedures.params.NotesId == null ? 0 : Clinical_Procedures.params.NotesId;
        ProcedureData.push(objData);

        var objData1 = new Object();
        objData1["procedureDetailModel"] = ProcedureData;
        objData1["ProcedureId"] = ProcedureId;
        objData1["IsActive"] = IsCheckedIn;
        objData1["ShowEMCodes"] = "0";
        objData1["commandType"] = "search_procedures";
        var data = JSON.stringify(objData1);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },

    //Author: M Ahmad Imran
    //Date :  03-31-2016
    //Reason: Intailize toggle of favorites
    domReadyFunction: function () {






        $(document).ready(function () {


            $('#' + Clinical_Procedures.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });
        });
    },
    // Search/Grid Load Functions
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");
        //var PriorAuthorization = "<div class='col-xs-2'><label class='control-label'>Prior Auth. Number</label><input class='form-control' id='txtPriorAuthorization" + ParentRowId + "' name='txtPriorAuthorization" + ParentRowId + "' type='text' placeholder='999-99-9999'  /></div>";
        //var PriorAuthorization = "<div class='col-xs-2'><label class='control-label'>Prior Auth. Number</label> <input class='form-control' type='text' list='ddlPriorAuthorization" + ParentRowId + "' name='PriorAuthorization" + ParentRowId + "' id='txtPriorAuthorization" + ParentRowId + "'/> <datalist id='ddlPriorAuthorization" + ParentRowId + "' name='PriorAuthorization" + ParentRowId + "><option  value='- SELECT -'></option></datalist></div>";
        var txtProblem = "<div class='col-xs-1'><label class='control-label'>Problem</label><input  class='form-control'type='text' id='txtProblem" + ParentRowId + "' name='Problem" + ParentRowId + "' disabled></div>";
        var txtDiagnosis = "<div class='col-xs-1'><label class='control-label'>ICD-9 ICD-10 Description</label><input class='form-control' id='Diagnosis" + ParentRowId + "' name='Diagnosis" + ParentRowId + "' type='text' /></div>";
        var txtChronicityLevel = "<div class='col-xs-1'><label class='control-label'>Chronicity Level</label><input class='form-control' id='txtChronicity" + ParentRowId + "' name='Chronicity" + ParentRowId + "' type='text' /></div>";
        var txtSeverity = "<div class='col-xs-1  size-min100'><label class='control-label'>Severity</label><input class='form-control' id='txtSeverity" + ParentRowId + "' name='Severity" + ParentRowId + "' type='text' /></div>";
        var ddlNDCMeasurement = "<div class='col-xs-2'><label class='control-label'>NDC Measurement Code</label><select id='ddlNDCMeasurement" + ParentRowId + "' name='ddlNDCMeasurement" + ParentRowId + "' class='form-control' ddlist='GetNDCMeasurementCode'></select></div>";
        var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea spellcheck='true' class='form-control' rows='1' id='txtComments" + ParentRowId + "' name='txtComments" + ParentRowId + "'></textarea></div>";
        var chkHold = "<div class='col-xs-1 pt-lg'><div class='checkbox-custom checkbox-default'><input type='checkbox' onclick=Clinical_Procedures.validateIsHold(this,'divHoldDays" + ParentRowId + "') id='chkHold" + ParentRowId + "' value name='chkHold" + ParentRowId + "'/><label class='control-label'>Is Hold</label></div></div>";
        var HoldDays = "<div id='divHoldDays" + ParentRowId + "' style='display:none' class='col-xs-1'><label class='control-label'>Hold Days</label><input type='text' class='form-control' onfocusout=Clinical_Procedures.validateHoldDays(this,'chkHold" + ParentRowId + "') id='txtHoldDays" + ParentRowId + "' data-mask='9?99' name='txtHoldDays" + ParentRowId + "'/></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(txtProblem, txtDiagnosis, txtChronicityLevel, txtSeverity, ddlNDCMeasurement, LineNotes, chkHold, HoldDays, spacer);
        return ChildHTML;

    },
    //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
    AddNewProceduresRow: function (RowId, mode, CurrRef, NotesId) {


        var CurrentRow = null;
        if (RowId && RowId > 0) {

            CurrentRow = Clinical_Procedures.EditableGrid.rowAdd(RowId, Clinical_Procedures.params.VitalSignsId, null, null, null, null, NotesId);

        }
        else {
            var TemplateRow = $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = Clinical_Procedures.EditableGrid.rowAdd(TemplateRowId - 1, Clinical_Procedures.params.VitalSignsId, null, null, null, null, null);
            //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        }

        var row = Clinical_Procedures.EditableGrid.datatable.row(CurrentRow);
        row.child(Clinical_Procedures.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        Clinical_Procedures.enableRemoveRow($(CurrentRow));
        return CurrentRow;
    },
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },
    // end editable grid functions

    // editabele grid functions

    rowSave: function ($row, obj) {

        //if (obj.rowValidate($row)) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
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
        var NotesId = $row.attr("proceduresnotesid");
        //End//31/12/2015//Ahmad Raza//Bug#178 fixed
        if (id && id > 0) {

            //Edit Record
            var strMessage = "";
            //Start//22/12/2015//Ahmad Raza//Logic implemented for Privileges
            //AppPrivileges.GetFormPrivileges("Medical_Procedures", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            Clinical_Procedures.UpdateProceduresRow(myJSON, id).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //Serialization
                    //  $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').data('serialize', $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').serialize());

                    //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                    //Start//31/12/2015//Ahmad Raza//Logic to update against current Note only
                    if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote" && NotesId.indexOf(Clinical_Procedures.params.NotesId) > -1) {
                        //Ends//31/12/2015//Ahmad Raza//Logic to update against current Note only
                        Clinical_Procedures.getProceduresInfo(id, true);
                    }
                    //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_Procedures.ProceduresSearch();
                    //Clinical_Procedures.rowDraw($row, _self, values);
                    //  Clinical_Procedures.ProceduresSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            //    }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
            //End//22/12/2015//Ahmad Raza//Logic implemented for Privileges
        }

    },

    rowDetail: function ($row, ClassName) {
        var currentVitalSignId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentVitalSignId > 0) {
            Clinical_Procedures.VitalsEdit(currentVitalSignId);
        }
    },

    rowHistory: function ($row, ClassName) {
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var currentProcedureId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentProcedureId > 0) {
            Clinical_Procedures.ShowHistory(currentProcedureId);
        }
    },
    ShowHistory: function (ProcedureId) {
        if (Clinical_Procedures.params.ParentCtrl == "Clinical_Treatment") {
            EMRUtility.showCurrentItemHistory(Clinical_Procedures.params.PanelID, null, ProcedureId, "Procedures", Clinical_Procedures.params.patientID, "Clinical_Procedures", null);
        }
        else {
            EMRUtility.showCurrentItemHistory(Clinical_Procedures.params.PanelID, null, ProcedureId, "Procedures", Clinical_Procedures.params.patientID, Clinical_Procedures.params.ParentCtrl != "clinicalTabProgressNote" ? Clinical_Procedures.params.TabID : "Clinical_Procedures", null);
        }

    },

    rowAdd: function () {
        //Start//21/12/2015//Ahmad Raza//Privileges logic implemented
        //AppPrivileges.GetFormPrivileges("Medical_Procedures", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        EditableGrid.rowAdd();
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        //End//21/12/2015//Ahmad Raza//Privileges logic implemented
    },

    rowRemove: function ($row, obj) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var id = $row.attr("id");
        if (id.split('-')[0] != "") {
            var id = id.split('-')[1];
        }
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Medical_Procedures", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('1', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else if (selectedValue < 0) {//Start//30/3/2016//M Ahmad Imran//
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();

                var myJSON = $row.getMyJSONByName();
                var objData = JSON.parse(myJSON);

                Clinical_Procedures.EnableFavProcedure(objData["ProcedureProcedure"]);
                //Clinical_Procedures.RemoveFromCashing(objData["ProcedureProcedure"]);
                utility.DisplayMessages("Successfully Deleted", 1);
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + selectedValue).length != 0) {
                    Clinical_Procedures.detachProceduresFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_ProgressNote.refreshPatientBanner(selectedValue);
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + selectedValue).remove();
                            Clinical_ProgressNote.saveComponentSOAPText('Procedures').done(function () {
                                Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);
                            });
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
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
                Clinical_Procedures.DeleteProcedure(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        if ($row.hasClass('adding')) {
                        }
                        var _self = obj;
                        _self.datatable.row($row.get(0)).remove().draw();

                        utility.DisplayMessages(response.Message, 1);
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + selectedValue).length != 0) {
                            Clinical_Procedures.detachProceduresFromNotes_DBCall(selectedValue).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_ProgressNote.refreshPatientBanner(selectedValue);
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + selectedValue).remove();
                                    Clinical_ProgressNote.saveComponentSOAPText('Procedures').done(function () {
                                        Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);
                                    });
                                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                    //Start//28/12/2015//Ahmad Raza// No Known Procedures hyperlink(link not visible when there is no problem list) issue fixed
                    Clinical_Procedures.ProceduresSearch();
                    //End//28/12/2015//Ahmad Raza// No Known Procedures hyperlink(link not visible when there is no problem list) issue fixed

                });
            }
        }, function () { },
            '1'
        );
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented

    },

    rowInactive: function ($row, obj) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var temp = $row.attr('id');
        var tempId = temp.split('-')[1];
        $row.attr('id', tempId);
        var strMessage = "";
        var id = $row.attr("id");
        var IsActive = null;
        IsActive = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == "1") {
            IsActive = "0";
        } else {
            IsActive = "1"
        }
        //AppPrivileges.GetFormPrivileges("Medical_Procedures", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('3', function () {
            var selectedValue = id;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
                    Clinical_ProgressNote.AttachedNoteComponentIds.push(selectedValue);
                }
                Clinical_Procedures.InActiveProcedures(selectedValue, null, null).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Clinical_Procedures.ProceduresSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 1);
                    }
                });

            }
        }, function () { },
               '3', null, null, null, IsActive
        );
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },

    InActiveProcedures: function (ProcedureId, comments, endDate) {

        var IsActiveRecord = null;
        IsActiveRecord = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');

        //     var ProcedureId = Clinical_Procedures.params.ProcedureId;
        if (IsActiveRecord == null || IsActiveRecord == '1') {
            IsActiveRecord = false;
        } else {
            IsActiveRecord = true;
        }
        var objData = new Object();
        objData["ProcedureId"] = ProcedureId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = IsActiveRecord;
        objData["commandType"] = "INACTIVE_PROCEDURES";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

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

        $row.find("#hfComments").val(Clinical_Procedures.PreviousComments);

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
        if (Clinical_Procedures.params.ActionPanContainer == "actionPanClinicalProgressNote" && $row.parent().parent().attr("id") == "dgvProcedures") {
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

        var VitalsGridId = "#" + Clinical_Procedures.params.PanelID + " #dgvProcedures";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Clinical_Procedures.EditableGrid.datatable.row(parentRow);

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
    UpdateProceduresRow: function (ProceduresData, ProcedureId) {

        var isactive = null;
        isactive = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
        var objData = {};
        var ProcedureDetail = [];
        var objData1 = JSON.parse(ProceduresData);
        objData1["ProcedureId"] = ProcedureId;
        objData1["Comments"] = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();//$('#' + Clinical_Procedures.params.PanelID + ' #hfGridComments').val();
        objData1["IsActive"] = isactive;
        objData1["ProblemListId_text"] = objData1["ProblemListId_text"] == "- Select -" ? "" : objData1["ProblemListId_text"];
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            objData1["NotesId"] = Clinical_Procedures.params.NotesId;
        }

        ProcedureDetail.push(objData1);
        objData["procedureDetailModel"] = ProcedureDetail;
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "update_procedure";
        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },
    //
    buildHistoryRows: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems) {
        var row = Clinical_Procedures.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.ProcedureId);

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
    DeleteProcedure: function (ProcedureId) {

        var objData = new Object();
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "DELETE_Procedure";
        objData["PatientId"] = Clinical_Procedures.params.patientID;



        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },

    // problem list grid load as per admin

    ProceduresGridLoadNew: function (response) {

        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        var isactive = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');
        // get Actions
        var actions = "";
        $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, " #pnlProcedures_Result");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures")) {
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnClearTable();
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
            $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody").find("tr").remove();
        }

        $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody").find("tr").remove();

        //  $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnClearTable();
        //   $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures tbody").find("tr").remove();


        //    if ($.fn.dataTable.isDataTable("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures")) {
        //        $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
        //    }

        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        //  var isactive = $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #divSwitch #switchActive').attr('isactive');



        if (response.ProcedureCount > 0) {
            var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
            //----------------------------------------kr
            /*ProceduresLoadJSONData.sort(SortByName);
            function SortByName(a, b) {
                var aName = a.ModifiedOn.toLowerCase();
                var bName = b.ModifiedOn.toLowerCase();
                return ((aName > bName) ? -1 : ((aName > bName) ? 1 : 0));
            }*/


            //----------------------------------------kr
            var ProcedureHistoryLoadJSONData = JSON.parse(response.ProcedureHistoryLoad_JSON);
            //if ($.fn.dataTable.isDataTable("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures")) {
            //    $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
            //}
            //tem array to hold rows and childs
            var arraTemp = [];

            $.each(ProceduresLoadJSONData, function (i, item) {

                var SurgicalValue;

                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", "txtpUnits-" + item.ProcedureId);
                $row.attr("ProceduresNotesId", item.NotesId);
                //Start//31/12/2015//Ahmad Raza//Bug#178 fixed
                $row.attr("ProblemListId", item.ProblemListId);
                //End//31/12/2015//Ahmad Raza//Bug#178 fixed
                $row.attr("name", "Procedures");

                if (item.Surgical == "True") {
                    $row.attr("SurgicalId", 1);
                    SurgicalValue = "Yes";
                }
                else {
                    $row.attr("SurgicalId", 0);
                    SurgicalValue = "No";
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

                if (item.Comments != "") {
                    var commentsMethod = "Clinical_Procedures.AddComments('" + item.ProcedureId + "');";
                    //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting"></i></a>';
                    comments = '<label data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                }
                var ProcedureId = item.ProcedureId;
                var ChildHistory_Procedures = $.grep(ProcedureHistoryLoadJSONData, function (n, i) {
                    return n.ProcedureId == ProcedureId;
                });
                var icd9String = '';
                if (item.ICD9) {
                    //icd9String = item.ICD9 + ' - ' + item.ICD10 + ' - ' + item.ICD10_DESCRIPTION
                    icd9String = item.ICD10 + ' - ' + item.ICD10_DESCRIPTION
                }
                if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
                    //$row.append('<td></td><td></td><td class="actions" id="' + item.ProcedureId + '" ></td><td>' + item.ProblemName + '</td><td>' + item.Description + '</td><td>' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td>');
                    //Start//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row

                    var SelectionCheckBoxColumn = "";
                    var Checked = "";
                    if (item.IsNoteLinked == "True" || item.IsNoteLinked == "1") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProcedureId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            Checked = " ";
                        } else {
                            Checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProcedureId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                    }
                    var CQMtd = '';
                    if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {

                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" class="pull-left mt-default" onchange="Clinical_Procedures.enableAddProbList(this);" id="' + item.ProcedureId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        CQMtd = '<td>' + (item.CQMEncounterType ? item.CQMEncounterType : "") + '</td>';
                    } else {
                        CQMtd = "<td></td>";
                    }
                    if (ChildHistory_Procedures.length > 0) {
                        //$row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td>' + item.ProcedureId + '</td><td>' + item.ICD9 + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td align="center"> ' + comments + ' </td>');
                        $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + SurgicalValue + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td>' + item.RefusalReason + '</td>' + CQMtd);
                        $row.attr("isHistory", '1');
                    } else {
                        //$row.append(SelectionCheckBoxColumn + '<td></td><td></td><td>' + item.ProcedureId + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + item.ICD9 + +' ' + item.ICD9_DESCRIPTION + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td align="center"> ' + comments + ' </td>');
                        $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + SurgicalValue + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td>' + item.RefusalReason + '</td>' + CQMtd);
                    }
                    //End//15/12/2015//Ahmad Raza//Adding check box with noKnownProblems row

                    var hfProceduresComments = $(' <input type="hidden" id="hfComments" name="Comments" value="' + item.Comments + '">');
                    $row.append(hfProceduresComments);
                }
                else {

                    //adding checkboxes column and disabling that row, if problem list already binded with notes
                    var SelectionCheckBoxColumn = "";
                    var Checked = "";
                    if (item.IsNoteLinked == "True") {
                        if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProcedureId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                            Checked = " ";
                        } else {
                            Checked = " checked";
                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProcedureId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                    }

                    if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                        SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" onchange="Clinical_Procedures.enableAddProbList(this);" id="' + item.ProcedureId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
                        // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-338
                    } else {
                        SelectionCheckBoxColumn = "";
                    }
                    if (ChildHistory_Procedures.length > 0) {
                        $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + SurgicalValue + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td>' + item.RefusalReason + '</td><td hidden></td>');
                        $row.attr("isHistory", '1');

                    } else {
                        $row.append(SelectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.ProcedureId + '" >' + actions + '</td><td>' + item.CPTCode + ' ' + item.CPT_DESCRIPTION + '</td><td>' + SurgicalValue + '</td><td>' + item.Unit + '</td><td>' + item.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td>' + item.RefusalReason + '</td><td hidden></td>');

                    }

                    var hfProceduresComments = $(' <input type="hidden" id="hfComments" name="Comments" value="' + item.Comments + '">');
                    $row.append(hfProceduresComments);
                }
                if (item.IsActive == "True") {
                    // $($row).find('a.edit-row').removeAttr('disabled', false);
                    $($row).find('a.edit-row').removeClass('disableAll')
                } else {
                    $($row).find('a.edit-row').addClass('disableAll')
                    //  $($row).find('a.edit-row').attr('disabled', 'disabled')
                }


                $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody").last().append($row);

                var CurrentRowchilds = $();

                if (ChildHistory_Procedures.length > 0) {
                    $.each(ChildHistory_Procedures, function (i, item1) {
                        // if (item1.ProcedureId == ProcedureId) {
                        //arrProceduresHistory.push(item1);
                        if (item1.Comments != "") {
                            var commentsMethod = "Clinical_Procedures.AddComments('" + item1.ProcedureId + "');";
                            //comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item1.Comments + '"><i class="fa fa-commenting"></i></a>';
                            comments = '<label data-toggle="tooltip" data-placement="left" title="' + item1.Comments + '"><i class="fa fa-commenting blue"></i></label>';
                        }
                        else {
                            comments = "";
                        }
                        icd9String = '';
                        if (item1.ICD9) {
                            icd9String = item1.ICD9 + ' - ' + item1.ICD10 + ' - ' + item1.ICD10_DESCRIPTION
                        }
                        var Title_Tooltip = "Inactive Reason: " + item1.InActiveChkBoxValue + (item1.EndDate != '' ? " <br/>End Date: " + utility.RemoveTimeFromDate(null, item1.EndDate) : "") + (item1.InActiveReason != '' ? " <br/>Comments: " + item1.InActiveReason : "");
                        var IsActiveText = "";
                        if (item1.IsActive == "True") {
                            IsActiveText = "<Label>[Active]</Label>";
                        } else {
                            IsActiveText = "<Label data-toggle='tooltip' data-placement='right' title='" + Title_Tooltip + "'>[Inactive]</Label>";
                        }


                        //Start//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed
                        if (Clinical_Procedures.params.ActionPanContainer == "actionPanClinicalProgressNote") {
                            var currentHistory = '<tr class="childRow-bg"><td></td><td></td><td class="actions" id="' + item1.ProcedureId + '" ></td><td>' + IsActiveText + '</td><td>' + SurgicalValue + '</td><td>' + item1.Unit + '</td><td>' + item1.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item1.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item1.EndDate) + '</td><td>' + item1.ModifiedOn + ' ' + item1.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td></td><td></td>';

                        }
                        else {

                            var currentHistory = '<tr class="childRow-bg"><td></td><td class="actions" id="' + item1.ProcedureId + '" ></td><td>' + IsActiveText + '</td><td>' + SurgicalValue + '</td><td>' + item1.Unit + '</td><td>' + item1.Modifier + '</td><td>' + icd9String + '</td><td>' + utility.RemoveTimeFromDate(null, item1.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item1.EndDate) + '</td><td>' + item1.ModifiedOn + ' ' + item1.ModifiedBy + '</td><td align="center"> ' + comments + ' </td><td></td><td hidden></td>';


                        }
                        //End//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed

                        //arrProceduresHistory.push(currentHistory);
                        CurrentRowchilds = CurrentRowchilds.add(currentHistory);

                        // }
                    });
                }

                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: CurrentRowchilds });
            });

            //Inalize grid
            var PanelGrid = "#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result";
            var GridId = "#" + Clinical_Procedures.params.PanelID + " #dgvProcedures";

            //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (Clinical_Procedures.myGrid != null) {
                //  Clinical_Procedures.myGrid.$table.find("tbody tr").remove();
                // Clinical_Procedures.myGrid.$table.dataTable().fnClearTable()
                //  Clinical_Procedures.myGrid.$table.dataTable().fnDestroy();

                //  Clinical_Procedures.myGrid.datatable.clear().draw();
                $("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures").dataTable().fnDestroy();
            }
            //End By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

            //   if ($.fn.dataTable.isDataTable("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result #dgvProcedures") == false) {
            Clinical_Procedures.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_Procedures, 0, false, true, false, true, false, null);
            //  $("#" + Clinical_Procedures.params.PanelID + " div.mystuff").attr('id', 'divSwitch');
            //   }

            //rander childs
            $.each(arraTemp, function (i, item) {

                if (Clinical_Procedures.myGrid != null) {
                    var row = Clinical_Procedures.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            //Start//04//01//2015//Ahmad Raza//Sorting removed from first column of grid
            // $('#dgvProcedures').dataTable().fnSettings().aoColumns[0].bSortable = false;
            //$('#' + Clinical_Procedures.params.PanelID + ' #dgvProcedures').dataTable().fnSettings().aoColumns[0].bSortable = false;
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
                         '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Procedures.ActiveProblemSearch(this);">' +
                          '</div><span class="pl-xs">Active</span>';

            $("#" + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            //  if ($('#pnlClinicalProcedures #switchActive').is(':checked') || $("#pnlProcedures_Result #btnNoKnownProblems").is(':visible')) {



            $('#' + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result #dgvProcedures').DataTable({
                "language": {
                    "emptyTable": "No Procedure Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
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
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Procedures.ActiveProblemSearch(this);">' +
                         '</div><span class="pl-xs">Active</span>';

            $("#" + Clinical_Procedures.params.PanelID + ' #pnlProcedures_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
            if (response.ProcedureTotalCount == 0) {
                $("#pnlProcedures_Result #btnNoKnownProblems").css("display", "");
            } else {
                $("#pnlProcedures_Result #btnNoKnownProblems").hide();
            }

            /* End of Code for making No Known Problem List hyperlink inline with checkbox and search box.
            *   By: ZeeshanAK with assistance of Azhar Shahzad | On: 5-Jan-2016
            */
        }


        EMRUtility.SwicthWidgetInializatoin();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //Start//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        // $('#dgvProcedures thead tr th:first-child').removeClass('sorting_asc');
        $('#' + Clinical_Procedures.params.PanelID + ' #dgvProcedures thead tr th:first-child').removeClass('sorting_asc');
        //End//04//01//2015//Ahmad Raza//Sorting icon removed from first column of grid
        //Editable Grid
        //var PanelGrid = "#pnlClinicalProcedures #pnlProcedures_Result";
        //var GridId = "#pnlClinicalProcedures #dgvProcedures";
        //EMRUtility.MakeEditableGrid(PanelGrid, GridId, Clinical_Procedures, 0);
        //txtLastUpdated
        EMRUtility.fixDataTableDuplication("#" + Clinical_Procedures.params.PanelID + " #pnlProcedures_Result");

    },

    //
    unLoadTab: function (nextOrPre, controlToInvoke) {
        Clinical_Procedures.controlToInvoke = controlToInvoke;
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */


        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            if (!$.isEmptyObject(Clinical_Procedures.ProceduresDetail) && Clinical_Procedures.ProceduresDetail.length > 0) {
                utility.myConfirm('12', function () {
                    $.when(Clinical_Procedures.ProceduresSave()).then(function () {
                        Clinical_Procedures.addProceduresToNotesOnClose();
                        EMRUtility.scrollToPNcomponent('clinical_procedures');
                        UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                        if (nextOrPre == true) {
                            EMRUtility.scrollToPNcomponent('clinical_procedures');
                            UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                            if (Clinical_Procedures.controlToInvoke != null) {

                                setTimeout(function () {
                                    Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                                    Clinical_Procedures.controlToInvoke = null;
                                }, 600);

                            }
                        }
                    });
                },
            function () {
                Clinical_Procedures.addProceduresToNotesOnClose();
                UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                if (nextOrPre == true) {
                    EMRUtility.scrollToPNcomponent('clinical_procedures');
                    UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                    if (Clinical_Procedures.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                            Clinical_Procedures.controlToInvoke = null;
                        }, 600);

                    }
                }
                Clinical_Procedures.ProceduresDetail = [];
            });

            }
            else {
                var exist = false;
                $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody").find('input[type="checkbox"]').each(function () {
                    if (this.checked) {
                        exist = true;
                    }
                    if (exist) {
                        return false;
                    }
                });
                if (exist == false && Clinical_ProgressNote.AttachedNoteComponentIds.length > 0) {
                    exist = true;
                }
                if (exist) {
                    utility.myConfirmNote('1', function () {
                        Clinical_Procedures.addProceduresToNotesOnClose();
                        UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_Procedures');
                        if (nextOrPre == true) {
                            EMRUtility.scrollToPNcomponent('clinical_procedures');
                            UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                            if (Clinical_Procedures.controlToInvoke != null) {

                                setTimeout(function () {
                                    Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                                    Clinical_Procedures.controlToInvoke = null;
                                }, 600);
                            }
                        }

                    }, "", function () {
                        UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_Procedures');
                        if (nextOrPre == true) {
                            EMRUtility.scrollToPNcomponent('clinical_procedures');
                            UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                            if (Clinical_Procedures.controlToInvoke != null) {

                                setTimeout(function () {
                                    Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                                    Clinical_Procedures.controlToInvoke = null;
                                }, 600);

                            }
                        }
                    });
                }
                else {
                    UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_Procedures');
                    if (nextOrPre == true) {
                        EMRUtility.scrollToPNcomponent('clinical_procedures');
                        UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                        if (Clinical_Procedures.controlToInvoke != null) {

                            setTimeout(function () {
                                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                                Clinical_Procedures.controlToInvoke = null;
                            }, 600);

                        }
                    }
                }
            }
        }
        else if (Clinical_Procedures.params["FromAdmin"] == "0") {
            if (!$.isEmptyObject(Clinical_Procedures.ProceduresDetail) && Clinical_Procedures.ProceduresDetail.length > 0) {
                utility.myConfirm('12', function () {
                    $.when(Clinical_Procedures.ProceduresSave()).then(function () {
                        UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                        if (nextOrPre == true) {
                            EMRUtility.scrollToPNcomponent('clinical_procedures');
                            UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                            if (Clinical_Procedures.controlToInvoke != null) {

                                setTimeout(function () {
                                    Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                                    Clinical_Procedures.controlToInvoke = null;
                                }, 600);

                            }
                        }
                    });
                },
            function () {
                EMRUtility.scrollToPNcomponent('clinical_procedures');
                UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                if (nextOrPre == true) {
                    UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                    if (Clinical_Procedures.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                            Clinical_Procedures.controlToInvoke = null;
                        }, 600);

                    }
                }
                Clinical_Procedures.ProceduresDetail = [];
            });

            }
            else {
                if (Clinical_Procedures.params != null && Clinical_Procedures.params.ParentCtrl != null && Clinical_Procedures.params.PrPanelID != null) {
                    UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures', null, Clinical_Procedures.params.PrPanelID);
                }
                else
                    UnloadActionPan(null, 'Clinical_Procedures');
            }
            if (nextOrPre == true) {
                UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
                if (Clinical_Procedures.controlToInvoke != null) {

                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Procedures.controlToInvoke.replace(/\s/g, ''));
                        Clinical_Procedures.controlToInvoke = null;
                    }, 600);

                }
            }
            if (Clinical_Procedures.params.ParentCtrl == "Clinical_Treatment") {
                Treatment_ProcedureListGrid.ProceduresSearch();
            }
        }
        else {
            //$("#mstrDivMedical #clinicalMenu_Medical_Procedures").remove();
            //$("#mstrDivMedical #clinicalMenu_Medical_Procedures").removeClass('active');
            //$("#sortableMedical #clinicalMenu_Medical_Procedures").removeClass('nav-active');
            RemoveAdminTab();
        }
        objDeffered.resolve();
        EMRUtility.scrollToPNcomponent('clinical_procedures');
        return objDeffered;
    },

    //Comments Update

    AddComments: function (ProcedureId) {


        var params = [];
        var PanelID = "";
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'Clinical_Procedures';
            PanelID = 'pnlClinicalProgressNote #pnlClinicalProcedures';
        }
        else if (Clinical_Procedures.params.ParentCtrl == "clinicalTabFaceSheet") {
            params["ParentCtrl"] = 'Clinical_Procedures';
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalProcedures';
        }
        else if (Clinical_Procedures.params.ParentCtrl == "Clinical_Treatment") {
            params["ParentCtrl"] = 'Clinical_Procedures';
            PanelID = 'pnlClinicalTreatment #pnlClinicalProcedures';
        }
        else {
            params["ParentCtrl"] = 'clinicalTabProcedures';
            PanelID = 'pnlClinicalProcedures';
        }
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            params["NotesId"] = Clinical_Procedures.params.NotesId;
        }
        else {
            params["NotesId"] = -1;
        }
        params["ProcedureID"] = ProcedureId;
        params["Comments"] = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + ProcedureId + "']").find("#hfComments").val();
        params["FromAdmin"] = "0";
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        LoadActionPan('Clinical_ProceduresComments', params, PanelID);
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

        Clinical_Procedures.ProceduresSearch();
    },

    // Start 27/11/2015 Muhammad Irfan Bug # 93,94
    ResetDiagnosis: function () {

        if ($("#" + Clinical_Procedures.params.PanelID + " #txtProblems").val() == "") {
            $('#' + Clinical_Procedures.params.PanelID + ' #frmClinicalProcedures').resetAllControls(null);
            $("#" + Clinical_Procedures.params.PanelID + " #txtDiagnosis,#ddlChronicityLevel,#ddlSeverity,#dpStartDate,#dpEndDate,#txtComments").prop("disabled", true);
        }
    },
    // End 27/11/2015 Muhammad Irfan Bug # 93,94


    // Start 7/2/2016 Muhammad Ahmad Imran
    //Purpose Save/update favList Status
    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_Procedures.params.PanelID + " #frmClinicalProcedures #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_Procedures.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_Procedures.FavListName, FavListVal);
        });
    },

    //SaveFavVal: function (FavListVal) {

    //
    //},
    // End 7/2/2016 Muhammad Ahmad Imran

    //-----------------Progress Note-------------
    // added on Dec 04,2015 by Muhammad Azhar Shahzad
    //Call Back function to add component to Progress Note
    addProceduresToNotes: function () {

        var SelectedProcedures = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
        if (SelectedProcedures != null && SelectedProcedures != '') {
            for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                var PLid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid).length != 0) {
                    var index = SelectedProcedures.indexOf(PLid);
                    if (index > -1) {
                        SelectedProcedures.splice(index, 1);
                    }
                }
            }
        }

        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
        if (detachedvalues.join() != '') {
            Clinical_Procedures.detachProceduresFromNotes(detachedvalues).done(function () {
                if (SelectedProcedures.join() != null && SelectedProcedures.join() != '') {
                    Clinical_Procedures.attachProceduresFromNotes(SelectedProcedures);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures');
                }
            });
        } else if (SelectedProcedures.join() != null && SelectedProcedures.join() != '') {
            Clinical_Procedures.getProceduresInfo(SelectedProcedures.join());
        }

        //When User has attached Allergies with notes than add on note button should be disabled
        $("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging #btnAddProceduresToNotes").prop('disabled', true);
        if (Clinical_Procedures.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_Procedures.params.ParentCtrl, 'Clinical_Procedures');
            EMRUtility.scrollToPNcomponent('clinical_procedures');
        }
        //Clinical_Procedures.unLoadTab();
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    addProceduresToNotesOnClose: function () {
        $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody").find('input[type="checkbox"]').each(function () {
            if ($(this).is(':checked')) {
                var procId = $(this).attr('id');
                Clinical_ProgressNote.AttachedNoteComponentIds.push(procId);
            }
        });
        var SelectedProcedures = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
        if (SelectedProcedures != null && SelectedProcedures != '') {
            for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                var PLid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid).length != 0) {
                    var index = SelectedProcedures.indexOf(PLid);
                    if (index > -1) {
                        //SelectedProcedures.splice(index, 1);
                    }
                }
            }
        }
        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
        if (detachedvalues.join() != '') {
            Clinical_Procedures.detachProceduresFromNotes(detachedvalues).done(function () {
                if (SelectedProcedures.join() != null && SelectedProcedures.join() != '') {
                    Clinical_Procedures.attachProceduresFromNotes(SelectedProcedures);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures');
                }
            });
        } else if (SelectedProcedures.join() != null && SelectedProcedures.join() != '') {
            Clinical_Procedures.getProceduresInfo(SelectedProcedures.join());
        }
        //When User has attached Allergies with notes than add on note button should be disabled
        $("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging #btnAddProceduresToNotes").prop('disabled', true);
        EMRUtility.scrollToPNcomponent('clinical_procedures');
        // Clinical_Procedures.UnLoadTab();
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    attachProceduresFromNotes: function (SelectedProcedures, hideAlertMessage, IsEnabled) {
        Clinical_Procedures.getProceduresInfo(SelectedProcedures.join(), hideAlertMessage).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_Procedures.params != null && Clinical_Procedures.params.PanelID != null && Clinical_Procedures.params.PanelID.indexOf('pnlClinicalProcedures') != -1) {
                    Clinical_Procedures.ProceduresSearch(null, null, null, IsEnabled);
                }
            }, 5);
        });
    },

    detachProceduresFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        var strMessage = "";
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Medical_Procedures", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue;
                if (detachedvalues.indexOf("Cli_Procedures_Main") > -1) {
                    selectedValue = detachedvalues.replace('Cli_Procedures_Main', '');
                    utility.myConfirm('1', function () {
                        EMRUtility.scrollToPNcomponent('clinical_procedures');
                        if (selectedValue == "" || selectedValue == "undefined") {
                            dfd.resolve('ok');
                        }
                        else {
                            Clinical_Procedures.detachProceduresFromNotes_DBCall(selectedValue).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_ProgressNote.refreshPatientBanner(selectedValue);
                                    var PLid = selectedValue.split(',');

                                    for (var i = 0; i < PLid.length; i++) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid[i]).remove();
                                    }
                                    //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid).remove();
                                    Clinical_ProgressNote.saveComponentSOAPText('Procedures').done(function () {
                                        Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);
                                    });
                                    Clinical_ProgressNote.HideShowBillingInfo();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                dfd.resolve('ok');
                            });
                        }
                    }, function () { },
                                        '1'
                                    );
                }
                else {
                    selectedValue = detachedvalues.join(",");
                    //utility.myConfirm('1', function () {

                    if (selectedValue == "" || selectedValue == "undefined") {
                        dfd.resolve('ok');
                    }
                    else {
                        Clinical_Procedures.detachProceduresFromNotes_DBCall(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_ProgressNote.refreshPatientBanner(selectedValue);
                                var PLid = selectedValue.split(',');

                                for (var i = 0; i < PLid.length; i++) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid[i]).remove();
                                }
                                //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid).remove();
                                Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);

                                Clinical_ProgressNote.HideShowBillingInfo();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            dfd.resolve('ok');
                        });
                    }
                    //}, function () { },
                    //                   '1'
                    //             );
                }



            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
        return dfd.promise();
    },

    //this function will get Problem Lists Soap Text and attach that to Progress note
    getProceduresInfo: function (ProceduresId, hideAlertMessage) {
        if (ProceduresId == null || ProceduresId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_Procedures.get_Procedures_ForSOAP(ProceduresId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_Procedures.createProceduresBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', ProceduresId, hideAlertMessage);
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
    checkProceduresExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ProceduresComponent" NoteComponentId="NCDummyId"> <header>' +
                '<Clinical_Procedures title="Procedures"  id="' + this.id + '" class="NotesComponent">' +
                    '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Procedures\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Procedures">Procedures</a> ' +
                    '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Procedures\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                    '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Procedures\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                '</Clinical_Procedures> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' +Clinical_ProgressNote.params["PanelID"]+ ' #hfNotesId').val());
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createProceduresBodyHTML: function (response, NoteHTMLCtrl, ProceduresId, hideAlertMessage, dontshowPhqPopup, phqtextneeded, fromEsuperBill, previousProceduresTextObj) {
        Clinical_Procedures.checkProceduresExists();
        if (response.ProcedureSoap_JSON != null && response.ProcedureSoap_JSON != '') {
            var Proceduresoap_JSON = JSON.parse(response.ProcedureSoap_JSON);
            var $mainDivVital = $(document.createElement('div'));

            if (Proceduresoap_JSON == null || Proceduresoap_JSON.length == 0) {
                Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
                return "";
            }
            if (response.ProcedureSoapCount > 0) {
                var PListId = [];
                $.each(Proceduresoap_JSON, function (index, element) {
                    var color = "";
                    var PLid = element.ProcedureId;
                    var $SectionBodyVital = $(document.createElement('section'));
                    $SectionBodyVital.attr('id', "Cli_Procedures_Main" + PLid);
                    var $DetailsDiv = $(document.createElement('div'));
                    $DetailsDiv.attr('id', "Cli_Procedures_" + PLid);
                    var $ListVital = $(document.createElement('ul'));

                    $ListVital.attr('class', 'list-unstyled')

                    $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Procedures_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Procedures_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');


                    var StartDateEndDate = "";
                    var inActiveText = "";
                    if (element.IsActive != null && element.IsActive == "False") {
                        inActiveText = "<span style = 'color:red;font-weight:bold'> (Inactive) </span>";
                    }
                    if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                        StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " from " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " to " + utility.RemoveTimeFromDate(null, element.EndDate));
                    }
                    else if (element.StartDate == '' || element.StartDate == null && (element.EndDate != '' && element.EndDate != null)) {
                        StartDateEndDate = (element.EndDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.EndDate));
                    }
                    else if (element.EndDate == '' || element.EndDate == null && (element.StartDate != '' && element.StartDate != null)) {
                        StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.StartDate));
                    }
                    else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                        StartDateEndDate = "";
                    }
                    else if (element.StartDate == element.EndDate) {
                        StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.StartDate));
                    }
                    if (element.ProcedureCodeName == '') {
                        //if (element.ICD10 != '' && element.ICD10_DESCRIPTION != '') {
                        //    element.ProcedureCodeName = element.ICD10 + ' - ' + unescape(element.ICD10_DESCRIPTION);
                        //}
                        //else if (element.ICD9 != '' && element.ICD9_DESCRIPTION != '') {
                        //    element.ProcedureCodeName = element.ICD9 + ' - ' + unescape(element.ICD9_DESCRIPTION);
                        //}
                        //else if (element.SNOMEDID != '' && element.SNOMED_DESCRIPTION != '') {
                        //    element.ProcedureCodeName = element.SNOMEDID + ' - ' + unescape(element.SNOMED_DESCRIPTION);
                        //}
                        if (element.ShowCptCode == 0) {
                            if (element.CPTCode != '' && element.CPT_DESCRIPTION != '') {
                                element.ProcedureCodeName = unescape(element.CPT_DESCRIPTION);
                            }
                        }
                        else {
                            if (element.CPTCode != '' && element.CPT_DESCRIPTION != '') {
                                element.ProcedureCodeName = element.CPTCode + ' - ' + unescape(element.CPT_DESCRIPTION);
                            }
                        }
                    }
                    if (element.Diagnosis == '' || element.Diagnosis == undefined) {
                        element.Diagnosis = element.ICD10 == '' || element.ICD10 == null ? '' : element.ICD10 + ' - ' + unescape(element.ICD10_DESCRIPTION);
                    }

                    //Start 27-09-2017 Edit By Humaira Yousaf IMP-1152
                    $ListVital.append("<li>" +
                        //(element.ProcedureCodeName == '' ? "" : "Patient underwent " + element.ProcedureCodeName) +
                        (element.ProcedureCodeName == '' ? "" : element.ProcedureCodeName) +
                        (element.Diagnosis == '' || element.Diagnosis == " select  " || element.Diagnosis == " Select  " || element.Diagnosis == null || element.Diagnosis == undefined ? "" : " based on the following assessment: " + element.Diagnosis)
                        //+ StartDateEndDate
                        );
                    //End 27-09-2017 Edit By Humaira Yousaf IMP-1152
                    $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
                    if (element.ProcedureTemplateSoapTextExists) {
                        ProcedureSysObservationDetail.GetProcedureTemplateSoapText(element.ProcedureId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status) {
                                $ListVital.append(utility.decodeHtml(response.ProcedureTemplateSoapText));
                            }
                        });
                    }
                    if (fromEsuperBill) {
                        if (previousProceduresTextObj.length > 0) {
                            var editorComments = $(previousProceduresTextObj).find('div[id=Cli_Procedures_' + PLid + '] ul li[id=Comments_Cli_Procedures_' + PLid + ']');
                            if (editorComments)
                                $ListVital.append(editorComments);
                        }
                    }

                    $DetailsDiv.append($ListVital);
                    $SectionBodyVital.append($DetailsDiv);
                    if ($(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid).length == 0) {
                        PListId.push(PLid);
                        $mainDivVital.append($SectionBodyVital);
                    } else {

                        var CommentHTML = "";
                        var CommentsID = $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid).html($SectionBodyVital.html());
                        $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid + ' ul').append(CommentHTML);;
                    }

                });
                //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                if (PListId.join(",") != "") {
                    ProceduresId = PListId.join(",");
                }
                //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed

                Clinical_Procedures.isPHQProcedure(ProceduresId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false && response.isPHQProcedure.toLowerCase() == "1") {

                        setTimeout(function () {
                            if (Clinical_Procedures.params.ShowScreen) {
                                Clinical_ProgressNote.showPHQScorePopUp('Clinical_Procedures');
                            }
                            else {
                                if (!dontshowPhqPopup)
                                    Clinical_ProgressNote.showPHQScorePopUp('clinicalTabProgressNote');
                            }
                        }, 1000);
                        Clinical_Procedures.CalculateVBPSocreAndAppend(Clinical_Procedures.params.NotesId, phqtextneeded).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('section').each(function () {
                                    if ($(this).find("ul li span strong").length > 0) {
                                        $(this).find("ul li span strong").closest('li').remove();
                                    }
                                    if ($(this).find("ul li a").length > 0 && $(this).find("ul li a") && $(this).find("ul li a").attr("onclick").indexOf('VBP_PHQQuestionnaire') > -1) {
                                        $(this).find("ul li a").closest('li').remove();
                                        $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + response.ProceudereID + ' ul').append(response.PHQSoapText);
                                        //Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
                                    }
                                    else {
                                        var SecId = $(this).attr("id"); var responseID = 'Cli_Procedures_Main' + response.ProceudereID;
                                        if (SecId == responseID) {
                                            $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + response.ProceudereID + ' ul').append(response.PHQSoapText);
                                            //Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
                                        }
                                    }
                                });
                                Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
                            }
                        });
                    }
                    else {
                        //utility.DisplayMessages(response.Message, 3);
                    }
                });
                if ($mainDivVital.html() != '') {
                    Clinical_Procedures.updateProceduresHtml($mainDivVital.html(), ProceduresId, NoteHTMLCtrl, hideAlertMessage);
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
                } else {
                    Clinical_Procedures.updateProceduresHtml('', ProceduresId, NoteHTMLCtrl);
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
                }
            }
        }
    },
    createProceduresBodyHTMLForNotes: function (Procedures, AttachedProcedures, NoteHTMLCtrl, ProceduresId, hideAlertMessage, bNotSaveCompt) {
        Clinical_Procedures.checkProceduresExists();
        var $mainDivVital = $(document.createElement('div'));

        if (!Procedures) {
            return;
        }
        var PListId = [];
        if (Procedures.length > 0) {

            $.each(Procedures, function (index, element) {
                var color = "";
                var PLid = element.ProcedureId;
                PListId.push(PLid);
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Procedures_Main" + PLid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Procedures_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Procedures_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Procedures_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');


                var StartDateEndDate = "";
                var inActiveText = "";
                if (element.IsActive != null && element.IsActive == "False") {
                    inActiveText = "<span style = 'color:red;font-weight:bold'> (Inactive) </span>";
                }
                if (element.StartDate != '' && element.StartDate != null && element.EndDate != '' && element.EndDate != null && utility.RemoveTimeFromDate(null, element.StartDate) != utility.RemoveTimeFromDate(null, element.EndDate)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " from " + utility.RemoveTimeFromDate(null, element.StartDate)) + (element.EndDate == '' ? "" : " to " + utility.RemoveTimeFromDate(null, element.EndDate));
                }
                else if (element.StartDate == '' || element.StartDate == null && (element.EndDate != '' && element.EndDate != null)) {
                    StartDateEndDate = (element.EndDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.EndDate));
                }
                else if (element.EndDate == '' || element.EndDate == null && (element.StartDate != '' && element.StartDate != null)) {
                    StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.StartDate));
                }
                else if (element.EndDate == '' || element.EndDate == null && (element.StartDate == '' || element.StartDate == null)) {
                    StartDateEndDate = "";
                }
                else if (element.StartDate == element.EndDate) {
                    StartDateEndDate = (element.StartDate == '' ? "" : inActiveText + " on " + utility.RemoveTimeFromDate(null, element.StartDate));
                }

                if (element.ProcedureCodeName == '' || element.ProcedureCodeName == undefined) {
                    if (element.ShowCPTCode == 0) {
                        if (element.CPT_DESCRIPTION != '') {
                            element.ProcedureCodeName = unescape(element.CPT_DESCRIPTION);
                        }
                    }
                    else {
                        if (element.CPTCode != '' && element.CPT_DESCRIPTION != '') {
                            element.ProcedureCodeName = element.CPTCode + ' - ' + unescape(element.CPT_DESCRIPTION);
                        }
                    }
                }
                if (element.Diagnosis == '' || element.Diagnosis == undefined) {
                    element.Diagnosis = element.ICD10 == '' || element.ICD10 == null ? '' : element.ICD10 + ' - ' + unescape(element.ICD10_DESCRIPTION);
                }
                //Start 27-09-2017 Edit By Humaira Yousaf IMP-1152
                element.ProcedureCodeName = element.ProcedureCodeName == undefined ? element.CPT_DESCRIPTION : element.ProcedureCodeName;
                $ListVital.append("<li>" +
                    //(element.ProcedureCodeName == '' ? "" : "Patient underwent " + element.ProcedureCodeName) +

                    (element.ProcedureCodeName == '' ? "" : element.ProcedureCodeName) +
                    (element.Diagnosis == '' || element.Diagnosis == " select  " || element.Diagnosis == " Select  " || element.Diagnosis == null || element.Diagnosis == undefined ? "" : " based on the following assessment: " + element.Diagnosis)
                    //+ StartDateEndDate
                    );
                //End 27-09-2017 Edit By Humaira Yousaf IMP-1152
                $ListVital.append(element.Comments == "" ? "" : "<li>Comments: " + element.Comments);
                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid).length == 0) {
                    $mainDivVital.append($SectionBodyVital);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + PLid + ' ul').append(CommentHTML);;
                }

            });
            //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if (PListId.join(",") != "") {
                ProceduresId = PListId.join(",");
            }
            //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            if ($mainDivVital.html() != '') {
                Clinical_Procedures.setProceduresHtml($mainDivVital.html(), ProceduresId, AttachedProcedures, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt);
            } else {
                Clinical_Procedures.setProceduresHtml('', ProceduresId, AttachedProcedures, NoteHTMLCtrl, bNotSaveCompt);
                if (!bNotSaveCompt)
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage);
            }
        }
    },

    setProceduresHtml: function (ProceduresHtml, ProcedureId, AttachedProcedures, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().addClass('initialVisitBody');
        if (ProceduresHtml != '') {
            $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().append(ProceduresHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProceduresHtml != '') {

            if (AttachedProcedures.length > 0) {
                if (!bNotSaveCompt)
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage).done(function () {
                        Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);
                    });
                Clinical_ProgressNote.HideShowBillingInfo();
            }
        }

    },

    CalculateVBPSocreAndAppend: function (NotesID, PHQ9TextNeeded) {
        var objData = new Object();
        objData["NotesId"] = NotesID;
        if (PHQ9TextNeeded) {
            objData["PHQTextNeeded"] = true;
        }
        else {
            objData["PHQTextNeeded"] = false;
        }
        objData["commandType"] = "Calculate_VBP_Socre";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },
    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    updateProceduresHtml: function (ProceduresHtml, ProcedureId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().addClass('initialVisitBody');
        if (ProceduresHtml != '') {
            $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().append(ProceduresHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ProceduresHtml != '') {
            Clinical_Procedures.attachProceduresignFromNotes(ProcedureId, hideAlertMessage, bNotSaveCompt);
        }

    },

    //This Function detach Problem list From progress note
    detach_ComponentsProcedures: function (ComponentName, IsUpdate, ProceduresComponentRemove) {
        var Clinical_ProcedureIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').parent().parent().find('section[id*="Cli_Procedures_Main"]').map(function () {
            return this.id.replace("Cli_Procedures_Main", "");
        }).get().join(',');

        if (ProceduresComponentRemove) {
            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').parent().parent().attr('NoteComponentId');
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedures').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Procedures', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedures').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Problem Lists']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedures').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Procedures', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_procedures').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }

        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedures').parent().parent().find('section[id*="Cli_Procedures_Main"]').remove();
        }

        if (Clinical_ProcedureIds == "" || Clinical_ProcedureIds == "undefined") {
            Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            Clinical_Procedures.detachProceduresFromNotes_DBCall(Clinical_ProcedureIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var detachedvalues = Clinical_ProcedureIds.split(',');
                    $.each(detachedvalues, function (index, value) {
                        var PLid = detachedvalues[index];
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + PLid).remove();
                    });
                    Clinical_ProgressNote.refreshPatientBanner(Clinical_ProcedureIds);
                    if (IsUpdate) {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Procedures', true)).then(function () {
                            Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);
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
    attachProceduresignFromNotes: function (ProceduresId, hideAlertMessage, bNotSaveCompt) {
        var strMessage = "";
        if (strMessage == "") {

            var selectedValue = ProceduresId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Procedures.attachProceduresWithNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (!bNotSaveCompt)
                            Clinical_ProgressNote.saveComponentSOAPText('Procedures', hideAlertMessage).done(function () {
                                Clinical_ProgressNote.SaveAndAttachProcedureReport($('#PatientProfile #hfPatientId').val(), Clinical_ProgressNote.params.NotesId, Clinical_ProgressNote.params.CurrentNotesProviderId, false);
                            });
                        Clinical_ProgressNote.HideShowBillingInfo();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }


        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
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
            $("#" + Clinical_ProgressNote.params.PanelID + ' #pnlProcedures_Result #chkHeaderProcedures').prop('checked', false);
            // Begin Date  Edit By Muhammad Ahmad Imran Bug # EMR-283
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }

        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
            $("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging #btnAddProceduresToNotes").prop('disabled', false);
        } else {
            $("#" + Clinical_Procedures.params.PanelID + "  #dgvProcedures_Paging #btnAddProceduresToNotes").prop('disabled', true);
        }

    },

    //If Procedures Component which is dropeed in Progress note has no Procedures attached, than it will call for Latest Procedures for this patient
    getLatestProceduresByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        if (strMessage == "") {
            Clinical_Procedures.getLatestProceduresByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var PHQProcedures = $.grep(JSON.parse(response.ProcedureSoap_JSON), function (procedure, idx) {
                        return procedure.CPTCode == "96127"
                    });
                    Clinical_Procedures.createProceduresBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
                    if (PHQProcedures != null && PHQProcedures.length > 0) {
                        //setTimeout(function () {
                        //   Clinical_ProgressNote.showPHQScorePopUp();
                        //}, 1000);
                    }
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

    getRcopiaInformaionForProceduresoap: function () {
        Clinical_Procedures.getLatestProceduresByPatientId();
    },


    //START Charge Editable Grid Code

    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription, unit, modifier) {

        var ProcedureGridId = "#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd";
        var TemplateRowId = 0;
        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (Clinical_Procedures.params.ParentCtrl != null) {
                CurrentRow = Clinical_Procedures.EditableGridAdd.rowAdd(RowId, "");
            }
            else {
                CurrentRow = Clinical_Procedures.EditableGridAdd.rowAdd(RowId, Clinical_Procedures.params.ProcedureId);
            }

        }
        else {
            var TemplateRow = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd tbody tr[id*=-]").last();
            TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = Clinical_Procedures.EditableGridAdd.rowAdd(TemplateRowId - 1, "");
        }
        var item = {}
        item["ProcedureId"] = TemplateRowId - 1;
        item["CPTCode"] = cptCode;
        item["CPT_DESCRIPTION"] = procDesc;
        item["SNOMEDID"] = SnomedID;
        item["SNOMED_DESCRIPTION"] = SnomedDescription;
        Clinical_Procedures.Procedures.push(item);
        var dtDOSFromId = String($(CurrentRow).find("input[id*=dtpStartDate]").attr('id'));
        var dtDOSToId = String($(CurrentRow).find("input[id*=dtpEndDate]").attr("id"));
        var txtUnit = String($(CurrentRow).find("input[id*=txtUnit]").attr("id"));
        $('#' + Clinical_Procedures.params.PanelID + ' #dgvProcedureAdd').on('keydown', '#' + txtUnit, function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
        $(CurrentRow).find('input[id*="txtUnit"]').bind("paste", function (e) {
            e.preventDefault();
        });
        utility.CreateDatePicker('frmClinicalProcedures #' + dtDOSFromId, function () {
            //on-change callback method
        }, true);
        utility.CreateDatePicker('frmClinicalProcedures #' + dtDOSToId, function () {
            //on-change callback method
        }, true);

        //$("#" + Clinical_Procedures.params.PanelID + " #dgvProcedure tbody tr td div").addClass("mb - tiny");

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-131 */
        Clinical_Procedures.ValidateFromToDate('frmClinicalProcedures', dtDOSFromId, dtDOSToId, true);



        var row = Clinical_Procedures.EditableGridAdd.datatable.row(CurrentRow);


        row.child(Clinical_Procedures.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));
        row.child.hide();


        if (cptDescription.indexOf(" (SCT: " + SnomedID + ")") >= 0) {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').val(cptCode + " " + procDesc);
        }

        else {
            $(CurrentRow).find('input[id*="ProcedureProcedure"]').val(cptCode + " " + procDesc + (SnomedID != "" ? " (SCT: " + SnomedID + ")" : ""));
        }

        $(CurrentRow).find('input[id*="txtUnit"]').val(unit);
        $(CurrentRow).find('input[id*="txtModifier"]').val(modifier);

        Clinical_Procedures.enableRemoveRow($(CurrentRow));
        var $DId = $(CurrentRow).find('select[id*="ProblemListId"]').attr('id');
        CacheManager.BindDropDownsByID("#" + Clinical_Procedures.params.PanelID + " #dgvProcedureAdd #" + $DId, 'LookupProblemLists', true, $('#PatientProfile #hfPatientId').val());
        $(CurrentRow).find('select[id*=txtSurgical]').html('<option value="1" refvalue="" refname="">Yes</option><option value="0" refvalue="" refname="" selected="selected">No</option>');
        return CurrentRow;
    },
    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    utility.DisplayMessages("From date is greater than To date", 3);
                }
            }

            $(CtrlToDate).attr('disabled', false);


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }

            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(CtrlToDate).attr("name");
            }
            if (typeof CtrFromDateName == 'undefined') {
                CtrFromDateName = $(CtrlFromDate).attr("name");
            }
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            // startDate: inputDate,
            format: date_format,
            //todayBtn: 'linked',
        }).change(function (e) {
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(this).attr("name");
            }
            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                        $(CtrlForm).bootstrapValidator('revalidateField', CurrentDatepicker.name);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                            utility.DisplayMessages("To date is smaller than from date", 3)
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {

                   if ($(CtrlFromDate).val() != "")
                       utility.ValidateDate(CtrlFromDate);
               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {

                if ($(CtrlToDate).val() != "")
                    utility.ValidateDate(CtrlToDate)
            }, 100);
        });
    },
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");

        var Comments = "<div class='col-xs-12 pl-xlg mb-tiny'><label class='control-label'>Comments</label><textarea type='text' class='form-control' spellcheck='true' id='Comments" + ParentRowId + "'  name='Comments'></textarea></div>";


        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(Comments);
        return ChildHTML;
    },
    ProcedureOrderTestGridLoad: function (response) {
        var PanelProcedureGrid = "#" + Clinical_Procedures.params.PanelID + " #pnlProcedure_Result";
        var ProcedureGridId = "#" + Clinical_Procedures.params.PanelID + " #dgvProcedure";
        $(ProcedureGridId + " tbody tr").remove();
        if ($.fn.dataTable.isDataTable(ProcedureGridId)) {
            $(ProcedureGridId).dataTable().fnClearTable();
            $(ProcedureGridId).dataTable().fnDestroy();
        }
        Clinical_Procedures.EditableGrid.datatable.clear().draw();
        var ProcedureOrderTestLoadJSONData = JSON.parse(response.procedureOrderTest_JSON);
        $.each(ProcedureOrderTestLoadJSONData, function (i, item) {
            var ProcedureOrderTestId = item.ProcedureOrderTestId;
            var CurrentRow = Clinical_Procedures.AddNewProcedureRow(ProcedureOrderTestId, null, null, null, null, null, null, null);
            var self = $("#" + Clinical_Procedures.params.PanelID + " tr#" + ProcedureOrderTestId);
            var row = Clinical_Procedures.EditableGrid.datatable.row(CurrentRow);
            var ProcedureTestTable = $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedure");

            //Start Farooq Ahmad 03/28/2016 bind values to the table
            var counter = 0;
            var BindFunction = function (counter, item, CurrentRow) {
                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                });
                if (counter++ < 5)
                    setTimeout(BindFunction, 1000, counter, item, CurrentRow);

            }
            BindFunction(counter, item, CurrentRow);

            //End Farooq Ahmad 03/28/2016 bind values to the table
        });

    },

    //------------------ end M Ahmad Imran Code for editable grid



    //-----Server calls of Notes----------
    attachProceduresWithNotes_DBCall: function (ProceduresId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureId"] = ProceduresId;
        objData["commandType"] = "attach_procedures_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    detachProceduresFromNotes_DBCall: function (ProceduresId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureId"] = ProceduresId;
        objData["commandType"] = "detach_procedures_from_notes";
        objData["ForVBP"] = true;
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    get_Procedures_ForSOAP: function (ProceduresId) {

        var objData = new Object();
        objData["ProcedureId"] = ProceduresId;
        objData["PatientId"] = Clinical_Procedures.params.patientID;
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId;
        objData["commandType"] = "get_procedures_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },

    getLatestProceduresByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["commandType"] = "getlatest_proceduresby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },
    //--------------end progress Note-----------
    setCommentsField: function (procedureId, comments) {
        $("#" + Clinical_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + procedureId + "']").find("#hfComments").val(comments);
    },

    logViewProcedures: function (procedureId) {


        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["ProcedureId"] = procedureId;
        objData["commandType"] = "search_procedures";
        objData["NotesId"] = Clinical_Procedures.params.NotesId == null ? 0 : Clinical_Procedures.params.NotesId;
        objData["IsActive"] = true;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },
    //FOR CCM
    SaveProceduresCCM: function (CCMProgram) {

        Clinical_Procedures.params.Program = CCMProgram;
        var objData = {};
        var objDetail = {};
        var ProceduresDetail = [];

        objDetail["Comments"] = "";
        objDetail["ProcedureId"] = -1;
        objDetail["SNOMEDID"] = "";
        objDetail["SNOMED_DESCRIPTION"] = "";
        objDetail["PatientId"] = Clinical_ProgressNote.params.patientID;
        objDetail["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objDetail["EndDate"] = $("#" + Clinical_Procedures.params.PanelID + ' #dtpVisitDate').val();
        objDetail["Modifier"] = "";
        objDetail["ProblemListId"] = "";
        objDetail["ProblemListId_text"] = "";
        objDetail["StartDate"] = $("#" + Clinical_Procedures.params.PanelID + ' #dtpVisitDate').val();


        var Duration = Clinical_ProgressNote.params.CCMDuration;
        if (Clinical_ProgressNote.params.CCMDuration && Clinical_ProgressNote.params.CCMDuration != "")
            Duration = Duration.split(':');

        var Hours = 0;
        var Minutes = 0;
        var Seconds = 0;

        if (Duration != null) {
            if (Duration.length > 2) {
                Hours = Duration[0];
                Minutes = Duration[1];
                Seconds = Duration[2];
            }
            else {
                if (Duration.length > 1) {
                    Minutes = Duration[0];
                    Seconds = Duration[1];
                }
                else {
                    if (Duration.length > 0) {
                        Seconds = Duration[0];
                    }
                }
            }
        }

        var minut = 0;
        if (parseInt(Hours) > 0)
            minut = parseInt(Hours) * 60;
        minut = parseInt(minut) + parseInt(Minutes);

        if (Clinical_Procedures.params.Program == "Non-Complex") {

            objDetail["CPTCode"] = "99490";
            objDetail["CPT_DESCRIPTION"] = "Chron care management srvc 20 min per month";
            objDetail["ProcedureProcedure"] = "99490 Chron care management srvc 20 min per month";
            ProceduresDetail.push(objDetail);
        }
        if (Clinical_Procedures.params.Program == "Complex  (at least 60 minutes required)") {
            if (minut >= 60 && minut < 90) {
                objDetail["CPTCode"] = "99487";
                objDetail["CPT_DESCRIPTION"] = "Cmplx chron care mgmt w/o pt vst 1st hr per mo";
                objDetail["ProcedureProcedure"] = "99487 Cmplx chron care mgmt w/o pt vst 1st hr per mo";
                objDetail["Unit"] = "1";

                ProceduresDetail.push(objDetail);
            }

            if (minut >= 90 && minut < 120) {

                objDetail["CPTCode"] = "99487";
                objDetail["CPT_DESCRIPTION"] = "Cmplx chron care mgmt w/o pt vst 1st hr per mo";
                objDetail["ProcedureProcedure"] = "99487 Cmplx chron care mgmt w/o pt vst 1st hr per mo";
                objDetail["Unit"] = "1";

                ProceduresDetail.push(objDetail);

                objDetail = {};
                objDetail["Comments"] = "";
                objDetail["ProcedureId"] = -1;
                objDetail["SNOMEDID"] = "";
                objDetail["SNOMED_DESCRIPTION"] = "";
                objDetail["PatientId"] = Clinical_ProgressNote.params.patientID;
                objDetail["NotesId"] = Clinical_ProgressNote.params.NotesId;
                objDetail["EndDate"] = $("#" + Clinical_Procedures.params.PanelID + ' #dtpVisitDate').val();
                objDetail["Modifier"] = "";
                objDetail["ProblemListId"] = "";
                objDetail["ProblemListId_text"] = "";
                objDetail["StartDate"] = $("#" + Clinical_Procedures.params.PanelID + ' #dtpVisitDate').val();

                objDetail["CPTCode"] = "99489";
                objDetail["CPT_DESCRIPTION"] = "Cmplx chron care mgmt ea addl 30 min per month";
                objDetail["ProcedureProcedure"] = "99489 Cmplx chron care mgmt ea addl 30 min per month";
                objDetail["Unit"] = "1";

                ProceduresDetail.push(objDetail);

            }
            if (minut >= 120) {
                objDetail["CPTCode"] = "99487";
                objDetail["CPT_DESCRIPTION"] = "Cmplx chron care mgmt w/o pt vst 1st hr per mo";
                objDetail["ProcedureProcedure"] = "99487 Cmplx chron care mgmt w/o pt vst 1st hr per mo";
                objDetail["Unit"] = "1";

                ProceduresDetail.push(objDetail);

                objDetail = {};
                objDetail["Comments"] = "";
                objDetail["ProcedureId"] = -1;
                objDetail["SNOMEDID"] = "";
                objDetail["SNOMED_DESCRIPTION"] = "";
                objDetail["PatientId"] = Clinical_ProgressNote.params.patientID;
                objDetail["NotesId"] = Clinical_ProgressNote.params.NotesId;
                objDetail["EndDate"] = $("#" + Clinical_Procedures.params.PanelID + ' #dtpVisitDate').val();
                objDetail["Modifier"] = "";
                objDetail["ProblemListId"] = "";
                objDetail["ProblemListId_text"] = "";
                objDetail["StartDate"] = $("#" + Clinical_Procedures.params.PanelID + ' #dtpVisitDate').val();

                objDetail["CPTCode"] = "99489";
                objDetail["CPT_DESCRIPTION"] = "Cmplx chron care mgmt ea addl 30 min per month";
                objDetail["ProcedureProcedure"] = "99489 Cmplx chron care mgmt ea addl 30 min per month";
                //objDetail["Unit"] = "2";

                if (minut < 150)
                    objDetail["Unit"] = "2";
                if (minut >= 150 && minut < 180)
                    objDetail["Unit"] = "3";
                if (minut >= 180 && minut < 210)
                    objDetail["Unit"] = "4";
                if (minut >= 210 && minut < 240)
                    objDetail["Unit"] = "5";
                if (minut >= 240 && minut < 270)
                    objDetail["Unit"] = "6";
                if (minut >= 310 && minut < 340)
                    objDetail["Unit"] = "7";

                ProceduresDetail.push(objDetail);
            }
        }

        objData["procedureDetailModel"] = ProceduresDetail;
        objData["commandType"] = "save_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },
    ProceduresSaveCCM: function (CCMProgram) {
        var objDeffered = $.Deferred();
        Clinical_Procedures.SaveProceduresCCM(CCMProgram).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                if (response.ProcedureCount > 0) {
                    var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
                    var ProcedureIds = $.map(ProceduresLoadJSONData, function (item) {
                        return item.ProcedureId;
                    });
                    if (ProcedureIds.join() != null && ProcedureIds.join() != '') {
                        Clinical_Procedures.getProceduresInfo(ProcedureIds.join(), true);
                    }
                }
                objDeffered.resolve();

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },
}