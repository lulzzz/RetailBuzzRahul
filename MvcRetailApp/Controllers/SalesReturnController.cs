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
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //[NoDirectAccess]
    [SessionExpireFilter]
    public class SalesReturnController : Controller
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

        private readonly ISalesReturnService _SalesReturnService;
        private readonly ISalesReturnItemService _SalesReturnItemService;
        private readonly IRetailBillService _RetailBillService;
        private readonly IRetailBillItemService _RetailBillItemService;
        private readonly ISalesBillService _SalesBillService;
        private readonly ISalesBillItemService _SalesBillItemService;
        private readonly IItemService _ItemService;
        private readonly IDesignService _DesignService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly IShopStockService _ShopStockService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierSalesBillService _CashierSalesBillService;
        private readonly IInventoryTaxService _InventoryTaxService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IUtilityService _UtilityService;
        private readonly IRetailBillCreditNoteService _RetailBillCreditNoteService;
        private readonly IRetailBillCreditNoteItemService _RetailBillCreditNoteItemService;
        private readonly ISalesBillCreditNoteService _SalesBillCreditNoteService;
        private readonly ISalesBillCreditNoteItemService _SalesBillCreditNoteItemService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;
        private readonly IShopService _ShopService;

        public SalesReturnController(ISalesReturnService SalesReturnService, ISalesReturnItemService SalesReturnItemService, IRetailBillService RetailBillService, IRetailBillItemService RetailBillItemService, ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService,
            IItemService ItemService, IDesignService DesignService, IItemTaxService ItemTaxService, IShopStockService ShopStockService, IStockItemDistributionService StockItemDistributionService, IOpeningStockService OpeningStockService, IEntryStockItemService EntryStockItemService, INonInventoryItemService NonInventoryItemService,
            ICashierRetailBillService CashierRetailBillService, ICashierSalesBillService CashierSalesBillService, IInventoryTaxService InventoryTaxService, IUserCredentialService IUserCredentialService, IModuleService iIModuleService,
            IUtilityService UtilityService, IRetailBillCreditNoteService RetailBillCreditNoteService, IRetailBillCreditNoteItemService RetailBillCreditNoteItemService, ISalesBillCreditNoteService SalesBillCreditNoteService, ISalesBillCreditNoteItemService SalesBillCreditNoteItemService, IDiscountMasterItemService DiscountMasterItemService, IShopService ShopService)
        {
            this._SalesReturnService = SalesReturnService;
            this._SalesReturnItemService = SalesReturnItemService;
            this._RetailBillService = RetailBillService;
            this._RetailBillItemService = RetailBillItemService;
            this._SalesBillService = SalesBillService;
            this._SalesBillItemService = SalesBillItemService;
            this._ItemService = ItemService;
            this._DesignService = DesignService;
            this._ItemTaxService = ItemTaxService;
            this._ShopStockService = ShopStockService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._OpeningStockService = OpeningStockService;
            this._EntryStockItemService = EntryStockItemService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._IUserCredentialService = IUserCredentialService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierSalesBillService = CashierSalesBillService;
            this._InventoryTaxService = InventoryTaxService;
            this._iIModuleService = iIModuleService;
            this._UtilityService = UtilityService;
            this._RetailBillCreditNoteService = RetailBillCreditNoteService;
            this._RetailBillCreditNoteItemService = RetailBillCreditNoteItemService;
            this._SalesBillCreditNoteService = SalesBillCreditNoteService;
            this._SalesBillCreditNoteItemService = SalesBillCreditNoteItemService;
            this._DiscountMasterItemService = DiscountMasterItemService;
            this._ShopService = ShopService;
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

        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public int Decode(string decodeMe)
        {
            byte[] decoded = Convert.FromBase64String(decodeMe);
            string decodedvalue = System.Text.Encoding.UTF8.GetString(decoded);
            return Convert.ToInt32(decodedvalue);
        }

        [HttpGet]
        public JsonResult EncodeId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SalesReturn()
        {
            MainApplication model = new MainApplication();
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                        Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        break;
                    }
                }
            }

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesReturn = _SalesReturnService.GetLastSalesReturnByFinYr(FinYr, shopcode);
            string SalesReturnCode = string.Empty;
            int SalesReturnLength = 0;
            int SalesReturnNo = 0;
            if (LastSalesReturn == null)
            {
                SalesReturnLength = 1;
                SalesReturnNo = 1;
            }
            else
            {
                int SalesReturnIndex = LastSalesReturn.SalesReturnNo.LastIndexOf('S');
                SalesReturnCode = LastSalesReturn.SalesReturnNo.Substring(SalesReturnIndex + 2, 6);
                SalesReturnLength = (Convert.ToInt32(SalesReturnCode) + 1).ToString().Length;
                SalesReturnNo = Convert.ToInt32(SalesReturnCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            SalesReturnCode = _UtilityService.getName(ShortCode + "/SR", SalesReturnLength, SalesReturnNo);
            SalesReturnCode = SalesReturnCode + FinYr;
            TempData["PreviousSalesReturnNo"] = SalesReturnCode;
            model.SalesReturnNo = SalesReturnCode;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            //GET LOGIN SHOP DETAILS


            return View(model);
        }

        //AUTO COMPLETE BILL NO
        [HttpGet]
        public JsonResult GetBillNos(string SearchTerm, string billtype, string fromdate, string todate)
        {

            //AUTO COMPLETE BILL NO
            if (SearchTerm.ToUpper().Contains("S") && billtype == "Sales Bill")
            {
                var data = _SalesBillService.GetSalesBillByFromAndToDate(Convert.ToDateTime(fromdate), Convert.ToDateTime(todate));
                List<string> No = new List<string>();
                foreach (var item in data)
                {
                    if (Convert.ToDateTime(item.Date).Date != DateTime.Now.Date)
                    {
                        No.Add(item.SalesBillNo);
                    }
                }
                return Json(No, JsonRequestBehavior.AllowGet);
            }
            else if (SearchTerm.ToUpper().Contains("R") && billtype == "Retail Bill")
            {
                var data = _RetailBillService.GetRetailBillByFromAndToDate(Convert.ToDateTime(fromdate), Convert.ToDateTime(todate));
                List<string> No = new List<string>();
                foreach (var item in data)
                {
                    if (Convert.ToDateTime(item.Date).Date != DateTime.Now.Date)
                    {
                        No.Add(item.RetailBillNo);
                    }
                }
                return Json(No, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Error";
                return Json(new { msg, billtype }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetShopQty(string itemcode, string shopcode)
        {
            double? shopqty = 0;
            var ShopStock = _ShopStockService.GetDetailsByItemCodeAndShopCode(itemcode, shopcode);
            if (ShopStock != null)
            {
                shopqty = ShopStock.Quantity;
            }
            return Json(shopqty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopUpPage(string code, string item, string type)
        {
            MainApplication model = new MainApplication();
            model.DesignDetails = _DesignService.GetDetailsByCode(code);
            if (type == "Inventory")
            {
                model.ItemDetails = _ItemService.GetDescriptionByItemCode(item);
            }
            else
            {
                model.NonInventoryItemDetails = _NonInventoryItemService.GetDetailsByItemCode(item);
            }
            return View(model);
        }

        //DISPLAYSALES BILL DETAILS ON POP UP PAGE
        public ActionResult SalesBillDetailsPop(string no)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillService.GetDetailsBySalesBillNo(no);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(no);
            return View(model);
        }

        //DISPLAY RETAIL INVOICE DETAILS ON POP UP PAGE
        public ActionResult RIBillDetailsPop(string no)
        {
            MainApplication model = new MainApplication();
            model.RetailBillDetails = _RetailBillService.GetDetailsByInvoiceNo(no);
            model.RetailBillItemList = _RetailBillItemService.GetDetailsByInvoiceNo(no);
            return View(model);
        }

        [HttpGet]
        //DISPLAY REFUND ITEM DETAILS IN MAIN PAGE
        public ActionResult GetReturnItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            if (BillNumber.Contains("R"))
            {
                model.RetailBillDetails = _RetailBillService.GetDetailsByInvoiceNo(BillNumber);
                model.RetailBillItemList = _RetailBillItemService.GetDetailsByRetailBillNo(BillNumber);
                model.RetailBillCreditNoteItemList = new List<RetailBillCreditNoteItem>();
                model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.RetailBillDetails.RetailBillNo);
                if (model.RetailBillDetails.SalesReturn != "No")
                {
                    model.RetailBillItemList = new List<RetailBillItem>();
                    var LastSalesReturnNo = _SalesReturnService.GetLastSalesOfBillNo(BillNumber);
                    model.SalesReturnDetails = LastSalesReturnNo;
                    model.RetailBillCreditNoteItemList = _RetailBillCreditNoteItemService.GetAllItemsByCreditNote(LastSalesReturnNo.CreditNoteNo);
                    model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(BillNumber);
                }
                TempData["BillDetails"] = model.RetailBillDetails;
                TempData["CreditNoteItemList"] = model.RetailBillCreditNoteItemList;
                TempData["BillItemList"] = model.RetailBillItemList;
            }
            else
            {
                model.SalesBillDetails = _SalesBillService.GetDetailsBySalesBillNo(BillNumber);
                model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(BillNumber);
                model.SalesBillCreditNoteItemList = new List<SalesBillCreditNoteItem>();
                model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesBillDetails.SalesBillNo);
                if (model.SalesBillDetails.SalesReturn != "No")
                {
                    model.SalesBillItemList = new List<SalesBillItem>();
                    var LastSalesReturnNo = _SalesReturnService.GetLastSalesOfBillNo(BillNumber);
                    model.SalesReturnDetails = LastSalesReturnNo;
                    model.SalesBillCreditNoteItemList = _SalesBillCreditNoteItemService.GetAllItemsByCreditNote(LastSalesReturnNo.CreditNoteNo);
                    model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(BillNumber);
                }
                TempData["BillDetails"] = model.SalesBillDetails;
                TempData["CreditNoteItemList"] = model.SalesBillCreditNoteItemList;
                TempData["BillItemList"] = model.SalesBillItemList;
            }
            return View(model);
        }

        //GET DETAILS BY BILL NO
        [HttpGet]
        public ActionResult GetDataByBillNo(string BillNo)
        {
            if (BillNo.Contains("RI"))
            {
                var data = _RetailBillService.GetDetailsByInvoiceNo(BillNo);
                return Json(new { data.ClientName, data.ClientContact }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _SalesBillService.GetDetailsBySalesBillNo(BillNo);
                return Json(new
                {
                    data.ClientName,
                    data.ClientContactNo,
                    data.ClientState,
                    data.ClientType,
                    data.ConsumeResell,
                    data.FormType,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //ITEM LIST FROM STOCK DISTRIBUTION AND SHOP STOCK
        [HttpGet]
        public JsonResult GetItemsFromShopStockAndStkDistrbtn(string ItemType, string LoginShop)
        {
            if (ItemType == "Inventory")
            {
                var stkdistrbtnlist = _ShopStockService.GetRowsByShopCode(LoginShop);
                var items = stkdistrbtnlist.Select(m => new SelectListItem
                {
                    Text = m.ItemName,
                    Value = m.ItemCode
                });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var noninvitems = _NonInventoryItemService.GetAll();
                var items = noninvitems.Select(m => new SelectListItem
                {
                    Text = m.itemName,
                    Value = m.itemCode
                });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetItemDetailsFromItems(string itemcode, string Type, string State, string Formtype, string ConsumeResell, string billno)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(itemcode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            double? shopqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(itemcode, Session["LOGINSHOPGODOWNCODE"].ToString()).Quantity;
            var itemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(itemcode, Session["LOGINSHOPGODOWNCODE"].ToString());
            string typematerial = itemdetails.Material;
            string colorname = itemdetails.Color;
            double taxPercent = 0;
            if (billno.Contains("R"))
            {
                var Tax = _ItemTaxService.GetLatestTax("VAT", itemdetails.SubCategory);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
            }
            else
            {
                if (State == "Maharashtra")
                {
                    if (Type == "Registered")
                    {
                        if (ConsumeResell != "Consumer")
                        {
                            taxPercent = 0;
                        }
                        else
                        {
                            var Tax = _ItemTaxService.GetLatestTax("VAT", itemdetails.SubCategory);
                            if (Tax != null)
                            {
                                taxPercent = Tax.Percentage;
                            }
                            else { taxPercent = 0; }
                        }
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", itemdetails.SubCategory);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    if (Type == "Registered")
                    {
                        if (ConsumeResell == "Reseller")
                        {
                            var Tax = _ItemTaxService.GetLatestTax("CST", itemdetails.SubCategory);
                            if (Tax != null)
                            {
                                if (Formtype == "C-Form")
                                {
                                    taxPercent = 2;
                                }
                                else
                                {
                                    taxPercent = Tax.Percentage;
                                }
                            }
                            else { taxPercent = 0; }

                        }
                        else
                        {
                            var Tax = _ItemTaxService.GetLatestTax("CST", itemdetails.SubCategory);
                            if (Tax != null)
                            {
                                taxPercent = Tax.Percentage;
                            }
                            else { taxPercent = 0; }
                        }
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", itemdetails.SubCategory);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
            }
            return Json(new { discount, discountrps, shopqty, taxPercent, itemdetails.Barcode, itemdetails.NumberType, itemdetails.Unit, itemdetails.ItemName, itemdetails.SellingPrice, itemdetails.MRP, itemdetails.Description, typematerial, itemdetails.DesignCode, itemdetails.DesignName, colorname, itemdetails.Brand, itemdetails.Size }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNonInvItemdetailsByItemCodeforQuot(string ItemCode, string Type, string State, string FormType, string ConsumeResell, string billno)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            //GET SUBCATEGORY USING ITEMCODE 
            double taxPercent = 0;
            var data = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            if (billno.Contains("R"))
            {
                var Tax = _ItemTaxService.GetLatestTax("VAT", data.itemSubCategory);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
            }
            else
            {
                if (State == "Maharashtra")
                {
                    if (Type == "Registered")
                    {
                        if (ConsumeResell != "Consumer")
                        {
                            taxPercent = 0;
                        }
                        else
                        {
                            var Tax = _ItemTaxService.GetLatestTax("VAT", data.itemSubCategory);
                            if (Tax != null)
                            {
                                taxPercent = Tax.Percentage;
                            }
                            else { taxPercent = 0; }
                        }
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", data.itemSubCategory);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    if (Type == "Registered")
                    {
                        if (ConsumeResell == "Reseller")
                        {
                            if (FormType == "C-Form")
                            {
                                taxPercent = 2;
                            }
                            else
                            {
                                var Tax = _ItemTaxService.GetLatestTax("CST", data.itemSubCategory);
                                if (Tax != null)
                                {
                                    taxPercent = Tax.Percentage;
                                }
                                else { taxPercent = 0; }
                            }
                        }
                        else
                        {
                            var Tax = _ItemTaxService.GetLatestTax("CST", data.itemSubCategory);
                            if (Tax != null)
                            {
                                taxPercent = Tax.Percentage;
                            }
                            else { taxPercent = 0; }
                        }
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", data.itemSubCategory);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
            }
            if (data != null)
            {
                return Json(new { discount, discountrps, taxPercent, data.NumberType, data.itemCode, data.itemName, data.description, data.colorCode, data.unit, data.sellingprice, data.mrp, data.typeOfMaterial, data.designCode, data.designName, data.brandName, data.size }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        // PARTIAL VIEW OF CASH PAYMENT DETAILS
        public ActionResult CashPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CREDITCARD PAYMENT DETAILS
        public ActionResult CardPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS
        public ActionResult ChequePayment()
        {
            return View();
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS.
        public ActionResult CashCardChequePayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SalesReturn(MainApplication model, FormCollection frmcol)
        {
            string loginshopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string loginshopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesReturn = _SalesReturnService.GetLastSalesReturnByFinYr(FinYr, loginshopcode);
            string SalesReturnCode = string.Empty;
            int SalesReturnLength = 0;
            int SalesReturnNo = 0;
            if (LastSalesReturn == null)
            {
                SalesReturnLength = 1;
                SalesReturnNo = 1;
            }
            else
            {
                int SalesReturnIndex = LastSalesReturn.SalesReturnNo.LastIndexOf('S');
                SalesReturnCode = LastSalesReturn.SalesReturnNo.Substring(SalesReturnIndex + 2, 6);
                SalesReturnLength = (Convert.ToInt32(SalesReturnCode) + 1).ToString().Length;
                SalesReturnNo = Convert.ToInt32(SalesReturnCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(loginshopname).ShortCode;
            SalesReturnCode = _UtilityService.getName(ShortCode + "/SR", SalesReturnLength, SalesReturnNo);
            SalesReturnCode = SalesReturnCode + FinYr;
            model.SalesReturnNo = SalesReturnCode;

            string CreditNoteCode = string.Empty;
            int CreditNoteLength = 0;
            int CreditNoteNo = 0;
            if (frmcol["BillNo"].Contains("R"))
            {
                var LastCreditNote = _RetailBillCreditNoteService.GetLastCreditNoteByFinYr(FinYr, loginshopcode);
                if (LastCreditNote == null)
                {
                    CreditNoteLength = 1;
                    CreditNoteNo = 1;
                }
                else
                {
                    int CreditNoteIndex = LastCreditNote.CreditNoteNo.LastIndexOf('R');
                    CreditNoteCode = LastCreditNote.CreditNoteNo.Substring(CreditNoteIndex + 4, 6);
                    CreditNoteLength = (Convert.ToInt32(CreditNoteCode) + 1).ToString().Length;
                    CreditNoteNo = Convert.ToInt32(CreditNoteCode) + 1;
                }
                CreditNoteCode = _UtilityService.getName(ShortCode + "/RBCD", CreditNoteLength, CreditNoteNo);
                CreditNoteCode = CreditNoteCode + FinYr;
            }
            else
            {
                var LastCreditNote = _SalesBillCreditNoteService.GetLastCreditNoteByFinYr(FinYr, loginshopcode);
                if (LastCreditNote == null)
                {
                    CreditNoteLength = 1;
                    CreditNoteNo = 1;
                }
                else
                {
                    int CreditNoteIndex = LastCreditNote.CreditNoteNo.LastIndexOf('S');
                    CreditNoteCode = LastCreditNote.CreditNoteNo.Substring(CreditNoteIndex + 4, 6);
                    CreditNoteLength = (Convert.ToInt32(CreditNoteCode) + 1).ToString().Length;
                    CreditNoteNo = Convert.ToInt32(CreditNoteCode) + 1;
                }
                CreditNoteCode = _UtilityService.getName(ShortCode + "/SBCD", CreditNoteLength, CreditNoteNo);
                CreditNoteCode = CreditNoteCode + FinYr;
            }

            //Save Retail Bill Details In Sales Return
            if (frmcol["BillNo"].Contains("R"))
            {
                var retailbilldata = TempData["BillDetails"] as RetailBill;
                model.SalesReturnDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                model.SalesReturnDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                model.SalesReturnDetails.SalesReturnNo = model.SalesReturnNo;
                model.SalesReturnDetails.SalesReturnDate = DateTime.Now;
                model.SalesReturnDetails.ClientName = frmcol["hdnClientName"];
                model.SalesReturnDetails.ClientContact = frmcol["hdnClientContact"];
                model.SalesReturnDetails.BillNo = frmcol["BillNo"];
                model.SalesReturnDetails.OldGrandTotal = retailbilldata.GrandTotal;
                model.SalesReturnDetails.TotalAmount = model.SalesReturnDetails.TotalAmount - Convert.ToDouble(frmcol["NewItemTotalAmount"]);
                model.SalesReturnDetails.TotalTaxAmount = model.SalesReturnDetails.TotalTaxAmount - Convert.ToDouble(frmcol["NewItemTotalTaxAmount"]);
                model.SalesReturnDetails.PackAndForwd = 0;
                model.SalesReturnDetails.GrandTotal = model.SalesReturnDetails.GrandTotal - Convert.ToDouble(frmcol["NewItemGrandAmount"]);
                model.SalesReturnDetails.Payment = retailbilldata.Payment;
                model.SalesReturnDetails.AdjustedAmount = retailbilldata.AdjustedAmount;
                var LastSales = _SalesReturnService.GetLastSalesOfBillNo(frmcol["BillNo"]);
                if (LastSales != null)
                {
                    model.SalesReturnDetails.OldGrandTotal = LastSales.GrandTotal;
                    model.SalesReturnDetails.AdjustedAmount = LastSales.AdjustedAmount;
                    model.SalesReturnDetails.Payment = LastSales.Payment;
                }
                double? remainingamount = model.SalesReturnDetails.GrandTotal - (model.SalesReturnDetails.Payment + model.SalesReturnDetails.AdjustedAmount);
                if (remainingamount <= 0)
                {
                    model.SalesReturnDetails.Balance = 0;
                    model.SalesReturnDetails.Refund = 0 - remainingamount;
                }
                else
                {
                    model.SalesReturnDetails.Refund = 0;
                    model.SalesReturnDetails.Balance = remainingamount;
                }
                model.SalesReturnDetails.RefundToClient = 0;
                model.SalesReturnDetails.AdjustedBill = retailbilldata.AdjustedBill;
                model.SalesReturnDetails.CreditNoteNo = CreditNoteCode;
                model.SalesReturnDetails.CreditNoteDate = DateTime.Now;
                model.SalesReturnDetails.CreditNoteAmount = Math.Round(Convert.ToDouble(frmcol["RefundableAmount"]));
                model.SalesReturnDetails.BillBalance = retailbilldata.BillBalance;
                if (model.SalesReturnDetails.Balance == 0 && model.SalesReturnDetails.Refund == 0)
                {
                    model.SalesReturnDetails.CashierStatus = "InActive";
                    model.SalesReturnDetails.BalanceStatus = "InActive";
                }
                else if (model.SalesReturnDetails.Balance == 0)
                {
                    model.SalesReturnDetails.CashierStatus = "Active";
                    model.SalesReturnDetails.BalanceStatus = "InActive";
                }
                else
                {
                    model.SalesReturnDetails.CashierStatus = "Active";
                    model.SalesReturnDetails.BalanceStatus = "Active";
                }
                model.SalesReturnDetails.Status = "Active";
                model.SalesReturnDetails.ModifiedOn = DateTime.Now;
                model.SalesReturnDetails.TaxStatus = retailbilldata.TaxStatus;
                var list = _SalesReturnService.GetAllSalesOfBill(frmcol["BillNo"]);
                foreach (var sales in list)
                {
                    if (sales.Status == "Active")
                    {
                        sales.Status = "InActive";
                        _SalesReturnService.Update(sales);
                    }
                }
                _SalesReturnService.Create(model.SalesReturnDetails);

                if (model.SalesReturnDetails.Balance == 0 && model.SalesReturnDetails.Refund == 0)
                {
                    retailbilldata.CashierStatus = "InActive";
                    retailbilldata.Status = "InActive";
                }
                else if (model.SalesReturnDetails.Balance == 0)
                {
                    retailbilldata.Status = "InActive";
                    retailbilldata.CashierStatus = "Active";
                }
                else
                {
                    retailbilldata.CashierStatus = "Active";
                    retailbilldata.Status = "Active";
                }
                _RetailBillService.Update(retailbilldata);

                RetailBillCreditNote CreditNote = new RetailBillCreditNote();
                CreditNote.CreditNoteNo = CreditNoteCode;
                CreditNote.ShopCode = model.SalesReturnDetails.ShopCode;
                CreditNote.ShopName = model.SalesReturnDetails.ShopName;
                CreditNote.BillNo = frmcol["BillNo"];
                CreditNote.BillType = "Retail Bill";
                CreditNote.ClientName = frmcol["hdnClientName"];
                CreditNote.Date = DateTime.Now;
                CreditNote.SalesReturnNo = model.SalesReturnNo;
                CreditNote.Amount = Math.Round(Convert.ToDouble(frmcol["RefundableAmount"]));
                CreditNote.Status = "Active";
                CreditNote.ModifiedBy = DateTime.Now;
                _RetailBillCreditNoteService.Create(CreditNote);
            }
            //Save Sales Bill Details In Sales Return
            else
            {
                var salesbilldata = TempData["BillDetails"] as SalesBill;
                model.SalesReturnDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                model.SalesReturnDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                model.SalesReturnDetails.SalesReturnNo = model.SalesReturnNo;
                model.SalesReturnDetails.SalesReturnDate = DateTime.Now;
                model.SalesReturnDetails.ClientName = frmcol["hdnClientName"];
                model.SalesReturnDetails.ClientContact = frmcol["hdnClientContact"];
                model.SalesReturnDetails.BillNo = frmcol["BillNo"];
                model.SalesReturnDetails.OldGrandTotal = salesbilldata.GrandTotal;
                model.SalesReturnDetails.TotalAmount = model.SalesReturnDetails.TotalAmount - Convert.ToDouble(frmcol["NewItemTotalAmount"]);
                model.SalesReturnDetails.TotalTaxAmount = model.SalesReturnDetails.TotalTaxAmount - Convert.ToDouble(frmcol["NewItemTotalTaxAmount"]);
                if (model.SalesBillDetails != null)
                {
                    model.SalesReturnDetails.PackAndForwd = model.SalesBillDetails.PackAndForwd;
                }

                model.SalesReturnDetails.GrandTotal = model.SalesReturnDetails.GrandTotal - Convert.ToDouble(frmcol["NewItemGrandAmount"]);
                model.SalesReturnDetails.Payment = salesbilldata.PaymentReceived;
                model.SalesReturnDetails.AdjustedAmount = salesbilldata.AdjustedAmount;
                model.SalesReturnDetails.BillDiscount = salesbilldata.BillDiscount;
                var LastSales = _SalesReturnService.GetLastSalesOfBillNo(frmcol["BillNo"]);
                if (LastSales != null)
                {
                    model.SalesReturnDetails.OldGrandTotal = LastSales.GrandTotal;
                    model.SalesReturnDetails.AdjustedAmount = LastSales.AdjustedAmount;
                    model.SalesReturnDetails.Payment = LastSales.Payment;
                }
                double? remainingamount = model.SalesReturnDetails.GrandTotal - (model.SalesReturnDetails.Payment + model.SalesReturnDetails.AdjustedAmount);
                if (remainingamount <= 0)
                {
                    model.SalesReturnDetails.Balance = 0;
                    model.SalesReturnDetails.Refund = 0 - remainingamount;
                }
                else
                {
                    model.SalesReturnDetails.Refund = 0;
                    model.SalesReturnDetails.Balance = remainingamount;
                }

                model.SalesReturnDetails.RefundToClient = 0;
                model.SalesReturnDetails.AdjustedBill = salesbilldata.AdjustedBill;
                model.SalesReturnDetails.CreditNoteNo = CreditNoteCode;
                model.SalesReturnDetails.CreditNoteDate = DateTime.Now;
                model.SalesReturnDetails.CreditNoteAmount = Math.Round(Convert.ToDouble(frmcol["RefundableAmount"]));
                model.SalesReturnDetails.BalanceStatus = "Active";

                if (model.SalesReturnDetails.Balance == 0 && model.SalesReturnDetails.Refund == 0)
                {
                    model.SalesReturnDetails.CashierStatus = "InActive";
                    model.SalesReturnDetails.BalanceStatus = "InActive";
                }
                else if (model.SalesReturnDetails.Balance == 0)
                {
                    model.SalesReturnDetails.CashierStatus = "Active";
                    model.SalesReturnDetails.BalanceStatus = "InActive";
                }
                else
                {
                    model.SalesReturnDetails.CashierStatus = "Active";
                    model.SalesReturnDetails.BalanceStatus = "Active";
                }
                model.SalesReturnDetails.Status = "Active";
                model.SalesReturnDetails.ModifiedOn = DateTime.Now;
                model.SalesReturnDetails.TaxStatus = salesbilldata.TaxStatus;
                var list = _SalesReturnService.GetAllSalesOfBill(frmcol["BillNo"]);
                foreach (var sales in list)
                {
                    if (sales.Status == "Active")
                    {
                        sales.Status = "InActive";
                        _SalesReturnService.Update(sales);
                    }
                }
                _SalesReturnService.Create(model.SalesReturnDetails);

                if (model.SalesReturnDetails.Balance == 0 && model.SalesReturnDetails.Refund == 0)
                {
                    salesbilldata.CashierStatus = "InActive";
                    salesbilldata.Status = "InActive";
                }
                else if (model.SalesReturnDetails.Balance == 0)
                {
                    salesbilldata.Status = "InActive";
                    salesbilldata.CashierStatus = "Active";
                }
                else
                {
                    salesbilldata.CashierStatus = "Active";
                    salesbilldata.Status = "Active";
                }
                _SalesBillService.Update(salesbilldata);

                SalesBillCreditNote SalesBillCreditNote = new SalesBillCreditNote();
                SalesBillCreditNote.CreditNoteNo = CreditNoteCode;
                SalesBillCreditNote.ShopCode = model.SalesReturnDetails.ShopCode;
                SalesBillCreditNote.ShopName = model.SalesReturnDetails.ShopName;
                SalesBillCreditNote.BillNo = frmcol["BillNo"];
                SalesBillCreditNote.BillType = "Sales Bill";
                SalesBillCreditNote.ClientName = frmcol["hdnClientName"];
                SalesBillCreditNote.Date = DateTime.Now;
                SalesBillCreditNote.SalesReturnNo = model.SalesReturnNo;
                SalesBillCreditNote.Amount = Math.Round(Convert.ToDouble(frmcol["RefundableAmount"]));
                SalesBillCreditNote.Status = "Active";
                SalesBillCreditNote.ModifiedBy = DateTime.Now;
                _SalesBillCreditNoteService.Create(SalesBillCreditNote);
            }

            //Save Retail Bill Item Details In Sales Return Item
            if (frmcol["BillNo"].Contains("R"))
            {
                model.RetailBillItemList = TempData["BillItemList"] as IEnumerable<RetailBillItem>;
                int count = 1;

                foreach (var item in model.RetailBillItemList)
                {
                    string checkbox = "CheckBox" + count;
                    string quantity = "quantity" + count;
                    string amount = "amountvalue" + count;
                    string prevqtyvalue = "prevquantityvalue" + count;
                    string itemtaxamt = "ItemTaxAmt" + count;
                    string discountrps = "discountrps" + count;
                    //Initialize SalesReturnItems And Save
                    model.SalesReturnItemDetails = new SalesReturnItem();
                    model.SalesReturnItemDetails.SalesReturnNo = model.SalesReturnNo;
                    model.SalesReturnItemDetails.BillNo = frmcol["BillNo"];
                    model.SalesReturnItemDetails.Barcode = item.Barcode;
                    model.SalesReturnItemDetails.ItemCode = item.ItemCode;
                    model.SalesReturnItemDetails.ItemName = item.ItemName;
                    model.SalesReturnItemDetails.ItemType = item.ItemType;
                    model.SalesReturnItemDetails.Narration = item.Narration;
                    model.SalesReturnItemDetails.Description = item.Description;
                    model.SalesReturnItemDetails.Material = item.Material;
                    model.SalesReturnItemDetails.Color = item.Color;
                    model.SalesReturnItemDetails.DesignCode = item.DesignCode;
                    model.SalesReturnItemDetails.DesignName = item.DesignName;
                    model.SalesReturnItemDetails.Brand = item.Brand;
                    model.SalesReturnItemDetails.Size = item.Size;
                    model.SalesReturnItemDetails.Unit = item.Unit;
                    model.SalesReturnItemDetails.NumberType = item.NumberType;
                    model.SalesReturnItemDetails.SellingPrice = item.SellingPrice;
                    model.SalesReturnItemDetails.MRP = item.MRP;
                    model.SalesReturnItemDetails.DiscPer = item.DiscountPercent;
                    model.SalesReturnItemDetails.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                    if (frmcol[checkbox] == "Yes")
                    {
                        model.SalesReturnItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                    }
                    else
                    {
                        model.SalesReturnItemDetails.Quantity = Convert.ToDouble(frmcol[prevqtyvalue]);
                    }
                    model.SalesReturnItemDetails.Status = "Active";
                    model.SalesReturnItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                    model.SalesReturnItemDetails.ItemTax = item.ItemTax;
                    model.SalesReturnItemDetails.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                    model.SalesReturnItemDetails.ModifiedOn = DateTime.Now;
                    if (model.SalesReturnItemDetails.Quantity != 0)
                    {
                        _SalesReturnItemService.Create(model.SalesReturnItemDetails);
                    }

                    RetailBillCreditNoteItem CreditNoteItem = new RetailBillCreditNoteItem();
                    CreditNoteItem.BillNo = frmcol["BillNo"];
                    CreditNoteItem.CreditNoteNo = CreditNoteCode;
                    CreditNoteItem.Barcode = item.Barcode;
                    CreditNoteItem.ItemCode = item.ItemCode;
                    CreditNoteItem.ItemName = item.ItemName;
                    CreditNoteItem.ItemType = item.ItemType;
                    CreditNoteItem.Narration = item.Narration;
                    CreditNoteItem.Description = item.Description;
                    CreditNoteItem.Color = item.Color;
                    CreditNoteItem.Material = item.Material;
                    CreditNoteItem.DesignCode = item.DesignCode;
                    CreditNoteItem.DesignName = item.DesignName;
                    CreditNoteItem.Brand = item.Brand;
                    CreditNoteItem.Size = item.Size;
                    CreditNoteItem.Unit = item.Unit;
                    CreditNoteItem.NumberType = item.NumberType;
                    CreditNoteItem.SellingPrice = item.SellingPrice;
                    CreditNoteItem.MRP = item.MRP;
                    CreditNoteItem.DiscPer = item.DiscountPercent;

                    if (frmcol[checkbox] == "Yes")
                    {
                        CreditNoteItem.Quantity = Convert.ToDouble(frmcol[quantity]);
                        CreditNoteItem.Balance = Convert.ToDouble(frmcol[prevqtyvalue]) - Convert.ToDouble(frmcol[quantity]);
                        CreditNoteItem.Amount = Convert.ToDouble(frmcol[amount]);
                        CreditNoteItem.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                        CreditNoteItem.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                        CreditNoteItem.Status = "Return";
                    }
                    else
                    {
                        CreditNoteItem.Quantity = 0;
                        CreditNoteItem.Balance = Convert.ToDouble(frmcol[prevqtyvalue]);
                        CreditNoteItem.Amount = 0;
                        CreditNoteItem.ItemTaxAmt = 0;
                        CreditNoteItem.DiscRs = 0;
                        CreditNoteItem.Status = "Active";
                    }
                    CreditNoteItem.ItemTax = item.ItemTax;
                    CreditNoteItem.ModifiedOn = DateTime.Now;
                    _RetailBillCreditNoteItemService.Create(CreditNoteItem);

                    if (frmcol[checkbox] == "Yes")
                    {
                        var itemdata = _RetailBillItemService.GetItemDetailsByBillNoAndItemCode(frmcol["BillNo"], item.ItemCode);
                        itemdata.SalesReturn = "Yes";
                        _RetailBillItemService.Update(itemdata);

                        //ADD REFUND ITEM QUANTITY TO SHOP STOCK QUANTITY
                        string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        if (item.ItemType == "Inventory")
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.SalesReturnItemDetails.ItemCode, shopcode);
                            if (shopdetails != null)
                            {
                                shopdetails.Quantity = shopdetails.Quantity + model.SalesReturnItemDetails.Quantity;
                                _ShopStockService.Update(shopdetails);
                            }
                            else
                            {
                                ShopStock ShopStock = new ShopStock();
                                ShopStock.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                                ShopStock.ShopName = Session["SHOPGODOWNNAME"].ToString();
                                ShopStock.Barcode = item.Barcode;
                                ShopStock.ItemCode = item.ItemCode;
                                ShopStock.ItemName = item.ItemName;
                                ShopStock.Description = item.Description;
                                ShopStock.Color = item.Color;
                                ShopStock.Material = item.Material;
                                ShopStock.DesignCode = item.DesignCode;
                                ShopStock.DesignName = item.DesignName;
                                ShopStock.Brand = item.Brand;
                                ShopStock.Size = item.Size;
                                ShopStock.Quantity = Convert.ToDouble(frmcol[quantity]);
                                ShopStock.Unit = item.Unit;
                                ShopStock.NumberType = item.NumberType;
                                ShopStock.SellingPrice = item.SellingPrice;
                                ShopStock.MRP = item.MRP;
                                ShopStock.Status = "Active";
                                ShopStock.ModifiedOn = DateTime.Now;
                                _ShopStockService.Create(ShopStock);
                            }

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.SalesReturnItemDetails.ItemCode, shopcode);
                            if (stockitemdetails != null)
                            {
                                stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity + model.SalesReturnItemDetails.Quantity;
                                _StockItemDistributionService.Update(stockitemdetails);
                            }
                            else
                            {
                                var ItemDetails = _ItemService.GetDescriptionByItemCode(item.ItemCode);
                                StockItemDistribution StockItemDistribution = new StockItemDistribution();
                                StockItemDistribution.Code = Session["LOGINSHOPGODOWNCODE"].ToString();
                                StockItemDistribution.GodownShopName = Session["SHOPGODOWNNAME"].ToString();
                                StockItemDistribution.Category = ItemDetails.itemCategory;
                                StockItemDistribution.SubCategory = ItemDetails.itemSubCategory;
                                StockItemDistribution.Barcode = ItemDetails.Barcode;
                                StockItemDistribution.ItemCode = item.ItemCode;
                                StockItemDistribution.ItemName = item.ItemName;
                                StockItemDistribution.Description = item.Description;
                                StockItemDistribution.Color = item.Color;
                                StockItemDistribution.Material = item.Material;
                                StockItemDistribution.DesignCode = item.DesignCode;
                                StockItemDistribution.DesignName = item.DesignName;
                                StockItemDistribution.Brand = item.Brand;
                                StockItemDistribution.Size = item.Size;
                                StockItemDistribution.Unit = item.Unit;
                                StockItemDistribution.NumberType = item.NumberType;
                                StockItemDistribution.ItemQuantity = Convert.ToDouble(frmcol[quantity]);
                                StockItemDistribution.SellingPrice = item.SellingPrice;
                                StockItemDistribution.MRP = item.MRP;
                                StockItemDistribution.Status = "Active";
                                StockItemDistribution.ModifiedOn = DateTime.Now;
                                _StockItemDistributionService.Create(StockItemDistribution);
                            }

                            var entrystockitems = _EntryStockItemService.getDetailsByItemCode(model.SalesReturnItemDetails.ItemCode);
                            if (entrystockitems != null)
                            {
                                entrystockitems.TotalQuantity = entrystockitems.TotalQuantity + model.SalesReturnItemDetails.Quantity;
                                _EntryStockItemService.Update(entrystockitems);
                            }
                            else
                            {
                                var openingstockitems = _OpeningStockService.GetDetailsByItemCode(item.ItemCode);
                                openingstockitems.TotalQuantity = openingstockitems.TotalQuantity + model.SalesReturnItemDetails.Quantity;
                                _OpeningStockService.UpdateStock(openingstockitems);
                            }
                        }
                    }
                    count++;
                }

                model.RetailBillCreditNoteItemList = TempData["CreditNoteItemList"] as IEnumerable<RetailBillCreditNoteItem>;
                int salescount = model.RetailBillItemList.Count() + 1;
                foreach (var item in model.RetailBillCreditNoteItemList)
                {
                    string checkbox = "CheckBox" + salescount;
                    string quantity = "quantity" + salescount;
                    string amount = "amountvalue" + salescount;
                    string prevqtyvalue = "prevquantityvalue" + salescount;
                    string itemtaxamt = "ItemTaxAmt" + salescount;
                    string discountrps = "discountrps" + salescount;
                    if (frmcol[checkbox] == "Yes")
                    {
                        item.CreditNoteNo = CreditNoteCode;
                        item.Quantity = Convert.ToDouble(frmcol[quantity]);
                        item.Balance = Convert.ToDouble(frmcol[prevqtyvalue]) - Convert.ToDouble(frmcol[quantity]);
                        item.Amount = Convert.ToDouble(frmcol[amount]);
                        item.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                        item.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                        item.Status = "Return";
                        item.ModifiedOn = DateTime.Now;
                        _RetailBillCreditNoteItemService.Create(item);

                        //var itemdata = _RetailBillItemService.GetItemDetailsByBillNoAndItemCode(frmcol["BillNo"], item.ItemCode);
                        //itemdata.SalesReturn = "Yes";
                        //_RetailBillItemService.Update(itemdata);

                        //ADD REFUND ITEM QUANTITY TO SHOP STOCK QUANTITY
                        string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        if (item.ItemType == "Inventory")
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, shopcode);
                            if (shopdetails != null)
                            {
                                shopdetails.Quantity = shopdetails.Quantity + item.Quantity;
                                _ShopStockService.Update(shopdetails);
                            }
                            else
                            {
                                ShopStock ShopStock = new ShopStock();
                                ShopStock.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                                ShopStock.ShopName = Session["SHOPGODOWNNAME"].ToString();
                                ShopStock.Barcode = item.Barcode;
                                ShopStock.ItemCode = item.ItemCode;
                                ShopStock.ItemName = item.ItemName;
                                ShopStock.Description = item.Description;
                                ShopStock.Color = item.Color;
                                ShopStock.Material = item.Material;
                                ShopStock.DesignCode = item.DesignCode;
                                ShopStock.DesignName = item.DesignName;
                                ShopStock.Brand = item.Brand;
                                ShopStock.Size = item.Size;
                                ShopStock.Quantity = Convert.ToDouble(frmcol[quantity]);
                                ShopStock.Unit = item.Unit;
                                ShopStock.NumberType = item.NumberType;
                                ShopStock.SellingPrice = item.SellingPrice;
                                ShopStock.MRP = item.MRP;
                                ShopStock.Status = "Active";
                                ShopStock.ModifiedOn = DateTime.Now;
                                _ShopStockService.Create(ShopStock);
                            }

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, shopcode);
                            if (stockitemdetails != null)
                            {
                                stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity + item.Quantity;
                                _StockItemDistributionService.Update(stockitemdetails);
                            }
                            else
                            {
                                var ItemDetails = _ItemService.GetDescriptionByItemCode(item.ItemCode);
                                StockItemDistribution StockItemDistribution = new StockItemDistribution();
                                StockItemDistribution.Code = Session["LOGINSHOPGODOWNCODE"].ToString();
                                StockItemDistribution.GodownShopName = Session["SHOPGODOWNNAME"].ToString();
                                StockItemDistribution.Category = ItemDetails.itemCategory;
                                StockItemDistribution.SubCategory = ItemDetails.itemSubCategory;
                                StockItemDistribution.Barcode = ItemDetails.Barcode;
                                StockItemDistribution.ItemCode = item.ItemCode;
                                StockItemDistribution.ItemName = item.ItemName;
                                StockItemDistribution.Description = item.Description;
                                StockItemDistribution.Color = item.Color;
                                StockItemDistribution.Material = item.Material;
                                StockItemDistribution.DesignCode = item.DesignCode;
                                StockItemDistribution.DesignName = item.DesignName;
                                StockItemDistribution.Brand = item.Brand;
                                StockItemDistribution.Size = item.Size;
                                StockItemDistribution.Unit = item.Unit;
                                StockItemDistribution.NumberType = item.NumberType;
                                StockItemDistribution.ItemQuantity = Convert.ToDouble(frmcol[quantity]);
                                StockItemDistribution.SellingPrice = item.SellingPrice;
                                StockItemDistribution.MRP = item.MRP;
                                StockItemDistribution.Status = "Active";
                                StockItemDistribution.ModifiedOn = DateTime.Now;
                                _StockItemDistributionService.Create(StockItemDistribution);
                            }

                            var entrystockitems = _EntryStockItemService.getDetailsByItemCode(item.ItemCode);
                            if (entrystockitems != null)
                            {
                                entrystockitems.TotalQuantity = entrystockitems.TotalQuantity + item.Quantity;
                                _EntryStockItemService.Update(entrystockitems);
                            }
                            else
                            {
                                var openingstockitems = _OpeningStockService.GetDetailsByItemCode(item.ItemCode);
                                openingstockitems.TotalQuantity = openingstockitems.TotalQuantity + item.Quantity;
                                _OpeningStockService.UpdateStock(openingstockitems);
                            }
                        }
                    }
                    else
                    {
                        item.CreditNoteNo = CreditNoteCode;
                        item.Quantity = 0;
                        item.ItemTaxAmt = 0;
                        item.Amount = 0;
                        item.DiscRs = 0;
                        _RetailBillCreditNoteItemService.Create(item);
                    }

                    SalesReturnItem SalesReturnItem = new SalesReturnItem();
                    SalesReturnItem.SalesReturnNo = model.SalesReturnNo;
                    SalesReturnItem.BillNo = frmcol["BillNo"];
                    SalesReturnItem.Barcode = item.Barcode;
                    SalesReturnItem.ItemCode = item.ItemCode;
                    SalesReturnItem.ItemName = item.ItemName;
                    SalesReturnItem.ItemType = item.ItemType;
                    SalesReturnItem.Narration = item.Narration;
                    SalesReturnItem.Description = item.Description;
                    SalesReturnItem.Color = item.Color;
                    SalesReturnItem.Material = item.Material;
                    SalesReturnItem.Brand = item.Brand;
                    SalesReturnItem.Size = item.Size;
                    SalesReturnItem.DesignCode = item.DesignCode;
                    SalesReturnItem.DesignName = item.DesignName;
                    if (frmcol[checkbox] == "Yes")
                    {
                        SalesReturnItem.Quantity = Convert.ToDouble(frmcol[prevqtyvalue]) - Convert.ToDouble(frmcol[quantity]);
                    }
                    else
                    {
                        SalesReturnItem.Quantity = Convert.ToDouble(frmcol[prevqtyvalue]);
                    }
                    SalesReturnItem.Unit = item.Unit;
                    SalesReturnItem.NumberType = item.NumberType;
                    SalesReturnItem.SellingPrice = item.SellingPrice;
                    SalesReturnItem.MRP = item.MRP;
                    SalesReturnItem.DiscPer = item.DiscPer;
                    SalesReturnItem.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                    SalesReturnItem.ItemTax = item.ItemTax;
                    SalesReturnItem.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                    SalesReturnItem.Amount = Convert.ToDouble(frmcol[amount]);
                    SalesReturnItem.Status = "Active";
                    SalesReturnItem.ModifiedOn = DateTime.Now;
                    if (SalesReturnItem.Quantity != 0)
                    {
                        _SalesReturnItemService.Create(SalesReturnItem);
                    }
                    salescount++;
                }

                var retailbill = _RetailBillService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                if (retailbill.SalesReturnNo != null)
                {
                    retailbill.SalesReturnNo = retailbill.SalesReturnNo + "," + model.SalesReturnNo;
                }
                else
                {
                    retailbill.SalesReturnNo = model.SalesReturnNo;
                }
                retailbill.SalesReturn = "Yes";
                _RetailBillService.Update(retailbill);

                List<SalesReturnItem> ReturnItemList = new List<SalesReturnItem>();
                int alreadypresentitems = Convert.ToInt32(frmcol["ItemList"]);
                int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);
                for (int i = alreadypresentitems + 1; i <= rowcount; i++)
                {
                    string itemCode = "itemCode" + i;
                    string subcat = "SubCat" + i;
                    string ItemType = "ItemType" + i;
                    string itemName = "itemName" + i;
                    string barcode = "Barcode" + i;
                    string description = "description" + i;
                    string narration = "narration" + i;
                    string quantity = "quantity" + i;
                    string rate = "rate" + i;
                    string mrp = "MRP" + i;
                    string sellingprice = "SellingPrice" + i;
                    string materialvalue = "materialvalue" + i;
                    string colorvalue = "colorvalue" + i;
                    string design = "design" + i;
                    string designName = "designName" + i;
                    string brand = "Brand" + i;
                    string size = "Size" + i;
                    string discountpercent = "discountpercent" + i;
                    string discountrps = "discountrps" + i;
                    string unitvalue = "unitvalue" + i;
                    string numbertype = "numberType" + i;
                    string amountvalue = "amountvalue" + i;
                    string itemtax = "ItemTaxValue" + i;
                    string itemtaxamt = "ItemTaxAmt" + i;
                    string proportionatetax = "proportionatetaxvalue" + i;
                    string proportionatetaxamount = "proportionatetaxamount" + i;

                    if (frmcol[quantity] != "" && frmcol[itemCode] != null)
                    {
                        model.SalesReturnItemDetails = new SalesReturnItem();
                        model.SalesReturnItemDetails.SalesReturnNo = model.SalesReturnNo;
                        model.SalesReturnItemDetails.BillNo = frmcol["BillNo"];
                        model.SalesReturnItemDetails.ItemName = frmcol[itemName];
                        model.SalesReturnItemDetails.ItemCode = frmcol[itemCode];
                        if (frmcol[ItemType] == "Inventory")
                        {
                            model.SalesReturnItemDetails.Barcode = frmcol[barcode];
                        }
                        model.SalesReturnItemDetails.ItemType = frmcol[ItemType];
                        model.SalesReturnItemDetails.Description = frmcol[description];
                        model.SalesReturnItemDetails.Narration = frmcol[narration];
                        model.SalesReturnItemDetails.Material = frmcol[materialvalue];
                        model.SalesReturnItemDetails.Color = frmcol[colorvalue];
                        model.SalesReturnItemDetails.DesignCode = frmcol[design];
                        model.SalesReturnItemDetails.DesignName = frmcol[designName];
                        model.SalesReturnItemDetails.Brand = frmcol[brand];
                        model.SalesReturnItemDetails.Size = frmcol[size];
                        model.SalesReturnItemDetails.Unit = frmcol[unitvalue];
                        model.SalesReturnItemDetails.NumberType = frmcol[numbertype];
                        model.SalesReturnItemDetails.DiscPer = Convert.ToDouble(frmcol[discountpercent]);
                        model.SalesReturnItemDetails.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                        if (frmcol[rate] != "")
                        {
                            model.SalesReturnItemDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                        }
                        else
                        {
                            model.SalesReturnItemDetails.SellingPrice = 0;
                        }
                        if (frmcol[mrp] != "")
                        {
                            model.SalesReturnItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            model.SalesReturnItemDetails.MRP = 0;
                        }

                        model.SalesReturnItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesReturnItemDetails.Balance = Convert.ToDouble(frmcol[quantity]); ;
                        model.SalesReturnItemDetails.Amount = Convert.ToDouble(frmcol[amountvalue]);
                        model.SalesReturnItemDetails.ItemTax = frmcol[itemtax];
                        model.SalesReturnItemDetails.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                        model.SalesReturnItemDetails.Status = "Active";
                        model.SalesReturnItemDetails.ModifiedOn = DateTime.Now;

                        ReturnItemList.Add(model.SalesReturnItemDetails);

                        //SUBTRACT REFUND ITEM QUANTITY TO SHOP STOCK QUANTITY
                        string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        if (frmcol[ItemType] == "Inventory")
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.SalesReturnItemDetails.ItemCode, shopcode);
                            shopdetails.Quantity = shopdetails.Quantity - model.SalesReturnItemDetails.Quantity;
                            _ShopStockService.Update(shopdetails);

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.SalesReturnItemDetails.ItemCode, shopcode);
                            stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity - model.SalesReturnItemDetails.Quantity;
                            _StockItemDistributionService.Update(stockitemdetails);

                            var entrystockitems = _EntryStockItemService.getDetailsByItemCode(model.SalesReturnItemDetails.ItemCode);
                            if (entrystockitems != null)
                            {
                                entrystockitems.TotalQuantity = entrystockitems.TotalQuantity - model.SalesReturnItemDetails.Quantity;
                                _EntryStockItemService.Update(entrystockitems);
                            }
                            else
                            {
                                var openingstockitems = _OpeningStockService.GetDetailsByItemCode(model.SalesReturnItemDetails.ItemCode);
                                openingstockitems.TotalQuantity = openingstockitems.TotalQuantity - model.SalesReturnItemDetails.Quantity;
                                _OpeningStockService.UpdateStock(openingstockitems);
                            }
                        }
                    }
                }

                if (rowcount > alreadypresentitems)
                {
                    if (ReturnItemList.Count() != 0)
                    {
                        var oldretailbill = _RetailBillService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                        var LastRetailBill = _RetailBillService.GetLastRetailBillByFinYr(FinYr, loginshopcode);
                        string RetailBillCode = string.Empty;
                        int RetailBillLength = 0;
                        int RetailBillNo = 0;
                        if (LastRetailBill == null)
                        {
                            RetailBillLength = 1;
                            RetailBillNo = 1;
                        }
                        else
                        {
                            int RetailBillIndex = LastRetailBill.RetailBillNo.LastIndexOf('R');
                            RetailBillCode = LastRetailBill.RetailBillNo.Substring(RetailBillIndex + 2, 6);
                            RetailBillLength = (Convert.ToInt32(RetailBillCode) + 1).ToString().Length;
                            RetailBillNo = Convert.ToInt32(RetailBillCode) + 1;
                        }
                        RetailBillCode = _UtilityService.getName(ShortCode + "/RI", RetailBillLength, RetailBillNo);
                        RetailBillCode = RetailBillCode + FinYr;
                        TempData["PreviousRetailno"] = RetailBillCode;

                        var newretailbill = new RetailBill();
                        newretailbill.RetailBillNo = RetailBillCode;
                        newretailbill.DeliveryChallanNo = oldretailbill.DeliveryChallanNo;
                        newretailbill.Date = DateTime.Now;
                        newretailbill.ClientName = oldretailbill.ClientName;
                        newretailbill.ClientContact = oldretailbill.ClientContact;
                        newretailbill.ClientAddress = oldretailbill.ClientAddress;
                        newretailbill.ClientEmail = oldretailbill.ClientEmail;
                        newretailbill.SalesPersonName = oldretailbill.SalesPersonName;
                        newretailbill.SalesPersonAddress = oldretailbill.SalesPersonAddress;
                        newretailbill.SalesPersonEmail = oldretailbill.SalesPersonEmail;
                        newretailbill.SalesPersonContact = oldretailbill.SalesPersonContact;
                        newretailbill.SalesPersonDesignation = oldretailbill.SalesPersonDesignation;
                        newretailbill.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        newretailbill.ShopName = oldretailbill.ShopName;
                        newretailbill.ShopAddress = oldretailbill.ShopAddress;
                        newretailbill.ShopEmail = oldretailbill.ShopEmail;
                        newretailbill.ShopContactPerson = oldretailbill.ShopContactPerson;
                        newretailbill.PreparedBy = oldretailbill.PreparedBy;
                        newretailbill.TotalAmount = Convert.ToDouble(frmcol["NewItemTotalAmount"]);
                        newretailbill.TotalDiscount = Convert.ToDouble(frmcol["NewItemTotalDiscount"]);
                        newretailbill.TotalTaxAmount = Convert.ToDouble(frmcol["NewItemTotalTaxAmount"]);
                        newretailbill.GrandTotal = Convert.ToDouble(frmcol["NewItemGrandAmount"]);
                        newretailbill.Payment = 0;
                        newretailbill.Refund = 0;
                        newretailbill.CashierStatus = "Active";
                        newretailbill.BillBalance = "None";
                        newretailbill.Balance = Convert.ToDouble(frmcol["NewItemGrandAmount"]);
                        newretailbill.AdjustedAmount = 0;
                        newretailbill.RefundToClient = 0;
                        newretailbill.Status = "Active";
                        newretailbill.ModifiedOn = DateTime.Now;
                        newretailbill.SalesReturn = "No";
                        newretailbill.TaxStatus = "WithTax";
                        _RetailBillService.Create(newretailbill);
                        foreach (var item in ReturnItemList)
                        {
                            RetailBillItem RetailBillItem = new RetailBillItem();
                            RetailBillItem.RetailBillNo = RetailBillCode;
                            RetailBillItem.ItemCode = item.ItemCode;
                            RetailBillItem.ItemName = item.ItemName;
                            RetailBillItem.ItemType = item.ItemType;
                            RetailBillItem.Description = item.Description;
                            RetailBillItem.Narration = item.Narration;
                            RetailBillItem.Barcode = item.Barcode;
                            RetailBillItem.DesignCode = item.DesignCode;
                            RetailBillItem.DesignName = item.DesignName;
                            RetailBillItem.Color = item.Color;
                            RetailBillItem.Material = item.Material;
                            RetailBillItem.Brand = item.Brand;
                            RetailBillItem.Size = item.Size;
                            RetailBillItem.Unit = item.Unit;
                            RetailBillItem.NumberType = item.NumberType;
                            RetailBillItem.Quantity = item.Quantity;
                            RetailBillItem.SellingPrice = item.SellingPrice;
                            RetailBillItem.MRP = item.MRP;
                            RetailBillItem.DiscountPercent = item.DiscPer;
                            RetailBillItem.DiscountRPS = item.DiscRs;
                            RetailBillItem.ItemTax = item.ItemTax;
                            RetailBillItem.ItemTaxAmt = item.ItemTaxAmt.ToString();
                            RetailBillItem.Amount = item.Amount;
                            RetailBillItem.Status = "Active";
                            RetailBillItem.ModifiedOn = DateTime.Now;
                            RetailBillItem.SalesReturn = "No";
                            _RetailBillItemService.Create(RetailBillItem);
                        }

                        //Store total tax and total amount of tax of New Retail Bill
                        int taxcount = Convert.ToInt32(frmcol["NewItemTaxCount"]);
                        model.InventoryTaxDetails = new InventoryTax();
                        for (int i = 1; i <= taxcount; i++)
                        {
                            string taxnumber = "NewItemTaxNumber" + i;
                            string taxamount = "NewAddedTaxAmounthdn" + i;
                            string amountontax = "NewAddedAmounthdn" + i;
                            if (Convert.ToDouble(frmcol[taxamount]) != 0)
                            {
                                model.InventoryTaxDetails.Code = RetailBillCode;
                                model.InventoryTaxDetails.Amount = frmcol[amountontax];
                                model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                                model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                                _InventoryTaxService.Create(model.InventoryTaxDetails);
                            }
                        }
                    }
                }
                //Store total tax and total amount of tax of Sales Return
                int itemtaxcount = Convert.ToInt32(frmcol["ReturnItemTaxCount"]);
                model.InventoryTaxDetails = new InventoryTax();
                for (int i = 1; i <= itemtaxcount; i++)
                {
                    string taxnumber = "ReturnItemTaxNumber" + i;
                    string taxamount = "ReturnAddedTaxAmounthdn" + i;
                    string amountontax = "ReturnAddedAmounthdn" + i;
                    if (Convert.ToDouble(frmcol[taxamount]) != 0)
                    {
                        model.InventoryTaxDetails.Code = model.SalesReturnNo;
                        model.InventoryTaxDetails.Amount = frmcol[amountontax];
                        model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                        model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                        _InventoryTaxService.Create(model.InventoryTaxDetails);
                    }
                }
            }
            //Save Sales Bill Item Details In Sales Return Item
            else
            {
                model.SalesBillItemList = TempData["BillItemList"] as IEnumerable<SalesBillItem>;
                int count = 1;
                foreach (var item in model.SalesBillItemList)
                {
                    string checkbox = "CheckBox" + count;

                    string quantity = "quantity" + count;
                    string amount = "amountvalue" + count;
                    string prevqtyvalue = "prevquantityvalue" + count;
                    string itemtaxamt = "ItemTaxAmt" + count;
                    string discountrps = "discountrps" + count;
                    string proportionatetax = "proportionatetaxvalue" + count;
                    string proportionatetaxamount = "proportionatetaxamount" + count;

                    model.SalesReturnItemDetails = new SalesReturnItem();
                    model.SalesReturnItemDetails.SalesReturnNo = model.SalesReturnNo;
                    model.SalesReturnItemDetails.BillNo = frmcol["BillNo"];
                    model.SalesReturnItemDetails.Barcode = item.Barcode;
                    model.SalesReturnItemDetails.ItemCode = item.ItemCode;
                    model.SalesReturnItemDetails.ItemName = item.ItemName;
                    model.SalesReturnItemDetails.ItemType = item.ItemType;
                    model.SalesReturnItemDetails.Narration = item.Narration;
                    model.SalesReturnItemDetails.Description = item.Description;
                    model.SalesReturnItemDetails.Material = item.Material;
                    model.SalesReturnItemDetails.Color = item.Color;
                    model.SalesReturnItemDetails.DesignCode = item.DesignCode;
                    model.SalesReturnItemDetails.DesignName = item.DesignName;
                    model.SalesReturnItemDetails.Brand = item.Brand;
                    model.SalesReturnItemDetails.Size = item.Size;
                    model.SalesReturnItemDetails.Unit = item.Unit;
                    model.SalesReturnItemDetails.NumberType = item.NumberType;
                    model.SalesReturnItemDetails.SellingPrice = item.SellingPrice;
                    model.SalesReturnItemDetails.MRP = item.MRP;
                    model.SalesReturnItemDetails.DiscPer = item.DiscountPercent;
                    model.SalesReturnItemDetails.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                    if (frmcol[checkbox] == "Yes")
                    {
                        model.SalesReturnItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                    }
                    else
                    {
                        model.SalesReturnItemDetails.Quantity = Convert.ToDouble(frmcol[prevqtyvalue]);
                    }
                    model.SalesReturnItemDetails.Status = "Active";
                    model.SalesReturnItemDetails.ItemTax = item.ItemTax;
                    model.SalesReturnItemDetails.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                    model.SalesReturnItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                    model.SalesReturnItemDetails.ModifiedOn = DateTime.Now;
                    model.SalesReturnItemDetails.PropTax = frmcol[proportionatetax];
                    model.SalesReturnItemDetails.PropTaxAmt = Convert.ToDouble(frmcol[proportionatetaxamount]);
                    if (model.SalesReturnItemDetails.Quantity != 0)
                    {
                        _SalesReturnItemService.Create(model.SalesReturnItemDetails);
                    }

                    SalesBillCreditNoteItem SalesBillCreditNoteItem = new SalesBillCreditNoteItem();
                    SalesBillCreditNoteItem.BillNo = frmcol["BillNo"];
                    SalesBillCreditNoteItem.CreditNoteNo = CreditNoteCode;
                    SalesBillCreditNoteItem.Barcode = item.Barcode;
                    SalesBillCreditNoteItem.ItemCode = item.ItemCode;
                    SalesBillCreditNoteItem.ItemName = item.ItemName;
                    SalesBillCreditNoteItem.ItemType = item.ItemType;
                    SalesBillCreditNoteItem.Narration = item.Narration;
                    SalesBillCreditNoteItem.Description = item.Description;
                    SalesBillCreditNoteItem.Color = item.Color;
                    SalesBillCreditNoteItem.Material = item.Material;
                    SalesBillCreditNoteItem.DesignCode = item.DesignCode;
                    SalesBillCreditNoteItem.DesignName = item.DesignName;
                    SalesBillCreditNoteItem.Brand = item.Brand;
                    SalesBillCreditNoteItem.Size = item.Size;
                    SalesBillCreditNoteItem.Unit = item.Unit;
                    SalesBillCreditNoteItem.NumberType = item.NumberType;
                    SalesBillCreditNoteItem.SellingPrice = item.SellingPrice;
                    SalesBillCreditNoteItem.MRP = item.MRP;
                    SalesBillCreditNoteItem.DiscPer = item.DiscountPercent;

                    if (frmcol[checkbox] == "Yes")
                    {
                        SalesBillCreditNoteItem.Quantity = Convert.ToDouble(frmcol[quantity]);
                        SalesBillCreditNoteItem.Balance = Convert.ToDouble(frmcol[prevqtyvalue]) - Convert.ToDouble(frmcol[quantity]);
                        SalesBillCreditNoteItem.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                        SalesBillCreditNoteItem.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                        SalesBillCreditNoteItem.Amount = Convert.ToDouble(frmcol[amount]);
                        SalesBillCreditNoteItem.Status = "Return";
                    }
                    else
                    {
                        SalesBillCreditNoteItem.Quantity = 0;
                        SalesBillCreditNoteItem.DiscRs = 0;
                        SalesBillCreditNoteItem.Balance = Convert.ToDouble(frmcol[prevqtyvalue]);
                        SalesBillCreditNoteItem.ItemTaxAmt = 0;
                        SalesBillCreditNoteItem.Amount = 0;
                        SalesBillCreditNoteItem.Status = "Active";
                    }
                    SalesBillCreditNoteItem.ItemTax = item.ItemTax;
                    SalesBillCreditNoteItem.PropTax = frmcol[proportionatetax];
                    SalesBillCreditNoteItem.PropTaxAmt = Convert.ToDouble(frmcol[proportionatetaxamount]);

                    SalesBillCreditNoteItem.ModifiedOn = DateTime.Now;
                    _SalesBillCreditNoteItemService.Create(SalesBillCreditNoteItem);

                    if (frmcol[checkbox] == "Yes")
                    {
                        var itemdata = _SalesBillItemService.GetItemDetailsByinvoiceNoAndItemCode(frmcol["BillNo"], model.SalesReturnItemDetails.ItemCode);
                        itemdata.SalesReturn = "Yes";
                        _SalesBillItemService.Update(itemdata);

                        //SUBTRACT REFUND ITEM QUANTITY TO SHOP STOCK QUANTITY
                        string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        if (item.ItemType == "Inventory")
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.SalesReturnItemDetails.ItemCode, shopcode);
                            if (shopdetails != null)
                            {
                                shopdetails.Quantity = shopdetails.Quantity + model.SalesReturnItemDetails.Quantity;
                                shopdetails.Status = "Active";
                                _ShopStockService.Update(shopdetails);
                            }
                            else
                            {
                                ShopStock ShopStock = new ShopStock();
                                ShopStock.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                                ShopStock.ShopName = Session["SHOPGODOWNNAME"].ToString();
                                ShopStock.Barcode = item.Barcode;
                                ShopStock.ItemCode = item.ItemCode;
                                ShopStock.ItemName = item.ItemName;
                                ShopStock.Description = item.Description;
                                ShopStock.Color = item.Color;
                                ShopStock.Material = item.Material;
                                ShopStock.DesignCode = item.DesignCode;
                                ShopStock.DesignName = item.DesignName;
                                ShopStock.Brand = item.Brand;
                                ShopStock.Size = item.Size;
                                ShopStock.Quantity = Convert.ToDouble(frmcol[quantity]);
                                ShopStock.Unit = item.Unit;
                                ShopStock.NumberType = item.NumberType;
                                ShopStock.SellingPrice = item.SellingPrice;
                                ShopStock.MRP = item.MRP;
                                ShopStock.Status = "Active";
                                ShopStock.ModifiedOn = DateTime.Now;
                                _ShopStockService.Create(ShopStock);
                            }

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.SalesReturnItemDetails.ItemCode, shopcode);
                            if (stockitemdetails != null)
                            {
                                stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity + model.SalesReturnItemDetails.Quantity;
                                stockitemdetails.Status = "Active";
                                _StockItemDistributionService.Update(stockitemdetails);
                            }
                            else
                            {
                                var ItemDetails = _ItemService.GetDescriptionByItemCode(item.ItemCode);
                                StockItemDistribution StockItemDistribution = new StockItemDistribution();
                                StockItemDistribution.Code = Session["LOGINSHOPGODOWNCODE"].ToString();
                                StockItemDistribution.GodownShopName = Session["SHOPGODOWNNAME"].ToString();
                                StockItemDistribution.Category = ItemDetails.itemCategory;
                                StockItemDistribution.SubCategory = ItemDetails.itemSubCategory;
                                StockItemDistribution.Barcode = item.Barcode;
                                StockItemDistribution.ItemCode = item.ItemCode;
                                StockItemDistribution.ItemName = item.ItemName;
                                StockItemDistribution.Description = item.Description;
                                StockItemDistribution.Color = item.Color;
                                StockItemDistribution.Material = item.Material;
                                StockItemDistribution.DesignCode = item.DesignCode;
                                StockItemDistribution.DesignName = item.DesignName;
                                StockItemDistribution.Brand = item.Brand;
                                StockItemDistribution.Size = item.Size;
                                StockItemDistribution.Unit = item.Unit;
                                StockItemDistribution.NumberType = item.NumberType;
                                StockItemDistribution.ItemQuantity = Convert.ToDouble(frmcol[quantity]);
                                StockItemDistribution.SellingPrice = item.SellingPrice;
                                StockItemDistribution.MRP = item.MRP;
                                StockItemDistribution.Status = "Active";
                                StockItemDistribution.ModifiedOn = DateTime.Now;
                                _StockItemDistributionService.Create(StockItemDistribution);
                            }

                            var entrystockitems = _EntryStockItemService.getDetailsByItemCode(model.SalesReturnItemDetails.ItemCode);
                            if (entrystockitems != null)
                            {
                                entrystockitems.TotalQuantity = entrystockitems.TotalQuantity + model.SalesReturnItemDetails.Quantity;
                                _EntryStockItemService.Update(entrystockitems);
                            }
                            else
                            {
                                var openingstockitems = _OpeningStockService.GetDetailsByItemCode(model.SalesReturnItemDetails.ItemCode);
                                openingstockitems.TotalQuantity = openingstockitems.TotalQuantity + model.SalesReturnItemDetails.Quantity;
                                _OpeningStockService.UpdateStock(openingstockitems);
                            }
                        }
                    }
                    count++;
                }

                model.SalesBillCreditNoteItemList = TempData["CreditNoteItemList"] as IEnumerable<SalesBillCreditNoteItem>;
                int salescount = model.SalesBillItemList.Count() + 1;
                foreach (var item in model.SalesBillCreditNoteItemList)
                {
                    string checkbox = "CheckBox" + salescount;
                    string quantity = "quantity" + salescount;
                    string amount = "amountvalue" + salescount;
                    string prevqtyvalue = "prevquantityvalue" + salescount;
                    string itemtaxamt = "ItemTaxAmt" + salescount;
                    string discountrps = "discountrps" + salescount;
                    string proportionatetax = "proportionatetaxvalue" + salescount;
                    string proportionatetaxamount = "proportionatetaxamount" + salescount;

                    if (frmcol[checkbox] == "Yes")
                    {
                        item.CreditNoteNo = CreditNoteCode;
                        item.Quantity = Convert.ToDouble(frmcol[quantity]);
                        item.Balance = Convert.ToDouble(frmcol[prevqtyvalue]) - Convert.ToDouble(frmcol[quantity]);
                        item.Amount = Convert.ToDouble(frmcol[amount]);
                        item.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                        item.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                        item.PropTax = frmcol[proportionatetax];
                        item.PropTaxAmt = Convert.ToDouble(frmcol[proportionatetaxamount]);
                        item.Status = "Return";
                        item.ModifiedOn = DateTime.Now;
                        _SalesBillCreditNoteItemService.Create(item);

                        //var itemdata = _SalesBillItemService.GetItemDetailsByinvoiceNoAndItemCode(frmcol["BillNo"], model.SalesReturnItemDetails.ItemCode);
                        //itemdata.SalesReturn = "Yes";
                        //_SalesBillItemService.Update(itemdata);

                        //ADD REFUND ITEM QUANTITY TO SHOP STOCK QUANTITY
                        string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        if (item.ItemType == "Inventory")
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, shopcode);
                            shopdetails.Quantity = shopdetails.Quantity + item.Quantity;
                            _ShopStockService.Update(shopdetails);

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, shopcode);
                            stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity + item.Quantity;
                            _StockItemDistributionService.Update(stockitemdetails);

                            var entrystockitems = _EntryStockItemService.getDetailsByItemCode(item.ItemCode);
                            if (entrystockitems != null)
                            {
                                entrystockitems.TotalQuantity = entrystockitems.TotalQuantity + item.Quantity;
                                _EntryStockItemService.Update(entrystockitems);
                            }
                            else
                            {
                                var openingstockitems = _OpeningStockService.GetDetailsByItemCode(item.ItemCode);
                                openingstockitems.TotalQuantity = openingstockitems.TotalQuantity + item.Quantity;
                                _OpeningStockService.UpdateStock(openingstockitems);
                            }
                        }
                    }
                    else
                    {
                        item.CreditNoteNo = CreditNoteCode;
                        item.Quantity = 0;
                        item.ItemTaxAmt = 0;
                        item.DiscRs = 0;
                        item.Amount = 0;
                        _SalesBillCreditNoteItemService.Create(item);
                    }

                    SalesReturnItem SalesReturnItem = new SalesReturnItem();
                    SalesReturnItem.SalesReturnNo = model.SalesReturnNo;
                    SalesReturnItem.BillNo = frmcol["BillNo"];
                    SalesReturnItem.Barcode = item.Barcode;
                    SalesReturnItem.ItemCode = item.ItemCode;
                    SalesReturnItem.ItemName = item.ItemName;
                    SalesReturnItem.ItemType = item.ItemType;
                    SalesReturnItem.Narration = item.Narration;
                    SalesReturnItem.Description = item.Description;
                    SalesReturnItem.Color = item.Color;
                    SalesReturnItem.Material = item.Material;
                    SalesReturnItem.Brand = item.Brand;
                    SalesReturnItem.Size = item.Size;
                    SalesReturnItem.DesignCode = item.DesignCode;
                    SalesReturnItem.DesignName = item.DesignName;
                    if (frmcol[checkbox] == "Yes")
                    {
                        SalesReturnItem.Quantity = Convert.ToDouble(frmcol[prevqtyvalue]) - Convert.ToDouble(frmcol[quantity]);
                    }
                    else
                    {
                        SalesReturnItem.Quantity = Convert.ToDouble(frmcol[prevqtyvalue]);
                    }
                    SalesReturnItem.Unit = item.Unit;
                    SalesReturnItem.NumberType = item.NumberType;
                    SalesReturnItem.SellingPrice = item.SellingPrice;
                    SalesReturnItem.MRP = item.MRP;
                    SalesReturnItem.DiscPer = item.DiscPer;
                    SalesReturnItem.DiscRs = Convert.ToDouble(frmcol[discountrps]);
                    SalesReturnItem.ItemTax = item.ItemTax;
                    SalesReturnItem.ItemTaxAmt = Convert.ToDouble(frmcol[itemtaxamt]);
                    SalesReturnItem.Amount = Convert.ToDouble(frmcol[amount]);
                    SalesReturnItem.PropTax = frmcol[proportionatetax];
                    SalesReturnItem.PropTaxAmt = Convert.ToDouble(frmcol[proportionatetaxamount]);
                    SalesReturnItem.Status = "Active";
                    SalesReturnItem.ModifiedOn = DateTime.Now;
                    if (SalesReturnItem.Quantity != 0)
                    {
                        _SalesReturnItemService.Create(SalesReturnItem);
                    }
                    salescount++;
                }

                var salesbill = _SalesBillService.GetDetailsBySalesBillNo(frmcol["BillNo"]);
                if (salesbill.SalesReturnNo != null)
                {
                    salesbill.SalesReturnNo = salesbill.SalesReturnNo + "," + model.SalesReturnNo;
                }
                else
                {
                    salesbill.SalesReturnNo = model.SalesReturnNo;
                }
                salesbill.SalesReturn = "Yes";
                _SalesBillService.Update(salesbill);

                //Store total tax and total amount of tax of PO
                int itemtaxcount = Convert.ToInt32(frmcol["ReturnItemTaxCount"]);
                model.InventoryTaxDetails = new InventoryTax();
                for (int i = 1; i <= itemtaxcount; i++)
                {
                    string taxnumber = "ReturnItemTaxNumber" + i;
                    string taxamount = "ReturnAddedTaxAmounthdn" + i;
                    string amountontax = "ReturnAddedAmounthdn" + i;
                    if (Convert.ToDouble(frmcol[taxamount]) != 0)
                    {
                        model.InventoryTaxDetails.Code = model.SalesReturnNo;
                        model.InventoryTaxDetails.Amount = frmcol[amountontax];
                        model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                        model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                        _InventoryTaxService.Create(model.InventoryTaxDetails);
                    }
                }
            }
            var SalesReturn = _SalesReturnService.GetSalesByReturnNo(model.SalesReturnNo);
            string salesreturncode = Encode(SalesReturn.Id.ToString());
            return RedirectToAction("SalesReturnDetails/" + salesreturncode);
        }

        [HttpGet]
        public ActionResult SalesReturnDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.SalesReturnDetails = _SalesReturnService.GetById(Id);
            model.RetailBillCreditNoteItemList = _RetailBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.SalesBillCreditNoteItemList = _SalesBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesReturnDetails.SalesReturnNo);
            string previoussalesreturn = TempData["PreviousSalesReturnNo"].ToString();
            if (previoussalesreturn != model.SalesReturnDetails.SalesReturnNo)
            {
                ViewData["SalesReturnChanged"] = previoussalesreturn + " is replaced with " + model.SalesReturnDetails.SalesReturnNo + " because " + previoussalesreturn + " is acquired by another person";
            }
            TempData["PreviousSalesReturnNo"] = previoussalesreturn;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintSalesReturn(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.SalesReturnDetails = _SalesReturnService.GetById(Id);
            model.RetailBillCreditNoteItemList = _RetailBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.SalesBillCreditNoteItemList = _SalesBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintCreditNoteWithSellingPrice(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.SalesReturnDetails = _SalesReturnService.GetById(Id);
            model.RetailBillCreditNoteItemList = _RetailBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.SalesBillCreditNoteItemList = _SalesBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintCreditNoteWithMRP(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.SalesReturnDetails = _SalesReturnService.GetById(Id);
            model.RetailBillCreditNoteItemList = _RetailBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.SalesBillCreditNoteItemList = _SalesBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult SalesReturnPrint()
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
        public JsonResult GetSalesReturnNos(string term)
        {
            var saleslist = _SalesReturnService.GetAllSalesReturnNo(term);
            List<string> names = new List<string>();
            foreach (var data in saleslist)
            {
                names.Add(data.SalesReturnNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSalesReturnDetails(string srno)
        {
            MainApplication model = new MainApplication();
            model.SalesReturnDetails = new SalesReturn();
            model.SalesReturnDetails = _SalesReturnService.GetSalesByReturnNo(srno);
            model.RetailBillCreditNoteItemList = _RetailBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.SalesBillCreditNoteItemList = _SalesBillCreditNoteItemService.GetItemListByCreditNote(model.SalesReturnDetails.CreditNoteNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesReturnDetails.SalesReturnNo);
            return View(model);
        }

    }
}
