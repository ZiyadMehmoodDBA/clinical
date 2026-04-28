ENMCodeSuggest = {
    params: 0,
    Load: function (params) {
        ENMCodeSuggest.params = params;
        var obj = ENMCodeSuggest.params.Suggestions;
        $('#EMCode').text('Current Recommended E&M Code: ' + obj.SuggestedCode);
        if (obj.NextSuggestions != undefined) {
            var suggestions = obj.NextSuggestions.split(';');
            var rows = '';
            $.each(suggestions, function (i, item) {
                if (item != "") {
                    rows += '<li> ' + item + '</li>';
                }
            });
            $('#suggestions').append(rows);
        }
        else {
            utility.DisplayMessages("Please select Type", 2);
            ENMCodeSuggest.UnLoad();
        }

    },
    UnLoad: function () {
        if (ENMCodeSuggest.params.ParentCtrl != null || ENMCodeSuggest.params.ParentCtrl != undefined) {
            UnloadActionPan(ENMCodeSuggest.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },

    UnLoadToMain: function () {
        if (ENMCodeSuggest.params.ParentCtrl != null || ENMCodeSuggest.params.ParentCtrl != undefined) {
            UnloadActionPan(ENMCodeSuggest.params.ParentCtrl);
            BillingInformation.UnLoad();
        }
        setTimeout(function () {
            if ($(document.body).hasClass('modal-open')) {
                $(document.body).removeClass('modal-open');
            }
        }, 501);
    }
}