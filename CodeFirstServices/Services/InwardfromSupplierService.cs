using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class InwardfromSupplierService : IInwardFromSupplierService
    {
        private readonly IInwardFromSupplierRepository _InwardFromSupplierRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IGetPOListByPaymentStatusAndSupplierRepository _GetPOListByPaymentStatusAndSupplierRepository;
        public InwardfromSupplierService(IInwardFromSupplierRepository InwardFromSupplierRepository, IUnitOfWork UnitOfWork, IGetPOListByPaymentStatusAndSupplierRepository GetPOListByPaymentStatusAndSupplierRepository)
        {
            this._InwardFromSupplierRepository = InwardFromSupplierRepository;
            this._UnitOfWork = UnitOfWork;
            this._GetPOListByPaymentStatusAndSupplierRepository = GetPOListByPaymentStatusAndSupplierRepository;
        }

        public void CreateInward(InwardFromSupplier PerformaInvoice)
        {
            _InwardFromSupplierRepository.Add(PerformaInvoice);
            _UnitOfWork.Commit();
        }

        public void UpdateInward(InwardFromSupplier PerformaInvoice)
        {
            _InwardFromSupplierRepository.Update(PerformaInvoice);
            _UnitOfWork.Commit();
        }

        public InwardFromSupplier GetDetailsByPoNo(string poNo)
        {
            var performa = _InwardFromSupplierRepository.Get(pi => pi.PoNo == poNo);
            return performa;
        }

        public IEnumerable<InwardFromSupplier> GetInwardItemsByPoNo(string pono)
        {
            var performa = _InwardFromSupplierRepository.GetMany(pi => pi.PoNo == pono);
            return performa;
        }

        public IEnumerable<InwardFromSupplier> GetInwardItemsByPoNoAndChallan(string pono)
        {
            var performa = _InwardFromSupplierRepository.GetMany(pi => pi.PoNo == pono && pi.ChallanNo != null);
            return performa;
        }

        public IEnumerable<InwardFromSupplier> GetInwardItemsByPoNoAndBill(string pono)
        {
            var performa = _InwardFromSupplierRepository.GetMany(pi => pi.PoNo == pono && pi.BillNo != null);
            return performa;
        }

        public IEnumerable<InwardFromSupplier> GetAllInvoices()
        {
            var supplier = _InwardFromSupplierRepository.GetAll();
            return supplier;
        }

        public InwardFromSupplier GetById(int no)
        {
            var data = _InwardFromSupplierRepository.GetById(no);
            return data;
        }

        public InwardFromSupplier GetDetailsByPINo(string pino)
        {
            var details = _InwardFromSupplierRepository.Get(d => d.InwardNo == pino);
            return details;
        }
        public IEnumerable<InwardFromSupplier> getReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _InwardFromSupplierRepository.GetMany(cl => Convert.ToDateTime(cl.InwardDate).Date >= fromdate.Date && Convert.ToDateTime(cl.InwardDate).Date <= todate);
            return value;
        }
        public IEnumerable<InwardFromSupplier> GetDataByPINoAndStatus()
        {
            var supplier = _InwardFromSupplierRepository.GetMany(d => d.Status == "Active");
            return supplier;
        }
        public IEnumerable<InwardFromSupplier> GetDetailsBySupplier(string supplier)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == supplier);
            return data;
        }

        public IEnumerable<InwardFromSupplier> GetReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == Supplier && Convert.ToDateTime(d.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(d.ModifiedOn).Date <= todate.Date);
            return data;
        }

        public IEnumerable<InwardFromSupplier> GetSupplierInwardList(string inwardno)
        {
            var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.ToLower().Contains(inwardno.ToString().ToLower()) && i.Status == "Active" && i.Editable == "Yes" && i.PoNo != "NoPO");
            return data;
        }

        public IEnumerable<InwardFromSupplier> GetDetailsForIWOPByPINo(string pino)
        {
            var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.ToLower().Contains(pino.ToString().ToLower()) && i.PoNo == "NoPO" && i.Status == "Active");
            return data;
        }

        public IEnumerable<InwardFromSupplier> GetInwardByPaymentStatus(string inward)
        {
            var data = _InwardFromSupplierRepository.GetMany(b => b.PaymentStatus == "Active" && b.InwardNo.ToLower().StartsWith(inward.ToString().ToLower())).OrderBy(b => b.InwardNo);
            return data;
        }

        public InwardFromSupplier GetInwardByInwardNo(string no)
        {
            var performa = _InwardFromSupplierRepository.Get(pi => pi.InwardNo == no);
            return performa;
        }

        public InwardFromSupplier GetInwardByChallanNo(string no)
        {
            var performa = _InwardFromSupplierRepository.Get(pi => pi.ChallanNo == no);
            return performa;
        }

        public InwardFromSupplier GetInwardBySupplierBillNo(string no)
        {
            var performa = _InwardFromSupplierRepository.Get(pi => pi.BillNo == no);
            return performa;
        }

        public IEnumerable<InwardFromSupplier> GetDetailsBySupplierAndPaymentStatus(string supplier)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == supplier && d.PaymentStatus == "Active");
            return data;
        }

        public IEnumerable<InwardFromSupplier> GetDetailsBySupplierAndPaymentStatusAndChallan(string supplier)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == supplier && d.PaymentStatus == "Active" && d.ChallanNo != null);
            return data;
        }

        public IEnumerable<InwardFromSupplier> GetDetailsBySupplierAndPaymentStatusAndBill(string supplier)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == supplier && d.PaymentStatus == "Active" && d.BillNo != null);
            return data;
        }

        public InwardFromSupplier GetDetailsByBillNoOrChallanNo(string billchallanno)
        {
            var data = _InwardFromSupplierRepository.Get(i => i.BillNo == billchallanno || i.ChallanNo == billchallanno);
            return data;
        }

        public IEnumerable<GetPOListByPaymentStatusAndSupplier> GetPONoByPaymentStatusAndSupplier(string procname, object[] supplier)
        {
            var list = _GetPOListByPaymentStatusAndSupplierRepository.ExecuteCustomStoredProcByParam(procname, supplier);
            return list;
        }

        public IEnumerable<InwardFromSupplier> GetSupplierBillNos(string supplier, string billno)
        {
            var list = _InwardFromSupplierRepository.GetMany(i => i.SupplierName == supplier && i.BillNo != null && i.BillNo.ToLower().StartsWith(billno.ToLower()) && i.Status == "InActive").OrderBy(i => i.BillNo);
            return list;
        }

        public IEnumerable<InwardFromSupplier> GetSupplierChallanNos(string supplier, string challanno)
        {
            var list = _InwardFromSupplierRepository.GetMany(i => i.SupplierName == supplier && i.ChallanNo != null && i.ChallanNo.ToLower().StartsWith(challanno.ToLower()) && i.Status == "InActive").OrderBy(i => i.BillNo);
            return list;
        }

        public IEnumerable<InwardFromSupplier> GetDailyInwardWithoutPo()
        {
            var list = _InwardFromSupplierRepository.GetMany(i => (Convert.ToDateTime(i.InwardDate).Date) == DateTime.Now.Date && i.PoNo == "NoPO");
            return list;
        }

        public IEnumerable<InwardFromSupplier> getInwardWithPOReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _InwardFromSupplierRepository.GetMany(cl => Convert.ToDateTime(cl.InwardDate).Date >= fromdate.Date && Convert.ToDateTime(cl.InwardDate).Date <= todate && cl.PoNo != "NoPO");
            return value;
        }

        public IEnumerable<InwardFromSupplier> GetInwardWithPOReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == Supplier && d.PoNo != "NoPO" && Convert.ToDateTime(d.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(d.ModifiedOn).Date <= todate.Date);
            return data;
        }

        public IEnumerable<InwardFromSupplier> getInwardWithoutPOReportByDate(DateTime fromdate, DateTime todate)
        {
            var value = _InwardFromSupplierRepository.GetMany(cl => Convert.ToDateTime(cl.InwardDate).Date >= fromdate.Date && Convert.ToDateTime(cl.InwardDate).Date <= todate && cl.PoNo == "NoPO");
            return value;
        }

        public IEnumerable<InwardFromSupplier> GetInwardWithoutPOReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate)
        {
            var data = _InwardFromSupplierRepository.GetMany(d => d.SupplierName == Supplier && d.PoNo == "NoPO" && Convert.ToDateTime(d.ModifiedOn).Date >= fromdate.Date && Convert.ToDateTime(d.ModifiedOn).Date <= todate.Date);
            return data;
        }

        public InwardFromSupplier GetLastInwardWithPOByFinYr(string year, string code)
        {
            if (code.Contains("SH"))
            {
                var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.Contains(year) && i.PoNo != "NoPO" && i.ShopCode == code).OrderBy(i => i.InwardNo).LastOrDefault();
                return data;
            }
            else
            {
                var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.Contains(year) && i.PoNo != "NoPO" && i.GodownCode == code).OrderBy(i => i.InwardNo).LastOrDefault();
                return data;
            }
        }

        public InwardFromSupplier GetLastInwardWithoutPOByFinYr(string year, string code)
        {
            if (code.Contains("SH"))
            {
                var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.Contains(year) && i.PoNo == "NoPO" && i.ShopCode == code).OrderBy(i => i.InwardNo).LastOrDefault();
                return data;
            }
            else
            {
                var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.Contains(year) && i.PoNo == "NoPO" && i.GodownCode == code).OrderBy(i => i.InwardNo).LastOrDefault();
                return data;
            }
        }

        public IEnumerable<InwardFromSupplier> GetDataByPINoAndStatusForNopo()
        {
            var supplier = _InwardFromSupplierRepository.GetMany(d => (d.PoNo == "NoPO") && (d.Status == "Active" || d.PaymentStatus == "Active"));
            return supplier;
        }

        public IEnumerable<InwardFromSupplier> GetDataByPINoAndStatusForPo()
        {
            var supplier = _InwardFromSupplierRepository.GetMany(d => (d.PoNo != "NoPO") && (d.Status == "Active" || d.PaymentStatus == "Active"));
            return supplier;
        }

        public IEnumerable<InwardFromSupplier> GetAllSupplierInwards(string term)
        {
            var list = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.ToLower().Contains(term.ToLower()) && i.PoNo != "NoPO");
            return list;
        }

        public IEnumerable<InwardFromSupplier> GetAllInwardsWithoutPoes(string term)
        {
            var list = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.ToLower().Contains(term.ToLower()) && i.PoNo == "NoPO");
            return list;
        }

        public IEnumerable<InwardFromSupplier> GetAllInwardNos(string inwardno)
        {
            var data = _InwardFromSupplierRepository.GetMany(i => i.InwardNo.ToLower().Contains(inwardno.ToString().ToLower()));
            return data;
        }
    }
}
