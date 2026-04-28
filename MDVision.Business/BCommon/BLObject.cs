
/*-------------------------------------------------------------------------------------------------------
 * FileName         : Business/DAObject.cs 
 * Author           : Muhammad Naeem.
 * Last Modified    : 20140904 
 * The DAObject represents the Return type of all the DataAccess functions. The idea of this object is the return the error message any other attached items along with the functions primitive DataType. i.e: if a function supposed to return a DataSet and in case of some exception we also want to some error message, then DAObject is the return type which will contain multiple objects.
 * 
---------------------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDVision.Business.BCommon
{
    [Serializable]
    public class BLObject<T>
    {
         /// <summary>
        /// Code of Error in case of any error to be returned with DAObject
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Error or Info message to be returned with DAObject
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The Data or real value needs to be returned with DAObject
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BLObject() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <param name="errCode"></param>
        public BLObject(T data, string msg = "", string errCode = "")
        {
            this.Message = msg;
            this.ErrorCode = errCode;
            this.Data = data;
        }
    }
}
