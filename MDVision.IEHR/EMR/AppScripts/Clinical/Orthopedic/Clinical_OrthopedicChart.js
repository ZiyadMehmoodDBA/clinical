Clinical_OrthopedicChart = {

    params: [],
    bIsFirstLoad: true,
    BodyParts: [],

    Load: function (params) {
        Clinical_OrthopedicChart.params = params;

        if (Clinical_OrthopedicChart.params.PanelID != 'pnlOrthopedicChart') {
            Clinical_OrthopedicChart.params.PanelID = Clinical_OrthopedicChart.params.PanelID + ' #pnlOrthopedicChart';
        }
        else {
            Clinical_OrthopedicChart.params.PanelID = 'pnlOrthopedicChart';
        }

        var self = $('#' + Clinical_OrthopedicChart.params.PanelID);

        //Clinical_OrthopedicChart.BodyParts =
        //        [
        //            { "BodyPart": "Right Shoulder", "Position": "23,115", "IsSelected": "false" },
        //            { "BodyPart": "Left Shoulder", "Position": "175,115", "IsSelected": "false" },
        //            { "BodyPart": "Neck", "Position": "100,85", "IsSelected": "false" },
        //            { "BodyPart": "Back bone", "Position": "100,185", "IsSelected": "false" },
        //            { "BodyPart": "Right elbow", "Position": "15,230", "IsSelected": "false" },
        //            { "BodyPart": "Left elbow", "Position": "188,230", "IsSelected": "false" },
        //            { "BodyPart": "Left wrist", "Position": "183,315", "IsSelected": "false" },
        //            { "BodyPart": "Right wrist", "Position": "15,315", "IsSelected": "false" },
        //            { "BodyPart": "Right knee", "Position": "65,440", "IsSelected": "false" },
        //            { "BodyPart": "Left knee", "Position": "135,440", "IsSelected": "false" },
        //            { "BodyPart": "Left tarsal", "Position": "130,570", "IsSelected": "false" },
        //            { "BodyPart": "Right tarsal", "Position": "70,570", "IsSelected": "false" },
        //            { "BodyPart": "Left hip joint", "Position": "148,293", "IsSelected": "false" },
        //            { "BodyPart": "Right hip joint", "Position": "50,293", "IsSelected": "false" },

        //        ];

        Clinical_OrthopedicChart.LoadBodyParts();
        

    },

    LoadBodyParts: function () {

        Clinical_OrthopedicChart.LoadBodyParts_DBCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrthopedicChart.BodyParts = response.OrthopedicChat_JSON;

                OrthopedicChartSkeleton.BodyParts= response.OrthopedicChat_JSON;
                OrthopedicChartSkeleton.CanvanId= "ChartCanvas";
                OrthopedicChartSkeleton.ParentControl = "Clinical_OrthopedicChart";
                OrthopedicChartSkeleton.NotesId = Clinical_OrthopedicChart.params["NotesId"];
                OrthopedicChartSkeleton.PatientId = Clinical_OrthopedicChart.params["PatientId"];
                OrthopedicChartSkeleton.drawSkeleton();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },


    LoadNotesBodyPartsComplaints: function () {

        OrthopedicComplaints.LoadNotesBodyPartsComplaints_DBCall(Clinical_OrthopedicChart.params["NotesId"]).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                NotesBodyPartsComplaints_list = response.NotesBodyPartsComplaints_JSON;
                $("#pnlOrthopedicChart #list_Complaints").html('');
                if (NotesBodyPartsComplaints_list.length > 0) {
                    $.each(NotesBodyPartsComplaints_list, function () {

                        var li = '<li id="notes_bodypart_complaint' + this.ComplaintDetailId + '" bodypart="' + this.BodyPart + '" >'
                                   + '<span>' + this.Complaint + ' </span>'
                                      + '<a class="btn  btn-xs pull-right" href="#" onclick="Clinical_OrthopedicChart.deleteNotesBodyPartAndComplaint(' + this.NotesBodyPartId + ',' + this.ComplaintDetailId + ',\'' + this.BodyPart + '\')" title="Delete Record"><i class="fa fa-close red"></i></a>'
                                   + ' </li>';

                        $("#pnlOrthopedicChart #list_Complaints").append(li);
                    });

                    $("#pnlOrthopedicChart #btn_loadChart").removeAttr("disabled");
                    Clinical_ProgressNote.params["IsBodyPart"] = true;
                }
                else {
                    $("#pnlOrthopedicChart #list_Complaints").append("<li style='text-align:center' >No Complaint is added.</li>");
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    deleteNotesBodyPartAndComplaint: function (NotesBodyPartId, ComplaintDetailId, BodyPart) {

        if (event != null) {
            event.stopPropagation();
        }

        var IsDeleteBodyPartAssociation = true;
        if ($("#pnlOrthopedicChart #list_Complaints li[bodypart='" + BodyPart + "']").length > 1)
            IsDeleteBodyPartAssociation = false;

        OrthopedicComplaints.DeleteBodyPartAndComplaint(NotesBodyPartId, ComplaintDetailId, Clinical_OrthopedicChart.params["NotesId"], Clinical_OrthopedicChart.params["PatientId"], IsDeleteBodyPartAssociation).done(function (res) {
            if (res == true) {
                //Remove Row
                $("#pnlOrthopedicChart #list_Complaints").find("#notes_bodypart_complaint" + ComplaintDetailId).remove();
                if (IsDeleteBodyPartAssociation)
                {
                    OrthopedicChartSkeleton.CanvanId = "ChartCanvas";
                    OrthopedicChartSkeleton.ParentControl = "Clinical_OrthopedicChart";
                    OrthopedicChartSkeleton.unSelectBodyPart(BodyPart);
                }

                if ($("#pnlOrthopedicChart #list_Complaints li").length <= 0) {
                    $("#pnlOrthopedicChart #list_Complaints").append("<li style='text-align:center' >No Complaint is added.</li>");
                    $("#pnlOrthopedicChart #btn_loadChart").attr("disabled", "disabled");
                    Clinical_ProgressNote.params["IsBodyPart"] = false;
                }
            }
        });

    },

    LoadBodyParts_DBCall: function () {

        var objData = {};
        objData["NotesId"] = Clinical_OrthopedicChart.params["NotesId"];
        objData["commandType"] = "load_bodyparts";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    Open_OrthopedicChart: function () {

        var NotesId = Clinical_OrthopedicChart.params.NotesId;
        var PatientId = Clinical_OrthopedicChart.params.PatientId;
        var ProviderId = Clinical_OrthopedicChart.params.ProviderId;

        Clinical_OrthopedicChart.UnLoad();
        setTimeout(function () {
            var params = [];
            params["ParentCtrl"] = 'clinicalTabProgressNote';
            params["NotesId"] = NotesId;
            params["PatientId"] = PatientId;
            params["ProviderId"] = ProviderId;
            LoadActionPan('Clinical_OrthopedicChartDetail', params);
        }, 1000);
        
    },

    UnLoad: function () {

        UnloadActionPan(Clinical_OrthopedicChart.params.ParentCtrl, 'Clinical_OrthopedicChart');

    },

};