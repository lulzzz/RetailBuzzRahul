using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;

namespace MvcRetailApp.ModelBinder
{
    public class logindetailBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllercontext, ModelBindingContext modelbindingcontext)
        {
            HttpRequestBase reqest = controllercontext.HttpContext.Request;
           // int id = Convert.ToInt32(reqest.Form.Get("loginid"));
            string name = reqest.Form.Get("username");
            string password = reqest.Form.Get("password");
            return new logindetail { 
              //  loginid=id,
                username=name,
                 password=password
            };
        }
    }
}