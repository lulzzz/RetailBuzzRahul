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
    //[NoDirectAccess]
    [SessionExpireFilter]
    public class ColorCodeController : Controller
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
        private readonly IColorCodeService _ColorCodeService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;

        public ColorCodeController(IUtilityService UtilityService, IColorCodeService ColorCodeService, IUserCredentialService UserCredentialService, IModuleService ModuleService)
        {

            this._UtilityService = UtilityService;
            this._ColorCodeService = ColorCodeService;
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
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
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
               ColorCodeDetails = new ColorCode(),
            };

            var details = _ColorCodeService.GetLastColor();
            if (details == null)
            {
                model.ColorCodeDetails.colorCode = "01";
            }
            else
            {
                if (details.colorCode.StartsWith("0"))
                {
                    details.colorCode.Substring(1);
                    model.ColorCodeDetails.colorCode = (Convert.ToInt32(details.colorCode) + 1).ToString();
                    model.ColorCodeDetails.colorCode = "0" + model.ColorCodeDetails.colorCode;
                }
                else
                {
                    model.ColorCodeDetails.colorCode = (Convert.ToInt32(details.colorCode) + 1).ToString();
                }
            }
            
            if (TempData["ColorList"] != null)
            {
                ViewBag.error = "Color Name Already Present";
            }
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetColor(string term)
        {
            var result = _ColorCodeService.GetColorList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.colorName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MainApplication model)
        {
            var details = _ColorCodeService.GetLastColor();
            if (details == null)
            {
                model.ColorCodeDetails.colorCode = "01";
            }
            else
            {
                if (details.colorCode.StartsWith("0"))
                {
                    details.colorCode.Substring(1);
                    model.ColorCodeDetails.colorCode = (Convert.ToInt32(details.colorCode) + 1).ToString();
                    model.ColorCodeDetails.colorCode = "0" + model.ColorCodeDetails.colorCode;
                }
                else
                {
                    model.ColorCodeDetails.colorCode = (Convert.ToInt32(details.colorCode) + 1).ToString();
                }
            }
            var namelist = _ColorCodeService.GetColorName(model.ColorCodeDetails.colorName);
            if (namelist.Count() != 0)
            {
                TempData["ColorList"] = "error";
                return RedirectToAction("Create","ColorCode");
            }
            else
            {
                model.ColorCodeDetails.Status = "Active";
                model.ColorCodeDetails.modifiedOn = DateTime.Now;
                _ColorCodeService.Create(model.ColorCodeDetails);
            }

            var id = _ColorCodeService.GetLastColor().colorId;
            var colorId = Encode(id.ToString());
            return RedirectToAction("ColorDetails/" + colorId, "ColorCode");
        }

        [HttpGet]
        public ActionResult ColorDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.ColorCodeDetails = _ColorCodeService.GetColorCodebyId(Id);
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
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
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Edit";
            return View(model);
        }

        [HttpGet]
        public ActionResult LoadColorCodes()
        {
            MainApplication model = new MainApplication();
            model.ColorCodeList = _ColorCodeService.getAllColorCode();
            TempData["ColorList"] = model.ColorCodeList;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication();
            model.ColorCodeDetails = _ColorCodeService.GetColorCodebyId(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(ColorCode color)
        {
            color.Status = "Active";
            color.modifiedOn = DateTime.Now;
            _ColorCodeService.Update(color);
            return RedirectToAction("Details/" + color.colorId, "ColorCode");
        }

        [HttpGet]
        public ActionResult Delete()
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

        //PARTIAL VIEW FOR DELETE ITEM CATEGORIES
        [HttpGet]
        public ActionResult DeletePartial(int id)
        {
            MainApplication model = new MainApplication();
            model.ColorCodeDetails = _ColorCodeService.GetColorCodebyId(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(ColorCode color)
        {
            color.Status = "InActive";
            color.modifiedOn = DateTime.Now;
            _ColorCodeService.Update(color);
            return RedirectToAction("Details/" + color.colorId, "ColorCode");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            ColorCode color = new ColorCode();
            TempData["ColorList"] = _ColorCodeService.getAllColorCode();
            color = _ColorCodeService.GetColorCodebyId(id);
            return View(color);
        }

        [HttpGet]
        public ActionResult ColorList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<ColorCode> ListOfColors = TempData["ColorList"] as IEnumerable<ColorCode>;
            ListOfColors = ListOfColors.Where(x => x.Status == "Active");
            List<ColorCode> FinalList = new List<ColorCode>();
            FinalList = FinalList.Concat(ListOfColors.Where(x => x.colorName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ColorCodeList = FinalList;
            TempData["ColorList"] = FinalList;
            return View(model);
        }
    }
}
