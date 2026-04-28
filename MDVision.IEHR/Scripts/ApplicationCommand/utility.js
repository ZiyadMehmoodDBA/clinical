utility = {
    ValidateLenght: null,
    tempFileRef: null,
    okFunc: null,
    alertOKFunc: null,
    alertOKdontShowFunc: null,
    cancelFunc: null,
    isMDVision: null,
    timer: 0,
    IsShowICD10: false,
    FuncArray: [],
    selectOptionThrText(ctrlId, text) {
        $.each($(ctrlId).find("option"), function (i, item) {
            if ($(item).text() == text) {
                $(ctrlId).val($(item).attr("value"));
            }
        })
    },
    CheckPageLoaded: function (pageName) {
        if (window.location.href.toLowerCase().indexOf(pageName.toLowerCase()) > -1) {
            return true;
        } else {
            return false;
        }
    },
    ValidateFileSize: function (files) {
        var size = 0;
        $.each(files, function (index, file) {
            size = size + Number((file.size / (1024 * 1024)).toFixed(2));
        });
        return size;

    },
    saveTabHtml: function (id, content) {
        //fix caching
        //store.save(id, content);
    },
    getTabHtml: function (id) {
        return store.fetch(id);
    },

    checkAll: function (CheckBox, Grid, Rowindex) {
        if ($(CheckBox).is(":checked")) {
            var a = $(Grid + " tr td:nth-child(" + Rowindex + ")").find('input:checkbox');
            $.each(a, function (i, v) {
                if ($(v).closest('tr').css('display') != "none") {
                    $(v).prop("checked", true);
                }
            });
        } else {
            $(Grid + " tr td:nth-child(" + Rowindex + ") input[type=checkbox]").prop("checked", false);
        }
    },

    SwicthWidgetInializatoin: function () {
        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    getTimeIn12HrFromDatetime: function (date) {
        if (date) {
            return time = date.getHours() + ":" + date.getMinutes() + " " + (date.getHours() >= 12 ? 'PM' : 'AM');
        }
        else {
            return "";
        }

    },
    encodeURIData: function (strData) {
        var encodedData = strData;
        encodedData = encodedData.replace(/\"/g, '\'');
        encodedData = encodedData.replace(/\\/g, '\\\\');
        encodedData = encodeURIComponent(encodedData);
        return encodedData;
    },
    // Pls. use DisplayMessage function instead of ShowMessage
    ShowMessage: function (type, msg) {
        if (top.window.parent.slideMessagePan != undefined)
            top.window.parent.slideMessagePan('topMessagePan', type, msg);
        else
            alert(msg);
    },
    decodeHtml: function (html) {
        var txt = document.createElement("textarea");
        txt.innerHTML = html;
        return txt.value;
    },
    getContent: function (html) {
        var span = document.createElement('span');
        span.innerHTML = html;
        return span.innerText;
    },
 
    StripHTML: function stripHTML(dirtyString) {
        var container = document.createElement('div');
        var text = document.createTextNode(dirtyString);
        container.appendChild(text);
        return container.innerHTML; // innerHTML will be a xss safe string

    },

    bindEnterKey: function (btnID) {
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $(btnID).click();
            }
        });

    },
    DisplayMessages: function (Message, Type, isSticky, delayTime) {

        if (!delayTime) {
            delayTime = 7000;
        }
        if (Message == "" || Message == undefined || Message == "undefined" || Message == null || Message.Redirect != null)
            return;

        if (Type == 1 && globalAppdata.IsShowSuccessMessages == "True")
        { $().toastmessage('showSuccessToast', Message).delay(delayTime); }
        if (Type == 2)
        { $().toastmessage('showNoticeToast', Message).delay(delayTime); }
        if (Type == 3) {
            //if (isSticky) {
            //    $().toastmessage('showToast', {
            //        text: Message,
            //        sticky: true,
            //        position: 'top-right',
            //        type: 'warning',
            //        close: function () { $('.modal-backdrop').remove(); }
            //    });
            //    $('body').append('<div class="modal-backdrop fade in"></div>')
            //} else {
            $().toastmessage('showWarningToast', Message).delay(delayTime);
            //}
        }
        if (Type == 4)
        { $().toastmessage('showErrorToast', Message).delay(delayTime); }

        $('.toast-container').click(function () {
            try {
                $(this).find('div.toast-item-wrapper').remove();
            }
            catch (err) { console.log(err) }
        })

    },

    getAorAn: function (word) {

        var a_ = "a";
        if (word != "") {

            var temp = "";
            var vowels = ["a", "e", "i", "o", "u"];
            temp = word.substring(0, 1);
            if (vowels.indexOf(temp.toLowerCase()) >= 0)
                a_ = "an";
        }

        return a_;

    },

    myConfirmDetail: function (dialogText, okFunc, cancelFunc, dialogTitle, printType, FooterNote) {
        var okBtnTitle = "Yes";
        var okBtnClass = "btn-success";
        var cancelBtnTitle = "No";
        var cancelBtnClass = "btn-danger";
        var rendomKey = utility.makeRendomKey();
        utility.makeFuncArray(rendomKey, okFunc, cancelFunc);

        var okHtml = '<a href="#" id="btnYes"  onclick="utility.saveConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn ' + okBtnClass + '">' + okBtnTitle + '</a>';
        var cancelHtml = '<a href="#" id="btnNo" onclick="utility.cancelConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn ' + cancelBtnClass + '" >' + cancelBtnTitle + '</a>';
        var BtnsHtml = okHtml + cancelHtml;
        var labelHtml = "";
        if (printType == 'Result')
            labelHtml = '<label class="pull-left col-sm-2 col-md-8 pl-none text-left"><b class="text-danger">NOTE: </b>' + FooterNote + '</label>';
        else
            labelHtml = "";
        var markUp = '<div id="modal-from-dom" class="modal fade ">' +
                         '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                         '<div class="modal-content">' +
                         '<div class="modal-header">' +
                            '<label class="changed-field">' + dialogTitle + '</label>' +
                         '</div>' +
                         '<div class="modal-body bg-white" >' +
                            '<p>' + dialogText + '</p>' +
                         '</div>' +
                         '<div class="modal-footer">' + labelHtml + ' ' + BtnsHtml + '</div>' +
                     '</div> <div></div>';

        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {
            $('#btnNo', this).focus();

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
    },

    myConfirm: function (dialogText, okFunc, cancelFunc, dialogTitle, okBtnTitle, cancelBtnTitle, IsOnlyOKBtn, IsActive) {

        var dialogKey = dialogText;

        if (utility.checkKeyExistsDoNotAskedAgain(dialogKey, okFunc)) {
            return;
        }

        if (dialogText == "1") {
            dialogText = "Do you want to delete this record?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "2") {
            dialogText = "Do you want to exit without saving?";
            dialogTitle = "Confirm Save";
        }

        else if (dialogText == "3") {

            var lDialogText = null;
            if (IsActive == "1") {
                lDialogText = "Do you want to mark the record as Active?";
            } else if (IsActive == "0") {
                lDialogText = "Do you want to mark the record as Inactive?";
            } else {
                lDialogText = "Do you want to Active/Inactive this record?";
            }
            dialogText = lDialogText;
            dialogTitle = "Confirm Active/Inactive";
        }
        else if (dialogText == "4") {
            dialogText = "Do you want to mark this record as Primary Contact?";
            dialogTitle = "Confirm Primary";
        }
        else if (dialogText == "5") {
            dialogText = "Do you want to mark change Insurance Plan priority?";
            dialogTitle = "Confirm Change Priority";
        }
        else if (dialogText == "6") {
            dialogText = "Do you want to mark this document as Reviewed?";
            dialogTitle = "Confirm Review Document";
        }
        else if (dialogText == "7") {
            dialogText = "Do you want to mark this document as Signed?";
            dialogTitle = "Confirm Sign Document";
        }
        else if (dialogText == "8") {
            dialogText = "Do you want to scan other side of this card?";
            dialogTitle = "Confirm Scan Card";
        }
        else if (dialogText == "9") {
            dialogText = "There are no Further Documents available.";
            dialogTitle = "Document Alert";
        } else if (dialogText == "10") {
            dialogText = "Do you want to close this Patient?";
            dialogTitle = "Close Patient";
        } else if (dialogText == "11") {
            dialogText = "Do you want to mark this record as Non-Primary contact?";
            dialogTitle = "Confirm Non-Primary";
        } else if (dialogText == "12") {
            dialogText = "Record was updated. Do you want to save changes?";
            dialogTitle = "Confirm Save";
        } else if (dialogText == "13") {
            dialogText = "Are you sure you want to post ERA?";
            dialogTitle = "Confirm ERA Payment Posting";
        } else if (dialogText == "14") {
            dialogText = "Do you want to replace the associated vital record with the new vital?";
            dialogTitle = "Confirm New Vital Association";
        } else if (dialogText == "15") {
            dialogText = "Vital records will be copied with current date and time. Would you want to proceed?";
            dialogTitle = "Confirm Copy Selected Vital";
        } else if (dialogText == "16") {
            //dialogText = "The selected record is associated with a note, by updating this record the note will also be updated would you like to proceed?";
            dialogText = "The selected record is associated with a note. Do you want to update record and note?";
            dialogTitle = "Confirm Updating Selected Vital";
        }
            // Begin 7-12-2015 Muhammad Arshad SocialHx confirm for Unremarkable
        else if (dialogText == "17") {
            dialogText = "Are you sure you want to mark Social History as Unremarkable?";
            dialogTitle = "Confirm Social History as Unremarkable";
        }
            // End 7-12-2015 Muhammad Arshad SocialHx confirm for Unremarkable
            // Begin 10-12-2015 Muhammad Azhar Shahzad ProgressNote Copy Previous Component confirm for over-write
        else if (dialogText == "18") {
            dialogText = "Previous components data will be overwrite, do you want to continue?";
            dialogTitle = "Copy Previous Component Note";
        }
            // End 10-12-2015 Muhammad Azhar Shahzad  ProgressNote Copy Previous Component confirm for over-write
            // Begin 11-12-2015 Muhammad Azhar Shahzad ||the prompt will update the date as well as add the social history component on the progress notes.
        else if (dialogText == "19") {
            dialogText = "Are you sure you want to save the changes? The date will be modified with current date.";
            dialogTitle = "Save Changes";
        }
        else if (dialogText == "20") {
            dialogText = "Are you sure you want to unlink this change?";
            dialogTitle = "Confirm Alert";
        }
            // Start 14-01-2016 Muhammad Arshad Confirm for change order of MiscHx Component
        else if (dialogText == "21") {
            dialogText = "Do you want to change Component Order?";
            dialogTitle = "Confirm Component Order";
        }
            // Start 14-01-2016 Muhammad Arshad Confirm for change order of MiscHx Component
            // End 11-12-2015 Muhammad Azhar Shahzad ||the prompt will update the date as well as add the social history component on the progress notes.-

            //Start//15/01/2015//Ahmad Raza//Implimented confirm prompt for Reset Form Values
        else if (dialogText == "22") {
            dialogText = "This will reset all field values in this section to default. This action is irreversible and cannot be undone. Would you like to proceed?";
            dialogTitle = "Confirm Reset Form";
        }
        else if (dialogText == "23") {
            dialogText = "Are you sure you want to delete the disease?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "24") {
            dialogText = "This will mark all Systems as Normal and reset all values in Systems. Would you like to proceed?";
            dialogTitle = "Confirm Mark Normal";
        }
            //End//15/01/2015//Ahmad Raza//Implimented confirm prompt for Reset Form Values

        else if (dialogText == "25") {
            dialogText = "Are you sure you want to delete the Member detail?";
            dialogTitle = "Confirm Delete";
        }
            // 26 By Khaleel Ur Rehman for Reset in Review of systems. Date : 04 Feb 2016.
        else if (dialogText == "26") {
            dialogText = "This will reset all values in all Systems and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?";
            dialogTitle = "Confirm Reset";
        }
            // 27 By M AHMAD IMRAN for Reset in Complaints. Date : 16 Feb 2016.
        else if (dialogText == "27") {
            dialogText = "This will reset all values in all Complaints and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?";
            dialogTitle = "Confirm Reset";
        }
        else if (dialogText == "28") {
            dialogText = "This will delete and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?";
            dialogTitle = "Confirm Reset";
        }
            //Start//19-02-2016//Ahmad Raza//Added prompt message string for PhysicalExam Soap detach and Component detahch from Notes
        else if (dialogText == "29") {
            dialogText = "This will remove all information of Physical Exam. This action is irreversible. Would you like to proceed?";
            dialogTitle = "Confirm Delete";
        }
            //End//19-02-2016//Ahmad Raza//Added prompt message string for PhysicalExam Soap detach and Component detahch from Notes
        else if (dialogText == "30") {
            dialogText = 'Are you sure you would like to delete this template?';
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "31") {
            dialogText = 'Insurance balance will be negative if you will post this payment?';
            dialogTitle = "Confirm Posting";
        }
        else if (dialogText == "32") {
            dialogText = 'This check has already been posted are you sure you want to repost?';
            dialogTitle = "Confirm Posting";
        }
        else if (dialogText == "33") {
            //Start || 15 July, 2016 || ZeeshanAK || Fix for EMR-1608
            dialogText = 'Are you sure you would like to delete this template?';
            //End   || 15 July, 2016 || ZeeshanAK || Fix for EMR-1608
            dialogTitle = "Confirm Posting";
        }
        else if (dialogText == "34") {
            dialogText = 'Are you sure you would like to delete this Letter?';
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "35") {
            dialogText = "Record was Reseted. Do you want to Update Progress Note?";
            dialogTitle = "Confirm Update";
        }
        else if (dialogText == "36") {
            dialogText = 'If you mark this result as "Acknowledged", you will not be able to make further changes to it. Are you sure you want to proceed?';
            dialogTitle = "Acknowledg Result";
        }
        else if (dialogText == "37") {
            dialogText = " Data has been modified. Are you sure you want to save the changes?";
            dialogTitle = "Confirm Save";
        }
        else if (dialogText == "38") {
            dialogText = " Do you want to add task?";
            dialogTitle = "Confirm Update";
        }
        else if (dialogText == "39") {
            dialogText = " Are you sure you want to void the dose?";
            dialogTitle = "Confirm Alert";
        } else if (dialogText == "40") {
            dialogText = "Do you want to delete selected message(s)?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "41") {
            dialogText = "Are you sure you want to cancel this order?";
            dialogTitle = "Confirm Action";
        }
        else if (dialogText == "42") {
            dialogText = "By unlocking this provider note you agree to release MDVision EMR from all legal implications that may arise pertaining to this patient and other patients that may be affected by this section." +
                "</br></br>" + "Do you want to continue?";
            dialogTitle = "Unsign Provider Note";
        }
        else if (dialogText == "43") {
            dialogText = "Are you sure you want to mark this note as reviewed?";
            dialogTitle = "Note Review";
        }
        else if (dialogText == "44") {
            dialogText = "Are you sure you want to mark this note as not reviewed?";
            dialogTitle = "Note Review";
        }
        else if (dialogText == "45") {
            dialogText = "Do you want to save the changes?";
            dialogTitle = "Exit Alert";
        }
        else if (dialogText == "46") {
            dialogText = "Do you want to change Existing E&M code?";
            dialogTitle = "Duplicate E&M Alert";
        }
        else if (dialogText == "47") {
            dialogText = "An E&M code already added, are you sure you want to add another E&M code?";
            dialogTitle = "Duplicate E&M Alert";
        }
        else if (dialogText == "48") {
            dialogText = "An E&M code already added, are you sure you want to add another E&M code?";
            dialogTitle = "Exit Alert";
        }
        else if (dialogText == "49") {
            dialogText = "The Selected immunization in the Order set is not recomended for Patient's Age! would you like to proceed?";
            dialogTitle = "Immunization Alert";
        }
        else if (dialogText == "51") {
            dialogText = "Do you want to delete this record(s)?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "52") {
            dialogText = "Are you sure you want to mark this batch as Completed?";
            dialogTitle = "Batch Complete";
        }
        else if (dialogText == "53") {
            dialogText = "Are you sure you want to Reprocess this batch?";
            dialogTitle = "Batch Reprocess";
        }
        else if (dialogText == "54") {
            dialogText = "Are you sure you want to delete this goal?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "55") {
            dialogText = "Are you sure you want to delete this health concern?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "56") {
            dialogText = "Are you sure you want to delete this intervention?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "57") {
            dialogText = "Are you sure you want to delete this outcome?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "58") {
            dialogText = "Do you want to append this document?";
            dialogTitle = "Confirm Append";
        } else if (dialogText == "59") {
            dialogText = "Kindly allocate unsaved document(s) in folder(s) or else they will be discarded." + "</br>" + "Do you want to proceed?";
            dialogTitle = "Confirmation Message";
        }
        else if (dialogText == "60") {
            dialogText = "You have not added the treatment plans on the note, your changes will be discarded. Do you still want to continue?";
            dialogTitle = "Confirm Save";
        }
        else if (dialogText == "61") {
            dialogText = 'Appointment for this patient already exists with "Cancel" status. Do you want to create a duplicate appointment?';
            dialogTitle = "Alert";
        }
        if (dialogText == "50") {
            dialogText = "Do you want to delete all selected records?";
            dialogTitle = "Confirm Delete";
        }
        else if (dialogText == "62") {
            dialogText = "Selected Referral/Prior Auth is expired. Are you sure you want to continue?";
            dialogTitle = "Referral Expired";
        }
        else if (dialogText == "63") {
            sessionStorage.removeItem("NoteMissingDataReason");
            dialogText = "<textarea id='txtNoteMissingDataReason' spellcheck='true' maxlenght='100' oninput='utility.setNoteMissingDataReason(this);' style='width: 100%; height: 100px;'></textarea>";
            dialogTitle = "<b>Please Enter The Reason</b>";
        }
        else if (dialogText == "64") {
            dialogText = "The selected complaint will be deleted.";
        }
        else if (dialogText == "65") {
            dialogText = "You might not be able to qualify for unselected measures. Do you still wish to continue?";
            dialogTitle = "Confirm MIPS Alerts";
        }
        else if (dialogText == "66") {
            dialogText = "Are you sure you want to delete the selected messages?";
            dialogTitle = "Confirm Delete";
        }

        if (!okBtnTitle)
            okBtnTitle = "Yes";
        var cancelBtnClass = "btn-danger";
        var okBtnClass = "btn-success";
        if (!cancelBtnTitle)
            cancelBtnTitle = "No";
        else if (cancelBtnTitle == "No, not this time") {
            cancelBtnClass = "btn-danger";
            okBtnClass = "btn-success";
        }
        //utility.okFunc = okFunc;
        //utility.cancelFunc = cancelFunc;
        var rendomKey = utility.makeRendomKey();
        utility.makeFuncArray(rendomKey, okFunc, cancelFunc);

        var okHtml = '<a href="#" id="btnYes"  onclick="utility.saveConfirmDialog(' + "'" + rendomKey + "'" + ",'" + dialogKey + "',this" + ');"  class="btn ' + okBtnClass + '">' + okBtnTitle + '</a>';
        var cancelHtml = '<a href="#" id="btnNo" onclick="utility.cancelConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn ' + cancelBtnClass + '" >' + cancelBtnTitle + '</a>';
        var BtnsHtml = "";

        if (IsOnlyOKBtn == true) {
            BtnsHtml = '<a href="#" id="btnYes"  onclick="utility.saveConfirmDialog(' + "'" + rendomKey + "'" + ",'" + dialogKey + "',this" + ');" data-dismiss="modal" class="btn btn-success">' + okBtnTitle + '</a>';;
        }
        else
            BtnsHtml = okHtml + cancelHtml;

        var doNotText = globalAppdata["IsConfigureAlerts"] == "True" ? '<div class="pull-left"> <div class="checkbox-custom"><input id="donotaskedagain"  type="checkbox" value="' + dialogKey + '" />   <label class="control-label" for="donotaskedagain"> Do not ask again</label></div> </div>' : '';

        var markUp = '<div id="modal-from-dom" class="modal fade ">' +
                         '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                         '<div class="modal-content">' +
                         '<div class="modal-header">' +
                            //'<a href="#" class="close">&times;</a>' +
                            '<label class="changed-field">' + dialogTitle + '</label>' +

                         '</div>' +
                         '<div class="modal-body bg-white" >' +
                         '<p>' + dialogText + '</p>' +
                         '</div>' +
                         '<div class="modal-footer">' + doNotText + BtnsHtml + '<br /></div>' +
                     '</div> <div></div>';
        $("#donotaskedagain").val(dialogKey);
        //$('body').append(markUp);

        //markup = utility.DoNotAskedAgainCheckBoxHTML(markUp, dialogKey);

        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {
            $('#btnNo', this).focus();

        }).on('hidden.bs.modal', function () {
            //$('body').css('overflow', 'auto !important');
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });




        //$('<div style="padding: 25px; min-width: 400px; max-width: 500px; word-wrap: break-word;"><i style="float:left;" class="fa fa-exclamation-circle fa-3x blue"></i><div style="margin:10px 10px 10px 60px;">' + dialogText + '</div></div>').dialog({
        //    draggable: false,
        //    modal: true,
        //    resizable: false,
        //    width: 'auto',
        //    title: dialogTitle || 'Confirm',
        //    minHeight: 75,
        //    buttons: {
        //        OK: function () {
        //            if (typeof (okFunc) == 'function') {
        //                setTimeout(okFunc, 50);
        //            }
        //            $(this).dialog('destroy');
        //        },
        //        Cancel: function () {
        //            if (typeof (cancelFunc) == 'function') {
        //                setTimeout(cancelFunc, 50);
        //            }
        //            $(this).dialog('destroy');
        //        }
        //    }
        //});
    },

    setNoteMissingDataReason: function (obj) {
        sessionStorage.setItem("NoteMissingDataReason", $(obj).val());
    },

    checkKeyExistsDoNotAskedAgain: function (dialogKey, okFunc) {

        var lsStr = localStorage.getItem(globalAppdata.AppUserName);
        if (lsStr) {
            lsArray = JSON.parse(lsStr);

            if ($.inArray(dialogKey, lsArray) > -1 && typeof (okFunc) == 'function') {
                setTimeout(okFunc, 50);
                return true;
            }
        }
        return false;
    },
    DoNotAskedAgainCheckBoxHTML: function (messageMarkup, checkBoxValue) {
        return messageMarkup += '<p><input id="donotaskedagain" type="checkbox" value="' + checkBoxValue + '" /> Do not ask again</p>';
    },
    //Start PRD-635 TahreeMalik Duplicate claim alert show in edit mode, when user wants to update record
    duplicateClaimAlert: function (dialogText, okFunc, cancelFunc, dialogTitle, okBtnTitle, cancelBtnTitle, IsOnlyOKBtn, IsClose, IsHideCloseBtn) {
        var okBtnClass = "btn-success";
        var cancelBtnClass = "btn-danger";
        var rendomKey = utility.makeRendomKey();
        utility.makeFuncArray(rendomKey, okFunc, cancelFunc);

        var okHtml = '<a href="#" id="btnYes"  onclick="utility.saveConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn ' + okBtnClass + '">' + okBtnTitle + '</a>';
        var cancelHtml = '<a href="#" id="btnNo" onclick="utility.cancelConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn ' + cancelBtnClass + '" >' + cancelBtnTitle + '</a>';
        var BtnsHtml = '';
        if (IsOnlyOKBtn == true) {
            BtnsHtml = '<a href="#" id="btnYes"  onclick="utility.saveConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn btn-success">' + okBtnTitle + '</a>';;
        }
        else
            BtnsHtml = okHtml + cancelHtml;
        
        var CloseHtml = '', HideCloseBtn = '';
        if (IsHideCloseBtn)
            HideCloseBtn = 'style="display:none;"';
        if (IsClose) {
            CloseHtml = '<button type="button" id="btnClose" class="close"' + HideCloseBtn + ' onclick="utility.unloadConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal"><span class=" red" aria-hidden="true">&nbsp;</span><span class="sr-only">Close</span></button>'
        }
        
        var markUp = '<div id="modal-from-dom" class="modal fade ">' +
                         '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                         '<div class="modal-content">' +
                         '<div class="modal-header">' + CloseHtml + '<label class="changed-field">' + dialogTitle + '</label>' +
                         '</div>' +
                         '<div class="modal-body bg-white" >' +
                            '<p>' + dialogText + '</p>' +
                         '</div>' +
                         '<div class="modal-footer">' + BtnsHtml + '</div>' +
                     '</div> <div></div>';

        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {
            $('#btnNo', this).focus();
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
    },
    //End PRD-635   TahreeMalik
    myConfirmNote: function (dialogText, saveNote, saveExit, exit, dialogTitle, close) {
        var dialogKey = dialogText;

        if (utility.checkKeyExistsDoNotAskedAgain(dialogKey, saveNote)) {
            return;
        }
        if (dialogText == "1") {
            dialogText = "You are about to exit. Select from one of the below actions:";
            dialogTitle = "Exit Alert";
        }
        if (dialogText == "CustomFormsPreview") {
            dialogText = "Do You want to Add Changes in Note?";
            dialogTitle = "Exit Alert";
            if (saveNote)
                saveAddToNote = "Yes";
            else
                saveAddToNote = "";
            if (saveExit)
                saveAndExit = "Save & Exit";
            else
                saveAndExit = "";
            if (exit)
                exitUnload = "No";
            else
                exitUnload = '';
        } else {
            if (saveNote)
                saveAddToNote = "Save & Add to Note";
            else
                saveAddToNote = "";
            if (saveExit)
                saveAndExit = "Save & Exit";
            else
                saveAndExit = "";
            if (exit)
                exitUnload = "Exit without Saving";
            else
                exitUnload = '';
            if (close)
                unloadConfirm = "Close";
            else
                unloadConfirm = '';
        }
        //utility.okFunc = okFunc;
        //utility.cancelFunc = cancelFunc;
        var rendomKey = utility.makeRendomKey();
        utility.makeFuncArrayNote(rendomKey, saveNote, saveExit, exit);
        var saveAddToNotehtml = "";
        var saveAndExithtml = "";
        var exitandUnloadhtml = "";
        if (saveNote)
            saveAddToNotehtml = '<a href="#" id="btnYes"  onclick="utility.saveNoteConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn btn-success">' + saveAddToNote + '</a>';
        if (saveExit)
            saveAndExithtml = '<a href="#" id="btnNo" onclick="utility.saveExitConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn btn-success" >' + saveAndExit + '</a>';
        if (exit)
            exitandUnloadhtml = '<a href="#" id="btnnew" onclick="utility.exitConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn btn-danger" >' + exitUnload + '</a>';
        if (close)
            unloadConfirmhtml = '<a href="#" id="btnclose" onclick="utility.unloadConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal" class="btn btn-danger" >' + unloadConfirm + '</a>';
        var BtnsHtml = "";
        BtnsHtml = saveAddToNotehtml + saveAndExithtml + exitandUnloadhtml;

        if (close)
            BtnsHtml += unloadConfirmhtml;

        var doNotText = globalAppdata["IsConfigureAlerts"] == "True" ? '<div class="col-sm-12 mb-sm"><div class="pull-left"> <div class="checkbox-custom"><input id="donotaskedagain"  type="checkbox" value="' + dialogKey + '" />   <label class="control-label" for="donotaskedagain"> Do not ask again</label></div> </div></div>' : '';


        var markUp = '<div id="modal-from-dom" class="modal fade ">' +
                         '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                         '<div class="modal-content">' +
                         '<div class="modal-header">' +
                         '<button type="button" class="close" onclick="utility.unloadConfirmDialog(' + "'" + rendomKey + "'" + ');" data-dismiss="modal"><span class=" red" aria-hidden="true">&nbsp;</span><span class="sr-only">Close</span></button>' +
                            //'<a href="#" class="close">&times;</a>' +
                            '<label class="changed-field">' + dialogTitle + '</label>' +
                         '</div>' +
                         '<div class="modal-body bg-white" >' +
                            '<p>' + dialogText + '</p>' +
                         '</div>' +
                         '<div class="modal-footer">' + doNotText + BtnsHtml + '</div>' +
                     '</div> <div></div>';
        $("#donotaskedagain").val(dialogKey);
        //$('body').append(markUp);


        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {
            $('#btnNo', this).focus();

        }).on('hidden.bs.modal', function () {
            //$('body').css('overflow', 'auto !important');
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });


    },
    saveConfirmDialog: function (rendonKey, dialogKey, obj) {

        var dfd = $.Deferred();
        if (dialogKey == "63") {
            if (!sessionStorage.getItem("NoteMissingDataReason")) {
                $("#txtNoteMissingDataReason").css("border-color", "red");
                utility.DisplayMessages("Please enter the Reason.", 2);
                dfd.resolve(false);
            }
            else {
                $("#txtNoteMissingDataReason").css("border-color", "#ccc");
                dfd.resolve(true);
            }
        }
        else {
            dfd.resolve(true);
        }

        dfd.then(function (IsContinue) {
            if (IsContinue) {
                var Func = utility.getFuncFromFuncArray(rendonKey);
                if (typeof (Func.okFunc) == 'function') {
                    setTimeout(Func.okFunc, 50);
                    utility.removeFromFuncArray(rendonKey);
                }
                $('body').addClass('modal-open');

                utility.AddKey2LocalStorageForDoNotAskedAgain();

                if (obj) {
                    $(obj).closest("#modal-from-dom").removeClass("fade").modal("hide");
                }
            }

        });

    },
    cancelConfirmDialog: function (rendonKey) {
        var Func = utility.getFuncFromFuncArray(rendonKey);
        if (typeof (Func.cancelFunc) == 'function') {
            if (Func.cancelFunc.toString().slice() != 'function () { }') {
                setTimeout(Func.cancelFunc, 500);
            }
            utility.removeFromFuncArray(rendonKey);
        }
        $('body').addClass('modal-open');

        utility.AddKey2LocalStorageForDoNotAskedAgain();

    },
    AddKey2LocalStorageForDoNotAskedAgain: function () {
        if ($("#donotaskedagain:checked").length > 0) {
            var keyValue = $("#donotaskedagain").val();
            var lsArray = [];

            var lsStr = localStorage.getItem(globalAppdata.AppUserName);
            if (lsStr) {
                lsArray = JSON.parse(lsStr);
                lsArray.push(keyValue);
            } else {
                lsArray.push(keyValue);
            }

            localStorage.setItem(globalAppdata.AppUserName, JSON.stringify(lsArray));
        }
        $("#donotaskedagain").detach();
    },
    saveNoteConfirmDialog: function (rendonKey) {
        var Func = utility.getFuncFromFuncArray(rendonKey);
        if (typeof (Func.saveNote) == 'function') {
            setTimeout(Func.saveNote, 50);
            utility.removeFromFuncArray(rendonKey);

        }
        $('body').addClass('modal-open');
        utility.AddKey2LocalStorageForDoNotAskedAgain();
    },
    saveExitConfirmDialog: function (rendonKey) {
        var Func = utility.getFuncFromFuncArray(rendonKey);
        if (typeof (Func.saveExit) == 'function') {
            if (Func.saveExit.toString().slice() != 'function () { }') {
                setTimeout(Func.saveExit, 500);
            }
            utility.removeFromFuncArray(rendonKey);
        }
        $('body').addClass('modal-open');
        utility.AddKey2LocalStorageForDoNotAskedAgain();
    },
    exitConfirmDialog: function (rendonKey) {
        var Func = utility.getFuncFromFuncArray(rendonKey);
        if (typeof (Func.exit) == 'function') {
            if (Func.exit.toString().slice() != 'function () { }') {
                setTimeout(Func.exit, 500);
            }
            utility.removeFromFuncArray(rendonKey);
        }
        $('body').addClass('modal-open');
        utility.AddKey2LocalStorageForDoNotAskedAgain();
    },

    unloadConfirmDialog: function () {
        $('body').addClass('modal-open');

    },

    alertDialog: function (dialogText, dialogTitle, CheckBoxLabel, alertOKFunc, alertOKdontShowFunc, alertShowScreenFunc, dynamicId) {
        utility.alertOKFunc = alertOKFunc;
        utility.alertOKdontShowFunc = alertOKdontShowFunc;
        utility.alertShowScreenFunc = alertShowScreenFunc;
        var markUp = '<div class="modal fade">' +
  '<div class="modal-dialog modal-dialog-sm">' +
    '<div class="modal-content">' +
      '<div class="modal-header">' +
        '<button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>' +
        '<h4 class="modal-title">' + dialogTitle + '</h4>' +
      '</div>' +
      '<div class="modal-body">' +
        '<p>' + dialogText + '</p>' +
      '</div>' +
      '<div class="modal-footer">';
        if (typeof dynamicId != "undefined") {
            markUp += "<div class='pull-left'><input type='checkbox' class='dontShowThis" + dynamicId + "' id='dontShowThis" + dynamicId + "' onclick='utility.alertOKCheckBoxAction();' name='noShowAgain'/>" + CheckBoxLabel + "</div>";
        }
        else {
            markUp += "<div class='pull-left'><input type='checkbox' id='dontShowThis' class='dontShowThis' onclick='utility.alertOKCheckBoxAction();' name='noShowAgain'/>" + CheckBoxLabel + "</div>";
        }
        markUp += '<a href="#" id="btnOK"  onclick="utility.alertDialogAction();" data-dismiss="modal" class="btn btn-success">OK</a>' +
        '<a href="#" id="btnShowAlert"  onclick="utility.alertShowScreenAction();" data-dismiss="modal" class="btn btn-success">SHOW</a>' +
        '</div>' +
      '</div>' +
    '</div>' +
  '</div>'
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            //$('body').css('overflow', 'auto !important');
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });
    },
    alertOKCheckBoxAction: function () {
        if (typeof (utility.alertOKFunc) == 'function') {
            setTimeout(utility.alertOKFunc, 50);

        }
        $('body').addClass('modal-open');
    },
    alertDialogAction: function () {
        if (typeof (utility.alertOKdontShowFunc) == 'function') {
            setTimeout(utility.alertOKdontShowFunc, 50);

        }
        $('body').addClass('modal-open');
    },
    alertShowScreenAction: function () {
        if (typeof (utility.alertShowScreenFunc) == 'function') {
            setTimeout(utility.alertShowScreenFunc, 50);

        }
        // $('body').addClass('modal-open');
    },
    UnLoadDialog: function (fromId, WithAlertFunc, WithOutAlertFunc) {

        if (fromId.indexOf("#") == 0)
            fromId = fromId;
        else
            fromId = "#" + fromId;

        var formCtr = $(fromId);

        if (formCtr.serialize() != formCtr.data('serialize')) {
            utility.myConfirm('2', function () {
                WithAlertFunc();
            }, function () { },
                    '2'
                );
        }
        else {
            WithOutAlertFunc();
        }

    },

    checkSpecialChar: function (event) {
        if (!((event.keyCode >= 65) && (event.keyCode <= 90) || (event.keyCode >= 97) && (event.keyCode <= 122) || (event.keyCode >= 48) && (event.keyCode <= 57) || (event.keyCode == 32) || (event.keyCode == 45))) {
            event.returnValue = false;
            return;
        }
        event.returnValue = true;
    },
    ValidateSpecialCharacters: function (obj) {

        var pat_ = /[`~,.<>;:"/[\]#@!?&$%^|{}()=_+-]/;
        if (pat_.test($(obj).val())) {
            $(obj).val($(obj).val().replace(pat_, ""));
            if (pat_.test($(obj).val()))
                utility.ValidateSpecialCharacters(obj);
        }
    },
    MinutesTemplate: function (value) {
        return value === "" ? "" : value + " (min)";
    },
    DateTemplate: function (date) {
        if (date == null || date == "")
            return "";
        else {
            var date_format = 'mm/dd/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];

            return moment(new Date(date)).format(date_format.toLocaleUpperCase());
        }
    },
    ClosePanel: function () {
        (function ($) {

            $(function () {
                $('.panel')
                    .on('click', '.panel-actions a.fa-caret-up', function (e) {
                        e.preventDefault();

                        var $this,
                            $panel;

                        $this = $(this);
                        $panel = $this.closest('.panel');

                        $this
                            .removeClass('fa-caret-up')
                            .addClass('fa-caret-down');

                        $panel.find('.panel-body, .panel-footer').slideDown(200);
                    })
                    .on('click', '.panel-actions a.fa-caret-down', function (e) {
                        e.preventDefault();

                        var $this,
                            $panel;

                        $this = $(this);
                        $panel = $this.closest('.panel');

                        $this
                            .removeClass('fa-caret-down')
                            .addClass('fa-caret-up');

                        $panel.find('.panel-body, .panel-footer').slideUp(200);
                    })
                    .on('click', '.panel-actions a.fa-times', function (e) {
                        e.preventDefault();

                        var $panel,
                            $row;

                        $panel = $(this).closest('.panel');

                        if (!!($panel.parent('div').attr('class') || '').match(/col-(xs|sm|md|lg)/g) && $panel.siblings().length === 0) {
                            $row = $panel.closest('.row');
                            $panel.parent('div').remove();
                            if ($row.children().length === 0) {
                                $row.remove();
                            }
                        } else {
                            $panel.remove();
                        }
                    });
            });

        })(jQuery);
    },


    ValidateZero: function (event, control) {
        $("#" + control.id).keyup(function () {
            if (!$("#" + control.id).val().match(/^\d+$/)) {
                $("#" + control.id).val("");
            }
        });
        //var evt = (evt) ? evt : window.event;
        if (event.keyCode == 9 || event.keyCode == 8) {
            return true;
        }
        if (control.value.length == 0 && (event.which > 48 && event.which <= 57)) {
            return true;
        }
        else if (control.value.length > 0 && (event.which > 47 && event.which <= 57)) {
            return true;
        }
        return false;

    },


    ValidateForwardSlash: function (event, control) {

        if (event.keyCode == 47) {
            event.preventDefault();
        }
    },

    ValidateOneAndNumeric: function (event, control) {
        $("#" + control.id).keyup(function () {
            var VAL = $("#" + control.id).val();
            var oneTo999 = new RegExp('^([1-9][0-9]{0,2}|999)$');
            if (!oneTo999.test(VAL)) {
                $("#" + control.id).val("");
            }
        });
        if (event.keyCode == 9 || event.keyCode == 8) {
            return true;
        }
        if (control.value.length == 0 && (event.which > 48 && event.which <= 57)) {
            return true;
        }
        else if (control.value.length > 0 && (event.which > 47 && event.which <= 57)) {
            return true;
        }
        return false;
    },

    ValidateZeroAndNumeric: function (event, control) {
        $("#" + control.id).keyup(function () {
            var VAL = $("#" + control.id).val();
            var oneTo999 = new RegExp('^([1-9][0-9]{0,2}|999)$');
            if (!oneTo999.test(VAL)) {
                $("#" + control.id).val("");
            }
        });
        if (event.keyCode == 9 || event.keyCode == 8) {
            return true;
        }
        if (control.value.length == 0 && (event.which >= 48 && event.which <= 57)) {
            return true;
        }
        else if (control.value.length > 0 && (event.which > 47 && event.which <= 57)) {
            return true;
        }
        return false;
    },

    SelectGridRow: function (obj) {
        //if (obj.className != 'active') {
        if (obj.hasClass("active")) {
        }
        else {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },
    bindMyJSON: function (bAuto, myJson, isValidation, self) {

        isValidation = isValidation || false; // to set default value of parameter if isValidation === undefined ::: asjad 28-nov-2014
        if (!isValidation) {
            //self.loadDropDowns(true).done(function () {
            self.selectDropDowns(bAuto, myJson);
            //});
            self.loadLabels(bAuto, myJson);
            self.selectCheckBoxes(bAuto, myJson);
            self.loadTextBoxes(bAuto, myJson);
            self.loadImages(bAuto, myJson);
        }
        else {
            self.loadTextBoxes(bAuto, myJson);
            self.loadImages(bAuto, myJson);
        }

        self.find('[type=hidden],[type=text], textarea').each(function () {
            if (this.type == "text" && typeof $(this).get(0) != "undefined" && typeof this.id != "undefined" && typeof $('#' + this.id).get(0) != "undefined" && $.data($('#' + this.id).get(0), 'datepicker') != null) {
                if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                    this.value = utility.RemoveTimeFromDate(null, this.value);
                    $(this).datepicker("setDate", this.value);
                }
            } else {
                this.defaultValue = this.value;
            }

        });
        self.find('[type=checkbox], [type=radio]').each(function () { this.defaultChecked = this.checked; });
        self.find('select option').each(function () { this.defaultSelected = this.selected; });

        var html = self.html();
        if (html != null) {
            for (prop in myJson) {
                var regex = new RegExp("{@" + prop + "}", "g");
                html = html.replace(regex, myJson[prop]);
            }
        }
        // this.html(html);

        return self.promise();
    },
    bindMyJSONByName: function (bAuto, myJson, isValidation, self) {

        isValidation = isValidation || false; // to set default value of parameter if isValidation === undefined ::: asjad 28-nov-2014
        if (!isValidation) {
            //self.loadDropDowns(true).done(function () {
            self.selectDropDownsByName(bAuto, myJson);
            //});
            self.loadLabelsByName(bAuto, myJson);
            self.selectCheckBoxesByName(bAuto, myJson);
            self.loadTextBoxesByName(bAuto, myJson);
            self.loadImagesByName(bAuto, myJson);
        }
        else {
            self.loadTextBoxesByName(bAuto, myJson);
            self.loadImagesByName(bAuto, myJson);
        }

        self.find('[type=number],[type=hidden],[type=text], textarea').each(function () {
            if (this.type == "text" && typeof $(this).get(0) != "undefined" && typeof this.name != "undefined" && typeof $('#' + this.name).get(0) != "undefined" && $.data($('#' + this.name).get(0), 'datepicker') != null) {
                if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                    $(this).datepicker("setDate", this.value);
                }
            } else {
                this.defaultValue = this.value;
            }

        });
        self.find('[type=checkbox], [type=radio]').each(function () {
            this.defaultChecked = this.checked;
        });
        self.find('select option').each(function () {
            this.defaultSelected = this.selected;
        });

        var html = self.html();
        if (html != null) {
            for (prop in myJson) {
                var regex = new RegExp("{@" + prop + "}", "g");
                html = html.replace(regex, myJson[prop]);
            }
        }
        // this.html(html);

        return self.promise();
    },

    ConvertSpaceToSpecial: function (field) {
        var val = document.getElementById(field).value;
        val = val.replace(/\s/g, '#');
        document.getElementById(field).value = val;
    },

    ClearFormValidation: function (formId, IsResetValue) {
        //$(formId + " input, select, textarea").val('');
        if ($(formId).data('bootstrapValidator'))
            $(formId).data('bootstrapValidator').resetForm(true, IsResetValue);
        //$(formId + " input, select, textarea").each(function () {
        //    if (this.name && this.name != "")
        //    {
        //        $(formId).data('bootstrapValidator').enableFieldValidators(this.name, false);
        //        $(formId).data('bootstrapValidator').enableFieldValidators(this.name, true);
        //    }

        //});
    },

    RevalidateControl: function (Ctrl, CtrlForm) {
        if (Ctrl != null) {
            var objform = CtrlForm != null ? $(CtrlForm) : $(Ctrl).closest("form");
            if ($(objform) != null && $(objform).data('bootstrapValidator') != null && typeof $(objform).data('bootstrapValidator') != 'undefined' && $(Ctrl).attr('name') != null) {
                $(objform).bootstrapValidator('revalidateField', $(Ctrl).attr('name'));
            }
        }

    },

    RemoveTimeFromDate: function (controlId, value) {
        if (value != null && value != "") {
            var dateValue = value.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '');
            if (controlId != null) {
                $(controlId).val(dateValue);
            }
            else
                return $.datepicker.formatDate(globalAppdata['DateFormat'].replace('yy', ''), new Date(dateValue));
        }
        else if (value == "") {
            return "";
        }
        else
            return $.datepicker.formatDate(globalAppdata['DateFormat'].replace('yy', ''), new Date(value));
    },
    RemoveDateFromDateTime: function (controlId, value) {
        if (value != null && value != "") {
            var DateSplit = value.split('/');
            // fix of date 1/1/2016 09:09:08 PM, it was returning the exact date, now I'm returning the time only
            var dateValue = value.match(/[0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/)[0];
            if (controlId != null) {
                $(controlId).val(dateValue);
            }
            else
                return dateValue;
        }
        else
            return value;
    },

    FillProviderNPI: function (control, providerCtrl, npiCtrl) {
        var npi = $(control + ' ' + npiCtrl).val() != "" ? $(control + ' ' + npiCtrl).val() : null;
        var providerId = $(control + ' ' + providerCtrl).val() != "" ? $(control + ' ' + providerCtrl).val() : null;

        if (npi != null || providerId != null) {
            var data = "npi=" + npi + "&providerId=" + providerId;
            // serach parameter , class name, command name of class
            MDVisionService.defaultService(data, "PROVIDER_NPI_CONFIG", "PROVIDER_NPI").done(function (response) {
                if (response.status != false) {
                    var providerNpi = JSON.parse(response.ProviderNpi_JSON);
                    if (providerNpi.txtProvider != "") {
                        $(control + ' #txtProvider').val(providerNpi.txtProvider);
                        $(control + ' ' + providerCtrl).val(providerNpi.txtProviderId);
                    }
                    if (providerNpi.txtNpi != "")
                        $(control + ' ' + npiCtrl).val(providerNpi.txtNpi);
                }
            });
        }
    },

    FillCityState: function (control, zipctrl, cityctr, statectrl) {
        var zipcode = $(control + ' ' + zipctrl).val();
        var cityname = null;
        var statename = null;
        if (zipcode != "" && $(control + ' ' + zipctrl).inputmask().val().indexOf('_') == -1) {
            var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
            // serach parameter , class name, command name of class
            MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE").done(function (response) {
                if (response.status != false) {
                    var citystate = JSON.parse(response.CITYSTATE_JSON);
                    $(control + ' ' + cityctr).val(citystate.txtCity);
                    $(control + ' ' + statectrl).val(citystate.txtState);

                    var cityctrlName = $(control + ' ' + cityctr).attr("name");
                    var statectrlName = $(control + ' ' + statectrl).attr("name");
                    var zipctrlName = $(control + ' ' + zipctrl).attr("name");
                    try { $(control).bootstrapValidator('revalidateField', cityctrlName); } catch (e) { console.warn(e.message); }
                    try { $(control).bootstrapValidator('revalidateField', statectrlName); } catch (e) { console.warn(e.message); }
                    try { $(control).bootstrapValidator('revalidateField', zipctrlName); } catch (e) { console.warn(e.message); }
                    // PMS-5769 , adnan maqbool
                    try { $(control).bootstrapValidator('revalidateField', cityctrlName); } catch (e) { console.warn(e.message); }
                    try { $(control).bootstrapValidator('revalidateField', statectrlName); } catch (e) { console.warn(e.message); }
                    try { $(control).bootstrapValidator('revalidateField', zipctrlName); } catch (e) { console.warn(e.message); }
                    //
                }
                else {
                    ////Practice Management System PMS-816
                    ////patient Entire Module : When user entered invalid Zip code , States and City data disappeared
                    //$(control + ' ' + cityctr).val('');
                    //$(control + ' ' + statectrl).val('');
                }
            });
        }

    },

    FillDemographicCityState: function (control, zipctrl, cityctr, hfCity, statectrl, countryCtrl, hfCountry, caller, cityValue) {
        var zipcode = $(control + ' ' + zipctrl).val();
        var cityname = $(control + ' ' + cityctr).val();

        if (caller == 'city') {
            cityname = cityValue.split('-')[0];
            zipcode = cityValue.split('-')[1];
        }
        else if (caller == 'zip') {
            cityname = '';
        }
        var data = "zipcode=" + zipcode + "&cityname=" + cityname;
        MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE").done(function (response) {
            if (response.status != false) {
                var citystate = JSON.parse(response.CITYSTATE_JSON);
                if (caller == 'zip') {
                    $(control + ' ' + cityctr).val(citystate.txtCity);
                    $(control + ' ' + hfCity).val(citystate.txtZip);
                    utility.SetKendoAutoCompleteSourceforValidate($(control + ' ' + cityctr), $(control + ' ' + cityctr).val(), $(control + ' ' + hfCity), $(control + ' ' + hfCity).val());
                }
                else if (caller == 'city') {
                    $(control + ' ' + zipctrl).val(citystate.txtZip);
                }

                $(control + ' ' + statectrl).val(citystate.txtState);
                if (countryCtrl && hfCountry) {
                    $(control + ' ' + countryCtrl).val(utility.titleCase(citystate.ddlCountry));
                    //EMR-6760
                    if ($(control + ' ' + countryCtrl).css("border-color") == "rgb(255, 0, 0)") {
                        $(control + ' ' + countryCtrl).css("border-color", "green");
                    }

                    $(control + ' ' + hfCountry).val(citystate.hfCountry);
                    utility.SetKendoAutoCompleteSourceforValidate($(control + ' ' + countryCtrl), $(control + ' ' + countryCtrl).val(), $(control + ' ' + hfCountry), $(control + ' ' + hfCountry).val());
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                $(control + ' ' + zipctrl).val('');
                $(control + ' ' + statectrl).val('');
                $(control + ' ' + cityctr).val('');
                $(control + ' ' + hfCity).val('');
                if (countryCtrl && hfCountry) {
                    $(control + ' ' + countryCtrl).val('');
                    $(control + ' ' + hfCountry).val('');
                }
            }
        });


    },

    GetCustomPaging: function (pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp) {
        GetPaging(pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp);
    },

    GetCustomPagingAdmin: function (PagingPanelControlID, iTotalDisplayRecords, PagesToDisplay, ClassControlName, CurrentPage, RecordsPerPage, SearchFunc) {
        GetPagingAdmin(PagingPanelControlID, iTotalDisplayRecords, PagesToDisplay, ClassControlName, CurrentPage, RecordsPerPage, SearchFunc);
    },

    //Start PMS-2387
    GetPatientBannerInfo: function (ctrlhfPatientId, CtrlPatientAccountNo, CtrlPatientFullName, CtrlRefProviderId, CtrlRefProviderName, onSelectionCallBackFunction, fromCreateClaim) {
        // This function will set BannerInfo to specific controls and then triggers specific callback function
        var SelectedPatientId = $("#PatientProfile #hfPatientId").val();
        var SelectedPatientAccount = $("#PatientProfile #hfAccountNo").val();
        //On Banner Full NameOnly is of form "new Last Name,new First Name"
        var selectedPatientFullName = $("#PatientProfile #hfPatientFullNameOnly").val();//"new Last Name,new First Name"
        var selectedPatientRefProviderId = $("#PatientProfile #hfPatientRefProviderId").val();//"new Last Name,new First Name"
        var selectedPatientRefProviderName = $("#PatientProfile #hfPatientRefProviderName").val();//"new Last Name,new First Name"
        //$("#PatientProfile #hfPatientFullName").val();//"new Last Name,new First Name P006"
        //var selectedPatientFullName = "";
        //if (selectedPatFullName.lastIndexOf(" ") > -1) {
        //    selectedPatientFullName = selectedPatFullName.substring(0, selectedPatFullName.lastIndexOf(" "));
        //}
        if (fromCreateClaim) {
            Encounter_CreateClaim.PatientDOB = $("#PatientProfile #hfPatientDOB").val();
        }
        if (SelectedPatientId != null && SelectedPatientId != "") {
            if (ctrlhfPatientId != null) {
                $(ctrlhfPatientId).val(SelectedPatientId);
            }
            if (CtrlPatientAccountNo != null) {
                $(CtrlPatientAccountNo).val(SelectedPatientAccount);
            }
            if (CtrlPatientFullName != null) {
                $(CtrlPatientFullName).val(selectedPatientFullName);
            }
            if (CtrlRefProviderId != null) {
                $(CtrlRefProviderId).val(selectedPatientRefProviderId);
            }
            if (CtrlRefProviderName != null) {
                $(CtrlRefProviderName).val(selectedPatientRefProviderName);
            }
            // trigger on selecttionCall Back Function
            if (onSelectionCallBackFunction != null && typeof (onSelectionCallBackFunction) == 'function') {
                setTimeout(onSelectionCallBackFunction, 50);
            }
        }
    },
    //End PMS-2387
    GetUserArray: function (Username, IsAccountWithFullName, IsGetAll) {
        var AllUsers = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (Username != null && Username.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Patient_MessageCreate.LoadUsers(Username).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.UserCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.UserLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllUsers.push({ id: item.UserId, value: item.UserName });


                        });
                    }
                }

                dfd.resolve(AllUsers);
            });
        }
        else {
            dfd.resolve(AllUsers);
        }

        return dfd.promise();

    },

    GetUserArraywithPractice: function (Username, IsAccountWithFullName, IsGetAll) {
        var AllUsers = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (Username != null && Username.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Patient_MessageCreate.LoadUsers(Username).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.UserCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.UserLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllUsers.push({ id: item.UserId, value: item.FirstName + " " + item.LastName + " - " + item.PracticeName });


                        });
                    }
                }

                dfd.resolve(AllUsers);
            });
        }
        else {
            dfd.resolve(AllUsers);
        }

        return dfd.promise();

    },
    GetPatientArray: function (AccountNo, IsAccountWithFullName, IsGetAll) {
        var AllPatients = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (AccountNo != null && AccountNo.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            appointmentDetail.LoadActivePatients(AccountNo).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {
                            if (IsAccountWithFullName != null && IsAccountWithFullName == "1") {
                                AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName.trim(), AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName, SelfPay: item.SelfPay, PatientPortalStatus: item.PatientPortalStatus });
                            }
                            else {
                                AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName, SelfPay: item.SelfPay, PatientPortalStatus: item.PatientPortalStatus });
                            }

                        });
                    }
                }

                dfd.resolve(AllPatients);
            });
        }
        else {
            dfd.resolve(AllPatients);
        }

        return dfd.promise();

    },
    GetPatientArrayByName: function (AccountNo, IsAccountWithFullName, IsGetAll, IsAccountWithFullNameAndDOB) {
        var AllPatients = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (AccountNo != null && AccountNo.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            appointmentDetail.LoadActivePatientsByName(AccountNo).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {
                            if (IsAccountWithFullName != null && IsAccountWithFullName == "1") {
                                if (IsAccountWithFullNameAndDOB != null && IsAccountWithFullNameAndDOB == "1") {
                                    AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName + " - " + item.SDOB, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName, SelfPay: item.SelfPay, PatientPortalStatus: item.PatientPortalStatus });
                                } else {
                                    AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName, SelfPay: item.SelfPay, PatientPortalStatus: item.PatientPortalStatus });
                                }
                            }
                            else {
                                AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName, SelfPay: item.SelfPay, PatientPortalStatus: item.PatientPortalStatus });
                            }

                        });
                    }
                }

                dfd.resolve(AllPatients);
            });
        }
        else {
            dfd.resolve(AllPatients);
        }

        return dfd.promise();

    },

    GetGuarontorArray: function (name, IsGetAll) {
        var AllGuarontors = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            guarantorDetail.LoadActiveGuarantors(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.GurantorCount > 0) {
                        var GurantorLoadJSONData = JSON.parse(responseData.GurantorLoad_JSON);
                        $.each(GurantorLoadJSONData, function (i, item) {

                            AllGuarontors.push({ id: item.GuarantorId, value: item.FullName });


                        });
                    }
                }

                dfd.resolve(AllGuarontors);
            });
        }
        else {
            dfd.resolve(AllGuarontors);
        }

        return dfd.promise();

    },
    ValidateAutoComplete: function (ev, hfId, optional, IsDash, IsId, CtrlFullName) {
        var link = $(ev).parent().prev('a');

        if ($(ev).length > 0 && $(ev).val().toLowerCase() == '- select -') {
            $("#" + hfId).val('');
            if ($(ev).attr('customName') != null)
                utility.DisplayMessages($(ev).attr('customName') + " is not Valid", 2);
            else
                utility.DisplayMessages($(ev).attr('name') + " is not Valid", 2);
            $(ev).val("");
            $(link).hide();
            $(link).prev().show();
            return false;
        }

        if (!optional && $(ev).val() != "") {
            return utility.CheckAutoComplete(ev, link, hfId, IsDash, IsId, CtrlFullName);
        }
        else {
            if ($(ev).val() != "") {
                return utility.CheckAutoComplete(ev, link, hfId, IsDash, IsId, CtrlFullName);
            }
            else {
                $("#" + hfId).val('');
                if (CtrlFullName != null) {
                    $(CtrlFullName).val("");
                }
                $(link).hide();
                $(link).prev().show();
            }
        }

    },

    ValidateAutoCompletePatientAccount: function (ev, hfId, optional, IsDash, IsId, CtrlFullName) {
        setTimeout(function () {
            var link = $(ev).parent().prev('a');

            if ($(ev).length > 0 && $(ev).val().toLowerCase() == '- select -') {
                $("#" + hfId).val('');
                if ($(ev).attr('customName') != null)
                    utility.DisplayMessages($(ev).attr('customName') + " is not Valid", 2);
                else
                    utility.DisplayMessages($(ev).attr('name') + " is not Valid", 2);
                $(ev).val("");
                $(link).hide();
                $(link).prev().show();
                return false;
            }

            if (!optional && $(ev).val() != "") {
                return utility.CheckAutoComplete(ev, link, hfId, IsDash, IsId, CtrlFullName);
            }
            else {
                if ($(ev).val() != "") {
                    return utility.CheckAutoComplete(ev, link, hfId, IsDash, IsId, CtrlFullName);
                }
                else {
                    $("#" + hfId).val('');
                    if (CtrlFullName != null) {
                        $(CtrlFullName).val("");
                    }
                    $(link).hide();
                    $(link).prev().show();
                }
            }
        }, 1000);
    },

    CheckAutoComplete: function (ev, link, hfId, IsDash, IsId, CtrlFullName) {
        if ($(ev).data('ui-autocomplete') != undefined) {
            var sourceArr = $(ev).autocomplete("option", "source");

            if (typeof (sourceArr) == 'function') {
                var obj = { term: "" }
                sourceArr(obj, function (res) {
                    sourceArr = res;
                });

            }
            var haveObject = sourceArr.filter(function (obj) {
                if (IsDash && IsDash == 1) {
                    var arrObjValues = obj.value.split("-");
                    if (arrObjValues && $(ev).val().toLowerCase() == arrObjValues[0].trim().toLowerCase()) {//.indexOf($(ev).val().toLowerCase()) > -1) {
                        return true;
                    }
                    else {
                        return false;
                    }
                    //return obj.value.toLowerCase() == $(ev).val().toLowerCase();
                }
                else {
                    //for the time being i am doing it only for patient insurance to avoid ripples on other screens.
                    //it should match the value instead of text. text can be duplicate .
                    var IsValid = "";
                    if (hfId == "frmPatientInsurance #hfInsurancePlan" || hfId == "pnlPatientInsurance #hfInsurancePlan" || hfId == "pnldemographicDetail #hfInsurancePlan") {
                        IsValid = obj.id == $("#" + hfId).val();
                    }
                    else if (obj.value) {
                        IsValid = obj.value.toLowerCase() == $(ev).val().toLowerCase();
                    } else {
                        IsValid = false;
                    }
                    return IsValid;
                }

            });
            if (haveObject.length == 0) {
                $("#" + hfId).val('');
                if (CtrlFullName != null) {
                    $(CtrlFullName).val("");
                }
                if ($(ev).attr('customName') != null) {
                    if (Patient_MessageCreate.params.MessageType == "Patient") {
                        utility.DisplayMessages("Cannot send message to non-existent user!", 3);
                        //$('#' + Patient_MessageCreate.params["PanelID"] + ' #txtAttatchPatient').val("");
                    } else if (Patient_MessageCreate.params.MessageType == "Practice") {
                        utility.DisplayMessages($(ev).attr('customName') + " is not Valid", 2);
                    }
                    else {
                        utility.DisplayMessages($(ev).attr('customName') + " is not Valid", 2);
                    }

                } else {
                    utility.DisplayMessages($(ev).attr('name') + " is not Valid", 2);
                }
                $(ev).val("");
                $(link).hide();
                $(link).prev().show();
                return false;
            }
            else {
                if (IsId == false)
                    $("#" + hfId).val(haveObject[0].AccountNumber);
                else
                    $("#" + hfId).val(haveObject[0].id);

                if (CtrlFullName != null) {
                    //Only for Patient Full Name, otherwise please specify
                    if (haveObject[0].FullName != null) {
                        $(CtrlFullName).val(haveObject[0].FullName);
                    }
                }
                $(link).show();
                $(link).prev().hide();
                return true;
            }
        }
    },

    ValidateAutoCompletePatientName: function (ev, hfId, optional, IsDash, IsId, CtrlFullName) {
        var link = $(ev).parent().prev('a');

        if ($(ev).length > 0 && $(ev).val().toLowerCase() == '- select -') {
            $("#" + hfId).val('');
            if ($(ev).attr('customName') != null)
                utility.DisplayMessages($(ev).attr('customName') + " is not Valid", 2);
            else
                utility.DisplayMessages($(ev).attr('name') + " is not Valid", 2);
            $(ev).val("");
            $(link).hide();
            $(link).prev().show();
            return false;
        }

        if (!optional && $(ev).val() != "") {
            return utility.CheckAutoCompletePatientName(ev, link, hfId, IsDash, IsId, CtrlFullName);
        }
        else {
            if ($(ev).val() != "") {
                return utility.CheckAutoCompletePatientName(ev, link, hfId, IsDash, IsId, CtrlFullName);
            }
            else {
                $("#" + hfId).val('');
                if (CtrlFullName != null) {
                    $(CtrlFullName).val("");
                }
                $(link).hide();
                $(link).prev().show();
            }
        }

    },

    CheckAutoCompletePatientName: function (ev, link, hfId, IsDash, IsId, CtrlFullName) {
        var sourceArr = $(ev).autocomplete("option", "source");
        var haveObject = sourceArr.filter(function (obj) {
            if (IsDash && IsDash == 1) {
                var arrObjValues = obj.value.split("-");
                if (arrObjValues && $(ev).val().trim().toLowerCase() == arrObjValues[1].trim().toLowerCase()) {//.indexOf($(ev).val().toLowerCase()) > -1) {
                    return true;
                }
                else {
                    return false;
                }
                //return obj.value.toLowerCase() == $(ev).val().toLowerCase();
            }
            else {
                var IsValid = obj.value.toLowerCase() == $(ev).val().toLowerCase();
                return IsValid;
            }

        });
        if (haveObject.length == 0) {
            $("#" + hfId).val('');
            if (CtrlFullName != null) {
                $(CtrlFullName).val("");
            }
            if ($(ev).attr('customName') != null)
                utility.DisplayMessages($(ev).attr('customName') + " is not Valid", 2);
            else
                utility.DisplayMessages($(ev).attr('name') + " is not Valid", 2);
            $(ev).val("");
            $(link).hide();
            $(link).prev().show();
            return false;
        }
        else {
            if (IsId == false)
                $("#" + hfId).val(haveObject[0].AccountNumber);
            else
                $("#" + hfId).val(haveObject[0].id);

            if (CtrlFullName != null) {
                //Only for Patient Full Name, otherwise please specify
                if (haveObject[0].FullName != null) {
                    $(CtrlFullName).val(haveObject[0].FullName);
                }
            }
            $(link).show();
            $(link).prev().hide();
            return true;
        }
    },

    BindAutoCompleteText: function (Crtl, controlName, cammandAction, hiddenCrtl, entityID, isDescription, minTextLength, iscode) {

        utility.KeyupdelayForModifiers(function () {

            //"COMMON_IMO_CODE"
            //"GET_IMO_CPTCODE"
            var RequiredLength = 2;
            if (cammandAction != undefined && cammandAction != null && cammandAction == "GET_MODIFIER_CODE") {
                RequiredLength = 1;
            }
            if (minTextLength != null && minTextLength != "" && minTextLength > -1) {
                RequiredLength = minTextLength;
            }
            var text = "";
            if (Crtl)
                text = $(Crtl).val();
            //if (text.length > 2) {
            BackgroundLoaderShow(false);
            var AllData = [];
            if (text != null && text.length >= RequiredLength) {
                BackgroundLoaderShow(true);
                var data = "text=" + text + "&entityID=" + entityID + "&iscode=" + iscode;
                MDVisionService.defaultService(data, controlName, cammandAction).done(function (result) {
                    AllData = [];
                    $.each(result, function (j, item) {
                        if (item.Name.toLowerCase() != "- select -")
                            AllData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                    });

                    $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                        _create: function () {
                            this._super();
                            this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                        },
                        _renderMenu: function (ul, items) {
                            var that = this;
                            ul.append("");
                            $.each(items, function (index, item) {
                                var li;
                                li = that._renderItemData(ul, item);
                            });
                        }
                    });

                    BackgroundLoaderShow(false);
                    $(Crtl).autocomplete({
                        autoFocus: true,
                        //source: AllCode, // pass an array (without a comma)
                        source: AllData,
                        select: function (event, ui) {
                            BackgroundLoaderShow(false);
                            var TABKEY = 9;
                            this.value = ui.item.value;

                            if (event.keyCode == TABKEY) {
                                event.preventDefault();
                                this.value = this.value;
                            }
                            // add the selected id
                            //$(Crtl).
                            setTimeout(function () {
                                $(Crtl).val(ui.item.RefValue);

                                if (typeof hiddenCrtl != "undefined")
                                    if (hiddenCrtl != null) {
                                        if (isDescription) {
                                            var description = ui.item.value.split("-");
                                            if (description.length > 1)
                                                setTimeout(function () { $(hiddenCrtl).val(description[1]).focus(); }, 100);
                                            else
                                                setTimeout(function () { $(hiddenCrtl).val(description[0]).focus(); }, 100);
                                        }
                                        else {
                                            setTimeout(function () { $(hiddenCrtl).val(ui.item.id).focus(); }, 100);
                                        }
                                    }


                            }, 10);
                            //$(Crtl).text(ui.item.RefValue);
                        }
                    });
                    try {
                        if ($(Crtl).closest("#tblBillingInformation").attr("id") == "tblBillingInformation")
                            $(Crtl).autocomplete("option", "appendTo", "#dgvBillVisitCharge_wrapper");

                        if ($(Crtl).closest("#dgvVisitCharge").attr("id") == "dgvVisitCharge") {
                            $(Crtl).autocomplete("option", "appendTo", "#dgvVisitCharge_wrapper");
                        }

                    } catch (ex) {
                        console.log(ex);
                    }
                    $(Crtl).autocomplete("search");

                });
            }

        });
    },

    //Reason: Function to bind kendo autocomplete
    BindKendoAutoComplete: function (Ctrl, minLength, dataTextField, filter, dataSource, func, hfCtrl, onSelect, onChange, onClose, onOpen) {
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
        var valid = false;
        if (onSelect) {
            var OnSelectTemp = onSelect;
            onSelect = function (e) {
                OnSelectTemp.call(this, this.dataItem(e.item.index()));
            };
        }
        if (onChange) {
            var OnChangeTemp = onChange;
            onChange = function () {
                OnChange.call(this);
                OnChangeTemp.call(this, valid);
            };
        }
        else {
            onChange = function () {
                OnChange.call(this);
            }
        }
        if (func) {
            $(Ctrl).kendoAutoComplete({
                dataTextField: dataTextField,
                filter: filter,
                minLength: minLength,
                select: onSelect,
                change: onChange,
                close: onClose,
                open: onOpen,
                dataSource: {
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            func().done(function (response) {
                                e.success(response);
                            });
                        },
                    }
                },

            });
        }
        else {
            $(Ctrl).kendoAutoComplete({
                dataTextField: dataTextField,
                filter: filter,
                minLength: minLength,
                select: onSelect,
                change: onChange,
                close: onClose,
                open: onOpen,
                dataSource: dataSource,
            });
        }
        var OnChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var options = this.dataSource.options.data;
            var data = this.dataSource.data();
            if (options && options.length > data.length)
                data = options;
            var haveObject = data.filter(function (obj) {
                var value = eval('obj.' + dataTextField);
                if (value)
                    if (value.toLowerCase() == $(Ctrl).val().toLowerCase()) {
                        id_ = obj.id;
                        value_ = value;
                        return true;
                    }
                    else { return false; }
            });
            if (haveObject.length > 0) {
                valid = true;
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }
            else {
                valid = false;

                if (hfCtrl)
                    $(hfCtrl).val('');
                if ($(Ctrl).val() != "") {
                    if ($(Ctrl).attr('customName') != null) {
                        if (Patient_MessageCreate.params.MessageType == "Patient") {
                            utility.DisplayMessages("Cannot send message to non-existent user!", 3);
                        }
                        else {
                            utility.DisplayMessages($(Ctrl).attr('customName') + " is not Valid", 2);
                        }

                    } else {
                        utility.DisplayMessages($(Ctrl).attr('name') + " is not Valid", 2);
                    }
                }

                this.value('');

                $(link).hide();
                $(link).prev().show();
            }
        };
    },

    //Reason: Function to set value in kendo autocomplete data source
    SetKendoAutoCompleteSourceforValidate: function ($Ctrl, Name, $hfCtrl, Id, dataTextField) {
        if ($Ctrl && $Ctrl.data('kendoAutoComplete') && Name && Id) {
            var widget = $Ctrl.getKendoAutoComplete();
            var dataSource = widget.dataSource;
            var options = dataSource.options.data;
            var data = dataSource.data();
            var obj_ = {};
            $Ctrl.val(Name);
            if ($hfCtrl)
                $hfCtrl.val(Id);
            if (!dataTextField)
                dataTextField = "value";
            if (options && options.length > data.length)
                data = options;
            obj_.id = Id;
            obj_[dataTextField] = Name;
            if (data.length > 0) {
                arr = $.grep(data, function (item) {
                    return item.id == Id && item[dataTextField] == Name;
                });
                if (arr.length <= 0)
                    dataSource.add(obj_);
            }
            else { dataSource.add(obj_); }
        }
    },

    GetISShowICD10: function () {

        utility.FillIsShowICD10(globalAppdata.AppUserName).done(function (response) {
            if (response.status == true) {
                var entityJSON = JSON.parse(response.ISShowICD10_JSON);
                if (entityJSON.hfIsShowICD10 == "True") {
                    utility.IsShowICD10 = true;
                }
                else {
                    utility.IsShowICD10 = false;
                }
            }


        });

    },
    FillIsShowICD10: function (userName) {
        var data = "userName=" + userName;
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "FILL_SHOW_ICD10");
    },
    BindIMOAutoCompleteText: function (Crtl, controlName, cammandAction, hiddenCrtl, entityID, isDescription, minTextLength, iscode, isIcd9, ParentControl, ContainerCtrl, isMDVision, AccessPanel, customFormToolsParentId) {
        //Start//05-05-2016//Ahmad Raza//implimented privileges for ICD Search
        utility.Keyupdelay(function () {
            AppPrivileges.GetFormPrivileges("ICD", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var requiredLength = 2;
                    if (minTextLength != null && minTextLength != "" && minTextLength > -1) {
                        requiredLength = minTextLength;
                    }
                    var text = "";
                    var hfSubColumnPlusChargeId = "";
                    if (Crtl) {
                        text = $(Crtl).val();
                        hfSubColumnPlusChargeId = $(Crtl).attr('id').split('txtICD')[1];
                    }

                    BackgroundLoaderShow(false);
                    var allData = [];
                    if (text != null && text.length >= requiredLength) {
                        BackgroundLoaderShow(true);

                        utility.GetISShowICD10();

                        if (entityID === undefined || entityID == null)
                            entityID = globalAppdata["SeletedEntityId"];

                        var data = "text=" + text + "&entityID=" + entityID + "&iscode=" + iscode + "&isMDVision=" + isMDVision + "&module=" + ParentControl;
                        var LocalOrIMO = "";
                        MDVisionService.defaultService(data, controlName, cammandAction).done(function (result) {
                            allData = [];
                            //Start on 19/4/16 BY M Ahmad Imran
                            var IsSnomedTypeSearch = false;
                            var sctString = "";
                            if ($(Crtl).val().substring(0, 3).toLowerCase() == "sct") {
                                //sctString=$(Crtl).val().substring(0, 3);
                                IsSnomedTypeSearch = true;
                                //$(Crtl).val($(Crtl).val().substring(3, ($(Crtl).val()).length))
                                sctString = $(Crtl).val();
                                $(Crtl).val(" ");



                            }
                            //End on 19/4/16 BY M Ahmad Imran
                            $.each(result, function (j, item) {
                                item.Name = utility.decodeHtml(item.Name);
                                if (item.Name.toLowerCase() != "- select -") {

                                    // Whether AutoComplete is CPT
                                    if (iscode == "CPT") {

                                        if (item.RefValue) {
                                            var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                            var _ConcatinatedString = item.Name;

                                            // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                            if (_ConcatinatedString.indexOf("!") >= 0) {

                                                LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                                CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                                ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                                SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                                _LexiCode = LexiCode;

                                                _CPT = CPT.split("+")[0];
                                                _CPTDescription = CPT.split("+")[1];
                                                _ICD = ICD.split("+")[0];
                                                _ICDDescription = ICD.split("+")[1];
                                                _SNOMED = SNOMED.split("+")[0];
                                                _SNOMEDDescription = SNOMED.split("+")[1];
                                            }
                                            else {
                                                CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                                _CPT = CPT.split("-")[0].trim();
                                                _CPTDescription = CPT.split("-")[1].trim();
                                            }

                                            var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                            // In IMO case Build Four Column Header, Else Two Columns

                                            _CPTDescription = _CPTDescription.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');

                                            //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                            if (ParentControl == 'PQRS_ICDCPTCodes') {
                                                var CPTMatched = false;
                                                $.each(PQRS_ICDCPTCodes.arrCPTs, function (i, item) {

                                                    if (_CPT == item.Code) {
                                                        CPTMatched = true;
                                                    }

                                                });
                                                if (CPTMatched == false) {
                                                    return true;
                                                }
                                            }
                                            //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes

                                            if (isIMO == "imo") {

                                                var duMulti = _LexiCode + "*" + CPT + "$" + ICD + "~" + SNOMED;
                                                _ICD = _ICD != "" ? _ICD : "NoICD";
                                                _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                                                //allData.push({ id: item.Value, value: _CPT + " - " + _ICD + " - " + _SNOMED + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                //Start on 19/4/16 BY M Ahmad Imran

                                                if (!IsSnomedTypeSearch) {
                                                    if (ParentControl == 'Clinical_SurgicalHx' || ParentControl == "Favorite_SurgicalHistoryDetail") {
                                                        allData.push({ id: item.Value, value: _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                    else {

                                                        allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                }
                                                else {
                                                    if (_CPT == "") {
                                                        allData.push({ id: item.Value, value: _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                    else {
                                                        allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                }
                                                //End on 19/4/16 BY M Ahmad Imran

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);
                                                //var icd9Length = ((spaceHandler_.split('-')[1].trim().length + 3) - 3);
                                                //var snomedLength = ((spaceHandler_.split('-')[2].trim().length + 3) - 6);

                                                var cptSpaces = "", icd9Spaces = "", snomedSpaces = "";

                                                for (var i = 0; i < cptLength; i++) { cptSpaces += '&nbsp;'; }
                                                //for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                                //snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                                //for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {

                                                            //ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>ICD" + icd9Spaces + "</label><label>SNOMED" + snomedSpaces + "</label><label>DESCRIPTION</label></li>");
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");

                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                                LocalOrIMO = "imo";
                                            }
                                            else {
                                                var duMulti = "*" + CPT + "$" + "~";
                                                allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);

                                                var cptSpaces = "";
                                                for (var i = 0; i < cptLength; i++)
                                                    cptSpaces += '&nbsp;';

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");
                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                            }
                                            LocalOrIMO = "MDVision";
                                        }
                                        //Start on 19/4/16 BY M Ahmad Imran
                                        if (item.RefValue == "") {
                                            var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                            var _ConcatinatedString = item.Name;

                                            // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                            if (_ConcatinatedString.indexOf("!") >= 0) {

                                                LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                                CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                                ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                                SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                                _LexiCode = LexiCode;

                                                _CPT = CPT.split("+")[0];
                                                _CPTDescription = CPT.split("+")[1];
                                                _ICD = ICD.split("+")[0];
                                                _ICDDescription = ICD.split("+")[1];
                                                _SNOMED = SNOMED.split("+")[0];
                                                _SNOMEDDescription = SNOMED.split("+")[1];
                                            }
                                            else {
                                                CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                                _CPT = CPT.split("-")[0].trim();
                                                _CPTDescription = CPT.split("-")[1].trim();
                                            }

                                            var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                            // In IMO case Build Four Column Header, Else Two Columns

                                            //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                            if (ParentControl == 'PQRS_ICDCPTCodes') {
                                                var CPTMatched = false;
                                                $.each(PQRS_ICDCPTCodes.arrCPTs, function (i, item) {

                                                    if (_CPT == item.Code) {
                                                        CPTMatched = true;
                                                    }

                                                });
                                                if (CPTMatched == false) {
                                                    return true;
                                                }
                                            }
                                            //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                            if (isIMO == "imo") {

                                                var duMulti = _LexiCode + "*" + CPT + "$" + ICD + "~" + SNOMED;
                                                _ICD = _ICD != "" ? _ICD : "NoICD";
                                                _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                                                //allData.push({ id: item.Value, value: _CPT + " - " + _ICD + " - " + _SNOMED + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                if (!IsSnomedTypeSearch) {
                                                    if (ParentControl == 'Clinical_SurgicalHx' || ParentControl == 'Favorite_SurgicalHistoryDetail') {
                                                        allData.push({ id: item.Value, value: _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                    else {

                                                        allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                    }

                                                }
                                                else {
                                                    if (_CPT == "") {
                                                        allData.push({ id: item.Value, value: _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                    else {
                                                        allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                }

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);
                                                //var icd9Length = ((spaceHandler_.split('-')[1].trim().length + 3) - 3);
                                                //var snomedLength = ((spaceHandler_.split('-')[2].trim().length + 3) - 6);

                                                var cptSpaces = "", icd9Spaces = "", snomedSpaces = "";

                                                for (var i = 0; i < cptLength; i++) { cptSpaces += '&nbsp;'; }
                                                //for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                                //snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                                //for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {

                                                            //ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>ICD" + icd9Spaces + "</label><label>SNOMED" + snomedSpaces + "</label><label>DESCRIPTION</label></li>");
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");

                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                                LocalOrIMO = "imo";
                                            }
                                            else {
                                                var duMulti = "*" + CPT + "$" + "~";
                                                allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);

                                                var cptSpaces = "";
                                                for (var i = 0; i < cptLength; i++)
                                                    cptSpaces += '&nbsp;';

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");
                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                            }
                                            LocalOrIMO = "MDVision";
                                        }
                                        //End on 19/4/16 BY M Ahmad Imran
                                    }
                                        // ICD
                                    else if (iscode == "ICD") {

                                        var LexiCode = "", ICD9 = "", ICD10 = "", SNOMED = "", _ICD9 = "", _ICD9Description = "", _ICD10 = "",
                                            _ICD10Description = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                        var _ConcatinatedString = item.Name;

                                        LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                        ICD9 = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                        ICD10 = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                        SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                        _LexiCode = LexiCode;

                                        _ICD9 = ICD9.split("+")[0];
                                        _ICD9Description = ICD9.split("+")[1];
                                        _ICD10 = ICD10.split("+")[0];
                                        _ICD10Description = ICD10.split("+")[1];
                                        _SNOMED = SNOMED.split("+")[0];
                                        _SNOMEDDescription = SNOMED.split("+")[1];

                                        var duMulti = _LexiCode + "*" + ICD9 + "$" + ICD10 + "~" + SNOMED;

                                        var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                        _ICD10 = _ICD10 != "" ? _ICD10 : "NoICD10";
                                        _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                                        //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                        if (ParentControl == 'PQRS_ICDCPTCodes') {
                                            var ICDMatched = false;
                                            $.each(PQRS_ICDCPTCodes.arrICDs, function (i, item) {

                                                if (_ICD10 == item.Code) {
                                                    ICDMatched = true;
                                                }

                                            });
                                            if (ICDMatched == false) {
                                                return true;
                                            }
                                        }
                                        //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                        if (isIMO == "imo") {
                                            if (ParentControl == "Clinical_FamilyHx" || ParentControl == "Clinical_MedicalHx" || ParentControl == "Clinical_HospitalizationHx" || ParentControl == "Favorite_FamilyHistoryDetail" || ParentControl == "Favorite_MedicalHistoryDetail") {

                                                allData.push({ id: item.Value, value: item.RefValue + " - " + _ICD10Description, RefValue: item.RefValue, RefName: duMulti });


                                            }

                                                //else {


                                                //        allData.push({ id: item.Value, value: _ICD10 + " - " + _ICD10Description, RefValue: item.RefValue, RefName: duMulti });
                                                //}

                                            else {

                                                if (utility.IsShowICD10) {
                                                    allData.push({ id: item.Value, value: _ICD10 + " - " + _ICD10Description, RefValue: item.RefValue, RefName: duMulti });
                                                }

                                                else {
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                }
                                            }

                                            //else {
                                            //    if (isIcd9)
                                            //        allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                            //    else if (!isIcd9)
                                            //        allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                            //    else {
                                            //        allData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                                            //    }
                                            //}


                                            LocalOrIMO = "imo";
                                        }
                                        else {
                                            if (ParentControl == "Clinical_FamilyHx" || ParentControl == "Clinical_MedicalHx" || ParentControl == "Clinical_HospitalizationHx" || ParentControl == "Favorite_FamilyHistoryDetail" || ParentControl == "Favorite_MedicalHistoryDetail") {
                                                allData.push({ id: item.Value, value: _ICD10Description, RefValue: item.RefValue, RefName: duMulti });
                                            } else {
                                                // if (ParentControl == "Clinical_ProblemLists" || ParentControl == "Clinical_Cognitive" || ParentControl == "BillingInformation") {
                                                allData.push({ id: item.Value, value: _ICD10 + " - " + _ICD10Description, RefValue: item.RefValue, RefName: duMulti });
                                                //}
                                                //else {
                                                //    if (isIcd9)
                                                //        allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                //    else if (!isIcd9)
                                                //        allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                //    else {
                                                //        allData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                                                //    }
                                                //}
                                            }
                                            LocalOrIMO = "MDVision";
                                        }
                                        if (ParentControl != "Clinical_FamilyHx" && ParentControl != "Clinical_MedicalHx" && ParentControl != "Clinical_HospitalizationHx" && ParentControl != "Favorite_FamilyHistoryDetail" && ParentControl != "Favorite_MedicalHistoryDetail") {
                                            //   if (ParentControl == "Clinical_ProblemLists" || ParentControl == "Clinical_Cognitive" || ParentControl == "BillingInformation") {
                                            var spaceHandler = allData[0].value;
                                            var icd9Length = ((spaceHandler.split('-')[0].trim().length + 3) - 4);
                                            var icd10Length = ((spaceHandler.split('-')[1].trim().length + 3) - 5);
                                            // var snomedLength = ((spaceHandler.split('-')[2].trim().length + 3) - 6);

                                            var icd9Spaces = "", icd10Spaces = "", snomedSpaces = "";

                                            for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                            icd10Length = icd10Length <= -1 ? 8 : icd10Length;
                                            for (var i = 0; i < icd10Length ; i++) { icd10Spaces += '&nbsp;'; }
                                            //    snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                            //    for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }
                                            //} else {
                                            //    var spaceHandler = allData[0].value;
                                            //    var icd9Length = ((spaceHandler.split('-')[0].trim().length + 3) - 4);
                                            //    var icd10Length = ((spaceHandler.split('-')[1].trim().length + 3) - 5);
                                            //    var snomedLength = ((spaceHandler.split('-')[2].trim().length + 3) - 6);

                                            //    var icd9Spaces = "", icd10Spaces = "", snomedSpaces = "";

                                            //    for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                            //    icd10Length = icd10Length <= -1 ? 8 : icd10Length;
                                            //    for (var i = 0; i < icd10Length ; i++) { icd10Spaces += '&nbsp;'; }
                                            //    snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                            //    for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }
                                            //}
                                        }

                                        $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                            _create: function () {
                                                this._super();
                                                this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                            },
                                            _renderMenu: function (ul, items) {
                                                var that = this;
                                                if (that.element.context.id == $(Crtl).attr('id')) {
                                                    // if (ParentControl == "Clinical_ProblemLists" || ParentControl == "Clinical_Cognitive" || ParentControl == "BillingInformation") {
                                                    ul.append("<li class='ui-autocomplete-category'><label>&nbsp; ICD9" + icd9Spaces + "</label><label>&nbsp; ICD10" + icd10Spaces + "</label><label>DESCRIPTION</label></li>");
                                                    // } else {
                                                    //      ul.append("<li class='ui-autocomplete-category'><label>&nbsp; ICD9" + icd9Spaces + "</label><label>&nbsp; ICD10" + icd10Spaces + "</label><label>SNOMED" + snomedSpaces + "</label><label>DESCRIPTION</label></li>");
                                                    //  }
                                                } $.each(items, function (index, item) {
                                                    var li;
                                                    li = that._renderItemData(ul, item);
                                                });

                                            }
                                        });
                                    }


                                    else {

                                    }
                                }
                            });

                            BackgroundLoaderShow(false);
                            $(Crtl).autocomplete({
                                autoFocus: true,
                                //source: AllCode, // pass an array (without a comma)
                                source: allData,
                                minLength: 0,
                                select: function (event, ui) {
                                    BackgroundLoaderShow(false);
                                    var TABKEY = 9;
                                    this.value = ui.item.value;

                                    if (event.keyCode == TABKEY) {
                                        event.preventDefault();
                                        this.value = this.value;
                                    }
                                    // add the selected id
                                    //$(Crtl).
                                    setTimeout(function () {

                                        if (ParentControl != "BillingInformation") {
                                            if (ui.item != undefined)
                                                $(Crtl).val(ui.item.RefValue);
                                            else
                                                $(Crtl).val("");
                                        }


                                        if (typeof hiddenCrtl != "undefined")
                                            if (hiddenCrtl != null) {
                                                if (ui.item != undefined) {
                                                    if (isDescription) {

                                                        if (iscode == "CPT") {
                                                            if (ui.item != undefined)
                                                                cptcodeDetail.SetControlValues(ui.item.value, ui.item.RefName, ParentControl, ContainerCtrl, hiddenCrtl, null, customFormToolsParentId, ui.item.id);

                                                            else
                                                                ;
                                                        }
                                                        else {
                                                            //if (isIcd9)
                                                            //    ICDDetail.SetControlValues(ui.item.RefName);
                                                            //else
                                                            if (ui.item != undefined)
                                                                SupperBillDetail.OpenIMODetail(ui.item.RefName, ui.item.value, ParentControl, hfSubColumnPlusChargeId, LocalOrIMO, text, AccessPanel, customFormToolsParentId);
                                                            else
                                                                ;
                                                        }
                                                        setTimeout(function () {
                                                            // adnan maqbool, PMS-4925
                                                            if (ParentControl == "SupperBillDetail") {
                                                                $(hiddenCrtl).val((ui.item.value).split(' - ')[1]).focus();
                                                            }
                                                            else if ($(hiddenCrtl).attr('id') != 'txtProblems' && $(hiddenCrtl).attr('id') != 'txtProceduresCustomForm' && (ParentControl != 'Clinical_FamilyHx' && $(hiddenCrtl).attr('id') != 'txtDisease' && $(hiddenCrtl).attr('id') != 'txtProcedures')) {
                                                                $(hiddenCrtl).val(ui.item.value).focus();
                                                            }
                                                            //
                                                        }, 30);
                                                        if (iscode == "CPT" && ParentControl == "BillingInformation")
                                                            BillingInformation.AddNewChargeRowWrapper();
                                                    }
                                                    else {
                                                        setTimeout(function () { $(hiddenCrtl).val(ui.item.id).focus(); }, 100);
                                                    }
                                                }
                                            }
                                    }, 10);
                                    //$(Crtl).text(ui.item.RefValue);
                                }
                            });
                            try {
                                if ($(Crtl).closest("#tblBillingInformation").attr("id") == "tblBillingInformation")
                                    $(Crtl).autocomplete("option", "appendTo", "#dgvBillVisitCharge_wrapper");

                                if ($(Crtl).closest("#dgvVisitCharge").attr("id") == "dgvVisitCharge") {
                                    $(Crtl).autocomplete("option", "appendTo", "#dgvVisitCharge_wrapper");
                                }

                            } catch (ex) {
                                console.log(ex);
                            }
                            $(Crtl).autocomplete("search", "");


                            //Start on 19/4/16 BY M Ahmad Imran
                            if (IsSnomedTypeSearch) {
                                //$(Crtl).val(sctString + $(Crtl).val());
                                $(Crtl).val(sctString);
                                IsSnomedTypeSearch = false;
                            }
                            //End on 19/4/16 BY M Ahmad Imran

                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        });
        //End//05-05-2016//Ahmad Raza//implimented privileges for ICD Search
    },

    BindCPTCompleteText: function (Crtl, controlName, cammandAction, hiddenCrtl, entityID, isDescription, minTextLength, iscode, isIcd9, ParentControl, ContainerCtrl, isMDVision, isComplaint) {
        //Start//05-05-2016//Ahmad Raza//implimented privileges for ICD Search
        utility.Keyupdelay(function () {
            AppPrivileges.GetFormPrivileges("ICD", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var requiredLength = 2;
                    if (minTextLength != null && minTextLength != "" && minTextLength > -1) {
                        requiredLength = minTextLength;
                    }
                    var text = "";
                    var hfSubColumnPlusChargeId = "";
                    if (Crtl) {
                        text = $(Crtl).val();
                        hfSubColumnPlusChargeId = $(Crtl).attr('id').split('txtICD')[1];
                    }

                    BackgroundLoaderShow(false);
                    var allData = [];
                    if (text != null && text.length >= requiredLength) {
                        BackgroundLoaderShow(true);

                        if (entityID === undefined || entityID == null)
                            entityID = globalAppdata["SeletedEntityId"];

                        var data = "text=" + text + "&entityID=" + entityID + "&iscode=" + iscode + "&isMDVision=" + isMDVision;
                        var LocalOrIMO = "";
                        MDVisionService.defaultService(data, controlName, cammandAction).done(function (result) {
                            allData = [];
                            //Start on 19/4/16 BY M Ahmad Imran
                            var IsSnomedTypeSearch = false;
                            var sctString = "";
                            if ($(Crtl).val().substring(0, 3).toLowerCase() == "sct") {
                                //sctString=$(Crtl).val().substring(0, 3);
                                IsSnomedTypeSearch = true;
                                //$(Crtl).val($(Crtl).val().substring(3, ($(Crtl).val()).length))
                                sctString = $(Crtl).val();
                                $(Crtl).val(" ");



                            }
                            //End on 19/4/16 BY M Ahmad Imran
                            $.each(result, function (j, item) {
                                if (item.Name.toLowerCase() != "- select -") {

                                    // Whether AutoComplete is CPT
                                    if (iscode == "CPT") {

                                        if (item.RefValue) {
                                            var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                            var _ConcatinatedString = item.Name;

                                            // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                            if (_ConcatinatedString.indexOf("!") >= 0) {

                                                LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                                CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                                ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                                SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                                _LexiCode = LexiCode;

                                                _CPT = CPT.split("+")[0];
                                                _CPTDescription = CPT.split("+")[1];
                                                _ICD = ICD.split("+")[0];
                                                _ICDDescription = ICD.split("+")[1];
                                                _SNOMED = SNOMED.split("+")[0];
                                                _SNOMEDDescription = SNOMED.split("+")[1];
                                            }
                                            else {
                                                CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                                _CPT = CPT.split("-")[0].trim();
                                                _CPTDescription = CPT.split("-")[1].trim();
                                            }

                                            var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                            // In IMO case Build Four Column Header, Else Two Columns

                                            _CPTDescription = _CPTDescription.replace(/&quot;/g, '"').replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&#39;/g, "'").replace(/&amp;/g, "&").replace(/\/\equal/g, '=');

                                            //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                            if (ParentControl == 'PQRS_ICDCPTCodes') {
                                                var CPTMatched = false;
                                                $.each(PQRS_ICDCPTCodes.arrCPTs, function (i, item) {

                                                    if (_CPT == item.Code) {
                                                        CPTMatched = true;
                                                    }

                                                });
                                                if (CPTMatched == false) {
                                                    return true;
                                                }
                                            }
                                            //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes

                                            if (isIMO == "imo") {

                                                var duMulti = _LexiCode + "*" + CPT + "$" + ICD + "~" + SNOMED;
                                                _ICD = _ICD != "" ? _ICD : "NoICD";
                                                _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                                                //allData.push({ id: item.Value, value: _CPT + " - " + _ICD + " - " + _SNOMED + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                //Start on 19/4/16 BY M Ahmad Imran
                                                if (!IsSnomedTypeSearch) {
                                                    allData.push({ id: item.Value, value: _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                }
                                                else {
                                                    if (_CPT == "") {
                                                        allData.push({ id: item.Value, value: _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                    else {
                                                        allData.push({ id: item.Value, value: _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                }
                                                //End on 19/4/16 BY M Ahmad Imran

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);
                                                //var icd9Length = ((spaceHandler_.split('-')[1].trim().length + 3) - 3);
                                                //var snomedLength = ((spaceHandler_.split('-')[2].trim().length + 3) - 6);

                                                var cptSpaces = "", icd9Spaces = "", snomedSpaces = "";

                                                for (var i = 0; i < cptLength; i++) { cptSpaces += '&nbsp;'; }
                                                //for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                                //snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                                //for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {

                                                            //ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>ICD" + icd9Spaces + "</label><label>SNOMED" + snomedSpaces + "</label><label>DESCRIPTION</label></li>");
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");

                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                                LocalOrIMO = "imo";
                                            }
                                            else {
                                                var duMulti = "*" + CPT + "$" + "~";
                                                allData.push({ id: item.Value, value: _CPT + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);

                                                var cptSpaces = "";
                                                for (var i = 0; i < cptLength; i++)
                                                    cptSpaces += '&nbsp;';

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");
                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                            }
                                            LocalOrIMO = "MDVision";
                                        }
                                        //Start on 19/4/16 BY M Ahmad Imran
                                        if (item.RefValue == "") {
                                            var LexiCode = "", CPT = "", ICD = "", SNOMED = "", _CPT = "", _CPTDescription = "", _ICD = "", _ICDDescription = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                            var _ConcatinatedString = item.Name;

                                            // In IMO case it would be true, IN MDVision Database Case it will fall in 'else' Block
                                            if (_ConcatinatedString.indexOf("!") >= 0) {

                                                LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                                CPT = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                                ICD = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                                SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                                _LexiCode = LexiCode;

                                                _CPT = CPT.split("+")[0];
                                                _CPTDescription = CPT.split("+")[1];
                                                _ICD = ICD.split("+")[0];
                                                _ICDDescription = ICD.split("+")[1];
                                                _SNOMED = SNOMED.split("+")[0];
                                                _SNOMEDDescription = SNOMED.split("+")[1];
                                            }
                                            else {
                                                CPT = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("^"));
                                                _CPT = CPT.split("-")[0].trim();
                                                _CPTDescription = CPT.split("-")[1].trim();
                                            }

                                            var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                            // In IMO case Build Four Column Header, Else Two Columns

                                            //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                            if (ParentControl == 'PQRS_ICDCPTCodes') {
                                                var CPTMatched = false;
                                                $.each(PQRS_ICDCPTCodes.arrCPTs, function (i, item) {

                                                    if (_CPT == item.Code) {
                                                        CPTMatched = true;
                                                    }

                                                });
                                                if (CPTMatched == false) {
                                                    return true;
                                                }
                                            }
                                            //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                            if (isIMO == "imo") {

                                                var duMulti = _LexiCode + "*" + CPT + "$" + ICD + "~" + SNOMED;
                                                _ICD = _ICD != "" ? _ICD : "NoICD";
                                                _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                                                //allData.push({ id: item.Value, value: _CPT + " - " + _ICD + " - " + _SNOMED + " - " + _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                if (!IsSnomedTypeSearch) {
                                                    allData.push({ id: item.Value, value: _CPTDescription, RefValue: item.RefValue, RefName: duMulti });
                                                }
                                                else {
                                                    if (_CPT == "") {
                                                        allData.push({ id: item.Value, value: _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                    else {
                                                        allData.push({ id: item.Value, value: _CPTDescription + " (SCT: " + _SNOMED + ")", RefValue: item.RefValue, RefName: duMulti });
                                                    }
                                                }

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);
                                                //var icd9Length = ((spaceHandler_.split('-')[1].trim().length + 3) - 3);
                                                //var snomedLength = ((spaceHandler_.split('-')[2].trim().length + 3) - 6);

                                                var cptSpaces = "", icd9Spaces = "", snomedSpaces = "";

                                                for (var i = 0; i < cptLength; i++) { cptSpaces += '&nbsp;'; }
                                                //for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                                //snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                                //for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {

                                                            //ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>ICD" + icd9Spaces + "</label><label>SNOMED" + snomedSpaces + "</label><label>DESCRIPTION</label></li>");
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");

                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                                LocalOrIMO = "imo";
                                            }
                                            else {
                                                var duMulti = "*" + CPT + "$" + "~";
                                                allData.push({ id: item.Value, value: _CPTDescription, RefValue: item.RefValue, RefName: duMulti });

                                                var spaceHandler_ = allData[0].value;
                                                var cptLength = ((spaceHandler_.split('-')[0].trim().length + 3) - 3);

                                                var cptSpaces = "";
                                                for (var i = 0; i < cptLength; i++)
                                                    cptSpaces += '&nbsp;';

                                                $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                    _create: function () {
                                                        this._super();
                                                        this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                    },
                                                    _renderMenu: function (ul, items) {
                                                        var that = this;
                                                        if (that.element.context.id == $(Crtl).attr('id')) {
                                                            ul.append("<li class='ui-autocomplete-category'><label>&nbsp; CPT" + cptSpaces + "</label><label>DESCRIPTION</label></li>");
                                                        }
                                                        $.each(items, function (index, item) {
                                                            var li;
                                                            li = that._renderItemData(ul, item);
                                                        });

                                                    }
                                                });
                                            }
                                            LocalOrIMO = "MDVision";
                                        }
                                        //End on 19/4/16 BY M Ahmad Imran
                                    }
                                        // ICD
                                    else if (iscode == "ICD") {

                                        var LexiCode = "", ICD9 = "", ICD10 = "", SNOMED = "", _ICD9 = "", _ICD9Description = "", _ICD10 = "",
                                            _ICD10Description = "", _SNOMED = "", _SNOMEDDescription = "", _LexiCode = "";

                                        var _ConcatinatedString = item.Name;

                                        LexiCode = _ConcatinatedString.substring(0, _ConcatinatedString.lastIndexOf("!"));
                                        ICD9 = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("!") + 1, _ConcatinatedString.lastIndexOf("@"));
                                        ICD10 = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("@") + 1, _ConcatinatedString.lastIndexOf("#"));
                                        SNOMED = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("#") + 1);
                                        _LexiCode = LexiCode;

                                        _ICD9 = ICD9.split("+")[0];
                                        _ICD9Description = ICD9.split("+")[1];
                                        _ICD10 = ICD10.split("+")[0];
                                        _ICD10Description = ICD10.split("+")[1];
                                        _SNOMED = SNOMED.split("+")[0];
                                        _SNOMEDDescription = SNOMED.split("+")[1];

                                        var duMulti = _LexiCode + "*" + ICD9 + "$" + ICD10 + "~" + SNOMED;

                                        var isIMO = _ConcatinatedString.substring(_ConcatinatedString.lastIndexOf("^") + 1);
                                        _ICD10 = _ICD10 != "" ? _ICD10 : "NoICD10";
                                        _SNOMED = _SNOMED != "" ? _SNOMED : "NoSNOMED";
                                        //Start//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes
                                        if (ParentControl == 'PQRS_ICDCPTCodes') {
                                            var ICDMatched = false;
                                            $.each(PQRS_ICDCPTCodes.arrICDs, function (i, item) {

                                                if (_ICD10 == item.Code) {
                                                    ICDMatched = true;
                                                }

                                            });
                                            if (ICDMatched == false) {
                                                return true;
                                            }
                                        }
                                        if (isComplaint == "fromComplaints") {
                                            if (isIMO == "imo") {
                                                if (isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else if (!isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else {
                                                    allData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                                                }
                                                LocalOrIMO = "imo";
                                            }
                                            else {
                                                if (isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else if (!isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else {
                                                    allData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                                                }
                                                LocalOrIMO = "MDVision";
                                            }
                                        } else {
                                            if (isIMO == "imo") {
                                                if (isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else if (!isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else {
                                                    allData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                                                }
                                                LocalOrIMO = "imo";
                                            }
                                            else {
                                                if (isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else if (!isIcd9)
                                                    allData.push({ id: item.Value, value: _ICD9 + " - " + _ICD10 + " - " + _SNOMED + " - " + _ICD9Description, RefValue: item.RefValue, RefName: duMulti });
                                                else {
                                                    allData.push({ id: item.Value, value: item.Name, RefValue: item.RefValue });
                                                }

                                            }
                                            LocalOrIMO = "MDVision";
                                            var spaceHandler = allData[0].value;
                                            var icd9Length = ((spaceHandler.split('-')[0].trim().length + 3) - 4);
                                            var icd10Length = ((spaceHandler.split('-')[1].trim().length + 3) - 5);
                                            var snomedLength = ((spaceHandler.split('-')[2].trim().length + 3) - 6);

                                            var icd9Spaces = "", icd10Spaces = "", snomedSpaces = "";

                                            for (var i = 0; i < icd9Length; i++) { icd9Spaces += '&nbsp;'; }
                                            icd10Length = icd10Length <= -1 ? 8 : icd10Length;
                                            for (var i = 0; i < icd10Length ; i++) { icd10Spaces += '&nbsp;'; }
                                            snomedLength = snomedLength <= -1 ? 8 : snomedLength;
                                            for (var i = 0; i < snomedLength ; i++) { snomedSpaces += '&nbsp;'; }

                                            $.widget('#' + $(Crtl).attr('id') + " custom.autocomplete", $.ui.autocomplete, {
                                                _create: function () {
                                                    this._super();
                                                    this.widget().menu("option", "items", "> :not(.ui-autocomplete-category)");
                                                },
                                                _renderMenu: function (ul, items) {
                                                    var that = this;
                                                    if (that.element.context.id == $(Crtl).attr('id')) {
                                                        ul.append("<li class='ui-autocomplete-category'><label>&nbsp; ICD9" + icd9Spaces + "</label><label>&nbsp; ICD10" + icd10Spaces + "</label><label>SNOMED" + snomedSpaces + "</label><label>DESCRIPTION</label></li>");

                                                    } $.each(items, function (index, item) {
                                                        var li;
                                                        li = that._renderItemData(ul, item);
                                                    });

                                                }
                                            });
                                        }
                                        //End//17-08-2016//Ahmad Raza//PQRS_ICDCPTCodes Search Changes


                                    }
                                    else {

                                    }
                                }
                            });

                            BackgroundLoaderShow(false);
                            $(Crtl).autocomplete({
                                autoFocus: true,
                                minLength: 0,
                                //source: AllCode, // pass an array (without a comma)
                                source: allData,
                                select: function (event, ui) {
                                    BackgroundLoaderShow(false);
                                    var TABKEY = 9;
                                    this.value = ui.item.value;

                                    if (event.keyCode == TABKEY) {
                                        event.preventDefault();
                                        this.value = this.value;
                                    }
                                    // add the selected id
                                    //$(Crtl).
                                    setTimeout(function () {

                                        if (ui.item != undefined)
                                            $(Crtl).val(ui.item.RefValue);
                                        else
                                            $(Crtl).val("");


                                        if (typeof hiddenCrtl != "undefined")
                                            if (hiddenCrtl != null) {
                                                if (ui.item != undefined) {
                                                    if (isDescription) {

                                                        if (iscode == "CPT") {
                                                            if (ui.item != undefined)
                                                                cptcodeDetail.SetControlValues(ui.item.value, ui.item.RefName, ParentControl, ContainerCtrl, hiddenCrtl);

                                                            else
                                                                ;
                                                        }
                                                        else {
                                                            //if (isIcd9)
                                                            //    ICDDetail.SetControlValues(ui.item.RefName);
                                                            //else
                                                            if (ui.item != undefined)
                                                                SupperBillDetail.OpenIMODetail(ui.item.RefName, ui.item.value, ParentControl, hfSubColumnPlusChargeId, LocalOrIMO, text, null, customFormToolsParentId);
                                                            else
                                                                ;
                                                        }
                                                        setTimeout(function () {
                                                            // adnan maqbool, PMS-4925
                                                            if (ParentControl == "SupperBillDetail") {
                                                                $(hiddenCrtl).val((ui.item.value).split(' - ')[1]).focus();
                                                            } else {
                                                                $(hiddenCrtl).val(ui.item.value).focus();
                                                            }
                                                            //
                                                        }, 30);
                                                    }
                                                    else {
                                                        setTimeout(function () { $(hiddenCrtl).val(ui.item.id).focus(); }, 100);
                                                    }
                                                }
                                            }
                                    }, 10);
                                    //$(Crtl).text(ui.item.RefValue);
                                }
                            });
                            try {
                                if ($(Crtl).closest("#tblBillingInformation").attr("id") == "tblBillingInformation")
                                    $(Crtl).autocomplete("option", "appendTo", "#dgvBillVisitCharge_wrapper");
                            } catch (ex) {
                                console.log(ex);
                            }
                            $(Crtl).autocomplete("search", "");


                            //Start on 19/4/16 BY M Ahmad Imran
                            if (IsSnomedTypeSearch) {
                                //$(Crtl).val(sctString + $(Crtl).val());
                                $(Crtl).val(sctString);
                                IsSnomedTypeSearch = false;
                            }
                            //End on 19/4/16 BY M Ahmad Imran

                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        });
        //End//05-05-2016//Ahmad Raza//implimented privileges for ICD Search
    },

    MakeEditableGrid: function (GridPanel, GridId, ClassName, AddDefaultRow, bInfo, bFilter, bPaginate, bSort, iPageSize) {
        $(GridPanel).css("display", "inline");
        if ($.fn.dataTable.isDataTable(GridId))
            ;
        else {
            //$(GridId + ' tbody').find("tr").remove();
            EditableGrid.initialize(GridId, ClassName, AddDefaultRow, bInfo, bFilter, bPaginate, bSort, iPageSize);
            $(GridId + "_length").remove();
        }

        return EditableGrid;
    },

    MakeTableHeaderFixed: function (DataTableObj, offsetTop, zIndexTop) {
        if ((offsetTop != null && parseInt(offsetTop) > 0) && (zIndexTop != null && parseInt(zIndexTop))) {
            new $.fn.dataTable.FixedHeader(DataTableObj, {
                'offsetTop': parseInt(offsetTop),// 150,
                'zTop': parseInt(zIndexTop),//1032,
            });
        }

    },

    ValidateStartToEndDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback, NotDisableToDate) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        //$(CtrlForm + ' ' + CtrlFromDate + ',' + CtrlToDate).datepicker({
        //    todayHighlight: true,
        //    format: date_format,
        //});
        if (NotDisableToDate) {
            $(CtrlToDate).attr('disabled', false);
        }
        else {
            $(CtrlToDate).attr('disabled', true);
        }
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    utility.DisplayMessages("Start date is greater than End date", 3);
                }
            }

            $(CtrlToDate).attr('disabled', false);
            // $(CtrlToDate).datepicker('remove');


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }

            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(CtrlToDate).attr("name");
            }
            if (typeof CtrFromDateName == 'undefined') {
                CtrFromDateName = $(CtrlFromDate).attr("name");
            }
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            // startDate: inputDate,
            format: date_format,
            //todayBtn: 'linked',
        }).change(function (e) {
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(this).attr("name");
            }
            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                        $(CtrlForm).bootstrapValidator('revalidateField', CurrentDatepicker.name);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                            utility.DisplayMessages("End date is smaller than Start date", 3)
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        //$(CtrlFromDate).inputmask({
        //    mask: DateNewFormat
        //});
        //$(CtrlToDate).inputmask({
        //    mask: DateNewFormat
        //});
        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {

                   if ($(CtrlFromDate).val() != "")
                       utility.ValidateDate(CtrlFromDate);
               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {

                if ($(CtrlToDate).val() != "")
                    utility.ValidateDate(CtrlToDate)
            }, 100);
        });
    },

    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback, NotDisableToDate, isCurrentDate) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        //$(CtrlForm + ' ' + CtrlFromDate + ',' + CtrlToDate).datepicker({
        //    todayHighlight: true,
        //    format: date_format,
        //});
        if (NotDisableToDate) {
            $(CtrlToDate).attr('disabled', false);
        }
        else {
            $(CtrlToDate).attr('disabled', true);
        }
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    utility.DisplayMessages("From date is greater than To date", 3);
                }
            }

            $(CtrlToDate).attr('disabled', false);
            // $(CtrlToDate).datepicker('remove');


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }

            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(CtrlToDate).attr("name");
            }
            if (typeof CtrFromDateName == 'undefined') {
                CtrFromDateName = $(CtrlFromDate).attr("name");
            }
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            // startDate: inputDate,
            format: date_format,
            //todayBtn: 'linked',
        }).change(function (e) {
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(this).attr("name");
            }
            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                        $(CtrlForm).bootstrapValidator('revalidateField', CurrentDatepicker.name);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                            utility.DisplayMessages("To date is smaller than from date", 3)
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        //$(CtrlFromDate).inputmask({
        //    mask: DateNewFormat
        //});
        //$(CtrlToDate).inputmask({
        //    mask: DateNewFormat
        //});
        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {

                   if ($(CtrlFromDate).val() != "")
                       utility.ValidateDate(CtrlFromDate);
               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {

                if ($(CtrlToDate).val() != "")
                    utility.ValidateDate(CtrlToDate)
            }, 100);
        });
        if (isCurrentDate) {
            $(CtrlFromDate).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date($('#userCurrentTime').html())));
            $(CtrlToDate).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date($('#userCurrentTime').html())));
        }
    },
    ValidateDateMonthView: function (Control) {
        if ($(Control).val().length == 7) {
            //$(Control).val(DateValued);
            //$(Control).datepicker("setDate", DateValued);
        } else {
            $(Control).val('');
            utility.DisplayMessages("Please enter valid date", 3);

        }
    },

    ValidateDate: function (Control) {
        var ControlValue = $(Control).val();
        //if ($(Control).val().length < date_format.length) {
        //    ControlValue = $.datepicker.formatDate(date_format.replace('yy', ''), $(Control).datepicker('getDate'));
        //}
        try {
            var firstOriginal = '';
            var firstModified = '';
            var secondOriginal = '';
            var secondModified = '';
            if (ControlValue.split('/')[0].length < 2) {
                firstOriginal = ControlValue.split('/')[0];
                firstModified = "0" + firstOriginal + "/";
            }
            if (ControlValue.split('/')[1].length < 2) {
                secondOriginal = ControlValue.split('/')[1];
                secondModified = "0" + secondOriginal + "/";
            }
            var first = firstModified == "" ? ControlValue.split('/')[0] + "/" : firstModified;
            var second = secondModified == "" ? ControlValue.split('/')[1] + "/" : secondModified;
            var third = ControlValue.split('/')[2];
            ControlValue = first + second + third;
        }
        catch (ex) {
            console.log(ex);
        }
        if (ControlValue.length == date_format.length) {
            if (!utility.isValidDate(ControlValue)) {
                var DateValued = ControlValue.substring(0, 2) + "/" + ControlValue.substring(2, 4) + "/" + ControlValue.substring(4, 8);
                if (!utility.isValidDate(DateValued)) {
                    $(Control).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                    if ($($(Control).parents('form')[0]).data('bootstrapValidator') != null && typeof $($(Control).parents('form')[0]).data('bootstrapValidator') != 'undefined') {
                        $($(Control).parents('form')[0]).bootstrapValidator('revalidateField', Control.name);
                    }
                }
                else {
                    //$(Control).val(DateValued);
                    $(Control).datepicker("setDate", DateValued);
                }

            } else {
                if ($.datepicker.formatDate(date_format.replace('yy', ''), $(Control).datepicker('getDate')) != $(Control).val()) {

                    $(Control).datepicker("setDate", $(Control).val());
                }

            }
        } else {
            $(Control).val('');
            utility.DisplayMessages("Please enter valid date", 3);
            if ($($(Control).parents('form')[0]).data('bootstrapValidator') != null && typeof $($(Control).parents('form')[0]).data('bootstrapValidator') != 'undefined') {
                $($(Control).parents('form')[0]).bootstrapValidator('revalidateField', Control.name);
            }
        }
    },
    CreateDatePicker: function (controlId, onChangeCallback, isCurrentDate, FormId, IsOptional) {
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));
        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];
        var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');
        //$('#' + controlId).inputmask({
        //    mask: DateNewFormat
        //});
        //  $('#' + controlId).attr('data-mask', DateNewFormat);
        $('#' + controlId).attr('maxlength', '10');
        $('#' + controlId).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).on('changeDate', function (e) {

            if (typeof (onChangeCallback) == 'function') {
                setTimeout(onChangeCallback, 50);
            }
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }
            //$(this).val(e.target.value);
            if (FormId != null && IsOptional != null && !IsOptional) {
                // to Handle Multiple Date Controls Revalidation
                $('#' + controlId).each(function () {
                    var CtrlName = $(this).attr("name");
                    if (CtrlName != null) {
                        $('#' + FormId).bootstrapValidator('revalidateField', CtrlName);
                    }
                });

            }
            $(this).datepicker('hide');
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
            //alert(e.target.value);

        }).on('blur', function (e) {
            var datepickerID = e.currentTarget;
            setTimeout(
               function () {
                   if ($(datepickerID).val() != "")
                       utility.ValidateDate(datepickerID);
               }, 100);

        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }
        });

        if (isCurrentDate)
            $('#' + controlId).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
    },


    ValidateMonthViewFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        //$(CtrlForm + ' ' + CtrlFromDate + ',' + CtrlToDate).datepicker({
        //    todayHighlight: true,
        //    format: date_format,
        //});
        var DateNewFormat = date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join('');
        $(CtrlToDate).attr('disabled', true);
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join(''),
            todayBtn: 'linked',
            minViewMode: 1
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = $(CtrlFromDate).val() != '' ? $(CtrlFromDate).datepicker('getDate') : "";
                var toDate = $(CtrlToDate).val() != '' ? $(CtrlToDate).datepicker('getDate') : "";

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                    utility.DisplayMessages("To date is smaller than from date", 3);

                }
            }

            $(CtrlToDate).attr('disabled', false);
            // $(CtrlToDate).datepicker('remove');
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(CtrlToDate).attr("name");
            }
            if (typeof CtrFromDateName == 'undefined') {
                CtrFromDateName = $(CtrlFromDate).attr("name");
            }


            var inputDate = $(CtrlFromDate).datepicker('getDate');

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }

            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);

            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.MonthViewAutoCompleteDate(this, startDate, endDate);
            }
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            format: date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join(''),
            todayBtn: 'linked',
            minViewMode: 1
        }).change(function (e) {


            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                        $(CurrentDatepicker).bootstrapValidator('revalidateField', CurrentDatepicker.name);
                    }
                }
                if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                    var fromDate = $(CtrlFromDate).val() != '' ? $(CtrlFromDate).datepicker('getDate') : "";
                    var toDate = $(CtrlToDate).val() != '' ? $(CtrlToDate).datepicker('getDate') : "";
                    if (fromDate > toDate) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("To date is smaller than from date", 3)
                        $(CurrentDatepicker).bootstrapValidator('revalidateField', CtrToDateName.name);
                    }
                }

            }, 50);
            if (typeof CtrToDateName == 'undefined') {
                CtrToDateName = $(this).attr("name");
            }
            $(this).datepicker('hide');
            if (!IsOptional)
                $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.MonthViewAutoCompleteDate(this, startDate, endDate);
            }
        });
        var DateNewFormat = date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join('');
        // var DateNewFormat = date_format.replace('dd', '99');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');

        //$(CtrlFromDate).inputmask({
        //    mask: DateNewFormat
        //});
        //$(CtrlToDate).inputmask({
        //    mask: DateNewFormat
        //});
        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {

                   if ($(CtrlFromDate).val() != "")
                       utility.ValidateDateMonthView(CtrlFromDate);
               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {

                if ($(CtrlToDate).val() != "")
                    utility.ValidateDateMonthView(CtrlToDate)
            }, 100);
        });
    },

    CreateMonthViewDatePicker: function (controlId, onChangeCallback, isCurrentDate, FormId, IsOptional) {
        var startDate = new Date('01/01/1700');
        var endDate = new Date('01/01/' + Number(new Date().getFullYear() + 35));
        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];


        var DateNewFormat = date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join('');
        DateNewFormat = DateNewFormat.replace('mm', '99');
        DateNewFormat = DateNewFormat.replace('yyyy', '9999');
        //$('#' + controlId).inputmask({
        //    mask: DateNewFormat
        //});
        //  $('#' + controlId).attr('data-mask', DateNewFormat);
        $('#' + controlId).attr('maxlength', '10');
        $('#' + controlId).datepicker({
            todayHighlight: true,
            format: date_format.split(date_format.substring(date_format.indexOf("d"), date_format.indexOf("d") + 3)).join(''),
            todayBtn: 'linked',
            viewMode: "months",
            minViewMode: "months"
        }).on('changeDate', function (e) {

            if (typeof (onChangeCallback) == 'function') {
                setTimeout(onChangeCallback, 50);
            }
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }
            //$(this).val(e.target.value);

            $(this).datepicker('hide');
            if (FormId != null && IsOptional != null && !IsOptional) {
                // to Handle Multiple Date Controls Revalidation
                $('#' + controlId).each(function () {
                    var CtrlName = $(this).attr("name");
                    if (CtrlName != null) {
                        $('#' + FormId).bootstrapValidator('revalidateField', CtrlName);
                    }
                });

            }
        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
            //alert(e.target.value);

        }).on('blur', function (e) {
            var datepickerID = e.currentTarget.id;
            setTimeout(
               function () {
                   if ($('#' + datepickerID).val() != "")
                       utility.ValidateDateMonthView('#' + datepickerID);
               }, 100);

        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.MonthViewAutoCompleteDate(this, startDate, endDate);
            }
        });

        if (isCurrentDate)
            $('#' + controlId).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
    },
    preventAlphabetsInDatePicker: function (e) {
        var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (e.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

        }
        else if ((keyCode >= 33 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20))
        { }
        else
            e.preventDefault();
    },
    ValidateDOB: function (FormId, CtrlDOBDateId, minDate, maxDate, IsOptional) {
        var startDate = new Date(minDate);
        var endDate = new Date(maxDate);
        var DOBName = $('#' + CtrlDOBDateId).attr("name");

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];
        $('#' + CtrlDOBDateId).attr('maxlength', '10');
        $('#' + CtrlDOBDateId).datepicker({
            // todayHighlight: true,
            startDate: startDate,
            endDate: endDate,
            format: date_format,
            todayBtn: 'linked',

        }).on('changeDate', function (e) {
            if (typeof DOBName == 'undefined') {
                DOBName = $(this).attr("name");
            }

            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }
            if (!IsOptional) {
                $('#' + FormId).bootstrapValidator('revalidateField', DOBName);
            }
            if (FormId == 'pnlPatient_Search #frmPatientSearch')
                $(this).focus();
            $(this).datepicker('hide');
        }).on('keyup', function (e) {
            //$(this).datepicker('hide');
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if (keyCode != 8) {
                utility.AutoCompleteDate(this, startDate, endDate);
            }

        }).on('keypress', function (e) {
            //$(this).datepicker('hide');
            utility.preventAlphabetsInDatePicker(e);
        }).on('blur', function (e) {
            var datepickerID = e.currentTarget;
            setTimeout(
               function () {
                   if ($(datepickerID).val() != "")
                       utility.ValidateDate(datepickerID);
               }, 100);
            if (!IsOptional) {
                $('#' + FormId).bootstrapValidator('revalidateField', DOBName);
            }

        })
    },

    AutoCompleteDate: function (obj, startDate, endDate) {
        var DateLength = $(obj).val().length;
        if (DateLength == 2 || DateLength == 5) {
            var DateValue = $(obj).val();
            DateValue += "/";
            $(obj).val(DateValue);
        }
        var CtrlValue = $(obj).val();
        if (CtrlValue.length > 0) {
            var minDate = new Date();
            var maxDate = new Date();
            if (startDate != null)
                minDate = new Date(startDate);
            if (endDate != null)
                maxDate = new Date(endDate);

            if (CtrlValue.length > 9) {
                var DateValue = new Date(CtrlValue);
                if (DateValue < minDate || DateValue > maxDate) {
                    $(obj).val("");
                    //alert("value reset");
                }
            }

        }
    },

    MonthViewAutoCompleteDate: function (obj, startDate, endDate) {
        var DateLength = $(obj).val().length;
        if (DateLength == 2) {
            var DateValue = $(obj).val();
            DateValue += "/";
            $(obj).val(DateValue);
        }
        var CtrlValue = $(obj).val();
        if (CtrlValue.length > 0) {
            var minDate = new Date();
            var maxDate = new Date();
            if (startDate != null)
                minDate = new Date(startDate);
            if (endDate != null)
                maxDate = new Date(endDate);

            if (CtrlValue.length > 7) {
                var DateValue = new Date(CtrlValue);
                if (DateValue < minDate || DateValue > maxDate) {
                    $(obj).val("");
                    //alert("value reset");
                }
            }

        }
    },

    AddDaysFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, FromDays, ToDays) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        Date.prototype.addDays = function (days) {
            var dat = new Date(this.valueOf());
            dat.setDate(dat.getDate() + days);
            return dat;
        }

        $(CtrlFromDate).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date().addDays(FromDays)));
        $(CtrlToDate).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date().addDays(ToDays)));

    },

    ValidateSearchCriteria: function (CtrlId, ValidCallBack, InValidCallBack) {

        var isok = false;
        CtrlId = "#" + CtrlId;
        $(CtrlId).find('select,[type=text],[type=hidden]').each(function () {
            if ($(this).hasClass('ValidateCriteria') == true && $(this).val() != "") {
                if (this.tagName.toLowerCase() == "select" && ($(this).val().toLowerCase() == "all")) {
                    // ignore.
                }
                else {
                    isok = true;
                    return false;
                }
            }
        });

        if (isok == true)
            ValidCallBack();
        else if (InValidCallBack)
            InValidCallBack();
        else
            utility.DisplayMessages('Please enter search criteria', 2);
    },

    ChangeNameCase: function (CtrlId) {
        var value = $(CtrlId).val() != null ? $(CtrlId).val() : "";
        var newValue = value.replace(/\w\S*/g, function (match) {
            return match.charAt(0).toUpperCase() + match.substring(1).toLowerCase();
        });
        $(CtrlId).val(newValue);
    },

    CapitalizeFirstCharCommaSepString: function (str) {
        var value = "";
        var arrTemp = [];
        if (str) {
            var arr = str.split(',');
            if (arr && arr.length > 0) {
                $.each(arr, function (i, item) {
                    arrTemp.push(item.split(' ').map(function (val) {
                        return val.charAt(0).toUpperCase() + val.substr(1).toLowerCase();
                    }).join(' '));
                });
                if (arrTemp && arrTemp.length > 0)
                    value = arrTemp.join(", ");
            }
        }
        else
            value = str;
        return value;
    },
    titleCase: function (str) {
        return str.split(' ').map(function (val) {
            return val.charAt(0).toUpperCase() + val.substr(1).toLowerCase();
        }).join(' ');
    },
    creditCardExpiryDate: function (controlId, onChangeCallback, FormId, IsOptional) {

        var date_format = 'mm/yyyy';
        //set default Date Format
        var day = '01/';
        var month = (Number(new Date().getMonth() + 2)) < 10 ? ("0" + (Number(new Date().getMonth() + 2))) : (Number(new Date().getMonth() + 2));
        var year = '/' + Number(new Date().getFullYear());
        var myStartDate = day + month + year;


        $('#' + controlId).attr('maxlength', '7');
        $('#' + controlId).datepicker({
            format: date_format,
            startDate: myStartDate,
        }).on('changeDate', function (e) {
            if (FormId != null && IsOptional != null && !IsOptional) {
                // to Handle Multiple Date Controls Revalidation
                $('#' + controlId).each(function () {
                    var CtrlName = $(this).attr("name");
                    if (CtrlName != null) {
                        $('#' + FormId).bootstrapValidator('revalidateField', CtrlName);
                    }
                });

            }
            if (typeof (onChangeCallback) == 'function') {
                setTimeout(onChangeCallback, 50);
            }


            $(this).datepicker('hide');
        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);

        }).on('blur', function (e) {
            var datepickerID = e.currentTarget.id;
            setTimeout(
               function () {
                   // if anything onblur is required
               }, 100);

        }).on('keyup', function (e) {
            var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        });


    },

    //Start 22-06-2018 TahreemMalik   To check whether string is empty or null AST-256
    IsNullOrEmptyString: function (value) {
        return !value;
    },
    //End 22-06-2018 TahreemMalik   To check whether string is empty or null AST-256

    MergeJSON: function (ParentJSON, ChildJSON) {
        var MergedJSON = null;
        if (ParentJSON && ChildJSON) {
            MergedJSON = JSON.stringify($.extend(false, {}, JSON.parse(ParentJSON), JSON.parse(ChildJSON)));
        }
        return MergedJSON;
    },

    MakeSerializableHTML: function (PanelId) {
        $(PanelId).find('[type=text],[type=password], textarea, [type=checkbox], [type=radio], select').each(function () {
            if (!$(this).attr("name")) {
                $(this).attr("name", $(this).attr("id"));
            }
        });
    },

    ClearFormControls: function (formId) {
        $(formId).find('[type=text],[type=password], textarea, [type=checkbox], [type=radio], select,[type=hidden]').each(function () {
            if ($(this).attr("type") == "text" || $(this).attr("type") == "password" || $(this).attr("type") == "hidden" || $(this).is('textarea')) {
                $(this).val("");
            }
            else if ($(this).attr("type") == "checkbox") {
                $(this).prop("checked", false);
            }
            else if ($(this).is('select')) {
                $(this).find("option:eq(0)").attr("selected", "selected");
                $(this).trigger("change");
            }
        });
    },
    //Author: Muhammad Arshad
    //Date: 31-01-2016
    //This function will get number part from string
    getNumericPart: function (obj) {
        var innernumericPart = 0;
        if (obj != null) {
            innernumericPart = obj.replace(/[^\d]+/, '');
        }
        return innernumericPart;
    },

    ValidateValue: function (event, decimalPlaces, min, max) {

        if (!event.shiftKey) {
            //Start 07-09-2016 Azhar Shahzad for decimal point
            if (!$.isNumeric(decimalPlaces)) {
                decimalPlaces = 0;
            }
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

            } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces > 0 && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if ((keyCode >= 33 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110)
            { } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces == 0) {
                event.preventDefault();
            } else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') == -1) {
                //event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
                //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
                //    event.preventDefault();

                //}
            else {
                event.preventDefault();
            }
            //End 07-09-2016 Azhar Shahzad for decimal point

            //if (!$("#" + event.currentTarget.id).val().match(/^\d+$/))
            //{
            //    $("#" + event.currentTarget.id).val("");
            //}


            /****** Range********/
            //to be set from globalAppdata
            if (min == null) {
                min = 0;
            }
            if (max == null || max == 0) {
                max = 9999999;
            }



            var textBox = event.target;
            var isOutOfRange = false;
            if (min > 0 && max > 0) {
                isOutOfRange = (parseFloat(textBox.value) < parseFloat(min)) || (parseFloat(textBox.value) > parseFloat(max));
            }
            else if (min > 0) {
                isOutOfRange = (parseFloat(textBox.value) < parseFloat(min));
            }
            else if (max > 0) {
                isOutOfRange = (parseFloat(textBox.value) > parseFloat(max));
            }

            if (isOutOfRange) {
                //event.preventDefault();
                textBox.value = "";
            }



            /*******************/

            if (event.currentTarget != null) {
                $("#" + event.currentTarget.id).on("focusout", function () {
                    var CurrentVal = $(this).val();
                    if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
                        $(this).val("");
                    }


                    if (CurrentVal != "") {

                        var CurrentValue = parseFloat(CurrentVal).toFixed(decimalPlaces);
                        if (isNaN(CurrentValue)) {
                            $(this).val("");
                        }
                        else if (CurrentValue > max) {
                            $(this).val("");
                        }
                        else
                            $(this).val(parseFloat(CurrentVal).toFixed(decimalPlaces));
                    }
                });
            }
            //else
            //    $("#" + event.currentTarget.id).val(parseFloat($("#" + event.currentTarget.id).val()).toFixed(decimalPlaces));
        } else {
            event.preventDefault();
        }
    },

    ValidateDecimal: function (event, decimalPlaces, min, max) {

        if (!event.shiftKey) {
            //Start 07-09-2016 Azhar Shahzad for decimal point
            if (!$.isNumeric(decimalPlaces)) {
                decimalPlaces = 0;
            }
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

            } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces > 0 && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if ((keyCode >= 33 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110)
            { } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces == 0) {
                event.preventDefault();
            } else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') == -1) {
                //event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
                //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
                //    event.preventDefault();

                //}
            else {
                event.preventDefault();
            }
            //End 07-09-2016 Azhar Shahzad for decimal point

            //if (!$("#" + event.currentTarget.id).val().match(/^\d+$/))
            //{
            //    $("#" + event.currentTarget.id).val("");
            //}


            /****** Range********/
            //to be set from globalAppdata
            if (min == null) {
                min = 0;
            }
            if (max == null) {
                max = 9999999;
            }



            var textBox = event.target;
            var isOutOfRange = (textBox.value < min) || (textBox.value >= max);
            if (isOutOfRange) {
                //event.preventDefault();
                textBox.value = textBox.value.slice(0, -1);
            }



            /*******************/

            if (event.currentTarget != null) {
                $("#" + event.currentTarget.id).on("focusout", function () {
                    var CurrentVal = $(this).val();
                    if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
                        $(this).val("");
                    }


                    if (CurrentVal != "") {

                        var CurrentValue = parseFloat(CurrentVal).toFixed(decimalPlaces);
                        if (isNaN(CurrentValue)) {
                            $(this).val("");
                        }
                        else if (CurrentValue > max) {
                            $(this).val("");
                        }
                        else
                            $(this).val(parseFloat(CurrentVal).toFixed(decimalPlaces));
                    }
                });
            }
            //else
            //    $("#" + event.currentTarget.id).val(parseFloat($("#" + event.currentTarget.id).val()).toFixed(decimalPlaces));
        } else {
            event.preventDefault();
        }
    },

    // this does not appand 0 at end if single decimal char typed.
    ValidateDecimal: function (event, decimalPlaces, min, max, notused) {

        if (!event.shiftKey) {
            //Start 07-09-2016 Azhar Shahzad for decimal point
            if (!$.isNumeric(decimalPlaces)) {
                decimalPlaces = 0;
            }
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

            } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces > 0 && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if ((keyCode >= 33 && keyCode <= 45) || keyCode == 47 || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110) {
                event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && decimalPlaces == 0) {
                event.preventDefault();
            } else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') == -1) {
                //event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if (keyCode >= 48 && keyCode <= 57 || keyCode == 46) { }
                //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
                //    event.preventDefault();

                //}
            else {
                event.preventDefault();
            }
            //End 07-09-2016 Azhar Shahzad for decimal point

            //if (!$("#" + event.currentTarget.id).val().match(/^\d+$/))
            //{
            //    $("#" + event.currentTarget.id).val("");
            //}


            /****** Range********/
            //to be set from globalAppdata
            if (min == null) {
                min = 0;
            }
            if (max == null) {
                max = 9999999;
            }



            var textBox = event.target;
            var isOutOfRange = (parseInt(textBox.value) < min) || (parseInt(textBox.value) > max);
            if (isOutOfRange) {
                //event.preventDefault();
                textBox.value = textBox.value.slice(0, -1);
            }



            /*******************/

            if (event.currentTarget != null) {
                $("#" + event.currentTarget.id).on("focusout", function () {
                    var CurrentVal = $(this).val();
                    if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
                        $(this).val("");
                    }


                    if (CurrentVal != "") {
                        var decimalLength = 0;
                        try {
                            var splitVal = CurrentVal.split(".");
                            if (splitVal.length > 1)
                                decimalLength = splitVal[1].length;
                        } catch (ex) {
                            console.log(ex);
                        }

                        var CurrentValue = 0;
                        if (decimalLength >= 2) {
                            CurrentValue = parseFloat(CurrentVal).toFixed(decimalPlaces);
                        } else {
                            CurrentValue = parseFloat(CurrentVal);
                        }
                        if (isNaN(CurrentValue)) {
                            $(this).val("");
                        }
                        else if (CurrentValue > max) {
                            $(this).val("");
                        }
                        else {
                            if (decimalLength >= 2) {
                                $(this).val(parseFloat(CurrentVal).toFixed(decimalPlaces));
                            }
                        }

                    }
                });
            }
            //else
            //    $("#" + event.currentTarget.id).val(parseFloat($("#" + event.currentTarget.id).val()).toFixed(decimalPlaces));
        } else {
            event.preventDefault();
        }
    },

    // this mathode will alow the user two enter two decimal amount in  Negative
    ValidateBatchDecimal: function (event, decimalPlaces, min, max, notused) {

        if (!event.shiftKey) {
            //Start 07-09-2016 Azhar Shahzad for decimal point
            if (!$.isNumeric(decimalPlaces)) {
                decimalPlaces = 0;
            }
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

            } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces > 0 && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if ((keyCode >= 33 && keyCode <= 45) || keyCode == 47 || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110) {
                // Allow enter only minus signs minus signs 
                if (keyCode != 45)
                    event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && decimalPlaces == 0) {
                event.preventDefault();
            } else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') == -1) {
                //event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if (keyCode >= 48 && keyCode <= 57 || keyCode == 46) { }
                //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
                //    event.preventDefault();

                //}
            else {
                event.preventDefault();
            }
            //End 07-09-2016 Azhar Shahzad for decimal point

            //if (!$("#" + event.currentTarget.id).val().match(/^\d+$/))
            //{
            //    $("#" + event.currentTarget.id).val("");
            //}


            /****** Range********/
            //to be set from globalAppdata
            if (min == null) {
                min = 0;
            }
            if (max == null) {
                max = 9999999;
            }



            var textBox = event.target;
            var isOutOfRange = (parseInt(textBox.value) < min) || (parseInt(textBox.value) > max);
            if (isOutOfRange) {
                //event.preventDefault();
                //textBox.value = textBox.value.slice(0, -1);
            }



            /*******************/

            if (event.currentTarget != null) {
                $("#" + event.currentTarget.id).on("focusout", function () {
                    var CurrentVal = $(this).val();
                    if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
                        $(this).val("");
                    }


                    if (CurrentVal != "") {
                        var decimalLength = 0;
                        try {
                            var splitVal = CurrentVal.split(".");
                            if (splitVal.length > 1)
                                decimalLength = splitVal[1].length;
                        } catch (ex) {
                            console.log(ex);
                        }

                        var CurrentValue = 0;
                        if (decimalLength >= 2) {
                            CurrentValue = parseFloat(CurrentVal).toFixed(decimalPlaces);
                        } else {
                            CurrentValue = parseFloat(CurrentVal);
                        }
                        if (isNaN(CurrentValue)) {
                            $(this).val("");
                        }
                        else if (CurrentValue > max) {
                            $(this).val("");
                        }
                        else {
                            if (decimalLength >= 2) {
                                $(this).val(parseFloat(CurrentVal).toFixed(decimalPlaces));
                            }
                        }

                    }
                });
            }
            //else
            //    $("#" + event.currentTarget.id).val(parseFloat($("#" + event.currentTarget.id).val()).toFixed(decimalPlaces));
        } else {
            event.preventDefault();
        }
    },



    ValidateNegativeDecimal: function (event, decimalPlaces, min, max, notused) {

        if (!event.shiftKey) {
            //Start 07-09-2016 Azhar Shahzad for decimal point
            if (!$.isNumeric(decimalPlaces)) {
                decimalPlaces = 0;
            }
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

            } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces > 0 && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if ((keyCode >= 33 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110)
            { } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces == 0) {
                event.preventDefault();
            } else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') == -1) {
                //event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
                //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
                //    event.preventDefault();

                //}
            else {
                event.preventDefault();
            }
            //End 07-09-2016 Azhar Shahzad for decimal point

            //if (!$("#" + event.currentTarget.id).val().match(/^\d+$/))
            //{
            //    $("#" + event.currentTarget.id).val("");
            //}


            /****** Range********/
            //to be set from globalAppdata
            if (min == null) {
                min = 0;
            }
            if (max == null) {
                max = 9999999;
            }



            //var textBox = event.target;
            //  var isOutOfRange = (parseInt(textBox.value) < min) || (parseInt(textBox.value) > max);
            ///if (isOutOfRange) {
            //    //event.preventDefault();
            //    textBox.value = textBox.value.slice(0, -1);
            //}



            /*******************/

            if (event.currentTarget != null) {
                $("#" + event.currentTarget.id).on("focusout", function () {
                    var CurrentVal = $(this).val();
                    if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
                        $(this).val("");
                    }


                    if (CurrentVal != "") {
                        var decimalLength = 0;
                        try {
                            var splitVal = CurrentVal.split(".");
                            decimalLength = splitVal[1].length;
                        } catch (ex) {
                            console.log(ex);
                        }

                        var CurrentValue = 0;
                        if (decimalLength >= 2) {
                            CurrentValue = parseFloat(CurrentVal).toFixed(decimalPlaces);
                        } else {
                            CurrentValue = parseFloat(CurrentVal);
                        }
                        if (isNaN(CurrentValue)) {
                            $(this).val("");
                        }
                        else if (CurrentValue > max) {
                            $(this).val("");
                        }
                        else {
                            if (decimalLength >= 2) {
                                $(this).val(parseFloat(CurrentVal).toFixed(decimalPlaces));
                            }
                        }

                    }
                });
            }
            //else
            //    $("#" + event.currentTarget.id).val(parseFloat($("#" + event.currentTarget.id).val()).toFixed(decimalPlaces));
        } else {
            event.preventDefault();
        }
    },
    ValidateDecimalCustomForm: function (event, decimalPlaces, min, max) {

        if (!event.shiftKey) {
            if (!$.isNumeric(decimalPlaces)) {
                decimalPlaces = 0;
            }
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;

            if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

            } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces > 0 && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
            else if ((keyCode >= 33 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110)
            { } else if ((keyCode == 110 || keyCode == 190) && decimalPlaces == 0) {
                event.preventDefault();
            } else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') == -1) {
                //event.preventDefault();
            }
            else if ((keyCode == 110 || keyCode == 190) && event.currentTarget.value.indexOf('.') != -1) {
                event.preventDefault();
            }
                //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
                //    event.preventDefault();

                //}
            else {
                event.preventDefault();
            }

            if (event.currentTarget != null) {
                $("#" + event.currentTarget.id).on("focusout", function () {
                    /****** Range********/
                    //to be set from globalAppdata
                    if (min == null) {
                        min = 0;
                    }
                    if (max == null) {
                        max = 9999999;
                    }
                    var textBox = event.target;
                    var isOutOfRange = (parseInt($(this).val()) < min) || (parseInt($(this).val()) > max);
                    if (isOutOfRange) {
                        //event.preventDefault();
                        $(this).val($(this).val().slice(0, -1));
                    }



                    /*******************/
                    var CurrentVal = $(this).val();
                    if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
                        $(this).val("");
                    }


                    if (CurrentVal != "") {
                        var decimalLength = 0;
                        try {
                            var splitVal = CurrentVal.split(".");
                            decimalLength = splitVal[1].length;
                        } catch (ex) {
                            console.log(ex);
                        }

                        var CurrentValue = 0;
                        if (decimalLength >= 2) {
                            CurrentValue = parseFloat(CurrentVal).toFixed(decimalPlaces);
                        } else {
                            CurrentValue = parseFloat(CurrentVal);
                        }
                        if (isNaN(CurrentValue)) {
                            $(this).val("");
                        }
                        else if (CurrentValue > max) {
                            $(this).val("");
                        }
                        else {
                            if (decimalLength >= 2) {
                                $(this).val(parseFloat(CurrentVal).toFixed(decimalPlaces));
                            }
                        }

                    }
                });
            }
        } else {
            event.preventDefault();
        }
    },
    ValidateState: function (event, control) {
        $("#" + control.id).keyup(function () {
            if (!$("#" + control.id).val().match(/^[a-zA-Z]+$/)) {
                $("#" + control.id).val("");
            }
        });
        if (event.keyCode == 9 || event.keyCode == 8) {
            return true;
        }
        if (control.value.length >= 0 && (event.which > 64 && event.which <= 90)) {
            return true;
        }
        else if (control.value.length >= 0 && (event.which > 96 && event.which <= 122)) {
            $("#" + control.id).keyup(function () {
                $("#" + control.id).val($("#" + control.id).val().toUpperCase());
            });
            return true;
        }
        return false;

        //if (!$.isNumeric(maxLength)) {
        //    maxLength = 0;
        //}
        //var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        //if (event.ctrlKey && (keyCode == 97 || keyCode == 99 || keyCode == 120 || keyCode == 118)) {

        //}
        //else if ((keyCode >= 33 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 13 || (keyCode >= 16 && keyCode <= 20) || (keyCode >= 96 && keyCode <= 105) || keyCode == 110)
        //{ }
        //else if ((keyCode != 46 || $("#" + event.currentTarget.id).val().indexOf('.') != -1)) {
        //    event.preventDefault();

        //} else {
        //    event.preventDefault();
        //}
        ////if (!$("#" + event.currentTarget.id).val().match(/^\d+$/))
        ////{
        ////    $("#" + event.currentTarget.id).val("");
        ////}

        //if (event.currentTarget != null) {
        //    $("#" + event.currentTarget.id).on("focusout", function () {
        //        var CurrentVal = $(this).val();
        //        if (CurrentVal.match(/^\d+$/) != null && !CurrentVal.match(/^\d+$/)) {
        //            $(this).val($(this).val().toUppererCase());
        //        }


        //        //if (CurrentVal != "") {

        //        //    var CurrentValue = parseFloat(CurrentVal).toFixed(maxLength);
        //        //    if (isNaN(CurrentValue)) {
        //        //        $(this).val("");
        //        //    }
        //        //    else if (CurrentValue > max) {
        //        //        $(this).val("");
        //        //    }
        //        //    else
        //        //        $(this).val(parseFloat(CurrentVal).toFixed(maxLength));
        //        //}
        //    });
        //}

    },


    ValidateLenght: function (obj, maxlength) {
        var val_ = $(obj).val();
        $(obj).attr("maxlength", maxlength);
        if (val_ > maxlength) {
            var res = val_.slice(0, maxlength);
            $(obj).val(res);
        }
    },


    ClearDropdowns: function () {
        var storage = store.fetchAllKeys(), key;
        for (key in storage) {
            if (key.indexOf('Get') > -1) {
                store.clear(key);
            }
        }

        return true;
    },

    getBool: function (val) {
        if (val != "")
            return !!JSON.parse(String(val).toLowerCase());
        else
            return false;
    },

    validateMasking: function (ev) {
        utility.tempFileRef = ev;
        setTimeout(function () {
            if ($(utility.tempFileRef).attr('data-mask').replace(/9/g, '') == $(utility.tempFileRef).val()) {
                $(utility.tempFileRef).val('');
                utility.tempFileRef = null;
            }
        }, 50)
    },
    isValidDate: function (str) {
        if (str == "" || str == null) { return false; }
        var m = null;
        var ret = true;
        var maxYear = new Date().getFullYear() + 35  //MAX YEAR;
        var minYear = 1800; //MIN YEAR
        var mmIndex, ddIndex, yyIndex;
        var sign = date_format.replace(/[a-zA-Z0-9]/g, '')[0];
        str = str.replace(sign, '-');
        str = str.replace(sign, '-');
        var helperFomat = date_format.replace(sign, '-');
        helperFomat = helperFomat.replace(sign, '-');
        helperFomat = helperFomat.match(/(\w{2})-(\w{2})-(\w{4})/);
        m = str.match(/(\w{2})-(\w{2})-(\w{4})/);
        mmIndex = helperFomat.indexOf('mm');
        ddIndex = helperFomat.indexOf('dd');
        yyIndex = helperFomat.indexOf('yyyy');
        if (m === null || typeof m !== 'object') { return false; }

        if (typeof m !== 'object' && m !== null && m.size !== 3) { return false; }

        // YEAR CHECK
        if ((m[yyIndex].length < 4) || m[yyIndex] < minYear || m[yyIndex] > maxYear) { ret = false; }
        // MONTH CHECK
        if ((m[mmIndex].length < 2) || m[mmIndex] < 1 || m[mmIndex] > 12) { ret = false; }
        // DAY CHECK
        if ((m[ddIndex].length < 2) || m[ddIndex] < 1 || m[ddIndex] > 31) { ret = false; }
        return ret;
    },

    ValidateIsEmpty: function (obj, codeType, hfcontrolid, ParentControl) {
        var Code = $(obj).val();
        if (codeType.toUpperCase() == "ICD") {
            if (Code.length == 0 || Code == "") {
                ICDDetail.ResetHiddenFields(obj, codeType, hfcontrolid, ParentControl);
            }
            //Begin Added on 10-Feb-2016 By Azeem Raza Tayyab to fix Bug#: PMS-3807
            //if (ParentControl == "EncounterChargeCapture") {
            //    var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            //    $($(ChargeGridId).find('div,input')).each(function () {
            //        var $this = $(this);
            //        //if ($this.hasClass("input-group")) {
            //        $this.data('title', $this.attr('title'));
            //        $this.removeAttr('title');
            //        //}
            //    });
            //}
            //End Added on 10-Feb-2016 By Azeem Raza Tayyab to fix Bug#: PMS-3807
        }
    },

    ValidateCode: function (obj, codeType, hfcontrolid, entityId) {

        if (entityId === undefined)
            entityId = globalAppdata["SeletedEntityId"];

        var Code = $(obj).val();
        if (Code != "") {
            if (codeType.toUpperCase() == "ICD") {
                return Admin_ICD.ValidateICDCode(Code, entityId).done(function (response) {
                    if (response.length == 0) {
                        $(obj).val("");
                    }
                    else if ($("#" + hfcontrolid).length > 0) {

                        $("#" + hfcontrolid).val(Code);
                    }
                });
            }
            else if (codeType.toUpperCase() == "CPT") {
                return Admin_CPTCode.ValidateCPTCode(Code, entityId).done(function (response) {
                    if (response.length == 0) {
                        $(obj).val("");
                    }
                    else if ($("#" + hfcontrolid).length > 0) {

                        $("#" + hfcontrolid).val(Code);
                    }
                });
            }
            else if (codeType.toUpperCase() == "MODIFIER") {
                return Admin_Modifier.ValidateModifierCode(Code).done(function (response) {
                    if (response.length == 0) {
                        $(obj).val("");
                    }
                    else if ($("#" + hfcontrolid).length > 0) {

                        //$("#" + hfcontrolid).val(Code);
                    }
                });
            }
            else if (codeType.toUpperCase() == "TOS") {
                return Admin_TypeOfService.ValidateTypeOfService(Code).done(function (response) {
                    if (response.iTotalDisplayRecords == 0) {
                        $(obj).val("");
                    }
                    else if ($("#" + hfcontrolid).length > 0) {

                        //$("#" + hfcontrolid).val(Code);
                    }
                });
            }
            else
                return [];
        } else {
            $("#" + hfcontrolid).val("");
            return [];
        }
    },
    ValidateCPTCode: function (CptCntrlobj, hfCptCntrlobj, value) {
        try {
            var source_ = CptCntrlobj.autocomplete("option", "source");
            if (source_ && source_.length > 0 && value != "") {
                var temp = $.grep(source_, function (v) {
                    return v.value.trim() == value.trim();
                });
                if (temp && temp.length == 0) {
                    utility.DisplayMessages("Invalid CPT Code.", 3);
                    CptCntrlobj.val("");
                    hfCptCntrlobj.val("");
                }
            }
            else {
                utility.DisplayMessages("Invalid CPT Code.", 3);
                CptCntrlobj.val("");
                hfCptCntrlobj.val("");
            }
        }
        catch (ex) {
            if (CptCntrlobj.val() != "")
                utility.DisplayMessages("Invalid CPT Code.", 3);
            CptCntrlobj.val("");
            hfCptCntrlobj.val("");
        }
    },
    UserBrowser: function () {

        //Check if browser is IE or not
        if (isIE()) {
            return "IE"
        }
            //Check if browser is Chrome or not
        else if (navigator.userAgent.search("Chrome") >= 0) {
            return "Chrome"
        }
            //Check if browser is Firefox or not
        else if (navigator.userAgent.search("Firefox") >= 0) {
            return "Firefox";
        }
            //Check if browser is Safari or not
        else if (navigator.userAgent.search("Safari") >= 0 && navigator.userAgent.search("Chrome") < 0) {
            return "Safari"
        }
            //Check if browser is Opera or not
        else if (navigator.userAgent.search("Opera") >= 0) {
            return "Opera";
        }

        function isIE(userAgent) {
            userAgent = userAgent || navigator.userAgent;
            return userAgent.indexOf("MSIE ") > -1 || userAgent.indexOf("Trident/") > -1;
        }
    },
    readypdfRendered: function (win) {
        return new Promise(function (resolve) {
            function checkReady(event) {
                if (win.document.readyState === 'complete') {
                    resolve(win);
                }
            }
            try {
                win.focus();
                win.addEventListener('pagesloaded', checkReady, false);
            } catch (ex) {
                utility.DisplayMessages("Pop-up Blocker is enabled! Please add this site to your exception list.", 2)
                console.log(ex);
            }

        });
    },

    TogglePDF: function ($viewer, $editor) {

        if ($viewer.css("display") == "none") {
            $editor.css("display", "none");
            $viewer.css("display", "block");
        }
        else {
            $viewer.css("display", "none");
            $editor.css("display", "block");
        }
    },

    PDFEditor: function (base64, $viewer, $editor, ObjectControlID, IsIframe) {

        $editor.append("<input id='hfPDFeditor' type='hidden'></input>")
        $editor.find("#hfPDFeditor").val(base64);
        if (IsIframe) {
            $('#' + ObjectControlID).attr('src', 'Scripts/js/pdf/web/pdf-editor.html');
        } else {
            $('#' + ObjectControlID).attr('data', 'Scripts/js/pdf/web/pdf-editor.html');
        }

        utility.TogglePDF($viewer, $editor);
    },

    PDFViewer: function (base64, IsnewTab, ObjectControlID, IsIframe, IsPrint) {
        if (IsnewTab) {

            var byteCharacters = atob(base64);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            var blob = new Blob([byteArray], { type: "application/pdf;base64" });
            var url = URL.createObjectURL(blob);
            var viewerUrl = 'Scripts/js/pdf/web/viewer.html?file=' + encodeURIComponent(url);
            if (IsPrint == null || !IsPrint) {
                window.open(viewerUrl, "popup", "menubar=1,resizable=1,width=500,height=500");
            } else {
                utility.readypdfRendered(open(viewerUrl, "popup", "menubar=1,resizable=1,width=500,height=500")).then(function (win) {
                    //pdf is ready
                    win.focus();
                    win.print();
                });
            }

        }
        else {

            if (utility.UserBrowser() == "Firefox") {
                var byteCharacters = atob(base64);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);
                var blob = new Blob([byteArray], { type: "application/pdf" });
                var blobUrl = URL.createObjectURL(blob);
                if (IsIframe) {
                    $('#' + ObjectControlID).attr('src', blobUrl);
                } else {
                    $('#' + ObjectControlID).attr('data', blobUrl);
                }
            }
            else {
                $('#' + ObjectControlID).parent().find("#helperPDF").val(base64);
                if (IsIframe) {
                    $('#' + ObjectControlID).attr('src', 'Scripts/js/pdf/web/viewer.html');
                } else {
                    $('#' + ObjectControlID).attr('data', 'Scripts/js/pdf/web/viewer.html');
                }
            }
        }

        function base64ToUint8Array(base64) {
            var raw = atob(base64);
            var uint8Array = new Uint8Array(raw.length);
            for (var i = 0; i < raw.length; i++) {
                uint8Array[i] = raw.charCodeAt(i);
            }
            return uint8Array;
        }

        function bindEvent(el, eventName, eventHandler) {
            if (el.addEventListener) {
                el.addEventListener(eventName, eventHandler, false);
            } else if (el.attachEvent) {
                el.attachEvent('on' + eventName, eventHandler);
            }
        }
    },

    SaveOrOpenBlob: function (base64, IsnewTab, ObjectControlID, IsIframe, IsPrint) {
        var byteCharacters = atob(base64);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: "application/pdf" });
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob);
        } else {
            var url = URL.createObjectURL(blob);
            var viewerUrl = 'Scripts/js/pdf/web/viewer.html?file=' + encodeURIComponent(url);
            if (IsPrint == null || !IsPrint) {
                window.open(viewerUrl, "popup", "menubar=1,resizable=1,width=500,height=500");
            } else {
                utility.readypdfRendered(open(viewerUrl, "popup", "menubar=1,resizable=1,width=500,height=500")).then(function (win) {
                    //pdf is ready
                    win.focus();
                    win.print();
                });
            }
        }
    },

    HighLightText: function (ControlId, sourceText) {

        try {
            $("#" + ControlId).html(function (i, html) {
                var regexp, replacement;
                regexp = RegExp('(' + sourceText + ')', 'gi');
                replacement = '<span class="custom-highlight">$1</span>';

                return html.replace(regexp, replacement);
            });
        } catch (ex) {
            console.log(ex);
            return true;
        }


    },

    POSAutoComplete: function (sourceDOM, hiddenField) {
        var myJSON = { "txtCode": $(sourceDOM).val(), "chkActive": "1" };
        var ArrPOS = [];

        Admin_PlaceOfService.SearchPlaceOfService(JSON.stringify(myJSON), 0).done(function (response) {
            if (response.status != false) {
                var data = JSON.parse(response.PlaceOfServiceLoad_JSON);
                $.each(data, function (i, item) {
                    ArrPOS.push({ id: item.POSCode, value: item.POSCode + ' - ' + item.Description });
                });
                $(sourceDOM).autocomplete({
                    autoFocus: true,
                    source: ArrPOS, // pass an array (without a comma)
                    select: function (event, ui) {
                        setTimeout(function () {
                            $(hiddenField).val(ui.item.id);
                            $(sourceDOM).val(ui.item.id);

                        }, 100);
                    }
                });
                $(sourceDOM).autocomplete("search");
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    TOSAutoComplete: function (sourceDOM, hiddenField) {
        var myJSON = { "txtCode": $(sourceDOM).val(), "ddlActive": "1", "txtName": "", "txtDiscription": "" };
        var ArrTOS = [];

        Admin_TypeOfService.SearchTypeOfService(JSON.stringify(myJSON), 0).done(function (response) {
            if (response.status != false) {
                var data = JSON.parse(response.TypeOfServiceLoad_JSON);
                $.each(data, function (i, item) {
                    ArrTOS.push({ id: item.TypeOfServiceCode, value: item.TypeOfServiceCode + ' - ' + item.Description });
                });
                $(sourceDOM).autocomplete({
                    autoFocus: true,
                    source: ArrTOS, // pass an array (without a comma)
                    select: function (event, ui) {
                        setTimeout(function () {
                            $(hiddenField).val(ui.item.id);
                            $(sourceDOM).val(ui.item.id);

                        }, 100);
                    }
                });
                $(sourceDOM).autocomplete("search");
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    AutoEnableAutoCompleteLink: function (frmDOM) {
        $(frmDOM).find('.ui-autocomplete-input').each(function (i, item) {
            if ($(item).val() != "" && $(item).val() != null) {
                if ($(item).parent().parent().find('a').length > 0) {
                    $(item).parent().parent().find('a').show();
                    $(item).parent().parent().find('label').hide();
                }
            }
        })
        $(frmDOM).find(".k-autocomplete").children("input").each(function (i, item) {
            if ($(item).val() != "" && $(item).val() != null) {
                if ($(item).parent().parent().parent().find('a').length > 0) {
                    $(item).parent().parent().parent().find('a').show();
                    $(item).parent().parent().parent().find('label').hide();
                }
            }
        })
    },


    BindModifierField: function (obj) {

        $(obj).attr('maxlength', '11');
        $(obj).on("keypress", function (e) {

            var specialKeys = new Array();
            specialKeys.push(8); //Backspace
            specialKeys.push(9); //Tab
            specialKeys.push(46); //Delete
            specialKeys.push(36); //Home
            specialKeys.push(35); //End
            specialKeys.push(37); //Left
            specialKeys.push(39); //Right

            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));

            var val_ = $(this).val();
            var len_ = val_.length;

            if (keyCode != 8) {
                if (len_ == 2 || len_ == 5 || len_ == 8) {
                    $(this).val(val_ + ",");
                }
            }

            return ret;

        });

        $(obj).on("blur", function (e) {
            var temp = "";
            var len_ = $(this).val().split(',');
            for (i = 0; i < len_.length; i++) {
                if (len_[i].length == 2)
                    temp += len_[i] + ",";
            }
            $(this).val(temp.slice(0, -1));
        });

    },

    Keyupdelay: function (callback) {
        clearTimeout(utility.timer);
        utility.timer = setTimeout(function () {
            //method
            callback();
            utility.timer = 0;
        }, 1000);
    },
    KeyupdelayForModifiers: function (callback) {
        clearTimeout(utility.timer);
        utility.timer = setTimeout(function () {
            //method
            callback();
            utility.timer = 0;
        }, 100);
    },
    basr64URLtoBlob: function (dataURL) {
        // Decode the dataURL
        var binary = atob(dataURL.split(',')[1]);
        // Create 8-bit unsigned array
        var array = [];
        for (var i = 0; i < binary.length; i++) {
            array.push(binary.charCodeAt(i));
        }
        // Return our Blob object
        return new Blob([new Uint8Array(array)], { type: 'image/jpg' });
    },

    BindInsurancePlan: function (obj, hfcontrolid, ContainerCtrl) {

        utility.Keyupdelay(function () {
            CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
                $(obj).autocomplete({
                    autoFocus: true,
                    source: InsurancePlans,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#" + ContainerCtrl + " #" + hfcontrolid).val(ui.item.id);
                            $(obj).val(ui.item.value);
                        }, 100);
                    }
                });
            });
        });
    },

    ValidateCtrlMask: function (fromId, obj) {

        if ($(obj).val().indexOf('_') >= 0) {
            $(obj).val('');
            $("#" + fromId).bootstrapValidator('revalidateField', $(obj).attr("name"));
        }
    },

    replaceSpecialCharacters: function (data_) {

        String.prototype.replaceAll = function (target, replacement) {
            return this.split(target).join(replacement);
        };

        return data_.replaceAll("&", "%*%");
    },
    replaceSymbols: function (data_, oldSymbols, newSymbols) {

        String.prototype.replaceAll = function (target, replacement) {
            return this.split(target).join(replacement);
        };

        //return data_.replaceAll("%", "!per");
        return data_.replaceAll(oldSymbols, newSymbols);
    },
    onlyUnique: function (value, index, self) {
        return self.indexOf(value) === index;
    },

    LeapYear: function (year) {
        return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
    },
    UpdateUserCashe: function (JsonData) {
        if (JsonData.HaveChage == true) {
            var temp_ = JsonData.ChangeSet.split(',');
            temp_ = temp_.filter(utility.onlyUnique);
            for (var i = 0; i < temp_.length; i++) {
                CacheManager.BindCodes(temp_[i], true);
            }
        }
    },

    DateCompare: function (DateA, DateB) {     // this function is good for dates > 01/01/1970

        var a = new Date(DateA);
        var b = new Date(DateB);

        var msDateA = Date.UTC(a.getFullYear(), a.getMonth() + 1, a.getDate());
        var msDateB = Date.UTC(b.getFullYear(), b.getMonth() + 1, b.getDate());

        if (parseFloat(msDateA) < parseFloat(msDateB))
            return -1;  // lt
        else if (parseFloat(msDateA) == parseFloat(msDateB))
            return 0;  // eq
        else if (parseFloat(msDateA) > parseFloat(msDateB))
            return 1;  // gt
        else
            return null;  // error
    },

    getDateDiff: function (date1, date2, interval) {
        var second = 1000,
        minute = second * 60,
        hour = minute * 60,
        day = hour * 24,
        week = day * 7;
        date1 = new Date(date1);
        date2 = (date2 == 'now') ? new Date() : new Date(date2);
        var date1_time = new Date(date1).getTime();
        var date2_time = (date2 == 'now') ? new Date().getTime() : new Date(date2).getTime();
        var timediff = date2_time - date1_time;
        if (isNaN(timediff)) return NaN;
        switch (interval) {
            case "years":
                return date2.getFullYear() - date1.getFullYear();
            case "months":
                return ((date2.getFullYear() * 12 + date2.getMonth()) - (date1.getFullYear() * 12 + date1.getMonth()));
            case "weeks":
                return Math.floor(timediff / week);
            case "days":
                return Math.floor(timediff / day);
            case "hours":
                return Math.floor(timediff / hour);
            case "minutes":
                return Math.floor(timediff / minute);
            case "seconds":
                return Math.floor(timediff / second);
            default:
                return undefined;
        }
    },

    getFullDate: function (indate) {
        try {
            var options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };

            //var options = {
            //    weekday: "long", year: "numeric", month: "short",
            //    day: "numeric", hour: "2-digit", minute: "2-digit"
            //};

            return indate.toLocaleDateString("en-US", options);
        } catch (e) {
            console.log(e.message);
            return indate.toDateString();
        }

    },

    //******************* AMOUNT/FIGURE CONVERSION (start) *****************************//

    convertToFigure: function (value, isAmount, isCredit) {

        var figure = (value == null || value == "" || value == undefined || isNaN(value)) ? 0 : value;

        if (figure > -0.001 && figure < 0.001)
            figure = 0;

        figure = Number(figure).toFixed(globalAppdata.DecimalPlaces);

        if (isCredit && isCredit == true)
            figure = "(" + figure + ")";

        if (isAmount && isAmount == true)
            figure = globalAppdata.DefaultCurrency + String(figure)

        return figure;

    },
    //******************* AMOUNT/FIGURE CONVERSION (end) *****************************//

    CalculatePeriod: function (startDate, endDate) {
        var sdate = startDate;
        var edate = endDate;
        edate.setDate(edate.getDate() + 1);
        edate = new Date(edate);
        if (sdate.valueOf() > edate.valueOf()) {
            return ('0');
        }
        else {
            var years = ((((edate.getDate() - sdate.getDate()) < 0 ? -1 : 0) + ((edate.getMonth() + 1) - (sdate.getMonth() + 1))) < 0 ? -1 : 0) + (edate.getFullYear() - sdate.getFullYear());
            var months = ((((edate.getDate() - sdate.getDate()) < 0 ? -1 : 0) + ((edate.getMonth() + 1) - (sdate.getMonth() + 1))) < 0 ? 12 : 0) + ((edate.getDate() - sdate.getDate()) < 0 ? -1 : 0) + ((edate.getMonth() + 1) - (sdate.getMonth() + 1));
            if ((edate.getMonth() - 1) != 1.0) {
                var days = ((edate.getDate() - sdate.getDate()) < 0 ? new Date(edate.getFullYear(), edate.getMonth(), 0).getDate() : 0) + (edate.getDate() - sdate.getDate());
            }
            else {
                var days = ((edate.getDate() - sdate.getDate()) < 0 ? new Date(edate.getFullYear(), edate.getMonth() + 1, 0).getDate() : 0) + (edate.getDate() - sdate.getDate());

                var day;
                var month;
                var year;
                if (years > 1) year = years + 'Years';
                else year = years + 'Year';
                if (months > 1) month = months + 'Months';
                else month = months + 'Month';
                if (days > 1) day = days + 'Days';
                else day = days + 'Day';
                if (years == 0 && months != 0 && days != 0) return (month + ', ' + day);
                else if (years != 0 && months == 0 && days != 0) return (year + ', ' + day);
                else if (years != 0 && months != 0 && days == 0) return (year + ', ' + month);
                else if (years == 0 && months == 0 && days != 0) return (day);
                else if (years == 0 && months != 0 && days == 0) return (month);
                else if (years != 0 && months == 0 && days == 0) return (year);
                else if (years == 0 && months == 0 && days == 0) return (day);
                else if (years != 0 && months != 0 && days != 0) return (year + ', ' + month + ', ' + day);
            }
        }
    },


    PatientDemographics: function (patientid, ParentCtrl, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                if (ParentCtrl.id == undefined) {
                    params["ParentCtrl"] = ParentCtrl;
                } else {
                    params["ParentCtrl"] = ParentCtrl.id;
                }
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    LoadVisitDetail: function (VisitId, PatientId, ParentCtrl, event, hyperlink) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = ParentCtrl;

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;
                params["hyperlink"] = hyperlink;
                LoadActionPan('EncounterChargeCapture', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenPatientCharges: function (ParentCtrl, Field) {

        var params = [];
        params["FromAdmin"] = 0;
        params["Field1"] = Field;
        params["ParentCtrl"] = ParentCtrl;
        LoadActionPan('Bill_ChargeSearch', params);
    },

    ShowHistory: function (ParentControlId, PanelID, ProfileName, DBTableName, ColumnKeyId) {
        var params = [];
        params["PanelID"] = PanelID;
        params['ParentCtrl'] = ParentControlId;
        params["ProfileName"] = ProfileName;
        params["DBTableName"] = DBTableName;
        params['AdminHistory'] = "1";
        params["ColumnKeyId"] = ColumnKeyId;

        LoadActionPan('Activity_Log', params);
    },

    FillVisitFromSearch: function (ClaimNumber, Field, VisitId, HiddenField, event) {
        if (event != null) {
            event.stopPropagation();
        }

        $(Field).val(ClaimNumber);
        $(HiddenField).val(VisitId);
        if ($(Field).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($(Field), ClaimNumber, $(HiddenField), VisitId, "ClaimNumber");
        Bill_ChargeSearch.UnLoad();
    },

    InsertRecentPatient: function (patientId) {

        utility.RecentPatientAdd(patientId).done(function (result) {
            //primary key
        });

    },

    RecentPatientAdd: function (patID) {
        //var objData = new
        var objData = new Object();
        objData["PatientID"] = patID;
        objData["CommandType"] = "insert_recent_patient";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "RecentPatient");
    },

    //******************** Start Referring Provider *******************\\

    GetRefProviderArray: function (name, IsGetAll) {
        var AllRefProviders = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 1)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }
        var dfd = new $.Deferred();
        if (IsValid) {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Referring Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            referringproviderDetail.LoadRefProvidersDBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.ReferringProviderCount > 0) {
                        var ReferringProviderLoadJSONData = JSON.parse(responseData.ReferringProviderLoad_JSON);
                        $.each(ReferringProviderLoadJSONData, function (i, item) {

                            AllRefProviders.push({ id: item.ReferringProviderId, value: item.FirstName });

                        });
                    }
                }
                dfd.resolve(AllRefProviders);
            });
            //}
            //else {
            //    dfd.resolve(AllRefProviders);
            //}
            //});
        }
        else {
            dfd.resolve(AllRefProviders);
        }

        return dfd.promise();

    },

    //******************** End Referring Provider *******************\\

    //******************** Start Facility *******************\\

    GetFacilityArray: function (name, IsGetAll) {

        var AllFacilities = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 1)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }
        var dfd = new $.Deferred();
        if (IsValid) {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Facility", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            Admin_Facility.LoadFacilityDBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.FacilityCount > 0) {
                        var FacilityLoadJSONData = JSON.parse(responseData.FacilityLoad_JSON);
                        $.each(FacilityLoadJSONData, function (i, item) {

                            AllFacilities.push({ id: item.FacilityId, value: item.ShortName, OrgId: item.OrgId, Location: item.Location, Practice: item.Practice, PracticeId: item.PracticeId });

                        });
                    }
                }
                dfd.resolve(AllFacilities);
            });
            //}
            //else {
            //    dfd.resolve(AllFacilities);
            //}
            //});
        }
        else {
            dfd.resolve(AllFacilities);
        }

        return dfd.promise();

    },
    GetProvidersDiagnosticImagingFacilityArray: function (name, ProviderId) {
        var AllFacilities = [];
        var IsValid = false;
        var dfd = new $.Deferred();
        utility.LoadProvidersDiagnosticImagingFacilityDBCall(name, ProviderId).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.FacilityCount > 0) {
                    var FacilityLoadJSONData = JSON.parse(responseData.FacilityLoad_JSON);
                    $.each(FacilityLoadJSONData, function (i, item) {
                        AllFacilities.push({ id: item.FacilityId, value: item.ShortName, OrgId: item.OrgId, Location: item.Location, Practice: item.Practice, PracticeId: item.PracticeId });
                    });
                }
            }
            dfd.resolve(AllFacilities);
        });
        return dfd.promise();
    },
    LoadProvidersDiagnosticImagingFacilityDBCall: function (name, ProviderId) {
        var data = "ShortName=" + name + "&ProviderId=" + ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY", "LOAD_PROVIDERS_DIAGNOSTIC_IMAGING_FACILITY_LOOKUP");
    },

    GetFacilityDescriptionArray: function (name, IsGetAll) {

        var AllFacilities = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 1)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }
        var dfd = new $.Deferred();
        if (IsValid) {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Facility", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            Admin_Facility.LoadFacilityDescriptionDBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.FacilityCount > 0) {
                        var FacilityLoadJSONData = JSON.parse(responseData.FacilityLoad_JSON);
                        $.each(FacilityLoadJSONData, function (i, item) {

                            AllFacilities.push({ id: item.FacilityId, value: item.FacilityDescription, OrgId: item.OrgId, Location: item.Location, Practice: item.Practice, PracticeId: item.PracticeId });
                        });
                    }
                }
                dfd.resolve(AllFacilities);
            });
            //}
            //else {
            //    dfd.resolve(AllFacilities);
            //}
            //});
        }
        else {
            dfd.resolve(AllFacilities);
        }

        return dfd.promise();

    },

    //******************** End Facility *******************\\

    //******************** Start Provider *******************\\

    GetProviderArray: function (name, IsGetAll) {
        var AllProviders = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 1)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }
        var dfd = new $.Deferred();

        if (IsValid) {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            Admin_Provider.LoadProviderDBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.ProviderCount > 0) {
                        var ProviderLoadJSONData = JSON.parse(responseData.ProviderLoad_JSON);
                        $.each(ProviderLoadJSONData, function (i, item) {

                            AllProviders.push({ id: item.ProviderId, value: item.ShortName });

                        });
                    }
                }
                dfd.resolve(AllProviders);
            });
            //    }
            //    else {
            //        dfd.resolve(AllProviders);
            //    }
            //});
        }
        else {
            dfd.resolve(AllProviders);
        }

        return dfd.promise();

    },

    //******************** End Provider *******************\\

    //******************** Start Target Site *******************\\

    GetTargetSiteArray: function (TargetSite, IsGetAll) {
        var AllTargetSites = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (TargetSite != null && TargetSite.length > 1)
                IsValid = true;
            else
                IsValid = false;
        }
        else
            IsValid = true;

        var dfd = new $.Deferred();
        if (IsValid) {
            Clinical_ImplantableDetail.LoadTargetSiteDBCall(TargetSite).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.TargetSiteCount > 0) {
                        var TargetSiteLoadJSONData = JSON.parse(responseData.TargetSiteLoad_JSON);
                        $.each(TargetSiteLoadJSONData, function (i, item) {
                            AllTargetSites.push({ id: item.Id, value: item.Code + " " + item.Name });
                        });
                    }
                }
                dfd.resolve(AllTargetSites);
            });
        }
        else
            dfd.resolve(AllTargetSites);
        return dfd.promise();
    },

    //******************** End Target Site *******************\\

    GetInsurancePlanArray: function (name, IsGetAll) {

        var AllInsurancePan = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 1)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }
        var dfd = new $.Deferred();
        if (IsValid) {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Facility", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            Admin_InsurancePlan.LoadInsuranceDBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.PlanCount > 0) {
                        var PlanLoad_JSONData = JSON.parse(responseData.PlanLoad_JSON);
                        $.each(PlanLoad_JSONData, function (i, item) {

                            AllInsurancePan.push({ id: item.InsurancePlanId, value: item.ShortName });

                        });
                    }
                }
                dfd.resolve(AllInsurancePan);
            });
            //}
            //else {
            //    dfd.resolve(AllFacilities);
            //}
            //});
        }
        else {
            dfd.resolve(AllInsurancePan);
        }

        return dfd.promise();

    },
    GetPreferedLanguagesArray: function (name) {
        var AllPreferedLanguages = [];
        var IsValid = true;
        var dfd = new $.Deferred();
        if (name) {
            utility.LoadPreferedLanguages_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.PreferedlanguagesCount > 0) {
                        var PreferedlanguagesLoad_JSON = JSON.parse(responseData.PreferedlanguagesLoad_JSON);
                        $.each(PreferedlanguagesLoad_JSON, function (i, item) {
                            AllPreferedLanguages.push({ id: item.Id, value: item.Description });
                        });
                    }
                }
                dfd.resolve(AllPreferedLanguages);
            });
        }
        else {
            dfd.resolve(AllPreferedLanguages);
        }

        return dfd.promise();
    },
    GetCountriesArray: function (name) {
        var AllCountries = [];
        var IsValid = true;
        var dfd = new $.Deferred();
        if (name) {
            utility.LoadCountries_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.CountriesCount > 0) {
                        var CountriesLoad_JSON = JSON.parse(responseData.CountriesLoad_JSON);
                        $.each(CountriesLoad_JSON, function (i, item) {
                            AllCountries.push({ id: item.Id, value: utility.titleCase(item.Description) }); //change country search list text from Upper to Title Case
                        });
                    }
                }
                dfd.resolve(AllCountries);
            });
        }
        else {
            dfd.resolve(AllCountries);
        }

        return dfd.promise();
    },
    GetPracticeArray: function (name, IsGetAll) {

        var AllPractices = [];
        var IsValid = true;
        //if (IsGetAll === undefined) {
        //    if (name != null && name.length > 1)
        //        IsValid = true;
        //    else
        //        IsValid = false;
        //} else {
        //    IsValid = true;
        //}
        var dfd = new $.Deferred();
        if (IsValid) {

            //var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Facility", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            Admin_Practice.LoadPracticeDBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.PracticeCount > 0) {
                        var PracticeLoadJSONData = JSON.parse(responseData.PracticeLoad_JSON);
                        $.each(PracticeLoadJSONData, function (i, item) {

                            AllPractices.push({ id: item.PracticeId, value: item.ShortName });

                        });
                    }
                }
                dfd.resolve(AllPractices);
            });
            //}
            //else {
            //    dfd.resolve(AllFacilities);
            //}
            //});
        }
        else {
            dfd.resolve(AllPractices);
        }

        return dfd.promise();

    },
    GetCitiesArray: function (name) {
        var AllCities = [];
        var IsValid = true;
        var dfd = new $.Deferred();
        if (name) {
            utility.LoadCities_DBCall(name).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.CitiesCount > 0) {
                        var CitiesLoad_JSON = responseData.CitiesLoad_JSON;
                        $.each(CitiesLoad_JSON, function (i, item) {
                            AllCities.push({ id: item.Zip, value: utility.titleCase(item.Description) + '-' + item.Zip }); //change city search list text from Upper to Title Case
                        });
                    }
                }
                dfd.resolve(AllCities);
            });
        }

        else {
            dfd.resolve(AllCities);
        }

        return dfd.promise();
    },
    LoadCities_DBCall: function (name) {
        var objData = new Object();
        objData["name"] = name;
        objData["CommandType"] = "get_cities_by_name";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "LanguagesAndCountries");
    },
    LoadPreferedLanguages_DBCall: function (name) {
        var objData = new Object();
        objData["name"] = name;
        objData["CommandType"] = "get_prefered_languages_by_name";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "LanguagesAndCountries");
    },
    LoadCountries_DBCall: function (name) {
        var objData = new Object();
        objData["name"] = name;
        objData["CommandType"] = "get_countries_by_name";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "LanguagesAndCountries");
    },
    // Set AutoComplete Source for single object mostly in case of Screen load
    SetAutoCompleteSource: function ($Ctrl, $hfCtrl, src_obj) {

        var Id = $hfCtrl.val();
        var Name = $Ctrl.val();
        if (Id && Name) {
            var source_ = [];
            try {

                source_ = $($Ctrl).autocomplete("option", "source");

                if (typeof (source_) == 'function') {
                    var obj = { term: "" }
                    source_(obj, function (res) {
                        source_ = res;
                    });

                }
                if (Admin_Provider.params.ParentCtrl == "ClinicalLabOrderDetail" && source_.length == null) {
                    source_ = JSON.parse(source_.GetProvider);
                }
                if (src_obj) {
                    var arr = $.grep(source_, function (a) {
                        return JSON.stringify(a) === JSON.stringify(src_obj);
                    });
                    if (arr.length <= 0)
                        source_.push(src_obj);
                }
                else {
                    var obj_ = { id: Id, value: Name };
                    if (source_.length > 0) {
                        arr = $.grep(source_, function (a) {
                            return JSON.stringify(a) === JSON.stringify(obj_);
                        });
                        if (arr.length <= 0)
                            source_.push(obj_);
                    }
                    else { source_.push(obj_); }
                }


                $($Ctrl).autocomplete('option', 'source', source_)
            }
            catch (e) {

                if (!Array.isArray(source_)) {
                    source_ = [];
                }

                if (src_obj) {
                    source_.push(src_obj);
                }
                else {
                    source_.push({ id: Id, value: Name });
                }
                $($Ctrl).autocomplete({
                    autoFocus: true,
                    source: source_,
                    select: function (event, ui) { }
                });
            }
        }
    },
    SetAutoCompleteSourceforValidate: function ($Ctrl, cptCode, cptDescription) {
        var ctrlVal = $Ctrl.val();
        if (ctrlVal) {
            var source_ = [];
            try {
                source_ = $($Ctrl).autocomplete("option", "source");
                source_.push({ id: cptCode, value: cptCode + " - " + cptDescription });
                $($Ctrl).autocomplete('option', 'source', source_)
            }
            catch (e) {
                source_.push({ id: cptCode, value: cptCode + " - " + cptDescription });
                $($Ctrl).autocomplete({
                    autoFocus: true,
                    source: source_,
                    select: function (event, ui) { }
                });
            }
        }
    },
    GetDocumentTagArray: function (name, IsGetAll) {

        var AllDocumentTag = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (name != null && name.length > 1)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }
        var dfd = new $.Deferred();
        if (IsValid) {
            Patient_DocumentTag.GetTagsByName(name).done(function (responseData) {
                if (responseData.status != false) {
                    $.each(responseData.DocumentTags, function (i, item) {

                        AllDocumentTag.push({ id: item.TagId, value: item.Name });

                    });

                }
                dfd.resolve(AllDocumentTag);
            });
        }
        else {
            dfd.resolve(AllDocumentTag);
        }

        return dfd.promise();

    },
    /*********************/
    arrayBufferToBase64: function (buffer) {
        var binary = "";
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }

        base64 = window.btoa(binary);
        return "data:image/jpg;base64," + base64;
    },

    formatDate: function (date) {

        var givenDate = new Date(date);

        return (givenDate.getMonth() + 1) + "/" + givenDate.getDate() + "/" + givenDate.getFullYear();




    },
    getpreferenceByPriority: function (PlanPriority) {
        var Preferance = "";
        if (PlanPriority) {
            PlanPriority = Number(PlanPriority);
            switch (PlanPriority) {
                case 1:
                    Preferance = "Primary";
                    break;
                case 2:
                    Preferance = "Secondary";
                    break;
                case 3:
                    Preferance = "Tertiary";
                    break;
                case 4:
                    Preferance = "Quaternary";
                    break;
                case 5:
                    Preferance = "Quinary";
                    break;
                case 6:
                    Preferance = "Senary";
                    break;
                case 7:
                    Preferance = "Septenary";
                    break;
                case 8:
                    Preferance = "Octonary";
                    break;
                case 9:
                    Preferance = "Nonary";
                    break;
                case 10:
                    Preferance = "Denary";
                    break;
                default:
            }
        }
        return Preferance;
    },

    makeRendomKey: function () {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for (var i = 0; i < 5; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    },

    makeFuncArray: function (rendmKey, okFunc, cancelFunc) {
        var Funcs = new Object();
        Funcs["rendmKey"] = rendmKey;
        Funcs["okFunc"] = okFunc;
        Funcs["cancelFunc"] = cancelFunc;
        utility.FuncArray.push(Funcs);
    },
    makeFuncArrayNote: function (rendmKey, saveNote, saveExit, exit) {
        var Funcs = new Object();
        Funcs["rendmKey"] = rendmKey;
        Funcs["saveNote"] = saveNote;
        Funcs["saveExit"] = saveExit;
        Funcs["exit"] = exit;
        utility.FuncArray.push(Funcs);
    },

    removeFromFuncArray: function (rendmKey) {
        if (utility.FuncArray != null) {
            var UpdatedArray = [];
            UpdatedArray = jQuery.grep(utility.FuncArray, function (value) {
                return value.rendmKey != rendmKey;
            });
            utility.FuncArray = UpdatedArray;
        }
    },

    getFuncFromFuncArray: function (rendmKey) {
        var func;
        if (utility.FuncArray != null) {
            for (var i = 0; i < utility.FuncArray.length; i++) {
                if (utility.FuncArray[i].rendmKey == rendmKey) {
                    func = utility.FuncArray[i];
                    break;
                }
            }
        }
        return func;
    },

    GetDayNameFromDayNumber: function (DayNumber) {

        var weekday = new Array(7);
        weekday[0] = "Sunday";
        weekday[1] = "Monday";
        weekday[2] = "Tuesday";
        weekday[3] = "Wednesday";
        weekday[4] = "Thursday";
        weekday[5] = "Friday";
        weekday[6] = "Saturday";
        return weekday[DayNumber];
    },
    /* drop down helping func start*/
    appendLookUpOptions: function (results, method_ddlist, $parent) {

        if ($.type(results) == "string") {
            responseResult = JSON.parse(results);
            name = method_ddlist[0];
            var ddl = $.merge($parent.find('select[ddlist=' + name + ']'),
                    $parent.find('select[ddlmultilist=' + name + ']'),
                    $parent.find('select[ddSuggestionlist=' + name + ']'));
            var l = $(ddl);
            //action pan required contrainer id
            if ($(ddl).length > 0 && $parent.find('#' + $(ddl)[0].id).length > 0) {
                utility.appendMultipleLookups(l, $parent, responseResult);
            } else {
                utility.appendDdlListOptions(l, $parent, responseResult);
            }

        } else if ($.type(results) == "object") {
            for (var name in results) {
                name = (name == null || name == '') ? method_ddlist[0] : name;
                var ddl = $.merge($parent.find('select[ddlist=' + name + ']'),
                    $parent.find('select[ddlmultilist=' + name + ']'),
                    $parent.find('select[ddSuggestionlist=' + name + ']'))
                ;
                var responseResult = $.isArray(results) ? results : JSON.parse(results[name]);
                if (name == 'GetPlaceOfService') {
                    var stringlist = [];
                    var numberlist = [];
                    var selectArray = [];
                    for (var i = 0; i < responseResult.length; i++) {
                        if (responseResult[i].Name.slice(0, 1).match(/\d+/)) {
                            numberlist.push(responseResult[i]);
                        } else {
                            if (responseResult[i].Name == "- Select -") {
                                selectArray.push(responseResult[i])
                            }
                            else {
                                stringlist.push(responseResult[i]);
                            }
                        }

                    }
                    numberlist = numberlist.sort(function (a, b) { return a.Name.split('-')[0] - b.Name.split('-')[0] });
                    stringlist = stringlist.sort(function (a, b) { return a.Name.split('-')[0] - b.Name.split('-')[0] });
                    selectArray = selectArray.concat(numberlist.concat(stringlist));
                    responseResult = selectArray;
                }


                var l = $(ddl);
                //action pan required contrainer id
                if ($(ddl).length > 0 && $parent.find('#' + $(ddl)[0].id).length > 0) {
                    utility.appendMultipleLookups(l, $parent, responseResult);
                } else {
                    utility.appendDdlListOptions(l, $parent, responseResult);
                }
            }
        }
    },

    appendLookUpOptionsWithTitle: function (results, method_ddlist, contrainerid) {

        var $parent = $("#" + contrainerid);

        if ($.type(results) == "string") {
            responseResult = JSON.parse(results);
            name = method_ddlist[0];
            var ddl = $.merge($parent.find('select[ddlist=' + name + ']'),
                     $parent.find('select[ddlmultilist=' + name + ']'),
                     $parent.find('select[ddSuggestionlist=' + name + ']'))
            ;
            var l = $(ddl);
            //action pan required contrainer id
            if ($(ddl).length > 0 && $parent.find('#' + $(ddl)[0].id).length > 0) {
                utility.appendMultipleLookups(l, $parent, responseResult);
            } else {
                utility.appendDdlListOptions(l, $parent, responseResult);
            }

        } else if ($.type(results) == "object") {
            for (var name in results) {
                name = (name == null || name == '') ? method_ddlist[0] : name;
                var ddl = $.merge($parent.find('select[ddlist=' + name + ']'),
                    $parent.find('select[ddlmultilist=' + name + ']'),
                    $parent.find('select[ddSuggestionlist=' + name + ']'))
                ;
                var responseResult = $.isArray(results) ? results : JSON.parse(results[name]);

                var l = $(ddl);
                //action pan required contrainer id
                if ($(ddl).length > 0 && $parent.find('#' + $(ddl)[0].id).length > 0) {
                    utility.appendMultipleLookupsWithTitle(l, contrainerid, responseResult);
                } else {
                    utility.appendDdlListOptions(l, $parent, responseResult);
                }
            }
        }
    },


    appendDdlListOptions: function (l, $parent, responseResult) {
        if (l.id == "LedgerAccount") {
            console.log(responseResult);
        }
        l.empty();
        var isMultiselect = false;
        if ($(l).attr('ddlmultilist') != null && $(l).attr('ddlmultilist') != '') {
            isMultiselect = true;
        }
        $.each(responseResult, function (j, result) {
            if (isMultiselect) {
                if (result.Name.toUpperCase() != "- SELECT -" && result.Name.toUpperCase() != "- Select -") {
                    l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName).attr("IsActive", result.IsActive));
                }
            }
            else {
                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
            }
        });
    },
    appendMultipleLookups: function (l, $parent, responseResult) {
        var optionHTML = null;
        var optionMultiddlHTML = null;
        for (var i = 0; i < l.length; i++) {
            l[i] = $parent.find('#' + $(l)[i].id);
            l[i].empty();
            if ($(l[i]).attr('ddlmultilist') != null && $(l[i]).attr('ddlmultilist') != '') {
                optionHTML = null;
                if (optionMultiddlHTML == null) {
                    $.each(responseResult, function (j, result) {
                        if (result.Name.toUpperCase() != "- SELECT -" && result.Name.toUpperCase() != "- Select -") {
                            l[i].append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName).attr("IsActive", result.IsActive).attr("ExName", result.ExName));
                        }
                    });
                    optionMultiddlHTML = l[i].html();
                } else {
                    l[i].append(optionMultiddlHTML);
                }
            } else {
                optionMultiddlHTML = null
                if (optionHTML == null) {
                    if (l[0].attr('id') == "ddlDefaultFacility") {
                        $.each(responseResult, function (j, result) {
                            l[i].append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName).attr("FacilityDescription", result.ExValue));
                        });
                    } else {
                        $.each(responseResult, function (j, result) {
                            l[i].append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                        });
                    }
                    optionHTML = l[i].html();
                } else {
                    l[i].append(optionHTML);
                }
            }


        }
    },

    appendMultipleLookupsWithTitle: function (l, contrainerid, responseResult) {
        var optionHTML = null;
        var optionMultiddlHTML = null;
        for (var i = 0; i < l.length; i++) {
            l[i] = $('#' + contrainerid + ' #' + $(l)[i].id);
            l[i].empty();
            if ($(l[i]).attr('ddlmultilist') != null && $(l[i]).attr('ddlmultilist') != '') {
                optionHTML = null;
                if (optionMultiddlHTML == null) {
                    $.each(responseResult, function (j, result) {
                        if (result.Name.toUpperCase() != "- SELECT -" && result.Name.toUpperCase() != "- Select -") {
                            l[i].append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName).attr("IsActive", result.IsActive));
                        }
                    });
                    optionMultiddlHTML = l[i].html();
                } else {
                    l[i].append(optionMultiddlHTML);
                }
            } else {
                optionMultiddlHTML = null
                if (optionHTML == null) {
                    $.each(responseResult, function (j, result) {
                        var CareTeamString = "";
                        var CareTeamMembers = "";
                        var CareTeamName = "";
                        var tempTitle = "";
                        if (result.Name != "" || result.Name != null || result.Name != undefined || result.Name != 'undefined') {
                            CareTeamString = result.Name;
                            if (CareTeamString != "- Select -") {
                                if (CareTeamString.indexOf("~_~") >= 0) {
                                    CareTeamName = CareTeamString.split("~_~")[0];
                                    CareTeamMembers = CareTeamString.split("~_~")[1];

                                    if ($.isNumeric(CareTeamMembers) || CareTeamMembers == null || CareTeamMembers == "") {
                                        tempTitle = "";
                                    }
                                    else {
                                        tempTitle = CareTeamMembers;
                                    }
                                }
                            }
                            else {
                                CareTeamName = "- Select -";
                            }
                        }

                        l[i].append($("<option />").val(result.Value).text(CareTeamName).attr("RefValue", result.RefValue).attr("RefName", CareTeamMembers).attr("title", tempTitle));
                    });
                    optionHTML = l[i].html();
                } else {
                    l[i].append(optionHTML);
                }
            }
        }
    },

    timeTo12HrFormat: function (time_) {
        // Take a time in 24 hour format and format it in 12 hour format
        if (time_) {
            var time_part_array = time_.split(":");
            var ampm = 'AM';

            if (time_part_array[0] >= 12) {
                ampm = 'PM';
            }

            if (time_part_array[0] > 12) {
                time_part_array[0] = time_part_array[0] - 12;
            }

            formatted_time = time_part_array[0] + ':' + time_part_array[1] + ':' + time_part_array[2] + ' ' + ampm;

            return formatted_time;
        }
        else
            return "";

    },


    unique: function (array) {
        return $.grep(array, function (el, index) {
            return index == $.inArray(el, array);
        });
    },

    /* drop down helping func ends*/


    callbackAfterAllDOMLoaded: function (callBackFnc) {

        var XHR_var = setInterval(function () { checkXHRCalls() }, 100);
        function checkXHRCalls() {
            if (xhrPool.length <= 0 && $("#BackgroundLoader").css('display') == 'none') {
                //call back method
                if (typeof (callBackFnc) == 'function') {
                    callBackFnc();
                }
                clearInterval(XHR_var);
            }
        }
    },

    kendoConfirm: function (popupTitle, confirmMsg, succCallBackFnc, cancelCallBackFnc) {

        var dialogKey = confirmMsg;

        if (utility.checkKeyExistsDoNotAskedAgain(dialogKey, succCallBackFnc)) {
            return;
        }
        var doNotText = globalAppdata["IsConfigureAlerts"] == "True" ? '<div class="pull-left"> <div class="checkbox-custom"><input id="donotaskedagain"  type="checkbox" value="' + dialogKey + '" />   <label class="control-label" for="donotaskedagain"> Do not ask again</label></div> </div>' : "";
        var rendomKey = utility.makeKendoKey();
        var continueConfirm = ' <script id="Confirm' + rendomKey + '" type="text/x-kendo-template">'
        + '<div class="spacer15"></div><p class="continue-message ">' + confirmMsg + '</p>'
        + '<div class="modal-footer text-right p-default">'
            + doNotText
            + '<button class="confirm' + rendomKey + ' btn btn-success">Yes</button>'
            + '<button class="cancel' + rendomKey + ' btn btn-danger">No</button>'
        + '</div>'
         + '</script>';
        $("<div id='kendoConfirmDiv_" + rendomKey + "'/>").kendoWindow({
            width: "550px",
            title: popupTitle,
            resizable: false,
            modal: true,
            draggable: false,
            open: function (e) {
                e.sender.wrapper.find('.k-i-close').closest('a').addClass('close');
            },
            animation: {
                open: {
                    effects: "slideIn:down fadeIn",
                    duration: 500
                },
                close: {
                    effects: "slideIn:down fadeIn",
                    reverse: true,
                    duration: 500
                }
            },
            deactivate: function () {
                this.destroy();
            },
        });
        if ($("#donotaskedagain:checked").length > 0) {
            $("#donotaskedagain").val(dialogKey);
            utility.AddKey2LocalStorageForDoNotAskedAgain();
        }

        $('#kendoConfirmDiv_' + rendomKey).data("kendoWindow")
           .content($(continueConfirm).html())
           .center().open();

        $('#kendoConfirmDiv_' + rendomKey).find(".confirm" + rendomKey + ",.cancel" + rendomKey)
            .click(function () {
                if ($(this).hasClass("confirm" + rendomKey)) {
                    if (typeof (succCallBackFnc) == 'function') {
                        succCallBackFnc();
                    }
                }
                else {
                    if (cancelCallBackFnc && typeof (cancelCallBackFnc) == 'function') {
                        cancelCallBackFnc();
                    }
                }
                $('#kendoConfirmDiv_' + rendomKey).data("kendoWindow").close();
            })
            .end();


    },
    makeKendoKey: function () {
        var text = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        for (var i = 0; i < 10; i++)
            text += possible.charAt(Math.floor(Math.random() * possible.length));

        return text;
    },

    kendoAlert: function (popupTitle, confirmMsg, succBtnName, succCallBackFnc, cancelBtnName, cancelCallBackFnc) {
        var rendomKey = utility.makeKendoKey();
        var succBtn = "";
        if (succBtnName) {
            succBtn = '<button class="confirm' + rendomKey + ' btn btn-success">' + succBtnName + '</button>';
        }
        var cancelBtn = "";
        if (cancelBtnName) {
            cancelBtn = '<button class="cancel' + rendomKey + ' btn btn-danger">' + cancelBtnName + '</button>';
        }
        var continueConfirm = ' <script id="Confirm' + rendomKey + '" type="text/x-kendo-template">'
        + '<p class="continue-message p-md pl-default">' + confirmMsg + '</p>'
        + '<div class="modal-footer text-right p-default">'
            + succBtn
            + cancelBtn
        + '</div>'
         + '</script>';
        $("<div id='kendoAlertDiv_" + rendomKey + "'/>").kendoWindow({
            width: "550px",
            title: popupTitle,
            resizable: false,
            modal: true,
            draggable: false,
            open: function (e) {
                e.sender.wrapper.find('.k-i-close').closest('a').addClass('close');
            },
            animation: {
                open: {
                    effects: "slideIn:down fadeIn",
                    duration: 500
                },
                close: {
                    effects: "slideIn:down fadeIn",
                    reverse: true,
                    duration: 500
                }
            },
            deactivate: function () {
                this.destroy();
            },
        });
        $('#kendoAlertDiv_' + rendomKey).data("kendoWindow")
           .content($(continueConfirm).html())
           .center().open();

        $('#kendoAlertDiv_' + rendomKey).find(".confirm" + rendomKey + ",.cancel" + rendomKey)
            .click(function () {
                if ($(this).hasClass("confirm" + rendomKey)) {
                    if (typeof (succCallBackFnc) == 'function') {
                        succCallBackFnc();
                    }
                }
                else {
                    if (cancelCallBackFnc && typeof (cancelCallBackFnc) == 'function') {
                        cancelCallBackFnc();
                    }
                }
                $('#kendoAlertDiv_' + rendomKey).data("kendoWindow").close();
            })
            .end();
    },

    documentPrint: function (documentData) {
        var raw = atob(documentData);

        var uint8Array = new Uint8Array(raw.length);
        for (var i = 0; i < raw.length; i++) {
            uint8Array[i] = raw.charCodeAt(i);
        }
        var byteArray = new Uint8Array(uint8Array);

        var blob = new Blob([byteArray], {
            type: 'application/pdf'
        });


        var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
        var url = URL.createObjectURL(blob);

        //for Internet Explorer
        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            //try {
            window.navigator.msSaveOrOpenBlob(blob);
            //window.open(url, "MsgWindow", 'width=' + width + ', height=' + height);
            //}
            //catch (ex) {
            //    window.navigator.msSaveOrOpenBlob(blob);
            //    console.log(ex);
            //}
        }
        else {
            var myWindow = window.open(url, "MsgWindow", 'width=' + width + ', height=' + height);
            myWindow.focus();
            myWindow.print();
        }
        return false;
    },

    LoadFileData: function (base64, pnlID) {
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
            utility.PDFViewer(filesData, false, pnlID);
        });
    },

    InitKendoRaceAutoComplete: function (ControlId, hfId) {
        var currentItem = '';
        //$('#pnlDemographic #ddlPatientRace')
        //$('#' + Patient_Demographic.params["PanelID"] + ' #hfRaceIds')
        !(function ($) {
            var checkInputs = function (elements) {
                elements.each(function () {
                    var element = $(this);
                    var input = element.children("input");

                    input.prop("checked", element.hasClass("k-state-selected"));
                });
            };

            $('#' + ControlId).kendoMultiSelect({
                autoClose: false,
                placeholder: "Select Race..",
                filter: "contains",
                dataTextField: "Name",
                dataValueField: "Value",
                dataSource: new kendo.data.DataSource({
                    schema: {
                        data: "data"
                    },
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            var Name = $('#' + ControlId).data("kendoMultiSelect")._prev;
                            MDVisionService.APIService(Name, "Patient", "GetPatientRaces").done(function (response) {
                                var Races = {
                                    data: []
                                };
                                var globalAppdataRaceIdsArr = [];
                                if (globalAppdata.RaceIds)
                                    globalAppdataRaceIdsArr = globalAppdata.RaceIds.split(',');
                                var data_ = jQuery.parseJSON(response);
                                var temp_ = [];
                                for (var i = 0; i < data_.length; i++) {
                                    if (!data_[i].RefValue) {
                                        var filter_data = $.grep(globalAppdataRaceIdsArr, function (item, j) {
                                            return item == data_[i].Value
                                        });
                                        if (filter_data.length != 0)
                                            temp_.push(data_[i]);
                                    }
                                    else
                                        temp_.push(data_[i]);
                                }

                                try {
                                    // set existing data with new filtered data
                                    var app_data = $('#' + ControlId).data("kendoMultiSelect").dataSource.data();
                                    for (var i = 0; i < app_data.length; i++) {

                                        var filter_data = $.grep(temp_, function (item, j) {
                                            return item.Name == app_data[i].Name
                                            && item.Value == app_data[i].Value
                                        });

                                        if (filter_data.length == 0) {
                                            temp_.push(app_data[i]);
                                        }
                                    }

                                } catch (e) {
                                    console.log(e.message);
                                }


                                Races.data = temp_;
                                //Refvalue
                                //globalAppdata.RaceIds
                                e.success(Races);
                            });
                        }
                    }
                }),
                //minLength: 3,
                separator: ", ",
                select: function (e) {
                    currentItem = e.item.text();
                },
                change: function (e) {
                    var oldobject;
                    var data = $('#' + ControlId).data("kendoMultiSelect");
                    var SelectedArr = data.dataItems();
                    if (SelectedArr) {
                        if (currentItem == "Declined to Specify" || currentItem == "Unreported/Refused to Report") {
                            oldobject = $.grep(SelectedArr, function (b) {
                                return (b.Name == "Declined to Specify" || b.Name == "Unreported/Refused to Report");
                            });
                            data.value([]);
                        }
                        else {
                            oldobject = $.grep(SelectedArr, function (b) {
                                return (b.Name != "Declined to Specify" && b.Name != "Unreported/Refused to Report");
                            });
                        }
                        if (oldobject) {
                            var values_ = new Array();
                            for (var i = 0; i < oldobject.length; i++)
                                values_[i] = oldobject[i].Value;
                            data.value(values_);
                        }
                    }
                    $('#' + hfId).val(e.sender._old.join(','));
                },
            });
        })($);
    },
    //17-5-2018 TahreemMalik MA-401: initialize kendo multi-select for selection of patients by name ** Begin **
    InitKendoPatAccountAutoComplete: function (ControlId, hfId) {
        var currentItem = '';
        !(function ($) {
            var checkInputs = function (elements) {
                elements.each(function () {
                    var element = $(this);
                    var input = element.children("input");
                    input.prop("checked", element.hasClass("k-state-selected"));
                });
            };

            $('#' + ControlId).kendoMultiSelect({
                autoClose: false,
                placeholder: "Start typing to search",
                filter: "contains",
                dataTextField: "FullName",
                dataValueField: "PatientId",
                dataSource: new kendo.data.DataSource({
                    schema: {
                        data: "data"
                    },
                    serverFiltering: true,
                    transport: {
                        read: function (e) {
                            var Name = $('#' + ControlId).data("kendoMultiSelect")._prev;
                            appointmentDetail.LoadActivePatientsByName(Name).done(function (response) {
                                var PatientData = {
                                    data: []
                                };
                                if (response.status != false) {
                                    if (response.PatientCount > 0) {
                                        var data_ = jQuery.parseJSON(response.PatientLoad_JSON);
                                        PatientData.data = data_;
                                        e.success(PatientData);
                                    } else {
                                        e.error("record not found.");
                                    }
                                }
                                else {
                                    e.error("record not found.");
                                }
                            });
                        }
                    }
                }),
                separator: ", ",
                select: function (e) {
                    currentItem = e.item.text();
                },
                change: function (e) {
                    $('#' + hfId).val(e.sender._old.join(','));
                },
            });
        })($);
    },
    //17-5-2018 TahreemMalik MA-401: initialize kendo multi-select for selection of patients by name ** End **
    allowNumbersOnly: function (cntrl) {
        var regex = /^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$/;
        if ($(cntrl).val() && !regex.test($(cntrl).val())) {
            $(cntrl).val("");
            utility.DisplayMessages('Please enter only number', 3);
        }
    },
    numbersNotAllowed: function (cntrl) {
        var regex = /^[a-zA-Z.,;:|\\\/~!@#$%^&*_-{}\[\]()`"'<>?\s]+$/;
        if ($(cntrl).val() && !regex.test($(cntrl).val())) {
            $(cntrl).val("");
        }
    },
    //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show in kendo/datatable grids. MA-537
    removePaginationFromGrid: function (Ctrl) {
        if ($(Ctrl).find('.dataTables_empty').length > 0)
            $(Ctrl).find('[class*=datatables-footer]').addClass('hidden');

        else if ($(Ctrl).find('tbody tr').length > 0 && $(Ctrl).is('.k-grid'))
            $(Ctrl).find('[class*=k-pager-wrap]').removeClass('hidden');

        else if ($(Ctrl).find('.k-grid-norecords-template').length > 0 || $(Ctrl).is('.k-grid'))
            $(Ctrl).find('[class*=k-pager-wrap ]').addClass('hidden');
    },
    //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show in kendo/datatable grids. MA-537

    toggelMU3Alerts: function (isShow, MissingDataAlertsCount) {
        var btnAlert = '<a href="#" style="text-decoration:none" onclick="OpenMU3Alerts()"> <span id="MissingDataAlertsLabel" class=" btnMU3Alert animated infinite flash btn-flash btn-danger btn-xs tab_space d-inline-block pt-tiny pb-tiny ml-xxs">Missing Data Alert(s)</span></a>';

        if (isShow == true) {
            if ($("#PatientProfile #lblPatientData").find(".btnMU3Alert").length > 0) {
                $("#PatientProfile #lblPatientData").find(".btnMU3Alert").css("display", "inline");
                $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").text('MIPS - ' + MissingDataAlertsCount + ' Missing Data Alert(s)');
                if (MissingDataAlertsCount > 0)
                    $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").css("display", "inline");
                else
                    $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").css("display", "none");
            }
            else {
                var selector = "";
                if (('#PatientProfile #lblPatientData:first-child button').length > 0) {
                    selector = '#PatientProfile #lblPatientData:first-child button:first';
                }
                else {
                    selector = '#PatientProfile #lblPatientData:first-child a';
                }
                $(btnAlert).insertAfter(selector);

                $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").text('MIPS - ' + MissingDataAlertsCount + ' Missing Data Alert(s)');
                if (MissingDataAlertsCount > 0)
                    $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").css("display", "inline");
                else
                    $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").css("display", "none");
            }
        }
        else {
            $("#PatientProfile #lblPatientData").find(".btnMU3Alert").css("display", "none");
            $("#PatientProfile #lblPatientData").find("#MissingDataAlertsLabel").css("display", "none");
        }
    },

    LoadMUAlerts: function (PatientId, IsShow, Type, Profiles) {

        MU_Alerts.LoadMUAlert_DBCALL(PatientId, IsShow, Type, Profiles).done(function (result) {
            if (result.status) {
                var data = JSON.parse(result.MUAlerts_JSON);
                if (data) {
                    var IsAnyAlert = data.filter(item=>item.PatientId + "" == PatientId && item.IsShowAlert == true);
                    if (IsAnyAlert.length > 0 && data[0].MissingDataAlertCount > 0) {
                        utility.toggelMU3Alerts(true, data[0].MissingDataAlertCount);
                    }
                    else {
                        utility.toggelMU3Alerts(false, 0);
                    }
                }
                else
                    utility.toggelMU3Alerts(false, 0);
            }

        });

    },

    ValidateMU3Demographics: function (PanelId) {

        // Commented as requirement may be need in future, so do not remove code. PRD-795
        //$(PanelId + " #ddlSexualOrientation,#ddlGenderIdentity,#ddlBirthSex,#ddlCountry,#ddlPrefAddress,#ddlPreferredPhone").on("change", function () {
        //    if ($(this).val() != "" && $(this).css("border-color") == "rgb(255, 0, 0)") {
        //        $(this).css("border-color", "green");
        //    }

        //});
    },
    MU3Demographics: function (PanelId, PatientId, DemographicsResponse) {

        // Commented as requirement may be need in future, so do not remove code. PRD-795
        return null;
        // EMR-6733
        //try {
        //        if (globalAppdata["isDemographics"].toLowerCase() == "true") {
        //            var m1_Fileds = "";
        //            var MU_ARRAY = [];

        //            // MU3 Demographics Measure
        //            if ($(PanelId + " #ddlSexualOrientation").val() == "" || (DemographicsResponse && DemographicsResponse.SexualOrientationId == "")) {
        //                m1_Fileds += "Sexual Orientation";
        //                $(PanelId + " #ddlSexualOrientation").css("border-color", "red");
        //            }
        //            else
        //                $(PanelId + " #ddlSexualOrientation").css("border-color", "#ccc");

        //            if ($(PanelId + " #ddlGenderIdentity").val() == "" || (DemographicsResponse && DemographicsResponse.GenderIdentityId == "")) {
        //                m1_Fileds += ",Gender Identity";
        //                $(PanelId + " #ddlGenderIdentity").css("border-color", "red");
        //            }
        //            else
        //                $(PanelId + " #ddlGenderIdentity").css("border-color", "#ccc");

        //            if ($(PanelId + " #ddlBirthSex").val() == "" || (DemographicsResponse && DemographicsResponse.BirthSex == "")) {
        //                m1_Fileds += ",Birth Sex";
        //                $(PanelId + " #ddlBirthSex").css("border-color", "red");
        //            }
        //            else
        //                $(PanelId + " #ddlBirthSex").css("border-color", "#ccc");

        //            var m1_obj = {
        //                ProfileName: "Demographics",
        //                Fields: "",
        //                PatientId: PatientId,
        //                IsShowAlert: false,
        //                Type: "MU3"
        //            };

        //            if (m1_Fileds != "") {
        //                m1_obj.Fields = m1_Fileds.indexOf(",") == 0 ? m1_Fileds.slice(1, m1_Fileds.length) : m1_Fileds;
        //                m1_obj.IsShowAlert = true;
        //            }
        //            else
        //                m1_obj.IsShowAlert = false;

        //            MU_ARRAY.push(m1_obj);

        //            var m2_Fileds = "";
        //            // MU3 Healthcare Surveys
        //            if ($(PanelId + " #ddlCountry").val() == "" || (DemographicsResponse && DemographicsResponse.Country == "")) {
        //                m2_Fileds += "Country";
        //                $(PanelId + " #ddlCountry").css("border-color", "red");
        //            }
        //            else
        //                $(PanelId + " #ddlCountry").css("border-color", "#ccc");

        //            if ($(PanelId + " #ddlPrefAddress").val() == "" || (DemographicsResponse && DemographicsResponse.PreferredAddressID == "")) {
        //                m2_Fileds += ",Preferred Address";
        //                $(PanelId + " #ddlPrefAddress").css("border-color", "red");
        //            }
        //            else
        //                $(PanelId + " #ddlPrefAddress").css("border-color", "#ccc");

        //            if ($(PanelId + " #ddlPreferredPhone").val() == "" || (DemographicsResponse && DemographicsResponse.PreferredPhoneID == "")) {
        //                m2_Fileds += ",Preferred Phone";
        //                $(PanelId + " #ddlPreferredPhone").css("border-color", "red");
        //            }
        //            else
        //                $(PanelId + " #ddlPreferredPhone").css("border-color", "#ccc");

        //            var m2_obj = {
        //                ProfileName: "Healthcare Surveys",
        //                Fields: "",
        //                PatientId: PatientId,
        //                IsShowAlert: false,
        //                Type: "MU3"
        //            };

        //            if (m2_Fileds != "") {
        //                m2_obj.Fields = m2_Fileds.indexOf(",") == 0 ? m2_Fileds.slice(1, m2_Fileds.length) : m2_Fileds;
        //                m2_obj.IsShowAlert = true;
        //            }
        //            else
        //                m2_obj.IsShowAlert = false;

        //            MU_ARRAY.push(m2_obj);

        //            return MU_ARRAY;
        //        }
        //        else
        //            return null;
        //    }
        
        //catch (ex) {
        //    console.log(ex);
        //}
        
    },

    VerifyMUAlert: function (ProfileName, Fields, PatientId, IsShowAlert, Type) {
        var array_ = [];
        var m1_obj = {
            ProfileName: ProfileName,
            Fields: Fields,
            PatientId: PatientId,
            IsShowAlert: IsShowAlert,
            Type: Type
        };

        array_.push(m1_obj);

        Patient_Demographic.UpdateMUAlert(array_).done(function (result) {
            if (result.status != false) {
                var data = JSON.parse(result.MUAlerts_JSON);
                var IsAnyOtherAlert = data.filter(item=>item.PatientId + "" == PatientId);
                if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0) {
                    utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                }
                else {
                    utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                }
            }
            else {
                console.log(result.Message);
            }
        });
    },
    //Start EMR-7049 TahreemMalik
    CCDANoteDisplayOnYes: function (PanelId, obj) {
        if ($(obj).val() == "1")
            $(PanelId + " #txtYesCCDA").removeClass('hidden');
        else
            $(PanelId + " #txtYesCCDA").addClass('hidden');
    },
    //End EMR-7049 TahreemMalik
};






