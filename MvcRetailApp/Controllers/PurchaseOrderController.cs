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
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class PurchaseOrderController : Controller
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
        private readonly IUnitService _unitmasterservice;
        private readonly IPurchaseOrderDetailService _purchaseOrderDetailService;
        private readonly ISuppliersMasterService _supplierMasterService;
        private readonly IPurchaseItemDetailService _purchaseItemDetailService;
        private readonly IGodownService _godownMasterService;
        private readonly IGodownAddressService _godownAddressService;
        private readonly ISubTaxMasterService _subTaxService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly ITypeOfMaterialService _TypeOfMaterialService;
        private readonly IDesignService _DesignService;
        private readonly IColorCodeService _ColorCodeService;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IBarcodeNumberService _BarcodeNumberService;
        private readonly IItemService _ItemService;
        private readonly ICountryService _CountryService;
        private readonly IStateService _stateservice;
        private readonly IDistrictService _districtservice;
        private readonly ISupplierBankDetailService _SupplierBankDetailService;
        private readonly IShopService _ShopService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IPurchaseInventoryTaxService _PurchaseInventoryTaxService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly ICatalogueService _CatalogueService;


        public PurchaseOrderController(IUtilityService utilityservice, IItemService ItemService, IUnitService unitmasterservice,
            IPurchaseOrderDetailService purchaseOrderDetailService, ISuppliersMasterService supplierMasterService, IPurchaseItemDetailService purchaseItemDetailService,
            IGodownService godownMasterService, IGodownAddressService godownAddressService, ISubTaxMasterService subTaxService, IUserCredentialService usercredentialservice,
            IModuleService iIModuleService, ITypeOfMaterialService TypeOfMaterialService, IDesignService DesignService, IColorCodeService ColorCodeService,
            IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice,
            IInwardItemFromSupplierService InwardItemFromSupplierService, INonInventoryItemService NonInventoryItemService, IBarcodeNumberService BarcodeNumberService, ICountryService CountryService,
            IStateService stateservice, IDistrictService districtservice, ISupplierBankDetailService supplierbankdetailservice, IShopService ShopService, IEmployeeMasterService EmployeeMasterService, 
            IPurchaseInventoryTaxService PurchaseInventoryTaxService, IItemTaxService ItemTaxService,ICatalogueService CatalogueService)
        {
            this._SupplierBankDetailService = supplierbankdetailservice;
            this._districtservice = districtservice;
            this._CountryService = CountryService;
            this._ItemService = ItemService;
            this._BarcodeNumberService = BarcodeNumberService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._utilityService = utilityservice;
            this._unitmasterservice = unitmasterservice;
            this._purchaseOrderDetailService = purchaseOrderDetailService;
            this._supplierMasterService = supplierMasterService;
            this._purchaseItemDetailService = purchaseItemDetailService;
            this._godownMasterService = godownMasterService;
            this._godownAddressService = godownAddressService;
            this._subTaxService = subTaxService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._TypeOfMaterialService = TypeOfMaterialService;
            this._DesignService = DesignService;
            this._ColorCodeService = ColorCodeService;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._NonInventoryItemService = NonInventoryItemService;
            this._stateservice = stateservice;
            this._ShopService = ShopService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._PurchaseInventoryTaxService = PurchaseInventoryTaxService;
            this._ItemTaxService = ItemTaxService;
            this._CatalogueService = CatalogueService;
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
        public JsonResult EncodePoId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PurchaseData()
        {
            MainApplication model = new MainApplication();

            var username = HttpContext.Session["UserName"].ToString();
            if (username != "SuperAdmin")
            {
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                        Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        break;
                    }
                }
            }
            string EmpDesig = _EmployeeMasterService.getEmpByEmail(UserEmail).Designation;
            if (EmpDesig == "PurchaseManager")
            {
                Session["LOGINSHOPGODOWNCODE"] = "PurchaseManager";
                Session["SHOPGODOWNNAME"] = "PurchaseManager";
            }
            else
            {
                string godownshopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                string godownshopname = Session["SHOPGODOWNNAME"].ToString();
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                var LastPo = _purchaseOrderDetailService.GetLastPOByFinYr(FinYr, godownshopcode);
                string POCode = string.Empty;
                int PONoLength = 0;
                int PurchaseNo = 0;
                if (LastPo == null)
                {
                    PONoLength = 1;
                    PurchaseNo = 1;
                }
                else
                {
                    int indexpurchasecode = LastPo.PoNo.LastIndexOf('P');
                    POCode = LastPo.PoNo.Substring(indexpurchasecode + 2, 6);
                    PONoLength = (Convert.ToInt32(POCode) + 1).ToString().Length;
                    PurchaseNo = Convert.ToInt32(POCode) + 1;
                }
                string ShortCode = string.Empty;
                if (godownshopcode.Contains("SH"))
                {
                    ShortCode = _ShopService.GetShopDetailsByName(godownshopname).ShortCode;
                }
                else
                {
                    ShortCode = _godownMasterService.GetGodownDetailsByName(godownshopname).ShortCode;
                }
                POCode = _utilityService.getName(ShortCode + "/PO", PONoLength, PurchaseNo);
                POCode = POCode + FinYr;
                TempData["PreviousPoNo"] = POCode;
                model.PoCode = POCode;
            }
            model.UnitList = _unitmasterservice.getallsize();
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public ActionResult NewItem()
        {
            MainApplication model = new MainApplication();
            return View(model);
        }

        //GET SUB CATEGORY TAX
        [HttpGet]
        public JsonResult GetSubCatTax(string SubCategory)
        {
            double tax = 0;
            var taxdetails = _ItemTaxService.GetTaxBySubCatAndVAT(SubCategory);
            if (taxdetails != null)
            {
                tax = taxdetails.Percentage;
            }
            return Json(tax, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewItem(MainApplication model, FormCollection frmItem)
        {
            model.ItemDetails = new Item();
            model.NonInventoryItemDetails = new NonInventoryItem();

            int lastitembefore = 0;
            var data = _ItemService.GetLastItem();
            if (data == null)
            {
                lastitembefore = 1;
            }
            else
            {
                lastitembefore = data.itemId + 1;
            }

            int lastnoninventoryitembefore = 0;
            var noninventorydata = _NonInventoryItemService.GetLastItem();
            if (noninventorydata == null)
            {
                lastnoninventoryitembefore = 1;
            }
            else
            {
                lastnoninventoryitembefore = noninventorydata.itemId + 1;
            }

            string Name = frmItem["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(Name))
            {
                int count = Convert.ToInt32(Name);
                string itemname1 = string.Empty;
                for (int i = 1; i <= count; i++)
                {
                    string itemdescription = "itemDescription" + i;
                    string costprice = "CostPrice" + i;
                    string materialType = "materialType" + i;
                    string colorCode = "colorCodeData" + i;
                    string designCode = "designCodeData" + i;
                    string designname = "designName" + i;
                    string size = "Size" + i;
                    string unit = "Unit" + i;
                    string type = "ItemType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string brandname = "Brand" + i;
                    string numbertype = "NumberType" + i;
                    string mrp = "MRP" + i;

                    string finalitemdescription = frmItem[itemdescription];
                    string finalmrp = frmItem[mrp];
                    string finalcostprice = frmItem[costprice];
                    string finalMaterialType = frmItem[materialType];
                    string finalColorCode = frmItem[colorCode];
                    string finalDesignCode = frmItem[designCode];
                    string finalDesignName = frmItem[designname];
                    string finalSize = frmItem[size];
                    string finalUnit = frmItem[unit];
                    string finalnumbertype = frmItem[numbertype];
                    string finalType = frmItem[type];
                    string finalsellingprice = frmItem[sellingprice];
                    string finalbrandname = frmItem[brandname];

                    //IF ITEM TYPE IS INVENTORY THEN ITEM STORED IN ITEM MASTER
                    if (finalType == "Inventory")
                    {

                        //CREATE ITEM CODE
                        var itemdata = _ItemService.GetLastItem();
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
                        string itemcode = _utilityService.getName("I", lenght, itemval);

                        if (!string.IsNullOrEmpty(finalcostprice))
                        {
                            model.ItemDetails.description = finalitemdescription;
                            model.ItemDetails.modifedOn = DateTime.Now;
                            model.ItemDetails.status = "Active";
                            model.ItemDetails.itemCategory = frmItem["ItemDetails.itemCategory"];
                            model.ItemDetails.itemSubCategory = frmItem["ItemDetails.itemSubCategory"];
                            model.ItemDetails.itemName = frmItem["itemDetails.itemName"];
                            model.ItemDetails.costprice = finalcostprice;
                            model.ItemDetails.typeOfMaterial = finalMaterialType;
                            model.ItemDetails.colorCode = finalColorCode;
                            model.ItemDetails.designCode = finalDesignCode;
                            model.ItemDetails.designName = finalDesignName;
                            model.ItemDetails.size = finalSize;
                            model.ItemDetails.unit = finalUnit;
                            model.ItemDetails.NumberType = finalnumbertype;
                            model.ItemDetails.itemtype = finalType;
                            model.ItemDetails.sellingprice = finalsellingprice;
                            model.ItemDetails.brandName = finalbrandname;
                            model.ItemDetails.mrp = finalmrp;
                            //if (finalCommissionInPercent != "0")
                            //{
                            //    model.ItemDetails.CommissionInPercent = Convert.ToDouble(finalCommissionInPercent);
                            //    model.ItemDetails.CommissionInRupees = 0;
                            //}
                            //else
                            //{
                            //    model.ItemDetails.CommissionInRupees = Convert.ToDouble(finalCommissionInRupees);
                            //    model.ItemDetails.CommissionInPercent = 0;
                            //}
                            model.ItemDetails.itemCode = itemcode;
                            _ItemService.createItem(model.ItemDetails);
                        }
                    }
                    //IF ITEM TYPE IS NON_INVENTORY THEN ITEM STORED IN NONINVENTORYITEMS
                    else
                    {
                        //CREATE NON-INVENTORY ITEM CODE
                        var noninvitemdata = _NonInventoryItemService.GetLastItem();
                        int noninvitemval = 0;
                        int noninvlenght = 0;
                        if (noninvitemdata != null)
                        {
                            noninvitemval = noninvitemdata.itemId;
                            noninvitemval = noninvitemval + 1;
                            noninvlenght = noninvitemval.ToString().Length;
                        }
                        else
                        {
                            noninvitemval = 1;
                            noninvlenght = 1;
                        }
                        string noninvitemcode = _utilityService.getName("NI", noninvlenght, noninvitemval);

                        if (!string.IsNullOrEmpty(finalType))
                        {
                            model.NonInventoryItemDetails.description = finalitemdescription;
                            model.NonInventoryItemDetails.modifedOn = DateTime.Now;
                            model.NonInventoryItemDetails.status = "Active";
                            model.NonInventoryItemDetails.itemCategory = frmItem["ItemDetails.itemCategory"];
                            model.NonInventoryItemDetails.itemSubCategory = frmItem["ItemDetails.itemSubCategory"];
                            model.NonInventoryItemDetails.itemName = frmItem["itemDetails.itemName"];
                            model.NonInventoryItemDetails.costprice = finalcostprice;
                            model.NonInventoryItemDetails.typeOfMaterial = finalMaterialType;
                            model.NonInventoryItemDetails.colorCode = finalColorCode;
                            model.NonInventoryItemDetails.designCode = finalDesignCode;
                            model.NonInventoryItemDetails.designName = finalDesignName;
                            model.NonInventoryItemDetails.size = finalSize;
                            model.NonInventoryItemDetails.unit = finalUnit;
                            model.NonInventoryItemDetails.NumberType = finalnumbertype;
                            model.NonInventoryItemDetails.itemtype = finalType;
                            model.NonInventoryItemDetails.sellingprice = finalsellingprice;
                            model.NonInventoryItemDetails.brandName = finalbrandname;
                            model.NonInventoryItemDetails.mrp = finalmrp;
                            model.NonInventoryItemDetails.itemCode = noninvitemcode;
                            //if (finalCommissionInPercent != "0")
                            //{
                            //    model.NonInventoryItemDetails.CommissionInPercent = Convert.ToDouble(finalCommissionInPercent);
                            //    model.NonInventoryItemDetails.CommissionInRupees = 0;
                            //}
                            //else
                            //{
                            //    model.NonInventoryItemDetails.CommissionInPercent = 0;
                            //    model.NonInventoryItemDetails.CommissionInRupees = Convert.ToDouble(finalCommissionInRupees);
                            //}
                            _NonInventoryItemService.createNonInventoryItem(model.NonInventoryItemDetails);
                        }
                    }
                }
            }
            int lastitemafter = _ItemService.GetLastItem().itemId;
            var lastnoninventoryitemafter = _NonInventoryItemService.GetLastItem();
            int lnonitemafter;

            if (lastnoninventoryitemafter != null)
            {
                lnonitemafter = lastnoninventoryitemafter.itemId;
            }
            else
            {
                lnonitemafter = 1;
            }

            return RedirectToAction("NewItemDetails", "PurchaseOrder", new { LastItemBefore = lastitembefore, LastItemAfter = lastitemafter, LastNonInventoryItemBefore = lastnoninventoryitembefore, LastNonInventoryItemAfter = lnonitemafter });
        }

        //GET ITEM BY SUBCATEGORY
        [HttpGet]
        public JsonResult GetInventoryItems()
        {
            var itemlist = _ItemService.getAllItems();
            MainApplication model = new MainApplication();
            model.ItemList = itemlist;
            var modeldata = model.ItemList.Select(m => new SelectListItem()
            {
                Text = m.itemName + " " + m.itemCode,
                Value = m.itemCode
            });
            return Json(modeldata, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemsFromCatalogue()
        {
            var list = _CatalogueService.GetAll();
            var select = list.Select(sel => new SelectListItem()
            {
                Text = sel.ItemName + " " + sel.ItemCode,
                Value = sel.ItemCode
            });
            return Json(select, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NewItemDetails(int LastItemBefore, int LastItemAfter, int LastNonInventoryItemBefore, int LastNonInventoryItemAfter)
        {
            MainApplication model = new MainApplication();
            var item = _ItemService.GetLastItem();
            int lastinsertedrow = item.itemId;
            TempData["ItemSubCat"] = item.itemSubCategory;
            model.ItemDetails = _ItemService.GetItem(LastItemBefore);
            if (model.ItemDetails == null)
            {
                model.NonInventoryItemDetails = _NonInventoryItemService.GetItemById(LastNonInventoryItemBefore);
            }
            model.ItemList = _ItemService.GetInsertedRow(LastItemBefore, LastItemAfter);
            model.NonInventoryItemList = _NonInventoryItemService.GetInsertedRow(LastNonInventoryItemBefore, LastNonInventoryItemAfter);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetItemDetails(string itemname, string Barcode, string FormType, string State)
        {
            var itemdetails = _ItemService.GetDescriptionByItemCode(itemname);
            if (itemname != "" && itemname != null)
            {
                itemdetails = _ItemService.GetDescriptionByItemCode(itemname);
            }
            else
            {
                itemdetails = _ItemService.GetItemByUniversalBarcode(Barcode); 
            }
            string subcat = itemdetails.itemSubCategory;
            string typematerial = itemdetails.typeOfMaterial;
            var list = _DesignService.GetDesignNamesBySubCat(itemdetails.itemSubCategory);
            var select = list.Select(m => new SelectListItem
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            string colorname = itemdetails.colorCode;
            double taxPercent = 0;

            if (State == "Maharashtra")
            {
                var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
            }
            else
            {
                var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                if (Tax != null)
                {
                    if (FormType == "C-Form")
                    {
                        taxPercent = 2;
                    }
                    else
                    {
                        taxPercent = Tax.Percentage;
                    }
                }
                else { taxPercent = 0; }
            }
            return Json(new
            {
                taxPercent,
                subcat,
                itemdetails.itemName,
                itemdetails.itemCode,
                itemdetails.sellingprice,
                itemdetails.mrp,
                itemdetails.costprice,
                itemdetails.unit,
                itemdetails.NumberType,
                itemdetails.description,
                typematerial,
                itemdetails.designCode,
                itemdetails.designName,
                itemdetails.brandName,
                itemdetails.size,
                itemdetails.UniversalBarcode,
                colorname,
                select
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCatalogueItemDetails(string Barcode,string ItemCode, string FormType, string State)
        {
            //var itemdeetails = _ItemService.GetDescriptionByItemCode(itemname);
            var itemdetails = _CatalogueService.GetLastCatalogue() ;  
            if (Barcode != null && Barcode != "")
            {
                itemdetails = _CatalogueService.GetRowByBarcode(Barcode);
            }
            else
            {
                itemdetails = _CatalogueService.GetRowByItemCode(ItemCode);
            }
            string subcat = itemdetails.Subcategory;
            string typematerial = itemdetails.TypeOfMaterial;
            var list = _DesignService.GetDesignNamesBySubCat(itemdetails.Subcategory);
            var select = list.Select(m => new SelectListItem
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            string colorname = itemdetails.Color;
            double taxPercent = 0;

            if (State == "Maharashtra")
            {
                var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
            }
            else
            {
                var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                if (Tax != null)
                {
                    if (FormType == "C-Form")
                    {
                        taxPercent = 2;
                    }
                    else
                    {
                        taxPercent = Tax.Percentage;
                    }
                }
                else { taxPercent = 0; }
            }
            return Json(new
            {
                taxPercent,
                subcat,
                itemdetails.ItemCode,
                itemdetails.ItemName,
                itemdetails.SellingPrice,
                itemdetails.MRP,
                itemdetails.CostPrice,
                itemdetails.Unit,
                itemdetails.NumberType,
                itemdetails.Description,
                typematerial,
                itemdetails.DesignCode,
                itemdetails.Design,
                itemdetails.Brand,
                itemdetails.Size,
                colorname,
                select
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNonInventoryItems()
        {
            var noninvitems = _NonInventoryItemService.GetAll();
            var items = noninvitems.Select(m => new SelectListItem
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNonIvnItemDetails(string itemcode, string FormType, string State)
        {
            var data = _NonInventoryItemService.GetDetailsByItemCode(itemcode);
            string subcat = data.itemSubCategory;
            var list = _DesignService.GetDesignNamesBySubCat(subcat);
            var select = list.Select(m => new SelectListItem
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            double taxPercent = 0;
            if (State == "Maharashtra")
            {
                var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
            }
            else
            {
                var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                if (Tax != null)
                {
                    if (FormType == "C-Form")
                    {
                        taxPercent = 2;
                    }
                    else
                    {
                        taxPercent = Tax.Percentage;
                    }
                }
                else { taxPercent = 0; }
            }
            return Json(new
            {
                taxPercent,
                data.itemName,
                subcat,
                data.typeOfMaterial,
                data.unit,
                data.NumberType,
                data.sellingprice,
                data.mrp,
                data.costprice,
                data.description,
                data.colorCode,
                data.designCode,
                data.designName,
                data.brandName,
                data.size,
                select
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDesignNameByCode(string code)
        {
            string designName = _ItemService.GetDesignNameByDesignCode(code).designName;
            return Json(designName, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PurchaseData(MainApplication model, FormCollection frmcoll)
        {
            MainApplication mainapp = new MainApplication()
            {
                PurchaseItemData = new PurchaseItemDetail(),
            };
            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            string EmpDesig = _EmployeeMasterService.getEmpByEmail(UserEmail).Designation;
            if (EmpDesig == "PurchaseManager")
            {
                if (model.PurchaseOrderData.GodownCode != null)
                {
                    godownshopcode = model.PurchaseOrderData.GodownCode;
                    godownshopname = model.PurchaseOrderData.GodownName;
                }
                else
                {
                    godownshopcode = model.PurchaseOrderData.ShopCode;
                    godownshopname = model.PurchaseOrderData.ShopName;
                }
            }
            else
            {
                godownshopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
                godownshopname = Session["SHOPGODOWNNAME"].ToString();
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastPo = _purchaseOrderDetailService.GetLastPOByFinYr(FinYr, godownshopcode);
            string POCode = string.Empty;
            int PONoLength = 0;
            int PurchaseNo = 0;
            if (LastPo == null)
            {
                PONoLength = 1;
                PurchaseNo = 1;
            }
            else
            {
                int indexpurchasecode = LastPo.PoNo.LastIndexOf('P');
                POCode = LastPo.PoNo.Substring(indexpurchasecode + 2, 6);
                PONoLength = (Convert.ToInt32(POCode) + 1).ToString().Length;
                PurchaseNo = Convert.ToInt32(POCode) + 1;
            }
            string ShortCode = string.Empty;
            if (godownshopcode.Contains("SH"))
            {
                ShortCode = _ShopService.GetShopDetailsByName(godownshopname).ShortCode;
            }
            else
            {
                ShortCode = _godownMasterService.GetGodownDetailsByName(godownshopname).ShortCode;
            }
            POCode = _utilityService.getName(ShortCode + "/PO", PONoLength, PurchaseNo);
            POCode = POCode + FinYr;
            string dynamicrow = frmcoll["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(dynamicrow))
            {
                int count = Convert.ToInt32(dynamicrow);
                for (int i = 1; i <= count; i++)
                {
                    string itemCode = "itemCode" + i;
                    string subcat = "SubCat" + i;
                    string itemName = "itemName" + i;
                    string itemtype = "ItemType" + i;
                    string description = "description" + i;
                    string quantity = "quantity" + i;
                    string sellingprice = "sellingprice" + i;
                    string mrp = "mrp" + i;
                    string costprice = "costprice" + i;
                    string materialvalue = "materialvalue" + i;
                    string colorvalue = "colorvalue" + i;
                    string brandvalue = "brandvalue" + i;
                    string sizevalue = "sizevalue" + i;
                    string design = "design" + i;
                    string designName = "designName" + i;
                    string discountpercent = "discountpercent" + i;
                    string discountrps = "discountrps" + i;
                    string unit = "unit" + i;
                    string unitvalue = "unitvalue" + i;
                    string numbertype = "numberType" + i;
                    string amountvalue = "amountvalue" + i;
                    string itemtax = "ItemTaxValue" + i;
                    string proportionatetax = "proportionatetaxvalue" + i;
                    string proportionatetaxamount = "proportionatetaxamount" + i;
                    string ubarcode = "ubarcode" + i;

                    if (frmcoll[quantity] != "" && frmcoll[itemCode] != null)
                    {
                        mainapp.PurchaseItemData.itemCode = frmcoll[itemCode];
                        mainapp.PurchaseItemData.Item = frmcoll[itemName];
                        mainapp.PurchaseItemData.Description = frmcoll[description];
                        mainapp.PurchaseItemData.Quantity = Convert.ToDouble(frmcoll[quantity]);
                        mainapp.PurchaseItemData.Unit = frmcoll[unitvalue];
                        mainapp.PurchaseItemData.NumberType = frmcoll[numbertype];
                        mainapp.PurchaseItemData.CostPrice = Convert.ToDouble(frmcoll[costprice]);
                        if (frmcoll[mrp] != "")
                        {
                            mainapp.PurchaseItemData.MRP = Convert.ToDouble(frmcoll[mrp]);
                        }
                        else
                        {
                            mainapp.PurchaseItemData.MRP = 0;
                        }
                        if (frmcoll[sellingprice] != "")
                        {
                            mainapp.PurchaseItemData.SellingPrice = Convert.ToDouble(frmcoll[sellingprice]);
                        }
                        else
                        {
                            mainapp.PurchaseItemData.SellingPrice = 0;
                        }
                        mainapp.PurchaseItemData.Amount = Convert.ToDouble(frmcoll[amountvalue]);
                        if (frmcoll[discountpercent] == "")
                        {
                            mainapp.PurchaseItemData.DiscountPercent = 0;
                        }
                        else
                        {
                            mainapp.PurchaseItemData.DiscountPercent = Convert.ToDouble(frmcoll[discountpercent]);
                        }
                        if (frmcoll[discountrps] == "")
                        {
                            mainapp.PurchaseItemData.DiscountRPS = 0;
                        }
                        else
                        {
                            mainapp.PurchaseItemData.DiscountRPS = Convert.ToDouble(frmcoll[discountrps]);
                        }
                        mainapp.PurchaseItemData.Material = frmcoll[materialvalue];
                        mainapp.PurchaseItemData.Color = frmcoll[colorvalue];
                        mainapp.PurchaseItemData.Brand = frmcoll[brandvalue];
                        mainapp.PurchaseItemData.Size = frmcoll[sizevalue];
                        mainapp.PurchaseItemData.Design = frmcoll[design];
                        mainapp.PurchaseItemData.DesignName = frmcoll[designName];
                        mainapp.PurchaseItemData.Status = "Active";
                        mainapp.PurchaseItemData.ModifiedOn = DateTime.Now;
                        mainapp.PurchaseItemData.PoNo = POCode;
                        mainapp.PurchaseItemData.Category = _itemsubcategoryservice.GetCategoryBySubCat(frmcoll[subcat]);
                        mainapp.PurchaseItemData.SubCategory = frmcoll[subcat];
                        mainapp.PurchaseItemData.ItemTax = frmcoll[itemtax];
                        mainapp.PurchaseItemData.ItemType = frmcoll[itemtype];
                        mainapp.PurchaseItemData.ProportionateTax = Convert.ToDouble(frmcoll[proportionatetax]);
                        mainapp.PurchaseItemData.ProportionateTaxAmt = Convert.ToDouble(frmcoll[proportionatetaxamount]);
                        mainapp.PurchaseItemData.UniversalBarcode = frmcoll[ubarcode];
                        _purchaseItemDetailService.CreatePurchaseItemDetail(mainapp.PurchaseItemData);
                    }
                }
            }
            model.PurchaseOrderData.PoNo = POCode;
            model.PurchaseOrderData.PoDate = DateTime.Now;
            if (model.PurchaseOrderData.GodownCode == null)
            {
                model.PurchaseOrderData.GodownArea = null;
            }
            model.PurchaseOrderData.TotalAmount = Convert.ToDouble(frmcoll["Total_Amount_Value"]);
            model.PurchaseOrderData.GrandTotal = Convert.ToDouble(frmcoll["Grand_Total_Value"]);
            model.PurchaseOrderData.status = "Active";
            model.PurchaseOrderData.modifiedOn = DateTime.Now;
            model.PurchaseOrderData.ReceivingStatus = "Pending";
            _purchaseOrderDetailService.CreatePurchaseOrder(model.PurchaseOrderData);

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcoll["ItemTaxCount"]);
            model.PurchaseInventoryTaxDetails = new PurchaseInventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcoll[taxamount]) != 0)
                {
                    model.PurchaseInventoryTaxDetails.Code = POCode;
                    model.PurchaseInventoryTaxDetails.Amount = frmcoll[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = frmcoll[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = frmcoll[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }
            var podetails = _purchaseOrderDetailService.GetDetailByPoNo(POCode);
            string PoId = Encode(podetails.PoId.ToString());
            return RedirectToAction("PurchaseOrder/" + PoId, "PurchaseOrder");
        }

        [HttpGet]
        public ActionResult PurchaseOrder(string id)
        {
            MainApplication model = new MainApplication()
            {
                PurchaseOrderData = new PurchaseOrderDetail(),
                PurchaseItemData = new PurchaseItemDetail()
            };
            int Id = Decode(id);
            model.PurchaseOrderData = _purchaseOrderDetailService.GetPurchaseOrderbyId(Id);
            model.PurchaseItemList = _purchaseItemDetailService.GetPurchaseItemsByPONo(model.PurchaseOrderData.PoNo);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.PurchaseOrderData.PoNo);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string previouspono = TempData["PreviousPoNo"].ToString();
            if (previouspono != model.PurchaseOrderData.PoNo)
            {
                ViewData["PoNoChanged"] = previouspono + " is replaced with " + model.PurchaseOrderData.PoNo + " because " + previouspono + " is acquired by another person";
            }
            TempData["PreviousPoNo"] = previouspono;
            TempData["ConvertToPdf"] = "Yes";
            return View(model);
        }

        [HttpGet]
        public JsonResult LoadItemAndDesignBySubCategory(string subcat, string itemtype)
        {
            var list = _DesignService.GetDesignNamesBySubCat(subcat);
            var designs = list.Select(m => new SelectListItem
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            if (itemtype == "Inventory")
            {
                var itemlist = _ItemService.getAllItems();
                var items = itemlist.Select(m => new SelectListItem()
                {
                    Text = m.itemName,
                    Value = m.itemCode
                });
                return Json(new
                {
                    items,
                    designs
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var nonitemlist = _NonInventoryItemService.GetAll();
                var items = nonitemlist.Select(m => new SelectListItem()
                {
                    Text = m.itemName,
                    Value = m.itemCode
                });
                return Json(new
                {
                    items,
                    designs
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSupplierDetails(string name)
        {
            var supplierlist = _supplierMasterService.GetByName(name);
            var GodownNamesList = _godownMasterService.GetGodownNames();
            var godownNames = GodownNamesList.Select(m => new SelectListItem
                {
                    Text = m.GodownName,
                    Value = m.GdCode
                });
            var ShopNamesList = _ShopService.GetAll();
            var shopnames = ShopNamesList.Select(m => new SelectListItem
            {
                Text = m.ShopName,
                Value = m.ShopCode
            });
            return Json(new
            {
                supplierlist.Address,
                supplierlist.ContactNo1,
                supplierlist.Email,
                supplierlist.State,
                supplierlist.Registered,
                godownNames,
                shopnames
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PopUpPage(string code)
        {
            var data = _ItemService.GetDesignNameByDesignCode(code);
            return View(data);
        }

        [HttpGet]
        public JsonResult GetUnits()
        {
            var unitlist = _unitmasterservice.getallsize();
            var utlist = unitlist.Select(m => new SelectListItem
            {
                Text = m.UnitName,
                Value = m.UnitName,
            });
            return Json(utlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownAddresses(string code)
        {
            string godownname = _godownMasterService.GetGodownByCode(code).GodownName;
            var godownAddress = _godownAddressService.GetAddressList(code);
            var addresslist = godownAddress.Select(m => new SelectListItem
            {
                Text = m.AreaName,
                Value = m.AreaName
            });
            string EmpDesig = _EmployeeMasterService.getEmpByEmail(UserEmail).Designation;
            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            string POCode = string.Empty;
            if (EmpDesig == "PurchaseManager")
            {
                godownshopcode = code;
                godownshopname = godownname;
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                var LastPo = _purchaseOrderDetailService.GetLastPOByFinYr(FinYr, godownshopcode);

                int PONoLength = 0;
                int PurchaseNo = 0;
                if (LastPo == null)
                {
                    PONoLength = 1;
                    PurchaseNo = 1;
                }
                else
                {
                    int indexpurchasecode = LastPo.PoNo.LastIndexOf('P');
                    POCode = LastPo.PoNo.Substring(indexpurchasecode + 2, 6);
                    PONoLength = (Convert.ToInt32(POCode) + 1).ToString().Length;
                    PurchaseNo = Convert.ToInt32(POCode) + 1;
                }
                string ShortCode = string.Empty;
                ShortCode = _godownMasterService.GetGodownDetailsByName(godownshopname).ShortCode;
                POCode = _utilityService.getName(ShortCode + "/PO", PONoLength, PurchaseNo);
                POCode = POCode + FinYr;
                TempData["PreviousPoNo"] = POCode;
            }
            return Json(new { godownname, addresslist, POCode }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetShopDetailsByName(string Code)
        {
            var details = _ShopService.GetShopByCode(Code);
            string EmpDesig = _EmployeeMasterService.getEmpByEmail(UserEmail).Designation;
            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            string POCode = string.Empty;
            if (EmpDesig == "PurchaseManager")
            {
                godownshopcode = Code;
                godownshopname = details.ShopName;
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                var LastPo = _purchaseOrderDetailService.GetLastPOByFinYr(FinYr, godownshopcode);

                int PONoLength = 0;
                int PurchaseNo = 0;
                if (LastPo == null)
                {
                    PONoLength = 1;
                    PurchaseNo = 1;
                }
                else
                {
                    int indexpurchasecode = LastPo.PoNo.LastIndexOf('P');
                    POCode = LastPo.PoNo.Substring(indexpurchasecode + 2, 6);
                    PONoLength = (Convert.ToInt32(POCode) + 1).ToString().Length;
                    PurchaseNo = Convert.ToInt32(POCode) + 1;
                }
                string ShortCode = string.Empty;
                ShortCode = _ShopService.GetShopDetailsByName(godownshopname).ShortCode;
                POCode = _utilityService.getName(ShortCode + "/PO", PONoLength, PurchaseNo);
                POCode = POCode + FinYr;
                TempData["PreviousPoNo"] = POCode;
            }
            return Json(new { POCode, details.ShopContactNo, details.ShopEmail, details.EmpName, details.ShopName, details.ShopAddress }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownDetails(string code, string area)
        {
            var data = _godownAddressService.GetAddressByAreaByCode(code, area);
            var godowndetail = _godownMasterService.GetGodownByCode(code);
            return Json(new
            {
                data.Address,
                godowndetail.ContactNo1,
                godowndetail.EmpName,
                godowndetail.GodownEmail,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintPurchaseOrder(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication()
            {
                PurchaseOrderData = new PurchaseOrderDetail(),
                PurchaseItemData = new PurchaseItemDetail()
            };
            model.PurchaseOrderData = _purchaseOrderDetailService.GetPurchaseOrderbyId(Id);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.PurchaseOrderData.PoNo);
            model.PurchaseItemList = _purchaseItemDetailService.GetPurchaseItemsByPONo(model.PurchaseOrderData.PoNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult ListOfPOes()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            TempData["ConvertToPdf"] = "No";
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPOesList(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.PurchaseOrderList = _purchaseOrderDetailService.GetPOesPendingsList(FDate, TDate);
            return View(model);
        }

        [HttpGet]
        public JsonResult DeletePO(string pono)
        {
            var podetails = _purchaseOrderDetailService.GetDetailByPoNo(pono);
            podetails.status = "InActive";
            podetails.ReceivingStatus = "Completed";
            _purchaseOrderDetailService.UpdatePurchaseOrder(podetails);
            var poitemlist = _purchaseItemDetailService.GetPurchaseItemsByPONo(pono);
            foreach (var data in poitemlist)
            {
                data.Status = "InActive";
                _purchaseItemDetailService.UpdatePurchaseItemDetail(data);
            }
            return Json(pono, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult POEdit()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult POEditList(string term)
        {
            var polist = _purchaseOrderDetailService.GetPOList(term);
            List<string> names = new List<string>();
            foreach (var item in polist)
            {
                names.Add(item.PoNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPOEditList(string Pono)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.PurchaseItemList = _purchaseItemDetailService.GetPurchaseItemsByPONo(Pono);
            TempData["POItemList"] = model.PurchaseItemList;
            model.PurchaseOrderData = _purchaseOrderDetailService.GetDetailByPoNo(Pono);
            model.ItemSubCategoryList = _itemsubcategoryservice.getSubCategory();
            model.UnitList = _unitmasterservice.getallsize();
            model.GodownMasterList = _godownMasterService.GetGodownNames();
            model.ShopList = _ShopService.GetAll();
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.PurchaseOrderData.PoNo);
            if (model.PurchaseInventoryTaxList.Count() != 0)
            {
                TempData["PoTaxList"] = model.PurchaseInventoryTaxList;
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetTaxForAlreadyPresentItems(string itemname, string FormType, string subcat, string State)
        {
            double taxPercent = 0;
            if (State == "Maharashtra")
            {
                var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                if (Tax != null)
                {
                    taxPercent = Tax.Percentage;
                }
                else { taxPercent = 0; }
            }
            else
            {
                var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                if (Tax != null)
                {
                    if (FormType == "C-Form")
                    {
                        taxPercent = 2;
                    }
                    else
                    {
                        taxPercent = Tax.Percentage;
                    }
                }
                else { taxPercent = 0; }
            }
            return Json(taxPercent, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInventoryAndNonInventoryItemDetails(string itemname, string FormType, string State, string itemtype)
        {
            double taxPercent = 0;
            string subcat = string.Empty;
            if (itemtype == "Inventory")
            {
                var itemdetails = _ItemService.GetDescriptionByItemCode(itemname);
                string typematerial = itemdetails.typeOfMaterial;
                string colorname = itemdetails.colorCode;
                subcat = itemdetails.itemSubCategory;
                if (State == "Maharashtra")
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        if (FormType == "C-Form")
                        {
                            taxPercent = 2;
                        }
                        else
                        {
                            taxPercent = Tax.Percentage;
                        }
                    }
                    else { taxPercent = 0; }
                }

                var list = _DesignService.GetDesignNamesBySubCat(subcat);
                var designlist = list.Select(m => new SelectListItem
                {
                    Text = m.DesignName,
                    Value = m.DesignCode,
                });

                return Json(new
                {
                    subcat,
                    taxPercent,
                    itemdetails.itemName,
                    itemdetails.Barcode,
                    itemdetails.unit,
                    itemdetails.NumberType,
                    itemdetails.costprice,
                    itemdetails.sellingprice,
                    itemdetails.mrp,
                    itemdetails.description,
                    typematerial,
                    itemdetails.designCode,
                    itemdetails.designName,
                    itemdetails.brandName,
                    itemdetails.size,
                    itemdetails.UniversalBarcode,
                    colorname,
                    designlist
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _NonInventoryItemService.GetDetailsByItemCode(itemname);
                subcat = data.itemSubCategory;

                if (State == "Maharashtra")
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        if (FormType == "C-Form")
                        {
                            taxPercent = 2;
                        }
                        else
                        {
                            taxPercent = Tax.Percentage;
                        }
                    }
                    else { taxPercent = 0; }
                }

                var list = _DesignService.GetDesignNamesBySubCat(subcat);
                var designlist = list.Select(m => new SelectListItem
                {
                    Text = m.DesignName,
                    Value = m.DesignCode,
                });

                return Json(new
                {
                    taxPercent,
                    data.itemName,
                    data.typeOfMaterial,
                    data.unit,
                    data.NumberType,
                    data.description,
                    data.colorCode,
                    data.costprice,
                    data.sellingprice,
                    data.mrp,
                    data.designCode,
                    data.designName,
                    data.brandName,
                    data.size,
                    designlist
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetPOEditList(MainApplication model, FormCollection frmcoll)
        {
            MainApplication mainapp = new MainApplication()
            {
                PurchaseItemData = new PurchaseItemDetail(),
            };
            var poitemlist = TempData["POItemList"] as IEnumerable<PurchaseItemDetail>;
            foreach (var deleteitem in poitemlist)
            {
                var delete = _purchaseItemDetailService.GetDetailsById(deleteitem.PoItemNo);
                _purchaseItemDetailService.DeletePurchaseItemDetail(delete);
            }
            var potaxlist = TempData["PoTaxList"] as IEnumerable<InventoryTax>;
            if (potaxlist != null)
            {
                foreach (var deletetax in potaxlist)
                {
                    var delete = _PurchaseInventoryTaxService.GetTaxById(deletetax.Id);
                    _PurchaseInventoryTaxService.Delete(delete);
                }
            }
            string dynamicrow = frmcoll["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(dynamicrow))
            {
                int count = Convert.ToInt32(dynamicrow);
                for (int i = 1; i <= count; i++)
                {
                    string itemCode = "itemCode" + i;
                    string category = "Category" + i;
                    string subcat = "SubCat" + i;
                    string itemName = "itemName" + i;
                    string itemtype = "ItemType" + i;
                    string description = "description" + i;
                    string quantity = "quantity" + i;
                    string sellingprice = "sellingprice" + i;
                    string mrp = "MRP" + i;
                    string costprice = "costprice" + i;
                    string materialvalue = "materialvalue" + i;
                    string colorvalue = "colorvalue" + i;
                    string brandvalue = "brandvalue" + i;
                    string sizevalue = "sizevalue" + i;
                    string design = "design" + i;
                    string designName = "designName" + i;
                    string discountpercent = "discountpercent" + i;
                    string discountrps = "discountrps" + i;
                    string unit = "unitvalue" + i;
                    string numbertype = "numberType" + i;
                    string amountvalue = "amountvalue" + i;
                    string itemtax = "ItemTaxValue" + i;
                    string proportionatetax = "proportionatetaxvalue" + i;
                    string proportionatetaxamount = "proportionatetaxamount" + i;
                    string ubarcode = "ubarcode" + i;

                    if (frmcoll[quantity] != "" && frmcoll[itemCode] != null)
                    {
                        mainapp.PurchaseItemData.UniversalBarcode = frmcoll[ubarcode];
                        mainapp.PurchaseItemData.itemCode = frmcoll[itemCode];
                        mainapp.PurchaseItemData.Item = frmcoll[itemName];
                        mainapp.PurchaseItemData.Description = frmcoll[description];
                        mainapp.PurchaseItemData.Quantity = Convert.ToDouble(frmcoll[quantity]);
                        mainapp.PurchaseItemData.Unit = frmcoll[unit];
                        mainapp.PurchaseItemData.NumberType = frmcoll[numbertype];
                        mainapp.PurchaseItemData.CostPrice = Convert.ToDouble(frmcoll[costprice]);
                        if (frmcoll[mrp] != "")
                        {
                            mainapp.PurchaseItemData.MRP = Convert.ToDouble(frmcoll[mrp]);
                        }
                        else
                        {
                            mainapp.PurchaseItemData.MRP = 0;
                        }
                        if (frmcoll[sellingprice] != "")
                        {
                            mainapp.PurchaseItemData.SellingPrice = Convert.ToDouble(frmcoll[sellingprice]);
                        }
                        else
                        {
                            mainapp.PurchaseItemData.SellingPrice = 0;
                        }
                        mainapp.PurchaseItemData.Amount = Convert.ToDouble(frmcoll[amountvalue]);
                        if (frmcoll[discountpercent] == "")
                        {
                            mainapp.PurchaseItemData.DiscountPercent = 0;
                        }
                        else
                        {
                            mainapp.PurchaseItemData.DiscountPercent = Convert.ToDouble(frmcoll[discountpercent]);
                        }
                        if (frmcoll[discountrps] == "")
                        {
                            mainapp.PurchaseItemData.DiscountRPS = 0;
                        }
                        else
                        {
                            mainapp.PurchaseItemData.DiscountRPS = Convert.ToDouble(frmcoll[discountrps]);
                        }
                        mainapp.PurchaseItemData.Material = frmcoll[materialvalue];
                        mainapp.PurchaseItemData.Color = frmcoll[colorvalue];
                        mainapp.PurchaseItemData.Brand = frmcoll[brandvalue];
                        mainapp.PurchaseItemData.Size = frmcoll[sizevalue];
                        mainapp.PurchaseItemData.Design = frmcoll[design];
                        mainapp.PurchaseItemData.DesignName = frmcoll[designName];
                        mainapp.PurchaseItemData.Status = "Active";
                        mainapp.PurchaseItemData.ModifiedOn = DateTime.Now;
                        mainapp.PurchaseItemData.PoNo = model.PurchaseOrderData.PoNo;
                        mainapp.PurchaseItemData.Category = _itemsubcategoryservice.GetCategoryBySubCat(frmcoll[subcat]);
                        mainapp.PurchaseItemData.SubCategory = frmcoll[subcat];
                        mainapp.PurchaseItemData.ItemTax = frmcoll[itemtax];
                        mainapp.PurchaseItemData.ItemType = frmcoll[itemtype];
                        mainapp.PurchaseItemData.ProportionateTax = Convert.ToDouble(frmcoll[proportionatetax]);
                        mainapp.PurchaseItemData.ProportionateTaxAmt = Convert.ToDouble(frmcoll[proportionatetaxamount]);
                        _purchaseItemDetailService.CreatePurchaseItemDetail(mainapp.PurchaseItemData);
                    }
                }
            }
            model.PurchaseOrderData.TotalAmount = Convert.ToDouble(frmcoll["Total_Amount_Value"]);
            model.PurchaseOrderData.GrandTotal = Convert.ToDouble(frmcoll["Grand_Total_Value"]);
            if (model.PurchaseOrderData.GodownCode == null)
            {
                model.PurchaseOrderData.GodownArea = null;
            }
            model.PurchaseOrderData.status = "Active";
            model.PurchaseOrderData.modifiedOn = DateTime.Now;
            _purchaseOrderDetailService.UpdatePurchaseOrder(model.PurchaseOrderData);

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcoll["ItemTaxCount"]);
            model.PurchaseInventoryTaxDetails = new PurchaseInventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcoll[taxamount]) != 0)
                {
                    model.PurchaseInventoryTaxDetails.Code = model.PurchaseOrderData.PoNo;
                    model.PurchaseInventoryTaxDetails.Amount = frmcoll[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = frmcoll[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = frmcoll[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }
            TempData["PreviousPoNo"] = model.PurchaseOrderData.PoNo;
            string PoId = Encode(model.PurchaseOrderData.PoId.ToString());
            return RedirectToAction("PurchaseOrder/" + PoId, "PurchaseOrder");
        }

        [HttpGet]
        public ActionResult PurchaseOrderPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetPurchaseOrderNos(string term)
        {
            var podata = _purchaseOrderDetailService.GetAllPOesForPrint(term);
            List<string> names = new List<string>();
            foreach (var item in podata)
            {
                names.Add(item.PoNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPurchaseOrderDetailsForPrint(string pono)
        {
            MainApplication model = new MainApplication()
            {
                PurchaseOrderData = new PurchaseOrderDetail(),
            };
            model.PurchaseOrderData = _purchaseOrderDetailService.GetDetailByPoNo(pono);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(pono);
            model.PurchaseItemList = _purchaseItemDetailService.GetPurchaseItemsByPONo(pono);
            TempData["ConvertToPdf"] = "Yes";
            return View(model);
        }

        [HttpGet]
        public ActionResult ItemList(string Pono)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierDetails = new InwardFromSupplier();
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@pono",Pono),
            };
            string query = "GetItemCodesList" + " " + "@pono";
            var list = _InwardItemFromSupplierService.GetItemCodesByPoNo(query, para);
            List<InwardItemsFromSupplier> newlist = new List<InwardItemsFromSupplier>();
            foreach (var details in list)
            {
                double? quantity = 0;
                var itemData = _InwardItemFromSupplierService.GetItemDetails(details.itemCode, Pono);
                var itemlist = _InwardItemFromSupplierService.GetAllQuantity(details.itemCode, Pono);
                foreach (var qtybal in itemlist)
                {
                    quantity += qtybal.ReceivedQuantity + qtybal.ExtraQty;
                }
                //if (itemData.Balance < 0)
                //{
                //    itemData.Balance = 0;
                //}
                //else { itemData.Balance = itemData.OrderedQuantity - quantity; }
                itemData.ReceivedQuantity = quantity;
                newlist.Add(itemData);
            }
            model.InwardItemsFromSupplierList = newlist;
            model.InwardFromSupplierDetails.PoNo = Pono;
            return View(model);
        }

        [HttpGet]
        public JsonResult CompletePO(string Pono)
        {
            MainApplication model = new MainApplication();
            var data = _purchaseOrderDetailService.GetDetailByPoNo(Pono);
            data.ReceivingStatus = "Completed";
            _purchaseOrderDetailService.UpdatePurchaseOrder(data);
            return Json(Pono, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateSupplier()
        {
            MainApplication model = new MainApplication();
            model.SupplierDetails = new SupplierMaster();
            model.ItemSubCategoryList = _itemsubcategoryservice.GetItemSubCategories();
            model.SupplierDetails.CountriesList = _CountryService.getallcountries();
            model.SupplierDetails.StatesList = _stateservice.GetStateByCountry(1);
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateSupplierBank()
        {
            MainApplication model = new MainApplication();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateSupplier(MainApplication mainApplication, FormCollection frmsuppcollection)
        {
            var Details = _supplierMasterService.getLastInsertedSupplier();
            int catVal = 0;
            int length = 0;
            if (Details != null)
            {
                catVal = Details.SupplierId;
                catVal = catVal + 1;
                length = catVal.ToString().Length;
            }
            else
            {
                catVal = 1;
                length = 1;
            }
            string catCode = _utilityService.getName("SP", length, catVal);

            string LogedinUSerName = HttpContext.Session["UserName"].ToString();
            if (TempData["SupplierBankList" + LogedinUSerName] != null)
            {
                var list = TempData["SupplierBankList" + LogedinUSerName] as IEnumerable<SupplierBankDetail>;
                foreach (var bank in list)
                {
                    bank.SupplierCode = catCode;
                    bank.Status = "Statas";
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
            _supplierMasterService.CreateSupplier(mainApplication.SupplierDetails);
            var LastRow = _supplierMasterService.getLastInsertedSupplier();
            return RedirectToAction("SupplierDetails/" + LastRow.SupplierId, "PurchaseOrder");
        }

        [HttpGet]
        public ActionResult SupplierDetails(int id)
        {
            MainApplication model = new MainApplication();
            model.SupplierDetails = _supplierMasterService.getById(id);
            model.SupplierBankDetailList = _SupplierBankDetailService.GetDetailsFromBank(model.SupplierDetails.SupplierCode);
            return View(model);
        }


    }
}