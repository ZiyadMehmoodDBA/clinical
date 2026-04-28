Design_Letter = {

    bIsFirstLoad: true,
    Load: function (params) {
        if (Design_Letter.bIsFirstLoad) {
            Design_Letter.bIsFirstLoad = false;

            var self = $('#pnlClinicalLetter');
            self.loadDropDowns(true).done(function () {
                if (globalAppdata['IsAdmin'] != "True") {
                    $("#pnlClinicalLetter #divEntity").css("display", "none");
                    $("#pnlClinicalLetter #lstEntityId").val(globalAppdata["SeletedEntityId"]);
                }
            });
            //AppPrivileges.GetFormPrivileges("Section", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            Design_Letter.LetterSearch();
            //    }
            //});
          //  GetLetters("DESIGN_LETTER", "SEARCH_LETTER");
        }
    },

    LetterAdd: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Section", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //if (strMessage == "") {
        var params = [];
            params["LetterId"] = "-1";
            params["mode"] = "Add";
            LoadActionPan('letterDetail', params);
                //LoadActionPan('userDetail', params);
            //}
            //else
                //utility.DisplayMessages(strMessage, 2);
        //});
    },

    LetterSearch: function (letterId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
         if (strMessage == "") {
             if ($("#pnlClinicalLetter #pnlLetter_Result").css("display") == "none") {
                 $("#pnlClinicalLetter #pnlLetter_Result").show();
          }

             var self = $("#pnlClinicalLetter");
            var myJSON = self.getMyJSON();

            Design_Letter.SearchLetter(myJSON, letterId).done(function (response) {
            if (response.status != false) {
                Design_Letter.letterGridLoad(response);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
         }
         else
             utility.DisplayMessages(strMessage, 2);
        //});
    },

    SearchLetter: function (letterData, letterId, aoData) {
        var data = "letterData=" + letterData + "&letterId=" + letterId;//+ aoData;
        
        return MDVisionService.defaultService(data, "DESIGN_LETTER", "SEARCH_LETTER");
    },

    letterGridLoad: function (response) {
        $("#pnlClinicalLetter #dgvLetter").dataTable().fnDestroy();
        $("#pnlClinicalLetter #pnlLetter_Result #dgvLetter tbody").find("tr").remove();
        if (response.letterCount > 0) {
            var letterLoadJSONData = JSON.parse(response.letterLoad_JSON);
            $.each(letterLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvLetter_row" + item.LetterId + "'))");
                $row.attr("id", "gvLetter_row" + item.LetterId);
                $row.attr("letterId", item.LetterId);
                $row.attr("Description", item.Description);
                $row.attr("ShortName", item.ShortName);
                $row.attr("Category", item.CategoryShortName);
                $row.attr("Folder", item.DocumentTypeShortName);
                $row.attr("Entity", item.EntityId);
                $row.attr("Labeled", item.IsLabeled);
                $row.attr("Active", item.IsActive);

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
                //var EditMethod = "Design_Letter.QuestionAddEdit(" + item.QuestionId + ",'Edit');";

                $row.append('<td style="display:none;">' + item.LetterId + '</td><td><a class="btn  btn-xs" href="#" onclick="Design_Letter.LetterDelete(' + item.LetterId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Design_Letter.LetterEdit(' + item.LetterId + ",'Edit'" + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Design_Letter.LetterActiveInactive(' + item.LetterId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>' + '</td><td>' + item.CategoryShortName + '</td>' + '</td><td>' + item.DocumentTypeShortName + '</td>' + '</td><td>' + item.EntityName + '</td>' + '</td><td>' + item.IsLabeled + '</td>' + '</td><td>' + item.IsActive + '</td>');

                $("#pnlLetter_Result #dgvLetter tbody").last().append($row);
            });
        }
        else {
            $('#pnlClinicalLetter #dgvLetter').DataTable({
                "language": {
                    "emptyTable": "No Letter is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlClinicalLetter #dgvLetter'))
            ;
        else
            $("#pnlClinicalLetter #pnlLetter_Result #dgvLetter").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    LoadEntityBasedData: function (entityID) {

        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#pnlClinicalLetter #lstFolderTypeId', 'GetDocumentType', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#pnlClinicalLetter #lstFolderTypeId', 'GetDocumentType', false, null);

        }
        //$('#' + Design_Letter.params["PanelID"] + ' #txtCPTCode').val("");
        //CacheManager.BindAutoCompleteText('#' + Design_Letter.params["PanelID"] + ' #txtCPTCode', 'GetCPTCode', true, null, entityID);

    },

    LetterDelete: function (LetterId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Patient Family", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LetterId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Design_Letter.LetterDeleted(selectedValue).done(function (response) {
                            if (response.status != false) {
                              
                                Design_Letter.LetterSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    LetterDeleted: function (LetterId) {
        var data = "letterID=" + LetterId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Design_Letter", "DELETE_LETTER");
    },

    LetterActiveInactive: function (LetterId, IsActive) {
        var strMessage = "";
       // AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
          //  if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = LetterId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Design_Letter.LetterUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Design_Letter.LetterSearch(selectedValue);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                   '3', null, null, null, IsActive
                );
           // }
          //  else
             //   utility.DisplayMessages(strMessage, 2);
       // });
    },

    LetterUpdateActiveInactive: function (MessageID, IsActive) {
        var data = "letterID=" + MessageID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Design_Letter", "UPDATE_LETTERS_ACTIVE_INACTIVE");
    },

    SearchSection: function (UserData, userID) {
        var data = "UserData=" + UserData + "&userID=" + userID;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "SEARCH_TEMPLATE");
    },

    SectionGridLoad: function (response) {
        $("#pnlClinicalSection #dgvSection").dataTable().fnDestroy();
        $("#pnlClinicalSection #pnlSection_Result #dgvSection tbody").find("tr").remove();
        if (response.UserCount > 0) {
            var UserLoadJSONData = JSON.parse(response.UserLoad_JSON);
            $.each(UserLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvSection_row" + item.UserId + "'))");
                $row.attr("id", "gvSection_row" + item.UserId);
                $row.attr("UserId", item.UserId);
                $row.attr("UserName", item.UserName);
                $row.attr("FirstName", item.FirstName);
                $row.attr("LastName", item.LastName);
                $row.attr("IsAdmin", item.IsAdmin);

                if (item.IsAdmin == "True")
                    admin = 'Yes';
                else
                    admin = 'No';

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }
                $row.append('<td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Design_Letter.SectionDelete(' + item.UserId + ",'" + item.IsAdmin + "'" + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Design_Letter.SectionEdit(' + item.UserId + ",'" + item.IsAdmin + "'" + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Section.SectionActiveInactive(' + item.UserId + "," + isactive + ",'" + item.IsAdmin + "'" + ');" title="Inactive Record"><i class="' + tglclass + '"></i></a></td><td>' + item.UserName + '</td><td>' + item.FirstName + '</td><td>' + item.LastName + '</td><td>' + item.EntityName + '</td><td>' + admin + '</td>');

                $("#pnlSection_Result #dgvSection tbody").last().append($row);
            });
        }
        else {
            $('#pnlClinicalSection #dgvSection').DataTable({
                "language": {
                    "emptyTable": "No User Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlClinicalSection #dgvSection'))
            ;
        else
            $("#pnlClinicalSection #pnlSection_Result #dgvSection").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SectionActiveInactive: function (UserId, IsActive, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
                { utility.DisplayMessages("Admin User can't be Active / In Active !", 3); }
                else
                {
                    utility.myConfirm('3', function () {
                        var selectedValue = UserId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {

                            userDetail.UpdateUserActiveInactive(selectedValue, IsActive).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Clinical_Section.SectionSearch('0');
                                    UnloadActionPan();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                       '3', null, null, null, IsActive
                    );
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LetterEdit: function (LetterID, mode) {
    
           
        var params = [];
                    params["LetterID"] = LetterID;
                   
                    params["mode"] = mode;
                    LoadActionPan('letterDetail', params);
                   
                    //LoadActionPan('userDetail', params);
               
      
    },

    SectionDelete: function (UserId, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
                { utility.DisplayMessages("Admin User can't be Delete !", 3); }
                else
                {
                    utility.myConfirm('1', function () {
                        var selectedValue = UserId;
                        var oTable = $('#dgvSection').DataTable();
                        var ind = $(this).index();
                        var idx = oTable.row(this).index();
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_Section.DeleteUser(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    var table1 = $('#dgvSection').DataTable();
                                    table1.row('.active').remove().draw(false);
                                    utility.DisplayMessages(response.Message, 1);
                                    //utility.DisplayMessages("Record Deleted Successfully.", 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () {

                    },
                        '1'
                    );
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteSection: function (userID) {
        var data = "UserID=" + userID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_USER");
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

    ABC: function (e, type) {
        $('#dt').hide();        
        //$('select').on('change', function (e) {
        var a = $('#chkIsActice :selected').text();
        var b = parseInt($('#chkIsActice :selected').val());
        var optionSelected = $("#chkIsActice option:selected", this);
        var valueSelected = b;
        //var textSelected = $("option:selected", this).text();
        var data = ['- SELECT -'];
        //$("#testid").find("select").remove();
        //$("#testid").find("input").remove();

        $('div#testid').empty();
        if (valueSelected == 0) {

            // Create Button
            //var $ctrl = $('<input/>').attr({ type: 'button', name: 'btn', value: 'Button' }).addClass("btn btn-success btn-sm");
            //$("#testid").append($ctrl);

            
            var lines = $('#mainScrollPan #pnlSection_Search #txtLastName').val().split(/\n/);
            var arraytoCommaSeperatedString = lines.join(','); //alert(arraytoCommaSeperatedString);
            var commaSeperatedStringtoArray = arraytoCommaSeperatedString.split(','); //alert(commaSeperatedStringtoArray);
            var texts = [];
            for (var i = 0; i < lines.length; i++) {
                // only push this line if it contains a non whitespace character.
                if (/\S/.test(lines[i])) {
                    texts.push($.trim(lines[i]));
                    data.push($.trim(lines[i]));
                }
            }
            //alert(JSON.stringify(texts));
            var s = $('<select />').addClass("form-control");
            for (var val in data) {
                $('<option />', { value: val, text: data[val] }).appendTo(s);
            }

            s.appendTo("#testid");
        }
        else if (valueSelected == 1) {

            //$("#testid").addclass('btn-group');
            //var lbl = $('#mainScrollPan #pnlSection_Search #txtLastName').val();
            //lbl.addClass('btn btn-default');
            //$("#testid").append(lbl);

            var lbl = $('#mainScrollPan #pnlSection_Search #txtFirstName').val();
            var lbl1 = $('#mainScrollPan #pnlSection_Search #txtUserName').val();

            var $ctrl = $('<input/>').attr({ type: 'radio', name: 'rad' }).addClass("rad");
            var $ctrl1 = $('<input/>').attr({ type: 'radio', name: 'rad' }).addClass("rad");
            $("#testid").append($ctrl);
            $("#testid").append("  " + lbl + "&nbsp;&nbsp;&nbsp;&nbsp;");
            $("#testid").append($ctrl1);
            $("#testid").append("  " + lbl1);
        }
        else if (valueSelected == 2) {
            //var lbl = $('#mainScrollPan #pnlSection_Search #txtFirstName').val();
            //var $ctrl = $('<input/>').attr({ type: 'text', name: 'text', value: lbl }).addClass("form-control");
            //$("#testid").append($ctrl);
            var html = '<div class="input-group">' +
                           ' <span class="input-group-addon"> <i class="fa fa-calendar"></i> </span>' +
                           '<input class="form-control " type="text" id="dtpDOB" name="DOB" data-plugin-datepicker="" >' +
                       '</div';
            $("#testid").append(html);

        }
        else if (valueSelected == 3) {

            var lbl = $('#mainScrollPan #pnlSection_Search #txtFirstName').val();
            if (lbl == 'checkbox') {
                var html = '<div class="checkbox-custom checkbox-default">' +
                                                    '<input type="checkbox" class="form-control" id="chkActive"> ' +
                                                    '<label class="control-label" for="chkActive">' + lbl + '</label> ' +
                                                '</div>'
                $("#testid").append(html);
                //var $ctrl = $('<input/>').attr({ type: 'checkbox', name: 'chka' }).addClass("checkbox-custom checkbox-default");
                //$("#testid").append($ctrl);
                //$("#testid").append(" " + lbl);
            }
            else if (lbl == 'text') {
                var $ctrl = $('<input/>').attr({ type: 'text', name: 'text', value: lbl }).addClass("form-control");
                $("#testid").append($ctrl);
            }
            else {

            }
           
            //var html = '<div class="checkbox-custom checkbox-default">' +
            //                                    '<input type="checkbox" class="form-control" id="chkActive"> ' +
            //                                    '<label class="control-label" for="chkActive">' + lbl + '</label> ' +
            //                                '</div>'
            //$("#testid").append(html);

        }
        else if (valueSelected == 4) {
            var html = '<input type="file" class="btn btn-default btn-xs mr-sm btn-file" onchange="readURL(this);" />' +
                       '<img id="imdID" src="#" alt="Image..." />';
            $("#testid").append(html);
        
        }
    },
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#imdID')
                .attr('src', e.target.result)
                .width(200)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