//////-------------Editable Data Grid Scripts Strats-----------------------

EditableGrid = {

    options: {
        table: '#dgvICD',
        dialog: {
            wrapper: '#dialog',
            cancelButton: '#dialogCancel',
            confirmButton: '#dialogConfirm',
        }
    },

    initialize: function (GridId, ClassName, AddDefaultRow, bInfo, bFilter, bPaginate, bSort, iPageSize, reBuild, isPeralelEditableGrid) {
        EditableGrid.options.table = GridId;
        EditableGrid.setVars();
        EditableGrid.build(bInfo, bFilter, bPaginate, bSort, iPageSize, reBuild, ClassName, isPeralelEditableGrid);
        //If EditableGrid has already been loaded. ignore reloading its events
        if (isPeralelEditableGrid) { }
        else {
            EditableGrid.events(ClassName);
        }
        //Start 28-06-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        if ($('#' + ClassName.params.PanelID).find('.dataTables_empty').length > 0) {
            $('#' + ClassName.params.PanelID).find('[class*=datatables-footer]').addClass('hidden');
        }
        //End 28-06-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        if (AddDefaultRow != "0") {
            EditableGrid.rowAdd("-1");
        }
    },

    setVars: function () {
        EditableGrid.$table = $(EditableGrid.options.table);

        // dialog
        EditableGrid.dialog = {
        };
        EditableGrid.dialog.$wrapper = $(EditableGrid.options.dialog.wrapper);
        EditableGrid.dialog.$cancel = $(EditableGrid.options.dialog.cancelButton);
        EditableGrid.dialog.$confirm = $(EditableGrid.options.dialog.confirmButton);

        return EditableGrid;
    },

    build: function (bInfo, bFilter, bPaginate, bSort, iPageSize, reBuild, ClassName, isPeralelEditableGrid) {
        var TotalColumn = [];//['','','',''];
        TotalColumn[0] = {
            "bInfo": (bInfo != null ? bInfo : true), "bFilter": (bFilter != null ? bFilter : true), "bPaginate": (bPaginate != null ? bPaginate : true), "bSort": (bSort != null ? bSort : false), "iDisplayLength": (iPageSize != null ? iPageSize : 10)
        };

        //TotalColumn[1] = null;
        var count = 1;
        $(EditableGrid.options.table + " tr th").each(function () {
            TotalColumn[count] = null;
            count++;
        });
        if (isPeralelEditableGrid) {
            if (ClassName.EditableGrid) {
                //If EditableGrid has already been loaded.
                EditableGrid.datatable = ClassName.EditableGrid.$table.DataTable();
            }
        }
        else {
            EditableGrid.datatable = EditableGrid.$table.DataTable(TotalColumn[0], TotalColumn);
        }

        if (reBuild == false) {
            //Code here ali
            //trigger Enter Button or ESC Button
        }


        window.dt = EditableGrid.datatable;

        return EditableGrid;
    },

    events: function (ClassName) {
        var _self = EditableGrid;

        EditableGrid.$table.off()
            .on('click', 'a.expand-row', function (e) {
                e.preventDefault();

                _self.rowExpand($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.save-row', function (e) {
                e.preventDefault();

                //_self.rowSave($(this).closest('tr'), ClassName);
                EditableGrid.newRowSave($(this).closest('tr'), ClassName);
            })
            .on('click', '.checked-row', function (e) {
                _self.rowChecked($(this), ClassName);
            })
            .on('click', 'a.cancel-row', function (e) {
                e.preventDefault();
                _self.rowCancel($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.up-row', function (e) {
                e.preventDefault();
                _self.rowUp($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.down-row', function (e) {
                e.preventDefault();
                _self.rowDown($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.title-row', function (e) {
                e.preventDefault();
                _self.rowTitle($(this).closest('tr'));
            })
            .on('click', 'a.edit-row', function (e) {
                e.preventDefault();

                //_self.rowEdit($(this).closest('tr'));
                EditableGrid.newRowEdit($(this).closest('tr'));
            }).on('click', 'a.row-detail', function (e) {
                e.preventDefault();
                _self.rowDetail($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.copy-row', function (e) {
                e.preventDefault();
                _self.rowCopy($(this).closest('tr'), ClassName);
            })
            .on('click', 'a.remove-row', function (e) {
                e.preventDefault();

                var $row = $(this).closest('tr');

                //_self.rowRemove($row, ClassName);

                EditableGrid.newRowRemove($(this).closest('tr'), ClassName);

                //$.magnificPopup.open({
                //	items: {
                //		src: '#dialog',
                //		type: 'inline'
                //	},
                //	preloader: false,
                //	modal: true,
                //	callbacks: {
                //		change: function() {
                //			_self.dialog.$confirm.on( 'click', function( e ) {
                //				e.preventDefault();

                //				_self.rowRemove( $row );
                //				$.magnificPopup.close();
                //			});
                //		},
                //		close: function() {
                //			_self.dialog.$confirm.off( 'click' );
                //		}
                //	}
                //});
            });



        EditableGrid.dialog.$cancel.on('click', function (e) {
            e.preventDefault();
            $.magnificPopup.close();
        });

        return this;
    },

    newRowEdit: function ($row) {

        var CurrentRow = $row;
        var selectedRowTable = $($row).closest('table').attr("id");
        if (EditableGrid.options.table.substring(0, EditableGrid.options.table.indexOf("#dgv")).trim() == "#cptcodeDetail")
        { EditableGrid.options.table = EditableGrid.options.table.substring(0, EditableGrid.options.table.indexOf("#dgv")); }
        var GridId = EditableGrid.options.table + "#" + selectedRowTable;
        if (GridId == "#cptcodeDetail #dgvCPTPlanInfo") {
            if ($.fn.dataTable.isDataTable(GridId)) {
                $(GridId).dataTable().fnDestroy();
            }

            $.when(utility.MakeEditableGrid("#cptcodeDetail #pnlCPTPlanInfo", GridId, cptcodeDetail, "0")).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EditableGrid.rowEdit(CurrentRow);
                });

            });

        }
        else if (GridId == "#cptcodeDetail #dgvCPTNdcInfo") {
            if ($.fn.dataTable.isDataTable(GridId)) {
                $(GridId).dataTable().fnDestroy();
            }

            $.when(utility.MakeEditableGrid("#cptcodeDetail #pnlCPTNdcInfo", GridId, cptcodeDetail, "0")).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EditableGrid.rowEdit(CurrentRow);
                });

            });

        }
        else {
            EditableGrid.rowEdit(CurrentRow);
        }

    },
    newRowSave: function ($row, ClassName) {
        var CurrentRow = $row;
        var selectedRowTable = $($row).closest('table').attr("id");
        if (EditableGrid.options.table.substring(0, EditableGrid.options.table.indexOf("#dgv")).trim() == "#cptcodeDetail")
        { EditableGrid.options.table = EditableGrid.options.table.substring(0, EditableGrid.options.table.indexOf("#dgv")); }
        var GridId = EditableGrid.options.table + "#" + selectedRowTable;
        if (GridId == "#cptcodeDetail #dgvCPTPlanInfo") {
            if ($.fn.dataTable.isDataTable(GridId)) {
                $(GridId).dataTable().fnDestroy();
            }

            $.when(utility.MakeEditableGrid("#cptcodeDetail #pnlCPTPlanInfo", GridId, cptcodeDetail, "0")).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EditableGrid.rowSave(CurrentRow, ClassName);
                });

            });

        }
        else if (GridId == "#cptcodeDetail #dgvCPTNdcInfo") {
            if ($.fn.dataTable.isDataTable(GridId)) {
                $(GridId).dataTable().fnDestroy();
            }

            $.when(utility.MakeEditableGrid("#cptcodeDetail #pnlCPTNdcInfo", GridId, cptcodeDetail, "0")).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EditableGrid.rowSave(CurrentRow, ClassName);
                });

            });

        }
        else {
            EditableGrid.rowSave(CurrentRow, ClassName);
        }
    },


    newRowRemove: function ($row, ClassName) {
        var CurrentRow = $row;
        var selectedRowTable = $($row).closest('table').attr("id");
        if (EditableGrid.options.table.substring(0, EditableGrid.options.table.indexOf("#dgv")).trim() == "#cptcodeDetail")
        { EditableGrid.options.table = EditableGrid.options.table.substring(0, EditableGrid.options.table.indexOf("#dgv")); }
        var GridId = EditableGrid.options.table + "#" + selectedRowTable;
        if (GridId == "#cptcodeDetail #dgvCPTPlanInfo") {
            if ($.fn.dataTable.isDataTable(GridId)) {
                $(GridId).dataTable().fnDestroy();
            }

            $.when(utility.MakeEditableGrid("#cptcodeDetail #pnlCPTPlanInfo", GridId, cptcodeDetail, "0")).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EditableGrid.rowRemove(CurrentRow, ClassName);
                });

            });

        }
        else if (GridId == "#cptcodeDetail #dgvCPTNdcInfo") {
            if ($.fn.dataTable.isDataTable(GridId)) {
                $(GridId).dataTable().fnDestroy();
            }

            $.when(utility.MakeEditableGrid("#cptcodeDetail #pnlCPTNdcInfo", GridId, cptcodeDetail, "0")).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    EditableGrid.rowRemove(CurrentRow, ClassName);
                });

            });

        }
        else {
            EditableGrid.rowRemove(CurrentRow, ClassName);
        }
    },

    // ==========================================================================================
    // ROW FUNCTIONS
    // ==========================================================================================
    rowAdd: function (RowId, VisitId, VitalLatestBPId, VitalLatestPulseId, VitalLatestTempId, VitalLatestRespId, checkBoxOnChange, Checked) {
        checkBoxOnChange == undefined ? "" : checkBoxOnChange


        var actions = [],
            data,
                $row;
        var AppendColumn = [];//['','','',''];
        //AppendColumn[0] = '<a href="#" class="hidden on-editing title-row" title="Save Title" ><i class="fa fa-plus-square-o"></i></a>';
        var count = 0;
        $(EditableGrid.options.table + " tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var ActionCount = 0;
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                }
                if ($.inArray("CheckBox", arrActionType) != -1) {
                    if (Checked != null && Checked == false) {
                        actions[ActionCount] = '<input type="checkbox" onchange="' + checkBoxOnChange + '"  id="chkActive' + RowId + '" class="btn-xs checked-row mr-none pr-none" title="Select Record" />';
                    }
                    else {
                        actions[ActionCount] = '<input type="checkbox" checked="checked" onchange="' + checkBoxOnChange + '"  id="chkActive' + RowId + '" class="btn-xs checked-row mr-none pr-none" title="Select Record" />';
                    }
                    ActionCount++;
                }
                if ($.inArray("Cancel", arrActionType) != -1) {
                    actions[ActionCount] = '<a class="btn-xs hidden on-editing cancel-row mr-none pr-none pl-none" title="Cancel Record"><i class="fa fa-close black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Save", arrActionType) != -1) {
                    actions[ActionCount] = '<a href="#" class="btn-xs hidden on-editing save-row mr-none pr-none pl-none" title="Save Record" ><i class="fa fa-save green"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("SaveTitle", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs hidden on-editing title-row mr-none pr-none pl-none" title="Save Title" ><i class="fa fa-paste black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Delete", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default remove-row mr-none pr-none pl-none" title="Delete Record" ><i class="fa fa-close red"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Edit", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default edit-row mr-none pr-none pl-none" title="Edit Record" ><i class="fa fa-edit black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Up", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default up-row mr-none pr-none pl-none" title="Up Record" ><i class="fa fa-arrow-up black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Down", arrActionType) != -1) {

                    actions[ActionCount] = '<a href="#" class="btn-xs on-default down-row mr-none pr-none pl-none" title="Down Record" ><i class="fa fa-arrow-down black"></i></a>';
                    ActionCount++;
                }
                if ($.inArray("Detail", arrActionType) != -1) {
                    if (RowId > 0) {
                        actions[ActionCount] = '<a href="#" class="btn-xs on-editing row-detail mr-none pr-none pl-none" title="Record Detail" ><i class="fa fa-book blue"></i></a>';
                        ActionCount++;
                    }
                }
                if ($.inArray("Copy", arrActionType) != -1) {
                    actions[ActionCount] = '<a href="#" class="btn-xs on-default copy-row mr-none pr-none pl-none" title="Row duplicate" ><i class="fa fa-copy black"></i></a>';
                    ActionCount++;
                }
                AppendColumn[count] = actions.join(' ');
            }
            else if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "expand") {
                AppendColumn[count] = '<div class="spacer3"></div><a href="#" class="btn-xs hidden on-editing expand-row  mr-none pr-none pl-none" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            }
            else
                AppendColumn[count] = '';
            count++;
        });

        data = EditableGrid.datatable.row.add(AppendColumn);
        $row = EditableGrid.datatable.row(data[0]).nodes().to$();
        $row.addClass('adding');
        $row.attr("visitid", VisitId);
        if (VitalLatestBPId != null) {
            $row.attr("VitalLatestBPId", VitalLatestBPId);
        }
        if (VitalLatestPulseId != null) {
            $row.attr("VitalLatestPulseId", VitalLatestPulseId);
        }
        if (VitalLatestTempId != null) {
            $row.attr("VitalLatestTempId", VitalLatestTempId);
        }
        if (VitalLatestRespId != null) {
            $row.attr("VitalLatestRespId", VitalLatestRespId);
        }
        if (RowId) {
            $row.attr("id", RowId);
        }
        else
            $row.attr("id", "-1");
        EditableGrid.rowEdit($row);
        EditableGrid.datatable.order([0, 'dsc']).draw(); // always show fields
        return $row;
    },

    rowCancel: function ($row, ClassName) {
        ClassName.rowCancel($row, EditableGrid);
    },

    rowChecked: function ($this, ClassName) {
        ClassName.rowChecked($this, event);
    },

    rowEdit: function ($row) {

        var _self = this,
                data;
        data = this.datatable.row($row.get(0)).data();
        var RowId = $($row).attr("id");
        var VisitId = $($row).attr("visitid");
        if (!RowId) {
            RowId = "-1";//0 means it's new row
            $($row).attr("id", "-1");
        }

        var ddlValue = "";

        $(EditableGrid.options.table + " tr th").each(function (i) {
            var $this = $row.find('td:nth-child(' + (i + 1) + ')');
            var controlId = $(this).attr("controlid");
            if (!controlId) {
                controlId = "";
            }
            var controlName = $(this).attr("controlname");
            if (!controlName) {
                controlName = "";
            }
            var controldisabledClass = $(this).attr("iscontroldisabled") == "1" ? "disabled" : "";
            var coltype = $(this).attr("coltype");
            var isoptional = $(this).attr("isoptional");
            var subcols = $(this).attr("subcols");
            var onfocussout = $(this).attr("onfocusout");//$(this).attr("onfocusout") != null ? $(this).attr("onfocusout") : ($(this).attr("onblur") != null ? $(this).attr("onblur") : "");
            if (onfocussout != null && $(this).attr("onblur")) {
                onfocussout = $(this).attr("onblur");
            }
            var onkeypress = $(this).attr("onkeypress");
            var oninput = $(this).attr("oninput");
            if (!isoptional) {
                isoptional = "1";
            }
            $this.attr("isoptional", isoptional);
            if (coltype && coltype.toLowerCase() == "expand") {
                $this.html('<div class="spacer3"></div><a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>');
                $this.addClass('expand');
            }
            else if (coltype && coltype.toLowerCase() == "action") {
                _self.rowSetActionsEditing($row);
                //$row.addClass('adding')
                //$row.find('td:first').addClass('actions')
                $this.addClass('actions');
            }
            else if (coltype && coltype.toLowerCase() == "action") {
                _self.rowSetActionsEditing($row);
                //$row.addClass('adding')
                //$row.find('td:first').addClass('actions')
                $this.addClass('actions');
            }
            else if (coltype && coltype.toLowerCase() == "textbox") {
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size20 p-none";
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var TextBoxes = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        var inputFocussOut = "";
                        if (onfocussout && onfocussout != "") {
                            var body = onfocussout.substring(1, onfocussout.lastIndexOf("("));
                            var Parameters = onfocussout.substring(onfocussout.lastIndexOf("(") + 1, onfocussout.lastIndexOf(")"));
                            if (Parameters != "") {
                                var arrParameters = Parameters.split(',');
                                for (var i = 0; i < arrParameters.length; i++) {
                                    arrParameters[i] = "'" + String(arrParameters[i]).trim().replace(/'/gi, "") + k + "" + RowId + "'";
                                }
                                body += "(" + arrParameters.join(',') + ")";
                            }
                            else
                                body += "()";
                        }
                        if (!onkeypress) {
                            onkeypress = "";
                        }
                        var attrAutoComplete = $(this).attr("autocompletemethod");
                        var AutoCompleteSearch = "";
                        if (attrAutoComplete && attrAutoComplete != "") {
                            AutoCompleteSearch = EditableGrid.getAutoCompleteSearch(attrAutoComplete, CtrlId);
                            if (attrAutoComplete == "Modifier") {
                                body = "utility.ValidateCode(this, 'Modifier','" + CtrlId + "');";
                            }

                            else if (attrAutoComplete == "icd") {
                                body = "utility.ValidateCode(this, 'icd','" + CtrlId + "');";
                            }

                            else if (attrAutoComplete == "cpt") {
                                body = "utility.ValidateCode(this, 'cpt','" + CtrlId + "');";
                            }
                            else if (attrAutoComplete == "POS") {
                                body = "utility.ValidateAutoComplete(this, '" + CtrlId + "', false, '1');";
                            }
                            else if (attrAutoComplete == "TOS") {
                                body = "utility.ValidateCode(this, 'TOS','" + CtrlId + "');";
                            }


                        }
                        //var hiddenField = "";
                        //var hfcontrolid = $(this).attr("hfcontrolid");
                        //var hiddenfieldId = "";
                        //if (hfcontrolid != null) {
                        //    hiddenfieldId = hfcontrolid;
                        //    hiddenField = "<input type='hidden' id='"+hfcontrolid+"' value=0 />";
                        //}

                        var arrHiddenFields = $(this).attr("hfcontrolid");
                        var currencyicon = $(this).attr("currencyicon") != null ? $(this).attr("currencyicon") : "";
                        var hiddenField = "";
                        if (arrHiddenFields != null && arrHiddenFields != "") {
                            if (arrHiddenFields.indexOf(",") > -1) {
                                arrHiddenFields = arrHiddenFields.split(",");
                                for (var i = 0; i < arrHiddenFields.length; i++) {
                                    var hfcontrolid = arrHiddenFields[i];
                                    var hiddenfieldId = "";
                                    if (hfcontrolid != null) {
                                        hiddenfieldId = hfcontrolid;
                                        hiddenField += "<input type='hidden' id='" + (hfcontrolid + k + RowId) + "' value=0 />";
                                    }
                                }
                            }
                            else {
                                var hfcontrolid = arrHiddenFields;
                                var hiddenfieldId = "";
                                if (hfcontrolid != null) {
                                    hiddenfieldId = hfcontrolid;
                                    hiddenField += "<input type='hidden' id='" + (hfcontrolid + k + RowId) + "' value=0 />";
                                }
                            }

                        }

                        var CurrencyStartHTML = "";
                        var CurrencyEndHTML = "";
                        if (currencyicon != "") {
                            CurrencyStartHTML = "<div class='input-group'><span class='input-group-addon xxs-font pl-tiny pr-tiny'><i class='" + currencyicon + "'></i></span>";
                            CurrencyEndHTML = "</div>";
                        }
                        TextBoxes += CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" type="text" ' + controldisabledClass + ' oninput="' + AutoCompleteSearch + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control pull-left ' + controlwidth + ' mr-xxs" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField;
                    }
                    $this.append(TextBoxes);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;

                    var inputFocussOut = "";
                    if (onfocussout && onfocussout != "") {
                        var body = onfocussout.substring(0, onfocussout.lastIndexOf("("));
                        if (body == null)
                            body = "";
                        var Parameters = onfocussout.substring(onfocussout.lastIndexOf("(") + 1, onfocussout.lastIndexOf(")"));
                        if (Parameters != "") {
                            var arrParameters = Parameters.split(',');
                            //PMS-4442 by:MAhmad
                            for (var ii = 0; ii < arrParameters.length; ii++) {
                                arrParameters[ii] = "'" + String(arrParameters[ii]).trim().replace(/'/gi, "") + RowId + "'";
                            }
                            //PMS-4442 by:MAhmad
                            body += "(" + arrParameters.join(',') + ")";
                        }
                        else
                            body += "()";
                    }

                    if (!onkeypress) {
                        onkeypress = "";
                    }
                    var attrAutoComplete = $(this).attr("autocompletemethod");
                    var AutoCompleteSearch = "";
                    if (attrAutoComplete) {
                        AutoCompleteSearch = EditableGrid.getAutoCompleteSearch(attrAutoComplete, CtrlId);
                        if (attrAutoComplete == "Modifier") {
                            body = "utility.ValidateCode(this, 'Modifier','" + CtrlId + "');";
                        }

                        else if (attrAutoComplete == "icd") {
                            body = " utility.ValidateCode(this, 'icd','" + CtrlId + "');";
                        }

                        else if (attrAutoComplete == "cpt") {
                            body = " utility.ValidateCode(this, 'cpt','" + CtrlId + "');";
                        }
                        else if (attrAutoComplete == "POS") {
                            body = " utility.ValidateAutoComplete(this, '" + CtrlId + "', false, '1');";
                        }
                        else if (attrAutoComplete == "TOS") {
                            body = "utility.ValidateCode(this, 'TOS','" + CtrlId + "');";
                        }
                    }
                    var arrHiddenFields = $(this).attr("hfcontrolid");
                    var currencyicon = $(this).attr("currencyicon") != null ? $(this).attr("currencyicon") : "";
                    var hiddenField = "";
                    if (arrHiddenFields != null && arrHiddenFields != "") {
                        if (arrHiddenFields.indexOf(",") > -1) {
                            arrHiddenFields = arrHiddenFields.split(",");
                            for (var i = 0; i < arrHiddenFields.length; i++) {
                                var hfcontrolid = arrHiddenFields[i];
                                var hiddenfieldId = "";
                                if (hfcontrolid != null) {
                                    hiddenfieldId = hfcontrolid;
                                    hiddenField += "<input type='hidden' id='" + (hfcontrolid + RowId) + "' value=0 />";
                                }
                            }
                        }
                        else {
                            var hfcontrolid = arrHiddenFields;
                            var hiddenfieldId = "";
                            if (hfcontrolid != null) {
                                hiddenfieldId = hfcontrolid;
                                hiddenField += "<input type='hidden' id='" + (hfcontrolid + RowId) + "' value=0 />";
                            }
                        }

                    }
                    var CurrencyStartHTML = "";
                    var CurrencyEndHTML = "";
                    if (currencyicon != "") {
                        CurrencyStartHTML = "<div class='input-group'><span class='input-group-addon xxs-font pl-tiny pr-tiny'><i class='" + currencyicon + "'></i></span>";
                        CurrencyEndHTML = "</div>";
                    }

                    if (CtrlId.indexOf('txtTotalFEE') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size-min75 p-tiny" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField);

                    else if (CtrlId.indexOf('txtUnits') >= 0)
                        $this.html('<input id="' + CtrlId + '" name="' + CtrlName + '" type="text" oninput="' + AutoCompleteSearch + oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size50 p-tiny" value="' + data[i] + '"/>' + hiddenField);

                    else if (CtrlId.indexOf('txtINSCharges') >= 0 || CtrlId.indexOf('txtPATCharges') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block size-min75 p-tiny" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField);
                        //adnan maqbool, pms-4877
                    else if (CtrlId.indexOf('txtPOS') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" maxlength="2"  oninput="' + AutoCompleteSearch + oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control size30 p-tiny mb-xxs" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField);
                    else if (CtrlId.indexOf('txtNDCCode') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' placeholder="99999999999" type="text" maxlength="11"  oninput="' + +oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control size80" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField);
                    else if (CtrlId.indexOf('txtNDCDescription') >= 0)
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" maxlength="55"  oninput="' + AutoCompleteSearch + oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField);
                    else if (CtrlId.indexOf("txtChargeOrder") >= 0) {
                        $this.css('display', 'none');
                        $this.html('<input id="' + CtrlId + '" name="' + CtrlName + '" type="text" maxlength="55" isoptional="' + isoptional + '" required class="form-control" value="' + data[i] + '"/>');
                    }
                    else
                        $this.html(CurrencyStartHTML + '<input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" oninput="' + AutoCompleteSearch + oninput + '" onkeypress="' + onkeypress + '" onblur="' + body + '" isoptional="' + isoptional + '" required class="form-control input-block p-tiny" value="' + data[i] + '"/>' + CurrencyEndHTML + hiddenField);
                }
            }
            else if (coltype && coltype.toLowerCase() == "datetime") {
                var subcols = $(this).attr("subcols");
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size70";
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var calendar = '<div class="input-group"><span class="input-group-addon pl-tiny pr-tiny xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDOSFrom" class="form-control " type="text" data-plugin-datepicker=""></div>';
                    var calendars = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        calendars += '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '"/></div>';
                    }
                    $this.append(calendars);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    $this.html('<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '"/></div>');
                }
            }
            else if (coltype && coltype.toLowerCase() == "time") {
                var subcols = $(this).attr("subcols");
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size70";
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var calendar = '<div class="input-group"><span class="input-group-addon pl-tiny pr-tiny xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDOSFrom" class="form-control " type="text" data-plugin-datepicker="" /></div>';
                    var calendars = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        calendars += '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '" data-plugin-timepicker /></div>';
                    }
                    $this.append(calendars);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    $this.html('<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' type="text" isoptional="' + isoptional + '" required class="form-control ' + controlwidth + ' p-tiny" value="' + data[i] + '" data-plugin-timepicker /></div>');
                }
            }
            else if (coltype && coltype.toLowerCase() == "autocomplete") {
                //var subcols = $(this).attr("subcols");
                var controlId = "";
                var CrlId = $(this).attr("controlid");
                if (CrlId != null) {
                    controlId = CrlId;
                }
                var controlwidth = $(this).attr("controlwidth");
                if (controlwidth != null)
                    ;
                else
                    controlwidth = "size50";
                var hfcontrolid = $(this).attr("hfcontrolid");
                var hiddenfieldId = "";
                if (hfcontrolid != null) {
                    hiddenfieldId = hfcontrolid;
                    hiddenfieldId = hiddenfieldId.split(",");
                }

                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var Autocompletes = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var attrAutoComplete = $(this).attr("autocompletemethod");
                        var attrSearchMethod = $(this).attr("searchmethod");
                        var attrparentctrl = $(this).attr("parentctrl") != null ? $(this).attr("parentctrl") : "";
                        var attrcontainerctrl = $(this).attr("containerctrl") != null ? $(this).attr("containerctrl") : "";
                        var iscode = $(this).attr("iscode");
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        var CurrentParentCtrl = attrparentctrl + VisitId;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }

                        if (attrAutoComplete == "ICD") {
                            var HiddentCtrlId = [];
                            $this.addClass('size-min260');
                        }
                        else {
                            var HiddentCtrlId = "";
                        }
                        for (var ii_ = 0; ii_ < hiddenfieldId.length; ii_++) {
                            HiddentCtrlId.push(hiddenfieldId[ii_] + k + "" + RowId);
                        }

                        Autocompletes += _self.addAutoCompleteField(CtrlId, data[i], controlwidth, HiddentCtrlId, attrAutoComplete, attrSearchMethod, isoptional, CtrlName, iscode, CurrentParentCtrl, attrcontainerctrl);
                    }
                    $this.append(Autocompletes);
                }
                else {
                    var attrAutoComplete = $(this).attr("autocompletemethod");
                    var attrSearchMethod = $(this).attr("searchmethod");
                    var attrparentctrl = $(this).attr("parentctrl") != null ? $(this).attr("parentctrl") : "";
                    var attrcontainerctrl = $(this).attr("containerctrl") != null ? $(this).attr("containerctrl") : "";
                    var iscode = $(this).attr("iscode");
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    var HiddentCtrlId = hiddenfieldId + RowId;
                    var CurrentParentCtrl = attrparentctrl + VisitId;

                    /***********/
                    if (attrAutoComplete == "CPT" || attrAutoComplete == "InsurancePlan" || attrAutoComplete == "Modifier") {
                        var HiddentCtrlId = [];
                        //CurrentParentCtrl = "EncounterChargeCapture";
                    }
                    else {
                        var HiddentCtrlId = "";
                    }
                    for (var ii_ = 0; ii_ < hiddenfieldId.length; ii_++) {
                        HiddentCtrlId.push(hiddenfieldId[ii_] + RowId);
                    }
                    /**********/

                    $this.html(_self.addAutoCompleteField(CtrlId, data[i], controlwidth, HiddentCtrlId, attrAutoComplete, attrSearchMethod, isoptional, CtrlName, iscode, CurrentParentCtrl, attrcontainerctrl));
                }
            }
            else if (coltype && coltype.toLowerCase() == "dropdown") {

                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var DropDowns = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        DropDowns += _self.addDropDownField(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName);
                    }
                    $this.append(DropDowns);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    ddlValue = data[i];
                    $this.html(_self.addDropDownField(CtrlId, data[i], $(this).attr("ddlist"), isoptional, CtrlName));
                }
            }
            else if (coltype && coltype.toLowerCase() == "checkbox") {

                var cntrols = [];
                if (subcols && subcols != "") {
                    $this.attr("subcols", subcols);
                    var CheckBoxes = "";
                    for (var k = 1; k <= subcols ; k++) {
                        var CtrlId = controlId + k + "" + RowId;
                        var CtrlName = controlName + k;
                        if (k != 1) {// to make all controls as required except first control
                            isoptional = "1";
                        }
                        if (data[i] && data[i].toLowerCase() == "true" || data[i] && data[i].toLowerCase() == "yes")
                            cntrols.push({ Id: CtrlId, Value: true });
                        else
                            cntrols.push({ Id: CtrlId, Value: false });

                        CheckBoxes += '<input type="checkbox" id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' isoptional="' + isoptional + '" class="input-block"/>';
                    }
                    $this.append(CheckBoxes);
                }
                else {
                    var CtrlId = controlId + RowId;
                    var CtrlName = controlName;
                    $this.html('<input type="checkbox" id="' + CtrlId + '" name="' + CtrlName + '" ' + controldisabledClass + ' isoptional="' + isoptional + '" class="input-block"/>');
                    if (data[i] && data[i].toLowerCase() == "true" || data[i] && data[i].toLowerCase() == "yes")
                        cntrols.push({ Id: CtrlId, Value: true });
                    else
                        cntrols.push({ Id: CtrlId, Value: false });
                }

                //set all checkboxs prop checked or not
                for (var i in cntrols) {
                    $("#" + cntrols[i].Id).prop("checked", cntrols[i].Value);
                }
            }
        });

        $row.find("input[id*=dtp]").each(function () {

            var date_format = 'dd/mm/yyyy';
            //set default Date Formate
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];


            if (($row.find("input[id*=dtpDOSFrom]").length > 0 && $row.find("input[id*=dtpDOSFrom]").attr('id').indexOf('dtpDOSFrom') > -1) || ($row.find("input[id*=dtpDOSTo]").length > 0 && $row.find("input[id*=dtpDOSTo]").attr("id").indexOf('dtpDOSTo') > -1)) {
                //var dtDOSFromId = $row.find("input[id*=dtpDOSFrom]").attr('id');
                //var dtDOSToId = $row.find("input[id*=dtpDOSTo]").attr("id");
                //utility.ValidateFromToDate('frmEncounterChargeCapture', dtDOSFromId, dtDOSToId, false);


                //if ($(this).attr('id').indexOf('dtpDOSFrom') != -1)
                //    $row.find("input[id*=dtpDOSTo]").val($(this).val());

            }
            else {


                $(this).datepicker({
                    todayHighlight: true,
                    format: date_format,
                }).on('changeDate', function (e) {

                    $(this).datepicker('hide');
                });
            }
        });

        $row.loadDropDowns(true).done(function () {
            $(EditableGrid.options.table + " tr th").each(function (i) {
                var controlId = $(this).attr("controlid");
                if (!controlId) {
                    controlId = "";
                }
                var coltype = $(this).attr("coltype");
                if (coltype == "dropdown") {
                    var CtrlId = controlId + RowId;
                    $("#" + CtrlId + " option").each(function () {
                        if ($(this).text() == ddlValue) {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }

            });

        });

        var dfd = new $.Deferred();
        $row.find("input[id*=txtCodePlan], input[id*=txtTOSPlan]").each(function () {
            CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
                var Ctrl = $row.find("input[id*=txtCodePlan], input[id*=txtTOSPlan]");
                var hfCtrl = $row.find("input[id*=hfPlan]");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
                dfd.resolve();
            });
        });
    },

    addDropDownField: function (id, selected_value, method, isoptional, CtrlName) {

        var ddl = document.createElement("select");

        ddl.setAttribute("class", "form-control");
        ddl.setAttribute("id", id);
        ddl.setAttribute("name", CtrlName);
        ddl.setAttribute("ddlist", method);
        ddl.setAttribute("isoptional", isoptional);
        return ddl


    },

    addAutoCompleteField: function (id, value, width, hfcontrolid, autoCompleteMethod, searchMethod, isoptional, CtrlName, iscode, ParentCtrl, ContainerCtrl) {

        var hfControl = "";

        if (Object.prototype.toString.call(hfcontrolid) === '[object Array]') {
            if (hfcontrolid != "") {
                for (var i = 0; i < hfcontrolid.length; i++) {
                    hfControl += '<input type="hidden" id="' + hfcontrolid[i] + '" />';
                }
            }
        }
        else {
            if (hfcontrolid != "") {
                hfControl = '<input type="hidden" id="' + hfcontrolid + '" />';
            }
        }

        var AutoCompleteSearch = "";
        var ClickToSearch = "";
        if (searchMethod && searchMethod != "") {
            ClickToSearch = searchMethod.substring(0, searchMethod.lastIndexOf(")")) + ",'" + id + "','" + hfcontrolid + "');";
            //ClickToSearch = searchMethod;

        }
        var onFocusOutMethod = "utility.ValidateAutoComplete(this, '" + hfcontrolid + "', true);";

        var classForICD = "";
        if (autoCompleteMethod && autoCompleteMethod == "ICD") {

            classForICD = "pull-left size70 mr-tiny"

            // MK
            //if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetICDCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            AutoCompleteSearch = "utility.BindIMOAutoCompleteText(this, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#" + hfcontrolid + "', '',true,'','ICD',true,'" + ParentCtrl + "','" + ContainerCtrl + "', true);";
            //}
            // -MK

            // AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetICDCode', true, '#" + hfcontrolid + "', '');";
            //ClickToSearch = "EditableGrid.OpenSearchPopup('ICD');";
            //            onFocusOutMethod = "utility.ValidateAutoComplete(this, '" + hfcontrolid + "', true,1);";

            //MK
            //onFocusOutMethod = "";//"utility.ValidateCode(this, 'ICD','" + hfcontrolid + "');";
            onFocusOutMethod = "utility.ValidateIsEmpty(this, 'ICD','" + hfcontrolid + "', '" + ParentCtrl + "' );EncounterChargeCapture.ValidateICDCode(this);";
        }
        else if (autoCompleteMethod && autoCompleteMethod == "CPT") {
            //MK//if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            //    AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#" + hfcontrolid + "', '');";
            //MK//}
            //AutoCompleteSearch = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //onFocusOutMethod = "utility.ValidateAutoComplete(this, '" + hfcontrolid + "', true,1);";
            AutoCompleteSearch = "utility.BindIMOAutoCompleteText(this, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#" + hfcontrolid + "', '',true,'','CPT',true,'" + ParentCtrl + "','" + ContainerCtrl + "', true);";
            onFocusOutMethod = "";//"utility.ValidateCode(this, 'CPT','" + hfcontrolid + "');";

        }
        else if (autoCompleteMethod && autoCompleteMethod == "Modifier") {
            //if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteMethod = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_CODE', 'GET_MODIFIER_CODE', '#" + hfcontrolid + "', '',false,0," + iscode + ");";
            onFocusOutMethod = "utility.ValidateCode(this, 'MODIFIER','" + hfcontrolid + "');";

            //}
            //ClickToSearch = "EditableGrid.OpenSearchPopup('Modifier');";

        }
        else if (autoCompleteMethod && autoCompleteMethod == "InsurancePlan") {
            AutoCompleteSearch = "utility.BindInsurancePlan(this, '" + hfcontrolid + "','" + ContainerCtrl + "');";
        }
        if (autoCompleteMethod && autoCompleteMethod != "InsurancePlan") {
            return autoField = '<div class="' + classForICD + '"><div id="div' + id + '" class="input-group">' +
            '<input id="' + id + '" class="form-control ' + width + '" isoptional="' + isoptional + '" name = "' + CtrlName + '"  placeholder="Start typing to search" value="' + value + '" type="text" oninput="' + AutoCompleteSearch + '" onblur="' + onFocusOutMethod + '">' +
                           '<div class="input-group-btn">' +
                              '<button class="btn btn-primary btn-xs pl-xxs pr-xxs" type="button"  onclick="' + ClickToSearch + '"><i class="glyphicon glyphicon-search tiny-font"></i></button>' +
                           '</div></div><div>' + hfControl + '</div></div>';
        }
        else {
            return autoField = '<div class="' + classForICD + '"><div id="div' + id + '" class="input-group">' +
                '<input id="' + id + '" class="form-control ' + width + '" isoptional="' + isoptional + '" name = "' + CtrlName + '"  placeholder="Start typing to search" value="' + value + '" type="text" >' +
                               '<div class="input-group-btn">' +
                                  '<button class="btn btn-primary btn-xs pl-xxs pr-xxs" type="button"  onclick="' + ClickToSearch + '"><i class="glyphicon glyphicon-search tiny-font"></i></button>' +
                               '</div></div><div>' + hfControl + '</div></div>';
        }
    },



    getAutoCompleteSearch: function (autoCompleteMethod, hfcontrolid) {
        var AutoCompleteSearch = "";
        if (autoCompleteMethod && autoCompleteMethod == "Modifier") {
            //if (globalAppdata['IMO_ID'] == "") {
            //    AutoCompleteMethod = " CacheManager.BindAutoCompleteText(this, 'GetCPTCode', true, '#" + hfcontrolid + "', '');";
            //}
            //else {
            AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_CODE', 'GET_MODIFIER_CODE', '#" + hfcontrolid + "', '',false,0);";
            //}
            //ClickToSearch = "EditableGrid.OpenSearchPopup('Modifier');";

        }
        if (autoCompleteMethod && autoCompleteMethod == "POS") {
            AutoCompleteSearch = "utility.POSAutoComplete(this,  '#" + hfcontrolid + "');";
        }
        if (autoCompleteMethod && autoCompleteMethod == "TOS") {
            AutoCompleteSearch = "utility.TOSAutoComplete(this,  '#" + hfcontrolid + "');";
        }
        return AutoCompleteSearch;
    },

    rowExpand: function ($row, ClassName) {
        ClassName.rowExpand($row, EditableGrid);
    },

    rowSave: function ($row, ClassName) {
        ClassName.rowSave($row, EditableGrid);
    },

    rowRemove: function ($row, ClassName) {
        ClassName.rowRemove($row, EditableGrid);
        //this.datatable.row($row.get(0)).remove().draw();
    },

    rowSetActionsEditing: function ($row) {
        $row.find('.on-editing').removeClass('hidden');
        $row.find('.on-default').addClass('hidden');
    },

    rowSetActionsDefault: function ($row) {
        $row.find('.on-editing').addClass('hidden');
        $row.find('.on-default').removeClass('hidden');
    },

    rowValidate: function ($row) {

        var firstInvalid = false;

        var isValidRow = true;
        var isValidRowChild = true;
        $row.find('td').map(function () {
            var $this = $(this);

            var Tags = $this.find('[type=hidden],[type=text],[type=password], textarea, select');//'[type=hidden],[type=text], textarea, ul'
            Tags.each(function () {
                if ($(this).attr("isoptional") && $(this).attr("isoptional") == "0" && $(this).val() == "") {
                    $(this).css("border", "1px solid red");
                    if (firstInvalid == false) {
                        firstInvalid = true;
                        $(this).focus();
                    }
                    if (isValidRow != false) {
                        isValidRow = false;
                    }
                }
                else
                    $(this).css("border", "1px solid #ccc");
            });
        });

        var childRow = EditableGrid.datatable.row($row).child();
        if (childRow) {
            childRow.find('td').map(function () {
                var $this = $(this);

                var Tags = $this.find('[type=hidden],[type=text],[type=password], textarea, select');//'[type=hidden],[type=text], textarea, ul'
                Tags.each(function () {
                    if ($(this).attr("isoptional") && $(this).attr("isoptional") == "0" && $(this).val() == "") {
                        $(this).css("border", "1px solid red");
                        if (firstInvalid == false) {
                            firstInvalid = true;
                            $(this).focus();
                        }
                        if (isValidRowChild != false) {
                            isValidRowChild = false;
                        }
                    }
                    else
                        $(this).css("border", "1px solid #ccc");
                });
            });
        }


        return isValidRow && isValidRowChild;
    },

    rowDown: function ($row, ClassName) {
        ClassName.rowDown($row, EditableGrid);
    },
    rowDetail: function ($row, ClassName) {
        ClassName.rowDetail($row, EditableGrid);
    },
    rowUp: function ($row, ClassName) {
        ClassName.rowUp($row, EditableGrid);
    },
    rowTitle: function ($row) {
    },

    rowCopy: function ($row, ClassName) {
        ClassName.rowCopy($row, EditableGrid);
    },

    GetActions: function (arrActionType) {
        var actions = "";
        if ($.inArray("CheckBox", arrActionType) != -1) {

            actions[ActionCount] = '<input type="checkbox"  class="btn-xs checked-row mr-none pr-none" title="Select Record" />';
            ActionCount++;
        }
        if ($.inArray("Cancel", arrActionType) != -1) {
            actions += '<a class="btn-xs hidden on-editing cancel-row pl-none mr-none pr-none" title="Cancel Record"><i class="fa fa-close black"></i></a>';
        }
        if ($.inArray("Save", arrActionType) != -1) {
            actions += '<a href="#" class="btn-xs hidden on-editing save-row mr-none pr-none" title="Save Record" ><i class="fa fa-save green"></i></a>';
        }
        if ($.inArray("SaveTitle", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs hidden on-editing title-row mr-none pr-none" title="Save Title" ><i class="fa fa-paste black"></i></a>';
        }
        if ($.inArray("Delete", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default remove-row pl-none mr-none pr-none" title="Delete Record" ><i class="fa fa-close red"></i></a>';
        }
        if ($.inArray("Edit", arrActionType) != -1) {

            actions += '&nbsp&nbsp<a href="#" class="btn-xs on-default edit-row  mr-none pr-none" title="Edit Record" ><i class="fa fa-edit black"></i></a>';
        }
        if ($.inArray("Up", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default up-row mr-none pr-none" title="Up Record" ><i class="fa fa-arrow-up black"></i></a>';
        }
        if ($.inArray("Down", arrActionType) != -1) {

            actions += '<a href="#" class="btn-xs on-default down-row mr-none pr-none" title="Down Record" ><i class="fa fa-arrow-down black"></i></a>';
        }
        if ($.inArray("Detail", arrActionType) != -1) {
            actions += '<a href="#" class="btn-xs on-editing row-detail mr-none pr-none" title="Record Detail" ><i class="fa fa-book blue"></i></a>';
        }

        return actions;
    },


};
//////-------------Editable Data Grid Scripts Ends-----------------------


jQuery.fn.extend({
    distinct: function (anArray) {
        var result = [];
        $.each(anArray, function (i, v) {
            if ($.inArray(v, result) == -1) result.push(v);
        });
        return result;
    },
    loadDropDowns: function (isLoad, data, ContainerId) {
        var ddls = this.find('select,datalist');
        var $parent = null;
        if (ContainerId == null) {
            if (this[0])
                $parent = $(this[0]);
        } else {
            $parent = $("#" + ContainerId);
        }

        if (data == null || data == "undefined") {
            data = "";
        }
        var method_ddlist = this.distinct($.map(ddls, function (ddl, index) {
            if (ddl.attributes['ddlist'] && $(ddl).attr('ddlist') != "undefined" && ddl.attributes['ddlist'].value != '' && ddl.attributes['ddlist'].value != null) {
                return ddl.attributes['ddlist'].value
            }
        }));
        var method_ddmultilist = this.distinct($.map(ddls, function (ddl, index) {
            if (ddl.attributes['ddlmultilist'] && $(ddl).attr('ddlmultilist') != "undefined" && ddl.attributes['ddlmultilist'].value != '' && ddl.attributes['ddlmultilist'].value != null) {
                return ddl.attributes['ddlmultilist'].value
            }
        }));
        var method_ddSuggestionlistt = this.distinct($.map(ddls, function (ddl, index) {
            if (ddl.attributes['ddSuggestionlist'] && $(ddl).attr('ddSuggestionlist') != "undefined" && ddl.attributes['ddSuggestionlist'].value != '' && ddl.attributes['ddSuggestionlist'].value != null) {
                return ddl.attributes['ddSuggestionlist'].value
            }
        }));
        BackgroundLoaderShow(true);
        $.merge(method_ddlist, method_ddmultilist, method_ddSuggestionlistt);
        method_ddlist = $.unique(method_ddlist);
        if (method_ddlist != null && method_ddlist != '') {//  if (ddl.attributes['ddlist'] && $(ddl).attr('ddlist') != "undefined") {
            return MDVisionService.lookups(method_ddlist, isLoad, data).done(function (results) {

                //if (isLoad && results) {
                if (results) {

                    utility.appendLookUpOptions(results, method_ddlist, $parent);
                }
                BackgroundLoaderShow(false);
            });
        }

        // if find any  select that not have ddlist attributes ignore it.
        if (method_ddSuggestionlistt.length == 0 || method_ddmultilist.length == 0 || method_ddlist.length == 0)
            BackgroundLoaderShow(false);

        return $.when.apply(null).promise();
    },

    loadDropDownsWithTitle: function (isLoad, data, ContainerId) {
        var ddls = this.find('select,datalist');
        var contrainerid = null;
        if (ContainerId == null) {
            if (this[0])
                contrainerid = this[0].id;
            var parent = $(this[0]).parent().attr('id');  // in case where contrainerid is multiple times in DOM append its parent it to be specific. By: Arsalan javed
            if (parent)
                contrainerid = parent + " #" + contrainerid;
        } else {
            contrainerid = ContainerId;
        }

        if (data == null || data == "undefined") {
            data = "";
        }
        var method_ddlist = $.unique($.map(ddls, function (ddl, index) {
            if (ddl.attributes['ddlist'] && $(ddl).attr('ddlist') != "undefined" && ddl.attributes['ddlist'].value != '' && ddl.attributes['ddlist'].value != null) {
                return ddl.attributes['ddlist'].value
            }
        }));
        var method_ddmultilist = $.unique($.map(ddls, function (ddl, index) {
            if (ddl.attributes['ddlmultilist'] && $(ddl).attr('ddlmultilist') != "undefined" && ddl.attributes['ddlmultilist'].value != '' && ddl.attributes['ddlmultilist'].value != null) {
                return ddl.attributes['ddlmultilist'].value
            }
        }));
        var method_ddSuggestionlistt = $.unique($.map(ddls, function (ddl, index) {
            if (ddl.attributes['ddSuggestionlist'] && $(ddl).attr('ddSuggestionlist') != "undefined" && ddl.attributes['ddSuggestionlist'].value != '' && ddl.attributes['ddSuggestionlist'].value != null) {
                return ddl.attributes['ddSuggestionlist'].value
            }
        }));
        BackgroundLoaderShow(true);
        $.merge(method_ddlist, method_ddmultilist, method_ddSuggestionlistt);
        method_ddlist = $.unique(method_ddlist);
        if (method_ddlist != null && method_ddlist != '') {//  if (ddl.attributes['ddlist'] && $(ddl).attr('ddlist') != "undefined") {
            return MDVisionService.lookups(method_ddlist, isLoad, data).done(function (results) {

                //if (isLoad && results) {
                if (results) {

                    utility.appendLookUpOptionsWithTitle(results, method_ddlist, contrainerid);
                }
                BackgroundLoaderShow(false);
            });
        }

        // if find any  select that not have ddlist attributes ignore it.
        if (method_ddSuggestionlistt.length == 0 || method_ddmultilist.length == 0 || method_ddlist.length == 0)
            BackgroundLoaderShow(false);

        return $.when.apply(null).promise();
    },

    selectDropDowns: function (bAuto, myJson) {
        var ddls = this.find('select');

        if (bAuto) {
            $.each(ddls, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['id'] && myJson.hasOwnProperty(v.attributes['id'].value)) {
                    var currentOptionValue = myJson[v.attributes['id'].value];
                    if ($.isNumeric(currentOptionValue) == false) {
                        $(v).find("option").each(function () {
                            if ($(this).text().toLowerCase() == currentOptionValue.toLowerCase())
                                $(v).val($(this).val());
                            else if ($(this).val().toLowerCase() == currentOptionValue.toLowerCase()) {
                                $(v).val($(this).val());
                            }
                        });

                    }

                    else {
                        //Modified by Azam Aftab Dated 7 Jan 2015 PMS - 3093
                        //if ($(v).find("option[value='" + currentOptionValue + "']").length <= 0) {
                        //    $(v).val("");
                        //} else {

                        $(v).val(currentOptionValue);
                        if ($(v).val() == null || $(v).val() == undefined) {
                            if (myJson["txt" + v.attributes['id'].value]) {
                                $(v).append("<option isActive='false' style='color:red' value='" + currentOptionValue + "'>" + myJson["txt" + v.attributes['id'].value] + "</option>");
                                $(v).val(currentOptionValue);
                                $(v).bind("change", function () {
                                    if ($(v).find(":selected").attr("isActive") == 'false') {
                                        $(v).val("");
                                        utility.DisplayMessages("Selected record is inactive.", 2);
                                    }


                                });
                            }
                            else
                                $(v).val("");
                        }


                        //}      //end by Azam Aftab Dated 7 Jan 2015 PMS - 3093
                    }
                }
            });
        }

        $.each(ddls, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).val(myJson[v.attributes['udata'].value]);
            }

        });

    },
    selectCheckBoxes: function (bAuto, myJson) {
        var a = this.find('[type=checkbox], [type=radio]');

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['id'] && myJson.hasOwnProperty(v.attributes['id'].value)) {
                    $(v).prop('checked', utility.getBool(myJson[v.attributes['id'].value]));
                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).prop('checked', utility.getBool(myJson[v.attributes['udata'].value]));
            }

        });

    },

    selectDropDownsByName: function (bAuto, myJson) {
        var ddls = this.find('select');        
        if (bAuto) {
            $.each(ddls, function (i, v) {
                var propName = null;
                if (v.attributes['name'] && v.attributes['name'].value.indexOf("_") > 0) {
                    propName = v.attributes['name'].value.substr(0, v.attributes['name'].value.indexOf("_"));                    
                }
                if (!v.attributes['udata'] && v.attributes['name'] && (myJson.hasOwnProperty(v.attributes['name'].value) || (propName && myJson.hasOwnProperty(propName)))) {
                    if(myJson[v.attributes['name'].value])
                    var currentOptionValue = myJson[v.attributes['name'].value];
                    else
                       var currentOptionValue = myJson[propName];
                    if ($.isNumeric(currentOptionValue) == false && currentOptionValue != null) {
                        $(v).find("option").each(function () {
                            if ($(this).text().toLowerCase() == currentOptionValue.toLowerCase())
                                $(v).val($(this).val());
                            else if ($(this).val().toLowerCase() == currentOptionValue.toLowerCase()) {
                                $(v).val($(this).val());
                            }
                        });
                    }
                    else
                        $(v).val(currentOptionValue);                    
                    
                }
            });
        }

        $.each(ddls, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).val(myJson[v.attributes['udata'].value]);
            }

        });

    },
    selectCheckBoxesByName: function (bAuto, myJson) {
        var a = this.find('[type=checkbox], [type=radio]');

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['name'] && myJson.hasOwnProperty(v.attributes['name'].value)) {
                    $(v).prop('checked', utility.getBool(myJson[v.attributes['name'].value]));
                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).prop('checked', utility.getBool(myJson[v.attributes['udata'].value]));
            }

        });

    },

    setx: function (value) {
        if (this.is("input") || this.is("select") || this.is("textarea")) {
            this.val(value);
        }
        else if (this.is("label") || this[0].tagName === "A") {
            this.html(value);
        }
    },
    getx: function () {
        if (this.is("input") || this.is("select") || this.is("textarea")) {
            return this.val();
        }
        else if (this.is("label") || this[0].tagName === "A") {
            return this.html();
        }
    },
    loadTextBoxes: function (bAuto, myJson) {
        var a = this.find("[type=number],[type=hidden],[type=text],[type=password], textarea");

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['id'] && myJson.hasOwnProperty(v.attributes['id'].value)) {
                    $(v).val(myJson[v.attributes['id'].value]);
                    //if ($(v).data("datepicker")) {
                    //    $(v).datepicker("setDate", myJson[v.attributes['id'].value]);
                    //}
                    //else {
                    //    $(v).val(myJson[v.attributes['id'].value]);
                    //}


                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).val(myJson[v.attributes['udata'].value]);
            }

        });
    },
    loadLabels: function (bAuto, myJson) {
        var a = this.find("label");

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['id'] && myJson.hasOwnProperty(v.attributes['id'].value)) {
                    $(v).text(myJson[v.attributes['id'].value]);
                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).text(myJson[v.attributes['udata'].value]);
            }

        });
    },
    loadImages: function (bAuto, myJson) {
        var a = this.find("img");

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['id'] && myJson.hasOwnProperty(v.attributes['id'].value)) {
                    $(v).attr('src', myJson[v.attributes['id'].value]);
                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).attr('src', myJson[v.attributes['udata'].value]);
            }

        });
    },



    loadTextBoxesByName: function (bAuto, myJson) {
        var a = this.find("[type=number],[type=hidden],[type=text],[type=password], textarea");

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['name'] && myJson.hasOwnProperty(v.attributes['name'].value)) {
                    $(v).val(myJson[v.attributes['name'].value]);
                    //if ($(v).data("datepicker")) {
                    //    $(v).datepicker("setDate", myJson[v.attributes['id'].value]);
                    //}
                    //else {
                    //    $(v).val(myJson[v.attributes['id'].value]);
                    //}


                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).val(myJson[v.attributes['udata'].value]);
            }

        });
    },
    loadLabelsByName: function (bAuto, myJson) {
        var a = this.find("label");

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['name'] && myJson.hasOwnProperty(v.attributes['name'].value)) {
                    $(v).text(myJson[v.attributes['name'].value]);
                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).text(myJson[v.attributes['udata'].value]);
            }

        });
    },
    loadImagesByName: function (bAuto, myJson) {
        var a = this.find("img");

        if (bAuto) {
            $.each(a, function (i, v) {
                if (!v.attributes['udata'] && v.attributes['name'] && myJson.hasOwnProperty(v.attributes['name'].value)) {
                    $(v).attr('src', myJson[v.attributes['name'].value]);
                }
            });
        }

        $.each(a, function (i, v) {
            if (v.attributes['udata'] && myJson.hasOwnProperty(v.attributes['udata'].value)) {
                $(v).attr('src', myJson[v.attributes['udata'].value]);
            }

        });
    },


    //bindMyJSON: function (bAuto, myJson, isValidation) {
    //    var self = this;
    //    isValidation = isValidation || false; // to set default value of parameter if isValidation === undefined ::: asjad 28-nov-2014
    //    if (!isValidation) {
    //        self.loadDropDowns(true).done(function () {
    //            self.selectDropDowns(bAuto, myJson);
    //        });
    //        self.loadLabels(bAuto, myJson);
    //        self.selectCheckBoxes(bAuto, myJson);
    //        self.loadTextBoxes(bAuto, myJson);
    //    }
    //    else
    //    {
    //        self.loadTextBoxes(bAuto, myJson);
    //    }

    //    self.find('[type=hidden],[type=text], textarea').each(function () { this.defaultValue = this.value; });
    //    self.find('[type=checkbox], [type=radio]').each(function () { this.defaultChecked = this.checked; });
    //    self.find('select option').each(function () { this.defaultSelected = this.selected; });

    //    var html = self.html();
    //    for (prop in myJson) {
    //        var regex = new RegExp("{@" + prop + "}", "g");
    //        html = html.replace(regex, myJson[prop]);
    //    }
    //    this.html(html);
    //},
    getMyJSON: function (CallBack, includeMultiselect) {
        var self = this;
        var item = {
        };
        self.find('[type=number],[type=hidden],[type=text],[type=password], textarea').each(function () {
            if (this.id) {
                item[this.id] = utility.encodeURIData(this.value.trim());
            }
        });
        self.find('[type=checkbox], [type=radio]').each(function () {
            if (this.id) {
                item[this.id] = this.checked;
            }
        });
        self.find('select').each(function (i, list) {
            if (list.id) {
                var l = $(list);
                var loptions = l.find('option');
                if (loptions.length > 0) {
                    if (l.attr("multiple") == "multiple" && includeMultiselect) {
                        if (l.val()) {
                            item[list.id] = $.map(l.val(), function (val, index) { return val ? val : null }).join(",");
                        } else {
                            item[list.id] = null;
                        }
                    } else {

                        var myoptions = l.find('option:selected');
                        myoptions.each(function (j, option) {
                            if ($(option).prop("selected")) {
                                var optionValue = option.value;

                                if (optionValue != "")
                                    optionValue = optionValue.replace(/&/gi, '');

                                item[list.id] = optionValue;

                                var optionText = option.text;

                                if (optionText != "")
                                    optionText = optionText.replace(/&/gi, '');

                                item[list.id + "_text"] = optionText;
                                if (l.attr('includeattibutesinjson') == "true") {
                                    $.each(option.attributes, function () {
                                        item[list.id + "_" + this.name] = this.value;
                                    });
                                }
                            }
                        });
                    }
                }
                else {
                    item[list.id] = "";
                }
            }
        });

        // Get Image src as JSON
        self.find('img').each(function () {
            if (this.id) {
                item[this.id] = $(this).attr('src');
            }
        });

        //! Get dives on control with id starting with custom-attribute_ & add its properties to JSON Items list
        self.find('div[id^="custom-attribute_"]').each(function () {
            $.each(this.attributes, function () {
                if (this.specified && this.id != "id") {
                    item[this.name] = this.value;
                }
            });
        });



        //return escape(JSON.stringify(item));
        return JSON.stringify(item);
    },
    getMyJSONByName: function (CallBack) {
        var self = this;
        var item = {
        };
        self.find('[type=number],[type=hidden],[type=text],[type=password], textarea').each(function () {
            if (this.name) {
                //item[this.name] = utility.encodeURIData(this.value.trim());
                item[this.name] = this.value.trim();
            }
        });
        self.find('[type=checkbox], [type=radio]').each(function () {
            if (this.name) {
                item[this.name] = this.checked;
                item[$(this).attr('value')] = this.checked;
            }
        });

        self.find('ul li').each(function () {
            var isItemActive = $(this).attr("class") != null ? $(this).attr("class").toLowerCase().indexOf("active") > -1 : false;
            var ulName = $($(this).parent()).attr("name") != null ? $($(this).parent()).attr("name") : "";
            if (ulName != "" && isItemActive == true) {
                item[ulName] = $(this).attr("id") != null ? $(this).attr("id") : $(this).val();
                /*
                     Change Implement BY: Muhammad Azhar Shahzad
                     Reason: To get selected Value Text to with id
                     Created Date: Dec 15, 2015
                 */
                item[ulName + "_text"] = $(this).text();
            }
        });

        self.find('select').each(function (i, list) {
            if (list.name) {
                var l = $(list);
                var loptions = l.find('option');
                if (loptions.length > 0) {
                    var myoptions = l.find('option:selected');
                    //Change by Azhar, for multi-select for Dropdown list on 7/22/2016
                    var selectedOptionValueIds = [];
                    var selectedOptionValueText = [];
                    var selectedOptionValueRefValues = [];
                    var isMultiSelect = l.attr('multiple');
                    myoptions.each(function (j, option) {
                        if ($(option).prop("selected")) {
                            var optionValue = option.value;
                            if (optionValue != "")
                                optionValue = optionValue.replace(/&/gi, '');

                            var optionText = option.text;
                            if (optionText != "")
                                optionText = optionText.replace(/&/gi, '');

                            if (isMultiSelect == null) {
                                item[list.name] = optionValue;
                                item[list.name + "_text"] = optionText;
                                item[list.name + "_RefValue"] = $(option).attr("refvalue");
                            } else {
                                selectedOptionValueIds.push(optionValue);
                                selectedOptionValueText.push(optionText);
                                selectedOptionValueRefValues.push($(option).attr("refvalue"));
                            }
                            if (l.attr('includeattibutesinjson') == "true") {
                                $.each(option.attributes, function () {
                                    item[list.id + "_" + this.name] = this.value;
                                });
                            }
                        }
                    });
                    if (isMultiSelect != null) {
                        item[list.name] = selectedOptionValueIds.join(',');
                        item[list.name + "_text"] = selectedOptionValueText.join(',');
                        item[list.name + "_RefValue"] = selectedOptionValueRefValues.join(',');
                    }
                }
                else {
                    item[list.name] = "";
                }
            }
        });

        // Get Image src as JSON
        self.find('img').each(function () {
            if (this.name) {
                item[this.name] = $(this).attr('src');
            }
        });

        //! Get dives on control with id starting with custom-attribute_ & add its properties to JSON Items list
        self.find('div[id^="custom-attribute_"]').each(function () {
            $.each(this.attributes, function () {
                if (this.specified && this.id != "id") {
                    item[this.name] = this.value;
                }
            });
        });



        //return escape(JSON.stringify(item));
        return JSON.stringify(item);
    },
    resetAllControls: function (DataTableIds) {
        var self = this;
        item = {
        }
        self.find('[type=hidden],[type=text],[type=password], textarea, ul').each(function () {
            $(this).val('');
        });
        self.find('[type=checkbox], [type=radio]').each(function () {
            this.checked = false;
        });
        self.find('select').each(function () {
            $(this).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(this).find('option:eq(0)').attr('selected', 'selected');
        });
        if (DataTableIds != null) {
            self.find(DataTableIds).each(function () {
                $(this).dataTable().fnDestroy();
                $(this).find("body").find("tr").remove();
                $(this).find('tbody').html('');
            });
        }
    },

    DisabledAll: function (IsAdd) {
        var self = this;
        self.find('[type=hidden],[type=text],[type=password], textarea').each(function () {
            subroutine(this, IsAdd);
        });
        self.find('[type=checkbox], [type=radio]').each(function () {
            subroutine(this, IsAdd);
        });
        self.find('select').each(function (i, list) {
            subroutine(this, IsAdd);
        });
        self.find('img').each(function () {
            subroutine(this, IsAdd);
        });
        self.find('div[id^="custom-attribute_"]').each(function () {
            subroutine(this, IsAdd);
        });

        if (IsAdd)
            self.addClass("disableAll");
        else
            self.removeClass("disableAll");

        function subroutine($obj, IsAdd) {
            if (IsAdd)
                $($obj).prop("disabled", true);
            else
                $($obj).removeAttr("disabled");
        };
    },
});



