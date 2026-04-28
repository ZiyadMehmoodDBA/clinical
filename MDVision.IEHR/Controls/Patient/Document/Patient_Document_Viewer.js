Document_Viewer = {
    bIsFirstLoad: true,
    params: [],
    scale: 1.0,
    scaleMultiplier: 0.8,
    needCanvasReset: false,
    interval: null,
    ObjVisitDeffered: '',
    PasswordJSON: "",
    PrivateDoc: "",
    NxtPrvPatDocID: "",
    NxtPrvPatientID: "",
    NxtPrvLinkDocGrdName: '',
    SectionRowIndex: null,
    multipleFoldersIds: "",
    //bVisitFirst: null,
    Load: function (params) {
        Document_Viewer.params = params;
        var self = $('#' + params.PanelID + ' #pnlDocumentViewer');
        //Document_Viewer.bVisitFirst = true;
        $.when(Document_Viewer.LoadDocumentProviderDropDown(), self.loadDropDowns(true)).then(function () {


            Document_Viewer.BindPatientAccount();
            Document_Viewer.BindTagAutocomplete();
            Document_Viewer.ValidateViewer();
            Document_Viewer.DocumentFill();
            Document_Viewer.LoadPatientCase(Document_Viewer.params.PatientID);
            Document_Viewer.BindClaimNumber();
            Document_Viewer.BindDocumentSourceDropDown();
            // intilize multi select drop down list
            Document_Viewer.IntializeMultiSelectDropDownFolders();
            $('#' + Document_Viewer.params.PanelID + ' #divPatientDocFolder .multiselect-container').find("li").eq(1).remove();
            $('#' + Document_Viewer.params.PanelID + ' #ddlFolder').find('option:contains(- Select -)').remove();
        });

        if (Document_Viewer.params.ParentCtrl == "advancePaymentDetail") {
            $('#pnlDocumentViewer #frmDocumentViewer #txtPatientName').attr("disabled", "disabled");
            $('#pnlDocumentViewer #frmDocumentViewer #lnkPatientAccount').attr("disabled", "disabled");
        }
        if (Document_Viewer.params.ParentCtrl == "Patient_Information_Submission") {
            $('#pnlDocumentViewer #frmDocumentViewer #lnkPreviousDocument').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #lnkNextDocument').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #btnDocumentDelete').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #btnSave').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #formcontrols').addClass('toggled1');
            $('#pnlDocumentViewer #frmDocumentViewer #formcontrols').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #collaspe-left').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #btnFaxDocument').hide();
        }
        if (Document_Viewer.params.RefDemographic == "View_Only") {
            $('#pnlDocumentViewer #frmDocumentViewer #lnkPreviousDocument').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #lnkNextDocument').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #btnDocumentDelete').hide();
            $('#pnlDocumentViewer #frmDocumentViewer #collaspe-left').hide();
        }

        if (Document_Viewer.params.RefDemographic == "View_Only" || Document_Viewer.params.ParentCtrl == "Patient_CustomForm") {
            $('#pnlDocumentViewer #frmDocumentViewer #formcontrols').hide();
        }
        //Start 28-04-2016 Humaira Yousaf to show lab result
        if (Document_Viewer.params.ParentCtrl == "ClinicalLabResultDetail" || Document_Viewer.params.ParentCtrl == "ClinicalRadiologyResultDetail" || Document_Viewer.params.ParentCtrl == "Clinical_RadiologyOrder" || Document_Viewer.params.ParentCtrl == "clinicalTabRadiologyOrder" || Document_Viewer.params.ParentCtrl == "Patient_Referrals_Incoming_Detail" || Document_Viewer.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Document_Viewer.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail" || Document_Viewer.params.ParentCtrl == 'patTabDemographic') {
            $('#pnlDocumentViewer #frmDocumentViewer #CtrDocumentViewer').addClass("disableAll");
            $('#pnlDocumentViewer #frmDocumentViewer #lnkPreviousDocument').addClass("disableAll");
            $('#pnlDocumentViewer #frmDocumentViewer #lnkNextDocument').addClass("disableAll");
        }
        //End 28-04-2016 Humaira Yousaf to show lab result
        //if (Document_Viewer.params.ParentCtrl == 'demographicDetail')
        //    $('#' + params.PanelID + ' #pnlDocumentViewer #CtrDocumentViewer').css("display", "none");


        if (Document_Viewer.params.ParentCtrl == 'clinicalTabPatientEducation' || Document_Viewer.params.ParentCtrl == "Clinical_PatientEducation" || Document_Viewer.params.ParentCtrl == "OrderSet_PatientEducation" || Document_Viewer.params.ParentCtrl == "Clinical_OrderSetDetails") {
            $("#" + Document_Viewer.params.PanelID + " #btnReviewDocument").addClass('hidden');
            $("#" + Document_Viewer.params.PanelID + " #btnSignDocument").addClass('hidden');
            $("#" + Document_Viewer.params.PanelID + " #btnSave").addClass('hidden');
            $("#" + Document_Viewer.params.PanelID + " #lnkPreviousDocument").hide();
            $("#" + Document_Viewer.params.PanelID + " #lnkNextDocument").hide();
            $("#" + Document_Viewer.params.PanelID + " #CtrDocumentViewer").hide();
            $("#" + Document_Viewer.params.PanelID + " #collaspe-left").hide();
        }
        // hide conntrols from Patient demographic Ast-94 Fixed:EMR-6442
        if (Document_Viewer.params.ParentCtrl == 'patTabDemographic' || Document_Viewer.params["isFromShowAttachment"]) {
            $("#" + Document_Viewer.params.PanelID + " #btnReviewDocument").addClass('hidden');
            $("#" + Document_Viewer.params.PanelID + " #btnSignDocument").addClass('hidden');
            $("#" + Document_Viewer.params.PanelID + " #btnSave").addClass('hidden');
            $("#" + Document_Viewer.params.PanelID + " #lnkPreviousDocument").hide();
            $("#" + Document_Viewer.params.PanelID + " #lnkNextDocument").hide();
          //  $("#" + Document_Viewer.params.PanelID + " #CtrDocumentViewer").hide();
          //  $("#" + Document_Viewer.params.PanelID + " #collaspe-left").hide();
            $("#" + Document_Viewer.params.PanelID + " #btnFaxDocument").hide();
            $("#" + Document_Viewer.params.PanelID + " #btnPrint").hide();
            $("#" + Document_Viewer.params.PanelID + " #btnDocumentDelete").hide();
        }
        if (Document_Viewer.params.FolderID || Document_Viewer.params.ParentCtrl == "EncounterChargeCapture") {
            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer" + " #lnkPreviousDocument").hide();
            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer" + " #lnkNextDocument").hide();
        }
        if (Document_Viewer.params.ParentCtrl == 'Bill_Insurance_Payment_Detail') {
            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer" + " #lnkPreviousDocument").addClass("disableAll");
            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer" + " #lnkNextDocument").addClass("disableAll");
        }
        //if (Document_Viewer.params.ParentCtrl == 'mstrTabDashBoard') {
        //    $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #lnkPreviousDocument").hide();
        //    $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #lnkNextDocument").hide();
        //} 
        Document_Viewer.SetCollapseExpandPanelPatientDocument();
        Document_Viewer.params.DocPrivate == "0";
    },
    IntializeMultiSelectDropDownFolders: function () {
        try {
            $('#' + Document_Viewer.params.PanelID + ' #ddlFolder').multiselect('destroy');
            $('#' + Document_Viewer.params.PanelID + ' #ddlFolder').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                onChange: function (element, checked) {
                    var self = $("#" + Document_Viewer.params.PanelID);
                    var FoldersIds = self.find('#divPatientDocFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                        return this.value;
                    }).get().join(',');
                    Document_Viewer.multipleFoldersIds = FoldersIds;
                    if (Document_Viewer.multipleFoldersIds != '')
                        Document_Viewer.validateFolders(2);
                    else
                        Document_Viewer.validateFolders(1);
                }
            });
        }
        catch (ex) {
            console.log(ex);
        }
    },
    validateFolders: function (operationid) { 
        $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divFolder label").find("i").remove();
        if (operationid == 1) {
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder .multiselect").css("border-color", "#cc2724");
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder").find(".control-label").css("color", "#cc2724");
           // $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Folder" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder .multiselect").css("border-color", "#3c763d");
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder").find(".control-label").css("color", "#3c763d");
           // $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Folder" style="display: block;"></i>');
        } else {
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder .multiselect").css("border-color", "#ccc");
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divPatientDocFolder").find(".control-label").css("color", "#000000");
        }
    },
  
    SetCollapseExpandPanelPatientDocument: function () {

        //1- Initialization
        $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer .splitterBtn a'));

        $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #splitterBody').show();
        }
        else {
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #splitterBody').removeClass('splitterBody active');
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #splitterBody').hide();
        }

    },
    ReviewDocument: function (IsReviewed, IsSigned, patientId, DocumentId) {
        isFolderMark = false;
        if (patientId) {
            Document_Viewer.params.PatientID = patientId;
        }
        if (DocumentId) {
            Document_Viewer.params.PatDocID = DocumentId;
        }
        var IsMessage = null;
        if (Document_Viewer.params.ParentCtrl == "Patient_MessageEdit")
            IsMessage = "1";
        var MessageType = "6";
        //if (IsReviewed == "1") {
        //    MessageType = "6";
        //}
        //else
        if (IsReviewed == "0" && IsSigned == "1") {
            MessageType = "7";
        }
        if (MessageType == "7") {
            utility.myConfirm(MessageType, function () {
                Document_Viewer.updatePatientDocument(IsReviewed, IsMessage, IsSigned,1);
            }, function () { },  MessageType);
        } else {
            Document_Viewer.updatePatientDocument(IsReviewed, IsMessage, IsSigned, 1);
        }
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Document_Viewer';
        LoadActionPan('Patient_Search', params);
    },

    LoadDocumentProviderDropDown: function () {
        var data = "IsActive=&ID=" + Document_Viewer.params.PatientID;
        return MDVisionService.lookups('GetDocumentProvider', true, data).done(function (result) {
            if (result['GetDocumentProvider']) {
                var options = JSON.parse(result['GetDocumentProvider']);
                var $documentProviderDDL = $('#' + Document_Viewer.params.PanelID + ' #ddlDocumentProvider');
                $documentProviderDDL.empty();
                $.each(options, function (i, item) {
                    if (item.Value != null) {
                        $documentProviderDDL.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                    }
                });
                $documentProviderDDL.append(
                    $('<option/>', {
                        value: 0,
                        html: 'Self',
                        refname: '',
                        refvalue: ''

                    })
                );
            }
        })
    },

    BindDocumentSourceDropDown: function () {
        $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDocumentSource").on("change", function (e) {
            if ($("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDocumentSource option:selected").text().toLowerCase() == "other") {
                $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divOtherDocumentSource").removeClass('hidden');
                $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtOtherDocumentSource").focus();
            }
            else {
                $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divOtherDocumentSource").addClass('hidden');
                $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtOtherDocumentSource").val('');
            }
        });
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#pnlDocumentViewer #hfPatientId").val(PatientId);
        $("#pnlDocumentViewer #txtPatientName").val(patFullName);
        if ($("#" + Document_Viewer.params.PanelID + " #lnkPatientNameEdit").css("display") == "none") {
            $("#" + Document_Viewer.params.PanelID + " #lnkPatientNameEdit").css("display", "inline");
            $("#" + Document_Viewer.params.PanelID + " #lblPatientName").css("display", "none");
        }
        //appointmentDetail.FillPatientAccount(PatientId);
        //item.AccountNumber + " - " + item.FullName
        UnloadActionPan("Document_Viewer");
        utility.InsertRecentPatient(PatientId);
        //$('#frmDocumentViewer').bootstrapValidator('revalidateField', 'PatientName');
        Document_Viewer.LoadPatientVisitDOS(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($("#pnlDocumentViewer #txtPatientName"), patFullName, $("#pnlDocumentViewer #hfPatientId"), PatientId);
    },
    BindPatientAccount: function () {
        var Ctrl = $('#' + Document_Viewer.params.PanelID + ' #txtPatientName');
        var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#pnlDocumentViewer #hfPatientId");
        var onSelect = function (e) {
            utility.InsertRecentPatient(e.id);
            Document_Viewer.LoadPatientVisitDOS(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect);

        setTimeout(function () { $('#pnlDocumentViewer #OpenDocumentIF').css('height', '68vh'); }, 1000);
        setTimeout(function () { $('#pnlDocumentViewer #canvasContainer').css('max-height', ($(document).height() - 120) + "px"); }, 1000);

        $('#pnlDocumentViewer .splitterBtn ').click(function () {
            if ($(this).prev().hasClass('active')) {
                $('#pnlDocumentViewer #OpenDocumentIF').css('height', '85vh');
                $('#pnlDocumentViewer #canvasContainer').css('max-height', '85vh');
            }
            else {
                $('#pnlDocumentViewer #OpenDocumentIF').css('height', '68vh');
                $('#pnlDocumentViewer #canvasContainer').css('max-height', '68vh');
            }
        })
    },
    BindTagAutocomplete: function () {
        var Ctrl = $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtTagName");
        var hfId = $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #hfDocumentTagId");
        var func = function () { return utility.GetDocumentTagArray(Ctrl.val(), 0) };
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfId);
    },
    ZoomIn: function () {
        var canvas = $("#canvas");
        canvas.css("width", (canvas.width() * 2) + "px");
        //document_Viewer.drawCanvas(2);
    },

    ZoomOut: function () {
        var canvas = $("#canvas");
        canvas.css("width", (canvas.width() / 2) + "px");
        //Document_Viewer.drawCanvas(0.5);
    },

    drawCanvas: function (scale) {
        var canvas = document.getElementById('canvas');
        var context = canvas.getContext('2d');
        if (Document_Viewer.needCanvasReset) {
            context.setTransform(1, 0, 0, 1, 0.5, 0.5);
            Document_Viewer.needCanvasReset = false;
        }
        context.scale(scale, scale);
        context.beginPath();
        context.restore();
    },

    navigateCanvas: function (navigation) {
        var canvas = document.getElementById("canvas");
        var context = canvas.getContext("2d");
        needCanvasReset = true;
        switch (navigation) {
            case "up":
                context.translate(0, -10);
                break;
            case "down":
                context.translate(0, 10);
                break;
            case "left":
                context.translate(-10, 0);
                break;
            case "right":
                context.translate(10, 0);
                break;
            case "reset":
                $("#canvas").removeAttr("style");
                //context.setTransform(1, 0, 0, 1, 0.5, 0.5);
                break;
        }
    },

    updatePatientDocument: function (IsReviewed, IsMessage, IsSigned, isFromMarkAndReview) {
        var self = $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer");
        var myJSON = self.getMyJSON();
        var addedNewFolderList = [];
        var deletedNewFolderList = [];
        var FolderList = [];
        var FoldersSelectedIds = [];
        var FoldersDeletedId = [];
        var isFolderMark = "";
        var deleteFoldersNow = "";
        var foldersAddedNow = "";
        if (Document_Viewer.params["LoadedFolderIds"])
        var loadedFolderId = Document_Viewer.params["LoadedFolderIds"].split(",");

        // get selected folders        
        self.find('#divPatientDocFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            var objectFolder = new Object();
            var objectSelectedFolder = new Object();
            objectSelectedFolder = this.value;
            FoldersSelectedIds.push(objectSelectedFolder);
            objectFolder.Value = this.value;
            objectFolder.Name = this.nextSibling.data.trim();
            FolderList.push(objectFolder);
        });
        // chek user mark folder checked/Uncheked or not
               
      
        // get new delete folders ids and make list 
        if (loadedFolderId) {
             deleteFoldersNow = loadedFolderId.filter(function (n) { return !this.has(n) }, new Set(FoldersSelectedIds))
             foldersAddedNow = FoldersSelectedIds.filter(function (n) { return !this.has(n) }, new Set(loadedFolderId))
        }      
        // if user did not mark check/uncheck new folder
        if (deleteFoldersNow.length > 0 || foldersAddedNow.length > 0) {
            isFolderMark = true;
            // get new selected folders id and make folder Added List  
            if (deleteFoldersNow.length > 0) {
                $.each(deleteFoldersNow, function (i, folderId) {
                    var objFolderDeleted = new Object();
                    objFolderDeleted.Value = folderId
                    objFolderDeleted.Name = $("#" + Document_Viewer.params["PanelID"] + " #divPatientDocFolder ul.multiselect-container li :checkbox[value='" + folderId + "']").parent().text();
                    deletedNewFolderList.push(objFolderDeleted);
                });
            }
            if (foldersAddedNow.length > 0) {
                $.each(foldersAddedNow, function (i, folderId) {
                    var objFolderAded = new Object();
                    objFolderAded.Value = folderId;
                    objFolderAded.Name = $("#" + Document_Viewer.params["PanelID"] + " #divPatientDocFolder ul.multiselect-container li :checkbox[value='" + folderId + "']").parent().text().trim();
                    addedNewFolderList.push(objFolderAded);
                });
            }
               
        }
        else {
            isFolderMark = false;            
        }
        

        if (isFromMarkAndReview == 1)
        {
            isFolderMark = false;
        }
        
        if (FolderList.length > 0) {
            var IsMessage = null;
            if (Document_Viewer.params.ParentCtrl == "Patient_MessageEdit")
                IsMessage = "1";
            if (Document_Viewer.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Document_Viewer.PatientDocumentUpdate(myJSON, Document_Viewer.params.PatientID, Document_Viewer.params.PatDocID, IsReviewed, IsMessage, IsSigned, JSON.stringify(FolderList), JSON.stringify(deletedNewFolderList), JSON.stringify(addedNewFolderList), isFolderMark).done(function (response) {
                            if (response.status != false) {
                                if (response.PatDocId)
                                { Document_Viewer.params.PatDocID = response.PatDocId; }
                                Document_AssignedTo.PendingCountSearchDoc();
                                if (Document_Viewer.params.RefCtrl == "advancePayment") {
                                    utility.DisplayMessages(response.message, 1);
                                    UnLoadTab();
                                } else {

                                    if (Document_Viewer.params.ParentCtrl != "Patient_MessageEdit") {
                                        //Patient_Document.DocumentSearch();
                                        //var ListItemId = Document_Viewer.params.FolderID;
                                        //Patient_Document.LoadFoldersAfterDeleteOrUpdateRecords().done(function () {
                                        //    if (ListItemId != null) {
                                        //        ListItemHighLight(ListItemId);
                                        //    }

                                    //});
                                    if (!Document_Viewer.params["IsDashboardDocUpdated"]) {
                                        if (!Document_Viewer.ValidateDocumentSearchScreen()) {
                                            utility.AddDaysFromToDate(Patient_Document.params.PanelID + " #frmPatientDocument", 'dtpFromEntry', 'dtpToEntry', -15, 0);
                                            if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {
                                                Patient_Document.LoadFolders(true).done(function () {
                                                    if (Document_Viewer.params.PatDocID) {
                                                        utility.callbackAfterAllDOMLoaded(function () {
                                                            setTimeout($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + Document_Viewer.params.PatDocID).find("a").click(), 1000);
                                                        });
                                                    }
                                                });
                                            }
                                            else { Patient_Document.LoadFolders(true); }
                                        }
                                        else {
                                            if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {
                                                Patient_Document.LoadFolders(true).done(function () {
                                                    $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentGridView).show();
                                                    $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentView).hide();
                                                    if (Document_Viewer.params.PatDocID) {
                                                        utility.callbackAfterAllDOMLoaded(function () {
                                                            setTimeout($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + Document_Viewer.params.PatDocID).find("a").click(), 1000);
                                                        });
                                                    }
                                                });
                                            }
                                            else { Patient_Document.LoadFolders(true); }
                                        }
                                    }
                                }
                                else if (Document_Viewer.params.ParentCtrl == "Patient_MessageEdit")
                                    Patient_MessageEdit.FillMessage(Document_Viewer.params.MessageId);
                                utility.DisplayMessages(response.message, 1);
                                //Patient_Document.DocumentSearch();
                                clearInterval(Document_Viewer.interval);
                                if (((IsSigned && IsSigned == "1") || (IsReviewed && IsReviewed == "1")) && ($("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #lnkNextDocument").hasClass("disabled") == false || Document_Viewer.params.ParentCtrl == "mstrTabDashBoard")
                                     //&& (typeof Document_Viewer.params.FolderID == "undefined")
                                ) {
                                    if (Document_Viewer.params.ParentCtrl == "mstrTabDashBoard" ) {
                                        Document_Viewer.nextDocument();
                                    }
                                    else if (typeof Document_Viewer.params.FolderID == "undefined")
                                    {
                                        Document_Viewer.nextDocument();
                                    }
                                    else if (((IsSigned && IsSigned == "1") || (IsReviewed && IsReviewed == "1")) && Document_Viewer.params.FolderID) {
                                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer  #btnReviewDocument").hide();
                                    }
                                } else {
                                    Document_Viewer.DocumentFill();
                                }
                            }


                                if (Document_Viewer.params["IsDashboardDocUpdated"]) {
                                    Document_Viewer.params["IsDashboardDocUpdated"] = "1";
                                }
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        }
        else
            Document_Viewer.validateFolders(1);
    },

    SaveAnnotatedDocument:function()
    {
        var data = new FormData();
        var panel = Document_Viewer.params["PanelID"];
        if (panel.indexOf("pnlDocumentViewer") <= 0)
            panel = panel + " #pnlDocumentViewer";
        data.append("Base64", $('#' + panel + ' #helperPDF').val());
        data.append("PatDocID", Document_Viewer.params.PatDocID);

        //var data = "PatDocID=" + PatDocID + "&bFileStream=1" + "&Base64=" + Base64;
        // serach parameter , class name, command name of class
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "SAVE_ANNOTATED_DOCUMENT");
    },
        PatientDocumentUpdate: function (PatientDocumentData, PatientID, DocumentID, IsReviewed, IsMessage, IsSigned, FolderList,deletedNewFolderList,AddedNewFoldersList,isFolderMark) {
        if (PatientID == null) {
            PatientID = 0;
        }
        if (IsReviewed == null) {
            IsReviewed = 0;
        }
        if (IsMessage == null) {
            IsMessage = 0;
        }
        if (IsSigned == null) {
            IsSigned = 0;
        }       
            // serach parameter , class name, command name of class
        if (isFolderMark) {
            var FolderName = $('#' + Document_Viewer.params.PanelID + " #ddlFolder option[value='" + Document_Viewer.params.FolderID + "']").text();
            var data = "PatientDocumentData=" + PatientDocumentData + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID + "&IsReviewed=" + IsReviewed + "&IsMessage=" + IsMessage + "&IsSigned=" + IsSigned + "&FolderList=" + FolderList + "&CurrentFolderID=" + Document_Viewer.params.FolderID + "&CurrentFolderName=" + FolderName + "&deletedNewFolderList=" + deletedNewFolderList + "&AddedNewFoldersList=" + AddedNewFoldersList;
            return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "INSERT_PATIENT_DOCUMENT_MULTIPLE_FOLDERS");
        }
        else {
            var data = "PatientDocumentData=" + PatientDocumentData + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID + "&IsReviewed=" + IsReviewed + "&IsMessage=" + IsMessage + "&IsSigned=" + IsSigned + "&CurrentFolderID=" + Document_Viewer.params.FolderID;
            // serach parameter , class name, command name of class
            return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT");
            
        }

        
       
    },

    FillDocument: function (DocumentData, PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=1";
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_DOCUMENT");
    },

    ValidateViewer: function () {
        $('#frmDocumentViewer')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    Folder: {
                        group: '.col-xs-6',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //PatientName: {
                    //    group: '.col-sm-4',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    FileName: {
                        group: '.col-md-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    /* DocumentProvider: {
                         group: '.col-md-2',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     DocumentProvider: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     DocumentSource: {
                         group: '.col-md-2',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     DocumentSource: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     ReceivedOn: {
                         group: '.col-md-2',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },
                     ReceivedOn: {
                         group: '.col-sm-4',
                         validators: {
                             notEmpty: {
                                 message: ''
                             }
                         }
                     },*/
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Document_Viewer.updatePatientDocument();


        });
    },

    DocumentFill: function () {
        var totalDocument = $('#' + Document_Viewer.params.PanelID + ' #dgvPatientDocument tr').length - 1;
        if (totalDocument == 1 && $("#" + Patient_Document.params["PanelID"] + " #DivDocumentView").is(":visible") == false) {
            //$('#' + Document_Viewer.params.PanelID + 'nextDocument #lnkNextDocument,#lnkPreviousDocument').hide();
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkNextDocument,#lnkPreviousDocument').addClass("disabled");
        }
        if (Document_Viewer.params.ParentCtrl == "batchTabDocuments" || Document_Viewer.params.ParentCtrl == "Patient_Document" || Document_Viewer.params.TabID == "patTabDocuments" || Document_Viewer.params.TabID == "batchTabDocuments") {
            Document_Viewer.NextPrevsButtonEnableDisabled();
        }

        if (Document_Viewer.params.ParentCtrl == "clinicalTabLabOrder") {
            var doclength = $('#dgvLabResult_wrapper #menuViewAttachment li').length;
            if (doclength == 1) {
                $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkNextDocument,#lnkPreviousDocument').addClass("disabled");
            }
        }
        var self = $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer');
        Document_Viewer.LoadVisitDOS(Document_Viewer.params.PatientID).done(function (responce) {
            if (responce.PatientDOS.length > 0)
                Document_Viewer.params["visitDos"] = responce.PatientDOS[0].DOSFrom;
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").empty();
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divddlDOS #ddlDOS").append($("<option/>", {
                value: "",
                text: "-Select-"
            }));
            $.each(responce.PatientDOS, function (k, item) {
                $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divddlDOS #ddlDOS").append($("<option/>", {
                    value: utility.RemoveTimeFromDate(null, item.DOSFrom),
                    text: utility.RemoveTimeFromDate(null, item.DOSFrom)
                }));
            });
             
            if (Document_Viewer.params.ParentCtrl == "Patient_Information_Submission")
                var documentCall = Patient_Information_Viewer.FillDocument(Document_Viewer.params.Url, Document_Viewer.params.FileType);
            else 
                var documentCall = Document_Viewer.FillDocument(null, Document_Viewer.params.PatientID, Document_Viewer.params.PatDocID);
            $.when(documentCall).done(function (response) {
                // documentCall.done(function (response) {
                if (response.status != false) {
                    var document_details = JSON.parse(response.DocumentLoad_JSON);
                    if (document_details.txtAccountNumber) {
                        $('#pnlDocumentViewer #pHeaderPatientInfo').html("<b> | </b>" + document_details.txtPatientNameFull + ' (' + document_details.txtAccountNumber + ') ' + utility.RemoveTimeFromDate(null, document_details.dtpDOB));
                    }
                    var linkedTitle = '';
                    if (Document_Viewer.NxtPrvLinkDocGrdName != "MainGrid") {
                        linkedTitle = " | Linked";
                    }
                         
                    
                    


                    if (document_details.txtFileName)
                        $('#pnlDocumentViewer #smDocumentInfo').html("<b> | </b>" + document_details.txtFileName + document_details.lnkFileNameExt + linkedTitle);

                    $('#pnlDocumentViewer  #frmDocumentViewer #hfXddlAssignedUserto').val(document_details.ddlAssignUserto);
                    var FileType = document_details.FileType;

                    if (document_details.DocPassword != "") {
                        $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("checked", true);
                        //$("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").parent().removeClass("hidden");
                        if (document_details.PasswordCreatedBy == globalAppdata.AppUserId) {
                            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("disabled", false);
                        } else {
                            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("disabled", true);
                        }
                    } else {
                        $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("checked", false);
                        //$("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").parent().addClass('hidden')
                    }

                    var LoadPrevious = document_details.txtLoadPrevious;
                    if (document_details.ReviewDate != "") {
                        $('#pnlDocumentViewer button#btnReviewDocument').css("display", "none");
                        $('#pnlPatientDocument #frmPatientDocument button#btnReviewDocument').css("display", "none");
                    }
                    else {
                        $('#pnlDocumentViewer button#btnReviewDocument').css("display", "inline");
                    }
                    if (document_details.SignDate != "") {
                        $('#pnlDocumentViewer button#btnSignDocument').css("display", "none");
                    }
                    else {
                        $('#pnlDocumentViewer button#btnSignDocument').css("display", "inline");
                    }
                    $('#pnlDocumentViewer #frmDocumentViewer #hfddlAssignedUserto').val(document_details.ddlAssignUserto);
                    if (FileType && FileType.indexOf("pdf") > -1) {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #btnPrint").hide();
                    }
                    utility.bindMyJSON(true, document_details, false, self).done(function () {
                        $('#' + Document_Viewer.params.PanelID + " #ddlFolder").val(document_details.ddlFolder);
                        $('#' + Document_Viewer.params.PanelID + " #ddlFolder").multiselect("refresh");
                        var FolderName = $('#' + Document_Viewer.params.PanelID + " #ddlFolder option[value='" + document_details.ddlFolder + "']").text();
                        if (FolderName.toLowerCase() == "pat education")
                            $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #btnDocumentDelete").addClass("hidden");
                        else
                            $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #btnDocumentDelete").removeClass("hidden");

                        var Ctrl_claim = $("#" + Document_Viewer.params.PanelID + " #txtClaimNumber");
                        var hfId_claim = $("#" + Document_Viewer.params.PanelID + " #hfVisitId");
                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl_claim, Ctrl_claim.val(), hfId_claim, hfId_claim.val(), "ClaimNumber");

                        var Ctrl_tag = $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtTagName");
                        var hfId_tag = $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #hfDocumentTagId");
                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl_tag, Ctrl_tag.val(), hfId_tag, hfId_tag.val());

                        var Ctrl_patient = $("#pnlDocumentViewer #txtPatientName");
                        var hfId_patient = $("#pnlDocumentViewer #hfPatientId");
                        utility.SetKendoAutoCompleteSourceforValidate(Ctrl_patient, Ctrl_patient.val(), hfId_patient, hfId_patient.val());
                    });

                    // check box marked related to document,
                    if (document_details.FolderIds) {
                        Document_Viewer.params["LoadedFolderIds"] = document_details.FolderIds;
                        var loadFoldersIds = document_details.FolderIds.split(",");
                        $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #ddlFolder').val(loadFoldersIds);
                        $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #ddlFolder').multiselect("refresh");
                    }
                    else {
                        Document_Viewer.params["LoadedFolderIds"] = document_details.ddlFolder;
                    }
                    // fill folder iDs 
                  
                    Document_Viewer.params["FolderID"] = document_details.ddlFolder;
                   
                   
                   
                    //if (loadFoldersIds) {
                    //    $.each(loadFoldersIds, function (i, item) {
                    //        $("#" + Document_Viewer.params["PanelID"] + " #divPatientDocFolder ul.multiselect-container li :checkbox[value='" + item + "']").prop("checked", "true");
                    //        $("#" + Document_Viewer.params["PanelID"] + " #divPatientDocFolder ul.multiselect-container li :checkbox[value='" + item + "']").parent().parent().parent().addClass("active");

                    //    });
                    //}
                    
                    if (document_details.rdDropdown) {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #rdDropdown").prop("checked", "checked");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").removeAttr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").attr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").val("");
                    }
                    else if (document_details.rdCalendar) {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #rdCalendar").prop("checked", "checked");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").removeAttr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").attr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").val("");
                    }
                        // prd 41
                    else if (Document_Viewer.params["visitDos"]) {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #rdDropdown").prop("checked", "checked");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").removeAttr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").attr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").val("");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS option:eq(1)").prop('selected', true);
                    }
                    else {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #rdCalendar").prop("checked", "checked");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").removeAttr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").attr("disabled", "disabled");
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").val("");

                    }
                    if (document_details.IsAttachedWithNote == 0) {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #lnkDocumentNote").hide();
                    }
                    else {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #lnkDocumentNote").show();
                    }
                    if (document_details.reviewed && document_details.reviewed == "1") {
                        $('#pnlDocumentViewer #divReviewedDoc').css("pointer-events", "none");
                        $('#pnlDocumentViewer #divReviewedDoc #reviewed').addClass("disabled");
                    }
                    if ($("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDocumentSource option:selected").text().toLowerCase() == "other") {
                        $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divOtherDocumentSource").removeClass('hidden');
                    }
                    if (document_details.lnkFileNameExt && document_details.FileType == "") {
                        if (document_details.lnkFileNameExt == ".jpg" || document_details.lnkFileNameExt == ".png") {
                            FileType = "image";
                        }
                    }

                    utility.AutoEnableAutoCompleteLink($('#pnlDocumentViewer #frmDocumentViewer'));
                    utility.CreateDatePicker('pnlDocumentViewer #dtpDOS', function () {
                        //on-change callback method
                    });
                    utility.CreateDatePicker('pnlDocumentViewer #dtpExpirtyDate', function () {
                        //on-change callback method
                    });
                    utility.CreateDatePicker('pnlDocumentViewer #dtpReceivedOn',
                     function (ev) {
                         if ($('#pnlDocumentViewer #frmDocumentViewer').data("bootstrapValidator") != null && typeof $('#frmDocumentViewer').data('bootstrapValidator') != 'undefined') {
                             $('#pnlDocumentViewer #frmDocumentViewer').bootstrapValidator('revalidateField', 'ReceivedOn');
                         }
                     }, false);
                    if (FileType.indexOf("pdf") > -1) {
                        $('#progressnotehtmldoc').remove();
                        $('#pnlDocumentViewer #canvasContainer').hide();
                        $('#pnlDocumentViewer #imagesControls').hide();
                        $('#pnlDocumentViewer #OpenDocumentIF').show();
                        $('#pnlDocumentViewer #OpenHTMLDocument').hide();
                        $('#pnlDocumentViewer #extraContorls').hide();
                        Document_Viewer.params['DocumentType'] = "PDF";
                        $('#progressnotehtmldoc').remove();
                        if (LoadPrevious == "0") {
                            utility.LoadFileData(document_details.Base64FileStream, 'pnlDocumentViewer #OpenDocumentIF');
                        } else {
                            utility.PDFViewer(document_details.Base64FileStream, false, 'pnlDocumentViewer #OpenDocumentIF');
                        }
                    }
                    else if (FileType == null || FileType == "") {

                        //Document_Viewer.LoadImagesData(document_details.Base64FileStream, document_details.FileType);
                        $('#progressnotehtmldoc').remove();
                        $('#pnlDocumentViewer #canvasContainer').hide();
                        $('#pnlDocumentViewer #imagesControls').hide();
                        $('#pnlDocumentViewer #OpenDocumentIF').show();
                        $('#pnlDocumentViewer #OpenHTMLDocument').hide();
                        $('#pnlDocumentViewer #extraContorls').hide();
                        Document_Viewer.params['DocumentType'] = "PDF";
                        $('#progressnotehtmldoc').remove();
                        if (LoadPrevious == "0") {
                            utility.LoadFileData(document_details.Base64FileStream, 'pnlDocumentViewer #OpenDocumentIF');
                        } else {
                            utility.PDFViewer(document_details.Base64FileStream, false, 'pnlDocumentViewer #OpenDocumentIF');
                        }
                    }
                    else if (FileType.indexOf("image") > -1) {
                        if (LoadPrevious == "0") {
                            Document_Viewer.LoadImagesData(document_details.Base64FileStream, document_details.FileType);
                        }
                        else {
                            try {
                                $('#pnlDocumentViewer #OpenDocumentIF').hide();
                                $('#pnlDocumentViewer #OpenHTMLDocument').hide();
                                $('#pnlDocumentViewer #canvasContainer').show();
                                $('#pnlDocumentViewer #imagesControls').show();
                                $('#pnlDocumentViewer #extraContorls').show();
                                Document_Viewer.params['DocumentType'] = "IMAGE";
                                var imageObj = new Image();
                                //for IE
                                //  imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;

                                imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;
                                var canvas = document.getElementById("canvas");
                                setTimeout(function () {
                                    canvas.width = imageObj.width;
                                    canvas.height = imageObj.height;
                                }, 1000);

                                var context = canvas.getContext("2d");


                                function draw() {
                                    var scale = 1;
                                    var originx = 0;
                                    var originy = 0;
                                    context.save();
                                    context.setTransform(1, 0, 0, 1, 0, 0);
                                    context.clearRect(0, 0, canvas.width, canvas.height);
                                    context.restore();
                                    context.drawImage(imageObj, 0, 0);
                                }

                                setTimeout(function () { draw(); }, 1000);
                                Document_Viewer.interval = setInterval(function () { draw(); }, 1000);
                            }
                            catch (ex) {
                                utility.DisplayMessages(ex, 2);
                                console.log(ex);
                            }
                        }
                    }
                    else if (FileType.indexOf("pdf") > -1) {
                        if (document_details.NoteHtml && document_details.NoteHtml != null && document_details.NoteHtml != "undefined") {
                            var ProgressNotehtml = $("<div id='progressnotehtmldoc' ></div>").append(document_details.NoteHtml);
                            $("#pnlDocumentViewer #Notedoc").append(ProgressNotehtml);
                            kendo.drawing.drawDOM("#pnlDocumentViewer #Notedoc", {
                                landscape: false,
                                scale: 0.6,
                                paperSize: "A4",
                                // margin: "2cm 3cm ",
                                margin: {
                                    left: "10mm",
                                    //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    top: "3mm",
                                    //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    right: "10mm",
                                    bottom: "15mm"
                                },
                                rowtemplate: $('#' + Document_Viewer.params["PanelID"] + " #page-templateLegacy").html()
                                // template: kendo.template( $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html())
                            }).then(function (group) {

                                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                                    var pdfurl = dataURL.split('data:application/pdf;base64,').join('');
                                    $('#progressnotehtmldoc').remove();
                                    $('#pnlDocumentViewer #canvasContainer').hide();
                                    $('#pnlDocumentViewer #imagesControls').hide();
                                    $('#pnlDocumentViewer #OpenDocumentIF').show();
                                    $('#pnlDocumentViewer #OpenHTMLDocument').hide();
                                    $('#pnlDocumentViewer #extraContorls').hide();
                                    Document_Viewer.params['DocumentType'] = "PDF";
                                    $('#progressnotehtmldoc').remove();
                                    utility.PDFViewer(pdfurl, false, 'pnlDocumentViewer #OpenDocumentIF');
                                });

                            });
                        } else {
                            $('#pnlDocumentViewer #canvasContainer').hide();
                            $('#pnlDocumentViewer #imagesControls').hide();
                            $('#pnlDocumentViewer #OpenDocumentIF').show();
                            $('#pnlDocumentViewer #OpenHTMLDocument').hide();
                            $('#pnlDocumentViewer #extraContorls').hide();
                            Document_Viewer.params['DocumentType'] = "PDF";

                            // window.open = ContentUri;

                            //PDF Viewer
                            utility.PDFViewer(document_details.Base64FileStream, false, 'pnlDocumentViewer #OpenDocumentIF');
                        }

                    }

                    else if (FileType.indexOf("html") > -1 || FileType.indexOf("text") > -1 || FileType.indexOf("txt") > -1 || FileType.indexOf("zip") > -1) {
                        try {
                            $('#pnlDocumentViewer #OpenHTMLDocument').show();
                            $('#pnlDocumentViewer #OpenDocumentIF').hide();
                            $('#pnlDocumentViewer #canvasContainer').hide();
                            $('#pnlDocumentViewer #imagesControls').hide();
                            $('#pnlDocumentViewer #extraContorls').hide();
                            Document_Viewer.params['DocumentType'] = "HTML";

                            if (FileType.indexOf("text") > -1 || FileType.indexOf("txt") > -1) {
                                var blobFile = Document_Viewer.dataURLtoFile('data:text/plain;base64,' + document_details.Base64FileStream, document_details.txtFileName);
                                //var blobFile = atob(document_details.Base64FileStream);
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').html("<pre>" + reader.result + "</pre>");
                                }
                                reader.readAsText(blobFile);
                            } else if (FileType.indexOf("zip") > -1) {
                                $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').html('<div class=" col-sm-4"><a class="btn btn-success btn-xs" id="linkDownload" download=' + document_details.txtFileName + ' href="data:' + FileType + ';base64,' + document_details.Base64FileStream + '"title="Download File"><i class="fa fa-download"></i>' + document_details.txtFileName + '.zip</a></div>')
                            }
                            else {
                                var blobFile = Document_Viewer.dataURLtoFile('data:application/html;base64,' + document_details.Base64FileStream, document_details.txtFileName);
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').html(reader.result);
                                    if (Document_Viewer.params.ParentCtrl == 'clinicalTabPatientEducation' || Document_Viewer.params.ParentCtrl == "Clinical_PatientEducation" || Document_Viewer.params.ParentCtrl == "OrderSet_PatientEducation" || Document_Viewer.params.ParentCtrl == "Clinical_OrderSetDetails") {
                                        $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').find('#btnEmail').remove();
                                        $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').find('#btnPrinter').remove();
                                        $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').find('#btnDownload').remove();
                                    }
                                }
                                reader.readAsText(blobFile);

                            }

                            if (Document_Viewer.params.ParentCtrl == 'clinicalTabPatientEducation' || Document_Viewer.params.ParentCtrl == 'Clinical_PatientEducation') {
                                $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').find('#btnEmail').remove();
                                $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').find('#btnPrinter').remove();
                                $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').find('#btnDownload').remove();
                            }
                        }
                        catch (ex) {
                            utility.DisplayMessages(ex, 2);
                            console.log(ex);
                        }
                    }

                    var table = $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table');
                    table.find('tbody tr[documentid=' + Document_Viewer.params.PatDocID + ']').addClass('active');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

                if (document_details.ddlFolder)
                var FolderName = $('#' + Document_Viewer.params.PanelID + " #ddlFolder option[value='" + document_details.ddlFolder + "']").text();
                //For Patient Education only
                if (FolderName == "Pat Education") {
                    $('#' + params.PanelID + ' #pnlDocumentViewer #CtrDocumentViewer').css("display", "none");
                    $('#' + params.PanelID + ' #pnlDocumentViewer #collaspe-left').hide();
                    $('#' + params.PanelID + ' #pnlDocumentViewer #splitterbodyLeft,#collaspe-left').css("display", "none");
                    $('#' + params.PanelID + ' #pnlDocumentViewer #lnkPreviousDocument').css("display", "none");
                    $('#' + params.PanelID + ' #pnlDocumentViewer #lnkNextDocument').css("display", "none");
                    $('#' + params.PanelID + ' #pnlDocumentViewer .modal-title').text("Patient Education");
                    // EMR-6413 set patient Education width from Patient Educatuion 
                    setTimeout(function () { $('#pnlDocumentViewer #OpenDocumentIF').css({ 'height': '89vh', 'width': '100%' }); }, 1000);
                } else if (FolderName == "Custom Form") {
                    $('#' + params.PanelID + ' #pnlDocumentViewer #btnSignDocument').hide();
                }
            });
        });
        var isLastPage = $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('#divShowingEntries').text().split(' ')[3] == $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('#divShowingEntries').text().split(' ')[5];
        isPrevPageExists = Number($('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('#divShowingEntries').text().split(' ')[1]) > 1 ? true : false;
        if (Document_Viewer.params.ParentCtrl == "advancePaymentDetail") {
            var table = $('#' + Document_Viewer.params.PanelID + ' #dgvAdvancePaymentDocument');
        }
        else if (Document_Viewer.params.ParentCtrl == "mstrTabDashBoard") {
            var table = $('#' + Document_Viewer.params.PanelID + ' #dgvPatientDocumentGrid_wrapper').find('table');
        }
        else {
            var table = $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table');
        }

        var activeRow = $(table).find('tr.active');
        var index = $.map($(table).find('tr'), function (obj, index) {
            if (obj == activeRow[0]) {
                return index;
            }
        })[0];

        if (isLastPage && index == ($(table).find('tr').length - 1)) {
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkPreviousDocument').removeClass("disabled");
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkNextDocument').addClass("disabled");

        }
        // if table's first row active always previous button disabled.
        if (index == 1) {
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkPreviousDocument').addClass("disabled");
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkNextDocument').removeClass("disabled");

        }

        // if selected row will be last index. 
        if (index == ($(table).find('tr').length - 1)) {
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkNextDocument').addClass("disabled");

        }

        if (isLastPage && index == ($(table).find('tr').length - 1) && index == 1) {
            //utility.myConfirm('9', function () { }, function () { }, '9');
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkNextDocument,#lnkPreviousDocument').addClass("disabled");

        }

        //enable disbale next prev buttons for lab result attach doc view
        if (Document_Viewer.params.ParentCtrl == "clinicalTabLabOrder") {
            var currentdoc = "";
            $('#dgvLabResult_wrapper #menuViewAttachment li').each(function () {
                if ($(this).find('a').attr('class') == "active") {

                    currentdoc = parseInt($(this).find('a').attr('doccount'));

                }
            });
            var doclength = $('#dgvLabResult_wrapper #menuViewAttachment li').length;
            if (doclength == 1) {
                $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkNextDocument,#lnkPreviousDocument').addClass("disabled");
            }
            if (doclength > 1 && currentdoc < doclength) {
                $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkNextDocument').removeClass("disabled");
            }
            if (doclength > 1 && currentdoc > 1) {
                $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkPreviousDocument').removeClass("disabled");
            }
            if (doclength > 1 && currentdoc == 1) {
                $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkPreviousDocument').addClass("disabled");
            }
            if (doclength > 1 && currentdoc == doclength) {
                $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkNextDocument').addClass("disabled");
            }
        }
    },

    dataURLtoFile: function (dataurl, filename) {
        var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new File([u8arr], filename, {
            type: mime
        });
    },

    previousDocument: function (IsFromConfirmPass, PassDocId) {
        if (IsFromConfirmPass == "true") {
            $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table').find("tr[documentid=" + PassDocId + "]").addClass("active");
        }
        $('#' + Document_Viewer.params.PanelID + ' #lnkNextDocument').removeClass("disabled");
        var PatDocID = '';
        //var table = $('#' + Document_Viewer.params.PanelID + ' #dgvPatientDocument');
        if (Document_Viewer.params.ParentCtrl == "advancePaymentDetail") {
            var table = $('#' + Document_Viewer.params.PanelID + ' #dgvAdvancePaymentDocument');
        } else if (Document_Viewer.params.ParentCtrl == "clinicalTabLabOrder") {
            $('#dgvLabResult_wrapper #menuViewAttachment li').each(function () {
                if ($(this).find('a').attr('class') == "active") {
                    $(this).find('a').removeAttr("class");
                    $(this).prev().find('a').attr('class', 'active');
                    PatDocID = $(this).prev().find('a').attr("id");
                    return false;
                }
            });
        }
        else if (Document_Viewer.params.ParentCtrl == "mstrTabDashBoard") {
            var table = $('#' + Document_Viewer.params.PanelID + ' #dgvPatientDocumentGrid_wrapper').find('table');
        }
        else {
            var table = $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table');
        }
        if (Document_Viewer.params.ParentCtrl != "clinicalTabLabOrder") {
            var lNormalFlows = true;

            if (Document_Viewer.params.ParentCtrl == "batchTabDocuments" || Document_Viewer.params.ParentCtrl == "Patient_Document" || Document_Viewer.params.TabID == "patTabDocuments" || Document_Viewer.params.TabID == "batchTabDocuments") {
                Document_Viewer.KendoGrdNextPrevs('previous');
                PatDocID = Document_Viewer.NxtPrvPatDocID;
                Document_Viewer.params["PatientID"] = Document_Viewer.NxtPrvPatientID;
                lNormalFlows = false;
            }

            if (lNormalFlows) {
                $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').text()
                var activeRow = $(table).find('tr.active');
                var pages = $(table).DataTable().page.info().pages;
                // var CurrPage = $(table).DataTable().page.info().page;
                var CurrPage = $(table).parent().parent().parent().find('#pagerParent li.active a').text();
                PatDocID = $(activeRow).prev().attr('documentid');
                Document_Viewer.params["PatientID"] = $(activeRow).prev().attr('Patientid');
                $(activeRow).prev().addClass('active');
                $(activeRow).removeClass('active');
            }
        }
        if (PatDocID != undefined) {
            clearInterval(Document_Viewer.interval);
            Document_Viewer.params['PatDocID'] = PatDocID;
            Patient_Document.CheckforPrivacy(Document_Viewer.params.PatientID, PatDocID).done(function (response) {
                var ShowPasswordAlert;
                if (response.status == true) {
                    if (response.DocPasswordInfoCount > 0) {
                        Document_Viewer.PrivateDoc = response.DocPasswordInfo[0].Password;
                        ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                    }
                }
                if ((Document_Viewer.PrivateDoc == "" && ShowPasswordAlert != "True") || (Document_Viewer.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                    Document_Viewer.DocumentFill();
                } else {
                    if ($('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').length < 1) {
                        Document_Viewer.VerifyPassword(Document_Viewer.params.PatientID, PatDocID, event, null, null, true, false);
                    } else {
                        $('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').modal("show");
                        $("#DivConfirmPasswordViewer #TxtDocPassword").val("");
                        $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentScan").attr("onclick", "Patient_Document.DocPasswordMatch('" + Document_Viewer.params.PatientID + "', '" + PatDocID + "', '" + null + "', '" + null + "', '" + true + "');");
                        $("#modal-from-dom-DocumentConfirmPasswordViewer").find(".close").attr("onclick", "Document_Viewer.cancelConfirmPasswordDialog();Document_Viewer.previousDocument('true', " + PatDocID + ");");
                        $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentSkip").attr("onclick", "Document_Viewer.cancelConfirmPasswordDialog();Document_Viewer.previousDocument('true', " + PatDocID + ");");
                        Document_Viewer.params.DocPrivate = "1";
                    }

                }
            });
        }
        else {
            var recordNumber = Number($('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('#divShowingEntries').text().substr(8).split(' ')[0]);
            var index = $.map($(table).find('tr'), function (obj, index) {
                if (obj == activeRow[0]) {
                    return index;
                }
            })[0];

            if (recordNumber == index) {
                utility.myConfirm('No further documents available inside the folder', function () { }, function () { }, 'Document Alert', "OK", "", true);

            }
            else {
                var self = $("#" + Patient_Document.params["PanelID"] + " #mainContainer");
                var myJSON = self.getMyJSON();
                var docMode = null;
                //if ($('#pnlPatientDocument_Result .tabs ul.nav-tabs li.active a').text().toLowerCase() == "pending")
                if ($('#pnlPatientDocument_Result .tabs-custom li.active a').text().toLowerCase() == 'pending')
                    docMode = '0';
                else
                    docMode = '1';

                Patient_Document.SearchDocument(myJSON, Document_Viewer.params.PatientID, (Number(CurrPage) - 1), 15, docMode).done(function (response) {
                    if (response.status != false) {
                        if (response.DocumentCount > 0) {

                        }
                        else {
                            $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging").css("display", "none");
                            $("#" + Patient_Document.params["PanelID"] + " #divReviewedPaging").css("display", "none");
                        }
                        if (response.DocumentLoad_JSON != "") {
                            Patient_Document.DocumentGridLoad(response, (Number(CurrPage) - 1), 15);
                        }

                        if (response.ReviewedDocumentLoad_JSON != "") {
                            Patient_Document.ReviewedDocumentGridLoad(response, (Number(CurrPage) - 1), 15);
                        }
                        var activeRow = $($(table).find('tr')[15]).addClass('active');
                        PatDocID = $(activeRow).attr('documentid');
                        clearInterval(Document_Viewer.interval);
                        Document_Viewer.params['PatDocID'] = PatDocID;
                        Document_Viewer.DocumentFill();

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });

                //if (CurrPage != 0) {
                //    $(table).DataTable().page('previous').draw(false);
                //    var activeRow = $($(table).find('tr')[$(table).DataTable().page.info().length]).addClass('active');
                //    PatDocID = $(activeRow).prev().attr('documentid');
                //    clearInterval(Document_Viewer.interval);
                //    Document_Viewer.params['PatDocID'] = PatDocID;
                //    Document_Viewer.DocumentFill();
                //}
                //else
                //    utility.myConfirm('9', function () { }, function () { }, '9');
            }
        }

    },
    nextDocument: function (IsFromConfirmPass, PassDocId) {
        if (IsFromConfirmPass == "true") {
            $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table').find("tr[documentid=" + PassDocId + "]").addClass("active");
        }
        $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer #lnkPreviousDocument').removeClass("disabled");
        var PatDocID = '';
        //var table = $('#' + Document_Viewer.params.PanelID + ' #dgvPatientDocument');
        if (Document_Viewer.params.ParentCtrl == "advancePaymentDetail") {
            var table = $('#' + Document_Viewer.params.PanelID + ' #dgvAdvancePaymentDocument');
        } else if (Document_Viewer.params.ParentCtrl == "clinicalTabLabOrder") {
            $('#dgvLabResult_wrapper #menuViewAttachment li').each(function () {
                if ($(this).find('a').attr('class') == "active") {
                    $(this).find('a').removeAttr("class");
                    $(this).next().find('a').attr('class', 'active');
                    PatDocID = $(this).next().find('a').attr("id");
                    return false;
                }
            });
        } else if (Document_Viewer.params.ParentCtrl == "mstrTabDashBoard") {
            var table = $('#' + Document_Viewer.params.PanelID + ' #dgvPatientDocumentGrid_wrapper').find('table');
            var pages = $(table).DataTable().page.info().pages;
            //var CurrPage = $(table).DataTable().page.info().page;
            var CurrPage = $(table).parent().parent().parent().find('#pagerParent li.active a').text();
            var activeRow = $(table).find('tr.active');
            if (DashBoard.PendingDocList.length > 0 && Document_Viewer.params.PatDocID && Document_Viewer.params["PatientID"]) {
                var currentDoc = $(DashBoard.PendingDocList).filter(function () {
                    return parseInt(this.PatDocId) === parseInt(Document_Viewer.params.PatDocID);
                });
                if (currentDoc && currentDoc.length > 0) {
                    var nextDoc = DashBoard.PendingDocList[($.inArray(currentDoc[0], DashBoard.PendingDocList) + 1)];
                    if (nextDoc) {
                        Document_Viewer.params["PatientID"] = parseInt(nextDoc.PatientId);
                        PatDocID = parseInt(nextDoc.PatDocId);
                    }
                    else {
                        PatDocID = $(activeRow).next().attr('documentid');
                        Document_Viewer.params["PatientID"] = $(activeRow).next().attr('Patientid');
                    }
                }
                else {
                    PatDocID = $(activeRow).next().attr('documentid');
                    Document_Viewer.params["PatientID"] = $(activeRow).next().attr('Patientid');
                }
            }
            else {
                PatDocID = $(activeRow).next().attr('documentid');
                Document_Viewer.params["PatientID"] = $(activeRow).next().attr('Patientid');
            }
            var pages = $(table).DataTable().page.info().pages;
            var CurrPage = $(table).DataTable().page.info().page;
            var CurrPage = $(table).parent().parent().parent().find('#pagerParent li.active a').text();
            PatDocID = $(activeRow).next().attr('documentid');
            Document_Viewer.params["PatientID"] = $(activeRow).next().attr('Patientid');
            $(activeRow).next().addClass('active');
            $(activeRow).removeClass('active');
        }
        else {
            //var table = $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table');
        }
        if (Document_Viewer.params.ParentCtrl != "clinicalTabLabOrder" && Document_Viewer.params.ParentCtrl != "mstrTabDashBoard") {

            var lNormalFlows = true;

            if (Document_Viewer.params.ParentCtrl == "batchTabDocuments" || Document_Viewer.params.ParentCtrl == "Patient_Document" ||  Document_Viewer.params.TabID == "patTabDocuments" || Document_Viewer.params.TabID =="batchTabDocuments") {
                Document_Viewer.KendoGrdNextPrevs('next');
                PatDocID = Document_Viewer.NxtPrvPatDocID;
                Document_Viewer.params["PatientID"] = Document_Viewer.NxtPrvPatientID;
                lNormalFlows = false;
            }
            if (lNormalFlows) {
                var activeRow = $(table).find('tr.active');
                var pages = $(table).DataTable().page.info().pages;
                var CurrPage = $(table).DataTable().page.info().page;
                var CurrPage = $(table).parent().parent().parent().find('#pagerParent li.active a').text();
                PatDocID = $(activeRow).next().attr('documentid');
                Document_Viewer.params["PatientID"] = $(activeRow).next().attr('Patientid');
                $(activeRow).next().addClass('active');
                $(activeRow).removeClass('active');
            }
        }

        if (PatDocID != undefined) {
            clearInterval(Document_Viewer.interval);
            Document_Viewer.params['PatDocID'] = PatDocID;
            Patient_Document.CheckforPrivacy(Document_Viewer.params.PatientID, PatDocID).done(function (response) {
                if (response.status == true) {
                    if (response.DocPasswordInfoCount > 0) {
                        Document_Viewer.PrivateDoc = response.DocPasswordInfo[0].Password;
                        ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                    }
                }
                if ((Document_Viewer.PrivateDoc == "" && ShowPasswordAlert != "True") || (Document_Viewer.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                    Document_Viewer.DocumentFill();
                } else {
                    if ($('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').length < 1) {
                        Document_Viewer.VerifyPassword(Document_Viewer.params.PatientID, PatDocID, event, null, null, true, true);
                    } else {
                        $('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').modal("show");
                        $("#DivConfirmPasswordViewer #TxtDocPassword").val("");
                        $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentScan").attr("onclick", "Patient_Document.DocPasswordMatch('" + Document_Viewer.params.PatientID + "', '" + PatDocID + "', '" + null + "', '" + null + "', '" + true + "');");
                        $("#modal-from-dom-DocumentConfirmPasswordViewer").find(".close").attr("onclick", "Document_Viewer.cancelConfirmPasswordDialog();Document_Viewer.nextDocument('true', " + PatDocID + ");");
                        $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentSkip").attr("onclick", "Document_Viewer.cancelConfirmPasswordDialog();Document_Viewer.nextDocument('true', " + PatDocID + ");");
                        Document_Viewer.params.DocPrivate = "1";
                    }

                }
            });
        }
        else {
            var isLastPage = $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('#divShowingEntries').text().split(' ')[3] == $('#' + Document_Viewer.params.PanelID + ' ' + $('#' + Document_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('#divShowingEntries').text().split(' ')[5];
            var index = $.map($(table).find('tr'), function (obj, index) {
                if (obj == activeRow[0]) {
                    return index;
                }
            })[0];

            if (isLastPage && index == ($(table).find('tr').length - 1)) {
                utility.myConfirm('No further documents available inside the folder', function () { }, function () { }, 'Document Alert', "OK", "", true);

            }
            else {

                //if ((pages - 1) != CurrPage) {
                //$(table).DataTable().page('next').draw(false);
                var self = $("#" + Patient_Document.params["PanelID"] + ' #mainContainer');
                var myJSON = self.getMyJSON();
                var docMode = null;
                //if ($('#pnlPatientDocument_Result .tabs ul.nav-tabs li.active a').text().toLowerCase() == "pending")
                if ($('#pnlPatientDocument_Result .tabs-custom li.active a').text().toLowerCase() == 'pending')
                    docMode = '0';
                else
                    docMode = '1';

                Patient_Document.SearchDocument(myJSON, Document_Viewer.params.PatientID, (Number(CurrPage) + 1), 15, docMode).done(function (response) {
                    if (response.status != false) {
                        if (response.DocumentCount > 0) {

                        }
                        else {
                            $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging").css("display", "none");
                            $("#" + Patient_Document.params["PanelID"] + " #divReviewedPaging").css("display", "none");
                        }
                        if (response.DocumentLoad_JSON != "") {
                            response.DocumentLoad_JSON = JSON.parse(response.DocumentLoad_JSON);
                            Patient_Document.DocumentGridLoad(response, (Number(CurrPage) + 1), 15);
                        }

                        if (response.ReviewedDocumentLoad_JSON != "") {
                            response.ReviewedDocumentLoad_JSON = JSON.parse(response.ReviewedDocumentLoad_JSON);
                            Patient_Document.ReviewedDocumentGridLoad(response, (Number(CurrPage) + 1), 15);
                        }
                        var activeRow = $($(table).find('tr')[1]).addClass('active');
                        PatDocID = $(activeRow).attr('documentid');
                        clearInterval(Document_Viewer.interval);
                        Document_Viewer.params['PatDocID'] = PatDocID;
                        Patient_Document.CheckforPrivacy(Document_Viewer.params.PatientID, PatDocID).done(function (response) {
                            if (response.status == true) {
                                if (response.DocPasswordInfoCount > 0) {
                                    Document_Viewer.PrivateDoc = response.DocPasswordInfo[0].Password;
                                    ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                                }
                            }
                            if ((Document_Viewer.PrivateDoc == "" && ShowPasswordAlert != "True") || (Document_Viewer.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                                Document_Viewer.DocumentFill();
                            } else {
                                if ($('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').length < 1) {
                                    Document_Viewer.VerifyPassword(Document_Viewer.params.PatientID, PatDocID, event, IsFromDashboard, FileType, "true", true);
                                } else {
                                    $('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').modal("show");
                                    $("#DivConfirmPasswordViewer #TxtDocPassword").val("");
                                    $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentScan").attr("onclick", "Patient_Document.DocPasswordMatch('" + Document_Viewer.params.PatientID + "', '" + PatDocID + "', '" + IsFromDashboard + "', '" + FileType + "', '" + true + "');");
                                    $("#modal-from-dom-DocumentConfirmPasswordViewer").find(".close").attr("onclick", "Document_Viewer.cancelConfirmPasswordDialog();Document_Viewer.nextDocument('true', " + PatDocID + ");");
                                    $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentSkip").attr("onclick", "Document_Viewer.cancelConfirmPasswordDialog();Document_Viewer.nextDocument('true', " + PatDocID + ");");
                                    Document_Viewer.params.DocPrivate = "1";
                                }

                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
                //Patient_Document.SelectedPageClick(CurrPage, this, 209, 15, 'divPendingPaging');
                //var activeRow = $($(table).find('tr')[1]).addClass('active');
                //PatDocID = $(activeRow).next().attr('documentid');
                //clearInterval(Document_Viewer.interval);
                //Document_Viewer.params['PatDocID'] = PatDocID;
                //Document_Viewer.DocumentFill();
                // }
                //else
                //    utility.myConfirm('9', function () { }, function () { }, '9');
            }
        }
    },

    PrintDocument: function () {
        var DocType = Document_Viewer.params.DocumentType.toLowerCase();
        var canvasObj = document.getElementById("pnlDocumentViewer").querySelector('#canvas');
        var canvasContext = canvasObj.getContext("2d");
        $('#pnlDocumentViewer #printHelper').append('<img src="' + canvasObj.toDataURL() + '"/>');
        var contents = "";
        if (DocType == 'image') {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
                var strcontents = $('#pnlDocumentViewer #printHelper').html();
                var uniqueName = new Date();
                var windowName = 'Print' + uniqueName.getTime();
                var printWindow = window.open('', 'printwindow');
                printWindow.document.write('<html><head><title>' + ReportName + ' | Print</title>');
                printWindow.document.write('</head><body>');
                //Append the external CSS file.
                printWindow.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
                //Append the DIV contents.
                printWindow.document.write(strcontents);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                printWindow.focus();
                printWindow.print();
                printWindow.close();
                setTimeout(function () {
                }, 200);
            }
            else {
                contents = $('#pnlDocumentViewer #printHelper').html();
                Document_Viewer.getPrint(contents);
            }
        }
        else if (DocType == 'html') {
            contents = $("#pnlDocumentViewer #OpenHTMLDocument").contents().find('html').html();
            Document_Viewer.getPrint(contents);
        }
        $('#pnlDocumentViewer #printHelper').empty();
    },
    getPrint: function (contents) {
        var ReportName = "Patient Document";
        var frame1 = $('<iframe />');
        frame1[0].name = ReportName.toLowerCase().trim();
        frame1.attr("scrolling", "no");
        frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
        $("body").append(frame1);
        var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
        frameDoc.document.open();
        //Create a new HTML document.
        frameDoc.document.write('<html><head><title>' + ReportName + ' | Print</title>');
        frameDoc.document.write('</head><body>');
        //Append the external CSS file.
        frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
        //Append the DIV contents.
        frameDoc.document.write(contents);
        frameDoc.document.write('</body></html>');
        frameDoc.document.close();
        setTimeout(function () {
            window.frames[ReportName.toLowerCase().trim()].focus();
            window.frames[ReportName.toLowerCase().trim()].print();
            frame1.remove();

        }, 200);
    },
    UnLoadTab: function () {
        clearInterval(Document_Viewer.interval);

        if (Patient_Document.SignalRHub) {
            $.connection.hub.stop();
        }
        if (Document_Viewer.params != null && Document_Viewer.params.ParentCtrl != null) {


            if (Document_Viewer.params["ParentCtrl"] == "Clinical_RadiologyOrder" || Document_Viewer.params["ParentCtrl"] == "ClinicalRadiologyResultDetail" ||
                Document_Viewer.params["ParentCtrl"] == "clinicalTabLabOrder" || Document_Viewer.params["ParentCtrl"] == "Clinical_LabOrder" || Document_Viewer.params["ParentCtrl"] == "ClinicalLabResultDetail" || Document_Viewer.params["ParentCtrl"] == "Patient_Case_Document") {

                UnloadActionPan(Document_Viewer.params["ParentCtrl"], "Document_Viewer", null, Document_Viewer.params["ParentCtrlPanelID"]);
            }
            else {

                if (Document_Viewer.params.ParentCtrl == 'mstrTabDashBoard' || Document_Viewer.params.ParentCtrl == 'clinicalTabPatientEducation' || Document_Viewer.params.ParentCtrl == 'Clinical_PatientEducation' || Document_Viewer.params.ParentCtrl == "OrderSet_PatientEducation" || Document_Viewer.params.ParentCtrl == 'patTabDemographic' || Document_Viewer.params.ParentCtrl == 'patTabDocuments'
                    || Document_Viewer.params.ParentCtrl == "Patient_Referrals_Incoming_Detail" || Document_Viewer.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Document_Viewer.params.ParentCtrl == 'Patient_MessageEdit'
                    || Document_Viewer.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail" || Document_Viewer.params.ParentCtrl == "advancePaymentDetail" || Document_Viewer.params.ParentCtrl == 'demographicDetail' || Document_Viewer.params.ParentCtrl == 'EncounterChargeCapture' || Document_Viewer.params.ParentCtrl == "Clinical_OrderSetDetails" || Document_Viewer.params.ParentCtrl == "Patient_Information_Submission" || Document_Viewer.params["ParentCtrl"] == "Bill_Insurance_Payment_Detail") {
                    UnloadActionPan(Document_Viewer.params.ParentCtrl, "Document_Viewer");

                    // Refresh DashBoard Documnet Grid
                    if (Document_Viewer.params.ParentCtrl == 'mstrTabDashBoard') {
                        var pageNo = $("#pnlDashboard #divPendingPagingGrid li.active a").text();
                        DashBoard.DashBoardDocumentSearch(pageNo, 15);
                    }


                    // update the Patient document grid at Dashbif(Document_Viewer.params.ParentCtrl == "")oard
                    if (Document_Viewer.params["IsDashboardDocUpdated"] && Document_Viewer.params["IsDashboardDocUpdated"] == "1" && Document_Viewer.params.ParentCtrl == "mstrTabDashBoard")
                    { DashBoard.DashBoardDocumentSearch(null, null, null); }                   
                }
                else if (Document_Viewer.params["PatientDetail"] == "1") {
                    if (Document_Viewer.params["ParentCtrl"] == "Patient_Document") {
                        UnloadActionPan(Document_Viewer.params["ParentCtrl"], "Document_Viewer", null, Document_Viewer.params["ParentCtrlPanelID"]);
                    }
                    else {
                        UnloadActionPan(Document_Viewer.params.ParentCtrl, "Document_Viewer");
                    }
                } else {
                    UnloadActionPan(Document_Viewer.params.ParentCtrl, "Document_Viewer", null, "pnlClinicalProgressNote #pnlClinicalPatientEducation");
                }

                if (Document_Viewer.params.ParentCtrl == "advancePaymentDetail")
                    advancePaymentDetail.DocumentSearch();
                //UnloadActionPan(Document_Viewer.params.ParentCtrl);
            }
        }
        else {
            ////Abid Ali modified For Radiology attachement
            //if (Document_Viewer.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
            //    UnloadActionPan(Document_Viewer.params["ParentCtrl"], "Document_Viewer", null, Document_Viewer.params["ParentCtrlPanelID"]);
            //}
            //else {
            UnloadActionPan(null, 'Document_Viewer');
            // Patient_Document.LoadFolders(true);           

            //}
        }
        if (($('.modal-backdrop').length > 0) && Document_Viewer.params.DocPrivate == "1") {
            $('.modal-backdrop').remove();
        }
        if ($('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkNextDocument,#lnkPreviousDocument').hasClass("disabled")
            && $("#" + Patient_Document.params["PanelID"] + " #DivDocumentView").is(":visible") == false
            )
        { $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkNextDocument,#lnkPreviousDocument').removeClass("disabled"); }
        if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {
            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).children("ul").children("li").find("#" + Document_Viewer.params.PatDocID).each(function (k, v) {

                if (!$(this).find("a").hasClass("jstree-clicked")) {
                    $(this).find("a").addClass("jstree-clicked");

                }
            });
        }
    },
    LoadPatientCase: function (PatientId) {
        CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
            var Ctrl = $('#' + Document_Viewer.params.PanelID + " input#txtCaseNumber");
            var hfCtrl = $("#" + Document_Import.params.PanelID + " #hfCaseId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", PatientCase, null, hfCtrl);
        });
    },
    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + Document_Viewer.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["PatientId"] = $("#" + Document_Viewer.params["PanelID"] + " #hfPatientId").val();
        params["RefCtrl"] = "txtCaseNumber";
        params["ParentCtrl"] = "Document_Viewer";
        LoadActionPan('Patient_Case_Detail', params);
    },
    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = Document_Viewer.params["TabID"];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        params["patientID"] = Document_Viewer.params.PatientID;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Document_Viewer";
        LoadActionPan('Patient_Case', params);
    },

    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Document_Viewer';
        params["patientID"] = Document_Viewer.params.PatientID;
        LoadActionPan('Encounter_Visits', params);
        if ($('body #OpenVisits').length > 1) {
            $('body #pnlEncounter').parent().not('#actionPanDocumentViewer').children("[id=pnlEncounter]").find('#OpenVisits').attr('id', 'OpenVisits1');
            //$('body #pnlEncounter').parent().not('#actionPanDocumentViewer').find('#pnlEncounter #CloseVisits').attr('id', 'CloseVisits1');
            $('body #pnlEncounter').parent().not('#actionPanDocumentViewer').children("[id=pnlEncounter]").find('#CloseVisits').attr('id', 'CloseVisits1');
            //$($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
            //$($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
            //Document_Viewer.bVisitFirst = false;
        }

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Document_Viewer.params.PanelID + " #txtClaimNumber").val(ClaimNumber);
        //$("#" + Document_Viewer.params.PanelID + " #hfPatientId").val(PatientId);
        //$("#" + Document_Viewer.params.PanelID + " #txtPatientName").val(AccountNumber + ' - ' + PatientName);
        $("#" + Document_Viewer.params.PanelID + " #hfVisitId").val(VisitId);
        if ($("#" + Document_Viewer.params.PanelID + " #lnkClaimNumberEdit").css("display") == "none") {
            $("#" + Document_Viewer.params.PanelID + " #lnkClaimNumberEdit").css("display", "inline");
            $("#" + Document_Viewer.params.PanelID + " #lblClaimNumber").css("display", "none");
        }
        //Document_Viewer.LoadPatientCase(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Document_Viewer.params.PanelID + " #txtClaimNumber"), ClaimNumber, $("#" + Document_Viewer.params.PanelID + " #hfVisitId"), VisitId, "ClaimNumber");
        Encounter_Visits.UnLoad();
    },
    BindClaimNumber: function () {
        var Ctrl = $("#" + Document_Viewer.params.PanelID + " #txtClaimNumber");
        var hfId = $("#" + Document_Viewer.params.PanelID + " #hfVisitId");
        var func = function () { return Document_Viewer.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfId);
    },
    LoadClaimNumers: function (claimNumber, patientID) {
        var data = "ClaimNumber=" + claimNumber + "&PatientID=" + patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },

    GetClaimNumberArray: function (ClaimNumber) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Document_Viewer.LoadClaimNumers(ClaimNumber, Document_Viewer.params.PatientID).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                }
            }
            dfd.resolve(AllClaimsVisits);
        });
        return dfd.promise();
    },

    LoadImagesData: function (base64, filetype) {
        var byteCharacters = atob(base64);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var objDef2 = $.Deferred();
        var zip = new JSZip();
        var filesData = "";
        zip.loadAsync(byteArray).then(function (response) {
            Object.keys(zip.files).forEach(function (filename) {
                zip.files[filename].async('base64').then(function (fileData) {
                    filesData = fileData;
                    objDef2.resolve("ok")
                })
            })
        });
        objDef2.then(function () {
            Document_Viewer.MergeImageInViewr(filetype, filesData);
        });
    },

    MergeImageInViewr: function (FileType, filesData) {
        try {
            $('#pnlDocumentViewer #OpenDocumentIF').hide();
            $('#pnlDocumentViewer #OpenHTMLDocument').hide();
            $('#pnlDocumentViewer #canvasContainer').show();
            $('#pnlDocumentViewer #imagesControls').show();
            $('#pnlDocumentViewer #extraContorls').show();
            Document_Viewer.params['DocumentType'] = "IMAGE";
            var imageObj = new Image();
            //for IE
            //  imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;

            imageObj.src = "data:" + FileType + ";base64," + filesData;
            var canvas = document.getElementById("canvas");
            setTimeout(function () {
                canvas.width = imageObj.width;
                canvas.height = imageObj.height;
            }, 1000);

            var context = canvas.getContext("2d");


            function draw() {
                var scale = 1;
                var originx = 0;
                var originy = 0;
                context.save();
                context.setTransform(1, 0, 0, 1, 0, 0);
                context.clearRect(0, 0, canvas.width, canvas.height);
                context.restore();
                context.drawImage(imageObj, 0, 0);
            }

            setTimeout(function () { draw(); }, 1000);
            Document_Viewer.interval = setInterval(function () { draw(); }, 1000);
            Document_Viewer.params.t1 = performance.now();
            console.log("Call to compression took " + (Document_Viewer.params.t1 - Document_Viewer.params.t0) + " milliseconds.")
        }
        catch (ex) {
            utility.DisplayMessages(ex, 2);
            console.log(ex);
        }
    },
    LoadPatientVisitDOS: function (patientId) {

        Document_Viewer.LoadVisitDOS(patientId).done(function (responce) {
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").empty();
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divddlDOS #ddlDOS").append($("<option/>", {
                value: "",
                text: "-Select-"
            }));
            if (responce.TotalDOS > 0) {

                $.each(responce.PatientDOS, function (k, item) {
                    $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #divddlDOS #ddlDOS").append($("<option/>", {
                        value: utility.RemoveTimeFromDate(null, item.DOSFrom),
                        text: utility.RemoveTimeFromDate(null, item.DOSFrom)
                    }));
                });

            }
        });


    },
    LoadVisitDOS: function (PatientID) {

        var data = "PatientID=" + PatientID;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "GET_VISIT_DOS");
    },
    SelectedDOSControl: function (obj) {
        if ($(obj).val() == "Calendar") {
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").removeAttr("disabled");
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").attr("disabled", "disabled");
        }
        else {
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #dtpDOS").attr("disabled", "disabled");
            $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #ddlDOS").removeAttr("disabled");
        }
    },

    OpenChangePassword: function (obj) {
        if ($(obj).prop("checked") == false) {
            if ($('body').find('#modal-from-dom-DocumentChangePassword').length < 1) {
                Document_Viewer.myConfirm();
            } else {
                $('body').find('#modal-from-dom-DocumentChangePassword').modal("show");
                $('#DivChangePassword #TxtDocPassword').val("");
                $('#DivChangePassword #DivShareAccess #TxtDocNewPassword').val("");
                $('#DivChangePassword #DivShareAccess #TxtDocConNewPassword').val("");
            }
        } else {
            Document_Scan.PrivateDocument(obj, 'Document_Viewer');
        }
    },

    myConfirm: function () {
        var DivFormGroup = '<div class="form-group">';
        var DivEnd = '</div>'
        var Password = '<input type="password" name="DocPassword" id="TxtDocPassword" class="form-control">';
        var ConfirmPassword = '<input type="password" name="DocConfirmPassword" id="TxtDocConfirmPassword" class="form-control">';
        var dialogTitle = "Remove/Change Privacy";
        var Required = '<span class="required">*</span>';
        var Clearfix = '<div class="clearfix"></div>';
        var Spacer20 = '<div class="spacer20"></div>'
        var Spacer10 = '<div class="spacer10"></div>'
        var ShareAccessCaption = 'Share access of this document by selecting the user(s) below. User(s) selected will receive system generated message containing password.';

        var markUp = '';
        markUp = '<div id="modal-from-dom-DocumentChangePassword" class="modal fade">' +
                        '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                            '<div class="modal-content">'
                            + '<div class="modal-header">' + '<button type="button" onclick="Document_Viewer.cancelConfirmDialog();"  class="close" "></button>'
                                + '<h4 class="modal-title">' + dialogTitle + ' </h4>'
                            + DivEnd
                                + '<div class="modal-body bg-white" id="DivChangePassword">'
                                    + '<div class="col-xs-6"><label class="control-label">Confirm Password' + Required + '</label>' + Password + DivEnd + Clearfix
                                    + DivFormGroup
                                        + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<a href="#" id="btnShareAccess" onclick="Document_Viewer.ShowChangePassword();" class="">Change Password</a>'
                                        + DivEnd
                                    + DivEnd //+ Spacer20
                                        //+ DivFormGroup
                                        + "<div class='form-group'>"
                                            + "<div class='col-xs-12 hidden' id='DivShareAccess'>"
                                                + "<div class='col-xs-9'>"
                                                    + "</div><div class='spacer10'></div>"
                                                    + "<div class='col-xs-6'>"
                                                        + "<label class='control-label'>New Password<span class='required'>*</span></label>"
                                                        + "<input type='password' name='DocNewPassword' id='TxtDocNewPassword' class='form-control'>"
                                                    + "</div>"
                                                    + Spacer10
                                                    + "<div class='col-xs-6'>"
                                                        + "<label class='control-label'>Confirm New Password<span class='required'>*</span></label>"
                                                        + "<input type='password' name='DocNewConPassword' id='TxtDocConNewPassword' class='form-control'>"
                                                    + "</div>"
                                        + "</div></div>"
                                         + '<div class="form-group"><div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="Document_Viewer.ChangeOrRemovePassword();">Save</button>'
                                        + DivEnd + DivEnd
                                 + DivEnd
                        + DivEnd
                    + DivEnd
                + '</div><div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });

        Document_Viewer.params.DocPrivate = "1";

        $('#DivChangePassword #TxtDocPassword').val("");
        $('#DivChangePassword #DivShareAccess #TxtDocNewPassword').val("");
        $('#DivChangePassword #DivShareAccess #TxtDocConNewPassword').val("");
        //if (ParentFlow == "Document_Import") {
        //    Document_Import.params.DocPrivate = "1";
        //} else {
        //    Document_Scan.params.DocPrivate = "1";
        //}
    },

    cancelConfirmDialog: function (Changed) {
        $("#modal-from-dom-DocumentChangePassword").modal('hide');
        if (Changed != "1") {
            $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("checked", true);
        }
    },

    ShowChangePassword: function () {
        if ($('#DivChangePassword #DivShareAccess').hasClass('hidden')) {
            $('#DivChangePassword #DivShareAccess').removeClass('hidden');
        } else {
            $('#DivChangePassword #DivShareAccess').addClass('hidden');
        }
    },

    ChangeOrRemovePassword: function () {
        if ($('#DivChangePassword #TxtDocPassword').val() == "") {
            utility.DisplayMessages("Please input your old password.", 2);
        } else {
            var OldPassword = $('#DivChangePassword #TxtDocPassword').val();
            var NewPassword = $('#DivChangePassword #DivShareAccess #TxtDocNewPassword').val();
            var ConfirmPassword = $('#DivChangePassword #DivShareAccess #TxtDocConNewPassword').val();
            if (NewPassword == ConfirmPassword) {
                if (NewPassword == "") {
                    utility.myConfirm("Are you sure you want to delete/remove password?", function () {
                        Document_Viewer.PasswordChangeOrRemove(Document_Viewer.params.PatDocID, OldPassword, NewPassword, ConfirmPassword).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Document_Viewer.cancelConfirmDialog("1");
                                if (NewPassword == "") {
                                    $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("checked", false);
                                }
                            } else {
                                utility.DisplayMessages(response.Message, 2);
                            }
                        });
                    }, function () { }, "Remove Password");
                } else {
                    Document_Viewer.PasswordChangeOrRemove(Document_Viewer.params.PatDocID, OldPassword, NewPassword, ConfirmPassword).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Document_Viewer.cancelConfirmDialog();
                            if (NewPassword == "") {
                                $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer #ChkPrivate").prop("checked", false);
                            }
                        } else {
                            utility.DisplayMessages(response.Message, 2);
                        }
                    });
                }
            } else {
                utility.DisplayMessages("New Password and Confirm new password doesnot match.", 2);
            }

        }
    },

    PasswordChangeOrRemove: function (PatDocID, OldPassword, NewPassword, ConfirmPassword) {
        var data = "OldPassword=" + OldPassword + "&NewPassword=" + NewPassword + "&ConfirmPassword=" + ConfirmPassword + "&PatDocID=" + PatDocID;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "CHANGE_OR_REMOVE_PASSWORD");
    },

    SaveDocPassword: function () {
        var DocPassword = {};
        if ($("#popDiv #TxtDocPassword").val() != "") {
            var PasswordMatch = $("#popDiv #TxtDocPassword").val() == $("#popDiv #TxtDocConfirmPassword").val() ? true : false;
            if (PasswordMatch) {
                var Selectedvalues = $("#popDiv").find("#DivShareAccess #ddlUsers").val();
                Selectedvalues = $.isArray(Selectedvalues) ? Selectedvalues.join() : Selectedvalues;
                DocPassword["SetPassword"] = $("#popDiv #TxtDocPassword").val();
                DocPassword["ConfirmPassword"] = $("#popDiv #TxtDocConfirmPassword").val();
                DocPassword["UserId"] = Selectedvalues;

                Document_Viewer.PasswordJSON = JSON.stringify(DocPassword);

                var self = $("#" + Document_Viewer.params.PanelID + " #frmDocumentViewer");
                var myJSON = self.getMyJSON();

                Document_Viewer.SavePassword(Document_Viewer.PasswordJSON, Document_Viewer.params.PatDocID, Document_Viewer.params.PatientID, myJSON).done(function () {
                    Document_Scan.cancelConfirmDialog();
                });
            } else {
                utility.DisplayMessages("Passwords entered do not match", 3);
            }
        } else {
            utility.DisplayMessages("Please enter Password.", 3);
        }

    },

    SavePassword: function (PasswordJSON, PatDocId, PatientID, myJSON) {
        var data = "PasswordJSON=" + PasswordJSON + "&PatDocID=" + PatDocId + "&PatientID=" + PatientID + "&FieldsJSON=" + myJSON;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "VIEWER_SAVE_PASSWORD");
    },

    VerifyPassword: function (PatientID, PatDocID, event, IsFromDashboard, FileType, IsFromViewer, NextDocument) {
        var DivFormGroup = '<div class="form-group">';
        var DivEnd = '</div>'
        var Password = '<input type="password" name="DocPassword" id="TxtDocPassword" class="form-control" autofocus>';
        //var ConfirmPassword = '<input type="password" name="DocConfirmPassword" id="TxtDocConfirmPassword" class="form-control">';
        var dialogTitle = "Confirm Password";
        var Required = '<span class="required">*</span>';
        var Clearfix = '<div class="clearfix"></div>';
        //var Spacer20 = '<div class="spacer20"></div>';
        var ShareAccessCaption = 'Share access of this document by selecting the user(s) below. User(s) selected will receive system generated message containing password.';
        var DocMatchFunction = "Patient_Document.DocPasswordMatch('" + PatientID + "', '" + PatDocID + "', '" + IsFromDashboard + "', '" + FileType + "', '" + IsFromViewer + "');";
        if (NextDocument == true) {
            var SkipFunction = "Document_Viewer.nextDocument('true', " + PatDocID + ");";
        } else {
            var SkipFunction = "Document_Viewer.previousDocument('true', " + PatDocID + ");";
        }


        var markUp = '';
        markUp = '<div id="modal-from-dom-DocumentConfirmPasswordViewer" class="modal fade">' +
                        '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                            '<div class="modal-content">'
                            + '<div class="modal-header">' + '<button type="button" onclick="Document_Viewer.cancelConfirmPasswordDialog();' + SkipFunction + '"  class="close" "></button>'
                                + '<h4 class="modal-title">' + dialogTitle + ' </h4>'
                            + DivEnd
                                + '<div class="modal-body bg-white" id="DivConfirmPasswordViewer">'
                                    + '<div class="col-xs-6"><label class="control-label">Confirm Password' + Required + '</label>' + Password + DivEnd + Clearfix
                                    + DivFormGroup
                                        + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="' + DocMatchFunction + '">Ok</button>'
                                            + '<button id="btnDocumentSkip" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="Document_Viewer.cancelConfirmPasswordDialog();' + SkipFunction + '">Skip</button>'
                                        + DivEnd
                                    + DivEnd
                                 + DivEnd
                        + DivEnd
                    + DivEnd
                + '</div><div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });

        $("#DivConfirmPasswordViewer #TxtDocPassword").val("");
        Document_Viewer.params.DocPrivate = "1";
    },



    DocumentDelete: function (IsFromAttach, ChildPatientDocId) {
        var strMessage = "";
        DocumentId = Document_Viewer.params.PatDocID;
        AppPrivileges.GetFormPrivileges("Documents", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = DocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var $def = $.Deferred();
                        if (IsFromAttach == true) {
                            Patient_Document.DetachDocumentToNote_DBCall(selectedValue).done(function (result) {
                                if (result.status != false) {
                                    $def.resolve("ok");
                                }
                                else {
                                    $def.resolve(result.Message);
                                }
                            });
                        }
                        else {
                            $def.resolve("ok");
                        }

                        $def.then(function (res_message) {
                            if (res_message == "ok") {
                                Document_Viewer.DeleteDocument(selectedValue, ChildPatientDocId).done(function (response) {
                                    if (response.status != false) {
                                       // PMS - 4617
                                        if (response.Message == "Document(s) are Associted with Medical Document." || response.Message == "Document(s) are linked with some other documents. Please delete its link and then try again.")
                                            utility.DisplayMessages(response.Message, 3);
                                        else
                                            utility.DisplayMessages(response.Message, 1);

                                        Document_AssignedTo.PendingCountSearchDoc();
                                        if (Document_Viewer.params.FolderID) {
                                            Patient_Document.LoadFolders(true, true);
                                            Document_Viewer.UnLoadTab();
                                            Patient_Document.RefreshLandScreenDocumentViewer();
                                        }
                                        else {
                                            if (!$('#lnkNextDocument').hasClass('disabled') && !$('#lnkPreviousDocument').hasClass('disabled')) {
                                                Document_Viewer.nextDocument();                                       
                                            }
                                            else if ($('#lnkPreviousDocument').hasClass('disabled') && !$('#lnkNextDocument').hasClass('disabled')) {
                                                Document_Viewer.nextDocument();
                                            }
                                            else if ($('#lnkNextDocument').hasClass('disabled')) {
                                                Document_Viewer.UnLoadTab();
                                            }
                                            else if ($('#lnkNextDocument').hasClass('disabled') && $('#lnkNextDocument').hasClass('disabled')) {
                                                Document_Viewer.UnLoadTab();
                                            }
                                            Patient_Document.LoadFolders(true, true);
                                            //  $("#" + Document_Viewer.params.PanelID + ' #dgvPatientDocument').find('tr.active').remove();
                                        }
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else {
                                utility.DisplayMessages(res_message, 3);
                            }
                        });
                    }
                }, function () { }, '1');
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteDocument: function (DocumentID, ChildPatientDocId) {
        var data = "DocumentID=" + DocumentID + "&ChildPatientDocId=" + ChildPatientDocId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DELETE_PATIENT_DOCUMENT");
    },
    cancelConfirmPasswordDialog: function () {
        $("#modal-from-dom-DocumentConfirmPasswordViewer").modal('hide');
        //$('body').find('.modal-backdrop').removeClass('modal-backdrop');
    },

    FillTagName: function (hfTagId, txtTagName) {
        $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtTagName").val(txtTagName);
        $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #hfDocumentTagId").val(hfTagId);
        Patient_DocumentTag.UnLoad();
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtTagName"), txtTagName, $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #hfDocumentTagId"), hfTagId);
        //   $("#" + Document_Viewer.params["PanelID"] + " #frmDocumentViewer #txtTagName").focus();
    },
    AddTags: function () {
        var params = [];
        params["ParentCtrl"] = "Document_Viewer"
        params["TabId"] = "patTabDocuments";
        LoadActionPan('Patient_DocumentTag', params);
    },
    LoadDocumentNote: function () {
        var params = [];
        params["ParentCtrl"] = "Document_Viewer";
        params["PatDocId"] = Document_Viewer.params.PatDocID;
        LoadActionPan("Patient_Document_Note", params);
    },

    sendAsFax: function () {
        Patient_Document.FillDocumentsForFax(null, Document_Viewer.params.PatientID, Document_Viewer.params.PatDocID).done(function (response) {
            if (response.status) {
                var params = [];
                params["PDFBase64"] = response.MergedContent;
                params["ParentCtrl"] = "Document_Viewer";
                params["PatientId"] = Document_Viewer.params.PatientID;
                LoadActionPan("Batch_FaxSend", params);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ValidateDocumentSearchScreen: function () {
        var CtrlId = "#" + Patient_Document.params.PanelID + " #frmPatientDocument";
        var isok = false;
        $(CtrlId).find('select,[type=text],[type=hidden]').each(function () {
            if ($(this).hasClass('ValidateCriteria') == true && $(this).val() != "") {
                if (this.tagName.toLowerCase() == "select" && ($(this).val().toLowerCase() == "all")) {
                    // ignore.
                }
                else {
                    isok = true;

                }
            }

        });
        return isok;
    },
    KendoGrdNextPrevs: function (mode) {
        var selectedrow = '';
        var lGridName = '';

        if ($("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #liPending").hasClass("active")) {
            lGridName = '#' + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument;
            selectedrow = $(lGridName).find("tr.k-state-selected");

        } else {
            lGridName = '#' + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument;
            selectedrow = $(lGridName).find("tr.k-state-selected");

        }
        var clas = Document_Viewer.NxtPrvLinkDocGrdName != "MainGrid" ? "tr.k-detail-row" : "tr.k-master-row";
        var rows = selectedrow.siblings(clas);
        var nextPrevsRow; 
        var nextRowuid = null; 
        var selectedrow = $(lGridName).find("tr.k-state-selected");
        var selectedIdx = null;

        if (Document_Viewer.NxtPrvLinkDocGrdName != "MainGrid") {
            lGridName = lGridName + " #" + Document_Viewer.NxtPrvLinkDocGrdName;
          
        }

        var dataSource = $(lGridName).data("kendoGrid").dataSource.data();
        if (mode == "next") {
            for (var i = 0; i < dataSource.length; i++) {
             
                if ((dataSource[i].uid == $(selectedrow).attr('data-uid'))) {
                    nextRowuid = i + 1;
                }              
            }

        } else {
            for (var i = 0; i < dataSource.length; i++) {
                if (dataSource[i].uid == $(selectedrow).attr('data-uid')) {
                    nextRowuid = i - 1;
                }
            }
        }     
        if ((mode == "next") && ($(selectedrow).attr('data-uid')==undefined))
        {
            Document_Viewer.SectionRowIndex= Document_Viewer.SectionRowIndex + 1
            nextRowuid = Document_Viewer.SectionRowIndex;
        }
      
        $(lGridName).find("tr.k-state-selected").removeClass('k-state-selected');
        
        if (nextRowuid != 'null') {
            nextPrevsRow = dataSource[nextRowuid];
        }

        if (nextPrevsRow != null) {
            $(lGridName + " tbody tr[data-uid=" + nextPrevsRow.uid + "]").addClass('k-state-selected');

            Document_Viewer.NxtPrvPatDocID = nextPrevsRow.PatDocId;
            Document_Viewer.NxtPrvPatientID = nextPrevsRow.PatientId;
        }

    },

    NextPrevsButtonEnableDisabled: function () {
        var lGridName;
        if ($("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #liPending").hasClass("active")) {
            lGridName = '#' + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument;
            selectedrow = $(lGridName).find("tr.k-state-selected");

        } else {
            lGridName = '#' + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument;
            selectedrow = $(lGridName).find("tr.k-state-selected");
        }
        if (Document_Viewer.NxtPrvLinkDocGrdName != "MainGrid") {

            lGridName = lGridName + " #" + Document_Viewer.NxtPrvLinkDocGrdName;

            $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer  #CtrDocumentViewer").addClass('disableAll');
            $('#' + Document_Viewer.params["PanelID"] + " #frmDocumentViewer  #btnReviewDocument,#btnSave").attr("disabled", "disabled");

        }
        var nextRowselectedIdx = 0;

        var selectedRowIdx = $(lGridName).find("tr.k-state-selected").closest("tr").index();

        if (selectedRowIdx == -1) {
            nextRowselectedIdx = 0 + 1;
           
        } else {
             nextRowselectedIdx = selectedRowIdx + 1;
        }
        

        var nextPrevsRow = $(lGridName).data("kendoGrid").dataSource.data()[nextRowselectedIdx];

        //var nextPrevsRow = $(lGridName).data("kendoGrid").dataItem($(lGridName).data("kendoGrid").tbody.find("tr:eq(" + nextRowselectedIdx + ")"));
        if (nextPrevsRow == null || nextPrevsRow == undefined) {
            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkNextDocument').addClass("disabled");
        }

        if (selectedRowIdx == 0 || selectedRowIdx == -1) {

            $('#' + Document_Viewer.params.PanelID + ' #frmDocumentViewer  #lnkPreviousDocument').addClass("disabled");
        }

    }

}
