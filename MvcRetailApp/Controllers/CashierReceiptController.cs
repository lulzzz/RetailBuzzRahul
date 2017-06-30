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
using System.Web.Routing;
using MvcRetailApp.Filters;

namespace MvcRetailApp.Controllers
{
    //ATTRIBUTE FOR NO ACCESS TO CHANGE URL
    [NoDirectAccess]
    [SessionExpireFilter]
    public class CashierReceiptController : Controller
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
        private readonly IRetailBillService _RetailBillService;
        private readonly ISalesBillService _SalesBillService;
        private readonly ICashierSalesOrderService _CashierSalesOrderService;
        private readonly ICashierRetailBillService _CashierRetailBillService;
        private readonly ICashierSalesBillService _CashierSalesBillService;
        private readonly ICardChequeHandoverService _CardChequeHandoverService;
        private readonly ICashHandoverService _CashHandoverService;
        private readonly IRetailBillAdjAmtDetailService _RetailBillAdjAmtDetailService;
        private readonly ISalesBillAdjAmtDetailService _SalesBillAdjAmtDetailService;
        private readonly ICashierRefundOrderService _CashierRefundOrderService;
        private readonly IBalanceCarryForwardService _BalanceCarryForwardService;
        private readonly IIncomeExpenseVoucherService _IncomeExchangeVoucherService;
        private readonly ITemporaryCashMemoService _TemporaryCashMemoService;
        private readonly ITemporaryCashMemoAdjAmtDetailService _TemporaryCashMemoAdjAmtDetailService;
        private readonly ICashierTemporaryCashMemoService _CashierTemporaryCashMemoService;
        private readonly ISalesReturnService _SalesReturnService;

