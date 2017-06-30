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
    //[NoDirectAccess]
    [SessionExpireFilter]
    public class ClientController : Controller
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
        private readonly IClientMasterService _ClientMasterService;
        private readonly ICountryService _CountryService;
        private readonly IStateService _StateService;
        private readonly IDistrictService _DistrictService;
        private readonly IBankService _BankService;
        private readonly IBankNameService _BankNameService;
        private readonly IUtilityService _UtilityService;
        private readonly IClientBankDetailService _ClientBankDetailService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;


        public ClientController(IClientMasterService ClientMasterService, ICountryService CountryService, IStateService StateService,
            IDistrictService DistrictService, IBankService BankService, IBankNameService BankNameService, IUtilityService UtilityService, IUserCredentialService usercredentialservice, IModuleService iIModuleService,
            IClientBankDetailService ClientBankDetailService)
        {
            this._ClientMasterService = ClientMasterService;
            this._CountryService = CountryService;
            this._StateService = StateService;
            this._DistrictService = DistrictService;
            this._BankService = BankService;
            this._BankNameService = BankNameService;
            this._UtilityService = UtilityService;
            this._ClientBankDetailService = ClientBankDetailService;
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
                ClientDetails = new ClientMaster(),
                ClientBankDetails = new ClientBankDetail(),
            };
            int Id = Decode(id);
            model.ClientDetails = _ClientMasterService.getById(Id);
            model.ClientBankDetailList = _ClientBankDetailService.GetDetailsFromBank(model.ClientDetails.ClientCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previousclient = TempData["clientcode"].ToString();
            if (previousclient != model.ClientDetails.ClientCode)
            {
                ViewData["clientchanged"] = previousclient + " is replaced with " + model.ClientDetails.ClientCode + " because " + previousclient + " is acquired by another person";
            }
            TempData["clientcode"] = previousclient;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                ClientDetails = new ClientMaster()
            };
            
            var ClientDetails = _ClientMasterService.GetLastInsertedClient();
            int lastid = 0;
            int length = 0;
            if (ClientDetails != null)
            {
                lastid = ClientDetails.ClientId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _UtilityService.getName("CL", length, lastid);
            model.ClientDetails.ClientCode = catCode;
            TempData["clientcode"] = catCode;

            var codelist = _ClientBankDetailService.GetDetailsFromBank(catCode);
            model.ClientDetails.StateList = _StateService.GetStateByCountry(1);
            if (codelist.Count() != 0)
            {
                _ClientBankDetailService.ExecuteProcedure(catCode);
            }
            Session["ClientCode"] = catCode;
            model.ClientDetails.CountryList = _CountryService.getallcountries();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetClientNames(string term)
        {
            var result = _ClientMasterService.GetClientNames(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetActiveAndMaharashtraClients(string term)
        {
            var result = _ClientMasterService.GetActiveAndMaharashtraClients(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidateClientName(string clientname)
        {
            string message = string.Empty;
            var clientnameList = _ClientMasterService.ValidateName(clientname);
            if (clientnameList.Count() != 0)
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
        public JsonResult ValidateEmail(string mail)
        {
            string message = string.Empty;
            var clientList = _ClientMasterService.GetEmail(mail);
            if (clientList.Count() != 0)
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
        public ActionResult PopUpPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MainApplication mainApplication)
        {
            var ClientDetails = _ClientMasterService.GetLastInsertedClient();
            int lastid = 0;
            int length = 0;
            if (ClientDetails != null)
            {
                lastid = ClientDetails.ClientId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _UtilityService.getName("CL", length, lastid);
            

            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["ClientBankList" + LogedinUSerName] != null)
            {
                var list = TempData["ClientBankList" + LogedinUSerName] as IEnumerable<ClientBankDetail>;
                foreach (var bank in list)
                {
                    bank.ClientCode = catCode;
                    bank.Status = "Active";
                    bank.ModifiedOn = DateTime.Now;
                    _ClientBankDetailService.CreateBankDetails(bank);
                }
            }

            if (mainApplication.ClientBankDetails.BankName != null)
            {
                mainApplication.ClientBankDetails.ClientCode = mainApplication.clientcode;
                mainApplication.ClientBankDetails.Status = "Active";
                mainApplication.ClientBankDetails.ModifiedOn = DateTime.Now;
                _ClientBankDetailService.CreateBankDetails(mainApplication.ClientBankDetails);
            }
            int did = Convert.ToInt32(mainApplication.ClientDetails.District);
            mainApplication.ClientDetails.District = _DistrictService.GetDistrictNamebyId(did);
            string Clientname = mainApplication.ClientDetails.ClientName;
            mainApplication.ClientDetails.ClientName = Clientname.First().ToString().ToUpper() + Clientname.Substring(1);
            mainApplication.ClientDetails.Status = "Active";
            mainApplication.ClientDetails.ModifiedOn = DateTime.Now;
            mainApplication.ClientDetails.ClientCode = catCode;
            _ClientMasterService.CreateClient(mainApplication.ClientDetails);
            var Details = _ClientMasterService.GetLastInsertedClient();
            string clientid = Encode(Details.ClientId.ToString());
            return RedirectToAction("Details/" + clientid, "Client");
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
            TempData["ViewType"] = "Edit";
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            model.ClientDetails = _ClientMasterService.getClientById(id);
            model.ClientDetails.CountryList = _CountryService.getallcountries();
            var countryData = _CountryService.getidbyname(model.ClientDetails.Country);
            model.ClientDetails.StateList = _StateService.GetStateByCountry(countryData);
            int stateid = _StateService.GetStateIdByName(model.ClientDetails.State);
            model.ClientDetails.DistrictList = _DistrictService.getDistrictbyState(stateid);
            var bankdetaillist = _ClientBankDetailService.GetDetailsFromBank(model.ClientDetails.ClientCode);
            string namebranch = string.Empty;
            List<ClientBankDetail> clbanklist = new List<ClientBankDetail>();
            foreach (var bankdetail in bankdetaillist)
            {
                ClientBankDetail clbank = new ClientBankDetail();
                namebranch = bankdetail.BankName + ", " + bankdetail.Branch;
                clbank.BankName = namebranch;
                clbank.BankId = bankdetail.BankId;
                clbanklist.Add(clbank);
                namebranch = string.Empty;
            }
            model.ClientBankDetailList = clbanklist;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(ClientMaster cltmaster)
        {
            cltmaster.Status = "Active";
            cltmaster.ModifiedOn = DateTime.Now;
            _ClientMasterService.UpdateClient(cltmaster);
            return RedirectToAction("ResultClientMaster/" + cltmaster.ClientId, "Client");
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
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        [HttpGet]
        public ActionResult DeletePartial(int id)
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            model.ClientDetails = _ClientMasterService.getClientById(id);
            model.ClientBankDetailList = _ClientBankDetailService.GetDetailsFromBank(model.ClientDetails.ClientCode);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(ClientMaster cltmaster)
        {
            cltmaster.Status = "InActive";
            cltmaster.ModifiedOn = DateTime.Now;
            _ClientMasterService.UpdateClient(cltmaster);
            var BankList = _ClientBankDetailService.GetDetailsFromBank(cltmaster.ClientCode);
            foreach (var bank in BankList)
            {
                bank.Status = "InActive";
                bank.ModifiedOn = DateTime.Now;
                _ClientBankDetailService.UpdateBankDetails(bank);
            }
            return RedirectToAction("ResultClientMaster/" + cltmaster.ClientId, "Client");
        }

        [HttpGet]
        public ActionResult ClientList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<ClientMaster> ListOfClient = TempData["ClientList"] as IEnumerable<ClientMaster>;
            ListOfClient = ListOfClient.Where(a => a.Status == "Active");
            List<ClientMaster> FinalList = new List<ClientMaster>();
            FinalList = FinalList.Concat(ListOfClient.Where(x => x.ClientName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.ClientList = FinalList;
            TempData["ClientList"] = FinalList;
            return View(model);
        }

        [HttpGet]
        public ActionResult ResultClientMaster(int id)
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            model.ClientDetails = _ClientMasterService.getById(id);
            TempData["ClientList"] = _ClientMasterService.GetAllClients();
            model.ClientBankDetailList = _ClientBankDetailService.GetDetailsFromBank(model.ClientDetails.ClientCode);
            return View(model);
        }

        [HttpGet]
        public JsonResult LoadStateByCountry(string countryname)
        {
            if (!string.IsNullOrEmpty(countryname))
            {
                int countryid = _CountryService.getidbyname(countryname);
                ClientMaster clientmaster = new ClientMaster();
                clientmaster.StateList = _StateService.GetStateByCountry(countryid);
                var modelData = clientmaster.StateList.Select(m => new SelectListItem()
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
                int stateid = _StateService.GetStateIdByName(statename);
                ClientMaster clientmaster = new ClientMaster();
                clientmaster.DistrictList = _DistrictService.getDistrictbyState(stateid);
                var modelData = clientmaster.DistrictList.Select(m => new SelectListItem()
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
        public ActionResult LoadNamesByMembershipCard()
        {
            MainApplication model = new MainApplication();
            model.ClientList = _ClientMasterService.GetActiveClients();
            TempData["ClientList"] = model.ClientList;
            if (model.ClientList.Count() == 0)
            {
                return RedirectToAction("EmptyList", "Client");
            }
            return View(model);
        }

        public JsonResult EmptyList()
        {
            return Json("Empty", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateBankDetail()
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            model.clientcode = Session["ClientCode"].ToString();
            model.ClientBankDetails.BankNameList = _BankNameService.getAllBankNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBankDetail(string bankname, string branch, string clientcode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {

            List<ClientBankDetail> ClientBankList = new List<ClientBankDetail>();
            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["ClientBankList" + LogedinUSerName] != null)
            {
                var clientbanklist = TempData["ClientBankList" + LogedinUSerName] as IEnumerable<ClientBankDetail>;
                foreach (var list in clientbanklist)
                {
                    ClientBankList.Add(list);
                }
            }

            ClientBankDetail BankDetails = new ClientBankDetail();
            BankDetails.BankName = bankname;
            BankDetails.Branch = branch;
            BankDetails.ClientCode = clientcode;
            BankDetails.BankIFSCNo = bankifscno;
            BankDetails.BankAddress = bankaddress;
            BankDetails.AccountNo = accountno;
            BankDetails.AccountType = accounttype;
            BankDetails.MICRCode = micr;
            BankDetails.Status = "Active";
            BankDetails.ModifiedOn = DateTime.Now;
            ClientBankList.Add(BankDetails);
            TempData["ClientBankList" + LogedinUSerName] = ClientBankList;
            return RedirectToAction("CreateBankDetail");
        }

        [HttpGet]
        public ActionResult UpdateBankDetail(int selectedbankname)
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            model.ClientBankDetails = _ClientBankDetailService.GetBankDetailsOfClient(selectedbankname);
            model.ClientBankDetails.BankBranchList = _BankService.GetBranchByBankName(model.ClientBankDetails.BankName);
            model.ClientBankDetails.BankNameList = _BankNameService.getAllBankNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateBankDetail(int bankid, string bankname, string branch, string clientcode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            model.ClientBankDetails.BankId = bankid;
            model.ClientBankDetails.BankName = bankname;
            model.ClientBankDetails.Branch = branch;
            model.ClientBankDetails.ClientCode = clientcode;
            model.ClientBankDetails.BankIFSCNo = bankifscno;
            model.ClientBankDetails.BankAddress = bankaddress;
            model.ClientBankDetails.AccountNo = accountno;
            model.ClientBankDetails.AccountType = accounttype;

            if (micr == "")
            {
                model.ClientBankDetails.MICRCode = null;
            }
            else
            {
                model.ClientBankDetails.MICRCode = micr;
            }
            model.ClientBankDetails.Status = "Active";
            model.ClientBankDetails.ModifiedOn = DateTime.Now;
            _ClientBankDetailService.UpdateBankDetails(model.ClientBankDetails);
            return RedirectToAction("UpdateBankDetail", "Client", new { selectedbankname = bankid });
        }

        [HttpGet]
        public ActionResult BankPopUpPage(string clientcode)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            ClientBankDetail clientbankdetials = new ClientBankDetail();
            clientbankdetials.ClientCode = clientcode;
            clientbankdetials.BankNameList = _BankNameService.getAllBankNames();
            return View(clientbankdetials);
        }

        [HttpPost]
        public JsonResult BankPopUpPage(string bankname, string branch, string clientcode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {
            ClientBankDetail clientbankdetials = new ClientBankDetail();
            clientbankdetials.BankName = bankname;
            clientbankdetials.Branch = branch;
            clientbankdetials.ClientCode = clientcode;
            clientbankdetials.BankIFSCNo = bankifscno;
            clientbankdetials.BankAddress = bankaddress;
            clientbankdetials.AccountNo = accountno;
            clientbankdetials.AccountType = accounttype;
            if (micr == "")
            {
                clientbankdetials.MICRCode = null;
            }
            else
            {
                clientbankdetials.MICRCode = micr;
            }
            clientbankdetials.Status = "Active";
            clientbankdetials.ModifiedOn = DateTime.Now;
            _ClientBankDetailService.CreateBankDetails(clientbankdetials);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdatedBankDetails(string clientcode)
        {
            MainApplication model = new MainApplication()
            {
                ClientBankDetails = new ClientBankDetail(),
            };
            var bankdetaillist = _ClientBankDetailService.GetDetailsFromBank(clientcode);
            string namebranch = string.Empty;
            List<ClientBankDetail> clbanklist = new List<ClientBankDetail>();
            foreach (var bankdetail in bankdetaillist)
            {
                ClientBankDetail clbank = new ClientBankDetail();
                namebranch = bankdetail.BankName + ", " + bankdetail.Branch;
                clbank.BankName = namebranch;
                clbank.BankId = bankdetail.BankId;
                clbanklist.Add(clbank);
                namebranch = string.Empty;
            }
            model.ClientBankDetailList = clbanklist;
            var banknamelist = model.ClientBankDetailList.Select(m => new SelectListItem()
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
                ClientBankDetails = new ClientBankDetail(),
            };
            model.ClientBankDetails.BankBranchList = _BankService.GetBranchByBankName(BankName);
            var modelData = model.ClientBankDetails.BankBranchList.Select(m => new SelectListItem()
            {
                Value = m.Branch,
                Text = m.Branch
            });
            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetIFSCNoAndAddress(string branch)
        {
            string IFSCNo = _BankService.GetIfscByBranch(branch);
            string BankAddress = _BankService.GetAddressByIFSC(IFSCNo);
            string Micr = _BankService.GetMICRByIFSC(IFSCNo).MICRCode;
            return Json(new { IFSCNo, BankAddress,Micr }, JsonRequestBehavior.AllowGet);
        }

    }
}