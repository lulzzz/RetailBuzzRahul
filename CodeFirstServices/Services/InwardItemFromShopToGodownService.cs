using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;

namespace CodeFirstServices.Services
{
    public class InwardItemFromShopToGodownService : IInwardItemFromShopToGodownService
    {
        private readonly IInwardItemFromShopToGodownRepository _InwardItemFromShopToGodownRepository;
    private readonly IUnitOfWork _unitOfWork;
    public InwardItemFromShopToGodownService(IInwardItemFromShopToGodownRepository InwardItemFromShopToGodownRepository, IUnitOfWork unitOfWork)
    {
        this._InwardItemFromShopToGodownRepository = InwardItemFromShopToGodownRepository;
        this._unitOfWork = unitOfWork;
    }

        public void Create(InwardItemFromShopToGodown Inward)
        {
            _InwardItemFromShopToGodownRepository.Add(Inward);
            _unitOfWork.Commit();
        }

        public void Update(InwardItemFromShopToGodown Inward)
        {
            _InwardItemFromShopToGodownRepository.Update(Inward);
            _unitOfWork.Commit();
        }

        public IEnumerable<InwardItemFromShopToGodown> GetItemsByInwardNo(string InwardNo)
        {
            var details = _InwardItemFromShopToGodownRepository.GetMany(i => i.InwardCode == InwardNo && i.Status == "Active");
            return details;
        }
    }
}
