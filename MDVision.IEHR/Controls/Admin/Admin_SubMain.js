
Admin_SubMain = {
bIsFirstLoad: true,
Load: function (params) {
    if (Admin_SubMain.bIsFirstLoad) {
             var self = $("#mstrDivAdmin");
             Admin_SubMain.bIsFirstLoad = false;
         }
     }
}