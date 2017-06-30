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

namespace MvcRetailApp.Controllers
{
    [NoDirectAccess]
    [SessionExpireFilter]
    public class PurchaseReturnController : Controller
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

        private readonly IPurchaseReturnService _PurchaseReturnService;
        private readonly IPurchaseReturnItemService _PurchaseReturnItemService;
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IInwardFromShopToGodownService _InwardFromShopToGodownService;
        private readonly IInwardItemFromShopToGodownService _InwardItemFromShopToGodownService;
        private readonly IUserCredentialService _UserCredentialService;
        private readonly IModuleService _ModuleService;
        private readonly IUtilityService _UtilityService;
        private readonly IShopStockService _ShopStockService;
        private readonly IGodownStockService _GodownStockService;
        private readonly IOpeningStockService _OpeningStockService;
        private readonly IEntryStockItemService _EntryStockItemService;
        private readonly IGodownService _GodownService;
        private readonly IStockItemDistributionService _StockItemDistributionService;
        private readonly IOutwardShopToGodownService _OutwardShopToGodownService;
        private readonly IOutwardItemShopToGodownService _OutwardItemShopToGodownService;
        private readonly ISuppliersMasterService _SuppliersMasterService;
        private readonly IEmployeeMasterService _EmployeeMasterService;
        private readonly IDebitNoteService _DebitNoteService;
        private readonly IDebitNoteItemService _DebitNoteItemService;
        private readonly IShopService _ShopService;
        private readonly IPurchaseInventoryTaxService _PurchaseInventoryTaxService;
        public PurchaseReturnController(IPurchaseReturnService PurchaseReturnService, IPurchaseReturnItemService PurchaseReturnItemService, IInwardFromSupplierService InwardFromSupplierService, IInwardItemFromSupplierService InwardItemFromSupplierService, IInwardFromShopToGodownService InwardFromShopToGodownService, IInwardItemFromShopToGodownService InwardItemFromShopToGodownService, IUserCredentialService UserCredentialService, IModuleService ModuleService,
            IUtilityService UtilityService, IShopStockService ShopStockService, IGodownStockService GodownStockService, IOpeningStockService OpeningStockService, IEntryStockItemService EntryStockItemService, IGodownService GodownService, IStockItemDistributionService StockItemDistributionService, IOutwardShopToGodownService OutwardShopToGodownService, IOutwardItemShopToGodownService OutwardItemShopToGodownService, ISuppliersMasterService SuppliersMasterService,
            IEmployeeMasterService EmployeeMasterService, IPurchaseInventoryTaxService PurchaseInventoryTaxService, IDebitNoteService DebitNoteService, IDebitNoteItemService DebitNoteItemService, IShopService ShopService)
        {
            this._PurchaseReturnService = PurchaseReturnService;
            this._PurchaseReturnItemService = PurchaseReturnItemService;
            this._InwardFromSupplierService = InwardFromSupplierService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._InwardFromShopToGodownService = InwardFromShopToGodownService;
            this._InwardItemFromShopToGodownService = InwardItemFromShopToGodownService;
            this._ModuleService = ModuleService;
            this._UserCredentialService = UserCredentialService;
            this._UtilityService = UtilityService;
            this._ShopStockService = ShopStockService;
            this._GodownStockService = GodownStockService;
            this._OpeningStockService = OpeningStockService;
            this._EntryStockItemService = EntryStockItemService;
            this._GodownService = GodownService;
            this._StockItemDistributionService = StockItemDistributionService;
            this._OutwardShopToGodownService = OutwardShopToGodownService;
            this._OutwardItemShopToGodownService = OutwardItemShopToGodownService;
            this._SuppliersMasterService = SuppliersMasterService;
            this._EmployeeMasterService = EmployeeMasterService;
            this._PurchaseInventoryTaxService = PurchaseInventoryTaxService;
            this._DebitNoteService = DebitNoteService;
            this._DebitNoteItemService = DebitNoteItemService;
            this._ShopService = ShopService;
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
        public ActionResult PurchaseReturn()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            var username = HttpContext.Session["UserName"].ToString();
            if (username != "SuperAdmin")
            {
                int modulelastcount = _ModuleService.GetLastRow().Id;
                for (int i = 96; i <= modulelastcount; i++)
                {
                    var assigndetails = _UserCredentialService.GetDetailsByEmailStatusAndModuleId(UserEmail, i);
                    if (assigndetails != null)
                    {
                        Session["LOGINSHOPGODOWNCODE"] = assigndetails.AssignRightsCode;
                        Session["SHOPGODOWNNAME"] = assigndetails.Modules;
                        break;
                    }
                }
            }

            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastPurchaseReturn = _PurchaseReturnService.GetLastPurchaseByFinYr(FinYr, shopcode);
            string PurchaseReturnCode = string.Empty;
            int PurchaseReturnLength = 0;
            int PurchaseReturnNo = 0;
            if (LastPurchaseReturn == null)
            {
                PurchaseReturnLength = 1;
                PurchaseReturnNo = 1;
            }
            else
            {
                int PurchaseReturnIndex = LastPurchaseReturn.PurchaseReturnNo.LastIndexOf('P');
                PurchaseReturnCode = LastPurchaseReturn.PurchaseReturnNo.Substring(PurchaseReturnIndex + 2, 6);
                PurchaseReturnLength = (Convert.ToInt32(PurchaseReturnCode) + 1).ToString().Length;
                PurchaseReturnNo = Convert.ToInt32(PurchaseReturnCode) + 1;
            }

            string name = Session["SHOPGODOWNNAME"] as string;
            string utilityname = string.Empty;
            if (shopcode.Contains("SH"))
            {
                var details = _ShopService.GetShopDetailsByName(name);
                utilityname = details.ShortCode + "/PR";
            }
            else
            {
                var details = _GodownService.GetGodownDetailsByName(name);
                utilityname = details.ShortCode + "/PR";
            }
            PurchaseReturnCode = _UtilityService.getName(utilityname, PurchaseReturnLength, PurchaseReturnNo);
            PurchaseReturnCode = PurchaseReturnCode + FinYr;
            model.PurchaseReturnNo = PurchaseReturnCode;
            TempData["PreviousPurchaseReturnNo"] = PurchaseReturnCode;

            return View(model);
        }

