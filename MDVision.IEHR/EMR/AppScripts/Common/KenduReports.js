KenduReports = {
   
    inializeKenduGrid: function (DataSourceJSON, ColumnHeaders, fieldRows, ControlId) {
        if (ControlId == null) {
            ControlId = '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid";
        }
        if ($(ControlId).data("kendoGrid") != null) {
            $(ControlId).data("kendoGrid").destroy(); // destroy the Grid
            $(ControlId).empty(); // empty the Grid content (inner HTML)
        }
        $(ControlId).kendoGrid({
            //toolbar: ["pdf"],
            //pdf: {
            //    allPages: true,
            //    avoidLinks: true,
            //    paperSize: "A4",
            //    margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            //    landscape: true,
            //   repeatHeaders: true,
            //    template: $("#page-template").html(),
            //     scale: 0.55
                
            //},
            dataSource: {
                data: DataSourceJSON,
                schema: {
                    model: {
                        fields: fieldRows
                    }
                },
            },
            noRecords: true,
            dataBound: function () {
                //var grid = $(ControlId).data("kendoGrid");
                //for (var i = 0; i < grid.columns.length; i++) {
                //   grid.autoFitColumn(i);
                //}
                //$(ControlId + " table").removeAttr("style");
                //if (grid._data.length == 0) {
                //    var contentDiv = this.wrapper.children(".k-grid-content"),
                //    dataTable = contentDiv.children("table");
                //    if (!dataTable.find("tr").length) {
                //        dataTable.children("tbody").append("<tr colspan='" + this.columns.length + "'><td style='border: none;'> </td></tr>");
                //        if ($.browser.msie) {
                //            dataTable.width(this.wrapper.children(".k-grid-header").find("table").width());
                //            contentDiv.scrollLeft(1);
                //        }
                //    }
                //}
            },
            height: 550,
            scrollable: true,
            sortable: true,
            filterable: false,
            columns: ColumnHeaders
        });
    },
    inializeKenduGridForImmunization: function (DataSourceJSON, ColumnHeaders, fieldRows, ControlId) {
        if (ControlId == null) {
            ControlId = '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid";
        }
        if ($(ControlId).data("kendoGrid") != null) {
            $(ControlId).data("kendoGrid").destroy(); // destroy the Grid
            $(ControlId).empty(); // empty the Grid content (inner HTML)
        }
        $(ControlId).kendoGrid({
            //toolbar: ["pdf"],
            //pdf: {
            //    allPages: true,
            //    avoidLinks: true,
            //    paperSize: "A4",
            //    margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
            //    landscape: true,
            //   repeatHeaders: true,
            //    template: $("#page-template").html(),
            //     scale: 0.55

            //},
            dataSource: {
                data: DataSourceJSON,
                schema: {
                    model: {
                        fields: fieldRows
                    }
                },
            },
            noRecords: true,
            dataBound: function () {
                var grid = $(ControlId).data("kendoGrid");
                for (var i = 0; i < grid.columns.length; i++) {
                    grid.autoFitColumn(i);
                }
                $(grid.dataSource._data).each(function (index, item) {
                    if (item.VoidDose == "True") {
                        grid.tbody.children(':nth-child(' + (index + 1) + ')').css('background-color', 'lightgray');
                    }
                });
            },
            // height: 550,
            scrollable: true,
            sortable: true,
            filterable: false,
            columns: ColumnHeaders
            
        });
    },
    getPDF: function (selector, Name) {
        if (selector == null) {
            selector = '#' + ReportsSSRSDashboard.params["PanelID"] + " #clinicalKenduGrid";
            Name = ReportsSSRSDashboard.ReportName;
        }
        kendo.drawing.drawDOM($(selector)).then(function (group) {
            kendo.drawing.pdf.saveAs(group, Name + ".pdf");
        });
       // kendo.drawing.drawDOM($(selector))
       //.then(function (group) {
       //    // Render the result as a PDF file
       //    return kendo.drawing.exportPDF(group, {
       //      //  paperSize: "auto",
       //      //  margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
       //        allPages: true,
       //         avoidLinks: true,
       //         paperSize: "A4",
       //    margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" },
       //         landscape: true,
       //         repeatHeaders: true,
       //         template: $("#page-template").html(),
       //         scale: 0.57
       //    });
       //})
       //.done(function (data) {
       //    // Save the PDF file
       //    kendo.saveAs({
       //        dataURI: data,
       //        fileName: "HR-Dashboard.pdf",
       //       // proxyURL: "//demos.telerik.com/kendo-ui/service/export"
       //    });
       //});
    },
}