Patient_DocumentTag = {
    params: [],
    bIsFirstLoad: true,
    UnloadParent: '',

    Load: function (params) {
        Patient_DocumentTag.params = params;
        Patient_DocumentTag.Documents_TagSearch();
        Patient_DocumentTag.ValidateDocumentTags();
    },

    DeleteDocumentTags: function (TagID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
            if (strMessage == "") {
                //utility.myConfirm('The Tag might be associated with any document(s). Do you still want to delete this record?', function () {

                    var selectedValue = TagID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_DocumentTag.DocumentTagDelete(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#pnlPatientDocumentsTag #dgvPatientDocumentsTag').DataTable();
                                table1.row('.active').remove().draw(false);
                                Patient_DocumentTag.Documents_TagSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                //}, function () { },
                //    'Confirm Delete'
                //);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        //});

    },

    ActiveInactivePatientDocumentTags: function (TagID,Name, event, obj) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }   
        if (strMessage == "") {
            utility.myConfirm('Do you want to Active/Inactive this record?', function () {
                var DocIsActive;
                if ($(obj).attr('title') == "Active Record")
                {
                    DocIsActive = false
                }
                else { DocIsActive = true }

                Patient_DocumentTag.ActiveInactivePatientTags(TagID, Name, DocIsActive).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#pnlPatientDocumentsTag #dgvPatientDocumentsTag').DataTable();
                            table1.row('.active').remove().draw(false);
                            Patient_DocumentTag.Documents_TagSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                
            }, function () { },
                'Confirm Active/Inactive'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});

    },
    Documents_TagSearch: function (Id, PageNumber, ResultPerPage) {
        $("#pnlPatientDocumentsTag #dgvPatientDocumentsTag").dataTable().fnDestroy();
        $("#pnlPatientDocumentsTag #dgvPatientDocumentsTag tbody").find("tr").remove();
        Patient_DocumentTag.PatientDocumentsTagSearch(Id, PageNumber, ResultPerPage).done(function (response) {
                    if (response.status != false) {
                        Patient_DocumentTag.Documents_TagGridLoad(response);
                                var TableControl = $("pnlPatientDocument #dgvPatientDocumentsTag");
                                var PagingPanelControlID = Patient_DocumentTag.params["PanelID"] + " #dvgPatientDocumentsTag_Paging";
                                 var ClassControlName = "Patient_DocumentTag";
                                var PagesToDisplay = 15;
                                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                                setTimeout(
                                    CreatePagination(response.TotalTagsCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (Id, PageNumber, ResultPerPage) {
                                        Patient_DocumentTag.Documents_TagSearch(Id, PageNumber, ResultPerPage);
                                    }), 10);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });            
    },

    Documents_TagGridLoad: function (response) {
        
        if (response.TotalTagsCount > 0) {
            $.each(response.DocumentTags, function (i, item) {
                var MethodMode = "";            
                if (Patient_DocumentTag.params["TabID"] == "patTabDocuments" || Patient_DocumentTag.params["TabID"] == "batchTabDocuments") {
                    MethodMode = "Patient_Document.FillTagName('" + item.TagId + "', '" + item.Name + "')";
                }
                else if (Patient_DocumentTag.params["ParentCtrl"] == "Document_Import") {
                    MethodMode = "Document_Import.FillTagName('" + item.TagId + "', '" + item.Name + "')";
                }
                else if (Patient_DocumentTag.params["ParentCtrl"] == "Document_Scan") {
                    MethodMode = "Document_Scan.FillTagName('" + item.TagId + "', '" + item.Name + "')";                 
                }
                else if (Patient_DocumentTag.params["ParentCtrl"] == "Document_Viewer") {
                    MethodMode = "Document_Viewer.FillTagName('" + item.TagId + "', '" + item.Name + "')";                  
                } else {
                    MethodMode = "Patient_Document.FillTagName('" + item.TagId + "', '" + item.Name + "')";
                }
                var $row = $('<tr/>');
                if (item.IsActive) {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }              
                 $row.attr("Id", item.TagId);
                 $row.attr("onclick", MethodMode);
                $row.append('<td><a class="btn  btn-xs" href="#" onclick="Patient_DocumentTag.DeleteDocumentTags(\'' + item.TagId + '\',event);" title="Delete Record"> <i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Patient_DocumentTag.ActiveInactivePatientDocumentTags(' + item.TagId + ', \'' + item.Name + '\',event,this);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;<a class="btn btn-xs" onclick="' + MethodMode + '" href="#" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;</td><td>' + item.Name + '</td>');
                $("#pnlPatientDocumentsTag_Result  #dgvPatientDocumentsTag tbody").last().append($row);
            });
        }
        else {
            utility.ClearFormValidation('#frmDocumentTags', true);
            $('#pnlPatientDocumentsTag_Result #dgvPatientDocumentsTag').DataTable({
                "language": {
                    "emptyTable": "No Document Tag Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }
        
        if ($.fn.dataTable.isDataTable('#pnlPatientDocumentsTag_Result #dgvPatientDocumentsTag'))
            ;
        else
            $("#pnlPatientDocumentsTag_Result #dgvPatientDocumentsTag").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": true, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown


        //$('.dataTables_filter input[type="search"]').css({
        //    'width': '150px', 'display': 'inline-block',
        //    'float': 'right',
        //    'text-align': 'left'
        //});
    },
    PatientDocumentsTagSearch: function (Id, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
       
        var data = "pageNumber=" + PageNumber + "&rowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_TAG_INFORMATION");
    },
    UnLoad: function () {
       
        if (Patient_DocumentTag.UnloadParent == 'ParentUnload') {
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            UnloadActionPan(Patient_DocumentTag.params["ParentCtrl"], 'Patient_DocumentTag', null, parentPanelId);
            Patient_DocumentTag.UnloadParent = "";
        }
        else {
            UnloadActionPan(Patient_DocumentTag.params["ParentCtrl"], "Patient_DocumentTag");
        }       
    }, 
    SaveNewDocumentTags: function () {
        var strMessage = "";
     
        var self = $("#pnlPatientDocumentsTag");
        var myJSON = self.getMyJSON();      
                if (strMessage == "") {
                    Patient_DocumentTag.DocumentNewTagSave(myJSON).done(function (response) {
                        if (response.status != false) {
                            Patient_DocumentTag.Documents_TagSearch();
                            utility.DisplayMessages(response.message, 1);
                            $("#pnlPatientDocumentsTag #txtTagName").val("");
                            $('#frmDocumentTags').bootstrapValidator('revalidateField', 'TagName', true);                        
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
    },
    DocumentNewTagSave: function (patient_DocumentTag, patientID) {
        var data = "Patient_DocumentTag=" + patient_DocumentTag + "&DocID=" + 10;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "ADD_NEW_TAG");
    },
    DocumentTagDelete: function (TagID) {
        var data = "TagID=" + TagID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DELETE_DOCUMENT_TAG");
    },
    ActiveInactivePatientTags: function (TagID, TagName,IsActive) {
        var data = "TagID=" + TagID + "&TagName=" + TagName + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "ACTIVEINACTIVE_DOCUMENT_TAG");
    },  
    ValidateDocumentTags: function () {
        $('#frmDocumentTags')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  TagName: {
                      group: '.col-xs-6',
                      enabled: true,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                 
                
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();         
               Patient_DocumentTag.SaveNewDocumentTags();          
       })      
    },

    GetTagsByName: function (name) {
        var data = "TagName=" + name;

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "TAG_BY_NAME");
    }
    
};

