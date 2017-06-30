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
    public class DebitNoteItemService : IDebitNoteItemService
    {
        private readonly IDebitNoteItemRepository _DebitNoteItemRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public DebitNoteItemService(IDebitNoteItemRepository DebitNoteItemRepository, IUnitOfWork UnitOfWork)
        {
            this._DebitNoteItemRepository = DebitNoteItemRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(DebitNoteItem DebitNoteItem)
        {
            _DebitNoteItemRepository.Add(DebitNoteItem);
            _UnitOfWork.Commit();
        }

        public IEnumerable<DebitNoteItem> GetDebitNotesItemPending(string dbno)
        {
            var data = _DebitNoteItemRepository.GetMany(d => d.DebitNoteNo == dbno && d.ItemType == "Inventory");
            return data;
        }
    }
}
