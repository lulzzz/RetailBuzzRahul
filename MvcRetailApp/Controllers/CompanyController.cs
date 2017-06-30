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
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    public class CompanyController : Controller
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
        private readonly IBankNameService _bankNameService;
        private readonly IBankAccountTypeService _bankAccountTypeService;
        private readonly ICompanyService _companyService;
        private readonly IStateService _stateservice;
        private readonly IBankService _bankService;
        private readonly IUtilityService _utilityservice;
        private readonly ICityService _cityservice;
        private readonly ICompanyBankDetailsService _compbankservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IUserService _userService;
        private readonly IEmployeeMasterService _employeeMasterService;
        private readonly IEmployeesCompanyService _EmployeesCompanyService;

        public CompanyController(ICompanyBankDetailsService compbnkservice, ICityService cityservice, IStateService stateservice, IUtilityService utilityservice,
            IBankService bankservice, ICompanyService companyService, IBankAccountTypeService bankAccountTypeService, IBankNameService bankNameService,
            IUserCredentialService usercredentialservice, IModuleService iIModuleService, IUserService userService, IEmployeeMasterService employeeMasterService, IEmployeesCompanyService EmployeesCompanyService)
        {
            this._bankAccountTypeService = bankAccountTypeService;
            this._bankNameService = bankNameService;
            this._companyService = companyService;
            this._utilityservice = utilityservice;
            this._bankService = bankservice;
            this._stateservice = stateservice;
            this._cityservice = cityservice;
            this._compbankservice = compbnkservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._userService = userService;
            this._employeeMasterService = employeeMasterService;
            this._EmployeesCompanyService = EmployeesCompanyService;
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

        public int Decode(string decodeMe)
        {
            byte[] decoded = Convert.FromBase64String(decodeMe);
            string decodedvalue = System.Text.Encoding.UTF8.GetString(decoded);
            return Convert.ToInt32(decodedvalue);
        }

        public ActionResult ReloadCompany()
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            MainApplication model = new MainApplication()
            {
                companydetails = new Company(),
            };
            int Id = Decode(id);
            model.companydetails = _companyService.getById(Id);
            model.CompanyBankList = _compbankservice.getBankDetailsByCompanyCode(model.companydetails.CompanyCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = (string)HttpContext.Session["CompanyCode"];
            model.CompanyName = (string)HttpContext.Session["CompanyName"];
            model.FinancialYear = (string)HttpContext.Session["FinancialYear"];
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            MainApplication model = new MainApplication()
            {
                companydetails = new Company(),
            };
            string mssgbox = string.Empty;
            Company company = _companyService.getLastInsertedCompany();
            int lastid = 0;
            int length = 0;
            if (company != null)
            {
                lastid = company.companyId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("COM", length, lastid);
            model.companydetails.CompanyCode = catCode;
            model.companydetails.StateList = _stateservice.GetStateByCountry(1);
            var banklist = _compbankservice.getBankDetailsByCompanyCode(catCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            if (banklist.Count() != 0)
            {
                _compbankservice.ExecuteProcedure(catCode);
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult ValidateEmail(string mail)
        {
            string message = string.Empty;
            var companylist = _companyService.GetEmail(mail);
            if (companylist.Count() != 0)
            {
                message = "success";
            }
            else
            {
                message = "fail";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CreateNewCompany()
        {
            HttpContext.Session["CreateCompany"] = "Yes";
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBank(string term)
        {
            List<string> titles = new List<string>();
            titles = _companyService.GetBank(term);
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCityByState(int id)
        {
            var city = _cityservice.GetCityByState(id);
            Company cmpMaster = new Company();
            cmpMaster.CityList = city;
            var modelData = cmpMaster.CityList.Select(m => new SelectListItem()
            {
                Text = m.cityName,
                Value = m.cityId.ToString()
            });

            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MainApplication mainapp)
        {
            //DatabaseName = "RetailManagement1";
            //CompanyName = "RetailManagement1";
            //FinancialYear = "RetailManagement1";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            Int32 sid = Convert.ToInt32(mainapp.companydetails.State);
            var state = _stateservice.GetStateNamebyId(sid);
            Int32 cid = Convert.ToInt32(mainapp.companydetails.City);
            var city = _cityservice.GetCityNamebyId(cid);
            mainapp.companydetails.State = state;
            mainapp.companydetails.City = city;
            mainapp.companydetails.Replicated = "No";
            mainapp.companydetails.isEnabled = "Active";
            mainapp.companydetails.ModifiedOn = DateTime.Now;
            if (mainapp.compbankdetails.BankName != null)
            {
                mainapp.compbankdetails.CompanyCode = mainapp.companydetails.CompanyCode;
                mainapp.compbankdetails.Status = "Active";
                mainapp.compbankdetails.ModifiedOn = DateTime.Now;
                _compbankservice.CreateCompanyBankDetails(mainapp.compbankdetails);
            }

            var CompaniesCounter = _companyService.GetCompanyCounter("ValidationCompany");
            int compcount = 0;
            foreach (var company in CompaniesCounter)
            {
                if (company.CompanyName == mainapp.companydetails.companyName)
                {
                    compcount++;
                }
            }

            if (compcount != 0 || CompaniesCounter.Count() < 4)
            {
                _companyService.CreateCompany(mainapp.companydetails);
                string BackUpDbQuery = "BackUpDatabase";
                _companyService.BackUpDatabase(BackUpDbQuery);

                string dbname = string.Empty;
                string datafilepath = string.Empty;
                string logfilepath = string.Empty;
                string FromFinancialYear = Convert.ToDateTime(mainapp.companydetails.FinancialYearFrom).ToString("dd-MM-yyyy");
                string ToFinancialYear = Convert.ToDateTime(mainapp.companydetails.FinancialYearTo).ToString("dd-MM-yyyy");
                string FinancialYearDate = FromFinancialYear + " To " + ToFinancialYear;
                dbname = mainapp.companydetails.companyName + " " + FinancialYearDate;
                datafilepath = ConfigurationManager.AppSettings["CreateDatabase.mdfFile"].ToString() + dbname + ".mdf";
                logfilepath = ConfigurationManager.AppSettings["CreateDatabase.ldfFile"].ToString() + dbname + "_log.ldf";
               
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@dbname",dbname),
                    new SqlParameter("@datafilepath",datafilepath),
                    new SqlParameter("@logfilepath",logfilepath),
                };
                string query = "CreateDynamicDb" + " " + "@dbname" + "," + "@datafilepath" + "," + "@logfilepath";
                var list = _companyService.CreateDynamicDatabase(query, para);
                var usercompanies = _userService.GetUserByName((string)HttpContext.Session["UserName"]);
                EmployeesCompany EmpComp = new EmployeesCompany();
                EmpComp.UserCode = usercompanies.UserCode;
                EmpComp.CompanyCode = mainapp.companydetails.CompanyCode;
                _EmployeesCompanyService.Create(EmpComp);

                var details = _companyService.getLastInsertedCompany();
                string compid = Encode(details.companyId.ToString());
                return RedirectToAction("Details/" + compid, "Company");
            }
            else
            {
                TempData["CompanyExceed"] = "CompanyExceeded";
                return RedirectToAction("ReloadCompany");
            }
        }

        [HttpGet]
        public ActionResult CreateBankDetails()
        {
            MainApplication mainapp = new MainApplication()
               {
                   compbankdetails = new companybankdetail(),
               };
            mainapp.compbankdetails.listOfBankNames = _bankNameService.getAllBankNames();
            return View(mainapp);
        }

        [HttpPost]
        public ActionResult CreateBankDetails(string bankname, string branch, string companycode, string bankifscno, string bankaddress, string accountno,
             string accounttype, string micr)
        {
            companybankdetail data = new companybankdetail();
            data.BankName = bankname;
            data.Branch = branch;
            data.CompanyCode = companycode;
            data.BankIfsc = bankifscno;
            data.BankLocation = bankaddress;
            data.AccountNo = accountno;
            data.AccountType = accounttype;
            data.MICRCode = micr;
            data.Status = "Active";
            data.ModifiedOn = DateTime.Now;
            _compbankservice.CreateCompanyBankDetails(data);
            return RedirectToAction("CreateBankDetails");
        }

        [HttpGet]
        public JsonResult UpdatedBankDetails(string code)
        {
            MainApplication mainapp = new MainApplication()
            {
                compbankdetails = new companybankdetail(),
            };
            mainapp.compbankdetails.BnkList = _compbankservice.BankInfo(code);
            var modelData = mainapp.compbankdetails.BnkList.Select(m => new SelectListItem()
            {
                Value = m.BankId.ToString(),
                Text = m.BankName
            }
           );
            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateBankDetail(int id)
        {
            MainApplication model = new MainApplication()
            {
                compbankdetails = new companybankdetail(),
            };
            var bnklist = _compbankservice.getBankDetailsById(id);
            model.compbankdetails = bnklist;
            model.compbankdetails.listOfBankNames = _bankNameService.getAllBankNames();
            model.compbankdetails.BranchList = _bankService.GetBranchByBankName(model.compbankdetails.BankName);
            return View(model);
        }

        [HttpGet]
        public ActionResult BankPopUp(string companycode)
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            companybankdetail compbankdetails = new companybankdetail();
            compbankdetails.CompanyCode = companycode;
            compbankdetails.listOfBankNames = _bankNameService.getAllBankNames();
            return View(compbankdetails);
        }

        [HttpPost]
        public JsonResult BankPopUp(string branch, string bankname, string suppliercode, string bankifscno, string bankaddress, string accountno, string accounttype, string micr)
        {
            companybankdetail compbankdetails = new companybankdetail();
            compbankdetails.BankName = bankname;
            compbankdetails.Branch = branch;
            compbankdetails.CompanyCode = suppliercode;
            compbankdetails.BankIfsc = bankifscno;
            compbankdetails.BankLocation = bankaddress;
            compbankdetails.AccountNo = accountno;
            compbankdetails.AccountType = accounttype;
            if (micr == "")
            {
                compbankdetails.MICRCode = null;
            }
            else
            {
                compbankdetails.MICRCode = micr;
            }
            compbankdetails.Status = "Active";
            compbankdetails.ModifiedOn = DateTime.Now;
            _compbankservice.CreateCompanyBankDetails(compbankdetails);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBranch(string BankName)
        {
            MainApplication model = new MainApplication()
            {
                compbankdetails = new companybankdetail(),
            };
            model.compbankdetails.BranchList = _bankService.GetBranchByBankName(BankName);
            var modelData = model.compbankdetails.BranchList.Select(m => new SelectListItem()
            {
                Value = m.Branch,
                Text = m.Branch
            }
            );
            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetIFSCNoAndAddress(string branch)
        {
            string IFSCNo = _bankService.GetIfscByBranch(branch);
            string BankAddress = _bankService.GetAddressByIFSC(IFSCNo);
            string Micr = _bankService.GetMICRByIFSC(IFSCNo).MICRCode;
            return Json(new { IFSCNo, BankAddress, Micr }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateBankDetail(int BankId, string BankCode, string BankName, string ifsc, string BankLocation, string Accounttype, string Accountno, string branch, string micr)
        {
            MainApplication mainapp = new MainApplication()
            {
                compbankdetails = new companybankdetail(),
            };
            mainapp.compbankdetails.BankId = BankId;
            mainapp.compbankdetails.BankName = BankName;
            mainapp.compbankdetails.Branch = branch;
            mainapp.compbankdetails.BankIfsc = ifsc;
            mainapp.compbankdetails.BankLocation = BankLocation;
            mainapp.compbankdetails.CompanyCode = BankCode;
            mainapp.compbankdetails.AccountType = Accounttype;
            mainapp.compbankdetails.AccountNo = Accountno;
            if (micr == "")
            {
                mainapp.compbankdetails.MICRCode = null;
            }
            else
            {
                mainapp.compbankdetails.MICRCode = micr;
            }
            mainapp.compbankdetails.Status = "Active";
            mainapp.compbankdetails.ModifiedOn = DateTime.Now;
            _compbankservice.UpdateBank(mainapp.compbankdetails);
            return RedirectToAction("UpdateBankDetail", "Company", new { id = BankId });
        }

        [HttpGet]
        public ActionResult Edit()
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            MainApplication model = new MainApplication()
            {
                companydetails = new Company(),
            };

            IEnumerable<BankName> listOfBankNames = _bankNameService.getAllBankNames();
            IEnumerable<BankAccountType> listOfBankAccountType = _bankAccountTypeService.getAllBankAccountTypes();
            model.companydetails.BankNames = listOfBankNames;
            model.companydetails.BankAccountTypes = listOfBankAccountType;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditDetails(string data)
        {
            MainApplication main = new MainApplication()
            {
                companydetails = new Company(),
                compbankdetails = new companybankdetail(),
            };
            main.companydetails = _companyService.getCompanyByName(data);
            string code = main.companydetails.CompanyCode;
            main.compbankdetails.BnkList = _compbankservice.BankInfo(code);
            main.companydetails.StateList = _stateservice.GetStateByCountry(1);
            int id = _stateservice.GetStateIdByName(main.companydetails.State);
            main.companydetails.CityList = _cityservice.GetCityByState(id);
            string FromFinancialYear = Convert.ToDateTime(main.companydetails.FinancialYearFrom).ToString("dd-MM-yyyy");
            string ToFinancialYear = Convert.ToDateTime(main.companydetails.FinancialYearTo).ToString("dd-MM-yyyy");
            string FinancialYearDate = FromFinancialYear + " To " + ToFinancialYear;
            string olddbname = main.companydetails.companyName + " " + FinancialYearDate;
            TempData["OldDbName"] = olddbname;
            return View(main);
        }

        [HttpPost]
        public ActionResult EditDetails(Company cmpdetail)
        {
            cmpdetail.ModifiedOn = DateTime.Now;
            cmpdetail.isEnabled = "Active";
            _companyService.UpdateCompany(cmpdetail);
            string olddbname = TempData["OldDbName"].ToString();
            string newdbname = string.Empty;
            string FromFinancialYear = Convert.ToDateTime(cmpdetail.FinancialYearFrom).ToString("dd-MM-yyyy");
            string ToFinancialYear = Convert.ToDateTime(cmpdetail.FinancialYearTo).ToString("dd-MM-yyyy");
            string FinancialYearDate = FromFinancialYear + "-" + ToFinancialYear;
            newdbname = cmpdetail.companyName + " " + FinancialYearDate;
            //if (olddbname != newdbname)
            //{
            //    SqlParameter[] para = new SqlParameter[]
            //            {
            //                new SqlParameter("@olddbname",olddbname),
            //                new SqlParameter("@newdbname",newdbname),
            //            };
            //    string query = "UpdateDynamicDb" + " " + "@olddbname" + "," + "@newdbname";
            //    var list = _companyService.UpdateDynamicDatabase(query, para);
            //}
            return RedirectToAction("ViewCompanyDetails/" + cmpdetail.companyId, "Company");
        }

        [HttpGet]
        public ActionResult ViewCompanyDetails(int id)
        {
            MainApplication model = new MainApplication()
            {
                companydetails = new Company(),
            };
            model.companydetails = _companyService.getById(id);
            model.CompanyBankList = _compbankservice.getBankDetailsByCompanyCode(model.companydetails.CompanyCode);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetCompany(string term)
        {
            var result = _companyService.GetCompany(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.companyName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCompanyNamesWithCodes(string term)
        {
            var companies = _companyService.GetCompany(term);
            List<string> names = new List<string>();
            foreach (var company in companies)
            {
                names.Add(company.CompanyCode + " " + company.companyName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCompanyDetails(string code)
        {
            var companydetails = _companyService.GetCompanyDataByCompCode(code);
            int stateid = _stateservice.GetStateIdByName(companydetails.State);
            int cityid = _cityservice.GetCityIdByName(companydetails.City);
            string registrationdate = Convert.ToDateTime(companydetails.CompanyRegisterationDate).ToShortDateString();
            return Json(new
            {
                companydetails.address,
                registrationdate,
                companydetails.MailingAddress,
                companydetails.pincode,
                companydetails.ContactNo1,
                companydetails.ContactNo2,
                companydetails.ContactNo3,
                companydetails.eMail,
                stateid,
                cityid,
                companydetails.City,
                companydetails.panCard,
                companydetails.salesTaxNo,
                companydetails.vatNo,
                companydetails.website,
                companydetails.regNo
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidateCompanyName(string id)
        {
            Company cmp = _companyService.getCompanyByName(id);
            string msgbox = string.Empty;
            if (cmp != null)
            {
                msgbox = "Company Name is already register";
                return Json(msgbox, JsonRequestBehavior.AllowGet);
            }
            msgbox = "";
            return Json(msgbox, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SelectCompany()
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            MainApplication model = new MainApplication();
            var usercompanies = _userService.GetUserByName((string)HttpContext.Session["UserName"]);
            model.EmployeeCompanyList = _EmployeesCompanyService.GetAllEmployeeCompaniesByEmpCode(usercompanies.UserCode);
            List<Company> CompanyList = new List<Company>();
            foreach (var compname in model.EmployeeCompanyList)
            {
                var detail = _companyService.GetCompanyDataByCompCode(compname.CompanyCode);
                CompanyList.Add(detail);
            }
            model.CmpList = CompanyList;
            Session["CompanyList"] = _companyService.getAllCompanies();
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectCompany(FormCollection frm)
        {
            CompanyCode = frm["CompanyCode"];
            CompanyName = frm["CompanyName"];
            FinancialYear = frm["FinancialYear"];
            DatabaseName = CompanyName + " " + FinancialYear;
            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
}