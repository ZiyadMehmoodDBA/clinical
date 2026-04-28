Immunization_ImmunizationAddImmInj = {
    bIsFirstLoad: true,
    params: [],
    GriddgvId: "dgvVaccine",
    Load: function (params) {
        Immunization_ImmunizationAddImmInj.params = params;
        if (Immunization_ImmunizationAddImmInj.params.PanelID != 'pnlImmunization_ImmunizationAddImmInj') {
            Immunization_ImmunizationAddImmInj.params.PanelID = Immunization_ImmunizationAddImmInj.params.PanelID + ' #pnlImmunization_ImmunizationAddImmInj';
        } else {
            Immunization_ImmunizationAddImmInj.params.PanelID = 'pnlImmunization_ImmunizationAddImmInj';
        }
        Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
    },
    BindAutocomplete: function () {
        var ImmInj = $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #txtImmInj').val();
        var Type = $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #ddlType').val();
        utility.Keyupdelay(function () {
            Immunization_ImmunizationAddImmInj.GetImmAndTheraArray(ImmInj, Type).done(function (response) {
                $("#frmSImmInj #txtImmInj").autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #hfImmInj').val(ui.item.id);
                            $("#frmSImmInj #txtImmInj").val(ui.item.value);
                        }, 100);
                    }
                });
                $("#frmSImmInj #txtImmInj").autocomplete("search");
            });
        });
    },
    GetImmAndTheraArray: function (ImmThera, Type) {
        var AllImmThera = [];
        var IsValid = false;

        if (ImmThera != null && ImmThera.length > 2) {
            IsValid = true;
        }

        else {
            IsValid = false;
        }



        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Immunization_ImmunizationAddImmInj.GetImmAndTheraArray_DBCALL(ImmThera, Type).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    if (responseData.VaccineNameCount > 0) {
                        var VaccineNamEJSON = responseData.VaccineName_JSON;
                        $.each(VaccineNamEJSON, function (i, item) {
                            if (Type != null && Type == "0") {
                                AllImmThera.push({ id: item.ImmunizationId, value: item.ImmunizationName + " (" + item.Type + ")", Type: item.Type });
                            }
                            else {
                                AllImmThera.push({ id: item.ImmunizationId, value: item.ImmunizationName, Type: item.Type });
                            }
                        });
                    }
                }

                dfd.resolve(AllImmThera);
            });
        }
        else {
            dfd.resolve(AllImmThera);
        }

        return dfd.promise();

    },
    GetImmAndTheraArray_DBCALL: function (ImmThera, Type) {

        var objData = new Object();
        objData["ImmunizationName"] = ImmThera;
        objData["Type"] = Type;
        objData["CptBaseSearch"] = true;
        objData["commandType"] = "Get_ImmAndThera_Array";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    SearchVaccineAndTherapeutic: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Immunization_Add Imm/Inj", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic_DBCall(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Immunization_ImmunizationAddImmInj.GridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var TableControl = Immunization_ImmunizationAddImmInj.params.PanelID + " #" + Immunization_ImmunizationAddImmInj.GriddgvId;
                var PagingPanelControlID = Immunization_ImmunizationAddImmInj.params.PanelID + " #dgvVaccine_Paging";
                var ClassControlName = "Immunization_ImmunizationAddImmInj";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.VaccineCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {

                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        //    } else {
        //        dfd.resolve();
        //        utility.DisplayMessages(strMessage, 2);
        //    }
        //});
        return dfd;
    },
    GridLoad: function (response) {
        var dfd = $.Deferred();
        $("#" + Immunization_ImmunizationAddImmInj.params.PanelID + ' #' + Immunization_ImmunizationAddImmInj.GriddgvId + ' tbody').empty();
        if (response.VaccineCount > 0) {
            var VaccineLoad_JSON = response.Vaccine_JSON;
            $.each(VaccineLoad_JSON, function (i, item) {
                var $row;
                var AddParameters = item.ImmunizationId + ",'" + item.Type + "',event,this";
                //$row = $('<tr onclick="Immunization_ImmunizationAddImmInj.EditImmunization_ImmunizationAddImmInj(' + AddParameters + ')"/>');

                if (item.Status.toLowerCase() == "active" || item.Status == null) {
                    toggleClass = 'class = "fa fa-toggle-on green"'
                } else {
                    toggleClass = 'class = "fa fa-toggle-on red"'
                }

                actions = '<a href="#" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" onclick="Immunization_ImmunizationAddImmInj.ActiveOrInActiveImmInj(' + AddParameters + ');"><i ' + toggleClass + '></i></a>';


                $row = $('<tr/>');
                $row.append('<td>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_ImmunizationAddImmInj.deleteImmunization_ImmunizationAddImmInj(' + AddParameters + ');"><i class="fa fa-times red"></i></a>' +
                    '&nbsp <a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_ImmunizationAddImmInj.EditImmunization_ImmunizationAddImmInj(' + AddParameters + ');"><i class="fa fa-edit blue"></i></a>&nbsp' + actions + '</td>');
                $row.append('<td>' + (item.Type == "Immunization" ? "Imm" : "Inj") + '</td>');
                $row.append('<td>' + item.ImmunizationName + '</td>');
                if (!($('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #ddlType').val() == "2")) {
                    $row.append('<td>' + (item.CVX != null ? item.CVX : "") + '</td>');
                }
                $row.append('<td>' + (item.CPT != null ? item.CPT : "") + '</td>');
                $row.append('<td>' + (item.AdminCode != null ? item.AdminCode : "") + '</td>');
                $row.append('<td>' + (item.ModifiedOn != null ? item.ModifiedOn : "") + '</td>');
                $row.append('<td>' + item.ModifiedBy + '</td>');
                $("#" + Immunization_ImmunizationAddImmInj.params.PanelID + ' #' + Immunization_ImmunizationAddImmInj.GriddgvId + ' tbody').last().append($row);

            });
            if ($('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #ddlType').val() == "2") {
                $("#" + Immunization_ImmunizationAddImmInj.params.PanelID + ' #' + Immunization_ImmunizationAddImmInj.GriddgvId + ' th#CVX').hide();
            }
            else {
                $("#" + Immunization_ImmunizationAddImmInj.params.PanelID + ' #' + Immunization_ImmunizationAddImmInj.GriddgvId + ' th#CVX').show();
            }
            dfd.resolve();
        }
        return dfd;
    },
    ActiveOrInActiveImmInj: function (Id, Type, event, obj) {
        var Status = 1;
        if ($(obj).find('i').hasClass('green')) {
            Status = 0;
            $(obj).find('i').removeClass('green');
            $(obj).find('i').addClass('red');
        }
        else {
            Status = 1;
            $(obj).find('i').removeClass('red');
            $(obj).find('i').addClass('green');
        }
        utility.myConfirm('3', function () {
            
            Immunization_ImmunizationAddImmInj.ActiveOrInActiveImmInj_DB_CALL(Type, Id, Status).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () { },
                 '3', null, null, null, Status
            );
    },
    ActiveOrInActiveImmInj_DB_CALL: function (Type, Id, Status) {
        var objData = {};
        objData["commandType"] = "ActiveOrInactiveImmOrThera";
        objData["Status"] = Status;
        objData["Type"] = Type;
        objData["Id"] = Id;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    SearchVaccineAndTherapeutic_DBCall: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        var ImmInj = $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #txtImmInj').val();
        ImmInj = ImmInj.replace("(Immunization)", "").trim();
        ImmInj = ImmInj.replace("(Therapeutic)", "").trim();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["ImmunizationId"] = $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #hfImmInj').val();
        objData["Type"] = $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #ddlType').val();
        objData["Status"] = $('#' + Immunization_ImmunizationAddImmInj.params.PanelID + ' #ddlStatus').val();
        objData["commandType"] = "Load_Vaccine_And_Therapeutic";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    EditImmunization_ImmunizationAddImmInj: function (Id, Type, event) {

        var params = [];
        var Page = "Immunization_AddVaccine";
        params["ParentCtrl"] = "Immunization_ImmunizationAddImmInj";
        params["FromAdmin"] = 0;
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;

        if (Type == "Immunization") {
            Page = "Immunization_AddVaccine";
            params["ImmunizationId"] = Id;
        }
        else {
            Page = "Immunization_TherapeuticDetail";
            params["TherapeuticId"] = Id;
        }
        LoadActionPan(Page, params);
    },
    deleteImmunization_ImmunizationAddImmInj: function (Id, Type) {

        utility.myConfirm('1', function () {
            Immunization_ImmunizationAddImmInj.DeleteImmunizationOrTherapeutic(Type, Id).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_ImmunizationAddImmInj.SearchVaccineAndTherapeutic();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }, function () { },
                   '1'
               );
    },
    DeleteImmunizationOrTherapeutic: function (Type, Id) {
        var objData = new Object();
        objData["Type"] = Type;
        objData["commandType"] = "DELETE_Immunization_Or_Therapeutic";
        objData["Id"] = Id;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    AddVaccine: function () {
        var params = [];
        params["ParentCtrl"] = "Immunization_ImmunizationAddImmInj";
        params["FromAdmin"] = 0;
        LoadActionPan("Immunization_VaccineTypeSelection", params);
    },
    GetVaccineInformation: function (Type, Id) {

        var dfd = $.Deferred();
        Immunization_ImmunizationAddImmInj.GetVaccineInformation_DB_CALL(Type, Id).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve(response.Vaccine_JSON[0]);
            }
            else {
                dfd.resolve(null);
                utility.DisplayMessages(response.message, 3);
            }
        });

        return dfd.then(function (result) {
            return result;
        });
    },

    GetVaccineInformation_DB_CALL: function (Type, Id) {
        var objData = new Object();
        objData["Type"] = Type;
        objData["Id"] = Id;
        objData["commandType"] = "GetVaccineInformationForAutoPopu";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_ImmunizationAddImmInj.params["FromAdmin"] == "0") {
            if (Immunization_ImmunizationAddImmInj.params != null && Immunization_ImmunizationAddImmInj.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_ImmunizationAddImmInj.params.ParentCtrl, 'Immunization_ImmunizationAddImmInj');
            }
            else
                UnloadActionPan(null, 'Immunization_ImmunizationAddImmInj');
        }
        else {
            RemoveAdminTab();
        }
        return objDeffered;
    },
}