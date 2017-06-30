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
    public class CashierReceivableService : ICashierReceivableService
    {
        private readonly ICashierReceivableRepository _CashierReceivableRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierReceivableService(ICashierReceivableRepository CashierReceivableRepository, IUnitOfWork unitOfWork)
        {
            this._CashierReceivableRepository = CashierReceivableRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierReceivable cash)
        {
            _CashierReceivableRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public CashierReceivable GetLastRow()
        {
            var row = _CashierReceivableRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<CashierReceivable> GetDetailsByDate()
        {
            var data = _CashierReceivableRepository.GetMany(d =>Convert.ToDateTime(d.ModifiedOn).Date == DateTime.Now.Date);
            return data;
        }

        public IEnumerable<CashierReceivable> GetAll()
        {
            var data = _CashierReceivableRepository.GetAll();
            return data;
        }

        public CashierReceivable GetDetailsById(int id)
        {
            var row = _CashierReceivableRepository.Get(r => r.Id == id);
            return row;
        }

        public IEnumerable<CashierReceivable> GetDetailsByBillNo(string billno)
        {
            var row = _CashierReceivableRepository.GetMany(r => r.Billno == billno);
            return row;
        }

        public CashierReceivable GetLastCashierByFinYr(string year)
        {
            var data = _CashierReceivableRepository.GetMany(p => p.CashierCode.Contains(year)).OrderBy(p => p.CashierCode).LastOrDefault();
            return data;
        }
    }
}
