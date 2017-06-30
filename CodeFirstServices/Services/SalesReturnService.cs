using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class SalesReturnService : ISalesReturnService
    {
        private readonly ISalesReturnRepository _SalesReturnRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public SalesReturnService(ISalesReturnRepository SalesReturnRepository, IUnitOfWork UnitOfWork)
        {
            this._SalesReturnRepository = SalesReturnRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(SalesReturn SalesReturn)
        {
            _SalesReturnRepository.Add(SalesReturn);
            _UnitOfWork.Commit();
        }

        public void Update(SalesReturn SalesReturn)
        {
            _SalesReturnRepository.Update(SalesReturn);
            _UnitOfWork.Commit();
        }

        public IEnumerable<SalesReturn> GetAllSalesReturnNo(string srno)
        {
            var list = _SalesReturnRepository.GetMany(s => s.SalesReturnNo.ToLower().Contains(srno.ToLower())).OrderBy(s => s.SalesReturnNo);
            return list;
        }

        public SalesReturn GetLastRow()
        {
            var data = _SalesReturnRepository.GetAll().LastOrDefault();
            return data;
        }

        public SalesReturn GetLastSalesReturnByFinYr(string year, string shopcode)
        {
            var data = _SalesReturnRepository.GetMany(s => s.SalesReturnNo.Contains(year) && s.ShopCode == shopcode).OrderBy(s => s.SalesReturnNo).LastOrDefault();
            return data;
        }

        public SalesReturn GetLastSalesOfBillNo(string billno)
        {
            var data = _SalesReturnRepository.GetMany(s => s.BillNo == billno).OrderBy(s => s.BillNo == billno).LastOrDefault();
            return data;
        }

        public SalesReturn GetById(int id)
        {
            var data = _SalesReturnRepository.GetById(id);
            return data;
        }

        public IEnumerable<SalesReturn> GetAllSalesOfBill(string billno)
        {
            var list = _SalesReturnRepository.GetMany(s => s.BillNo == billno);
            return list;
        }

        public SalesReturn GetDataByBillNoAndStatus(string billno)
        {
            var data = _SalesReturnRepository.Get(d => d.BillNo == billno && d.Status == "Active");
            return data;
        }

        public SalesReturn GetSalesByReturnNo(string srno)
        {
            var data = _SalesReturnRepository.Get(s => s.SalesReturnNo == srno);
            return data;
        }

        public IEnumerable<SalesReturn> GetDailySalesReturns()
        {
            var list = _SalesReturnRepository.GetMany(s => Convert.ToDateTime(s.SalesReturnDate).Date == DateTime.Now.Date);
            return list;
        }

        public IEnumerable<SalesReturn> GetReturnsByBillNo(string no)
        {
            var list = _SalesReturnRepository.GetMany(s => s.BillNo == no && s.Status == "Active");
            return list;
        }

        public IEnumerable<SalesReturn> GetBillsByDate(DateTime date)
        {
            var data = _SalesReturnRepository.GetMany(s => Convert.ToDateTime(s.SalesReturnDate).Date == date);
            return data;
        }
    }
}
