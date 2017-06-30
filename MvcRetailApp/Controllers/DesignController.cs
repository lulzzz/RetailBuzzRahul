using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using MvcRetailApp.ModelBinder;
using CodeFirstServices.Interfaces;
using CodeFirstServices.Services;
using System.Configuration;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class DesignController : Controller
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
        private readonly IUtilityService _UtilityService;
        private readonly IDesignService _DesignService;
        private readonly IItemCategoryService _ItemCategoryService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IItemService _ItemService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public DesignController(IUtilityService utilityservice, IDesignService designservice, IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IItemService itemservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._UtilityService = utilityservice;
            this._DesignService = designservice;
            this._ItemCategoryService = itemcategoryservice;
            this._ItemSubCategoryService = itemsubcategoryservice;
            this._ItemService = itemservice;
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

        //CREATE DESIGN MASTER
        [HttpGet]
        public ActionResult Create()
        {
            MainApplication model = new MainApplication()
            {
                DesignDetails = new DesignMaster(),
            };

            string designcode = generateCode();
            int count = _DesignService.getDesignsByDesignCode(designcode);
            if (count == 0)
            {
                model.DesignDetails.DesignCode = designcode;
            }
            else
            {
                model.DesignDetails.DesignCode = Convert.ToString(designcode.Reverse());
            }

            //CONCAT CATEGORYCODE AND CATEGORYNAME
            var namelist = _ItemCategoryService.GetAllItemCategories();
            string categories = string.Empty;
            List<ItemCategory> catlist = new List<ItemCategory>();
            foreach (var data in namelist)
            {
                ItemCategory catdata = new ItemCategory();
                categories = data.ItemCategoryCode + "  " + data.CategoryName;
                catdata.ItemCategoryCode = categories;
                catdata.CategoryName = data.CategoryName;
                catlist.Add(catdata);
                categories = string.Empty;
            }
            model.ItemCategoryList = catlist;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }


        //SUBCATEGORY DROP DOWN LIST
        [HttpGet]
        public JsonResult LoadSubCatByCat(string category)
        {
            MainApplication model = new MainApplication();
            model.ItemSubCategoryList = _ItemSubCategoryService.GetSubCatByCatCode(category);
            var data = model.ItemSubCategoryList.Select(m => new SelectListItem
            {
                Text = m.subCategoryName,
                Value = m.subCategoryName,
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GANERETE DESIGN CODE
        [HttpGet]
        public static string generateCode()
        {
            string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random randNum = new Random();
            char[] chars = new char[4];
            int allowedCharCount = allowedChars.Length;
            for (int i = 0; i < 4; i++)
            {
                chars[i] = allowedChars[(int)((allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        //SUB CATEGORY PARTIAL VIEW
        [HttpGet]
        public ActionResult DesignDetailsPartial()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DesignDetailsPartial([ModelBinder(typeof(DesignCustomBinder))]DesignMaster itemdesign, HttpPostedFileBase file)
        {
            MainApplication model = new MainApplication();

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = ConfigurationManager.AppSettings["DesignPhotos"].ToString() + "/" + fileName;
                file.SaveAs(path);
              itemdesign.DesignPhoto = fileName;
                
            }
            itemdesign.CategoryCode = _ItemCategoryService.GetCodeByName(itemdesign.CategoryCode);
            _DesignService.CreateDesign(itemdesign);
            var DesignId = Encode(itemdesign.DesignId.ToString());
            return RedirectToAction("CreateDetails/" + DesignId, "Design");
        }

        //SHOW CREATE DETAILS
        [HttpGet]
        public ActionResult CreateDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.DesignDetails = _DesignService.GetDetailsById(Id);
            model.DesignDetails.DesignPhoto = "../../DesignPhotos/" + model.DesignDetails.DesignPhoto;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //DISPLAY DESIGN NAMES LIST
        [HttpGet]
        public ActionResult LoadDesignNames(string name)
        {
            MainApplication model = new MainApplication()
            {
                DesignDetails = new DesignMaster(),
            };
            model.DesignList = _DesignService.GetDesignNamesBySubCat(name);
            return View(model);
        }


        //EDIT DESIGN DETAILS
        [HttpGet]
        public ActionResult Edit()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            //CONCAT DESIGNCODE AND DESINNAME
            var namelist = _ItemCategoryService.GetAllItemCategories();
            string categories = string.Empty;
            List<ItemCategory> catlist = new List<ItemCategory>();
            foreach (var data in namelist)
            {
                ItemCategory catdata = new ItemCategory();
                categories = data.ItemCategoryCode + "  " + data.CategoryName;
                catdata.ItemCategoryCode = categories;
                catdata.CategoryName = data.CategoryName;
                catlist.Add(catdata);
                categories = string.Empty;
            }
            model.ItemCategoryList = catlist;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Edit";
            return View(model);
        }

        //PARTIAL VIEW FOR EDIT
        [HttpGet]
        public ActionResult EditPartial(int Id)
        {
            MainApplication model = new MainApplication();
            model.DesignDetails = _DesignService.GetDetailsById(Id);
            TempData["DesignPhotoName"] = model.DesignDetails.DesignPhoto;
            model.DesignDetails.DesignPhoto = "../DesignPhotos/" + model.DesignDetails.DesignPhoto;
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPartial(DesignMaster designmastr)
        {
            var designData = _DesignService.GetById(designmastr.DesignId);

            //designData.CategoryCode = designmastr.CategoryCode;
            designData.CategoryCode = _ItemCategoryService.GetCodeByName(designmastr.CategoryCode);
            designData.SubCategoryName = designmastr.SubCategoryName;
            designData.DesignCode = designmastr.DesignCode;
            designData.DesignDescription = designmastr.DesignDescription;
            designData.DesignId = designmastr.DesignId;
            designData.DesignName = designmastr.DesignName;
            //designData.DesignSize = designmastr.DesignSize;
            designData.DesignPhoto = designmastr.DesignPhoto;

            if (designmastr.DesignPhoto == null && TempData["DesignPhotoName"] == null)
            {
                designData.DesignPhoto = null;
            }
            else if (designmastr.DesignPhoto != null)
            {
                designData.DesignPhoto = designmastr.DesignPhoto;
            }
            else
            {
                designData.DesignPhoto = TempData["DesignPhotoName"].ToString();
            }

            designData.Status = "Active";
            designData.ModifiedOn = DateTime.Now;
            _DesignService.UpdateDesign(designData);

            if (Request.Files.Count != 0)
            {
                HttpPostedFileBase fileField = Request.Files[0];
                if (fileField != null)
                {
                    var fileName = Path.GetFileName(fileField.FileName);
                    System.IO.File.Delete(ConfigurationManager.AppSettings["DesignPhotos"].ToString() + "/" + designmastr.DesignPhoto);
                    var path = ConfigurationManager.AppSettings["DesignPhotos"].ToString() + "/" + fileName;
                    fileField.SaveAs(path);
                }
            }
            return RedirectToAction("UpdateDetails/" + designmastr.DesignId, "Design");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            //CONCAT DESIGNCODE AND DESINNAME
            var namelist = _ItemCategoryService.GetAllItemCategories();
            string categories = string.Empty;
            List<ItemCategory> catlist = new List<ItemCategory>();
            foreach (var data in namelist)
            {
                ItemCategory catdata = new ItemCategory();
                categories = data.ItemCategoryCode + "  " + data.CategoryName;
                catdata.ItemCategoryCode = categories;
                catdata.CategoryName = data.CategoryName;
                catlist.Add(catdata);
                categories = string.Empty;
            }
            model.ItemCategoryList = catlist;
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ViewType"] = "Delete";
            return View(model);
        }

        //PARTIAL VIEW FOR DELETE   
        [HttpGet]
        public ActionResult DeletePartial(int Id)
        {
            MainApplication model = new MainApplication();
            model.DesignDetails = _DesignService.GetById(Id);
            model.DesignDetails.DesignPhoto = "../DesignPhotos/" + model.DesignDetails.DesignPhoto;
            return View(model);
        }

        [HttpPost]
        public ActionResult DeletePartial(DesignMaster designmastr)
        {
            var designData = _DesignService.GetById(designmastr.DesignId);

            designData.CategoryCode = designmastr.CategoryCode;
            designData.SubCategoryName = designmastr.SubCategoryName;
            designData.DesignCode = designmastr.DesignCode;
            designData.DesignDescription = designmastr.DesignDescription;
            designData.DesignId = designmastr.DesignId;
            designData.DesignName = designmastr.DesignName;
            //designData.DesignSize = designmastr.DesignSize;
            designData.Status = "InActive";
            designData.ModifiedOn = DateTime.Now;

            _DesignService.UpdateDesign(designData);
            return RedirectToAction("UpdateDetails/" + designmastr.DesignId, "Design");
        }

        //SHOW EDITDELETE DETAILS
        [HttpGet]
        public ActionResult UpdateDetails(int Id)
        {
            DesignMaster data = new DesignMaster();
            data = _DesignService.GetById(Id);
            data.DesignPhoto = "../../DesignPhotos/" + data.DesignPhoto;
            return View(data);
        }

        [HttpGet]
        public JsonResult GetDesignCodeByName(string Name)
        {
            var details = _DesignService.GetNamebyDesignName(Name);
            return Json(details, JsonRequestBehavior.AllowGet);
        }
    }
}


