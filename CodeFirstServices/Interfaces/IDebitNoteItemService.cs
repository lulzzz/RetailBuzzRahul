using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IDebitNoteItemService
    {
        void Create(DebitNoteItem DebitNoteItem);
        IEnumerable<DebitNoteItem> GetDebitNotesItemPending(string dbno);
    }
}
