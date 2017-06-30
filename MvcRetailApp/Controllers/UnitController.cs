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
using System.IO;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class UnitController : Controller
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
        private readonly IUnitService _unitservice;
        private readonly IUtilityService _utilityservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IUnitConversionService _UnitConversionService;

        public UnitController(IUnitService unitservice, IUtilityService utilityservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IUnitConversionService UnitConversionService)
        {
            this._unitservice = unitservice;
            this._utilityservice = utilityservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._UnitConversionService = UnitConversionService;
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
        public ActionResult Create()
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
        public JsonResult GetSizeDetails(string UnitCodes)
        {
            UnitMaster size = new UnitMaster();
            size = _unitservice.getBycode(UnitCodes);
            int id = size.UnitId;
            string name = size.UnitName;
            string code = size.UnitCode;
            return Json(new
            {
                Id = id,
                name = name,
                code = code,

            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(UnitCustomBinder))] UnitMaster unitmaster)
        {
            MainApplication model = new MainApplication();
            var details = _unitservice.getLastInserted();
            int catVal = 0;
            int length = 0;
            if (details != null)
            {
                catVal = details.UnitId;
                catVal = catVal + 1;
                length = catVal.ToString().Length;
            }
            else
            {
                catVal = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("UNT", length, catVal);
            unitmaster.UnitCode = catCode;
            _unitservice.createunit(unitmaster);
            var id = _unitservice.getLastInserted().UnitId;
            var UnitId = Encode(id.ToString());
            return RedirectToAction("CreateDetails/" + UnitId, "Unit");
        }

        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.UnitDetails = _unitservice.getById(Id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["Type"] = "edit";
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["Type"] = "delete";
            return View(model);
        }

        [HttpGet]
        public ActionResult GetUnitNameList()
        {
            MainApplication model = new MainApplication();
            model.UnitList = _unitservice.getallsize();
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(int Id)
        {
            MainApplication model = new MainApplication();
            model.UnitDetails = _unitservice.getById(Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(UnitMaster untmastr)
        {
            untmastr.status = "Active";
            untmastr.modifiedOn = DateTime.Now;
            _unitservice.updateunit(untmastr);
            return RedirectToAction("ShowUnit/" + untmastr.UnitId, "Unit");
        }


        [HttpGet]
        public ActionResult DeletePartial(int Id)
        {
            MainApplication model = new MainApplication();
            model.UnitDetails = _unitservice.getById(Id);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(UnitMaster untmastr)
        {
            untmastr.status = "InActive";
            untmastr.modifiedOn = DateTime.Now;
            _unitservice.updateunit(untmastr);
            return RedirectToAction("ShowUnit/" + untmastr.UnitId, "Unit");
        }

        [HttpGet]
        public ActionResult ShowUnit(int id)
        {
            MainApplication model = new MainApplication();
            model.UnitDetails = _unitservice.getById(id);
            return View(model);
        }

        [HttpGet]
        public JsonResult CheckUnit(string Unit)
        {
            var details = _unitservice.GetDetailsByName(Unit);
            string msg = string.Empty;
            if (details != null)
            {
                msg = "true";
            }
            else
            {
                msg = "false";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnitValue(string fromunit, string tounit)
        {
            string value = "1";
            var data = _UnitConversionService.GetUnitValue(fromunit, tounit);
            if (data != null)
            {
                value = data.Value;
            }
            string numbertype = _unitservice.GetDetailsByName(tounit).NumberType;
            return Json(new { value, numbertype }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnits()
        {
            var unitlist = _unitservice.getallsize();
            var utlist = unitlist.Select(m => new SelectListItem
            {
                Text = m.UnitName,
                Value = m.UnitName,
            });
            return Json(utlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnitList(string term)
        {
            var result = _unitservice.GetUnitList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.UnitName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetailsByUnitName(string Name)
        {
            var details = _unitservice.GetDetailsByName(Name);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
    }
}