String.prototype.bindMyJSON = function (myJson) {
    var html = this.toString();
    for (prop in myJson) {
        var regex = new RegExp("{@" + prop + "}", "g");
        html = html.replace(regex, myJson[prop]);
    }
    return html;
}
$.fn.inputmask.prototype.listen = function () {

    var pasteEventName = (isIE ? 'paste' : 'input') + ".mask"

    this.$element
      .on("keyup.bs.inputmask", function () {

          $(this.parents('form')[0]).bootstrapValidator('revalidateField', this.name);
      });

}

$.fn.delayKeyup = function (callback, ms) {
    var timer = 0;
    var el = $(this);
    $(this).keyup(function () {
        clearTimeout(timer);
        timer = setTimeout(function () {
            callback(el)
        }, ms);
    });
    return $(this);
};

//---------------------------Paging Function------------------------//
function GetPaging(pagingDivId, TotalRecords, DisplayPages, ClassName, PageNo, rpp) {
    var RecordsPerPage = rpp != null ? rpp : 15;
    var CurrentPage = PageNo != null ? PageNo : 1;
    var TotalPages = Math.ceil(TotalRecords / RecordsPerPage);
    var totalPagesToDisplay = TotalPages > 0 ? TotalPages : 1;
    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < TotalRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : TotalRecords;
    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + TotalRecords + " Record(s)";

    if (ClassName != null) {
        ClassName = ClassName + ".";
    }
    else
        ClassName = "";
    $("#" + pagingDivId).attr("class", "col-sm-12 p-none");
    if ($("#" + pagingDivId + " #divShowingEntries"))
        $("#" + pagingDivId + " #divShowingEntries").remove();
    if ($("#" + pagingDivId + " #lnkPrevPages"))
        $("#" + pagingDivId + " #lnkPrevPages").remove();
    if ($("#" + pagingDivId + " #lnkNextPages"))
        $("#" + pagingDivId + " #lnkNextPages").remove();
    if ($("#" + pagingDivId + " #preLink"))
        $("#" + pagingDivId + " #preLink").remove();
    if ($("#" + pagingDivId + " #pageslink"))
        $("#" + pagingDivId + " #pageslink").remove();
    if ($("#" + pagingDivId + " #nextLink"))
        $("#" + pagingDivId + " #nextLink").remove();
    if ($("#" + pagingDivId + " #pagerParent"))
        $("#" + pagingDivId + " #pagerParent").remove();
    var ShowingEntriesDiv = '<div class="col-sm-6 p-none" id="divShowingEntries">' + showingText + '</div>';
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" href="javascript:void(0);" title="Previous Pages" onclick="PreviousClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages" href="javascript:void(0);" title="Next Pages" onclick="NextClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" href="javascript:void(0);" title="Previous Page" onclick="PreviousPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');' + ClassName + 'PreviousPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" href="javascript:void(0);" title="Next Page" onclick="NextPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');' + ClassName + 'NextPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    //if (TotalRecords > 15) {
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    // }
    var noOfDisplayPages = TotalPages < DisplayPages ? TotalPages : DisplayPages;
    for (i = 1 ; i <= noOfDisplayPages ; i++) {
        pagesLinkDiv += '<li><a href="javascript:void(0);" onclick="CurrentPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');' + ClassName + 'SelectedPageClick(' + i + ',this,' + TotalRecords + ',' + rpp + ',\'' + pagingDivId + '\');">' + i + '</a></li>';
    }
    // if (TotalRecords > 15) {
    pagesLinkDiv += nextLink;
    pagesLinkDiv += nextChunck;
    //}
    pagesLinkDiv += '</ul>';


    var pagerParent = '<div id="pagerParent" class="col-sm-6 p-none">' + pagesLinkDiv + '</div>';
    $("#" + pagingDivId).append(ShowingEntriesDiv, pagerParent);

    if (TotalPages == 1) {
        $("#" + pagingDivId).find('#pagerParent').find('li').each(function (index, item) {
            if ($(this).attr('Id') == "preLink" || $(this).attr('Id') == "nextLink") {
                $(this).find('a').addClass('disableAll');
            }
        });
    }
    if (PageNo == 1) {
        DisabledPreviousClicks(pagingDivId, true);
    }
    if (TotalPages == DisplayPages) {
        $('#' + pagingDivId + ' #pagerParent ul li:last-child a').addClass('disableAll');
    }
}
function CurrentPageSelected(currentPageNo, TotalPages, pagingDivId) {
    DisableNavigation(pagingDivId, TotalPages, currentPageNo)
    //if (currentPageNo == TotalPages) {
    //    DisabledNextClicks(pagingDivId, true);
    //}else{
    //    DisabledNextClicks(pagingDivId, false);
    //}
    //if (currentPageNo == 1){
    //    DisabledPreviousClicks(pagingDivId, true)
    //}else {
    //    DisabledPreviousClicks(pagingDivId, false);
    //}
}

