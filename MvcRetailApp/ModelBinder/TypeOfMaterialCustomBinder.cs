using System;
using CodeFirstEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class TypeOfMaterialCustomBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id = Convert.ToInt32(request.Form.Get("MaterialDetails.MaterialId"));
            string MaterialName = request.Form.Get("MaterialDetails.MaterialName");
            String MaterialShortName = request.Form.Get("MaterialDetails.MaterialShortName");
            String MaterialDescription = request.Form.Get("MaterialDetails.MaterialDescription");
            DateTime ModifiedOn = DateTime.Now;
            string Status = "Active";
            string MaterialCode = request.Form.Get("MaterialDetails.MaterialCode");
            return new TypeOfMaterial
            {
                MaterialId=id,
                MaterialName = MaterialName,
                MaterialShortName=MaterialShortName,
                MaterialDescription=MaterialDescription,
                ModifiedOn=ModifiedOn,
                Status=Status,
                MaterialCode=MaterialCode
            };
        }
       
    }
}