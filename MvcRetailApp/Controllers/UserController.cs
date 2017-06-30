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
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.Routing;

namespace MvcRetailApp.Controllers
{
    public class UserController : Controller
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
        private readonly IUserService _iuserservice;
        private readonly IUtilityService _iutilityservice;
        private readonly IEmployeeMasterService _iemployeeservice;
        private readonly IModuleService _imoduleservice;
        private readonly IUserCredentialService _iusercredentailservice;
        private readonly IModuleService _iIModuleService;
        private readonly ICompanyService _iCompanyService;
        private readonly IEmployeesCompanyService _iEmployeesCompanyService;
        private readonly IShopService _ShopService;
        private readonly IGodownService _GodownService;
        public UserController(IUserCredentialService usercredentialservice, IModuleService moduleservice, IUserService userservice, IUtilityService utilityservice, IEmployeeMasterService employeeservice, IModuleService iIModuleService, ICompanyService companyservice, IEmployeesCompanyService EmployeesCompanyService, IShopService ShopService, IGodownService GodownService)
        {
            this._iuserservice = userservice;
            this._iutilityservice = utilityservice;
            this._iemployeeservice = employeeservice;
            this._imoduleservice = moduleservice;
            this._iusercredentailservice = usercredentialservice;
            this._iCompanyService = companyservice;
            this._iEmployeesCompanyService = EmployeesCompanyService;
            this._iIModuleService = iIModuleService;
            this._ShopService = ShopService;
            this._GodownService = GodownService;
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
        public ActionResult TimeOutExpired()
        {
            Response.Redirect("~/User/Login");
            return View();
        }

        public ActionResult Login()
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            if (TempData["NotEnoughRights"] != null)
            {
                ViewData["NotEnoughRights"] = "Yes";
            }
            else
            {
                ViewData["NotEnoughRights"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user, FormCollection frmUserCollection)
        {
            if (frmUserCollection["ChooseView"] == "Company")
            {
                //set the value of username & password
                var encryptPass = _iutilityservice.Encryptdata(user.Password);
                var userDetails = _iuserservice.GetLogin(user.UserName, encryptPass);
                if (userDetails == null)
                {
                    ViewBag.errormsg = "Invalid Username Or Password!";
                    if (user.UserName == "SuperAdmin")
                    {
                        MvcApplication.LoginTimes++;
                        if (MvcApplication.LoginTimes == 3)
                        {
                            SmtpClient client = new SmtpClient("180.149.243.22");
                            client.Port = 587;
                            client.EnableSsl = false;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
                            string body = string.Empty;
                            string IPAddress = string.Empty;
                            IPHostEntry HostEntry = Dns.GetHostEntry(Dns.GetHostName());
                            IPAddress[] AllIps = HostEntry.AddressList;
                            for (int i = 0; i < AllIps.Length; i++)
                            {
                                if (AllIps[i].AddressFamily.ToString() == "InterNetwork")
                                {
                                    IPAddress = AllIps[i].ToString();
                                    break;
                                }
                            }
                            MailMessage message = new MailMessage();
                            message.IsBodyHtml = true;
                            MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
                            message.From = messageFrom;
                            message.Subject = "Login";
                            body += "Dear SuperAdmin ,<br /><br />";
                            body += "\t Some Unauthorized person tried to log in to the Retail Software from Pc " + HostEntry.HostName + " " + IPAddress + " @" + DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.TimeOfDay;
                            message.Body = body;
                            MailAddress messageTo = new MailAddress("admin@retailmanagement.in");
                            message.To.Add(messageTo);
                            client.Send(message);
                            message.Dispose();
                            MvcApplication.LoginTimes = 0;
                        }
                    }

                    //field will get blank
                    user.UserName = "";
                    user.Password = "";
                }
                else
                {
                    UserEmail = userDetails.Email;
                    HttpContext.Session["UserName"] = userDetails.UserName;
                    HttpContext.Session["UserEmail"] = UserEmail;
                    if (userDetails != null)
                    {
                        if (userDetails.UserName == "SuperAdmin")
                        {
                            SmtpClient client = new SmtpClient("180.149.243.22");
                            client.Port = 587;
                            client.EnableSsl = false;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
                            string body = string.Empty;
                            string IPAddress = string.Empty;
                            IPHostEntry HostEntry = Dns.GetHostEntry(Dns.GetHostName());
                            IPAddress[] AllIps = HostEntry.AddressList;
                            for (int i = 0; i < AllIps.Length; i++)
                            {
                                if (AllIps[i].AddressFamily.ToString() == "InterNetwork")
                                {
                                    IPAddress = AllIps[i].ToString();
                                    break;
                                }
                            }
                            MailMessage message = new MailMessage();
                            message.IsBodyHtml = true;
                            MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
                            message.From = messageFrom;
                            message.Subject = "Login";
                            body += "Dear SuperAdmin ,<br /><br />";
                            body += "\t You are currently logged in to the Retail Software from PC " + HostEntry.HostName + " " + IPAddress + " @" + DateTime.Now.ToString("dd/MM/yyyy") + " " + DateTime.Now.TimeOfDay;
                            message.Body = body;
                            MailAddress messageTo = new MailAddress("admin@retailmanagement.in");
                            message.To.Add(messageTo);
                            //client.Send(message);
                            message.Dispose();
                        }

                        return RedirectToAction("SelectCompany", "Company");
                    }
                }
                return View();
            }
            else
            {
                if (user.UserName == "SuperAdmin")
                {
                    var encryptPass = _iutilityservice.Encryptdata(user.Password);
                    var userDetails = _iuserservice.GetLogin(user.UserName, encryptPass);
                    if (userDetails == null)
                    {
                        ViewBag.errormsg = "Invalid Username Or Password!";
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("CreateUser");
                    }
                }
                else
                {
                    ViewBag.msg = "only superadmin can create user!";
                    return View();
                }
            }
        }

        [NoDirectAccess]
        [HttpGet]
        public ActionResult CreateUser()
        {
            //DatabaseName = "RetailManagement";
            //CompanyName = "RetailManagement";
            //FinancialYear = "RetailManagement";
            DatabaseName = "A2ZRetail";
            CompanyName = "A2ZRetail";
            FinancialYear = "A2ZRetail";
            User User = new User();
            //CREATE CASHIE RECEIVABLE CODE
            var userdata = _iuserservice.GetLastUser();
            int userval = 0;
            int length = 0;
            if (userdata != null)
            {
                userval = userdata.UserId;
                userval = userval + 1;
                length = userval.ToString().Length;
            }
            else
            {
                userval = 1;
                length = 1;
            }
            string usercode = _iutilityservice.getName("USR", length, userval);
            User.CompanyList = _iCompanyService.getAllCompanies();
            List<Company> CompanyList = new List<Company>();
            foreach (var data in User.CompanyList)
            {
                Company Company = new Company();
                Company.CompanyCode = data.CompanyCode;
                Company.companyName = data.companyName + " " + Convert.ToDateTime(data.FinancialYearFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(data.FinancialYearTo).ToString("dd/MM/yyyy");
                CompanyList.Add(Company);
            }
            User.CompanyList = new List<Company>();
            User.CompanyList = CompanyList;
            User.UserCode = usercode;
            return View(User);
        }

        [HttpGet]
        public JsonResult ValidateUserName(string name)
        {
            var userlist = _iuserservice.GetRegisteredUsersByName(name);
            string msg = string.Empty;
            if (userlist.Count() == 0)
            {
                msg = "Success";
            }
            else
            {
                msg = "Fail";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidateEmail(string email)
        {
            var userlist = _iuserservice.GetRegisteredUsersByEmail(email);
            string msg = string.Empty;
            if (userlist.Count() == 0)
            {
                msg = "Success";
            }
            else
            {
                msg = "Fail";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateUser(User User)
        {
            string pass = CreateRandomPassword();
            string encryptPass = _iutilityservice.Encryptdata(pass);
            string password = _iutilityservice.Decryptdata(encryptPass);
            SmtpClient client = new SmtpClient("180.149.243.22");
            client.Port = 587;
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
            string body = string.Empty;
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            MailAddress messageFrom = new MailAddress("admin@retailmanagement.in");
            message.From = messageFrom;
            message.Subject = "Login Credentials";
            body += "Dear " + User.UserName + ",<br /><br />";
            body += "Your Login Credentials for RetailManagement Software are given below<br /><br />";
            body += "User Name:  " + User.UserName + " <br/>";
            body += "Password:  " + password + " <br/>";
            message.Body = body;
            MailAddress messageTo = new MailAddress(User.Email);
            message.To.Add(messageTo);
            client.Send(message);
            message.Dispose();

            SmtpClient NewClient = new SmtpClient("180.149.243.22");
            NewClient.Port = 587;
            NewClient.EnableSsl = false;
            NewClient.UseDefaultCredentials = false;
            NewClient.Credentials = new NetworkCredential("admin@retailmanagement.in", "70L0u?9");
            string NewBody = string.Empty;
            MailMessage NewMessage = new MailMessage();
            message.IsBodyHtml = true;
            MailAddress NewMessageFrom = new MailAddress("admin@retailmanagement.in");
            NewMessage.From = messageFrom;
            NewMessage.Subject = "Login Credentials";
            NewBody += "Dear " + User.UserName + ",<br /><br />";
            NewBody += "Your Login Credentials for RetailManagement Software are given below<br /><br />";
            NewBody += "User Name:  " + User.UserName + " <br/>";
            NewBody += "Password:  " + password + " <br/>";
            NewMessage.Body = body;
            MailAddress NewMessageTo = new MailAddress("admin@retailmanagement.in");
            NewMessage.To.Add(NewMessageTo);
            NewClient.Send(NewMessage);
            NewMessage.Dispose();

            User.Password = encryptPass;
            User.ModifiedBy = DateTime.Now;
            User.Status = "Active";
            _iuserservice.CreateUser(User);

            EmployeesCompany Empcomp = new EmployeesCompany();
            Empcomp.CompanyCode = User.CompanyCode;
            Empcomp.UserCode = User.UserCode;
            _iEmployeesCompanyService.Create(Empcomp);
            return RedirectToAction("UserCreated");
        }

        [HttpGet]
        public ActionResult UserCreated()
        {
            return View();
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

        [HttpGet]
        public ActionResult Home()
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
                module = new Module(),
            };

            model.modulelist = _imoduleservice.getAllModules();

            return View(model);
        }

        [HttpGet]
        public ActionResult UserRights()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
                usercredential = new UserCredential(),

            };
            model.userCredentialList = _iusercredentailservice.GetUserCredentialsByEmail("superadmin@gmail.com");
            model.modulelist = _iIModuleService.getAllModules();
            model.EmpList = _iemployeeservice.getActiveEmployee();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);

        }
        [HttpGet]
        public ActionResult GetModuleList(string email)
        {
            var list = _iusercredentailservice.GetByEmail(email);
            if (list == null)
            {
                return RedirectToAction("Add", "User");
            }
            TempData["loginId"] = email;
            return RedirectToAction("Update", "User");
        }

        [HttpGet]
        public ActionResult Update()
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
                module = new Module(),
            };
            string email = TempData["loginId"].ToString();
            model.modulelist = _imoduleservice.getAllModules();
            model.UpdatedUserCredentailList = _iusercredentailservice.GetUserCredentialsByEmail(email);
            model.userCredentialList = _iusercredentailservice.GetUserCredentialsByEmail("superadmin@gmail.com");
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmployeeDetails.Designation = _iemployeeservice.getEmpByEmail(UserEmail).Designation;
            //find login user start assign id
            var EmpDynamicStartIndex = _iusercredentailservice.GetDataByEmailAndComponyModuleId(email).AssignId;
            Session["EmpDynamicStartIndex"] = EmpDynamicStartIndex;

            //find shop and godown assign id's login range
            var godownlist = _GodownService.GetAll();
            var shoplist = _ShopService.GetAll();
            Session["DynamicShopGodownRange"] = godownlist.Count() + shoplist.Count();
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
                module = new Module(),
            };
            model.EmployeeDetails.Designation = _iemployeeservice.getEmpByEmail(UserEmail).Designation;
            model.modulelist = _imoduleservice.getAllModules();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddPermission(string moduleName, string moduleStatus, string email, string finalRights, string iconname, string assignrightcode)
        {
            var userDetails = _iemployeeservice.getEmpByEmail(email);
            string[] names = moduleName.Split('$');
            string[] status = moduleStatus.Split('$');
            string[] rights = finalRights.Split('$');
            string[] icons = iconname.Split('$');
            string[] assigncode = assignrightcode.Split('$');

            for (int i = 0; i < (names.Length - 1); i++)
            {
                UserCredential temp = new UserCredential();
                temp.Email = email;
                temp.UseName = userDetails.Name;
                temp.Modules = names[i];
                temp.Status = status[i];
                temp.Icon = icons[i];
                if (assigncode[i] == "")
                { temp.AssignRightsCode = null; }
                else
                {
                    temp.AssignRightsCode = assigncode[i];
                }
                temp.ModuleRights = rights[i];
                temp.ModuleId = _imoduleservice.getModuleIdByModulename(names[i]);

                //FOR SAVE DASHBOARD ID WHEN ASSIGN USER CREDENTIAL
                if (temp.Status == null)
                {
                    temp.Status = "";
                }

                if (temp.Status != "")
                {
                    var DashId = _imoduleservice.GetDataByModuleId(temp.ModuleId).DashboardId;
                    temp.AssignDashboardId = DashId;
                }
                else
                {
                    temp.AssignDashboardId = "";
                }
                _iusercredentailservice.CreateUserCredential(temp);
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updatePermissions(string moduleName, string moduleStatus, string email, string finalRights, string iconname)
        {
            var userDetails = _iemployeeservice.getEmpByEmail(email);
            var tempList = _iusercredentailservice.GetUserCredentialsByEmail(email);
            string[] names = moduleName.Split('$');
            string[] status = moduleStatus.Split('$');
            string[] rights = finalRights.Split('$');
            string[] icons = iconname.Split('$');
            int i = 0;
            foreach (var item in tempList)
            {
                item.Modules = names[i];
                item.Status = status[i];
                item.ModuleRights = rights[i];
                item.Icon = icons[i];

                //FOR SAVE DASHBOARD ID WHEN ASSIGN USER CREDENTIAL
                item.AssignDashboardId = "";
                if (item.Status == null)
                {
                    item.Status = "";
                }

                if (item.Status != "")
                {
                    var DashId = _imoduleservice.GetDataByModuleId(item.ModuleId).DashboardId;
                    item.AssignDashboardId = DashId;
                }
                else
                {
                    item.AssignDashboardId = "";
                }
                _iusercredentailservice.UpdateUserCredential(item);
                i++;
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EmailValidate(string email)
        {
            string strPattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9_\\+-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$";
            string msg = string.Empty;
            if (System.Text.RegularExpressions.Regex.IsMatch(email, strPattern))
            {
                msg = "true";
            }
            else { msg = "false"; }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}

