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
    public class BankController : Controller
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
        private readonly IBankService _bankService;
        private readonly IBankNameService _bankNameService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public BankController(IUtilityService utilityService, IBankService bankService, IBankNameService bankNameService, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._utilityService = utilityService;
            this._bankService=bankService;
            this._bankNameService = bankNameService;
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
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE BANK NAME
        [HttpGet]
        public JsonResult GetBankNames(string term)
        {
            var result = _bankNameService.GetBankNames(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.bankName);
            }
            return Json(names,JsonRequestBehavior.AllowGet);
        }

        //IFSC VALIDITY
        [HttpGet]
        public JsonResult ValidateIFSCNo(string ifscno)
        {
            var ifsnolist = _bankService.GetIFSCList(ifscno);
            string msg = string.Empty;
            if (ifsnolist.Count() != 0)
            {
                msg = "success";
            }
            else 
            {
                msg = "fail";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        //EMAIL VALIDITY
        [HttpGet]
        public JsonResult ValidateEmail(string email)
        {
            var Emaillist = _bankService.GetEmailList(email);
            string msg = string.Empty;
            if (Emaillist.Count() != 0)
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
        public ActionResult Create([ModelBinder(typeof(BankCustomBinder))]BankMaster bankmaster)
        {
            //insert bank name in bank names
            BankName bank = new BankName();
            bank.bankName = bankmaster.bankName;
            _bankNameService.CreateBankName(bank);

            if (bankmaster.MICRCode == "")
            {
                bankmaster.MICRCode = null;
            }

            _bankService.CreateBank(bankmaster);
            var bankdata = _bankService.getLastInsertedBank();
            var BankId = Encode(bankdata.bankId.ToString());
            return RedirectToAction("CreateDetails/" + BankId, "Bank");

        }

        //SHOW PAGE AFTER CREATE
        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.BankDetails = _bankService.getById(Id);
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
            return View(model);
        }

        //DISPLAY BRANCH LIST IN PARTIAL VIEW
        [HttpGet]
        public ActionResult LoadBranchesByBankName(string name)
        {
            MainApplication model = new MainApplication();
            model.BankList = _bankService.GetBranchFromBankName(name);
            TempData["BranchList"] = model.BankList;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(int Id)
        {
            MainApplication model = new MainApplication();
            model.BankDetails = _bankService.GetBankDetailsById(Id);
            return View(model);
        }

        //DISPLAY LIST AFTER SEARCH
        [HttpGet]
        public ActionResult BranchList(string name)
        {
            MainApplication model = new MainApplication();
            List<BankMaster> ListOfBranches = TempData["BranchList"] as List<BankMaster>;
            List<BankMaster> FinalList = new List<BankMaster>();
            FinalList = FinalList.Concat((IEnumerable<BankMaster>)ListOfBranches.Where(x => x.Branch.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.BankList = FinalList;
            TempData["BranchList"] = FinalList;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(BankMaster bnk)
        {
            bnk.status = "Active";
            bnk.modifiedon = DateTime.Now;
            _bankService.UpdateBank(bnk);
            return RedirectToAction("UpdateDetails/" + bnk.bankId, "Bank");
        }

        //SHOW PAGE AFTER EDIT
        [HttpGet]
        public ActionResult UpdateDetails(int id)
        {
            MainApplication model = new MainApplication();
            model.BankDetails = _bankService.getById(id);
            TempData["BranchList"] = _bankService.getAllBanks();
            return View(model);
        }

    }
}
