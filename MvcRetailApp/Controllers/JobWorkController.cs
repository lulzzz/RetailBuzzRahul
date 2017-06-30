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
    public class JobWorkController : Controller
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

        private readonly IJobWorkTypeService _JobWorkTypeService;
        private readonly IUtilityService _utilityService;
        private readonly IModuleService _ModuleService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IJobWorkerService _JobWorkerService;
        private readonly IStateService _StateService;
        private readonly ICityService _CityService;
        private readonly IBankNameService _BankNameService;
        private readonly IBloodGroupService _BloodGroupService;
        private readonly IYearExperienceService _YearExperienceService;
        private readonly IMonthExperienceService _MonthExperienceService;
        private readonly IDepartmentService _DepartmentService;
        private readonly IDesignationMasterService _DesignationMasterService;
        private readonly ITypeOfSupplierService _TypeOfSupplierService;
        private readonly IBankService _BankService;
        private readonly IJobWorkPaymentService _JobWorkPaymentService;
        private readonly IOutwardToTailorService _OutwardToTailorService;
        private readonly IOutwardToTailorItemService _OutwardToTailorItemService;
        private readonly IJobWorkStockService _JobWorkStockService;
        private readonly IJobWorkOutwardToClientService _JobWorkOutwardToClientService;

        public JobWorkController(IJobWorkTypeService JobWorkTypeService, IUtilityService UtilityService, IModuleService ModuleService,
            IUserCredentialService UserCredentialService, IJobWorkerService JobWorkerService, IStateService StateService, ICityService CityService,
            IBankNameService BankNameService, IBloodGroupService BloodGroupService, IYearExperienceService YearExperienceService, IMonthExperienceService MonthExperienceService,
            IDepartmentService DepartmentService, IDesignationMasterService DesignationMasterService, ITypeOfSupplierService TypeOfSupplierService,
            IBankService BankService, IJobWorkPaymentService JobWorkPaymentService, IOutwardToTailorService OutwardToTailorService,
            IOutwardToTailorItemService OutwardToTailorItemService, IJobWorkStockService JobWorkStockService, IJobWorkOutwardToClientService JobWorkOutwardToClientService)
        {
            this._JobWorkTypeService = JobWorkTypeService;
            this._utilityService = UtilityService;
            this._ModuleService = ModuleService;
            this._IUserCredentialService = UserCredentialService;
            this._JobWorkerService = JobWorkerService;
            this._StateService = StateService;
            this._CityService = CityService;
            this._BankNameService = BankNameService;
            this._BloodGroupService = BloodGroupService;
            this._YearExperienceService = YearExperienceService;
            this._MonthExperienceService = MonthExperienceService;
            this._DepartmentService = DepartmentService;
            this._DesignationMasterService = DesignationMasterService;
            this._TypeOfSupplierService = TypeOfSupplierService;
            this._BankService = BankService;
            this._JobWorkPaymentService = JobWorkPaymentService;
            this._OutwardToTailorService = OutwardToTailorService;
            this._OutwardToTailorItemService = OutwardToTailorItemService;
            this._JobWorkStockService = JobWorkStockService;
            this._JobWorkOutwardToClientService = JobWorkOutwardToClientService;
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

        //CREATE JOB WORK TYPE
        [HttpGet]
        public ActionResult CreateJobWorkType()
        {
            MainApplication model = new MainApplication()
            {
                JobWorkTypeDetails = new JobWorkType(),
            };

            var details = _JobWorkTypeService.GetLastRow();
            int Typeval = 0;
            int length = 0;
            if (details != null)
            {
                Typeval = details.Id;
                Typeval = Typeval + 1;
                length = Typeval.ToString().Length;
            }
            else
            {
                Typeval = 1;
                length = 1;
            }
            String code = _utilityService.getName("JWT", length, Typeval);
            model.JobWorkTypeDetails.Code = code;
            TempData["PreviousJobWorkType"] = code;

            if (TempData["TypeList"] != null)
            {
                ViewBag.error = "Type Already Present In Database..";
            }

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //CATEGORY NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetJobWorkType(string term)
        {
            var result = _JobWorkTypeService.GetJobWorkTypeList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.Type);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //SAVE JOB WORK TYPE
        [HttpPost]
        public ActionResult CreateJobWorkType(MainApplication mainapp)
        {
            var details = _JobWorkTypeService.GetLastRow();
            int Typeval = 0;
            int length = 0;
            if (details != null)
            {
                Typeval = details.Id;
                Typeval = Typeval + 1;
                length = Typeval.ToString().Length;
            }
            else
            {
                Typeval = 1;
                length = 1;
            }
            String code = _utilityService.getName("JWT", length, Typeval);
            mainapp.JobWorkTypeDetails.Code = code;

            var worktype = _JobWorkTypeService.GetJobWorkType(mainapp.JobWorkTypeDetails.Type);
            if (worktype.Count() != 0)
            {
                TempData["TypeList"] = "error";
                return RedirectToAction("CreateJobWorkType");
            }
            else
            {
                mainapp.JobWorkTypeDetails.Status = "Active";
                mainapp.JobWorkTypeDetails.ModifiedOn = DateTime.Now;
                _JobWorkTypeService.Create(mainapp.JobWorkTypeDetails);
            }

            var id = _JobWorkTypeService.GetLastRow().Id;
            var TypeId = Encode(id.ToString());
            return RedirectToAction("CreateJobWorkTypeDetails/" + TypeId, "JobWork");
        }

        //SHOW JOB WORK TYPE DETAILS AFTER CREATE
        [HttpGet]
        public ActionResult CreateJobWorkTypeDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.JobWorkTypeDetails = _JobWorkTypeService.GetJobWorkTypeById(Id);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previousjobworktype = TempData["PreviousJobWorkType"].ToString();
            if (TempData["PreviousJobWorkType"].ToString() != model.JobWorkTypeDetails.Code)
            {
                ViewData["JobWorkTypeChanged"] = previousjobworktype + " is replaced with " + model.JobWorkTypeDetails.Code + " because " + previousjobworktype + " is acquired by another person";
            }
            TempData["PreviousJobWorkType"] = previousjobworktype;

            return View(model);
        }

        //******************************************** JOB WORKER ****************************************************

        //CREATE JOB WORKER
        [HttpGet]
        public ActionResult CreateJobWorker()
        {
            MainApplication model = new MainApplication()
            {
                JobWorkerDetails = new JobWorker(),
            };
            JobWorker worker = _JobWorkerService.GetLastRow();
            int Val = 0;
            int length = 0;
            if (worker != null)
            {
                Val = worker.Id;
                Val = Val + 1;
                length = Val.ToString().Length;
            }
            else
            {
                Val = 1;
                length = 1;
            }
            string code = _utilityService.getName("JW", length, Val);
            model.JobWorkerDetails.Code = code;
            TempData["PreviousJobWorker"] = code;

            string mssgbox = string.Empty;

            model.BankNameList = _BankNameService.getAllBankNames();
            model.StateList = _StateService.GetStateByCountry(1);
            model.BloodGroups = _BloodGroupService.GetBloodGroup();
            model.totalExpYears = _YearExperienceService.GetYearExp();
            model.totalExpmonths = _MonthExperienceService.GetMonthExp();
            model.DepartmentList = _DepartmentService.getAllDepartments();
            model.DesignationList = _DesignationMasterService.getAllDesignation();
            model.TypeOfSupplierList = _TypeOfSupplierService.GetTypeOfSuppliers();

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult ValidateEmail(string mail)
        {
            string message = string.Empty;
            var jobworkerlist = _JobWorkerService.GetEmail(mail);
            if (jobworkerlist.Count() != 0)
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
        public JsonResult GetBranch(string BankName)
        {
            MainApplication model = new MainApplication()
            {
                JobWorkerDetails = new JobWorker()
            };
            model.BankList = _BankService.GetBranchByBankName(BankName);
            var modelData = model.BankList.Select(m => new SelectListItem()
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
            string IFSCNo = _BankService.GetIfscByBranch(branch);
            string BankAddress = _BankService.GetAddressByIFSC(IFSCNo);
            return Json(new { IFSCNo, BankAddress }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCityByState(int id)
        {
            var city = _CityService.GetCityByState(id);
            EmployeeMaster EmpMaster = new EmployeeMaster();
            EmpMaster.CityList = city;
            var modelData = EmpMaster.CityList.Select(m => new SelectListItem()
            {
                Text = m.cityName,
                Value = m.cityId.ToString()
            });
            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateJobWorker(MainApplication model, HttpPostedFileBase file)
        {
            MainApplication mainapp = new MainApplication()
            {
                user = new User(),
            };

            JobWorker worker = _JobWorkerService.GetLastRow();
            int Val = 0;
            int length = 0;
            if (worker != null)
            {
                Val = worker.Id;
                Val = Val + 1;
                length = Val.ToString().Length;
            }
            else
            {
                Val = 1;
                length = 1;
            }
            string Code = _utilityService.getName("JW", length, Val);


            string msgbox = string.Empty;
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = ConfigurationManager.AppSettings["JobWorkerPhotos"].ToString() + "/" + fileName;
                file.SaveAs(path);

                model.JobWorkerDetails.Photo = fileName;
            }
            if (!string.IsNullOrEmpty(msgbox))
            {
                TempData["errorMsg"] = msgbox;
                return RedirectToAction("CreateJobWorker", "JobWork");
            }

            Int32 sid = Convert.ToInt32(model.JobWorkerDetails.State);
            model.JobWorkerDetails.State = _StateService.GetStateNamebyId(sid);
            Int32 cid = Convert.ToInt32(model.JobWorkerDetails.City);
            model.JobWorkerDetails.City = _CityService.GetCityNamebyId(cid);
            model.JobWorkerDetails.Code = Code;
            model.JobWorkerDetails.Status = "Active";
            model.JobWorkerDetails.ModifiedOn = DateTime.Now;
            model.JobWorkerDetails.CompanyCode = CompanyCode;
            _JobWorkerService.Create(model.JobWorkerDetails);

            var Id = _JobWorkerService.GetLastRow().Id;
            var WorkerId = Encode(Id.ToString());
            return RedirectToAction("CreateJobWorkerDetails/" + WorkerId, "JobWork");
        }

        [HttpGet]
        public ActionResult CreateJobWorkerDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                JobWorkerDetails = new JobWorker(),
            };

            int Id = Decode(id);
            model.JobWorkerDetails = _JobWorkerService.GetDetailsById(Id);
            model.JobWorkerDetails.Photo = "../../JobWorkerPhotos/" + model.JobWorkerDetails.Photo;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previousjobworker = TempData["PreviousJobWorker"].ToString();
            if (TempData["PreviousJobWorker"].ToString() != model.JobWorkerDetails.Code)
            {
                ViewData["JobWorkerChanged"] = previousjobworker + " is replaced with " + model.JobWorkerDetails.Code + " because " + previousjobworker + " is acquired by another person";
            }
            TempData["PreviousJobWorker"] = previousjobworker;

            return View(model);
        }

        //CREATE JOB WORK PAYMENT
        [HttpGet]
        public ActionResult JobWorkPayment()
        {
            MainApplication model = new MainApplication()
            {
                JobWorkPaymentDetails = new JobWorkPayment(),
            };

            //CREATE JOB WORK PAYMENT CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string paymentcode = string.Empty;

            var jobworkpaymentdata = _JobWorkPaymentService.GetLastPaymentByFinYr(FinYr);
            int PaymentVal = 0;
            int length = 0;
            if (jobworkpaymentdata != null)
            {
                paymentcode = jobworkpaymentdata.PaymentCode.Substring(3, 6);
                length = (Convert.ToInt32(paymentcode) + 1).ToString().Length;
                PaymentVal = Convert.ToInt32(paymentcode) + 1;
            }
            else
            {
                PaymentVal = 1;
                length = 1;
            }

            paymentcode = _utilityService.getName("JWP", length, PaymentVal);
            paymentcode = paymentcode + FinYr;
            model.JobWorkPaymentDetails.PaymentCode = paymentcode;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;

                int modulelastcount = _ModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE OUTWARD TO TAILOR NO
        [HttpGet]
        public JsonResult GetOutwardToTailorNos(string SearchTerm)
        {
            var data = _OutwardToTailorService.GetRowsByStatusAndInwardStatus(SearchTerm);
            List<string> No = new List<string>();
            foreach (var item in data)
            {
                No.Add(item.OutwardCode);
            }
            return Json(new { No }, JsonRequestBehavior.AllowGet);

        }

        //GET DETAILS BY OUTWARD TO TAILOR NO
        [HttpGet]
        public JsonResult GetDetailsByOutwardNo(string OutwardNo)
        {
            var data = _OutwardToTailorService.GetDetailsByCode(OutwardNo);
            return Json(new { data.ClientName, data.TailorName, data.AdvancePayment, data.GrandTotal, data.Balance }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY BILL ITEM DETAILS IN MAIN PAGE
        public ActionResult GetOutwardToTailorItemDetails(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.OutwardToTailorDetails = _OutwardToTailorService.GetDetailsByCode(OutwardNo);
            model.OutwardToTailorItemList = _OutwardToTailorItemService.GetRowsByCode(OutwardNo);
            return View(model);
        }

        // PARTIAL VIEW OF CASH PAYMENT DETAILS
        public ActionResult CashPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CARD PAYMENT DETAILS
        public ActionResult CardPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CHEQUE PAYMENT DETAILS
        public ActionResult ChequePayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CASH CARD CHEQUE PAYMENT DETAILS
        public ActionResult CashCardChequePayment()
        {
            return View();
        }

        //SAVE JOB WORK PAYMENT
        [HttpPost]
        public ActionResult JobWorkPayment(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                JobWorkOutwardToClientDetails = new JobWorkOutwardToClient(),
            };

            //CREATE JOB WORK PAYMENT CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string paymentcode = string.Empty;

            var jobworkpaymentdata = _JobWorkPaymentService.GetLastPaymentByFinYr(FinYr);
            int PaymentVal = 0;
            int length = 0;
            if (jobworkpaymentdata != null)
            {
                paymentcode = jobworkpaymentdata.PaymentCode.Substring(3, 6);
                length = (Convert.ToInt32(paymentcode) + 1).ToString().Length;
                PaymentVal = Convert.ToInt32(paymentcode) + 1;
            }
            else
            {
                PaymentVal = 1;
                length = 1;
            }

            paymentcode = _utilityService.getName("JWP", length, PaymentVal);
            paymentcode = paymentcode + FinYr;
            mainapp.JobWorkPaymentDetails.PaymentCode = paymentcode;

            //update previous job work payment status inactive..
            var jobworkpaymentlist = _JobWorkPaymentService.GetRowsByOutwardNo(mainapp.JobWorkPaymentDetails.OutwardToTailorNo);
            foreach (var row in jobworkpaymentlist)
            {
                row.Status = "InActive";
                _JobWorkPaymentService.Update(row);
            }

            //SAVE CASHIER RECEIVABLES PAYMENT DETAILS
            mainapp.JobWorkPaymentDetails.Cash_1000 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_1000"]);
            mainapp.JobWorkPaymentDetails.Cash_500 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_500"]);
            mainapp.JobWorkPaymentDetails.Cash_100 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_100"]);
            mainapp.JobWorkPaymentDetails.Cash_50 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_50"]);
            mainapp.JobWorkPaymentDetails.Cash_20 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_20"]);
            mainapp.JobWorkPaymentDetails.Cash_10 = Convert.ToInt32(frmcol["JobWorkPaymentDetails.Cash_10"]);
            mainapp.JobWorkPaymentDetails.Cash_1 = Convert.ToDouble(frmcol["JobWorkPaymentDetails.Cash_1"]);

            mainapp.JobWorkPaymentDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
            mainapp.JobWorkPaymentDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
            mainapp.JobWorkPaymentDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
            mainapp.JobWorkPaymentDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
            mainapp.JobWorkPaymentDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
            mainapp.JobWorkPaymentDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
            mainapp.JobWorkPaymentDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);

            mainapp.JobWorkPaymentDetails.TotalCash = Convert.ToDouble(frmcol["JobWorkPaymentDetails.TotalCash"]);

            mainapp.JobWorkPaymentDetails.SelectedCard = frmcol["Card"];

            mainapp.JobWorkPaymentDetails.CreditCardNo = frmcol["JobWorkPaymentDetails.CreditCardNo"];
            if (frmcol["JobWorkPaymentDetails.CreditCardAmount"] == "")
            {
                mainapp.JobWorkPaymentDetails.CreditCardAmount = 0;
                mainapp.JobWorkPaymentDetails.HandoverCreditAmt = 0;
            }
            else
            {
                mainapp.JobWorkPaymentDetails.CreditCardAmount = Convert.ToDouble(frmcol["JobWorkPaymentDetails.CreditCardAmount"]);
                mainapp.JobWorkPaymentDetails.HandoverCreditAmt = Convert.ToDouble(frmcol["JobWorkPaymentDetails.CreditCardAmount"]);
            }
            mainapp.JobWorkPaymentDetails.CreditCardType = frmcol["JobWorkPaymentDetails.CreditCardType"];
            mainapp.JobWorkPaymentDetails.CreditCardBank = frmcol["JobWorkPaymentDetails.CreditCardBank"];
            mainapp.JobWorkPaymentDetails.DebitCardNo = frmcol["JobWorkPaymentDetails.DebitCardNo"];
            mainapp.JobWorkPaymentDetails.DebitCardName = frmcol["JobWorkPaymentDetails.DebitCardName"];
            mainapp.JobWorkPaymentDetails.DebitCardType = frmcol["JobWorkPaymentDetails.DebitCardType"];
            mainapp.JobWorkPaymentDetails.DebitCardBank = frmcol["JobWorkPaymentDetails.DebitCardBank"];
            if (frmcol["JobWorkPaymentDetails.DebitCardAmount"] == "")
            {
                mainapp.JobWorkPaymentDetails.DebitCardAmount = 0;
                mainapp.JobWorkPaymentDetails.HandoverDebitAmt = 0;
            }
            else
            {
                mainapp.JobWorkPaymentDetails.DebitCardAmount = Convert.ToDouble(frmcol["JobWorkPaymentDetails.DebitCardAmount"]);
                mainapp.JobWorkPaymentDetails.HandoverDebitAmt = Convert.ToDouble(frmcol["JobWorkPaymentDetails.DebitCardAmount"]);
            }
            mainapp.JobWorkPaymentDetails.ChequeNo = frmcol["JobWorkPaymentDetails.ChequeNo"];
            mainapp.JobWorkPaymentDetails.ChequeAccNo = frmcol["JobWorkPaymentDetails.ChequeAccNo"];
            if (frmcol["JobWorkPaymentDetails.ChequeAmount"] == "")
            {
                mainapp.JobWorkPaymentDetails.ChequeAmount = 0;
                mainapp.JobWorkPaymentDetails.HandoverChequeAmt = 0;
            }
            else
            {
                mainapp.JobWorkPaymentDetails.ChequeAmount = Convert.ToDouble(frmcol["JobWorkPaymentDetails.ChequeAmount"]);
                mainapp.JobWorkPaymentDetails.HandoverChequeAmt = Convert.ToDouble(frmcol["JobWorkPaymentDetails.ChequeAmount"]);
            }
            if (mainapp.JobWorkPaymentDetails.ChequeNo != null && mainapp.JobWorkPaymentDetails.ChequeNo != "")
            {
                mainapp.JobWorkPaymentDetails.ChequeDate = Convert.ToDateTime(frmcol["JobWorkPaymentDetails.ChequeDate"]);
            }
            else
            {
                mainapp.JobWorkPaymentDetails.ChequeDate = null;
            }
            mainapp.JobWorkPaymentDetails.ChequeBank = frmcol["JobWorkPaymentDetails.ChequeBank"];
            mainapp.JobWorkPaymentDetails.ChequeBranch = frmcol["JobWorkPaymentDetails.ChequeBranch"];


            var outwarddetails = _OutwardToTailorService.GetDetailsByCode(mainapp.JobWorkPaymentDetails.OutwardToTailorNo);
            mainapp.JobWorkPaymentDetails.OutwardToTailorDate = outwarddetails.Date;
            mainapp.JobWorkPaymentDetails.ClientName = outwarddetails.ClientName;
            mainapp.JobWorkPaymentDetails.ClientAddress = outwarddetails.ClientAddress;
            mainapp.JobWorkPaymentDetails.ClientContact = outwarddetails.ClientContact;
            mainapp.JobWorkPaymentDetails.ClientEmail = outwarddetails.ClientEmail;
            mainapp.JobWorkPaymentDetails.TailorName = outwarddetails.TailorName;
            mainapp.JobWorkPaymentDetails.TailorAddress = outwarddetails.TailorAddress;
            mainapp.JobWorkPaymentDetails.TailorContact = outwarddetails.TailorContact;
            mainapp.JobWorkPaymentDetails.TailorEmail = outwarddetails.TailorEmail;
            mainapp.JobWorkPaymentDetails.GrandTotal = outwarddetails.GrandTotal;
            mainapp.JobWorkPaymentDetails.AdvancePayment = outwarddetails.AdvancePayment;
            mainapp.JobWorkPaymentDetails.Balance = Convert.ToDouble(frmcol["BalanceVal"]);
            mainapp.JobWorkPaymentDetails.ItemsDeliveredStatus = frmcol["Delivered"];

            var username = HttpContext.Session["UserName"].ToString();
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                mainapp.JobWorkPaymentDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                mainapp.JobWorkPaymentDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
            }
            else
            {
                mainapp.JobWorkPaymentDetails.ShopCode = "SuperAdmin";
                mainapp.JobWorkPaymentDetails.ShopName = "SuperAdmin";
            }

            mainapp.JobWorkPaymentDetails.Date = DateTime.Now;
            mainapp.JobWorkPaymentDetails.Status = "Active";
            mainapp.JobWorkPaymentDetails.ModifiedOn = DateTime.Now;

            //SAVE HANDOVER STATUS
            if (mainapp.JobWorkPaymentDetails.CreditCardAmount == 0 && mainapp.JobWorkPaymentDetails.DebitCardAmount == 0 && mainapp.JobWorkPaymentDetails.ChequeAmount == 0)
            {
                mainapp.JobWorkPaymentDetails.HandoverStatus = "InActive";
            }
            else
            {
                mainapp.JobWorkPaymentDetails.HandoverStatus = "Active";
            }
            _JobWorkPaymentService.Create(mainapp.JobWorkPaymentDetails);

            //UPDATE OUTWARD TO TAILOR PAYMENT AND BALANCE AFTER CASHIER PAYMENT
            var OutwardToTailorData = _OutwardToTailorService.GetDetailsByCode(mainapp.JobWorkPaymentDetails.OutwardToTailorNo);
            OutwardToTailorData.AdvancePayment = OutwardToTailorData.AdvancePayment + mainapp.JobWorkPaymentDetails.Payment;
            OutwardToTailorData.Balance = mainapp.JobWorkPaymentDetails.Balance;

            if (OutwardToTailorData.Balance <= 0)
            {
                OutwardToTailorData.Status = "InActive";
            }
            _OutwardToTailorService.Update(OutwardToTailorData);

            //UPDATE OUTWARD TO TAILOR ITEM INACTIVE AFTER CASHIER PAYMENT
            if (OutwardToTailorData.Balance <= 0)
            {
                var OutwardToTailorItemData = _OutwardToTailorItemService.GetRowsByCode(mainapp.JobWorkPaymentDetails.OutwardToTailorNo);
                foreach (var data in OutwardToTailorItemData)
                {
                    data.Status = "InActive";
                    _OutwardToTailorItemService.Update(data);
                }
            }

            if (frmcol["Delivered"] == "Yes")
            {
                //IF DELIVERED STATUS IS YES THEN UPDATE JOB WORK STOCK
                var jobworkstockitems = _JobWorkStockService.GetRowsByOutwardNo(mainapp.JobWorkPaymentDetails.OutwardToTailorNo);
                foreach (var item in jobworkstockitems)
                {
                    item.Status = "InActive";
                    _JobWorkStockService.Update(item);

                    //IF DELIVERED STATUS IS YES THEN ADD THIS STOCK IN JUOB WORK OUTWARD TO CLIENT
                    model.JobWorkOutwardToClientDetails.OutwardToTailorNo = mainapp.JobWorkPaymentDetails.OutwardToTailorNo;
                    model.JobWorkOutwardToClientDetails.PaymentCode = mainapp.JobWorkPaymentDetails.PaymentCode;
                    model.JobWorkOutwardToClientDetails.ClientName = mainapp.JobWorkPaymentDetails.ClientName;
                    model.JobWorkOutwardToClientDetails.TailorName = mainapp.JobWorkPaymentDetails.TailorName;
                    model.JobWorkOutwardToClientDetails.ItemName = item.ItemName;
                    model.JobWorkOutwardToClientDetails.Quantity = item.Quantity;
                    model.JobWorkOutwardToClientDetails.Narration = item.Narration;
                    model.JobWorkOutwardToClientDetails.ShopCode = mainapp.JobWorkPaymentDetails.ShopCode;
                    model.JobWorkOutwardToClientDetails.ShopName = mainapp.JobWorkPaymentDetails.ShopName;
                    model.JobWorkOutwardToClientDetails.Status = "Active";
                    model.JobWorkOutwardToClientDetails.ModifiedOn = DateTime.Now;
                    _JobWorkOutwardToClientService.Create(model.JobWorkOutwardToClientDetails);
                }
            }

            return RedirectToAction("JobWorkPayment");
        }
    }
}
