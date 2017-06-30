using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using CodeFirstServices.Interfaces;
using CodeFirstServices.Services;
using MvcRetailApp.ModelBinder;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class DiscountMasterController : Controller
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
        private readonly IUtilityService _UtilityService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;
        private readonly IDiscountMasterService _DiscountMasterService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;
        private readonly IItemCategoryService _ItemCategoryService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IItemService _ItemService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IBrandMasterService _BrandMasterService;

        public DiscountMasterController(IUtilityService UtilityService, IUserCredentialService UserCredentialService, IModuleService ModuleService,
            IDiscountMasterService DiscountMasterService,IDiscountMasterItemService DiscountMasterItemService, IItemCategoryService ItemCategoryService, IItemSubCategoryService ItemSubCategoryService,
            IItemService ItemService, INonInventoryItemService NonInventoryItemService, IBrandMasterService BrandMasterService)
        {

            this._UtilityService = UtilityService;
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
            this._DiscountMasterService = DiscountMasterService;
            this._DiscountMasterItemService = DiscountMasterItemService;
            this._ItemCategoryService=ItemCategoryService;
            this._ItemSubCategoryService=ItemSubCategoryService;
            this._ItemService = ItemService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._BrandMasterService = BrandMasterService;
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

        //CREATE DISCOUNT MASTER
        [HttpGet]
        public ActionResult CreateDiscount()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                DiscountMasterDetails = new DiscountMaster(),
            };

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string DiscCode = string.Empty;
            var details = _DiscountMasterService.GetLastRowrByFinYr(FinYr);
            int discval = 0;
            int length = 0;
            if (details != null)
            {
                DiscCode = details.DiscountCode.Substring(4, 6);
                length = (Convert.ToInt32(DiscCode) + 1).ToString().Length;
                discval = Convert.ToInt32(DiscCode) + 1;
                discval = details.Id;
                discval = discval + 1;
                length = discval.ToString().Length;
            }
            else
            {
                discval = 1;
                length = 1;
            }
            DiscCode = _UtilityService.getName("DISC", length, discval);
            DiscCode = DiscCode + FinYr;
            model.DiscountMasterDetails.DiscountCode = DiscCode;
            TempData["PreviousDiscountCode"] = DiscCode;

            model.BrandMasterList = _BrandMasterService.GetAllBrands();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //CATEGORY NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetCategory(string term)
        {
            var result = _ItemCategoryService.GetItemCategoryList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.CategoryName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //BRAND NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetBrands(string term)
        {
            var result = _BrandMasterService.GetBrandList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.BrandName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //GET SUB CATEGORY BY CATEGORY
        [HttpGet]
        public JsonResult LoadSubCategoryByCategory(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var list = _ItemSubCategoryService.GetSubCategoryByCategory(id);
                MainApplication model = new MainApplication();
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

        //GET ITEM DETAILS FOR DSICOUNT
        [HttpGet]
        public ActionResult GetItemDetails()
        {
            MainApplication model = new MainApplication();
            model.ItemList = _ItemService.getAllItems();
            model.NonInventoryItemList = _NonInventoryItemService.GetAll();
            return View(model);
        }

        //GET ITEM DETAILS BY CATEGORY
        [HttpGet]
        public ActionResult GetItemDetailsByCategory(string category)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _ItemService.GetItemsByCategory(category);
            model.NonInventoryItemList = _NonInventoryItemService.GetItemsByCategory(category);
            return View(model);
        }

        //GET ITEM DETAILS BY SUBCATEGORY
        [HttpGet]
        public ActionResult GetItemDetailsBySubcategory(string subcategory)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _ItemService.GetItemsBySubCategory(subcategory);
            model.NonInventoryItemList = _NonInventoryItemService.GetItemsBySubcategory(subcategory);
            return View(model);
        }

        //GET ITEM DETAILS BY CATEGORY
        [HttpGet]
        public ActionResult GetItemDetailsByBrand(string brand)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _ItemService.GetItemsByBrand(brand);
            model.NonInventoryItemList = _NonInventoryItemService.GetItemsByBrand(brand);
            return View(model);
        }

        //GET DISCOUNT ITEM
        [HttpGet]
        public JsonResult GetDiscountItem(string itemcode, DateTime fromdate)
        {
            var FromDate = fromdate;
            var data = _DiscountMasterItemService.GetDetailsByItemCodeAndFromDate(itemcode, FromDate);
            if (data != null)
            {
                string msg = "ItemPresent";
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "ItemAbsent";
                return Json(new { msg }, JsonRequestBehavior.AllowGet);
            }
        }

        //SAVE DISCOUNT MASTER
        [HttpPost]
        public ActionResult CreateDiscount(MainApplication mainapp, FormCollection frmcol)
        {
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string DiscCode = string.Empty;
            var details = _DiscountMasterService.GetLastRowrByFinYr(FinYr);
            int discval = 0;
            int length = 0;
            if (details != null)
            {
                DiscCode = details.DiscountCode.Substring(4, 6);
                length = (Convert.ToInt32(DiscCode) + 1).ToString().Length;
                discval = Convert.ToInt32(DiscCode) + 1;
                discval = details.Id;
                discval = discval + 1;
                length = discval.ToString().Length;
            }
            else
            {
                discval = 1;
                length = 1;
            }
            DiscCode = _UtilityService.getName("DISC", length, discval);
            DiscCode = DiscCode + FinYr;
            mainapp.DiscountMasterDetails.DiscountCode = DiscCode;

            //save main discount master
            mainapp.DiscountMasterDetails.Status = "Active";
            mainapp.DiscountMasterDetails.ModifiedOn = DateTime.Now;
            _DiscountMasterService.Create(mainapp.DiscountMasterDetails);

            //save outward to tailor items..
            int itemcount = Convert.ToInt32(frmcol["ItemList"]);
            if (itemcount != 0)
            {
                for (int i = 1; i < itemcount; i++)
                {
                    string checkbox = "CheckBox" + i;
                    string itemcode = "ItemCode" + i;
                    string itemname = "ItemName" + i;
                    string discinper = "DiscInPer" + i;
                    string discinrs = "DiscInRs" + i;
                    string unit = "Unit" + i;
                    string size = "Size" + i;
                    string designname = "DesignnName" + i;
                    string costprice = "CostPrice" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;

                    if (frmcol[checkbox] == "Yes")
                    {
                        mainapp.DiscountMasterItemDetails.DiscountCode = mainapp.DiscountMasterDetails.DiscountCode;
                        mainapp.DiscountMasterItemDetails.FromDate = mainapp.DiscountMasterDetails.FromDate;
                        mainapp.DiscountMasterItemDetails.ToDate = mainapp.DiscountMasterDetails.ToDate;
                        mainapp.DiscountMasterItemDetails.ItemCode = frmcol[itemcode];
                        mainapp.DiscountMasterItemDetails.ItemName = frmcol[itemname];
                        mainapp.DiscountMasterItemDetails.Size = frmcol[size];
                        mainapp.DiscountMasterItemDetails.Unit = frmcol[unit];
                        mainapp.DiscountMasterItemDetails.Design = frmcol[designname];
                        mainapp.DiscountMasterItemDetails.CostPrice = Convert.ToDouble(frmcol[costprice]);
                        if (frmcol[sellingprice] != "")
                        {
                            mainapp.DiscountMasterItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        }
                        else
                        {
                            mainapp.DiscountMasterItemDetails.SellingPrice = null;
                        }
                        if (frmcol[mrp] != "")
                        {
                            mainapp.DiscountMasterItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            mainapp.DiscountMasterItemDetails.MRP = null;
                        }
                        mainapp.DiscountMasterItemDetails.DiscInPercentage = Convert.ToDouble(frmcol[discinper]);
                        mainapp.DiscountMasterItemDetails.DiscInRupees = Convert.ToDouble(frmcol[discinrs]);
                        mainapp.DiscountMasterItemDetails.Status = "Active";
                        mainapp.DiscountMasterItemDetails.ModifiedOn = DateTime.Now;

                        if (frmcol[itemcode].Contains("NI"))
                        {
                            var noninvitemdetails = _NonInventoryItemService.GetDetailsByItemCode(mainapp.DiscountMasterItemDetails.ItemCode);
                            mainapp.DiscountMasterItemDetails.Color = noninvitemdetails.colorCode;
                            mainapp.DiscountMasterItemDetails.ItemCategory = noninvitemdetails.itemCategory;
                            mainapp.DiscountMasterItemDetails.ItemSubCategory = noninvitemdetails.itemSubCategory;
                        }
                        else
                        {
                            var itemdetails = _ItemService.GetDescriptionByItemCode(mainapp.DiscountMasterItemDetails.ItemCode);
                            mainapp.DiscountMasterItemDetails.Color = itemdetails.colorCode;
                            mainapp.DiscountMasterItemDetails.Barcode = itemdetails.Barcode;
                            mainapp.DiscountMasterItemDetails.ItemCategory = itemdetails.itemCategory;
                            mainapp.DiscountMasterItemDetails.ItemSubCategory = itemdetails.itemSubCategory;
                        }
                        
                        _DiscountMasterItemService.Create(mainapp.DiscountMasterItemDetails);
                    }
                }
            }

            var DiscId = _DiscountMasterService.getLastRow().Id;
            var DiscountId = Encode(DiscId.ToString());
            return RedirectToAction("CreateDiscountDetails/" + DiscountId, "DiscountMaster");
        }

        //DETAILS PAGE OF DISCOUNT CREATE
        [HttpGet]
        public ActionResult CreateDiscountDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.DiscountMasterDetails = _DiscountMasterService.GetDetailsById(Id);
            model.DiscountMasterItemList = _DiscountMasterItemService.GetRowsByCode(model.DiscountMasterDetails.DiscountCode);

            string previousdisc = TempData["PreviousDiscountCode"].ToString();
            if (TempData["PreviousDiscountCode"].ToString() != model.DiscountMasterDetails.DiscountCode)
            {
                ViewData["DiscountChanged"] = previousdisc + " is replaced with " + model.DiscountMasterDetails.DiscountCode + " because " + previousdisc + " is acquired by another person";
            }
            TempData["PreviousDiscountCode"] = previousdisc;
            return View(model);
        }

    }
}