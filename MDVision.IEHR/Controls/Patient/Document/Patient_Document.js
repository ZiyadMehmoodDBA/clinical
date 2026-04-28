
Patient_Document = {
    bIsFirstLoad: true,
    params: [],
    GridPatientDocument: "PendingKGrid",
    GridRevDocument: "ReviewedKGrid",
    SelectedDocs: [],
    FileTypesArray: [],
    FaxDocsArray: [],
    AttachedDocsArray: [],
    ParentFormPanelID: '',
    PriorityDDLLoad: false,
    FolderSearchType: "0",
    PasswordTries: "1",
    PrivateDoc: "",
    interval: null,
    FavListName: "Patient_Document_Status",
    SelectedDocumentPatientId: '',
    SelectedDocumentId: '',
    SelectedDocumentFolderName: '',
    SignalRHub: null,
    AnotherUserAccessSameDocument: true,
    Load: function (parameters) {
        Patient_Document.params = parameters;
        Document_Viewer.NxtPrvLinkDocGrdName = "MainGrid";
        if (Patient_Document.params["PanelID"] != 'pnlPatientDocument')
            Patient_Document.params["PanelID"] = Patient_Document.params["PanelID"] + ' #pnlPatientDocument';
        if (Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {

            Patient_Document.params.PatientID = Patient_Document.params["PatientId"];
            $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #hfPatientId").val(Patient_Document.params["PatientId"]);

            $("#" + Patient_Document.params.PanelID + " #dialogDiv").addClass("modal-dialog");
            if (Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                Patient_Document.params.CaseId = Patient_Document.params["CaseId"];
                Patient_Document.params.CaseNo = Patient_Document.params["CaseNo"];
            }
        } else if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
            Patient_Document.params.PatientID = Clinical_FaceSheet.params.patientID;
            Patient_Document.params["PatientId"] = Clinical_FaceSheet.params.patientID;
        }
        else {
            $("#" + Patient_Document.params.PanelID + " #dialogDiv").removeClass("modal-dialog");
        }

        if (EMRUtility.getFavListStatus(Patient_Document.FavListName)) {
            Patient_Document.FolderSearchType = "1";
        }
        else {
            Patient_Document.FolderSearchType = "0";
        }


        var Tab = GetTab(Patient_Document.params["TabID"]);

        if (Tab["PanelID"] == "pnldemographicDetail" || Tab["PanelID"] == "pnlClinicalFaceSheet" || Tab["PanelID"] == "pnlEncounterChargeCapture") {
            Patient_Document.SetDefaultDocument();

        }

        if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "") {

            Patient_Document.params["PanelID"] = Tab["Container"] + ' #' + Tab["PanelID"];

            if (Tab["MasterTabID"] == "mstrTabPatient" || Tab["MasterTabID"] == "mstrTabClinical")
                Patient_Document.SetDefaultDocument();
            else
                Patient_Document.SetDocument(Tab);
        }
        else if (Tab["PanelID"] == "Batch_FaxSend") {
            Patient_Document.SetDefaultDocument();
        }

        //Setup patient document for Note Attachment
        if (Patient_Document.params["ParentCtrl"] == "clinicalTabProgressNote") {
            // MU3-MU3-528 Faizan Ameen 
            if (Patient_Document.params["PanelID"] != 'pnlPatientDocument')
                Patient_Document.params["PanelID"] = Patient_Document.params["PanelID"] + ' #pnlPatientDocument';
            Patient_Document.SetDocumentForNote();
        }
        if (Patient_Document.params["ParentCtrl"] == "Batch_FaxSend")
            Patient_Document.SetDocumentForFax();

        var self = $('#' + Patient_Document.params["PanelID"] + " #frmPatientDocument");
        self.loadDropDowns(false);


        utility.ValidateFromToDate(Patient_Document.params["PanelID"] + " #frmPatientDocument", 'dtpFromEntry', 'dtpToEntry', true);
        utility.ValidateFromToDate(Patient_Document.params["PanelID"] + " #frmPatientDocument", 'dtpFromDOS', 'dtpToDOS', true);
        utility.ValidateFromToDate(Patient_Document.params["PanelID"] + " #frmPatientDocument", 'dtpFromExpiry', 'dtpToExpiry', true);
        //if (Patient_Demographic.params["patientID"] != "") {
        //    Patient_Demographic.FillPatientInfo(Patient_Demographic.params);
        //}
        if (Patient_Document.bIsFirstLoad && (Patient_Document.params.PanelID == "pnlBatchDocuments" || Patient_Document.params.PanelID == "Batch_FaxSend #pnlPatientDocument") && $('#PatientProfile #hfPatientId').val() == "") {
            utility.AddDaysFromToDate('frmPatientDocument', 'dtpFromEntry', 'dtpToEntry', -15, 0);
        }


        //if (Patient_Document.bIsFirstLoad) {
        //    Patient_Document.bIsFirstLoad = false;
        //    var self = $('#' + Patient_Document.params["PanelID"]);
        //    self.loadDropDowns(true);
        //}



        if (params.PreviousTab.TabID == "patTabDemographic") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }
        else if (params.PreviousTab.TabID == "patTabInsurance") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }

        if (Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Patient_Document.params.PanelID + " div#FaceSheetPager", Patient_Document.params.FaceSheetComponents, 'patientdocument');
        } else if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlPatientDocument' + " div#FaceSheetPager", Patient_Document.params.FaceSheetComponents, 'patientdocument');
        }

        if (Patient_Document.params.PanelID == "pnlBatchDocuments") {
            if ($('#PatientProfile #hfPatientId').val() == "") {
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtAccountNumber").val("");
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #hfPatientId").val("");
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtFullName").val("");
            }
            if (Patient_Document.bIsFirstLoad) {
                //PRD-95
                if (Patient_Document.params.PanelID == "pnlBatchDocuments") {
                    if ($('#PatientProfile #hfPatientId').val()) {
                        $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtAccountNumber").val("");
                        $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #hfPatientId").val("");
                        $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtFullName").val("");
                        Patient_Document.bIsFirstLoad = false;
                    }
                }
                Patient_Document.LoadAllControls();
            }
        } else {

            var self = $('#' + Patient_Document.params.PanelID);
            self.find("#frmPatientDocument #practiceDiv").remove();
            Patient_Document.params.PatientDocumentCanvas = "canvasdocument";
            Patient_Document.params.PatientDocumentCanvasPanel = "canvasContainer";
            Patient_Document.params.PatientDocumentGridView = "divGridView";
            Patient_Document.params.PatientDocumentView = "DivDocumentView";
        }
        var IsDDLAppend = $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #ddlDocumentPriority > option").length;
        if (IsDDLAppend == 0)
        { Patient_Document.PriorityDDLLoad = false; }

        if (!Patient_Document.PriorityDDLLoad) {
            $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #ddlDocumentPriority").html("");
            CacheManager.BindCodes('GetDocumentPriority', false).done(function (result) {
                var priorities = JSON.parse(result.GetDocumentPriority)
                $.each(priorities, function (k, obj) {
                    var color = "";
                    if (obj.Name.toLowerCase().trim() == "low")
                        color = "green bold";
                    else if (obj.Name.toLowerCase().trim() == "medium")
                        color = "dark-yellow bold";
                    else if (obj.Name.toLowerCase().trim() == "high")
                        color = "red bold";
                    $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #ddlDocumentPriority").append("<option value='" + obj.Value + "' class='" + color + "' " + ">" + obj.Name + "</option>");
                });
            });
            Patient_Document.PriorityDDLLoad = true;
        }

        //select default patient PMS-1611
        if (!$("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val()) {
            Patient_Document.ResetPatient();


            if (Patient_Document.params.PatientId) {
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(Patient_Document.params.PatientId);
            }
        }

        if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet") {
            $("#" + Patient_Document.params["PanelID"] + " #scanButton").hide()
        }
        if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
            $("#pnlClinicalProgressNote #pnlPatientDocument #scanButton").hide();
            $('#' + Patient_Document.params["PanelID"]).find("#canvasContainer,#canvasdocument").each(function () {
                var id_ = $(this).attr('id');
                $(this).attr('id', id_ + "ClinicalDocument");
            });
            Patient_Document.params.PatientDocumentCanvasPanel = "canvasContainerClinicalDocument";
            Patient_Document.params.PatientDocumentCanvas = "canvasdocumentClinicalDocument";
        }

        if (Patient_Document.params.ParentCtrl == "demographicDetail") {
            $("#pnldemographicDetail #scanButton").hide();
            $('#' + Patient_Document.params["PanelID"]).find("#canvasContainer,#canvasdocument").each(function () {
                var id_ = $(this).attr('id');
                $(this).attr('id', id_ + "demographicDetail");
            });
            Patient_Document.params.PatientDocumentCanvasPanel = "canvasContainerdemographicDetail";
            Patient_Document.params.PatientDocumentCanvas = "canvasdocumentdemographicDetail";
            if (Patient_Document.params.PatientFullName) {
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtFullName").val(Patient_Document.params.PatientFullName);
            }
        }
        else if (Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
            if (Patient_Document.params.PatientFullName) {
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtFullName").val(Patient_Document.params.PatientFullName);
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #txtFullName,#lnkPatientName").attr("disabled", "disabled");
            }
            if (Patient_Document.params.dtpDOSFrom) {
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #dtpFromDOS").datepicker('setDate', Patient_Document.params.dtpDOSFrom);
            }
            if (Patient_Document.params.dtpDOSTo) {
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #dtpToDOS").datepicker('setDate', Patient_Document.params.dtpDOSTo);
                
                $("#" + Patient_Document.params.PanelID + " #frmPatientDocument #dtpToDOS").removeAttr("disabled","disabled");
            }
            if (Patient_Document.params.AccountNumber) {
                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfAccountNo').val(Patient_Document.params.AccountNumber)
            }
        }

        Patient_Document.SetGrid('Reviewed');
        if (Patient_Document.params.ParentCtrl != "Batch_FaxSend") {
            Patient_Document.ParentFormPanelID = "";
        }


        utility.callbackAfterAllDOMLoaded(function () {

            if (Patient_Document.bIsFirstLoad) {
                //AST 105
                if ($('#' + Patient_Document.params["PanelID"] + ' #txtFullName').val() && Patient_Document.params['PatientId']) {
                    var PatientFullName = $('#' + Patient_Document.params["PanelID"] + ' #txtFullName').val();
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + Patient_Document.params["PanelID"] + ' #txtFullName'), PatientFullName + " ", $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId'), Patient_Document.params['PatientId'], "FullName");
                }
                // prd 95 reset patient in Patient batch document from Patient->Documnets flow if (isfirstLoad=true).
                if( Patient_Document.params.PanelID != "pnlBatchDocuments"){
                    if ($('#PatientProfile #hfPatientId').val()) {
                        $("#" + 'pnlBatchDocuments' + " #frmPatientDocument #txtAccountNumber").val("");
                        $("#" + 'pnlBatchDocuments' + " #frmPatientDocument #hfPatientId").val("");
                        $("#" + 'pnlBatchDocuments' + " #frmPatientDocument #txtFullName").val("");
                    }
                }

                Patient_Document.bIsFirstLoad = false;
            }
            // AST 107 Patient documents button Dispaly from  Batch flow.
            if (Patient_Document.params.PanelID == "pnlBatchDocuments") {
                if (Patient_Document.params['PageBatchState'].LandingPage != "Scan") {
                    if ($('#' + Patient_Document.params["PanelID"] + ' #btnExport').hasClass('hidden'))
                        $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
                    if ($('#' + Patient_Document.params["PanelID"] + ' #btnDelete').hasClass('hidden'))
                        $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                    if ($('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').hasClass('hidden'))
                        $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                }
            }
            //AST 107 for Patient Document documents button Dispaly 
            if (Patient_Document.params.PanelID == "ctrlPanPatient #pnlPatientDocument") {
                if (Patient_Document.params['PagePatientState'].LandingPage != "Scan") {
                    if ($('#' + Patient_Document.params["PanelID"] + ' #btnExport').hasClass('hidden'))
                        $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
                    if ($('#' + Patient_Document.params["PanelID"] + ' #btnDelete').hasClass('hidden'))
                        $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                    if ($('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').hasClass('hidden'))
                        $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                }

            }
            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1412px");
            }
        });
        Patient_Document.SetCollapseExpandPanelPatientDocument();
        ////selection of tree item
        //$('#' + Patient_Document.params["PanelID"] + ' #treeBasicDocument').on("select_node.jstree", function (e, data) {
        //    Patient_Document.TogglingView(data);
        //});

        if (Patient_Document.params["ParentCtrl"] == "Batch_FaxSend") {
            $("#" + Patient_Document.params["PanelID"] + ' #divPatDocument  #divGridView').removeClass('col-sm-9 col-md-12 col-sm-9 col-md-12 col-sm-3 col-md-5').addClass('col-sm-12');
        }
        //Ast 105
        $.when(Patient_Document.SetPatientDocumentPageState(), self.loadDropDowns(true)).then(function () {
            Patient_Document.BindAutocomplete();
            Patient_Document.BindPatientNameAutocomplete();
            Patient_Document.BindTagAutocomplete();
        });
        if (globalAppdata["isPatientHealthInformationCapture"] && globalAppdata["isPatientHealthInformationCapture"].toLowerCase() == "false")
            $('#' + Patient_Document.params.PanelID + " #docActionsDiv #btnInformationSubmission").addClass("hidden");
        else
            $('#' + Patient_Document.params.PanelID + " #docActionsDiv #btnInformationSubmission").removeClass("hidden");

    },
    TogglingView: function (data) {
        var id = data.node.id;
        var ParentId = data.node.parent;
        var FolderCount = data.node.original.FolderCount;
        var IsReviewed = data.node.original.IsDocumentReviewed;
        var PatientId = data.node.original.PatientId;
        var FolderName = data.node.original.DocumentFolderName;
        var folderText = data.node.original.text;
        if (folderText.indexOf('(') > -1) {
            folderText = folderText.split('(')[0].trim();
        }


        $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val(FolderCount);
        $('#' + Patient_Document.params["PanelID"] + ' #hfFolderId').val(ParentId);

        if (ParentId == "#") {
            if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                $('#' + Patient_Document.params["PanelID"] + ' #btnAddToNote').removeClass('hidden');
            }
            else {
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnExport').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnDelete').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').removeClass('hidden');
            }
            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentGridView).show();

            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentView).hide();
            //$("#divGridView").show();
            // $("#DivDocumentView").hide();
            // EMR - 6434 if review tab will be active then reviewandsigned will remain disabled. 
            if ($('#' + Patient_Document.params["PanelID"] + ' #liReviewd').hasClass('active'))
                $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').addClass('hidden');
            $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
            $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
            var FolderId = id;
            $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val(FolderId);
            $('#' + Patient_Document.params["PanelID"] + ' #hfFolder').val(folderText);
            Patient_Document.DocumentSearch();
        }
        else {
            if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').addClass('hidden');
                $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').addClass('hidden');
                $('#' + Patient_Document.params["PanelID"] + ' #btnAddToNote').addClass('hidden');
            }
            else {
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnExport').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnDelete').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                if ($('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').hasClass("hidden"))
                    $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').removeClass('hidden');
            }
            var IndexClicked;
            $('#' + data.node.parent).children("ul").children("li").each(function (k, v) {
                if ($(this).find("a").hasClass("jstree-clicked")) {
                    IndexClicked = k;
                }
            });
            // var IndexClicked = Array.prototype.indexOf.call($('#' + data.node.parent)[0].children[2].children, $('#' + data.node.id)[0]);
            $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument' + ' #DivDocumentView' + ' #txtpageNo').val(IndexClicked + 1);
            var documentId = id;
            // $("#divGridView").hide();
            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentGridView).hide();
            // $("#DivDocumentView").show();
            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentView).show();
            $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
            $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
            $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val(documentId);
            $('#' + Patient_Document.params["PanelID"] + ' #lblTotalCount').text(FolderCount);
            Patient_Document.LoadViewer(documentId);
            if (IsReviewed == 0) {
                Patient_Document.SelectedDocumentPatientId = PatientId;
                Patient_Document.SelectedDocumentId = documentId;
                Patient_Document.SelectedDocumentFolderName = FolderName;
            }
            else {
                Patient_Document.SelectedDocumentPatientId = PatientId;
                $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').addClass("hidden")
            }

        }
        Patient_Document.ShowHideMoveButton();
        Patient_Document.setState(false);
    },
    DefaultView: function (IsFromDocumentViewer) {
        if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
            $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
            $('#' + Patient_Document.params["PanelID"] + ' #btnAddToNote').removeClass('hidden');
        }
        else {
            if ($('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').hasClass("hidden"))
                $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
            if ($('#' + Patient_Document.params["PanelID"] + ' #btnExport').hasClass("hidden"))
                $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
            if ($('#' + Patient_Document.params["PanelID"] + ' #btnDelete').hasClass("hidden"))
                $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
        }
        /// start PRD-30
        if ($("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #liPending").hasClass("active")
            && $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').hasClass("hidden")
            ) {
            $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').removeClass("hidden");
        }
        else if ($("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #liReviewd").hasClass("active")) {
            $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').addClass("hidden");
        }
        /// end PRD-30
        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentGridView).show();
        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentView).hide();
        if (IsFromDocumentViewer && IsFromDocumentViewer == 1) {
            $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val($("#" + Patient_Document.params["PanelID"] + " #hfFolderId").val());
        }
        else {
            $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val("");
        }
        $('#' + Patient_Document.params["PanelID"] + ' #helperPDF').val("");
        $('#' + Patient_Document.params["PanelID"] + ' #PDFEditorIF').css("display","none");

        try {
            if (!Document_Viewer.ValidateDocumentSearchScreen()) {
                utility.AddDaysFromToDate(Patient_Document.params.PanelID + " #frmPatientDocument", 'dtpFromEntry', 'dtpToEntry', -15, 0);
                Patient_Document.LoadFolders(true);
            }
            else {
                Patient_Document.LoadFolders(true);
            }
        } catch (e) {

        }

    },
    nextDocument: function (IsFromConfirmPass, PassDocId) {
        var pageNo = $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val();
        var DocumentCount = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val();
        var SelectedDocuemntId = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
        var PatDocID = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id");
        var PatientID = Patient_Document.params.patientID;
        var nextDocId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PatDocID).next().attr("id");
        $('#' + Patient_Document.params["PanelID"] + ' #lblTotalCount').text(DocumentCount);
        pageNo = Number(pageNo) + 1;

        if (pageNo <= DocumentCount) {
            $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val(pageNo);
        }
        
        if (IsFromConfirmPass == "true") {
            Patient_Document.skipDocument(SelectedDocuemntId, PatDocID, null, nextDocId, false, true);
        }
        else if (PatDocID) {
            Patient_Document.checkDocPrivacy(PatientID, SelectedDocuemntId, PatDocID, false, true);
        }
    },
    previousDocument: function (IsFromConfirmPass, PassDocId) {
        var pageNo = $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val();
        var DocumentCount = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val();
        var SelectedDocuemntId = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
        var PatDocID = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id");
        var PatientID = Patient_Document.params.patientID;
        var prevDocId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PatDocID).prev().attr("id");
        $('#' + Patient_Document.params["PanelID"] + ' #lblTotalCount').text(DocumentCount);
        pageNo = Number(pageNo) - 1;

        if (pageNo > 0 && pageNo <= DocumentCount) {
            $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val(pageNo);
        }
        
        if (IsFromConfirmPass == "true") {
            Patient_Document.skipDocument(SelectedDocuemntId, PatDocID, prevDocId, null, true, false);
        }
        else if (PatDocID) {
            Patient_Document.checkDocPrivacy(PatientID, SelectedDocuemntId, PatDocID, true, false);
        }
    },
    checkDocPrivacy: function (PatientID, SelectedDocuemntId, PatDocId, IsFromPrev, IsFromNext) {
        Patient_Document.CheckforPrivacy(PatientID, PatDocId).done(function (response) {
            var ShowPasswordAlert;
            if (response.status == true) {
                if (response.DocPasswordInfoCount > 0) {
                    Patient_Document.PrivateDoc = response.DocPasswordInfo[0].Password;
                    ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                }
            }
            if ((Patient_Document.PrivateDoc == "" && ShowPasswordAlert != "True") || (Patient_Document.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                if (IsFromPrev == true) {
                    if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id")) {
                        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().find("a").click();
                    }
                }
                else if (IsFromNext == true) {
                    if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id")) {
                        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().find("a").click();
                    }
                }
            }
            else {          
                if ($('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').length < 1)
                    Patient_Document.VerifyPassword(PatientID, PatDocId, event, false, null, true, false, IsFromPrev, IsFromNext);
                else {
                    Patient_Document.params.DocPrivate = "1";
                    $('body').find('#modal-from-dom-DocumentConfirmPasswordViewer').modal("show");
                    $("#DivConfirmPasswordViewer #TxtDocPassword").val("");
                    $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentScan").attr("onclick", "Patient_Document.DocPasswordMatch('" + PatientID + "', '" + PatDocId + "', '" + false + "', '" + null + "', '" + true + "', '" + false + "', '" + true + "', '" + false + "');");
                    if (IsFromPrev == true) {
                        $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentSkip").attr("onclick", "Patient_Document.cancelConfirmDialogViewer();Patient_Document.previousDocument('true', " + PatDocId + ");");
                    }
                    else if (IsFromNext == true) {                     
                        $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #btnDocumentSkip").attr("onclick", "Patient_Document.cancelConfirmDialogViewer();Patient_Document.nextDocument('true', " + PatDocId + ");");
                    }
                }
            }
        });
    },
    cancelConfirmDialogViewer: function () {
        $("#modal-from-dom-DocumentConfirmPasswordViewer").modal('hide');
        $('body').find('.modal-backdrop').removeClass('modal-backdrop');
    },
    skipDocument: function (selectedDocuemntId, PatDocId, prevDocId, nextDocId, IsFromPrev, IsFromNext) {
        if (IsFromPrev) {
            if ($('#' + Patient_Document.params["PanelID"] + " #" + PatDocId).prev().attr('islocked') == 1) {
                //when previous consecutive documents locked
                $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val(PatDocId);
                Patient_Document.previousDocument('false', PatDocId);
            }
            else {
                if (prevDocId)                  //previous document will display which is unlocked.
                    $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + prevDocId).find("a").click();
                else {
                    //In case of consecutive documents locked. If no previous document found, set hiddenFolderDocumentId with next selected document id.
                    var nextId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + selectedDocuemntId).next().attr('id');
                    $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val(nextId);
                }
            }
        }
        else if (IsFromNext) {
            if ($('#' + Patient_Document.params["PanelID"] + " #" + PatDocId).next().attr('islocked') == 1) {
                //when next consecutive documents locked
                $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val(PatDocId);
                Patient_Document.nextDocument('false', PatDocId);
            }
            else {
                if (nextDocId)              //next document will display which is unlocked.
                    $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + nextDocId).find("a").click();
                else {
                    //In case of consecutive documents locked. If no next document found, set hiddenFolderDocumentId with previous selected document id.
                    var prevId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + selectedDocuemntId).prev().attr('id');
                    $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val(prevId);
                }
            }
        }
    },
    SignAndReview: function () {
        var docIds;
        if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible")) {
            docIds = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
        }
        else {
            docIds =
              $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument tbody input[id*="chkPatDoc"]:checked').map(function () {
                  return this.id.replace("chkPatDoc", "");
              }).get().join(',');
            if (docIds.length == 0) {
                utility.DisplayMessages("Please select any document(s) to Review & Sign.", 2);
                return;
            }
        }
        Patient_Document.ReviewedAndSignedDocument(docIds, Patient_Document.SelectedDocumentPatientId, Patient_Document.SelectedDocumentFolderName).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                Document_AssignedTo.PendingCountSearchDoc();
                if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible"))
                { Patient_Document.RefreshLandScreenDocumentViewer(); }
                else { Patient_Document.LoadFolders(true); }
            }
            else {
                utility.DisplayMessages(response.message, 2);
            }
        });
    },

    LoadViewer: function (DocId) {
        clearInterval(Document_Viewer.interval);
        $("#" + Patient_Document.params["PanelID"] + " #OpenHTMLDocument").contents().find('html').html("");
        // AST 281 enabled/disabled  next previous button from documents tree by sameer
        var SelectedDocuemntIndex = $('#' + Patient_Document.params["PanelID"] + " #" + DocId).index() + 1
        var folderCount = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val();
        // if selected last documnet then next buttons will be disabled
        if (SelectedDocuemntIndex == folderCount && folderCount != 1) {
            $("#" + Patient_Document.params["PanelID"] + " #lnkPreviousDocument").removeAttr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #lnkNextDocument").attr("disabled", "disabled");
        }
            // if folder have only one document then both buttons will be disabled
        else if (SelectedDocuemntIndex == 1 && folderCount == 1) {
            $("#" + Patient_Document.params["PanelID"] + " #lnkPreviousDocument").attr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #lnkNextDocument").attr("disabled", "disabled");
        }
            // At first documents  select previous button will be disabled
        else if (SelectedDocuemntIndex == 1) {
            $("#" + Patient_Document.params["PanelID"] + " #lnkPreviousDocument").attr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #lnkNextDocument").removeAttr("disabled", "disabled");

        }
        else {
            $("#" + Patient_Document.params["PanelID"] + " #lnkPreviousDocument").removeAttr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #lnkNextDocument").removeAttr("disabled", "disabled");

        }

        var documentCall = Document_Viewer.FillDocument(null, null, DocId);
        $.when(documentCall).done(function (response) {
            // documentCall.done(function (response) {
            if (response.status != false) {
                var document_details = JSON.parse(response.DocumentLoad_JSON);
                var FileType = document_details.FileType;
                var LoadPrevious = document_details.txtLoadPrevious;
                if (document_details.lnkFileNameExt && document_details.FileType == "") {
                    if (document_details.lnkFileNameExt == ".jpg" || document_details.lnkFileNameExt == ".png") {
                        FileType = "image";
                    }
                }
                $("#hfFileType").val(FileType);
                if (FileType.indexOf("pdf") > -1) {
                    // $('#progressnotehtmldoc').remove();
                    $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentCanvasPanel).hide();
                    //  $('#pnlDocumentViewer #imagesControls').hide();
                    $('#' + Patient_Document.params["PanelID"] + ' #OpenDocumentIF').show();
                    $('#' + Patient_Document.params["PanelID"] + ' #OpenHTMLDocument').hide();
                    //  $('#pnlDocumentViewer #extraContorls').hide();
                    Patient_Document.params['DocumentType'] = "PDF";
                    // $('#progressnotehtmldoc').remove();
                    if (LoadPrevious == "0") {
                        utility.LoadFileData(document_details.Base64FileStream, "#" + Patient_Document.params["PanelID"] + ' #OpenDocumentIF');
                    } else {
                        utility.PDFViewer(document_details.Base64FileStream, false, Patient_Document.params["PanelID"] + ' #OpenDocumentIF');
                    }
                }
                else if (FileType == null || FileType == "") {

                    //Document_Viewer.LoadImagesData(document_details.Base64FileStream, document_details.FileType);
                    //  $('#progressnotehtmldoc').remove();
                    $("#" + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentCanvasPanel).hide();
                    //$('#pnlDocumentViewer #imagesControls').hide();
                    $("#" + Patient_Document.params["PanelID"] + ' #OpenDocumentIF').show();
                    $("#" + Patient_Document.params["PanelID"] + ' #OpenHTMLDocument').hide();
                    //$('#pnlPatientDocument #extraContorls').hide();
                    Patient_Document.params['DocumentType'] = "PDF";
                    // $('#progressnotehtmldoc').remove();
                    if (LoadPrevious == "0") {
                        utility.LoadFileData(document_details.Base64FileStream, "#" + Patient_Document.params["PanelID"] + ' #OpenDocumentIF');
                    } else {
                        utility.PDFViewer(document_details.Base64FileStream, false, Patient_Document.params["PanelID"] + ' #OpenDocumentIF');
                    }
                }
                else if (FileType.indexOf("image") > -1) {
                    if (LoadPrevious == "0") {
                        Patient_Document.LoadImagesData(document_details.Base64FileStream, document_details.FileType);
                    }
                    else {
                        try {
                            $("#" + Patient_Document.params["PanelID"] + ' #OpenDocumentIF').hide();
                            $("#" + Patient_Document.params["PanelID"] + ' #OpenHTMLDocument').hide();
                            $("#" + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentCanvasPanel).show();
                            // $('#pnlPatientDocument #imagesControls').show();
                            //  $('#pnlDocumentViewer #extraContorls').show();
                            Patient_Document.params['DocumentType'] = "IMAGE";
                            var imageObj = new Image();
                            //for IE
                            //  imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;

                            imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;
                            var canvas = document.getElementById(Patient_Document.params.PatientDocumentCanvas);

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

                            setTimeout(function () {
                                draw();
                            }, 1000);

                        }
                        catch (ex) {
                            utility.DisplayMessages(ex, 2);
                            console.log(ex);
                        }
                    }
                }

                else if (FileType.indexOf("html") > -1 || FileType.indexOf("text") > -1 || FileType.indexOf("txt") > -1 || FileType.indexOf("zip") > -1) {
                    try {
                        $("#" + Patient_Document.params["PanelID"] + ' #OpenHTMLDocument').show();
                        $("#" + Patient_Document.params["PanelID"] + ' #OpenDocumentIF').hide();
                        $("#" + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentCanvasPanel).hide();
                        // $('#pnlPatientDocument #imagesControls').hide();
                        //  $('#pnlPatientDocument #extraContorls').hide();
                        Patient_Document.params['DocumentType'] = "HTML";

                        if (FileType.indexOf("text") > -1 || FileType.indexOf("txt") > -1) {
                            var blobFile = Patient_Document.dataURLtoFile('data:text/plain;base64,' + document_details.Base64FileStream, document_details.txtFileName);
                            //var blobFile = atob(document_details.Base64FileStream);
                            var reader = new FileReader();
                            reader.onload = function (e) {
                                $("#" + Patient_Document.params["PanelID"] + " #OpenHTMLDocument").contents().find('html').html("<pre>" + reader.result + "</pre>");
                            }
                            reader.readAsText(blobFile);
                        } else if (FileType.indexOf("zip") > -1) {
                            $("#" + Patient_Document.params["PanelID"] + " #OpenHTMLDocument").contents().find('html').html('<div class=" col-sm-4"><a class="btn btn-success btn-xs" id="linkDownload" download=' + document_details.txtFileName + ' href="data:' + FileType + ';base64,' + document_details.Base64FileStream + '"title="Download File"><i class="fa fa-download"></i>' + document_details.txtFileName + '.zip</a></div>')
                        }
                        else {
                            var blobFile = Document_Viewer.dataURLtoFile('data:application/html;base64,' + document_details.Base64FileStream, document_details.txtFileName);
                            var reader = new FileReader();
                            reader.onload = function (e) {
                                $("#" + Patient_Document.params["PanelID"] + " #OpenHTMLDocument").contents().find('html').html(reader.result);
                            }
                            reader.readAsText(blobFile);
                        }


                    }
                    catch (ex) {
                        utility.DisplayMessages(ex, 2);
                        console.log(ex);
                    }
                }

                if ($("#" + Patient_Document.params["PanelID"] + ' #PDFEditorIF').parent().find("#PDFEditorIF").length > 0)
                    $("#" + Patient_Document.params["PanelID"] + ' #PDFEditorIF').parent().find("#PDFEditorIF").hide();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },
    SetDocumentForNote: function () {

        $("#" + Patient_Document.params["PanelID"] + " #headingTitle").html("Attach Document");
        $("#" + Patient_Document.params["PanelID"] + " .not-for-note").addClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #op_attached").removeClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #btnAddToNote").removeClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #docActionsDiv").removeClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #btnAddToFax").addClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #dialogDiv").removeClass("modal-dialog-full");
        $("#" + Patient_Document.params["PanelID"] + " #dialogDiv").addClass("modal-dialog modal-dialog-full");
        if ($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument div").length > 0 && Patient_Document.params.ParentCtrl != "clinicalTabProgressNote")
            $($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument div")[0]).addClass("col-sm-3 col-md-5");
    },

    BindAutocomplete: function () {
        var Ctrl = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber");
        var hfCtrl = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var onSelect = function (e) {
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val(e.FullName);
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").attr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #lnkPatientName").attr("disabled", "disabled");
            utility.InsertRecentPatient(e.id);
        };
        var onChange = function (valid) {
            if (!valid) {
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").removeAttr("disabled", "disabled");
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #lnkPatientName").removeAttr("disabled", "disabled");
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val("");

            }
        }
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    BindTagAutocomplete: function () {
        var Ctrl = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtTagName");
        var hfCtrl = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfDocumentTagId");
        var func = function () { return utility.GetDocumentTagArray(Ctrl.val(), 0) };
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl);
    },
    BindPatientNameAutocomplete: function () {
        var valid = false;

        var Ctrl = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName");
        var hfCtrl = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.FullName && obj.FullName.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.FullName;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
                if (Patient_Document.params.PanelID && (Patient_Document.params.PanelID == "ctrlPanClinical #pnlClinicalFaceSheet")) {
                    Patient_Document.params.PatientId = localStorage.SelectedPatientId;
                }
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.FullName);
            $(hfCtrl).val(dataItem.id);
            // Set Patient Account Number.
            if (dataItem.AccountNumber)
                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfAccountNo').val(dataItem.AccountNumber);
            utility.InsertRecentPatient(dataItem.id);
            if (Patient_Document.params.PanelID && (Patient_Document.params.PanelID == "ctrlPanClinical #pnlClinicalFaceSheet")) {
                Patient_Document.params.PatientId = dataItem.id;
            }
        }
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 4,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArrayByName(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },
    SetDefaultDocument: function () {

        $('#' + Patient_Document.params["PanelID"] + " #divCommonControls").css("display", "none");


        Patient_Document.params.GridPatientDocument = "PendingKGrid";
        Patient_Document.params.GridRevDocument = "ReviewedKGrid";

        Patient_Document.params.PatientDocumentTree = "treeBasicDocument";



        if (Patient_Document.params["PanelID"] == 'pnldemographicDetail #pnlPatientDocument') {

            $('#' + Patient_Document.params["PanelID"]).find("#treeBasicDocument").each(function () {
                var id_ = $(this).attr('id');
                $(this).attr('id', id_ + "demographic");
            });
            Patient_Document.params.PatientDocumentTree = $('#treeBasicDocument' + 'demographic').attr('id');;
        }

        // set Js tree Id form Note flow.
        if (Patient_Document.params["PanelID"] == 'ctrlPanClinical #pnlClinicalProgressNote') {

            $('#' + Patient_Document.params["PanelID"]).find("#treeBasicDocument").each(function () {
                var id_ = $(this).attr('id');
                $(this).attr('id', id_ + "ProgressNote");
            });
            Patient_Document.params.PatientDocumentTree = $('#treeBasicDocument' + 'ProgressNote').attr('id');
        }
        
        if (Patient_Document.params["PanelID"] == 'pnlEncounterChargeCapture #pnlPatientDocument') {

            $('#' + Patient_Document.params["PanelID"]).find("#treeBasicDocument").each(function () {
                var id_ = $(this).attr('id');
                $(this).attr('id', id_ + "Claim");
            });
            Patient_Document.params.PatientDocumentTree = $('#treeBasicDocument' + 'Claim').attr('id');
        }

        $('#' + Patient_Document.params["PanelID"]).find("#dgvPatientDocumentBatch,#dgvPatRevDocumentBatch").each(function () {
            var id_ = $(this).attr('id');
            $(this).attr('id', id_.replace('Batch', ''));
        });
        if (!$("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber").val() || $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber").val() != $("#PatientProfile #hfAccountNo").val()) {
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(Patient_Document.params.patientID);
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber").val(Patient_Document.params.PatientAccountNo);
            // AST - 177 not set undefiend.
            if (Patient_Document.params.PatientLastName && Patient_Document.params.PatientFirstName) {
                var patientFullName = Patient_Document.params.PatientLastName + ", " + Patient_Document.params.PatientFirstName + " ";
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val(patientFullName);

            }


            $("#" + Patient_Document.params["PanelID"] + " #divPatDocument #frmPatientDocument #DivDocumentView").hide()
            $("#" + Patient_Document.params["PanelID"] + " #divPatDocument #frmPatientDocument #divGridView").show();
        }

        if (Patient_Document.params["PanelID"] == 'pnldemographicDetail #pnlPatientDocument') {

            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(Patient_Document.params.PatientId);
        }

        Patient_Document.params.PatientDocumentCanvasPanel = "canvasContainerBatch";
        Patient_Document.params.PatientDocumentCanvas = "canvasdocumentBatch";
        Patient_Document.params.PatientDocumentGridView = "divGridViewBatch";
        Patient_Document.params.PatientDocumentView = "DivDocumentViewBatch";
    },

    SetDocument: function (Tab) {

        $("#" + Tab["Container"]).find("#actionPanPatientDocument").attr('id', Tab['ActionPanContainer']);
        $("#" + Tab["Container"]).find("#pnlPatientDocument").attr('id', Tab['PanelID']);
        Patient_Document.params["PanelID"] = Tab['PanelID'];
        $('#' + Patient_Document.params["PanelID"]).css("display", "block");

        //$('#' + Patient_Document.params["PanelID"] + " #divCommonControls").css("display", "block");
        //$('#' + Patient_Document.params["PanelID"] + " #formpanelheading").css('display', 'block');

        Patient_Document.params.GridPatientDocument = "PendingKGrid";
        Patient_Document.params.GridRevDocument = "ReviewedKGrid";

        Patient_Document.params.PatientDocumentTree = "treeBasicDocumentBatch";
        Patient_Document.params.PatientDocumentCanvasPanel = "canvasContainerBatch";
        Patient_Document.params.PatientDocumentCanvas = "canvasdocumentBatch";
        Patient_Document.params.PatientDocumentGridView = "divGridViewBatch";
        Patient_Document.params.PatientDocumentView = "DivDocumentViewBatch";
          if (Patient_Document.params.PanelID == "ctrlPanBatch #pnlBatchDocuments" || Patient_Document.params.PanelID =="pnlBatchDocuments") {
                // Make dynamically PendingKGrid
                //$('#' +Patient_Document.params["PanelID"]).find("#PendingKGrid").each(function () {
                //    var id_ = $(this).attr('id');
              //    $(this).attr('id', id_ + "BatchDocuments");
              //});
              $('#' + Patient_Document.params["PanelID"]).find("#PendingKGrid").attr('id', "PendingKGrid" + "BatchDocuments");
                Patient_Document.params.GridPatientDocument = $('#PendingKGrid' + 'BatchDocuments').attr('id');

                // Make Dynamically ReviewedKGrid
                //$('#' +Patient_Document.params["PanelID"]).find("#ReviewedKGrid").each(function () {
                //    var id_ = $(this).attr('id');
                //    $(this).attr('id', id_ + "BatchDocuments");
              //    });
                $('#' + Patient_Document.params["PanelID"]).find("#ReviewedKGrid").attr('id', "ReviewedKGrid" + "BatchDocuments");
                Patient_Document.params.GridRevDocument = $('#ReviewedKGrid' + 'BatchDocuments').attr('id');

        }
        $('#' + Patient_Document.params["PanelID"]).find("#treeBasicDocument,#canvasContainer,#canvasdocument,#divGridView,#DivDocumentView").each(function () {
            var id_ = $(this).attr('id');
            $(this).attr('id', id_ + "Batch");
        });
        $('#' + Patient_Document.params["PanelID"]).find("#dgvPatientDocument,#dgvPatRevDocument").each(function () {
            var id_ = $(this).attr('id');
            $(this).attr('id', id_ + "Batch");
        });

        $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result").find(".custom-made-tabs active").each(function () {
            var id_ = $(this).attr('id');
            var href_ = $(this).attr('href');
            if (id_) {
                id_ = id_.replace('_1', '');
                $(this).attr('id', id_ + "_1");
            }
            else if (href_) {
                href_ = href_.replace('_1', '');
                $(this).attr('href', href_ + "_1");
            }


        });

    },

    setHiddenValue: function (DocumentId) {
        $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val(DocumentId);
    },

    ValidateAutoComplete: function (obj) {

        utility.ValidateAutoComplete(obj, Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId', false);
        if ($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber').val() == "") {
            $("#" + Patient_Document.params["PanelID"] + " #txtFullName").removeAttr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #lnkPatientName").removeAttr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #txtFullName").val("");
        }
    },
    ValidatePatientNameAutoComplete: function (obj) {

        utility.ValidateAutoCompletePatientName(obj, Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId', false);

    },
    SearchFolder: function () {
        if (Patient_Document.FolderSearchType == "1") {
            Patient_Document.FolderSearchType = "0";
            $('#' + Patient_Document.params["PanelID"] + ' #FavType').html("Show All");
            EMRUtility.insertUpdateFavListStatus(Patient_Document.FavListName, false);
        }
        else {
            Patient_Document.FolderSearchType = "1";
            $('#' + Patient_Document.params["PanelID"] + ' #FavType').html("Show Less");
            EMRUtility.insertUpdateFavListStatus(Patient_Document.FavListName, true);
        }
        Patient_Document.LoadFolders();
    },
    LoadFolders: function (FromLoad, IsGridReload) {
        var patientId;
        var def = $.Deferred();
        // if patient is selected in Patient-> document or batch-> document or patient is from schedular->demographics then  set Patientid
        //PMS 4385
        if ($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val() || Patient_Document.params['ParentCtrl'] == 'demographicDetail') {
            patientId = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();// == $('#PatientProfile #hfPatientId').val() ? $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val() : $('#PatientProfile #hfPatientId').val();
        }
        else {
            if ($('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').hasClass('hidden')) {

                patientId = $('#PatientProfile #hfPatientId').val() == $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #hfPatientId").val() ? $('#PatientProfile #hfPatientId').val() : $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #hfPatientId").val();
                Patient_Document.params.PatientId = patientId;
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(patientId);

            } else {
                patientId = localStorage.SelectedPatientId == $('#PatientProfile #hfPatientId').val() ? localStorage.SelectedPatientId : $('#PatientProfile #hfPatientId').val();
            }
        }

        // patientId = (Document_Scan && Document_Scan.params["PanelID"]) ? $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #hfPatientId").val() : null;
        if (!patientId) {
            patientId = Patient_Document.params.PatientId == null ? $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val() : Patient_Document.params.PatientId;
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(patientId);
            if (!patientId)
                patientId = localStorage.SelectedPatientId;
        }


        //CacheManager.BindListCodes('#' + Patient_Document.params["PanelID"] + ' #lstDocument', 'GetDocumentWithCount', true, patientId, Patient_Document.FolderSearchType).done(function () {
        //    $('#' + Patient_Document.params["PanelID"] + ' #lstDocument li:first').click();
        //});
        if (IsGridReload && !IsGridReload)
        { }
        else
        { Patient_Document.DocumentSearch(); }
        var self = "";
        if ($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #divGridView").length == 0) {
            self = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument .searchPanel");
        }
        else {
            self = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #divGridView");
        }
        var myJSON = self.getMyJSON();
        myJSON = JSON.parse(myJSON);
        myJSON.ddlEnteredBy_text = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #ddlEnteredBy option:selected").attr('refvalue');
        myJSON = JSON.stringify(myJSON);
        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).jstree("destroy");
        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).html("");
        Patient_Document.GetPatientDocument(myJSON, patientId).done(function (response) {
            if (response.status == true) {
                //$('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).jstree("destroy");
                //$('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).html("");
                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).bind("select_node.jstree", function (e, data) {
                    if (data.node.original.parent == "#") {
                        Patient_Document.TogglingView(data);
                        data.instance.toggle_node(data.node);
                        Patient_Document.DisplayLockIconInFolderPane();
                        e.stopPropagation();
                    }
                    else {
                        $.when(Patient_Document.OpenEditDocumentScreen(data.node.original.PatientId, data.node.original.id, event, false, null, true, data)).done(function () {
                            data.instance.toggle_node(data.node);
                            Patient_Document.DisplayLockIconInFolderPane();
                            e.stopPropagation();
                        });
                    }
                });
                var Folders_JSON = JSON.parse(response.Folders_JSON);
                var FolderChild_JSON = JSON.parse(response.FolderChild_JSON);
                if (Patient_Document.FolderSearchType == "0") {
                    var Folders_JSON_New = [];
                    $.each(Folders_JSON, function (i, item) {
                        if (item.FolderCount > 0) {
                            Folders_JSON_New.push(item);
                        }
                    });
                    Folders_JSON = Folders_JSON_New;
                }

                var pair = [];

                $.each(Folders_JSON, function (i, item) {

                    pair.push({ "id": item.DocId, "parent": "#", "text": item.ShortName, "FolderCount": item.FolderCount, 'icon': 'fa  fa-folder text-warning ', 'li_attr': { 'class': 'rootFolder' } });
                });

                $.each(FolderChild_JSON, function (i, item) {
                    if (item.IsReviewed == 1) {
                        pair.push({ "id": item.DocumentId, "parent": item.FolderId, "text": (item.IsLocked == 0 ? (" " + item.DocumentName) : item.DocumentName), "FolderCount": item.DocCount, "IsDocumentReviewed": item.IsReviewed, "DocumentFolderName": item.DocumentFolderName, "PatientId": item.PatientId, 'title': item.DocumentName, 'icon': 'fa fa-file blue', 'li_attr': { 'class': 'searchData', 'title': item.DocumentName, 'isReviewed': 'Yes', 'IsLocked': item.IsLocked } });
                    }
                    else {
                        pair.push({ "id": item.DocumentId, "parent": item.FolderId, "text": (item.IsLocked == 0 ? (" " + item.DocumentName) : item.DocumentName), "FolderCount": item.DocCount, "IsDocumentReviewed": item.IsReviewed, "DocumentFolderName": item.DocumentFolderName, "PatientId": item.PatientId, 'title': item.DocumentName, 'icon': 'fa fa-file blue', 'li_attr': { 'class': 'red  searchData', 'title': item.DocumentName, 'isReviewed': 'No', 'IsLocked': item.IsLocked } });
                    }
                });
                if (Patient_Document.params.ParentCtrl == 'demographicDetail') {
                    $('#' + Patient_Document.params["PanelID"] + ' #secPanel').addClass('col-xs-12 pl-sm');
                }

                $('#' + Patient_Document.params["PanelID"] + ' #DocTreeSearch').val("").trigger(jQuery.Event('keyup', { keycode: 13 }));
                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).jstree({
                    'core': {
                        'check_callback': true,
                        'data': pair
                    },
                    "search": {
                        "case_insensitive": true,
                        "show_only_matches": true,
                        "fuzzy": false,
                    },
                    "root": {
                        "icon": "/Content/Default/images/tree_icon.png"//,
                    },
                    'sort': function (a, b) {
                        //a1 = this.get_node(a);
                        //b1 = this.get_node(b);
                        //if (a1.icon == b1.icon) {
                        //    return a1.text.toLowerCase().localeCompare(b1.text.toLowerCase());
                        //} else {
                        //    return a1.icon.toLowerCase().localeCompare(b1.icon.toLowerCase());
                        //}
                    },

                    "plugins": ["search", 'types']
                }).bind('loaded.jstree', function (e, data) {
                    //PRD-31 by:MAHMAD
                    if (Patient_Document.params.PanelID && Patient_Document.params.PanelID != "pnlBatchDocuments" && globalAppdata.IsExpandFolderTree && globalAppdata.IsExpandFolderTree.toLowerCase() == "true") {
                        data.instance.open_all();
                    }
                    //PRD-31 by:MAHMAD
                    $.each($('#' + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentTree + " ul").children(), function (i, item) {
                        if (item.innerText.indexOf('(0)') != -1) {
                            $($(item)[0].firstChild).css('visibility', 'hidden');
                        }
                    });
                    Patient_Document.DisplayLockIconInFolderPane();
                }).bind('ready.jstree', function (e, data) {
                    Patient_Document.AppendMoveDocBtn();
                }).on('search.jstree', function (e, data) {
                    if (data.res.length == 0) {
                        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).hide();
                    } else {

                        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).show();

                    }
                });
                $('#' + Patient_Document.params["PanelID"] + " .search-input").keypress(function (e) {
                    if (e.which == 13) return false;
                    //or...
                    if (e.which == 13) e.preventDefault();
                });
                var to = null;
                $('#' + Patient_Document.params["PanelID"] + " .search-input").keyup(function (e, data) {
                    if (to) { clearTimeout(to); }
                    to = setTimeout(function () {
                        if ($.trim($('#' + Patient_Document.params["PanelID"] + " .search-input").val()).length >= 3) {
                            if (e.keyCode == 13 || e.keyCode == 8) {
                                e.preventDefault();
                            }
                            //var searchString = $(this).val();
                            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).show();
                            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).jstree(true).search($('#' + Patient_Document.params["PanelID"] + " .search-input").val());
                        }
                        else {
                            // $('.searchData').show();
                            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).jstree('search', "");
                            if (!$('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).is(":visible")) {
                                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).show();
                            }
                        }
                    }, 250);
                });

            } // end if
            else {
                utility.DisplayMessages(response.Message, 3);
            }

            if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
                patientId = Clinical_FaceSheet.params.patientID;
            }

            //CacheManager.BindListCodes('#' + Patient_Document.params["PanelID"] + ' #lstDocument', 'GetDocumentWithCount', true, patientId, Patient_Document.FolderSearchType).done(function () {
            //    $('#' + Patient_Document.params["PanelID"] + ' #lstDocument li:first').click();
            //});

            //call to get document
            if (FromLoad) {
                if (Patient_Document.FolderSearchType == "1") {
                    $('#' + Patient_Document.params["PanelID"] + ' #FavType').html("Show Less");
                }
                else {
                    $('#' + Patient_Document.params["PanelID"] + ' #FavType').html("Show All");
                }
            }
            def.resolve();
        });
        return def;


        //var $treeview = $("#treeBasicDocument");
        //$treeview
        //.jstree(options)
        //.on('loaded.jstree', function () {
        //    $treeview.jstree('open_all');
        //});
    },
    DisplayLockIconInFolderPane: function(){
        $.each($('#' + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentTree + " ul").children().find('.searchData'), function (i, item) {
            if ((item.attributes.islocked.value == 1) && ($($(item).find('a')).find('i').hasClass('fa-lock') == false)) {
                $(item).find('a').prepend("<i class='fa fa-lock blue' style='font-size:15px;width: 12px;line-height: 1.6;'></i>");
            }
            else if (item.attributes.islocked.value == 0) {
                $($(item).find('a')).find('.fa-file').css('width', '12px');
            }
        });
    },
    LoadFoldersAfterDeleteOrUpdateRecords: function () {
        var patientId = "-1";
        if (Patient_Document.params["PanelID"] == "pnlBatchDocuments") {
            patientId = $('#PatientProfile #hfPatientId').val() == "" ? "-1" : $('#PatientProfile #hfPatientId').val();
        } else {
            patientId = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
        }
        if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
            patientId = Clinical_FaceSheet.params.patientID;
        }

        return CacheManager.BindListCodes('#' + Patient_Document.params["PanelID"] + ' #lstDocument', 'GetDocumentWithCount', true, patientId, Patient_Document.FolderSearchType);
    },
    checkUncheckAll: function (obj) {
        var selectedDiv = $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result li.active a").attr("href");
        var GridId = "";
        if (selectedDiv == "#Pending") {
            //PMS-4635
            GridId = Patient_Document.params.GridPatientDocument;
        }
        else if (selectedDiv == "#Reviewed") {
            //PMS-4635
            GridId = Patient_Document.params.GridRevDocument;
        }
        var dataSource = $('#' + Patient_Document.params.PanelID+ ' #' + GridId).data("kendoGrid").dataSource.data();
        if (dataSource) {
            if ($(obj).prop("checked")) {
                var LockedDoc = false;
                $.each((dataSource), function (i, item) {
                    if (item.ShowPassword != "True" && item.IsLocked == "1") {
                        $('#' + Patient_Document.params["PanelID"] + ' #' + GridId + ' #chkPatDoc' + item.PatDocId + '').prop('checked', false);
                        $(obj).prop("checked", false);
                        LockedDoc = true;
                    } else {
                        $('#' + Patient_Document.params["PanelID"] + ' #' + GridId + ' #chkPatDoc' + item.PatDocId + '').prop('checked', true);
                    }
                });
                if (LockedDoc == true) {
                    utility.DisplayMessages("Please unlock private document(s) in order to proceed.", 2);
                }
            } else {
                $('#' + Patient_Document.params.PanelID + ' #' + GridId + " tbody input:checkbox").prop("checked", false);
            }
        }
        if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "Batch_FaxSend") {
            if (dataSource) {
                $.each((dataSource), function (j, items) {
                    if ((items.ShowPassword == "True" && items.IsLocked == "1") || (items.ShowPassword != "True" && items.IsLocked != "1")) {
                        Patient_Document.SelectDocument(null, obj, items.PatDocId, items.ShowPassword, items.IsLocked);
                    }
                });
            }
        }
    },

    DocumentAssignedTo: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
        var PatDocIds;
                if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible")) {
                    PatDocIds = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
                }
                else {
                    PatDocIds = $('#' + Patient_Document.params["PanelID"] + ' input[id*="chkPatDoc"]:checked').map(function () {
                        return this.id.replace("chkPatDoc", "");
                    }).get().join(',');
                    if ($("#" + Patient_Document.params["PanelID"] + " #DivDocumentView").is(":visible") == true) {
                        PatDocIds = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
                    }
                    if (PatDocIds == "") {
                        utility.DisplayMessages("Please select any document(s) to Assign.", 2);
                        return false;
                    }
                }
                var params = [];
                if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                    params["patientId"] = Patient_Document.params["PatientId"];
                } else {
                    params["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                }
                //params["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #hfPatientId").val();
                params["PatDocIds"] = PatDocIds;
                params["FolderId"] = $("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
                params["mode"] = "Add";
                params["FromAdmin"] = "0";

                if (Patient_Document.params["FromAdmin"] == "0") {
                    //if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                    //    params["ParentCtrl"] = "Patient_Document";
                    //    params["PatientDetail"] = "1";
                    //    params["PanelID"] = Patient_Document.params.PanelID;
                    //} else {
                    //    params["ParentCtrl"] = Patient_Document.params["TabID"];
                    //    params["PatientDetail"] = "0";
                    //}

                    var parentPanelId;
                    if (Patient_Document.params["TabID"] == "patTabDocuments") {
                        params["PanelID"] = "pnlPatientDocument";
                    }
                    else if (Patient_Document.params["TabID"] == "batchTabDocuments") {
                        params["PanelID"] = "pnlBatchDocuments";
                    }
                    else if (Patient_Document.params["TabID"] == "demographicDetail" || Patient_Document.params["TabID"] == "EncounterChargeCapture") {
                        params["ParentCtrl"] = "Patient_Document";
                    }
                    else if (Patient_Document.params["TabID"] == "clinicalTabProgressNote") {
                        params["ParentCtrl"] = "Patient_Document";
                    }
                    else if (Patient_Document.params["TabID"] == "clinicalTabFaceSheet") {
                        params["ParentCtrl"] = "Patient_Document";
                    }
                    else if (Patient_Document.params["TabID"] == "Clinical_FaceSheet") {
                        params["ParentCtrl"] = "Patient_Document";
                    }
                    params["PatientDetail"] = "0";
                    if (Patient_Document.params["PanelID"] == "pnldemographicDetail #pnlPatientDocument"
                     || Patient_Document.params["PanelID"] == "ctrlPanClinical #pnlClinicalProgressNote #pnlPatientDocument") {
                        params["TabID"] = Patient_Document.params["TabID"];
                        Document_AssignedTo.UnloadParent = "ParentUnload";
                        parentPanelId = Patient_Document.params["PanelID"];//GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
                        params["PatientDetail"] = "1";
                        params["ParentCtrlPanelID"] = Patient_Document.params["PanelID"];
                    }
                }
                LoadActionPan('Document_AssignedTo', params, parentPanelId);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DocumentImport: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var PatDocIds = $('#' + Patient_Document.params["PanelID"] + ' input[id*="chkPatDoc"]:checked').map(function () {
                    return this.id.replace("chkPatDoc", "");
                }).get().join(',');

                var params = [];
                if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                    params["patientId"] = Patient_Document.params["PatientId"];
                    params["CaseId"] = Patient_Document.params["CaseId"];
                    params["CaseNo"] = Patient_Document.params["CaseNo"];
                    if (Patient_Document.params.ParentCtrl == "demographicDetail") {
                        params["AccountNo"] = Patient_Document.params["AccountNo"];
                        params["GrandParent"] = Patient_Document.params["ParentCtrl"];
                    }
                } else {
                    params["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                    params["AccountNo"] = $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfAccountNo').val();
                }
                params["FolderId"] = $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentTree + " li a.jstree-clicked").parent().attr("id"); //$("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
                params["PatDocIds"] = PatDocIds;
                params["RefCtrl"] = "ImportDoc";
                params["mode"] = "Add";
                //AST-285 BY:MAHMAD
                params["from"] = "Patient_Document";
                //AST-285 BY:MAHMAD
                params["FromAdmin"] = "0";
                if (Patient_Document.params["FromAdmin"] == "0") {
                    if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                        params["ParentCtrl"] = "Patient_Document";
                        params["PatientDetail"] = "1";
                        if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
                            params["PopulateDOS"] = true;
                            params["FromNote"] = "1";
                        }
                        if (Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                            if (Patient_Document.params.dtpDOSFrom)
                            { params["DOS"] = Patient_Document.params.dtpDOSFrom; }
                            if (Patient_Document.params.ClaimNumber)
                            {
                                params["ClaimNumber"] = Patient_Document.params.ClaimNumber;
                                params["VisitId"] = Patient_Document.params.VisitId;
                            }
                           
                        }
                        params["ParentCtrlPanelID"] = Patient_Document.params.PanelID;
                        LoadActionPan('Document_Import', params, Patient_Document.params.PanelID);
                    }
                    else {
                        params["ParentCtrl"] = Patient_Document.params["TabID"];
                        params["PatientDetail"] = "0";
                        LoadActionPan('Document_Import', params);
                    }
                }
                else {
                    LoadActionPan('Document_Import', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FolderImport: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var PatDocIds = $('#' + Patient_Document.params["PanelID"] + ' input[id*="chkPatDoc"]:checked').map(function () {
                    return this.id.replace("chkPatDoc", "");
                }).get().join(',');

                var params = [];
                params["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                params["FolderId"] = $("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
                params["RefCtrl"] = "ImportFolder";
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["PatientDetail"] = "1";
                if (Patient_Document.params["FromAdmin"] == "0") {
                    if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                        params["ParentCtrl"] = "Patient_Document";
                        params["CaseId"] = Patient_Document.params["CaseId"];
                        params["CaseNo"] = Patient_Document.params["CaseNo"];
                    }
                    else if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
                        params["PopulateDOS"] = true;
                    }
                    else {
                        params["ParentCtrl"] = Patient_Document.params["TabID"];
                    };
                }
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FolderAdd: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                params["mode"] = "Add";
                params["FromAdmin"] = "0";
                params["PatientDetail"] = "1";
                //if (Patient_Document.params["FromAdmin"] == "0") {
                //prd 53
                if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = "Patient_Document";
                } else {
                    params["ParentCtrl"] = Patient_Document.params["TabID"];
                }
                //}
                LoadActionPan('folderDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DocumentAdd: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["DocumentId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Patient_Document.params["FromAdmin"];
                if (Patient_Document.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Patient_Document';
                }
                //LoadActionPan('folderDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenPatientAccount: function (RefCtrl) {
        var PanelID = "";
        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = RefCtrl;
        if (Patient_Document.params.ParentCtrl == "demographicDetail") {
            params["ParentCtrl"] = "Patient_Document";
            PanelID = 'pnldemographicDetail #pnlPatientDocument';
        }
        else if (Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
            params["ParentCtrl"] = "Patient_Document";
            PanelID = 'pnlPatientCaseDetail #pnlPatientDocument';
        }
        else if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet") {
            params["ParentCtrl"] = "Patient_Document";
            PanelID = 'pnlClinicalFaceSheet #pnlPatientDocument';
        }
        else if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = "Patient_Document";
            PanelID = 'pnlClinicalProgressNote #pnlPatientDocument';
        }
        else {
            if (Patient_Document.params["TabID"] == "batchTabDocuments")
                PanelID = "pnlBatchDocuments";
            else
                PanelID = 'pnlPatientDocument';
            params["ParentCtrl"] = Patient_Document.params["TabID"];
        }
        params["ParentPanelID"] = PanelID;
        LoadActionPan("Patient_Search", params, PanelID);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, FirstName, LastName, RefCtrl, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (RefCtrl == "txtFullName") {
            $('#' + Patient_Document.params["PanelID"] + ' #txtFullName').val(LastName + ", " + FirstName + " ");
        }
        else {
            $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber').val(AccountNo);
            // Patient_Document.BindAutocomplete();
            $("#" + Patient_Document.params["PanelID"] + " #txtFullName").val(LastName + ", " + FirstName + " ");
            $("#" + Patient_Document.params["PanelID"] + " #txtFullName").attr("disabled", "disabled");
            $("#" + Patient_Document.params["PanelID"] + " #lnkPatientName").attr("disabled", "disabled");
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber'), AccountNo, $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId'), PatientId);
            $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber').focus();
        }

        if (Patient_Document.params.PanelID && (Patient_Document.params.PanelID == "ctrlPanClinical #pnlClinicalFaceSheet")) {
            Patient_Document.params.PatientId = PatientId;
        }
        $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(PatientId);
        $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfAccountNo').val(AccountNo);
        utility.SetKendoAutoCompleteSourceforValidate($('#' + Patient_Document.params["PanelID"] + ' #txtFullName'), LastName + ", " + FirstName + " ", $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId'), PatientId, "FullName");
        if (Patient_Search.params != null && Patient_Search.params.ParentCtrl) {
            if (Patient_Search.params.ParentCtrl == "Patient_Document") {

                UnloadActionPan(Patient_Search.params.ParentCtrl, 'Patient_Search', null, Patient_Search.params.ParentPanelID);
            } else {
                UnloadActionPan(Patient_Search.params.ParentCtrl);
            }

            Patient_Search.params = null;
        } else {
            UnloadActionPan("Patient_Document");
        }
        utility.InsertRecentPatient(PatientId);

        // load folser as well as load grid if new patient is add fro demographic deatail form AST-20
        if (Patient_Document.params['ParentCtrl'] == 'demographicDetail') {
            //Patient_Document.LoadFolders(true, true);
            Patient_Document.params.PatientId = PatientId;
        }



    },
    EditViewerDocument: function () {
        var params = [];

        if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {
            params["PatientID"] = Patient_Document.SelectedDocumentPatientId;
        }
        else {
            params["PatientID"] = Patient_Document.params["patientID"];
        }
        if (!params["PatientID"])
            params["PatientID"] = Patient_Document.params["PatientID"];
        params["PatDocID"] = $("#" + Patient_Document.params["PanelID"] + " #hfFolderDocumentId").val();
        params["FolderID"] = $("#" + Patient_Document.params["PanelID"] + " #hfFolderId").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = Patient_Document.params["FromAdmin"];
        if ((Patient_Document.params.ParentCtrl == "demographicDetail" && $('#mstrTabDashBoard').hasClass('active')) || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
            params["ParentCtrl"] = 'Patient_Document';
            params["PatientDetail"] = "1";
        } else if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet") {
            params["ParentCtrl"] = 'Patient_Document';
            params["PatientDetail"] = "1";
        }
        else {
            params["ParentCtrl"] = Patient_Document.params["TabID"];
            params["PatientDetail"] = "0";
        }

        LoadActionPan('Document_Viewer', params);
    },
    DocumentEdit: function (PatientID, PatDocID, event, IsFromDashboard, fileType, DocumentFolderId) {

        $.connection.hub.stop();
        Patient_Document.SignalRHub = null;

        $.connection.hub.qs = {
            "Username": globalAppdata.AppUserFirstName + " " + globalAppdata.AppUserLastName,
            "UserId": globalAppdata.AppUserId,
            "PatDocID": PatDocID,
        }

        Patient_Document.SignalRHub = $.connection.patientDocumentAccessHub;
        Patient_Document.SignalRHub.client.otherUserAccessDocument = function (response) {
            response = JSON.parse(response);
            if (response) {
                utility.DisplayMessages(response.UserName + " is accessing the same document", 2);
            }
        };

        var FileType
        if (fileType) {
            FileType = fileType;
        } else {
            FileType = $($($(event)[0].currentTarget).closest("tr")).attr('FileType');
        }

        $.connection.hub.start()
       .done(function () {

           var Username = globalAppdata.AppUserFirstName + " " + globalAppdata.AppUserLastName;
           Patient_Document.SignalRHub.server.concurrencyDocumentAccessAlert(Username, PatDocID, globalAppdata.AppUserId).done(function (response) {

               response = JSON.parse(response);
               if (response.status) {

                   var lMsg = response.UserName + " is accessing the same document, and any actions you take may overwrite those of the other user. Do you still want to proceed?";
                   utility.myConfirm(lMsg, function () {
                       Patient_Document.OpenEditDocumentScreen(PatientID, PatDocID, event, IsFromDashboard, FileType, false, null);
                       Patient_Document.SignalRHub.server.concurrencyAlertMessageToCurrentUser(PatDocID, globalAppdata.AppUserId).done(function (response) { });

                   }, function () {
                       $.connection.hub.stop();

                   }, 'Access Confirmation');
               } else {

                   Patient_Document.OpenEditDocumentScreen(PatientID, PatDocID, event, IsFromDashboard, FileType, false, null);
               }

           });
       })

       .fail(function () {
           //console.log('You could not have connected to the server.');
       });

    },

    OpenEditDocumentScreen: function (PatientID, PatDocID, event, IsFromDashboard, FileType, IsFromLoadFolder,LoadFolderData,DocumentFolderId) {

        var strMessage = "";
        var ShowPasswordAlert = "";
        if (event != null) {
            //event.stopPropagation();
        }
        //var FileType = $($($(event)[0].currentTarget).closest("tr")).attr('FileType');
        
        Patient_Document.CheckforPrivacy(PatientID, PatDocID).done(function (response) {
            if (response.status == true) {
                if (response.DocPasswordInfoCount > 0) {
                    Patient_Document.PrivateDoc = response.DocPasswordInfo[0].Password;
                    ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                }
            }

            utility.SelectGridRow($("dgvPatientDocument_row" + PatDocID));
            if ((Patient_Document.PrivateDoc == "" && ShowPasswordAlert != "True") || (Patient_Document.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                if(IsFromLoadFolder != true)
                    Patient_Document.OpenDocumentEdit(PatientID, PatDocID, IsFromDashboard, FileType, DocumentFolderId);
                else
                    Patient_Document.TogglingView(LoadFolderData);
            }
            else {
                if ($('body').find('#modal-from-dom-DocumentConfirmPassword').length < 1) {
                    if (IsFromLoadFolder != true)
                        Patient_Document.VerifyPassword(PatientID, PatDocID, event, IsFromDashboard, FileType);
                    else
                        Patient_Document.VerifyPassword(PatientID, PatDocID, event, IsFromDashboard, FileType, 'null', 'true');
                }
                else {
                    $('body').find('#modal-from-dom-DocumentConfirmPassword').modal("show");
                    $("#DivConfirmPassword #TxtDocPassword").val("");
                    if (IsFromLoadFolder != true)
                        $("#modal-from-dom-DocumentConfirmPassword #DivConfirmPassword #btnDocumentScan").attr("onclick", "Patient_Document.DocPasswordMatch('" + PatientID + "', '" + PatDocID + "', '" + IsFromDashboard + "', '" + FileType + "');");
                    else
                        $("#modal-from-dom-DocumentConfirmPassword #DivConfirmPassword #btnDocumentScan").attr("onclick", "Patient_Document.DocPasswordMatch('" + PatientID + "', '" + PatDocID + "', '" + IsFromDashboard + "', '" + FileType + "', '" + null + "', '" + true + "');");
                }
            }
        });
    },

    OpenDocumentEdit: function (PatientID, PatDocID, IsFromDashboard, FileType, DocumentFolderId) {
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" && FileType && FileType.indexOf("image") >= 0) {
                    var params = [];
                    params["PatientID"] = PatientID;
                    params["PatDocID"] = PatDocID;
                    params["NotesId"] = Patient_Document.params.NotesId;
                    params["FolderID"] = DocumentFolderId ? DocumentFolderId : $("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Patient_Document.params["FromAdmin"];
                    params["ParentCtrl"] = "Patient_Document";
                    params["PatientDetail"] = "1";
                    params["IsAttached"] = Patient_Document.getIsChecked(PatDocID);
                    params["ParentCtrlPanelID"] = Patient_Document.params.PanelID;
                    LoadActionPan('Patient_Document_Image_Annotation', params, Patient_Document.params.PanelID);
                }
                else {
                    var params = [];
                    params["PatientID"] = PatientID;
                    params["PatDocID"] = PatDocID;
                    params["FolderID"] = DocumentFolderId ? DocumentFolderId : $("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Patient_Document.params["FromAdmin"];
                    if (IsFromDashboard == 1) {
                        params["ParentCtrl"] = "mstrTabDashBoard";
                        params["IsDashboardDocUpdated"] = "0";
                    }
                    if (Patient_Document.params["FromAdmin"] == "0") {
                        if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "Batch_FaxSend" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                            params["ParentCtrl"] = "Patient_Document";
                            params["PatientDetail"] = "1";
                            params["ParentCtrlPanelID"] = Patient_Document.params.PanelID;
                            LoadActionPan('Document_Viewer', params, Patient_Document.params.PanelID);
                        } else {
                            params["ParentCtrl"] = Patient_Document.params["TabID"];
                            params["PatientDetail"] = "0";
                            LoadActionPan('Document_Viewer', params);
                        }
                    }
                    else {
                        LoadActionPan('Document_Viewer', params);
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CheckforPrivacy: function (PatientID, PatDocID) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientID=" + PatientID + "&PatDocID=" + PatDocID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "CHECK_FOR_PRIVACY");
    },

    getIsChecked: function (PatDocId) {

        var selectedDiv = $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result li.active a").attr("href");
        var GridId = "";
        if (selectedDiv == "#Pending") {
            GridId = " #" + Patient_Document.params.GridPatientDocument;
        }
        else if (selectedDiv == "#Reviewed") {
            GridId = " #" + Patient_Document.params.GridRevDocument;
        }

        var chkBox = $('#' + Patient_Document.params["PanelID"] + GridId).find('input[id="chkPatDoc' + PatDocId + '"]');

        return $(chkBox).prop('checked');
    },
    SelectedDocumentDelete: function () {
        if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible")) {
            var docIds = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
            Patient_Document.DocumentDelete(docIds);
        }
        else {
            var PatDocIds = $('#' + Patient_Document.params.PanelID + ' ' + $('#' + Patient_Document.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table input[id*="chkPatDoc"]:checked').map(function () {
                return this.id.replace("chkPatDoc", "");
            }).get().join(',');
            if (PatDocIds == "") {
                utility.DisplayMessages("Please select any document(s) to delete.", 2);
                return false;
            }

            else {
                Patient_Document.DocumentDelete(PatDocIds);
            }
        }
    },
    sendAsFax: function () {
        if (Patient_Document.ParentFormPanelID != "") {
            Patient_Document.params.PanelID = Patient_Document.ParentFormPanelID;
        }
        else {
            Patient_Document.params.PanelID = Patient_Document.params.PanelID;
        }
        var docIds;
        if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible")) {
            docIds = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
        }
        else {
            var objLength = $('#' + Patient_Document.params.PanelID + ' input:checked').length;
            if (objLength == 0) {
                utility.DisplayMessages("Please select any document(s) to Fax.", 2);
                return;
            }
            docIds =
                   $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument tbody input[id*="chkPatDoc"]:checked').map(function () {
                       return this.id.replace("chkPatDoc", "");
                   }).get().join(',');
        }

        var DocumentId = docIds;
        Patient_Document.FillDocumentsForFax(null, Clinical_ProgressNote.params.patientID, DocumentId).done(function (response) {
            if (response.status) {
                var params = [];
                params["PDFBase64"] = response.MergedContent;
                var patientId=$("#"+Patient_Document.params.PanelID+" #frmPatientDocument #hfPatientId").val();

                if(!patientId){
                    patientId=  $('#PatientProfile #hfPatientId').val()
                }
                params["PatientId"] = patientId;
                if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                    params["ParentCtrl"] = "Patient_Document";
                    params["PatientDetail"] = "1";
                } else {
                    params["ParentControl"] = "BatchDocuments";
                }
                if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).is(":visible"))
                {
                    var pendingGrid = $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).data("kendoGrid");
                    if (pendingGrid.tbody.find("input:checked").length == 1) {
                        pendingGrid.tbody.find("input:checked").closest("tr").each(function (index) {
                            params["FileName"] = $(this).find("td:eq(4)").text().trim();
                        });
                    }
                }
                if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).is(":visible"))
                {
                    var reviewedGrid = $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).data("kendoGrid");
                    if (reviewedGrid.tbody.find("input:checked").length == 1) {
                        reviewedGrid.tbody.find("input:checked").closest("tr").each(function (index) {
                            params["FileName"] = $(this).find("td:eq(4)").text().trim();
                        });
                    }
                }

                Patient_Document.ParentFormPanelID = Patient_Document.params.PanelID;
                //EMR-6446 by:MAHMAD
                if (Patient_Document.params.TabID == "demographicDetail") {
                    // EMR-6486 EMR-6464
                    if (Patient_Document.params.PanelID == "pnldemographicDetail #pnlPatientDocument")
                        PanelID = Patient_Document.params.PanelID
                    else
                    PanelID = 'pnlClinicalFaceSheet #pnlPatientDocument';
                    LoadActionPan("Batch_FaxSend", params, PanelID);
                }
                else {
                    LoadActionPan("Batch_FaxSend", params);
                }
                //EMR-6446 by:MAHMAD
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


    },

    DocumentDelete: function (DocumentId, event, IsFromAttach, ChildPatientDocId, LinkDocumentId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("dgvPatientDocument_row" + DocumentId));
        AppPrivileges.GetFormPrivileges("Documents", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var ShowPasswordAlert = "";
                Patient_Document.CheckforPrivacy(Patient_Document.params.patientID, DocumentId).done(function (response) {
                    if (response.status == true) {
                        if (response.DocPasswordInfoCount > 0) {
                            Patient_Document.PrivateDoc = response.DocPasswordInfo[0].Password;
                            ShowPasswordAlert = response.DocPasswordInfo[0].ShowPasswordAlert;
                        }
                    }

                    if ((Patient_Document.PrivateDoc == "" && ShowPasswordAlert != "True") || (Patient_Document.PrivateDoc != "" && ShowPasswordAlert == "True")) {
                        Patient_Document.SignalRUsersDocumentsAccess(DocumentId);
                        setTimeout(function () {
                            if (Patient_Document.AnotherUserAccessSameDocument) {
                                var documentIdsCount = DocumentId.split(",")
                                if (documentIdsCount.length > 1) {
                                    utility.myConfirm('50', function () {
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
                                                    Patient_Document.DeleteDocument(selectedValue, ChildPatientDocId,LinkDocumentId).done(function (response) {
                                                        if (response.status != false) {
                                                            if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {
                                                                // Pening Document count set
                                                                Document_AssignedTo.PendingCountSearchDoc();
                                                                var pageNo = $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val();
                                                                var DocumentCount = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val();
                                                                $('#' + Patient_Document.params["PanelID"] + ' #lblTotalCount').text(DocumentCount);
                                                                pageNo = Number(pageNo) + 1;
                                                                if (pageNo <= DocumentCount) {
                                                                    $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val(pageNo);
                                                                }
                                                                var SelectedDocuemntId = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
                                                                if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id")) {
                                                                    var NextDocumentId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id");
                                                                    Patient_Document.LoadFolders(true, false).done(function () {
                                                                        if (NextDocumentId) {
                                                                            utility.callbackAfterAllDOMLoaded(function () {
                                                                                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + NextDocumentId).find("a").click();
                                                                            });
                                                                        }
                                                                    });
                                                                }
                                                                else if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id")) {
                                                                    var PreviousDocumentId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id");
                                                                    Patient_Document.LoadFolders(true, false).done(function () {
                                                                        if (PreviousDocumentId) {
                                                                            utility.callbackAfterAllDOMLoaded(function () {
                                                                                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PreviousDocumentId).find("a").click();
                                                                            });
                                                                        }
                                                                    });
                                                                }
                                                                else {
                                                                    Patient_Document.DefaultView();
                                                                }
                                                            }
                                                            else {
                                                                // Pening Document count set
                                                                Document_AssignedTo.PendingCountSearchDoc();
                                                                if (response.Message == "Document(s) are Associted with Medical Document." || response.Message == "Document(s) are linked with some other documents. Please delete its link and then try again.")
                                                                    utility.DisplayMessages(response.Message, 3);
                                                                else
                                                                    utility.DisplayMessages(response.Message, 1);
                                                                $.connection.hub.stop();
                                                                Patient_Document.LoadFolders(true);
                                                            }
                                                        }
                                                        else {
                                                            Document_AssignedTo.PendingCountSearchDoc();
                                                            utility.DisplayMessages(response.Message, 3);
                                                            $.connection.hub.stop();
                                                            Patient_Document.LoadFolders(true);
                                                        }
                                                    });
                                                }
                                                else {
                                                    utility.DisplayMessages(res_message, 3);
                                                }
                                            });
                                        } // first else 
                                    }, function () {
                                        $.connection.hub.stop();
                                    },
                              '50'
                          );
                                }
                                    // in case of one records.
                                else {
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
                                                    Patient_Document.DeleteDocument(selectedValue, ChildPatientDocId, LinkDocumentId).done(function (response) {
                                                        if (response.status != false) {
                                                            if (response.Message == "Document(s) are Associted with Medical Document." || response.Message == "Document(s) are linked with some other documents. Please delete its link and then try again.")
                                                                utility.DisplayMessages(response.Message, 3);
                                                            else
                                                                utility.DisplayMessages(response.Message, 1);
                                                            if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {
                                                                // Pening Document count set
                                                                Document_AssignedTo.PendingCountSearchDoc();
                                                                var pageNo = $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val();
                                                                var DocumentCount = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val();
                                                                $('#' + Patient_Document.params["PanelID"] + ' #lblTotalCount').text(DocumentCount);
                                                                pageNo = Number(pageNo) + 1;
                                                                if (pageNo <= DocumentCount) {
                                                                    $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val(pageNo);
                                                                }
                                                                var SelectedDocuemntId = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
                                                                if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id")) {
                                                                    var NextDocumentId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id");
                                                                    Patient_Document.LoadFolders(true, false).done(function () {
                                                                        if (NextDocumentId) {
                                                                            utility.callbackAfterAllDOMLoaded(function () {
                                                                                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + NextDocumentId).find("a").click();
                                                                            });
                                                                        }
                                                                    });
                                                                }
                                                                else if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id")) {
                                                                    var PreviousDocumentId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id");
                                                                    Patient_Document.LoadFolders(true, false).done(function () {
                                                                        if (PreviousDocumentId) {
                                                                            utility.callbackAfterAllDOMLoaded(function () {
                                                                                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PreviousDocumentId).find("a").click();
                                                                            });
                                                                        }
                                                                    });
                                                                }
                                                                else {
                                                                    Patient_Document.DefaultView();
                                                                }
                                                            }
                                                            else {
                                                                // Pening Document count set
                                                                Document_AssignedTo.PendingCountSearchDoc();
                                                                $.connection.hub.stop();
                                                                Patient_Document.LoadFolders(true);
                                                            }
                                                        }
                                                        else {
                                                            Document_AssignedTo.PendingCountSearchDoc();
                                                            utility.DisplayMessages(response.Message, 3);
                                                            $.connection.hub.stop();
                                                            Patient_Document.LoadFolders(true);
                                                        }
                                                    });
                                                }
                                                else {
                                                    utility.DisplayMessages(res_message, 3);
                                                }
                                            });
                                        } // first else 
                                    }, function () {
                                        $.connection.hub.stop();
                                    },
                            '1'
                        );
                                }  // end else
                            }
                            else { $.connection.hub.stop(); }
                        },
                           400);
                    } else {
                        utility.DisplayMessages("Please unlock private document in order to proceed.", 2);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DocumentActiveInactive: function (FolderId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = FolderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Document.UpdateFolderActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Document.DocumentSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { }, '3', null, null, null, IsActive);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DocumentSearch: function (DocumentId, PageNo, rpp, IsReviewed) {
                var pendingGridPageSize;
                var reviewedGridPageSize;
                var pendingGridPage;
                var reviewedGridPage;
                var pendingGrid = $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).data("kendoGrid");
                var reviewedGrid = $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).data("kendoGrid");
                if (PageNo == undefined) {
                    pendingGridPage = pendingGrid ? pendingGrid.dataSource.page() : 1;
                    reviewedGridPage = reviewedGrid ? reviewedGrid.dataSource.page() : 1;
                }
                else {
                    pendingGridPage = PageNo;
                    reviewedGridPage = PageNo;
                }
                if (rpp == undefined) {
                    pendingGridPageSize = pendingGrid ? pendingGrid.dataSource.pageSize() : 15;
                    reviewedGridPageSize = reviewedGrid ? reviewedGrid.dataSource.pageSize() : 15;
                }
                else {
                    pendingGridPageSize = rpp;
                    reviewedGridPageSize = rpp;
                }
                Patient_Document.KPendingGridLoad(pendingGridPage, pendingGridPageSize, "0");
                Patient_Document.KReviewedGridLoad(reviewedGridPage, reviewedGridPageSize, "1");
    },

    SetGrid: function (tab) {

        if (tab == "Pending") {

            //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocument thead").find("#threviewd").css("display", "none");
            //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocument thead").find("#threviewddate").css("display", "none");
            //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocument thead").find("#thsigned").css("display", "none");

            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + " thead").find("#threviewd").css("display", "");
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + " thead").find("#threviewddate").css("display", "");
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + " thead").find("#thsigned").css("display", "");
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #Pending").show();
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #Reviewed").hide();
            ///PRD-30
            if ($('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').hasClass("hidden"))
            { $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').removeClass('hidden'); }
            if (Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                if (!$('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').hasClass("hidden"))
                { $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').addClass('hidden'); }
            }
        }
        else {
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #Pending").hide();
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #Reviewed").show();
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #liPending").removeClass('active');
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #liReviewd").addClass('active');
            // PRD-30
            if (!$('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').hasClass("hidden"))
            { $('#' + Patient_Document.params["PanelID"] + ' #btnReviewSign').addClass('hidden'); }
            //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocument thead").find("#threviewd").css("display", "");
            //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocument thead").find("#threviewddate").css("display", "");
            //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocument thead").find("#thsigned").css("display", "");
        }
    },

    DocumentGridLoad: function (response, PageNo, rpp) {
        if (Patient_Document.params.GridPatientDocument == "" || Patient_Document.params.GridPatientDocument == null) {
            Patient_Document.params.GridPatientDocument = Patient_Document.params.GridPatientDocument;
        }
        $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridPatientDocument + " #chkMasterPatDoc").prop("checked", false);
        //$("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridPatientDocument).dataTable().fnDestroy();
        //$("#" + Patient_Document.params["PanelID"] + " #dgvPatientDocumentReviewed").dataTable().fnDestroy();

        if ($.fn.dataTable.isDataTable("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridPatientDocument)) {
            $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridPatientDocument).dataTable().fnClearTable();
            $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridPatientDocument).dataTable().fnDestroy();
        }

        $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument + " tbody").find("tr").remove();

        //var PendingDivHTML = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result div#Pending").html();
        //var objDivReviewed = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result div#Reviewed");

        //objDivReviewed.empty().append(PendingDivHTML);
        //objDivReviewed.find("#PatientDocument_SelectedDataKeys").attr("id", "PatientDocumentReviewed_SelectedDataKeys");
        //objDivReviewed.find("#dgvPatientDocument").attr("id", "dgvPatientDocumentReviewed");

        //default open tab
        //Patient_Document.SetGrid("Pending");

        if (response.PendingCount > 0) {
            var DocumentLoad_JSONData = response.DocumentLoad_JSON;
            $.each(DocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                if (Patient_Document.params["ParentCtrl"] != "clinicalTabProgressNote") {
                    $row.attr("onclick", "Patient_Document.DocumentEdit('" + item.PatientId + "'  ,'" + item.PatDocId + "',event   );utility.SelectGridRow($(this))");
                }
                if (Patient_Document.params["ParentCtrl"] == "Batch_FaxSend") {
                    $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument).find('#ColumnAction').addClass('hidden');
                }

                $row.attr("id", "dgvPatientDocument_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);
                $row.attr("Patientid", item.PatientId);
                $row.attr("FileType", item.FileType);
                $row.attr("IsLocked", item.IsLocked);
                $row.attr("ShowPassword", item.ShowPassword);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var DocPriority = "";
                if (item.DocPrioirty) {
                    if (item.DocPrioirty.toLowerCase().trim() == "low") {
                        DocPriority = '<span class=green bold>' + item.DocPrioirty + '</span>';
                    }
                    else if (item.DocPrioirty.toLowerCase().trim() == "medium") {
                        DocPriority = '<span class=dark-yellow bold>' + item.DocPrioirty + '</span>';
                    } else if (item.DocPrioirty.toLowerCase().trim() == "high") {
                        DocPriority = '<span class=red bold>' + item.DocPrioirty + '</span>';
                    }
                }
                var FileNameWithIcon = "";
                if (item.IsLocked == 1 && item.LinkDocument && item.LinkDocument == 0) {
                    FileNameWithIcon = "<div class='ellipses size100'><i class='fa fa-lock blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + item.FilePath + "</div>"
                }
                else if (item.IsLocked == 0 && item.LinkDocument && item.LinkDocument > 0) {
                    FileNameWithIcon = "<div class='ellipses size100'><i class='fa fa-paperclip blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + item.FilePath + "</div>"
                }
                else if (item.IsLocked == 1 && item.LinkDocument && item.LinkDocument > 0) {
                    FileNameWithIcon = "<div class='ellipses size100'><i class='fa fa-lock  blue' style='font-size:15px;'></i> <i class='fa fa-paperclip blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + item.FilePath + "</div>"
                }
                else {
                    FileNameWithIcon = "<div class='ellipses size100'>&nbsp;&nbsp;&nbsp; " + item.FilePath + "</div>"
                }
                //var ToolTipComment = item.Comments;
                // item.Comments = ToolTipComment.substring(0, 20);

                var selectDocument = "";

                if (item.ReviewBy == "") {
                    //Begin: Active Inactive button added in the grid as per issue number PMS-2835, author Abdur Rehman Latif, Date: March 29th, 2016
                    //Pending
                    var linkDocument = "";
                    if (item.LinkDocument && item.LinkDocument > 0) {
                        linkDocument = '<a href="#" onclick="Patient_Document.attachedDocumentsRowsExpand(this,event,\'' + item.PatDocId + '\',1);" class="tab_space" title="Expand/Collapse Record"><i id="CollapaseExpandAttachedDocs" class="fa fa-plus-square"></i></a>';
                    }
                    if (Patient_Document.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        var Checked = "";
                        if (item.IsAttachedWithNote == "1") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.PatDocId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                            $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" ' + Checked + ' /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="" title="Attach"><i class="fa fa-paperclip"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedByName + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td data-toggle=tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                        }
                        else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.PatDocId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                            $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" ' + Checked + ' /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedByName + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td data-toggle=tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                        }

                        Patient_Document.createFileTypesArray(item);
                    }
                    else if (Patient_Document.params["ParentCtrl"] == "Batch_FaxSend") {
                        var Checked = "";
                        var disabled = "";
                        if (Patient_Document.FaxDocsArray.length != 0 && $.inArray(item.PatDocId + "", Patient_Document.FaxDocsArray) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                        if (Patient_Document.AttachedDocsArray.length != 0 && $.inArray(item.PatDocId + "", Patient_Document.AttachedDocsArray) > -1) {
                            $row.addClass('disableAll');
                        }
                        else {
                            $row.removeClass('disableAll');
                        }
                        $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" ' + Checked + ' /></td><td hidden><a class="btn  btn-xs disableAll" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + item.PatDocId + ');"> <i class="fa fa-history blue"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedByName + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" /></td><td>' + linkDocument + '<a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + item.PatDocId + ');"> <i class="fa fa-history blue"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedByName + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                    }
                    //End: Active Inactive button added in the grid as per issue number PMS-2835, author Abdur Rehman Latif, Date: March 29th, 2016
                    //$row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(' + item.PatientId + '  ,  ' + item.PatDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectDocument + '</td><td>' + item.DocumentName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td class="ellip100"  title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>');


                    $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument + " tbody").last().append($row);
                    //var $row2 = $('<tr/>').addClass("childRow-bg");
                    //$row2.append('<td class="hidden"></td>  <td class="hidden"></td><td colspan="16"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
                    //$("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument + " tbody").last().append($row2);
                }

                //else {

                //    //Reviewed
                //    $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(' + item.PatientId + '  ,  ' + item.PatDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectDocument + '</td><td>' + item.DocumentName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td  class="ellip100"  title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td  class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');

                //    $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocumentReviewed tbody").last().append($row);
                //}


            });
            var pendingRows = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument + " tbody").find("tr");
            //var ReviewedRows = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocumentReviewed tbody").find("tr");
            if (pendingRows.length < 1) {
                $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging").css("display", "none");
                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.GridPatientDocument).DataTable({
                    "language": {
                        "emptyTable": "No Pending Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "orderable": false, "aTargets": [1, 2] }]
                });
            }
            //if (ReviewedRows.length < 1) {
            //    $('#' + Patient_Document.params["PanelID"] + ' #dgvPatientDocumentReviewed').DataTable({
            //        "language": {
            //            "emptyTable": "No Reviewed Document Found"
            //        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            //    });
            //}

            //------------Pagination Pending Documents-----------
            $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.PendingCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Patient_Document.params["PanelID"] + " #divPendingPaging", response.PendingCount, 5, "Patient_Document", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.PendingCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.PendingCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.PendingCount + " Record(s)";
            $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #divPendingPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------
        }
        else {
            $("#" + Patient_Document.params["PanelID"] + " #divPendingPaging").css("display", "none");
            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.GridPatientDocument).DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Pending Document Found"
                }, "autoWidth": false, "bFilter": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "orderable": false, "aTargets": [1, 2] }]
            });
            //$('#' + Patient_Document.params["PanelID"] + ' #dgvPatientDocumentReviewed').DataTable({
            //    "language": {
            //        "emptyTable": "No Reviewed Document Found"
            //    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            //});
        }

        if ($.fn.dataTable.isDataTable('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.GridPatientDocument))
            ;
        else
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument + "").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1, 2] }] }); // to remove records per page dropdown

        EMRUtility.fixDataTableDuplication("#" + Patient_Document.params.PanelID + " #Pending");
        //if ($.fn.dataTable.isDataTable('#' + Patient_Document.params["PanelID"] + ' #dgvPatientDocumentReviewed'))
        //    ;
        //else
        //    $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatientDocumentReviewed").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        // display badge info
    },

    ShowHistory: function (DocID) {

        var params = [];
        params["DocID"] = DocID;

        if (Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "Batch_FaxSend") {
            params["ParentCtrl"] = "Patient_Document";
            params["ParentCtrlPanelID"] = Patient_Document.params.PanelID;
            LoadActionPan('Patient_Documents_Audit', params, Patient_Document.params.PanelID);

        } else {
            params["ParentCtrl"] = Patient_Document.params["TabID"];
            LoadActionPan('Patient_Documents_Audit', params);
        }
    },


    rowHistory: function (DocID) {
        if (event != null) {
            event.stopPropagation();
        }
        var currentNotesId = DocID != null ? DocID : -1;
        if (currentNotesId > 0) {
            Patient_Document.ShowHistory(DocID);
        }
    },

    attachedDocumentsRowsExpand: function ($row, event, id, doctype) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }
        $row = $($row).parent().parent();
        var tr = $(this).closest('tr');


        EditableGridAttachedDocs = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridPatientDocument).DataTable({
            "destroy": true,
            "aaSorting": [],
            "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false
        });
        var row = EditableGridAttachedDocs.row(tr);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if ($row.find("#CollapaseExpandAttachedDocs").hasClass("fa-minus-square")) {
            // This row is already open - close it
            $row.find("#CollapaseExpandAttachedDocs").attr("class", "fa fa-plus-square");
            row.child.hide();
            $(".tbl_attachedDoc_" + id).remove();
            //tr.removeClass('shown');
        }
        else {
            $row.find("#CollapaseExpandAttachedDocs").attr("class", "fa fa-minus-square");
            Patient_Document.SearchLinkedDocument(id).done(function (response) {
                if (response.status) {
                    $($row).after(Patient_Document.AppendChildRows(response, id, doctype));
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //row.child(Patient_Document.format()).show();
            //$('#my_table > tbody > tr').eq(i - 1).after(html);
        }
    },

    AppendChildRows: function (response, id, doctype) {
        var DocumentLoad_JSONData = JSON.parse(response.DocumentLoad_JSON);
        var $tblRowChild = "";
        var colspanrow = 0;
        if (doctype == 1) {
            colspanrow = 13;
        } else if (doctype == 2) {
            colspanrow = 15;
        }
        $.each(DocumentLoad_JSONData, function (i, item) {
            $tblRowChild += '<tr id=dgvPatientDocument_Linked_row' + item.PatDocId + ' DocumentId=' + item.PatDocId + ' FileType=' + item.FileType + ' class=tbl_attachedDoc_' + id + '>';

            if (item.IsActive == "True") {
                isactive = 0;
                activeTitle = "Active Record";
                tglclass = "fa fa-toggle-on green";
            }
            else {
                isactive = 1;
                activeTitle = "Inactive Record";
                tglclass = "fa fa-toggle-on red";
            }
            var DocPriority = "";
            if (item.DocPrioirty) {
                if (item.DocPrioirty.toLowerCase().trim() == "low") {
                    DocPriority = '<span class=green bold>' + item.DocPrioirty + '</span>';
                }
                else if (item.DocPrioirty.toLowerCase().trim() == "medium") {
                    DocPriority = '<span class=dark-yellow bold>' + item.DocPrioirty + '</span>';
                } else if (item.DocPrioirty.toLowerCase().trim() == "high") {
                    DocPriority = '<span class=red bold>' + item.DocPrioirty + '</span>';
                }
            }
            if (doctype == 1) {
                $tblRowChild += '<td style="display:none;">' + item.PatDocId + '</td>';
                $tblRowChild += '<td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" /></td>';
                $tblRowChild += '<td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + id + '\', event, false, ' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>';
                $tblRowChild += '<td>' + item.AccountNumber + "-" + item.PatientName + '</td>';
                $tblRowChild += '<td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td>';
                $tblRowChild += '<td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td>';
                //  $tblRowChild += '<td>' + item.DocumentName + '</td>';
                $tblRowChild += '<td class="ellip100 bold"  title="' + item.FilePath + '">' + item.FilePath + '<br /><span class="blue">(Linked)</span></td>';
                // $tblRowChild += '<td>' + item.Pages + '</td><td class="bold">' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>'
                $tblRowChild += '<td>' + item.Pages + '</td>';
                $tblRowChild += '<td>' + item.CreatedByName + '</td>';
                $tblRowChild += '<td>' + item.ViewBy + '</td>';
                $tblRowChild += '<td>' + DocPriority + '</td>';
                $tblRowChild += '<td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>';
                $tblRowChild += '</tr>';
            } else {
                $tblRowChild += '<td style="display:none;">' + item.PatDocId + '</td>';
                $tblRowChild += '<td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" /></td>';
                $tblRowChild += '<td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + id + '\', event, false, ' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>';
                $tblRowChild += '<td>' + item.AccountNumber + "-" + item.PatientName + '</td>';
                $tblRowChild += '<td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td>';
                $tblRowChild += '<td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td>';
                // $tblRowChild += '<td>' + item.DocumentName + '</td>';
                $tblRowChild += '<td class="ellip100 bold"  title="' + item.FilePath + '">' + item.FilePath + '<br /><span class="blue">(Linked)</span></td>';
                //  $tblRowChild += '<td>' + item.Pages + '</td><td class="bold">' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>'
                $tblRowChild += '<td>' + item.Pages + '</td>';
                $tblRowChild += '<td>' + item.CreatedByName + '</td>';
                $tblRowChild += '<td>' + item.ViewBy + '</td>';
                $tblRowChild += '<td>' + DocPriority + '</td>';
                $tblRowChild += '<td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td>'
                $tblRowChild += '<td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>';
                $tblRowChild += '</tr>';
            }
        });
        //$tblRowChild += '</table></td></tr>';
        return $tblRowChild;
    },

    AppendChildRows1: function (response, id) {
        var DocumentLoad_JSONData = JSON.parse(response.DocumentLoad_JSON);
        //var $tblRowChild = $('<tr/>');
        //$tblRowChild.attr("id", "tbl_attachedDoc_'" + id);
        //$tblRowChild.append('</td>').attr("colspan", 16);
        //$tblRowChild.append('<table>');
        var $tblRowChild = '<tr id=tbl_attachedDoc_' + id + '>';
        //$tblRowChild.attr("id", "tbl_attachedDoc_'" + id);
        $tblRowChild += '<td colspan="16">';
        $tblRowChild += '<table class="table table-bordered table-striped table-condensed table-hover" cellspacing="0" border="0">';
        $.each(DocumentLoad_JSONData, function (i, item) {
            $tblRowChild += '<tr id=dgvPatientDocument_Linked_row' + item.PatDocId + ' DocumentId=' + item.PatDocId + ' FileType=' + item.FileType + '>';

            //$row.attr("id", "dgvPatientDocument_Linked_row" + item.PatDocId);
            //$row.attr("DocumentId", item.PatDocId);
            //$row.attr("FileType", item.FileType);

            if (item.IsActive == "True") {
                isactive = 0;
                activeTitle = "Active Record";
                tglclass = "fa fa-toggle-on green";
            }
            else {
                isactive = 1;
                activeTitle = "Inactive Record";
                tglclass = "fa fa-toggle-on red";
            }
            var DocPriority = "";
            if (item.DocPrioirty) {
                if (item.DocPrioirty.toLowerCase().trim() == "low") {
                    DocPriority = '<span class=green bold>' + item.DocPrioirty + '</span>';
                }
                else if (item.DocPrioirty.toLowerCase().trim() == "medium") {
                    DocPriority = '<span class=dark-yellow bold>' + item.DocPrioirty + '</span>';
                } else if (item.DocPrioirty.toLowerCase().trim() == "high") {
                    DocPriority = '<span class=red bold>' + item.DocPrioirty + '</span>';
                }
            }
            $tblRowChild += '<td style="display:none;">' + item.PatDocId + '</td>';
            $tblRowChild += '<td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" /></td>';
            $tblRowChild += '<td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>';
            $tblRowChild += '<td>' + item.AccountNumber + "-" + item.PatientName + '</td>';
            $tblRowChild += '<td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td>';
            $tblRowChild += '<td>' + item.DocumentName + '</td>';
            $tblRowChild += '<td class="ellip100"  title="' + item.FilePath + '">' + item.FilePath + '</td>';
            $tblRowChild += '<td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>'
            $tblRowChild += '<td>' + item.CreatedByName + '</td>';
            $tblRowChild += '<td>' + item.ViewBy + '</td>';
            $tblRowChild += '<td>' + DocPriority + '</td>';
            $tblRowChild += '<td data-toggle="tooltip" class="ellip100" data-placement="left" title="' + item.Comments + '"  >' + item.Comments + '</td>';
            $tblRowChild += '</tr>';
        });

        $tblRowChild += '</table></td></tr>';
        return $tblRowChild;
    },

    SelectDocument: function (event, obj, PatDocId, showPassword, IsLocked) {
        if (event != null) {
            event.stopPropagation();
        }
        if (showPassword != "True" && IsLocked == "1") {
            $(obj).prop("checked", false);
            return utility.DisplayMessages("Please unlock private document(s) in order to proceed.", 2);
        }
        if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote") {
            var id = PatDocId;
            if ($(obj).prop("checked")) {
                if ($.inArray(id, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                    Clinical_ProgressNote.AttachedNoteComponentIds.push(id);
                } if ($.inArray(id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                    var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(id);
                    if (index > -1) {
                        Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                    }
                }
            }
            else {
                var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(id);
                if (index > -1) {
                    Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
                }
                if ($.inArray(id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.push(id);
                }
            }

            if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
                $("#" + Patient_Document.params.PanelID + "  #btnAddToNote").prop('disabled', false);
            } else {
                $("#" + Patient_Document.params.PanelID + "  #btnAddToNote").prop('disabled', true);
            }
        }

        if (Patient_Document.params.ParentCtrl == "Batch_FaxSend") {
            var id = PatDocId;
            if ($(obj).prop("checked")) {
                if ($.inArray(id, Patient_Document.FaxDocsArray) == -1) {
                    Patient_Document.FaxDocsArray.push(id);
                }
            }
            else {
                var index = Patient_Document.FaxDocsArray.indexOf(id);
                if (index > -1) {
                    Patient_Document.FaxDocsArray.splice(index, 1);
                }
            }

            if (Patient_Document.FaxDocsArray.length > 0) {
                $("#" + Patient_Document.params.PanelID + "  #btnAddToFax").prop('disabled', false);
            } else {
                $("#" + Patient_Document.params.PanelID + "  #btnAddToFax").prop('disabled', true);
            }
        }
    },

    ReviewedDocumentGridLoad: function (response, PageNo, rpp) {
        if (Patient_Document.params.GridRevDocument == "" || Patient_Document.params.GridRevDocument == null) {
            Patient_Document.params.GridRevDocument = Patient_Document.params.GridRevDocument;
        }
        $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridRevDocument + " #chkMasterPatDoc").prop("checked", false);
        //  $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridRevDocument).dataTable().fnDestroy();
        //$("#" + Patient_Document.params["PanelID"] + " #dgvPatientDocumentReviewed").dataTable().fnDestroy();

        if ($.fn.dataTable.isDataTable("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridRevDocument)) {
            $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridRevDocument).dataTable().fnClearTable();
            $("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.GridRevDocument).dataTable().fnDestroy();
        }

        $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + " tbody").find("tr").remove();

        //var PendingDivHTML = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result div#Pending").html();
        //var objDivReviewed = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result div#Reviewed");

        //objDivReviewed.empty().append(PendingDivHTML);
        //objDivReviewed.find("#PatientDocument_SelectedDataKeys").attr("id", "PatientDocumentReviewed_SelectedDataKeys");
        //objDivReviewed.find("#dgvPatientDocument").attr("id", "dgvPatientDocumentReviewed");

        //default open tab
        //Patient_Document.SetGrid("Reviewed");

        if (response.ReviewedCount > 0) {
            var ReviewedDocumentLoad_JSONData = response.ReviewedDocumentLoad_JSON;
            $.each(ReviewedDocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                var linkDocument = "";
                if (item.LinkDocument && item.LinkDocument > 0) {
                    linkDocument = '<a href="#" onclick="Patient_Document.attachedDocumentsRowsExpand(this,event,\'' + item.PatDocId + '\',2);" class="tab_space" title="Expand/Collapse Record"><i id="CollapaseExpandAttachedDocs" class="fa fa-plus-square"></i></a>';
                }
                if (Patient_Document.params["ParentCtrl"] != "clinicalTabProgressNote") {
                    $row.attr("onclick", "Patient_Document.DocumentEdit('" + item.PatientId + "','" + item.PatDocId + "',event);utility.SelectGridRow($(this))");
                }
                if (Patient_Document.params["ParentCtrl"] == "Batch_FaxSend") {
                    $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument).find('#ColumnAction').addClass('hidden');
                }
                $row.attr("id", "dgvPatRevDocument_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);
                $row.attr("Patientid", item.PatientId);
                $row.attr("FileType", item.FileType);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var DocPriority = "";
                if (item.DocPrioirty.toLowerCase().trim() == "low") {
                    DocPriority = '<span class=green bold>' + item.DocPrioirty + '</span>';
                }
                else if (item.DocPrioirty.toLowerCase().trim() == "medium") {
                    DocPriority = '<span class=dark-yellow bold>' + item.DocPrioirty + '</span>';
                } else if (item.DocPrioirty.toLowerCase().trim() == "high") {
                    DocPriority = '<span class=red bold>' + item.DocPrioirty + '</span>';
                }
                var FileNameWithIcon = "";
                if (item.IsLocked == 1 && item.LinkDocument && item.LinkDocument == 0) {
                    FileNameWithIcon = "<div class='ellipses size100'><i class='fa fa-lock blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp;" + item.FilePath + "</div>"
                }
                else if (item.IsLocked == 0 && item.LinkDocument && item.LinkDocument > 0) {
                    FileNameWithIcon = "<div class='ellipses size100'><i class='fa fa-paperclip blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp;" + item.FilePath + "</div>"
                }
                else if (item.IsLocked == 1 && item.LinkDocument && item.LinkDocument > 0) {
                    FileNameWithIcon = "<div class='ellipses size100'><i class='fa fa-lock  blue' style='font-size:15px;'></i>  <i class='fa fa-paperclip blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + item.FilePath + "</div>"
                }
                else {
                    FileNameWithIcon = "<div class='ellipses size100'> " + item.FilePath + "</div>"
                }
                var selectDocument = "";

                if (item.ReviewDate != "") {

                    //Reviewed

                    if (Patient_Document.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        var Checked = "";
                        if (item.IsAttachedWithNote == "1") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.PatDocId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                            $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" ' + Checked + '/></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="" title="Attach"><i class="fa fa-paperclip"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + item.PatDocId + ');"> <i class="fa fa-history blue"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td   title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td  class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                        }
                        else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.PatDocId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                            $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" ' + Checked + '/></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + item.PatDocId + ');"> <i class="fa fa-history blue"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td    title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td  class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                        }

                        Patient_Document.createFileTypesArray(item);
                    }
                    else if (Patient_Document.params["ParentCtrl"] == "Batch_FaxSend") {
                        var Checked = "";
                        if (Patient_Document.FaxDocsArray.length != 0 && $.inArray(item.PatDocId + "", Patient_Document.FaxDocsArray) > -1) {
                            Checked = " checked";
                        } else {
                            Checked = "";
                        }
                        if (Patient_Document.AttachedDocsArray.length != 0 && $.inArray(item.PatDocId + "", Patient_Document.AttachedDocsArray) > -1) {
                            $row.addClass('disableAll');
                        }
                        else {
                            $row.removeClass('disableAll');
                        }

                        $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" ' + Checked + '/></td><td hidden><a class="btn  btn-xs disableAll" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + item.PatDocId + ');"> <i class="fa fa-history blue"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td    title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td  class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');

                    }
                    else {
                        $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" /></td><td>' + linkDocument + '<a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + item.PatDocId + ');"> <i class="fa fa-history blue"></i></a>' + selectDocument + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ExpiryDate) + '</td><td    title="' + item.FilePath + '">' + FileNameWithIcon + '</td><td>' + item.Pages + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + DocPriority + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td  class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                    }

                    //$row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(' + item.PatDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(' + item.PatientId + '  ,  ' + item.PatDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectDocument + '</td><td>' + item.DocumentName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td  class="ellip100"  title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + item.ViewBy + '</td><td>' + item.ReviewBy + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReviewDate) + '</td><td>' + item.SignBy + '</td><td  class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');

                    $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + " tbody").last().append($row);

                    //var $row2 = $('<tr/>').addClass("childRow-bg");
                    //$row2.append('<td class="hidden"></td>  <td class="hidden"></td><td colspan="16"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');

                    //$row.append($row2);

                }


            });

            var ReviewedRows = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + " tbody").find("tr");

            if (ReviewedRows.length < 1) {
                $("#" + Patient_Document.params["PanelID"] + " #divReviewedPaging").css("display", "none");
                $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.GridRevDocument).DataTable({
                    "language": {
                        "emptyTable": "No Reviewed Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "orderable": false, "aTargets": [1, 2] }]
                });
            }
            //------------Pagination Reviewed Documents-----------
            $("#" + Patient_Document.params["PanelID"] + " #divReviewedPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.ReviewedCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Patient_Document.params["PanelID"] + " #divReviewedPaging", response.ReviewedCount, 5, "Patient_Document", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.ReviewedCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.ReviewedCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.ReviewedCount + " Record(s)";
            $("#" + Patient_Document.params["PanelID"] + " #divReviewedPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #divReviewedPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------
        }
        else {
            $("#" + Patient_Document.params["PanelID"] + " #divReviewedPaging").css("display", "none");
            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.GridRevDocument).DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Reviewed Document Found"
                }, "autoWidth": false, "bFilter": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "orderable": false, "aTargets": [1, 2] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.GridRevDocument))
            ;
        else
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #" + Patient_Document.params.GridRevDocument + "").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1, 2] }] });


        EMRUtility.fixDataTableDuplication("#" + Patient_Document.params.PanelID + " #Reviewed");
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    AttachedDocumentGridLoad: function (AttachedDocuments, PageNo, rpp) {

        $("#" + Patient_Document.params["PanelID"] + " #dgvPatAttachedDocument #chkMasterPatDoc").prop("checked", false);
        $("#" + Patient_Document.params["PanelID"] + " #dgvPatAttachedDocument").dataTable().fnDestroy();
        $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatAttachedDocument tbody").find("tr").remove();


        if (AttachedDocuments.length > 0) {
            $.each(AttachedDocuments, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Patient_Document.DocumentEdit('" + item.PatientId + "','" + item.PatDocId + "',event);utility.SelectGridRow($(this))");
                $row.attr("id", "dgvPatAttachDocument_row" + item.PatDocId);
                $row.attr("DocumentId", item.PatDocId);
                $row.attr("FileType", item.FileType);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var selectDocument = "";
                var status = "";
                if (item.Reviewed == "1")
                    status = "Reviewed";
                else if (item.Pending == "1")
                    status = "Pending";

                $row.append('<td style="display:none;">' + item.PatDocId + '</td><td><input type="checkbox" id="chkPatDoc' + item.PatDocId + '" onclick="Patient_Document.SelectDocument(event,this)" /></td><td><a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + item.PatDocId + '\',event,true);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Document.DocumentEdit(\'' + item.PatientId + '\'  ,  \'' + item.PatDocId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + item.PatDocId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectDocument + '</td><td>' + item.DocumentName + '</td><td>' + item.AccountNumber + "-" + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td  class="ellip100" title="' + item.FilePath + '">' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td><td>' + status + '</td><td data-toggle=tooltip" data-placement="left" class="ellip100" title="' + item.Comments + '"  >' + item.Comments + '</td>');
                $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatAttachedDocument tbody").last().append($row);

            });

            var AttachedRows = $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatAttachedDocument tbody").find("tr");

            if (AttachedRows.length < 1) {
                $('#' + Patient_Document.params["PanelID"] + ' #dgvPatAttachedDocument').DataTable({
                    "language": {
                        "emptyTable": "No Attached Document Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
        }
        else {
            $('#' + Patient_Document.params["PanelID"] + ' #dgvPatAttachedDocument').DataTable({
                "language": {
                    "emptyTable": "No Attached Document Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Patient_Document.params["PanelID"] + ' #dgvPatAttachedDocument'))
            ;
        else
            $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #dgvPatAttachedDocument").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    AttachDocumentToNote: function () {

        var SelectedDocs = Clinical_ProgressNote.AttachedNoteComponentIds.slice();

        var detachedvalues = Clinical_ProgressNote.DetachedNoteComponentIds;
        if (detachedvalues.join() != '') {
            Patient_Document.detachDocumentsFromNote(detachedvalues).done(function () {
                if (SelectedDocs.join() != null && SelectedDocs.join() != '') {
                    Patient_Document.addDocumentToNote(SelectedDocs);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Images').done(function () {
                        Patient_Document.UnLoadTab();
                    });
                }
            });
        } else if (SelectedDocs.join() != null && SelectedDocs.join() != '') {
            Patient_Document.addDocumentToNote(SelectedDocs);
        }

    },

    addDocumentToNote: function (PatDocIds) {


        // var PatDocIds = Patient_Document.SelectedDocs.join(',');
        Patient_Document.SelectedDocs = [];
        if (PatDocIds == "") {
            utility.DisplayMessages("Please select any document to attach.", 4);
            return false;
        }
        else {
            Patient_Document.AttachDocumentToNote_DBCall(PatDocIds).done(function (response) {
                if (response.status != false) {
                    utility.VerifyMUAlert("Patient Document", "", Clinical_Notes.params.patientID, false, 'MU3');
                    //utility.DisplayMessages(response.Message, 1);
                    $("#" + Clinical_Notes.params.PanelID + " #noteatch_i").addClass("fa-paperclip");
                    var imagesPatDocIds = Patient_Document.getImagesPatDocIds(PatDocIds.join(','));
                    if (imagesPatDocIds) {
                        Patient_Document.FillData(imagesPatDocIds, response.NoteDocumentId);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 1);
                        Patient_Document.UnLoadTab();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }

            });
        }

    },

    FillData: function (PatDocIds, noteDocumentId) {
        var self = $('#' + Patient_Document.params.PanelID);
        var documentCall = Patient_Document.FillDocuments(null, Patient_Document.params.PatientID, PatDocIds);
        $.when(documentCall).done(function (response) {
            if (response.status != false) {
                var document_details = JSON.parse(response.DocumentLoad_JSON);
                $(document_details).each(function (index, item) {
                    var FileType = item.FileType;
                    if (FileType.indexOf("image") > -1) {
                        var base64stream = "data:" + FileType + ";base64," + item.Base64FileStream;
                        Patient_Document.CreateHTMLProgressNoteImages(item.PatDocId, base64stream);
                        // Patient_Document.CreateHTMLProgressNoteImages(noteDocumentId, base64stream);                        
                    }
                });
                Clinical_ProgressNote.saveComponentSOAPText('Images').done(function (response) {
                    Patient_Document.UnLoadTab();
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    FillDocuments: function (DocumentData, PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=1";
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_DOCUMENTS");
    },
    FillDocumentsMerged: function (DocumentData, PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=1";
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_DOCUMENTS_MERGED");
    },
    AttachDocumentToNote_DBCall: function (PatDocIds) {
        var data = "DocumentIDs=" + PatDocIds + "&NotesId=" + Patient_Document.params["NotesId"];
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT_IMAGE_ANNOTATION", "ATTACH_PATIENT_DOCUMENT_TO_NOTE");
    },


    detachDocumentsFromNote: function (detachValues) {
        var def = $.Deferred();
        var selectedValue = '';
        if (detachValues.indexOf("Cli_Images_Main") > -1) {
            selectedValue = detachedvalues.replace('Cli_Images_Main', '');
        }
        else {
            selectedValue = detachValues.join(",");
        }

        if (selectedValue == "" || selectedValue == "undefined") {
            Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            Patient_Document.detachImagesComponentFromNotes_DBCall(selectedValue).done(function (response) {
                if (response.status != false) {
                    Clinical_ProgressNote.HideShowBillingInfo();
                    var PLid = selectedValue.split(',');
                    for (var i = 0; i < PLid.length; i++) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Images_Main' + PLid[i]).remove();
                    }
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    utility.DisplayMessages('Successfully Saved', 1);
                    Clinical_ProgressNote.updateAttachDocumentButtonImg();
                    def.resolve();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    def.resolve();
                }
            });
        }
        return def;
    },

    detachImagesComponentFromNotes_DBCall: function (clinicalImagesIds) {
        var data = "PatientDocumentIds=" + clinicalImagesIds + "&NotesId=" + Patient_Document.params["NotesId"];
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT_IMAGE_ANNOTATION", "DETACH_PATIENT_DOCUMENTS_FROM_NOTE");
    },

    //Add to Note START

    checkImagesExists: function (NoteDocumentId) {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ImagesComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_images title="Images"  id="' + NoteDocumentId + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Images\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Images">Images</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Images\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_images> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    CreateHTMLProgressNoteImages: function (patDocId, base64FileStream) {

        Patient_Document.checkImagesExists(patDocId);

        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        var file_base64 = base64FileStream;

        var $image_ = document.createElement("IMG");
        $image_.setAttribute("src", file_base64);
        $image_.setAttribute("width", "175");
        $image_.setAttribute("height", "150");
        $image_.setAttribute("alt", "Note attchment image");

        var $mainDivImages = $(document.createElement('div'));
        var $sectionBodyImages = $(document.createElement('section'));
        var $detailsDiv = $(document.createElement('div'));
        var $listImages = $(document.createElement('ul'));
        $detailsDiv.attr('id', "Cli_Images_" + patDocId);

        $sectionBodyImages.attr('id', "Cli_Images_Main" + patDocId);
        $sectionBodyImages.attr('id_', "" + patDocId + "");
        $sectionBodyImages.css('margin-bottom', "5px");


        $sectionBodyImages.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Images_" + patDocId + '"><i class="fa fa-edit"></i></a>' +
                   '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Images_Main" + patDocId + '"  ><i class="fa fa-times"></i></a></div>');

        $listImages.append($image_);

        $detailsDiv.append($listImages);

        $sectionBodyImages.append($detailsDiv);

        $mainDivImages.append($sectionBodyImages);
        $mainDivImages.html()

        $(noteHTMLCtrl + ' clinical_images').parent().parent().addClass('initialVisitBody');

        if ($(noteHTMLCtrl).find("#Cli_Images_Main" + patDocId).length > 0) {
            $(noteHTMLCtrl).find("#Cli_Images_Main" + patDocId).html($sectionBodyImages.html());
        }
        else {
            $(noteHTMLCtrl + ' clinical_images').parent().parent().append($mainDivImages.html());
        }
    },

    createFileTypesArray: function (docItem) {
        var doc = $.grep(Patient_Document.FileTypesArray, function (item, index) {
            return item.PatDocId == docItem.PatDocId
        });
        if (doc.length == 0) {
            var file = {
                PatDocId: docItem.PatDocId,
                FileType: docItem.FileType
            };

            Patient_Document.FileTypesArray.push(file);
        }
    },
    getImagesPatDocIds: function (patDocIds) {
        var docIds = patDocIds.split(",");
        var imagesDocIds = [];

        var selectedDiv = $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result li.active a").attr("href");
        var GridId = "";
        if (selectedDiv == "#Pending") {
            GridId = " #" + Patient_Document.params.GridPatientDocument;
        }
        else if (selectedDiv == "#Reviewed") {
            GridId = " #" + Patient_Document.params.GridRevDocument;
        }

        for (var i = 0; i < docIds.length; i++) {

            var doc = $.grep(Patient_Document.FileTypesArray, function (item, index) {
                return item.PatDocId == docIds[i]
            });

            if (doc[0].FileType && doc[0].FileType.indexOf('image') >= 0) {
                imagesDocIds.push(docIds[i]);
            }
        }
        return imagesDocIds.join(',');
    },
    //Add to Note END

    DetachDocumentToNote_DBCall: function (DocumentID) {

        var data = "DocumentID=" + DocumentID + "&NotesId=" + Patient_Document.params["NotesId"];
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT_IMAGE_ANNOTATION", "DETACH_PATIENT_DOCUMENT_TO_NOTE");

    },

    ActiveInactivePatientDocument: function (PatientDocumentId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PatientDocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var patientId = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                        Patient_Document.UpdateDocumentActiveInactive(patientId, selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Document.DocumentSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { }, '3', null, null, null, IsActive);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchDocument: function (DocumentData, PatientID, PageNumber, RowsPerPage, IsReviewed) {
        if (PatientID == "" || PatientID == null) {
            Patient_Document.params["ParentCtrl"] == "Clinical_FaceSheet" ? PatientID = Clinical_FaceSheet.params.patientID : PatientID = 0;
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var NoteId = 0;
        if (Patient_Document.params.NotesId)
            NoteId = Patient_Document.params.NotesId;

        var data = "PatientDocumentData=" + DocumentData + "&PatientID=" + PatientID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&IsReviewed=" + IsReviewed + "&NoteId=" + NoteId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_PATIENT_DOCUMENT");
    },

    SearchLinkedDocument: function (patDocId) {
        var data = "MedicalParentDocId=" + patDocId;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_PATIENT_LINKED_DOCUMENT");
    },

    DeleteDocument: function (DocumentID, ChildPatientDocId, LinkDocumentId) {
        var data = "DocumentID=" + DocumentID + "&ChildPatientDocId=" + ChildPatientDocId + "&LinkDocumentId=" + LinkDocumentId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DELETE_PATIENT_DOCUMENT");
    },
    GetPatientDocument: function (DocumentData, PatientID) {
        var data = "PatientID=" + PatientID;
        if (PatientID == "" || PatientID == null) {
            Patient_Document.params["ParentCtrl"] == "Clinical_FaceSheet" ? PatientID = Clinical_FaceSheet.params.patientID : PatientID = 0;
        }
        var NoteId = 0;
        if (Patient_Document.params.NotesId)
            NoteId = Patient_Document.params.NotesId;

        var data = "PatientDocumentData=" + DocumentData + "&PatientID=" + PatientID + "&NoteId=" + NoteId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "GET_PATIENT_DOCUMENT");
    },

    UpdateDocument: function (PatientID, DocumentID) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT");
    },

    UpdateDocumentActiveInactive: function (PatientID, DocumentID, IsActive) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "DocumentID=" + DocumentID + "&IsActive=" + IsActive + "&PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT_ACTIVE_INACTIVE");
    },

    DocumentExport: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var DocIDs, checkboxes;
                if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible")) {
                    DocIDs = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
                }
                else {
                    checkboxes = $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument tbody input[id*="chkPatDoc"]:checked');
                    if (checkboxes.length == 0) {
                        utility.DisplayMessages("Please select  any document(s) to Export.", 2);
                        return;
                    } else {
                        var DocIDs = checkboxes.map(function () {
                            return this.id.replace("chkPatDoc", "");
                        }).get().join(',');
                    }
                }
                var param = [];
                if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                    param["patientId"] = Patient_Document.params["PatientId"];
                } else {
                    param["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                }
                //param["patientId"] = $("#" + Patient_Document.params["PanelID"] + " #hfPatientId").val();
                param["PatDocIDs"] = DocIDs;
                param["mode"] = "Export";
                if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                    param["ParentCtrl"] = "Patient_Document";
                    param["PatientDetail"] = "1";
                } else {
                    //params["ParentCtrl"] = Patient_Document.params["TabID"];
                    param["PatientDetail"] = "0";
                }
                if (Patient_Document.params.ParentCtrl == "demographicDetail" && $("#" + demographicDetail.params["PanelID"] + " #btnPatDemo").hasClass('active') == true) {
                    var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
                    LoadActionPan('Document_Export', param, parentPanelId);
                } else {
                    LoadActionPan('Document_Export', param);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SetPatientDocumentPageState: function () {

        if (Patient_Document.params.TabID == "batchTabDocuments") {

            if (Patient_Document.params["PageBatchState"]) {
                if (Patient_Document.params["PageBatchState"].LandingPage == "Scan") {
                    Patient_Document.setScannerDefaultCntrl();
                }
                else {
                    $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
                    $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").hide();
                    $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
                    Patient_Document.ResetPatient();
                }

            }
            else {
                if ($("#" + Patient_Document.params["PanelID"] + " #divPatDocument").css('display') == 'block') {
                    Patient_Document.params["PageBatchState"] = { TabName: "BatchDocument", LandingPage: "Default" };
                }
                else if ($("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").css('display') == 'block') {
                    Patient_Document.params["PageBatchState"] = { TabName: "BatchDocument", LandingPage: "Scan" };
                    Patient_Document.setScannerDefaultCntrl();
                }
                else {
                    Patient_Document.params["PageBatchState"] = { TabName: "BatchDocument", LandingPage: "Default" };
                    $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
                    $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").hide();
                    $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
                    Patient_Document.ResetPatient();
                }
            }
        }
        else if (Patient_Document.params.TabID == "patTabDocuments") {

            if (Patient_Document.params["PagePatientState"]) {
                if (Patient_Document.params["PagePatientState"].LandingPage == "Scan") {
                    Patient_Document.setScannerDefaultCntrl();
                }
                else {
                    $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
                    $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").hide();
                    $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
                    Patient_Document.ResetPatient();
                }

            }
            else {
                if ($("#" + Patient_Document.params["PanelID"] + " #divPatDocument").css('display') == 'block') {
                    Patient_Document.params["PagePatientState"] = { TabName: "PatientDocument", LandingPage: "Default" };
                }
                else if ($("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").css('display') == 'block') {
                    Patient_Document.params["PagePatientState"] = { TabName: "PatientDocument", LandingPage: "Scan" };
                    Patient_Document.setScannerDefaultCntrl();
                }
                else {
                    Patient_Document.params["PagePatientState"] = { TabName: "PatientDocument", LandingPage: "Default" };
                    $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
                    $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").hide();
                    $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
                    Patient_Document.ResetPatient();
                }
            }
        }
        Patient_Document.LoadFolders(true);
    },

    setScannerDefaultCntrl: function () {
        var $ctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan');
        var $hfctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId');
        $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").hide();
        $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").show();
        $("#" + Patient_Document.params["PanelID"] + " #divDocScan").show();
        if (localStorage.SelectedAccountNumber) {
            Document_Scan.ResetControls();
            // if patient is selected in Patient-> document or batch-> document then set form ctrl value and hfPatientId. 
            if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                $ctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                $hfctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val());
                var kAutoComp = $ctr.data("kendoAutoComplete");
                if (kAutoComp) {
                    kAutoComp.value($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(), "FullName");
                }
            }
            else {
                $ctr.val($('#PatientProfile #hfPatientFullNameOnly').val());
                $hfctr.val(localStorage.SelectedPatientId);
                var kAutoComp = $ctr.data("kendoAutoComplete");
                if (kAutoComp) {
                    kAutoComp.value($('#PatientProfile #hfPatientFullNameOnly').val());
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#PatientProfile #hfPatientFullNameOnly').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), localStorage.SelectedPatientId, "FullName");
                }
            }

        }
        Document_Scan.ValidateScanner(Document_Scan.params.PanelID + ' #frmDocumentScan');

        if (Dynamsoft.WebTwainEnv) {
            customViewMode();
        }
    },
    setState: function (IsScan) {

        if (Patient_Document.params.TabID == "patTabDocuments") {
            if (IsScan == true)
                Patient_Document.params["PagePatientState"] = { TabName: "PatientDocument", LandingPage: "Scan" };
            else
                Patient_Document.params["PagePatientState"] = { TabName: "PatientDocument", LandingPage: "Default" };
        }
        else if (Patient_Document.params.TabID == "batchTabDocuments") {
            if (IsScan == true)
                Patient_Document.params["PageBatchState"] = { TabName: "BatchDocument", LandingPage: "Scan" };
            else
                Patient_Document.params["PageBatchState"] = { TabName: "BatchDocument", LandingPage: "Default" };
        }

    },
    DocumentScan: function (event) {
        if (event != null) {
            event.stopPropagation();
        }

        Patient_Document.setState(true);
        Patient_Document.ShowHideMoveButton();
        reSetDefaultValuesForScanCanvas();

        if ($("#" + Patient_Document.params["PanelID"] + " #divDocScan").html() == "") {
            ScannerLoaded = "WIA-fi-7160";
            AppPrivileges.GetFormPrivileges("Documents", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var param = [];
                    param["mode"] = "Scan";
                    param["FromAdmin"] = "0";

                    if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                        param['patientID'] = Patient_Document.params['PatientID']
                        param["PatientDetail"] = "1";
                        param["ParentCtrl"] = 'mstrTabBatch';
                    } else {
                        param["PatientDetail"] = "0";
                        param['patientID'] = Patient_Document.params['patientID']
                    }
                    //LoadActionPan('Document_Scan', param);
                    var CurrentTab = GetCurrentSelectedTab();
                    var ctrl = "Document_Scan";
                    var html;
                    html = utility.getTabHtml(ctrl);
                    var dfd = new $.Deferred();
                    if (html) {
                        dfd.resolve(html);
                    }
                    else {
                        $.get(GetTab(ctrl).Path, {
                            cache: false
                        }, function (content) {
                            html = content;
                            dfd.resolve(html);
                        });
                    }
                    $.when(dfd).then(function () {

                        //if ($("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").length == 0) {

                        $("#" + Patient_Document.params["PanelID"] + " #divDocScan").empty();

                        eval(ctrl + '.bIsFirstLoad=true');

                        try {
                            Dynamsoft.WebTwainEnv ? Dynamsoft.WebTwainEnv = null : null;
                        } catch (e) {
                            console.log(e.message);
                        }

                        $("#" + Patient_Document.params["PanelID"] + " #divDocScan").append(html);
                        if (param != null) {
                            param["PanelID"] = CurrentTab.PanelID;
                            param["ActionPanContainer"] = CurrentTab.ActionPanContainer;
                            param["TabID"] = CurrentTab.TabID;
                        }
                        eval(ctrl + '.Load')(param);
                        $('#' + Patient_Document.params["PanelID"] + ' #btnExport').addClass('hidden');
                        $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').addClass('hidden');
                        $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').addClass('hidden');
                        $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").hide();
                        $("#" + Patient_Document.params["PanelID"] + " #divDocScan").show();
                        $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").show();
                        $($("#" + Patient_Document.params["PanelID"] + " #divDocScan").find(".modal-body")[0]).toggleClass("panel-body");
                        $($("#" + Patient_Document.params["PanelID"] + " #divDocScan").find(".modal-content")[0]).removeClass("modal-content");
                        $($("#" + Patient_Document.params["PanelID"] + " #divDocScan").find(".modal-dialog")[0]).removeClass("modal-dialog modal-dialog-lg modal-dialog-full");
                        $($("#" + Patient_Document.params["PanelID"] + " #divDocScan").find(".modal-header")[0]).addClass("pt-xs pb-xs");
                        $($("#" + Patient_Document.params["PanelID"] + " #divDocScan").find(".close")[0]).attr("onClick", "Patient_Document.UnLoadScanDoc();");
                        $('#' + Patient_Document.params["PanelID"] + ' #btnDocumentScanExit').removeClass('hidden');
                        Patient_Document.AppendMoveDocBtn();

                        // Set Patient Name at Document Scan Screen AST-16.
                        var $ctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan');
                        var $hfctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId');
                        if (localStorage.SelectedAccountNumber) {
                            Document_Scan.ResetControls();
                            // if patient is selected in Patient-> document or batch-> document then set Ctrl value and hfPatientId.
                            if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                                $ctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                                $hfctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val());
                                var kAutoComp = $ctr.data("kendoAutoComplete");
                                if (kAutoComp) {
                                    kAutoComp.value($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                                    utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(), "FullName");
                                }
                            }
                            else {
                                // if patient is not selectd in Patient->Document or Batch Document the patient will be select from Patient Banner
                                $ctr.val($('#PatientProfile #hfPatientFullNameOnly').val());
                                $hfctr.val(localStorage.SelectedPatientId);
                                var kAutoComp = $ctr.data("kendoAutoComplete");
                                if (kAutoComp) {
                                    kAutoComp.value($('#PatientProfile #hfPatientFullNameOnly').val());
                                    utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#PatientProfile #hfPatientFullNameOnly').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), localStorage.SelectedPatientId, "FullName");
                                }
                            }
                            Patient_Document.LoadFolders(true);
                        }
                            // if Patient is not select in banner but select at Batch Dcoument 
                        else {
                            Document_Scan.ResetControls();
                            if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                                $ctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                                $hfctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val());
                                var kAutoComp = $ctr.data("kendoAutoComplete");
                                if (kAutoComp) {
                                    kAutoComp.value($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                                    utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(), "FullName");
                                }
                            }
                            Patient_Document.LoadFolders(true);
                        }
                        return dfd.promise();
                        //     }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            var $ctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan');
            var $hfctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId');
            if (localStorage.SelectedAccountNumber) {
                Document_Scan.ResetControls();
                // if patient is selected in Patient-> document or batch-> document then set Ctrl value and hfPatientId.
                if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                    $ctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                    $hfctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val());
                    var kAutoComp = $ctr.data("kendoAutoComplete");
                    if (kAutoComp) {
                        kAutoComp.value($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(), "FullName");
                    }
                }
                else {
                    $ctr.val($('#PatientProfile #hfPatientFullNameOnly').val());
                    $hfctr.val(localStorage.SelectedPatientId);
                    var kAutoComp = $ctr.data("kendoAutoComplete");
                    if (kAutoComp) {
                        kAutoComp.value($('#PatientProfile #hfPatientFullNameOnly').val());
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#PatientProfile #hfPatientFullNameOnly').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), localStorage.SelectedPatientId, "FullName");
                    }
                }

                Patient_Document.LoadFolders(true);
            }
                // if Patient is not select in banner but select at Batch Dcoument 
            else {
                Document_Scan.ResetControls();
                if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                    $ctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                    $hfctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val());
                    var kAutoComp = $ctr.data("kendoAutoComplete");
                    if (kAutoComp) {
                        kAutoComp.value($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val());
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(), $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), $('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(), "FullName");
                    }
                }
                Patient_Document.LoadFolders(true);
            } // end else
            $('#' + Patient_Document.params["PanelID"] + ' #btnExport').addClass('hidden');
            $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').addClass('hidden');
            $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').addClass('hidden');
            $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").hide();
            $("#" + Patient_Document.params["PanelID"] + " #divDocScan").show();
            $("#" + Patient_Document.params["PanelID"] + " #pnlDocumentScan").show();
            Document_Scan.ValidateScanner(Document_Scan.params.PanelID + ' #frmDocumentScan');
            Patient_Document.AppendMoveDocBtn();
        }

        try {
            if (Dynamsoft.WebTwainEnv) {
                customViewMode()
                // if First time from Batch flow
            }
        }
        catch (ex) {
            console.log(ex);
        }
        // end of Dynamic soft.
    },

    InformationSubmission: function (event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["FolderId"] = $("#" + Patient_Document.params["PanelID"] + " #lstDocument li.active").val();
                if (Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                    param["PatientId"] = Patient_Document.params["PatientId"];
                } else {
                    param["PatientId"] = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                }
                if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet" || Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "demographicDetail" || Patient_Document.params.ParentCtrl == "Patient_Case_Detail" || Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                    param["ParentCtrl"] = "Patient_Document";
                    param["PatientDetail"] = "1";
                } else {
                    //params["ParentCtrl"] = Patient_Document.params["TabID"];
                    param["PatientDetail"] = "0";
                }
                LoadActionPan('Patient_Information_Submission', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FillPatientDocumentExpiryAlert_DbCall: function (PatientId) {
        var data = "PatientId=" + PatientId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_DOCUMENT_EXPIRY_ALERT");
    },

    InsertPatientDocumentExpiryAlert: function (PatDocIds) {
        if (PatDocIds != "") {
            Patient_Document.InsertPatientDocumentExpiryAlert_DbCall(PatDocIds).done(function (response) {
                if (response.status) {
                    //Do something
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    InsertPatientDocumentExpiryAlert_DbCall: function (PatDocIds) {
        var data = "PatDocIds=" + PatDocIds;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "INSERT_PATIENT_DOCUMENT_EXPIRY_ALERT");
    },

    UnLoadTab: function () {
        var parentPanelId = null;
        Patient_Document.FileTypesArray = [];
        Patient_Document.FaxDocsArray = [];
        if (Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {

            if (Patient_Document.params["FromAdmin"] == "0") {
                if (Patient_Document.params != null && Patient_Document.params.ParentCtrl != null) {
                    if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
                        parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                        Clinical_FaceSheet.params.ChildPanelID = null;
                        UnloadActionPan(Patient_Document.params.ParentCtrl, 'Patient_Document', null, parentPanelId);
                    } else {
                        UnloadActionPan(Patient_Document.params.ParentCtrl, 'Patient_Document');
                    }

                }
                else
                    UnloadActionPan(null, 'Patient_Document');
            }
            else {
                RemoveAdminTab();
            }
            Clinical_FaceSheet.loadFaceSheet();
        }
        else {
            if (Patient_Document.params["FromAdmin"] == "0") {
                if (Patient_Document.params != null && Patient_Document.params.ParentCtrl != null) {
                    UnloadActionPan(Patient_Document.params.ParentCtrl, 'Patient_Document');
                    if (Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                        EncounterChargeCapture.ClaimDocumentsSearch();
                    }
                    if (Patient_Document.params.ParentCtrl == "clinicalTabProgressNote" || Patient_Document.params.ParentCtrl == "demographicDetail")
                        Patient_Document.params = [];
                    if (Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                        Patient_Case_Detail.LoadCase();
                    }
                    
                }
                else
                    UnloadActionPan(null, 'Patient_Document');
            }
            else {
                RemoveAdminTab();
            }
        }

    },


    PagerUnload: function () {
        Patient_Document.ParentFormPanelID = "";
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Patient_Document.params.ParentCtrl == "clinicalTabFaceSheet" || Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {

            if (Patient_Document.params["FromAdmin"] == "0") {
                if (Patient_Document.params != null && Patient_Document.params.ParentCtrl != null) {
                    UnloadActionPan(Patient_Document.params.ParentCtrl, 'Patient_Document');
                }
                else
                    UnloadActionPan(null, 'Patient_Document');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('patient_document');


            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Patient_Document.params["FromAdmin"] == "0") {
                if (Patient_Document.params != null && Patient_Document.params.ParentCtrl != null) {
                    UnloadActionPan(Patient_Document.params.ParentCtrl, 'Patient_Document');
                }
                else
                    UnloadActionPan(null, 'Patient_Document');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_Problems").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('patient_document');
        }
        return objDeffered;
    },
    //--------------Pagination functions-----

    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {

        if (pagingDivId.indexOf("divReviewedPaging") >= 0) {
            Patient_Document.DocumentSearch(0, PageNo, 15, '1');
        }
        else if (pagingDivId.indexOf("divPendingPaging") >= 0) {
            Patient_Document.DocumentSearch(0, PageNo, 15, '0');
        }

    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        if (pagingDivId.indexOf("divReviewedPaging") >= 0) {
            var reviewedCurrentPage;
            $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #divReviewedPaging li").each(function () {
                if ($(this).attr("class") == "active") {
                    reviewedCurrentPage = parseInt($(this).text());
                }

            });
            reviewedCurrentPage = reviewedCurrentPage != "" ? reviewedCurrentPage - 1 : "";
            Patient_Document.DocumentSearch(0, reviewedCurrentPage, 15, '1');
        }

        else if (pagingDivId.indexOf("divPendingPaging") >= 0) {
            var pendingCurrentPage;
            $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #divPendingPaging li").each(function () {
                if ($(this).attr("class") == "active") {
                    pendingCurrentPage = parseInt($(this).text());
                }

            });
            pendingCurrentPage = pendingCurrentPage != "" ? pendingCurrentPage - 1 : "";
            Patient_Document.DocumentSearch(0, pendingCurrentPage, 15, '0');
        }

    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        if (pagingDivId.indexOf("divReviewedPaging") >= 0) {
            var reviewedCurrentPage;
            $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #divReviewedPaging li").each(function () {
                if ($(this).attr("class") == "active") {
                    reviewedCurrentPage = parseInt($(this).text());
                }

            });
            reviewedCurrentPage = reviewedCurrentPage != "" ? reviewedCurrentPage + 1 : "";
            Patient_Document.DocumentSearch(0, reviewedCurrentPage, 15, '1');
        }
        else if (pagingDivId.indexOf("divPendingPaging") >= 0) {
            var pendingCurrentPage;
            $('#' + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result #divPendingPaging li").each(function () {
                if ($(this).attr("class") == "active") {
                    pendingCurrentPage = parseInt($(this).text());
                }

            });
            pendingCurrentPage = pendingCurrentPage != "" ? pendingCurrentPage + 1 : "";
            Patient_Document.DocumentSearch(0, pendingCurrentPage, 15, '0');
        }

    },

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = null;
        var parentPanelId = null;
        LoadActionPan('Admin_Practice', params, null);
    },

    LoadAllControls: function () {
        var self = $('#' + Patient_Document.params.PanelID);
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = self.find("#frmPatientDocument #txtPractice");
            var hfCtrl = self.find("#hfPractice");
            var onSelect = function () {
                self.find("#lnkFacilityEdit").css("display", "none");
                self.find("#lblFacility").css("display", "inline");
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Practices, null, hfCtrl, onSelect);
        });
    },

    //---------------End Pagination functions-------

    SetDocumentForFax: function () {

        $("#" + Patient_Document.params["PanelID"] + " #headingTitle").html("Attach Document");
        $("#" + Patient_Document.params["PanelID"] + " .not-for-note").addClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #btnAddToNote").addClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #btnAddToFax").removeClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #docActionsDiv").addClass('hidden');
        $("#" + Patient_Document.params["PanelID"] + " #dialogDiv").removeClass("modal-dialog-full");
        $("#" + Patient_Document.params["PanelID"] + " #dialogDiv").addClass("modal-dialog modal-dialog-full");
        if ($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument div").length > 0)
            $($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument div")[0]).addClass("col-sm-3 col-md-5");
    },

    AttachDocumentToFax: function () {

        var SelectedDocs = Patient_Document.FaxDocsArray;
        if (SelectedDocs && SelectedDocs.length > 0) {
            Patient_Document.AddDocumentsToFax(SelectedDocs);
        }
        else {
            utility.DisplayMessages("Please select any document to attach.", 4);
            return false;
        }
    },

    AddDocumentsToFax: function (SelectedDocs) {

        if (SelectedDocs && SelectedDocs.length > 0) {
            var documentCall = Patient_Document.FillDocumentsForFax(null, Clinical_ProgressNote.params.patientID, SelectedDocs.join(','));
            $.when(documentCall).done(function (response) {
                Patient_Document.FaxDocsArray = [];
                if (response.status != false) {
                    Batch_FaxSend.params["PDFBase64"] = response.MergedContent;
                    Batch_FaxSend.params["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    Batch_FaxSend.ShowMergedFile();
                    for (var i = 0; i < SelectedDocs.length; i++) {
                        Patient_Document.AttachedDocsArray.push(SelectedDocs[i]);
                    }
                    Patient_Document.UnLoadTab();
                    utility.DisplayMessages("Successfully Attached.", 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    SaveAnnotatedDocument:function()
    {
        //var PatDocID = $("#" + Patient_Document.params["PanelID"] + " #hfFolderDocumentId").val();
        //var Base64 = $('#' + Patient_Document.params["PanelID"] + ' #helperPDF').val();
        var data = new FormData();
        var panel = Patient_Document.params["PanelID"];
        if (panel != "pnlBatchDocuments" && panel.indexOf("pnlPatientDocument") <= 0)
            panel = panel + " #pnlPatientDocument";
        data.append("Base64", $('#' + panel + ' #helperPDF').val());
        data.append("PatDocID", $("#" + Patient_Document.params["PanelID"] + " #hfFolderDocumentId").val());

        //var data = "PatDocID=" + PatDocID + "&bFileStream=1" + "&Base64=" + Base64;
        // serach parameter , class name, command name of class
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "SAVE_ANNOTATED_DOCUMENT");

    },

    FillDocumentsForFax: function (DocumentData, PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var attachedFilesStream = (Batch_FaxSend.AnnotateArray["base64"] == null || Batch_FaxSend.AnnotateArray["base64"] == "") ? "" : Batch_FaxSend.AnnotateArray["base64"];
        var data = "PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=1" + "&AttachedFilesStream=" + attachedFilesStream;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_DOCUMENTS_FOR_FAX");
    },

    FillMyComputerDocsForFax: function (data) {
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "FILL_MYCOMPUTER_DOCS_FOR_FAX");
    },

    AddTags: function () {

        var params = [];

        if (Patient_Document.params["TabID"] == "patTabDocuments") {
            params["PanelID"] = "pnlPatientDocument";
        }
        else if (Patient_Document.params["TabID"] == "batchTabDocuments") {
            params["PanelID"] = "pnlBatchDocuments";

        }
        else if (Patient_Document.params["TabID"] == "demographicDetail") {
            params["ParentCtrl"] = "Patient_Document";
            params["PanelID"] = "pnldemographicDetail #pnlPatientDocument";
        }

        else if (Patient_Document.params["TabID"] == "clinicalTabProgressNote") {

            params["PanelID"] = "ctrlPanClinical #pnlClinicalProgressNote #pnlPatientDocument";

        }
        else if (Patient_Document.params["TabID"] == "clinicalTabFaceSheet") {
            Patient_Document.params["PanelID"] = "ctrlPanClinical #pnlClinicalFaceSheet #pnlPatientDocument";
            params["PanelID"] = "ctrlPanClinical #pnlClinicalFaceSheet #pnlPatientDocument";
        }


        if (Patient_Document.params["TabID"] == "demographicDetail" && Patient_Document.params["PanelID"] == "pnldemographicDetail #pnlPatientDocument" && Patient_Document.params["ParentCtrl"] == "demographicDetail") {
            params["TabID"] = Patient_Document.params["TabID"];
            //params["ParentCtrl"] = "Patient_Document";
            Patient_DocumentTag.UnloadParent = "ParentUnload";
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            LoadActionPan('Patient_DocumentTag', params, parentPanelId);
        }
        else if (Patient_Document.params["TabID"] == "clinicalTabProgressNote" && Patient_Document.params["PanelID"] == "ctrlPanClinical #pnlClinicalProgressNote #pnlPatientDocument") {
            params["ParentCtrl"] = "Patient_Document";
            params["TabID"] = Patient_Document.params["TabID"];
            //params["ParentCtrl"] = "Patient_Document";
            Patient_DocumentTag.UnloadParent = "ParentUnload";
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            LoadActionPan('Patient_DocumentTag', params, parentPanelId);
        }
        else if (Patient_Document.params["TabID"] == "clinicalTabFaceSheet" && Patient_Document.params["PanelID"] == "ctrlPanClinical #pnlClinicalFaceSheet #pnlPatientDocument") {
            params["ParentCtrl"] = "Patient_Document";
            params["TabID"] = Patient_Document.params["TabID"];
            Patient_DocumentTag.UnloadParent = "ParentUnload";
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            LoadActionPan('Patient_DocumentTag', params, parentPanelId);
        }
        else {
            params["TabID"] = Patient_Document.params["TabID"];
            LoadActionPan('Patient_DocumentTag', params);
        }


    },

    markUpWithSkip: function (dialogTitle, Required, Clearfix, Password, DocMatchFunction, PatientID, PatDocID, IsFromPrev, IsFromNext) {
        if (IsFromNext == true) {
            var SkipFunction = "Patient_Document.nextDocument('true', " + PatDocID + ");";
        }
        else {
            var SkipFunction = "Patient_Document.previousDocument('true', " + PatDocID + ");";
        }
        var markUp = '';
            markUp = '<div id="modal-from-dom-DocumentConfirmPasswordViewer" class="modal fade">' +
                        '<div class="modal-dialog modal-dialog-smd modal-top-adjust">'
                            + '<div class="modal-content">'
                                + '<div class="modal-header">'
                                    + '<button type="button" onclick="Patient_Document.cancelConfirmDialogViewer();" class="close" "></button>'
                                    + '<h4 class="modal-title">' + dialogTitle + ' </h4>'
                                + '</div>'
                                + '<div class="modal-body bg-white" id="DivConfirmPasswordViewer">'
                                    + '<div class="col-xs-6">'
                                        + '<label class="control-label">Confirm Password' + Required + '</label>' + Password
                                    + '</div>' + Clearfix
                                    + '<div class="form-group"><div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="' + DocMatchFunction + '">Ok</button>'
                                            + '<button id="btnDocumentSkip" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="Patient_Document.cancelConfirmDialogViewer();' + SkipFunction + '">Skip</button>'
                                            + '</div></div></div></div></div></div><div></div>';
            return markUp;
    },

    VerifyPassword: function (PatientID, PatDocID, event, IsFromDashboard, FileType, IsFromViewer, IsFromFolder, PrevDoc, NextDoc) {
        var DivFormGroup = '<div class="form-group">';
        var DivEnd = '</div>'
        var Password = '<input type="password" name="DocPassword" id="TxtDocPassword" class="form-control" autofocus>';
        //var ConfirmPassword = '<input type="password" name="DocConfirmPassword" id="TxtDocConfirmPassword" class="form-control">';
        var dialogTitle = "Confirm Password";
        var Required = '<span class="required">*</span>';
        var Clearfix = '<div class="clearfix"></div>';
        //var Spacer20 = '<div class="spacer20"></div>';
        var ShareAccessCaption = 'Share access of this document by selecting the user(s) below. User(s) selected will receive system generated message containing password.';
        var DocMatchFunction = "Patient_Document.DocPasswordMatch('" + PatientID + "', '" + PatDocID + "', '" + IsFromDashboard + "', '" + FileType + "', '" + IsFromViewer + "', '" + IsFromFolder + "');";
        
        if ((PrevDoc != true && NextDoc == true) || (PrevDoc == true && NextDoc != true)) {
            markUp = Patient_Document.markUpWithSkip(dialogTitle, Required, Clearfix, Password, DocMatchFunction, PatientID, PatDocID, PrevDoc, NextDoc);
        }
        else {
            markUp = '<div id="modal-from-dom-DocumentConfirmPassword" class="modal fade">' +
                            '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                                '<div class="modal-content">'
                                + '<div class="modal-header">' + '<button type="button" onclick="Patient_Document.cancelConfirmDialog();" class="close" "></button>'
                                    + '<h4 class="modal-title">' + dialogTitle + ' </h4>'
                                + DivEnd
                                    + '<div class="modal-body bg-white" id="DivConfirmPassword">'
                                        + '<div class="col-xs-6"><label class="control-label">Confirm Password' + Required + '</label>' + Password + DivEnd + Clearfix
                                        + DivFormGroup
                                            + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                                + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="' + DocMatchFunction + '">Ok</button>'
                                            + DivEnd
                                        + DivEnd
                                     + DivEnd
                            + DivEnd
                        + DivEnd
                    + '</div><div></div>';
        }
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

        $("#DivConfirmPassword #TxtDocPassword").val("");
    },

    cancelConfirmDialog: function () {
        $("#modal-from-dom-DocumentConfirmPassword").modal('hide');
        $('body').find('.modal-backdrop').removeClass('modal-backdrop');
    },

    DocPasswordMatch: function (PatientID, PatDocID, IsFromDashboard, FileType, IsFromViewer, IsFromFolder, IsFromPrevDoc, IsFromNextDoc) {
        var TypedPassword = "";
        if (parseInt(Patient_Document.PasswordTries) >= 0 && parseInt(Patient_Document.PasswordTries) <= 3) {
            if (IsFromViewer != "true") {
                TypedPassword = $("#DivConfirmPassword #TxtDocPassword").val();
            }
            else {
                TypedPassword = $("#modal-from-dom-DocumentConfirmPasswordViewer #DivConfirmPasswordViewer #TxtDocPassword").val();
            }

            Patient_Document.MatchDocPassword(PatDocID, TypedPassword).done(function (response) {
                if (response.status != false) {
                    if (IsFromViewer == "true") {
                        if (IsFromPrevDoc == 'true' || IsFromNextDoc == 'true') {
                            Patient_Document.cancelConfirmDialogViewer();
                            $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PatDocID).find("a").click();
                        }
                        else {
                            Document_Viewer.cancelConfirmPasswordDialog();
                            Document_Viewer.DocumentFill();
                        }
                    }
                    else if (IsFromFolder == 'true') {
                        Patient_Document.cancelConfirmDialog();
                        $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PatDocID).find("a").click();
                    }
                    else {
                        Patient_Document.cancelConfirmDialog();
                        Patient_Document.OpenDocumentEdit(PatientID, PatDocID, IsFromDashboard, FileType);
                    }

                    Patient_Document.PasswordTries = 0;
                }
                else {
                    utility.DisplayMessages("Password is incorrect or You do not have Access to the document.", 2);
                    if (parseInt(Patient_Document.PasswordTries) == 3) {
                        if (IsFromViewer == "true") {
                            if (IsFromPrevDoc == 'true' || IsFromNextDoc == 'true')
                                Patient_Document.cancelConfirmDialogViewer();
                            else
                                Document_Viewer.cancelConfirmPasswordDialog();
                        }
                        else
                            Patient_Document.cancelConfirmDialog();
                        Patient_Document.PasswordTries = 0;
                    }
                }
                Patient_Document.PasswordTries = parseInt(Patient_Document.PasswordTries) + 1;
            });
        }
        else {
            if (IsFromViewer == "true") {
                if (IsFromPrevDoc == 'true' || IsFromNextDoc == 'true')
                    Patient_Document.cancelConfirmDialogViewer();
                else
                    Document_Viewer.cancelConfirmPasswordDialog();
            }
            else {
                $("#modal-from-dom-DocumentConfirmPassword").modal('hide');
                $('body').find('.modal-backdrop').removeClass('modal-backdrop');
            }
        }
    },

    MatchDocPassword: function (PatDocID, Password) {
        var data = "Password=" + Password + "&PatDocID=" + PatDocID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DOC_PASSWORD_MATCH");
    },
    FillTagName: function (hfTagId, txtTagName) {
        $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtTagName").val(txtTagName);
        $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfDocumentTagId").val(hfTagId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtTagName"), txtTagName, $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfDocumentTagId"), hfTagId);
        Patient_DocumentTag.UnLoad();

        //$("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtTagName").focus();
    },
    SetCollapseExpandPanelPatientDocument: function () {

        //1- Initialization
        $('#' + Patient_Document.params.PanelID + ' #frmPatientDocument .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Patient_Document.params.PanelID + ' #frmPatientDocument .splitterBtn a'));

        $('#' + Patient_Document.params.PanelID + ' #frmPatientDocument .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Patient_Document.params.PanelID + ' #frmPatientDocument  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Patient_Document.params.PanelID + ' #frmPatientDocument  #splitterBody').show();
        }
        else {
            $('#' + Patient_Document.params.PanelID + ' #frmPatientDocument  #splitterBody').removeClass('splitterBody active');
            $('#' + Patient_Document.params.PanelID + ' #frmPatientDocument  #splitterBody').hide();
        }

    },
    OpenDocumentFax: function (DocumentId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        Patient_Document.FillDocumentsForFax(null, Clinical_ProgressNote.params.patientID, DocumentId).done(function (response) {
            if (response.status) {
                var params = [];

                if (Patient_Document.params["TabID"] == "patTabDocuments") {
                    params["PanelID"] = "pnlPatientDocument";
                }
                else if (Patient_Document.params["TabID"] == "batchTabDocuments") {
                    params["PanelID"] = "pnlBatchDocuments";
                }
                else if (Patient_Document.params["TabID"] == "demographicDetail") {
                    params["ParentCtrl"] = "Patient_Document";
                }
                else if (Patient_Document.params["TabID"] == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = "Patient_Document";
                }

                if (Patient_Document.params["PanelID"] == "pnldemographicDetail #pnlPatientDocument"
                 || Patient_Document.params["PanelID"] == "ctrlPanClinical #pnlClinicalProgressNote #pnlPatientDocument") {
                    params["TabID"] = Patient_Document.params["TabID"];
                    Batch_FaxSend.UnloadParent = "ParentUnload";
                    var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
                    params["PDFBase64"] = response.MergedContent;
                    LoadActionPan('Batch_FaxSend', params, parentPanelId);
                }
                else {
                    params["PDFBase64"] = response.MergedContent;
                    LoadActionPan("Batch_FaxSend", params);
                }
            }
            else {
                utility.DisplayMessages((response.Message ? response.Message.charAt(0).toUpperCase() + response.Message.slice(1) : response.Message), 3);
            }
        });
    },
    PrintViewDocument: function (DocumentId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var docIds;
        if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.PatientDocumentView).is(":visible")) {
            docIds = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
        }
        else {
            var objLength = $('#' + Patient_Document.params.PanelID + ' input:checked').length;
            if (objLength == 0) {
                utility.DisplayMessages("Please select any document(s) to Print.", 2);
                return;
            }
            docIds =
                   $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument tbody input[id*="chkPatDoc"]:checked').map(function () {
                       return this.id.replace("chkPatDoc", "");
                   }).get().join(',');
        }
        Patient_Document.FillDocumentsForFax(null, Clinical_ProgressNote.params.patientID, docIds).done(function (response) {
            if (response.status) {
                //var params = [];
                //if (Patient_Document.params["TabID"] == "patTabDocuments") {
                //    params["PanelID"] = "pnlPatientDocument";
                //}
                //else if (Patient_Document.params["TabID"] == "batchTabDocuments") {
                //    params["PanelID"] = "pnlBatchDocuments";
                //}
                //else if (Patient_Document.params["TabID"] == "demographicDetail") {
                //    params["ParentCtrl"] = "Patient_Document";
                //}
                //else if (Patient_Document.params["TabID"] == "clinicalTabProgressNote") {
                //    params["ParentCtrl"] = "Patient_Document";
                //}

                //if (Patient_Document.params["PanelID"] == "pnldemographicDetail #pnlPatientDocument"
                // || Patient_Document.params["PanelID"] == "ctrlPanClinical #pnlClinicalProgressNote #pnlPatientDocument") {
                //    params["TabID"] = Patient_Document.params["TabID"];
                //    Batch_FaxView.UnloadParent = "ParentUnload";
                //    var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
                //    params["FaxHtml"] = response.MergedContent;
                //    LoadActionPan('Batch_FaxView', params, parentPanelId);
                //}
                //else {
                //    params["FaxHtml"] = response.MergedContent;
                //    LoadActionPan("Batch_FaxView", params);
                //}
                var blobitem = DashBoard.getblob(response.MergedContent, 'application/pdf');
                var pdfObjectUrl = URL.createObjectURL(blobitem);
                var ifrm = document.createElement("iframe");
                ifrm.className = "DashboardFrame";
                ifrm.setAttribute("src", pdfObjectUrl);
                ifrm.style.minWidth = "640px";
                ifrm.style.minHeight = "480px";
                ifrm.width = "100%";
                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument ').append(ifrm);
                ifrm.contentWindow.print();
                if ($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument .DashboardFrame').length > 1) {
                    $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument .DashboardFrame:not(:last)').each(function (k, v) {
                        $(this).remove();
                    });
                }
                // $(ifrm).hide();
                $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument .DashboardFrame').hide();
            }
            else
                utility.DisplayMessages((response.Message ? response.Message.charAt(0).toUpperCase() + response.Message.slice(1) : response.Message), 3);
        });
    },
    ReviewedAndSignedDocument: function (PatDocId, PatientId, FolderName) {
        var data = "PatDocId=" + PatDocId + "&PatientID=" + PatientId + "&FolderName=" + FolderName;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "REVIEWED_AND_SIGN_DOCUMENT");
    },

    ValidateSearchCriteria: function () {
        $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val("");
        if (Patient_Document.params.PanelID == "pnlBatchDocuments") {
            utility.ValidateSearchCriteria(Patient_Document.params.PanelID + " #frmPatientDocument", function () {
                Patient_Document.LoadFolders();

            });
        }
        else {
            Patient_Document.LoadFolders();
        }
    },

    ExpandJsTree: function (e) {

        $('#' + Patient_Document.params["PanelID"] + ' #folderPanel').removeClass('reportToggle NoRadiusB');
        $('#' + Patient_Document.params["PanelID"] + ' #folderPanel').parent().removeClass('folder');
        $('#' + Patient_Document.params["PanelID"] + ' #splitterbody').removeClass('folder-main');
        // $('#divDocScan').removeClass('folder-main');
        if ($('.ds-dwt-container-box')) {
            var prntWdth = $('#dwtcontrolContainer').innerWidth();
            $('.ds-dwt-container-box').width(prntWdth ? prntWdth : 580);
        }
    },
    UnLoadScanDoc: function () {
        try {
            var size = DWObject.HowManyImagesInBuffer;
            if (parseFloat(size) > 0) {
                utility.myConfirm('59', function () {
                    if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {
                        if (localStorage.SelectedAccountNumber) {
                            //Load Folder and Refreh grid if patient selected in Patient-> document or batch-> Document.
                            if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                                Patient_Document.LoadFolders(true, true);
                            }
                            else {
                                $('#' + Document_Scan.params["PanelID"] + ' #hfPatientId').val(localStorage.SelectedPatientId);
                                Patient_Document.LoadFolders(true);
                            }

                        }
                        else {
                            if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                                Patient_Document.LoadFolders(true, true);
                            }
                        }
                    }
                    DWObject.RemoveAllImages();
                    updatePageInfo();
                    //   Dynamsoft.WebTwainEnv = null;
                    $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
                    $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
                    $("#" + Patient_Document.params["PanelID"] + " #divDocScan").html("");
                    Patient_Document.setState(false);
                    $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').addClass("hidden");
                    $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
                    $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                    $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                }, function () { },
                                    '59'
                );
            } else {
                if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {

                    if (localStorage.SelectedAccountNumber) {
                        // if patient is loaded in patient-> document fied then dont need to update hidden field.
                        if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                            Patient_Document.LoadFolders(true, true);

                        }
                        else {
                            $('#' + Document_Scan.params["PanelID"] + ' #hfPatientId').val(localStorage.SelectedPatientId);
                            Patient_Document.LoadFolders(true);
                        }
                    }
                    else {
                        if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                            Patient_Document.LoadFolders(true, true);
                        }
                    }

                }
                $('#' + Patient_Document.params["PanelID"] + ' #btnExport').removeClass('hidden');
                $('#' + Patient_Document.params["PanelID"] + ' #btnDelete').removeClass('hidden');
                $('#' + Patient_Document.params["PanelID"] + ' #btnAssignUserToReview').removeClass('hidden');
                //var tree = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree).jstree(true);
                //if (tree) {
                //    tree.refresh();
                //}
                DWObject.RemoveAllImages();
                updatePageInfo();
                // Dynamsoft.WebTwainEnv = null;
                $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
                $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
                $("#" + Patient_Document.params["PanelID"] + " #divDocScan").html("");
                Patient_Document.setState(false);
                $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').addClass("hidden");


            }
        }
        catch (ex) {
            if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {
                if (localStorage.SelectedAccountNumber) {
                    // if patient is loaded in patient-> document fied then dont need to update hidden field.
                    if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                        Patient_Document.LoadFolders(true, true);
                    }
                    else {
                        $('#' + Document_Scan.params["PanelID"] + ' #hfPatientId').val(localStorage.SelectedPatientId);
                        Patient_Document.LoadFolders(true);
                    }
                }
                else {
                    // patient is not selected in banner.
                    if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val()) {
                        Patient_Document.LoadFolders(true, true);
                    }

                }

            }
            Dynamsoft.WebTwainEnv = null;
            $("#" + Patient_Document.params["PanelID"] + " #divPatDocument").show();
            $("#" + Patient_Document.params["PanelID"] + " #divDocScan").hide();
            $("#" + Patient_Document.params["PanelID"] + " #divDocScan").html("");
            Patient_Document.setState(false);
            $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').addClass("hidden");
            console.log(ex);

        }
        ScannerLoaded = "ScanShell 800DX";
    },
    MoveFilesToFolder: function (FolderId, FolderName, event) {
        if (event != null)
            event.stopPropagation();
        if (FolderId && FolderName) {
            Document_Scan.MoveSelectDocs(FolderId, FolderName);
        }
    },
    RedrawImage: function (src) {
        var imageObj = new Image();
        imageObj.src = src;
        var canvas = document.getElementById(Patient_Document.params.PatientDocumentCanvas);

        canvas.width = $("#" + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentCanvasPanel).width();
        canvas.height = $("#" + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentCanvasPanel).height();

        var context = canvas.getContext("2d");


        $(canvas).css("width", wdth - 10 + "px");
        $(canvas).css("height", hght - 10 + "px");
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
        draw();
    },
    AppendMoveDocBtn: function () {
        $.each($('#' + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentTree + " .rootFolder"), function (i, item) {
            var $li = $(this);
            $li.css("position", "relative");
            if (DWObject && DWObject.SelectedImagesCount > 0) {
                if ($li.find('.btn-docMove').length == 0) {
                    $li.append('<a href="javascript:void(0);" class=" btn removeIconListHover btn-docMove" title="Move" folderId="' + $(this).attr('id') + '" onclick="Patient_Document.MoveFilesToFolder(' + $li.attr("id") + ',\'' + $($li.find(".jstree-anchor")[0]).text() + '\',event)"><i class="fa fa-sign-in fa-rotate-180 blue"></i></a>');
                }
            }
            else {
                if ($li.find('.btn-docMove').length == 0) {
                    $li.append('<a href="javascript:void(0);" class=" btn removeIconListHover hidden btn-docMove" title="Move" folderId="' + $(this).attr('id') + '" onclick="Patient_Document.MoveFilesToFolder(' + $li.attr("id") + ',\'' + $($li.find(".jstree-anchor")[0]).text() + '\',event)"><i class="fa fa-sign-in fa-rotate-180 blue"></i></a>');
                }
            }
            var selectedFolderId = $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val();
            if (selectedFolderId && $($li).attr("id") == selectedFolderId) {

                $($li).children("a.jstree-anchor").addClass("jstree-clicked");
            }
        });
        Patient_Document.ShowHideMoveButton();
    },

    SignalRUsersDocumentsAccess: function (PatDocID) {
        try {
            $.connection.hub.stop();
            Patient_Document.SignalRHub = null;

            $.connection.hub.qs = {
                "Username": globalAppdata.AppUserFirstName + " " + globalAppdata.AppUserLastName,
                "UserId": globalAppdata.AppUserId,
                "PatDocID": PatDocID,
            }
            Patient_Document.SignalRHub = $.connection.patientDocumentAccessHub;

            $.connection.hub.start()
           .done(function () {
               var Username = globalAppdata.AppUserFirstName + " " + globalAppdata.AppUserLastName;
               Patient_Document.SignalRHub.server.concurrencyDocumentAccessAlert(Username, PatDocID, globalAppdata.AppUserId).done(function (response) {
                   response = JSON.parse(response);
                   if (response.status) {
                       Patient_Document.AnotherUserAccessSameDocument = false;
                       utility.DisplayMessages("Unable to delete because " + response.UserName + " is accessing the same document.", 2);
                   } else {
                       Patient_Document.AnotherUserAccessSameDocument = true;
                   }
               });

           })
           .fail(function () {
               console.log('You could not have connected to the server.');
           });
        } catch (e) {
            console.log(e.message);
        }
    },
    RefreshLandScreenDocumentViewer: function () {
        if ($("#" + Patient_Document.params["PanelID"] + " #" + Patient_Document.params.PatientDocumentView).is(":visible") == true) {

            var pageNo = $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val();
            var DocumentCount = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderCount').val();
            $('#' + Patient_Document.params["PanelID"] + ' #lblTotalCount').text(DocumentCount);
            pageNo = Number(pageNo) + 1;
            if (pageNo <= DocumentCount) {
                $('#' + Patient_Document.params["PanelID"] + ' #txtpageNo').val(pageNo);
            }
            var SelectedDocuemntId = $('#' + Patient_Document.params["PanelID"] + ' #hfFolderDocumentId').val();
            if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id")) {
                var NextDocumentId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).next().attr("id");
                Patient_Document.LoadFolders(true, false).done(function () {
                    if (NextDocumentId) {
                        utility.callbackAfterAllDOMLoaded(function () {
                            setTimeout($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + NextDocumentId).find("a").click(), 1000);
                        });
                    }
                });
            }
            else if ($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id")) {
                var PreviousDocumentId = $('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + SelectedDocuemntId).prev().attr("id");
                Patient_Document.LoadFolders(true, false).done(function () {
                    if (PreviousDocumentId) {
                        utility.callbackAfterAllDOMLoaded(function () {
                            setTimeout($('#' + Patient_Document.params["PanelID"] + ' #' + Patient_Document.params.PatientDocumentTree + ' #' + PreviousDocumentId).find("a").click(), 1000);
                        });
                    }
                });
            }
            else {
                Patient_Document.DefaultView();
            }

        }
    },
    ShowHideMoveButton: function () {
        if (DWObject && DWObject.HowManyImagesInBuffer > 0 && $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').length > 0 && $("#" + Patient_Document.params["PanelID"] + " #divDocScan").css('display') != 'none') {
            $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').removeClass("hidden");
        }
        else {
            if ($("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').length > 0) {
                $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').addClass("hidden");
            }
        }
    },
    ResetPatient: function () {
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(Clinical_FaceSheet.params.patientID);
            }
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber").val($("#PatientProfile #hfAccountNo").val());
            $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val($("#PatientProfile #hfPatientFullNameOnly").val());
            if (Patient_Document.params["PanelID"] == "pnlBatchDocuments") {
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val($("#PatientProfile #hfPatientFullNameOnly").val());
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").attr("disabled", "disabled");
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #lnkPatientName").attr("disabled", "disabled");
                $("#" + Patient_Document.params["PanelID"] + " #divPatDocument #frmPatientDocument #DivDocumentViewBatch").hide();
                $("#" + Patient_Document.params["PanelID"] + " #divPatDocument #frmPatientDocument #divGridViewBatch").show();
            }
        }
        else if ($('#PatientProfile #hfPatientId').val() == "") {
            if ($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber").val() == "") {
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val("");
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtAccountNumber").val("");
                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #txtFullName").val("");
            }
        }
    },
    KPendingGridLoad: function (pageNo, rpp,IsReviewed) {
        $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).empty();
        // empty data source at patient => document  when from Batch flow PMS-4609.
        if (Patient_Document.params.PanelID == "ctrlPanBatch #pnlBatchDocuments" || Patient_Document.params.PanelID == "pnlBatchDocuments") {
            if ($("#" + "pnlPatientDocument" + " #" + "PendingKGrid").data("kendoGrid") && $("#" + "pnlPatientDocument" + " #" + "PendingKGrid").data("kendoGrid").dataSource.data.length > 0)
                $("#" + "pnlPatientDocument" + " #" + "PendingKGrid").data("kendoGrid").dataSource.data([]);                        
        }
        var pendingdataSource = new kendo.data.DataSource({
            schema: {
                data: "data",
                total: "total",
            },
            serverPaging: true,
            pageSize: rpp,
            page: pageNo,
            transport: {
                read: function (e) {

                    if ($("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result").css("display") == "none") {
                        $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result").css("display", "inline");
                    }

                    var self = "";
                    if ($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #divGridView").length == 0) {
                        self = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument .searchPanel")
                    }
                    else {
                        self = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #divGridView");
                    }
                    var patientId = null;
                    if (Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                        patientId = Patient_Document.params["PatientId"];
                    }
                    if ($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val() || Patient_Document.params['ParentCtrl'] == 'demographicDetail') {
                        patientId = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                    }
                    else {
                        patientId = utility.IsNullOrEmptyString(Patient_Document.params.PatientId) ? $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val() : Patient_Document.params.PatientId;
                        $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(patientId);
                    }
                    if (patientId == "") {
                        patientId = $("#PatientProfile #hfPatientId").val();
                    }

                    if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
                        patientId = Clinical_FaceSheet.params.patientID;
                    }
                    var myJSON = self.getMyJSON();
                    myJSON = JSON.parse(myJSON);
                    myJSON.ddlEnteredBy_text = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #ddlEnteredBy option:selected").attr('refvalue');
                    myJSON = JSON.stringify(myJSON);

                    Patient_Document.SearchDocument(myJSON, patientId, e.data.page, e.data.pageSize, IsReviewed).done(function (response) {
                        var data_ = { data: [], total: 0 };
                        var PendingDocumentlist = [];

                        if (response != "") {
                            if (response.status != false) {

                                if (response.DocumentLoad_JSON != undefined)
                                    $.each(jQuery.parseJSON(response.DocumentLoad_JSON), function (key, value) {
                                        var  Objectlist = {
                                            "Documentid": value.Documentid,
                                            "PatDocId": value.PatDocId,
                                            "PatientId": value.PatientId,
                                            "FileType": value.FileType,
                                            "ShowPassword": value.ShowPassword,
                                            "IsLocked": value.IsLocked,
                                            "PatientName": value.AccountNumber + '-' + value.PatientName,
                                            "DOS": utility.RemoveTimeFromDate(null, value.DOS.trim()),
                                            "DocPrioirty": value.DocPrioirty,
                                            "Pages": value.Pages,
                                            "DocumentName": value.DocumentName,
                                            "FilePath": value.FilePath,
                                            "ViewBy": value.ViewBy,
                                            "LinkDocument": value.LinkDocument,
                                            "IsAttachedWithNote": value.IsAttachedWithNote,
                                            "LinkWithClaim": value.LinkWithClaim,
                                            "children": [{
                                                "ExpiryDate": utility.RemoveTimeFromDate(null, value.ExpiryDate.trim()),
                                                "CreatedBy": value.CreatedBy,
                                                "Comments": value.Comments
                                            }]
                                        };
                                        PendingDocumentlist.push(Objectlist);
                                    });
                                data_.data = PendingDocumentlist;
                                data_.total = response.PendingCount;
                                e.success(data_);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                                e.success(data_);

                            }
                            if (response.DocumentCount > 0) {
                                if (response.PendingCount) {
                                    $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #pendingDocument").text(response.PendingCount);
                                }
                            } else {
                                $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #pendingDocument").text(0);
                            }

                        }
                        else {
                            e.error();
                        }

                    });
                }
            }
        });
        if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).data("kendoTooltip"))
            $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).data("kendoTooltip").destroy();

        $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument).kendoGrid({
            dataSource: pendingdataSource,
            resizable: true,
            scrollable: false,
            noRecords: true,
            serverPaging: true,
            sortable: true,
            messages: {
                noRecords: "No Pending Document Found."
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 15, 20, 50, 100],
                buttonCount: 6
            },
            columns: [
            { field: "DOS", width: "50px", headerTemplate: 'DOS <span class="k-icon k-i-kpi"></span>' },
            { field: "PatientName", width: "170px", headerTemplate: 'Patient Info <span class="k-icon k-i-kpi"></span>' },
            { field: "DocumentName", width: "90px", headerTemplate: 'Folder <span class="k-icon k-i-kpi"></span>' },
            { field: "FilePath", attributes: { "class": "maxColumnWidth270" }, template: '#=Patient_Document.LinkedAndLockDocuments(data)#', width: "200px", headerTemplate: 'File Name <span class="k-icon k-i-kpi"></span>' },
            { field: "DocPrioirty", attributes: { "class": "text-center" }, template: '#=Patient_Document.PrioirtyTemplate(data)#', width: "70px", headerTemplate: 'Prioirty <span class="k-icon k-i-kpi"></span>' },
            { field: "Pages", width: "60px", attributes: { "class": "text-center" }, headerTemplate: 'Pages <span class="k-icon k-i-kpi"></span>' },
            { field: "ViewBy", width: "90px", headerTemplate: 'Assignee<span class="k-icon k-i-kpi"></span>' },
            { title: "Log", width: "50px", template: '#=Patient_Document.logRowHistory(data)#' },

            ],
            detailInit: function (e) { Patient_Document.detailInit(e,2) },
            selectable: "multiple, row",
            change: function (e) {
                var selectedRows = this.select();
                var selectedDataItems = [];
                for (var i = 0; i < selectedRows.length; i++) {
                    var dataItem = this.dataItem(selectedRows[i]);
                    selectedDataItems.push(dataItem);
                }
                if (Document_Viewer.NxtPrvLinkDocGrdName != "MainGrid") {
                    $("#" +Patient_Document.params.PanelID + " #" +Patient_Document.params.GridPatientDocument +" #" +Document_Viewer.NxtPrvLinkDocGrdName).find("tr.k-state-selected").removeClass('k-state-selected');
                }
                Document_Viewer.NxtPrvLinkDocGrdName = "MainGrid";
                Document_Viewer.SectionRowIndex =selectedRows[0].sectionRowIndex;
                Patient_Document.DocumentEdit(selectedDataItems[0].PatientId, selectedDataItems[0].PatDocId, this, false, selectedDataItems[0].FileType, selectedDataItems[0].Documentid);
            },
            dataBound: function () {
                $('.k-hierarchy-cell.k-header').html(function (i, h) {
                    return h.replace(/&nbsp;/g, "<input id='chkMasterPatDoc'  onclick=Patient_Document.checkUncheckAll(this) type='checkbox', class='check-box' type='checkbox'/> ");
                });
                var grid = this;
                grid.tbody.find('>tr').each(function () {
                    var dataItem = grid.dataItem(this);
                    if (!dataItem.hasChildren) {
                        $(this).find('.k-hierarchy-cell').addClass("size60");
                        var attachmentIcon = "";
                        if (Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                            var enableDisableIcon = '';
                            if (dataItem.LinkWithClaim == "1") {
                                enableDisableIcon = " disableAll";
                            }
                            attachmentIcon = "<i class='fa fa-paperclip m-default hand-cursor blue " + enableDisableIcon + "' style='font-size:14px;' onclick='Patient_Document.LinkDocumentWithClaim(event,this,\"" +dataItem.PatDocId + "\")' title='Link Cliam'></i>"
                        }
                        if (Patient_Document.params["ParentCtrl"] == "clinicalTabProgressNote") {
                            var Checked = "";
                            if (dataItem.IsAttachedWithNote == "1") {
                                if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(dataItem.PatDocId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                    Checked = " ";
                                } else {
                                    Checked = " checked";
                                }
                                $(this).find('.k-hierarchy-cell').prepend("<input id='chkPatDoc" + dataItem.PatDocId + "', type='checkbox' " + Checked + " class='btn p-none mt-none' onclick='Patient_Document.SelectDocument(event, this ,\"" + dataItem.PatDocId + "\", \"" + dataItem.showPassword + "\",\"" + dataItem.IsLocked + "\");' />" );
                                $(this).find('.k-hierarchy-cell .k-icon').after(attachmentIcon);
                            }
                            else {
                                if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(dataItem.PatDocId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                    Checked = " checked";
                                } else {
                                    Checked = "";
                                }
                                $(this).find('.k-hierarchy-cell').prepend("<input id='chkPatDoc" + dataItem.PatDocId + "', type='checkbox' " + Checked + " class='btn p-none mt-none' onclick='Patient_Document.SelectDocument(event, this ,\"" + dataItem.PatDocId + "\", \"" + dataItem.showPassword + "\",\"" + dataItem.IsLocked + "\");' />");
                                $(this).find('.k-hierarchy-cell .k-icon').after(attachmentIcon);
                            }
                        }
                        else {
                            $(this).find('.k-hierarchy-cell').prepend("<input id='chkPatDoc" + dataItem.PatDocId + "', type='checkbox' class='btn p-none mt-none' onclick='Patient_Document.SelectDocument(event, this ,\"" + dataItem.PatDocId + "\", \"" + dataItem.showPassword + "\",\"" + dataItem.IsLocked + "\");' />");
                            $(this).find('.k-hierarchy-cell .k-icon').after(attachmentIcon);
                        }
                    }
                    Patient_Document.createFileTypesArray(dataItem);
                })
                Patient_Document.SetColumnWidthFileName();
            },

        });
        Patient_Document.SetTooltipReviewedPending("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument, "pending");
    },
    detailInit: function (e,gridid) {
        var grdDetails = "grdDtl-" + e.data.PatDocId;
        var grdLinkDocument = "grdLnkDoc-" + e.data.PatDocId;

        var tab = '<ul class="tabs-custom "><li id="liDetails" class="active"> <a class="custom-made-tabs" href="#' + grdDetails + '" onclick=Patient_Document.SetNestedGrid("' + grdDetails + '") data-toggle="tab">Details</a></li> <li id="liLinked" > <a class="custom-made-tabs"  href="#' + grdLinkDocument + '" onclick=Patient_Document.SetNestedGrid("' + grdLinkDocument + '")  data-toggle="tab" >Linked Documents </a></li></ul>';
        var tabDetails = '<div id="' + grdDetails + '" class="tab-pane custom-made-tabs "> <div  class="table table-bordered table-striped table-condensed table-hover mb-none "></div> <div id="' + grdDetails + '"></div></div>'
        var tabLinksDocuments = '<div id="' + grdLinkDocument + '" class="tab-pane custom-made-tabs"> <div  class="table table-bordered table-striped table-condensed table-hover mb-none "></div> <div id="' + grdLinkDocument + '></div> </div>'

        var grid = $(tab + ' <div class="tab-content tabs-custom-body pl-xxs pr-xxs">' + tabDetails + " " + tabLinksDocuments + " </div>").appendTo(e.detailCell);
        e.detailRow.find("td:first-child").remove();
        $(".k-detail-cell").attr("colspan", "10");
        
        var detailsGrd = $(e.detailCell).find('#' + grdDetails).kendoGrid({
            dataSource: {
                data: e.data.children
            },
            noRecords: true,
            scrollable: false,
            sortable: true,
            columns: gridid == 1 ? [
              { title: "Expiry Date", field: "ExpiryDate", width: "100px" },
              { title: "Entered By", field: "CreatedBy", width: "100px" },
              { title: "Reviewed By", field: "ReviewBy", width: "100px" },
              { title: "Reviewed Date", field: "ReviewDate", width: "100px" },
              { title: "Assignee", field: "ViewBy", width: "100px" },
              { title: "Comments", field: "Comments", width: "100px" }]:
              [{ title: "Expiry Date", field: "ExpiryDate", width: "100px" },
              { title: "Entered By", field: "CreatedBy", width: "100px" },
              { title: "Comments", field: "Comments", width: "100px" }]
        });

        $('#' + grdDetails).addClass('active');
        Patient_Document.SearchLinkedDocument(e.data.PatDocId).done(function (response) {
            var object = [];
            if (response.status == false) {
                object = [];
                utility.DisplayMessages(response.Message, 3);
            }
            else {
                $.each(JSON.parse(response.DocumentLoad_JSON), function (key, value) {
                    var linkDocumentObject = {
                        "LinkDocumentId": value.LinkDocumentId,
                        "PatDocId": value.PatDocId,
                        "PatientId": value.PatientId,
                        "FileType": value.FileType,
                        "PatientName": value.AccountNumber + '-' + value.PatientName,
                        "DOS": utility.RemoveTimeFromDate(null, value.DOS.trim()),
                        "Pages": value.Pages,
                        "DocumentName": value.DocumentName,
                        "FilePath": value.FilePath,
                        "ViewBy": value.ViewBy,
                        "IsActive": value.IsActive
                    };
                    object.push(linkDocumentObject);

                });
            }

            var data = new kendo.data.DataSource({
                data: object,
                pageSize: 15
            });

            $(e.detailCell).find('#' + grdLinkDocument).kendoGrid({
                dataSource: data,
                resizable: true,
                scrollable: false,
                noRecords: true,
                messages: {
                    noRecords: "No Link Document Found."
                },
                pageable: {
                    refresh: false,
                    pageSizes: [5, 10, 15, 20, 50, 100],
                    buttonCount: 6
                },
                columns: [
                  { title: "Action", width: "100px", template: '#=Patient_Document.ActionLinkDocuments(data)#' },
                  { title: "DOS", field: "DOS", width: "100px" },
                  { title: "Patient Info", field: "PatientName", width: "100px" },
                  { title: "Folder", field: "DocumentName", width: "100px" },
                  { title: "File Name", field: "FilePath", width: "100px" },
                  { title: "Pages", field: "Pages", width: "100px" },
                  { title: "Assignee", field: "ViewBy", width: "100px" }
                ],
                selectable: "multiple, row",
                change: function (e) {
                    var selectedRows = this.select();
                    var selectedDataItems = [];
                    for (var i = 0; i < selectedRows.length; i++) {
                        var dataItem = this.dataItem(selectedRows[i]);
                        selectedDataItems.push(dataItem);
                    }
                    if (Document_Viewer.NxtPrvLinkDocGrdName == "MainGrid") {
                        $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridPatientDocument ).find("tr.k-state-selected").removeClass('k-state-selected');
                    }
                    Document_Viewer.NxtPrvLinkDocGrdName = this.element.context.id;
                    Document_Viewer.SectionRowIndex = selectedRows[0].sectionRowIndex;
                    Patient_Document.DocumentEdit(selectedDataItems[0].PatientId, selectedDataItems[0].PatDocId, this, false, selectedDataItems[0].FileType, selectedDataItems[0].Documentid);
                },
            });
        });

    },
    logRowHistory: function (data) {
        return '<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Patient_Document.rowHistory(' + data.PatDocId + ');"> <i class="fa fa-history blue"></i></a>';
    },
    ActionLinkDocuments: function (data) {
        if (data.IsActive == "True") {
            isactive = 0;
            activeTitle = "Active Record";
            tglclass = "fa fa-toggle-on green";
        }
        else {
            isactive = 1;
            activeTitle = "Inactive Record";
            tglclass = "fa fa-toggle-on red fa-rotate-180";
        }
        return '<a class="btn  btn-xs" href="#" onclick="Patient_Document.DocumentDelete(\'' + data.PatDocId + '\', event, false,null, ' + data.LinkDocumentId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_Document.ActiveInactivePatientDocument(' + data.PatDocId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },

    LinkedAndLockDocuments: function (data) {

        var FileNameWithIcon = "";

        if (data.IsLocked == 1 && data.LinkDocument && data.LinkDocument == 0) {
            FileNameWithIcon = "<div><i class='fa fa-lock blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + data.FilePath + "</div>"
        }
        else if (data.IsLocked == 0 && data.LinkDocument && data.LinkDocument > 0) {
            FileNameWithIcon = "<div><i class='fa fa-paperclip blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + data.FilePath + "</div>"
        }
        else if (data.IsLocked == 1 && data.LinkDocument && data.LinkDocument > 0) {
            FileNameWithIcon = "<div><i class='fa fa-lock  blue' style='font-size:15px;'></i> <i class='fa fa-paperclip blue' style='font-size:15px;'></i>&nbsp;&nbsp;&nbsp; " + data.FilePath + "</div>"
        }
        else {
            FileNameWithIcon = "<div>&nbsp;&nbsp;&nbsp; " + data.FilePath + "</div>"
        }


        return FileNameWithIcon;
    },

    KReviewedGridLoad: function (pageNo, rpp,IsReviewed) {
        $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).empty();
        //PMS - 4609
        if (Patient_Document.params.PanelID == "ctrlPanBatch #pnlBatchDocuments" || Patient_Document.params.PanelID == "pnlBatchDocuments") {
            if ($("#" + "pnlPatientDocument" + " #" + "ReviewedKGrid").data("kendoGrid") && $("#" + "pnlPatientDocument" + " #" + "ReviewedKGrid").data("kendoGrid").dataSource.data.length > 0)
                $("#" + "pnlPatientDocument" + " #" + "ReviewedKGrid").data("kendoGrid").dataSource.data([]);
        }
        var ReviewedDataSource = new kendo.data.DataSource({
            schema: {
                data: "data",
                total: "total",
            },
            serverPaging: true,
            pageSize: rpp,
            page: pageNo,
            transport: {
                read: function (e) {

                    if ($("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result").css("display") == "none") {
                        $("#" + Patient_Document.params["PanelID"] + " #pnlPatientDocument_Result").css("display", "inline");
                    }

                    var self = "";
                    if ($("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #divGridView").length == 0) {
                        self = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument .searchPanel")
                    }
                    else {
                        self = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #divGridView");
                    }
                    var patientId = null;
                    if (Patient_Document.params.ParentCtrl == "Patient_Case_Detail") {
                        patientId = Patient_Document.params["PatientId"];
                    }
                    if ($('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument' + ' #txtFullName').val() || Patient_Document.params['ParentCtrl'] == 'demographicDetail') {
                        patientId = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val();
                    }
                    else {
                        patientId = utility.IsNullOrEmptyString(Patient_Document.params.PatientId) ? $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val() : Patient_Document.params.PatientId;
                        $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument #hfPatientId").val(patientId);
                    }
                    if (patientId == "") {
                        patientId = $("#PatientProfile #hfPatientId").val();
                    }

                    if (Patient_Document.params.ParentCtrl == "Clinical_FaceSheet") {
                        patientId = Clinical_FaceSheet.params.patientID;
                    }
                    var myJSON = self.getMyJSON();
                    myJSON = JSON.parse(myJSON);
                    myJSON.ddlEnteredBy_text = $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #ddlEnteredBy option:selected").attr('refvalue');
                    myJSON = JSON.stringify(myJSON);

                    Patient_Document.SearchDocument(myJSON, patientId, e.data.page, e.data.pageSize, IsReviewed).done(function (response) {
                        var rData_ = { data: [], total: 0 };
                        var ReviewedDocumentlist = [];
                        if (response != "") {
                            if (response.status != false) {

                                if (response.ReviewedDocumentLoad_JSON != undefined)
                                    $.each(jQuery.parseJSON(response.ReviewedDocumentLoad_JSON), function (key, value) {
                                        var Objectlist = {
                                            "Documentid": value.Documentid,
                                            "PatDocId": value.PatDocId,
                                            "PatientId": value.PatientId,
                                            "FileType": value.FileType,
                                            "PatientName": value.AccountNumber + '-' + value.PatientName,
                                            "DOS": utility.RemoveTimeFromDate(null, value.DOS.trim()),
                                            "ShowPassword": value.ShowPassword,
                                            "IsLocked": value.IsLocked,
                                            "DocPrioirty": value.DocPrioirty,
                                            "Pages": value.Pages,
                                            "DocumentName": value.DocumentName,
                                            "FilePath": value.FilePath,
                                            "SignBy": value.SignBy,
                                            "SignDate": utility.RemoveTimeFromDate(null, value.SignDate.trim()),
                                            "LinkDocument": value.LinkDocument,
                                            "IsAttachedWithNote": value.IsAttachedWithNote,
                                            "LinkWithClaim": value.LinkWithClaim,
                                            "children": [{
                                                "ExpiryDate": utility.RemoveTimeFromDate(null, value.ExpiryDate.trim()),
                                                "CreatedBy": value.CreatedBy,
                                                "Comments": value.Comments,
                                                "ViewBy": value.ViewBy,
                                                "ReviewBy": value.ReviewBy,
                                                "ReviewDate": utility.RemoveTimeFromDate(null, value.ReviewDate.trim()),
                                            }]
                                        };
                                        ReviewedDocumentlist.push(Objectlist);
                                    });

                                rData_.data = ReviewedDocumentlist;
                                rData_.total = response.ReviewedCount;
                                e.success(rData_);

                                if (response.DocumentCount > 0) {
                                    if (response.ReviewedCount) {
                                        $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #ReviewedDocument").text(response.ReviewedCount);
                                    }
                                } else {
                                    $("#" + Patient_Document.params["PanelID"] + " #frmPatientDocument" + " #ReviewedDocument").text(0);
                                }



                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                                e.success(rData_);
                            }
                        }
                        else {
                            e.error();
                        }
                    });
                }
            }
        });
        if ($("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).data("kendoTooltip"))
            $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).data("kendoTooltip").destroy();

        $("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument).kendoGrid({
            dataSource: ReviewedDataSource,
            resizable: true,
            scrollable: false,
            sortable: true,
            noRecords: true,
            messages: {
                noRecords: "No Reviewed Document Found."
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 15, 20, 50, 100],
                buttonCount: 6
            },
            columns: [
            { field: "DOS", width: "50px", headerTemplate: 'DOS <span class="k-icon k-i-kpi"></span>' },
            { field: "PatientName", width: "150px", headerTemplate: 'Patient Info <span class="k-icon k-i-kpi"></span>' },
            { field: "DocumentName", width: "90px", headerTemplate: 'Folder <span class="k-icon k-i-kpi"></span>' },
            { field: "FilePath", attributes: { "class": "maxColumnWidth200" }, template: '#=Patient_Document.LinkedAndLockDocuments(data)#', width: 200, headerTemplate: 'File Name <span class="k-icon k-i-kpi"></span>' },
            { field: "DocPrioirty", attributes: { "class": "text-center" }, template: '#=Patient_Document.PrioirtyTemplate(data)#', width: "70px", headerTemplate: 'Prioirty <span class="k-icon k-i-kpi"></span>' },
            { field: "Pages", width: "60px", attributes: { "class": "text-center" }, headerTemplate: 'Pages <span class="k-icon k-i-kpi"></span>' },
            { field: "SignBy", width: "95px", headerTemplate: 'Signed By <span class="k-icon k-i-kpi"></span>' },
            { field: "SignDate", width: "95px", headerTemplate: 'Signed Date <span class="k-icon k-i-kpi"></span>' },
            { title: "Log", width: "50px", template: '#=Patient_Document.logRowHistory(data)#' }, ],
            detailInit: function (e) { Patient_Document.detailInit(e,1) },
            selectable: "multiple, row",
            change: function (e) {
                var selectedRows = this.select();
                var selectedDataItems = [];
                for (var i = 0; i < selectedRows.length; i++) {
                    var dataItem = this.dataItem(selectedRows[i]);
                    selectedDataItems.push(dataItem);
                }
                Document_Viewer.NxtPrvLinkDocGrdName = "MainGrid";
                Document_Viewer.SectionRowIndex = selectedRows[0].sectionRowIndex;
                Patient_Document.DocumentEdit(selectedDataItems[0].PatientId, selectedDataItems[0].PatDocId, this, false, selectedDataItems[0].FileType,selectedDataItems[0].Documentid);
            },
            dataBound: function () {
                $('.k-hierarchy-cell.k-header').html(function (i, h) {
                    return h.replace(/&nbsp;/g, "<input id='chkMasterPatDoc'  onclick=Patient_Document.checkUncheckAll(this) type='checkbox', class='check-box' type='checkbox'/>");
                });
                var grid = this;
                grid.tbody.find('>tr').each(function () {
                    var dataItem = grid.dataItem(this);
                    var attachmentIcon = "";
                    if (Patient_Document.params.ParentCtrl == "EncounterChargeCapture") {
                        var enableDisableIcon = '';
                        if (dataItem.LinkWithClaim == "1") {
                            enableDisableIcon = " disableAll";
                        }
                        attachmentIcon = "<i class='fa fa-paperclip m-default hand-cursor blue " + enableDisableIcon + "' style='font-size:14px;' onclick='Patient_Document.LinkDocumentWithClaim(event,this,\"" + dataItem.PatDocId + "\")' title='Link Cliam'></i>"
                    }
                    if (!dataItem.hasChildren) {
                        $(this).find('.k-hierarchy-cell').addClass("size60");


                        if (Patient_Document.params["ParentCtrl"] == "clinicalTabProgressNote") {
                            var Checked = "";
                            if (dataItem.IsAttachedWithNote == "1") {
                                if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(dataItem.PatDocId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                    Checked = " ";
                                } else {
                                    Checked = " checked";
                                }
                                $(this).find('.k-hierarchy-cell').prepend("<input id='chkPatDoc" + dataItem.PatDocId + "', type='checkbox' "+Checked+" class='btn p-none mt-none' onclick='Patient_Document.SelectDocument(event, this, \"" + dataItem.PatDocId + "\", \"" + dataItem.showPassword + "\",\"" + dataItem.IsLocked + "\");'/>");
                                $(this).find('.k-hierarchy-cell .k-icon').after(attachmentIcon);
                            }
                            else {
                                if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(dataItem.PatDocId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                    Checked = " checked";
                                } else {
                                    Checked = "";
                                }
                                $(this).find('.k-hierarchy-cell').prepend("<input id='chkPatDoc" + dataItem.PatDocId + "', type='checkbox' " + Checked + " class='btn p-none mt-none' onclick='Patient_Document.SelectDocument(event, this, \"" + dataItem.PatDocId + "\", \"" + dataItem.showPassword + "\",\"" + dataItem.IsLocked + "\");'/>");
                                $(this).find('.k-hierarchy-cell .k-icon').after(attachmentIcon);
                            }
                        }
                        else {
                            $(this).find('.k-hierarchy-cell').prepend("<input id='chkPatDoc" + dataItem.PatDocId + "', type='checkbox' class='btn p-none mt-none' onclick='Patient_Document.SelectDocument(event, this, \"" + dataItem.PatDocId + "\", \"" + dataItem.showPassword + "\",\"" + dataItem.IsLocked + "\");' />");
                            $(this).find('.k-hierarchy-cell .k-icon').after(attachmentIcon);
                        }
                    }
                    Patient_Document.createFileTypesArray(dataItem);
                })
                Patient_Document.SetColumnWidthFileName();
            },

        });
        Patient_Document.SetTooltipReviewedPending("#" + Patient_Document.params.PanelID + " #" + Patient_Document.params.GridRevDocument, "reviewed");
    },

    SetTooltipReviewedPending: function (ctrlid, ctrl) {
        var tdColumn = "";
        if (ctrl == "reviewed") {
            tdColumn = "td:nth-child(5)";
        } else if (ctrl == "pending") {
            tdColumn = "td:nth-child(5)";
        }
        $(ctrlid).kendoTooltip({
            filter: tdColumn,
            position: "top",
            animation: {
                close: {
                    effects: 'fade:out'
                },
                open: {
                    effects: 'fade:in'
                }
            },
            content: function (e) {
                var dataItem = $(ctrlid).data("kendoGrid").dataItem(e.target.closest("tr"));
                var content = dataItem.FilePath;
                return "<div style='width: " + content.length * .6 + "em;padding: 6px; border-radius: 4px;background:black;color:white;max-width: 30em'>" + content + "</div>";
            }
        }).data("kendoTooltip");

        $(ctrlid).click(function () {
            $(ctrlid).data("kendoTooltip").hide();
        });
    },

    SetColumnWidthFileName: function () {
        if (!$("#divFolderMenu").hasClass("folder")) {
            $("#" + Patient_Document.params.GridRevDocument + " .maxColumnWidth300").addClass("maxColumnWidth200");
            $("#" + Patient_Document.params.GridRevDocument + " .maxColumnWidth200").removeClass("maxColumnWidth300");
            $("#" + Patient_Document.params.GridPatientDocument + " .maxColumnWidth350").addClass("maxColumnWidth270");
            $("#" + Patient_Document.params.GridPatientDocument + " .maxColumnWidth270").addClass("maxColumnWidth350");
        } else {
            $("#" + Patient_Document.params.GridRevDocument + " .maxColumnWidth200").addClass("maxColumnWidth300");
            $("#" + Patient_Document.params.GridRevDocument + " .maxColumnWidth300").removeClass("maxColumnWidth200");

            $("#" + Patient_Document.params.GridPatientDocument + " .maxColumnWidth270").addClass("maxColumnWidth350");
            $("#" + Patient_Document.params.GridPatientDocument + " .maxColumnWidth350").addClass("maxColumnWidth270");
        }
    },

    SetNestedGrid: function (tab) {
        var Doc = tab.split("-")[0];
        if (Doc == "grdDtl") {
            $('#' + tab).show();
            $('#grdLnkDoc-' + tab.split("-")[1]).hide();
        }
        else {
            $('#' + tab).show();
            $('#grdDtl-' + tab.split("-")[1]).hide();
        }
    },
    PrioirtyTemplate: function (data) {
        var DocPriority = '';
        if (data.DocPrioirty.toLowerCase().trim() == "low") {
            DocPriority = '<span class=green bold>' + data.DocPrioirty + '</span>';
        }
        else if (data.DocPrioirty.toLowerCase().trim() == "medium") {
            DocPriority = '<span class=dark-yellow bold>' + data.DocPrioirty + '</span>';
        } else if (data.DocPrioirty.toLowerCase().trim() == "high") {
            DocPriority = '<span class=red bold>' + data.DocPrioirty + '</span>';
        }
        return DocPriority;
    },
    LinkDocumentWithClaim: function (event, obj, PatDocId) {
    if (event != null) {
        event.stopPropagation();
    }
    Patient_Document.UpdateDocumentVisit(PatDocId, Patient_Document.params.VisitId).done(function (response) {
        if (response.status != false) {
            utility.DisplayMessages(response.Message, 2);
            $(obj).addClass("disableAll");
        }
       
        else {
                utility.DisplayMessages(response.Message, 3);
        }
        
    });
    },
    UpdateDocumentVisit: function (PatDocId, VisitId) {
        
        var data = "PatDocId=" + PatDocId + "&VisitId=" + VisitId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT_FROM_CLAIM");
    },
}
function ListItemClick(obj, Id) {
    $("#" + Patient_Document.params["PanelID"] + " #lstDocument li").each(function () {
        if ($(this).val() == Id) {
            $(this).attr("class", "active");
        }
        else
            $(this).removeAttr("class");
    });
    $('#' + Patient_Document.params["PanelID"] + ' #hfDocumentId').val(Id);
    $('#' + Patient_Document.params["PanelID"] + ' #hfFolder').val(Id);
    Patient_Document.DocumentSearch();
}

function ListItemHighLight(Id) {

    var IsHighlighted = false;

    var listItems = $("#" + Patient_Document.params["PanelID"] + " #lstDocument li");
    listItems.each(function (indx, li) {
        var item = $(li);
        if (item.val() == Id) {
            $(item).attr("class", "active");
            IsHighlighted = true;
        }

        // and the rest of your code
    });
    if (!IsHighlighted) {
        Patient_Document.LoadFolders();
        // $('#' + Patient_Document.params["PanelID"] + ' #lstDocument li:first').click();
    }
}