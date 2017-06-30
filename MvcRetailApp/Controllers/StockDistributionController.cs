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
using System.Diagnostics;
using System.Threading;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class StockDistributionController : Controller
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
        private readonly IUtilityService _utilityservice;
        private readonly IGodownService _godownservice;
        private readonly IGodownAddressService _godownaddressservice;
        private readonly IShopService _shopservice;
        private readonly IOpeningStockService _openingstockservice;
        private readonly IEntryStockItemService _entrystockitemservice;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IItemService _itemservice;
        private readonly IStockDistributionService _stockdistributionservice;
        private readonly IStockItemDistributionService _stockitemdistributionservice;
        private readonly IOutwardStockDistributionService _outwardstkdisservice;
        private readonly IOutwardItemStockDistributionService _outwarditemstkdisservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public StockDistributionController(IUtilityService utilityservice, IGodownService godownservice, IGodownAddressService godownaddressservice, IShopService shopservice, IOpeningStockService openingstockservice, IEntryStockItemService entrystockitemservice, IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IItemService itemservice, IStockDistributionService stockdistributionservice,
                                           IStockItemDistributionService stockitemdistributionservice, IOutwardStockDistributionService outwardstkdisservice, IOutwardItemStockDistributionService outwarditemstkdisservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._utilityservice = utilityservice;
            this._godownservice = godownservice;
            this._godownaddressservice = godownaddressservice;
            this._shopservice = shopservice;
            this._openingstockservice = openingstockservice;
            this._entrystockitemservice = entrystockitemservice;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._itemservice = itemservice;
            this._stockdistributionservice = stockdistributionservice;
            this._stockitemdistributionservice = stockitemdistributionservice;
            this._outwardstkdisservice = outwardstkdisservice;
            this._outwarditemstkdisservice = outwarditemstkdisservice;
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

        //CREATE STOCK DISTRIBUTION
        [HttpGet]
        public ActionResult StockDistribution()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication()
            {
                StockDistributionDetails = new StockDistribution(),
            };

            //CREATE STOCK DISTRIBUTION CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string stockdistributioncode = string.Empty;
            
            var StkDisData = _stockdistributionservice.GetLastStockDisByFinYr(FinYr);
            int StkDisVal = 0;
            int length = 0;
            if (StkDisData != null)
            {
                stockdistributioncode = StkDisData.StockDistributionCode.Substring(3, 6);
                length = (Convert.ToInt32(stockdistributioncode) + 1).ToString().Length;
                StkDisVal = Convert.ToInt32(stockdistributioncode) + 1;
            }
            else
            {
                StkDisVal = 1;
                length = 1;
            }
            stockdistributioncode = _utilityservice.getName("SDS", length, StkDisVal);
            stockdistributioncode = stockdistributioncode + FinYr;
            model.StockDistributionDetails.StockDistributionCode = stockdistributioncode;
            TempData["PreviousStkDisNo"] = stockdistributioncode;

            model.GodownMasterList = _godownservice.GetGodownNames();
            model.ShopList = _shopservice.GetAll();
            model.ItemCategoryList = _itemcategoryservice.GetAllItemCategories();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }


        //GET GODOWN ADDRESS DDL 
        [HttpGet]
        public JsonResult GetGodownAddresses(string code)
        {
            string godownname = _godownservice.GetGodownByCode(code).GodownName;
            var godownAddress = _godownaddressservice.GetAddressList(code);
            var addresslist = godownAddress.Select(m => new SelectListItem
            {
                Text = m.AreaName,
                Value = m.AreaName
            });
            return Json(new { godownname, addresslist }, JsonRequestBehavior.AllowGet);
        }

        //GET GODOWN DETAILS
        [HttpGet]
        public JsonResult GetGodownDetails(string area, string GDCode)
        {
            var data = _godownaddressservice.GetAddressByAreaByCode(GDCode, area);
            var godowndetail = _godownservice.GetGodownByCode(data.GdCode);
            return Json(new
            {
                data.Address,
                godowndetail.GodownName,
                godowndetail.ContactNo1,
                godowndetail.EmpName,
                godowndetail.GodownEmail,
            }, JsonRequestBehavior.AllowGet);
        }

        //GET SHOP DETAILS
        [HttpGet]
        public JsonResult GetShopDetails(string shopcode)
        {
            var data = _shopservice.GetShopByCode(shopcode);
            return Json(new { data.ShopContactNo, data.EmpName, data.ShopName }, JsonRequestBehavior.AllowGet);
        }

        //GET ITEM LIST FOR STOCK DISTRIBUTION
        [HttpGet]
        public ActionResult GetItemList()
        {
            MainApplication model = new MainApplication();

            model.OpeningStockList = _openingstockservice.GetItemsByStatus();

            List<OpeningStockMaster> openinglist = new List<OpeningStockMaster>();
            //if item is already transfer in godown then Quantity= Stock Quantity - Transfer Quantity
            foreach (var data in model.OpeningStockList)
            {
                var stkdistrbtndata = _stockitemdistributionservice.GetRowsByItemCode(data.itemCode);
                //if item not fount in stock distribution then stocky quantity is same as a opening stock quantity
                if (stkdistrbtndata.Count() != 0)
                {
                    double? totalitemqty1 = 0;
                    foreach (var details in stkdistrbtndata)
                    {
                        totalitemqty1 += details.ItemQuantity;
                    }
                    data.TotalQuantity = data.TotalQuantity - totalitemqty1;
                }
                openinglist.Add(data);
            }
            model.OpeningStockList = openinglist;
            TempData["OpeningItemList"] = openinglist;

            model.EntryStockItemList = _entrystockitemservice.GetItemsByQuantity();

            List<EntryStockItem> entrylist = new List<EntryStockItem>();
            //if item is already transfer in godown then Quantity= Stock Quantity - Transfer Quantity
            foreach (var data in model.EntryStockItemList)
            {
                var stkdistrbtndata = _stockitemdistributionservice.GetRowsByItemCode(data.ItemCode);
                //if item not fount in stock distribution then stocky quantity is same as a entry stock quantity
                if (stkdistrbtndata.Count() != 0)
                {
                    double? totalitemqty2 = 0;
                    foreach (var details in stkdistrbtndata)
                    {
                        totalitemqty2 += details.ItemQuantity;
                    }
                    data.TotalQuantity = data.TotalQuantity - totalitemqty2;
                }
                entrylist.Add(data);
            }
            model.EntryStockItemList = entrylist;
            TempData["EntryItemList"] = entrylist;
            return View(model);
        }

        //SAVE DATA IN STOCK DISTRIBUTION MASTER
        [HttpPost]
        public ActionResult GetItemList(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                OutwardStkDisDetails = new OutwardStockDistribution(),
                OutwardItemStkDisDetails = new OutwardItemStockDistribution(),
                StockItemDistributionDetails = new StockItemDistribution(),
            };

            //CREATE STOCK DISTRIBUTION CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string stockdistributioncode = string.Empty;

            var StkDisData = _stockdistributionservice.GetLastStockDisByFinYr(FinYr);
            int StkDisVal = 0;
            int length = 0;
            if (StkDisData != null)
            {
                stockdistributioncode = StkDisData.StockDistributionCode.Substring(3, 6);
                length = (Convert.ToInt32(stockdistributioncode) + 1).ToString().Length;
                StkDisVal = Convert.ToInt32(stockdistributioncode) + 1;
            }
            else
            {
                StkDisVal = 1;
                length = 1;
            }
            stockdistributioncode = _utilityservice.getName("SDS", length, StkDisVal);
            stockdistributioncode = stockdistributioncode + FinYr;
            mainapp.StockDistributionDetails.StockDistributionCode = stockdistributioncode;

            //save stock distribution
            mainapp.StockDistributionDetails.Status = "Active";
            mainapp.StockDistributionDetails.ModifiedOn = DateTime.Now;
            _stockdistributionservice.Create(mainapp.StockDistributionDetails);

            //save outward stock distribution
            model.OutwardStkDisDetails.OutwardCode = stockdistributioncode;
            model.OutwardStkDisDetails.GodownName = mainapp.StockDistributionDetails.GodownName;
            model.OutwardStkDisDetails.Code = mainapp.StockDistributionDetails.Code;
            model.OutwardStkDisDetails.GodownContactNo = mainapp.StockDistributionDetails.GodownContactNo;
            model.OutwardStkDisDetails.GodownContactPerson = mainapp.StockDistributionDetails.GodownContactPerson;
            model.OutwardStkDisDetails.ShopName = mainapp.StockDistributionDetails.ShopName;
            model.OutwardStkDisDetails.ShopContactNo = mainapp.StockDistributionDetails.ShopContactNo;
            model.OutwardStkDisDetails.ShopContactPerson = mainapp.StockDistributionDetails.ShopContactPerson;
            model.OutwardStkDisDetails.GatePass = mainapp.StockDistributionDetails.GatePass;
            model.OutwardStkDisDetails.Narration = mainapp.StockDistributionDetails.Narration;
            model.OutwardStkDisDetails.Status = "Active";
            model.OutwardStkDisDetails.ModifiedOn = DateTime.Now;
            _outwardstkdisservice.Create(model.OutwardStkDisDetails);

            //save stock item distribution 
            mainapp.OpeningStockList = TempData["OpeningItemList"] as IEnumerable<OpeningStockMaster>;
            mainapp.EntryStockItemList = TempData["EntryItemList"] as IEnumerable<EntryStockItem>;

            int count = 1;
            foreach (var data in mainapp.EntryStockItemList)
            {
                string itemcode = "ItemCode" + count;
                string intertransfer = "InterTransfer" + count;
                if (frmcol[intertransfer] != "" && frmcol[intertransfer] != null)
                {
                    //if same item with same godowncode then update item in stockdistribution 
                    var itemdetails = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(frmcol[itemcode], mainapp.StockDistributionDetails.Code);
                    if (itemdetails == null)
                    {
                        //if item already prsent in stock distribution (same item and same godown) then update item otherwise create new item
                        var itemdata = _itemservice.GetDescriptionByItemCode(frmcol[itemcode]);
                        model.StockItemDistributionDetails.Category = itemdata.itemCategory;
                        model.StockItemDistributionDetails.SubCategory = itemdata.itemSubCategory;
                        model.StockItemDistributionDetails.ItemCode = itemdata.itemCode;
                        model.StockItemDistributionDetails.ItemName = itemdata.itemName;
                        model.StockItemDistributionDetails.Description = itemdata.description;
                        model.StockItemDistributionDetails.ItemQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        model.StockItemDistributionDetails.Color = itemdata.colorCode;
                        model.StockItemDistributionDetails.DesignCode = itemdata.designCode;
                        model.StockItemDistributionDetails.DesignName = itemdata.designName;
                        model.StockItemDistributionDetails.Unit = itemdata.unit;
                        model.StockItemDistributionDetails.NumberType = itemdata.NumberType;
                        model.StockItemDistributionDetails.SellingPrice = Convert.ToDouble(itemdata.sellingprice);
                        model.StockItemDistributionDetails.MRP = Convert.ToDouble(itemdata.mrp);
                        model.StockItemDistributionDetails.Material = itemdata.typeOfMaterial;
                        model.StockItemDistributionDetails.StockDistributionCode = stockdistributioncode;
                        model.StockItemDistributionDetails.Code = mainapp.StockDistributionDetails.Code;
                        if (mainapp.StockDistributionDetails.Code.Contains("GD"))
                        {
                            model.StockItemDistributionDetails.GodownShopName = _godownservice.GetGodownByCode(mainapp.StockDistributionDetails.Code).GodownName;
                        }
                        else
                        {
                            model.StockItemDistributionDetails.GodownShopName = _shopservice.GetShopByCode(mainapp.StockDistributionDetails.Code).ShopName;
                        }
                        model.StockItemDistributionDetails.Brand = itemdata.brandName;
                        model.StockItemDistributionDetails.Size = itemdata.size;
                        model.StockItemDistributionDetails.Barcode = itemdata.Barcode;
                        model.StockItemDistributionDetails.Status = "Active";
                        model.StockItemDistributionDetails.ModifiedOn = DateTime.Now;
                        _stockitemdistributionservice.Create(model.StockItemDistributionDetails);

                        //Save Outward Item To Godown

                        //Calculate total remaining quantity = Item Quantity + (Previous row quantity but this row having the same itemcode & godowncode)
                        var QuanDetails = _outwarditemstkdisservice.GetLastRowFromItemAndGodownCode(frmcol[itemcode], mainapp.StockDistributionDetails.Code);
                        if (QuanDetails != null)
                        {
                            double CalcQuan = Convert.ToDouble(frmcol[intertransfer]) + Convert.ToDouble(QuanDetails.TotalQuantity);
                            model.OutwardItemStkDisDetails.TotalQuantity = CalcQuan;
                        }
                        else
                        {
                            model.OutwardItemStkDisDetails.TotalQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        }

                        model.OutwardItemStkDisDetails.ItemCode = model.StockItemDistributionDetails.ItemCode;
                        model.OutwardItemStkDisDetails.ItemName = model.StockItemDistributionDetails.ItemName;
                        model.OutwardItemStkDisDetails.ItemQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        model.OutwardItemStkDisDetails.Code = mainapp.StockDistributionDetails.Code;
                        model.OutwardItemStkDisDetails.OutwardCode = stockdistributioncode;
                        model.OutwardItemStkDisDetails.Description = model.StockItemDistributionDetails.Description;
                        model.OutwardItemStkDisDetails.Color = model.StockItemDistributionDetails.Color;
                        model.OutwardItemStkDisDetails.DesignCode = model.StockItemDistributionDetails.DesignCode;
                        model.OutwardItemStkDisDetails.DesignName = model.StockItemDistributionDetails.DesignName;
                        model.OutwardItemStkDisDetails.Material = model.StockItemDistributionDetails.Material;
                        model.OutwardItemStkDisDetails.SellingPrice = model.StockItemDistributionDetails.SellingPrice;
                        model.OutwardItemStkDisDetails.MRP = model.StockItemDistributionDetails.MRP;
                        model.OutwardItemStkDisDetails.Unit = model.StockItemDistributionDetails.Unit;
                        model.OutwardItemStkDisDetails.NumberType = model.StockItemDistributionDetails.NumberType;
                        model.OutwardItemStkDisDetails.Brand = model.StockItemDistributionDetails.Brand;
                        model.OutwardItemStkDisDetails.Size = model.StockItemDistributionDetails.Size;
                        model.OutwardItemStkDisDetails.Barcode = model.StockItemDistributionDetails.Barcode;
                        model.OutwardItemStkDisDetails.Status = "Active";
                        model.OutwardItemStkDisDetails.ModifiedOn = DateTime.Now;
                        _outwarditemstkdisservice.Create(model.OutwardItemStkDisDetails);
                    }
                    else
                    {
                        itemdetails.ItemQuantity = itemdetails.ItemQuantity + Convert.ToDouble(frmcol[intertransfer]);
                        itemdetails.Status = "Active";
                        itemdetails.ModifiedOn = DateTime.Now;
                        _stockitemdistributionservice.Update(itemdetails);

                        //Save Outward Item To Godown

                        //Calculate total remaining quantity = Item Quantity + (Previous row quantity but this row having the same itemcode & godowncode)
                        var QuanDetails = _outwarditemstkdisservice.GetLastRowFromItemAndGodownCode(frmcol[itemcode], mainapp.StockDistributionDetails.Code);
                        if (QuanDetails != null)
                        {
                            double CalcQuan = Convert.ToDouble(frmcol[intertransfer]) + Convert.ToDouble(QuanDetails.TotalQuantity);
                            model.OutwardItemStkDisDetails.TotalQuantity = CalcQuan;
                        }
                        else
                        {
                            model.OutwardItemStkDisDetails.TotalQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        }

                        model.OutwardItemStkDisDetails.ItemCode = itemdetails.ItemCode;
                        model.OutwardItemStkDisDetails.ItemName = itemdetails.ItemName;
                        model.OutwardItemStkDisDetails.ItemQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        model.OutwardItemStkDisDetails.Code = mainapp.StockDistributionDetails.Code;
                        model.OutwardItemStkDisDetails.OutwardCode = stockdistributioncode;
                        model.OutwardItemStkDisDetails.Description = itemdetails.Description;
                        model.OutwardItemStkDisDetails.Color = itemdetails.Color;
                        model.OutwardItemStkDisDetails.DesignCode = itemdetails.DesignCode;
                        model.OutwardItemStkDisDetails.DesignName = itemdetails.DesignName;
                        model.OutwardItemStkDisDetails.Material = itemdetails.Material;
                        model.OutwardItemStkDisDetails.SellingPrice = itemdetails.SellingPrice;
                        model.OutwardItemStkDisDetails.MRP = itemdetails.MRP;
                        model.OutwardItemStkDisDetails.Unit = itemdetails.Unit;
                        model.OutwardItemStkDisDetails.NumberType = itemdetails.NumberType;
                        model.OutwardItemStkDisDetails.Brand = itemdetails.Brand;
                        model.OutwardItemStkDisDetails.Size = itemdetails.Size;
                        model.OutwardItemStkDisDetails.Barcode = itemdetails.Barcode;
                        model.OutwardItemStkDisDetails.Status = "Active";
                        model.OutwardItemStkDisDetails.ModifiedOn = DateTime.Now;
                        _outwarditemstkdisservice.Create(model.OutwardItemStkDisDetails);
                    }
                }
                count = count + 1;
            }

            foreach (var data in mainapp.OpeningStockList)
            {
                string itemcode = "ItemCode" + count;
                string intertransfer = "InterTransfer" + count;
                if (frmcol[intertransfer] != "" && frmcol[intertransfer] != null)
                {
                    //if same item with same godowncode then update item in stockdistribution 
                    var itemdetails = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(frmcol[itemcode], mainapp.StockDistributionDetails.Code);
                    if (itemdetails == null)
                    {
                        var itemdata = _itemservice.GetDescriptionByItemCode(frmcol[itemcode]);
                        model.StockItemDistributionDetails.Category = itemdata.itemCategory;
                        model.StockItemDistributionDetails.SubCategory = itemdata.itemSubCategory;
                        model.StockItemDistributionDetails.ItemCode = itemdata.itemCode;
                        model.StockItemDistributionDetails.ItemName = itemdata.itemName;
                        model.StockItemDistributionDetails.Description = itemdata.description;
                        model.StockItemDistributionDetails.ItemQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        model.StockItemDistributionDetails.Color = itemdata.colorCode;
                        model.StockItemDistributionDetails.DesignCode = itemdata.designCode;
                        model.StockItemDistributionDetails.DesignName = itemdata.designName;
                        model.StockItemDistributionDetails.Unit = itemdata.unit;
                        model.StockItemDistributionDetails.NumberType = itemdata.NumberType;
                        model.StockItemDistributionDetails.SellingPrice = Convert.ToDouble(itemdata.sellingprice);
                        model.StockItemDistributionDetails.MRP = Convert.ToDouble(itemdata.mrp);
                        model.StockItemDistributionDetails.Material = itemdata.typeOfMaterial;
                        model.StockItemDistributionDetails.StockDistributionCode = stockdistributioncode;
                        model.StockItemDistributionDetails.Code = mainapp.StockDistributionDetails.Code;
                        model.StockItemDistributionDetails.Brand = itemdata.brandName;
                        model.StockItemDistributionDetails.Size = itemdata.size;
                        model.StockItemDistributionDetails.Barcode = itemdata.Barcode;
                        model.StockItemDistributionDetails.Status = "Active";
                        model.StockItemDistributionDetails.ModifiedOn = DateTime.Now;

                        _stockitemdistributionservice.Create(model.StockItemDistributionDetails);

                        //Save Outward Item To Godown

                        //Calculate total remaining quantity = Item Quantity + (Previous row quantity but this row having the same itemcode & godowncode)
                        var QuanDetails = _outwarditemstkdisservice.GetLastRowFromItemAndGodownCode(frmcol[itemcode], mainapp.StockDistributionDetails.Code);
                        if (QuanDetails != null)
                        {
                            double CalcQuan = Convert.ToDouble(frmcol[intertransfer]) + Convert.ToDouble(QuanDetails.TotalQuantity);
                            model.OutwardItemStkDisDetails.TotalQuantity = CalcQuan;
                        }
                        else
                        {
                            model.OutwardItemStkDisDetails.TotalQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        }

                        model.OutwardItemStkDisDetails.ItemCode = model.StockItemDistributionDetails.ItemCode;
                        model.OutwardItemStkDisDetails.ItemName = model.StockItemDistributionDetails.ItemName;
                        model.OutwardItemStkDisDetails.ItemQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        model.OutwardItemStkDisDetails.Code = mainapp.StockDistributionDetails.Code;
                        model.OutwardItemStkDisDetails.OutwardCode = stockdistributioncode;
                        model.OutwardItemStkDisDetails.Description = model.StockItemDistributionDetails.Description;
                        model.OutwardItemStkDisDetails.Color = model.StockItemDistributionDetails.Color;
                        model.OutwardItemStkDisDetails.DesignCode = model.StockItemDistributionDetails.DesignCode;
                        model.OutwardItemStkDisDetails.DesignName = model.StockItemDistributionDetails.DesignName;
                        model.OutwardItemStkDisDetails.Material = model.StockItemDistributionDetails.Material;
                        model.OutwardItemStkDisDetails.SellingPrice = model.StockItemDistributionDetails.SellingPrice;
                        model.OutwardItemStkDisDetails.MRP = model.StockItemDistributionDetails.MRP;
                        model.OutwardItemStkDisDetails.Unit = model.StockItemDistributionDetails.Unit;
                        model.OutwardItemStkDisDetails.NumberType = model.StockItemDistributionDetails.NumberType;
                        model.OutwardItemStkDisDetails.Brand = model.StockItemDistributionDetails.Brand;
                        model.OutwardItemStkDisDetails.Size = model.StockItemDistributionDetails.Size;
                        model.OutwardItemStkDisDetails.Barcode = model.StockItemDistributionDetails.Barcode;
                        model.OutwardItemStkDisDetails.Status = "Active";
                        model.OutwardItemStkDisDetails.ModifiedOn = DateTime.Now;
                        _outwarditemstkdisservice.Create(model.OutwardItemStkDisDetails);
                    }
                    else
                    {
                        itemdetails.ItemQuantity = itemdetails.ItemQuantity + Convert.ToDouble(frmcol[intertransfer]);
                        itemdetails.Status = "Active";
                        itemdetails.ModifiedOn = DateTime.Now;
                        _stockitemdistributionservice.Update(itemdetails);

                        //Save Outward Item To Godown

                        //Calculate total remaining quantity = Item Quantity + (Previous row quantity but this row having the same itemcode & godowncode)
                        var QuanDetails = _outwarditemstkdisservice.GetLastRowFromItemAndGodownCode(frmcol[itemcode], mainapp.StockDistributionDetails.Code);
                        if (QuanDetails != null)
                        {
                            double CalcQuan = Convert.ToDouble(frmcol[intertransfer]) + Convert.ToDouble(QuanDetails.TotalQuantity);
                            model.OutwardItemStkDisDetails.TotalQuantity = CalcQuan;
                        }
                        else
                        {
                            model.OutwardItemStkDisDetails.TotalQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        }

                        model.OutwardItemStkDisDetails.ItemCode = itemdetails.ItemCode;
                        model.OutwardItemStkDisDetails.ItemName = itemdetails.ItemName;
                        model.OutwardItemStkDisDetails.ItemQuantity = Convert.ToDouble(frmcol[intertransfer]);
                        model.OutwardItemStkDisDetails.Code = mainapp.StockDistributionDetails.Code;
                        model.OutwardItemStkDisDetails.OutwardCode = stockdistributioncode;
                        model.OutwardItemStkDisDetails.Description = itemdetails.Description;
                        model.OutwardItemStkDisDetails.Color = itemdetails.Color;
                        model.OutwardItemStkDisDetails.DesignCode = itemdetails.DesignCode;
                        model.OutwardItemStkDisDetails.DesignName = itemdetails.DesignName;
                        model.OutwardItemStkDisDetails.Material = itemdetails.Material;
                        model.OutwardItemStkDisDetails.SellingPrice = itemdetails.SellingPrice;
                        model.OutwardItemStkDisDetails.MRP = itemdetails.MRP;
                        model.OutwardItemStkDisDetails.Unit = itemdetails.Unit;
                        model.OutwardItemStkDisDetails.NumberType = itemdetails.NumberType;
                        model.OutwardItemStkDisDetails.Brand = itemdetails.Brand;
                        model.OutwardItemStkDisDetails.Size = itemdetails.Size;
                        model.OutwardItemStkDisDetails.Barcode = itemdetails.Barcode;
                        model.OutwardItemStkDisDetails.Status = "Active";
                        model.OutwardItemStkDisDetails.ModifiedOn = DateTime.Now;
                        _outwarditemstkdisservice.Create(model.OutwardItemStkDisDetails);
                    }
                }
                count = count + 1;
            }

            var SDID = _stockdistributionservice.GetDetailsByCode(mainapp.StockDistributionDetails.StockDistributionCode).Id;
            var StkDisId = Encode(SDID.ToString());
            return RedirectToAction("StockDistributionDetails/" + StkDisId, "StockDistribution");
        }


        //SHOW DETAILS AFTER STOCK DISTRIBUTION
        [HttpGet]
        public ActionResult StockDistributionDetails(string Id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int id = Decode(Id);
            var StkDisCode = _stockdistributionservice.GetDetailsById(id).StockDistributionCode;
            model.OutwardStkDisDetails = _outwardstkdisservice.GetOutwardByCode(StkDisCode);
            model.OutwardItemStkDisList = _outwarditemstkdisservice.GetItemsByOutwardCode(StkDisCode);

            string previousstkdisno = TempData["PreviousStkDisNo"].ToString();
            if (previousstkdisno != model.OutwardStkDisDetails.OutwardCode)
            {
                ViewData["StkDisNoChanged"] = previousstkdisno + " is replaced with " + model.OutwardStkDisDetails.OutwardCode + " because " + previousstkdisno + " is acquired by another person";
            }
            TempData["PreviousStkDisNo"] = previousstkdisno;

            return View(model);
        }

        [HttpGet]
        public ActionResult PrintStockDistribution(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            var StkDisCode = _stockdistributionservice.GetDetailsById(Id).StockDistributionCode;
            model.OutwardStkDisDetails = _outwardstkdisservice.GetOutwardByCode(StkDisCode);
            model.OutwardItemStkDisList = _outwarditemstkdisservice.GetItemsByOutwardCode(StkDisCode);
            return View(model);
        }

        //******************************************* UPDATE STOCK DISTRIBUTION *************************************************

        // UPDATE STOCK DISTRIBUTIOM
        [HttpGet]
        public ActionResult StockdistributionUpdate()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            model.StockDistributionList = _stockdistributionservice.GetItemsByStatus();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE STOCK DISTRIBUTION NO
        [HttpGet]
        public JsonResult GetStockDistributionNo(string term)
        {
            var outstkdisdata = _outwardstkdisservice.GetOutwardStkDisNo(term);
            List<string> names = new List<string>();
            foreach (var item in outstkdisdata)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET DETAILS BY SALES ORDER NO
        [HttpGet]
        public ActionResult GetDetailsByStockDistributionNo(string stkdisno)
        {
            MainApplication model = new MainApplication();
            model.OutwardStkDisDetails = _outwardstkdisservice.GetOutwardByCode(stkdisno);
            model.OutwardItemStkDisList = _outwarditemstkdisservice.GetItemsByOutwardCode(stkdisno);
            var OSDItemList = model.OutwardItemStkDisList;
            OSDItemList.ToList().Clear();
            foreach (var item in model.OutwardItemStkDisList)
            {
                var entrystkdata = _entrystockitemservice.getDetailsByItemCode(item.ItemCode);
                var openingstkdata = _openingstockservice.GetDetailsByItemCode(item.ItemCode);
                if (entrystkdata != null)
                {
                    item.ReadOnlyQuantity = entrystkdata.TotalQuantity;
                }
                else
                {
                    item.ReadOnlyQuantity = openingstkdata.TotalQuantity;
                }
                OSDItemList.ToList().Add(item);
            }
            model.OutwardItemStkDisList = OSDItemList;
            TempData["OSDItemList"] = model.OutwardItemStkDisList;
            return View(model);
        }

        //SAVE UPDATE STOCK DISTRIBUTIOM
        [HttpPost]
        public ActionResult StockdistributionUpdate(MainApplication mainapp, FormCollection frmcol)
        {
            var OutwardStkDisCode = frmcol["OutwardStkDisCode"];
            double itemcount = Convert.ToDouble(frmcol["hdnRowCount"]);
            for (int i = 1; i <= itemcount; i++)
            {
                string itemcode = "ItemCode" + i;
                string intertransfer = "InterTransfer" + i;
                string previntertransfer = "PrevInterTransfer" + i;

                var intertransferval = Convert.ToDouble(frmcol[intertransfer]);
                var previntertransferval = Convert.ToDouble(frmcol[previntertransfer]);

                //update outward stock distribution items..
                var outwardstkdisitemdetails = _outwarditemstkdisservice.GetItemByItemAndOutwardCode(OutwardStkDisCode, frmcol[itemcode]);
                outwardstkdisitemdetails.ItemQuantity = Convert.ToDouble(intertransferval);
                outwardstkdisitemdetails.TotalQuantity = (outwardstkdisitemdetails.TotalQuantity - Convert.ToDouble(previntertransferval)) + Convert.ToDouble(intertransferval);
                outwardstkdisitemdetails.ModifiedOn = DateTime.Now;
                _outwarditemstkdisservice.Update(outwardstkdisitemdetails);

                //update stock distribution items..
                var stkdisitemdetails = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(frmcol[itemcode], frmcol["ShopGodownCode"]);
                stkdisitemdetails.ItemQuantity = (stkdisitemdetails.ItemQuantity - Convert.ToDouble(previntertransferval)) + Convert.ToDouble(intertransferval);
                _stockitemdistributionservice.Update(stkdisitemdetails);

                ////update entry stock and opening stock items
                //var entrystockitemdetails = _entrystockitemservice.getDetailsByItemCode(frmcol[itemcode]);
                //var openingstockdetails = _openingstockservice.GetDetailsByItemCode(frmcol[itemcode]);

                ////inter transfer difference
                //var intertransferdifference = intertransferval - previntertransferval;

                //if (entrystockitemdetails != null)
                //{
                //    entrystockitemdetails.TotalQuantity = entrystockitemdetails.TotalQuantity - intertransferdifference;
                //    _entrystockitemservice.Update(entrystockitemdetails);
                //}
                //else
                //{
                //    openingstockdetails.TotalQuantity = openingstockdetails.TotalQuantity - intertransferdifference;
                //    _openingstockservice.UpdateStock(openingstockdetails);
                //}
            }

            TempData["PreviousStkDisNo"] = OutwardStkDisCode;
            var SDID = _stockdistributionservice.GetDetailsByCode(OutwardStkDisCode).Id;
            var StkDisId = Encode(SDID.ToString());
            return RedirectToAction("StockDistributionDetails/" + StkDisId, "StockDistribution");
        }

        //******************************************* PRINT STOCK DISTRIBUTION *************************************************

        //PRINT STOCK DISTRIBUTION DETAILS
        [HttpGet]
        public ActionResult StockdistributionPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE STOCK DISTRIBUTION NO
        [HttpGet]
        public JsonResult GetStkDisNo(string term)
        {
            var outstkdisdata = _outwardstkdisservice.GetAllOutwardNos(term);
            List<string> names = new List<string>();
            foreach (var item in outstkdisdata)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET DETAILS BY STOCK DISTRIBUTION NO FOR PRINT
        [HttpGet]
        public ActionResult GetStkDisDetailsForPrint(string stkdisno)
        {
            MainApplication model = new MainApplication();
            model.OutwardStkDisDetails = _outwardstkdisservice.GetOutwardByCode(stkdisno);
            model.OutwardItemStkDisList = _outwarditemstkdisservice.GetItemsByOutwardCode(stkdisno);
            return View(model);
        }
    }
}
