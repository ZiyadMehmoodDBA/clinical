Clinical_AddReportHeaderTemplate = {
    bIsFirstLoad: true,
    params: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    //Start || 24 August, 2016 || Talha Tanweer ||
    Load: function (params) {
        Clinical_AddReportHeaderTemplate.params = params;


        if (Clinical_AddReportHeaderTemplate.params.PanelID != 'pnlAddReportHeaderTemplate') {
            Clinical_AddReportHeaderTemplate.params.PanelID = Clinical_AddReportHeaderTemplate.params.PanelID + ' #pnlAddReportHeaderTemplate';
        } else {
            Clinical_AddReportHeaderTemplate.params.PanelID = 'pnlAddReportHeaderTemplate';
        }
        Clinical_AddReportHeaderTemplate.specialityCheckedIds = [];
        Clinical_AddReportHeaderTemplate.providerCheckedIds = [];
        Clinical_AddReportHeaderTemplate.SpecialtyIds = '';
        Clinical_AddReportHeaderTemplate.ProviderIds = '';

        //Clinical_AddReportHeaderTemplate.isEntitySelected();
        if (Clinical_AddReportHeaderTemplate.bIsFirstLoad) {
            Clinical_AddReportHeaderTemplate.bIsFirstLoad = false;
            $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' select[name!="hiddenProvider"]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            })
            var self = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID);
            Clinical_AddReportHeaderTemplate.TemplateTagsLookupsLoad();
            var data = "EntityId=";
            self.loadDropDowns(true, data).done(function () {

                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlEntity').multiselect('destroy');

                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlFacility').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true
                });
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlEntity').multiselect('destroy');
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlEntity').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true
                });
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlEntity').multiselect('selectAll', false);
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlEntity').multiselect('updateButtonText');
                if (Clinical_AddReportHeaderTemplate.params.mode == "Edit") {
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #headingTitle').text('Edit Report Header Template');
                    Clinical_AddReportHeaderTemplate.fillReportHeader();
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #LireportConfiguration').removeClass('disableAll');
                    Clinical_AddReportHeaderTemplate.loadHeaderConfiuration();
                } else {
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #headingTitle').text('Add Report Header Template');
                    Clinical_AddReportHeaderTemplate.isEntitySelected("");
                }
            });
        }

        Clinical_AddReportHeaderTemplate.ValidateReportHeader();

    },

    //Binding Validation Functionk
    ValidateReportHeader: function () {
        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #frmAddReportHeader').bootstrapValidator('destroy');
        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #frmAddReportHeader')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  ReportHeaderName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }


              }
          })
       .on('success.form.bv', function (e) {
           Clinical_AddReportHeaderTemplate.saveReportHeader();
       });
    },

    fillReportHeader: function () {
        Clinical_AddReportHeaderTemplate.fillReportHeader_DbCall(Clinical_AddReportHeaderTemplate.params.ReportHeaderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var self = $("#" + Clinical_AddReportHeaderTemplate.params.PanelID);
                var reportHeaderInfo = JSON.parse(response.reportHeaderList_JSON)[0];
                utility.bindMyJSONByName(true, reportHeaderInfo, false, self);

                if (reportHeaderInfo['IsActive'] == true)
                    $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + ' #Active').attr("checked", true);
                else
                    $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + ' #Active').attr("checked", false);


                Clinical_AddReportHeaderTemplate.isEntitySelected(reportHeaderInfo['EntityIds']).done(function () {
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
                    // Set the value                
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #ddlSpecialty").val(reportHeaderInfo['SpecialtyIds'].split(','));
                    // Then refresh
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect("refresh");

                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
                    // Set the value
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #ddlProvider").val(reportHeaderInfo['ProviderIds'].split(','));
                    // Then refresh
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').multiselect("refresh");

                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlFacility').multiselect('clearSelection', false);
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlFacility').multiselect('updateButtonText');
                    // Set the value
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #ddlFacility").val(reportHeaderInfo['FacilityIds'].split(','));
                    // Then refresh
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlFacility').multiselect("refresh");
                });
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlEntity').multiselect('clearSelection', false);
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlEntity').multiselect('updateButtonText');
                // Set the value
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #ddlEntity").val(reportHeaderInfo['EntityIds'].split(','));
                // Then refresh
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlEntity').multiselect("refresh");
                var PracticeTags = 'Practice Name: <Practice Name><br/>Practice Address: <Practice Address><br/><Practice City>, <State>, <Zip><br/>Practice Ph: <Practice Phone>';
                //Start 14-11-2016 Edit By Humaira Yousaf for EMR-1615
		var ProviderTags = 'Provider Name: <Provider Name><br/>Specialty: <Specialty><br/><Visit Date>,<Visit Time><br/><Visit Reason>';
		//End 14-11-2016 Edit By Humaira Yousaf for EMR-1615
                var PatientTags = '<Patient Name><br/><Age> Years<br/>DOB: <DOB><br/>Acc. #: <Account Number><br/>Ph: <Phone number><br/><Email Address><br/><Address>';
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #txtareaPractice").val(reportHeaderInfo['PracticeTags'].replace(/<br *\/?>/gi, '\n'));
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #txtareaPatient").val(reportHeaderInfo['PatientTags'].replace(/<br *\/?>/gi, '\n'));
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + " #txtareaProvider").val(reportHeaderInfo['ProviderTags'].replace(/<br *\/?>/gi, '\n'));
                var PracticeDefault = (PracticeTags == reportHeaderInfo['PracticeTags']);
                var ProviderDefault = (ProviderTags == reportHeaderInfo['ProviderTags']);
                var PatientDefault = (PatientTags == reportHeaderInfo['PatientTags']);
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags #PatientDefault').prop('checked', PatientDefault);
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags #PracticeDefault').prop('checked', PracticeDefault);
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags #ProviderDefault').prop('checked', ProviderDefault);
                Clinical_AddReportHeaderTemplate.loadDefaultPracticeTags();
                Clinical_AddReportHeaderTemplate.loadDefaultProviderTags();
                Clinical_AddReportHeaderTemplate.loadDefaultPatientTags();
                if (reportHeaderInfo.HeaderLogo != null && reportHeaderInfo.HeaderLogo != '') {
                    Clinical_AddReportHeaderTemplate.setImageInfo(reportHeaderInfo.HeaderLogo, reportHeaderInfo.HeaderLogoName, reportHeaderInfo.HeaderLogoUpldDate);
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    fillReportHeader_DbCall: function (ReportHeaderId) {
        var objData = {};

        objData["ReportHeaderId"] = ReportHeaderId;
        objData["commandType"] = "fill_report_header";


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },
    saveReportHeader: function () {
        var self = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();
        if (Clinical_AddReportHeaderTemplate.params.mode == "Add") {
            Clinical_AddReportHeaderTemplate.saveReportHeader_DbCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_AddReportHeaderTemplate.params.ReportHeaderId = response.ReportHeaderId;
                    Clinical_AddReportHeaderTemplate.params.mode = "Edit";
                    Clinical_AddReportHeaderTemplate.ValidateReportHeader();
                    Clinical_ReportHeader.ReportHeaderSearch();
                    Clinical_AddReportHeaderTemplate.toggleRprtHeaderTab('rprtconfig');
                    //Clinical_AddReportHeaderTemplate.UnLoadTab();
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #LireportConfiguration').removeClass('disableAll');
                    Clinical_AddReportHeaderTemplate.loadHeaderConfiuration();

                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        else if (Clinical_AddReportHeaderTemplate.params.mode == "Edit") {

            var myJSON = self.getMyJSONByName();
            Clinical_AddReportHeaderTemplate.saveReportHeader_DbCall(myJSON, Clinical_AddReportHeaderTemplate.params.ReportHeaderId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ReportHeader.ReportHeaderSearch();
                    Clinical_AddReportHeaderTemplate.UnLoadTab();
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    saveReportHeader_DbCall: function (ReportHeaderData, ReportHeaderId) {
        var objData = JSON.parse(ReportHeaderData);
        if (ReportHeaderId == null) {
            objData["commandType"] = "SAVE_REPORT_HEADER";
        } else {
            objData["ReportHeaderId"] = ReportHeaderId;
            objData["commandType"] = "UPDATE_REPORT_HEADER";
        }
        objData["PracticeTags"] = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #frmAddReportHeader #txtareaPractice').val().replace(/\n/g, '<br/>');
        objData["ProviderTags"] = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #frmAddReportHeader #txtareaProvider').val().replace(/\n/g, '<br/>');
        objData["PatientTags"] = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #frmAddReportHeader #txtareaPatient').val().replace(/\n/g, '<br/>');
        objData["SpecialtyIds_RefValue"] = null;
        objData["SpecialtyIds_text"] = null;
        objData["ProviderIds_text"] = null;
        objData["ProviderIds_RefValue"] = null;
        objData["FacilityIds_text"] = null;
        objData["FacilityIds_RefValue"] = null;
        objData["PracticeTags_text"] = null;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },
    TemplateTagsLookupsLoad: function () {

        Clinical_AddReportHeaderTemplate.FillReportHeaderTagNames_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlPatientTags').multiselect('destroy');
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlProviderTags').multiselect('destroy');
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlPracticeTags').multiselect('destroy');

                var PatientList = JSON.parse(response.PatientList_JSON);
                var PracticeList = JSON.parse(response.PracticeList_JSON);
                var ProviderList = JSON.parse(response.ProviderList_JSON);
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlPracticeTags').empty();
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlPatientTags').empty();
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProviderTags').empty();
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlPracticeTags').append(
                        $('<option/>', {
                            value: '',
                            html: ' - Select Tag Name - '
                        })
                    );
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlPatientTags').append(
                        $('<option/>', {
                            value: '',
                            html: ' - Select Tag Name - '
                        })
                    );
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProviderTags').append(
                        $('<option/>', {
                            value: '',
                            html: ' - Select Tag Name - '
                        })
                    );
                $.each(PracticeList, function (i, item) {
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlPracticeTags').append(
                        $('<option/>', {
                            value: item.NotesTagNameId,
                            html: item.ShortName
                        })
                    );
                });
                $.each(PatientList, function (i, item) {
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlPatientTags').append(
                        $('<option/>', {
                            value: item.NotesTagNameId,
                            html: item.ShortName
                        })
                    );
                });
                $.each(ProviderList, function (i, item) {
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProviderTags').append(
                        $('<option/>', {
                            value: item.NotesTagNameId,
                            html: item.ShortName
                        })
                    );
                });
            }

        }).then(function () {

            $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlPracticeTags').on('change', function () {
                var selectedText = $(this).find("option:selected").text();
                var selectedValue = $(this).val();
                Clinical_AddReportHeaderTemplate.insertTagName('#PracticeTags textarea', selectedText, true);
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlPracticeTags').val('');
            });

            $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlPatientTags').on('change', function () {
                var selectedText = $(this).find("option:selected").text();
                var selectedValue = $(this).val();
                Clinical_AddReportHeaderTemplate.insertTagName('#PatientTags textarea', selectedText, true);
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlPatientTags').val('');
            });

            $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlProviderTags').on('change', function () {
                var selectedText = $(this).find("option:selected").text();
                var selectedValue = $(this).val();
                Clinical_AddReportHeaderTemplate.insertTagName('#ProviderTags textarea', selectedText, true);
                $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlProviderTags').val('');

            });

            Clinical_AddReportHeaderTemplate.loadDefaultPracticeTags();
            Clinical_AddReportHeaderTemplate.loadDefaultProviderTags();
            Clinical_AddReportHeaderTemplate.loadDefaultPatientTags();
        });
    },

    insertTagName: function (cntrl, text, checked) {
        var TagText = ' <' + text + '> ';
        var value = $(cntrl).val();
        if (checked) {
            value = value + TagText;
        }
        else {
            value = value.split(TagText).join('');
            value = value.split('<' + text + '>').join('');
        }
        $(cntrl).val(value);
    },

    FillReportHeaderTagNames_DBCall: function () {
        var objData = {};
        objData["commandType"] = "get_report_header_tagname";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },
    loadDefaultPracticeTags: function () {
        if ($('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags #PracticeDefault').is(':checked')) {
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags textarea').val('Practice Name: <Practice Name>\nPractice Address: <Practice Address>\n<Practice City>, <State>, <Zip>\nPractice Ph: <Practice Phone>');
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags #ddlPracticeTags').prop('disabled', true);
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags textarea').prop('disabled', true);
        } else {
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags #ddlPracticeTags').prop('disabled', false);
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PracticeTags textarea').prop('disabled', false);
        }
    },
    loadDefaultProviderTags: function () {
        if ($('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags #ProviderDefault').is(':checked')) {
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags #ddlProviderTags').prop('disabled', true);
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags textarea').prop('disabled', true);
	    //Start 14-11-2016 Edit By Humaira Yousaf for EMR-1615
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags textarea').val('Provider Name: <Provider Name>\nSpecialty: <Specialty>\n<Visit Date>,<Visit Time>\n<Visit Reason>');
	    //End 14-11-2016 Edit By Humaira Yousaf for EMR-1615
        } else {
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags #ddlProviderTags').prop('disabled', false);
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ProviderTags textarea').prop('disabled', false);
        }
    },
    loadDefaultPatientTags: function () {
        if ($('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags #PatientDefault').is(':checked')) {
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags #ddlPatientTags').prop('disabled', true);
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags textarea').val('<Patient Name>\n<Age> Years\nDOB: <DOB>\nAcc. #: <Account Number>\nPh: <Phone number>\n<Email Address>\n<Address>');
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags textarea').prop('disabled', true);
        } else {
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags #ddlPatientTags').prop('disabled', false);
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #PatientTags textarea').prop('disabled', false);
        }
    },

    toggleRprtHeaderTab: function (tabShowId) {
        if (tabShowId == 'rprtconfig') {
            $('#tabreportConfiguration').trigger('click');
        } else if (tabShowId == 'hdrinfo') {
            $('#tabheaderInfo').trigger('click');
        }
    },
    UnLoad: function (caller) {
        var parentContolId = "adminTabReportHeader";
        var controlId = "Clinical_AddReportHeaderTemplate";
        UnloadActionPan(parentContolId, controlId);


    },


    //Author: Talha Tanweer
    //Date  : 05/08/2016
    BufferFile: function (input) {


        if (input.files) {
            if (utility.ValidateFileSize(input.files) > 2) {
                utility.DisplayMessages("Maximum 2MB  is allowed", 4);
                $(Clinical_AddReportHeaderTemplate.params.PanelID + ' #Upload_Image_file').val('');
                return false;
            }
            else {
                var fileType = input.files[0].type.toLowerCase();
                //var fileType = input.files[0].type.toLowerCase();
                if (fileType == "image/jpeg" || fileType == "image/png" || fileType == "image/jpg" || fileType == "image/gif" || fileType == "image/bmp") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        var fileName = input.files[0].name.split(".")[0];
                        var d = new Date();
                        var UploadedOnDate = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear();
                        Clinical_AddReportHeaderTemplate.setImageInfo(e.target.result, fileName, UploadedOnDate);

                    }

                    reader.readAsDataURL(input.files[0]);

                }
                else {
                    utility.DisplayMessages("Only JPG, PNG, BMP, JPEG, GIF files are allowed.", 2);
                    $(Clinical_AddReportHeaderTemplate.params.PanelID + ' #Upload_Image_file').val(null);
                    $(Clinical_AddReportHeaderTemplate.params.PanelID + ' #imgUploadImage').attr('src', "");
                    $(Clinical_AddReportHeaderTemplate.params.PanelID + ' #imgUploadImage').hide();
                }
            }
        }
    },

    //Author: Talha Tanweer
    //Date  : 05/08/201
    BrowsebtnClick: function (control) {
        this.value = null;
    },

    setImageInfo: function (imageSrc, ImageName, UploadedDate) {
        $('#pnlAddReportHeaderTemplate #imgUploadImage').attr('src', imageSrc);
        $('#pnlAddReportHeaderTemplate #ImguploadContainer_AddHeaderTemplate').show();
        $('#pnlAddReportHeaderTemplate #imgUploadImage').show();

        if (UploadedDate !=null && UploadedDate !=undefined && UploadedDate !='' ) {
          UploadedDate = UploadedDate.split(' ')[0];
        }
      

        $('#pnlAddReportHeaderTemplate #HeaderLogoName').val(ImageName);
        $('#pnlAddReportHeaderTemplate #lblHeaderLogoTextReportHeader').html('<strong>' + ImageName + "</strong> uploaded on " + UploadedDate);
        $('#pnlAddReportHeaderTemplate #spnUploadFilebtn #spnUploadFilebtntxt').text("Replace");
    },
    RemoveImage: function () {
        utility.myConfirm('Are you sure you want to delete the header logo?', function () {
            $('#pnlAddReportHeaderTemplate #imgUploadImage').attr('src', "");
            $(' #pnlAddReportHeaderTemplate #Upload_Image_file').val('');

                 $(' #pnlAddReportHeaderTemplate #imgUploadImage').val('');

            $('#pnlAddReportHeaderTemplate #ImguploadContainer_AddHeaderTemplate').hide();
            $('#pnlAddReportHeaderTemplate #imgUploadImage').hide();
            $('#pnlAddReportHeaderTemplate #lblHeaderLogoTextReportHeader').text("");
            $('#pnlAddReportHeaderTemplate #spnUploadFilebtn #spnUploadFilebtntxt').text("Browse");
        }, '3', "Confirm Delete");
    },

    PreviewReportHeader: function () {


        var params = [];
        params["PracticeTags"] = $("#pnlAddReportHeaderTemplate #divCHIreportHeader #txtareaPractice").val().replace(/\</g, '< ').replace(/\>/g, '> ').replace(/\n/g, '<br/>');
        params["PatientTags"] = $("#pnlAddReportHeaderTemplate #divCHIreportHeader #txtareaPatient").val().replace(/\</g, '< ').replace(/\>/g, '> ').replace(/\n/g, '<br/>');
        params["ProviderTags"] = $("#pnlAddReportHeaderTemplate #divCHIreportHeader #txtareaProvider").val().replace(/\</g, '< ').replace(/\>/g, '> ').replace(/\n/g, '<br/>');
        params["FooterGeneratedBy"] = $("#pnlAddReportHeaderTemplate  #txtFooterText").val();
        params["HeaderLogo"] = $('#pnlAddReportHeaderTemplate #imgUploadImage').attr('src');


        params["ParentCtrl"] = "Clinical_AddReportHeaderTemplate";

        LoadActionPan('Clinical_PreviewReportHeaderTemplate', params);

        //params["ReportHeaderId"] = ReportHeaderId;
        //params["mode"] = "Edit";
        //params["FromAdmin"] = 0;

    },

    UnLoadTab: function () {
        if (Clinical_AddReportHeaderTemplate.params["FromAdmin"] == "0") {
            if (Clinical_AddReportHeaderTemplate.params != null && Clinical_AddReportHeaderTemplate.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_AddReportHeaderTemplate.params.ParentCtrl, 'Clinical_AddReportHeaderTemplate');
            }
            else
                UnloadActionPan(null, 'Clinical_AddReportHeaderTemplate');
        }
        else {
            RemoveAdminTab();
        }
    },

    //Author: Talha Tanweer
    //Date :  09-08-2016

    //Author: Talha Tanweer
    //Date :  09-08-2016
    ReportConfigurationGridLoad: function (response) {
        $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #pnlReportHeader_Configruation #dgvReportConfiguration").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #pnlReportHeader_Configruation #dgvReportConfiguration tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.ClinicalReportConfigurationCount > 0) {
            var ReportConfigurationLoadJSONData = JSON.parse(response.ClinicalReportConfiguration_JSON); //Parsing array to JSON
            $.each(ReportConfigurationLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                //  $row.attr("onclick", "ClinicalReportConfigurationDetail.ReportConfigurationEdit('" + item.ReportConfigurationId + "',event);");
                $row.attr("id", "gvReportConfiguration_row" + item.ReportConfigurationId);
                $row.attr("ReportConfigurationId", item.ReportConfigurationId);


                if (item.IsActive == "True") {
                    isactive = 1
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 0
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var ActiveInactiveMethod = "Clinical_AddReportHeaderTemplate.ActiveInactiveLab(" + item.LabId.trim() + "," + isactive + ",event);";

                $row.append('<td style="display:none;">' + item.LabId + '</td>' +

                 '<td>' + item.Name + '</td><td>' + item.Type + '<td>' + item.ModifiedOn + "<br/>" + item.ModifiedBy);
                $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #pnlReportHeader_Configruation #dgvReportConfiguration tbody").last().append($row);
            });
        }
        else {
            //Initialize data table with no record added
            $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + ' #pnlReportHeader_Configruation #dgvReportConfiguration').DataTable({
                "language": {
                    "emptyTable": "No Report Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_AddReportHeaderTemplate.params.PanelID + ' #pnlReportHeader_Configruation #dgvReportConfiguration'))
            ;
        else {
            $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #pnlReportHeader_Configruation #dgvReportConfiguration").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown          
        }
    },







    // ---------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------- Start Provider and Speciality Dropdown ----------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------
    //Specialties 
    loadFacilityDropdown: function (EntityId) {
        var self = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #DivFacility');
        $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlFacility').empty();
        var data = "EntityId=" + EntityId;
        self.loadDropDowns(true, data).done(function () {
            $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlFacility').multiselect('destroy')
            $('#' + Clinical_AddReportHeaderTemplate.params["PanelID"] + ' #ddlFacility').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
        });
    },

    isEntitySelected: function (EntityId) {
        var objDeffered = $.Deferred();
        if (EntityId == null && EntityId != "") {
            var EntityIds = $('#ddlEntity').val();
            EntityIds = $.isArray(EntityIds) ? EntityIds.join(',') : EntityIds;
        } else {
            EntityIds = EntityId;
        }
        EntityIds = EntityIds == null ? "" : EntityIds;
        Clinical_AddReportHeaderTemplate.loadFacilityDropdown(EntityIds);
        $.when(Clinical_AddReportHeaderTemplate.loadEntitySpecialty(EntityIds)).then(function () {
            $.when(Clinical_AddReportHeaderTemplate.loadEntityProvider(EntityIds)).then(function () {
                objDeffered.resolve();
            });
        });
        return objDeffered;
    },
    //This function will load entity based specialty
    loadEntitySpecialty: function (entityID) {

        // Loads Spacialties Based on entityId
        var objDeffered = $.Deferred();
        if (entityID != null) {
            entityID = (entityID == "" ? -1 : entityID);
            var data = "EntityId=" + (entityID == "" ? -1 : entityID);
            MDVisionService.lookups('GetSpecialty', true, data).done(function (result) {
                result = JSON.parse(result["GetSpecialty"]);
                var options = result;
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').empty();
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {
                        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name + "(" + item.RefName + ")",
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                    }
                });

                //Assign server side spacialties to the specialityCheckedIds array
                if (Clinical_AddReportHeaderTemplate.SpecialtyIds != '') {
                    var Specialties = Clinical_AddReportHeaderTemplate.SpecialtyIds.split(",");
                    Clinical_AddReportHeaderTemplate.specialityCheckedIds = Specialties;
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').val(Specialties);
                }
            }).then(function () {
                Clinical_AddReportHeaderTemplate.IntializeMultiSelectDropDownSpecialties();
                objDeffered.resolve();
            });
        }
        return objDeffered;
    },

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + (entityId == "" ? -1 : entityId);
        if (entityId != null) {
            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlHiddenProvider');

                //Empty both the providers ddls.
                $providerDdl.empty();
                $providerHiddenDdl.empty();

                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {

                        // User will see these providers in multiSelect dropdownlist
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                        // Populate hidden ddl provider
                        //A Hack to load all the providers in hidden dropdownlist
                        $providerHiddenDdl.append(
                             $('<option/>', {
                                 value: item.Value,
                                 html: item.Name,
                                 refname: item.RefName,
                                 refvalue: item.RefValue

                             })
                        );
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Clinical_AddReportHeaderTemplate.ProviderIds != '') {
                    var Providers = Clinical_AddReportHeaderTemplate.ProviderIds.split(",");
                    Clinical_AddReportHeaderTemplate.providerCheckedIds = Providers;
                    $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #divSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Clinical_AddReportHeaderTemplate.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });

        }

        return objDeffered;
    },


    //This function will save privder ids and will check speciality on provider selection
    checkSpecialtiesByProviderId: function (option, checked, select) {
        //provider context
        var providerContext = '#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #divProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_AddReportHeaderTemplate.providerCheckedIds = [];
            Clinical_AddReportHeaderTemplate.ProviderIds = '';
        }
            //push all provider checked items
        else if (!isAllProviderSelected) {
            // provider value
            var providerValue = $(option).val();
            // add to provider array if checked
            if (checked) {
                if ($.inArray(Clinical_AddReportHeaderTemplate.providerCheckedIds, providerValue) == -1) {
                    Clinical_AddReportHeaderTemplate.providerCheckedIds.push(providerValue);
                }
            }
                //delete from provider array if not checked
            else {
                Clinical_AddReportHeaderTemplate.providerCheckedIds = Clinical_AddReportHeaderTemplate.removeFromArray(Clinical_AddReportHeaderTemplate.providerCheckedIds, providerValue);
            }
        }
        else {
            Clinical_AddReportHeaderTemplate.providerCheckedIds = [];
            var values = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').val();
            if ($.isArray(values)) {
                Clinical_AddReportHeaderTemplate.providerCheckedIds = values
            } else if (values != null && values != '') {
                Clinical_AddReportHeaderTemplate.providerCheckedIds.push(values);
            }
        }
        Clinical_AddReportHeaderTemplate.setSpacialtiesByselectedProviderIds();
    },

    //This function will set specialty Ids in specailChekedIds array
    setSpacialtiesByselectedProviderIds: function () {
        // $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect('select', []);
        $.when($.each(Clinical_AddReportHeaderTemplate.providerCheckedIds, function (index, item) {

            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider option:checked').each(function () {
                if ($(this).val() != '' && $(this).val() == item) {
                    if (jQuery.inArray($(this).attr('refname'), Clinical_AddReportHeaderTemplate.specialityCheckedIds) == -1) {
                        Clinical_AddReportHeaderTemplate.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        })).then(function () {
            if (Clinical_AddReportHeaderTemplate.specialityCheckedIds != null && Clinical_AddReportHeaderTemplate.specialityCheckedIds.length > 0) {
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').val(Clinical_AddReportHeaderTemplate.specialityCheckedIds);
                $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
            }

        });
    },
    // This function will initialize provider multiselect ddl
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_AddReportHeaderTemplate.checkSpecialtiesByProviderId(option, checked, select);
            },
        });
    },

    //which intialize all multi select dropdowns
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_AddReportHeaderTemplate.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_AddReportHeaderTemplate.SpecialtyIds != '') {
                    var spacialties = Clinical_AddReportHeaderTemplate.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_AddReportHeaderTemplate.specialityCheckedIds = Clinical_AddReportHeaderTemplate.removeFromArray(Clinical_AddReportHeaderTemplate.specialityCheckedIds, item);
                            Clinical_AddReportHeaderTemplate.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_AddReportHeaderTemplate.setSpacialtiesByselectedProviderIds();
            },
        });
    },

    //Start//03/16/2016//M Ahmad Imran//Implimented "checkProvidersBySpecialityIds" This function will save spacialty ids and will show privders on spacialty selection
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #divSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_AddReportHeaderTemplate.specialityCheckedIds = [];
            Clinical_AddReportHeaderTemplate.providerCheckedIds = [];
            Clinical_AddReportHeaderTemplate.ProviderIds = '';
            Clinical_AddReportHeaderTemplate.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    if ($.inArray(Clinical_AddReportHeaderTemplate.specialityCheckedIds, spacialityId) == -1) {
                        Clinical_AddReportHeaderTemplate.specialityCheckedIds.push(spacialityId);
                    }
                }
                else {
                    Clinical_AddReportHeaderTemplate.specialityCheckedIds = Clinical_AddReportHeaderTemplate.removeFromArray(Clinical_AddReportHeaderTemplate.specialityCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_AddReportHeaderTemplate.specialityCheckedIds = [];
                var values = $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlSpecialty').val();
                if ($.isArray(values)) {
                    Clinical_AddReportHeaderTemplate.specialityCheckedIds = values
                } else if (values != null && values != '') {
                    Clinical_AddReportHeaderTemplate.specialityCheckedIds.push(values);
                }
            }
        }
        $.when(Clinical_AddReportHeaderTemplate.filterProvidersBySpecialtyIds()).then(function () {
            if (Clinical_AddReportHeaderTemplate.ProviderIds != '') {
                var Providers = Clinical_AddReportHeaderTemplate.ProviderIds.split(",");
                if (Providers != '' && typeof Providers != 'undefined') {
                    $.each(Providers, function (index, item) {
                        Clinical_AddReportHeaderTemplate.providerCheckedIds = Clinical_AddReportHeaderTemplate.removeFromArray(Clinical_AddReportHeaderTemplate.providerCheckedIds, item);
                        if ($.inArray(Clinical_AddReportHeaderTemplate.providerCheckedIds, item) == -1) {
                            Clinical_AddReportHeaderTemplate.providerCheckedIds.push(item);
                        }
                    });
                }
            }
            $('#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider').val(Clinical_AddReportHeaderTemplate.providerCheckedIds);
            Clinical_AddReportHeaderTemplate.IntializeMultiSelectDropDownProviders();
        });
    },

    //This function will remove item from the "array and item" provided as input args
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    //This function will save spacialty ids and will show privders on spacialty selection
    filterProvidersBySpecialtyIds: function () {
        var providerHiddenContext = '#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlHiddenProvider';
        var providerContext = '#' + Clinical_AddReportHeaderTemplate.params.PanelID + ' #ddlProvider';
        $(providerContext).empty();
        if (Clinical_AddReportHeaderTemplate.specialityCheckedIds.length > 0) {
            $.each(Clinical_AddReportHeaderTemplate.specialityCheckedIds, function (index, specialtyId) {
                $(providerHiddenContext).find('option').each(function (index, option) {
                    if ($(option).attr('refname') == specialtyId) {
                        $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
                    }
                });
            });
        }
        else {
            $(providerHiddenContext).find('option').each(function (index, option) {
                $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
            });
        }
    },


    // ---------------------------------------------------------------------------------------------------------------------------
    // ------------------------------------------- end Provider and Speciality Dropdown ----------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------


    //Function Name: loadHeaderConfiuration 
    //Author: Humaira Yousaf
    //Date: 10-08-2016
    //Description: loads header settings
    loadHeaderConfiuration: function () {

        Clinical_AddReportHeaderTemplate.loadHeaderConfiuration_Dbcall(Clinical_AddReportHeaderTemplate.params.ReportHeaderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #dgvReportConfiguration tbody").empty();

                if (response.HeaderConfigurationCount > 0) {
                    var ReportConfigurationLoadJSONData = JSON.parse(response.HeaderConfigurationFill_JSON);
                    $.each(ReportConfigurationLoadJSONData, function (i, item) {
                        var $row = $('<tr/>');
                        $row.attr("id", "gvReportConfiguration_row" + item.RptHeaderSettingId);
                        $row.attr("HeaderSettingId", item.RptHeaderSettingId);


                        if (item.IsActive == "True") {
                            isactive = 1
                            activeTitle = "Active Record";
                            tglclass = "fa fa-toggle-on green";
                        }
                        else {
                            isactive = 0
                            activeTitle = "Inactive Record";
                            tglclass = "fa fa-toggle-on red";
                        }
                        var checked = '';
                        if (isactive == "0" || isactive == 0) {
                            isactive = "0";
                        } else if (isactive == null) {
                            isactive = "1";
                            checked = 'checked="checked"';
                        } else {
                            isactive = "1";
                            checked = 'checked="checked"';
                        }
                        var HtmlOfSwitch = '<div class="btnWidgetSwitch switch switch-xs switch-success">' +
                               '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_AddReportHeaderTemplate.headerSettingsActiveInactive(' + item.RptHeaderSettingId + ', ' + isactive + ', ' + item.RptHeaderConfigurationId + ',event);">' +
                                '</div>';
                        $row.append('<td style="display:none;">' + item.RptHeaderSettingId + '</td>' +
                         '<td style="display:none;">' + item.RptHeaderConfigurationId + '</td>' +
                         '<td>' + item.Name + '</td>' +
                         '<td>' + HtmlOfSwitch + '</td>');
                        $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #dgvReportConfiguration tbody").last().append($row);

                    });
                    EMRUtility.SwicthWidgetInializatoin();
                }
                else {
                    var $row = $('<tr/>');
                    $row.append('No Header Settings Found');
                    $("#" + Clinical_AddReportHeaderTemplate.params.PanelID + " #dgvReportConfiguration tbody").last().append($row);
                }
            }
        });
    },
    //Function Name: loadHeaderConfiuration_Dbcall 
    //Author: Humaira Yousaf
    //Date: 10-08-2016
    //Description: db call to load header settings 
    loadHeaderConfiuration_Dbcall: function (reportHeaderId) {
        var objData = {};
        objData["ReportHeaderId"] = Clinical_AddReportHeaderTemplate.params.ReportHeaderId;
        objData["commandType"] = "loadHeaderConfiuration";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },

    //Function Name: headerSettingsActiveInactive 
    //Author: Humaira Yousaf
    //Date: 10-08-2016
    //Description: updates header settings 
    headerSettingsActiveInactive: function (headerSettingId, IsActive, headerConfigurationId, event) {
        //utility.myConfirm('3', function () {
        //    if (headerSettingId == "" || headerSettingId == "undefined") {
        //        }
        //        else {
        Clinical_AddReportHeaderTemplate.headerSettingsActiveInactive_Dbcall(headerSettingId, IsActive, headerConfigurationId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_AddReportHeaderTemplate.loadHeaderConfiuration();
                utility.DisplayMessages(response.message, 1);
            }
            else {
                utility.DisplayMessages(response.message, 3);
            }
        });
        //    }
        //}, function () { },
        //                '3'
        //            );
    },

    //Function Name: headerSettingsActiveInactive 
    //Author: Humaira Yousaf
    //Date: 10-08-2016
    //Description: db call to update header settings 
    headerSettingsActiveInactive_Dbcall: function (headerSettingId, IsActive, headerConfigurationId) {
        var objData = {};

        objData["ReportHeaderId"] = Clinical_AddReportHeaderTemplate.params.ReportHeaderId;
        objData["RptHeaderSettingId"] = headerSettingId;
        objData["RptHeaderConfigurationId"] = headerConfigurationId;
        objData["IsActive"] = IsActive == 0 ? 1 : 0;

        objData["commandType"] = "saveheadersettings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "ReportHeader");
    },
}