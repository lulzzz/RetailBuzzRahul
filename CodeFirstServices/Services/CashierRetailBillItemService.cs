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
    public class CashierRetailBillItemService:ICashierRetailBillItemService
    {
        private readonly ICashierRetailBillItemRepository _CashierRetailBillItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CashierRetailBillItemService(ICashierRetailBillItemRepository CashierRetailBillItemRepository, IUnitOfWork unitOfWork)
        {
            this._CashierRetailBillItemRepository = CashierRetailBillItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CashierRetailBillItem cash)
        {
            _CashierRetailBillItemRepository.Add(cash);
            _unitOfWork.Commit();
        }

        public IEnumerable<CashierRetailBillItem> GetDetailsByCashierCode(string code)
        {
            var data = _CashierRetailBillItemRepository.GetMany(d => d.CashierCode == code);
            return data;
        }

        public IEnumerable<CashierRetailBillItem> GetRowsByRetailBillNo(string retailno)
        {
            var data = _CashierRetailBillItemRepository.GetMany(d => d.RetailInvoiceNo == retailno);
            return data;
        }
    }
}
