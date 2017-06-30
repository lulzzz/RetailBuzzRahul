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
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class CashierModuleController : Controller
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
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly IDeliveryChallanService _DeliveryChallanService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly IRetailBillService _RetailInvoiceMasterService;
        private readonly IRetailBillItemService _RetailInvoiceItemService;
        private readonly ISalesBillService _SalesBillService;
        private readonly ISalesBillItemService _SalesBillItemService;
        private readonly ICashierReceivableService _CashierReceivableService;
        private readonly ICashierSalesOrderService _CashierSalesOrderService;
        private readonly ICashierSalesOrderItemService _CashierSalesOrderItemService;
        private readonly ICashierSalesBillService _CashierSalesBillService;
        private readonly ICashierSalesBillItemService _CashierSalesBillItemService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierRetailBillItemService _CashierRetailBillItemService;
        private readonly ICashierPayableService _CashierPayableService;
        private readonly ICashierRefundOrderService _CashierRefundOrderService;
        private readonly ICashierRefundOrderItemService _CashierRefundsOrderItemService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly ICashHandoverService _CashHandoverService;
        private readonly ICardChequeHandoverService _CardChequeHandoverService;
        private readonly IGodownStockService _ShopStockItemService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IBillPaymentService _BillPaymentService;
        private readonly IBillPaymentItemService _BillPaymentItemService;
        private readonly ISuppliersMasterService _SuppliersMasterService;
        private readonly IRetailBillCreditNoteService _RetailBillCreditNoteService;
        private readonly ISalesReturnService _SalesReturnService;
        private readonly ISalesReturnItemService _SalesReturnItemService;
        private readonly ITemporaryCashMemoService _TemporaryCashMemoService;
        private readonly ITemporaryCashMemoItemService _TemporaryCashMemoItemService;
        private readonly ICashierTemporaryCashMemoService _CashierTemporaryCashMemoService;
        private readonly ICashierTemporaryCashMemoItemService _CashierTemporaryCashMemoItemService;
        private readonly ISalesBillCreditNoteService _SalesBillCreditNoteService;
        private readonly ISalesBillCreditNoteItemService _SalesBillCreditNoteItemService;
        private readonly IJobWorkPaymentService _JobWorkPaymentService;

        public CashierModuleController(IUtilityService utilityservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService, ISalesOrderService SalesOrderService, ISalesOrderItemService SalesOrderItemservice,
                                       IDeliveryChallanService DeliveryChallanService, IDeliveryChallanItemService DeliveryChallanItemService, IRetailBillService RetailInvoiceMasterService, IRetailBillItemService RetailInvoiceItemService,
                                       ICashierReceivableService CashierReceivableService, ICashierSalesBillService CashierSalesBillService, ICashierSalesOrderService CashierSalesOrderService, ICashierRetailBillService CashierRetailBillService,
                                       ICashierSalesBillItemService CashierSalesBillItemService, ICashierSalesOrderItemService CashierSalesOrderItemService, ICashierRetailBillItemService CashierRetailBillItemService,
                                       IEmployeeMasterService EmployeeMasterService, ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService, IBillPaymentService BillPaymentService, IBillPaymentItemService BillPaymentItemService,
                                       ICashierPayableService CashierPayableService, ICashierRefundOrderService CashierRefundOrderService, ICashierRefundOrderItemService CashierRefundOrderItemService, IGodownStockService ShopStockItemService,
                                       IClientMasterService ClientMasterService, ICardChequeHandoverService CardChequeHandoverService, ICashHandoverService CashHandoverService,
                                       IInwardFromSupplierService InwardFromSupplierService, IInwardItemFromSupplierService InwardItemFromSupplierService,
                                       ISuppliersMasterService SuppliersMasterService, IRetailBillCreditNoteService RetailBillCreditNoteService, ISalesReturnService SalesReturnService, ISalesReturnItemService SalesReturnItemService,
                                       ITemporaryCashMemoService TemporaryCashMemoService, ITemporaryCashMemoItemService TemporaryCashMemoItemService, ICashierTemporaryCashMemoService CashierTemporaryCashMemoService, ICashierTemporaryCashMemoItemService CashierTemporaryCashMemoItemService,
                                       ISalesBillCreditNoteService SalesBillCreditNoteService, ISalesBillCreditNoteItemService SalesBillCreditNoteItemService, IJobWorkPaymentService JobWorkPaymentService)
        {
            this._utilityservice = utilityservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._SalesOrderService = SalesOrderService;
            this._SalesOrderItemService = SalesOrderItemservice;
            this._DeliveryChallanService = DeliveryChallanService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._RetailInvoiceMasterService = RetailInvoiceMasterService;
            this._RetailInvoiceItemService = RetailInvoiceItemService;
            this._SalesBillService = SalesBillService;
            this._SalesBillItemService = SalesBillItemService;
            this._CashierSalesOrderService = CashierSalesOrderService;
            this._CashierSalesBillService = CashierSalesBillService;
            this._CashierReceivableService = CashierReceivableService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierSalesOrderItemService = CashierSalesOrderItemService;
            this._CashierSalesBillItemService = CashierSalesBillItemService;
            this._CashierRetailBillItemService = CashierRetailBillItemService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._CashHandoverService = CashHandoverService;
            this._CardChequeHandoverService = CardChequeHandoverService;
            this._CashierPayableService = CashierPayableService;
            this._CashierRefundOrderService = CashierRefundOrderService;
            this._CashierRefundsOrderItemService = CashierRefundOrderItemService;
            this._ShopStockItemService = ShopStockItemService;
            this._ClientMasterService = ClientMasterService;
            this._InwardFromSupplierService = InwardFromSupplierService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._BillPaymentService = BillPaymentService;
            this._BillPaymentItemService = BillPaymentItemService;
            this._SuppliersMasterService = SuppliersMasterService;
            this._RetailBillCreditNoteService = RetailBillCreditNoteService;
            this._SalesReturnService = SalesReturnService;
            this._SalesReturnItemService = SalesReturnItemService;
            this._TemporaryCashMemoService = TemporaryCashMemoService;
            this._TemporaryCashMemoItemService = TemporaryCashMemoItemService;
            this._CashierTemporaryCashMemoService = CashierTemporaryCashMemoService;
            this._CashierTemporaryCashMemoItemService = CashierTemporaryCashMemoItemService;
            this._SalesBillCreditNoteService = SalesBillCreditNoteService;
            this._SalesBillCreditNoteItemService = SalesBillCreditNoteItemService;
            this._JobWorkPaymentService = JobWorkPaymentService;
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

        //MAIN PAGE OF CASHIER MODULE
        [HttpGet]
        public ActionResult CashierModule()
        {
            MainApplication model = new MainApplication();

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
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

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE BILL NO
        [HttpGet]
        public JsonResult GetBillNos(string SearchTerm, string billtype)
        {
            if (billtype != "Refund Order")
            {
                //CREATE CASHIER RECEIVABLE CODE
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                string cashiercode = string.Empty;

                var cashierdata = _CashierReceivableService.GetLastCashierByFinYr(FinYr);
                int CashierVal = 0;
                int length = 0;
                if (cashierdata != null)
                {
                    cashiercode = cashierdata.CashierCode.Substring(3, 6);
                    length = (Convert.ToInt32(cashiercode) + 1).ToString().Length;
                    CashierVal = Convert.ToInt32(cashiercode) + 1;
                }
                else
                {
                    CashierVal = 1;
                    length = 1;
                }
                cashiercode = _utilityservice.getName("CHR", length, CashierVal);
                cashiercode = cashiercode + FinYr;

                //AUTO COMPLETE BILL NO
                if (SearchTerm.ToUpper().Contains("S") && billtype == "Sales Order")
                {
                    var data = _SalesOrderService.GetOrdersByStatus(SearchTerm);
                    List<string> No = new List<string>();
                    foreach (var item in data)
                    {
                        No.Add(item.OrderNo);
                    }
                    return Json(new { No, cashiercode }, JsonRequestBehavior.AllowGet);
                }
                else if (SearchTerm.ToUpper().Contains("S") && billtype == "Additional Advance")
                {
                    var data = _CashierSalesOrderService.GetSalesOrderNos(SearchTerm);
                    List<string> No = new List<string>();
                    foreach (var item in data)
                    {
                        No.Add(item.OrderNo);
                    }
                    return Json(new { No, cashiercode }, JsonRequestBehavior.AllowGet);
                }
                else if (SearchTerm.ToUpper().Contains("S") && billtype == "Sales Bill")
                {
                    var data = _SalesBillService.GetSalesBillNosByCashierStatus(SearchTerm);
                    List<string> No = new List<string>();
                    foreach (var item in data)
                    {
                        No.Add(item.SalesBillNo);
                    }
                    return Json(new { No, cashiercode }, JsonRequestBehavior.AllowGet);
                }
                else if (SearchTerm.ToUpper().Contains("R") && billtype == "Retail Bill")
                {
                    var data = _RetailInvoiceMasterService.GetRetailByCashierStatus(SearchTerm);
                    List<string> No = new List<string>();
                    foreach (var item in data)
                    {
                        No.Add(item.RetailBillNo);
                    }
                    return Json(new { No, cashiercode }, JsonRequestBehavior.AllowGet);
                }
                else if (SearchTerm.ToUpper().Contains("P") && billtype == "Provision Cash Memo")
                {
                    var data = _TemporaryCashMemoService.GetDetailsByCashierAndConvertStatus(SearchTerm);
                    List<string> No = new List<string>();
                    foreach (var item in data)
                    {
                        No.Add(item.TempCashMemoNo);
                    }
                    return Json(new { No, cashiercode }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string msg = "Error";
                    return Json(new { msg, billtype }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //CREATE CASHIER PAYABLE CODE
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                string cashiercode = string.Empty;

                var cashierdata = _CashierPayableService.GetLastCashierByFinYr(FinYr);
                int CashierVal = 0;
                int length = 0;
                if (cashierdata != null)
                {
                    cashiercode = cashierdata.CashierCode.Substring(3, 6);
                    length = (Convert.ToInt32(cashiercode) + 1).ToString().Length;
                    CashierVal = Convert.ToInt32(cashiercode) + 1;
                }
                else
                {
                    CashierVal = 1;
                    length = 1;
                }
                cashiercode = _utilityservice.getName("CHP", length, CashierVal);
                cashiercode = cashiercode + FinYr;

                if (SearchTerm.ToUpper().Contains("S") && billtype == "Refund Order")
                {
                    var data = _SalesOrderService.GetActiveSalesOrderForAutoComplete(SearchTerm);
                    List<string> No = new List<string>();
                    foreach (var item in data)
                    {
                        No.Add(item.OrderNo);
                    }
                    return Json(new { No, cashiercode }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    string msg = "Error";
                    return Json(new { msg, billtype }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //AUTO COMPLETE CLIENT NAME
        [HttpGet]
        public JsonResult GetClientsForMultipleSB(string SearchTerm)
        {
            //CREATE CASHIER CODE FOR MULTIPLE SALES BILL
            var csahierdata = _CashierReceivableService.GetLastRow();
            int CashierVal = 0;
            int length = 0;
            if (csahierdata != null)
            {
                CashierVal = csahierdata.Id;
                CashierVal = CashierVal + 1;
                length = CashierVal.ToString().Length;
            }
            else
            {
                CashierVal = 1;
                length = 1;
            }
            string cashiercode = _utilityservice.getName("CHR", length, CashierVal);

            var clientdata = _ClientMasterService.GetClientNames(SearchTerm);
            List<string> names = new List<string>();
            foreach (var item in clientdata)
            {
                names.Add(item.ClientName);
            }
            return Json(new { names, cashiercode }, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE CREDIT NOTE NO FROM CLIENT NAME
        [HttpGet]
        public JsonResult GetCreditNoteNoForRetailBill(string SearchTerm, string client)
        {
            var creditnotenos = _RetailBillCreditNoteService.GetRetailBillCreditNoteNosFromClient(SearchTerm, client);
            List<string> names = new List<string>();
            foreach (var data in creditnotenos)
            {
                names.Add(data.CreditNoteNo);
            }
            return Json(new { names }, JsonRequestBehavior.AllowGet);
        }

        //GET CREDIT NOTE DETAILS
        [HttpGet]
        public JsonResult GetCreditNoteDetails(string no)
        {
            var data = _RetailBillCreditNoteService.GetCreditNoteDetails(no);
            return Json(new { data.Amount }, JsonRequestBehavior.AllowGet);
        }

        //GET SUPPLIER DETAILS BY SUPPLIER NAME
        [HttpGet]
        public JsonResult GetSuppplierDetails(string SupplierName)
        {
            var data = _SuppliersMasterService.GetByName(SupplierName);
            return Json(new { data.SupplierType, data.State, data.ContactNo1, data.Registered }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY SALES ORDER DETAILS ON POP UP PAGE
        public ActionResult SOBillDetailsPop(string no, string BillType)
        {
            MainApplication model = new MainApplication();
            if (BillType == "Sales Order")
            {
                model.SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(no);
                model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(no);
            }
            else if (BillType == "Additional Advance")
            {
                model.SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(no);
                var cashdetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(model.SalesOrderDetails.OrderNo);
                model.SalesOrderDetails.AdvancePayment = cashdetails.TotalAdvancePayment;
                model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(no);
            }
            else if (BillType == "Refund Order")
            {
                model.SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(no);
                model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(no);
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
            model.RetailBillDetails = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(no);
            model.RetailBillItemList = _RetailInvoiceItemService.GetDetailsByInvoiceNo(no);
            return View(model);
        }

        //DISPLAY TEMPORARY CASH MEMO DETAILS ON POP UP PAGE
        public ActionResult TemporaryCashMemoDetailsPop(string no)
        {
            MainApplication model = new MainApplication();
            model.TemporaryCashMemoDetails = _TemporaryCashMemoService.GetDetailsByInvoiceNo(no);
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNo(no);
            return View(model);
        }

        //GET DETAILS BY BILL NO
        [HttpGet]
        public ActionResult GetDataByBillNo(string BillNo)
        {
            if (BillNo.Contains("SO"))
            {
                //var data = _SalesOrderService.GetDetailsByOrderNo(BillNo);
                var data = _SalesOrderService.GetDetailsBySalesOrderNo(BillNo);
                return Json(new { data.ClientName, data.ClientContactNo, data.ClientType, data.ClientState, data.FormType }, JsonRequestBehavior.AllowGet);
            }
            else if (BillNo.Contains("RI"))
            {
                var data = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(BillNo);
                return Json(new { data.ClientName, data.ClientContact }, JsonRequestBehavior.AllowGet);
            }
            else if (BillNo.Contains("PCM"))
            {
                var data = _TemporaryCashMemoService.GetDetailsByInvoiceNo(BillNo);
                return Json(new { data.ClientName, data.ClientContact }, JsonRequestBehavior.AllowGet);
            }
            else if (BillNo.Contains("SB"))
            {
                var data = _SalesBillService.GetDetailsBySalesBillNo(BillNo);
                return Json(new { data.ClientName, data.ClientContactNo, data.ClientType, data.ClientState, data.FormType }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _InwardFromSupplierService.GetInwardByInwardNo(BillNo);
                return Json(new { data.SupplierName, data.SupplierContactNo, data.Registered, data.SupplierState, data.FormType }, JsonRequestBehavior.AllowGet);
            }
        }

        //DISPLAY BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetSalesOrderItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(BillNumber);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(BillNumber);
            TempData["SalesOrderDetails"] = model.SalesOrderDetails;
            TempData["SalesOrderItemList"] = model.SalesOrderItemList;
            return View(model);
        }

        //DISPLAY ADDITIONAL ADVANCE ITEM DETAILS IN MAIN PAGE
        public ActionResult GetAddAdvanceItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderDetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNumber);
            model.CashierSalesOrderItemList = _CashierSalesOrderItemService.GetDetailsByCashierCode(model.CashierSalesOrderDetails.CashierCode);
            model.SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(BillNumber);
            TempData["AddAdvSalesOrderDetails"] = model.CashierSalesOrderDetails;
            TempData["AddAdvSalesOrderItemList"] = model.CashierSalesOrderItemList;
            return View(model);
        }

        //DISPLAY SALES BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetSalesBillItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            string salesreturn = string.Empty;
            model.SalesBillDetails = _SalesBillService.GetDetailsBySalesBillNo(BillNumber);
            //if sales return no then get data from sales bill
            if (model.SalesBillDetails.SalesReturn == "No")
            {
                salesreturn = "No";
                model.SalesBillItemList = _SalesBillItemService.GetItemsBySalesBillNo(BillNumber);
            }
            //if sales return yes then get data from sales return
            else
            {
                salesreturn = "Yes";
                model.SalesReturnDetails = _SalesReturnService.GetDataByBillNoAndStatus(BillNumber);
                model.SalesReturnItemList = _SalesReturnItemService.GetItemListBySalesReturnNo(model.SalesReturnDetails.SalesReturnNo);
            }

            //this is use when we save the data in cashier database
            if (salesreturn == "No")
            {
                TempData["SalesReturn"] = "No";
                TempData["SalesBillDetails"] = model.SalesBillDetails;
                TempData["SalesBillItemList"] = model.SalesBillItemList;
            }
            else
            {
                TempData["SalesReturn"] = "Yes";
                TempData["SalesReturnDetails"] = model.SalesReturnDetails;
                TempData["SalesReturnItemList"] = model.SalesReturnItemList;
            }
            return View(model);
        }

        //GET CREDIT NOTE DETAILS FROM SALES BILL
        [HttpGet]
        public JsonResult GetCreditNoteDetailsFromSalesBill(string SearchTerm, string BillNo)
        {
            var creditnotenos = _SalesBillCreditNoteService.GetCreditNoteFromSalesBill(SearchTerm, BillNo);
            List<string> names = new List<string>();
            foreach (var data in creditnotenos)
            {
                names.Add(data.CreditNoteNo);
            }
            return Json(new { names }, JsonRequestBehavior.AllowGet);
        }

        //GET CREDIT NOTE DETAILS
        [HttpGet]
        public JsonResult GetSalesBillCreditNoteDetails(string no)
        {
            var data = _SalesBillCreditNoteService.GetCreditNoteDetails(no);
            return Json(new { data.Amount }, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE CREDIT NOTE NO FROM CLIENT NAME
        [HttpGet]
        public JsonResult GetCreditNoteNoForSalesBill(string SearchTerm, string client)
        {
            var creditnotenos = _SalesBillCreditNoteService.GetSalesBillCreditNoteNosFromClient(SearchTerm, client);
            List<string> names = new List<string>();
            foreach (var data in creditnotenos)
            {
                names.Add(data.CreditNoteNo);
            }
            return Json(new { names }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY MULTIPLE SALES BILL DETAILS IN MAIN PAGE
        public ActionResult GetMultipleSalesBillItemDetails(string clientname)
        {
            MainApplication model = new MainApplication();
            model.SalesBillList = _SalesBillService.GetSalesBillByClient(clientname);
            return View(model);
        }

        //CHECK RETAIL BILL IS NONE OR BILL BALANCE
        [HttpGet]
        public JsonResult CheckBillBalanceValue(string BillNo)
        {
            MainApplication model = new MainApplication();
            string BILLBAL = string.Empty;
            if (BillNo.Contains("RI"))
            {
                BILLBAL = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(BillNo).BillBalance;
            }
            else
            {
                BILLBAL = _TemporaryCashMemoService.GetDetailsByInvoiceNo(BillNo).BillBalance;
            }
            return Json(BILLBAL, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY RETAIL BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetRetailBillItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            string salesreturn = string.Empty;
            model.RetailBillDetails = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(BillNumber);
            //if sales return no then get data from retail bill
            if (model.RetailBillDetails.SalesReturn == "No")
            {
                salesreturn = "No";
                model.RetailBillItemList = _RetailInvoiceItemService.GetDetailsByInvoiceNoAndStatus(BillNumber);
            }
            //if sales return yes then get data from sales return
            else
            {
                salesreturn = "Yes";
                model.SalesReturnDetails = _SalesReturnService.GetDataByBillNoAndStatus(BillNumber);
                model.SalesReturnItemList = _SalesReturnItemService.GetItemListBySalesReturnNo(model.SalesReturnDetails.SalesReturnNo);
            }

            //this is use when we save the data in cashier database
            if (salesreturn == "No")
            {
                TempData["SalesReturn"] = "No";
                TempData["RetailBillDetails"] = model.RetailBillDetails;
                TempData["RetailBillItemList"] = model.RetailBillItemList;
            }
            else
            {
                TempData["SalesReturn"] = "Yes";
                TempData["SalesReturnDetails"] = model.SalesReturnDetails;
                TempData["SalesReturnItemList"] = model.SalesReturnItemList;
            }
            return View(model);
        }

        //DISPLAYTEMPORARY CASH MEMO ITEM DETAILS IN MAIN PAGE
        public ActionResult GetTemporaryCashMemoItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.TemporaryCashMemoDetails = _TemporaryCashMemoService.GetDetailsByInvoiceNo(BillNumber);
            model.TemporaryCashMemoItemList = _TemporaryCashMemoItemService.GetDetailsByInvoiceNoAndStatus(BillNumber);
            TempData["TemporaryCashMemoDetails"] = model.TemporaryCashMemoDetails;
            TempData["TemporaryCashMemoItemList"] = model.TemporaryCashMemoItemList;
            return View(model);
        }

        //GET CREDIT NOTE DETAILS FROM RETAIL BILL
        [HttpGet]
        public JsonResult GetCreditNoteDetailsFromRetailBill(string SearchTerm, string BillNo)
        {
            var creditnotenos = _RetailBillCreditNoteService.GetCreditNoteFromRetailBill(SearchTerm, BillNo);
            List<string> names = new List<string>();
            foreach (var data in creditnotenos)
            {
                names.Add(data.CreditNoteNo);
            }
            return Json(new { names }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY REFUND ITEM DETAILS IN MAIN PAGE
        public ActionResult GetRefundItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = _SalesOrderService.GetDetailsBySalesOrderNo(BillNumber);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(BillNumber);
            TempData["RefundOrderDetails"] = model.SalesOrderDetails;
            TempData["RefundOrderItemList"] = model.SalesOrderItemList;
            return View(model);
        }

        // PARTIAL VIEW OF CASH PAYMENT DETAILS
        public ActionResult CashPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CARD PAYMENT DETAILS
        public ActionResult CardPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CHEQUE PAYMENT DETAILS
        public ActionResult ChequePayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CASH CARD CHEQUE PAYMENT DETAILS
        public ActionResult CashCardChequePayment()
        {
            return View();
        }

        //FANCY BOX REFUND ALERT
        [HttpGet]
        public ActionResult RefundAlert(string Refund)
        {
            MainApplication model = new MainApplication();
            model.RefundAmount = Refund;
            return View(model);
        }

        //SAVE CASHIER MODULE
        [HttpPost]
        public ActionResult CashierModule(MainApplication mainapp, FormCollection frmcol)
        {
            //USE SAME MAIN APP OBJECT IN VIEW
            mainapp.CashierSalesOrderDetails = new CashierSalesOrder();
            mainapp.CashierSalesOrderItemDetails = new CashierSalesOrderItem();
            mainapp.CashierSalesBillDetails = new CashierSalesBill();
            mainapp.CashierSalesBillItemDetails = new CashierSalesBillItem();
            mainapp.CashierRetailBillDetails = new CashierRetailBill();
            mainapp.CashierRetailBillItemDetails = new CashierRetailBillItem();
            mainapp.CashierTemporaryCashMemoDetails = new CashierTemporaryCashMemo();
            mainapp.CashierTemporaryCashMemoItemDetails = new CashierTemporaryCashMemoItem();
            mainapp.CashierRefundOrderDetails = new CashierRefundOrder();
            mainapp.CashierRefundOrderItemDetails = new CashierRefundOrderItem();

            //SAVE CASHIER RECEIVABLES, SALES BILL, RETAIL BILL..
            if (frmcol["BillType"] != "Refund Order" && frmcol["BillType"] != "Multiple Sales Bill")
            {
                //CREATE CASHIER RECEIVABLE CODE
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                string cashiercode = string.Empty;

                var cashierdata = _CashierReceivableService.GetLastCashierByFinYr(FinYr);
                int CashierVal = 0;
                int length = 0;
                if (cashierdata != null)
                {
                    cashiercode = cashierdata.CashierCode.Substring(3, 6);
                    length = (Convert.ToInt32(cashiercode) + 1).ToString().Length;
                    CashierVal = Convert.ToInt32(cashiercode) + 1;
                }
                else
                {
                    CashierVal = 1;
                    length = 1;
                }
                cashiercode = _utilityservice.getName("CHR", length, CashierVal);
                cashiercode = cashiercode + FinYr;

                //SAVE CASHIER RECEIVABLES PAYMENT DETAILS
                mainapp.CashierReceivableDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                mainapp.CashierReceivableDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                mainapp.CashierReceivableDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                mainapp.CashierReceivableDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                mainapp.CashierReceivableDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                mainapp.CashierReceivableDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                mainapp.CashierReceivableDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);

                mainapp.CashierReceivableDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                mainapp.CashierReceivableDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                mainapp.CashierReceivableDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                mainapp.CashierReceivableDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                mainapp.CashierReceivableDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                mainapp.CashierReceivableDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                mainapp.CashierReceivableDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);

                mainapp.CashierReceivableDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);

                mainapp.CashierReceivableDetails.SelectedCard = frmcol["Card"];

                mainapp.CashierReceivableDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    mainapp.CashierReceivableDetails.CreditCardAmount = 0;
                    mainapp.CashierReceivableDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    mainapp.CashierReceivableDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    mainapp.CashierReceivableDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                mainapp.CashierReceivableDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                mainapp.CashierReceivableDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                mainapp.CashierReceivableDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                mainapp.CashierReceivableDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                mainapp.CashierReceivableDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                mainapp.CashierReceivableDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    mainapp.CashierReceivableDetails.DebitCardAmount = 0;
                    mainapp.CashierReceivableDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    mainapp.CashierReceivableDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    mainapp.CashierReceivableDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                mainapp.CashierReceivableDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                mainapp.CashierReceivableDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                {
                    mainapp.CashierReceivableDetails.ChequeAmount = 0;
                    mainapp.CashierReceivableDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    mainapp.CashierReceivableDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    mainapp.CashierReceivableDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                }
                if (mainapp.CashierReceivableDetails.ChequeNo != null && mainapp.CashierReceivableDetails.ChequeNo != "")
                {
                    mainapp.CashierReceivableDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    mainapp.CashierReceivableDetails.ChequeDate = null;
                }
                mainapp.CashierReceivableDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];
                mainapp.CashierReceivableDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                mainapp.CashierReceivableDetails.CashierCode = cashiercode;
                mainapp.CashierReceivableDetails.BillType = frmcol["BillType"];
                mainapp.CashierReceivableDetails.Billno = frmcol["BillNo"];
                mainapp.CashierReceivableDetails.Date = DateTime.Now;
                mainapp.CashierReceivableDetails.Status = "Active";
                mainapp.CashierReceivableDetails.ModifiedOn = DateTime.Now;

                var username = HttpContext.Session["UserName"].ToString();
                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierReceivableDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierReceivableDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierReceivableDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierReceivableDetails.ShopName = "SuperAdmin";
                }

                //SAVE CASHIER RECEIVABLES AMOUNT DETAILS USING SALES ORDER
                if (mainapp.CashierReceivableDetails.Billno.Contains("SO"))
                {
                    if (frmcol["BillType"] == "Sales Order")
                    {
                        var receivables = TempData["SalesOrderDetails"] as SalesOrder;
                        mainapp.CashierReceivableDetails.ClientName = receivables.ClientName;
                        mainapp.CashierReceivableDetails.ClientState = receivables.ClientState;
                        mainapp.CashierReceivableDetails.ClientContact = receivables.ClientContactNo;
                        mainapp.CashierReceivableDetails.ClientType = receivables.ClientType;
                        mainapp.CashierReceivableDetails.ClientFormType = receivables.FormType;
                        mainapp.CashierReceivableDetails.TotalAmount = receivables.TotalAmount;
                        mainapp.CashierReceivableDetails.PackAndForwd = Convert.ToDouble(receivables.PackAndForwd);
                        mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(receivables.TotalTaxAmount);
                        mainapp.CashierReceivableDetails.GrandTotal = receivables.GrandTotal;
                        mainapp.CashierReceivableDetails.AdvancePayment = receivables.AdvancePayment;
                        mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                    }
                    else if (frmcol["BillType"] == "Additional Advance")
                    {
                        var CashSODetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(frmcol["BillNo"]);
                        mainapp.CashierReceivableDetails.ClientName = CashSODetails.ClientName;
                        mainapp.CashierReceivableDetails.ClientState = CashSODetails.ClientState;
                        mainapp.CashierReceivableDetails.ClientContact = CashSODetails.ClientContact;
                        mainapp.CashierReceivableDetails.ClientType = CashSODetails.ClientType;
                        mainapp.CashierReceivableDetails.ClientFormType = CashSODetails.ClientFormType;
                        mainapp.CashierReceivableDetails.TotalAmount = CashSODetails.TotalAmount;
                        mainapp.CashierReceivableDetails.PackAndForwd = Convert.ToDouble(CashSODetails.PackAndForwd);
                        mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(CashSODetails.TotalTaxAmount);
                        mainapp.CashierReceivableDetails.GrandTotal = CashSODetails.GrandTotal;
                        mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["SalesOrderDetails.AdvancePayment"]);
                        mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                    }
                }

                //SAVE CASHIER RECEIVABLES AMOUNT DETAILS USING SALES BILL
                else if (mainapp.CashierReceivableDetails.Billno.Contains("SB"))
                {
                    string salesreturn = TempData["SalesReturn"].ToString();
                    //if sales return is no means we get data from retail bill
                    if (salesreturn == "No")
                    {
                        var receivables = TempData["SalesBillDetails"] as SalesBill;
                        mainapp.CashierReceivableDetails.ClientName = receivables.ClientName;
                        mainapp.CashierReceivableDetails.ClientState = receivables.ClientState;
                        mainapp.CashierReceivableDetails.ClientContact = receivables.ClientContactNo;
                        mainapp.CashierReceivableDetails.ClientType = receivables.ClientType;
                        mainapp.CashierReceivableDetails.ClientFormType = receivables.FormType;
                        mainapp.CashierReceivableDetails.TotalAmount = Convert.ToDouble(receivables.TotalAmount);
                        mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(receivables.TotalTaxAmount);
                        if (frmcol["BalanceVal"] != "")
                        {
                            mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                        }
                        else
                        {
                            mainapp.CashierReceivableDetails.Balance = 0;
                        }
                        mainapp.CashierReceivableDetails.GrandTotal = Convert.ToDouble(receivables.GrandTotal);
                        mainapp.CashierReceivableDetails.PackAndForwd = Convert.ToDouble(receivables.PackAndForwd);
                        mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["CashierSalesBillDetails.Payment"]);
                    }
                    else
                    {
                        var receivables = TempData["SalesReturnDetails"] as SalesReturn;
                        mainapp.CashierReceivableDetails.ClientName = receivables.ClientName;
                        mainapp.CashierReceivableDetails.ClientContact = receivables.ClientContact;
                        mainapp.CashierReceivableDetails.TotalAmount = Convert.ToDouble(receivables.TotalAmount);
                        mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(receivables.TotalTaxAmount);
                        if (frmcol["BalanceVal"] != "")
                        {
                            mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                        }
                        else
                        {
                            mainapp.CashierReceivableDetails.Balance = 0;
                        }
                        mainapp.CashierReceivableDetails.GrandTotal = Convert.ToDouble(receivables.GrandTotal);
                        mainapp.CashierReceivableDetails.PackAndForwd = Convert.ToDouble(receivables.PackAndForwd);
                        mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["CashierSalesBillDetails.Payment"]);
                    }
                }

                //SAVE CASHIER RECEIVABLES AMOUNT DETAILS USING RETAIL BILL
                else if (mainapp.CashierReceivableDetails.Billno.Contains("RI"))
                {
                    string salesreturn = TempData["SalesReturn"].ToString();
                    //if sales return is no means we get data from retail bill
                    if (salesreturn == "No")
                    {
                        var receivables = TempData["RetailBillDetails"] as RetailBill;
                        mainapp.CashierReceivableDetails.ClientName = receivables.ClientName;
                        mainapp.CashierReceivableDetails.ClientContact = receivables.ClientContact;
                        mainapp.CashierReceivableDetails.TotalAmount = Convert.ToDouble(receivables.TotalAmount);
                        mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(receivables.TotalTaxAmount);
                        if (frmcol["BalanceVal"] != "")
                        {
                            mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                        }
                        else
                        {
                            mainapp.CashierReceivableDetails.Balance = 0;
                        }
                        mainapp.CashierReceivableDetails.GrandTotal = Convert.ToDouble(receivables.GrandTotal);
                        mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["CashierRetailBillDetails.Payment"]);
                    }
                    //if sales return is yes means we get data from sales return
                    else
                    {
                        var receivables = TempData["SalesReturnDetails"] as SalesReturn;
                        mainapp.CashierReceivableDetails.ClientName = receivables.ClientName;
                        mainapp.CashierReceivableDetails.ClientContact = receivables.ClientContact;
                        mainapp.CashierReceivableDetails.TotalAmount = Convert.ToDouble(receivables.TotalAmount);
                        mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(receivables.TotalTaxAmount);
                        if (frmcol["BalanceVal"] != "")
                        {
                            mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                        }
                        else
                        {
                            mainapp.CashierReceivableDetails.Balance = 0;
                        }
                        mainapp.CashierReceivableDetails.GrandTotal = Convert.ToDouble(receivables.GrandTotal);
                        mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["CashierRetailBillDetails.Payment"]);
                    }
                }

                //SAVE CASHIER RECEIVABLES AMOUNT DETAILS USING TEMPORARY CASH MEMO
                else if (mainapp.CashierReceivableDetails.Billno.Contains("PCM"))
                {
                    var receivables = TempData["TemporaryCashMemoDetails"] as TemporaryCashMemo;
                    mainapp.CashierReceivableDetails.ClientName = receivables.ClientName;
                    mainapp.CashierReceivableDetails.ClientContact = receivables.ClientContact;
                    mainapp.CashierReceivableDetails.TotalAmount = Convert.ToDouble(receivables.TotalAmount);
                    mainapp.CashierReceivableDetails.TotalTaxAmount = Convert.ToDouble(receivables.TotalTaxAmount);
                    if (frmcol["BalanceVal"] != "")
                    {
                        mainapp.CashierReceivableDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
                    }
                    else
                    {
                        mainapp.CashierReceivableDetails.Balance = 0;
                    }
                    mainapp.CashierReceivableDetails.GrandTotal = Convert.ToDouble(receivables.GrandTotal);
                    mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["CashierTemporaryCashMemoDetails.Payment"]);
                }
                _CashierReceivableService.Create(mainapp.CashierReceivableDetails);
                // *********************************** CASHIER RECEIVABLE SAVE END ***********************************************


                // CHECK WHITCH TYPE OF BILL IS HIT AND SAVE INTO THE DATABSE
                string billno = frmcol["BillNo"];

                // SAVE CASHIER SALES ORDER
                if (billno.Contains("SO"))
                {
                    mainapp.CashierSalesOrderDetails.CashierCode = cashiercode;
                    mainapp.CashierSalesOrderDetails.OrderNo = frmcol["BillNo"];
                    mainapp.CashierSalesOrderDetails.CashierDate = DateTime.Now;
                    mainapp.CashierSalesOrderDetails.Status = "Active";
                    mainapp.CashierSalesOrderDetails.ModifiedOn = DateTime.Now;

                    if (frmcol["BillType"] == "Sales Order")
                    {
                        mainapp.SalesOrderDetails = TempData["SalesOrderDetails"] as SalesOrder;
                        mainapp.CashierSalesOrderDetails.ClientName = mainapp.SalesOrderDetails.ClientName;
                        mainapp.CashierSalesOrderDetails.ClientState = mainapp.SalesOrderDetails.ClientState;
                        mainapp.CashierSalesOrderDetails.ClientContact = mainapp.SalesOrderDetails.ClientContactNo;
                        mainapp.CashierSalesOrderDetails.ClientType = mainapp.SalesOrderDetails.ClientType;
                        mainapp.CashierSalesOrderDetails.ClientFormType = mainapp.SalesOrderDetails.FormType;
                        mainapp.CashierSalesOrderDetails.TotalAmount = mainapp.SalesOrderDetails.TotalAmount;
                        mainapp.CashierSalesOrderDetails.PackAndForwd = Convert.ToDouble(mainapp.SalesOrderDetails.PackAndForwd);
                        mainapp.CashierSalesOrderDetails.TotalTaxAmount = Convert.ToDouble(mainapp.SalesOrderDetails.TotalTaxAmount);
                        mainapp.CashierSalesOrderDetails.GrandTotal = mainapp.SalesOrderDetails.GrandTotal;
                        mainapp.CashierSalesOrderDetails.Balance = mainapp.CashierReceivableDetails.Balance;
                        mainapp.CashierSalesOrderDetails.AdvancePayment = mainapp.SalesOrderDetails.AdvancePayment;
                        mainapp.CashierSalesOrderDetails.TotalAdvancePayment = mainapp.CashierSalesOrderDetails.AdvancePayment;
                        mainapp.CashierSalesOrderDetails.SavingFrom = "SalesOrder";
                        mainapp.CashierSalesOrderDetails.OrderDate = mainapp.SalesOrderDetails.Date;
                    }
                    else if (frmcol["BillType"] == "Additional Advance")
                    {
                        //calculate total advance payment in additional advance..
                        var cashierdetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(frmcol["BillNo"]);
                        mainapp.CashierSalesOrderDetails.ClientName = cashierdetails.ClientName;
                        mainapp.CashierSalesOrderDetails.ClientState = cashierdetails.ClientState;
                        mainapp.CashierSalesOrderDetails.ClientContact = cashierdetails.ClientContact;
                        mainapp.CashierSalesOrderDetails.ClientType = cashierdetails.ClientType;
                        mainapp.CashierSalesOrderDetails.ClientFormType = cashierdetails.ClientFormType;
                        mainapp.CashierSalesOrderDetails.TotalAmount = cashierdetails.TotalAmount;
                        mainapp.CashierSalesOrderDetails.PackAndForwd = cashierdetails.PackAndForwd;
                        mainapp.CashierSalesOrderDetails.TotalTaxAmount = cashierdetails.TotalTaxAmount;
                        mainapp.CashierSalesOrderDetails.GrandTotal = cashierdetails.GrandTotal;
                        mainapp.CashierSalesOrderDetails.Balance = mainapp.CashierReceivableDetails.Balance;
                        mainapp.CashierSalesOrderDetails.AdvancePayment = Convert.ToDouble(frmcol["SalesOrderDetails.AdvancePayment"]);
                        mainapp.CashierSalesOrderDetails.TotalAdvancePayment = cashierdetails.TotalAdvancePayment + mainapp.CashierSalesOrderDetails.AdvancePayment;
                        mainapp.CashierSalesOrderDetails.SavingFrom = "AdditionalAdvance";
                        mainapp.CashierSalesOrderDetails.OrderDate = cashierdetails.OrderDate;

                        //update previous sales orders in cashier inactive..
                        var cashierlist = _CashierSalesOrderService.GetRowsBySONo(frmcol["BillNo"]);
                        foreach (var row in cashierlist)
                        {
                            row.Status = "InActive";
                            _CashierSalesOrderService.Update(row);
                        }

                        //UPDATE SALES ORDER TOTAL ADVANCE PAYMENT AFTER CASHIER ENTRY
                        var SOData = _SalesOrderService.GetDetailsByOrderNo(mainapp.CashierSalesOrderDetails.OrderNo);
                        SOData.TotalAdvancePayment = mainapp.CashierSalesOrderDetails.TotalAdvancePayment;
                        SOData.RemainingAdvance = SOData.RemainingAdvance + mainapp.CashierSalesOrderDetails.AdvancePayment;
                        _SalesOrderService.Update(SOData);
                    }

                    mainapp.CashierSalesOrderDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierSalesOrderDetails.PaymentType = mainapp.CashierReceivableDetails.PaymentType;

                    //save cashier sales order payment details..
                    mainapp.CashierSalesOrderDetails.Cash_1000 = mainapp.CashierReceivableDetails.Cash_1000;
                    mainapp.CashierSalesOrderDetails.Cash_500 = mainapp.CashierReceivableDetails.Cash_500;
                    mainapp.CashierSalesOrderDetails.Cash_100 = mainapp.CashierReceivableDetails.Cash_100;
                    mainapp.CashierSalesOrderDetails.Cash_50 = mainapp.CashierReceivableDetails.Cash_50;
                    mainapp.CashierSalesOrderDetails.Cash_20 = mainapp.CashierReceivableDetails.Cash_20;
                    mainapp.CashierSalesOrderDetails.Cash_10 = mainapp.CashierReceivableDetails.Cash_10;
                    mainapp.CashierSalesOrderDetails.Cash_1 = mainapp.CashierReceivableDetails.Cash_1;
                    mainapp.CashierSalesOrderDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                    mainapp.CashierSalesOrderDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                    mainapp.CashierSalesOrderDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                    mainapp.CashierSalesOrderDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                    mainapp.CashierSalesOrderDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                    mainapp.CashierSalesOrderDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                    mainapp.CashierSalesOrderDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                    mainapp.CashierSalesOrderDetails.TotalCash = mainapp.CashierReceivableDetails.TotalCash;
                    mainapp.CashierSalesOrderDetails.SelectedCard = mainapp.CashierReceivableDetails.SelectedCard;
                    mainapp.CashierSalesOrderDetails.CreditCardNo = mainapp.CashierReceivableDetails.CreditCardNo;
                    mainapp.CashierSalesOrderDetails.CreditCardType = mainapp.CashierReceivableDetails.CreditCardType;
                    mainapp.CashierSalesOrderDetails.CreditCardAmount = mainapp.CashierReceivableDetails.CreditCardAmount;
                    mainapp.CashierSalesOrderDetails.HandoverCreditAmt = mainapp.CashierReceivableDetails.HandoverCreditAmt;
                    mainapp.CashierSalesOrderDetails.CreditCardBank = mainapp.CashierReceivableDetails.CreditCardBank;
                    mainapp.CashierSalesOrderDetails.DebitCardName = mainapp.CashierReceivableDetails.DebitCardName;
                    mainapp.CashierSalesOrderDetails.DebitCardNo = mainapp.CashierReceivableDetails.DebitCardNo;
                    mainapp.CashierSalesOrderDetails.DebitCardAmount = mainapp.CashierReceivableDetails.DebitCardAmount;
                    mainapp.CashierSalesOrderDetails.HandoverDebitAmt = mainapp.CashierReceivableDetails.HandoverDebitAmt;
                    mainapp.CashierSalesOrderDetails.DebitCardType = mainapp.CashierReceivableDetails.DebitCardType;
                    mainapp.CashierSalesOrderDetails.DebitCardBank = mainapp.CashierReceivableDetails.DebitCardBank;
                    mainapp.CashierSalesOrderDetails.ChequeNo = mainapp.CashierReceivableDetails.ChequeNo;
                    mainapp.CashierSalesOrderDetails.ChequeAccNo = mainapp.CashierReceivableDetails.ChequeAccNo;
                    mainapp.CashierSalesOrderDetails.ChequeAmount = mainapp.CashierReceivableDetails.ChequeAmount;
                    mainapp.CashierSalesOrderDetails.HandoverChequeAmt = mainapp.CashierReceivableDetails.HandoverChequeAmt;
                    if (mainapp.CashierSalesOrderDetails.ChequeNo != null)
                    {
                        mainapp.CashierSalesOrderDetails.ChequeDate = mainapp.CashierReceivableDetails.ChequeDate;
                    }
                    else
                    {
                        mainapp.CashierSalesOrderDetails.ChequeDate = null;
                    }
                    mainapp.CashierSalesOrderDetails.ChequeBank = mainapp.CashierReceivableDetails.ChequeBank;
                    mainapp.CashierSalesOrderDetails.ChequeBranch = mainapp.CashierReceivableDetails.ChequeBranch;

                    //SAVE HANDOVER STATUS
                    if (mainapp.CashierSalesOrderDetails.CreditCardAmount == 0 && mainapp.CashierSalesOrderDetails.DebitCardAmount == 0 && mainapp.CashierSalesOrderDetails.ChequeAmount == 0)
                    {
                        mainapp.CashierSalesOrderDetails.HandoverStatus = "InActive";
                    }
                    else
                    {
                        mainapp.CashierSalesOrderDetails.HandoverStatus = "Active";
                    }

                    //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                    if (username != "SuperAdmin")
                    {
                        mainapp.CashierSalesOrderDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        mainapp.CashierSalesOrderDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                    }
                    else
                    {
                        mainapp.CashierSalesOrderDetails.ShopCode = "SuperAdmin";
                        mainapp.CashierSalesOrderDetails.ShopName = "SuperAdmin";
                    }
                    _CashierSalesOrderService.Create(mainapp.CashierSalesOrderDetails);


                    //SAVE CASHIER SALES ORDER ITEM
                    if (mainapp.CashierSalesOrderDetails.SavingFrom == "SalesOrder")
                    {
                        mainapp.SalesOrderItemList = TempData["SalesOrderItemList"] as IEnumerable<SalesOrderItem>;
                        foreach (var item in mainapp.SalesOrderItemList)
                        {
                            mainapp.CashierSalesOrderItemDetails.CashierCode = cashiercode;
                            mainapp.CashierSalesOrderItemDetails.OrderNo = frmcol["BillNo"];
                            mainapp.CashierSalesOrderItemDetails.ItemName = item.ItemName;
                            mainapp.CashierSalesOrderItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierSalesOrderItemDetails.Description = item.Description;
                            mainapp.CashierSalesOrderItemDetails.Material = item.Material;
                            mainapp.CashierSalesOrderItemDetails.Color = item.Color;
                            mainapp.CashierSalesOrderItemDetails.SellingPrice = item.SellingPrice;
                            mainapp.CashierSalesOrderItemDetails.MRP = item.MRP;
                            mainapp.CashierSalesOrderItemDetails.Quantity = item.Quantity;
                            mainapp.CashierSalesOrderItemDetails.Amount = item.Amount;
                            mainapp.CashierSalesOrderItemDetails.DesignName = item.DesignName;
                            mainapp.CashierSalesOrderItemDetails.Unit = item.Unit;
                            mainapp.CashierSalesOrderItemDetails.DisInRs = Convert.ToDouble(item.DisInRs);
                            mainapp.CashierSalesOrderItemDetails.DisInPer = Convert.ToDouble(item.DisInPercent);
                            mainapp.CashierSalesOrderItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierSalesOrderItemDetails.Status = "Active";
                            mainapp.CashierSalesOrderItemDetails.ModifiedOn = DateTime.Now;
                            _CashierSalesOrderItemService.Create(mainapp.CashierSalesOrderItemDetails);
                        }

                        // SALES ORDER CASHIER STATUS IS INACTIVE WHEN SO IS SAVE INTO THE CASHIER DATABASE..
                        var salesorderdata = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol["BillNo"]);
                        salesorderdata.CashierStatus = "InActive";
                        _SalesOrderService.Update(salesorderdata);
                    }

                    //SAVE CASHIER ADDITIONAL ADVANCE ITEM
                    else
                    {
                        mainapp.CashierSalesOrderItemList = TempData["AddAdvSalesOrderItemList"] as IEnumerable<CashierSalesOrderItem>;
                        foreach (var item in mainapp.CashierSalesOrderItemList)
                        {
                            mainapp.CashierSalesOrderItemDetails.CashierCode = cashiercode;
                            mainapp.CashierSalesOrderItemDetails.OrderNo = frmcol["BillNo"];
                            mainapp.CashierSalesOrderItemDetails.ItemName = item.ItemName;
                            mainapp.CashierSalesOrderItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierSalesOrderItemDetails.Description = item.Description;
                            mainapp.CashierSalesOrderItemDetails.Material = item.Material;
                            mainapp.CashierSalesOrderItemDetails.Color = item.Color;
                            mainapp.CashierSalesOrderItemDetails.SellingPrice = item.SellingPrice;
                            mainapp.CashierSalesOrderItemDetails.MRP = item.MRP;
                            mainapp.CashierSalesOrderItemDetails.Quantity = item.Quantity;
                            mainapp.CashierSalesOrderItemDetails.Amount = item.Amount;
                            mainapp.CashierSalesOrderItemDetails.DesignName = item.DesignName;
                            mainapp.CashierSalesOrderItemDetails.Unit = item.Unit;
                            mainapp.CashierSalesOrderItemDetails.DisInRs = Convert.ToDouble(item.DisInRs);
                            mainapp.CashierSalesOrderItemDetails.DisInPer = Convert.ToDouble(item.DisInPer);
                            mainapp.CashierSalesOrderItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierSalesOrderItemDetails.Status = "Active";
                            mainapp.CashierSalesOrderItemDetails.ModifiedOn = DateTime.Now;
                            _CashierSalesOrderItemService.Create(mainapp.CashierSalesOrderItemDetails);
                        }
                    }


                }
                //*********************************** CASHIER SALES ORDER SAVE END ***************************************

                // SAVE CASHIER SALES BILL
                else if (billno.Contains("SB"))
                {
                    //update previous sales bill in cashier inactive..
                    var cashiersalesbilllist = _CashierSalesBillService.GetRowsBySBNo(frmcol["BillNo"]);
                    foreach (var row in cashiersalesbilllist)
                    {
                        row.Status = "InActive";
                        _CashierSalesBillService.Update(row);
                    }

                    // SAVE CASHIER SALES BILL
                    mainapp.CashierSalesBillDetails.CashierCode = cashiercode;
                    mainapp.CashierSalesBillDetails.SalesBillNo = frmcol["BillNo"];
                    mainapp.CashierSalesBillDetails.Date = DateTime.Now;
                    mainapp.CashierSalesBillDetails.Status = "Active";
                    mainapp.CashierSalesBillDetails.ModifiedOn = DateTime.Now;
                    mainapp.CashierSalesBillDetails.SavingFrom = "SalesBill";

                    mainapp.CashierSalesBillDetails.TotalTaxAmount = mainapp.CashierReceivableDetails.TotalTaxAmount;
                    mainapp.CashierSalesBillDetails.TotalAmount = mainapp.CashierReceivableDetails.TotalAmount;
                    mainapp.CashierSalesBillDetails.GrandTotal = mainapp.CashierReceivableDetails.GrandTotal;
                    mainapp.CashierSalesBillDetails.Balance = mainapp.CashierReceivableDetails.Balance;
                    mainapp.CashierSalesBillDetails.Payment = mainapp.CashierReceivableDetails.AdvancePayment;
                    mainapp.CashierSalesBillDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierSalesBillDetails.RefundAmount = 0;
                    mainapp.CashierSalesBillDetails.PaymentType = mainapp.CashierReceivableDetails.PaymentType;

                    if (frmcol["SBDiscount"] != "")
                    {
                        mainapp.CashierSalesBillDetails.BillDiscount = Convert.ToDouble(frmcol["SBDiscount"]);
                    }
                    else
                    {
                        mainapp.CashierSalesBillDetails.BillDiscount = 0;
                    }

                    //SAVE CASHIER REFUND AMOUNT 0 IF WE CREATE CASHIER FOR REFUND
                    string salesreturn = TempData["SalesReturn"].ToString();
                    //if sales return is no means we get data from retail bill
                    if (salesreturn == "No")
                    {
                        mainapp.SalesBillDetails = TempData["SalesBillDetails"] as SalesBill;
                        if (frmcol["RefundInRBCashier"] == "Yes")
                        {
                            mainapp.CashierSalesBillDetails.RefundToClient = mainapp.SalesBillDetails.Refund;
                        }
                        mainapp.CashierSalesBillDetails.SalesBillDate = mainapp.SalesBillDetails.Date;
                    }
                    else
                    {
                        mainapp.SalesReturnDetails = TempData["SalesReturnDetails"] as SalesReturn;
                        if (frmcol["RefundInRBCashier"] == "Yes")
                        {
                            mainapp.CashierSalesBillDetails.RefundToClient = mainapp.SalesReturnDetails.Refund;
                        }
                        mainapp.CashierSalesBillDetails.SalesBillDate = mainapp.SalesReturnDetails.SalesReturnDate;
                    }

                    //mainapp.CashierRetailBillDetails.RefundAmount = 0;
                    mainapp.CashierSalesBillDetails.SalesBillDate = mainapp.SalesBillDetails.Date;
                    mainapp.CashierSalesBillDetails.Cash_1000 = mainapp.CashierReceivableDetails.Cash_1000;
                    mainapp.CashierSalesBillDetails.Cash_500 = mainapp.CashierReceivableDetails.Cash_500;
                    mainapp.CashierSalesBillDetails.Cash_100 = mainapp.CashierReceivableDetails.Cash_100;
                    mainapp.CashierSalesBillDetails.Cash_50 = mainapp.CashierReceivableDetails.Cash_50;
                    mainapp.CashierSalesBillDetails.Cash_20 = mainapp.CashierReceivableDetails.Cash_20;
                    mainapp.CashierSalesBillDetails.Cash_10 = mainapp.CashierReceivableDetails.Cash_10;
                    mainapp.CashierSalesBillDetails.Cash_1 = mainapp.CashierReceivableDetails.Cash_1;
                    mainapp.CashierSalesBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                    mainapp.CashierSalesBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                    mainapp.CashierSalesBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                    mainapp.CashierSalesBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                    mainapp.CashierSalesBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                    mainapp.CashierSalesBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                    mainapp.CashierSalesBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                    mainapp.CashierSalesBillDetails.TotalCash = mainapp.CashierReceivableDetails.TotalCash;
                    mainapp.CashierSalesBillDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierSalesBillDetails.CreditCardNo = mainapp.CashierReceivableDetails.CreditCardNo;
                    mainapp.CashierSalesBillDetails.CreditCardAmount = mainapp.CashierReceivableDetails.CreditCardAmount;
                    mainapp.CashierSalesBillDetails.HandoverCreditAmt = mainapp.CashierReceivableDetails.HandoverCreditAmt;
                    mainapp.CashierSalesBillDetails.CreditCardType = mainapp.CashierReceivableDetails.CreditCardType;
                    mainapp.CashierSalesBillDetails.CreditCardBank = mainapp.CashierReceivableDetails.CreditCardBank;
                    mainapp.CashierSalesBillDetails.DebitCardName = mainapp.CashierReceivableDetails.DebitCardName;
                    mainapp.CashierSalesBillDetails.DebitCardNo = mainapp.CashierReceivableDetails.DebitCardNo;
                    mainapp.CashierSalesBillDetails.DebitCardAmount = mainapp.CashierReceivableDetails.DebitCardAmount;
                    mainapp.CashierSalesBillDetails.HandoverDebitAmt = mainapp.CashierReceivableDetails.HandoverDebitAmt;
                    mainapp.CashierSalesBillDetails.DebitCardType = mainapp.CashierReceivableDetails.DebitCardType;
                    mainapp.CashierSalesBillDetails.DebitCardBank = mainapp.CashierReceivableDetails.DebitCardBank;
                    mainapp.CashierSalesBillDetails.ChequeNo = mainapp.CashierReceivableDetails.ChequeNo;
                    mainapp.CashierSalesBillDetails.ChequeAccNo = mainapp.CashierReceivableDetails.ChequeAccNo;
                    mainapp.CashierSalesBillDetails.ChequeAmount = mainapp.CashierReceivableDetails.ChequeAmount;
                    mainapp.CashierSalesBillDetails.HandoverChequeAmt = mainapp.CashierReceivableDetails.HandoverChequeAmt;
                    if (mainapp.CashierSalesBillDetails.ChequeNo != null)
                    {
                        mainapp.CashierSalesBillDetails.ChequeDate = mainapp.CashierReceivableDetails.ChequeDate;
                    }
                    else
                    {
                        mainapp.CashierSalesBillDetails.ChequeDate = null;
                    }
                    mainapp.CashierSalesBillDetails.ChequeBank = mainapp.CashierReceivableDetails.ChequeBank;
                    mainapp.CashierSalesBillDetails.ChequeBranch = mainapp.CashierReceivableDetails.ChequeBranch;

                    //SAVE HANDOVER STATUS
                    if (mainapp.CashierSalesBillDetails.CreditCardAmount == 0 && mainapp.CashierSalesBillDetails.DebitCardAmount == 0 && mainapp.CashierSalesBillDetails.ChequeAmount == 0)
                    {
                        mainapp.CashierSalesBillDetails.HandoverStatus = "InActive";
                    }
                    else
                    {
                        mainapp.CashierSalesBillDetails.HandoverStatus = "Active";
                    }

                    //IF REFUND TO CLIENT IN CASHIER SALES BILL THEN HANDOVER STATUS IS INACTIVE
                    if (frmcol["RefundInSBCashier"] == "Yes")
                    {
                        mainapp.CashierSalesBillDetails.HandoverStatus = "InActive";
                    }

                    //SAVE CREDIT NOTE NO AND CREDIT NOTE AMOUNT
                    if (Convert.ToInt32(frmcol["SBCreditNoteAmtVal"]) != 0)
                    {
                        mainapp.CashierSalesBillDetails.CreditNoteNo = frmcol["CashierSalesBillDetails.CreditNoteNo"];
                        mainapp.CashierSalesBillDetails.CreditNoteAmount = Convert.ToDouble(frmcol["SBCreditNoteAmtVal"]);
                    }
                    else
                    {
                        mainapp.CashierRetailBillDetails.CreditNoteAmount = 0;
                    }

                    //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                    if (username != "SuperAdmin")
                    {
                        mainapp.CashierSalesBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        mainapp.CashierSalesBillDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                    }
                    else
                    {
                        mainapp.CashierSalesBillDetails.ShopCode = "SuperAdmin";
                        mainapp.CashierSalesBillDetails.ShopName = "SuperAdmin";
                    }
                    _CashierSalesBillService.Create(mainapp.CashierSalesBillDetails);

                   
                    //ADD DISCOUNT AMOUNT IN SALES BILL FROM CASHIER SALES BILL
                    var salesbilldata = _SalesBillService.GetDetailsBySalesBillNo(frmcol["BillNo"]);
                    salesbilldata.BillDiscount = salesbilldata.BillDiscount + mainapp.CashierSalesBillDetails.BillDiscount;
                    _SalesBillService.Update(salesbilldata);


                    //SAVE CASHIER SALES BILL ITEM

                    //SAVE CASHIER REFUND AMOUNT 0 IF WE CREATE CASHIER FOR REFUND
                    //if sales return is no means we get data from sales bill
                    if (salesreturn == "No")
                    {
                        mainapp.SalesBillItemList = TempData["SalesBillItemList"] as IEnumerable<SalesBillItem>;
                        foreach (var item in mainapp.SalesBillItemList)
                        {
                            mainapp.CashierSalesBillItemDetails.CashierCode = cashiercode;
                            mainapp.CashierSalesBillItemDetails.SalesBillNo = frmcol["BillNo"];
                            mainapp.CashierSalesBillItemDetails.ItemName = item.ItemName;
                            mainapp.CashierSalesBillItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierSalesBillItemDetails.Description = item.Description;
                            mainapp.CashierSalesBillItemDetails.Material = item.Material;
                            mainapp.CashierSalesBillItemDetails.Color = item.Color;
                            mainapp.CashierSalesBillItemDetails.SellingPrice = Convert.ToDouble(item.SellingPrice);
                            mainapp.CashierSalesBillItemDetails.MRP = Convert.ToDouble(item.MRP);
                            mainapp.CashierSalesBillItemDetails.Quantity = Convert.ToDouble(item.Quantity);
                            mainapp.CashierSalesBillItemDetails.Amount = Convert.ToDouble(item.Amount);
                            mainapp.CashierSalesBillItemDetails.DesignName = item.DesignName;
                            mainapp.CashierSalesBillItemDetails.Unit = item.Unit;
                            mainapp.CashierSalesBillItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                            mainapp.CashierSalesBillItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                            mainapp.CashierSalesBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierSalesBillItemDetails.Status = "Active";
                            mainapp.CashierSalesBillItemDetails.ModifiedOn = DateTime.Now;
                            _CashierSalesBillItemService.Create(mainapp.CashierSalesBillItemDetails);
                        }
                    }
                    else
                    {
                        mainapp.SalesReturnItemList = TempData["SalesReturnItemList"] as IEnumerable<SalesReturnItem>;
                        foreach (var item in mainapp.SalesReturnItemList)
                        {
                            mainapp.CashierSalesBillItemDetails.CashierCode = cashiercode;
                            mainapp.CashierSalesBillItemDetails.SalesBillNo = frmcol["BillNo"];
                            mainapp.CashierSalesBillItemDetails.ItemName = item.ItemName;
                            mainapp.CashierSalesBillItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierSalesBillItemDetails.Description = item.Description;
                            mainapp.CashierSalesBillItemDetails.Material = item.Material;
                            mainapp.CashierSalesBillItemDetails.Color = item.Color;
                            mainapp.CashierSalesBillItemDetails.SellingPrice = Convert.ToDouble(item.SellingPrice);
                            mainapp.CashierSalesBillItemDetails.MRP = Convert.ToDouble(item.MRP);
                            mainapp.CashierSalesBillItemDetails.Quantity = Convert.ToDouble(item.Quantity);
                            mainapp.CashierSalesBillItemDetails.Amount = Convert.ToDouble(item.Amount);
                            mainapp.CashierSalesBillItemDetails.DesignName = item.DesignName;
                            mainapp.CashierSalesBillItemDetails.Unit = item.Unit;
                            mainapp.CashierSalesBillItemDetails.DisInRs = Convert.ToDouble(item.DiscRs);
                            mainapp.CashierSalesBillItemDetails.DisInPer = Convert.ToDouble(item.DiscPer);
                            mainapp.CashierSalesBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierSalesBillItemDetails.Status = "Active";
                            mainapp.CashierSalesBillItemDetails.ModifiedOn = DateTime.Now;
                            _CashierSalesBillItemService.Create(mainapp.CashierSalesBillItemDetails);
                        }
                    }


                    //UPDATE SALES BILL..
                    //ADD ADJUST AMOUNT TO THE ACTUAL SALES BILL AND MINUS INTO THE BALANCE
                    var salesbilldetails = _SalesBillService.GetDetailsBySalesBillNo(frmcol["BillNo"]);

                    salesbilldetails.PaymentReceived = salesbilldetails.PaymentReceived + mainapp.CashierSalesBillDetails.Payment;
                    if (mainapp.CashierSalesBillDetails.CreditCardAmount == null)
                    {
                        if (Convert.ToDouble(salesbilldetails.Balance) != 0)
                        {
                            salesbilldetails.Balance = salesbilldetails.Balance - mainapp.CashierSalesBillDetails.Payment - mainapp.CashierSalesBillDetails.CreditNoteAmount;
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(salesbilldetails.Balance) != 0)
                        {
                            salesbilldetails.Balance = salesbilldetails.Balance - mainapp.CashierSalesBillDetails.Payment - mainapp.CashierSalesBillDetails.BillDiscount;
                        }
                    }

                    //SAVE CREDIT NOTE NO AND CREDIT NOTE AMOUNT IN RETAIL BILL
                    if (Convert.ToInt32(frmcol["SBCreditNoteAmtVal"]) != 0)
                    {
                        salesbilldetails.CreditNoteNo = frmcol["CashierSalesBillDetails.CreditNoteNo"];
                        salesbilldetails.CreditNoteAmount = Convert.ToDouble(frmcol["SBCreditNoteAmtVal"]);
                    }
                    _SalesBillService.Update(salesbilldetails);


                    //IF BALANCE AND REFUND IS 0 THEN CASHIER STATUS AND STATUS IS INACTIVE OF THAT RETAIL BILL
                    var sbilldata = _SalesBillService.GetDetailsBySalesBillNo(frmcol["BillNo"]);
                    if (mainapp.CashierSalesBillDetails.Balance == 0 && mainapp.CashierSalesBillDetails.RefundAmount == 0)
                    {
                        sbilldata.Status = "InActive";
                        sbilldata.CashierStatus = "InActive";
                        sbilldata.Refund = 0;
                        if (frmcol["RefundInSBCashier"] == "Yes")
                        {
                            if (salesreturn == "No")
                            {
                                sbilldata.RefundToClient = mainapp.RetailBillDetails.Refund;
                            }
                            else
                            {
                                sbilldata.RefundToClient = mainapp.SalesReturnDetails.Refund;
                            }
                        }
                        _SalesBillService.Update(sbilldata);
                    }
                    else if (mainapp.CashierSalesBillDetails.Balance == 0)
                    {
                        sbilldata.Status = "InActive";
                        _SalesBillService.Update(sbilldata);
                    }

                    //INACTIVE ADJUSTED CREDIT NOTE
                    if (Convert.ToInt32(frmcol["SBCreditNoteAmtVal"]) != 0)
                    {
                        var creditnotedetails = _SalesBillCreditNoteService.GetCreditNoteDetails(frmcol["CashierSalesBillDetails.CreditNoteNo"]);
                        creditnotedetails.Status = "InActive";
                        _SalesBillCreditNoteService.Update(creditnotedetails);
                    }
                }


                //*********************************** CASHIER SALES BILL SAVE END ***************************************


                // SAVE CASHIER RETAIL BILL
                else if (billno.Contains("RI"))
                {
                   
                    //update previous retail bill in cashier inactive..
                    var cashierretailbilllist = _CashierRetailBillService.GetRowsByRBNo(frmcol["BillNo"]);
                    foreach (var row in cashierretailbilllist)
                    {
                        row.Status = "InActive";
                        _CashierRetailBillService.Update(row);
                    }

                    // SAVE CASHIER SALES BILL
                    mainapp.CashierRetailBillDetails.CashierCode = cashiercode;
                    mainapp.CashierRetailBillDetails.RetailBillNo = frmcol["BillNo"];
                    mainapp.CashierRetailBillDetails.Date = DateTime.Now;
                    mainapp.CashierRetailBillDetails.Status = "Active";
                    mainapp.CashierRetailBillDetails.ModifiedOn = DateTime.Now;

                    mainapp.CashierRetailBillDetails.TotalTaxAmount = mainapp.CashierReceivableDetails.TotalTaxAmount;
                    mainapp.CashierRetailBillDetails.TotalAmount = mainapp.CashierReceivableDetails.TotalAmount;
                    mainapp.CashierRetailBillDetails.GrandTotal = mainapp.CashierReceivableDetails.GrandTotal;
                    mainapp.CashierRetailBillDetails.Balance = mainapp.CashierReceivableDetails.Balance;
                    mainapp.CashierRetailBillDetails.Payment = mainapp.CashierReceivableDetails.AdvancePayment;
                    mainapp.CashierRetailBillDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierRetailBillDetails.PaymentType = mainapp.CashierReceivableDetails.PaymentType;
                    
                    mainapp.CashierRetailBillDetails.ClientName = mainapp.CashierReceivableDetails.ClientName;
                    mainapp.CashierRetailBillDetails.ClientContact = mainapp.CashierReceivableDetails.ClientContact;
                    mainapp.CashierRetailBillDetails.ClientState = mainapp.CashierReceivableDetails.ClientState;
                    mainapp.CashierRetailBillDetails.ClientType = mainapp.CashierReceivableDetails.ClientType;

                    //SAVE CASHIER REFUND AMOUNT 0 IF WE CREATE CASHIER FOR REFUND
                    string salesreturn = TempData["SalesReturn"].ToString();
                    //if sales return is no means we get data from retail bill
                    if (salesreturn == "No")
                    {
                        mainapp.RetailBillDetails = TempData["RetailBillDetails"] as RetailBill;
                        if (frmcol["RefundInRBCashier"] == "Yes")
                        {
                            mainapp.CashierRetailBillDetails.RefundToClient = mainapp.RetailBillDetails.Refund;
                        }
                        mainapp.CashierRetailBillDetails.RetailBillDate = mainapp.RetailBillDetails.Date;
                    }
                    else
                    {
                        mainapp.SalesReturnDetails = TempData["SalesReturnDetails"] as SalesReturn;
                        if (frmcol["RefundInRBCashier"] == "Yes")
                        {
                            mainapp.CashierRetailBillDetails.RefundToClient = mainapp.SalesReturnDetails.Refund;
                        }
                        mainapp.CashierRetailBillDetails.RetailBillDate = mainapp.SalesReturnDetails.SalesReturnDate;
                    }

                    //DECIDE BILL DATE STATUS
                    if (salesreturn == "No")
                    {
                        if (Convert.ToDateTime(mainapp.RetailBillDetails.Date).Date == Convert.ToDateTime(DateTime.Now).Date)
                        {
                            mainapp.CashierRetailBillDetails.BillDateStatus = "Same";
                        }
                        else
                        {
                            mainapp.CashierRetailBillDetails.BillDateStatus = "Different";
                        }
                    }
                    else
                    {
                        var retaildata = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(mainapp.SalesReturnDetails.BillNo);
                        if (Convert.ToDateTime(retaildata.Date).Date == Convert.ToDateTime(DateTime.Now).Date)
                        {
                            mainapp.CashierRetailBillDetails.BillDateStatus = "Same";
                        }
                        else
                        {
                            mainapp.CashierRetailBillDetails.BillDateStatus = "Different";
                        }
                    }

                    mainapp.CashierRetailBillDetails.RefundAmount = 0;
                    mainapp.CashierRetailBillDetails.Cash_1000 = mainapp.CashierReceivableDetails.Cash_1000;
                    mainapp.CashierRetailBillDetails.Cash_500 = mainapp.CashierReceivableDetails.Cash_500;
                    mainapp.CashierRetailBillDetails.Cash_100 = mainapp.CashierReceivableDetails.Cash_100;
                    mainapp.CashierRetailBillDetails.Cash_50 = mainapp.CashierReceivableDetails.Cash_50;
                    mainapp.CashierRetailBillDetails.Cash_20 = mainapp.CashierReceivableDetails.Cash_20;
                    mainapp.CashierRetailBillDetails.Cash_10 = mainapp.CashierReceivableDetails.Cash_10;
                    mainapp.CashierRetailBillDetails.Cash_1 = mainapp.CashierReceivableDetails.Cash_1;
                    mainapp.CashierRetailBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                    mainapp.CashierRetailBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                    mainapp.CashierRetailBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                    mainapp.CashierRetailBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                    mainapp.CashierRetailBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                    mainapp.CashierRetailBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                    mainapp.CashierRetailBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                    mainapp.CashierRetailBillDetails.TotalCash = mainapp.CashierReceivableDetails.TotalCash;
                    mainapp.CashierRetailBillDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierRetailBillDetails.SelectedCard = mainapp.CashierReceivableDetails.SelectedCard;
                    mainapp.CashierRetailBillDetails.CreditCardNo = mainapp.CashierReceivableDetails.CreditCardNo;
                    mainapp.CashierRetailBillDetails.CreditCardAmount = mainapp.CashierReceivableDetails.CreditCardAmount;
                    mainapp.CashierRetailBillDetails.HandoverCreditAmt = mainapp.CashierReceivableDetails.HandoverCreditAmt;
                    mainapp.CashierRetailBillDetails.CreditCardType = mainapp.CashierReceivableDetails.CreditCardType;
                    mainapp.CashierRetailBillDetails.CreditCardBank = mainapp.CashierReceivableDetails.CreditCardBank;
                    mainapp.CashierRetailBillDetails.DebitCardName = mainapp.CashierReceivableDetails.DebitCardName;
                    mainapp.CashierRetailBillDetails.DebitCardNo = mainapp.CashierReceivableDetails.DebitCardNo;
                    mainapp.CashierRetailBillDetails.DebitCardAmount = mainapp.CashierReceivableDetails.DebitCardAmount;
                    mainapp.CashierRetailBillDetails.HandoverDebitAmt = mainapp.CashierReceivableDetails.HandoverDebitAmt;
                    mainapp.CashierRetailBillDetails.DebitCardType = mainapp.CashierReceivableDetails.DebitCardType;
                    mainapp.CashierRetailBillDetails.DebitCardBank = mainapp.CashierReceivableDetails.DebitCardBank;
                    mainapp.CashierRetailBillDetails.ChequeNo = mainapp.CashierReceivableDetails.ChequeNo;
                    mainapp.CashierRetailBillDetails.ChequeAccNo = mainapp.CashierReceivableDetails.ChequeAccNo;
                    mainapp.CashierRetailBillDetails.ChequeAmount = mainapp.CashierReceivableDetails.ChequeAmount;
                    mainapp.CashierRetailBillDetails.HandoverChequeAmt = mainapp.CashierReceivableDetails.HandoverChequeAmt;
                    if (mainapp.CashierRetailBillDetails.ChequeNo != null)
                    {
                        mainapp.CashierRetailBillDetails.ChequeDate = mainapp.CashierReceivableDetails.ChequeDate;
                    }
                    else
                    {
                        mainapp.CashierRetailBillDetails.ChequeDate = null;
                    }
                    mainapp.CashierRetailBillDetails.ChequeBank = mainapp.CashierReceivableDetails.ChequeBank;
                    mainapp.CashierRetailBillDetails.ChequeBranch = mainapp.CashierReceivableDetails.ChequeBranch;

                    //SAVE HANDOVER STATUS
                    if (mainapp.CashierRetailBillDetails.CreditCardAmount == 0 && mainapp.CashierRetailBillDetails.DebitCardAmount == 0 && mainapp.CashierRetailBillDetails.ChequeAmount == 0)
                    {
                        mainapp.CashierRetailBillDetails.HandoverStatus = "InActive";
                    }
                    else
                    {
                        mainapp.CashierRetailBillDetails.HandoverStatus = "Active";
                    }

                    //IF REFUND TO CLIENT IN CASHIER RETAIL BILL THEN HANDOVER STATUS IS INACTIVE
                    if (frmcol["RefundInRBCashier"] == "Yes")
                    {
                        mainapp.CashierRetailBillDetails.HandoverStatus = "InActive";
                    }

                    //SAVE BILL BALANCE VALUE
                    mainapp.CashierRetailBillDetails.BillBalance = frmcol["BillBalanceVal"];

                    //SAVE CREDIT NOTE NO AND CREDIT NOTE AMOUNT
                    if (Convert.ToInt32(frmcol["RBCreditNoteAmtVal"]) != 0)
                    {
                        mainapp.CashierRetailBillDetails.CreditNoteNo = frmcol["CashierRetailBillDetails.CreditNoteNo"];
                        mainapp.CashierRetailBillDetails.CreditNoteAmount = Convert.ToDouble(frmcol["RBCreditNoteAmtVal"]);
                    }
                    else
                    {
                        mainapp.CashierRetailBillDetails.CreditNoteAmount = 0;
                    }

                    //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                    if (username != "SuperAdmin")
                    {
                        mainapp.CashierRetailBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        mainapp.CashierRetailBillDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                    }
                    else
                    {
                        mainapp.CashierRetailBillDetails.ShopCode = "SuperAdmin";
                        mainapp.CashierRetailBillDetails.ShopName = "SuperAdmin";
                    }

                    //SAVE ADJUST AMOUNT AND ADJUSTED BILL
                    var retailinvoicedata = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                    mainapp.CashierRetailBillDetails.AdjustedAmount = retailinvoicedata.AdjustedAmount;
                    mainapp.CashierRetailBillDetails.AdjustedBill = retailinvoicedata.AdjustedBill;

                    //save total advance payment
                    var prevcashierretailbills = _CashierRetailBillService.GetRowsByRBNo(frmcol["BillNo"]);
                    if (prevcashierretailbills.Count() != 0)
                    {
                        double ttladvpay = 0;
                        foreach (var data in prevcashierretailbills)
                        {
                            ttladvpay = Convert.ToDouble(ttladvpay) + Convert.ToDouble(data.Payment);
                        }
                        ttladvpay = ttladvpay + Convert.ToDouble(mainapp.CashierRetailBillDetails.Payment);
                        mainapp.CashierRetailBillDetails.TotalAdvancePayment = ttladvpay;
                    }
                    else
                    {
                        mainapp.CashierRetailBillDetails.TotalAdvancePayment = mainapp.CashierRetailBillDetails.Payment;
                    }

                    _CashierRetailBillService.Create(mainapp.CashierRetailBillDetails);

                    //SAVE CASHIER RETAIL BILL ITEM

                    //SAVE CASHIER REFUND AMOUNT 0 IF WE CREATE CASHIER FOR REFUND
                    //if sales return is no means we get data from retail bill
                    if (salesreturn == "No")
                    {
                        mainapp.RetailBillItemList = TempData["RetailBillItemList"] as IEnumerable<RetailBillItem>;
                        foreach (var item in mainapp.RetailBillItemList)
                        {
                            mainapp.CashierRetailBillItemDetails.CashierCode = cashiercode;
                            mainapp.CashierRetailBillItemDetails.RetailInvoiceNo = frmcol["BillNo"];
                            mainapp.CashierRetailBillItemDetails.ItemName = item.ItemName;
                            mainapp.CashierRetailBillItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierRetailBillItemDetails.Description = item.Description;
                            mainapp.CashierRetailBillItemDetails.Material = item.Material;
                            mainapp.CashierRetailBillItemDetails.Color = item.Color;
                            mainapp.CashierRetailBillItemDetails.SellingPrice = Convert.ToDouble(item.SellingPrice);
                            mainapp.CashierRetailBillItemDetails.MRP = Convert.ToDouble(item.MRP);
                            mainapp.CashierRetailBillItemDetails.Quantity = Convert.ToDouble(item.Quantity);
                            mainapp.CashierRetailBillItemDetails.Amount = Convert.ToDouble(item.Amount);
                            mainapp.CashierRetailBillItemDetails.DesignName = item.DesignName;
                            mainapp.CashierRetailBillItemDetails.DesignCode = item.DesignCode;
                            mainapp.CashierRetailBillItemDetails.Unit = item.Unit;
                            mainapp.CashierRetailBillItemDetails.Size = item.Size;
                            mainapp.CashierRetailBillItemDetails.Barcode = item.Barcode;
                            mainapp.CashierRetailBillItemDetails.Brand = item.Brand;
                            mainapp.CashierRetailBillItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                            mainapp.CashierRetailBillItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                            mainapp.CashierRetailBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierRetailBillItemDetails.Status = "Active";
                            mainapp.CashierRetailBillItemDetails.ModifiedOn = DateTime.Now;
                            _CashierRetailBillItemService.Create(mainapp.CashierRetailBillItemDetails);
                        }
                    }
                    else
                    {
                        mainapp.SalesReturnItemList = TempData["SalesReturnItemList"] as IEnumerable<SalesReturnItem>;
                        foreach (var item in mainapp.SalesReturnItemList)
                        {
                            mainapp.CashierRetailBillItemDetails.CashierCode = cashiercode;
                            mainapp.CashierRetailBillItemDetails.RetailInvoiceNo = frmcol["BillNo"];
                            mainapp.CashierRetailBillItemDetails.ItemName = item.ItemName;
                            mainapp.CashierRetailBillItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierRetailBillItemDetails.Description = item.Description;
                            mainapp.CashierRetailBillItemDetails.Material = item.Material;
                            mainapp.CashierRetailBillItemDetails.Color = item.Color;
                            mainapp.CashierRetailBillItemDetails.SellingPrice = Convert.ToDouble(item.SellingPrice);
                            mainapp.CashierRetailBillItemDetails.MRP = Convert.ToDouble(item.MRP);
                            mainapp.CashierRetailBillItemDetails.Quantity = Convert.ToDouble(item.Quantity);
                            mainapp.CashierRetailBillItemDetails.Amount = Convert.ToDouble(item.Amount);
                            mainapp.CashierRetailBillItemDetails.DesignName = item.DesignName;
                            mainapp.CashierRetailBillItemDetails.DesignCode = item.DesignCode;
                            mainapp.CashierRetailBillItemDetails.Unit = item.Unit;
                            mainapp.CashierRetailBillItemDetails.Size = item.Size;
                            mainapp.CashierRetailBillItemDetails.Barcode = item.Barcode;
                            mainapp.CashierRetailBillItemDetails.Brand = item.Brand;
                            mainapp.CashierRetailBillItemDetails.DisInRs = Convert.ToDouble(item.DiscRs);
                            mainapp.CashierRetailBillItemDetails.DisInPer = Convert.ToDouble(item.DiscPer);
                            mainapp.CashierRetailBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierRetailBillItemDetails.Status = "Active";
                            mainapp.CashierRetailBillItemDetails.ModifiedOn = DateTime.Now;
                            _CashierRetailBillItemService.Create(mainapp.CashierRetailBillItemDetails);
                        }
                    }

                    //UPDATE RETAIL BILL..
                    //ADD ADJUST AMOUNT TO THE ACTUAL RETAIL BILL AND MINUS INTO THE BALANCE
                    var retailbilldata = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                    retailbilldata.Payment = retailbilldata.Payment + mainapp.CashierRetailBillDetails.Payment;
                    if (Convert.ToDouble(retailbilldata.Balance) != 0)
                    {
                        retailbilldata.Balance = retailbilldata.Balance - mainapp.CashierRetailBillDetails.Payment - mainapp.CashierRetailBillDetails.CreditNoteAmount;
                    }
                    retailbilldata.BillBalance = frmcol["BillBalanceVal"];
                    //SAVE CREDIT NOTE NO AND CREDIT NOTE AMOUNT IN RETAIL BILL
                    if (Convert.ToInt32(frmcol["RBCreditNoteAmtVal"]) != 0)
                    {
                        retailbilldata.CreditNoteNo = frmcol["CashierRetailBillDetails.CreditNoteNo"];
                        retailbilldata.CreditNoteAmount = Convert.ToDouble(frmcol["RBCreditNoteAmtVal"]);
                    }
                    _RetailInvoiceMasterService.Update(retailbilldata);

                    //IF BALANCE AND REFUND IS 0 THEN CASHIER STATUS AND STATUS IS INACTIVE OF THAT RETAIL BILL
                    var retailbilldetails = _RetailInvoiceMasterService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                    if (mainapp.CashierRetailBillDetails.Balance == 0 && mainapp.CashierRetailBillDetails.RefundAmount == 0)
                    {
                        retailbilldetails.Status = "InActive";
                        retailbilldetails.CashierStatus = "InActive";
                        retailbilldetails.Refund = 0;
                        if (frmcol["RefundInRBCashier"] == "Yes")
                        {
                            if (salesreturn == "No")
                            {
                                retailbilldetails.RefundToClient = mainapp.RetailBillDetails.Refund;
                            }
                            else
                            {
                                retailbilldetails.RefundToClient = mainapp.SalesReturnDetails.Refund;
                            }
                        }
                        _RetailInvoiceMasterService.Update(retailbilldetails);
                    }
                    else if (mainapp.CashierRetailBillDetails.Balance == 0)
                    {
                        retailbilldetails.Status = "InActive";
                        _RetailInvoiceMasterService.Update(retailbilldetails);
                    }

                    //INACTIVE ADJUSTED CREDIT NOTE
                    if (Convert.ToInt32(frmcol["RBCreditNoteAmtVal"]) != 0)
                    {
                        var creditnotedetails = _RetailBillCreditNoteService.GetCreditNoteDetails(frmcol["CashierRetailBillDetails.CreditNoteNo"]);
                        creditnotedetails.Status = "InActive";
                        _RetailBillCreditNoteService.Update(creditnotedetails);
                    }
                }
                //*********************************** CASHIER RETAIL BILL SAVE END ***************************************

                 // SAVE CASHIER TEMPORARY CASH MEMO
                else if (billno.Contains("PCM"))
                {
                    //update previous temporary cash memo in cashier inactive..
                    var cashiertemporarycashmemolist = _CashierTemporaryCashMemoService.GetRowsByRBNo(frmcol["BillNo"]);
                    foreach (var row in cashiertemporarycashmemolist)
                    {
                        row.Status = "InActive";
                        _CashierTemporaryCashMemoService.Update(row);
                    }

                    // SAVE CASHIER TEMPORARY CASH MEMO
                    mainapp.CashierTemporaryCashMemoDetails.CashierCode = cashiercode;
                    mainapp.CashierTemporaryCashMemoDetails.TempCashMemoNo = frmcol["BillNo"];
                    mainapp.CashierTemporaryCashMemoDetails.Date = DateTime.Now;
                    mainapp.CashierTemporaryCashMemoDetails.Status = "Active";
                    mainapp.CashierTemporaryCashMemoDetails.ModifiedOn = DateTime.Now;

                    mainapp.CashierTemporaryCashMemoDetails.TotalTaxAmount = mainapp.CashierReceivableDetails.TotalTaxAmount;
                    mainapp.CashierTemporaryCashMemoDetails.TotalAmount = mainapp.CashierReceivableDetails.TotalAmount;
                    mainapp.CashierTemporaryCashMemoDetails.GrandTotal = mainapp.CashierReceivableDetails.GrandTotal;
                    mainapp.CashierTemporaryCashMemoDetails.Balance = mainapp.CashierReceivableDetails.Balance;
                    mainapp.CashierTemporaryCashMemoDetails.Payment = mainapp.CashierReceivableDetails.AdvancePayment;
                    mainapp.CashierTemporaryCashMemoDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierTemporaryCashMemoDetails.PaymentType = mainapp.CashierReceivableDetails.PaymentType;

                    mainapp.CashierTemporaryCashMemoDetails.ClientName = mainapp.CashierReceivableDetails.ClientName;
                    mainapp.CashierTemporaryCashMemoDetails.ClientContact = mainapp.CashierReceivableDetails.ClientContact;
                    mainapp.CashierTemporaryCashMemoDetails.ClientState = mainapp.CashierReceivableDetails.ClientState;
                    mainapp.CashierTemporaryCashMemoDetails.ClientType = mainapp.CashierReceivableDetails.ClientType;

                    //SAVE CASHIER REFUND AMOUNT 0 IF WE CREATE CASHIER FOR REFUND
                    mainapp.TemporaryCashMemoDetails = TempData["TemporaryCashMemoDetails"] as TemporaryCashMemo;
                    if (frmcol["RefundInTCMCashier"] == "Yes")
                    {
                        mainapp.CashierTemporaryCashMemoDetails.RefundToClient = mainapp.TemporaryCashMemoDetails.Refund;
                    }
                    mainapp.CashierTemporaryCashMemoDetails.TempCashMemoDate = mainapp.TemporaryCashMemoDetails.Date;


                    //DECIDE BILL DATE STATUS
                    if (Convert.ToDateTime(mainapp.TemporaryCashMemoDetails.Date).Date == Convert.ToDateTime(DateTime.Now).Date)
                    {
                        mainapp.CashierTemporaryCashMemoDetails.BillDateStatus = "Same";
                    }
                    else
                    {
                        mainapp.CashierTemporaryCashMemoDetails.BillDateStatus = "Different";
                    }

                    mainapp.CashierTemporaryCashMemoDetails.RefundAmount = 0;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_1000 = mainapp.CashierReceivableDetails.Cash_1000;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_500 = mainapp.CashierReceivableDetails.Cash_500;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_100 = mainapp.CashierReceivableDetails.Cash_100;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_50 = mainapp.CashierReceivableDetails.Cash_50;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_20 = mainapp.CashierReceivableDetails.Cash_20;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_10 = mainapp.CashierReceivableDetails.Cash_10;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_1 = mainapp.CashierReceivableDetails.Cash_1;
                    mainapp.CashierTemporaryCashMemoDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                    mainapp.CashierTemporaryCashMemoDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                    mainapp.CashierTemporaryCashMemoDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                    mainapp.CashierTemporaryCashMemoDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                    mainapp.CashierTemporaryCashMemoDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                    mainapp.CashierTemporaryCashMemoDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                    mainapp.CashierTemporaryCashMemoDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                    mainapp.CashierTemporaryCashMemoDetails.TotalCash = mainapp.CashierReceivableDetails.TotalCash;
                    mainapp.CashierTemporaryCashMemoDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                    mainapp.CashierTemporaryCashMemoDetails.SelectedCard = mainapp.CashierReceivableDetails.SelectedCard;
                    mainapp.CashierTemporaryCashMemoDetails.CreditCardNo = mainapp.CashierReceivableDetails.CreditCardNo;
                    mainapp.CashierTemporaryCashMemoDetails.CreditCardAmount = mainapp.CashierReceivableDetails.CreditCardAmount;
                    mainapp.CashierTemporaryCashMemoDetails.HandoverCreditAmt = mainapp.CashierReceivableDetails.HandoverCreditAmt;
                    mainapp.CashierTemporaryCashMemoDetails.CreditCardType = mainapp.CashierReceivableDetails.CreditCardType;
                    mainapp.CashierTemporaryCashMemoDetails.CreditCardBank = mainapp.CashierReceivableDetails.CreditCardBank;
                    mainapp.CashierTemporaryCashMemoDetails.DebitCardName = mainapp.CashierReceivableDetails.DebitCardName;
                    mainapp.CashierTemporaryCashMemoDetails.DebitCardNo = mainapp.CashierReceivableDetails.DebitCardNo;
                    mainapp.CashierTemporaryCashMemoDetails.DebitCardAmount = mainapp.CashierReceivableDetails.DebitCardAmount;
                    mainapp.CashierTemporaryCashMemoDetails.HandoverDebitAmt = mainapp.CashierReceivableDetails.HandoverDebitAmt;
                    mainapp.CashierTemporaryCashMemoDetails.DebitCardType = mainapp.CashierReceivableDetails.DebitCardType;
                    mainapp.CashierTemporaryCashMemoDetails.DebitCardBank = mainapp.CashierReceivableDetails.DebitCardBank;
                    mainapp.CashierTemporaryCashMemoDetails.ChequeNo = mainapp.CashierReceivableDetails.ChequeNo;
                    mainapp.CashierTemporaryCashMemoDetails.ChequeAccNo = mainapp.CashierReceivableDetails.ChequeAccNo;
                    mainapp.CashierTemporaryCashMemoDetails.ChequeAmount = mainapp.CashierReceivableDetails.ChequeAmount;
                    mainapp.CashierTemporaryCashMemoDetails.HandoverChequeAmt = mainapp.CashierReceivableDetails.HandoverChequeAmt;
                    if (mainapp.CashierTemporaryCashMemoDetails.ChequeNo != null)
                    {
                        mainapp.CashierTemporaryCashMemoDetails.ChequeDate = mainapp.CashierReceivableDetails.ChequeDate;
                    }
                    else
                    {
                        mainapp.CashierTemporaryCashMemoDetails.ChequeDate = null;
                    }
                    mainapp.CashierTemporaryCashMemoDetails.ChequeBank = mainapp.CashierReceivableDetails.ChequeBank;
                    mainapp.CashierTemporaryCashMemoDetails.ChequeBranch = mainapp.CashierReceivableDetails.ChequeBranch;

                    //SAVE HANDOVER STATUS
                    if (mainapp.CashierTemporaryCashMemoDetails.CreditCardAmount == 0 && mainapp.CashierTemporaryCashMemoDetails.DebitCardAmount == 0 && mainapp.CashierTemporaryCashMemoDetails.ChequeAmount == 0)
                    {
                        mainapp.CashierTemporaryCashMemoDetails.HandoverStatus = "InActive";
                    }
                    else
                    {
                        mainapp.CashierTemporaryCashMemoDetails.HandoverStatus = "Active";
                    }

                    //IF REFUND TO CLIENT IN CASHIER TEMPORARY CASH MEMO THEN HANDOVER STATUS IS INACTIVE
                    if (frmcol["RefundInTCMCashier"] == "Yes")
                    {
                        mainapp.CashierTemporaryCashMemoDetails.HandoverStatus = "InActive";
                    }

                    //SAVE BILL BALANCE VALUE
                    mainapp.CashierTemporaryCashMemoDetails.BillBalance = frmcol["BillBalanceVal"];

                    //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                    if (username != "SuperAdmin")
                    {
                        mainapp.CashierTemporaryCashMemoDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                        mainapp.CashierTemporaryCashMemoDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                    }
                    else
                    {
                        mainapp.CashierTemporaryCashMemoDetails.ShopCode = "SuperAdmin";
                        mainapp.CashierTemporaryCashMemoDetails.ShopName = "SuperAdmin";
                    }
                    _CashierTemporaryCashMemoService.Create(mainapp.CashierTemporaryCashMemoDetails);

                    //SAVE CASHIER SALES BILL ITEM
                    //SAVE CASHIER REFUND AMOUNT 0 IF WE CREATE CASHIER FOR REFUND
                    mainapp.TemporaryCashMemoItemList = TempData["TemporaryCashMemoItemList"] as IEnumerable<TemporaryCashMemoItem>;
                    foreach (var item in mainapp.TemporaryCashMemoItemList)
                    {
                        mainapp.CashierTemporaryCashMemoItemDetails.CashierCode = cashiercode;
                        mainapp.CashierTemporaryCashMemoItemDetails.TempCashMemoNo = frmcol["BillNo"];
                        mainapp.CashierTemporaryCashMemoItemDetails.ItemName = item.ItemName;
                        mainapp.CashierTemporaryCashMemoItemDetails.ItemCode = item.ItemCode;
                        mainapp.CashierTemporaryCashMemoItemDetails.Description = item.Description;
                        mainapp.CashierTemporaryCashMemoItemDetails.Material = item.Material;
                        mainapp.CashierTemporaryCashMemoItemDetails.Color = item.Color;
                        mainapp.CashierTemporaryCashMemoItemDetails.SellingPrice = Convert.ToDouble(item.SellingPrice);
                        mainapp.CashierTemporaryCashMemoItemDetails.MRP = Convert.ToDouble(item.MRP);
                        mainapp.CashierTemporaryCashMemoItemDetails.Quantity = Convert.ToDouble(item.Quantity);
                        mainapp.CashierTemporaryCashMemoItemDetails.Amount = Convert.ToDouble(item.Amount);
                        mainapp.CashierTemporaryCashMemoItemDetails.DesignName = item.DesignName;
                        mainapp.CashierTemporaryCashMemoItemDetails.DesignCode = item.DesignCode;
                        mainapp.CashierTemporaryCashMemoItemDetails.Unit = item.Unit;
                        mainapp.CashierTemporaryCashMemoItemDetails.Size = item.Size;
                        mainapp.CashierTemporaryCashMemoItemDetails.Barcode = item.Barcode;
                        mainapp.CashierTemporaryCashMemoItemDetails.Brand = item.Brand;
                        mainapp.CashierTemporaryCashMemoItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                        mainapp.CashierTemporaryCashMemoItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                        mainapp.CashierTemporaryCashMemoItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                        mainapp.CashierTemporaryCashMemoItemDetails.Status = "Active";
                        mainapp.CashierTemporaryCashMemoItemDetails.ModifiedOn = DateTime.Now;
                        _CashierTemporaryCashMemoItemService.Create(mainapp.CashierTemporaryCashMemoItemDetails);
                    }

                    //UPDATE RETAIL BILL..
                    //ADD ADJUST AMOUNT TO THE ACTUAL TEMPORARY CASH MEMO AND MINUS INTO THE BALANCE
                    var temporarycashmemodata = _TemporaryCashMemoService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                    temporarycashmemodata.Payment = temporarycashmemodata.Payment + mainapp.CashierTemporaryCashMemoDetails.Payment;
                    if (Convert.ToDouble(temporarycashmemodata.Balance) != 0)
                    {
                        temporarycashmemodata.Balance = temporarycashmemodata.Balance - mainapp.CashierTemporaryCashMemoDetails.Payment;
                    }
                    temporarycashmemodata.BillBalance = frmcol["BillBalanceVal"];
                    _TemporaryCashMemoService.Update(temporarycashmemodata);

                    //IF BALANCE AND REFUND IS 0 THEN CASHIER STATUS AND STATUS IS INACTIVE OF THAT TEMPORARY CASH MEMO
                    var temporarycashmemodetails = _TemporaryCashMemoService.GetDetailsByInvoiceNo(frmcol["BillNo"]);
                    if (mainapp.CashierTemporaryCashMemoDetails.Balance == 0 && mainapp.CashierTemporaryCashMemoDetails.RefundAmount == 0)
                    {
                        temporarycashmemodetails.Status = "InActive";
                        temporarycashmemodetails.CashierStatus = "InActive";
                        temporarycashmemodetails.Refund = 0;
                        if (frmcol["RefundInTCMCashier"] == "Yes")
                        {
                            temporarycashmemodetails.RefundToClient = mainapp.TemporaryCashMemoDetails.Refund;
                        }
                        _TemporaryCashMemoService.Update(temporarycashmemodetails);
                    }
                    else if (mainapp.CashierTemporaryCashMemoDetails.Balance == 0)
                    {
                        temporarycashmemodetails.Status = "InActive";
                        _TemporaryCashMemoService.Update(temporarycashmemodetails);
                    }
                }
                //*********************************** CASHIER TEMPORARY CASH MEMO SAVE END ***************************************
                return RedirectToAction("CashierModule");
            }

            // SAVE CASHIER MULTIPLE SALES BILL IN CASHIER SALES BILL
            else if (frmcol["BillType"] == "Multiple Sales Bill")
            {
                //CREATE CASHIER RECEIVABLE CODE
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                string cashiercode = string.Empty;

                var cashierdata = _CashierReceivableService.GetLastCashierByFinYr(FinYr);
                int CashierVal = 0;
                int length = 0;
                if (cashierdata != null)
                {
                    cashiercode = cashierdata.CashierCode.Substring(3, 6);
                    length = (Convert.ToInt32(cashiercode) + 1).ToString().Length;
                    CashierVal = Convert.ToInt32(cashiercode) + 1;
                }
                else
                {
                    CashierVal = 1;
                    length = 1;
                }
                cashiercode = _utilityservice.getName("CHR", length, CashierVal);
                cashiercode = cashiercode + FinYr;

                //SAVE CASHIER MUILTIPLE SALES BILL IN CASHIER RECEIVABLES
                mainapp.CashierReceivableDetails.CashierCode = cashiercode;
                mainapp.CashierReceivableDetails.BillType = "MultipleSalesBill";
                mainapp.CashierReceivableDetails.Date = DateTime.Now;
                mainapp.CashierReceivableDetails.Status = "Active";
                mainapp.CashierReceivableDetails.ModifiedOn = DateTime.Now;
                mainapp.CashierReceivableDetails.AdvancePayment = Convert.ToDouble(frmcol["TotalPaidAmountVal"]);

                mainapp.CashierReceivableDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                mainapp.CashierReceivableDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                mainapp.CashierReceivableDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                mainapp.CashierReceivableDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                mainapp.CashierReceivableDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                mainapp.CashierReceivableDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                mainapp.CashierReceivableDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                mainapp.CashierReceivableDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                mainapp.CashierReceivableDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                mainapp.CashierReceivableDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                mainapp.CashierReceivableDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                mainapp.CashierReceivableDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                mainapp.CashierReceivableDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                mainapp.CashierReceivableDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                mainapp.CashierReceivableDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                mainapp.CashierReceivableDetails.SelectedCard = frmcol["Card"];
                mainapp.CashierReceivableDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    mainapp.CashierReceivableDetails.CreditCardAmount = 0;
                    mainapp.CashierReceivableDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    mainapp.CashierReceivableDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                    mainapp.CashierReceivableDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                mainapp.CashierReceivableDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                mainapp.CashierReceivableDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                mainapp.CashierReceivableDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                mainapp.CashierReceivableDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                mainapp.CashierReceivableDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                mainapp.CashierReceivableDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    mainapp.CashierReceivableDetails.DebitCardAmount = 0;
                    mainapp.CashierReceivableDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    mainapp.CashierReceivableDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                    mainapp.CashierReceivableDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                mainapp.CashierReceivableDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                mainapp.CashierReceivableDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];

                if (frmcol["CashierReceivableDetails.ChequeAmount"] == "")
                {
                    mainapp.CashierReceivableDetails.ChequeAmount = 0;
                    mainapp.CashierReceivableDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    mainapp.CashierReceivableDetails.ChequeAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                    mainapp.CashierReceivableDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["CashierReceivableDetails.ChequeAmount"]);
                }
                if (mainapp.CashierReceivableDetails.ChequeNo != null && mainapp.CashierReceivableDetails.ChequeNo != "")
                {
                    mainapp.CashierReceivableDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    mainapp.CashierReceivableDetails.ChequeDate = null;
                }
                mainapp.CashierReceivableDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];
                mainapp.CashierReceivableDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];

                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                var username = HttpContext.Session["UserName"].ToString();
                if (username != "SuperAdmin")
                {
                    mainapp.CashierReceivableDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierReceivableDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierReceivableDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierReceivableDetails.ShopName = "SuperAdmin";
                }
                _CashierReceivableService.Create(mainapp.CashierReceivableDetails);

                // SAVE CASHIER MULTIPLE SALES BILL IN CASHIER SALES BILL
                mainapp.CashierSalesBillDetails.CashierCode = cashiercode;
                mainapp.CashierSalesBillDetails.Date = DateTime.Now;
                mainapp.CashierSalesBillDetails.Status = "Active";
                mainapp.CashierSalesBillDetails.ModifiedOn = DateTime.Now;
                mainapp.CashierSalesBillDetails.SavingFrom = "MultipleSalesBill";
                mainapp.CashierSalesBillDetails.Payment = Convert.ToDouble(frmcol["TotalPaidAmountVal"]);
                mainapp.CashierSalesBillDetails.BillDiscount = Convert.ToDouble(frmcol["TotalDiscountVal"]);
                mainapp.CashierSalesBillDetails.PaymentType = mainapp.CashierReceivableDetails.PaymentType;

                mainapp.CashierSalesBillDetails.Cash_1000 = mainapp.CashierReceivableDetails.Cash_1000;
                mainapp.CashierSalesBillDetails.Cash_500 = mainapp.CashierReceivableDetails.Cash_500;
                mainapp.CashierSalesBillDetails.Cash_100 = mainapp.CashierReceivableDetails.Cash_100;
                mainapp.CashierSalesBillDetails.Cash_50 = mainapp.CashierReceivableDetails.Cash_50;
                mainapp.CashierSalesBillDetails.Cash_20 = mainapp.CashierReceivableDetails.Cash_20;
                mainapp.CashierSalesBillDetails.Cash_10 = mainapp.CashierReceivableDetails.Cash_10;
                mainapp.CashierSalesBillDetails.Cash_1 = mainapp.CashierReceivableDetails.Cash_1;
                mainapp.CashierSalesBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                mainapp.CashierSalesBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                mainapp.CashierSalesBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                mainapp.CashierSalesBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                mainapp.CashierSalesBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                mainapp.CashierSalesBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                mainapp.CashierSalesBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                mainapp.CashierSalesBillDetails.TotalCash = mainapp.CashierReceivableDetails.TotalCash;
                mainapp.CashierSalesBillDetails.Refund = mainapp.CashierReceivableDetails.Refund;
                mainapp.CashierSalesBillDetails.SelectedCard = mainapp.CashierReceivableDetails.SelectedCard;
                mainapp.CashierSalesBillDetails.CreditCardNo = mainapp.CashierReceivableDetails.CreditCardNo;
                mainapp.CashierSalesBillDetails.CreditCardType = mainapp.CashierReceivableDetails.CreditCardType;
                mainapp.CashierSalesBillDetails.CreditCardAmount = mainapp.CashierReceivableDetails.CreditCardAmount;
                mainapp.CashierSalesBillDetails.HandoverCreditAmt = mainapp.CashierReceivableDetails.HandoverCreditAmt;
                mainapp.CashierSalesBillDetails.CreditCardBank = mainapp.CashierReceivableDetails.CreditCardBank;
                mainapp.CashierSalesBillDetails.DebitCardName = mainapp.CashierReceivableDetails.DebitCardName;
                mainapp.CashierSalesBillDetails.DebitCardNo = mainapp.CashierReceivableDetails.DebitCardNo;
                mainapp.CashierSalesBillDetails.DebitCardAmount = mainapp.CashierReceivableDetails.DebitCardAmount;
                mainapp.CashierSalesBillDetails.HandoverDebitAmt = mainapp.CashierReceivableDetails.HandoverDebitAmt;
                mainapp.CashierSalesBillDetails.DebitCardType = mainapp.CashierReceivableDetails.DebitCardType;
                mainapp.CashierSalesBillDetails.DebitCardBank = mainapp.CashierReceivableDetails.DebitCardBank;
                mainapp.CashierSalesBillDetails.ChequeNo = mainapp.CashierReceivableDetails.ChequeNo;
                mainapp.CashierSalesBillDetails.ChequeAccNo = mainapp.CashierReceivableDetails.ChequeAccNo;
                mainapp.CashierSalesBillDetails.ChequeAmount = mainapp.CashierReceivableDetails.ChequeAmount;
                mainapp.CashierSalesBillDetails.HandoverChequeAmt = mainapp.CashierReceivableDetails.HandoverChequeAmt;
                if (mainapp.CashierSalesBillDetails.ChequeNo != null)
                {
                    mainapp.CashierSalesBillDetails.ChequeDate = mainapp.CashierReceivableDetails.ChequeDate;
                }
                else
                {
                    mainapp.CashierSalesBillDetails.ChequeDate = null;
                }
                mainapp.CashierSalesBillDetails.ChequeBank = mainapp.CashierReceivableDetails.ChequeBank;
                mainapp.CashierSalesBillDetails.ChequeBranch = mainapp.CashierReceivableDetails.ChequeBranch;

                //SAVE HANDOVER STATUS
                if (mainapp.CashierSalesBillDetails.CreditCardAmount == 0 && mainapp.CashierSalesBillDetails.DebitCardAmount == 0 && mainapp.CashierSalesBillDetails.ChequeAmount == 0)
                {
                    mainapp.CashierSalesBillDetails.HandoverStatus = "InActive";
                }
                else
                {
                    mainapp.CashierSalesBillDetails.HandoverStatus = "Active";
                }

                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierSalesBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierSalesBillDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierSalesBillDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierSalesBillDetails.ShopName = "SuperAdmin";
                }
                _CashierSalesBillService.Create(mainapp.CashierSalesBillDetails);

                // SAVE CASHIER SALES BILL ITEM
                int MultipleSBRows = Convert.ToInt32(frmcol["MultipleSBhdnRowCount"]);
                for (int i = 1; i <= MultipleSBRows; i++)
                {
                    string salesbillno = "MultipleSBNo" + i;
                    string payment = "MultipleSBPayment" + i;
                    //save only those sales bills which have transactions
                    if (Convert.ToDouble(frmcol[payment]) != 0)
                    {
                        var salesbillitems = _SalesBillItemService.GetItemsBySalesBillNo(frmcol[salesbillno]);
                        foreach (var item in salesbillitems)
                        {
                            mainapp.CashierSalesBillItemDetails.CashierCode = cashiercode;
                            mainapp.CashierSalesBillItemDetails.SalesBillNo = frmcol[salesbillno];
                            mainapp.CashierSalesBillItemDetails.ItemName = item.ItemName;
                            mainapp.CashierSalesBillItemDetails.ItemCode = item.ItemCode;
                            mainapp.CashierSalesBillItemDetails.Description = item.Description;
                            mainapp.CashierSalesBillItemDetails.Material = item.Material;
                            mainapp.CashierSalesBillItemDetails.Color = item.Color;
                            mainapp.CashierSalesBillItemDetails.SellingPrice = Convert.ToDouble(item.SellingPrice);
                            mainapp.CashierSalesBillItemDetails.MRP = Convert.ToDouble(item.MRP);
                            mainapp.CashierSalesBillItemDetails.Quantity = Convert.ToDouble(item.Quantity);
                            mainapp.CashierSalesBillItemDetails.Amount = Convert.ToDouble(item.Amount);
                            mainapp.CashierSalesBillItemDetails.DesignName = item.DesignName;
                            mainapp.CashierSalesBillItemDetails.Unit = item.Unit;
                            mainapp.CashierSalesBillItemDetails.DisInRs = Convert.ToDouble(item.DiscountRPS);
                            mainapp.CashierSalesBillItemDetails.DisInPer = Convert.ToDouble(item.DiscountPercent);
                            mainapp.CashierSalesBillItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                            mainapp.CashierSalesBillItemDetails.Status = "Active";
                            mainapp.CashierSalesBillItemDetails.ModifiedOn = DateTime.Now;
                            _CashierSalesBillItemService.Create(mainapp.CashierSalesBillItemDetails);
                        }

                        //ADD MULTIPLE SALES BILL CODE IN CASHIER SALES BILL
                        var cashiersalesbilldata = _CashierSalesBillService.GetDetailsByCashierCode(mainapp.CashierSalesBillDetails.CashierCode);
                        if (cashiersalesbilldata.SalesBillNo == null)
                        {
                            cashiersalesbilldata.SalesBillNo = mainapp.CashierSalesBillItemDetails.SalesBillNo;
                        }
                        else
                        {
                            cashiersalesbilldata.SalesBillNo = cashiersalesbilldata.SalesBillNo + "," + mainapp.CashierSalesBillItemDetails.SalesBillNo;
                        }
                    }
                }

                //UPDATE MAIN SALES BILL AFTER MULTIPLE SALES ORDER
                for (int i = 1; i <= MultipleSBRows; i++)
                {
                    string salesbillno = "MultipleSBNo" + i;
                    string payment = "MultipleSBPayment" + i;
                    string discount = "MultipleSBDiscount" + i;
                    string balanceval = "MultipleSBBalanceVal" + i;
                    string refundval = "MultipleSBRefundVal" + i;

                    var salesbilldata = _SalesBillService.GetDetailsBySalesBillNo(frmcol[salesbillno]);
                    salesbilldata.PaymentReceived = salesbilldata.PaymentReceived + Convert.ToDouble(frmcol[payment]);
                    salesbilldata.BillDiscount = salesbilldata.BillDiscount + Convert.ToDouble(frmcol[discount]);
                    salesbilldata.Balance = Convert.ToDouble(frmcol[balanceval]);
                    salesbilldata.Refund = Convert.ToDouble(frmcol[refundval]);

                    //IF BALANCE AND REFUND IS ZERO THAT MEANS INACTIVE THAT SALES BILL CASHIER STATUS
                    if (salesbilldata.Balance == 0 && salesbilldata.Refund == 0)
                    {
                        salesbilldata.CashierStatus = "InActive";
                    }

                    //IF BALANCE IS ZERO THAT MEANS INACTIVE THAT SALES BILL
                    if (salesbilldata.Balance == 0)
                    {
                        salesbilldata.Status = "InActive";
                    }
                    _SalesBillService.Update(salesbilldata);

                    //IF BALANCE IS ZERO THAT MEANS INACTIVE THAT SALES BILL ITEMS
                    if (salesbilldata.Balance == 0)
                    {
                        var salesbillitemdata = _SalesBillItemService.GetItemsBySalesBillNo(frmcol[salesbillno]);
                        foreach (var data in salesbillitemdata)
                        {
                            data.Status = "InActive";
                            _SalesBillItemService.Update(data);
                        }

                    }
                }
                return RedirectToAction("CashierModule");
            }
            //**************************** CASHIER MULTIPLE SALES BILL END *********************************

            //SAVE CASHIER PAYABLE AND ITS BILLS
            //else if (frmcol["BillType"] == "Refund Order")
            else
            {
                //CREATE CASHIER PAYABLE CODE
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                string cashiercode = string.Empty;

                var cashierdata = _CashierPayableService.GetLastCashierByFinYr(FinYr);
                int CashierVal = 0;
                int length = 0;
                if (cashierdata != null)
                {
                    cashiercode = cashierdata.CashierCode.Substring(3, 6);
                    length = (Convert.ToInt32(cashiercode) + 1).ToString().Length;
                    CashierVal = Convert.ToInt32(cashiercode) + 1;
                }
                else
                {
                    CashierVal = 1;
                    length = 1;
                }
                cashiercode = _utilityservice.getName("CHP", length, CashierVal);
                cashiercode = cashiercode + FinYr;

                //SAVE CASHIER PAYABLE PAYMENT DETAILS
                mainapp.CashierPayableDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                mainapp.CashierPayableDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                mainapp.CashierPayableDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                mainapp.CashierPayableDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                mainapp.CashierPayableDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                mainapp.CashierPayableDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                mainapp.CashierPayableDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                mainapp.CashierPayableDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                mainapp.CashierPayableDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                mainapp.CashierPayableDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                mainapp.CashierPayableDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                mainapp.CashierPayableDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                mainapp.CashierPayableDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                mainapp.CashierPayableDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                mainapp.CashierPayableDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                mainapp.CashierPayableDetails.SelectedCard = frmcol["Card"];
                mainapp.CashierPayableDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    mainapp.CashierPayableDetails.CreditCardAmount = 0;
                }
                else
                {
                    mainapp.CashierPayableDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                mainapp.CashierPayableDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                mainapp.CashierPayableDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                mainapp.CashierPayableDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                mainapp.CashierPayableDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                mainapp.CashierPayableDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                mainapp.CashierPayableDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    mainapp.CashierPayableDetails.DebitCardAmount = 0;
                }
                else
                {
                    mainapp.CashierPayableDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                mainapp.CashierPayableDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                mainapp.CashierPayableDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                mainapp.CashierPayableDetails.ChequeAmount = frmcol["CashierReceivableDetails.ChequeAmount"];
                if (frmcol["CashierReceivableDetails.ChequeNo"] == "")
                {
                    mainapp.CashierPayableDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    mainapp.CashierPayableDetails.ChequeDate = null;
                }
                mainapp.CashierPayableDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];
                mainapp.CashierPayableDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];
                mainapp.CashierPayableDetails.CashierCode = cashiercode;
                mainapp.CashierPayableDetails.BillType = frmcol["BillType"];
                mainapp.CashierPayableDetails.Billno = frmcol["BillNo"];
                mainapp.CashierPayableDetails.Date = DateTime.Now;
                mainapp.CashierPayableDetails.Status = "Active";
                mainapp.CashierPayableDetails.ModifiedOn = DateTime.Now;

                var username = HttpContext.Session["UserName"].ToString();
                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierPayableDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierPayableDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierPayableDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierPayableDetails.ShopName = "SuperAdmin";
                }

                //SAVE CASHIER PAYABLE AMOUNT DETAILS USING REFUND ORDER
                if (mainapp.CashierPayableDetails.Billno.Contains("SO"))
                {
                    var receivables = TempData["RefundOrderDetails"] as SalesOrder;
                    mainapp.CashierPayableDetails.ClientName = receivables.ClientName;
                    mainapp.CashierPayableDetails.ClientState = receivables.ClientState;
                    mainapp.CashierPayableDetails.ClientContact = receivables.ClientContactNo;
                    mainapp.CashierPayableDetails.ClientType = receivables.ClientType;
                    mainapp.CashierPayableDetails.ClientFormType = receivables.FormType;
                    mainapp.CashierPayableDetails.TotalAmount = receivables.TotalAmount;
                    mainapp.CashierPayableDetails.PackAndForwd = Convert.ToDouble(receivables.PackAndForwd);
                    mainapp.CashierPayableDetails.GrandTotal = receivables.GrandTotal;
                }
                _CashierPayableService.Create(mainapp.CashierPayableDetails);
                //************************** CASHIER PAYABLE SAVE END ****************************

                // SAVE REFUND ORDER
                mainapp.CashierRefundOrderDetails.Cash_1000 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_1000"]);
                mainapp.CashierRefundOrderDetails.Cash_500 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_500"]);
                mainapp.CashierRefundOrderDetails.Cash_100 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_100"]);
                mainapp.CashierRefundOrderDetails.Cash_50 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_50"]);
                mainapp.CashierRefundOrderDetails.Cash_20 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_20"]);
                mainapp.CashierRefundOrderDetails.Cash_10 = Convert.ToInt32(frmcol["CashierReceivableDetails.Cash_10"]);
                mainapp.CashierRefundOrderDetails.Cash_1 = Convert.ToDouble(frmcol["CashierReceivableDetails.Cash_1"]);
                mainapp.CashierRefundOrderDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                mainapp.CashierRefundOrderDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                mainapp.CashierRefundOrderDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                mainapp.CashierRefundOrderDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                mainapp.CashierRefundOrderDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                mainapp.CashierRefundOrderDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                mainapp.CashierRefundOrderDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                mainapp.CashierRefundOrderDetails.TotalCash = Convert.ToDouble(frmcol["CashierReceivableDetails.TotalCash"]);
                mainapp.CashierRefundOrderDetails.Refund = mainapp.CashierPayableDetails.Refund;
                mainapp.CashierRefundOrderDetails.SelectedCard = frmcol["Card"];
                mainapp.CashierRefundOrderDetails.CreditCardNo = frmcol["CashierReceivableDetails.CreditCardNo"];
                if (frmcol["CashierReceivableDetails.CreditCardAmount"] == "")
                {
                    mainapp.CashierRefundOrderDetails.CreditCardAmount = 0;
                }
                else
                {
                    mainapp.CashierRefundOrderDetails.CreditCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.CreditCardAmount"]);
                }
                mainapp.CashierRefundOrderDetails.CreditCardType = frmcol["CashierReceivableDetails.CreditCardType"];
                mainapp.CashierRefundOrderDetails.CreditCardBank = frmcol["CashierReceivableDetails.CreditCardBank"];
                mainapp.CashierRefundOrderDetails.DebitCardNo = frmcol["CashierReceivableDetails.DebitCardNo"];
                mainapp.CashierRefundOrderDetails.DebitCardName = frmcol["CashierReceivableDetails.DebitCardName"];
                mainapp.CashierRefundOrderDetails.DebitCardType = frmcol["CashierReceivableDetails.DebitCardType"];
                mainapp.CashierRefundOrderDetails.DebitCardBank = frmcol["CashierReceivableDetails.DebitCardBank"];
                if (frmcol["CashierReceivableDetails.DebitCardAmount"] == "")
                {
                    mainapp.CashierRefundOrderDetails.DebitCardAmount = 0;
                }
                else
                {
                    mainapp.CashierRefundOrderDetails.DebitCardAmount = Convert.ToDouble(frmcol["CashierReceivableDetails.DebitCardAmount"]);
                }
                mainapp.CashierRefundOrderDetails.ChequeNo = frmcol["CashierReceivableDetails.ChequeNo"];
                mainapp.CashierRefundOrderDetails.ChequeAccNo = frmcol["CashierReceivableDetails.ChequeAccNo"];
                mainapp.CashierRefundOrderDetails.ChequeAmount = frmcol["CashierReceivableDetails.ChequeAmount"];
                if (frmcol["CashierReceivableDetails.ChequeNo"] == "")
                {
                    mainapp.CashierRefundOrderDetails.ChequeDate = Convert.ToDateTime(frmcol["CashierReceivableDetails.ChequeDate"]);
                }
                else
                {
                    mainapp.CashierRefundOrderDetails.ChequeDate = null;
                }
                mainapp.CashierRefundOrderDetails.ChequeBank = frmcol["CashierReceivableDetails.ChequeBank"];
                mainapp.CashierRefundOrderDetails.ChequeBranch = frmcol["CashierReceivableDetails.ChequeBranch"];
                mainapp.CashierRefundOrderDetails.CashierCode = cashiercode;
                mainapp.CashierRefundOrderDetails.SalesOrderNo = frmcol["BillNo"];
                mainapp.CashierRefundOrderDetails.Date = DateTime.Now;
                mainapp.CashierRefundOrderDetails.Status = "Active";
                mainapp.CashierRefundOrderDetails.ModifiedOn = DateTime.Now;
                mainapp.CashierRefundOrderDetails.ClientName = mainapp.CashierPayableDetails.ClientName;
                mainapp.CashierRefundOrderDetails.ClientState = mainapp.CashierPayableDetails.ClientState;
                mainapp.CashierRefundOrderDetails.ClientContact = mainapp.CashierPayableDetails.ClientContact;
                mainapp.CashierRefundOrderDetails.ClientType = mainapp.CashierPayableDetails.ClientType;
                mainapp.CashierRefundOrderDetails.RefundAmount = mainapp.CashierPayableDetails.RefundAmount;

                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierRefundOrderDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierRefundOrderDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierRefundOrderDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierRefundOrderDetails.ShopName = "SuperAdmin";
                }
                _CashierRefundOrderService.Create(mainapp.CashierRefundOrderDetails);

                //SAVE CASHIER REFUND ORDER ITEM
                mainapp.SalesOrderItemList = TempData["RefundOrderItemList"] as IEnumerable<SalesOrderItem>;
                foreach (var item in mainapp.SalesOrderItemList)
                {
                    mainapp.CashierRefundOrderItemDetails.CashierCode = cashiercode;
                    mainapp.CashierRefundOrderItemDetails.SalesOrderNo = frmcol["BillNo"];
                    mainapp.CashierRefundOrderItemDetails.ItemName = item.ItemName;
                    mainapp.CashierRefundOrderItemDetails.ItemCode = item.ItemCode;
                    mainapp.CashierRefundOrderItemDetails.Description = item.Description;
                    mainapp.CashierRefundOrderItemDetails.Material = item.Material;
                    mainapp.CashierRefundOrderItemDetails.Color = item.Color;
                    mainapp.CashierRefundOrderItemDetails.SellingPrice = item.SellingPrice;
                    mainapp.CashierRefundOrderItemDetails.MRP = item.MRP;
                    mainapp.CashierRefundOrderItemDetails.Quantity = item.Quantity;
                    mainapp.CashierRefundOrderItemDetails.Amount = item.Amount;
                    mainapp.CashierRefundOrderItemDetails.DesignName = item.DesignName;
                    mainapp.CashierRefundOrderItemDetails.Unit = item.Unit;
                    mainapp.CashierRefundOrderItemDetails.DisInRs = Convert.ToDouble(item.DisInRs);
                    mainapp.CashierRefundOrderItemDetails.DisInPer = Convert.ToDouble(item.DisInPercent);
                    mainapp.CashierRefundOrderItemDetails.ItemTax = Convert.ToDouble(item.ItemTax);
                    mainapp.CashierRefundOrderItemDetails.Status = "Active";
                    mainapp.CashierRefundOrderItemDetails.ModifiedOn = DateTime.Now;
                    _CashierRefundsOrderItemService.Create(mainapp.CashierRefundOrderItemDetails);
                }

                //UPDATE SALES ORDER AFTER REFUND AMOUNT..
                var salesorderdata = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol["BillNo"]);
                salesorderdata.RefundToClient = salesorderdata.RefundToClient + mainapp.CashierRefundOrderDetails.RefundAmount;
                salesorderdata.RemainingAdvance = Convert.ToDouble(frmcol["AdvanceBalanceVal"]);
                if (Convert.ToDouble(frmcol["AdvanceBalanceVal"]) == 0)
                {
                    salesorderdata.Status = "InActive";
                }
                _SalesOrderService.Update(salesorderdata);

                //UPDATE CASHIER SALES ORDER STATUS IS INACTIVE
                var CashSOData = _CashierSalesOrderService.GetDetailsBySONoAndStatus(frmcol["BillNo"]);
                CashSOData.Status = "InActive";
                _CashierSalesOrderService.Update(CashSOData);

                return RedirectToAction("CashierModule");
            }
            //**************************** CASHIER REFUND ORDER BILL END *********************************
        }


        ////DETAILS PAGE OF CASHIER RECEIVABLE
        //[HttpGet]
        //public ActionResult CashierReceivableDetails(int Id)
        //{
        //    MainApplication model = new MainApplication();
        //    model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
        //    model.modulelist = _iIModuleService.getAllModules();
        //    model.CompanyCode = CompanyCode;
        //    model.CompanyName = CompanyName;
        //    model.FinancialYear = FinancialYear;
        //    model.CashierReceivableDetails = _CashierReceivableService.GetDetailsById(Id);

        //    var billno = model.CashierReceivableDetails.Billno;

        //    if (billno.Contains("SO"))
        //    {
        //        model.CashierSalesOrderItemList = _CashierSalesOrderItemService.GetDetailsByCashierCode(model.CashierReceivableDetails.CashierCode);
        //    }
        //    else if (billno.Contains("DC"))
        //    {
        //        model.CashierSalesBillItemList = _CashierSalesBillItemService.GetDetailsByCashierCode(model.CashierReceivableDetails.CashierCode);
        //    }
        //    else
        //    {
        //        model.CashierRetailBillItemList = _CashierRetailBillItemService.GetDetailsByCashierCode(model.CashierReceivableDetails.CashierCode);
        //    }

        //    return View(model);
        //}

        ////PRINT PAGE OF CASHIER RECEIVABLES
        //[HttpGet]
        //public ActionResult PrintCashierReceivable(int id)
        //{
        //    MainApplication model = new MainApplication();
        //    model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
        //    model.modulelist = _iIModuleService.getAllModules();
        //    model.CompanyCode = CompanyCode;
        //    model.CompanyName = CompanyName;
        //    model.FinancialYear = FinancialYear;
        //    model.CashierReceivableDetails = _CashierReceivableService.GetDetailsById(id);

        //    var billno = model.CashierReceivableDetails.Billno;

        //    if (billno.Contains("SO"))
        //    {
        //        model.CashierSalesOrderItemList = _CashierSalesOrderItemService.GetDetailsByCashierCode(model.CashierReceivableDetails.CashierCode);
        //    }
        //    else if (billno.Contains("DC"))
        //    {
        //        model.CashierSalesBillItemList = _CashierSalesBillItemService.GetDetailsByCashierCode(model.CashierReceivableDetails.CashierCode);
        //    }
        //    else
        //    {
        //        model.CashierRetailBillItemList = _CashierRetailBillItemService.GetDetailsByCashierCode(model.CashierReceivableDetails.CashierCode);
        //    }
        //    return View(model);
        //}

        // *********************************** CASHIER MODULE UPDATE **************************************

        //CASHIER MODULE UPDATE
        [HttpGet]
        public ActionResult CashierModuleUpdate()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE BILL NO
        [HttpGet]
        public JsonResult GetCashierEditBills(string SearchTerm, string billtype, DateTime Date)
        {
            if (SearchTerm.ToUpper().Contains("S") && billtype == "Sales Order")
            {
                //var data = _CashierSalesOrderService.GetRowsByDate(SearchTerm, Date);
                //List<string> no1 = new List<string>();
                //foreach (var item in data)
                //{
                //    no1.Add(item.OrderNo);
                //}

                //if SO Amount is not adjust in any bill then only update sales order
                var CashSOData = _CashierSalesOrderService.GetRowsByDate(SearchTerm, Date);
                List<string> no1 = new List<string>();
                foreach (var Data in CashSOData)
                {
                    List<SalesOrder> SOList = new List<SalesOrder>();
                    var SOData = _SalesOrderService.GetDetailsBySalesOrderNo(Data.OrderNo);
                    if (SOData.TotalAdvancePayment == SOData.RemainingAdvance)
                    {
                        SOList.Add(SOData);
                       
                        foreach (var item in SOList)
                        {
                            no1.Add(item.OrderNo);
                        }
                    }
                }
                return Json(no1, JsonRequestBehavior.AllowGet);
            }
            else if (SearchTerm.ToUpper().Contains("R") && billtype == "Retail Bill")
            {
                var data = _CashierRetailBillService.GetRowsByDate(SearchTerm, Date);
                List<string> no3 = new List<string>();
                foreach (var item in data)
                {
                    no3.Add(item.RetailBillNo);
                }
                return Json(no3, JsonRequestBehavior.AllowGet);
            }
            else if (SearchTerm.ToUpper().Contains("S") && billtype == "Sales Bill")
            {
                var data = _CashierSalesBillService.GetRowsByDate(SearchTerm, Date);
                List<string> no2 = new List<string>();
                foreach (var item in data)
                {
                    no2.Add(item.SalesBillNo);
                }
                return Json(no2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Error";
                return Json(new { msg, billtype }, JsonRequestBehavior.AllowGet);
            }
        }

        //CHECK PAYMEMT TYPE
        [HttpGet]
        public JsonResult CheckPaymentType(string BillNumber)
        {
            string msg = string.Empty;
            //check payment type in cashier sales order
            if (BillNumber.Contains("SO"))
            {
                var CashSO = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNumber);
                msg = CashSO.PaymentType;
            }
            else if (BillNumber.Contains("RI"))
            {
                var CashRB = _CashierRetailBillService.GetDataByStatusAndRetailBillNo(BillNumber);
                msg = CashRB.PaymentType;
            }
            else if (BillNumber.Contains("SB"))
            {
                var CashSB = _CashierSalesBillService.GetDataByStatusAndSalesBillNo(BillNumber);
                msg = CashSB.PaymentType;
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }

        // ********************************* UPDATE CASHIER SALES ORDER ***********************************

        //DISPLAY BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetEditSalesOrderItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderDetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNumber);
            model.CashierSalesOrderItemList = _CashierSalesOrderItemService.GetDetailsByCashierCode(model.CashierSalesOrderDetails.CashierCode);
            TempData["CashierSODetails"] = model.CashierSalesOrderDetails;
            TempData["CashierSOItemList"] = model.CashierSalesOrderItemList;
            return View(model);
        }

        // PARTIAL VIEW OF CASH PAYMENT DETAILS IN CASHIER EDIT
        public ActionResult CashPaymentUpdateForSO(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderDetails = new CashierSalesOrder();
            model.CashierSalesOrderDetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF CREDITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult CardPaymentUpdateForSO(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderDetails = new CashierSalesOrder();
            model.CashierSalesOrderDetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult ChequePaymentUpdateForSO(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderDetails = new CashierSalesOrder();
            model.CashierSalesOrderDetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult CashCardChequePaymentUpdateForSO(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesOrderDetails = new CashierSalesOrder();
            model.CashierSalesOrderDetails = _CashierSalesOrderService.GetDetailsBySONoAndStatus(BillNo);
            return View(model);
        }

        // ****************************************** UPDATE CASHIER SALES ORDER *********************************************

        //DISPLAY BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetEditRetailBillItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillDetails = _CashierRetailBillService.GetDataByStatusAndRetailBillNo(BillNumber);
            model.CashierRetailBillItemList = _CashierRetailBillItemService.GetDetailsByCashierCode(model.CashierRetailBillDetails.CashierCode);
            TempData["CashierRBDetails"] = model.CashierRetailBillDetails;
            TempData["CashierRBItemList"] = model.CashierRetailBillItemList;
            return View(model);
        }

        // PARTIAL VIEW OF CASH PAYMENT DETAILS IN CASHIER EDIT
        public ActionResult CashPaymentUpdateForRB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillDetails = new CashierRetailBill();
            model.CashierRetailBillDetails = _CashierRetailBillService.GetDataByStatusAndRetailBillNo(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF CREDITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult CardPaymentUpdateForRB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillDetails = new CashierRetailBill();
            model.CashierRetailBillDetails = _CashierRetailBillService.GetDataByStatusAndRetailBillNo(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult ChequePaymentUpdateForRB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillDetails = new CashierRetailBill();
            model.CashierRetailBillDetails = _CashierRetailBillService.GetDataByStatusAndRetailBillNo(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult CashCardChequePaymentUpdateForRB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierRetailBillDetails = new CashierRetailBill();
            model.CashierRetailBillDetails = _CashierRetailBillService.GetDataByStatusAndRetailBillNo(BillNo);
            return View(model);
        }

        // ****************************************** UPDATE CASHIER SALES BILL *********************************************

        //DISPLAY BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetEditSalesBillItemDetails(string BillNumber)
        {
            MainApplication model = new MainApplication();
            model.SalesBillDetails = _SalesBillService.GetDetailsBySalesBillNo(BillNumber);
            model.CashierSalesBillDetails = _CashierSalesBillService.GetDataByStatusAndSalesBillNo(BillNumber);
            model.CashierSalesBillItemList = _CashierSalesBillItemService.GetDetailsByCashierCode(model.CashierSalesBillDetails.CashierCode);
            TempData["CashierSBDetails"] = model.CashierRetailBillDetails;
            TempData["CashierSBItemList"] = model.CashierRetailBillItemList;
            return View(model);
        }

        // PARTIAL VIEW OF CASH PAYMENT DETAILS IN CASHIER EDIT
        public ActionResult CashPaymentUpdateForSB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesBillDetails = new CashierSalesBill();
            model.CashierSalesBillDetails = _CashierSalesBillService.GetDataByStatusAndSalesBillNo(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF CREDITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult CardPaymentUpdateForSB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesBillDetails = new CashierSalesBill();
            model.CashierSalesBillDetails = _CashierSalesBillService.GetDataByStatusAndSalesBillNo(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult ChequePaymentUpdateForSB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesBillDetails = new CashierSalesBill();
            model.CashierSalesBillDetails = _CashierSalesBillService.GetDataByStatusAndSalesBillNo(BillNo);
            return View(model);
        }

        // PARTIAL VIEW OF DEBITCARD PAYMENT DETAILS CASHIER EDIT
        public ActionResult CashCardChequePaymentUpdateForSB(string BillNo)
        {
            MainApplication model = new MainApplication();
            model.CashierSalesBillDetails = new CashierSalesBill();
            model.CashierSalesBillDetails = _CashierSalesBillService.GetDataByStatusAndSalesBillNo(BillNo);
            return View(model);
        }


        // ****** SAVE CASHIER UPDATE *****
        // UPDATE CASHIER MODULE 
        [HttpPost]
        public ActionResult CashierModuleUpdate(MainApplication mainapp, FormCollection frmcol)
        {
            //UPDATE PAYMENT TYPE DETAILS
            if (frmcol["BillType"] == "Sales Order")
            {

                //UPDATE SALES ORDER ADDITIONAL ADVANCE
                if (frmcol["UpdateSOType"] == "AdditionalAdvance")
                {
                    mainapp.CashierSalesOrderDetails.AdvancePayment = Convert.ToDouble(frmcol["AdvancePaymentTxtBox"]);

                    //calculate advance payment
                    double prevadvpay = Convert.ToDouble(frmcol["AdvPayLabelVal"]);
                    double calc = 0;
                    double totaladvpay = 0;
                    calc = Convert.ToDouble(mainapp.CashierSalesOrderDetails.TotalAdvancePayment) - prevadvpay;
                    totaladvpay = calc + Convert.ToDouble(frmcol["AdvancePaymentTxtBox"]);
                    mainapp.CashierSalesOrderDetails.TotalAdvancePayment = totaladvpay;

                    //calculate balance
                    var balance = mainapp.CashierSalesOrderDetails.GrandTotal - mainapp.CashierSalesOrderDetails.TotalAdvancePayment;
                    if (balance >= 0)
                    {
                        mainapp.CashierSalesOrderDetails.Balance = balance;
                    }
                    else
                    {
                        mainapp.CashierSalesOrderDetails.Balance = 0;
                    }

                    //UPDATE SALES ORDER TOTAL ADVANCE PAYMENT AFTER CASHIER ENTRY
                    var SOData = _SalesOrderService.GetDetailsBySalesOrderNo(frmcol["BillNo"]);
                    SOData.TotalAdvancePayment = mainapp.CashierSalesOrderDetails.TotalAdvancePayment;
                    SOData.RemainingAdvance = mainapp.CashierSalesOrderDetails.TotalAdvancePayment;
                    _SalesOrderService.Update(SOData);
                }


                mainapp.CashierSalesOrderDetails.PaymentType = frmcol["PaymentType"];
                mainapp.CashierSalesOrderDetails.OrderNo = frmcol["BillNo"];
                mainapp.CashierSalesOrderDetails.Status = "Active";
                mainapp.CashierSalesOrderDetails.ModifiedOn = DateTime.Now;

                if (frmcol["Amt1"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_1000_Amt = 0;
                }
                if (frmcol["Amt2"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_500_Amt = 0;
                }
                if (frmcol["Amt3"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_100_Amt = 0;
                }
                if (frmcol["Amt4"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_50_Amt = 0;
                }
                if (frmcol["Amt5"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_20_Amt = 0;
                }
                if (frmcol["Amt6"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_10_Amt = 0;
                }
                if (frmcol["Amt7"] != "")
                {
                    mainapp.CashierSalesOrderDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.Cash_1_Amt = 0;
                }
                if (mainapp.CashierSalesOrderDetails.TotalCash == null)
                {
                    mainapp.CashierSalesOrderDetails.TotalCash = 0;
                }

                if (mainapp.CashierSalesOrderDetails.PaymentType == "Card" || mainapp.CashierSalesOrderDetails.PaymentType == "CashCardCheque")
                {
                    mainapp.CashierSalesOrderDetails.SelectedCard = frmcol["Card"];
                }

                //SAVE HANDOVER AMOUNT AFTER EDIT SALES ORDER
                if (Convert.ToString(mainapp.CashierSalesOrderDetails.CreditCardAmount) == "")
                {
                    mainapp.CashierSalesOrderDetails.CreditCardAmount = 0;
                    mainapp.CashierSalesOrderDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.HandoverCreditAmt = mainapp.CashierSalesOrderDetails.CreditCardAmount;
                }
                if (Convert.ToString(mainapp.CashierSalesOrderDetails.DebitCardAmount) == "")
                {
                    mainapp.CashierSalesOrderDetails.DebitCardAmount = 0;
                    mainapp.CashierSalesOrderDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.HandoverDebitAmt = mainapp.CashierSalesOrderDetails.DebitCardAmount;
                }
                if (Convert.ToString(mainapp.CashierSalesOrderDetails.ChequeAmount) == "")
                {
                    mainapp.CashierSalesOrderDetails.ChequeAmount = 0;
                    mainapp.CashierSalesOrderDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.HandoverChequeAmt = mainapp.CashierSalesOrderDetails.ChequeAmount;
                }

                var username = HttpContext.Session["UserName"].ToString();
                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierSalesOrderDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierSalesOrderDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierSalesOrderDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierSalesOrderDetails.ShopName = "SuperAdmin";
                }
                _CashierSalesOrderService.Update(mainapp.CashierSalesOrderDetails);
            }

            // UPDATE CASHIER RETAIL BILL
            else if (frmcol["BillType"] == "Retail Bill")
            {
                mainapp.CashierRetailBillDetails.PaymentType = frmcol["PaymentType"];
                mainapp.CashierRetailBillDetails.RetailBillNo = frmcol["BillNo"];
                mainapp.CashierRetailBillDetails.Status = "Active";
                mainapp.CashierRetailBillDetails.ModifiedOn = DateTime.Now;

                if (frmcol["Amt1"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_1000_Amt = 0;
                }
                if (frmcol["Amt2"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_500_Amt = 0;
                }
                if (frmcol["Amt3"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_100_Amt = 0;
                }
                if (frmcol["Amt4"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_50_Amt = 0;
                }
                if (frmcol["Amt5"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_20_Amt = 0;
                }
                if (frmcol["Amt6"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_10_Amt = 0;
                }
                if (frmcol["Amt7"] != "")
                {
                    mainapp.CashierRetailBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                }
                else
                {
                    mainapp.CashierRetailBillDetails.Cash_1_Amt = 0;
                }
                if (mainapp.CashierRetailBillDetails.TotalCash == null)
                {
                    mainapp.CashierRetailBillDetails.TotalCash = 0;
                }

                if (mainapp.CashierRetailBillDetails.PaymentType == "Card" || mainapp.CashierRetailBillDetails.PaymentType == "CashCardCheque")
                {
                    mainapp.CashierRetailBillDetails.SelectedCard = frmcol["Card"];
                }

                //SAVE HANDOVER AMOUNT AFTER EDIT SALES ORDER
                if (Convert.ToString(mainapp.CashierRetailBillDetails.CreditCardAmount) == "")
                {
                    mainapp.CashierRetailBillDetails.CreditCardAmount = 0;
                    mainapp.CashierRetailBillDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    mainapp.CashierRetailBillDetails.HandoverCreditAmt = mainapp.CashierRetailBillDetails.CreditCardAmount;
                }
                if (Convert.ToString(mainapp.CashierRetailBillDetails.DebitCardAmount) == "")
                {
                    mainapp.CashierRetailBillDetails.DebitCardAmount = 0;
                    mainapp.CashierRetailBillDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    mainapp.CashierRetailBillDetails.HandoverDebitAmt = mainapp.CashierRetailBillDetails.DebitCardAmount;
                }
                if (Convert.ToString(mainapp.CashierRetailBillDetails.ChequeAmount) == "")
                {
                    mainapp.CashierRetailBillDetails.ChequeAmount = 0;
                    mainapp.CashierRetailBillDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    mainapp.CashierRetailBillDetails.HandoverChequeAmt = mainapp.CashierRetailBillDetails.ChequeAmount;
                }

                var username = HttpContext.Session["UserName"].ToString();
                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierRetailBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierRetailBillDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierRetailBillDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierRetailBillDetails.ShopName = "SuperAdmin";
                }
                _CashierRetailBillService.Update(mainapp.CashierRetailBillDetails);
            }

            // UPDATE CASHIER RETAIL BILL
            else if (frmcol["BillType"] == "Sales Bill")
            {
                mainapp.CashierSalesBillDetails.PaymentType = frmcol["PaymentType"];
                mainapp.CashierSalesBillDetails.SalesBillNo = frmcol["BillNo"];
                mainapp.CashierSalesBillDetails.Status = "Active";
                mainapp.CashierSalesBillDetails.ModifiedOn = DateTime.Now;

                if (frmcol["Amt1"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_1000_Amt = 0;
                }
                if (frmcol["Amt2"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_500_Amt = 0;
                }
                if (frmcol["Amt3"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_100_Amt = 0;
                }
                if (frmcol["Amt4"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_50_Amt = 0;
                }
                if (frmcol["Amt5"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_20_Amt = 0;
                }
                if (frmcol["Amt6"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_10_Amt = 0;
                }
                if (frmcol["Amt7"] != "")
                {
                    mainapp.CashierSalesBillDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                }
                else
                {
                    mainapp.CashierSalesBillDetails.Cash_1_Amt = 0;
                }
                if (mainapp.CashierSalesBillDetails.TotalCash == null)
                {
                    mainapp.CashierSalesBillDetails.TotalCash = 0;
                }

                if (mainapp.CashierSalesBillDetails.PaymentType == "Card" || mainapp.CashierSalesBillDetails.PaymentType == "CashCardCheque")
                {
                    mainapp.CashierSalesBillDetails.SelectedCard = frmcol["Card"];
                }

                //SAVE HANDOVER AMOUNT AFTER EDIT SALES ORDER
                if (Convert.ToString(mainapp.CashierSalesBillDetails.CreditCardAmount) == "")
                {
                    mainapp.CashierSalesBillDetails.CreditCardAmount = 0;
                    mainapp.CashierSalesBillDetails.HandoverCreditAmt = 0;
                }
                else
                {
                    mainapp.CashierSalesBillDetails.HandoverCreditAmt = mainapp.CashierSalesBillDetails.CreditCardAmount;
                }
                if (Convert.ToString(mainapp.CashierSalesBillDetails.DebitCardAmount) == "")
                {
                    mainapp.CashierSalesBillDetails.DebitCardAmount = 0;
                    mainapp.CashierSalesBillDetails.HandoverDebitAmt = 0;
                }
                else
                {
                    mainapp.CashierSalesBillDetails.HandoverDebitAmt = mainapp.CashierSalesBillDetails.DebitCardAmount;
                }
                if (Convert.ToString(mainapp.CashierSalesBillDetails.ChequeAmount) == "")
                {
                    mainapp.CashierSalesBillDetails.ChequeAmount = 0;
                    mainapp.CashierSalesBillDetails.HandoverChequeAmt = 0;
                }
                else
                {
                    mainapp.CashierSalesBillDetails.HandoverChequeAmt = mainapp.CashierSalesBillDetails.ChequeAmount;
                }

                var username = HttpContext.Session["UserName"].ToString();
                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.CashierSalesBillDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.CashierSalesBillDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.CashierSalesBillDetails.ShopCode = "SuperAdmin";
                    mainapp.CashierSalesBillDetails.ShopName = "SuperAdmin";
                }

                if (mainapp.CashierSalesBillDetails.RefundAmount == null)
                {
                    mainapp.CashierSalesBillDetails.RefundAmount = 0;
                }
                _CashierSalesBillService.Update(mainapp.CashierSalesBillDetails);
            }



            return RedirectToAction("CashierModuleUpdate");
        }

        //************************************************* CASH HANDOVER *************************************************

        //CASH HANDOVER MODULE
        [HttpGet]
        public ActionResult CashHandover()
        {
            MainApplication model = new MainApplication()
            {
                CashHandoverDetails = new CashHandover(),
            };

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
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

            //CREATE CASH HANDOVER CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var cashhanddata = _CashHandoverService.GetLastRowByFinYr(FinYr);
            string CashHandCode = string.Empty;

            int CashHandVal = 0;
            int length = 0;
            if (cashhanddata != null)
            {
                CashHandCode = cashhanddata.CashHandCode.Substring(3, 6);
                length = (Convert.ToInt32(CashHandCode) + 1).ToString().Length;
                CashHandVal = Convert.ToInt32(CashHandCode) + 1;
            }
            else
            {
                CashHandVal = 1;
                length = 1;
            }
            CashHandCode = _utilityservice.getName("CHH", length, CashHandVal);
            CashHandCode = CashHandCode + FinYr;
            model.CashHandoverDetails.CashHandCode = CashHandCode;

            //GET LOGIN USER NAME AND EMAIL
            var useremail = HttpContext.Session["UserEmail"].ToString();
            var empname = _EmployeeMasterService.getEmpByEmail(useremail).Name;
            Session["EmpName"] = empname;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.GetEmpByDesignation();
            return View(model);
        }

        //GET GODOWN ADDRESS DETAILS
        [HttpGet]
        public JsonResult GetEmployeeDetails(string Emp)
        {
            var data = _EmployeeMasterService.GetEmpByCode(Emp);
            if (data != null)
            {
                return Json(new { data.Address, data.Name, data.Email, data.MobileNo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        // PARTIAL VIEW OF HANDOVER CASH PAYMENT DETAILS
        public ActionResult CashPaymentHandover()
        {
            return View();
        }

        //SAVE CASH HANDOVER MODULE
        [HttpPost]
        public ActionResult CashHandover(MainApplication mainapp, FormCollection frmcol)
        {
            //CHEQUE CASH HANDOVER CODE BEFORE SAVING
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var cashhanddata = _CashHandoverService.GetLastRowByFinYr(FinYr);
            string CashHandCode = string.Empty;

            int CashHandVal = 0;
            int length = 0;
            if (cashhanddata != null)
            {
                CashHandCode = cashhanddata.CashHandCode.Substring(3, 6);
                length = (Convert.ToInt32(CashHandCode) + 1).ToString().Length;
                CashHandVal = Convert.ToInt32(CashHandCode) + 1;
            }
            else
            {
                CashHandVal = 1;
                length = 1;
            }
            CashHandCode = _utilityservice.getName("CHH", length, CashHandVal);
            CashHandCode = CashHandCode + FinYr;
            mainapp.CashHandoverDetails.CashHandCode = CashHandCode;

            mainapp.CashHandoverDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
            mainapp.CashHandoverDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
            mainapp.CashHandoverDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
            mainapp.CashHandoverDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
            mainapp.CashHandoverDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
            mainapp.CashHandoverDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
            mainapp.CashHandoverDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
            mainapp.CashHandoverDetails.PreparedByName = Session["EmpName"].ToString();
            mainapp.CashHandoverDetails.PreparedByEmail = UserEmail;
            mainapp.CashHandoverDetails.Status = "Active";
            mainapp.CashHandoverDetails.ModifiedOn = DateTime.Now;
            mainapp.CashHandoverDetails.Date = DateTime.Now;
            _CashHandoverService.Create(mainapp.CashHandoverDetails);
            return RedirectToAction("CashHandover");
        }

        //************************************************* CARD AND CHEQUE HANDOVER *************************************************

        //CREATE CARD/CHEQUE HANDOVER
        [HttpGet]
        public ActionResult CardChequeHandover()
        {
            MainApplication model = new MainApplication()
            {
                CardChequeHandoverDetails = new CardChequeHandover(),
            };

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
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

            //CREATE CARD CHEQUE HANDOVER CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var cardchequedata = _CardChequeHandoverService.GetLastRowByFinYr(FinYr);
            string CardChequeHandCode = string.Empty;

            int CardChequeHandVal = 0;
            int length = 0;
            if (cardchequedata != null)
            {
                CardChequeHandCode = cardchequedata.CardChequeHandCode.Substring(4, 6);
                length = (Convert.ToInt32(CardChequeHandCode) + 1).ToString().Length;
                CardChequeHandVal = Convert.ToInt32(CardChequeHandCode) + 1;
            }
            else
            {
                CardChequeHandVal = 1;
                length = 1;
            }
            CardChequeHandCode = _utilityservice.getName("CRCH", length, CardChequeHandVal);
            CardChequeHandCode = CardChequeHandCode + FinYr;
            model.CardChequeHandoverDetails.CardChequeHandCode = CardChequeHandCode;

            //GET LOGIN USER NAME AND EMAIL
            var useremail = HttpContext.Session["UserEmail"].ToString();
            var empname = _EmployeeMasterService.getEmpByEmail(useremail).Name;
            Session["EmpName"] = empname;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.GetEmpByDesignation();

            //GET ACTIVE CARD/CHEQUE BILLS
            model.CashierSalesOrderList = _CashierSalesOrderService.GetBillsByHandoverStatus();
            model.CashierRetailBillList = _CashierRetailBillService.GetBillsByHandoverStatus();
            model.CashierSalesBillList = _CashierSalesBillService.GetBillsByHandoverStatus();
            model.CashierTemporaryCashMemoList = _CashierTemporaryCashMemoService.GetBillsByHandoverStatus();
            return View(model);
        }

        //SAVE CARD CHEQUE HANDOVER
        [HttpPost]
        public ActionResult CardChequeHandover(MainApplication mainapp, FormCollection frmcol)
        {
            //CHEQUE CARD CHEQUE HANDOVER CODE BEFORE SAVING
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var cardchequedata = _CardChequeHandoverService.GetLastRowByFinYr(FinYr);
            string CardChequeHandCode = string.Empty;

            int CardChequeHandVal = 0;
            int length = 0;
            if (cardchequedata != null)
            {
                CardChequeHandCode = cardchequedata.CardChequeHandCode.Substring(4, 6);
                length = (Convert.ToInt32(CardChequeHandCode) + 1).ToString().Length;
                CardChequeHandVal = Convert.ToInt32(CardChequeHandCode) + 1;
            }
            else
            {
                CardChequeHandVal = 1;
                length = 1;
            }
            CardChequeHandCode = _utilityservice.getName("CRCH", length, CardChequeHandVal);
            CardChequeHandCode = CardChequeHandCode + FinYr;
            mainapp.CardChequeHandoverDetails.CardChequeHandCode = CardChequeHandCode;

            var count = Convert.ToInt32(frmcol["ItemList"]);
            for (int i = 1; i <= count; i++)
            {
                string checkbox = "CheckBox" + i;
                string cashiercodeval = "CashierCodeVal" + i;
                string billnumberval = "BillNoVal" + i;
                string dateval = "DateVal" + i;
                string paytypeval = "PaymentTypeVal" + i;
                string cardchequenoval = "CardChequeNoVal" + i;
                string amountval = "AmountVal" + i;

                if (frmcol[checkbox] == "Yes")
                {
                    mainapp.CardChequeHandoverDetails.CashierCode = frmcol[cashiercodeval];
                    mainapp.CardChequeHandoverDetails.BillNo = frmcol[billnumberval];
                    mainapp.CardChequeHandoverDetails.CardChequeNo = frmcol[cardchequenoval];
                    mainapp.CardChequeHandoverDetails.HandoverType = frmcol[paytypeval];
                    mainapp.CardChequeHandoverDetails.CardChequeAmount = Convert.ToDouble(frmcol[amountval]);
                    mainapp.CardChequeHandoverDetails.CardChequeDate = Convert.ToDateTime(frmcol[dateval]);

                    mainapp.CardChequeHandoverDetails.Date = DateTime.Now;
                    mainapp.CardChequeHandoverDetails.Status = "Active";
                    mainapp.CardChequeHandoverDetails.ModifiedOn = DateTime.Now;
                    mainapp.CardChequeHandoverDetails.PreparedByName = Session["EmpName"].ToString();
                    mainapp.CardChequeHandoverDetails.PreparedByEmail = HttpContext.Session["UserEmail"].ToString();
                    _CardChequeHandoverService.Create(mainapp.CardChequeHandoverDetails);

                    //IF CARD OR CHEQUE HANDOVER THEN NEXT TIME IT CANNOT BE SHOW..
                    if (frmcol[billnumberval].Contains("SO"))
                    {
                        var CashierSODetails = _CashierSalesOrderService.GetRowsBySOAndCashierNo(frmcol[billnumberval], frmcol[cashiercodeval]);
                        if (frmcol[paytypeval] == "CreditCard")
                        {
                            CashierSODetails.HandoverCreditAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "DebitCard")
                        {
                            CashierSODetails.HandoverDebitAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "Cheque")
                        {
                            CashierSODetails.HandoverChequeAmt = 0;
                        }

                        //if credit/debit/cheque amount both are 0 then handover status in "InActive"..
                        if (CashierSODetails.HandoverCreditAmt == 0 && CashierSODetails.HandoverDebitAmt == 0 && CashierSODetails.HandoverChequeAmt == 0)
                        {
                            CashierSODetails.HandoverStatus = "InActive";
                        }
                        _CashierSalesOrderService.Update(CashierSODetails);
                    }
                    else if (frmcol[billnumberval].Contains("RI"))
                    {
                        var CashierRIDetails = _CashierRetailBillService.GetRowsByRIAndCashierNo(frmcol[billnumberval], frmcol[cashiercodeval]);
                        if (frmcol[paytypeval] == "CreditCard")
                        {
                            CashierRIDetails.HandoverCreditAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "DebitCard")
                        {
                            CashierRIDetails.HandoverDebitAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "Cheque")
                        {
                            CashierRIDetails.HandoverChequeAmt = 0;
                        }

                        //if credit/debit/cheque amount both are 0 then handover status in "InActive"..
                        if (CashierRIDetails.HandoverCreditAmt == 0 && CashierRIDetails.HandoverDebitAmt == 0 && CashierRIDetails.HandoverChequeAmt == 0)
                        {
                            CashierRIDetails.HandoverStatus = "InActive";
                        }
                        _CashierRetailBillService.Update(CashierRIDetails);
                    }
                    else if (frmcol[billnumberval].Contains("SB"))
                    {
                        var CashierSBDetails = _CashierSalesBillService.GetRowsBySBAndCashierNo(frmcol[billnumberval], frmcol[cashiercodeval]);
                        if (frmcol[paytypeval] == "CreditCard")
                        {
                            CashierSBDetails.HandoverCreditAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "DebitCard")
                        {
                            CashierSBDetails.HandoverDebitAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "Cheque")
                        {
                            CashierSBDetails.HandoverChequeAmt = 0;
                        }

                        //if credit/debit/cheque amount both are 0 then handover status in "InActive"..
                        if (CashierSBDetails.HandoverCreditAmt == 0 && CashierSBDetails.HandoverDebitAmt == 0 && CashierSBDetails.HandoverChequeAmt == 0)
                        {
                            CashierSBDetails.HandoverStatus = "InActive";
                        }
                        _CashierSalesBillService.Update(CashierSBDetails);
                    }

                    else if (frmcol[billnumberval].Contains("PCM"))
                    {
                        var CashierTCMDetails = _CashierTemporaryCashMemoService.GetRowsByTCMAndCashierNo(frmcol[billnumberval], frmcol[cashiercodeval]);
                        if (frmcol[paytypeval] == "CreditCard")
                        {
                            CashierTCMDetails.HandoverCreditAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "DebitCard")
                        {
                            CashierTCMDetails.HandoverDebitAmt = 0;
                        }
                        else if (frmcol[paytypeval] == "Cheque")
                        {
                            CashierTCMDetails.HandoverChequeAmt = 0;
                        }

                        //if credit/debit/cheque amount both are 0 then handover status in "InActive"..
                        if (CashierTCMDetails.HandoverCreditAmt == 0 && CashierTCMDetails.HandoverDebitAmt == 0 && CashierTCMDetails.HandoverChequeAmt == 0)
                        {
                            CashierTCMDetails.HandoverStatus = "InActive";
                        }
                        _CashierTemporaryCashMemoService.Update(CashierTCMDetails);
                    }
                }
            }
            return RedirectToAction("CardChequeHandover");
        }

        //UPDATE CARD/CHEQUE HANDOVER
        [HttpGet]
        public ActionResult EditCardChequeHandover()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeMasterService.GetEmpByDesignation();

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
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

            //GET LOGIN USER NAME AND EMAIL
            var useremail = HttpContext.Session["UserEmail"].ToString();
            var empname = _EmployeeMasterService.getEmpByEmail(useremail).Name;
            Session["EmpName"] = empname;

            //GET LAST FIVE DAYS CARD CHECK NO WHICH IS HANDOVER
            DateTime Today = DateTime.Now;
            DateTime FiveDaysBefore = DateTime.Now.AddDays(-4);
            model.CardChequeHandoverList = _CardChequeHandoverService.GetRowsByFromAndToDate(FiveDaysBefore, Today);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditCardChequeHandover(MainApplication mainapp, FormCollection frmcol)
        {
            var count = Convert.ToInt32(frmcol["HandoverList"]);
            for (int i = 1; i <= count; i++)
            {
                string cardchequeid = "cardChequeId" + i;
                string checkbox = "CheckBox" + i;
                string cashiercodeval = "CashierCodeVal" + i;
                string billnumberval = "BillNoVal" + i;
                string dateval = "DateVal" + i;
                string paytypeval = "PaymentTypeVal" + i;
                string cardchequenoval = "CardChequeNoVal" + i;
                string amountval = "AmountVal" + i;

                if (frmcol[checkbox] == "Yes")
                {
                    var billno = frmcol[billnumberval];
                    var cashiercode = frmcol[cashiercodeval];
                    var paymenttype = frmcol[paytypeval];
                    var amount = frmcol[amountval];

                    //UPDATE CASHIER SALES ORDER TABLE
                    if (billno.Contains("SO"))
                    {
                        var CashSODetails = _CashierSalesOrderService.GetRowsBySOAndCashierNo(billno, cashiercode);
                        if (paymenttype == "CreditCard")
                        {
                            CashSODetails.HandoverCreditAmt = Convert.ToDouble(amount);
                            CashSODetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "DebitCard")
                        {
                            CashSODetails.HandoverDebitAmt = Convert.ToDouble(amount);
                            CashSODetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "Cheque")
                        {
                            CashSODetails.HandoverChequeAmt = Convert.ToDouble(amount);
                            CashSODetails.HandoverStatus = "Active";
                        }
                        _CashierSalesOrderService.Update(CashSODetails);
                    }

                    //UPDATE CASHIER RETAIL BILL TABLE
                    else if (billno.Contains("RI"))
                    {
                        var CashRIDetails = _CashierRetailBillService.GetRowsByRIAndCashierNo(billno, cashiercode);
                        if (paymenttype == "CreditCard")
                        {
                            CashRIDetails.HandoverCreditAmt = Convert.ToDouble(amount);
                            CashRIDetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "DebitCard")
                        {
                            CashRIDetails.HandoverDebitAmt = Convert.ToDouble(amount);
                            CashRIDetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "Cheque")
                        {
                            CashRIDetails.HandoverChequeAmt = Convert.ToDouble(amount);
                            CashRIDetails.HandoverStatus = "Active";
                        }
                        _CashierRetailBillService.Update(CashRIDetails);
                    }

                    //UPDATE CASHIER SALES BILL TABLE
                    else if (billno.Contains("SB"))
                    {
                        var CashSBDetails = _CashierSalesBillService.GetRowsBySBAndCashierNo(billno, cashiercode);
                        if (paymenttype == "CreditCard")
                        {
                            CashSBDetails.HandoverCreditAmt = Convert.ToDouble(amount);
                            CashSBDetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "DebitCard")
                        {
                            CashSBDetails.HandoverDebitAmt = Convert.ToDouble(amount);
                            CashSBDetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "Cheque")
                        {
                            CashSBDetails.HandoverChequeAmt = Convert.ToDouble(amount);
                            CashSBDetails.HandoverStatus = "Active";
                        }
                        _CashierSalesBillService.Update(CashSBDetails);
                    }

                    //UPDATE CASHIER TEMPORARY CASH MEMO TABLE
                    else if (billno.Contains("PCM"))
                    {
                        var CashTCMDetails = _CashierTemporaryCashMemoService.GetRowsByTCMAndCashierNo(billno, cashiercode);
                        if (paymenttype == "CreditCard")
                        {
                            CashTCMDetails.HandoverCreditAmt = Convert.ToDouble(amount);
                            CashTCMDetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "DebitCard")
                        {
                            CashTCMDetails.HandoverDebitAmt = Convert.ToDouble(amount);
                            CashTCMDetails.HandoverStatus = "Active";
                        }
                        else if (paymenttype == "Cheque")
                        {
                            CashTCMDetails.HandoverChequeAmt = Convert.ToDouble(amount);
                            CashTCMDetails.HandoverStatus = "Active";
                        }
                        _CashierTemporaryCashMemoService.Update(CashTCMDetails);
                    }

                    //DELETE UPDATED DATA INTO CARD CHEQUE HANDOVER
                    var CardChequeHandId = Convert.ToInt32(frmcol[cardchequeid]);
                    var CardChequeHandDetails = _CardChequeHandoverService.GetDetailsById(CardChequeHandId);
                    _CardChequeHandoverService.Delete(CardChequeHandDetails);

                }
            }
            return RedirectToAction("EditCardChequeHandover");
        }

    }
}
