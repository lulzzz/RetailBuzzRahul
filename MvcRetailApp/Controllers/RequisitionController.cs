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
using MvcRetailApp.ModelBinder;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;
using System.Net.Mail;
using System.Net;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class RequisitionController : Controller
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

        private readonly IUtilityService _utilityservice;
        private readonly IShopRequisitionGodownService _ishoprequisitiongodownservice;
        private readonly IShopRequisitionGodownItemService _ishoprequisitiongodownitemservice;
        private readonly IGodownService _igodownservive;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IShopStockService _iShopStockServive;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IShopService _iShopService;
        private readonly IRetailBillItemService _iRetailBillItemService;
        private readonly IGodownStockService _igodownstockservice;
        private readonly IRequisitionForShopService _iRequisitionForShopService;
        private readonly IRequisitionForGodownService _iRequisitionforgodownservice;
        private readonly IRequisitionOfPoService _iRequisitionOfPoService;
        private readonly IOutwardToShopService _iOutwardToShopService;
        private readonly IOutwardItemToShopService _iOutwardItemToShopService;
        private readonly IOpeningStockService _iOpeningStockService;
        private readonly IStockItemDistributionService _iStockItemDistributionService;
        private readonly IUserService _iUserService;
        private readonly IOutwardInterShopService _OutwardInterShopService;
        private readonly IOutwardItemInterShopservice _OutwardItemInterShopservice;
        private readonly IOutwardInterGodownService _outwardInterGodownService;
        private readonly IOutwardItemInterGodownService _outwardItemInterGodownService;
        private readonly IGodownAddressService _iGodownAddressService;
        private readonly IItemSubCategoryService _iItemSubCategoryService;
        private readonly IItemService _iItemService;
        private readonly IShopRequisitionGodownSalesBillService _ShopRequisitionGodownSalesBillService;
        private readonly IShopRequisitionGodownItemSalesBillService _iShopRequisitionGodownItemSalesBillService;
        private readonly IEntryStockItemService _iEntryStockItemService;
        private readonly IRequisitionForNewItemService _iRequisitionForNewItemService;
        private readonly INonInventoryItemService _iNonInventoryItemService;
        private readonly IBrandMasterService _iBrandMasterService;

        public RequisitionController(IUtilityService utilityservice, IShopRequisitionGodownService shoprequisitiongodownservice, IShopRequisitionGodownItemService shoprequisitiongodownitemservice, IGodownService godownservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService,
            IShopStockService shopStockService, IEmployeeMasterService EmployeeMasterService, IShopService Shopservice,
            IGodownStockService godownstockservice, IRequisitionForShopService RequisitionForShopService, IRequisitionForGodownService Requisitionforgodownservice,
            IRequisitionOfPoService RequisitionOfPo, IRetailBillItemService RetailBillItemService,
            IOutwardToShopService OutwardToShopService, IOutwardItemToShopService Outwarditemtoshopservice,
            IOpeningStockService OpeningStockService, IStockItemDistributionService StockItemDistributionService,
            IUserService UserService, IOutwardInterShopService OutwardInterShopService,
            IOutwardItemInterShopservice OutwardItemInterShopservice, IOutwardInterGodownService outwardintergodownservice,
            IOutwardItemInterGodownService outwarditemintergodowndervice, IGodownAddressService godownaddressservice,
            IItemSubCategoryService ItemSubCategoryService, IItemService ItemService, IShopRequisitionGodownSalesBillService ShopRequisitionGodownSalesBillService,
            IShopRequisitionGodownItemSalesBillService ShopRequisitionGodownItemSalesBillService, IEntryStockItemService EntryStockItemService,
            IRequisitionForNewItemService RequisitionForNewItemService, INonInventoryItemService NonInventoryItemService, IBrandMasterService BrandMasterService)
        {
            this._ishoprequisitiongodownservice = shoprequisitiongodownservice;
            this._ishoprequisitiongodownitemservice = shoprequisitiongodownitemservice;
            this._utilityservice = utilityservice;
            this._igodownservive = godownservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._iShopStockServive = shopStockService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._iShopService = Shopservice;
            this._igodownstockservice = godownstockservice;
            this._iRequisitionForShopService = RequisitionForShopService;
            this._iRequisitionforgodownservice = Requisitionforgodownservice;
            this._iRequisitionOfPoService = RequisitionOfPo;
            this._iRetailBillItemService = RetailBillItemService;
            this._iOutwardToShopService = OutwardToShopService;
            this._iOutwardItemToShopService = Outwarditemtoshopservice;
            this._iOpeningStockService = OpeningStockService;
            this._iStockItemDistributionService = StockItemDistributionService;
            this._iUserService = UserService;
            this._OutwardInterShopService = OutwardInterShopService;
            this._OutwardItemInterShopservice = OutwardItemInterShopservice;
            this._outwardInterGodownService = outwardintergodownservice;
            this._outwardItemInterGodownService = outwarditemintergodowndervice;
            this._iGodownAddressService = godownaddressservice;
            this._iItemSubCategoryService = ItemSubCategoryService;
            this._iItemService = ItemService;
            this._ShopRequisitionGodownSalesBillService = ShopRequisitionGodownSalesBillService;
            this._iShopRequisitionGodownItemSalesBillService = ShopRequisitionGodownItemSalesBillService;
            this._iEntryStockItemService = EntryStockItemService;
            this._iRequisitionForNewItemService = RequisitionForNewItemService;
            this._iNonInventoryItemService = NonInventoryItemService;
            this._iBrandMasterService = BrandMasterService;
        }

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

        [HttpGet]
        public ActionResult Requisition()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.ShopStockList = _iShopStockServive.getShopStock();
            model.GodownStockList = _igodownstockservice.getGodownStock();
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            string shopcodevalue = string.Empty;
            string godownname = string.Empty;
            var detailsemployee = _EmployeeMasterService.getEmpByEmail(UserEmail);
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            shopcodevalue = assigndetails.AssignRightsCode;
                            return RedirectToAction("ShopRequisitionForShopLogin", "Requisition");
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            godownname = assigndetails.Modules;
                            return RedirectToAction("GodownRequisitionForGodownLogin", "Requisition");
                        }
                        break;
                    }
                }
            }

            else
                if (detailsemployee.Designation == "PurchaseManager")
                {

                    return RedirectToAction("RequisitionOfPo", "Requisition");
                }
            return RedirectToAction("RequisitionOfPo", "Requisition");

        }


        [HttpGet]
        public JsonResult GetItemNames(string term)
        {
            var shopname = Session["SHOPGODOWNNAME"].ToString();
            var result = _iItemService.GetItemNames(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.itemName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemNamesOfItemMaster(string term)
        {
            //var shopname = Session["SHOPGODOWNNAME"].ToString();
            var result = _iItemService.GetItemNames(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.itemCode + "+" + item.itemName);
                //names.Add(item.brandName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemNamesOfItemMasterForBrand(string term)
        {
            //var shopname = Session["SHOPGODOWNNAME"].ToString();
            var result = _iBrandMasterService.GetBrandList(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.BrandName);
                //names.Add(item.brandName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemNamesForPo(string term)
        {
            var result = _iShopStockServive.getShopStockByItemName(term);
            var result2 = _igodownstockservice.getGodownStockNameByItemName(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                foreach (var item2 in result2)
                {
                    names.Add(item.ItemName);
                    names.Add(item2.ItemName);
                }
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemNamesForGodown(string term)
        {
            var shopname = Session["SHOPGODOWNNAME"].ToString();
            var result = _iItemService.GetItemNames(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.itemName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult getItemReport(string itemname)
        {
            MainApplication model = new MainApplication()
            {
                ShopStockDetails = new ShopStock(),
            };

            var shopname = Session["SHOPGODOWNNAME"].ToString();

            model.RequisitionForShopList = _iRequisitionForShopService.GetRequisitionDetailsByItemnameandShopName(itemname, shopname);
            return View(model);
        }

        [HttpGet]
        public ActionResult getItemReportOfItemMaster(string itemname, string brandname)
        {
            MainApplication model = new MainApplication()
            {
                ShopStockDetails = new ShopStock(),
                EntryStockItemDetails = new EntryStockItem(),
                RequisitionofNewItemsForShopDetails = new RequisitionofNewItemsForShop(),
                ItemDetails = new Item(),
            };

            model.RequisitionofNewItemsForShopDetails.BrandName = brandname;
            char[] delimiterChars = { '+', ',', '.', ':', '\t' };
            string[] words = itemname.Split(delimiterChars);

            if (words.Count() >= 2)
            {
                var details1 = _iEntryStockItemService.getDetailsByItemNameandItemCodeAndBrandName(words[0], words[1], brandname);

                if (details1 == null)
                {
                    ViewBag.data = "Stock Not Available";
                    model.ItemDetails = _iItemService.GetDescriptionByItemNameandItemCode(words[0], words[1]);
                }
                else
                {
                    ViewBag.datavailable = "Stock Available";
                }
            }

            else
            {

                var itemdetails = _iItemService.GetDescriptionByItemAndBrandName(words[0], brandname);
                if (itemdetails == null)
                {
                    ViewBag.dataitemavailable = "Item Not Available";
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult NewItem()
        {
            MainApplication model = new MainApplication();
            return View(model);
        }

        [HttpPost]
        public ActionResult NewItem(MainApplication model, FormCollection frmItem)
        {
            model.ItemDetails = new Item();
            model.NonInventoryItemDetails = new NonInventoryItem();

            int lastitembefore = 0;
            var data = _iItemService.GetLastItem();
            if (data == null)
            {
                lastitembefore = 1;
            }
            else
            {
                lastitembefore = data.itemId + 1;
            }

            int lastnoninventoryitembefore = 0;
            var noninventorydata = _iNonInventoryItemService.GetLastItem();
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
                    string type = "ItemType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string brandname = "Brand" + i;
                    string numbertype = "NumberType" + i;
                    string mrp = "MRP" + i;
                    string CommissionInPercent = "CommissionInPercent" + i;
                    string CommissionInRupees = "CommissionInRupees" + i;

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
                    string finalType = frmItem[type];
                    string finalsellingprice = frmItem[sellingprice];
                    string finalbrandname = frmItem[brandname];
                    string finalCommissionInPercent = frmItem[CommissionInPercent];
                    string finalCommissionInRupees = frmItem[CommissionInRupees];

                    //IF ITEM TYPE IS INVENTORY THEN ITEM STORED IN ITEM MASTER
                    if (finalType == "Inventory")
                    {

                        //CREATE ITEM CODE
                        var itemdata = _iItemService.GetLastItem();
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
                        string itemcode = _utilityservice.getName("I", lenght, itemval);

                        if (!string.IsNullOrEmpty(finalcostprice))
                        {
                            model.ItemDetails.description = finalitemdescription;
                            model.ItemDetails.modifedOn = DateTime.Now;
                            model.ItemDetails.status = "Active";
                            model.ItemDetails.itemCategory = frmItem["ItemDetails.itemCategory"];
                            model.ItemDetails.itemSubCategory = frmItem["ItemDetails.itemSubCategory"];
                            model.ItemDetails.itemName = frmItem["itemDetails.itemName"];
                            model.ItemDetails.costprice = finalcostprice;
                            model.ItemDetails.typeOfMaterial = finalMaterialType;
                            model.ItemDetails.colorCode = finalColorCode;
                            model.ItemDetails.designCode = finalDesignCode;
                            model.ItemDetails.designName = finalDesignName;
                            model.ItemDetails.size = finalSize;
                            model.ItemDetails.unit = finalUnit;
                            model.ItemDetails.NumberType = finalnumbertype;
                            model.ItemDetails.itemtype = finalType;
                            model.ItemDetails.sellingprice = finalsellingprice;
                            model.ItemDetails.brandName = finalbrandname;
                            model.ItemDetails.mrp = finalmrp;
                            model.ItemDetails.itemCode = itemcode;
                            _iItemService.createItem(model.ItemDetails);
                        }
                    }
                    //IF ITEM TYPE IS NON_INVENTORY THEN ITEM STORED IN NONINVENTORYITEMS
                    else
                    {
                        //CREATE NON-INVENTORY ITEM CODE
                        var noninvitemdata = _iNonInventoryItemService.GetLastItem();
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
                        string noninvitemcode = _utilityservice.getName("NI", noninvlenght, noninvitemval);

                        if (!string.IsNullOrEmpty(finalType))
                        {
                            model.NonInventoryItemDetails.description = finalitemdescription;
                            model.NonInventoryItemDetails.modifedOn = DateTime.Now;
                            model.NonInventoryItemDetails.status = "Active";
                            model.NonInventoryItemDetails.itemCategory = frmItem["ItemDetails.itemCategory"];
                            model.NonInventoryItemDetails.itemSubCategory = frmItem["ItemDetails.itemSubCategory"];
                            model.NonInventoryItemDetails.itemName = frmItem["itemDetails.itemName"];
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
                            model.NonInventoryItemDetails.brandName = finalbrandname;
                            model.NonInventoryItemDetails.mrp = finalmrp;
                            model.NonInventoryItemDetails.itemCode = noninvitemcode;
                            _iNonInventoryItemService.createNonInventoryItem(model.NonInventoryItemDetails);
                        }
                    }
                }
            }
            int lastitemafter = _iItemService.GetLastItem().itemId;
            int lastnoninventoryitemafter = _iNonInventoryItemService.GetLastItem().itemId;
            return RedirectToAction("NewItemDetails", "Requisition", new { LastItemBefore = lastitembefore, LastItemAfter = lastitemafter, LastNonInventoryItemBefore = lastnoninventoryitembefore, LastNonInventoryItemAfter = lastnoninventoryitemafter });
        }

        public ActionResult NewItemDetails(int LastItemBefore, int LastItemAfter, int LastNonInventoryItemBefore, int LastNonInventoryItemAfter)
        {
            MainApplication model = new MainApplication();
            var item = _iItemService.GetLastItem();
            int lastinsertedrow = item.itemId;
            TempData["ItemSubCat"] = item.itemSubCategory;
            model.ItemDetails = _iItemService.GetItem(LastItemBefore);
            if (model.ItemDetails == null)
            {
                model.NonInventoryItemDetails = _iNonInventoryItemService.GetItemById(LastNonInventoryItemBefore);
            }
            model.ItemList = _iItemService.GetInsertedRow(LastItemBefore, LastItemAfter);
            model.NonInventoryItemList = _iNonInventoryItemService.GetInsertedRow(LastNonInventoryItemBefore, LastNonInventoryItemAfter);
            return View(model);
        }
        [HttpGet]
        public ActionResult getItemReportForPo(string itemname)
        {
            MainApplication model = new MainApplication()
            {
                ShopStockDetails = new ShopStock(),
            };

            model.ShopStockList = _iShopStockServive.getStockByItemName(itemname);
            model.GodownStockList = _igodownstockservice.GetRowsByGodownItemName(itemname);
            return View(model);
        }

        [HttpGet]
        public ActionResult getItemReportForGodown(string itemname)
        {
            MainApplication model = new MainApplication()
            {
                ShopStockDetails = new ShopStock(),
                GodownStockDetails = new GodownStock(),
            };

            var shopname = Session["SHOPGODOWNNAME"].ToString();
            model.Requisitionforgodownlist = _iRequisitionforgodownservice.GetRowsByItemnameandGodownNameForRequisitionForGodown(itemname, shopname);
            return View(model);
        }

        [HttpGet]
        public ActionResult getStockDetailsByItemName(string itemnamevalue)
        {
            MainApplication model = new MainApplication();
            var value = itemnamevalue;
            char[] delimiterChars = { ',' };
            string[] words = value.Split(delimiterChars);
            int lenght = words.Count();
            int i = 0;
            string[] wordsvalue = new string[lenght];
            List<ShopStock> listvalue = new List<ShopStock>();
            List<ShopStock> listvalue1 = new List<ShopStock>();
            var shopname = Session["SHOPGODOWNNAME"].ToString();

            for (i = 0; i < lenght; i++)
            {
                wordsvalue[i] = words[i];

                char[] delimiterChars1 = { '.' };
                string[] words1 = wordsvalue[i].Split(delimiterChars1);

                model.ShopStockList = _iShopStockServive.GetRowsByItemcodeandItemname(words1[0], words1[1], shopname);

                listvalue = model.ShopStockList.ToList();
                foreach (var item in listvalue)
                {
                    listvalue1.Add(item);
                }
                model.ShopStockList = listvalue1;
            }
            return View(model);

        }

        [HttpGet]
        public ActionResult getStockDetailsByItemNameForGodown(string itemnamevalue)
        {
            MainApplication model = new MainApplication();
            var value = itemnamevalue;
            char[] delimiterChars = { ',' };
            string[] words = value.Split(delimiterChars);
            int lenght = words.Count();
            int i = 0;
            string[] wordsvalue = new string[lenght];
            var godownname = Session["SHOPGODOWNNAME"].ToString();
            List<GodownStock> listvalue = new List<GodownStock>();
            List<GodownStock> listvalue1 = new List<GodownStock>();

            for (i = 0; i < lenght; i++)
            {

                wordsvalue[i] = words[i];

                char[] delimiterChars1 = { '.' };
                string[] words1 = wordsvalue[i].Split(delimiterChars1);

                model.GodownStockList = _igodownstockservice.GetRowsByItemcodeandItemname(words1[0], words1[1], godownname);
                listvalue = model.GodownStockList.ToList();
                foreach (var item in listvalue)
                {
                    listvalue1.Add(item);
                }
                model.GodownStockList = listvalue1;

            }
            return View(model);

        }


        [HttpGet]
        public ActionResult getStockDetailsByItemNameForPo(string itemnamevalue)
        {
            MainApplication model = new MainApplication();
            var value = itemnamevalue;
            char[] delimiterChars = { ',' };
            string[] words = value.Split(delimiterChars);
            int lenght = words.Count();
            int i = 0;
            string[] wordsvalue = new string[lenght];
            List<ShopStock> listvalue = new List<ShopStock>();
            List<ShopStock> listvalue1 = new List<ShopStock>();

            for (i = 0; i < lenght; i++)
            {

                wordsvalue[i] = words[i];

                char[] delimiterChars1 = { '.' };
                string[] words1 = wordsvalue[i].Split(delimiterChars1);

                model.ShopStockList = _iShopStockServive.GetRowsByItemcodeandItemnameWithoutshopname(words1[0], words1[1]);
                model.GodownStockList = _igodownstockservice.GetRowsByItemcodeandItemnameWithoutGodownName(words1[0], words1[1]);
                listvalue = model.ShopStockList.ToList();
                foreach (var item in listvalue)
                {
                    listvalue1.Add(item);
                }
                model.ShopStockList = listvalue1;

            }
            return View(model);

        }
        [HttpGet]
        public ActionResult GetItemDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime? TDate = Convert.ToDateTime(ToDate);
            DateTime? FDate = Convert.ToDateTime(FromDate);
            model.RequisitionForShopList = _iRequisitionForShopService.getItemDetailsByDateForRequisitionForShop(FDate, TDate);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetItemDetailsByDateForPo(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.ShopStockList = _iShopStockServive.getItemDetailsByDate(FDate, TDate);
            model.GodownStockList = _igodownstockservice.getItemDetailsByDate(FDate, TDate);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetItemDetailsByDateForGodown(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.Requisitionforgodownlist = _iRequisitionforgodownservice.getItemDetailsByDateForRequisitionForGodown(FDate, TDate);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetItemDetailsByItemNameandDate(string Itemname, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.RequisitionForShopList = _iRequisitionForShopService.getItemDetailsByDateAndItemNameForRequisitionForShop(FDate, TDate, Itemname);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetItemDetailsByItemNameandDateForPo(string Itemname, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.ShopStockList = _iShopStockServive.getItemDetailsByDateAndItemName(FDate, TDate, Itemname);
            model.GodownStockList = _igodownstockservice.getItemDetailsByDateAndItemName(FDate, TDate, Itemname);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetItemDetailsByItemNameandDateForGodown(string Itemname, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.Requisitionforgodownlist = _iRequisitionforgodownservice.getItemDetailsByDateAndItemNameForRequisitionForGodown(FDate, TDate, Itemname);
            return View(model);
        }

        [HttpGet]
        public ActionResult ShopRequisitionByItem()
        {
            MainApplication model = new MainApplication();
            model.ItemSubCategoryList = _iItemSubCategoryService.getSubCategory();


            return View(model);
        }

        [HttpGet]
        public ActionResult GodownRequisitionByItem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShopRequisitionByDate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GodownRequisitionByDate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PoShopRequisitionByDate()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getItemNameBySubcategory(string valuesubcat, string valuecat)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _iItemService.GetItemByCategoryForRequisition(valuecat, valuesubcat);
            var data = model.ItemList.Select(s => new SelectListItem()
            {
                Text = s.itemCode + "." + s.itemName,
                Value = s.itemCode + "." + s.itemName,
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult LoadItemBySubCategory(string subcat)
        {

            MainApplication model = new MainApplication()
            {

                ItemDetails = new Item(),
            };
            var itemlist = _iItemService.GetItemsBySubCategory(subcat);
            foreach (var item in itemlist)
            {
                model.ItemDetails.itemName = item.itemName;
            }


            return View(model.ItemDetails.itemName);
        }


        [HttpGet]
        public ActionResult ShopRequisitionByItemNameAndDate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShopRequisitionByItemNameCategory()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ShopRequisitionByItemNameCategoryForPo()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GodownRequisitionByItemNameCategory()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ShopRequisitionByItemNameAndDateForPo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GodownRequisitionByItemNameAndDate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PoRequisitionByItem()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GodownRequisitionForGodownLogin()
        {

            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
                usercredential = new UserCredential(),
                RequisitionForGodownDetails = new RequisitionForGodown(),
                EmployeeDetails = new EmployeeMaster(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.EmployeeDetails = _EmployeeMasterService.getEmpByEmail(UserEmail);
            model.RequisitionForGodownDetails.EmpName = model.EmployeeDetails.Name;
            model.RequisitionForGodownDetails.EmpDesignation = model.EmployeeDetails.Designation;
            model.RequisitionForGodownDetails.EmailIdEmployee = model.EmployeeDetails.Email;
            model.RequisitionForGodownDetails.EmpContactNo = model.EmployeeDetails.MobileNo;
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            string shopcodevalue = string.Empty;
            string godownname = string.Empty;

            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            shopcodevalue = assigndetails.AssignRightsCode;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            godownname = assigndetails.AssignRightsCode;
                        }
                        break;
                    }
                }
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var Data = _iRequisitionforgodownservice.GetLastRequisition();
            int length = 0;
            int number = 0;
            if (Data != null)
            {

                length = 0;
                number = 0;
                string code = string.Empty;
                code = Data.ReqCode.Substring(3, 6);
                length = (Convert.ToInt32(code) + 1).ToString().Length;
                number = Convert.ToInt32(code) + 1;
                string Code = _utilityservice.getName("IIG", length, number);
                model.RequisitionForGodownDetails.ReqCode = Code + FinYr;
            }
            else
            {
                length = 1;
                number = 1;
                string Code = _utilityservice.getName("IIG", length, number);
                model.RequisitionForGodownDetails.ReqCode = Code;
            }
            TempData["PreviousGodownRequisition"] = model.RequisitionForGodownDetails.ReqCode;

            //RetailbillRequisition
            model.ShopRequisitionGodownList = _ishoprequisitiongodownservice.getAllDetails();
            List<ShopRequisitionGodownItem> newlist = new List<ShopRequisitionGodownItem>();
            foreach (var item in model.ShopRequisitionGodownList)
            {
                var shoprequisition = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemDetailsByReqCode(item.ReqCode);
                foreach (var qty in shoprequisition)
                {
                    var details1 = _igodownstockservice.GetRowsByItemCodeandShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                    if (details1 != null)
                    {
                        qty.GodownQuantity = details1.Quantity;
                    }
                    else
                    {
                        qty.GodownQuantity = 0;

                    }
                    newlist.Add(qty);
                }
            }

            IEnumerable<ShopRequisitionGodownItem> finalnewlist = newlist;
            IEnumerable<ShopRequisitionGodownItem> finalnewlist2 = newlist;
            IEnumerable<ShopRequisitionGodownItem> finalnewlist3 = newlist;
            IEnumerable<ShopRequisitionGodownItem> finalnewlist1 = Enumerable.Empty<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue = new List<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue1 = new List<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue2 = new List<ShopRequisitionGodownItem>();
            newlistvalue1 = newlist;

            foreach (var itemvalue in newlistvalue1.GroupBy(m => new { m.ItemCode, m.SRDate, m.FromShopName, m.RequisitionToGodownName }).Select(y => y.First()))
            {

                var details = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemdetailsBiItemCodeAndDateAndRequisitionFromGodownName(itemvalue.ItemCode, itemvalue.SRDate, itemvalue.FromShopName, itemvalue.RequisitionToGodownName);

                if (details != null)
                {
                    double? sum = 0;
                    foreach (var data in details)
                    {
                        sum += data.RequiredQtyForGodown;
                        itemvalue.RequiredQtyForGodown = sum;
                    }
                }

                else
                {
                    itemvalue.RequiredQtyForGodown = itemvalue.RequiredQuantity;
                }
                newlistvalue2.Add(itemvalue);
            }


            foreach (var itemvalue in newlistvalue2)
            {

                string sessiongodownname = Session["SHOPGODOWNNAME"] as string;

                if (itemvalue.FromShopName != sessiongodownname && itemvalue.RequisitionToGodownName == sessiongodownname)
                {
                    model.ShopRequisitionGodownItemList = newlistvalue;
                    newlistvalue.Add(itemvalue);
                }

                else
                {
                    model.ShopRequisitionGodownItemListForNull = null;
                }

            }

            //SalesBill Requisition
            model.ShopRequisitionGodownsalesbillList = _ShopRequisitionGodownSalesBillService.getAllDetails();
            List<ShopRequisitionGodownitemsalesbill> middlelist = new List<ShopRequisitionGodownitemsalesbill>();
            foreach (var item in model.ShopRequisitionGodownsalesbillList)
            {
                var shoprequisition = _iShopRequisitionGodownItemSalesBillService.getShopRequisitionGodownItemDetailsByReqCode(item.ReqCode);
                foreach (var qty in shoprequisition)
                {
                    var details1 = _igodownstockservice.GetRowsByItemCodeandShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                    if (details1 != null)
                    {
                        qty.GodownQuantity = details1.Quantity;
                    }
                    else
                    {
                        qty.GodownQuantity = 0;

                    }
                    middlelist.Add(qty);
                }
            }

            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist = middlelist;
            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist2 = middlelist;
            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist3 = middlelist;
            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist1 = Enumerable.Empty<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue = new List<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue1 = new List<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue2 = new List<ShopRequisitionGodownitemsalesbill>();
            middlelistvalue1 = middlelist;

            foreach (var itemvalue in middlelistvalue1.GroupBy(m => new { m.ItemCode, m.SRDate, m.FromShopName, m.RequisitionFromGodownName }).Select(y => y.First()))
            {
                var details = _iShopRequisitionGodownItemSalesBillService.getShopRequisitionGodownItemdetailsBiItemCodeAndDateAndRequisitionFromGodownName(itemvalue.ItemCode, itemvalue.SRDate, itemvalue.FromShopName, itemvalue.RequisitionFromGodownName);

                if (details != null)
                {
                    double? sum = 0;
                    foreach (var data in details)
                    {
                        sum += data.RequiredQtyForGodown;
                        itemvalue.RequiredQtyForGodown = sum;

                    }
                }

                else
                {
                    itemvalue.RequiredQtyForGodown = itemvalue.RequiredQuantity;
                }
                middlelistvalue2.Add(itemvalue);
            }


            foreach (var itemvalue in middlelistvalue2)
            {
                string sessiongodownname = Session["SHOPGODOWNNAME"] as string;

                if (itemvalue.FromShopName != sessiongodownname && itemvalue.RequisitionFromGodownName == sessiongodownname)
                {
                    model.ShopRequisitionGodownitemsalesbillList = middlelistvalue;
                    middlelistvalue.Add(itemvalue);
                }
                else
                {
                    model.ShopRequisitionGodownitemsalesbillListForNull = null;
                }

            }

            //Independant ShopRequsition
            var detailsofrequisitionforshop = _iRequisitionForShopService.getDetailsOfToGodownIfNotNull();
            List<RequisitionForShop> RequisitionForShopListDetailsvalue = new List<RequisitionForShop>();

            foreach (var qty in detailsofrequisitionforshop)
            {
                var detailofshop = _igodownstockservice.GetRowsByItemCodeandShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                if (detailofshop != null)
                {
                    qty.GodownQty = detailofshop.Quantity;

                }
                else
                {
                    qty.GodownQty = 0;
                }
                RequisitionForShopListDetailsvalue.Add(qty);
            }

            List<RequisitionForShop> lastlist = new List<RequisitionForShop>();
            List<RequisitionForShop> lastlist1 = new List<RequisitionForShop>();
            List<RequisitionForShop> lastlist2 = new List<RequisitionForShop>();
            lastlist1 = RequisitionForShopListDetailsvalue;

            foreach (var itemvalue in lastlist1.GroupBy(m => new { m.ItemCode, m.ModifiedOn, m.LoggedInShopName, m.ToGodownName }).Select(g => g.First()))
            {

                var details1 = _iRequisitionForShopService.getRequisitionForShopdetailsBiItemCodeAndDateForGodown(itemvalue.ItemCode, itemvalue.ModifiedOn, itemvalue.LoggedInShopName, itemvalue.ToGodownName);
                if (details1 != null)
                {
                    double? sum = 0;
                    foreach (var data in details1)
                    {
                        sum += data.ReqQuantity;
                        itemvalue.ReqQuantity = sum;
                    }

                }

                else
                {
                    itemvalue.ReqQuantity = itemvalue.ReqQuantity;
                }
                lastlist2.Add(itemvalue);
            }


            foreach (var itemvalue in lastlist2)
            {

                if (itemvalue.LoggedInShopName != Session["SHOPGODOWNNAME"] as string && itemvalue.ToGodownName == Session["SHOPGODOWNNAME"] as string)
                {
                    model.RequisitionForShopList = lastlist;
                    lastlist.Add(itemvalue);
                }

                else
                {
                    model.RequisitionForShopListForNull = null;
                }
            }

            //Independant godown requisition
            var detailsofrequisitionforgodown = _iRequisitionforgodownservice.getDetailsOfToGodownIfNotNull();
            List<RequisitionForGodown> RequisitionForGodownDetailsValue = new List<RequisitionForGodown>();

            foreach (var qty in detailsofrequisitionforgodown)
            {
                var detailofgodown = _igodownstockservice.GetRowsByItemCodeandShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                if (detailofgodown != null)
                {
                    qty.GodownQty = detailofgodown.Quantity;

                }
                else
                {
                    qty.GodownQty = 0;
                }
                RequisitionForGodownDetailsValue.Add(qty);
            }

            List<RequisitionForGodown> firstgodownlist = new List<RequisitionForGodown>();
            List<RequisitionForGodown> firstgodownlist1 = new List<RequisitionForGodown>();
            List<RequisitionForGodown> firstgodownlist2 = new List<RequisitionForGodown>();
            firstgodownlist1 = RequisitionForGodownDetailsValue;

            foreach (var godownitemvalue in firstgodownlist1.GroupBy(m => new { m.ItemCode, m.ModifiedOn, m.LoggedInGodownName, m.ToGodownName }).Select(g => g.First()))
            {

                var detailsgodown1 = _iRequisitionforgodownservice.getRequisitionForGodownDetailsiItemCodeAndDate(godownitemvalue.ItemCode, godownitemvalue.ModifiedOn, godownitemvalue.LoggedInGodownName, godownitemvalue.ToGodownName);
                if (detailsgodown1 != null)
                {
                    double? sumvaluegodown = 0;
                    foreach (var data in detailsgodown1)
                    {
                        sumvaluegodown += data.RequiredQuantity;
                        godownitemvalue.RequiredQuantity = sumvaluegodown;
                    }

                }

                else
                {
                    godownitemvalue.RequiredQuantity = godownitemvalue.RequiredQuantity;
                }
                firstgodownlist2.Add(godownitemvalue);
            }


            foreach (var itemvaluegodown in firstgodownlist2)
            {

                if (itemvaluegodown.LoggedInGodownName != Session["SHOPGODOWNNAME"] as string && itemvaluegodown.ToGodownName == Session["SHOPGODOWNNAME"] as string)
                {
                    model.Requisitionforgodownlist = firstgodownlist;
                    firstgodownlist.Add(itemvaluegodown);
                }

                else
                {
                    model.RequisitionforgodownlistNull = null;
                }
            }


            model.GodownStockList = _igodownstockservice.GetRowsByGodowCode(godownname);
            model.GodownMasterList = _igodownservive.GetGodownNamesExcludingOne(Session["SHOPGODOWNNAME"].ToString());
            return View(model);
        }

        [HttpPost]
        public ActionResult GodownRequisitionForGodownLoginCreate(MainApplication mainapp, FormCollection frm)
        {
            MainApplication model = new MainApplication()
            {

                ShopRequisitionGodown = new ShopRequisitionGodown(),
                usercredential = new UserCredential(),
                EmployeeDetails = new EmployeeMaster(),

                ShopDetails = new ShopMaster(),
                GodownDetails = new GodownMaster(),
            };
            string gdcode = Session["LOGINSHOPGODOWNCODE"] as string;
            //generating code for outwardtoshop
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var shoptoshop = _iOutwardToShopService.GetLastOutwardByFinYr(FinYr, gdcode);
            string code1 = string.Empty;
            string code = string.Empty;
            int length = 0;
            int number = 0;
            if (shoptoshop != null)
            {
                string trim = shoptoshop.OutwardCode;
                int index = trim.LastIndexOf('O');
                code1 = shoptoshop.OutwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(code1) + 1).ToString().Length;
                number = Convert.ToInt32(code1) + 1;
            }
            else
            {
                length = 1;
                number = 1;
            }
            string godowname = Session["SHOPGODOWNNAME"] as string;
            var gddetails1 = _igodownservive.GetGodownDetailsByName(godowname);
            string utilityname = gddetails1.ShortCode + "/OWGS";
            code = _utilityservice.getName(utilityname, length, number);
            code = code + FinYr;

            //generating code for outwardtogodown
            string year1 = FinancialYear;
            string[] yr1 = year1.Split(' ', '-');
            string FinYr1 = "/" + yr1[2].Substring(2) + "-" + yr1[6].Substring(2);
            var data = _outwardInterGodownService.GetLastOutwardByFinYr(FinYr1, gdcode);
            string outwardcode1 = string.Empty;
            string outwardcode = string.Empty;
            int length1 = 0;
            int number1 = 0;
            if (data != null)
            {
                string trim = data.OutwardCode;
                int index = trim.LastIndexOf('O');
                outwardcode1 = data.OutwardCode.Substring(index + 4, 6);
                length1 = (Convert.ToInt32(outwardcode1) + 1).ToString().Length;
                number1 = Convert.ToInt32(outwardcode1) + 1;
            }
            else
            {
                length1 = 1;
                number1 = 1;
            }
            string gdnname = Session["SHOPGODOWNNAME"] as string;
            var gddetails = _igodownservive.GetGodownDetailsByName(gdnname);
            var utilityname1 = gddetails.ShortCode + "/OWGG";
            outwardcode = _utilityservice.getName(utilityname1, length1, number1);
            outwardcode = outwardcode + FinYr1;


            RequisitionForGodown item = new RequisitionForGodown();
            string count = "hdnrowcount";
            int finalcount = Convert.ToInt32(frm[count]);

            var detailsvalue = default(ShopRequisitionGodownItem);
            var detailsvalue1 = default(RequisitionForShop);
            var detailsvaluesalesbill = default(ShopRequisitionGodownitemsalesbill);
            var detailsvalue1godownupdate = default(RequisitionForGodown);
            var shopdetails1 = default(ShopMaster);
            var godowndetails1 = default(GodownMaster);
            int counter = 1;
            for (int i = 1; i <= (finalcount) - 1; i++)
            {
                string itemname = "itemname" + i;
                string itemcode = "itemcode" + i;
                string itemdescription = "itemdescription" + i;
                string itemquantity = "itemquantity" + i;
                string requiredquantity = "requiredquantity" + i;
                string requisitionfrom = "requisitionfromshopname" + i;
                string rejectquantity = "rejectquantity" + i;
                string codevalue = "itemcodevalue" + i;
                string PrepareBy = "PrepareBy" + i;
                string EmailIdEmployee = "EmailIdEmployee" + i;
                string ContactNoEmployee = "ContactNoEmployee" + i;
                string outwardquantity = "outwardquantity" + i;
                string balancevalue = "balancevalue" + i;
                string datevalue = "requisitiondate" + i;
                string finalitemname = frm[itemname];

                if (frm[codevalue] != null)
                {
                    item.codevaluerequisition = frm[codevalue];
                }
                if (frm[outwardquantity] != "")
                {
                    item.Status = "Outward";
                    item.OutwardQuantity = Convert.ToDouble(frm[outwardquantity]);
                }
                else
                {
                    item.OutwardQuantity = null;
                }
                if (frm[balancevalue] != "0")
                {
                    item.Balance = Convert.ToDouble(frm[balancevalue]);
                }
                else
                {
                    item.Balance = null;
                }
                if (frm[datevalue] != "0")
                {
                    item.RequsitionDate = Convert.ToDateTime(frm[datevalue]);
                    if (item.RequsitionDate == Convert.ToDateTime("1/1/0001"))
                    {
                        item.RequsitionDate = null;
                    }
                }
                else
                {
                    item.RequsitionDate = DateTime.Now;
                }

                if (frm[requiredquantity] != "")
                {
                    item.RequiredQuantity = Convert.ToDouble(frm[requiredquantity]);
                }
                else
                {
                    item.RequiredQuantity = null;
                }
                if (frm[rejectquantity] != "")
                {
                    item.RejectQuantity = Convert.ToDouble(frm[rejectquantity]);
                }
                else
                {
                    item.RejectQuantity = null;
                }

                item.ItemName = finalitemname;
                item.ItemCode = frm[itemcode];
                item.Category = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemCategory;
                item.SubCategory = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemSubCategory;
                item.Description = frm[itemdescription];
                item.QuantityInStock = Convert.ToDouble(frm[itemquantity]);
                item.RequisitionFrom = frm[requisitionfrom];
                item.PrepareBy = mainapp.RequisitionForGodownDetails.RequisitionForGodownPrepareBy;
                item.EmailIdEmployee = mainapp.RequisitionForGodownDetails.RequisitionForGodownEmailIdEmployee;
                item.ContactNoEmployee = mainapp.RequisitionForGodownDetails.RequisitionForGodownContactNoEmployee;
                item.LoggedInGodownName = Session["SHOPGODOWNNAME"] as string;

                item.RequisitionForGodownPrepareBy = mainapp.RequisitionForGodownDetails.RequisitionForGodownPrepareBy;
                item.RequisitionForGodownEmailIdEmployee = mainapp.RequisitionForGodownDetails.RequisitionForGodownEmailIdEmployee;
                item.RequisitionForGodownContactNoEmployee = mainapp.RequisitionForGodownDetails.RequisitionForGodownContactNoEmployee;
                item.RequisitionForGodownPrepareTimeEmployee = DateTime.Now;
                item.PrepareTimeEmployee = DateTime.Now;
                item.ModifiedOn = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());
                item.EmpName = mainapp.RequisitionForGodownDetails.EmpName;
                item.EmpDesignation = mainapp.RequisitionForGodownDetails.EmpDesignation;
                item.EmpContactNo = mainapp.RequisitionForGodownDetails.EmpContactNo;
                item.EmpEmail = mainapp.RequisitionForGodownDetails.EmailIdEmployee;
                item.ReqCode = mainapp.RequisitionForGodownDetails.ReqCode;

                model.EmployeeDetails = _EmployeeMasterService.getEmpByEmail(item.EmailIdEmployee);
                if (model.EmployeeDetails != null)
                {
                    item.DesignationEmployee = model.EmployeeDetails.Designation;
                }
                else
                {
                    item.DesignationEmployee = "null";
                }

                if (item.RequisitionFrom == "null")
                {
                    item.Status = "Requisition";
                    item.RequisitionFrom = item.LoggedInGodownName;
                    item.ToGodownName = mainapp.RequisitionForShopDetails.ToGodownName;
                    if (frm["RadioBtn"] == null)
                    {
                        item.RequestToPo = "No";
                    }
                    else
                    {
                        item.RequestToPo = frm["RadioBtn"];
                    }
                }
                else
                {
                    if (item.RequisitionFrom != "null")
                    {
                        item.ToGodownName = "null";
                        item.RequestToPo = "null";
                    }
                }

                SmtpClient client = new SmtpClient("180.149.243.22");
                client.Port = 587;
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
                string body = string.Empty;
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
                message.From = messageFrom;
                message.Subject = "Shop Requisition List";
                body += "Dear Purchase Department ,<br /><br />";
                body += "\t There is a shortage of following items";
                body += "<table border=1><tr><th style=text-align:center>FromShopName</th><th style=text-align:center>ItemCode</th><th style=text-align:center>ItemName</th><th style=text-align:center>Quantity Required</th> </tr> <tr><td style=text-align:center>" + item.LoggedInGodownName + "</td><td style=text-align:center>" + item.ItemCode + "</td><td>" + item.ItemName + "</td><td style=text-align:center>" + item.RequiredQuantity + "</td></tr> </table>";
                message.Body = body;
                MailAddress messageTo = new MailAddress("athulyanair91@gmail.com");
                message.To.Add(messageTo);
                client.Send(message);
                message.Dispose();

                if (item.RequiredQuantity != null && item.OutwardQuantity != null)
                {
                    _iRequisitionforgodownservice.CreateRequisitionForGodown(item);
                }

                if (item.OutwardQuantity != null && item.OutwardQuantity != 0)
                {
                    shopdetails1 = _iShopService.GetShopDetailsByName(item.RequisitionFrom);
                    if (shopdetails1 == null)
                    {
                        godowndetails1 = _igodownservive.GetGodownDetailsByName(item.RequisitionFrom);
                    }

                    if (shopdetails1 != null)
                    {
                        OutwardToShop outwardtoshopnew = new OutwardToShop();
                        OutwardItemToShop outwarditemtoshopnew = new OutwardItemToShop();
                        outwardtoshopnew.OutwardCode = code;
                        EmployeeMaster emp = _EmployeeMasterService.getEmpByEmail(mainapp.RequisitionForGodownDetails.EmailIdEmployee);
                        outwardtoshopnew.EmpName = mainapp.RequisitionForGodownDetails.EmpName;
                        outwardtoshopnew.EmpDesignation = mainapp.RequisitionForGodownDetails.EmpDesignation;
                        outwardtoshopnew.EmpEmail = mainapp.RequisitionForGodownDetails.EmailIdEmployee;
                        outwardtoshopnew.EmpContactNo = mainapp.RequisitionForGodownDetails.EmpContactNo;
                        outwardtoshopnew.SalesmanName = mainapp.RequisitionForGodownDetails.RequisitionForGodownPrepareBy;
                        outwardtoshopnew.SalesmanDesignation = emp.Designation;
                        outwardtoshopnew.SalesmanContactNo = mainapp.RequisitionForGodownDetails.RequisitionForGodownContactNoEmployee;
                        outwardtoshopnew.SalesmanEmail = mainapp.RequisitionForGodownDetails.RequisitionForGodownEmailIdEmployee;
                        outwardtoshopnew.GodownName = Session["SHOPGODOWNNAME"] as string;
                        outwardtoshopnew.Date = DateTime.Now;
                        outwardtoshopnew.ShopName = item.RequisitionFrom;
                        var shopdetails = _iShopService.GetShopDetailsByName(item.RequisitionFrom);

                        outwardtoshopnew.ShopEmail = shopdetails.ShopEmail;
                        outwardtoshopnew.ShopContactNo = shopdetails.ShopContactNo;
                        outwardtoshopnew.ShopAddress = shopdetails.ShopAddress;
                        outwardtoshopnew.ShopCode = shopdetails.ShopCode;

                        string godownname = Session["SHOPGODOWNNAME"] as string;
                        var godowndetails = _igodownservive.GetGodownDetailsByName(godownname);
                        outwardtoshopnew.GodownCode = godowndetails.GdCode;
                        outwardtoshopnew.ShopCode = outwardtoshopnew.ShopCode;
                        outwardtoshopnew.PrepaidBy = mainapp.RequisitionForGodownDetails.RequisitionForGodownPrepareBy;
                        outwardtoshopnew.Status = "Active";
                        outwardtoshopnew.ModifiedOn = DateTime.Now;
                        Random rnd = new Random();
                        string gatepassno = Convert.ToString(rnd.Next(1, 1000));

                        outwardtoshopnew.GatePass = Convert.ToString(gatepassno);
                        TempData["gatepass"] = gatepassno;
                        outwardtoshopnew.Narration = "Note";

                        var shopstockdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(item.ItemCode, outwardtoshopnew.ShopName);

                        outwarditemtoshopnew.OutwardCode = outwardtoshopnew.OutwardCode;
                        outwarditemtoshopnew.ItemName = item.ItemName;
                        outwarditemtoshopnew.ItemCode = item.ItemCode;
                        outwarditemtoshopnew.ItemDescription = item.Description;
                        outwarditemtoshopnew.StockQuantity = Convert.ToDouble(item.QuantityInStock);

                        outwarditemtoshopnew.OutwardQuantity = item.OutwardQuantity;
                        outwarditemtoshopnew.CurrentBalance = Convert.ToString(item.QuantityInStock - item.OutwardQuantity);
                        outwarditemtoshopnew.Status = "Active";
                        outwarditemtoshopnew.ModifiedOn = DateTime.Now;
                        outwarditemtoshopnew.GodownCode = outwardtoshopnew.GodownCode;
                        outwarditemtoshopnew.ShopCode = outwardtoshopnew.ShopCode;
                        outwarditemtoshopnew.Material = shopstockdetails.Material;
                        outwarditemtoshopnew.Color = shopstockdetails.Color;
                        outwarditemtoshopnew.DesignCode = shopstockdetails.DesignCode;
                        outwarditemtoshopnew.Unit = shopstockdetails.Unit;
                        outwarditemtoshopnew.Barcode = shopstockdetails.Barcode;
                        outwarditemtoshopnew.Brand = shopstockdetails.Brand;
                        outwarditemtoshopnew.DesignName = shopstockdetails.DesignName;
                        outwarditemtoshopnew.NumberType = shopstockdetails.NumberType;
                        outwarditemtoshopnew.Size = shopstockdetails.Size;
                        outwarditemtoshopnew.SellingPrice = Convert.ToDouble(shopstockdetails.SellingPrice);
                        outwarditemtoshopnew.MRP = Convert.ToDouble(shopstockdetails.MRP);
                        var outwardintershopcodevalue = _iOutwardToShopService.GetLastRow();
                        
                        if (counter == 1)
                        {
                            _iOutwardToShopService.Create(outwardtoshopnew);
                            counter++;
                        }
                        TempData["outwardtoshopnew"] = outwardtoshopnew;

                        //To Update Required Qty
                        if (item.codevaluerequisition.Contains("RE"))
                        {
                            detailsvalue = _ishoprequisitiongodownitemservice.GetDetailsByItemCodeAndFromShopNameForGodownLogin(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                        }
                        if (item.codevaluerequisition.Contains("IIS"))
                        {
                            detailsvalue1 = _iRequisitionForShopService.GetDetailsByIcAndfsAndDTAndRSForGodown(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);

                        }

                        if (item.codevaluerequisition.Contains("SB"))
                        {
                            detailsvaluesalesbill = _iShopRequisitionGodownItemSalesBillService.GetDetailsByItemCodeAndFromShopNameForGodownLogin(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                        }

                        //to update retail bill
                        if (detailsvalue != null)
                        {
                            detailsvalue.RequiredQtyForGodown = Convert.ToDouble(frm[balancevalue]);

                            if (detailsvalue.RequiredQtyForGodown == 0)
                            {
                                var detailsienumerable = _ishoprequisitiongodownitemservice.GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerable(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                                foreach (var value in detailsienumerable)
                                {
                                    value.RequiredQtyForGodown = 0;
                                    _ishoprequisitiongodownitemservice.Update(detailsvalue);
                                }
                            }
                            else
                            {
                                var detailsienumerable1 = _ishoprequisitiongodownitemservice.GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerableSkipFirst(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                                foreach (var deienumerable1 in detailsienumerable1)
                                {
                                    deienumerable1.RequiredQtyForGodown = 0;
                                    _ishoprequisitiongodownitemservice.Update(deienumerable1);
                                }
                            }
                            _ishoprequisitiongodownitemservice.Update(detailsvalue);
                        }

                        //to update independant shop requisition
                        if (detailsvalue1 != null)
                        {
                            detailsvalue1.RequiredQtyForGodown = Convert.ToDouble(frm[balancevalue]);

                            if (detailsvalue1.RequiredQtyForGodown == 0)
                            {
                                var detailsienumerable2 = _iRequisitionForShopService.GetDetailsByICAndFShNameIenumerableForGodown(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                                foreach (var value in detailsienumerable2)
                                {
                                    value.RequiredQtyForGodown = 0;
                                    _iRequisitionForShopService.Update(detailsvalue1);
                                }
                            }
                            else
                            {
                                var detailsienumerable3 = _iRequisitionForShopService.GetDetailsByICodeAndSNIenumerableSkipFirstForGodown(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);

                                foreach (var deienumerable3 in detailsienumerable3)
                                {
                                    deienumerable3.RequiredQtyForGodown = 0;
                                    _iRequisitionForShopService.Update(deienumerable3);
                                }
                            }
                            _iRequisitionForShopService.Update(detailsvalue1);
                        }

                        //to update sales bill
                        if (detailsvaluesalesbill != null)
                        {
                            detailsvaluesalesbill.RequiredQtyForGodown = Convert.ToDouble(frm[balancevalue]);

                            if (detailsvaluesalesbill.RequiredQtyForGodown == 0)
                            {
                                var detailsienumerable = _iShopRequisitionGodownItemSalesBillService.GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerable(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                                foreach (var value in detailsienumerable)
                                {
                                    value.RequiredQtyForGodown = 0;
                                    _iShopRequisitionGodownItemSalesBillService.Update(detailsvaluesalesbill);
                                }
                            }
                            else
                            {
                                var detailsienumerable1 = _iShopRequisitionGodownItemSalesBillService.GetDetailsByItemCodeAndFromShopNameForGodownLoginIenumerableSkipFirst(outwarditemtoshopnew.ItemCode, outwardtoshopnew.ShopName, item.RequsitionDate, godownname);
                                foreach (var deienumerable1 in detailsienumerable1)
                                {
                                    deienumerable1.RequiredQtyForGodown = 0;
                                    _iShopRequisitionGodownItemSalesBillService.Update(deienumerable1);
                                }
                            }
                            _iShopRequisitionGodownItemSalesBillService.Update(detailsvaluesalesbill);
                        }
                        _iOutwardItemToShopService.Create(outwarditemtoshopnew);

                        //Minus from Stock Item Distribution
                        model.StockItemDistributionDetails = _iStockItemDistributionService.GetLastRowFromItemAndGodownCode(outwarditemtoshopnew.ItemCode, outwardtoshopnew.GodownCode);

                        model.StockItemDistributionDetails.ItemQuantity = model.StockItemDistributionDetails.ItemQuantity - outwarditemtoshopnew.OutwardQuantity;
                        _iStockItemDistributionService.Update(model.StockItemDistributionDetails);

                        //Minus from Godown Stock
                        var godownstockdata = _igodownstockservice.GetDetailsByItemCodeAndGodownCode(outwarditemtoshopnew.ItemCode, outwardtoshopnew.GodownCode);
                        godownstockdata.Quantity -= Convert.ToDouble(outwarditemtoshopnew.OutwardQuantity);
                        _igodownstockservice.Update(godownstockdata);
                    }

                    else if (godowndetails1 != null)
                    {
                        OutwardInterGodown outwardintergodown = new OutwardInterGodown();
                        OutwardItemInterGodown outwarditemintergodown = new OutwardItemInterGodown();
                        outwardintergodown.OutwardCode = outwardcode;
                        outwardintergodown.OutwardDate = DateTime.Now;
                        outwardintergodown.FromGodownname = Session["SHOPGODOWNNAME"] as string;
                        var outwardgodowndetails = _igodownservive.GetGodownDetailsByName(outwardintergodown.FromGodownname);

                        outwardintergodown.FromGodownCode = outwardgodowndetails.GdCode;
                        outwardintergodown.FromGodownContactNo = outwardgodowndetails.ContactNo1;
                        outwardintergodown.FromGodownEmail = outwardgodowndetails.GodownEmail;
                        outwardintergodown.FromGodownContactPerson = outwardgodowndetails.EmpName;

                        outwardintergodown.ToGodownname = item.RequisitionFrom;
                        var outwardgodowndetailsto = _igodownservive.GetGodownDetailsByName(outwardintergodown.ToGodownname);
                        outwardintergodown.ToGodownCode = outwardgodowndetailsto.GdCode;

                        var detailsgodownaddress = _iGodownAddressService.GetAllAddressesByCode(outwardintergodown.ToGodownCode);
                        foreach (var address in detailsgodownaddress)
                        {
                            outwardintergodown.ToDeliveryAt = address.Address;
                        }
                        outwardintergodown.ToGodownContactNo = outwardgodowndetailsto.ContactNo1;
                        outwardintergodown.ToGodownEmail = outwardgodowndetailsto.GodownEmail;
                        outwardintergodown.ToGodownContactPerson = outwardgodowndetailsto.EmpName;

                        outwardintergodown.Status = "Active";
                        outwardintergodown.ModifiedOn = DateTime.Now;

                        outwardintergodown.PrepareTime = DateTime.Now;
                        outwardintergodown.PrepareBy = mainapp.RequisitionForGodownDetails.RequisitionForGodownPrepareBy;
                        outwardintergodown.PrepareByContactNo = mainapp.RequisitionForGodownDetails.RequisitionForGodownContactNoEmployee;
                        outwardintergodown.PrepareByEmail = mainapp.RequisitionForGodownDetails.RequisitionForGodownEmailIdEmployee;
                        outwardintergodown.FromGodownStoreKeeperName = mainapp.RequisitionForGodownDetails.RequisitionForGodownPrepareBy;
                        EmployeeMaster emp = _EmployeeMasterService.getEmpByEmail(mainapp.RequisitionForGodownDetails.RequisitionForGodownEmailIdEmployee);
                        outwardintergodown.FromGodownStoreKeeperDesignation = emp.Designation;
                        outwardintergodown.FromGodownStoreKeeperContactNo = mainapp.RequisitionForGodownDetails.RequisitionForGodownContactNoEmployee;
                        outwardintergodown.FromGodownStoreKeeperEmail = mainapp.RequisitionForGodownDetails.RequisitionForGodownEmailIdEmployee;
                        Random rnd = new Random();
                        string gatepassno = Convert.ToString(rnd.Next(1, 1000));
                        outwardintergodown.GatePass = Convert.ToString(gatepassno);
                        TempData["gatepass"] = gatepassno;

                        outwardintergodown.Narration = "Note";

                        var outwardintergodowncode = _outwardInterGodownService.GetLastRow();
                        _outwardInterGodownService.Create(outwardintergodown);
                        TempData["PreviousGodownRequisition"] = outwardintergodown;

                        var godowndetails = _igodownstockservice.GetDetailsByItemCodeAndGodownCode(item.ItemCode, outwardintergodown.ToGodownCode);
                        outwarditemintergodown.Barcode = godowndetails.Barcode;
                        outwarditemintergodown.ItemCode = item.ItemCode;
                        outwarditemintergodown.ItemName = item.ItemName;
                        outwarditemintergodown.Description = item.Description;
                        outwarditemintergodown.QuantityTransfer = Convert.ToString(item.OutwardQuantity);
                        outwarditemintergodown.TypeOfMaterial = godowndetails.Material;
                        outwarditemintergodown.ColorCode = godowndetails.Color;
                        outwarditemintergodown.DesignCode = godowndetails.DesignCode;
                        outwarditemintergodown.DesignName = godowndetails.DesignName;
                        outwarditemintergodown.Unit = godowndetails.Unit;
                        outwarditemintergodown.NumberType = godowndetails.NumberType;
                        outwarditemintergodown.SellingPrice = Convert.ToDouble(godowndetails.SellingPrice);
                        outwarditemintergodown.MRP = Convert.ToDouble(godowndetails.MRP);
                        outwarditemintergodown.OutwardCode = outwardintergodown.OutwardCode;
                        outwarditemintergodown.Balance = Convert.ToString(item.QuantityInStock - item.OutwardQuantity);
                        outwarditemintergodown.Size = godowndetails.Size;
                        outwarditemintergodown.Brand = godowndetails.Brand;
                        outwarditemintergodown.FromGdCode = outwardintergodown.FromGodownCode;
                        outwarditemintergodown.ToGdCode = outwardintergodown.ToGodownCode;
                        outwarditemintergodown.Status = "Active";
                        outwarditemintergodown.ModifiedOn = DateTime.Now;
                        _outwardItemInterGodownService.Create(outwarditemintergodown);

                        //To Update Required Qty
                        if (item.codevaluerequisition.Contains("IIG"))
                        {
                            detailsvalue1godownupdate = _iRequisitionforgodownservice.GetDetailsByIcAndfsAndDTAndRSForGodownUpdate(outwarditemintergodown.ItemCode, item.RequisitionFrom, item.RequsitionDate, outwardintergodown.FromGodownname);
                        }

                        //To update independant godown
                        if (detailsvalue1godownupdate != null)
                        {
                            detailsvalue1godownupdate.RequiredQuantity = Convert.ToDouble(frm[balancevalue]);

                            if (detailsvalue1godownupdate.RequiredQuantity == 0)
                            {
                                var detailsienumerablegodown = _iRequisitionforgodownservice.GetDetailsByICAndFShNameIenumerableForRequisitionOfgodown(outwarditemintergodown.ItemCode, item.RequisitionFrom, item.ModifiedOn, outwardintergodown.FromGodownname);
                                foreach (var value in detailsienumerablegodown)
                                {
                                    value.RequiredQuantity = 0;
                                    _iRequisitionforgodownservice.Update(detailsvalue1godownupdate);
                                }
                            }
                            else
                            {
                                var detailsienumerablegodown1 = _iRequisitionforgodownservice.GetDetailsByICodeAndSNIenumerableSkipFirstForGodownUpdate(outwarditemintergodown.ItemCode, item.RequisitionFrom, item.ModifiedOn, outwardintergodown.FromGodownname);
                                foreach (var deienumerable1 in detailsienumerablegodown1)
                                {
                                    deienumerable1.RequiredQuantity = 0;
                                    _iRequisitionforgodownservice.Update(deienumerable1);
                                }
                            }
                            _iRequisitionforgodownservice.Update(detailsvalue1godownupdate);
                        }

                        var fromstockdist = _iStockItemDistributionService.GetDetailsByItemCodeAndGodownCode(item.ItemCode, outwarditemintergodown.FromGdCode);
                        var gdstocks = _igodownstockservice.GetDetailsByItemCodeAndGodownCode(item.ItemCode, outwarditemintergodown.FromGdCode);

                        fromstockdist.ItemQuantity -= item.OutwardQuantity;
                        fromstockdist.ModifiedOn = DateTime.Now;
                        _iStockItemDistributionService.Update(fromstockdist);

                        gdstocks.Quantity -= item.OutwardQuantity;
                        gdstocks.ModifiedOn = DateTime.Now;
                        _igodownstockservice.Update(gdstocks);

                    }
                }

            }

            if (shopdetails1 != null)
            {
                var godowntoshopdetails = _iOutwardToShopService.GetLastRow();
                int Id = godowntoshopdetails.OutwardId;
                var encoded = Encode(Id.ToString());
                return RedirectToAction("OutwardToShopDetails/" + encoded, "Requisition");
            }
            else if (godowndetails1 != null)
            {
                var id = _outwardInterGodownService.GetLastRow().OutwardId;
                int Id = id;
                var encoded = Encode(Id.ToString());
                return RedirectToAction("OutwardInterGodownDetails/" + encoded, "Requisition");
            }
            else
                return RedirectToAction("RequisitionFullfilled");
        }


        //Encoding Function
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }
        //Decoding Function
        public int Decode(string decodeMe)
        {
            byte[] decoded = Convert.FromBase64String(decodeMe);
            string decodedvalue = System.Text.Encoding.UTF8.GetString(decoded);
            return Convert.ToInt32(decodedvalue);
        }
        [HttpGet]
        public ActionResult OutwardInterGodownDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                OutwardInterGodownDetails = new OutwardInterGodown(),
            };
            model.RequisitionForGodownDetails = new RequisitionForGodown();


            byte[] encoded = Convert.FromBase64String(id);

            //Decoding the encoded value i.e retaining the original value of id
            string decodedvalue = System.Text.Encoding.UTF8.GetString(encoded);
            int idvalue = Convert.ToInt32(decodedvalue);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.OutwardInterGodownDetails = _outwardInterGodownService.GetDetailsByOutwardId(idvalue);
            model.OutwardItemInterGodownList = _outwardItemInterGodownService.GetDetailsByOutwardCode(model.OutwardInterGodownDetails.OutwardCode);
            string previousshopoutward = TempData["PreviousGodownRequisition"].ToString();
            if (previousshopoutward != model.RequisitionForGodownDetails.ReqCode)
            {
                ViewData["PreviousGodownRequisitionViewdata"] = previousshopoutward + " is replaced with " + model.RequisitionForGodownDetails.ReqCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousGodownRequisition"] = previousshopoutward;
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardToShopDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.RequisitionForGodownDetails = new RequisitionForGodown();

            OutwardToShop outwardtoshop = new OutwardToShop();
            byte[] encoded = Convert.FromBase64String(id);

            //Decoding the encoded value i.e retaining the original value of id

            int idvalue = Decode(id);
            string outwardtoshopvalue = TempData["outwardtoshopnew"].ToString();
            model.OutwardToShopDetails = _iOutwardToShopService.GetDetailsbyId(idvalue);
            model.OutwardItemToShopList = _iOutwardItemToShopService.GetDetailsByOutwardNo(model.OutwardToShopDetails.OutwardCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string previousshopoutward = TempData["PreviousGodownRequisition"].ToString();
            if (previousshopoutward != model.RequisitionForGodownDetails.ReqCode)
            {
                ViewData["PreviousGodownRequisitionViewdata"] = previousshopoutward + " is replaced with " + model.RequisitionForGodownDetails.ReqCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousGodownRequisition"] = previousshopoutward;
            return View(model);
        }


        [HttpGet]
        public ActionResult RequisitionOfPO()
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
                usercredential = new UserCredential(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            string shopcodevalue = string.Empty;

            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            shopcodevalue = assigndetails.AssignRightsCode;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            //RetailBill Requisition
            model.ShopRequisitionGodownList = _ishoprequisitiongodownservice.getListByPo("Yes");
            List<ShopRequisitionGodownItem> newlist = new List<ShopRequisitionGodownItem>();

            foreach (var item in model.ShopRequisitionGodownList)
            {
                var shoprequisition = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemDetailsByReqCode(item.ReqCode);
                foreach (var qty in shoprequisition)
                {
                    newlist.Add(qty);
                }
            }
            List<ShopRequisitionGodownItem> newlistvalue1 = new List<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue2 = new List<ShopRequisitionGodownItem>();
            newlistvalue1 = newlist;
            foreach (var itemvalue in newlistvalue1.GroupBy(m => new { m.ItemCode, m.SRDate, m.FromShopName, m.RequisitionToShopName }).Select(y => y.First()))
            {
                var details = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemdetailsBiItemCodeAndDateForPo(itemvalue.ItemCode, itemvalue.SRDate, itemvalue.FromShopName, itemvalue.RequisitionToShopName);

                if (details != null)
                {
                    double? sum = 0;
                    foreach (var data in details)
                    {
                        sum += data.RequiredQuantity;
                        itemvalue.RequiredQuantity = sum;
                    }
                }

                else
                {
                    itemvalue.RequiredQuantity = itemvalue.RequiredQuantity;

                }
                newlistvalue2.Add(itemvalue);
            }

            model.ShopRequisitionGodownItemList = newlistvalue2;

            //SalesBill Requisition
            model.ShopRequisitionGodownsalesbillList = _ShopRequisitionGodownSalesBillService.getListByPo("Yes");
            List<ShopRequisitionGodownitemsalesbill> middlelist = new List<ShopRequisitionGodownitemsalesbill>();

            foreach (var item in model.ShopRequisitionGodownsalesbillList)
            {
                var shoprequisition = _iShopRequisitionGodownItemSalesBillService.getShopRequisitionGodownItemDetailsByReqCode(item.ReqCode);
                foreach (var qty in shoprequisition)
                {
                    middlelist.Add(qty);
                }
            }
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue1 = new List<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue2 = new List<ShopRequisitionGodownitemsalesbill>();
            middlelistvalue1 = middlelist;
            foreach (var itemvalue in middlelistvalue1.GroupBy(m => new { m.ItemCode, m.SRDate, m.FromShopName, m.RequisitionFromShopName }).Select(y => y.First()))
            {
                var details = _iShopRequisitionGodownItemSalesBillService.getShopRequisitionGodownItemdetailsBiItemCodeAndDateForPo(itemvalue.ItemCode, itemvalue.SRDate, itemvalue.FromShopName, itemvalue.RequisitionFromShopName);

                if (details != null)
                {
                    double? sum = 0;
                    foreach (var data in details)
                    {
                        sum += data.RequiredQuantity;
                        itemvalue.RequiredQuantity = sum;
                    }
                }

                else
                {
                    itemvalue.RequiredQuantity = itemvalue.RequiredQuantity;

                }
                middlelistvalue2.Add(itemvalue);

            }
            model.ShopRequisitionGodownitemsalesbillList = middlelistvalue2;

            //IndependantGodown Requisition
            List<RequisitionForGodown> newlistvalue3 = new List<RequisitionForGodown>();
            List<RequisitionForGodown> newlistvalue4 = new List<RequisitionForGodown>();

            model.Requisitionforgodownlist = _iRequisitionforgodownservice.GetRequisitionsForPo("Yes");

            newlistvalue3 = model.Requisitionforgodownlist.ToList();
            foreach (var godownitemvalue in newlistvalue3.GroupBy(m => new { m.ItemCode, m.ModifiedOn, m.LoggedInGodownName, m.ToGodownName }).Select(g => g.First()))
            {

                var detailsgodown1 = _iRequisitionforgodownservice.getRequisitionForGodownDetailsiItemCodeAndDate(godownitemvalue.ItemCode, godownitemvalue.ModifiedOn, godownitemvalue.LoggedInGodownName, godownitemvalue.ToGodownName);
                if (detailsgodown1 != null)
                {
                    double? sumvaluegodown = 0;
                    foreach (var data in detailsgodown1)
                    {
                        sumvaluegodown += data.RequiredQuantity;
                        godownitemvalue.RequiredQuantity = sumvaluegodown;
                    }

                }

                else
                {
                    godownitemvalue.RequiredQuantity = godownitemvalue.RequiredQuantity;
                }
                newlistvalue4.Add(godownitemvalue);
            }

            model.Requisitionforgodownlist = newlistvalue4;

            //Independant ShopRequisition
            List<RequisitionForShop> newlistvalue5 = new List<RequisitionForShop>();
            List<RequisitionForShop> newlistvalue6 = new List<RequisitionForShop>();

            model.RequisitionForShopList = _iRequisitionForShopService.GetRequisitionsForPo("Yes");
            newlistvalue5 = model.RequisitionForShopList.ToList();

            foreach (var itemvalue in newlistvalue5.GroupBy(m => new { m.ItemCode, m.ModifiedOn, m.LoggedInShopName, m.ToShopName }).Select(g => g.First()))
            {

                var details1 = _iRequisitionForShopService.getRequisitionForShopdetailsBiItemCodeAndDate(itemvalue.ItemCode, itemvalue.ModifiedOn, itemvalue.LoggedInShopName, itemvalue.ToShopName);
                if (details1 != null)
                {
                    double? sum = 0;
                    foreach (var data in details1)
                    {
                        sum += data.ReqQuantity;
                        itemvalue.ReqQuantity = sum;
                    }

                }

                else
                {
                    itemvalue.ReqQuantity = itemvalue.ReqQuantity;
                }
                newlistvalue6.Add(itemvalue);
            }

            model.RequisitionForShopList = newlistvalue6;
            return View(model);
        }

        [HttpPost]
        public ActionResult RequisitionOfPo(MainApplication mainapp, FormCollection frm)
        {
            RequisitionOfPo item = new RequisitionOfPo();
            string count = "hdnrowcount";
            int finalcount = Convert.ToInt32(frm[count]);
            for (int i = 1; i <= (finalcount) - 1; i++)
            {
                string itemname = "itemname" + i;
                string itemcode = "itemcode" + i;
                string itemdescription = "itemdescription" + i;
                string itemquantity = "itemquantity" + i;
                string requiredquantity = "requiredquantity" + i;
                string requisitionfrom = "requisitionfromshopname" + i;
                string rejectquantity = "rejectquantity" + i;
                string finalitemname = frm[itemname];
                item.ItemName = finalitemname;
                item.ItemCode = frm[itemcode];
                item.Description = frm[itemdescription];
                item.QuantityInStock = frm[itemquantity];
                item.RequiredQuantity = frm[requiredquantity];
                item.RequisitionFrom = frm[requisitionfrom];
                item.RejectQuantity = frm[rejectquantity];
                item.LoggedInName = "Purchase Manager";
                _iRequisitionOfPoService.createRequisitionOfPo(item);
            }
            return View("RequisitionOfPoDetails");
        }


        [HttpGet]
        public ActionResult ShopRequisitionForShopLogin()
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
                ShopRequisitionGodownsalesbillDetails = new ShopRequisitionGodownsalesbill(),
                usercredential = new UserCredential(),
                RequisitionForShopDetails = new RequisitionForShop(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            string shopcodevalue = string.Empty;
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            var details = _iShopService.GetShopDetailsByName(assigndetails.Modules);
                            shopcodevalue = details.ShopCode;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var Data = _iRequisitionForShopService.GetLastRequisition();
            int length = 0;
            int number = 0;
            if (Data != null)
            {

                length = 0;
                number = 0;
                string code = string.Empty;
                code = Data.ReqCode.Substring(3, 6);
                length = (Convert.ToInt32(code) + 1).ToString().Length;
                number = Convert.ToInt32(code) + 1;
                string Code = _utilityservice.getName("IIS", length, number);
                model.RequisitionForShopDetails.ReqCode = Code + FinYr;
            }
            else
            {
                length = 1;
                number = 1;
                string Code = _utilityservice.getName("IIS", length, number);
                model.RequisitionForShopDetails.ReqCode = Code;
            }
            TempData["PreviousShopIndependantRequisition"] = model.RequisitionForShopDetails.ReqCode;
            //For ShoprequisitionRetailBill
            model.ShopRequisitionGodownList = _ishoprequisitiongodownservice.getAllDetails();
            List<ShopRequisitionGodownItem> newlist = new List<ShopRequisitionGodownItem>();

            foreach (var item in model.ShopRequisitionGodownList)
            {
                var shoprequisition = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemDetailsByReqCode(item.ReqCode);

                foreach (var qty in shoprequisition)
                {
                    var detailofshop = _iShopStockServive.GetDetailsByItemCodeAndShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                    if (detailofshop != null)
                    {
                        qty.Shopqty = detailofshop.Quantity;

                    }
                    else
                    {
                        qty.Shopqty = 0;
                    }
                    newlist.Add(qty);
                }
            }

            IEnumerable<ShopRequisitionGodownItem> finalnewlist = newlist;
            IEnumerable<ShopRequisitionGodownItem> finalnewlist2 = newlist;
            IEnumerable<ShopRequisitionGodownItem> finalnewlist3 = newlist;
            IEnumerable<ShopRequisitionGodownItem> finalnewlist1 = Enumerable.Empty<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue = new List<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue1 = new List<ShopRequisitionGodownItem>();
            List<ShopRequisitionGodownItem> newlistvalue2 = new List<ShopRequisitionGodownItem>();
            newlistvalue1 = newlist;

            foreach (var itemvalue in newlistvalue1.GroupBy(m => new { m.ItemCode, m.SRDate, m.FromShopName, m.RequisitionToShopName }).Select(y => y.First()))
            {
                var details = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemdetailsBiItemCodeAndDate(itemvalue.ItemCode, itemvalue.SRDate, itemvalue.FromShopName, itemvalue.RequisitionToShopName);

                if (details != null)
                {
                    double? sum = 0;
                    foreach (var data in details)
                    {
                        sum += data.RequiredQuantity;
                        itemvalue.RequiredQuantity = sum;
                    }
                }

                else
                {
                    itemvalue.RequiredQuantity = itemvalue.RequiredQuantity;

                }
                newlistvalue2.Add(itemvalue);
            }


            foreach (var itemvalue in newlistvalue2)
            {
                if (itemvalue.FromShopName != Session["SHOPGODOWNNAME"] as string && itemvalue.RequisitionToShopName == Session["SHOPGODOWNNAME"] as string)
                {
                    model.ShopRequisitionGodownItemList = newlistvalue;
                    newlistvalue.Add(itemvalue);
                }

                else
                {
                    model.ShopRequisitionGodownItemListForNull = null;
                }

            }

            //For ShoprequisitionSalesBill
            model.ShopRequisitionGodownsalesbillList = _ShopRequisitionGodownSalesBillService.getAllDetails();
            List<ShopRequisitionGodownitemsalesbill> middlelist = new List<ShopRequisitionGodownitemsalesbill>();
            foreach (var item in model.ShopRequisitionGodownsalesbillList)
            {
                var shoprequisition = _iShopRequisitionGodownItemSalesBillService.getShopRequisitionGodownItemDetailsByReqCode(item.ReqCode);

                foreach (var qty in shoprequisition)
                {
                    var detailofshop = _iShopStockServive.GetDetailsByItemCodeAndShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                    if (detailofshop != null)
                    {
                        qty.Shopqty = detailofshop.Quantity;

                    }
                    else
                    {
                        qty.Shopqty = 0;
                    }
                    middlelist.Add(qty);
                }
            }

            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist = middlelist;
            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist2 = middlelist;
            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist3 = middlelist;
            IEnumerable<ShopRequisitionGodownitemsalesbill> middlenewlist1 = Enumerable.Empty<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue = new List<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue1 = new List<ShopRequisitionGodownitemsalesbill>();
            List<ShopRequisitionGodownitemsalesbill> middlelistvalue2 = new List<ShopRequisitionGodownitemsalesbill>();
            middlelistvalue1 = middlelist;

            foreach (var itemvalue in middlelistvalue1.GroupBy(m => new { m.ItemCode, m.SRDate, m.FromShopName, m.RequisitionFromShopName }).Select(y => y.First()))
            {
                var details = _iShopRequisitionGodownItemSalesBillService.getShopRequisitionGodownItemdetailsBiItemCodeAndDate(itemvalue.ItemCode, itemvalue.SRDate, itemvalue.FromShopName, itemvalue.RequisitionFromShopName);
                if (details != null)
                {
                    double? sum = 0;
                    foreach (var data in details)
                    {
                        sum += data.RequiredQuantity;
                        itemvalue.RequiredQuantity = sum;
                    }
                }

                else
                {
                    itemvalue.RequiredQuantity = itemvalue.RequiredQuantity;
                }
                middlelistvalue2.Add(itemvalue);

            }


            foreach (var itemvalue in middlelistvalue2)
            {
                if (itemvalue.FromShopName != Session["SHOPGODOWNNAME"] as string && itemvalue.RequisitionFromShopName == Session["SHOPGODOWNNAME"] as string)
                {

                    model.ShopRequisitionGodownitemsalesbillList = middlelistvalue;
                    middlelistvalue.Add(itemvalue);
                }

                else
                {
                    model.ShopRequisitionGodownitemsalesbillListForNull = null;
                }

            }

            //For IndividualRequisition
            var detailsofrequisitionforshop = _iRequisitionForShopService.getDetailsOfToShopIfNotNull();
            List<RequisitionForShop> RequisitionForShopListDetailsvalue = new List<RequisitionForShop>();

            foreach (var qty in detailsofrequisitionforshop)
            {
                var detailofshop = _iShopStockServive.GetDetailsByItemCodeAndShopName(qty.ItemCode, Session["SHOPGODOWNNAME"].ToString());
                if (detailofshop != null)
                {
                    qty.ShpQtyShopStock = detailofshop.Quantity;

                }
                else
                {
                    qty.ShpQtyShopStock = 0;
                }
                RequisitionForShopListDetailsvalue.Add(qty);
            }

            List<RequisitionForShop> lastlist = new List<RequisitionForShop>();
            List<RequisitionForShop> lastlist1 = new List<RequisitionForShop>();
            List<RequisitionForShop> lastlist2 = new List<RequisitionForShop>();
            lastlist1 = RequisitionForShopListDetailsvalue;

            foreach (var itemvalue in lastlist1.GroupBy(m => new { m.ItemCode, m.ModifiedOn, m.LoggedInShopName, m.ToShopName }).Select(g => g.First()))
            {

                var details1 = _iRequisitionForShopService.getRequisitionForShopdetailsBiItemCodeAndDate(itemvalue.ItemCode, itemvalue.ModifiedOn, itemvalue.LoggedInShopName, itemvalue.ToShopName);
                if (details1 != null)
                {
                    double? sum = 0;
                    foreach (var data in details1)
                    {
                        sum += data.ReqQuantity;
                        itemvalue.ReqQuantity = sum;
                    }

                }

                else
                {
                    itemvalue.ReqQuantity = itemvalue.ReqQuantity;
                }
                lastlist2.Add(itemvalue);
            }


            foreach (var itemvalue in lastlist2)
            {

                if (itemvalue.LoggedInShopName != Session["SHOPGODOWNNAME"] as string && itemvalue.ToShopName == Session["SHOPGODOWNNAME"] as string)
                {
                    model.RequisitionForShopList = lastlist;
                    lastlist.Add(itemvalue);
                }

                else
                {
                    model.RequisitionForShopListForNull = null;
                }
            }

            model.ItemSubCategoryList = _iItemSubCategoryService.getSubCategory();
            model.ShopStockList = _iShopStockServive.GetRowsByShopCodeFoRrequsition(shopcodevalue);
            model.ShopList = _iShopService.GetAllShopName(Session["SHOPGODOWNNAME"].ToString());
            model.GodownMasterList = _igodownservive.GetGodownNames();
            return View(model);
        }

        [HttpGet]
        public ActionResult ShopRequisitionForNewItems()
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
                ShopRequisitionGodownsalesbillDetails = new ShopRequisitionGodownsalesbill(),
                usercredential = new UserCredential(),
                RequisitionForShopDetails = new RequisitionForShop(),
                RequisitionofNewItemsForShopDetails = new RequisitionofNewItemsForShop(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            string shopcodevalue = string.Empty;
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 82; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            var details = _iShopService.GetShopDetailsByName(assigndetails.Modules);
                            shopcodevalue = details.ShopCode;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }


            return View(model);

        }

        [HttpPost]
        public ActionResult ShopRequisitionForNewItems(MainApplication mainapp, FormCollection frm)
        {

            MainApplication main = new MainApplication()
            {
                RequisitionofNewItemsForShopDetails = new RequisitionofNewItemsForShop(),
            };
            int i = 1;

            string itemname = "itemname" + i;
            string itemcode = "itemcode" + i;
            string itemdescription = "itemdescription" + i;
            string brandname = "brandname" + i;
            string requiredquantity = "requiredquantity" + i;

            main.RequisitionofNewItemsForShopDetails.ShopOrGodownName = Session["SHOPGODOWNNAME"].ToString();
            main.RequisitionofNewItemsForShopDetails.ItemName = frm[itemname];
            main.RequisitionofNewItemsForShopDetails.ItemCode = frm[itemcode];
            main.RequisitionofNewItemsForShopDetails.Quantity = Convert.ToInt16(frm[requiredquantity]);
            main.RequisitionofNewItemsForShopDetails.BrandName = mainapp.RequisitionofNewItemsForShopDetails.BrandName;
            main.RequisitionofNewItemsForShopDetails.ModifiedOn = DateTime.Now;
            main.RequisitionofNewItemsForShopDetails.Description = frm[itemdescription];
            _iRequisitionForNewItemService.createNewRequisition(main.RequisitionofNewItemsForShopDetails);

            SmtpClient client = new SmtpClient("180.149.243.22");
            client.Port = 587;
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
            string body = string.Empty;
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
            message.From = messageFrom;
            message.Subject = "Shop Requisition List";
            body += "Dear Purchase Department ,<br /><br />";
            body += "\t There is a shortage of following items";
            body += "<table border=1><tr><th style=text-align:center>FromShopOrGodown Name</th><th style=text-align:center>ItemCode</th><th style=text-align:center>ItemName</th><th style=text-align:center>BrandName</th><th style=text-align:center>Description</th><th style=text-align:center>Quantity Required</th> </tr> <tr><td style=text-align:center>" + main.RequisitionofNewItemsForShopDetails.ShopOrGodownName + "</td><td style=text-align:center>" + main.RequisitionofNewItemsForShopDetails.ItemCode + "</td><td>" + main.RequisitionofNewItemsForShopDetails.ItemName + "</td><td style=text-align:center>" + main.RequisitionofNewItemsForShopDetails.BrandName + "</td><td>" + main.RequisitionofNewItemsForShopDetails.Description + "</td><td style=text-align:center>" + main.RequisitionofNewItemsForShopDetails.Quantity + "</td></tr> </table>";
            message.Body = body;
            MailAddress messageTo = new MailAddress("athulyanair91@gmail.com");
            message.To.Add(messageTo);
            client.Send(message);
            message.Dispose();
            return RedirectToAction("ShopRequisitionForNewItemDetails");
        }

        [HttpGet]
        public ActionResult ShopRequisitionForNewItemDetails()
        {


            return View();
        }

        [HttpPost]
        public ActionResult ShopRequisitionForShopLogin(MainApplication mainapp, FormCollection frm)
        {
            MainApplication model = new MainApplication()
            {
                user = new User(),
                EmployeeDetails = new EmployeeMaster(),
                OutwardInterShopDetails = new OutwardInterShop(),
                OutwardItemInterShopdetails = new OutwardItemInterShop(),
            };
            string outwards = "false";
            string shcode = Session["LOGINSHOPGODOWNCODE"] as string;
            List<OutwardItemInterShop> outwarditemintershopdetailsvalue = new List<OutwardItemInterShop>();
            model.user = _iUserService.GetUserByEmail(UserEmail);
            RequisitionForShop item = new RequisitionForShop();
            string count = "hdnrowcount";
            int finalcount = Convert.ToInt32(frm[count]);
            var username = HttpContext.Session["UserName"].ToString();
            int counter = 1;

            //generete code for outward
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var shoptoshop = _OutwardInterShopService.GetLastOutwardByFinYear(finyr, shcode);
            string code1 = string.Empty;
            string code = string.Empty;
            int length = 0;
            int number = 0;

            if (shoptoshop != null)
            {
                string trim = shoptoshop.OutwardCode;
                int index = trim.LastIndexOf('O');
                code1 = shoptoshop.OutwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(code1) + 1).ToString().Length;
                number = Convert.ToInt32(code1) + 1;
            }
            else
            {
                length = 1;
                number = 1;
            }

            string shopname = Session["SHOPGODOWNNAME"] as string;
            var shopdetails = _iShopService.GetShopDetailsByName(shopname);
            var utilityname = shopdetails.ShortCode + "/OWSS";
            code = _utilityservice.getName(utilityname, length, number);
            code = code + finyr;

            var detailsvalue = default(ShopRequisitionGodownItem);
            var detailsvalue1 = default(RequisitionForShop);
            var detailsvalueredirect = default(IEnumerable<RequisitionForShop>);
            var detailsvaluesalesbillrequisition = default(ShopRequisitionGodownitemsalesbill);
            for (int i = 1; i <= (finalcount) - 1; i++)
            {
                string itemname = "itemname" + i;
                string itemcode = "itemcode" + i;
                string datevalue = "requisitiondate" + i;
                string itemdescription = "itemdescription" + i;
                string itemquantity = "itemquantity" + i;
                string requiredquantity = "requiredquantity" + i;
                string codevalue = "itemcodevalue" + i;
                string requisitionfrom = "requisitionfromshopname" + i;
                string rejectquantity = "rejectquantity" + i;
                string outwardquantity = "outwardquantity" + i;
                string balancevalue = "balancevalue" + i;

                string finalitemname = frm[itemname];
                if (frm[requisitionfrom] != null)
                {
                    item.RequisitionFrom = frm[requisitionfrom];
                }

                if (frm[codevalue] != null)
                {
                    item.codevaluerequisition = frm[codevalue];
                }


                item.ItemName = finalitemname;
                item.ItemCode = frm[itemcode];
                item.Category = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemCategory;
                item.SubCategory = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemSubCategory;
                item.Description = frm[itemdescription];
                item.ShpQty = Convert.ToDouble(frm[itemquantity]);
                item.ModifiedOn = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());

                if (frm[outwardquantity] != "")
                {
                    item.Status = "Outward";
                    item.OutwardQuantity = Convert.ToDouble(frm[outwardquantity]);
                }
                else
                {
                    item.OutwardQuantity = null;
                }
                if (frm[balancevalue] != "0")
                {

                    item.Balance = Convert.ToDouble(frm[balancevalue]);
                }
                else
                {
                    item.Balance = null;
                }

                if (frm[datevalue] != "0")
                {
                    item.RequsitionDate = Convert.ToDateTime(frm[datevalue]);

                    if (item.RequsitionDate == Convert.ToDateTime("1/1/0001"))
                    {
                        item.RequsitionDate = null;
                    }
                }
                else
                {
                    //item.RequsitionDate = null;
                    item.RequsitionDate = DateTime.Now;
                }
                if (frm[requiredquantity] != "")
                {
                    item.ReqQuantity = Convert.ToDouble(frm[requiredquantity]);
                }
                else
                {
                    item.ReqQuantity = null;
                }
                if (frm[rejectquantity] != "")
                {
                    item.RejectQuantity = Convert.ToDouble(frm[rejectquantity]);
                }
                else
                {
                    item.RejectQuantity = null;
                }

                item.LoggedInShopName = Session["SHOPGODOWNNAME"] as string;
                item.RequisitionForGodownPrepareBy = mainapp.RequisitionForShopDetails.RequisitionForGodownPrepareBy;
                item.RequisitionForGodownEmailIdEmployee = mainapp.RequisitionForShopDetails.RequisitionForGodownEmailIdEmployee;
                item.RequisitionForGodownContactNoEmployee = mainapp.RequisitionForShopDetails.RequisitionForGodownContactNoEmployee;
                item.RequisitionForGodownPrepareTimeEmployee = DateTime.Now;
                item.PrepareBy = model.user.UserName;
                item.EmailIdEmployee = model.user.Email;
                item.ContactNoEmployee = model.user.ContactNo;
                model.EmployeeDetails = _EmployeeMasterService.GetEmpDetailsOfStoreKeeper();
                item.EmpName = model.EmployeeDetails.Name;
                item.EmpDesignation = model.EmployeeDetails.Designation;
                item.EmpContactNo = model.EmployeeDetails.MobileNo;
                item.EmpEmail = model.EmployeeDetails.Email;
                item.ReqCode = mainapp.RequisitionForShopDetails.ReqCode;

                if (item.RequisitionFrom == "null" && item.ReqQuantity != null)
                {
                    item.Status = "Requisition";
                    item.RequisitionFrom = Session["SHOPGODOWNNAME"] as string;
                    item.ToGodownName = mainapp.RequisitionForShopDetails.ToGodownName;
                    item.ToShopName = mainapp.RequisitionForShopDetails.ToShopName;
                    if (item.ToGodownName != "null")
                    {
                        item.RequiredQtyForGodown = Convert.ToDouble(frm[requiredquantity]);
                    }
                    else
                    {
                        item.RequiredQtyForGodown = 0;
                    }
                    if (item.ToShopName != "null")
                    {
                        item.ReqQuantity = Convert.ToDouble(frm[requiredquantity]);
                    }
                    else
                    {
                        item.ReqQuantity = 0;
                    }


                    if (frm["RadioBtn"] == null)
                    {
                        item.RequestToPo = "No";
                    }
                    else
                    {
                        item.RequestToPo = frm["RadioBtn"];
                    }
                }
                else
                    if (item.RequisitionFrom != "null" && item.ReqQuantity != null)
                    {
                        item.ToGodownName = "null";
                        item.ToShopName = "null";
                        item.RequestToPo = "null";
                    }
                SmtpClient client = new SmtpClient("180.149.243.22");
                client.Port = 587;
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
                string body = string.Empty;
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
                message.From = messageFrom;
                message.Subject = "Shop Requisition List";
                body += "Dear Purchase Department ,<br /><br />";
                body += "\t There is a shortage of following items";
                body += "<table border=1><tr><th style=text-align:center>FromShopName</th><th style=text-align:center>ItemCode</th><th style=text-align:center>ItemName</th><th style=text-align:center>Quantity Required</th> </tr> <tr><td style=text-align:center>" + item.LoggedInShopName + "</td><td style=text-align:center>" + item.ItemCode + "</td><td>" + item.ItemName + "</td><td style=text-align:center>" + item.ReqQuantity + "</td></tr> </table>";
                message.Body = body;
                MailAddress messageTo = new MailAddress("athulyanair91@gmail.com");
                message.To.Add(messageTo);
                //client.Send(message);
                message.Dispose();

                if (item.ReqQuantity != null && item.OutwardQuantity != null)
                {
                    _iRequisitionForShopService.CreateRequisitionForShop(item);
                }

                detailsvalueredirect = _iRequisitionForShopService.GetAll();
                if (item.OutwardQuantity != null && item.OutwardQuantity != 0)
                {   
                    string shopcode;
                    model.OutwardInterShopDetails.OutwardCode = code;
                    model.OutwardInterShopDetails.OutwardDate = DateTime.Now;
                    model.OutwardInterShopDetails.FromShopName = item.LoggedInShopName;
                    var details = _iShopService.GetShopDetailsByName(item.LoggedInShopName);
                    model.OutwardInterShopDetails.FromShopCode = details.ShopCode;
                    model.OutwardInterShopDetails.FromShopAddress = details.ShopAddress;
                    model.OutwardInterShopDetails.FromShopContactNo = details.ShopContactNo;
                    model.OutwardInterShopDetails.FromShopEmail = details.ShopEmail;
                    model.OutwardInterShopDetails.FromShopContactPerson = details.EmpName;

                    model.OutwardInterShopDetails.ToShopName = item.RequisitionFrom;
                    var details1 = _iShopService.GetShopDetailsByName(model.OutwardInterShopDetails.ToShopName);
                    model.OutwardInterShopDetails.ToShopCode = details1.ShopCode;
                    shopcode = details1.ShopCode;
                    model.OutwardInterShopDetails.ToDeliveryAt = details1.ShopAddress;
                    model.OutwardInterShopDetails.ToShopContactNo = details1.ShopContactNo;
                    model.OutwardInterShopDetails.ToShopEmail = details1.ShopEmail;
                    model.OutwardInterShopDetails.ToShopContactPerson = details1.EmpName;
                    model.OutwardInterShopDetails.SalesmanName = details.EmpName;
                    EmployeeMaster emp = _EmployeeMasterService.getEmpByName(details.EmpName);

                    model.OutwardInterShopDetails.SalesmanEmail = emp.Email;
                    model.OutwardInterShopDetails.SalesmanDesignation = emp.Designation;
                    model.OutwardInterShopDetails.SalesmanContactNo = emp.MobileNo;

                    model.OutwardItemInterShopdetails.ToShopCode = shopcode;
                    model.OutwardInterShopDetails.PrepareTime = DateTime.Now;
                    model.OutwardInterShopDetails.PrepareBy = item.RequisitionForGodownPrepareBy;
                    EmployeeMaster emp1 = _EmployeeMasterService.getEmpByName(item.RequisitionForGodownPrepareBy);

                    model.OutwardInterShopDetails.PrepareByEmail = item.RequisitionForGodownEmailIdEmployee;
                    model.OutwardInterShopDetails.PrepareByDesignation = emp1.Designation;
                    model.OutwardInterShopDetails.Status = "Active";
                    model.OutwardInterShopDetails.ModifiedOn = DateTime.Now;
                    Random rnd = new Random();
                    string gatepassno = Convert.ToString(rnd.Next(1, 1000));
                    model.OutwardInterShopDetails.GatePass = Convert.ToString(gatepassno);
                    TempData["gatepass"] = gatepassno;

                    model.OutwardInterShopDetails.Narration = null;

                    var outwardintershopcodevalue = _OutwardInterShopService.GetLastRow();
                    if(counter == 1)
                    {
                        _OutwardInterShopService.CreateOutwardInterShop(model.OutwardInterShopDetails);
                        counter++;
                        outwards = "true";
                    }
                    model.OutwardItemInterShopdetails = new OutwardItemInterShop();
                    model.OutwardItemInterShopdetails.ItemName = item.ItemName;
                    model.OutwardItemInterShopdetails.ItemCode = item.ItemCode;

                    var details2 = _iShopStockServive.GetDetailsByItemCodeAndShopName(item.ItemCode, model.OutwardInterShopDetails.ToShopName);

                    model.OutwardItemInterShopdetails.Description = details2.Description;
                    model.OutwardItemInterShopdetails.TypeOfMaterial = details2.Material;
                    model.OutwardItemInterShopdetails.ColorCode = details2.Color;
                    model.OutwardItemInterShopdetails.Barcode = details2.Barcode;
                    model.OutwardItemInterShopdetails.Brand = details2.Brand;
                    model.OutwardItemInterShopdetails.DesignCode = details2.DesignCode;
                    model.OutwardItemInterShopdetails.NumberType = details2.NumberType;
                    model.OutwardItemInterShopdetails.Unit = details2.Unit;
                    model.OutwardItemInterShopdetails.Size = details2.Size;
                    model.OutwardItemInterShopdetails.Status = "Active";
                    model.OutwardItemInterShopdetails.ModifiedOn = DateTime.Now;
                    model.OutwardItemInterShopdetails.SellingPrice = Convert.ToDouble(details2.SellingPrice);
                    model.OutwardItemInterShopdetails.MRP = Convert.ToDouble(details2.MRP);
                    model.OutwardItemInterShopdetails.DesignName = details2.Description;
                    model.OutwardItemInterShopdetails.FromShopCode = model.OutwardInterShopDetails.FromShopCode;
                    model.OutwardItemInterShopdetails.ToShopCode = model.OutwardInterShopDetails.ToShopCode;
                    model.OutwardItemInterShopdetails.OutwardCode = model.OutwardInterShopDetails.OutwardCode;
                    model.OutwardItemInterShopdetails.QuantityTransfer = item.OutwardQuantity;
                    model.OutwardItemInterShopdetails.Balance = (item.ShpQty - item.OutwardQuantity);

                    //to update retail bill
                    if (item.codevaluerequisition.Contains("RE"))
                    {
                        detailsvalue = _ishoprequisitiongodownitemservice.GetDetailsByItemCodeAndFromShopName(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                    }

                    if (item.codevaluerequisition.Contains("IIS"))
                    {
                        detailsvalue1 = _iRequisitionForShopService.GetDetailsByIcAndfsAndDTAndRS(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                    }

                    if (item.codevaluerequisition.Contains("SB"))
                    {
                        detailsvaluesalesbillrequisition = _iShopRequisitionGodownItemSalesBillService.GetDetailsByItemCodeAndFromShopName(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                    }
                    if (detailsvalue != null)
                    {
                        detailsvalue.RequiredQuantity = Convert.ToDouble(frm[balancevalue]);

                        if (detailsvalue.RequiredQuantity == 0)
                        {
                            var detailsienumerable = _ishoprequisitiongodownitemservice.GetDetailsByItemCodeAndFromShopNameIenumerable(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                            foreach (var value in detailsienumerable)
                            {
                                value.RequiredQuantity = 0;
                                _ishoprequisitiongodownitemservice.Update(detailsvalue);
                            }
                        }
                        else
                        {
                            var detailsienumerable1 = _ishoprequisitiongodownitemservice.GetDetailsByItemCodeAndFromShopNameIenumerableSkipFirst(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);

                            foreach (var deienumerable1 in detailsienumerable1)
                            {
                                deienumerable1.RequiredQuantity = 0;
                                _ishoprequisitiongodownitemservice.Update(deienumerable1);
                            }
                        }
                        _ishoprequisitiongodownitemservice.Update(detailsvalue);
                    }

                    //to update Independant shop requisition
                    if (detailsvalue1 != null)
                    {
                        detailsvalue1.ReqQuantity = Convert.ToDouble(frm[balancevalue]);

                        if (detailsvalue1.ReqQuantity == 0)
                        {
                            var detailsienumerable1 = _iRequisitionForShopService.GetDetailsByICAndFShNameIenumerableForRequisitionOfShop(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                            foreach (var value in detailsienumerable1)
                            {
                                value.ReqQuantity = 0;
                                _iRequisitionForShopService.Update(detailsvalue1);
                            }
                        }
                        else
                        {
                            var detailsienumerable2 = _iRequisitionForShopService.GetDetailsByICodeAndSNIenumerableSkipFirstForRequisitionOfShop(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                            foreach (var deienumerable2 in detailsienumerable2)
                            {
                                deienumerable2.ReqQuantity = 0;
                                _iRequisitionForShopService.Update(deienumerable2);
                            }
                        }
                        _iRequisitionForShopService.Update(detailsvalue1);
                    }

                    //to update sales bill
                    if (detailsvaluesalesbillrequisition != null)
                    {
                        detailsvaluesalesbillrequisition.RequiredQuantity = Convert.ToDouble(frm[balancevalue]);

                        if (detailsvaluesalesbillrequisition.RequiredQuantity == 0)
                        {
                            var detailsienumerable = _iShopRequisitionGodownItemSalesBillService.GetDetailsByItemCodeAndFromShopNameIenumerable(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);
                            foreach (var value in detailsienumerable)
                            {
                                value.RequiredQuantity = 0;
                                _iShopRequisitionGodownItemSalesBillService.Update(detailsvaluesalesbillrequisition);
                            }
                        }
                        else
                        {
                            var detailsienumerable1 = _iShopRequisitionGodownItemSalesBillService.GetDetailsByItemCodeAndFromShopNameIenumerableSkipFirst(model.OutwardItemInterShopdetails.ItemCode, model.OutwardInterShopDetails.ToShopName, item.RequsitionDate, model.OutwardInterShopDetails.FromShopName);

                            foreach (var deienumerable1 in detailsienumerable1)
                            {
                                deienumerable1.RequiredQuantity = 0;
                                _iShopRequisitionGodownItemSalesBillService.Update(deienumerable1);
                            }
                        }
                        _iShopRequisitionGodownItemSalesBillService.Update(detailsvaluesalesbillrequisition);
                    }

                    _OutwardItemInterShopservice.CreateInterShopitem(model.OutwardItemInterShopdetails);
                    var datapresent1 = _iStockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.OutwardInterShopDetails.ToShopCode);

                    double? intertransfer = item.OutwardQuantity;
                    double? balance = model.OutwardItemInterShopdetails.Balance;
                    ShopStock shstock = new ShopStock();

                    if (intertransfer != null && intertransfer != 0)
                    {
                        //Quantity Getting Subtracted From Stock Item Distribution
                        var fromshop = _iStockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.OutwardInterShopDetails.FromShopCode);
                        fromshop.ItemQuantity -= intertransfer;
                        fromshop.ModifiedOn = DateTime.Now;
                        _iStockItemDistributionService.Update(fromshop);

                        var fromshstockpresent = _iShopStockServive.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.OutwardInterShopDetails.FromShopCode);
                        var toshstockpresent = _iShopStockServive.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.OutwardInterShopDetails.ToShopCode);

                        fromshstockpresent.Quantity -= intertransfer;
                        fromshstockpresent.ModifiedOn = DateTime.Now;
                        _iShopStockServive.Update(fromshstockpresent);
                    }
                }
            }
            var id = _OutwardInterShopService.GetLastRow();
            if (id != null && outwards == "true")
            {
                var iid = id.OutwardId;
                int Id = iid;

                var encoded = Encode(Id.ToString());
                return RedirectToAction("RequisitionFullfilledForShop/" + encoded, "Requisition");
            }
            else
            {
                return RedirectToAction("ShopRequisitionForShopLogin");
            }
        }

        [HttpGet]
        public ActionResult RequisitionFullfilledForShop(string id)
        {
            MainApplication model = new MainApplication()
            {
                RequisitionForShopDetails = new RequisitionForShop(),
                OutwardInterShopDetails = new OutwardInterShop(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RequisitionForShopDetails = new RequisitionForShop();
            byte[] encoded = Convert.FromBase64String(id);

            //Decoding the encoded value i.e retaining the original value of id
            string decodedvalue = System.Text.Encoding.UTF8.GetString(encoded);

            int idvalue = Convert.ToInt32(decodedvalue);
            model.OutwardInterShopDetails = _OutwardInterShopService.GetDetailsByOutwardId(idvalue);
            model.OutwardItemInterShopList = _OutwardItemInterShopservice.GetDetailsByOutwardCode(model.OutwardInterShopDetails.OutwardCode);
            string previousshopoutward = TempData["PreviousShopIndependantRequisition"].ToString();
            if (previousshopoutward != model.RequisitionForShopDetails.ReqCode)
            {
                ViewData["PreviousShopIndependantRequisitionViewdata"] = previousshopoutward + " is replaced with " + model.RequisitionForShopDetails.ReqCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousShopIndependantRequisition"] = previousshopoutward;
            return View(model);
        }


        [HttpGet]
        public ActionResult RequisitionFullfilled()
        {
            MainApplication model = new MainApplication()
            {
                RequisitionForShopDetails = new RequisitionForShop(),
                OutwardInterShopDetails = new OutwardInterShop(),
                OutwardToShopDetails = new OutwardToShop(),
            };


            string previousshopoutward = TempData["PreviousGodownRequisition"].ToString();
            if (previousshopoutward != model.OutwardToShopDetails.OutwardCode)
            {
                ViewData["PreviousGodownRequisitionViewdata"] = previousshopoutward + " is replaced with " + model.OutwardToShopDetails.OutwardCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousGodownRequisition"] = previousshopoutward;
            return View(model);
        }


        [HttpGet]
        public ActionResult ShopRequisitionForGodown()
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
            };

            var Data = _ishoprequisitiongodownservice.GetLastShopRequisition();
            int Val = 0;
            int length = 0;
            if (Data != null)
            {
                Val = Data.Id;
                Val = Val + 1;
                length = Val.ToString().Length;
            }
            else
            {
                Val = 1;
                length = 1;
            }
            string Code = _utilityservice.getName("RE", length, Val);
            model.ShopRequisitionGodown.ReqCode = Code;
            model.REcode = Code;
            TempData["code"] = model.ShopRequisitionGodown.ReqCode;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.ShopStockList = _iShopStockServive.getShopStock();
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            return View(model);
        }

        [HttpGet]
        public JsonResult SREditList(string term)
        {
            var list = _ishoprequisitiongodownservice.GetREList(term);
            List<string> names = new List<string>();
            foreach (var item in list)
            {
                names.Add(item.ReqCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSREditList(string Reno)
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodownItemDetails = new ShopRequisitionGodownItem()
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            model.ShopStockList = _iShopStockServive.getShopStock();
            model.ShopRequisitionGodown = _ishoprequisitiongodownservice.getShopRequisitionDetailsByShopName(Reno);
            model.ShopRequisitionGodownItemDetails.ShopoRequisitionGodownItemList = _ishoprequisitiongodownitemservice.getShopRequisitionGodownItemDetailsByReqCode(Reno);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MainApplication mainapp, FormCollection frmcoll)
        {
            ShopRequisitionGodownItem item = new ShopRequisitionGodownItem();
            string code;
            if (mainapp.ShopRequisitionGodown != null)
            {
                mainapp.ShopRequisitionGodown.SRDate = DateTime.Now;
                item.ReqCode = mainapp.ShopRequisitionGodown.ReqCode;
                code = item.ReqCode;
                string count = "hdnrowcount";
                int finalcount = Convert.ToInt32(frmcoll[count]);

                for (int i = 1; i <= finalcount; i++)
                {
                    string itemname = "itemname" + i;
                    string itemcode = "itemcode" + i;
                    string itemdescription = "itemdescription" + i;
                    string itemquantity = "itemquantity" + i;
                    string requiredquantity = "requiredquantity" + i;
                    string finalitemname = frmcoll[itemname];

                    item.ItemName = finalitemname;
                    item.ItemCode = frmcoll[itemcode];
                    item.Description = frmcoll[itemdescription];
                    item.StockQuantity = frmcoll[itemquantity];
                    item.RequiredQuantity = Convert.ToDouble(frmcoll[requiredquantity]);
                    _ishoprequisitiongodownitemservice.CreateShopRequisitionGodownItem(item);
                }
                _ishoprequisitiongodownservice.CreateShopRequisitionGodown(mainapp.ShopRequisitionGodown);
            }

            else
            {
                int rows = Convert.ToInt32(frmcoll["hdnRowCount"]);
                for (int i = 1; i <= rows; i++)
                {
                    string itemname = "Item" + i;
                    string description = "itemDescription" + i;
                    string costprice = "CostPrice" + i;

                    if (frmcoll[itemname] != null)
                    {
                        item.ItemName = frmcoll[itemname];
                        item.Description = frmcoll[description];
                        item.RequiredQuantity = Convert.ToDouble(frmcoll[costprice]);
                        item.ReqCode = TempData["code"].ToString();
                        _ishoprequisitiongodownitemservice.CreateShopRequisitionGodownItem(item);
                    }
                }
                return RedirectToAction("NewItemDetails");
            }
            return View();
        }


        [HttpPost]
        public ActionResult ShopRequisitionForSalesBill(MainApplication mainapp, FormCollection frmcoll)
        {
            ShopRequisitionGodownitemsalesbill item = new ShopRequisitionGodownitemsalesbill();
            mainapp.ShopRequisitionGodownsalesbillDetails.SRDate = DateTime.Now;
            if (frmcoll["RadioBtn"] == null)
            {
                mainapp.ShopRequisitionGodownsalesbillDetails.RequestForPO = "No";
            }
            else
            {
                mainapp.ShopRequisitionGodownsalesbillDetails.RequestForPO = frmcoll["RadioBtn"];
            }
            mainapp.ShopRequisitionGodownsalesbillDetails.ToGodownName = mainapp.ShopRequisitionGodownsalesbillDetails.ToGodownName;
            mainapp.ShopRequisitionGodownsalesbillDetails.ToShopName = mainapp.ShopRequisitionGodownsalesbillDetails.ToShopName;
            mainapp.ShopRequisitionGodownsalesbillDetails.PrepareTimeEmployee = DateTime.Now;
            string count = "hdnrowcount";
            int finalcount = Convert.ToInt32(frmcoll[count]);

            for (int i = 1; i <= finalcount; i++)
            {
                string itemname = "itemname" + i;
                string itemcode = "itemcode" + i;
                string itemdescription = "itemdescription" + i;
                string itemquantity = "itemquantity" + i;
                string requiredquantity = "requiredquantity" + i;

                string finalitemname = frmcoll[itemname];
                item.ItemName = finalitemname;
                item.ItemCode = frmcoll[itemcode];
                item.Category = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemCategory;
                item.SubCategory = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemSubCategory;
                item.Description = frmcoll[itemdescription];
                item.StockQuantity = frmcoll[itemquantity];
                item.RequiredQuantity = Convert.ToDouble(frmcoll[requiredquantity]);
                item.RequiredQtyForGodown = Convert.ToDouble(frmcoll[requiredquantity]);
                item.ReqCode = mainapp.ShopRequisitionGodownsalesbillDetails.ReqCode;
                item.RequisitionFromShopName = mainapp.ShopRequisitionGodownsalesbillDetails.ToShopName;
                item.RequisitionFromGodownName = mainapp.ShopRequisitionGodownsalesbillDetails.ToGodownName;
                item.FromShopName = mainapp.ShopRequisitionGodownsalesbillDetails.FromShopName;
                item.PrepareBy = mainapp.ShopRequisitionGodownsalesbillDetails.PrepareBy;
                item.EmailIdEmployee = mainapp.ShopRequisitionGodownsalesbillDetails.EmailIdEmployee;
                item.ContactNoEmployee = mainapp.ShopRequisitionGodownsalesbillDetails.ContactNoEmployee;
                item.SRDate = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());

            }

            var shop = _iShopRequisitionGodownItemSalesBillService.getAllItems(item.ItemCode);
            var shopname = mainapp.ShopRequisitionGodownsalesbillDetails.FromShopName;
            var shopgodownlist = _ShopRequisitionGodownSalesBillService.getShopRequisitionDetailsByShopName(shopname);
            string retailbillcode = "retailcode";
            string finalretailcode = frmcoll[retailbillcode];
            int id = 1;
            string transqty = "transqty" + id;
            string finaltransqty = frmcoll[transqty];
            id++;
            var retailbilldetails = _iRetailBillItemService.GetDetailsByRetailBillNo(finalretailcode);

            if ((shop == null || shopgodownlist == null || retailbilldetails.Count() == 0))
            {
                _ShopRequisitionGodownSalesBillService.CreateShopRequisitionGodown(mainapp.ShopRequisitionGodownsalesbillDetails);
                _iShopRequisitionGodownItemSalesBillService.CreateShopRequisitionGodownItem(item);
                SmtpClient client = new SmtpClient("180.149.243.22");
                client.Port = 587;
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
                string body = string.Empty;

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
                message.From = messageFrom;
                message.Subject = "Shop Requisition List";
                body += "Dear Purchase Department ,<br /><br />";
                body += "\t There is a shortage of following items while creating Retail Bill";
                body += "<table border=1><tr><th style=text-align:center>FromShopName</th><th style=text-align:center>ItemCode</th><th style=text-align:center>ItemName</th><th style=text-align:center>Quantity Required</th> </tr> <tr><td style=text-align:center>" + mainapp.ShopRequisitionGodownsalesbillDetails.FromShopName + "</td><td style=text-align:center>" + item.ItemCode + "</td><td>" + item.ItemName + "</td><td style=text-align:center>" + item.RequiredQuantity + "</td></tr> </table>";
                message.Body = body;
                MailAddress messageTo = new MailAddress("athulyanair91@gmail.com");
                message.To.Add(messageTo);
                client.Send(message);
                message.Dispose();
                return RedirectToAction("ShopRequisitionForSalesBillDetails");
            }
            return RedirectToAction("ShopRequisitionForSalesBillDetails");
        }

        [HttpGet]
        public ActionResult ShopRequisitionForSalesBillDetails()
        {
            MainApplication model = new MainApplication();
            model.ShopRequisitionGodownsalesbillDetails = new ShopRequisitionGodownsalesbill();
            string previousshopoutward = TempData["PreviousDataSalesBill"].ToString();

            if (previousshopoutward != model.ShopRequisitionGodownsalesbillDetails.ReqCode)
            {
                ViewData["PreviousDataSalesBillViewdata"] = previousshopoutward + " is replaced with " + model.ShopRequisitionGodownsalesbillDetails.ReqCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousDataSalesBill"] = previousshopoutward;
            return View(model);
        }

        [HttpGet]
        public ActionResult ShopRequisitionForRetailBillDetails()
        {
            MainApplication model = new MainApplication();
            model.RequisitionForShopDetails = new RequisitionForShop();
            string previousshopoutward = TempData["PreviousDataRetailBill"].ToString();
            if (previousshopoutward != model.RequisitionForShopDetails.ReqCode)
            {
                ViewData["PreviousDataRetailBillViewdata"] = previousshopoutward + " is replaced with " + model.RequisitionForShopDetails.ReqCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousDataRetailBill"] = previousshopoutward;
            return View(model);
        }

        [HttpPost]
        public ActionResult ShopRequisitionForRetailBill(MainApplication mainapp, FormCollection frmcoll)
        {
            ShopRequisitionGodownItem item = new ShopRequisitionGodownItem();
            mainapp.ShopRequisitionGodown.SRDate = DateTime.Now;
            if (frmcoll["RadioBtn"] == null)
            {
                mainapp.ShopRequisitionGodown.RequestForPO = "No";
            }
            else
            {
                mainapp.ShopRequisitionGodown.RequestForPO = frmcoll["RadioBtn"];
            }
            mainapp.ShopRequisitionGodown.ToGodownName = mainapp.ShopRequisitionGodown.ToGodownName;
            mainapp.ShopRequisitionGodown.ToShopName = mainapp.ShopRequisitionGodown.ToShopName;
            mainapp.ShopRequisitionGodown.PrepareTimeEmployee = DateTime.Now;
            string count = "hdnrowcount";
            int finalcount = Convert.ToInt32(frmcoll[count]);

            for (int i = 1; i <= finalcount; i++)
            {
                string itemname = "itemname" + i;
                string itemcode = "itemcode" + i;
                string itemdescription = "itemdescription" + i;
                string itemquantity = "itemquantity" + i;
                string requiredquantity = "requiredquantity" + i;

                string finalitemname = frmcoll[itemname];
                item.ItemName = finalitemname;
                item.ItemCode = frmcoll[itemcode];
                item.Category = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemCategory;
                item.SubCategory = _iItemService.GetDescriptionByItemCode(item.ItemCode).itemSubCategory;
                item.Description = frmcoll[itemdescription];
                item.StockQuantity = frmcoll[itemquantity];
                item.RequiredQuantity = Convert.ToDouble(frmcoll[requiredquantity]);
                item.RequiredQtyForGodown = Convert.ToDouble(frmcoll[requiredquantity]);
                item.ReqCode = mainapp.ShopRequisitionGodown.ReqCode;
                item.RequisitionToShopName = mainapp.ShopRequisitionGodown.ToShopName;
                item.RequisitionToGodownName = mainapp.ShopRequisitionGodown.ToGodownName;
                item.FromShopName = mainapp.ShopRequisitionGodown.FromShopName;
                item.PrepareBy = mainapp.ShopRequisitionGodown.PrepareBy;
                item.EmailIdEmployee = mainapp.ShopRequisitionGodown.EmailIdEmployee;
                item.ContactNoEmployee = mainapp.ShopRequisitionGodown.ContactNoEmployee;
                item.SRDate = Convert.ToDateTime(DateTime.Now.Date.ToShortDateString());

            }

            var shop = _ishoprequisitiongodownitemservice.getAllItems(item.ItemCode);
            var shopname = mainapp.ShopRequisitionGodown.FromShopName;
            var shopgodownlist = _ishoprequisitiongodownservice.getShopRequisitionDetailsByShopName(shopname);
            string retailbillcode = "retailcode";
            string finalretailcode = frmcoll[retailbillcode];
            int id = 1;
            string transqty = "transqty" + id;
            string finaltransqty = frmcoll[transqty];
            id++;
            var retailbilldetails = _iRetailBillItemService.GetDetailsByRetailBillNo(finalretailcode);

            if ((shop == null || shopgodownlist == null || retailbilldetails.Count() == 0))
            {
                _ishoprequisitiongodownservice.CreateShopRequisitionGodown(mainapp.ShopRequisitionGodown);
                _ishoprequisitiongodownitemservice.CreateShopRequisitionGodownItem(item);
                SmtpClient client = new SmtpClient("180.149.243.22");
                client.Port = 587;
                client.EnableSsl = false;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
                string body = string.Empty;

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
                message.From = messageFrom;
                message.Subject = "Shop Requisition List";
                body += "Dear Purchase Department ,<br /><br />";
                body += "\t There is a shortage of following items while creating Retail Bill";
                body += "<table border=1><tr><th style=text-align:center>FromShopName</th><th style=text-align:center>ItemCode</th><th style=text-align:center>ItemName</th><th style=text-align:center>Quantity Required</th> </tr> <tr><td style=text-align:center>" + mainapp.ShopRequisitionGodown.FromShopName + "</td><td style=text-align:center>" + item.ItemCode + "</td><td>" + item.ItemName + "</td><td style=text-align:center>" + item.RequiredQuantity + "</td></tr> </table>";
                message.Body = body;
                MailAddress messageTo = new MailAddress("athulyanair91@gmail.com");
                message.To.Add(messageTo);
                client.Send(message);
                message.Dispose();
                return RedirectToAction("ShopRequisitionForRetailBillDetails");
            }
            return RedirectToAction("ShopRequisitionForRetailBillDetails");
        }


        [HttpGet]
        public ActionResult StockVerification()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ShopStockList = _iShopStockServive.getShopStock();
            model.GodownStockList = _igodownstockservice.getGodownStock();

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult ShopRequisitionForRetailBill(string code)
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
                usercredential = new UserCredential(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            double? qty = 0;

            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    model.usercredential = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (model.usercredential != null)
                    {
                        string code1 = model.usercredential.AssignRightsCode;
                        if (model.usercredential.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = model.usercredential.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = model.usercredential.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = model.usercredential.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = model.usercredential.Modules;
                        }

                        model.ShopRequisitionGodown.FromShopName = model.usercredential.Modules;
                        model.ShopRequisitionGodown.FromShopCode = model.usercredential.AssignRightsCode;
                        model.ShopStockDetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(code, Session["SHOPGODOWNNAME"].ToString());
                        qty = model.ShopStockDetails.Quantity;
                        break;
                    }
                }
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var Data = _ishoprequisitiongodownservice.GetLastShopRequisition();
            int Val = 0;
            int length = 0;
            if (Data != null)
            {
                Val = Data.Id;
                Val = Val + 1;
                length = Val.ToString().Length;
            }
            else
            {
                Val = 1;
                length = 1;
            }
            var shopname = model.ShopRequisitionGodown.FromShopName;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = shopname.Split(delimiterChars);
            var name = words[0];
            var name1 = words[1];
            string sub = name.Substring(0, 1);
            string sub1 = name1.Substring(0, 1);
            string Code = _utilityservice.getName(sub + sub1 + "/RE", length, Val);
            model.ShopRequisitionGodown.ReqCode = Code + FinYr;
            model.REcode = Code + FinYr;
            TempData["PreviousDataRetailBill"] = model.ShopRequisitionGodown.ReqCode;
            TempData["code"] = model.ShopRequisitionGodown.ReqCode;
            model.ShopList = _iShopService.GetAllShopName(model.usercredential.Modules);

            List<ShopStock> newlistvalue = new List<ShopStock>();
            foreach (var data in model.ShopList)
            {
                model.ShopStockList = _iShopStockServive.GetDetailsByItemCodeAndShopCodeForDynamicList(code, data.ShopCode);
                foreach (var qtyvalue in model.ShopStockList)
                {
                    if (qtyvalue.Quantity != 0)
                    {

                        newlistvalue.AddRange(model.ShopStockList);
                    }
                }
            }
            model.ShopStockList = newlistvalue;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            List<GodownStock> newlistvalueforgodown = new List<GodownStock>();
            // foreach (string name in DB.Books.Select(x=> new {name = x.authorname}).Distinct())

            foreach (var data1 in model.GodownMasterList)
            {
                model.GodownStockList = _igodownstockservice.GetDetailsByItemCodeAndGodownCodeForDynamicList(code, data1.GdCode);

                //foreach(var godownname in model.GodownStockList.Select(x=>new {godownname=x.GodownName}).Distinct())
                //{
                foreach (var qtyvaluegodown in model.GodownStockList)
                {

                    //if (!newlistvalueforgodown.Contains(data1.GodownName.ToString()))
                    //{
                    if (qtyvaluegodown.Quantity != 0)
                    {
                        newlistvalueforgodown.AddRange(model.GodownStockList);
                    }
                }
            }
            //}

            // newlistvalueforgodown.Distinct();
            model.GodownStockList = newlistvalueforgodown;
            //model.GodownStockList.Distinct();
            return View(model);
        }

        [HttpGet]
        public ActionResult ShopRequisitionForSalesBill(string code)
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodownsalesbillDetails = new ShopRequisitionGodownsalesbill(),
                usercredential = new UserCredential(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            var username = HttpContext.Session["UserName"].ToString();
            double? qty = 0;
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    model.usercredential = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (model.usercredential != null)
                    {
                        string code1 = model.usercredential.AssignRightsCode;
                        if (model.usercredential.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = model.usercredential.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = model.usercredential.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = model.usercredential.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = model.usercredential.Modules;
                        }

                        model.ShopRequisitionGodownsalesbillDetails.FromShopName = model.usercredential.Modules;
                        model.ShopRequisitionGodownsalesbillDetails.FromShopCode = model.usercredential.AssignRightsCode;
                        model.ShopStockDetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(code, Session["SHOPGODOWNNAME"].ToString());
                        qty = model.ShopStockDetails.Quantity;
                        break;
                    }
                }
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var Data = _ShopRequisitionGodownSalesBillService.GetLastShopRequisition();
            int Val = 0;
            int length = 0;
            if (Data != null)
            {
                Val = Data.Id;
                Val = Val + 1;
                length = Val.ToString().Length;
            }
            else
            {
                Val = 1;
                length = 1;
            }

            var shopname = model.ShopRequisitionGodownsalesbillDetails.FromShopName;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = shopname.Split(delimiterChars);
            var name = words[0];
            var name1 = words[1];
            string sub = name.Substring(0, 1);
            string sub1 = name1.Substring(0, 1);
            string Code = _utilityservice.getName(sub + sub1 + "/SB", length, Val);
            model.ShopRequisitionGodownsalesbillDetails.ReqCode = Code + FinYr;
            model.REcode = Code + FinYr;
            TempData["PreviousDataSalesBill"] = model.ShopRequisitionGodownsalesbillDetails.ReqCode;
            TempData["code"] = model.ShopRequisitionGodownsalesbillDetails.ReqCode;
            model.ShopList = _iShopService.GetAllShopName(model.usercredential.Modules);

            List<ShopStock> newlistvalue = new List<ShopStock>();
            foreach (var data in model.ShopList)
            {

                model.ShopStockList = _iShopStockServive.GetDetailsByItemCodeAndShopCodeForDynamicList(code, data.ShopCode);
                foreach (var qtyvalue in model.ShopStockList)
                {
                    if (qtyvalue.Quantity != 0)
                    {

                        newlistvalue.AddRange(model.ShopStockList);
                    }
                }
            }
            model.ShopStockList = newlistvalue;
            model.GodownMasterList = _igodownservive.GetGodownNames();
            List<GodownStock> newlistvalueforgodown = new List<GodownStock>();

            foreach (var data1 in model.GodownMasterList)
            {
                model.GodownStockList = _igodownstockservice.GetDetailsByItemCodeAndGodownCodeForDynamicList(code, data1.GdCode);
                foreach (var qtyvaluegodown in model.GodownStockList)
                {
                    if (qtyvaluegodown.Quantity != 0)
                    {

                        newlistvalueforgodown.AddRange(model.GodownStockList);
                    }
                }
            }
            model.GodownStockList = newlistvalueforgodown;
            return View(model);
        }





        // ********************************  REQUISITION REPORTS ************************************

        // THIS IS FOR SHOP + GODOWN REPORTS..
        [HttpGet]
        public ActionResult RequisitionReports()
        {
            MainApplication model = new MainApplication()
            {
                ShopRequisitionGodown = new ShopRequisitionGodown(),
                usercredential = new UserCredential(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            var username = HttpContext.Session["UserName"].ToString();
            string shopcodevalue = string.Empty;

            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 100; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            shopcodevalue = assigndetails.AssignRightsCode;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }

            }
            return View(model);
        }

        //RETAIL BILL REQUISITION REPORTS
        public ActionResult RequisitionReportForRetailBill()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRBRequisionItemsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.ShopRequisitionGodownItemList = _ishoprequisitiongodownitemservice.GetRowsByFromAndToDate(FDate, TDate);

            double shopqty = 0;
            double godownqty = 0;
            var RetailBillReqList = model.ShopRequisitionGodownItemList;
            RetailBillReqList.ToList().Clear();
            foreach (var data in model.ShopRequisitionGodownItemList)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.RequisitionToShopName != null)
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.RequisitionToShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.Shopqty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.RequisitionToGodownName != null)
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.RequisitionToGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQuantity = godownqty;
                RetailBillReqList.ToList().Add(data);
            }
            model.ShopRequisitionGodownItemList.ToList().Clear();
            model.ShopRequisitionGodownItemList = RetailBillReqList;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSubCategory(string term)
        {
            var result = _iItemSubCategoryService.GetActiveSubCategories(term);
            List<string> subcategories = new List<string>();
            foreach (var data in result)
            {
                subcategories.Add(data.subCategoryName);
            }
            return Json(subcategories, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult LoadItemsBySubCategory(string subcat)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _iItemService.GetItemsBySubCategory(subcat);
            var data = model.ItemList.Select(s => new SelectListItem()
            {
                Text = s.itemCode + " " + s.itemName,
                Value = s.itemCode,
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRBRequisionItemsByDateAndSubCategory(string fromdate, string todate, string subcategory)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(todate);
            DateTime FDate = Convert.ToDateTime(fromdate);

            model.ShopRequisitionGodownItemList = _ishoprequisitiongodownitemservice.GetRowsByFromAndToDateAndSubCat(FDate, TDate, subcategory);

            double shopqty = 0;
            double godownqty = 0;
            var RetailBillReqList = model.ShopRequisitionGodownItemList;
            RetailBillReqList.ToList().Clear();
            foreach (var data in model.ShopRequisitionGodownItemList)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.RequisitionToShopName != null)
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.RequisitionToShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.Shopqty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.RequisitionToGodownName != null)
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.RequisitionToGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQuantity = godownqty;
                RetailBillReqList.ToList().Add(data);
            }
            model.ShopRequisitionGodownItemList.ToList().Clear();
            model.ShopRequisitionGodownItemList = RetailBillReqList;
            return View(model);

        }


        [HttpGet]
        public ActionResult GetRBRequisionItemDetailsByItemName(string fromdate, string todate, string ItemCode)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(todate);
            DateTime FDate = Convert.ToDateTime(fromdate);
            string itemname = ItemCode;
            string[] singleitem = itemname.Split(',');

            List<ShopRequisitionGodownItem> RequisitionItemList = new List<ShopRequisitionGodownItem>();
            for (int i = 0; i < singleitem.Count(); i++)
            {
                model.ShopRequisitionGodownItemList = _ishoprequisitiongodownitemservice.GetRowsByFromAndToDateAndItemCode(FDate, TDate, singleitem[i]);

                double shopqty = 0;
                double godownqty = 0;
                var RetailBillReqList = model.ShopRequisitionGodownItemList;
                RetailBillReqList = new List<ShopRequisitionGodownItem>();
                foreach (var data in model.ShopRequisitionGodownItemList)
                {
                    //CALCULATE SHOP QUANTITY FOR SHOP
                    if (data.RequisitionToShopName != null)
                    {
                        var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.RequisitionToShopName);
                        shopqty = Convert.ToDouble(shopdetails.Quantity);
                    }
                    data.Shopqty = shopqty;

                    //CALCULATE GODOWN QUANTITY FOR GODOWN
                    if (data.RequisitionToGodownName != null)
                    {
                        var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.RequisitionToGodownName);
                        godownqty = Convert.ToDouble(godowndetails.Quantity);
                    }
                    data.GodownQuantity = godownqty;
                    RetailBillReqList.ToList().Add(data);
                    RequisitionItemList.Add(data);
                }
                model.ShopRequisitionGodownItemList.ToList().Clear();
                model.ShopRequisitionGodownItemList = RequisitionItemList;
            }
            return View(model);

        }

        //SALES BILL REQUISITION REPORTS
        public ActionResult RequisitionReportForSalesBill()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetSBRequisionItemsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.ShopRequisitionGodownitemsalesbillList = _iShopRequisitionGodownItemSalesBillService.GetRowsByFromAndToDate(FDate, TDate);

            double shopqty = 0;
            double godownqty = 0;
            var RetailBillReqList = model.ShopRequisitionGodownitemsalesbillList;
            RetailBillReqList.ToList().Clear();
            foreach (var data in model.ShopRequisitionGodownitemsalesbillList)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.RequisitionFromShopName != null)
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.RequisitionFromShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.Shopqty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.RequisitionFromGodownName != null)
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.RequisitionFromGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQuantity = godownqty;
                RetailBillReqList.ToList().Add(data);
            }
            model.ShopRequisitionGodownitemsalesbillList.ToList().Clear();
            model.ShopRequisitionGodownitemsalesbillList = RetailBillReqList;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetSBRequisionItemsByDateAndSubCategory(string fromdate, string todate, string subcategory)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(todate);
            DateTime FDate = Convert.ToDateTime(fromdate);

            model.ShopRequisitionGodownitemsalesbillList = _iShopRequisitionGodownItemSalesBillService.GetRowsByFromAndToDateAndSubCat(FDate, TDate, subcategory);

            double shopqty = 0;
            double godownqty = 0;
            var RetailBillReqList = model.ShopRequisitionGodownitemsalesbillList;
            RetailBillReqList.ToList().Clear();
            foreach (var data in model.ShopRequisitionGodownitemsalesbillList)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.RequisitionFromShopName != null)
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.RequisitionFromShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.Shopqty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.RequisitionFromGodownName != null)
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.RequisitionFromGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQuantity = godownqty;
                RetailBillReqList.ToList().Add(data);
            }
            model.ShopRequisitionGodownitemsalesbillList.ToList().Clear();
            model.ShopRequisitionGodownitemsalesbillList = RetailBillReqList;
            return View(model);

        }

        [HttpGet]
        public ActionResult GetSBRequisionItemDetailsByItemName(string fromdate, string todate, string ItemCode)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(todate);
            DateTime FDate = Convert.ToDateTime(fromdate);
            string itemname = ItemCode;
            string[] singleitem = itemname.Split(',');

            List<ShopRequisitionGodownitemsalesbill> RequisitionItemList = new List<ShopRequisitionGodownitemsalesbill>();
            for (int i = 0; i < singleitem.Count(); i++)
            {
                model.ShopRequisitionGodownitemsalesbillList = _iShopRequisitionGodownItemSalesBillService.GetRowsByFromAndToDateAndItemCode(FDate, TDate, singleitem[i]);

                double shopqty = 0;
                double godownqty = 0;
                var RetailBillReqList = model.ShopRequisitionGodownitemsalesbillList;
                RetailBillReqList = new List<ShopRequisitionGodownitemsalesbill>();
                foreach (var data in model.ShopRequisitionGodownitemsalesbillList)
                {
                    //CALCULATE SHOP QUANTITY FOR SHOP
                    if (data.RequisitionFromShopName != null)
                    {
                        var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.RequisitionFromShopName);
                        shopqty = Convert.ToDouble(shopdetails.Quantity);
                    }
                    data.Shopqty = shopqty;

                    //CALCULATE GODOWN QUANTITY FOR GODOWN
                    if (data.RequisitionFromGodownName != null)
                    {
                        var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.RequisitionFromGodownName);
                        godownqty = Convert.ToDouble(godowndetails.Quantity);
                    }
                    data.GodownQuantity = godownqty;
                    RetailBillReqList.ToList().Add(data);
                    RequisitionItemList.Add(data);
                }
                model.ShopRequisitionGodownitemsalesbillList.ToList().Clear();
                model.ShopRequisitionGodownitemsalesbillList = RequisitionItemList;
            }
            return View(model);

        }

        //INDIVIDUAL REQUISITION REPORTS
        public ActionResult IndividualRequisitionReports()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetIndRequisionItemsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);

            model.RequisitionForShopList = _iRequisitionForShopService.GetRowsByFromAndToDate(FDate, TDate);
            double shopqty = 0;
            double godownqty = 0;
            var ShopReqList = model.RequisitionForShopList;
            ShopReqList.ToList().Clear();
            foreach (var data in model.RequisitionForShopList)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.ToShopName != null && data.ToShopName != "null")
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.ToShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.ShpQty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.ToGodownName != null && data.ToGodownName != "null")
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.ToGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQty = godownqty;
                ShopReqList.ToList().Add(data);
            }
            model.RequisitionForShopList.ToList().Clear();
            model.RequisitionForShopList = ShopReqList;

            model.Requisitionforgodownlist = _iRequisitionforgodownservice.GetRowsByFromAndToDate(FDate, TDate);
            double shopqty1 = 0;
            double godownqty1 = 0;
            var GodownReqList = model.Requisitionforgodownlist;
            GodownReqList.ToList().Clear();
            foreach (var data in model.Requisitionforgodownlist)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.ToShopName != null && data.ToShopName != "null")
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.ToShopName);
                    shopqty1 = Convert.ToDouble(shopdetails.Quantity);
                }
                data.ShopQty = shopqty1;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.ToGodownName != null && data.ToGodownName != "null")
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.ToGodownName);
                    godownqty1 = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQty = godownqty1;
                GodownReqList.ToList().Add(data);
            }
            model.Requisitionforgodownlist.ToList().Clear();
            model.Requisitionforgodownlist = GodownReqList;
            return View(model);

        }

        [HttpGet]
        public ActionResult GetIndRequisionItemsByDateAndSubCategory(string fromdate, string todate, string subcategory)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(todate);
            DateTime FDate = Convert.ToDateTime(fromdate);

            model.RequisitionForShopList = _iRequisitionForShopService.GetRowsByFromAndToDateAndSubCat(FDate, TDate, subcategory);
            double shopqty = 0;
            double godownqty = 0;
            var ShopReqList = model.RequisitionForShopList;
            ShopReqList.ToList().Clear();
            foreach (var data in model.RequisitionForShopList)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.ToShopName != null && data.ToShopName != "null")
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.ToShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.ShpQty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.ToGodownName != null && data.ToGodownName != "null")
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.ToGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQty = godownqty;
                ShopReqList.ToList().Add(data);
            }
            model.RequisitionForShopList.ToList().Clear();
            model.RequisitionForShopList = ShopReqList;

            model.Requisitionforgodownlist = _iRequisitionforgodownservice.GetRowsByFromAndToDateAndSubCat(FDate, TDate, subcategory);
            double shopqty1 = 0;
            double godownqty1 = 0;
            var GodownReqList = model.Requisitionforgodownlist;
            GodownReqList.ToList().Clear();
            foreach (var data in model.Requisitionforgodownlist)
            {
                //CALCULATE SHOP QUANTITY FOR SHOP
                if (data.ToShopName != null && data.ToShopName != "null")
                {
                    var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.ToShopName);
                    shopqty = Convert.ToDouble(shopdetails.Quantity);
                }
                data.ShopQty = shopqty;

                //CALCULATE GODOWN QUANTITY FOR GODOWN
                if (data.ToGodownName != null && data.ToGodownName != "null")
                {
                    var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.ToGodownName);
                    godownqty = Convert.ToDouble(godowndetails.Quantity);
                }
                data.GodownQty = godownqty;
                GodownReqList.ToList().Add(data);
            }
            model.Requisitionforgodownlist.ToList().Clear();
            model.Requisitionforgodownlist = GodownReqList;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetIndRequisionItemDetailsByItemName(string fromdate, string todate, string ItemCode)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(todate);
            DateTime FDate = Convert.ToDateTime(fromdate);
            string itemname = ItemCode;
            string[] singleitem = itemname.Split(',');

            List<RequisitionForShop> ShopRequisitionItemList = new List<RequisitionForShop>();
            for (int i = 0; i < singleitem.Count(); i++)
            {
                model.RequisitionForShopList = _iRequisitionForShopService.GetRowsByFromAndToDateAndItemCode(FDate, TDate, singleitem[i]);

                double shopqty = 0;
                double godownqty = 0;
                var ShopReqList = model.RequisitionForShopList;
                ShopReqList = new List<RequisitionForShop>();
                foreach (var data in model.RequisitionForShopList)
                {
                    //CALCULATE SHOP QUANTITY FOR SHOP
                    if (data.ToShopName != null && data.ToShopName != "null")
                    {
                        var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.ToShopName);
                        shopqty = Convert.ToDouble(shopdetails.Quantity);
                    }
                    data.ShpQty = shopqty;

                    //CALCULATE GODOWN QUANTITY FOR GODOWN
                    if (data.ToGodownName != null && data.ToGodownName != "null")
                    {
                        var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.ToGodownName);
                        godownqty = Convert.ToDouble(godowndetails.Quantity);
                    }
                    data.GodownQty = godownqty;
                    ShopReqList.ToList().Add(data);
                    ShopRequisitionItemList.Add(data);
                }
                model.RequisitionForShopList.ToList().Clear();
                model.RequisitionForShopList = ShopRequisitionItemList;
            }

            List<RequisitionForGodown> GodownRequisitionItemList = new List<RequisitionForGodown>();
            for (int i = 0; i < singleitem.Count(); i++)
            {
                model.Requisitionforgodownlist = _iRequisitionforgodownservice.GetRowsByFromAndToDateAndItemCode(FDate, TDate, singleitem[i]); ;

                double shopqty = 0;
                double godownqty = 0;
                var GodownReqList = model.Requisitionforgodownlist;
                GodownReqList = new List<RequisitionForGodown>();
                foreach (var data in model.Requisitionforgodownlist)
                {
                    //CALCULATE SHOP QUANTITY FOR SHOP
                    if (data.ToShopName != null && data.ToShopName != "null")
                    {
                        var shopdetails = _iShopStockServive.GetDetailsByItemCodeAndShopName(data.ItemCode, data.ToShopName);
                        shopqty = Convert.ToDouble(shopdetails.Quantity);
                    }
                    data.ShopQty = shopqty;

                    //CALCULATE GODOWN QUANTITY FOR GODOWN
                    if (data.ToGodownName != null && data.ToGodownName != "null")
                    {
                        var godowndetails = _igodownstockservice.GetRowsByItemCodeandShopName(data.ItemCode, data.ToGodownName);
                        godownqty = Convert.ToDouble(godowndetails.Quantity);
                    }
                    data.GodownQty = godownqty;
                    GodownReqList.ToList().Add(data);
                    GodownRequisitionItemList.Add(data);
                }
                model.Requisitionforgodownlist.ToList().Clear();
                model.Requisitionforgodownlist = GodownRequisitionItemList;
            }
            return View(model);
        }
    }



}

