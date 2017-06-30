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
    public class InwardFromTailorService : IInwardFromTailorService
    {
         private readonly IInwardFromTailorRepository _InwardFromTailorRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public InwardFromTailorService(IInwardFromTailorRepository InwardFromTailorRepository, IUnitOfWork UnitOfWork)
        {
            this._InwardFromTailorRepository = InwardFromTailorRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public InwardFromTailor GetLastRowrByFinYr(string year)
        {
            var data = _InwardFromTailorRepository.GetMany(p => p.InwardCode.Contains(year)).OrderBy(p => p.InwardCode).LastOrDefault();
            return data;
        }

        public void Create(InwardFromTailor inward)
        {
            _InwardFromTailorRepository.Add(inward);
            _UnitOfWork.Commit();
        }

        public IEnumerable<InwardFromTailor> GetActiveTailor()
        {
            var data = _InwardFromTailorRepository.GetMany(p => p.Status == "Active");
            return data;
        }

    }
}
