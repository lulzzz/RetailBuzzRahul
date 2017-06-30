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
using System.IO;
using System.Drawing;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class StockController : Controller
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
        private readonly IOpeningStockService _openingstockservice;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IItemService _itemservice;
        private readonly ISuppliersMasterService _supplierservice;
        private readonly IUnitService _unitservice;
        private readonly IEntryStockService _entrystockservice;
        private readonly IEntryStockItemService _entrystockitemservice;
        private readonly IInwardFromSupplierService _performainvoiceservice;
        private readonly IInwardItemFromSupplierService _performainvoiceitemservice;
        private readonly IGodownService _godownservice;
        private readonly IGodownAddressService _godownaddressservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IBarcodeNumberService _BarcodeNumberService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IShopService _ShopService;
        private readonly IShopStockService _ShopStockService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly IPhysicalStockTakingService _PhysicalStockTakingService;
       // private readonly IBarcodeServices _BarcodeService;
        

        public StockController(IUtilityService utilityservice, IOpeningStockService openingstockservice, IItemCategoryService itemcategoryservice,
            IItemSubCategoryService itemsubcategoryservice, IItemService itemservice, ISuppliersMasterService supplierservice,
            IUnitService unitservice, IEntryStockService entrystockservice, IEntryStockItemService entrystockitemservice,
            IInwardFromSupplierService performainvoiceservice, IInwardItemFromSupplierService performainvoiceitemservice,
            IUserCredentialService usercredentialservice, IModuleService iIModuleService, IBarcodeNumberService BarcodeNumberService,
            IGodownService godownservice, IGodownAddressService godownaddressservice, IStockItemDistributionService StockItemDistributionService,
            IGodownStockService GodownStockService, IShopStockService ShopStockService, IDeliveryChallanItemService DeliveryChallanItemService,
            ISalesOrderItemService SalesOrderItemService, IShopService ShopService, IPhysicalStockTakingService PhysicalStockTakingService)
        {
            this._utilityservice = utilityservice;
            this._openingstockservice = openingstockservice;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._itemservice = itemservice;
            this._supplierservice = supplierservice;
            this._unitservice = unitservice;
            this._entrystockservice = entrystockservice;
            this._entrystockitemservice = entrystockitemservice;
            this._performainvoiceservice = performainvoiceservice;
            this._performainvoiceitemservice = performainvoiceitemservice;
            this._godownservice = godownservice;
            this._godownaddressservice = godownaddressservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._BarcodeNumberService = BarcodeNumberService;
            this._ShopStockService = ShopStockService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._SalesOrderItemService = SalesOrderItemService;
            this._GodownStockService = GodownStockService;
            this._ShopService = ShopService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._PhysicalStockTakingService = PhysicalStockTakingService;
            //this._BarcodeService = BarcodeService;
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

        public string decoded(string decodeMe)
        {
            byte[] decoded = Convert.FromBase64String(decodeMe);
            string decodedvalue = System.Text.Encoding.UTF8.GetString(decoded);
            return (decodedvalue);
        }

        //CREATE STOCK
        [HttpGet]
        public ActionResult Stock()
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

        //CREATE OPENING STOCK
        [HttpGet]
        public ActionResult OpeningStock()
        {
            MainApplication model = new MainApplication()
            {
                OpeningStockDetails = new OpeningStockMaster(),
            };

            //CREATE OPENING STOCK CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string openingstockcode = string.Empty;

            var openingstockdata = _openingstockservice.GetLastStockByFinYr(FinYr);
            int openstockVal = 0;
            int length = 0;
            if (openingstockdata != null)
            {
                openingstockcode = openingstockdata.OpeningStockCode.Substring(3, 6);
                length = (Convert.ToInt32(openingstockcode) + 1).ToString().Length;
                openstockVal = Convert.ToInt32(openingstockcode) + 1;
            }
            else
            {
                openstockVal = 1;
                length = 1;
            }
            openingstockcode = _utilityservice.getName("OST", length, openstockVal);
            openingstockcode = openingstockcode + FinYr;
            model.OpeningStockDetails.OpeningStockCode = openingstockcode;
            TempData["PreviousOpenStockNo"] = openingstockcode;

            //CONCAT CATEGORYCODE AND CATEGORYNAME
            var namelist = _itemcategoryservice.GetAllItemCategories();
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
            model.ShopList = _ShopService.GetAll();
            model.GodownMasterList = _godownservice.GetGodownNames();
            model.SupplierList = _supplierservice.getAllSuppliers();
            return View(model);
        }

        //LOAD UNIT LIST
        [HttpGet]
        public JsonResult LoadUnitList()
        {
            MainApplication model = new MainApplication();
            model.UnitList = _unitservice.getallsize();
            var data = model.UnitList.Select(s => new SelectListItem()
            {
                Text = s.UnitName,
                Value = s.UnitName
            });
            return Json(data, JsonRequestBehavior.AllowGet);
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

        //GET ITEM BY SUBCATEGORY
        [HttpGet]
        public JsonResult LoadItemBySubCategory(string subcat)
        {
            var itemlist = _itemservice.GetItemsBySubCategory(subcat);
            MainApplication model = new MainApplication();
            model.ItemList = itemlist;
            var modeldata = model.ItemList.Select(m => new SelectListItem()
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(modeldata, JsonRequestBehavior.AllowGet);
        }

        //GET ITEMNAME BY CATEGORY
        [HttpGet]
        public JsonResult LoadItemByCategory(string category)
        {
            MainApplication model = new MainApplication();
            model.ItemList = _itemservice.GetSubCatByCatCode(category);
            var data = model.ItemList.Select(m => new SelectListItem
                {
                    Text = m.itemName,
                    Value = m.itemId.ToString(),
                });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GET ITEM DETAILS BY ITEMCODE
        [HttpGet]
        public JsonResult GetItemDetails(string code)
        {
            var openingdata = _openingstockservice.GetDataByItemCode(code);
            if (openingdata != null)
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _itemservice.GetDescriptionByItemCode(code);
                return Json(new { data.description, data.itemName, data.colorCode, data.size, data.typeOfMaterial, data.brandName, data.designName, data.designCode, data.costprice, data.sellingprice, data.mrp, data.unit, data.NumberType }, JsonRequestBehavior.AllowGet);
            }
        }

        //GENERATE BARCODE
        [HttpGet]
        public JsonResult CreateBarcode(string itemData)
        {
            OpeningStockMaster openstock = new OpeningStockMaster();
            openstock.Barcode = itemData;

            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            string barcodeNo = string.Empty;

            var barcode = _BarcodeNumberService.GetLastBarcodeNumber();
            int serialNo;
            /*To Check if it's the first barcode*/
            if (barcode == null)
            {
                serialNo = 1111111;
            }
            else
            {
                int length = barcode.Number.Length;
                int position = (barcode.Number.IndexOf(".") - 2);
                //int position = length - 1;
                serialNo = Convert.ToInt32(barcode.Number.Substring(1, position));
                serialNo += 1;
            }

            /*Creation Of Barcode*/
            barcodeNo = itemData.Substring(0, 1).ToUpper() + serialNo.ToString() + itemData.Substring(itemData.Length - 1).ToUpper();
            if (System.IO.File.Exists(Server.MapPath("../Barcode.txt")))
            {
                System.IO.File.Delete(Server.MapPath("../BarCode.txt"));
            }
            System.IO.File.WriteAllText(Server.MapPath("../BarCode.txt"), barcodeNo);
            Process.Start(Server.MapPath("../BarCodeGenerate.exe"));
                 
            

           
            int barcodecount = Convert.ToInt32(barcodeNo.Substring(1, barcodeNo.Length - 2));
            imgBarCode.ImageUrl = "../../Images/" + barcodeNo + ".png";           

            /*Converting image into binary*/
            string filepath = "D:\\Dinesh\\MvcRetailApp1\\A2ZRetailApp\\RetailApp\\ATOZ\\MvcRetailApp\\Images/" + barcodeNo + ".png"; 
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            /*Saving the Barcode Number*/
            BarcodeNumber BarcodeNumbr = new BarcodeNumber();
            BarcodeNumbr.Number = barcodeNo + ".png";
            barcodeNo = barcodeNo.ToString();
            BarcodeNumbr.BarcodeImage = bytes;
            BarcodeNumbr.ModifiedDate = DateTime.Now;
            _BarcodeNumberService.CreateBarcode(BarcodeNumbr);
            fs.Close();
            br.Dispose();

            FileInfo file = new FileInfo(filepath);
            if (file.Exists)
            {
                file.Delete();
            }
            //if (System.IO.File.Exists(filepath))
            //{
                //System.IO.File.Delete(filepath);
            //}

            br.Close();
            fs.Close();
            if (imgBarCode.ImageUrl != null)
            {
                
            }


            return Json(new
            {
                barcodeNo = barcodeNo,
                imageUrl = imgBarCode.ImageUrl,
            }, JsonRequestBehavior.AllowGet);
        }

       
        //GET GODOWN ADDRESS DDL 
        [HttpGet]
        public JsonResult GetGodownAddresses(string code)
        {
            var godownAddress = _godownaddressservice.GetAddressList(code);
            var addresslist = godownAddress.Select(m => new SelectListItem
            {
                Text = m.AreaName,
                Value = m.AreaName
            });
            return Json(addresslist, JsonRequestBehavior.AllowGet);
        }

        //GET GODOWN ADDRESS DETAILS
        [HttpGet]
        public JsonResult GetGodownDetails(string area)
        {
            var data = _godownaddressservice.GetAddressByArea(area);
            var godowndetail = _godownservice.GetGodownByCode(data.GdCode);
            return Json(new
            {
                data.Address,
                godowndetail.ContactNo1,
                godowndetail.EmpName,
                godowndetail.GodownEmail,
            }, JsonRequestBehavior.AllowGet);
        }

        //SAVE OPENING STOCK DETAILS
        [HttpPost]
        public ActionResult OpeningStock(MainApplication mainapp, FormCollection frmItem)
        {
            //CREATE OPENING STOCK CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string openingstockcode = string.Empty;

            var openingstockdata = _openingstockservice.GetLastStockByFinYr(FinYr);
            int openstockVal = 0;
            int length = 0;
            if (openingstockdata != null)
            {
                openingstockcode = openingstockdata.OpeningStockCode.Substring(3, 6);
                length = (Convert.ToInt32(openingstockcode) + 1).ToString().Length;
                openstockVal = Convert.ToInt32(openingstockcode) + 1;
            }
            else
            {
                openstockVal = 1;
                length = 1;
            }
            openingstockcode = _utilityservice.getName("OST", length, openstockVal);
            openingstockcode = openingstockcode + FinYr;
            mainapp.OpeningStockDetails.OpeningStockCode = openingstockcode;

            string ShopCode = frmItem["ShopDetails.ShopName"];
            string GodownCode = frmItem["GodownDetails.GodownName"];
            string ShopName = string.Empty;
            string GodownName = string.Empty;
            if (ShopCode != null)
            {
                ShopName = _ShopService.GetShopByCode(ShopCode).ShopName;
            }
            else
            {
                GodownName = _godownservice.GetGodownByCode(GodownCode).GodownName;
            }

            string hdncount = frmItem["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(hdncount))
            {
                int count = Convert.ToInt32(hdncount);
                for (int i = 1; i <= count; i++)
                {
                    string itemcode = "itemCode" + i;
                    string itemname = "itemName" + i;
                    string description = "Description" + i;
                    string designname = "itemDesignName" + i;
                    string designcode = "itemDesignCode" + i;
                    string size = "ItemSize" + i;
                    string color = "ItemColor" + i;
                    string itemquantity = "ItemQuantity" + i;
                    string unit = "ItemUnit" + i;
                    string numbertype = "ItemNumberType" + i;
                    string costprice = "ItemCostPrice" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "Rate" + i;
                    string material = "ItemMaterial" + i;
                    string brand = "ItemBrand" + i;
                    string itembarcode = "barcodeNo" + i;

                    if (!string.IsNullOrEmpty(frmItem[itemcode]))
                    {
                        mainapp.OpeningStockDetails.itemCode = frmItem[itemcode];
                        mainapp.OpeningStockDetails.ItemName = frmItem[itemname];
                        mainapp.OpeningStockDetails.Description = frmItem[description];
                        mainapp.OpeningStockDetails.DesignName = frmItem[designname];
                        mainapp.OpeningStockDetails.DesignCode = frmItem[designcode];
                        mainapp.OpeningStockDetails.Size = frmItem[size];
                        mainapp.OpeningStockDetails.Color = frmItem[color];
                        mainapp.OpeningStockDetails.ItemQuantity = Convert.ToDouble(frmItem[itemquantity]);
                        mainapp.OpeningStockDetails.TotalQuantity = Convert.ToDouble(frmItem[itemquantity]);
                        mainapp.OpeningStockDetails.Unit = frmItem[unit];
                        mainapp.OpeningStockDetails.NumberType = frmItem[numbertype];
                        mainapp.OpeningStockDetails.CostPrice = Convert.ToDouble(frmItem[costprice]);
                        mainapp.OpeningStockDetails.SellingPrice = Convert.ToDouble(frmItem[sellingprice]);
                        mainapp.OpeningStockDetails.MRP = Convert.ToDouble(frmItem[mrp]);
                        mainapp.OpeningStockDetails.Material = frmItem[material];
                        mainapp.OpeningStockDetails.Brand = frmItem[brand];
                        mainapp.OpeningStockDetails.Barcode = frmItem[itembarcode].ToString();
                        mainapp.OpeningStockDetails.Status = "Active";
                        mainapp.OpeningStockDetails.ModifiedOn = DateTime.Now;
                        
                        _openingstockservice.CreateStock(mainapp.OpeningStockDetails);

                        if (ShopName != "")
                        {
                            mainapp.ShopStockDetails = new ShopStock();
                            mainapp.ShopStockDetails.ShopCode = ShopCode;
                            mainapp.ShopStockDetails.ShopName = ShopName;
                            mainapp.ShopStockDetails.ItemName = frmItem[itemname];
                            mainapp.ShopStockDetails.ItemCode = frmItem[itemcode];
                            mainapp.ShopStockDetails.Barcode = frmItem[itembarcode] + ".png";
                            mainapp.ShopStockDetails.Description = frmItem[description];
                            mainapp.ShopStockDetails.Quantity = Convert.ToDouble(frmItem[itemquantity]);
                            mainapp.ShopStockDetails.Material = frmItem[material];
                            mainapp.ShopStockDetails.Color = frmItem[color];
                            mainapp.ShopStockDetails.DesignCode = frmItem[designcode];
                            mainapp.ShopStockDetails.DesignName = frmItem[designname];
                            mainapp.ShopStockDetails.Unit = frmItem[unit];
                            mainapp.ShopStockDetails.NumberType = frmItem[numbertype];
                            mainapp.ShopStockDetails.SellingPrice = Convert.ToDouble(frmItem[sellingprice]);
                            mainapp.ShopStockDetails.MRP = Convert.ToDouble(frmItem[mrp]);
                            mainapp.ShopStockDetails.Size = frmItem[size];
                            mainapp.ShopStockDetails.Brand = frmItem[brand];
                            mainapp.ShopStockDetails.Status = "Active";
                            mainapp.ShopStockDetails.ModifiedOn = DateTime.Now;

                            _ShopStockService.Create(mainapp.ShopStockDetails);
                        }
                        else
                        {
                            mainapp.GodownStockDetails = new GodownStock();
                            mainapp.GodownStockDetails.ItemName = frmItem[itemname];
                            mainapp.GodownStockDetails.ItemCode = frmItem[itemcode];
                            mainapp.GodownStockDetails.Barcode = frmItem[itembarcode] + ".png";
                            mainapp.GodownStockDetails.Description = frmItem[description];
                            mainapp.GodownStockDetails.Quantity = Convert.ToDouble(frmItem[itemquantity]);
                            mainapp.GodownStockDetails.Material = frmItem[material];
                            mainapp.GodownStockDetails.Color = frmItem[color];
                            mainapp.GodownStockDetails.DesignCode = frmItem[designcode];
                            mainapp.GodownStockDetails.DesignName = frmItem[designname];
                            mainapp.GodownStockDetails.Unit = frmItem[unit];
                            mainapp.GodownStockDetails.NumberType = frmItem[numbertype];
                            mainapp.GodownStockDetails.SellingPrice = Convert.ToDouble(frmItem[sellingprice]);
                            mainapp.GodownStockDetails.MRP = Convert.ToDouble(frmItem[mrp]);
                            mainapp.GodownStockDetails.Size = frmItem[size];
                            mainapp.GodownStockDetails.GodownName = GodownName;
                            mainapp.GodownStockDetails.GodownCode = GodownCode;
                            mainapp.GodownStockDetails.Brand = frmItem[brand];
                            mainapp.GodownStockDetails.Status = "Active";
                            mainapp.GodownStockDetails.ModifiedOn = DateTime.Now;

                            _GodownStockService.Create(mainapp.GodownStockDetails);
                        }

                        var itemdata = _itemservice.GetDescriptionByItemCode(frmItem[itemcode]);
                        itemdata.Barcode = mainapp.OpeningStockDetails.Barcode;
                        _itemservice.updateItem(itemdata);
                    }
                }
            }

            var OSIs = _openingstockservice.GetDetailsByStockCode(mainapp.OpeningStockDetails.OpeningStockCode).OpeningStockId;
            var OpenStkId = Encode(OSIs.ToString());
            return RedirectToAction("OpeningStockDetails/" + OpenStkId, "Stock");
        }

        //DISPLAY OPENING STOCK CREATE DETAILS AFTER CREATION
        [HttpGet]
        public ActionResult OpeningStockDetails(string id)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //display barcode images in details page
            int Id = Decode(id);
            model.OpeningStockDetails = _openingstockservice.GetById(Id);
            model.OpeningStockList = _openingstockservice.GetListByStockCode(model.OpeningStockDetails.OpeningStockCode);
            List<OpeningStockMaster> newlist = new List<OpeningStockMaster>();
            foreach (var data in model.OpeningStockList)
            {
                data.Barcode = "../../Images/" + data.Barcode;
                newlist.Add(data);
            }
            model.OpeningStockList = newlist;

            string previousopenstockno = TempData["PreviousOpenStockNo"].ToString();
            if (previousopenstockno != model.OpeningStockDetails.OpeningStockCode)
            {
                ViewData["OpenStockNoChanged"] = previousopenstockno + " is replaced with " + model.OpeningStockDetails.OpeningStockCode + " because " + previousopenstockno + " is acquired by another person";
            }
            TempData["PreviousOpenStockNo"] = previousopenstockno;
            return View(model);
        }

        //POPUP PAGE FOR PRINT BARCODE
        [HttpGet]
        public ActionResult PrintBarcode(int openstockid)
        {
            MainApplication model = new MainApplication();
            model.OpeningStockDetails = _openingstockservice.GetById(openstockid);
            //model.OpeningStockDetails.Barcode = "../../Images/" + model.OpeningStockDetails.Barcode;
            var barcodeno = _openingstockservice.GetById(openstockid).Barcode;
            barcodeno = barcodeno + ".png";
            model.BarcodeDetails = _BarcodeNumberService.GetBarcodeImageByBarcode(barcodeno);             
            return View(model);
        }

        [HttpGet]
        public ActionResult ShowAllQuantity(string code)
        {
            MainApplication model = new MainApplication();
            model.GodownStockList = _GodownStockService.GetRowsByItemCode(code);
            model.ShopStockList = _ShopStockService.GetRowsByItemCode(code);
            return View(model);
        }

        //CREATE ENTRY STOCK
        [HttpGet]
        public ActionResult EntryStock()
        {
            MainApplication model = new MainApplication()
            {
                EntryStockDetails = new EntryStockMaster(),
            };

            //CREATE ENTRY STOCK CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string entrystockcode = string.Empty;

            var enrtystockdata = _entrystockservice.GetLastStockByFinYr(FinYr);
            int entrystockvalue = 0;
            int len = 0;
            if (enrtystockdata != null)
            {
                entrystockcode = enrtystockdata.EntryStockCode.Substring(3, 6);
                len = (Convert.ToInt32(entrystockcode) + 1).ToString().Length;
                entrystockvalue = Convert.ToInt32(entrystockcode) + 1;

                entrystockvalue = enrtystockdata.EntryStockNo;
                entrystockvalue = entrystockvalue + 1;
                len = entrystockvalue.ToString().Length;
            }
            else
            {
                entrystockvalue = 1;
                len = 1;
            }
            entrystockcode = _utilityservice.getName("EST", len, entrystockvalue);
            entrystockcode = entrystockcode + FinYr;
            model.EntryStockDetails.EntryStockCode = entrystockcode;

            model.InwardFromSupplierList = _performainvoiceservice.GetDataByPINoAndStatus();
            return View(model);
        }

        //GET DETAILS BY INWARD NO DDL CHANGE EVENT
        [HttpGet]
        public JsonResult GetDetailsByPINo(string pino)
        {
            var data = _performainvoiceservice.GetDetailsByPINo(pino);
            string PoDate = Convert.ToDateTime(data.PoDate).ToShortDateString();
            string ReceivingDate = Convert.ToDateTime(data.ReceivingDate).ToShortDateString();
            return Json(new { data.SupplierName, PoDate, ReceivingDate }, JsonRequestBehavior.AllowGet);
        }

        //GET ITEM LIST FROM PERFORMA INVOICE
        [HttpGet]
        public ActionResult GetItemListFromPerformaInvoice(string pino)
        {
            MainApplication model = new MainApplication();
            model.InwardItemsFromSupplierList = _performainvoiceitemservice.GetInvItemsByInwardNo(pino);
            List<EntryStockItem> stocklist = new List<EntryStockItem>();
            //GET STOCK QUANTITY (FROM INWARD)
            foreach (var data in model.InwardItemsFromSupplierList)
            {
                EntryStockItem EntryStockItem = new EntryStockItem();
                EntryStockItem.Item = data.Item;
                EntryStockItem.Category = data.Category;
                EntryStockItem.SubCategory = data.SubCategory;
                EntryStockItem.ItemCode = data.itemCode;
                EntryStockItem.Description = data.Description;
                EntryStockItem.OrderedQuantity = data.OrderedQuantity;
                //received quantity will be actual received + extra
                EntryStockItem.ReceivedQuantity = data.ReceivedQuantity + data.ExtraQty;
                EntryStockItem.Unit = data.Unit;
                EntryStockItem.NumberType = data.NumberType;
                EntryStockItem.CostPrice = Convert.ToDouble(data.CostPrice);
                EntryStockItem.SellingPrice = Convert.ToDouble(data.SellingPrice);
                EntryStockItem.MRP = Convert.ToDouble(data.MRP);
                EntryStockItem.Material = data.Material;
                EntryStockItem.Color = data.Color;
                EntryStockItem.DesignCode = data.Design;
                EntryStockItem.DesignName = data.DesignName;
                EntryStockItem.Brand = data.Brand;
                EntryStockItem.Size = data.Size;
                EntryStockItem.Barcode = data.Barcode;

                var openingstockrow = _openingstockservice.GetTotalQuantityByItem(data.itemCode);
                var entrystockrow = _entrystockitemservice.getDetailsByItemCode(data.itemCode);

                if (entrystockrow != null)
                {
                    EntryStockItem.StockQuantity = entrystockrow.TotalQuantity;
                }
                else if (openingstockrow != null)
                {
                    EntryStockItem.StockQuantity = Convert.ToDouble(openingstockrow.ItemQuantity);
                }
                else
                {
                    EntryStockItem.StockQuantity = 0;
                }

                //CALCULATE TOTAL QUANTITY 
                EntryStockItem.TotalQuantity = EntryStockItem.StockQuantity + EntryStockItem.ReceivedQuantity;
                stocklist.Add(EntryStockItem);
            }
            model.EntryStockItemList = stocklist;

            TempData["StockItemList"] = stocklist;

            return View(model);
        }

        // SAVE DATA IN ENTRY MASTER
        [HttpPost]
        public ActionResult GetItemListFromPerformaInvoice(MainApplication mainapp, FormCollection frmcol)
        {
            //CREATE ENTRY STOCK CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string entrystkcode = string.Empty;

            var enrtystockdata = _entrystockservice.GetLastStockByFinYr(FinYr);
            int entrystockvalue = 0;
            int len = 0;
            if (enrtystockdata != null)
            {
                entrystkcode = enrtystockdata.EntryStockCode.Substring(3, 6);
                len = (Convert.ToInt32(entrystkcode) + 1).ToString().Length;
                entrystockvalue = Convert.ToInt32(entrystkcode) + 1;

                entrystockvalue = enrtystockdata.EntryStockNo;
                entrystockvalue = entrystockvalue + 1;
                len = entrystockvalue.ToString().Length;
            }
            else
            {
                entrystockvalue = 1;
                len = 1;
            }
            entrystkcode = _utilityservice.getName("EST", len, entrystockvalue);
            entrystkcode = entrystkcode + FinYr;
            mainapp.EntryStockDetails.EntryStockCode = entrystkcode;

            mainapp.EntryStockItemList = TempData["StockItemList"] as IEnumerable<EntryStockItem>;
            int count = 1;

            //save entry stock item entries
            foreach (var data in mainapp.EntryStockItemList)
            {
                string sellingprice = "SellingPrice" + count;
                string mrp = "MRP" + count;
                string percentage = "Percentage" + count;

                data.MRP = Convert.ToDouble(frmcol[mrp]);
                data.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                data.Percentage = frmcol[percentage];

                var entrystkdata = _entrystockitemservice.getDetailsByItemCode(data.ItemCode);
                if (entrystkdata != null)
                {
                    // var entrystockdata = _entrystockitemservice.getDetailsByItemCode(data.ItemCode);
                    entrystkdata.TotalQuantity = data.TotalQuantity;
                    _entrystockitemservice.Update(entrystkdata);
                }
                else
                {
                    //check entry stock master entries is already in master table
                    var entrystockcode = _entrystockservice.GetDataByStockCode(mainapp.EntryStockDetails.EntryStockCode);
                    if (entrystockcode == null)
                    {
                        //save entry stock master entries
                        mainapp.EntryStockDetails.FinalQuantity = Convert.ToDouble(frmcol["TotalQuantityValue"]);
                        mainapp.EntryStockDetails.FinalAmount = Convert.ToDouble(frmcol["TotalAmountValue"]);
                        mainapp.EntryStockDetails.Status = "Active";
                        mainapp.EntryStockDetails.ModifiedOn = DateTime.Now;
                        mainapp.EntryStockDetails.Date = DateTime.Now;
                        _entrystockservice.Create(mainapp.EntryStockDetails);
                    }

                    //save entry stock item entries
                    data.EntryStockCode = mainapp.EntryStockDetails.EntryStockCode;
                    data.Status = "Active";
                    data.ModifiedOn = DateTime.Now;
                    _entrystockitemservice.Create(data);

                    var openingstkdata = _openingstockservice.GetDataByItemCode(data.ItemCode);
                    if (openingstkdata != null)
                    {
                        //Opening stock status is inactive when entry save in entry stock
                        openingstkdata.Status = "InActive";
                        _openingstockservice.UpdateStock(openingstkdata);
                    }
                }

                //Inward status inactive when inward stored in Entry Stock
                var InwardRow = _performainvoiceservice.GetDetailsByPINo(mainapp.EntryStockDetails.PINo);
                InwardRow.Status = "InActive";
                _performainvoiceservice.UpdateInward(InwardRow);
                count = count + 1;
            }
            return RedirectToAction("Stock");
        }


        [HttpGet]
        public ActionResult PhysicalStockTaking()
        {
            MainApplication model = new MainApplication();
            model.PhysicalStockTakingDetails = new PhysicalStockTaking();

            var details = _PhysicalStockTakingService.GetLastRow();
            int catVal = 0;
            int length = 0;
            if (details != null)
            {
                catVal = Convert.ToInt32(details.PhysicalStockTakingCode.Substring(3));
                catVal = catVal + 1;
                length = catVal.ToString().Length;
            }
            else
            {
                catVal = 1;
                length = 1;
            }
            string code = _utilityservice.getName("PST", length, catVal);
            model.PhysicalStockTakingDetails.PhysicalStockTakingCode = code;
            TempData["stocktaking"] = code;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _godownservice.GetGodownNames();
            model.ShopList = _ShopService.GetAll();
            model.EntryStockItemList = _entrystockitemservice.GetAllItems();
            return View(model);
        }

        [HttpGet]
        public ActionResult GetStockTakingItems(string Code)
        {
            MainApplication model = new MainApplication();
            if (Code.Contains("SH"))
            {
                model.ShopStockList = _ShopStockService.GetRowsByShopCode(Code);
                TempData["shopstock"] = model.ShopStockList;
                TempData["type"] = "shop";
            }
            else
            {
                model.GodownStockList = _GodownStockService.GetRowsByGodownCode(Code);
                TempData["godownstock"] = model.GodownStockList;
                TempData["type"] = "godown";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetStockTakingItems(FormCollection form)
        {
            MainApplication model = new MainApplication();
            model.PhysicalStockTakingDetails = new PhysicalStockTaking();

            var detailss = _PhysicalStockTakingService.GetLastRow();
            int catVal = 0;
            int length = 0;
            if (detailss != null)
            {
                catVal = Convert.ToInt32(detailss.PhysicalStockTakingCode.Substring(3));
                catVal = catVal + 1;
                length = catVal.ToString().Length;
            }
            else
            {
                catVal = 1;
                length = 1;
            }
            string code = _utilityservice.getName("PST", length, catVal);
            model.PhysicalStockTakingDetails.PhysicalStockTakingCode = code;

            string type = form["temp"];
            if (type == "shop")
            {
                var ShopStock = TempData["shopstock"] as IEnumerable<ShopStock>;

                int count = 1;
                foreach (var row in ShopStock)
                {
                    var quantity = "Quantity" + count;
                    if (Convert.ToDouble(form[quantity]) != row.Quantity)
                    {
                        var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(row.ItemCode, row.ShopCode);
                        stockitemdetails.ItemQuantity = Convert.ToDouble(form[quantity]);
                        _StockItemDistributionService.Update(stockitemdetails);

                        var entrystockdetails = _entrystockitemservice.getDetailsByItemCode(row.ItemCode);
                        if (entrystockdetails != null)
                        {
                            if (Convert.ToDouble(form[quantity]) > row.Quantity)
                            {
                                entrystockdetails.TotalQuantity += Convert.ToDouble(form[quantity]) - row.Quantity;
                                _entrystockitemservice.Update(entrystockdetails);
                            }
                            else
                            {
                                entrystockdetails.TotalQuantity -= row.Quantity - Convert.ToDouble(form[quantity]);
                                _entrystockitemservice.Update(entrystockdetails);
                            }
                        }
                        else
                        {
                            var openingstockdetails = _openingstockservice.GetDetailsByItemCode(row.ItemCode);
                            if (openingstockdetails != null)
                            {
                                if (Convert.ToDouble(form[quantity]) > row.Quantity)
                                {
                                    openingstockdetails.TotalQuantity += Convert.ToDouble(form[quantity]) - row.Quantity;
                                    _openingstockservice.UpdateStock(openingstockdetails);
                                }
                                else
                                {
                                    openingstockdetails.TotalQuantity -= row.Quantity - Convert.ToDouble(form[quantity]);
                                    _openingstockservice.UpdateStock(openingstockdetails);
                                }
                            }
                        }

                        model.PhysicalStockTakingDetails.PhysicalStockTakingCode = form["PhysicalStockTakingDetails.PhysicalStockTakingCode"];
                        model.PhysicalStockTakingDetails.PreviousQuantity = Convert.ToDouble(row.Quantity);
                        model.PhysicalStockTakingDetails.CurrentQuantity = Convert.ToDouble(form[quantity]);
                        model.PhysicalStockTakingDetails.ItemCode = row.ItemCode;
                        model.PhysicalStockTakingDetails.ItemName = row.ItemName;
                        model.PhysicalStockTakingDetails.Code = row.ShopCode;
                        model.PhysicalStockTakingDetails.Name = row.ShopName;
                        model.PhysicalStockTakingDetails.Status = "Active";
                        model.PhysicalStockTakingDetails.ModifiedOn = DateTime.Now;
                        _PhysicalStockTakingService.Create(model.PhysicalStockTakingDetails);

                        row.Quantity = Convert.ToDouble(form[quantity]);
                        _ShopStockService.Update(row);

                    }
                    count++;
                }
            }
            else
            {
                var GodownStock = TempData["godownstock"] as IEnumerable<GodownStock>;

                int count = 1;
                foreach (var row in GodownStock)
                {
                    var quantity = "Quantity" + count;
                    if (Convert.ToDouble(form[quantity]) != row.Quantity)
                    {
                        var details = _StockItemDistributionService.GetDetailsByItemCodeAndGodownCode(row.ItemCode, row.GodownCode);
                        details.ItemQuantity = Convert.ToDouble(form[quantity]);
                        _StockItemDistributionService.Update(details);

                        var entrystockdetails = _entrystockitemservice.getDetailsByItemCode(row.ItemCode);
                        if (entrystockdetails != null)
                        {
                            if (Convert.ToDouble(form[quantity]) > row.Quantity)
                            {
                                entrystockdetails.TotalQuantity += Convert.ToDouble(form[quantity]) - row.Quantity;
                                _entrystockitemservice.Update(entrystockdetails);
                            }
                            else
                            {
                                entrystockdetails.TotalQuantity -= row.Quantity - Convert.ToDouble(form[quantity]);
                                _entrystockitemservice.Update(entrystockdetails);
                            }
                        }
                        else
                        {
                            var openingstockdetails = _openingstockservice.GetDetailsByItemCode(row.ItemCode);
                            if (openingstockdetails != null)
                            {
                                if (Convert.ToDouble(form[quantity]) > row.Quantity)
                                {
                                    openingstockdetails.TotalQuantity += Convert.ToDouble(form[quantity]) - row.Quantity;
                                    _openingstockservice.UpdateStock(openingstockdetails);
                                }
                                else
                                {
                                    openingstockdetails.TotalQuantity -= row.Quantity - Convert.ToDouble(form[quantity]);
                                    _openingstockservice.UpdateStock(openingstockdetails);
                                }
                            }
                        }

                        model.PhysicalStockTakingDetails.PhysicalStockTakingCode = form["PhysicalStockTakingDetails.PhysicalStockTakingCode"];
                        model.PhysicalStockTakingDetails.PreviousQuantity = Convert.ToDouble(row.Quantity);
                        model.PhysicalStockTakingDetails.CurrentQuantity = Convert.ToDouble(form[quantity]);
                        model.PhysicalStockTakingDetails.ItemCode = row.ItemCode;
                        model.PhysicalStockTakingDetails.ItemName = row.ItemName;
                        model.PhysicalStockTakingDetails.Code = row.GodownCode;
                        model.PhysicalStockTakingDetails.Name = row.GodownName;
                        model.PhysicalStockTakingDetails.Status = "Active";
                        model.PhysicalStockTakingDetails.ModifiedOn = DateTime.Now;
                        _PhysicalStockTakingService.Create(model.PhysicalStockTakingDetails);

                        row.Quantity = Convert.ToDouble(form[quantity]);
                        _GodownStockService.Update(row);
                    }
                    count++;
                }
            }

            string codes = model.PhysicalStockTakingDetails.PhysicalStockTakingCode;
            string Code = Encode(codes.ToString());
            return RedirectToAction("PhysicalStockTakingDetails", "Stock", new { Code = Code });
        }

        [HttpGet]
        public ActionResult PhysicalStockTakingDetails(string Code)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string code = decoded(Code);
            model.PhysicalStockTakingList = _PhysicalStockTakingService.GetRowsByCode(code.ToString());

            string stockcode = string.Empty;

            foreach (var data in model.PhysicalStockTakingList)
            {
                stockcode = data.PhysicalStockTakingCode;
                break;
            }

            string previousstktaking = TempData["stocktaking"].ToString();
            if (previousstktaking != stockcode)
            {
                ViewData["stocktakingchanged"] = previousstktaking + " is replaced with " + stockcode + " because " + previousstktaking + " is acquired by another person";
            }
            TempData["stocktaking"] = previousstktaking;

            return View(model);
        }

    }
}
