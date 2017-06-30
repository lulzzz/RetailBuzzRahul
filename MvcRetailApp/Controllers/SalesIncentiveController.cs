using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using CodeFirstEntities;
using CodeFirstData;
using MvcRetailApp.ModelBinder;
using CodeFirstServices.Interfaces;
using CodeFirstServices.Services;


namespace MvcRetailApp.Controllers
{
    public class SalesIncentiveController : Controller
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
        private readonly ISalesIncentiveService _salesincentiveSerive;
        private readonly IUnitService _utilitymasterService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public SalesIncentiveController(ISalesIncentiveService salesincentiveService, IUnitService utilitymasterService, IUserCredentialService usercredentialservice,IModuleService iIModuleService)
        {
            this._salesincentiveSerive = salesincentiveService;
            this._utilitymasterService = utilitymasterService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
        }

        [HttpGet]
        public ActionResult Create()
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

        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(SalesIncentiveCustumBinder))]SalesIncentiveMaster salesincentive)
        {
            _salesincentiveSerive.CreateSI(salesincentive);
            return RedirectToAction("CreateDetails/" + salesincentive.SIId, "SalesIncentive");
        }

        [HttpGet]
        public ActionResult CreateDetails(int id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.SalesIncentiveDetails = _salesincentiveSerive.GetSIById(id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();

            var SIList = _salesincentiveSerive.GetAllSI();
            string slabname = string.Empty;
            List<SalesIncentiveMaster> list = new List<SalesIncentiveMaster>();
            foreach (var item in SIList)
            {
                SalesIncentiveMaster slabData = new SalesIncentiveMaster();
                slabname = item.SISlabFrom + " - " + item.SISlabTo;
                slabData.SISlabFrom = slabname;
                slabData.SIId = item.SIId;
                list.Add(slabData);
                slabname = string.Empty;
            }
            model.SalesIncentiveList = list;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }


        [HttpGet]
        public JsonResult GetDetails(int Id)
        {
            var data = _salesincentiveSerive.GetSIById(Id);
            return Json(new { data.SIId, data.Percentage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(MainApplication model)
        {
            var details = _salesincentiveSerive.GetSIById(model.SalesIncentiveDetails.SIId);
            _salesincentiveSerive.DeleteSI(details);
            return RedirectToAction("Delete");
        }

    }
}
