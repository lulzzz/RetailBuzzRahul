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
    public class DebitNoteService : IDebitNoteService
    {
        private readonly IDebitNoteRepository _DebitNoteRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public DebitNoteService(IDebitNoteRepository DebitNoteRepository, IUnitOfWork UnitOfWork)
        {
            this._DebitNoteRepository = DebitNoteRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void Create(DebitNote DebitNote)
        {
            _DebitNoteRepository.Add(DebitNote);
            _UnitOfWork.Commit();
        }

        public void Update(DebitNote DebitNote)
        {
            _DebitNoteRepository.Update(DebitNote);
            _UnitOfWork.Commit();
        }

        public IEnumerable<DebitNote> GetDebitNoteListByInwardNo(string inwardno)
        {
            var data = _DebitNoteRepository.GetMany(d => d.InwardNo == inwardno && d.Status == "Active");
            return data;
        }

        public IEnumerable<DebitNote> GetDebitNotesPending()
        {
            var data = _DebitNoteRepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public IEnumerable<DebitNote> GetAllDebitNotes()
        {
            var data = _DebitNoteRepository.GetAll();
            return data;
        }

        public IEnumerable<DebitNote> GetDebitNotesBySupplier(string suppliername)
        {
            var data = _DebitNoteRepository.GetMany(c => c.SupplierName == suppliername);
            return data;
        }

        public IEnumerable<DebitNote> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate)
        {
            var data = _DebitNoteRepository.GetMany(d => Convert.ToDateTime(d.DebitNoteDate).Date >= fromdate.Date && Convert.ToDateTime(d.DebitNoteDate).Date <= todate.Date);
            return data;
        }

        public IEnumerable<DebitNote> GetRowsByFromAndToDateAndSupplier(DateTime fromdate, DateTime todate, string supplier)
        {
            var data = _DebitNoteRepository.GetMany(d => Convert.ToDateTime(d.DebitNoteDate).Date >= fromdate.Date && Convert.ToDateTime(d.DebitNoteDate).Date <= todate.Date && d.SupplierName == supplier);
            return data;
        }
    }
}
