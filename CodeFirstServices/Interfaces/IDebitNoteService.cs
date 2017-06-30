using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IDebitNoteService
    {
        void Create(DebitNote DebitNote);
        void Update(DebitNote DebitNote);
        IEnumerable<DebitNote> GetDebitNoteListByInwardNo(string inwardno);
        IEnumerable<DebitNote> GetDebitNotesPending();
        IEnumerable<DebitNote> GetAllDebitNotes();
        IEnumerable<DebitNote> GetDebitNotesBySupplier(string suppliername);
        IEnumerable<DebitNote> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate);
        IEnumerable<DebitNote> GetRowsByFromAndToDateAndSupplier(DateTime fromdate, DateTime todate, string supplier);
    }
}
