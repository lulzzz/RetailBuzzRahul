using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class StockCustomBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int stockid = Convert.ToInt32(request.Form.Get("StockId"));
            string stockcode = request.Form.Get("StockCode");
            string category = request.Form.Get("Category");
            string barcode = request.Form.Get("Barcode");
            string name = request.Form.Get("Name");
            string description = request.Form.Get("Description");
            double quantity = Convert.ToDouble(request.Form.Get("Quantity"));
            DateTime purchaseddate = Convert.ToDateTime(request.Form.Get("PurchasedDate"));
            string suppliername = request.Form.Get("SupplierName");
            double rate = Convert.ToDouble(request.Form.Get("Rate"));
            double weight = Convert.ToDouble(request.Form.Get("Weight"));
            string size = request.Form.Get("Size");
            string measurement = request.Form.Get("Measurement");
            string status = "Active";
            DateTime modifiedOn = DateTime.Now;
            return new StockMaster
            {
                StockId = stockid,
                Barcode = barcode,
                Category = category,
                Name = name,
                Description=description,
                Quantity = quantity,
                PurchasedDate = purchaseddate,
                SupplierName = suppliername,
                Rate = rate,
                Weight = weight,
                Size=size,
                Measurement=measurement,
                Status = status,
                ModifiedOn = modifiedOn

            };

        }

    }
}