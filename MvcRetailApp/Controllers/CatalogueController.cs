using CodeFirstData;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRetailApp.Filters;
using System.IO;

namespace MvcRetailApp.Controllers
{
    //[NoDirectAccess]
    [SessionExpireFilter]
    public class CatalogueController : Controller
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
        private readonly IClientMasterService _ClientMasterService;
        private readonly ICountryService _CountryService;
        private readonly IStateService _StateService;
        private readonly IDistrictService _DistrictService;
        private readonly IBankService _BankService;
        private readonly IBankNameService _BankNameService;
        private readonly IUtilityService _UtilityService;
        private readonly IClientBankDetailService _ClientBankDetailService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly ICatalogueService _CatalogueService;
        private readonly IBarcodeNumberService _BarcodeNumberService;
        private readonly IItemNameService _ItemNameService;
        private readonly IUniversalBarcodeNumberService _UniversalBarcodeNumberService;
        private readonly IItemService _ItemService;
        private readonly IColorCodeService _ColorCodeService;
        private readonly IItemCategoryService _CategoryService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IUnitService _UnitService;

        public CatalogueController(IClientMasterService ClientMasterService, ICountryService CountryService, IStateService StateService,
            IDistrictService DistrictService, IBankService BankService, IBankNameService BankNameService, IUtilityService UtilityService, IUserCredentialService usercredentialservice, IModuleService iIModuleService,
            IClientBankDetailService ClientBankDetailService, ICatalogueService CatalogueService, IBarcodeNumberService BarcodeNumberService, IItemNameService ItemNameService, 
            IUniversalBarcodeNumberService UniversalBarocodeNumberService,IItemService ItemService,IColorCodeService ColorCodeService,IItemCategoryService CategoryService,
            IEntryStockItemService EntryStockItemService,IOpeningStockService OpeningStockService,IInwardItemFromSupplierService InwardItemFromSupplierService,IUnitService UnitService)
        {
            this._ClientMasterService = ClientMasterService;
            this._CountryService = CountryService;
            this._StateService = StateService;
            this._DistrictService = DistrictService;
            this._BankService = BankService;
            this._BankNameService = BankNameService;
            this._UtilityService = UtilityService;
            this._ClientBankDetailService = ClientBankDetailService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._CatalogueService = CatalogueService;
            this._BarcodeNumberService = BarcodeNumberService;
            this._ItemNameService = ItemNameService;
            this._UniversalBarcodeNumberService = UniversalBarocodeNumberService;
            this._ItemService = ItemService;
            this._ColorCodeService = ColorCodeService;
            this._CategoryService = CategoryService;
            this._EntryStockItemService = EntryStockItemService;
            this._OpeningStockService = OpeningStockService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._UnitService = UnitService;
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
            MainApplication model = new MainApplication();
            model.CatalogueDetails = new Catalogue();
            var details = _CatalogueService.GetLastCatalogue();
            int Catval = 0;
            int length = 0;
            if (details != null)
            {
                string bn = details.BookNumber;
                Catval =   Convert.ToInt32(bn.Substring(2));
                Catval = Catval + 1;
                length = Catval.ToString().Length;
            }
            else
            {
                Catval = 1;
                length = 1;
            }
            string bookno = _UtilityService.getName("BN", length,Catval);
            model.CatalogueDetails.BookNumber = bookno;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MainApplication model,FormCollection form)
        {
            model.CatalogueDetails = new Catalogue();
            var detailsbefore = _CatalogueService.GetLastCatalogue();
            int Catval = 0;
            int length = 0;
            if (detailsbefore != null)
            {
                Catval = detailsbefore.Id;
                Catval = Catval + 1;
                length = Catval.ToString().Length;
            }
            else
            {
                Catval = 1;
                length = 1;
            }
            string bookno = _UtilityService.getName("BN", length, Catval);
            model.CatalogueDetails.BookNumber = bookno;

            int count = Convert.ToInt32(form["hdnRowCount"]);
            for (int i = 1; i <= count; i++)
            {
                string typeOfMaterial = "materialType" + i;
                string colorCode = "colorCodeData" + i;
                string designCode = "designCodeData" + i;
                string designname = "designName" + i;
                string description = "itemDescription" + i;
                string size = "Size" + i;
                string unit = "Unit" + i;
                string brand = "Brand" + i;
                string costprice = "CostPrice" + i;
                string sellingprice = "SellingPrice" + i;
                string mrp = "MRP" + i;
                string subcategory = "subcategory" + i;
                string itemname = "itemname" + i;
                string itemtype = "ItemType" + i;
                string serialno = "serialno" + i;
                string suppliername = "supplier" + i;
                string numbertype = "NumberType" + i;

                if (form[itemname] != null)
                {
                    model.CatalogueDetails = new Catalogue();

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
                    string itemcode = _UtilityService.getName("I", lenght, itemval);
                    model.CatalogueDetails.ItemCode = itemcode;

                    model.CatalogueDetails.BookNumber = form["CatalogueDetails.BookNumber"];
                    model.CatalogueDetails.Category = form["CatalogueDetails.Category"];
                    if (form[colorCode] != "")
                        model.CatalogueDetails.Color = form[colorCode];
                    else
                        model.CatalogueDetails.Color = null;
                    var colordetails = _ColorCodeService.GetcolorcodebyName(form[colorCode]);
                    if (colordetails == null)
                    {
                        ColorCode color = new ColorCode();
                        color.colorName = model.CatalogueDetails.Color;

                        var colordata = _ColorCodeService.GetLastColor();
                        int colorval = 0;
                        int lengths = 0;
                        if (colordata != null)
                        {
                            colorval = Convert.ToInt32(colordata.colorCode);
                            colorval = Convert.ToInt32(colorval) + 1;
                            lengths = colorval.ToString().Length;
                        }
                        else
                        {
                            colorval = 1;
                            lengths = 1;
                        }
                        color.colorCode = colorval.ToString();
                        color.colorName = form[colorCode];
                        color.Status = "Active";
                        color.modifiedOn = DateTime.Now;
                        _ColorCodeService.Create(color);
                    }
                    model.CatalogueDetails.Date = DateTime.Now;
                    model.CatalogueDetails.ItemName = form[itemname];
                    if (form[costprice] != "")
                        model.CatalogueDetails.CostPrice = form[costprice];
                    else
                        model.CatalogueDetails.CostPrice = "0";
                    if (form[mrp] != "")
                        model.CatalogueDetails.MRP = form[mrp];
                    else
                        model.CatalogueDetails.MRP = "0";
                    if (form[sellingprice] != "")
                        model.CatalogueDetails.SellingPrice = form[sellingprice];
                    else
                        model.CatalogueDetails.SellingPrice = "0";
                    if (form[serialno] != "")
                        model.CatalogueDetails.SerialNumber = form[serialno];
                    else
                        model.CatalogueDetails.SerialNumber = null;
                    model.CatalogueDetails.Brand = form[brand];
                    model.CatalogueDetails.Description = form[description];
                    model.CatalogueDetails.Design = form[designCode];
                    model.CatalogueDetails.DesignCode = form[designname];
                    model.CatalogueDetails.Size = form[size];
                    model.CatalogueDetails.ItemType = form["ItemType"];
                    model.CatalogueDetails.Status = "Active";
                    model.CatalogueDetails.Subcategory = form["CatalogueDetails.Subcategory"];
                    model.CatalogueDetails.SupplierName = form["supplier"];
                    model.CatalogueDetails.TypeOfMaterial = form[typeOfMaterial];
                    model.CatalogueDetails.Unit = form[unit];
                    model.CatalogueDetails.NumberType = form[numbertype];
                    model.CatalogueDetails.ModifiedOn = DateTime.Now;

                    var details = _ItemNameService.GetDataByItemName(model.CatalogueDetails.ItemName);
                    if (details == null)
                    {
                        model.ItemNameDetails = new ItemName();
                        model.ItemNameDetails.Name = model.CatalogueDetails.ItemName;
                        model.ItemNameDetails.ItemCategory = model.CatalogueDetails.Category;
                        model.ItemNameDetails.ItemSubcategory = model.CatalogueDetails.Subcategory;
                        model.ItemNameDetails.ItemType = model.CatalogueDetails.ItemType;
                        model.ItemNameDetails.DeleteStatus = "InActive";
                        model.ItemNameDetails.Status = "Active";
                        model.ItemNameDetails.ModifiedOn = DateTime.Now;
                        _ItemNameService.Create(model.ItemNameDetails);
                    }

                    System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                    string barcodeNo = string.Empty;
                    //var Lastbarcode = _UniversalBarcodeNumber.GetLastBarcodeNumber();
                    var Lastbarcode = _UniversalBarcodeNumberService.GetLastBarcode();
                    int serialNo;
                    /*To Check if it's the first barcode*/
                    if (Lastbarcode == null)
                    {
                        serialNo = 1111111;
                    }
                    else
                    {
                        int position = (Lastbarcode.Number.IndexOf(".") - 3);
                        serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                        serialNo += 1;
                    }
                    /*Creation Of Barcode*/
                    barcodeNo = model.CatalogueDetails.ItemName.Substring(0, 1).ToUpper() + serialNo.ToString() + model.CatalogueDetails.ItemName.Substring(model.CatalogueDetails.ItemName.Length - 1).ToUpper() + "U";
                    if (System.IO.File.Exists(Server.MapPath("../Barcode.txt")))
                    {
                        System.IO.File.Delete(Server.MapPath("../BarCode.txt"));
                    }
                    System.IO.File.WriteAllText(Server.MapPath("../BarCode.txt"), barcodeNo);
                    Process.Start(Server.MapPath("../BarCodeGenerate.exe"));

                    imgBarCode.ImageUrl = "../../Images/" + barcodeNo + ".png";

                    /*Converting image into binary*/
                    string filepath = Server.MapPath("/Images/" + barcodeNo + ".png");
                    System.Threading.Thread.Sleep(1000);
                    FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    /*Saving the Barcode Number*/
                    model.UniversalBarcodeNumberDetails = new UniversalBarcodeNumber();
                    model.UniversalBarcodeNumberDetails.Number = (barcodeNo + ".png").ToString();
                    model.UniversalBarcodeNumberDetails.BarcodeImage = bytes;
                    model.UniversalBarcodeNumberDetails.ModifiedDate = DateTime.Now;
                    _UniversalBarcodeNumberService.Create(model.UniversalBarcodeNumberDetails);
                    model.CatalogueDetails.CatalogueBarcode = barcodeNo + ".png";
                    fs.Close();
                    br.Dispose();

                    var itemdetails = _ItemService.GetDescriptionByItemCode(model.CatalogueDetails.ItemCode);
                    if (itemdetails != null)
                    {
                        if (itemdetails.brandName != model.CatalogueDetails.Brand && itemdetails.colorCode != model.CatalogueDetails.Color && itemdetails.costprice != model.CatalogueDetails.CostPrice && itemdetails.designCode != model.CatalogueDetails.DesignCode && itemdetails.designName != model.CatalogueDetails.Design && itemdetails.mrp != model.CatalogueDetails.MRP &&
                            itemdetails.sellingprice != model.CatalogueDetails.SellingPrice && itemdetails.size != model.CatalogueDetails.Size && itemdetails.typeOfMaterial != model.CatalogueDetails.TypeOfMaterial && itemdetails.unit != model.CatalogueDetails.Unit)
                        {
                            Item item = new Item();
                            item.UniversalBarcode = model.CatalogueDetails.CatalogueBarcode;
                            item.brandName = model.CatalogueDetails.Brand;
                            item.colorCode = model.CatalogueDetails.Color;
                            item.costprice = model.CatalogueDetails.CostPrice;
                            item.description = model.CatalogueDetails.Description;
                            item.designCode = model.CatalogueDetails.DesignCode;
                            item.designName = model.CatalogueDetails.Design;
                            item.itemCategory = model.CatalogueDetails.Category;
                            item.itemCode = model.CatalogueDetails.ItemCode;
                            item.itemName = model.CatalogueDetails.ItemName;
                            item.itemSubCategory = model.CatalogueDetails.Subcategory;
                            item.itemtype = model.CatalogueDetails.ItemType;
                            item.modifedOn = DateTime.Now;
                            item.NumberType = model.CatalogueDetails.NumberType;
                            item.mrp = model.CatalogueDetails.MRP;
                            item.sellingprice = model.CatalogueDetails.SellingPrice;
                            item.size = model.CatalogueDetails.Size;
                            item.status = "Active";
                            item.typeOfMaterial = model.CatalogueDetails.TypeOfMaterial;
                            item.unit = model.CatalogueDetails.Unit;
                            if (item.sellingprice == null)
                                item.sellingprice = "0";
                            if (item.mrp == null)
                                item.mrp = "0";
                            _ItemService.createItem(item);
                        }
                    }
                    else
                    {
                        Item item = new Item();
                        
                        item.Barcode = model.CatalogueDetails.InwardBarcode;
                        item.brandName = model.CatalogueDetails.Brand;
                        item.colorCode = model.CatalogueDetails.Color;
                        item.costprice = model.CatalogueDetails.CostPrice;
                        item.description = model.CatalogueDetails.Description;
                        item.designCode = model.CatalogueDetails.DesignCode;
                        item.designName = model.CatalogueDetails.Design;
                        item.itemCategory = model.CatalogueDetails.Category;
                        item.itemCode = model.CatalogueDetails.ItemCode;
                        item.itemName = model.CatalogueDetails.ItemName;
                        item.itemSubCategory = model.CatalogueDetails.Subcategory;
                        item.itemtype = model.CatalogueDetails.ItemType;
                        item.modifedOn = DateTime.Now;
                        item.NumberType = model.CatalogueDetails.NumberType;
                        item.mrp = model.CatalogueDetails.MRP;
                        item.sellingprice = model.CatalogueDetails.SellingPrice;
                        item.size = model.CatalogueDetails.Size;
                        item.status = "Active";
                        item.typeOfMaterial = model.CatalogueDetails.TypeOfMaterial;
                        item.unit = model.CatalogueDetails.Unit;
                        item.UniversalBarcode = model.CatalogueDetails.CatalogueBarcode;
                        if (item.sellingprice == null)
                            item.sellingprice = "0";
                        if (item.mrp == null)
                            item.mrp = "0";
                        _ItemService.createItem(item);
                    }
                   
                    FileInfo file = new FileInfo(filepath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    if (model.CatalogueDetails.SellingPrice == null)
                        model.CatalogueDetails.SellingPrice = "0";
                    if (model.CatalogueDetails.MRP == null)
                        model.CatalogueDetails.MRP = "0";
                    _CatalogueService.Create(model.CatalogueDetails);
                }
            }
            int detailsafter = _CatalogueService.GetLastCatalogue().Id;

            var cataloguebefore = Encode(Catval.ToString());
            var catalogueafter = Encode(detailsafter.ToString());

            return RedirectToAction("Details/", "Catalogue", new { CatalogueBefore = cataloguebefore, CatalogueAfter = catalogueafter, });
        }

        [HttpGet]
        public ActionResult Details(string CatalogueBefore,string CatalogueAfter)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int id1 = Decode(CatalogueBefore);
            int id2 = Decode(CatalogueAfter);
            model.CatalogueDetails = _CatalogueService.GetCatalogue(id1);
            model.CatalogueList = _CatalogueService.GetInsertedRow(id1, id2);

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
            model.CatalogueList = _CatalogueService.GetAll();
            model.ItemCategoryList = _CategoryService.GetAllItemCategories();
            return View(model);
        }