        [HttpGet]
        public JsonResult GetSupplierDetails(string inwardno)
        {
            var data = _InwardFromSupplierService.GetDetailsByPINo(inwardno);
            return Json(new
            {
                data.SupplierName,
                data.SupplierContactNo,
                data.PoNo,
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSupplierBillNos(string SearchTerm, string name)
        {
            var BillList = _InwardFromSupplierService.GetSupplierBillNos(name, SearchTerm);
            List<string> No = new List<string>();
            foreach (var data in BillList)
            {
                if (data.BillNo != null && data.BillNo != "")
                {
                    No.Add(data.BillNo);
                }
            }
            return Json(No, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSupplierChallanNos(string SearchTerm, string name)
        {
            var ChallanList = _InwardFromSupplierService.GetSupplierChallanNos(name, SearchTerm);
            List<string> No = new List<string>();
            foreach (var data in ChallanList)
            {
                if (data.ChallanNo != null && data.ChallanNo != "")
                {
                    No.Add(data.ChallanNo);
                }
            }
            return Json(No, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInwardDetails(string no)
        {
            var data = _InwardFromSupplierService.GetDetailsByBillNoOrChallanNo(no);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //GET LOGIN EMPLOPYEE DETAILS BY EMPLOYEE EMAIL
        [HttpGet]
        public JsonResult GetPreparedByEmpDetails(string email)
        {
            var data = _EmployeeMasterService.getEmpByEmail(email);
            return Json(new { data.Name, data.MobileNo }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetReturnItems(string inwardno)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierDetails = _InwardFromSupplierService.GetDetailsByPINo(inwardno);
            model.InwardItemsFromSupplierList = _InwardItemFromSupplierService.GetItemsByPINo(inwardno);
            List<InwardItemsFromSupplier> InwardItemList = new List<InwardItemsFromSupplier>();
            foreach (var item in model.InwardItemsFromSupplierList)
            {
                if (item.PurchaseReturn == "No")
                {
                    InwardItemList.Add(item);
                }
            }
            model.InwardItemsFromSupplierList = InwardItemList;
            model.PurchaseInventoryTaxList = _PurchaseInventoryTaxService.GetTaxesByCode(inwardno);
            model.PurchaseReturnItemList = new List<PurchaseReturnItem>();
            var LastPurchaseReturn = _PurchaseReturnService.GetLastReturnBillOfSupplier(inwardno);
            if (LastPurchaseReturn != null)
            {
                model.PurchaseReturnDetails = LastPurchaseReturn;
                model.PurchaseReturnItemList = _PurchaseReturnItemService.GetReturnItemsByReturnNo(LastPurchaseReturn.PurchaseReturnNo);
            }
            TempData["InwardItemList"] = model.InwardItemsFromSupplierList;
            TempData["PurchaseReturnList"] = model.PurchaseReturnItemList;
            return View(model);
        }

        [HttpPost]
        public ActionResult PurchaseReturn(MainApplication model, FormCollection frmcol)
        {
            string shopcode = Session["LOGINSHOPGODOWNCODE"].ToString();
            string shopname = Session["SHOPGODOWNNAME"].ToString();

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);
            var LastPurchaseReturn = _PurchaseReturnService.GetLastPurchaseByFinYr(FinYr, shopcode);
            string PurchaseReturnCode = string.Empty;
            int PurchaseReturnLength = 0;
            int PurchaseReturnNo = 0;
            if (LastPurchaseReturn == null)
            {
                PurchaseReturnLength = 1;
                PurchaseReturnNo = 1;
            }
            else
            {
                int PurchaseReturnIndex = LastPurchaseReturn.PurchaseReturnNo.LastIndexOf('P');
                PurchaseReturnCode = LastPurchaseReturn.PurchaseReturnNo.Substring(PurchaseReturnIndex + 2, 6);
                PurchaseReturnLength = (Convert.ToInt32(PurchaseReturnCode) + 1).ToString().Length;
                PurchaseReturnNo = Convert.ToInt32(PurchaseReturnCode) + 1;
            }

            string ShortCode = string.Empty;
            if (shopcode.Contains("SH"))
            {
                var detail = _ShopService.GetShopDetailsByName(shopname);
                ShortCode = detail.ShortCode;
            }
            else
            {
                var detail = _GodownService.GetGodownDetailsByName(shopname);
                ShortCode = detail.ShortCode;
            }
            PurchaseReturnCode = _UtilityService.getName(ShortCode + "/PR", PurchaseReturnLength, PurchaseReturnNo);
            PurchaseReturnCode = PurchaseReturnCode + FinYr;
            string DebitNoteNo = _UtilityService.getName(ShortCode + "/DBN", PurchaseReturnLength, PurchaseReturnNo);
            DebitNoteNo = DebitNoteNo + FinYr;
            model.PurchaseReturnDetails.Code = Session["LOGINSHOPGODOWNCODE"].ToString();
            model.PurchaseReturnDetails.PurchaseReturnNo = PurchaseReturnCode;
            model.PurchaseReturnDetails.PurchaseReturnDate = DateTime.Now;
            model.PurchaseReturnDetails.DebitNoteNo = DebitNoteNo;
            model.PurchaseReturnDetails.DebitNoteDate = DateTime.Now;
            if (model.PurchaseReturnDetails.PackAndForwd == null)
            {
                model.PurchaseReturnDetails.PackAndForwd = 0;
            }
            model.PurchaseReturnDetails.DebitNoteAmount = Convert.ToDouble(frmcol["SupplierAmount"]);
            model.PurchaseReturnDetails.TotalAmount = Convert.ToDouble(frmcol["TotalAmountValue"]);
            model.PurchaseReturnDetails.TotalTaxAmount = Convert.ToDouble(frmcol["TotalTaxAmountValue"]);
            model.PurchaseReturnDetails.GrandTotal = Convert.ToDouble(frmcol["GrandTotalValue"]);
            model.PurchaseReturnDetails.Status = "Active";
            model.PurchaseReturnDetails.ModifiedOn = DateTime.Now;
            _PurchaseReturnService.Create(model.PurchaseReturnDetails);

            DebitNote DebitNote = new DebitNote();
            DebitNote.DebitNoteNo = DebitNoteNo;
            DebitNote.DebitNoteDate = DateTime.Now;
            DebitNote.SupplierName = model.PurchaseReturnDetails.SupplierName;
            DebitNote.BillNo = model.PurchaseReturnDetails.SupplierBillNo;
            DebitNote.ChallanNo = model.PurchaseReturnDetails.SupplierChallanNo;
            DebitNote.InwardNo = model.PurchaseReturnDetails.InwardNo;
            DebitNote.PurchaseReturnNo = PurchaseReturnCode;
            DebitNote.Amount = Convert.ToDouble(frmcol["SupplierAmount"]);
            DebitNote.Status = "Active";
            DebitNote.ModifiedOn = DateTime.Now;
            _DebitNoteService.Create(DebitNote);

            var Inwardreturnlist = TempData["InwardItemList"] as IEnumerable<InwardItemsFromSupplier>;
            int count = 1;
            foreach (var item in Inwardreturnlist)
            {
                string checkbox = "CheckBox" + count;
                string quantity = "quantity" + count;
                string amount = "amountvalue" + count;
                string prevqty = "prevquantityvalue" + count;
                if (frmcol[checkbox] == "Yes")
                {
                    PurchaseReturnItem PurchaseReturnItem = new PurchaseReturnItem();
                    PurchaseReturnItem.PurchaseReturnNo = PurchaseReturnCode;
                    PurchaseReturnItem.Barcode = item.Barcode;
                    PurchaseReturnItem.ItemCode = item.itemCode;
                    PurchaseReturnItem.ItemName = item.Item;
                    PurchaseReturnItem.ItemType = item.ItemType;
                    PurchaseReturnItem.Description = item.Description;
                    PurchaseReturnItem.DesignCode = item.Design;
                    PurchaseReturnItem.DesignName = item.DesignName;
                    PurchaseReturnItem.Color = item.Color;
                    PurchaseReturnItem.Material = item.Material;
                    PurchaseReturnItem.Brand = item.Brand;
                    PurchaseReturnItem.Size = item.Size;
                    PurchaseReturnItem.Unit = item.Unit;
                    PurchaseReturnItem.NumberType = item.NumberType;
                    PurchaseReturnItem.Quantity = Convert.ToDouble(frmcol[quantity]);
                    PurchaseReturnItem.Balance = Convert.ToDouble(frmcol[prevqty]) - Convert.ToDouble(frmcol[quantity]);
                    PurchaseReturnItem.CostPrice = item.CostPrice;
                    PurchaseReturnItem.DisPer = item.Discount.ToString();
                    PurchaseReturnItem.SellingPrice = item.SellingPrice;
                    PurchaseReturnItem.MRP = item.MRP;
                    PurchaseReturnItem.ItemTax = item.ItemTax;
                    PurchaseReturnItem.Amount = Convert.ToDouble(frmcol[amount]);
                    PurchaseReturnItem.Status = "Active";
                    PurchaseReturnItem.ModifiedOn = DateTime.Now;
                    _PurchaseReturnItemService.Create(PurchaseReturnItem);

                    var inwarditem = _InwardItemFromSupplierService.GetItemByInwardNoAndItemCode(model.PurchaseReturnDetails.InwardNo, PurchaseReturnItem.ItemCode);
                    inwarditem.PurchaseReturn = "Yes";
                    _InwardItemFromSupplierService.Update(inwarditem);

                    DebitNoteItem DebitNoteItem = new DebitNoteItem();
                    DebitNoteItem.DebitNoteNo = DebitNoteNo;
                    DebitNoteItem.ItemCode = item.itemCode;
                    DebitNoteItem.ItemName = item.Item;
                    DebitNoteItem.Description = item.Description;
                    DebitNoteItem.ItemType = item.ItemType;
                    DebitNoteItem.Color = item.Color;
                    DebitNoteItem.Material = item.Material;
                    DebitNoteItem.DesignName = item.DesignName;
                    DebitNoteItem.Brand = item.Brand;
                    DebitNoteItem.Size = item.Size;
                    DebitNoteItem.Quantity = Convert.ToDouble(frmcol[quantity]);
                    DebitNoteItem.Unit = item.Unit;
                    DebitNoteItem.CostPrice = item.CostPrice;
                    DebitNoteItem.NumberType = item.NumberType;
                    DebitNoteItem.SellingPrice = item.SellingPrice;
                    DebitNoteItem.MRP = item.MRP;
                    DebitNoteItem.ItemTax = item.ItemTax;
                    DebitNoteItem.Amount = Convert.ToDouble(frmcol[amount]);
                    DebitNoteItem.Status = "Active";
                    DebitNoteItem.ModifiedOn = DateTime.Now;
                    _DebitNoteItemService.Create(DebitNoteItem);

                    var iteminward = _InwardItemFromSupplierService.GetItemByInwardNoAndItemCode(model.PurchaseReturnDetails.InwardNo, item.itemCode);
                    iteminward.PurchaseReturn = "Yes";
                    _InwardItemFromSupplierService.Update(iteminward);

                    string code = Session["LOGINSHOPGODOWNCODE"].ToString();
                    if (item.ItemType == "Inventory")
                    {
                        if (code.Contains("SH"))
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.itemCode, code);
                            shopdetails.Quantity = shopdetails.Quantity - PurchaseReturnItem.Quantity;
                            _ShopStockService.Update(shopdetails);

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.itemCode, code);
                            stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity - PurchaseReturnItem.Quantity;
                            _StockItemDistributionService.Update(stockitemdetails);
                        }
                        else
                        {
                            var godowndetails = _GodownStockService.GetDetailsByItemCodeAndGodownCode(item.itemCode, code);
                            godowndetails.Quantity = godowndetails.Quantity - PurchaseReturnItem.Quantity;
                            _GodownStockService.Update(godowndetails);

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndGodownCode(item.itemCode, code);
                            stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity - PurchaseReturnItem.Quantity;
                            _StockItemDistributionService.Update(stockitemdetails);
                        }

                        var entrystockitems = _EntryStockItemService.getDetailsByItemCode(item.itemCode);
                        if (entrystockitems != null)
                        {
                            entrystockitems.TotalQuantity = entrystockitems.TotalQuantity - PurchaseReturnItem.Quantity;
                            _EntryStockItemService.Update(entrystockitems);
                        }
                        else
                        {
                            var openingstockitems = _OpeningStockService.GetDetailsByItemCode(item.itemCode);
                            openingstockitems.TotalQuantity = openingstockitems.TotalQuantity - PurchaseReturnItem.Quantity;
                            _OpeningStockService.UpdateStock(openingstockitems);
                        }
                    }
                }
                count++;
            }
            count = Inwardreturnlist.Count() + 1;
            var purchasereturnlist = TempData["PurchaseReturnList"] as IEnumerable<PurchaseReturnItem>;
            foreach (var item in purchasereturnlist)
            {
                string checkbox = "CheckBox" + count;
                string quantity = "quantity" + count;
                string amount = "amountvalue" + count;
                string prevqty = "prevquantityvalue" + count;
                if (frmcol[checkbox] == "Yes")
                {
                    item.PurchaseReturnNo = PurchaseReturnCode;
                    item.Quantity = Convert.ToDouble(frmcol[quantity]);
                    item.Balance = Convert.ToDouble(frmcol[prevqty]) - Convert.ToDouble(frmcol[quantity]);
                    item.Amount = Convert.ToDouble(frmcol[amount]);
                    item.Status = "Active";
                    item.ModifiedOn = DateTime.Now;
                    _PurchaseReturnItemService.Create(item);

                    DebitNoteItem DebitNoteItem = new DebitNoteItem();
                    DebitNoteItem.DebitNoteNo = DebitNoteNo;
                    DebitNoteItem.ItemCode = item.ItemCode;
                    DebitNoteItem.ItemName = item.ItemName;
                    DebitNoteItem.Description = item.Description;
                    DebitNoteItem.ItemType = item.ItemType;
                    DebitNoteItem.Color = item.Color;
                    DebitNoteItem.Material = item.Material;
                    DebitNoteItem.DesignName = item.DesignName;
                    DebitNoteItem.Brand = item.Brand;
                    DebitNoteItem.Size = item.Size;
                    DebitNoteItem.Quantity = Convert.ToDouble(frmcol[quantity]);
                    DebitNoteItem.Unit = item.Unit;
                    DebitNoteItem.NumberType = item.NumberType;
                    DebitNoteItem.CostPrice = item.CostPrice;
                    DebitNoteItem.SellingPrice = item.SellingPrice;
                    DebitNoteItem.MRP = item.MRP;
                    DebitNoteItem.ItemTax = item.ItemTax;
                    DebitNoteItem.Amount = Convert.ToDouble(frmcol[amount]);
                    DebitNoteItem.Status = "Active";
                    DebitNoteItem.ModifiedOn = DateTime.Now;
                    _DebitNoteItemService.Create(DebitNoteItem);

                    string code = Session["LOGINSHOPGODOWNCODE"].ToString();
                    if (item.ItemType == "Inventory")
                    {
                        if (code.Contains("SH"))
                        {
                            var shopdetails = _ShopStockService.GetDetailsByItemCodeAndShopCode(item.ItemCode, code);
                            shopdetails.Quantity = shopdetails.Quantity - item.Quantity;
                            _ShopStockService.Update(shopdetails);

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndShopCode(item.ItemCode, code);
                            stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity - item.Quantity;
                            _StockItemDistributionService.Update(stockitemdetails);
                        }
                        else
                        {
                            var godowndetails = _GodownStockService.GetDetailsByItemCodeAndGodownCode(item.ItemCode, code);
                            godowndetails.Quantity = godowndetails.Quantity - item.Quantity;
                            _GodownStockService.Update(godowndetails);

                            var stockitemdetails = _StockItemDistributionService.GetDetailsByItemCodeAndGodownCode(item.ItemCode, code);
                            stockitemdetails.ItemQuantity = stockitemdetails.ItemQuantity - item.Quantity;
                            _StockItemDistributionService.Update(stockitemdetails);
                        }

                        var entrystockitems = _EntryStockItemService.getDetailsByItemCode(item.ItemCode);
                        if (entrystockitems != null)
                        {
                            entrystockitems.TotalQuantity = entrystockitems.TotalQuantity - item.Quantity;
                            _EntryStockItemService.Update(entrystockitems);
                        }
                        else
                        {
                            var openingstockitems = _OpeningStockService.GetDetailsByItemCode(item.ItemCode);
                            openingstockitems.TotalQuantity = openingstockitems.TotalQuantity - item.Quantity;
                            _OpeningStockService.UpdateStock(openingstockitems);
                        }
                    }
                }
                else
                {
                    item.PurchaseReturnNo = PurchaseReturnCode;
                    item.Quantity = 0;
                    _PurchaseReturnItemService.Create(item);
                }
                count++;
            }
            var data = _InwardFromSupplierService.GetInwardByInwardNo(model.PurchaseReturnDetails.InwardNo);
            data.PurchaseReturn = "Yes";
            if (data.PurchaseReturnNo == null)
            {
                data.PurchaseReturnNo = PurchaseReturnCode;
            }
            else
            {
                data.PurchaseReturnNo = data.PurchaseReturnNo + "," + PurchaseReturnCode;
            }
            _InwardFromSupplierService.UpdateInward(data);

            //Store total tax and total amount of tax of PO
            int itemtaxcount = Convert.ToInt32(frmcol["ReturnItemTaxCount"]);
            model.PurchaseInventoryTaxDetails = new PurchaseInventoryTax();
            for (int i = 1; i <= itemtaxcount; i++)
            {
                string taxnumber = "ReturnItemTaxNumber" + i;
                string taxamount = "ReturnAddedTaxAmounthdn" + i;
                string amountontax = "ReturnAddedAmounthdn" + i;
                if (Convert.ToDouble(frmcol[taxamount]) != 0)
                {
                    model.PurchaseInventoryTaxDetails.Code = PurchaseReturnCode;
                    model.PurchaseInventoryTaxDetails.Amount = frmcol[amountontax];
                    model.PurchaseInventoryTaxDetails.Tax = frmcol[taxnumber];
                    model.PurchaseInventoryTaxDetails.TaxAmount = frmcol[taxamount];
                    _PurchaseInventoryTaxService.Create(model.PurchaseInventoryTaxDetails);
                }
            }

            var details = _PurchaseReturnService.GetPurchaseByReturnNo(PurchaseReturnCode);
            string PRId = Encode(details.Id.ToString());
            return RedirectToAction("PurchaseReturnDetails/" + PRId);
        }

        [HttpGet]
        public ActionResult DebitNote(string id)
        {
            int Id = Decode(id);
            MainApplication model = new MainApplication();
            model.PurchaseReturnDetails = _PurchaseReturnService.GetDetailsById(Id);
            model.PurchaseReturnItemList = _PurchaseReturnItemService.GetItemsByPurchaseReturnNo(model.PurchaseReturnDetails.PurchaseReturnNo);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSupplierContact(string name)
        {
            var supplierdate = _SuppliersMasterService.GetByName(name);
            return Json(supplierdate.ContactNo1, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PurchaseReturnDetails(string id)
        {
            MainApplication model = new MainApplication()
            {
                PurchaseReturnDetails = new PurchaseReturn(),
            };
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            int Id = Decode(id);
            model.PurchaseReturnDetails = _PurchaseReturnService.GetDetailsById(Id);
            model.PurchaseReturnItemList = _PurchaseReturnItemService.GetItemsByPurchaseReturnNo(model.PurchaseReturnDetails.PurchaseReturnNo);
            string previouspurchasereturn = TempData["PreviousPurchaseReturnNo"].ToString();
            if (previouspurchasereturn != model.PurchaseReturnDetails.PurchaseReturnNo)
            {
                ViewData["PurchaseReturnNoChanged"] = previouspurchasereturn + " is replaced with " + model.PurchaseReturnDetails.PurchaseReturnNo + " because " + previouspurchasereturn + " is acquired by another person";
            }
            TempData["PreviousPurchaseReturnNo"] = previouspurchasereturn;
            return View(model);
        }

        [HttpGet]
        public ActionResult PrintPurchaseReturn(string id)
        {
            MainApplication model = new MainApplication()
            {
                PurchaseReturnDetails = new PurchaseReturn(),
            };
            int Id = Decode(id);
            model.PurchaseReturnDetails = _PurchaseReturnService.GetDetailsById(Id);
            model.PurchaseReturnItemList = _PurchaseReturnItemService.GetItemsByPurchaseReturnNo(model.PurchaseReturnDetails.PurchaseReturnNo);
            return View(model);
        }

        [HttpGet]
        public ActionResult PurchaseReturnPrint()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _UserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _ModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetPurchaseReturnNos(string term)
        {
            var prnolist = _PurchaseReturnService.GetAllPurchaseReturns(term);
            List<string> codes = new List<string>();
            foreach (var data in prnolist)
            {
                codes.Add(data.PurchaseReturnNo);
            }
            return Json(codes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPurchaseReturnDetails(string prno)
        {
            MainApplication model = new MainApplication();
            model.PurchaseReturnDetails = _PurchaseReturnService.GetPurchaseByReturnNo(prno);
            model.PurchaseReturnItemList = _PurchaseReturnItemService.GetItemsByPurchaseReturnNo(model.PurchaseReturnDetails.PurchaseReturnNo);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetStockQuantity(string itemcode, string code)
        {
            double? Qty = 0;
            if (code.Contains("SH"))
            {
                var shopstock = _ShopStockService.GetDetailsByItemCodeAndShopCode(itemcode, code);
                if (shopstock != null)
                {
                    Qty = shopstock.Quantity;
                }
            }
            else
            {
                var godownstock = _GodownStockService.GetDetailsByItemCodeAndGodownCode(itemcode, code);
                if (godownstock != null)
                {
                    Qty = godownstock.Quantity;
                }
            }
            return Json(Qty, JsonRequestBehavior.AllowGet);
        }
    }
}
