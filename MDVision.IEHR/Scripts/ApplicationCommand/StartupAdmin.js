$(document).idleTimer((parseInt(globalAppdata["SessionTimout"]) * 1000 * 60) - (30 * 1000));
$(document).on("idle.idleTimer", function (event, elem, obj) {
    // function you want to fire when the user goes idle
   var count = parseInt(localStorage.getItem("tabsCount"));
    count--;
    localStorage.setItem("tabsCount", count );

    if (count <= 0) {
        if (!$('#ReLogin').hasClass('modal')) {
            var parameters = [];
            //params['ParentCntrlID'] = ParentCntrlID;
            parameters['UserName'] = globalAppdata['AppUserName']
            $('#ReLogin').addClass('modal fade')
            LoadActionPan('UserReLogin', parameters, 'ReLogin', true);//, GetCurrentSelectedTab().PanelID);
            ////idle Users, auto save Before sign out of the user, Dr Hijjar Requirement, Implemeneted By Azhar Shahzad on July 14, 2016
            if (params.NotesId != null && params.NotesId != "" && Number(params.NotesId) > 0 && $('#pnlClinicalProgressNote').is(':visible') && Number(globalAppdata["SessionTimout"]) <= 5) {

            }
        }
    }  
    
});

$(window).unload(function () {
    //do something
   var count = parseInt(localStorage.getItem("tabsCount"));
   localStorage.setItem("tabsCount", count - 1);
});

$(document).ready(function () {
    addActiveTab();
});
 
function addActiveTab()
{
    var count = 0;
    if (localStorage.getItem("tabsCount") != null)
        count = parseInt(localStorage.getItem("tabsCount"));
    count < 0 ? count = 0 : count;
    localStorage.setItem("tabsCount", count + 1);
}

$(document).on("active.idleTimer", function (event, elem, obj, triggerevent) {
    // function you want to fire when the user becomes active again
      addActiveTab();
});

if (localStorage.getItem('VersionNo') == null || localStorage.getItem('VersionNo') != globalAppdata['VersionNo'] || localStorage.getItem('PatchNo') == null || localStorage.getItem('PatchNo') != globalAppdata['PatchNo']) {
    store.clearAll();
    store.clearAllSession();
    localStorage.clear();
    localStorage.setItem('VersionNo', globalAppdata['VersionNo']);
    localStorage.setItem('PatchNo', globalAppdata['PatchNo']);
    html = null;
    location.reload(true);
}

LoadApplication();

if (globalAppdata['DateFormat'])
    date_format = globalAppdata['DateFormat'];

////setInterval(Patient_Message.RefreshCount, globalAppdata['RefreshTime']);
//showing date timer
setInterval(function () { globallyDateTime() }, 1000);
//setInterval(Patient_Message.RefreshCount, 2000);
$(document).on("keydown", function (e) {
    if (e.which === 8 && !$(e.target).is("input:not([readonly]):not([type=radio]):not([type=checkbox]), textarea, [contentEditable], [contentEditable=true]")) {
        e.preventDefault();
    }
});
$(document).mousedown(function (e) {
    switch (e.which) {

        case 2:
            e.preventDefault();
            break;

    }
    return true;
});

//$(window).on('beforeunload', function () {
//    return 'Are you sure you want to leave?';
//});

//$(window).on('unload', function () {
//    logout();
//});
/*Doc -33 Emergency work flow
     4.1.2	Login with emergency access user in application
    1.	A user having emergency access login in mdvision application.
    2.	The icon with username will be displayed in Red indicating the user will perform actions having emergency access role. This can been seen in given screen.
    3.	The Red username icon will be displayed in blinking form and upon hover the text “Emergency Access User” will be displayed.
    /removing blinking after 5 mins of user login
*/
setTimeout(function () {
    $('#mainForm #userbox #emergencyUserIconSpan').removeClass('animated infinite flash');
}, 60000 * 5);


