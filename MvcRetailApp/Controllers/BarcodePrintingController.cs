using CodeFirstData;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class BarcodePrintingController : Controller
    {
        private string UserEmail
        {
            get { return (string)HttpContext.Session["UserEmail"]; }
            set { HttpContext.Session["UserEmail"] = value; }
        }
        private string CompanyCode
        {
            get { return (string)HttpContext.Session["CompanyCode"]; }
            set { HttpContext.Session["CompanyCode"] = value; }
        }
        private string CompanyName
        {
            get { return (string)HttpContext.Session["CompanyName"]; }
            set { HttpContext.Session["CompanyName"] = value; }
        }
        private string FinancialYear
        {
            get { return (string)HttpContext.Session["FinancialYear"]; }
            set { HttpContext.Session["FinancialYear"] = value; }
        }
        private static string DatabaseName
        {
            set { ((dynamic)System.Web.HttpContext.Current.ApplicationInstance).DynamicDatabase = value; }
        }
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IOpeningStockService _openingstockservice;
        private readonly IItemCategoryService _itemcategortservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IItemService _itemservice;
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IBarcodeNumberService _BarcodeNumberService;

        public BarcodePrintingController(IUserCredentialService IUserCredentialService, IBarcodeNumberService IBarcodeNumberService, IModuleService iIModuleService, IOpeningStockService openingstockservice, IItemCategoryService itemcategortservice, IItemSubCategoryService itemsubcategortservice, IItemService itemservice, IInwardFromSupplierService InwardFromSupplierService, IInwardItemFromSupplierService InwardItemFromSupplierService)
        {
            this._openingstockservice = openingstockservice;
            this._IUserCredentialService = IUserCredentialService;
            this._iIModuleService = iIModuleService;
            this._itemcategortservice = itemcategortservice;
            this._itemsubcategoryservice = itemsubcategortservice;
            this._itemservice = itemservice;
            this._InwardFromSupplierService = InwardFromSupplierService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._BarcodeNumberService = IBarcodeNumberService;
        }

        //THIS IS FOR NO ACCESS TO CHANGE URL IF ANYONE TRY TO CHANGE THEN GOES TO LOGIN PAGE
        //using System.Web.Routing is required for this..
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null ||
                            filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                              RouteValueDictionary(new { controller = "User", action = "Login", area = "" }));
                }
            }
        }

        //encode the id value which is display in the details page..
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        //decode the id value which is display in the details page..
        public int Decode(string decodeMe)
        {
            byte[] decoded = Convert.FromBase64String(decodeMe);
            string decodedvalue = System.Text.Encoding.UTF8.GetString(decoded);
            return Convert.ToInt32(decodedvalue);
        }

        //CREATE BARCODE PRINTING
        public ActionResult BarcodePrinting()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ItemCategoryList = _itemcategortservice.GetAllItemCategories();
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            return View(model);
        }

        //CREATE DIRECT PRINTING
        public ActionResult DirectPrinting()
        {
            MainApplication model = new MainApplication();
            model.ItemCategoryList = _itemcategortservice.GetAllItemCategories();
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            return View(model);
        }

        //DISPLAY ITEM DISTAILS USING BARCODE
        [HttpGet]
        public JsonResult GetDetailsByBarcode(string barcode)
        {
            var barcodevalue = barcode.ToUpper() + ".png";
            var data = _itemservice.GetDetailsByBarcode(barcodevalue);
            var barcodeImage = "../../Images/" + data.Barcode;
            return Json(new { data.itemCode, data.itemName, data.designName, data.size, data.mrp, data.sellingprice, data.description, data.colorCode, barcodeImage }, JsonRequestBehavior.AllowGet);
        }

        //GET SUB CATEGORY BY CATEGORY
        [HttpGet]
        public JsonResult LoadSubCatByCat(string cat)
        {
            if (!string.IsNullOrEmpty(cat))
            {
                MainApplication model = new MainApplication();
                var list = _itemsubcategoryservice.GetSubCategoryByCategory(cat);
                model.ItemSubCategoryList = list;
                var data = model.ItemSubCategoryList.Select(s => new SelectListItem()
                {
                    Text = s.subCategoryName,
                    Value = s.subCategoryName
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = string.Empty;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        //GET SUB ITEM BY SUBCATEGORY
        [HttpGet]
        public JsonResult LoadItemBySubCat(string subcat)
        {
            if (!string.IsNullOrEmpty(subcat))
            {
                MainApplication model = new MainApplication();
                var list = _openingstockservice.GetDetailsBySubCategory(subcat);
                var data = list.Select(s => new SelectListItem()
                {
                    Text = s.itemCode + " " + s.ItemName,
                    Value = s.itemCode
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = string.Empty;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        //SHOW ITEM DETAILS ON ITEM NAME DDL
        [HttpGet]
        public ActionResult GetItemDetails(string itemcode)
        {
            var data = _itemservice.GetDescriptionByItemCode(itemcode);
            var barcodeImage = "../../Images/" + data.Barcode;
            var barcode = data.Barcode;
            barcode = barcode.Remove(barcode.Length-4,4);
            return Json(new { data.itemCode, data.itemName, data.description, data.colorCode, data.mrp, data.sellingprice, data.designName, data.size, barcodeImage ,barcode}, JsonRequestBehavior.AllowGet);
        }

        //POPUP PAGE FOR PRINT BARCODE
        [HttpGet]
        public ActionResult PrintBarcode(string code)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.OpeningStockDetails = _openingstockservice.GetDetailsByItemCode(code);
            //model.OpeningStockDetails.Barcode = "../../Images/" + model.OpeningStockDetails.Barcode;
            var barcodeno = _openingstockservice.GetDetailsByItemCode(code).Barcode;
            //barcodeno = barcodeno + ".png";
            model.BarcodeDetails = _BarcodeNumberService.GetBarcodeImageByBarcode(barcodeno);
            //model.BarcodeDetails.BarcodeImage = bytes;
            return View(model);
        }

        //CREATE INWARD PRINTING
        public ActionResult InwardPrinting()
        {
            return View();
        }

        //AUTO COMPLETE RETAIL BILL NO FOR EDIT
        [HttpGet]
        public JsonResult GetInwardNos(string SearchTerm)
        {
            var inwarddata = _InwardFromSupplierService.GetAllInwardNos(SearchTerm);
            List<string> names = new List<string>();
            foreach (var item in inwarddata)
            {
                names.Add(item.InwardNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //SHOW ITEM DETAILS ON INWARD NO
        [HttpGet]
        public ActionResult GetDetailsByInwardNo(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(InwardNo);
            return View(model);
        }

    }
}


