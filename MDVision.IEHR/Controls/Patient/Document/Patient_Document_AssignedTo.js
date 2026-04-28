Document_AssignedTo = {
    params: [],
    Load: function (params) {
        Document_AssignedTo.params = params;
        var self = $('#pnlDocumentAssignedTo');
        self.loadDropDowns(true);
        if (Document_AssignedTo.params.FromLetter == "1") {
            $('#pnlDocumentAssignedTo #ddlFoldergroup').hide();
            $('#pnlDocumentAssignedTo #Lettertitle').text('Send Letter');
            $('#pnlDocumentAssignedTo #lblAssignUser').text('Assign User');
            $('#pnlDocumentAssignedTo #PriorityDropDown').show();
        }
        $('#pnlDocumentAssignedTo #ddlFolder option').filter(function () { return $(this).val() == Document_AssignedTo.params.FolderId; }).prop("selected", true);
        Document_AssignedTo.ValidateAssignedTo();
    },

    ValidateAssignedTo: function () {
        $('#frmDocumentAssignedTo')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    AssignedUserTo: {
                        group: '.col-xs-6',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Folder1: {
                        group: '.col-sm-8',
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
            if (Document_AssignedTo.params.FromLetter == "1") {
                Document_AssignedTo.SendToUser();
            } else {
                Document_AssignedTo.AssignedToSave();
            }
           
        });
    },
    SendToUser: function () {
        var self = $("#pnlDocumentAssignedTo");
        var myJSON = self.getMyJSON();
        if (Document_AssignedTo.params.mode == "Add") {
            Document_AssignedTo.SendToUser_DBCall(myJSON, Document_AssignedTo.params.patientId, Document_AssignedTo.params.PatDocIds).done(function (response) {
                if (response.status != false) {
                    //folderDetail.params["FolderID"] = response.DocId;
                    //if (folderDetail.params.ParentCtrl != null && folderDetail.params.ParentCtrl == "patTabDocuments") {
                    //    Patient_Document.LoadFolders();
                    //}
                    //else
                    //    Admin_Folder.FolderSearch(response.docId);
                    Patient_MessageCompose.UserMessageCount().done(function (response) {
                        if (response.status != false) {
                            var MessageCountsJSONData = JSON.parse(response.MessageCount_JSON);
                            var totalCount = 0;
                            $.each(MessageCountsJSONData, function (index, element) {
                                if (element.MessageType != "Task") {
                                    totalCount += parseInt(element.MessageCounts);
                                }
                            });

                            if (totalCount == 0) {
                                $('#wpanel div.wMessages .badge').hide();
                                $('.notifications #Messages .badge').hide();
                            } else {
                                $('#wpanel div.wMessages .badge').show();
                                $('.notifications #Messages .badge').show();
                                $('#wpanel div.wMessages .badge').text(totalCount);
                                $('.notifications #Messages .badge').text(totalCount);
                            }

                            $('#spnMessageCount').text(MessageCountsJSONData[3].MessageCounts == 0 ? '' : MessageCountsJSONData[3].MessageCounts);
                            $('#liMsgsPractice span').text(MessageCountsJSONData[1].MessageCounts == 0 ? '' : MessageCountsJSONData[1].MessageCounts);
                            $('#liMsgsPatient span').text(MessageCountsJSONData[2].MessageCounts == 0 ? '' : MessageCountsJSONData[2].MessageCounts);
                            $('#liMsgsDirect span').text(MessageCountsJSONData[0].MessageCounts == 0 ? '' : MessageCountsJSONData[0].MessageCounts);
                        }
                    });
                    if ($('#ddlAssignUserto option:selected').text().toLowerCase() == globalAppdata['AppUserName'].toLowerCase()) {
                        var newTotal = Number($("#spnMessageCount").text()) + 1;
                        $("#spnMessageCount").text(newTotal);
                    }
                    if (Document_AssignedTo.params.FromLetter == "1") {
                        UnloadActionPan(Document_AssignedTo.params["ParentCtrl"], "Document_AssignedTo");
                    } else {
                        Patient_Document.DocumentSearch();
                        UnloadActionPan(folderDetail.params["ParentCtrl"], "Document_AssignedTo");
                    }
                    utility.DisplayMessages(response.message, 1);

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SendToUser_DBCall: function (messagesData, filedata, filetype, filepath) {
        var UniqueNumber = "";
        UniqueNumber = Patient_MessageCreate.GenerateUUID();
        var objData = new Object();
        objData["MessageDtl123"] = $("#pnlDocumentAssignedTo #txtComments").val();
        objData["CommandType"] = "save_practice_message";
        objData["MessageType"] = "Practice";
        objData["UniqueNumber"] = UniqueNumber;
        objData["PatientLetterId"] = Document_AssignedTo.params.Patient_Letter_Id;
        objData["hfMessageTo"] = $('#pnlDocumentAssignedTo #ddlAssignUserto').val();
        objData["AttatchedPatientId"] = Document_AssignedTo.params.patientId;
        objData["Subject"] = "Letter";
        objData["Priority"] = $('#pnlDocumentAssignedTo #ddlPriority').val();

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");
    },
    AssignedToSave: function () {
        var self = $("#pnlDocumentAssignedTo");
        var myJSON = self.getMyJSON();
        if (Document_AssignedTo.params.mode == "Add") {
            Document_AssignedTo.SaveAssignedTo(myJSON, Document_AssignedTo.params.patientId, Document_AssignedTo.params.PatDocIds).done(function (response) {
                if (response.status != false) {
                    //folderDetail.params["FolderID"] = response.DocId;
                    //if (folderDetail.params.ParentCtrl != null && folderDetail.params.ParentCtrl == "patTabDocuments") {
                    //    Patient_Document.LoadFolders();
                    //}
                    //else
                    //    Admin_Folder.FolderSearch(response.docId);
                    var self = $("#pnlDocumentAssignedTo");
                    var myJson = JSON.parse(myJSON)
                    if (myJson.ddlAssignUserto == globalAppdata['AppUserId']) {
                        Document_AssignedTo.PendingCountSearchDoc();
                    }

                    if ($('#ddlAssignUserto option:selected').text().toLowerCase() == globalAppdata['AppUserName'].toLowerCase()) {
                        var newTotal = Number($("#spnMessageCount").text()) + 1;
                        $("#spnMessageCount").text(newTotal);
                    }
                    utility.DisplayMessages(response.message, 1);

                    if ($("#" + Patient_Document.params["PanelID"] + " #DivDocumentView").is(":visible") == false || $("#" + Patient_Document.params["PanelID"] + " #DivDocumentView").length <= 0) {
                        //Patient_Document.DocumentSearch();
                        Patient_Document.LoadFolders(true, true);
                    }
                    else if ($("#" + Patient_Document.params["PanelID"] + " #DivDocumentView").is(":visible")) {
                        Patient_Document.RefreshLandScreenDocumentViewer();
                    }

                    if (Document_AssignedTo.params.PatientDetail == "1") {
                        if (Document_AssignedTo.UnloadParent && Document_AssignedTo.UnloadParent == 'ParentUnload') {
                            Document_AssignedTo.UnLoad();
                        }
                        else {
                            UnloadActionPan(Document_AssignedTo.params.ParentCtrl, 'Document_AssignedTo');
                        }
                    } else {
                        UnloadActionPan(Document_AssignedTo.params["ParentCtrl"], "Document_AssignedTo");
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    PendingCountSearchDoc: function () {                  
            Document_AssignedTo.SearchDocPendingCount().done(function (response) {
                if (response.status != false) {
                    if (response.PendingCount > 0) {
                        $('#spnPendingDocumentsCount').show();
                        $('#spnPendingDocumentsCount').text(response.PendingCount)
                    }
                    else {
                        $('#spnPendingDocumentsCount').hide();
                    }
                    
                  }
                        else {
                            $('#spnPendingDocumentsCount').hide();
                        }
                    });
                
    },
  
    SearchDocPendingCount: function (JASONData) {        
        var objData = new JSON.constructor();
        objData["RowsPerPage"] = 15;
        objData["PageNumber"] = 1;       
        objData["PatientId"] = "";
        objData["DocAssignToReview"] = globalAppdata['AppUserId'];
        objData["DocStatus"] = 'Pending';
        objData["DOSFrom"] ="";
        objData["DOSTo"] = ""
        objData["DocPriority"] = null;
        objData["CommandType"] = "Search_Documents";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },

    SaveAssignedTo: function (PatientDocumentData, PatientID, DocumentID) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientDocumentData=" + PatientDocumentData + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID + "&IsMessage=0";
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT");
    },

    UnLoad: function () {
        if (Document_AssignedTo.UnloadParent && Document_AssignedTo.UnloadParent == 'ParentUnload') {
            var parentPanelId = Document_AssignedTo.params["ParentCtrlPanelID"];//GetTab(Document_AssignedTo.params["ParentCtrl"]).PanelID;
            UnloadActionPan(Document_AssignedTo.params["ParentCtrl"], 'Document_AssignedTo', null, parentPanelId);
            delete Document_AssignedTo.UnloadParent;
        }
       else if (Document_AssignedTo.params["FromAdmin"] == "0") {
            if (Document_AssignedTo.params != null && Document_AssignedTo.params.ParentCtrl != null) {
                UnloadActionPan(Document_AssignedTo.params.ParentCtrl, 'Document_AssignedTo');
            }
            else
                UnloadActionPan(null, 'Document_AssignedTo');
        }
        else {
            RemoveAdminTab();
        }
    },
}