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
    public class IncomeExpenseVoucherService : IIncomeExpenseVoucherService
    {
        private readonly IIncomeExpenseVoucherRepository _IncomeExchangeVoucherRepository;
        private readonly IUnitOfWork _unitOfWork;
        public IncomeExpenseVoucherService(IIncomeExpenseVoucherRepository IncomeExchangeVoucherRepository, IUnitOfWork unitOfWork)
        {
            this._IncomeExchangeVoucherRepository = IncomeExchangeVoucherRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(IncomeExpenseVoucher VoucherEntry)
        {
            _IncomeExchangeVoucherRepository.Add(VoucherEntry);
            _unitOfWork.Commit();
        }

        public IncomeExpenseVoucher GetLastRow()
        {
            var data = _IncomeExchangeVoucherRepository.GetAll().LastOrDefault();
            return data;
        }

        public IEnumerable<IncomeExpenseVoucher> GetEntryByDateAndVoucherType(DateTime date, string type)
        {
            var data = _IncomeExchangeVoucherRepository.GetMany(s => Convert.ToDateTime(s.Date).Date == date.Date && s.VoucherType == type);
            return data;
        }

        public IncomeExpenseVoucher GetLastRowByFinYr(string year)
        {
            var data = _IncomeExchangeVoucherRepository.GetMany(p => p.Code.Contains(year)).OrderBy(p => p.Code).LastOrDefault();
            return data;
        }

        public IEnumerable<IncomeExpenseVoucher> GetAll()
        {
            var data = _IncomeExchangeVoucherRepository.GetAll();
            return data;
        }

        public IEnumerable<IncomeExpenseVoucher> GetDailyIncomeExchangeVoucher()
        {
            var list = _IncomeExchangeVoucherRepository.GetMany(c => Convert.ToDateTime(c.Date).Date == DateTime.Now.Date);
            return list;
        }
    }
}
