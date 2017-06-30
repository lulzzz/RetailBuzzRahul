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
using System.Configuration; 
using System.Data.SqlClient;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class TransportController : Controller
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
        private readonly ITransportService _TransportMasterService;
        private readonly ICountryService _countryservice;
        private readonly IStateService _stateservice;
        private readonly IDistrictService _districtservice;
        private readonly IUtilityService _utilityservice;
        private readonly IBankNameService _banknameservice;
        private readonly IBankService _bankservice;
        private readonly ITransportBankDetailService _TransportBankDetailService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;


        public TransportController(ITransportService TransportMasterService, ICountryService countryservice, IStateService stateservice,
            IDistrictService districtservice, IUtilityService utilityservice, IBankNameService banknameservice, IBankService bankservice, ITransportBankDetailService TransportBankDetailService,
            IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._TransportMasterService = TransportMasterService;
            this._countryservice = countryservice;
            this._stateservice = stateservice;
            this._districtservice = districtservice;
            this._utilityservice = utilityservice;
            this._banknameservice = banknameservice;
            this._bankservice = bankservice;
            this._TransportBankDetailService = TransportBankDetailService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
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

        [HttpGet]
        public ActionResult Details(string id)
        {
            MainApplication model = new MainApplication()
            {
                TransportDetails = new TransportMaster(),
                TransportBankDetails = new TransportBankDetail(),
            };
            int Id = Decode(id);
            model.TransportDetails = _TransportMasterService.getById(Id);
            model.TransportBankDetailList = _TransportBankDetailService.GetDetailsFromBank(model.TransportDetails.TransportCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previoustransport = TempData["transportcode"].ToString();
            if (previoustransport != model.TransportDetails.TransportCode)
            {
                ViewData["transportchanged"] = previoustransport + " is replaced with " + model.TransportDetails.TransportCode + " because " + previoustransport + " is acquired by another person";
            }
            TempData["transportcode"] = previoustransport;

            return View(model);
        }

        //
        // GET: /Transport/Create
        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                TransportDetails = new TransportMaster(),
            };

            var Details = _TransportMasterService.getLastInsertedTransport();
            int lastid = 0;
            int length = 0;
            if (Details != null)
            {
                lastid = Details.TransportId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("TRS", length, lastid);
            model.TransportDetails.TransportCode = catCode;
            TempData["transportcode"] = catCode;
            
            var BankList = _TransportBankDetailService.GetDetailsFromBank(catCode);
            if (BankList.Count() != 0)
            {
                _TransportBankDetailService.ExecuteProcedure(catCode);
            }
            Session["TransportCode"] = catCode;
            model.TransportDetails.CountryList = _countryservice.getallcountries();
            model.TransportDetails.StateList = _stateservice.GetStateByCountry(1);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetTransportNames(string term)
        {
            var result = _TransportMasterService.GetTranportNames(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.TransportName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidateTransportName(string transportname)
        {
            string message = string.Empty;
            var transnameList = _TransportMasterService.ValidateName(transportname);
            if (transnameList.Count() != 0)
            {
                message = "success";
            }
            else
            {
                message = "fail";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MainApplication mainApplication)
        {
            var Details = _TransportMasterService.getLastInsertedTransport();
            int lastid = 0;
            int length = 0;
            if (Details != null)
            {
                lastid = Details.TransportId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("TRS", length, lastid);
        
            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["TransportBankList" + LogedinUSerName] != null)
            {
                var list = TempData["TransportBankList" + LogedinUSerName] as IEnumerable<TransportBankDetail>;
                foreach (var bank in list)
                {
                    bank.TransportCode = catCode;
                    bank.Status = "Active";
                    bank.ModifiedOn = DateTime.Now;
                    _TransportBankDetailService.CreateBankDetails(bank);
                }
            }

            if (mainApplication.TransportBankDetails.BankName != null)
            {
                mainApplication.TransportBankDetails.TransportCode = mainApplication.transportcode;
                mainApplication.TransportBankDetails.Status = "Active";
                mainApplication.TransportBankDetails.ModifiedOn = DateTime.Now;
                _TransportBankDetailService.CreateBankDetails(mainApplication.TransportBankDetails);
            }
            int did = Convert.ToInt32(mainApplication.TransportDetails.District);
            mainApplication.TransportDetails.District = _districtservice.GetDistrictNamebyId(did);
            string Transportname = mainApplication.TransportDetails.TransportName;
            mainApplication.TransportDetails.TransportName = Transportname.First().ToString().ToUpper() + Transportname.Substring(1);
            mainApplication.TransportDetails.Status = "Active";
            mainApplication.TransportDetails.ModifiedOn = DateTime.Now;
            mainApplication.TransportDetails.TransportCode = catCode;
            _TransportMasterService.CreateTransport(mainApplication.TransportDetails);
            var details = _TransportMasterService.getLastInsertedTransport();
            string id = Encode(details.TransportId.ToString());
            return RedirectToAction("Details/" + id, "Transport");
        }

        [HttpGet]
        public JsonResult ValidateEmail(string mail)
        {
            string message = string.Empty;
            var translist = _TransportMasterService.GetEmail(mail);
            if (translist.Count() != 0)
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
        public ActionResult Edit()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Edit";
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.TransportDetails = _TransportMasterService.getById(id);
            model.TransportDetails.CountryList = _countryservice.getallcountries();
            int countryid = _countryservice.getidbyname(model.TransportDetails.Country);
            model.TransportDetails.StateList = _stateservice.GetStateByCountry(countryid);
            int stateid = _stateservice.GetStateIdByName(model.TransportDetails.State);
            model.TransportDetails.DistrictList = _districtservice.getDistrictbyState(stateid);
            var bankdetaillist = _TransportBankDetailService.GetDetailsFromBank(model.TransportDetails.TransportCode);
            string namebranch = string.Empty;
            List<TransportBankDetail> transbanklist = new List<TransportBankDetail>();
            foreach (var bankdetail in bankdetaillist)
            {
                TransportBankDetail transbank = new TransportBankDetail();
                namebranch = bankdetail.BankName + ", " + bankdetail.Branch;
                transbank.BankName = namebranch;
                transbank.BankId = bankdetail.BankId;
                transbanklist.Add(transbank);
                namebranch = string.Empty;
            }
            model.TransportBankDetailList = transbanklist;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(TransportMaster transmaster)
        {
            transmaster.Status = "Active";
            transmaster.ModifiedOn = DateTime.Now;
            _TransportMasterService.UpdateTransport(transmaster);
            return RedirectToAction("ResultTransportMaster/" + transmaster.TransportId, "Transport");
        }

        [HttpGet]
        public ActionResult TransportList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<TransportMaster> ListOfTransport = TempData["TransList"] as IEnumerable<TransportMaster>;
            ListOfTransport = ListOfTransport.Where(x => x.Status == "Active");
            List<TransportMaster> FinalList = new List<TransportMaster>();
            FinalList = FinalList.Concat(ListOfTransport.Where(x => x.TransportName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.TransportList = FinalList;
            TempData["TransList"] = FinalList;
            return View(model);
        }

        [HttpGet]
        public ActionResult ResultTransportMaster(int id)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.TransportDetails = _TransportMasterService.getById(id);
            model.TransportBankDetailList = _TransportBankDetailService.GetDetailsFromBank(model.TransportDetails.TransportCode);
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
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        [HttpGet]
        public ActionResult DeletePartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.TransportDetails = _TransportMasterService.getById(id);
            model.TransportBankDetailList = _TransportBankDetailService.GetDetailsFromBank(model.TransportDetails.TransportCode);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(TransportMaster transmaster)
        {
            transmaster.Status = "InActive";
            transmaster.ModifiedOn = DateTime.Now;
            _TransportMasterService.UpdateTransport(transmaster);
            var BankList = _TransportBankDetailService.GetDetailsFromBank(transmaster.TransportCode);
            foreach (var bank in BankList)
            {
                bank.Status = "InActive";
                bank.ModifiedOn = DateTime.Now;
                _TransportBankDetailService.UpdateBankDetails(bank);
            }
            return RedirectToAction("ResultTransportMaster/" + transmaster.TransportId, "Transport");
        }

        [HttpGet]
        public JsonResult LoadStateByCountry(string countryname)
        {
            if (!string.IsNullOrEmpty(countryname))
            {
                int countryid = _countryservice.getidbyname(countryname);
                TransportMaster transmaster = new TransportMaster();
                transmaster.StateList = _stateservice.GetStateByCountry(countryid);
                var modelData = transmaster.StateList.Select(m => new SelectListItem()
                {
                    Text = m.StateName,
                    Value = m.StateName
                });
                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modelData = string.Empty;
                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult LoadDistrictByState(string statename)
        {
            if (!string.IsNullOrEmpty(statename))
            {
                int stateid = _stateservice.GetStateIdByName(statename);
                TransportMaster transmaster = new TransportMaster();
                transmaster.DistrictList = _districtservice.getDistrictbyState(stateid);
                var modelData = transmaster.DistrictList.Select(m => new SelectListItem()
                {
                    Text = m.DistrictName,
                    Value = m.DistrictId.ToString()
                });
                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modelData = string.Empty;
                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LoadNamesByTransportMode(string name)
        {
            MainApplication model = new MainApplication();
            model.TransportList = _TransportMasterService.GetTransportByMode(name);
            TempData["TransList"] = model.TransportList;
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateBankDetail()
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.transportcode = Session["TransportCode"].ToString();
            model.TransportBankDetails.BankNameList = _banknameservice.getAllBankNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBankDetail(string bankname, string branch, string transportcode,
            string bankifscno,string bankaddress, string accountno, string accounttype,
            string micr)
        {
            List<TransportBankDetail> TransportBankList = new List<TransportBankDetail>();
            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["TransportBankList" + LogedinUSerName] != null)
            {
                var transportbanklist = TempData["TransportBankList" + LogedinUSerName] as IEnumerable<TransportBankDetail>;
                foreach (var list in transportbanklist)
                {
                    TransportBankList.Add(list);
                }
            }

            TransportBankDetail BankDetails = new TransportBankDetail();
            BankDetails.BankName = bankname;
            BankDetails.Branch = branch;
            BankDetails.TransportCode = transportcode;
            BankDetails.BankIFSCNo = bankifscno;
            BankDetails.BankAddress = bankaddress;
            BankDetails.AccountNo = accountno;
            BankDetails.AccountType = accounttype;
            BankDetails.MICRCode = micr;
            BankDetails.Status = "Active";
            BankDetails.ModifiedOn = DateTime.Now;
            TransportBankList.Add(BankDetails);
            TempData["TransportBankList" + LogedinUSerName] = TransportBankList;
            return RedirectToAction("CreateBankDetail");
        }

        [HttpGet]
        public ActionResult UpdateBankDetail(int selectedbankname)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.TransportBankDetails = _TransportBankDetailService.GetBankDetailsOfTransport(selectedbankname);
            model.TransportBankDetails.BankBranchList = _bankservice.GetBranchByBankName(model.TransportBankDetails.BankName);
            model.TransportBankDetails.BankNameList = _banknameservice.getAllBankNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateBankDetail(int bankid, string bankname, string branch,
            string transportcode, string bankifscno,string bankaddress, string accountno, 
            string accounttype,string micr)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.TransportBankDetails.BankId = bankid;
            model.TransportBankDetails.BankName = bankname;
            model.TransportBankDetails.Branch = branch;
            model.TransportBankDetails.TransportCode = transportcode;
            model.TransportBankDetails.BankIFSCNo = bankifscno;
            model.TransportBankDetails.BankAddress = bankaddress;
            model.TransportBankDetails.AccountNo = accountno;
            model.TransportBankDetails.AccountType = accounttype;
            if (micr == "")
            {
                model.TransportBankDetails.MICRCode = null;
            }
            else
            {
                model.TransportBankDetails.MICRCode = micr;
            }
            model.TransportBankDetails.Status = "Active";
            model.TransportBankDetails.ModifiedOn = DateTime.Now;
            _TransportBankDetailService.UpdateBankDetails(model.TransportBankDetails);
            return RedirectToAction("UpdateBankDetail", "Transport", new { selectedbankname = bankid });
        }

        [HttpGet]
        public ActionResult PopUpPage(string transportcode)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            TransportBankDetail TransportBankDetails = new TransportBankDetail();
            TransportBankDetails.TransportCode = transportcode;
            TransportBankDetails.BankNameList = _banknameservice.getAllBankNames();
            return View(TransportBankDetails);
        }

        [HttpPost]
        public JsonResult PopUpPage(string bankname, string branch, string transportcode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {
            TransportBankDetail TransportBankDetails = new TransportBankDetail();
            TransportBankDetails.BankName = bankname;
            TransportBankDetails.Branch = branch;
            TransportBankDetails.TransportCode = transportcode;
            TransportBankDetails.BankIFSCNo = bankifscno;
            TransportBankDetails.BankAddress = bankaddress;
            TransportBankDetails.AccountNo = accountno;
            TransportBankDetails.AccountType = accounttype;
            if (micr == "")
            {
                TransportBankDetails.MICRCode = null;
            }
            else
            {
                TransportBankDetails.MICRCode = micr;    
            }
            TransportBankDetails.Status = "Active";
            TransportBankDetails.ModifiedOn = DateTime.Now;
            _TransportBankDetailService.CreateBankDetails(TransportBankDetails);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdatedBankDetails(string transportcode)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            var bankdetaillist = _TransportBankDetailService.GetDetailsFromBank(transportcode);
            string namebranch = string.Empty;
            List<TransportBankDetail> transbanklist = new List<TransportBankDetail>();
            foreach (var bankdetail in bankdetaillist)
            {
                TransportBankDetail transbank = new TransportBankDetail();
                namebranch = bankdetail.BankName + ", " + bankdetail.Branch;
                transbank.BankName = namebranch;
                transbank.BankId = bankdetail.BankId;
                transbanklist.Add(transbank);
                namebranch = string.Empty;
            }
            model.TransportBankDetailList = transbanklist;
            var banknamelist = model.TransportBankDetailList.Select(m => new SelectListItem()
            {
                Text = m.BankName,
                Value = m.BankId.ToString(),
            });
            return Json(banknamelist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBranch(string BankName)
        {
            MainApplication model = new MainApplication()
            {
                TransportBankDetails = new TransportBankDetail(),
            };
            model.TransportBankDetails.BankBranchList = _bankservice.GetBranchByBankName(BankName);
            var modelData = model.TransportBankDetails.BankBranchList.Select(m => new SelectListItem()
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
            string IFSCNo = _bankservice.GetIfscByBranch(branch);
            string BankAddress = _bankservice.GetAddressByIFSC(IFSCNo);
            string Micr = _bankservice.GetMICRByIFSC(IFSCNo).MICRCode;
            return Json(new { IFSCNo, BankAddress,Micr }, JsonRequestBehavior.AllowGet);
        }
    }
}