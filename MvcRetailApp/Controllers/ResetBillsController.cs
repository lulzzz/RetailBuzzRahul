using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;
using System.IO;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class ResetBillsController : Controller
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

        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;
        private readonly IUtilityService _UtilityService;
        private readonly ISalesBillService _SalesBillService;
        private readonly ISalesBillItemService _SalesBillItemService;
        private readonly ISalesBillAdjAmtDetailService _SalesBillAdjAmtDetailService;
        private readonly IRetailBillService _RetailBillService;
        private readonly IRetailBillItemService _RetailBillItemService;
        private readonly IRetailBillAdjAmtDetailService _RetailBillAdjAmtDetailService;
        private readonly ICustomerDetailService _CustomerDetailService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IQuotationService _QuotationService;
        private readonly IQuotationItemService _QuotationItemService;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly IDeliveryChallanService _DeliveryChallanService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly IShopStockService _ShopStockService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;
        private readonly IItemService _ItemService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IShopService _ShopService;
        private readonly IResetRetailBillService _ResetRetailBillService;
        private readonly IResetRetailBillItemService _ResetRetailBillItemService;
        private readonly IResetRetailBillAdjAmtDetailService _ResetRetailBillAdjAmtDetailService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IInventoryTaxService _InventoryTaxService;
        private readonly ICashierReceivableService _CashierReceivableService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierRetailBillItemService _CashierRetailBillItemService;
        private readonly IResetSalesBillService _ResetSalesBillService;
        private readonly IResetSalesBillItemService _ResetSalesBillItemService;
        private readonly IResetSalesBillAdjAmtDetailService _ResetSalesBillAdjAmtDetailService;

        public ResetBillsController(IUserCredentialService UserCredentialService, IModuleService ModuleService, IUtilityService UtilityService,
            ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService, ISalesBillAdjAmtDetailService SalesBillAdjAmtDetailService, IRetailBillService RetailBillService, IRetailBillItemService RetailBillItemService,
            IRetailBillAdjAmtDetailService RetailBillAdjAmtDetailService, ICustomerDetailService CustomerDetailService, IEmployeeMasterService EmployeeMasterService,
            IQuotationService QuotationService, ISalesOrderService SalesOrderService, IDeliveryChallanService DeliveryChallanService, IQuotationItemService QuotationItemService,
            ISalesOrderItemService SalesOrderItemService, IDeliveryChallanItemService DeliveryChallanItemService, IShopStockService ShopStockService, IGodownStockService GodownStockService,
            IDiscountMasterItemService DiscountMasterItemService, IItemService ItemService, IItemTaxService ItemTaxService, INonInventoryItemService NonInventoryItemService,
            IShopService ShopService, IResetRetailBillService ResetRetailBillService, IResetRetailBillItemService ResetRetailBillItemService, IResetRetailBillAdjAmtDetailService ResetRetailBillAdjAmtDetailService,
            IOpeningStockService OpeningStockService, IEntryStockItemService EntryStockItemService, IStockItemDistributionService StockItemDistributionService,
            IInventoryTaxService InventoryTaxService, ICashierReceivableService CashierReceivableService, ICashierRetailBillService CashierRetailBillService,
            ICashierRetailBillItemService CashierRetailBillItemService, IResetSalesBillService ResetSalesBillService, IResetSalesBillItemService ResetSalesBillItemService, IResetSalesBillAdjAmtDetailService ResetSalesBillAdjAmtDetailService)
        {
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
            this._UtilityService = UtilityService;
            this._SalesBillService = SalesBillService;
            this._SalesBillItemService = SalesBillItemService;
            this._SalesBillAdjAmtDetailService = SalesBillAdjAmtDetailService;
            this._RetailBillService = RetailBillService;
            this._RetailBillItemService = RetailBillItemService;
            this._RetailBillAdjAmtDetailService = RetailBillAdjAmtDetailService;
            this._CustomerDetailService = CustomerDetailService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._QuotationService = QuotationService;
            this._QuotationItemService = QuotationItemService;
            this._SalesOrderService = SalesOrderService;
            this._SalesOrderItemService = SalesOrderItemService;
            this._DeliveryChallanService = DeliveryChallanService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._ShopStockService = ShopStockService;
            this._GodownStockService = GodownStockService;
            this._DiscountMasterItemService = DiscountMasterItemService;
            this._ItemService = ItemService;
            this._ItemTaxService = ItemTaxService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._ShopService = ShopService;
            this._ResetRetailBillService = ResetRetailBillService;
            this._ResetRetailBillItemService = ResetRetailBillItemService;
            this._ResetRetailBillAdjAmtDetailService = ResetRetailBillAdjAmtDetailService;
            this._OpeningStockService = OpeningStockService;
            this._EntryStockItemService = EntryStockItemService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._InventoryTaxService = InventoryTaxService;
            this._CashierReceivableService = CashierReceivableService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierRetailBillItemService = CashierRetailBillItemService;
            this._ResetSalesBillService = ResetSalesBillService;
            this._ResetSalesBillItemService = ResetSalesBillItemService;
            this._ResetSalesBillAdjAmtDetailService = ResetSalesBillAdjAmtDetailService;
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

        [HttpGet]
        public JsonResult EncodeId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult StartingBlankPage()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult SelectResetBillPopup()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        /***********************************************RESET RETAIL BILL*******************************************/

        //AUTO COMPLETE RETAILBILL NO
        [HttpGet]
        public JsonResult GetRetailBillNos(string SearchTerm)
        {
            var data = _RetailBillService.GetAllRetailBillNos(SearchTerm);
            List<string> No = new List<string>();
            foreach (var item in data)
            {
                No.Add(item.RetailBillNo);
            }
            return Json(new { No }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY RETAIL BILL DETAILS ON POP UP PAGE
        public ActionResult RetailBillDetails(string no)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RetailBillDetails = _RetailBillService.GetDetailsByInvoiceNo(no);
            model.RetailBillItemList = _RetailBillItemService.GetDetailsByInvoiceNo(no);
            model.RetailBillAdjAmtDetailList = _RetailBillAdjAmtDetailService.GetBillsByRetailNo(no);
            return View(model);
        }

        //RESET RETAIL BILL
        [HttpGet]
        public ActionResult ResetRetailBill(string retailbill)
        {
            MainApplication model = new MainApplication()
            {
                RetailBillDetails = new RetailBill(),
            };
            var retailbilldetails = _RetailBillService.GetDetailsByInvoiceNo(retailbill);
            model.RetailBillDetails.RetailBillNo = retailbill;
            model.RetailBillDetails.RetailMasterId = retailbilldetails.RetailMasterId;
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["PrevRetailBill"] = retailbilldetails;
            TempData["PreviousRetailno"] = retailbill;
            return View(model);
        }

        //AUTO COMPLETE CLIENT NAME CUSTOMER DETAILS
        [HttpGet]
        public JsonResult GetCustomerNames(string term)
        {
            var customerdata = _CustomerDetailService.GetCustomerName(term);
            List<string> names = new List<string>();
            foreach (var data in customerdata)
            {
                names.Add(data.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET CLIENT DETAILS BY CLIENT NAME
        [HttpGet]
        public JsonResult GetCustomerDetails(string name)
        {
            var data = _CustomerDetailService.GetDetailsByName(name);
            return Json(new { data.Address, data.EmailId, data.Contact }, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE SALES PERSON NAME
        [HttpGet]
        public JsonResult GetEmployeeName(string term)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            var employeedata = _EmployeeMasterService.GetEmpByNameAndSalesDepartment(term);
            List<string> names = new List<string>();
            foreach (var item in employeedata)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //QUOTATION NO AUTO COMPLETE
        [HttpGet]
        public JsonResult GetQuotationNo(string SearchTerm)
        {
            if (SearchTerm.ToLower().Contains("q"))
            {
                var quotdata = _QuotationService.GetActiveQuotNo(SearchTerm);
                List<string> names = new List<string>();
                foreach (var item in quotdata)
                {
                    names.Add(item.QuotNo);
                }
                return Json(names, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //SALES ORDER NO AUTO COMPLETE
        [HttpGet]
        public JsonResult GetSalesOrderNo(string SearchTerm)
        {
            if (SearchTerm.ToLower().Contains("s"))
            {
                var orderdata = _SalesOrderService.GetDetailsByStatusAndCashierStatus(SearchTerm);
                List<string> names = new List<string>();
                foreach (var item in orderdata)
                {
                    names.Add(item.OrderNo);
                }
                return Json(names, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        // CHALLAN NO AUTO COMPLETE
        [HttpGet]
        public JsonResult GetDeliveryChallanNo(string SearchTerm)
        {
            if (SearchTerm.ToLower().Contains("d"))
            {
                var deliverydata = _DeliveryChallanService.GetActiveChallanNo(SearchTerm);
                List<string> names = new List<string>();
                foreach (var item in deliverydata)
                {
                    names.Add(item.ChallanNo);
                }
                return Json(names, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //GET CLIENT DETAILS ON BILL NO CHANGE EVENT
        [HttpGet]
        public JsonResult GetClientDetails(string BillNo)
        {
            if (BillNo.Contains("Qu"))
            {
                var clientdata = _QuotationService.GetDetailsByQuotNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.FormType, clientdata.SalesPersonName }, JsonRequestBehavior.AllowGet);
            }
            else if (BillNo.Contains("SO"))
            {
                var clientdata = _SalesOrderService.GetDetailsByOrderNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.FormType, clientdata.SalesPersonName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var clientdata = _DeliveryChallanService.GetDetailsByChallanNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.FormType, clientdata.SalesPersonName }, JsonRequestBehavior.AllowGet);
            }
        }

        //GET MULTIPLE BILL ITEM DETAILS POP UP PAGE
        [HttpGet]
        public ActionResult GetClientPendingBills(string BillNo, string ShopCode)
        {
            MainApplication model = new MainApplication();
            if (BillNo.Contains("Qu"))
            {
                var clientdetails = _QuotationService.GetDetailsByQuotNo(BillNo);
                List<QuotationItem> QUItemList = new List<QuotationItem>();
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<DeliveryChallanItem> DCItemList = new List<DeliveryChallanItem>();

                //Get quotation items by clientname
                var ClientQU = _QuotationService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientQuots in ClientQU)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientQuots.QuotNo);
                    foreach (var item in quitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            //calculate shop quantity..
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        QUItemList.Add(item);
                    }
                }
                model.QuotationItemList = QUItemList;

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);
                    foreach (var item in soitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get challan items by clientname
                var ClientDC = _DeliveryChallanService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientChallans in ClientDC)
                {
                    var dcitems = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(clientChallans.ChallanNo);
                    foreach (var item in dcitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        DCItemList.Add(item);
                    }
                }
                model.DeliveryChallanItemList = DCItemList;
            }

            else if (BillNo.Contains("SO"))
            {
                var clientdetails = _SalesOrderService.GetDetailsByOrderNo(BillNo);
                List<QuotationItem> QUItemList = new List<QuotationItem>();
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<DeliveryChallanItem> DCItemList = new List<DeliveryChallanItem>();

                //Get quotation items by clientname
                var ClientQU = _QuotationService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientQuots in ClientQU)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientQuots.QuotNo);
                    foreach (var item in quitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();


                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        QUItemList.Add(item);
                    }
                }
                model.QuotationItemList = QUItemList;

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);
                    foreach (var item in soitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get challan items by clientname
                var ClientDC = _DeliveryChallanService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientChallans in ClientDC)
                {
                    var dcitems = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(clientChallans.ChallanNo);
                    foreach (var item in dcitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();


                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        DCItemList.Add(item);
                    }
                }
                model.DeliveryChallanItemList = DCItemList;
            }

            else
            {
                var clientdetails = _DeliveryChallanService.GetDetailsByChallanNo(BillNo);
                List<QuotationItem> QUItemList = new List<QuotationItem>();
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<DeliveryChallanItem> DCItemList = new List<DeliveryChallanItem>();

                //Get quotation items by clientname
                var ClientQU = _QuotationService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientQuots in ClientQU)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientQuots.QuotNo);
                    foreach (var item in quitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();


                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        QUItemList.Add(item);
                    }
                }
                model.QuotationItemList = QUItemList;

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);
                    foreach (var item in soitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get challan items by clientname
                var ClientDC = _DeliveryChallanService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientChallans in ClientDC)
                {
                    var dcitems = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(clientChallans.ChallanNo);
                    foreach (var item in dcitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        DCItemList.Add(item);
                    }
                }
                model.DeliveryChallanItemList = DCItemList;
            }
            return View(model);
        }

        //DISPLAY ITEM DISTAILS FROM SHOP STOCK
        [HttpGet]
        public JsonResult GetBarcodeItemDetails(string barcode, string shop)
        {
            var barcodevalue = barcode.ToUpper() + ".png";
            var data = _ShopStockService.GetDetailsByBarcode(barcodevalue);

            if (data != null)
            {
                double? discount = 0;
                double? discountrps = 0;
                var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(data.ItemCode);
                if (discountdetails != null)
                {
                    discount = discountdetails.DiscInPercentage;
                    discountrps = discountdetails.DiscInRupees;
                }
                //calculate total quantity (shop quantity + godown quantity)
                double? shopqty = 0;
                double? godownqty = 0;
                double? totalstkqty = 0;
                var shopstockitems = _ShopStockService.GetRowsByItemCode(data.ItemCode);
                foreach (var item in shopstockitems)
                {
                    shopqty = shopqty + item.Quantity;
                }

                var godownstockitems = _GodownStockService.GetRowsByItemCode(data.ItemCode);
                foreach (var item in godownstockitems)
                {
                    godownqty = godownqty + item.Quantity;
                }
                totalstkqty = shopqty + godownqty;

                var itemdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(data.ItemCode, shop);
                if (itemdetails != null)
                {
                    string costprice = _ItemService.GetDescriptionByItemCode(data.ItemCode).costprice;
                    string subcat = _ItemService.GetDescriptionByItemCode(data.ItemCode).itemSubCategory;
                    double taxPercent = 0;
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                    return Json(new
                    {
                        discount,
                        discountrps,
                        itemdetails.ItemCode,
                        itemdetails.ItemName,
                        itemdetails.Description,
                        itemdetails.Quantity,
                        itemdetails.SellingPrice,
                        itemdetails.MRP,
                        costprice,
                        itemdetails.Unit,
                        itemdetails.NumberType,
                        itemdetails.Color,
                        itemdetails.Material,
                        itemdetails.Size,
                        itemdetails.Brand,
                        itemdetails.DesignCode,
                        itemdetails.DesignName,
                        taxPercent,
                        totalstkqty
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string msg = "Fail";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //GET NON INVENTORY ITEM DDL
        [HttpGet]
        public JsonResult GetNonInventoryItems()
        {
            var noninvitems = _NonInventoryItemService.GetAll();
            var items = noninvitems.Select(m => new SelectListItem
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //GET SHOP DETAILS BY SHOP CODE
        [HttpGet]
        public JsonResult GetShopDetails(string code)
        {
            var data = _ShopService.GetShopByCode(code);
            return Json(new { data.ShopName, data.ShopAddress, data.ShopEmail, data.EmpName }, JsonRequestBehavior.AllowGet);
        }

        //GET LOGIN EMPLOPYEE DETAILS BY EMPLOYEE EMAIL
        [HttpGet]
        public JsonResult GetPreparedByEmpDetails(string email)
        {
            var data = _EmployeeMasterService.getEmpByEmail(email);
            return Json(new { data.Name, data.MobileNo }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY ITEM DISTAILS BY NONINVENTORY ITEM CODE
        [HttpGet]
        public JsonResult GetNonInvItemdetailsByItemCode(string ItemCode)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            var itemdetails = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            if (itemdetails != null)
            {
                double taxPercent = 0;
                var Tax = _ItemTaxService.GetLatestTax("VAT", itemdetails.itemSubCategory);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
                if (itemdetails.costprice == "")
                {
                    itemdetails.costprice = "0";
                }
                return Json(new
                {
                    discount,
                    itemdetails.itemName,
                    itemdetails.itemCode,
                    itemdetails.description,
                    itemdetails.sellingprice,
                    itemdetails.costprice,
                    itemdetails.mrp,
                    itemdetails.unit,
                    itemdetails.NumberType,
                    itemdetails.colorCode,
                    itemdetails.typeOfMaterial,
                    itemdetails.size,
                    itemdetails.brandName,
                    itemdetails.designCode,
                    itemdetails.designName,
                    taxPercent
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //GET EMPLOPYEE DETAILS BY EMPLOYEE NAME
        [HttpGet]
        public JsonResult GetEmployeeDetails(string name)
        {
            var data = _EmployeeMasterService.getEmpByName(name);
            return Json(new { data.MobileNo, data.Designation, data.Email, data.Address }, JsonRequestBehavior.AllowGet);
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

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS
        public ActionResult CashCardChequePayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetRetailBill(MainApplication model, FormCollection frmcol)
        {
            MainApplication mainapp = new MainApplication()
            {
                ResetRetailBillDetails = new ResetRetailBill(),
                ResetRetailBillItemDetails = new ResetRetailBillItem(),
                ResetRetailBillAdjAmtDetails = new ResetRetailBillAdjAmtDetail(),
            };

            var retailbillno = model.RetailBillDetails.RetailBillNo;

            // ****************** REPLACE(UPDATE) ACTUAL RETAIL BILL WITH NEW BILL AFTER RESET RETAIL BILL *******************
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string RetailBillCode = model.RetailBillDetails.RetailBillNo;

            //SAVE DIRECT(COUNTER) RETAIL BILL AND RETAIL BILL USING MULTIPLE BILLS..
            model.RetailBillDetails.Date = DateTime.Now;
            model.RetailBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            model.RetailBillDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
            model.RetailBillDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
            model.RetailBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
            model.RetailBillDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
            model.RetailBillDetails.Status = "Active";
            model.RetailBillDetails.ModifiedOn = DateTime.Now;
            model.RetailBillDetails.CashierStatus = "Active";
            model.RetailBillDetails.SalesReturn = "No";
            model.RetailBillDetails.AdjustedAmount = 0;
            model.RetailBillDetails.BillBalance = "None";
            model.RetailBillDetails.CreditNoteAmount = 0;

            //IF PAYMENT IS NULL THEN SAVE PAYMENT IS 0 IN RETAIL BILL
            if (model.RetailBillDetails.Payment == null)
            {
                model.RetailBillDetails.Payment = 0;
            }

            //IF BALNCE IS 0 THEN WE CHECK THE REFUND FOR SAVE
            if (model.RetailBillDetails.Balance == 0)
            {
                model.RetailBillDetails.Refund = model.RetailBillDetails.Payment - model.RetailBillDetails.GrandTotal;
            }
            else
            {
                model.RetailBillDetails.Refund = 0;
            }

            model.RetailBillDetails.TaxStatus = frmcol["TaxStatus"];
            _RetailBillService.Update(model.RetailBillDetails);

            // SAVE MAIN RESET RETAIL BILL
            var retailbilldetails = TempData["PrevRetailBill"] as RetailBill;
            mainapp.ResetRetailBillDetails.RetailBillNo = retailbilldetails.RetailBillNo;
            mainapp.ResetRetailBillDetails.Date = retailbilldetails.Date;
            mainapp.ResetRetailBillDetails.QuotationNo = retailbilldetails.QuotationNo;
            mainapp.ResetRetailBillDetails.SalesOrderNo = retailbilldetails.SalesOrderNo;
            mainapp.ResetRetailBillDetails.DeliveryChallanNo = retailbilldetails.DeliveryChallanNo;
            mainapp.ResetRetailBillDetails.ClientName = retailbilldetails.ClientName;
            mainapp.ResetRetailBillDetails.ClientAddress = retailbilldetails.ClientAddress;
            mainapp.ResetRetailBillDetails.ClientEmail = retailbilldetails.ClientEmail;
            mainapp.ResetRetailBillDetails.ClientContact = retailbilldetails.ClientContact;
            mainapp.ResetRetailBillDetails.SalesPersonName = retailbilldetails.SalesPersonName;
            mainapp.ResetRetailBillDetails.SalesPersonAddress = retailbilldetails.SalesPersonAddress;
            mainapp.ResetRetailBillDetails.SalesPersonEmail = retailbilldetails.SalesPersonEmail;
            mainapp.ResetRetailBillDetails.SalesPersonContact = retailbilldetails.SalesPersonContact;
            mainapp.ResetRetailBillDetails.SalesPersonDesignation = retailbilldetails.SalesPersonDesignation;
            mainapp.ResetRetailBillDetails.TotalAmount = retailbilldetails.TotalAmount;
            mainapp.ResetRetailBillDetails.TotalDiscount = retailbilldetails.TotalDiscount;
            mainapp.ResetRetailBillDetails.TotalTaxAmount = retailbilldetails.TotalTaxAmount;
            mainapp.ResetRetailBillDetails.Payment = retailbilldetails.Payment;
            mainapp.ResetRetailBillDetails.GrandTotal = retailbilldetails.GrandTotal;
            mainapp.ResetRetailBillDetails.Balance = retailbilldetails.Balance;
            mainapp.ResetRetailBillDetails.RefundToClient = retailbilldetails.RefundToClient;
            mainapp.ResetRetailBillDetails.Refund = retailbilldetails.Refund;
            mainapp.ResetRetailBillDetails.PreparedBy = retailbilldetails.PreparedBy;
            mainapp.ResetRetailBillDetails.ShopCode = retailbilldetails.ShopCode;
            mainapp.ResetRetailBillDetails.ShopName = retailbilldetails.ShopName;
            mainapp.ResetRetailBillDetails.ShopAddress = retailbilldetails.ShopAddress;
            mainapp.ResetRetailBillDetails.ShopContactPerson = retailbilldetails.ShopContactPerson;
            mainapp.ResetRetailBillDetails.ShopEmail = retailbilldetails.ShopEmail;
            mainapp.ResetRetailBillDetails.AdjustedStatus = retailbilldetails.AdjustedStatus;
            mainapp.ResetRetailBillDetails.AdjustedAmount = retailbilldetails.AdjustedAmount;
            mainapp.ResetRetailBillDetails.AdjustedBill = retailbilldetails.AdjustedBill;
            mainapp.ResetRetailBillDetails.CashierStatus = retailbilldetails.CashierStatus;
            mainapp.ResetRetailBillDetails.SalesReturnNo = retailbilldetails.SalesReturnNo;
            mainapp.ResetRetailBillDetails.SalesReturn = retailbilldetails.SalesReturn;
            mainapp.ResetRetailBillDetails.Status = retailbilldetails.Status;
            mainapp.ResetRetailBillDetails.ModifiedOn = retailbilldetails.ModifiedOn;
            mainapp.ResetRetailBillDetails.BillBalance = retailbilldetails.BillBalance;
            mainapp.ResetRetailBillDetails.CreditNoteNo = retailbilldetails.CreditNoteNo;
            mainapp.ResetRetailBillDetails.CreditNoteAmount = retailbilldetails.CreditNoteAmount;
            mainapp.ResetRetailBillDetails.TemporaryCashMemo = retailbilldetails.TemporaryCashMemo;
            mainapp.ResetRetailBillDetails.TaxStatus = retailbilldetails.TaxStatus;
            _ResetRetailBillService.Create(mainapp.ResetRetailBillDetails);

            //IF RESET BILL IS WITH SALES ORDER AMOUNT ADJUSTMENT THEN AGAIN LIVE THIS ORDER..
            var AdjustedSalesOrders = _RetailBillAdjAmtDetailService.GetBillsByRetailNo(retailbillno);
            foreach (var order in AdjustedSalesOrders)
            {
                var SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(order.AdjustedBill);
                SalesOrderDetails.Status = "Active";
                SalesOrderDetails.RemainingAdvance = SalesOrderDetails.RemainingAdvance + SalesOrderDetails.AdjustedAmount;
                _SalesOrderService.Update(SalesOrderDetails);
            }

            // SAVE RESET RETAIL BILL ITEMS
            var retailbillitemdetails = _RetailBillItemService.GetDetailsByInvoiceNo(retailbillno);
            foreach (var data in retailbillitemdetails)
            {
                mainapp.ResetRetailBillItemDetails.RetailBillNo = data.RetailBillNo;
                mainapp.ResetRetailBillItemDetails.BillNo = data.BillNo;
                mainapp.ResetRetailBillItemDetails.ItemCode = data.ItemCode;
                mainapp.ResetRetailBillItemDetails.ItemName = data.ItemName;
                mainapp.ResetRetailBillItemDetails.ItemType = data.ItemType;
                mainapp.ResetRetailBillItemDetails.Barcode = data.Barcode;
                mainapp.ResetRetailBillItemDetails.Narration = data.Narration;
                mainapp.ResetRetailBillItemDetails.Description = data.Description;
                mainapp.ResetRetailBillItemDetails.Quantity = data.Quantity;
                mainapp.ResetRetailBillItemDetails.Unit = data.Unit;
                mainapp.ResetRetailBillItemDetails.NumberType = data.NumberType;
                mainapp.ResetRetailBillItemDetails.SellingPrice = data.SellingPrice;
                mainapp.ResetRetailBillItemDetails.MRP = data.MRP;
                mainapp.ResetRetailBillItemDetails.Size = data.Size;
                mainapp.ResetRetailBillItemDetails.Brand = data.Brand;
                mainapp.ResetRetailBillItemDetails.DesignCode = data.DesignCode;
                mainapp.ResetRetailBillItemDetails.DesignName = data.DesignName;
                mainapp.ResetRetailBillItemDetails.Amount = data.Amount;
                mainapp.ResetRetailBillItemDetails.Color = data.Color;
                mainapp.ResetRetailBillItemDetails.Material = data.Material;
                mainapp.ResetRetailBillItemDetails.DiscountPercent = data.DiscountPercent;
                mainapp.ResetRetailBillItemDetails.DiscountRPS = data.DiscountRPS;
                mainapp.ResetRetailBillItemDetails.ItemTax = data.ItemTax;
                mainapp.ResetRetailBillItemDetails.ItemTaxAmt = data.ItemTaxAmt;
                mainapp.ResetRetailBillItemDetails.SalesReturn = data.SalesReturn;
                mainapp.ResetRetailBillItemDetails.Status = data.Status;
                mainapp.ResetRetailBillItemDetails.ModifiedOn = data.ModifiedOn;
                mainapp.ResetRetailBillItemDetails.ReadOnlyTotalStockQuantity = data.ReadOnlyTotalStockQuantity;
                _ResetRetailBillItemService.Create(mainapp.ResetRetailBillItemDetails);

                //ADD RESET BILL ITEMS IN STOCK
                if (data.ItemType == "Inventory")
                {
                    if (data.BillNo == null || data.BillNo.Contains("Qu") || data.BillNo.Contains("SO"))
                    {
                        //UPDATE SHOP STOCK (SHOP STOCK QUANTITY + RESET BILL QUANTITY)
                        var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(mainapp.ResetRetailBillItemDetails.ItemCode, mainapp.ResetRetailBillDetails.ShopCode);
                        shopstockdata.Quantity = shopstockdata.Quantity + mainapp.ResetRetailBillItemDetails.Quantity;
                        _ShopStockService.Update(shopstockdata);

                        //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY + RESET BILL QUANTITY)
                        var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(mainapp.ResetRetailBillItemDetails.ItemCode, mainapp.ResetRetailBillDetails.ShopCode);
                        stkdisdata.ItemQuantity = stkdisdata.ItemQuantity + Convert.ToDouble(mainapp.ResetRetailBillItemDetails.Quantity);
                        _StockItemDistributionService.Update(stkdisdata);

                        //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY + RESET BILL QUANTITY)
                        var entrystkdata = _EntryStockItemService.getDetailsByItemCode(mainapp.ResetRetailBillItemDetails.ItemCode);
                        //if item not present in entry stock then opening stock
                        if (entrystkdata != null)
                        {
                            entrystkdata.TotalQuantity = entrystkdata.TotalQuantity + Convert.ToDouble(mainapp.ResetRetailBillItemDetails.Quantity);
                            _EntryStockItemService.Update(entrystkdata);
                        }
                        //UPDATE OPENING STOCK (OPENING STOCK QUANTITY + RESET BILL QUANTITY)
                        else
                        {
                            var openingstkdata = _OpeningStockService.GetDetailsByItemCode(mainapp.ResetRetailBillItemDetails.ItemCode);
                            openingstkdata.TotalQuantity = openingstkdata.TotalQuantity + Convert.ToDouble(mainapp.ResetRetailBillItemDetails.Quantity);
                            _OpeningStockService.UpdateStock(openingstkdata);
                        }
                    }
                }

                if (data.BillNo != null)
                {
                    if (data.BillNo.Contains("Qu"))
                    {
                        var quotitemdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(data.ItemCode, data.BillNo);
                        quotitemdata.Balance = quotitemdata.Balance + data.Quantity;
                        if (quotitemdata.Status == "InActive")
                        {
                            quotitemdata.Status = "Active";
                        }
                        _QuotationItemService.Update(quotitemdata);

                        var QuotDetails = _QuotationService.GetDetailsByQuotNo(data.BillNo);
                        if (QuotDetails.Status == "InActive")
                        {
                            QuotDetails.Status = "Active";
                            _QuotationService.Update(QuotDetails);
                        }
                    }
                    else if (data.BillNo.Contains("SO"))
                    {
                        var soitemdata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(data.ItemCode, data.BillNo);
                        soitemdata.SOBalance = soitemdata.SOBalance + data.Quantity;
                        if (soitemdata.Status == "InActive")
                        {
                            soitemdata.Status = "Active";
                        }
                        _SalesOrderItemService.Update(soitemdata);

                        var SODetails = _SalesOrderService.GetDetailsBySalesOrderNo(data.BillNo);
                        if (SODetails.Status == "InActive")
                        {
                            SODetails.Status = "Active";
                            _SalesOrderService.Update(SODetails);
                        }
                    }
                    else
                    {
                        var deliverychallanitem = _DeliveryChallanItemService.GetDetailsByItemCodeAndChallanNo(data.ItemCode, data.BillNo);
                        deliverychallanitem.DelBalance = deliverychallanitem.DelBalance + data.Quantity;
                        if (deliverychallanitem.Status == "InActive")
                        {
                            deliverychallanitem.Status = "Active";
                        }
                        _DeliveryChallanItemService.Update(deliverychallanitem);

                        var DCDetails = _DeliveryChallanService.GetDetailsByChallanNo(data.BillNo);
                        if (DCDetails.Status == "InActive")
                        {
                            DCDetails.Status = "Active";
                            _DeliveryChallanService.Update(DCDetails);
                        }
                    }
                }
            }

            //DELETE RESET RETAIL BILL ITEM ADJUST AMOUNT DETAILS
            var RBItemList = _RetailBillItemService.GetDetailsByInvoiceNo(retailbillno);
            foreach (var row in RBItemList)
            {
                _RetailBillItemService.Delete(row);
            }

            // SAVE RESET RETAIL BILL ADJUST AMOUNT DETAILS
            var retailbilladjamtdetails = _RetailBillAdjAmtDetailService.GetBillsByRetailNo(retailbillno);
            foreach (var data in retailbilladjamtdetails)
            {
                mainapp.ResetRetailBillAdjAmtDetails.RetailBillNo = data.RetailBillNo;
                mainapp.ResetRetailBillAdjAmtDetails.AdjustedBill = data.AdjustedBill;
                mainapp.ResetRetailBillAdjAmtDetails.AdjustedAmount = data.AdjustedAmount;
                mainapp.ResetRetailBillAdjAmtDetails.Status = data.Status;
                mainapp.ResetRetailBillAdjAmtDetails.ModifiedOn = data.ModifiedOn;
                _ResetRetailBillAdjAmtDetailService.Create(mainapp.ResetRetailBillAdjAmtDetails);
            }

            //DELETE RESET BILL ADJUST AMOUNT DETAILS
            var RBAdjAmtList = _RetailBillAdjAmtDetailService.GetBillsByRetailNo(retailbillno);
            foreach (var row in RBAdjAmtList)
            {
                _RetailBillAdjAmtDetailService.Delete(row);
            }

            // SAVE CUSTOMER DETAILS
            if (model.RetailBillDetails.ClientName != null)
            {
                model.CustomerDetails = new CustomerDetail();

                var customerdata = _CustomerDetailService.GetDetailsByName(model.RetailBillDetails.ClientName);
                if (customerdata == null)
                {
                    model.CustomerDetails.Name = model.RetailBillDetails.ClientName;
                    model.CustomerDetails.Address = model.RetailBillDetails.ClientAddress;
                    model.CustomerDetails.Contact = model.RetailBillDetails.ClientContact;
                    model.CustomerDetails.EmailId = model.RetailBillDetails.ClientEmail;
                    model.CustomerDetails.Status = "Active";
                    model.CustomerDetails.ModifiedOn = DateTime.Now;
                    _CustomerDetailService.CreateInvoice(model.CustomerDetails);
                }
                else
                {
                    customerdata.Address = model.RetailBillDetails.ClientAddress;
                    customerdata.Contact = model.RetailBillDetails.ClientContact;
                    customerdata.EmailId = model.RetailBillDetails.ClientEmail;
                    customerdata.ModifiedOn = DateTime.Now;
                    _CustomerDetailService.UpdateInvoice(customerdata);
                }
            }

            var retailbilltype = "";

            // SAVE DIRECT RETAIL BILL ITEMS
            if (model.RetailBillDetails.QuotationNo == null && model.RetailBillDetails.SalesOrderNo == null && model.RetailBillDetails.DeliveryChallanNo == null)
            {
                retailbilltype = "Direct";
                int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);

                for (int row = 1; row <= rowcount; row++)
                {
                    string itemcode = "ItemCodeVal" + row;
                    string itemname = "ItemName" + row;
                    string itemnameval = "ItemNameVal" + row;
                    string itemtype = "ItemType" + row;
                    string barcode = "Barcode" + row;
                    string description = "Description" + row;
                    string quantity = "Quantity" + row;
                    string unit = "UnitVal" + row;
                    string numbertype = "NumberType" + row;
                    string sellingprice = "SellingPrice" + row;
                    string mrp = "MRP" + row;
                    string amount = "AmountVal" + row;
                    string color = "Color" + row;
                    string material = "Material" + row;
                    string size = "Size" + row;
                    string brand = "Brand" + row;
                    string designcode = "DesignCode" + row;
                    string designnname = "DesignName" + row;
                    string narration = "Narration" + row;
                    string disrs = "DisRs" + row;
                    string disper = "Disper" + row;
                    string proptax = "PropTax" + row;
                    string proptaxamt = "proportionatetaxamount" + row;
                    string itemtax = "ItemTaxVal" + row;
                    string itemtaxamt = "ItemTaxAmt" + row;

                    if (frmcol[quantity] != null)
                    {
                        model.RetailBillItemDetails = new RetailBillItem();
                        model.RetailBillItemDetails.RetailBillNo = model.RetailBillDetails.RetailBillNo;
                        model.RetailBillItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.RetailBillItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            model.RetailBillItemDetails.ItemName = frmcol[itemnameval];
                        }
                        model.RetailBillItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.RetailBillItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                        }
                        model.RetailBillItemDetails.Narration = frmcol[narration];
                        model.RetailBillItemDetails.Description = frmcol[description];
                        model.RetailBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.RetailBillItemDetails.Unit = frmcol[unit];
                        model.RetailBillItemDetails.NumberType = frmcol[numbertype];
                        model.RetailBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.RetailBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        model.RetailBillItemDetails.Size = frmcol[size];
                        model.RetailBillItemDetails.Brand = frmcol[brand];
                        model.RetailBillItemDetails.DesignCode = frmcol[designcode];
                        model.RetailBillItemDetails.DesignName = frmcol[designnname];
                        model.RetailBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        model.RetailBillItemDetails.Color = frmcol[color];
                        model.RetailBillItemDetails.Material = frmcol[material];
                        model.RetailBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        model.RetailBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        model.RetailBillItemDetails.ItemTax = frmcol[itemtax];
                        model.RetailBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        model.RetailBillItemDetails.Status = "Active";
                        model.RetailBillItemDetails.ModifiedOn = DateTime.Now;
                        model.RetailBillItemDetails.SalesReturn = "No";
                        _RetailBillItemService.Create(model.RetailBillItemDetails);

                        if (frmcol[itemtype] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - model.RetailBillItemDetails.Quantity;
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }
                    }
                }
            }
            // SAVE RETAIL BILL ITEMS USING MULTIPLE BILLS
            else
            {
                retailbilltype = "MultipleBill";
                int count = Convert.ToInt32(frmcol["hdnRowCount"]);
                for (int i = 1; i <= count; i++)
                {
                    string billno = "BillNo" + i;
                    string itemcode = "ItemCodeVal" + i;
                    string itemname = "ItemName" + i;
                    string itemnameval = "ItemNameVal" + i;
                    string itemtype = "ItemType" + i;
                    string barcode = "Barcode" + i;
                    string description = "Description" + i;
                    string quantity = "Quantity" + i;
                    string unit = "UnitVal" + i;
                    string numbertype = "NumberType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string amount = "AmountVal" + i;
                    string color = "Color" + i;
                    string material = "Material" + i;
                    string size = "Size" + i;
                    string brand = "Brand" + i;
                    string designcode = "DesignCode" + i;
                    string designnname = "DesignName" + i;
                    string narration = "Narration" + i;
                    string disrs = "DisRs" + i;
                    string disper = "Disper" + i;
                    string proptax = "PropTax" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    string itemtax = "ItemTaxVal" + i;
                    string itemtaxamt = "ItemTaxAmt" + i;

                    if (frmcol[quantity] != null)
                    {
                        model.RetailBillItemDetails = new RetailBillItem();
                        model.RetailBillItemDetails.RetailBillNo = model.RetailBillDetails.RetailBillNo;
                        model.RetailBillItemDetails.BillNo = frmcol[billno];
                        model.RetailBillItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.RetailBillItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            if (frmcol[itemnameval] != null)
                            {
                                model.RetailBillItemDetails.ItemName = frmcol[itemnameval];
                            }
                            else
                            {
                                model.RetailBillItemDetails.ItemName = frmcol[itemname];
                            }
                        }
                        model.RetailBillItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.RetailBillItemDetails.Barcode = frmcol[barcode] + ".png";
                        }
                        model.RetailBillItemDetails.Narration = frmcol[narration];
                        model.RetailBillItemDetails.Description = frmcol[description];
                        model.RetailBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.RetailBillItemDetails.Unit = frmcol[unit];
                        model.RetailBillItemDetails.NumberType = frmcol[numbertype];
                        model.RetailBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.RetailBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        model.RetailBillItemDetails.Size = frmcol[size];
                        model.RetailBillItemDetails.Brand = frmcol[brand];
                        model.RetailBillItemDetails.DesignCode = frmcol[designcode];
                        model.RetailBillItemDetails.DesignName = frmcol[designnname];
                        model.RetailBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        model.RetailBillItemDetails.Color = frmcol[color];
                        model.RetailBillItemDetails.Material = frmcol[material];
                        model.RetailBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        model.RetailBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        model.RetailBillItemDetails.ItemTax = frmcol[itemtax];
                        model.RetailBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        model.RetailBillItemDetails.Status = "Active";
                        model.RetailBillItemDetails.ModifiedOn = DateTime.Now;
                        model.RetailBillItemDetails.SalesReturn = "No";
                        _RetailBillItemService.Create(model.RetailBillItemDetails);

                        //UPDATE QUOTATION, SALES ORDER, DELIVERY CHALLAN QUANTITY WHEN IT IS PROCESSED IN RETAIL BILL..
                        if (frmcol[billno] != null)
                        {
                            if (frmcol[billno].Contains("Qu"))
                            {
                                var quotdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frmcol[itemcode], frmcol[billno]);
                                quotdata.Balance = quotdata.Balance - (model.RetailBillItemDetails.Quantity);

                                //if we send all quantity then inactive that item
                                if (quotdata.Balance <= 0)
                                {
                                    quotdata.Status = "InActive";
                                    quotdata.Balance = 0;
                                }
                                _QuotationItemService.Update(quotdata);

                                var QuotDetails = _QuotationService.GetDetailsByQuotNo(frmcol[billno]);
                                var QuotActiveItemsList = _QuotationItemService.GetAllActiveItemsByQuotNo(frmcol[billno]);
                                if (QuotActiveItemsList.Count() == 0)
                                {
                                    QuotDetails.Status = "InActive";
                                    _QuotationService.Update(QuotDetails);
                                }
                            }
                            else if (frmcol[billno].Contains("SO"))
                            {
                                var sodata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(frmcol[itemcode], frmcol[billno]);
                                sodata.SOBalance = sodata.SOBalance - (model.RetailBillItemDetails.Quantity);

                                //if we send all quantity then inactive that item
                                if (sodata.SOBalance <= 0)
                                {
                                    sodata.Status = "InActive";
                                    sodata.SOBalance = 0;
                                }
                                _SalesOrderItemService.Update(sodata);

                                var SODetails = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol[billno]);
                                var SOItemActiveList = _SalesOrderItemService.GetAllActiveItemsByOrderNo(frmcol[billno]);
                                if (SOItemActiveList.Count() == 0)
                                {
                                    SODetails.Status = "InActive";
                                    _SalesOrderService.Update(SODetails);
                                }
                            }
                            else if (frmcol[billno].Contains("DC"))
                            {
                                var dcdata = _DeliveryChallanItemService.GetDetailsByItemCodeAndChallanNo(frmcol[itemcode], frmcol[billno]);
                                dcdata.DelBalance = dcdata.DelBalance - (model.RetailBillItemDetails.Quantity);

                                //if we send all quantity then inactive that item
                                if (dcdata.DelBalance <= 0)
                                {
                                    dcdata.Status = "InActive";
                                    dcdata.DelBalance = 0;
                                }
                                _DeliveryChallanItemService.Update(dcdata);

                                var DCDetails = _DeliveryChallanService.GetDetailsByChallanNo(frmcol[billno]);
                                var DCItemActiveList = _DeliveryChallanItemService.GetAllActiveItemsByChallanNo(frmcol[billno]);
                                if (DCItemActiveList.Count() == 0)
                                {
                                    DCDetails.Status = "InActive";
                                    _DeliveryChallanService.Update(DCDetails);
                                }
                            }
                        }

                        //THIS IS FOR UPDATE STOCK
                        if (frmcol[itemtype] == "Inventory")
                        {
                            //IF NEW INVENTORY/NON-INVENTORY ITEM IS ADD IN RETAIL BILL
                            if (frmcol[billno] == null)
                            {
                                //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - model.RetailBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }

                            //ONLY QUOTATION AND SALES ORDER ITEMS WILL BE MINUS INTO THE STOCK
                            else if (frmcol[billno].Contains("Qu") || frmcol[billno].Contains("SO"))
                            {
                                //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - model.RetailBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }
                        }
                    }
                }
            }

            // IF WE FILL PAYMENT DETAILS THEN SAVE PAYMENT DETAILS IN CASHIER RECEIVABLES AND CASHIER RETAIL BILL..
            if (frmcol["PaymentDetails"] == "Yes")
            {
                var csahierdata = _CashierReceivableService.GetLastRow();
                int CashierVal = 0;
                int newlength = 0;
                if (csahierdata != null)
                {
                    CashierVal = csahierdata.Id;
                    CashierVal = CashierVal + 1;
                    newlength = CashierVal.ToString().Length;
                }
                else
                {
                    CashierVal = 1;
                    newlength = 1;
                }
                string cashiercode = _UtilityService.getName("CHR", newlength, CashierVal);

                // USE SAME MAIN APP OBJECT IN VIEW
                model.CashierRetailBillDetails = new CashierRetailBill();
                model.CashierRetailBillItemDetails = new CashierRetailBillItem();

                // SAVE CASHIER RECEIVABLE 
                model.CashierReceivableDetails.CashierCode = cashiercode;
                model.CashierReceivableDetails.Date = DateTime.Now;
                model.CashierReceivableDetails.BillType = "RetailBill";
                model.CashierReceivableDetails.Billno = model.RetailBillDetails.RetailBillNo;
                model.CashierReceivableDetails.ClientName = model.RetailBillDetails.ClientName;
                model.CashierReceivableDetails.ClientContact = model.RetailBillDetails.ClientContact;
                model.CashierReceivableDetails.ClientState = frmcol["ClientState"];
                model.CashierReceivableDetails.ClientType = frmcol["ClientType"];
                model.CashierReceivableDetails.ClientFormType = frmcol["FormType"];

                model.CashierReceivableDetails.PackAndForwd = 0;
                model.CashierReceivableDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                model.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                model.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(model.RetailBillDetails.Payment);
                model.CashierReceivableDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                model.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                model.CashierReceivableDetails.PaymentType = frmcol["CashierPaymentType"];

                model.CashierReceivableDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                model.CashierReceivableDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                model.CashierReceivableDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                model.CashierReceivableDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                model.CashierReceivableDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                model.CashierReceivableDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                model.CashierReceivableDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                model.CashierReceivableDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                model.CashierReceivableDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                model.CashierReceivableDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                model.CashierReceivableDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                model.CashierReceivableDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                model.CashierReceivableDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                model.CashierReceivableDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                model.CashierReceivableDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                model.CashierReceivableDetails.SelectedCard = frmcol["Card"];
                model.CashierReceivableDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    model.CashierReceivableDetails.CreditCardAmount = 0;
                    model.CashierReceivableDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    model.CashierReceivableDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    model.CashierReceivableDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                model.CashierReceivableDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                model.CashierReceivableDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                model.CashierReceivableDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                model.CashierReceivableDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                model.CashierReceivableDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                model.CashierReceivableDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    model.CashierReceivableDetails.DebitCardAmount = 0;
                    model.CashierReceivableDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    model.CashierReceivableDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    model.CashierReceivableDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                model.CashierReceivableDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                model.CashierReceivableDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                {
                    model.CashierReceivableDetails.ChequeAmount = 0;
                    model.CashierReceivableDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    model.CashierReceivableDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    model.CashierReceivableDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                }
                if (model.CashierReceivableDetails.ChequeNo != null && model.CashierReceivableDetails.ChequeNo != "")
                {
                    model.CashierReceivableDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    model.CashierReceivableDetails.ChequeDate = null;
                }
                model.CashierReceivableDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];

                model.CashierReceivableDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                model.CashierReceivableDetails.Status = "Active";
                model.CashierReceivableDetails.ModifiedOn = DateTime.Now;
                _CashierReceivableService.Create(model.CashierReceivableDetails);

                // SAVE CASHIER RETAIL BILL
                model.CashierRetailBillDetails.CashierCode = cashiercode;
                model.CashierRetailBillDetails.Date = DateTime.Now;
                model.CashierRetailBillDetails.RetailBillDate = DateTime.Now;
                model.CashierRetailBillDetails.RetailBillNo = model.RetailBillDetails.RetailBillNo;
                //main.CashierRetailBillDetails.BillDateStatus = "Same";
                model.CashierRetailBillDetails.ClientName = model.RetailBillDetails.ClientName;
                model.CashierRetailBillDetails.ClientContact = model.RetailBillDetails.ClientContact;
                model.CashierRetailBillDetails.ClientState = frmcol["ClientState"];
                model.CashierRetailBillDetails.ClientType = frmcol["ClientType"];
                model.CashierRetailBillDetails.FormType = frmcol["FormType"];
                model.CashierRetailBillDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                model.CashierRetailBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                model.CashierRetailBillDetails.Payment = Convert.ToDouble(model.RetailBillDetails.Payment);
                model.CashierRetailBillDetails.TotalAdvancePayment = Convert.ToDouble(model.RetailBillDetails.Payment);
                model.CashierRetailBillDetails.SavingFrom = "RetailBill";
                model.CashierRetailBillDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                model.CashierRetailBillDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                model.CashierRetailBillDetails.RefundAmount = model.RetailBillDetails.Refund;
                model.CashierRetailBillDetails.AdjustedAmount = 0;
                model.CashierRetailBillDetails.PaymentType = frmcol["CashierPaymentType"];
                model.CashierRetailBillDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                model.CashierRetailBillDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                model.CashierRetailBillDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                model.CashierRetailBillDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                model.CashierRetailBillDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                model.CashierRetailBillDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                model.CashierRetailBillDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                model.CashierRetailBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                model.CashierRetailBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                model.CashierRetailBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                model.CashierRetailBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                model.CashierRetailBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                model.CashierRetailBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                model.CashierRetailBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                model.CashierRetailBillDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                model.CashierRetailBillDetails.Refund = model.CashierReceivableDetails.Refund;
                model.CashierRetailBillDetails.SelectedCard = frmcol["Card"];

                model.CashierRetailBillDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    model.CashierRetailBillDetails.CreditCardAmount = 0;
                    model.CashierRetailBillDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    model.CashierRetailBillDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    model.CashierRetailBillDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                model.CashierRetailBillDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                model.CashierRetailBillDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                model.CashierRetailBillDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                model.CashierRetailBillDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                model.CashierRetailBillDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                model.CashierRetailBillDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    model.CashierRetailBillDetails.DebitCardAmount = 0;
                    model.CashierRetailBillDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    model.CashierRetailBillDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    model.CashierRetailBillDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                model.CashierRetailBillDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                model.CashierRetailBillDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                {
                    model.CashierRetailBillDetails.ChequeAmount = 0;
                    model.CashierRetailBillDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    model.CashierRetailBillDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    model.CashierRetailBillDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                }
                if (model.CashierRetailBillDetails.ChequeNo != null && model.CashierReceivableDetails.ChequeNo != "")
                {
                    model.CashierRetailBillDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    model.CashierRetailBillDetails.ChequeDate = null;
                }
                model.CashierRetailBillDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];

                model.CashierRetailBillDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                model.CashierRetailBillDetails.Status = "Active";
                model.CashierRetailBillDetails.ModifiedOn = DateTime.Now;

                //SAVE HANDOVER STATUS
                if (model.CashierRetailBillDetails.CreditCardAmount == 0 && model.CashierRetailBillDetails.DebitCardAmount == 0 && model.CashierRetailBillDetails.ChequeAmount == 0)
                {
                    model.CashierRetailBillDetails.HandoverStatus = "InActive";
                }
                else
                {
                    model.CashierRetailBillDetails.HandoverStatus = "Active";
                }
                //main.CashierRetailBillDetails.SalesReturn = "No";
                _CashierRetailBillService.Create(model.CashierRetailBillDetails);

                // SAVE CASHIER RETAIL BILL ITEM
                model.RetailBillItemList = _RetailBillItemService.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);
                foreach (var item in model.RetailBillItemList)
                {
                    model.CashierRetailBillItemDetails.CashierCode = cashiercode;
                    model.CashierRetailBillItemDetails.RetailInvoiceNo = model.RetailBillDetails.RetailBillNo;
                    model.CashierRetailBillItemDetails.ItemName = item.ItemName;
                    model.CashierRetailBillItemDetails.ItemCode = item.ItemCode;
                    model.CashierRetailBillItemDetails.Description = item.Description;
                    model.CashierRetailBillItemDetails.Material = item.Material;
                    model.CashierRetailBillItemDetails.Color = item.Color;
                    model.CashierRetailBillItemDetails.SellingPrice = item.SellingPrice;
                    model.CashierRetailBillItemDetails.MRP = item.MRP;
                    model.CashierRetailBillItemDetails.Quantity = item.Quantity;
                    model.CashierRetailBillItemDetails.Amount = item.Amount;
                    model.CashierRetailBillItemDetails.DesignCode = item.DesignCode;
                    model.CashierRetailBillItemDetails.DesignName = item.DesignName;
                    model.CashierRetailBillItemDetails.Size = item.Size;
                    model.CashierRetailBillItemDetails.Brand = item.Brand;
                    model.CashierRetailBillItemDetails.Barcode = item.Barcode;
                    model.CashierRetailBillItemDetails.Unit = item.Unit;
                    model.CashierRetailBillItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                    model.CashierRetailBillItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                    model.CashierRetailBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                    model.CashierRetailBillItemDetails.Status = "Active";
                    model.CashierRetailBillItemDetails.ModifiedOn = DateTime.Now;
                    _CashierRetailBillItemService.Create(model.CashierRetailBillItemDetails);
                }

                var RetailBillData = _RetailBillService.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);

                //if bill balance is 0 and cashier details also created then inactive that RETAIL BILL..
                double retailbillbal = Convert.ToDouble(model.RetailBillDetails.Balance);
                double retailbillref = Convert.ToDouble(model.RetailBillDetails.Refund);
                if (retailbillbal == 0 && retailbillref == 0)
                {
                    RetailBillData.Status = "InActive";
                    RetailBillData.CashierStatus = "InActive";
                    _RetailBillService.Update(RetailBillData);
                }
                //if we save the cashier payment details in retail bill then cashier status is ProcessInStatus for that bill..
                else if (retailbillbal == 0)
                {
                    RetailBillData.Status = "InActive";
                    _RetailBillService.Update(RetailBillData);
                }
            }

            TempData["ClientName"] = model.RetailBillDetails.ClientName;
            TempData["RetailBill"] = model.RetailBillDetails.RetailBillNo;

            //REMOVE PREVIOUS INVENTORY TAXES
            var invtaxdetails = _InventoryTaxService.GetTaxesByCode(model.RetailBillDetails.RetailBillNo);
            foreach (var data in invtaxdetails)
            {
                _InventoryTaxService.Delete(data);
            }

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = model.RetailBillDetails.RetailBillNo;
                    model.InventoryTaxDetails.Amount = frmcol[amountontax];
                    model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }

            //get retail bill after reset ID
            int RBAfterResetRow = _RetailBillService.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo).RetailMasterId;
            var RetailId = Encode(RBAfterResetRow.ToString());
            TempData["LastRetailBillId"] = RBAfterResetRow;

            if (retailbilltype == "MultipleBill")
            {
                if (model.RetailBillDetails.Balance == 0)
                {
                    return RedirectToAction("RetailBillCreateDetails/" + RetailId);
                }
                else
                {
                    TempData["BillPayment"] = model.RetailBillDetails.Balance;
                    return RedirectToAction("AdjustAmountBackView", "RetailBill");
                }
            }
            else
            {
                return RedirectToAction("RetailBillCreateDetails/" + RetailId);
            }
        }



        //BACK PAGE OF ADJUST AMOUNT POP UP
        [HttpGet]
        public ActionResult AdjustAmountBackView()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //ADJUST AMOUNT YES/NO CONDITION POP UP
        [HttpGet]
        public ActionResult AdjustAmountPopup()
        {
            return View();
        }

        //BILLS DETAILS FOR ADJUST AMOUNT
        [HttpGet]
        public ActionResult AdjustAmountBills()
        {
            MainApplication model = new MainApplication();
            string clientname = TempData["ClientName"].ToString();
            TempData["ClientName"] = clientname;
            model.SalesOrderList = _SalesOrderService.GetDetailsByClientAndBalance(clientname);
            return View(model);
        }

        [HttpPost]
        public ActionResult AdjustAmountBills(FormCollection frmcol)
        {
            string retailbill = TempData["RetailBill"].ToString();

            string MultipleSO = string.Empty;
            int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);
            for (int i = 1; i <= rowcount; i++)
            {
                string billnumberval = "BillNoVal" + i;
                string adjustamount = "AdjustAmount" + i;
                string balance = "BalanceVal" + i;
                if (frmcol[adjustamount] != "" && Convert.ToDouble(frmcol[adjustamount]) != 0)
                {
                    //MINUS ADJUSTED AMOUNT INTO SO ADVANCE PAYMENT
                    var SOData = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol[billnumberval]);
                    SOData.RemainingAdvance = Convert.ToDouble(frmcol[balance]);
                    if (SOData.AdjustedAmount != 0)
                    {
                        SOData.AdjustedAmount = SOData.AdjustedAmount + Convert.ToDouble(frmcol[adjustamount]);
                    }
                    else
                    {
                        SOData.AdjustedAmount = Convert.ToDouble(frmcol[adjustamount]);
                    }
                    if (SOData.AdjustedForBill == null)
                    {
                        SOData.AdjustedForBill = retailbill;
                    }
                    else
                    {
                        SOData.AdjustedForBill = SOData.AdjustedForBill + "," + retailbill;
                    }
                    if (SOData.RemainingAdvance == 0)
                    {
                        SOData.Status = "InActive";
                    }
                    _SalesOrderService.Update(SOData);

                    //SAVE ADJUSTED SALESORDER IN RETAIL BILL
                    if (SOData.AdjustedAmount != 0)
                    {
                        if (MultipleSO == "")
                        {
                            MultipleSO = frmcol[billnumberval];
                        }
                        else
                        {
                            MultipleSO = MultipleSO + "," + frmcol[billnumberval];
                        }
                    }
                }
            }

            //ADJUSTED AMOUNT SAVE IN RETAIL BILL
            var retailbilldata = _RetailBillService.GetDetailsByInvoiceNo(retailbill);
            retailbilldata.AdjustedBill = MultipleSO;
            retailbilldata.AdjustedAmount = Convert.ToDouble(frmcol["TotalAdjustAmount"]);

            //if bal < 0 then calculate refund
            var bal = retailbilldata.Balance - Convert.ToDouble(frmcol["TotalAdjustAmount"]);
            if (bal <= 0)
            {
                //save refund in retail bill
                var refund = Convert.ToDouble(frmcol["TotalAdjustAmount"]) - retailbilldata.Balance;
                retailbilldata.Refund = refund;
                //if refund is 0 then inactive cashier status of that salesbill..
                if (retailbilldata.Refund == 0)
                {
                    retailbilldata.CashierStatus = "InActive";
                }
                retailbilldata.Balance = 0;
                retailbilldata.Status = "InActive";
            }
            else
            {
                retailbilldata.Balance = bal;
            }
            _RetailBillService.Update(retailbilldata);

            //SAVE DATA IN SALES BILL ADJUSTED AMOUNT DETAILS TABLE..
            MainApplication model = new MainApplication()
            {
                RetailBillAdjAmtDetails = new RetailBillAdjAmtDetail(),
            };

            for (int i = 1; i <= rowcount; i++)
            {
                string billnumberval = "BillNoVal" + i;
                string adjustamount = "AdjustAmount" + i;
                string balance = "BalanceVal" + i;

                if (frmcol[adjustamount] != "" && Convert.ToDouble(frmcol[adjustamount]) != 0)
                {
                    model.RetailBillAdjAmtDetails.AdjustedBill = frmcol[billnumberval];
                    model.RetailBillAdjAmtDetails.AdjustedAmount = Convert.ToDouble(frmcol[adjustamount]);
                    model.RetailBillAdjAmtDetails.RetailBillNo = retailbill;
                    model.RetailBillAdjAmtDetails.Status = "Active";
                    model.RetailBillAdjAmtDetails.ModifiedOn = DateTime.Now;
                    _RetailBillAdjAmtDetailService.Create(model.RetailBillAdjAmtDetails);
                }
            }

            return RedirectToAction("ClosePopUp", "ResetBills");
        }

        //CLOSE POPUP AFTER ADJUST AMOUNT
        [HttpGet]
        public ActionResult ClosePopUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RetailBillCreateDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.RetailBillDetails = _RetailBillService.GetDetailsById(Id);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.RetailBillDetails.RetailBillNo);
            model.RetailBillItemList = _RetailBillItemService.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);
            string previousretailno = TempData["PreviousRetailno"].ToString();
            if (TempData["PreviousRetailno"].ToString() != model.RetailBillDetails.RetailBillNo)
            {
                ViewData["RetailnoChanged"] = previousretailno + " is replaced with " + model.RetailBillDetails.RetailBillNo + " because " + previousretailno + " is acquired by another person";
            }
            TempData["PreviousRetailno"] = previousretailno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintRetailBill(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RetailBillDetails = _RetailBillService.GetDetailsById(Id);
            model.RetailBillItemList = _RetailBillItemService.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);
            return View(model);
        }



        /***********************************************RESET SALES BILL*******************************************/

        //AUTO COMPLETE RETAILBILL NO
        [HttpGet]
        public JsonResult GetSalesBillNos(string SearchTerm)
        {
            var data = _SalesBillService.GetAllSalesBill(SearchTerm);
            List<string> No = new List<string>();
            foreach (var item in data)
            {
                No.Add(item.SalesBillNo);
            }
            return Json(new { No }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY SALES BILL DETAILS ON POP UP PAGE
        public ActionResult SalesBillDetails(string no)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesBillDetails = _SalesBillService.GetDetailsBySalesBillNo(no);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(no);
            model.SalesBillAdjAmtDetailList = _SalesBillAdjAmtDetailService.GetBillsBySalesNo(no);
            return View(model);
        }

        //RESET SALES BILL
        [HttpGet]
        public ActionResult ResetSalesBill(string salesbill)
        {
            MainApplication model = new MainApplication()
            {
                SalesBillDetails = new SalesBill(),
            };
            var salesbilldetails = _SalesBillService.GetDetailsBySalesBillNo(salesbill);
            model.SalesBillDetails.SalesBillNo = salesbill;
            model.SalesBillDetails.Id = salesbilldetails.Id;
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["PrevSalesBill"] = salesbilldetails;
            TempData["PreviousSalesBillNo"] = salesbill;
            return View(model);
        }

        //GET MULTIPLE BILL ITEM DETAILS POP UP PAGE
        [HttpGet]
        public ActionResult GetClientPendingBillsForSalesBill(string BillNo, string ShopCode)
        {
            MainApplication model = new MainApplication();
            if (BillNo.Contains("Qu"))
            {
                var clientdetails = _QuotationService.GetDetailsByQuotNo(BillNo);
                List<QuotationItem> QUItemList = new List<QuotationItem>();
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<DeliveryChallanItem> DCItemList = new List<DeliveryChallanItem>();

                //Get quotation items by clientname
                var ClientQU = _QuotationService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientQuots in ClientQU)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientQuots.QuotNo);
                    foreach (var item in quitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            //calculate shop quantity..
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        QUItemList.Add(item);
                    }
                }
                model.QuotationItemList = QUItemList;

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);
                    foreach (var item in soitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get challan items by clientname
                var ClientDC = _DeliveryChallanService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientChallans in ClientDC)
                {
                    var dcitems = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(clientChallans.ChallanNo);
                    foreach (var item in dcitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        DCItemList.Add(item);
                    }
                }
                model.DeliveryChallanItemList = DCItemList;
            }

            else if (BillNo.Contains("SO"))
            {
                var clientdetails = _SalesOrderService.GetDetailsByOrderNo(BillNo);
                List<QuotationItem> QUItemList = new List<QuotationItem>();
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<DeliveryChallanItem> DCItemList = new List<DeliveryChallanItem>();

                //Get quotation items by clientname
                var ClientQU = _QuotationService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientQuots in ClientQU)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientQuots.QuotNo);
                    foreach (var item in quitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();


                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        QUItemList.Add(item);
                    }
                }
                model.QuotationItemList = QUItemList;

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);
                    foreach (var item in soitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get challan items by clientname
                var ClientDC = _DeliveryChallanService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientChallans in ClientDC)
                {
                    var dcitems = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(clientChallans.ChallanNo);
                    foreach (var item in dcitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();


                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        DCItemList.Add(item);
                    }
                }
                model.DeliveryChallanItemList = DCItemList;
            }

            else
            {
                var clientdetails = _DeliveryChallanService.GetDetailsByChallanNo(BillNo);
                List<QuotationItem> QUItemList = new List<QuotationItem>();
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<DeliveryChallanItem> DCItemList = new List<DeliveryChallanItem>();

                //Get quotation items by clientname
                var ClientQU = _QuotationService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientQuots in ClientQU)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientQuots.QuotNo);
                    foreach (var item in quitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();


                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        QUItemList.Add(item);
                    }
                }
                model.QuotationItemList = QUItemList;

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);
                    foreach (var item in soitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get challan items by clientname
                var ClientDC = _DeliveryChallanService.GetDetailsByClientName(clientdetails.ClientName);
                foreach (var clientChallans in ClientDC)
                {
                    var dcitems = _DeliveryChallanItemService.GetDetailsByChallanNoAndStatus(clientChallans.ChallanNo);
                    foreach (var item in dcitems)
                    {
                        if (item.ItemType == "Inventory")
                        {
                            var stkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, ShopCode);
                            if (stkqty == null)
                            {
                                item.ReadOnlyShopQuantity = 0;
                            }
                            else
                            {
                                item.ReadOnlyShopQuantity = Convert.ToDouble(stkqty.Quantity);
                            }
                            item.Barcode = item.Barcode.Substring(0, 9).ToString();

                            //calculate total stock quantity (shop quantity + godown quantity)
                            double? shopqty = 0;
                            double? godownqty = 0;
                            double? totalstkqty = 0;
                            var shopstockitems = _ShopStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in shopstockitems)
                            {
                                shopqty = shopqty + data.Quantity;
                            }
                            var godownstockitems = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            foreach (var data in godownstockitems)
                            {
                                godownqty = godownqty + data.Quantity;
                            }
                            totalstkqty = shopqty + godownqty;

                            item.ReadOnlyTotalStockQuantity = totalstkqty;
                        }
                        DCItemList.Add(item);
                    }
                }
                model.DeliveryChallanItemList = DCItemList;
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetBarcodeItemDetailsForSB(string barcode, string shop, string ClientType, string ClientState, string FormType, string ConsumeResell)
        {
            var barcodevalue = barcode.ToUpper() + ".png";
            var data = _ShopStockService.GetDetailsByBarcode(barcodevalue);

            if (data != null)
            {
                double? discount = 0;
                double? discountrps = 0;
                var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(data.ItemCode);
                if (discountdetails != null)
                {
                    discount = discountdetails.DiscInPercentage;
                    discountrps = discountdetails.DiscInRupees;
                }
                //calculate total quantity (shop quantity + godown quantity)
                double? shopqty = 0;
                double? godownqty = 0;
                double? totalstkqty = 0;
                var shopstockitems = _ShopStockService.GetRowsByItemCode(data.ItemCode);
                foreach (var item in shopstockitems)
                {
                    shopqty = shopqty + item.Quantity;
                }

                var godownstockitems = _GodownStockService.GetRowsByItemCode(data.ItemCode);
                foreach (var item in godownstockitems)
                {
                    godownqty = godownqty + item.Quantity;
                }
                totalstkqty = shopqty + godownqty;

                var itemdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(data.ItemCode, shop);
                if (itemdetails != null)
                {
                    string costprice = _ItemService.GetDescriptionByItemCode(data.ItemCode).costprice;
                    string subcat = _ItemService.GetDescriptionByItemCode(data.ItemCode).itemSubCategory;
                    double taxPercent = 0;
                    if (ClientState == "Maharashtra")
                    {
                        if (ClientType == "Registered")
                        {
                            if (ConsumeResell != "Consumer")
                            {
                                taxPercent = 0;
                            }
                            else
                            {
                                var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                                if (Tax != null)
                                {
                                    taxPercent = Tax.Percentage;
                                }
                                else { taxPercent = 0; }
                            }
                        }
                        else
                        {
                            var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                            if (Tax != null)
                            {
                                taxPercent = Tax.Percentage;
                            }
                            else { taxPercent = 0; }
                        }
                    }
                    else
                    {
                        if (ClientType == "Registered")
                        {
                            if (ConsumeResell == "Reseller")
                            {
                                var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                                if (Tax != null)
                                {
                                    if (FormType == "C-Form")
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
                                var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                                if (Tax != null)
                                {
                                    taxPercent = Tax.Percentage;
                                }
                                else { taxPercent = 0; }
                            }
                        }
                        else
                        {
                            var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                            if (Tax != null)
                            {
                                taxPercent = Tax.Percentage;
                            }
                            else { taxPercent = 0; }
                        }
                    }
                    return Json(new
                    {
                        discount,
                        discountrps,
                        itemdetails.ItemCode,
                        itemdetails.ItemName,
                        itemdetails.Description,
                        itemdetails.Quantity,
                        costprice,
                        itemdetails.SellingPrice,
                        itemdetails.MRP,
                        itemdetails.Unit,
                        itemdetails.NumberType,
                        itemdetails.Color,
                        itemdetails.Material,
                        itemdetails.Size,
                        itemdetails.Brand,
                        itemdetails.DesignCode,
                        itemdetails.DesignName,
                        taxPercent,
                        totalstkqty
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string msg = "Fail";
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //DISPLAY ITEM DISTAILS BY NONINVENTORY ITEM CODE
        [HttpGet]
        public JsonResult GetNonInvItemdetailsByItemCodeForSb(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            string subcat = _NonInventoryItemService.GetDetailsByItemCode(ItemCode).itemSubCategory;
            var itemdetails = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            double taxPercent = 0;
            if (ClientState == "Maharashtra")
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell == "Reseller")
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            if (FormType == "C-Form")
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
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }

            if (itemdetails.costprice == "")
            {
                itemdetails.costprice = "0";
            }
            return Json(new
            {
                discount,
                discountrps,
                itemdetails.itemName,
                itemdetails.itemCode,
                itemdetails.description,
                itemdetails.costprice,
                itemdetails.sellingprice,
                itemdetails.mrp,
                itemdetails.unit,
                itemdetails.NumberType,
                itemdetails.colorCode,
                itemdetails.typeOfMaterial,
                itemdetails.size,
                itemdetails.brandName,
                itemdetails.designCode,
                itemdetails.designName,
                taxPercent
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ResetSalesBill(MainApplication model, FormCollection frmcol)
        {
            MainApplication mainapp = new MainApplication()
            {
                ResetSalesBillDetails = new ResetSalesBill(),
                ResetSalesBillItemDetails = new ResetSalesBillItem(),
                ResetSalesBillAdjAmtDetails = new ResetSalesBillAdjAmtDetail(),
            };

            var salesbillno = model.SalesBillDetails.SalesBillNo;

            // ****************** REPLACE(UPDATE) ACTUAL SALES BILL WITH NEW BILL AFTER RESET SALES BILL *******************
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string SalesBillCode = model.SalesBillDetails.SalesBillNo;

            //SAVE DIRECT(COUNTER) SALES BILL AND SALES BILL USING MULTIPLE BILLS..
            model.SalesBillDetails.Date = DateTime.Now;
            model.SalesBillDetails.SalesBillNo = SalesBillCode;
            model.SalesBillDetails.ShopCode = shopcode;
            model.SalesBillDetails.ShopName = shopname;
            model.SalesBillDetails.TotalAmount = Convert.ToDouble(frmcol["Total_Amount_Value"]);
            model.SalesBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
            model.SalesBillDetails.GrandTotal = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            model.SalesBillDetails.TaxStatus = "WithTax";
            model.SalesBillDetails.Balance = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            model.SalesBillDetails.Refund = 0;
            model.SalesBillDetails.AdjustedAmount = 0;
            model.SalesBillDetails.PaymentReceived = 0;
            model.SalesBillDetails.RefundToClient = 0;
            model.SalesBillDetails.BillDiscount = 0;
            model.SalesBillDetails.Status = "Active";
            model.SalesBillDetails.ModifiedOn = DateTime.Now;
            model.SalesBillDetails.CashierStatus = "Active";
            model.SalesBillDetails.SalesReturn = "No";
            _SalesBillService.Update(model.SalesBillDetails);

            // SAVE MAIN RESET SALES BILL
            var salesbilldetails = TempData["PrevSalesBill"] as SalesBill;
            mainapp.ResetSalesBillDetails.SalesBillNo = salesbilldetails.SalesBillNo;
            mainapp.ResetSalesBillDetails.Date = salesbilldetails.Date;
            mainapp.ResetSalesBillDetails.QuotationNo = salesbilldetails.QuotationNo;
            mainapp.ResetSalesBillDetails.SalesOrderNo = salesbilldetails.SalesOrderNo;
            mainapp.ResetSalesBillDetails.DeliveryChallanNo = salesbilldetails.DeliveryChallanNo;
            mainapp.ResetSalesBillDetails.ClientName = salesbilldetails.ClientName;
            mainapp.ResetSalesBillDetails.ClientAddress = salesbilldetails.ClientAddress;
            mainapp.ResetSalesBillDetails.ClientEmail = salesbilldetails.ClientEmail;
            mainapp.ResetSalesBillDetails.ClientContactNo = salesbilldetails.ClientContactNo;
            mainapp.ResetSalesBillDetails.ClientType = salesbilldetails.ClientType;
            mainapp.ResetSalesBillDetails.ConsumeResell = salesbilldetails.ConsumeResell;
            mainapp.ResetSalesBillDetails.FormType = salesbilldetails.FormType;
            mainapp.ResetSalesBillDetails.TransportMode = salesbilldetails.TransportMode;
            mainapp.ResetSalesBillDetails.TransportName = salesbilldetails.TransportName;
            mainapp.ResetSalesBillDetails.TransportContact = salesbilldetails.TransportContact;
            mainapp.ResetSalesBillDetails.ShopCode = salesbilldetails.ShopCode;
            mainapp.ResetSalesBillDetails.ShopName = salesbilldetails.ShopName;
            mainapp.ResetSalesBillDetails.ShopAddress = salesbilldetails.ShopAddress;
            mainapp.ResetSalesBillDetails.ShopContactPerson = salesbilldetails.ShopContactPerson;
            mainapp.ResetSalesBillDetails.ShopEmail = salesbilldetails.ShopEmail;
            mainapp.ResetSalesBillDetails.SalesPersonName = salesbilldetails.SalesPersonName;
            mainapp.ResetSalesBillDetails.SalesPersonAddress = salesbilldetails.SalesPersonAddress;
            mainapp.ResetSalesBillDetails.SalesPersonEmail = salesbilldetails.SalesPersonEmail;
            mainapp.ResetSalesBillDetails.SalesPersonContact = salesbilldetails.SalesPersonContact;
            mainapp.ResetSalesBillDetails.SalesPersonDesignation = salesbilldetails.SalesPersonDesignation;
            mainapp.ResetSalesBillDetails.TotalAmount = salesbilldetails.TotalAmount;
            mainapp.ResetSalesBillDetails.PackAndForwd = salesbilldetails.PackAndForwd;
            mainapp.ResetSalesBillDetails.TotalTaxAmount = salesbilldetails.TotalTaxAmount;
            mainapp.ResetSalesBillDetails.GrandTotal = salesbilldetails.GrandTotal;
            mainapp.ResetSalesBillDetails.BillDiscount = salesbilldetails.BillDiscount;
            mainapp.ResetSalesBillDetails.PaymentReceived = salesbilldetails.PaymentReceived;
            mainapp.ResetSalesBillDetails.Balance = salesbilldetails.Balance;
            mainapp.ResetSalesBillDetails.Refund = salesbilldetails.Refund;
            mainapp.ResetSalesBillDetails.TotalDiscount = salesbilldetails.TotalDiscount;
            mainapp.ResetSalesBillDetails.AdjustedStatus = salesbilldetails.AdjustedStatus;
            mainapp.ResetSalesBillDetails.AdjustedAmount = salesbilldetails.AdjustedAmount;
            mainapp.ResetSalesBillDetails.RefundToClient = salesbilldetails.RefundToClient;
            mainapp.ResetSalesBillDetails.AdjustedBill = salesbilldetails.AdjustedBill;
            mainapp.ResetSalesBillDetails.CashierStatus = salesbilldetails.CashierStatus;
            mainapp.ResetSalesBillDetails.PreparedBy = salesbilldetails.PreparedBy;
            mainapp.ResetSalesBillDetails.Status = salesbilldetails.Status;
            mainapp.ResetSalesBillDetails.ModifiedOn = salesbilldetails.ModifiedOn;
            mainapp.ResetSalesBillDetails.SalesReturnNo = salesbilldetails.SalesReturnNo;
            mainapp.ResetSalesBillDetails.SalesReturn = salesbilldetails.SalesReturn;
            mainapp.ResetSalesBillDetails.CreditNoteNo = salesbilldetails.CreditNoteNo;
            mainapp.ResetSalesBillDetails.CreditNoteAmount = salesbilldetails.CreditNoteAmount;
            mainapp.ResetSalesBillDetails.TaxStatus = salesbilldetails.TaxStatus;
            _ResetSalesBillService.Create(mainapp.ResetSalesBillDetails);

            //IF RESET BILL IS WITH SALES ORDER AMOUNT ADJUSTMENT THEN AGAIN LIVE THIS ORDER..
            var AdjustedSalesOrders = _SalesBillAdjAmtDetailService.GetBillsBySalesNo(salesbillno);
            foreach (var order in AdjustedSalesOrders)
            {
                var SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(order.AdjustedBill);
                SalesOrderDetails.Status = "Active";
                SalesOrderDetails.RemainingAdvance = SalesOrderDetails.RemainingAdvance + SalesOrderDetails.AdjustedAmount;
                _SalesOrderService.Update(SalesOrderDetails);
            }

            // SAVE RESET SALES BILL ITEMS
            var salesbillitemdetails = _SalesBillItemService.GetItemsBySalesBillNo(salesbillno);
            foreach (var data in salesbillitemdetails)
            {
                mainapp.ResetSalesBillItemDetails.SalesBillNo = data.SalesBillNo;
                mainapp.ResetSalesBillItemDetails.BillNo = data.BillNo;
                mainapp.ResetSalesBillItemDetails.ItemCode = data.ItemCode;
                mainapp.ResetSalesBillItemDetails.ItemName = data.ItemName;
                mainapp.ResetSalesBillItemDetails.ItemType = data.ItemType;
                mainapp.ResetSalesBillItemDetails.Barcode = data.Barcode;
                mainapp.ResetSalesBillItemDetails.Narration = data.Narration;
                mainapp.ResetSalesBillItemDetails.Description = data.Description;
                mainapp.ResetSalesBillItemDetails.Quantity = data.Quantity;
                mainapp.ResetSalesBillItemDetails.Unit = data.Unit;
                mainapp.ResetSalesBillItemDetails.NumberType = data.NumberType;
                mainapp.ResetSalesBillItemDetails.SellingPrice = data.SellingPrice;
                mainapp.ResetSalesBillItemDetails.MRP = data.MRP;
                mainapp.ResetSalesBillItemDetails.Size = data.Size;
                mainapp.ResetSalesBillItemDetails.Brand = data.Brand;
                mainapp.ResetSalesBillItemDetails.DesignCode = data.DesignCode;
                mainapp.ResetSalesBillItemDetails.DesignName = data.DesignName;
                mainapp.ResetSalesBillItemDetails.Amount = data.Amount;
                mainapp.ResetSalesBillItemDetails.Color = data.Color;
                mainapp.ResetSalesBillItemDetails.Material = data.Material;
                mainapp.ResetSalesBillItemDetails.DiscountPercent = data.DiscountPercent;
                mainapp.ResetSalesBillItemDetails.DiscountRPS = data.DiscountRPS;
                mainapp.ResetSalesBillItemDetails.ItemTax = data.ItemTax;
                mainapp.ResetSalesBillItemDetails.ItemTaxAmt = data.ItemTaxAmt;
                mainapp.ResetSalesBillItemDetails.ProportionateTax = data.ProportionateTax;
                mainapp.ResetSalesBillItemDetails.ProportionateTaxAmt = data.ProportionateTaxAmt;
                mainapp.ResetSalesBillItemDetails.SalesReturn = data.SalesReturn;
                mainapp.ResetSalesBillItemDetails.Status = data.Status;
                mainapp.ResetSalesBillItemDetails.ModifiedOn = data.ModifiedOn;
                mainapp.ResetSalesBillItemDetails.ReadOnlyTotalStockQuantity = data.ReadOnlyTotalStockQuantity;
                _ResetSalesBillItemService.Create(mainapp.ResetSalesBillItemDetails);

                //ADD RESET BILL ITEMS IN STOCK
                if (data.ItemType == "Inventory")
                {
                    if (data.BillNo == null || data.BillNo.Contains("Qu") || data.BillNo.Contains("SO"))
                    {
                        //UPDATE SHOP STOCK (SHOP STOCK QUANTITY + RESET BILL QUANTITY)
                        var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(mainapp.ResetSalesBillItemDetails.ItemCode, mainapp.ResetSalesBillDetails.ShopCode);
                        shopstockdata.Quantity = shopstockdata.Quantity + mainapp.ResetSalesBillItemDetails.Quantity;
                        _ShopStockService.Update(shopstockdata);

                        //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY + RESET BILL QUANTITY)
                        var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(mainapp.ResetSalesBillItemDetails.ItemCode, mainapp.ResetSalesBillDetails.ShopCode);
                        stkdisdata.ItemQuantity = stkdisdata.ItemQuantity + Convert.ToDouble(mainapp.ResetSalesBillItemDetails.Quantity);
                        _StockItemDistributionService.Update(stkdisdata);

                        //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY + RESET BILL QUANTITY)
                        var entrystkdata = _EntryStockItemService.getDetailsByItemCode(mainapp.ResetSalesBillItemDetails.ItemCode);
                        //if item not present in entry stock then opening stock
                        if (entrystkdata != null)
                        {
                            entrystkdata.TotalQuantity = entrystkdata.TotalQuantity + Convert.ToDouble(mainapp.ResetSalesBillItemDetails.Quantity);
                            _EntryStockItemService.Update(entrystkdata);
                        }
                        //UPDATE OPENING STOCK (OPENING STOCK QUANTITY + RESET BILL QUANTITY)
                        else
                        {
                            var openingstkdata = _OpeningStockService.GetDetailsByItemCode(mainapp.ResetSalesBillItemDetails.ItemCode);
                            openingstkdata.TotalQuantity = openingstkdata.TotalQuantity + Convert.ToDouble(mainapp.ResetSalesBillItemDetails.Quantity);
                            _OpeningStockService.UpdateStock(openingstkdata);
                        }
                    }
                }

                if (data.BillNo != null)
                {
                    if (data.BillNo.Contains("Qu"))
                    {
                        var quotitemdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(data.ItemCode, data.BillNo);
                        quotitemdata.Balance = quotitemdata.Balance + data.Quantity;
                        if (quotitemdata.Status == "InActive")
                        {
                            quotitemdata.Status = "Active";
                        }
                        _QuotationItemService.Update(quotitemdata);

                        var QuotDetails = _QuotationService.GetDetailsByQuotNo(data.BillNo);
                        if (QuotDetails.Status == "InActive")
                        {
                            QuotDetails.Status = "Active";
                            _QuotationService.Update(QuotDetails);
                        }
                    }
                    else if (data.BillNo.Contains("SO"))
                    {
                        var soitemdata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(data.ItemCode, data.BillNo);
                        soitemdata.SOBalance = soitemdata.SOBalance + data.Quantity;
                        if (soitemdata.Status == "InActive")
                        {
                            soitemdata.Status = "Active";
                        }
                        _SalesOrderItemService.Update(soitemdata);

                        var SODetails = _SalesOrderService.GetDetailsBySalesOrderNo(data.BillNo);
                        if (SODetails.Status == "InActive")
                        {
                            SODetails.Status = "Active";
                            _SalesOrderService.Update(SODetails);
                        }
                    }
                    else
                    {
                        var deliverychallanitem = _DeliveryChallanItemService.GetDetailsByItemCodeAndChallanNo(data.ItemCode, data.BillNo);
                        deliverychallanitem.DelBalance = deliverychallanitem.DelBalance + data.Quantity;
                        if (deliverychallanitem.Status == "InActive")
                        {
                            deliverychallanitem.Status = "Active";
                        }
                        _DeliveryChallanItemService.Update(deliverychallanitem);

                        var DCDetails = _DeliveryChallanService.GetDetailsByChallanNo(data.BillNo);
                        if (DCDetails.Status == "InActive")
                        {
                            DCDetails.Status = "Active";
                            _DeliveryChallanService.Update(DCDetails);
                        }
                    }
                }
            }

            //DELETE RESET SALES BILL ITEM ADJUST AMOUNT DETAILS
            var SBItemList = _SalesBillItemService.GetItemsBySalesBillNo(salesbillno);
            foreach (var row in SBItemList)
            {
                _SalesBillItemService.Delete(row);
            }

            // SAVE RESET SALES BILL ADJUST AMOUNT DETAILS
            var salesbilladjamtdetails = _SalesBillAdjAmtDetailService.GetBillsBySalesNo(salesbillno);
            foreach (var data in salesbilladjamtdetails)
            {
                mainapp.ResetSalesBillAdjAmtDetails.SalesBillNo = data.SalesBillNo;
                mainapp.ResetSalesBillAdjAmtDetails.AdjustedBill = data.AdjustedBill;
                mainapp.ResetSalesBillAdjAmtDetails.AdjustedAmount = data.AdjustedAmount;
                mainapp.ResetSalesBillAdjAmtDetails.Status = data.Status;
                mainapp.ResetSalesBillAdjAmtDetails.ModifiedOn = data.ModifiedOn;
                _ResetSalesBillAdjAmtDetailService.Create(mainapp.ResetSalesBillAdjAmtDetails);
            }

            //DELETE RESET BILL ADJUST AMOUNT DETAILS
            var SBAdjAmtList = _SalesBillAdjAmtDetailService.GetBillsBySalesNo(salesbillno);
            foreach (var row in SBAdjAmtList)
            {
                _SalesBillAdjAmtDetailService.Delete(row);
            }

            var salesbilltype = "";

            // SAVE DIRECT RETAIL BILL ITEMS
            if (model.SalesBillDetails.QuotationNo == null && model.SalesBillDetails.SalesOrderNo == null && model.SalesBillDetails.DeliveryChallanNo == null)
            {
                salesbilltype = "Direct";
                int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);

                for (int row = 1; row <= rowcount; row++)
                {
                    string itemcode = "ItemCodeVal" + row;
                    string itemname = "ItemName" + row;
                    string itemnameval = "ItemNameVal" + row;
                    string narration = "Narration" + row;
                    string itemtype = "ItemType" + row;
                    string barcode = "Barcode" + row;
                    string description = "Description" + row;
                    string quantity = "Quantity" + row;
                    string unit = "UnitVal" + row;
                    string numbertype = "NumberType" + row;
                    string rate = "Rate" + row;
                    string mrp = "MRP" + row;
                    string sellingprice = "SellingPrice" + row;
                    string amount = "AmountVal" + row;
                    string color = "Color" + row;
                    string material = "Material" + row;
                    string size = "Size" + row;
                    string brand = "Brand" + row;
                    string designcode = "DesignCode" + row;
                    string designnname = "DesignName" + row;
                    string disrs = "DisRs" + row;
                    string disper = "Disper" + row;
                    string proptax = "proportionatetaxvalue" + row;
                    string proptaxamt = "proportionatetaxamount" + row;
                    string itemtax = "ItemTaxVal" + row;
                    string itemtaxamt = "ItemTaxAmt" + row;

                    if (frmcol[quantity] != null)
                    {
                        model.SalesBillItemDetails = new SalesBillItem();
                        model.SalesBillItemDetails.SalesBillNo = SalesBillCode;
                        model.SalesBillItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.SalesBillItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            model.SalesBillItemDetails.ItemName = frmcol[itemnameval];
                        }
                        model.SalesBillItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.SalesBillItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                        }
                        model.SalesBillItemDetails.Narration = frmcol[narration];
                        model.SalesBillItemDetails.Description = frmcol[description];
                        model.SalesBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesBillItemDetails.Unit = frmcol[unit];
                        model.SalesBillItemDetails.NumberType = frmcol[numbertype];

                        model.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                        if (frmcol[mrp] != "")
                        {
                            model.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            model.SalesBillItemDetails.MRP = 0;
                        }
                        model.SalesBillItemDetails.Size = frmcol[size];
                        model.SalesBillItemDetails.Brand = frmcol[brand];
                        model.SalesBillItemDetails.DesignCode = frmcol[designcode];
                        model.SalesBillItemDetails.DesignName = frmcol[designnname];
                        model.SalesBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        model.SalesBillItemDetails.Color = frmcol[color];
                        model.SalesBillItemDetails.Material = frmcol[material];
                        model.SalesBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        model.SalesBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        model.SalesBillItemDetails.ItemTax = frmcol[itemtax];
                        model.SalesBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        model.SalesBillItemDetails.ProportionateTax = frmcol[proptax];
                        model.SalesBillItemDetails.ProportionateTaxAmt = frmcol[proptaxamt];
                        model.SalesBillItemDetails.Status = "Active";
                        model.SalesBillItemDetails.ModifiedOn = DateTime.Now;
                        model.SalesBillItemDetails.SalesReturn = "No";
                        _SalesBillItemService.Create(model.SalesBillItemDetails);

                        if (frmcol[itemtype] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - SALES BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.SalesBillItemDetails.ItemCode, model.SalesBillDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - model.SalesBillItemDetails.Quantity;
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.SalesBillItemDetails.ItemCode, model.SalesBillDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.SalesBillItemDetails.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.SalesBillItemDetails.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }
                    }
                }
            }
            // SAVE RETAIL BILL ITEMS USING MULTIPLE BILLS
            else
            {
                salesbilltype = "MultipleBill";
                int count = Convert.ToInt32(frmcol["hdnRowCount"]);
                for (int i = 1; i <= count; i++)
                {
                    string billno = "BillNo" + i;
                    string itemcode = "ItemCodeVal" + i;
                    string itemname = "ItemName" + i;
                    string itemnameval = "ItemNameVal" + i;
                    string itemtype = "ItemType" + i;
                    string barcode = "Barcode" + i;
                    string description = "Description" + i;
                    string quantity = "Quantity" + i;
                    string unit = "UnitVal" + i;
                    string numbertype = "NumberType" + i;
                    string rate = "Rate" + i;
                    string mrp = "MRP" + i;
                    string sellingprice = "SellingPrice" + i;
                    string amount = "AmountVal" + i;
                    string color = "Color" + i;
                    string material = "Material" + i;
                    string size = "Size" + i;
                    string brand = "Brand" + i;
                    string designcode = "DesignCode" + i;
                    string designnname = "DesignName" + i;
                    string narration = "Narration" + i;
                    string disrs = "DisRs" + i;
                    string disper = "Disper" + i;
                    string proptax = "proportionatetaxvalue" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    string itemtax = "ItemTaxVal" + i;
                    string itemtaxamt = "ItemTaxAmt" + i;

                    if (frmcol[quantity] != null)
                    {
                        model.SalesBillItemDetails = new SalesBillItem();
                        model.SalesBillItemDetails.SalesBillNo = SalesBillCode;
                        model.SalesBillItemDetails.BillNo = frmcol[billno];
                        model.SalesBillItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.SalesBillItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            if (frmcol[itemnameval] != null)
                            {
                                model.SalesBillItemDetails.ItemName = frmcol[itemnameval];
                            }
                            else
                            {
                                model.SalesBillItemDetails.ItemName = frmcol[itemname];
                            }
                        }
                        model.SalesBillItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            model.SalesBillItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                        }
                        model.SalesBillItemDetails.Narration = frmcol[narration];
                        model.SalesBillItemDetails.Description = frmcol[description];
                        model.SalesBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesBillItemDetails.Unit = frmcol[unit];
                        model.SalesBillItemDetails.NumberType = frmcol[numbertype];
                        model.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                        if (frmcol[mrp] != "")
                        {
                            model.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            model.SalesBillItemDetails.MRP = 0;
                        }
                        model.SalesBillItemDetails.Size = frmcol[size];
                        model.SalesBillItemDetails.Brand = frmcol[brand];
                        model.SalesBillItemDetails.DesignCode = frmcol[designcode];
                        model.SalesBillItemDetails.DesignName = frmcol[designnname];
                        model.SalesBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        model.SalesBillItemDetails.Color = frmcol[color];
                        model.SalesBillItemDetails.Material = frmcol[material];
                        model.SalesBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        model.SalesBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        model.SalesBillItemDetails.ItemTax = frmcol[itemtax];
                        model.SalesBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        model.SalesBillItemDetails.ProportionateTax = frmcol[proptax];
                        model.SalesBillItemDetails.ProportionateTaxAmt = frmcol[proptaxamt];
                        model.SalesBillItemDetails.Status = "Active";
                        model.SalesBillItemDetails.ModifiedOn = DateTime.Now;
                        model.SalesBillItemDetails.SalesReturn = "No";
                        _SalesBillItemService.Create(model.SalesBillItemDetails);

                        //UPDATE QUOTATION, SALES ORDER, DELIVERY CHALLAN QUANTITY WHEN IT IS PROCESSED IN RETAIL BILL..
                        if (frmcol[billno] != null)
                        {
                            if (frmcol[billno].Contains("Qu"))
                            {
                                var quotdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frmcol[itemcode], frmcol[billno]);
                                quotdata.Balance = quotdata.Balance - (model.SalesBillItemDetails.Quantity);

                                //if we send all quantity then inactive that item
                                if (quotdata.Balance <= 0)
                                {
                                    quotdata.Status = "InActive";
                                    quotdata.Balance = 0;
                                }
                                _QuotationItemService.Update(quotdata);

                                var QuotDetails = _QuotationService.GetDetailsByQuotNo(frmcol[billno]);
                                var QuotActiveItemsList = _QuotationItemService.GetAllActiveItemsByQuotNo(frmcol[billno]);
                                if (QuotActiveItemsList.Count() == 0)
                                {
                                    QuotDetails.Status = "InActive";
                                    _QuotationService.Update(QuotDetails);
                                }
                            }
                            else if (frmcol[billno].Contains("SO"))
                            {
                                var sodata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(frmcol[itemcode], frmcol[billno]);
                                sodata.SOBalance = sodata.SOBalance - (model.SalesBillItemDetails.Quantity);

                                //if we send all quantity then inactive that item
                                if (sodata.SOBalance <= 0)
                                {
                                    sodata.Status = "InActive";
                                    sodata.SOBalance = 0;
                                }
                                _SalesOrderItemService.Update(sodata);

                                var SODetails = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol[billno]);
                                var SOItemActiveList = _SalesOrderItemService.GetAllActiveItemsByOrderNo(frmcol[billno]);
                                if (SOItemActiveList.Count() == 0)
                                {
                                    SODetails.Status = "InActive";
                                    _SalesOrderService.Update(SODetails);
                                }
                            }
                            else if (frmcol[billno].Contains("DC"))
                            {
                                var dcdata = _DeliveryChallanItemService.GetDetailsByItemCodeAndChallanNo(frmcol[itemcode], frmcol[billno]);
                                dcdata.DelBalance = dcdata.DelBalance - (model.SalesBillItemDetails.Quantity);

                                //if we send all quantity then inactive that item
                                if (dcdata.DelBalance <= 0)
                                {
                                    dcdata.Status = "InActive";
                                    dcdata.DelBalance = 0;
                                }
                                _DeliveryChallanItemService.Update(dcdata);

                                var DCDetails = _DeliveryChallanService.GetDetailsByChallanNo(frmcol[billno]);
                                var DCItemActiveList = _DeliveryChallanItemService.GetAllActiveItemsByChallanNo(frmcol[billno]);
                                if (DCItemActiveList.Count() == 0)
                                {
                                    DCDetails.Status = "InActive";
                                    _DeliveryChallanService.Update(DCDetails);
                                }
                            }
                        }

                        if (frmcol[itemtype] == "Inventory")
                        {
                            //IF NEW INVENTORY/NON-INVENTORY ITEM IS ADD IN RETAIL BILL
                            if (frmcol[billno] == null)
                            {
                                //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - SALES BILL QUANTITY)
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.SalesBillItemDetails.ItemCode, model.SalesBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - model.SalesBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.SalesBillItemDetails.ItemCode, model.SalesBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.SalesBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.SalesBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }

                            //ONLY QUOTATION AND SALES ORDER ITEMS WILL BE MINUS INTO THE STOCK
                            else if (frmcol[billno].Contains("Qu") || frmcol[billno].Contains("SO"))
                            {
                                //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - SALES BILL QUANTITY)
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.SalesBillItemDetails.ItemCode, model.SalesBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - model.SalesBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.SalesBillItemDetails.ItemCode, model.SalesBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.SalesBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.SalesBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.SalesBillItemDetails.Quantity);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }
                        }
                    }
                }
            }

            TempData["SalesBillClientName"] = model.SalesBillDetails.ClientName;
            TempData["SalesBill"] = model.SalesBillDetails.SalesBillNo;

            //REMOVE PREVIOUS INVENTORY TAXES
            var invtaxdetails = _InventoryTaxService.GetTaxesByCode(model.SalesBillDetails.SalesBillNo);
            foreach (var data in invtaxdetails)
            {
                _InventoryTaxService.Delete(data);
            }

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = model.SalesBillDetails.SalesBillNo;
                    model.InventoryTaxDetails.Amount = frmcol[amountontax];
                    model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }

            //get sales bill after reset ID
            int SBAfterResetRow = _SalesBillService.GetDetailsBySalesBillNo(model.SalesBillDetails.SalesBillNo).Id;
            var SalesId = Encode(SBAfterResetRow.ToString());
            TempData["LastSalesBillId"] = SBAfterResetRow;

            if (salesbilltype == "MultipleBill")
            {
                if (model.SalesBillDetails.Balance == 0)
                {
                    return RedirectToAction("SalesBillCreateDetails/" + SalesId);
                }
                else
                {
                    TempData["BillPayment"] = model.SalesBillDetails.Balance;
                    return RedirectToAction("AdjustAmountBackViewForSalesBill", "ResetBills");
                }
            }
            else
            {
                return RedirectToAction("SalesBillCreateDetails/" + SalesId);
            }
        }

        //BACK PAGE OF ADJUST AMOUNT POP UP
        [HttpGet]
        public ActionResult AdjustAmountBackViewForSalesBill()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //ADJUST AMOUNT YES/NO CONDITION POP UP
        [HttpGet]
        public ActionResult AdjustAmountPopupForSalesBill()
        {
            return View();
        }

        //BILLS DETAILS FOR ADJUST AMOUNT
        [HttpGet]
        public ActionResult AdjustAmountBillsForSalesBill()
        {
            MainApplication model = new MainApplication();
            string clientname = TempData["SalesBillClientName"].ToString();
            TempData["SalesBillClientName"] = clientname;
            model.SalesOrderList = _SalesOrderService.GetDetailsByClientAndBalance(clientname);
            return View(model);
        }

        [HttpPost]
        public ActionResult AdjustAmountBillsForSalesBill(FormCollection frmcol)
        {
            string salesbill = TempData["SalesBill"].ToString();

            string MultipleSO = string.Empty;
            int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);
            for (int i = 1; i <= rowcount; i++)
            {
                string billnumberval = "BillNoVal" + i;
                string adjustamount = "AdjustAmount" + i;
                string balance = "BalanceVal" + i;

                if (frmcol[adjustamount] != "" && Convert.ToDouble(frmcol[adjustamount]) != 0)
                {
                    //MINUS ADJUSTED AMOUNT INTO SO ADVANCE PAYMENT
                    var SOData = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol[billnumberval]);
                    SOData.RemainingAdvance = Convert.ToDouble(frmcol[balance]);
                    if (SOData.AdjustedAmount != 0)
                    {
                        SOData.AdjustedAmount = SOData.AdjustedAmount + Convert.ToDouble(frmcol[adjustamount]);
                    }
                    else
                    {
                        SOData.AdjustedAmount = Convert.ToDouble(frmcol[adjustamount]);
                    }
                    if (SOData.AdjustedForBill == null)
                    {
                        SOData.AdjustedForBill = salesbill;
                    }
                    else
                    {
                        SOData.AdjustedForBill = SOData.AdjustedForBill + "," + salesbill;
                    }
                    if (SOData.RemainingAdvance == 0)
                    {
                        SOData.Status = "InActive";
                        //WHEN SALES ORDER GETS CLOSED THEN INACTIVE ALL THE ITEMS OF PARTICULAR SALES ORDER
                        var SoItemList = _SalesOrderItemService.GetDetailsByOrderNo(SOData.OrderNo);
                        {
                            foreach (var item in SoItemList)
                            {
                                if (item.Status == "Active")
                                {
                                    item.Status = "InActive";
                                    _SalesOrderItemService.Update(item);
                                }
                            }
                        }
                    }
                    _SalesOrderService.Update(SOData);

                    //SAVE ADJUSTED SALESORDER IN SALES BILL
                    if (SOData.AdjustedAmount != 0)
                    {
                        if (MultipleSO == "")
                        {
                            MultipleSO = frmcol[billnumberval];
                        }
                        else
                        {
                            MultipleSO = MultipleSO + "," + frmcol[billnumberval];
                        }
                    }
                }
            }

            //ADJUSTED AMOUNT SAVE IN SALES BILL & ADJUSTED AMOUNT IS MINUS INTO TO SALES ORDER BALANCE
            var salesbilldata = _SalesBillService.GetDetailsBySalesBillNo(salesbill);
            salesbilldata.AdjustedBill = MultipleSO;
            if (frmcol["TotalAdjustAmount"] != "")
            {
                salesbilldata.AdjustedAmount = Convert.ToDouble(frmcol["TotalAdjustAmount"]);
            }
            else
            {
                salesbilldata.AdjustedAmount = 0;
            }
            //if bal < 0 then calculate refund
            var bal = salesbilldata.Balance - Convert.ToDouble(frmcol["TotalAdjustAmount"]);
            if (bal <= 0)
            {
                //save refund and 0 balance in sales bill
                var refund = Convert.ToDouble(frmcol["TotalAdjustAmount"]) - salesbilldata.Balance;
                salesbilldata.Refund = refund;
                //if refund is 0 then inactive cashier status of that salesbill..
                if (salesbilldata.Refund == 0)
                {
                    salesbilldata.CashierStatus = "InActive";
                }

                salesbilldata.Balance = 0;

                //if balance and refund is 0 then inactive status of that salesbill..
                salesbilldata.Status = "InActive";
            }
            else
            {
                salesbilldata.Balance = bal;
            }
            salesbilldata.AdjustedStatus = "Yes";
            _SalesBillService.Update(salesbilldata);

            //SAVE DATA IN SALES BILL ADJUSTED AMOUNT DETAILS TABLE..
            MainApplication model = new MainApplication()
            {
                SalesBillAdjAmtDetails = new SalesBillAdjAmtDetail(),
            };

            for (int i = 1; i <= rowcount; i++)
            {
                string billnumberval = "BillNoVal" + i;
                string adjustamount = "AdjustAmount" + i;
                string balance = "BalanceVal" + i;

                if (frmcol[adjustamount] != "" && Convert.ToDouble(frmcol[adjustamount]) != 0)
                {
                    model.SalesBillAdjAmtDetails.AdjustedBill = frmcol[billnumberval];
                    model.SalesBillAdjAmtDetails.AdjustedAmount = Convert.ToDouble(frmcol[adjustamount]);
                    model.SalesBillAdjAmtDetails.SalesBillNo = salesbill;
                    model.SalesBillAdjAmtDetails.Status = "Active";
                    model.SalesBillAdjAmtDetails.ModifiedOn = DateTime.Now;
                    _SalesBillAdjAmtDetailService.Create(model.SalesBillAdjAmtDetails);
                }
            }
            return RedirectToAction("ClosePopUp", "ResetBills");
        }

        [HttpGet]
        public ActionResult SalesBillCreateDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = new SalesBill();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.SalesBillDetails = _SalesBillService.GetDetailsById(Id);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesBillDetails.SalesBillNo);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(model.SalesBillDetails.SalesBillNo);
            string previoussalesbillno = TempData["PreviousSalesBillNo"].ToString();
            if (TempData["PreviousSalesBillNo"].ToString() != model.SalesBillDetails.SalesBillNo)
            {
                ViewData["SalesBillNoChanged"] = previoussalesbillno + " is replaced with " + model.SalesBillDetails.SalesBillNo + " because " + previoussalesbillno + " is acquired by another person";
            }
            TempData["PreviousSalesBillNo"] = previoussalesbillno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintSalesBill(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillService.GetDetailsById(Id);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(model.SalesBillDetails.SalesBillNo);
            return View(model);
        }

    }
}
