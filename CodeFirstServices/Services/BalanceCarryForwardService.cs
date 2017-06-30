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
    public class BalanceCarryForwardService : IBalanceCarryForwardService
    {
        private readonly IBalanceCarryForwardRepository _BalanceCarryForwardRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BalanceCarryForwardService(IBalanceCarryForwardRepository BalanceCarryForwardRepository, IUnitOfWork unitOfWork)
        {
            this._BalanceCarryForwardRepository = BalanceCarryForwardRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(BalanceCarryForward data)
        {
            _BalanceCarryForwardRepository.Add(data);
            _unitOfWork.Commit();
        }

        public BalanceCarryForward GetLastRow()
        {
            var row = _BalanceCarryForwardRepository.GetAll().LastOrDefault();
            return row;
        }

        public BalanceCarryForward GetDataByDate(DateTime date)
        {
            var data = _BalanceCarryForwardRepository.Get(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public BalanceCarryForward GetLastRowByFinYr(string year)
        {
            var data = _BalanceCarryForwardRepository.GetMany(p => p.CarryForwardCode.Contains(year)).OrderBy(p => p.CarryForwardCode).LastOrDefault();
            return data;
        }

        public BalanceCarryForward GetPendingBalances()
        {
            var data = _BalanceCarryForwardRepository.Get(s => Convert.ToDateTime(s.Date).Date == DateTime.Now);
            return data;
        }
    }
}
