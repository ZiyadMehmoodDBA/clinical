letterDetail = {
    params: [],
    bIsFirstLoad: true,
    LetterID: -1,
    imageSize:0,
    Load: function (params) {

        letterDetail.params = params;

        if (letterDetail.params["PanelID"] != 'letterDetail')
            letterDetail.params["PanelID"] = letterDetail.params["PanelID"] + ' #letterDetail'
        
       

        if (letterDetail.bIsFirstLoad) {
            letterDetail.bIsFirstLoad = false;
            var self = $('#' + letterDetail.params["PanelID"]);

            self.loadDropDowns(true).done(function () {

                if (letterDetail.params.mode == "Add") {
                    //intialize TinyMCE instance on textarea control and tinymce disabled
                    InitTinymceControl(true);

                    //serialize data
                    $('#frmletterDetail').data('serialize', $('#frmletterDetail').serialize());
                }
                else if (letterDetail.params.mode == "Edit") {


                    letterDetail.LoadLetter();
                }
                letterDetail.ValidateQLetterDetail(letterDetail.params.LetterID);

            });
            
        }
    },

    LoadLetter: function (LetterId, mode) {
        if (letterDetail.params.mode == "Add") {

            //serialize data
            $('#frmletterDetail').data('serialize', $('#frmletterDetail').serialize());
        }
        else if (letterDetail.params.mode == "Edit") {

            letterDetail.FillLetter(letterDetail.params.LetterID).done(function (response) {
                if (response.status != false) {
                   
                    var self = $('#letterDetail');
                    utility.bindMyJSON(true, JSON.parse(response.LetterLoad_JSON), false, self);
                    if (JSON.parse(response.LetterLoad_JSON)['chkLabel'] == 'True')
                        $('#letterDetail #chkLabel').attr("checked", true);
                    else {
                        $('#letterDetail #chkLabel').attr("checked", false);
                        
                    }
                    if (JSON.parse(response.LetterLoad_JSON)['chkActive'] == 'True')
                        $('#letterDetail #chkActive').attr("checked", true);
                    else
                        $('#letterDetail #chkActive').attr("checked", false);
                    ////intialize TinyMCE instance on textarea control and enabled tinymce
                   
                    setTimeout(function () { InitTinymceControl(false); }, 100);

                    //serialize data
                    $('#frmletterDetail').data('serialize', $('#frmletterDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    FillLetter: function (letterID) {
        var data = "LetterID=" + letterID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "DESIGN_LETTER", "FILL_LETTER");
    },

    ValidateQLetterDetail: function (letterID) {
        $('#frmletterDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  txtLetterName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  lstCategoryId: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  lstEntityId: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           letterDetail.saveLetter(letterID);
       });
    },

    saveLetter: function (letterID) {
        var strMessage = "";
        var self = $('#letterDetail');
        var myJSON = self.getMyJSON();
        myJSON = JSON.parse(myJSON);
        myJSON.elm1 = tinyMCE.activeEditor.getContent();
        myJSON = JSON.stringify(myJSON);
     
        //if (questionDetail.params.mode == null) {
        //AppPrivileges.GetFormPrivileges("Messages", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            if (letterDetail.params.mode == "Edit") {
                letterDetail.letterEdit(myJSON, letterDetail.params.LetterID).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        DELETED_FieldsIDsFrom_USER();
                        Design_Letter.LetterSearch();
                        if (letterDetail.params != null && letterDetail.params.ParentCtrl != null) {
                          //  UnloadActionPan(letterDetail.params.ParentCtrl, 'letterDetail');
                           
                        }
                      //  else
                          //  UnloadActionPan(null, 'letterDetail');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else if (letterDetail.params.mode == "Add") {
                letterDetail.letterSave(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        DELETED_FieldsIDsFrom_USER();
                        Design_Letter.LetterSearch();
                        InitTinymceControl(false);
                        letterDetail.params.LetterID = response.LetterId;
                        letterDetail.params.mode = "Edit";
                        $('#letterDetail #hfLetterId').val(response.LetterId);
                            if (letterDetail.params != null && letterDetail.params.ParentCtrl != null) {
                           // UnloadActionPan(letterDetail.params.ParentCtrl, 'letterDetail');
                        }
                       // else
                          //  UnloadActionPan(null, 'letterDetail');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
        //}
    },

    letterSave: function (letterData) {
        var data = "letterData=" + letterData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "DESIGN_LETTER", "SAVE_LETTER");
    },

    letterEdit: function (letterData, letterID) {
        if (ValidImagesSize(tinyMCE.activeEditor.getContent())) {
            var data = "letterData=" + letterData.replace(/&/g, '||') + "&letterID=" + letterID + "&letterBody=" + tinyMCE.activeEditor.getContent().replace(/&/g, '||');//'{EditorContent:" + tinyMCE.activeEditor.getContent()+"'";
            // serach parameter , class name, command name of class 
            return MDVisionService.defaultService(data, "DESIGN_LETTER", "UPDATE_LETTER");
        } else {
            utility.DisplayMessages("Please select Image less than 5 mb Size", 2);
           
        }
       
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmletterDetail', function () {
            
            //DELETED_FieldsIDsFrom_USER();
            //removing tinyMCE Instance from text area
            tinymce.remove('#elm1');
            UnloadActionPan();
            UnloadActionPan(letterDetail.params.ParentCtrl, 'letterDetail');

        }, function () {
           
            //DELETED_FieldsIDsFrom_USER();
            UnloadActionPan();

        });

    }
}

function ValidImagesSize(htmlString) {
    var $jQueryObject = $($.parseHTML(htmlString));
    var ImagesList = $jQueryObject.find("img");
    var ImageSize = 0;
    for (var i = 0; i < ImagesList.length; i++) {
        ImageSize += Base64ImageSizeCalculator(ImagesList[i].src);
    }
    
    if (ImageSize > Number(globalAppdata['FileSize'])) {
        return false;
    }
    return true;
}
function Base64ImageSizeCalculator(EncodedImageString) {
    return ((EncodedImageString.length) / 1.37) / 1000000;

}
function DELETED_FieldsIDsFrom_USER() {
    htmlString = tinyMCE.activeEditor.getContent();
    var deletedFieldsIDsFromLetter = '';

    var $jQueryObject = $($.parseHTML(htmlString));
    var deletedFieldsIDsFromLettessr = $jQueryObject.find(".FieldInserted_PK");
    for (var i = 0; i < deletedFieldsIDsFromLettessr.length; i++) {

        if (i > 0) {
            deletedFieldsIDsFromLetter += ",";
        }
        deletedFieldsIDsFromLetter += deletedFieldsIDsFromLettessr[i].id;

    }


    var data = "deletedFieldsIDsFromLetter=" + deletedFieldsIDsFromLetter + "&LetterID=" + $('#letterDetail #hfLetterId').val();
    MDVisionService.defaultService(data, "DESIGN_LETTER", "DELETED_FieldsIDsFrom_USER");

}

function InitTinymceControl(Isreadonly) {
    if (typeof tinymce.activeEditor != 'undefined') {
      
        tinymce.EditorManager.execCommand('mceRemoveEditor', true, "elm1");
    }
    var UpdateImageTiny;
    tinymce.init({
        selector: "textarea#elm1",
        theme: "modern",
        readonly:Isreadonly,
        height: 400,
        plugins: [
            "advlist autolink  image lists charmap print preview hr anchor pagebreak spellchecker",
            "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime  nonbreaking",
            "save table contextmenu directionality  template paste textcolor importcss"
        ],
        content_css: "css/development.css",
        add_unload_trigger: false,
        paste_data_images: true, //enable drag drop image pasting
        toolbar1: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons table",
        toolbar2: "custompanelbutton textbutton spellchecker",

        image_advtab: true,

        style_formats: [
            { title: 'Bold text', format: 'h1' },
            { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
            { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
            { title: 'Example 1', inline: 'span', classes: 'example1' },
            { title: 'Example 2', inline: 'span', classes: 'example2' },
            { title: 'Table styles' },
            { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ],

        template_replace_values: {
            username: "Jack Black"
        },

        template_preview_replace_values: {
            username: "Preview user name"
        },

        //file_browser_callback: function() {},

        templates: [
            { title: 'Some title 1', description: 'Some desc 1', content: '<strong class="red">My content: {$username}</strong>' },
            { title: 'Some title 2', description: 'Some desc 2', url: 'development.html' }
        ],

        setup: function (ed) {
            ed.addButton('custompanelbutton', {
                type: 'panelbutton',
                text: 'Panel',
                panel: {
                    type: 'form',
                    items: [
                        { type: 'button', text: 'Ok' },
                        { type: 'button', text: 'Cancel' }
                    ]
                }
            });
            ed.on("init",
                function (ed) {
                // tinymce.activeEditor.setContent(' <input type="text" readonly id="EncodedImageString" value="Azhar" style="min-width: 10px; margin: 0 10px; margin-right: right:10px; border: none;border-right: black solid 1px;border-left: black solid 1px;padding: 0 5px;"/>');
                 ed.target.addMenuItem('InsertDataField', {
                        text: 'Insert Field',
                        context: 'InsertDataField',
                        onPostRender: postRenderPatient,
                        onclick: function (e) {
                        
                            var params = [];
                            params["LetterId"] = "-1";
                            params["mode"] = "Add";
                            params["ParentCtrl"] = "letterDetail";
                            params["CategoryID"] = $('#letterDetail #lstCategoryId option:selected').val();
                            params["letterID"] = $('#letterDetail #hfLetterId').val();
                            LoadActionPan('designLetterDataFieldsInsert', params);
                           // var tempsss = "";
                           // tinymce.execCommand('mceInsertContent', false, "[ " + e.target.innerHTML + " ]");

                        }
                    });
                    ed.target.addMenuItem('CreateNewDataField', {
                        text: 'Create New Field',
                        context: 'CreateNewDataField',
                        onPostRender: postRenderPatient,
                        onclick: function (e) {

                            var params = [];
                            params["LetterId"] = "-1";
                            params["mode"] = "Add";
                            params["CategoryID"] = $('#letterDetail #lstCategoryId option:selected').val();
                            params["ParentCtrl"] = "letterDetail";
                            LoadActionPan('designLetterDataFieldsCreate', params);
                            // var tempsss = "";
                            // tinymce.execCommand('mceInsertContent', false, "[ " + e.target.innerHTML + " ]");

                        }
                    });
                });
            ed.addButton('textbutton', {
                type: 'button',
                text: 'Text'
            });
        },

        spellchecker_callback: function (method, words, callback) {
            if (method == "spellcheck") {
                var suggestions = {};

                for (var i = 0; i < words.length; i++) {
                    suggestions[words[i]] = ["First", "second"];
                }

                callback(suggestions);
            }
        },

        file_picker_callback: function (callback, value, meta) {
            UpdateImageTiny = callback;
            document.getElementById("ImageUploaderTinymce").click();
            if (meta.filetype == 'file') {
            }
            // Provide image and alt text for the image dialog
            if (meta.filetype == 'image') {
                callback($('#EncodedImageString').val(), { alt: $('#ImageUploaderTinymce').val().split('\\').pop() });
                
                
            }

            // Provide alternative source and posted for the media dialog
            if (meta.filetype == 'media') {
            }
        },
        contextmenu: "link image inserttable | cell row column deletetable | CreateNewDataField InsertDataField"
    });

    jQuery(function () {
        document.getElementById('ImageUploaderTinymce').addEventListener('change', function () {
            readImage(this);
        }, false);
    });
    function postRenderPatient() {

    }
    function readImage(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#EncodedImageString').val(e.target.result);
                if ($('#ImageUploaderTinymce').val() != "") {
                   
                 
                    if ((($('#EncodedImageString').val().length) / 1.37) / 1000000 > Number(globalAppdata['FileSize'])) {
                       
                        utility.DisplayMessages("Please select Image less than 5 mb Size", 2);
                        return false;
                    } else {
                        UpdateImageTiny($('#EncodedImageString').val(), { alt: $('#ImageUploaderTinymce').val().split('\\').pop() });
                    }
                    
                }
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

}

