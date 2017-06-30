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
using System.Data.SqlClient;
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class BillPaymentController : Controller
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
        private readonly IUserCredentialService _IUserCredentialService;
        private readonly IModuleService _iIModuleService;
        private readonly IBillPaymentService _BillPaymentService;
        private readonly IBillPaymentItemService _BillPaymentItemService;
        private readonly ISuppliersMasterService _SuppliersMasterService;
        private readonly IInwardFromSupplierService _InwardFromSupplierService;
        private readonly IInwardItemFromSupplierService _InwardItemFromSupplierService;
        private readonly IPurchaseOrderDetailService _PurchaseOrderDetailService;
        private readonly IDebitNoteService _DebitNoteService;
        private readonly IEmployeeMasterService _EmployeeMasterService;

        public BillPaymentController(IUtilityService utilityservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService,
            IBillPaymentService BillPaymentService, IBillPaymentItemService BillPaymentItemService, ISuppliersMasterService SuppliersMasterService,
            IInwardFromSupplierService InwardFromSupplierService, IInwardItemFromSupplierService InwardItemFromSupplierService, IPurchaseOrderDetailService PurchaseOrderDetailService,
            IDebitNoteService DebitNoteService, IEmployeeMasterService EmployeeMasterService)
        {
            this._utilityservice = utilityservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._BillPaymentService = BillPaymentService;
            this._BillPaymentItemService = BillPaymentItemService;
            this._SuppliersMasterService = SuppliersMasterService;
            this._InwardFromSupplierService = InwardFromSupplierService;
            this._InwardItemFromSupplierService = InwardItemFromSupplierService;
            this._PurchaseOrderDetailService = PurchaseOrderDetailService;
            this._DebitNoteService = DebitNoteService;
            this._EmployeeMasterService = EmployeeMasterService;
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

        [HttpGet]
        public ActionResult CreateBillPayment()
        {
            MainApplication model = new MainApplication()
            {
                BillPaymentDetails = new BillPayment(),
            };

            var details = _BillPaymentService.GetLastRow();
            int val = 0;
            int length = 0;
            if (details != null)
            {
                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            String code = _utilityservice.getName("BP", length, val);
            model.BillPaymentDetails.BillPaymentCode = code;

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

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;
            return View(model);
        }

        //AUTO COMPLETE SUPLLIER NAME
        [HttpGet]
        public JsonResult GetSupplierNameForSupplierPayment(string SearchTerm)
        {
            var suppdata = _SuppliersMasterService.GetSupplierNames(SearchTerm);
            List<string> names = new List<string>();
            foreach (var item in suppdata)
            {
                names.Add(item.SupplierName);
            }
            return Json(new { names }, JsonRequestBehavior.AllowGet);
        }

        //GET INWARD NOS BY SUPPLIER NAME
        [HttpGet]
        public JsonResult GetInwardsFromSuppplier(string SupplierName)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = _InwardFromSupplierService.GetDetailsBySupplierAndPaymentStatus(SupplierName);
            var InwardNos = model.InwardFromSupplierList.Select(s => new SelectListItem()
            {
                Text = s.InwardNo,
                Value = s.InwardNo
            });
            return Json(new { InwardNos}, JsonRequestBehavior.AllowGet);
        }

        //GET PURCHASE ORDER NOS BY SUPPLIER NAME
        [HttpGet]
        public JsonResult GetPurchaseOrdersFromSuppplier(string SupplierName)
        {
            //get po no from supplier and payment status is active(using store procedure groupby po no)..
            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@supplier",SupplierName),
            };
            string query = "GetPOListByPaymentStatusAndSupplier" + " " + "@supplier";
            var POList = _InwardFromSupplierService.GetPONoByPaymentStatusAndSupplier(query, para);
            return Json(POList, JsonRequestBehavior.AllowGet);
        }

        //GET PURCHASE ORDER NOS BY SUPPLIER NAME
        [HttpGet]
        public JsonResult GetChallanNosFromSuppplier(string SupplierName)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = _InwardFromSupplierService.GetDetailsBySupplierAndPaymentStatusAndChallan(SupplierName);
            var ChallanNos = model.InwardFromSupplierList.Select(s => new SelectListItem()
            {
                Text = s.ChallanNo,
                Value = s.ChallanNo
            });
            return Json(new { ChallanNos }, JsonRequestBehavior.AllowGet);
        }

        //GET PURCHASE ORDER NOS BY SUPPLIER NAME
        [HttpGet]
        public JsonResult GetBillNosFromSuppplier(string SupplierName)
        {
            MainApplication model = new MainApplication();
            model.InwardFromSupplierList = _InwardFromSupplierService.GetDetailsBySupplierAndPaymentStatusAndBill(SupplierName);
            var BillNos = model.InwardFromSupplierList.Select(s => new SelectListItem()
            {
                Text = s.BillNo,
                Value = s.BillNo
            });
            return Json(new { BillNos }, JsonRequestBehavior.AllowGet);
        }

        //GET INWARD NOS FROM PO NOS
        [HttpGet]
        public JsonResult GetInwardNosFromPONos(string POs)
        {
            MainApplication model = new MainApplication();
            string PONos = POs;
            string[] SinglePONo = PONos.Split(',');
            List<InwardFromSupplier> InwardNosList = new List<InwardFromSupplier>();
            for (int i = 0; i < SinglePONo.Count(); i++)
            {
                var inwardlist = _InwardFromSupplierService.GetInwardItemsByPoNo(SinglePONo[i]);
                foreach (var data in inwardlist)
                {
                    InwardNosList.Add(data);
                }
            }
            return Json(new { InwardNosList }, JsonRequestBehavior.AllowGet);
        }

        //GET CHALLAN NOS FROM PO NOS
        [HttpGet]
        public JsonResult GetChallanNosFromPONos(string POs)
        {
            MainApplication model = new MainApplication();
            string PONos = POs;
            string[] SinglePONo = PONos.Split(',');
            List<InwardFromSupplier> InwardNosList = new List<InwardFromSupplier>();
            for (int i = 0; i < SinglePONo.Count(); i++)
            {
                var inwardlist = _InwardFromSupplierService.GetInwardItemsByPoNoAndChallan(SinglePONo[i]);
                foreach (var data in inwardlist)
                {
                    InwardNosList.Add(data);
                }
            }
            return Json(new { InwardNosList }, JsonRequestBehavior.AllowGet);
        }

        //GET BILL NOS FROM PO NOS
        [HttpGet]
        public JsonResult GetBillNosFromPONos(string POs)
        {
            MainApplication model = new MainApplication();
            string PONos = POs;
            string[] SinglePONo = PONos.Split(',');
            List<InwardFromSupplier> InwardNosList = new List<InwardFromSupplier>();
            for (int i = 0; i < SinglePONo.Count(); i++)
            {
                var inwardlist = _InwardFromSupplierService.GetInwardItemsByPoNoAndBill(SinglePONo[i]);
                foreach (var data in inwardlist)
                {
                    InwardNosList.Add(data);
                }
            }
            return Json(new { InwardNosList }, JsonRequestBehavior.AllowGet);
        }

        //GET SUPPLIER DETAILS BY SUPPLIER NAME
        [HttpGet]
        public JsonResult GetSuppplierDetails(string SupplierName)
        {
            var data = _SuppliersMasterService.GetByName(SupplierName);
            return Json(new {data.Address, data.SupplierType, data.State, data.ContactNo1, data.Registered }, JsonRequestBehavior.AllowGet);
        }

        //DISPLAY INWARD LIST USING MULTIPLE INWARD NO
        [HttpGet]
        public ActionResult GetInwardListFromInwardNo(string No)
        {
            MainApplication model = new MainApplication();
            string InwardNos = No;
            string[] SingleInwardNo = InwardNos.Split(',');
            List<InwardFromSupplier> InwardList = new List<InwardFromSupplier>();

            for (int i = 0; i < SingleInwardNo.Count(); i++)
            {
                //for inward main table
                model.InwardFromSupplierDetails = _InwardFromSupplierService.GetInwardByInwardNo(SingleInwardNo[i]);

                //access debit note amount
                double DebitNoteAmt = 0;
                var DebitNoteList = _DebitNoteService.GetDebitNoteListByInwardNo(SingleInwardNo[i]);
                if (DebitNoteList != null)
                {
                    foreach (var data in DebitNoteList)
                    {
                        DebitNoteAmt = DebitNoteAmt + Convert.ToDouble(data.Amount);
                    }
                }
                model.InwardFromSupplierDetails.ReadOnlyDebitNoteAmt = DebitNoteAmt;
                InwardList.Add(model.InwardFromSupplierDetails);
            }
            model.InwardFromSupplierList = InwardList;
            return View(model);
        }

        //DISPLAY INWARD LIST USING MULTIPLE CHALLAN NO
        [HttpGet]
        public ActionResult GetInwardListFromChallanNo(string No)
        {
            MainApplication model = new MainApplication();
            string ChallanNos = No;
            string[] SingleChallanNo = ChallanNos.Split(',');
            List<InwardFromSupplier> InwardList = new List<InwardFromSupplier>();

            for (int i = 0; i < SingleChallanNo.Count(); i++)
            {
                //for inward main table
                model.InwardFromSupplierDetails = _InwardFromSupplierService.GetInwardByChallanNo(SingleChallanNo[i]);

                //access debit note amount
                double DebitNoteAmt = 0;
                var DebitNoteList = _DebitNoteService.GetDebitNoteListByInwardNo(model.InwardFromSupplierDetails.InwardNo);
                if (DebitNoteList != null)
                {
                    foreach (var data in DebitNoteList)
                    {
                        DebitNoteAmt = DebitNoteAmt + Convert.ToDouble(data.Amount);
                    }
                }
                model.InwardFromSupplierDetails.ReadOnlyDebitNoteAmt = DebitNoteAmt;
                InwardList.Add(model.InwardFromSupplierDetails);
            }
            model.InwardFromSupplierList = InwardList;
            return View(model);
        }

        //DISPLAY INWARD LIST USING MULTIPLE BILL NO
        [HttpGet]
        public ActionResult GetInwardListFromBillNo(string No)
        {
            MainApplication model = new MainApplication();
            string BillNos = No;
            string[] SingleBillNo = BillNos.Split(',');
            List<InwardFromSupplier> InwardList = new List<InwardFromSupplier>();

            for (int i = 0; i < SingleBillNo.Count(); i++)
            {
                //for inward main table
                model.InwardFromSupplierDetails = _InwardFromSupplierService.GetInwardBySupplierBillNo(SingleBillNo[i]);

                //access debit note amount
                double DebitNoteAmt = 0;
                var DebitNoteList = _DebitNoteService.GetDebitNoteListByInwardNo(model.InwardFromSupplierDetails.InwardNo);
                if (DebitNoteList != null)
                {
                    foreach (var data in DebitNoteList)
                    {
                        DebitNoteAmt = DebitNoteAmt + Convert.ToDouble(data.Amount);
                    }
                }
                model.InwardFromSupplierDetails.ReadOnlyDebitNoteAmt = DebitNoteAmt;
                InwardList.Add(model.InwardFromSupplierDetails);
            }
            model.InwardFromSupplierList = InwardList;
            return View(model);
        }

        public ActionResult CashPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CARD PAYMENT DETAILS
        public ActionResult CardPayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CHEQUE PAYMENT DETAILS
        public ActionResult ChequePayment()
        {
            return View();
        }

        // PARTIAL VIEW OF CASH CARD CHEQUE PAYMENT DETAILS
        public ActionResult CashCardChequePayment()
        {
            return View();
        }

        //GET LOGIN EMPLOPYEE DETAILS BY EMPLOYEE EMAIL
        [HttpGet]
        public JsonResult GetPreparedByEmpDetails(string email)
        {
            var data = _EmployeeMasterService.getEmpByEmail(email);
            return Json(new { data.Name, data.MobileNo }, JsonRequestBehavior.AllowGet);
        }

        //SAVE BILL PAYMENT AND ITS ITEMS
        [HttpPost]
        public ActionResult CreateBillPayment(MainApplication mainapp, FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                BillPaymentDetails = new BillPayment(),
                BillPaymentItemDetails = new BillPaymentItem(),
            };

            var details = _BillPaymentService.GetLastRow();
            int val = 0;
            int length = 0;
            if (details != null)
            {
                val = details.Id;
                val = val + 1;
                length = val.ToString().Length;
            }
            else
            {
                val = 1;
                length = 1;
            }
            String code = _utilityservice.getName("BP", length, val);
            mainapp.BillPaymentDetails.BillPaymentCode = code;

            // SAVE BILL PAYMENT
            mainapp.BillPaymentDetails.Date = DateTime.Now;
            mainapp.BillPaymentDetails.SupplierName = frmcol["SupplierName"];
            mainapp.BillPaymentDetails.Payment = Convert.ToDouble(frmcol["TotalPaymentVal"]);
            mainapp.BillPaymentDetails.PaymentType = frmcol["PaymentType"];
            mainapp.BillPaymentDetails.Status = "Active";
            mainapp.BillPaymentDetails.ModifiedOn = DateTime.Now;

            mainapp.BillPaymentDetails.Cash_1000 = Convert.ToInt32(frmcol["BillPaymentDetails.Cash_1000"]);
            mainapp.BillPaymentDetails.Cash_500 = Convert.ToInt32(frmcol["BillPaymentDetails.Cash_500"]);
            mainapp.BillPaymentDetails.Cash_100 = Convert.ToInt32(frmcol["BillPaymentDetails.Cash_100"]);
            mainapp.BillPaymentDetails.Cash_50 = Convert.ToInt32(frmcol["BillPaymentDetails.Cash_50"]);
            mainapp.BillPaymentDetails.Cash_20 = Convert.ToInt32(frmcol["BillPaymentDetails.Cash_20"]);
            mainapp.BillPaymentDetails.Cash_10 = Convert.ToInt32(frmcol["BillPaymentDetails.Cash_10"]);
            mainapp.BillPaymentDetails.Cash_1 = Convert.ToDouble(frmcol["BillPaymentDetails.Cash_1"]);
            mainapp.BillPaymentDetails.Cash_1000_Amt = Convert.ToDouble(frmcol["Amt1"]);
            mainapp.BillPaymentDetails.Cash_500_Amt = Convert.ToDouble(frmcol["Amt2"]);
            mainapp.BillPaymentDetails.Cash_100_Amt = Convert.ToDouble(frmcol["Amt3"]);
            mainapp.BillPaymentDetails.Cash_50_Amt = Convert.ToDouble(frmcol["Amt4"]);
            mainapp.BillPaymentDetails.Cash_20_Amt = Convert.ToDouble(frmcol["Amt5"]);
            mainapp.BillPaymentDetails.Cash_10_Amt = Convert.ToDouble(frmcol["Amt6"]);
            mainapp.BillPaymentDetails.Cash_1_Amt = Convert.ToDouble(frmcol["Amt7"]);
            mainapp.BillPaymentDetails.TotalCash = Convert.ToDouble(frmcol["BillPaymentDetails.TotalCash"]);
            mainapp.BillPaymentDetails.SelectedCard = frmcol["Card"];
            mainapp.BillPaymentDetails.CreditCardNo = frmcol["BillPaymentDetails.CreditCardNo"];
            if (frmcol["BillPaymentDetails.CreditCardAmount"] == "")
            {
                mainapp.BillPaymentDetails.CreditCardAmount = 0;
            }
            else
            {
                mainapp.BillPaymentDetails.CreditCardAmount = Convert.ToDouble(frmcol["BillPaymentDetails.CreditCardAmount"]);
            }
            mainapp.BillPaymentDetails.CreditCardType = frmcol["BillPaymentDetails.CreditCardType"];
            mainapp.BillPaymentDetails.CreditCardBank = frmcol["BillPaymentDetails.CreditCardBank"];
            mainapp.BillPaymentDetails.DebitCardNo = frmcol["BillPaymentDetails.DebitCardNo"];
            mainapp.BillPaymentDetails.DebitCardName = frmcol["BillPaymentDetails.DebitCardName"];
            mainapp.BillPaymentDetails.DebitCardType = frmcol["BillPaymentDetails.DebitCardType"];
            mainapp.BillPaymentDetails.DebitCardBank = frmcol["BillPaymentDetails.DebitCardBank"];
            if (frmcol["BillPaymentDetails.DebitCardAmount"] == "")
            {
                mainapp.BillPaymentDetails.DebitCardAmount = 0;
            }
            else
            {
                mainapp.BillPaymentDetails.DebitCardAmount = Convert.ToDouble(frmcol["BillPaymentDetails.DebitCardAmount"]);
            }
            mainapp.BillPaymentDetails.ChequeNo = frmcol["BillPaymentDetails.ChequeNo"];
            mainapp.BillPaymentDetails.ChequeAccNo = frmcol["BillPaymentDetails.ChequeAccNo"];
            mainapp.BillPaymentDetails.ChequeAmount = frmcol["BillPaymentDetails.ChequeAmount"];
            if (mainapp.BillPaymentDetails.ChequeNo != null && mainapp.BillPaymentDetails.ChequeNo != "")
            {
                mainapp.BillPaymentDetails.ChequeDate = Convert.ToDateTime(frmcol["BillPaymentDetails.ChequeDate"]);
            }
            else
            {
                mainapp.BillPaymentDetails.ChequeDate = null;
            }
            mainapp.BillPaymentDetails.ChequeBank = frmcol["BillPaymentDetails.ChequeBank"];
            mainapp.BillPaymentDetails.ChequeBranch = frmcol["BillPaymentDetails.ChequeBranch"];

            var username = HttpContext.Session["UserName"].ToString();
            //IF EXCEPT SUPERADMIN LOGIN THEN SHOW SHOP OR GODOWN
            if (username != "SuperAdmin")
            {
                mainapp.BillPaymentDetails.ShopCode = Session["LOGINSHOPGODOWNCODE"].ToString();
                mainapp.BillPaymentDetails.ShopName = Session["SHOPGODOWNNAME"].ToString();
            }
            else
            {
                mainapp.BillPaymentDetails.ShopCode = "SuperAdmin";
                mainapp.BillPaymentDetails.ShopName = "SuperAdmin";
            }
            _BillPaymentService.Create(mainapp.BillPaymentDetails);

            //SAVE BILL PAYMENT ITEM
            int inwardlistcount = Convert.ToInt32(frmcol["InwardListCount"]);
            if (inwardlistcount != 0)
            {
                for (int i = 1; i < inwardlistcount; i++)
                {
                    string checkbox = "CheckBox" + i;
                    string inwardno = "InwardNo" + i;
                    string grandtotal = "GrandTotal" + i;
                    string debitnoteamt = "DebitNoteAmt" + i;
                    string amountpaid = "AmountPaid" + i;
                    string payment = "Payment" + i;
                    string discount = "Discount" + i;
                    string balanceval = "BalanceVal" + i;

                    if (frmcol[checkbox] == "Yes")
                    {
                        model.BillPaymentItemDetails.BillPaymentCode = mainapp.BillPaymentDetails.BillPaymentCode;
                        model.BillPaymentItemDetails.InwardFromSupplierNo = frmcol[inwardno];

                        //get details by inward no..
                        var inwarddetails = _InwardFromSupplierService.GetDetailsByPINo(frmcol[inwardno]);
                        model.BillPaymentItemDetails.InwardFromSupplierDate = inwarddetails.InwardDate;
                        model.BillPaymentItemDetails.SupplierBillNo = inwarddetails.BillNo;
                        model.BillPaymentItemDetails.SupplierChallanNo = inwarddetails.ChallanNo;
                        model.BillPaymentItemDetails.PONo = inwarddetails.PoNo;
                        model.BillPaymentItemDetails.GrandTotal = Convert.ToDouble(frmcol[grandtotal]);
                        model.BillPaymentItemDetails.DebitNoteAmount = Convert.ToDouble(frmcol[debitnoteamt]);
                        model.BillPaymentItemDetails.PreviousPayment = Convert.ToDouble(frmcol[amountpaid]);
                        model.BillPaymentItemDetails.Payment = Convert.ToDouble(frmcol[payment]);
                        model.BillPaymentItemDetails.Discount = Convert.ToDouble(frmcol[discount]);
                        model.BillPaymentItemDetails.Balance = Convert.ToDouble(frmcol[balanceval]);
                        model.BillPaymentItemDetails.Status = "Active";
                        model.BillPaymentItemDetails.ModifiedOn = DateTime.Now;
                        _BillPaymentItemService.Create(model.BillPaymentItemDetails);

                        //UPDATE INWARD FROM SUPPLIER AFTER SUPPLIER PAYMENT..
                        var InwardData = _InwardFromSupplierService.GetDetailsByPINo(frmcol[inwardno]);
                        InwardData.PaymentAmount = InwardData.PaymentAmount + model.BillPaymentItemDetails.Payment;
                        InwardData.DebitNotesAmount = InwardData.DebitNotesAmount + model.BillPaymentItemDetails.DebitNoteAmount;
                        InwardData.PaymentBalance = model.BillPaymentItemDetails.Balance;

                        //if inward balanace is 0 then inactive payment status of that inward
                        if (InwardData.PaymentBalance == 0)
                        {
                            InwardData.PaymentStatus = "InActive";
                        }
                        _InwardFromSupplierService.UpdateInward(InwardData);

                        //UPDATE DEBIT NOTE (STATUS IS INAVTIVE WHEN DEBIT NOTE ADJUSTED FOR INWARD BILL PAYMENT
                        var DebitNoteData = _DebitNoteService.GetDebitNoteListByInwardNo(frmcol[inwardno]);
                        foreach (var data in DebitNoteData)
                        {
                            data.Status = "InActive";
                            _DebitNoteService.Update(data);
                        }

                    }
                }
            }
            return RedirectToAction("CreateBillPayment");
        }
    }
}