function NextPageClickSelection(TotalPages, DisplayPages, pagingDivId, ClassName) {
    var currentPageNo = "";
    $('#' + pagingDivId + ' #pagerParent ul li').each(function () {
        if ($(this).attr("class") == "active") {
            currentPageNo = parseInt($(this).text());
        }
    });

    currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";

    var LastPageNo = $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(3)').text();
    setTimeout(function () {
        if (currentPageNo == TotalPages) {
            NextClick(TotalPages, DisplayPages, pagingDivId, ClassName, true);
        } else
            if (currentPageNo > Number(LastPageNo)) {

                NextClick(TotalPages, DisplayPages, pagingDivId, ClassName, true);
                $('#' + pagingDivId + ' #pagerParent ul li:nth-child(3)').attr("class", "active");

            } else {
                DisableNavigation(pagingDivId, TotalPages, currentPageNo)
            }
    }, 200);
}
function PreviousPageClickSelection(TotalPages, DisplayPages, pagingDivId, ClassName) {
    var currentPageNo = "";
    $('#' + pagingDivId + ' #pagerParent ul li').each(function () {
        if ($(this).attr("class") == "active") {
            //$(this).removeAttr("class");
            currentPageNo = parseInt($(this).text());
        }
    });
    currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";
    var LastPageNo = $('#' + pagingDivId + ' #pagerParent ul li:nth-child(3)').text();
    setTimeout(function () {
        if (currentPageNo == 0 || currentPageNo == 1 || currentPageNo == "") {
            PreviousClick(TotalPages, DisplayPages, pagingDivId, ClassName, true);
        }
        else if (currentPageNo < Number(LastPageNo)) {

            PreviousClick(TotalPages, DisplayPages, pagingDivId, ClassName, true);
            $('#' + pagingDivId + ' #pagerParent ul li:nth-child(4)').attr("class", "active");

        } else {
            DisableNavigation(pagingDivId, TotalPages, currentPageNo)
        }
    }, 200);
}

