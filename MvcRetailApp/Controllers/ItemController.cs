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
    //[NoDirectAccess]
    [SessionExpireFilter]
    public class ItemController : Controller
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
        private readonly IUtilityService _utilityService;
        private readonly IItemService _itemService;
        private readonly INonInventoryItemService _NonInventoryService;
        private readonly IGenerateItemCodeService _GenerateItemCodeService;
        private readonly IItemCategoryService _ItemCategoryService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IItemTempDetailService _ItemTempDetailService;
        private readonly ITypeOfMaterialService _TypeOfMaterialService;
        private readonly IColorCodeService _ColorCodeService;
        private readonly IDesignService _DesignService;
        private readonly IBarcodeNumberService _BarcodeNumberService;
        private readonly IBarcodeTempDetailService _BarcodeTempDetailService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IUnitService _UnitService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly IBrandMasterService _BrandMasterService;
        private readonly IItemNameService _ItemNameService;

        public ItemController(IItemService itemService, INonInventoryItemService NonInventoryService, IUtilityService utilityService, IGenerateItemCodeService GenerateItemCodeService, IItemCategoryService ItemCategoryService,
                              IItemSubCategoryService ItemSubCategoryService, IItemTempDetailService ItemTempDetailService, ITypeOfMaterialService TypeOfMaterialService,
                              ColorCodeService ColorCodeService, IDesignService DesignService, IBarcodeNumberService BarcodeNumberService, IBarcodeTempDetailService BarcodeTempDetailService, IUserCredentialService usercredentialservice,
                              IModuleService iIModuleService, IUnitService UnitService, IOpeningStockService OpeningStockService, IEntryStockItemService EntryStockItemService, IInwardItemFromSupplierService InwardItemFromSupplierService,
                              IItemTaxService ItemTaxService, IBrandMasterService BrandMasterService, IItemNameService ItemNameService)
        {
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._OpeningStockService = OpeningStockService;
            this._EntryStockItemService = EntryStockItemService;
            this._itemService = itemService;
            this._NonInventoryService = NonInventoryService;
            this._utilityService = utilityService;
            this._GenerateItemCodeService = GenerateItemCodeService;
            this._ItemCategoryService = ItemCategoryService;
            this._ItemSubCategoryService = ItemSubCategoryService;
            this._ItemTempDetailService = ItemTempDetailService;
            this._TypeOfMaterialService = TypeOfMaterialService;
            this._ColorCodeService = ColorCodeService;
            this._DesignService = DesignService;
            this._BarcodeNumberService = BarcodeNumberService;
            this._BarcodeTempDetailService = BarcodeTempDetailService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._UnitService = UnitService;
            this._ItemTaxService = ItemTaxService;
            this._BrandMasterService = BrandMasterService;
            this._ItemNameService = ItemNameService;
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

        // CREATE ITEM CATEGORY
        public ActionResult CreateCategory()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                ItemCategoryDetails = new ItemCategory(),
            };

            var details = _ItemCategoryService.getLastItemCategory();
            int Catval = 0;
            int length = 0;
            if (details != null)
            {
                Catval = details.CategoryId;
                Catval = Catval + 1;
                length = Catval.ToString().Length;
            }
            else
            {
                Catval = 1;
                length = 1;
            }
            String itemcode = _utilityService.getName("IC", length, Catval);
            model.ItemCategoryDetails.ItemCategoryCode = itemcode;

            if (TempData["CategoryList"] != null)
            {
                ViewBag.error = "Name Already Present";
            }
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
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

        [HttpPost]
        public ActionResult CreateCategory([ModelBinder(typeof(ItemCategoryCustomBinder))]ItemCategory itemCategory)
        {
            var details = _ItemCategoryService.getLastItemCategory();
            int Catval = 0;
            int length = 0;
            if (details != null)
            {
                Catval = details.CategoryId;
                Catval = Catval + 1;
                length = Catval.ToString().Length;
            }
            else
            {
                Catval = 1;
                length = 1;
            }
            String itemcode = _utilityService.getName("IC", length, Catval);
            var namelist = _ItemCategoryService.GetCategoryName(itemCategory.CategoryName);
            if (namelist.Count() != 0)
            {
                TempData["CategoryList"] = "error";
                return RedirectToAction("CreateCategory");
            }
            else
            {
                _ItemCategoryService.CreateItemCatgory(itemCategory);
            }

            var id = _ItemCategoryService.getLastItemCategory().CategoryId;
            var CategoryId = Encode(id.ToString());
            return RedirectToAction("CreateCategoryDetails/" + CategoryId, "Item");
        }

        //SHOW CATEGORY DETAILS AFTER CREATE
        [HttpGet]
        public ActionResult CreateCategoryDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.ItemCategoryDetails = _ItemCategoryService.getById(Id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        // EDIT ITEM CATEGORY
        [HttpGet]
        public ActionResult EditCategory()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Edit";
            return View(model);
        }

        //LOAD CATEGORY LIST ONLOAD FUNCTION
        [HttpGet]
        public ActionResult LoadCategories()
        {
            MainApplication model = new MainApplication();
            model.ItemCategoryList = _ItemCategoryService.GetItemCategories();
            TempData["CatList"] = model.ItemCategoryList;
            return View(model);
        }

        //SEARCH CATEGORIES LIST
        [HttpGet]
        public ActionResult CategoryList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<ItemCategory> ListOfCategory = TempData["CatList"] as IEnumerable<ItemCategory>;
            ListOfCategory = ListOfCategory.Where(x => x.itemCategoryStatus == "Active");
            List<ItemCategory> FinalList = new List<ItemCategory>();
            FinalList = FinalList.Concat(ListOfCategory.Where(x => x.CategoryName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ItemCategoryList = FinalList;
            TempData["CatList"] = FinalList;
            return View(model);
        }

        //PARTIAL VIEW FOR EDIT ITEM CATEGORIES
        [HttpGet]
        public ActionResult CategoryEditPartial(int id1)
        {
            MainApplication model = new MainApplication();
            model.ItemCategoryDetails = _ItemCategoryService.GetId(id1);
            return View(model);
        }

        [HttpPost]
        public ActionResult CategoryEditPartial(ItemCategory itmcategory)
        {
            itmcategory.itemCategoryStatus = "Active";
            itmcategory.ModifiedOn = DateTime.Now;
            _ItemCategoryService.UpdateItemCatgory(itmcategory);
            return RedirectToAction("CategoryUpdateDetails/" + itmcategory.CategoryId, "Item");
        }

        [HttpGet]
        public ActionResult DeleteCategory()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        //PARTIAL VIEW FOR DELETE ITEM CATEGORIES
        [HttpGet]
        public ActionResult CategoryDeletePartial(int id2)
        {
            MainApplication model = new MainApplication();
            model.ItemCategoryDetails = _ItemCategoryService.GetId(id2);
            return View(model);
        }

        [HttpPost]
        public ActionResult CategoryDeletePartial(ItemCategory itmcategory)
        {
            itmcategory.itemCategoryStatus = "InActive";
            itmcategory.ModifiedOn = DateTime.Now;
            _ItemCategoryService.UpdateItemCatgory(itmcategory);
            return RedirectToAction("CategoryUpdateDetails/" + itmcategory.CategoryId, "Item");
        }

        //SHOW CATEGORY DETAILS AFTER EDITDELETE
        [HttpGet]
        public ActionResult CategoryUpdateDetails(int id)
        {
            ItemCategory itmcat = new ItemCategory();
            TempData["CatList"] = _ItemCategoryService.GetAllItemCategories();
            itmcat = _ItemCategoryService.getById(id);
            return View(itmcat);
        }


        //******** START SUB CATEGORY *********



        //CREATE ITEM SUB CATEGORY
        [HttpGet]
        public ActionResult CreateSubCategory()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            IEnumerable<ItemCategory> itemCatList = _ItemCategoryService.GetItemCategories();
            MainApplication model = new MainApplication()
            {
                ItemSubCategoryDetails = new ItemSubCategory(),
            };

            var details = _ItemSubCategoryService.getLastItemSubCategory();
            int Catval = 0;
            int length = 0;
            if (details != null)
            {
                Catval = details.subCategoryId;
                Catval = Catval + 1;
                length = Catval.ToString().Length;
            }
            else
            {
                Catval = 1;
                length = 1;
            }
            string itemsubcode = _utilityService.getName("ISC", length, Catval);
            model.ItemSubCategoryDetails.ItemSubCategoryCode = itemsubcode;
            model.ItemCategoryList = itemCatList;

            if (TempData["SubCategoryList"] != null)
            {
                ViewBag.error = "This SubCategoryName is Already Present";
            }
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //GET SUB CATEGORY LIST BY CATEGORY
        [HttpGet]
        public JsonResult GetSubCategoryList(string term)
        {
            var result = _ItemSubCategoryService.GetItemSubCategoryList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.subCategoryName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CreateSubCategory([ModelBinder(typeof(ItemSubCategoryCustomBinder))]ItemSubCategory itemsubcategory)
        {
            var details = _ItemSubCategoryService.getLastItemSubCategory();
            int Catval = 0;
            int length = 0;
            if (details != null)
            {
                Catval = details.subCategoryId;
                Catval = Catval + 1;
                length = Catval.ToString().Length;
            }
            else
            {
                Catval = 1;
                length = 1;
            }
            string itemsubcode = _utilityService.getName("ISC", length, Catval);
            string cat = itemsubcategory.categoryName;
            string subcat = itemsubcategory.subCategoryName;
            var namelist = _ItemSubCategoryService.GetSubCategoryName(cat, subcat);
            if (namelist.Count() != 0)
            {
                TempData["SubCategoryList"] = "error";
                return RedirectToAction("CreateSubCategory");
            }
            else
            {
                _ItemSubCategoryService.CreateItemCatgory(itemsubcategory);
            }
            var id = _ItemSubCategoryService.getLastItemSubCategory().subCategoryId;
            var SubcategoryId = Encode(id.ToString());
            return RedirectToAction("CreateSubCategoryDetails/" + SubcategoryId, "Item");
        }


        //SHOW SUBCATEGORY CREATE DETAILS
        [HttpGet]
        public ActionResult CreateSubCategoryDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.ItemSubCategoryDetails = _ItemSubCategoryService.GetSubCAtegorybyId(Id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }


        //EDIT ITEM SUB CATEGORY
        [HttpGet]
        public ActionResult EditSubCategory()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            IEnumerable<ItemCategory> itemCatList = _ItemCategoryService.GetItemCategories();
            IEnumerable<ItemSubCategory> itemSubCat = _ItemSubCategoryService.getSubCategory();
            MainApplication model = new MainApplication();
            model.ItemCategoryList = itemCatList;
            model.ItemSubCategoryList = itemSubCat;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Edit";
            return View(model);
        }


        [HttpGet]
        public ActionResult LoadSubCategories(string name)
        {
            MainApplication model = new MainApplication()
            {
                ItemSubCategoryDetails = new ItemSubCategory(),
            };
            model.ItemSubCategoryList = _ItemSubCategoryService.GetSubCategoryByCategory(name);
            TempData["SubCatList"] = model.ItemSubCategoryList;
            return View(model);
        }

        //SEARCH SUB CATEGORIES LIST
        [HttpGet]
        public ActionResult SubCategoryList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<ItemSubCategory> ListOfSubCategory = TempData["SubCatList"] as IEnumerable<ItemSubCategory>;
            ListOfSubCategory = ListOfSubCategory.Where(x => x.status == "Active");
            List<ItemSubCategory> FinalList = new List<ItemSubCategory>();
            FinalList = FinalList.Concat(ListOfSubCategory.Where(x => x.subCategoryName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ItemSubCategoryList = FinalList;
            TempData["SubCatList"] = FinalList;
            return View(model);
        }

        ///PARTIAL VIEW FOR EDIT ITEM SUB CATEGORY
        [HttpGet]
        public ActionResult SubCategoryEditPartial(int ID)
        {
            MainApplication model = new MainApplication();
            model.ItemSubCategoryDetails = _ItemSubCategoryService.GetId(ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult SubCategoryEditPartial(ItemSubCategory itmsubcategory)
        {
            itmsubcategory.status = "Active";
            itmsubcategory.ModifiedOn = DateTime.Now;
            _ItemSubCategoryService.UpdateItemCatgory(itmsubcategory);
            return RedirectToAction("SubCategoryUpdateDetails/" + itmsubcategory.subCategoryId, "Item");
        }

        [HttpGet]
        public ActionResult DeleteSubCategory()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            IEnumerable<ItemCategory> itemCatList = _ItemCategoryService.GetItemCategories();
            IEnumerable<ItemSubCategory> itemSubCat = _ItemSubCategoryService.getSubCategory();
            MainApplication model = new MainApplication();
            model.ItemCategoryList = itemCatList;
            model.ItemSubCategoryList = itemSubCat;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        ///PARTIAL VIEW FOR DELETE ITEM SUB CATEGORY
        [HttpGet]
        public ActionResult SubCategoryDeletePartial(int ID)
        {
            MainApplication model = new MainApplication();
            model.ItemSubCategoryDetails = _ItemSubCategoryService.GetId(ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult SubCategoryDeletePartial(ItemSubCategory itmsubcategory)
        {
            itmsubcategory.status = "InActive";
            itmsubcategory.ModifiedOn = DateTime.Now;
            _ItemSubCategoryService.UpdateItemCatgory(itmsubcategory);
            return RedirectToAction("SubCategoryUpdateDetails/" + itmsubcategory.subCategoryId, "Item");
        }

        ///SHOW SUBCATEGORY DETAILS AFTER EDITDELETE
        [HttpGet]
        public ActionResult SubCategoryUpdateDetails(int id)
        {
            ItemSubCategory itmsubcat = new ItemSubCategory();
            TempData["SubCatList"] = _ItemSubCategoryService.getSubCategory();
            itmsubcat = _ItemSubCategoryService.GetSubCAtegorybyId(id);
            return View(itmsubcat);
        }



        //******************************** START ITEM *************************************

        [HttpGet]
        public ActionResult CreateItem()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.ItemSubCategoryList = _ItemSubCategoryService.GetItemSubCategories();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //GET SUB CATEGORY TAX
        //[HttpGet]
        //public JsonResult GetItemTypeByItemName(string item)
        //{
        //    var data = _ItemNameService.GetDataByItemName(item);
        //    return Json(new { data.ItemType }, JsonRequestBehavior.AllowGet);
        //}

        //GET SUB CATEGORY TAX
        [HttpGet]
        public JsonResult GetSubCatTax(string SubCategory)
        {
            double Percentage = 0;
            var taxdetails = _ItemTaxService.GetTaxBySubCatAndVAT(SubCategory);
            if (taxdetails != null)
            {
                Percentage = taxdetails.Percentage;
            }
            return Json(new { Percentage }, JsonRequestBehavior.AllowGet);
        }

        //LOAD ITEM LIST
        [HttpGet]
        public JsonResult LoadItemList(string term)
        {
            var itemname = _ItemNameService.GetitemName(term);
            List<string> item = new List<string>();
            foreach (var items in itemname)
            {
                item.Add(items.Name);
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        //Load Design List
        public JsonResult LoadDesignList(string term)
        {
            var designname = _DesignService.GetDesignName(term);
            List<string> design = new List<string>();
            foreach (var designs in designname)
            {
                design.Add(designs.DesignName);
            }
            return Json(design, JsonRequestBehavior.AllowGet);
        }

        //LOAD UNIT LIST
        [HttpGet]
        public JsonResult LoadUnitList()
        {
            MainApplication model = new MainApplication();
            model.UnitList = _UnitService.getallsize();
            var data = model.UnitList.Select(s => new SelectListItem()
            {
                Text = s.UnitName,
                Value = s.UnitName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD BRAND LIST
        [HttpGet]
        public JsonResult LoadBrandList()
        {
            MainApplication model = new MainApplication();
            model.BrandMasterList = _BrandMasterService.GetAllBrands();
            var data = model.BrandMasterList.Select(s => new SelectListItem()
            {
                Text = s.BrandName,
                Value = s.BrandName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintItem(string Category, string SubCategory)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            var ItemList = _itemService.GetItemByCategory(Category, SubCategory);
            List<Item> itemTempList = new List<Item>();
            foreach (var item in ItemList)
            {
                Item tempData = new Item();
                tempData.itemName = item.itemName;
                tempData.description = item.description;
                tempData.costprice = item.costprice;
                //tempData.barcode = "../../Images/" + item.barcode;
                tempData.itemId = item.itemId;
                var colorData = _ColorCodeService.getColorNameByCode(item.colorCode);
                tempData.colorCode = colorData.colorName;
                var typeData = _TypeOfMaterialService.GetTypeOfMaterialByCode(item.typeOfMaterial);
                tempData.typeOfMaterial = typeData.MaterialName;
                tempData.modifedOn = item.modifedOn;
                itemTempList.Add(tempData);
            }
            model.ItemList = itemTempList as IEnumerable<Item>;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        ////CREATE ITEM CODE
        //[HttpGet]
        //public JsonResult CreateItemCode()
        //{
        //    //CREATE ITEM CODE
        //    var itemdata = _itemService.GetLastItem();
        //    int itemval = 0;
        //    int lenght = 0;
        //    if (itemdata != null)
        //    {
        //        itemval = itemdata.itemId;
        //        itemval = itemval + 1;
        //        lenght = itemval.ToString().Length;
        //    }
        //    else
        //    {
        //        itemval = 1;
        //        lenght = 1;
        //    }
        //    string itemcode = _utilityService.getName("I", lenght, itemval);
        //    return Json(itemcode, JsonRequestBehavior.AllowGet);
        //}

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

        //GET SUB CATEGORY BY CATEGORY
        [HttpGet]
        public JsonResult LoadItemBySubCategory(string SubCat)
        {
            if (!string.IsNullOrEmpty(SubCat))
            {
                var list = _ItemNameService.GetItemBySubCategory(SubCat);
                MainApplication model = new MainApplication();
                model.ItemNameList = list;
                var data = model.ItemNameList.Select(s => new SelectListItem()
                {
                    Text = s.Name,
                    Value = s.Name
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = string.Empty;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        //LOAD TYPE TYPE OF MATERIAL
        [HttpGet]
        public JsonResult LoadTypeOfMaterial()
        {
            MainApplication model = new MainApplication();
            model.typeOfMaterialList = _TypeOfMaterialService.getAllTypeOfMaterial();
            var data = model.typeOfMaterialList.Select(s => new SelectListItem()
            {
                Text = s.MaterialName,
                Value = s.MaterialName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD COLOR CODE
        [HttpGet]
        public JsonResult LoadColorCode()
        {
            MainApplication model = new MainApplication();
            model.ColorCodeList = _ColorCodeService.getAllColorCode();
            var data = model.ColorCodeList.Select(s => new SelectListItem()
            {
                Text = s.colorName,
                Value = s.colorName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD DESIGN CODE
        [HttpGet]
        public JsonResult LoadDesignCode(string type)
        {
            IEnumerable<DesignMaster> dm = _DesignService.GetDesignNamesBySubCat(type);
            var data = dm.Select(m => new SelectListItem()
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GET DESIGN DETAILS
        [HttpGet]
        public JsonResult GetDesignDetails(string code)
        {
            string data = _DesignService.getNameByCode(code);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GET ALL ITEM IN DATABASE
        [HttpGet]
        public JsonResult GetAllItemDetails(string Material, string Color, string Costprice, string Brand, string Size, string Design, string Unit, string SubCat)
        {
            int counter = 0;
            string Cat = _ItemSubCategoryService.GetCategoryBySubCat(SubCat);
            var items = _itemService.GetAllItems();
            foreach (var row1 in items)
            {
                if (row1.typeOfMaterial == Material && row1.colorCode == Color && row1.costprice == Costprice && row1.brandName == Brand &&
                    row1.size == Size && row1.designCode == Design && row1.unit == Unit && row1.itemCategory == Cat && row1.itemSubCategory == SubCat)
                {
                    counter = 1;
                    break;
                }
            }
            if (counter == 0)
            {
                var noninvitems = _NonInventoryService.GetAll();
                foreach (var row2 in noninvitems)
                {
                    if (row2.typeOfMaterial == Material && row2.colorCode == Color && row2.costprice == Costprice && row2.brandName == Brand &&
                        row2.size == Size && row2.designCode == Design && row2.unit == Unit && row2.itemCategory == Cat && row2.itemSubCategory == SubCat)
                    {
                        counter = 1;
                        break;
                    }
                }
            }
            if (counter == 0)
            {
                return Json("Absent", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Present", JsonRequestBehavior.AllowGet);
            }
        }

        //SAVE ITEM MASTER
        [HttpPost]
        public ActionResult CreateItem(MainApplication mainApplication, FormCollection frmItem)
        {
            MainApplication model = new MainApplication()
            {
                ItemDetails = new Item(),
                NonInventoryItemDetails = new NonInventoryItem(),
                MaterialDetails = new TypeOfMaterial(),
                DesignDetails = new DesignMaster(),
                BrandMasterDetails = new BrandMaster(),
                ColorCodeDetails = new ColorCode(),
                ItemNameDetails = new ItemName()
            };

            int lastitembefore = 0;
            var data = _itemService.GetLastItem();
            if (data == null)
            {
                lastitembefore = 1;
            }
            else
            {
                lastitembefore = data.itemId + 1;
            }

            int lastnoninventoryitembefore = 0;
            var noninventorydata = _NonInventoryService.GetLastItem();
            if (noninventorydata == null)
            {
                lastnoninventoryitembefore = 1;
            }
            else
            {
                lastnoninventoryitembefore = noninventorydata.itemId + 1;
            }

            string Name = frmItem["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(Name))
            {
                int count = Convert.ToInt32(Name);
                string itemname1 = string.Empty;
                for (int i = 1; i <= count; i++)
                {
                    string itemdescription = "itemDescription" + i;
                    string costprice = "CostPrice" + i;
                    string materialType = "materialType" + i;
                    string colorCode = "colorCodeData" + i;
                    string designCode = "designCodeData" + i;
                    string designname = "designName" + i;
                    string size = "Size" + i;
                    string unit = "Unit" + i;
                    string sellingprice = "SellingPrice" + i;
                    string brand = "Brand" + i;
                    string numbertype = "NumberType" + i;
                    string mrp = "MRP" + i;

                    string finalitemdescription = frmItem[itemdescription];
                    string finalmrp = frmItem[mrp];
                    string finalcostprice = frmItem[costprice];
                    string finalMaterialType = frmItem[materialType];
                    string finalColorCode = frmItem[colorCode];
                    string finalDesignCode = frmItem[designCode];
                    string finalDesignName = frmItem[designname];
                    string finalSize = frmItem[size];
                    string finalUnit = frmItem[unit];
                    string finalnumbertype = frmItem[numbertype];
                    string finalsellingprice = frmItem[sellingprice];
                    string finalbrand = frmItem[brand];

                    var finalType = "Inventory";
                    
                    //IF ITEM TYPE IS INVENTORY THEN ITEM STORED IN ITEM MASTER
                    if (finalType == "Inventory")
                    {
                        
                            //CREATE ITEM CODE need to validate item master entry
                            string itemcode;
                            var itemName = mainApplication.ItemDetails.itemName;
                            var itemmastername = _itemService.GetItemName(itemName);
                            var itemmastercolor = _ColorCodeService.getColorName(finalColorCode);
                            var itemcolor = _itemService.GetcolorCodebyColorcode(itemmastercolor);
                            
                            if (itemmastername == null)
                            {
                                //if (itemcolor == null)
                                //{

                                    //Item Name Entry 
                                    var itemdetails = _ItemNameService.GetItemIdByName(itemName);
                                    if (itemdetails == null)
                                    {
                                        model.ItemNameDetails.ItemCategory = _ItemSubCategoryService.GetCategoryBySubCat(mainApplication.ItemDetails.itemSubCategory);
                                        model.ItemNameDetails.ItemSubcategory = mainApplication.ItemDetails.itemSubCategory;
                                        model.ItemNameDetails.Name = mainApplication.ItemDetails.itemName;
                                        model.ItemNameDetails.ItemType = finalType;
                                        model.ItemNameDetails.DeleteStatus = "Active";
                                        model.ItemNameDetails.Status = "Active";                                        
                                    }

                                    //Type Of Material Masters Entry
                                    var materialname = frmItem[materialType];
                                    var matdetails = _TypeOfMaterialService.GetMaterialName(materialname);
                                    if (matdetails == null)
                                    {
                                        String MaterialCode;

                                        var mardetails = _TypeOfMaterialService.GetMaterialLast();
                                        int matval = 0;
                                        int length = 0;
                                        if (mardetails != null)
                                        {
                                            matval = mardetails.MaterialId;
                                            matval = matval + 1;
                                            length = matval.ToString().Length;
                                            MaterialCode = _utilityService.getName("TM", length, matval);
                                            model.MaterialDetails.MaterialCode = MaterialCode;
                                            model.MaterialDetails.MaterialName = finalMaterialType;
                                            model.MaterialDetails.MaterialShortName = finalMaterialType;
                                            model.MaterialDetails.MaterialDescription = finalMaterialType;
                                            model.MaterialDetails.Status = "Active";
                                        }
                                        else
                                        {
                                            matval = 1;
                                            length = 1;
                                            MaterialCode = _utilityService.getName("TM", length, matval);
                                            model.MaterialDetails.MaterialCode = MaterialCode;
                                            model.MaterialDetails.MaterialName = finalMaterialType;
                                            model.MaterialDetails.MaterialShortName = finalMaterialType;
                                            model.MaterialDetails.MaterialDescription = finalMaterialType;
                                            model.MaterialDetails.Status = "Active";
                                            
                                        }
                                    }


                                    //Color Code Master Entries                                    
                                    var colorname = finalColorCode;
                                    
                                    var colordetails = _ColorCodeService.getColorName(colorname);
                                    var colorcode = _ColorCodeService.GetcolorcodebyName(colorname).colorCode;
                                    if (colordetails == null)
                                    {
                                        var coldetails = _ColorCodeService.GetLastColor();
                                        if (coldetails == null)
                                        {
                                            model.ColorCodeDetails.colorCode = "01";
                                        }
                                        else
                                        {
                                            if (coldetails.colorCode.StartsWith("0"))
                                            {
                                                coldetails.colorCode.Substring(1);
                                                model.ColorCodeDetails.colorCode = (Convert.ToInt32(coldetails.colorCode) + 1).ToString();
                                                model.ColorCodeDetails.colorCode = "0" + model.ColorCodeDetails.colorCode;
                                            }
                                            else
                                            {
                                                var coldetailss = (Convert.ToInt32(coldetails.colorCode) + 1).ToString();
                                                model.ColorCodeDetails.colorCode = coldetailss;
                                                model.ColorCodeDetails.colorName = finalColorCode;
                                                model.ColorCodeDetails.Status = "Active";                                                
                                            }

                                        }
                                        
                                    }

                                    //Design Masters Entries
                                    var designnme = finalDesignCode;
                                    var designdetails = _DesignService.GetNamebyDesignName(designnme);
                                    if (designdetails == null)
                                    {
                                        string dcode = generateCode();
                                        int counts = _DesignService.getDesignsByDesignCode(dcode);
                                        if (counts == 0)
                                        {
                                            model.DesignDetails.DesignCode = dcode;
                                            model.DesignDetails.SubCategoryName = mainApplication.ItemDetails.itemSubCategory;
                                            model.DesignDetails.DesignName = finalDesignCode;
                                            model.DesignDetails.Status = "Active";                                            
                                        }
                                        else
                                        {
                                            model.DesignDetails.DesignCode = dcode;
                                            var subcatname = mainApplication.ItemDetails.itemCategory;
                                            var catcode = _ItemCategoryService.GetCodeByName(subcatname);
                                            model.DesignDetails.CategoryCode = catcode;
                                            model.DesignDetails.SubCategoryName = mainApplication.ItemDetails.itemSubCategory;
                                            model.DesignDetails.DesignName = finalDesignCode;
                                            model.DesignDetails.Status = "Active";                                             
                                        }
                                                                               
                                    }

                                    //Brand Masters Entry
                                    String code;
                                    var brandname = finalbrand;
                                    var branddetails = _BrandMasterService.brandnamebyname(brandname);
                                    var details = _BrandMasterService.getLastBrand();
                                    if (branddetails == null)
                                    {
                                        int Brandval = 0;
                                        int Brandlength = 0;
                                        if (details != null)
                                        {
                                            Brandval = details.Id;
                                            Brandval = Brandval + 1;
                                            Brandlength = Brandval.ToString().Length;
                                            code = _utilityService.getName("BRD", Brandlength, Brandval);
                                            model.BrandMasterDetails.BrandCode = code;
                                            model.BrandMasterDetails.BrandName = finalbrand;
                                            model.BrandMasterDetails.Status = "Active";
                                        }
                                        else
                                        {
                                            Brandval = 1;
                                            Brandlength = 1;
                                            code = _utilityService.getName("BRD", Brandlength, Brandval);
                                            model.BrandMasterDetails.BrandCode = code;
                                            model.BrandMasterDetails.BrandName = finalbrand;
                                            model.BrandMasterDetails.Status = "Active";                                            
                                        }
                                        
                                    }

                                    var itemdata = _itemService.GetLastItem();
                                    int itemval = 0;
                                    int lenght = 0;
                                    if (itemdata != null)
                                    {
                                        itemval = itemdata.itemId;
                                        itemval = itemval + 1;
                                        lenght = itemval.ToString().Length;
                                    }
                                    else
                                    {
                                        itemval = 1;
                                        lenght = 1;
                                    }
                                    itemcode = _utilityService.getName("I", lenght, itemval);


                                    //if (!string.IsNullOrEmpty(finalcostprice))

                                    model.ItemDetails.description = finalitemdescription;
                                    model.ItemDetails.modifedOn = DateTime.Now;
                                    model.ItemDetails.status = "Active";
                                    model.ItemDetails.itemSubCategory = mainApplication.ItemDetails.itemSubCategory;
                                    model.ItemDetails.itemCategory = _ItemSubCategoryService.GetCategoryBySubCat(mainApplication.ItemDetails.itemSubCategory);
                                    model.ItemDetails.itemName = mainApplication.ItemDetails.itemName;
                                    model.ItemDetails.costprice = finalcostprice;
                                    model.ItemDetails.typeOfMaterial = finalMaterialType;
                                    if (model.ColorCodeDetails.colorCode == null)
                                    {
                                        model.ItemDetails.colorCode = finalColorCode;
                                    }
                                    else
                                    {
                                        model.ItemDetails.colorCode = model.ColorCodeDetails.colorCode;
                                    }
                                    //model.ItemDetails.designCode = model.DesignDetails.DesignCode;
                                    var designdet = _DesignService.GetNamebyDesignName(finalDesignCode);
                                    if (designdet != null)
                                        model.ItemDetails.designCode = designdet.DesignCode;
                                    model.ItemDetails.designName = finalDesignCode;
                                    model.ItemDetails.size = finalSize;
                                    model.ItemDetails.unit = finalUnit;
                                    var unitdetails = _UnitService.GetDetailsByName(model.ItemDetails.unit);
                                    if(unitdetails != null)
                                        model.ItemDetails.NumberType = unitdetails.NumberType;
                                    model.ItemDetails.itemtype = finalType;
                                    model.ItemDetails.sellingprice = finalsellingprice;
                                    model.ItemDetails.brandName = finalbrand;
                                    model.ItemDetails.mrp = finalmrp;
                                    model.ItemDetails.itemCode = itemcode;

                                    if (model.ColorCodeDetails.colorName != null)
                                    {
                                        model.ColorCodeDetails.modifiedOn = DateTime.Now;
                                        _ColorCodeService.Create(model.ColorCodeDetails);
                                    }
                                    if (model.MaterialDetails.MaterialName != null)
                                    {
                                         model.MaterialDetails.ModifiedOn = DateTime.Now;
                                        _TypeOfMaterialService.AddTypeOfMaterial(model.MaterialDetails);
                                    }                                    
                                    if (model.DesignDetails.DesignName != null)
                                    {
                                         model.DesignDetails.ModifiedOn = DateTime.Now;
                                        _DesignService.CreateDesign(model.DesignDetails);
                                    }
                                    if (model.BrandMasterDetails.BrandName != null)
                                    {
                                         model.BrandMasterDetails.ModifiedOn = DateTime.Now;
                                        _BrandMasterService.Create(model.BrandMasterDetails);
                                    }
                                    if (model.ItemNameDetails.Name != null)
                                    {
                                         model.ItemNameDetails.ModifiedOn = DateTime.Now;
                                        _ItemNameService.Create(model.ItemNameDetails);
                                    }
                                    if (model.ItemDetails.itemName != null)
                                    {
                                        _itemService.createItem(model.ItemDetails);
                                    }
                                    
                                //}
                            }

                            if (itemmastername != null)
                            {
                                if (itemcolor == null)
                                {

                                    var itemdetails = _ItemNameService.GetItemIdByName(itemName);
                                    if (itemdetails == null)
                                    {
                                        model.ItemNameDetails.ItemCategory = _ItemSubCategoryService.GetCategoryBySubCat(mainApplication.ItemDetails.itemSubCategory);
                                        model.ItemNameDetails.ItemSubcategory = mainApplication.ItemDetails.itemSubCategory;
                                        model.ItemNameDetails.Name = mainApplication.ItemDetails.itemName;
                                        model.ItemNameDetails.ItemType = finalType;
                                        model.ItemNameDetails.DeleteStatus = "Active";
                                        model.ItemNameDetails.Status = "Active";
                                    }

                                    //Type Of Material Masters Entry
                                    var materialname = frmItem[materialType];
                                    var matdetails = _TypeOfMaterialService.GetMaterialName(materialname);
                                    if (matdetails == null)
                                    {
                                        String MaterialCode;

                                        var mardetails = _TypeOfMaterialService.GetMaterialLast();
                                        int matval = 0;
                                        int length = 0;
                                        if (mardetails != null)
                                        {
                                            matval = mardetails.MaterialId;
                                            matval = matval + 1;
                                            length = matval.ToString().Length;
                                            MaterialCode = _utilityService.getName("TM", length, matval);
                                            model.MaterialDetails.MaterialCode = MaterialCode;
                                            model.MaterialDetails.MaterialName = finalMaterialType;
                                            model.MaterialDetails.MaterialShortName = finalMaterialType;
                                            model.MaterialDetails.MaterialDescription = finalMaterialType;
                                            model.MaterialDetails.Status = "Active";
                                        }
                                        else
                                        {
                                            matval = 1;
                                            length = 1;
                                            MaterialCode = _utilityService.getName("TM", length, matval);
                                            model.MaterialDetails.MaterialCode = MaterialCode;
                                            model.MaterialDetails.MaterialName = finalMaterialType;
                                            model.MaterialDetails.MaterialShortName = finalMaterialType;
                                            model.MaterialDetails.MaterialDescription = finalMaterialType;
                                            model.MaterialDetails.Status = "Active";

                                        }
                                    }


                                    //Color Code Master Entries                                    
                                    var colorname = finalColorCode;

                                    var colordetails = _ColorCodeService.getColorName(colorname);
                                    var colorcode = _ColorCodeService.GetcolorcodebyName(colorname).colorCode;
                                    if (colordetails == null)
                                    {
                                        var coldetails = _ColorCodeService.GetLastColor();
                                        if (coldetails == null)
                                        {
                                            model.ColorCodeDetails.colorCode = "01";
                                        }
                                        else
                                        {
                                            if (coldetails.colorCode.StartsWith("0"))
                                            {
                                                coldetails.colorCode.Substring(1);
                                                model.ColorCodeDetails.colorCode = (Convert.ToInt32(coldetails.colorCode) + 1).ToString();
                                                model.ColorCodeDetails.colorCode = "0" + model.ColorCodeDetails.colorCode;
                                            }
                                            else
                                            {
                                                var coldetailss = (Convert.ToInt32(coldetails.colorCode) + 1).ToString();
                                                model.ColorCodeDetails.colorCode = coldetailss;
                                                model.ColorCodeDetails.colorName = finalColorCode;
                                                model.ColorCodeDetails.Status = "Active";
                                            }

                                        }

                                    }

                                    //Design Masters Entries
                                    var designnme = finalDesignCode;
                                    var designdetails = _DesignService.GetNamebyDesignName(designnme);
                                    if (designdetails == null)
                                    {
                                        string dcode = generateCode();
                                        int counts = _DesignService.getDesignsByDesignCode(dcode);
                                        if (counts == 0)
                                        {
                                            model.DesignDetails.DesignCode = dcode;
                                            model.DesignDetails.SubCategoryName = mainApplication.ItemDetails.itemSubCategory;
                                            model.DesignDetails.DesignName = finalDesignCode;
                                            model.DesignDetails.Status = "Active";
                                        }
                                        else
                                        {
                                            model.DesignDetails.DesignCode = dcode;
                                            var subcatname = mainApplication.ItemDetails.itemCategory;
                                            var catcode = _ItemCategoryService.GetCodeByName(subcatname);
                                            model.DesignDetails.CategoryCode = catcode;
                                            model.DesignDetails.SubCategoryName = mainApplication.ItemDetails.itemSubCategory;
                                            model.DesignDetails.DesignName = finalDesignCode;
                                            model.DesignDetails.Status = "Active";
                                        }

                                    }

                                    //Brand Masters Entry
                                    String code;
                                    var brandname = finalbrand;
                                    var branddetails = _BrandMasterService.brandnamebyname(brandname);
                                    var details = _BrandMasterService.getLastBrand();
                                    if (branddetails == null)
                                    {
                                        int Brandval = 0;
                                        int Brandlength = 0;
                                        if (details != null)
                                        {
                                            Brandval = details.Id;
                                            Brandval = Brandval + 1;
                                            Brandlength = Brandval.ToString().Length;
                                            code = _utilityService.getName("BRD", Brandlength, Brandval);
                                            model.BrandMasterDetails.BrandCode = code;
                                            model.BrandMasterDetails.BrandName = finalbrand;
                                            model.BrandMasterDetails.Status = "Active";
                                        }
                                        else
                                        {
                                            Brandval = 1;
                                            Brandlength = 1;
                                            code = _utilityService.getName("BRD", Brandlength, Brandval);
                                            model.BrandMasterDetails.BrandCode = code;
                                            model.BrandMasterDetails.BrandName = finalbrand;
                                            model.BrandMasterDetails.Status = "Active";
                                        }

                                    }

                                    var itemdata = _itemService.GetLastItem();
                                    int itemval = 0;
                                    int lenght = 0;
                                    if (itemdata != null)
                                    {
                                        itemval = itemdata.itemId;
                                        itemval = itemval + 1;
                                        lenght = itemval.ToString().Length;
                                    }
                                    else
                                    {
                                        itemval = 1;
                                        lenght = 1;
                                    }
                                    itemcode = _utilityService.getName("I", lenght, itemval);


                                    //if (!string.IsNullOrEmpty(finalcostprice))

                                    model.ItemDetails.description = finalitemdescription;
                                    model.ItemDetails.modifedOn = DateTime.Now;
                                    model.ItemDetails.status = "Active";
                                    model.ItemDetails.itemSubCategory = mainApplication.ItemDetails.itemSubCategory;
                                    model.ItemDetails.itemCategory = _ItemSubCategoryService.GetCategoryBySubCat(mainApplication.ItemDetails.itemSubCategory);
                                    model.ItemDetails.itemName = mainApplication.ItemDetails.itemName;
                                    model.ItemDetails.costprice = finalcostprice;                                    
                                    model.ItemDetails.typeOfMaterial = finalMaterialType;                                    
                                    if (model.ColorCodeDetails.colorCode == null)
                                    {
                                        model.ItemDetails.colorCode = finalColorCode;
                                    }
                                    else
                                    {
                                        model.ItemDetails.colorCode = model.ColorCodeDetails.colorCode;
                                    }
                                    //model.ItemDetails.designCode = _DesignService.GetNamebyDesignName(finalDesignCode).DesignCode;
                                    //model.ItemDetails.designCode = model.DesignDetails.DesignCode;                                    
                                    var designdet = _DesignService.GetNamebyDesignName(finalDesignCode);
                                    if (designdet != null)
                                        model.ItemDetails.designCode = designdet.DesignCode;
                                    model.ItemDetails.designName = finalDesignCode;
                                    model.ItemDetails.size = finalSize;
                                    model.ItemDetails.unit = finalUnit;
                                    var unitdetails = _UnitService.GetDetailsByName(model.ItemDetails.unit);
                                    if (unitdetails != null)
                                        model.ItemDetails.NumberType = unitdetails.NumberType;
                                    //model.ItemDetails.NumberType = _UnitService.GetDetailsByName(finalUnit).NumberType;
                                    //model.ItemDetails.NumberType = finalnumbertype;
                                    model.ItemDetails.itemtype = finalType;
                                    model.ItemDetails.sellingprice = finalsellingprice;
                                    model.ItemDetails.brandName = finalbrand;
                                    model.ItemDetails.mrp = finalmrp;
                                    model.ItemDetails.itemCode = itemcode;

                                    if (model.ColorCodeDetails.colorName != null)
                                    {
                                        model.ColorCodeDetails.modifiedOn = DateTime.Now;
                                        _ColorCodeService.Create(model.ColorCodeDetails);
                                    }
                                    if (model.MaterialDetails.MaterialName != null)
                                    {
                                        model.MaterialDetails.ModifiedOn = DateTime.Now;
                                        _TypeOfMaterialService.AddTypeOfMaterial(model.MaterialDetails);
                                    }
                                    if (model.DesignDetails.DesignName != null)
                                    {
                                        model.DesignDetails.ModifiedOn = DateTime.Now;
                                        _DesignService.CreateDesign(model.DesignDetails);
                                    }
                                    if (model.BrandMasterDetails.BrandName != null)
                                    {
                                        model.BrandMasterDetails.ModifiedOn = DateTime.Now;
                                        _BrandMasterService.Create(model.BrandMasterDetails);
                                    }
                                    if (model.ItemNameDetails.Name != null)
                                    {
                                        model.ItemNameDetails.ModifiedOn = DateTime.Now;
                                        _ItemNameService.Create(model.ItemNameDetails);
                                    }
                                    if (model.ItemDetails.itemName != null)
                                    {
                                        _itemService.createItem(model.ItemDetails);
                                    }
                                    
                                }
                            }
                    }
                    //IF ITEM TYPE IS NON_INVENTORY THEN ITEM STORED IN NONINVENTORYITEMS
                    else
                    {
                        //CREATE NON-INVENTORY ITEM CODE
                        var noninvitemdata = _NonInventoryService.GetLastItem();
                        int noninvitemval = 0;
                        int noninvlenght = 0;
                        if (noninvitemdata != null)
                        {
                            noninvitemval = noninvitemdata.itemId;
                            noninvitemval = noninvitemval + 1;
                            noninvlenght = noninvitemval.ToString().Length;
                        }
                        else
                        {
                            noninvitemval = 1;
                            noninvlenght = 1;
                        }
                        string noninvitemcode = _utilityService.getName("NI", noninvlenght, noninvitemval);

                        if (!string.IsNullOrEmpty(finalType))
                        {
                            model.NonInventoryItemDetails.description = finalitemdescription;
                            model.NonInventoryItemDetails.modifedOn = DateTime.Now;
                            model.NonInventoryItemDetails.status = "Active";
                            model.NonInventoryItemDetails.itemCategory = _ItemSubCategoryService.GetCategoryBySubCat(mainApplication.ItemDetails.itemSubCategory);
                            model.NonInventoryItemDetails.itemSubCategory = mainApplication.ItemDetails.itemSubCategory;
                            model.NonInventoryItemDetails.itemName = mainApplication.ItemDetails.itemName;
                            model.NonInventoryItemDetails.costprice = finalcostprice;
                            model.NonInventoryItemDetails.typeOfMaterial = finalMaterialType;
                            model.NonInventoryItemDetails.colorCode = finalColorCode;
                            model.NonInventoryItemDetails.designCode = finalDesignCode;
                            model.NonInventoryItemDetails.designName = finalDesignName;
                            model.NonInventoryItemDetails.size = finalSize;
                            model.NonInventoryItemDetails.unit = finalUnit;
                            model.NonInventoryItemDetails.NumberType = finalnumbertype;
                            model.NonInventoryItemDetails.itemtype = finalType;
                            model.NonInventoryItemDetails.sellingprice = finalsellingprice;
                            model.NonInventoryItemDetails.brandName = finalbrand;
                            model.NonInventoryItemDetails.mrp = finalmrp;
                            model.NonInventoryItemDetails.itemCode = noninvitemcode;
                            _NonInventoryService.createNonInventoryItem(model.NonInventoryItemDetails);
                        }
                    }
                }
            }

            //UPDATE ITEM NAME DELETE STATUS IS INACTIVE
            var itemnamedetails = _ItemNameService.GetDataByItemName(mainApplication.ItemDetails.itemName);
            itemnamedetails.DeleteStatus = "InActive";
            _ItemNameService.Update(itemnamedetails);

            int lastitemafter = _itemService.GetLastItem().itemId;
            //int lastnoninventoryitemafter = _NonInventoryService.GetLastItem().itemId;

            var ItemBefore = Encode(lastitembefore.ToString());
            var ItemAfter = Encode(lastitemafter.ToString());
            //var NonInvItemBefore = Encode(lastnoninventoryitembefore.ToString());
            //var NonInvItemAfter = Encode(lastnoninventoryitemafter.ToString());

            return RedirectToAction("CreateItemDetails", "Item", new { LastItemBefore = ItemBefore, LastItemAfter = ItemAfter,});
        }

        //GENERETE DESIGN CODE
        [HttpGet]
        public static string generateCode()
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random randNum = new Random();
            char[] chars = new char[4];
            int allowedCharCount = allowedChars.Length;
            for (int i = 0; i < 4; i++)
            {
                chars[i] = allowedChars[(int)((allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        //SHOW ITEM CREATE DETAILS
        [HttpGet]
        public ActionResult CreateItemDetails(string LastItemBefore, string LastItemAfter, string LastNonInventoryItemBefore, string LastNonInventoryItemAfter)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int LastIdBefore = Decode(LastItemBefore);
            int LastIdAfter = Decode(LastItemAfter);
            //int NonInvLastIdBefore = Decode(LastNonInventoryItemBefore);
            //int NonInvLastIdAfter = Decode(LastNonInventoryItemAfter);

            model.ItemDetails = _itemService.GetItem(LastIdBefore);
            //if (model.ItemDetails == null)
            //{
            //    model.NonInventoryItemDetails = _NonInventoryService.GetItemById(NonInvLastIdBefore);
            //}
            model.ItemList = _itemService.GetInsertedRow(LastIdBefore, LastIdAfter);
            //model.NonInventoryItemList = _NonInventoryService.GetInsertedRow(NonInvLastIdBefore, NonInvLastIdAfter);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetUnitType(string unit)
        {
            string numbertype = _UnitService.GetNumberTypeByUnit(unit);
            return Json(numbertype, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditItem()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ItemSubCategoryList = _ItemSubCategoryService.GetItemSubCategories();
            model.ItemCategoryList = _ItemCategoryService.GetAllItemCategories();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSubCatByCat(string name)
        {
            var details = _ItemSubCategoryService.GetSubCategoryByCategory(name);
            var select = details.Select(m => new SelectListItem
            {
                Text = m.subCategoryName,
                Value = m.subCategoryName
            });
            return Json(select, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemList(string name)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _itemService.GetItemsBySubCategory(name);
            TempData["ItemList"] = model.ItemList;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication();
            model.ItemDetails = _itemService.getById(id);
            model.typeOfMaterialList = _TypeOfMaterialService.getAllTypeOfMaterial();
            model.ColorCodeList = _ColorCodeService.getAllColorCode();
            model.DesignList = _DesignService.GetAll();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(Item item)
        {
            item.status = "Active";
            item.modifedOn = DateTime.Now;
            _itemService.updateItem(item);
            return RedirectToAction("ItemDetails/" + Convert.ToInt32(item.itemId), "Item");
        }

        [HttpGet]
        public ActionResult ItemDetails(int id)
        {
            MainApplication model = new MainApplication()
            {
                ItemDetails = new Item(),
            };
            model.ItemDetails = _itemService.getById(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult ItemSearch(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<Item> ListOfItems = TempData["ItemList"] as IEnumerable<Item>;
            List<Item> FinalList = new List<Item>();
            FinalList = FinalList.Concat(ListOfItems.Where(x => x.itemName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ItemList = FinalList;
            TempData["ItemList"] = FinalList;
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteItem()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ItemCategoryList = _ItemCategoryService.GetAllItemCategories();
            return View(model);
        }

        [HttpGet]
        public ActionResult ItemListForDelete(string subcategory)
        {
            MainApplication model = new MainApplication();
            var details = _itemService.GetItemsBySubCategory(subcategory);
            List<Item> itemlist = new List<Item>();
            foreach (var data in details)
            {
                var entrystock = _EntryStockItemService.getDetailsByItemCode(data.itemCode);
                var openingstock = _OpeningStockService.GetDataByItemCode(data.itemCode);
                var inwarddetails = _InwardItemFromSupplierService.GetItemDetailsByItemCode(data.itemCode);
                if (entrystock == null && openingstock == null && inwarddetails == null)
                {
                    itemlist.Add(data);
                }
            }
            model.ItemList = itemlist;
            TempData["ItemListForDelete"] = model.ItemList;
            return View(model);
        }

        [HttpGet]
        public JsonResult InActiveItem(string code)
        {
            var data = _itemService.GetDescriptionByItemCode(code);
            data.status = "InActive";
            data.modifedOn = DateTime.Now;
            _itemService.updateItem(data);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        // ****************************************** CREATE ITEM NAME **********************************************

        [HttpGet]
        public ActionResult CreateItemName()
        {
            MainApplication model = new MainApplication();
            IEnumerable<ItemSubCategory> itemSubCatList = _ItemSubCategoryService.GetItemSubCategories();
            model.ItemSubCategoryList = itemSubCatList;

            if (TempData["ItemNameList"] != null)
            {
                ViewBag.error = "This Item Name is Already Present";
            }
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            return View(model);
        }

        //GET SUB CATEGORY LIST BY CATEGORY
        [HttpGet]
        public JsonResult GetItemNameList(string term)
        {
            var result = _ItemNameService.GetItemNameList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.Name);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateItemName(FormCollection frmcol, MainApplication mainapp)
        {
            var itemtype = frmcol["ItemType"];

            MainApplication model = new MainApplication()
            {
                ItemNameDetails = new ItemName(),
            };

            mainapp.ItemNameDetails.ItemCategory = _ItemSubCategoryService.GetCategoryBySubCat(mainapp.ItemNameDetails.ItemSubcategory);
            mainapp.ItemNameDetails.ItemType = frmcol["ItemType"];
            mainapp.ItemNameDetails.DeleteStatus = "Active";
            mainapp.ItemNameDetails.Status = "Active";
            mainapp.ItemNameDetails.ModifiedOn = DateTime.Now;

            var namelist = _ItemNameService.GetItemNameListBySubcategory(mainapp.ItemNameDetails.ItemSubcategory, mainapp.ItemNameDetails.Name);
            if (namelist.Count() != 0)
            {
                TempData["ItemNameList"] = "error";
                return RedirectToAction("CreateItemName");
            }
            else
            {
                _ItemNameService.Create(mainapp.ItemNameDetails);
            }

            var id = _ItemNameService.GetLastItemName().Id;
            var ItemNameId = Encode(id.ToString());
            return RedirectToAction("CreateItemNameDetails/" + ItemNameId, "Item");
        }
        
        //SHOW ITEM NAME CREATE DETAILS
        [HttpGet]
        public ActionResult CreateItemNameDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.ItemNameDetails = _ItemNameService.GetItemNamebyId(Id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        // DELETE ITEM NAME
        [HttpGet]
        public ActionResult DeleteItemName()
        {
            MainApplication model = new MainApplication();
            model.ItemSubCategoryList = _ItemSubCategoryService.GetItemSubCategories();
            model.ItemNameList = _ItemNameService.GetItemNameForDelete();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        [HttpGet]
        public ActionResult LoadItemNames(string name)
        {
            MainApplication model = new MainApplication()
            {
                ItemNameDetails=new ItemName(),
                ItemSubCategoryDetails = new ItemSubCategory(),
            };
            model.ItemNameList = _ItemNameService.GetItemBySubCategoryForDelete(name);
            TempData["ItemNameList"] = model.ItemNameList;
            return View(model);
        }

        //SEARCH ITEM NAME LIST
        [HttpGet]
        public ActionResult ItemNameList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<ItemName> ListOfItemName = TempData["ItemNameList"] as IEnumerable<ItemName>;
            ListOfItemName = ListOfItemName.Where(x => x.Status == "Active");
            List<ItemName> FinalList = new List<ItemName>();
            FinalList = FinalList.Concat(ListOfItemName.Where(x => x.Name.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ItemNameList = FinalList;
            TempData["ItemNameList"] = FinalList;
            return View(model);
        }

        ///PARTIAL VIEW FOR DELETE ITEM NAME
        [HttpGet]
        public ActionResult ItemNameDeletePartial(int ID)
        {
            MainApplication model = new MainApplication();
            model.ItemNameDetails = _ItemNameService.GetId(ID);
            return View(model);
        }

        [HttpPost]
        public ActionResult ItemNameDeletePartial(ItemName itemname)
        {
            itemname.Status = "InActive";
            itemname.DeleteStatus = "InActive";
            itemname.ModifiedOn = DateTime.Now;
            _ItemNameService.Update(itemname);
            return RedirectToAction("ItemNameDeleteDetails/" + itemname.Id, "Item");
        }

        ///SHOW ITEM NAME DETAILS AFTER DELETE
        [HttpGet]
        public ActionResult ItemNameDeleteDetails(int id)
        {
            ItemName name = new ItemName();
            TempData["ItemNameList"] = _ItemNameService.GetItemNames();
            name = _ItemNameService.GetItemNamebyId(id);
            return View(name);
        }

        [HttpGet]
        public JsonResult GetSubCategories(string term,string category)
        {
            var details = _ItemSubCategoryService.GetSubcategoryList(term, category);
            List<string> list = new List<string>();
            foreach (var data in details)
            {
                list.Add(data.subCategoryName);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}

