UserReLogin = {
    bIsFirstLoad: true,
    params: [],
    countDownTimer: null, // to clear time out function for timer
    Load: function (params) {
        // clearUserSession();
        UserReLogin.params = params;
        var self = $('#UserReLogin');
        $('#ReLogin').css('background-color', 'black');
        var timer = 30;
        UserReLogin.countDownTimer = setTimeout(function () {
            UserReLogin.countDown(timer)
        }, 1000);
    },

    // to Interact with server 
    //Author:Muhamamd Azhar Shahzad
    //Date: April 07,2016
    LogInUser_DbCall: function (UserInfo) {
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(null, "USER_RELOGIN", "USER_SESSION_RESET_RELOGIN");
    },

    // Count Down timer function
    //Author:Muhamamd Azhar Shahzad
    //Date: April 07,2016
    countDown: function (timer) {
        if (timer > 0 && timer < 10) {
            $('#pnlUserReLogin #digit').text("0" + timer);
        }
        else {
            $('#pnlUserReLogin #digit').text(timer);
        }
        timer--;
        if (timer >= 0) {
            UserReLogin.countDownTimer = setTimeout(function () {
                UserReLogin.countDown(timer)
            }, 1000);
        } else {
            UserReLogin.LogOutUser();
        }
    },

    //this function is used to log out the user
    //Author:Muhamamd Azhar Shahzad
    //Date: April 07,2016
    LogOutUser: function () {
        clearTimeout(UserReLogin.countDownTimer);
        $('#hdnIsSessionTimeout').val("1");
        $('#btnLogout').trigger('click');

    },

    //this function is used to User session reset and stayed user as logged in
    //Author:Muhamamd Azhar Shahzad
    //Date: April 07,2016
    LogInUser: function () {
        clearTimeout(UserReLogin.countDownTimer);
        UserReLogin.LogInUser_DbCall().done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);

                // you need to write code here to update time and date :)
                //start syed zia 08-02-2016, bug #PMS-3708
                setTimeout(function () {
                    var currentDate = moment().format("ddd, MMM DD, YYYY h:mm A");
                    $("#userCurrentTime").text(currentDate);
                }, 300);

                //end syed zia 08-02-2016, bug #PMS-3708
                // initSession();
                $('#ReLogin').modal('hide');
                $('#ReLogin').removeClass('modal fade')
                $('#ReLogin').empty();
                $(document).idleTimer("reset");
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                var timer = 30;
                UserReLogin.countDownTimer = setTimeout(function () {
                    UserReLogin.countDown(timer)
                }, 1000);
            }
        });
    },
}