function DisabledPreviousClicks(pagingDivId, IsDisabled) {
    if (IsDisabled) {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(1) a').addClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').addClass('disableAll');
    } else {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(1) a').removeClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').removeClass('disableAll');
    }
}
function DisabledNextClicks(pagingDivId, IsDisabled) {
    if (IsDisabled) {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').addClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:last-child a').addClass('disableAll');
    } else {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').removeClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:last-child a').removeClass('disableAll');
    }
}
function DisableNavigation(pagingDivId, TotalPages, currentPageNo) {
    if (typeof currentPageNo == "undefiend" || currentPageNo == "" || currentPageNo == null) {
        currentPageNo = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    }
    if (currentPageNo == 1 && TotalPages == 1) {
        DisabledNextClicks(pagingDivId, true);
        DisabledPreviousClicks(pagingDivId, true);
    }
    else if (currentPageNo == TotalPages) {
        DisabledNextClicks(pagingDivId, true);
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').removeClass('disableAll');
    }
    else if (currentPageNo == 1 && currentPageNo < TotalPages) {
        DisabledPreviousClicks(pagingDivId, true);
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').removeClass('disableAll');
    }
    else if (currentPageNo == 1 && currentPageNo == TotalPages) {
        DisabledNextClicks(pagingDivId, false);
        DisabledPreviousClicks(pagingDivId, false);
    }
    else if (currentPageNo > 1 && currentPageNo < TotalPages) {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').removeClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').removeClass('disableAll');
    } else {
        DisabledNextClicks(pagingDivId, false);
        DisabledPreviousClicks(pagingDivId, false);
    }

}

