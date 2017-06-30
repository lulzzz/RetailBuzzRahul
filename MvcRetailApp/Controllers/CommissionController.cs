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
    [NoDirectAccess]
    [SessionExpireFilter]
    public class CommissionController : Controller
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
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IEmployeeMasterService _EmployeeService;
        private readonly ICommissionService _CommissionService;
        private readonly ICommissionProductService _CommissionProductService;
        private readonly IUnitService _UnitService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IItemService _ItemService;

        public CommissionController(IUtilityService utilityService, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IEmployeeMasterService EmployeeService, ICommissionService CommissionService, IUnitService UnitService, ICommissionProductService CommissionProductService, 
            IItemSubCategoryService ItemSubCategoryService,IItemService ItemService)
        {
            this._utilityService = utilityService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._EmployeeService = EmployeeService;
            this._CommissionService = CommissionService;
            this._CommissionProductService = CommissionProductService;
            this._UnitService = UnitService;
            this._ItemSubCategoryService = ItemSubCategoryService;
            this._ItemService = ItemService;
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
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                CommissionMasterDetails = new CommissionMaster()
            };

            var details = _CommissionService.GetLast();
            int lastid = 0;
            int length = 0;
            if (details != null)
            {
                lastid = details.CommissionId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            
            string commcode = _utilityService.getName("CMS", length, lastid);
            model.CommissionMasterDetails.CommissionCode = commcode;
            TempData["commissioncode"] = commcode;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.EmpList = _EmployeeService.GetEmployeeListBySalesAndTarget();
            model.UnitList = _UnitService.getallsize();
            model.ItemSubCategoryList = _ItemSubCategoryService.GetItemSubCategories();
            return View(model);
        }

        [HttpGet]
        public JsonResult CheckDate(string FromDate, string ToDate, string EmpName)
        {
            DateTime FD = Convert.ToDateTime(FromDate);
            DateTime TD = Convert.ToDateTime(ToDate);
            string message = "Success";
            var details = _CommissionService.CheckDate(FD, TD, EmpName);

            if (details != null)
            {
                message = "Error";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnits()
        {
            var units = _UnitService.getallsize();
            var select = units.Select(m => new SelectListItem
            {
                Value = m.UnitName,
                Text = m.UnitName,
            });
            return Json(select, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(MainApplication model, FormCollection form)
        {
            var details = _CommissionService.GetLast();
            int lastid = 0;
            int length = 0;
            if (details != null)
            {
                lastid = details.CommissionId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string commcode = _utilityService.getName("CMS", length, lastid);
            model.CommissionMasterDetails.CommissionCode = commcode;
            

            model.CommissionProductDetails = new CommissionProduct();

            model.CommissionMasterDetails.ModifiedOn = DateTime.Now;
            model.CommissionMasterDetails.Status = "Active";
            
            if(model.CommissionMasterDetails.ItemCode != null)
            {
                model.CommissionMasterDetails.ItemName = _ItemService.GetDescriptionByItemCode(model.CommissionMasterDetails.ItemCode).itemName;
            }
            _CommissionService.Create(model.CommissionMasterDetails);

            int count = Convert.ToInt32(form["rowcount"]);

            for (int i = 1; i <= count; i++)
            {
                var product = "product" + i;
                var productamount = "productamount" + i;
                var productpercent = "productpercent" + i;

                if (form[product] != null && form[product] != "")
                {
                    model.CommissionProductDetails.CommissionCode = model.CommissionMasterDetails.CommissionCode;
                    model.CommissionProductDetails.EmployeeName = model.CommissionMasterDetails.EmployeeName;
                    model.CommissionProductDetails.UnitName = form[product];
                    model.CommissionProductDetails.UnitAmount = form[productamount];
                    model.CommissionProductDetails.UnitPercent = Convert.ToDouble(form[productpercent]);
                    model.CommissionProductDetails.Status = "Active";
                    model.CommissionProductDetails.ModifiedOn = DateTime.Now;
                    _CommissionProductService.Create(model.CommissionProductDetails);
                }
            }

            var detailss = _CommissionService.GetLast();
            string Id = Encode(detailss.CommissionId.ToString());
            return RedirectToAction("CommissionDetails/" + Id, "Commission");
        }

        [HttpGet]
        public ActionResult CommissionDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.CommissionMasterDetails = _CommissionService.GetDetailsById(Id);
            model.CommissionProductList = _CommissionProductService.GetDetailsByCommCode(model.CommissionMasterDetails.CommissionCode);

            if (TempData["commissioncode"].ToString() != "")
            {
                string previouscommission = TempData["commissioncode"].ToString();
                if (previouscommission != model.CommissionMasterDetails.CommissionCode)
                {
                    ViewData["commissionchanged"] = previouscommission + " is replaced with " + model.CommissionMasterDetails.CommissionCode + " because " + previouscommission + " is acquired by another person";
                }
                TempData["commissioncode"] = previouscommission;

            }
            return View(model);
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
            TempData["commissioncode"] = "";
            return View(model);
        }

        [HttpGet]
        public ActionResult EditPartial(string Code)
        {
            MainApplication model = new MainApplication();
            model.CommissionMasterDetails = _CommissionService.GetDetailsByCommCode(Code);
            model.CommissionProductList = _CommissionProductService.GetDetailsByCommCode(Code);
            TempData["UnitList"] = model.CommissionProductList;
            model.UnitList = _UnitService.getallsize();
            model.ItemSubCategoryList = _ItemSubCategoryService.GetItemSubCategories();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(MainApplication model,FormCollection form)
        {
            model.CommissionMasterDetails.Status = "Active";
            model.CommissionMasterDetails.ModifiedOn = DateTime.Now;

            if (model.CommissionMasterDetails.ItemCode != null)
            {
                model.CommissionMasterDetails.ItemName = _ItemService.GetDescriptionByItemCode(model.CommissionMasterDetails.ItemCode).itemName;
            }

            _CommissionService.Update(model.CommissionMasterDetails);

            var details = TempData["UnitList"] as IEnumerable<CommissionProduct>;

            foreach (var data in details)
            {
                var row = _CommissionProductService.GetSingleRowByCommCode(data.CommissionCode);
                _CommissionProductService.Delete(row);
            }

            int count = Convert.ToInt32(form["rowcount"]);

            for (int i = 1; i <= count; i++)
            {
                var product = "product" + i;
                var productamount = "productamount" + i;
                var productpercent = "productpercent" + i;

                if (form[product] != null && form[product] != "")
                {
                    CommissionProduct unit = new CommissionProduct();
                    unit.CommissionCode = model.CommissionMasterDetails.CommissionCode;
                    unit.EmployeeName = model.CommissionMasterDetails.EmployeeName;
                    unit.UnitName = form[product];
                    unit.UnitAmount = form[productamount];
                    unit.UnitPercent = Convert.ToDouble(form[productpercent]);
                    unit.Status = "Active";
                    unit.ModifiedOn = DateTime.Now;
                    _CommissionProductService.Create(unit);
                }
            }
            TempData["Edit"] = "Edit";
            string id = Encode(model.CommissionMasterDetails.CommissionId.ToString());
            return RedirectToAction("CommissionDetails/" + id, "Commission");
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
            model.CommissionMasterList = _CommissionService.GetAllEmployees();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetEmployees(string FromDate, string ToDate)
        { 
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            var details = _CommissionService.GetEmployeesByDate(FDate, TDate);

            var select = details.Select(m => new SelectListItem
            {
                Text = m.EmployeeName,
                Value = m.CommissionCode,
            });

            return Json(select, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult DeletePartial(string Name)
        {
            MainApplication model = new MainApplication();
            model.CommissionMasterDetails = _CommissionService.GetDetailsByCommCode(Name);
            model.CommissionProductList = _CommissionProductService.GetDetailsByCommCode(Name);
            TempData["MasterList"] = model.CommissionMasterDetails;
            TempData["ProductList"] = model.CommissionProductList;
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(CommissionMaster comm)
        {
            comm.Status = "InActive";
            comm.ModifiedOn = DateTime.Now;
            _CommissionService.Update(comm);
            var productlist = _CommissionProductService.GetDetailsByCommCode(comm.CommissionCode);

            if (productlist != null)
            {
                foreach (var data in productlist)
                {
                    data.Status = "InActive";
                    data.ModifiedOn = DateTime.Now;
                    _CommissionProductService.Update(data);
                }
            }
            return RedirectToAction("Details/" + Convert.ToInt32(comm.CommissionId), "Commission");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            MainApplication model = new MainApplication();
            model.CommissionMasterDetails = _CommissionService.GetDetailsById(id);
            model.CommissionProductList = _CommissionProductService.GetDetailsByCommCode(model.CommissionMasterDetails.CommissionCode);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSellingPrice(string item)
        {
            var details = _ItemService.GetDescriptionByItemCode(item);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

    }
}
