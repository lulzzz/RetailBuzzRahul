using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IGodownStockService
    {
        void Create(GodownStock godownstock);
        void Update(GodownStock godownstock);
        IEnumerable<GodownStock> GetGodownStockTillDate();
        GodownStock GetDetailsByItemCodeAndGodownCode(string itemcode, string godowncode);
        IEnumerable<GodownStock> GetRowsByItemCode(string itemcode);
        IEnumerable<GodownStock> GetRowsByGodownCode(string code);
        IEnumerable<GodownStock> getGodownStock();
        IEnumerable<GodownStock> GetRowsByGodownName(string name);
        IEnumerable<GodownStock> GetNamesByItem(string name, string godownname);
        IEnumerable<GodownStock> GetRowsByItemnameandShopName(string itemname, string godownname);
        IEnumerable<GodownStock> getItemDetailsByDate(DateTime fromdate, DateTime todate);
        IEnumerable<GodownStock> getItemDetailsByDateAndItemName(DateTime fromdate, DateTime todate, string itemname);
        IEnumerable<GodownStock> getGodownStockNameByItemName(string name);
        IEnumerable<GodownStock> GetRowsByGodownItemName(string name);
        GodownStock GetRowsByItemCodeandShopName(string code, string godownname);
        IEnumerable<GodownStock> GetRowsByGodowCode(string code);
        IEnumerable<GodownStock> GetDetailsByItemCodeAndGodownCodeForDynamicList(string itemcode, string godowncode);
        IEnumerable<GodownStock> GetRowsByItemcodeandItemname(string itemcode, string itemname, string godownname);
        IEnumerable<GodownStock> GetRowsByItemcodeandItemnameWithoutGodownName(string itemcode, string itemname);
    }
}
