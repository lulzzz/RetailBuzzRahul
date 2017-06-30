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
    public class InwardToTailorService : IInwardToTailorService
    {
        private readonly IInwardToTailorRepository _InwardToTailorRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public InwardToTailorService(IInwardToTailorRepository InwardToTailorRepository, IUnitOfWork UnitOfWork)
        {
            this._InwardToTailorRepository = InwardToTailorRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(InwardToTailor inward)
        {
            _InwardToTailorRepository.Add(inward);
            _UnitOfWork.Commit();
        }

        public InwardToTailor GetLastRowrByFinYr(string year)
        {
            var data = _InwardToTailorRepository.GetMany(p => p.InwardCode.Contains(year)).OrderBy(p => p.InwardCode).LastOrDefault();
            return data;
        }

        public IEnumerable<InwardToTailor> GetReportByTailorNameAndDate(string Tailor, DateTime fromdate, DateTime todate)
        {
            var data = _InwardToTailorRepository.GetMany(d => d.TailorName == Tailor && d.Date.Date >= fromdate.Date && d.Date.Date <= todate.Date);
            return data;
        }


        public IEnumerable<InwardToTailor> GetActiveTailor()
        {
            var data = _InwardToTailorRepository.GetMany(p => p.Status == "Active");
            return data;
        }
    }
}
