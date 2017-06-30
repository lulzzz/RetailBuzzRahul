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
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class ShopController : Controller
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
        private readonly IShopService _shopService;
        private readonly IEmployeeMasterService _employeeMasterService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IUtilityService _utilityService;

        public ShopController(IUtilityService utilityservice, IShopService shopService, IEmployeeMasterService employeeMasterService, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._shopService = shopService;
            this._employeeMasterService = employeeMasterService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._utilityService = utilityservice;
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

        //CREATE SHOP MASTER    
        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                ShopDetails = new ShopMaster(),
            };
            var shopmaster = _shopService.GetLastShop();
            int shopVal = 0;
            int length = 0;
            if (shopmaster != null)
            {
                shopVal = shopmaster.ShopId;
                shopVal = shopVal + 1;
                length = shopVal.ToString().Length;
            }
            else
            {
                shopVal = 1;
                length = 1;
            }
            string shopCode = _utilityService.getName("SH", length, shopVal);
            model.ShopDetails.ShopCode = shopCode;
            model.EmpList = _employeeMasterService.getAllemployees();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //SHOP NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetShopName(string term)
        {
            var result = _shopService.GetShopList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.ShopName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //GET EMPLOYEE DETAILS BY EMPLOYEE NAME
        [HttpGet]
        public JsonResult GetDetailsByEmpname(string EmpName)
        {
            var data = _employeeMasterService.getEmpByName(EmpName);
            return Json(new { data.Email, data.Designation }, JsonRequestBehavior.AllowGet);
        }

        //SAVE CREATE SHOP MASTER
        [HttpPost]
        public ActionResult Create(MainApplication mainapp)
        {
            var shopmaster = _shopService.GetLastShop();
            int shopVal = 0;
            int length = 0;
            if (shopmaster != null)
            {
                shopVal = shopmaster.ShopId;
                shopVal = shopVal + 1;
                length = shopVal.ToString().Length;
            }
            else
            {
                shopVal = 1;
                length = 1;
            }
            string shopCode = _utilityService.getName("SH", length, shopVal);

            mainapp.ShopDetails.ShopCode = shopCode;
            mainapp.ShopDetails.Status = "Active";
            mainapp.ShopDetails.ModifiedOn = DateTime.Now;
            _shopService.Create(mainapp.ShopDetails);

            //SAVE SHOP IN USERCREDENTIAL AND MODULES
            Module modulesdetails=new Module();
            UserCredential usercredentialdetails=new UserCredential();
            
            modulesdetails.ModuleName=mainapp.ShopDetails.ShopName;
            modulesdetails.ModuleRights="Add";
            modulesdetails.AssignRightsCode = shopCode;

            int LastRowPrimaryKey = _iIModuleService.GetLastRow().Id;
            int actualdashboardID = LastRowPrimaryKey + 1;
            modulesdetails.DashboardId = "D" + actualdashboardID;
            _iIModuleService.CreateModule(modulesdetails);

            var username = HttpContext.Session["UserName"].ToString();
            var lastmodulerow = _iIModuleService.GetLastRow();
            //IF SUPERADMIN CREATE THE SHOP THEN MAKE THE ENTRY FOR SUPERADMIN ONLY..
            if (username == "SuperAdmin")
            {
                usercredentialdetails.ModuleId = lastmodulerow.Id;
                usercredentialdetails.Modules = mainapp.ShopDetails.ShopName;
                usercredentialdetails.ModuleRights = "Add";
                usercredentialdetails.UseName = username;
                usercredentialdetails.Email = UserEmail;
                usercredentialdetails.Status = "add";
                _IUserCredentialService.CreateUserCredential(usercredentialdetails);

                string query = "GetUsersEmail";
                var list = _shopService.GetUserEmails(query);
                foreach (var data in list)
                {
                    var userdetails = _IUserCredentialService.GetByEmail(data.Email);
                    usercredentialdetails.ModuleId = lastmodulerow.Id;
                    usercredentialdetails.Modules = mainapp.ShopDetails.ShopName;
                    usercredentialdetails.ModuleRights = "Add";
                    usercredentialdetails.UseName = userdetails.UseName;
                    usercredentialdetails.Email = data.Email;
                    usercredentialdetails.Status = "";
                    usercredentialdetails.AssignRightsCode = shopCode;
                    _IUserCredentialService.CreateUserCredential(usercredentialdetails);
                }
            }
            //IF ANY OTHER EMPLOYEE CREATE THE SHOP THEN MAKE THE ENTRY FOR SUPERADMIN + EMPLOYEE..
            else
            {
                //IF EXCEPT SUPERADMIN ADD SHOP THEN IT WELL DISPLAY ALL EMPLOYEE IN USERCREDENTIAL
                string query = "GetUsersEmail";
                var list = _shopService.GetUserEmails(query);
                foreach(var data in list)
                {
                    var userdetails = _IUserCredentialService.GetByEmail(data.Email);
                    usercredentialdetails.ModuleId = lastmodulerow.Id;
                    usercredentialdetails.Modules = mainapp.ShopDetails.ShopName;
                    usercredentialdetails.ModuleRights = "Add";
                    usercredentialdetails.UseName = userdetails.UseName;
                    usercredentialdetails.Email = data.Email;
                    usercredentialdetails.Status = "";
                    usercredentialdetails.AssignRightsCode = shopCode;
                    _IUserCredentialService.CreateUserCredential(usercredentialdetails);
                }

                usercredentialdetails.AssignRightsCode = null;
                usercredentialdetails.Status = "add";
                usercredentialdetails.UseName = "SuperAdmin";
                usercredentialdetails.Email = "superadmin@gmail.com";
                _IUserCredentialService.CreateUserCredential(usercredentialdetails);

            }
            var lastrow = _shopService.GetLastShop();
            var ShopId = Encode(lastrow.ShopId.ToString());
            return RedirectToAction("CreateDetails/" + ShopId, "Shop");
        }

        //SHOW DETAILS AFTER CREATE SHOP
        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.ShopDetails = _shopService.GetById(Id);
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
        public ActionResult GetShopList()
        {
            MainApplication model = new MainApplication();
            model.ShopList = _shopService.GetAll();
            TempData["ShopList"] = model.ShopList;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetEmp(string name)
        {
            var details = _employeeMasterService.getEmpByName(name);
            return Json(new { details.Designation, details.Email }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetShopDetailsByName(string Code)
        {
            var details = _shopService.GetShopByCode(Code);
            return Json(new { details.ShopContactNo, details.ShopEmail, details.EmpName, details.ShopName, details.ShopAddress }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                ShopDetails = new ShopMaster(),
            };
            model.ShopDetails = _shopService.GetById(id);
            model.EmpList = _employeeMasterService.getAllemployees();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(ShopMaster shop)
        {
            shop.Status = "Active";
            shop.ModifiedOn = DateTime.Now;
            _shopService.Update(shop);
            TempData["ShopList"] = _shopService.GetAll();
            return RedirectToAction("ShopDetails/" + shop.ShopId, "Shop");
        }

        [HttpGet]
        public ActionResult DeletePartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                ShopDetails = new ShopMaster(),
            };

            model.ShopDetails = _shopService.GetById(id);
            TempData["ShopDetails"] = model.ShopDetails;
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(ShopMaster shop)
        {
            shop.Status = "InActive";
            shop.ModifiedOn = DateTime.Now;
            _shopService.Update(shop);
            return RedirectToAction("ShopDetails/" + Convert.ToInt32(shop.ShopId), "Shop");
        }

        [HttpGet]
        public ActionResult ShopDetails(int id)
        {
            MainApplication model = new MainApplication()
            {
                ShopDetails = new ShopMaster(),
            };

            if (TempData["ShopDetails"] != null)
            {
                model.ShopDetails = TempData["ShopDetails"] as ShopMaster;
            }
            else
            {
                model.ShopDetails = _shopService.GetById(id);
            }
            return View(model);

        }

        [HttpGet]
        public ActionResult ShopList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<ShopMaster> ListOfShops = TempData["ShopList"] as IEnumerable<ShopMaster>;
            ListOfShops = ListOfShops.Where(x => x.Status == "Active");
            List<ShopMaster> FinalList = new List<ShopMaster>();
            FinalList = FinalList.Concat(ListOfShops.Where(x => x.ShopName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ShopList = FinalList;
            TempData["ShopList"] = FinalList;
            return View(model);
        }

        [HttpGet]
        public JsonResult CheckShop(string ShopName)
        {
            var details = _shopService.GetShopDetailsByName(ShopName);
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
        public JsonResult CheckShortCode(string Code)
        {
            var details = _shopService.CheckShortCode(Code);
            string msg = string.Empty;
            if (details == null)
            {
                msg = "accept";
            }
            else
            {
                msg = "reject";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
