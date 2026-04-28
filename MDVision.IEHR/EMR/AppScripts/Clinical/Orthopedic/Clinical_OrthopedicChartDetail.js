Clinical_OrthopedicChartDetail = {

    params: [],
    bIsFirstLoad: true,
    BodyParts: [],

    Load: function (params) {
        Clinical_OrthopedicChartDetail.params = params;

        if (Clinical_OrthopedicChartDetail.params.PanelID != 'pnlOrthopedicChartDetail')
            Clinical_OrthopedicChartDetail.params.PanelID = Clinical_OrthopedicChartDetail.params.PanelID + ' #pnlOrthopedicChartDetail';
        else
            Clinical_OrthopedicChartDetail.params.PanelID = 'pnlOrthopedicChartDetail';

        var self = $('#' + Clinical_OrthopedicChartDetail.params.PanelID);

        Clinical_OrthopedicChartDetail.LoadBodyPartsByNotes().done(function () {
            //Load Problems Screen
            Clinical_OrthopedicChartDetail.SelectOrthoTab('Clinical_OrthoProblemList', 'btnOrthoProblems');

        });

        Clinical_OrthopedicChartDetail.initializeSlider();

    },

    SelectOrthoTab: function (TabId,ButtonId) {

        var currentTab = GetTab(TabId);
        if (currentTab) {

            var def = $.Deferred();

            var IsLoadFirst = true;
            // Is This Tab is already loaded in DOM
            if ($("#" + Clinical_OrthopedicChartDetail.params.PanelID + " #ctrlPanOrthoDetail #" + currentTab.PanelID).length > 0) {
                // Reopen Tab
                IsLoadFirst = false;
                if ($("#" + Clinical_OrthopedicChartDetail.params.PanelID + " #ctrlPanOrthoDetail #" + currentTab.PanelID).css('display') == "none") {
                    // close all screens.
                    $("#" + Clinical_OrthopedicChartDetail.params.PanelID + " #ctrlPanOrthoDetail div[id^=pnlOrtho]").css('display', 'none');
                    // reopen current screen.
                    $("#" + Clinical_OrthopedicChartDetail.params.PanelID + " #ctrlPanOrthoDetail #" + currentTab.PanelID).css('display', 'block');
                    def.resolve(true);
                }
                else {
                    // do nothing as current tab is clicked again.
                    def.resolve(true);
                }
            }
            else {
                // Open Tab
                $.get(currentTab.Path, { cache: false }, function (content) {
                    html = content;
                    $("#" + currentTab.Container).prepend(html);
                    $("#" + Clinical_OrthopedicChartDetail.params.PanelID + " #ctrlPanOrthoDetail div[id^=pnlOrtho]").css('display', 'none');
                    $("#" + currentTab.PanelID).css("display", "block");
                    def.resolve(true);
                });
            }

            def.done(function (res) {

                if (res)
                {
                    $("#mstrDivOrthoDetail span").removeClass('tab_selected');
                    $("#mstrDivOrthoDetail #" + ButtonId).parent().addClass('tab_selected');
                    var params = [];
                    params["NotesId"] = Clinical_OrthopedicChartDetail.params.NotesId;
                    params["PatientId"] = Clinical_OrthopedicChartDetail.params.PatientId;
                    params["NotesProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId;
                    params["ParentCtrl"] = "Clinical_OrthopedicChartDetail";
                    params["PanelID"] = Clinical_OrthopedicChartDetail.params.PanelID;
                    params["ActionPanContainer"] = Clinical_OrthopedicChartDetail.params.ActionPanContainer;
                    params["ContainerControlID"] = Clinical_OrthopedicChartDetail.params.ContainerControlID;

                    eval(currentTab.ContainerControlID + ".bIsFirstLoad=" + IsLoadFirst + "");
                    eval(currentTab.ContainerControlID + '.Load')(params);

                }
            });

        }
        else {
            console.log('Tab not found.');
        }


    },

    initializeSlider: function () {

        $("#pnlOrthopedicChartDetail #ortho_slider").slick({
            dots: false,
            infinite: true,
            vertical: true,
            centerMode: true,
            focusOnSelect: true,
            verticalSwiping: true,
            prevArrow: $('#pnlOrthopedicChartDetail .ortho-prev'),
            nextArrow: $('#pnlOrthopedicChartDetail .ortho-next'),
            arrows: true,
            slidesToShow: 5,
            slidesToScroll: 5

        });
        $("#pnlOrthopedicChartDetail #ortho_slider").on('afterChange', function (event, slick, currentSlide) {
            console.log($('#pnlOrthopedicChartDetail #ortho_slider div[data-slick-index=' + index + ']'));
        });

    },

    LoadBodyPartsByNotes: function () {
        var objDeffered = $.Deferred();
        Clinical_OrthopedicChartDetail.LoadBodyPartsByNotes_DBCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_OrthopedicChartDetail.BodyParts = response.OrthopedicChat_JSON;

                OrthopedicChartSkeleton.BodyParts = response.OrthopedicChat_JSON;
                OrthopedicChartSkeleton.CanvanId = "ChartCanvas";
                OrthopedicChartSkeleton.ParentControl = "Clinical_OrthopedicChartDetail";
                OrthopedicChartSkeleton.NotesId = Clinical_OrthopedicChartDetail.params["NotesId"];
                OrthopedicChartSkeleton.PatientId = Clinical_OrthopedicChartDetail.params["PatientId"];
                OrthopedicChartSkeleton.drawSkeleton();
                Clinical_OrthopedicChartDetail.LoadNotesBodyPartsComplaints();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            objDeffered.resolve();
        });
        return objDeffered;
    },

    LoadNotesBodyPartsComplaints: function () {

        OrthopedicComplaints.LoadNotesBodyPartsComplaints_DBCall(Clinical_OrthopedicChartDetail.params["NotesId"]).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                NotesBodyPartsComplaints_list = response.NotesBodyPartsComplaints_JSON;
                $("#pnlOrthopedicChartDetail #list_Complaints").html('');
                if (NotesBodyPartsComplaints_list.length > 0) {
                    $.each(NotesBodyPartsComplaints_list, function () {

                        var li = '<li id="notes_bodypart_complaint' + this.ComplaintDetailId + '" bodypart="' + this.BodyPart + '" >'
                                   + '<span>' + this.Complaint + ' </span>'
                                      + '<a class="btn  btn-xs pull-right" href="#" onclick="Clinical_OrthopedicChartDetail.deleteNotesBodyPartAndComplaint(' + this.NotesBodyPartId + ',' + this.ComplaintDetailId + ',\'' + this.BodyPart + '\')" title="Delete Record"><i class="fa fa-close red"></i></a>'
                                   + ' </li>';

                        $("#pnlOrthopedicChartDetail #list_Complaints").append(li);
                    });

                    Clinical_ProgressNote.params["IsBodyPart"] = true;
                }
                else {
                    $("#pnlOrthopedicChartDetail #list_Complaints").append("<li style='text-align:center' >No Complaint is added.</li>");
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
        if ($("#pnlOrthopedicChartDetail #list_Complaints li[bodypart='" + BodyPart + "']").length > 1)
            IsDeleteBodyPartAssociation = false;

        OrthopedicComplaints.DeleteBodyPartAndComplaint(NotesBodyPartId, ComplaintDetailId, Clinical_OrthopedicChartDetail.params["NotesId"], Clinical_OrthopedicChartDetail.params["PatientId"], IsDeleteBodyPartAssociation).done(function (res) {
            if (res == true) {
                //Remove Row
                $("#pnlOrthopedicChartDetail #list_Complaints").find("#notes_bodypart_complaint" + ComplaintDetailId).remove();
                if (IsDeleteBodyPartAssociation) {
                    OrthopedicChartSkeleton.CanvanId = "ChartCanvas";
                    OrthopedicChartSkeleton.ParentControl = "Clinical_OrthopedicChartDetail";
                    OrthopedicChartSkeleton.unSelectBodyPart(BodyPart);
                }
                if ($("#pnlOrthopedicChartDetail #list_Complaints li").length <= 0) {
                    $("#pnlOrthopedicChartDetail #list_Complaints").append("<li style='text-align:center' >No Complaint is added.</li>");
                    Clinical_ProgressNote.params["IsBodyPart"] = false;
                }
            }
        });

    },

    LoadBodyPartsByNotes_DBCall: function () {

        var objData = {};
        objData["NotesId"] = Clinical_OrthopedicChartDetail.params["NotesId"];
        objData["commandType"] = "load_bodyparts_by_notes";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "Orthopedic", "Orthopedic");

    },

    UnLoad: function () {

        UnloadActionPan(Clinical_OrthopedicChartDetail.params.ParentCtrl, 'Clinical_OrthopedicChartDetail');

    },

};