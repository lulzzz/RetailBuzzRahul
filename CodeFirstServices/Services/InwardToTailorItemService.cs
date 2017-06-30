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
    public class InwardToTailorItemService : IInwardToTailorItemService
    {
        private readonly IInwardToTailorItemRepository _InwardToTailorItemRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public InwardToTailorItemService(IInwardToTailorItemRepository InwardToTailorItemRepository, IUnitOfWork UnitOfWork)
        {
            this._InwardToTailorItemRepository = InwardToTailorItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(InwardToTailorItem inward)
        {
            _InwardToTailorItemRepository.Add(inward);
            _UnitOfWork.Commit();
        }

        public IEnumerable<InwardToTailorItem> GetReportByTailorNameAndDate(string Tailor, DateTime fromdate, DateTime todate)
        {
            var data = _InwardToTailorItemRepository.GetMany(d => d.TailorName == Tailor && d.Date.Date >= fromdate.Date && d.Date.Date <= todate.Date);
            return data;
        }

        public IEnumerable<InwardToTailorItem> GetActiveTailorItemsByCode(string code)
        {
            var data = _InwardToTailorItemRepository.GetMany(p => p.InwardCode == code);
            return data;
        }
    }
}
