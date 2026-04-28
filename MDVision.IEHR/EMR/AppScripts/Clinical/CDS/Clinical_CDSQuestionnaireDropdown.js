ClinicalCDSQuestionnaireDropdown = {

    params: [],
    bIsFirstLoad: true,
    Load: function (params) {


        ClinicalCDSQuestionnaireDropdown.params = params;

        ClinicalCDSQuestionnaireDropdown.bindulQuestionnaireDropdownLi();

    },


    bindulQuestionnaireDropdownLi: function () {

        if (ClinicalCDSDetail.QuestionnaireDropDownValues[ClinicalCDSQuestionnaireDropdown.params.CDSQuestionnaireId] == null) {

            ClinicalCDSDetail.QuestionnaireDropDownValues[ClinicalCDSQuestionnaireDropdown.params.CDSQuestionnaireId] = "";
        }
        var t = ClinicalCDSDetail.QuestionnaireDropDownValues[ClinicalCDSQuestionnaireDropdown.params.CDSQuestionnaireId].split(",");

        $.each(t, function (i, item) {

            ClinicalCDSQuestionnaireDropdown.buildulQuestionnaireDropdownLi(item);

        });
    },


    buildulQuestionnaireDropdownLi: function (dropDownValue) {

        var textControl = $("#ClinicalCDSQuestionnaireDropdown #txtQuestionnaireDropdownValue");
        if (!dropDownValue) {

            dropDownValue = textControl.val();
        }

        if (dropDownValue == "") {

            return;
        }

        //  var currId = "";
        //$("#ClinicalCDSQuestionnaireDropdown ul#ulQuestionnaireDropdown li[id*='-']").each(function (i, item) {
        //    currId = $(this).attr("id");
        //});
        //currId = parseInt(currId) + (-1);
        var li = '';

        li = "<li>"
            + "<div class='col-sm-12 col-lg-12'>"
            + "<span class='display ' onclick='ClinicalCDSQuestionnaireDropdown.makeEditable(this);'>"
            + dropDownValue
            + "</span>"
            + "<input class='edit form-control size95per' type='text' onblur='ClinicalCDSQuestionnaireDropdown.doneEditing(this);' style='display:none'/>"
            + "<a href='#' onclick='ClinicalCDSQuestionnaireDropdown.deleteDropDownValue(this);' class='removeIconListHover'><i class='fa fa-times'></i></a>"
            + "</div><div class='clearfix'></div></li>";

        var IsAlreadyExist = false;
        $('#ClinicalCDSQuestionnaireDropdown #ulQuestionnaireDropdown li').each(function () {
            if ($(this).text() == dropDownValue) {

                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#ClinicalCDSQuestionnaireDropdown #ulQuestionnaireDropdown').append(li);


            textControl.val('');

        }
        else {
            utility.DisplayMessages(dropDownValue + ' already added before', 2);

            textControl.val('');
        }
    },

    deleteDropDownValue: function (obj) {
        $(obj).closest("li").remove()
    },

    makeEditable: function (obj) {
        $(obj).hide().siblings(".edit").show().val($(obj).text()).focus();
    },

    doneEditing: function (obj) {

        $(obj).hide().siblings(".display").show().text($(obj).val());
    },
    UnLoad: function () {

        var dropDownValues = [];
        $('#ClinicalCDSQuestionnaireDropdown #ulQuestionnaireDropdown li').each(function () {
            dropDownValues.push($(this).text())
        });
        ClinicalCDSDetail.QuestionnaireDropDownValues[ClinicalCDSQuestionnaireDropdown.params.CDSQuestionnaireId] = dropDownValues.toString();
        UnloadActionPan(ClinicalCDSQuestionnaireDropdown.params["ParentCtrl"], "ClinicalCDSQuestionnaireDropdown");
    },
}