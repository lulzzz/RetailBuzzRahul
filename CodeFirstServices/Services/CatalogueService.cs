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
    public class CatalogueService : ICatalogueService
    {
         private readonly ICatalogueRepository _CatalogueRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CatalogueService(ICatalogueRepository CatalogueRepository, IUnitOfWork unitOfWork)
        {
            this._CatalogueRepository = CatalogueRepository;
            this._unitOfWork = unitOfWork;
        }

        public Catalogue GetLastCatalogue()
        {
            var details = _CatalogueRepository.GetAll().LastOrDefault();
            return details;
        }
        
        public void Create(Catalogue catalogue)
        {
            _CatalogueRepository.Add(catalogue);
            _unitOfWork.Commit();
        }

        public Catalogue GetCatalogue(int LastCatalogueBefore)
        {
            var list = _CatalogueRepository.Get(l => l.Id == LastCatalogueBefore);
            return list;
        }

        public IEnumerable<Catalogue> GetInsertedRow(int LastCatalogueBefore, int LastCatalogueAfter)
        {
            var list = _CatalogueRepository.GetMany(l => l.Id >= LastCatalogueBefore && l.Id <= LastCatalogueAfter);
            return list;
        }

        public Catalogue GetRowByBarcode(string Barcode)
        {
            var details = _CatalogueRepository.Get(ct => ct.CatalogueBarcode == Barcode);
            return details;
        }

        public IEnumerable<Catalogue> GetAll()
        {
            var details = _CatalogueRepository.GetAll();
            return details;
        }

        public Catalogue GetRowByItemCode(string ItemCode)
        {
            var details = _CatalogueRepository.Get(cat => cat.ItemCode == ItemCode);
            return details;
        }

        public IEnumerable<Catalogue> GetCataloguesBySubCategory(string SubCategory)
        {
            var details = _CatalogueRepository.GetMany(c => c.Subcategory == SubCategory && c.Status == "Active");
            return details;
        }

        public void Update(Catalogue catalogue)
        {
            _CatalogueRepository.Update(catalogue);
            _unitOfWork.Commit();
        }

        public IEnumerable<Catalogue> GetDataByBookNumber(string BookNo)
        {
            var data = _CatalogueRepository.GetMany(m => m.BookNumber == BookNo);
            return data;
        }

        public void ActualDelete(Catalogue catalogue)
        {
            _CatalogueRepository.Delete(catalogue);
            _unitOfWork.Commit();
        }
    }
}
