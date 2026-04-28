Patient_AccountManager_Detail = {
    Load: function (params) {
        Patient_AccountManager_Detail.params = params;

        Patient_AccountManager_Detail.FillPatientDefaultData();

    },

    FillPatientDefaultData: function () {
        $('#Patient_AccountManager_Detail #spanUserName').html(Patient_AccountManager_Detail.params.UserName);
        $('#Patient_AccountManager_Detail #spanUserPassword').html(Patient_AccountManager_Detail.params.Password);
        $('#Patient_AccountManager_Detail #userName').html("Welcome " + Patient_AccountManager_Detail.params.FirstName + "!");
    },

    PrintLetter: function () {
        $("#Patient_AccountManager_Detail #printHTMlDiv").html($("#Patient_AccountManager_Detail #printLetter").html());
        //$("#Patient_AccountManager_Detail #printHTMlDiv").printMe();

        var div1 = $("<div></div>").html($("#Patient_AccountManager_Detail #printLetter").html());

        //$('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').printMe({ 'path': 'Content/Default/bootstrap.css,Content/Blue/theme.css,Content/Blue/theme-custom.css,Content/Blue/default.css,Content/Default/print-media.css' })
        var docType = '<!doctype html>';
        //$('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').show();
        //var docCnt = $('#Patient_AccountManager_Detail #printHTMlDiv')[0].outerHTML;

        var docCnt = div1[0].outerHTML;

        //$('#' + Bill_PatientStatement.params.PanelID + ' #accountStatementContainer').hide();

        



        var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                     + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                     + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;utility.myConfirm("Do you want to submit statement ?", function () {window.opener.Bill_PatientStatement.saveStatements("false");}, function () {},"<b>Confirm submission</b>");window.print(); } '
                     + '</script>'
                     + '</head>';
        var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=865, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
        var newWin = window.open("", "_blank", winAttr);
        writeDoc = newWin.document;
        writeDoc.open();
        writeDoc.write(docType + '<html>' + docHead + '<body>' + docCnt + '</body></html>');
        writeDoc.close();
        newWin.focus();


    },

    Unload: function () {
        UnloadActionPan(Patient_AccountManager_Detail.params.ParentCtrl, 'Patient_AccountManager_Detail');
    },
}