using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICashierTemporaryCashMemoService
    {
        void Create(CashierTemporaryCashMemo cash);
        void Update(CashierTemporaryCashMemo cash);
        IEnumerable<CashierTemporaryCashMemo> GetRowsByRBNo(string billno);
        CashierTemporaryCashMemo GetDetailsByTempCashMemoNo(string tempcashmemono);
        IEnumerable<CashierTemporaryCashMemo> GetBillsByHandoverStatus();
        CashierTemporaryCashMemo GetRowsByTCMAndCashierNo(string tempcashmemono, string cashierno);
        IEnumerable<CashierTemporaryCashMemo> GetBillsByDateAndTempCashMemoNo(DateTime date, string billno);
        IEnumerable<CashierTemporaryCashMemo> GetAll();
        IEnumerable<CashierTemporaryCashMemo> GetDailyCashierTempCashMemo();
        IEnumerable<CashierTemporaryCashMemo> GetActivecashierTemporaryCashMemo(string orderno);
        IEnumerable<CashierTemporaryCashMemo> GetHandoverTemporaryCashMemoService();
    }
}
