
var MDVisionService = {
    reloadLookups: false,
    sess_LastActivity: new Date(),
    lookups: function (method, reload, data) {
        var dynamicMethods = [];
        var staticMethods = [];

        if ($.isArray(method)) {

            $.each(method, function (i, item) {
                if ($.inArray(item, Object.keys(StaticLookups)) < 0) {
                    dynamicMethods.push(item);
                }
                else {
                    staticMethods.push(item);
                }
            });
        } else {

            if ($.inArray(method, Object.keys(StaticLookups) < 0)) {
                dynamicMethods = method;
            }
            else {
                staticMethods = method;
            }
        }

        //if (!reload) {
        //} else {

        //}
        return $.when(MDVisionService.staticLookupRequest(staticMethods, reload, data), MDVisionService.ajaxLookupRequest(dynamicMethods, reload, data))
           .then(function (result, result1) {
               if (result == null)
                   result = new Object();
               $.extend(result, result1[0]);
               return result;
           }).promise();
    },
    staticLookupRequest: function (method, reload, data) {
        RefreshWindowOnEntityChange();
        var dfd = new $.Deferred();
        var responseObject = {};
        if (method.length > 0) {
            if ($.isArray(method)) {
                $.each(method, function (i, item) {
                    if ($.inArray(item, Object.keys(StaticLookups)) >= 0) {

                        responseObject[item] = StaticLookups[item];

                    }
                });

                dfd.resolve(responseObject);
                return dfd.promise();
            }
            else {

                if ($.inArray(method, Object.keys(StaticLookups)) >= 0) {
                    dfd.resolve(results);
                    return dfd.promise();

                }
            }
        }
    },

    cacheLookupRequest: function (method, reload, data) {
        var results = store.fetch(method);
        //overwrite reload when global reloadlookups is true
        if (MDVisionService.reloadLookups == true)
            reload = true;
        if (results && (!reload)) {
            var dfd = new $.Deferred();
            dfd.resolve(results);
            return dfd.promise();
        }
    },

    ajaxLookupRequest: function (method, reload, data) {
        RefreshWindowOnEntityChange();
        var dfd = new $.Deferred();
        if (method.length > 0) {
            return $.ajax({
                type: 'GET',
                url: "common/MDVisionLookups.ashx?method=" + method,
                data: data,
                dataType: "json",
                cache: false,
                beforeSend: function () {

                    BackgroundLoaderShow(true);
                },
                success: function (result) {
                    // if (result) {
                    //for (var name in result) {
                    //    method = name;
                    //    if (method != 'GetGuarantor') {
                    //        //fix caching
                    //        //  store.save(method, result);
                    //    }
                    //}

                    //  dfd.resolve(result);
                    // return dfd.promise();

                    // }
                    sess_Reset(globalAppdata["SessionTimout"]);
                    BackgroundLoaderShow(false);
                },

            }).promise();

        } else {

            dfd.resolve("");
            return dfd.promise();

        }

    },


    defaultService: function (data, controlName, cammandAction) {
        RefreshWindowOnEntityChange();
        if (!cammandAction) {
            cammandAction = "";
        }
        return $.ajax({
            type: "POST",
            url: "Common/MDVisionHandler.ashx?controlName=" + controlName + "&cammandAction=" + cammandAction,
            data: data,
            //dataType: "json",
            cache: false,
            rp: 10,
            beforeSend: function () {

                //if (IsBackgroundLoaderShow == true)
                //BackgroundLoaderShow(true);
            },
            success: function (result) {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                //$('#ajaxactionloader').hide();
                //$('#CopyrightText').show();
                //debugger;
                //if (jqXHR.status == 0) {
                //    utility.DisplayMessage({
                //        'message': 'No internet connection available, Please check your network',
                //        'errorType': "2"
                //    });
                //} else if (jqXHR.status == 404) {
                //    utility.DisplayMessage({
                //        'message': 'Server is not responding, please try again later.',
                //        'errorType': "2"
                //    });
                //} else {
                //    utility.DisplayMessage({
                //        'message': jqXHR.responseText,
                //        'errorType': "2"
                //    });
                //}
            }
            //contentType: "application/json; charset=utf-8",
        }).then(function (result) {

            try {

                presult = jQuery.parseJSON(result);
                if (cammandAction.toLowerCase() != "refresh_count")
                    sess_Reset(globalAppdata["SessionTimout"]);

                BackgroundLoaderShow(false);
                if (presult.Redirect == "Redirect") {
                    if (MDVisionService.IsToRedirect())
                        window.location.href = presult.Url;
                }
                else if (presult.Redirect != undefined && presult.Redirect != "undefined") {
                    sessLogOut();

                    //$.resolve();
                    //return presult;


                    ////  ParentCntrlID = params.PanelID;
                    //  params = [];
                    //  //params['ParentCntrlID'] = ParentCntrlID;
                    //  params['UserName'] = globalAppdata['AppUserName']

                    //  LoadActionPan('UserReLogin', params,'ReLogin');//, GetCurrentSelectedTab().PanelID);
                    //     window.location.href = presult.Url;
                }
                return presult;

            } catch (ex) {
                console.log(ex);
            }

        });

    },

    IsToRedirect: function () {

        if (localStorage.length <= 0)
            return false;

        if (localStorage.getItem("IsRedirectToLogin") && Boolean(localStorage.getItem("IsRedirectToLogin")) == true) {
            return false;
        }
        else {
            localStorage.setItem("IsRedirectToLogin", true);
            return true;
        }
    },

    fileService: function (data, controlName, cammandAction) {
        if (!cammandAction) {
            cammandAction = "";
        }
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "Common/MDVisionHandler.ashx?controlName=" + controlName + "&cammandAction=" + cammandAction,
            data: data,
            //dataType: "json",
            cache: false,
            processData: false,
            contentType: false,
            error: function (msg) {
                if (msg && msg.responseText) {
                    if (msg.responseText.indexOf("HTTP Error 404.13 - Not Found") > -1) {
                        utility.DisplayMessages("File size exceeds the maximum length", 4);
                    }
                }
            }
        }).then(function (result) {

            try {

                result = jQuery.parseJSON(result);
                sess_Reset(globalAppdata["SessionTimout"]);

                BackgroundLoaderShow(false);
                if (result.Redirect == "Redirect") {
                    if (MDVisionService.IsToRedirect())
                        window.location.href = result.Url;
                }
                else if (result.Redirect != undefined && result.Redirect != "undefined") {
                    sessLogOut();
                }

            } catch (ex) {
                console.log(ex);
            }

            return result;
        });
    },
    defaultService1: function (data, controlName, cammandAction) {
        if (!cammandAction) {
            cammandAction = "";
        }
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "Common/MDVisionHandler.ashx?controlName=" + controlName + "&cammandAction=" + cammandAction,
            data: data,
            dataType: "json",
            cache: false,
            rp: 10,
            success: function (data, textStatus, xmlHttpRequest) {

                sess_Reset(globalAppdata["SessionTimout"]);
                var jData = $(data);
                var totalRecords = data.iTotalDisplayRecords,
                recordsFiltered = data.iTotalDisplayRecords,
                json = '{"draw":"1", "recordsTotal":"' + totalRecords + '", "recordsFiltered":"' + totalRecords + '", "jsondata":' + JSON.stringify(eval("(" + data.PatientLoad_JSON + ")")) + '}';
                var myjson = JSON.stringify(eval("(" + json + ")"));
                return myjson;

                //return JSON.parse(json);
                //var json = { "status": data.status, "PatientCount": data.PatientCount, "iTotalDisplayRecords": data.iTotalDisplayRecords, "PatientLoad_JSON": data.PatientLoad_JSON, "aaData": [] };
                //json.PatientLoad_JSON = data.PatientLoad_JSON
                //var PatientLoadJSONData = JSON.parse(data.PatientLoad_JSON);
                //var RecordCount = "";
                //$.each(PatientLoadJSONData, function (i, item) {
                //    RecordCount = item.RecordCount;
                //});
                //json.iTotalRecords = 100;//RecordCount;//jData.find("[nodeName='opensearch:PatientCount']").text();
                //json.iTotalDisplayRecords = json.PatientCount;//jData.find("[nodeName='opensearch:iTotalDisplayRecords']").text();
                //var jsonData = json;

                //var items = jData.find("entry").each(function(){
                //    json.aaData.push([
                //        $(this).find("icon").text(),
                //        $(this).find("link").attr("href"),
                //        $(this).find("title").text(),
                //        $(this).find("author").find("name").text(),
                //    ]);
                //});

                //fnCallback(json);
            }
            ////beforeSend: function () {
            ////    $('#CopyrightText').hide();
            ////    $('#ajaxactionloader').show();
            ////},
            //success: function () {
            //    $('#ajaxactionloader').hide();
            //    $('#CopyrightText').show();
            //},
            ////error: function (jqXHR, textStatus, errorThrown) {
            ////    $('#ajaxactionloader').hide();
            ////    $('#CopyrightText').show();
            ////    if (jqXHR.status == 0) {
            ////        utility.DisplayMessage({
            ////            'message': 'No internet connection available, Please check your network',
            ////            'errorType': "2"
            ////        });
            ////    } else if (jqXHR.status == 404) {
            ////        utility.DisplayMessage({
            ////            'message': 'Server is not responding, please try again later.',
            ////            'errorType': "2"
            ////        });
            ////    } else {
            ////        utility.DisplayMessage({
            ////            'message': jqXHR.responseText,
            ////            'errorType': "2"
            ////        });
            ////    }
            ////}
            //contentType: "application/json; charset=utf-8",
        }).promise();

    },

    APIService: function (inputData, ModuleName, SubModule) {
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "api/" + ModuleName + "/" + SubModule,
            data: { data: inputData },
            //contentType: "application/json",
            dataType: "json",
            cache: false,
            rp: 10,
            beforeSend: function () {

                BackgroundLoaderShow(true);
            },
            success: function (result) {
                sess_Reset(globalAppdata["SessionTimout"]);

                BackgroundLoaderShow(false);
                presult = JSON.parse(result);

                if (presult.Redirect == "Redirect") {
                    if (MDVisionService.IsToRedirect())
                        window.location.href = presult.Url;
                }
                else if (presult.Redirect != undefined && presult.Redirect != "undefined") {
                    sessLogOut();
                }

                //if (presult.Redirect) {
                //    if (presult.Sender && presult.Sender == "ClinicalNotes") {
                //        //var basePath = window.location.pathname.replace('MDVisionDefault.aspx', '');
                //        window.location.href =  presult.Redirect;
                //    }
                //}
            },
        }).promise();

    },

    APIServiceSync: function (inputData, ModuleName, SubModule) {
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "api/" + ModuleName + "/" + SubModule,
            data: { data: inputData },
            //contentType: "application/json",
            dataType: "json",
            async: false,
            cache: false,
            rp: 10,
            beforeSend: function () {

                BackgroundLoaderShow(true);
            },
            success: function (result) {
                sess_Reset(globalAppdata["SessionTimout"]);

                BackgroundLoaderShow(false);
                presult = JSON.parse(result);

                if (presult.Redirect == "Redirect") {
                    if (MDVisionService.IsToRedirect())
                        window.location.href = presult.Url;
                }
                else if (presult.Redirect != undefined && presult.Redirect != "undefined") {
                    sessLogOut();
                }



            },
        }).promise();

    },
    APIServiceSyncCall: function (inputData, ModuleName, SubModule) {
        RefreshWindowOnEntityChange();
        var response = null;
        $.ajax({
            type: "POST",
            url: "api/" + ModuleName + "/" + SubModule,
            data: { data: inputData },
            //contentType: "application/json",
            dataType: "json",
            async: false,
            cache: false,
            rp: 10,
            beforeSend: function () {

                BackgroundLoaderShow(true);
            },
            success: function (result) {
                sess_Reset(globalAppdata["SessionTimout"]);

                BackgroundLoaderShow(false);
                presult = JSON.parse(result);

                if (presult.Redirect == "Redirect") {
                    if (MDVisionService.IsToRedirect())
                        window.location.href = presult.Url;
                }
                else if (presult.Redirect != undefined && presult.Redirect != "undefined") {
                    sessLogOut();
                }
                response = result;


            },
        })
        return response;

    },
    PMSAPIService: function (inputData, ModuleName, SubModule) {
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "api/" + ModuleName + "/" + SubModule,
            data: { data: inputData },
            //contentType: "application/json",
            dataType: "json",
            cache: false,
            rp: 10,
            beforeSend: function () {

                BackgroundLoaderShow(true);
            },
            success: function (result) {

            },
        }).then(function (result) {

            try {

                sess_Reset(globalAppdata["SessionTimout"]);
                BackgroundLoaderShow(false);

                result = jQuery.parseJSON(result);
                if (result.RedirectSet) {

                    var redirect = jQuery.parseJSON(result.RedirectSet);

                  MDVisionService.CheckForRedirection(redirect);
                }
                else if (result.Redirect)
                {
                    MDVisionService.CheckForRedirection(result);
                }

                if (result.ResponseModel)
                    return JSON.parse(result.ResponseModel);
                else
                    return result;

            } catch (ex) {
                console.log(ex);
            }

        }).promise();

    },

    CCMAPIService: function (inputData, ModuleName, SubModule) {
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "api/" + ModuleName + "/" + SubModule,
            data: { data: inputData },
            //contentType: "application/json",
            dataType: "json",
            cache: false,
            rp: 10,
            beforeSend: function () {

                BackgroundLoaderShow(true);
            },
            success: function (result) {

            },
        }).then(function (result) {

            try {
                sess_Reset(globalAppdata["SessionTimout"]);
                BackgroundLoaderShow(false);

                result = jQuery.parseJSON(result);
                if (result.RedirectSet) {

                    var redirect = jQuery.parseJSON(result.RedirectSet);

                    MDVisionService.CheckForRedirection(redirect);

                }
                else if (result.Redirect) {

                    MDVisionService.CheckForRedirection(result);
                }

                if (result.ResponseModel)
                    return JSON.parse(result.ResponseModel);
                else
                    return result;

            } catch (ex) {
                console.log(ex);
            }

        });

    },

    APIServiceComplex: function (inputData, ModuleName, SubModule) {
        RefreshWindowOnEntityChange();
        return $.ajax({
            type: "POST",
            url: "api/" + ModuleName + "/" + SubModule,
            data: $.postify(inputData),
            //contentType: "application/json",
            dataType: "json",
            cache: false,
            rp: 10,
            beforeSend: function () {

                BackgroundLoaderShow(true);
            },
            success: function (result) {
                sess_Reset(globalAppdata["SessionTimout"]);

                BackgroundLoaderShow(false);

                result = jQuery.parseJSON(result);
                if (result.RedirectSet) {

                    var redirect = jQuery.parseJSON(result.RedirectSet);

                    MDVisionService.CheckForRedirection(redirect);

                }
                else if (result.Redirect) {
                    
                    MDVisionService.CheckForRedirection(result);
                }

                return result;
            },
        }).promise();
    },

    CheckForRedirection: function (result) {

        if (result.Redirect == "Redirect") {
            if (MDVisionService.IsToRedirect())
                window.location.href = result.Url;
        }
        else if (result.Redirect != undefined && result.Redirect != "undefined") {
            sessLogOut();
        }
    },

    //PracticeService: function (data, controlName, cammandAction) {
    //    if (!cammandAction) {
    //        cammandAction = "";
    //    }
    //    return $.ajax({
    //        type: "POST",
    //        url: "MDVisionHandler.ashx?controlName=" + controlName + "&cammandAction=" + cammandAction,
    //        data: data,
    //        dataType: "json",
    //        cache: "false",
    //        success: function () {
    //            alert("success");
    //        },
    //        failure: function () {
    //            alert("failure");
    //        },
    //        error: function () {
    //            alert("error");
    //        },
    //        beforeSend: function () {
    //            //$('#CopyrightText').hide();
    //            //$('#ajaxactionloader').show();
    //        }
    //        //success: function () {
    //        //    //$('#ajaxactionloader').hide();
    //        //    //$('#CopyrightText').show();
    //        //},
    //        //error: function (jqXHR, textStatus, errorThrown) {
    //        //    //$('#ajaxactionloader').hide();
    //        //    //$('#CopyrightText').show();
    //        //    if (jqXHR.status == 0) {
    //        //        utility.DisplayMessage({
    //        //            'message': 'No internet connection available, Please check your network',
    //        //            'errorType': "2"
    //        //        });
    //        //    } else if (jqXHR.status == 404) {
    //        //        utility.DisplayMessage({
    //        //            'message': 'Server is not responding, please try again later.',
    //        //            'errorType': "2"
    //        //        });
    //        //    } else {
    //        //        utility.DisplayMessage({
    //        //            'message': jqXHR.responseText,
    //        //            'errorType': "2"
    //        //        });
    //        //    }
    //        //}
    //        //contentType: "application/json; charset=utf-8",
    //    }).promise();

    //}

    //  parameter , class name, command name of class
    ////defaultService: function (data, controlName, cammandAction) {
    ////    if (!cammandAction) {
    ////        cammandAction = "";
    ////    }
    ////    return $.ajax({
    ////        type: "POST",
    ////        url: "MDVisionHandler.ashx?controlName=" + controlName + "&cammandAction=" + cammandAction,
    ////        data: data,
    ////        dataType: "json",
    ////        cache: "false",
    ////        ////beforeSend: function () {
    ////        ////    $('#CopyrightText').hide();
    ////        ////    $('#ajaxactionloader').show();
    ////        ////},
    ////        ////success: function () {
    ////        ////    $('#ajaxactionloader').hide();
    ////        ////    $('#CopyrightText').show();
    ////        ////},
    ////        ////error: function (jqXHR, textStatus, errorThrown) {
    ////        ////    $('#ajaxactionloader').hide();
    ////        ////    $('#CopyrightText').show();
    ////        ////    if (jqXHR.status == 0) {
    ////        ////        utility.DisplayMessage({
    ////        ////            'message': 'No internet connection available, Please check your network',
    ////        ////            'errorType': "2"
    ////        ////        });
    ////        ////    } else if (jqXHR.status == 404) {
    ////        ////        utility.DisplayMessage({
    ////        ////            'message': 'Server is not responding, please try again later.',
    ////        ////            'errorType': "2"
    ////        ////        });
    ////        ////    } else {
    ////        ////        utility.DisplayMessage({
    ////        ////            'message': jqXHR.responseText,
    ////        ////            'errorType': "2"
    ////        ////        });
    ////        ////    }
    ////        ////}
    ////        //contentType: "application/json; charset=utf-8",
    ////    }).promise();

    ////},

    //  parameter , class name, command name of class


};




