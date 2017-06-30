using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;
using System.Linq;

namespace CodeFirstServices.Services
{
    public class BankNameService : IBankNameService
   {

        private readonly IBankNameRepository _bankNameRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BankNameService(IBankNameRepository bankNameRepository, IUnitOfWork unitOfWork)
        {
            this._bankNameRepository = bankNameRepository;
            this._unitOfWork = unitOfWork;
        }

        public void CreateBankName(BankName Bank)
        {
            _bankNameRepository.Add(Bank);
            _unitOfWork.Commit();
        }

       public IEnumerable<BankName> getAllBankNames()
       {
           var bankName = _bankNameRepository.GetAll();
           return bankName;
       }

       public string GetBankyNamebyId(int id)
       {
           var BankName = _bankNameRepository.Get(bank => bank.bankId == id);
           return BankName.bankName;
       }

       public IEnumerable<BankName> GetBankNames(string bankname)
       {
           var banklist = _bankNameRepository.GetMany(bank => bank.bankName.ToLower().StartsWith(bankname.ToString().ToLower())).OrderBy(bank => bank.bankName);
           return banklist;
       }
   }
}
