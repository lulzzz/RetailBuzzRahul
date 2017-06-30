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
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Drawing;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class MisReportsController : Controller
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
        private readonly IOpeningStockService _openingstockservice;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IEntryStockService _entrystockservice;
        private readonly IEntryStockItemService _entrystockitemservice;
        private readonly IPurchaseOrderDetailService _purchaseorderdetailservice;
        private readonly IPurchaseItemDetailService _purchaseitemdetailservice;
        private readonly IItemService _ItemService;
        private readonly ISuppliersMasterService _supplierservice;
        private readonly ISupplierBankDetailService _SupplierBankDetailService;
        private readonly IGodownService _godownservice;
        private readonly IStockItemDistributionService _stockitemdistributionservice;
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplier;
        private readonly IInwardFromGodownService _InwardFromGodownService;
        private readonly IInwardItemFromGodownService _InwardItemFromGodownService;
        private readonly IInwardFromStkDistService _InwardFromStkDistService;
        private readonly IInwardItemFromStockDistributionService _InwardItemFromStkDistService;
        private readonly IOutwardToShopService _outwardfromgodowntoshopservice;
        private readonly IOutwardItemToShopService _outwarditemfromgodowntoshopservice;
        private readonly IOutwardInterGodownService _OutwardInterGodownService;
        private readonly IOutwardInterShopService _OutwardInterShopService;
        private readonly IOutwardShopToGodownService _OutwardShopToGodownService;
        private readonly IOutwardItemInterGodownService _OutwardItemInterGodownService;
        private readonly IOutwardToClientService _OutwardToClientService;
        private readonly IOutwardItemToClientService _OutwardItemToClientService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IClientBankDetailService _ClientBankService;
        private readonly IQuotationService _QuotationService;
        private readonly IQuotationItemService _QuotationItemService;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly IDeliveryChallanService _DeliveryChallanSevice;
        private readonly ICashierReceivableService _CashierReceivableService;
        private readonly ISalesBillService _SalesBillService;
        private readonly ISalesBillItemService _SalesBillItemService;
        private readonly IRetailBillService _RetailInvoiceMasterService;
        private readonly IRetailBillItemService _RetailBillItemService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IShopStockService _ShopStockService;
        private readonly IShopService _ShopSevice;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IInwardFromShopToGodownService _InwardFromShopToGodownService;
        private readonly ICommissionService _CommissionService;
        private readonly ICommissionProductService _CommissionProductService;
        private readonly IUnitService _UnitService;
        private readonly IInwardInterGodownService _InwardInterGodownService;
        private readonly IInwardInterShopService _InwardInterShopService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly IDepartmentService _DepartmentService;
        private readonly ICashierSalesOrderService _CashierSalesOrderService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierSalesBillService _CashierSalesBillService;
        private readonly ICashierTemporaryCashMemoService _CashierTemporaryCashMemoService;
        private readonly ICashierRefundOrderService _CashierRefundOrderService;
        private readonly ICashHandoverService _CashHandoverService;
        private readonly ICardChequeHandoverService _CardChequeHandoverService;
        private readonly IIncomeExpenseVoucherService _IncomeExchangeVoucherService;
        private readonly ITemporaryCashMemoService _TemporaryCashMemoService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IRetailBillCreditNoteService _RetailBillCreditNoteService;
        private readonly ISalesBillCreditNoteService _SalesBillCreditNoteService;
        private readonly IDebitNoteService _DebitNoteService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly ITemporaryCashMemoItemService _TemporaryCashMemoItemService;
        private readonly IRetailBillCreditNoteItemService _RetailBillCreditNoteItemService;
        private readonly ISalesBillCreditNoteItemService _SalesBillCreditNoteItemService;
        private readonly ISalesReturnService _SalesReturnService;

        public MisReportsController(IOpeningStockService openingstockservice, IItemCategoryService itemcategoryservice, IEntryStockService entrystockservice, IEntryStockItemService entrystockitemservice, IPurchaseOrderDetailService purchaseorderdetailservice, IPurchaseItemDetailService purchaseitemdetailservice, ISuppliersMasterService suppliermasterservice, IGodownService godownservice, IStockItemDistributionService stockitemdistributionservice, IInwardFromSupplierService performainvoiceservice, IInwardItemFromSupplierService performainvoiceitemservice,
                                    IInwardFromGodownService InwardFromGodownService, IInwardItemFromGodownService InwardItemFromGodownService, IInwardFromStkDistService InwardFromStkDistService, IInwardItemFromStockDistributionService InwardItemFromStkDistService, IOutwardToShopService outwardfromgodowntoshopservice, IOutwardItemToShopService outwarditemfromgodowntoshopservice, IOutwardInterGodownService OutwardInterGodownService, IOutwardItemInterGodownService OutwardItemInterGodownService, IOutwardToClientService OutwardToClientService,
                                    IOutwardItemToClientService OutwardItemToClientService, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IClientMasterService ClientMasterService, IQuotationService QuotationService, IQuotationItemService QuotationItemService, ICashierReceivableService CashierReceivableService, ISalesOrderService SalesOrderService, IDeliveryChallanService DeliveryChallanSevice, ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService, IRetailBillService RetailInvoiceMasterService, IGodownStockService GodownStockService,
                                    IShopStockService ShopStockService, IShopService ShopSevice, IEmployeeMasterService EmployeeMasterService, IRetailBillItemService RetailBilltemService, IOutwardShopToGodownService OutwardShopToGodownService, IInwardFromShopToGodownService InwardFromShopToGodownService, IUnitService UnitService, ICommissionService CommissionService, ICommissionProductService CommissionProductService, IInwardInterGodownService InwardInterGodowService, IInwardInterShopService InwardInterShopService, IOutwardInterShopService OutwardInterShopService,
                                    IClientBankDetailService ClientBankService, ISupplierBankDetailService SupplierBankDetailService, IItemTaxService ItemTaxService, IDepartmentService DepartmentService, IItemService ItemService, ICashierSalesOrderService CashierSalesOrderService, ICashierRetailBillService CashierRetailBillService, ICashierSalesBillService CashierSalesBillService, ICashierTemporaryCashMemoService CashierTemporaryCashMemoService, ICashierRefundOrderService CashierRefundOrderService, ICashHandoverService CashHandoverService,
                                    ICardChequeHandoverService CardChequeHandoverService, IIncomeExpenseVoucherService IncomeExchangeVoucherService, ITemporaryCashMemoService TemporaryCashMemoService, IItemSubCategoryService ItemSubCategoryService, IRetailBillCreditNoteService RetailBillCreditNoteService, ISalesBillCreditNoteService SalesBillCreditNoteService, IDebitNoteService DebitNoteService, IDeliveryChallanItemService DeliveryChallanItemService, ISalesOrderItemService SalesOrderItemService, ITemporaryCashMemoItemService TemporaryCashMemoItemService,
                                    IRetailBillCreditNoteItemService RetailBillCreditNoteItemService, ISalesBillCreditNoteItemService SalesBillCreditNoteItemService, ISalesReturnService SalesReturnService)
                                    
        {
            this._InwardFromShopToGodownService = InwardFromShopToGodownService;
            this._DepartmentService = DepartmentService;
            this._ItemService = ItemService;
            this._ItemTaxService = ItemTaxService;
            this._OutwardShopToGodownService = OutwardShopToGodownService;
            this._ClientBankService = ClientBankService;
            this._RetailBillItemService = RetailBilltemService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._itemcategoryservice = itemcategoryservice;
            this._openingstockservice = openingstockservice;
            this._entrystockservice = entrystockservice;
            this._entrystockitemservice = entrystockitemservice;
            this._purchaseorderdetailservice = purchaseorderdetailservice;
            this._purchaseitemdetailservice = purchaseitemdetailservice;
            this._supplierservice = suppliermasterservice;
            this._SupplierBankDetailService = SupplierBankDetailService;
            this._godownservice = godownservice;
            this._stockitemdistributionservice = stockitemdistributionservice;
            this._InwardFromSupplierService = performainvoiceservice;
            this._InwardItemFromSupplier = performainvoiceitemservice;
            this._InwardFromGodownService = InwardFromGodownService;
            this._InwardItemFromGodownService = InwardItemFromGodownService;
            this._InwardFromStkDistService = InwardFromStkDistService;
            this._InwardItemFromStkDistService = InwardItemFromStkDistService;
            this._outwardfromgodowntoshopservice = outwardfromgodowntoshopservice;
            this._outwarditemfromgodowntoshopservice = outwarditemfromgodowntoshopservice;
            this._OutwardInterGodownService = OutwardInterGodownService;
            this._OutwardInterShopService = OutwardInterShopService;
            this._OutwardItemInterGodownService = OutwardItemInterGodownService;
            this._OutwardToClientService = OutwardToClientService;
            this._OutwardItemToClientService = OutwardItemToClientService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._ClientMasterService = ClientMasterService;
            this._QuotationService = QuotationService;
            this._QuotationItemService = QuotationItemService;
            this._CashierReceivableService = CashierReceivableService;
            this._SalesOrderService = SalesOrderService;
            this._DeliveryChallanSevice = DeliveryChallanSevice;
            this._SalesBillService = SalesBillService;
            this._SalesBillItemService = SalesBillItemService;
            this._RetailInvoiceMasterService = RetailInvoiceMasterService;
            this._GodownStockService = GodownStockService;
            this._ShopStockService = ShopStockService;
            this._ShopSevice = ShopSevice;
            this._CommissionService = CommissionService;
            this._CommissionProductService = CommissionProductService;
            this._UnitService = UnitService;
            this._InwardInterGodownService = InwardInterGodowService;
            this._InwardInterShopService = InwardInterShopService;
            this._CashierSalesOrderService = CashierSalesOrderService;
            this._CashierSalesBillService = CashierSalesBillService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierTemporaryCashMemoService = CashierTemporaryCashMemoService;
            this._CashierRefundOrderService = CashierRefundOrderService;
            this._CashHandoverService = CashHandoverService;
            this._CardChequeHandoverService = CardChequeHandoverService;
            this._IncomeExchangeVoucherService = IncomeExchangeVoucherService;
            this._TemporaryCashMemoService = TemporaryCashMemoService;
            this._ItemSubCategoryService = ItemSubCategoryService;
            this._RetailBillCreditNoteService = RetailBillCreditNoteService;
            this._SalesBillCreditNoteService = SalesBillCreditNoteService;
            this._DebitNoteService = DebitNoteService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._SalesOrderItemService = SalesOrderItemService;
            this._TemporaryCashMemoItemService = TemporaryCashMemoItemService;
            this._RetailBillCreditNoteItemService = RetailBillCreditNoteItemService;
            this._SalesBillCreditNoteItemService = SalesBillCreditNoteItemService;
            this._SalesReturnService = SalesReturnService;
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

        // ************************ PURCHASE ORDER REPORT *******************************************************
        [HttpGet]
        public ActionResult PurchaseOrderReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);

        }

        //PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult PurchaseOrderReportDatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult GetPurchaseOrderDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.PurchaseOrderList = _purchaseorderdetailservice.getReportByDate(FDate, TDate);
            TempData["PurchaseOrderListDatewise"] = model.PurchaseOrderList;
            TempData["ConvertToPdf"] = "No";
            return View(model);
        }

        //PRINT PAGE OF PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult PrintPurchaseOrderDatewise()
        {
            MainApplication model = new MainApplication();
            model.PurchaseOrderList = TempData["PurchaseOrderListDatewise"] as IEnumerable<PurchaseOrderDetail>;
            TempData["PurchaseOrderListDatewise"] = model.PurchaseOrderList;
            return View(model);
        }

        //PURCHASE ORDER REPORT SUPPLIERWISE
        [HttpGet]
        public ActionResult PurchaseOrderReportSupplierwise()
        {
            MainApplication model = new MainApplication();
            model.SupplierList = _supplierservice.getAllSuppliers();
            return View(model);
        }


        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult GetPurchaseOrderDetailsBySupplier(string Supplier, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string SuppNames = Supplier;
            string[] SingleSupp = SuppNames.Split(',');
            List<PurchaseOrderDetail> purchaselist = new List<PurchaseOrderDetail>();

            for (int i = 0; i < SingleSupp.Count(); i++)
            {
                var list = _purchaseorderdetailservice.GetReportBySupplierNameAndDate(SingleSupp[i], FDate, TDate);
                foreach (var data in list)
                {
                    purchaselist.Add(data);
                }
            }
            model.PurchaseOrderList = purchaselist;
            TempData["PurchaseOrderListSupplerwise"] = model.PurchaseOrderList;
            TempData["ConvertToPdf"] = "No";
            return View(model);
        }

        //PRINT PAGE OF PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult PrintPurchaseOrderSupplierwise()
        {
            MainApplication model = new MainApplication();
            model.PurchaseOrderList = TempData["PurchaseOrderListSupplerwise"] as IEnumerable<PurchaseOrderDetail>;
            TempData["PurchaseOrderListSupplerwise"] = model.PurchaseOrderList;
            return View(model);
        }

        //PURCHASE ORDER REPORT ITEMWISE
        [HttpGet]
        public ActionResult PurchaseOrderReportItemwise()
        {
            return View();
        }

        //GET ITEM LIST
        [HttpGet]
        public JsonResult GetAllItems()
        {
            var ItemList = _ItemService.getAllItems();
            var items = ItemList.Select(m => new SelectListItem
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN PURCHASE ORDER REPORT ITEMWISE
        [HttpGet]
        public ActionResult GetPurchaseOrderDetailsByItem(string Item, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);
            string itemname = Item;
            string[] singleitem = itemname.Split(',');
            
            List<PurchaseOrderDetail> POList = new List<PurchaseOrderDetail>();
            for (int i = 0; i < singleitem.Count(); i++)
            {
                var list = _purchaseitemdetailservice.GetReportByItemAndDate(singleitem[i], FDate, TDate);
                foreach (var data in list)
                {
                    var PONo = data.PoNo;
                    var PODetails = _purchaseorderdetailservice.GetDetailByPoNo(PONo);
                    POList.Add(PODetails);
                }
            }
            model.PurchaseOrderList = POList;
            TempData["PurchaseOrderListItemwise"] = model.PurchaseOrderList;
            TempData["ConvertToPdf"] = "No";
            return View(model);
        }

        //PRINT PAGE OF PURCHASE ORDER REPORT ITEMWISE
        [HttpGet]
        public ActionResult PrintPurchaseOrderItemwise()
        {
            MainApplication model = new MainApplication();
            model.PurchaseOrderList = TempData["PurchaseOrderListItemwise"] as IEnumerable<PurchaseOrderDetail>;
            TempData["PurchaseOrderListItemwise"] = model.PurchaseOrderList;
            return View(model);
        }


        // ***************************************** ITEM WISE REPORT *********************************************
        [HttpGet]
        public ActionResult ItemWiseReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
            return View(model);
        }

        //GET SUB CATEGORY BY CATEGORY
        [HttpGet]
        public JsonResult LoadSubCategoryByCategory(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                MainApplication model = new MainApplication();
                var list = _ItemSubCategoryService.GetSubCategoryByCategory(id);
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
        public JsonResult LoadItemsBySubCategory(string subcat)
        {
            if (!string.IsNullOrEmpty(subcat))
            {
                MainApplication model = new MainApplication();
                var list = _ItemService.GetItemsBySubCategory(subcat);
                model.ItemList = list;
                var data = model.ItemList.Select(s => new SelectListItem()
                {
                    Text = s.itemName,
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

        [HttpGet]
        public JsonResult GetItemDetailsForItemwiseReport(string Item)
        {
            var data = _ItemService.GetDescriptionByItemCode(Item);
            return Json(new { data.itemCode, data.itemName, data.typeOfMaterial, data.colorCode, data.brandName, data.size, data.unit, data.designName, data.costprice, data.sellingprice, data.mrp }, JsonRequestBehavior.AllowGet);
        }

        // *************************** OPENING STOCK REPORT ************************************************************
        [HttpGet]
        public ActionResult OpeningStockReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM LIST
        [HttpGet]
        public ActionResult GetOpeningStockItemsByDateAndCategory(string fromdate, string todate, string category)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(fromdate);
            DateTime TDate = Convert.ToDateTime(todate);

            string CatNames = category;
            string[] SingleCat = CatNames.Split(',');
            List<OpeningStockMaster> Catlist = new List<OpeningStockMaster>();

            for (int i = 0; i < SingleCat.Count(); i++)
            {
                var list = _openingstockservice.GetDetailsByDateAndCategory(FDate, TDate, SingleCat[i]);
                foreach (var data in list)
                {
                    Catlist.Add(data);
                }
            }
            model.OpeningStockList = Catlist;
            TempData["OpeningStockItemList"] = model.OpeningStockList;
            return View(model);
        }

        //PRINT PAGE OF OPENING STOCK
        [HttpGet]
        public ActionResult PrintOpeningStock()
        {
            MainApplication model = new MainApplication();
            model.OpeningStockList = TempData["OpeningStockItemList"] as IEnumerable<OpeningStockMaster>;
            TempData["OpeningStockItemList"] = model.OpeningStockList;
            return View(model);
        }

        // ****************************** ENTRY STOCK REPORT ***********************************************************
        [HttpGet]
        public ActionResult EntryStockReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //ENTRY STOCK REPORT ITEMWISE
        [HttpGet]
        public ActionResult EntryStockReportItemwise()
        {
            MainApplication model = new MainApplication();
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN ENTRY STOCK REPORT ITEMWISE
        [HttpGet]
        public ActionResult GetEntryStockItemDetailsByCategory(string category)
        {
            MainApplication model = new MainApplication();
            model.EntryStockItemList = _entrystockitemservice.GetDetailsByCategory(category);
            TempData["EntryStockItemListCatWise"] = model.EntryStockItemList;
            return View(model);
        }

        //PRINT PAGE OF ENTRY STOCK
        [HttpGet]
        public ActionResult PrintEntryStockCategorywise()
        {
            MainApplication model = new MainApplication();
            model.EntryStockItemList = TempData["EntryStockItemListCatWise"] as IEnumerable<EntryStockItem>;
            TempData["EntryStockItemListCatWise"] = model.EntryStockItemList;
            return View(model);
        }


        //ENTRY STOCK REPORT ITEMWISE
        [HttpGet]
        public ActionResult EntryStockReportDatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN ENTRY STOCK REPORT DATEWISE
        [HttpGet]
        public ActionResult GetEntryStockItemDetailsByDate(DateTime fromdate, DateTime todate)
        {
            MainApplication model = new MainApplication();
            model.EntryStockItemList = _entrystockitemservice.GetDetailsByDate(fromdate, todate);
            TempData["EntryStockItemListByDatewise"] = model.EntryStockItemList;
            return View(model);
        }

        //PRINT PAGE OF ENTRY STOCK
        [HttpGet]
        public ActionResult PrintEntryStockDatewise()
        {
            MainApplication model = new MainApplication();
            model.EntryStockItemList = TempData["EntryStockItemListByDatewise"] as IEnumerable<EntryStockItem>;
            TempData["EntryStockItemListByDatewise"] = model.EntryStockItemList;
            return View(model);
        }

        // ********************************** STOCK DISTRIBUTION REPORT *****************************************************
        [HttpGet]
        public ActionResult StockDistributionReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _godownservice.GetGodownNames();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY GODOWN DETAILS IN STOCK DISTRIBUTION REPORT
        [HttpGet]
        public ActionResult GetStockDistributionDetails(string godowncode)
        {
            MainApplication model = new MainApplication();

            //USING STORE PROCEDURE GET ITEMCODE LIST USING GROUP BY CLAUSE
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@godowncode",godowncode),
            };
            string query = "GetItemsFromStockDistribution" + " " + "@godowncode";
            var Itemlist = _stockitemdistributionservice.GetItemsUsingStoreProc(query, para);
            List<StockItemDistribution> list = new List<StockItemDistribution>();
            foreach (var data in Itemlist)
            {
                var details = _stockitemdistributionservice.GetLastRowFromItemAndGodownCode(data.itemCode, godowncode);
                list.Add(details);
            }
            model.StockItemDistributionList = list;
            TempData["StockDistributionItems"] = model.StockItemDistributionList;
            return View(model);
        }

        //PRINT PAGE OF STOCK DISTRIBUTION
        [HttpGet]
        public ActionResult PrintStockDistribution()
        {
            MainApplication model = new MainApplication();
            model.StockItemDistributionList = TempData["StockDistributionItems"] as IEnumerable<StockItemDistribution>;
            return View(model);
        }

        // ********************************************* INWARD REPORT *******************************************************

        //INWARD WITH PO REPORT
        [HttpGet]
        public ActionResult InwardsWithPOReports()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //INWARD WITH PO REPORT DATEWISE
        [HttpGet]
        public ActionResult InwardWithPODatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY INWARD WITH PO DATEWISE
        [HttpGet]
        public ActionResult GetInwardWithPOByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardFromSupplierList = _InwardFromSupplierService.getInwardWithPOReportByDate(FDate, TDate);
            TempData["InwardFromSupplierListDatewise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //PRINT PAGE OF INWARD WITH PO DATEWISE
        [HttpGet]
        public ActionResult PrintInwardWithPODatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = TempData["InwardFromSupplierListDatewise"] as IEnumerable<InwardFromSupplier>;
            TempData["InwardFromSupplierListDatewise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //INWARD WITH PO REPORT SUPPLIERWISE
        [HttpGet]
        public ActionResult InwardWithPOSupplierwise()
        {
            MainApplication model = new MainApplication();
            model.SupplierList = _supplierservice.getAllSuppliers();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY INWARD WITH PO SUPPLIERWISE
        [HttpGet]
        public ActionResult GetInwardWithPOBySupplier(string Supplier, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string SuppNames = Supplier;
            string[] SingleSupp = SuppNames.Split(',');
            List<InwardFromSupplier> inwardlist = new List<InwardFromSupplier>();

            for (int i = 0; i < SingleSupp.Count(); i++)
            {
                var list = _InwardFromSupplierService.GetInwardWithPOReportBySupplierNameAndDate(SingleSupp[i], FDate, TDate);
                foreach (var data in list)
                {
                    inwardlist.Add(data);
                }
            }
            model.InwardFromSupplierList = inwardlist;
            TempData["InwardFromSupplierListSupplerwise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //PRINT PAGE OF INWARD FWITH PO SUPPLIERWISE
        [HttpGet]
        public ActionResult PrintInwardWithPOSupplierwise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = TempData["InwardFromSupplierListSupplerwise"] as IEnumerable<InwardFromSupplier>;
            TempData["InwardFromSupplierListSupplerwise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //INWARD WITHOUT PO REPORT
        [HttpGet]
        public ActionResult InwardsWithoutPOReports()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //INWARD WITHOUT PO REPORT DATEWISE
        [HttpGet]
        public ActionResult InwardWithoutPODatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY INWARD WITHOUT PO DATEWISE
        [HttpGet]
        public ActionResult GetInwardWithoutPOByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardFromSupplierList = _InwardFromSupplierService.getInwardWithoutPOReportByDate(FDate, TDate);
            TempData["InwardFromSupplierListDatewise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //PRINT PAGE OF INWARD WITHOUT PO DATEWISE
        [HttpGet]
        public ActionResult PrintInwardWithoutPODatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = TempData["InwardFromSupplierListDatewise"] as IEnumerable<InwardFromSupplier>;
            TempData["InwardFromSupplierListDatewise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //INWARD WITHOUT PO REPORT SUPPLIERWISE
        [HttpGet]
        public ActionResult InwardWithoutPOSupplierwise()
        {
            MainApplication model = new MainApplication();
            model.SupplierList = _supplierservice.getAllSuppliers();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY INWARD WITHOUT PO SUPPLIERWISE
        [HttpGet]
        public ActionResult GetInwardWithoutPOBySupplier(string Supplier, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string SuppNames = Supplier;
            string[] SingleSupp = SuppNames.Split(',');
            List<InwardFromSupplier> inwardlist = new List<InwardFromSupplier>();

            for (int i = 0; i < SingleSupp.Count(); i++)
            {
                var list = _InwardFromSupplierService.GetInwardWithoutPOReportBySupplierNameAndDate(SingleSupp[i], FDate, TDate);
                foreach (var data in list)
                {
                    inwardlist.Add(data);
                }
            }
            model.InwardFromSupplierList = inwardlist;
            TempData["InwardFromSupplierListSupplerwise"] = model.InwardFromSupplierList;
            return View(model);
        }

        //PRINT PAGE OF INWARD WITHOUT PO SUPPLIERWISE
        [HttpGet]
        public ActionResult PrintInwardWithoutPOSupplierwise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = TempData["InwardFromSupplierListSupplerwise"] as IEnumerable<InwardFromSupplier>;
            TempData["InwardFromSupplierListSupplerwise"] = model.InwardFromSupplierList;
            return View(model);
        }

        [HttpGet]
        public ActionResult GodownShopInwardsReports()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }


        //INWARD TO SHOP REPORT
        [HttpGet]
        public ActionResult InwardToShop()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //INWARD TO SHOP REPORT DATEWISE
        [HttpGet]
        public ActionResult InwardToShopDatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY INWARD TO SHOP DATEWISE
        [HttpGet]
        public ActionResult GetInwardToShopDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardFromGodownList = _InwardFromGodownService.GetReportByDate(FDate, TDate);
            TempData["InwardToShopListDatewise"] = model.InwardFromGodownList;
            return View(model);
        }

        //PRINT PAGE OF INWARD TO SHOP DATEWISE
        [HttpGet]
        public ActionResult PrintInwardToShopDatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromGodownList = TempData["InwardToShopListDatewise"] as IEnumerable<InwardFromGodown>;
            TempData["InwardToShopListDatewise"] = model.InwardFromGodownList;
            return View(model);
        }

        //INWARD TO SHOP REPORT GODOWNWISE
        [HttpGet]
        public ActionResult InwardToShopGodownwise()
        {
            MainApplication model = new MainApplication();
            model.GodownMasterList = _godownservice.GetGodownNames();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY INWARD TO SHOP GODOWNWISE
        [HttpGet]
        public ActionResult GetInwardToShopDetailsByGodown(string Godown, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string GdnNames = Godown;
            string[] SingleGdn = GdnNames.Split(',');
            List<InwardFromGodown> inwardlist = new List<InwardFromGodown>();

            for (int i = 0; i < SingleGdn.Count(); i++)
            {
                var list = _InwardFromGodownService.GetReportByGodownNameAndDate(SingleGdn[i], FDate, TDate);
                foreach (var data in list)
                {
                    inwardlist.Add(data);
                }
            }
            model.InwardFromGodownList = inwardlist;
            TempData["InwardToShopListGodownwise"] = model.InwardFromGodownList;
            return View(model);
        }

        //PRINT PAGE OF INWARD TO SHOP GodownWISE
        [HttpGet]
        public ActionResult PrintInwardToShopGodownwise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromGodownList = TempData["InwardToShopListGodownwise"] as IEnumerable<InwardFromGodown>;
            TempData["InwardToShopListGodownwise"] = model.InwardFromGodownList;
            return View(model);
        }

        //INWARD TO GODOWN REPORT
        [HttpGet]
        public ActionResult InwardToGodown()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY INWARD TO GODOWN DATEWISE
        [HttpGet]
        public ActionResult GetInwardToGodownDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardFromStockDistributionList = _InwardFromStkDistService.GetReportByDate(FDate, TDate);
            TempData["InwardToGodownListDatewise"] = model.InwardFromStockDistributionList;
            return View(model);
        }

        //PRINT PAGE OF INWARD TO GODOWN DATEWISE
        [HttpGet]
        public ActionResult PrintInwardToGodownDatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromStockDistributionList = TempData["InwardToGodownListDatewise"] as IEnumerable<InwardFromStockDistribution>;
            TempData["InwardToGodownListDatewise"] = model.InwardFromStockDistributionList;
            return View(model);
        }

        // ******************************************* OUTWARD REPORT ***************************************************

        [HttpGet]
        public ActionResult OutwardReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //OUTWARD TO SHOP REPORT
        [HttpGet]
        public ActionResult OutwardToShopReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //OUTWARD TO SHOP REPORT DATEWISE
        [HttpGet]
        public ActionResult OutwardToShopDatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY OUTWARD TO SHOP DATEWISE
        [HttpGet]
        public ActionResult GetOutwardToShopDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.OutwardToShopList = _outwardfromgodowntoshopservice.GetDetailsByDate(FDate, TDate);
            TempData["OutwardToShopListDatewise"] = model.OutwardToShopList;
            return View(model);
        }

        //PRINT PAGE OF OUTWARD TO SHOP DATEWISE
        [HttpGet]
        public ActionResult PrintOutwardToShopDatewise()
        {
            MainApplication model = new MainApplication();
            model.OutwardToShopList = TempData["OutwardToShopListDatewise"] as IEnumerable<OutwardToShop>;
            TempData["OutwardToShopListDatewise"] = model.OutwardToShopList;
            return View(model);
        }

        //OUTWARD TO SHOP REPORT GODOWNWISE
        [HttpGet]
        public ActionResult OutwardToShopGodownwise()
        {
            MainApplication model = new MainApplication();
            model.GodownMasterList = _godownservice.GetGodownNames();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY OUTWARD TO SHOP GODOWNWISE
        [HttpGet]
        public ActionResult GetOutwardToShopDetailsByGodown(string Godown, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string GdnNames = Godown;
            string[] SingleGdn = GdnNames.Split(',');
            List<OutwardToShop> outwardlist = new List<OutwardToShop>();

            model.OutwardToShopList = outwardlist;
            TempData["OutwardToShopListGodownwise"] = model.OutwardToShopList;
            return View(model);
        }

        //PRINT PAGE OF OUTWARD TO SHOP GODOWNWISE
        [HttpGet]
        public ActionResult PrintOutwardToShopGodownwise()
        {
            MainApplication model = new MainApplication();
            model.OutwardToShopList = TempData["OutwardToShopListGodownwise"] as IEnumerable<OutwardToShop>;
            TempData["InwardToShopListGodownwise"] = model.OutwardToShopList;
            return View(model);
        }

        //OUTWARD TO CLIENT REPORT
        [HttpGet]
        public ActionResult OutwardToClientReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //OUTWARD TO CLIENT REPORT DATEWISE
        [HttpGet]
        public ActionResult OutwardToClientDatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY OUTWARD TO CLIENT DATEWISE
        [HttpGet]
        public ActionResult GetOutwardToClientDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.OutwardToClientList = _OutwardToClientService.GetDetailsByDate(FDate, TDate);
            TempData["OutwardToClientListDatewise"] = model.OutwardToClientList;
            return View(model);
        }

        //PRINT PAGE OF OUTWARD TO CLIENT DATEWISE
        [HttpGet]
        public ActionResult PrintOutwardToClientDatewise()
        {
            MainApplication model = new MainApplication();
            model.OutwardToClientList = TempData["OutwardToClientListDatewise"] as IEnumerable<OutwardToClient>;
            TempData["OutwardToClientListDatewise"] = model.OutwardToClientList;
            return View(model);
        }

        //OUTWARD TO SHOP REPORT GODOWNWISE
        [HttpGet]
        public ActionResult OutwardToClientClientwise()
        {
            MainApplication model = new MainApplication();
            model.ClientList = _ClientMasterService.GetAllClients();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY OUTWARD TO SHOP GODOWNWISE
        [HttpGet]
        public ActionResult GetOutwardToClientDetailsByClient(string Client, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string ClientNames = Client;
            string[] SingleClient = ClientNames.Split(',');
            List<OutwardToClient> clientlist = new List<OutwardToClient>();

            for (int i = 0; i < SingleClient.Count(); i++)
            {
                var list = _OutwardToClientService.GetReportByClientAndDate(SingleClient[i], FDate, TDate);
                foreach (var data in list)
                {
                    clientlist.Add(data);
                }
            }
            model.OutwardToClientList = clientlist;
            TempData["OutwardToClientListClientwise"] = model.OutwardToClientList;
            return View(model);
        }

        //PRINT PAGE OF OUTWARD TO SHOP GODOWNWISE
        [HttpGet]
        public ActionResult PrintOutwardToClientClientwise()
        {
            MainApplication model = new MainApplication();
            model.OutwardToClientList = TempData["OutwardToClientListClientwise"] as IEnumerable<OutwardToClient>;
            TempData["InwardToShopListGodownwise"] = model.OutwardToClientList;
            return View(model);
        }

        //OUTWARD FOR INTER GODOWN REPORT
        [HttpGet]
        public ActionResult OutwardInterGodownReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY OUTWARD FOR INTER GODOWN DATEWISE
        [HttpGet]
        public ActionResult GetOutwardInterGodownDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.OutwardInterGodownList = _OutwardInterGodownService.GetReportByDate(FDate, TDate);
            TempData["OutwardInterGodownListDatewise"] = model.OutwardInterGodownList;
            return View(model);
        }

        //PRINT PAGE OF OUTWARD FOR INTER GODOWN DATEWISE
        [HttpGet]
        public ActionResult PrintOutwardInterGodownDatewise()
        {
            MainApplication model = new MainApplication();
            model.OutwardInterGodownList = TempData["OutwardInterGodownListDatewise"] as IEnumerable<OutwardInterGodown>;
            TempData["OutwardInterGodownListDatewise"] = model.OutwardInterGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardInterShopReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY OUTWARD FOR INTER GODOWN DATEWISE
        [HttpGet]
        public ActionResult GetOutwardInterShopDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.OutwardInterShopList = _OutwardInterShopService.GetDetailsByDate(FDate, TDate);
            TempData["OutwardInterShopListDatewise"] = model.OutwardInterShopList;
            return View(model);
        }

        //PRINT PAGE OF OUTWARD FOR INTER GODOWN DATEWISE
        [HttpGet]
        public ActionResult PrintOutwardInterShopDatewise()
        {
            MainApplication model = new MainApplication();
            model.OutwardInterShopList = TempData["OutwardInterShopListDatewise"] as IEnumerable<OutwardInterShop>;
            TempData["OutwardInterShopListDatewise"] = model.OutwardInterShopList;
            return View(model);
        }

        // ******************************************* BILLING REPORT ***************************************************

        //MAIN PAGE OF BILLING REPORTS
        [HttpGet]
        public ActionResult BillingReports()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.QuotationList = _QuotationService.GetAll();
            TempData["Quotations"] = model.QuotationList;
            model.SalesOrderList = _SalesOrderService.GetAll();
            TempData["SalesOrders"] = model.SalesOrderList;
            model.DeliveryChallanList = _DeliveryChallanSevice.GetAll();
            TempData["DeliveryChallans"] = model.DeliveryChallanList;
            model.SalesBillList = _SalesBillService.GetAll();
            TempData["SalesBills"] = model.SalesBillList;
            model.RetailBillList = _RetailInvoiceMasterService.GetAll();
            TempData["RetailBills"] = model.RetailBillList;
            model.TemporaryCashMemoList = _TemporaryCashMemoService.GetAll();
            TempData["TemporaryCashMemoes"] = model.TemporaryCashMemoList;
            return View(model);
        }

        //DISPLAY DETAILS OF QUOTATONS
        [HttpGet]
        public ActionResult GetQuotationList()
        {
            MainApplication model = new MainApplication();
            model.QuotationList = TempData["Quotations"] as IEnumerable<Quotation>;
            TempData["Quotations"] = model.QuotationList;
            return View(model);
        }

        //DISPLAY DETAILS OF SALES ORDER
        [HttpGet]
        public ActionResult GetSalesOrderList()
        {
            MainApplication model = new MainApplication();
            model.SalesOrderList = TempData["SalesOrders"] as IEnumerable<SalesOrder>;
            TempData["SalesOrders"] = model.SalesOrderList;
            return View(model);
        }

        //DISPLAY DETAILS OF DELIVERY CHALLAN
        [HttpGet]
        public ActionResult GetDeliveryChallanList()
        {
            MainApplication model = new MainApplication();
            model.DeliveryChallanList = TempData["DeliveryChallans"] as IEnumerable<DeliveryChallan>;
            TempData["DeliveryChallans"] = model.DeliveryChallanList;
            return View(model);
        }

        //DISPLAY DETAILS OF SALES BILL
        [HttpGet]
        public ActionResult GetSalesBillList()
        {
            MainApplication model = new MainApplication();
            model.SalesBillList = TempData["SalesBills"] as IEnumerable<SalesBill>;
            TempData["SalesBills"] = model.SalesBillList;
            return View(model);
        }

        //DISPLAY DETAILS OF RETAIL BILL
        [HttpGet]
        public ActionResult GetRetailBillList()
        {
            MainApplication model = new MainApplication();
            model.RetailBillList = TempData["RetailBills"] as IEnumerable<RetailBill>;
            TempData["RetailBills"] = model.RetailBillList;
            return View(model);
        }

        //DISPLAY DETAILS OF TEMPORARY CASH MEMO
        [HttpGet]
        public ActionResult GetTemporaryCashMemoList()
        {
            MainApplication model = new MainApplication();
            model.TemporaryCashMemoList = TempData["TemporaryCashMemoes"] as IEnumerable<TemporaryCashMemo>;
            TempData["TemporaryCashMemoes"] = model.CashierTemporaryCashMemoList;
            return View(model);
        }

        // ************************************ GODOWN STOCK REPORTS **************************************************
        //GODOWN STOCK REPORTS
        [HttpGet]
        public ActionResult GodownStockReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _godownservice.GetGodownNames();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM LIST
        [HttpGet]
        public ActionResult GetGodownStockItemDetails(string gdcode)
        {
            MainApplication model = new MainApplication();
            model.GodownStockList = _GodownStockService.GetRowsByGodownCode(gdcode);
            TempData["GodownStockList"] = model.GodownStockList;
            return View(model);
        }

        //PRINT PAGE OF OPENING STOCK
        [HttpGet]
        public ActionResult PrintGodownStock()
        {
            MainApplication model = new MainApplication();
            model.GodownStockList = TempData["GodownStockList"] as IEnumerable<GodownStock>;
            TempData["GodownStockList"] = model.GodownStockList;
            return View(model);
        }

        // ************************************ SHOP STOCK REPORTS **************************************************
        //SHOP STOCK REPORTS
        [HttpGet]
        public ActionResult ShopStockReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ShopList = _ShopSevice.GetAll();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM LIST
        [HttpGet]
        public ActionResult GetShopStockItemDetails(string shopcode)
        {
            MainApplication model = new MainApplication();
            model.ShopStockList = _ShopStockService.GetRowsByShopCode(shopcode);
            TempData["ShopStockList"] = model.ShopStockList;
            return View(model);
        }

        //PRINT PAGE OF OPENING STOCK
        [HttpGet]
        public ActionResult PrintShopStock()
        {
            MainApplication model = new MainApplication();
            model.ShopStockList = TempData["ShopStockList"] as IEnumerable<ShopStock>;
            TempData["ShopStockList"] = model.ShopStockList;
            return View(model);
        }


        /********************************** Outward From Shop To Godown Reports****************/

        [HttpGet]
        public ActionResult OutwardFromShopToGodownReports()
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
        public ActionResult OutwardFromShopToGodownDatewise()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOutwardFromShopToGodownDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.OutwardShopToGodownList = _OutwardShopToGodownService.GetDetailsByDate(FDate, TDate);
            TempData["OutwardShopToGodownListDatewise"] = model.OutwardShopToGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintOutwardFromShopToGodownDatewise()
        {
            MainApplication model = new MainApplication();
            model.OutwardShopToGodownList = TempData["OutwardShopToGodownListDatewise"] as IEnumerable<OutwardShopToGodown>;
            TempData["OutwardShopToGodownListDatewise"] = model.OutwardShopToGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardFromShopToGodownShopwise()
        {
            MainApplication model = new MainApplication();
            model.ShopList = _ShopSevice.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult GetOutwardFromShopToGodownDetailsByShop(string Shop, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string ShopNames = Shop;
            string[] SingleShop = ShopNames.Split(',');
            List<OutwardShopToGodown> outwardlist = new List<OutwardShopToGodown>();

            for (int i = 0; i < SingleShop.Count(); i++)
            {
                var list = _OutwardShopToGodownService.GetDetailsByShopAndDate(SingleShop[i], FDate, TDate);
                foreach (var data in list)
                {
                    outwardlist.Add(data);
                }
            }
            model.OutwardShopToGodownList = outwardlist;
            TempData["OutwardFromShopToGodownShopwise"] = model.OutwardShopToGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintOutwardShopToGodownShopwise()
        {
            MainApplication model = new MainApplication();
            model.OutwardShopToGodownList = TempData["OutwardFromShopToGodownShopwise"] as IEnumerable<OutwardShopToGodown>;
            TempData["OutwardFromShopToGodownShopwise"] = model.OutwardShopToGodownList;
            return View(model);
        }

        /********************************END Outward From Shop To Godown Reports****************/




        /********************************Inward From Shop To Godown Reports****************/

        [HttpGet]
        public ActionResult InwardFromShopToGodownReports()
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
        public ActionResult InwardFromShopToGodownDatewise()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetInwardFromShopToGodownDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardFromShopToGodownList = _InwardFromShopToGodownService.GetDetailsByDate(FDate, TDate);
            TempData["InwardShopToGodownListDatewise"] = model.InwardFromShopToGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardFromShopToGodownDatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromShopToGodownList = TempData["InwardShopToGodownListDatewise"] as IEnumerable<InwardFromShopToGodown>;
            TempData["InwardShopToGodownListDatewise"] = model.InwardFromShopToGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardFromShopToGodownShopwise()
        {
            MainApplication model = new MainApplication();
            model.ShopList = _ShopSevice.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult GetInwardFromShopToGodownDetailsByShop(string Shop, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);

            string ShopNames = Shop;
            string[] SingleShop = ShopNames.Split(',');
            List<InwardFromShopToGodown> inwardlist = new List<InwardFromShopToGodown>();

            for (int i = 0; i < SingleShop.Count(); i++)
            {
                var list = _InwardFromShopToGodownService.GetDetailsByShopAndDate(SingleShop[i], FDate, TDate);
                foreach (var data in list)
                {
                    inwardlist.Add(data);
                }
            }
            model.InwardFromShopToGodownList = inwardlist;
            TempData["InwardFromShopToGodownShopwise"] = model.InwardFromShopToGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardFromShopToGodownShopwise()
        {
            MainApplication model = new MainApplication();
            model.InwardFromShopToGodownList = TempData["InwardFromShopToGodownShopwise"] as IEnumerable<InwardFromShopToGodown>;
            TempData["InwardFromShopToGodownShopwise"] = model.InwardFromShopToGodownList;
            return View(model);
        }


        [HttpGet]
        public ActionResult InwardInterGodownReport()
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
        public ActionResult GetInwardInterGodownDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardInterGodownList = _InwardInterGodownService.GetDetailsByDate(FDate, TDate);
            TempData["InwardInterGodownListDatewise"] = model.InwardInterGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardInterGodownDatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardInterGodownList = TempData["InwardInterGodownListDatewise"] as IEnumerable<InwardInterGodown>;
            TempData["InwardInterGodownListDatewise"] = model.InwardInterGodownList;
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardInterShopReport()
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
        public ActionResult GetInwardInterShopDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.InwardInterShopList = _InwardInterShopService.GetDetailsByDate(FDate, TDate);
            TempData["InwardInterShopListDatewise"] = model.InwardInterShopList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardInterShopDatewise()
        {
            MainApplication model = new MainApplication();
            model.InwardInterShopList = TempData["InwardInterShopListDatewise"] as IEnumerable<InwardInterShop>;
            TempData["InwardInterShopListDatewise"] = model.InwardInterShopList;
            return View(model);
        }

        /********************************END Inward From Shop To Godown Reports****************/

        [HttpGet]
        public ActionResult CommissionReport()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.getActiveEmployee();
            model.UnitList = _UnitService.getallsize();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetEmployees(string ShopName)
        {
            var EmpName = _EmployeeMasterService.GetEmployeeListBySalesAndTarget();  
            var select = EmpName.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name
            });
            return Json(select, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetEmployeeByName(string name)
        {
            var details = _EmployeeMasterService.getEmpByName(name);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRetailBillDetailsByShop(string FromDate, string ToDate, string Emp, string Unit)
        {
            MainApplication model = new MainApplication();
            DateTime fromdate = Convert.ToDateTime(FromDate);
            DateTime todate = Convert.ToDateTime(ToDate);

            string[] EmpName = Emp.Split(',');
            string[] UnitName = Unit.Split(',');
            List<string> emplist = new List<string>();


            for (int i = 0; i < EmpName.Length; i++)
            {
                for (int j = 0; j < UnitName.Length; j++)
                {
                    var list = _EmployeeMasterService.GetEmployeesByTarget(EmpName[i]);
                    foreach (var newdata in list)
                    {
                        if (!emplist.Contains(newdata))
                        {
                            emplist.Add(newdata);
                        }
                    }
                }
            }

            EmpName = new string[EmpName.Length];
            int k = 0;
            foreach (var data in emplist)
            {
                EmpName[k] = data;
                k++;
            }

            List<RetailBill> retailbilllist = new List<RetailBill>();
            List<EmployeeMaster> EmpList = new List<EmployeeMaster>();
            List<RetailBillItem> RetailItemList = new List<RetailBillItem>();
            List<EmployeeMaster> FinalEmpList = new List<EmployeeMaster>();
            List<CommissionMaster> CommissionList = new List<CommissionMaster>();
            List<CommissionProduct> CommProductList = new List<CommissionProduct>();

            for (int i = 0; i < EmpName.Count(); i++)
            {
                var list = _RetailInvoiceMasterService.GetReportByDate(fromdate, todate, EmpName[i]);
                foreach (var data in list)
                {
                    retailbilllist.Add(data);
                }
            }

            foreach (var item in retailbilllist)
            {
                var data = _RetailBillItemService.GetDetailsByInvoiceNo(item.RetailBillNo);
                foreach (var items in data)
                {
                    RetailItemList.Add(items);
                }
            }

            for (int i = 0; i < EmpName.Length; i++)
            {
                var empnames = _EmployeeMasterService.getEmpByName(EmpName[i]);
                if (empnames != null)
                {
                    FinalEmpList.Add(empnames);
                }
            }

            foreach (var emp in FinalEmpList)
            {
                var list = _CommissionService.GetCommByEmployeeName(emp.Name, fromdate, todate);
                if (list != null)
                {
                    CommissionList.Add(list);
                }

                if (list != null)
                {
                    var newlist = _CommissionProductService.GetDetailsByCommCode(list.CommissionCode);
                    foreach (var data in newlist)
                    {
                        CommProductList.Add(data);
                    }
                }
            }

            model.RetailBillList = retailbilllist;
            TempData["UnitList"] = UnitName;
            model.EmpList = FinalEmpList;
            model.RetailBillItemList = RetailItemList;
            model.CommissionMasterList = CommissionList;
            model.CommissionProductList = CommProductList;

            TempData["RetailBillItemList"] = model.RetailBillItemList;
            TempData["EmpList"] = model.EmpList;
            TempData["RetailBills"] = model.RetailBillList;
            TempData["Comm"] = model.CommissionMasterList;
            TempData["CommProd"] = model.CommissionProductList;
            return View(model);
        }

        public ActionResult PrintCommissionReport()
        {
            MainApplication model = new MainApplication();
            model.RetailBillItemList = TempData["RetailBillItemList"] as IEnumerable<RetailBillItem>;
            model.EmpList = TempData["EmpList"] as IEnumerable<EmployeeMaster>;
            model.RetailBillList = TempData["RetailBills"] as IEnumerable<RetailBill>;
            model.CommissionMasterList = TempData["Comm"] as IEnumerable<CommissionMaster>;
            model.CommissionProductList = TempData["CommProd"] as IEnumerable<CommissionProduct>;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetItemDetails(string ItemCode)
        {
            var details = _ItemService.GetDescriptionByItemCode(ItemCode);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SaveCommission(DateTime FromDate, DateTime ToDate, string EmpName,string Amount)
        {
            var details = _CommissionService.GetCommByEmployeeName(EmpName,FromDate, ToDate);
            if (details != null)
            {
                details.Amount = Amount;
                _CommissionService.Update(details);
            }
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CommissionReleaseReport()
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
        public ActionResult GetCommissionEmployeewise(DateTime FromDate, DateTime ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime fromdate = Convert.ToDateTime(FromDate);
            DateTime todate = Convert.ToDateTime(ToDate);
            model.CommissionMasterList = _CommissionService.CommissionGivenEmployees(fromdate, todate);
            TempData["ReleaseReport"] = model.CommissionMasterList;

            List<string> code = new List<string>();
            foreach (var data in model.CommissionMasterList)
            {
                string details = _EmployeeMasterService.getEmpByName(data.EmployeeName).EmployeeCode;
                code.Add(details);
            }

            TempData["Code"] = code;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintCommissionReleaseReport()
        {
            MainApplication model = new MainApplication();
            model.CommissionMasterList = TempData["ReleaseReport"] as IEnumerable<CommissionMaster>;
            List<string> code = new List<string>();
            foreach (var data in model.CommissionMasterList)
            {
                string details = _EmployeeMasterService.getEmpByName(data.EmployeeName).EmployeeCode;
                code.Add(details);
            }
            TempData["EmpCode"] = code;
            return View(model);
        }

        /************************ MASTER REPORTS *****************************/
        [HttpGet]
        public ActionResult ClientReport()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ClientList = _ClientMasterService.GetAllClients();
            return View(model);
        }

        [HttpGet]
        public ActionResult ClientBankDetails(string Code)
        {
            MainApplication model = new MainApplication();
            model.ClientBankDetailList = _ClientBankService.GetDetailsFromBank(Code);
            return View(model);
        }

        [HttpGet]
        public ActionResult SupplierReport()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SupplierList = _supplierservice.getAllSuppliers();
            return View(model);
        }

        [HttpGet]
        public ActionResult SupplierBankDetails(string Code)
        {
            MainApplication model = new MainApplication();
            model.SupplierBankDetailList = _SupplierBankDetailService.GetDetailsFromBank(Code);
            return View(model);
        }

        [HttpGet]
        public ActionResult ItemTaxReport()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.ItemTaxList = _ItemTaxService.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult EmployeeReport()
        {
            MainApplication model = new MainApplication();
            model.EmployeeDetails = new EmployeeMaster();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmployeeDetails.deptlist = _DepartmentService.getAllDepartments();
            return View(model);
        }

        [HttpGet]
        public ActionResult AllEmployee()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.EmpList = _EmployeeMasterService.getAllemployees();
            TempData["EmpList"] = model.EmpList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintAllEmployees()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.EmpList = TempData["EmpList"] as IEnumerable<EmployeeMaster>;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetEmployeesByDept(string Name)
        {
            MainApplication model = new MainApplication();
            model.EmpList = _EmployeeMasterService.GetEmployeeListByDepartment(Name);
            TempData["DeptEmpList"] = model.EmpList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintEmployeesDepartmentwise()
        {
            MainApplication model = new MainApplication();
            model.EmpList = TempData["DeptEmpList"] as IEnumerable<EmployeeMaster>;
            return View(model);
        }




        // ******************************************* CASHIER REPORT ***************************************************

        //MAIN PAGE OF CASHIER REPORTS
        [HttpGet]
        public ActionResult CashierReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            model.CashierSalesOrderList = _CashierSalesOrderService.GetAll();
            TempData["CashierSalesOrders"] = model.CashierSalesOrderList;
            model.CashierRetailBillList = _CashierRetailBillService.GetAll();
            TempData["CashierRetailBills"] = model.CashierRetailBillList;
            model.CashierTemporaryCashMemoList = _CashierTemporaryCashMemoService.GetAll();
            TempData["CashierTemporaryCashMemoes"] = model.CashierTemporaryCashMemoList;
            model.CashierSalesBillList = _CashierSalesBillService.GetAll();
            TempData["CashierSalesBills"] = model.CashierSalesBillList;
            model.CashierRefundOrderList = _CashierRefundOrderService.GetAll();
            TempData["CashierRefundOrders"] = model.CashierRefundOrderList;
            model.CashHandoverList = _CashHandoverService.GetAll();
            TempData["CashHandovers"] = model.CashHandoverList;
            model.CardChequeHandoverList = _CardChequeHandoverService.GetAll();
            TempData["CardChequeHandovers"] = model.CardChequeHandoverList;
            model.IncomeExpenseVoucherList = _IncomeExchangeVoucherService.GetAll();
            TempData["IncomeExchangeVouchers"] = model.IncomeExpenseVoucherList;
            return View(model);
        }

        //DISPLAY DETAILS OF CASHIER SALES ORDERS
        [HttpGet]
        public ActionResult GetCashierSalesOrderList()
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderList = TempData["CashierSalesOrders"] as IEnumerable<CashierSalesOrder>;
            TempData["CashierSalesOrders"] = model.CashierSalesOrderList;
            return View(model);
        }

        //DISPLAY DETAILS OF CASHIER RETAIL BILLS
        [HttpGet]
        public ActionResult GetCashierRetailBillList()
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillList = TempData["CashierRetailBills"] as IEnumerable<CashierRetailBill>;
            TempData["CashierRetailBills"] = model.CashierRetailBillList;
            return View(model);
        }

        //DISPLAY DETAILS OF CASHIER TEMPORARY CASH MEMOES
        [HttpGet]
        public ActionResult GetCashierTemporaryCashMemoList()
        {
            MainApplication model = new MainApplication();
            model.CashierTemporaryCashMemoList = TempData["CashierTemporaryCashMemoes"] as IEnumerable<CashierTemporaryCashMemo>;
            TempData["CashierTemporaryCashMemoes"] = model.CashierTemporaryCashMemoList;
            return View(model);
        }

        //DISPLAY DETAILS OF CASHIER SALES BILLS
        [HttpGet]
        public ActionResult GetCashierSalesBillList()
        {
            MainApplication model = new MainApplication();
            model.CashierSalesBillList = TempData["CashierSalesBills"] as IEnumerable<CashierSalesBill>;
            TempData["CashierSalesBills"] = model.CashierSalesBillList;
            return View(model);
        }

        //DISPLAY DETAILS OF CASHIER REFUND ORDER
        [HttpGet]
        public ActionResult GetCashierRefundOrderList()
        {
            MainApplication model = new MainApplication();
            model.CashierRefundOrderList = TempData["CashierRefundOrders"] as IEnumerable<CashierRefundOrder>;
            TempData["CashierRefundOrders"] = model.CashierRefundOrderList;
            return View(model);
        }

        //DISPLAY DETAILS OF CASH HANDOVERS
        [HttpGet]
        public ActionResult GetCashHandoverList()
        {
            MainApplication model = new MainApplication();
            model.CashHandoverList = TempData["CashHandovers"] as IEnumerable<CashHandover>;
            TempData["CashHandovers"] = model.CashHandoverList;
            return View(model);
        }

        //DISPLAY DETAILS OF CARD CHEQUE HANDOVERS
        [HttpGet]
        public ActionResult GetCardChequeHandoverList()
        {
            MainApplication model = new MainApplication();
            model.CardChequeHandoverList = TempData["CardChequeHandovers"] as IEnumerable<CardChequeHandover>;
            TempData["CardChequeHandovers"] = model.CardChequeHandoverList;
            return View(model);
        }

        //DISPLAY DETAILS OF INCOME EXCHANGE VOUCHER
        [HttpGet]
        public ActionResult GetIncomeExchangeVoucherList()
        {
            MainApplication model = new MainApplication();
            model.IncomeExpenseVoucherList = TempData["IncomeExchangeVouchers"] as IEnumerable<IncomeExpenseVoucher>;
            TempData["IncomeExchangeVouchers"] = model.IncomeExpenseVoucherList;
            return View(model);
        }

        // ************************************ CREDIT AND DEBIT NOTE REPORT ***********************************

        //MAIN PAGE OF CASHIER REPORTS
        [HttpGet]
        public ActionResult DebitCreditNoteReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //GET ALL CREDIT NOTE REPORT
        [HttpGet]
        public ActionResult GetAllCreditNoteReport()
        {
            MainApplication model = new MainApplication();
            model.RetailBillCreditNoteList = _RetailBillCreditNoteService.GetAllCreditNotes();
            model.SalesBillCreditNoteList = _SalesBillCreditNoteService.GetAllCreditNotes();
            return View(model);
        }

        //CLIENT NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetClientNames(string term)
        {
            var result = _ClientMasterService.GetClientNames(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.ClientName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        
        //GET CREDIT NOTE REPORT BY CLIENT
        [HttpGet]
        public ActionResult GetCreditNoteReportsByClient(string ClientName)
        {
            MainApplication model = new MainApplication();
            model.RetailBillCreditNoteList = _RetailBillCreditNoteService.GetCreditNotesByClient(ClientName);
            model.SalesBillCreditNoteList = _SalesBillCreditNoteService.GetCreditNotesByClient(ClientName);
            return View(model);
        }

        //GET CREDIT NOTE REPORT DATEWISE
        [HttpGet]
        public ActionResult GetCreditNoteReportsByDate(DateTime fromdate, DateTime todate, string client)
        {
            MainApplication model = new MainApplication();
            if (client == "" || client == null)
            {
                model.RetailBillCreditNoteList = _RetailBillCreditNoteService.GetRowsByFromAndToDate(fromdate, todate);
                model.SalesBillCreditNoteList = _SalesBillCreditNoteService.GetRowsByFromAndToDate(fromdate, todate);
            }
            else
            {
                model.RetailBillCreditNoteList = _RetailBillCreditNoteService.GetRowsByFromAndToDateAndClient(fromdate, todate, client);
                model.SalesBillCreditNoteList = _SalesBillCreditNoteService.GetRowsByFromAndToDateAndClient(fromdate, todate, client);
            }
            return View(model);
        }

        //DEBIT NOTE REPORT
        [HttpGet]
        public ActionResult GetAllDebitNoteReport()
        {
            MainApplication model = new MainApplication();
            model.DebitNoteList = _DebitNoteService.GetAllDebitNotes();
            return View(model);
        }

        //CLIENT NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetSupplierNames(string term)
        {
            var result = _supplierservice.GetSupplierNames(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.SupplierName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //GET DEBIT NOTE REPORT BY CLIENT
        [HttpGet]
        public ActionResult GetDebitNoteReportsBySupplier(string SupplierName)
        {
            MainApplication model = new MainApplication();
            model.DebitNoteList = _DebitNoteService.GetDebitNotesBySupplier(SupplierName);
            return View(model);
        }

        //GET DEBIT NOTE REPORT DATEWISE
        [HttpGet]
        public ActionResult GetDebitNoteReportsByDate(DateTime fromdate, DateTime todate, string supplier)
        {
            MainApplication model = new MainApplication();
            if (supplier == "" || supplier == null)
            {
                model.DebitNoteList = _DebitNoteService.GetRowsByFromAndToDate(fromdate, todate);
            }
            else
            {
                model.DebitNoteList = _DebitNoteService.GetRowsByFromAndToDateAndSupplier(fromdate, todate, supplier);
            }
            return View(model);
        }

        //************************************** DAILY MONTHLY SALES REPORT ***************************************
        
        [HttpGet]
        public ActionResult StartingBlankPageForSalesReport()
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
        public ActionResult SelectDateForDailyMontlySalesReporPopup()
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
        public ActionResult RetailBillSalesReport(string todate, string fromdate)
        {
            string useremail = UserEmail;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(useremail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            DateTime ToDate = Convert.ToDateTime(todate);
            DateTime FromDate = Convert.ToDateTime(fromdate);
            int DayInterval = 1;

            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(FromDate);
            while (FromDate.AddDays(DayInterval) <= ToDate)
            {
                FromDate = FromDate.AddDays(DayInterval);
                dateList.Add(FromDate);
            }

            Session["FromDate"] = fromdate;
            Session["ToDate"] = todate;
            Session["SalesDateList"] = dateList;
            return View(model);
        }

        [HttpGet]
        public JsonResult RetailBillSalesCalc(string billdate)
        {
            var RetailBillDate = DateTime.ParseExact(billdate, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            DateTime BillDate = Convert.ToDateTime(RetailBillDate).Date;

            double TotalTaxFreeGoodsVal = 0;
            double TaxFreeGoodsVal = 0;
            double TaxFreeGoodsValWitSellQty = 0;
            double CNTaxFreeGoodsVal = 0;
            double CNTaxFreeGoodsValWitSellQty = 0;

            double TotalOnePerGoodsVal = 0;
            double OnePerGoodsVal = 0;
            double OnePerGoodsValWithSellQty = 0;
            double TotalOnePerGoodsValTaxAmt = 0;
            double OnePerGoodsValTaxAmt = 0;
            double CNOnePerGoodsVal = 0;
            double CNOnePerGoodsValWithSellQty = 0;
            double CNOnePerGoodsValTaxAmt = 0;

            double TotalFivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsValWithSellQty = 0;
            double TotalFivePointFivePerGoodsValTaxAmt = 0;
            double FivePointFivePerGoodsValTaxAmt = 0;
            double CNFivePointFivePerGoodsVal = 0;
            double CNFivePointFivePerGoodsValWithSellQty = 0;
            double CNFivePointFivePerGoodsValTaxAmt = 0;

            double TotalTwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsValWithSellQty = 0;
            double TotalTwelvePointFivePerGoodsValTaxAmt = 0;
            double TwelvePointFivePerGoodsValTaxAmt = 0;
            double CNTwelvePointFivePerGoodsVal = 0;
            double CNTwelvePointFivePerGoodsValWithSellQty = 0;
            double CNTwelvePointFivePerGoodsValTaxAmt = 0;

            var RBDataTaxFreeGoodsVal = _RetailBillItemService.GetTaxFreeItemsByDate(BillDate);
            foreach (var data in RBDataTaxFreeGoodsVal)
            {
                TaxFreeGoodsValWitSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TaxFreeGoodsVal = TaxFreeGoodsVal + TaxFreeGoodsValWitSellQty;
            }

            var CNDataTaxFreeGoodsVal = _RetailBillCreditNoteItemService.GetTaxFreeItemsByDate(BillDate);
            foreach (var data in CNDataTaxFreeGoodsVal)
            {
                CNTaxFreeGoodsValWitSellQty = Convert.ToDouble(data.Amount);
                CNTaxFreeGoodsVal = CNTaxFreeGoodsVal + CNTaxFreeGoodsValWitSellQty;
            }

            TotalTaxFreeGoodsVal = TaxFreeGoodsVal - CNTaxFreeGoodsVal;


            var RBDataOnePerGoodsVal = _RetailBillItemService.GetOnePerTaxItemsByDate(BillDate);
            foreach (var data in RBDataOnePerGoodsVal)
            {
                OnePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                OnePerGoodsVal = OnePerGoodsVal + OnePerGoodsValWithSellQty;
                OnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            var CNDataOnePerGoodsVal = _RetailBillCreditNoteItemService.GetOnePerTaxItemsByDate(BillDate);
            foreach (var data in CNDataOnePerGoodsVal)
            {
                CNOnePerGoodsValWithSellQty = Convert.ToDouble(data.Amount);
                CNOnePerGoodsVal = CNOnePerGoodsVal + CNOnePerGoodsValWithSellQty;
                CNOnePerGoodsValTaxAmt = CNOnePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            TotalOnePerGoodsVal = OnePerGoodsVal - CNOnePerGoodsVal;
            TotalOnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt - CNOnePerGoodsValTaxAmt;


            var RBDataFivePointFivePerGoodsVal = _RetailBillItemService.GetFivePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in RBDataFivePointFivePerGoodsVal)
            {
                FivePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                FivePointFivePerGoodsVal = FivePointFivePerGoodsVal + FivePointFivePerGoodsValWithSellQty;
                FivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            var CNDataFivePointFivePerGoodsVal = _RetailBillCreditNoteItemService.GetFivePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in CNDataFivePointFivePerGoodsVal)
            {
                CNFivePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.Amount);
                CNFivePointFivePerGoodsVal = CNFivePointFivePerGoodsVal + CNFivePointFivePerGoodsValWithSellQty;
                CNFivePointFivePerGoodsValTaxAmt = CNFivePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            TotalFivePointFivePerGoodsVal = FivePointFivePerGoodsVal - CNFivePointFivePerGoodsVal;
            TotalFivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt - CNFivePointFivePerGoodsValTaxAmt;


            var RBDataTwelvePointFivePerGoodsVal = _RetailBillItemService.GetTwelvePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in RBDataTwelvePointFivePerGoodsVal)
            {
                TwelvePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal + TwelvePointFivePerGoodsValWithSellQty;
                TwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            var CNDataTwelvePointFivePerGoodsVal = _RetailBillCreditNoteItemService.GetTwelvePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in CNDataTwelvePointFivePerGoodsVal)
            {
                CNTwelvePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.Amount);
                CNTwelvePointFivePerGoodsVal = CNTwelvePointFivePerGoodsVal + CNTwelvePointFivePerGoodsValWithSellQty;
                CNTwelvePointFivePerGoodsValTaxAmt = CNTwelvePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            TotalTwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal - CNTwelvePointFivePerGoodsVal;
            TotalTwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt - CNTwelvePointFivePerGoodsValTaxAmt;

            return Json(new { TotalTaxFreeGoodsVal, TotalOnePerGoodsVal, TotalOnePerGoodsValTaxAmt, TotalFivePointFivePerGoodsVal, TotalFivePointFivePerGoodsValTaxAmt, TotalTwelvePointFivePerGoodsVal, TotalTwelvePointFivePerGoodsValTaxAmt }, JsonRequestBehavior.AllowGet);
        }

        //FOR CREATE EXCEL SHEET FOR RETAIL BILL SALES REPORT
        [HttpPost]
        public ActionResult RetailBillSalesReport(FormCollection frmcol)
        {
            var fromdate = frmcol["RBFromDate"];
            var todate = frmcol["RBToDate"];

            DateTime ToDate = Convert.ToDateTime(todate);
            DateTime FromDate = Convert.ToDateTime(fromdate);
            int DayInterval = 1;

            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(FromDate);
            while (FromDate.AddDays(DayInterval) <= ToDate)
            {
                FromDate = FromDate.AddDays(DayInterval);
                dateList.Add(FromDate);
            }

            List<string> BillDatesList = new List<string>();
            List<string> TaxFreeGoodsList = new List<string>();
            List<string> OnePerGoodsValList = new List<string>();
            List<string> OnePerVATValList = new List<string>();
            List<string> FivePerGoodsValList = new List<string>();
            List<string> FivePerVATValList = new List<string>();
            List<string> TwelvePerGoodsValList = new List<string>();
            List<string> TwelvePerVATValList = new List<string>();
            List<string> NetAmtValList = new List<string>();

            var rowscount = dateList.Count();
            for (int i = 1; i <= rowscount; i++)
            {
                var billdateval = "BillDateVal" + i;
                var taxfreegoodsval = "TaxFreeGoodsVal" + i;
                var onepergoodsval = "OnePerGoodsVal" + i;
                var onepervatval = "OnePerVATVal" + i;
                var fivepergoodsval = "FivePerGoodsVal" + i;
                var fivepervatval = "FivePerVATVal" + i;
                var twelevepergoodsval = "TwelvePerGoodsVal" + i;
                var twelevepervatval = "TwelvePerVATVal" + i;
                var netamtval = "NetAmtVal" + i;

                var billdate = frmcol[billdateval];
                BillDatesList.Add(billdate);

                var taxfreegoods = frmcol[taxfreegoodsval];
                TaxFreeGoodsList.Add(taxfreegoods);

                var onepergood = frmcol[onepergoodsval];
                OnePerGoodsValList.Add(onepergood);

                var onepervat = frmcol[onepervatval];
                OnePerVATValList.Add(onepervat);

                var fivepergoods = frmcol[fivepergoodsval];
                FivePerGoodsValList.Add(fivepergoods);

                var fivepervat = frmcol[fivepervatval];
                FivePerVATValList.Add(fivepervat);

                var twelevepergoods = frmcol[twelevepergoodsval];
                TwelvePerGoodsValList.Add(twelevepergoods);

                var twelevepervat = frmcol[twelevepervatval];
                TwelvePerVATValList.Add(twelevepervat);

                var netamt = frmcol[netamtval];
                NetAmtValList.Add(netamt);
                
            }

            var ttlbilldateval = frmcol["ttlbilldate"];
            BillDatesList.Add(ttlbilldateval);
            var ttltaxfreegoodsval=frmcol["ttltaxfreegoodsval"];
            TaxFreeGoodsList.Add(ttltaxfreegoodsval);
            var ttlonepergoodsval=frmcol["ttlonepergoodsval"];
            OnePerGoodsValList.Add(ttlonepergoodsval);
            var ttlonepervatval=frmcol["ttlonepervatval"];
            OnePerVATValList.Add(ttlonepervatval);
            var ttlfivepergoodsval=frmcol["ttlfivepergoodsval"];
            FivePerGoodsValList.Add(ttlfivepergoodsval);
            var ttlfivepervatval=frmcol["ttlfivepervatval"];
            FivePerVATValList.Add(ttlfivepervatval);
            var ttltwelevepergoodsval=frmcol["ttltwelevepergoodsval"];
            TwelvePerGoodsValList.Add(ttltwelevepergoodsval);
            var ttltwelevepervatval=frmcol["ttltwelevepervatval"];
            TwelvePerVATValList.Add(ttltwelevepervatval);
            var ttlnetamtval=frmcol["ttlnetamtval"];
            NetAmtValList.Add(ttlnetamtval);

            //this is code for5 export to excel
            //#region generate Excel Data
            //DirectoryInfo outputDir = new DirectoryInfo(@"D:\Shraddha\RetailApp\ATOZ\MvcRetailApp");
            DirectoryInfo outputDir = new DirectoryInfo(@"C:\A2ZFurnishings");
            if (!outputDir.Exists) throw new Exception("Output Directory doesn't exists");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\SalesReport(RetailBill).xlsx");
            // If any file exists in this directory having name 'Sample1.xlsx', then delete it
            if (newFile.Exists)
            {
                newFile.Delete(); // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\SalesReport(RetailBill).xlsx");
            }
            ExcelPackage xls = new ExcelPackage(newFile);
            ExcelWorksheet xlWorkSheet = xls.Workbook.Worksheets.Add("SalesReport");

            int count = 5;

            xlWorkSheet.Cells["A3:O3"].Merge = true;
            xlWorkSheet.Cells["A3:O3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            xlWorkSheet.Cells["A3:O3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            xlWorkSheet.Cells["A3:O3"].Style.WrapText = true;
            xlWorkSheet.Cells["A3:O3"].Style.Font.SetFromFont(new Font("Calibri", 24, FontStyle.Bold));
            xlWorkSheet.Cells["A3:O3"].Style.Font.Color.SetColor(Color.FromArgb(255, 0, 0));
            xlWorkSheet.Row(3).Height = 60;
            xlWorkSheet.Cells["A4:O4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            xlWorkSheet.Cells[3, 1].Value = "Sales Report (Retail Bill)";

            xlWorkSheet.Cells["5,1"].Worksheet.Column(1).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(2).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(3).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(4).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(5).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(6).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(7).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(8).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(9).Width = 22;
            xlWorkSheet.Cells["A4:O4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            xlWorkSheet.Cells["A4:O4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            xlWorkSheet.Cells["A4:O4"].Style.WrapText = true;
            xlWorkSheet.Cells["A4:O4"].Style.Font.SetFromFont(new Font("Calibri", 18, FontStyle.Bold));
            xlWorkSheet.Cells["A4:O4"].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
            xlWorkSheet.Row(4).Height = 70;
            xlWorkSheet.Cells["A4:O4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

            xlWorkSheet.Cells["A4:O4"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            xlWorkSheet.Cells["A4:O4"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            xlWorkSheet.Cells["A4:O4"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            xlWorkSheet.Cells["A4:O4"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

            xlWorkSheet.Cells[4, 1].Value = "Bill Date";
            xlWorkSheet.Cells[4, 2].Value = "Tax Free Goods Value";
            xlWorkSheet.Cells[4, 3].Value = "1% Goods Value";
            xlWorkSheet.Cells[4, 4].Value = " 1% VAT Amount";
            xlWorkSheet.Cells[4, 5].Value = " 5% Goods Value";
            xlWorkSheet.Cells[4, 6].Value = " 5% VAT Amount";
            xlWorkSheet.Cells[4, 7].Value = " 12.5% Goods Value";
            xlWorkSheet.Cells[4, 8].Value = " 12.5% VAT Amount";
            xlWorkSheet.Cells[4, 9].Value = " Net Amount";

            xlWorkSheet.Cells[4, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 100, 122));
            xlWorkSheet.Cells[4, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(18, 174, 196));
            xlWorkSheet.Cells[4, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 63, 138));
            xlWorkSheet.Cells[4, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(101, 189, 119));
            xlWorkSheet.Cells[4, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(233, 150, 122));
            xlWorkSheet.Cells[4, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 153, 153));
            xlWorkSheet.Cells[4, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(178, 102, 255));
            xlWorkSheet.Cells[4, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(235, 80, 85));
            xlWorkSheet.Cells[4, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 204, 0));

            for (int i = 0; i < rowscount+1 ; i++)
            {
                xlWorkSheet.Cells[count, 1].Value = BillDatesList[i];
                xlWorkSheet.Cells[count, 2].Value = TaxFreeGoodsList[i];
                xlWorkSheet.Cells[count, 3].Value = OnePerGoodsValList[i];
                xlWorkSheet.Cells[count, 4].Value = OnePerVATValList[i];
                xlWorkSheet.Cells[count, 5].Value = FivePerGoodsValList[i];
                xlWorkSheet.Cells[count, 6].Value = FivePerVATValList[i];
                xlWorkSheet.Cells[count, 7].Value = TwelvePerGoodsValList[i];
                xlWorkSheet.Cells[count, 8].Value = TwelvePerVATValList[i];
                xlWorkSheet.Cells[count, 9].Value = NetAmtValList[i];

                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                count++;
            }
            xls.Save();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment;filename=SalesReport(RetailBill).xlsx");
            Response.WriteFile(Server.MapPath("~/SalesReport(RetailBill).xlsx"));
            Response.End();
            return RedirectToAction("StartingBlankPageForSalesReport");
        }

        [HttpGet]
        public ActionResult SalesBillSalesReport(string todate, string fromdate)
        {
            string useremail = UserEmail;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(useremail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            DateTime ToDate = Convert.ToDateTime(todate);
            DateTime FromDate = Convert.ToDateTime(fromdate);
            int DayInterval = 1;

            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(FromDate);
            while (FromDate.AddDays(DayInterval) <= ToDate)
            {
                FromDate = FromDate.AddDays(DayInterval);
                dateList.Add(FromDate);
            }

            Session["FromDate"] = fromdate;
            Session["ToDate"] = todate;
            Session["SalesDateList"] = dateList;
            return View(model);
        }

        [HttpGet]
        public JsonResult SalesBillSalesCalc(string billdate)
        {
            var SalesBillDate = DateTime.ParseExact(billdate, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            DateTime BillDate = Convert.ToDateTime(SalesBillDate).Date;

            double TotalTaxFreeGoodsVal = 0;
            double TaxFreeGoodsVal = 0;
            double TaxFreeGoodsValWitSellQty = 0;
            double CNTaxFreeGoodsVal = 0;
            double CNTaxFreeGoodsValWitSellQty = 0;

            double TotalOnePerGoodsVal = 0;
            double OnePerGoodsVal = 0;
            double OnePerGoodsValWithSellQty = 0;
            double TotalOnePerGoodsValTaxAmt = 0;
            double OnePerGoodsValTaxAmt = 0;
            double CNOnePerGoodsVal = 0;
            double CNOnePerGoodsValWithSellQty = 0;
            double CNOnePerGoodsValTaxAmt = 0;

            double TotalFivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsValWithSellQty = 0;
            double TotalFivePointFivePerGoodsValTaxAmt = 0;
            double FivePointFivePerGoodsValTaxAmt = 0;
            double CNFivePointFivePerGoodsVal = 0;
            double CNFivePointFivePerGoodsValWithSellQty = 0;
            double CNFivePointFivePerGoodsValTaxAmt = 0;

            double TotalTwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsValWithSellQty = 0;
            double TotalTwelvePointFivePerGoodsValTaxAmt = 0;
            double TwelvePointFivePerGoodsValTaxAmt = 0;
            double CNTwelvePointFivePerGoodsVal = 0;
            double CNTwelvePointFivePerGoodsValWithSellQty = 0;
            double CNTwelvePointFivePerGoodsValTaxAmt = 0;

            var SBDataTaxFreeGoodsVal = _SalesBillItemService.GetTaxFreeItemsByDate(BillDate);
            foreach (var data in SBDataTaxFreeGoodsVal)
            {
                TaxFreeGoodsValWitSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TaxFreeGoodsVal = TaxFreeGoodsVal + TaxFreeGoodsValWitSellQty;
            }

            var CNDataTaxFreeGoodsVal = _SalesBillCreditNoteItemService.GetTaxFreeItemsByDate(BillDate);
            foreach (var data in CNDataTaxFreeGoodsVal)
            {
                CNTaxFreeGoodsValWitSellQty = Convert.ToDouble(data.Amount);
                CNTaxFreeGoodsVal = CNTaxFreeGoodsVal + CNTaxFreeGoodsValWitSellQty;
            }

            TotalTaxFreeGoodsVal = TaxFreeGoodsVal - CNTaxFreeGoodsVal;

            var SBDataOnePerGoodsVal = _SalesBillItemService.GetOnePerTaxItemsByDate(BillDate);
            foreach (var data in SBDataOnePerGoodsVal)
            {
                OnePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                OnePerGoodsVal = OnePerGoodsVal + OnePerGoodsValWithSellQty;
                OnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            var CNDataOnePerGoodsVal = _SalesBillCreditNoteItemService.GetOnePerTaxItemsByDate(BillDate);
            foreach (var data in CNDataOnePerGoodsVal)
            {
                CNOnePerGoodsValWithSellQty = Convert.ToDouble(data.Amount);
                CNOnePerGoodsVal = CNOnePerGoodsVal + CNOnePerGoodsValWithSellQty;
                CNOnePerGoodsValTaxAmt = CNOnePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            TotalOnePerGoodsVal = OnePerGoodsVal - CNOnePerGoodsVal;
            TotalOnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt - CNOnePerGoodsValTaxAmt;

            

            var SBDataFivePointFivePerGoodsVal = _SalesBillItemService.GetFivePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in SBDataFivePointFivePerGoodsVal)
            {
                FivePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                FivePointFivePerGoodsVal = FivePointFivePerGoodsVal + FivePointFivePerGoodsValWithSellQty;
                FivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            var CNDataFivePointFivePerGoodsVal = _SalesBillCreditNoteItemService.GetFivePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in CNDataFivePointFivePerGoodsVal)
            {
                CNFivePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.Amount);
                CNFivePointFivePerGoodsVal = CNFivePointFivePerGoodsVal + CNFivePointFivePerGoodsValWithSellQty;
                CNFivePointFivePerGoodsValTaxAmt = CNFivePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            TotalFivePointFivePerGoodsVal = FivePointFivePerGoodsVal - CNFivePointFivePerGoodsVal;
            TotalFivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt - CNFivePointFivePerGoodsValTaxAmt;


            var SBDataTwelvePointFivePerGoodsVal = _SalesBillItemService.GetTwelvePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in SBDataTwelvePointFivePerGoodsVal)
            {
                TwelvePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal + TwelvePointFivePerGoodsValWithSellQty;
                TwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            var CNDataTwelvePointFivePerGoodsVal = _SalesBillCreditNoteItemService.GetTwelvePointFivePerTaxItemsByDate(BillDate);
            foreach (var data in CNDataTwelvePointFivePerGoodsVal)
            {
                CNTwelvePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.Amount);
                CNTwelvePointFivePerGoodsVal = CNTwelvePointFivePerGoodsVal + CNTwelvePointFivePerGoodsValWithSellQty;
                CNTwelvePointFivePerGoodsValTaxAmt = CNTwelvePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }

            TotalTwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal - CNTwelvePointFivePerGoodsVal;
            TotalTwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt - CNTwelvePointFivePerGoodsValTaxAmt;

            return Json(new { TotalTaxFreeGoodsVal, TotalOnePerGoodsVal, TotalOnePerGoodsValTaxAmt, TotalFivePointFivePerGoodsVal, TotalFivePointFivePerGoodsValTaxAmt, TotalTwelvePointFivePerGoodsVal, TotalTwelvePointFivePerGoodsValTaxAmt }, JsonRequestBehavior.AllowGet);
        }

        //FOR CREATE EXCEL SHEET FOR SALES BILL SALES REPORT
        [HttpPost]
        public ActionResult SalesBillSalesReport(FormCollection frmcol)
        {
            var fromdate = frmcol["SBFromDate"];
            var todate = frmcol["SBToDate"];

            DateTime ToDate = Convert.ToDateTime(todate);
            DateTime FromDate = Convert.ToDateTime(fromdate);
            int DayInterval = 1;

            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(FromDate);
            while (FromDate.AddDays(DayInterval) <= ToDate)
            {
                FromDate = FromDate.AddDays(DayInterval);
                dateList.Add(FromDate);
            }

            List<string> BillDatesList = new List<string>();
            List<string> TaxFreeGoodsList = new List<string>();
            List<string> OnePerGoodsValList = new List<string>();
            List<string> OnePerVATValList = new List<string>();
            List<string> FivePerGoodsValList = new List<string>();
            List<string> FivePerVATValList = new List<string>();
            List<string> TwelvePerGoodsValList = new List<string>();
            List<string> TwelvePerVATValList = new List<string>();
            List<string> NetAmtValList = new List<string>();

            var rowscount = dateList.Count();
            for (int i = 1; i <= rowscount; i++)
            {
                var billdateval = "BillDateVal" + i;
                var taxfreegoodsval = "TaxFreeGoodsVal" + i;
                var onepergoodsval = "OnePerGoodsVal" + i;
                var onepervatval = "OnePerVATVal" + i;
                var fivepergoodsval = "FivePerGoodsVal" + i;
                var fivepervatval = "FivePerVATVal" + i;
                var twelevepergoodsval = "TwelvePerGoodsVal" + i;
                var twelevepervatval = "TwelvePerVATVal" + i;
                var netamtval = "NetAmtVal" + i;

                var billdate = frmcol[billdateval];
                BillDatesList.Add(billdate);

                var taxfreegoods = frmcol[taxfreegoodsval];
                TaxFreeGoodsList.Add(taxfreegoods);

                var onepergood = frmcol[onepergoodsval];
                OnePerGoodsValList.Add(onepergood);

                var onepervat = frmcol[onepervatval];
                OnePerVATValList.Add(onepervat);

                var fivepergoods = frmcol[fivepergoodsval];
                FivePerGoodsValList.Add(fivepergoods);

                var fivepervat = frmcol[fivepervatval];
                FivePerVATValList.Add(fivepervat);

                var twelevepergoods = frmcol[twelevepergoodsval];
                TwelvePerGoodsValList.Add(twelevepergoods);

                var twelevepervat = frmcol[twelevepervatval];
                TwelvePerVATValList.Add(twelevepervat);

                var netamt = frmcol[netamtval];
                NetAmtValList.Add(netamt);

            }

            var ttlbilldateval = frmcol["ttlbilldate"];
            BillDatesList.Add(ttlbilldateval);
            var ttltaxfreegoodsval = frmcol["ttltaxfreegoodsval"];
            TaxFreeGoodsList.Add(ttltaxfreegoodsval);
            var ttlonepergoodsval = frmcol["ttlonepergoodsval"];
            OnePerGoodsValList.Add(ttlonepergoodsval);
            var ttlonepervatval = frmcol["ttlonepervatval"];
            OnePerVATValList.Add(ttlonepervatval);
            var ttlfivepergoodsval = frmcol["ttlfivepergoodsval"];
            FivePerGoodsValList.Add(ttlfivepergoodsval);
            var ttlfivepervatval = frmcol["ttlfivepervatval"];
            FivePerVATValList.Add(ttlfivepervatval);
            var ttltwelevepergoodsval = frmcol["ttltwelevepergoodsval"];
            TwelvePerGoodsValList.Add(ttltwelevepergoodsval);
            var ttltwelevepervatval = frmcol["ttltwelevepervatval"];
            TwelvePerVATValList.Add(ttltwelevepervatval);
            var ttlnetamtval = frmcol["ttlnetamtval"];
            NetAmtValList.Add(ttlnetamtval);

            //this is code for5 export to excel
            //#region generate Excel Data
            DirectoryInfo outputDir = new DirectoryInfo(@"D:\Shraddha\RetailApp\ATOZ\MvcRetailApp");
            if (!outputDir.Exists) throw new Exception("Output Directory doesn't exists");
            FileInfo newFile = new FileInfo(outputDir.FullName + @"\SalesReport(SalesBill).xlsx");
            // If any file exists in this directory having name 'Sample1.xlsx', then delete it
            if (newFile.Exists)
            {
                newFile.Delete(); // ensures we create a new workbook
                newFile = new FileInfo(outputDir.FullName + @"\SalesReport(SalesBill).xlsx");
            }
            ExcelPackage xls = new ExcelPackage(newFile);
            ExcelWorksheet xlWorkSheet = xls.Workbook.Worksheets.Add("SalesReport");

            int count = 5;

            xlWorkSheet.Cells["A3:O3"].Merge = true;
            xlWorkSheet.Cells["A3:O3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            xlWorkSheet.Cells["A3:O3"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            xlWorkSheet.Cells["A3:O3"].Style.WrapText = true;
            xlWorkSheet.Cells["A3:O3"].Style.Font.SetFromFont(new Font("Calibri", 24, FontStyle.Bold));
            xlWorkSheet.Cells["A3:O3"].Style.Font.Color.SetColor(Color.FromArgb(255, 0, 0));
            xlWorkSheet.Row(3).Height = 60;
            xlWorkSheet.Cells["A4:O4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            xlWorkSheet.Cells[3, 1].Value = "Sales Report (Sales Bill)";

            xlWorkSheet.Cells["5,1"].Worksheet.Column(1).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(2).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(3).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(4).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(5).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(6).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(7).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(8).Width = 22;
            xlWorkSheet.Cells["5,1"].Worksheet.Column(9).Width = 22;
            xlWorkSheet.Cells["A4:O4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            xlWorkSheet.Cells["A4:O4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            xlWorkSheet.Cells["A4:O4"].Style.WrapText = true;
            xlWorkSheet.Cells["A4:O4"].Style.Font.SetFromFont(new Font("Calibri", 18, FontStyle.Bold));
            xlWorkSheet.Cells["A4:O4"].Style.Font.Color.SetColor(Color.FromArgb(255, 255, 255));
            xlWorkSheet.Row(4).Height = 70;
            xlWorkSheet.Cells["A4:O4"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

            xlWorkSheet.Cells["A4:O4"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            xlWorkSheet.Cells["A4:O4"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            xlWorkSheet.Cells["A4:O4"].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            xlWorkSheet.Cells["A4:O4"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

            xlWorkSheet.Cells[4, 1].Value = "Bill Date";
            xlWorkSheet.Cells[4, 2].Value = "Tax Free Goods Value";
            xlWorkSheet.Cells[4, 3].Value = "1% Goods Value";
            xlWorkSheet.Cells[4, 4].Value = " 1% VAT Amount";
            xlWorkSheet.Cells[4, 5].Value = " 5% Goods Value";
            xlWorkSheet.Cells[4, 6].Value = " 5% VAT Amount";
            xlWorkSheet.Cells[4, 7].Value = " 12.5% Goods Value";
            xlWorkSheet.Cells[4, 8].Value = " 12.5% VAT Amount";
            xlWorkSheet.Cells[4, 9].Value = " Net Amount";

            xlWorkSheet.Cells[4, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 100, 122));
            xlWorkSheet.Cells[4, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(18, 174, 196));
            xlWorkSheet.Cells[4, 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 63, 138));
            xlWorkSheet.Cells[4, 4].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(101, 189, 119));
            xlWorkSheet.Cells[4, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(233, 150, 122));
            xlWorkSheet.Cells[4, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 153, 153));
            xlWorkSheet.Cells[4, 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(178, 102, 255));
            xlWorkSheet.Cells[4, 8].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(235, 80, 85));
            xlWorkSheet.Cells[4, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 204, 0));
            
            for (int i = 0; i < rowscount + 1; i++)
            {
                xlWorkSheet.Cells[count, 1].Value = BillDatesList[i];
                xlWorkSheet.Cells[count, 2].Value = TaxFreeGoodsList[i];
                xlWorkSheet.Cells[count, 3].Value = OnePerGoodsValList[i];
                xlWorkSheet.Cells[count, 4].Value = OnePerVATValList[i];
                xlWorkSheet.Cells[count, 5].Value = FivePerGoodsValList[i];
                xlWorkSheet.Cells[count, 6].Value = FivePerVATValList[i];
                xlWorkSheet.Cells[count, 7].Value = TwelvePerGoodsValList[i];
                xlWorkSheet.Cells[count, 8].Value = TwelvePerVATValList[i];
                xlWorkSheet.Cells[count, 9].Value = NetAmtValList[i];

                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                xlWorkSheet.Cells["A" + count + ":O" + count].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                count++;
            }
            xls.Save();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AppendHeader("Content-Disposition", "attachment;filename=SalesReport(SalesBill).xlsx");
            Response.WriteFile(Server.MapPath("~/SalesReport(SalesBill).xlsx"));
            Response.End();
            return RedirectToAction("StartingBlankPageForSalesReport");

        }

        //********************************* RETAIL BILL DAILY SALES REPORT ***************************************
        [HttpGet]
        public ActionResult GetRBDailySalesReport(string Date)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            var SelectedDate = DateTime.ParseExact(Date, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            DateTime ReportDate = Convert.ToDateTime(SelectedDate);
            model.RetailBillList = _RetailInvoiceMasterService.GetBillsByDate(ReportDate);
            Session["SelectedDate"] = SelectedDate;
            return View(model);
        }

        [HttpGet]
        public JsonResult RBDailySalesCalc(string billno)
        {
            double TotalTaxFreeGoodsVal = 0;
            double TaxFreeGoodsVal = 0;
            double TaxFreeGoodsValWitSellQty = 0;

            double TotalOnePerGoodsVal = 0;
            double OnePerGoodsVal = 0;
            double OnePerGoodsValWithSellQty = 0;
            double TotalOnePerGoodsValTaxAmt = 0;
            double OnePerGoodsValTaxAmt = 0;

            double TotalFivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsValWithSellQty = 0;
            double TotalFivePointFivePerGoodsValTaxAmt = 0;
            double FivePointFivePerGoodsValTaxAmt = 0;

            double TotalTwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsValWithSellQty = 0;
            double TotalTwelvePointFivePerGoodsValTaxAmt = 0;
            double TwelvePointFivePerGoodsValTaxAmt = 0;

            double TotalReportDateCashSales = 0;
            double TotalRBCreditNoteAmount = 0;

            var RBDataTaxFreeGoodsVal = _RetailBillItemService.GetTaxFreeItemsByBillNo(billno);
            foreach (var data in RBDataTaxFreeGoodsVal)
            {
                TaxFreeGoodsValWitSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TaxFreeGoodsVal = TaxFreeGoodsVal + TaxFreeGoodsValWitSellQty;
            }
            TotalTaxFreeGoodsVal = TaxFreeGoodsVal;


            var RBDataOnePerGoodsVal = _RetailBillItemService.GetOnePerTaxItemsByBillNo(billno);
            foreach (var data in RBDataOnePerGoodsVal)
            {
                OnePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                OnePerGoodsVal = OnePerGoodsVal + OnePerGoodsValWithSellQty;
                OnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }
            TotalOnePerGoodsVal = OnePerGoodsVal;
            TotalOnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt;


            var RBDataFivePointFivePerGoodsVal = _RetailBillItemService.GetFivePointFivePerTaxItemsByBillNo(billno);
            foreach (var data in RBDataFivePointFivePerGoodsVal)
            {
                FivePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                FivePointFivePerGoodsVal = FivePointFivePerGoodsVal + FivePointFivePerGoodsValWithSellQty;
                FivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }
            TotalFivePointFivePerGoodsVal = FivePointFivePerGoodsVal;
            TotalFivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt;


            var RBDataTwelvePointFivePerGoodsVal = _RetailBillItemService.GetTwelvePointFivePerTaxItemsByBillNo(billno);
            foreach (var data in RBDataTwelvePointFivePerGoodsVal)
            {
                TwelvePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal + TwelvePointFivePerGoodsValWithSellQty;
                TwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }
            TotalTwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal;
            TotalTwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt;

            DateTime SelectedDate = Convert.ToDateTime(Session["SelectedDate"]);
            var TotalReportDateCashSalesDetails = _RetailInvoiceMasterService.GetBillsByDate(SelectedDate);
            foreach (var data in TotalReportDateCashSalesDetails)
            {
                TotalReportDateCashSales = TotalReportDateCashSales + Convert.ToDouble(data.GrandTotal);
            }

            var RBCreditNoteAmount = _RetailBillCreditNoteService.GetBillsByDate(SelectedDate);
            foreach (var data in RBCreditNoteAmount)
            {
                TotalRBCreditNoteAmount = TotalRBCreditNoteAmount + Convert.ToDouble(data.Amount);
            }

            return Json(new { TotalTaxFreeGoodsVal, TotalOnePerGoodsVal, TotalOnePerGoodsValTaxAmt, TotalFivePointFivePerGoodsVal, TotalFivePointFivePerGoodsValTaxAmt, TotalTwelvePointFivePerGoodsVal, TotalTwelvePointFivePerGoodsValTaxAmt, TotalReportDateCashSales, TotalRBCreditNoteAmount }, JsonRequestBehavior.AllowGet);
        }

        //********************************* SALES BILL DAILY SALES REPORT ***************************************

        [HttpGet]
        public ActionResult GetSBDailySalesReport(string Date)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            var SelectedDate = DateTime.ParseExact(Date, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");
            DateTime ReportDate = Convert.ToDateTime(SelectedDate);
            model.SalesBillList = _SalesBillService.GetBillsByDate(ReportDate);
            Session["SelectedDate"] = SelectedDate;
            return View(model);
        }

        [HttpGet]
        public JsonResult SBDailySalesCalc(string billno)
        {
            double TotalTaxFreeGoodsVal = 0;
            double TaxFreeGoodsVal = 0;
            double TaxFreeGoodsValWitSellQty = 0;

            double TotalOnePerGoodsVal = 0;
            double OnePerGoodsVal = 0;
            double OnePerGoodsValWithSellQty = 0;
            double TotalOnePerGoodsValTaxAmt = 0;
            double OnePerGoodsValTaxAmt = 0;

            double TotalFivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsVal = 0;
            double FivePointFivePerGoodsValWithSellQty = 0;
            double TotalFivePointFivePerGoodsValTaxAmt = 0;
            double FivePointFivePerGoodsValTaxAmt = 0;

            double TotalTwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsVal = 0;
            double TwelvePointFivePerGoodsValWithSellQty = 0;
            double TotalTwelvePointFivePerGoodsValTaxAmt = 0;
            double TwelvePointFivePerGoodsValTaxAmt = 0;

            double TotalReportDateCashSales = 0;
            double TotalSBCreditNoteAmount = 0;

            var SBDataTaxFreeGoodsVal = _SalesBillItemService.GetTaxFreeItemsByBillNo(billno);
            foreach (var data in SBDataTaxFreeGoodsVal)
            {
                TaxFreeGoodsValWitSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TaxFreeGoodsVal = TaxFreeGoodsVal + TaxFreeGoodsValWitSellQty;
            }
            TotalTaxFreeGoodsVal = TaxFreeGoodsVal;


            var SBDataOnePerGoodsVal = _SalesBillItemService.GetOnePerTaxItemsByBillNo(billno);
            foreach (var data in SBDataOnePerGoodsVal)
            {
                OnePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                OnePerGoodsVal = OnePerGoodsVal + OnePerGoodsValWithSellQty;
                OnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }
            TotalOnePerGoodsVal = OnePerGoodsVal;
            TotalOnePerGoodsValTaxAmt = OnePerGoodsValTaxAmt;


            var SBDataFivePointFivePerGoodsVal = _SalesBillItemService.GetFivePointFivePerTaxItemsByBillNo(billno);
            foreach (var data in SBDataFivePointFivePerGoodsVal)
            {
                FivePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                FivePointFivePerGoodsVal = FivePointFivePerGoodsVal + FivePointFivePerGoodsValWithSellQty;
                FivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }
            TotalFivePointFivePerGoodsVal = FivePointFivePerGoodsVal;
            TotalFivePointFivePerGoodsValTaxAmt = FivePointFivePerGoodsValTaxAmt;


            var SBDataTwelvePointFivePerGoodsVal = _SalesBillItemService.GetTwelvePointFivePerTaxItemsByBillNo(billno);
            foreach (var data in SBDataTwelvePointFivePerGoodsVal)
            {
                TwelvePointFivePerGoodsValWithSellQty = Convert.ToDouble(data.SellingPrice) * Convert.ToDouble(data.Quantity);
                TwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal + TwelvePointFivePerGoodsValWithSellQty;
                TwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt + Convert.ToDouble(data.ItemTaxAmt);
            }
            TotalTwelvePointFivePerGoodsVal = TwelvePointFivePerGoodsVal;
            TotalTwelvePointFivePerGoodsValTaxAmt = TwelvePointFivePerGoodsValTaxAmt;

            DateTime SelectedDate = Convert.ToDateTime(Session["SelectedDate"]);
            var TotalReportDateCashSalesDetails = _SalesBillService.GetBillsByDate(SelectedDate);
            foreach (var data in TotalReportDateCashSalesDetails)
            {
                TotalReportDateCashSales = TotalReportDateCashSales + Convert.ToDouble(data.GrandTotal);
            }

            var SBCreditNoteAmount = _SalesBillCreditNoteService.GetBillsByDate(SelectedDate);
            foreach (var data in SBCreditNoteAmount)
            {
                TotalSBCreditNoteAmount = TotalSBCreditNoteAmount + Convert.ToDouble(data.Amount);
            }

            return Json(new { TotalTaxFreeGoodsVal, TotalOnePerGoodsVal, TotalOnePerGoodsValTaxAmt, TotalFivePointFivePerGoodsVal, TotalFivePointFivePerGoodsValTaxAmt, TotalTwelvePointFivePerGoodsVal, TotalTwelvePointFivePerGoodsValTaxAmt, TotalReportDateCashSales, TotalSBCreditNoteAmount }, JsonRequestBehavior.AllowGet);
        }


        //********************************* PENDING BILL REPORT ***************************************
        [HttpGet]
        public ActionResult PendingBillReports(string quotno)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            model.QuotationList = _QuotationService.GetActiveQuotations();
            TempData["Quotation"] = model.QuotationList;
            model.SalesOrderList = _SalesOrderService.GetAllActiveSalesOrder();
            TempData["SalesOrderList"] = model.SalesOrderList;
            model.DeliveryChallanList = _DeliveryChallanSevice.GetChallanNoByStatus();
            TempData["DeliveryChallanList"] = model.DeliveryChallanList;
            model.SalesBillList = _SalesBillService.GetAllActiveSalesBill();
            TempData["SalesBillList"] = model.SalesBillList;
            model.RetailBillList = _RetailInvoiceMasterService.GetAllActiveRetailBill();
            TempData["RetailBillList"] = model.RetailBillList;
            model.TemporaryCashMemoList = _TemporaryCashMemoService.GetAllActiveTemporaryCashMemo();
            TempData["TemporaryCashMemoList"] = model.TemporaryCashMemoList;
            return View(model);
        }

        //DISPLAY DETAILS OF QUOTATONS
        [HttpGet]
        public ActionResult GetPendingQuotationReports()
        {
            MainApplication model = new MainApplication();
            model.QuotationList = TempData["Quotation"] as IEnumerable<Quotation>;
            TempData["Quotation"] = model.QuotationList;
            return View(model);
        }

        [HttpGet]
        public JsonResult EncodeId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintPendingQuotations(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.QuotationDetails = _QuotationService.GetQuotById(Id);
            model.QuotationItemList = _QuotationItemService.GetDetailsByQuotNo(model.QuotationDetails.QuotNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPendingSalesReports()
        {
            MainApplication model = new MainApplication();
            model.SalesOrderList = TempData["SalesOrderList"] as IEnumerable<SalesOrder>;
            TempData["SalesOrderList"] = model.SalesOrderList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintPendingSalesOrders(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsBySONoAndStatus(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPendingDeliveryChallanReports()
        {
            MainApplication model = new MainApplication();
            model.DeliveryChallanList = TempData["DeliveryChallanList"] as IEnumerable<DeliveryChallan>;
            TempData["DeliveryChallanList"] = model.DeliveryChallanList;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintPendingDeliveryChallans(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanSevice.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPendingSalesBillReports()
        {
            MainApplication model = new MainApplication();
            model.SalesBillList = TempData["SalesBillList"] as IEnumerable<SalesBill>;
            TempData["SalesBillList"] = model.SalesBillList;
            return View(model);
        }
        [HttpGet]
        public ActionResult PrintPendingSalesBills(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.SalesBillDetails = _SalesBillService.GetDetailsById(Id);
            model.SalesBillItemList = _SalesBillItemService.GetActiveSalesBillNo(model.SalesBillDetails.SalesBillNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPendingRetailBillReports()
        {
            MainApplication model = new MainApplication();
            model.RetailBillList = TempData["RetailBillList"] as IEnumerable<RetailBill>;
            TempData["RetailBillList"] = model.RetailBillList;
            return View(model);
        }
        [HttpGet]
        public ActionResult PrintPendingRetailBills(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.RetailBillDetails = _RetailInvoiceMasterService.GetById(Id);
            model.RetailBillItemList = _RetailBillItemService.GetDetailsByInvoiceNoAndStatus(model.RetailBillDetails.RetailBillNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPendingTemporaryCashMemoReports()
        {
            MainApplication model = new MainApplication();
            model.TemporaryCashMemoList = TempData["TemporaryCashMemoList"] as IEnumerable<TemporaryCashMemo>;
            TempData["TemporaryCashMemoList"] = model.TemporaryCashMemoList;
            return View(model);

        }

        [HttpGet]
        public ActionResult PrintPendingTemporaryCashMemos(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.TemporaryCashMemoDetails = _TemporaryCashMemoService.GetById(Id);
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNoAndStatus(model.TemporaryCashMemoDetails.TempCashMemoNo);
            return View(model);
        }

    }
}

