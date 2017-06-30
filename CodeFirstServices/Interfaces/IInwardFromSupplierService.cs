using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardFromSupplierService
    {
        void CreateInward(InwardFromSupplier InwardFromSupplier);
        void UpdateInward(InwardFromSupplier InwardFromSupplier);
        InwardFromSupplier GetLastInwardWithPOByFinYr(string year, string code);
        InwardFromSupplier GetLastInwardWithoutPOByFinYr(string year, string code);
        InwardFromSupplier GetDetailsByPoNo(string poNo);
        IEnumerable<InwardFromSupplier> GetInwardItemsByPoNo(string pono);
        IEnumerable<InwardFromSupplier> GetInwardItemsByPoNoAndChallan(string pono);
        IEnumerable<InwardFromSupplier> GetInwardItemsByPoNoAndBill(string pono);
        IEnumerable<InwardFromSupplier> GetAllInvoices();
        InwardFromSupplier GetById(int pino);
        InwardFromSupplier GetDetailsByPINo(string pino);
        IEnumerable<InwardFromSupplier> getReportByDate(DateTime fromdate, DateTime todate);
        IEnumerable<InwardFromSupplier> GetDataByPINoAndStatus();
        IEnumerable<InwardFromSupplier> GetDetailsBySupplier(string supplier);
        IEnumerable<InwardFromSupplier> GetReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate);
        IEnumerable<InwardFromSupplier> GetSupplierInwardList(string inwardno);
        IEnumerable<InwardFromSupplier> GetDetailsForIWOPByPINo(string pino);
        IEnumerable<InwardFromSupplier> GetInwardByPaymentStatus(string inward);
        InwardFromSupplier GetInwardByInwardNo(string no);
        InwardFromSupplier GetInwardByChallanNo(string no);
        InwardFromSupplier GetInwardBySupplierBillNo(string no);
        IEnumerable<InwardFromSupplier> GetDetailsBySupplierAndPaymentStatus(string supplier);
        IEnumerable<InwardFromSupplier> GetDetailsBySupplierAndPaymentStatusAndChallan(string supplier);
        IEnumerable<InwardFromSupplier> GetDetailsBySupplierAndPaymentStatusAndBill(string supplier);
        InwardFromSupplier GetDetailsByBillNoOrChallanNo(string billchallanno);
        IEnumerable<GetPOListByPaymentStatusAndSupplier> GetPONoByPaymentStatusAndSupplier(string procname, object[] supplier);
        IEnumerable<InwardFromSupplier> GetSupplierBillNos(string supplier, string billno);
        IEnumerable<InwardFromSupplier> GetSupplierChallanNos(string supplier, string challanno);
        IEnumerable<InwardFromSupplier> GetDailyInwardWithoutPo();
        IEnumerable<InwardFromSupplier> getInwardWithPOReportByDate(DateTime fromdate, DateTime todate);
        IEnumerable<InwardFromSupplier> GetInwardWithPOReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate);
        IEnumerable<InwardFromSupplier> getInwardWithoutPOReportByDate(DateTime fromdate, DateTime todate);
        IEnumerable<InwardFromSupplier> GetInwardWithoutPOReportBySupplierNameAndDate(string Supplier, DateTime fromdate, DateTime todate);
        IEnumerable<InwardFromSupplier> GetDataByPINoAndStatusForNopo();
        IEnumerable<InwardFromSupplier> GetDataByPINoAndStatusForPo();
        IEnumerable<InwardFromSupplier> GetAllSupplierInwards(string term);
        IEnumerable<InwardFromSupplier> GetAllInwardsWithoutPoes(string term);
        IEnumerable<InwardFromSupplier> GetAllInwardNos(string inwardno);
    }
}
