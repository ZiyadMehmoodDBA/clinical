using MDVision.DataAccess.DAL.Admin;
using MDVision.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MDVision.WebAPI.Controllers
{
    public static class WebApiCommon
    {
        public static bool CheckNetwrokIP(string IP)
        {
            if (IP == null || IP == "")
            { return false; }
            else
            {
                string ConstanIpPortion = IP.Substring(0, IP.LastIndexOf(".") + 1);
                string VariableIpPortion = IP.Substring(IP.LastIndexOf(".") + 1);
                List<NetworkIp> Obj = DALCustomers.Instance.LoadNetworkIP();
                if (Obj == null)
                {

                    return false;
                }
                else
                {
                    if (Obj.Count < 1)
                    {
                        return false;
                    }
                    else
                    {
                        List<NetworkIp> filteredObj = Obj.Where(m => m.IPAdress.Contains(ConstanIpPortion)).ToList();
                        if (filteredObj.Count < 1)
                        { return false; }
                        else
                        {
                            if (int.Parse(VariableIpPortion) > 0 && int.Parse(VariableIpPortion) < 255)
                            {
                                return true;
                            }
                            else
                            {
                                return false;

                            }
                        }
                    }
                }
            }
            

        }
    }
}