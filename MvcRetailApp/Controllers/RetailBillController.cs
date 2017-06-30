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
    public class RetailBillController : Controller
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
        private readonly IRetailBillItemService _retailbillitemservice;
        private readonly IRetailBillService _retailbillservice;
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
        private readonly IRetailBillAdjAmtDetailService _RetailBillAdjAmtDetailService;
        private readonly ITemporaryCashMemoService _TemporaryCashMemoService;
        private readonly ITemporaryCashMemoItemService _TemporaryCashMemoItemService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;

        public RetailBillController(IItemTaxService itemtaxservice, ISalesBillItemService salesbillitemdetailservice, ISalesBillService salesbillservice, IUtilityService utilityservice, ICompanyService companyservice, ICustomerDetailService customerdetailservice, IRetailBillItemService retailbillitemservice, IRetailBillService retailbillservice,
        ITransportService TransportService, IClientMasterService ClientMasterService, IEntryStockItemService EntryStockItemService, IItemService itemservice, IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IUserCredentialService usercredentialservice,
            IModuleService iIModuleService, IShopStockService ShopStockService, IGodownStockService GodownStockService, IEmployeeMasterService EmployeeMasterService, IShopService ShopService, IQuotationService QuotationService, IQuotationItemService QuotationItemService, ISalesOrderService SalesOrderService, IDeliveryChallanService DeliveryChallanService, ISalesOrderItemService SalesOrderItemService, IStockItemDistributionService StockItemDistributionService,
            IDeliveryChallanItemService DeliveryChallanItemService, IOpeningStockService OpeningStockService, IInventoryTaxService InventoryTaxService, ICashierReceivableService CashierReceivableService, ICashierRetailBillService CashierRetailBillService, ICashierRetailBillItemService CashierRetailBillItemService, IUserCredentialService UserCredentialService, IRetailBillAdjAmtDetailService RetailBillAdjAmtDetailService,
            INonInventoryItemService NonInventoryItemService, ITemporaryCashMemoService TemporaryCashMemoService, ITemporaryCashMemoItemService TemporaryCashMemoItemService, IDiscountMasterItemService DiscountMasterItemService)
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
            this._retailbillitemservice = retailbillitemservice;
            this._retailbillservice = retailbillservice;
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
            this._RetailBillAdjAmtDetailService = RetailBillAdjAmtDetailService;
            this._TemporaryCashMemoService = TemporaryCashMemoService;
            this._TemporaryCashMemoItemService = TemporaryCashMemoItemService;
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

        //CREATE RETAIL BILL
        [HttpGet]
        public ActionResult RetailBill()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
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
            return View(model);
        }

        // CREATE RETAIL BILL
        [HttpGet]
        public ActionResult RetailBillWithTax()
        {
            MainApplication model = new MainApplication()
            {
                RetailBillDetails = new RetailBill(),
            };

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastRetailBill = _retailbillservice.GetLastRetailBillByFinYr(FinYr, shopcode);
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
                int RetailIndex = LastRetailBill.RetailBillNo.LastIndexOf('R');
                RetailBillCode = LastRetailBill.RetailBillNo.Substring(RetailIndex + 2, 6);
                RetailBillLength = (Convert.ToInt32(RetailBillCode) + 1).ToString().Length;
                RetailBillNo = Convert.ToInt32(RetailBillCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            RetailBillCode = _utilityService.getName(ShortCode + "/RI", RetailBillLength, RetailBillNo);
            RetailBillCode = RetailBillCode + FinYr;
            model.RetailBillDetails.RetailBillNo = RetailBillCode;
            TempData["PreviousRetailno"] = RetailBillCode;

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

        //TEMP CASH MEMO NO AUTO COMPLETE
        [HttpGet]
        public JsonResult GetTemporaryCashMemoNo(string SearchTerm)
        {
            if (SearchTerm.ToLower().Contains("p"))
            {
                var tempcashmemodata = _TemporaryCashMemoService.GetBillsByConvertStatus();
                List<string> bills = new List<string>();
                foreach (var item in tempcashmemodata)
                {
                    bills.Add(item.TempCashMemoNo);
                }
                return Json(bills, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //GET ITEM LIST FROM QUOTATION
        [HttpGet]
        public ActionResult GetTempCashMemoItemsWithSellingPrice(string TempCashMemoNo)
        {
            MainApplication model = new MainApplication();
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByRetailBillNo(TempCashMemoNo);
            var tempcashmemoitemlist = model.TemporaryCashMemoItemList;
            tempcashmemoitemlist.ToList().Clear();
            foreach (var data in model.TemporaryCashMemoItemList)
            {
                //CALCULATE TOTAL STOCK QUANTITY..
                double Qty;
                double gdwnstkqty = 0;
                double shopstkqty = 0;
                var gdwnstkdata = _GodownStockService.GetRowsByItemCode(data.ItemCode);
                var shopstkdata = _ShopStockService.GetRowsByItemCode(data.ItemCode);

                //calculate sum of quantity in godown stock
                foreach (var data1 in gdwnstkdata)
                {
                    gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                }
                //calculate sum of quantity in shop stock
                foreach (var data2 in shopstkdata)
                {
                    shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                }
                //calulate total quantity
                Qty = gdwnstkqty + shopstkqty;
                data.ReadOnlyTotalStockQuantity = Qty;

                //calculate login shop quantity
                double loginshopquantity = 0;
                string loginshop = Session["LOGINSHOPGODOWNCODE"].ToString();
                var shopdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(data.ItemCode, loginshop);
                if (shopdata != null)
                {
                    loginshopquantity = Convert.ToDouble(shopdata.Quantity);
                }
                else
                {
                    loginshopquantity = 0;
                }
                data.ReadOnlyShopQuantity = loginshopquantity;

                //substring of barcode no (remove .png)
                if (data.Barcode != null)
                {
                    data.Barcode = data.Barcode.Substring(0, 9);
                }

                tempcashmemoitemlist.ToList().Add(data);
            }
            model.TemporaryCashMemoItemList.ToList().Clear();
            model.TemporaryCashMemoItemList = tempcashmemoitemlist;

            //TempData["QuotOrSalesItems"] = tempcashmemoitemlist;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetTempCashMemoItemsWithMRP(string TempCashMemoNo)
        {
            MainApplication model = new MainApplication();
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByRetailBillNo(TempCashMemoNo);
            var tempcashmemoitemlist = model.TemporaryCashMemoItemList;
            tempcashmemoitemlist.ToList().Clear();
            foreach (var data in model.TemporaryCashMemoItemList)
            {
                //CALCULATE TOTAL STOCK QUANTITY..
                double Qty;
                double gdwnstkqty = 0;
                double shopstkqty = 0;
                var gdwnstkdata = _GodownStockService.GetRowsByItemCode(data.ItemCode);
                var shopstkdata = _ShopStockService.GetRowsByItemCode(data.ItemCode);

                //calculate sum of quantity in godown stock
                foreach (var data1 in gdwnstkdata)
                {
                    gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                }
                //calculate sum of quantity in shop stock
                foreach (var data2 in shopstkdata)
                {
                    shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                }
                //calulate total quantity
                Qty = gdwnstkqty + shopstkqty;
                data.ReadOnlyTotalStockQuantity = Qty;

                //calculate login shop quantity
                double loginshopquantity = 0;
                string loginshop = Session["LOGINSHOPGODOWNCODE"].ToString();
                var shopdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(data.ItemCode, loginshop);
                if (shopdata != null)
                {
                    loginshopquantity = Convert.ToDouble(shopdata.Quantity);
                }
                else
                {
                    loginshopquantity = 0;
                }
                data.ReadOnlyShopQuantity = loginshopquantity;

                //substring of barcode no (remove .png)
                if (data.Barcode != null)
                {
                    data.Barcode = data.Barcode.Substring(0, 9);
                }

                tempcashmemoitemlist.ToList().Add(data);
            }
            model.TemporaryCashMemoItemList.ToList().Clear();
            model.TemporaryCashMemoItemList = tempcashmemoitemlist;

            //TempData["QuotOrSalesItems"] = tempcashmemoitemlist;
            return View(model);
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
            else if (BillNo.Contains("DC"))
            {
                var clientdata = _DeliveryChallanService.GetDetailsByChallanNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.FormType, clientdata.SalesPersonName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var clientdata = _TemporaryCashMemoService.GetDetailsByInvoiceNo(BillNo);
                Session["TemporaryCashMemo"] = clientdata;
                double TotalPayment = Convert.ToDouble(clientdata.Payment + clientdata.AdjustedAmount);
                return Json(new
                {
                    clientdata.ClientName,
                    clientdata.ClientEmail,
                    clientdata.ClientAddress,
                    clientdata.ClientContact,
                    clientdata.SalesPersonName,
                    clientdata.SalesPersonAddress,
                    clientdata.SalesPersonEmail,
                    clientdata.SalesPersonContact,
                    clientdata.SalesPersonDesignation,
                    clientdata.TotalAmount,
                    clientdata.TotalTaxAmount,
                    clientdata.GrandTotal,
                    clientdata.Balance,
                    TotalPayment,
                    clientdata.RefundToClient,
                    clientdata.QuotationNo,
                    clientdata.SalesOrderNo,
                    clientdata.DeliveryChallanNo
                }, JsonRequestBehavior.AllowGet);
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
                    string costprice = _itemservice.GetDescriptionByItemCode(data.ItemCode).costprice;
                    string subcat = _itemservice.GetDescriptionByItemCode(data.ItemCode).itemSubCategory;
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
                    discountrps,
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

        //GET TAX SUMMARY FROM QUOTATIONS
        [HttpGet]
        public JsonResult GetTaxFromTemporaryCashMemo(string TempCashMemoNo)
        {
            var list = _InventoryTaxService.GetTaxesByCode(TempCashMemoNo);
            var taxlist = list.Select(m => new InventoryTax
            {
                Tax = m.Tax,
                Amount = m.Amount,
                TaxAmount = m.TaxAmount
            });
            return Json(taxlist, JsonRequestBehavior.AllowGet);
        }

        // SAVE RETAIL INVOICE
        [HttpPost]
        public ActionResult RetailBill(MainApplication main, FormCollection frmcol)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();
            // SAVE RETAIL BILL MASTER
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastRetailBill = _retailbillservice.GetLastRetailBillByFinYr(FinYr, shopcode);
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
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            RetailBillCode = _utilityService.getName(ShortCode + "/RI", RetailBillLength, RetailBillNo);
            RetailBillCode = RetailBillCode + FinYr;


            //SAVE DIRECT(COUNTER) RETAIL BILL AND RETAIL BILL USING MULTIPLE BILLS..
            if (main.RetailBillDetails.TemporaryCashMemo == null)
            {
                main.RetailBillDetails.Date = DateTime.Now;
                main.RetailBillDetails.RetailBillNo = RetailBillCode;
                main.RetailBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                main.RetailBillDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                main.RetailBillDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                main.RetailBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                main.RetailBillDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                main.RetailBillDetails.Status = "Active";
                main.RetailBillDetails.ModifiedOn = DateTime.Now;
                main.RetailBillDetails.CashierStatus = "Active";
                main.RetailBillDetails.SalesReturn = "No";
                main.RetailBillDetails.AdjustedAmount = 0;
                main.RetailBillDetails.BillBalance = "None";
                main.RetailBillDetails.CreditNoteAmount = 0;

                //IF PAYMENT IS NULL THEN SAVE PAYMENT IS 0 IN RETAIL BILL
                if (main.RetailBillDetails.Payment == null)
                {
                    main.RetailBillDetails.Payment = 0;
                }

                //IF BALNCE IS 0 THEN WE CHECK THE REFUND FOR SAVE
                if (main.RetailBillDetails.Balance == 0)
                {
                    main.RetailBillDetails.Refund = main.RetailBillDetails.Payment - main.RetailBillDetails.GrandTotal;
                }
                else
                {
                    main.RetailBillDetails.Refund = 0;
                }

                main.RetailBillDetails.TaxStatus = frmcol["TaxStatus"];
                _retailbillservice.Create(main.RetailBillDetails);

                // SAVE CUSTOMER DETAILS
                if (main.RetailBillDetails.ClientName != null)
                {
                    main.CustomerDetails = new CustomerDetail();

                    var customerdata = _customerdetailservice.GetDetailsByName(main.RetailBillDetails.ClientName);
                    if (customerdata == null)
                    {
                        main.CustomerDetails.Name = main.RetailBillDetails.ClientName;
                        main.CustomerDetails.Address = main.RetailBillDetails.ClientAddress;
                        main.CustomerDetails.Contact = main.RetailBillDetails.ClientContact;
                        main.CustomerDetails.EmailId = main.RetailBillDetails.ClientEmail;
                        main.CustomerDetails.Status = "Active";
                        main.CustomerDetails.ModifiedOn = DateTime.Now;
                        _customerdetailservice.CreateInvoice(main.CustomerDetails);
                    }
                    else
                    {
                        customerdata.Address = main.RetailBillDetails.ClientAddress;
                        customerdata.Contact = main.RetailBillDetails.ClientContact;
                        customerdata.EmailId = main.RetailBillDetails.ClientEmail;
                        customerdata.ModifiedOn = DateTime.Now;
                        _customerdetailservice.UpdateInvoice(customerdata);
                    }
                }

                var retailbilltype = "";

                // SAVE DIRECT RETAIL BILL ITEMS
                if (main.RetailBillDetails.QuotationNo == null && main.RetailBillDetails.SalesOrderNo == null && main.RetailBillDetails.DeliveryChallanNo == null)
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
                            main.RetailBillItemDetails = new RetailBillItem();
                            main.RetailBillItemDetails.RetailBillNo = main.RetailBillDetails.RetailBillNo;
                            main.RetailBillItemDetails.ItemCode = frmcol[itemcode];
                            if (frmcol[itemtype] == "Inventory")
                            {
                                main.RetailBillItemDetails.ItemName = frmcol[itemname];
                            }
                            else
                            {
                                main.RetailBillItemDetails.ItemName = frmcol[itemnameval];
                            }
                            main.RetailBillItemDetails.ItemType = frmcol[itemtype];
                            if (frmcol[itemtype] == "Inventory")
                            {
                                main.RetailBillItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                            }
                            main.RetailBillItemDetails.Narration = frmcol[narration];
                            main.RetailBillItemDetails.Description = frmcol[description];
                            main.RetailBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                            main.RetailBillItemDetails.Unit = frmcol[unit];
                            main.RetailBillItemDetails.NumberType = frmcol[numbertype];
                            main.RetailBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                            main.RetailBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                            main.RetailBillItemDetails.Size = frmcol[size];
                            main.RetailBillItemDetails.Brand = frmcol[brand];
                            main.RetailBillItemDetails.DesignCode = frmcol[designcode];
                            main.RetailBillItemDetails.DesignName = frmcol[designnname];
                            main.RetailBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                            main.RetailBillItemDetails.Color = frmcol[color];
                            main.RetailBillItemDetails.Material = frmcol[material];
                            main.RetailBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                            main.RetailBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                            main.RetailBillItemDetails.ItemTax = frmcol[itemtax];
                            main.RetailBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                            main.RetailBillItemDetails.Status = "Active";
                            main.RetailBillItemDetails.ModifiedOn = DateTime.Now;
                            main.RetailBillItemDetails.SalesReturn = "No";
                            _retailbillitemservice.Create(main.RetailBillItemDetails);

                            if (frmcol[itemtype] == "Inventory")
                            {
                                //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - main.RetailBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
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
                            main.RetailBillItemDetails = new RetailBillItem();
                            main.RetailBillItemDetails.RetailBillNo = main.RetailBillDetails.RetailBillNo;
                            main.RetailBillItemDetails.BillNo = frmcol[billno];
                            main.RetailBillItemDetails.ItemCode = frmcol[itemcode];
                            if (frmcol[itemtype] == "Inventory")
                            {
                                main.RetailBillItemDetails.ItemName = frmcol[itemname];
                            }
                            else
                            {
                                if (frmcol[itemnameval] != null)
                                {
                                    main.RetailBillItemDetails.ItemName = frmcol[itemnameval];
                                }
                                else
                                {
                                    main.RetailBillItemDetails.ItemName = frmcol[itemname];
                                }
                            }
                            main.RetailBillItemDetails.ItemType = frmcol[itemtype];
                            if (frmcol[itemtype] == "Inventory")
                            {
                                main.RetailBillItemDetails.Barcode = frmcol[barcode] + ".png";
                            }
                            main.RetailBillItemDetails.Narration = frmcol[narration];
                            main.RetailBillItemDetails.Description = frmcol[description];
                            main.RetailBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                            main.RetailBillItemDetails.Unit = frmcol[unit];
                            main.RetailBillItemDetails.NumberType = frmcol[numbertype];
                            main.RetailBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                            main.RetailBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                            main.RetailBillItemDetails.Size = frmcol[size];
                            main.RetailBillItemDetails.Brand = frmcol[brand];
                            main.RetailBillItemDetails.DesignCode = frmcol[designcode];
                            main.RetailBillItemDetails.DesignName = frmcol[designnname];
                            main.RetailBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                            main.RetailBillItemDetails.Color = frmcol[color];
                            main.RetailBillItemDetails.Material = frmcol[material];
                            main.RetailBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                            main.RetailBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                            main.RetailBillItemDetails.ItemTax = frmcol[itemtax];
                            main.RetailBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                            main.RetailBillItemDetails.Status = "Active";
                            main.RetailBillItemDetails.ModifiedOn = DateTime.Now;
                            main.RetailBillItemDetails.SalesReturn = "No";
                            _retailbillitemservice.Create(main.RetailBillItemDetails);

                            //UPDATE QUOTATION, SALES ORDER, DELIVERY CHALLAN QUANTITY WHEN IT IS PROCESSED IN RETAIL BILL..
                            if (frmcol[billno] != null)
                            {
                                if (frmcol[billno].Contains("Qu"))
                                {
                                    var quotdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frmcol[itemcode], frmcol[billno]);
                                    quotdata.Balance = quotdata.Balance - (main.RetailBillItemDetails.Quantity);

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
                                    sodata.SOBalance = sodata.SOBalance - (main.RetailBillItemDetails.Quantity);

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
                                    dcdata.DelBalance = dcdata.DelBalance - (main.RetailBillItemDetails.Quantity);

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
                                    var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                                    shopstockdata.Quantity = shopstockdata.Quantity - main.RetailBillItemDetails.Quantity;
                                    _ShopStockService.Update(shopstockdata);

                                    //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                                    var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                                    stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                    _StockItemDistributionService.Update(stkdisdata);

                                    //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                                    var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                    //if item not present in entry stock then opening stock
                                    if (entrystkdata != null)
                                    {
                                        entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                        _EntryStockItemService.Update(entrystkdata);
                                    }
                                    //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                                    else
                                    {
                                        var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                        openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                        _OpeningStockService.UpdateStock(openingstkdata);
                                    }
                                }

                                //ONLY QUOTATION AND SALES ORDER ITEMS WILL BE MINUS INTO THE STOCK
                                else if (frmcol[billno].Contains("Qu") || frmcol[billno].Contains("SO"))
                                {
                                    //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                                    var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                                    shopstockdata.Quantity = shopstockdata.Quantity - main.RetailBillItemDetails.Quantity;
                                    _ShopStockService.Update(shopstockdata);

                                    //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                                    var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                                    stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                    _StockItemDistributionService.Update(stkdisdata);

                                    //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                                    var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                    //if item not present in entry stock then opening stock
                                    if (entrystkdata != null)
                                    {
                                        entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                        _EntryStockItemService.Update(entrystkdata);
                                    }
                                    //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                                    else
                                    {
                                        var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                        openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                        _OpeningStockService.UpdateStock(openingstkdata);
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
                    main.CashierRetailBillDetails = new CashierRetailBill();
                    main.CashierRetailBillItemDetails = new CashierRetailBillItem();

                    // SAVE CASHIER RECEIVABLE 
                    main.CashierReceivableDetails.CashierCode = cashiercode;
                    main.CashierReceivableDetails.Date = DateTime.Now;
                    main.CashierReceivableDetails.BillType = "RetailBill";
                    main.CashierReceivableDetails.Billno = main.RetailBillDetails.RetailBillNo;
                    main.CashierReceivableDetails.ClientName = main.RetailBillDetails.ClientName;
                    main.CashierReceivableDetails.ClientContact = main.RetailBillDetails.ClientContact;
                    main.CashierReceivableDetails.ClientState = frmcol["ClientState"];
                    main.CashierReceivableDetails.ClientType = frmcol["ClientType"];
                    main.CashierReceivableDetails.ClientFormType = frmcol["FormType"];

                    main.CashierReceivableDetails.PackAndForwd = 0;
                    main.CashierReceivableDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                    main.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                    main.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(main.RetailBillDetails.Payment);
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
                    main.CashierRetailBillDetails.CashierCode = cashiercode;
                    main.CashierRetailBillDetails.Date = DateTime.Now;
                    main.CashierRetailBillDetails.RetailBillDate = DateTime.Now;
                    main.CashierRetailBillDetails.RetailBillNo = main.RetailBillDetails.RetailBillNo;
                    //main.CashierRetailBillDetails.BillDateStatus = "Same";
                    main.CashierRetailBillDetails.ClientName = main.RetailBillDetails.ClientName;
                    main.CashierRetailBillDetails.ClientContact = main.RetailBillDetails.ClientContact;
                    main.CashierRetailBillDetails.ClientState = frmcol["ClientState"];
                    main.CashierRetailBillDetails.ClientType = frmcol["ClientType"];
                    main.CashierRetailBillDetails.FormType = frmcol["FormType"];
                    main.CashierRetailBillDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                    main.CashierRetailBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                    main.CashierRetailBillDetails.Payment = Convert.ToDouble(main.RetailBillDetails.Payment);
                    main.CashierRetailBillDetails.TotalAdvancePayment = Convert.ToDouble(main.RetailBillDetails.Payment);
                    main.CashierRetailBillDetails.SavingFrom = "RetailBill";
                    main.CashierRetailBillDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                    main.CashierRetailBillDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                    main.CashierRetailBillDetails.RefundAmount = main.RetailBillDetails.Refund;
                    main.CashierRetailBillDetails.AdjustedAmount = 0;
                    main.CashierRetailBillDetails.PaymentType = frmcol["CashierPaymentType"];
                    main.CashierRetailBillDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                    main.CashierRetailBillDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                    main.CashierRetailBillDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                    main.CashierRetailBillDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                    main.CashierRetailBillDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                    main.CashierRetailBillDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                    main.CashierRetailBillDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                    main.CashierRetailBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                    main.CashierRetailBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                    main.CashierRetailBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                    main.CashierRetailBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                    main.CashierRetailBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                    main.CashierRetailBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                    main.CashierRetailBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                    main.CashierRetailBillDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                    main.CashierRetailBillDetails.Refund = main.CashierReceivableDetails.Refund;
                    main.CashierRetailBillDetails.SelectedCard = frmcol["Card"];

                    main.CashierRetailBillDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                    if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                    {
                        main.CashierRetailBillDetails.CreditCardAmount = 0;
                        main.CashierRetailBillDetails.HandoverCreditAmt = 0;
                    }
                    else
                    {
                        main.CashierRetailBillDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                        main.CashierRetailBillDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    }
                    main.CashierRetailBillDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                    main.CashierRetailBillDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                    main.CashierRetailBillDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                    main.CashierRetailBillDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                    main.CashierRetailBillDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                    main.CashierRetailBillDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                    if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                    {
                        main.CashierRetailBillDetails.DebitCardAmount = 0;
                        main.CashierRetailBillDetails.HandoverDebitAmt = 0;
                    }
                    else
                    {
                        main.CashierRetailBillDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                        main.CashierRetailBillDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    }
                    main.CashierRetailBillDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                    main.CashierRetailBillDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                    if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                    {
                        main.CashierRetailBillDetails.ChequeAmount = 0;
                        main.CashierRetailBillDetails.HandoverChequeAmt = 0;
                    }
                    else
                    {
                        main.CashierRetailBillDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                        main.CashierRetailBillDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    }
                    if (main.CashierRetailBillDetails.ChequeNo != null && main.CashierReceivableDetails.ChequeNo != "")
                    {
                        main.CashierRetailBillDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                    }
                    else
                    {
                        main.CashierRetailBillDetails.ChequeDate = null;
                    }
                    main.CashierRetailBillDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];

                    main.CashierRetailBillDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                    main.CashierRetailBillDetails.Status = "Active";
                    main.CashierRetailBillDetails.ModifiedOn = DateTime.Now;

                    //SAVE HANDOVER STATUS
                    if (main.CashierRetailBillDetails.CreditCardAmount == 0 && main.CashierRetailBillDetails.DebitCardAmount == 0 && main.CashierRetailBillDetails.ChequeAmount == 0)
                    {
                        main.CashierRetailBillDetails.HandoverStatus = "InActive";
                    }
                    else
                    {
                        main.CashierRetailBillDetails.HandoverStatus = "Active";
                    }
                    //main.CashierRetailBillDetails.SalesReturn = "No";
                    _CashierRetailBillService.Create(main.CashierRetailBillDetails);

                    // SAVE CASHIER RETAIL BILL ITEM
                    main.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(main.RetailBillDetails.RetailBillNo);
                    foreach (var item in main.RetailBillItemList)
                    {
                        main.CashierRetailBillItemDetails.CashierCode = cashiercode;
                        main.CashierRetailBillItemDetails.RetailInvoiceNo = main.RetailBillDetails.RetailBillNo;
                        main.CashierRetailBillItemDetails.ItemName = item.ItemName;
                        main.CashierRetailBillItemDetails.ItemCode = item.ItemCode;
                        main.CashierRetailBillItemDetails.Description = item.Description;
                        main.CashierRetailBillItemDetails.Material = item.Material;
                        main.CashierRetailBillItemDetails.Color = item.Color;
                        main.CashierRetailBillItemDetails.SellingPrice = item.SellingPrice;
                        main.CashierRetailBillItemDetails.MRP = item.MRP;
                        main.CashierRetailBillItemDetails.Quantity = item.Quantity;
                        main.CashierRetailBillItemDetails.Amount = item.Amount;
                        main.CashierRetailBillItemDetails.DesignCode = item.DesignCode;
                        main.CashierRetailBillItemDetails.DesignName = item.DesignName;
                        main.CashierRetailBillItemDetails.Size = item.Size;
                        main.CashierRetailBillItemDetails.Brand = item.Brand;
                        main.CashierRetailBillItemDetails.Barcode = item.Barcode;
                        main.CashierRetailBillItemDetails.Unit = item.Unit;
                        main.CashierRetailBillItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                        main.CashierRetailBillItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                        main.CashierRetailBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                        main.CashierRetailBillItemDetails.Status = "Active";
                        main.CashierRetailBillItemDetails.ModifiedOn = DateTime.Now;
                        _CashierRetailBillItemService.Create(main.CashierRetailBillItemDetails);
                    }

                    var RetailBillData = _retailbillservice.GetDetailsByInvoiceNo(main.RetailBillDetails.RetailBillNo);

                    //if bill balance is 0 and cashier details also created then inactive that RETAIL BILL..
                    double retailbillbal = Convert.ToDouble(main.RetailBillDetails.Balance);
                    double retailbillref = Convert.ToDouble(main.RetailBillDetails.Refund);
                    if (retailbillbal == 0 && retailbillref == 0)
                    {
                        RetailBillData.Status = "InActive";
                        RetailBillData.CashierStatus = "InActive";
                        _retailbillservice.Update(RetailBillData);
                    }
                    //if we save the cashier payment details in retail bill then cashier status is ProcessInStatus for that bill..
                    else if (retailbillbal == 0)
                    {
                        RetailBillData.Status = "InActive";
                        _retailbillservice.Update(RetailBillData);
                    }
                }

                TempData["ClientName"] = main.RetailBillDetails.ClientName;
                TempData["RetailBill"] = main.RetailBillDetails.RetailBillNo;

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
                        main.InventoryTaxDetails.Code = main.RetailBillDetails.RetailBillNo;
                        main.InventoryTaxDetails.Amount = frmcol[amountontax];
                        main.InventoryTaxDetails.Tax = frmcol[taxnumber];
                        main.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                        _InventoryTaxService.Create(main.InventoryTaxDetails);
                    }
                }

                int lastRow = _retailbillservice.GetLastInvoiceDetails().RetailMasterId;
                var RetailId = Encode(lastRow.ToString());
                TempData["LastRetailBillId"] = lastRow;

                if (retailbilltype == "MultipleBill")
                {
                    if (main.RetailBillDetails.Balance == 0)
                    {
                        return RedirectToAction("RetailBillDetails/" + RetailId);
                    }
                    else
                    {
                        TempData["BillPayment"] = main.RetailBillDetails.Balance;
                        return RedirectToAction("AdjustAmountBackView", "RetailBill");
                    }
                }
                else
                {
                    return RedirectToAction("RetailBillDetails/" + RetailId);
                }
            }


            //SAVE RETAIL BILL FROM CONVERTIN TEMPORARY CASH MEMO INTO RETAIL BILL..
            else
            {
                main.RetailBillDetails.Date = DateTime.Now;
                main.RetailBillDetails.RetailBillNo = RetailBillCode;
                main.RetailBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                main.RetailBillDetails.TotalAmount = Convert.ToDouble(frmcol["hdnTotalAmount"]);
                main.RetailBillDetails.GrandTotal = Convert.ToDouble(frmcol["hdnGrandTotal"]);
                main.RetailBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
                main.RetailBillDetails.Balance = Convert.ToDouble(frmcol["hdnBalance"]);
                //main.RetailBillDetails.Status = "Active";
                main.RetailBillDetails.ModifiedOn = DateTime.Now;
                main.RetailBillDetails.SalesReturn = "No";
                main.RetailBillDetails.CreditNoteAmount = 0;

                var temporarycashmemodetails = Session["TemporaryCashMemo"] as TemporaryCashMemo;
                main.RetailBillDetails.Status = temporarycashmemodetails.Status;
                main.RetailBillDetails.CashierStatus = temporarycashmemodetails.CashierStatus;
                main.RetailBillDetails.RefundToClient = temporarycashmemodetails.RefundToClient;
                main.RetailBillDetails.Refund = temporarycashmemodetails.Refund;
                main.RetailBillDetails.Payment = temporarycashmemodetails.Payment;
                main.RetailBillDetails.AdjustedAmount = temporarycashmemodetails.AdjustedAmount;
                main.RetailBillDetails.AdjustedBill = temporarycashmemodetails.AdjustedBill;
                main.RetailBillDetails.BillBalance = temporarycashmemodetails.BillBalance;
                main.RetailBillDetails.CreditNoteAmount = 0;
                main.RetailBillDetails.TotalDiscount = 0;

                //IF PAYMENT IS NULL THEN SAVE PAYMENT IS 0 IN RETAIL BILL
                if (main.RetailBillDetails.Payment == null)
                {
                    main.RetailBillDetails.Payment = 0;
                }

                //IF BALNCE IS 0 THEN WE CHECK THE REFUND FOR SAVE
                if (main.RetailBillDetails.Balance == 0)
                {
                    main.RetailBillDetails.Refund = main.RetailBillDetails.Payment - main.RetailBillDetails.GrandTotal;
                }
                else
                {
                    main.RetailBillDetails.Refund = 0;
                }

                main.RetailBillDetails.TaxStatus = frmcol["TaxStatus"];
                _retailbillservice.Create(main.RetailBillDetails);


                // SAVE CUSTOMER DETAILS
                if (main.RetailBillDetails.ClientName != null)
                {
                    main.CustomerDetails = new CustomerDetail();

                    var customerdata = _customerdetailservice.GetDetailsByName(main.RetailBillDetails.ClientName);
                    if (customerdata == null)
                    {
                        main.CustomerDetails.Name = main.RetailBillDetails.ClientName;
                        main.CustomerDetails.Address = main.RetailBillDetails.ClientAddress;
                        main.CustomerDetails.Contact = main.RetailBillDetails.ClientContact;
                        main.CustomerDetails.EmailId = main.RetailBillDetails.ClientEmail;
                        main.CustomerDetails.Status = "Active";
                        main.CustomerDetails.ModifiedOn = DateTime.Now;
                        _customerdetailservice.CreateInvoice(main.CustomerDetails);
                    }
                    else
                    {
                        customerdata.Address = main.RetailBillDetails.ClientAddress;
                        customerdata.Contact = main.RetailBillDetails.ClientContact;
                        customerdata.EmailId = main.RetailBillDetails.ClientEmail;
                        customerdata.ModifiedOn = DateTime.Now;
                        _customerdetailservice.UpdateInvoice(customerdata);
                    }
                }

                //SAVE RETAIL BILL ITEMS FROM TEMPORARY CASH MEMO ITEMS..
                double rowcount = Convert.ToDouble(frmcol["hdnRowCountForTempCashMemo"]);

                for (int row = 1; row < rowcount; row++)
                {
                    string billnoval = "BillNoVal" + row;
                    string itemcode = "ItemCodeVal" + row;
                    string itemname = "ItemName" + row;
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

                    main.RetailBillItemDetails = new RetailBillItem();
                    main.RetailBillItemDetails.RetailBillNo = RetailBillCode;
                    main.RetailBillItemDetails.BillNo = frmcol[billnoval];
                    main.RetailBillItemDetails.ItemCode = frmcol[itemcode];
                    main.RetailBillItemDetails.ItemName = frmcol[itemname];
                    main.RetailBillItemDetails.ItemType = frmcol[itemtype];
                    main.RetailBillItemDetails.Narration = frmcol[narration];
                    main.RetailBillItemDetails.Barcode = frmcol[barcode];
                    main.RetailBillItemDetails.Description = frmcol[description];
                    main.RetailBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                    main.RetailBillItemDetails.Unit = frmcol[unit];
                    main.RetailBillItemDetails.NumberType = frmcol[numbertype];
                    main.RetailBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                    main.RetailBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                    main.RetailBillItemDetails.Size = frmcol[size];
                    main.RetailBillItemDetails.Brand = frmcol[brand];
                    main.RetailBillItemDetails.DesignCode = frmcol[designcode];
                    main.RetailBillItemDetails.DesignName = frmcol[designnname];
                    main.RetailBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                    main.RetailBillItemDetails.Color = frmcol[color];
                    main.RetailBillItemDetails.Material = frmcol[material];
                    main.RetailBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                    main.RetailBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                    main.RetailBillItemDetails.ItemTax = frmcol[itemtax];
                    main.RetailBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                    main.RetailBillItemDetails.SalesReturn = "No";
                    main.RetailBillItemDetails.Status = "Active";
                    main.RetailBillItemDetails.ModifiedOn = DateTime.Now;
                    _retailbillitemservice.Create(main.RetailBillItemDetails);


                    //THIS IS FOR UPDATE STOCK
                    if (frmcol[itemtype] == "Inventory")
                    {
                        //IF NEW INVENTORY/NON-INVENTORY ITEM IS ADD IN RETAIL BILL
                        if (frmcol[billnoval] == "")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - main.RetailBillItemDetails.Quantity;
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }

                        //ONLY QUOTATION AND SALES ORDER ITEMS WILL BE MINUS INTO THE STOCK
                        else if (frmcol[billnoval].Contains("Qu") || frmcol[billnoval].Contains("SO"))
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - main.RetailBillItemDetails.Quantity;
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.RetailBillItemDetails.ItemCode, main.RetailBillDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.RetailBillItemDetails.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.RetailBillItemDetails.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }
                    }

                }

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
                        main.InventoryTaxDetails.Code = main.RetailBillDetails.RetailBillNo;
                        main.InventoryTaxDetails.Amount = frmcol[amountontax];
                        main.InventoryTaxDetails.Tax = frmcol[taxnumber];
                        main.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                        _InventoryTaxService.Create(main.InventoryTaxDetails);
                    }
                }

                //when temporary cash memo converted into retail bill then ConvertStatus is InActive of that temporary cash memo..
                var tempcashmemodetails = _TemporaryCashMemoService.GetDetailsByInvoiceNo(main.RetailBillDetails.TemporaryCashMemo);
                tempcashmemodetails.Convertstatus = "InActive";

                if (Convert.ToDateTime(tempcashmemodetails.Date).Date == Convert.ToDateTime(DateTime.Now).Date)
                {
                    tempcashmemodetails.ConvertDateStatus = "Same";
                }
                else
                {
                    tempcashmemodetails.ConvertDateStatus = "Different";
                }
                _TemporaryCashMemoService.Update(tempcashmemodetails);

                int lastRow = _retailbillservice.GetLastInvoiceDetails().RetailMasterId;
                var RetailId = Encode(lastRow.ToString());
                TempData["LastRetailBillId"] = lastRow;
                return RedirectToAction("RetailBillDetails/" + RetailId);
            }
        }

        //********************************* RETAIL BILL WITHOUT TAX (ON MRP) ***************************************

        //CREATE RETAIL BILL WITHOUT TAX
        [HttpGet]
        public ActionResult RetailBillWithoutTax()
        {
            MainApplication model = new MainApplication()
            {
                RetailBillDetails = new RetailBill(),
            };

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastRetailBill = _retailbillservice.GetLastRetailBillByFinYr(FinYr, shopcode);
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
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            RetailBillCode = _utilityService.getName(ShortCode + "/RI", RetailBillLength, RetailBillNo);
            RetailBillCode = RetailBillCode + FinYr;
            model.RetailBillDetails.RetailBillNo = RetailBillCode;
            TempData["PreviousRetailno"] = RetailBillCode;

            model.EmpList = _EmployeeMasterService.GetEmployeeByStatus();
            model.ShopList = _ShopService.GetAll();

            return View(model);

        }

        //GET MULTIPLE BILL ITEM DETAILS POP UP PAGE
        [HttpGet]
        public ActionResult GetClientPendingBillsForWithoutTax(string BillNo, string ShopCode)
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




        // ****************************** BILL BALANCE ADJUSTMENT FROM SALES ORDER **********************************

        //BACK PAGE OF ADJUST AMOUNT POP UP
        [HttpGet]
        public ActionResult AdjustAmountBackView()
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
            var retailbilldata = _retailbillservice.GetDetailsByInvoiceNo(retailbill);
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
            _retailbillservice.Update(retailbilldata);

            //IN CASHIER totaladvancepayment = payment + adjust amount;
            //var cashiertaildata = _CashierRetailBillService.GetDetailsByRetailNo(retailbill);
            //if (cashiertaildata != null)
            //{
            //    double? payment = retailbilldata.Payment + retailbilldata.AdjustedAmount;
            //    cashiertaildata.TotalAdvancePayment = payment;
            //    cashiertaildata.AdjustedBill = MultipleSO;
            //    cashiertaildata.AdjustedAmount = Convert.ToDouble(frmcol["TotalAdjustAmount"]);

            //    //if bal < 0 then calculate refund
            //    var cashbal = cashiertaildata.Balance - Convert.ToDouble(frmcol["TotalAdjustAmount"]);
            //    if (cashbal <= 0)
            //    {
            //        //save refund in retail bill
            //        var cashrefund = Convert.ToDouble(frmcol["TotalAdjustAmount"]) - cashiertaildata.Balance;
            //        cashiertaildata.Refund = cashrefund;
            //        cashiertaildata.Balance = 0;
            //    }
            //    else
            //    {
            //        cashiertaildata.Balance = cashbal;
            //    }
            //    _CashierRetailBillService.Update(cashiertaildata);
            //}

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

            return RedirectToAction("ClosePopUp", "RetailBill");
        }

        //CLOSE POPUP AFTER ADJUST AMOUNT
        [HttpGet]
        public ActionResult ClosePopUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RetailBillDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.RetailBillDetails = _retailbillservice.GetDetailsById(Id);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.RetailBillDetails.RetailBillNo);
            model.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);
            string previousretailno = TempData["PreviousRetailno"].ToString();
            if (TempData["PreviousRetailno"].ToString() != model.RetailBillDetails.RetailBillNo)
            {
                ViewData["RetailnoChanged"] = previousretailno + " is replaced with " + model.RetailBillDetails.RetailBillNo + " because " + previousretailno + " is acquired by another person";
            }
            TempData["PreviousRetailno"] = previousretailno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintRetailBillWithTaxPreprinted(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RetailBillDetails = _retailbillservice.GetDetailsById(Id);
            model.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintRetailBillWithoutTaxPreprinted(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RetailBillDetails = _retailbillservice.GetDetailsById(Id);
            model.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(model.RetailBillDetails.RetailBillNo);
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

        //******************************************* UPDATE RETAIL BILL *************************************************

        // UPDATE RETAIL BILL
        [HttpGet]
        public ActionResult RetailBillUpdate()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RetailBillList = _retailbillservice.GetAll();

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
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
            return View(model);
        }

        //AUTO COMPLETE RETAIL BILL NO FOR EDIT
        [HttpGet]
        public JsonResult GetRetailBillNoForEdit(string SearchTerm, string taxstatus)
        {
            var retailbilldata = new List<RetailBill>();
            if (taxstatus == "WithTax")
            {
                retailbilldata = _retailbillservice.GetRetailBillNosByWithTaxStatus(SearchTerm, taxstatus).ToList();
            }
            else
            {
                retailbilldata = _retailbillservice.GetRetailBillNosByWithoutTaxStatus(SearchTerm, taxstatus).ToList();
            }
            List<string> names = new List<string>();
            foreach (var item in retailbilldata)
            {
                names.Add(item.RetailBillNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }


        //GET DETAILS BY RETAIL BILL NO FOR EDIT
        [HttpGet]
        public ActionResult GetDetailsByRBNoForEditWithTax(string retailbillno)
        {
            MainApplication model = new MainApplication();
            model.RetailBillDetails = _retailbillservice.GetDetailsByInvoiceNo(retailbillno);
            model.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(retailbillno);
            TempData["RetailBillItemList"] = model.RetailBillItemList;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(retailbillno);
            if (model.InventoryTaxList.Count() != 0)
            {
                TempData["RBTaxList"] = model.InventoryTaxList;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetDetailsByRBNoForEditWithoutTax(string retailbillno)
        {
            MainApplication model = new MainApplication();
            model.RetailBillDetails = _retailbillservice.GetDetailsByInvoiceNo(retailbillno);
            model.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(retailbillno);
            TempData["RetailBillItemList"] = model.RetailBillItemList;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(retailbillno);
            if (model.InventoryTaxList.Count() != 0)
            {
                TempData["RBTaxList"] = model.InventoryTaxList;
            }
            return View(model);
        }

        //GET STOCK QUANTITY & SHOP QUANTITY
        [HttpGet]
        public JsonResult GetStockandShopQuantity(string ItemCode, string shopcode)
        {
            //CALCULATE TOTAL QUANTITY..
            double? Qty;
            double? gdstkqty = 0;
            double? shopstkqty = 0;
            var gdstkdata = _GodownStockService.GetRowsByItemCode(ItemCode);
            var shopstkdata = _ShopStockService.GetRowsByItemCode(ItemCode);

            //calculate sum of quantity in godown stock
            foreach (var data1 in gdstkdata)
            {
                gdstkqty = gdstkqty + data1.Quantity;
            }
            //calculate sum of quantity in shop stock
            foreach (var data2 in shopstkdata)
            {
                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
            }
            //calulate total quantity
            Qty = gdstkqty + shopstkqty;
            double? shopqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(ItemCode, shopcode).Quantity;
            return Json(new { Qty, shopqty }, JsonRequestBehavior.AllowGet);
        }

        //SAVE RETAIL BILL UPDATE DATA
        [HttpPost]
        public ActionResult GetDetailsByRBNoForEdit(MainApplication model, FormCollection frmcol)
        {
            model.RetailBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            model.RetailBillDetails.TotalAmount = Convert.ToDouble(frmcol["Total_Amount_Value"]);
            model.RetailBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
            model.RetailBillDetails.GrandTotal = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            model.RetailBillDetails.Status = "Active";

            double? ActualPaymentForBill = model.RetailBillDetails.Payment + model.RetailBillDetails.AdjustedAmount;
            double? RemainingAmount = model.RetailBillDetails.GrandTotal - ActualPaymentForBill;
            if (RemainingAmount > 0)
            {
                model.RetailBillDetails.Balance = RemainingAmount;
                model.RetailBillDetails.Refund = 0;
                model.RetailBillDetails.CashierStatus = "Active";
                model.RetailBillDetails.Status = "Active";
            }
            else if (RemainingAmount < 0)
            {
                model.RetailBillDetails.Refund = 0 - RemainingAmount;
                model.RetailBillDetails.Balance = 0;
                model.RetailBillDetails.CashierStatus = "Active";
                model.RetailBillDetails.Status = "InActive";
            }
            else
            {
                model.RetailBillDetails.Balance = 0;
                model.RetailBillDetails.Refund = 0;
                model.RetailBillDetails.CashierStatus = "InActive";
                model.RetailBillDetails.Status = "InActive";
            }

            model.RetailBillDetails.ModifiedOn = DateTime.Now;
            _retailbillservice.Update(model.RetailBillDetails);

            var retailbillitemlist = TempData["RetailBillItemList"] as IEnumerable<RetailBillItem>;
            foreach (var item in retailbillitemlist)
            {
                var itemdata = _retailbillitemservice.GetById(item.SrNo);
                _retailbillitemservice.Delete(itemdata);
            }

            var itemtaxlist = TempData["RBTaxList"] as IEnumerable<InventoryTax>;
            if (itemtaxlist != null)
            {
                foreach (var tax in itemtaxlist)
                {
                    var taxdata = _InventoryTaxService.GetTaxById(tax.Id);
                    _InventoryTaxService.Delete(taxdata);
                }
            }

            int previouscount = 1;
            foreach (var item in retailbillitemlist)
            {
                string narration = "Narration" + previouscount;
                string quantity = "Quantity" + previouscount;
                string prevqty = "PrevQuantity" + previouscount;
                string sellingprice = "SellingPrice" + previouscount;
                string amount = "AmountVal" + previouscount;
                string itemtaxamt = "ItemTaxAmt" + previouscount;

                if (Convert.ToDouble(frmcol[quantity]) != 0)
                {
                    item.Narration = frmcol[narration];
                    item.Quantity = Convert.ToDouble(frmcol[quantity]);
                    item.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                    item.Amount = Convert.ToDouble(frmcol[amount]);
                    item.ItemTaxAmt = frmcol[itemtaxamt];
                    item.Status = "Active";
                    item.ModifiedOn = DateTime.Now;
                    _retailbillitemservice.Create(item);
                }

                double remainqty = Convert.ToDouble(frmcol[quantity]) - Convert.ToDouble(frmcol[prevqty]);
                if (item.ItemType == "Inventory")
                {
                    if (item.BillNo == null || item.BillNo.Contains("Qu") || item.BillNo.Contains("SO"))
                    {
                        //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - CHANGE QUANTITY)
                        var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.RetailBillDetails.ShopCode);
                        if (remainqty < 0)
                        {
                            shopstockdata.Quantity = shopstockdata.Quantity + (0 - remainqty);
                            _ShopStockService.Update(shopstockdata);
                        }
                        else if (remainqty > 0)
                        {
                            shopstockdata.Quantity = shopstockdata.Quantity - remainqty;
                            _ShopStockService.Update(shopstockdata);
                        }

                        //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - CHANGE QUANTITY)
                        var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.RetailBillDetails.ShopCode);
                        if (remainqty < 0)
                        {
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity + (0 - remainqty);
                            _StockItemDistributionService.Update(stkdisdata);
                        }
                        else if (remainqty > 0)
                        {
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - remainqty;
                            _StockItemDistributionService.Update(stkdisdata);
                        }

                        //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - CHANGE QUANTITY)
                        var entrystkdata = _EntryStockItemService.getDetailsByItemCode(item.ItemCode);
                        //if item not present in entry stock then opening stock
                        if (entrystkdata != null)
                        {
                            if (remainqty < 0)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity + (0 - remainqty);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            else if (remainqty > 0)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - remainqty;
                                _EntryStockItemService.Update(entrystkdata);
                            }
                        }
                        //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - CHANGE QUANTITY)
                        else
                        {
                            var openingstkdata = _OpeningStockService.GetDetailsByItemCode(item.ItemCode);
                            if (remainqty < 0)
                            {
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity + (0 - remainqty);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                            else if (remainqty > 0)
                            {
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - remainqty;
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }
                    }
                    else
                    {
                        if (remainqty > 0)
                        {
                            var challanitemdata = _DeliveryChallanItemService.GetDetailsByItemCodeAndChallanNo(item.ItemCode, item.BillNo);
                            double? delbalance = challanitemdata.DelBalance - remainqty;
                            if (challanitemdata.DelBalance < 0)
                            {
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.RetailBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - (0 - remainqty);
                                _ShopStockService.Update(shopstockdata);

                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.RetailBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - (0 - remainqty);
                                _StockItemDistributionService.Update(stkdisdata);

                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(item.ItemCode);
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - (0 - remainqty);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(item.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - (0 - remainqty);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }
                        }
                    }
                }

                if (item.BillNo != null)
                {
                    if (item.BillNo.Contains("Qu"))
                    {
                        var quotitemdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(item.ItemCode, item.BillNo);
                        if (remainqty < 0)
                        {
                            quotitemdata.Balance = quotitemdata.Balance + (0 - remainqty);
                            if (quotitemdata.Status == "InActive")
                            {
                                quotitemdata.Status = "Active";
                            }
                        }
                        else
                        {
                            quotitemdata.Balance = quotitemdata.Balance - remainqty;
                            if (quotitemdata.Balance <= 0)
                            {
                                quotitemdata.Balance = 0;
                                quotitemdata.Status = "InActive";
                            }
                        }
                        _QuotationItemService.Update(quotitemdata);

                        var QuotDetails = _QuotationService.GetDetailsByQuotNo(item.BillNo);
                        var QuotActiveItemsList = _QuotationItemService.GetAllActiveItemsByQuotNo(item.BillNo);
                        if (QuotActiveItemsList.Count() == 0)
                        {
                            QuotDetails.Status = "InActive";
                            _QuotationService.Update(QuotDetails);
                        }
                        else
                        {
                            QuotDetails.Status = "Active";
                            _QuotationService.Update(QuotDetails);
                        }
                    }
                    else if (item.BillNo.Contains("SO"))
                    {
                        var soitemdata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(item.ItemCode, item.BillNo);
                        if (remainqty < 0)
                        {
                            soitemdata.SOBalance = soitemdata.SOBalance + (0 - remainqty);
                            if (soitemdata.Status == "InActive")
                            {
                                soitemdata.Status = "Active";
                            }
                        }
                        else
                        {
                            soitemdata.SOBalance = soitemdata.SOBalance - remainqty;
                            if (soitemdata.SOBalance <= 0)
                            {
                                soitemdata.SOBalance = 0;
                                soitemdata.Status = "InActive";
                            }
                        }
                        _SalesOrderItemService.Update(soitemdata);

                        var SODetails = _SalesOrderService.GetDetailsBySalesOrderNo(item.BillNo);
                        var SOItemActiveList = _SalesOrderItemService.GetAllActiveItemsByOrderNo(item.BillNo);
                        if (SOItemActiveList.Count() == 0)
                        {
                            SODetails.Status = "InActive";
                            _SalesOrderService.Update(SODetails);
                        }
                        else
                        {
                            SODetails.Status = "Active";
                            _SalesOrderService.Update(SODetails);
                        }
                    }
                    else
                    {
                        var deliverychallanitem = _DeliveryChallanItemService.GetDetailsByItemCodeAndChallanNo(item.ItemCode, item.BillNo);
                        if (remainqty < 0)
                        {
                            deliverychallanitem.DelBalance = deliverychallanitem.DelBalance + (0 - remainqty);
                            if (deliverychallanitem.Status == "InActive")
                            {
                                deliverychallanitem.Status = "Active";
                            }
                        }
                        else
                        {
                            deliverychallanitem.DelBalance = deliverychallanitem.DelBalance - remainqty;
                            if (deliverychallanitem.DelBalance <= 0)
                            {
                                deliverychallanitem.DelBalance = 0;
                                deliverychallanitem.Status = "InActive";
                            }
                        }
                        _DeliveryChallanItemService.Update(deliverychallanitem);

                        var DCDetails = _DeliveryChallanService.GetDetailsByChallanNo(item.BillNo);
                        var DCItemActiveList = _DeliveryChallanItemService.GetAllActiveItemsByChallanNo(item.BillNo);
                        if (DCItemActiveList.Count() == 0)
                        {
                            DCDetails.Status = "InActive";
                            _DeliveryChallanService.Update(DCDetails);
                        }
                        else
                        {
                            DCDetails.Status = "Active";
                            _DeliveryChallanService.Update(DCDetails);
                        }
                    }
                }
                previouscount++;
            }

            int count = retailbillitemlist.Count() + 1;
            int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);
            for (int i = count; i <= rowcount; i++)
            {
                string itemcode = "ItemCode" + i;
                string itemname = "ItemName" + i;
                string itemnameval = "ItemNameVal" + i;
                string narration = "Narration" + i;
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
                string disrs = "DisRs" + i;
                string disper = "Disper" + i;
                string itemtax = "ItemTaxVal" + i;
                string itemtaxamt = "ItemTaxAmt" + i;

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
                    _retailbillitemservice.Create(model.RetailBillItemDetails);

                    if (frmcol[itemtype] == "Inventory")
                    {
                        //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - SALES BILL QUANTITY)
                        var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                        shopstockdata.Quantity = shopstockdata.Quantity - model.RetailBillItemDetails.Quantity;
                        _ShopStockService.Update(shopstockdata);

                        //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                        var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(model.RetailBillItemDetails.ItemCode, model.RetailBillDetails.ShopCode);
                        stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                        _StockItemDistributionService.Update(stkdisdata);

                        //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                        var entrystkdata = _EntryStockItemService.getDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                        //if item not present in entry stock then opening stock
                        if (entrystkdata != null)
                        {
                            entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                            _EntryStockItemService.Update(entrystkdata);
                        }
                        //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                        else
                        {
                            var openingstkdata = _OpeningStockService.GetDetailsByItemCode(model.RetailBillItemDetails.ItemCode);
                            openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(model.RetailBillItemDetails.Quantity);
                            _OpeningStockService.UpdateStock(openingstkdata);
                        }
                    }
                }
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
            TempData["PreviousRetailno"] = model.RetailBillDetails.RetailBillNo;

            var RetailId = Encode(model.RetailBillDetails.RetailMasterId.ToString());
            return RedirectToAction("RetailBillDetails/" + RetailId, "RetailBill");
        }


        //******************************************* PRINT RETAIL BILL *************************************************

        //PRINT RETAIL BILL DETAILS
        [HttpGet]
        public ActionResult RetailBillPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE RETAIL BILL NO
        [HttpGet]
        public JsonResult GetRetailBillNo(string term)
        {
            var retailbilldata = _retailbillservice.GetAllRetailBillNos(term);
            List<string> names = new List<string>();
            foreach (var item in retailbilldata)
            {
                names.Add(item.RetailBillNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET DETAILS BY RETAIL BILL NO FOR PRINT
        [HttpGet]
        public ActionResult GetRetailBillDetailsForPrint(string retailbillno)
        {
            MainApplication model = new MainApplication();
            model.RetailBillDetails = _retailbillservice.GetDetailsByInvoiceNo(retailbillno);
            model.RetailBillItemList = _retailbillitemservice.GetDetailsByInvoiceNo(retailbillno);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.RetailBillDetails.RetailBillNo);
            return View(model);
        }

    }
}
