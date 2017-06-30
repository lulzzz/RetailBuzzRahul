using CodeFirstData;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class JobWorkMisReportsController : Controller
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

        private readonly IJobWorkStockService _JobWorkStockService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;
        private readonly IOutwardToTailorService _OutwardToTailorService;
        private readonly IJobWorkerService _JobWorkerService;
        private readonly IInwardToTailorService _InwardToTailorService;
        private readonly IInwardToTailorItemService _InwardToTailorItemService;

        public JobWorkMisReportsController(IJobWorkStockService JobWorkStockService, IUserCredentialService UserCredentialService, IModuleService ModuleService,
            IOutwardToTailorService OutwardToTailorService, IJobWorkerService JobWorkerService,IInwardToTailorService InwardToTailorService, IInwardToTailorItemService InwardToTailorItemService)
        {
            this._JobWorkStockService = JobWorkStockService;
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
            this._OutwardToTailorService = OutwardToTailorService;
            this._JobWorkerService = JobWorkerService;
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

        // ************************ JOB WORK STOCK REPORT *******************************************************
        [HttpGet]
        public ActionResult JobWorkStockReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //JOB WORK STOCK REPORT DATEWISE
        [HttpGet]
        public ActionResult JobWorkStockReportDatewise()
        {
            return View();
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult GetJobWorkStockDetailsByDate(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime TDate = Convert.ToDateTime(ToDate);
            DateTime FDate = Convert.ToDateTime(FromDate);
            model.JobWorkStockList = _JobWorkStockService.GetReportByDate(FDate, TDate);
            TempData["JobWorkStockListDatewise"] = model.JobWorkStockList;
            return View(model);
        }

        //PRINT PAGE OFJOB WORK STOCK REPORT DATEWISE
        [HttpGet]
        public ActionResult PrintJobWorkStockDatewise()
        {
            MainApplication model = new MainApplication();
            model.JobWorkStockList = TempData["JobWorkStockListDatewise"] as IEnumerable<JobWorkStock>;
            TempData["JobWorkStockListDatewise"] = model.JobWorkStockList;
            return View(model);
        }

        //JOB WORK STOCK REPORT DATEWISE
        [HttpGet]
        public ActionResult JobWorkStockReportOutwardwise()
        {
            return View();
        }

        //AUTO COMPLETE OUTWARD TO TAILOR NO
        [HttpGet]
        public JsonResult GetOutwardToTailorNos(string term)
        {
            var data = _OutwardToTailorService.GetOutwardNoByInActiveInwardStatus(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult GetJobWorkStockDetailsByOutward(string outward)
        {
            MainApplication model = new MainApplication();
            model.JobWorkStockList = _JobWorkStockService.GetActiveRowsByOutwardNo(outward);
            TempData["JobWorkStockListOutwardwise"] = model.JobWorkStockList;
            return View(model);
        }

        //PRINT PAGE OF PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult PrintJobWorkStockOutwardwise()
        {
            MainApplication model = new MainApplication();
            model.JobWorkStockList = TempData["JobWorkStockListOutwardwise"] as IEnumerable<JobWorkStock>;
            TempData["JobWorkStockListOutwardwise"] = model.JobWorkStockList;
            return View(model);
        }

        // ************************ INWARD TO TAILOR REPORT *******************************************************

        [HttpGet]
        public ActionResult JobWorkerReport()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.JobWorkerList = _JobWorkerService.GetActiveJobWorkers();
            return View(model);
        }

        //PARTIAL VIEW FOR DISPLAY ITEM DETAILS IN PURCHASE ORDER REPORT DATEWISE
        [HttpGet]
        public ActionResult GetJobWorkerDetailsByTailor(string Tailor, string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            var FDate = Convert.ToDateTime(FromDate);
            var TDate = Convert.ToDateTime(ToDate);
            List<InwardToTailorItem> inwardtotailoritemlist = new List<InwardToTailorItem>();
            model.OutwardToTailorList = _OutwardToTailorService.GetReportByTailorNameAndDate(Tailor, FDate, TDate);
            return View(model);
        }
    }
}
