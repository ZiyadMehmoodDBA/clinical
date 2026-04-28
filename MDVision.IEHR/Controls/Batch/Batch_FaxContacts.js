Batch_FaxContacts = {
    bIsFirstLoad: true,
    params: [],
    client_id: "",
    client_secret: "",
    user_id: -1,
    ProviderCount: 0,
    FacilityCount: 0,
    // Programmer: Ahsan Nasir

    Load: function (params) {
        Batch_FaxContacts.params = params;

        $('#selectAllContactsCC').change(function () {
            if (this.checked) {
                $("#pnlBatch_FaxContacts [name^='CC']").prop("checked", true);
            }
            else {
                $("#pnlBatch_FaxContacts [name^='CC']").prop("checked", false);
            }
        });
        $('#selectAllContactsTo').change(function () {
            if (this.checked) {
                $("#pnlBatch_FaxContacts [name^='No']").prop("checked", true);
            }
            else {
                $("#pnlBatch_FaxContacts [name^='No']").prop("checked", false);
            }
        });
        Batch_FaxContacts.ValidateFaxSend();
        Batch_FaxContacts.loadContacts();
        Batch_FaxContacts.BindRefProviderReferral();
    },
    ValidateFaxSend: function () {
        $('#' + Batch_FaxContacts.params["PanelID"] + '  #formpanelheading')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    FaxNumber: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ContactName: {
                        group: '.col-sm-3',
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
                Batch_FaxContacts.RecipientAdd();
            });
    },
    loadContacts: function () {
        var grid = $('#tblFaxContacts tbody');

        var rows = "";
        Batch_FaxContacts.getContacts(Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(
               function (resp) {
                   Batch_FaxContacts.fillGrid(resp);
               }
            );

    },
    AddSelectedContacts: function () {

        var recipients = $('#faxRecipients tbody');
        $("#tblFaxContacts tbody tr").each(function () {
            var chkbxCC = $(this).find("[name^='CC']");
            if (chkbxCC.length > 0) {
                var chkbxTo = $(this).find("[name^='No']");
                var Contact_Id = (chkbxTo).attr("id");
                chkbxCC = chkbxCC[0].checked;
                chkbxTo = chkbxTo[0].checked;
                if (chkbxCC || chkbxTo) {
                    var ProviderName = $(this).find("#Name").text().trim();
                    var faxNo = $(this).find("#FaxNo").text().trim();
                    var cleanFaxNo = faxNo.replace(/\D/g, "");
                    var found = false;
                    $.each(Batch_FaxSend.arrayReceipients, function (i) {
                        if (Batch_FaxSend.arrayReceipients[i].ContactId == Contact_Id) {
                            Batch_FaxSend.arrayReceipients.splice(i, 1);
                            found = true;
                            return false;
                        }
                    });
                    if (chkbxCC) {
                        var faxRecObject = { ContactId: Contact_Id, Name: ProviderName, FaxNumber: cleanFaxNo, CC: chkbxCC, To: chkbxTo };
                        Batch_FaxSend.arrayReceipients.push(faxRecObject);
                    }
                    else if (chkbxTo) {
                        var faxRecObject = { ContactId: Contact_Id, Name: ProviderName, FaxNumber: cleanFaxNo, CC: chkbxCC, To: chkbxTo };
                        Batch_FaxSend.arrayReceipients.push(faxRecObject);
                    }

                    if (!found) {
                        recipients.append('<tr><td><i class="fa fa-user"></i><span id="Name">' + ProviderName + '</span> </td><td> <i class="fa fa-print"></i>  <span id="FaxNo">' + faxNo + '</span> </td><td><a id="Contact' + Contact_Id + '" class="btn  btn-xs" href="#" onclick="Batch_FaxSend.removeResp(this);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                    }
                }
            }
        });
        var textCC = '';
        $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #toCarbonCopy").val("");
        $.each(Batch_FaxSend.arrayReceipients, function (i) {
            if (Batch_FaxSend.arrayReceipients[i].CC == true) {
                textCC += Batch_FaxSend.arrayReceipients[i].Name + ";";
                $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #toCarbonCopy").val(textCC);
            }
        });
        Batch_FaxContacts.UnLoadTab();
    },
    RecipientAdd: function (contactName, FaxNo) {
        if ($("#" + Batch_FaxContacts.params.PanelID + " #formpanelheading #txtContactName").val() != "" && $("#" + Batch_FaxContacts.params.PanelID + " #formpanelheading #txtFaxNumber").val() != "") {
            var contactName = $("#" + Batch_FaxContacts.params.PanelID + " #formpanelheading #txtContactName").val();
            var FaxNo = $("#" + Batch_FaxContacts.params.PanelID + " #formpanelheading #txtFaxNumber").val();
            // Batch_FaxContacts.params.Id //ProviderId or FacilityId //Batch_FaxSend.Type Facility or Provider
            Batch_FaxContacts.getContacts(Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function (resp) {
                if (resp.status != false) {
                    var contacts = [];
                    var found = false;
                    if (Batch_FaxContacts.params.Type == "Provider")
                        contacts = JSON.parse(resp.FaxProviderContacts);
                    else if (Batch_FaxContacts.params.Type == "Facility")
                        contacts = JSON.parse(resp.FaxFacilityContacts);

                    var cleanFaxNo = FaxNo.replace(/\D/g, "");
                    var arr = jQuery.grep(contacts, function (a) {
                        return a.FaxNumber && a.FaxNumber.replace(/\D/g, "") == cleanFaxNo;
                    });
                    if (arr.length == 0) {
                        Batch_FaxContacts.SaveReceipient(contactName, FaxNo, Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function () {
                            Batch_FaxContacts.loadContacts();
                        });
                    }
                    else {
                        utility.DisplayMessages("Selected Provider already exist in the list.", 4);
                        $('#' + Batch_FaxContacts.params.PanelID + ' #formpanelheading').resetAllControls(null);
                    }
                }
            });
        }
    },

    SaveReceipient: function (contactName, FaxNo, Id, type) {
        var objDeffered = $.Deferred();
        var recipients = $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #faxRecipients tbody");
        Batch_FaxSend.saveContact(contactName, FaxNo, Id, type).done(function (response) {
            if (response.status != false) {
                $('#' + Batch_FaxContacts.params.PanelID + ' #formpanelheading').resetAllControls(null);
                $("#" + Batch_FaxContacts.params.PanelID + " #formpanelheading #txtFaxNumber").val("");
                recipients.append('<tr><td><i class="fa fa-user"></i><span id="Name">' + contactName + '</span></td><td><i class="fa fa-print"></i> <span id="FaxNo">' + FaxNo + '</span> </td><td><a id="Contact' + response.ContactId + '" class="btn  btn-xs" href="#" onclick="Batch_FaxSend.removeResp(this);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                var cleanFaxNo = FaxNo.replace(/\D/g, "");
                if (Batch_FaxContacts.params.CC) {
                    var faxRecObject = { ContactId: response.ContactId, Name: contactName, FaxNumber: cleanFaxNo, CC: Batch_FaxContacts.params.CC, To: false };
                    Batch_FaxSend.arrayReceipients.push(faxRecObject);
                    var textCC = '';
                    $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #toCarbonCopy").val("");
                    //$.each(Batch_FaxSend.arrayReceipients, function (i) {
                    //    if (Batch_FaxSend.arrayReceipients[i].CC == true) {
                    //        textCC += Batch_FaxSend.arrayReceipients[i].Name + ";";
                    //        $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #toCarbonCopy").val(textCC);
                    //    }
                    //});
                }
                else {
                    if (!Batch_FaxContacts.params.CC)
                        Batch_FaxContacts.params.CC = false;
                    var faxRecObject = {
                        ContactId: response.ContactId, Name: contactName, FaxNumber: cleanFaxNo, CC: Batch_FaxContacts.params.CC, To: true
                    };
                    Batch_FaxSend.arrayReceipients.push(faxRecObject);
                }
                Batch_FaxContacts.loadContacts();
                objDeffered.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                objDeffered.resolve();
            }
        });
        return objDeffered;
    },
    deleteContact: function (ContactId) {
        if (Batch_FaxContacts.params.Type == "Provider") {
            var obj = new Object();
            obj["Type"] = "Provider";
            obj["ProviderId"] = Batch_FaxContacts.params.Id;
            obj["ContactId"] = ContactId;
            obj = JSON.stringify(obj);
            var data = "ContactData=" + obj;
            MDVisionService.defaultService(data, "BATCH_FAX_CLINICAL", "DELETE_CONTACT").done(function () {
                Batch_FaxContacts.loadContacts();
            });
        }
        else {

            var obj = new Object();
            obj["Type"] = "Facility";
            obj["FacilityId"] = Batch_FaxContacts.params.Id;
            obj["ContactId"] = ContactId;
            obj = JSON.stringify(obj);
            var data = "ContactData=" + obj;
            MDVisionService.defaultService(data, "BATCH_FAX_CLINICAL", "DELETE_CONTACT").done(function () {
                Batch_FaxContacts.loadContacts();
            });
        }
    },
    fillGrid: function (resp) {
        $("#tblFaxContacts").dataTable().fnDestroy();
        $("#pnlBatch_FaxContacts #tblFaxContacts tbody").find("tr").remove();
        if (resp.status != false) {
            var grid = $('#tblFaxContacts tbody');
            var rows = '';
            if (Batch_FaxContacts.params.Type == "Provider") {
                //if (resp.status == "False" || resp.status == false) {
                //    grid.find("tr").remove();
                //    rows += '<tr><td colspan=4>No Contacts Found</td></tr>';
                //    grid.append(rows);
                //}
                //else {
                var contacts = JSON.parse(resp.FaxProviderContacts);
                var count = 0;
                grid.find("tr").remove();
                $.each(contacts, function (i, item) {
                    rows += '  <tr><td><input type="checkbox" name="CC' + count + '" name="contacts" id="' + item.ContactId + '"><input style="margin-left: 19%;" type="checkbox" name="No' + count + '" name="contacts" id="' + item.ContactId + '">  &nbsp;<a class="btn  btn-xs" href="#" onclick="Batch_FaxContacts.deleteContact(' + item.ContactId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td><td> <span id="Name"> ' + item.ContactName + ' </span></td><td> <span id="FaxNo">' + item.FaxNumber + '</span></td></tr>';
                    count++;
                });
                grid.append(rows);
                //}
            }
            else {
                //if (resp.status == "False" || resp.status == false) {
                //    grid.find("tr").remove();
                //    rows += '<tr><td colspan=4>No Contacts Found</td></tr>';
                //    grid.append(rows);

                //}
                //else {

                var contacts = JSON.parse(resp.FaxFacilityContacts);
                var count = 0;
                grid.find("tr").remove();
                $.each(contacts, function (i, item) {
                    rows += '  <tr><td><input type="checkbox" name="CC' + count + '" name="contacts" id="' + item.ContactId + '"><input  style="margin-left: 19%;" type="checkbox" name="No' + count + '" name="contacts" id="' + item.ContactId + '">  &nbsp;<a class="btn  btn-xs" href="#" onclick="Batch_FaxContacts.deleteContact(' + item.ContactId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td><td> <span id="Name"> ' + item.ContactName + ' </span></td><td> <span id="FaxNo">' + item.FaxNumber + '</span></td></tr>';
                    count++;
                });
                grid.append(rows);

                //}
            }
        }
        else {
            $('#tblFaxContacts').DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#tblFaxContacts'))
            ;
        else
            $("#pnlBatch_FaxContacts #tblFaxContacts").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] });
    },
    OpenProviderReferral: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "formpanelheading";
        params["ProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_FaxContacts";
        params["RefCtrl"] = "txtProviderReferral";
        params["RefCtrlHidden"] = "hfProviderReferral";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    BindRefProviderReferral: function () {
        var Ctrl = $('#pnlBatch_FaxContacts #txtProviderReferral');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBatch_FaxContacts #hfProviderReferral");
        var onSelect = function (e) {
            if (e.id && e.id != '') {
                Batch_FaxContacts.getContacts(Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function (resp) {
                    if (resp.status != false) {
                        var contacts = [];
                        var found = false;
                        if (Batch_FaxContacts.params.Type == "Provider")
                            contacts = JSON.parse(resp.FaxProviderContacts);
                        else if (Batch_FaxContacts.params.Type == "Facility")
                            contacts = JSON.parse(resp.FaxFacilityContacts);
                        Batch_FaxContacts.FillReferringProvider_DbCall(e.id).done(function (response) {
                            if (response.status != false) {
                                var ReferringProvider_detail = JSON.parse(response.ReferringProviderFill_JSON);
                                if (ReferringProvider_detail.txtFax != "") {
                                    var cleanFaxNo = ReferringProvider_detail.txtFax.replace(/\D/g, "");
                                    var arr = jQuery.grep(contacts, function (a) {
                                        return a.FaxNumber && a.FaxNumber.replace(/\D/g, "") == cleanFaxNo;
                                    });
                                    if (arr.length == 0) {
                                        Batch_FaxContacts.SaveReceipient(ReferringProvider_detail.txtFirstName + ' ' + ReferringProvider_detail.txtLastName, ReferringProvider_detail.txtFax, Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function () {
                                            Batch_FaxContacts.loadContacts();
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages("Selected Provider already exist in the list.", 4);
                                        $('#' + Batch_FaxContacts.params.PanelID + ' #formpanelheading').resetAllControls(null);
                                    }
                                }
                                else {
                                    utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                                }
                            }
                            else
                                utility.DisplayMessages(response.Message, 3);
                        });
                    }
                    else {
                        Batch_FaxContacts.FillReferringProvider_DbCall(e.id).done(function (response) {
                            if (response.status != false) {
                                var ReferringProvider_detail = JSON.parse(response.ReferringProviderFill_JSON);
                                if (ReferringProvider_detail.txtFax != "") {
                                    Batch_FaxContacts.SaveReceipient(ReferringProvider_detail.txtFirstName + ' ' + ReferringProvider_detail.txtLastName, ReferringProvider_detail.txtFax, Batch_FaxContacts.params.Id, Batch_FaxContacts.params.Type).done(function () {
                                        $('#' + Batch_FaxContacts.params.PanelID + ' #formpanelheading').resetAllControls(null);
                                        Batch_FaxContacts.loadContacts();
                                    });
                                }
                                else {
                                    $('#' + Batch_FaxContacts.params.PanelID + ' #formpanelheading').resetAllControls(null);
                                    utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                                }
                            }
                            else
                                utility.DisplayMessages(response.Message, 3);
                        });
                    }
                });
            }
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    //Id is Facility ID or Provider ID , Type is Provider or Facility
    AddtoRefProvider: function (RefProviderId, Id, Type) {
        Batch_FaxContacts.FillReferringProvider_DbCall(RefProviderId).done(function (response) {
            if (response.status != false) {
                var ReferringProvider_detail = JSON.parse(response.ReferringProviderFill_JSON);
                if (ReferringProvider_detail.txtFax != "") {
                    var cleanFaxNo = ReferringProvider_detail.txtFax.replace(/\D/g, "");
                    Batch_FaxContacts.SaveReceipient(ReferringProvider_detail.txtFirstName + ' ' + ReferringProvider_detail.txtLastName, cleanFaxNo, Id, Type).done(function () {
                        Batch_FaxContacts.loadContacts();
                    });
                }
                else {
                    utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                }
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    FillReferringProvider_DbCall: function (ReferringProviderID) {
        var data = "ReferringProviderID=" + ReferringProviderID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "FILL_REFERRING_PROVIDER");
    },
    searchContacts: function () {
        var input = $('#txtContact').val();
        if (input != "") {
            var obj = new Object();
            obj["ContactName"] = input;

            if (Batch_FaxContacts.params.Type == "Provider") {
                obj["ProviderId"] = Batch_FaxContacts.params.Id;
            }
            else {
                obj["FacilityId"] = Batch_FaxContacts.params.Id;
            }
            obj["Type"] = Batch_FaxContacts.params.Type;

            Batch_FaxContacts.contactsSearch(obj).done(
                function (resp) {

                    Batch_FaxContacts.fillGrid(resp);
                });
        }
        else {
            utility.DisplayMessages("Please type something to search", 2);
        }
    },
    contactsSearch: function (data) {
        var obj = JSON.stringify(data);
        var mydata = "SearchFaxData=" + obj;
        return MDVisionService.defaultService(mydata, "BATCH_FAX_CLINICAL", "SEARCH_CONTACTS");

    },
    getContacts: function (Id, type) {
        var typeId = '';

        if (type == "Provider") {
            typeId = "ProviderId=" + Id;
            return MDVisionService.defaultService(typeId, "BATCH_FAX_CLINICAL", "LOAD_PROVIDER_CONTACTS");
        }
        else {
            typeId = "FacilityId=" + Id;
            return MDVisionService.defaultService(typeId, "BATCH_FAX_CLINICAL", "LOAD_FACILITY_CONTACTS");
        }
    },

    UnLoadTab: function () {

        if (Batch_FaxContacts.params != null && Batch_FaxContacts.params.ParentCtrl) {
            UnloadActionPan(Batch_FaxContacts.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    }

}