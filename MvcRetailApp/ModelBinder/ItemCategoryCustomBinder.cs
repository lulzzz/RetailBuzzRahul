using System;
using CodeFirstEntities;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class ItemCategoryCustomBinder:IModelBinder
    {


         public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id = Convert.ToInt32(request.Form.Get("ItemCategoryDetails.CategoryId"));
            string itemCategoryName = request.Form.Get("ItemCategoryDetails.CategoryName");
            string itemcategorycode = request.Form.Get("ItemCategoryDetails.ItemCategoryCode");
            string status = "Active";
            DateTime dateModifiedOn = DateTime.Now;

            return new ItemCategory
                {
                    CategoryId = id,
                    CategoryName = itemCategoryName,
                    ItemCategoryCode=itemcategorycode,
                    itemCategoryStatus = status,
                    ModifiedOn = dateModifiedOn
                };
            }
        
     }


   }
