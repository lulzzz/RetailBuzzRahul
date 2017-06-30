using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ITemporaryCashMemoAdjAmtDetailService
    {
        void Create(TemporaryCashMemoAdjAmtDetail data);
        IEnumerable<TemporaryCashMemoAdjAmtDetail> GetBillsByDate(DateTime date);
        IEnumerable<TemporaryCashMemoAdjAmtDetail> GetBillsByTemporaryCashMemoNo(string no);
    }
}
