using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class EmployeeController : Controller
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
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IYearExperienceService _YearExperienceService;
        private readonly IMonthExperienceService _MonthExperienceService;
        private readonly IBloodGroupService _BloodGroupService;
        private readonly ITypeOfSupplierService _TypeOfSupplierService;
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        private readonly IDesignationMasterService _DesignationMasterService;
        private readonly IBankNameService _BankNameService;
        private readonly IBankService _BankService;
        private readonly IDepartmentService _departmentservive;
        private readonly IUtilityService _utilityservice;
        private readonly IUserService _iuserservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IEmployeesCompanyService _EmployeesCompanyService;
        public EmployeeController(IDepartmentService deptservice, IUtilityService utilityservive, IBankNameService banknameservice, IBankService bankservice, IDesignationMasterService DesignationMasterService, IEmployeeMasterService EmployeeMasterService, IYearExperienceService YearExperienceService,
                                        IMonthExperienceService MonthExperienceService, IBloodGroupService BloodGroupservice, ITypeOfSupplierService TypeOfSupplierSevice, ICityService cityService, IStateService stateService, IUserService userservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IEmployeesCompanyService EmployeesCompanyService)
        {
            this._EmployeeMasterService = EmployeeMasterService;
            this._MonthExperienceService = MonthExperienceService;
            this._YearExperienceService = YearExperienceService;
            this._BloodGroupService = BloodGroupservice;
            this._TypeOfSupplierService = TypeOfSupplierSevice;
            this._cityService = cityService;
            this._stateService = stateService;
            this._DesignationMasterService = DesignationMasterService;
            this._BankNameService = banknameservice;
            this._BankService = bankservice;
            this._utilityservice = utilityservive;
            this._departmentservive = deptservice;
            this._iuserservice = userservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
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

        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
            };
            EmployeeMaster emp = _EmployeeMasterService.getLastInsertedEmp();
            int lastid = 0;
            int length = 0;
            if (emp != null)
            {
                lastid = emp.EmpId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("EMP", length, lastid);
            model.EmployeeDetails.EmployeeCode = catCode;
            TempData["employeecode"] = catCode;

            string mssgbox = string.Empty;
            model.BloodGroups = _BloodGroupService.GetBloodGroup();
            model.totalExpYears = _YearExperienceService.GetYearExp();
            model.totalExpmonths = _MonthExperienceService.GetMonthExp();
            model.TypeOfSupplierList = _TypeOfSupplierService.GetTypeOfSuppliers();
            model.StateList = _stateService.GetStateByCountry(1);
            model.EmployeeDetails.deptlist = _departmentservive.getAllDepartments();
            model.EmployeeDetails.DesignationList = _DesignationMasterService.GetDesignations();
            model.EmployeeDetails.BankNameList = _BankNameService.getAllBankNames();
            model.EmployeeDetails.deptlist = _departmentservive.getAllDepartments();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult ValidateEmail(string mail)
        {
            string message = string.Empty;
            var employeelist = _EmployeeMasterService.GetEmail(mail);
            if (employeelist.Count() != 0)
            {
                message = "success";
            }
            else
            {
                message = "fail";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public static string CreateRandomPassword()
        {

            string _allowedChars =
            "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            Random randNum = new Random();
            char[] chars = new char[6];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < 6; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        [HttpPost]
        public ActionResult Create(MainApplication model, HttpPostedFileBase file)
        {
            MainApplication mainapp = new MainApplication()
            {
                user = new User(),
            };

            EmployeeMaster emp = _EmployeeMasterService.getLastInsertedEmp();
            int lastid = 0;
            int length = 0;
            if (emp != null)
            {
                lastid = emp.EmpId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _utilityservice.getName("EMP", length, lastid);

            string msgbox = string.Empty;
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = ConfigurationManager.AppSettings["EmployeePhotos"].ToString() + "/" + fileName;
                file.SaveAs(path);

                model.EmployeeDetails.Photo = fileName;
            }
            if (!string.IsNullOrEmpty(msgbox))
            {
                TempData["errorMsg"] = msgbox;
                return RedirectToAction("Create", "Employee");
            }
            Int32 sid = Convert.ToInt32(model.EmployeeDetails.State);
            model.EmployeeDetails.State = _stateService.GetStateNamebyId(sid);
            Int32 cid = Convert.ToInt32(model.EmployeeDetails.City);
            model.EmployeeDetails.City = _cityService.GetCityNamebyId(cid);
            model.EmployeeDetails.EmployeeCode = catCode;
            model.EmployeeDetails.Status = "Active";
            model.EmployeeDetails.ModifiedOn = DateTime.Now;
            model.EmployeeDetails.companyCode = CompanyCode;

            _EmployeeMasterService.CreateEmployee(model.EmployeeDetails);
            var EmpDetails = _EmployeeMasterService.getLastInsertedEmp();
            int empId = EmpDetails.EmpId;

            //Encoding the ShopId Parameter
            var encoded = Encode(empId.ToString());
            var Id = _EmployeeMasterService.getLastInsertedEmp().EmpId;
            return RedirectToAction("ViewDetailsCreate/" + encoded, "Employee");
        }

        //Encoding Function
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }


        [HttpGet]
        public ActionResult ViewDetailsCreate(string id)
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
            };

            byte[] encoded = Convert.FromBase64String(id);

            //Decoding the encoded value i.e retaining the original value of id
            string decodedvalue = System.Text.Encoding.UTF8.GetString(encoded);

            int idvalue = Convert.ToInt32(decodedvalue);
            model.EmployeeDetails = _EmployeeMasterService.getById(idvalue);
            model.EmployeeDetails.Photo = "../../EmployeePhotos/" + model.EmployeeDetails.Photo;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previousemployee = TempData["employeecode"].ToString();
            if (previousemployee != model.EmployeeDetails.EmployeeCode)
            {
                ViewData["employeechanged"] = previousemployee + " is replaced with " + model.EmployeeDetails.EmployeeCode + " because " + previousemployee + " is acquired by another person";
            }
            TempData["employeecode"] = previousemployee;

            return View(model);
        }

        // GET: /EmplyoeeMaster/Edit/5
        [HttpGet]
        public ActionResult Edit()
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
            };
            IEnumerable<DesignationMaster> empdesignationlist = _DesignationMasterService.GetDesignations();
            model.EmployeeDetails.DesignationList = empdesignationlist;
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
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
            };
            IEnumerable<DesignationMaster> empdesignationlist = _DesignationMasterService.getAllDesignation();
            model.EmployeeDetails.DesignationList = empdesignationlist;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["Type"] = "delete";
            return View(model);
        }

        [HttpPost]
        public ActionResult GetEmployeeDetails(string designation)
        {
            MainApplication model = new MainApplication();
            Session["empdesignation"] = designation;
            model.EmpList = _EmployeeMasterService.getEmpDetailsByDesignation(designation);
            TempData["EmployeeList"] = model.EmpList;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditDetails(int value)
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
            };

            model.EmployeeDetails = _EmployeeMasterService.getEmpById(value);
            TempData["PhotoName"] = model.EmployeeDetails.Photo;
            model.EmployeeDetails.Photo = "../EmployeePhotos/" + model.EmployeeDetails.Photo;
            model.EmployeeDetails.EmployeeCode = model.EmployeeDetails.EmployeeCode;
            model.EmployeeDetails.BankNameList = _BankNameService.getAllBankNames();
            model.EmployeeDetails.BranchList = _BankService.GetBranchByBankName(model.EmployeeDetails.BankName);
            model.EmployeeDetails.DesignationList = _DesignationMasterService.GetDesignations();
            model.EmployeeDetails.deptlist = _departmentservive.getAllDepartments();
            model.totalExpYears = _YearExperienceService.GetYearExp();
            model.totalExpmonths = _MonthExperienceService.GetMonthExp();
            model.BloodGroups = _BloodGroupService.GetBloodGroup();
            model.TypeOfSupplierList = _TypeOfSupplierService.GetTypeOfSuppliers();
            model.StateList = _stateService.GetStateByCountry(1);
            int id = _stateService.GetStateIdByName(model.EmployeeDetails.State);
            model.CityList = _cityService.GetCityByState(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult EmployeeDetails(string id)
        {
            MainApplication model = new MainApplication();
            List<EmployeeMaster> ListOfEmployee = TempData["EmployeeList"] as List<EmployeeMaster>;
            List<EmployeeMaster> FinalList = new List<EmployeeMaster>();
            FinalList = FinalList.Concat((IEnumerable<EmployeeMaster>)ListOfEmployee.Where(x => x.Name.ToLower().Contains(id.ToLower()))).Distinct().ToList();
            model.EmpList = FinalList;
            TempData["EmployeeList"] = FinalList;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditDetails(EmployeeMaster empmaster)
        {
            empmaster.Status = "Active";
            empmaster.ModifiedOn = DateTime.Now;
            if (empmaster.Photo == " ")
            {
                if (TempData["PhotoName"] != null)
                {
                    empmaster.Photo = TempData["PhotoName"].ToString();
                    TempData["PhotoName"] = empmaster.Photo;
                }
                else
                {
                    empmaster.Photo = "";
                }
            }
            Int32 sid = Convert.ToInt32(empmaster.State);
            empmaster.State = _stateService.GetStateNamebyId(sid);
            bool str = (empmaster.City).All(char.IsDigit);
            if (str != true)
            {

            }
            else
            {
                Int32 cid = Convert.ToInt32(empmaster.City);
                empmaster.City = _cityService.GetCityNamebyId(cid);
            }
            _EmployeeMasterService.UpdateEmployee(empmaster);
            return RedirectToAction("SuccessMsg", "Employee");
        }

        public JsonResult SuccessMsg()
        {
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertPhoto(int id)
        {
            if (Request.Files.Count != 0)
            {
                HttpPostedFileBase fileField = Request.Files[0];
                if (fileField != null)
                {
                    var fileName = Path.GetFileName(fileField.FileName);
                    if (TempData["PhotoName"] != null)
                    {
                        System.IO.File.Delete(ConfigurationManager.AppSettings["EmployeePhotos"].ToString() + "/" + TempData["PhotoName"].ToString());
                    }
                    var path = ConfigurationManager.AppSettings["EmployeePhotos"].ToString() + "/" + fileName;
                    fileField.SaveAs(path);
                }
            }
            return RedirectToAction("ViewDetails/" + id, "Employee");
        }

        [HttpGet]
        public ActionResult ViewDetails(int id)
        {
            EmployeeMaster Employee = new EmployeeMaster();
            Employee = _EmployeeMasterService.getById(id);
            //Employee.Photo = "../EmployeePhotos/" + Employee.Photo;
            Employee.DesignationList = _DesignationMasterService.getAllDesignation();
            TempData["EmployeeList"] = _EmployeeMasterService.getAllemployees();
            return View(Employee);
        }

        [HttpGet]
        public JsonResult GetBranch(string BankName)
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster()
            };
            model.EmployeeDetails.BranchList = _BankService.GetBranchByBankName(BankName);
            var modelData = model.EmployeeDetails.BranchList.Select(m => new SelectListItem()
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
            return Json(new { IFSCNo, BankAddress, Micr }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteDetails(int value)
        {
            MainApplication emp = new MainApplication();
            emp.EmployeeDetails = _EmployeeMasterService.getEmpById(value);
            return View(emp);
        }

        [HttpPost]
        public ActionResult DeleteDetails(EmployeeMaster empmaster)
        {
            empmaster.Status = "InActive";
            empmaster.ModifiedOn = DateTime.Now;
            _EmployeeMasterService.UpdateEmployee(empmaster);
            return RedirectToAction("ViewDetails/" + empmaster.EmpId, "Employee");
        }

        public JsonResult ValidateEmailForEmp(string id)
        {
            IEnumerable<EmployeeMaster> ListOfClient = _EmployeeMasterService.getAllemployees();
            string mssgbox = string.Empty;
            foreach (var item in ListOfClient)
            {
                if (item.Email.Equals(id.ToString()))
                {
                    mssgbox = "Email id is already register";
                    return Json(mssgbox, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(mssgbox, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadCityByState(int id)
        {
            var city = _cityService.GetCityByState(id);
            EmployeeMaster EmpMaster = new EmployeeMaster();
            EmpMaster.CityList = city;
            var modelData = EmpMaster.CityList.Select(m => new SelectListItem()
            {
                Text = m.cityName,
                Value = m.cityId.ToString()
            });
            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ShowEmployee(string id)
        {
            int EmpId = Convert.ToInt32(id);
            EmployeeMaster model = new EmployeeMaster();
            model = _EmployeeMasterService.getById(EmpId);
            model.Photo = "/EmployeePhotos/" + model.Photo;
            return View(model);
        }

        [HttpPost]
        public ActionResult EmployeeSearch(string id)
        {
            MainApplication model = new MainApplication();
            var list = new List<EmployeeMaster>();
            IEnumerable<EmployeeMaster> ListOfEmp = _EmployeeMasterService.GetEmployee(id);
            foreach (var item in ListOfEmp)
            {
                list.Add(item);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult DeletePartialData(string id)
        {
            int Eid = Convert.ToInt32(id);
            EmployeeMaster model = new EmployeeMaster();
            var Emp = _EmployeeMasterService.getById(Eid);
            Emp.Status = "InActive";
            model.ModifiedOn = DateTime.Now;
            _EmployeeMasterService.UpdateEmployee(Emp);
            string msg = "sucess";
            return Json(msg);
        }

        [HttpGet]
        public JsonResult GetStateById(string State)
        {
            int stateid = _stateService.GetStateIdByName(State);
            return Json(stateid, JsonRequestBehavior.AllowGet);
        }
    }
}