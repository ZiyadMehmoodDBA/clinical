Batch_Fax_QuickLink = {
    bIsFirstLoad: true,
    params: [],
    inboxItems: 0,
    CheckedBoxes: [],
    access: false,
   
    Load: function (params) {
        Batch_Fax_QuickLink.CheckedBoxes = [];
        Batch_Fax_QuickLink.params = params;
        if (Batch_Fax_QuickLink.params["PanelID"] != 'pnlBatchFaxQuickLink')
            Batch_Fax_QuickLink.params["PanelID"] = Batch_Fax_QuickLink.params["PanelID"] + ' #pnlBatchFaxQuickLink';
        Batch_Fax_QuickLink.getAccess().done(function (response) {
            $('#FaxInboxTab').attr('class', 'active');
            $('#FaxOutboxTab').attr('class', '');
            Batch_Fax_QuickLink.getInboxItems();
            $("#Outbox").hide();
            Batch_Fax_QuickLink.initDatePickers();
            Batch_Fax_QuickLink.bindChangeEvents();
            Batch_Fax_QuickLink.BindProvider();
            Batch_Fax_QuickLink.BindProviderOutPut();
        });
        if (Batch_Fax_QuickLink.params.ParentCtrl == "batchTabFax") {
            $("#" + Batch_Fax_QuickLink.params["PanelID"] + " #btn-add").hide();
        }
    },
    BindProvider: function () {
        var Ctrl = $("#pnlBatchFaxQuickLink #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBatchFaxQuickLink #hfProviderId");
        var onSelect = function (e) {
            $("#pnlBatchFaxQuickLink #hfProviderId").val(e.id);

        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);


    },
    ResetProvider: function () {
        $("#pnlBatchFaxQuickLink #hfProviderId").val("");

    },
    OpenProviderDetail: function () {

        var params = [];
        params["ProviderId"] = $('#' + Batch_Fax_QuickLink.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProvider';
        params["ParentCtrl"] = 'batchTabFax';

        LoadActionPan('providerDetail', params, 'pnlBatchFaxQuickLink');
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnlBatchFaxQuickLink #txtProvider').attr("ProviderId", "-1");
            $('#pnlBatchFaxQuickLink #hfProviderId').val("-1");
            $("#pnlBatchFaxQuickLink #lnkProviderEdit").css("display", "none");
            $("#pnlBatchFaxQuickLink #lblProvider").css("display", "inline");
        } else {

            $("#pnlBatchFaxQuickLink #lnkProviderEdit").css("display", "inline");
            $("#pnlBatchFaxQuickLink #lblProvider").css("display", "none");
        }
    },
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ParentCtrl"] = "Batch_Fax_QuickLink";
        LoadActionPan('Admin_Provider', params);
    },

    BindProviderOutPut: function () {
        var Ctrl = $("#pnlBatchFaxQuickLink #txtProviderOutPut");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBatchFaxQuickLink #hfProviderIdOutPut");
        var onSelect = function (e) {
            $("#pnlBatchFaxQuickLink #hfProviderIdOutPut").val(e.id);

        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);


    },
    ResetProviderOutPut: function () {
        $("#pnlBatchFaxQuickLink #hfProviderIdOutPut").val("");

    },
    OpenProviderDetailOutPut: function () {

        var params = [];
        params["ProviderId"] = $('#' + Batch_Fax_QuickLink.params.PanelID + ' #hfProviderIdOutPut').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProviderOutPut';
        params["ParentCtrl"] = 'Batch_Fax_QuickLink';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLinkOutPut: function (value) {
        if (value == "") {
            $('#pnlBatchFaxQuickLink #txtProviderOutPut').attr("ProviderId", "-1");
            $('#pnlBatchFaxQuickLink #hfProviderIdOutPut').val("-1");
            $("#pnlBatchFaxQuickLink #lnkProviderEditOutPut").css("display", "none");
            $("#pnlBatchFaxQuickLink #lblProviderOutPut").css("display", "inline");
        } else {

            $("#pnlBatchFaxQuickLink #lnkProviderEditOutPut").css("display", "inline");
            $("#pnlBatchFaxQuickLink #lblProviderOutPut").css("display", "none");
        }
    },
    OpenProviderOutPut: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProviderOutPut";
        params["RefCtrlHidden"] = "hfProviderIdOutPut";
        params["ParentCtrl"] = "Batch_Fax_QuickLink";
        LoadActionPan('Admin_Provider', params);
    },

    bindChangeEvents: function () {
        $('#dtpFromDate').on('change', function () {
            $('#dtpToDate').attr('disabled', false);
        });
        $('#selectAll').change(function (event) {
            if (event != null) {
                event.stopPropagation();
            }
            Batch_Fax_QuickLink.selectAll(this.checked);
        });
        $('#Inbox input:checkbox').on('change', function (event) {
            if (event != null) {
                event.stopPropagation();
            }
        });
    },
    initDatePickers: function () {
        // Date pickers
        utility.CreateDatePicker('dtpFromDate', function () {
        });
        utility.CreateDatePicker('dtpToDate', function () {
        });
        utility.CreateDatePicker('dtpOutFromDate', function () {
        });
        utility.CreateDatePicker('dtpOutToDate', function () {
        });
    },
    selectAll: function (checkVal) {

        $('#Inbox input:checkbox').prop('checked', checkVal);

        $("#Inbox input:checkbox:not('#selectAll')").each(function (i, item) {
            //var $this = $(this);
            //$this.prop('checked', checkVal);
            Batch_Fax_QuickLink.addRemoveFaxId(item, item.id);
        });
    },

    addRemoveFaxId: function (item, id) {

        id = id.toString();
        var find = $.inArray(id, Batch_Fax_QuickLink.CheckedBoxes);
        if (item.checked && find == -1) {
            Batch_Fax_QuickLink.CheckedBoxes.push(id);
        } else if (!item.checked && find > -1) {
            Batch_Fax_QuickLink.CheckedBoxes.splice(find, 1);
        }
        if ($("#Inbox input:checkbox:not('#selectAll'):not(:checked)").length > 0) {
            $('#selectAll').prop('checked', false);
        } else {
            $('#selectAll').prop('checked', true);
        }
    },
    getInboxItems: function () {
        $("#Outbox").hide();
        $("#Inbox").show();
        Batch_Fax_QuickLink.getInbox(false);
        Batch_Fax_QuickLink.CheckedBoxes = [];
    },
    getInbox: function (search) {

        var obj = null;
        if (search) {
            //if ($("#dtpFromDate").val().length > 0 && $("#dtpToDate").val().length > 0) {
                obj = {};
                obj["sPeriod"] = "RANGE";
                obj["ProviderId"] = $("#pnlBatchFaxQuickLink #hfProviderId").val();
                obj["sStartDate"] = $("#dtpFromDate").val().length > 0 ? moment($("#dtpFromDate").val()).format("YYYYMMDD") : moment(Date()).format("YYYYMMDD");
                obj["sEndDate"] = $("#dtpToDate").val().length > 0 ? moment($("#dtpToDate").val()).format("YYYYMMDD") : moment(Date()).format("YYYYMMDD");
            //} else {
            //    utility.DisplayMessages("Please select both From and To dates", 4);
            //    return;
            //}
        }
        Batch_Fax_QuickLink.requestAPI(obj, "GetFaxInbox")
          .done(function (data) {
              var Batch_Fax_QuickLinkInbox = JSON.parse(data);
              if (Batch_Fax_QuickLinkInbox.Status == "Success") {
                  Batch_Fax_QuickLink.inboxGridLoad(Batch_Fax_QuickLinkInbox.Result);
                  Batch_Fax_QuickLink.QuickIconCount(Batch_Fax_QuickLinkInbox.Result);
              } else {
                  utility.DisplayMessages(Batch_Fax_QuickLinkInbox.Result, 4);
              }
          })
          .fail(function (e) {
              utility.DisplayMessages(e.responseText, 4);
          });
    },
    inboxGridLoad: function (Batch_Fax_QuickLinkInbox) {
        $('#tblInbox').dataTable().fnDestroy();
        $("#tblInbox tbody").find("tr").remove();
        var rows = '';
        Batch_Fax_QuickLink.unreadItems = 0;
        $.each(Batch_Fax_QuickLinkInbox, function (i, item) {
            var id = item.FileName.split('|')[1];
            if (item.ViewedStatus == 'N') {
                Batch_Fax_QuickLink.unreadItems = Batch_Fax_QuickLink.unreadItems + 1;

                rows += '<tr class="bold" id="' + id + '"><td><input onclick="Batch_Fax_QuickLink.addRemoveFaxId(this,\'' + id + '\');" type="checkbox"  id="' + id + '"><label for = "' + item.FileName + '"></label></td><td><a class="btn  btn-xs" href="#" onclick="Batch_Fax_QuickLink.deleteFax(\'' + item.FileName + '\',\'IN\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax_QuickLink.PreviewFax(\'' + item.FileName + '\',\'IN\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a></td><td>' + Batch_Fax_QuickLink.MaskNumber(item.CallerID, "IN") + '</td><td>' + utility.DateTemplate(item.Date) + '</td></tr>';
            }
            else {
                rows += '<tr  id="' + id + '"><td><input onclick="Batch_Fax_QuickLink.addRemoveFaxId(this,' + id + ');" type="checkbox"  id="' + id + '"><label for = "' + item.FileName + '"></label></td><td><a class="btn  btn-xs" href="#" onclick="Batch_Fax_QuickLink.deleteFax(\'' + item.FileName + '\',\'IN\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax_QuickLink.PreviewFax(\'' + item.FileName + '\',\'IN\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a></td><td>' + Batch_Fax_QuickLink.MaskNumber(item.CallerID, "IN") + '</td><td>' + utility.DateTemplate(item.Date) + '</td></tr>';
            }
        });
        $('#tblInbox tbody').append(rows);
        if (!$.fn.DataTable.isDataTable('#tblInbox')) {
            $("#tblInbox").DataTable({
                "bInfo": true, "searching": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 15, "columnDefs": [{ "bSortable": false, "targets": [0, 1, 2, 3], "orderable": false }]
            });

            //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
            utility.removePaginationFromGrid($('#' + Batch_Fax_QuickLink.params.PanelID + ' #Inbox'));
            //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        }
        // Set inbox item count
        if (Batch_Fax_QuickLink.unreadItems > 0) {
            $('#inboxTab').text('Inbox (' + Batch_Fax_QuickLink.unreadItems + ')');
        }
        else {
            $('#inboxTab').text('Inbox');
        }
        // remove duplicate footer
        if ($("#tblInbox_wrapper").find(".datatables-footer").length > 1) {
            $("#tblInbox_wrapper").find(".datatables-footer").last().remove();
            $("#tblInbox_wrapper").find(".Of-a").first().removeClass("Of-a");
        }

        if ($("#tblInbox_wrapper .dataTables_filter").length > 1) {
            $("#tblInbox_wrapper .dataTables_filter").last().remove();
        }
    }, // Provide data as JSON Object, binds values in grid
    getOutboxItems: function () {
        $("#Inbox").hide();
        $("#Outbox").show();
        Batch_Fax_QuickLink.getOutbox(false);
    },
    getOutbox: function (search, PageNumber, ResultPerPage) {

        var obj = {};
        if (search) {
            //if ($("#dtpOutFromDate").val().length > 0 && $("#dtpOutToDate").val().length > 0) {
                obj["sPeriod"] = "RANGE";
                obj["sStartDate"] = $("#dtpOutFromDate").val().length > 0 ? moment($("#dtpOutFromDate").val()).format("YYYYMMDD") : null;
                obj["sEndDate"] = $("#dtpOutToDate").val().length > 0 ? moment($("#dtpOutToDate").val()).format("YYYYMMDD") : null;
            //} else {
            //    utility.DisplayMessages("Please select both From and To dates", 4);
            //    return;
            //}
        }

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (ResultPerPage == null) {
            ResultPerPage = 15;
        }
        obj["PageNumber"] = PageNumber;
        obj["ProviderId"] = $("#pnlBatchFaxQuickLink #hfProviderIdOutPut").val() == "-1" ? null : $("#pnlBatchFaxQuickLink #hfProviderIdOutPut").val();
        obj["ResultPerPage"] = ResultPerPage;
        if (!Batch_Fax_QuickLink.params.PatientId) {
            obj["PatientId"] = 0;
        }
        else { obj["PatientId"] = Batch_Fax_QuickLink.params.PatientId; }
        Batch_Fax_QuickLink.requestAPI(obj, "GetFaxOutbox").done(function (data) {

            var Batch_Fax_QuickLinkOutbox = JSON.parse(data);
            if (Batch_Fax_QuickLinkOutbox.status) {
                Batch_Fax_QuickLink.outboxGridLoad(Batch_Fax_QuickLinkOutbox);
                var TableControl = Batch_Fax_QuickLink.params.PanelID + " #tblOutbox";

                var PagingPanelControlID = Batch_Fax_QuickLink.params.PanelID + " #dgvtblOutbox_Paging";
                var ClassControlName = "Batch_Fax_QuickLink";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = Batch_Fax_QuickLinkOutbox.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(Batch_Fax_QuickLinkOutbox.Batch_FaxOutboxCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (Id, PageNumber, ResultPerPage) {
                        Batch_Fax_QuickLink.getOutbox(search, PageNumber, ResultPerPage);
                    }), 10);
            } else {
                utility.DisplayMessages(Batch_Fax_QuickLinkOutbox.Message, 4);
            }
        })
        .fail(function (e) {
            utility.DisplayMessages(e.responseText, 4);
        });
    },
    outboxGridLoad: function (Batch_Fax_QuickLinkOutbox) {
        $('#tblOutbox').dataTable().fnDestroy();
        $("#tblOutbox tbody").find("tr").remove();
        if (Batch_Fax_QuickLinkOutbox.Batch_FaxOutboxCount > 0) {
            var rows = '';
            $.each(Batch_Fax_QuickLinkOutbox.Result, function (i, item) {
                var id = item.FaxDetailsID;
                if (item.SentStatus == 'Sent') {
                    rows += '<tr id="' + id + '"><td style="display:none;">' + id + '</td><td><a class="btn  btn-xs" onclick="Batch_Fax_QuickLink.deleteFax(\'' + item.FileName + '\',\'OUT\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax_QuickLink.PreviewFax(\'' + item.FileName + '\',\'OUT\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a></td><td>' + Batch_Fax_QuickLink.MaskNumber(item.ToFaxNumber, "OUT") + '</td><td>' + item.Subject + '</td><td>' + item.SentStatus + '</td><td>' + item.CreatedOn + '</td><td>' + item.SentToName + '</td><td>' + item.Pages + '</td><td>' + item.ProviderName + '</td><td>' + item.SenderName + '</td></tr>';
                } else if (item.SentStatus == 'Failed') {
                    rows += '<tr id="' + id + '"><td style="display:none;">' + id + '</td><td><a class="btn  btn-xs" onclick="Batch_Fax_QuickLink.deleteFax(\'' + item.FileName + '\',\'OUT\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax_QuickLink.PreviewFax(\'' + item.FileName + '\',\'OUT\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax_QuickLink.FaxStatus(' + id + ',event);" title="Fax Status"> <i class="fa fa-info-circle blue"></i></a>&nbsp;<a class="btn  btn-xs" onclick="Batch_Fax_QuickLink.ForwardFax(' + id + ',event);" title="Resend Fax"> <i class="glyphicon glyphicon-refresh blue"></i></a></td><td>' + Batch_Fax_QuickLink.MaskNumber(item.ToFaxNumber, "OUT") + '</td><td>' + item.Subject + '</td><td>' + item.SentStatus + '</td><td>' + item.CreatedOn + '</td><td>' + item.SentToName + '</td><td>' + item.Pages + '</td><td>' + item.ProviderName + '</td><td>' + item.SenderName + '</td></tr>';
                } else {
                    rows += '<tr id="' + id + '"><td style="display:none;">' + id + '</td><td></td><td>' + Batch_Fax_QuickLink.MaskNumber(item.ToFaxNumber, "OUT") + '</td><td>' + item.Subject + '</td><td>' + item.SentStatus + '</td><td>' + item.CreatedOn + '</td><td>' + item.SentToName + '</td><td>' + item.Pages + '</td><td>' + item.ProviderName + '</td><td>' + item.SenderName + '</td></tr>';
                }

            });
            $('#tblOutbox tbody').append(rows);
        }
        else {

            $('#tblOutbox').DataTable({
                "language": {
                    "emptyTable": "No Fax Found"
                }, "autoWidth": false, "searching": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], "orderable": false }]
            });
        }
        if (!$.fn.dataTable.isDataTable('#tblOutbox'))
            $("#tblOutbox").DataTable({
                "bInfo": false, "searching": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], "orderable": false }]
            }); // to remove records per page dropdown
    },  // Binds outbox grid
    FaxStatus: function (id, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var obj = {};

        obj["sFaxDetailsID"] = id;

        var inputParam = JSON.stringify(obj);

        Batch_Fax_QuickLink.requestAPI(inputParam, "GetFaxStatus")
         .done(function (data) {
             var Fax = JSON.parse(data);
             if (Fax.Status == "Success") {
                 utility.DisplayMessages(Fax.Result.ErrorCode, 4);
             } else {
                 utility.DisplayMessages(Fax.Result, 4);
             }
         });
    },
    ForwardFax: function (id, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var obj = {};

        obj["sFaxDetailsID"] = id;

        var inputParam = JSON.stringify(obj);

        Batch_Fax_QuickLink.requestAPI(inputParam, "ForwardFax")
         .done(function (data) {
             var Fax = JSON.parse(data);
             if (Fax.Status == "Success") {
                 utility.DisplayMessages("Fax has been queued successfully.", 1);
                 Batch_Fax_QuickLink.getOutbox(false);
             } else {
                 utility.DisplayMessages(Fax.Result, 4);
             }
         });
    },
    PreviewFax: function (FileNameId, sDirection, e, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var splitArray = FileNameId.split('|');
        var fileName = splitArray[0];
        var id = splitArray[1];

        var obj = {};
        obj["sFaxFileName"] = FileNameId;
        obj["sFaxDetailsID"] = id;
        obj["sDirection"] = sDirection;

        var inputParam = JSON.stringify(obj);

        Batch_Fax_QuickLink.requestAPI(inputParam, "RetrieveFax")
         .done(function (data) {
             var Fax = JSON.parse(data);

             if (Fax.Status == "Success") {
                 var params = [];
                 Batch_Fax_QuickLinkSend.AnnotateArray["base64"] = "data:application/pdf;base64," + Fax.Result;
                 params["ParentCtrl"] = 'Batch_Fax_QuickLink';
                 params["FaxId"] = FileNameId;
                 LoadActionPan('Batch_Fax_QuickLinkSendAnnotate', params);

                 var row = $(e.parentNode.parentNode);
                 if (row.hasClass("bold")) {
                     var count = parseInt($("#inboxTab").text().split(/[()]+/)[1]) - 1;
                     if (count > 0) {
                         $("#inboxTab").text("Inbox (" + count + ")");
                         $("#spnFaxesCount").text(count);
                     }
                     else {
                         $("#inboxTab").text("Inbox");
                         $("#spnFaxesCount").text("");
                         $('#spnFaxesCount').hide();
                     }
                     row.removeClass("bold");
                 }
             } else {
                 utility.DisplayMessages(Fax.Result, 4);
             }
         });
    },
    loadSendFax: function () {
        var params = [];
        params["access_token"] = Batch_Fax_QuickLink.access_token;
        params["ParentCtrl"] = 'Batch_Fax_QuickLink';
        Batch_FaxSend.loadUserProfiles().done(function (resp) {
            if (resp) {
                LoadActionPan('Batch_FaxSend', params);
            }
            else {
                utility.DisplayMessages("Please add fax settings first.", 4);
            }
        });

    },
    deleteFax: function (FileNameId, sDirection, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm("1", function () {
            var splitArray = FileNameId.split('|');
            var id = splitArray[1];

            Batch_Fax_QuickLink.faxDelete(id, sDirection);
            Batch_Fax_QuickLink.getInbox(false);
        }, function () { });
    },
    faxDelete: function (id, sDirection) {
        var obj = {};
        //obj["sFaxFileName"] = FileNameId;
        obj["sFaxDetailsID_1"] = id;
        obj["sDirection"] = sDirection;

        var inputParam = JSON.stringify(obj);

        Batch_Fax_QuickLink.requestAPI(inputParam, "DeleteFax")
         .done(function (data) {
             var Fax = JSON.parse(data);
             if (Fax.Status == "Success") {
                 if (sDirection == "IN")
                     Batch_Fax_QuickLink.RemoveTableRow(sDirection, id);
                 else
                     Batch_Fax_QuickLink.getOutbox(false);
                 utility.DisplayMessages("Fax deleted", 4);
             } else {
                 utility.DisplayMessages(Fax.Result, 4);
             }
         });
    },
    RemoveTableRow: function (sDirection, id) {
        var table = "";
        if (sDirection == "IN") {
            table = "tblInbox";
        } else {
            table = "tblOutbox";
        }
        $("#" + table + " tr#" + id).remove();
    },
    destroyGrid: function () {
        $('#tblInbox').dataTable().fnDestroy();
        $("#tblInbox tbody").find("tr").remove();
        $('#tblOutbox').dataTable().fnDestroy();
        $("#tblOutbox tbody").find("tr").remove();
    },
    getAccess: function () {
        var Fax = "";
        return MDVisionService.defaultService(Fax, "Batch_Fax_CLINICAL", "FAX_ACCESS");
    },
    UnLoad: function () {
        if (Batch_Fax_QuickLink.params.ParentCtrl && Batch_Fax_QuickLink.params.ParentCtrl != "batchTabFax") {
            Batch_Fax_QuickLink.destroyGrid();
        }
        if (Batch_Fax_QuickLink.params != null && Batch_Fax_QuickLink.params.ParentCtrl != null) {
            if (Batch_Fax_QuickLink.params.ParentCtrl == "batchTabFax") {
                Batch_Fax_QuickLink.params = [];
                Batch_Fax_QuickLink.params = params;
            }
            UnloadActionPan(Batch_Fax_QuickLink.params.ParentCtrl, 'Batch_Fax_QuickLink');
        }
        else
            UnloadActionPan(null, 'Batch_Fax_QuickLink');

    },
    faxAccess: function () {
        var data = "FaxData=";
        return MDVisionService.defaultService(data, "Batch_Fax_CLINICAL", "LOAD_FAX_ACCESS");
    },
    ShowFaxCount: function () {
        if (Batch_Fax_QuickLink.access == true) {
            Batch_Fax_QuickLink.requestAPI(null, "GetFaxInbox")
              .done(function (data) {
                  var Batch_Fax_QuickLinkInbox = JSON.parse(data);
                  if (Batch_Fax_QuickLinkInbox.Status == "Success") {
                      Batch_Fax_QuickLink.QuickIconCount(Batch_Fax_QuickLinkInbox.Result);
                  }
              })
              .fail(function (e) {
                  if (e.responseText.indexOf("The underlying connection was closed: An unexpected error occurred on a receive.") < 0)
                  utility.DisplayMessages(e.responseText, 4);
              });

        }
        else if (Batch_Fax_QuickLink.access == false) {
            $('#btnFaxes').hide();
        }

    },
    QuickIconCount: function (result) {
        var unreadItems = 0;
        $.each(result, function (i, item) {
            if (item.ViewedStatus == 'N') {
                unreadItems = unreadItems + 1;
            }
        });
        if (unreadItems > 0) {
            $('#spnFaxesCount').text(unreadItems);
            $('#spnFaxesCount').show();
        } else {
            $('#spnFaxesCount').hide();
        }
    },
    deleteAllFaxes: function () {
        utility.myConfirm("1", function () {
            var defConfirm = $.Deferred();
            for (check = 0 ; check < Batch_Fax_QuickLink.CheckedBoxes.length ; check++) {
                Batch_Fax_QuickLink.faxDelete(Batch_Fax_QuickLink.CheckedBoxes[check], "IN");

                if (check == Batch_Fax_QuickLink.CheckedBoxes.length - 1) {
                    defConfirm.resolve('ok');
                }
            }
            $.when(defConfirm).done(function () {
                Batch_Fax_QuickLink.CheckedBoxes = [];

                Batch_Fax_QuickLink.getInbox(false);
            });
        }, function () { });
    },

    requestAPI: function (data, method) {
        return MDVisionService.APIService(data, "Fax", method);
    },
    MaskNumber: function (num, type) {
        if (type == "OUT") {
            return "(" + num.substring(1, 4) + ") " + num.substring(4, 7) + "-" + num.substring(7, 11);
        } else {
            return "(" + num.substring(0, 3) + ") " + num.substring(3, 6) + "-" + num.substring(6, 10);
        }
    }
}