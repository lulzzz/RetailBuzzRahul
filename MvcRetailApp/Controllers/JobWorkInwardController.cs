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
    public class JobWorkInwardController : Controller
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
        private readonly IInwardFromTailorService _InwardFromTailorService;
        private readonly IInwardFromTailorItemService _InwardFromTailorItemService;
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
        private readonly IJobWorkStockService _JobWorkStockService;
        private readonly IInwardToTailorService _InwardToTailorService;
        private readonly IInwardToTailorItemService _InwardToTailorItemService;

        public JobWorkInwardController(IUtilityService UtilityService, IModuleService ModuleService, IUserCredentialService IUserCredentialService, IInwardFromTailorService InwardFromTailorService, IInwardFromTailorItemService InwardFromTailorItemService,
            IOutwardToTailorService OutwardToTailorService, IOutwardToTailorItemService OutwardToTailorItemService, IClientMasterService ClientMasterService, IRetailBillService RetailBillService, IRetailBillItemService RetailBillItemService,
            ISalesBillService SalesBillService, ISalesBillItemService SalesBillItemService, IJobWorkTypeService JobWorkTypeService, IJobWorkerService JobWorkerService, IEmployeeMasterService EmployeeMasterService, IJobWorkStockService JobWorkStockService,
            IInwardToTailorService InwardToTailorService, IInwardToTailorItemService InwardToTailorItemService)
        {
            this._UtilityService = UtilityService;
            this._ModuleService = ModuleService;
            this._IUserCredentialService = IUserCredentialService;
            this._InwardFromTailorService = InwardFromTailorService;
            this._InwardFromTailorItemService = InwardFromTailorItemService;
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
            this._JobWorkStockService = JobWorkStockService;
            this._InwardToTailorService = InwardToTailorService;
            this._InwardToTailorItemService = InwardToTailorItemService;
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


        // ********************************** CRAETE INWARD FROM TAILOR TAILOR *************************************
        [HttpGet]
        public ActionResult InwardFromTailor()
        {
            MainApplication model = new MainApplication()
            {
                InwardFromTailorDetails = new InwardFromTailor(),
            };

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            string InwardCode = string.Empty;

            var details = _InwardFromTailorService.GetLastRowrByFinYr(FinYr);
            int val = 0;
            int length = 0;
            if (details != null)
            {
                InwardCode = details.InwardCode.Substring(4, 6);
                length = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                val = Convert.ToInt32(InwardCode) + 1;

                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            InwardCode = _UtilityService.getName("IWFT", length, val);
            InwardCode = InwardCode + FinYr;
            model.InwardFromTailorDetails.InwardCode = InwardCode;

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
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardToTailorNos(string term)
        {
            var data = _OutwardToTailorService.GetOutwardNoByInwardAndsTailorInwardStatus(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInwardDetails(string outward)
        {
            var data = _OutwardToTailorService.GetDetailsByCode(outward);
            return Json(new { data.TailorName, data.TailorAddress, data.TailorContact, data.ClientName, data.ClientAddress, data.ClientContact, data.GrandTotal, data.AdvancePayment, data.Balance }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY OUTWARD TO TAILOR DETAILS ON POP UP PAGE
        public ActionResult OutwardToTailorDetailsPop(string no)
        {
            MainApplication model = new MainApplication();
            model.OutwardToTailorDetails = _OutwardToTailorService.GetDetailsByCode(no);
            model.OutwardToTailorItemList = _OutwardToTailorItemService.GetRowsByCode(no);
            return View(model);
        }

        //GET OUTWARD TO TAILOR ITEMS
        public ActionResult GetOutwardToTailorItemDetails(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.OutwardToTailorItemList = _OutwardToTailorItemService.GetRowsByCode(OutwardNo);
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardFromTailor(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            string InwardCode = string.Empty;

            var details = _InwardFromTailorService.GetLastRowrByFinYr(FinYr);
            int val = 0;
            int length = 0;
            if (details != null)
            {
                InwardCode = details.InwardCode.Substring(4, 6);
                length = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                val = Convert.ToInt32(InwardCode) + 1;

                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            InwardCode = _UtilityService.getName("IWFT", length, val);
            InwardCode = InwardCode + FinYr;
            mainapp.InwardFromTailorDetails.InwardCode = InwardCode;

            var data = _OutwardToTailorService.GetDetailsByCode(mainapp.InwardFromTailorDetails.OutwardToTailorCode);
            if (data != null)
            {
                mainapp.InwardFromTailorDetails.OutwardToTailorCode = data.OutwardCode;
                mainapp.InwardFromTailorDetails.Date = DateTime.Now;
                mainapp.InwardFromTailorDetails.TailorName = data.TailorName;
                mainapp.InwardFromTailorDetails.TailorAddress = data.TailorAddress;
                mainapp.InwardFromTailorDetails.TailorContact = data.TailorContact;
                mainapp.InwardFromTailorDetails.TailorEmail = data.TailorEmail;
                mainapp.InwardFromTailorDetails.ClientName = data.ClientName;
                mainapp.InwardFromTailorDetails.ClientAddress = data.ClientAddress;
                mainapp.InwardFromTailorDetails.ClientContact = data.ClientContact;
                mainapp.InwardFromTailorDetails.ClientEmail = data.ClientEmail;
                mainapp.InwardFromTailorDetails.RetailBills = data.RetailBills;
                mainapp.InwardFromTailorDetails.SalesBills = data.SalesBills;
                mainapp.InwardFromTailorDetails.GrandTotal = data.GrandTotal;
                mainapp.InwardFromTailorDetails.Payment = data.AdvancePayment;
                mainapp.InwardFromTailorDetails.JobWorkStatus = frmcol["InwardType"];
                //save balance after tailor paid amount
                if(frmcol["InwardType"]=="SentToClient")
                {
                    mainapp.InwardFromTailorDetails.Balance = Convert.ToDouble(frmcol["BalanceValAfterTailorAmt"]);
                }
                else
                {
                mainapp.InwardFromTailorDetails.Balance = data.Balance;
                }
                mainapp.InwardFromTailorDetails.Status = "Active";
                mainapp.InwardFromTailorDetails.ModifiedOn = DateTime.Now;

                var username = HttpContext.Session["UserName"].ToString();
                //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                if (username != "SuperAdmin")
                {
                    mainapp.InwardFromTailorDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                    mainapp.InwardFromTailorDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                }
                else
                {
                    mainapp.InwardFromTailorDetails.ShopCode = "SuperAdmin";
                    mainapp.InwardFromTailorDetails.ShopName = "SuperAdmin";
                }
                _InwardFromTailorService.Create(mainapp.InwardFromTailorDetails);
            }

            if (frmcol["InwardType"] == "InwardToShop")
            {
                int count = Convert.ToInt32(frmcol["hdnRowCount"]);
                for (int i = 1; i <= count; i++)
                {
                    string item = "Item" + i;
                    string narration = "Narration" + i;
                    string quantity = "Quantity" + i;

                    if (frmcol[quantity] != null)
                    {
                        //save inward from tailor items..
                        mainapp.InwardFromTailorItemDetails = new InwardFromTailorItem();
                        mainapp.InwardFromTailorItemDetails.InwardCode = mainapp.InwardFromTailorDetails.InwardCode;
                        mainapp.InwardFromTailorItemDetails.Item = frmcol[item];
                        mainapp.InwardFromTailorItemDetails.Narration = frmcol[narration];
                        mainapp.InwardFromTailorItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        mainapp.InwardFromTailorItemDetails.Status = "Active";
                        mainapp.InwardFromTailorItemDetails.ModifiedOn = DateTime.Now;
                        _InwardFromTailorItemService.Create(mainapp.InwardFromTailorItemDetails);

                        //save job work stock items..
                        mainapp.JobWorkStockDetails = new JobWorkStock();
                        mainapp.JobWorkStockDetails.StockDate = DateTime.Now;
                        mainapp.JobWorkStockDetails.InwardCode = mainapp.InwardFromTailorDetails.InwardCode;
                        mainapp.JobWorkStockDetails.OutwardCode = mainapp.InwardFromTailorDetails.OutwardToTailorCode;
                        mainapp.JobWorkStockDetails.ItemName = frmcol[item];
                        mainapp.JobWorkStockDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        mainapp.JobWorkStockDetails.Narration = frmcol[narration];
                        mainapp.JobWorkStockDetails.Status = "Active";
                        mainapp.JobWorkStockDetails.ModifiedOn = DateTime.Now;

                        var username = HttpContext.Session["UserName"].ToString();
                        //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
                        if (username != "SuperAdmin")
                        {
                            mainapp.JobWorkStockDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                            mainapp.JobWorkStockDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
                        }
                        else
                        {
                            mainapp.JobWorkStockDetails.ShopCode = "SuperAdmin";
                            mainapp.JobWorkStockDetails.ShopName = "SuperAdmin";
                        }
                        _JobWorkStockService.Create(mainapp.JobWorkStockDetails);
                    }
                }
            }

            //UPDATE OUTWARD TO TAILOR AFTER INWARD
            var outwardtotailordetails = _OutwardToTailorService.GetDetailsByCode(mainapp.InwardFromTailorDetails.OutwardToTailorCode);
            outwardtotailordetails.InwardStatus = "InActive";
            if (frmcol["InwardType"] == "SentToClient")
            {
                //outwardtotailordetails.AdvancePayment = outwardtotailordetails.AdvancePayment + mainapp.InwardFromTailorDetails.PaidAmountToTailor;
                outwardtotailordetails.PaidAmountToTailor = mainapp.InwardFromTailorDetails.PaidAmountToTailor;
                outwardtotailordetails.Balance = mainapp.InwardFromTailorDetails.Balance;
                if (outwardtotailordetails.Balance == 0)
                {
                    outwardtotailordetails.Status = "InActive";
                }
            }
            _OutwardToTailorService.Update(outwardtotailordetails);

            var outwardtotailoritems = _OutwardToTailorItemService.GetRowsByCode(mainapp.InwardFromTailorDetails.OutwardToTailorCode);
            foreach (var item in outwardtotailoritems)
            {
                item.InwardStatus = "InActive";
                _OutwardToTailorItemService.Update(item);
            }

            return RedirectToAction("InwardFromTailor");
        }

        [HttpGet]
        public JsonResult GetEmployeeDetails(string Email)
        {
            var details = _EmployeeMasterService.getEmpByEmail(Email);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        // ********************************** CRAETE INWARD TO TAILOR TAILOR *************************************
        [HttpGet]
        public ActionResult InwardToTailor()
        {
            MainApplication model = new MainApplication()
            {
                InwardToTailorDetails = new InwardToTailor(),
            };

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            string InwardCode = string.Empty;

            var details = _InwardToTailorService.GetLastRowrByFinYr(FinYr);
            int val = 0;
            int length = 0;
            if (details != null)
            {
                InwardCode = details.InwardCode.Substring(4, 6);
                length = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                val = Convert.ToInt32(InwardCode) + 1;

                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            InwardCode = _UtilityService.getName("IWTT", length, val);
            InwardCode = InwardCode + FinYr;
            model.InwardToTailorDetails.InwardCode = InwardCode;

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
            return View(model);
        }

        //TAILOR NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetTailorNames(string term)
        {
            var data = _JobWorkerService.GetJobWorkerNames(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET OUTWARD TO TAILOT LIST BY TAILOR NAME
        [HttpGet]
        public ActionResult GetOutwardToTailorList(string Tailor)
        {
            MainApplication model = new MainApplication();
            model.OutwardToTailorList = _OutwardToTailorService.GetRowsByTailotName(Tailor);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetTailorDetailsByTailorName(string Tailor)
        {
            var data = _JobWorkerService.GetDetailsByName(Tailor);
            return Json(new { data.Address, data.Email, data.MobileNo}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InwardToTailor(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                InwardToTailorItemDetails = new InwardToTailorItem(),
            };
           
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string InwardCode = string.Empty;
            var details = _InwardToTailorService.GetLastRowrByFinYr(FinYr);
            int val = 0;
            int length = 0;
            if (details != null)
            {
                InwardCode = details.InwardCode.Substring(4, 6);
                length = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                val = Convert.ToInt32(InwardCode) + 1;

                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            InwardCode = _UtilityService.getName("IWTT", length, val);
            InwardCode = InwardCode + FinYr;
            mainapp.InwardToTailorDetails.InwardCode = InwardCode;

            mainapp.InwardToTailorDetails.Date = DateTime.Now;
            mainapp.InwardToTailorDetails.Status = "Active";
            mainapp.InwardToTailorDetails.ModifiedOn = DateTime.Now;

            var username = HttpContext.Session["UserName"].ToString();
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                mainapp.InwardToTailorDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                mainapp.InwardToTailorDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
            }
            else
            {
                mainapp.InwardToTailorDetails.ShopCode = "SuperAdmin";
                mainapp.InwardToTailorDetails.ShopName = "SuperAdmin";
            }
            _InwardToTailorService.Create(mainapp.InwardToTailorDetails);

             //save outward to tailor items..
            int count = Convert.ToInt32(frmcol["OutwardList"]);
            if (count != 0)
            {
                for (int i = 1; i < count; i++)
                {
                    string checkbox = "CheckBox" + i;
                    string outardno = "OutwardNo" + i;
                    string clientname = "ClientName" + i;
                    string tailoramt = "TailorAmount" + i;
                    string payment = "Payment" + i;
                    string balance = "Balance" + i;

                    if (frmcol[checkbox] == "Yes")
                    {
                        model.InwardToTailorItemDetails.InwardCode = mainapp.InwardToTailorDetails.InwardCode;
                        model.InwardToTailorItemDetails.OutwardToTailorCode = frmcol[outardno];
                        if (frmcol[clientname] != "")
                        {
                            model.InwardToTailorItemDetails.ClientName = frmcol[clientname];
                        }
                        else
                        {
                            model.InwardToTailorItemDetails.ClientName = null;
                        }

                        model.InwardToTailorItemDetails.Date = DateTime.Now;
                        model.InwardToTailorItemDetails.TailorName = mainapp.InwardToTailorDetails.TailorName;
                        model.InwardToTailorItemDetails.TailorAmount = Convert.ToDouble(frmcol[tailoramt]);
                        model.InwardToTailorItemDetails.AdvancePayment = Convert.ToDouble(frmcol[payment]);
                        model.InwardToTailorItemDetails.Balance = Convert.ToDouble(frmcol[balance]);
                        model.InwardToTailorItemDetails.Status = "Active";
                        model.InwardToTailorItemDetails.ModifiedOn = DateTime.Now;
                        _InwardToTailorItemService.Create(model.InwardToTailorItemDetails);

                        var outwarddetails = _OutwardToTailorService.GetDetailsByCode(model.InwardToTailorItemDetails.OutwardToTailorCode);
                        outwarddetails.TailorInwardStatus = "InActive";
                        _OutwardToTailorService.Update(outwarddetails);
                    }
                }
            }


            return RedirectToAction("InwardToTailor");
        }
    }
}