DashBoardChangePwd = {
    params: [],
    bIsFirstLoad: true,
    regex: '',
    Admin: false,
    Load: function (params) {
        DashBoardChangePwd.params = params;
        if (DashBoardChangePwd.bIsFirstLoad) {
            DashBoardChangePwd.bIsFirstLoad = false;
        }
        if (DashBoardChangePwd.params.ParentCtrl != "userDetail") {
            DashBoardChangePwd.params.UserID = globalAppdata["AppUserId"];
        }
        DashBoardChangePwd.ValidateChangePassword();
        DashBoardChangePwd.BindRegex();
        DashBoardChangePwd.AdminValidation();

        $("#pnldashboardchangepass .password").keypress(function (event) {
            var ew = event.which;
            if (ew == 38)
                return false;
        });
    },




    AdminValidation: function () {
        if (globalAppdata["IsAdmin"] == 'True' && DashBoardChangePwd.params.ParentCtrl == "userDetail") {
            $('#frmdashboardchangepass #txtoldpwd').prop("disabled", true);
            $('#frmdashboardchangepass #txtnewpwd').focus();
            $('#frmdashboardchangepass #oldpswrd').css("display", "none");
            $('#frmdashboardchangepass').data('bootstrapValidator').enableFieldValidators('oldpwd', false);
            Admin = true;
        }
        else {
            $('#frmdashboardchangepass').data('bootstrapValidator').enableFieldValidators('oldpwd', true);
            Admin = false;
        }
    },
    ValidateChangePassword: function () {
        $('#frmdashboardchangepass')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   oldpwd: {
                       group: '.col-xs-12',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //newpwd: {
                   //    group: '.col-xs-12',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        },
                   //        regexp: {
                   //            regexp: globalAppdata.AppUserId, /* /((?=.*\d)(?=.*[a-zA-Z])(?=.*[\_\W]).{8,50})/*/
                   //            message: 'The minimum 8 character password should have alphabets, digits & Special Characters'
                   //        }
                   //    }
                   //},
                   conpwd: {
                       group: '.col-xs-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           identical: {
                               field: 'newpwd',
                               message: 'The password and its confirm are not the same'
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();

            DashBoardChangePwd.SaveChangePassword();
        });

        /// /^(?=(.*\d){1})(?=(.*[a-z]){1})(?=(.*[A-Z]){1})(?=(.*\W){1}).{1,}$/

    },
    BindRegex: function () {

        $('#frmdashboardchangepass').bootstrapValidator('removeField', 'newpwd');
        $('#frmdashboardchangepass').bootstrapValidator('addField', 'newpwd', {
            group: '.col-xs-12',
            validators: {
                notEmpty: {
                    message: ''
                },
                regexp: {
                    regexp: globalAppdata.PasswordRegex, /*/((?=.*\d)(?=.*[a-zA-Z])(?=.*[\_\W]).{8,50})/,*/
                    message: 'The Password does not meet the Criteria set for User\'s Default Entity'
                }
            }
        });

    },




    EnableValidation: function () {
        if ($('#frmdashboardchangepass #txtoldpwd').val() != "") {
            $('#frmdashboardchangepass').data('bootstrapValidator').enableFieldValidators('oldpwd', false);
        }
        else {
            $('#frmdashboardchangepass').data('bootstrapValidator').enableFieldValidators('oldpwd', true);
        }
    },

    SaveChangePassword: function () {

        var self = $("#pnldashboardchangepass #frmdashboardchangepass");
        var myJSON = self.getMyJSON();
        //alert('success!!');
        //return;
        DashBoardChangePwd.UpdateUserPassword(DashBoardChangePwd.params.UserID, myJSON, Admin).done(function (response) {
            if (response.status != false) {
                if (DashBoardChangePwd.params.ParentCtrl == "userDetail") {
                    userDetail.LoadUser();
                }
                utility.DisplayMessages(response.Message, 1);
                DashBoardChangePwd.UnLoad();
            }
            else {

                if (response.IsValidate == true) {
                    self.find('#txtoldpwd').parent().removeClass('has-success');
                    self.find('#txtoldpwd').parent().addClass('has-error');
                    self.find('#txtoldpwd').parent().find('i').removeClass('glyphicon glyphicon-ok');
                    self.find('#txtoldpwd').parent().find('i').addClass('glyphicon glyphicon-remove');
                    self.find('#txtoldpwd').parent().find('i').attr('style', '');
                }
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    UpdateUserPassword: function (UserID, UserData, IsAdmin) {
        var data = "UserID=" + UserID + "&UserData=" + UserData + "&IsAdmin=" + IsAdmin;
        return MDVisionService.defaultService(data, "DASHBOARDSETTING", "UPDATE_USER_PASSWORD");
    },

    UnLoad: function () {
        if (DashBoardChangePwd.params != null && DashBoardChangePwd.params.ParentCtrl != null) {
            UnloadActionPan(DashBoardChangePwd.params.ParentCtrl, "DashBoardChangePwd");
        }
        else
            UnloadActionPan();
    },
}
