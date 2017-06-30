using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICardChequeHandoverService
    {
        void Create(CardChequeHandover cash);
        void Update(CardChequeHandover cash);
        void Delete(CardChequeHandover cash);
        CardChequeHandover GetLastRow();
        IEnumerable<CardChequeHandover> GetDataByDate(DateTime date);
        CardChequeHandover GetLastRowByFinYr(string year);
        IEnumerable<CardChequeHandover> GetRowsByFromAndToDate(DateTime fromdate, DateTime todate);
        CardChequeHandover GetDetailsById(int id);
        IEnumerable<CardChequeHandover> GetAll();
        IEnumerable<CardChequeHandover> GetDailyCardChequeHandover();
    }
}
