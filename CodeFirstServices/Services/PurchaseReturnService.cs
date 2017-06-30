using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;

namespace CodeFirstServices.Services
{
    public class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly IPurchaseReturnRepository _PurchaseReturnRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public PurchaseReturnService(IPurchaseReturnRepository PurchaseReturnRepository, IUnitOfWork UnitOfWork)
        {
            this._PurchaseReturnRepository = PurchaseReturnRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(PurchaseReturn PurchaseReturn)
        {
            _PurchaseReturnRepository.Add(PurchaseReturn);
            _UnitOfWork.Commit();
        }

        public void Update(PurchaseReturn PurchaseReturn)
        {
            _PurchaseReturnRepository.Update(PurchaseReturn);
            _UnitOfWork.Commit();
        }

        public PurchaseReturn GetLastRow()
        {
            var data = _PurchaseReturnRepository.GetAll().LastOrDefault();
            return data;
        }

        public PurchaseReturn GetDetailsById(int id)
        {
            var data = _PurchaseReturnRepository.GetById(id);
            return data;
        }

        public PurchaseReturn GetLastReturnBillOfSupplier(string inwardno)
        {
            var data = _PurchaseReturnRepository.GetMany(m => m.InwardNo == inwardno).LastOrDefault();
            return data;
        }

        public PurchaseReturn GetPurchaseByReturnNo(string prno)
        {
            var data = _PurchaseReturnRepository.Get(p => p.PurchaseReturnNo == prno);
            return data;
        }

        public IEnumerable<PurchaseReturn> GetAllPurchaseReturns(string prno)
        {
            var list = _PurchaseReturnRepository.GetMany(p => p.PurchaseReturnNo.ToLower().Contains(prno.ToLower().ToString())).OrderBy(p => p.PurchaseReturnNo);
            return list;
        }

        public IEnumerable<PurchaseReturn> GetDailyPurchaseReturns()
        {
            var list = _PurchaseReturnRepository.GetMany(p => Convert.ToDateTime(p.PurchaseReturnDate).Date == DateTime.Now.Date);
            return list;
        }

        public PurchaseReturn GetLastPurchaseByFinYr(string year, string code)
        {
            var data = _PurchaseReturnRepository.GetMany(p => p.PurchaseReturnNo.Contains(year) && p.Code == code).OrderBy(p => p.PurchaseReturnNo).LastOrDefault();
            return data;
        }
    }
}
