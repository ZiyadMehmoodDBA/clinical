Clinical_OrthoProblemList = {

    params: [],
    bIsFirstLoad: true,

    Load: function (params) {

        Clinical_OrthoProblemList.params = params;

        if (Clinical_OrthoProblemList.bIsFirstLoad == true)
        {
            Clinical_OrthoProblemList.bIsFirstLoad = false;
            Clinical_OrthoProblemList.CreateBodProblemTabs();
            Clinical_OrthoProblemList.LoadOrthoProblemSearch();
        }
    },

    CreateBodProblemTabs: function () {
        var $ul = "ulBodyPartsProblemTabs";
        var $div = "divOrthoProblemTabsBody";
        $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul).empty();
        $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $div).empty();
        $.each(Clinical_OrthoProblemList.GetSelectedBodyPartArray(Clinical_OrthopedicChartDetail.BodyParts), function (i, item) {
            var rendomKey = utility.makeRendomKey();
            var classActive4FirstRecord = 'active';
            if (i != 0)
                classActive4FirstRecord = '';
            var bodyPartNameWithoutSpace = item.BodyPart.replace(/\s/g, '');
            var onClickMethod = "Clinical_OrthoProblemList.LoadFavoriteListProblem(" + item.BodyPartId + ")";
            var $li = '<li class="' + bodyPartNameWithoutSpace + "_" + item.BodyPartId + " " + classActive4FirstRecord + ' custom-made-tabs" id="' + rendomKey + '" onclick="' + onClickMethod + '"> <a href=".' + bodyPartNameWithoutSpace + '" data-toggle="tab">' + item.BodyPart + '<span id="draftNotesCount" class="badge"></span></a></li>';
            var $divBody = '<div class="' + bodyPartNameWithoutSpace + " " + classActive4FirstRecord + ' tab-pane" id="' + bodyPartNameWithoutSpace + "_" + item.BodyPartId + '">' +
                '<div class="panel panel-featured"><header class="panel-heading"><h2 class="panel-title" id="divOrthoProblemFavlistName">' + item.BodyPart + '</h2></header><div class="panel-body p-none"><table class="table table-bordered table-striped table-hover mb-none table-condensed" id="dgvOrthoProblemFavICDList"><tbody></tbody></table></div></div>' +

                '</div>';
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul).append($li);
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $div).append($divBody);
        });
        $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul + " li:first").trigger('click');
    },

    GetSelectedBodyPartArray: function (BodyPartsArray) {
        return $.grep(BodyPartsArray, function (item) {
            return item.IsSelected == true;
        });
    },

    LoadFavoriteListProblem: function (BodyPartId) {
        var FavListType = "Problems"; var $ul = 'ulFavoritiesListProblems';
        $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul).empty();
        Clinical_OrthoProblemList.LoadFavoriteList_DbCall(FavListType, BodyPartId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.each(response.OrthopedicFavList_JSON, function (i, item) {
                    var onClickMethod = Clinical_OrthoProblemList.GetFavoritiesOnClickMethod(FavListType, item);
                    var classActiveRecord = 'active';
                    if (i != 0)
                        classActiveRecord = '';
                    var li = '<li class="' + classActiveRecord + '" id="ortho_FavList' + item.FavoriteListId + '" onclick="' + onClickMethod + '"> ' + item.Name + '</li>';
                    $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul).append(li);
                });
                $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul + " li:first").trigger('click');
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },

    GetFavoritiesOnClickMethod: function (FavListType, item) {
        switch (FavListType) {
            case 'Problems':
                return "Clinical_OrthoProblemList.AddProblemToTabs(this," + item.FavoriteListId + ",'" + item.BodyPart.replace(/\s/g, '') + "_" + item.BodyPartId + "')";
                break;
            default:
                return "";
        }
    },

    AddProblemToTabs: function (element, FavoriteListId, divBodyPartsICDList) {
        Clinical_OrthoProblemList.GetFavoriteListICDsByFavId_DbCall(FavoriteListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ul = 'ulFavoritiesListProblems'
                $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + divBodyPartsICDList + " #dgvOrthoProblemFavICDList tbody").find("tr").remove();
                $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + $ul + " li").removeClass('active');
                $(element).addClass('active');
                if (response.FavoriteListICDCount > 0) {
                    var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
                    $.each(FavoriteListJSON, function (i, item) {
                        var $row = $('<tr/>');
                        $row.append('<td>' + item.ICD10Code + " - " + item.ICD10CodeDescription + '</td>');
                        $("#" + Clinical_OrthoProblemList.params.PanelID + " #" + divBodyPartsICDList + " #dgvOrthoProblemFavICDList tbody").append($row);
                    });
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadOrthoProblemSearch: function (ProblemListId, PageNo, rpp) {
        var strMessage = "";
        if ($("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result").css("display") == "none")
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result").show();
        Clinical_OrthoProblemList.SearchProblemList_DBCall(ProblemListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (Clinical_OrthoProblemList.params.ParentCtrl == "clinicalTabProgressNote") {
                    if ($("#" + Clinical_OrthoProblemList.params.PanelID + " #dgvProblemLists thead tr #SelectRecord").length == 0) {
                        $("#" + Clinical_OrthoProblemList.params.PanelID + " #dgvProblemLists thead tr").prepend(' <th id="SelectRecord" class="size10 center" style="padding-right: 6.9px !important;"  coltype="checkbox"> <input type="checkbox" id="chkHeaderProblemsList" onchange="Clinical_ProblemLists.checkUncheckAllProblemsList(this);"  class="input-block" coltype="checkbox"/> </th>');
                    }
                } else
                    $("#" + Clinical_OrthoProblemList.params.PanelID + " #dgvProblemLists th#SelectRecord").remove();
                Clinical_OrthoProblemList.ProblemListGridLoadNew(response);
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },

    ProblemListGridLoadNew: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists")) {
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnClearTable();
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").dataTable().fnDestroy();
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
        } else {
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists tbody").find("tr").remove();
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #pnlProblemLists_Result #dgvProblemLists").parent().parent().find('div.row').remove();
        }
        var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
        $.each(ProblemListLoadJSONData, function (i, item) {
            var $row = $('<tr/>');
            $row.attr("onclick", "utility.SelectGridRow($(this))");
            $row.attr("id", item.ProblemListId);
            $row.attr("ProblemListNotesId", item.NoteId);
            $row.attr("name", "Problems");
            var color = "";
            if (item.Severity == "Mild Intermittent" || item.Severity == "Mild Persistent")
                color = 'style = "color:green;font-weight:bold"'
            if (item.Severity == "Severe Persistent" || item.Severity == "Unspecified Severity")
                color = 'style = "color:red;font-weight:bold"'
            if (item.Severity == "Moderate Persistent")
                color = 'style = "color:orange;font-weight:bold"'
            var comments = "";
            var commentsMethod = "Clinical_ProblemLists.AddComments('" + item.ProblemListId + "');";
            comments = '<a href="#" onclick="' + commentsMethod + '" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '"><i class="fa fa-commenting blue"></i></a>';
            var ProblemListId = item.ProblemListId;
            var SelectionCheckBoxColumn = "";
            var Checked = "";
            if (item.IsNoteLinked == "True") {
                if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1)
                    Checked = " ";
                else
                    Checked = " checked";
            } else {
                if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProblemListId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1)
                    Checked = " checked";
                else
                    Checked = "";
            }
            SelectionCheckBoxColumn = '<td class="sorting_1 center"><input type="checkbox" onchange="Clinical_ProblemLists.enableAddProbList(this);" id="' + item.ProblemListId + '" name="SelectCheckBoxProbList" ' + Checked + ' class="input-block text-center"/></td>';
            $row.append(SelectionCheckBoxColumn + '<td style="display:none" id="' + item.ProblemListId + '" ></td><td Icd10=' + item.ICD10 + '>' + item.Description + '</td><td>' + "Neck" + '</td><td ' + item.ChronicityLevel + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.ModifiedOn + ' ' + item.ModifiedBy + '</td><td  id="hfComments' + item.ProblemListId + '"  style="display:none"><input type="hidden" name="hfComments" value="' + item.Comments + '"/></td>');
            $("#" + Clinical_OrthoProblemList.params.PanelID + " #dgvProblemLists tbody").last().append($row);
        });
    },

    SearchProblemList_DBCall: function (ProblemListId, PageNumber, RowsPerPage) {
        var objData = new Object();

        objData["PatientId"] = Clinical_OrthoProblemList.params.PatientId && parseInt(Clinical_OrthoProblemList.params.PatientId) > 0 ? Clinical_OrthoProblemList.params.PatientId : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = true;
        objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        objData["NoteId"] = Clinical_OrthoProblemList.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    GetFavoriteListICDsByFavId_DbCall: function (FavoriteListId) {
        var objData = {};
        objData["IsActive"] = true;
        objData["FavoriteListId"] = FavoriteListId;
        objData["FavoriteListICDId"] = 0;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = 1;
        objData["SearchData"] = '';
        objData["RowsPerPage"] = '2000';
        objData["commandType"] = "load_favoritelist_icd";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");

    },

    LoadFavoriteList_DbCall: function (FavListType, BodypartId) {
        var objData = {};
        objData["ListType"] = FavListType;
        objData["FavListBodyPartIds"] = BodypartId;
        objData["ProviderId"] = Clinical_OrthoProblemList.params.NotesProviderId;
        objData["commandType"] = "load_FavoriteList_Orthopedic";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Orthopedic", "OrthopedicFavoriteList");
    },

    UnLoad: function () {

    },

};