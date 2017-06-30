using System;
using CodeFirstEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MvcRetailApp.ModelBinder
{
    public class ItemSubCategoryCustomBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id = Convert.ToInt32(request.Form.Get("ItemSubCategoryDetails.subCategoryId"));
            string itemSubCategoryName = request.Form.Get("ItemSubCategoryDetails.subCategoryName");
            string itemCategoryName = request.Form.Get("ItemSubCategoryDetails.categoryName");
            string subcatcode = request.Form.Get("ItemSubCategoryDetails.ItemSubCategoryCode");
            string status = "Active";
            DateTime dateModifiedOn = DateTime.Now;

            return new ItemSubCategory
            {
                subCategoryId = id,
                subCategoryName = itemSubCategoryName,
                categoryName = itemCategoryName,
                ItemSubCategoryCode = subcatcode,
                status=status,
                ModifiedOn = dateModifiedOn
            };
        }
    }
}