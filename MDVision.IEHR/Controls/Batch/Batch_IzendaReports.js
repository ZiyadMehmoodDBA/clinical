Batch_IzendaReports = {
    bIsFirstLoad: true,
    params: [],
    Load: function(params) {
        Batch_IzendaReports.params = params;

        if (Batch_IzendaReports.bIsFirstLoad) {
            Batch_IzendaReports.bIsFirstLoad = false;

            var self;
            if (Batch_IzendaReports.params["PanelID"] !== "pnlBatchIzendaReports")
                self = $("#" + Batch_IzendaReports.params["PanelID"] + " #pnlBatchIzendaReports");
            else
                self = $("#pnlBatchIzendaReports");
        }

        Batch_IzendaReports.LoadIzendaReports();

        //$("a").on("click", alert('ckl'));

        //$("a").click(function () {
        //    alert('clciked');
        //});

        //var frame = document.getElementById('PreviewIzendaReports');
        //frame.contentWindow.postMessage('khawer', 'http://localhost:2502/Reporting/ReportViewer?rn=Patient\PopUp+Test');
        //window.addEventListener("message", receiveMessage, false);

    },

    receiveMessage: function (event)
    {
        if (event.origin !== "http://localhost:2502/Reporting/ReportViewer?rn=Patient\PopUp+Test")
            return;
        else
            window.addEventListener("message", receiveMessage, false);
    },

    onMyFrameLoad: function ($this) {
        //alert("Laoded");
        //alert($this);
        //setTimeout(function () {
        //    alert('page is loaded, One Minuste psseed');
        //}, 10000);
    },

    LoadIzendaReports: function ()
    {
       
      //  setTimeout(Batch_IzendaReports.CheckUserPrivileges, 10000);

        //// get the iframe in my documnet
        //var iframe = document.getElementById("PreviewIzendaReports");
        //// get the window associated with that iframe
        //var iWindow = iframe.contentWindow;
        //// wait for the window to load before accessing the content
        //iWindow.addEventListener("load", function () {
        //    // get the document from the window
        //    var doc = iframe.contentDocument || iframe.contentWindow.document;

        //    // find the target in the iframe content
        //     doc.getElementById("dropdownSettings").display="none";
            
        //});

        var loc = "";
        if (self == top) {
            loc = "http://localhost:2502/Reporting/ReportDesigner?clear=1&tab=Data%20Sources&SomeId=" + globalAppdata["AppUserId"] + "&SomeBit=false";
        }

        //var loc = "http://localhost:2502/Reporting/ReportDesigner?clear=1&tab=Data%20Sources&SomeId=" + globalAppdata["AppUserId"];
        document.getElementById('PreviewIzendaReports').setAttribute('src', loc);

        window.addEventListener("message",
            function (e)
            {
                if (e.origin !== 'http://localhost')
                {
                    return;
                }
                alert(e.data);
            },
            false);

        //var frame = document.getElementById('PreviewIzendaReports');
        //frame.contentWindow.postMessage('khawer', 'http://localhost');
        //window.addEventListener("message", receiveMessage, false);

        //setTimeout(Batch_IzendaReports.CheckUserPrivileges, 10000);
    },

    receiveMessage: function (event)
    {
        if (event.origin !== "http://localhost")
        return;
    },




    CheckUserPrivileges: function ()
    {
        if (globalAppdata["AppUserName"] != "mdvision")
        {
            $("#dropdownSettings").hide();
            //$('#PreviewIzendaReports').contents().find('#dropdownSettings').hide();
            //$("#topnav li:last").hide();
        }
    },

    iframeClick: function ()
    {
        window.location.href("ReportDesigner.aspx");
    },

    UnLoadTab: function ()
    {
        window.addEventListener("message",
           function (e)
           {
              alert(e.data);
           }, false);
        RemoveReportsTab("batchTabIzendaReports");
    }
}