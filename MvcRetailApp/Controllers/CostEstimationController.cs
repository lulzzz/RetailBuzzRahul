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
using System.IO;
using System.Configuration;


namespace MvcRetailApp.Controllers
{
    public class CostEstimationController : Controller
    {
        private CodeFirstContext db = new CodeFirstContext();
        private readonly ICostEstimationOfArticleService _icostestimationarticleservice;
        private readonly IUtilityService _iutiltyservice;
        private readonly ICategoryService _categoryservice;
        private readonly IItemMasterService _itemmasterservice;
        private readonly IUnitMasterService _iunitmasterservice;
        private readonly IClientMasterService _iclientservice;
        private readonly IDesignMasterService _idesignservice;
        private readonly ISuppliersMasterService _isupplierservice;
        private readonly ISizeMasterService _isizemasterservice;

        public CostEstimationController(ISizeMasterService sizemasterservice ,ISuppliersMasterService supplierservice, IDesignMasterService designservice, IClientMasterService clientservice, ICostEstimationOfArticleService costestimationarticleservice, IUtilityService utilityservice, ICategoryService categoryservice, IItemMasterService itemservice, IUnitMasterService unitmasterservice)
        {

            this._icostestimationarticleservice = costestimationarticleservice;
            this._iutiltyservice = utilityservice;
            this._categoryservice = categoryservice;
            this._itemmasterservice = itemservice;
            this._iunitmasterservice = unitmasterservice;
            this._iclientservice = clientservice;
            this._idesignservice = designservice;
            this._isupplierservice = supplierservice;
            this._isizemasterservice = sizemasterservice;
        }



        //
        // GET: /CostEstimation/

       
        //
        // GET: /CostEstimation/Create
[HttpGet]
        
        public ActionResult Create()
        {
            CostEstimationOfArticle costmaster = new CostEstimationOfArticle();
            //IEnumerable<ClientMaster> clientnames = _iclientservice.getAllClients();
            //costmaster.clientname = clientnames;
            IEnumerable<Category> categorylist = _categoryservice.getActiveCategory();
            costmaster.categorylist = categorylist;

            IEnumerable<SizeMaster> suppliername = _isizemasterservice.getallsize();
            costmaster.sizematerial = suppliername;


            IEnumerable<UnitMaster> unitname = _iunitmasterservice.getallsize();
            costmaster.itemunittype = unitname;
            CostEstimationOfArticle size = _icostestimationarticleservice.getLastInsertedClientcode();

            int catVal = 0;
            int length = 0;
            if (size != null)
            {
                catVal = size.clientId;
                catVal = catVal + 1;
                length = catVal.ToString().Length;
            }
            else
            {
                catVal = 1;
                length = 1;
            }
            string catCode = _iutiltyservice.getName("CLI", length, catVal);
            costmaster.clientCode = catCode;
           
  
    
            return View(costmaster);
        }

        //
        // POST: /CostEstimation/Create

       

        //
        // GET: /CostEstimation/Edit/5

      
        [HttpGet]

        public JsonResult getSize(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var coderesult = _categoryservice.getCategoryByCode(code);
                var itemname = _itemmasterservice.getItemByShrtName(coderesult.categoryShortName);
                BarCodeMaster barcode = new BarCodeMaster();
                barcode.items = itemname;
                var modelData = barcode.items.Select(m => new SelectListItem()
                {
                    Text = m.itemCode,
                    Value = m.itemCode
                });

                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modelData = string.Empty;
                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getSizes(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                //var coderesult = _categoryservice.getCategoryByCode(code);
                //var itemname = _itemmasterservice.getItemByShortName(coderesult.categoryShortName);
                var designnmae = _idesignservice.getdesignbyitemname(code);

                CostEstimationOfArticle cost = new CostEstimationOfArticle();
                cost.designname = designnmae;
                var modelData = cost.designname.Select(m => new SelectListItem()
                {
                    Text = m.designName,
                    Value = m.designName
                });

                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var modelData = string.Empty;
                return Json(modelData, JsonRequestBehavior.AllowGet);
            }
        }



        //[HttpGet]
        //public JsonResult GetClientDetails(string name)
        //{
        //    ClientMaster client = new ClientMaster();
        //    client = _iclientservice.getClientByName(name);
        //    int id = client.clientId;
        //    string address = client.clientAddress;
        //    int? pincode = client.pincode;
        //    long? contact = client.contactNo;
        //    string email = client.clientEmail;
        //    string website = client.clientWebsite;
        //    string bankid = client.BankId;
        //    return Json(new
        //    {
        //        Id = id,
        //        address = address,
                
                
        //        pincode = pincode,
        //        contact = contact,
        //      //  date = date1,
        //        email = email,
        //        website=website,
        //        bnkid=bankid,
                

        //    }, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public ActionResult Create([ModelBinder(typeof(CostEstimationCustomBinder))] CostEstimationOfArticle cost)
        {
            if (ModelState.IsValid)
            {

               
                _icostestimationarticleservice.CreateClientcode(cost);
                var sizelast = _icostestimationarticleservice.getLastInsertedClientcode();
                int sizeid = sizelast.clientId;
                return RedirectToAction("Details/" + sizeid, "CostEstimation");
            }

            return View();
        }


        public ActionResult Details(int id = 0)
        {
            CostEstimationOfArticle barcodemaster = db.CostEstimationOfArticles.Find(id);
            if (barcodemaster == null)
            {
                return HttpNotFound();
            }
            return View(barcodemaster);
        }

        //


        // POST: /CostEstimation/Edit/5

        [HttpPost]
        public ActionResult Edit(CostEstimationOfArticle costestimationofarticlemaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costestimationofarticlemaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(costestimationofarticlemaster);
        }

        //
        // GET: /CostEstimation/Delete/5

        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}