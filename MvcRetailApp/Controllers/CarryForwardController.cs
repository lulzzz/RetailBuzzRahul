using System;
using System.Collections.Generic;
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
    [NoDirectAccess]
    [SessionExpireFilter]
    public class CarryForwardController : Controller
    {
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly ICompanyService _icompanyservice;
        private readonly IPurchaseOrderDetailService _ipurchaseorderdetails;
        private readonly IPurchaseItemDetailService _iPurchaseItemService;
        private readonly IInwardFromSupplierService _iInwardFromSupplierService;
        private readonly IInwardItemFromSupplierService _iInwardItemFryuomSupplierService;
        private readonly IOpeningStockService _iOpeningStockService;
        private readonly IEntryStockService _iEntryStockService;
        private readonly IEntryStockItemService _iEntryStockItemService;
        private readonly IUtilityService _iUtlityService;
        private readonly IQuotationService _iQuotationService;
        private readonly IQuotationItemService _iQuotationItemService;
        private readonly ISalesOrderService _iSalesOrderService;
        private readonly ISalesOrderItemService _iSalesOrderItemService;
        private readonly IStockDistributionService _iStockDistributionService;
        private readonly IStockItemDistributionService _iStockItemDistributionService;
        private readonly IDeliveryChallanService _iDeliveryChallanService;
        private readonly IDeliveryChallanItemService _iDelivaryChallanItemService;
        private readonly IDebitNoteService _iDebitNoteService;
        private readonly IDebitNoteItemService _iDebitNoteItemService;
        private readonly IOutwardToTailorService _iOutwardToTailorService;
        private readonly IOutwardToTailorItemService _iOutwardToTailorItemService;
        private readonly IRetailBillService _iRetailBillService;
        private readonly IRetailBillItemService _iRetailBillItemService;
        private readonly IRetailBillAdjAmtDetailService _iRetailBillAdjustAmountDetailService;
        private readonly ISalesReturnService _iSalesReturnService;
        private readonly ISalesReturnItemService _iSalesReturnItemService;
        private readonly IRetailBillCreditNoteService _iRetailBillCreditNoteService;
        private readonly IRetailBillCreditNoteItemService _iRetailBillCreditNoteItemService;
        private readonly IInventoryTaxService _iInventoryTaxService;
        private readonly IBalanceCarryForwardService _iBalanceCarryForwardService;
        private readonly ISalesBillService _iSalesBillService;
        private readonly ISalesBillItemService _iSalesbillItemService;
        private readonly ISalesBillAdjAmtDetailService _IsalesBillAdjsutAmountDetails;
        private readonly ISalesBillCreditNoteService _iSalesBillCreditNoteService;
        private readonly ISalesBillCreditNoteItemService _iSalesBillCreditNoteItemService;
        private readonly ICashierSalesOrderService _iCashierSalesOrderService;
        private readonly ICashierSalesOrderItemService _iCashierSalesOrderItemService;
        private readonly ICardChequeHandoverService _iCardChequeHandoverService;
        private readonly ICashierRetailBillService _iCashierRetailBillService;
        private readonly ICashierRetailBillItemService _iCashierRetailBillItemService;
        private readonly ICashierSalesBillService _iCashierSalesBillService;
        private readonly ICashierSalesBillItemService _iCashierSalesBillItemService;
        private readonly IRequisitionForShopService _iRequisitionForShopService;
        private readonly IRequisitionForGodownService _iRequisitionForGodownService;
        private readonly IInwardFromTailorService _iInwardFromTailorService;
        private readonly IInwardFromTailorItemService _iInwardfromTailorItemService;
        private readonly ITemporaryCashMemoService _iTemporaryCashMemoService;
        private readonly ITemporaryCashMemoItemService _iTemporaryCashMemoItemService;
        private readonly ITemporaryCashMemoAdjAmtDetailService _ITemporaryCashMemoAdjustAmountDetailService;
        private readonly ICashierTemporaryCashMemoService _iCashierTemporaryCashMemoService;
        private readonly ICashierTemporaryCashMemoItemService _iCashierTemporarayCashMemoItemService;
        private readonly IJobWorkPaymentService _iJobWorkPaymentService;
        private readonly IShopStockService _iShopStockService;
        private readonly IGodownStockService _iGodownStockService;
        private readonly IDiscountMasterService _iDiscountMasterService;
        private readonly IDiscountMasterItemService _iDiscountMasterItemservice;
        private readonly ICostCodeCreationService _icostcodecreation;
        private readonly IOutwardToTailorService _ioutwardtotailorservice;
        private readonly IOutwardToTailorItemService _ioutwardtotailoritemservice;
        private readonly IInwardFromTailorService _iinwardfromtailorservice;
        private readonly IInwardFromTailorItemService _iinwardfromtailoritemservice;
        private readonly IInwardToTailorService _iinwardtotailorservice;
        private readonly IInwardToTailorItemService _iinwardtotailoritemservice;
        private readonly IJobWorkStockService _iJobWorkStockService;
        private readonly IJobWorkOutwardToClientService _ijobworkoutwardtoclient;

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

        public CarryForwardController(IUserCredentialService usercredentialservice, IModuleService iIModuleService,ICompanyService CompanyService, IPurchaseOrderDetailService purchaseorderdetailservice, IPurchaseItemDetailService purchaseitemdetailservice, IInwardFromSupplierService inwardFromSupplierService,
            IInwardItemFromSupplierService InwardItemFromSupplierService, IOpeningStockService OpeningStockService,
            IEntryStockItemService EntryStocItemkService, IEntryStockService EntryStockService, IUtilityService UtilityService, IQuotationService QuotationService,
            IQuotationItemService QuotationItemService, ISalesOrderService SalesOrderService, ISalesOrderItemService SalesOrderItemService,
            IStockDistributionService StockDistributionService, IStockItemDistributionService StockItemDistributionService,
            IDeliveryChallanService DelivaryChallanService, IDeliveryChallanItemService DelivaryChallanItemService,
            IDebitNoteService DebitNoteService, IDebitNoteItemService DebitNoteItemService, IOutwardToTailorService OutwardToTailorService,
            IOutwardToTailorItemService OutwardToTailorItemService, IInventoryTaxService InventoryTaxService, IRetailBillService RetailBillService,
            IRetailBillItemService RetailBillItemService, IRetailBillAdjAmtDetailService RetailBillAdjAmtDetailService, ISalesReturnService SalesReturnService,
            ISalesReturnItemService SalesReturnItemService, IRetailBillCreditNoteService RetailBillCreditNoteService, IRetailBillCreditNoteItemService RetailBillCreditNoteItemService,
            IBalanceCarryForwardService BalanceCarryForwardService, ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService,
            ISalesBillAdjAmtDetailService SalesBillAdjAmtDetailService, ICashierSalesOrderService CashierSalesOrderService,
            ICashierSalesOrderItemService CashierSalesOrderItemService, ICardChequeHandoverService CardChequeHandoverService, ICashierRetailBillService CashierRetailBillService,
            ICashierRetailBillItemService CashierRetailBillItemService, ICashierSalesBillService CashierSalesBillService,
            ICashierSalesBillItemService CashierSalesBillItemService, IRequisitionForShopService RequisitionForShopService,
            IRequisitionForGodownService RequisitionForGodownService,IInwardFromTailorService InwardFromTailorService,IInwardFromTailorItemService InwarditemFromTailorService,
            ITemporaryCashMemoService TemporaryCashMemoService, ITemporaryCashMemoItemService TemporaryCashMemoItemService,
            ITemporaryCashMemoAdjAmtDetailService TemporaryCashMemoAdjAmtDetailService, ICashierTemporaryCashMemoService CashierTemporaryCashMemoService,
            ICashierTemporaryCashMemoItemService CashierTemporaryCashMemoItemService, ISalesBillCreditNoteService SalesBillCreditNoteService,
            ISalesBillCreditNoteItemService SalesBillCreditNoteItemService, IJobWorkPaymentService JobWorkPaymentService,
            IShopStockService ShopStockService, IGodownStockService GodownStockService, IDiscountMasterService DiscountMasterService,
            IDiscountMasterItemService DiscountMasterItemService,ICostCodeCreationService costcodecreation,
            IOutwardToTailorService outwardtotailorservice, IOutwardToTailorItemService outwardtotailoritemservice,IInwardFromTailorService inwardfromtailorservice,
            IInwardFromTailorItemService inwardfromtailoritemservice,IInwardToTailorService inwardtotailorservice,IInwardToTailorItemService inwardtotailoritemservice,
            IJobWorkStockService jobworkstockservice,IJobWorkOutwardToClientService jobworkoutwardtoclientservice)
        {
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._icompanyservice = CompanyService;
            this._ipurchaseorderdetails = purchaseorderdetailservice;
            this._iPurchaseItemService = purchaseitemdetailservice;
            this._iInwardFromSupplierService = inwardFromSupplierService;
            this._iInwardItemFryuomSupplierService = InwardItemFromSupplierService;
            this._iOpeningStockService = OpeningStockService;
            this._iEntryStockItemService = EntryStocItemkService;
            this._iEntryStockService = EntryStockService;
            this._iUtlityService = UtilityService;
            this._iQuotationService = QuotationService;
            this._iQuotationItemService = QuotationItemService;
            this._iSalesOrderService = SalesOrderService;
            this._iSalesOrderItemService = SalesOrderItemService;
            this._iStockDistributionService = StockDistributionService;
            this._iStockItemDistributionService = StockItemDistributionService;
            this._iDeliveryChallanService = DelivaryChallanService;
            this._iDelivaryChallanItemService = DelivaryChallanItemService;
            this._iDebitNoteService = DebitNoteService;
            this._iDebitNoteItemService = DebitNoteItemService;
            this._iOutwardToTailorService = OutwardToTailorService;
            this._iOutwardToTailorItemService = OutwardToTailorItemService;
            this._iInventoryTaxService = InventoryTaxService;
            this._iRetailBillService = RetailBillService;
            this._iRetailBillItemService = RetailBillItemService;
            this._iRetailBillAdjustAmountDetailService = RetailBillAdjAmtDetailService;
            this._iSalesReturnService = SalesReturnService;
            this._iSalesReturnItemService = SalesReturnItemService;
            this._iRetailBillCreditNoteService = RetailBillCreditNoteService;
            this._iRetailBillCreditNoteItemService = RetailBillCreditNoteItemService;
            this._iBalanceCarryForwardService = BalanceCarryForwardService;
            this._iSalesBillService = SalesBillService;
            this._iSalesbillItemService = SalesBillItemService;
            this._IsalesBillAdjsutAmountDetails = SalesBillAdjAmtDetailService;
            this._iSalesbillItemService = SalesBillItemService;
            this._IsalesBillAdjsutAmountDetails = SalesBillAdjAmtDetailService;
            this._iCashierSalesOrderService = CashierSalesOrderService;
            this._iCashierSalesOrderItemService = CashierSalesOrderItemService;
            this._iCardChequeHandoverService = CardChequeHandoverService;
            this._iCashierRetailBillService = CashierRetailBillService;
            this._iCashierRetailBillItemService = CashierRetailBillItemService;
            this._iCashierRetailBillService = CashierRetailBillService;
            this._iCashierRetailBillItemService = CashierRetailBillItemService;
            this._iCashierSalesBillService = CashierSalesBillService;
            this._iCashierSalesBillItemService = CashierSalesBillItemService;
            this._iRequisitionForShopService = RequisitionForShopService;
            this._iRequisitionForGodownService = RequisitionForGodownService;
            this._iInwardFromTailorService = InwardFromTailorService;
            this._iInwardfromTailorItemService = InwarditemFromTailorService;
            this._iTemporaryCashMemoService = TemporaryCashMemoService;
            this._iTemporaryCashMemoItemService = TemporaryCashMemoItemService;
            this._ITemporaryCashMemoAdjustAmountDetailService = TemporaryCashMemoAdjAmtDetailService;
            this._iSalesBillCreditNoteService = SalesBillCreditNoteService;
            this._iSalesBillCreditNoteItemService = SalesBillCreditNoteItemService;
            this._iJobWorkPaymentService = JobWorkPaymentService;
            this._iCashierTemporaryCashMemoService = CashierTemporaryCashMemoService;
            this._iCashierTemporarayCashMemoItemService = CashierTemporaryCashMemoItemService;
            this._iShopStockService = ShopStockService;
            this._iGodownStockService = GodownStockService;
            this._iDiscountMasterService = DiscountMasterService;
            this._iDiscountMasterItemservice = DiscountMasterItemService;
            this._icostcodecreation = costcodecreation;
            this._ioutwardtotailorservice = outwardtotailorservice;
            this._iOutwardToTailorItemService = OutwardToTailorItemService;
            this._iinwardfromtailorservice = inwardfromtailorservice;
            this._iInwardfromTailorItemService = inwardfromtailoritemservice;
            this._iinwardtotailorservice = inwardtotailorservice;
            this._iinwardtotailoritemservice = inwardtotailoritemservice;
            this._iJobWorkStockService = jobworkstockservice;
            this._ijobworkoutwardtoclient = jobworkoutwardtoclientservice;
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
        public ActionResult Create()
        {
            MainApplication model = new MainApplication
            
            {
                companydetails = new Company(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string Cname = CompanyName;
            string Fyear = FinancialYear;
            Session["fyear"] = Fyear;

            //session to retreive main database
            var CmpList = (IEnumerable<Company>)Session["CompanyList"];

            //splitting loggedin database financial year date
            char[] delimiterChars = { ' ', ',', '.', ':', '\t', '-' };
            string[] words = Fyear.Split(delimiterChars);
            string fyfrom = words[1] + "/" + words[0] + "/" + words[2];
            string fyto = words[5] + "/" + words[4] + "/" + words[6];

            DateTime? fromdate = Convert.ToDateTime(fyfrom);
            List<Company> companylist = new List<Company>();

            string namebranch = string.Empty;
            var companyname = HttpContext.Session["CompanyName"].ToString();
            var financialyear = HttpContext.Session["FinancialYear"].ToString();

            foreach (var data in CmpList)
            {
                if (data.companyName == companyname && data.FinancialYearFrom > fromdate)
                {
                    namebranch = data.companyName + " " + Convert.ToDateTime(data.FinancialYearFrom).ToString("dd-MM-yyyy") + " " + "To" + " " + Convert.ToDateTime(data.FinancialYearTo).ToString("dd-MM-yyyy");
                    data.companyName = namebranch;
                    companylist.Add(data);
                    namebranch = string.Empty;
                }
            }
            model.CmpList= companylist;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MainApplication mainapp, FormCollection frm)
        {
            string TocompanyName = frm["ddlCompany"];
            TempData["TempCompanyName"] = TocompanyName;
            string ToPurchaseOrder = frm["PurchaseOrder"];
            TempData["TempPurchaseOrder"] = ToPurchaseOrder;
            string InwardFromSupplier = frm["InwardFromSupplier"];
            TempData["TempInwardFromSupplier"] = InwardFromSupplier;
            string InwardWithoutpo = frm["InwardWithoutPo"];
            TempData["TempInwardWithoutpo"] = InwardWithoutpo;
            string quotation = frm["Quotations"];
            TempData["TempQuotation"] = quotation;
            string DeliveryChallan = frm["DelivaryChallan"];
            TempData["TempDeliveyChallan"] = DeliveryChallan;
            string SalesReturn = frm["SalesReturn"];
            TempData["TempSalesReturn"] = SalesReturn;
            string OpeningStock = frm["OpeningStock"];
            TempData["TempOpeningStockvalue"] = OpeningStock;
            string OutwardToTailor = frm["OutwardToTailor"];
            TempData["TempOutwardToTailor"] = OutwardToTailor;
            //string EntryStock = frm["EntryStock"];
            //TempData["TempEntryStock"] = EntryStock;
            string SalesOrder = frm["SalesOrder"];
            TempData["TempSalesOrder"] = SalesOrder;
            //string StockDistribution = frm["StockDistribution"];
            //TempData["TempStockDistribution"] = StockDistribution;
            string RetailBill = frm["RetailBill"];
            TempData["TempRetailBill"] = RetailBill;
            string SalesBill = frm["SalesBill"];
            TempData["TempSalesBill"] = SalesBill;
            string TemporaryCashMemo = frm["TemporaryCashMemo"];
            TempData["TempTemporaryCashMemo"] = TemporaryCashMemo;
            string BalanceCarryForward = frm["BalanceCarryForward"];
            TempData["TempBalanceCarryForward"] = BalanceCarryForward;
            string CashierSalesOrder = frm["CashierSalesOrder"];
            TempData["TempCashierSalesOrder"] = CashierSalesOrder;
            string CashierRetailBill = frm["CashierRetailBill"];
            TempData["TempCashierRetailBill"] = CashierRetailBill;
            string CashierSalesBill = frm["CashierSalesBill"];
            TempData["TempCashierSalesBill"] = CashierSalesBill;
            string ShopRequisition = frm["ShopRequisition"];
            TempData["TempShopRequisition"] = ShopRequisition;
            string GodownRequisition = frm["GodownRequisition"];
            TempData["TempGodownRequisition"] = GodownRequisition;
            string InwardFromTailor = frm["InwardFromTailor"];
            TempData["TempInwardFromTailor"] = InwardFromTailor;
            string CashierTemporaryCashMemo = frm["CashierTemporaryCashMemo"];
            TempData["TempCashierTemporaryCashMemo"] = CashierTemporaryCashMemo;
            string JobWorkPayments = frm["JobWorkPayments"];
            TempData["TempJobWorkPayments"] = JobWorkPayments;
            string Discount = frm["Discount"];
            TempData["TempDiscount"] = Discount;
            string costcodecreation = frm["CostCode"];
            TempData["TempCostCode"] = costcodecreation;
            //string outwardtotailor = frm["outwardtotailor"];
            //TempData["Tempoutwardtotailor"] = outwardtotailor;
            //string inwardfromtailor = frm["inwardfromtailor"];
            //TempData["Tempinwardfromtailor"] = inwardfromtailor;
            string sentitemstotailor = frm["sentitemstotailor"];
            TempData["Tempsentitemstotailor"] = sentitemstotailor;
            string tailorstock = frm["tailorstock"];
            TempData["Temptailorstock"] = tailorstock;
            string outwarditemstoclient = frm["outwarditemstoclient"];
            TempData["Tempoutwarditemstoclient"] = outwarditemstoclient;
         
            IEnumerable<PurchaseOrderDetail> purchaseoredrdetailsforincomplete;
            IEnumerable<PurchaseItemDetail> purchaseItemDetailService;
            IEnumerable<InventoryTax> InventoryTaxpurchaseOrder;

            IEnumerable<InwardFromSupplier> InwardFromSupplierList;
            IEnumerable<InwardItemsFromSupplier> InwardItemsFromSupplier;
            IEnumerable<InventoryTax> InventoryTaxInwardFromSupplier;
            IEnumerable<DebitNote> DebitNoteList;
            IEnumerable<DebitNoteItem> DebitNoteItemList;
            IEnumerable<InventoryTax> InventoryTaxInwardWithoutPo;

            IEnumerable<OpeningStockMaster> OpeningStockList;
            IEnumerable<EntryStockMaster> EntryStockMaster;
            IEnumerable<EntryStockItem> EntryStockItem;
            IEnumerable<ShopStock> ShopStockItem;
            IEnumerable<GodownStock> GodownStockItem;
            IEnumerable<StockDistribution> StockDistributionList;
            IEnumerable<StockItemDistribution> StockItemDistributionList;

            IEnumerable<Quotation> QuotationList;
            IEnumerable<QuotationItem> Quotationitem;
            IEnumerable<InventoryTax> InventoryTaxQuotation;

            IEnumerable<SalesOrder> Salesorderlist;
            IEnumerable<SalesOrderItem> Salesorderitemlist;
            IEnumerable<InventoryTax> InventoryTaxSalesOrder;

            IEnumerable<DeliveryChallan> DeliveryChallanList;
            IEnumerable<DeliveryChallanItem> DeliveryChallanItemList;
            IEnumerable<InventoryTax> InventoryTaxDelivaryChallan;

            IEnumerable<OutwardToTailor> OutwardToTailorList;
            IEnumerable<OutwardToTailorItem> OutwardToTailorItemList;

            IEnumerable<RetailBill> RetailBillMainTable;
            IEnumerable<RetailBillItem> RetailBillItem;
            IEnumerable<InventoryTax> InventoryTaxRetailBill;
            IEnumerable<RetailBillAdjAmtDetail> RetailBillAdjAmtDetail;
            IEnumerable<SalesReturn> RetailBillSalesreturn;
            IEnumerable<SalesReturnItem> RetailBillSalesReturnItem;
            IEnumerable<InventoryTax> InventoryTaxSalesReturn;
            IEnumerable<RetailBillCreditNote> RetailBillCreditNote;
            IEnumerable<RetailBillCreditNoteItem> RetailBillCreditNoteItem;

            IEnumerable<SalesBill> SalesBillMainTable;
            IEnumerable<SalesBillItem> SalesBillItem;
            IEnumerable<InventoryTax> InventoryTaxSalesBill;
            IEnumerable<SalesBillAdjAmtDetail> SalesBillAdjAmtDetail;
            IEnumerable<SalesReturn> SalesBillSalesreturn;
            IEnumerable<SalesReturnItem> SalesBillSalesReturnItem;
            IEnumerable<InventoryTax> InventoryTaxSalesReturnSalesBill;
            IEnumerable<SalesBillCreditNote> SalesBillCreditNote;
            IEnumerable<SalesBillCreditNoteItem> SalesBillCreditNoteItem;

            BalanceCarryForward BalanceCarryForwardEntity;

            IEnumerable<SalesOrder> ActiveSalesOrder;
            IEnumerable<CashierSalesOrder> CashierSalesOrderEnum;
            IEnumerable<CashierSalesOrderItem> CashierSalesorderItem;
            IEnumerable<CashierSalesOrder> CashierSalesOrderCardChequeHandOver;
            IEnumerable<CashierSalesOrderItem> CashierSalesorderItemCardChequeHandOver;

            IEnumerable<RetailBill> ActiveRetailBill;
            IEnumerable<CashierRetailBill> CashierRetailBillEnum;
            IEnumerable<CashierRetailBillItem> CashierRetailBillItem;
            IEnumerable<CashierRetailBill> CashierRetailBillCardChequeHandOver;
            IEnumerable<CashierRetailBillItem> CashierRetailBillItemCardChequeHandOver;

            IEnumerable<SalesBill> ActiveSalesBill;
            IEnumerable<CashierSalesBill> CashierSalesBillEnum;
            IEnumerable<CashierSalesBillItem> CashierSalesBillItem;
            IEnumerable<CashierSalesBill> CashierSalesBillCardChequeHandOver;
            IEnumerable<CashierSalesBillItem> CashierSalesBillItemCardChequeHandOver;

            IEnumerable<RequisitionForShop> RequisitionForShop;
            IEnumerable<RequisitionForGodown> RequisitionForGodown;

            IEnumerable<InwardFromTailor> InwardFromTailorEnum;
            IEnumerable<InwardFromTailorItem> InwardFromTailorItemEnum;

            IEnumerable<TemporaryCashMemo> TCM;
            IEnumerable<TemporaryCashMemoItem> TCMItem;
            IEnumerable<InventoryTax> InventoryTaxTCM;
            IEnumerable<TemporaryCashMemoAdjAmtDetail> TCMAdjAmtDetail;

            IEnumerable<TemporaryCashMemo> ActiveTemporaryCashMemo;
            IEnumerable<CashierTemporaryCashMemo> CashierTemporaryCashMemoEnum;
            IEnumerable<CashierTemporaryCashMemoItem> CashierTemporaryCashMemoItem;
            IEnumerable<CashierTemporaryCashMemo> CashierTemporaryCashMemoCardChequeHandOver;
            IEnumerable<CashierTemporaryCashMemoItem> CashierTemporaryCashMemoItemCardChequeHandOver;

            IEnumerable<JobWorkPayment> JobWorkPaymentEnum;

            IEnumerable<DiscountMaster> DiscountMaster;
            IEnumerable<DiscountMasterItem> DiscountMasterItem;

            IEnumerable<CostCodeCreation> costcodecreationenum;

            //IEnumerable<OutwardToTailor> OutwardToTailorenum;
            //IEnumerable<OutwardToTailorItem> OutwardToTailorItemenum;
            //IEnumerable<InwardFromTailor> InwardFromTailorenum;
            //IEnumerable<InwardFromTailorItem> InwardFromTailorItemenum;

            IEnumerable<InwardToTailor> InwardToTailorenum;
            IEnumerable<InwardToTailorItem> InwardToTailorItemenum;
            IEnumerable<JobWorkStock> JobWorkStockenum;
            IEnumerable<JobWorkOutwardToClient> JobWorkOutwardToClientenum;

            //DateTime systemdate = DateTime.Now.Date;

            ////string systemdatevalue = Session["fromdate"].ToString();
            //string systemdatevalue1 = frm["dropdownvalue"];
            //char[] delimiterChars = { ' ', ',', '.', ':', '\t', '-', 'A'};
            //string[] words = systemdatevalue1.Split(delimiterChars);
            //string fyfrom = words[3] + "-" + words[2] + "-" + words[4];
            ////string fyto = words[4] + "/" + words[3] + "/" + words[5];

            //DateTime? systemdatevaluehidden = Convert.ToDateTime(fyfrom);

           // if (systemdate == systemdatevaluehidden)
          // {
               //Purchase Order
               if (ToPurchaseOrder != null)
               {
                   purchaseoredrdetailsforincomplete = _ipurchaseorderdetails.GetPendingPoForCarryForward();
                   Session["PurchaseOrderDetail"] = purchaseoredrdetailsforincomplete;
                   List<PurchaseItemDetail> purchaseitemdetail = new List<PurchaseItemDetail>();
                   List<InventoryTax> InventoryTaxPurchaseOrderList = new List<InventoryTax>();
                   foreach (var data in purchaseoredrdetailsforincomplete)
                   {
                       purchaseItemDetailService = _iPurchaseItemService.GetPurchaseInventoryItemsByPONo(data.PoNo);

                       foreach (var item in purchaseItemDetailService)
                       {
                           purchaseitemdetail.Add(item);
                       }

                   }
                   purchaseItemDetailService = purchaseitemdetail;
                   Session["PurchaseItemDetail"] = purchaseItemDetailService;

                   foreach (var data in purchaseoredrdetailsforincomplete)
                   {
                       InventoryTaxpurchaseOrder = _iInventoryTaxService.GetTaxesByCode(data.PoNo);

                       foreach (var item in InventoryTaxpurchaseOrder)
                       {
                           InventoryTaxPurchaseOrderList.Add(item);
                       }
                   }
                   InventoryTaxpurchaseOrder = InventoryTaxPurchaseOrderList;
                   Session["InventoryTaxpurchaseOrder"] = InventoryTaxpurchaseOrder;
               }
           //}
           // else
           // {
           // }

               //InwardFromSupplier
               if (InwardFromSupplier != null)
               {
                   InwardFromSupplierList = _iInwardFromSupplierService.GetDataByPINoAndStatusForPo();
                   Session["InwardFromSupplier"] = InwardFromSupplierList;
                   List<InwardItemsFromSupplier> InwardItemFromSupplier = new List<InwardItemsFromSupplier>();
                   List<InventoryTax> InventoryTaxInwardFromSupplierList = new List<InventoryTax>();
                   foreach (var data1 in InwardFromSupplierList)
                   {
                       InwardItemsFromSupplier = _iInwardItemFryuomSupplierService.GetInvItemsByInwardNo(data1.InwardNo);
                       foreach (var item in InwardItemsFromSupplier)
                       {
                           InwardItemFromSupplier.Add(item);
                       }
                   }

                   InwardItemsFromSupplier = InwardItemFromSupplier;
                   Session["InwardItemsFromSupplier"] = InwardItemsFromSupplier;

                   foreach (var data in InwardFromSupplierList)
                   {
                       InventoryTaxInwardFromSupplier = _iInventoryTaxService.GetTaxesByCode(data.InwardNo);
                       foreach (var item in InventoryTaxInwardFromSupplier)
                       {
                           InventoryTaxInwardFromSupplierList.Add(item);
                       }

                   }
                   InventoryTaxInwardFromSupplier = InventoryTaxInwardFromSupplierList;
                   Session["InventoryTaxInwardFromSupplier"] = InventoryTaxInwardFromSupplier;

                   DebitNoteList = _iDebitNoteService.GetDebitNotesPending();
                   Session["DebitNote"] = DebitNoteList;
                   List<DebitNoteItem> DebitNoteItem = new List<DebitNoteItem>();
                   foreach (var data in DebitNoteList)
                   {
                       DebitNoteItemList = _iDebitNoteItemService.GetDebitNotesItemPending(data.DebitNoteNo);
                       foreach (var item in DebitNoteItemList)
                       {
                           DebitNoteItem.Add(item);
                       }
                   }
                   DebitNoteItemList = DebitNoteItem;
                   Session["DebitNoteItemList"] = DebitNoteItemList;
               }

               //Inward Without Po
               if (InwardWithoutpo != null)
               {
                   InwardFromSupplierList = _iInwardFromSupplierService.GetDataByPINoAndStatusForNopo();
                   Session["InwardWithoutPo"] = InwardFromSupplierList;
                   List<InwardItemsFromSupplier> InwardItemFromSupplier = new List<InwardItemsFromSupplier>();
                   List<InventoryTax> InventoryTaxInwardWithoutPoList = new List<InventoryTax>();
                   foreach (var data1 in InwardFromSupplierList)
                   {
                       InwardItemsFromSupplier = _iInwardItemFryuomSupplierService.GetInvItemsByInwardNoForNopO(data1.InwardNo);
                       foreach (var item in InwardItemsFromSupplier)
                       {
                           InwardItemFromSupplier.Add(item);
                       }
                   }

                   InwardItemsFromSupplier = InwardItemFromSupplier;
                   Session["InwardWithoutPoItems"] = InwardItemsFromSupplier;

                   foreach (var data in InwardFromSupplierList)
                   {
                       InventoryTaxInwardFromSupplier = _iInventoryTaxService.GetTaxesByCode(data.InwardNo);
                       foreach (var item in InventoryTaxInwardFromSupplier)
                       {
                           InventoryTaxInwardWithoutPoList.Add(item);
                       }
                   }
                   InventoryTaxInwardWithoutPo = InventoryTaxInwardWithoutPoList;
                   Session["InventoryTaxInventoryTaxInwardWithoutPo"] = InventoryTaxInwardWithoutPo;
               }

               //Quotation
               if (quotation != null)
               {
                   QuotationList = _iQuotationService.GetPendingQuotationsPoForCarryForward();
                   Session["Quotation"] = QuotationList;
                   List<QuotationItem> QuotationItemList = new List<QuotationItem>();
                   List<InventoryTax> InventoryTaxQuotationList = new List<InventoryTax>();
                   foreach (var data in QuotationList)
                   {
                       Quotationitem = _iQuotationItemService.GetInventoryItemsByQuotNo(data.QuotNo);
                       foreach (var item in Quotationitem)
                       {
                           QuotationItemList.Add(item);
                       }
                   }
                   Quotationitem = QuotationItemList;
                   Session["QuotationItem"] = Quotationitem;

                   foreach (var data in QuotationList)
                   {
                       InventoryTaxQuotation = _iInventoryTaxService.GetTaxesByCode(data.QuotNo);
                       foreach (var item in InventoryTaxQuotation)
                       {
                           InventoryTaxQuotationList.Add(item);
                       }
                   }
                   InventoryTaxQuotation = InventoryTaxQuotationList;
                   Session["InventoryTaxQuotation"] = InventoryTaxQuotation;
               }

               //Delivary Challan
               if (DeliveryChallan != null)
               {
                   DeliveryChallanList = _iDeliveryChallanService.GetChallanNoByStatus();
                   Session["DeliveryChallan"] = DeliveryChallanList;
                   List<DeliveryChallanItem> DeliveryChallanItem = new List<DeliveryChallanItem>();
                   List<InventoryTax> InventoryTaxDeliveryChallanList = new List<InventoryTax>();
                   foreach (var data in DeliveryChallanList)
                   {
                       DeliveryChallanItemList = _iDelivaryChallanItemService.GetInventoryItemsByChallanNo(data.ChallanNo);
                       foreach (var item in DeliveryChallanItemList)
                       {
                           DeliveryChallanItem.Add(item);
                       }
                   }
                   DeliveryChallanItemList = DeliveryChallanItem;
                   Session["DeliveryChallanItemList"] = DeliveryChallanItemList;

                   foreach (var data in DeliveryChallanList)
                   {
                       InventoryTaxDelivaryChallan = _iInventoryTaxService.GetTaxesByCode(data.ChallanNo);

                       foreach (var item in InventoryTaxDelivaryChallan)
                       {
                           InventoryTaxDeliveryChallanList.Add(item);
                       }
                   }
                   InventoryTaxDelivaryChallan = InventoryTaxDeliveryChallanList;
                   Session["InventoryTaxDelivaryChallan"] = InventoryTaxDelivaryChallan;
               }

               //Opening Stock
               if (OpeningStock != null)
               {
                   OpeningStockList = _iOpeningStockService.GetItemsByStatus();
                   Session["OpeningSockValue"] = OpeningStockList;

                   //Entrystock
                   EntryStockMaster = _iEntryStockService.GetItemsByStatus();
                   Session["EntryStockMaster"] = EntryStockMaster;
                   List<EntryStockItem> EntryStockItemList = new List<EntryStockItem>();
                   foreach (var data in EntryStockMaster)
                   {
                       EntryStockItem = _iEntryStockItemService.GetDetailsByEntryStockCode(data.EntryStockCode);
                       foreach (var item in EntryStockItem)
                       {
                           EntryStockItemList.Add(item);
                       }
                   }

                   EntryStockItem = EntryStockItemList;
                   Session["EntryStockItem"] = EntryStockItem;

                   //ShopStock
                   ShopStockItem = _iShopStockService.getShopStock();
                   Session["ShopStockItem"] = ShopStockItem;

                   //GodownStock
                   GodownStockItem = _iGodownStockService.getGodownStock();
                   Session["GodownStockItem"] = GodownStockItem;

                   //StockDistribution
                   StockDistributionList = _iStockDistributionService.GetItemsByStatus();
                   Session["StockDistributionList"] = StockDistributionList;

                   List<StockItemDistribution> StockItemDistribution = new List<StockItemDistribution>();
                   foreach (var data in StockDistributionList)
                   {
                       StockItemDistributionList = _iStockItemDistributionService.GetDetailsByStkCode(data.StockDistributionCode);
                       foreach (var item in StockItemDistributionList)
                       {
                           StockItemDistribution.Add(item);
                       }
                   }
                   StockItemDistributionList = StockItemDistribution;
                   Session["StockItemDistributionList"] = StockItemDistributionList;
               }

               //Outward To Tailor
               if (OutwardToTailor != null)
               {
                   OutwardToTailorList = _iOutwardToTailorService.GetPendingOutwardToTailors();
                   Session["OutwardToTailor"] = OutwardToTailorList;
                   List<OutwardToTailorItem> Outwardtotailoritemlist = new List<OutwardToTailorItem>();
                   foreach (var data in OutwardToTailorList)
                   {
                       OutwardToTailorItemList = _iOutwardToTailorItemService.GetPendingOutwardToTailors(data.OutwardCode);
                       foreach (var item in OutwardToTailorItemList)
                       {
                           Outwardtotailoritemlist.Add(item);
                       }
                   }
                   OutwardToTailorItemList = Outwardtotailoritemlist;
                   Session["OutwardToTailorItemList"] = OutwardToTailorItemList;
               }

               //Entry Stock
               //if (EntryStock != null)
               //{
               //    EntryStockMaster = _iEntryStockService.GetItemsByStatus();
               //    Session["EntryStockMaster"] = EntryStockMaster;
               //    List<EntryStockItem> EntryStockItemList = new List<EntryStockItem>();
               //    foreach (var data in EntryStockMaster)
               //    {
               //        EntryStockItem = _iEntryStockItemService.GetDetailsByEntryStockCode(data.EntryStockCode);
               //        foreach (var item in EntryStockItem)
               //        {
               //            EntryStockItemList.Add(item);
               //        }
               //    }
               //    EntryStockItem = EntryStockItemList;
               //    Session["EntryStockItem"] = EntryStockItem;
               //}

               //Stock Distribution
               //if (StockDistribution != null)
               //{
               //    StockDistributionList = _iStockDistributionService.GetItemsByStatus();
               //    Session["StockDistributionList"] = StockDistributionList;

               //    List<StockItemDistribution> StockItemDistribution = new List<StockItemDistribution>();
               //    foreach (var data in StockDistributionList)
               //    {
               //        StockItemDistributionList = _iStockItemDistributionService.GetDetailsByStkCode(data.StockDistributionCode);
               //        foreach (var item in StockItemDistributionList)
               //        {
               //            StockItemDistribution.Add(item);
               //        }
               //    }
               //    StockItemDistributionList = StockItemDistribution;
               //    Session["StockItemDistributionList"] = StockItemDistributionList;
               //}

               //SalesOrder
               if (SalesOrder != null)
               {
                   Salesorderlist = _iSalesOrderService.GetAllActiveSalesOrder();
                   Session["SalesOrderList"] = Salesorderlist;
                   List<SalesOrderItem> SalesOrderItem = new List<SalesOrderItem>();
                   List<InventoryTax> InventoryTaxSalesOrderList = new List<InventoryTax>();
                   foreach (var data in Salesorderlist)
                   {
                       var salesitemlist = _iSalesOrderItemService.GetDetailsBySONoandItemType(data.OrderNo);
                       foreach (var item in salesitemlist)
                       {
                           SalesOrderItem.Add(item);
                       }
                   }
                   Salesorderitemlist = SalesOrderItem;
                   Session["SalesOrderItemList"] = Salesorderitemlist;
                   foreach (var data in Salesorderlist)
                   {
                       InventoryTaxSalesOrder = _iInventoryTaxService.GetTaxesByCode(data.OrderNo);

                       foreach (var item in InventoryTaxSalesOrder)
                       {
                           InventoryTaxSalesOrderList.Add(item);
                       }
                   }
                   InventoryTaxSalesOrder = InventoryTaxSalesOrderList;
                   Session["InventoryTaxSalesOrder"] = InventoryTaxSalesOrder;

               }

               //Retail Bill
               if (RetailBill != null)
               {
                   RetailBillMainTable = _iRetailBillService.GetAllActiveRetailBill();
                   Session["RetailBillMainTable"] = RetailBillMainTable;
                   List<RetailBillItem> RetailBillItemList = new List<RetailBillItem>();
                   List<RetailBillAdjAmtDetail> RetailBillAdjAmtDetailList = new List<RetailBillAdjAmtDetail>();
                   List<InventoryTax> InventoryTaxRetailBillList = new List<InventoryTax>();
                   List<SalesReturn> RetailBillSalesreturnList = new List<SalesReturn>();
                   List<SalesReturnItem> RetailBillSalesreturnItemList = new List<SalesReturnItem>();
                   List<InventoryTax> InventoryTaxSalesReturnList = new List<InventoryTax>();
                   List<RetailBillCreditNote> RetailBillCreditNoteList = new List<RetailBillCreditNote>();
                   List<RetailBillCreditNoteItem> RetailBillCreditNoteItemList = new List<RetailBillCreditNoteItem>();
                   foreach (var data1 in RetailBillMainTable)
                   {
                       RetailBillItem = _iRetailBillItemService.GetItemsByCodeAndType(data1.RetailBillNo);

                       foreach (var item in RetailBillItem)
                       {
                           RetailBillItemList.Add(item);
                       }
                   }

                   RetailBillItem = RetailBillItemList;
                   Session["RetailBillItem"] = RetailBillItem;

                   foreach (var data in RetailBillMainTable)
                   {
                       RetailBillAdjAmtDetail = _iRetailBillAdjustAmountDetailService.GetBillsByRetailNo
                           (data.RetailBillNo);

                       foreach (var item in RetailBillAdjAmtDetail)
                       {
                           RetailBillAdjAmtDetailList.Add(item);
                       }

                   }
                   RetailBillAdjAmtDetail = RetailBillAdjAmtDetailList;
                   Session["RetailBillAdjAmtDetail"] = RetailBillAdjAmtDetail;

                   foreach (var data in RetailBillMainTable)
                   {
                       InventoryTaxRetailBill = _iInventoryTaxService.GetTaxesByCode(data.RetailBillNo);

                       foreach (var item in InventoryTaxRetailBill)
                       {
                           InventoryTaxRetailBillList.Add(item);
                       }

                   }
                   InventoryTaxRetailBill = InventoryTaxRetailBillList;
                   Session["InventoryTaxRetailBill"] = InventoryTaxRetailBill;

                   foreach (var data in RetailBillMainTable)
                   {
                       if (data.SalesReturn == "Yes")
                       {
                           RetailBillSalesreturn = _iSalesReturnService.GetReturnsByBillNo(data.RetailBillNo);
                           Session["RetailBillSalesreturn"] = RetailBillSalesreturn;

                           foreach (var data1 in RetailBillSalesreturn)
                           {
                               RetailBillSalesReturnItem = _iSalesReturnItemService.GetItemListByBillNo(data1.SalesReturnNo);

                               foreach (var item in RetailBillSalesReturnItem)
                               {
                                   RetailBillSalesreturnItemList.Add(item);
                               }
                               InventoryTaxSalesReturn = _iInventoryTaxService.GetTaxesByCode(data1.SalesReturnNo);

                               foreach (var item in InventoryTaxSalesReturn)
                               {
                                   InventoryTaxSalesReturnList.Add(item);
                               }

                           }
                       }
                   }

                   RetailBillSalesReturnItem = RetailBillSalesreturnItemList;
                   Session["RetailBillSalesReturnItem"] = RetailBillSalesReturnItem;

                   InventoryTaxSalesReturn = InventoryTaxSalesReturnList;
                   Session["InventoryTaxSalesReturn"] = InventoryTaxSalesReturn;

                   RetailBillCreditNote = _iRetailBillCreditNoteService.GetActiveCreditNoteDetails();
                   Session["RetailBillCreditNote"] = RetailBillCreditNote;
                   //List<CreditNoteItem> DebitNoteItem = new List<CreditNoteItem>();
                   foreach (var data in RetailBillCreditNote)
                   {
                       RetailBillCreditNoteItem = _iRetailBillCreditNoteItemService.GetItemListByCreditNoteandItemType(data.CreditNoteNo);
                       foreach (var item in RetailBillCreditNoteItem)
                       {
                           RetailBillCreditNoteItemList.Add(item);
                       }
                   }
                   RetailBillCreditNoteItem = RetailBillCreditNoteItemList;
                   Session["RetailBillCreditNoteItem"] = RetailBillCreditNoteItem;
               }

               //Sales Bill
               if (SalesBill != null)
               {
                   SalesBillMainTable = _iSalesBillService.GetAllActiveSalesBill();
                   Session["SalesBillMainTable"] = SalesBillMainTable;
                   List<SalesBillItem> SalesBillItemList = new List<SalesBillItem>();
                   List<SalesBillAdjAmtDetail> SalesBillAdjAmtDetailList = new List<SalesBillAdjAmtDetail>();
                   List<InventoryTax> InventoryTaxSalesBillList = new List<InventoryTax>();
                   List<SalesReturn> SalesBillSalesreturnList = new List<SalesReturn>();
                   List<SalesReturnItem> SalesBillSalesreturnItemList = new List<SalesReturnItem>();
                   List<InventoryTax> InventoryTaxSalesReturnListSalesBill = new List<InventoryTax>();
                   List<SalesBillCreditNote> SalesBillCreditNoteList = new List<SalesBillCreditNote>();
                   List<SalesBillCreditNoteItem> SalesBillCreditNoteItemList = new List<SalesBillCreditNoteItem>();

                   foreach (var data1 in SalesBillMainTable)
                   {
                       SalesBillItem = _iSalesbillItemService.GetItemsByCodeAndType(data1.SalesBillNo);

                       foreach (var item in SalesBillItem)
                       {
                           SalesBillItemList.Add(item);
                       }
                   }

                   SalesBillItem = SalesBillItemList;
                   Session["SalesBillItem"] = SalesBillItem;

                   foreach (var data in SalesBillMainTable)
                   {
                       SalesBillAdjAmtDetail = _IsalesBillAdjsutAmountDetails.GetBillsBySalesNo(data.SalesBillNo);

                       foreach (var item in SalesBillAdjAmtDetail)
                       {
                           SalesBillAdjAmtDetailList.Add(item);
                       }

                   }
                   SalesBillAdjAmtDetail = SalesBillAdjAmtDetailList;
                   Session["SalesBillAdjAmtDetail"] = SalesBillAdjAmtDetail;

                   foreach (var data in SalesBillMainTable)
                   {
                       InventoryTaxSalesBill = _iInventoryTaxService.GetTaxesByCode(data.SalesBillNo);

                       foreach (var item in InventoryTaxSalesBill)
                       {
                           InventoryTaxSalesBillList.Add(item);
                       }

                   }
                   InventoryTaxSalesBill = InventoryTaxSalesBillList;
                   Session["InventoryTaxSalesBill"] = InventoryTaxSalesBill;

                   foreach (var data in SalesBillMainTable)
                   {
                       if (data.SalesReturn == "Yes")
                       {
                           SalesBillSalesreturn = _iSalesReturnService.GetReturnsByBillNo(data.SalesBillNo);
                           Session["SalesBillSalesreturn"] = SalesBillSalesreturn;

                           foreach (var data1 in SalesBillSalesreturn)
                           {
                               SalesBillSalesReturnItem = _iSalesReturnItemService.GetItemListByBillNo(data1.SalesReturnNo);

                               foreach (var item in SalesBillSalesReturnItem)
                               {
                                   SalesBillSalesreturnItemList.Add(item);
                               }


                               InventoryTaxSalesReturnSalesBill = _iInventoryTaxService.GetTaxesByCode(data1.SalesReturnNo);

                               foreach (var item in InventoryTaxSalesReturnSalesBill)
                               {
                                   InventoryTaxSalesReturnListSalesBill.Add(item);
                               }

                           }
                       }
                   }

                   SalesBillSalesReturnItem = SalesBillSalesreturnItemList;
                   Session["SalesBillSalesReturnItem"] = SalesBillSalesReturnItem;

                   InventoryTaxSalesReturnSalesBill = InventoryTaxSalesReturnListSalesBill;
                   Session["InventoryTaxSalesReturn"] = InventoryTaxSalesReturnSalesBill;

                   SalesBillCreditNote = _iSalesBillCreditNoteService.GetActiveSalesBillCreditNoteDetails();
                   Session["SalesBillCreditNote"] = SalesBillCreditNote;

                   foreach (var data in SalesBillCreditNote)
                   {
                       SalesBillCreditNoteItem = _iSalesBillCreditNoteItemService.GetItemListByCreditNoteandItemType(data.CreditNoteNo);
                       foreach (var item in SalesBillCreditNoteItem)
                       {
                           SalesBillCreditNoteItemList.Add(item);
                       }
                   }
                   SalesBillCreditNoteItem = SalesBillCreditNoteItemList;
                   Session["SalesBillCreditNoteItem"] = SalesBillCreditNoteItem;

               }
               //BalanceCarryForward
               if (BalanceCarryForward != null)
               {
                   BalanceCarryForwardEntity = _iBalanceCarryForwardService.GetPendingBalances();
                   Session["BalanceCarryForwardEntity"] = BalanceCarryForwardEntity;
               }

               //CashierSalesOrder
               if (CashierSalesOrder != null)
               {
                   ActiveSalesOrder = _iSalesOrderService.GetAllActiveSalesOrder();
                   List<CashierSalesOrder> CashierSalesOrderList = new List<CashierSalesOrder>();
                   List<CashierSalesOrderItem> CashierSalesOrderItemList = new List<CashierSalesOrderItem>();
                   List<CashierSalesOrder> CashierSalesOrderHandOverList = new List<CashierSalesOrder>();
                   List<CashierSalesOrderItem> CashierSalesOrderHandOverItemList = new List<CashierSalesOrderItem>();
                   foreach (var data in ActiveSalesOrder)
                   {

                       CashierSalesOrderEnum = _iCashierSalesOrderService.GetActivecashierSalesorder(data.OrderNo);
                       foreach (var item1 in CashierSalesOrderEnum)
                       {
                           CashierSalesOrderList.Add(item1);
                       }
                   }
                   CashierSalesOrderEnum = CashierSalesOrderList;
                   Session["CashierSalesOrderEnum"] = CashierSalesOrderEnum;

                   foreach (var item in CashierSalesOrderEnum)
                   {
                       CashierSalesorderItem = _iCashierSalesOrderItemService.GetDetailsByCashierCode(item.CashierCode);

                       foreach (var item1 in CashierSalesorderItem)
                       {
                           CashierSalesOrderItemList.Add(item1);
                       }
                   }

                   CashierSalesorderItem = CashierSalesOrderItemList;
                   Session["CashierSalesorderItem"] = CashierSalesorderItem;

                   CashierSalesOrderCardChequeHandOver = _iCashierSalesOrderService.GetHandoverCashiers();

                   foreach (var item in CashierSalesOrderCardChequeHandOver)
                   {
                       CashierSalesOrderHandOverList.Add(item);
                   }

                   CashierSalesOrderCardChequeHandOver = CashierSalesOrderHandOverList;
                   Session["CashierSalesOrderCardChequeHandOver"] = CashierSalesOrderCardChequeHandOver;

                   foreach (var item in CashierSalesOrderCardChequeHandOver)
                   {
                       CashierSalesorderItemCardChequeHandOver = _iCashierSalesOrderItemService.GetDetailsByCashierCode(item.CashierCode);
                       foreach (var item1 in CashierSalesorderItemCardChequeHandOver)
                       {
                           CashierSalesOrderHandOverItemList.Add(item1);
                       }

                   }
                   CashierSalesorderItemCardChequeHandOver = CashierSalesOrderHandOverItemList;
                   Session["CashierSalesorderItemCardChequeHandOver"] = CashierSalesorderItemCardChequeHandOver;
               }

               //CashierRetailBill
               if (CashierRetailBill != null)
               {
                   ActiveRetailBill = _iRetailBillService.GetAllActiveRetailBill();
                   List<CashierRetailBill> CashierRetailBillList = new List<CashierRetailBill>();
                   List<CashierRetailBillItem> CashierRetailBillItemList = new List<CashierRetailBillItem>();
                   List<CashierRetailBill> CashierRetailBillHandOverList = new List<CashierRetailBill>();
                   List<CashierRetailBillItem> CashierRetailBillHandOverItemList = new List<CashierRetailBillItem>();
                   foreach (var data in ActiveRetailBill)
                   {

                       CashierRetailBillEnum = _iCashierRetailBillService.GetActivecashierRetailbill(data.RetailBillNo);
                       foreach (var item1 in CashierRetailBillEnum)
                       {
                           CashierRetailBillList.Add(item1);
                       }
                   }
                   CashierRetailBillEnum = CashierRetailBillList;
                   Session["CashierRetailBillEnum"] = CashierRetailBillEnum;

                   foreach (var item in CashierRetailBillEnum)
                   {
                       CashierRetailBillItem = _iCashierRetailBillItemService.GetDetailsByCashierCode(item.CashierCode);

                       foreach (var item1 in CashierRetailBillItem)
                       {
                           CashierRetailBillItemList.Add(item1);
                       }
                   }

                   CashierRetailBillItem = CashierRetailBillItemList;
                   Session["CashierRetailBillItem"] = CashierRetailBillItem;

                   CashierRetailBillCardChequeHandOver = _iCashierRetailBillService.GetHandoverRetailCashier();

                   foreach (var item in CashierRetailBillCardChequeHandOver)
                   {
                       CashierRetailBillHandOverList.Add(item);
                   }

                   CashierRetailBillCardChequeHandOver = CashierRetailBillHandOverList;
                   Session["CashierRetailBillCardChequeHandOver"] = CashierRetailBillCardChequeHandOver;

                   foreach (var item in CashierRetailBillCardChequeHandOver)
                   {
                       CashierRetailBillItemCardChequeHandOver = _iCashierRetailBillItemService.GetDetailsByCashierCode(item.CashierCode);
                       foreach (var item1 in CashierRetailBillItemCardChequeHandOver)
                       {
                           CashierRetailBillHandOverItemList.Add(item1);
                       }

                   }
                   CashierRetailBillItemCardChequeHandOver = CashierRetailBillHandOverItemList;
                   Session["CashierRetailBillItemCardChequeHandOver"] = CashierRetailBillItemCardChequeHandOver;
               }

               //CashierSalesBill
               if (CashierSalesBill != null)
               {
                   ActiveSalesBill = _iSalesBillService.GetAllActiveSalesBill();
                   List<CashierSalesBill> CashierSalesBillList = new List<CashierSalesBill>();
                   List<CashierSalesBillItem> CashierSalesBillItemList = new List<CashierSalesBillItem>();
                   List<CashierSalesBill> CashierSalesBillHandOverList = new List<CashierSalesBill>();
                   List<CashierSalesBillItem> CashierSalesBillHandOverItemList = new List<CashierSalesBillItem>();
                   foreach (var data in ActiveSalesBill)
                   {

                       CashierSalesBillEnum = _iCashierSalesBillService.GetActivecashierSalesbill(data.SalesBillNo);
                       foreach (var item1 in CashierSalesBillEnum)
                       {
                           CashierSalesBillList.Add(item1);
                       }
                   }
                   CashierSalesBillEnum = CashierSalesBillList;
                   Session["CashierSalesBillEnum"] = CashierSalesBillEnum;

                   foreach (var item in CashierSalesBillEnum)
                   {
                       CashierSalesBillItem = _iCashierSalesBillItemService.GetDetailsByCashierCode(item.CashierCode);

                       foreach (var item1 in CashierSalesBillItem)
                       {
                           CashierSalesBillItemList.Add(item1);
                       }
                   }

                   CashierSalesBillItem = CashierSalesBillItemList;
                   Session["CashierSalesBillItem"] = CashierSalesBillItem;

                   CashierSalesBillCardChequeHandOver = _iCashierSalesBillService.GetHandoverSalesBillCashier();

                   foreach (var item in CashierSalesBillCardChequeHandOver)
                   {
                       CashierSalesBillHandOverList.Add(item);
                   }
                   CashierSalesBillCardChequeHandOver = CashierSalesBillHandOverList;
                   Session["CashierSalesBillCardChequeHandOver"] = CashierSalesBillCardChequeHandOver;

                   foreach (var item in CashierSalesBillCardChequeHandOver)
                   {
                       CashierSalesBillItemCardChequeHandOver = _iCashierSalesBillItemService.GetDetailsByCashierCode(item.CashierCode);
                       foreach (var item1 in CashierSalesBillItemCardChequeHandOver)
                       {
                           CashierSalesBillHandOverItemList.Add(item1);
                       }
                   }
                   CashierSalesBillItemCardChequeHandOver = CashierSalesBillHandOverItemList;
                   Session["CashierSalesBillItemCardChequeHandOver"] = CashierSalesBillItemCardChequeHandOver;
               }

               //ShopRequisition
               if (ShopRequisition != null)
               {
                   RequisitionForShop = _iRequisitionForShopService.getAllActiveRequisitions();
                   Session["RequisitionForShop"] = RequisitionForShop;
               }

               //GodownRequisition
               if (GodownRequisition != null)
               {
                   RequisitionForGodown = _iRequisitionForGodownService.getAllActiveRequisitions();
                   Session["RequisitionForGodown"] = RequisitionForGodown;
               }

               //Inward From Tailor
               if (InwardFromTailor != null)
               {
                   InwardFromTailorEnum = _iInwardFromTailorService.GetActiveTailor();
                   Session["InwardFromTailorEnum"] = InwardFromTailorEnum;
                   List<InwardFromTailorItem> InwardFromTailorItemList = new List<InwardFromTailorItem>();

                   foreach (var data in InwardFromTailorEnum)
                   {
                       InwardFromTailorItemEnum = _iInwardfromTailorItemService.GetActiveTailorItems(data.InwardCode);
                       foreach (var item in InwardFromTailorItemEnum)
                       {
                           InwardFromTailorItemList.Add(item);
                       }
                   }
                   InwardFromTailorItemEnum = InwardFromTailorItemList;
                   Session["InwardFromTailorItemEnum"] = InwardFromTailorItemEnum;
               }

               //Temporary Cash Memo
               if (TemporaryCashMemo != null)
               {
                   TCM = _iTemporaryCashMemoService.GetAllActiveTemporaryCashMemo();
                   Session["TCM"] = TCM;
                   List<TemporaryCashMemoItem> TCMItemList = new List<TemporaryCashMemoItem>();
                   List<TemporaryCashMemoAdjAmtDetail> TCMAdjAmtDetailList = new List<TemporaryCashMemoAdjAmtDetail>();
                   List<InventoryTax> InventoryTaxTCMList = new List<InventoryTax>();

                   foreach (var data1 in TCM)
                   {
                       TCMItem = _iTemporaryCashMemoItemService.GetItemsByCodeAndType(data1.TempCashMemoNo);

                       foreach (var item in TCMItem)
                       {
                           TCMItemList.Add(item);
                       }
                   }

                   TCMItem = TCMItemList;
                   Session["TCMItem"] = TCMItem;

                   foreach (var data in TCMItem)
                   {
                       TCMAdjAmtDetail = _ITemporaryCashMemoAdjustAmountDetailService.GetBillsByTemporaryCashMemoNo(data.TempCashMemolNo);

                       foreach (var item in TCMAdjAmtDetail)
                       {
                           TCMAdjAmtDetailList.Add(item);
                       }

                   }
                   TCMAdjAmtDetail = TCMAdjAmtDetailList;
                   Session["TCMAdjAmtDetail"] = TCMAdjAmtDetail;

                   foreach (var data in TCMAdjAmtDetail)
                   {
                       InventoryTaxTCM = _iInventoryTaxService.GetTaxesByCode(data.TempCashMemoNo);

                       foreach (var item in InventoryTaxTCM)
                       {
                           InventoryTaxTCMList.Add(item);
                       }

                   }
                   InventoryTaxTCM = InventoryTaxTCMList;
                   Session["InventoryTaxTCM"] = InventoryTaxTCM;
               }

               //CashierTCM
               if (CashierTemporaryCashMemo != null)
               {
                   ActiveTemporaryCashMemo = _iTemporaryCashMemoService.GetAllActiveTemporaryCashMemo();
                   List<CashierTemporaryCashMemo> CashierTemporaryCashMemoList = new List<CashierTemporaryCashMemo>();
                   List<CashierTemporaryCashMemoItem> CashierTemporaryCashMemoItemList = new List<CashierTemporaryCashMemoItem>();
                   List<CashierTemporaryCashMemo> CashierTemporaryCashMemoHandOverList = new List<CashierTemporaryCashMemo>();
                   List<CashierTemporaryCashMemoItem> CashierTemporaryCashMemoHandOverItemList = new List<CashierTemporaryCashMemoItem>();
                   foreach (var data in ActiveTemporaryCashMemo)
                   {

                       CashierTemporaryCashMemoEnum = _iCashierTemporaryCashMemoService.GetActivecashierTemporaryCashMemo(data.TempCashMemoNo);
                       foreach (var item1 in CashierTemporaryCashMemoEnum)
                       {
                           CashierTemporaryCashMemoList.Add(item1);
                       }
                   }
                   CashierTemporaryCashMemoEnum = CashierTemporaryCashMemoList;
                   Session["CashierTemporaryCashMemoEnum"] = CashierTemporaryCashMemoEnum;

                   foreach (var item in CashierTemporaryCashMemoEnum)
                   {
                       CashierTemporaryCashMemoItem = _iCashierTemporarayCashMemoItemService.GetDetailsByCashierCode(item.CashierCode);
                       foreach (var item1 in CashierTemporaryCashMemoItem)
                       {
                           CashierTemporaryCashMemoItemList.Add(item1);
                       }
                   }

                   CashierTemporaryCashMemoItem = CashierTemporaryCashMemoItemList;
                   Session["CashierTemporaryCashMemoItem"] = CashierTemporaryCashMemoItem;

                   CashierTemporaryCashMemoCardChequeHandOver = _iCashierTemporaryCashMemoService.GetHandoverTemporaryCashMemoService();

                   foreach (var item in CashierTemporaryCashMemoCardChequeHandOver)
                   {
                       CashierTemporaryCashMemoHandOverList.Add(item);
                   }
                   CashierTemporaryCashMemoCardChequeHandOver = CashierTemporaryCashMemoHandOverList;
                   Session["CashierTemporaryCashMemoCardChequeHandOver"] = CashierTemporaryCashMemoCardChequeHandOver;

                   foreach (var item in CashierTemporaryCashMemoCardChequeHandOver)
                   {
                       CashierTemporaryCashMemoItemCardChequeHandOver = _iCashierTemporarayCashMemoItemService.GetDetailsByCashierCode(item.CashierCode);
                       foreach (var item1 in CashierTemporaryCashMemoItemCardChequeHandOver)
                       {
                           CashierTemporaryCashMemoHandOverItemList.Add(item1);
                       }
                   }
                   CashierTemporaryCashMemoItemCardChequeHandOver = CashierTemporaryCashMemoHandOverItemList;
                   Session["CashierTemporaryCashMemoItemCardChequeHandOver"] = CashierTemporaryCashMemoItemCardChequeHandOver;
               }

               //JobWorkPayments
               if (JobWorkPayments != null)
               {
                   JobWorkPaymentEnum = _iJobWorkPaymentService.GetHandoverJobWorkPayment();
                   Session["JobWorkPayment"] = JobWorkPaymentEnum;
               }

              //DiscountMaster
              string year = Session["fyear"].ToString();
             //  string[] yr = year.Split(' ', '-');
             //string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

               FinancialYear = frm["dropdownvalue"];
               string[] words = year.Split(' ','-','T','O');
               string fyfrom = words[1] + "/" + words[0] + "/" + words[2];
               string fyto = words[6] + "/" + words[5] + "/" + words[7];

               DateTime? fromdate = Convert.ToDateTime(fyfrom);
               //var systemdate1 = "1-";
               if (Discount != null)
               {
                   DiscountMaster = _iDiscountMasterService.getItemsByDiscount(fromdate);
                   Session["DiscountMaster"] = DiscountMaster;
                   List<DiscountMasterItem> DiscountMasterItemList = new List<DiscountMasterItem>();

                   foreach (var data in DiscountMaster)
                   {
                       DiscountMasterItem = _iDiscountMasterItemservice.GetRowsByCode(data.DiscountCode);

                       foreach (var item in DiscountMasterItem)
                       {
                           DiscountMasterItemList.Add(item);
                       }

                   }
                   DiscountMasterItem = DiscountMasterItemList;
                   Session["DiscountMasterItem"] = DiscountMasterItem;
               }

                //cost code creation 
               if (costcodecreation !=null)
               {
                   costcodecreationenum = _icostcodecreation.GetActiveData();
                   Session["costcodecreation"] = costcodecreationenum;
               }

              //outward to tailor
               //if (outwardtotailor != null)
               //{
               //    OutwardToTailorenum = _ioutwardtotailorservice.GetPendingOutwardToTailors();
               //    Session["OutwardToTailor"] = OutwardToTailorenum;
               //    List<OutwardToTailorItem> outwardtotailordetail = new List<OutwardToTailorItem>();

               //    foreach (var data in OutwardToTailorenum)
               //    {
               //        OutwardToTailorItemenum = _iOutwardToTailorItemService.GetRowsByCode(data.OutwardCode);

               //        foreach (var item in OutwardToTailorItemenum)
               //        {
               //            outwardtotailordetail.Add(item);
               //        }

               //    }
               //    OutwardToTailorItemenum = outwardtotailordetail;
               //    Session["OutwardToTailorItem"] = OutwardToTailorItemenum;
               //}

            //inward from tailor
               ////if (inwardfromtailor != null)
               ////{
               ////    InwardFromTailorenum = _iinwardfromtailorservice.GetActiveTailor();
               ////    Session["InwardFromTailor"] = InwardFromTailorenum;
               ////    List<InwardFromTailorItem> InwardFromTailorItem = new List<InwardFromTailorItem>();

               ////    foreach (var data in InwardFromTailorenum)
               ////    {
               ////        InwardFromTailorItemenum = _iInwardfromTailorItemService.GetActiveTailorItemsByCode(data.InwardCode);

               ////        foreach (var item in InwardFromTailorItemenum)
               ////        {
               ////            InwardFromTailorItem.Add(item);
               ////        }

               ////    }
               ////    InwardFromTailorItemenum = InwardFromTailorItem;
               ////    Session["InwardFromTailorItem"] = InwardFromTailorItemenum;
               ////}


               //sent items to tailor
               if (sentitemstotailor != null)
               {
                   InwardToTailorenum = _iinwardtotailorservice.GetActiveTailor();
                   Session["InwardToTailor"] = InwardToTailorenum;
                   List<InwardToTailorItem> InwardToTailorItem = new List<InwardToTailorItem>();

                   foreach (var data in InwardToTailorenum)
                   {
                       InwardToTailorItemenum = _iinwardtotailoritemservice.GetActiveTailorItemsByCode(data.InwardCode);

                       foreach (var item in InwardToTailorItemenum)
                       {
                           InwardToTailorItem.Add(item);
                       }

                   }
                   InwardToTailorItemenum = InwardToTailorItem;
                   Session["InwardToTailorItem"] = InwardToTailorItemenum;
               }


               //tailor stock
               if (tailorstock != null)
               {
                   JobWorkStockenum = _iJobWorkStockService.GetActiveStock();
                   Session["JobWorkStock"] = JobWorkStockenum;
               }

              //outwrad items to client
               if (outwarditemstoclient != null)
               {
                   JobWorkOutwardToClientenum = _ijobworkoutwardtoclient.GetActiveOutwardToClients();
                   Session["JobWorkOutwardToClient"] = JobWorkOutwardToClientenum;
               }


               DatabaseName = frm["dropdownvalue"];
               CompanyName = frm["dropdownvalue"];
               FinancialYear = frm["dropdownvalue"];
               //PurchaseOrderDetail  poincomplete = Convert(purchaseoredrdetailsforincomplete);
               return RedirectToAction("InsertData");
         
        }


        [HttpGet]
        public ActionResult InsertData()
        {
            var polist = Session["PurchaseOrderDetail"] as IEnumerable<PurchaseOrderDetail>;
            var poitemlist = Session["PurchaseItemDetail"] as IEnumerable<PurchaseItemDetail>;
            var InventoryTaxpurchaseOrder = Session["InventoryTaxpurchaseOrder"] as IEnumerable<InventoryTax>;

            var inwardfromsupplierlist = Session["InwardFromSupplier"] as IEnumerable<InwardFromSupplier>;
            var inwarditemfromsupplierlist = Session["InwardItemsFromSupplier"] as IEnumerable<InwardItemsFromSupplier>;
            var InventoryTaxInwardFromSupplier = Session["InventoryTaxInwardFromSupplier"] as IEnumerable<InventoryTax>;
            var DebitNoteList = Session["DebitNote"] as IEnumerable<DebitNote>;
            var DebitNoteItemList = Session["DebitNoteItemList"] as IEnumerable<DebitNoteItem>;

            var InwardWithoutPo = Session["InwardWithoutPo"] as IEnumerable<InwardFromSupplier>;
            var InwardWithoutPoItems = Session["InwardWithoutPoItems"] as IEnumerable<InwardItemsFromSupplier>;
            var InventoryTaxInwardWithoutPo = Session["InventoryTaxInventoryTaxInwardWithoutPo"] as IEnumerable<InventoryTax>;

            var Quotationlist = Session["Quotation"] as IEnumerable<Quotation>;
            var QuotationItemList = Session["QuotationItem"] as IEnumerable<QuotationItem>;
            var InventoryTaxQuotation = Session["InventoryTaxQuotation"] as IEnumerable<InventoryTax>;

            var DeliveryChallanList = Session["DeliveryChallan"] as IEnumerable<DeliveryChallan>;
            var DeliveryChallanItemList = Session["DeliveryChallanItemList"] as IEnumerable<DeliveryChallanItem>;
            var InventoryTaxDelivaryChallan = Session["InventoryTaxDelivaryChallan"] as IEnumerable<InventoryTax>;

            var openingstocklist = Session["OpeningSockValue"] as IEnumerable<OpeningStockMaster>;
            var entrystocklist = Session["EntryStockMaster"] as IEnumerable<EntryStockMaster>;
            var entrystockitem = Session["EntryStockItem"] as IEnumerable<EntryStockItem>;
            var ShopStockItem = Session["ShopStockItem"] as IEnumerable<ShopStock>;
            var GodownStockItem = Session["GodownStockItem"] as IEnumerable<GodownStock>;
            var StockDistribution = Session["StockDistributionList"] as IEnumerable<StockDistribution>;
            var StockItemDistribution = Session["StockItemDistributionList"] as IEnumerable<StockItemDistribution>;

            var OutwardToTailor = Session["OutwardToTailor"] as IEnumerable<OutwardToTailor>;
            var OutwardToTailorItemList = Session["OutwardToTailorItemList"] as IEnumerable<OutwardToTailorItem>;
           
            var SalesOrderList = Session["SalesOrderList"] as IEnumerable<SalesOrder>;
            var SalesOrderItemList = Session["SalesOrderItemList"] as IEnumerable<SalesOrderItem>;
            var InventoryTaxSalesOrder = Session["InventoryTaxSalesOrder"] as IEnumerable<InventoryTax>;

            var RetailBillMainTable = Session["RetailBillMainTable"] as IEnumerable<RetailBill>;
            var RetailBillItem = Session["RetailBillItem"] as IEnumerable<RetailBillItem>;
            var RetailBillAdjAmtDetail = Session["RetailBillAdjAmtDetail"] as IEnumerable<RetailBillAdjAmtDetail>;
            var InventoryTaxRetailBill = Session["InventoryTaxRetailBill"] as IEnumerable<InventoryTax>;
            var RetailBillSalesreturn = Session["RetailBillSalesreturn"] as IEnumerable<SalesReturn>;
            var RetailBillSalesReturnItem = Session["RetailBillSalesReturnItem"] as IEnumerable<SalesReturnItem>;
            var InventoryTaxSalesReturn = Session["InventoryTaxSalesReturn"] as IEnumerable<InventoryTax>;
            var RetailBillCreditNote = Session["RetailBillCreditNote"] as IEnumerable<RetailBillCreditNote>;
            var RetailBillCreditNoteItem = Session["RetailBillCreditNoteItem"] as IEnumerable<RetailBillCreditNoteItem>;

            var BalanceCarryForwardEntity = Session["BalanceCarryForwardEntity"] as IEnumerable<BalanceCarryForward>;

            var SalesBillMainTable = Session["SalesBillMainTable"] as IEnumerable<SalesBill>;
            var SalesBillItem = Session["SalesBillItem"] as IEnumerable<SalesBillItem>;
            var SalesBillAdjAmtDetail = Session["SalesBillAdjAmtDetail"] as IEnumerable<SalesBillAdjAmtDetail>;
            var InventoryTaxSalesBill = Session["InventoryTaxSalesBill"] as IEnumerable<InventoryTax>;
            var SalesBillSalesreturn = Session["SalesBillSalesreturn"] as IEnumerable<SalesReturn>;
            var SalesBillSalesReturnItem = Session["SalesBillSalesReturnItem"] as IEnumerable<SalesReturnItem>;
            var InventoryTaxSalesReturnSalesBill = Session["InventoryTaxSalesReturn"] as IEnumerable<InventoryTax>;
            var SalesBillCreditNote = Session["SalesBillCreditNote"] as IEnumerable<SalesBillCreditNote>;
            var SalesBillCreditNoteItem = Session["SalesBillCreditNoteItem"] as IEnumerable<SalesBillCreditNoteItem>;

            var CashierSalesOrderEnum = Session["CashierSalesOrderEnum"] as IEnumerable<CashierSalesOrder>;
            var CashierSalesorderItem = Session["CashierSalesorderItem"] as IEnumerable<CashierSalesOrderItem>;
            var CashierSalesOrderCardChequeHandOver = Session["CashierSalesOrderCardChequeHandOver"] as IEnumerable<CashierSalesOrder>;
            var CashierSalesorderItemCardChequeHandOver = Session["CashierSalesorderItemCardChequeHandOver"] as IEnumerable<CashierSalesOrderItem>;

            var CashierRetailBillEnum = Session["CashierRetailBillEnum"] as IEnumerable<CashierRetailBill>;
            var CashierRetailBillItem = Session["CashierRetailBillItem"] as IEnumerable<CashierRetailBillItem>;
            var CashierRetailBillCardChequeHandOver = Session["CashierRetailBillCardChequeHandOver"] as IEnumerable<CashierRetailBill>;
            var CashierRetailBillItemCardChequeHandOver = Session["CashierRetailBillItemCardChequeHandOver"] as IEnumerable<CashierRetailBillItem>;

            var CashierSalesBillEnum = Session["CashierSalesBillEnum"] as IEnumerable<CashierSalesBill>;
            var CashierSalesBillItem = Session["CashierSalesBillItem"] as IEnumerable<CashierSalesBillItem>;
            var CashierSalesBillCardChequeHandOver = Session["CashierSalesBillCardChequeHandOver"] as IEnumerable<CashierSalesBill>;
            var CashierSalesBillItemCardChequeHandOver = Session["CashierSalesBillItemCardChequeHandOver"] as IEnumerable<CashierSalesBillItem>;

            var RequisitionForShop = Session["RequisitionForShop"] as IEnumerable<RequisitionForShop>;
            var RequisitionForGodown = Session["RequisitionForGodown"] as IEnumerable<RequisitionForGodown>;

            var InwardFromTailor= Session["InwardFromTailorEnum"] as IEnumerable<InwardFromTailor>;
            var InwardFromTailorItem = Session["InwardFromTailorItemEnum"] as IEnumerable<InwardFromTailorItem>;

            var TCM = Session["TCM"] as IEnumerable<TemporaryCashMemo>;
            var TCMItem = Session["TCMItem"] as IEnumerable<TemporaryCashMemoItem>;
            var TCMAdjAmtDetail = Session["TCMAdjAmtDetail"] as IEnumerable<TemporaryCashMemoAdjAmtDetail>;
            var InventoryTaxTCM = Session["InventoryTaxTCM"] as IEnumerable<InventoryTax>;

            var CashierTemporaryCashMemoEnum = Session["CashierTemporaryCashMemoEnum"] as IEnumerable<CashierTemporaryCashMemo>;
            var CashierTemporaryCashMemoItem = Session["CashierTemporaryCashMemoItem"] as IEnumerable<CashierTemporaryCashMemoItem>;
            var CashierTemporaryCashMemoCardChequeHandOver = Session["CashierTemporaryCashMemoCardChequeHandOver"] as IEnumerable<CashierTemporaryCashMemo>;
            var CashierTemporaryCashMemoItemCardChequeHandOver = Session["CashierTemporaryCashMemoItemCardChequeHandOver"] as IEnumerable<CashierTemporaryCashMemoItem>;

            var JobWorkPayment = Session["JobWorkPayment"] as IEnumerable<JobWorkPayment>;

            var DiscountMaster = Session["DiscountMaster"] as IEnumerable<DiscountMaster>;
            var DiscountMasterItem = Session["DiscountMasterItem"] as IEnumerable<DiscountMasterItem>;

            var costcodecreation = Session["costcodecreation"] as IEnumerable<CostCodeCreation>;

           // var OutwardToTailor = Session["OutwardToTailor"] as IEnumerable<OutwardToTailor>;
           // var OutwardToTailorItem = Session["OutwardToTailorItem"] as IEnumerable<OutwardToTailorItem>;

           // var InwardFromTailor = Session["InwardFromTailor"] as IEnumerable<InwardFromTailor>;
           // var costcodecreation = Session["InwardFromTailorItem"] as IEnumerable<InwardFromTailorItem>;

            var InwardToTailor = Session["InwardToTailor"] as IEnumerable<InwardToTailor>;
            var InwardToTailorItem = Session["InwardToTailorItem"] as IEnumerable<InwardToTailorItem>;

            var JobWorkStock = Session["JobWorkStock"] as IEnumerable<JobWorkStock>;

            var JobWorkOutwardToClient = Session["JobWorkOutwardToClient"] as IEnumerable<JobWorkOutwardToClient>;

            //Purchase Order
            if (polist != null && poitemlist != null)
            {
                foreach (var data in polist)
                {
                    data.modifiedOn = DateTime.Now;
                    _ipurchaseorderdetails.CreatePurchaseOrder(data);
                }
                foreach (var data1 in poitemlist)
                {
                    _iPurchaseItemService.CreatePurchaseItemDetail(data1);
                }
                foreach (var data2 in InventoryTaxpurchaseOrder)
                {
                    _iInventoryTaxService.Create(data2);
                }
            }

            //InwardFromSupplier
            if (inwardfromsupplierlist != null)
            {
                foreach (var data1 in inwardfromsupplierlist)
                {
                    _iInwardFromSupplierService.CreateInward(data1);
                }
                foreach (var data2 in inwarditemfromsupplierlist)
                {
                    _iInwardItemFryuomSupplierService.CreateInwardItems(data2);
                }

                foreach (var data2 in InventoryTaxInwardFromSupplier)
                {
                    _iInventoryTaxService.Create(data2);
                }
            }
            if (DebitNoteList != null)
            {
                foreach (var data in DebitNoteList)
                {
                    _iDebitNoteService.Create(data);
                }

                foreach (var data1 in DebitNoteItemList)
                {
                    _iDebitNoteItemService.Create(data1);
                }
            }

            //Inward Without PO
            if (InwardWithoutPo != null)
            {
                foreach (var data1 in InwardWithoutPo)
                {
                    _iInwardFromSupplierService.CreateInward(data1);
                }
                foreach (var data2 in InwardWithoutPoItems)
                {
                    _iInwardItemFryuomSupplierService.CreateInwardItems(data2);
                }

                foreach (var data2 in InventoryTaxInwardWithoutPo)
                {
                    _iInventoryTaxService.Create(data2);
                }
            }

            //Quotation
            if (Quotationlist != null)
            {
                foreach (var data in Quotationlist)
                {
                    _iQuotationService.Create(data);
                }
                foreach (var data1 in QuotationItemList)
                {
                    _iQuotationItemService.Create(data1);
                }
                foreach (var data2 in InventoryTaxQuotation)
                {
                    _iInventoryTaxService.Create(data2);
                }
            }

            //DelivaryChallan
            if (DeliveryChallanList != null)
            {
                foreach (var data in DeliveryChallanList)
                {
                    _iDeliveryChallanService.Create(data);
                }
                foreach (var data1 in DeliveryChallanItemList)
                {
                    _iDelivaryChallanItemService.Create(data1);
                }
                foreach (var data2 in InventoryTaxDelivaryChallan)
                {
                    _iInventoryTaxService.Create(data2);
                }
            }

            //OpeningStock 
            if (openingstocklist != null)
            {
                foreach (var data in openingstocklist)
                {
                    _iOpeningStockService.CreateStock(data);
                } 

                OpeningStockMaster op = new OpeningStockMaster();
                string year = Session["fyear"].ToString();
                string[] yr = year.Split(' ', '-');
               // string FinYr = "/" + openingstockdata.OpeningStockCode.Substring(2) + "-" + openingstockdata.OpeningStockCode.Substring(2);
                //var openingstockdata = _iOpeningStockService.GetLastRow();
                var finyr=string.Empty;
                    if (entrystocklist != null)
                    {
                        //foreach (var data1 in entrystocklist)
                        //{
                            foreach (var data2 in entrystockitem)
                            {
                                string openingstockcode = string.Empty;
                                string openingstockcode1 = string.Empty;
                                string splityear = string.Empty;

                                int openstockVal = 0; 
                               
                                int length = 0;
                                var openingstockdata = _iOpeningStockService.GetLastRow();
                                if (openingstockdata != null)
                                {
                                    openingstockcode = openingstockdata.OpeningStockCode.Substring(3, 6);
                                   //  finyr = openingstockdata.OpeningStockCode.Substring(7, 11);
                                     finyr =openingstockdata.OpeningStockCode.Substring(9);
                                    openingstockcode1 = openingstockdata.OpeningStockCode;
                                    length = (Convert.ToInt32(openingstockcode) + 1).ToString().Length;
                                    openstockVal = Convert.ToInt32(openingstockcode) + 1;
                                }

                                else
                                {
                                    openstockVal = 1;
                                    length = 1;
                                }

                                openingstockcode = _iUtlityService.getName("OST", length, openstockVal);
                                openingstockcode = openingstockcode + finyr;
                                op.OpeningStockCode = openingstockcode;
                                op.Category = data2.Category;
                                op.SubCategory = data2.SubCategory;
                                op.ItemName = data2.Item;
                                op.itemCode = data2.ItemCode;
                                op.Description = data2.Description;
                                op.ItemQuantity = data2.TotalQuantity;
                                op.Size = data2.Size;
                                op.Unit = data2.Unit;
                                op.NumberType = data2.NumberType;
                                op.TotalQuantity = data2.TotalQuantity;
                                op.Color = data2.Color;
                                op.Material = data2.Material;
                                op.DesignCode = data2.DesignCode;
                                op.DesignName = data2.DesignName;
                                op.Brand = data2.Brand;
                                op.CostPrice = data2.CostPrice;
                                op.SellingPrice = data2.SellingPrice;
                                op.MRP = data2.MRP;
                                op.Barcode = data2.Barcode;
                                op.Status = data2.Status;
                                op.ModifiedOn = data2.ModifiedOn;
                                _iOpeningStockService.CreateStock(op);
                            }
                        }
                    //}

                //ShopStock
                if (ShopStockItem != null)
                {
                    foreach (var data in ShopStockItem)
                    {
                        _iShopStockService.Create(data);
                    }
                }

                //GodownStock
                if (GodownStockItem != null)
                {
                    foreach (var data1 in GodownStockItem)
                    {
                        _iGodownStockService.Create(data1);
                    }
                }

                //StockDistribution
                if (StockDistribution != null)
                {
                    foreach (var data in StockDistribution)
                    {
                        _iStockDistributionService.Create(data);
                    }
                    foreach (var data1 in StockItemDistribution)
                    {
                        _iStockItemDistributionService.Create(data1);
                    }
                }
            }

            //Outward To Tailor
            if (OutwardToTailor != null)
            {
                foreach (var data in OutwardToTailor)
                {
                    _iOutwardToTailorService.Create(data);
                }
                foreach (var data1 in OutwardToTailorItemList)
                {
                    _iOutwardToTailorItemService.Create(data1);
                }
            }

            //Entry Stock
            //if (entrystocklist != null)
            //{
            //    foreach (var data in entrystocklist)
            //    {
            //        _iEntryStockService.Create(data);
            //    }
            //    foreach (var data1 in entrystockitem)
            //    {
            //        _iEntryStockItemService.Create(data1);
            //    }
            //}


            //Stock Distribution
            //if (StockDistribution != null)
            //{
            //    foreach (var data in StockDistribution)
            //    {
            //        _iStockDistributionService.Create(data);
            //    }
            //    foreach (var data1 in StockItemDistribution)
            //    {
            //        _iStockItemDistributionService.Create(data1);
            //    }
            //}

            //Sales Order
            if (SalesOrderList != null && SalesOrderItemList != null)
            {
                foreach (var data in SalesOrderList)
                {
                    _iSalesOrderService.Create(data);
                }

                foreach (var data1 in SalesOrderItemList)
                {
                    _iSalesOrderItemService.Create(data1);
                }

                foreach (var data1 in InventoryTaxSalesOrder)
                {
                    _iInventoryTaxService.Create(data1);
                }
            }

            //Retail Bill
            if (RetailBillMainTable != null)
            {
                foreach (var data in RetailBillMainTable)
                {
                    _iRetailBillService.Create(data);
                }
                foreach (var data in RetailBillItem)
                {
                    _iRetailBillItemService.Create(data);
                }
                foreach (var data in InventoryTaxRetailBill)
                {
                    _iInventoryTaxService.Create(data);
                }
            }

            if (RetailBillAdjAmtDetail != null)
            {
                foreach (var data in RetailBillAdjAmtDetail)
                {
                    _iRetailBillAdjustAmountDetailService.Create(data);
                }

            }

            if (RetailBillSalesreturn != null)
            {
                foreach (var data in RetailBillSalesreturn)
                {
                    _iSalesReturnService.Create(data);
                }
                foreach (var data in RetailBillSalesReturnItem)
                {
                    _iSalesReturnItemService.Create(data);
                }
                foreach (var data in InventoryTaxSalesReturn)
                {
                    _iInventoryTaxService.Create(data);
                }
                foreach (var data in RetailBillCreditNote)
                {
                    _iRetailBillCreditNoteService.Create(data);
                }
                foreach (var data in RetailBillCreditNoteItem)
                {
                    _iRetailBillCreditNoteItemService.Create(data);
                }
            }

            //SalesBill
            if (SalesBillMainTable != null)
            {
                foreach (var data in SalesBillMainTable)
                {
                    _iSalesBillService.Create(data);
                }
                foreach (var data in SalesBillItem)
                {
                    _iSalesbillItemService.Create(data);
                }
                foreach (var data in InventoryTaxSalesBill)
                {
                    _iInventoryTaxService.Create(data);
                }

            }

            if (SalesBillAdjAmtDetail != null)
            {
                foreach (var data in SalesBillAdjAmtDetail)
                {
                    _IsalesBillAdjsutAmountDetails.Create(data);
                }

            }

            if (SalesBillSalesreturn != null)
            {
                foreach (var data in SalesBillSalesreturn)
                {
                    _iSalesReturnService.Create(data);
                }
                foreach (var data in SalesBillSalesReturnItem)
                {
                    _iSalesReturnItemService.Create(data);
                }
                foreach (var data in InventoryTaxSalesReturnSalesBill)
                {
                    _iInventoryTaxService.Create(data);
                }
                foreach (var data in SalesBillCreditNote)
                {
                    _iSalesBillCreditNoteService.Create(data);
                }
                foreach (var data in SalesBillCreditNoteItem)
                {
                    _iSalesBillCreditNoteItemService.Create(data);
                }
            }


            //Balance Carry Forward
            if (BalanceCarryForwardEntity != null)
            {
                foreach (var data in BalanceCarryForwardEntity)
                {
                    _iBalanceCarryForwardService.Create(data);
                }
            }

            //Cashier Sales Order
            if (CashierSalesOrderEnum != null)
            {
                foreach (var data in CashierSalesOrderEnum)
                {
                    _iCashierSalesOrderService.Create(data);
                }
                foreach (var data in CashierSalesorderItem)
                {
                    _iCashierSalesOrderItemService.Create(data);
                }
            }

            if (CashierSalesOrderCardChequeHandOver != null)
            {
                foreach (var data in CashierSalesOrderCardChequeHandOver)
                {
                    _iCashierSalesOrderService.Create(data);
                }
                foreach (var data in CashierSalesorderItemCardChequeHandOver)
                {
                    _iCashierSalesOrderItemService.Create(data);
                }
            }

            //Cashier Retail Bill
            if (CashierRetailBillEnum != null)
            {
                foreach (var data in CashierRetailBillEnum)
                {
                    _iCashierRetailBillService.Create(data);
                }
                foreach (var data in CashierRetailBillItem)
                {
                    _iCashierRetailBillItemService.Create(data);
                }
            }

            if (CashierRetailBillCardChequeHandOver != null)
            {
                foreach (var data in CashierRetailBillCardChequeHandOver)
                {
                    _iCashierRetailBillService.Create(data);
                }
                foreach (var data in CashierRetailBillItemCardChequeHandOver)
                {
                    _iCashierRetailBillItemService.Create(data);
                }
            }

            //CashierSalesBill
            if (CashierSalesBillEnum != null)
            {
                foreach (var data in CashierSalesBillEnum)
                {
                    _iCashierSalesBillService.Create(data);
                }
                foreach (var data in CashierSalesBillItem)
                {
                    _iCashierSalesBillItemService.Create(data);
                }
            }

            if (CashierSalesBillCardChequeHandOver != null)
            {
                foreach (var data in CashierSalesBillCardChequeHandOver)
                {
                    _iCashierSalesBillService.Create(data);
                }
                foreach (var data in CashierSalesBillItemCardChequeHandOver)
                {
                    _iCashierSalesBillItemService.Create(data);
                }
            }

            //Shop Requisition
            if (RequisitionForShop != null)
            {
                foreach (var data in RequisitionForShop)
                {
                    _iRequisitionForShopService.CreateRequisitionForShop(data);
                }
            }

            //GodownRequisition
            if (RequisitionForGodown != null)
            {
                foreach (var data in RequisitionForGodown)
                {
                    _iRequisitionForGodownService.CreateRequisitionForGodown(data);
                }
            }

            //Inward From Tailor
            if (InwardFromTailor != null)
            {
                foreach (var data in InwardFromTailor)
                {
                    _iInwardFromTailorService.Create(data);
                }
                foreach (var data1 in InwardFromTailorItem)
                {
                    _iInwardfromTailorItemService.Create(data1);
                }
            }

            //TCM
            if (TCM != null)
            {
                foreach (var data in TCM)
                {
                    _iTemporaryCashMemoService.Create(data);
                }
                foreach (var data in TCMItem)
                {
                    _iTemporaryCashMemoItemService.Create(data);
                }
                foreach (var data in InventoryTaxTCM)
                {
                    _iInventoryTaxService.Create(data);
                }

            }

            if (TCMAdjAmtDetail != null)
            {
                foreach (var data in TCMAdjAmtDetail)
                {
                    _ITemporaryCashMemoAdjustAmountDetailService.Create(data);
                }

            }

            //Cashier TCM
            if (CashierTemporaryCashMemoEnum != null)
            {
                foreach (var data in CashierTemporaryCashMemoEnum)
                {
                    _iCashierTemporaryCashMemoService.Create(data);
                }
                foreach (var data in CashierTemporaryCashMemoItem)
                {
                    _iCashierTemporarayCashMemoItemService.Create(data);
                }
            }

            if (CashierTemporaryCashMemoCardChequeHandOver != null)
            {
                foreach (var data in CashierTemporaryCashMemoCardChequeHandOver)
                {
                    _iCashierTemporaryCashMemoService.Create(data);
                }
                foreach (var data in CashierTemporaryCashMemoItemCardChequeHandOver)
                {
                    _iCashierTemporarayCashMemoItemService.Create(data);
                }
            }

            //Job WoRk Payment
            if (JobWorkPayment != null)
            {
                foreach (var data in JobWorkPayment)
                {
                    _iJobWorkPaymentService.Create(data);
                }
            }

            //Discount Master
            if (DiscountMaster != null )
            {
                foreach (var data in DiscountMaster)
                {
                    _iDiscountMasterService.Create(data);
                }
                foreach (var data1 in DiscountMasterItem)
                {
                    _iDiscountMasterItemservice.Create(data1);
                }
               
            }

            //cost code creation
            if (costcodecreation != null)
            {
                foreach (var data in costcodecreation)
                {
                    _icostcodecreation.Create(data);
                }
            }

            //inward to tailor
            if (InwardToTailor != null)
            {
                foreach (var data in InwardToTailor)
                {
                    _iinwardtotailorservice.Create(data);
                }
                foreach (var data1 in InwardToTailorItem)
                {
                    _iinwardtotailoritemservice.Create(data1);
                }
            }

            //jobworkstock
            if (JobWorkStock != null)
            {
                foreach (var data in JobWorkStock)
                {
                    _iJobWorkStockService.Create(data);
                }
            }

            //jobwork outward to client
            if (JobWorkOutwardToClient != null)
            {
                foreach (var data in JobWorkOutwardToClient)
                {
                    _ijobworkoutwardtoclient.Create(data);
                }
            }


            return View();
          

        }
    }
}
