Immunization_Manufacturer = {
    bIsFirstLoad: true,
    params: [],
    GriddgvId: "dgvManufacturer",
    Load: function (params) {
        Immunization_Manufacturer.params = params;
        if (Immunization_Manufacturer.params.PanelID != 'pnlImmunization_Manufacturer') {
            Immunization_Manufacturer.params.PanelID = Immunization_Manufacturer.params.PanelID + ' #pnlImmunization_Manufacturer';
        } else {
            Immunization_Manufacturer.params.PanelID = 'pnlImmunization_Manufacturer';
        }
        Immunization_Manufacturer.SearchManufacturer();
    },
    BindAutocomplete: function () {
        var ManufacturerName = $('#' + Immunization_Manufacturer.params.PanelID + ' #txtManufacturer').val();
        utility.Keyupdelay(function () {
            Immunization_Manufacturer.GetManufacturerArray(ManufacturerName).done(function (response) {
                $("#frmManufacturer #txtManufacturer").autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#" + Immunization_Manufacturer.params.PanelID + " #hfManufacturerId").val(ui.item.id);
                            $("#" + Immunization_Manufacturer.params.PanelID + " #txtManufacturer").val(ui.item.value);
                        }, 100);
                    }
                });
                $("#frmSImmInj #txtManufacturer").autocomplete("search");
            });
        });
    },

    GetManufacturerArray: function (ManufacturerName) {
        var AllManufacturer = [];
        var IsValid = false;

        if (ManufacturerName != null && ManufacturerName.length > 2) {
            IsValid = true;
        }

        else {
            IsValid = false;
        }



        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Immunization_Manufacturer.GetManufacturerArray_DBCALL(ManufacturerName).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    if (responseData.ManufacturerCount > 0) {
                        var ManufacturerJSON = responseData.Manufacturer_JSON;
                        $.each(ManufacturerJSON, function (i, item) {
                            AllManufacturer.push({ id: item.ManufacturerId, value: item.ManufacturerName });
                        });
                    }
                }

                dfd.resolve(AllManufacturer);
            });
        }
        else {
            dfd.resolve(AllManufacturer);
        }

        return dfd.promise();

    },
    GetManufacturerArray_DBCALL: function (ManufacturerName) {
        var objData = new Object();
        objData["ManufacturerName"] = ManufacturerName;
        objData["commandType"] = "Get_Manufacturer_Array";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },
    SearchManufacturer: function (PrimaryID, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Immunization_Add Imm/Inj", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        Immunization_Manufacturer.SearchManufacturer_DBCall(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Immunization_Manufacturer.GridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var TableControl = Immunization_Manufacturer.params.PanelID + " #" + Immunization_Manufacturer.GriddgvId;
                var PagingPanelControlID = Immunization_Manufacturer.params.PanelID + " #dgvManufacturer_Paging";
                var ClassControlName = "Immunization_Manufacturer";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ManufacturerCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Immunization_Manufacturer.SearchManufacturer(PrimaryID, PageNumber, ResultPerPage);
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
        $("#" + Immunization_Manufacturer.params.PanelID + ' #' + Immunization_Manufacturer.GriddgvId + ' tbody').empty();
        if (response.ManufacturerCount > 0) {
            var Manufacturer_JSON = response.Manufacturer_JSON;
            $.each(Manufacturer_JSON, function (i, item) {
                var $row;
                var AddParameters = item.ManufacturerId + ",event,this";
                //$row = $('<tr onclick="Immunization_Manufacturer.EditImmunization_Manufacturer(' + AddParameters + ')"/>');

                if (item.Status == "Active" || item.Status == null) {
                    toggleClass = 'class = "fa fa-toggle-on green"'
                } else {
                    toggleClass = 'class = "fa fa-toggle-on red"'
                }

                actions = '<a href="#" class="btn-xs on-default remove-active mr-none btn" title="Record Active/Inactive" onclick="Immunization_Manufacturer.ActiveOrInActiveManucaturer(' + AddParameters + ');"><i ' + toggleClass + '></i></a>';


                $row = $('<tr/>');
                $row.append('<td>' + '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_Manufacturer.DeleteManufacturer(' + AddParameters + ');"><i class="fa fa-times red"></i></a>' +
                    '&nbsp <a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Immunization_Manufacturer.EditManufacturer(' + AddParameters + ');"><i class="fa fa-edit blue"></i></a>&nbsp' + actions + '</td>');
                $row.append('<td>' + (item.ManufacturerName != null ? item.ManufacturerName : "") + '</td>');
                $row.append('<td>' + (item.MVXCode != null ? item.MVXCode : "") + '</td>');
                $row.append('<td>' + (item.Status != null ? item.Status : "") + '</td>');
                $row.append('<td>' + (item.ModifiedOn != null ? item.ModifiedOn : "") + '</td>');
                $row.append('<td>' + (item.ModifiedBy != null ? item.ModifiedBy : "") + '</td>');
                $("#" + Immunization_Manufacturer.params.PanelID + ' #' + Immunization_Manufacturer.GriddgvId + ' tbody').last().append($row);
            });
            dfd.resolve();
        }
        return dfd;
    },
    SearchManufacturer_DBCall: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        var ManufacturerName = $('#' + Immunization_Manufacturer.params.PanelID + ' #frmManufacturer #txtManufacturer').val();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["ManufacturerName"] = ManufacturerName;
        objData["MVXCode"] = $('#' + Immunization_Manufacturer.params.PanelID + ' #frmManufacturer #txtMVXCode').val();
        objData["Status"] = $('#' + Immunization_Manufacturer.params.PanelID + ' #frmManufacturer #ddlStatus').val();
        objData["commandType"] = "Load_Manufacturer";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },
    AddManufacturer: function () {
        var params = [];
        params["ParentCtrl"] = "Immunization_Manufacturer";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("Immunization_ManufacturerDetail", params);
    },
    EditManufacturer: function (ManufacturerId, event, obj) {
        var params = [];
        params["ParentCtrl"] = "Immunization_Manufacturer";
        params["FromAdmin"] = 0;
        params["ManufacturerId"] = ManufacturerId;
        params["mode"] = "Edit";
        LoadActionPan("Immunization_ManufacturerDetail", params);
    },
    DeleteManufacturer: function (ManufacturerId, event, obj) {
        utility.myConfirm('1', function () {
            Immunization_Manufacturer.ManufacturerDelete(ManufacturerId).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_Manufacturer.SearchManufacturer();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }, function () { },
                           '1'
                       );
    },
    ActiveOrInActiveManucaturer: function (ManufacturerId, event, obj) {

        var IsActive = null;
        var Status = "Active";
        if ($(obj).find('i').hasClass('green')) {
            Status = "InActive";
            $(obj).find('i').removeClass('green');
            $(obj).find('i').addClass('red');
            IsActive = "0";
        }
        else {
            Status = "Active";
            $(obj).find('i').removeClass('red');
            $(obj).find('i').addClass('green');
            IsActive = "1";
        }
        utility.myConfirm('3', function () {
           
            Immunization_Manufacturer.ActiveOrInActiveManufacturer_DB_CALL(ManufacturerId, Status).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_Manufacturer.SearchManufacturer();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () { },
                 '3', null, null, null, IsActive
            );
    },
    ActiveOrInActiveManufacturer_DB_CALL: function (ManufacturerId, Status) {
        var objData = {};
        objData["commandType"] = "ActiveOrInactiveManufacturer";
        objData["Status"] = Status;
        objData["ManufacturerId"] = ManufacturerId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },
    ManufacturerDelete: function (ManufacturerId) {
        var objData = new Object();
        objData["commandType"] = "DELETE_Manufacturer";
        objData["ManufacturerId"] = ManufacturerId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_Manufacturer.params["FromAdmin"] == "0") {
            if (Immunization_Manufacturer.params != null && Immunization_Manufacturer.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_Manufacturer.params.ParentCtrl, 'Immunization_Manufacturer');
            }
            else
                UnloadActionPan(null, 'Immunization_Manufacturer');
        }
        else {
            RemoveAdminTab();
        }
        return objDeffered;
    },
}