        [HttpGet]
        public ActionResult GetDataByBookNumber(string BookNumber)
        {
            MainApplication model = new MainApplication();
            model.CatalogueList = _CatalogueService.GetDataByBookNumber(BookNumber);
            Session["CatalogueList"] = model.CatalogueList;
            Session["count"] = model.CatalogueList.Count();
            List<int> ids = new List<int>();
            foreach (var data in model.CatalogueList)
            {
                ids.Add(data.Id);
            }
            Session["ids"] = ids;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MainApplication model, FormCollection form)
        {
            int alreadypresent = Convert.ToInt32(Session["count"]);
            int count = Convert.ToInt32(form["hdnRowCount"]);
            
            var list = Session["ids"] as IEnumerable<int>;
            int[] a = new int[list.Count()];

            int b=0;
            foreach (var data in list)
            {
                a[b] = data;
                b++;
            }

            for (int i = 1; i <= alreadypresent; i++)
            {
                string typeOfMaterial = "materialType" + i;
                string colorCode = "colorCodeData" + i;
                string designCode = "designCodeData" + i;
                string designname = "designName" + i;
                string description = "itemDescription" + i;
                string size = "Size" + i;
                string unit = "Unit" + i;
                string brand = "Brand" + i;
                string costprice = "CostPrice" + i;
                string sellingprice = "SellingPrice" + i;
                string mrp = "MRP" + i;
                string subcategory = "subcategory" + i;
                string itemname = "itemname" + i;
                string itemtype = "ItemType" + i;
                string serialno = "serialno" + i;
                string suppliername = "supplier" + i;
                string numbertype = "numbertype" + i;
                string barcode = "barcode" + i;
                string id = ("Id") + i;
                string ItemCode = "ItemCode" + i;

                if (form[itemname] != null)
                {
                    model.CatalogueDetails = new Catalogue();

                    model.CatalogueDetails.BookNumber = form["CatalogueDetails.BookNumber"];
                    model.CatalogueDetails.Category = form["CatalogueDetails.Category"];
                    if (form[colorCode] != "")
                        model.CatalogueDetails.Color = form[colorCode];
                    else
                        model.CatalogueDetails.Color = null;
                    var colordetails = _ColorCodeService.GetcolorcodebyName(form[colorCode]);
                    if (colordetails == null)
                    {
                        ColorCode color = new ColorCode();
                        color.colorName = model.CatalogueDetails.Color;

                        var colordata = _ColorCodeService.GetLastColor();
                        int colorval = 0;
                        int lengths = 0;
                        if (colordata != null)
                        {
                            colorval = Convert.ToInt32(colordata.colorCode);
                            colorval = Convert.ToInt32(colorval) + 1;
                            lengths = colorval.ToString().Length;
                        }
                        else
                        {
                            colorval = 1;
                            lengths = 1;
                        }
                        color.colorCode = colorval.ToString();
                        color.colorName = form[colorCode];
                        color.Status = "Active";
                        color.modifiedOn = DateTime.Now;
                        _ColorCodeService.Create(color);
                    }
                    model.CatalogueDetails.Date = DateTime.Now;
                    model.CatalogueDetails.ItemName = form[itemname];
                    if (form[costprice] != "")
                        model.CatalogueDetails.CostPrice = form[costprice];
                    else
                        model.CatalogueDetails.CostPrice = "0";
                    if (form[mrp] != "")
                        model.CatalogueDetails.MRP = form[mrp];
                    else
                        model.CatalogueDetails.MRP = "0";
                    if (form[sellingprice] != "")
                        model.CatalogueDetails.SellingPrice = form[sellingprice];
                    else
                        model.CatalogueDetails.SellingPrice = "0";
                    if (form[serialno] != "")
                        model.CatalogueDetails.SerialNumber = form[serialno];
                    else
                        model.CatalogueDetails.SerialNumber = null;
                    model.CatalogueDetails.Brand = form[brand];
                    model.CatalogueDetails.Description = form[description];
                    model.CatalogueDetails.Design = form[designCode];
                    model.CatalogueDetails.DesignCode = form[designname];
                    model.CatalogueDetails.Size = form[size];
                    model.CatalogueDetails.ItemType = form["ItemType"];
                    model.CatalogueDetails.Status = "Active";
                    model.CatalogueDetails.Subcategory = form["CatalogueDetails.Subcategory"];
                    model.CatalogueDetails.SupplierName = form["supplier"];
                    model.CatalogueDetails.TypeOfMaterial = form[typeOfMaterial];
                    model.CatalogueDetails.Unit = form[unit];
                    model.CatalogueDetails.NumberType = form[numbertype];
                    model.CatalogueDetails.ModifiedOn = DateTime.Now;
                    model.CatalogueDetails.CatalogueBarcode = form[barcode];
                    model.CatalogueDetails.ItemCode = form[ItemCode];
                    model.CatalogueDetails.Id = a[i - 1];
                    if (model.CatalogueDetails.SellingPrice == null)
                        model.CatalogueDetails.SellingPrice = "0";
                    if (model.CatalogueDetails.MRP == null)
                        model.CatalogueDetails.MRP = "0";

                    var details = _ItemService.GetDescriptionByItemCode(form[ItemCode]);
                    details.itemCode = model.CatalogueDetails.ItemCode;
                    details.brandName = model.CatalogueDetails.Brand;
                    details.colorCode = model.CatalogueDetails.Color;
                    details.costprice = model.CatalogueDetails.CostPrice;
                    details.description = model.CatalogueDetails.Description;
                    details.itemCategory = model.CatalogueDetails.Category;
                    details.itemSubCategory = model.CatalogueDetails.Subcategory;
                    details.itemtype = model.CatalogueDetails.ItemType;
                    details.mrp = model.CatalogueDetails.MRP;
                    details.NumberType = model.CatalogueDetails.NumberType;
                    details.sellingprice = model.CatalogueDetails.SellingPrice;
                    details.size = model.CatalogueDetails.Size;
                    details.unit = model.CatalogueDetails.Unit;
                    details.typeOfMaterial = model.CatalogueDetails.TypeOfMaterial;
                    details.status = "Active";
                    details.modifedOn = DateTime.Now;
                    _ItemService.updateItem(details);

                    _CatalogueService.Update(model.CatalogueDetails);
                }
            }

            int verify = count - alreadypresent;

            if (verify != 0)
            {
                for (int i = alreadypresent + 1; i <= count; i++)
                {
                    string typeOfMaterial = "materialType" + i;
                    string colorCode = "colorCodeData" + i;
                    string designCode = "designCodeData" + i;
                    string designname = "designName" + i;
                    string description = "itemDescription" + i;
                    string size = "Size" + i;
                    string unit = "Unit" + i;
                    string brand = "Brand" + i;
                    string costprice = "CostPrice" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string subcategory = "subcategory" + i;
                    string itemname = "itemname" + i;
                    string itemtype = "ItemType" + i;
                    string serialno = "serialno" + i;
                    string suppliername = "supplier" + i;
                    string numbertype = "NumberType" + i;

                    if (form[itemname] != null)
                    {
                        model.CatalogueDetails = new Catalogue();

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
                        string itemcode = _UtilityService.getName("I", lenght, itemval);
                        model.CatalogueDetails.ItemCode = itemcode;

                        model.CatalogueDetails.BookNumber = form["CatalogueDetails.BookNumber"];
                        model.CatalogueDetails.Category = form["CatalogueDetails.Category"];
                        if (form[colorCode] != "")
                            model.CatalogueDetails.Color = form[colorCode];
                        else
                            model.CatalogueDetails.Color = null;
                        var colordetails = _ColorCodeService.GetcolorcodebyName(form[colorCode]);
                        if (colordetails == null)
                        {
                            ColorCode color = new ColorCode();
                            color.colorName = model.CatalogueDetails.Color;

                            var colordata = _ColorCodeService.GetLastColor();
                            int colorval = 0;
                            int lengths = 0;
                            if (colordata != null)
                            {
                                colorval = Convert.ToInt32(colordata.colorCode);
                                colorval = Convert.ToInt32(colorval) + 1;
                                lengths = colorval.ToString().Length;
                            }
                            else
                            {
                                colorval = 1;
                                lengths = 1;
                            }
                            color.colorCode = colorval.ToString();
                            color.colorName = form[colorCode];
                            color.Status = "Active";
                            color.modifiedOn = DateTime.Now;
                            _ColorCodeService.Create(color);
                        }
                        model.CatalogueDetails.Date = DateTime.Now;
                        model.CatalogueDetails.ItemName = form[itemname];
                        if (form[costprice] != "")
                            model.CatalogueDetails.CostPrice = form[costprice];
                        else
                            model.CatalogueDetails.CostPrice = "0";
                        if (form[mrp] != "")
                            model.CatalogueDetails.MRP = form[mrp];
                        else
                            model.CatalogueDetails.MRP = "0";
                        if (form[sellingprice] != "")
                            model.CatalogueDetails.SellingPrice = form[sellingprice];
                        else
                            model.CatalogueDetails.SellingPrice = "0";
                        if (form[serialno] != "")
                            model.CatalogueDetails.SerialNumber = form[serialno];
                        else
                            model.CatalogueDetails.SerialNumber = null;
                        model.CatalogueDetails.Brand = form[brand];
                        model.CatalogueDetails.Description = form[description];
                        model.CatalogueDetails.Design = form[designCode];
                        model.CatalogueDetails.DesignCode = form[designname];
                        model.CatalogueDetails.Size = form[size];
                        model.CatalogueDetails.ItemType = form["ItemType"];
                        model.CatalogueDetails.Status = "Active";
                        model.CatalogueDetails.Subcategory = form["CatalogueDetails.Subcategory"];
                        model.CatalogueDetails.SupplierName = form["supplier"];
                        model.CatalogueDetails.TypeOfMaterial = form[typeOfMaterial];
                        model.CatalogueDetails.Unit = form[unit];
                        model.CatalogueDetails.NumberType = _UnitService.GetDetailsByName(form[unit]).NumberType;
                        model.CatalogueDetails.ModifiedOn = DateTime.Now;

                        if (model.CatalogueDetails.SellingPrice == null)
                            model.CatalogueDetails.SellingPrice = "0";
                        if (model.CatalogueDetails.MRP == null)
                            model.CatalogueDetails.MRP = "0";

                        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                        string barcodeNo = string.Empty;
                        //var Lastbarcode = _UniversalBarcodeNumber.GetLastBarcodeNumber();
                        var Lastbarcode = _UniversalBarcodeNumberService.GetLastBarcode();
                        int serialNo;
                        /*To Check if it's the first barcode*/
                        if (Lastbarcode == null)
                        {
                            serialNo = 1111111;
                        }
                        else
                        {
                            int position = (Lastbarcode.Number.IndexOf(".") - 3);
                            serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                            serialNo += 1;
                        }
                        /*Creation Of Barcode*/
                        barcodeNo = model.CatalogueDetails.ItemName.Substring(0, 1).ToUpper() + serialNo.ToString() + model.CatalogueDetails.ItemName.Substring(model.CatalogueDetails.ItemName.Length - 1).ToUpper() + "U";
                        if (System.IO.File.Exists(Server.MapPath("../Barcode.txt")))
                        {
                            System.IO.File.Delete(Server.MapPath("../BarCode.txt"));
                        }
                        System.IO.File.WriteAllText(Server.MapPath("../BarCode.txt"), barcodeNo);
                        Process.Start(Server.MapPath("../BarCodeGenerate.exe"));

                        imgBarCode.ImageUrl = "../../Images/" + barcodeNo + ".png";

                        /*Converting image into binary*/
                        string filepath = Server.MapPath("/Images/" + barcodeNo + ".png");
                        System.Threading.Thread.Sleep(1000);
                        FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                        /*Saving the Barcode Number*/
                        model.UniversalBarcodeNumberDetails = new UniversalBarcodeNumber();
                        model.UniversalBarcodeNumberDetails.Number = (barcodeNo + ".png").ToString();
                        model.UniversalBarcodeNumberDetails.BarcodeImage = bytes;
                        model.UniversalBarcodeNumberDetails.ModifiedDate = DateTime.Now;
                        _UniversalBarcodeNumberService.Create(model.UniversalBarcodeNumberDetails);
                        model.CatalogueDetails.CatalogueBarcode = barcodeNo + ".png";
                        fs.Close();
                        br.Dispose();

                        Item item = new Item();
                        item.brandName = model.CatalogueDetails.Brand;
                        item.colorCode = model.CatalogueDetails.Color;
                        item.costprice = model.CatalogueDetails.CostPrice;
                        item.description = model.CatalogueDetails.Description;
                        item.itemCategory = model.CatalogueDetails.Category;
                        item.itemCode = model.CatalogueDetails.ItemCode;
                        item.itemName = model.CatalogueDetails.ItemName;
                        item.itemSubCategory = model.CatalogueDetails.Subcategory;
                        item.itemtype = model.CatalogueDetails.ItemType;
                        item.mrp = model.CatalogueDetails.MRP;
                        item.NumberType = model.CatalogueDetails.NumberType;
                        item.sellingprice = model.CatalogueDetails.SellingPrice;
                        item.size = model.CatalogueDetails.Size;
                        item.status = "Active";
                        item.typeOfMaterial = model.CatalogueDetails.TypeOfMaterial;
                        item.unit = model.CatalogueDetails.Unit;
                        item.UniversalBarcode = model.CatalogueDetails.CatalogueBarcode;
                        item.modifedOn = DateTime.Now;
                        _ItemService.createItem(item);

                        _CatalogueService.Create(model.CatalogueDetails);
                    }
                }
            }
            return RedirectToAction("Edit", "Catalogue");
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
            model.ItemCategoryList = _CategoryService.GetAllItemCategories();
            model.CatalogueList = _CatalogueService.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult CatalogueListForDelete(string subcategory)
        {
            MainApplication model = new MainApplication();
            var details = _CatalogueService.GetCataloguesBySubCategory(subcategory);
            List<Catalogue> catlist = new List<Catalogue>();
            foreach (var data in details)
            {
                var entrystock = _EntryStockItemService.getDetailsByItemCode(data.ItemCode);
                var openingstock = _OpeningStockService.GetDataByItemCode(data.ItemCode);
                var inwarddetails = _InwardItemFromSupplierService.GetItemDetailsByItemCode(data.ItemCode);
                if (entrystock == null && openingstock == null && inwarddetails == null)
                {
                    catlist.Add(data);
                }
            }
            model.CatalogueList = catlist;
            return View(model);
        }

        [HttpGet]
        public JsonResult InActiveItem(string code)
        {
            var data = _CatalogueService.GetRowByItemCode(code);
            data.Status = "InActive";
            data.ModifiedOn = DateTime.Now;
            _CatalogueService.Update(data);
            var itemdata = _ItemService.GetDescriptionByItemCode(code);
            if (itemdata != null)
            {
                itemdata.status = "InActive";
                itemdata.modifedOn = DateTime.Now;
                _ItemService.updateItem(itemdata);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}