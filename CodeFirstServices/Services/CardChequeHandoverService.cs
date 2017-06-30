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
    public class CardChequeHandoverService : ICardChequeHandoverService
    {
        private readonly ICardChequeHandoverRepository _CardChequeHandoverRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CardChequeHandoverService(ICardChequeHandoverRepository CardChequeHandoverRepository, IUnitOfWork unitOfWork)
        {
            this._CardChequeHandoverRepository = CardChequeHandoverRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CardChequeHandover cash)
        {
            _CardChequeHandoverRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public void Update(CardChequeHandover cash)
        {
            _CardChequeHandoverRepository.Update(cash);
            _unitOfWork.Commit();
        }

        public void Delete(CardChequeHandover cash)
        {
            _CardChequeHandoverRepository.Delete(cash);
            _unitOfWork.Commit();
        }

        public CardChequeHandover GetLastRow()
        {
            var row = _CardChequeHandoverRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<CardChequeHandover> GetDataByDate(DateTime date)
        {
            var data = _CardChequeHandoverRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date);
            return data;
        }

        public CardChequeHandover GetLastRowByFinYr(string year)
        {
            var data = _CardChequeHandoverRepository.GetMany(p => p.CardChequeHandCode.Contains(year)).OrderBy(p => p.CardChequeHandCode).LastOrDefault();
            return data;
        }

        public IEnumerable<CardChequeHandover> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate)
        {
            var list = _CardChequeHandoverRepository.GetMany(r => Convert.ToDateTime(r.Date).Date >= fromdate.Date && Convert.ToDateTime(r.Date).Date <= todate.Date);
            return list;
        }

        public CardChequeHandover GetDetailsById(int id)
        {
            var row = _CardChequeHandoverRepository.GetById(id);
            return row;
        }

        public IEnumerable<CardChequeHandover> GetAll()
        {
            var data = _CardChequeHandoverRepository.GetAll();
            return data;
        }

        public IEnumerable<CardChequeHandover> GetDailyCardChequeHandover()
        {
            var list = _CardChequeHandoverRepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }
    }
}
