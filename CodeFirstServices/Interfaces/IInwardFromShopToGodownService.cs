using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IInwardFromShopToGodownService
    {
        InwardFromShopToGodown GetLastInward();
        void Create(InwardFromShopToGodown Inward);
        void Update(InwardFromShopToGodown Inward);
        InwardFromShopToGodown GetInwardDetailById(int id);
        IEnumerable<InwardFromShopToGodown> GetDetailsByDate(DateTime FromDate,DateTime ToDate);
        IEnumerable<InwardFromShopToGodown> GetDetailsByShopAndDate(string Shop, DateTime FromDate, DateTime ToDate);
        IEnumerable<InwardFromShopToGodown> GetInwardNo(string term, string godowncode);
        InwardFromShopToGodown GetDetailsByInwardCode(string InwardCode);
        IEnumerable<InwardFromShopToGodown> GetDebitNoteNos(string term);
        InwardFromShopToGodown GetDataByDebitNoteNo(string debitnoteno);
        IEnumerable<InwardFromShopToGodown> GetDailyInwardsShopToGodown();
        InwardFromShopToGodown GetLastInwardByFinYear(string Year,string GdCode);
    }
}
