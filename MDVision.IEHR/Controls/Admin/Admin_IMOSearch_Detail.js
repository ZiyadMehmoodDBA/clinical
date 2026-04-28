IMODetail = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {

        IMODetail.params = params;
        $("#lblNoRecordFound").hide();
        if (IMODetail.params["PanelID"] != 'IMODetail')
            IMODetail.params["PanelID"] = IMODetail.params["PanelID"] + ' #IMODetail';

        if (IMODetail.bIsFirstLoad) {
            IMODetail.bIsFirstLoad = false;

            IMODetail.SearchIMO(IMODetail.params.lexiCodeId).done(function (response) {

                if (response.IMOCount <= 0) {

                    var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "", icd9Code = "", icd9Description = "";

                    if (IMODetail.params.ICD9CodePlusICD9Title != undefined) {
                        icd9Code = IMODetail.params.ICD9CodePlusICD9Title.split("+")[0];
                        icd9Description = IMODetail.params.ICD9CodePlusICD9Title.split("+")[1];
                    }

                    if (IMODetail.params.ICD10CodePlusICD10Title != undefined) {
                        icd10Code = IMODetail.params.ICD10CodePlusICD10Title.split("+")[0];
                        icd10Description = IMODetail.params.ICD10CodePlusICD10Title.split("+")[1];
                    }

                    if (IMODetail.params.SNOMEDCodePlusSNOMEDTitle != undefined) {
                        snomedCode = IMODetail.params.SNOMEDCodePlusSNOMEDTitle.split("+")[0];
                        snomedDescription = IMODetail.params.SNOMEDCodePlusSNOMEDTitle.split("+")[1];
                    }

                    var textIcdDescriptionFiled = IMODetail.params.Ctrltext;
                    $("#ICDDetail #txtICDAndDescription").val(textIcdDescriptionFiled);
                    $("#ICDDetail #txtICD9Code").val(icd9Code);
                    $("#ICDDetail #txtICD9Description").val(icd9Description);
                    $("#ICDDetail #txtICD10Code").val(icd10Code);
                    $("#ICDDetail #txtICD10Description").val(icd10Description);
                    $("#ICDDetail #txtSnomedCode").val(snomedCode);
                    $("#ICDDetail #txtSnomedDescription").val(snomedDescription);
                    $("#ICDDetail #hfLexiCode").val(IMODetail.params.lexiCodeId);

                    UnloadActionPan(IMODetail.params.ParentCtrl, 'IMODetail');
                }
                if (response.status != false) {
                    IMODetail.IMOGridLoad(response);
                }
                else {
                    $("#lblNoRecordFound").show();
                    $("#dgvIMO_info, #dgvIMO").remove();
                    $('#IMODetail #dgvIMO').DataTable({
                        "language": {
                            "emptyTable": "No ICD found"
                        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }

                $("#IMODetail #popTitle").text(IMODetail.params.ICD9CodePlusICD9Title.replace("+", " "));
                $("#IMODetail #icd9").text(" " + IMODetail.params.ICD9CodePlusICD9Title.replace("+", " "));
                $("#IMODetail #icd10").text(" " + IMODetail.params.ICD10CodePlusICD10Title.replace("+", " "));
            });
        }
    },

    IMOSearch: function (lexiCodeId) {
        var strMessage = "";
        if (strMessage == "") {
            IMODetail.SearchIMO(lexiCodeId).done(function (response) {
                if (response.IMOCount <= 0) {
                    UnloadActionPan(IMODetail.params.ParentCtrl, 'IMODetail');
                }
                if (response.status != false) {
                    IMODetail.IMOGridLoad(response);
                }
                else {
                    $("#IMODetail #lblNoRecordFound").show();
                    $("#IMODetail #dgvIMO_info, #dgvIMO").remove();
                    $('#IMODetail #IMODetail #dgvIMO').DataTable({
                        "language": {
                            "emptyTable": "No ICD found"
                        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
            });
        }
    },

    SearchIMO: function (lexiCodeId) {
        var data = "lexiCodeId=" + lexiCodeId;
        return MDVisionService.defaultService(data, "ADMIN_IMOSEARCH_DETAIL", "SEARCH_IMO");
    },

    SearchICD10: function (modifiersArray, lexiCodeId) {
        var data = "modifiersArray=" + modifiersArray + "&lexiCodeId=" + lexiCodeId;
        return MDVisionService.defaultService(data, "ADMIN_IMOSEARCH_DETAIL", "SEARCH_ICD10");
    },

    IMOGridLoad: function (response) {

        if (response.IMOCount > 0) {
            var imoLoadJsonData = JSON.parse(response.IMOLoad_JSON);
            var imoLoadJsonDataCombinations = JSON.parse(response.IMOLoad_JSONCombinations);

            var arr_ = []; $.each(imoLoadJsonDataCombinations, function (i, item) { arr_.push(item.MODIFIER_ALT_TITLE + '!' + item.MODIFIERCOMBINATIONS); });

            var tblBody = "";

            // Creating datatable dynamically
            $.each(imoLoadJsonData, function () {
                var tblRow = "";
                HeadKeys = '<tr role="row">';

                $.each(this, function (headerText, modifierTitle) {
                    if (modifierTitle != "") {
                        for (var i = 0; i < arr_.length; i++) {
                            var modifierTitle_ = arr_[i].split('!')[0];
                            if (modifierTitle_ == modifierTitle) {
                                tblRow += "<td id='" + arr_[i].split('!')[1] + "'>" + modifierTitle + "</td>";
                            }
                        }
                    } else
                        tblRow += "<td>" + modifierTitle + "</td>";

                    HeadKeys += '<th tabindex="0" class="sorting_desc" style="width: 0px;" aria-sort="descending" rowspan="1" colspan="1">' + headerText + '</th>';
                });

                tblBody += '<tr>' + tblRow + "</tr>";
            });
            $("#IMODetail #pnlIMO_Result #dgvIMO thead").html(HeadKeys + '</tr>');
            $("#IMODetail #pnlIMO_Result #dgvIMO tbody").html(tblBody);

            // Removing Empty Rows in the Table
            $('#IMODetail #dgvIMO tr').each(function () {
                if (!$.trim($(this).text())) $(this).remove();
            });
        }
        else {
            $('#IMODetail #dgvIMO').DataTable({
                "language": {
                    "emptyTable": "No Problem found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        // Sorting DataTable
        if ($.fn.dataTable.isDataTable('#IMODetail #dgvIMO'))
            ;
        else
            $("#IMODetail #pnlIMO_Result #dgvIMO").DataTable({ aaSorting: [[0, 'desc']], "bLengthChange": false, "autoWidth": false });

        // Array for setting random colors on 'Cell Selection'
        //var colors = ["#FF1744", "#00B8D4", "#304FFE", "#78909C", "#9FA8DA", "#81D4FA", "#4DB6AC", "#CDDC39", "#689F38", "#388E3C", "#827717", "#00E676", "#FF9800", "#E65100", "#607D8B", "#5D4037", "#FF5722", "#006064", "#6200EA", "#C51162", "#AA00FF", "#4A148C"];
        var colors = ["#BCAAA4"];

        $('#IMODetail #dgvIMO tbody').on('click', 'td', function () {

            // Get random color
            var color = colors[Math.floor(Math.random() * colors.length)];

            // Index of Column agaist selected Cell^td
            var columnIndex = $(this).parent().children().index($(this));

            // OnCell Select iterating over datatable and removing current column and column right to current column style and class attribute
            var table = $("#IMODetail #dgvIMO tbody");
            table.find('tr').each(function (i) {

                for (var colIndex = columnIndex; colIndex < $('#IMODetail #dgvIMO thead th').length; colIndex++) {
                    var $tds = $(this).find('td'), cellValue = $tds.eq(columnIndex).text();
                    $tds.eq(colIndex).removeAttr('style');
                    $tds.eq(colIndex).removeAttr('class');
                }

            });

            // On cell click set random Color and class attribute for identification of current cell in a column
            if ($(this)[0].id == "") {
                $(this).attr('style', "background:white !important; color:white !important;");
                $(this).attr('class', 'marked');

            }
            else {
                $(this).attr('style', "background:" + color + " !important; color:white !important;");
                $(this).attr('class', 'marked');
            }

            // Clear ICD10 modifiers list's
            $('#IMODetail' + ' #ICD10Id').empty();
            $('#IMODetail' + ' #ICD10Description').empty();


            // On Cell Select get selected modifers value and push in an array for data minning
            var arr = [];
            $('#IMODetail #dgvIMO tbody tr').each(function () {
                $(this, 'tr').each(function (index, tr) {
                    var lines = $('td.marked', tr).map(function (index, td) {
                        arr.push($(td).text());
                    });
                });
            });

            // Giving extracted data to sever side for further data extraction and filteration
            var lexiCodeId = IMODetail.params.lexiCodeId;
            IMODetail.SearchICD10(arr, lexiCodeId).done(function (response) {
                if (response.status != false) {
                    if (response.ICD10Count > 0) {
                        var icd10LoadJsonData = JSON.parse(response.ICD10Load_JSON);
                        $.each(icd10LoadJsonData, function (i, item) {

                            // fill the list against filtered data
                            $('#IMODetail' + ' #ICD10Id').append('<li>' + item.I10_CODE + '</li>');
                            $('#IMODetail' + ' #ICD10Description').append('<li><a href="#" id=' + item.I10_CODE + ' onClick="IMODetail.GetSelectedICDAndSetTextBoxValue(this);  return false;">' + item.I10_TITLE + '</a></li>');
                        });
                    }
                }
                else {
                    //utility.DisplayMessages(response.Message, 3);
                    //$(this).attr('style', "background: white !important;");
                }
            });

        });

        // Setting first Row's first Cell 'Click event'
        //$('#dgvIMO tr:nth(1)').click();
        $('#dgvIMO tr td:nth(0)').click();

    },

    GetSelectedICDAndSetTextBoxValue: function (obj) {

        //var icdCodeTitle = obj.id + " - " + $(obj).text();

        var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "", icd9Code = "", icd9Description = "";

        if (IMODetail.params.ICD9CodePlusICD9Title != undefined) {
            icd9Code = IMODetail.params.ICD9CodePlusICD9Title.split("+")[0];
            icd9Description = IMODetail.params.ICD9CodePlusICD9Title.split("+")[1];
        }

        if (obj != null && obj != undefined) {
            icd10Code = obj.id;
            icd10Description = $(obj).text();
        }

        if (IMODetail.params.SNOMEDCodePlusSNOMEDTitle != undefined) {
            snomedCode = IMODetail.params.SNOMEDCodePlusSNOMEDTitle.split("+")[0];
            snomedDescription = IMODetail.params.SNOMEDCodePlusSNOMEDTitle.split("+")[1];

            if (snomedDescription.indexOf("^") >= 0) {
                snomedDescription = snomedDescription.split("^")[0];
            }
        }

        if (IMODetail.params.Ctrltext.indexOf("MedicalHx") > -1) {
            IMODetail.params.ParentCtrl = "Clinical_MedicalHx";
        }
        else if (IMODetail.params.Ctrltext.indexOf("HospitalizationHx") > -1) {
            IMODetail.params.ParentCtrl = "Clinical_HospitalizationHx";
        }
        else if (IMODetail.params.Ctrltext.indexOf("SurgicalHx") > -1) {
            IMODetail.params.ParentCtrl = "Clinical_SurgicalHx";
        }
        else if (IMODetail.params.Ctrltext.indexOf("FamilyHx") > -1) {
            IMODetail.params.ParentCtrl = "Clinical_FamilyHx";
        }
        else if (IMODetail.params.Ctrltext.indexOf("BillingInformation") > -1) {
            IMODetail.params.ParentCtrl = "BillingInformation";
        }
        else if (IMODetail.params.Ctrltext.indexOf("OrderSet_Problems") > -1) {
            IMODetail.params.ParentCtrl = "OrderSet_Problems";
        }
        else if (IMODetail.params.Ctrltext.indexOf("Clinical_CustomFormsPreview") > -1) {
            IMODetail.params.ParentCtrl = "Clinical_CustomFormsPreview";
        }
        else if (IMODetail.params.Ctrltext.indexOf("Batch_FaxSend") > -1) {
            IMODetail.params.ParentCtrl = "Batch_FaxSend";
        }
        var textIcdDescriptionFiled = IMODetail.params.Ctrltext;

        if (IMODetail.params.ParentCtrl == "ICDDetail") {

            $("#ICDDetail #txtICDAndDescription").val(textIcdDescriptionFiled);
            $("#ICDDetail #txtICD9Code").val(icd9Code);
            $("#ICDDetail #txtICD9Description").val(icd9Description);
            $("#ICDDetail #txtICD10Code").val(icd10Code);
            $("#ICDDetail #txtICD10Description").val(icd10Description);
            $("#ICDDetail #txtSnomedCode").val(snomedCode);
            $("#ICDDetail #txtSnomedDescription").val(snomedDescription);
            $("#ICDDetail #hfLexiCode").val(IMODetail.params.lexiCodeId);

            setTimeout(function () { $("#ICDDetail #txtICDAndDescription").val(textIcdDescriptionFiled); }, 130);
            $("#ICDDetail #divICDFields").show();
        }

        else if (IMODetail.params.ParentCtrl == "Admin_CCMICDGroups_Detail") {

            $("#CCMICDGroupsDetail #hftxtICD9Code").val(icd9Code);
            $("#CCMICDGroupsDetail #hftxtICD9Description").val(icd9Description);
            $("#CCMICDGroupsDetail #hftxtICD10Code").val(icd10Code);
            $("#CCMICDGroupsDetail #hftxtICD10Description").val(icd10Description);
            $("#CCMICDGroupsDetail #hftxtSNOMEDCode").val(snomedCode);
            $("#CCMICDGroupsDetail #hftxtSNOMEDDescription").val(snomedDescription);

            var ControlsArray = [];
            var thatisSomeShit = (icd9Code + "$" + icd9Description + '*' + icd10Code + '$' + icd10Description + '#' + snomedCode + '$' + snomedDescription);
            ControlsArray.push(thatisSomeShit);
            Admin_CCMICDGroups_Detail.BindICDNValues($("#CCMICDGroupsDetail"), ControlsArray);
        }

        else if (IMODetail.params.ParentCtrl.indexOf("Admin_IMOICD") >= 0) {

            var ContainerCtrl = IMODetail.params.ContainerCtrl;

            var RefHiddenCtrlArray = [];
            RefHiddenCtrlArray = ContainerCtrl.split(",");
            if (RefHiddenCtrlArray.length > 1) {

                var ctrl = RefHiddenCtrlArray[0].substring(5);
                $("#txtICD" + ctrl).val(icd10Code);
                $(" #" + RefHiddenCtrlArray[0]).val(icd9Code);
                $(" #" + RefHiddenCtrlArray[1]).val(icd9Description);
                $(" #" + RefHiddenCtrlArray[2]).val(icd10Code);
                $(" #" + RefHiddenCtrlArray[3]).val(icd10Description);
                $(" #" + RefHiddenCtrlArray[4]).val(snomedCode);
                $(" #" + RefHiddenCtrlArray[5]).val(snomedDescription);
                $("#txtICD" + ctrl).parent("div").attr("data-original-title", icd10Description);
            }

            $("#SupperBillDetail #txtICD_0").val(icd9Code);
            $("#SupperBillDetail #txtICDDescription_0").val(icd9Description);
            $("#SupperBillDetail #txtICD10_0").val(icd10Code);
            $("#SupperBillDetail #hfICD10Description_0").val(icd10Description);
            $("#SupperBillDetail #hfSNOMED_0").val(snomedCode);
            $("#SupperBillDetail #hfSNOMEDDescription_0").val(snomedDescription);
            $("#SupperBillDetail #hfLexiCode_0").val(snomedCode);

            //------------------------------------------------
            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
            if (Admin_IMOICD.params.ParentCtrl.indexOf("Clinical_ProblemLists") >= 0) {
                $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
                $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
                $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
                $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
                $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

                if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                    $('#pnlClinicalProblemLists #ddlSeverity').val('Unspecified Severity');
                } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                    $('#pnlClinicalProblemLists #ddlSeverity').val('Severe Persistent');
                } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                    $('#pnlClinicalProblemLists #ddlSeverity').val('Moderate Persistent');
                } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                    $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Persistent');
                } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                    $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Intermittent');
                } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                    $('#pnlClinicalProblemLists #ddlSeverity').val(6);
                }

                $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
                $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
                // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
                $("#pnlClinicalProblemLists #hfIMOProblem").val(icd10Description);
                var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
                $('#pnlClinicalProblemLists #ulProblemDisease').html(li);
                $("#pnlClinicalProblemLists #ProblemUl").val(icd10Description);
                // End 19/01/2016 Muhammad Irfan for bug # EMR-219
                $("#pnlClinicalProblemLists #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');
            }


            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Favorite_Complaints_Detail") >= 0) {
                if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                }
                else {


                    var currId = -1;
                    $("#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);
                    var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                    var IsAlreadyExist = false;
                    $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease').append(li);
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                        Favorite_Complaints_Detail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                        var isUnload = "false";
                        var txt = $('#pnlFavoriteComplaintsDetail #txtDiagnosis');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }

                    }
                    else {
                        utility.DisplayMessages('Diagnosis already added', 2);

                        $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                    }
                }

            }


            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Favorite_MedicalHistoryDetail") >= 0) {

                var currId = -1;
                $("#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail ul#ulFavMedicalHistoryDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });

                currId = parseInt(currId) + (-1);

                var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_MedicalHistoryDetail.showIcon(this);' onmouseout='Favorite_MedicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_MedicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                var IsAlreadyExist = false;
                $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease').append(li);
                    $(li).trigger('click');
                    item = {}
                    item["ICD9"] = icd9Code;
                    item["ICD10"] = icd10Code;
                    item["ICD10Description"] = icd10Description;
                    item["ICD9Description"] = icd9Description;
                    item["SNOMEDDescription"] = snomedDescription;
                    item["SNOMED"] = snomedCode;
                    Favorite_MedicalHistoryDetail.CPTData.push(item);

                    $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis').val('');


                    var isUnload = "false";
                    var txt = $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis');
                    if (txt.is('[data-popupunload]')) {
                        isUnload = txt.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txt.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages('Diagnose already added', 2);

                    $('#pnlFavoriteMedicalHistoryDetail #txtDiagnose').val('');
                }
                //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
            }

            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Favorite_HospitalizationHistoryDetail") >= 0) {

                var currId = -1;
                $("#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail ul#ulFavHospitalizationHistoryDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });

                currId = parseInt(currId) + (-1);

                var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_HospitalizationHistoryDetail.showIcon(this);' onmouseout='Favorite_HospitalizationHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_HospitalizationHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                var IsAlreadyExist = false;
                $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease').append(li);
                    $(li).trigger('click');
                    item = {}
                    item["ICD9"] = icd9Code;
                    item["ICD10"] = icd10Code;
                    item["ICD10Description"] = icd10Description;
                    item["ICD9Description"] = icd9Description;
                    item["SNOMEDDescription"] = snomedDescription;
                    item["SNOMED"] = snomedCode;
                    Favorite_HospitalizationHistoryDetail.CPTData.push(item);

                    $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis').val('');


                    var isUnload = "false";
                    var txt = $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis');
                    if (txt.is('[data-popupunload]')) {
                        isUnload = txt.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txt.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages('Diagnose already added', 2);

                    $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnose').val('');
                }
            }

            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Favorite_FamilyHistoryDetail") >= 0) {

                var currId = -1;
                $("#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail ul#ulFavFamilyHistoryDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });

                currId = parseInt(currId) + (-1);

                var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_FamilyHistoryDetail.showIcon(this);' onmouseout='Favorite_FamilyHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Favorite_FamilyHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                var IsAlreadyExist = false;
                $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease').append(li);
                    $(li).trigger('click');
                    item = {}
                    item["ICD9"] = icd9Code;
                    item["ICD10"] = icd10Code;
                    item["ICD10Description"] = icd10Description;
                    item["ICD9Description"] = icd9Description;
                    item["SNOMEDDescription"] = snomedDescription;
                    item["SNOMED"] = snomedCode;
                    Favorite_FamilyHistoryDetail.CPTData.push(item);

                    $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

                    var isUnload = "false";
                    var txt = $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis');
                    if (txt.is('[data-popupunload]')) {
                        isUnload = txt.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txt.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages('Diagnose already added', 2);

                    $('#pnlFavoriteFamilyHistoryDetail #txtDiagnose').val('');
                }
                //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
            }

            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Favorite_ProblemsDetail") >= 0) {
                if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                }
                else {


                    var currId = -1;
                    $("#pnlFavoriteProblemsDetail #frmFavoriteProblemsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);
                    var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_ProblemsDetail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                    var IsAlreadyExist = false;
                    $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease').append(li);
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                        Favorite_ProblemsDetail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                        var isUnload = "false";
                        var txt = $('#pnlFavoriteProblemsDetail #txtDiagnosis');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Diagnosis already added', 2);

                        $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                    }
                }

            }


            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Clinical_Cognitive") >= 0) {
                if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                }
                else {
                    if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtCognitiveStatus') {
                        var currId = -1;
                        $("#pnlCognitive #frmCognitive #ulCognitiveStatus li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);

                        var status = "Status";
                        var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + status + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + status + "\");'><i class='fa fa-close'></i></span></a></li>"


                        var IsAlreadyExist = false;
                        $('#pnlCognitive #ulCognitiveStatus li').each(function () {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlCognitive #ulCognitiveStatus').append(li);
                            $(li).trigger('click');
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlCognitive #txtCognitiveStatus').val('');
                            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                            var isUnload = "false";
                            var txt = $('#pnlCognitive #txtCognitiveStatus');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Cognitive Status already added', 2);

                            $('#pnlCognitive #txtCognitiveStatus').val('');
                        }
                    }

                    else if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtFunctionalStatus') {
                        var currId = -1;
                        $("#pnlCognitive #frmCognitive #ulFunctionalStatus li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var functionalStatus = "FunctionalStatus";
                        var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + functionalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveFunctionalStatus($(this), event);'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + functionalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"


                        var IsAlreadyExist = false;
                        $('#pnlCognitive #ulFunctionalStatus li').each(function () {
                            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlCognitive #ulFunctionalStatus').append(li);
                            $(li).trigger('click');
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlCognitive #txtFunctionalStatus').val('');
                            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                            var isUnload = "false";
                            var txt = $('#pnlCognitive #txtFunctionalStatus');
                            if (txt.is('[data-popupunload]')) {
                                isUnload = txt.attr('data-popupunload');
                            }

                            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                                txt.attr("data-popupunload", "false");
                                Admin_IMOICD.UnLoadTab();
                            }
                        }
                        else {
                            utility.DisplayMessages('Functional Status already added', 2);

                            $('#pnlCognitive #txtFunctionalStatus').val('');
                        }
                    }
                }


            }





            else if (Admin_IMOICD.params.ParentCtrl.indexOf("Clinical_Complaints") >= 0) {
                if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                }
                else {
                    var currId = -1;
                    $("#pnlClinicalComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);
                    var li = "<li  id=" + currId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                        "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                            "<span class='removeIconListHover'>" +
                                "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                    "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                    "</span>" +
                        "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                        "<div class='clearfix'></div>" +
                    "</li>";
                    var IsAlreadyExist = false;
                    $('#pnlClinicalComplaints #ulCompliantDisease li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                            IsAlreadyExist = true;
                        } else {
                            if ($(this).text().trim() == +(icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#pnlClinicalComplaints #txtComplaints').val('');
                        Clinical_Complaints.AddInArray(currId, icd10Description, true);//for record of complaints

                        var isUnload = "false";
                        var txt = $('#pnlClinicalComplaints #txtComplaints');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                        Clinical_Complaints.IsUpdate = true;
                    }
                    else {
                        utility.DisplayMessages('Disease already added', 2);

                        $('#pnlClinicalComplaints #txtComplaints').val('');
                    }
                }
            }
            //------------------------------------------------
        }
        else if (IMODetail.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0) {

            var ContainerCtrl = IMODetail.params.ContainerCtrl;

            var RefHiddenCtrlArray = [];
            RefHiddenCtrlArray = ContainerCtrl.split(",");
            if (RefHiddenCtrlArray.length > 1) {
                var ctrt = RefHiddenCtrlArray[0].substring(5);
                $("#txtICD" + ctrt).val(icd10Code);
                $("#txtICD10Description" + ctrt).val(icd10Description);
                $(" #" + RefHiddenCtrlArray[0]).val(icd9Code);
                $(" #" + RefHiddenCtrlArray[1]).val(icd9Description);
                $(" #" + RefHiddenCtrlArray[2]).val(icd10Code);
                $(" #" + RefHiddenCtrlArray[3]).val(icd10Description);
                $(" #" + RefHiddenCtrlArray[4]).val(snomedCode);
                $(" #" + RefHiddenCtrlArray[5]).val(snomedDescription);
                $("#txtICD" + ctrt).parent("div").attr("title", icd10Description).attr("data-original-title", icd10Description).attr("data-toggle", "tooltip").attr("data-placement", "right");
                //Set ToolTip for Comments.
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                EncounterChargeCapture.ValidateICDCodeAndSetDesc($("#txtICD" + ctrt));
            }
            else {

                $("#txtICD" + ContainerCtrl).val(icd10Code);
                $("#txtICD10Description" + ContainerCtrl).val(icd10Description);
                $("#hfICD" + ContainerCtrl).val(icd9Code);
                $("#hfICDDescription" + ContainerCtrl).val(icd9Description);
                $("#hfICD10" + ContainerCtrl).val(icd10Code);
                $("#hfICD10Description" + ContainerCtrl).val(icd10Description);
                $("#hfSNOMED" + ContainerCtrl).val(snomedCode);
                $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
                EncounterChargeCapture.ValidateICDCodeAndSetDesc($("#txtICD" + ContainerCtrl));
            }
        }
        else if (IMODetail.params.ParentCtrl == "SupperBillDetail") {

            var ContainerCtrl = IMODetail.params.ContainerCtrl;

            $("#SupperBillDetail #txtICD" + ContainerCtrl).val(icd9Code);
            $("#SupperBillDetail #txtICDDescription" + ContainerCtrl).val(icd9Description);
            $("#SupperBillDetail #txtICD10" + ContainerCtrl).val(icd10Code);
            $("#SupperBillDetail #hfICD10Description" + ContainerCtrl).val(icd10Description);
            $("#SupperBillDetail #hfSNOMED" + ContainerCtrl).val(snomedCode);
            $("#SupperBillDetail #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
            $("#SupperBillDetail #hfLexiCode" + ContainerCtrl).val(IMODetail.params.lexiCodeId);
        }
        else if (IMODetail.params.ParentCtrl == "chargeSearchDetail") {
            var ContainerCtrl = IMODetail.params.ContainerCtrl;
            setTimeout(function () {
                $("#chargeSearchDetail #txtICD" + ContainerCtrl).val(icd10Code);
                $("#chargeSearchDetail #hfICD" + ContainerCtrl).val(icd9Code);
                $("#chargeSearchDetail #hfICDDescription" + ContainerCtrl).val(icd9Description);
                $("#chargeSearchDetail #hfICD10" + ContainerCtrl).val(icd10Code);
                $("#chargeSearchDetail #hfICD10Description" + ContainerCtrl).val(icd10Description);
                $("#chargeSearchDetail #hfSNOMED" + ContainerCtrl).val(snomedCode);
                $("#chargeSearchDetail #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
            }, 130);
            //setTimeout(function () { $("#chargeSearchDetail #txtICD" + ContainerCtrl).val(icd9Code); $("#hfICD" + ContainerCtrl).val(icd9Code); $("#hfICDDescription" + ContainerCtrl).val(icd9Description); $("#hfICD10" + ContainerCtrl).val(icd10Code); $("#hfICD10Description" + ContainerCtrl).val(icd10Description); $("#hfSNOMED" + ContainerCtrl).val(snomedCode); $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription); }, 110);
        }
        else if (IMODetail.params.ParentCtrl == "BillingInformation") {
            var ControlsArray = [];
            if (IMODetail.params.ContainerCtrl != null)
                ControlsArray = IMODetail.params.ContainerCtrl.split(',');
            else if (BillingInformation != null && BillingInformation.params != null && BillingInformation.params.RefHiddenCtrl != null)
                ControlsArray = BillingInformation.params.RefHiddenCtrl.split(',');

            if (ControlsArray.length > 0) {

                for (var index in ControlsArray) {

                    if (ControlsArray[index].indexOf("hfICDCode9") >= 0)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Code)

                    if (ControlsArray[index].indexOf("hfICDDescription9") >= 0)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Description);

                    if (ControlsArray[index].indexOf("hfICDCode10") >= 0)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code);

                    if (ControlsArray[index].indexOf("hfICDDescription10") >= 0)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Description);

                    if (ControlsArray[index].indexOf("hfSNOMEDCode") >= 0)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedCode);

                    if (ControlsArray[index].indexOf("hfSNOMEDDescription") >= 0)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedDescription);

                    if (ControlsArray[index].indexOf("txtDisease_") >= 0) {
                        // $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + icd10Description);
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code + ' - ' + icd10Description);
                    }

                    if (icd10Code == '' && icd9Code == '') {
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val('')
                    }

                }
            }
        }
        else if (IMODetail.params.ParentCtrl == "Clinical_ProblemLists") {

            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
            $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
            $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
            $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
            $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
            $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

            if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Unspecified Severity');
            } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Severe Persistent');
            } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Moderate Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Intermittent');
            } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val(6);
            }

            $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
            $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
            // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
            $("#pnlClinicalProblemLists #hfIMOProblem").val(icd10Description);
            var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
            $('#pnlClinicalProblemLists #ulProblemDisease').html(li);
            $("#pnlClinicalProblemLists #ProblemUl").val(icd10Description);
            // End 19/01/2016 Muhammad Irfan for bug # EMR-219
        }

            //Start 8/02/2016 Muhammad Ahmad Imran Chief Compliant IMO search'
        else if (IMODetail.params.ParentCtrl == "Clinical_Complaints") {
            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {


                var currId = -1;
                $("#pnlClinicalComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);
                var li = "<li  id=" + currId + " onclick='Clinical_Complaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                        "<span class='removeIconListHover'>" +
                            "<a href='#' onclick='Clinical_Complaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                "<a href='#' onclick='Clinical_Complaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                "</span>" +
                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_Complaints.updateComplaintsText(this, event);'/>" +
                    "<div class='clearfix'></div>" +
                "</li>";
                var IsAlreadyExist = false;
                $('#pnlClinicalComplaints #ulCompliantDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                        IsAlreadyExist = true;
                    } else {
                        if ($(this).text().trim() == (icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                            IsAlreadyExist = true;
                        }
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlClinicalComplaints #ulCompliantDisease').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlClinicalComplaints #txtComplaints').val('');
                    Clinical_Complaints.AddInArray(currId, icd10Description, true);//for record of complaints
                    Clinical_Complaints.IsUpdate = true;
                }
                else {
                    utility.DisplayMessages('Disease already added', 2);

                    $('#pnlClinicalComplaints #txtComplaints').val('');
                }
            }

        }
            //End 8/02/2016 Muhammad Ahmad Imran Chief Compliant IMO search'
        else if (IMODetail.params.ParentCtrl == "Clinical_HPIComplaints") {
            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {


                var currId = -1;
                $("#pnlClinicalHPIComplaints #frmClinicalComplaints ul#ulCompliantDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);
                var li = "<li  id=" + currId + " onclick='Clinical_HPIComplaints.fillChiefComplaints(this, event);'  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\">" +
                    "<a href='#' class='pull-left pr-xlg'><span class='complaint-text'>" + (icd10Code != '' ? icd10Code + " - " : "") + icd10Description + "</span></a>" +
                        "<span class='removeIconListHover'>" +
                            "<a href='#' onclick='Clinical_HPIComplaints.editComplaintsText(this, event);'><i class='fa fa-edit blue'></i></a>" +
			                "<a href='#' onclick='Clinical_HPIComplaints.deleteChiefComplaint(" + currId + ", event);'><i class='fa fa-times red'></i></a>" +
		                "</span>" +
                    "<input type='text' class='edit-complaint form-control hidden' onblur='Clinical_HPIComplaints.updateComplaintsText(this, event);'/>" +
                    "<div class='clearfix'></div>" +
                "</li>";
                var IsAlreadyExist = false;
                $('#pnlClinicalHPIComplaints #ulCompliantDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                        IsAlreadyExist = true;
                    } else {
                        if ($(this).text().trim() == (icd10Code != '' ? icd10Code + " - " : "") + icd10Description.trim()) {
                            IsAlreadyExist = true;
                        }
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlClinicalHPIComplaints #ulCompliantDisease').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlClinicalHPIComplaints #txtComplaints').val('');
                    Clinical_HPIComplaints.AddInArray(currId, icd10Description, true);//for record of complaints
                    Clinical_HPIComplaints.IsUpdate = true;
                }
                else {
                    utility.DisplayMessages('Disease already added', 2);

                    $('#pnlClinicalHPIComplaints #txtComplaints').val('');
                }
            }

        }
            //Start 22/03/2016 Muhammad Ahmad Imran favorite Chief Compliant IMO search'
        else if (IMODetail.params.ParentCtrl == "Favorite_Complaints_Detail") {
            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {


                var currId = -1;
                $("#pnlFavoriteComplaintsDetail #frmFavoriteComplaintsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);
                var li = "<li  id=" + currId + "  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'  class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                var IsAlreadyExist = false;
                $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlFavoriteComplaintsDetail #ulFavCompliantDisease').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                    Favorite_Complaints_Detail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints
                }
                else {
                    utility.DisplayMessages('Diagnosis already added', 2);

                    $('#pnlFavoriteComplaintsDetail #txtDiagnosis').val('');
                }
            }

        }

            //Start 22/03/2016 Muhammad Ahmad Imran Chief favorite Chief Compliant IMO search'
            //---------------------------------------------
            //Start 22/03/2016 By Khaleel Ur Rehman.
        else if (IMODetail.params.ParentCtrl == "Favorite_ProblemsDetail") {
            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {


                var currId = -1;
                $("#pnlFavoriteProblemsDetail #frmFavoriteProblemsDetail ul#ulFavCompliantDisease li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);
                var li = "<li  id=" + currId + " icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_ProblemsDetail.deleteFavChiefComplaint(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                var IsAlreadyExist = false;
                $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease li').each(function () {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#pnlFavoriteProblemsDetail #ulFavCompliantDisease').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                    Favorite_ProblemsDetail.AddInArray(currId, icd9Code, icd10Code, snomedCode, icd10Description, true, null, icd9Description, snomedDescription);//for record of complaints

                    var isUnload = "false";
                    var txt = $('#pnlFavoriteProblemsDetail #txtDiagnosis');
                    if (txt.is('[data-popupunload]')) {
                        isUnload = txt.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txt.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages('Diagnosis already added', 2);

                    $('#pnlFavoriteProblemsDetail #txtDiagnosis').val('');
                }
            }

        }
            //kr
            //End 22/03/2016 By Khaleel Ur Rehman.
            //----------------------------------------------
        else if (IMODetail.params.ParentCtrl == "OrderSet_Problems") {

            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
            $("#" + OrderSet_Problems.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
            $("#" + OrderSet_Problems.params.PanelID + " #ddlSeverity").prop("disabled", false);
            $("#" + OrderSet_Problems.params.PanelID + " #dpStartDate").prop("disabled", false);
            $("#" + OrderSet_Problems.params.PanelID + " #dpEndDate").prop("disabled", false);
            $("#" + OrderSet_Problems.params.PanelID + " #txtComments").prop("disabled", false);

            if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
            } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
            } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
            } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                $("#" + OrderSet_Problems.params.PanelID + ' #ddlSeverity').val(6);
            }

            $("#" + OrderSet_Problems.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
            $("#" + OrderSet_Problems.params.PanelID + " #txtProblems").val(icd10Description);
            $("#" + OrderSet_Problems.params.PanelID + " #hfIMOProblem").val(icd10Description);
            var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
            $("#" + OrderSet_Problems.params.PanelID + ' #ulProblemDisease').html(li);
            $("#" + OrderSet_Problems.params.PanelID + " #frmClinicalProblemLists").bootstrapValidator('revalidateField', 'ProblemName');//kr
            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && Admin_IMOICD.params.length >= 0)
                Admin_IMOICD.UnLoadTab();



        }
        else if (IMODetail.params.ParentCtrl == "CCM_Patient_Hub") {
            CCM_Patient_Hub.ResetDiagnosis();
            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #ddlSeverity").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #dpStartDate").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #dpEndDate").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #txtComments").prop("disabled", false);

            if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
            } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
            } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
            } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val(6);
            }

            $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
            $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val(icd10Description);

            $("#" + CCM_Patient_Hub.params.PanelID + " #hfIMOProblem").val(icd10Description);
            var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
            $("#" + CCM_Patient_Hub.params.PanelID + ' #ulProblemDisease').html(li);
            $("#" + CCM_Patient_Hub.params.PanelID + ' #ProblemUl').val(icd10Description);

        }
            //Start 11/01/2016 Muhammad Irfan MedicalHx IMO search\'' + patvisitname + '\'


        else if (IMODetail.params.ParentCtrl != null && (IMODetail.params.ParentCtrl.toLowerCase() == "clinicaltabmedicalhx" || IMODetail.params.ParentCtrl == "Clinical_MedicalHx")) {

            var currId = -1;
            $("#pnlClinicalMedicalHx #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            console.log(currId);
            var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;


            $('#pnlClinicalMedicalHx #ulMedicalDisease li').each(function () {
                if ($(this).attr('icd9Code') == null) {
                    if ($(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                        IsAlreadyExist = true;
                    }
                }
                else {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                        IsAlreadyExist = true;
                    }
                }
            });



            if (!IsAlreadyExist) {
                $('#pnlClinicalMedicalHx #ulMedicalDisease').append(li);
                $(li).trigger('click');
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#pnlClinicalMedicalHx #txtDisease').val('');

                if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var diseaseDetails = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
                    $(diseaseDetails).resetAllControls(null);
                    var diseaseData = $(diseaseDetails).getMyJSONByName();
                    Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData, disease);
                }
            }
            else {
                utility.DisplayMessages('Disease already added', 2);

                $('#pnlClinicalMedicalHx #txtDisease').val('');
            }
        }

            //End 11/01/2016 Muhammad Irfan MedicalHx IMO search
            //Start 20/01/2016 Farooq Ahmad HospitalizationHx IMO search
        else if (IMODetail.params.ParentCtrl == "Clinical_HospitalizationHx") {


            var currId = -1;
            //Start 11/02/2016 Abid Ali, for bug# 311
            $("#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #HospitalizationHxDisease ul#ulHospitalizationDisease li[id*='-']").each(function (i, item) {
                //End 11/02/2016 Abid Ali, for bug# 311
                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;

            $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease li').each(function () {
                if ($(this).attr('icd9Code') == null) {
                    if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                        IsAlreadyExist = true;
                    }
                }
                else {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                        IsAlreadyExist = true;
                    }
                }
            });


            if (!IsAlreadyExist) {
                $('#pnlClinicalHospitalizationHx #ulHospitalizationDisease').append(li);
                $(li).trigger('click');
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#pnlClinicalHospitalizationHx #txtDisease').val('');

                if (Clinical_HospitalizationHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                    Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData, disease);
                }
            }
            else {
                //alert('Diagnose already added');
                utility.DisplayMessages('Diagnose already added', 2);
                $('#pnlClinicalHospitalizationHx #txtDisease').val('');
            }


        }
            //End 20/01/2016 Farooq Ahmad HospitalizationHx IMO search
            /*Start 20/01/2016 Muhammad Irfan Clinical_FamilyHx IMO search*/
        else if (IMODetail.params.ParentCtrl == "Clinical_FamilyHx") {

            var currId = -1;
            $("#pnlClinicalFamilyHx #frmClinicalFamilyHx #FamilyDisease ul#ulFamilyHxDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            console.log(currId);
            var li = "<li  id=" + currId + " onclick='Clinical_FamilyHx.fillFamilyHxDisease(this, event);' onmouseover='Clinical_FamilyHx.showIcon(this);' onmouseout='Clinical_FamilyHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_FamilyHx.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#pnlClinicalFamilyHx #ulFamilyHxDisease li').each(function () {
                if ($(this).attr('icd9Desc') == null) {
                    if ($(this).text().toLowerCase() == icd9Description.toLowerCase()) {
                        IsAlreadyExist = true;
                    }
                } else {
                    if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                        IsAlreadyExist = true;
                    }
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlClinicalFamilyHx #ulFamilyHxDisease').append(li);
                $(li).trigger('click');
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#pnlClinicalFamilyHx #txtDisease').val('');

                if (Clinical_FamilyHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var diseaseId = currId;
                    var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var detailsdata = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').clone();
                    $(detailsdata).resetAllControls(null);
                    var diseaseData = $(detailsdata).getMyJSONByName();
                    Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, diseaseData, disease);
                    $('#' + Clinical_FamilyHx.params.PanelID + " #ulFamilyMember li.active").find("a").css("color", "green");

                }
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);
                $('#pnlClinicalFamilyHx #txtDisease').val('');
            }
        }
            /*End 20/01/2016 Muhammad Irfan Clinical_FamilyHx IMO search*/

            //Start// 11/06/2016 //Ahmad Raza// FamilyHx Favorite list IMO search
        else if (IMODetail.params.ParentCtrl == "Favorite_FamilyHistoryDetail") {

            var currId = -1;
            $("#pnlFavoriteFamilyHistoryDetail #frmFavoriteFamilyHistoryDetail ul#ulFavFamilyHistoryDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_FamilyHistoryDetail.showIcon(this);' onmouseout='Favorite_FamilyHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_FamilyHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

            var IsAlreadyExist = false;
            $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlFavoriteFamilyHistoryDetail #ulFavFamilyHistoryDisease').append(li);
                $(li).trigger('click');
                item = {}
                item["ICD9"] = icd9Code;
                item["ICD10"] = icd10Code;
                item["ICD10Description"] = icd10Description;
                item["ICD9Description"] = icd9Description;
                item["SNOMEDDescription"] = snomedDescription;
                item["SNOMED"] = snomedCode;
                Favorite_FamilyHistoryDetail.CPTData.push(item);

                $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

                var isUnload = "false";
                var txt = $('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis');
                if (txt.is('[data-popupunload]')) {
                    isUnload = txt.attr('data-popupunload');
                }

                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                    txt.attr("data-popupunload", "false");
                    Admin_IMOICD.UnLoadTab();
                }
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);

                $('#pnlFavoriteFamilyHistoryDetail #txtDiagnose').val('');
            }
            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');
        }
            //End// 11/06/2016 //Ahmad Raza// FamilyHx Favorite list IMO search

            //Start// 11/06/2016 //Ahmad Raza// MedicalHx Favorite list IMO search
        else if (IMODetail.params.ParentCtrl == "Favorite_MedicalHistoryDetail") {

            var currId = -1;
            $("#pnlFavoriteMedicalHistoryDetail #frmFavoriteMedicalHistoryDetail ul#ulFavMedicalHistoryDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_MedicalHistoryDetail.showIcon(this);' onmouseout='Favorite_MedicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_MedicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

            var IsAlreadyExist = false;
            $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlFavoriteMedicalHistoryDetail #ulFavMedicalHistoryDisease').append(li);
                $(li).trigger('click');
                item = {}
                item["ICD9"] = icd9Code;
                item["ICD10"] = icd10Code;
                item["ICD10Description"] = icd10Description;
                item["ICD9Description"] = icd9Description;
                item["SNOMEDDescription"] = snomedDescription;
                item["SNOMED"] = snomedCode;
                Favorite_MedicalHistoryDetail.CPTData.push(item);

                $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis').val('');

                var isUnload = "false";
                var txt = $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis');
                if (txt.is('[data-popupunload]')) {
                    isUnload = txt.attr('data-popupunload');
                }

                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                    txt.attr("data-popupunload", "false");
                    Admin_IMOICD.UnLoadTab();
                }
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);

                $('#pnlFavoriteMedicalHistoryDetail #txtDiagnose').val('');
            }
            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

        }
            //End// 11/06/2016 //Ahmad Raza// MedicalHx Favorite list IMO search

        else if (IMODetail.params.ParentCtrl == "Favorite_HospitalizationHistoryDetail") {

            var currId = -1;
            $("#pnlFavoriteHospitalizationHistoryDetail #frmFavoriteHospitalizationHistoryDetail ul#ulFavHospitalizationHistoryDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_HospitalizationHistoryDetail.showIcon(this);' onmouseout='Favorite_HospitalizationHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Favorite_HospitalizationHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

            var IsAlreadyExist = false;
            $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlFavoriteHospitalizationHistoryDetail #ulFavHospitalizationHistoryDisease').append(li);
                $(li).trigger('click');
                item = {}
                item["ICD9"] = icd9Code;
                item["ICD10"] = icd10Code;
                item["ICD10Description"] = icd10Description;
                item["ICD9Description"] = icd9Description;
                item["SNOMEDDescription"] = snomedDescription;
                item["SNOMED"] = snomedCode;
                Favorite_HospitalizationHistoryDetail.CPTData.push(item);

                $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis').val('');

                var isUnload = "false";
                var txt = $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis');
                if (txt.is('[data-popupunload]')) {
                    isUnload = txt.attr('data-popupunload');
                }

                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                    txt.attr("data-popupunload", "false");
                    Admin_IMOICD.UnLoadTab();
                }
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);

                $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnose').val('');
            }

        }













            //Start// 11/06/2016 //Ahmad Raza// SurgicalHx Favorite list IMO search
        else if (IMODetail.params.ParentCtrl == "Favorite_SurgicalHistoryDetail") {

            var currId = -1;
            $("#pnlFavoriteSurgicalHistoryDetail #frmFavoriteSurgicalHistoryDetail ul#ulFavSurgicalHistoryDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_SurgicalHistoryDetail.showIcon(this);' onmouseout='Favorite_SurgicalHistoryDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Code + " - " + icd9Description + "<span class='removeIconListHover' onclick='Favorite_SurgicalHistoryDetail.deleteCPTFromCPTData(this,\"" + icd9Code + "\",\"" + icd9Description + "\",\"" + icd10Code + "\",\"" + icd10Description + "\",\"" + snomedCode + "\",\"" + snomedDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

            var IsAlreadyExist = false;
            $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease').append(li);
                $(li).trigger('click');
                var item = {};
                item["ICD9"] = icd9Code;
                item["ICD10"] = icd10Code;
                item["ICD10Description"] = icd10Description;
                item["ICD9Description"] = icd9Description;
                item["SNOMEDDescription"] = snomedDescription;
                item["SNOMED"] = snomedCode;
                Favorite_SurgicalHistoryDetail.CPTData.push(item);

                $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis').val('');

                var isUnload = "false";
                var txt = $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis');
                if (txt.is('[data-popupunload]')) {
                    isUnload = txt.attr('data-popupunload');
                }

                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                    txt.attr("data-popupunload", "false");
                    Admin_IMOICD.UnLoadTab();
                }
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);

                $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnose').val('');
            }
            //$('#pnlFavoriteFamilyHistoryDetail #txtDiagnosis').val('');

        }
            //End// 11/06/2016 //Ahmad Raza// SurgicalHx Favorite list IMO search

            //Start 31-03-016 //Ahmad Raza// PlanOfCare IMO search
        else if (IMODetail.params.ParentCtrl == "Clinical_PlanOfCare") {

            var currId = -1;
            $("#pnlPlanOfCare #frmPlanOfCare ul#ulPlanOfCareGoal li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            console.log(currId);
            var li = "<li  id=" + currId + " onclick='Clinical_PlanOfCare.fillPlanOfCareGoal(this, event);' onmouseover='Clinical_PlanOfCare.showIcon(this);' onmouseout='Clinical_PlanOfCare.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_PlanOfCare.activeInActive($(this), event);'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_PlanOfCare.deleteFamilyHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#pnlPlanOfCare #ulPlanOfCareGoal li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlPlanOfCare #ulPlanOfCareGoal').append(li);
                $(li).trigger('click');
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#pnlPlanOfCare #txtGoal').val('');
            }
            else {
                utility.DisplayMessages('Goal already added', 2);
                $('#pnlPlanOfCare #txtGoal').val('');
            }
        }
        if (IMODetail.params.ParentCtrl == "Clinical_CustomFormsPreview") {
            var txtBoxCtrlParentId = IMODetail.params.ContainerTextBoxParentId;
            if (txtBoxCtrlParentId) {
                var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + txtBoxCtrlParentId);
                var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');

                var currId = -1;
                $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                    currId = $(this).attr("id");
                });

                currId = parseInt(currId) + (-1);

                var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                var IsAlreadyExist = false;

                $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                    if ($(this).attr('icd9Code') == null) {
                        if (!$(this).hasClass('hidden') && $(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                            IsAlreadyExist = true;
                        }
                    }
                    else {
                        if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                            IsAlreadyExist = true;
                        }
                    }
                });

                if (!IsAlreadyExist) {

                    var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                    if (problemsExist == 0) {
                        var problemsDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .problemsListDiv').clone().removeClass('problemsListDiv');
                        $(problemsDiv).find("ul#customFormProblemsList").append(li);
                        $(problemsDiv).css('display', 'block');
                        $(selectedProblemTool).append($(problemsDiv));
                        $(selectedProblemTool).css('height', 'auto');
                    }
                    else {
                        $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                        $(selectedProblemTool).children().last().css('display', 'block');
                    }

                    Clinical_CustomFormsPreview.saveProblem(li, txtBoxCtrlParentId);

                    $(txtBoxCtrl).val('');
                    if ($(txtBoxCtrl).attr('style')) {
                        $(txtBoxCtrl).removeAttr('style');
                    }

                    var isUnload = "false";
                    if ($(txtBoxCtrl).is('[data-popupunload]')) {
                        isUnload = $(txtBoxCtrl).attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        $(txtBoxCtrl).attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages('Problem already added', 2);
                    $(txtBoxCtrl).val('');
                }
            }
        }

        else if (IMODetail.params.ParentCtrl == "Batch_FaxSend") {
            var txtBoxCtrlParentId = IMODetail.params.ContainerTextBoxParentId;
            if (txtBoxCtrlParentId) {
                var selectedProblemTool = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend').find('#' + txtBoxCtrlParentId);
                var txtBoxCtrl = $(selectedProblemTool).find('#txtProblemsCustomForm');

                var currId = -1;
                $(selectedProblemTool).find("ul#customFormProblemsList li[id*='-']").each(function (i, item) {
                    currId = $(this).attr("id");
                });

                currId = parseInt(currId) + (-1);

                var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

                var IsAlreadyExist = false;

                $(selectedProblemTool).find('ul#customFormProblemsList li').each(function () {
                    if ($(this).attr('icd9Code') == null) {
                        if (!$(this).hasClass('hidden') && $(this).attr('icd9Desc').toLowerCase() == icd9Description.toLowerCase()) {
                            IsAlreadyExist = true;
                        }
                    }
                    else {
                        if (!$(this).hasClass('hidden') && $(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                            $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                            $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                            IsAlreadyExist = true;
                        }
                    }
                });

                if (!IsAlreadyExist) {

                    var problemsExist = $(selectedProblemTool).find("ul#customFormProblemsList").length;

                    if (problemsExist == 0) {
                        var problemsDiv = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend .problemsListDiv').clone().removeClass('problemsListDiv');
                        $(problemsDiv).find("ul#customFormProblemsList").append(li);
                        $(problemsDiv).css('display', 'block');
                        $(selectedProblemTool).append($(problemsDiv));
                        $(selectedProblemTool).css('height', 'auto');
                    }
                    else {
                        $(selectedProblemTool).find("ul#customFormProblemsList").append(li);
                        $(selectedProblemTool).children().last().css('display', 'block');
                    }

                    $(txtBoxCtrl).val('');
                    if ($(txtBoxCtrl).attr('style')) {
                        $(txtBoxCtrl).removeAttr('style');
                    }

                    var isUnload = "false";
                    if ($(txtBoxCtrl).is('[data-popupunload]')) {
                        isUnload = $(txtBoxCtrl).attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        $(txtBoxCtrl).attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages('Problem already added', 2);
                    $(txtBoxCtrl).val('');
                }
            }
        }
        else if (IMODetail.params.ParentCtrl == "Clinical_Cognitive") {

            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {
                if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtCognitiveStatus') {
                    var currId = -1;
                    $("#pnlCognitive #frmCognitive #ulCognitiveStatus li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);
                    var status = "Status";
                    //old //var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"'Status'\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"'Status'\");'><i class='fa fa-close red'></i></div></a></li>"

                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + status + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + status + "\");'><i class='fa fa-close'></i></span></a></li>"


                    var IsAlreadyExist = false;
                    $('#pnlCognitive #ulCognitiveStatus li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlCognitive #ulCognitiveStatus').append(li);
                        $(li).trigger('click');
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#pnlCognitive #txtCognitiveStatus').val('');
                        //     Clinical_Complaints.AddInArray(currId, icd10Description, true);
                        var isUnload = "false";
                        var txt = $('#pnlCognitive #txtCognitiveStatus');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Cognitive Status already added', 2);

                        $('#pnlCognitive #txtCognitiveStatus').val('');
                    }
                }

                else if ($('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val() == 'txtFunctionalStatus') {
                    var currId = -1;
                    $("#pnlCognitive #frmCognitive #ulFunctionalStatus li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);
                    var functionalStatus = "FunctionalStatus";
                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + functionalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveFunctionalStatus($(this), event);'>" + icd10Description + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + functionalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"

                    var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"'FunctionalStatus'\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a onclick='Clinical_Cognitive.activeInActiveFunctionalStatus($(this), event);'>" + icd10Description + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"'FunctionalStatus'\");'><i class='fa fa-close red'></i></div></a></li>"


                    var IsAlreadyExist = false;
                    $('#pnlCognitive #ulFunctionalStatus li').each(function () {
                        if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                        $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                        $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlCognitive #ulFunctionalStatus').append(li);
                        $(li).trigger('click');
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#pnlCognitive #txtFunctionalStatus').val('');
                        //     Clinical_Complaints.AddInArray(currId, icd10Description, true);
                        var isUnload = "false";
                        var txt = $('#pnlCognitive #txtFunctionalStatus');
                        if (txt.is('[data-popupunload]')) {
                            isUnload = txt.attr('data-popupunload');
                        }

                        if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                            txt.attr("data-popupunload", "false");
                            Admin_IMOICD.UnLoadTab();
                        }
                    }
                    else {
                        utility.DisplayMessages('Functional Status already added', 2);

                        $('#pnlCognitive #txtFunctionalStatus').val('');
                    }
                }
            }
        }


            //End 31-03-016 //Ahmad Raza// PlanOfCare IMO search

            //Start//02-03-2016//Ahmad Raza // ClinicalCDSDetail IMO Search
        else if (IMODetail.params.ParentCtrl != null && (IMODetail.params.ParentCtrl.toLowerCase() == "adminTabCDS" || IMODetail.params.ParentCtrl == "ClinicalCDSDetail")) {

            var currId = -1;
            $("#ClinicalCDSDetail #frmClinicalCDSDetail #dvProblemList ul#ulCDSProblemList li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            console.log(currId);
            var li = "<li   data=\"" + icd10Description + "\"   id=" + currId + " onclick='' onmouseover='ClinicalCDSDetail.showIcon(this);' onmouseout='ClinicalCDSDetail.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#ClinicalCDSDetail #ulCDSProblemList li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {

                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#ClinicalCDSDetail #ulCDSProblemList').append(li);
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#ClinicalCDSDetail #txtCDSProblemList').val('');
                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
                    Admin_IMOICD.UnLoadTab();
            }
            else {
                utility.DisplayMessages('ProblemList already added', 2);

                $('#ClinicalCDSDetail #txtCDSProblemList').val('');
            }
        }
            //End//02-03-2016//Ahmad Raza // ClinicalCDSDetail IMO Search

            //Start//02-03-2016//Ahmad Raza // Clinical_OrderSetDetails IMO Search
        else if (IMODetail.params.ParentCtrl != null && (IMODetail.params.ParentCtrl.toLowerCase() == "adminTabCDS" || IMODetail.params.ParentCtrl == "Clinical_OrderSetDetails")) {

            var currId = -1;
            $("#pnlClinicalOrderSetDetails #frmOrderSetDetails #dvProblemList ul#ulOrderSetProblemList li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            console.log(currId);
            var li = "<li   data=\"" + icd10Description + "\"   id=" + currId + " onclick='' onmouseover='Clinical_OrderSetDetails.showIcon(this);' onmouseout='Clinical_OrderSetDetails.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList li').each(function () {
                    if ($(this).attr('icd10Code') == icd10Code) {
                        IsAlreadyExist = true;
                    }
            });

            if (!IsAlreadyExist) {
                $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList').append(li);
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');
                if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
                    Admin_IMOICD.UnLoadTab();
            }
            else {
                utility.DisplayMessages('ProblemList already added', 2);

                $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');
            }
        }
            //End//10-06-2017//Ahmad imran // Clinical_OrderSetDetails IMO Search



            //start 11/01/2016 syed zia ,Surgical IMO search
        else if (IMODetail.params.ParentCtrl == "Clinical_SurgicalHx") {

            var currId = -1;
            $("#pnlClinicalSurgicalHx #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);

            console.log(currId);
            var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd9Description + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

            var IsAlreadyExist = false;
            $('#pnlClinicalSurgicalHx #ulSurgicalDisease li').each(function () {
                if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                    $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                    $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlClinicalSurgicalHx #ulSurgicalDisease').append(li);
                $(li).trigger('click');
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $('#pnlClinicalSurgicalHx #txtDisease').val('');
            }
            else {
                utility.DisplayMessages('Diagnose already added', 2);
                $('#pnlClinicalSurgicalHx #txtDisease').val('');
            }
        }
        //End 11/01/2016 syed zia ,Surgical IMO search
        if (IMODetail.params.Ctrltext == "mstrTabReports" || IMODetail.params.ParentCtrl == "mstrTabReports") {
            $("#pnlReportsSSRSDashboard #txtICD").val(icd10Code);
            $("#pnlReportsSSRSDashboard #hfICDCode").val(icd10Code);

        }

        if (IMODetail.params.Ctrltext.indexOf("EncounterChargeCapture") >= 0)
            EncounterChargeCapture.BindICDNValues($("#pnlEncounterChargeCapture"));

        if (IMODetail.params.Ctrltext.indexOf("ClinicalCDSDetail") >= 0) {
            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {
                var currId = -1;
                $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSProblemList li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);


                //Start//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                if ($('#ClinicalCDSDetail #ulCDSProblemList li:first').length > 0) {
                    var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlProblemList" + currId + "' name = 'CDSProblemList" + snomedDescription + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + snomedDescription + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                }
                else {
                    var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + snomedDescription + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                }
                //End//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                var IsAlreadyExist = false;
                $('#ClinicalCDSDetail #ulCDSProblemList li').each(function () {
                    if ($(this).attr('name') == snomedDescription) {

                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#ClinicalCDSDetail #ulCDSProblemList').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#ClinicalCDSDetail #txtCDSProblemList').val('');

                    var isUnload = "false";
                    var txtProblemInCDS = $('#ClinicalCDSDetail #txtCDSProblemList');
                    if (txtProblemInCDS.is('[data-popupunload]')) {
                        isUnload = txtProblemInCDS.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txtProblemInCDS.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                    //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                }
                else {
                    utility.DisplayMessages('Problem List already added', 2);

                    $('#ClinicalCDSDetail #txtCDSProblemList').val('');
                }
            }
        }
        if (IMODetail.params.Ctrltext.indexOf("Clinical_OrderSetDetails") >= 0) {
            if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

            }
            else {
                var currId = -1;
                $("#pnlClinicalOrderSetDetails #frmOrderSetDetails ul#ulOrderSetProblemList li[id*='-']").each(function (i, item) {

                    currId = $(this).attr("id");

                });
                currId = parseInt(currId) + (-1);


                //Start//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                //if ($('#pnlClinicalOrderSetDetails #ulOrderSetProblemList li:first').length > 0) {
                //    var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlProblemList" + currId + "' name = 'CDSProblemList" + snomedDescription + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + snomedCode + ' - ' + snomedDescription + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                //}
                //else {
                var li = "<li data=\"" + snomedCode + "\" name='" + snomedDescription + "' id=" + currId + " onclick=''  icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><div class='col-sm-12 col-lg-12'><a href='#'>" + icd9Code + ' - ' + icd10Code + ' - ' + icd10Description + "<span class='removeIconListHover' onclick='Clinical_OrderSetDetails.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                //}
                //End//16-03-2016//Ahmad Raza//logic to add ProblemListOperator dropdown
                var IsAlreadyExist = false;
                $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList li').each(function () {
                        if ($(this).attr('icd10Code') == icd10Code) {
                            IsAlreadyExist = true;
                        }
                });

                if (!IsAlreadyExist) {
                    $('#pnlClinicalOrderSetDetails #ulOrderSetProblemList').append(li);
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');

                    var isUnload = "false";
                    var txtProblemInCDS = $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList');
                    if (txtProblemInCDS.is('[data-popupunload]')) {
                        isUnload = txtProblemInCDS.attr('data-popupunload');
                    }

                    if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        txtProblemInCDS.attr("data-popupunload", "false");
                        Admin_IMOICD.UnLoadTab();
                    }
                    //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

                }
                else {
                    utility.DisplayMessages('Problem List already added', 2);

                    $('#pnlClinicalOrderSetDetails #txtOrderSetProblemList').val('');
                }
            }
        }
        UnloadActionPan(IMODetail.params.TabID, "IMODetail");
        if (IMODetail.params.TabID != "BillingInformation" && IMODetail.params.TabID != "Favorite_ProblemsDetail" && IMODetail.params.TabID != "Favorite_Complaints_Detail" && IMODetail.params.TabID != "Clinical_Cognitive" && IMODetail.params.TabID != "Clinical_Complaints" && IMODetail.params.TabID != "EncounterChargeCapture" && IMODetail.params.TabID != "Admin_CCMICDGroups_Detail" && IMODetail.params.TabID != "Clinical_ProblemLists"
             && IMODetail.params.TabID != "CCM_Patient_Hub" && IMODetail.params.TabID != "OrderSet_Problems" && IMODetail.params.TabID != "Clinical_MedicalHx" && IMODetail.params.TabID != "Clinical_FamilyHx" && IMODetail.params.TabID != "Clinical_HospitalizationHx" && IMODetail.params.TabID != "Favorite_FamilyHistoryDetail" && IMODetail.params.TabID != "Favorite_MedicalHistoryDetail" && IMODetail.params.TabID != "Favorite_HospitalizationHistoryDetail" && IMODetail.params.TabID != "Favorite_Complaints_Detail" && IMODetail.params.TabID != "Clinical_CustomFormsPreview" && IMODetail.params.TabID != "Clinical_HPIComplaints" && IMODetail.params.TabID != "Batch_FaxSend" && IMODetail.params.TabID != "Admin_IMOICD") {
            Admin_IMOICD.UnLoadTab();
        }

        if (IMODetail.params.ParentCtrl != "ICDDetail") {
            if (IMODetail.params.ParentCtrl != "SupperBillDetail" && IMODetail.params.ParentCtrl != "chargeSearchDetail" && IMODetail.params.ParentCtrl != "EncounterChargeCapture") {
                // && IMODetail.params.ParentCtrl.substring(0, 22).substring(22) != ""
                if (IMODetail.params.ParentCtrl != "BillingInformation" && IMODetail.params.ParentCtrl != "Favorite_Complaints_Detail" && IMODetail.params.ParentCtrl != "Favorite_ProblemsDetail" && IMODetail.params.ParentCtrl != "Favorite_FamilyHistoryDetail" && IMODetail.params.ParentCtrl != "Clinical_Cognitive" && IMODetail.params.ParentCtrl != "Clinical_Complaints" && IMODetail.params.TabID != "Admin_CCMICDGroups_Detail" && IMODetail.params.TabID != "Clinical_ProblemLists"
                     && IMODetail.params.TabID != "CCM_Patient_Hub" && IMODetail.params.TabID != "OrderSet_Problems" && IMODetail.params.TabID != "Clinical_MedicalHx" && IMODetail.params.TabID != "Clinical_FamilyHx" && IMODetail.params.TabID != "Clinical_HospitalizationHx" && IMODetail.params.TabID != "Favorite_FamilyHistoryDetail" && IMODetail.params.TabID != "Favorite_MedicalHistoryDetail" && IMODetail.params.TabID != "Favorite_HospitalizationHistoryDetail" && IMODetail.params.TabID != "Favorite_Complaints_Detail" && IMODetail.params.TabID != "Clinical_CustomFormsPreview" && IMODetail.params.TabID != "Clinical_HPIComplaints" && IMODetail.params.TabID != "Batch_FaxSend" && IMODetail.params.TabID != "Admin_IMOICD") {
                    Admin_IMOICD.UnLoadTab();
                }
            }
            if (Admin_IMOICD.params['fromIcon'] != null && Admin_IMOICD.params['fromIcon'] == "true") {
                Admin_IMOICD.params['fromIcon'] = "false";
                BillingInformation.EnableDisableICDs();
            }
        }
    },

    UnLoad: function () {

        var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "", icd9Code = "", icd9Description = "";
        if (IMODetail.params.ICD10CodePlusICD10Title != undefined) {
            icd10Code = IMODetail.params.ICD10CodePlusICD10Title.split("+")[0];
            icd10Description = IMODetail.params.ICD10CodePlusICD10Title.split("+")[1];
        }

        if (IMODetail.params.SNOMEDCodePlusSNOMEDTitle != undefined) {
            snomedCode = IMODetail.params.SNOMEDCodePlusSNOMEDTitle.split("+")[0];
            snomedDescription = IMODetail.params.SNOMEDCodePlusSNOMEDTitle.split("+")[1];
        }

        if (IMODetail.params.ICD9CodePlusICD9Title != undefined) {
            icd9Code = IMODetail.params.ICD9CodePlusICD9Title.split("+")[0];
            icd9Description = IMODetail.params.ICD9CodePlusICD9Title.split("+")[1];
        }
        var textIcdDescriptionFiled = IMODetail.params.Ctrltext;
        if (IMODetail.params.TabID.indexOf("EncounterChargeCapture") >= 0) {

            var ContainerCtrl = IMODetail.params.ContainerCtrl;
            $("#txtICD" + ContainerCtrl).val(icd10Code);
            $("#txtICD10Description" + ContainerCtrl).val(icd10Description);
            $("#hfICD" + ContainerCtrl).val(icd9Code);
            $("#hfICDDescription" + ContainerCtrl).val(icd9Description);
            $("#hfICD10" + ContainerCtrl).val(icd10Code);
            $("#hfICD10Description" + ContainerCtrl).val(icd10Description);
            $("#hfSNOMED" + ContainerCtrl).val(snomedCode);
            $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
            $("#txtICD" + ContainerCtrl).parent("div").attr("title", icd10Description).attr("data-original-title", icd10Description).attr("data-toggle", "tooltip").attr("data-placement", "right");
            //Set ToolTip for Comments.
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            EncounterChargeCapture.ValidateICDCodeAndSetDesc($("#txtICD" + ContainerCtrl));
        }
        else if (IMODetail.params.ParentCtrl == "SupperBillDetail") {

            var ContainerCtrl = IMODetail.params.ContainerCtrl;

            $("#SupperBillDetail #txtICD" + ContainerCtrl).val(icd9Code);
            $("#SupperBillDetail #txtICDDescription" + ContainerCtrl).val(icd9Description);
            $("#SupperBillDetail #txtICD10" + ContainerCtrl).val(icd10Code);
            $("#SupperBillDetail #hfICD10Description" + ContainerCtrl).val(icd10Description);
            $("#SupperBillDetail #hfSNOMED" + ContainerCtrl).val(snomedCode);
            $("#SupperBillDetail #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
            $("#SupperBillDetail #hfLexiCode" + ContainerCtrl).val(IMODetail.params.lexiCodeId);

        }
        else if (IMODetail.params.ParentCtrl == "chargeSearchDetail") {
            var ContainerCtrl = IMODetail.params.ContainerCtrl;

            $("#pnlBillChargeSearch #txtICD" + ContainerCtrl).val(icd10Code);

            $("#pnlBillChargeSearch #hfICD" + ContainerCtrl).val(icd9Code);
            $("#pnlBillChargeSearch #hfICDDescription" + ContainerCtrl).val(icd9Description);
            $("#pnlBillChargeSearch #hfICD10" + ContainerCtrl).val(icd10Code);
            $("#pnlBillChargeSearch #hfICD10Description" + ContainerCtrl).val(icd10Description);
            $("#pnlBillChargeSearch #hfSNOMED" + ContainerCtrl).val(snomedCode);
            $("#pnlBillChargeSearch #hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);

            setTimeout(function () { $("#pnlBillChargeSearch #txtICD" + ContainerCtrl).val(icd9Code); $("#hfICD" + ContainerCtrl).val(icd9Code); $("#hfICDDescription" + ContainerCtrl).val(icd9Description); $("#hfICD10" + ContainerCtrl).val(icd10Code); $("#hfICD10Description" + ContainerCtrl).val(icd10Description); $("#hfSNOMED" + ContainerCtrl).val(snomedCode); $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription); }, 110);
        }
        else if (IMODetail.params.ParentCtrl == "ICDDetail") {
            $("#ICDDetail #txtICDAndDescription").val(textIcdDescriptionFiled);
            $("#ICDDetail #txtICD9Code").val(icd9Code);
            $("#ICDDetail #txtICD9Description").val(icd9Description);
            $("#ICDDetail #txtICD10Code").val(icd10Code);
            $("#ICDDetail #txtICD10Description").val(icd10Description);
            $("#ICDDetail #txtSnomedCode").val(snomedCode);
            $("#ICDDetail #txtSnomedDescription").val(snomedDescription);
            $("#ICDDetail #hfLexiCode").val(IMODetail.params.lexiCodeId);
            $("#ICDDetail #divICDFields").show();
        }
        else if (IMODetail.params.ParentCtrl == "Clinical_ProblemLists") {
            Clinical_ProblemLists.ResetDiagnosis();
            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
            $("#pnlClinicalProblemLists #ddlChronicityLevel").prop("disabled", false);
            $("#pnlClinicalProblemLists #ddlSeverity").prop("disabled", false);
            $("#pnlClinicalProblemLists #dpStartDate").prop("disabled", false);
            $("#pnlClinicalProblemLists #dpEndDate").prop("disabled", false);
            $("#pnlClinicalProblemLists #txtComments").prop("disabled", false);

            if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Unspecified Severity');
            } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Severe Persistent');
            } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Moderate Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val('Mild Intermittent');
            } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                $('#pnlClinicalProblemLists #ddlSeverity').val(6);
            }

            $("#pnlClinicalProblemLists #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
            $("#pnlClinicalProblemLists #txtProblems").val(icd10Description);
            // Start 19/01/2016 Muhammad Irfan for bug # EMR-219
            $("#pnlClinicalProblemLists #hfIMOProblem").val(icd10Description);
            var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
            $('#pnlClinicalProblemLists #ulProblemDisease').html(li);
            $("#pnlClinicalProblemLists #ProblemUl").val(icd10Description);
        }
        else if (IMODetail.params.ParentCtrl == "BillingInformation") {
            var ControlsArray = [];
            if (IMODetail.params.ContainerCtrl != null)
                ControlsArray = IMODetail.params.ContainerCtrl.split(',');
            else if (BillingInformation != null && BillingInformation.params != null && BillingInformation.params.RefHiddenCtrl != null)
                ControlsArray = BillingInformation.params.RefHiddenCtrl.split(',');

            if (ControlsArray.length > 0) {

                for (var index in ControlsArray) {
                    if (ControlsArray[index].toString().indexOf("txtDisease") > -1) {

                        var Description = '';
                        if (icd10Code != '')
                            Description = icd10Code + ' - ';
                        else if (icd9Code != '')
                            Description = icd9Code + ' - ';

                        if (icd10Description != '')
                            Description = Description + icd10Description;
                        else if (icd9Description != '')
                            Description = Description + icd9Description;

                    }

                    if (ControlsArray[index].toString().indexOf("hfICDCode9") > -1)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Code);
                    if (ControlsArray[index].toString().indexOf("hfICDDescription9") > -1)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd9Description);
                    if (ControlsArray[index].toString().indexOf("hfICDCode10") > -1)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Code);
                    if (ControlsArray[index].toString().indexOf("hfICDDescription10") > -1)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(icd10Description);
                    if (ControlsArray[index].toString().indexOf("hfSNOMEDCode") > -1)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedCode);
                    if (ControlsArray[index].toString().indexOf("hfSNOMEDDescription") > -1)
                        $('#pnlBillingInformation  #' + ControlsArray[index]).val(snomedDescription);
                }
            }
        }
        else if (IMODetail.params.ParentCtrl == "CCM_Patient_Hub") {
            CCM_Patient_Hub.ResetDiagnosis();
            //$("#pnlClinicalProblemLists #txtDiagnosis").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #ddlChronicityLevel").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #ddlSeverity").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #dpStartDate").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #dpEndDate").prop("disabled", false);
            $("#" + CCM_Patient_Hub.params.PanelID + " #txtComments").prop("disabled", false);

            if (icd10Description.toLowerCase().indexOf("unspecified") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Unspecified Severity');
            } else if (icd10Description.toLowerCase().indexOf("severe persistent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Severe Persistent');
            } else if (icd10Description.toLowerCase().indexOf("moderate persistent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Moderate Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild persistent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Persistent');
            } else if (icd10Description.toLowerCase().indexOf("mild intermittent") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val('Mild Intermittent');
            } else if (icd10Description.toLowerCase().indexOf("unknown") >= 0) {
                $("#" + CCM_Patient_Hub.params.PanelID + ' #ddlSeverity').val(6);
            }

            $("#" + CCM_Patient_Hub.params.PanelID + " #txtDiagnosis").val(icd10Code + ' - ' + icd10Description);
            $("#" + CCM_Patient_Hub.params.PanelID + " #txtProblems").val(icd10Description);

            $("#" + CCM_Patient_Hub.params.PanelID + " #hfIMOProblem").val(icd10Description);
            var li = "<li icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#' class='pr-sm'>" + icd10Description + "</a></li>"
            $("#" + CCM_Patient_Hub.params.PanelID + ' #ulProblemDisease').html(li);
            $("#" + CCM_Patient_Hub.params.PanelID + ' #ProblemUl').val(icd10Description);

        }

        UnloadActionPan(IMODetail.params.ParentCtrl, 'IMODetail');

        //}
        //else {
        //    UnloadActionPan(IMODetail.params.ParentCtrl, 'IMODetail');
        //}

    }

}
