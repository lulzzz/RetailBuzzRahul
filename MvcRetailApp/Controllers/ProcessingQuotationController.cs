using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstData;
using CodeFirstServices.Interfaces;
using System.Data.SqlClient;
using System.Web.Routing;
using MvcRetailApp.Filters;
using System.IO;
using System.Configuration;

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class ProcessingQuotationController : Controller
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
        private readonly IQuotationService _QuotationService;
        private readonly IQuotationItemService _QuotationItemService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IUtilityService _UtilityService;
        private readonly IClientMasterService _ClientMasterService;
        private readonly IItemCategoryService _Itemcategoryservice;
        private readonly IItemSubCategoryService _Itemsubcategoryservice;
        private readonly IUnitService _UnitService;
        private readonly ITransportService _TransportService;
        private readonly IEmployeeMasterService _employeeservice;
        private readonly ISalesOrderService _SalesOrderService;
        private readonly ISalesOrderItemService _SalesOrderItemService;
        private readonly IItemService _ItemService;
        private readonly IDesignService _DesignService;
        private readonly IItemTaxService _ItemTaxService;
        private readonly IDeliveryChallanService _DeliveryChallanService;
        private readonly IDeliveryChallanItemService _DeliveryChallanItemService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IGodownService _GodownService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IInventoryTaxService _InventoryTaxService;
        private readonly IShopStockService _ShopStockService;
        private readonly IGodownStockService _GodownStockService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IShopService _ShopService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IDiscountMasterItemService _DiscountMasterItemService;
        private readonly IClientBankDetailService _ClientBankDetailService;
        private readonly ICountryService _CountryService;
        private readonly IStateService _StateService;

        public ProcessingQuotationController(IQuotationService QuotationService, IQuotationItemService QuotationItemService, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IUtilityService UtilityService, IClientMasterService ClientMasterService,
             IItemCategoryService Itemcategoryservice, IItemSubCategoryService Itemsubcategoryservice, IUnitService UnitService, ITransportService TransportService, IEmployeeMasterService employeeservice, ISalesOrderService SalesOrderService,
             ISalesOrderItemService SalesOrderItemService, IItemService ItemService, IDesignService DesignService, IItemTaxService ItemTaxService, IDeliveryChallanService DeliveryChallanService, IDeliveryChallanItemService DeliveryChallanItemService,
            IEntryStockItemService EntryStockItemService, IGodownService GodownService, IStockItemDistributionService StockItemDistributionService, IOpeningStockService OpeningStockService, IInventoryTaxService InventoryTaxService, IShopStockService ShopStockService,
            IGodownStockService GodownStockService, INonInventoryItemService NonInventoryItemService, IShopService ShopService, IEmployeeMasterService EmployeeMasterService, IDiscountMasterItemService DiscountMasterItemService, IClientBankDetailService ClientBankDetailService, ICountryService CountryService, IStateService StateService)
        {
            this._QuotationService = QuotationService;
            this._QuotationItemService = QuotationItemService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._UtilityService = UtilityService;
            this._ClientMasterService = ClientMasterService;
            this._Itemcategoryservice = Itemcategoryservice;
            this._Itemsubcategoryservice = Itemsubcategoryservice;
            this._UnitService = UnitService;
            this._TransportService = TransportService;
            this._employeeservice = employeeservice;
            this._SalesOrderService = SalesOrderService;
            this._SalesOrderItemService = SalesOrderItemService;
            this._ItemService = ItemService;
            this._DesignService = DesignService;
            this._ItemTaxService = ItemTaxService;
            this._DeliveryChallanService = DeliveryChallanService;
            this._DeliveryChallanItemService = DeliveryChallanItemService;
            this._EntryStockItemService = EntryStockItemService;
            this._GodownService = GodownService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._OpeningStockService = OpeningStockService;
            this._InventoryTaxService = InventoryTaxService;
            this._ShopStockService = ShopStockService;
            this._GodownStockService = GodownStockService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._ShopService = ShopService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._DiscountMasterItemService = DiscountMasterItemService;
            this._CountryService = CountryService;
            this._ClientBankDetailService = ClientBankDetailService;
            this._StateService = StateService;
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
        public JsonResult EncodeId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Quotation()
        {
            MainApplication model = new MainApplication()
            {
                QuotationDetails = new Quotation(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
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

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastQuot = _QuotationService.GetLastQuotByFinYr(FinYr, shopcode);
            string QuotCode = string.Empty;
            int QuotNoLength = 0;
            int QuotationNo = 0;
            if (LastQuot == null)
            {
                QuotNoLength = 1;
                QuotationNo = 1;
            }
            else
            {
                int QuotIndex = LastQuot.QuotNo.LastIndexOf('Q');
                QuotCode = LastQuot.QuotNo.Substring(QuotIndex + 2, 6);
                QuotNoLength = (Convert.ToInt32(QuotCode) + 1).ToString().Length;
                QuotationNo = Convert.ToInt32(QuotCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            QuotCode = _UtilityService.getName(ShortCode + "/Qu", QuotNoLength, QuotationNo);
            QuotCode = QuotCode + FinYr;
            TempData["PreviousQuotation"] = QuotCode;
            model.QuotationDetails.QuotNo = QuotCode;
            model.ItemSubCategoryList = _Itemsubcategoryservice.GetItemSubCategories();
            model.EmpList = _employeeservice.getAllemployees();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetClientDetails(string name)
        {
            var details = _ClientMasterService.getClientByName(name);
            return Json(new
            {
                details.ClientName,
                details.Address,
                details.Email,
                details.ContactNo1,
                details.State,
                details.ClientType,
                details.ConsumeResell,
                details.FormType,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadItemBySubCategoryFromStkDist(string code)
        {

            var StkItemList = _ShopStockService.GetRowsByShopCode(code);
            var itemlist = StkItemList.Select(m => new SelectListItem()
            {
                Text = m.ItemName,
                Value = m.ItemCode
            });
            return Json(itemlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemDetailsFromItems(string itemname, string type, string State, string Formtype, string ConsumeResell, string shopcode)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(itemname);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            var itemdata = _ItemService.GetDescriptionByItemCode(itemname);
            string subcat = itemdata.itemSubCategory;
            string costprice = itemdata.costprice;
            var itemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(itemname, shopcode);
            string typematerial = itemdetails.Material;
            var list = _DesignService.GetDesignNamesBySubCat(itemdetails.SubCategory);
            var select = list.Select(m => new SelectListItem
            {
                Text = m.DesignName,
                Value = m.DesignCode,
            });
            string colorname = itemdetails.Color;
            double taxPercent = 0;
            if (State == "Maharashtra")
            {
                if (type == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (type == "Registered")
                {
                    if (ConsumeResell == "Reseller")
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            if (Formtype == "C-Form")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            return Json(new { discount, discountrps, taxPercent, subcat, costprice, itemdetails.Barcode, itemdetails.NumberType, itemdetails.Unit, itemdetails.ItemName, itemdetails.SellingPrice, itemdetails.MRP, itemdetails.Description, typematerial, itemdetails.DesignCode, itemdetails.DesignName, colorname, itemdetails.Brand, itemdetails.Size, select }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTaxForAlreadyPresentItems(string itemcode, string Formtype, string State, string ConsumeResell, string type, string itemtype)
        {
            double taxPercent = 0;
            string subcat = string.Empty;
            if (itemtype == "Inventory")
            {
                subcat = _ItemService.GetDescriptionByItemCode(itemcode).itemSubCategory;
            }
            else
            {
                subcat = _NonInventoryItemService.GetDetailsByItemCode(itemcode).itemSubCategory;
            }

            if (State == "Maharashtra")
            {
                if (type == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (type == "Registered")
                {
                    if (ConsumeResell == "Reseller")
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            if (Formtype == "C-Form")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            return Json(taxPercent, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PopUpPage(string code, string item, string type)
        {
            MainApplication model = new MainApplication();
            model.DesignDetails = _DesignService.GetDetailsByCode(code);
            if (type == "Inventory")
            {
                model.ItemDetails = _ItemService.GetDescriptionByItemCode(item);
            }
            else
            {
                model.NonInventoryItemDetails = _NonInventoryItemService.GetDetailsByItemCode(item);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateClient()
        {
            MainApplication model = new MainApplication()
            {
                ClientDetails = new ClientMaster()
            };

            var ClientDetails = _ClientMasterService.GetLastInsertedClient();
            int lastid = 0;
            int length = 0;
            if (ClientDetails != null)
            {
                lastid = ClientDetails.ClientId;
                lastid = lastid + 1;
                length = lastid.ToString().Length;
            }
            else
            {
                lastid = 1;
                length = 1;
            }
            string catCode = _UtilityService.getName("CL", length, lastid);
            model.ClientDetails.ClientCode = catCode;
            TempData["clientcode"] = catCode;

            var codelist = _ClientBankDetailService.GetDetailsFromBank(catCode);
            model.ClientDetails.StateList = _StateService.GetStateByCountry(1);
            if (codelist.Count() != 0)
            {
                _ClientBankDetailService.ExecuteProcedure(catCode);
            }
            Session["ClientCode"] = catCode;
            model.ClientDetails.CountryList = _CountryService.getallcountries();
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateClientBank()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Quotation(MainApplication model, FormCollection frm, HttpPostedFileBase file)
        {
            MainApplication mainapp = new MainApplication()
            {
                QuotationItemDetails = new QuotationItem(),
            };

            //if(Request!=null)
            //{
            //    HttpPostedFileBase file = Request.Files["Attachment"];
            //    if ((file != null) &&!string.IsNullOrEmpty(file.FileName))
            //    {
            //        string fileName = file.FileName;
            //        string fileContentType = file.ContentType;
            //        byte[] fileBytes = new byte[file.ContentLength];
            //        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            //    }
            //}
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = ConfigurationManager.AppSettings["Attachment"].ToString() + "/" + fileName;
                file.SaveAs(path);

                model.QuotationDetails.Attachment = fileName;
            }

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastQuot = _QuotationService.GetLastQuotByFinYr(FinYr, shopcode);
            string QuotCode = string.Empty;
            int QuotNoLength = 0;
            int QuotationNo = 0;
            if (LastQuot == null)
            {
                QuotNoLength = 1;
                QuotationNo = 1;
            }
            else
            {
                int QuotIndex = LastQuot.QuotNo.LastIndexOf('Q');
                QuotCode = LastQuot.QuotNo.Substring(QuotIndex + 2, 6);
                QuotNoLength = (Convert.ToInt32(QuotCode) + 1).ToString().Length;
                QuotationNo = Convert.ToInt32(QuotCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            QuotCode = _UtilityService.getName(ShortCode + "/Qu", QuotNoLength, QuotationNo);
            QuotCode = QuotCode + FinYr;

            //var quotlist = _QuotationService.GetQuotListByClientNameAndProcessedIn(model.QuotationDetails.ClientName);
            //foreach (var data in quotlist)
            //{
            //    if (data.Editable == "Yes")
            //    {
            //        data.Editable = "No";
            //    }
            //    _QuotationService.Update(data);
            //}
            int rowid = Convert.ToInt32(frm["hdnRowCount"]);
            for (int i = 1; i <= rowid; i++)
            {
                string itemCode = "itemCode" + i;
                string subcat = "SubCat" + i;
                string ItemType = "ItemType" + i;
                string itemName = "itemName" + i;
                string barcode = "Barcode" + i;
                string description = "description" + i;
                string narration = "narration" + i;
                string quantity = "quantity" + i;
                string sellingprice = "SellingPrice" + i;
                string mrp = "MRP" + i;
                string materialvalue = "materialvalue" + i;
                string colorvalue = "colorvalue" + i;
                string design = "design" + i;
                string designName = "designName" + i;
                string brand = "Brand" + i;
                string size = "Size" + i;
                string discountpercent = "discountpercent" + i;
                string discountrps = "discountrps" + i;
                string unitvalue = "unitvalue" + i;
                string numbertype = "numberType" + i;
                string amountvalue = "amountvalue" + i;
                string itemtax = "ItemTaxValue" + i;
                string itemtaxamt = "ItemTaxAmount" + i;
                string proportionatetax = "proportionatetaxvalue" + i;
                string proportionatetaxamount = "proportionatetaxamount" + i;
                string attachment = "attachment" + i;

                if (frm[quantity] != "" && frm[itemCode] != null)
                {
                    mainapp.QuotationItemDetails.QuotNo = QuotCode;
                    mainapp.QuotationItemDetails.SubCategory = frm[subcat];
                    mainapp.QuotationItemDetails.ItemType = frm[ItemType];
                    mainapp.QuotationItemDetails.ItemCode = frm[itemCode];
                    mainapp.QuotationItemDetails.ItemName = frm[itemName];
                    if (frm[barcode] == "")
                    {
                        mainapp.QuotationItemDetails.Barcode = null;
                    }
                    else
                    {
                        mainapp.QuotationItemDetails.Barcode = frm[barcode];
                    }
                    mainapp.QuotationItemDetails.Description = frm[description];
                    mainapp.QuotationItemDetails.Narration = frm[narration];
                    mainapp.QuotationItemDetails.Color = frm[colorvalue];
                    mainapp.QuotationItemDetails.Material = frm[materialvalue];
                    mainapp.QuotationItemDetails.DesignCode = frm[design];
                    mainapp.QuotationItemDetails.DesignName = frm[designName];
                    mainapp.QuotationItemDetails.Brand = frm[brand];
                    mainapp.QuotationItemDetails.Size = frm[size];
                    mainapp.QuotationItemDetails.Quantity = frm[quantity];
                    mainapp.QuotationItemDetails.Balance = Convert.ToDouble(frm[quantity]);
                    mainapp.QuotationItemDetails.Unit = frm[unitvalue];
                    mainapp.QuotationItemDetails.NumberType = frm[numbertype];
                    mainapp.QuotationItemDetails.SellingPrice = Convert.ToDouble(frm[sellingprice]);
                    mainapp.QuotationItemDetails.MRP = Convert.ToDouble(frm[mrp]);
                    mainapp.QuotationItemDetails.Amount = frm[amountvalue];
                    mainapp.QuotationItemDetails.Attachment = frm[attachment];

                    if (frm[discountpercent] != "")
                    {
                        mainapp.QuotationItemDetails.DiscountPercent = Convert.ToDouble(frm[discountpercent]);
                    }
                    else { mainapp.QuotationItemDetails.DiscountPercent = 0; }
                    if (frm[discountrps] != "")
                    {
                        mainapp.QuotationItemDetails.DiscountRPS = Convert.ToDouble(frm[discountrps]);
                    }
                    else { mainapp.QuotationItemDetails.DiscountRPS = 0; }
                    mainapp.QuotationItemDetails.ItemTax = frm[itemtax];
                    mainapp.QuotationItemDetails.ItemTaxAmt = frm[itemtaxamt];
                    mainapp.QuotationItemDetails.ProportionateTax = frm[proportionatetax];
                    mainapp.QuotationItemDetails.ProportionateTaxAmt = frm[proportionatetaxamount];
                    mainapp.QuotationItemDetails.Status = "Active";
                    mainapp.QuotationItemDetails.ProcessedIn = "Fresh";
                    mainapp.QuotationItemDetails.ModifiedOn = DateTime.Now;
                    _QuotationItemService.Create(mainapp.QuotationItemDetails);
                }
            }

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frm["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frm[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = QuotCode;
                    model.InventoryTaxDetails.Amount = frm[amountontax];
                    model.InventoryTaxDetails.Tax = frm[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frm[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }
            model.QuotationDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            model.QuotationDetails.PreparedTime = DateTime.Now.ToLongTimeString();
            model.QuotationDetails.QuotNo = QuotCode;
            model.QuotationDetails.QuotDate = DateTime.Now;
            model.QuotationDetails.Status = "Active";
            model.QuotationDetails.SendingStatus = "Pending";
            model.QuotationDetails.ProcessedIn = "Fresh";
            model.QuotationDetails.Editable = "Yes";
            model.QuotationDetails.ModifiedOn = DateTime.Now;
            model.QuotationDetails.TotalAmount = frm["Total_Amount_Value"];
            model.QuotationDetails.GrandTotal = frm["Grand_Total_Value"];
            _QuotationService.Create(model.QuotationDetails);
            int id = _QuotationService.GetDetailsByQuotNo(QuotCode).Id;
            string QuotId = Encode(id.ToString());
            return RedirectToAction("QuotationDetails/" + QuotId, "ProcessingQuotation");
        }

        [HttpGet]
        public ActionResult QuotationDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.QuotationDetails = _QuotationService.GetQuotById(Id);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.QuotationDetails.QuotNo);
            model.QuotationItemList = _QuotationItemService.GetItemsByCode(model.QuotationDetails.QuotNo);
            string previousquotation = TempData["PreviousQuotation"].ToString();
            if (previousquotation != model.QuotationDetails.QuotNo)
            {
                ViewData["QuotationChanged"] = previousquotation + " is replaced with " + model.QuotationDetails.QuotNo + " because " + previousquotation + " is acquired by another person";
            }
            TempData["PreviousQuotation"] = previousquotation;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintQuotationWithSellingPrice(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.QuotationDetails = _QuotationService.GetQuotById(Id);
            model.QuotationItemList = _QuotationItemService.GetItemsByCode(model.QuotationDetails.QuotNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintQuotationWithMRP(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.QuotationDetails = _QuotationService.GetQuotById(Id);
            model.QuotationItemList = _QuotationItemService.GetItemsByCode(model.QuotationDetails.QuotNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetQuotationList(string FromDate, string ToDate)
        {
            MainApplication model = new MainApplication();
            DateTime FDate = Convert.ToDateTime(FromDate);
            DateTime TDate = Convert.ToDateTime(ToDate);
            model.QuotationList = _QuotationService.GetQuotByDate(FDate, TDate);
            return View(model);
        }

        [HttpGet]
        public JsonResult DeleteQuotation(string QuotationNo)
        {
            MainApplication model = new MainApplication();
            var data = _QuotationService.GetQuotByCode(QuotationNo);
            data.Status = "InActive";
            data.ModifiedOn = DateTime.Now;
            _QuotationService.Update(data);
            var itemdet = _QuotationItemService.GetDetailsByQuotNo(QuotationNo);
            foreach (var item in itemdet)
            {
                item.Status = "InActive";
                item.ModifiedOn = DateTime.Now;
                _QuotationItemService.Update(item);
            }
            return Json(QuotationNo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult QuotationEdit()
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
        public JsonResult QuotationEditList(string term)
        {
            var data = _QuotationService.GetQuotNo(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.QuotNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult QuotationPrintList(string term)
        {
            var data = _QuotationService.GetAllQuotations(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.QuotNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadItemsBySubCategory(string subcat)
        {
            var itemlist = _ItemService.GetItemsBySubCategory(subcat);
            var items = itemlist.Select(m => new SelectListItem()
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetQuotationEditList(string QuotNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.QuotationDetails = _QuotationService.GetQuotByCode(QuotNo);
            model.QuotationItemList = _QuotationItemService.GetDetailsByQuotNo(QuotNo);
            TempData["QuotItemList"] = model.QuotationItemList;
            model.EmpList = _employeeservice.getAllemployees();
            model.ItemSubCategoryList = _Itemsubcategoryservice.GetItemSubCategories();
            model.UnitList = _UnitService.getallsize();
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(QuotNo);
            if (model.InventoryTaxList.Count() != 0)
            {
                TempData["QuotTaxList"] = model.InventoryTaxList;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult GetQuotationEditList(MainApplication model, FormCollection frm)
        {
            model.QuotationItemDetails = new QuotationItem();
            var quotitemlist = TempData["QuotItemList"] as IEnumerable<QuotationItem>;
            foreach (var deleteitem in quotitemlist)
            {
                var delete = _QuotationItemService.GetRowByQuotNo(deleteitem.QuotNo);
                _QuotationItemService.Delete(delete);
            }
            var quottaxlist = TempData["QuotTaxList"] as IEnumerable<InventoryTax>;
            if (quottaxlist != null)
            {
                foreach (var deletetax in quottaxlist)
                {
                    var delete = _InventoryTaxService.GetTaxById(deletetax.Id);
                    _InventoryTaxService.Delete(delete);
                }
            }
            int rowid = Convert.ToInt32(frm["hdnRowCount"]);
            for (int i = 1; i <= rowid; i++)
            {
                string itemCode = "itemCode" + i;
                string subcat = "SubCat" + i;
                string itemName = "itemName" + i;
                string barcode = "Barcode" + i;
                string ItemType = "ItemType" + i;
                string description = "description" + i;
                string narration = "narration" + i;
                string quantity = "quantity" + i;
                string sellingprice = "SellingPrice" + i;
                string mrp = "MRP" + i;
                string materialvalue = "materialvalue" + i;
                string colorvalue = "colorvalue" + i;
                string design = "design" + i;
                string designName = "designName" + i;
                string brand = "Brand" + i;
                string size = "Size" + i;
                string discountpercent = "discountpercent" + i;
                string discountrps = "discountrps" + i;
                string unitvalue = "unitvalue" + i;
                string numbertype = "numberType" + i;
                string amountvalue = "amountvalue" + i;
                string itemtax = "ItemTaxValue" + i;
                string itemtaxamt = "ItemTaxAmount" + i;
                string processedin = "ProcessedIn" + i;
                string proportionatetaxamount = "proportionatetaxamount" + i;
                string proportionatetaxvalue = "proportionatetaxvalue" + i;
                if (frm[quantity] != "" && frm[itemCode] != null)
                {
                    model.QuotationItemDetails.QuotNo = model.QuotationDetails.QuotNo;
                    model.QuotationItemDetails.SubCategory = frm[subcat];
                    model.QuotationItemDetails.ItemCode = frm[itemCode];
                    model.QuotationItemDetails.ItemName = frm[itemName];
                    if (frm[barcode] == "")
                    {
                        model.QuotationItemDetails.Barcode = null;
                    }
                    else
                    {
                        model.QuotationItemDetails.Barcode = frm[barcode];
                    }
                    model.QuotationItemDetails.ItemType = frm[ItemType];
                    model.QuotationItemDetails.Description = frm[description];
                    model.QuotationItemDetails.Narration = frm[narration];
                    model.QuotationItemDetails.Color = frm[colorvalue];
                    model.QuotationItemDetails.Material = frm[materialvalue];
                    model.QuotationItemDetails.DesignCode = frm[design];
                    model.QuotationItemDetails.DesignName = frm[designName];
                    model.QuotationItemDetails.Brand = frm[brand];
                    model.QuotationItemDetails.Size = frm[size];
                    model.QuotationItemDetails.Quantity = frm[quantity];
                    model.QuotationItemDetails.Balance = Convert.ToDouble(frm[quantity]);
                    model.QuotationItemDetails.Unit = frm[unitvalue];
                    model.QuotationItemDetails.NumberType = frm[numbertype];
                    model.QuotationItemDetails.SellingPrice = Convert.ToDouble(frm[sellingprice]);
                    model.QuotationItemDetails.MRP = Convert.ToDouble(frm[mrp]);
                    model.QuotationItemDetails.Amount = frm[amountvalue];
                    if (frm[discountpercent] != "")
                    {
                        model.QuotationItemDetails.DiscountPercent = Convert.ToDouble(frm[discountpercent]);
                    }
                    else { model.QuotationItemDetails.DiscountPercent = 0; }
                    if (frm[discountrps] != "")
                    {
                        model.QuotationItemDetails.DiscountRPS = Convert.ToDouble(frm[discountrps]);
                    }
                    else { model.QuotationItemDetails.DiscountRPS = 0; }
                    if (frm[processedin] == null)
                    {
                        model.QuotationItemDetails.ProcessedIn = "Fresh";
                    }
                    else
                    {
                        model.QuotationItemDetails.ProcessedIn = frm[processedin];
                    }
                    model.QuotationItemDetails.ItemTax = frm[itemtax];
                    model.QuotationItemDetails.ItemTaxAmt = frm[itemtaxamt];
                    model.QuotationItemDetails.ProportionateTax = frm[proportionatetaxvalue];
                    model.QuotationItemDetails.ProportionateTaxAmt = frm[proportionatetaxamount];
                    model.QuotationItemDetails.Status = "Active";
                    model.QuotationItemDetails.ModifiedOn = DateTime.Now;
                    _QuotationItemService.Create(model.QuotationItemDetails);
                }
            }
            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frm["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frm[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = model.QuotationDetails.QuotNo;
                    model.InventoryTaxDetails.Amount = frm[amountontax];
                    model.InventoryTaxDetails.Tax = frm[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frm[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }

            model.QuotationDetails.Status = "Active";
            model.QuotationDetails.ModifiedOn = DateTime.Now;
            model.QuotationDetails.TotalAmount = frm["Total_Amount_Value"];
            model.QuotationDetails.GrandTotal = frm["Grand_Total_Value"];
            TempData["PreviousQuotation"] = model.QuotationDetails.QuotNo;
            _QuotationService.Update(model.QuotationDetails);
            string QuotId = Encode(model.QuotationDetails.Id.ToString());
            return RedirectToAction("QuotationDetails/" + QuotId, "ProcessingQuotation");
        }

        [HttpGet]
        public ActionResult ListOfQuotation()
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
        public ActionResult QuotationPrint()
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
        public ActionResult GetQuotationDetailsForPrint(string QuotNo)
        {
            MainApplication model = new MainApplication();
            model.QuotationDetails = _QuotationService.GetQuotByCode(QuotNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.QuotationDetails.QuotNo);
            model.QuotationItemList = _QuotationItemService.GetDetailsByQuotNo(QuotNo);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetNonInventoryItemsForQuotation()
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
        public JsonResult GetNonInvItemdetailsByItemCodeforQuot(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            var data = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            //GET SUBCATEGORY USING ITEMCODE
            string subcat = data.itemSubCategory;
            double taxPercent = 0;
            if (ClientState == "Maharashtra")
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", data.itemSubCategory);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell == "Reseller")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            return Json(new { discount, discountrps, taxPercent, subcat, data.NumberType, data.itemCode, data.itemName, data.description, data.colorCode, data.unit, data.costprice, data.sellingprice, data.mrp, data.typeOfMaterial, data.designCode, data.designName, data.brandName, data.size }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ItemList(string QuotNo)
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = new SalesOrder();
            model.SalesOrderItemDetails = new SalesOrderItem();
            var quotdetail = _QuotationService.GetQuotByCode(QuotNo);
            if (quotdetail.ProcessedIn.Contains("SO"))
            {
                model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByQuotNo(QuotNo);
                List<SalesOrderItem> SalesOrderItemList = new List<SalesOrderItem>();

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@quotno", QuotNo)
                };

                string query = "GetItemCodesByQuotAndItemCode" + " " + "@quotno";
                var SOList = _SalesOrderItemService.GetItemsByQuotItemCode(query, para);

                foreach (var details in SOList)
                {
                    double qty = 0;
                    var SOItemList = _SalesOrderItemService.GetRowsByItemCodeAndQuot(details.ItemCode, QuotNo);
                    var SOItemDetails = _SalesOrderItemService.GetDetailsByItemCodeAndQuot(details.ItemCode, QuotNo);
                    foreach (var data in SOItemList)
                    {
                        qty = qty + Convert.ToDouble(data.Quantity);
                    }

                    //CALCULATE TOTAL STOCK QUANTITY..
                    double? Quantity;
                    double? stkdisqty = 0;
                    double? shopstkqty = 0;
                    var stkdisdata = _StockItemDistributionService.GetRowsByItemCode(details.ItemCode);
                    var shopstkdata = _ShopStockService.GetRowsByItemCode(details.ItemCode);

                    //calculate sum of quantity in stock distribution
                    foreach (var data1 in stkdisdata)
                    {
                        stkdisqty = stkdisqty + data1.ItemQuantity;
                    }
                    //calculate sum of quantity in shop stock
                    foreach (var data2 in shopstkdata)
                    {
                        shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                    }
                    //calulate total quantity
                    Quantity = stkdisqty + shopstkqty;
                    SOItemDetails.ReadOnlyTotalStockQuantity = Quantity;

                    SOItemDetails.SOBalance = SOItemDetails.QuotQuantity - qty;
                    if (SOItemDetails.SOBalance < 0)
                    {
                        SOItemDetails.SOBalance = 0;
                    }
                    SalesOrderItemList.Add(SOItemDetails);
                }
                model.SalesOrderItemList = SalesOrderItemList;
                model.QuotationDetails = quotdetail;
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult CompleteQuotation(string QuotNo)
        {
            var details = _QuotationService.GetQuotByCode(QuotNo);
            details.SendingStatus = "Completed";
            _QuotationService.Update(details);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNonInvItemsWOSubcategory()
        {
            var data = _NonInventoryItemService.GetAll();
            var select = data.Select(m => new SelectListItem
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(select, JsonRequestBehavior.AllowGet);
        }

        // ****************************************** SALES ORDER ***************************************

        //CREATE ORDER PROCESSING
        [HttpGet]
        public ActionResult SalesOrder()
        {
            MainApplication model = new MainApplication()
            {
                SalesOrderDetails = new SalesOrder(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            //CREATE CONSIGNMENT CODE
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesOrder = _SalesOrderService.GetLastSalerOrderByFinYr(FinYr, shopcode);
            string SalesOrderCode = string.Empty;
            int SalesOrderLength = 0;
            int SalesOrderNo = 0;
            if (LastSalesOrder == null)
            {
                SalesOrderLength = 1;
                SalesOrderNo = 1;
            }
            else
            {
                int SalesIndex = LastSalesOrder.OrderNo.LastIndexOf('S');
                SalesOrderCode = LastSalesOrder.OrderNo.Substring(SalesIndex + 2, 6);
                SalesOrderLength = (Convert.ToInt32(SalesOrderCode) + 1).ToString().Length;
                SalesOrderNo = Convert.ToInt32(SalesOrderCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            SalesOrderCode = _UtilityService.getName(ShortCode + "/SO", SalesOrderLength, SalesOrderNo);
            SalesOrderCode = SalesOrderCode + FinYr;
            model.SalesOrderDetails.OrderNo = SalesOrderCode;
            TempData["PreviousSalesOrder"] = SalesOrderCode;

            //CONCAT GODOWNCODE AND GODOWNNAME
            var godownlist = _GodownService.GetGodownNames();
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
            model.EmpList = _employeeservice.getAllemployees();

            return View(model);
        }

        //ITEM LIST FROM STOCK DISTRIBUTION AND SHOP STOCK
        [HttpGet]
        public JsonResult GetItemsFromShopStockAndStkDistrbtn(string ItemType, string LoginShop)
        {
            if (ItemType == "Inventory")
            {
                var stkdistrbtnlist = _ShopStockService.GetRowsByShopCode(LoginShop);
                var items = stkdistrbtnlist.Select(m => new SelectListItem
                {
                    Text = m.ItemName,
                    Value = m.ItemCode
                });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var noninvitems = _NonInventoryItemService.GetAll();
                var items = noninvitems.Select(m => new SelectListItem
                {
                    Text = m.itemName,
                    Value = m.itemCode
                });
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        //ITEM LIST FROM ITEM MASTER
        [HttpGet]
        public JsonResult GetItemsFromItemMaster()
        {
            var itemlist = _ItemService.getAllItems();
            var items = itemlist.Select(m => new SelectListItem
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //ITEM LIST NON-INVENTORY
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

        //GET ITEM DETAILS ON ITEM DDL CHANGE EVENT
        [HttpGet]
        public JsonResult GetItemdetailsByItemCode(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            //GET SUBCATEGORY USING ITEMCODE 
            var itemdetails = _ItemService.GetDescriptionByItemCode(ItemCode);
            string subcat = itemdetails.itemSubCategory;
            string costprice = itemdetails.costprice;
            double taxPercent = 0;
            if (ClientState == "Maharashtra")
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell == "Reseller")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }

            //CALCULATE TOTAL QUANTITY..
            double Qty;
            double gdwnstkqty = 0;
            double shopstkqty = 0;
            var gdwnstkdata = _GodownStockService.GetRowsByItemCode(ItemCode);
            var shopstkdata = _ShopStockService.GetRowsByItemCode(ItemCode);

            //calculate sum of quantity in godown stock
            foreach (var data1 in gdwnstkdata)
            {
                gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
            }
            //calculate sum of quantity in shop stock
            foreach (var data2 in shopstkdata)
            {
                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
            }
            //calulate total quantity
            Qty = gdwnstkqty + shopstkqty;

            var data = _StockItemDistributionService.GetDetailsByItemCode(ItemCode);
            if (data != null)
            {
                return Json(new { discount, discountrps, taxPercent, data.ItemCode, data.ItemName, data.ItemQuantity, data.Description, data.Color, data.Unit, data.NumberType, costprice, data.SellingPrice, data.MRP, data.Material, data.DesignCode, data.DesignName, data.Barcode, data.Brand, data.Size, Qty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

        }

        //GET NON-INVENTORY ITEM DETAILS ON ITEM DDL CHANGE EVENT
        [HttpGet]
        public JsonResult GetNonInvItemdetailsByItemCode(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            //GET SUBCATEGORY USING ITEMCODE 
            string subcat = _NonInventoryItemService.GetDetailsByItemCode(ItemCode).itemSubCategory;

            double taxPercent = 0;
            if (ClientState == "Maharashtra")
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell == "Reseller")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }

            var data = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            //var design = 
            if (data != null)
            {
                return Json(new { discount, discountrps, subcat, taxPercent, data.itemCode, data.itemName, data.description, data.colorCode, data.unit, data.NumberType, data.costprice, data.sellingprice, data.mrp, data.typeOfMaterial, data.designCode, data.designName, data.size, data.brandName }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        //AUTO COMPLETE CLIENT NAME
        [HttpGet]
        public JsonResult GetClientNames(string term)
        {
            var ClientData = _ClientMasterService.GetClientNames(term);
            List<string> names = new List<string>();
            foreach (var item in ClientData)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE CLIENT NAME
        [HttpGet]
        public JsonResult GetActiveAndMaharashtraClients(string term)
        {
            var ClientData = _ClientMasterService.GetActiveAndMaharashtraClients(term);
            List<string> names = new List<string>();
            foreach (var item in ClientData)
            {
                names.Add(item.ClientName);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTransportNames(string mode)
        {
            var transport = _TransportService.GetTransportByMode(mode);
            var list = transport.Select(m => new SelectListItem
            {
                Text = m.TransportName,
                Value = m.TransportName
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTransportDetails(string name)
        {
            var transport = _TransportService.GetByName(name);
            return Json(new { name = transport.TransportName, no = transport.ContactNo1 }, JsonRequestBehavior.AllowGet);
        }

        //GET ALL ITEMS
        [HttpGet]
        public JsonResult GetItemList()
        {
            var itemlist = _ItemService.getAllItems();
            var items = itemlist.Select(m => new SelectListItem
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        //GET EMPLOPYEE DETAILS BY EMPLOYEE NAME
        [HttpGet]
        public JsonResult GetEmployeeDetails(string name)
        {
            var data = _employeeservice.getEmpByName(name);
            return Json(new { data.Address, data.MobileNo, data.Designation, data.Email }, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE QUOTATION NO
        [HttpGet]
        public JsonResult GetQuotationNo(string term)
        {
            var quotationdata = _QuotationService.GetActiveQuotNo(term);
            List<string> names = new List<string>();
            foreach (var item in quotationdata)
            {
                names.Add(item.QuotNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE SALES PERSON NAME
        [HttpGet]
        public JsonResult GetSalesManName(string term)
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            var employeedata = _employeeservice.GetEmpByNameAndSalesDepartment(term);
            List<string> names = new List<string>();
            foreach (var item in employeedata)
            {
                names.Add(item.Name);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET QUOTATIONS DETAILS ON QUOT NO CHANGE EVENT
        [HttpGet]
        public JsonResult GetQuotationDetails(string Quot)
        {
            string QuotDate;
            var data = _QuotationService.GetQuotByCode(Quot);
            QuotDate = Convert.ToDateTime(data.QuotDate).ToShortDateString();
            TempData["SOQuotDate"] = data.QuotDate;
            return Json(new
            {
                data.SalesPersonName,
                data.SalesPersonAddress,
                data.SalesPersonContact,
                data.SalesPersonDesignation,
                data.SalesPersonEmail,
                data.ClientName,
                data.ClientAddress,
                data.ClientEmail,
                data.ClientContactNo,
                data.ClientState,
                data.ClientType,
                data.FormType,
                data.ConsumeResell,
                data.TransportMode,
                data.TransportName,
                data.TransportContactNo,
                QuotDate,
                data.PackAndForwd,
                data.TotalDiscount,
            }, JsonRequestBehavior.AllowGet);
        }

        //GET ITEM LIST FROM QUOTATION
        [HttpGet]
        public ActionResult GetItemListFromQuotation(string QuotNo)
        {
            MainApplication model = new MainApplication();
            model.QuotationItemList = _QuotationItemService.GetDetailsByQuotNo(QuotNo);
            var quotitemlist = model.QuotationItemList;
            quotitemlist.ToList().Clear();
            foreach (var data in model.QuotationItemList)
            {
                double? discount = 0;
                double? discountrps = 0;
                var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(data.ItemCode);
                if (discountdetails != null)
                {
                    discount = discountdetails.DiscInPercentage;
                    discountrps = discountdetails.DiscInRupees;
                    data.DiscountPercent = discount;
                    data.DiscountRPS = discountrps;
                }
                //CALCULATE TOTAL STOCK QUANTITY..
                double Qty;
                double gdwnstkqty = 0;
                double shopstkqty = 0;
                var gdwnstkdata = _GodownStockService.GetRowsByItemCode(data.ItemCode);
                var shopstkdata = _ShopStockService.GetRowsByItemCode(data.ItemCode);

                //calculate sum of quantity in godown stock
                foreach (var data1 in gdwnstkdata)
                {
                    gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                }
                //calculate sum of quantity in shop stock
                foreach (var data2 in shopstkdata)
                {
                    shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                }
                //calulate total quantity
                Qty = gdwnstkqty + shopstkqty;
                data.ReadOnlyTotalStockQuantity = Qty;
                quotitemlist.ToList().Add(data);
            }
            model.QuotationItemList.ToList().Clear();
            model.QuotationItemList = quotitemlist;

            TempData["QuotOrSalesItems"] = quotitemlist;
            return View(model);
        }

        //GET TAX SUMMARY FROM QUOTATIONS
        [HttpGet]
        public JsonResult GetTaxFromQuotation(string Quotation)
        {
            var list = _InventoryTaxService.GetTaxesByCode(Quotation);
            var taxlist = list.Select(m => new InventoryTax
            {
                Tax = m.Tax,
                Amount = m.Amount,
                TaxAmount = m.TaxAmount
            });
            return Json(taxlist, JsonRequestBehavior.AllowGet);
        }

        //GET LOGIN EMPLOPYEE DETAILS BY EMPLOYEE EMAIL
        [HttpGet]
        public JsonResult GetPreparedByEmpDetails(string email)
        {
            var data = _EmployeeMasterService.getEmpByEmail(email);
            return Json(new { data.Name, data.MobileNo }, JsonRequestBehavior.AllowGet);
        }

        // SAVE DATA IN SALES ORDER
        [HttpPost]
        public ActionResult SalesOrder(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                SalesOrderItemDetails = new SalesOrderItem(),
            };

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastSalesOrder = _SalesOrderService.GetLastSalerOrderByFinYr(FinYr, shopcode);
            string SalesOrderCode = string.Empty;
            int SalesOrderLength = 0;
            int SalesOrderNo = 0;
            if (LastSalesOrder == null)
            {
                SalesOrderLength = 1;
                SalesOrderNo = 1;
            }
            else
            {
                int SalesIndex = LastSalesOrder.OrderNo.LastIndexOf('S');
                SalesOrderCode = LastSalesOrder.OrderNo.Substring(SalesIndex + 2, 6);
                SalesOrderLength = (Convert.ToInt32(SalesOrderCode) + 1).ToString().Length;
                SalesOrderNo = Convert.ToInt32(SalesOrderCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            SalesOrderCode = _UtilityService.getName(ShortCode + "/SO", SalesOrderLength, SalesOrderNo);
            SalesOrderCode = SalesOrderCode + FinYr;


            // SAVE ORDER PROCESSING DATA
            if (mainapp.SalesOrderDetails.QuotationNo != null)
            {
                var salesorderlist = _SalesOrderService.GetDetailsByQuotNo(mainapp.SalesOrderDetails.QuotationNo);
                foreach (var row in salesorderlist)
                {
                    if (row.Editable == "Yes")
                    {
                        row.Editable = "No";
                        _SalesOrderService.Update(row);
                    }
                }
                var QuotDetails = _QuotationService.GetDetailsByQuotNo(mainapp.SalesOrderDetails.QuotationNo);
                QuotDetails.ProcessedIn = SalesOrderCode;
                _QuotationService.Update(QuotDetails);
            }

            // SAVE ORDER PROCESSING DATA
            if (mainapp.SalesOrderDetails.QuotationNo != null)
            {
                mainapp.SalesOrderDetails.QuotationDate = Convert.ToDateTime(TempData["SOQuotDate"]);
            }
            mainapp.SalesOrderDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            mainapp.SalesOrderDetails.TotalAdvancePayment = mainapp.SalesOrderDetails.AdvancePayment;
            mainapp.SalesOrderDetails.RemainingAdvance = mainapp.SalesOrderDetails.AdvancePayment;
            mainapp.SalesOrderDetails.RefundAmount = 0;
            mainapp.SalesOrderDetails.Date = DateTime.Now;
            mainapp.SalesOrderDetails.Status = "Active";
            mainapp.SalesOrderDetails.ModifiedOn = DateTime.Now;
            mainapp.SalesOrderDetails.PreparedTime = DateTime.Now.ToLongTimeString();
            mainapp.SalesOrderDetails.SendingStatus = "Pending";
            mainapp.SalesOrderDetails.Editable = "Yes";
            mainapp.SalesOrderDetails.ProcessedIn = "Fresh";
            mainapp.SalesOrderDetails.CashierStatus = "Active";
            mainapp.SalesOrderDetails.AdjustedAmount = 0;
            mainapp.SalesOrderDetails.RefundToClient = 0;
            _SalesOrderService.Create(mainapp.SalesOrderDetails);

            int RowCount = Convert.ToInt32(frmcol["hdnRowCount"]);

            // SAVE ORDER PROCESSING ITEMS WITH QUOTATION..
            if (mainapp.SalesOrderDetails.QuotationNo != null)
            {
                int countervalue = 0;
                model.QuotationItemList = TempData["QuotOrSalesItems"] as IEnumerable<QuotationItem>;
                countervalue = model.QuotationItemList.Count();
                int count = 1;
                foreach (var data in model.QuotationItemList)
                {
                    string sellingprice = "SellingPrice" + count;
                    string quantity = "Quantity" + count;
                    string quotqty = "QuotQtyVal" + count;
                    string balance = "BalanceVal" + count;
                    string barcodevalue = "BarcodeValue" + count;
                    string amountvalue = "AmountValue" + count;
                    string disinrs = "DisRs" + count;
                    string disinper = "DisPer" + count;
                    string proptax = "proportionatehdn" + count;
                    string proptaxamt = "proportionatetaxamount" + count;
                    string narration = "Narration" + count;
                    string itemtaxamt = "ItemTaxAmount" + count;
                    string mrp = "MRP" + count;

                    if (Convert.ToDouble(frmcol[quantity]) != 0)
                    {
                        model.SalesOrderItemDetails.ItemName = data.ItemName;
                        model.SalesOrderItemDetails.ItemCode = data.ItemCode;
                        model.SalesOrderItemDetails.ItemType = data.ItemType;
                        model.SalesOrderItemDetails.Description = data.Description;
                        model.SalesOrderItemDetails.Color = data.Color;
                        model.SalesOrderItemDetails.Unit = data.Unit;
                        model.SalesOrderItemDetails.NumberType = data.NumberType;
                        model.SalesOrderItemDetails.Material = data.Material;
                        model.SalesOrderItemDetails.DesignCode = data.DesignCode;
                        model.SalesOrderItemDetails.DesignName = data.DesignName;
                        model.SalesOrderItemDetails.Brand = data.Brand;
                        model.SalesOrderItemDetails.Size = data.Size;
                        model.SalesOrderItemDetails.ItemTax = data.ItemTax;
                        model.SalesOrderItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        model.SalesOrderItemDetails.Barcode = frmcol[barcodevalue];
                        model.SalesOrderItemDetails.Narration = frmcol[narration];
                        model.SalesOrderItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.SalesOrderItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);

                        if (frmcol[disinrs] == "")
                        {
                            model.SalesOrderItemDetails.DisInRs = "0";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInRs = frmcol[disinrs];
                        }
                        if (frmcol[disinper] == "")
                        {
                            model.SalesOrderItemDetails.DisInPercent = "0";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInPercent = frmcol[disinper];
                        }
                        model.SalesOrderItemDetails.Amount = Convert.ToDouble(frmcol[amountvalue]);
                        model.SalesOrderItemDetails.PropTax = Convert.ToDouble(frmcol[proptax]);
                        model.SalesOrderItemDetails.PropTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        model.SalesOrderItemDetails.OrderNo = SalesOrderCode;
                        model.SalesOrderItemDetails.QuotationNo = mainapp.SalesOrderDetails.QuotationNo;
                        model.SalesOrderItemDetails.QuotQuantity = Convert.ToDouble(frmcol[quotqty]);

                        model.SalesOrderItemDetails.SOBalance = Convert.ToDouble(frmcol[quantity]);

                        model.SalesOrderItemDetails.Status = "Active";
                        model.SalesOrderItemDetails.ModifiedOn = DateTime.Now;
                        model.SalesOrderItemDetails.ProcessedIn = "Fresh";
                        _SalesOrderItemService.Create(model.SalesOrderItemDetails);

                        var details = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(data.ItemCode, data.QuotNo);
                        details.Balance = details.Balance - model.SalesOrderItemDetails.Quantity;
                        if (details.Balance <= 0)
                        {
                            details.Balance = 0;
                            details.Status = "InActive";
                        }
                        details.ProcessedIn = SalesOrderCode;
                        _QuotationItemService.Update(details);
                        
                        
                        //data.Balance = data.Balance - model.SalesOrderItemDetails.Quantity;
                        //if (data.Balance <= 0)
                        //{
                        //    data.Balance = 0;
                        //    data.Status = "InActive";
                        //}
                        //data.ProcessedIn = SalesOrderCode;
                        //_QuotationItemService.Update(data);

                        var QuotDetails = _QuotationService.GetDetailsByQuotNo(mainapp.SalesOrderDetails.QuotationNo);
                        var QuotItemList = _QuotationItemService.GetAllActiveItemsByQuotNo(mainapp.SalesOrderDetails.QuotationNo);
                        if (QuotItemList.Count() == 0)
                        {
                            QuotDetails.Status = "InActive";
                            _QuotationService.Update(QuotDetails);
                        }
                    }
                    count = count + 1;
                }

                //save processed in quotation..
                var quotdetails = _QuotationService.GetQuotByCode(mainapp.SalesOrderDetails.QuotationNo);
                quotdetails.ProcessedIn = SalesOrderCode;
                quotdetails.Editable = "No";
                quotdetails.SendingStatus = "InComplete";
                _QuotationService.Update(quotdetails);

                //SAVE INVENTORY + NEW ITEM
                for (int i = countervalue + 1; i <= RowCount; i++)
                {
                    string item = "ItemCode" + i;
                    string itemname = "ItemName" + i;
                    string descvalue = "DescriptionValue" + i;
                    string designcode = "DesignCode" + i;
                    string designname = "DesignName" + i;
                    string brand = "Brand" + i;
                    string size = "Size" + i;
                    string colorvalue = "ColorValue" + i;
                    string materialvalue = "MaterialValue" + i;
                    string unitvalue = "UnitValue" + i;
                    string numbertype = "NumberType" + i;
                    string amountvalue = "AmountValue" + i;
                    string itemtaxvalue = "ItemTaxValue" + i;
                    string itemtaxamt = "ItemTaxAmount" + i;
                    string proptaxvalue = "proportionatehdn" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    string narration = "Narration" + i;
                    string quantity = "Quantity" + i;
                    string quotqty = "QuotQtyVal" + i;
                    string balance = "BalanceVal" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string disper = "DisPer" + i;
                    string disrs = "DisRs" + i;
                    string type = "Type" + i;
                    string barcodevalue = "BarcodeValue" + i;

                    if (frmcol[item] != null)
                    {
                        model.SalesOrderItemDetails = new SalesOrderItem();
                        model.SalesOrderItemDetails.ItemCode = frmcol[item];
                        model.SalesOrderItemDetails.ItemName = frmcol[itemname];
                        model.SalesOrderItemDetails.ItemType = frmcol[type];
                        model.SalesOrderItemDetails.Description = frmcol[descvalue];
                        model.SalesOrderItemDetails.Color = frmcol[colorvalue];
                        model.SalesOrderItemDetails.Unit = frmcol[unitvalue];
                        model.SalesOrderItemDetails.NumberType = frmcol[numbertype];
                        model.SalesOrderItemDetails.Material = frmcol[materialvalue];
                        model.SalesOrderItemDetails.DesignCode = frmcol[designcode];
                        model.SalesOrderItemDetails.DesignName = frmcol[designname];
                        model.SalesOrderItemDetails.Brand = frmcol[brand];
                        model.SalesOrderItemDetails.Size = frmcol[size];
                        model.SalesOrderItemDetails.ItemTax = frmcol[itemtaxvalue];
                        model.SalesOrderItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        model.SalesOrderItemDetails.Narration = frmcol[narration];
                        model.SalesOrderItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.SOBalance = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.SalesOrderItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);

                        if (frmcol[disrs] == "")
                        {
                            model.SalesOrderItemDetails.DisInRs = "0";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInRs = frmcol[disrs];
                        }
                        if (frmcol[disper] == "")
                        {
                            model.SalesOrderItemDetails.DisInPercent = "0";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInPercent = frmcol[disper];
                        }
                        model.SalesOrderItemDetails.Amount = Convert.ToDouble(frmcol[amountvalue]);
                        model.SalesOrderItemDetails.PropTax = Convert.ToDouble(frmcol[proptaxvalue]);
                        model.SalesOrderItemDetails.PropTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        model.SalesOrderItemDetails.OrderNo = SalesOrderCode;
                        model.SalesOrderItemDetails.QuotQuantity = Convert.ToDouble(frmcol[quotqty]);
                        model.SalesOrderItemDetails.Barcode = frmcol[barcodevalue];
                        model.SalesOrderItemDetails.Status = "Active";
                        model.SalesOrderItemDetails.ProcessedIn = "Fresh";
                        model.SalesOrderItemDetails.ModifiedOn = DateTime.Now;
                        _SalesOrderItemService.Create(model.SalesOrderItemDetails);
                    }
                }
            }
            // SAVE ORDER PROCESSING ITEMS WITHOUT QUOTATION..
            else
            {
                for (int i = 1; i <= RowCount; i++)
                {
                    string itemcode = "ItemCode" + i;
                    string itemname = "ItemName" + i;
                    string quantity = "Quantity" + i;
                    string unit = "UnitValue" + i;
                    string numbertype = "NumberType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string amount = "AmountValue" + i;
                    string barcodevalue = "BarcodeValue" + i;
                    string description = "DescriptionValue" + i;
                    string material = "MaterialValue" + i;
                    string color = "ColorValue" + i;
                    string designcode = "DesignCode" + i;
                    string designname = "DesignName" + i;
                    string brand = "Brand" + i;
                    string size = "Size" + i;
                    string itemTax = "ItemTaxValue" + i;
                    string disinrs = "DisRs" + i;
                    string disinper = "DisPer" + i;
                    string proptax = "proportionatehdn" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    string narration = "Narration" + i;
                    string type = "Type" + i;
                    string itemtaxamt = "ItemTaxAmount" + i;

                    if (frmcol[itemcode] != null)
                    {
                        model.SalesOrderItemDetails.ItemName = frmcol[itemname];
                        model.SalesOrderItemDetails.ItemCode = frmcol[itemcode];
                        model.SalesOrderItemDetails.ItemType = frmcol[type];
                        model.SalesOrderItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.SOBalance = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.Unit = frmcol[unit];
                        model.SalesOrderItemDetails.NumberType = frmcol[numbertype];
                        model.SalesOrderItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.SalesOrderItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        model.SalesOrderItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        model.SalesOrderItemDetails.Description = frmcol[description];
                        model.SalesOrderItemDetails.Material = frmcol[material];
                        model.SalesOrderItemDetails.Color = frmcol[color];
                        model.SalesOrderItemDetails.DesignCode = frmcol[designcode];
                        model.SalesOrderItemDetails.DesignName = frmcol[designname];
                        model.SalesOrderItemDetails.Brand = frmcol[brand];
                        model.SalesOrderItemDetails.Size = frmcol[size];
                        model.SalesOrderItemDetails.ItemTax = frmcol[itemTax];
                        model.SalesOrderItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        if (frmcol[disinrs] == "")
                        {
                            model.SalesOrderItemDetails.DisInRs = "0";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInRs = frmcol[disinrs];
                        }
                        if (frmcol[disinper] == "")
                        {
                            model.SalesOrderItemDetails.DisInPercent = "0";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInPercent = frmcol[disinper];
                        }
                        model.SalesOrderItemDetails.PropTax = Convert.ToDouble(frmcol[proptax]);
                        model.SalesOrderItemDetails.PropTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        model.SalesOrderItemDetails.Narration = frmcol[narration];
                        model.SalesOrderItemDetails.Barcode = frmcol[barcodevalue];
                        model.SalesOrderItemDetails.OrderNo = SalesOrderCode;
                        model.SalesOrderItemDetails.QuotationNo = mainapp.SalesOrderDetails.QuotationNo;
                        model.SalesOrderItemDetails.Status = "Active";
                        model.SalesOrderItemDetails.ProcessedIn = "Fresh";
                        model.SalesOrderItemDetails.ModifiedOn = DateTime.Now;
                        _SalesOrderItemService.Create(model.SalesOrderItemDetails);
                    }
                }
            }

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = SalesOrderCode;
                    model.InventoryTaxDetails.Amount = frmcol[amountontax];
                    model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }

            var id = _SalesOrderService.GetLastRow().Id;
            string SOId = Encode(id.ToString());
            return RedirectToAction("SalesOrderDetails/" + SOId, "ProcessingQuotation");
        }

        //DISPLAY ITEM DETAILS POP UP PAGE ON ITEM CHANGE EVENT
        [HttpGet]
        public ActionResult SOItemDetailsPopUp(string ItemCode)
        {
            MainApplication model = new MainApplication();
            if (ItemCode.Contains("NI"))
            {
                model.NonInventoryItemDetails = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            }
            else
            {
                model.ItemDetails = _ItemService.GetDescriptionByItemCode(ItemCode);
            }
            return View(model);
        }


        //DETAILS PAGE OF SALES ORDER
        [HttpGet]
        public ActionResult SalesOrderDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesOrderDetails.OrderNo);
            string previoussalesorder = TempData["PreviousSalesOrder"].ToString();
            if (previoussalesorder != model.SalesOrderDetails.OrderNo)
            {
                ViewData["SalesOrderChanged"] = previoussalesorder + " is replaced with " + model.SalesOrderDetails.OrderNo + " because " + previoussalesorder + " is acquired by another person";
            }
            TempData["PreviousSalesOrder"] = previoussalesorder;
            return View(model);
        }

        //PRINT PAGE OF SALES ORDER WITH SELLING PRICE
        [HttpGet]
        public ActionResult PrintSalesOrderWithSellingPrice(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        //PRINT PAGE OF SALES ORDER WITH MRP
        [HttpGet]
        public ActionResult PrintSalesOrderWithMRP(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        //PRINT PAGE OF SALES ORDER WITH SELLING PRICE
        [HttpGet]
        public ActionResult PrintOrderSellingPricePrePrinted(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        //PRINT PAGE OF SALES ORDER WITH MRP
        [HttpGet]
        public ActionResult PrintOrderMRPPrePrinted(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        //PRINT PAGE OF SALES ORDER WITH SELLING PRICE
        [HttpGet]
        public ActionResult PrintOrderSellingPriceLH(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        //PRINT PAGE OF SALES ORDER WITH MRP
        [HttpGet]
        public ActionResult PrintOrderMRPLH(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.SalesOrderDetails = _SalesOrderService.GetDetailsById(Id);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(model.SalesOrderDetails.OrderNo);
            return View(model);
        }

        //EDIT SALES ORDER
        [HttpGet]
        public ActionResult SalesOrderUpdate()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE SALES ORDER NO
        [HttpGet]
        public JsonResult GetSalesOrderNo(string term)
        {
            var salesdata = _SalesOrderService.GetOrderNos(term);
            List<string> names = new List<string>();
            foreach (var item in salesdata)
            {
                names.Add(item.OrderNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //AUTO COMPLETE SALES ORDER NO
        [HttpGet]
        public JsonResult GetSalesOrderNoForPrint(string term)
        {
            var salesdata = _SalesOrderService.GetAllSalesOrders(term);
            List<string> names = new List<string>();
            foreach (var item in salesdata)
            {
                names.Add(item.OrderNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        //GET STOCK QUANTITY
        [HttpGet]
        public JsonResult GetStockQuantity(string ItemCode)
        {
            //CALCULATE TOTAL QUANTITY..
            double? Qty;
            double? gdstkqty = 0;
            double? shopstkqty = 0;
            var gdstkdata = _GodownStockService.GetRowsByItemCode(ItemCode);
            var shopstkdata = _ShopStockService.GetRowsByItemCode(ItemCode);

            //calculate sum of quantity in godown stock
            foreach (var data1 in gdstkdata)
            {
                gdstkqty = gdstkqty + data1.Quantity;
            }
            //calculate sum of quantity in shop stock
            foreach (var data2 in shopstkdata)
            {
                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
            }
            //calulate total quantity
            Qty = gdstkqty + shopstkqty;

            return Json(Qty, JsonRequestBehavior.AllowGet);
        }

        //GET DETAILS BY SALES ORDER NO
        [HttpGet]
        public ActionResult GetDetailsBySalesOrderNo(string orderno)
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = _SalesOrderService.GetDetailsByOrderNo(orderno);
            //TempData["SODetails"] = model.SalesOrderDetails.QuotationDate;
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(orderno);
            var SoItemList = model.SalesOrderItemList;
            SoItemList.ToList().Clear();
            foreach (var item in model.SalesOrderItemList)
            {
                var quotitem = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(item.ItemCode, item.QuotationNo);
                if (quotitem != null)
                {
                    item.ReadOnlyQuotbalance = quotitem.Balance;
                }
                else
                {
                    item.ReadOnlyQuotbalance = 0;
                }
                SoItemList.ToList().Add(item);
            }
            model.SalesOrderItemList = SoItemList;
            TempData["SOItemList"] = model.SalesOrderItemList;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(orderno);
            if (model.InventoryTaxList.Count() != 0)
            {
                TempData["SOTaxList"] = model.InventoryTaxList;
            }
            //model.NonInventoryItemList = _NonInventoryItemService.GetItemsbySalesOrderNo(orderno);
            model.EmpList = _employeeservice.getAllemployees();
            return View(model);
        }


        //UPDATE SALES ORDER DETAILS
        [HttpPost]
        public ActionResult GetDetailsBySalesOrderNo(MainApplication model, FormCollection frmcol)
        {
            var SOItemList = TempData["SOItemList"] as IEnumerable<SalesOrderItem>;
            foreach (var deleteitem in SOItemList)
            {
                var deleterow = _SalesOrderItemService.GetDetailsById(deleteitem.Id);
                _SalesOrderItemService.Delete(deleterow);
            }

            var sotaxlist = TempData["SOTaxList"] as IEnumerable<InventoryTax>;
            if (sotaxlist != null)
            {
                foreach (var deletetax in sotaxlist)
                {
                    var delete = _InventoryTaxService.GetTaxById(deletetax.Id);
                    _InventoryTaxService.Delete(delete);
                }
            }

            string RowCount = frmcol["hdnRowCount"];
            if (!string.IsNullOrEmpty(RowCount))
            {
                int count = Convert.ToInt32(RowCount);
                for (int i = 1; i <= count; i++)
                {
                    string barcode = "BarcodeValue" + i;
                    string itemcode = "ItemCode" + i;
                    string itemname = "ItemName" + i;
                    string itemtype = "ItemType" + i;
                    string quotno = "QuotNo" + i;
                    string quantity = "Quantity" + i;
                    string quotquantity = "QuotQtyValue" + i;
                    string unit = "UnitValue" + i;
                    string numbertype = "NumberType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string amount = "AmountValue" + i;
                    string description = "DescriptionValue" + i;
                    string material = "MaterialValue" + i;
                    string color = "ColorValue" + i;
                    string designcode = "DesignCode" + i;
                    string designname = "DesignName" + i;
                    string brand = "Brand" + i;
                    string size = "Size" + i;
                    string itemTax = "ItemTaxValue" + i;
                    string itemtaxamt = "ItemTaxAmount" + i;
                    string disinrs = "DisRs" + i;
                    string disinper = "DisPer" + i;
                    string proptax = "proportionatetaxvalue" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    string narration = "Narration" + i;
                    string bal = "BalanceVal" + i;

                    if (frmcol[itemcode] != null)
                    {
                        model.SalesOrderItemDetails = new SalesOrderItem();

                        model.SalesOrderItemDetails.Barcode = frmcol[barcode];
                        model.SalesOrderItemDetails.ItemCode = frmcol[itemcode];
                        model.SalesOrderItemDetails.ItemType = frmcol[itemtype];
                        model.SalesOrderItemDetails.Quantity = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.SOBalance = Convert.ToDouble(frmcol[quantity]);
                        model.SalesOrderItemDetails.Unit = frmcol[unit];
                        model.SalesOrderItemDetails.NumberType = frmcol[numbertype];
                        model.SalesOrderItemDetails.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        model.SalesOrderItemDetails.MRP = Convert.ToDouble(frmcol[mrp]);
                        model.SalesOrderItemDetails.Amount = Convert.ToDouble(frmcol[amount]);
                        model.SalesOrderItemDetails.Description = frmcol[description];
                        model.SalesOrderItemDetails.Material = frmcol[material];
                        model.SalesOrderItemDetails.Color = frmcol[color];
                        model.SalesOrderItemDetails.DesignCode = frmcol[designcode];
                        model.SalesOrderItemDetails.DesignName = frmcol[designname];
                        model.SalesOrderItemDetails.Brand = frmcol[brand];
                        model.SalesOrderItemDetails.Size = frmcol[size];
                        model.SalesOrderItemDetails.ItemTax = frmcol[itemTax];
                        model.SalesOrderItemDetails.ItemTaxAmt = frmcol[itemtaxamt];
                        if (frmcol[disinrs] == "")
                        {
                            model.SalesOrderItemDetails.DisInRs = "0.00";
                        }
                        else
                        {
                            model.SalesOrderItemDetails.DisInRs = frmcol[disinrs];
                        }
                        model.SalesOrderItemDetails.DisInPercent = frmcol[disinper];
                        model.SalesOrderItemDetails.PropTax = Convert.ToDouble(frmcol[proptax]);
                        if (frmcol[proptaxamt] != "")
                        {
                            model.SalesOrderItemDetails.PropTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        }
                        else
                        {
                            model.SalesOrderItemDetails.PropTaxAmt = 0;
                        }
                        model.SalesOrderItemDetails.Narration = frmcol[narration];
                        model.SalesOrderItemDetails.ItemName = frmcol[itemname];

                        if (model.SalesOrderDetails.QuotationNo != null)
                        {
                            model.SalesOrderItemDetails.QuotQuantity = Convert.ToDouble(frmcol[quotquantity]);
                        }

                        if (frmcol[quotno] != null && frmcol[quotno] != "")
                        {
                            model.SalesOrderItemDetails.QuotationNo = model.SalesOrderDetails.QuotationNo;
                            var quotitemdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frmcol[itemcode], frmcol[quotno]);
                            quotitemdata.Balance = Convert.ToDouble(frmcol[bal]);

                            if (quotitemdata.Balance != 0)
                            {
                                if (quotitemdata.Status == "InActive")
                                {
                                    quotitemdata.Status = "Active";
                                }
                            }
                            else if (quotitemdata.Balance == 0)
                            {
                                quotitemdata.Status = "InActive";
                            }
                            _QuotationItemService.Update(quotitemdata);

                            var QuotDetails = _QuotationService.GetDetailsByQuotNo(model.SalesOrderDetails.QuotationNo);
                            var QuotItemList = _QuotationItemService.GetAllActiveItemsByQuotNo(model.SalesOrderDetails.QuotationNo);
                            if (QuotItemList.Count() == 0)
                            {
                                QuotDetails.Status = "InActive";
                                _QuotationService.Update(QuotDetails);
                            }
                            else
                            {
                                QuotDetails.Status = "Active";
                                _QuotationService.Update(QuotDetails);
                            }
                        }
                        model.SalesOrderItemDetails.OrderNo = model.SalesOrderDetails.OrderNo;
                        model.SalesOrderItemDetails.Status = "Active";
                        model.SalesOrderItemDetails.ModifiedOn = DateTime.Now;
                        string Save = "Yes";
                        if (Convert.ToDouble(frmcol[quantity]) == 0 & Convert.ToDouble(frmcol[bal]) != 0)
                        {
                            Save = "No";
                        }
                        else if (Convert.ToDouble(frmcol[quantity]) == 0 & Convert.ToDouble(frmcol[bal]) == 0)
                        {
                            Save = "No";
                        }
                        if (Save != "No")
                        {
                            _SalesOrderItemService.Create(model.SalesOrderItemDetails);
                        }

                    }
                }
            }
            model.SalesOrderDetails.RemainingAdvance = model.SalesOrderDetails.AdvancePayment;
            model.SalesOrderDetails.TotalAdvancePayment = model.SalesOrderDetails.AdvancePayment;
            model.SalesOrderDetails.Status = "Active";
            model.SalesOrderDetails.ModifiedOn = DateTime.Now;
            model.SalesOrderDetails.Editable = "Yes";
            model.SalesOrderDetails.CashierStatus = "Active";
            model.SalesOrderDetails.PreparedTime = DateTime.Now.ToLongTimeString();
            _SalesOrderService.Update(model.SalesOrderDetails);

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = model.SalesOrderDetails.OrderNo;
                    model.InventoryTaxDetails.Amount = frmcol[amountontax];
                    model.InventoryTaxDetails.Tax = frmcol[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }
            TempData["PreviousSalesOrder"] = model.SalesOrderDetails.OrderNo;

            var SOId = Encode(model.SalesOrderDetails.Id.ToString());
            return RedirectToAction("SalesOrderDetails/" + SOId, "ProcessingQuotation");
        }

        //DELETE SALES ORDER DETAILS
        [HttpGet]
        public ActionResult SalesOrderDelete()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //GET DETAILS BY SALES ORDER NO
        [HttpGet]
        public ActionResult GetSalesOrderDetailsForDelete(string orderno)
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = _SalesOrderService.GetDetailsByOrderNo(orderno);
            TempData["SalesOrderUpdateDetails"] = model.SalesOrderDetails;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesOrderDetails.OrderNo);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(orderno);
            TempData["SalesOrderUpdateList"] = model.SalesOrderItemList;
            return View(model);
        }

        //DELETE SALES ORDER NO
        [HttpPost]
        public ActionResult GetSalesOrderDetailsForDelete()
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = TempData["SalesOrderUpdateDetails"] as SalesOrder;
            model.SalesOrderDetails.Status = "InActive";
            _SalesOrderService.Update(model.SalesOrderDetails);
            model.SalesOrderItemList = TempData["SalesOrderUpdateList"] as IEnumerable<SalesOrderItem>;
            foreach (var item in model.SalesOrderItemList)
            {
                item.Status = "InActive";
                _SalesOrderItemService.Update(item);
            }

            return RedirectToAction("SalesOrderDelete");
        }

        //DELETE SALES ORDER DETAILS
        [HttpGet]
        public ActionResult SalesOrderPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //GET DETAILS BY SALES ORDER NO FOR PRINT
        [HttpGet]
        public ActionResult GetSalesOrderDetailsForPrint(string orderno)
        {
            MainApplication model = new MainApplication();
            model.SalesOrderDetails = _SalesOrderService.GetDetailsByOrderNo(orderno);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.SalesOrderDetails.OrderNo);
            model.SalesOrderItemList = _SalesOrderItemService.GetDetailsByOrderNo(orderno);
            return View(model);
        }

        /****************************************** Delivery Challan ***************************************/

        [HttpGet]
        public ActionResult DeliveryChallan()
        {
            MainApplication model = new MainApplication()
            {
                EmployeeDetails = new EmployeeMaster(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //GET LOGIN SHOP DETAILS
            var username = HttpContext.Session["UserName"].ToString();
            // IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _IUserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastChallan = _DeliveryChallanService.GetLastChallanByFinYr(FinYr, shopcode);
            string ChallanCode = string.Empty;
            int ChallanNoLength = 0;
            int ChallanNo = 0;
            if (LastChallan == null)
            {
                ChallanNoLength = 1;
                ChallanNo = 1;
            }
            else
            {
                int ChallanIndex = LastChallan.ChallanNo.LastIndexOf('D');
                ChallanCode = LastChallan.ChallanNo.Substring(ChallanIndex + 2, 6);
                ChallanNoLength = (Convert.ToInt32(ChallanCode) + 1).ToString().Length;
                ChallanNo = Convert.ToInt32(ChallanCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            ChallanCode = _UtilityService.getName(ShortCode + "/DC", ChallanNoLength, ChallanNo);
            ChallanCode = ChallanCode + FinYr;
            TempData["PreviousChallanno"] = ChallanCode;
            model.ChallanCode = ChallanCode;
            model.EmpList = _employeeservice.getAllemployees();
            model.GodownMasterList = _GodownService.GetGodownNames();


            return View(model);
        }

        [HttpGet]
        public JsonResult GetQuotNo(string SearchTerm)
        {
            if (SearchTerm.ToLower().Contains("q"))
            {
                var quotationdata = _QuotationService.GetQuotProcessedInChallanAndSales(SearchTerm);
                List<string> names = new List<string>();
                foreach (var item in quotationdata)
                {
                    names.Add(item.QuotNo);
                }
                return Json(names, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetOrderNo(string SearchTerm)
        {
            if (SearchTerm.ToLower().Contains("s"))
            {
                var orderdata = _SalesOrderService.GetDetailsByStatusAndCashierStatus(SearchTerm);
                List<string> names = new List<string>();
                foreach (var item in orderdata)
                {
                    names.Add(item.OrderNo);
                }
                return Json(names, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetClient(string code)
        {
            string clientname = string.Empty;
            string clientemail = string.Empty;
            string clientstate = string.Empty;
            string clientcontact = string.Empty;
            string clientaddress = string.Empty;
            string clienttype = string.Empty;
            string consumeresell = string.Empty;
            string formtype = string.Empty;
            string salespersonname = string.Empty;
            string salespersonaddress = string.Empty;
            string salespersondesignation = string.Empty;
            string salespersoncontact = string.Empty;
            string salespersonemail = string.Empty;
            string transportmode = string.Empty;
            string transportname = string.Empty;
            string transportcontactno = string.Empty;
            string advancepayment = string.Empty;
            string tradediscount = string.Empty;
            string tradediscountamount = string.Empty;
            string packandforwd = string.Empty;
            string quotation = string.Empty;
            if (code.Contains("Qu"))
            {
                var quot = _QuotationService.GetQuotByCode(code);
                clientname = quot.ClientName;
                clientemail = quot.ClientEmail;
                clientstate = quot.ClientState;
                clientcontact = quot.ClientContactNo;
                clientaddress = quot.ClientAddress;
                clienttype = quot.ConsumeResell;
                consumeresell = quot.ConsumeResell;
                salespersonname = quot.SalesPersonName;
                salespersonaddress = quot.SalesPersonAddress;
                salespersoncontact = quot.SalesPersonContact;
                salespersondesignation = quot.SalesPersonDesignation;
                salespersonemail = quot.SalesPersonEmail;
                formtype = quot.FormType;
                transportmode = quot.TransportMode;
                transportname = quot.TransportName;
                transportcontactno = quot.TransportContactNo;
                packandforwd = quot.PackAndForwd;
                advancepayment = "0";
            }
            else
            {
                var order = _SalesOrderService.GetDetailsBySalesOrderNo(code);
                quotation = order.QuotationNo;
                clientname = order.ClientName;
                clientemail = order.ClientEmail;
                clientstate = order.ClientState;
                clientcontact = order.ClientContactNo;
                clientaddress = order.ClientAddress;
                clienttype = order.ClientType;
                consumeresell = order.ConsumeResell;
                salespersonname = order.SalesPersonName;
                salespersonaddress = order.SalesPersonAddress;
                salespersoncontact = order.SalesPersonContact;
                salespersondesignation = order.SalesPersonDesignation;
                salespersonemail = order.SalesPersonEmail;
                formtype = order.FormType;
                transportmode = order.TransportMode;
                transportname = order.TransportName;
                tradediscount = order.TotalDiscount;
                packandforwd = order.PackAndForwd;
                advancepayment = order.AdvancePayment.ToString();
            }
            return Json(new
            {
                quotation,
                clientname,
                clientemail,
                clientstate,
                clientcontact,
                clientaddress,
                clienttype,
                consumeresell,
                salespersonname,
                salespersonaddress,
                salespersoncontact,
                salespersondesignation,
                salespersonemail,
                formtype,
                transportmode,
                transportname,
                transportcontactno,
                tradediscount,
                tradediscountamount,
                packandforwd,
                advancepayment
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetClientPendingQuotOrder(string No)
        {
            MainApplication model = new MainApplication();
            if (No.Contains("SO")) //if sales order no is selected in delivery challan
            {
                var clientdetails = _SalesOrderService.GetDetailsByOrderNo(No);//get client name of particular sales order
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<QuotationItem> QuItemList = new List<QuotationItem>();

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName); //get all sales order of that particular client
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo);//get all sales order items of that particular client
                    foreach (var item in soitems)
                    {
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DisInPercent = discount.ToString();
                            item.DisInRs = discountrps.ToString();
                        }

                        if (item.ItemType == "Inventory")
                        {

                            //CALCULATE TOTAL QUANTITY..
                            double Qty;
                            double gdwnstkqty = 0;
                            double shopstkqty = 0;
                            var gdwnstkdata = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            var shopstkdata = _ShopStockService.GetRowsByItemCode(item.ItemCode);

                            //calculate sum of quantity in godown stock
                            foreach (var data1 in gdwnstkdata)
                            {
                                gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                            }
                            //calculate sum of quantity in shop stock
                            foreach (var data2 in shopstkdata)
                            {
                                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                            }
                            //calulate total quantity
                            Qty = gdwnstkqty + shopstkqty;
                            item.ReadOnlyTotalStockQuantity = Qty;
                            item.ReadOnlyShopQuantity = shopstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get quot items by clientname
                var ClientQu = _QuotationService.GetClientQuotList(clientdetails.ClientName);//get all quotations of that particular client
                foreach (var clientquots in ClientQu)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientquots.QuotNo); //get all quotations items of that particular client
                    foreach (var item in quitems)
                    {
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }

                        if (item.ItemType == "Inventory")
                        {
                            //CALCULATE TOTAL QUANTITY..
                            double Qty;
                            double gdwnstkqty = 0;
                            double shopstkqty = 0;
                            var gdwnstkdata = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            var shopstkdata = _ShopStockService.GetRowsByItemCode(item.ItemCode);

                            //calculate sum of quantity in godown stock
                            foreach (var data1 in gdwnstkdata)
                            {
                                gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                            }
                            //calculate sum of quantity in shop stock
                            foreach (var data2 in shopstkdata)
                            {
                                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                            }
                            //calulate total quantity
                            Qty = gdwnstkqty + shopstkqty;
                            item.ReadOnlyTotalStockQuantity = Qty;
                            item.ReadOnlyShopQuantity = shopstkqty;
                        }
                        QuItemList.Add(item);
                    }
                }
                model.QuotationItemList = QuItemList;
            }
            else //if quot no is selected from delivery challan
            {
                var clientdetails = _QuotationService.GetQuotByCode(No);
                List<SalesOrderItem> SOItemList = new List<SalesOrderItem>();
                List<QuotationItem> QuItemList = new List<QuotationItem>();

                //Get salesorder items by clientname
                var ClientSO = _SalesOrderService.GetDetailsByClientName(clientdetails.ClientName); //get all sales order of that particular client
                foreach (var clientSOOrders in ClientSO)
                {
                    var soitems = _SalesOrderItemService.GetDetailsBySONoAndStatus(clientSOOrders.OrderNo); //get all sales order items of that particular client
                    foreach (var item in soitems)
                    {
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DisInPercent = discount.ToString();
                            item.DisInRs = discountrps.ToString();
                        }

                        if (item.ItemType == "Inventory")
                        {
                            //CALCULATE TOTAL QUANTITY..
                            double Qty;
                            double gdwnstkqty = 0;
                            double shopstkqty = 0;
                            var gdwnstkdata = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            var shopstkdata = _ShopStockService.GetRowsByItemCode(item.ItemCode);

                            //calculate sum of quantity in godown stock
                            foreach (var data1 in gdwnstkdata)
                            {
                                gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                            }
                            //calculate sum of quantity in shop stock
                            foreach (var data2 in shopstkdata)
                            {
                                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                            }
                            //calulate total quantity
                            Qty = gdwnstkqty + shopstkqty;
                            item.ReadOnlyTotalStockQuantity = Qty;
                            item.ReadOnlyShopQuantity = shopstkqty;
                        }
                        SOItemList.Add(item);
                    }
                }
                model.SalesOrderItemList = SOItemList;

                //Get quot items by clientname
                var ClientQu = _QuotationService.GetClientQuotList(clientdetails.ClientName); //get all quotations of that particular client
                foreach (var clientquots in ClientQu)
                {
                    var quitems = _QuotationItemService.GetRowsByQuotNoAndStatus(clientquots.QuotNo); //get all quotations items of that particular client
                    foreach (var item in quitems)
                    {
                        double? discount = 0;
                        double? discountrps = 0;
                        var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(item.ItemCode);
                        if (discountdetails != null)
                        {
                            discount = discountdetails.DiscInPercentage;
                            discountrps = discountdetails.DiscInRupees;
                            item.DiscountPercent = discount;
                            item.DiscountRPS = discountrps;
                        }

                        if (item.ItemType == "Inventory")
                        {
                            //CALCULATE TOTAL QUANTITY..
                            double Qty;
                            double gdwnstkqty = 0;
                            double shopstkqty = 0;
                            var gdwnstkdata = _GodownStockService.GetRowsByItemCode(item.ItemCode);
                            var shopstkdata = _ShopStockService.GetRowsByItemCode(item.ItemCode);

                            //calculate sum of quantity in godown stock
                            foreach (var data1 in gdwnstkdata)
                            {
                                gdwnstkqty = gdwnstkqty + Convert.ToDouble(data1.Quantity);
                            }
                            //calculate sum of quantity in shop stock
                            foreach (var data2 in shopstkdata)
                            {
                                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
                            }
                            //calulate total quantity
                            Qty = gdwnstkqty + shopstkqty;
                            item.ReadOnlyTotalStockQuantity = Qty;
                            item.ReadOnlyShopQuantity = shopstkqty;
                        }
                        QuItemList.Add(item);
                    }
                }
                model.QuotationItemList = QuItemList;
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetItemdetailsFromStkDist(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell, string shopcode)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            //GET SUBCATEGORY USING ITEMCODE 
            var itemdetails = _ItemService.GetDescriptionByItemCode(ItemCode);
            string subcat = itemdetails.itemSubCategory;
            string costprice = itemdetails.costprice;
            double taxPercent = 0;
            if (ClientState == "Maharashtra")
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell == "Reseller")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }

            //CALCULATE TOTAL QUANTITY..
            double Qty;
            double totalgdwnstkqty = 0;
            double totalshopstkqty = 0;
            var gdwnstkdata = _GodownStockService.GetRowsByItemCode(ItemCode);
            var shopstkdata = _ShopStockService.GetRowsByItemCode(ItemCode);
            double? shopstkqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(ItemCode, shopcode).Quantity;
            //calculate sum of quantity in stock distribution
            foreach (var data1 in gdwnstkdata)
            {
                totalgdwnstkqty = totalgdwnstkqty + Convert.ToDouble(data1.Quantity);
            }
            //calculate sum of quantity in shop stock
            foreach (var data2 in shopstkdata)
            {
                totalshopstkqty = totalshopstkqty + Convert.ToDouble(data2.Quantity);
            }
            //calulate total quantity
            Qty = totalgdwnstkqty + totalshopstkqty;

            var data = _StockItemDistributionService.GetDetailsByItemCode(ItemCode);
            if (data != null)
            {
                return Json(new { discount, discountrps, taxPercent, data.ItemName, data.Description, data.Color, data.Unit, costprice, data.SellingPrice, data.MRP, data.Material, data.NumberType, data.Barcode, data.DesignCode, data.DesignName, data.Brand, data.Size, Qty, shopstkqty }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetNonInvItemdetails(string ItemCode, string ClientType, string ClientState, string FormType, string ConsumeResell)
        {
            double? discount = 0;
            double? discountrps = 0;
            var discountdetails = _DiscountMasterItemService.GetDailyDiscountByItemCode(ItemCode);
            if (discountdetails != null)
            {
                discount = discountdetails.DiscInPercentage;
                discountrps = discountdetails.DiscInRupees;
            }
            string subcat = _NonInventoryItemService.GetDetailsByItemCode(ItemCode).itemSubCategory;
            //GET SUBCATEGORY USING ITEMCODE 
            double taxPercent = 0;
            if (ClientState == "Maharashtra")
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell != "Consumer")
                    {
                        taxPercent = 0;
                    }
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("VAT", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }
            else
            {
                if (ClientType == "Registered")
                {
                    if (ConsumeResell == "Reseller")
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
                    else
                    {
                        var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                        if (Tax != null)
                        {
                            taxPercent = Tax.Percentage;
                        }
                        else { taxPercent = 0; }
                    }
                }
                else
                {
                    var Tax = _ItemTaxService.GetLatestTax("CST", subcat);
                    if (Tax != null)
                    {
                        taxPercent = Tax.Percentage;
                    }
                    else { taxPercent = 0; }
                }
            }

            var data = _NonInventoryItemService.GetDetailsByItemCode(ItemCode);
            if (data != null)
            {
                return Json(new { discount, discountrps, taxPercent, data.NumberType, data.itemCode, data.itemName, data.description, data.colorCode, data.unit, data.costprice, data.sellingprice, data.mrp, data.typeOfMaterial, data.designCode, data.designName, data.brandName, data.size }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string msg = "Fail";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetTaxFromQuotOrOrder(string orderno)
        {
            var list = _InventoryTaxService.GetTaxesByCode(orderno);
            var taxlist = list.Select(m => new InventoryTax
            {
                Tax = m.Tax,
                Amount = m.Amount,
            });
            return Json(taxlist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult OrderAlreadyProcessed()
        {
            string msg = "Present";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownNames()
        {
            var GodownNamesList = _GodownService.GetGodownNames();
            var godownNames = GodownNamesList.Select(m => new SelectListItem
            {
                Text = m.GodownName,
                Value = m.GdCode
            });
            return Json(godownNames, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStockDistributionItems(string gdcode)
        {
            var itemlist = _StockItemDistributionService.GetDetailsByGodownCode(gdcode);
            var items = itemlist.Select(m => new SelectListItem
            {
                Text = m.ItemName,
                Value = m.ItemCode,
            });
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BillPopUp()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.QuotationList = _QuotationService.GetOnlyPendingQuotations();
            model.SalesOrderList = _SalesOrderService.GetAllActiveSalesOrder();
            return View(model);
        }

        //GET SHOP DETAILS BY SHOP CODE
        [HttpGet]
        public JsonResult GetShopDetails(string code)
        {
            var data = _ShopService.GetShopByCode(code);
            return Json(new { data.ShopName, data.ShopAddress, data.ShopEmail, data.EmpName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeliveryChallan(MainApplication model, FormCollection frm)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastChallan = _DeliveryChallanService.GetLastChallanByFinYr(FinYr, shopcode);
            string ChallanCode = string.Empty;
            int ChallanNoLength = 0;
            int ChallanNo = 0;
            if (LastChallan == null)
            {
                ChallanNoLength = 1;
                ChallanNo = 1;
            }
            else
            {
                int ChallanIndex = LastChallan.ChallanNo.LastIndexOf('D');
                ChallanCode = LastChallan.ChallanNo.Substring(ChallanIndex + 2, 6);
                ChallanNoLength = (Convert.ToInt32(ChallanCode) + 1).ToString().Length;
                ChallanNo = Convert.ToInt32(ChallanCode) + 1;
            }
            string ShortCode = _ShopService.GetShopDetailsByName(shopname).ShortCode;
            ChallanCode = _UtilityService.getName(ShortCode + "/DC", ChallanNoLength, ChallanNo);
            ChallanCode = ChallanCode + FinYr;

            if (model.DeliveryChallanDetails.QuotNo != null)
            {
                var challanlist = _DeliveryChallanService.GetChallanListByQuotNo(model.DeliveryChallanDetails.QuotNo);
                foreach (var row in challanlist)
                {
                    if (row.Editable == "Yes")
                    {
                        row.Editable = "No";
                        _DeliveryChallanService.Update(row);
                    }
                }
                var QuotDetails = _QuotationService.GetDetailsByQuotNo(model.DeliveryChallanDetails.QuotNo);
                QuotDetails.ProcessedIn = ChallanCode;
                _QuotationService.Update(QuotDetails);
            }
            else if (model.DeliveryChallanDetails.OrderNo != null)
            {
                var challanlist = _DeliveryChallanService.GetChallanListByOrderno(model.DeliveryChallanDetails.OrderNo);
                foreach (var row in challanlist)
                {
                    if (row.Editable == "Yes")
                    {
                        row.Editable = "No";
                        _DeliveryChallanService.Update(row);
                    }
                }
                var OrderDetails = _SalesOrderService.GetDetailsByOrderNo(model.DeliveryChallanDetails.OrderNo);
                OrderDetails.ProcessedIn = ChallanCode;
                _SalesOrderService.Update(OrderDetails);
            }

            model.DeliveryChallanDetails.PreparedTime = DateTime.Now.ToLongTimeString();
            model.DeliveryChallanDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
            model.DeliveryChallanDetails.ChallanNo = ChallanCode;
            model.DeliveryChallanDetails.ChallanDate = DateTime.Now;
            model.DeliveryChallanDetails.Editable = "Yes";
            model.DeliveryChallanDetails.Status = "Active";
            model.DeliveryChallanDetails.ModifiedOn = DateTime.Now;
            _DeliveryChallanService.Create(model.DeliveryChallanDetails);
            var PartialView = frm["PartialView"];

            // DELIVERY CHALLAN WITH MULTIPLE BILLS
            if (PartialView == "Yes")
            {
                int rowcount = Convert.ToInt32(frm["hdnRowCount"]);
                for (int i = 1; i <= rowcount; i++)
                {
                    string ItemCode = "ItemCode" + i;
                    string ItemName = "ItemName" + i;
                    string ItemType = "ItemType" + i;
                    string barcode = "Barcode" + i;
                    string description = "DescriptionValue" + i;
                    string narration = "Narration" + i;
                    string color = "ColorValue" + i;
                    string designcode = "DesignCode" + i;
                    string designname = "DesignName" + i;
                    string brand = "Brand" + i;
                    string size = "Size" + i;
                    string quantity = "Quantity" + i;
                    string quotororderquantity = "QuotOrderQtyValue" + i;
                    string quotororderno = "QuotOrderNo" + i;
                    string balance = "BalanceValue" + i;
                    string extraqty = "ExtraQty" + i;
                    string materialvalue = "MaterialValue" + i;
                    string unit = "UnitValue" + i;
                    string numbertype = "NumberType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string ItemTax = "ItemTaxValue" + i;
                    string ItemTaxAmt = "ItemTaxAmount" + i;
                    string amountvalue = "AmountValue" + i;
                    string discountpercent = "DisPer" + i;
                    string discountrps = "DisRs" + i;
                    string proportionatetax = "proportionatetaxvalue" + i;
                    string proportionatetaxamount = "proportionatetaxamount" + i;

                    if (frm[ItemCode] != null)
                    {
                        DeliveryChallanItem ChallanItem = new DeliveryChallanItem();
                        ChallanItem.ChallanNo = ChallanCode;
                        if (frm[quotororderno] != null)
                        {
                            ChallanItem.QuotOrOrderNo = frm[quotororderno];
                        }
                        if (frm[barcode] == "")
                        {
                            ChallanItem.Barcode = null;
                        }
                        else
                        {
                            ChallanItem.Barcode = frm[barcode];
                        }
                        ChallanItem.ItemCode = frm[ItemCode];
                        ChallanItem.ItemName = frm[ItemName];
                        ChallanItem.ItemType = frm[ItemType];
                        ChallanItem.Description = frm[description];
                        ChallanItem.Narration = frm[narration];
                        ChallanItem.Color = frm[color];
                        ChallanItem.Material = frm[materialvalue];
                        ChallanItem.Design = frm[designcode];
                        ChallanItem.DesignName = frm[designname];
                        ChallanItem.Brand = frm[brand];
                        ChallanItem.Size = frm[size];
                        ChallanItem.Quantity = frm[quantity];
                        ChallanItem.QuotOrderQty = frm[quotororderquantity];
                        ChallanItem.DelBalance = Convert.ToDouble(frm[quantity]);
                        ChallanItem.ExtraQty = Convert.ToDouble(frm[extraqty]);
                        ChallanItem.Unit = frm[unit];
                        ChallanItem.NumberType = frm[numbertype];
                        ChallanItem.SellingPrice = Convert.ToDouble(frm[sellingprice]);
                        ChallanItem.MRP = Convert.ToDouble(frm[mrp]);
                        ChallanItem.DiscountPercent = Convert.ToDouble(frm[discountpercent]);
                        ChallanItem.DiscountRPS = Convert.ToDouble(frm[discountrps]);
                        ChallanItem.Amount = frm[amountvalue];
                        ChallanItem.ItemTax = frm[ItemTax];
                        ChallanItem.ItemTaxAmt = frm[ItemTaxAmt];
                        ChallanItem.ProportionateTax = frm[proportionatetax];
                        ChallanItem.ProportionateTaxAmt = frm[proportionatetaxamount];
                        ChallanItem.Status = "Active";
                        ChallanItem.ModifiedOn = DateTime.Now;
                        _DeliveryChallanItemService.Create(ChallanItem);

                        if (frm[ItemType] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - Convert.ToDouble(ChallanItem.Quantity);
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(ChallanItem.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(ChallanItem.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(ChallanItem.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(ChallanItem.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(ChallanItem.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }

                        if (Convert.ToDouble(frm[quotororderquantity]) != 0)
                        {
                            //if salesorderitem processed in deliverychallan then update sales order (QuotBalance and SObalance)
                            if (frm[quotororderno].Contains("SO"))
                            {
                                var SOitemdetails = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(frm[ItemCode], frm[quotororderno]);

                                double? remainsobalance = 0;
                                remainsobalance = SOitemdetails.SOBalance - Convert.ToDouble(ChallanItem.Quantity);
                                //if item balace is 0 in delivery challan then InActive that item..
                                if (remainsobalance <= 0)
                                {
                                    SOitemdetails.Status = "InActive";
                                }
                                if (remainsobalance >= 0)
                                {
                                    SOitemdetails.SOBalance = remainsobalance;
                                }
                                else
                                {
                                    SOitemdetails.SOBalance = 0;
                                }
                                SOitemdetails.ProcessedIn = ChallanItem.ChallanNo;
                                _SalesOrderItemService.Update(SOitemdetails);

                                var SODetails = _SalesOrderService.GetDetailsByOrderNo(frm[quotororderno]);
                                var SOItemActiveList = _SalesOrderItemService.GetAllActiveItemsByOrderNo(frm[quotororderno]);
                                if (SOItemActiveList.Count() == 0)
                                {
                                    SODetails.Status = "InActive";
                                    _SalesOrderService.Update(SODetails);
                                }
                            }

                            //if quotationitem processed in deliverychallan then update quotation balance + sales order balance
                            else
                            {
                                var QUitemdetails = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frm[ItemCode], frm[quotororderno]);
                                double? remainquotbalance = 0;
                                remainquotbalance = QUitemdetails.Balance - Convert.ToDouble(ChallanItem.Quantity);
                                //if item balace is 0 in delivery challan then InActive that item..
                                if (remainquotbalance <= 0)
                                {
                                    QUitemdetails.Status = "InActive";
                                }
                                if (remainquotbalance >= 0)
                                {
                                    QUitemdetails.Balance = remainquotbalance;
                                }
                                else
                                {
                                    QUitemdetails.Balance = 0;
                                }
                                QUitemdetails.ProcessedIn = ChallanItem.ChallanNo;
                                _QuotationItemService.Update(QUitemdetails);

                                var QuotDetails = _QuotationService.GetDetailsByQuotNo(frm[quotororderno]);
                                var QuotActiveItemsList = _QuotationItemService.GetAllActiveItemsByQuotNo(frm[quotororderno]);
                                if (QuotActiveItemsList.Count() == 0)
                                {
                                    QuotDetails.Status = "InActive";
                                    _QuotationService.Update(QuotDetails);
                                }
                            }
                        }
                    }
                }
            }

            //DIRECT DELIVERY CHALLAN
            else
            {
                model.DeliveryChallanItemDetails = new DeliveryChallanItem();
                int rowid = Convert.ToInt32(frm["hdnRowCount"]);
                for (int i = 1; i <= rowid; i++)
                {
                    string ItemCode = "ItemCode" + i;
                    string ItemName = "ItemName" + i;
                    string ItemType = "ItemType" + i;
                    string barcode = "Barcode" + i;
                    string description = "DescriptionValue" + i;
                    string narration = "Narration" + i;
                    string color = "ColorValue" + i;
                    string designcode = "DesignCode" + i;
                    string designname = "DesignName" + i;
                    string brand = "Brand" + i;
                    string size = "Size" + i;
                    string quantity = "Quantity" + i;
                    string quotororderquantity = "QuotOrderQtyValue" + i;
                    string balance = "BalanceValue" + i;
                    string extraqty = "ExtraQty" + i;
                    string materialvalue = "MaterialValue" + i;
                    string unit = "UnitValue" + i;
                    string numbertype = "NumberType" + i;
                    string sellingprice = "SellingPrice" + i;
                    string mrp = "MRP" + i;
                    string ItemTax = "ItemTaxValue" + i;
                    string ItemTaxAmt = "ItemTaxAmount" + i;
                    string amountvalue = "AmountValue" + i;
                    string discountpercent = "DisPer" + i;
                    string discountrps = "DisRs" + i;
                    string proportionatetax = "proportionatetaxvalue" + i;
                    string proportionatetaxamount = "proportionatetaxamount" + i;

                    if (frm[ItemCode] != null)
                    {
                        DeliveryChallanItem ChallanItem = new DeliveryChallanItem();
                        ChallanItem.ChallanNo = ChallanCode;
                        ChallanItem.QuotOrOrderNo = null;
                        ChallanItem.ItemCode = frm[ItemCode];
                        ChallanItem.ItemName = frm[ItemName];
                        if (frm[barcode] == "")
                        {
                            ChallanItem.Barcode = null;
                        }
                        else
                        {
                            ChallanItem.Barcode = frm[barcode];
                        }
                        ChallanItem.ItemType = frm[ItemType];
                        ChallanItem.Description = frm[description];
                        ChallanItem.Narration = frm[narration];
                        ChallanItem.Color = frm[color];
                        ChallanItem.Material = frm[materialvalue];
                        ChallanItem.Design = frm[designcode];
                        ChallanItem.DesignName = frm[designname];
                        ChallanItem.Brand = frm[brand];
                        ChallanItem.Size = frm[size];
                        ChallanItem.Quantity = frm[quantity];
                        ChallanItem.DelBalance = Convert.ToDouble(frm[quantity]);
                        ChallanItem.ExtraQty = 0;
                        ChallanItem.Unit = frm[unit];
                        ChallanItem.NumberType = frm[numbertype];
                        ChallanItem.SellingPrice = Convert.ToDouble(frm[sellingprice]);
                        ChallanItem.MRP = Convert.ToDouble(frm[mrp]);
                        ChallanItem.DiscountPercent = Convert.ToDouble(frm[discountpercent]);
                        ChallanItem.DiscountRPS = Convert.ToDouble(frm[discountrps]);
                        ChallanItem.Amount = frm[amountvalue];
                        ChallanItem.ItemTax = frm[ItemTax];
                        ChallanItem.ItemTaxAmt = frm[ItemTaxAmt];
                        ChallanItem.ProportionateTax = frm[proportionatetax];
                        ChallanItem.ProportionateTaxAmt = frm[proportionatetaxamount];
                        ChallanItem.Status = "Active";
                        ChallanItem.ModifiedOn = DateTime.Now;
                        _DeliveryChallanItemService.Create(ChallanItem);

                        if (frm[ItemType] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - Convert.ToDouble(ChallanItem.Quantity);
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(ChallanItem.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(ChallanItem.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(ChallanItem.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(ChallanItem.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(ChallanItem.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }
                    }
                }
            }
            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frm["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frm[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = ChallanCode;
                    model.InventoryTaxDetails.Amount = frm[amountontax];
                    model.InventoryTaxDetails.Tax = frm[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frm[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }

            int id = _DeliveryChallanService.GetDetailsByChallanNo(ChallanCode).Id;
            string ChallanId = Encode(id.ToString());
            return RedirectToAction("DeliveryChallanDetails/" + ChallanId, "ProcessingQuotation");
        }

        [HttpGet]
        public ActionResult DeliveryChallanDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.DeliveryChallanDetails.ChallanNo);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            string previouschallanno = TempData["PreviousChallanno"].ToString();
            if (TempData["PreviousChallanno"].ToString() != model.DeliveryChallanDetails.ChallanNo)
            {
                ViewData["ChallannoChanged"] = previouschallanno + " is replaced with " + model.DeliveryChallanDetails.ChallanNo + " because " + previouschallanno + " is acquired by another person";
            }
            TempData["PreviousChallanno"] = previouschallanno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintDeliveryChallanWithSellingPrice(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintDeliveryChallanWithMRP(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }


        [HttpGet]
        public ActionResult PrintChallanSellingPricePrePrinted(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintChallanMRPPrePrinted(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }


        [HttpGet]
        public JsonResult GetSalesAndQuotationItems(string orderno)
        {
            if (orderno.Contains("SO"))
            {
                var Salesorderitemlist = _SalesOrderItemService.GetDetailsByOrderNo(orderno);
                var Salesorderitemlist2 = Salesorderitemlist;
                Salesorderitemlist.ToList().Clear();
                foreach (var itemlist in Salesorderitemlist2)
                {
                    double? gdqty = 0, shopqty = 0;
                    var shopitemlist = _ShopStockService.GetRowsByItemCode(itemlist.ItemCode);
                    var stkitemlist = _StockItemDistributionService.GetRowsByItemCode(itemlist.ItemCode);
                    foreach (var i in stkitemlist)
                    {
                        gdqty = gdqty + i.ItemQuantity;
                    }
                    foreach (var i in shopitemlist)
                    {
                        shopqty = shopqty + i.Quantity;
                    }
                    double? totalqty = shopqty + gdqty;
                    itemlist.ReadOnlyTotalStockQuantity = totalqty;
                    Salesorderitemlist.ToList().Add(itemlist);
                }
                var list = Salesorderitemlist.Select(m => new SalesOrderItem
                {
                    OrderNo = m.OrderNo,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    Description = m.Description,
                    Color = m.Color,
                    Material = m.Material,
                    Unit = m.Unit,
                    DesignName = m.DesignName,
                    //Rate = m.Rate,
                    Amount = m.Amount,
                    Quantity = m.Quantity,
                    ItemTax = m.ItemTax,
                    PropTax = m.PropTax,
                    ReadOnlyTotalStockQuantity = m.ReadOnlyTotalStockQuantity
                });
                string msg = "Sales";
                return Json(new
                {
                    list,
                    msg
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Quotationitemlist = _QuotationItemService.GetDetailsByQuotNo(orderno);
                var Quotationitemlist2 = Quotationitemlist;
                Quotationitemlist.ToList().Clear();
                foreach (var itemlist in Quotationitemlist2)
                {
                    double? gdqty = 0, shopqty = 0;
                    var shopitemlist = _ShopStockService.GetRowsByItemCode(itemlist.ItemCode);
                    var stkitemlist = _StockItemDistributionService.GetRowsByItemCode(itemlist.ItemCode);
                    foreach (var i in stkitemlist)
                    {
                        gdqty = gdqty + i.ItemQuantity;
                    }
                    foreach (var i in shopitemlist)
                    {
                        shopqty = shopqty + i.Quantity;
                    }
                    double? totalqty = shopqty + gdqty;
                    itemlist.ReadOnlyTotalStockQuantity = totalqty;
                    Quotationitemlist.ToList().Add(itemlist);
                }
                var list = Quotationitemlist.Select(m => new QuotationItem
                {
                    QuotNo = m.QuotNo,
                    ItemCode = m.ItemCode,
                    ItemName = m.ItemName,
                    Color = m.Color,
                    Description = m.Description,
                    Material = m.Material,
                    Unit = m.Unit,
                    DesignName = m.DesignName,
                    //Rate = m.Rate,
                    Amount = m.Amount,
                    Quantity = m.Quantity,
                    ItemTax = m.ItemTax,
                    ProportionateTax = m.ProportionateTax,
                    ReadOnlyTotalStockQuantity = m.ReadOnlyTotalStockQuantity
                });
                string msg = "Quotation";
                return Json(new
                {
                    list,
                    msg
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ChallanEdit()
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
        public JsonResult ChallanEditList(string term)
        {
            var data = _DeliveryChallanService.GetChallanNos(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.ChallanNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ChallanPrintList(string term)
        {
            var data = _DeliveryChallanService.GetAllDeliveryChallans(term);
            List<string> names = new List<string>();
            foreach (var item in data)
            {
                names.Add(item.ChallanNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetChallanEditList(string ChallanNo)
        {
            MainApplication model = new MainApplication();
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsByChallanNo(ChallanNo);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(ChallanNo);
            var DcItemList = model.DeliveryChallanItemList;
            DcItemList.ToList().Clear();
            foreach (var item in model.DeliveryChallanItemList)
            {
                if (item.QuotOrOrderNo != null)
                {
                    if (item.QuotOrOrderNo.Contains("Qu"))
                    {
                        var quotitemdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(item.ItemCode, item.QuotOrOrderNo);
                        if (quotitemdata != null)
                        {
                            item.ReadOnlySOBalance = quotitemdata.Balance;
                        }
                        else
                        {
                            item.ReadOnlySOBalance = 0;
                        }
                    }
                    else if (item.QuotOrOrderNo.Contains("SO"))
                    {
                        var soitemdata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(item.ItemCode, item.QuotOrOrderNo);
                        if (soitemdata != null)
                        {
                            item.ReadOnlySOBalance = soitemdata.SOBalance;
                        }
                        else
                        {
                            item.ReadOnlySOBalance = 0;
                        }
                    }
                }
                else
                {
                    item.ReadOnlySOBalance = 0;
                }
                DcItemList.ToList().Add(item);
            }
            model.DeliveryChallanItemList.ToList().Clear();
            model.DeliveryChallanItemList = DcItemList;
            TempData["DCItemList"] = model.DeliveryChallanItemList;
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(ChallanNo);
            if (model.InventoryTaxList.Count() != 0)
            {
                TempData["DCTaxList"] = model.InventoryTaxList;
            }
            return View(model);
        }

        //GET STOCK QUANTITY
        [HttpGet]
        public JsonResult GetStockandShopQuantity(string ItemCode, string shopcode)
        {
            //CALCULATE TOTAL QUANTITY..
            double? Qty;
            double? gdstkqty = 0;
            double? shopstkqty = 0;
            var gdstkdata = _GodownStockService.GetRowsByItemCode(ItemCode);
            var shopstkdata = _ShopStockService.GetRowsByItemCode(ItemCode);

            //calculate sum of quantity in godown stock
            foreach (var data1 in gdstkdata)
            {
                gdstkqty = gdstkqty + data1.Quantity;
            }
            //calculate sum of quantity in shop stock
            foreach (var data2 in shopstkdata)
            {
                shopstkqty = shopstkqty + Convert.ToDouble(data2.Quantity);
            }
            //calulate total quantity
            Qty = gdstkqty + shopstkqty;
            double? shopqty = _ShopStockService.GetDetailsByItemCodeAndShopCode(ItemCode, shopcode).Quantity;
            return Json(new { Qty, shopqty }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetChallanEditList(MainApplication model, FormCollection frm)
        {
            model.DeliveryChallanDetails.PreparedTime = DateTime.Now.ToLongTimeString();
            model.DeliveryChallanDetails.ChallanDate = DateTime.Now;
            model.DeliveryChallanDetails.Editable = "Yes";
            model.DeliveryChallanDetails.Status = "Active";
            model.DeliveryChallanDetails.ModifiedOn = DateTime.Now;
            _DeliveryChallanService.Update(model.DeliveryChallanDetails);

            var DCItemList = TempData["DCItemList"] as IEnumerable<DeliveryChallanItem>;
            foreach (var delete in DCItemList)
            {
                var deletedata = _DeliveryChallanItemService.GetById(delete.Id);
                _DeliveryChallanItemService.Delete(deletedata);
            }

            var DCTaxList = TempData["DCTaxList"] as IEnumerable<InventoryTax>;
            if (DCTaxList != null)
            {
                foreach (var delete in DCTaxList)
                {
                    var deletedata = _InventoryTaxService.GetTaxById(delete.Id);
                    _InventoryTaxService.Delete(deletedata);
                }
            }

            // DELIVERY CHALLAN WITH MULTIPLE BILLS
            int rowcount = Convert.ToInt32(frm["hdnRowCount"]);
            for (int i = 1; i <= rowcount; i++)
            {
                string ItemCode = "ItemCode" + i;
                string ItemName = "ItemName" + i;
                string ItemType = "ItemType" + i;
                string barcode = "Barcode" + i;
                string description = "DescriptionValue" + i;
                string narration = "Narration" + i;
                string color = "ColorValue" + i;
                string designcode = "DesignCode" + i;
                string designname = "DesignName" + i;
                string brand = "Brand" + i;
                string size = "Size" + i;
                string quantity = "Quantity" + i;
                string prevqty = "PrevQuantity" + i;
                string quotororderquantity = "QuotOrderQtyValue" + i;
                string quotororderno = "QuotOrderNo" + i;
                string balance = "BalanceVal" + i;
                string materialvalue = "MaterialValue" + i;
                string unit = "UnitValue" + i;
                string numbertype = "NumberType" + i;
                string sellingprice = "SellingPrice" + i;
                string mrp = "MRP" + i;
                string ItemTax = "ItemTaxValue" + i;
                string ItemTaxAmt = "ItemTaxAmount" + i;
                string amountvalue = "AmountValue" + i;
                string discountpercent = "DisPer" + i;
                string discountrps = "DisRs" + i;
                string proportionatetax = "proportionatetaxvalue" + i;
                string proportionatetaxamount = "proportionatetaxamount" + i;

                if (frm[ItemCode] != null)
                {
                    DeliveryChallanItem ChallanItem = new DeliveryChallanItem();
                    ChallanItem.ChallanNo = model.DeliveryChallanDetails.ChallanNo;
                    if (frm[quotororderno] != null && frm[quotororderno] != "")
                    {
                        ChallanItem.QuotOrOrderNo = frm[quotororderno];
                    }
                    if (frm[barcode] == "")
                    {
                        ChallanItem.Barcode = null;
                    }
                    else
                    {
                        ChallanItem.Barcode = frm[barcode];
                    }
                    ChallanItem.ItemCode = frm[ItemCode];
                    ChallanItem.ItemName = frm[ItemName];
                    ChallanItem.ItemType = frm[ItemType];
                    ChallanItem.Description = frm[description];
                    ChallanItem.Narration = frm[narration];
                    ChallanItem.Color = frm[color];
                    ChallanItem.Material = frm[materialvalue];
                    ChallanItem.Design = frm[designcode];
                    ChallanItem.DesignName = frm[designname];
                    ChallanItem.Brand = frm[brand];
                    ChallanItem.Size = frm[size];
                    ChallanItem.Quantity = frm[quantity];
                    ChallanItem.QuotOrderQty = frm[quotororderquantity];
                    ChallanItem.DelBalance = Convert.ToDouble(frm[quantity]);
                    ChallanItem.Unit = frm[unit];
                    ChallanItem.NumberType = frm[numbertype];
                    ChallanItem.SellingPrice = Convert.ToDouble(frm[sellingprice]);
                    ChallanItem.MRP = Convert.ToDouble(frm[mrp]);
                    ChallanItem.DiscountPercent = Convert.ToDouble(frm[discountpercent]);
                    ChallanItem.DiscountRPS = Convert.ToDouble(frm[discountrps]);
                    ChallanItem.Amount = frm[amountvalue];
                    ChallanItem.ItemTax = frm[ItemTax];
                    ChallanItem.ItemTaxAmt = frm[ItemTaxAmt];
                    ChallanItem.ProportionateTax = frm[proportionatetax];
                    ChallanItem.ProportionateTaxAmt = frm[proportionatetaxamount];
                    ChallanItem.Status = "Active";
                    ChallanItem.ModifiedOn = DateTime.Now;

                    if (frm[prevqty] != null)
                    {
                        if (frm[ItemType] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - CHANGE QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            double changespstkqty = Convert.ToDouble(frm[quantity]) - Convert.ToDouble(frm[prevqty]);
                            if (changespstkqty < 0)
                            {
                                shopstockdata.Quantity = shopstockdata.Quantity + (0 - changespstkqty);
                                _ShopStockService.Update(shopstockdata);
                            }
                            else if (changespstkqty > 0)
                            {
                                shopstockdata.Quantity = shopstockdata.Quantity - changespstkqty;
                                _ShopStockService.Update(shopstockdata);
                            }

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - CHANGE QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            double changestkdistqty = Convert.ToDouble(frm[quantity]) - Convert.ToDouble(frm[prevqty]);
                            if (changestkdistqty < 0)
                            {
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity + (0 - changestkdistqty);
                                _StockItemDistributionService.Update(stkdisdata);
                            }
                            else if (changestkdistqty > 0)
                            {
                                stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - changestkdistqty;
                                _StockItemDistributionService.Update(stkdisdata);
                            }

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - CHANGE QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(ChallanItem.ItemCode);
                            double changeentryopeningqty = Convert.ToDouble(frm[quantity]) - Convert.ToDouble(frm[prevqty]);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                if (changeentryopeningqty < 0)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity + (0 - changeentryopeningqty);
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                                else if (changeentryopeningqty > 0)
                                {
                                    entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - changeentryopeningqty;
                                    _EntryStockItemService.Update(entrystkdata);
                                }
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - CHANGE QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(ChallanItem.ItemCode);
                                if (changeentryopeningqty < 0)
                                {
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity + (0 - changeentryopeningqty);
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                                else if (changeentryopeningqty > 0)
                                {
                                    openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - changeentryopeningqty;
                                    _OpeningStockService.UpdateStock(openingstkdata);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (frm[ItemType] == "Inventory")
                        {
                            //UPDATE SHOP STOCK (SHOP STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var shopstockdata = _ShopStockService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            shopstockdata.Quantity = shopstockdata.Quantity - Convert.ToDouble(ChallanItem.Quantity);
                            _ShopStockService.Update(shopstockdata);

                            //UPDATE STOCK DISTRIBUTION (STOCK DISTRIBUTION QUANTITY - RETAIL BILL QUANTITY)
                            var stkdisdata = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(ChallanItem.ItemCode, model.DeliveryChallanDetails.ShopCode);
                            stkdisdata.ItemQuantity = stkdisdata.ItemQuantity - Convert.ToDouble(ChallanItem.Quantity);
                            _StockItemDistributionService.Update(stkdisdata);

                            //UPDATE ENTRY STOCK (ENTRY STOCK QUANTITY - RETAIL BILL QUANTITY)
                            var entrystkdata = _EntryStockItemService.getDetailsByItemCode(ChallanItem.ItemCode);
                            //if item not present in entry stock then opening stock
                            if (entrystkdata != null)
                            {
                                entrystkdata.TotalQuantity = entrystkdata.TotalQuantity - Convert.ToDouble(ChallanItem.Quantity);
                                _EntryStockItemService.Update(entrystkdata);
                            }
                            //UPDATE OPENING STOCK (OPENING STOCK QUANTITY - RETAIL BILL QUANTITY)
                            else
                            {
                                var openingstkdata = _OpeningStockService.GetDetailsByItemCode(ChallanItem.ItemCode);
                                openingstkdata.TotalQuantity = openingstkdata.TotalQuantity - Convert.ToDouble(ChallanItem.Quantity);
                                _OpeningStockService.UpdateStock(openingstkdata);
                            }
                        }
                    }

                    if (frm[quotororderno] != null && frm[quotororderno] != "")
                    {
                        if (frm[quotororderno].Contains("Qu"))
                        {
                            var quotitemdata = _QuotationItemService.GetItemDetailsByItemCodeAndQuotNo(frm[ItemCode], frm[quotororderno]);
                            quotitemdata.Balance = Convert.ToDouble(frm[balance]);

                            if (quotitemdata.Balance != 0)
                            {
                                if (quotitemdata.Status == "InActive")
                                {
                                    quotitemdata.Status = "Active";
                                }
                            }
                            else if (quotitemdata.Balance == 0)
                            {
                                quotitemdata.Status = "InActive";
                            }
                            _QuotationItemService.Update(quotitemdata);

                            var QuotDetails = _QuotationService.GetDetailsByQuotNo(frm[quotororderno]);
                            var QuotActiveItemsList = _QuotationItemService.GetAllActiveItemsByQuotNo(frm[quotororderno]);
                            if (QuotActiveItemsList.Count() == 0)
                            {
                                QuotDetails.Status = "InActive";
                                _QuotationService.Update(QuotDetails);
                            }
                            else
                            {
                                QuotDetails.Status = "Active";
                                _QuotationService.Update(QuotDetails);
                            }
                        }
                        else
                        {
                            var orderitemdata = _SalesOrderItemService.GetItemDetailsByItemCodeandOrderNo(frm[ItemCode], frm[quotororderno]);
                            orderitemdata.SOBalance = Convert.ToDouble(frm[balance]);
                            if (orderitemdata.SOBalance != 0)
                            {
                                if (orderitemdata.Status == "InActive")
                                {
                                    orderitemdata.Status = "Active";
                                }
                            }
                            else if (orderitemdata.SOBalance == 0)
                            {
                                orderitemdata.Status = "InActive";
                            }
                            _SalesOrderItemService.Update(orderitemdata);

                            var SODetails = _SalesOrderService.GetDetailsBySalesOrderNo(frm[quotororderno]);
                            var SOItemActiveList = _SalesOrderItemService.GetAllActiveItemsByOrderNo(frm[quotororderno]);
                            if (SOItemActiveList.Count() == 0)
                            {
                                SODetails.Status = "InActive";
                                _SalesOrderService.Update(SODetails);
                            }
                            else
                            {
                                SODetails.Status = "Active";
                                _SalesOrderService.Update(SODetails);
                            }
                        }
                    }
                    string Save = "Yes";
                    if (Convert.ToDouble(frm[quantity]) == 0)
                    {
                        Save = "No";
                    }
                    if (Save != "No")
                    {
                        _DeliveryChallanItemService.Create(ChallanItem);
                    }
                }
            }
            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frm["ItemTaxCount"]);
            model.InventoryTaxDetails = new InventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frm[taxamount]) != 0)
                {
                    model.InventoryTaxDetails.Code = model.DeliveryChallanDetails.ChallanNo;
                    model.InventoryTaxDetails.Amount = frm[amountontax];
                    model.InventoryTaxDetails.Tax = frm[taxnumber];
                    model.InventoryTaxDetails.TaxAmount = frm[taxamount];
                    _InventoryTaxService.Create(model.InventoryTaxDetails);
                }
            }
            TempData["PreviousChallanno"] = model.DeliveryChallanDetails.ChallanNo;
            string ChallanId = Encode(model.DeliveryChallanDetails.Id.ToString());
            return RedirectToAction("DeliveryChallanDetails/" + ChallanId, "ProcessingQuotation");
        }


        [HttpGet]
        public ActionResult ChallanPrint()
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
        public ActionResult GetChallanDetailsForPrint(string ChallanNo)
        {
            MainApplication model = new MainApplication();
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsByChallanNo(ChallanNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.DeliveryChallanDetails.ChallanNo);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(ChallanNo);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetItemsCostPrice(string itemcode, string itemtype)
        {
            string costprice = "0";
            if (itemtype == "Inventory")
            {
                costprice = _ItemService.GetDescriptionByItemCode(itemcode).costprice;
            }
            else
            {
                costprice = _NonInventoryItemService.GetDetailsByItemCode(itemcode).costprice;
                if (costprice == "")
                {
                    costprice = "0";
                }
            }
            return Json(costprice, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintChallanSellingPriceLH(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintChallanMRPLH(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.DeliveryChallanDetails = _DeliveryChallanService.GetDetailsById(Id);
            model.DeliveryChallanItemList = _DeliveryChallanItemService.GetDetailsByChallanNo(model.DeliveryChallanDetails.ChallanNo);
            model.InventoryTaxList = _InventoryTaxService.GetTaxesByCode(model.DeliveryChallanDetails.ChallanNo);
            return View(model);
        }
    }
}
