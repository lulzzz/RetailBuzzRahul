using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstData;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using MvcRetailApp.ModelBinder;
using CodeFirstServices.Interfaces;
using CodeFirstServices.Services;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class TypeOfMaterialController : Controller
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
        private readonly ITypeOfMaterialService _TypeOfMaterialService;
        private readonly IUtilityService _utilityService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IColorCodeService _ColorCodeService;

        public TypeOfMaterialController(ITypeOfMaterialService TypeOfMaterialService, IUtilityService utilityService, IUserCredentialService usercredentialservice, IModuleService iIModuleService,IColorCodeService ColorCodeService)
        {
            this._TypeOfMaterialService = TypeOfMaterialService;
            this._utilityService = utilityService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._ColorCodeService = ColorCodeService;
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

        //CREATE TYPE OF MATERIAL
        [HttpGet]
        public ActionResult Create()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                MaterialDetails = new TypeOfMaterial(),
            };
            var details = _TypeOfMaterialService.GetMaterialLast();
            int matval = 0;
            int length = 0;
            if (details != null)
            {
                matval = details.MaterialId;
                matval = matval + 1;
                length = matval.ToString().Length;
            }
            else
            {
                matval = 1;
                length = 1;
            }
            String MaterialCode = _utilityService.getName("TM", length, matval);
            model.MaterialDetails.MaterialCode = MaterialCode;

            if (TempData["MaterialList"] != null)
            {
                ViewBag.error = "Material Name Already Created";
            }

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

       
        //CREATE MATERIAL
        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(TypeOfMaterialCustomBinder))]TypeOfMaterial typeofmaterial)
        {
            MainApplication model = new MainApplication();
            model.TypeOfMaterialData = new TypeOfMaterial();
            var details = _TypeOfMaterialService.GetMaterialLast();
            int matval = 0;
            int length = 0;
            if (details != null)
            {
                matval = details.MaterialId;
                matval = matval + 1;
                length = matval.ToString().Length;
            }
            else
            {
                matval = 1;
                length = 1;
            }
            String MaterialCode = _utilityService.getName("TM", length, matval);
            model.TypeOfMaterialData.MaterialCode = MaterialCode;

            var materiallist = _TypeOfMaterialService.GetMaterialNames(typeofmaterial.MaterialName);
            if (materiallist.Count() != 0)
            {
                TempData["MaterialList"] = "error";
                return RedirectToAction("Create");
            }
            else
            {
                _TypeOfMaterialService.AddTypeOfMaterial(typeofmaterial);
            }

            var id = _TypeOfMaterialService.GetMaterialLast().MaterialId;
            var MaterialId = Encode(id.ToString());
            return RedirectToAction("CreateDetails/" + MaterialId, "TypeOfMaterial");
        }

        //SHOW CREATE MATERIAL
        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.MaterialDetails = _TypeOfMaterialService.GetMaterialDetailById(Id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //CODE FOR AUTO COMPLETE OF MATERIAL NAME
        [HttpGet]
        public JsonResult GetMaterialList(string term)
        {
            var result = _TypeOfMaterialService.GetMaterialforList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.MaterialName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //EDIT MATERIAL
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

        //LOAD MATERIAL LIST ONLOAD FUNCTION
        [HttpGet]
        public ActionResult LoadMaterial()
        {
            MainApplication model = new MainApplication();
            model.MaterialList = _TypeOfMaterialService.GetMaterialList();
            TempData["MaterialList"] = model.MaterialList;
            return View(model);
        }

        //SEARCH MATERIAL LIST
        [HttpGet]
        public ActionResult MaterialList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<TypeOfMaterial> ListOfMaterial = TempData["MaterialList"] as IEnumerable<TypeOfMaterial>;
            ListOfMaterial = ListOfMaterial.Where(x => x.Status == "Active");
            List<TypeOfMaterial> FinalList = new List<TypeOfMaterial>();
            FinalList = FinalList.Concat(ListOfMaterial.Where(x => x.MaterialName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.MaterialList = FinalList;
            TempData["MaterialList"] = FinalList;
            return View(model);
        }

        //EDIT PARTIAL VIEW
        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication();
            model.MaterialDetails = _TypeOfMaterialService.GetMaterialDetailById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditPartial(TypeOfMaterial typeofmtrl)
        {
            typeofmtrl.Status = "Active";
            typeofmtrl.ModifiedOn = DateTime.Now;
            _TypeOfMaterialService.UpdateTypeOfMaterial(typeofmtrl);
            return RedirectToAction("UpdateDetails/" + typeofmtrl.MaterialId, "TypeOfMaterial");
        }

        //DELETE PARTIAL VIEW
        [HttpGet]
        public ActionResult DeletePartial(int id)
        {
            MainApplication model = new MainApplication();
            model.MaterialDetails = _TypeOfMaterialService.GetMaterialDetailById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult DeletePartial(TypeOfMaterial typeofmtrl)
        {
            typeofmtrl.Status = "InActive";
            typeofmtrl.ModifiedOn = DateTime.Now;
            _TypeOfMaterialService.UpdateTypeOfMaterial(typeofmtrl);
            return RedirectToAction("UpdateDetails/" + typeofmtrl.MaterialId, "TypeOfMaterial");
        }

        //SHOW MATERIAL DETAILS AFTER EDITDELETE
        [HttpGet]
        public ActionResult UpdateDetails(int id)
        {
            TypeOfMaterial material = new TypeOfMaterial();
            material = _TypeOfMaterialService.GetMaterialDetailById(id);
            TempData["MaterialList"] = _TypeOfMaterialService.getAllTypeOfMaterial();
            return View(material);
        }

        [HttpGet]
        public JsonResult GetColorList(string term)
        {
            var result = _ColorCodeService.GetColorList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.colorName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

    }
}