function NextClick(TotalPages, DisplayPages, pagingDivId, ClassName, FullPageBack) {
    //pagingDivId=$(pagesLinkDiv).attr("id");
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" href="javascript:void(0);" title="Previous Pages" onclick="PreviousClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages" href="javascript:void(0);" title="Next Pages" onclick="NextClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" href="javascript:void(0);" title="Previous Page" onclick="PreviousPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');' + ClassName + 'PreviousPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" href="javascript:void(0);" title="Next Page" onclick="NextPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');' + ClassName + 'NextPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    start = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    var TotalLi = $('#' + pagingDivId + ' #pagerParent ul li').length;
    last = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ')').text());
    if (last < TotalPages) {
        last = last + 1;
        //$("#pageslink").empty();
        var pagesLinks = "";
        var currentPageNo = 1;
        for (i = last ; i <= TotalPages && (currentPageNo <= DisplayPages) ; i++) {
            if (TotalPages == last) {

                pagesLinkDiv += '<li class="active"><a href="javascript:void(0);" onclick="CurrentPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');' + ClassName + 'SelectedPageClick(' + i + ',this,0,0,\'' + pagingDivId + '\');">' + i + '</a></li>';
            } else {
                pagesLinkDiv += '<li><a href="javascript:void(0);" onclick="CurrentPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');' + ClassName + 'SelectedPageClick(' + i + ',this,0,0,\'' + pagingDivId + '\');">' + i + '</a></li>';
            }

            currentPageNo += 1;
        }

        pagesLinkDiv += nextLink;
        pagesLinkDiv += nextChunck;
        pagesLinkDiv += '</ul>';
        //var pagerParent = '<div id="pagerParent" class="col-sm-6">' + pagesLinkDiv + '</div>';
        $("#" + pagingDivId + " #pagerParent").html(pagesLinkDiv);
        if (!FullPageBack) {
            setTimeout(function () {
                currentPageNo = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
                $('#' + pagingDivId + ' #pagerParent ul li:eq(2) a').trigger("click");
                DisableNavigation(pagingDivId, TotalPages, currentPageNo);
            }, 100);
        } else {
            currentPageNo = last;
            DisableNavigation(pagingDivId, TotalPages, currentPageNo);
        }
    } else if (last == TotalPages) {
        var currentpage = $('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ') a').text();
        if (isNaN(currentpage) || TotalPages != Number(currentpage)) {
            $('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ') a').trigger("click");
        }
        DisableNavigation(pagingDivId, TotalPages, TotalPages);
    } else {
        setTimeout(function () {
            DisableNavigation(pagingDivId, TotalPages, currentPageNo);
        }, 200);
    }
}
function PreviousClick(TotalPages, DisplayPages, pagingDivId, ClassName, FullPageBack) {
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" href="javascript:void(0);" title="Previous Pages" onclick="PreviousClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages" href="javascript:void(0);" title="Next Pages" onclick="NextClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" href="javascript:void(0);" title="Previous Page" onclick="PreviousPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');' + ClassName + 'PreviousPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" href="javascript:void(0);" title="Next Page" onclick="NextPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');' + ClassName + 'NextPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    start = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    var TotalLi = $('#' + pagingDivId + ' #pagerParent ul li').length;
    last = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ')').text());
    DisplayPages = parseInt(DisplayPages);
    last = last + 1;
    if (start > 1) {
        for (i = start - DisplayPages ; i < start  ; i++) {
            if (i > 0) {
                pagesLinkDiv += '<li><a href="javascript:void(0);" onclick="CurrentPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');' + ClassName + 'SelectedPageClick(' + i + ',this,0,0,\'' + pagingDivId + '\');">' + i + '</a></li>';
            }
        }
        pagesLinkDiv += nextLink;
        pagesLinkDiv += nextChunck;
        pagesLinkDiv += '</ul>';
        $("#" + pagingDivId + " #pagerParent").html(pagesLinkDiv);
        if (!FullPageBack) {
            setTimeout(function () {
                $('#' + pagingDivId + ' #pagerParent ul li:eq(2) a').trigger("click");
                currentPageNo = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
                DisableNavigation(pagingDivId, TotalPages, currentPageNo);
            }, 100);
        }

    }
    if (start == 1) {
        setTimeout(function () {
            DisableNavigation(pagingDivId, TotalPages, 1);
        }, 200);
    }

}
//---------------------------Paging Function Ends------------------------//

