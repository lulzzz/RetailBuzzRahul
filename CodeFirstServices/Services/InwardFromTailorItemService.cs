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
    public class InwardFromTailorItemService : IInwardFromTailorItemService
    {
        private readonly IInwardFromTailorItemRepository _InwardFromTailorItemRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public InwardFromTailorItemService(IInwardFromTailorItemRepository InwardFromTailorItemRepository, IUnitOfWork UnitOfWork)
        {
            this._InwardFromTailorItemRepository = InwardFromTailorItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(InwardFromTailorItem inward)
        {
            _InwardFromTailorItemRepository.Add(inward);
            _UnitOfWork.Commit();
        }


        public IEnumerable<InwardFromTailorItem> GetActiveTailorItems(string code)
        {
            var data = _InwardFromTailorItemRepository.GetMany(p => p.Status == "Active" && p.InwardCode==code);
            return data;
        }

        public IEnumerable<InwardFromTailorItem> GetActiveTailorItemsByCode(string code)
        {
            var data = _InwardFromTailorItemRepository.GetMany(p=>p.InwardCode == code);
            return data;
        }

    }
}
