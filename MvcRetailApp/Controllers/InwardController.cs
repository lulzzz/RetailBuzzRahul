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
    [NoDirectAccess]
    [SessionExpireFilter]
    public class InwardController : Controller
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
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly IPurchaseOrderDetailService _purchaseOrderDetailService;
        private readonly IInwardInterGodownService _InwardInterGodownService;
        private readonly IInwardInterGodownItemService _InwardInterGodownItemService;
        private readonly IInwardInterShopService _InwardInterShopService;
        private readonly IInwardInterShopItemService _InwardInterShopItemService;
        private readonly IPurchaseItemDetailService _purchaseItemDetailService;
        private readonly IUtilityService _utilityService;
        private readonly ISuppliersMasterService _supplierMasterService;
        private readonly ITransportService _TransportService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IGodownService _godownMasterService;
        private readonly IGodownAddressService _godownAddressService;
        private readonly IItemCategoryService _itemcategoryservice;
        private readonly IItemSubCategoryService _itemsubcategoryservice;
        private readonly IUnitService _unitmasterservice;
        private readonly IDesignService _DesignService;
        private readonly IItemService _ItemService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IBarcodeNumberService _BarcodeNumberService;
        private readonly IOutwardToShopService _OutwardFromGodownToShopService;
        private readonly IOutwardItemToShopService _OutwardItemFromGodownToShopService;
        private readonly IShopService _ShopService;
        private readonly IInwardFromGodownService _InwardFromGodownService;
        private readonly IInwardItemFromGodownService _InwardItemFromGodownService;
        private readonly IShopStockService _ShopStockService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IInwardFromStkDistService _InwardFromStkDistService;
        private readonly IOutwardStockDistributionService _OutwardStkDisService;
        private readonly IOutwardItemStockDistributionService _OutwardItemStkDisService;
        private readonly IInwardItemFromStockDistributionService _InwardItemFromStockDistributionService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly INonInventoryItemService _NonInventoryItemService;
        private readonly IInwardFromShopToGodownService _InwardFromShopToGodownService;
        private readonly IInwardItemFromShopToGodownService _InwardItemFromShopToGodownService;
        private readonly IOutwardShopToGodownService _OutwardShopToGodownService;
        private readonly IOutwardInterGodownService _OutwardInterGodownService;
        private readonly IOutwardItemInterGodownService _OutwardInterGodownItemService;
        private readonly IOutwardInterShopService _OutwardInterShopService;
        private readonly IOutwardItemInterShopservice _OutwardInterShopItemService;
        private readonly IOutwardItemShopToGodownService _OutwardItemShopToGodownService;
        private readonly IStockItemDistributionService _stockitemdistributionservice;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IPurchaseInventoryTaxService _PurchaseInventoryTaxService;
        private readonly IItemTaxService _ItemTaxService;

        public InwardController(IInwardFromSupplierService PerformaInvoiceService, IPurchaseOrderDetailService purchaseOrderDetailService,
            IPurchaseItemDetailService purchaseItemDetailService, IUtilityService utilityservice, ISuppliersMasterService supplierMasterService,
            ITransportService TransportService, IInwardItemFromSupplierService InwardItemFromSupplierService, IUserCredentialService usercredentialservice,
            IModuleService iIModuleService, IGodownService godownMasterService, IGodownAddressService godownAddressService,
            IItemCategoryService itemcategoryservice, IItemSubCategoryService itemsubcategoryservice, IUnitService unitmasterservice, IDesignService DesignService,
            IItemService ItemService, IOpeningStockService OpeningStockService, IBarcodeNumberService BarcodeNumberService, IOutwardToShopService OutwardFromGodownToShopService,
            IOutwardItemToShopService OutwardItemFromGodownToShopService, IShopService ShopService, IInwardFromGodownService InwardFromGodownService,
            IInwardItemFromGodownService InwardItemFromGodownService, IShopStockService ShopStockService, IGodownStockService GodownStockService, IInwardFromStkDistService InwardFromStkDistService,
            IOutwardStockDistributionService OutwardStkDisService, IOutwardItemStockDistributionService OutwardItemStkDisService, IInwardItemFromStockDistributionService InwardItemFromStockDistributionService,
            IEmployeeMasterService EmployeeMasterService, IPurchaseInventoryTaxService PurchaseInventoryTaxService, IItemTaxService ItemTaxService,
            INonInventoryItemService NonInventoryItemService, IInwardFromShopToGodownService InwardFromShopToGodownService, IInwardItemFromShopToGodownService InwardItemFromShopToGodownService,
            IOutwardShopToGodownService OutwardShopToGodownService, IOutwardItemShopToGodownService OutwardItemShopToGodownService, IStockItemDistributionService stockitemdistributionservice,
            IInwardInterGodownService InwardInterGodownService, IInwardInterGodownItemService InwardInterGodownItemService, IOutwardInterGodownService OutwardInterGodownService, IOutwardItemInterGodownService OutwardInterGodownItemService,
            IInwardInterShopService InwardInterShopService, IInwardInterShopItemService InwardInterShopItemService, IOutwardItemInterShopservice OutwardInterShopItemService, IOutwardInterShopService OutwardInterShopService, IEntryStockItemService EntryStockItemService)
        {
            this._InwardInterShopService = InwardInterShopService;
            this._InwardInterShopItemService = InwardInterShopItemService;
            this._InwardInterGodownService = InwardInterGodownService;
            this._OutwardInterGodownItemService = OutwardInterGodownItemService;
            this._OutwardInterShopService = OutwardInterShopService;
            this._OutwardInterShopItemService = OutwardInterShopItemService;
            this._OutwardInterGodownService = OutwardInterGodownService;
            this._InwardInterGodownItemService = InwardInterGodownItemService;
            this._stockitemdistributionservice = stockitemdistributionservice;
            this._OutwardShopToGodownService = OutwardShopToGodownService;
            this._OutwardItemShopToGodownService = OutwardItemShopToGodownService;
            this._InwardFromShopToGodownService = InwardFromShopToGodownService;
            this._InwardItemFromShopToGodownService = InwardItemFromShopToGodownService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._InwardItemFromStockDistributionService = InwardItemFromStockDistributionService;
            this._InwardFromSupplierService = PerformaInvoiceService;
            this._purchaseOrderDetailService = purchaseOrderDetailService;
            this._purchaseItemDetailService = purchaseItemDetailService;
            this._utilityService = utilityservice;
            this._supplierMasterService = supplierMasterService;
            this._TransportService = TransportService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._godownMasterService = godownMasterService;
            this._godownAddressService = godownAddressService;
            this._itemcategoryservice = itemcategoryservice;
            this._itemsubcategoryservice = itemsubcategoryservice;
            this._unitmasterservice = unitmasterservice;
            this._DesignService = DesignService;
            this._ItemService = ItemService;
            this._OpeningStockService = OpeningStockService;
            this._BarcodeNumberService = BarcodeNumberService;
            this._OutwardFromGodownToShopService = OutwardFromGodownToShopService;
            this._OutwardItemFromGodownToShopService = OutwardItemFromGodownToShopService;
            this._ShopService = ShopService;
            this._InwardFromGodownService = InwardFromGodownService;
            this._InwardItemFromGodownService = InwardItemFromGodownService;
            this._ShopStockService = ShopStockService;
            this._GodownStockService = GodownStockService;
            this._InwardFromStkDistService = InwardFromStkDistService;
            this._OutwardStkDisService = OutwardStkDisService;
            this._OutwardItemStkDisService = OutwardItemStkDisService;
            this._NonInventoryItemService = NonInventoryItemService;
            this._EntryStockItemService = EntryStockItemService;
            this._PurchaseInventoryTaxService = PurchaseInventoryTaxService;
            this._ItemTaxService = ItemTaxService;
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
        public JsonResult EncodeInwardId(int id)
        {
            return Json(Encode(id.ToString()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SupplierInwards()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        // ************************************** START INWARD FROM SUPPLIER ******************************************

        [HttpGet]
        public ActionResult InwardFromSupplier()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSupplierDetails(string name)
        {
            var supplierlist = _supplierMasterService.GetByName(name);

            var PONoList = _purchaseOrderDetailService.GetPendingPoNosBySupplier(name);
            var ponos = PONoList.Select(m => new SelectListItem
            {
                Text = m.PoNo,
                Value = m.PoNo
            });
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
                supplierlist.SupplierName,
                supplierlist.Address,
                supplierlist.ContactNo1,
                supplierlist.Email,
                supplierlist.State,
                ponos,
                supplierlist.Registered,
                godownNames,
                shopnames
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetItemTax(string itemname, string subcat, string State, string Formtype)
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
            return Json(taxPercent, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInventoryItems()
        {
            var itemlist = _ItemService.getAllItems();
            MainApplication model = new MainApplication();
            model.ItemList = itemlist;
            var modeldata = model.ItemList.Select(m => new SelectListItem()
            {
                Text = m.itemName,
                Value = m.itemCode
            });
            return Json(modeldata, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetInventoryAndNonInvItemDetailsFromItems(string itemname, string State, string Formtype, string ItemType)
        {
            if (ItemType == "Inventory")
            {
                var itemdetails = _ItemService.GetDescriptionByItemCode(itemname);
                string subcat = itemdetails.itemSubCategory;
                string typematerial = itemdetails.typeOfMaterial;
                string colorname = itemdetails.colorCode;
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
                return Json(new
                {
                    taxPercent,
                    subcat,
                    itemdetails.UniversalBarcode,
                    itemdetails.NumberType,
                    itemdetails.Barcode,
                    itemdetails.unit,
                    itemdetails.itemName,
                    itemdetails.sellingprice,
                    itemdetails.mrp,
                    itemdetails.costprice,
                    itemdetails.description,
                    typematerial,
                    itemdetails.designCode,
                    itemdetails.designName,
                    colorname,
                    itemdetails.brandName,
                    itemdetails.size,
                    select
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = _NonInventoryItemService.GetDetailsByItemCode(itemname);
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
                return Json(new
                {
                    taxPercent,
                    subcat,
                    data.itemName,
                    data.typeOfMaterial,
                    data.unit,
                    data.NumberType,
                    data.description,
                    data.colorCode,
                    data.sellingprice,
                    data.mrp,
                    data.costprice,
                    data.designCode,
                    data.designName,
                    data.brandName,
                    data.size,
                    select
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDesignNameByCode(string code)
        {
            string designName = _ItemService.GetDesignNameByDesignCode(code).designName;
            return Json(designName, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPODetails(string pono)
        {
            var detail = _InwardFromSupplierService.GetDetailsByPoNo(pono);
            string msg = string.Empty;
            var podetail = _purchaseOrderDetailService.GetDetailByPoNo(pono);
            string DelDate = podetail.DelDate.ToShortDateString();
            string PODate = podetail.PoDate.ToShortDateString();
            string remarks = string.Empty;
            double count = 0;
            if (podetail.DelDate.Date < DateTime.Now.Date)
            {
                count = (DateTime.Now.Date - podetail.DelDate.Date).TotalDays;
                if (count != 1)
                {
                    remarks = (int)count + " days delay";
                }
                else { remarks = (int)count + " day delay"; }
            }
            else if (podetail.DelDate.Date >= DateTime.Now.Date)
            {
                count = (podetail.DelDate.Date - DateTime.Now.Date).TotalDays;
                if (count != 1 && count != 0)
                {
                    remarks = (int)count + " days earlier";
                }
                else { remarks = (int)count + " day earlier"; }
            }

            if (detail == null)
            {
                msg = "From PO";
            }
            else
            {
                msg = "From Performa Invoice";
            }

            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            if (podetail.GodownCode != null)
            {
                godownshopcode = podetail.GodownCode;
                godownshopname = podetail.GodownName;
            }
            else
            {
                godownshopcode = podetail.ShopCode;
                godownshopname = podetail.ShopName;
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastInward = _InwardFromSupplierService.GetLastInwardWithPOByFinYr(FinYr, godownshopcode);
            string InwardCode = string.Empty;
            int InwardNoLength = 0;
            int InwardNo = 0;
            if (LastInward == null)
            {
                InwardNoLength = 1;
                InwardNo = 1;
            }
            else
            {
                int indexinwardwithpo = LastInward.InwardNo.LastIndexOf('I');
                InwardCode = LastInward.InwardNo.Substring(indexinwardwithpo + 2, 6);
                InwardNoLength = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                InwardNo = Convert.ToInt32(InwardCode) + 1;
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
            InwardCode = _utilityService.getName(ShortCode + "/IW", InwardNoLength, InwardNo);
            InwardCode = InwardCode + FinYr;
            TempData["PreviousSupplierInward"] = InwardCode;

            return Json(new
            {
                DelDate,
                PODate,
                podetail.DelAt,
                remarks,
                msg,
                InwardCode,
                podetail.ModeOfTransport,
                podetail.TransportContactNo,
                podetail.TransportName,
                podetail.GodownCode,
                podetail.GodownName,
                podetail.GodownArea,
                podetail.GodownEmail,
                podetail.GodownContactNo,
                podetail.GodownContactPerson,
                podetail.ShopCode,
                podetail.ShopName,
                podetail.ShopEmail,
                podetail.ShopContactNo,
                podetail.ShopContactPerson,
                podetail.FormType
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetItemsListFromPo(string pono)
        {
            MainApplication model = new MainApplication();
            model.PurchaseOrderData = _purchaseOrderDetailService.GetDetailByPoNo(pono);
            model.PurchaseItemList = _purchaseItemDetailService.GetPurchaseItemsByPONo(pono);
            TempData["PurchaseItemList"] = model.PurchaseItemList;
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(pono);
            if (model.PurchaseInventoryTaxList.Count() != 0)
            {
                TempData["InwardTaxList"] = model.PurchaseInventoryTaxList;
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetChallan(string pono)
        {
            var data = _InwardFromSupplierService.GetDetailsByPoNo(pono);
            string challandate = Convert.ToDateTime(data.ChallanDate).ToShortDateString();
            string billdate = Convert.ToDateTime(data.BillDate).ToShortDateString();
            return Json(new
            {

                challanno = data.ChallanNo,
                challandate,
                billno = data.BillNo,
                billdate,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemsListFromInward(string pono)
        {
            MainApplication model = new MainApplication();
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@pono",pono),
            };
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetInwardItemsByPoNo(pono).LastOrDefault();
            string query = "GetItemCodesList" + " " + "@pono";
            var list = _InwardItemFromSupplierService.GetItemCodesByPoNo(query, para);
            List<InwardItemsFromSupplier> newlist = new List<InwardItemsFromSupplier>();
            foreach (var details in list)
            {
                double? quantity = 0;
                var itemData = _InwardItemFromSupplierService.GetItemDetails(details.itemCode, pono);
                var itemlist = _InwardItemFromSupplierService.GetAllQuantity(details.itemCode, pono);
                foreach (var qtybal in itemlist)
                {
                    if (qtybal.OrderedQuantity != 0)
                    {
                        quantity += qtybal.ReceivedQuantity;
                    }
                }
                itemData.Balance = itemData.OrderedQuantity - quantity;
                itemData.ReceivedQuantity = quantity;
                if (itemData.Balance > 0)
                {
                    newlist.Add(itemData);
                }

            }
            model.InwardItemsFromSupplierList = newlist;
            if (model.InwardItemsFromSupplierList.Count() == 0)
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
            }
            TempData["PerformaItemList"] = newlist;
            var lastCreatedInwardOfPo = _InwardFromSupplierService.GetInwardItemsByPoNo(pono).LastOrDefault();
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(lastCreatedInwardOfPo.InwardNo);
            if (model.PurchaseInventoryTaxList.Count() != 0)
            {
                TempData["InwardTaxList"] = model.PurchaseInventoryTaxList;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardFromSupplier(MainApplication model, FormCollection frmcol)
        {
            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            if (model.InwardFromSupplierDetails.GodownCode != null)
            {
                godownshopcode = model.InwardFromSupplierDetails.GodownCode;
                godownshopname = model.InwardFromSupplierDetails.GodownName;
            }
            else
            {
                godownshopcode = model.InwardFromSupplierDetails.ShopCode;
                godownshopname = model.InwardFromSupplierDetails.ShopName;
            }
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastInward = _InwardFromSupplierService.GetLastInwardWithPOByFinYr(FinYr, godownshopcode);
            string InwardCode = string.Empty;
            int InwardNoLength = 0;
            int InwardNo = 0;
            if (LastInward == null)
            {
                InwardNoLength = 1;
                InwardNo = 1;
            }
            else
            {
                int indexinwardwithpo = LastInward.InwardNo.LastIndexOf('I');
                InwardCode = LastInward.InwardNo.Substring(indexinwardwithpo + 2, 6);
                InwardNoLength = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                InwardNo = Convert.ToInt32(InwardCode) + 1;
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
            InwardCode = _utilityService.getName(ShortCode + "/IW", InwardNoLength, InwardNo);
            InwardCode = InwardCode + FinYr;

            string openingstockcode = string.Empty;
            var openingstockdata = _OpeningStockService.GetLastStockByFinYr(FinYr);
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
            openingstockcode = _utilityService.getName("OST", length, openstockVal);
            openingstockcode = openingstockcode + FinYr;

            string msg = frmcol["GetItemListFrom"];
            int count = 1;
            int rowcount = Convert.ToInt32(frmcol["hdnRowCount"]);

            if (msg == "From PO")
            {

                var PurchaseItemList = TempData["PurchaseItemList"] as IEnumerable<PurchaseItemDetail>;
                foreach (var data in PurchaseItemList)
                {
                    string receivedquantity = "receivedquantity" + count;
                    string barcode = "barcode" + count;
                    string balance = "balanceqtyvalue" + count;
                    string amount = "amountvalue" + count;
                    string extraqty = "ExtraQty" + count;
                    string freeqty = "FreeQty" + count;
                    string itemtax = "ItemTaxValue" + count;
                    string proptax = "proportionatetaxvalue" + count;
                    string proptaxamt = "proportionatetaxamount" + count;
                    string newpopupitem = "NewPopUpItem" + count;
                    InwardItemsFromSupplier PerformaInvoiceItemData = new InwardItemsFromSupplier();
                    PerformaInvoiceItemData.InwardNo = InwardCode;
                    PerformaInvoiceItemData.PoNo = model.InwardFromSupplierDetails.PoNo;
                    PerformaInvoiceItemData.Item = data.Item;
                    PerformaInvoiceItemData.itemCode = data.itemCode;
                    PerformaInvoiceItemData.Barcode = frmcol[barcode];
                    PerformaInvoiceItemData.Description = data.Description;
                    PerformaInvoiceItemData.Color = data.Color;
                    PerformaInvoiceItemData.Material = data.Material;
                    PerformaInvoiceItemData.Brand = data.Brand;
                    PerformaInvoiceItemData.Size = data.Size;
                    PerformaInvoiceItemData.Design = data.Design;
                    PerformaInvoiceItemData.DesignName = data.DesignName;
                    PerformaInvoiceItemData.OrderedQuantity = Convert.ToDouble(data.Quantity);
                    if (frmcol[newpopupitem] != null)
                    {
                        PerformaInvoiceItemData.OrderedQuantity = 0;
                    }
                    if (Convert.ToDouble(frmcol[extraqty]) != 0)
                    {
                        PerformaInvoiceItemData.ReceivedQuantity = Convert.ToDouble(frmcol[receivedquantity]) - Convert.ToDouble(frmcol[extraqty]);
                    }
                    else
                    {
                        PerformaInvoiceItemData.ReceivedQuantity = Convert.ToDouble(frmcol[receivedquantity]);
                    }
                    PerformaInvoiceItemData.ExtraQty = Convert.ToDouble(frmcol[extraqty]);
                    if (frmcol[freeqty] == "")
                    {
                        PerformaInvoiceItemData.FreeQty = 0;
                    }
                    else
                    {
                        PerformaInvoiceItemData.FreeQty = Convert.ToDouble(frmcol[freeqty]);
                    }
                    PerformaInvoiceItemData.Balance = Convert.ToDouble(frmcol[balance]);
                    PerformaInvoiceItemData.Unit = data.Unit;
                    PerformaInvoiceItemData.NumberType = data.NumberType;
                    PerformaInvoiceItemData.SellingPrice = data.SellingPrice;
                    PerformaInvoiceItemData.MRP = data.MRP;
                    PerformaInvoiceItemData.CostPrice = data.CostPrice;
                    PerformaInvoiceItemData.Discount = Convert.ToDouble(data.DiscountPercent);
                    PerformaInvoiceItemData.Amount = Convert.ToDouble(frmcol[amount]);
                    PerformaInvoiceItemData.Category = data.Category;
                    PerformaInvoiceItemData.SubCategory = data.SubCategory;
                    PerformaInvoiceItemData.ItemTax = frmcol[itemtax];
                    PerformaInvoiceItemData.ItemType = data.ItemType;
                    PerformaInvoiceItemData.ProportionateTax = Convert.ToDouble(frmcol[proptax]);
                    PerformaInvoiceItemData.ProportionateTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                    PerformaInvoiceItemData.Status = "Active";
                    PerformaInvoiceItemData.ModifiedOn = DateTime.Now;
                    PerformaInvoiceItemData.PurchaseReturn = "No";
                    if (PerformaInvoiceItemData.ItemType == "Inventory")
                    {
                        var itemdetails = _ItemService.GetDescriptionByItemCode(PerformaInvoiceItemData.itemCode);
                        if (itemdetails.Barcode != null)
                        {
                            PerformaInvoiceItemData.Barcode = itemdetails.Barcode;
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                            string barcodeNo = string.Empty;
                            var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                            int serialNo = 1;
                            /*To Check if it's the first barcode*/
                            if (Lastbarcode == null)
                            {
                                serialNo = 1111111;
                            }
                            else
                            {
                                int pos = Lastbarcode.Number.IndexOf(".");
                                int position = (pos - 2);
                                serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                serialNo += 1;
                            }
                            /*Creation Of Barcode*/
                            barcodeNo = PerformaInvoiceItemData.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + PerformaInvoiceItemData.Item.Substring(PerformaInvoiceItemData.Item.Length - 1).ToUpper();
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
                            BarcodeNumber BarcodeNumber = new BarcodeNumber();
                            BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                            BarcodeNumber.BarcodeImage = bytes;
                            BarcodeNumber.ModifiedDate = DateTime.Now;
                            _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                            fs.Close();
                            br.Dispose();

                            FileInfo file = new FileInfo(filepath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }

                            PerformaInvoiceItemData.Barcode = barcodeNo + ".png";
                            var itemdata = _ItemService.GetDescriptionByItemCode(PerformaInvoiceItemData.itemCode);
                            itemdata.Barcode = PerformaInvoiceItemData.Barcode;
                            _ItemService.updateItem(itemdata);

                            OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                            OpeningStockMaster.OpeningStockCode = openingstockcode;
                            OpeningStockMaster.itemCode = PerformaInvoiceItemData.itemCode;
                            OpeningStockMaster.ItemName = PerformaInvoiceItemData.Item;
                            OpeningStockMaster.Barcode = PerformaInvoiceItemData.Barcode;
                            OpeningStockMaster.Brand = PerformaInvoiceItemData.Brand;
                            OpeningStockMaster.Category = PerformaInvoiceItemData.Category;
                            OpeningStockMaster.SubCategory = PerformaInvoiceItemData.SubCategory;
                            OpeningStockMaster.Color = PerformaInvoiceItemData.Color;
                            OpeningStockMaster.Description = PerformaInvoiceItemData.Description;
                            OpeningStockMaster.DesignCode = PerformaInvoiceItemData.Design;
                            OpeningStockMaster.DesignName = PerformaInvoiceItemData.DesignName;
                            OpeningStockMaster.Material = PerformaInvoiceItemData.Material;
                            OpeningStockMaster.Unit = PerformaInvoiceItemData.Unit;
                            OpeningStockMaster.Size = PerformaInvoiceItemData.Size;
                            OpeningStockMaster.NumberType = PerformaInvoiceItemData.NumberType;
                            OpeningStockMaster.ItemQuantity = 0;
                            OpeningStockMaster.TotalQuantity = 0;
                            OpeningStockMaster.CostPrice = PerformaInvoiceItemData.CostPrice;
                            OpeningStockMaster.SellingPrice = PerformaInvoiceItemData.SellingPrice;
                            OpeningStockMaster.MRP = PerformaInvoiceItemData.MRP;
                            OpeningStockMaster.Status = "Active";
                            OpeningStockMaster.ModifiedOn = DateTime.Now;
                            _OpeningStockService.CreateStock(OpeningStockMaster);
                        }
                    }
                    _InwardItemFromSupplierService.CreateInwardItems(PerformaInvoiceItemData);
                    count += 1;
                }

                //Save All The Dynamically Added Items
                for (int i = 1; i <= rowcount; i++)
                {
                    string subcat = "SubCat" + i;
                    string ItemType = "ItemType" + i;
                    string itemCode = "itemCode" + i;
                    string itemName = "itemName" + i;
                    string barcode = "barcode" + i;
                    string descriptionvalue = "descriptionvalue" + i;
                    string materialvalue = "materialvalue" + i;
                    string colorvalue = "colorvalue" + i;
                    string quantity = "receivedquantity" + i;
                    string freeqty = "FreeQty" + i;
                    string rate = "rate" + i;
                    string mrp = "MRP" + i;
                    string sellingprice = "sellingprice" + i;
                    string discount = "discount" + i;
                    string amountvalue = "amountvalue" + i;
                    string unitvalue = "unitvalue" + i;
                    string numbertype = "numberType" + i;
                    string designcode = "design" + i;
                    string designNameValue = "designNameValue" + i;
                    string brandvalue = "brandvalue" + i;
                    string sizevalue = "sizevalue" + i;
                    string itemtaxvalue = "ItemTaxValue" + i;
                    string newlyadded = "NewAdded" + i;
                    string proptax = "proportionatetaxvalue" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    if (frmcol[newlyadded] != null && Convert.ToDouble(frmcol[quantity]) != 0)
                    {
                        InwardItemsFromSupplier PerformaInvoiceItemData = new InwardItemsFromSupplier();
                        PerformaInvoiceItemData.InwardNo = InwardCode;
                        PerformaInvoiceItemData.PoNo = model.InwardFromSupplierDetails.PoNo;
                        PerformaInvoiceItemData.Item = frmcol[itemName];
                        PerformaInvoiceItemData.itemCode = frmcol[itemCode];
                        PerformaInvoiceItemData.Barcode = frmcol[barcode];
                        PerformaInvoiceItemData.Description = frmcol[descriptionvalue];
                        PerformaInvoiceItemData.Color = frmcol[colorvalue];
                        PerformaInvoiceItemData.Brand = frmcol[brandvalue];
                        PerformaInvoiceItemData.Size = frmcol[sizevalue];
                        PerformaInvoiceItemData.Material = frmcol[materialvalue];
                        PerformaInvoiceItemData.Design = frmcol[designcode];
                        PerformaInvoiceItemData.DesignName = frmcol[designNameValue];
                        PerformaInvoiceItemData.OrderedQuantity = 0;
                        PerformaInvoiceItemData.ReceivedQuantity = Convert.ToDouble(frmcol[quantity]);
                        PerformaInvoiceItemData.ExtraQty = 0;
                        if (frmcol[freeqty] == "")
                        {
                            PerformaInvoiceItemData.FreeQty = 0;
                        }
                        else
                        {
                            PerformaInvoiceItemData.FreeQty = Convert.ToDouble(frmcol[freeqty]);
                        }
                        PerformaInvoiceItemData.Balance = 0;
                        PerformaInvoiceItemData.Unit = frmcol[unitvalue];
                        PerformaInvoiceItemData.NumberType = frmcol[numbertype];
                        PerformaInvoiceItemData.CostPrice = Convert.ToDouble(frmcol[rate]);
                        if (frmcol[mrp] != "")
                        {
                            PerformaInvoiceItemData.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            PerformaInvoiceItemData.MRP = 0;
                        }

                        if (frmcol[sellingprice] != "")
                        {
                            PerformaInvoiceItemData.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        }
                        else
                        {
                            PerformaInvoiceItemData.SellingPrice = 0;
                        }

                        if (frmcol[discount] != "")
                        {
                            PerformaInvoiceItemData.Discount = Convert.ToDouble(frmcol[discount]);
                        }
                        else
                        {
                            PerformaInvoiceItemData.Discount = 0;
                        }
                        PerformaInvoiceItemData.Amount = Convert.ToDouble(frmcol[amountvalue]);
                        PerformaInvoiceItemData.Category = _itemsubcategoryservice.GetCategoryBySubCat(frmcol[subcat]);
                        PerformaInvoiceItemData.SubCategory = frmcol[subcat];
                        PerformaInvoiceItemData.ItemTax = frmcol[itemtaxvalue];
                        PerformaInvoiceItemData.ItemType = frmcol[ItemType];
                        PerformaInvoiceItemData.Status = "Active";
                        PerformaInvoiceItemData.ModifiedOn = DateTime.Now;
                        PerformaInvoiceItemData.ProportionateTax = Convert.ToDouble(frmcol[proptax]);
                        PerformaInvoiceItemData.ProportionateTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        PerformaInvoiceItemData.PurchaseReturn = "No";
                        if (PerformaInvoiceItemData.ItemType == "Inventory")
                        {
                            var itemdetails = _ItemService.GetDescriptionByItemCode(PerformaInvoiceItemData.itemCode);
                            if (itemdetails.Barcode != null)
                            {
                                PerformaInvoiceItemData.Barcode = itemdetails.Barcode;
                            }
                            else
                            {
                                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                string barcodeNo = string.Empty;
                                var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                                int serialNo;
                                /*To Check if it's the first barcode*/
                                if (Lastbarcode == null)
                                {
                                    serialNo = 1111111;
                                }
                                else
                                {
                                    int position = (Lastbarcode.Number.IndexOf(".") - 2);
                                    serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                    serialNo += 1;
                                }
                                /*Creation Of Barcode*/
                                barcodeNo = PerformaInvoiceItemData.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + PerformaInvoiceItemData.Item.Substring(PerformaInvoiceItemData.Item.Length - 1).ToUpper();
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
                                BarcodeNumber BarcodeNumber = new BarcodeNumber();
                                BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                                BarcodeNumber.BarcodeImage = bytes;
                                BarcodeNumber.ModifiedDate = DateTime.Now;
                                _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                                fs.Close();
                                br.Dispose();

                                FileInfo file = new FileInfo(filepath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                                PerformaInvoiceItemData.Barcode = barcodeNo + ".png";
                                var itemdata = _ItemService.GetDescriptionByItemCode(PerformaInvoiceItemData.itemCode);
                                itemdata.Barcode = PerformaInvoiceItemData.Barcode;
                                _ItemService.updateItem(itemdata);

                                OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                                OpeningStockMaster.OpeningStockCode = openingstockcode;
                                OpeningStockMaster.itemCode = PerformaInvoiceItemData.itemCode;
                                OpeningStockMaster.ItemName = PerformaInvoiceItemData.Item;
                                OpeningStockMaster.Barcode = PerformaInvoiceItemData.Barcode;
                                OpeningStockMaster.Brand = PerformaInvoiceItemData.Brand;
                                OpeningStockMaster.Category = PerformaInvoiceItemData.Category;
                                OpeningStockMaster.SubCategory = PerformaInvoiceItemData.SubCategory;
                                OpeningStockMaster.Color = PerformaInvoiceItemData.Color;
                                OpeningStockMaster.Description = PerformaInvoiceItemData.Description;
                                OpeningStockMaster.DesignCode = PerformaInvoiceItemData.Design;
                                OpeningStockMaster.DesignName = PerformaInvoiceItemData.DesignName;
                                OpeningStockMaster.Material = PerformaInvoiceItemData.Material;
                                OpeningStockMaster.Unit = PerformaInvoiceItemData.Unit;
                                OpeningStockMaster.Size = PerformaInvoiceItemData.Size;
                                OpeningStockMaster.NumberType = PerformaInvoiceItemData.NumberType;
                                OpeningStockMaster.ItemQuantity = 0;
                                OpeningStockMaster.TotalQuantity = 0;
                                OpeningStockMaster.CostPrice = PerformaInvoiceItemData.CostPrice;
                                OpeningStockMaster.SellingPrice = PerformaInvoiceItemData.SellingPrice;
                                OpeningStockMaster.MRP = PerformaInvoiceItemData.MRP;
                                OpeningStockMaster.Status = "Active";
                                OpeningStockMaster.ModifiedOn = DateTime.Now;
                                _OpeningStockService.CreateStock(OpeningStockMaster);
                            }
                        }
                        _InwardItemFromSupplierService.CreateInwardItems(PerformaInvoiceItemData);
                    }
                }

                var podetails = _purchaseOrderDetailService.GetDetailByPoNo(model.InwardFromSupplierDetails.PoNo);
                podetails.status = "Active";
                podetails.ReceivingStatus = "InComplete";
                _purchaseOrderDetailService.UpdatePurchaseOrder(podetails);
            }
            else
            {
                var PurchaseItemList = TempData["PerformaItemList"] as IEnumerable<InwardItemsFromSupplier>;
                foreach (var data in PurchaseItemList)
                {
                    string receivedquantity = "receivedquantity" + count;
                    string balance = "balanceqtyvalue" + count;
                    string barcode = "barcode" + count;
                    string amount = "amountvalue" + count;
                    string extraqty = "ExtraQty" + count;
                    string freeqty = "FreeQty" + count;
                    string itemtax = "ItemTaxValue" + count;
                    string proptax = "proportionatetaxvalue" + count;
                    string proptaxamt = "proportionatetaxamount" + count;
                    if (Convert.ToDouble(frmcol[receivedquantity]) != 0)
                    {
                        data.InwardNo = InwardCode;
                        data.PoNo = model.InwardFromSupplierDetails.PoNo;
                        if (Convert.ToDouble(frmcol[extraqty]) != 0)
                        {
                            data.ReceivedQuantity = Convert.ToDouble(frmcol[receivedquantity]) - Convert.ToDouble(frmcol[extraqty]);
                        }
                        else
                        {
                            data.ReceivedQuantity = Convert.ToDouble(frmcol[receivedquantity]);
                        }
                        data.ExtraQty = Convert.ToDouble(frmcol[extraqty]);
                        if (frmcol[freeqty] == "")
                        {
                            data.FreeQty = 0;
                        }
                        else
                        {
                            data.FreeQty = Convert.ToDouble(frmcol[freeqty]);
                        }
                        data.Balance = Convert.ToDouble(frmcol[balance]);
                        data.Amount = Convert.ToDouble(frmcol[amount]);
                        data.ItemTax = frmcol[itemtax];
                        data.ProportionateTax = Convert.ToDouble(frmcol[proptax]);
                        data.ProportionateTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        data.Status = "Active";
                        data.ModifiedOn = DateTime.Now;
                        data.PurchaseReturn = "No";
                        if (data.ItemType == "Inventory")
                        {
                            var itemdetails = _ItemService.GetDescriptionByItemCode(data.itemCode);
                            if (itemdetails.Barcode != null)
                            {
                                data.Barcode = itemdetails.Barcode;
                            }
                            else
                            {
                                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                string barcodeNo = string.Empty;
                                var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                                int serialNo;
                                /*To Check if it's the first barcode*/
                                if (Lastbarcode == null)
                                {
                                    serialNo = 1111111;
                                }
                                else
                                {
                                    int position = (Lastbarcode.Number.IndexOf(".") - 2);
                                    serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                    serialNo += 1;
                                }
                                /*Creation Of Barcode*/
                                barcodeNo = data.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + data.Item.Substring(data.Item.Length - 1).ToUpper();
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
                                BarcodeNumber BarcodeNumber = new BarcodeNumber();
                                BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                                BarcodeNumber.BarcodeImage = bytes;
                                BarcodeNumber.ModifiedDate = DateTime.Now;
                                _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                                fs.Close();
                                br.Dispose();

                                FileInfo file = new FileInfo(filepath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                                data.Barcode = barcodeNo + ".png";
                                var itemdata = _ItemService.GetDescriptionByItemCode(data.itemCode);
                                itemdata.Barcode = data.Barcode;
                                _ItemService.updateItem(itemdata);

                                OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                                OpeningStockMaster.OpeningStockCode = openingstockcode;
                                OpeningStockMaster.itemCode = data.itemCode;
                                OpeningStockMaster.ItemName = data.Item;
                                OpeningStockMaster.Barcode = data.Barcode;
                                OpeningStockMaster.Brand = data.Brand;
                                OpeningStockMaster.Category = data.Category;
                                OpeningStockMaster.SubCategory = data.SubCategory;
                                OpeningStockMaster.Color = data.Color;
                                OpeningStockMaster.Description = data.Description;
                                OpeningStockMaster.DesignCode = data.Design;
                                OpeningStockMaster.DesignName = data.DesignName;
                                OpeningStockMaster.Material = data.Material;
                                OpeningStockMaster.Unit = data.Unit;
                                OpeningStockMaster.Size = data.Size;
                                OpeningStockMaster.NumberType = data.NumberType;
                                OpeningStockMaster.ItemQuantity = 0;
                                OpeningStockMaster.TotalQuantity = 0;
                                OpeningStockMaster.CostPrice = data.CostPrice;
                                OpeningStockMaster.SellingPrice = data.SellingPrice;
                                OpeningStockMaster.MRP = data.MRP;
                                OpeningStockMaster.Status = "Active";
                                OpeningStockMaster.ModifiedOn = DateTime.Now;
                                _OpeningStockService.CreateStock(OpeningStockMaster);
                            }
                        }
                        _InwardItemFromSupplierService.CreateInwardItems(data);
                    }
                    count += 1;
                }

                //Save All The Dynamically Added Items
                for (int i = 1; i <= rowcount; i++)
                {
                    string subcat = "SubCat" + i;
                    string ItemType = "ItemType" + i;
                    string itemCode = "itemCode" + i;
                    string itemName = "itemName" + i;
                    string barcode = "barcode" + i;
                    string descriptionvalue = "descriptionvalue" + i;
                    string materialvalue = "materialvalue" + i;
                    string colorvalue = "colorvalue" + i;
                    string quantity = "receivedquantity" + i;
                    string freeqty = "FreeQty" + i;
                    string rate = "rate" + i;
                    string mrp = "MRP" + i;
                    string sellingprice = "sellingprice" + i;
                    string discount = "discount" + i;
                    string amountvalue = "amountvalue" + i;
                    string unitvalue = "unitvalue" + i;
                    string numbertype = "numberType" + i;
                    string designcode = "design" + i;
                    string designNameValue = "designNameValue" + i;
                    string brandvalue = "brandvalue" + i;
                    string sizevalue = "sizevalue" + i;
                    string itemtaxvalue = "ItemTaxValue" + i;
                    string newlyadded = "NewAdded" + i;
                    string proptax = "proportionatetaxvalue" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    if (frmcol[newlyadded] != null && Convert.ToDouble(frmcol[quantity]) != 0)
                    {
                        InwardItemsFromSupplier PerformaInvoiceItemData = new InwardItemsFromSupplier();
                        PerformaInvoiceItemData.InwardNo = InwardCode;
                        PerformaInvoiceItemData.PoNo = model.InwardFromSupplierDetails.PoNo;
                        PerformaInvoiceItemData.Item = frmcol[itemName];
                        PerformaInvoiceItemData.itemCode = frmcol[itemCode];
                        PerformaInvoiceItemData.Barcode = frmcol[barcode];
                        PerformaInvoiceItemData.Description = frmcol[descriptionvalue];
                        PerformaInvoiceItemData.Color = frmcol[colorvalue];
                        PerformaInvoiceItemData.Material = frmcol[materialvalue];
                        PerformaInvoiceItemData.Brand = frmcol[brandvalue];
                        PerformaInvoiceItemData.Size = frmcol[sizevalue];
                        PerformaInvoiceItemData.Design = frmcol[designcode];
                        PerformaInvoiceItemData.DesignName = frmcol[designNameValue];
                        PerformaInvoiceItemData.OrderedQuantity = Convert.ToDouble(frmcol[quantity]);
                        PerformaInvoiceItemData.ReceivedQuantity = Convert.ToDouble(frmcol[quantity]);
                        PerformaInvoiceItemData.ExtraQty = 0;
                        if (frmcol[freeqty] == "")
                        {
                            PerformaInvoiceItemData.FreeQty = 0;
                        }
                        else
                        {
                            PerformaInvoiceItemData.FreeQty = Convert.ToDouble(frmcol[freeqty]);
                        }
                        PerformaInvoiceItemData.Balance = 0;
                        PerformaInvoiceItemData.Unit = frmcol[unitvalue];
                        PerformaInvoiceItemData.NumberType = frmcol[numbertype];
                        PerformaInvoiceItemData.CostPrice = Convert.ToDouble(frmcol[rate]);
                        if (frmcol[mrp] != "")
                        {
                            PerformaInvoiceItemData.MRP = Convert.ToDouble(frmcol[mrp]);
                        }
                        else
                        {
                            PerformaInvoiceItemData.MRP = 0;
                        }

                        if (frmcol[sellingprice] != "")
                        {
                            PerformaInvoiceItemData.SellingPrice = Convert.ToDouble(frmcol[sellingprice]);
                        }
                        else
                        {
                            PerformaInvoiceItemData.SellingPrice = 0;
                        }

                        if (frmcol[discount] != "")
                        {
                            PerformaInvoiceItemData.Discount = Convert.ToDouble(frmcol[discount]);
                        }
                        else
                        {
                            PerformaInvoiceItemData.Discount = 0;
                        }
                        PerformaInvoiceItemData.Amount = Convert.ToDouble(frmcol[amountvalue]);
                        PerformaInvoiceItemData.Category = _itemsubcategoryservice.GetCategoryBySubCat(frmcol[subcat]);
                        PerformaInvoiceItemData.SubCategory = frmcol[subcat];
                        PerformaInvoiceItemData.ItemTax = frmcol[itemtaxvalue];
                        PerformaInvoiceItemData.ItemType = frmcol[ItemType];
                        PerformaInvoiceItemData.ProportionateTax = Convert.ToDouble(frmcol[proptax]);
                        PerformaInvoiceItemData.ProportionateTaxAmt = Convert.ToDouble(frmcol[proptaxamt]);
                        PerformaInvoiceItemData.Status = "Active";
                        PerformaInvoiceItemData.ModifiedOn = DateTime.Now;
                        PerformaInvoiceItemData.PurchaseReturn = "No";
                        if (PerformaInvoiceItemData.ItemType == "Inventory")
                        {
                            var itemdetails = _ItemService.GetDescriptionByItemCode(PerformaInvoiceItemData.itemCode);
                            if (itemdetails.Barcode != null)
                            {
                                PerformaInvoiceItemData.Barcode = itemdetails.Barcode;
                            }
                            else
                            {
                                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                string barcodeNo = string.Empty;
                                var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                                int serialNo;
                                /*To Check if it's the first barcode*/
                                if (Lastbarcode == null)
                                {
                                    serialNo = 1111111;
                                }
                                else
                                {
                                    int position = (Lastbarcode.Number.IndexOf(".") - 2);
                                    serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                    serialNo += 1;
                                }
                                /*Creation Of Barcode*/
                                barcodeNo = PerformaInvoiceItemData.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + PerformaInvoiceItemData.Item.Substring(PerformaInvoiceItemData.Item.Length - 1).ToUpper();
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
                                BarcodeNumber BarcodeNumber = new BarcodeNumber();
                                BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                                BarcodeNumber.BarcodeImage = bytes;
                                BarcodeNumber.ModifiedDate = DateTime.Now;
                                _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                                fs.Close();
                                br.Dispose();

                                FileInfo file = new FileInfo(filepath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                                PerformaInvoiceItemData.Barcode = barcodeNo + ".png";
                                var itemdata = _ItemService.GetDescriptionByItemCode(PerformaInvoiceItemData.itemCode);
                                itemdata.Barcode = PerformaInvoiceItemData.Barcode;
                                _ItemService.updateItem(itemdata);

                                OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                                OpeningStockMaster.OpeningStockCode = openingstockcode;
                                OpeningStockMaster.itemCode = PerformaInvoiceItemData.itemCode;
                                OpeningStockMaster.ItemName = PerformaInvoiceItemData.Item;
                                OpeningStockMaster.Barcode = PerformaInvoiceItemData.Barcode;
                                OpeningStockMaster.Brand = PerformaInvoiceItemData.Brand;
                                OpeningStockMaster.Category = PerformaInvoiceItemData.Category;
                                OpeningStockMaster.SubCategory = PerformaInvoiceItemData.SubCategory;
                                OpeningStockMaster.Color = PerformaInvoiceItemData.Color;
                                OpeningStockMaster.Description = PerformaInvoiceItemData.Description;
                                OpeningStockMaster.DesignCode = PerformaInvoiceItemData.Design;
                                OpeningStockMaster.DesignName = PerformaInvoiceItemData.DesignName;
                                OpeningStockMaster.Material = PerformaInvoiceItemData.Material;
                                OpeningStockMaster.Unit = PerformaInvoiceItemData.Unit;
                                OpeningStockMaster.Size = PerformaInvoiceItemData.Size;
                                OpeningStockMaster.NumberType = PerformaInvoiceItemData.NumberType;
                                OpeningStockMaster.ItemQuantity = 0;
                                OpeningStockMaster.TotalQuantity = 0;
                                OpeningStockMaster.CostPrice = PerformaInvoiceItemData.CostPrice;
                                OpeningStockMaster.SellingPrice = PerformaInvoiceItemData.SellingPrice;
                                OpeningStockMaster.MRP = PerformaInvoiceItemData.MRP;
                                OpeningStockMaster.Status = "Active";
                                OpeningStockMaster.ModifiedOn = DateTime.Now;
                                _OpeningStockService.CreateStock(OpeningStockMaster);
                            }
                        }
                        _InwardItemFromSupplierService.CreateInwardItems(PerformaInvoiceItemData);
                    }
                }
            }

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ItemTaxCount"]);
            model.PurchaseInventoryTaxDetails = new PurchaseInventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    model.PurchaseInventoryTaxDetails.Code = InwardCode;
                    model.PurchaseInventoryTaxDetails.Amount = frmcol[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = frmcol[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }

            //Only Newly Created Inward Will Be Editable Not The Previous Ones
            var inwardlist = _InwardFromSupplierService.GetInwardItemsByPoNo(model.InwardFromSupplierDetails.PoNo);
            foreach (var inward in inwardlist)
            {
                if (inward.Editable == "Yes")
                {
                    inward.Editable = "No";
                    inward.ModifiedOn = DateTime.Now;
                    _InwardFromSupplierService.UpdateInward(inward);
                }
            }

            model.InwardFromSupplierDetails.ReceivingDate = DateTime.Now;
            model.InwardFromSupplierDetails.ModifiedOn = DateTime.Now;
            model.InwardFromSupplierDetails.InwardDate = DateTime.Now;
            model.InwardFromSupplierDetails.Editable = "Yes";
            model.InwardFromSupplierDetails.TotalAmount = Convert.ToDouble(frmcol["Total_Amount_Value"]);
            model.InwardFromSupplierDetails.TotalTaxAmount = Convert.ToDouble(frmcol["TotalTaxAmountValue"]);
            model.InwardFromSupplierDetails.GrandTotal = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.PaymentAmount = 0;
            model.InwardFromSupplierDetails.PaymentBalance = Convert.ToDouble(frmcol["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.TotalReceivedQuantity = frmcol["Total_Quantity_Value"];
            model.InwardFromSupplierDetails.TotalRemainingQuantity = frmcol["Total_Remaining_Quantity_Value"];
            model.InwardFromSupplierDetails.TotalExtraQuantity = frmcol["Total_Extra_Quantity_Value"];
            model.InwardFromSupplierDetails.Status = "Active";
            model.InwardFromSupplierDetails.PaymentStatus = "Active";
            model.InwardFromSupplierDetails.InwardNo = InwardCode;
            model.InwardFromSupplierDetails.DebitNotesAmount = 0;
            model.InwardFromSupplierDetails.PurchaseReturn = "No";
            _InwardFromSupplierService.CreateInward(model.InwardFromSupplierDetails);
            var details = _InwardFromSupplierService.GetDetailsByPINo(InwardCode);
            string InwardId = Encode(details.InwardId.ToString());
            return RedirectToAction("InwardFromSupplierDetails/" + InwardId, "Inward");
        }

        [HttpGet]
        public ActionResult InwardFromSupplierDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromSupplierDetails = new InwardFromSupplier(),
            };
            int Id = Decode(id);
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetById(Id);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.InwardFromSupplierDetails.InwardNo);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(model.InwardFromSupplierDetails.InwardNo);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string previousinwardno = TempData["PreviousSupplierInward"].ToString();
            if (previousinwardno != model.InwardFromSupplierDetails.InwardNo)
            {
                ViewData["SupplierInwardnoChanged"] = previousinwardno + " is replaced with " + model.InwardFromSupplierDetails.InwardNo + " because " + previousinwardno + " is acquired by another person";
            }
            TempData["PreviousSupplierInward"] = previousinwardno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardFromSupplier(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication()
            {
                InwardFromSupplierDetails = new InwardFromSupplier(),
            };
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetById(Id);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(model.InwardFromSupplierDetails.InwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardFromSupplierEdit()
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
        public JsonResult InwardFromSupplierList(string term)
        {
            MainApplication model = new MainApplication();
            List<string> names = new List<string>();
            var data = _InwardFromSupplierService.GetSupplierInwardList(term);
            foreach (var item in data)
            {
                names.Add(item.InwardNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InwardFromSupplierEditDetails(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetDetailsByPINo(InwardNo);
            TempData["SupplierInwardDetails"] = model.InwardFromSupplierDetails;
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(InwardNo);
            Session["SupplierInwardItemList"] = model.InwardItemsFromSupplierList;
            model.ItemSubCategoryList = _itemsubcategoryservice.GetItemSubCategories();
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.InwardFromSupplierDetails.InwardNo);
            if (model.PurchaseInventoryTaxList.Count() != 0)
            {
                Session["SupplierInwardTaxList"] = model.PurchaseInventoryTaxList;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardFromSupplierEditDetails(MainApplication model, FormCollection form)
        {
            model.InwardItemsFromSupplierDetails = new InwardItemsFromSupplier();
            model.InwardFromSupplierDetails = TempData["SupplierInwardDetails"] as InwardFromSupplier;
            model.InwardFromSupplierDetails.TotalAmount = Convert.ToDouble(form["Total_Amount_Value"]);
            model.InwardFromSupplierDetails.TotalTaxAmount = Convert.ToDouble(form["TotalTaxAmountValue"]);
            model.InwardFromSupplierDetails.GrandTotal = Convert.ToDouble(form["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.PaymentBalance = Convert.ToDouble(form["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.TotalReceivedQuantity = form["Total_Quantity_Value"];
            model.InwardFromSupplierDetails.TotalRemainingQuantity = form["Total_Remaining_Quantity_Value"];
            model.InwardFromSupplierDetails.TotalExtraQuantity = form["Total_Extra_Quantity_Value"];
            model.InwardFromSupplierDetails.ChallanNo = form["InwardFromSupplierDetails.ChallanNo"];
            if (model.InwardFromSupplierDetails.ChallanNo != "")
            {
                model.InwardFromSupplierDetails.ChallanDate = Convert.ToDateTime(form["InwardFromSupplierDetails.ChallanDate"]);
            }
            else
            {
                model.InwardFromSupplierDetails.ChallanDate = null;
                model.InwardFromSupplierDetails.ChallanNo = null;
            }
            model.InwardFromSupplierDetails.BillNo = form["InwardFromSupplierDetails.BillNo"];
            if (model.InwardFromSupplierDetails.BillNo != "")
            {
                model.InwardFromSupplierDetails.BillDate = Convert.ToDateTime(form["InwardFromSupplierDetails.BillDate"]);
            }
            else
            {
                model.InwardFromSupplierDetails.BillDate = null;
                model.InwardFromSupplierDetails.BillNo = null;
            }
            _InwardFromSupplierService.UpdateInward(model.InwardFromSupplierDetails);

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string openingstockcode = string.Empty;
            var openingstockdata = _OpeningStockService.GetLastStockByFinYr(FinYr);
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
            openingstockcode = _utilityService.getName("OST", length, openstockVal);
            openingstockcode = openingstockcode + FinYr;

            var data = Session["SupplierInwardItemList"] as IEnumerable<InwardItemsFromSupplier>;
            var itemcount = Convert.ToInt32(form["hdnRowCount"]);

            int count = 1;
            foreach (var item in data)
            {
                var receivedquantity = "receivedquantity" + count;
                var extraquantity = "ExtraQty" + count;
                var freequantity = "FreeQty" + count;
                var balanceqtyvalue = "balanceqtyvalue" + count;
                var amountvalue = "amountvalue" + count;

                item.ReceivedQuantity = Convert.ToDouble(form[receivedquantity]);
                item.Amount = Convert.ToDouble(form[amountvalue]);
                item.ExtraQty = Convert.ToDouble(form[extraquantity]);
                item.FreeQty = Convert.ToDouble(form[freequantity]);
                item.Balance = Convert.ToDouble(form[balanceqtyvalue]);
                _InwardItemFromSupplierService.Update(item);
                count++;
            }

            for (var i = count; i <= itemcount; i++)
            {
                string subcat = "SubCat" + i;
                string ItemType = "ItemType" + i;
                string itemCode = "itemCode" + i;
                string itemName = "itemName" + i;
                string barcode = "barcode" + i;
                string descriptionvalue = "descriptionvalue" + i;
                string materialvalue = "materialvalue" + i;
                string colorvalue = "colorvalue" + i;
                string receivedquantity = "receivedquantity" + i;
                string freeqty = "FreeQty" + i;
                string balanceqty = "balanceqty" + i;
                string rate = "rate" + i;
                string mrp = "MRP" + i;
                string sellingprice = "sellingprice" + i;
                string discount = "discount" + i;
                string amountvalue = "amountvalue" + i;
                string itemtaxvalue = "ItemTaxValue" + i;
                string unitvalue = "unitvalue" + i;
                string numbertype = "numberType" + i;
                string designcode = "design" + i;
                string designNameValue = "designNameValue" + i;
                string brandvalue = "brandvalue" + i;
                string sizevalue = "sizevalue" + i;
                string proptax = "proportionatetaxvalue" + i;
                string proptaxamt = "proportionatetaxamount" + i;
                if (form[itemCode] != null && form[receivedquantity] != null)
                {

                    model.InwardItemsFromSupplierDetails.Amount = Convert.ToDouble(form[amountvalue]);
                    model.InwardItemsFromSupplierDetails.Barcode = form[barcode];
                    if (form[freeqty] != "")
                    {
                        model.InwardItemsFromSupplierDetails.FreeQty = Convert.ToDouble(form[freeqty]);
                    }
                    else
                    {
                        model.InwardItemsFromSupplierDetails.FreeQty = 0;
                    }
                    model.InwardItemsFromSupplierDetails.Balance = 0;
                    model.InwardItemsFromSupplierDetails.Color = form[colorvalue];
                    model.InwardItemsFromSupplierDetails.Description = form[descriptionvalue];
                    model.InwardItemsFromSupplierDetails.Design = form[designcode];
                    model.InwardItemsFromSupplierDetails.DesignName = form[designNameValue];
                    model.InwardItemsFromSupplierDetails.Brand = form[brandvalue];
                    model.InwardItemsFromSupplierDetails.Size = form[sizevalue];
                    model.InwardItemsFromSupplierDetails.Discount = Convert.ToDouble(form[discount]);
                    model.InwardItemsFromSupplierDetails.ExtraQty = 0;
                    model.InwardItemsFromSupplierDetails.InwardNo = model.InwardFromSupplierDetails.InwardNo;
                    model.InwardItemsFromSupplierDetails.Item = form[itemName];
                    model.InwardItemsFromSupplierDetails.itemCode = form[itemCode];
                    model.InwardItemsFromSupplierDetails.ItemTax = form[itemtaxvalue];
                    model.InwardItemsFromSupplierDetails.ItemType = form[ItemType];
                    model.InwardItemsFromSupplierDetails.Material = form[materialvalue];
                    model.InwardItemsFromSupplierDetails.NumberType = form[numbertype];
                    model.InwardItemsFromSupplierDetails.OrderedQuantity = Convert.ToDouble(form[receivedquantity]);
                    model.InwardItemsFromSupplierDetails.PoNo = model.InwardFromSupplierDetails.PoNo;
                    model.InwardItemsFromSupplierDetails.CostPrice = Convert.ToDouble(form[rate]);
                    if (form[mrp] != "")
                    {
                        model.InwardItemsFromSupplierDetails.MRP = Convert.ToDouble(form[mrp]);
                    }
                    else
                    {
                        model.InwardItemsFromSupplierDetails.MRP = 0;
                    }
                    if (form[sellingprice] != "")
                    {
                        model.InwardItemsFromSupplierDetails.SellingPrice = Convert.ToDouble(form[sellingprice]);
                    }
                    else
                    {
                        model.InwardItemsFromSupplierDetails.SellingPrice = 0;
                    }
                    model.InwardItemsFromSupplierDetails.ReceivedQuantity = Convert.ToDouble(form[receivedquantity]);
                    model.InwardItemsFromSupplierDetails.Status = "Active";
                    model.InwardItemsFromSupplierDetails.Category = _itemsubcategoryservice.GetCategoryBySubCat(form[subcat]);
                    model.InwardItemsFromSupplierDetails.SubCategory = form[subcat];
                    model.InwardItemsFromSupplierDetails.Unit = form[unitvalue];
                    model.InwardItemsFromSupplierDetails.ModifiedOn = DateTime.Now;
                    model.InwardItemsFromSupplierDetails.ProportionateTax = Convert.ToDouble(form[proptax]);
                    model.InwardItemsFromSupplierDetails.ProportionateTaxAmt = Convert.ToDouble(form[proptaxamt]);
                    model.InwardItemsFromSupplierDetails.PurchaseReturn = "No";
                    if (model.InwardItemsFromSupplierDetails.ItemType == "Inventory")
                    {
                        var itemdetails = _ItemService.GetDescriptionByItemCode(model.InwardItemsFromSupplierDetails.itemCode);
                        if (itemdetails.Barcode != null)
                        {
                            model.InwardItemsFromSupplierDetails.Barcode = itemdetails.Barcode;
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                            string barcodeNo = string.Empty;
                            var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                            int serialNo;
                            /*To Check if it's the first barcode*/
                            if (Lastbarcode == null)
                            {
                                serialNo = 1111111;
                            }
                            else
                            {
                                int position = (Lastbarcode.Number.IndexOf(".") - 2);
                                serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                serialNo += 1;
                            }
                            /*Creation Of Barcode*/
                            barcodeNo = model.InwardItemsFromSupplierDetails.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + model.InwardItemsFromSupplierDetails.Item.Substring(model.InwardItemsFromSupplierDetails.Item.Length - 1).ToUpper();
                            if (System.IO.File.Exists(Server.MapPath("../Barcode.txt")))
                            {
                                System.IO.File.Delete(Server.MapPath("../BarCode.txt"));
                            }
                            System.IO.File.WriteAllText(Server.MapPath("../BarCode.txt"), barcodeNo);
                            Process.Start(Server.MapPath("../BarCodeGenerate.exe"));

                            /*Saving the Barcode Number*/
                            BarcodeNumber BarcodeNumber = new BarcodeNumber();
                            BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                            _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                            model.InwardItemsFromSupplierDetails.Barcode = barcodeNo + ".png";
                            var itemdata = _ItemService.GetDescriptionByItemCode(model.InwardItemsFromSupplierDetails.itemCode);
                            itemdata.Barcode = model.InwardItemsFromSupplierDetails.Barcode;
                            _ItemService.updateItem(itemdata);

                            OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                            OpeningStockMaster.OpeningStockCode = openingstockcode;
                            OpeningStockMaster.itemCode = model.InwardItemsFromSupplierDetails.itemCode;
                            OpeningStockMaster.ItemName = model.InwardItemsFromSupplierDetails.Item;
                            OpeningStockMaster.Barcode = model.InwardItemsFromSupplierDetails.Barcode;
                            OpeningStockMaster.Brand = model.InwardItemsFromSupplierDetails.Brand;
                            OpeningStockMaster.Category = model.InwardItemsFromSupplierDetails.Category;
                            OpeningStockMaster.SubCategory = model.InwardItemsFromSupplierDetails.SubCategory;
                            OpeningStockMaster.Color = model.InwardItemsFromSupplierDetails.Color;
                            OpeningStockMaster.Description = model.InwardItemsFromSupplierDetails.Description;
                            OpeningStockMaster.DesignCode = model.InwardItemsFromSupplierDetails.Design;
                            OpeningStockMaster.DesignName = model.InwardItemsFromSupplierDetails.DesignName;
                            OpeningStockMaster.Material = model.InwardItemsFromSupplierDetails.Material;
                            OpeningStockMaster.Unit = model.InwardItemsFromSupplierDetails.Unit;
                            OpeningStockMaster.Size = model.InwardItemsFromSupplierDetails.Size;
                            OpeningStockMaster.NumberType = model.InwardItemsFromSupplierDetails.NumberType;
                            OpeningStockMaster.ItemQuantity = 0;
                            OpeningStockMaster.TotalQuantity = 0;
                            OpeningStockMaster.CostPrice = model.InwardItemsFromSupplierDetails.CostPrice;
                            OpeningStockMaster.SellingPrice = model.InwardItemsFromSupplierDetails.SellingPrice;
                            OpeningStockMaster.MRP = model.InwardItemsFromSupplierDetails.MRP;
                            OpeningStockMaster.Status = "Active";
                            OpeningStockMaster.ModifiedOn = DateTime.Now;
                            _OpeningStockService.CreateStock(OpeningStockMaster);
                        }
                    }
                    _InwardItemFromSupplierService.CreateInwardItems(model.InwardItemsFromSupplierDetails);
                }
            }

            var inwardtaxlist = Session["SupplierInwardTaxList"] as IEnumerable<PurchaseInventoryTax>;
            if (inwardtaxlist != null)
            {
                foreach (var deletetax in inwardtaxlist)
                {
                    var delete = _PurchaseInventoryTaxService.GetTaxById(deletetax.Id);
                    _PurchaseInventoryTaxService.Delete(delete);
                }
            }

            int itemtaxcount = Convert.ToInt32(form["ItemTaxCount"]);
            model.PurchaseInventoryTaxDetails = new PurchaseInventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(form[taxamount]) != 0)
                {
                    model.PurchaseInventoryTaxDetails.Code = model.InwardFromSupplierDetails.InwardNo;
                    model.PurchaseInventoryTaxDetails.Amount = form[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = form[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = form[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }

            TempData["PreviousSupplierInward"] = model.InwardFromSupplierDetails.InwardNo;
            string InwardId = Encode(model.InwardFromSupplierDetails.InwardId.ToString());
            return RedirectToAction("InwardFromSupplierDetails/" + InwardId, "Inward");
        }

        [HttpGet]
        public ActionResult InwardFromSupplierPrint()
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
        public JsonResult GetAllInwardFromSupplierList(string term)
        {
            MainApplication model = new MainApplication();
            List<string> names = new List<string>();
            var data = _InwardFromSupplierService.GetAllSupplierInwards(term);
            foreach (var item in data)
            {
                names.Add(item.InwardNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardFromSupplierDetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetDetailsByPINo(InwardNo);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.InwardFromSupplierDetails.InwardNo);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(InwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult NewItem()
        {
            DatabaseName = CompanyName + " " + FinancialYear;
            MainApplication model = new MainApplication();
            return View(model);
        }

        [HttpPost]
        public ActionResult NewItem(MainApplication model, FormCollection frm)
        {
            string msg = frm["GetItemListFrom"];
            List<PurchaseItemDetail> PurchaseItemList = new List<PurchaseItemDetail>();
            List<InwardItemsFromSupplier> PerformaItemList = new List<InwardItemsFromSupplier>();
            if (msg == "From PO")
            {
                PurchaseItemList = TempData["PurchaseItemList"] as List<PurchaseItemDetail>;
            }
            else
            {
                PerformaItemList = TempData["PerformaItemList"] as List<InwardItemsFromSupplier>;
            }

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

            List<string> ItemCodeList = new List<string>();
            int rows = Convert.ToInt32(frm["hdnRowCount"]);
            int mainrows = Convert.ToInt32(frm["MainPagehdnRowCount"]);
            for (int i = 1; i <= rows; i++)
            {
                string item = "ItemType" + i;
                string typeOfMaterial = "materialType" + i;
                string colorCode = "colorCodeData" + i;
                string designCode = "designCodeData" + i;
                string designname = "designName" + i;
                string description = "itemDescription" + i;
                string size = "Size" + i;
                string costprice = "CostPrice" + i;
                string sellingprice = "SellingPrice" + i;
                string mrp = "MRP" + i;
                string brandname = "Brand" + i;
                string unit = "Unit" + i;
                string numbertype = "NumberType" + i;
                string discount = "Discount" + i;
                string quantity = "Quantity" + i;
                var CommissionInPercent = "CommissionInPercent" + i;
                var CommissionInRupees = "CommissionInRupees" + i;
                var amount = "AmountValue" + i;

                if (frm[item] == "Inventory")
                {
                    //CREATE ITEM CODE
                    if (frm[item] != null)
                    {
                        Item NewItem = new Item();
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
                        ItemCodeList.Add(itemcode);
                        NewItem.itemCategory = frm["ItemDetails.itemCategory"];
                        NewItem.itemSubCategory = frm["ItemDetails.itemSubCategory"];
                        NewItem.itemCode = itemcode;
                        NewItem.itemName = frm["ItemDetails.itemName"];
                        NewItem.itemtype = frm[item];
                        NewItem.description = frm[description];
                        NewItem.typeOfMaterial = frm[typeOfMaterial];
                        NewItem.colorCode = frm[colorCode];
                        NewItem.designCode = frm[designCode];
                        NewItem.designName = frm[designname];
                        NewItem.brandName = frm[brandname];
                        NewItem.unit = frm[unit];
                        NewItem.NumberType = frm[numbertype];
                        NewItem.size = frm[size];
                        NewItem.costprice = frm[costprice];
                        NewItem.sellingprice = frm[sellingprice];
                        NewItem.mrp = frm[mrp];
                        NewItem.status = "Active";
                        NewItem.modifedOn = DateTime.Now;
                        _ItemService.createItem(NewItem);
                        var insertedrow = _ItemService.GetLastItem();
                        if (msg == "From PO")
                        {
                            PurchaseItemDetail POitemdetail = new PurchaseItemDetail();
                            POitemdetail.PoNo = frm["PoNO"];
                            POitemdetail.Category = insertedrow.itemCategory;
                            POitemdetail.SubCategory = insertedrow.itemSubCategory;
                            POitemdetail.itemCode = insertedrow.itemCode;
                            POitemdetail.Item = insertedrow.itemName;
                            POitemdetail.ItemType = insertedrow.itemtype;
                            POitemdetail.Description = insertedrow.description;
                            POitemdetail.Material = insertedrow.typeOfMaterial;
                            POitemdetail.Color = insertedrow.colorCode;
                            POitemdetail.Design = insertedrow.designCode;
                            POitemdetail.DesignName = insertedrow.designName;
                            POitemdetail.Brand = insertedrow.brandName;
                            POitemdetail.Size = insertedrow.size;
                            if (frm[quantity] != "")
                            {
                                POitemdetail.Quantity = Convert.ToDouble(frm[quantity]);
                            }
                            else
                            {
                                POitemdetail.Quantity = 0;
                            }
                            if (frm[discount] != "")
                            {
                                POitemdetail.DiscountPercent = Convert.ToDouble(frm[discount]);
                            }
                            else
                            {
                                POitemdetail.DiscountPercent = 0;
                            }
                            if (insertedrow.sellingprice != "")
                            {
                                POitemdetail.SellingPrice = Convert.ToDouble(insertedrow.sellingprice);
                            }
                            else
                            {
                                POitemdetail.SellingPrice = 0;
                            }
                            if (insertedrow.mrp != "")
                            {
                                POitemdetail.MRP = Convert.ToDouble(insertedrow.mrp);
                            }
                            else
                            {
                                POitemdetail.SellingPrice = 0;
                            }
                            if (insertedrow.costprice != "")
                            {
                                POitemdetail.CostPrice = Convert.ToDouble(insertedrow.costprice);
                            }
                            else
                            {
                                POitemdetail.CostPrice = 0;
                            }
                            POitemdetail.Unit = insertedrow.unit;
                            POitemdetail.NumberType = insertedrow.NumberType;
                            if (frm[amount] != "")
                            {
                                POitemdetail.Amount = Convert.ToDouble(frm[amount]);
                            }
                            else
                            {
                                POitemdetail.Amount = 0;
                            }
                            PurchaseItemList.Add(POitemdetail);
                        }
                        else
                        {
                            InwardItemsFromSupplier PerformaItem = new InwardItemsFromSupplier();
                            PerformaItem.PoNo = frm["PoNO"];
                            PerformaItem.Category = insertedrow.itemCategory;
                            PerformaItem.SubCategory = insertedrow.itemSubCategory;
                            PerformaItem.itemCode = insertedrow.itemCode;
                            PerformaItem.Item = insertedrow.itemName;
                            PerformaItem.ItemType = insertedrow.itemtype;
                            PerformaItem.Description = insertedrow.description;
                            PerformaItem.Material = insertedrow.typeOfMaterial;
                            PerformaItem.Color = insertedrow.colorCode;
                            PerformaItem.Design = insertedrow.designCode;
                            PerformaItem.DesignName = insertedrow.designName;
                            PerformaItem.Brand = insertedrow.brandName;
                            PerformaItem.Size = insertedrow.size;
                            PerformaItem.Unit = insertedrow.unit;
                            PerformaItem.NumberType = insertedrow.NumberType;
                            if (insertedrow.sellingprice != "")
                            {
                                PerformaItem.SellingPrice = Convert.ToDouble(insertedrow.sellingprice);
                            }
                            else
                            {
                                PerformaItem.SellingPrice = 0;
                            }
                            if (insertedrow.mrp != "")
                            {
                                PerformaItem.MRP = Convert.ToDouble(insertedrow.mrp);
                            }
                            else
                            {
                                PerformaItem.MRP = 0;
                            }
                            if (insertedrow.costprice != "")
                            {
                                PerformaItem.CostPrice = Convert.ToDouble(insertedrow.costprice);
                            }
                            else
                            {
                                PerformaItem.CostPrice = 0;
                            }
                            PerformaItem.OrderedQuantity = 0;
                            PerformaItem.Balance = 0;
                            if (frm[quantity] != "")
                            {
                                PerformaItem.ReceivedQuantity = Convert.ToDouble(frm[quantity]);
                            }
                            else
                            {
                                PerformaItem.ReceivedQuantity = 0;

                            }
                            if (frm[discount] != "")
                            {
                                PerformaItem.Discount = Convert.ToDouble(frm[discount]);
                            }
                            else
                            {
                                PerformaItem.Discount = 0;
                            }
                            if (frm[amount] != "")
                            {
                                PerformaItem.Amount = Convert.ToDouble(frm[amount]);
                            }
                            else
                            {
                                PerformaItem.Amount = 0;
                            }
                            PerformaItemList.Add(PerformaItem);
                        }
                    }
                }
                else
                {
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

                    if (!string.IsNullOrEmpty(item))
                    {
                        ItemCodeList.Add(noninvitemcode);
                        NonInventoryItem noninvitem = new NonInventoryItem();
                        noninvitem.description = frm[description];
                        noninvitem.modifedOn = DateTime.Now;
                        noninvitem.status = "Active";
                        noninvitem.itemCategory = frm["ItemDetails.itemCategory"];
                        noninvitem.itemSubCategory = frm["ItemDetails.itemSubCategory"];
                        noninvitem.itemName = frm["ItemDetails.itemName"];
                        noninvitem.costprice = frm[costprice];
                        noninvitem.typeOfMaterial = frm[typeOfMaterial];
                        noninvitem.colorCode = frm[colorCode];
                        noninvitem.designCode = frm[designCode];
                        noninvitem.designName = frm[designname];
                        noninvitem.size = frm[size];
                        noninvitem.unit = frm[unit];
                        noninvitem.NumberType = frm[numbertype];
                        noninvitem.itemtype = frm[item];
                        noninvitem.sellingprice = frm[sellingprice];
                        noninvitem.brandName = frm[brandname];
                        noninvitem.mrp = frm[mrp];
                        noninvitem.itemCode = noninvitemcode;
                        _NonInventoryItemService.createNonInventoryItem(noninvitem);
                        var insertedrow = _NonInventoryItemService.GetLastItem();
                        if (msg == "From PO")
                        {
                            PurchaseItemDetail POitemdetail = new PurchaseItemDetail();
                            POitemdetail.PoNo = frm["PoNO"];
                            POitemdetail.Category = insertedrow.itemCategory;
                            POitemdetail.SubCategory = insertedrow.itemSubCategory;
                            POitemdetail.itemCode = insertedrow.itemCode;
                            POitemdetail.Item = insertedrow.itemName;
                            POitemdetail.ItemType = insertedrow.itemtype;
                            POitemdetail.Description = insertedrow.description;
                            POitemdetail.Material = insertedrow.typeOfMaterial;
                            POitemdetail.Color = insertedrow.colorCode;
                            POitemdetail.Design = insertedrow.designCode;
                            POitemdetail.DesignName = insertedrow.designName;
                            POitemdetail.Brand = insertedrow.brandName;
                            POitemdetail.Size = insertedrow.size;
                            if (frm[quantity] != "")
                            {
                                POitemdetail.Quantity = Convert.ToDouble(frm[quantity]);
                            }
                            else
                            {
                                POitemdetail.Quantity = 0;
                            }
                            if (frm[discount] != "")
                            {
                                POitemdetail.DiscountPercent = Convert.ToDouble(frm[discount]);
                            }
                            else
                            {
                                POitemdetail.DiscountPercent = 0;
                            }
                            if (insertedrow.sellingprice != "")
                            {
                                POitemdetail.SellingPrice = Convert.ToDouble(insertedrow.sellingprice);
                            }
                            else
                            {
                                POitemdetail.SellingPrice = 0;
                            }
                            if (insertedrow.mrp != "")
                            {
                                POitemdetail.MRP = Convert.ToDouble(insertedrow.mrp);
                            }
                            else
                            {
                                POitemdetail.SellingPrice = 0;
                            }
                            if (insertedrow.costprice != "")
                            {
                                POitemdetail.CostPrice = Convert.ToDouble(insertedrow.costprice);
                            }
                            else
                            {
                                POitemdetail.CostPrice = 0;
                            }
                            POitemdetail.Unit = insertedrow.unit;
                            POitemdetail.NumberType = insertedrow.NumberType;
                            if (frm[amount] != "")
                            {
                                POitemdetail.Amount = Convert.ToDouble(frm[amount]);
                            }
                            else
                            {
                                POitemdetail.Amount = 0;
                            }
                            PurchaseItemList.Add(POitemdetail);
                        }
                        else
                        {
                            InwardItemsFromSupplier PerformaItem = new InwardItemsFromSupplier();
                            PerformaItem.PoNo = frm["PoNO"];
                            PerformaItem.Category = insertedrow.itemCategory;
                            PerformaItem.SubCategory = insertedrow.itemSubCategory;
                            PerformaItem.itemCode = insertedrow.itemCode;
                            PerformaItem.Item = insertedrow.itemName;
                            PerformaItem.ItemType = insertedrow.itemtype;
                            PerformaItem.Description = insertedrow.description;
                            PerformaItem.Material = insertedrow.typeOfMaterial;
                            PerformaItem.Color = insertedrow.colorCode;
                            PerformaItem.Design = insertedrow.designCode;
                            PerformaItem.DesignName = insertedrow.designName;
                            PerformaItem.Brand = insertedrow.brandName;
                            PerformaItem.Size = insertedrow.size;
                            PerformaItem.Unit = insertedrow.unit;
                            PerformaItem.NumberType = insertedrow.NumberType;
                            if (insertedrow.sellingprice != "")
                            {
                                PerformaItem.SellingPrice = Convert.ToDouble(insertedrow.sellingprice);
                            }
                            else
                            {
                                PerformaItem.SellingPrice = 0;
                            }
                            if (insertedrow.mrp != "")
                            {
                                PerformaItem.MRP = Convert.ToDouble(insertedrow.mrp);
                            }
                            else
                            {
                                PerformaItem.MRP = 0;
                            }
                            if (insertedrow.costprice != "")
                            {
                                PerformaItem.CostPrice = Convert.ToDouble(insertedrow.costprice);
                            }
                            else
                            {
                                PerformaItem.CostPrice = 0;
                            }
                            PerformaItem.OrderedQuantity = 0;
                            PerformaItem.Balance = 0;
                            if (frm[quantity] != "")
                            {
                                PerformaItem.ReceivedQuantity = Convert.ToDouble(frm[quantity]);
                            }
                            else
                            {
                                PerformaItem.ReceivedQuantity = 0;

                            }
                            if (frm[discount] != "")
                            {
                                PerformaItem.Discount = Convert.ToDouble(frm[discount]);
                            }
                            else
                            {
                                PerformaItem.Discount = 0;
                            }
                            if (frm[amount] != "")
                            {
                                PerformaItem.Amount = Convert.ToDouble(frm[amount]);
                            }
                            else
                            {
                                PerformaItem.Amount = 0;
                            }
                            PerformaItemList.Add(PerformaItem);
                        }
                    }
                }
            }

            TempData["ItemCodeList"] = ItemCodeList;
            TempData["MainPageRowCount"] = mainrows - ItemCodeList.Count();
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

            if (msg == "From PO")
            {
                TempData["PurchaseItemList"] = PurchaseItemList;
            }
            else
            {
                TempData["PerformaItemList"] = PerformaItemList;
            }

            return RedirectToAction("NewItemDetails", "Inward", new { LastItemBefore = lastitembefore, LastItemAfter = lastitemafter, LastNonInventoryItemBefore = lastnoninventoryitembefore, LastNonInventoryItemAfter = lnonitemafter });
        }

        [HttpGet]
        public ActionResult NewItemDetails(int LastItemBefore, int LastItemAfter, int LastNonInventoryItemBefore, int LastNonInventoryItemAfter)
        {
            MainApplication model = new MainApplication();
            model.ItemDetails = _ItemService.GetItem(LastItemBefore);
            if (model.ItemDetails == null)
            {
                model.NonInventoryItemDetails = _NonInventoryItemService.GetItemById(LastNonInventoryItemBefore);
            }
            model.ItemList = _ItemService.GetInsertedRow(LastItemBefore, LastItemAfter);
            model.NonInventoryItemList = _NonInventoryItemService.GetInsertedRow(LastNonInventoryItemBefore, LastNonInventoryItemAfter);
            return View(model);
        }

        // ************************************** END INWARD FROM SUPPLIER ******************************************

        // ************************************** START INWARD WITHOUT PO ******************************************

        [HttpGet]
        public ActionResult InwardWithoutPO()
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierDetails = new InwardFromSupplier();
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
                var LastInward = _InwardFromSupplierService.GetLastInwardWithoutPOByFinYr(FinYr, godownshopcode);
                string InwardCode = string.Empty;
                int InwardNoLength = 0;
                int InwardNo = 0;
                if (LastInward == null)
                {
                    InwardNoLength = 1;
                    InwardNo = 1;
                }
                else
                {
                    int indexinwardwithoutpo = LastInward.InwardNo.LastIndexOf('I');
                    InwardCode = LastInward.InwardNo.Substring(indexinwardwithoutpo + 4, 6);
                    InwardNoLength = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                    InwardNo = Convert.ToInt32(InwardCode) + 1;
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
                InwardCode = _utilityService.getName(ShortCode + "/IWOP", InwardNoLength, InwardNo);
                InwardCode = InwardCode + FinYr;
                TempData["PreviousInward"] = InwardCode;
                model.PerformaCode = InwardCode;
            }
            model.ItemSubCategoryList = _itemsubcategoryservice.GetItemSubCategories();
            model.UnitList = _unitmasterservice.getallsize();
            model.ShopList = _ShopService.GetAll();
            return View(model);
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
            string InwardCode = string.Empty;
            if (EmpDesig == "PurchaseManager")
            {
                godownshopcode = code;
                godownshopname = godownname;
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                var LastInward = _InwardFromSupplierService.GetLastInwardWithoutPOByFinYr(FinYr, godownshopcode);
                int InwardNoLength = 0;
                int InwardNo = 0;
                if (LastInward == null)
                {
                    InwardNoLength = 1;
                    InwardNo = 1;
                }
                else
                {
                    int indexinwardwithoutpo = LastInward.InwardNo.LastIndexOf('I');
                    InwardCode = LastInward.InwardNo.Substring(indexinwardwithoutpo + 4, 6);
                    InwardNoLength = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                    InwardNo = Convert.ToInt32(InwardCode) + 1;
                }
                string ShortCode = string.Empty;
                ShortCode = _godownMasterService.GetGodownDetailsByName(godownshopname).ShortCode;
                InwardCode = _utilityService.getName(ShortCode + "/IWOP", InwardNoLength, InwardNo);
                InwardCode = InwardCode + FinYr;
                TempData["PreviousInward"] = InwardCode;
            }
            return Json(new { godownname, addresslist, InwardCode }, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetShopDetailsByName(string Code)
        {
            var details = _ShopService.GetShopByCode(Code);
            string EmpDesig = _EmployeeMasterService.getEmpByEmail(UserEmail).Designation;
            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            string InwardCode = string.Empty;
            if (EmpDesig == "PurchaseManager")
            {
                godownshopcode = Code;
                godownshopname = details.ShopName;
                string year = FinancialYear;
                string[] yr = year.Split(' ', '-');
                string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
                var LastInward = _InwardFromSupplierService.GetLastInwardWithoutPOByFinYr(FinYr, godownshopcode);
                int InwardNoLength = 0;
                int InwardNo = 0;
                if (LastInward == null)
                {
                    InwardNoLength = 1;
                    InwardNo = 1;
                }
                else
                {
                    int indexinwardwithoutpo = LastInward.InwardNo.LastIndexOf('I');
                    InwardCode = LastInward.InwardNo.Substring(indexinwardwithoutpo + 4, 6);
                    InwardNoLength = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                    InwardNo = Convert.ToInt32(InwardCode) + 1;
                }
                string ShortCode = string.Empty;
                ShortCode = _ShopService.GetShopDetailsByName(godownshopname).ShortCode;
                InwardCode = _utilityService.getName(ShortCode + "/IWOP", InwardNoLength, InwardNo);
                InwardCode = InwardCode + FinYr;
                TempData["PreviousInward"] = InwardCode;
            }
            return Json(new { InwardCode, details.ShopContactNo, details.ShopEmail, details.EmpName, details.ShopName, details.ShopAddress }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InwardWithoutPO(MainApplication model, FormCollection frmcoll)
        {
            MainApplication mainapp = new MainApplication()
            {
                InwardItemsFromSupplierDetails = new InwardItemsFromSupplier()
            };
            string godownshopcode = string.Empty;
            string godownshopname = string.Empty;
            string EmpDesig = _EmployeeMasterService.getEmpByEmail(UserEmail).Designation;
            if (EmpDesig == "PurchaseManager")
            {
                if (model.InwardFromSupplierDetails.GodownCode != null)
                {
                    godownshopcode = model.InwardFromSupplierDetails.GodownCode;
                    godownshopname = model.InwardFromSupplierDetails.GodownName;
                }
                else
                {
                    godownshopcode = model.InwardFromSupplierDetails.ShopCode;
                    godownshopname = model.InwardFromSupplierDetails.ShopName;
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
            var LastInward = _InwardFromSupplierService.GetLastInwardWithoutPOByFinYr(FinYr, godownshopcode);
            string InwardCode = string.Empty;
            int InwardNoLength = 0;
            int InwardNo = 0;
            if (LastInward == null)
            {
                InwardNoLength = 1;
                InwardNo = 1;
            }
            else
            {
                int indexlastinwardcode = LastInward.InwardNo.LastIndexOf('I');
                InwardCode = LastInward.InwardNo.Substring(indexlastinwardcode + 4, 6);
                InwardNoLength = (Convert.ToInt32(InwardCode) + 1).ToString().Length;
                InwardNo = Convert.ToInt32(InwardCode) + 1;
            }

            string openingstockcode = string.Empty;
            var openingstockdata = _OpeningStockService.GetLastStockByFinYr(FinYr);
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
            openingstockcode = _utilityService.getName("OST", length, openstockVal);
            openingstockcode = openingstockcode + FinYr;

            string ShortCode = string.Empty;
            if (godownshopcode.Contains("SH"))
            {
                ShortCode = _ShopService.GetShopDetailsByName(godownshopname).ShortCode;
            }
            else
            {
                ShortCode = _godownMasterService.GetGodownDetailsByName(godownshopname).ShortCode;
            }
            InwardCode = _utilityService.getName(ShortCode + "/IWOP", InwardNoLength, InwardNo);
            InwardCode = InwardCode + FinYr;

            string dynamicrow = frmcoll["hdnRowCount"].ToString();

            int count = Convert.ToInt32(dynamicrow);
            for (int i = 1; i <= count; i++)
            {
                string barcode = "barcode" + i;
                string itemCode = "itemCode" + i;
                string subcat = "SubCat" + i;
                string itemName = "itemName" + i;
                string itemtype = "ItemType" + i;
                string description = "description" + i;
                string quantity = "quantity" + i;
                string rate = "rate" + i;
                string mrp = "MRP" + i;
                string sellingprice = "sellingprice" + i;
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
                string proptax = "proportionatetaxvalue" + i;
                string proptaxamt = "proportionatetaxamount" + i;
                string universalbarcode = "ubarcode" + i;

                if (frmcoll[quantity] != "" && frmcoll[itemCode] != null)
                {
                    mainapp.InwardItemsFromSupplierDetails = new InwardItemsFromSupplier();
                    if (frmcoll[barcode] != "")
                    {
                        mainapp.InwardItemsFromSupplierDetails.Barcode = frmcoll[barcode];
                    }
                    else
                    {
                        mainapp.InwardItemsFromSupplierDetails.Barcode = null;
                    }
                    mainapp.InwardItemsFromSupplierDetails.UniversalBarcode = frmcoll[universalbarcode];
                    mainapp.InwardItemsFromSupplierDetails.itemCode = frmcoll[itemCode];
                    mainapp.InwardItemsFromSupplierDetails.Item = frmcoll[itemName];
                    mainapp.InwardItemsFromSupplierDetails.Description = frmcoll[description];
                    mainapp.InwardItemsFromSupplierDetails.OrderedQuantity = Convert.ToDouble(frmcoll[quantity]);
                    mainapp.InwardItemsFromSupplierDetails.ReceivedQuantity = Convert.ToDouble(frmcoll[quantity]);
                    mainapp.InwardItemsFromSupplierDetails.ExtraQty = 0;
                    mainapp.InwardItemsFromSupplierDetails.Balance = 0;
                    mainapp.InwardItemsFromSupplierDetails.ItemType = frmcoll[itemtype];
                    mainapp.InwardItemsFromSupplierDetails.Unit = frmcoll[unit];
                    mainapp.InwardItemsFromSupplierDetails.NumberType = frmcoll[numbertype];
                    mainapp.InwardItemsFromSupplierDetails.CostPrice = Convert.ToDouble(frmcoll[rate]);
                    if (frmcoll[mrp] != "")
                    {
                        mainapp.InwardItemsFromSupplierDetails.MRP = Convert.ToDouble(frmcoll[mrp]);
                    }
                    else
                    {
                        mainapp.InwardItemsFromSupplierDetails.MRP = 0;
                    }

                    if (frmcoll[sellingprice] != "")
                    {
                        mainapp.InwardItemsFromSupplierDetails.SellingPrice = Convert.ToDouble(frmcoll[sellingprice]);
                    }
                    else
                    {
                        mainapp.InwardItemsFromSupplierDetails.SellingPrice = 0;
                    }
                    mainapp.InwardItemsFromSupplierDetails.Amount = Convert.ToDouble(frmcoll[amountvalue]);
                    if (frmcoll[discountpercent] != "")
                    {
                        mainapp.InwardItemsFromSupplierDetails.Discount = Convert.ToDouble(frmcoll[discountpercent]);
                    }
                    else
                    {
                        mainapp.InwardItemsFromSupplierDetails.Discount = 0;
                    }
                    if (frmcoll[discountrps] != "")
                    {
                        mainapp.InwardItemsFromSupplierDetails.DiscountRPS = Convert.ToDouble(frmcoll[discountrps]);
                    }
                    else
                    {
                        mainapp.InwardItemsFromSupplierDetails.DiscountRPS = 0;
                    }
                    mainapp.InwardItemsFromSupplierDetails.ExtraQty = 0;
                    mainapp.InwardItemsFromSupplierDetails.FreeQty = 0;
                    mainapp.InwardItemsFromSupplierDetails.Material = frmcoll[materialvalue];
                    mainapp.InwardItemsFromSupplierDetails.Color = frmcoll[colorvalue];
                    mainapp.InwardItemsFromSupplierDetails.Brand = frmcoll[brandvalue];
                    mainapp.InwardItemsFromSupplierDetails.Size = frmcoll[sizevalue];
                    mainapp.InwardItemsFromSupplierDetails.Design = frmcoll[design];
                    mainapp.InwardItemsFromSupplierDetails.DesignName = frmcoll[designName];
                    mainapp.InwardItemsFromSupplierDetails.Status = "Active";
                    mainapp.InwardItemsFromSupplierDetails.ModifiedOn = DateTime.Now;
                    mainapp.InwardItemsFromSupplierDetails.PoNo = "NoPO";
                    mainapp.InwardItemsFromSupplierDetails.InwardNo = InwardCode;
                    mainapp.InwardItemsFromSupplierDetails.Category = _itemsubcategoryservice.GetCategoryBySubCat(frmcoll[subcat]);
                    mainapp.InwardItemsFromSupplierDetails.SubCategory = frmcoll[subcat];
                    mainapp.InwardItemsFromSupplierDetails.ItemTax = frmcoll[itemtax];
                    mainapp.InwardItemsFromSupplierDetails.ProportionateTax = Convert.ToDouble(frmcoll[proptax]);
                    mainapp.InwardItemsFromSupplierDetails.ProportionateTaxAmt = Convert.ToDouble(frmcoll[proptaxamt]);
                    if (mainapp.InwardItemsFromSupplierDetails.ItemType == "Inventory")
                    {
                        var itemdetails = _ItemService.GetDescriptionByItemCode(mainapp.InwardItemsFromSupplierDetails.itemCode);
                        if (itemdetails.Barcode != null)
                        {
                            mainapp.InwardItemsFromSupplierDetails.Barcode = itemdetails.Barcode;
                        }
                        else
                        {
                            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                            string barcodeNo = string.Empty;
                            var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                            int serialNo;
                            /*To Check if it's the first barcode*/
                            if (Lastbarcode == null)
                            {
                                serialNo = 1111111;
                            }
                            else
                            {
                                int position = (Lastbarcode.Number.IndexOf(".") - 2);
                                serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                serialNo += 1;
                            }
                            /*Creation Of Barcode*/
                            barcodeNo = mainapp.InwardItemsFromSupplierDetails.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + mainapp.InwardItemsFromSupplierDetails.Item.Substring(mainapp.InwardItemsFromSupplierDetails.Item.Length - 1).ToUpper();
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
                            BarcodeNumber BarcodeNumber = new BarcodeNumber();
                            BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                            BarcodeNumber.BarcodeImage = bytes;
                            BarcodeNumber.ModifiedDate = DateTime.Now;
                            _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                            fs.Close();
                            br.Dispose();

                            FileInfo file = new FileInfo(filepath);
                            if (file.Exists)
                            {
                                file.Delete();
                            }                            

                            var itemdata = _ItemService.GetDescriptionByItemCode(mainapp.InwardItemsFromSupplierDetails.itemCode);
                            itemdata.Barcode = BarcodeNumber.Number;
                            mainapp.InwardItemsFromSupplierDetails.Barcode = BarcodeNumber.Number;
                            _ItemService.updateItem(itemdata);

                            OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                            OpeningStockMaster.OpeningStockCode = openingstockcode;
                            OpeningStockMaster.itemCode = mainapp.InwardItemsFromSupplierDetails.itemCode;
                            OpeningStockMaster.ItemName = mainapp.InwardItemsFromSupplierDetails.Item;
                            OpeningStockMaster.Barcode = mainapp.InwardItemsFromSupplierDetails.Barcode;
                            OpeningStockMaster.Brand = mainapp.InwardItemsFromSupplierDetails.Brand;
                            OpeningStockMaster.Category = mainapp.InwardItemsFromSupplierDetails.Category;
                            OpeningStockMaster.SubCategory = mainapp.InwardItemsFromSupplierDetails.SubCategory;
                            OpeningStockMaster.Color = mainapp.InwardItemsFromSupplierDetails.Color;
                            OpeningStockMaster.Description = mainapp.InwardItemsFromSupplierDetails.Description;
                            OpeningStockMaster.DesignCode = mainapp.InwardItemsFromSupplierDetails.Design;
                            OpeningStockMaster.DesignName = mainapp.InwardItemsFromSupplierDetails.DesignName;
                            OpeningStockMaster.Material = mainapp.InwardItemsFromSupplierDetails.Material;
                            OpeningStockMaster.Unit = mainapp.InwardItemsFromSupplierDetails.Unit;
                            OpeningStockMaster.Size = mainapp.InwardItemsFromSupplierDetails.Size;
                            OpeningStockMaster.NumberType = mainapp.InwardItemsFromSupplierDetails.NumberType;
                            OpeningStockMaster.ItemQuantity = 0;
                            OpeningStockMaster.TotalQuantity = 0;
                            OpeningStockMaster.CostPrice = mainapp.InwardItemsFromSupplierDetails.CostPrice;
                            OpeningStockMaster.SellingPrice = mainapp.InwardItemsFromSupplierDetails.SellingPrice;
                            OpeningStockMaster.MRP = mainapp.InwardItemsFromSupplierDetails.MRP;
                            OpeningStockMaster.Status = "Active";
                            OpeningStockMaster.ModifiedOn = DateTime.Now;
                            _OpeningStockService.CreateStock(OpeningStockMaster);
                        }
                    }
                    mainapp.InwardItemsFromSupplierDetails.PurchaseReturn = "No";
                    _InwardItemFromSupplierService.CreateInwardItems(mainapp.InwardItemsFromSupplierDetails);
                }
            }

            model.InwardFromSupplierDetails.InwardNo = InwardCode;
            model.InwardFromSupplierDetails.PoNo = "NoPO";
            model.InwardFromSupplierDetails.DelDate = DateTime.Now;
            model.InwardFromSupplierDetails.InwardDate = DateTime.Now;
            if (model.InwardFromSupplierDetails.GodownCode == null)
            {
                model.InwardFromSupplierDetails.GodownArea = null;
            }
            model.InwardFromSupplierDetails.ReceivingDate = DateTime.Now;
            model.InwardFromSupplierDetails.TotalAmount = Convert.ToDouble(frmcoll["Total_Amount_Value"]);
            model.InwardFromSupplierDetails.TotalTaxAmount = Convert.ToDouble(frmcoll["TotalTaxAmountValue"]);
            model.InwardFromSupplierDetails.GrandTotal = Convert.ToDouble(frmcoll["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.PaymentAmount = 0;
            model.InwardFromSupplierDetails.PaymentBalance = Convert.ToDouble(frmcoll["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.TotalExtraQuantity = "0";
            model.InwardFromSupplierDetails.TotalRemainingQuantity = "0";
            model.InwardFromSupplierDetails.Status = "Active";
            model.InwardFromSupplierDetails.PaymentStatus = "Active";
            model.InwardFromSupplierDetails.Editable = "Yes";
            model.InwardFromSupplierDetails.PurchaseReturn = "No";
            model.InwardFromSupplierDetails.ModifiedOn = DateTime.Now;
            model.InwardFromSupplierDetails.DebitNotesAmount = 0;
            _InwardFromSupplierService.CreateInward(model.InwardFromSupplierDetails);

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
                    model.PurchaseInventoryTaxDetails.Code = InwardCode;
                    model.PurchaseInventoryTaxDetails.Amount = frmcoll[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = frmcoll[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = frmcoll[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }
            var details = _InwardFromSupplierService.GetInwardByInwardNo(InwardCode);
            string InwardId = Encode(details.InwardId.ToString());
            return RedirectToAction("InwardWithoutPODetails/" + InwardId, "Inward");
        }

        [HttpGet]
        public ActionResult PopUpPage(string code)
        {
            var data = _ItemService.GetDesignNameByDesignCode(code);
            return View(data);
        }

        [HttpGet]
        public ActionResult InwardWithoutPODetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromSupplierDetails = new InwardFromSupplier(),
            };
            int Id = Decode(id);
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetById(Id);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.InwardFromSupplierDetails.InwardNo);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(model.InwardFromSupplierDetails.InwardNo);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string previousinward = TempData["PreviousInward"].ToString();
            if (TempData["PreviousInward"].ToString() != model.InwardFromSupplierDetails.InwardNo)
            {
                ViewData["InwardChanged"] = previousinward + " is replaced with " + model.InwardFromSupplierDetails.InwardNo + " because " + previousinward + " is acquired by another person";
            }
            TempData["PreviousInward"] = previousinward;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardWithoutPO(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromSupplierDetails = new InwardFromSupplier(),
            };
            int Id = Decode(id);
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetById(Id);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(model.InwardFromSupplierDetails.InwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardWithoutPOEdit()
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
        public JsonResult InwardWOPOList(string term)
        {
            MainApplication model = new MainApplication();
            List<string> names = new List<string>();
            var data = _InwardFromSupplierService.GetDetailsForIWOPByPINo(term);
            foreach (var item in data)
            {
                names.Add(item.InwardNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InwardWithoutPOEditDetails(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetDetailsByPINo(InwardNo);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(InwardNo);
            TempData["IWOPOItemList"] = model.InwardItemsFromSupplierList;
            model.ItemSubCategoryList = _itemsubcategoryservice.GetItemSubCategories();
            model.GodownMasterList = _godownMasterService.GetGodownNames();
            model.ShopList = _ShopService.GetAll();
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(model.InwardFromSupplierDetails.InwardNo);
            if (model.PurchaseInventoryTaxList.Count() != 0)
            {
                Session["IWOPOTaxList"] = model.PurchaseInventoryTaxList;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardWithoutPOEditDetails(MainApplication model, FormCollection form)
        {
            MainApplication mainapp = new MainApplication()
            {
                InwardItemsFromSupplierDetails = new InwardItemsFromSupplier()
            };
            var inwarditemlist = TempData["IWOPOItemList"] as IEnumerable<InwardItemsFromSupplier>;
            foreach (var deleteitem in inwarditemlist)
            {
                var delete = _InwardItemFromSupplierService.GetItemDetailsByInwardId(deleteitem.InwardItemNo);
                _InwardItemFromSupplierService.DeleteInwardItems(delete);
            }

            var potaxlist = Session["IWOPOTaxList"] as IEnumerable<PurchaseInventoryTax>;
            if (potaxlist != null)
            {
                foreach (var deletetax in potaxlist)
                {
                    var delete = _PurchaseInventoryTaxService.GetTaxById(deletetax.Id);
                    _PurchaseInventoryTaxService.Delete(delete);
                }
            }

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            string openingstockcode = string.Empty;
            var openingstockdata = _OpeningStockService.GetLastStockByFinYr(FinYr);
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
            openingstockcode = _utilityService.getName("OST", length, openstockVal);
            openingstockcode = openingstockcode + FinYr;

            string dynamicrow = form["hdnRowCount"].ToString();
            if (!string.IsNullOrEmpty(dynamicrow))
            {
                int count = Convert.ToInt32(dynamicrow);
                for (int i = 1; i <= count; i++)
                {
                    string itemCode = "itemCode" + i;
                    string ItemType = "ItemType" + i;
                    string subcat = "SubCat" + i;
                    string itemName = "itemName" + i;
                    string barcode = "barcode" + i;
                    string description = "description" + i;
                    string quantity = "quantity" + i;
                    string rate = "rate" + i;
                    string mrp = "MRP" + i;
                    string sellingprice = "sellingprice" + i;
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
                    string proptax = "proportionatetaxvalue" + i;
                    string proptaxamt = "proportionatetaxamount" + i;
                    string ubarcode = "ubarcode" + i;

                    if (form[quantity] != "" && form[itemCode] != null)
                    {
                        mainapp.InwardItemsFromSupplierDetails.UniversalBarcode = form[ubarcode];
                        mainapp.InwardItemsFromSupplierDetails.Barcode = form[barcode];
                        mainapp.InwardItemsFromSupplierDetails.itemCode = form[itemCode];
                        mainapp.InwardItemsFromSupplierDetails.Item = form[itemName];
                        mainapp.InwardItemsFromSupplierDetails.Description = form[description];
                        mainapp.InwardItemsFromSupplierDetails.OrderedQuantity = Convert.ToDouble(form[quantity]);
                        mainapp.InwardItemsFromSupplierDetails.ReceivedQuantity = Convert.ToDouble(form[quantity]);
                        mainapp.InwardItemsFromSupplierDetails.ExtraQty = 0;
                        mainapp.InwardItemsFromSupplierDetails.FreeQty = 0;
                        mainapp.InwardItemsFromSupplierDetails.Balance = 0;
                        mainapp.InwardItemsFromSupplierDetails.ItemType = form[ItemType];
                        mainapp.InwardItemsFromSupplierDetails.Unit = form[unit];
                        mainapp.InwardItemsFromSupplierDetails.NumberType = form[numbertype];
                        mainapp.InwardItemsFromSupplierDetails.CostPrice = Convert.ToDouble(form[rate]);
                        if (form[mrp] != "")
                        {
                            mainapp.InwardItemsFromSupplierDetails.MRP = Convert.ToDouble(form[mrp]);
                        }
                        else
                        {
                            mainapp.InwardItemsFromSupplierDetails.MRP = 0;
                        }
                        if (form[sellingprice] != "")
                        {
                            mainapp.InwardItemsFromSupplierDetails.SellingPrice = Convert.ToDouble(form[sellingprice]);
                        }
                        else
                        {
                            mainapp.InwardItemsFromSupplierDetails.SellingPrice = 0;
                        }
                        mainapp.InwardItemsFromSupplierDetails.Amount = Convert.ToDouble(form[amountvalue]);
                        if (form[discountpercent] != "")
                        {
                            mainapp.InwardItemsFromSupplierDetails.Discount = Convert.ToDouble(form[discountpercent]);
                        }
                        else
                        {
                            mainapp.InwardItemsFromSupplierDetails.Discount = 0;
                        }
                        if (form[discountrps] != "")
                        {
                            mainapp.InwardItemsFromSupplierDetails.DiscountRPS = Convert.ToDouble(form[discountrps]);
                        }
                        else
                        {
                            mainapp.InwardItemsFromSupplierDetails.DiscountRPS = 0;
                        }
                        mainapp.InwardItemsFromSupplierDetails.Material = form[materialvalue];
                        mainapp.InwardItemsFromSupplierDetails.Color = form[colorvalue];
                        mainapp.InwardItemsFromSupplierDetails.Brand = form[brandvalue];
                        mainapp.InwardItemsFromSupplierDetails.Size = form[sizevalue];
                        mainapp.InwardItemsFromSupplierDetails.Design = form[design];
                        mainapp.InwardItemsFromSupplierDetails.DesignName = form[designName];
                        mainapp.InwardItemsFromSupplierDetails.Status = "Active";
                        mainapp.InwardItemsFromSupplierDetails.ModifiedOn = DateTime.Now;
                        mainapp.InwardItemsFromSupplierDetails.PoNo = "NoPO";
                        mainapp.InwardItemsFromSupplierDetails.InwardNo = model.InwardFromSupplierDetails.InwardNo;
                        mainapp.InwardItemsFromSupplierDetails.Category = _itemsubcategoryservice.GetCategoryBySubCat(form[subcat]);
                        mainapp.InwardItemsFromSupplierDetails.SubCategory = form[subcat];
                        mainapp.InwardItemsFromSupplierDetails.ItemTax = form[itemtax];
                        mainapp.InwardItemsFromSupplierDetails.ProportionateTax = Convert.ToDouble(form[proptax]);
                        mainapp.InwardItemsFromSupplierDetails.ProportionateTaxAmt = Convert.ToDouble(form[proptaxamt]);
                        if (form[ItemType] == "Inventory")
                        {
                            var itemdetails = _ItemService.GetDescriptionByItemCode(mainapp.InwardItemsFromSupplierDetails.itemCode);
                            if (itemdetails.Barcode != null)
                            {
                                mainapp.InwardItemsFromSupplierDetails.Barcode = itemdetails.Barcode;
                            }
                            else
                            {
                                System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                                string barcodeNo = string.Empty;
                                var Lastbarcode = _BarcodeNumberService.GetLastBarcodeNumber();
                                int serialNo;
                                /*To Check if it's the first barcode*/
                                if (Lastbarcode == null)
                                {
                                    serialNo = 1111111;
                                }
                                else
                                {
                                    int position = (Lastbarcode.Number.IndexOf(".") - 2);
                                    serialNo = Convert.ToInt32(Lastbarcode.Number.Substring(1, position));
                                    serialNo += 1;
                                }
                                /*Creation Of Barcode*/
                                barcodeNo = mainapp.InwardItemsFromSupplierDetails.Item.Substring(0, 1).ToUpper() + serialNo.ToString() + mainapp.InwardItemsFromSupplierDetails.Item.Substring(mainapp.InwardItemsFromSupplierDetails.Item.Length - 1).ToUpper();
                                if (System.IO.File.Exists(Server.MapPath("../Barcode.txt")))
                                {
                                    System.IO.File.Delete(Server.MapPath("../BarCode.txt"));
                                }
                                System.IO.File.WriteAllText(Server.MapPath("../BarCode.txt"), barcodeNo);
                                Process.Start(Server.MapPath("../BarCodeGenerate.exe"));

                                /*Saving the Barcode Number*/
                                BarcodeNumber BarcodeNumber = new BarcodeNumber();
                                BarcodeNumber.Number = (barcodeNo + ".png").ToString();
                                _BarcodeNumberService.CreateBarcode(BarcodeNumber);
                                mainapp.InwardItemsFromSupplierDetails.Barcode = barcodeNo + ".png";
                                var itemdata = _ItemService.GetDescriptionByItemCode(mainapp.InwardItemsFromSupplierDetails.itemCode);
                                itemdata.Barcode = mainapp.InwardItemsFromSupplierDetails.Barcode;
                                _ItemService.updateItem(itemdata);

                                OpeningStockMaster OpeningStockMaster = new OpeningStockMaster();
                                OpeningStockMaster.OpeningStockCode = openingstockcode;
                                OpeningStockMaster.itemCode = mainapp.InwardItemsFromSupplierDetails.itemCode;
                                OpeningStockMaster.ItemName = mainapp.InwardItemsFromSupplierDetails.Item;
                                OpeningStockMaster.Barcode = mainapp.InwardItemsFromSupplierDetails.Barcode;
                                OpeningStockMaster.Brand = mainapp.InwardItemsFromSupplierDetails.Brand;
                                OpeningStockMaster.Category = mainapp.InwardItemsFromSupplierDetails.Category;
                                OpeningStockMaster.SubCategory = mainapp.InwardItemsFromSupplierDetails.SubCategory;
                                OpeningStockMaster.Color = mainapp.InwardItemsFromSupplierDetails.Color;
                                OpeningStockMaster.Description = mainapp.InwardItemsFromSupplierDetails.Description;
                                OpeningStockMaster.DesignCode = mainapp.InwardItemsFromSupplierDetails.Design;
                                OpeningStockMaster.DesignName = mainapp.InwardItemsFromSupplierDetails.DesignName;
                                OpeningStockMaster.Material = mainapp.InwardItemsFromSupplierDetails.Material;
                                OpeningStockMaster.Unit = mainapp.InwardItemsFromSupplierDetails.Unit;
                                OpeningStockMaster.Size = mainapp.InwardItemsFromSupplierDetails.Size;
                                OpeningStockMaster.NumberType = mainapp.InwardItemsFromSupplierDetails.NumberType;
                                OpeningStockMaster.ItemQuantity = 0;
                                OpeningStockMaster.TotalQuantity = 0;
                                OpeningStockMaster.CostPrice = mainapp.InwardItemsFromSupplierDetails.CostPrice;
                                OpeningStockMaster.SellingPrice = mainapp.InwardItemsFromSupplierDetails.SellingPrice;
                                OpeningStockMaster.MRP = mainapp.InwardItemsFromSupplierDetails.MRP;
                                OpeningStockMaster.Status = "Active";
                                OpeningStockMaster.ModifiedOn = DateTime.Now;
                                _OpeningStockService.CreateStock(OpeningStockMaster);
                            }
                        }
                        mainapp.InwardItemsFromSupplierDetails.PurchaseReturn = "No";
                        _InwardItemFromSupplierService.CreateInwardItems(mainapp.InwardItemsFromSupplierDetails);
                    }
                }
            }

            model.InwardFromSupplierDetails.ChallanNo = form["InwardFromSupplierDetails.ChallanNo"];
            if (model.InwardFromSupplierDetails.ChallanNo != "")
            {
                model.InwardFromSupplierDetails.ChallanDate = Convert.ToDateTime(form["InwardFromSupplierDetails.ChallanDate"]);
            }
            else
            {
                model.InwardFromSupplierDetails.ChallanDate = null;
                model.InwardFromSupplierDetails.ChallanNo = null;
            }
            model.InwardFromSupplierDetails.BillNo = form["InwardFromSupplierDetails.BillNo"];
            if (model.InwardFromSupplierDetails.BillNo != "")
            {
                model.InwardFromSupplierDetails.BillDate = Convert.ToDateTime(form["InwardFromSupplierDetails.BillDate"]);
            }
            else
            {
                model.InwardFromSupplierDetails.BillDate = null;
                model.InwardFromSupplierDetails.BillNo = null;
            }

            model.InwardFromSupplierDetails.InwardNo = model.InwardFromSupplierDetails.InwardNo;
            model.InwardFromSupplierDetails.PoNo = "NoPO";
            model.InwardFromSupplierDetails.DelDate = DateTime.Now;
            model.InwardFromSupplierDetails.InwardDate = DateTime.Now;
            model.InwardFromSupplierDetails.ReceivingDate = DateTime.Now;
            model.InwardFromSupplierDetails.TotalAmount = Convert.ToDouble(form["Total_Amount_Value"]);
            model.InwardFromSupplierDetails.TotalTaxAmount = Convert.ToDouble(form["TotalTaxAmountValue"]);
            model.InwardFromSupplierDetails.GrandTotal = Convert.ToDouble(form["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.PaymentBalance = Convert.ToDouble(form["Grand_Total_Value"]);
            model.InwardFromSupplierDetails.Status = "Active";
            model.InwardFromSupplierDetails.ModifiedOn = DateTime.Now;
            _InwardFromSupplierService.UpdateInward(model.InwardFromSupplierDetails);

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(form["ItemTaxCount"]);
            model.PurchaseInventoryTaxDetails = new PurchaseInventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ItemTaxNumber" + i;
                string taxamount = "AddedTaxAmounthdn" + i;
                string amountontax = "AddedAmounthdn" + i;
                if (Convert.ToDouble(form[taxamount]) != 0)
                {
                    model.PurchaseInventoryTaxDetails.Code = model.InwardFromSupplierDetails.InwardNo;
                    model.PurchaseInventoryTaxDetails.Amount = form[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = form[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = form[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }
            TempData["PreviousInward"] = model.InwardFromSupplierDetails.InwardNo;
            string InwardId = Encode(model.InwardFromSupplierDetails.InwardId.ToString());
            return RedirectToAction("InwardWithoutPODetails/" + InwardId, "Inward");
        }

        public ActionResult InwardWithoutPOPrint()
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
        public JsonResult InwardWOPOListForPrint(string term)
        {
            MainApplication model = new MainApplication();
            List<string> names = new List<string>();
            var data = _InwardFromSupplierService.GetAllInwardsWithoutPoes(term);
            foreach (var item in data)
            {
                names.Add(item.InwardNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardWithoutPODetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetDetailsByPINo(InwardNo);
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(InwardNo);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(InwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult NewItemDetailsWOPO(int LastItemBefore, int LastItemAfter, int LastNonInventoryItemBefore, int LastNonInventoryItemAfter)
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
        public ActionResult NewItemInwardWOPO()
        {
            MainApplication model = new MainApplication();
            return View(model);
        }

        [HttpPost]
        public ActionResult NewItemInwardWOPO(MainApplication model, FormCollection frmItem)
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
            List<string> ItemCodeList = new List<string>();
            int mainrows = Convert.ToInt32(frmItem["MainPagehdnRowCount"]);
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
                    string CommissionInPercent = "CommissionInPercent" + i;
                    string CommissionInRupees = "CommissionInRupees" + i;

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
                    string finalCommissionInPercent = frmItem[CommissionInPercent];
                    string finalCommissionInRupees = frmItem[CommissionInRupees];

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
                            model.ItemDetails.itemCode = itemcode;
                            ItemCodeList.Add(itemcode);
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
                            ItemCodeList.Add(noninvitemcode);
                            _NonInventoryItemService.createNonInventoryItem(model.NonInventoryItemDetails);
                        }
                    }
                }
            }

            TempData["ItemCodeList"] = ItemCodeList;
            TempData["MainPageRowCount"] = mainrows - ItemCodeList.Count();
            int lastitemafter = _ItemService.GetLastItem().itemId;
            var lastnoninventoryitemafter = _NonInventoryItemService.GetLastItem();
            int lnoninvitemafter;
            if (lastnoninventoryitemafter != null)
            {
                lnoninvitemafter = lastnoninventoryitemafter.itemId;
            }
            else
            {
                lnoninvitemafter = 1;
            }
            return RedirectToAction("NewItemDetailsWOPO", "Inward", new { LastItemBefore = lastitembefore, LastItemAfter = lastitemafter, LastNonInventoryItemBefore = lastnoninventoryitembefore, LastNonInventoryItemAfter = lnoninvitemafter });
        }


        [HttpGet]
        public JsonResult GetTransportDetails(string name)
        {
            var transport = _TransportService.GetByName(name);
            return Json(new { name = transport.TransportName, no = transport.ContactNo1 }, JsonRequestBehavior.AllowGet);
        }

        // ************************************** END INWARD WITHOUT PO ******************************************

        // ************************************** START INWARD FROM GODOWN ******************************************

        [HttpGet]
        public ActionResult InwardFromGodown()
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
                        if (assigndetails.AssignRightsCode.Contains("SH"))
                        {
                            Session["LOGINSHOPGODOWNCODEIWG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            string shopcodes = Session["LOGINSHOPGODOWNCODEIWG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardFromGodownService.GetLastInwardByFinYear(finyr, shopcodes);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 3, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string shopname = Session["SHOPGODOWNNAMEIWG"] as string;
            var shopdetails = _ShopService.GetShopDetailsByName(shopname);
            string utilityname = shopdetails.ShortCode + "/IWG";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.PerformaCode = inwardcode;
            TempData["PreviousGodownInward"] = inwardcode;

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            model.OutwardToShopList = _OutwardFromGodownToShopService.GetActiveOutwards();
            model.ShopList = _ShopService.GetAll();


            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNumbers(string term)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODEIWG"] as string;
            var result = _OutwardFromGodownToShopService.GetOutwardNo(term, shopcode);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetShopDetails(string ShopCode)
        {
            var details = _ShopService.GetShopByCode(ShopCode);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOutwardAndShopDetails(string outwardno)
        {
            var data = _OutwardFromGodownToShopService.GetDetailsByOutwardCode(outwardno);
            var godowndetails = _godownMasterService.GetGodownByCode(data.GodownCode);
            if (data != null)
            {
                return Json(new
                {
                    data.Narration,
                    data.GodownCode,
                    data.GatePass,
                    GodownName = godowndetails.GodownName,
                    GodownContactNo = godowndetails.ContactNo1,
                    GodownContactPerson = godowndetails.EmpName,
                    GodownEmail = godowndetails.GodownName,
                    data.ShopCode,
                    data.ShopName,
                    data.ShopAddress,
                    data.ShopContactNo,
                    data.ShopEmail,
                }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(new { msg = "No" }, JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public ActionResult GetOutwardItems(string outwardcode, string type)
        {
            MainApplication model = new MainApplication();
            if (type == "shop")
            {
                model.OutwardItemToShopList = _OutwardItemFromGodownToShopService.GetDetailsByOutwardNo(outwardcode);
                TempData["OutwardItemList"] = model.OutwardItemToShopList;
            }
            else
            {
                model.OutwardItemStkDisList = _OutwardItemStkDisService.GetItemsByOutwardCode(outwardcode);
                TempData["OutwardItemList"] = model.OutwardItemStkDisList;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardFromGodown(MainApplication model, FormCollection form)
        {
            string shopcodes = Session["LOGINSHOPGODOWNCODEIWG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardFromGodownService.GetLastInwardByFinYear(finyr, shopcodes);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 3, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string shopnames = Session["SHOPGODOWNNAMEIWG"] as string;
            var shopdetails = _ShopService.GetShopDetailsByName(shopnames);
            string utilityname = shopdetails.ShortCode + "/IWG";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;

            model.InwardFromGodownDetails.InwardCode = inwardcode;
            model.InwardFromGodownDetails.Status = "Active";
            model.InwardFromGodownDetails.ModifiedOn = DateTime.Now;
            model.InwardFromGodownDetails.Date = DateTime.Now;
            _InwardFromGodownService.Create(model.InwardFromGodownDetails);
            var list = TempData["OutwardItemList"] as IEnumerable<OutwardItemToShop>;

            foreach (var data in list)
            {
                InwardItemFromGodown InwardItem = new InwardItemFromGodown();
                InwardItem.ItemCode = data.ItemCode;
                InwardItem.ItemName = data.ItemName;
                InwardItem.ItemDescription = data.ItemDescription;
                InwardItem.Quantity = data.OutwardQuantity;
                InwardItem.InwardCode = inwardcode;
                InwardItem.Color = data.Color;
                InwardItem.Material = data.Material;
                InwardItem.DesignName = data.DesignName;
                InwardItem.SellingPrice = data.SellingPrice;
                InwardItem.MRP = data.MRP;
                InwardItem.Unit = data.Unit;
                InwardItem.Status = "Active";
                InwardItem.ModifiedOn = DateTime.Now;
                InwardItem.Barcode = data.Barcode;
                InwardItem.ShopCode = data.ShopCode;
                InwardItem.GodownCode = data.GodownCode;
                _InwardItemFromGodownService.Create(InwardItem);

                var shopstock = _ShopStockService.GetDetailsByItemCodeAndShopCode(data.ItemCode, data.ShopCode);
                if (shopstock != null)
                {
                    shopstock.Quantity += data.OutwardQuantity;
                    shopstock.ModifiedOn = DateTime.Now;
                    _ShopStockService.Update(shopstock);
                }
                else
                {
                    ShopStock shopstockitem = new ShopStock();
                    shopstockitem.Barcode = data.Barcode;
                    shopstockitem.Brand = data.Brand;
                    shopstockitem.Color = data.Color;
                    shopstockitem.Description = data.ItemDescription;
                    shopstockitem.DesignCode = data.DesignCode;
                    shopstockitem.DesignName = data.DesignName;
                    shopstockitem.ItemCode = data.ItemCode;
                    shopstockitem.ItemName = data.ItemName;
                    shopstockitem.Material = data.Material;
                    shopstockitem.NumberType = data.NumberType;
                    shopstockitem.Quantity = data.OutwardQuantity;
                    shopstockitem.SellingPrice = data.SellingPrice;
                    shopstockitem.MRP = data.MRP;
                    shopstockitem.ShopCode = data.ShopCode;
                    string shopname = _ShopService.GetShopByCode(data.ShopCode).ShopName;
                    shopstockitem.ShopName = shopname;
                    shopstockitem.Size = data.Size;
                    shopstockitem.Status = "Active";
                    shopstockitem.ModifiedOn = DateTime.Now;
                    shopstockitem.Unit = data.Unit;
                    _ShopStockService.Create(shopstockitem);
                }
                var stockitem = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(data.ItemCode, data.ShopCode);
                if (stockitem != null)
                {
                    stockitem.ItemQuantity += data.OutwardQuantity;
                    stockitem.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Update(stockitem);
                }
                else
                {
                    StockItemDistribution stkitem = new StockItemDistribution();
                    stkitem.Barcode = data.Barcode;
                    stkitem.Brand = data.Brand;
                    stkitem.Code = data.ShopCode;
                    stkitem.Color = data.Color;
                    stkitem.Description = data.ItemDescription;
                    stkitem.DesignCode = data.DesignCode;
                    stkitem.DesignName = data.DesignName;
                    string shopname = _ShopService.GetShopByCode(data.ShopCode).ShopName;
                    stkitem.GodownShopName = shopname;
                    stkitem.ItemCode = data.ItemCode;
                    stkitem.ItemName = data.ItemName;
                    stkitem.ItemQuantity = data.OutwardQuantity;
                    stkitem.Material = data.Material;
                    stkitem.ModifiedOn = DateTime.Now;
                    stkitem.NumberType = data.NumberType;
                    stkitem.MRP = data.MRP;
                    stkitem.SellingPrice = data.SellingPrice;
                    stkitem.Size = data.Size;
                    stkitem.Status = "Active";
                    stkitem.StockDistributionCode = model.InwardFromGodownDetails.OutwardNo;
                    stkitem.Unit = data.Unit;
                    _stockitemdistributionservice.Create(stkitem);
                }
            }

            var GetOutward = _OutwardFromGodownToShopService.GetDetailsByOutwardCode(model.InwardFromGodownDetails.OutwardNo);
            GetOutward.Status = "InActive";
            _OutwardFromGodownToShopService.Udpate(GetOutward);

            var lastrow = _InwardFromGodownService.GetLastRow();
            string inwrdid = Encode(lastrow.Id.ToString());
            return RedirectToAction("InwardFromGodownDetails/" + inwrdid, "Inward");
        }

        [HttpGet]
        public ActionResult InwardFromGodownDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromGodownDetails = new InwardFromGodown(),
            };

            int Id = Decode(id);
            model.InwardFromGodownDetails = _InwardFromGodownService.GetDetailsById(Id);
            model.InwardItemFromGodownlist = _InwardItemFromGodownService.GetDetailsByCode(model.InwardFromGodownDetails.InwardCode);
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            string previousgodowninward = TempData["PreviousGodownInward"].ToString();
            if (TempData["PreviousGodownInward"].ToString() != model.InwardFromGodownDetails.InwardCode)
            {
                ViewData["GodownInwardNoChanged"] = previousgodowninward + " is replaced with " + model.InwardFromGodownDetails.InwardCode + " because " + previousgodowninward + " is acquired by another person";
            }
            TempData["PreviousGodownInward"] = previousgodowninward;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardFromGodown(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromGodownDetails = new InwardFromGodown(),
            };
            int Id = Decode(id);
            model.InwardFromGodownDetails = _InwardFromGodownService.GetDetailsById(Id);
            model.InwardItemFromGodownlist = _InwardItemFromGodownService.GetDetailsByCode(model.InwardFromGodownDetails.InwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardFromGodownPrint()
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
                            Session["LOGINSHOPGODOWNCODEIWGS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWGS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWGS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWGS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetInwardNumbersFromGodown(string term)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODEIWGS"] as string;
            var result = _InwardFromGodownService.GetInwardNo(term, shopcode);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.InwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardFromGodownDetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardFromGodownDetails = _InwardFromGodownService.GetDetailsByCode(InwardNo);
            model.InwardItemFromGodownlist = _InwardItemFromGodownService.GetDetailsByCode(InwardNo);
            return View(model);
        }

        // ************************************** END INWARD FROM GODOWN ******************************************

        // ************************************** START INWARD FROM STOCK DISTRIBUTION ******************************************

        [HttpGet]
        public ActionResult InwardFromStockDistribution()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

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
                            Session["LOGINSHOPGODOWNCODEIWSD"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSD"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWSD"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSD"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            string code = Session["LOGINSHOPGODOWNCODEIWSD"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            InwardFromStockDistribution lastinward = new InwardFromStockDistribution();
            if (code.Contains("SH"))
            {
                lastinward = _InwardFromStkDistService.GetLastInwardByFinYearForShop(finyr, code);
            }
            else
            {
                lastinward = _InwardFromStkDistService.GetLastInwardByFinYearForGodown(finyr, code);
            }

            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardNo;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardNo.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string utilityname = string.Empty;
            string gdshopname = Session["SHOPGODOWNNAMEIWSD"] as string;
            if (code.Contains("SH"))
            {
                var details = _ShopService.GetShopDetailsByName(gdshopname);
                utilityname = details.ShortCode + "/IWSD";
            }
            else
            {
                var details = _godownMasterService.GetGodownDetailsByName(gdshopname);
                utilityname = details.ShortCode + "/IWSD";
            }

            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.PerformaCode = inwardcode;
            TempData["PreviousStockInward"] = inwardcode;
            model.EmpList = _EmployeeMasterService.getAllemployees();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNos(string term)
        {
            string code = Session["LOGINSHOPGODOWNCODEIWSD"] as string;

            var result = _OutwardStkDisService.GetOutwardNo(term, code);
            List<string> names = new List<string>();
            foreach (var item in result)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGodownShopPersonDetails(string GodownShopCode)
        {
            if (GodownShopCode.Contains("SH"))
            {
                var details = _ShopService.GetShopByCode(GodownShopCode);
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var details = _godownMasterService.GetGodownByCode(GodownShopCode);
                return Json(details, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetOutwardandGodownDetails(string outward)
        {
            string msg = "";
            var data = _OutwardStkDisService.GetOutwardByCode(outward);
            if (data.Code.Contains("GD"))
            {
                msg = "GodownDetails";
            }
            else
            {
                msg = "ShopDetails";
            }

            if (data != null)
            {
                return Json(new
                {
                    msg,
                    data.Narration,
                    data.Code,
                    data.GatePass,
                    data.GodownName,
                    data.GodownContactNo,
                    data.GodownContactPerson,
                    data.ShopName,
                    data.ShopContactNo,
                    data.ShopContactPerson,
                }, JsonRequestBehavior.AllowGet);
            }
            else { return Json(new { msg = "No" }, JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public ActionResult InwardFromStockDistribution(MainApplication model, FormCollection form)
        {
            string codes = Session["LOGINSHOPGODOWNCODEIWSD"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            InwardFromStockDistribution lastinward = new InwardFromStockDistribution();
            if (codes.Contains("SH"))
            {
                lastinward = _InwardFromStkDistService.GetLastInwardByFinYearForShop(finyr, codes);
            }
            else
            {
                lastinward = _InwardFromStkDistService.GetLastInwardByFinYearForGodown(finyr, codes);
            }
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardNo;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardNo.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string gdshopname = Session["SHOPGODOWNNAMEIWSD"] as string;
            string utilityname = string.Empty;

            if (codes.Contains("SH"))
            {
                var details = _ShopService.GetShopDetailsByName(gdshopname);
                utilityname = details.ShortCode + "/IWSD";
            }
            else
            {
                var details = _godownMasterService.GetGodownDetailsByName(gdshopname);
                utilityname = details.ShortCode + "/IWSD";
            }

            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;

            model.InwardFromStockDistributionDetails.InwardNo = inwardcode;
            model.InwardFromStockDistributionDetails.Status = "Active";
            model.InwardFromStockDistributionDetails.ModifiedOn = DateTime.Now;
            model.InwardFromStockDistributionDetails.InwardDate = DateTime.Now;
            _InwardFromStkDistService.Create(model.InwardFromStockDistributionDetails);
            var list = TempData["OutwardItemList"] as IEnumerable<OutwardItemStockDistribution>;
            foreach (var outwarditem in list)
            {
                InwardItemFromStockDistribution inwarditem = new InwardItemFromStockDistribution();
                inwarditem.InwardNo = inwardcode;
                inwarditem.ItemCode = outwarditem.ItemCode;
                inwarditem.ItemName = outwarditem.ItemName;
                inwarditem.Description = outwarditem.Description;
                inwarditem.Color = outwarditem.Color;
                inwarditem.Material = outwarditem.Material;
                inwarditem.DesignCode = outwarditem.DesignCode;
                inwarditem.DesignName = outwarditem.DesignName;
                inwarditem.Unit = outwarditem.Unit;
                inwarditem.NumberType = outwarditem.NumberType;
                inwarditem.Brand = outwarditem.Brand;
                inwarditem.Size = outwarditem.Size;
                inwarditem.Barcode = outwarditem.Barcode;
                inwarditem.ItemQuantity = outwarditem.ItemQuantity;
                inwarditem.SellingPrice = outwarditem.SellingPrice;
                inwarditem.MRP = outwarditem.MRP;
                inwarditem.Status = "Active";
                inwarditem.ModifiedOn = DateTime.Now;
                _InwardItemFromStockDistributionService.Create(inwarditem);

                //if stock distribution for shop then it save into the shop stock
                string code = model.InwardFromStockDistributionDetails.Code;
                if (code.Contains("SH"))
                {
                    //if item with same shop name is already not present in the database then cretae new row
                    var shopitem = _ShopStockService.GetDetailsByItemCodeAndShopCode(inwarditem.ItemCode, code);
                    if (shopitem == null)
                    {
                        ShopStock shopstockitem = new ShopStock();
                        shopstockitem.ItemCode = inwarditem.ItemCode;
                        shopstockitem.ItemName = inwarditem.ItemName;
                        shopstockitem.Description = inwarditem.Description;
                        shopstockitem.Color = inwarditem.Color;
                        shopstockitem.Material = inwarditem.Material;
                        shopstockitem.DesignCode = inwarditem.DesignCode;
                        shopstockitem.DesignName = inwarditem.DesignName;
                        shopstockitem.Unit = inwarditem.Unit;
                        shopstockitem.NumberType = inwarditem.NumberType;
                        shopstockitem.Brand = inwarditem.Brand;
                        shopstockitem.Size = inwarditem.Size;
                        shopstockitem.Barcode = inwarditem.Barcode;
                        shopstockitem.Quantity = inwarditem.ItemQuantity;
                        shopstockitem.SellingPrice = Convert.ToDouble(inwarditem.SellingPrice);
                        shopstockitem.MRP = Convert.ToDouble(inwarditem.MRP);
                        shopstockitem.ShopCode = code;
                        shopstockitem.ShopName = model.InwardFromStockDistributionDetails.ShopName;
                        shopstockitem.Status = "Active";
                        shopstockitem.ModifiedOn = DateTime.Now;
                        _ShopStockService.Create(shopstockitem);
                    }
                    //if item with same shop name is already present in the database then update this row
                    else
                    {
                        shopitem.Quantity = shopitem.Quantity + inwarditem.ItemQuantity;
                        shopitem.Status = "Active";
                        shopitem.ModifiedOn = DateTime.Now;
                        _ShopStockService.Update(shopitem);
                    }
                }
                //if stock distribution for shop then it save into the shop stock
                else
                {
                    //if item with same shop name is already not present in the database then cretae new row
                    var godownitem = _GodownStockService.GetDetailsByItemCodeAndGodownCode(inwarditem.ItemCode, code);
                    if (godownitem == null)
                    {
                        GodownStock gdstockitem = new GodownStock();
                        gdstockitem.ItemCode = inwarditem.ItemCode;
                        gdstockitem.ItemName = inwarditem.ItemName;
                        gdstockitem.Description = inwarditem.Description;
                        gdstockitem.Color = inwarditem.Color;
                        gdstockitem.Material = inwarditem.Material;
                        gdstockitem.DesignCode = inwarditem.DesignCode;
                        gdstockitem.DesignName = inwarditem.DesignName;
                        gdstockitem.Unit = inwarditem.Unit;
                        gdstockitem.NumberType = inwarditem.NumberType;
                        gdstockitem.Brand = inwarditem.Brand;
                        gdstockitem.Size = inwarditem.Size;
                        gdstockitem.Barcode = inwarditem.Barcode;
                        gdstockitem.Quantity = inwarditem.ItemQuantity;
                        gdstockitem.SellingPrice = Convert.ToDouble(inwarditem.SellingPrice);
                        gdstockitem.MRP = Convert.ToDouble(inwarditem.MRP);
                        gdstockitem.GodownCode = code;
                        gdstockitem.GodownName = model.InwardFromStockDistributionDetails.GodownName;
                        gdstockitem.Status = "Active";
                        gdstockitem.ModifiedOn = DateTime.Now;
                        _GodownStockService.Create(gdstockitem);
                    }
                    else
                    {
                        godownitem.Quantity = godownitem.Quantity + inwarditem.ItemQuantity;
                        godownitem.Status = "Active";
                        godownitem.ModifiedOn = DateTime.Now;
                        _GodownStockService.Update(godownitem);
                    }
                }


            }
            var outwarddata = _OutwardStkDisService.GetOutwardByCode(model.InwardFromStockDistributionDetails.StockDistributionNo);
            outwarddata.Status = "InActive";
            outwarddata.ModifiedOn = DateTime.Now;
            _OutwardStkDisService.Update(outwarddata);

            var LastRow = _InwardFromStkDistService.GetLastRow();
            string inwardid = Encode(LastRow.InwardId.ToString());
            return RedirectToAction("InwardFromStockDistributionDetails/" + inwardid, "Inward");
        }

        //DETAILS PAGE OF INWARD FROM STOCK DISTRIBUTION..
        public ActionResult InwardFromStockDistributionDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromStockDistributionDetails = new InwardFromStockDistribution(),
            };

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.InwardFromStockDistributionDetails = _InwardFromStkDistService.GetDetailsById(Id);
            model.InwardItemFromStockDistributionList = _InwardItemFromStockDistributionService.GetItemListByCode(model.InwardFromStockDistributionDetails.InwardNo);

            string previousstockinward = TempData["PreviousStockInward"].ToString();
            if (TempData["PreviousStockInward"].ToString() != model.InwardFromStockDistributionDetails.InwardNo)
            {
                ViewData["StockInwardNoChanged"] = previousstockinward + " is replaced with " + model.InwardFromStockDistributionDetails.InwardNo + " because " + previousstockinward + " is acquired by another person";
            }
            TempData["PreviousStockInward"] = previousstockinward;
            return View(model);
        }

        public ActionResult PrintInwardFromStockDistribution(string id)
        {
            MainApplication model = new MainApplication()
            {
                InwardFromStockDistributionDetails = new InwardFromStockDistribution(),
            };
            int Id = Decode(id);
            model.InwardFromStockDistributionDetails = _InwardFromStkDistService.GetDetailsById(Id);
            model.InwardItemFromStockDistributionList = _InwardItemFromStockDistributionService.GetItemListByCode(model.InwardFromStockDistributionDetails.InwardNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardFromStockDistributionPrint()
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
                            Session["LOGINSHOPGODOWNCODEIWSD"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSD"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWSD"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSD"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetInwardNumbersStockDistribution(string term)
        {
            string code = Session["LOGINSHOPGODOWNCODEIWSD"] as string;
            var result = _InwardFromStkDistService.GetInwardNo(term, code);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.InwardNo);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardStockDistributionDetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardFromStockDistributionDetails = _InwardFromStkDistService.GetDetailsByInwardNo(InwardNo);
            model.InwardItemFromStockDistributionList = _InwardItemFromStockDistributionService.GetItemListByCode(InwardNo);
            return View(model);
        }

        // ************************************** END INWARD FROM STOCK DISTRIBUTION ******************************************

        public ActionResult LoginVerification()
        {
            return View();
        }

        /******************************************* Inward From Shop To Godown ****************************************/


        [HttpGet]
        public ActionResult InwardFromShopToGodown()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.InwardFromShopToGodownDetails = new InwardFromShopToGodown();

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
                            Session["LOGINSHOPGODOWNCODEIWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            string gdcode = Session["LOGINSHOPGODOWNCODEIWSG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardFromShopToGodownService.GetLastInwardByFinYear(finyr, gdcode);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string godownname = Session["SHOPGODOWNNAMEIWSG"] as string;
            var gddetails = _godownMasterService.GetGodownDetailsByName(godownname);
            string utilityname = gddetails.ShortCode + "/IWSG";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.InwardFromShopToGodownDetails.InwardCode = inwardcode;
            TempData["PreviousShopInward"] = inwardcode;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNosForShopToGodown(string term)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEIWSG"] as string;
            List<string> names = new List<string>();
            var data = _OutwardShopToGodownService.GetOutwardNos(term, godowncode);
            foreach (var item in data)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeDetails(string Email)
        {
            var details = _EmployeeMasterService.getEmpByEmail(Email);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStoreKeeperDetails(string GodownCode)
        {
            var details = _godownMasterService.GetGodownByCode(GodownCode);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOutwardShopToGodownDetails(string Outwardno)
        {
            var details = _OutwardShopToGodownService.GetDetailsByOutwardNo(Outwardno);
            var shopdetails = _ShopService.GetShopByCode(details.ShopCode);
            return Json(new { details, shopdetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemListShopToGodown(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.OutwardItemShopToGodownList = _OutwardItemShopToGodownService.GetItemsByOutwardNo(OutwardNo);
            TempData["ShopToGodownItems"] = model.OutwardItemShopToGodownList;
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardFromShopToGodown(MainApplication model, FormCollection form)
        {
            string gdcode = Session["LOGINSHOPGODOWNCODEIWSG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardFromShopToGodownService.GetLastInwardByFinYear(finyr, gdcode);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string godownname = Session["SHOPGODOWNNAMEIWSG"] as string;
            var gddetails = _godownMasterService.GetGodownDetailsByName(godownname);
            string utilityname = gddetails.ShortCode + "/IWSG";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.InwardFromShopToGodownDetails.InwardCode = inwardcode;

            model.InwardFromShopToGodownDetails.ModifiedOn = DateTime.Now;
            model.InwardFromShopToGodownDetails.Date = DateTime.Now;
            model.InwardFromShopToGodownDetails.Status = "Active";
            _InwardFromShopToGodownService.Create(model.InwardFromShopToGodownDetails);

            var details = _OutwardShopToGodownService.GetDetailsByOutwardNo(model.InwardFromShopToGodownDetails.OutwardNo);
            details.Status = "InActive";
            details.ModifiedOn = DateTime.Now;
            _OutwardShopToGodownService.Update(details);

            var list = TempData["ShopToGodownItems"] as IEnumerable<OutwardItemShopToGodown>;
            int count = 1;
            foreach (var data in list)
            {
                InwardItemFromShopToGodown item = new InwardItemFromShopToGodown();
                var quantity = "quantity" + count;
                item.Quantity = Convert.ToDouble(data.OutwardQuantity);
                item.Barcode = data.Barcode;
                item.Color = data.Color;
                item.Description = data.ItemDescription;
                item.DesignName = data.DesignName;
                item.DesignCode = data.DesignCode;
                item.Size = data.Size;
                item.NumberType = data.NumberType;
                item.Brand = data.Brand;
                item.InwardCode = model.InwardFromShopToGodownDetails.InwardCode;
                item.ShopCode = model.InwardFromShopToGodownDetails.ShopCode;
                item.GodownCode = model.InwardFromShopToGodownDetails.GodownCode;
                item.ItemCode = data.ItemCode;
                item.ItemName = data.ItemName;
                item.Material = data.Material;
                item.SellingPrice = data.SellingPrice;
                item.MRP = data.MRP;
                item.Unit = data.Unit;
                item.Status = "Active";
                item.ModifiedOn = DateTime.Now;
                _InwardItemFromShopToGodownService.Create(item);

                var stkdistitemforgd = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(data.ItemCode, model.InwardFromShopToGodownDetails.GodownCode);

                var godownstkitem = _GodownStockService.GetDetailsByItemCodeAndGodownCode(data.ItemCode, model.InwardFromShopToGodownDetails.GodownCode);

                if (stkdistitemforgd != null)
                {
                    stkdistitemforgd.ItemQuantity += Convert.ToDouble(data.OutwardQuantity);
                    stkdistitemforgd.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Update(stkdistitemforgd);
                }
                else
                {
                    StockItemDistribution stock = new StockItemDistribution();
                    stock.Barcode = data.Barcode;
                    stock.Brand = data.Brand;
                    stock.Color = item.Color;
                    stock.Description = data.ItemDescription;
                    stock.DesignCode = data.DesignCode;
                    stock.DesignName = data.DesignName;
                    stock.Unit = data.Unit;
                    stock.ModifiedOn = DateTime.Now;
                    stock.SellingPrice = item.SellingPrice;
                    stock.MRP = item.MRP;
                    stock.NumberType = data.NumberType;
                    stock.Material = item.Material;
                    stock.Size = data.Size;
                    stock.ItemQuantity = Convert.ToDouble(data.OutwardQuantity);
                    stock.StockDistributionCode = model.InwardFromShopToGodownDetails.InwardCode;
                    stock.Code = model.InwardFromShopToGodownDetails.GodownCode;
                    stock.GodownShopName = model.InwardFromShopToGodownDetails.GodownName;
                    stock.ItemCode = data.ItemCode;
                    stock.ItemName = data.ItemName;
                    stock.Status = "Active";
                    stock.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Create(stock);
                }

                if (godownstkitem != null)
                {
                    godownstkitem.Quantity += Convert.ToDouble(data.OutwardQuantity);
                    godownstkitem.ModifiedOn = DateTime.Now;
                    _GodownStockService.Update(godownstkitem);
                }
                else
                {
                    GodownStock gdstock = new GodownStock();
                    gdstock.Barcode = data.Barcode;
                    gdstock.Brand = data.Brand;
                    gdstock.Color = data.Color;
                    gdstock.Description = data.ItemDescription;
                    gdstock.DesignCode = data.DesignCode;
                    gdstock.DesignName = data.DesignName;
                    gdstock.Unit = data.Unit;
                    gdstock.ModifiedOn = DateTime.Now;
                    gdstock.SellingPrice = data.SellingPrice;
                    gdstock.MRP = data.MRP;
                    gdstock.NumberType = data.NumberType;
                    gdstock.Material = data.Material;
                    gdstock.Size = data.Size;
                    gdstock.Quantity = Convert.ToDouble(data.OutwardQuantity);
                    gdstock.GodownCode = model.InwardFromShopToGodownDetails.GodownCode;
                    gdstock.GodownName = model.InwardFromShopToGodownDetails.GodownName;
                    gdstock.ItemCode = data.ItemCode;
                    gdstock.ItemName = data.ItemName;
                    gdstock.Status = "Active";
                    gdstock.ModifiedOn = DateTime.Now;
                    _GodownStockService.Create(gdstock);
                }

                count++;
            }
            var lastrow = _InwardFromShopToGodownService.GetLastInward();
            string inwardid = Encode(lastrow.InwardId.ToString());
            return RedirectToAction("InwardFromShopToGodownDetails/" + inwardid, "Inward");
        }

        [HttpGet]
        public ActionResult InwardFromShopToGodownDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            int Id = Decode(id);
            model.InwardFromShopToGodownDetails = _InwardFromShopToGodownService.GetInwardDetailById(Id);
            model.InwardItemFromShopToGodownList = _InwardItemFromShopToGodownService.GetItemsByInwardNo(model.InwardFromShopToGodownDetails.InwardCode);
            string previousinward = TempData["PreviousShopInward"].ToString();
            if (TempData["PreviousShopInward"].ToString() != model.InwardFromShopToGodownDetails.InwardCode)
            {
                ViewData["InwardChanged"] = previousinward + " is replaced with " + model.InwardFromShopToGodownDetails.InwardCode + " because " + previousinward + " is acquired by another person";
            }
            TempData["PreviousShopInward"] = previousinward;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInwardFromShopToGodown(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.InwardFromShopToGodownDetails = _InwardFromShopToGodownService.GetInwardDetailById(Id);
            model.InwardItemFromShopToGodownList = _InwardItemFromShopToGodownService.GetItemsByInwardNo(model.InwardFromShopToGodownDetails.InwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardFromShopToGodownPrint()
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
                            Session["LOGINSHOPGODOWNCODEIWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWSG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        public JsonResult GetInwardNumbersShopToGodown(string term)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEIWSG"] as string;
            var result = _InwardFromShopToGodownService.GetInwardNo(term, godowncode);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.InwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardFromShopToGodownDetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardFromShopToGodownDetails = _InwardFromShopToGodownService.GetDetailsByInwardCode(InwardNo);
            model.InwardItemFromShopToGodownList = _InwardItemFromShopToGodownService.GetItemsByInwardNo(InwardNo);
            return View(model);
        }

        /************************************ Inward Shop To Godown End *******************/

        /*************************************** Inward Inter Godown **********************/


        [HttpGet]
        public ActionResult InwardInterGodown()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.InwardInterGodownDetails = new InwardInterGodown();

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
                            Session["LOGINSHOPGODOWNCODEIWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWGG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWGG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            string gdcode = Session["LOGINSHOPGODOWNCODEIWGG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardInterGodownService.GetLastInwardByFinYear(finyr, gdcode);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string gdname = Session["SHOPGODOWNNAMEIWGG"] as string;
            var gddetails = _godownMasterService.GetGodownDetailsByName(gdname);
            string utilityname = gddetails.ShortCode + "/IWGG";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.InwardInterGodownDetails.InwardCode = inwardcode;
            TempData["PreviousInterGodownInward"] = inwardcode;

            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNosInterGodown(string term)
        {
            string gdcode = Session["LOGINSHOPGODOWNCODEIWGG"] as string;
            var details = _OutwardInterGodownService.GetOutwardNos(term, gdcode);
            List<string> names = new List<string>();
            foreach (var item in details)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOutwardInterGodownDetails(string Outwardno)
        {
            var data = _OutwardInterGodownService.GetDetailsByOutwardCode(Outwardno);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemListInterGodown(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.OutwardItemInterGodownList = _OutwardInterGodownItemService.GetDetailsByOutwardCode(OutwardNo);
            TempData["InterGodownItems"] = model.OutwardItemInterGodownList;
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardInterGodown(MainApplication model, FormCollection form)
        {
            string gdcode = Session["LOGINSHOPGODOWNCODEIWGG"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardInterGodownService.GetLastInwardByFinYear(finyr, gdcode);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }

            string gdnames = Session["SHOPGODOWNNAMEIWGG"] as string;
            var gddetails = _godownMasterService.GetGodownDetailsByName(gdnames);
            string utilityname = gddetails.ShortCode + "/IWGG";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.InwardInterGodownDetails.InwardCode = inwardcode;

            model.InwardInterGodownDetails.Date = DateTime.Now;
            model.InwardInterGodownDetails.Status = "Active";
            model.InwardInterGodownDetails.ModifiedOn = DateTime.Now;
            _InwardInterGodownService.Create(model.InwardInterGodownDetails);

            var details = _OutwardInterGodownService.GetDetailsByOutwardCode(model.InwardInterGodownDetails.OutwardCode);
            details.Status = "InActive";
            details.ModifiedOn = DateTime.Now;
            _OutwardInterGodownService.Update(details);

            var itemlist = TempData["InterGodownItems"] as IEnumerable<OutwardItemInterGodown>;

            foreach (var item in itemlist)
            {
                InwardInterGodownItem inwarditem = new InwardInterGodownItem();
                inwarditem.Color = item.ColorCode;
                inwarditem.DesignName = item.DesignName;
                inwarditem.ItemDescription = item.Description;
                inwarditem.ItemCode = item.ItemCode;
                inwarditem.ItemName = item.ItemName;
                item.ToGdCode = model.InwardInterGodownDetails.FromGodownCode;
                item.FromGdCode = model.InwardInterGodownDetails.ToGodownCode;
                inwarditem.Material = item.TypeOfMaterial;
                inwarditem.Quantity = Convert.ToDouble(item.QuantityTransfer);
                inwarditem.SellingPrice = item.SellingPrice;
                inwarditem.MRP = item.MRP;
                inwarditem.InwardCode = model.InwardInterGodownDetails.InwardCode;
                inwarditem.FromGodownCode = model.InwardInterGodownDetails.FromGodownCode;
                inwarditem.ToGodownCode = model.InwardInterGodownDetails.ToGodownCode;
                inwarditem.Unit = item.Unit;
                inwarditem.Status = "Status";
                inwarditem.ModifiedOn = DateTime.Now;
                _InwardInterGodownItemService.Create(inwarditem);

                var stockdistitems = _stockitemdistributionservice.GetDetailsByItemCodeAndGodownCode(item.ItemCode, model.InwardInterGodownDetails.ToGodownCode);
                var godownstockitems = _GodownStockService.GetDetailsByItemCodeAndGodownCode(item.ItemCode, model.InwardInterGodownDetails.ToGodownCode);

                if (godownstockitems != null)
                {
                    godownstockitems.Quantity += Convert.ToDouble(item.QuantityTransfer);
                    godownstockitems.ModifiedOn = DateTime.Now;
                    _GodownStockService.Update(godownstockitems);
                }
                else
                {
                    GodownStock gdstock = new GodownStock();
                    gdstock.Barcode = item.Barcode;
                    gdstock.Brand = item.Brand;
                    gdstock.DesignCode = item.DesignCode;
                    gdstock.GodownCode = item.ToGdCode;
                    string gdname = _godownMasterService.GetGodownByCode(item.ToGdCode).GodownName;
                    gdstock.GodownName = gdname;
                    gdstock.NumberType = item.NumberType;
                    gdstock.Size = item.Size;
                    gdstock.Unit = item.Unit;
                    gdstock.Color = item.ColorCode;
                    gdstock.Description = item.Description;
                    gdstock.DesignName = item.DesignName;
                    gdstock.SellingPrice = item.SellingPrice;
                    gdstock.MRP = item.MRP;
                    gdstock.Quantity = Convert.ToDouble(item.QuantityTransfer);
                    gdstock.ItemCode = item.ItemCode;
                    gdstock.ItemName = item.ItemName;
                    gdstock.Material = item.TypeOfMaterial;
                    gdstock.Status = "Active";
                    gdstock.ModifiedOn = DateTime.Now;
                    _GodownStockService.Create(gdstock);
                }

                if (stockdistitems != null)
                {
                    stockdistitems.ItemQuantity += Convert.ToDouble(item.QuantityTransfer);
                    stockdistitems.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Update(stockdistitems);
                }
                else
                {
                    StockItemDistribution stkdist = new StockItemDistribution();
                    stkdist.Barcode = item.Barcode;
                    stkdist.Brand = item.Brand;
                    stkdist.DesignCode = item.DesignCode;
                    stkdist.NumberType = item.NumberType;
                    stkdist.Size = item.Size;
                    stkdist.Unit = item.Unit;
                    stkdist.Color = item.ColorCode;
                    stkdist.Code = model.InwardInterGodownDetails.ToGodownCode;
                    stkdist.Description = item.Description;
                    stkdist.DesignName = item.DesignName;
                    stkdist.Code = item.ToGdCode;
                    string gdname = _godownMasterService.GetGodownByCode(item.ToGdCode).GodownName;
                    stkdist.GodownShopName = gdname;
                    stkdist.ItemCode = item.ItemCode;
                    stkdist.ItemName = item.ItemName;
                    stkdist.ItemQuantity = Convert.ToDouble(item.QuantityTransfer);
                    stkdist.Material = item.TypeOfMaterial;
                    stkdist.SellingPrice = item.SellingPrice;
                    stkdist.MRP = item.MRP;
                    stkdist.Status = "Active";
                    stkdist.ModifiedOn = DateTime.Now;
                    stkdist.StockDistributionCode = model.InwardInterGodownDetails.OutwardCode;
                    stkdist.Unit = item.Unit;
                    _stockitemdistributionservice.Create(stkdist);
                }
            }

            var detail = _InwardInterGodownService.GetLastInward();
            string inwradid = Encode(detail.Id.ToString());
            return RedirectToAction("InwardInterGodownDetails/" + inwradid, "Inward");
        }

        [HttpGet]
        public ActionResult InwardInterGodownDetails(string Id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int id = Decode(Id);
            model.InwardInterGodownDetails = _InwardInterGodownService.GetDetailsById(id);
            model.InwardInterGodownItemList = _InwardInterGodownItemService.GetItemListByInwardCode(model.InwardInterGodownDetails.InwardCode);

            string previousinwardno = TempData["PreviousInterGodownInward"].ToString();
            if (previousinwardno != model.InwardInterGodownDetails.InwardCode)
            {
                ViewData["InterGodownInwardChanged"] = previousinwardno + " is replaced with " + model.InwardInterGodownDetails.InwardCode + " because " + previousinwardno + " is acquired by another person";
            }
            TempData["PreviousInterGodownInward"] = previousinwardno;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintInterGodown(string Id)
        {
            MainApplication model = new MainApplication();
            model.InwardInterGodownDetails = new InwardInterGodown();
            model.InwardInterGodownItemDetails = new InwardInterGodownItem();
            int id = Decode(Id);
            model.InwardInterGodownDetails = _InwardInterGodownService.GetDetailsById(id);
            model.InwardInterGodownItemList = _InwardInterGodownItemService.GetItemListByInwardCode(model.InwardInterGodownDetails.InwardCode);
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardInterGodownPrint()
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
                            Session["LOGINSHOPGODOWNCODEIWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWGG"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWGG"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWGG"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetInwardNumbersInterGodown(string term)
        {
            string godowncode = Session["LOGINSHOPGODOWNCODEIWGG"] as string;
            var result = _InwardInterGodownService.GetInwardNo(term, godowncode);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.InwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardInterGodownDetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardInterGodownDetails = _InwardInterGodownService.GetDetailsByInwardCode(InwardNo);
            model.InwardInterGodownItemList = _InwardInterGodownItemService.GetItemListByInwardCode(InwardNo);
            return View(model);
        }


        /********************************* Inward Inter Godown End *********************/

        /********************************* Inward Inter Shop ***************************/

        [HttpGet]
        public ActionResult InwardInterShop()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            model.InwardInterShopDetails = new InwardInterShop();

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
                            Session["LOGINSHOPGODOWNCODEIWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }

            string shopcodes = Session["LOGINSHOPGODOWNCODEIWSS"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardInterShopService.GetLastInwardByFinYear(finyr, shopcodes);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string shopname = Session["SHOPGODOWNNAMEIWSS"] as string;
            var shopdetails = _ShopService.GetShopDetailsByName(shopname);
            string utilityname = shopdetails.ShortCode + "/IWSS";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.InwardInterShopDetails.InwardCode = inwardcode;
            TempData["PreviousInterShopInward"] = inwardcode;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOutwardNoInterShop(string term)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODEIWSS"] as string;
            var details = _OutwardInterShopService.GetOutwardNo(term, shopcode);
            List<string> names = new List<string>();
            foreach (var item in details)
            {
                names.Add(item.OutwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalesmanDetails(string ShopCode)
        {
            var details = _ShopService.GetShopByCode(ShopCode);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOutwardInterShopDetails(string Outwardno)
        {
            var details = _OutwardInterShopService.GetDetailsByOutwardCode(Outwardno);
            var shopdetails = _ShopService.GetShopByCode(details.FromShopCode);
            return Json(new { details, shopdetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemListInterShop(string OutwardNo)
        {
            MainApplication model = new MainApplication();
            model.OutwardItemInterShopList = _OutwardInterShopItemService.GetDetailsByOutwardCode(OutwardNo);
            TempData["OutwardInterShopItems"] = model.OutwardItemInterShopList;
            return View(model);
        }

        [HttpPost]
        public ActionResult InwardInterShop(MainApplication model, FormCollection form)
        {
            model.InwardInterShopItemDetails = new InwardInterShopItem();

            string shopcode = Session["LOGINSHOPGODOWNCODEIWSS"] as string;
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string finyr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var lastinward = _InwardInterShopService.GetLastInwardByFinYear(finyr, shopcode);
            string inwardcode = string.Empty;
            int inwardVal = 0;
            int length = 0;
            if (lastinward != null)
            {
                string trim = lastinward.InwardCode;
                int index = trim.LastIndexOf('I');
                inwardcode = lastinward.InwardCode.Substring(index + 4, 6);
                length = (Convert.ToInt32(inwardcode) + 1).ToString().Length;
                inwardVal = Convert.ToInt32(inwardcode) + 1;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string shopnames = Session["SHOPGODOWNNAMEIWSS"] as string;
            var shopdetails = _ShopService.GetShopDetailsByName(shopnames);
            string utilityname = shopdetails.ShortCode + "/IWSS";
            inwardcode = _utilityService.getName(utilityname, length, inwardVal);
            inwardcode = inwardcode + finyr;
            model.InwardInterShopDetails.InwardCode = inwardcode;

            model.InwardInterShopDetails.Date = DateTime.Now;
            model.InwardInterShopDetails.Status = "Active";
            model.InwardInterShopDetails.ModifiedOn = DateTime.Now;
            _InwardInterShopService.Create(model.InwardInterShopDetails);

            var details = _OutwardInterShopService.GetDetailsByOutwardCode(model.InwardInterShopDetails.OutwardCode);
            details.Status = "InActive";
            details.ModifiedOn = DateTime.Now;
            _OutwardInterShopService.Update(details);

            var itemlist = TempData["OutwardInterShopItems"] as IEnumerable<OutwardItemInterShop>;
            foreach (var item in itemlist)
            {
                InwardInterShopItem inwarditem = new InwardInterShopItem();
                inwarditem.Color = item.ColorCode;
                inwarditem.DesignName = item.DesignName;
                inwarditem.ItemDescription = item.Description;
                inwarditem.ItemCode = item.ItemCode;
                inwarditem.ItemName = item.ItemName;
                item.ToShopCode = model.InwardInterShopDetails.ToShopCode;
                item.FromShopCode = model.InwardInterShopDetails.FromShopCode;
                inwarditem.Material = item.TypeOfMaterial;
                inwarditem.Quantity = Convert.ToDouble(item.QuantityTransfer);
                inwarditem.SellingPrice = item.SellingPrice;
                inwarditem.MRP = item.MRP;
                inwarditem.InwardCode = model.InwardInterShopDetails.InwardCode;
                inwarditem.FromShopCode = model.InwardInterShopDetails.FromShopCode;
                inwarditem.ToShopCode = model.InwardInterShopDetails.ToShopCode;
                inwarditem.Status = "Status";
                inwarditem.ModifiedOn = DateTime.Now;
                _InwardInterShopItemService.Create(inwarditem);

                var stockdistitems = _stockitemdistributionservice.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.InwardInterShopDetails.ToShopCode);
                var shopstockitems = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, model.InwardInterShopDetails.ToShopCode);

                if (shopstockitems != null)
                {
                    shopstockitems.Quantity += Convert.ToDouble(item.QuantityTransfer);
                    shopstockitems.ModifiedOn = DateTime.Now;
                    _ShopStockService.Update(shopstockitems);
                }
                else
                {
                    ShopStock shopstock = new ShopStock();
                    shopstock.Brand = item.Brand;
                    shopstock.DesignCode = item.DesignCode;
                    shopstock.NumberType = item.NumberType;
                    shopstock.ShopCode = item.ToShopCode;
                    string shopname = _ShopService.GetShopByCode(item.ToShopCode).ShopName;
                    shopstock.ShopName = shopname;
                    shopstock.Size = item.Size;
                    shopstock.Unit = item.Unit;
                    shopstock.Barcode = item.Barcode;
                    shopstock.Color = item.ColorCode;
                    shopstock.Description = item.Description;
                    shopstock.DesignName = item.DesignName;
                    shopstock.SellingPrice = item.SellingPrice;
                    shopstock.MRP = item.MRP;
                    shopstock.Quantity = Convert.ToDouble(item.QuantityTransfer);
                    shopstock.ItemCode = item.ItemCode;
                    shopstock.ItemName = item.ItemName;
                    shopstock.Material = item.TypeOfMaterial;
                    shopstock.Status = "Active";
                    shopstock.ModifiedOn = DateTime.Now;
                    _ShopStockService.Create(shopstock);
                }

                if (stockdistitems != null)
                {
                    stockdistitems.ItemQuantity += Convert.ToDouble(item.QuantityTransfer);
                    stockdistitems.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Update(stockdistitems);
                }
                else
                {
                    StockItemDistribution stkitemdist = new StockItemDistribution();
                    stkitemdist.Brand = item.Brand;
                    stkitemdist.DesignCode = item.DesignCode;
                    stkitemdist.Size = item.Size;
                    stkitemdist.NumberType = item.NumberType;
                    stkitemdist.Barcode = item.Barcode;
                    stkitemdist.Color = item.ColorCode;
                    stkitemdist.Code = model.InwardInterShopDetails.ToShopCode;
                    stkitemdist.Description = item.Description;
                    stkitemdist.DesignName = item.DesignName;
                    string shopname = _ShopService.GetShopByCode(item.ToShopCode).ShopName;
                    stkitemdist.GodownShopName = shopname;
                    stkitemdist.ItemCode = item.ItemCode;
                    stkitemdist.ItemName = item.ItemName;
                    stkitemdist.ItemQuantity = Convert.ToDouble(item.QuantityTransfer);
                    stkitemdist.Material = item.TypeOfMaterial;
                    stkitemdist.SellingPrice = item.SellingPrice;
                    stkitemdist.MRP = item.MRP;
                    stkitemdist.StockDistributionCode = model.InwardInterShopDetails.OutwardCode;
                    stkitemdist.Unit = item.Unit;
                    stkitemdist.Status = "Active";
                    stkitemdist.ModifiedOn = DateTime.Now;
                    _stockitemdistributionservice.Create(stkitemdist);
                }
            }

            var detals = _InwardInterShopService.GetLastInward();
            string inwardid = Encode(detals.Id.ToString());
            return RedirectToAction("InwardInterShopDetails/" + inwardid, "Inward");
        }

        [HttpGet]
        public ActionResult InwardInterShopDetails(string id)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.InwardInterShopDetails = _InwardInterShopService.GetDetailsById(Id);
            model.InwardInterShopItemList = _InwardInterShopItemService.GetItemListByInwardCode(model.InwardInterShopDetails.InwardCode);

            string previousinwardno = TempData["PreviousInterShopInward"].ToString();
            if (previousinwardno != model.InwardInterShopDetails.InwardCode)
            {
                ViewData["InterShopInwardChanged"] = previousinwardno + " is replaced with " + model.InwardInterShopDetails.InwardCode + " because " + previousinwardno + " is acquired by another person";
            }
            TempData["PreviousInterShopInward"] = previousinwardno;
            return View(model);
        }

        public ActionResult PrintInwardInterShop(string id)
        {
            MainApplication model = new MainApplication();
            int Id = Decode(id);
            model.InwardInterShopDetails = _InwardInterShopService.GetDetailsById(Id);
            model.InwardInterShopItemList = _InwardInterShopItemService.GetItemListByInwardCode(model.InwardInterShopDetails.InwardCode);
            return View(model);
        }


        [HttpGet]
        public ActionResult InwardInterShopPrint()
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
                            Session["LOGINSHOPGODOWNCODEIWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSS"] = assigndetails.Modules;
                        }
                        else
                        {
                            Session["LOGINSHOPGODOWNCODEIWSS"] = assigndetails.AssignRightsCode;
                            Session["SHOPGODOWNNAMEIWSS"] = assigndetails.Modules;
                        }
                        break;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult GetInwardNumbersInterShop(string term)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODEIWSS"] as string;
            var result = _InwardInterShopService.GetInwardNo(term, shopcode);
            List<string> names = new List<string>();
            foreach (var data in result)
            {
                names.Add(data.InwardCode);
            }
            return Json(names, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetInwardInterShopDetailsForPrint(string InwardNo)
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.InwardInterShopDetails = _InwardInterShopService.GetDetailsByInwardNo(InwardNo);
            model.InwardInterShopItemList = _InwardInterShopItemService.GetItemListByInwardCode(InwardNo);
            return View(model);
        }

        /********************************* Inward Inter Shop End ***************************/
    }
}