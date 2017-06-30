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
    public class IncomeExpenseVoucherController : Controller
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

        private readonly IUtilityService _utilityService;
        private readonly IModuleService _ModuleService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IIncomeExpenseVoucherService _IncomeExchangeVoucherService;
        private readonly IEmployeeMasterService _EmployeeMasterService;

        public IncomeExpenseVoucherController(IJobWorkTypeService JobWorkTypeService, IUtilityService UtilityService, IModuleService ModuleService,
            IUserCredentialService UserCredentialService, IIncomeExpenseVoucherService IncomeExchangeVoucherService, IEmployeeMasterService EmployeeMasterService)
        {
            this._utilityService = UtilityService;
            this._ModuleService = ModuleService;
            this._IUserCredentialService = UserCredentialService;
            this._IncomeExchangeVoucherService = IncomeExchangeVoucherService;
            this._EmployeeMasterService = EmployeeMasterService;
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

        //CREATE INCOME EXPENSE VOUCHER ENTRY..
        [HttpGet]
        public ActionResult IncomeExpenseVoucherEntry()
        {
            MainApplication model = new MainApplication()
            {
                IncomeExpenseVoucherDetails = new IncomeExpenseVoucher(),
            };

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var details = _IncomeExchangeVoucherService.GetLastRowByFinYr(FinYr);
            string Code = string.Empty;

            int Typeval = 0;
            int length = 0;
            if (details != null)
            {
                Code = details.Code.Substring(4, 6);
                length = (Convert.ToInt32(Code) + 1).ToString().Length;
                Typeval = Convert.ToInt32(Code) + 1;
            }
            else
            {
                Typeval = 1;
                length = 1;
            }
            Code = _utilityService.getName("IEVE", length, Typeval);
            Code = Code + FinYr;
            model.IncomeExpenseVoucherDetails.Code = Code;

            //GET LOGIN USER NAME AND EMAIL
            var useremail = HttpContext.Session["UserEmail"].ToString();
            var empname = _EmployeeMasterService.getEmpByEmail(useremail).Name;
            Session["EmpName"] = empname;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //PARTIAL VIEW OF CASH PAYMENT DETAILS
        public ActionResult CashPayment()
        {
            return View();
        }

        //PARTIAL VIEW OF CARD PAYMENT DETAILS
        public ActionResult CardPayment()
        {
            return View();
        }

        //PARTIAL VIEW OF CHEQUE PAYMENT DETAILS
        public ActionResult ChequePayment()
        {
            return View();
        }

        //PARTIAL VIEW OF CASH CARD CHEQUE PAYMENT DETAILS
        public ActionResult CashCardChequePayment()
        {
            return View();
        }

        //SAVE INCOME EXCHNAGE VOUCHER ENTRY..
        [HttpPost]
        public ActionResult IncomeExpenseVoucherEntry(MainApplication mainapp, FormCollection frmcol)
        {
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var details = _IncomeExchangeVoucherService.GetLastRowByFinYr(FinYr);
            string Code = string.Empty;

            int Typeval = 0;
            int length = 0;
            if (details != null)
            {
                Code = details.Code.Substring(4, 6);
                length = (Convert.ToInt32(Code) + 1).ToString().Length;
                Typeval = Convert.ToInt32(Code) + 1;
            }
            else
            {
                Typeval = 1;
                length = 1;
            }
            Code = _utilityService.getName("IEVE", length, Typeval);
            Code = Code + FinYr;
            mainapp.IncomeExpenseVoucherDetails.Code = Code;

            mainapp.IncomeExpenseVoucherDetails.VoucherType = frmcol["VoucherType"];
            mainapp.IncomeExpenseVoucherDetails.Date = DateTime.Now;
            mainapp.IncomeExpenseVoucherDetails.PreparedByName = Session["EmpName"].ToString();
            mainapp.IncomeExpenseVoucherDetails.PreparedByEmail = HttpContext.Session["UserEmail"].ToString();
            mainapp.IncomeExpenseVoucherDetails.Status = "Active";
            mainapp.IncomeExpenseVoucherDetails.ModifiedOn = DateTime.Now;
            _IncomeExchangeVoucherService.Create(mainapp.IncomeExpenseVoucherDetails);
            return RedirectToAction("IncomeExpenseVoucherEntry");
        }
    }
}
