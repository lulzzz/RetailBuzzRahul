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
    public class CashHandoverService:ICashHandoverService
    {
        private readonly ICashHandoverRepository _CashHandoverRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashHandoverService(ICashHandoverRepository CashHandoverRepository, IUnitOfWork unitOfWork)
        {
            this._CashHandoverRepository = CashHandoverRepository;
            this._unitOfWork = unitOfWork;
        }

        public CashHandover GetLastRow()
        {
            var row = _CashHandoverRepository.GetAll().LastOrDefault();
            return row;
        }

        public void Create(CashHandover cash)
        {
            _CashHandoverRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashHandover> GetDataByDate(DateTime date)
        {
            var data = _CashHandoverRepository.GetMany(s => Convert.ToDateTime(s.ModifiedOn).Date == date);
            return data;
        }

        public CashHandover GetLastRowByFinYr(string year)
        {
            var data = _CashHandoverRepository.GetMany(p => p.CashHandCode.Contains(year)).OrderBy(p => p.CashHandCode).LastOrDefault();
            return data;
        }

        public IEnumerable<CashHandover> GetAll()
        {
            var data = _CashHandoverRepository.GetAll();
            return data;
        }

        public IEnumerable<CashHandover> GetDailyCashHandover()
        {
            var list = _CashHandoverRepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }
    }
}
