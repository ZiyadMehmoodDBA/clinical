Batch_Fax = {
    bIsFirstLoad: true,
    params: [],
    inboxItems: 0,
    CheckedBoxes: [],
    access: false,
    // Programmer: Ahsan Nasir
    Load: function (params) {
        Batch_Fax.CheckedBoxes = [];
        Batch_Fax.params = params;
        if (Batch_Fax.params["PanelID"] != 'pnlBatchFax')
            Batch_Fax.params["PanelID"] = Batch_Fax.params["PanelID"] + ' #pnlBatchFax';
        Batch_Fax.getAccess().done(function (response) {
            $('#FaxInboxTab').attr('class', 'active');
            $('#FaxOutboxTab').attr('class', '');
            Batch_Fax.getInboxItems();
            $("#Outbox").hide();
            Batch_Fax.initDatePickers();
            Batch_Fax.bindChangeEvents();
            Batch_Fax.BindProvider();
            Batch_Fax.BindProviderOutPut();
        });
        if (Batch_Fax.params.ParentCtrl == "batchTabFax") {
            $("#" + Batch_Fax.params["PanelID"] + " #btn-add").hide();
        }
    },
    BindProvider: function () {
        var Ctrl = $("#pnlBatchFax #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBatchFax #hfProviderId");
        var onSelect = function (e) {
            $("#pnlBatchFax #hfProviderId").val(e.id);
           
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);


    },
    ResetProvider: function () {
        $("#pnlBatchFax #hfProviderId").val("");
       
    },
    OpenProviderDetail: function () {

        var params = [];
        params["ProviderId"] = $('#' + Batch_Fax.params.PanelID + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProvider';
        params["ParentCtrl"] = 'Batch_Fax';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnlBatchFax #txtProvider').attr("ProviderId", "-1");
            $('#pnlBatchFax #hfProviderId').val("-1");
            $("#pnlBatchFax #lnkProviderEdit").css("display", "none");
            $("#pnlBatchFax #lblProvider").css("display", "inline");
        } else {

            $("#pnlBatchFax #lnkProviderEdit").css("display", "inline");
            $("#pnlBatchFax #lblProvider").css("display", "none");
        }
    },
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProviderId";
        params["ParentCtrl"] = "Batch_Fax";
        LoadActionPan('Admin_Provider', params);
    },

    BindProviderOutPut: function () {
        var Ctrl = $("#pnlBatchFax #txtProviderOutPut");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnlBatchFax #hfProviderIdOutPut");
        var onSelect = function (e) {
            $("#pnlBatchFax #hfProviderIdOutPut").val(e.id);

        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);


    },
    ResetProviderOutPut: function () {
        $("#pnlBatchFax #hfProviderIdOutPut").val("");

    },
    OpenProviderDetailOutPut: function () {

        var params = [];
        params["ProviderId"] = $('#' + Batch_Fax.params.PanelID + ' #hfProviderIdOutPut').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = 'txtProviderOutPut';
        params["ParentCtrl"] = 'Batch_Fax';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLinkOutPut: function (value) {
        if (value == "") {
            $('#pnlBatchFax #txtProviderOutPut').attr("ProviderId", "-1");
            $('#pnlBatchFax #hfProviderIdOutPut').val("-1");
            $("#pnlBatchFax #lnkProviderEditOutPut").css("display", "none");
            $("#pnlBatchFax #lblProviderOutPut").css("display", "inline");
        } else {

            $("#pnlBatchFax #lnkProviderEditOutPut").css("display", "inline");
            $("#pnlBatchFax #lblProviderOutPut").css("display", "none");
        }
    },
    OpenProviderOutPut: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProviderOutPut";
        params["RefCtrlHidden"] = "hfProviderIdOutPut";
        params["ParentCtrl"] = "Batch_Fax";
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
            Batch_Fax.selectAll(this.checked);
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
            Batch_Fax.addRemoveFaxId(item, item.id);
        });
    },

    addRemoveFaxId: function (item, id) {

        id = id.toString();
        var find = $.inArray(id, Batch_Fax.CheckedBoxes);
        if (item.checked && find == -1) {
            Batch_Fax.CheckedBoxes.push(id);
        } else if (!item.checked && find > -1) {
            Batch_Fax.CheckedBoxes.splice(find, 1);
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
        Batch_Fax.getInbox(false);
        Batch_Fax.CheckedBoxes = [];
    },
    getInbox: function (search) {

        var obj = null;
        if (search) {
            //if ($("#dtpFromDate").val().length > 0 && $("#dtpToDate").val().length > 0) {
                obj = {};
                obj["sPeriod"] = "RANGE";
                obj["ProviderId"] = $("#pnlBatchFax #hfProviderId").val();
                obj["sStartDate"] = $("#dtpFromDate").val().length > 0 ? moment($("#dtpFromDate").val()).format("YYYYMMDD") : moment(Date()).format("YYYYMMDD");
                obj["sEndDate"] = $("#dtpToDate").val().length > 0 ? moment(Date()).format("YYYYMMDD") : moment(Date()).format("YYYYMMDD");
            //} else {
            //    utility.DisplayMessages("Please select both From and To dates", 4);
            //    return;
            //}
        }
        Batch_Fax.requestAPI(obj, "GetFaxInbox")
          .done(function (data) {
              var Batch_FaxInbox = JSON.parse(data);
              if (Batch_FaxInbox.Status == "Success") {
                  Batch_Fax.inboxGridLoad(Batch_FaxInbox.Result);
                  Batch_Fax.QuickIconCount(Batch_FaxInbox.Result);
              } else {
                  utility.DisplayMessages(Batch_FaxInbox.Result, 4);
              }
          })
          .fail(function (e) {
              utility.DisplayMessages(e.responseText, 4);
          });
    },
    inboxGridLoad: function (Batch_FaxInbox) {
        $('#tblInbox').dataTable().fnDestroy();
        $("#tblInbox tbody").find("tr").remove();
        var rows = '';
        Batch_Fax.unreadItems = 0;
        $.each(Batch_FaxInbox, function (i, item) {
            var id = item.FileName.split('|')[1];
            if (item.ViewedStatus == 'N') {
                Batch_Fax.unreadItems = Batch_Fax.unreadItems + 1;

                rows += '<tr class="bold" id="' + id + '"><td><input onclick="Batch_Fax.addRemoveFaxId(this,\'' + id + '\');" type="checkbox"  id="' + id + '"><label for = "' + item.FileName + '"></label></td><td><a class="btn  btn-xs" href="#" onclick="Batch_Fax.deleteFax(\'' + item.FileName + '\',\'IN\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax.PreviewFax(\'' + item.FileName + '\',\'IN\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a></td><td>' + Batch_Fax.MaskNumber(item.CallerID, "IN") + '</td><td>' + utility.DateTemplate(item.Date) + '</td></tr>';
            }
            else {
                rows += '<tr  id="' + id + '"><td><input onclick="Batch_Fax.addRemoveFaxId(this,' + id + ');" type="checkbox"  id="' + id + '"><label for = "' + item.FileName + '"></label></td><td><a class="btn  btn-xs" href="#" onclick="Batch_Fax.deleteFax(\'' + item.FileName + '\',\'IN\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax.PreviewFax(\'' + item.FileName + '\',\'IN\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a></td><td>' + Batch_Fax.MaskNumber(item.CallerID, "IN") + '</td><td>' + utility.DateTemplate(item.Date) + '</td></tr>';
            }
        });
        $('#tblInbox tbody').append(rows);
        if (!$.fn.DataTable.isDataTable('#tblInbox')) {
            $("#tblInbox").DataTable({
                "bInfo": true, "searching": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 15, "columnDefs": [{ "bSortable": false, "targets": [0, 1, 2, 3], "orderable": false }]
            });

            //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                utility.removePaginationFromGrid($('#' + Batch_Fax.params.PanelID + ' #Inbox'));
            //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        }
        // Set inbox item count
        if (Batch_Fax.unreadItems > 0) {
            $('#inboxTab').text('Inbox (' + Batch_Fax.unreadItems + ')');
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
        Batch_Fax.getOutbox(false);
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
        obj["ProviderId"] = $("#pnlBatchFax #hfProviderIdOutPut").val() == "-1" ? null : $("#pnlBatchFax #hfProviderIdOutPut").val();
        obj["ResultPerPage"] = ResultPerPage;
        if (!Batch_Fax.params.PatientId) {
            obj["PatientId"] = 0;
        }
        else { obj["PatientId"] = Batch_Fax.params.PatientId; }
        Batch_Fax.requestAPI(obj, "GetFaxOutbox").done(function (data) {

            var Batch_FaxOutbox = JSON.parse(data);
            if (Batch_FaxOutbox.status) {
                Batch_Fax.outboxGridLoad(Batch_FaxOutbox);
                var TableControl = Batch_Fax.params.PanelID + " #tblOutbox";

                var PagingPanelControlID = Batch_Fax.params.PanelID + " #dgvtblOutbox_Paging";
                var ClassControlName = "Batch_Fax";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = Batch_FaxOutbox.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(Batch_FaxOutbox.Batch_FaxOutboxCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (Id, PageNumber, ResultPerPage) {
                        Batch_Fax.getOutbox(search, PageNumber, ResultPerPage);
                    }), 10);
            } else {
                utility.DisplayMessages(Batch_FaxOutbox.Message, 4);
            }
        })
        .fail(function (e) {
            utility.DisplayMessages(e.responseText, 4);
        });
    },
    outboxGridLoad: function (Batch_FaxOutbox) {
        $('#tblOutbox').dataTable().fnDestroy();
        $("#tblOutbox tbody").find("tr").remove();
        if (Batch_FaxOutbox.Batch_FaxOutboxCount > 0) {
            var rows = '';
            $.each(Batch_FaxOutbox.Result, function (i, item) {
                var id = item.FaxDetailsID;
                if (item.SentStatus == 'Sent') {
                    rows += '<tr id="' + id + '"><td style="display:none;">' + id + '</td><td><a class="btn  btn-xs" onclick="Batch_Fax.deleteFax(\'' + item.FileName + '\',\'OUT\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax.PreviewFax(\'' + item.FileName + '\',\'OUT\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a></td><td>' + Batch_Fax.MaskNumber(item.ToFaxNumber, "OUT") + '</td><td>' + item.Subject + '</td><td>' + item.SentStatus + '</td><td>' + item.CreatedOn + '</td><td>' + item.SentToName + '</td><td>' + item.Pages + '</td><td>' + item.ProviderName + '</td><td>' + item.SenderName + '</td></tr>';
                } else if (item.SentStatus == 'Failed') {
                    rows += '<tr id="' + id + '"><td style="display:none;">' + id + '</td><td><a class="btn  btn-xs" onclick="Batch_Fax.deleteFax(\'' + item.FileName + '\',\'OUT\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax.PreviewFax(\'' + item.FileName + '\',\'OUT\',this,event);" title="Preview Fax"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Batch_Fax.FaxStatus(' + id + ',event);" title="Fax Status"> <i class="fa fa-info-circle blue"></i></a>&nbsp;<a class="btn  btn-xs" onclick="Batch_Fax.ForwardFax(' + id + ',event);" title="Resend Fax"> <i class="glyphicon glyphicon-refresh blue"></i></a></td><td>' + Batch_Fax.MaskNumber(item.ToFaxNumber, "OUT") + '</td><td>' + item.Subject + '</td><td>' + item.SentStatus + '</td><td>' + item.CreatedOn + '</td><td>' + item.SentToName + '</td><td>' + item.Pages + '</td><td>' + item.ProviderName + '</td><td>' + item.SenderName + '</td></tr>';
                } else {
                    rows += '<tr id="' + id + '"><td style="display:none;">' + id + '</td><td></td><td>' + Batch_Fax.MaskNumber(item.ToFaxNumber, "OUT") + '</td><td>' + item.Subject + '</td><td>' + item.SentStatus + '</td><td>' + item.CreatedOn + '</td><td>' + item.SentToName + '</td><td>' + item.Pages + '</td><td>' + item.ProviderName + '</td><td>' + item.SenderName + '</td></tr>';
                }

            });
            $('#tblOutbox tbody').append(rows);
        }
        else {

            $('#tblOutbox').DataTable({
                "language": {
                    "emptyTable": "No Fax Found"
                }, "autoWidth": false, "searching": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false,  "bPaginate": false, "targets": [0, 1, 2, 3, 4,5,6,7,8,9], "orderable": false }]
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

        Batch_Fax.requestAPI(inputParam, "GetFaxStatus")
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

        Batch_Fax.requestAPI(inputParam, "ForwardFax")
         .done(function (data) {
             var Fax = JSON.parse(data);
             if (Fax.Status == "Success") {
                 utility.DisplayMessages("Fax has been queued successfully.", 1);
                 Batch_Fax.getOutbox(false);
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

        Batch_Fax.requestAPI(inputParam, "RetrieveFax")
         .done(function (data) {
             var Fax = JSON.parse(data);

             if (Fax.Status == "Success") {
                 var params = [];
                 Batch_FaxSend.AnnotateArray["base64"] = "data:application/pdf;base64," + Fax.Result;
                 params["ParentCtrl"] = 'Batch_Fax';
                 params["FaxId"] = FileNameId;
                 LoadActionPan('Batch_FaxSendAnnotate', params);

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
        params["access_token"] = Batch_Fax.access_token;
        params["ParentCtrl"] = 'Batch_Fax';
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

            Batch_Fax.faxDelete(id, sDirection);
            Batch_Fax.getInbox(false);
        }, function () { });
    },
    faxDelete: function (id, sDirection) {
        var obj = {};
        //obj["sFaxFileName"] = FileNameId;
        obj["sFaxDetailsID_1"] = id;
        obj["sDirection"] = sDirection;

        var inputParam = JSON.stringify(obj);

        Batch_Fax.requestAPI(inputParam, "DeleteFax")
         .done(function (data) {
             var Fax = JSON.parse(data);
             if (Fax.Status == "Success") {
                 if (sDirection == "IN")
                     Batch_Fax.RemoveTableRow(sDirection, id);
                 else
                     Batch_Fax.getOutbox(false);
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
        return MDVisionService.defaultService(Fax, "BATCH_FAX_CLINICAL", "FAX_ACCESS");
    },
    UnLoad: function () {
        if (Batch_Fax.params.ParentCtrl && Batch_Fax.params.ParentCtrl != "batchTabFax") {
            Batch_Fax.destroyGrid();
        }
        if (Batch_Fax.params != null && Batch_Fax.params.ParentCtrl != null) {
            if ( Batch_Fax.params.ParentCtrl == "batchTabFax") {
                Batch_Fax.params = [];
                Batch_Fax.params = params;
            }
            UnloadActionPan(Batch_Fax.params.ParentCtrl, 'Batch_Fax');
        }
        else
            UnloadActionPan(null, 'Batch_Fax');

    },
    faxAccess: function () {
        var data = "FaxData=";
        return MDVisionService.defaultService(data, "BATCH_FAX_CLINICAL", "LOAD_FAX_ACCESS");
    },
    ShowFaxCount: function () {
        if (Batch_Fax.access == true) {
            Batch_Fax.requestAPI(null, "GetFaxInbox")
              .done(function (data) {
                  var Batch_FaxInbox = JSON.parse(data);
                  if (Batch_FaxInbox.Status == "Success") {
                      Batch_Fax.QuickIconCount(Batch_FaxInbox.Result);
                  }
              })
              .fail(function (e) {
                  utility.DisplayMessages(e.responseText, 4);
              });

        }
        else if (Batch_Fax.access == false) {
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
            for (check = 0 ; check < Batch_Fax.CheckedBoxes.length ; check++) {
                Batch_Fax.faxDelete(Batch_Fax.CheckedBoxes[check], "IN");
            
                if (check == Batch_Fax.CheckedBoxes.length - 1)
                {
                    defConfirm.resolve('ok');
                }
            }
            $.when(defConfirm).done(function () {
                Batch_Fax.CheckedBoxes = [];

                Batch_Fax.getInbox(false);
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