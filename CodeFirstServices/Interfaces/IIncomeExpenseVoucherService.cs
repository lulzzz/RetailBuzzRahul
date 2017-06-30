using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IIncomeExpenseVoucherService
    {
        void Create(IncomeExpenseVoucher VoucherEntry);
        IncomeExpenseVoucher GetLastRow();
        IEnumerable<IncomeExpenseVoucher> GetEntryByDateAndVoucherType(DateTime date, string type);
        IncomeExpenseVoucher GetLastRowByFinYr(string year);
        IEnumerable<IncomeExpenseVoucher> GetAll();
        IEnumerable<IncomeExpenseVoucher> GetDailyIncomeExchangeVoucher();
    }
}
