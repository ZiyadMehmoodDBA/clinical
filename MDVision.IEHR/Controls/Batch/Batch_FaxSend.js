Batch_FaxSend = {
    bIsFirstLoad: true,
    params: [],
    client_id: "",
    client_secret: "",
    user_id: -1,
    Type: "",
    ProviderId: null,
    FacilityId: null,
    ContactId: null,
    arrayReceipients: [],
    recipientCount: 0,
    coverPageBase64: "",
    AnnotateArray: [],// filename and base64 keys for storing and retriving data in this array.
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    Parent: "",
    // Programmer: Ahsan Nasir

    Load: function (params) {
        Batch_FaxSend.params = params;
        Batch_FaxSend.Parent = Batch_FaxSend.params.ParentCtrl;
        if (Batch_FaxSend.params.hasOwnProperty("PDFBase64") == true) {

            Batch_FaxSend.AnnotateArray["base64"] = params["PDFBase64"];
            Batch_FaxSend.AnnotateArray["filename"] = params["ParentCtrl"] + '.pdf';
            $("#fileEdit").show();
            $("#btnRemoveAttachment").removeClass('hidden');
            if (params["ParentCtrl"] != undefined) {
                if (params["ParentCtrl"] == "clinicalTabProgressNote") {
                    $('#filename').html("Clinical_ProgressNote.pdf");
                }
                else {
                    $('#filename').html(params["ParentCtrl"] + '.pdf');
                }
            }
            else {
                $('#filename').html('document' + '.pdf');
            }

            $('#fileToUpload').hide();
        }



        Batch_FaxSend.arrayReceipients = [];
        Batch_FaxSend.Type = null;


        Batch_FaxSend.ProviderId = null;
        Batch_FaxSend.FacilityId = null;

        // get User ID
        Batch_FaxSend.user_id = globalAppdata['AppUserId'];

        // Responsible for initializing dropdowns, Setting select events 
        Batch_FaxSend.InitProperties();

        Batch_FaxSend.ValidateFaxSend();
        
        EMRUtility.InitTinymceControl(false, 'message', 200);
        

        $('.mce-i-emoticons').parent().remove();
        $("#fileToUpload").on('change', function () {
            Batch_FaxSend.UploadFile();
        });
        if ($('#PatientProfile #hfPatientId').val() == "") {
            $("#letterCustomForm").attr("disabled", "disabled");
        }
    },
    InitProperties: function () {
        Batch_FaxSend.UserProfilesLookup();

        var options = $('#from');
        if (Batch_FaxSend.params.ParentCtrl == "Clinical_LabResultView") {
            $('#subject').val('Lab Result');
        }
        else if (Batch_FaxSend.params.ParentCtrl == "Clinical_ProcedureOrderView") {
            $('#subject').val('Procedure Order Requisition');
        }
        else if (Batch_FaxSend.params.ParentCtrl == "Clinical_RadiologyResultView") {
            $('#subject').val('Diagnostic Imaging Result');
        }
        else if (Batch_FaxSend.params.ParentCtrl == "Create_Letter") {
            $('#subject').val('Letter: ' + Batch_FaxSend.params.Subject);
        }
        else if (Batch_FaxSend.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Batch_FaxSend.params.ParentCtrl == "Patient_Referrals_Incoming_Detail") {
            $('#subject').val('Patient Referral');
        }
        else if (Batch_FaxSend.params.TabID && (Batch_FaxSend.params.TabID == "Patient_Document" || Batch_FaxSend.params.TabID == "batchTabDocuments" || Batch_FaxSend.params.TabID == "patTabDocuments")) {
            if (Batch_FaxSend.params.FileName) {
                $('#subject').val(Batch_FaxSend.params.FileName);
            }
            else { $('#subject').val('Patient Documents'); }
        }
        else { $('#subject').val('Progress note'); }

        options.on('change', function () {
            var faxNo = $('#from option:selected').val();


            var tempId = ($('#from').children(":selected").attr("ProviderId"));
            var temp = $('#from').children(":selected").attr("FacilityId");

            if (tempId != undefined || tempId != null) {
                Batch_FaxSend.ProviderId = ($('#from').children(":selected").attr("ProviderId"));
                Batch_FaxSend.Type = "Provider";
            }
            else if ((temp != undefined || temp != null)) {
                Batch_FaxSend.FacilityId = ($('#from').children(":selected").attr("FacilityId"));
                Batch_FaxSend.Type = "Facility";
            }
            else {
                Batch_FaxSend.FacilityId = null;
                Batch_FaxSend.ProviderId = null;
                Batch_FaxSend.Type = null;
            }


            $('#fromfaxnumber').val(faxNo);
            $('#fromfaxnumber').attr('disabled', 'disabled');

        });
    },

    addRecipient: function (recName, FaxNo, IsDbCall) {

        var recipients = $('#faxRecipients tbody');
        var rows = "";

        //if (Batch_FaxSend.FacilityId != null || Batch_FaxSend.FacilityId != undefined || Batch_FaxSend.ProviderId != null || Batch_FaxSend.ProviderId != undefined) {   // If "From" field is selected

        cleanFaxNo = FaxNo.replace(/\D/g, "");
        // cleanFaxNo =  cleanFaxNo;
        var rowCount = $('#faxRecipients tbody tr').length;
        if (rowCount == 0) {

            if (Batch_FaxSend.ProviderId != null || Batch_FaxSend.ProviderId != undefined) {
                if (IsDbCall == true) {
                    Batch_FaxSend.saveContact(recName, FaxNo, Batch_FaxSend.ProviderId, "Provider");
                }
            }
            else {
                if (IsDbCall == true) {
                    Batch_FaxSend.saveContact(recName, FaxNo, Batch_FaxSend.FacilityId, "Facility");
                }

            }
            Batch_FaxSend.ContactId++;
            recipients.append('<tr><td><i class="fa fa-user"></i> <span id="Name">' + recName + '</span> </td><td><i class="fa fa-print"></i> <span id="FaxNo">' + FaxNo + '</span> </td><td><a id="Contact' + Batch_FaxSend.ContactId + '" class="btn  btn-xs" href="#" onclick="Batch_FaxSend.removeResp(this);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
            prepareFaxReceipient(recName, cleanFaxNo);

        }
        else {
            var counter = 0;
            for (it = 0; it < Batch_FaxSend.arrayReceipients.length; it++) {

                var obj = Batch_FaxSend.arrayReceipients[it];

                if (obj.Name == recName && obj.FaxNumber == cleanFaxNo) {
                    counter++;
                }


            }
            if (counter == 0) { // 0 means the contact does not already exist

                if (Batch_FaxSend.ProviderId != null || Batch_FaxSend.ProviderId != undefined) {
                    if (IsDbCall == true) {
                        Batch_FaxSend.saveContact(recName, FaxNo, Batch_FaxSend.ProviderId, "Provider");
                    }
                }
                else {
                    if (IsDbCall == true) {
                        Batch_FaxSend.saveContact(recName, FaxNo, Batch_FaxSend.FacilityId, "Facility");
                    }
                }

                Batch_FaxSend.ContactId++;
                recipients.append('<tr><td><i class="fa fa-user"></i><span id="Name">' + recName + '</span> </td><td> <i class="fa fa-print"></i>  <span id="FaxNo">' + FaxNo + '</span> </td><td><a id="Contact' + Batch_FaxSend.ContactId + '" class="btn  btn-xs" href="#" onclick="Batch_FaxSend.removeResp(this);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                prepareFaxReceipient(recName, cleanFaxNo);


            }
            else {

            }
        }
        //}

        //else {
        //    utility.DisplayMessages("Please select a Fax Sender.", 2);
        //}




        function prepareFaxReceipient(faxName, faxNumber) {


            faxRecObject = {
                Name: faxName,
                FaxNumber: faxNumber
            }
            Batch_FaxSend.arrayReceipients.push(faxRecObject);
        }
    },
    addRecipient_New: function (contactName, FaxNo, IsDbCall) {
        var id = '';
        var type = '';
        if (Batch_FaxSend.ProviderId) {
            id = Batch_FaxSend.ProviderId;
            type = "Provider";
        }
        else if (Batch_FaxSend.FacilityId) {
            id = Batch_FaxSend.FacilityId;
            type = "Facility";
        }
        else {
            utility.DisplayMessages("Please select a Fax Sender.", 2);
            return false;
        }
        var contacts = [];
        var found = false;

        var cleanFaxNo = FaxNo.replace(/\D/g, "");
        var arr = jQuery.grep(Batch_FaxSend.arrayReceipients, function (a) {
            return a.FaxNumber && a.FaxNumber.replace(/\D/g, "") == cleanFaxNo;
        });
        if (arr.length == 0) {
            var faxRecObject = { ContactId: -1, Name: contactName, FaxNumber: cleanFaxNo, CC: false, To: true };
            Batch_FaxSend.arrayReceipients.push(faxRecObject);
            if (Batch_FaxSend.ProviderId) {
                Batch_FaxSend.SaveReceipient_New(contactName, FaxNo, Batch_FaxSend.ProviderId, "Provider");
            }
            else if (Batch_FaxSend.FacilityId) {
                Batch_FaxSend.SaveReceipient_New(contactName, FaxNo, Batch_FaxSend.FacilityId, "Facility");
            }
        }
        //else {
        //    utility.DisplayMessages("Selected Provider already exist in the list.", 4);
        //}
    },
    SaveReceipient_New: function (contactName, FaxNo, Id, type) {
        var recipients = $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #faxRecipients tbody");
        var cleanFaxNo = FaxNo.replace(/\D/g, "");
        Batch_FaxSend.saveContact(contactName, FaxNo, Id, type).done(function (response) {
            if (response.status != false) {
                recipients.append('<tr faxnum="' + cleanFaxNo + '"><td><i class="fa fa-user"></i><span id="Name">' + contactName + '</span></td><td><i class="fa fa-print"></i> <span id="FaxNo">' + FaxNo + '</span> </td><td><a id="Contact' + response.ContactId + '" class="btn  btn-xs" href="#" onclick="Batch_FaxSend.removeResp(this);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                Batch_FaxContacts.loadContacts();
                for (i = 0; i < Batch_FaxSend.arrayReceipients.length; i++) {
                    var obj = Batch_FaxSend.arrayReceipients[i];
                    if (obj.Name == contactName && obj.FaxNumber == cleanFaxNo) {
                        obj.ContactId = response.ContactId;
                    }
                }
            }
            else {
                for (i = 0; i < Batch_FaxSend.arrayReceipients.length; i++) {
                    var obj = Batch_FaxSend.arrayReceipients[i];
                    if (obj.Name == contactName && obj.FaxNumber == cleanFaxNo) {
                        Batch_FaxSend.arrayReceipients.splice(i, 1);
                    }
                }
            }
        });

    },
    saveContact: function (contactName, FaxNo, Id, Type) {
        var contactData = '';
        var obj = new Object();
        obj["Type"] = Type;
        obj["Id"] = Id;
        obj["ContactName"] = contactName;
        obj["FaxNumber"] = FaxNo;
        obj = JSON.stringify(obj);
        contactData = 'ContactData=' + obj;
        return MDVisionService.defaultService(contactData, "BATCH_FAX_CLINICAL", "SAVE_CONTACT");
    },
    removeResp: function (resp) { // Removes recipient
        //var elem = resp.parentElement.parentElement;
        var elem = $(resp).parent().parent();
        var x = '#' + resp.id;
        var rows = $(x).closest('tr').children('td');
        var name = rows.children('#Name').text();
        var faxNo = rows.children('#FaxNo').text();
        cleanFaxNo = faxNo.replace(/\D/g, "");
        elem.remove();

        for (i = 0; i < Batch_FaxSend.arrayReceipients.length; i++) {
            var obj = Batch_FaxSend.arrayReceipients[i];
            if (obj.Name == name && obj.FaxNumber == cleanFaxNo) {
                Batch_FaxSend.arrayReceipients.splice(i, 1);
            }
        }
        var textCC = '';
        $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #toCarbonCopy").val("");
        $.each(Batch_FaxSend.arrayReceipients, function (i) {
            if (Batch_FaxSend.arrayReceipients[i].CC == true) {
                textCC += Batch_FaxSend.arrayReceipients[i].Name + ";";
                $("#" + Batch_FaxSend.params.PanelID + " #frmBatch_FaxSend #toCarbonCopy").val(textCC);
            }
        });

        //Update To dropdown on Recipient delete
        var $outgoingRefsDdl = $('#' + Batch_FaxSend.params.PanelID + ' #ddlNotesOutgroinRefs');
        $outgoingRefsDdl.find('option').each(function (i, e) {
            if (e.text == name) {
                $outgoingRefsDdl.multiselect('deselect', e.value);
            }

        });

    },
    loadClassifyPages: function () {
        var params = [];
        params["ParentCtrl"] = "Batch_FaxSend";
        LoadActionPan("Batch_FaxClassifyPages", params);
    },
    sendFax: function () {
        var toFax = [];
        var toFaxNameNumb = [];
        for (arr = 0; arr < Batch_FaxSend.arrayReceipients.length ; arr++) {
            toFax.push("1" + Batch_FaxSend.arrayReceipients[arr].FaxNumber);
        }

        var obj = {};
        obj["sCallerID"] = $('#fromfaxnumber').val();
        obj["sToFaxNumber"] = toFax.join("|");
        obj["sCPFromName"] = $('#from option:selected').text();
        obj["sCPSubject"] = $('#subject').val();
        obj["sFileName_1"] = "ConverPage.pdf";
        obj["sFileContent_1"] = "";
        obj["sFileName_2"] = "";
        obj["sFileContent_2"] = "";
        obj["ProviderId"] = $('#from').children(":selected").attr('providerid');
        if (Batch_FaxSend.params.PatientId)
            obj["PatientId"] = Batch_FaxSend.params.PatientId;
        else
            obj["PatientId"] = 0;
        var SentToName = [];
        $("#faxRecipients > tbody > tr").each(function (index, element) {
            SentToName.push($(element).find('span[id="Name"]').text());
            toFaxNameNumb.push($(element).find('span[id="Name"]').text() + " - " + $(element).find('span[id="FaxNo"]').text());
        });
        obj["SentToName"] = SentToName.join("|");

        var custom = $('#from').children(":selected").attr("iscustom");
        var page = $('#from').children(":selected").attr("coverpage");
        page = page.replace(/ /g, "+");

        //obj["TimeZone"] = $('#from').children(":selected").attr('timezone');

        if (custom == "True" && (page != null || page != undefined || page != "")) {
            // use default cover page

            obj["sFileContent_1"] = page;
            obj["sFileName_2"] = Batch_FaxSend.AnnotateArray["filename"];
            obj["sFileContent_2"] = Batch_FaxSend.AnnotateArray["base64"].split('data:application/pdf;base64,').join('');

            Batch_FaxSend.FaxSend_Call(obj);

        } else {
            var $isCustomform = $('#letterCustomForm option:selected');
            //if (!$isCustomform.val()) {
            //    if (tinyMCE.activeEditor.getContent().replace("&nbsp;", "").length <= 72) {
            //        utility.DisplayMessages("Please write something in message.", 4);
            //    }
            //}
            var selection = $isCustomform.attr('refvalue');

            var data = new Object();
            data["From"] = $('#from').children(":selected").text();
            data["Subject"] = $('#subject').val();
            data["FaxNumber"] = $("#fromfaxnumber").val();
            data["FaxNotes"] = "fax_notes";
            data["CompanyName"] = $('#from').children(":selected").attr('companyname');
            data["PhoneNo"] = $('#from').children(":selected").attr('phoneno');
            data["TimeZone"] = $('#from').children(":selected").attr('timezone');
            data["SentToName"] = toFaxNameNumb.join("| ");

            Batch_FaxSend.convertHtmlToBase64(selection, data).done(function (resp) {
                if (typeof resp === "string") {
                    Batch_FaxSend.coverPageBase64 = resp;

                    obj["sFileContent_1"] = Batch_FaxSend.coverPageBase64.split('data:application/pdf;base64,').join('');;
                    obj["sFileName_2"] = Batch_FaxSend.AnnotateArray["filename"];
                    obj["sFileContent_2"] = Batch_FaxSend.AnnotateArray["base64"].split('data:application/pdf;base64,').join('');

                    Batch_FaxSend.FaxSend_Call(obj);
                }
            });
        }
    },

    FaxSend_Call: function (obj) {
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "Fax", "QueueFax")
                .done(function (res) {
                    var response = JSON.parse(res);
                    if (response.Status == "Success") {
                        utility.DisplayMessages("Fax has been queued successfully.", 1);
                        Batch_FaxSend.UnLoad();
                    } else {
                        utility.DisplayMessages(response.Result, 4);
                    }
                })
                .fail(function (res) {
                    utility.DisplayMessages("Fax sending failed.", 4);
                });
    },
    openContacts: function () {

        var paramsContacts = new Array();
        paramsContacts["ParentCtrl"] = "Batch_FaxSend";
        if (Batch_FaxSend.Type != undefined || Batch_FaxSend.Type != null) {
            if (Batch_FaxSend.Type == "Provider") {
                paramsContacts["Type"] = Batch_FaxSend.Type;
                paramsContacts["Id"] = Batch_FaxSend.ProviderId;
            }
            else {
                paramsContacts["Type"] = Batch_FaxSend.Type;
                paramsContacts["Id"] = Batch_FaxSend.FacilityId;
            }
            paramsContacts["CC"] = false;

            LoadActionPan('Batch_FaxContacts', paramsContacts);
        }
        else {
            utility.DisplayMessages("Please select a Fax Sender", 3);
        }
    },
    openCarvonCopyContacts: function () {

        var paramsContacts = new Array();
        paramsContacts["ParentCtrl"] = "Batch_FaxSend";
        if (Batch_FaxSend.Type != undefined || Batch_FaxSend.Type != null) {
            if (Batch_FaxSend.Type == "Provider") {
                paramsContacts["Type"] = Batch_FaxSend.Type;
                paramsContacts["Id"] = Batch_FaxSend.ProviderId;
            }
            else {
                paramsContacts["Type"] = Batch_FaxSend.Type;
                paramsContacts["Id"] = Batch_FaxSend.FacilityId;
            }
            paramsContacts["CC"] = true;

            LoadActionPan('Batch_FaxContacts', paramsContacts);
        }
        else {
            utility.DisplayMessages("Please select a Fax Sender", 3);
        }
    },
    UserProfilesLookup: function () {
        Batch_FaxSend.loadUserProfiles().done(function (resp) {
            if (resp) {
                var ProviderRows = JSON.parse(resp.FaxUsersProviders);
                var FacilityRows = JSON.parse(resp.FaxUsersFacilities);
                var options = $("#from");
                options.append($('<option/>', {
                    value: "",
                    html: "- Select -"
                }));
                //var rows = '<option> - Select -</option>';
                var rows = '';
                rows += '';
                count = ProviderRows.length + FacilityRows.length;

                if (count > 1) {
                    $.each(ProviderRows, function (i, item) {

                        rows += '<option IsCustom="' + item.IsCoverPage + '" CoverPage="' + item.CoverPage + '" No="' + i + '" ProviderId="' + item.ProviderId + '" id="' + item.ProviderId + '" value="' + item.FaxNumber + '" CompanyName="' + item.CompanyName + '" PhoneNo="' + item.PhoneNo + '" TimeZone="' + item.TimeZone + '" >' + item.DisplayName + '</option>';

                    });
                    options.append(rows);
                    rows = '';
                    $.each(FacilityRows, function (i, item) {
                        rows += '<option IsCustom="' + item.IsCoverPage + '" CoverPage="' + item.CoverPage + '" No="' + i + '"  FacilityId="' + item.FacilityId + '" id="' + item.FacilityId + '" value="' + item.FaxNumber + '" CompanyName="' + item.CompanyName + '" PhoneNo="' + item.PhoneNo + '" TimeZone="' + item.TimeZone + '" >' + item.DisplayName + '</option>';
                    });
                    options.append(rows);

                }
                else {

                    $.each(ProviderRows, function (i, item) {

                        rows += '<option IsCustom="' + item.IsCoverPage + '" CoverPage="' + item.CoverPage + '" No="' + i + '" ProviderId="' + item.ProviderId + '" id="' + item.ProviderId + '" value="' + item.FaxNumber + '" CompanyName="' + item.CompanyName + '" PhoneNo="' + item.PhoneNo + '" TimeZone="' + item.TimeZone + '" > ' + item.DisplayName + '</option>';

                        $('#fromfaxnumber').val(item.FaxNumber);
                        $('#fromfaxnumber').attr('disabled', 'disabled');
                    });
                    options.append(rows);
                    rows = '';
                    $.each(FacilityRows, function (i, item) {
                        rows += '<option IsCustom="' + item.IsCoverPage + '" CoverPage="' + item.CoverPage + '" No="' + i + '"  FacilityId="' + item.FacilityId + '" id="' + item.FacilityId + '" value="' + item.FaxNumber + '" CompanyName="' + item.CompanyName + '" PhoneNo="' + item.PhoneNo + '" TimeZone="' + item.TimeZone + '" >' + item.DisplayName + '</option>';


                        $('#fromfaxnumber').val(item.FaxNumber);
                        $('#fromfaxnumber').attr('disabled', 'disabled');
                    });
                    options.append(rows);

                    $('#from option:eq(1)').prop('selected', true);
                    var tempId = ($('#from').children(":selected").attr("ProviderId"));
                    var temp = $('#from').children(":selected").attr("FacilityId");

                    if (tempId != undefined || tempId != null) {
                        Batch_FaxSend.ProviderId = ($('#from').children(":selected").attr("ProviderId"));
                        Batch_FaxSend.Type = "Provider";
                    }
                    else if ((temp != undefined || temp != null)) {
                        Batch_FaxSend.FacilityId = ($('#from').children(":selected").attr("FacilityId"));
                        Batch_FaxSend.Type = "Facility";
                    }
                    else {
                        Batch_FaxSend.FacilityId = null;
                        Batch_FaxSend.ProviderId = null;
                        Batch_FaxSend.Type = null;
                    }


                    // select if only one row
                    //if (ProviderRows.length > 0) {
                    //    $('#from option:eq(1)').prop('selected', true);
                    //    Batch_FaxSend.Type = "Provider";
                    //}
                    //else if (FacilityRows.length > 0) {
                    //    $('#from option:eq(1)').prop('selected', true);
                    //    Batch_FaxSend.Type = "Facility";
                    //}



                }

            } else {
                utility.DisplayMessages("Please add fax settings first.", 4);
                //Batch_FaxSend.UnLoad();
            }
        });

        var self = $('#Batch_FaxSend');
        self.loadDropDowns(true).done(function () {
            if (Batch_FaxSend.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail" || Batch_FaxSend.params.ParentControl == "BatchDocuments" || Batch_FaxSend.params.ParentCtrl == "Batch_Fax") {
                Batch_FaxSend.loadPatientReferrals();
            } else {
                Batch_FaxSend.loadNotesReferrals();
            }
        });
    },
    loadUserProfiles: function () {
        var id = globalAppdata['AppUserId'];
        var userId = "UserId=" + id;
        return MDVisionService.defaultService(userId, "BATCH_FAX_CLINICAL", "LOAD_USER_PROFILES");
    },
    ValidateFaxSend: function () {
        $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

                    //from: {
                    //    group: '.col-sm-6',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //fromfaxnumber: {
                    //    group: '.col-sm-6',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    subject: {
                        name: 'subject',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    }
                    ,
                    //fromFaxnumber: {
                    //    name:'fromFaxnumber',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //faxnumber: {
                    //    name: 'faxnumber',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //}
                    //,
                    from: {
                        name: 'from',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //,
                    //to: {
                    //    name: 'to',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //}

                }
            })
            .on('success.form.bv', function (e) {
                e.preventDefault();
                if (Batch_FaxSend.arrayReceipients.length > 0) {
                    if (Batch_FaxSend.AnnotateArray["base64"] != undefined && Batch_FaxSend.AnnotateArray["base64"] != "") {
                        Batch_FaxSend.sendFax();
                    }
                    else {
                        utility.DisplayMessages("Please select a file.", 4);
                    }

                }
                else {
                    utility.DisplayMessages("Please add some recipients first.", 4);
                }
            });
    },


    UnLoad: function () {
        Batch_FaxSend.AnnotateArray["base64"] = "";
        Patient_Document.FaxDocsArray = [];
        Patient_Document.AttachedDocsArray = [];
        Batch_FaxSend.AnnotateArray["filename"] = "";
        $('#filename').html('');
        $("#fileEdit").hide();
        $("#btnRemoveAttachment").addClass('hidden');
        $("#Attach_Import_file").val('');
        if (Patient_Document.params["ParentCtrl"] && Patient_Document.params["ParentCtrl"] == "demographicDetail") {
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            UnloadActionPan(Batch_FaxSend.params["ParentCtrl"], 'Batch_FaxSend', null, parentPanelId);
            delete Batch_FaxSend.UnloadParent;
        } else {
            if (Batch_FaxSend.UnloadParent && Batch_FaxSend.UnloadParent == 'ParentUnload') {
                var parentPanelId = GetTab(Batch_FaxSend.params["ParentCtrl"]).PanelID;
                UnloadActionPan(Batch_FaxSend.params["ParentCtrl"], 'Batch_FaxSend', null, parentPanelId);
                delete Batch_FaxSend.UnloadParent;
            }
        }
       
        if (Batch_FaxSend.params.TabID == "patTabDocuments") {
            Patient_Document.params.ActionPanContainer = "actionPanPatientDocument";
            Patient_Document.params.PanelID = "ctrlPanPatient #pnlPatientDocument";
            Patient_Document.params.TabID = "patTabDocuments";
        }
        else if (Batch_FaxSend.params.TabID == "batchTabDocuments") {
            Patient_Document.params.ActionPanContainer = "actionPanBatchDocuments";
            Patient_Document.params.PanelID = "pnlBatchDocuments";
            Patient_Document.params.TabID = "batchTabDocuments";
        }
        if (Patient_Document.params["ParentCtrl"] && Patient_Document.params["ParentCtrl"] != "demographicDetail") {
            if (Batch_FaxSend.Parent != null || Batch_FaxSend.Parent != undefined) {
                UnloadActionPan(Batch_FaxSend.Parent);
            }
            else {
                UnloadActionPan();
            }
        }
        else
            UnloadActionPan(Batch_FaxSend.params["ParentCtrl"], 'Batch_FaxSend');
    },
    UploadFile: function () {
        var files = $('#fileToUpload')[0].files;
        if (files.length > 0) {
            var filename = files[0].name;
            var extension = filename.replace(/^.*\./, '');
            if (extension == filename) {
                extension = '';
            } else {
                extension = extension.toLowerCase();
            }
            if (extension == 'pdf') {
                Batch_FaxSend.AnnotateArray["filename"] = files[0].name;
                var baseData = Batch_FaxSend.GetBase64(files[0]);
            } else {
                utility.DisplayMessages("Please attach PDF files only.", 4);
            }
        }
    },
    AnnotateFile: function () {
        //var params = [];
        //params["ParentCtrl"] = "Batch_FaxSend";

        //LoadActionPan("Batch_FaxSendAnnotate", params);

        var params = [];
        params["ParentCtrl"] = "Batch_FaxSend";

        var FaxHtmlData=Batch_FaxSend.AnnotateArray["base64"].split('data:application/pdf;base64,').join('');
        params["FaxHtml"] = FaxHtmlData;

        LoadActionPan("Batch_FaxView", params);

    },
    GetBase64: function (file) {
        var reader = new FileReader();
        var base64 = reader.readAsDataURL(file);
        reader.onload = function () {

            // get Cover Page here

            Batch_FaxSend.AnnotateArray["base64"] = reader.result;
            $("#fileEdit").show();
            $("#btnRemoveAttachment").removeClass('hidden');
            //$('#filename').html(file.name);
            Batch_FaxSend.SetFileName();
        };
        reader.onerror = function (error) {
            utility.DisplayMessages("File not correct, please attach a correct file.", 4);

        };
    },
    LoadPdfCoverPage: function (data, isCustomForm) {
        data = JSON.stringify(data);
        data = data.replace("fax_notes", isCustomForm ? btoa($('#frmCustomFormPreview').html()) : btoa(tinyMCE.activeEditor.getContent()));
        base64 = "CoverPageData=" + data;
        return MDVisionService.defaultService(base64, "BATCH_FAX_CLINICAL", "GET_COVER");
    },
    DataURLtoBlob: function dataURLtoBlob(dataurl) {
        var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new Blob([u8arr], { type: mime });
    },
    selectCustomFormLetter: function (obj) {
        if (obj) {
            var selectedOption = $(obj).find("option:selected");
            var id = selectedOption.val();
            if (selectedOption.attr('refvalue') == "Custom Form") {
                $('#divTinyMce').hide();
                $('#frmCustomFormPreview').show();
                Batch_FaxSend.loadCustomForm(id)
            } else if (selectedOption.attr('refvalue') == "Letter") {
                $('#divTinyMce').show();
                $('#frmCustomFormPreview').hide();
                Batch_FaxSend.loadTemplateLetter(id);
            } else {
                $('#divTinyMce').show();
                $('#frmCustomFormPreview').hide();
                tinyMCE.activeEditor.setContent('');
            }

        }
    },
    loadCustomForm: function (customFormId) {
        Clinical_CustomFormsDetails.customFormFill_DBCall(customFormId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var actionPan = $('#Batch_FaxSend #frmCustomFormPreview');
                if (response.listCustomForm[0].CustomFormHTML != "") {
                    var html = response.listCustomForm[0].CustomFormHTML;
                    $("#customFormDetails").empty();
                    $("#customFormDetails").html($(html));
                    $("#customFormDetails").children().find('#txtProblemsCustomForm').attr('oninput', 'Batch_FaxSend.BindICD9AutoComplete(this)');
                    $("#customFormDetails").children().find('#txtProceduresCustomForm').attr('oninput', 'Batch_FaxSend.bindAutoComplete(this)');

                    $("#customFormDetails").find('.questionAction').remove();
                    $("#customFormDetails").attr('canvasCol', response.listCustomForm[0].CanvasCols);

                    //For initilizing toggle for question groups.
                    $($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find($('.toggleButton'))).click(function () {
                        var section = $(this).closest('section');
                        if (section) {
                            if (section.hasClass('active')) {
                                section.removeClass('active');
                                $(section).find('.toggle-content').css('display', 'none');
                            }
                            else {
                                section.addClass('active');
                                $(section).find('.toggle-content').css('display', 'block');
                            }
                        }
                    });
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " .toggleEditableHeader").find('#lnkQuestionGroupTitle').remove()
                    Batch_FaxSend.setWidthofTable();
                    Batch_FaxSend.initilizeDatePickers();
                    Batch_FaxSend.initilizeTimePickers();
                    $("#customFormDetails .toggleCheck").each(function () {
                        var $this = $(this);
                        var $patent = $($this.closest('li').find('.toolcontroldiv'));
                        $this.themePluginIOS7Switch();
                        if ($patent && $patent.attr('defaultselection')) {
                            if ($patent.attr('defaultselection') == "0")
                                $($patent.find('.ios-switch')).removeClass('on').removeClass('off').addClass('on');
                            else
                                $($patent.find('.ios-switch')).removeClass('on').removeClass('off').addClass('off');
                        }
                    });
                    $("#customFormDetails #customFormIosSwitchPreview").remove();
                }
                if ($("[id^='customFormMultipleSelectCombo_").find('select').length > 0) {
                    Batch_FaxSend.initilizeMultiSelectCustomFormPreview();
                }

                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                $('#frmPreview').data('serialize', $('#frmPreview').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    convertHtmlToBase64: function (selection, data) {
        var def = $.Deferred();

        if (selection == "Custom Form") {
            var html = $('#containerCustomFormPreview').html();
        } else {
            html = tinyMCE.activeEditor.getContent();
        }
        var formHeaderHtml = '<h4> Fax Transmission: </h4><hr class="line mt-none">' +
         '<div class="ml-md"><h4> From: </h4>' +
            '<ul class="list-unstyled">' +
            '<li>Name: <span>' + data.From + '</span></li>' +
            '<li>Fax Number: <span>' + data.FaxNumber + '</span></li>' +
            '<li>Company Name: <span>' + data.CompanyName + '</span></li>' +
            '<li>Phone No: <span>' + data.PhoneNo + '</span></li>' +
            '<li>TimeZone: <span>' + data.TimeZone + '</span></li>' +
            '<li>Date & Time: <span>' + new Date(new Date().getTime()).toLocaleString() + '</span></li>' +
            '</ul></div>' +
            '<div class="ml-md"><h4> To: </h4>' +
            '<ul class="list-unstyled">' +
            '<li>Name: <span>' + data.SentToName + '</span></li>' +
            '</ul></div>' +
            '<h4> Subject: <span>' + data.Subject + '</span></h4><hr class="line mt-none">' +
        '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>';

        var html = formHeaderHtml;
        var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
        $("#CustomeformprintparentdivFax").append(ProgressNoteSign);
        $("#customformsign #txtTextField").each(function (i, e) {
            var scrollHeight = e.scrollHeight;
            if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                $(e).height(scrollHeight + 50);
            } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                $(e).height(scrollHeight + 40);
            } else {
                $(e).height(scrollHeight + 20);
            }
            $(e).parents('.toolcontroldiv').addClass('heightReset');
        });
        $("#customformsign #txtFreeText").each(function (i, e) {
            var scrollHeight = e.scrollHeight;
            if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                $(e).height(scrollHeight + 130);
            } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                $(e).height(scrollHeight + 100);
            } else {
                $(e).height(scrollHeight + 50);
            }
            $(e).parents('.toolcontroldiv').addClass('heightReset');
        });
        $('#customformsign span.required').remove();
        kendo.drawing.drawDOM("#customformsign", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "3mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: $("#Batch_FaxSend #page-templateLegacy").html()
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                Patient_CustomForm.pdf = dataURL;
                var params = [];
                params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                params["PreviewPdf"] = true;
                def.resolve(dataURL);
                $("#CustomeformprintparentdivFax").html('');

            });

        });
        return def.promise();
    },

    loadTemplateLetter: function (letterId) {

        Batch_FaxSend.FillLetter(letterId).done(function (response) {

            response = JSON.parse(response);

            if (response.status != false) {
                if (response.PatientLetterContentCount != 0) {

                    var strToReplaceByte = '';

                    var strToReplace = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="184" class="TagInserted" style="min-width: 10px; border: none; width: 240px;" readonly="readonly" type="text" value="{{ Primary Care Provider eSignature }}" />') > -1) {
                        strToReplace = '<input id="184" class="TagInserted" style="min-width: 10px; border: none; width: 240px;" readonly="readonly" type="text" value="{{ Primary Care Provider eSignature }}" />';
                    }

                    if (response.PatientLetterContent.indexOf('<input id="191" class="TagInserted" style="min-width: 10px; border: none; width: 128px;" readonly="readonly" type="text" value="9737397163STATE FARM INSURANCE" />') > -1) {
                        var str = response.PatientLetterContent.substring(response.PatientLetterContent.indexOf('<input id="191"'));
                        str = str.substring(0, str.indexOf('/>') + 2);
                        response.PatientLetterContent = response.PatientLetterContent.replace(str, $(str).css("width", "300px")[0].outerHTML);

                    }


                    if (response.PatientLetterContent.indexOf('<input id="192" class="TagInserted" style="min-width: 10px; border: none; width: 142px;" readonly="readonly" type="text" value="respiratory treatment and nebulizer" />') > -1) {
                        var str = response.PatientLetterContent.substring(response.PatientLetterContent.indexOf('<input id="192"'));
                        str = str.substring(0, str.indexOf('/>') + 2);
                        response.PatientLetterContent = response.PatientLetterContent.replace(str, $(str).css("width", "230px")[0].outerHTML);
                    }

                    var strToReplaceCurentProvider = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="185" class="TagInserted" style="min-width: 10px; border: none; width: 205px;" readonly="readonly" type="text" value="{{ Current Provider eSignature }}" />') > -1) {
                        strToReplaceCurentProvider = '<input id="185" class="TagInserted" style="min-width: 10px; border: none; width: 205px;" readonly="readonly" type="text" value="{{ Current Provider eSignature }}" />';
                    }

                    var strToReplaceMiscellaneous = '<img id="imgProvidereSignature" alt="" />';
                    if (response.PatientLetterContent.indexOf('<input id="186" class="TagInserted" style="min-width: 10px; border: none; width: 184px;" readonly="readonly" type="text" value="{{ Miscellaneous eSignature }}" />') > -1) {
                        strToReplaceMiscellaneous = '<input id="186" class="TagInserted" style="min-width: 10px; border: none; width: 184px;" readonly="readonly" type="text" value="{{ Miscellaneous eSignature }}" />';
                    }

                    if (response.eSignatureFaxBase64) {

                        var isIEBrowser = EMRUtility.GetIEVersion() > 0 ? true : false;
                        var base64Type = "image/png;";
                        if (isIEBrowser) {
                            base64Type = "image/gif";
                        }

                        var faxProvidereSignature = "<img id=\"imgFaxProvidereSignature\" src=\"data:" + base64Type + ";base64," + response.eSignatureFaxBase64 + "\" />";
                        response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Fax Provider eSignature }}/g, faxProvidereSignature);
                    }

                    if (response.eSignatureBase64 != "") {

                        var isIEBrowser = EMRUtility.GetIEVersion() > 0 ? true : false;
                        var base64Type = "image/png;";
                        if (isIEBrowser) {
                            base64Type = "image/gif";
                        }

                        strToReplaceByte = "<img id=\"imgProvidereSignature\" src=\"data:" + base64Type + ";base64," + response.eSignatureBase64 + "\" />";
                    }

                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplace, "gi"), strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplaceCurentProvider, "gi"), strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(new RegExp(strToReplaceMiscellaneous, "gi"), strToReplaceByte);
                    var isReadonly = false;
                    if (Create_Letter.params.Status == "Signed") {
                        isReadonly = true
                    }
                    response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Miscellaneous eSignature }}/g, strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Primary Care Provider eSignature }}/g, strToReplaceByte);
                    response.PatientLetterContent = response.PatientLetterContent.replace(/{{ Current Provider eSignature }}/g, strToReplaceByte);

                    tinyMCE.activeEditor.setContent(response.PatientLetterContent, { format: 'raw' });
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    FillLetter: function (letterId) {
        var objData = {};
        objData["TemplateLetterId"] = letterId;
        objData["PatientId"] =   $('#hfPatientId').val();
        objData["Mode"] = "Add";
        objData["ProviderId"] = $('#from option:selected').attr('providerid');
        objData["commandType"] = "Get_Content_Of_LETTER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "PatientTemplateLetter");
    },


    setWidthofTable: function () {
        $("#customFormDetails").find("#tblContext").each(function () {
            var table = $(this);
            if (table) {
                cols = $(table.find('tr:first-child')).find('td').length;
                if (cols) {
                    var totalCols = (parseInt(cols) + 1);
                    var widthofTd = Math.floor(100 / totalCols);
                    $(".tblContextTd").css({ 'width': widthofTd + '%' });
                }
            }
        });
    },
    initilizeDatePickers: function () {
        $("#customFormDetails").find(".dateField").each(function () {
            var dateFormat = $(this).attr('dateformat');
            var date_format = 'dd/mm/yyyy';
            if (dateFormat)
                date_format = dateFormat.toLowerCase();
            $(this).datepicker({
                format: date_format,
            }).on('changeDate', function (e) {
                $(this).datepicker('hide');
            });
            if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                this.value = utility.RemoveTimeFromDate(null, this.value);
                $(this).datepicker("setDate", this.value);
            }
        });
    },
    initilizeTimePickers: function () {
        $("#customFormDetails").find(".timeField").each(function () {
            var timeFormat = $(this).attr('timeformat');
            var showMerdn = false;
            if (timeFormat == "24")
                showMerdn = false;
            else if (timeFormat == "12")
                showMerdn = true;
            $(this).timepicker({
                showMeridian: showMerdn,
                appendWidgetTo: $(this).closest('.controlContainerDiv'),
            }).on('show.timepicker', function (e) {
                $(".bootstrap-timepicker-widget").css("top", "100%");
            });
            if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                $(this).timepicker("setTime", this.value);
            }
        });
    },
    initilizeMultiSelectCustomFormPreview: function () {
        $("[id^='customFormMultipleSelectCombo_").find('select').each(function (i, e) {
            $(e).multiselect('destroy');
            $(e).attr('multiple', 'multiple')
            $(e).multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $(e).parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });
            var defaultValues = $(e).parent().parent().parent().attr('defaultselection');
            $(e).val('');

            $(e).multiselect('select', defaultValues.split(','));
            $(e).multiselect("refresh");
        });
        $("[id^='customFormMultipleSelectCombo_").find('select').next().next().remove()
    },

    BindICD9AutoComplete: function (element) {

        var selectedProblemTool = $(element).parents("div[id*='toolProblems_']");
        if (selectedProblemTool) {
            var parentId = $(selectedProblemTool).attr('id');
            $(element).attr("data-popupunload", "false");

            var descriptionCrtl = $(element);
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Batch_FaxSend", null, false, null, parentId);
        }
    },

    bindAutoComplete: function (element) {

        var selectedProcedureTool = $(element).parents("div[id*='toolProcedures_']");
        if (selectedProcedureTool) {
            var parentId = $(selectedProcedureTool).attr('id');
            $(element).attr("data-popupunload", "false");

            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', $(element), null, true, -1, "CPT", true, "Batch_FaxSend", element, true, null, parentId);
        }
    },

    BindProcedureListItem: function (cptCode, cptDescription, ContainerCrtl) {

        var selectedProcedureTool = $(ContainerCrtl).parents("div[id*='toolProcedures_']");

        var currId = -1;
        $(selectedProcedureTool).find("ul#customFormProceduresList li[id*='-']").each(function (i, item) {
            currId = $(this).attr("id");
        });

        currId = parseInt(currId) + (-1);
        var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' cptcode=\"" + cptCode + "\" cptDescription=\"" + cptDescription + "\"><a href='#'>" + cptCode + " - " + cptDescription + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProcedure($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

        var IsAlreadyExist = false;

        $(selectedProcedureTool).find("ul#customFormProceduresList li").each(function (i, item) {
            var code = $(item).attr('cptcode');
            var desc = $(item).attr('cptDescription');
            if (cptCode == code && $.trim(cptDescription.toLowerCase()) == $.trim(desc.toLowerCase())) {
                IsAlreadyExist = true;
                return;
            }
        });

        if (!IsAlreadyExist) {

            var procedureExist = $(selectedProcedureTool).find("ul#customFormProceduresList > li").length;

            if (procedureExist == 0) {
                var procedureDiv = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend .proceduresListDiv').clone().removeClass('proceduresListDiv');
                $(procedureDiv).find("ul#customFormProceduresList").append(li);
                $(procedureDiv).css('display', 'block');
                $(selectedProcedureTool).append($(procedureDiv));
                $(selectedProcedureTool).css('height', 'auto');
            }
            else {
                $(selectedProcedureTool).find("ul#customFormProceduresList").append(li);
                $(selectedProcedureTool).children().last().css('display', 'block');
            }

            $(ContainerCrtl).val('');
            if ($(ContainerCrtl).attr('style')) {
                $(ContainerCrtl).removeAttr('style');
            }
        }
        else {
            utility.DisplayMessages('Procedure already added', 2);
            $(ContainerCrtl).val('');
        }
    },

    LoadPatientDocuments: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmBatch_FaxSend";
        params["FacilityId"] = "-1";
        params["NotesId"] = Clinical_ProgressNote.params.NotesId;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_FaxSend";
        LoadActionPan('Patient_Document', params);
    },
    ShowMergedFile: function () {
        if (Batch_FaxSend.params["PDFBase64"] != "" || Batch_FaxSend.params["PDFBase64"] != null || Batch_FaxSend.params["PDFBase64"] != undefined) {
            Batch_FaxSend.AnnotateArray["base64"] = Batch_FaxSend.params["PDFBase64"];
            $("#fileEdit").show();
            $("#btnRemoveAttachment").removeClass('hidden');
            $('#fileToUpload').hide();
            Batch_FaxSend.SetFileName();
            Batch_FaxSend.AnnotateArray["filename"] = $('#filename').text();
        }
    },

    BufferFile: function (obj) {
        var toReturn = true;

        if (obj.files && obj.files.length != 0) {
            Batch_FaxSend.ValidateUploadedFiles();
            Batch_FaxSend.FilesContainer.Files = obj.files;

            Batch_FaxSend.AttachFile();
        }
        else {
            delete Batch_FaxSend.FilesContainer.Files;
            $('#' + Batch_FaxSend.params.PanelID + ' #Attach_Import_file').val('');
            toReturn = false;
        }
        return toReturn;
    },
    ValidateUploadedFiles: function () {
        var files = $('#Attach_Import_file').get(0).files;
        for (var i = 0; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/pdf" && fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp" && fileType != "text/html") {
                utility.DisplayMessages("File Type is Invalid", 4);
                $('#' + Batch_FaxSend.params.PanelID + ' #Attach_Import_file').val('');
                return false;
            }
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                $('#' + Batch_FaxSend.params.PanelID + ' #Attach_Import_file').val('');
                return false;
            }
        }
        return true;
    },

    AttachFile: function () {
        var objDefIportSave = $.Deferred();
        var data = new FormData();
        var counter = 0;
        var fileCount = Batch_FaxSend.FilesContainer.Files.length;

        $.each(Batch_FaxSend.FilesContainer.Files, function (key, value) {
            var objDef = $.Deferred();
            filenameFull = value.name;
            FileType = value.type;
            data.append("FileType", value.type);
            data.append("FileName", value.name);
            data.append(key, value);
            counter = counter + 1;
            if (fileCount == counter) {
                objDefIportSave.resolve("ok");
            }
        });
        data.append("AttchedFilesStream", (Batch_FaxSend.AnnotateArray["base64"] == null || Batch_FaxSend.AnnotateArray["base64"] == "") ? "" : Batch_FaxSend.AnnotateArray["base64"]);
        objDefIportSave.then(function () {
            var documentCall = Patient_Document.FillMyComputerDocsForFax(data);
            $.when(documentCall).done(function (response) {
                if (response.status != false) {
                    Batch_FaxSend.params["PDFBase64"] = response.MergedContent;
                    Batch_FaxSend.ShowMergedFile();
                    utility.DisplayMessages("Successfully Attached.", 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        });
    },

    RemoveAttachment: function (obj) {

        utility.myConfirm('Are you sure you want to delete the attachment?', function () {
            Batch_FaxSend.AnnotateArray["base64"] = "";
            Patient_Document.FaxDocsArray = [];
            Patient_Document.AttachedDocsArray = [];
            Batch_FaxSend.AnnotateArray["filename"] = "";
            $('#filename').html('');
            $("#fileEdit").hide();
            $(obj).addClass('hidden');
            $("#Attach_Import_file").val('');
            utility.DisplayMessages("Successfully Deleted.", 1);
        }, function () {
        },
         'Delete Attachment'
        );
    },

    SetFileName: function () {
        var patientName = $("#PatientProfile").find('#hfPatientFullNameOnly').val();
        var fName = patientName.substr(patientName.indexOf(",") + 1).trim();
        var lName = patientName.substr(0, patientName.indexOf(",")).trim();
        var dt = $.datepicker.formatDate('ddmmyy', new Date());
        var fileName = fName + lName + dt;
        $('#filename').html(fileName + '.pdf');
    },

    AddContacts: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmBatch_FaxSend";
        params["IsOptional"] = true;
        params["RefCtrl"] = "txtRefProvider";
        //params["RefCtrlHidden"] = "hfRefProvider";
        params["ParentCtrl"] = "Batch_FaxSend";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    loadNotesReferrals: function () {
        if (Batch_FaxSend.params.ParentControl && Batch_FaxSend.params.ParentControl == "BatchDocuments") {
            Batch_FaxSend.IntializeReferralsMultiSelect();
        } else {
            var noteId = $('#frmClinicalProgressNote #hfNoteId').val();
            var patientId = $('#hfPatientId').val();
            var data = "ID=" + patientId;
            if (noteId && patientId) {
                var data = "ID=" + noteId + "&" + "ID2=" + patientId;
            }

            MDVisionService.lookups('GetOutgoingReferrals', true, data).done(function (result) {
                if (!$.isEmptyObject(result)) {
                    var options = JSON.parse(result["GetOutgoingReferrals"]);
                    var $outgoingRefsDdl = $('#' + Batch_FaxSend.params.PanelID + ' #ddlNotesOutgroinRefs');

                    $outgoingRefsDdl.empty();
                    $.each(options, function (i, item) {
                        $outgoingRefsDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refvalue: item.RefValue
                            })
                        );
                    });
                }
            }).then(function () {
                Batch_FaxSend.IntializeReferralsMultiSelect();
            });
        }
    },
    loadPatientReferrals: function () {
        var patientId = $('#hfPatientId').val();
        if (patientId) {
            var data = "ID=" + patientId;

            MDVisionService.lookups('GetPatientReferrals', true, data).done(function (result) {
                var options = JSON.parse(result["GetPatientReferrals"]);
                var $outgoingRefsDdl = $('#' + Batch_FaxSend.params.PanelID + ' #ddlNotesOutgroinRefs');

                $outgoingRefsDdl.empty();
                $.each(options, function (i, item) {
                    $outgoingRefsDdl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                            refvalue: item.RefValue
                        })
                    );
                });
            }).then(function () {
                Batch_FaxSend.IntializeReferralsMultiSelect();
            });
        } else {
            Batch_FaxSend.IntializeReferralsMultiSelect();
        }

    },

    IntializeReferralsMultiSelect: function () {
        var $outgoingRefsDdl = $('#' + Batch_FaxSend.params.PanelID + ' #ddlNotesOutgroinRefs');

        $outgoingRefsDdl.multiselect('destroy');
        $outgoingRefsDdl.multiselect({
            maxHeight: 247,
            includeSelectAllOption: true,
            onChange: function (option, checked) {
                if (option) { //if option is undefined means, 'Select All' was selected
                    var cleanFaxNo = option[0].attributes.refvalue.value.replace(/\D/g, "");
                    if (checked) {
                        if (option && option[0].attributes.refvalue.value) {
                            Batch_FaxContacts.params.CC = false;
                            utility.callbackAfterAllDOMLoaded(function () {
                                Batch_FaxSend.addRecipient_New(option.text(), option[0].attributes.refvalue.value);
                            });
                        } else if (option && option[0].attributes.refvalue.value == "") {
                            utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                            $outgoingRefsDdl.multiselect('deselect', option.val());
                        }
                    } else { //Update Recipient list if Provider is unchecked from To dropdown
                        $.each(Batch_FaxSend.arrayReceipients, function (i, obj) {
                            if (obj.Name == option.text() && obj.FaxNumber == cleanFaxNo) {
                                Batch_FaxSend.arrayReceipients.splice(i, 1);
                                return false;
                            }
                        });
                        $("#faxRecipients tr[faxnum='" + cleanFaxNo + "']").remove();
                    }
                } else { // dirty fix to handle Deselect All
                    if (!checked) {
                        var arrCC = $.grep(Batch_FaxSend.arrayReceipients, function (e) {
                            if (!(e.CC)) {
                                $("#faxRecipients tr[faxnum='" + e.FaxNumber + "']").remove();
                            }
                            return e.CC;
                        });
                        Batch_FaxSend.arrayReceipients = arrCC;
                    }
                }
            },
            onSelectAll: function () {
                var isFaxMissing = 0;
                var $outgoingRefsDdl = $('#' + Batch_FaxSend.params.PanelID + ' #ddlNotesOutgroinRefs');
                $outgoingRefsDdl.find('option').each(function (i, e) {
                    if ($(e).attr('refvalue')) {
                        Batch_FaxContacts.params.CC = false;
                        utility.callbackAfterAllDOMLoaded(function () {
                            Batch_FaxSend.addRecipient_New($(e).text(), $(e).attr('refvalue'), true);
                        });
                    } else {
                        $outgoingRefsDdl.multiselect('deselect', e.value);
                        isFaxMissing++;
                    }
                });
                if (isFaxMissing) {
                    utility.DisplayMessages("Fax number missing for the selected Provider.", 4);
                }
            }
        });
        if (length >= 1) {
            $outgoingRefsDdl.multiselect('select', 1, true);
        }
    },

}