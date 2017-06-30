using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class LabCustomBinder:IModelBinder
   
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            string labname =request.Form.Get("labName");
            int labId = Convert.ToInt32(request.Form.Get("labId"));
            string labdescription =request.Form.Get("labDescription");
            string logo1 = request.Form.Get("fileField");
            string labSatus = "Active";
            string labcode = request.Form.Get("labCode");
            DateTime modifiedOn = DateTime.Now;

            return new LabMaster
           {

               labName=labname,
               labId=labId,
               labCode=labcode,
               labDescription=labdescription,
               logo=logo1,
               labStatus=labSatus,
               modifiedOn=modifiedOn
           };


            }



        }



    }


