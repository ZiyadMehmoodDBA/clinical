using MDVision.IEHR.EMR.Model.OrdersAndResults.LabOrder;
using MDVision.IEHR.EMR.Model.OrdersAndResults.ProcedureOrder;
using MDVision.IEHR.EMR.Model.OrdersAndResults.RadiologyOrder;
using MDVision.Model.Clinical.OrdersAndResults.ConsultationOrder;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace MDVision.IEHR.Common
{
    public class OrderHelpers
    {
        public static List<object> GetListOfObject(string objectType, string selectedIds, Dictionary<string, dynamic> dictCurrentJSON)
        {
            Type CurrentModel = null;
            List<object> lstObjects = new List<object>();

            if (objectType == "LabTestModel")
            {
                CurrentModel = typeof(LabOrderTestModel);
            }
            else if (objectType == "RadiologyTestModel")
            {
                CurrentModel = typeof(RadiologyOrderTestModel);
            }
            else if (objectType == "ProcedureTestModel")
            {
                CurrentModel = typeof(ProcedureOrderTestModel);
            }
            else if (objectType == "ConsultationTestModel")
            {
                CurrentModel = typeof(ConsultationOrderTestModel);
            }


            PropertyInfo[] ArrCurrentModelPropertyInfo = CurrentModel.GetProperties();
            foreach (string item in selectedIds.Split(','))
            {
                if (item != "" && item.ToLower() != "template")
                {
                    object currentObject = null;
                    if (objectType == "LabTestModel")
                    {
                        currentObject = new LabOrderTestModel();
                    }
                    else if (objectType == "RadiologyTestModel")
                    {
                        currentObject = new RadiologyOrderTestModel();
                    }
                    else if (objectType == "ProcedureTestModel")
                    {
                        currentObject = new ProcedureOrderTestModel();
                    }
                    else if (objectType == "ConsultationTestModel")
                    {
                        currentObject = new ConsultationOrderTestModel();
                    }

                    if (currentObject != null)
                    {
                        foreach (PropertyInfo CurrentProperty in ArrCurrentModelPropertyInfo)
                        {
                            string objProperty = CurrentProperty.Name;
                            
                            if (item.Equals("0") || dictCurrentJSON.ContainsKey(objProperty)) {
                                
                                objProperty = objProperty.Equals("Urgency_text") ? "Urgency" + item + "_text" : objProperty;
                                objProperty = objProperty.Equals("CollectedAt_text") ? "CollectedAt" + item + "_text" : objProperty;

                                dynamic jsonPropertyValue = dictCurrentJSON[objProperty];
                                string stringValue = jsonPropertyValue.GetType() == typeof(string) ? jsonPropertyValue : jsonPropertyValue.ToString();

                                currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, stringValue);
                            }
                            if (dictCurrentJSON.ContainsKey(CurrentProperty.Name + item))
                            {
                                objProperty = CurrentProperty.Name + item;
                                currentObject.GetType().GetProperty(CurrentProperty.Name).SetValue(currentObject, dictCurrentJSON[objProperty]);
                            }

                        }

                        lstObjects.Add(currentObject);
                    }
                }
            }

            return lstObjects;
        }
    }
}