using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
    public class BankAccountTypeService:IBankAccountTypeService
    {
        private readonly IBankAccountTypeRepository _bankAccountTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountTypeService(IBankAccountTypeRepository bankAccountTypeRepository, IUnitOfWork unitOfWork)
        {
            this._bankAccountTypeRepository = bankAccountTypeRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<BankAccountType> getAllBankAccountTypes()
        {
            var BankAccountType = _bankAccountTypeRepository.GetAll();
            return BankAccountType;
        }

        public string GetBankyAccountTypeNamebyId(int id)
        {
            var BankAccountType = _bankAccountTypeRepository.Get(type => type.accountTypeId == id);
            return BankAccountType.accountType;
        }
    }
}
