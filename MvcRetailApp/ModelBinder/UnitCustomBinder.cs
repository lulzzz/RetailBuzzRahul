using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class UnitCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id = Convert.ToInt32(request.Form.Get("UnitDetails.UnitId"));
            string name = request.Form.Get("UnitDetails.UnitName");
            string numbertype = request.Form.Get("UnitDetails.NumberType");
            string status = "Active";
            DateTime modifiedOn = DateTime.Now;

            return new UnitMaster
            {
                UnitId = id,
                UnitName = name,
                status = status,
                NumberType = numbertype,
                modifiedOn = modifiedOn
            };
        }
    }
}