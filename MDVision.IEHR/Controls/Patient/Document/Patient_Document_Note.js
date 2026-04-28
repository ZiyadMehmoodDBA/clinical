Patient_Document_Note = {
  params:[],
  Load:function(params){
      Patient_Document_Note.params = params;
      Patient_Document_Note.params.PanelID = "pnlDocumentNote";
      Patient_Document_Note.LoadDocumentNotes();
  },
  LoadDocumentNotes:function(){
      Patient_Document_Note.DocumentNotes(Patient_Document_Note.params.PatDocId).done(function (response) {
          Patient_Document_Note.NotesSignedGridLoad(response);
      });
  },
  NotesSignedGridLoad: function (response, PageNo, rpp) {
      var IsNoteUnSign = false;
      Clinical_Notes.params.HasUnSignPermission = false;
      $("#"+Patient_Document_Note.params["PanelID"]+ " #dgvClinicalSignedNotes").dataTable().fnDestroy();
      $("#"+Patient_Document_Note.params["PanelID"]+ " #dgvClinicalSignedNotes tbody").find("tr").remove();
              if (response.ClinicalNotesCount > 0) {
                  $.each(response.NotesLoad_JSON, function (i, item) {
                      if (item.NoteStatus == "Signed") {
                          var $row = $('<tr/>');
                          $row.attr("id", "gvSignedNotes_row" + item.NotesId);
                          $row.attr("NotesId", item.NotesId);
                          $row.attr("VisitDate", utility.RemoveTimeFromDate(null, item.VisitDate));
                          $row.attr("VisitTime", item.VisitTime);
                          $row.attr("NoteType", item.NoteTempType);
                          $row.attr("CC", item.ChiefComplaint);
                          $row.attr("Status", item.EntityId);
                          $row.attr("Provider", item.ProviderName);
                          $row.attr("SignedBy", item.SignedBy);
                          $row.attr("Facility", item.FacilityName);
                          $row.attr("Room", item.RoomNo);
                          $row.attr("Comments", item.Comments);
                          $row.attr("Active", item.IsActive);
                          $row.attr("IsUnSigned", item.UnSignedStatus);
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
                          var isVisible = 'style="display:none;';
                          var Isdisabled = "disabled =true";
                          if (item.NoteStatus == "Draft" || item.NoteStatus == "") {
                              Isdisabled = "";
                          }
                          var NotesPreview = "Clinical_Notes.NotesPreview('" + item.NotesId + "'," + "'Patient_Document_Note'" + ",'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : "'" + item.RefProviderId + "'") + ",'" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "'," + (item.IsPhoneEncounter == "0" ? false : true) + ",'" + item.BillingStatus + "',"+true+");";
                          $row.attr("onclick", NotesPreview);
                          $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                          $("#" + Patient_Document_Note.params["PanelID"] + " #dgvClinicalSignedNotes tbody").last().append($row);
                      }
                  });

                  var signedNotesRows = $("#" + Patient_Document_Note.params["PanelID"] + " #dgvClinicalSignedNotes tbody").find("tr");

                  if (signedNotesRows.length < 1) {
                      $("#" + Patient_Document_Note.params["PanelID"] + " #dgvClinicalSignedNotes").DataTable({
                          "language": {
                              "emptyTable": "No Signed Notes Found"
                          }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                      });  
                  }
              }
              var IsDataTable = $.fn.dataTable.isDataTable("#"+Patient_Document_Note.params["PanelID"]+' #dgvClinicalSignedNotes');

              if (!IsDataTable) {
                  // to remove records per page dropdown
                  $("#" + Patient_Document_Note.params["PanelID"] + " #dgvClinicalSignedNotes").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
              }      
    
  },
  DocumentNotes: function (PatDocID) {
      var data = "PatDocId=" + PatDocID;
      // serach parameter , class name, command name of class
      return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DOCUMENT_NOTES_LIST");
  },
  UnLoad: function () {
      UnloadActionPan(Patient_Document_Note.params["ParentCtrl"]);
  }
}