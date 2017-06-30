
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;

namespace MvcRetailApp.ModelBinder
{
    public class SalesIncentiveCustumBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int Id = Convert.ToInt32(request.Form.Get("SalesIncentiveDetails.SIId"));
            string SlabFrom = request.Form.Get("SalesIncentiveDetails.SISlabFrom");
            string SlabTo = request.Form.Get("SalesIncentiveDetails.SISlabTo");
            string percentage = request.Form.Get("SalesIncentiveDetails.Percentage");
            string Status = "Active";
            DateTime ModifiedOn = DateTime.Now;
            return new SalesIncentiveMaster
            {
                SIId = Id,
                SISlabFrom = SlabFrom,
                SISlabTo = SlabTo,
                Percentage = percentage,
                Status = Status,
                ModifiedOn = ModifiedOn
            };
        }
    }
}