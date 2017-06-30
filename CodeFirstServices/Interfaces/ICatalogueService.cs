using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface ICatalogueService
    {
        Catalogue GetLastCatalogue();
        void Create(Catalogue catalogue);
        Catalogue GetCatalogue(int LastCatalogueBefore);
        IEnumerable<Catalogue> GetInsertedRow(int LastCatalogueBefore, int LastCatalogueAfter);
        Catalogue GetRowByBarcode(string Barcode);
        IEnumerable<Catalogue> GetAll();
        Catalogue GetRowByItemCode(string ItemCode);
        IEnumerable<Catalogue> GetCataloguesBySubCategory(string SubCategory);
        void Update(Catalogue catalogue);
        IEnumerable<Catalogue> GetDataByBookNumber(string BookNo);
        void ActualDelete(Catalogue catalogue);
    }
}
