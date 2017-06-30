using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;


namespace CodeFirstServices.Interfaces
{
   public interface IItemTempDetailService
    {
        IEnumerable<itemTempDetail> getAllItems();
        void createItemTempData(itemTempDetail item);
        void updateItemTempData(itemTempDetail item);
        void deleteItemTempData(itemTempDetail item);
        itemTempDetail getById(int id);
        IEnumerable<itemTempDetail> getTempListByDate(string category, string subCategory);
        itemTempDetail CheckForData(BarcodeTempDetail Data);
    }
}
