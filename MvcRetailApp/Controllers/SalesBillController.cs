using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using CodeFirstServices.Interfaces;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //[NoDirectAccess]
    [SessionExpireFilter]
    public class SalesBillController : Controller
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
        private readonly IUtilityService _UtilityService;
        private readonly ISalesBillService _SalesBillservice;
        private readonly ISalesBillItemService _SalesBillItemService;
        private readonly IQuotationService _QuotationService;
        private readonly IQuotationItemService _QuotationItemService;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly IDeliveryChallanService _DeliveryChallanService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IShopService _ShopService;
        private readonly IShopStockService _ShopStockService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IInventoryTaxService _InventoryTaxService;
        private readonly IItemService _ItemService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly ISalesBillAdjAmtDetailService _SalesBillAdjAmtDetailService;
        private readonly ICashierSalesBillService _CashierSalesBillService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;

        public SalesBillController(IUserCredentialService IUserCredentialService, IModuleService iIModuleService, IUtilityService UtilityService, ISalesBillService SalesBillservice, ISalesBillItemService SalesBillItemService, IQuotationService QuotationService, IQuotationItemService QuotationItemService, ISalesOrderService SalesOrderService, ISalesOrderItemService SalesOrderItemService,
                                   IDeliveryChallanService DeliveryChallanService, IDeliveryChallanItemService DeliveryChallanItemService, IClientMasterService ClientMasterService, IEmployeeMasterService EmployeeMasterService, IStockItemDistributionService StockItemDistributionService, IGodownStockService GodownStockService, IShopService ShopService, IShopStockService ShopStockItemService,
                                    IEntryStockItemService EntryStockItemService, IOpeningStockService OpeningStockService, IInventoryTaxService InventoryTaxService, IItemService ItemService, INonInventoryItemService NonInventoryItemService, IItemTaxService ItemTaxService, ISalesBillAdjAmtDetailService SalesBillAdjAmtDetailService, ICashierSalesBillService CashierSalesBillService, IDiscountMasterItemService DiscountMasterItemService)
        {
            this._IUserCredentialService = IUserCredentialService;
            this._iIModuleService = iIModuleService;
            this._UtilityService = UtilityService;
            this._SalesBillservice = SalesBillservice;
            this._SalesBillItemService = SalesBillItemService;
            this._QuotationService = QuotationService;
            this._QuotationItemService = QuotationItemService;
            this._SalesOrderService = SalesOrderService;
            this._SalesOrderItemService = SalesOrderItemService;
            this._DeliveryChallanService = DeliveryChallanService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._ClientMasterService = ClientMasterService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._EntryStockItemService = EntryStockItemService;
            this._OpeningStockService = OpeningStockService;
            this._InventoryTaxService = InventoryTaxService;
            this._GodownStockService = GodownStockService;
            this._ShopService = ShopService;
            this._ShopStockService = ShopStockItemService;
            this._ItemService = ItemService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._ItemTaxService = ItemTaxService;
            this._SalesBillAdjAmtDetailService = SalesBillAdjAmtDetailService;
            this._CashierSalesBillService = CashierSalesBillService;
            this._DiscountMasterItemService = DiscountMasterItemService;
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
        public JsonResult EncodeSalesBillId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SalesBill()
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

        [HttpGet]
        public ActionResult SalesBillWithTax()
        {
            MainApplication model = new MainApplication();
            //CREATE SALESBILL CODE
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesBill = _SalesBillservice.GetLastSalesBillByFinYr(FinYr, shopcode);
            string SalesBillCode = string.Empty;
            int SalesBillNoLength = 0;
            int SalesBillNo = 0;
            if (LastSalesBill == null)
            {
                SalesBillNoLength = 1;
                SalesBillNo = 1;
            }
            else
            {
                int SalesBillIndex = LastSalesBill.SalesBillNo.LastIndexOf('S');
                SalesBillCode = LastSalesBill.SalesBillNo.Substring(SalesBillIndex + 2, 6);
                SalesBillNoLength = (Convert.ToInt32(SalesBillCode) + 1).ToString().Length;
                SalesBillNo = Convert.ToInt32(SalesBillCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            SalesBillCode = _UtilityService.getName(ShortCode + "/SB", SalesBillNoLength, SalesBillNo);
            SalesBillCode = SalesBillCode + FinYr;
            model.SalesBillCode = SalesBillCode;
            TempData["PreviousSalesBillNo"] = SalesBillCode;
            return View(model);
        }

        [HttpGet]
        public ActionResult SalesBillWithoutTax()
        {
            MainApplication model = new MainApplication();

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            //CREATE SALESBILL CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesBill = _SalesBillservice.GetLastSalesBillByFinYr(FinYr, shopcode);
            string SalesBillCode = string.Empty;
            int SalesBillNoLength = 0;
            int SalesBillNo = 0;
            if (LastSalesBill == null)
            {
                SalesBillNoLength = 1;
                SalesBillNo = 1;
            }
            else
            {
                int SalesBillIndex = LastSalesBill.SalesBillNo.LastIndexOf('S');
                SalesBillCode = LastSalesBill.SalesBillNo.Substring(SalesBillIndex + 2, 6);
                SalesBillNoLength = (Convert.ToInt32(SalesBillCode) + 1).ToString().Length;
                SalesBillNo = Convert.ToInt32(SalesBillCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            SalesBillCode = _UtilityService.getName(ShortCode + "/SB", SalesBillNoLength, SalesBillNo);
            SalesBillCode = SalesBillCode + FinYr;
            model.SalesBillCode = SalesBillCode;
            TempData["PreviousSalesBillNo"] = SalesBillCode;
            return View(model);
        }

        //AUTO COMPLETE CLIENT NAME
        [HttpGet]
        public JsonResult GetClientName(string term)
        {
            var ClientData = _ClientMasterService.GetClientNames(term);
            List<string> names = new List<string>();
            foreach (var item in ClientData)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE CLIENT NAME
        [HttpGet]
        public JsonResult GetActiveAndMaharashtraClients(string term)
        {
            var ClientData = _ClientMasterService.GetActiveAndMaharashtraClients(term);
            List<string> names = new List<string>();
            foreach (var item in ClientData)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET CLIENT DETAILS
        [HttpGet]
        public JsonResult GetClientDetails(string name)
        {
            var clientdata = _ClientMasterService.getClientByName(name);
            return Json(new
            {
                clientdata.Address,
                clientdata.State,
                clientdata.ContactNo1,
                clientdata.Email,
                clientdata.ConsumeResell,
                clientdata.ClientType,
                clientdata.FormType
            }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetClientDetailsOfBill(string BillNo)
        {
            if (BillNo.Contains("Qu"))
            {
                var clientdata = _QuotationService.GetDetailsByQuotNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.ConsumeResell, clientdata.FormType, clientdata.SalesPersonName, clientdata.SalesPersonAddress, clientdata.SalesPersonContact, clientdata.SalesPersonDesignation, clientdata.SalesPersonEmail, clientdata.TransportMode, clientdata.TransportName, clientdata.TransportContactNo }, JsonRequestBehavior.AllowGet);
            }
            else if (BillNo.Contains("SO"))
            {
                var clientdata = _SalesOrderService.GetDetailsByOrderNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.ConsumeResell, clientdata.FormType, clientdata.SalesPersonName, clientdata.SalesPersonAddress, clientdata.SalesPersonContact, clientdata.SalesPersonDesignation, clientdata.SalesPersonEmail, clientdata.TransportMode, clientdata.TransportName, clientdata.TransportContactNo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var clientdata = _DeliveryChallanService.GetDetailsByChallanNo(BillNo);
                return Json(new { clientdata.ClientName, clientdata.ClientEmail, clientdata.ClientAddress, clientdata.ClientContactNo, clientdata.ClientState, clientdata.ClientType, clientdata.ConsumeResell, clientdata.FormType, clientdata.SalesPersonName, clientdata.SalesPersonAddress, clientdata.SalesPersonContact, clientdata.SalesPersonDesignation, clientdata.SalesPersonEmail, clientdata.TransportMode, clientdata.TransportName, clientdata.TransportContactNo }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetLastSalesDate()
        {
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var dates = _SalesBillservice.GetLastSalesBillByFinYr(FinYr, shopcode);
            DateTime? lastdate;
            if (dates != null)
            {
                lastdate = dates.Date;
            }
            else
            {
                lastdate = DateTime.Now;
            }
            string date = Convert.ToDateTime(lastdate).ToShortDateString();
            return Json(date, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBarcodeItemDetails(string barcode, string shop, string ClientType, string ClientState, string FormType, string ConsumeResell)
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

        [HttpGet]
        public JsonResult GetItemsCostPrice(string itemcode, string itemtype)
        {
            string costprice = "0";
            if (itemtype == "Inventory")
            {
                costprice = _ItemService.GetDescriptionByItemCode(itemcode).costprice;
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

        //DISPLAY ITEM DISTAILS BY NONINVENTORY ITEM CODE
        [HttpGet]
        public JsonResult GetNonInvItemdetailsByItemCode(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell)
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

        //SAVE SALES BILL
        [HttpPost]
        public ActionResult SalesBill(MainApplication main, FormCollection frmcol)
        {

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesBill = _SalesBillservice.GetLastSalesBillByFinYr(FinYr, shopcode);
            string SalesBillCode = string.Empty;
            int SalesBillNoLength = 0;
            int SalesBillNo = 0;
            if (LastSalesBill == null)
            {
                SalesBillNoLength = 1;
                SalesBillNo = 1;
            }
            else
            {
                int SalesBillIndex = LastSalesBill.SalesBillNo.LastIndexOf('S');
                SalesBillCode = LastSalesBill.SalesBillNo.Substring(SalesBillIndex + 2, 6);
                SalesBillNoLength = (Convert.ToInt32(SalesBillCode) + 1).ToString().Length;
                SalesBillNo = Convert.ToInt32(SalesBillCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            SalesBillCode = _UtilityService.getName(ShortCode + "/SB", SalesBillNoLength, SalesBillNo);
            SalesBillCode = SalesBillCode + FinYr;
            main.SalesBillDetails.Date = DateTime.Now;
            main.SalesBillDetails.SalesBillNo = SalesBillCode;
            main.SalesBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            main.SalesBillDetails.TotalAmount = Convert.ToDouble(frmcol["Total_Amount_Value"]);
            main.SalesBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
            main.SalesBillDetails.GrandTotal = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            main.SalesBillDetails.TaxStatus = frmcol["TaxStatus"];
            main.SalesBillDetails.Balance = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            main.SalesBillDetails.Refund = 0;
            main.SalesBillDetails.AdjustedAmount = 0;
            main.SalesBillDetails.PaymentReceived = 0;
            main.SalesBillDetails.RefundToClient = 0;
            main.SalesBillDetails.BillDiscount = 0;
            main.SalesBillDetails.Status = "Active";
            main.SalesBillDetails.ModifiedOn = DateTime.Now;
            main.SalesBillDetails.CashierStatus = "Active";
            main.SalesBillDetails.SalesReturn = "No";
            _SalesBillservice.Create(main.SalesBillDetails);

            string salesbilltype = "";

            // SAVE DIRECT RETAIL BILL ITEMS
            if (main.SalesBillDetails.QuotationNo == null && main.SalesBillDetails.SalesOrderNo == null && main.SalesBillDetails.DeliveryChallanNo == null)
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
                        main.SalesBillItemDetails = new SalesBillItem();
                        main.SalesBillItemDetails.SalesBillNo = SalesBillCode;
                        main.SalesBillItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.SalesBillItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            main.SalesBillItemDetails.ItemName = frmcol[itemnameval];
                        }
                        main.SalesBillItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.SalesBillItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                        }
                        main.SalesBillItemDetails.Narration = frmcol[narration];
                        main.SalesBillItemDetails.Description = frmcol[description];
                        main.SalesBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        main.SalesBillItemDetails.Unit = frmcol[unit];
                        main.SalesBillItemDetails.NumberType = frmcol[numbertype];
                        if (main.SalesBillDetails.TaxStatus == "WithTax")
                        {
                            main.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                            if (frmcol[mrp] != "")
                            {
                                main.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                            }
                            else
                            {
                                main.SalesBillItemDetails.MRP = 0;
                            }
                        }
                        else
                        {
                            main.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[rate]);
                            if (frmcol[sellingprice] != "")
                            {
                                main.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                            }
                            else
                            {
                                main.SalesBillItemDetails.SellingPrice = 0;
                            }
                        }
                        main.SalesBillItemDetails.Size = frmcol[size];
                        main.SalesBillItemDetails.Brand = frmcol[brand];
                        main.SalesBillItemDetails.DesignCode = frmcol[designcode];
                        main.SalesBillItemDetails.DesignName = frmcol[designnname];
                        main.SalesBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        main.SalesBillItemDetails.Color = frmcol[color];
                        main.SalesBillItemDetails.Material = frmcol[material];
                        main.SalesBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        main.SalesBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        main.SalesBillItemDetails.ItemTax = frmcol[itemtax];
                        main.SalesBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        main.SalesBillItemDetails.ProportionateTax = frmcol[proptax];
                        main.SalesBillItemDetails.ProportionateTaxAmt = frmcol[proptaxamt];
                        main.SalesBillItemDetails.Status = "Active";
                        main.SalesBillItemDetails.ModifiedOn = DateTime.Now;
                        main.SalesBillItemDetails.SalesReturn = "No";
                        _SalesBillItemService.Create(main.SalesBillItemDetails);

                        if (frmcol[itemtype] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - SALES BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.SalesBillItemDetails.ItemCode, main.SalesBillDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - main.SalesBillItemDetails.Quantity;
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.SalesBillItemDetails.ItemCode, main.SalesBillDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.SalesBillItemDetails.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.SalesBillItemDetails.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
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
                        main.SalesBillItemDetails = new SalesBillItem();
                        main.SalesBillItemDetails.SalesBillNo = SalesBillCode;
                        main.SalesBillItemDetails.BillNo = frmcol[billno];
                        main.SalesBillItemDetails.ItemCode = frmcol[itemcode];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.SalesBillItemDetails.ItemName = frmcol[itemname];
                        }
                        else
                        {
                            if (frmcol[itemnameval] != null)
                            {
                                main.SalesBillItemDetails.ItemName = frmcol[itemnameval];
                            }
                            else
                            {
                                main.SalesBillItemDetails.ItemName = frmcol[itemname];
                            }
                        }
                        main.SalesBillItemDetails.ItemType = frmcol[itemtype];
                        if (frmcol[itemtype] == "Inventory")
                        {
                            main.SalesBillItemDetails.Barcode = frmcol[barcode].ToUpper() + ".png";
                        }
                        main.SalesBillItemDetails.Narration = frmcol[narration];
                        main.SalesBillItemDetails.Description = frmcol[description];
                        main.SalesBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        main.SalesBillItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        main.SalesBillItemDetails.Unit = frmcol[unit];
                        main.SalesBillItemDetails.NumberType = frmcol[numbertype];
                        if (main.SalesBillDetails.TaxStatus == "WithTax")
                        {
                            main.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                            if (frmcol[mrp] != "")
                            {
                                main.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                            }
                            else
                            {
                                main.SalesBillItemDetails.MRP = 0;
                            }
                        }
                        else
                        {
                            main.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[rate]);
                            if (frmcol[sellingprice] != "")
                            {
                                main.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                            }
                            else
                            {
                                main.SalesBillItemDetails.SellingPrice = 0;
                            }
                        }
                        main.SalesBillItemDetails.Size = frmcol[size];
                        main.SalesBillItemDetails.Brand = frmcol[brand];
                        main.SalesBillItemDetails.DesignCode = frmcol[designcode];
                        main.SalesBillItemDetails.DesignName = frmcol[designnname];
                        main.SalesBillItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        main.SalesBillItemDetails.Color = frmcol[color];
                        main.SalesBillItemDetails.Material = frmcol[material];
                        main.SalesBillItemDetails.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                        main.SalesBillItemDetails.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                        main.SalesBillItemDetails.ItemTax = frmcol[itemtax];
                        main.SalesBillItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        main.SalesBillItemDetails.ProportionateTax = frmcol[proptax];
                        main.SalesBillItemDetails.ProportionateTaxAmt = frmcol[proptaxamt];
                        main.SalesBillItemDetails.Status = "Active";
                        main.SalesBillItemDetails.ModifiedOn = DateTime.Now;
                        main.SalesBillItemDetails.SalesReturn = "No";
                        _SalesBillItemService.Create(main.SalesBillItemDetails);

                        //UPDATE QUOTATION, SALES ORDER, DELIVERY CHALLAN QUANTITY WHEN IT IS PROCESSED IN RETAIL BILL..
                        if (frmcol[billno] != null)
                        {
                            if (frmcol[billno].Contains("Qu"))
                            {
                                var quotdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frmcol[itemcode], frmcol[billno]);
                                quotdata.Balance = quotdata.Balance - (main.SalesBillItemDetails.Quantity);

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
                                sodata.SOBalance = sodata.SOBalance - (main.SalesBillItemDetails.Quantity);

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
                                dcdata.DelBalance = dcdata.DelBalance - (main.SalesBillItemDetails.Quantity);

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
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.SalesBillItemDetails.ItemCode, main.SalesBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - main.SalesBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.SalesBillItemDetails.ItemCode, main.SalesBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.SalesBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.SalesBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }

                            //ONLY QUOTATION AND SALES ORDER ITEMS WILL BE MINUS INTO THE STOCK
                            else if (frmcol[billno].Contains("Qu") || frmcol[billno].Contains("SO"))
                            {
                                //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - SALES BILL QUANTITY)
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(main.SalesBillItemDetails.ItemCode, main.SalesBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - main.SalesBillItemDetails.Quantity;
                                _ShopStockService.Update(shopstockdata);

                                //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - SALES BILL QUANTITY)
                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(main.SalesBillItemDetails.ItemCode, main.SalesBillDetails.ShopCode);
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                _StockItemDistributionService.Update(stkdisdata);

                                //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - SALES BILL QUANTITY)
                                var entrystkdata = _EntryStockItemService.getDetailsByItemCode(main.SalesBillItemDetails.ItemCode);
                                //if item not present in entry stock then opening stock
                                if (entrystkdata != null)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - SALES BILL QUANTITY)
                                else
                                {
                                    var openingstkdata = _OpeningStockService.GetDetailsByItemCode(main.SalesBillItemDetails.ItemCode);
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(main.SalesBillItemDetails.Quantity);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }
                        }
                    }
                }
            }

            TempData["ClientName"] = main.SalesBillDetails.ClientName;
            TempData["SalesBill"] = main.SalesBillDetails.SalesBillNo;

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
                    main.InventoryTaxDetails.Code = SalesBillCode;
                    main.InventoryTaxDetails.Amount = frmcol[amountontax];
                    main.InventoryTaxDetails.Tax = frmcol[taxnumber];
                    main.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _InventoryTaxService.Create(main.InventoryTaxDetails);
                }
            }
            var lastRow = _SalesBillservice.GetDetailsBySalesBillNo(SalesBillCode);
            string SalesBillId = Encode(lastRow.Id.ToString());
            TempData["LastSalesBillId"] = lastRow.Id;
            if (salesbilltype == "MultipleBill")
            {
                if (main.SalesBillDetails.Balance == 0)
                {
                    return RedirectToAction("SalesBillDetails/" + SalesBillId);
                }
                else
                {
                    TempData["BillPayment"] = main.SalesBillDetails.Balance;
                    return RedirectToAction("AdjustAmountBackView", "SalesBill");
                }
            }
            else
            {
                return RedirectToAction("SalesBillDetails/" + SalesBillId);
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

                    //
                }
            }

            //ADJUSTED AMOUNT SAVE IN SALES BILL & ADJUSTED AMOUNT IS MINUS INTO TO SALES ORDER BALANCE
            var salesbilldata = _SalesBillservice.GetDetailsBySalesBillNo(salesbill);
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
            _SalesBillservice.Update(salesbilldata);

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

            return RedirectToAction("ClosePopUp", "SalesBill");
        }

        //CLOSE POPUP AFTER ADJUST AMOUNT
        [HttpGet]
        public ActionResult ClosePopUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SalesBillEdit()
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

        [HttpGet]
        public JsonResult GetSalesBillNo(string SearchTerm, string taxstatus)
        {

            var salesbilllist = new List<SalesBill>();
            if (taxstatus == "WithTax")
            {
                salesbilllist = _SalesBillservice.GetSalesBillNosByWithTaxStatus(SearchTerm).ToList();
            }
            else
            {
                salesbilllist = _SalesBillservice.GetSalesBillNosByWithoutTaxStatus(SearchTerm).ToList();
            }
            List<string> names = new List<string>();
            foreach (var data in salesbilllist)
            {
                var cashierdone = _CashierSalesBillService.GetDetailsBySBNo(data.SalesBillNo);
                if (cashierdone == null)
                {
                    names.Add(data.SalesBillNo);
                }
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetailsBySalesBillWithTax(string billno)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillservice.GetDetailsBySalesBillNo(billno);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(billno);
            TempData["SalesBillItemList"] = model.SalesBillItemList;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(billno);
            TempData["InventoryTaxList"] = model.InventoryTaxList;
            return View(model);
        }

        [HttpGet]
        public ActionResult GetDetailsBySalesBillWithoutTax(string billno)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillservice.GetDetailsBySalesBillNo(billno);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(billno);
            TempData["SalesBillItemList"] = model.SalesBillItemList;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(billno);
            TempData["InventoryTaxList"] = model.InventoryTaxList;
            return View(model);
        }

        //GET STOCK QUANTITY
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

        [HttpPost]
        public ActionResult GetDetailsBySalesBill(MainApplication model, FormCollection frmcol)
        {
            model.SalesBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            model.SalesBillDetails.TotalAmount = Convert.ToDouble(frmcol["Total_Amount_Value"]);
            model.SalesBillDetails.TotalTaxAmount = Convert.ToDouble(frmcol["hdnTotalTaxAmount"]);
            model.SalesBillDetails.GrandTotal = Convert.ToDouble(frmcol["Grand_Total_Value"]);

            double? RemainingAmount = model.SalesBillDetails.GrandTotal - model.SalesBillDetails.AdjustedAmount;
            if (RemainingAmount > 0)
            {
                model.SalesBillDetails.Balance = RemainingAmount;
                model.SalesBillDetails.Refund = 0;
                model.SalesBillDetails.CashierStatus = "Active";
                model.SalesBillDetails.Status = "Active";
            }
            else if (RemainingAmount < 0)
            {
                model.SalesBillDetails.Refund = 0 - RemainingAmount;
                model.SalesBillDetails.Balance = 0;
                model.SalesBillDetails.CashierStatus = "Active";
                model.SalesBillDetails.Status = "InActive";
            }
            else
            {
                model.SalesBillDetails.Balance = 0;
                model.SalesBillDetails.Refund = 0;
                model.SalesBillDetails.CashierStatus = "InActive";
                model.SalesBillDetails.Status = "InActive";
            }

            model.SalesBillDetails.ModifiedOn = DateTime.Now;
            _SalesBillservice.Update(model.SalesBillDetails);

            var salesbillitemlist = TempData["SalesBillItemList"] as IEnumerable<SalesBillItem>;
            foreach (var item in salesbillitemlist)
            {
                var itemdata = _SalesBillItemService.GetItemById(item.Id);
                _SalesBillItemService.Delete(itemdata);
            }

            var itemtaxlist = TempData["InventoryTaxList"] as IEnumerable<InventoryTax>;
            if (itemtaxlist != null)
            {
                foreach (var tax in itemtaxlist)
                {
                    var taxdata = _InventoryTaxService.GetTaxById(tax.Id);
                    _InventoryTaxService.Delete(taxdata);
                }
            }

            int previouscount = 1;
            foreach (var item in salesbillitemlist)
            {
                string quantity = "Quantity" + previouscount;
                string narration = "Narration" + previouscount;
                string prevqty = "PrevQuantity" + previouscount;
                string sellingprice = "SellingPrice" + previouscount;
                string mrp = "MRP" + previouscount;
                string rate = "Rate" + previouscount;
                string disper = "DisPer" + previouscount;
                string disrs = "DisRs" + previouscount;
                string amount = "AmountVal" + previouscount;
                string itemtaxamt = "ItemTaxAmt" + previouscount;
                string proptaxvalue = "proportionatetaxvalue" + previouscount;
                string proportionateamount = "proportionatetaxamount" + previouscount;

                if (Convert.ToDouble(frmcol[quantity]) != 0)
                {
                    item.Narration = frmcol[narration];
                    item.Quantity = Convert.ToDouble(frmcol[quantity]);
                    if (model.SalesBillDetails.TaxStatus == "WithTax")
                    {
                        item.SellingPrice = Convert.ToDouble(frmcol[rate]);
                        item.MRP = Convert.ToDouble(frmcol[mrp]);
                    }
                    else
                    {
                        item.MRP = Convert.ToDouble(frmcol[rate]);
                        item.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);

                    }
                    item.Amount = Convert.ToDouble(frmcol[amount]);
                    item.DiscountPercent = Convert.ToDouble(frmcol[disper]);
                    item.DiscountRPS = Convert.ToDouble(frmcol[disrs]);
                    item.ItemTaxAmt = frmcol[itemtaxamt];
                    item.ProportionateTax = frmcol[proptaxvalue];
                    item.ProportionateTaxAmt = frmcol[proportionateamount];
                    item.Status = "Active";
                    item.ModifiedOn = DateTime.Now;
                    _SalesBillItemService.Create(item);
                }

                double remainqty = Convert.ToDouble(frmcol[quantity]) - Convert.ToDouble(frmcol[prevqty]);
                if (item.ItemType == "Inventory")
                {
                    if (item.BillNo == null || item.BillNo.Contains("Qu") || item.BillNo.Contains("SO"))
                    {
                        //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - CHANGE QUANTITY)
                        var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.SalesBillDetails.ShopCode);
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
                        var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.SalesBillDetails.ShopCode);
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
                                var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.SalesBillDetails.ShopCode);
                                shopstockdata.Quantity = shopstockdata.Quantity - (0 - remainqty);
                                _ShopStockService.Update(shopstockdata);

                                var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.SalesBillDetails.ShopCode);
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

            int count = salesbillitemlist.Count() + 1;
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
                string disrs = "DisRs" + i;
                string disper = "Disper" + i;
                string proptax = "proportionatetaxvalue" + i;
                string proptaxamt = "proportionatetaxamount" + i;
                string itemtax = "ItemTaxVal" + i;
                string itemtaxamt = "ItemTaxAmt" + i;

                if (frmcol[quantity] != null)
                {
                    model.SalesBillItemDetails = new SalesBillItem();
                    model.SalesBillItemDetails.SalesBillNo = model.SalesBillDetails.SalesBillNo;
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
                    if (model.SalesBillDetails.TaxStatus == "WithTax")
                    {
                        model.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                        model.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                    }
                    else
                    {
                        model.SalesBillItemDetails.MRP = Convert.ToDouble(frmcol[rate]);
                        model.SalesBillItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
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
            TempData["PreviousSalesBillNo"] = model.SalesBillDetails.SalesBillNo;
            string SalesBillId = Encode(model.SalesBillDetails.Id.ToString());
            return RedirectToAction("SalesBillDetails/" + SalesBillId, "SalesBill");
        }

        [HttpGet]
        public ActionResult SalesBillDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = new SalesBill();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.SalesBillDetails = _SalesBillservice.GetDetailsById(Id);
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
            model.SalesBillDetails = _SalesBillservice.GetDetailsById(Id);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(model.SalesBillDetails.SalesBillNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintSalesBillPreprinted(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillservice.GetDetailsById(Id);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(model.SalesBillDetails.SalesBillNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult SalesBillPrint()
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
        public JsonResult GetAllSalesBillNo(string term)
        {
            var salesbilllist = _SalesBillservice.GetAllSalesBill(term);
            List<string> names = new List<string>();
            foreach (var data in salesbilllist)
            {
                names.Add(data.SalesBillNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSalesBillForPrint(string billno)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillservice.GetDetailsBySalesBillNo(billno);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(billno);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(billno);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintSalesBillLetterHead(string id)
        {
            MainApplication model = new MainApplication();
            int Id  = Decode(id);
            model.SalesBillDetails = _SalesBillservice.GetDetailsById(Id);
            model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(model.SalesBillDetails.SalesBillNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesBillDetails.SalesBillNo);
            return View(model);
        }

    }
}