function disableInjection() {
    document.onkeypress = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123 || event.keyCode == 93) {
            //alert('No F-12');
            return false;
        }
    }
    document.onkeydown = function (event) {
        event = (event || window.event);
        if (event.keyCode == 123 || event.keyCode == 93) {
            //alert('No F-keys');
            return false;
        }
    }
    $(this).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    shortcut.add("Ctrl+Shift+I", function () {

    });
    shortcut.add("Ctrl+U", function () {

    });
    shortcut.add("Ctrl+S", function () {

    });
}
function openPopup() {
    var wndw = window.open('http://localhost/MDVision.IEHR/MDVisionDefault.aspx#', '', 'fullscreen=yes', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no');
    wndw.onload = function () {
        wndw.document.onkeypress = function (event) {
            event = (event || window.event);
            if (event.keyCode == 123 || event.keyCode == 93) {
                //alert('No F-12');
                return false;
            }
        }
        wndw.document.onkeydown = function (event) {
            event = (event || window.event);
            if (event.keyCode == 123 || event.keyCode == 93) {
                //alert('No F-keys');
                return false;
            }
        }
        $(wndw.document).bind("contextmenu", function (e) {
            e.preventDefault();
        });
        wndw.shortcut.add("Ctrl+Shift+I", function () {

        });
        wndw.shortcut.add("Ctrl+U", function () {

        });
        wndw.shortcut.add("Ctrl+S", function () {

        });
        wndw.onunload = function () {
            window.alert('hola!');
        };
    }
}



//---------------------------Paging Function Admin Panels------------------------//
//PagingPanelControlID, iTotalDisplayRecords, PagesToDisplay, ClassControlName, CurrentPage, RecordsPerPage, SearchFunc
function GetPagingAdmin(pagingDivId, TotalRecords, DisplayPages, ClassName, PageNo, rpp, SearchFunc) {
    var RecordsPerPage = rpp != null ? rpp : 15;
    var CurrentPage = PageNo != null ? PageNo : 1;
    var TotalPages = Math.ceil(TotalRecords / RecordsPerPage);
    var totalPagesToDisplay = TotalPages > 0 ? TotalPages : 1;
    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < TotalRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : TotalRecords;
    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + TotalRecords + " Record(s)";

    if (ClassName != null) {
        ClassName = ClassName + ".";
    }
    else
        ClassName = "";
    $("#" + pagingDivId).attr("class", "col-sm-12 p-none");
    if ($("#" + pagingDivId + " #divShowingEntries"))
        $("#" + pagingDivId + " #divShowingEntries").remove();
    if ($("#" + pagingDivId + " #lnkPrevPages"))
        $("#" + pagingDivId + " #lnkPrevPages").remove();
    if ($("#" + pagingDivId + " #lnkNextPages"))
        $("#" + pagingDivId + " #lnkNextPages").remove();
    if ($("#" + pagingDivId + " #preLink"))
        $("#" + pagingDivId + " #preLink").remove();
    if ($("#" + pagingDivId + " #pageslink"))
        $("#" + pagingDivId + " #pageslink").remove();
    if ($("#" + pagingDivId + " #nextLink"))
        $("#" + pagingDivId + " #nextLink").remove();
    if ($("#" + pagingDivId + " #pagerParent"))
        $("#" + pagingDivId + " #pagerParent").remove();
    var noOfDisplayPages = TotalPages < DisplayPages ? TotalPages : DisplayPages;
    var ShowingEntriesDiv = '<div class="col-sm-6 p-none" id="divShowingEntries">' + showingText + '</div>';
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" href="javascript:void(0);" title="Previous Pages" onclick="AdminPreviousClick(' + TotalPages + ',' + noOfDisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages" href="javascript:void(0);" title="Next Pages" onclick="AdminNextClick(' + TotalPages + ',' + noOfDisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" href="javascript:void(0);" title="Previous Page" onclick="PreviousAdminPageClickSelection(' + TotalPages + ',' + noOfDisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');PreviousPageAdminClick(' + TotalPages + ',' + noOfDisplayPages + ',\'' + pagingDivId + '\',' + SearchFunc + ');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" href="javascript:void(0);" title="Next Page" onclick="NextAdminPageClickSelection(' + TotalPages + ',' + noOfDisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');NextPageAdminClick(' + TotalPages + ',' + noOfDisplayPages + ',\'' + pagingDivId + '\',' + SearchFunc + ');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    //if (TotalRecords > 15) {
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    // }

    for (i = 1 ; i <= noOfDisplayPages ; i++) {
        pagesLinkDiv += '<li><a href="javascript:void(0);" onclick="CurrentAdminPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');SelectedPageAdminClick(' + i + ',this,' + TotalRecords + ',' + rpp + ',\'' + pagingDivId + '\',' + SearchFunc + ');">' + i + '</a></li>';
    }
    // if (TotalRecords > 15) {
    pagesLinkDiv += nextLink;
    pagesLinkDiv += nextChunck;
    //}
    pagesLinkDiv += '</ul>';


    var pagerParent = '<div id="pagerParent" class="col-sm-6 p-none">' + pagesLinkDiv + '</div>';
    $("#" + pagingDivId).append(ShowingEntriesDiv, pagerParent);

    if (TotalPages == 1) {
        $("#" + pagingDivId).find('#pagerParent').find('li').each(function (index, item) {
            if ($(this).attr('Id') == "preLink" || $(this).attr('Id') == "nextLink") {
                $(this).find('a').addClass('disableAll');
            }
        });
    }
    if (PageNo == 1) {
        DisabledAdminPreviousClicks(pagingDivId, true);
    }
    if (TotalPages == noOfDisplayPages) {
        $('#' + pagingDivId + ' #pagerParent ul li:last-child a').addClass('disableAll');
    }
}
function CurrentAdminPageSelected(currentPageNo, TotalPages, pagingDivId) {
    DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo)
    //if (currentPageNo == TotalPages) {
    //    DisabledAdminNextClicks(pagingDivId, true);
    //}else{
    //    DisabledAdminNextClicks(pagingDivId, false);
    //}
    //if (currentPageNo == 1){
    //    DisabledAdminPreviousClicks(pagingDivId, true)
    //}else {
    //    DisabledAdminPreviousClicks(pagingDivId, false);
    //}
}

function NextAdminPageClickSelection(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc) {
    var currentPageNo = "";
    $('#' + pagingDivId + ' #pagerParent ul li').each(function () {
        if ($(this).attr("class") == "active") {
            currentPageNo = parseInt($(this).text());
        }
    });

    currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";

    var LastPageNo = $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(3)').text();
    setTimeout(function () {
        if (currentPageNo == TotalPages) {
            AdminNextClick(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc, true);
        } else
            if (currentPageNo > Number(LastPageNo)) {

                AdminNextClick(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc, true);
                $('#' + pagingDivId + ' #pagerParent ul li:nth-child(3)').attr("class", "active");

            } else {
                DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo)
            }
    }, 200);
}
function PreviousAdminPageClickSelection(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc) {
    var currentPageNo = "";
    $('#' + pagingDivId + ' #pagerParent ul li').each(function () {
        if ($(this).attr("class") == "active") {
            //$(this).removeAttr("class");
            currentPageNo = parseInt($(this).text());
        }
    });
    currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";
    var LastPageNo = $('#' + pagingDivId + ' #pagerParent ul li:nth-child(3)').text();
    setTimeout(function () {
        if (currentPageNo == 0 || currentPageNo == 1 || currentPageNo == "") {
            AdminPreviousClick(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc, true);
        }
        else if (currentPageNo < Number(LastPageNo)) {

            AdminPreviousClick(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc, true);
            $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(3)').attr("class", "active");

        } else {
            DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo)
        }
    }, 200);
}

