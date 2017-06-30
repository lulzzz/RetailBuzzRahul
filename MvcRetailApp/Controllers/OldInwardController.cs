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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcRetailApp.Controllers
{
    public class OldInwardController : Controller
    {
        public int userId1
        {
            get
            {
                return (int)Session["userId"];
            }
            set
            {
                Session["userId"] = value;
            }
        }

        private readonly IInwardFromSupplierService _inwardFromSupplierService;
        private readonly IPurchaseOrderDetailService _purchaseOrderDetailService;
        private readonly IPurchaseItemDetailService _purchaseItemDetailService;
        private readonly IUtilityService _utilityService;
        private readonly ISuppliersMasterService _supplierMasterService;
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;

        public OldInwardController(IInwardFromSupplierService inwardFromSupplierService, IPurchaseOrderDetailService purchaseOrderDetailService,
            IPurchaseItemDetailService purchaseItemDetailService, IUtilityService utilityservice, ISuppliersMasterService supplierMasterService, IUserCredentialService usercredentialservice, IModuleService iIModuleService)
        {
            this._inwardFromSupplierService = inwardFromSupplierService;
            this._purchaseOrderDetailService = purchaseOrderDetailService;
            this._purchaseItemDetailService = purchaseItemDetailService;
            this._utilityService = utilityservice;
            this._supplierMasterService = supplierMasterService;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
        }

        public ActionResult Inward()
        {
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.getUserCredentialListById(userId1);
            model.modulelist = _iIModuleService.getAllModules();
            return View(model);
        }

        [HttpGet]
        public ActionResult InwardFromSupplier()
        {
            MainApplication model = new MainApplication();
            var inwardData = _inwardFromSupplierService.GetLastInward();
            int inwardVal = 0;
            int length = 0;
            if (inwardData != null)
            {
                inwardVal = inwardData.InwardId;
                inwardVal = inwardVal + 1;
                length = inwardVal.ToString().Length;
            }
            else
            {
                inwardVal = 1;
                length = 1;
            }
            string InwardCode = _utilityService.getName("IW", length, inwardVal);
            model.InwardCode = InwardCode;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSupplierDetails(string name)
        {
            var inwardData = _inwardFromSupplierService.GetLastInward();
            var supplierlist = _supplierMasterService.GetByName(name);
            var PONoList = _purchaseOrderDetailService.GetPoNosBySupplier(name);
            var ponos = PONoList.Select(m => new SelectListItem
            {
                Text = m.PoNo,
                Value = m.PoNo
            });
            return Json(new
            {
                supplierlist.SupplierName,
                supplierlist.Address,
                supplierlist.ContactNo1,
                supplierlist.Email,
                supplierlist.district,
                ponos,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPODetails(string pono)
        {
            var detail = _inwardFromSupplierService.GetDetailsByPoNo(pono);
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
            else if (podetail.DelDate.Date > DateTime.Now.Date)
            {
                count = (podetail.DelDate.Date - DateTime.Now.Date).TotalDays;
                if (count != 1)
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
                msg = "From Inward";
            }
            return Json(new
            {
                DelDate,
                PODate,
                podetail.DelAt,
                remarks,
                msg
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetItemsList(string pono, string msg)
        {
            if (msg == "From PO")
            {
                var itemlist = _purchaseItemDetailService.GetPurchaseItemsByPONo(pono);
                var list = itemlist.Select(m => new PurchaseItemDetail()
                    {
                        Item = m.Item,
                        Description = m.Description,
                        Quantity = m.Quantity,
                        Unit = m.Unit,
                    });
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var inwardlist = _inwardFromSupplierService.GetDetailsByPoNo(pono);

            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}