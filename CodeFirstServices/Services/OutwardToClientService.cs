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
    public class OutwardToClientService : IOutwardToClientService
    {
        private readonly IOutwardToClientRepository _outwardtoclientRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OutwardToClientService(IOutwardToClientRepository outwardtoclientRepository, IUnitOfWork unitOfWork)
        {
            this._outwardtoclientRepository = outwardtoclientRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(OutwardToClient data)
        {
            _outwardtoclientRepository.Add(data);
            _unitOfWork.Commit();
        }

        public OutwardToClient GetLastRow()
        {
            var row = _outwardtoclientRepository.GetAll().LastOrDefault();
            return row;
        }

        public IEnumerable<OutwardToClient> GetOutWardToClientByDate()
        {
            var data = _outwardtoclientRepository.GetMany(o => Convert.ToDateTime(o.Date).Date == DateTime.Now.Date);
            return data;
        }

        public IEnumerable<OutwardToClient> GetDetailsByDate(DateTime fromdate, DateTime todate)
        {
            var value = _outwardtoclientRepository.GetMany(cl => Convert.ToDateTime(cl.Date).Date >= fromdate.Date && Convert.ToDateTime(cl.Date).Date <= todate.Date);
            return value;
        }

        public IEnumerable<OutwardToClient> GetReportByClientAndDate(string Client, DateTime fromdate, DateTime todate)
        {
            var data = _outwardtoclientRepository.GetMany(d => d.ClientName == Client && Convert.ToDateTime(d.Date).Date >= fromdate.Date && Convert.ToDateTime(d.Date).Date <= todate.Date);
            return data;
        }

        public OutwardToClient GetDetailsById(int id)
        {
            var data = _outwardtoclientRepository.Get(d => d.OutwardId == id);
            return data;
        }

        public IEnumerable<OutwardToClient> GetOutwardNo(string term, string code)
        {
            var details = _outwardtoclientRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.ShopCode == code).OrderBy(m => m.OutwardCode);
            return details;
        }

        public IEnumerable<OutwardToClient> GetOutwardNoForGodown(string term, string code)
        {
            var details = _outwardtoclientRepository.GetMany(m => m.OutwardCode.ToLower().Contains(term.ToLower()) && m.GodownCode == code).OrderBy(m => m.OutwardCode);
            return details;
        }

        public OutwardToClient GetDetailsByOutwardCode(string OutwardNo)
        {
            var details = _outwardtoclientRepository.Get(o => o.OutwardCode == OutwardNo);
            return details;
        }

        public OutwardToClient GetLastOutwardByFinYr(string year,string Code)
        {
            var data = _outwardtoclientRepository.GetMany(o => o.OutwardCode.Contains(year) && (o.GodownCode == Code || o.ShopCode == Code)).OrderBy(o => o.OutwardCode).LastOrDefault();
            return data;
        }
    }
}