function DisabledAdminPreviousClicks(pagingDivId, IsDisabled) {
    if (IsDisabled) {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(1) a').addClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').addClass('disableAll');
    } else {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(1) a').removeClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').removeClass('disableAll');
    }
}
function DisabledAdminNextClicks(pagingDivId, IsDisabled) {
    if (IsDisabled) {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').addClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:last-child a').addClass('disableAll');
    } else {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').removeClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:last-child a').removeClass('disableAll');
    }
}
function DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo) {
    if (typeof currentPageNo == "undefiend" || currentPageNo == "" || currentPageNo == null) {
        currentPageNo = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    }
    if (currentPageNo == 1 && TotalPages == 1) {
        DisabledAdminNextClicks(pagingDivId, true);
        DisabledAdminPreviousClicks(pagingDivId, true);
    }
    else if (currentPageNo == TotalPages) {
        DisabledAdminNextClicks(pagingDivId, true);
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').removeClass('disableAll');
    }
    else if (currentPageNo == 1 && currentPageNo < TotalPages) {
        DisabledAdminPreviousClicks(pagingDivId, true);
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').removeClass('disableAll');
    }
    else if (currentPageNo == 1 && currentPageNo == TotalPages) {
        DisabledAdminNextClicks(pagingDivId, false);
        DisabledAdminPreviousClicks(pagingDivId, false);
    }
    else if (currentPageNo > 1 && currentPageNo < TotalPages) {
        $('#' + pagingDivId + ' #pagerParent ul li:nth-last-child(2) a').removeClass('disableAll');
        $('#' + pagingDivId + ' #pagerParent ul li:nth-child(2) a').removeClass('disableAll');
    } else {
        DisabledAdminNextClicks(pagingDivId, false);
        DisabledAdminPreviousClicks(pagingDivId, false);
    }

}

function AdminNextClick(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc, FullPageBack) {
    //pagingDivId=$(pagesLinkDiv).attr("id");
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" href="javascript:void(0);" title="Previous Pages" onclick="AdminPreviousClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages" href="javascript:void(0);" title="Next Pages" onclick="AdminNextClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" href="javascript:void(0);" title="Previous Page" onclick="PreviousAdminPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');PreviousPageAdminClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',' + SearchFunc + ');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" href="javascript:void(0);" title="Next Page" onclick="NextAdminPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');NextPageAdminClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',' + SearchFunc + ');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    start = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    var TotalLi = $('#' + pagingDivId + ' #pagerParent ul li').length;
    last = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ')').text());
    if (last < TotalPages) {
        last = last + 1;
        //$("#pageslink").empty();
        var pagesLinks = "";
        var currentPageNo = 1;
        for (i = last ; i <= TotalPages && (currentPageNo <= DisplayPages) ; i++) {
            if (TotalPages == last) {

                pagesLinkDiv += '<li class="active"><a href="javascript:void(0);" onclick="CurrentAdminPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');SelectedPageAdminClick(' + i + ',this,0,0,\'' + pagingDivId + '\',' + SearchFunc + ');">' + i + '</a></li>';
            } else {
                pagesLinkDiv += '<li><a href="javascript:void(0);" onclick="CurrentAdminPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');SelectedPageAdminClick(' + i + ',this,0,0,\'' + pagingDivId + '\',' + SearchFunc + ');">' + i + '</a></li>';
            }

            currentPageNo += 1;
        }

        pagesLinkDiv += nextLink;
        pagesLinkDiv += nextChunck;
        pagesLinkDiv += '</ul>';
        //var pagerParent = '<div id="pagerParent" class="col-sm-6">' + pagesLinkDiv + '</div>';
        $("#" + pagingDivId + " #pagerParent").html(pagesLinkDiv);
        if (!FullPageBack) {
            setTimeout(function () {
                currentPageNo = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
                $('#' + pagingDivId + ' #pagerParent ul li:eq(2) a').trigger("click");
                DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo);
            }, 100);
        } else {
            currentPageNo = last;
            DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo);
        }
    } else if (last == TotalPages) {
        $('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ') a').trigger("click");
        DisableAdminNavigation(pagingDivId, TotalPages, TotalPages);
    } else {
        setTimeout(function () {
            DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo);
        }, 200);
    }
}
function AdminPreviousClick(TotalPages, DisplayPages, pagingDivId, ClassName, SearchFunc, FullPageBack) {
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" href="javascript:void(0);" title="Previous Pages" onclick="AdminPreviousClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages" href="javascript:void(0);" title="Next Pages" onclick="AdminNextClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" href="javascript:void(0);" title="Previous Page" onclick="PreviousAdminPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');PreviousPageAdminClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',' + SearchFunc + ');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" href="javascript:void(0);" title="Next Page" onclick="NextAdminPageClickSelection(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\',' + SearchFunc + ');NextPageAdminClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',' + SearchFunc + ');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    start = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    var TotalLi = $('#' + pagingDivId + ' #pagerParent ul li').length;
    last = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ')').text());
    DisplayPages = parseInt(DisplayPages);
    last = last + 1;
    if (start > 1) {
        for (i = start - DisplayPages ; i < start  ; i++) {
            if (i > 0) {
                pagesLinkDiv += '<li><a href="javascript:void(0);" onclick="CurrentAdminPageSelected(' + i + ',' + TotalPages + ',\'' + pagingDivId + '\');\SelectedPageAdminClick(' + i + ',this,0,0,\'' + pagingDivId + '\',' + SearchFunc + ');">' + i + '</a></li>';
            }
        }
        pagesLinkDiv += nextLink;
        pagesLinkDiv += nextChunck;
        pagesLinkDiv += '</ul>';
        $("#" + pagingDivId + " #pagerParent").html(pagesLinkDiv);
        if (!FullPageBack) {
            setTimeout(function () {
                $('#' + pagingDivId + ' #pagerParent ul li:eq(2) a').trigger("click");
                currentPageNo = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
                DisableAdminNavigation(pagingDivId, TotalPages, currentPageNo);
            }, 100);
        }

    }
    if (start == 1) {
        setTimeout(function () {
            DisableAdminNavigation(pagingDivId, TotalPages, 1);
        }, 200);
    }

}


function SelectedPageAdminClick(PageNo, objPage, TotalRecords, RecordsPerpage, pagingDivId, SearchFunc) {
    // Change Background Color to Black for selected page
    $('#' + pagingDivId + " li").each(function () {
        if ($(this).text() == PageNo) {
            $(this).attr("class", "active");
        }
        else
            $(this).removeAttr("class");
    });
    RecordsPerpage = RecordsPerpage == null || RecordsPerpage == 0 ? 15 : RecordsPerpage;
    PageNo = PageNo == null || PageNo == 0 ? 1 : PageNo;
    setTimeout(SearchFunc(0, PageNo, RecordsPerpage), 5);
}
function PreviousPageAdminClick(TotalPages, DisplayPages, pagingDivId, SearchFunc) {
    var currentPageNo = "";
    $('#' + pagingDivId + " li").each(function () {
        if ($(this).attr("class") == "active") {
            $(this).removeAttr("class");
            currentPageNo = parseInt($(this).text());
        }

    });
    currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

    if (currentPageNo != "" && currentPageNo > 0) {

        setTimeout(SearchFunc(0, currentPageNo, 15), 5);

    }
}

function NextPageAdminClick(TotalPages, DisplayPages, pagingDivId, SearchFunc) {

    var currentPageNo = "";
    $('#' + pagingDivId + " li").each(function () {
        if ($(this).attr("class") == "active") {
            $(this).removeAttr("class");
            currentPageNo = parseInt($(this).text());
        }
    });

    currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
    if (currentPageNo != "" && currentPageNo <= TotalPages) {

        setTimeout(SearchFunc(0, currentPageNo, 15), 5);
    }
}


function CreatePagination(responseCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, SearchFunc) {
    if (responseCount > 0) {
        $('#' + PagingPanelControlID).css("display", "inline");
        //Showing 1 to 15 of 15 entries
        var RecordsPerPage = rpp != null ? rpp : 15;
        var CurrentPage = PageNo != null ? PageNo : 1;

        if (PageNo == "" || PageNo == 1) {
            CurrentPage = 1;
            PageNo = null;
        }

        var PagesToShow = Math.ceil(iTotalDisplayRecords / RecordsPerPage);
        var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
        if (PageNo == null || $('#' + PagingPanelControlID).html() == '' || $('#' + PagingPanelControlID + ' li').length > (PagesToDisplay + 4) || $('#' + PagingPanelControlID + ' li').length > (PagesToShow + 4)) {
            utility.GetCustomPagingAdmin(PagingPanelControlID, iTotalDisplayRecords, PagesToDisplay, ClassControlName, CurrentPage, RecordsPerPage, SearchFunc);
        }

        var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : iTotalDisplayRecords;
        var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + iTotalDisplayRecords + " Record(s)";
        $('#' + PagingPanelControlID + " #divShowingEntries").text(showingText);
        // Change Background Color to Black for selected page
        $('#' + PagingPanelControlID + " li").each(function () {
            if ($(this).text() == CurrentPage) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
    }
    else {
        $('#' + PagingPanelControlID).css("display", "none");
        var PanelId = PagingPanelControlID.split(' ');
        //Start 25-06-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        if ($('#' + PanelId[0]).find('.dataTables_empty').length > 0) {
            $('#' + PanelId[0]).find('[class*=datatables-footer]').addClass('hidden');
        }
        //End 25-06-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    }
}

//---------------------------Paging Function Ends------------------------//


// ajax retry for IE

ajaxRetryService = function (ajaxCall, retries) {
    var _deferred = jQuery.Deferred();
    var _tryCounter = 0;
    var _retries = retries - 1;
    var _retryCallback;
    var self = {};

    self.onEachTry = function (retryCallback) {
        _retryCallback = retryCallback;
        return self;
    };

    self.tryIt = function () {

        _tryCounter++;
        _retryCallback(_tryCounter);

        ajaxCall().done(function (results) {
            return _deferred.resolve(results);
        })
        .fail(function () {
            if (_tryCounter <= _retries) {
                customWait(1000);
                self.tryIt();
            } else {
                _deferred.reject();
            }
        });

        return _deferred.promise();
    };
    return self;
};

function customWait(ms) {
    var start = Date.now(),
        now = start;
    while (now - start < ms) {
        now = Date.now();
    }
}

// PolyFills for IE includes
if (!Array.prototype.includes) {
    Array.prototype.includes = function (searchElement /*, fromIndex*/) {
        'use strict';
        if (this == null) {
            throw new TypeError('Array.prototype.includes called on null or undefined');
        }

        var O = Object(this);
        var len = parseInt(O.length, 10) || 0;
        if (len === 0) {
            return false;
        }
        var n = parseInt(arguments[1], 10) || 0;
        var k;
        if (n >= 0) {
            k = n;
        } else {
            k = len + n;
            if (k < 0) { k = 0; }
        }
        var currentElement;
        while (k < len) {
            currentElement = O[k];
            if (searchElement === currentElement ||
               (searchElement !== searchElement && currentElement !== currentElement)) { // NaN !== NaN
                return true;
            }
            k++;
        }
        return false;
    };
}

function CustomLogOutRemovals() {

    localStorage.removeItem('SelectedPatientId');
    localStorage.removeItem('BillingSelectedPage');

    return true;
}

OrthopedicChartSkeleton = {
    BodyParts: [],
    CanvanId: "ChartCanvas",
    ParentControl: "",
    NotesId: 0,
    PatientId: 0,

    drawSkeleton: function () {

        var elem = document.getElementById(OrthopedicChartSkeleton.CanvanId);
        var context = elem.getContext('2d');
        OrthopedicChartSkeleton.drawImage(context).done(function () {

            elem.addEventListener('click', function (event) {

                event.preventDefault();
                event.stopPropagation();

                var x = event.offsetX;
                var y = event.offsetY;
                var isFromUnselect = false;
                if (x == 0 && y == 0) {
                    x = parseInt($(this).attr("x"));
                    y = parseInt($(this).attr("y"));
                    $(this).removeAttr("x").removeAttr("y");
                    isFromUnselect = true;
                }

                var SelectedBodyPart = null;
                $.each(OrthopedicChartSkeleton.BodyParts, function () {
                    var xy = this.Position.split(',');
                    var elem_ = OrthopedicChartSkeleton.drawCircle(parseInt(xy[0]), parseInt(xy[1]), false);
                    if (elem_.getContext('2d').isPointInPath(x, y)) {
                        SelectedBodyPart = this;
                        return false;
                    }
                });
                if (SelectedBodyPart) {

                    // In case of when body part is already selected then only open the Pop-up window.
                    if (isFromUnselect == false && SelectedBodyPart.IsSelected == true) {
                        OpenOrthopedicComplaints(OrthopedicChartSkeleton.ParentControl, SelectedBodyPart.BodyPart, OrthopedicChartSkeleton.NotesId, OrthopedicChartSkeleton.PatientId);
                        return;
                    }
                    $.each(OrthopedicChartSkeleton.BodyParts, function () {
                        if (this.BodyPart == SelectedBodyPart.BodyPart) {
                            this.IsSelected = (!JSON.parse(SelectedBodyPart.IsSelected));
                            return false;
                        }
                    });
                    setTimeout(function () {

                        context.clearRect(0, 0, elem.width, elem.height);
                        context.restore();
                        context.globalAlpha = 1;
                        OrthopedicChartSkeleton.drawImage(context).done(function () {
                            // Draw Body Parts 
                            $.each(OrthopedicChartSkeleton.BodyParts, function () {
                                var xy = this.Position.split(',');
                                OrthopedicChartSkeleton.drawCircle(parseInt(xy[0]), parseInt(xy[1]), this.IsSelected, true);
                            });
                        });
                    }, 200);

                    if (SelectedBodyPart.IsSelected == true)
                        OpenOrthopedicComplaints(OrthopedicChartSkeleton.ParentControl, SelectedBodyPart.BodyPart, OrthopedicChartSkeleton.NotesId, OrthopedicChartSkeleton.PatientId);
                }

            }, false);

            elem.addEventListener('mousemove', function (event) {

                event.preventDefault();
                event.stopPropagation();

                var x = event.offsetX;
                var y = event.offsetY;

                var IsBodyPart = null;
                $.each(OrthopedicChartSkeleton.BodyParts, function () {
                    var xy = this.Position.split(',');
                    var elem_ = OrthopedicChartSkeleton.drawCircle(parseInt(xy[0]), parseInt(xy[1]), false, false);
                    if (elem_.getContext('2d').isPointInPath(x, y)) {
                        IsBodyPart = this;
                        return false;
                    }
                });
                if (IsBodyPart) {
                    this.style.cursor = "pointer";
                    this.title = IsBodyPart.BodyPart;
                }
                else {
                    this.style.cursor = "default";
                    this.title = "";
                }
            });

            setTimeout(function () {
                // Draw Body Parts 
                $.each(OrthopedicChartSkeleton.BodyParts, function () {
                    var xy = this.Position.split(',');
                    OrthopedicChartSkeleton.drawCircle(parseInt(xy[0]), parseInt(xy[1]), this.IsSelected, true);
                });

            }, 500);
        });

    },

    drawImage: function (context) {
        var def = $.Deferred();
        var img = new Image();
        img.onload = function () {
            context.drawImage(img, 0, 0);
            def.resolve(true);
        };
        img.src = '../Content/images/skeleton.jpg';

        return def;
    },

    drawCircle: function (x, y, Isselected, IsFill) {

        var elem_ = document.getElementById(OrthopedicChartSkeleton.CanvanId);
        var cx = elem_.getContext('2d');
        cx.beginPath();
        if (IsFill)
            cx.globalAlpha = 0.7;
        cx.arc(parseInt(x), parseInt(y), 12, 0, 2 * Math.PI);

        if (IsFill) {
            if (Isselected == true)
                cx.fillStyle = "red";
            else
                cx.fillStyle = "#468cec";

            cx.fill();
        }

        return elem_;
    },

    unSelectBodyPart: function (BodyPart) {
        var Selected = null;
        if (OrthopedicChartSkeleton.ParentControl == "Clinical_OrthopedicChart") {
            $.each(Clinical_OrthopedicChart.BodyParts, function () {
                if (this.BodyPart == BodyPart && this.IsSelected == true) {
                    Selected = this;
                    return false;
                }
            });
        }
        else if (OrthopedicChartSkeleton.ParentControl == "Clinical_OrthopedicChartDetail") {
            $.each(Clinical_OrthopedicChartDetail.BodyParts, function () {
                if (this.BodyPart == BodyPart && this.IsSelected == true) {
                    Selected = this;
                    return false;
                }
            });
        }

        if (Selected) {
            var xy = Selected.Position.split(',');
            var e = new $.Event("click");
            e.pageX = xy[0];
            e.pageY = xy[1];
            $("#" + OrthopedicChartSkeleton.CanvanId).attr("x", xy[0]).attr("y", xy[1]);
            $("#" + OrthopedicChartSkeleton.CanvanId).trigger(e);
        }
    },
};