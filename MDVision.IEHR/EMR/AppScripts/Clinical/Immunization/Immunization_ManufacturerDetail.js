Immunization_ManufacturerDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Immunization_ManufacturerDetail.params = params;
        if (Immunization_ManufacturerDetail.params.PanelID != 'pnlImmunization_ManufacturerDetail') {
            Immunization_ManufacturerDetail.params.PanelID = Immunization_ManufacturerDetail.params.PanelID + ' #pnlImmunization_ManufacturerDetail';
        } else {
            Immunization_ManufacturerDetail.params.PanelID = 'pnlImmunization_ManufacturerDetail';
        }

        var self = $('#' + Immunization_ManufacturerDetail.params.PanelID);

        if (Immunization_ManufacturerDetail.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                Immunization_ManufacturerDetail.bIsFirstLoad = false;
               
               
                Immunization_ManufacturerDetail.ValidateManufacturer();
                if (Immunization_ManufacturerDetail.params.mode == "Edit") {
                    $('#' + Immunization_ManufacturerDetail.params.PanelID + " #frmManufacturerDetail #IsActive").attr("disabled", false);
                    Immunization_ManufacturerDetail.LoadManufacturerDetail();
                }
                else {
                    $('#' + Immunization_ManufacturerDetail.params.PanelID + " #frmManufacturerDetail #IsActive").attr("disabled", true);
                }
            });
        }
    },
    
    LoadManufacturerDetail: function () {
        Immunization_ManufacturerDetail.SearchManufacturerDetail().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var self = $('#' + Immunization_ManufacturerDetail.params.PanelID + " #frmManufacturerDetail");
                var Manufacturer_JSON = response.Manufacturer_JSON[0];
                utility.bindMyJSONByName(true, Manufacturer_JSON, false, self).done(function () {
                    if (Manufacturer_JSON.Status == "Active") {
                        $('#' + Immunization_ManufacturerDetail.params.PanelID + " #frmManufacturerDetail #IsActive").prop('checked', true);
                    }
                    else {
                        $('#' + Immunization_ManufacturerDetail.params.PanelID + " #frmManufacturerDetail #IsActive").prop('checked', false);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SearchManufacturerDetail: function () {
        var objData = new Object();
        objData["ManufacturerId"] = Immunization_ManufacturerDetail.params.ManufacturerId;
        objData["commandType"] = "Load_Manufacturer_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },

    ValidateManufacturer: function () {
        $('#' + Immunization_ManufacturerDetail.params.PanelID + ' #frmManufacturerDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  ManufacturerName: {
                      group: '.col-md-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  MVXCode: {
                      group: '.col-md-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Immunization_ManufacturerDetail.ManufacturerSave();
       });
    },
    ManufacturerSave: function () {
        var self = $("#" + Immunization_ManufacturerDetail.params.PanelID + " #frmManufacturerDetail");
        var myJSON = self.getMyJSONByName();
        if (Immunization_ManufacturerDetail.params.mode == "Add") {
            Immunization_ManufacturerDetail.SaveManufacturer(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_Manufacturer.SearchManufacturer();
                    Immunization_ManufacturerDetail.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (Immunization_ManufacturerDetail.params.mode == "Edit") {
            Immunization_ManufacturerDetail.SaveManufacturer(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Immunization_Manufacturer.SearchManufacturer();
                    Immunization_ManufacturerDetail.UnLoadTab();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveManufacturer: function (ManufacturerData) {
        var objData = JSON.parse(ManufacturerData);
        
        if (Immunization_ManufacturerDetail.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_Manufacturer";
            if (objData.IsActive) {
                objData["Status"] = "Active";
            }
            else {
                objData["Status"] = "InActive";
            }
            objData["ManufacturerId"] = Immunization_ManufacturerDetail.params.ManufacturerId;
        }
        else {
            objData["commandType"] = "SAVE_Manufacturer";
            objData["Status"] = "Active";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_ManufacturerDetail.params["FromAdmin"] == "0") {
            if (Immunization_ManufacturerDetail.params != null && Immunization_ManufacturerDetail.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_ManufacturerDetail.params.ParentCtrl, 'Immunization_ManufacturerDetail');
            }
            else
                UnloadActionPan(null, 'Immunization_ManufacturerDetail');
        }
        else {
            RemoveAdminTab();
        }
        return objDeffered;
    },

}