var sess_intervalID = 0;
var sess_lastActivity;


function logOutUser() {
    //works for chrome, IE
    return MDVisionService.defaultService(null, "USER_LOGOUT", null);
}

function initSession() {
    //sess_lastActivity = new Date();
    sessSetInterval();
    //$(document).bind('keypress.session', function (ed, e) {
    //    sessKeyPressed(ed, e);
    //});
}
function sessSetInterval() {
    //How frequently to check for session expiration in milliseconds
    var sess_pollInterval = 10000;
    if (sess_intervalID == 0)
         sess_intervalID = setInterval('sessInterval()', sess_pollInterval);
}
function sessClearInterval() {
    clearInterval(sess_intervalID);
    sess_intervalID = 0;

}
function sess_Reset(TimeOUt) {
    sess_lastActivity = new Date();
}

document.onkeypress = sess_Reset;
document.onmousedown = sess_Reset;

function BindForIframeActivity() {

    $.each($("iframe,object"), function () {

        var $content = $(this).contents();
        if ($content.length > 0)
        {
            if ($._data($content.get(0), "events")
                && $._data($content.get(0), "events").keyup
                && $._data($content.get(0), "events").keyup.length > 0
                && $._data($content.get(0), "events").mousedown
                && $._data($content.get(0), "events").mousedown.length > 0)
                ;
            else {
                $($content).on("keyup", function () {
                    sess_Reset();
                });
                $($content).on("mousedown", function () {
                    sess_Reset();
                });
            }
        }
    });
   
}

