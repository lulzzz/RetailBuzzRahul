using System;
using CodeFirstEntities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.ModelBinder
{
    public class DesignCustomBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;
            int id = Convert.ToInt32(request.Form.Get("DesignDetails.DesignId"));
            string designcode=request.Form.Get("DesignDetails.DesignCode");
            string catcode=request.Form.Get("DesignDetails.CategoryCode");
            string subcategory=request.Form.Get("DesignDetails.SubCategoryName");
            string designname = request.Form.Get("DesignDetails.DesignName");
            string designdes=request.Form.Get("DesignDetails.DesignDescription");
            string designsize=request.Form.Get("DesignDetails.DesignSize");
            string designpic=request.Form.Get("DesignDetails.DesignPhoto");
            string status = "Active";
            DateTime dateModifiedOn = DateTime.Now;
          
            return new DesignMaster
                {
                    DesignId=id,
                    DesignCode=designcode,
                    CategoryCode=catcode,
                    SubCategoryName = subcategory,
                    DesignName=designname,
                    DesignDescription=designdes,
                    //DesignSize=designsize,
                    DesignPhoto=designpic,
                    Status=status,
                    ModifiedOn=dateModifiedOn

                };
            }
        }
    }

