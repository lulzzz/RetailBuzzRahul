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

namespace MvcRetailApp.Controllers
{
    public class JobWorkItemController : Controller
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
        private readonly IJobWorkItemService _jobworkitemService;
        private readonly INonInventoryItemService _NonInventoryService;
        private readonly IItemCategoryService _ItemCategoryService;
        private readonly IItemSubCategoryService _ItemSubCategoryService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly ITypeOfMaterialService _TypeOfMaterialService;
        private readonly IColorCodeService _ColorCodeService;
        private readonly IDesignService _DesignService;
        private readonly IUnitService _UnitService;

        public JobWorkItemController(IJobWorkItemService jobworkitemService, INonInventoryItemService NonInventoryService, IUtilityService utilityService, IItemCategoryService ItemCategoryService,
                             IItemSubCategoryService ItemSubCategoryService, IUserCredentialService UserCredentialService, IModuleService ModuleService,ITypeOfMaterialService TypeOfMaterialService,
                              IColorCodeService ColorCodeService, IDesignService DesignService, IUnitService UnitService)
        {
            this._jobworkitemService = jobworkitemService;
            this._NonInventoryService = NonInventoryService;
            this._utilityService = utilityService;
            this._ItemCategoryService = ItemCategoryService;
            this._ItemSubCategoryService = ItemSubCategoryService;
            this._IUserCredentialService = UserCredentialService;
            this._iIModuleService = ModuleService;
            this._TypeOfMaterialService = TypeOfMaterialService;
            this._ColorCodeService = ColorCodeService;
            this._DesignService = DesignService;
            this._UnitService = UnitService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //CATEGORY NAME AUTO COMPLETE
        [HttpGet]
        public JsonResult GetCategory(string term)
        {
            var result = _ItemCategoryService.GetItemCategoryList(term);
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.CategoryName);
            }
            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        //GET SUB CATEGORY BY CATEGORY
        [HttpGet]
        public JsonResult LoadSubCategoryByCategory(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var list = _ItemSubCategoryService.GetSubCategoryByCategory(id);
                MainApplication model = new MainApplication();
                model.ItemSubCategoryList = list;
                var data = model.ItemSubCategoryList.Select(s => new SelectListItem()
                {
                    Text = s.subCategoryName,
                    Value = s.subCategoryName
                });
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = string.Empty;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        //GET ALL ITEM IN DATABASE
        [HttpGet]
        public JsonResult GetAllItemDetails(string Material, string Color, string Brand, string Size, string Design, string Unit, string Cat, string SubCat)
        {
            int counter = 0;
            var items = _jobworkitemService.GetAllItems();
            foreach (var row1 in items)
            {
                if (row1.typeOfMaterial == Material && row1.colorCode == Color && row1.brandName == Brand &&
                    row1.size == Size && row1.designCode == Design && row1.unit == Unit && row1.itemCategory == Cat && row1.itemSubCategory == SubCat)
                {
                    counter = 1;
                    break;
                }
            }
            if (counter == 0)
            {
                return Json("Absent", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Present", JsonRequestBehavior.AllowGet);
            }
        }

        //GET DESIGN DETAILS
        [HttpGet]
        public JsonResult GetDesignDetails(string code)
        {
            string data = _DesignService.getNameByCode(code);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD TYPE TYPE OF MATERIAL
        [HttpGet]
        public JsonResult LoadTypeOfMaterial()
        {
            MainApplication model = new MainApplication();
            model.typeOfMaterialList = _TypeOfMaterialService.getAllTypeOfMaterial();
            var data = model.typeOfMaterialList.Select(s => new SelectListItem()
            {
                Text = s.MaterialName,
                Value = s.MaterialName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD COLOR CODE
        [HttpGet]
        public JsonResult LoadColorCode()
        {
            MainApplication model = new MainApplication();
            model.ColorCodeList = _ColorCodeService.getAllColorCode();
            var data = model.ColorCodeList.Select(s => new SelectListItem()
            {
                Text = s.colorName,
                Value = s.colorName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD DESIGN CODE
        [HttpGet]
        public JsonResult LoadDesignCode(string type)
        {
            IEnumerable<DesignMaster> dm = _DesignService.GetDesignNamesBySubCat(type);
            var data = dm.Select(m => new SelectListItem()
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //LOAD UNIT LIST
        [HttpGet]
        public JsonResult LoadUnitList()
        {
            MainApplication model = new MainApplication();
            model.UnitList = _UnitService.getallsize();
            var data = model.UnitList.Select(s => new SelectListItem()
            {
                Text = s.UnitName,
                Value = s.UnitName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnitType(string unit)
        {
            string numbertype = _UnitService.GetNumberTypeByUnit(unit);
            return Json(numbertype, JsonRequestBehavior.AllowGet);
        }


        //SAVE ITEM MASTER
        [HttpPost]
        public ActionResult Create(MainApplication mainApplication, FormCollection frmItem)
        {
            MainApplication model = new MainApplication()
            {
                JobWorkItemDetails = new JobWorkItem(),
            };

            int lastitembefore = 0;
            var data = _jobworkitemService.GetLastItem();
            if (data == null)
            {
                lastitembefore = 1;
            }
            else
            {
                lastitembefore = data.itemId + 1;
            }

            string Name = frmItem["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(Name))
            {
                int count = Convert.ToInt32(Name);
                string itemname1 = string.Empty;
                for (int i = 1; i <= count; i++)
                {
                    string itemdescription = "itemDescription" + i;
                    string materialType = "materialType" + i;
                    string colorCode = "colorCodeData" + i;
                    string designCode = "designCodeData" + i;
                    string designname = "designName" + i;
                    string size = "Size" + i;
                    string unit = "Unit" + i;
                    string brandname = "BrandName" + i;
                    string numbertype = "NumberType" + i;

                    string finalitemdescription = frmItem[itemdescription];
                    string finalMaterialType = frmItem[materialType];
                    string finalColorCode = frmItem[colorCode];
                    string finalDesignCode = frmItem[designCode];
                    string finalDesignName = frmItem[designname];
                    string finalSize = frmItem[size];
                    string finalUnit = frmItem[unit];
                    string finalnumbertype = frmItem[numbertype];
                    string finalbrandname = frmItem[brandname];

                        //CREATE ITEM CODE
                        var itemdata = _jobworkitemService.GetLastItem();
                        int itemval = 0;
                        int lenght = 0;
                        if (itemdata != null)
                        {
                            itemval = itemdata.itemId;
                            itemval = itemval + 1;
                            lenght = itemval.ToString().Length;
                        }
                        else
                        {
                            itemval = 1;
                            lenght = 1;
                        }
                        string itemcode = _utilityService.getName("JWI", lenght, itemval);

                        if (!string.IsNullOrEmpty(finalUnit))
                        {
                            model.JobWorkItemDetails.description = finalitemdescription;
                            model.JobWorkItemDetails.modifedOn = DateTime.Now;
                            model.JobWorkItemDetails.status = "Active";
                            model.JobWorkItemDetails.itemCategory = mainApplication.ItemDetails.itemCategory;
                            model.JobWorkItemDetails.itemSubCategory = mainApplication.ItemDetails.itemSubCategory;
                            model.JobWorkItemDetails.itemName = mainApplication.ItemDetails.itemName;
                            model.JobWorkItemDetails.typeOfMaterial = finalMaterialType;
                            model.JobWorkItemDetails.colorCode = finalColorCode;
                            model.JobWorkItemDetails.designCode = finalDesignCode;
                            model.JobWorkItemDetails.designName = finalDesignName;
                            model.JobWorkItemDetails.size = finalSize;
                            model.JobWorkItemDetails.unit = finalUnit;
                            model.JobWorkItemDetails.NumberType = finalnumbertype;
                            model.JobWorkItemDetails.brandName = finalbrandname;
                            model.JobWorkItemDetails.itemCode = itemcode;
                            _jobworkitemService.Create(model.JobWorkItemDetails);
                        }
                }
            }
            int lastitemafter = _jobworkitemService.GetLastItem().itemId;
            return RedirectToAction("CreateDetails", "JobWorkItem", new { LastItemBefore = lastitembefore, LastItemAfter = lastitemafter});
        }

        //SHOW ITEM CREATE DETAILS
        [HttpGet]
        public ActionResult CreateDetails(int LastItemBefore, int LastItemAfter)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.JobWorkItemDetails = _jobworkitemService.GetItem(LastItemBefore);
            model.JobWorkItemList = _jobworkitemService.GetInsertedRow(LastItemBefore, LastItemAfter);
            return View(model);
        }

    }
}
