Bunch_AppointmentStatus = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Bunch_AppointmentStatus.bIsFirstLoad) {
            Bunch_AppointmentStatus.bIsFirstLoad = false;

        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}