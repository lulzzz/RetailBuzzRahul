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
    public class EntryStockService:IEntryStockService
    {
        private readonly IEntryStockRepository _EntryStockMasterRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public EntryStockService(IEntryStockRepository EntryStockMasterRepository, IUnitOfWork UnitOfWork)
        {
            this._EntryStockMasterRepository = EntryStockMasterRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(EntryStockMaster tax)
        {
            _EntryStockMasterRepository.Add(tax);
            _UnitOfWork.Commit();
        }

        public EntryStockMaster GetLastRow()
        {
            var row = _EntryStockMasterRepository.GetAll().LastOrDefault();
            return row;
        }

        public EntryStockMaster GetDataByStockCode(string entrystockcode)
        {
            var data = _EntryStockMasterRepository.Get(c => c.EntryStockCode == entrystockcode);
            return data;
        }

        public EntryStockMaster GetLastStockByFinYr(string year)
        {
            var data = _EntryStockMasterRepository.GetMany(p => p.EntryStockCode.Contains(year)).OrderBy(p => p.EntryStockCode).LastOrDefault();
            return data;
        }

        public IEnumerable<EntryStockMaster> GetItemsByStatus()
        {
            var data = _EntryStockMasterRepository.GetMany(d => d.Status == "Active");
            return data;
        }
    }
}
