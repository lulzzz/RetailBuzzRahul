using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
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
    public class GodownController : Controller
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
        private readonly IGodownService _godownService;
        private readonly IGodownAddressService _godownaddresService;
        private readonly ICityService _cityService;
        private readonly IEmployeeMasterService _employeemasterService;
        private readonly ICompanyService _componyService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public GodownController(IUtilityService utilityService, IGodownService godownService, IGodownAddressService godownaddressService, ICityService cityService, IEmployeeMasterService employeemasterService, ICompanyService componyService, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._utilityService = utilityService;
            this._godownService = godownService;
            this._godownaddresService = godownaddressService;
            this._cityService = cityService;
            this._employeemasterService = employeemasterService;
            this._componyService = componyService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
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
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                GodownDetails = new GodownMaster(),
            };
            var details = _godownService.getGodownLast();
            int lastid = 0;
            int length = 0;
            if (details != null)
            {
                lastid = details.GodownId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string Godowncode = _utilityService.getName("GD", length, lastid);
            _godownaddresService.DeleteGoDownAddr(Godowncode);
            model.GodownDetails.GdCode = Godowncode;
            //model.CityList = _cityService.getAllCities();

            var EmployeeList = _employeemasterService.getAllemployees();
            List<EmployeeMaster> emplist = new List<EmployeeMaster>();
            foreach (var item in EmployeeList)
            {
                EmployeeMaster empData = new EmployeeMaster();
                empData.Name = item.Name;
                empData.EmpId = item.EmpId;
                emplist.Add(empData);
            }
            model.EmpList = emplist;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //COMPONY NAME AUTO COMPLETE 
        [HttpGet]
        public JsonResult GetComponyNameList(string term)
        {
            var data = _componyService.GetComponyList(term);
            List<string> titles = new List<string>();
            foreach (var item in data)
            {
                titles.Add(item.companyName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //GODOWN NAME AUTO COMPLETE 
        [HttpGet]
        public JsonResult GetGodownNameList(string term)
        {
            var data = _godownService.GetGodownList(term);
            List<string> titles = new List<string>();
            foreach (var item in data)
            {
                titles.Add(item.GodownName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //GET GODOWN ADDRESS DETAILS
        [HttpGet]
        public ActionResult getGodownAddressDetails()
        {
            MainApplication model = new MainApplication()
            {
                GodownAddressDetails = new GodownAddress(),
            };
            model.CityList = _cityService.getAllCities();
            return View(model);
        }

        [HttpPost]
        public ActionResult getGodownAddressDetails(string gdcode, string godownLoc, string godownAddr, string area)
        {
            MainApplication model = new MainApplication()
            {
                GodownAddressDetails = new GodownAddress(),
            };

            GodownAddress godownAddrData = new GodownAddress();
            godownAddrData.Address = godownAddr;
            godownAddrData.Location = godownLoc;
            godownAddrData.GdCode = gdcode;
            godownAddrData.AreaName = area;
            _godownaddresService.CreateGoDown(godownAddrData);
            return RedirectToAction("getGodownAddressDetails", "Godown");
        }

        [HttpGet]
        public JsonResult GetDetailsByEmpname(int EmpId)
        {
            var data = _employeemasterService.getById(EmpId);
            return Json(new { data.Email, data.Designation }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetailByEmpname(string name)
        {
            var data = _employeemasterService.getEmpByName(name);
            return Json(new { data.Email, data.Designation }, JsonRequestBehavior.AllowGet);
        }

        //VALIDATE EMAIL
        [HttpGet]
        public JsonResult ValidateEmailId(string Email)
        {
            var EmailList = _godownService.GetEmailList(Email);
            string msg = string.Empty;
            if (EmailList.Count() != 0)
            {
                msg = "success";
            }
            else
            {
                msg = "fail";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MainApplication mainapp)
        {
            MainApplication model = new MainApplication()
            {
                GodownDetails = new GodownMaster(),
            };
            var details = _godownService.getGodownLast();
            int lastid = 0;
            int length = 0;
            if (details != null)
            {
                lastid = details.GodownId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string Godowncode = _utilityService.getName("GD", length, lastid);

            mainapp.GodownDetails.Status = "Active";
            mainapp.GodownDetails.ModifiedOn = DateTime.Now;
            int empid = Convert.ToInt32(mainapp.GodownDetails.EmpName);
            var emplist = _employeemasterService.getEmpById(empid);
            mainapp.GodownDetails.EmpName = emplist.Name;
            _godownService.CreateGodown(mainapp.GodownDetails);
            mainapp.GodownAddressDetails.GdCode = Godowncode;
            _godownaddresService.CreateGoDown(mainapp.GodownAddressDetails);
            var GodownDetails = _godownService.getGodownLast();

            //SAVE SHOP IN USERCREDENTIAL AND MODULES
            Module modulesdetails = new Module();
            UserCredential usercredentialdetails = new UserCredential();

            modulesdetails.ModuleName = mainapp.GodownDetails.GodownName;
            modulesdetails.ModuleRights = "Add";
            modulesdetails.AssignRightsCode = Godowncode;

            int LastRowPrimaryKey = _iIModuleService.GetLastRow().Id;
            int actualdashboardID = LastRowPrimaryKey + 1;
            modulesdetails.DashboardId = "D" + actualdashboardID;
            _iIModuleService.CreateModule(modulesdetails);

            var username = HttpContext.Session["UserName"].ToString();
            var lastmodulerow = _iIModuleService.GetLastRow();
            if (username == "SuperAdmin")
            {
                usercredentialdetails.ModuleId = lastmodulerow.Id;
                usercredentialdetails.Modules = mainapp.GodownDetails.GodownName;
                usercredentialdetails.ModuleRights = "Add";
                usercredentialdetails.UseName = username;
                usercredentialdetails.Email = UserEmail;
                usercredentialdetails.Status = "add";
                _IUserCredentialService.CreateUserCredential(usercredentialdetails);

                string query = "GetUsersEmail";
                var list = _godownService.GetUserEmails(query);
                foreach (var data in list)
                {
                    var userdetails = _IUserCredentialService.GetByEmail(data.Email);
                    usercredentialdetails.ModuleId = lastmodulerow.Id;
                    usercredentialdetails.Modules = mainapp.GodownDetails.GodownName;
                    usercredentialdetails.ModuleRights = "Add";
                    usercredentialdetails.UseName = userdetails.UseName;
                    usercredentialdetails.Email = data.Email;
                    usercredentialdetails.Status = "";
                    usercredentialdetails.AssignRightsCode = Godowncode;
                    _IUserCredentialService.CreateUserCredential(usercredentialdetails);
                }
            }
            else
            {
                //IF EXCEPT SUPERADMIN ADD GODOWN THEN IT WELL DISPLAY ALL EMPLOYEE IN USERCREDENTIAL
                string query = "GetUsersEmail";
                var list = _godownService.GetUserEmails(query);
                foreach(var data in list)
                {
                    var userdetails = _IUserCredentialService.GetByEmail(data.Email);
                    usercredentialdetails.ModuleId = lastmodulerow.Id;
                    usercredentialdetails.Modules =  mainapp.GodownDetails.GodownName;
                    usercredentialdetails.ModuleRights = "Add";
                    usercredentialdetails.UseName = userdetails.UseName;
                    usercredentialdetails.Email = data.Email;
                    usercredentialdetails.Status = "";
                    usercredentialdetails.AssignRightsCode = Godowncode;
                    _IUserCredentialService.CreateUserCredential(usercredentialdetails);

                }
                usercredentialdetails.AssignRightsCode = null;
                usercredentialdetails.Status = "add";
                usercredentialdetails.UseName = "SuperAdmin";
                usercredentialdetails.Email = "superadmin@gmail.com";
                _IUserCredentialService.CreateUserCredential(usercredentialdetails);

            }
            var Id = _godownService.getGodownLast().GodownId;
            var GodownId = Encode(Id.ToString());
            return RedirectToAction("CreateDetails/" + GodownId, "Godown");
        }

        //SHOW PAGE AFTER CREATE
        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                GodownDetails = new GodownMaster(),
            };
            int Id = Decode(id);
            model.GodownDetails = _godownService.GetGodownDetailsById(Id);
            model.GodownAddressList = _godownaddresService.GetAddressList(model.GodownDetails.GdCode);
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
        public ActionResult GetGodown()
        {
            MainApplication model = new MainApplication();
            model.GodownMasterList = _godownService.GetGodownNames();
            TempData["GodownList"] = model.GodownMasterList;  //For Search Button
            return View(model);
        }

        [HttpGet]
        public ActionResult GetGodownForDelete()
        {
            MainApplication model = new MainApplication();
            model.GodownMasterList = _godownService.GetGodownNames();
            TempData["GodownList"] = model.GodownMasterList;  //For Search Button
            return View(model);
        }


        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                GodownDetails = new GodownMaster(),
            };
            model.GodownDetails = _godownService.GetGodownDetailsById(id);
            model.GodownAddressList = _godownaddresService.GetAddressList(model.GodownDetails.GdCode);
            model.CityList = _cityService.getAllCities();
            model.EmpList = _employeemasterService.getAllemployees();
            return View(model);
        }


        [HttpGet]
        public ActionResult PopUpPage(string gdcode)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.CityList = _cityService.getAllCities();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(GodownMaster godown)
        {
            godown.Status = "Active";
            godown.ModifiedOn = DateTime.Now;
            _godownService.UpdateGodown(godown);
            TempData["GodownList"] = _godownService.GetGodownNames();
            return RedirectToAction("GodownDetails/" + godown.GodownId, "Godown");
        }

        [HttpGet]
        public ActionResult DeletePartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                GodownDetails = new GodownMaster(),
            };

            model.GodownDetails = _godownService.GetGodownDetailsById(id);
            TempData["GodownDetails"] = model.GodownDetails;
            model.GodownAddressList = _godownaddresService.GetAllAddressesByCode(model.GodownDetails.GdCode);
            TempData["AddressesList"] = model.GodownAddressList;
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(GodownMaster Godown)
        {
            Godown.Status = "InActive";
            Godown.ModifiedOn = DateTime.Now;
            _godownService.UpdateGodown(Godown);
            var bnklist = _godownaddresService.GetAllAddressesByCode(Godown.GdCode);
            foreach (var item in bnklist)
            {
                _godownaddresService.UpdateGodown(item);

            }
            return RedirectToAction("GodownDetails/" + Convert.ToInt32(Godown.GodownId), "Godown");
        }

        [HttpGet]
        public ActionResult GodownDetails(int id)
        {
            MainApplication model = new MainApplication()
            {
                GodownDetails = new GodownMaster(),
                GodownAddressDetails = new GodownAddress(),
            };
            if (TempData["GodownDetails"] != null)
            {
                model.GodownDetails = TempData["GodownDetails"] as GodownMaster;
                model.GodownAddressList = TempData["AddressesList"] as IEnumerable<GodownAddress>;
            }
            else
            {
                model.GodownDetails = _godownService.GetGodownDetailsById(id);
                model.GodownAddressList = _godownaddresService.GetAllAddressesByCode(model.GodownDetails.GdCode);
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetAddressesByArea(string areaname)
        {
            var details = _godownaddresService.GetAddressByArea(areaname);
            return Json(new { details.AddressId, details.Location, details.Address }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdateAddresses(int id, string area, string location, string address, string code)
        {
            GodownAddress details = new GodownAddress();
            details.AddressId = id;
            details.AreaName = area;
            if (location == "")
            {
                details.Location = null;
            }
            else
            {
                details.Location = location;
            }
            details.Address = address;
            details.GdCode = code;
            _godownaddresService.UpdateGodown(details);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CreateAddress(string Location, string Address, string AreaName, string GdCode)
        {
            GodownAddress details = new GodownAddress();
            details.GdCode = GdCode;
            details.AreaName = AreaName;
            details.Address = Address;
            if (Location == "")
            {
                details.Location = null;
            }
            else
            {
                details.Location = Location;
            }
            _godownaddresService.CreateGoDown(details);
            return Json(GdCode, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllAddresses(string GdCode)
        {
            var details = _godownaddresService.GetAllAddressesByCode(GdCode);
            var select = details.Select(m => new SelectListItem
            {
                Value = m.AreaName,
                Text = m.AreaName,
            });
            return Json(select, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckGodown(string GdName)
        {
            var details = _godownService.GetGodownDetailsByName(GdName);
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
        public ActionResult GodownSearch(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<GodownMaster> ListOfGodowns = TempData["GodownList"] as IEnumerable<GodownMaster>;
            ListOfGodowns = ListOfGodowns.Where(x => x.Status == "Active");
            List<GodownMaster> FinalList = new List<GodownMaster>();
            FinalList = FinalList.Concat(ListOfGodowns.Where(x => x.GodownName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.GodownMasterList = FinalList;
            TempData["GodownList"] = FinalList;
            return View(model);
        }

        [HttpGet]
        public JsonResult CheckShortCode(string Code)
        {
            var details = _godownService.CheckShortCode(Code);
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