using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstServices.Interfaces;
using CodeFirstEntities;
using CodeFirstData.EntityRepositories;
using CodeFirstData.DBInteractions;

namespace CodeFirstServices.Services
{
    public class SupplierBankDetailService : ISupplierBankDetailService
    {
        private readonly ISupplierBankDetailRepository _SupplierBankDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public SupplierBankDetailService(ISupplierBankDetailRepository SupplierBankDetailRepository, IUnitOfWork UnitOfWork)
        {
            this._SupplierBankDetailRepository = SupplierBankDetailRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void CreateBankDetails(SupplierBankDetail supplierbankdetail)
        {
            _SupplierBankDetailRepository.Add(supplierbankdetail);
            _UnitOfWork.Commit();
        }

        public void UpdateBankDetails(SupplierBankDetail supplierbankdetail)
        {
            _SupplierBankDetailRepository.Update(supplierbankdetail);
            _UnitOfWork.Commit();
        }

        public IEnumerable<SupplierBankDetail> GetDetailsFromBank(string suppliercode)
        {
            var banklist = _SupplierBankDetailRepository.GetMany(code => code.SupplierCode == suppliercode);
            return banklist;
        }

        public void ExecuteProcedure(string suppliercode)
        {
            var bankList = _SupplierBankDetailRepository.GetMany(bnk => bnk.SupplierCode == suppliercode);
            foreach (var item in bankList)
            {
                _SupplierBankDetailRepository.Delete(item);
                _UnitOfWork.Commit();
            }
        }

        public SupplierBankDetail GetBankDetailsOfSupplier(int selectedbankname)
        {
            var banklist = _SupplierBankDetailRepository.Get(bank => bank.BankId == selectedbankname);
            return banklist;
        }

        public IEnumerable<SupplierBankDetail> GetAllSupplierBanks()
        {
            var list = _SupplierBankDetailRepository.GetAll();
            return list;
        }
    }
}
