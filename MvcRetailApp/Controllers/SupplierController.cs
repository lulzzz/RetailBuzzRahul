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
    public class SupplierController : Controller
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
        private readonly ISuppliersMasterService _suppliermasterservice;
        private readonly ICountryService _countryservice;
        private readonly IStateService _stateservice;
        private readonly ITypeOfSupplierService _typeofsupplierservice;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IDistrictService _districtservice;
        private readonly IUtilityService _utilityservice;
        private readonly IBankNameService _banknameservice;
        private readonly IBankService _bankservice;
        private readonly ISupplierBankDetailService _SupplierBankDetailService;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public SupplierController(ISuppliersMasterService suppliermasterservice, ICountryService countryservice, IStateService stateservice, ITypeOfSupplierService typeofsupplierservice,
            IDistrictService districtservice, IUtilityService utilityservice, IBankNameService banknameservice, IBankService bankservice, ISupplierBankDetailService SupplierBankDetailService, IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._suppliermasterservice = suppliermasterservice;
            this._countryservice = countryservice;
            this._stateservice = stateservice;
            this._typeofsupplierservice = typeofsupplierservice;
            this._districtservice = districtservice;
            this._utilityservice = utilityservice;
            this._banknameservice = banknameservice;
            this._bankservice = bankservice;
            this._SupplierBankDetailService = SupplierBankDetailService;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
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
                SupplierDetails = new SupplierMaster(),
                SupplierBankDetails = new SupplierBankDetail(),
            };

            int Id = Decode(id);
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.SupplierDetails = _suppliermasterservice.getById(Id);
            model.SupplierBankDetailList = _SupplierBankDetailService.GetDetailsFromBank(model.SupplierDetails.SupplierCode);
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previoussupplier = TempData["suppliercode"].ToString();
            if (previoussupplier != model.SupplierDetails.SupplierCode)
            {
                ViewData["supplierchanged"] = previoussupplier + " is replaced with " + model.SupplierDetails.SupplierCode + " because " + previoussupplier + " is acquired by another person";
            }
            TempData["suppliercode"] = previoussupplier;

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                SupplierDetails = new SupplierMaster(),
            };
            var Details = _suppliermasterservice.getLastInsertedSupplier();
            int lastid = 0;
            int length = 0;
            if (Details != null)
            {
                lastid = Details.SupplierId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("SP", length, lastid);
            model.SupplierDetails.SupplierCode = catCode;
            TempData["suppliercode"] = catCode;
            
            var BankList = _SupplierBankDetailService.GetDetailsFromBank(catCode);
            if (BankList.Count() != 0)
            {
                _SupplierBankDetailService.ExecuteProcedure(catCode);
            }
            model.SupplierDetails.CountriesList = _countryservice.getallcountries();
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.SupplierDetails.StatesList = _stateservice.GetStateByCountry(1);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            TempData["SupplierBankList" + LogedinUSerName] = null;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSupplierNames(string term)
        {
            var result = _suppliermasterservice.GetActiveSupplersByName(term);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.SupplierName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidateSupplierName(string suppliername)
        {
            string message = string.Empty;
            var suppnameList = _suppliermasterservice.ValidateName(suppliername);
            if (suppnameList.Count() != 0)
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
        public ActionResult Create(MainApplication mainApplication, FormCollection frmsuppcollection)
        {
            var Details = _suppliermasterservice.getLastInsertedSupplier();
            int lastid = 0;
            int length = 0;
            if (Details != null)
            {
                lastid = Details.SupplierId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("SP", length, lastid);
            
            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["SupplierBankList" + LogedinUSerName] != null)
            {
                var list = TempData["SupplierBankList" + LogedinUSerName] as IEnumerable<SupplierBankDetail>;
                foreach (var bank in list)
                {
                    bank.SupplierCode = catCode;
                    bank.Status = "Active";
                    bank.ModifiedOn = DateTime.Now;
                    _SupplierBankDetailService.CreateBankDetails(bank);
                }
            }

            if (mainApplication.SupplierBankDetails.BankName != null)
            {
                mainApplication.SupplierBankDetails.SupplierCode = catCode;
                mainApplication.SupplierBankDetails.Status = "Active";
                mainApplication.SupplierBankDetails.ModifiedOn = DateTime.Now;
                _SupplierBankDetailService.CreateBankDetails(mainApplication.SupplierBankDetails);
            }

            //save multiple values in database
            string ddltypesupplier = frmsuppcollection["DDLTypeSupplier"].ToString();
            mainApplication.SupplierDetails.SupplierType = ddltypesupplier;
            int did = Convert.ToInt32(mainApplication.SupplierDetails.district);
            mainApplication.SupplierDetails.district = _districtservice.GetDistrictNamebyId(did);
            string Suppliername = mainApplication.SupplierDetails.SupplierName;
            mainApplication.SupplierDetails.SupplierName = Suppliername.First().ToString().ToUpper() + Suppliername.Substring(1);
            mainApplication.SupplierDetails.SupplierCode = catCode;
            mainApplication.SupplierDetails.Status = "Active";
            mainApplication.SupplierDetails.ModifiedOn = DateTime.Now;
            _suppliermasterservice.CreateSupplier(mainApplication.SupplierDetails);
            var LastRow = _suppliermasterservice.getLastInsertedSupplier();
            string supplierid = Encode(LastRow.SupplierId.ToString());
            return RedirectToAction("Details/" + supplierid, "Supplier");
        }


        [HttpGet]
        public JsonResult ValidateEmail(string mail)
        {
            string message = string.Empty;
            var suppilerList = _suppliermasterservice.GetEmail(mail);
            if (suppilerList.Count() != 0)
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
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
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
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierDetails = _suppliermasterservice.getById(id);
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.SupplierDetails.CountriesList = _countryservice.getallcountries();
            int countryid = _countryservice.getidbyname(model.SupplierDetails.Country);
            model.SupplierDetails.StatesList = _stateservice.GetStateByCountry(countryid);
            int stateid = _stateservice.GetStateIdByName(model.SupplierDetails.State);
            model.SupplierDetails.DistrictsList = _districtservice.getDistrictbyState(stateid);
            var bankdetaillist = _SupplierBankDetailService.GetDetailsFromBank(model.SupplierDetails.SupplierCode);
            string namebranch = string.Empty;
            List<SupplierBankDetail> supbanklist = new List<SupplierBankDetail>();
            foreach (var bankdetail in bankdetaillist)
            {
                SupplierBankDetail suppbank = new SupplierBankDetail();
                namebranch = bankdetail.BankName + ", " + bankdetail.Branch;
                suppbank.BankName = namebranch;
                suppbank.BankId = bankdetail.BankId;
                supbanklist.Add(suppbank);
                namebranch = string.Empty;
            }
            model.SupplierBankDetailList = supbanklist;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(SupplierMaster suppmaster)
        {
            suppmaster.Status = "Active";
            suppmaster.ModifiedOn = DateTime.Now;
            //save multiple values in database
            _suppliermasterservice.UpdateSupplier(suppmaster);
            return RedirectToAction("ResultSupplierMaster/" + suppmaster.SupplierId, "Supplier");
        }

        [HttpGet]
        public ActionResult SupplierList(string name)
        {
            MainApplication model = new MainApplication();
            IEnumerable<SupplierMaster> ListOfSupplier = TempData["SuppList"] as IEnumerable<SupplierMaster>;
            ListOfSupplier = ListOfSupplier.Where(x => x.Status == "Active");
            List<SupplierMaster> FinalList = new List<SupplierMaster>();
            FinalList = FinalList.Concat(ListOfSupplier.Where(x => x.SupplierName.ToLower().Contains(name.ToLower()))).Distinct().ToList();
            model.SupplierList = FinalList;
            TempData["SuppList"] = FinalList;
            return View(model);
        }

        [HttpGet]
        public ActionResult ResultSupplierMaster(int id)
        {
            MainApplication model = new MainApplication()
            {
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierDetails = _suppliermasterservice.getById(id);
            model.SupplierBankDetailList = _SupplierBankDetailService.GetDetailsFromBank(model.SupplierDetails.SupplierCode);
            TempData["SuppList"] = _suppliermasterservice.getAllSuppliers();
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            MainApplication model = new MainApplication();
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
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
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierDetails = _suppliermasterservice.getById(id);
            model.SupplierBankDetailList = _SupplierBankDetailService.GetDetailsFromBank(model.SupplierDetails.SupplierCode);
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(SupplierMaster suppmaster)
        {
            suppmaster.Status = "InActive";
            suppmaster.ModifiedOn = DateTime.Now;
            _suppliermasterservice.UpdateSupplier(suppmaster);
            var BankList = _SupplierBankDetailService.GetDetailsFromBank(suppmaster.SupplierCode);
            foreach (var bank in BankList)
            {
                bank.Status = "InActive";
                bank.ModifiedOn = DateTime.Now;
                _SupplierBankDetailService.UpdateBankDetails(bank);
            }
            return RedirectToAction("ResultSupplierMaster/" + suppmaster.SupplierId, "Supplier");
        }

        [HttpGet]
        public JsonResult LoadStateByCountry(string countryname)
        {
            if (!string.IsNullOrEmpty(countryname))
            {
                int countryid = _countryservice.getidbyname(countryname);
                SupplierMaster suppmaster = new SupplierMaster();
                suppmaster.StatesList = _stateservice.GetStateByCountry(countryid);
                var modelData = suppmaster.StatesList.Select(m => new SelectListItem()
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
                SupplierMaster suppmaster = new SupplierMaster();
                suppmaster.DistrictsList = _districtservice.getDistrictbyState(stateid);
                var modelData = suppmaster.DistrictsList.Select(m => new SelectListItem()
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
        public ActionResult LoadNamesByTypeSupplier()
        {
            MainApplication model = new MainApplication();
            model.SupplierList = _suppliermasterservice.GetActiveSuppliers();
            TempData["SuppList"] = model.SupplierList;
            if (model.SupplierList.Count() == 0)
            {
                return RedirectToAction("EmptyList", "Supplier");
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
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierBankDetails.BankNameList = _banknameservice.getAllBankNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBankDetail(string bankname, string branch, string suppliercode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {
            List<SupplierBankDetail> SupplierBankList = new List<SupplierBankDetail>();
            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["SupplierBankList" + LogedinUSerName] != null)
            {
                var suppbanklist = TempData["SupplierBankList" + LogedinUSerName] as IEnumerable<SupplierBankDetail>;
                foreach (var list in suppbanklist)
                {
                    SupplierBankList.Add(list);
                }
            }

            SupplierBankDetail BankDetails = new SupplierBankDetail();
            BankDetails.BankName = bankname;
            BankDetails.Branch = branch;
            BankDetails.BankIFSCNo = bankifscno;
            BankDetails.BankAddress = bankaddress;
            BankDetails.AccountNo = accountno;
            BankDetails.AccountType = accounttype;
            if (micr == "")
            {
                BankDetails.MICRCode = null;
            }
            else
            {
                BankDetails.MICRCode = micr;
            }
            
            SupplierBankList.Add(BankDetails);
            TempData["SupplierBankList" + LogedinUSerName] = SupplierBankList;
            return RedirectToAction("CreateBankDetail");
        }

        [HttpGet]
        public ActionResult UpdateBankDetail(int selectedbankname)
        {
            MainApplication model = new MainApplication()
            {
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierBankDetails = _SupplierBankDetailService.GetBankDetailsOfSupplier(selectedbankname);
            model.SupplierBankDetails.BankBranchList = _bankservice.GetBranchByBankName(model.SupplierBankDetails.BankName);
            model.SupplierBankDetails.BankNameList = _banknameservice.getAllBankNames();
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateBankDetail(int bankid, string bankname, string branch, string suppliercode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {
            MainApplication model = new MainApplication()
            {
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierBankDetails.BankId = bankid;
            model.SupplierBankDetails.BankName = bankname;
            model.SupplierBankDetails.Branch = branch;
            model.SupplierBankDetails.SupplierCode = suppliercode;
            model.SupplierBankDetails.BankIFSCNo = bankifscno;
            model.SupplierBankDetails.BankAddress = bankaddress;
            model.SupplierBankDetails.AccountNo = accountno;
            model.SupplierBankDetails.AccountType = accounttype;
            if (micr == "")
            {
                model.SupplierBankDetails.MICRCode = null;
            }
            else
            {
                model.SupplierBankDetails.MICRCode = micr;
            }
            model.SupplierBankDetails.Status = "Active";
            model.SupplierBankDetails.ModifiedOn = DateTime.Now;
            _SupplierBankDetailService.UpdateBankDetails(model.SupplierBankDetails);
            return RedirectToAction("UpdateBankDetail", "Supplier", new { selectedbankname = bankid });
        }

        [HttpGet]
        public ActionResult PopUpPage(string suppliercode)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            SupplierBankDetail supplierbankdetails = new SupplierBankDetail();
            supplierbankdetails.SupplierCode = suppliercode;
            supplierbankdetails.BankNameList = _banknameservice.getAllBankNames();
            return View(supplierbankdetails);
        }

        [HttpPost]
        public JsonResult PopUpPage(string bankname, string branch, string suppliercode, string bankifscno,
            string bankaddress, string accountno, string accounttype,string micr)
        {
            SupplierBankDetail supplierbankdetails = new SupplierBankDetail();
            supplierbankdetails.BankName = bankname;
            supplierbankdetails.Branch = branch;
            supplierbankdetails.SupplierCode = suppliercode;
            supplierbankdetails.BankIFSCNo = bankifscno;
            supplierbankdetails.BankAddress = bankaddress;
            supplierbankdetails.AccountNo = accountno;
            supplierbankdetails.AccountType = accounttype;
            if (micr == "")
            {
                supplierbankdetails.MICRCode = null;
            }
            else
            {
                supplierbankdetails.MICRCode = micr;
            }
            
            supplierbankdetails.Status = "Active";
            supplierbankdetails.ModifiedOn = DateTime.Now;
            _SupplierBankDetailService.CreateBankDetails(supplierbankdetails);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdatedBankDetails(string suppliercode)
        {
            MainApplication model = new MainApplication()
            {
                SupplierBankDetails = new SupplierBankDetail(),
            };
            var bankdetaillist = _SupplierBankDetailService.GetDetailsFromBank(suppliercode);
            string namebranch = string.Empty;
            List<SupplierBankDetail> supbanklist = new List<SupplierBankDetail>();
            foreach (var bankdetail in bankdetaillist)
            {
                SupplierBankDetail suppbank = new SupplierBankDetail();
                namebranch = bankdetail.BankName + ", " + bankdetail.Branch;
                suppbank.BankName = namebranch;
                suppbank.BankId = bankdetail.BankId;
                supbanklist.Add(suppbank);
                namebranch = string.Empty;
            }
            model.SupplierBankDetailList = supbanklist;
            var banknamelist = model.SupplierBankDetailList.Select(m => new SelectListItem()
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
                SupplierBankDetails = new SupplierBankDetail(),
            };
            model.SupplierBankDetails.BankBranchList = _bankservice.GetBranchByBankName(BankName);
            var modelData = model.SupplierBankDetails.BankBranchList.Select(m => new SelectListItem()
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