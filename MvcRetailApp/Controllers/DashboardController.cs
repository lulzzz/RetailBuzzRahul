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
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class DashboardController : Controller
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
        private readonly IPurchaseOrderDetailService _IPurchaseOrderDetailService;
        private readonly IOutwardToShopService _OutwardToShopService;
        private readonly IOutwardShopToGodownService _OutwardShopToGodownService;
        private readonly IOutwardInterGodownService _OutwardInterGodownService;
        private readonly IOutwardInterShopService _OutwardInterShopService;
        private readonly IOutwardToClientService _IOutwardToClientService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IShopStockService _IShopStockService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IStockItemDistributionService _IStockItemDistributionService;
        private readonly IRetailBillService _IRetailInvoiceMasterService;
        private readonly ISalesBillService _ISalesBillService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IQuotationService _QuotationService;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly IDeliveryChallanService _DeliveryChallanService;
        private readonly IRequisitionForShopService _RequisitionForShopService;
        private readonly IRequisitionForGodownService _RequisitionForGodownService;
        private readonly IInwardFromGodownService _InwardFromGodownService;
        private readonly IInwardFromShopToGodownService _InwardFromShopToGodownService;
        private readonly IInwardInterGodownService _InwardInterGodownService;
        private readonly IInwardInterShopService _InwardInterShopService;
        private readonly ISalesReturnService _SalesReturnService;
        private readonly IPurchaseReturnService _PurchaseReturnService;
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly ICashierSalesOrderService _CashierSalesOrderService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierTemporaryCashMemoService _CashierTemporaryCashMemoService;
        private readonly ICashierSalesBillService _CashierSalesBillService;
        private readonly ICashierRefundOrderService _CashierRefundOrderService;
        private readonly ICashHandoverService _CashHandoverService;
        private readonly ICardChequeHandoverService _CardChequeHandoverService;
        private readonly IIncomeExpenseVoucherService _IncomeExchangeVoucherService;

        public DashboardController(IUtilityService utilityservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IPurchaseOrderDetailService IPurchaseOrderDetailService, IOutwardToShopService OutwardToShopService, IOutwardToClientService IOutwardToClientService, IShopStockService IShopStockService,
            IGodownStockService GodownStockService, IStockItemDistributionService IStockItemDistributionService, IRetailBillService IRetailInvoiceMasterService, ISalesBillService ISalesBillService, IEmployeeMasterService EmployeeMasterService, IEntryStockItemService EntryStockItemService,
            IQuotationService QuotationService, ISalesOrderService SalesOrderService, IDeliveryChallanService DeliveryChallanService, IInwardFromGodownService InwardFromGodownService, IInwardFromShopToGodownService InwardFromShopToGodownService, IInwardInterGodownService InwardInterGodownService, IInwardInterShopService InwardInterShopService,
            IRequisitionForShopService RequisitionForShopService, IRequisitionForGodownService RequisitionForGodownService, IOutwardShopToGodownService OutwardShopToGodownService, IOutwardInterGodownService OutwardInterGodownService, IOutwardInterShopService OutwardInterShopService, ISalesReturnService SalesReturnService,
            IPurchaseReturnService PurchaseReturnService, IInwardFromSupplierService InwardFromSupplierService, ICashierSalesOrderService CashierSalesOrderService, ICashierRetailBillService CashierRetailBillService, ICashierTemporaryCashMemoService CashierTemporaryCashMemoService, ICashierSalesBillService CashierSalesBillService,
            ICashierRefundOrderService CashierRefundOrderService, ICashHandoverService CashHandoverService, ICardChequeHandoverService CardChequeHandoverService, IIncomeExpenseVoucherService IncomeExchangeVoucherService)
        {
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._IPurchaseOrderDetailService = IPurchaseOrderDetailService;
            this._IOutwardToClientService = IOutwardToClientService;
            this._EntryStockItemService = EntryStockItemService;
            this._IShopStockService = IShopStockService;
            this._GodownStockService = GodownStockService;
            this._IStockItemDistributionService = IStockItemDistributionService;
            this._IRetailInvoiceMasterService = IRetailInvoiceMasterService;
            this._ISalesBillService = ISalesBillService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._QuotationService = QuotationService;
            this._SalesOrderService = SalesOrderService;
            this._DeliveryChallanService = DeliveryChallanService;
            this._RequisitionForShopService = RequisitionForShopService;
            this._RequisitionForGodownService = RequisitionForGodownService;
            this._OutwardToShopService = OutwardToShopService;
            this._OutwardShopToGodownService = OutwardShopToGodownService;
            this._OutwardInterGodownService = OutwardInterGodownService;
            this._OutwardInterShopService = OutwardInterShopService;
            this._InwardFromGodownService = InwardFromGodownService;
            this._InwardFromShopToGodownService = InwardFromShopToGodownService;
            this._InwardInterGodownService = InwardInterGodownService;
            this._InwardInterShopService = InwardInterShopService;
            this._SalesReturnService = SalesReturnService;
            this._PurchaseReturnService = PurchaseReturnService;
            this._InwardFromSupplierService = InwardFromSupplierService;
            this._CashierSalesOrderService = CashierSalesOrderService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierTemporaryCashMemoService = CashierTemporaryCashMemoService;
            this._CashierSalesBillService = CashierSalesBillService;
            this._CashierRefundOrderService = CashierRefundOrderService;
            this._CashHandoverService = CashHandoverService;
            this._CardChequeHandoverService = CardChequeHandoverService;
            this._IncomeExchangeVoucherService = IncomeExchangeVoucherService;
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

        //*****************    Dashboard View    *****************//
        [HttpGet]
        public ActionResult Dashboard()
        {
            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                var AssignRights = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
                if (AssignRights.Count() == 0)
                {
                    TempData["NotEnoughRights"] = "Yes";
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    int rightscount = 0;
                    for (int i = 96; i <= modulelastcount; i++)
                    {
                        var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                        if (assigndetails != null)
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                            break;
                        }
                        else
                        {
                            rightscount++;
                        }
                    }
                    if (modulelastcount - 96 == rightscount)
                    {
                        Session["LOGINSHOPGODOWNCODE"] = null;
                        Session["SHOPGODOWNNAME"] = null;
                    }
                }
            }

            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string loggedinshopname = Session["SHOPGODOWNNAME"] as string;
            TempData["PendingPoes"] = _IPurchaseOrderDetailService.GetPendingPo("Pending");
            TempData["Inward"] = _IPurchaseOrderDetailService.GetInwardDetailsByDate();
            TempData["OutwardToShop"] = _OutwardToShopService.GetDailyOutwardsToShop();
            TempData["OutwardShopToGodown"] = _OutwardShopToGodownService.GetDailyOutwardsToGodown();
            TempData["OutwardInterGodown"] = _OutwardInterGodownService.GetDailyOutwardInterGodown();
            TempData["OutwardInterShop"] = _OutwardInterShopService.GetDailyOutwardInterShop();
            TempData["OutwardToClient"] = _IOutwardToClientService.GetOutWardToClientByDate();
            TempData["InwardWithoutPO"] = _InwardFromSupplierService.GetDailyInwardWithoutPo();
            TempData["InwardFromGodown"] = _InwardFromGodownService.GetDailyInwardsFromGodown();
            TempData["InwardShopToGodown"] = _InwardFromShopToGodownService.GetDailyInwardsShopToGodown();
            TempData["InwardInterGodown"] = _InwardInterGodownService.GetDailyInwardInterGodown();
            TempData["InwardInterShop"] = _InwardInterShopService.GetDailyInwardInterShop();
            TempData["EntryStock"] = _EntryStockItemService.GetAllItems();
            TempData["ShopStock"] = _IShopStockService.GetShopStockTillDate();
            TempData["GodownStock"] = _GodownStockService.GetGodownStockTillDate();
            TempData["RetailBill"] = _IRetailInvoiceMasterService.GetRetailBillByDate();
            TempData["SalesBill"] = _ISalesBillService.GetSalesBillByDate();
            TempData["Quotations"] = _QuotationService.GetDailyQuotations();
            TempData["SalesOrders"] = _SalesOrderService.GetDailySalesOrders();
            TempData["DeliveryChallans"] = _DeliveryChallanService.GetDailyChallans();
            TempData["SalesReturns"] = _SalesReturnService.GetDailySalesReturns();
            TempData["PurchaseReturns"] = _PurchaseReturnService.GetDailyPurchaseReturns();
            TempData["CashierSalesOrder"] = _CashierSalesOrderService.GetDailyCashierSalesOrder();
            TempData["CashierRetailBill"] = _CashierRetailBillService.GetDailyCashierRetailBill();
            TempData["CashierTempCashMemo"] = _CashierTemporaryCashMemoService.GetDailyCashierTempCashMemo();
            TempData["CashierSalesBill"] = _CashierSalesBillService.GetDailyCashierSalesBill();
            TempData["CashierRefundOrder"] = _CashierRefundOrderService.GetDailyCashierRefundOrder();
            TempData["CashHandover"] = _CashHandoverService.GetDailyCashHandover();
            TempData["CardChequeHandover"] = _CardChequeHandoverService.GetDailyCardChequeHandover();
            TempData["IncomeExchangeVoucher"] = _IncomeExchangeVoucherService.GetDailyIncomeExchangeVoucher();
            TempData["Requisitionsshops"] = _RequisitionForShopService.GetDailyRequisitionsForShops(loggedinshopname);
            TempData["Requisitionsgodowns"] = _RequisitionForShopService.GetDailyRequisitionsForGodowns(loggedinshopname);
            TempData["Requisitionssuperadmin"] = _RequisitionForShopService.GetDailyRequisitionsForSuperAdmin(loggedinshopname);
            TempData["Requisitionsgodowns1"] = _RequisitionForGodownService.GetDailyRequisitionsForGodowns(loggedinshopname);
            TempData["Requisitionssuperadmin1"] = _RequisitionForGodownService.GetDailyRequisitionsForSuperAdmin(loggedinshopname);

            //DASHBOARD FOR LOGIN PERSON FOR EXCEPT SUPERADMIN
            var loginuserdetails = _EmployeeMasterService.getEmpByEmail(UserEmail);
            TempData["EmployeeDesignation"] = loginuserdetails.Designation;
            TempData["EmployeeDepartment"] = loginuserdetails.department;
            if (loginuserdetails.Designation == "PurchaseManager")
            {
                Session["LOGINSHOPGODOWNCODE"] = "PurchaseManager";
                Session["SHOPGODOWNNAME"] = "PurchaseManager";
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult PurchaseOrderReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Pending Po's    *****************//
        [HttpGet]
        public ActionResult GetPendingPo()
        {
            MainApplication model = new MainApplication();
            model.PurchaseOrderList = TempData["PendingPoes"] as IEnumerable<PurchaseOrderDetail>;
            TempData["PendingPoes"] = model.PurchaseOrderList;
            TempData["ConvertToPdf"] = "No";
            return View(model);
        }

        //*****************    List Of Get Inward Details By Date  *****************//
        [HttpGet]
        public ActionResult GetInwardDetailsByDate()
        {
            MainApplication model = new MainApplication();
            model.PurchaseOrderList = TempData["Inward"] as IEnumerable<PurchaseOrderDetail>;
            TempData["Inward"] = model.PurchaseOrderList;
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Get Inward Without Po By Date  *****************//
        [HttpGet]
        public ActionResult GetInwardWithoutPoByDate()
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = TempData["InwardWithoutPO"] as IEnumerable<InwardFromSupplier>;
            TempData["InwardWithoutPO"] = model.InwardFromSupplierList;
            return View(model);
        }

        //*****************    List Of Get Inward From Godown By Date  *****************//
        [HttpGet]
        public ActionResult GetInwardFromGodownByDate()
        {
            MainApplication model = new MainApplication();
            model.InwardFromGodownList = TempData["InwardFromGodown"] as IEnumerable<InwardFromGodown>;
            TempData["InwardFromGodown"] = model.InwardFromGodownList;
            return View(model);
        }

        //*****************    List Of Get Inward Shop To Godown By Date  *****************//
        [HttpGet]
        public ActionResult GetInwardShopToGodownByDate()
        {
            MainApplication model = new MainApplication();
            model.InwardFromShopToGodownList = TempData["InwardShopToGodown"] as IEnumerable<InwardFromShopToGodown>;
            TempData["InwardShopToGodown"] = model.InwardFromShopToGodownList;
            return View(model);
        }

        //*****************    List Of Get Inward Inter Godown By Date  *****************//
        [HttpGet]
        public ActionResult GetInwardInterGodownByDate()
        {
            MainApplication model = new MainApplication();
            model.InwardInterGodownList = TempData["InwardInterGodown"] as IEnumerable<InwardInterGodown>;
            TempData["InwardInterGodown"] = model.InwardInterGodownList;
            return View(model);
        }

        //*****************    List Of Get Inward Inter Shop By Date  *****************//
        [HttpGet]
        public ActionResult GetInwardInterShopByDate()
        {
            MainApplication model = new MainApplication();
            model.InwardInterShopList = TempData["InwardInterShop"] as IEnumerable<InwardInterShop>;
            TempData["InwardInterShop"] = model.InwardInterShopList;
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Get Outward To Shop By Date  *****************//
        [HttpGet]
        public ActionResult GetOutwardToShopByDate()
        {
            MainApplication model = new MainApplication();
            model.OutwardToShopList = TempData["OutwardToShop"] as IEnumerable<OutwardToShop>;
            TempData["OutwardToShop"] = model.OutwardToShopList;
            return View(model);
        }

        //*****************    List Of Get Outward Shop To Godown By Date  *****************//
        [HttpGet]
        public ActionResult GetOutwardShopToGodownByDate()
        {
            MainApplication model = new MainApplication();
            model.OutwardShopToGodownList = TempData["OutwardShopToGodown"] as IEnumerable<OutwardShopToGodown>;
            TempData["OutwardToShop"] = model.OutwardShopToGodownList;
            return View(model);
        }

        //*****************    List Of Get Outward Shop To Godown By Date  *****************//
        [HttpGet]
        public ActionResult GetOutwardInterGodownByDate()
        {
            MainApplication model = new MainApplication();
            model.OutwardInterGodownList = TempData["OutwardInterGodown"] as IEnumerable<OutwardInterGodown>;
            TempData["OutwardInterGodown"] = model.OutwardInterGodownList;
            return View(model);
        }

        //*****************    List Of Get Outward Shop To Godown By Date  *****************//
        [HttpGet]
        public ActionResult GetOutwardInterShopByDate()
        {
            MainApplication model = new MainApplication();
            model.OutwardInterShopList = TempData["OutwardInterShop"] as IEnumerable<OutwardInterShop>;
            TempData["OutwardInterShop"] = model.OutwardInterShopList;
            return View(model);
        }

        //*****************    List Of Get Outward To Client By Date  *****************//
        [HttpGet]
        public ActionResult GetOutwardToClientByDate()
        {
            MainApplication model = new MainApplication();
            model.OutwardToClientList = TempData["OutwardToClient"] as IEnumerable<OutwardToClient>;
            TempData["OutwardToClient"] = model.OutwardToClientList;
            return View(model);
        }

        [HttpGet]
        public ActionResult StockReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Entry Stock  *****************//
        [HttpGet]
        public ActionResult GetEntryStockTillDate()
        {
            MainApplication model = new MainApplication();
            model.EntryStockItemList = TempData["EntryStock"] as IEnumerable<EntryStockItem>;
            TempData["EntryStock"] = model.EntryStockItemList;
            return View(model);
        }

        //*****************    List Of Get ShopStock By Date  *****************//
        [HttpGet]
        public ActionResult GetShopStockTillDate()
        {
            MainApplication model = new MainApplication();
            model.ShopStockList = TempData["ShopStock"] as IEnumerable<ShopStock>;
            TempData["ShopStock"] = model.ShopStockList;
            return View(model);
        }

        //*****************    List Of Get Godown Stock By Date  *****************//
        [HttpGet]
        public ActionResult GetGodownStockTillDate()
        {
            MainApplication model = new MainApplication();
            model.GodownStockList = TempData["GodownStock"] as IEnumerable<GodownStock>;
            TempData["GodownStock"] = model.GodownStockList;
            return View(model);
        }

        [HttpGet]
        public ActionResult BillingReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Get Quotations By Date  *****************//
        [HttpGet]
        public ActionResult GetQuotationList()
        {
            MainApplication model = new MainApplication();
            model.QuotationList = TempData["Quotations"] as IEnumerable<Quotation>;
            TempData["Quotations"] = model.QuotationList;
            return View(model);
        }

        //*****************    List Of Get Sales Orders By Date  *****************//
        [HttpGet]
        public ActionResult GetSalesOrderList()
        {
            MainApplication model = new MainApplication();
            model.SalesOrderList = TempData["SalesOrders"] as IEnumerable<SalesOrder>;
            TempData["SalesOrders"] = model.SalesOrderList;
            return View(model);
        }

        //*****************    List Of Get Challans By Date  *****************//
        [HttpGet]
        public ActionResult GetDeliveryChallanList()
        {
            MainApplication model = new MainApplication();
            model.DeliveryChallanList = TempData["DeliveryChallans"] as IEnumerable<DeliveryChallan>;
            TempData["DeliveryChallans"] = model.DeliveryChallanList;
            return View(model);
        }

        //*****************    List Of Get Retail Bill By Date  *****************//
        [HttpGet]
        public ActionResult GetRetailBillByDate()
        {
            MainApplication model = new MainApplication();
            model.RetailBillList = TempData["RetailBill"] as IEnumerable<RetailBill>;
            TempData["RetailBill"] = model.RetailBillList;
            return View(model);
        }

        //*****************    List Of Get Sales Bill By Date  *****************//
        [HttpGet]
        public ActionResult GetSalesBillByDate()
        {
            MainApplication model = new MainApplication();
            model.SalesBillList = TempData["SalesBill"] as IEnumerable<SalesBill>;
            TempData["SalesBill"] = model.SalesBillList;
            return View(model);
        }

        //*****************    List Of Get Sales Return By Date  *****************//
        [HttpGet]
        public ActionResult GetSalesReturnByDate()
        {
            MainApplication model = new MainApplication();
            model.SalesReturnList = TempData["SalesReturns"] as IEnumerable<SalesReturn>;
            TempData["SalesReturns"] = model.SalesReturnList;
            return View(model);
        }

        //*****************    List Of Get Purchase Return By Date  *****************//
        [HttpGet]
        public ActionResult GetPurchaseReturnByDate()
        {
            MainApplication model = new MainApplication();
            model.PurchaseReturnList = TempData["PurchaseReturns"] as IEnumerable<PurchaseReturn>;
            TempData["PurchaseReturns"] = model.PurchaseReturnList;
            return View(model);
        }

        [HttpGet]
        public ActionResult CashierReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Daily Cashier Sales Order   *****************//
        [HttpGet]
        public ActionResult GetCashierSalesOrderList()
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderList = TempData["CashierSalesOrder"] as IEnumerable<CashierSalesOrder>;
            TempData["CashierSalesOrder"] = model.CashierSalesOrderList;
            return View(model);
        }

        //*****************    List Of Daily Cashier Retail Bill   *****************//
        [HttpGet]
        public ActionResult GetCashierRetailBillList()
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillList = TempData["CashierRetailBill"] as IEnumerable<CashierRetailBill>;
            TempData["CashierRetailBill"] = model.CashierRetailBillList;
            return View(model);
        }

        //*****************    List Of Daily Cashier Temporary Cash Memo   *****************//
        [HttpGet]
        public ActionResult GetCashierTempCashMemoList()
        {
            MainApplication model = new MainApplication();
            model.CashierTemporaryCashMemoList = TempData["CashierTempCashMemo"] as IEnumerable<CashierTemporaryCashMemo>;
            TempData["CashierTempCashMemo"] = model.CashierTemporaryCashMemoList;
            return View(model);
        }

        //*****************    List Of Daily Cashier Sales Bill   *****************//
        [HttpGet]
        public ActionResult GetCashierSalesBillList()
        {
            MainApplication model = new MainApplication();
            model.CashierSalesBillList = TempData["CashierSalesBill"] as IEnumerable<CashierSalesBill>;
            TempData["CashierSalesBill"] = model.CashierSalesBillList;
            return View(model);
        }

        //*****************    List Of Daily Cashier Refund Order   *****************//
        [HttpGet]
        public ActionResult GetCashierRefundOrderList()
        {
            MainApplication model = new MainApplication();
            model.CashierRefundOrderList = TempData["CashierRefundOrder"] as IEnumerable<CashierRefundOrder>;
            TempData["CashierRefundOrder"] = model.CashierRefundOrderList;
            return View(model);
        }

        //*****************    List Of Daily Cash HandOver   *****************//
        [HttpGet]
        public ActionResult GetCashHandoverList()
        {
            MainApplication model = new MainApplication();
            model.CashHandoverList = TempData["CashHandover"] as IEnumerable<CashHandover>;
            TempData["CashHandover"] = model.CashHandoverList;
            return View(model);
        }

        //*****************    List Of Daily Card Cheque HandOver   *****************//
        [HttpGet]
        public ActionResult GetCardChequeHandoverList()
        {
            MainApplication model = new MainApplication();
            model.CardChequeHandoverList = TempData["CardChequeHandover"] as IEnumerable<CardChequeHandover>;
            TempData["CardChequeHandover"] = model.CardChequeHandoverList;
            return View(model);
        }

        //*****************    List Of Daily Income Exchange Voucher   *****************//
        [HttpGet]
        public ActionResult GetIncomeExchangeVoucherList()
        {
            MainApplication model = new MainApplication();
            model.IncomeExpenseVoucherList = TempData["IncomeExchangeVoucher"] as IEnumerable<IncomeExpenseVoucher>;
            TempData["IncomeExchangeVoucher"] = model.IncomeExpenseVoucherList;
            return View(model);
        }

        [HttpGet]
        public ActionResult RequisitionReports()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            return View(model);
        }

        //*****************    List Of Get Requisitions By Date For Shop *****************//
        [HttpGet]
        public ActionResult GetRequisitionDetailsByDateForShop()
        {
            MainApplication model = new MainApplication();
            model.RequisitionForShopList = TempData["Requisitionsshops"] as IEnumerable<RequisitionForShop>;
            TempData["Requisitionsshops"] = model.RequisitionForShopList;
            return View(model);
        }

        //*****************    List Of Get Requisitions By Date For Godown  *****************//
        [HttpGet]
        public ActionResult GetRequisitionDetailsByDateForGodown()
        {
            MainApplication model = new MainApplication();
            model.RequisitionForShopList = TempData["Requisitionsgodowns"] as IEnumerable<RequisitionForShop>;
            TempData["Requisitionsgodowns"] = model.RequisitionForShopList;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetRequisitionDetailsByDateForSuperAdmin()
        {
            MainApplication model = new MainApplication();
            model.RequisitionForShopList = TempData["Requisitionssuperadmin"] as IEnumerable<RequisitionForShop>;
            TempData["Requisitionssuperadmin"] = model.RequisitionForShopList;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetRequisitionDetailsByDateForGodown1()
        {
            MainApplication model = new MainApplication();
            model.Requisitionforgodownlist = TempData["Requisitionsgodowns1"] as IEnumerable<RequisitionForGodown>;
            TempData["Requisitionsgodowns1"] = model.Requisitionforgodownlist;
            return View(model);
        }


        [HttpGet]
        public ActionResult GetRequisitionDetailsByDateForSuperAdmin1()
        {
            MainApplication model = new MainApplication();
            model.Requisitionforgodownlist = TempData["Requisitionssuperadmin1"] as IEnumerable<RequisitionForGodown>;
            TempData["Requisitionssuperadmin1"] = model.Requisitionforgodownlist;
            return View(model);
        }

    }
}