        public CashierReceiptController(IUtilityService utilityservice, IUserCredentialService usercredentialservice, IModuleService iIModuleService, IRetailBillService RetailBillService,
            ISalesBillService SalesBillService, ICashierSalesOrderService CashierSalesOrderService, ICashierRetailBillService CashierRetailBillService, ICashierSalesBillService CashierSalesBillService,
            ICardChequeHandoverService CardChequeHandoverService, ICashHandoverService CashHandoverService, IRetailBillAdjAmtDetailService RetailBillAdjAmtDetailService,
            ISalesBillAdjAmtDetailService SalesBillAdjAmtDetailService, ICashierRefundOrderService CashierRefundOrderService, IBalanceCarryForwardService BalanceCarryForwardService,
            IIncomeExpenseVoucherService IncomeExchangeVoucherService, ITemporaryCashMemoService TemporaryCashMemoService, ITemporaryCashMemoAdjAmtDetailService TemporaryCashMemoAdjAmtDetailService,
            ICashierTemporaryCashMemoService CashierTemporaryCashMemoService, ISalesReturnService SalesReturnService)
        {
            this._utilityservice = utilityservice;
            this._IUserCredentialService = usercredentialservice;
            this._iIModuleService = iIModuleService;
            this._RetailBillService = RetailBillService;
            this._SalesBillService = SalesBillService;
            this._CashierSalesOrderService = CashierSalesOrderService;
            this._CashierRetailBillService = CashierRetailBillService;
            this._CashierSalesBillService = CashierSalesBillService;
            this._CardChequeHandoverService = CardChequeHandoverService;
            this._CashHandoverService = CashHandoverService;
            this._RetailBillAdjAmtDetailService = RetailBillAdjAmtDetailService;
            this._SalesBillAdjAmtDetailService = SalesBillAdjAmtDetailService;
            this._CashierRefundOrderService = CashierRefundOrderService;
            this._BalanceCarryForwardService = BalanceCarryForwardService;
            this._IncomeExchangeVoucherService = IncomeExchangeVoucherService;
            this._TemporaryCashMemoService = TemporaryCashMemoService;
            this._TemporaryCashMemoAdjAmtDetailService = TemporaryCashMemoAdjAmtDetailService;
            this._CashierTemporaryCashMemoService = CashierTemporaryCashMemoService;
            this._SalesReturnService = SalesReturnService;
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
        public ActionResult StartingBlankPage()
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
        public ActionResult FromAndToDatePopup()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CashierReceipt(string todate, string fromdate)
        {
            string useremail = UserEmail;
            MainApplication model = new MainApplication();
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(useremail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            DateTime ToDate = Convert.ToDateTime(todate);
            DateTime FromDate = Convert.ToDateTime(fromdate);
            int DayInterval = 1;

            List<DateTime> dateList = new List<DateTime>();
            dateList.Add(FromDate);
            while (FromDate.AddDays(DayInterval) <= ToDate)
            {
                FromDate = FromDate.AddDays(DayInterval);
                dateList.Add(FromDate);
            }
            Session["ReceiptDateList"] = dateList;
            return View(model);
        }

        [HttpGet]
        public ActionResult CashierReceiptCalculation(string receiptdate)
        {
            MainApplication model = new MainApplication();

            //*************************************** FOR CREDIT *******************************************

            var CashierReceiptDate = DateTime.ParseExact(receiptdate, "dd/MM/yyyy", null).ToString("MM/dd/yyyy");

            var previousdate = Convert.ToDateTime(CashierReceiptDate).AddDays(-1);

            double TotalOpeningBal = 0;
            var CarryForwardDetails = _BalanceCarryForwardService.GetDataByDate(Convert.ToDateTime(previousdate).Date);

            if (CarryForwardDetails == null)
            {
                TotalOpeningBal = 0;
            }
            else
            {
                TotalOpeningBal = Convert.ToDouble(CarryForwardDetails.ClosingBalance);
            }
            Session["TotalOpeningBalance"] = TotalOpeningBal;

            double CashSalesAmount = 0;

            model.RetailBillList = _RetailBillService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            double CashSalesAmountForRetailBill = 0;
            foreach (var retaildata in model.RetailBillList)
            {
                CashSalesAmountForRetailBill = CashSalesAmountForRetailBill + Convert.ToDouble(retaildata.GrandTotal);
            }

            //minus retail bill sales return amount into cash sales
            model.SalesReturnList = _SalesReturnService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            double CashSalesAmountForSalesReturn = 0;
            foreach (var SRData in model.SalesReturnList)
            {
                CashSalesAmountForSalesReturn = CashSalesAmountForSalesReturn + Convert.ToDouble(SRData.CreditNoteAmount);
            }
            CashSalesAmount = CashSalesAmountForRetailBill - CashSalesAmountForSalesReturn;

            //model.TemporaryCashMemoList = _TemporaryCashMemoService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            //double CashSalesAmountForTempCashMemo = 0;
            //foreach (var TCMData in model.TemporaryCashMemoList)
            //{
            //    if (TCMData.ConvertDateStatus != "Same")
            //    {
            //        CashSalesAmountForTempCashMemo = CashSalesAmountForTempCashMemo + Convert.ToDouble(TCMData.GrandTotal);
            //    }
            //}

            //CashSalesAmount = CashSalesAmountForRetailBill + CashSalesAmountForTempCashMemo;
            Session["CashSalesAmount"] = CashSalesAmount;

            var CashierSOListForCard = _CashierSalesOrderService.GetOrderNoByDateAndCard(Convert.ToDateTime(CashierReceiptDate));
            Session["CashierSOListForCard"] = CashierSOListForCard;

            var CashierSOListForCash = _CashierSalesOrderService.GetOrderNoByDateAndCash(Convert.ToDateTime(CashierReceiptDate));
            Session["CashierSOListForCash"] = CashierSOListForCash;

            var CashierSOListForCheque = _CashierSalesOrderService.GetOrderNoByDateAndCheque(Convert.ToDateTime(CashierReceiptDate));
            Session["CashierSOListForCheque"] = CashierSOListForCheque;

            var CashierRBListForBillBalance = _CashierRetailBillService.GetBillsByDateAndDateStatus(Convert.ToDateTime(CashierReceiptDate));
            Session["CashierRBListForBillBalance"] = CashierRBListForBillBalance;

            var CashierSBListForCreditInvRec = _CashierSalesBillService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["CashierSBListForCreditInvRec"] = CashierSBListForCreditInvRec;

            var AdjAmtSBListForCreditInvRec = _SalesBillAdjAmtDetailService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["AdjAmtSBListForCreditInvRec"] = AdjAmtSBListForCreditInvRec;

            double IndirectIncome = 0;
            var IndirectIncomeData = _IncomeExchangeVoucherService.GetEntryByDateAndVoucherType(Convert.ToDateTime(CashierReceiptDate), "Receipt");
            foreach (var entry in IndirectIncomeData)
            {
                IndirectIncome = IndirectIncome + Convert.ToDouble(entry.Amount);
            }
            Session["IndirectIncome"] = IndirectIncome;

            //*************************************** FOR DEBIT *******************************************

            var RBListForAdvanceAdjust = _RetailBillAdjAmtDetailService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["RBListForAdvanceAdjust"] = RBListForAdvanceAdjust;

            var TCMListForAdvanceAdjust = _TemporaryCashMemoAdjAmtDetailService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["TCMListForAdvanceAdjust"] = TCMListForAdvanceAdjust;

            var SBListForAdvanceAdjust = _SalesBillAdjAmtDetailService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["SBListForAdvanceAdjust"] = SBListForAdvanceAdjust;

            var RBListForDate = _RetailBillService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["RBListForPendingBillBalance"] = RBListForDate;

            int RBListForDateCount = RBListForDate.Count();
            string[] MyRBList = new string[RBListForDateCount];
            string[] MyRBListAmount = new string[RBListForDateCount];
            int i = 0;

            int CashRBCounter = 0;
            foreach (var RBData in RBListForDate)
            {
                double pendingbillbalanceamount = 0;
                double CashRBAmount = 0;

                if (RBData.TemporaryCashMemo == null)
                {
                    var CashRBList = _CashierRetailBillService.GetBillsByDateAndRetailBillNo(Convert.ToDateTime(CashierReceiptDate), RBData.RetailBillNo);
                    foreach (var CashRBData in CashRBList)
                    {
                        CashRBAmount = CashRBAmount + Convert.ToDouble(CashRBData.Payment);
                        CashRBCounter = 1;
                    }
                }
                else
                {
                    var CashRBList = _CashierTemporaryCashMemoService.GetBillsByDateAndTempCashMemoNo(Convert.ToDateTime(CashierReceiptDate), RBData.TemporaryCashMemo);
                    foreach (var CashRBData in CashRBList)
                    {
                        CashRBAmount = CashRBAmount + Convert.ToDouble(CashRBData.Payment);
                        CashRBCounter = 1;
                    }
                }

                pendingbillbalanceamount = Convert.ToDouble(RBData.GrandTotal) - Convert.ToDouble(RBData.AdjustedAmount);

                if (CashRBCounter == 1)
                {
                    pendingbillbalanceamount = pendingbillbalanceamount - CashRBAmount;
                }

                if (pendingbillbalanceamount < 0)
                {
                    pendingbillbalanceamount = 0;
                }

                MyRBList[i] = RBData.RetailBillNo.ToString();

                MyRBListAmount[i] = pendingbillbalanceamount.ToString();
                i++;
                CashRBCounter = 0;
            }
            Session["RBListForPendingBillBalanceCounter"] = RBListForDateCount;
            Session["RBListForPendingBillBalance"] = MyRBList;
            Session["RBListForPendingBillBalanceAmount"] = MyRBListAmount;

            var TCMListForDate = _TemporaryCashMemoService.GetBillsByDateAndConvertStatus(Convert.ToDateTime(CashierReceiptDate));
            Session["TCMListForPendingBillBalance"] = TCMListForDate;

            int TCMListForDateCount = TCMListForDate.Count();
            string[] MyTCMList = new string[TCMListForDateCount];
            string[] MyTCMListAmount = new string[TCMListForDateCount];
            int z = 0;

            int CashTCMCounter = 0;
            foreach (var TCMData in TCMListForDate)
            {
                double pendingbillbalanceamount = 0;
                double CashTCMAmount = 0;

                var CashTCMList = _CashierTemporaryCashMemoService.GetBillsByDateAndTempCashMemoNo(Convert.ToDateTime(CashierReceiptDate), TCMData.TempCashMemoNo);
                foreach (var CashTCMData in CashTCMList)
                {
                    CashTCMAmount = CashTCMAmount + Convert.ToDouble(CashTCMData.Payment);
                    CashTCMCounter = 1;
                }

                pendingbillbalanceamount = Convert.ToDouble(TCMData.GrandTotal) - Convert.ToDouble(TCMData.AdjustedAmount);

                if (CashTCMCounter == 1)
                {
                    pendingbillbalanceamount = pendingbillbalanceamount - CashTCMAmount;
                }

                if (pendingbillbalanceamount < 0)
                {
                    pendingbillbalanceamount = 0;
                }

                MyTCMList[z] = TCMData.TempCashMemoNo.ToString();

                MyTCMListAmount[z] = pendingbillbalanceamount.ToString();
                z++;
                CashTCMCounter = 0;
            }
            Session["TCMListForPendingBillBalanceCounter"] = TCMListForDateCount;
            Session["TCMListForPendingBillBalance"] = MyTCMList;
            Session["TCMListForPendingBillBalanceAmount"] = MyTCMListAmount;

            var CreditCardHandList = _CardChequeHandoverService.GetDataByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["CreditCardHandList"] = CreditCardHandList;

            model.CashHandoverList = _CashHandoverService.GetDataByDate(Convert.ToDateTime(CashierReceiptDate));
            Session["CashHandList"] = model.CashHandoverList;

            double IndirectExpenses = 0;
            var IndirectExpensesData = _IncomeExchangeVoucherService.GetEntryByDateAndVoucherType(Convert.ToDateTime(CashierReceiptDate), "Payment");
            foreach (var entry in IndirectExpensesData)
            {
                IndirectExpenses = IndirectExpenses + Convert.ToDouble(entry.Amount);
            }
            Session["IndirectExpenses"] = IndirectExpenses;

            double SOClosingBal = 0;
            double RBClosingBal = 0;
            double SBClosingBal = 0;

            double ROClosingBal = 0;
            double CardChequeHandClosingBal = 0;
            double CashHandClosingBal = 0;
            double TotalClosingBal = 0;

            model.CashierSalesOrderList = _CashierSalesOrderService.GetOrderNoByDate(Convert.ToDateTime(CashierReceiptDate));
            foreach (var cashsalesbilldata in model.CashierSalesOrderList)
            {
                SOClosingBal = SOClosingBal + Convert.ToDouble(cashsalesbilldata.AdvancePayment);
            }

            model.CashierRetailBillList = _CashierRetailBillService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            foreach (var cashretailbilldata in model.CashierRetailBillList)
            {
                RBClosingBal = RBClosingBal + Convert.ToDouble(cashretailbilldata.Payment);
            }

            model.CashierSalesBillList = _CashierSalesBillService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            foreach (var cashsalesbilldata in model.CashierSalesBillList)
            {
                SBClosingBal = SBClosingBal + Convert.ToDouble(cashsalesbilldata.Payment);
            }


            model.CashierRefundOrderList = _CashierRefundOrderService.GetBillsByDate(Convert.ToDateTime(CashierReceiptDate));
            foreach (var cashdrefundorderdata in model.CashierRefundOrderList)
            {
                ROClosingBal = ROClosingBal + Convert.ToDouble(cashdrefundorderdata.RefundAmount);
            }

            model.CardChequeHandoverList = _CardChequeHandoverService.GetDataByDate(Convert.ToDateTime(CashierReceiptDate));
            foreach (var cardchequedata in model.CardChequeHandoverList)
            {
                CardChequeHandClosingBal = CardChequeHandClosingBal + Convert.ToDouble(cardchequedata.CardChequeAmount);
            }

            model.CashHandoverList = _CashHandoverService.GetDataByDate(Convert.ToDateTime(CashierReceiptDate));
            foreach (var cashdata in model.CashHandoverList)
            {
                CashHandClosingBal = CashHandClosingBal + Convert.ToDouble(cashdata.HandoverCash);
            }

            TotalClosingBal = TotalOpeningBal + SOClosingBal + RBClosingBal + SBClosingBal - ROClosingBal - CardChequeHandClosingBal - CashHandClosingBal;
            Session["TotalClosingBalance"] = TotalClosingBal;

            return View(model);
        }


        // ******************************** BALANCE CARRY FORWARD **************************************
        [HttpGet]
        public ActionResult CreateCarryForward()
        {
            MainApplication model = new MainApplication()
            {
                BalanceCarryForwardDetails = new BalanceCarryForward(),
            };

            var username = HttpContext.Session["UserName"].ToString();
            if (username != "SuperAdmin")
            {
                string shopcode = string.Empty;
                int modulelastcount = _iIModuleService.GetLastRow().Id;
                for (int i = 95; i <= modulelastcount; i++)
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

            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var carryforwarddata = _BalanceCarryForwardService.GetLastRowByFinYr(FinYr);
            string CarryaforwardCode = string.Empty;

            int carryforwardvalue = 0;
            int len = 0;
            if (carryforwarddata != null)
            {
                CarryaforwardCode = carryforwarddata.CarryForwardCode.Substring(4, 6);
                len = (Convert.ToInt32(CarryaforwardCode) + 1).ToString().Length;
                carryforwardvalue = Convert.ToInt32(CarryaforwardCode) + 1;
            }
            else
            {
                carryforwardvalue = 1;
                len = 1;
            }
            CarryaforwardCode = _utilityservice.getName("BCFW", len, carryforwardvalue);
            CarryaforwardCode = CarryaforwardCode + FinYr;
            model.BalanceCarryForwardDetails.CarryForwardCode = CarryaforwardCode;

            var TodayDate = DateTime.Now;
            var PreviousDate = DateTime.Now.AddDays(-1);
            var newPreviousDate = PreviousDate;
            double TodayOpeningBalance = 0;
            double TodayClosingBalance = 0;

            var CarryForwdDetails = _BalanceCarryForwardService.GetDataByDate(Convert.ToDateTime(TodayDate).Date);
            if (CarryForwdDetails != null)
            {
                Session["TodayCarryForwdData"] = "Yes";
            }
            else
            {
                Session["TodayCarryForwdData"] = "No";
            }

            var CarryForwardData = _BalanceCarryForwardService.GetDataByDate(Convert.ToDateTime(PreviousDate).Date);
            if (CarryForwardData == null)
            {
                TodayOpeningBalance = 0;
                Session["TodayOpeningBalance"] = 0;
            }
            else
            {
                TodayOpeningBalance = Convert.ToDouble(CarryForwardData.ClosingBalance);
                Session["TodayOpeningBalance"] = TodayOpeningBalance;
            }

            double SOOpeningBal = 0;
            double RBOpeningBal = 0;
            double SBOpeningBal = 0;
            double IIClosingBal = 0;
            double IEClosingBal = 0;
            double ROOpeningBal = 0;
            double CardChequeHandOpeningBal = 0;
            double CashHandOpeningBal = 0;
            double TotalOpeningBal = 0;

            model.CashierSalesOrderList = _CashierSalesOrderService.GetOrderNoByDate(Convert.ToDateTime(TodayDate).Date);
            foreach (var cashsalesbilldata in model.CashierSalesOrderList)
            {
                SOOpeningBal = SOOpeningBal + Convert.ToDouble(cashsalesbilldata.AdvancePayment);
            }

            model.CashierRetailBillList = _CashierRetailBillService.GetBillsByDate(Convert.ToDateTime(TodayDate).Date);
            foreach (var cashretailbilldata in model.CashierRetailBillList)
            {
                RBOpeningBal = RBOpeningBal + Convert.ToDouble(cashretailbilldata.Payment);
            }

            model.CashierSalesBillList = _CashierSalesBillService.GetBillsByDate(Convert.ToDateTime(TodayDate).Date);
            foreach (var cashsalesbilldata in model.CashierSalesBillList)
            {
                SBOpeningBal = SBOpeningBal + Convert.ToDouble(cashsalesbilldata.Payment);
            }

            var IndirectIncomeDetails = _IncomeExchangeVoucherService.GetEntryByDateAndVoucherType(Convert.ToDateTime(TodayDate), "Receipt");
            foreach (var entry in IndirectIncomeDetails)
            {
                IIClosingBal = IIClosingBal + Convert.ToDouble(entry.Amount);
            }

            model.CashierRefundOrderList = _CashierRefundOrderService.GetBillsByDate(Convert.ToDateTime(TodayDate).Date);
            foreach (var cashdrefundorderdata in model.CashierRefundOrderList)
            {
                ROOpeningBal = ROOpeningBal + Convert.ToDouble(cashdrefundorderdata.RefundAmount);
            }

            model.CardChequeHandoverList = _CardChequeHandoverService.GetDataByDate(Convert.ToDateTime(TodayDate).Date);
            foreach (var cardchequedata in model.CardChequeHandoverList)
            {
                CardChequeHandOpeningBal = CardChequeHandOpeningBal + Convert.ToDouble(cardchequedata.CardChequeAmount);
            }

            model.CashHandoverList = _CashHandoverService.GetDataByDate(Convert.ToDateTime(TodayDate).Date);
            foreach (var cashdata in model.CashHandoverList)
            {
                CashHandOpeningBal = CashHandOpeningBal + Convert.ToDouble(cashdata.HandoverCash);
            }

            var IndirectIncData = _IncomeExchangeVoucherService.GetEntryByDateAndVoucherType(Convert.ToDateTime(TodayDate), "Expense");
            foreach (var entry in IndirectIncData)
            {
                IEClosingBal = IEClosingBal + Convert.ToDouble(entry.Amount);
            }


            TodayClosingBalance = TodayOpeningBalance + SOOpeningBal + RBOpeningBal + SBOpeningBal + IIClosingBal - ROOpeningBal - CardChequeHandOpeningBal - CashHandOpeningBal - IEClosingBal;
            Session["TodayClosingBalance"] = TodayClosingBalance;

            return View(model);

        }

        [HttpPost]
        public ActionResult CreateCarryForward(FormCollection frmcol)
        {
            MainApplication model = new MainApplication()
            {
                BalanceCarryForwardDetails = new BalanceCarryForward(),
            };
            model.userCredentialList = _IUserCredentialService.GetUserCredentialsByEmail(UserEmail);
            model.modulelist = _iIModuleService.getAllModules();
            model.CompanyCode = CompanyCode;
            model.CompanyName = CompanyName;
            model.FinancialYear = FinancialYear;

            //CHECK BCF CODE FOR SAVING TYM..
            string year = FinancialYear;
            string[] yr = year.Split(' ', '-');
            string FinYr = "/" + yr[2].Substring(2) + "-" + yr[6].Substring(2);

            var carryforwarddata = _BalanceCarryForwardService.GetLastRowByFinYr(FinYr);
            string CarryaforwardCode = string.Empty;

            int carryforwardvalue = 0;
            int len = 0;
            if (carryforwarddata != null)
            {
                CarryaforwardCode = carryforwarddata.CarryForwardCode.Substring(4, 6);
                len = (Convert.ToInt32(CarryaforwardCode) + 1).ToString().Length;
                carryforwardvalue = Convert.ToInt32(CarryaforwardCode) + 1;
            }
            else
            {
                carryforwardvalue = 1;
                len = 1;
            }
            CarryaforwardCode = _utilityservice.getName("BCFW", len, carryforwardvalue);
            CarryaforwardCode = CarryaforwardCode + FinYr;

            model.BalanceCarryForwardDetails.CarryForwardCode = CarryaforwardCode;
            model.BalanceCarryForwardDetails.Date = DateTime.Now;
            //model.BalanceCarryForwardDetails.CarryForwardCode = frmcol["CarryForwardCode"];
            model.BalanceCarryForwardDetails.OpeningBalance = Convert.ToDouble(frmcol["OpeningBalanceAmount"]);
            model.BalanceCarryForwardDetails.ClosingBalance = Convert.ToDouble(frmcol["ClosingBalanceAmount"]);
            model.BalanceCarryForwardDetails.Status = "Active";
            model.BalanceCarryForwardDetails.ModifiedOn = DateTime.Now;
            _BalanceCarryForwardService.Create(model.BalanceCarryForwardDetails);
            return View(model);
        }

    }
}
