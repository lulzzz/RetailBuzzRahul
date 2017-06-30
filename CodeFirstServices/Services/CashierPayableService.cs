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
    public class CashierPayableService : ICashierPayableService
    {
        private readonly ICashierPayableRepository _CashierPayableRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierPayableService(ICashierPayableRepository CashierPayableRepository, IUnitOfWork unitOfWork)
        {
            this._CashierPayableRepository = CashierPayableRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierPayable cash)
        {
            _CashierPayableRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public CashierPayable GetLastRow()
        {
            var row = _CashierPayableRepository.GetAll().LastOrDefault();
            return row;
        }

        public CashierPayable GetLastCashierByFinYr(string year)
        {
            var data = _CashierPayableRepository.GetMany(p => p.CashierCode.Contains(year)).OrderBy(p => p.CashierCode).LastOrDefault();
            return data;
        }
    }
}
