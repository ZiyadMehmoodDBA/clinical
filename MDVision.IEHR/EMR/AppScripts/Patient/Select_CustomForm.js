Select_CustomForm = {
    Load: function (params) {
        Select_CustomForm.params = params;

        if (Select_CustomForm.params.PanelID != 'pnlSelectCustomForm') {
            Select_CustomForm.params.PanelID = Select_CustomForm.params.PanelID + ' #pnlSelectCustomForm';
        } else {
            Select_CustomForm.params.PanelID = 'pnlSelectCustomForm';
        }
        EMRUtility.setFavoriteSectionStyle(Select_CustomForm.params.PanelID);
        Select_CustomForm.favoriteListSearch();
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Select_CustomForm.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + Select_CustomForm.params.PanelID);
        self.loadDropDowns(true).done(function () {

       
            Select_CustomForm.loadfavoriteListContent($("#" + Select_CustomForm.params.PanelID + " #ddlFavoriteListCustomForms"));
            Select_CustomForm.ValidateSelect_CustomForm();
        });

    },
    searchFavoriteList_CF_DBCall: function (FavoriteListId, PageNumber, PageNumber) {

        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["IsActive"] = "1";
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;

        objData["commandType"] = "load_favoritelist_customforms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    loadfavoriteListContent: function (obj) {
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            if (selectedOption.attr("id") != "-1") {
                Select_CustomForm.searchFavoriteList_CF_DBCall(selectedOption.attr("id")).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var $UL = $('#' + Select_CustomForm.params.PanelID + ' #ulFavoriteListCustomFormsContent');
                        $UL.empty();
                        if (response.customFormCount > 0) {
                            var listCustomForm_JSON = response.listCustomForm;
                            $.each(listCustomForm_JSON, function (i, item) {
                                if (item.ProviderNames != "") {
                                    var onclick = 'Select_CustomForm.BindCustomFrmsUl(\'' + item.CustomFormId + '\')';

                                    var LiId = item.CustomFormId;
                                    $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.FormName + '</li>');
                                }
                            });
                        }
                    }
                });
               // Select_CustomForm.favoriteList_CPTSearch(selectedOption.attr("id"));
            }
            else {
                $('#' + Select_CustomForm.params.PanelID + ' #ulFavoriteListCustomFormsContent').empty();
               // $('#' + Select_CustomForm.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            }
        }
    },
    favoriteList_CPTSearch: function (FavoriteListId) {
        Favorite_CustomForms.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var $UL = $('#' + Select_CustomForm.params.PanelID + ' #ulFavoriteListCustomFormsContent');
                $UL.empty();

                if (response.FavoriteListCustomFormCount > 0) {
                    // $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                    var FavoriteListJSON = JSON.parse(response.FavoriteListCustomFormJSON);

                    //if (FavoriteListJSON.length > 0) {
                    //    $('#' + Select_CustomForm.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                    //}
                    //else {
                    //    $('#' + Select_CustomForm.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
                    //}

                    var li = "";
                    $.each(FavoriteListJSON, function (i, item) {
                       
                      

                        var onclick = 'Select_CustomForm.BindCustomFrmsUl(\'' + item.CustomFormId + '\')';

                        var LiId = item.CustomFormId;

                        //var isFound = Select_CustomForm.isFavoriteHistoryFound(LiId);
                        //if (isFound == true) {
                        //    $UL.append('<li class="disableAll" onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        //}
                        //else {
                            $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.CustomFormName + '</li>');
                       // }

                        //$UL.append('<li onclick="' + onclick + '">' + item.ICD9CodeDescription + ' - ' + item.SNOMEDID + '</li>');


                    });

                }
            }
        });
    },
    BindCustomFrmsUl: function(customFormId){

        $('#frmSelectCustomForm #ddlCustomForm').val(customFormId);

    },
    isFavoriteHistoryFound: function (favICDCode, favCPTDesc) {

        var isFound = false;
        $("#" + Select_CustomForm.params.PanelID + " #ulMedicalDisease li").each(function (index, item) {
            if ($(item).attr('icd9desc') != null) {
                var currentRowCPTCode = $(item).text() != null ? $(item).attr('icd9desc') + '-' + $(item).attr('snomedcode') : "";
                if (currentRowCPTCode == favICDCode) {
                    isFound = true;
                }
            }
        });

        return isFound;
    },
    searchFavoriteList_ICD_DBCall: function (FavoriteListICDId, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["IsActive"] = true;
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? 0 : FavoriteListICDId;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = 0;
        }
        else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist_customforms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    ValidateSelect_CustomForm: function () {
        $('#frmSelectCustomForm')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  TemplateLetterId: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Please select a Custom Form.'
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Select_CustomForm.CreateCustomForm();
       });
    },
    UnLoadTab: function () {
        UnloadActionPan(Select_CustomForm.params.ParentCtrl, 'Select_CustomForm');
    },

    CreateCustomForm: function () {
        var dropDown = $('#frmSelectCustomForm #ddlCustomForm :selected');
        customFormName = dropDown.text();
        formId = dropDown.val();
        Select_CustomForm.UnLoadTab();
        setTimeout(function () {
            var PanelID = 'pnlPatientCustomForm';
            var params = [];
            params["ParentCtrl"] = 'Patient_CustomForm';
            params["IsFromAdminOrNot"] = "0";
            params["IsAddToNote"] = false;
            params["CustomFormId"] = formId;
            params["CustomFormName"] = customFormName;
            params["PanelID"] = 'pnlPatientCustomForm';
            LoadActionPan("Clinical_CustomFormsPreview", params, PanelID);
        }, 510);

        //Clinical_CustomForms.customFormPreview(formId, customFormName, null, false);

    },


    favoriteListSearch: function () {

        Select_CustomForm.searchFavoriteList_DBCall("CustomForms", null, 1, 5000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Select_CustomForm.params.PanelID + ' #ddlFavoriteListCustomForms');
                var favouriteCustomForms = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                $ddl.append($('<option/>', {
                    id: -1,
                    value: -1,
                    html: "- Select -"
                }));
                $.each(favouriteCustomForms, function (i, item) {
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
                if (favouriteCustomForms.length > 0) {
                    //EMRUtility.getFavListValue(Select_CustomForm.FavListName).done(function (response1) {
                    //    response1 = JSON.parse(response1);
                    //    if (response1.status != false) {
                    //        if (response1.favListVal != "" && response1.favListVal != "-1") {
                    //            if ($("#" + Select_CustomForm.params.PanelID + " #ddlFavoriteListCustomForms option[value='" + response1.favListVal + "']").length > 0) {
                    //                $ddl.val(response1.favListVal);
                    //                $ddl.trigger("onchange");
                    //            }
                    //            else {
                    //                if (favouriteCustomForms.length == 1) {
                    //                    $ddl.val(favouriteCustomForms[0].FavoriteListId);
                    //                    $ddl.trigger("onchange");
                    //                }
                    //                else if (favouriteCustomForms.length > 1) {
                    //                    $ddl.trigger("onchange");
                    //                }
                    //            }
                    //        }
                    //        else {
                    //            if (favouriteCustomForms.length == 1) {
                    //                $ddl.val(favouriteCustomForms[0].FavoriteListId);
                    //                $ddl.trigger("onchange");
                    //            }
                    //            else if (favouriteCustomForms.length > 1) {
                    //                $ddl.trigger("onchange");
                    //            }
                    //        }
                    //    }
                    //    else {
                    //        utility.DisplayMessages(response.Message, 3);
                    //    }
                    //});

                }
                //   $ddl.trigger("onchange");

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },
    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'CustomForms' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["IsActive"] = true
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
}