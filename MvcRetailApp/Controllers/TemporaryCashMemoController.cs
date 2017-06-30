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
    public class TemporaryCashMemoController : Controller
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
        private readonly ISalesBillService _isalesbillservice;
        private readonly ISalesBillItemService _isalesbillitemdetailservice;
        private readonly IUtilityService _utilityService;
        private readonly ICompanyService _icompanyservice;
        private readonly ITransportService _TransportService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly ICustomerDetailService _customerdetailservice;
        private readonly ITemporaryCashMemoService _TemporaryCashMemoService;
        private readonly ITemporaryCashMemoItemService _TemporaryCashMemoItemService;
        private readonly IItemService _itemservice;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IShopStockService _ShopStockService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IShopService _ShopService;
        private readonly IQuotationService _QuotationService;
        private readonly IQuotationItemService _QuotationItemService;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly IDeliveryChallanService _DeliveryChallanService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IInventoryTaxService _InventoryTaxService;
        private readonly ICashierReceivableService _CashierReceivableService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierRetailBillItemService _CashierRetailBillItemService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly ITemporaryCashMemoAdjAmtDetailService _TemporaryCashMemoAdjAmtDetailService;
        private readonly ICashierTemporaryCashMemoService _CashierTemporaryCashMemoService;
        private readonly ICashierTemporaryCashMemoItemService _CashierTemporaryCashMemoItemService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;

        public TemporaryCashMemoController(IItemTaxService itemtaxservice, ISalesBillItemService salesbillitemdetailservice, ISalesBillService salesbillservice, IUtilityService utilityservice, ICompanyService companyservice, ICustomerDetailService customerdetailservice, ITemporaryCashMemoService TemporaryCashMemoService, ITemporaryCashMemoItemService TemporaryCashMemoItemService,
        ITransportService TransportService, IClientMasterService ClientMasterService, IEntryStockItemService EntryStockItemService, IItemService itemservice, IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IUserCredentialService usercredentialservice,
            IModuleService iIModuleService, IShopStockService ShopStockService, IGodownStockService GodownStockService, IEmployeeMasterService EmployeeMasterService, IShopService ShopService, IQuotationService QuotationService, IQuotationItemService QuotationItemService, ISalesOrderService SalesOrderService, IDeliveryChallanService DeliveryChallanService, ISalesOrderItemService SalesOrderItemService, IStockItemDistributionService StockItemDistributionService,
            IDeliveryChallanItemService DeliveryChallanItemService, IOpeningStockService OpeningStockService, IInventoryTaxService InventoryTaxService, ICashierReceivableService CashierReceivableService, ICashierRetailBillService CashierRetailBillService, ICashierRetailBillItemService CashierRetailBillItemService, IUserCredentialService UserCredentialService, ITemporaryCashMemoAdjAmtDetailService TemporaryCashMemoAdjAmtDetailService,
            INonInventoryItemService NonInventoryItemService, ICashierTemporaryCashMemoService CashierTemporaryCashMemoService, ICashierTemporaryCashMemoItemService CashierTemporaryCashMemoItemService, IDiscountMasterItemService DiscountMasterItemService)
        {
            this._isalesbillservice = salesbillservice;
            this._utilityService = utilityservice;
            this._icompanyservice = companyservice;
            this._TransportService = TransportService;
            this._ClientMasterService = ClientMasterService;
            this._ItemTaxService = itemtaxservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._customerdetailservice = customerdetailservice;
            this._TemporaryCashMemoService = TemporaryCashMemoService;
            this._TemporaryCashMemoItemService = TemporaryCashMemoItemService;
            this._itemservice = itemservice;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._EntryStockItemService = EntryStockItemService;
            this._isalesbillitemdetailservice = salesbillitemdetailservice;
            this._ShopStockService = ShopStockService;
            this._GodownStockService = GodownStockService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._ShopService = ShopService;
            this._QuotationService = QuotationService;
            this._QuotationItemService = QuotationItemService;
            this._SalesOrderService = SalesOrderService;
            this._SalesOrderItemService = SalesOrderItemService;
            this._DeliveryChallanService = DeliveryChallanService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._OpeningStockService = OpeningStockService;
            this._InventoryTaxService = InventoryTaxService;
            this._CashierReceivableService = CashierReceivableService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierRetailBillItemService = CashierRetailBillItemService;
            this._UserCredentialService = UserCredentialService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._TemporaryCashMemoAdjAmtDetailService = TemporaryCashMemoAdjAmtDetailService;
            this._CashierTemporaryCashMemoService = CashierTemporaryCashMemoService;
            this._CashierTemporaryCashMemoItemService = CashierTemporaryCashMemoItemService;
            this._DiscountMasterItemService = DiscountMasterItemService;
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

        // CREATE RETAIL BILL
        [HttpGet]
        public ActionResult TemporaryCashMemo()
        {
            MainApplication model = new MainApplication()
            {
                TemporaryCashMemoDetails = new TemporaryCashMemo(),
            };

            //GET LOGIN SHOP DETAILS
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
            var LastTempCashMemo = _TemporaryCashMemoService.GetLastTempCashMemoByFinYr(FinYr, shopcode);
            string InvoiceCode = string.Empty;
            int InvoiceNoLength = 0;
            int InvoiceNo = 0;
            if (LastTempCashMemo == null)
            {
                InvoiceNoLength = 1;
                InvoiceNo = 1;
            }
            else
            {
                int TempCashIndex = LastTempCashMemo.TempCashMemoNo.LastIndexOf('P');
                InvoiceCode = LastTempCashMemo.TempCashMemoNo.Substring(TempCashIndex + 3, 6);
                InvoiceNoLength = (Convert.ToInt32(InvoiceCode) + 1).ToString().Length;
                InvoiceNo = Convert.ToInt32(InvoiceCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            InvoiceCode = _utilityService.getName(ShortCode + "/PCM", InvoiceNoLength, InvoiceNo);
            InvoiceCode = InvoiceCode + FinYr;
            model.TemporaryCashMemoDetails.TempCashMemoNo = InvoiceCode;
            TempData["PreviousRetailno"] = InvoiceCode;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            model.ShopList = _ShopService.GetAll();
            return View(model);
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

        //AUTO COMPLETE ITEM NAME
        [HttpGet]
        public JsonResult GetItemCodeName(string term)
        {
            var employeedata = _EmployeeMasterService.GetEmpBySalesDesignation(term);
            List<string> names = new List<string>();
            foreach (var item in employeedata)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET EMPLOPYEE DETAILS BY EMPLOYEE NAME
        [HttpGet]
        public JsonResult GetEmployeeDetails(string name)
        {
            var data = _EmployeeMasterService.getEmpByName(name);
            return Json(new { data.MobileNo, data.Designation, data.Email, data.Address }, JsonRequestBehavior.AllowGet);
        }

        //GET LOGIN EMPLOPYEE DETAILS BY EMPLOYEE EMAIL
        [HttpGet]
        public JsonResult GetPreparedByEmpDetails(string email)
        {
            var data = _EmployeeMasterService.getEmpByEmail(email);
            return Json(new { data.Name, data.MobileNo }, JsonRequestBehavior.AllowGet);
        }

        //GET SHOP DETAILS BY SHOP CODE
        [HttpGet]
        public JsonResult GetShopDetails(string code)
        {
            var data = _ShopService.GetShopByCode(code);
            return Json(new { data.ShopName, data.ShopAddress, data.ShopEmail, data.EmpName }, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE CLIENT NAME CUSTOMER DETAILS
        [HttpGet]
        public JsonResult GetCustomerNames(string term)
        {
            var customerdata = _customerdetailservice.GetCustomerName(term);
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
            var data = _customerdetailservice.GetDetailsByName(name);
            return Json(new { data.Address, data.EmailId, data.Contact }, JsonRequestBehavior.AllowGet);
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }
                        
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DisInPercent = discount.ToString();
                            item.DisInRs = discountrps.ToString();
                        }
                        
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }
                        
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }
                       
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DisInPercent = discount.ToString();
                            item.DisInRs = discountrps.ToString();
                        }
                        
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }
                        
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }
                       
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DisInPercent = discount.ToString();
                            item.DisInRs = discountrps.ToString();
                        }
                        
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
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }
                        
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
        public JsonResult GetChallanNo(string term)
        {
            var challandata = _DeliveryChallanService.GetActiveChallanNo(term);
            List<string> names = new List<string>();
            foreach (var item in challandata)
            {
                names.Add(item.ChallanNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
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
                    var itemdata = _itemservice.GetDescriptionByItemCode(data.ItemCode);
                    string subcat = itemdata.itemSubCategory;
                    string costprice = itemdata.costprice;
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

        [HttpGet]
        public JsonResult GetItemsCostPrice(string itemcode, string itemtype)
        {
            string costprice = "0";
            if (itemtype == "Inventory")
            {
                costprice = _itemservice.GetDescriptionByItemCode(itemcode).costprice;
            }
            else
            {
                costprice = _NonInventoryItemService.GetDetailsByItemCode(itemcode).costprice;
                if (costprice == "")
                {
                    costprice = "0";
                }
            }
            return Json(costprice, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY ITEM DISTAILS BY NONINVENTORY ITEM CODE
        [HttpGet]
        public JsonResult GetNonInvItemdetailsByItemCode(string ItemCode)
        {
            double? discount = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
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
                return Json(new
                {
                    discount,
                    itemdetails.itemName,
                    itemdetails.itemCode,
                    itemdetails.description,
                    itemdetails.sellingprice,
                    itemdetails.mrp,
                    itemdetails.costprice,
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

        [HttpGet]
        public JsonResult DeliveryChallanMade()
        {
            string msg = "Present";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaxFromOrderOrChallan(string orderno)
        {
            var list = _InventoryTaxService.GetTaxesByCode(orderno);
            var taxlist = list.Select(m => new InventoryTax
            {
                Tax = m.Tax,
                Amount = m.Amount,
            });
            return Json(taxlist, JsonRequestBehavior.AllowGet);
        }

        // SAVE TEMPORARY CASH MEMO
        [HttpPost]
        public ActionResult TemporaryCashMemo(MainApplication main, FormCollection frmcol)
        {

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            // SAVE TEMPORARY CASH MEMO BILL MASTER
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastTempCashMemo = _TemporaryCashMemoService.GetLastTempCashMemoByFinYr(FinYr, shopcode);
            string InvoiceCode = string.Empty;
            int InvoiceNoLength = 0;
            int InvoiceNo = 0;
            if (LastTempCashMemo == null)
            {
                InvoiceNoLength = 1;
                InvoiceNo = 1;
            }
            else
            {
                int TempCashIndex = LastTempCashMemo.TempCashMemoNo.LastIndexOf('P');
                InvoiceCode = LastTempCashMemo.TempCashMemoNo.Substring(TempCashIndex + 3, 6);
                InvoiceNoLength = (Convert.ToInt32(InvoiceCode) + 1).ToString().Length;
                InvoiceNo = Convert.ToInt32(InvoiceCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            InvoiceCode = _utilityService.getName(ShortCode + "/PCM", InvoiceNoLength, InvoiceNo);
            InvoiceCode = InvoiceCode + FinYr;


            main.TemporaryCashMemoDetails.Date = DateTime.Now;
            main.TemporaryCashMemoDetails.TempCashMemoNo = InvoiceCode;
            main.TemporaryCashMemoDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            main.TemporaryCashMemoDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
            main.TemporaryCashMemoDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
            main.TemporaryCashMemoDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
            main.TemporaryCashMemoDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
            main.TemporaryCashMemoDetails.Status = "Active";
            main.TemporaryCashMemoDetails.ModifiedOn = DateTime.Now;
            main.TemporaryCashMemoDetails.CashierStatus = "Active";
            main.TemporaryCashMemoDetails.SalesReturn = "No";
            main.TemporaryCashMemoDetails.AdjustedAmount = 0;
            main.TemporaryCashMemoDetails.BillBalance = "None";
            main.TemporaryCashMemoDetails.Convertstatus = "Active";
            main.TemporaryCashMemoDetails.ConvertDateStatus = "NotConverted";

            //IF PAYMENT IS NULL THEN SAVE PAYMENT IS 0 IN RETAIL BILL
            if (main.TemporaryCashMemoDetails.Payment == null)
            {
                main.TemporaryCashMemoDetails.Payment = 0;
            }

            //IF BALNCE IS 0 THEN WE CHECK THE REFUND FOR SAVE
            if (main.TemporaryCashMemoDetails.Balance == 0)
            {
                main.TemporaryCashMemoDetails.Refund = main.TemporaryCashMemoDetails.Payment - main.TemporaryCashMemoDetails.GrandTotal;
            }
            else
            {
                main.TemporaryCashMemoDetails.Refund = 0;
            }
            _TemporaryCashMemoService.Create(main.TemporaryCashMemoDetails);

            // SAVE CUSTOMER DETAILS
            if (main.TemporaryCashMemoDetails.ClientName != null)
            {
                main.CustomerDetails = new CustomerDetail();

                var customerdata = _customerdetailservice.GetDetailsByName(main.TemporaryCashMemoDetails.ClientName);
                if (customerdata == null)
                {
                    main.CustomerDetails.Name = main.TemporaryCashMemoDetails.ClientName;
                    main.CustomerDetails.Address = main.TemporaryCashMemoDetails.ClientAddress;
                    main.CustomerDetails.Contact = main.TemporaryCashMemoDetails.ClientContact;
                    main.CustomerDetails.EmailId = main.TemporaryCashMemoDetails.ClientEmail;
                    main.CustomerDetails.Status = "Active";
                    main.CustomerDetails.ModifiedOn = DateTime.Now;
                    _customerdetailservice.CreateInvoice(main.CustomerDetails);
                }
                else
                {
                    customerdata.Address = main.TemporaryCashMemoDetails.ClientAddress;
                    customerdata.Contact = main.TemporaryCashMemoDetails.ClientContact;
                    customerdata.EmailId = main.TemporaryCashMemoDetails.ClientEmail;
                    customerdata.ModifiedOn = DateTime.Now;
                    _customerdetailservice.UpdateInvoice(customerdata);
                }
            }

            var tempcashmemotype = "";

            // SAVE DIRECT RETAIL BILL ITEMS
            if (main.TemporaryCashMemoDetails.QuotationNo == null && main.TemporaryCashMemoDetails.SalesOrderNo == null && main.TemporaryCashMemoDetails.DeliveryChallanNo == null)
            {
                tempcashmemotype = "Direct";
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
                        main.TemporaryCashMemoItemDetails = new TemporaryCashMemoItem();
                        main.TemporaryCashMemoItemDetails.TempCashMemolNo = main.TemporaryCashMemoDetails.TempCashMemoNo;
                        main.TemporaryCashMemoItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.TemporaryCashMemoItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            main.TemporaryCashMemoItemDetails.ItemName = frmcol[itemnameval];
                        }
                        main.TemporaryCashMemoItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.TemporaryCashMemoItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                        }
                        main.TemporaryCashMemoItemDetails.Narration = frmcol[narration];
                        main.TemporaryCashMemoItemDetails.Description = frmcol[description];
                        main.TemporaryCashMemoItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        main.TemporaryCashMemoItemDetails.Unit = frmcol[unit];
                        main.TemporaryCashMemoItemDetails.NumberType = frmcol[numbertype];
                        main.TemporaryCashMemoItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        main.TemporaryCashMemoItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        main.TemporaryCashMemoItemDetails.Size = frmcol[size];
                        main.TemporaryCashMemoItemDetails.Brand = frmcol[brand];
                        main.TemporaryCashMemoItemDetails.DesignCode = frmcol[designcode];
                        main.TemporaryCashMemoItemDetails.DesignName = frmcol[designnname];
                        main.TemporaryCashMemoItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        main.TemporaryCashMemoItemDetails.Color = frmcol[color];
                        main.TemporaryCashMemoItemDetails.Material = frmcol[material];
                        if (frmcol[disper] != null)
                        {
                            main.TemporaryCashMemoItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        }
                        if (frmcol[disrs] != null)
                        {
                            main.TemporaryCashMemoItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        }
                        main.TemporaryCashMemoItemDetails.ItemTax = frmcol[itemtax];
                        main.TemporaryCashMemoItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        main.TemporaryCashMemoItemDetails.Status = "Active";
                        main.TemporaryCashMemoItemDetails.ModifiedOn = DateTime.Now;
                        main.TemporaryCashMemoItemDetails.SalesReturn = "No";
                        _TemporaryCashMemoItemService.Create(main.TemporaryCashMemoItemDetails);
                    }
                }
            }
            // SAVE TEMPORARY CASH MEMO ITEMS USING MULTIPLE BILLS
            else
            {
                tempcashmemotype = "MultipleBill";
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
                        main.TemporaryCashMemoItemDetails = new TemporaryCashMemoItem();
                        main.TemporaryCashMemoItemDetails.TempCashMemolNo = main.TemporaryCashMemoDetails.TempCashMemoNo;
                        main.TemporaryCashMemoItemDetails.BillNo = frmcol[billno];
                        main.TemporaryCashMemoItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.TemporaryCashMemoItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            main.TemporaryCashMemoItemDetails.ItemName = frmcol[itemnameval];
                        }
                        main.TemporaryCashMemoItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.TemporaryCashMemoItemDetails.Barcode = frmcol[barcode] + ".png";
                        }
                        main.TemporaryCashMemoItemDetails.Narration = frmcol[narration];
                        main.TemporaryCashMemoItemDetails.Description = frmcol[description];
                        main.TemporaryCashMemoItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        main.TemporaryCashMemoItemDetails.Unit = frmcol[unit];
                        main.TemporaryCashMemoItemDetails.NumberType = frmcol[numbertype];
                        main.TemporaryCashMemoItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        main.TemporaryCashMemoItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        main.TemporaryCashMemoItemDetails.Size = frmcol[size];
                        main.TemporaryCashMemoItemDetails.Brand = frmcol[brand];
                        main.TemporaryCashMemoItemDetails.DesignCode = frmcol[designcode];
                        main.TemporaryCashMemoItemDetails.DesignName = frmcol[designnname];
                        main.TemporaryCashMemoItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        main.TemporaryCashMemoItemDetails.Color = frmcol[color];
                        main.TemporaryCashMemoItemDetails.Material = frmcol[material];
                        main.TemporaryCashMemoItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        main.TemporaryCashMemoItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        main.TemporaryCashMemoItemDetails.ItemTax = frmcol[itemtax];
                        main.TemporaryCashMemoItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        main.TemporaryCashMemoItemDetails.Status = "Active";
                        main.TemporaryCashMemoItemDetails.ModifiedOn = DateTime.Now;
                        main.TemporaryCashMemoItemDetails.SalesReturn = "No";
                        _TemporaryCashMemoItemService.Create(main.TemporaryCashMemoItemDetails);

                        //UPDATE QUOTATION, SALES ORDER, DELIVERY CHALLAN QUANTITY WHEN IT IS PROCESSED IN RETAIL BILL..
                        if (frmcol[billno] != null)
                        {
                            if (frmcol[billno].Contains("Qu"))
                            {
                                var quotdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frmcol[itemcode], frmcol[billno]);
                                quotdata.Balance = quotdata.Balance - (main.TemporaryCashMemoItemDetails.Quantity);

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
                                sodata.SOBalance = sodata.SOBalance - (main.TemporaryCashMemoItemDetails.Quantity);

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
                                dcdata.DelBalance = dcdata.DelBalance - (main.TemporaryCashMemoItemDetails.Quantity);

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

                    }
                }
            }

            // IF WE FILL PAYMENT DERAILS THEN SAVE PAYMENT DETAILS IN CASHIER RECEIVABLES AND CASHIER RETAIL BILL..
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
                string cashiercode = _utilityService.getName("CHR", newlength, CashierVal);

                // USE SAME MAIN APP OBJECT IN VIEW
                main.CashierTemporaryCashMemoDetails = new CashierTemporaryCashMemo();
                main.CashierTemporaryCashMemoItemDetails = new CashierTemporaryCashMemoItem();

                // SAVE CASHIER RECEIVABLE 
                main.CashierReceivableDetails.CashierCode = cashiercode;
                main.CashierReceivableDetails.Date = DateTime.Now;
                main.CashierReceivableDetails.BillType = "TemporaryCashMemo";
                main.CashierReceivableDetails.Billno = main.TemporaryCashMemoDetails.TempCashMemoNo;
                main.CashierReceivableDetails.ClientName = main.TemporaryCashMemoDetails.ClientName;
                main.CashierReceivableDetails.ClientContact = main.TemporaryCashMemoDetails.ClientContact;
                main.CashierReceivableDetails.ClientState = frmcol["ClientState"];
                main.CashierReceivableDetails.ClientType = frmcol["ClientType"];
                main.CashierReceivableDetails.ClientFormType = frmcol["FormType"];

                main.CashierReceivableDetails.PackAndForwd = 0;
                main.CashierReceivableDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                main.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                main.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(main.TemporaryCashMemoDetails.Payment);
                main.CashierReceivableDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                main.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                main.CashierReceivableDetails.PaymentType = frmcol["CashierPaymentType"];

                main.CashierReceivableDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                main.CashierReceivableDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                main.CashierReceivableDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                main.CashierReceivableDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                main.CashierReceivableDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                main.CashierReceivableDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                main.CashierReceivableDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                main.CashierReceivableDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                main.CashierReceivableDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                main.CashierReceivableDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                main.CashierReceivableDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                main.CashierReceivableDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                main.CashierReceivableDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                main.CashierReceivableDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                main.CashierReceivableDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                main.CashierReceivableDetails.SelectedCard = frmcol["Card"];
                main.CashierReceivableDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    main.CashierReceivableDetails.CreditCardAmount = 0;
                    main.CashierReceivableDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    main.CashierReceivableDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    main.CashierReceivableDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                main.CashierReceivableDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                main.CashierReceivableDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                main.CashierReceivableDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                main.CashierReceivableDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                main.CashierReceivableDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                main.CashierReceivableDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    main.CashierReceivableDetails.DebitCardAmount = 0;
                    main.CashierReceivableDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    main.CashierReceivableDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    main.CashierReceivableDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                main.CashierReceivableDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                main.CashierReceivableDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                {
                    main.CashierReceivableDetails.ChequeAmount = 0;
                    main.CashierReceivableDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    main.CashierReceivableDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    main.CashierReceivableDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                }
                if (main.CashierReceivableDetails.ChequeNo != null && main.CashierReceivableDetails.ChequeNo != "")
                {
                    main.CashierReceivableDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    main.CashierReceivableDetails.ChequeDate = null;
                }
                main.CashierReceivableDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];

                main.CashierReceivableDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                main.CashierReceivableDetails.Status = "Active";
                main.CashierReceivableDetails.ModifiedOn = DateTime.Now;
                _CashierReceivableService.Create(main.CashierReceivableDetails);

                // SAVE CASHIER RETAIL BILL
                main.CashierTemporaryCashMemoDetails.CashierCode = cashiercode;
                main.CashierTemporaryCashMemoDetails.Date = DateTime.Now;
                main.CashierTemporaryCashMemoDetails.TempCashMemoDate = DateTime.Now;
                main.CashierTemporaryCashMemoDetails.TempCashMemoNo = main.TemporaryCashMemoDetails.TempCashMemoNo;
                //main.CashierTemporaryCashMemoDetails.BillDateStatus = "Same";
                main.CashierTemporaryCashMemoDetails.ClientName = main.TemporaryCashMemoDetails.ClientName;
                main.CashierTemporaryCashMemoDetails.ClientContact = main.TemporaryCashMemoDetails.ClientContact;
                main.CashierTemporaryCashMemoDetails.ClientState = frmcol["ClientState"];
                main.CashierTemporaryCashMemoDetails.ClientType = frmcol["ClientType"];
                main.CashierTemporaryCashMemoDetails.FormType = frmcol["FormType"];
                main.CashierTemporaryCashMemoDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                main.CashierTemporaryCashMemoDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                main.CashierTemporaryCashMemoDetails.Payment = Convert.ToDouble(main.TemporaryCashMemoDetails.Payment);
                main.CashierTemporaryCashMemoDetails.TotalAdvancePayment = Convert.ToDouble(main.TemporaryCashMemoDetails.Payment);
                main.CashierTemporaryCashMemoDetails.SavingFrom = "TemporaryCashMemo";
                main.CashierTemporaryCashMemoDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                main.CashierTemporaryCashMemoDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                main.CashierTemporaryCashMemoDetails.RefundAmount = main.TemporaryCashMemoDetails.Refund;
                main.CashierTemporaryCashMemoDetails.AdjustedAmount = 0;
                main.CashierTemporaryCashMemoDetails.PaymentType = frmcol["CashierPaymentType"];
                main.CashierTemporaryCashMemoDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                main.CashierTemporaryCashMemoDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                main.CashierTemporaryCashMemoDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                main.CashierTemporaryCashMemoDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                main.CashierTemporaryCashMemoDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                main.CashierTemporaryCashMemoDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                main.CashierTemporaryCashMemoDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                main.CashierTemporaryCashMemoDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                main.CashierTemporaryCashMemoDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                main.CashierTemporaryCashMemoDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                main.CashierTemporaryCashMemoDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                main.CashierTemporaryCashMemoDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                main.CashierTemporaryCashMemoDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                main.CashierTemporaryCashMemoDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                main.CashierTemporaryCashMemoDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                main.CashierTemporaryCashMemoDetails.Refund = main.CashierReceivableDetails.Refund;
                main.CashierTemporaryCashMemoDetails.SelectedCard = frmcol["Card"];

                main.CashierTemporaryCashMemoDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    main.CashierTemporaryCashMemoDetails.CreditCardAmount = 0;
                    main.CashierTemporaryCashMemoDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    main.CashierTemporaryCashMemoDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    main.CashierTemporaryCashMemoDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                main.CashierTemporaryCashMemoDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                main.CashierTemporaryCashMemoDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                main.CashierTemporaryCashMemoDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                main.CashierTemporaryCashMemoDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                main.CashierTemporaryCashMemoDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                main.CashierTemporaryCashMemoDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    main.CashierTemporaryCashMemoDetails.DebitCardAmount = 0;
                    main.CashierTemporaryCashMemoDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    main.CashierTemporaryCashMemoDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    main.CashierTemporaryCashMemoDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                main.CashierTemporaryCashMemoDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                main.CashierTemporaryCashMemoDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                {
                    main.CashierTemporaryCashMemoDetails.ChequeAmount = 0;
                    main.CashierTemporaryCashMemoDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    main.CashierTemporaryCashMemoDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    main.CashierTemporaryCashMemoDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                }
                if (main.CashierTemporaryCashMemoDetails.ChequeNo != null && main.CashierReceivableDetails.ChequeNo != "")
                {
                    main.CashierTemporaryCashMemoDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    main.CashierTemporaryCashMemoDetails.ChequeDate = null;
                }
                main.CashierTemporaryCashMemoDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];

                main.CashierTemporaryCashMemoDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                main.CashierTemporaryCashMemoDetails.Status = "Active";
                main.CashierTemporaryCashMemoDetails.ModifiedOn = DateTime.Now;

                //SAVE HANDOVER STATUS
                if (main.CashierTemporaryCashMemoDetails.CreditCardAmount == 0 && main.CashierTemporaryCashMemoDetails.DebitCardAmount == 0 && main.CashierTemporaryCashMemoDetails.ChequeAmount == 0)
                {
                    main.CashierTemporaryCashMemoDetails.HandoverStatus = "InActive";
                }
                else
                {
                    main.CashierTemporaryCashMemoDetails.HandoverStatus = "Active";
                }
                _CashierTemporaryCashMemoService.Create(main.CashierTemporaryCashMemoDetails);

                // SAVE CASHIER TEMPORARY CASH MEMO ITEM
                main.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNo(main.TemporaryCashMemoDetails.TempCashMemoNo);
                foreach (var item in main.TemporaryCashMemoItemList)
                {
                    main.CashierTemporaryCashMemoItemDetails.CashierCode = cashiercode;
                    main.CashierTemporaryCashMemoItemDetails.TempCashMemoNo = main.TemporaryCashMemoDetails.TempCashMemoNo;
                    main.CashierTemporaryCashMemoItemDetails.ItemName = item.ItemName;
                    main.CashierTemporaryCashMemoItemDetails.ItemCode = item.ItemCode;
                    main.CashierTemporaryCashMemoItemDetails.Description = item.Description;
                    main.CashierTemporaryCashMemoItemDetails.Material = item.Material;
                    main.CashierTemporaryCashMemoItemDetails.Color = item.Color;
                    main.CashierTemporaryCashMemoItemDetails.SellingPrice = item.SellingPrice;
                    main.CashierTemporaryCashMemoItemDetails.MRP = item.MRP;
                    main.CashierTemporaryCashMemoItemDetails.Quantity = item.Quantity;
                    main.CashierTemporaryCashMemoItemDetails.Amount = item.Amount;
                    main.CashierTemporaryCashMemoItemDetails.DesignCode = item.DesignCode;
                    main.CashierTemporaryCashMemoItemDetails.DesignName = item.DesignName;
                    main.CashierTemporaryCashMemoItemDetails.Size = item.Size;
                    main.CashierTemporaryCashMemoItemDetails.Brand = item.Brand;
                    main.CashierTemporaryCashMemoItemDetails.Barcode = item.Barcode;
                    main.CashierTemporaryCashMemoItemDetails.Unit = item.Unit;
                    main.CashierTemporaryCashMemoItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                    main.CashierTemporaryCashMemoItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                    main.CashierTemporaryCashMemoItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                    main.CashierTemporaryCashMemoItemDetails.Status = "Active";
                    main.CashierTemporaryCashMemoItemDetails.ModifiedOn = DateTime.Now;
                    _CashierTemporaryCashMemoItemService.Create(main.CashierTemporaryCashMemoItemDetails);
                }

                var TempCashMemoData = _TemporaryCashMemoService.GetDetailsByInvoiceNo(main.TemporaryCashMemoDetails.TempCashMemoNo);

                //if bill balance is 0 and cashier details also created then inactive that RETAIL BILL..
                double tempcashmemobal = Convert.ToDouble(main.TemporaryCashMemoDetails.Balance);
                double tempcashmemobillref = Convert.ToDouble(main.TemporaryCashMemoDetails.Refund);
                if (tempcashmemobal == 0 && tempcashmemobillref == 0)
                {
                    TempCashMemoData.Status = "InActive";
                    TempCashMemoData.CashierStatus = "InActive";
                    _TemporaryCashMemoService.Update(TempCashMemoData);
                }
                //if we save the cashier payment details in retail bill then cashier status is ProcessInStatus for that bill..
                else if (tempcashmemobal == 0)
                {
                    TempCashMemoData.Status = "InActive";
                    _TemporaryCashMemoService.Update(TempCashMemoData);
                }
            }

            TempData["ClientName"] = main.TemporaryCashMemoDetails.ClientName;
            TempData["TemporaryCashMemo"] = main.TemporaryCashMemoDetails.TempCashMemoNo;

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ItemTaxCount"]);
            main.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    main.InventoryTaxDetails.Code = main.TemporaryCashMemoDetails.TempCashMemoNo;
                    main.InventoryTaxDetails.Amount = frmcol[amountontax];
                    main.InventoryTaxDetails.Tax = frmcol[taxnumber];
                    main.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _InventoryTaxService.Create(main.InventoryTaxDetails);
                }
            }

            int lastRow = _TemporaryCashMemoService.GetLastInvoiceDetails().Id;
            var TempCashMemoId = Encode(lastRow.ToString());
            TempData["LastRetailBillId"] = lastRow;

            if (tempcashmemotype == "MultipleBill")
            {
                if (main.TemporaryCashMemoDetails.Balance == 0)
                {
                    return RedirectToAction("TemporaryCashMemoDetails/" + TempCashMemoId);
                }
                else
                {
                    TempData["BillPayment"] = main.TemporaryCashMemoDetails.Balance;
                    return RedirectToAction("AdjustAmountBackView", "TemporaryCashMemo");
                }
            }
            else
            {
                return RedirectToAction("TemporaryCashMemoDetails/" + TempCashMemoId);
            }
        }

        //BACK PAGE OF ADJUST AMOUNT POP UP
        [HttpGet]
        public ActionResult AdjustAmountBackView()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
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
            string TempCashMemo = TempData["TemporaryCashMemo"].ToString();

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
                        SOData.AdjustedForBill = TempCashMemo;
                    }
                    else
                    {
                        SOData.AdjustedForBill = SOData.AdjustedForBill + "," + TempCashMemo;
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
            var tempcashmemodata = _TemporaryCashMemoService.GetDetailsByInvoiceNo(TempCashMemo);
            tempcashmemodata.AdjustedBill = MultipleSO;
            tempcashmemodata.AdjustedAmount = Convert.ToDouble(frmcol["TotalAdjustAmount"]);

            //if bal < 0 then calculate refund
            var bal = tempcashmemodata.Balance - Convert.ToDouble(frmcol["TotalAdjustAmount"]);
            if (bal <= 0)
            {
                //save refund in retail bill
                var refund = Convert.ToDouble(frmcol["TotalAdjustAmount"]) - tempcashmemodata.Balance;
                tempcashmemodata.Refund = refund;
                //if refund is 0 then inactive cashier status of that salesbill..
                if (tempcashmemodata.Refund == 0)
                {
                    tempcashmemodata.CashierStatus = "InActive";
                }
                tempcashmemodata.Balance = 0;
                tempcashmemodata.Status = "InActive";
            }
            else
            {
                tempcashmemodata.Balance = bal;
            }
            _TemporaryCashMemoService.Update(tempcashmemodata);

            //IN CASHIER totaladvancepayment = payment + adjust amount;
            var cashiertempcashmemodata = _CashierTemporaryCashMemoService.GetDetailsByTempCashMemoNo(TempCashMemo);
            if (cashiertempcashmemodata != null)
            {
                double? payment = tempcashmemodata.Payment + tempcashmemodata.AdjustedAmount;
                cashiertempcashmemodata.TotalAdvancePayment = payment;
                cashiertempcashmemodata.AdjustedBill = MultipleSO;
                cashiertempcashmemodata.AdjustedAmount = Convert.ToDouble(frmcol["TotalAdjustAmount"]);

                //if bal < 0 then calculate refund
                var cashbal = cashiertempcashmemodata.Balance - Convert.ToDouble(frmcol["TotalAdjustAmount"]);
                if (cashbal <= 0)
                {
                    //save refund in retail bill
                    var cashrefund = Convert.ToDouble(frmcol["TotalAdjustAmount"]) - cashiertempcashmemodata.Balance;
                    cashiertempcashmemodata.Refund = cashrefund;
                    cashiertempcashmemodata.Balance = 0;
                }
                else
                {
                    cashiertempcashmemodata.Balance = cashbal;
                }
                _CashierTemporaryCashMemoService.Update(cashiertempcashmemodata);
            }

            //SAVE DATA IN TEMPORARY CASH MEMO ADJUSTED AMOUNT DETAILS TABLE..
            MainApplication model = new MainApplication()
            {
                TemporaryCashMemoAdjAmtDetails = new TemporaryCashMemoAdjAmtDetail(),
            };

            for (int i = 1; i <= rowcount; i++)
            {
                string billnumberval = "BillNoVal" + i;
                string adjustamount = "AdjustAmount" + i;
                string balance = "BalanceVal" + i;

                if (Convert.ToDouble(frmcol[adjustamount]) != 0)
                {
                    model.TemporaryCashMemoAdjAmtDetails.AdjustedBill = frmcol[billnumberval];
                    model.TemporaryCashMemoAdjAmtDetails.AdjustedAmount = Convert.ToDouble(frmcol[adjustamount]);
                    model.TemporaryCashMemoAdjAmtDetails.TempCashMemoNo = TempCashMemo;
                    model.TemporaryCashMemoAdjAmtDetails.Status = "Active";
                    model.TemporaryCashMemoAdjAmtDetails.ModifiedOn = DateTime.Now;
                    _TemporaryCashMemoAdjAmtDetailService.Create(model.TemporaryCashMemoAdjAmtDetails);
                }
            }

            return RedirectToAction("ClosePopUp", "TemporaryCashMemo");
        }

        //CLOSE POPUP AFTER ADJUST AMOUNT
        [HttpGet]
        public ActionResult ClosePopUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TemporaryCashMemoDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.TemporaryCashMemoDetails = _TemporaryCashMemoService.GetDetailsById(Id);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.TemporaryCashMemoDetails.TempCashMemoNo);
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNo(model.TemporaryCashMemoDetails.TempCashMemoNo);

            string previousretailno = TempData["PreviousRetailno"].ToString();
            if (TempData["PreviousRetailno"].ToString() != model.TemporaryCashMemoDetails.TempCashMemoNo)
            {
                ViewData["RetailnoChanged"] = previousretailno + " is replaced with " + model.TemporaryCashMemoDetails.TempCashMemoNo + " because " + previousretailno + " is acquired by another person";
            }
            TempData["PreviousRetailno"] = previousretailno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintTemporaryCashMemoWithSellingPrice(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.TemporaryCashMemoDetails = _TemporaryCashMemoService.GetDetailsById(Id);
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNo(model.TemporaryCashMemoDetails.TempCashMemoNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintTemporaryCashMemoWithMRP(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.TemporaryCashMemoDetails = _TemporaryCashMemoService.GetDetailsById(Id);
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNo(model.TemporaryCashMemoDetails.TempCashMemoNo);
            return View(model);
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
    }
}
