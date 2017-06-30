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
    public class DesignationController : Controller
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
        private readonly IDesignationMasterService _DesignationMasterService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;

        public DesignationController(IUtilityService UtilityService, IDesignationMasterService DesignationMasterService, IUserCredentialService UserCredentialService, IModuleService ModuleService)
        {

            this._UtilityService = UtilityService;
            this._DesignationMasterService = DesignationMasterService;
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
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

        // CREATE DESIGNATION
        public ActionResult CreateDesignation()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                DesignationDetails = new DesignationMaster(),
            };

            var details = _DesignationMasterService.getLastDesignation();
            int Designationval = 0;
            int length = 0;
            if (details != null)
            {
                Designationval = details.Id;
                Designationval = Designationval + 1;
                length = Designationval.ToString().Length;
            }
            else
            {
                Designationval = 1;
                length = 1;
            }
            String code = _UtilityService.getName("DSG", length, Designationval);
            model.DesignationDetails.DesignationCode = code;

            if (TempData["DesignationList"] != null)
            {
                ViewBag.error = "Designation Name Already Present";
            }
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //DESIGNATION NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetDesignation(string term)
        {
            var result = _DesignationMasterService.GetDesignationList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.DesignationName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateDesignation(MainApplication mainapp)
        {
            var details = _DesignationMasterService.getLastDesignation();
            int Designationval = 0;
            int length = 0;
            if (details != null)
            {
                Designationval = details.Id;
                Designationval = Designationval + 1;
                length = Designationval.ToString().Length;
            }
            else
            {
                Designationval = 1;
                length = 1;
            }
            String code = _UtilityService.getName("DSG", length, Designationval);
            mainapp.DesignationDetails.DesignationCode = code;

            var namelist = _DesignationMasterService.GetDesignationName(mainapp.DesignationDetails.DesignationName);
            if (namelist.Count() != 0)
            {
                TempData["DesignationList"] = "error";
                return RedirectToAction("CreateDesignation");
            }
            else
            {
                mainapp.DesignationDetails.Status = "Active";
                mainapp.DesignationDetails.ModifiedOn = DateTime.Now;
                _DesignationMasterService.Create(mainapp.DesignationDetails);
            }

            var id = _DesignationMasterService.getLastDesignation().Id;
            var DesignationId = Encode(id.ToString());
            return RedirectToAction("CreateDesignationDetails/" + DesignationId, "Designation");
        }

        //SHOW DESIGNATION DETAILS AFTER CREATE
        [HttpGet]
        public ActionResult CreateDesignationDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.DesignationDetails = _DesignationMasterService.getById(Id);
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        // EDIT DESIGNATION
        [HttpGet]
        public ActionResult EditDesignation()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Edit";
            return View(model);
        }

        //LOAD DESIGNATION LIST ONLOAD FUNCTION
        [HttpGet]
        public ActionResult LoadDesignations()
        {
            MainApplication model = new MainApplication();
            model.DesignationList = _DesignationMasterService.GetDesignations();
            TempData["DesignationList"] = model.DesignationList;
            return View(model);
        }

        //SEARCH DESIGNATION LIST
        [HttpGet]
        public ActionResult DesignationList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<DesignationMaster> ListOfDesignation = TempData["DesignationList"] as IEnumerable<DesignationMaster>;
            ListOfDesignation = ListOfDesignation.Where(x => x.Status == "Active");
            List<DesignationMaster> FinalList = new List<DesignationMaster>();
            FinalList = FinalList.Concat(ListOfDesignation.Where(x => x.DesignationName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.DesignationList = FinalList;
            TempData["DesignationList"] = FinalList;
            return View(model);
        }

        //PARTIAL VIEW FOR EDIT DESIGNATION
        [HttpGet]
        public ActionResult DesignationEditPartial(int id1)
        {
            MainApplication model = new MainApplication();
            model.DesignationDetails = _DesignationMasterService.GetId(id1);
            return View(model);
        }

        [HttpPost]
        public ActionResult DesignationEditPartial(DesignationMaster designation)
        {
            designation.Status = "Active";
            designation.ModifiedOn = DateTime.Now;
            _DesignationMasterService.Update(designation);
            return RedirectToAction("DesignationUpdateDetails/" + designation.Id, "Designation");
        }

        [HttpGet]
        public ActionResult DeleteDesignation()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        //PARTIAL VIEW FOR DELETE DESIGNATION
        [HttpGet]
        public ActionResult DesignationDeletePartial(int id2)
        {
            MainApplication model = new MainApplication();
            model.DesignationDetails = _DesignationMasterService.GetId(id2);
            return View(model);
        }

        [HttpPost]
        public ActionResult DesignationDeletePartial(DesignationMaster designation)
        {
            designation.Status = "InActive";
            designation.ModifiedOn = DateTime.Now;
            _DesignationMasterService.Update(designation);
            return RedirectToAction("DesignationUpdateDetails/" + designation.Id, "Designation");
        }

        //SHOW DESIGNATION DETAILS AFTER EDITDELETE
        [HttpGet]
        public ActionResult DesignationUpdateDetails(int id)
        {
            DesignationMaster designatn = new DesignationMaster();
            TempData["DesignationList"] = _DesignationMasterService.getAllDesignation();
            designatn = _DesignationMasterService.getById(id);
            return View(designatn);
        }


    }
}

