using System;
using CodeFirstEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class BrandCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id = Convert.ToInt32(request.Form.Get("BrandDetails.Id"));
            string brandName = request.Form.Get("BrandDetails.BrandName");
            string brandCode = request.Form.Get("BrandDetails.BrandCode");
            string status = "Active";
            DateTime dateModifiedOn = DateTime.Now;

            return new BrandMaster
            {
                Id = id,
                BrandName = brandName,
                BrandCode = brandCode,
                Status = status,
                ModifiedOn = dateModifiedOn
            };
        }

    }


}