function sessLogOut() {
    //  ParentCntrlID = params.PanelID;
    if (!$('#ReLogin').hasClass('modal')) {
        var parameters = [];
        //params['ParentCntrlID'] = ParentCntrlID;
        parameters['UserName'] = globalAppdata['AppUserName']
        $('#ReLogin').addClass('modal fade')

        LoadActionPan('UserReLogin', parameters, 'ReLogin', true);//, GetCurrentSelectedTab().PanelID);
    }
}
function sessInterval() {

    // if there is any iframe loaded then add event to check user activity.
    BindForIframeActivity();

    //How many minutes before the warning prompt
    var sess_warningMinutes = 0.70;
    //How many minutes the session is valid for
    var sess_expirationMinutes = parseInt(globalAppdata["SessionTimout"]);

    var now = new Date();
    //get milliseconds of differneces
    var diff = now - sess_lastActivity;
    //get minutes between differences
    var diffMins = (diff / 1000 / 60);
    if (diffMins >= (sess_expirationMinutes - sess_warningMinutes)) {
        //warn before expiring
        //stop the timer
        sessClearInterval();
        //prompt for Re Login
        sessLogOut();
    } else {
        //  initSession();
        //    sessSetInterval();
        // sess_lastActivity = new Date();
    }
}

$.postify = function (value) {
    var result = {};

    var buildResult = function (object, prefix) {
        for (var key in object) {

            var postKey = isFinite(key)
                ? (prefix != "" ? prefix : "") + "[" + key + "]"
                : (prefix != "" ? prefix + "." : "") + key;

            switch (typeof (object[key])) {
                case "number": case "string": case "boolean":
                    result[postKey] = object[key];
                    break;

                case "object":
                    if (object[key].toUTCString)
                        result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
                    else {
                        buildResult(object[key], postKey != "" ? postKey : key);
                    }
            }
        }
    };

    buildResult(value, "");

    return result;
};