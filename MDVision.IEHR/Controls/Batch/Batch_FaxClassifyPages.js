Batch_FaxClassifyPages = {
    params: [],
    Load: function (params) {
        Batch_FaxClassifyPages.params = params;
        var self = $('#pnlBatch_FaxClassifyPages');
        // faizan ameen
        // bug EMR-2368
        // select fax as default in ddl.

        self.loadDropDowns(true).done(function(){
            var optionRef = $('#pnlBatch_FaxClassifyPages #ddlFolder option').filter(function () {
                return $(this).text() == 'Fax';
            }).prop("selected", true);
        
        
            $('#pnlBatch_FaxClassifyPages #ddlFolder').attr('disabled', true);
        });

       


    },
    saveFaxDocument: function() {
        var data = $('#frmBatch_FaxClassifyPages').getMyJSON();
        var obj = new Object();
        data = JSON.parse(data);
       
        obj["DocumentId"] = data.ddlFolder;
        if (obj["DocumentId"] == null || obj["DocumentId"] == undefined) {
            utility.DisplayMessages("Please select a folder", 3);
        }
        else {
            obj["UserId"] = data.ddlAssignUserto;
            obj["Comments"] = data.txtComments;
            obj["FileType"] = "application/pdf";
            var d = new Date();
            var n = d.getMilliseconds();
            obj["FilePath"] = "Fax " + (n * 30) + ".pdf";
            obj["IsActive"] = true;
            obj["FaxId"] = Batch_FaxClassifyPages.params["FaxId"];
            obj["IsConfidential"] = data.chkIsConfidential;
            var str = Batch_FaxClassifyPages.params["pdfBase64"];
            str = str.substring(28, str.length);
          //  chars = str.substring((str.length - 2), str.length);
               obj["FileStream"] = str;
         
            obj["Pages"] = "2";

            var strdata = JSON.stringify(obj);
            strdata = "faxDocData=" + strdata;
            MDVisionService.defaultService(strdata, "BATCH_FAX_CLINICAL", "SAVE_FAX_DOCUMENT").done(
                function (resp) {
                    resp = resp.substring(0, resp.length - 6);
                    if (resp == "Success") {
                        utility.DisplayMessages("Your document has been classified.", 1);
                    }
                    else {
                        utility.DisplayMessages("Your document could not be classified", 4);
                    }
                      
                });
            
            
            Batch_FaxClassifyPages.UnLoad();
        }


    },
    UnLoad: function () {

        if (Batch_FaxClassifyPages.params != null && Batch_FaxClassifyPages.params.ParentCtrl) {
            UnloadActionPan(Batch_FaxClassifyPages.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    }
}