
findInDiv = {

    findwindow: document.createElement("div"),
    find_msg: null,
    find_window_background: "white",
    find_window_border: "blue",
    find_text_color: "black",
    find_title_color: "white",
    find_window_width: 245,
    find_window_height: 85,
    find_root_node: '#ProgressnoteHTML',
    find_parent_node: 'pnlClinicalProgressNote',

    drag: { mousex: 0, mousey: 0, tempx: '', tempy: '', isdrag: false, drag_obj: null, drag_obj_x: 0, drag_obj_y: 0 },

    find_timer: 0,


    highlights: [],


    find_pointer: -1,

    find_text: '',

    found_highlight_rule: 0,
    found_selected_rule: 0,


    Load: function () {

        document.onmousedown = findInDiv.MouseDown;
        document.onmousemove = findInDiv.MouseMove;
        document.onmouseup = findInDiv.MouseUp;
        document.ontouchstart = findInDiv.MouseDown;
        document.ontouchmove = findInDiv.MouseMove;
        document.ontouchend = findInDiv.MouseUp;
        findInDiv.create_div();
        findInDiv.find_msg = document.getElementById('find_msg');
    },
    highlight: function (word, node,txt) {
        if (!node)
            node = document.body;

        var re = new RegExp(word, "i");

        for (node = node.firstChild; node; node = node.nextSibling) {

            if (node.nodeType == 3) {
                var n = node;

                var match_pos = 0;

                {
                    match_pos = n.nodeValue.search(re);

                    if (match_pos > -1) {
                        var before = n.nodeValue.substr(0, match_pos);
                        var middle = n.nodeValue.substr(match_pos, word.length);

                        var after = document.createTextNode(n.nodeValue.substr(match_pos + word.length));
                        var highlight_span = document.createElement("span");
                        if (findInDiv.found_highlight_rule == 1)
                            highlight_span.className = "highlight";
                        else
                            highlight_span.style.backgroundColor = "yellow";

                        highlight_span.appendChild(document.createTextNode(middle));
                        n.nodeValue = before;
                        n.parentNode.insertBefore(after, n.nextSibling);
                        n.parentNode.insertBefore(highlight_span, n.nextSibling);
                        findInDiv.highlights.push(highlight_span);
                        highlight_span.id = "highlight_span" + findInDiv.highlights.length;
                        node = node.nextSibling;
                    }
                }
            }
            else {

                if (node.nodeType == 1 && node.nodeName.match(/textarea/i) && !findInDiv.getStyle(node, "display").match(/none/i))
                    findInDiv.textarea2pre(node);
                else {
                    if (node.nodeType == 1 && !findInDiv.getStyle(node, "visibility").match(/hidden/i))
                        if (node.nodeType == 1 && !findInDiv.getStyle(node, "display").match(/none/i))
                            findInDiv.highlight(word, node, txt);
                }
            }
        }

        $(txt).focus();

    },


    unhighlight: function () {
        for (var i = 0; i < findInDiv.highlights.length; i++) {

            var the_text_node = findInDiv.highlights[i].firstChild;

            var parent_node = findInDiv.highlights[i].parentNode;


            if (findInDiv.highlights[i].parentNode) {
                findInDiv.highlights[i].parentNode.replaceChild(the_text_node, findInDiv.highlights[i]);
                if (i == findInDiv.find_pointer) findInDiv.selectElementContents(the_text_node);
                parent_node.normalize();
                findInDiv.normalize(parent_node);
            }
        }

        findInDiv.highlights = [];
        findInDiv.find_pointer = -1;
    },


    normalize: function (node) {
        if (!node) { return; }
        if (node.nodeType == 3) {
            while (node.nextSibling && node.nextSibling.nodeType == 3) {
                node.nodeValue += node.nextSibling.nodeValue;
                node.parentNode.removeChild(node.nextSibling);
            }
        } else {
            findInDiv.normalize(node.firstChild);
        }
        findInDiv.normalize(node.nextSibling);
    },


    findit: function (isFromProgressNote,txt) {
        var string = document.getElementById('fwtext').value;
        if (isFromProgressNote) {
            string = $('#pnlClinicalProgressNote #fwtext').val();
        }

        findInDiv.findwindow.style.visibility = 'hidden';

        if (findInDiv.find_text.toLowerCase() == string.toLowerCase() &&
            findInDiv.find_pointer >= 0) {
            findInDiv.findnext(isFromProgressNote);
        }
        else {
            findInDiv.unhighlight();

            if (string == '') {
                findInDiv.find_msg.innerHTML = "";
                if (!isFromProgressNote)
                    findInDiv.findwindow.style.visibility = 'visible';
                else
                    findInDiv.findwindow.style.visibility = 'hidden';

                return;
            }

            findInDiv.find_text = string;


            if (findInDiv.find_root_node != null && findInDiv.find_parent_node != null) {
                var node = document.getElementById(findInDiv.find_parent_node).querySelector(findInDiv.find_root_node);
            } else if (findInDiv.find_root_node != null) {
                var node = document.getElementById(findInDiv.find_parent_node);
            }



            else
                var node = null;

            findInDiv.highlight(string, node, txt);

            if (findInDiv.highlights.length > 0) {
                findInDiv.find_pointer = -1;
                findInDiv.findnext(isFromProgressNote,txt);
            }
            else {
                findInDiv.find_msg.innerHTML = "&nbsp;<b>0 of 0</b>";
                findInDiv.find_pointer = -1;
            }
        }

        if (!isFromProgressNote)
            findInDiv.findwindow.style.visibility = 'visible';
        else
            findInDiv.findwindow.style.visibility = 'hidden';
        

    },


    findnext: function (isFromProgressNote, txt) {
        var current_find;

        if (findInDiv.find_pointer != -1) {
            current_find = findInDiv.highlights[findInDiv.find_pointer];


            if (findInDiv.found_highlight_rule == 1)
                current_find.className = "highlight";
            else
                current_find.style.backgroundColor = "yellow";
        }

        findInDiv.find_pointer++;

        if (findInDiv.find_pointer >= findInDiv.highlights.length)
            findInDiv.find_pointer = 0;

        var display_find = findInDiv.find_pointer + 1;

        findInDiv.find_msg.innerHTML = display_find + " of " + findInDiv.highlights.length;
        if (isFromProgressNote) {
            $('#pnlClinicalProgressNote #find_msg').html(findInDiv.find_msg.innerHTML);
        }
        current_find = findInDiv.highlights[findInDiv.find_pointer];


        if (findInDiv.found_selected_rule == 1)
            current_find.className = "find_selected";
        else
            current_find.style.backgroundColor = "orange";


        findInDiv.scrollToPosition(findInDiv.highlights[findInDiv.find_pointer]);

        $(txt).focus();

    },




    findprev: function (isFromProgressNote) {
        var current_find;

        if (findInDiv.highlights.length < 1) return;

        if (findInDiv.find_pointer != -1) {
            current_find = findInDiv.highlights[findInDiv.find_pointer];


            if (findInDiv.found_highlight_rule == 1)
                current_find.className = "highlight";
            else
                current_find.style.backgroundColor = "yellow";
        }

        findInDiv.find_pointer--;

        if (findInDiv.find_pointer < 0)
            findInDiv.find_pointer = findInDiv.highlights.length - 1;

        var display_find = findInDiv.find_pointer + 1;

        findInDiv.find_msg.innerHTML = display_find + " of " + findInDiv.highlights.length;
        if (isFromProgressNote) {
            $('#pnlClinicalProgressNote #find_msg').html(findInDiv.find_msg.innerHTML);
        }

        current_find = findInDiv.highlights[findInDiv.find_pointer];


        if (findInDiv.found_selected_rule == 1)
            current_find.className = "find_selected";
        else
            current_find.style.backgroundColor = "orange";


        findInDiv.scrollToPosition(findInDiv.highlights[findInDiv.find_pointer]);

    },



    checkkey: function (e) {
        var keycode;
        if (window.event)
            keycode = window.event.keyCode;
        else
            keycode = e.which;



        if (keycode == 13) {

            if (window.event && event.srcElement.id.match(/fwtext/i)) event.srcElement.blur();
            else if (e && e.target.id.match(/fwtext/i)) e.target.blur();
            findInDiv.findit();
        }
        else if (keycode == 27) {
            findInDiv.hide();
        }
    }, // end function checkkey()

    ShowProgressNoteSearch: function (objBtn) {
        findInDiv.unhighlight();
        $(objBtn).closest('header.panel-heading').find('#SearchInput').remove();
        $(objBtn).closest('header.panel-heading').find('div.panel-search').prepend('<div id="SearchInput"><div class="col-xs-12 p-none"><i class="fa fa-search fa-flip-horizontal"></i><input type="search" size="25" maxlength="25" id="fwtext" onchange="findInDiv.resettext();" onkeyup="Clinical_ProgressNote.FindTextOnNote(this)" placeholder="Find..." class="form-control mb-xs"></div>  <div class="col-xs-1 pr-none"><a href="javascript:void(0);" class="closeBtn pull-right" id="btnClose" onclick="findInDiv.hide(true);"><i class="fa fa-times"></i></a></div><div class="clearfix"></div><div class="controls"><input id="btn" type="button" value="Next" onclick="this.blur(); findInDiv.findit(true);" class="btn btn-default btn-xs pull-right"><input type="button" value="Prev" onclick="findInDiv.findprev(true);" class="btn btn-default btn-xs mr-xs pull-right"> <span id="find_msg"></span></div></div>')
        $(objBtn).closest('header.panel-heading').find('div.panel-search').find('#fwtext').focus();
        $(objBtn).closest('header.panel-heading').find('div.panel-search').find('#fwtext').keypress(function (event) {
            var keycode;
            if (window.event)
                keycode = window.event.keyCode;
            else
                keycode = e.which;

            if (keycode == 13) {
                $(this).blur();
                event.preventDefault();
                findInDiv.findit(true);
            }
            else if (keycode == 27) {
                findInDiv.hide();
            }
        }).keyup(function () {
            if ($(this).val() == "") {
                $(this).closest("#SearchInput").find("#find_msg").html('');
                findInDiv.unhighlight();
                $(this).focus()
            }

        });

    },

    // This function makes the findwindow DIV visible
    // so they can type in what they want to search for
    ShowProgressNoteSearchPopUp: function () {
        $("#findwindow").show();
        var textbox = document.getElementById('fwtext');


        findInDiv.findwindow.style.visibility = 'visible';



        textbox.focus();
        textbox.select();
        textbox.setSelectionRange(0, 9999);

        findInDiv.find_timer = setInterval('findInDiv.move_window();', 500);

        document.onkeydown = findInDiv.checkkey;

    },
    hide: function (isFromProgressNote) {
        findInDiv.unhighlight();
        if (isFromProgressNote) {
            $('#pnlClinicalProgressNote #SearchInput').remove();
        }
        $("#findwindow").hide();
        findInDiv.findwindow.style.visibility = 'hidden';


        clearTimeout(findInDiv.find_timer);


        document.onkeydown = null;

    },
    resettext: function () {
        if (findInDiv.find_text.toLowerCase() != document.getElementById('fwtext').value.toLowerCase())
            findInDiv.unhighlight();

    },


    move_window: function () {
        var fwtop = parseFloat(findInDiv.findwindow.style.top);
        var fwleft = parseFloat(findInDiv.findwindow.style.left);
        var fwheight = parseFloat(findInDiv.findwindow.style.height);


        if (document.documentElement.scrollTop)
            var current_top = document.documentElement.scrollTop;
        else
            var current_top = document.body.scrollTop;


        var current_bottom = (window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight) + current_top;


        if (document.documentElement.scrollLeft)
            var current_left = document.documentElement.scrollLeft;
        else
            var current_left = document.body.scrollLeft;


        var current_right = (window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) + current_left;



        if (fwtop < current_top) {

            findInDiv.findwindow.style.top = current_top + 30 + 'px';
        }
        else if (fwtop > current_bottom - fwheight) {

            findInDiv.findwindow.style.top = current_bottom - fwheight + 'px';
        }


        if (fwleft < current_left ||
            fwleft > current_right) {
            findInDiv.findwindow.style.left = current_left + 'px';
        }



    },

    MouseDown: function (event) {
        findInDiv.drag.tempx = findInDiv.drag.tempy = '';
        if (!event) event = window.event;
        var fobj = event.target || event.srcElement;


        var scrollLeft = document.body.scrollLeft || document.documentElement.scrollLeft;
        var scrollTop = document.body.scrollTop || document.documentElement.scrollTop;


        if (typeof fobj.nodeName != "undefined")
            if (fobj.nodeName.toLowerCase() == "input" ||
                fobj.nodeName.toLowerCase() == "textarea")
                return true;


        for (fobj; fobj; fobj = fobj.parentNode) {

            if (fobj.className)
                if (fobj.className.match(/dragme/i))
                    break;
        }


        if (fobj)
            if (fobj.className.match(/dragme/i)) {


                findInDiv.drag.drag_obj = fobj;
                findInDiv.drag.isdrag = true;
                findInDiv.drag.drag_obj_x = parseInt(findInDiv.drag.drag_obj.offsetLeft);
                findInDiv.drag.drag_obj_y = parseInt(findInDiv.drag.drag_obj.offsetTop);


                findInDiv.drag.mousex = event.clientX + scrollLeft;
                findInDiv.drag.mousey = event.clientY + scrollTop;


                if (event.type == "touchstart")
                    if (event.touches.length == 1) {
                        var touch = event.touches[0];
                        var node = touch.target;

                        findInDiv.drag.mousex = touch.pageX;
                        findInDiv.drag.mousey = touch.pageY;
                    }
                return true;
            }
    }, // end function MouseDown(event) 


    MouseMove: function (event) {
        if (findInDiv.drag.isdrag) {

            if (!event) event = window.event;
            findInDiv.drag.tempx = event.clientX;
            findInDiv.drag.tempy = event.clientY;


            var scrollLeft = document.body.scrollLeft || document.documentElement.scrollLeft;
            var scrollTop = document.body.scrollTop || document.documentElement.scrollTop;



            findInDiv.drag.tempx += scrollLeft;
            findInDiv.drag.tempy += scrollTop;

            findInDiv.drag.drag_obj.style.position = 'absolute';


            if (event.type == "touchmove")
                if (event.touches.length == 1) {
                    var touch = event.touches[0];
                    var node = touch.target;

                    findInDiv.drag.tempx = touch.pageX;
                    findInDiv.drag.tempy = touch.pageY;
                }


            findInDiv.drag.drag_obj.style.left = findInDiv.drag.drag_obj_x + findInDiv.drag.tempx - findInDiv.drag.mousex + "px";
            findInDiv.drag.drag_obj.style.top = findInDiv.drag.drag_obj_y + findInDiv.drag.tempy - findInDiv.drag.mousey + "px";
            return false;
        }
    }, // end function MouseMove(event)



    MouseUp: function () {
        if (findInDiv.drag.isdrag == true) {
            if (findInDiv.drag.tempx == '' && findInDiv.drag.tempy == '') {

            }
        }

        findInDiv.drag.isdrag = false;

    },


    scrollToPosition: function (field) {
        var scrollLeft = document.body.scrollLeft || document.documentElement.scrollLeft;
        var scrollTop = document.body.scrollTop || document.documentElement.scrollTop;
        var scrollBottom = (window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight) + scrollTop;
        var scrollRight = (window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth) + scrollLeft;


        if (field) {
            var theElement = field;
            var elemPosX = theElement.offsetLeft;
            var elemPosY = theElement.offsetTop;
            theElement = theElement.offsetParent;
            while (theElement != null) {
                elemPosX += theElement.offsetLeft
                elemPosY += theElement.offsetTop;
                theElement = theElement.offsetParent;
            }

            if (elemPosX < scrollLeft || elemPosX > scrollRight ||
                elemPosY < scrollTop || elemPosY > scrollBottom)

                field.scrollIntoView();
        }
    },
    getStyle: function (el, styleProp) {
        var x = (document.getElementById(el)) ? document.getElementById(el) : el;
        if (x.currentStyle)
            var y = x.currentStyle[styleProp];
        else if (window.getComputedStyle)
            var y = document.defaultView.getComputedStyle(x, null).getPropertyValue(styleProp);
        return y;
    },




    create_div: function (dleft, dtop, dwidth, dheight) {
        if (document.documentElement.scrollTop)
            var current_top = document.documentElement.scrollTop;
        else
            var current_top = document.body.scrollTop;

        if (document.getElementById('findwindow')) {
            findInDiv.findwindow = document.getElementById('findwindow');

        }
        else {
            findInDiv.findwindow.id = "findwindow";
            findInDiv.findwindow.style.position = 'absolute';

            document.body.insertBefore(findInDiv.findwindow, document.body.firstChild);
            findInDiv.findwindow.className = 'findwindow dragme';
            findInDiv.findwindow.style.visibility = 'hidden';
        }

        findInDiv.findwindow.style.backgroundColor = findInDiv.find_window_background;
        findInDiv.findwindow.style.border = '2px solid ' + findInDiv.find_window_border;
        findInDiv.findwindow.style.color = findInDiv.find_text_color;
        findInDiv.findwindow.style.width = findInDiv.find_window_width + 'px';
        findInDiv.findwindow.style.height = +findInDiv.find_window_height + 'px';
        findInDiv.findwindow.style.top = '20px';
        findInDiv.findwindow.style.left = '20px';
        findInDiv.findwindow.style.padding = '0px';
        findInDiv.findwindow.style.zIndex = 2000;
        findInDiv.findwindow.style.fontSize = '14px';
        findInDiv.findwindow.style.overflowX = 'hidden';

        findInDiv.findwindow.innerHTML = '<div style="text-align: center'
        + ';width: ' + (findInDiv.find_window_width - 20) + 'px'
        + ';cursor: move'
        + ';color: ' + findInDiv.find_title_color
        + ';border: 1px solid ' + findInDiv.find_text_color
        + ';background-color: ' + findInDiv.find_window_border
        + ';float: left'
        + ';" onmouseover="over=1;" onmouseout="over=0;">'
        + 'Find Window</div>';

        findInDiv.findwindow.innerHTML += '<div onclick="findInDiv.hide();" class="close" style="text-align: center'
        + ';width: ' + (16) + 'px'
        + ';cursor: default'
        + ';font-weight: bold'
        + ';background-color: red'
        + ';border: 1px solid ' + findInDiv.find_text_color
        + ';float: right'
        + ';">'
        + 'X'
        + '</div><br />\n';

        findInDiv.findwindow.innerHTML += '<div id="window_body" style="padding: 5px;">'
        + '<form onsubmit="return false;"><input type="search" size="25" maxlength="25" id="fwtext"'
        + ' onchange="findInDiv.resettext();" placeholder="Enter text to find">'
        + '<input type="button" value="Find Prev" onclick="findInDiv.findprev();">'
        + '<input id="btn" type="button" value="Find Next" onclick="this.blur(); findInDiv.findit();">'
        + ' <span id="find_msg"><br /></span>'
        + '</form></div>\n';



        // ss.cssRules is available, so proceed with desired operations.
        var sheets = document.styleSheets;
        for (var i = 0; i < sheets.length; i++) {
            // cssRules respects same-origin policy, as per
            // https://code.google.com/p/chromium/issues/detail?id=49001#c10.
            var rules_ = null;
            var cssRules_ = null;
            try {
                rules_ = sheets[i].rules;
                cssRules_ = sheets[i].cssRules;
            } catch (e) {
                console.log(e.message);
            }

            if (rules_ != null)
            {
                if (!rules_) {


                    try {
                        // In Chrome, if stylesheet originates from a different domain,
                        // ss.cssRules simply won't exist. I believe the same is true for IE, but
                        // I haven't tested it.
                        //
                        // In Firefox, if stylesheet originates from a different domain, trying
                        // to access ss.cssRules will throw a SecurityError. Hence, we must use
                        // try/catch to detect this condition in Firefox.
                        if (!sheets[i].cssRules)
                            return;
                    } catch (e) {
                        // Rethrow exception if it's not a SecurityError. Note that SecurityError
                        // exception is specific to Firefox.
                        if (e.name !== 'SecurityError')
                            throw e;
                        return;
                    }
                }
                var rules = (rules_) ? rules_ : cssRules_;
                if (rules != null)
                    for (var j = 0; j < rules.length; j++) {
                        if (rules[j].selectorText == '.highlight')
                            findInDiv.found_highlight_rule = 1;
                        else if (rules[j].selectorText == '.find_selected')
                            findInDiv.found_selected_rule = 1;
                    }
            }
           
        }


    },

    textarea2pre: function (el) {
        if (el.nextSibling && el.nextSibling.id && el.nextSibling.id.match(/pre_/i))
            var pre = el.nextsibling;
        else
            var pre = document.createElement("pre");

        var the_text = el.value;
        the_text = the_text.replace(/>/g, '&gt;').replace(/</g, '&lt;').replace(/"/g, '&quot;');

        pre.innerHTML = the_text;

        var completeStyle = "";
        if (el.currentStyle) {
            var elStyle = el.currentStyle;
            for (var k in elStyle) { completeStyle += k + ":" + elStyle[k] + ";"; }

            pre.style.border = "1px solid black";
        }
        else // webkit
        {
            completeStyle = window.getComputedStyle(el, null).cssText;
            pre.style.cssText = completeStyle;
        }

        el.parentNode.insertBefore(pre, el.nextSibling);


        el.onblur = function () { this.style.display = "none"; pre.style.display = "block"; };

        el.onchange = function () { pre.innerHTML = el.value.replace(/>/g, '&gt;').replace(/</g, '&lt;').replace(/"/g, '&quot;'); };

        el.style.display = "none";
        pre.id = "pre_" + findInDiv.highlights.length;


        pre.onclick = function () { this.style.display = "none"; el.style.display = "block"; el.focus(); el.click() };




    },

    // ver 5.1 - 10/17/2014
    selectElementContents: function (el) {
        if (window.getSelection && document.createRange) {

            var range = document.createRange();
            range.selectNodeContents(el);
            var sel = window.getSelection();
            sel.removeAllRanges();
            sel.addRange(range);
        } else if (document.body.createTextRange) {

            var textRange = document.body.createTextRange();
            textRange.moveToElementText(el);
            textRange.select();

        }
    }, // end function selectElementContents(el) 
}