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
using CodeFirstServices.Services;
using MvcRetailApp.ModelBinder;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class OutwardToTailorController : Controller
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

        private readonly IUtilityService _UtilityService;
        private readonly IModuleService _ModuleService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IOutwardToTailorService _OutwardToTailorService;
        private readonly IOutwardToTailorItemService _OutwardToTailorItemService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IRetailBillService _RetailBillService;
        private readonly IRetailBillItemService _RetailBillItemService;
        private readonly ISalesBillService _SalesBillService;
        private readonly ISalesBillItemService _SalesBillItemService;
        private readonly IJobWorkTypeService _JobWorkTypeService;
        private readonly IJobWorkerService _JobWorkerService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IJobWorkPaymentService _JobWorkPaymentService;

        public OutwardToTailorController(IUtilityService UtilityService, IModuleService ModuleService, IUserCredentialService IUserCredentialService,
            IOutwardToTailorService OutwardToTailorService, IOutwardToTailorItemService OutwardToTailorItemService, IClientMasterService ClientMasterService, IRetailBillService RetailBillService, IRetailBillItemService RetailBillItemService,
            ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService, IJobWorkTypeService JobWorkTypeService, IJobWorkerService JobWorkerService, IEmployeeMasterService EmployeeMasterService, IJobWorkPaymentService JobWorkPaymentService)
        {
            this._UtilityService = UtilityService;
            this._ModuleService = ModuleService;
            this._IUserCredentialService = IUserCredentialService;
            this._OutwardToTailorService = OutwardToTailorService;
            this._OutwardToTailorItemService = OutwardToTailorItemService;
            this._ClientMasterService = ClientMasterService;
            this._RetailBillService = RetailBillService;
            this._RetailBillItemService = RetailBillItemService;
            this._SalesBillService = SalesBillService;
            this._SalesBillItemService = SalesBillItemService;
            this._JobWorkTypeService = JobWorkTypeService;
            this._JobWorkerService = JobWorkerService;
            this._EmployeeMasterService = EmployeeMasterService;
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


        //CRAETE OUTWARD TO TAILOR
        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                OutwardToTailorDetails = new OutwardToTailor(),
            };

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            string OutwardCode = string.Empty;

            var details = _OutwardToTailorService.GetLastRowrByFinYr(FinYr);
            int val = 0;
            int length = 0;
            if (details != null)
            {
                OutwardCode = details.OutwardCode.Substring(4, 6);
                length = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                val = Convert.ToInt32(OutwardCode) + 1;

                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            OutwardCode = _UtilityService.getName("OWTT", length, val);
            OutwardCode = OutwardCode + FinYr;
            model.OutwardToTailorDetails.OutwardCode = OutwardCode;
            TempData["PreviousOutToTailor"] = OutwardCode;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _ModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWGS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWGS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.RetailBillList = _RetailBillService.GetAll();
            model.JobWorkerList = _JobWorkerService.GetAll();
            return View(model);
        }

        //GET RETAIL BILL ON ONLOAD FUNCTION
        [HttpGet]
        public JsonResult GetRetailBills()
        {
            var billnos = _RetailBillService.GetAll();
            return Json(billnos, JsonRequestBehavior.AllowGet);
        }

        //GET SALES BILL ON ONLOAD FUNCTION
        [HttpGet]
        public JsonResult GetSalesBills()
        {
            var billnos = _SalesBillService.GetAll();
            return Json(billnos, JsonRequestBehavior.AllowGet);
        }

        //GET JOB WORK TYPE ON ONLOAD FUNCTION
        [HttpGet]
        public JsonResult GetJobWorkTypes()
        {
            var jobworktypes = _JobWorkTypeService.GetAll();
            return Json(jobworktypes, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE CLIENT NAME
        [HttpGet]
        public JsonResult GetClientNames(string SearchTerm)
        {
            var ClientData = _ClientMasterService.GetClientNames(SearchTerm);
            List<string> names = new List<string>();
            foreach (var item in ClientData)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET TAILOR DETAILS
        [HttpGet]
        public JsonResult GetClientDetails(string name)
        {
            var data = _ClientMasterService.getClientByName(name);
            return Json(new { data.Address, data.ContactNo1, data.Email }, JsonRequestBehavior.AllowGet);
        }

        //GET TAILOR DETAILS
        [HttpGet]
        public JsonResult GetTailorDetails(string name)
        {
            var data = _JobWorkerService.GetDetailsByName(name);
            return Json(new { data.Address, data.MobileNo, data.Email }, JsonRequestBehavior.AllowGet);
        }

        //GET RETAIL BILL BY CLIENT NAME
        [HttpGet]
        public JsonResult GetRetailBillFromClient(string ClientName)
        {
            MainApplication model = new MainApplication();
            model.RetailBillList = _RetailBillService.GetBillsByClientName(ClientName);
            var RetailBillNos = model.RetailBillList.Select(s => new SelectListItem()
            {
                Text = s.RetailBillNo,
                Value = s.RetailBillNo
            });
            return Json(new { RetailBillNos }, JsonRequestBehavior.AllowGet);
        }

        //GET SALES BILL BY CLIENT NAME
        [HttpGet]
        public JsonResult GetSalesBillFromClient(string ClientName)
        {
            MainApplication model = new MainApplication();
            model.SalesBillList = _SalesBillService.GetBillsByClientName(ClientName);
            var SalesBillNos = model.SalesBillList.Select(s => new SelectListItem()
            {
                Text = s.SalesBillNo,
                Value = s.SalesBillNo
            });
            return Json(new { SalesBillNos }, JsonRequestBehavior.AllowGet);
        }

        //GET SALES BILL ITEMS
        [HttpGet]
        public ActionResult GetRetailAndSalesBillItemsFromBills(string RBBillNos, string SBBillNos)
        {
            MainApplication model = new MainApplication();
            if (RBBillNos.Contains("RI"))
            {
                string RetailBillNos = RBBillNos;
                string[] SingleRetailBillNo = RetailBillNos.Split(',');
                List<RetailBillItem> ItemList = new List<RetailBillItem>();
                for (int i = 0; i < SingleRetailBillNo.Count(); i++)
                {
                    var retailitemlist = _RetailBillItemService.GetDetailsByRetailBillNo(SingleRetailBillNo[i]);
                    foreach (var data in retailitemlist)
                    {
                        ItemList.Add(data);
                    }
                }
                model.RetailBillItemList = ItemList;
            }
            if (SBBillNos.Contains("SB"))
            {
                string SalesBillNos = SBBillNos;
                string[] SingleSalesBillNo = SalesBillNos.Split(',');
                List<SalesBillItem> ItemList = new List<SalesBillItem>();
                for (int i = 0; i < SingleSalesBillNo.Count(); i++)
                {
                    var salesitemlist = _SalesBillItemService.GetItemsBySalesBillNo(SingleSalesBillNo[i]);
                    foreach (var data in salesitemlist)
                    {
                        ItemList.Add(data);
                    }
                }
                model.SalesBillItemList = ItemList;
            }
            return View(model);
        }

        //GET LOGIN EMPLOPYEE DETAILS BY EMPLOYEE EMAIL
        [HttpGet]
        public JsonResult GetPreparedByEmpDetails(string email)
        {
            var data = _EmployeeMasterService.getEmpByEmail(email);
            return Json(new { data.Name, data.MobileNo }, JsonRequestBehavior.AllowGet);
        }

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

        //SAVE OUTWARD TO TAILOR
        [HttpPost]
        public ActionResult Create(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                OutwardToTailorItemDetails = new OutwardToTailorItem(),
                JobWorkPaymentDetails=new JobWorkPayment(),
            };

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            string OutwardCode = string.Empty;

            var details = _OutwardToTailorService.GetLastRowrByFinYr(FinYr);
            int val = 0;
            int length = 0;
            if (details != null)
            {
                OutwardCode = details.OutwardCode.Substring(4, 6);
                length = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                val = Convert.ToInt32(OutwardCode) + 1;

                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            OutwardCode = _UtilityService.getName("OWTT", length, val);
            OutwardCode = OutwardCode + FinYr;
            mainapp.OutwardToTailorDetails.OutwardCode = OutwardCode;

            //save main outward to tailor..
            mainapp.OutwardToTailorDetails.Date = DateTime.Now;
            mainapp.OutwardToTailorDetails.RetailBills = frmcol["RetailBillNoDDL"];
            mainapp.OutwardToTailorDetails.SalesBills = frmcol["SalesBillNoDDL"];
            mainapp.OutwardToTailorDetails.GrandTotal = Convert.ToDouble(frmcol["GrandTotalVal"]);
            mainapp.OutwardToTailorDetails.PaidAmountToTailor = 0;
            mainapp.OutwardToTailorDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
            mainapp.OutwardToTailorDetails.Status = "Active";
            mainapp.OutwardToTailorDetails.InwardStatus = "Active";
            mainapp.OutwardToTailorDetails.TailorInwardStatus = "Active";
            mainapp.OutwardToTailorDetails.ModifiedOn = DateTime.Now;

            var username = HttpContext.Session["UserName"].ToString();
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                mainapp.OutwardToTailorDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                mainapp.OutwardToTailorDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
            }
            else
            {
                mainapp.OutwardToTailorDetails.ShopCode = "SuperAdmin";
                mainapp.OutwardToTailorDetails.ShopName = "SuperAdmin";
            }
            _OutwardToTailorService.Create(mainapp.OutwardToTailorDetails);

            //save outward to tailor items..
            int itemcount = Convert.ToInt32(frmcol["ItemList"]);
            if (itemcount != 0)
            {
                for (int i = 1; i < itemcount; i++)
                {
                    string checkbox = "CheckBox" + i;
                    string billno = "BillNo" + i;
                    string itemcode = "ItemCode" + i;
                    string itemname = "ItemName" + i;
                    string narration = "Narration" + i;
                    string quantity = "Quantity" + i;
                    string tailoramt = "TailorAmt" + i;
                    string accessoriesamt = "AccessoriesAmt" + i;
                    string jobworktypeddl = "JobWorkTypeDDL" + i;
                    string itemtype = "ItemType" + i;
                    string description = "Description" + i;
                    string barcode = "Barcode" + i;
                    string unit = "Unit" + i;
                    string numbertype = "NumberType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string size = "Size" + i;
                    string brand = "Brand" + i;
                    string designcode = "DesignCode" + i;
                    string designname = "DesignnName" + i;
                    string color = "Color" + i;
                    string material = "Material" + i;

                    if (frmcol[checkbox] == "Yes")
                    {
                        model.OutwardToTailorItemDetails.BillNo = frmcol[billno];
                        model.OutwardToTailorItemDetails.ItemCode = frmcol[itemcode];
                        model.OutwardToTailorItemDetails.ItemName = frmcol[itemname];
                        model.OutwardToTailorItemDetails.NarrationForTailor = frmcol[narration];
                        model.OutwardToTailorItemDetails.Quantity = frmcol[quantity];
                        model.OutwardToTailorItemDetails.JobWorktype = frmcol[jobworktypeddl];
                        model.OutwardToTailorItemDetails.TailorAmount = Convert.ToDouble(frmcol[tailoramt]);
                        model.OutwardToTailorItemDetails.AccessoriesAmount = Convert.ToDouble(frmcol[accessoriesamt]);
                        model.OutwardToTailorItemDetails.ItemType = frmcol[itemtype];
                        model.OutwardToTailorItemDetails.Description = frmcol[description];
                        model.OutwardToTailorItemDetails.Barcode = frmcol[barcode];
                        model.OutwardToTailorItemDetails.Unit = frmcol[unit];
                        model.OutwardToTailorItemDetails.NumberType = frmcol[numbertype];
                        if (frmcol[sellingprice] != "")
                        {
                            model.OutwardToTailorItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        }
                        else
                        {
                            model.OutwardToTailorItemDetails.SellingPrice = null;
                        }
                        if (frmcol[mrp] != "")
                        {
                            model.OutwardToTailorItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            model.OutwardToTailorItemDetails.MRP = null;
                        }
                        model.OutwardToTailorItemDetails.Size = frmcol[size];
                        model.OutwardToTailorItemDetails.Brand = frmcol[brand];
                        model.OutwardToTailorItemDetails.DesignCode = frmcol[designcode];
                        model.OutwardToTailorItemDetails.DesignName = frmcol[designname];
                        model.OutwardToTailorItemDetails.Color = frmcol[color];
                        model.OutwardToTailorItemDetails.Material = frmcol[material];
                        model.OutwardToTailorItemDetails.OutwardCode = mainapp.OutwardToTailorDetails.OutwardCode;
                        model.OutwardToTailorItemDetails.Status = "Active";
                        model.OutwardToTailorItemDetails.InwardStatus = "Active";
                        model.OutwardToTailorItemDetails.ModifiedOn = DateTime.Now;
                        _OutwardToTailorItemService.Create(model.OutwardToTailorItemDetails);
                    }
                }
            }

            //save job work payment..
            if (mainapp.OutwardToTailorDetails.AdvancePayment != 0)
            {
                //CREATE JOB WORK PAYMENT CODE
                string year1 = FinancialYear;
                string[] yr1 = year1.Split(' ', '-');
                string FinYr1 = "/" + yr1[2].Substring(2) + "-" + yr1[6].Substring(2);
                string paymentcode = string.Empty;

                var jobworkpaymentdata = _JobWorkPaymentService.GetLastPaymentByFinYr(FinYr1);
                int PaymentVal = 0;
                int length1 = 0;
                if (jobworkpaymentdata != null)
                {
                    paymentcode = jobworkpaymentdata.PaymentCode.Substring(3, 6);
                    length1 = (Convert.ToInt32(paymentcode) + 1).ToString().Length;
                    PaymentVal = Convert.ToInt32(paymentcode) + 1;
                }
                else
                {
                    PaymentVal = 1;
                    length1 = 1;
                }

                paymentcode = _UtilityService.getName("JWP", length1, PaymentVal);
                paymentcode = paymentcode + FinYr1;
                model.JobWorkPaymentDetails.PaymentCode = paymentcode;


                model.JobWorkPaymentDetails.PaymentType = frmcol["PaymentType"];
                model.JobWorkPaymentDetails.Cash_1000 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_1000"]);
                model.JobWorkPaymentDetails.Cash_500 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_500"]);
                model.JobWorkPaymentDetails.Cash_100 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_100"]);
                model.JobWorkPaymentDetails.Cash_50 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_50"]);
                model.JobWorkPaymentDetails.Cash_20 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_20"]);
                model.JobWorkPaymentDetails.Cash_10 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_10"]);
                model.JobWorkPaymentDetails.Cash_1 = Convert.ToDouble(frmcol["JobWorkPaymentDetails.Cash_1"]);
                model.JobWorkPaymentDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
                model.JobWorkPaymentDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
                model.JobWorkPaymentDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
                model.JobWorkPaymentDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
                model.JobWorkPaymentDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
                model.JobWorkPaymentDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
                model.JobWorkPaymentDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
                model.JobWorkPaymentDetails.TotalCash = Convert.ToDouble(frmcol["JobWorkPaymentDetails.TotalCash"]);
                model.JobWorkPaymentDetails.SelectedCard = frmcol["Card"];
                model.JobWorkPaymentDetails.CreditCardNo = frmcol["JobWorkPaymentDetails.CreditCardNo"];
                if (frmcol["JobWorkPaymentDetails.CreditCardAmount"] == "")
                {
                    model.JobWorkPaymentDetails.CreditCardAmount = 0;
                }
                else
                {
                    model.JobWorkPaymentDetails.CreditCardAmount = Convert.ToDouble(frmcol["JobWorkPaymentDetails.CreditCardAmount"]);
                }
                model.JobWorkPaymentDetails.CreditCardType = frmcol["JobWorkPaymentDetails.CreditCardType"];
                model.JobWorkPaymentDetails.CreditCardBank = frmcol["JobWorkPaymentDetails.CreditCardBank"];
                model.JobWorkPaymentDetails.DebitCardNo = frmcol["JobWorkPaymentDetails.DebitCardNo"];
                model.JobWorkPaymentDetails.DebitCardName = frmcol["JobWorkPaymentDetails.DebitCardName"];
                model.JobWorkPaymentDetails.DebitCardType = frmcol["JobWorkPaymentDetails.DebitCardType"];
                model.JobWorkPaymentDetails.DebitCardBank = frmcol["JobWorkPaymentDetails.DebitCardBank"];
                if (frmcol["JobWorkPaymentDetails.DebitCardAmount"] == "")
                {
                    model.JobWorkPaymentDetails.DebitCardAmount = 0;
                }
                else
                {
                    model.JobWorkPaymentDetails.DebitCardAmount = Convert.ToDouble(frmcol["JobWorkPaymentDetails.DebitCardAmount"]);
                }
                model.JobWorkPaymentDetails.ChequeNo = frmcol["JobWorkPaymentDetails.ChequeNo"];
                model.JobWorkPaymentDetails.ChequeAccNo = frmcol["JobWorkPaymentDetails.ChequeAccNo"];
                model.JobWorkPaymentDetails.ChequeAmount = Convert.ToDouble(frmcol["JobWorkPaymentDetails.ChequeAmount"]);
                if (model.JobWorkPaymentDetails.ChequeNo != null && model.JobWorkPaymentDetails.ChequeNo != "")
                {
                    model.JobWorkPaymentDetails.ChequeDate = Convert.ToDateTime(frmcol["JobWorkPaymentDetails.ChequeDate"]);
                }
                else
                {
                    model.JobWorkPaymentDetails.ChequeDate = null;
                }
                model.JobWorkPaymentDetails.ChequeBank = frmcol["JobWorkPaymentDetails.ChequeBank"];
                model.JobWorkPaymentDetails.ChequeBranch = frmcol["JobWorkPaymentDetails.ChequeBranch"];

                model.JobWorkPaymentDetails.Date = DateTime.Now;
                model.JobWorkPaymentDetails.OutwardToTailorNo = mainapp.OutwardToTailorDetails.OutwardCode;
                model.JobWorkPaymentDetails.OutwardToTailorDate = mainapp.OutwardToTailorDetails.Date;
                model.JobWorkPaymentDetails.ClientName = mainapp.OutwardToTailorDetails.ClientName;
                model.JobWorkPaymentDetails.ClientAddress = mainapp.OutwardToTailorDetails.ClientAddress;
                model.JobWorkPaymentDetails.ClientContact = mainapp.OutwardToTailorDetails.ClientContact;
                model.JobWorkPaymentDetails.ClientEmail = mainapp.OutwardToTailorDetails.ClientEmail;
                model.JobWorkPaymentDetails.TailorName = mainapp.OutwardToTailorDetails.TailorName;
                model.JobWorkPaymentDetails.TailorContact = mainapp.OutwardToTailorDetails.TailorContact;
                model.JobWorkPaymentDetails.TailorAddress = mainapp.OutwardToTailorDetails.TailorAddress;
                model.JobWorkPaymentDetails.TailorEmail = mainapp.OutwardToTailorDetails.TailorEmail;
                model.JobWorkPaymentDetails.GrandTotal = mainapp.OutwardToTailorDetails.GrandTotal;
                model.JobWorkPaymentDetails.AdvancePayment = mainapp.OutwardToTailorDetails.AdvancePayment;
                model.JobWorkPaymentDetails.Payment = mainapp.OutwardToTailorDetails.AdvancePayment;
                model.JobWorkPaymentDetails.Balance = mainapp.OutwardToTailorDetails.Balance;
                model.JobWorkPaymentDetails.ShopCode = mainapp.OutwardToTailorDetails.ShopCode;
                model.JobWorkPaymentDetails.ShopName = mainapp.OutwardToTailorDetails.ShopName;
                model.JobWorkPaymentDetails.PreparedBy = mainapp.OutwardToTailorDetails.PreparedBy;
                model.JobWorkPaymentDetails.Status = "Active";
                model.JobWorkPaymentDetails.ModifiedOn = DateTime.Now;
                _JobWorkPaymentService.Create(model.JobWorkPaymentDetails);
            }

            var OutToTailorId = _OutwardToTailorService.GetLastRow().Id;
            var Outwardid = Encode(OutToTailorId.ToString());
            return RedirectToAction("CreateDetails/" + Outwardid, "OutwardToTailor");
        }

        //DETAILS PAGE OF OUTWARD TO TAILOR CREATE
        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.OutwardToTailorDetails = _OutwardToTailorService.GetDetailsById(Id);
            model.OutwardToTailorItemList = _OutwardToTailorItemService.GetRowsByCode(model.OutwardToTailorDetails.OutwardCode);

            string previousouttotailor = TempData["PreviousOutToTailor"].ToString();
            if (TempData["PreviousOutToTailor"].ToString() != model.OutwardToTailorDetails.OutwardCode)
            {
                ViewData["OutwardToTailorChanged"] = previousouttotailor + " is replaced with " + model.OutwardToTailorDetails.OutwardCode + " because " + previousouttotailor + " is acquired by another person";
            }
            TempData["PreviousOutToTailor"] = previousouttotailor;

            return View(model);
        }

        //PRINT OUTWARD TO TAILOR
        [HttpGet]
        public ActionResult PrintOutwardToTailor(int id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.OutwardToTailorDetails = _OutwardToTailorService.GetDetailsById(id);
            model.OutwardToTailorItemList = _OutwardToTailorItemService.GetRowsByCode(model.OutwardToTailorDetails.OutwardCode);
            return View(model);
        }

    }
}

