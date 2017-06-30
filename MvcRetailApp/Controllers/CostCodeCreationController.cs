using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;
using System.IO;
using System.Configuration;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class CostCodeCreationController : Controller
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

        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;
        private readonly ICostCodeCreationService _CostCodeCreationService;
        private readonly IEmployeeMasterService _EmployeeMasterService;

        public CostCodeCreationController(IUserCredentialService UserCredentialService, IModuleService ModuleService, ICostCodeCreationService CostCodeCreationService, IEmployeeMasterService EmployeeMasterService)
        {
            this._UserCredentialService = UserCredentialService;
            this._ModuleService = ModuleService;
            this._CostCodeCreationService = CostCodeCreationService;
            this._EmployeeMasterService = EmployeeMasterService;
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

        //CREATE COST CODE CREATION
        [HttpGet]
        public ActionResult CostCodeCreate()
        {
            MainApplication model = new MainApplication();
            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _ModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _UserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWGS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWGS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //SAVE COST CODE CREATION
        [HttpPost]
        public ActionResult CostCodeCreate(MainApplication mainapp,FormCollection frmcol)
        {
            var costcodedetails = _CostCodeCreationService.GetActiveData();
            if (costcodedetails != null)
            {
                foreach (var data in costcodedetails)
                {
                    data.Status = "InActive";
                    data.ActiveTo = DateTime.Now.ToString();
                    _CostCodeCreationService.Update(data);
                }
            }

            mainapp.CostCodeCreationDetails.Date = DateTime.Now;
            mainapp.CostCodeCreationDetails.ActiveFrom = DateTime.Now;
            mainapp.CostCodeCreationDetails.ActiveTo = "InProcess";
            mainapp.CostCodeCreationDetails.Status = "Active";
            mainapp.CostCodeCreationDetails.ModifiedOn = DateTime.Now;

            var username = HttpContext.Session["UserName"].ToString();
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                mainapp.CostCodeCreationDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                mainapp.CostCodeCreationDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
            }
            else
            {
                mainapp.CostCodeCreationDetails.ShopCode = "SuperAdmin";
                mainapp.CostCodeCreationDetails.ShopName = "SuperAdmin";
            }

            var empdetails = _EmployeeMasterService.getEmpByEmail(frmcol["preparedbyemail"]);
            mainapp.CostCodeCreationDetails.PreparedBy = empdetails.Name;

            _CostCodeCreationService.Create(mainapp.CostCodeCreationDetails);
            return RedirectToAction("CostCodeCreate");
        }

    }
}
