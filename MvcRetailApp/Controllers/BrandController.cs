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
    public class BrandController : Controller
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
        private readonly IBrandMasterService _BrandMasterService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;

        public BrandController(IUtilityService UtilityService, IBrandMasterService BrandMasterService, IUserCredentialService UserCredentialService, IModuleService ModuleService)
        {

            this._UtilityService = UtilityService;
            this._BrandMasterService = BrandMasterService;
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

        // CREATE ITEM CATEGORY
        public ActionResult CreateBrand()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                BrandMasterDetails = new BrandMaster(),
            };

            var details = _BrandMasterService.getLastBrand();
            int Brandval = 0;
            int length = 0;
            if (details != null)
            {
                Brandval = details.Id;
                Brandval = Brandval + 1;
                length = Brandval.ToString().Length;
            }
            else
            {
                Brandval = 1;
                length = 1;
            }
            String code = _UtilityService.getName("BRD", length, Brandval);
            model.BrandMasterDetails.BrandCode = code;

            if (TempData["BrandList"] != null)
            {
                ViewBag.error = "Brand Name Already Present";
            }
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //CATEGORY NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetBrand(string term)
        {
            var result = _BrandMasterService.GetBrandList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.BrandName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateBrand(MainApplication mainapp)
        {
            var details = _BrandMasterService.getLastBrand();
            int Brandval = 0;
            int length = 0;
            if (details != null)
            {
                Brandval = details.Id;
                Brandval = Brandval + 1;
                length = Brandval.ToString().Length;
            }
            else
            {
                Brandval = 1;
                length = 1;
            }
            String code = _UtilityService.getName("BRD", length, Brandval);
            mainapp.BrandMasterDetails.BrandCode = code;

            var namelist = _BrandMasterService.GetBrandName(mainapp.BrandMasterDetails.BrandName);
            if (namelist.Count() != 0)
            {
                TempData["BrandList"] = "error";
                return RedirectToAction("CreateBrand");
            }
            else
            {
                mainapp.BrandMasterDetails.Status = "Active";
                mainapp.BrandMasterDetails.ModifiedOn = DateTime.Now;
                _BrandMasterService.Create(mainapp.BrandMasterDetails);
            }

            var id = _BrandMasterService.getLastBrand().Id;
            var BrandId = Encode(id.ToString());
            return RedirectToAction("CreateBrandDetails/" + BrandId, "Brand");
        }

        //SHOW CATEGORY DETAILS AFTER CREATE
        [HttpGet]
        public ActionResult CreateBrandDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.BrandMasterDetails = _BrandMasterService.getById(Id);
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        // EDIT ITEM CATEGORY
        [HttpGet]
        public ActionResult EditBrand()
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

        //LOAD CATEGORY LIST ONLOAD FUNCTION
        [HttpGet]
        public ActionResult LoadBrands()
        {
            MainApplication model = new MainApplication();
            model.BrandMasterList = _BrandMasterService.GetBrands();
            TempData["BrandList"] = model.BrandMasterList;
            return View(model);
        }

        //SEARCH CATEGORIES LIST
        [HttpGet]
        public ActionResult BrandList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<BrandMaster> ListOfBrand = TempData["BrandList"] as IEnumerable<BrandMaster>;
            ListOfBrand = ListOfBrand.Where(x => x.Status == "Active");
            List<BrandMaster> FinalList = new List<BrandMaster>();
            FinalList = FinalList.Concat(ListOfBrand.Where(x => x.BrandName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.BrandMasterList = FinalList;
            TempData["BrandList"] = FinalList;
            return View(model);
        }

        //PARTIAL VIEW FOR EDIT ITEM CATEGORIES
        [HttpGet]
        public ActionResult BrandEditPartial(int id1)
        {
            MainApplication model = new MainApplication();
            model.BrandMasterDetails = _BrandMasterService.GetId(id1);
            return View(model);
        }

        [HttpPost]
        public ActionResult BrandEditPartial(BrandMaster brand)
        {
            brand.Status = "Active";
            brand.ModifiedOn = DateTime.Now;
            _BrandMasterService.Update(brand);
            return RedirectToAction("BrandUpdateDetails/" + brand.Id, "Brand");
        }

        [HttpGet]
        public ActionResult DeleteBrand()
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
        public ActionResult BrandDeletePartial(int id2)
        {
            MainApplication model = new MainApplication();
            model.BrandMasterDetails = _BrandMasterService.GetId(id2);
            return View(model);
        }

        [HttpPost]
        public ActionResult BrandDeletePartial(BrandMaster brand)
        {
            brand.Status = "InActive";
            brand.ModifiedOn = DateTime.Now;
            _BrandMasterService.Update(brand);
            return RedirectToAction("BrandUpdateDetails/" + brand.Id, "Brand");
        }

        //SHOW CATEGORY DETAILS AFTER EDITDELETE
        [HttpGet]
        public ActionResult BrandUpdateDetails(int id)
        {
            BrandMaster brnd = new BrandMaster();
            TempData["BrandList"] = _BrandMasterService.GetAllBrands();
            brnd = _BrandMasterService.getById(id);
            return View(brnd);
        }


    }
}

