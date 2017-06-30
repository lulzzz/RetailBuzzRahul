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
    public class TransportBankDetailService : ITransportBankDetailService
    {
        private readonly ITransportBankDetailRepository _TransportBankDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public TransportBankDetailService(ITransportBankDetailRepository TransportBankDetailRepository, IUnitOfWork UnitOfWork)
        {
            this._TransportBankDetailRepository = TransportBankDetailRepository;
            this._UnitOfWork = UnitOfWork;
        }

        public void CreateBankDetails(TransportBankDetail TransportBankDetail)
        {
            _TransportBankDetailRepository.Add(TransportBankDetail);
            _UnitOfWork.Commit();
        }

        public void UpdateBankDetails(TransportBankDetail TransportBankDetail)
        {
            _TransportBankDetailRepository.Update(TransportBankDetail);
            _UnitOfWork.Commit();
        }

        public IEnumerable<TransportBankDetail> GetDetailsFromBank(string transportcode)
        {
            var banklist = _TransportBankDetailRepository.GetMany(code => code.TransportCode == transportcode);
            return banklist;
        }

        public void ExecuteProcedure(string transportcode)
        {
            var bankList = _TransportBankDetailRepository.GetMany(bnk => bnk.TransportCode == transportcode);
            foreach (var item in bankList)
            {
                _TransportBankDetailRepository.Delete(item);
                _UnitOfWork.Commit();
            }
        }

        public TransportBankDetail GetBankDetailsOfTransport(int selectedbankname)
        {
            var banklist = _TransportBankDetailRepository.Get(bank => bank.BankId == selectedbankname);
            return banklist;
        }

        public IEnumerable<TransportBankDetail> GetAllTransportBanks()
        {
            var list = _TransportBankDetailRepository.GetAll();
            return list;
        }
    }
}
