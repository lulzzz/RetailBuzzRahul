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
    public class OutwardController : Controller
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
        private readonly IOutwardToShopService _OutwardToShopService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IEmployeeMasterService _employeeservice;
        private readonly IGodownService _godownservice;
        private readonly IGodownAddressService _godownaddressservice;
        private readonly IEntryStockItemService _entrystockitemservice;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IShopService _ishopservice;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IOpeningStockService _openingstockservice;
        private readonly IStockDistributionService _StockDistributionService;
        private readonly IStockItemDistributionService _stockitemdistributionservice;
        private readonly IClientMasterService _clientmasterservice;
        private readonly IShopStockService _shopstockservice;
        private readonly IOutwardToClientService _outwardtoclientservice;
        private readonly IOutwardItemToClientService _outwarditemtoclientservice;
        private readonly IOutwardToShopService _outwardtoshopservice;
        private readonly IOutwardItemToShopService _outwarditemtoshopservice;
        private readonly IOutwardInterGodownService _OutwardInterGodownService;
        private readonly IOutwardItemInterGodownService _OutwardItemInterGodownService;
        private readonly IOutwardShopToGodownService _OutwardShopToGodownService;
        private readonly IOutwardItemShopToGodownService _OutwardItemShopToGodownService;
        private readonly IOutwardItemStockDistributionService _OutwardItemToGodownService;
        private readonly IOutwardStockDistributionService _OutwardToGodownService;
        private readonly IOutwardInterShopService _OutwardInterShopService;
        private readonly IOutwardItemInterShopservice _OutwardItemInterShopservice;

        public OutwardController(IUtilityService utilityservice, IOutwardToShopService outwardtoshopservice, IOutwardItemToShopService outwarditemtoshopservice, IEmployeeMasterService employeeservice, IGodownService godownservice, IGodownAddressService godownaddressservice, IEntryStockItemService entrystockitemservice,
            IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IShopService shopservice, IOpeningStockService openingstockservice, IStockItemDistributionService stockitemdistributionservice,
            IOutwardToClientService outwardtoclientservice, IOutwardItemToClientService outwarditemtoclientservice, IClientMasterService clientmasterservice, IShopStockService shopstockservice,
            IOutwardItemInterGodownService OutwardItemInterGodownService, IOutwardInterGodownService OutwardInterGodownService, IOutwardShopToGodownService OutwardShopToGodownService, IOutwardItemShopToGodownService OutwardItemShopToGodownService,
            IOutwardItemStockDistributionService OutwardItemToGodownService, IOutwardStockDistributionService OutwardToGodownService, IGodownStockService GodownStockService,
            IOutwardInterShopService OutwardInterShopService, IOutwardItemInterShopservice OutwardItemInterShopservice, IStockDistributionService StockDistributionService, IOutwardToShopService OutwardToShopService, IShopService ShopService)
        {
            this._OutwardToShopService = OutwardToShopService;
            this._OutwardToGodownService = OutwardToGodownService;
            this._OutwardItemToGodownService = OutwardItemToGodownService;
            this._StockDistributionService = StockDistributionService;
            this._OutwardItemShopToGodownService = OutwardItemShopToGodownService;
            this._OutwardShopToGodownService = OutwardShopToGodownService;
            this._OutwardInterGodownService = OutwardInterGodownService;
            this._OutwardItemInterGodownService = OutwardItemInterGodownService;
            this._utilityservice = utilityservice;
            this._outwardtoshopservice = outwardtoshopservice;
            this._outwarditemtoshopservice = outwarditemtoshopservice;
            this._employeeservice = employeeservice;
            this._godownservice = godownservice;
            this._godownaddressservice = godownaddressservice;
            this._entrystockitemservice = entrystockitemservice;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._ishopservice = shopservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._openingstockservice = openingstockservice;
            this._stockitemdistributionservice = stockitemdistributionservice;
            this._outwardtoclientservice = outwardtoclientservice;
            this._outwarditemtoclientservice = outwarditemtoclientservice;
            this._clientmasterservice = clientmasterservice;
            this._shopstockservice = shopstockservice;
            this._GodownStockService = GodownStockService;
            this._OutwardInterShopService = OutwardInterShopService;
            this._OutwardItemInterShopservice = OutwardItemInterShopservice;
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
        public JsonResult EncodeOutwardId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        //************************************** OUTWARD FROM GODOWN TO SHOP ************************************************

        //CREATE OUTWARD FROM GODOWN TO SHOP
        [HttpGet]
        public ActionResult OutwardToShop()
        {
            MainApplication model = new MainApplication()
            {
                OutwardToShopDetails = new OutwardToShop(),
            };

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
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

            //CREATE OUTWARD CODE
            string gdcode = Session["LOGINSHOPGODOWNCODEOWGS"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastOutward = _outwardtoshopservice.GetLastOutwardByFinYr(FinYr, gdcode);
            string OutwardCode = string.Empty;
            int OutwardNoLength = 0;
            int OutwardNo = 0;
            if (LastOutward == null)
            {
                OutwardNoLength = 1;
                OutwardNo = 1;
            }
            else
            {
                string trim = LastOutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                OutwardCode = LastOutward.OutwardCode.Substring(index + 4, 6);
                OutwardNoLength = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                OutwardNo = Convert.ToInt32(OutwardCode) + 1;
            }
            string godownname = Session["SHOPGODOWNNAMEOWGS"] as string;
            var gddetails = _godownservice.GetGodownDetailsByName(godownname);
            string utilityname = gddetails.ShortCode + "/OWGS";
            OutwardCode = _utilityservice.getName(utilityname, OutwardNoLength, OutwardNo);
            OutwardCode = OutwardCode + FinYr;
            model.OutwardToShopDetails.OutwardCode = OutwardCode;
            TempData["PreviousShopOutward"] = OutwardCode;

            model.ShopList = _ishopservice.GetAll();
            model.EmpList = _employeeservice.getAllemployees();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardShopToGodown()
        {
            MainApplication model = new MainApplication()
            {
                OutwardShopToGodownDetails = new OutwardShopToGodown(),
            };

            model.GodownMasterList = _godownservice.GetGodownNames();
            model.EmpList = _employeeservice.getAllemployees();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            //CREATE OUTWARD CODE
            string shcode = Session["LOGINSHOPGODOWNCODEOWSG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastoutward = _OutwardShopToGodownService.GetLastOutwardByFinYear(finyr, shcode);
            string outwardcode = string.Empty;
            int outwardVal = 0;
            int length = 0;
            if (lastoutward != null)
            {
                string trim = lastoutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                outwardcode = lastoutward.OutwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(outwardcode) + 1).ToString().Length;
                outwardVal = Convert.ToInt32(outwardcode) + 1;
            }
            else
            {
                outwardVal = 1;
                length = 1;
            }
            string shopname = Session["SHOPGODOWNNAMEOWSG"] as string;
            var shopdetails = _ishopservice.GetShopDetailsByName(shopname);
            var utilityname = shopdetails.ShortCode + "/OWSG";
            outwardcode = _utilityservice.getName(utilityname, length, outwardVal);
            outwardcode = outwardcode + finyr;
            model.OutwardShopToGodownDetails.OutwardCode = outwardcode;
            TempData["outwardcode"] = outwardcode;

            return View(model);
        }

        [HttpPost]
        public ActionResult OutwardShopToGodown(MainApplication mainapp, FormCollection form)
        {
            mainapp.OutwardStkDisDetails = new OutwardStockDistribution();
            mainapp.OutwardItemStkDisDetails = new OutwardItemStockDistribution();
            mainapp.OutwardItemShopToGodownDetails = new OutwardItemShopToGodown();

            string shcode = Session["LOGINSHOPGODOWNCODEOWSG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastoutward = _OutwardShopToGodownService.GetLastOutwardByFinYear(finyr, shcode);
            string outwardcode = string.Empty;
            int outwardVal = 0;
            int length = 0;
            if (lastoutward != null)
            {
                string trim = lastoutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                outwardcode = lastoutward.OutwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(outwardcode) + 1).ToString().Length;
                outwardVal = Convert.ToInt32(outwardcode) + 1;
            }
            else
            {
                outwardVal = 1;
                length = 1;
            }
            string shopname = Session["SHOPGODOWNNAMEOWSG"] as string;
            var shopdetails = _ishopservice.GetShopDetailsByName(shopname);

            var utilityname = shopdetails.ShortCode + "/OWSG";
            outwardcode = _utilityservice.getName(utilityname, length, outwardVal);
            outwardcode = outwardcode + finyr;

            mainapp.OutwardShopToGodownDetails.OutwardCode = outwardcode;
            mainapp.OutwardShopToGodownDetails.DebitNoteNo = "No";
            mainapp.OutwardShopToGodownDetails.StockReturn = "No";
            mainapp.OutwardShopToGodownDetails.ModifiedOn = DateTime.Now;
            mainapp.OutwardShopToGodownDetails.Status = "Active";
            mainapp.OutwardShopToGodownDetails.Date = DateTime.Now;
            _OutwardShopToGodownService.Create(mainapp.OutwardShopToGodownDetails);

            var count = Convert.ToInt32(form["rowcount"]);
            for (int i = 1; i <= count; i++)
            {
                var itemcode = "ItemCode" + i;
                var itemname = "ItemName" + i;
                var barcode = "Barcode" + i;
                var descriptionvalue = "DescriptionValue" + i;
                var designvalue = "DesignValue" + i;
                var colorvalue = "ColorValue" + i;
                var unitvalue = "UnitValue" + i;
                var rate = "Rate" + i;
                var mrp = "MRP" + i;
                var material = "Material" + i;
                var brand = "Brand" + i;
                var size = "Size" + i;
                var numbertype = "NumberType" + i;
                var designcode = "DesignCode" + i;
                var outwardquantity = "OutwardQuantity" + i;
                var StockQty = "StockQuantityValue" + i;
                var balancevalue = "BalanceValue" + i;


                if (form[outwardquantity] != "" && Convert.ToInt32(form[outwardquantity]) != 0)
                {
                    mainapp.OutwardItemShopToGodownDetails.Barcode = form[barcode];
                    mainapp.OutwardItemShopToGodownDetails.ItemCode = form[itemcode];
                    mainapp.OutwardItemShopToGodownDetails.ItemDescription = form[descriptionvalue];
                    mainapp.OutwardItemShopToGodownDetails.DesignName = form[designvalue];
                    mainapp.OutwardItemShopToGodownDetails.Unit = form[unitvalue];
                    mainapp.OutwardItemShopToGodownDetails.Color = form[colorvalue];
                    mainapp.OutwardItemShopToGodownDetails.DesignCode = form[designcode];
                    mainapp.OutwardItemShopToGodownDetails.Brand = form[brand];
                    mainapp.OutwardItemShopToGodownDetails.NumberType = form[numbertype];
                    mainapp.OutwardItemShopToGodownDetails.Size = form[size];
                    mainapp.OutwardItemShopToGodownDetails.ItemName = form[itemname];
                    mainapp.OutwardItemShopToGodownDetails.ModifiedOn = DateTime.Now;
                    mainapp.OutwardItemShopToGodownDetails.Status = "Active";
                    mainapp.OutwardItemShopToGodownDetails.OutwardCode = outwardcode;
                    mainapp.OutwardItemShopToGodownDetails.OutwardQuantity = Convert.ToDouble(form[outwardquantity]);
                    mainapp.OutwardItemShopToGodownDetails.ShopCode = mainapp.OutwardShopToGodownDetails.ShopCode;
                    mainapp.OutwardItemShopToGodownDetails.GodownCode = mainapp.OutwardShopToGodownDetails.GodownCode;
                    mainapp.OutwardItemShopToGodownDetails.SellingPrice = Convert.ToDouble(form[rate]);
                    mainapp.OutwardItemShopToGodownDetails.MRP = Convert.ToDouble(form[mrp]);
                    mainapp.OutwardItemShopToGodownDetails.Material = form[material];
                    mainapp.OutwardItemShopToGodownDetails.StockQuantity = Convert.ToDouble(form[StockQty]);
                    mainapp.OutwardItemShopToGodownDetails.CurrentBalance = Convert.ToDouble(form[balancevalue]);
                    _OutwardItemShopToGodownService.Create(mainapp.OutwardItemShopToGodownDetails);
                }

                var stkdistitemforshop = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(form[itemcode], mainapp.OutwardShopToGodownDetails.ShopCode);
                var shopstockitem = _shopstockservice.GetDetailsByItemCodeAndShopCode(form[itemcode], mainapp.OutwardShopToGodownDetails.ShopCode);

                stkdistitemforshop.ItemQuantity -= Convert.ToDouble(form[outwardquantity]);
                stkdistitemforshop.ModifiedOn = DateTime.Now;
                _stockitemdistributionservice.Update(stkdistitemforshop);

                shopstockitem.Quantity -= Convert.ToDouble(form[outwardquantity]);
                shopstockitem.ModifiedOn = DateTime.Now;
                _shopstockservice.Update(shopstockitem);
            }
            var id = _OutwardShopToGodownService.GetLastRow();
            string Id = Encode(id.OutwardId.ToString());
            return RedirectToAction("OutwardShopToGodownDetails/" + Id, "Outward");
        }




        [HttpGet]
        public ActionResult OutwardShopToGodownDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.OutwardShopToGodownDetails = _OutwardShopToGodownService.GetDetailsById(Id);
            model.OutwardItemShopToGodownList = _OutwardItemShopToGodownService.GetItemsByOutwardNo(model.OutwardShopToGodownDetails.OutwardCode);

            string previousoutward = TempData["outwardcode"].ToString();
            if (previousoutward != model.OutwardShopToGodownDetails.OutwardCode)
            {
                ViewData["outwardchanged"] = previousoutward + " is replaced with " + model.OutwardShopToGodownDetails.OutwardCode + " because " + previousoutward + " is acquired by another person";
            }
            TempData["outwardcode"] = previousoutward;

            return View(model);
        }

        [HttpGet]
        public ActionResult PrintOutwardShopToGodown(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.OutwardShopToGodownDetails = _OutwardShopToGodownService.GetDetailsById(Id);
            model.OutwardItemShopToGodownList = _OutwardItemShopToGodownService.GetItemsByOutwardNo(model.OutwardShopToGodownDetails.OutwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardShopToGodownPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();

            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNumbersShopToGodown(string term)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODEOWSG"] as string;
            var result = _OutwardShopToGodownService.GetOutwardNoForPrint(term, shopcode);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOutwardShopToGodownDetailsForPrint(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardShopToGodownDetails = _OutwardShopToGodownService.GetDetailsByOutwardNo(OutwardNo);
            model.OutwardItemShopToGodownList = _OutwardItemShopToGodownService.GetItemsByOutwardNo(OutwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardShopToGodownDelete()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetOutwardShopToGodownDetailsForDelete(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardShopToGodownDetails = _OutwardShopToGodownService.GetDetailsByOutwardNo(OutwardNo);
            TempData["OutwardDetails"] = model.OutwardShopToGodownDetails;
            model.OutwardItemShopToGodownList = _OutwardItemShopToGodownService.GetItemsByOutwardNo(OutwardNo);
            TempData["OutwardItems"] = model.OutwardItemShopToGodownList;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOutwardShopToGodownDetailsForDelete(MainApplication model)
        {
            var outwarddata = TempData["OutwardDetails"] as OutwardShopToGodown;
            var itemlist = TempData["OutwardItems"] as IEnumerable<OutwardItemShopToGodown>;
            string shopcode = Session["LOGINSHOPGODOWNCODEOWSG"] as string;

            foreach (var data in itemlist)
            {
                var shopstock = _shopstockservice.GetDetailsByItemCodeAndShopCode(data.ItemCode, shopcode);
                shopstock.Quantity = shopstock.Quantity + data.OutwardQuantity;
                _shopstockservice.Update(shopstock);

                var stockdist = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(data.ItemCode, shopcode);
                stockdist.ItemQuantity = stockdist.ItemQuantity + data.OutwardQuantity;
                _stockitemdistributionservice.Update(stockdist);

                if (data != null)
                {
                    _OutwardItemShopToGodownService.Delete(data);
                }
            }

            _OutwardShopToGodownService.Delete(outwarddata);
            return RedirectToAction("OutwardShopToGodownDelete");
        }

        [HttpGet]
        public JsonResult GetGodownDetailsByGdCode(string Code)
        {
            var details = _godownservice.GetGodownByCode(Code);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        //GET GODOWN ADDRESS DDL 
        [HttpGet]
        public JsonResult GetGodownAddresses(string code)
        {
            string gdname = _godownservice.GetGodownByCode(code).GodownName;
            var godownAddress = _godownaddressservice.GetAddressList(code);
            var addresslist = godownAddress.Select(m => new SelectListItem
            {
                Text = m.AreaName,
                Value = m.AreaName
            });
            return Json(new { gdname, addresslist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetShopAddresses(string code)
        {
            string gdname = _ishopservice.GetShopByCode(code).ShopName;
            string codevalue = code;
            var godownAddress = _ishopservice.GetAddressList(code);
            var addresslist = godownAddress.Select(m => new SelectListItem
            {
                Text = m.ShopAddress,
                Value = m.ShopAddress
            });
            return Json(new { gdname, codevalue, addresslist }, JsonRequestBehavior.AllowGet);
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
                godowndetail.GodownName,
            }, JsonRequestBehavior.AllowGet);
        }

        //GET Shop ADDRESS DETAILS
        [HttpGet]
        public JsonResult GetShopDetailsvalue(string area)
        {
            var data = _ishopservice.GetAddressByArea(area);
            var detail = _ishopservice.GetShopByCode(data.ShopCode);
            return Json(new
            {
                data.ShopAddress,
                detail.ShopContactNo,
                detail.EmpName,


                detail.ShopEmail,
                detail.ShopName,
            }, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE SALESMAN NAME
        [HttpGet]
        public JsonResult GetSalesman(string term)
        {
            var EmpData = _employeeservice.GetEmpByNameAndSalesDepartment(term);
            List<string> names = new List<string>();
            foreach (var item in EmpData)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStoreKeepers(string term)
        {
            var EmpData = _employeeservice.GetShopEmp(term);
            List<string> names = new List<string>();
            foreach (var item in EmpData)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE EMPLOYEE NAME
        [HttpGet]
        public JsonResult GetEmployee(string term)
        {
            var EmpData = _employeeservice.GetEmployeeByDepartment(term);
            List<string> names = new List<string>();
            foreach (var item in EmpData)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET ITEM LIST FROM STOCK DISTRIBUTION
        [HttpGet]
        public JsonResult GetItemListFromStockDistribution(string GodownCode)
        {
            var itemlist = _GodownStockService.GetRowsByGodownCode(GodownCode);
            var items = itemlist.Select(m => new SelectListItem
            {
                Text = m.ItemName,
                Value = m.ItemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemsFromShopStock(string ShopCode)
        {
            var details = _shopstockservice.GetRowsByShopCode(ShopCode);
            var itemlist = details.Select(m => new SelectListItem
            {
                Value = m.ItemCode,
                Text = m.ItemName + " " + m.ItemCode
            });
            return Json(itemlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemDetailsFromShopStock(string ItemCode, string ShopCode)
        {
            var details = _shopstockservice.GetDetailsByItemCodeAndShopCode(ItemCode, ShopCode);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        //GET STOCK DISTRIBUTION ITEM DETAILS BY ITEM CODE
        [HttpGet]
        public JsonResult GetStockDistributionItemdetails(string ItemCode, string GodownCode)
        {
            var items = _GodownStockService.GetDetailsByItemCodeAndGodownCode(ItemCode, GodownCode);
            string barcode = _openingstockservice.GetDetailsByItemCode(ItemCode).Barcode;
            return Json(new { items.ItemName, ItemDescription = items.Description, items.Quantity, barcode, items.Color, items.Material, items.DesignName, items.DesignCode, items.Unit, items.SellingPrice, items.MRP, items.Size, items.Brand, items.ItemCode, items.NumberType }, JsonRequestBehavior.AllowGet);
        }

        //GET EMPLOPYEE DETAILS BY EMPLOYEE NAME
        [HttpGet]
        public JsonResult GetEmployeeDetails(string name)
        {
            var data = _employeeservice.getEmpByName(name);
            return Json(new { data.MobileNo, data.Designation, data.Email }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStoreKeeperDetails(string name)
        {
            var data = _employeeservice.getEmpByName(name);
            return Json(new { data.MobileNo, data.Designation, data.Email }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalesmanDetails(string name)
        {
            var data = _employeeservice.GetEmpRowBySalesDesignation(name);
            return Json(new { data.MobileNo, data.Designation, data.Email }, JsonRequestBehavior.AllowGet);
        }

        //GET SHOP DETAILS BY SHOP NAME
        [HttpGet]
        public JsonResult GetShopDetails(string code)
        {
            var data = _ishopservice.GetShopByCode(code);
            return Json(new { data.ShopCode, data.ShopAddress, data.ShopEmail, data.ShopContactNo, data.ShopName, data.EmpName }, JsonRequestBehavior.AllowGet);
        }

        //SAVE DETAILS IN OUTWARD FROM GODOWN TO SHOP MASTER
        [HttpPost]
        public ActionResult OutwardToShop(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                OutwardItemToShopDetails = new OutwardItemToShop(),
            };

            string gdcode = Session["LOGINSHOPGODOWNCODEOWGS"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastOutward = _outwardtoshopservice.GetLastOutwardByFinYr(FinYr, gdcode);
            string OutwardCode = string.Empty;
            int OutwardNoLength = 0;
            int OutwardNo = 0;
            if (LastOutward == null)
            {
                OutwardNoLength = 1;
                OutwardNo = 1;
            }
            else
            {
                string trim = LastOutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                OutwardCode = LastOutward.OutwardCode.Substring(index + 4, 6);
                OutwardNoLength = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                OutwardNo = Convert.ToInt32(OutwardCode) + 1;
            }
            string godownname = Session["SHOPGODOWNNAMEOWGS"] as string;
            var gddetails = _godownservice.GetGodownDetailsByName(godownname);
            string utilityname = gddetails.ShortCode + "/OWGS";
            OutwardCode = _utilityservice.getName(utilityname, OutwardNoLength, OutwardNo);
            OutwardCode = OutwardCode + FinYr;

            string godowncode = Session["LOGINSHOPGODOWNCODEOWGS"].ToString();
            //SAVE OUTWARD TO GODOWN 
            mainapp.OutwardToShopDetails.OutwardCode = OutwardCode;
            mainapp.OutwardToShopDetails.Date = DateTime.Now;
            mainapp.OutwardToShopDetails.Status = "Active";
            mainapp.OutwardToShopDetails.ModifiedOn = DateTime.Now;
            mainapp.OutwardToShopDetails.GodownCode = godowncode;
            mainapp.OutwardToShopDetails.GodownName = Session["SHOPGODOWNNAMEOWGS"].ToString();
            _outwardtoshopservice.Create(mainapp.OutwardToShopDetails);

            string ItemCount = frmcol["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(ItemCount))
            {
                int count = Convert.ToInt32(ItemCount);
                for (int i = 1; i <= count; i++)
                {
                    //SAVE OUTWARD TO GODOWN ITEMS
                    string itemcodes = "ItemCode" + i;
                    string itemnames = "ItemName" + i;
                    string descriptionvalue = "DescriptionValue" + i;
                    string stockquantityvalue = "StockQuantityValue" + i;
                    string outwardquantity = "OutwardQuantity" + i;
                    string balancevalue = "BalanceValue" + i;
                    string barcode = "Barcode" + i;
                    string colorvalue = "ColorValue" + i;
                    string material = "Material" + i;
                    string rate = "Rate" + i;
                    string mrp = "MRP" + i;
                    string brand = "Brand" + i;
                    string numbertype = "NumberType" + i;
                    string designcode = "DesignCode" + i;
                    string designvalue = "DesignValue" + i;
                    string unitvalue = "UnitValue" + i;
                    string size = "Size" + i;

                    if (frmcol[itemcodes] != null)
                    {
                        model.OutwardItemToShopDetails.ItemCode = frmcol[itemcodes];

                        model.OutwardItemToShopDetails.Brand = frmcol[brand];
                        model.OutwardItemToShopDetails.Size = frmcol[size];
                        model.OutwardItemToShopDetails.NumberType = frmcol[numbertype];
                        model.OutwardItemToShopDetails.DesignCode = frmcol[designcode];
                        model.OutwardItemToShopDetails.ItemName = frmcol[itemnames];
                        model.OutwardItemToShopDetails.ItemDescription = frmcol[descriptionvalue];
                        model.OutwardItemToShopDetails.StockQuantity = Convert.ToDouble(frmcol[stockquantityvalue]);
                        model.OutwardItemToShopDetails.OutwardQuantity = Convert.ToDouble(frmcol[outwardquantity]);
                        model.OutwardItemToShopDetails.GodownCode = godowncode;
                        model.OutwardItemToShopDetails.ShopCode = mainapp.OutwardToShopDetails.ShopCode;
                        model.OutwardItemToShopDetails.CurrentBalance = frmcol[balancevalue];
                        model.OutwardItemToShopDetails.OutwardCode = OutwardCode;
                        model.OutwardItemToShopDetails.Status = "Active";
                        model.OutwardItemToShopDetails.ModifiedOn = DateTime.Now;
                        model.OutwardItemToShopDetails.Barcode = frmcol[barcode];
                        model.OutwardItemToShopDetails.Color = frmcol[colorvalue];
                        model.OutwardItemToShopDetails.Material = frmcol[material];
                        model.OutwardItemToShopDetails.DesignName = frmcol[designvalue];
                        model.OutwardItemToShopDetails.SellingPrice = Convert.ToDouble(frmcol[rate]);
                        model.OutwardItemToShopDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        model.OutwardItemToShopDetails.Unit = frmcol[unitvalue];
                        _outwarditemtoshopservice.Create(model.OutwardItemToShopDetails);

                        model.StockItemDistributionDetails = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(model.OutwardItemToShopDetails.ItemCode, godowncode);

                        model.StockItemDistributionDetails.ItemQuantity = model.StockItemDistributionDetails.ItemQuantity - Convert.ToDouble(model.OutwardItemToShopDetails.OutwardQuantity);
                        _stockitemdistributionservice.Update(model.StockItemDistributionDetails);

                        var godownstockdata = _GodownStockService.GetDetailsByItemCodeAndGodownCode(model.OutwardItemToShopDetails.ItemCode, godowncode);

                        godownstockdata.Quantity -= Convert.ToDouble(model.OutwardItemToShopDetails.OutwardQuantity);
                        _GodownStockService.Update(godownstockdata);
                    }

                }
            }
            var godowntoshopdetails = _outwardtoshopservice.GetLastRow();
            var OWTSId = Encode(godowntoshopdetails.OutwardId.ToString());
            return RedirectToAction("OutwardToShopDetails/" + OWTSId, "Outward");
        }

        // DETAILS PAGE OF OUTWARD FROM GODOWN TO SHOP 
        [HttpGet]
        public ActionResult OutwardToShopDetails(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.OutwardToShopDetails = _outwardtoshopservice.GetDetailsbyId(Id);
            model.OutwardItemToShopList = _outwarditemtoshopservice.GetDetailsByOutwardNo(model.OutwardToShopDetails.OutwardCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string previousshopoutward = TempData["PreviousShopOutward"].ToString();
            if (TempData["PreviousShopOutward"].ToString() != model.OutwardToShopDetails.OutwardCode)
            {
                ViewData["ShopOutwardNoChanged"] = previousshopoutward + " is replaced with " + model.OutwardToShopDetails.OutwardCode + " because " + previousshopoutward + " is acquired by another person";
            }
            TempData["PreviousShopOutward"] = previousshopoutward;
            return View(model);
        }

        // PRINT PAGE OF OUTWARD FROM GODOWN TO SHOP 
        [HttpGet]
        public ActionResult PrintOutwardToShop(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication()
            {
                OutwardToShopDetails = new OutwardToShop(),
                OutwardItemToShopDetails = new OutwardItemToShop()
            };
            model.OutwardToShopDetails = _outwardtoshopservice.GetDetailsbyId(Id);
            model.OutwardItemToShopList = _outwarditemtoshopservice.GetDetailsByOutwardNo(model.OutwardToShopDetails.OutwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardToShopEdit()
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
        public ActionResult OutwardToShopEditDetails(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
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
            model.OutwardToShopDetails = _outwardtoshopservice.GetDetailsByOutwardCode(OutwardNo);
            TempData["OutwardToShopList"] = model.OutwardToShopDetails;
            model.OutwardItemToShopList = _outwarditemtoshopservice.GetDetailsByOutwardNo(OutwardNo);
            TempData["OutwardItemToShopList"] = model.OutwardItemToShopList;
            return View(model);
        }

        [HttpPost]
        public ActionResult OutwardToShopEditDetails(MainApplication model, FormCollection form)
        {
            model.OutwardToShopDetails = TempData["OutwardToShopList"] as OutwardToShop;
            model.OutwardToShopDetails.ModifiedOn = DateTime.Now;
            _outwardtoshopservice.Udpate(model.OutwardToShopDetails);

            var godowncode = Session["LOGINSHOPGODOWNCODE"].ToString();

            var data = TempData["OutwardItemToShopList"] as IEnumerable<OutwardItemToShop>;
            int count = 1;

            foreach (var item in data)
            {
                double? remainingQty = 0;
                string minusadd = string.Empty;
                string outwardquantity = "OutwardQuantity" + count;
                string balancevalue = "BalanceValue" + count;
                string qty = form[outwardquantity];
                if (qty == "")
                {
                    item.OutwardQuantity = 0;
                }
                else
                {
                    if (item.OutwardQuantity > Convert.ToDouble(qty))
                    {
                        remainingQty = item.OutwardQuantity - Convert.ToDouble(qty);
                        minusadd = "minus";
                    }
                    else if (item.OutwardQuantity < Convert.ToDouble(qty))
                    {
                        remainingQty = Convert.ToDouble(qty) - item.OutwardQuantity;
                        minusadd = "add";
                    }
                    item.OutwardQuantity = Convert.ToDouble(form[outwardquantity]);
                }
                item.CurrentBalance = form[balancevalue];
                item.ModifiedOn = DateTime.Now;

                var stockdistitems = _stockitemdistributionservice.GetLastRowFromItemAndGodownCode(item.ItemCode, godowncode);
                var entrystockdata = _entrystockitemservice.getDetailsByItemCode(item.ItemCode);
                var openingstockdata = _openingstockservice.GetDetailsByItemCode(item.ItemCode);

                if (minusadd == "add")
                {
                    stockdistitems.ItemQuantity = stockdistitems.ItemQuantity + Convert.ToDouble(remainingQty);
                    _stockitemdistributionservice.Update(stockdistitems);

                    if (entrystockdata != null)
                    {
                        entrystockdata.TotalQuantity = entrystockdata.TotalQuantity + Convert.ToDouble(remainingQty);
                        _entrystockitemservice.Update(entrystockdata);
                    }
                    else
                    {
                        openingstockdata.TotalQuantity = openingstockdata.TotalQuantity + Convert.ToDouble(remainingQty); ;
                        _openingstockservice.UpdateStock(openingstockdata);
                    }
                }
                else if (minusadd == "minus")
                {
                    stockdistitems.ItemQuantity = stockdistitems.ItemQuantity - Convert.ToDouble(remainingQty);
                    _stockitemdistributionservice.Update(stockdistitems);

                    if (entrystockdata != null)
                    {
                        entrystockdata.TotalQuantity = entrystockdata.TotalQuantity - Convert.ToDouble(remainingQty);
                        _entrystockitemservice.Update(entrystockdata);
                    }
                    else
                    {
                        openingstockdata.TotalQuantity = openingstockdata.TotalQuantity - Convert.ToDouble(remainingQty);
                        _openingstockservice.UpdateStock(openingstockdata);
                    }
                }
                _outwarditemtoshopservice.Update(item);
                count++;
            }
            string OutwardId = Encode(model.OutwardToShopDetails.OutwardId.ToString());
            return RedirectToAction("OutwardToShopDetails/" + OutwardId, "Outward");
        }

        [HttpGet]
        public ActionResult OutwardToShopPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
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
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNumbers(string term)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEOWGS"] as string;
            var result = _OutwardToShopService.GetOutwardNoForPrint(term, godowncode);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetOutwardToShopDetailsForPrint(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardToShopDetails = _outwardtoshopservice.GetDetailsByOutwardCode(OutwardNo);
            model.OutwardItemToShopList = _outwarditemtoshopservice.GetDetailsByOutwardNo(OutwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardToShopDelete()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
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
            return View(model);
        }

        [HttpGet]
        public ActionResult GetOutwardToShopDetailsForDelete(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardToShopDetails = _outwardtoshopservice.GetDetailsByOutwardCode(OutwardNo);
            TempData["OutwardToShop"] = model.OutwardToShopDetails;
            model.OutwardItemToShopList = _outwarditemtoshopservice.GetDetailsByOutwardNo(OutwardNo);
            TempData["OutwardToShopItems"] = model.OutwardItemToShopList;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOutwardToShopDetailsForDelete(MainApplication model)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEOWGS"] as string;
            var outwarddata = TempData["OutwardToShop"] as OutwardToShop;
            var outwarditems = TempData["OutwardToShopItems"] as IEnumerable<OutwardItemToShop>;

            foreach (var data in outwarditems)
            {
                var godownstock = _GodownStockService.GetDetailsByItemCodeAndGodownCode(data.ItemCode, godowncode);
                godownstock.Quantity = godownstock.Quantity + data.OutwardQuantity;
                _GodownStockService.Update(godownstock);

                var stkitemdist = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(data.ItemCode, godowncode);
                stkitemdist.ItemQuantity = stkitemdist.ItemQuantity + data.OutwardQuantity;
                _stockitemdistributionservice.Update(stkitemdist);

                var tempdata = _outwarditemtoshopservice.GetDetailsByOutwardCode(data.OutwardCode);
                _outwarditemtoshopservice.Delete(tempdata);
            }

            var details = _outwardtoshopservice.GetDetailsByOutwardCode(outwarddata.OutwardCode);
            _outwardtoshopservice.Delete(details);

            return RedirectToAction("OutwardToShopDelete");
        }

        //******************************************** OUTWARD TO CLIENT *************************************************************

        // CREATE OUTWARD TO CLIENT
        [HttpGet]
        public ActionResult OutwardToClient()
        {
            MainApplication model = new MainApplication()
            {
                OutwardToClientDetails = new OutwardToClient(),
            };

            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWC"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWC"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWC"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWC"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            //CREATE OUTWARD CODE
            string code = Session["LOGINSHOPGODOWNCODEOWC"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastOutward = _outwardtoclientservice.GetLastOutwardByFinYr(FinYr, code);
            string OutwardCode = string.Empty;
            int OutwardNoLength = 0;
            int OutwardNo = 0;
            if (LastOutward == null)
            {
                OutwardNoLength = 1;
                OutwardNo = 1;
            }
            else
            {
                string trim = LastOutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                OutwardCode = LastOutward.OutwardCode.Substring(index + 3, 6);
                OutwardNoLength = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                OutwardNo = Convert.ToInt32(OutwardCode) + 1;
            }
            string name = Session["SHOPGODOWNNAMEOWC"] as string;
            string codes = Session["LOGINSHOPGODOWNCODEOWC"] as string;
            string utilityname = string.Empty;
            if (code.Contains("SH"))
            {
                var details = _ishopservice.GetShopDetailsByName(name);
                utilityname = details.ShortCode + "/OWC";
            }
            else
            {
                var details = _godownservice.GetGodownDetailsByName(name);
                utilityname = details.ShortCode + "/OWC";
            }
            OutwardCode = _utilityservice.getName(utilityname, OutwardNoLength, OutwardNo);
            OutwardCode = OutwardCode + FinYr;
            model.OutwardToClientDetails.OutwardCode = OutwardCode;
            TempData["PreviousClientOutward"] = OutwardCode;

            //CONCAT GODOWNCODE AND GODOWNNAME
            var godownlist = _godownservice.GetGodownNames();
            string godown = string.Empty;
            List<GodownMaster> GdwnList = new List<GodownMaster>();
            foreach (var data in godownlist)
            {
                GodownMaster GodownData = new GodownMaster();
                godown = data.GdCode + "  " + data.GodownName;
                GodownData.GodownName = godown;
                GodownData.GdCode = data.GdCode;
                GdwnList.Add(GodownData);
                godown = string.Empty;
            }
            model.GodownMasterList = GdwnList;
            model.ShopList = _ishopservice.GetAll();
            model.ClientList = _clientmasterservice.GetAllClients();
            model.EmpList = _employeeservice.getAllemployees();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            return View(model);
        }

        //GET SHOP DETAILS BY SHOP NAME
        [HttpGet]
        public JsonResult GetClientDetails(string name)
        {
            var data = _clientmasterservice.getClientByName(name);
            return Json(new { data.Address, data.Email, data.ContactNo1, data.State }, JsonRequestBehavior.AllowGet);
        }

        //GET ITEM LIST FROM SHOP STOCK
        [HttpGet]
        public JsonResult GetItemListFromShopStock(string shopcode)
        {
            var itemlist = _shopstockservice.GetRowsByShopCode(shopcode);
            var items = itemlist.Select(m => new SelectListItem
            {
                Text = m.ItemName,
                Value = m.ItemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //GET STOCK DISTRIBUTION ITEM DETAILS BY ITEM CODE
        [HttpGet]
        public JsonResult GetShopStockItemdetails(string ItemCode, string ShopCode)
        {
            var items = _shopstockservice.GetDetailsByItemCodeAndShopCode(ItemCode, ShopCode);
            return Json(new { items.ItemName, items.Description, items.Quantity, items.Barcode, items.Color, items.Material, items.DesignName, items.Unit, items.SellingPrice, items.MRP }, JsonRequestBehavior.AllowGet);
        }

        //SAVE DETAILS IN OUTWARD TO CLIENT MASTER
        [HttpPost]
        public ActionResult OutwardToClient(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                OutwardToClientDetails = new OutwardToClient(),
                OutwardItemToClientDetails = new OutwardItemToClient(),
            };

            string code = Session["LOGINSHOPGODOWNCODEOWC"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastOutward = _outwardtoclientservice.GetLastOutwardByFinYr(FinYr, code);
            string OutwardCode = string.Empty;
            int OutwardNoLength = 0;
            int OutwardNo = 0;
            if (LastOutward == null)
            {
                OutwardNoLength = 1;
                OutwardNo = 1;
            }
            else
            {
                string trim = LastOutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                OutwardCode = LastOutward.OutwardCode.Substring(index + 3, 6);
                OutwardNoLength = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                OutwardNo = Convert.ToInt32(OutwardCode) + 1;
            }
            string name = Session["SHOPGODOWNNAMEOWC"] as string;
            string codes = Session["LOGINSHOPGODOWNCODEOWC"] as string;
            string utilityname = string.Empty;
            if (code.Contains("SH"))
            {
                var details = _ishopservice.GetShopDetailsByName(name);
                utilityname = details.ShortCode + "/OWC";
            }
            else
            {
                var details = _godownservice.GetGodownDetailsByName(name);
                utilityname = details.ShortCode + "/OWC";
            }
            OutwardCode = _utilityservice.getName(utilityname, OutwardNoLength, OutwardNo);
            OutwardCode = OutwardCode + FinYr;

            //SAVE OUTWARD TO CLIENT MASTERS
            mainapp.OutwardToClientDetails.OutwardCode = OutwardCode;
            mainapp.OutwardToClientDetails.Date = DateTime.Now;
            mainapp.OutwardToClientDetails.Status = "Active";
            mainapp.OutwardToClientDetails.ModifiedOn = DateTime.Now;
            if (mainapp.OutwardToClientDetails.GodownCode != null)
            {
                var data = _godownservice.GetGodownByCode(mainapp.OutwardToClientDetails.GodownCode);
                mainapp.OutwardToClientDetails.GodownName = data.GodownName;
            }
            else
            {
                var data = _shopstockservice.GetDetailsByShopCode(mainapp.OutwardToClientDetails.ShopCode);
                mainapp.OutwardToClientDetails.ShopName = data.ShopName;
            }
            _outwardtoclientservice.Create(mainapp.OutwardToClientDetails);

            //SAVE OUTWARD TO CLIENT ITEM DETAILS
            string ItemCount = frmcol["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(ItemCount))
            {
                int count = Convert.ToInt32(ItemCount);
                for (int i = 1; i <= count; i++)
                {
                    //SAVE OUTWARD TO GODOWN ITEMS
                    string itemcodes = "ItemCode" + i;
                    string itemnames = "ItemName" + i;
                    string descriptionvalue = "DescriptionValue" + i;
                    string stockquantityvalue = "StockQuantityValue" + i;
                    string outwardquantity = "OutwardQuantity" + i;
                    string balancevalue = "BalanceValue" + i;
                    string barcode = "Barcode" + i;
                    string color = "Color" + i;
                    string material = "Material" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string design = "DesignName" + i;
                    string unit = "Unit" + i;

                    if (frmcol[itemcodes] != null)
                    {
                        model.OutwardItemToClientDetails.ItemCode = frmcol[itemcodes];
                        model.OutwardItemToClientDetails.ItemName = frmcol[itemnames];
                        model.OutwardItemToClientDetails.ItemDescription = frmcol[descriptionvalue];
                        model.OutwardItemToClientDetails.StockQuantity = Convert.ToDouble(frmcol[stockquantityvalue]);
                        model.OutwardItemToClientDetails.OutwardQuantity = Convert.ToDouble(frmcol[outwardquantity]);
                        model.OutwardItemToClientDetails.CurrentBalance = Convert.ToDouble(frmcol[balancevalue]);
                        model.OutwardItemToClientDetails.OutwardCode = OutwardCode;
                        model.OutwardItemToClientDetails.Status = "Active";
                        model.OutwardItemToClientDetails.ModifiedOn = DateTime.Now;
                        model.OutwardItemToClientDetails.Barcode = frmcol[barcode];
                        model.OutwardItemToClientDetails.Color = frmcol[color];
                        model.OutwardItemToClientDetails.Material = frmcol[material];
                        model.OutwardItemToClientDetails.DesignName = frmcol[design];
                        model.OutwardItemToClientDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.OutwardItemToClientDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        model.OutwardItemToClientDetails.Unit = frmcol[unit];
                        //if we select godown DDL then godown code stored in the item table else shop code will be stored
                        if (mainapp.OutwardToClientDetails.GodownCode != null)
                        {
                            model.OutwardItemToClientDetails.Code = mainapp.OutwardToClientDetails.GodownCode;
                        }
                        else
                        {
                            model.OutwardItemToClientDetails.Code = mainapp.OutwardToClientDetails.ShopCode;
                        }
                        _outwarditemtoclientservice.Create(model.OutwardItemToClientDetails);

                        //Minus outward quantity from stock distribution / shop stock
                        if (mainapp.OutwardToClientDetails.GodownCode != null)
                        {
                            model.StockItemDistributionDetails = _stockitemdistributionservice.GetLastRowFromItemAndGodownCode(model.OutwardItemToClientDetails.ItemCode, model.OutwardItemToClientDetails.Code);
                            model.StockItemDistributionDetails.ItemQuantity -= model.OutwardItemToClientDetails.OutwardQuantity;
                            model.StockItemDistributionDetails.ModifiedOn = DateTime.Now;
                            _stockitemdistributionservice.Update(model.StockItemDistributionDetails);

                            var godownstock = _GodownStockService.GetDetailsByItemCodeAndGodownCode(model.OutwardItemToClientDetails.ItemCode, model.OutwardItemToClientDetails.Code);
                            godownstock.Quantity -= model.OutwardItemToClientDetails.OutwardQuantity;
                            godownstock.ModifiedOn = DateTime.Now;
                            _GodownStockService.Update(godownstock);
                        }
                        else
                        {
                            var stockitem = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(model.OutwardItemToClientDetails.ItemCode, model.OutwardItemToClientDetails.Code);
                            stockitem.ItemQuantity -= model.OutwardItemToClientDetails.OutwardQuantity;
                            stockitem.ModifiedOn = DateTime.Now;
                            _stockitemdistributionservice.Update(stockitem);

                            model.ShopStockDetails = _shopstockservice.GetLastRowFromItemAndShopCode(model.OutwardItemToClientDetails.ItemCode, model.OutwardItemToClientDetails.Code);
                            model.ShopStockDetails.Quantity -= model.OutwardItemToClientDetails.OutwardQuantity;
                            _shopstockservice.Update(model.ShopStockDetails);
                        }

                        //Minus outward quantity from Entry / Opening stock
                        var entrystockdata = _entrystockitemservice.getDetailsByItemCode(model.OutwardItemToClientDetails.ItemCode);
                        var openingstockdata = _openingstockservice.GetDetailsByItemCode(model.OutwardItemToClientDetails.ItemCode);
                        if (entrystockdata != null)
                        {
                            entrystockdata.TotalQuantity = entrystockdata.TotalQuantity - Convert.ToDouble(model.OutwardItemToClientDetails.OutwardQuantity);
                            _entrystockitemservice.Update(entrystockdata);
                        }
                        else
                        {
                            openingstockdata.TotalQuantity = openingstockdata.TotalQuantity - model.OutwardItemToClientDetails.OutwardQuantity;
                            _openingstockservice.UpdateStock(openingstockdata);
                        }
                    }
                }
            }
            var outwardtoclientdetails = _outwardtoclientservice.GetLastRow();
            var OWTCId = Encode(outwardtoclientdetails.OutwardId.ToString());
            return RedirectToAction("OutwardToClientDetails/" + OWTCId, "Outward");
        }

        // DETAILS PAGE OF OUTWARD TO CLIENT 
        [HttpGet]
        public ActionResult OutwardToClientDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                OutwardToClientDetails = new OutwardToClient(),
                OutwardItemToClientDetails = new OutwardItemToClient(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.OutwardToClientDetails = _outwardtoclientservice.GetDetailsById(Id);
            model.OutwardItemToClientList = _outwarditemtoclientservice.GetDetailsByOutwardCode(model.OutwardToClientDetails.OutwardCode);
            string previousclientoutward = TempData["PreviousClientOutward"].ToString();
            if (TempData["PreviousClientOutward"].ToString() != model.OutwardToClientDetails.OutwardCode)
            {
                ViewData["ClientOutwardNoChanged"] = previousclientoutward + " is replaced with " + model.OutwardToClientDetails.OutwardCode + " because " + previousclientoutward + " is acquired by another person";
            }
            TempData["PreviousClientOutward"] = previousclientoutward;
            return View(model);
        }

        // PRINT PAGE OF OUTWARD TO CLIENT 
        [HttpGet]
        public ActionResult PrintOutwardToClient(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication()
            {
                OutwardToClientDetails = new OutwardToClient(),
                OutwardItemToClientDetails = new OutwardItemToClient(),
            };
            model.OutwardToClientDetails = _outwardtoclientservice.GetDetailsById(Id);
            model.OutwardItemToClientList = _outwarditemtoclientservice.GetDetailsByOutwardCode(model.OutwardToClientDetails.OutwardCode);
            return View(model);
        }


        [HttpGet]
        public ActionResult OutwardToClientPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();

            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWC"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWC"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWC"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWC"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNumbersClient(string term)
        {
            string code = Session["LOGINSHOPGODOWNCODEOWC"] as string;
            List<string> names = new List<string>();
            if (code.Contains("SH"))
            {
                var result = _outwardtoclientservice.GetOutwardNo(term, code);
                foreach (var data in result)
                {
                    names.Add(data.OutwardCode);
                }
            }
            else
            {
                var result = _outwardtoclientservice.GetOutwardNoForGodown(term, code);
                foreach (var data in result)
                {
                    names.Add(data.OutwardCode);
                }
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOutwardToClientDetailsForPrint(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardToClientDetails = _outwardtoclientservice.GetDetailsByOutwardCode(OutwardNo);
            model.OutwardItemToClientList = _outwarditemtoclientservice.GetDetailsByOutwardCode(OutwardNo);
            return View(model);
        }

        //************************************** OUTWARD FROM GODOWN TO GODOWN ******************************************************

        //CREATE OUTWARD FROM GODOWN TO GODOWN
        [HttpGet]
        public ActionResult OutwardInterGodown()
        {
            MainApplication model = new MainApplication
            {
                OutwardInterGodownDetails = new OutwardInterGodown(),
            };

            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }


            //CREATE GODOWN TO GODOWN CODE
            string gdcode = Session["LOGINSHOPGODOWNCODEOWGG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastOutward = _OutwardInterGodownService.GetLastOutwardByFinYr(FinYr, gdcode);
            string OutwardCode = string.Empty;
            int OutwardNoLength = 0;
            int OutwardNo = 0;
            if (LastOutward == null)
            {
                OutwardNoLength = 1;
                OutwardNo = 1;
            }
            else
            {
                string trim = LastOutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                OutwardCode = LastOutward.OutwardCode.Substring(index + 4, 6);
                OutwardNoLength = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                OutwardNo = Convert.ToInt32(OutwardCode) + 1;
            }
            string godownname = Session["SHOPGODOWNNAMEOWGG"] as string;
            var gddetails = _godownservice.GetGodownDetailsByName(godownname);
            var utilityname = gddetails.ShortCode + "/OWGG";
            OutwardCode = _utilityservice.getName(utilityname, OutwardNoLength, OutwardNo);
            OutwardCode = OutwardCode + FinYr;
            model.OutwardInterGodownDetails.OutwardCode = OutwardCode;
            TempData["PreviousGodownOutward"] = OutwardCode;

            model.ShopList = _ishopservice.GetAll();
            model.EmpList = _employeeservice.getAllemployees();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.GodownMasterList = _godownservice.GetGodownNamesExcludingOne(Session["SHOPGODOWNNAMEOWGG"].ToString());

            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardInterGodownPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();

            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNumbersInterGodown(string term)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEOWGG"] as string;
            var result = _OutwardInterGodownService.GetOutwardNoForPrint(term, godowncode);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOutwardInterGodownDetailsForPrint(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardInterGodownDetails = _OutwardInterGodownService.GetDetailsByOutwardCode(OutwardNo);
            model.OutwardItemInterGodownList = _OutwardItemInterGodownService.GetDetailsByOutwardCode(OutwardNo);
            return View(model);
        }

        [HttpPost]
        public ActionResult InterGodown(MainApplication model, FormCollection frm)
        {
            string gdcode = Session["LOGINSHOPGODOWNCODEOWGG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastOutward = _OutwardInterGodownService.GetLastOutwardByFinYr(FinYr, gdcode);
            string OutwardCode = string.Empty;
            int OutwardNoLength = 0;
            int OutwardNo = 0;
            if (LastOutward == null)
            {
                OutwardNoLength = 1;
                OutwardNo = 1;
            }
            else
            {
                string trim = LastOutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                OutwardCode = LastOutward.OutwardCode.Substring(index + 4, 6);
                OutwardNoLength = (Convert.ToInt32(OutwardCode) + 1).ToString().Length;
                OutwardNo = Convert.ToInt32(OutwardCode) + 1;
            }
            string godownname = Session["SHOPGODOWNNAMEOWGG"] as string;
            var gddetails = _godownservice.GetGodownDetailsByName(godownname);
            var utilityname = gddetails.ShortCode + "/OWGG";
            OutwardCode = _utilityservice.getName(utilityname, OutwardNoLength, OutwardNo);
            OutwardCode = OutwardCode + FinYr;

            var outwardlist = TempData["OutwardGodownToGodownItemList"] as IEnumerable<GodownStock>;
            model.OutwardInterGodownDetails.OutwardCode = OutwardCode;
            model.OutwardInterGodownDetails.Status = "Active";
            model.OutwardInterGodownDetails.ModifiedOn = DateTime.Now;
            model.OutwardInterGodownDetails.OutwardDate = DateTime.Now;
            model.OutwardInterGodownDetails.PrepareTime = DateTime.Now;
            _OutwardInterGodownService.Create(model.OutwardInterGodownDetails);
            int count = 1;

            foreach (var item in outwardlist)
            {
                OutwardItemInterGodown gditem = new OutwardItemInterGodown();
                string intertransfer = "InterTransfer" + count;
                string balance = "BalanceValue" + count;
                if (frm[intertransfer] != "" && Convert.ToDouble(frm[intertransfer]) != 0)
                {
                    gditem.OutwardCode = OutwardCode;
                    gditem.Barcode = item.Barcode;
                    gditem.DesignCode = item.DesignCode;
                    gditem.ColorCode = item.Color;
                    gditem.SellingPrice = item.SellingPrice;
                    gditem.MRP = item.MRP;
                    gditem.Description = item.Description;
                    gditem.DesignName = item.DesignName;
                    gditem.FromGdCode = model.OutwardInterGodownDetails.FromGodownCode;
                    gditem.ToGdCode = model.OutwardInterGodownDetails.ToGodownCode;
                    gditem.TypeOfMaterial = item.Material;
                    gditem.ItemName = item.ItemName;
                    gditem.ItemCode = item.ItemCode;
                    gditem.Unit = item.Unit;
                    gditem.NumberType = item.NumberType;
                    gditem.Size = item.Size;
                    gditem.Brand = item.Brand;
                    gditem.Balance = frm[balance];
                    gditem.QuantityTransfer = frm[intertransfer];
                    gditem.Status = "Active";
                    gditem.ModifiedOn = DateTime.Now;
                    _OutwardItemInterGodownService.Create(gditem);
                }
                count++;
            }

            int itemcount = 1;
            foreach (var item in outwardlist)
            {
                string intertransfer = "InterTransfer" + itemcount;
                string balance = "BalanceValue" + itemcount;

                if (frm[intertransfer] != "" && Convert.ToDouble(frm[intertransfer]) != 0)
                {
                    var fromstockdist = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(item.ItemCode, model.OutwardInterGodownDetails.FromGodownCode);
                    var gdstocks = _GodownStockService.GetDetailsByItemCodeAndGodownCode(item.ItemCode, model.OutwardInterGodownDetails.FromGodownCode);

                    fromstockdist.ItemQuantity -= Convert.ToDouble(frm[intertransfer]);
                    fromstockdist.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Update(fromstockdist);

                    gdstocks.Quantity -= Convert.ToDouble(frm[intertransfer]);
                    gdstocks.ModifiedOn = DateTime.Now;
                    _GodownStockService.Update(gdstocks);
                }
                itemcount++;
            }

            var id = _OutwardInterGodownService.GetLastRow().OutwardId;
            var OWIGId = Encode(id.ToString());
            return RedirectToAction("OutwardInterGodownDetails/" + OWIGId, "Outward");
        }

        [HttpGet]
        public ActionResult OutwardInterGodownDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                OutwardInterGodownDetails = new OutwardInterGodown(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.OutwardInterGodownDetails = _OutwardInterGodownService.GetDetailsByOutwardId(Id);
            model.OutwardItemInterGodownList = _OutwardItemInterGodownService.GetDetailsByOutwardCode(model.OutwardInterGodownDetails.OutwardCode);
            string previousgodownoutward = TempData["PreviousGodownOutward"].ToString();
            if (TempData["PreviousGodownOutward"].ToString() != model.OutwardInterGodownDetails.OutwardCode)
            {
                ViewData["InterGodownOutwardNoChanged"] = previousgodownoutward + " is replaced with " + model.OutwardInterGodownDetails.OutwardCode + " because " + previousgodownoutward + " is acquired by another person";
            }
            TempData["PreviousGodownOutward"] = previousgodownoutward;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintOutwardInterGodown(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication()
            {
                OutwardInterGodownDetails = new OutwardInterGodown(),
            };

            model.OutwardInterGodownDetails = _OutwardInterGodownService.GetDetailsByOutwardId(Id);
            model.OutwardItemInterGodownList = _OutwardItemInterGodownService.GetDetailsByOutwardCode(model.OutwardInterGodownDetails.OutwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardInterGodownDelete()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();

            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWGG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetOutwardInterGodownDetailsForDelete(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardInterGodownDetails = _OutwardInterGodownService.GetDetailsByOutwardCode(OutwardNo);
            TempData["InterGodown"] = model.OutwardInterGodownDetails;
            model.OutwardItemInterGodownList = _OutwardItemInterGodownService.GetDetailsByOutwardCode(OutwardNo);
            TempData["InterGodownItems"] = model.OutwardItemInterGodownList;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOutwardInterGodownDetailsForDelete(MainApplication model)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEOWGG"] as string;

            var outwardintergodown = TempData["InterGodown"] as OutwardInterGodown;
            var outwarditems = TempData["InterGodownItems"] as IEnumerable<OutwardItemInterGodown>;

            foreach (var data in outwarditems)
            {
                var godownstock = _GodownStockService.GetDetailsByItemCodeAndGodownCode(data.ItemCode, godowncode);
                godownstock.Quantity = godownstock.Quantity + Convert.ToDouble(data.QuantityTransfer);
                _GodownStockService.Update(godownstock);

                var stockitemdist = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(data.ItemCode, godowncode);
                stockitemdist.ItemQuantity = stockitemdist.ItemQuantity + Convert.ToDouble(data.QuantityTransfer);
                _stockitemdistributionservice.Update(stockitemdist);

                var item = _OutwardItemInterGodownService.GetRowByOutwardCode(data.OutwardCode);
                _OutwardItemInterGodownService.Delete(item);
            }

            var details = _OutwardInterGodownService.GetDetailsByOutwardCode(outwardintergodown.OutwardCode);
            _OutwardInterGodownService.Delete(details);

            return RedirectToAction("OutwardInterGodownDelete");
        }

        //************************************** OUTWARD FROM Shop TO Shop ******************************************************

        //CREATE OUTWARD FROM Shop TO Shop
        [HttpGet]
        public ActionResult OutwardInterShop()
        {
            MainApplication model = new MainApplication
            {
                OutwardInterShopDetails = new OutwardInterShop(),
            };

            model.GodownMasterList = _godownservice.GetGodownNames();
            model.ShopList = _ishopservice.GetAll();
            model.EmpList = _employeeservice.getAllemployees();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            //CREATE SHOP TO SHOP CODE

            string shcode = Session["LOGINSHOPGODOWNCODEOWSS"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastoutward = _OutwardInterShopService.GetLastOutwardByFinYear(finyr, shcode);
            string outwardcode = string.Empty;
            int outwardVal = 0;
            int length = 0;
            if (lastoutward != null)
            {
                string trim = lastoutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                outwardcode = lastoutward.OutwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(outwardcode) + 1).ToString().Length;
                outwardVal = Convert.ToInt32(outwardcode) + 1;
            }
            else
            {
                outwardVal = 1;
                length = 1;
            }
            string shopname = Session["SHOPGODOWNNAMEOWSS"] as string;
            var shopdetails = _ishopservice.GetShopDetailsByName(shopname);
            var utilityname = shopdetails.ShortCode + "/OWSS";
            outwardcode = _utilityservice.getName(utilityname, length, outwardVal);
            outwardcode = outwardcode + finyr;
            model.OutwardInterShopDetails.OutwardCode = outwardcode;
            TempData["PreviousInterShopOutward"] = outwardcode;

            return View(model);
        }

        [HttpPost]
        public ActionResult InterShop(MainApplication model, FormCollection frm)
        {
            string shcode = Session["LOGINSHOPGODOWNCODEOWSS"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastoutward = _OutwardInterShopService.GetLastOutwardByFinYear(finyr, shcode);
            string outwardcode = string.Empty;
            int outwardVal = 0;
            int length = 0;
            if (lastoutward != null)
            {
                string trim = lastoutward.OutwardCode;
                int index = trim.LastIndexOf('O');
                outwardcode = lastoutward.OutwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(outwardcode) + 1).ToString().Length;
                outwardVal = Convert.ToInt32(outwardcode) + 1;
            }
            else
            {
                outwardVal = 1;
                length = 1;
            }
            string shopname = Session["SHOPGODOWNNAMEOWSS"] as string;
            var shopdetails = _ishopservice.GetShopDetailsByName(shopname);
            var utilityname = shopdetails.ShortCode + "/OWSS";
            outwardcode = _utilityservice.getName(utilityname, length, outwardVal);
            outwardcode = outwardcode + finyr;

            var outwardlist = TempData["OutwardShopToShopItemList"] as IEnumerable<ShopStock>;
            model.OutwardInterShopDetails.OutwardCode = outwardcode;
            model.OutwardInterShopDetails.Status = "Active";
            model.OutwardInterShopDetails.ModifiedOn = DateTime.Now;
            model.OutwardInterShopDetails.OutwardDate = DateTime.Now;
            model.OutwardInterShopDetails.PrepareTime = DateTime.Now;
            _OutwardInterShopService.CreateOutwardInterShop(model.OutwardInterShopDetails);

            int count = 1;

            foreach (var item in outwardlist)
            {
                OutwardItemInterShop gditem = new OutwardItemInterShop();
                string intertransfer = "InterTransfer" + count;
                string balance = "BalanceValue" + count;
                if (frm[intertransfer] != "" && Convert.ToDouble(frm[intertransfer]) != 0)
                {
                    gditem.OutwardCode = outwardcode;
                    gditem.Barcode = item.Barcode;
                    gditem.Brand = item.Brand;
                    gditem.ColorCode = item.Color;
                    gditem.Description = item.Description;
                    gditem.DesignCode = item.DesignCode;
                    gditem.DesignName = item.DesignName;
                    gditem.FromShopCode = model.OutwardInterShopDetails.FromShopCode;
                    gditem.ToShopCode = model.OutwardInterShopDetails.ToShopCode;
                    gditem.TypeOfMaterial = item.Material;
                    gditem.SellingPrice = item.SellingPrice;
                    gditem.MRP = item.MRP;
                    gditem.ItemName = item.ItemName;
                    gditem.ItemCode = item.ItemCode;
                    gditem.NumberType = item.NumberType;
                    gditem.Size = item.Size;
                    gditem.Unit = item.Unit;
                    gditem.Balance = Convert.ToDouble(frm[balance]);
                    gditem.QuantityTransfer = Convert.ToDouble(frm[intertransfer]);
                    gditem.Status = "Active";
                    gditem.ModifiedOn = DateTime.Now;
                    _OutwardItemInterShopservice.CreateInterShopitem(gditem);
                }
                count++;
            }


            int itemcount = 1;
            foreach (var item in outwardlist)
            {
                string intertransfer = "InterTransfer" + itemcount;
                string balance = "BalanceValue" + itemcount;

                ShopStock shstock = new ShopStock();
                if (frm[intertransfer] != "" && Convert.ToDouble(frm[intertransfer]) != 0)
                {
                    var stockdist = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.OutwardInterShopDetails.FromShopCode);

                    stockdist.ItemQuantity -= Convert.ToDouble(frm[intertransfer]);
                    stockdist.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Update(stockdist);

                    var fromshstockpresent = _shopstockservice.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.OutwardInterShopDetails.FromShopCode);

                    fromshstockpresent.Quantity -= Convert.ToDouble(frm[intertransfer]);
                    fromshstockpresent.ModifiedOn = DateTime.Now;
                    _shopstockservice.Update(fromshstockpresent);
                }
                itemcount++;
            }

            var outwrddet = _OutwardInterShopService.GetLastRow();
            string Id = Encode(outwrddet.OutwardId.ToString());
            return RedirectToAction("OutwardInterShopDetails/" + Id, "Outward");
        }


        [HttpGet]
        public ActionResult OutwardInterShopDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                OutwardInterShopDetails = new OutwardInterShop(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.OutwardInterShopDetails = _OutwardInterShopService.GetDetailsByOutwardId(Id);
            model.OutwardItemInterShopList = _OutwardItemInterShopservice.GetDetailsByOutwardCode(model.OutwardInterShopDetails.OutwardCode);
            string previousshopnoutward = TempData["PreviousInterShopOutward"].ToString();
            if (TempData["PreviousInterShopOutward"].ToString() != model.OutwardInterShopDetails.OutwardCode)
            {
                ViewData["InterOutwardNoChanged"] = previousshopnoutward + " is replaced with " + model.OutwardInterShopDetails.OutwardCode + " because " + previousshopnoutward + " is acquired by another person";
            }
            TempData["PreviousInterShopOutward"] = previousshopnoutward;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintOutwardInterShop(string id)
        {
            MainApplication model = new MainApplication();
            model.OutwardInterShopDetails = new OutwardInterShop();
            model.OutwardItemInterShopdetails = new OutwardItemInterShop();
            int Id = Decode(id);
            model.OutwardInterShopDetails = _OutwardInterShopService.GetDetailsByOutwardId(Id);
            model.OutwardItemInterShopList = _OutwardItemInterShopservice.GetDetailsByOutwardCode(model.OutwardInterShopDetails.OutwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardInterShopPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();

            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNumbersInterShop(string term)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODEOWSS"] as string;
            var result = _OutwardInterShopService.GetOutwardNoForPrint(term, shopcode);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOutwardInterShopDetailsForPrint(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardInterShopDetails = _OutwardInterShopService.GetDetailsByOutwardCode(OutwardNo);
            model.OutwardItemInterShopList = _OutwardItemInterShopservice.GetDetailsByOutwardCode(OutwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult OutwardInterShopDelete()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();

            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEOWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEOWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEOWSS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetOutwardInterShopDetailsForDelete(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.OutwardInterShopDetails = _OutwardInterShopService.GetDetailsByOutwardCode(OutwardNo);
            TempData["InterShop"] = model.OutwardInterShopDetails;
            model.OutwardItemInterShopList = _OutwardItemInterShopservice.GetDetailsByOutwardCode(OutwardNo);
            TempData["InterShopItems"] = model.OutwardItemInterShopList;
            return View(model);
        }

        [HttpPost]
        public ActionResult GetOutwardInterShopDetailsForDelete(MainApplication model)
        {
            var outwarddata = TempData["InterShop"] as OutwardInterShop;
            var itemlist = TempData["InterShopItems"] as IEnumerable<OutwardItemInterShop>;
            string shopcode = Session["LOGINSHOPGODOWNCODEOWSS"] as string;

            foreach (var data in itemlist)
            {
                var shopstock = _shopstockservice.GetDetailsByItemCodeAndShopCode(data.ItemCode, shopcode);
                shopstock.Quantity = shopstock.Quantity + data.QuantityTransfer;
                _shopstockservice.Update(shopstock);

                var stockdist = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(data.ItemCode, shopcode);
                stockdist.ItemQuantity = stockdist.ItemQuantity + data.QuantityTransfer;
                _stockitemdistributionservice.Update(stockdist);

                var item = _OutwardItemInterShopservice.GetDetailsByItemCodeandShopCode(data.ItemCode, shopcode);
                _OutwardItemInterShopservice.Delete(item);
            }

            var details = _OutwardInterShopService.GetDetailsByOutwardCode(outwarddata.OutwardCode);
            _OutwardInterShopService.Delete(details);

            return RedirectToAction("OutwardInterShopDelete");
        }

        //GET ITEMS FROM STOCK DISTRIBUTION
        [HttpGet]
        public ActionResult GetItemsFromStockDistribtn(string GodownCode)
        {
            MainApplication model = new MainApplication();
            model.GodownStockList = _GodownStockService.GetRowsByGodownCode(GodownCode);
            TempData["OutwardGodownToGodownItemList"] = model.GodownStockList;
            return View(model);
        }


        [HttpGet]
        public ActionResult GetItemsFromStockDistribtnforshop(string ShopCode)
        {
            MainApplication model = new MainApplication();
            model.ShopStockList = _shopstockservice.GetRowsByShopCode(ShopCode);
            TempData["OutwardShopToShopItemList"] = model.ShopStockList;
            return View(model);
        }

        [HttpGet]
        public JsonResult CheckReferenceNo(string refno)
        {
            var details = _StockDistributionService.GetReferenceNo(refno);
            string msg = "true";
            if(details == null)
            {
                msg = "false";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
