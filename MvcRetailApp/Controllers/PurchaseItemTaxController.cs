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
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class PurchaseItemTaxController : Controller
    {
        private string UserEmail
        {
            get { return (string)HttpContext.Session["UserEmail"]; }
            set { HttpContext.Application["UserEmail"] = value; }
        }
        private string CompanyCode
        {
            get { return (string)HttpContext.Session["CompanyCode"]; }
            set { HttpContext.Application["CompanyCode"] = value; }
        }
        private string CompanyName
        {
            get { return (string)HttpContext.Session["CompanyName"]; }
            set { HttpContext.Application["CompanyName"] = value; }
        }
        private string FinancialYear
        {
            get { return (string)HttpContext.Session["FinancialYear"]; }
            set { HttpContext.Application["FinancialYear"] = value; }
        }
        private static string DatabaseName
        {
            set { ((dynamic)System.Web.HttpContext.Current.ApplicationInstance).DynamicDatabase = value; }
        }
        private readonly IUtilityService _utilityService;
        private readonly IPurchaseItemTaxService _purchaseitemtaxService;
        private readonly IItemService _itemservice;
        private readonly IItemCategoryService _itemcategoryService;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly ICityService _cityservice;
        private readonly IStateService _StateService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public PurchaseItemTaxController(IUtilityService utilityService, IPurchaseItemTaxService purchaseitemtaxService, IItemService itemservice, IItemCategoryService itemcategoryService, IItemSubCategoryService itemsubcategoryservice, ICityService cityservice,
            IStateService StateService, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._utilityService = utilityService;
            this._purchaseitemtaxService = purchaseitemtaxService;
            this._itemservice = itemservice;
            this._itemcategoryService = itemcategoryService;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._cityservice = cityservice;
            this._StateService = StateService;
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

        //CREATE TAX MASTER
        [HttpGet]
        public ActionResult Create()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.ItemCategoryList = _itemcategoryService.GetAllItemCategories();
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.StateList = _StateService.GetAll();
            model.CityList = _cityservice.getAllCities();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //GET SUBCATEGORY BY CATEGORY
        [HttpGet]
        public JsonResult LoadSubCategoryByCategory(string cat)
        {
            var subcat = _itemsubcategoryservice.GetSubCategoryByCategory(cat);
            MainApplication model = new MainApplication();
            model.ItemSubCategoryList = subcat;
            var modeldata = model.ItemSubCategoryList.Select(m => new SelectListItem()
            {
                Text = m.subCategoryName,
                Value = m.subCategoryName
            });
            return Json(modeldata, JsonRequestBehavior.AllowGet);
        }

        //SAVE TAX MASTER
        [HttpPost]
        public ActionResult Create(MainApplication mainapp, FormCollection frmcollection)
        {
            MainApplication model = new MainApplication()
            {
                PurchaseItemTaxDetails = new PurchaseItemTaxMaster(),
                ItemTaxDetails = new ItemTaxMaster(),
            };

            int lastitembefore = 0;
            var data = _purchaseitemtaxService.GetLastRow();
            if (data == null)
            {
                lastitembefore = 1;
            }
            else
            {
                lastitembefore = data.TaxItemId + 1;
            }

            //SAVE DYNAMIOS ROWS
            string taxitem = frmcollection["hdnRowCount"].ToString();

            if (!string.IsNullOrEmpty(taxitem))
            {
                int count = Convert.ToInt32(taxitem);
                for (int i = 1; i <= count; i++)
                {
                    string subcat = "subcat" + i;
                    string taxtype = "taxtype" + i;
                    string percentage = "percentage" + i;

                    string finalsubcat = frmcollection[subcat];
                    string finaltaxtype = frmcollection[taxtype];
                    double finalpercentage = Convert.ToDouble(frmcollection[percentage]);

                    if (!string.IsNullOrEmpty(finalsubcat))
                    {
                        model.PurchaseItemTaxDetails.SubCategory = finalsubcat;
                        model.PurchaseItemTaxDetails.TaxType = finaltaxtype;
                        model.PurchaseItemTaxDetails.Percentage = finalpercentage;
                        model.PurchaseItemTaxDetails.Status = "Active";
                        model.PurchaseItemTaxDetails.ModifiedOn = DateTime.Now;

                        model.PurchaseItemTaxDetails.Category = mainapp.PurchaseItemTaxDetails.Category;
                        model.PurchaseItemTaxDetails.State = mainapp.PurchaseItemTaxDetails.State;
                        model.PurchaseItemTaxDetails.FromDate = mainapp.PurchaseItemTaxDetails.FromDate;
                        model.PurchaseItemTaxDetails.ToDate = mainapp.PurchaseItemTaxDetails.ToDate;

                        _purchaseitemtaxService.Create(model.PurchaseItemTaxDetails);

                        model.ItemTaxDetails.SubCategory = finalsubcat;
                        model.ItemTaxDetails.TaxType = finaltaxtype;
                        model.ItemTaxDetails.Percentage = finalpercentage;
                        model.ItemTaxDetails.Status = "Active";
                        model.ItemTaxDetails.ModifiedOn = DateTime.Now;

                        model.ItemTaxDetails.Category = mainapp.PurchaseItemTaxDetails.Category;
                        model.ItemTaxDetails.City = mainapp.PurchaseItemTaxDetails.State;
                        model.ItemTaxDetails.FromDate = mainapp.PurchaseItemTaxDetails.FromDate;
                        model.ItemTaxDetails.ToDate = mainapp.PurchaseItemTaxDetails.ToDate;

                    }
                }
            }
            int lastitemafter = _purchaseitemtaxService.GetLastRow().TaxItemId;
            var ItemBefore = Encode(lastitembefore.ToString());
            var ItemAfter = Encode(lastitemafter.ToString());
            return RedirectToAction("CreateDetails", "PurchaseItemTax", new { lastrowbefore = ItemBefore, lastitemafter = ItemAfter });
        }

        //SHOW DETAILS AFTER CREATE
        [HttpGet]
        public ActionResult CreateDetails(string lastrowbefore, string lastitemafter)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int BeforeId = Decode(lastrowbefore);
            int AfterId = Decode(lastitemafter);
            model.PurchaseItemTaxList = _purchaseitemtaxService.GetInsertedRows(BeforeId, AfterId);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //EDIT TAX MASTER
        [HttpGet]
        public ActionResult Edit()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.PurchaseItemTaxList = _purchaseitemtaxService.GetAll();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //SHOW EDIT DETAILS
        [HttpGet]
        public ActionResult EditPartial(string fromdate, string todate, string taxtype)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(fromdate);
            DateTime TDate = Convert.ToDateTime(todate);
            model.PurchaseItemTaxList = _purchaseitemtaxService.GetDetailsByDate(FDate, TDate, taxtype);
            TempData["PurchaseTaxItems"] = model.PurchaseItemTaxList;
            return View(model);
        }

        //SAVE EDIT DETAILS
        [HttpPost]
        public ActionResult EditPartial(MainApplication mainapp, FormCollection fromcol)
        {
            mainapp.PurchaseItemTaxDetails.Status = "Active";
            mainapp.PurchaseItemTaxDetails.ModifiedOn = DateTime.Now;

            mainapp.PurchaseItemTaxList = TempData["PurchaseTaxItems"] as IEnumerable<PurchaseItemTaxMaster>;
            string todate = string.Empty;
            int count = 1;

            int lastitembefore = 0;
            var row = _purchaseitemtaxService.GetLastRow();
            if (row == null)
            {
                lastitembefore = 1;
            }
            else
            {
                lastitembefore = row.TaxItemId + 1;
            }

            foreach (var data in mainapp.PurchaseItemTaxList)
            {

                todate = "todate" + count;
                if (fromcol[todate] != null)
                {
                    mainapp.PurchaseItemTaxDetails.State = data.State;
                    mainapp.PurchaseItemTaxDetails.Category = data.Category;
                    mainapp.PurchaseItemTaxDetails.SubCategory = data.SubCategory;
                    mainapp.PurchaseItemTaxDetails.TaxType = data.TaxType;
                    mainapp.PurchaseItemTaxDetails.FromDate = data.FromDate;

                    mainapp.PurchaseItemTaxDetails.Percentage = data.Percentage;
                    mainapp.PurchaseItemTaxDetails.Status = "Active";
                    mainapp.PurchaseItemTaxDetails.ModifiedOn = DateTime.Now;
                    if (Convert.ToDateTime(fromcol[todate]) != data.ToDate)
                    {
                        mainapp.PurchaseItemTaxDetails.ToDate = Convert.ToDateTime(fromcol[todate]);
                        _purchaseitemtaxService.Create(mainapp.PurchaseItemTaxDetails);
                    }
                    count = count + 1;
                }
            }
            int lastitemafter = _purchaseitemtaxService.GetLastRow().TaxItemId;
            return RedirectToAction("UpdateDetails", "ItemTax", new { lastrowbefore = lastitembefore, lastrowafter = lastitemafter });
        }

        [HttpGet]
        public ActionResult UpdateDetails(int lastrowbefore, int lastrowafter)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.PurchaseItemTaxList = _purchaseitemtaxService.GetInsertedRows(lastrowbefore, lastrowafter);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetFinancialYear()
        {
            string year = FinancialYear;
            string[] FinYr = year.Split(' ', '-');
            string fromdate = string.Empty;
            string todate = string.Empty;
            fromdate = FinYr[1] + "/" + FinYr[0] + "/" + FinYr[2];
            todate = FinYr[5] + "/" + FinYr[4] + "/" + FinYr[6];
            fromdate = Convert.ToDateTime(fromdate).ToShortDateString();
            todate = Convert.ToDateTime(todate).ToShortDateString();
            return Json(new { fromdate, todate }, JsonRequestBehavior.AllowGet);
        }

    }
}
