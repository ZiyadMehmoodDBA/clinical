AboutScreen = {
    params: [],
    Load: function (params) {

        var d = new Date();
        var yearPart = d.getFullYear();

        AboutScreen.params = params;

        $('#pnlAboutScreen #spnVersion').text("5.1");
        $('#pnlAboutScreen #spnCopyRightYear').text(' '+yearPart+' ');
    },
    UnLoad: function () {
        $('#containerAbout').modal('hide');
        $('#containerAbout').removeClass('modal fade')
        $('#containerAbout').empty();
    }
}
