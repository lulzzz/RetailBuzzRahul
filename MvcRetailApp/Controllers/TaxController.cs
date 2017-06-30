using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using MvcRetailApp.ModelBinder;

namespace MvcRetailApp.Controllers
{
    public class TaxController : Controller
    {
        //public int userId1
        //{
        //    get
        //    {
        //        return (int)Session["userId"];
        //    }
        //    set
        //    {
        //        Session["userId"] = value;
        //    }
        //}
        private readonly ITaxService _taxservice;
        private readonly ICountryService _countryservice;
        private readonly IStateService _stateservice;
        private readonly IDistrictService _districtservice;
        private readonly ISubTaxMasterService _subtaxservice;
        private readonly IUtilityService _utilityservice;
        
        public TaxController(IUtilityService utilty, ISubTaxMasterService subtaxservice, IDistrictService districtservice, ITaxService taxservice, ICountryService countryservice, ICityService cityservice, IStateService stateservice, IUserCredentialService usercredentialservice,
            IModuleService iIModuleService)
        {
            this._taxservice = taxservice;
            this._countryservice = countryservice;
            this._stateservice = stateservice;
            this._districtservice = districtservice;
            this._subtaxservice = subtaxservice;
            this._utilityservice = utilty;

        }
        //
        // GET: /Tax/
        [HttpGet]
        public ActionResult CreateTax()
        {
            MainApplication mainapp = new MainApplication()
            {
                MainTaxMasters = new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };

            string mssgbox = string.Empty;
            MainTaxMaster maintax = _taxservice.getLastInsertedTax();
            int catVal = 0;
            int length = 0;
            if (maintax != null)
            {
                catVal = maintax.Id;
                catVal = catVal + 1;
                length = catVal.ToString().Length;
            }
            else
            {
                catVal = 1;
                length = 1;
            }
            string vatCode = _utilityservice.getName("VAT", length, catVal);
            string stCode = _utilityservice.getName("SAT", length, catVal);
            string otcode = _utilityservice.getName("OTH", length, catVal);
            mainapp.MainTaxMasters.VAT= vatCode;
            mainapp.MainTaxMasters.SalesTax = stCode;
            mainapp.MainTaxMasters.OtherTax = otcode;
            mainapp.MainTaxMasters.CountryList = _countryservice.getallcountries();
            //mainapp.userCredentialList = _IUserCredentialService.getUserCredentialListById(userId1);
            //mainapp.modulelist = _iIModuleService.getAllModules();
            return View(mainapp);
        }
        [HttpGet]
        public JsonResult LoadStateByCountry(int id)
        {
            var state = _stateservice.GetStateByCountry(id);
            MainApplication mainapp = new MainApplication()
            {
                MainTaxMasters = new MainTaxMaster(),

            };
            mainapp.MainTaxMasters.StateList = state;
            var modelData = mainapp.MainTaxMasters.StateList.Select(m => new SelectListItem()
           {
               Text = m.StateName,
               Value = m.Stateid.ToString()
           });
            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadVatInCitiesByState(int id)
        {
            int state = id;
            var statename = _stateservice.GetStateNamebyId(state);
            var taxmasterdetails = _taxservice.getAllStateDetails(statename);
            var subtaxmasterdetails = _subtaxservice.getSubTaxMasterDetails(taxmasterdetails.VAT);
            MainApplication mainapp = new MainApplication()
            {
                SubTaxMasters = new SubTaxMaster(),

            };

            mainapp.SubTaxMasters.Vatlist = subtaxmasterdetails;
            var modeldata = mainapp.SubTaxMasters.Vatlist.Select(m => new SelectListItem()
            {
                Text = m.City,
                Value = m.City
            });

            return Json(modeldata,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadSTInCitiesByState(int id)
        {
            int state = id;
            var statename = _stateservice.GetStateNamebyId(state);
            var taxmasterdetails = _taxservice.getAllStateDetails(statename);
            var subtaxmasterdetails = _subtaxservice.getSubTaxMasterDetails(taxmasterdetails.SalesTax);
            MainApplication mainapp = new MainApplication()
            {
                SubTaxMasters = new SubTaxMaster(),

            };
            mainapp.SubTaxMasters.Stlist = subtaxmasterdetails;
            var modeldata = mainapp.SubTaxMasters.Stlist.Select(m => new SelectListItem()
            {
                Text = m.City,
                Value = m.City
            });

            return Json(modeldata, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult LoadOTInCitiesByState(int id)
        {
            int state = id;
            var statename = _stateservice.GetStateNamebyId(state);
            var taxmasterdetails = _taxservice.getAllStateDetails(statename);
            var subtaxmasterdetails = _subtaxservice.getSubTaxMasterDetails(taxmasterdetails.OtherTax);
            MainApplication mainapp = new MainApplication()
            {
                SubTaxMasters = new SubTaxMaster(),

            };

            mainapp.SubTaxMasters.Otlist = subtaxmasterdetails;
            var modeldata = mainapp.SubTaxMasters.Otlist.Select(m => new SelectListItem()
            {
                Text = m.City,
                Value = m.City
            });

            return Json(modeldata, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadDistrictByState(int id)
        {
            var district = _districtservice.getDistrictbyState(id);
            MainApplication mainapp = new MainApplication()
            {
                MainTaxMasters = new MainTaxMaster(),

            };
            mainapp.MainTaxMasters.DistrictList = district;
            var modelData = mainapp.MainTaxMasters.DistrictList.Select(m => new SelectListItem()
            {
                Text = m.DistrictName,
                Value = m.DistrictName
            });

            return Json(modelData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getDetailsByCity(string value, int statevalue)
        {
            var details = _subtaxservice.getDetailsByCity(value);
            var  state = _stateservice.GetStateNamebyId(statevalue);
            var taxdetails = _taxservice.getAllStateDetails(state);
            MainApplication main = new MainApplication()
            {
                MainTaxMasters=new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };
            main.SubTaxMasters.Id = details.Id;
            main.MainTaxMasters.State = state;
            //main.SubTaxMasters.Code = details.Code;
            main.MainTaxMasters.VAT = taxdetails.VAT;
            main.MainTaxMasters.SalesTax = taxdetails.SalesTax;
            main.MainTaxMasters.OtherTax = taxdetails.OtherTax;
            main.SubTaxMasters.FromDate = details.FromDate;
            main.SubTaxMasters.ToDate = details.ToDate;
            main.SubTaxMasters.Percentage = details.Percentage;
            main.SubTaxMasters.TaxType = details.TaxType;
            main.SubTaxMasters.City = details.City;
            main.stateid = statevalue;
            return View(main);
        }
       
        [HttpGet]
        public ActionResult DynamicRowCreationForState(string Vat, string st, string ot, string state)
        {
            MainApplication main = new MainApplication()
            {
                MainTaxMasters=new MainTaxMaster(),

                SubTaxMasters = new SubTaxMaster(),

            };
            var subtaxmasterlist= _subtaxservice.getLastInserted();
            main.SubTaxMasters.Id = subtaxmasterlist.Id;
            main.MainTaxMasters.State = state;
            main.MainTaxMasters.VAT = Vat;
            main.MainTaxMasters.SalesTax = st;
            main.MainTaxMasters.OtherTax = ot;
            return View(main);

        }
        [HttpPost]
        public ActionResult DynamicRowCreationForState(FormCollection frm,MainApplication mainapp)
        {

             MainApplication main = new MainApplication()
            {

                MainTaxMasters = new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };

             string submaster = frm["hdnRowCount"];
             if (!string.IsNullOrEmpty(submaster))
             {
                 int count = Convert.ToInt32(submaster);

                 for (int i = 1; i <= count; i++)
                 {
                     string taxtype = "taxtype" + i;
                     string cities = "city" + i;
                     string fromdate = "fromdate" + i;
                     string todate = "todate" + i;
                     string percentage = "percentage" + i;

                     string finaltaxtype = frm[taxtype];
                     string finalcity = frm[cities];
                     DateTime finalfromdate = Convert.ToDateTime(frm[fromdate]);
                     DateTime finaltodate = Convert.ToDateTime(frm[todate]);
                     double finalpercentage = Convert.ToDouble(frm[percentage]);
                     main.SubTaxMasters.FromDate = finalfromdate;
                     main.SubTaxMasters.ToDate = finaltodate;
                     main.SubTaxMasters.City = finalcity;
                     main.SubTaxMasters.TaxType = finaltaxtype;
                     main.SubTaxMasters.Percentage = finalpercentage;
                     main.SubTaxMasters.Status = "Active";
                     main.SubTaxMasters.ModifiedOn = DateTime.Now;
                     if (main.SubTaxMasters.TaxType == "VAT")
                     {
                         main.SubTaxMasters.Code = mainapp.MainTaxMasters.VAT;
                     }
                     if (main.SubTaxMasters.TaxType == "SalesTax")
                     {
                         main.SubTaxMasters.Code = mainapp.MainTaxMasters.SalesTax;
                     }
                     if (main.SubTaxMasters.TaxType == "OtherTax")
                     {
                       main.SubTaxMasters.Code = mainapp.MainTaxMasters.OtherTax;
                     }
                  
                     _subtaxservice.CreateSubTax(main.SubTaxMasters);
                 }
             }
             return RedirectToAction("ViewDetailsForDynamicPage", new { Idvalue = mainapp.SubTaxMasters.Id, state = mainapp.MainTaxMasters.State, taxtype = main.SubTaxMasters.TaxType });
        }

        [HttpPost]
        public ActionResult UpdateTax(SubTaxMaster taxmaster)
        {
            MainApplication main = new MainApplication()
            {
                SubTaxMasters = new SubTaxMaster(),

            };

            main.SubTaxMasters.Id = taxmaster.Id;
            main.SubTaxMasters.ToDate = taxmaster.ToDate;
            main.SubTaxMasters.FromDate = taxmaster.FromDate;
            main.SubTaxMasters.TaxType = taxmaster.TaxType;
            main.SubTaxMasters.Percentage = taxmaster.Percentage;
            main.SubTaxMasters.City = taxmaster.City;
            main.SubTaxMasters.Code = taxmaster.Code;
            main.SubTaxMasters.Status = "Active";
            main.SubTaxMasters.ModifiedOn = DateTime.Now;
            _subtaxservice.UpdateSubTax(main.SubTaxMasters);
            return RedirectToAction("ViewTaxDetails/"+taxmaster.Id,"Tax");
        }

        [HttpGet]
        public ActionResult ViewTaxDetails(int id)
        {
            MainApplication main = new MainApplication()
            {
                SubTaxMasters = new SubTaxMaster(),

            };

        var subdetails= _subtaxservice.getDetailsById(id);
        main.SubTaxMasters.ToDate = subdetails.ToDate;
        return View(main);

        }

        [HttpGet]
        public ActionResult ViewDetailsForDynamicPage(int Idvalue, string state, string taxtype)
        {
            MainApplication main = new MainApplication()
            {

                MainTaxMasters = new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };
            int id = Idvalue + 1;
            var subdetails = _subtaxservice.getDetailsById(id);
            var sulastdetails = _subtaxservice.getLastInserted();
            int id1 = subdetails.Id;
            int id2 = sulastdetails.Id;
            var dynamicviewdetails = _subtaxservice.getDetailsOnSpecifiedId(id1,id2);
            main.DynamicList = dynamicviewdetails;
            main.MainTaxMasters.State = state;
            main.SubTaxMasters.TaxType = taxtype;
            return View(main);
        }

        [HttpPost]
        public ActionResult InsertIntoSubTaxMaster(FormCollection frm,string state, string hdnRowCount)
        {
            MainApplication main = new MainApplication()
            {

                MainTaxMasters = new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };
          
            Int32 sid = Convert.ToInt32(state);
            var statename = _stateservice.GetStateNamebyId(sid);
            var code = _taxservice.getAllStateDetails(statename);
            string submaster = frm["hdnRowCount"];
            if (!string.IsNullOrEmpty(submaster))
            {
                int count = Convert.ToInt32(submaster);

                for (int i = 1; i <= count; i++)
                {
                    string taxtype = "taxtype" + i;
                    string cities = "city" + i;
                    string fromdate = "fromdate" + i;
                    string todate = "todate" + i;
                    string percentage = "percentage" + i;

                    string finaltaxtype = frm[taxtype];
                    string finalcity = frm[cities];
                    DateTime finalfromdate = Convert.ToDateTime(frm[fromdate]);
                    DateTime finaltodate = Convert.ToDateTime(frm[todate]);
                    double finalpercentage = Convert.ToDouble(frm[percentage]);

                    main.SubTaxMasters.FromDate = finalfromdate;

                    main.SubTaxMasters.ToDate = finaltodate;
                    main.SubTaxMasters.City = finalcity;
                    main.SubTaxMasters.TaxType = finaltaxtype;
                    main.SubTaxMasters.Percentage = finalpercentage;
                    main.SubTaxMasters.Status = "Active";
                    main.SubTaxMasters.ModifiedOn = DateTime.Now;
                    if (main.SubTaxMasters.TaxType == "VAT")
                    {
                        main.SubTaxMasters.Code = code.VAT;

                    }
                    if (main.SubTaxMasters.TaxType == "SalesTax")
                    {
                        main.SubTaxMasters.Code = code.SalesTax;

                    }
                    if (main.SubTaxMasters.TaxType == "OtherTax")
                    {

                        main.SubTaxMasters.Code = code.OtherTax;

                    }

                    _subtaxservice.CreateSubTax(main.SubTaxMasters);
                }
            }
            return View(); 
        }

        [HttpPost]
        public ActionResult CreateTax(MainApplication mainapp, FormCollection frmcollection)
        {
            MainApplication main = new MainApplication()
            {

                MainTaxMasters = new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };
           Int32 sid = Convert.ToInt32(mainapp.MainTaxMasters.State);
           var state = _stateservice.GetStateNamebyId(sid);
            main.MainTaxMasters.Country = mainapp.MainTaxMasters.Country;
            main.MainTaxMasters.State = state;
            main.MainTaxMasters.VAT = mainapp.MainTaxMasters.VAT;
            main.MainTaxMasters.SalesTax = mainapp.MainTaxMasters.SalesTax;
            main.MainTaxMasters.OtherTax = mainapp.MainTaxMasters.OtherTax;
            main.MainTaxMasters.Status = "Active";
            main.MainTaxMasters.ModifiedOn = DateTime.Now;
            _taxservice.CreateTax(main.MainTaxMasters);
            string submaster = frmcollection["hdnRowCount"].ToString();

            if (!string.IsNullOrEmpty(submaster))
            {
                int count = Convert.ToInt32(submaster);

                for (int i = 1; i <= count; i++)
                {
                    string taxtype = "taxtype" + i;
                    string city = "city" + i;
                    string fromdate = "fromdate" + i;
                    string todate = "todate" + i;
                    string percentage = "percentage" + i;

                    string finaltaxtype = frmcollection[taxtype];
                    string finalcity = frmcollection[city];
                    DateTime finalfromdate = Convert.ToDateTime( frmcollection[fromdate]);
                    DateTime finaltodate = Convert.ToDateTime(frmcollection[todate]);
                    double finalpercentage =Convert.ToDouble( frmcollection[percentage]);
                    main.SubTaxMasters.FromDate = finalfromdate;
                    main.SubTaxMasters.ToDate = finaltodate;
                    main.SubTaxMasters.City = finalcity;
                    main.SubTaxMasters.TaxType = finaltaxtype;
                    main.SubTaxMasters.Percentage = finalpercentage;
                    main.SubTaxMasters.Status = "Active";
                    main.SubTaxMasters.ModifiedOn = DateTime.Now;

                    if (main.SubTaxMasters.TaxType == "VAT")
                    {
                    
                        main.SubTaxMasters.Code = mainapp.MainTaxMasters.VAT;
                        
                    }
                    if (main.SubTaxMasters.TaxType == "SalesTax")
                    {
                      
                        main.SubTaxMasters.Code = mainapp.MainTaxMasters.SalesTax;

                    }
                    if (main.SubTaxMasters.TaxType == "OtherTax")
                    {
                      
                        main.SubTaxMasters.Code = mainapp.MainTaxMasters.OtherTax;

                    }
                    _subtaxservice.CreateSubTax(main.SubTaxMasters);
                }
            }
            var tax = _taxservice.getLastInsertedTax();
            return RedirectToAction("CreateTax","Tax"); 
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            MainApplication model = new MainApplication();
            model.MainTaxMasters = _taxservice.GetById(id);
            model.DynamicList = _subtaxservice.getAllTaxesByStates(model.MainTaxMasters.VAT, model.MainTaxMasters.SalesTax, model.MainTaxMasters.OtherTax);
            return View(model);
        }

        [HttpGet]
        public ActionResult EditTax()
        {
            MainApplication main = new MainApplication()
            {

                MainTaxMasters = new MainTaxMaster(),
                SubTaxMasters = new SubTaxMaster(),

            };
            var country = _countryservice.getallcountries();
            main.MainTaxMasters.CountryList = country;
            //main.userCredentialList = _IUserCredentialService.getUserCredentialListById(userId1);
            //main.modulelist = _iIModuleService.getAllModules();
            return View(main);
        }

    }
}