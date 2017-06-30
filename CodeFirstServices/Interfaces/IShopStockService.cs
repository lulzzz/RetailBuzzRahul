using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IShopStockService
    {
        void Create(ShopStock shopstockitem);
        void Update(ShopStock shopstockitem);
        ShopStock GetDetailsByItemCode(string code);
        ShopStock GetDetailsByBarcode(string barcode);
        IEnumerable<ShopStock> GetRowsByShopCode(string shopcode);
        ShopStock GetDetailsByItemCodeAndShopCode(string itemcode, string shopcode);
        ShopStock GetLastRowFromItemAndShopCode(string itemcode, string shopcode);
        IEnumerable<ShopStock> GetRowsByItemCode(string code);
        IEnumerable<ShopStock> GetShopStockTillDate();
        ShopStock GetLastRow();
        ShopStock GetDetailsByShopCode(string code);
        IEnumerable<ShopStock> getShopStock();
        IEnumerable<ShopStock> GetNamesByItem(string name, string shopname);
        IEnumerable<ShopStock> GetRowsByItemnameandShopName(string itemname, string shopname);
        IEnumerable<ShopStock> getItemDetailsByDate(DateTime fromdate, DateTime todate);
        IEnumerable<ShopStock> getItemDetailsByDateAndItemName(DateTime fromdate, DateTime todate, string itemname);
        IEnumerable<ShopStock> getStockByItemName(string name);
        IEnumerable<ShopStock> getShopStockByItemName(string name);
        ShopStock GetDetailsByItemCodeAndShopName(string itemcode, string shopname);
        IEnumerable<ShopStock> GetDetailsByItemCodeAndShopCodeForDynamicList(string itemcode, string shopcode);
        IEnumerable<ShopStock> GetRowsByShopCodeFoRrequsition(string shopcode);
        IEnumerable<ShopStock> GetRowsByItemcodeandItemname(string itemcode, string itemname, string shopname);
        IEnumerable<ShopStock> GetRowsByItemcodeandItemnameWithoutshopname(string itemcode, string itemname);
    }
}
