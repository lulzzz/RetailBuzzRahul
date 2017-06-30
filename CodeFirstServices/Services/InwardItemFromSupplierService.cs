using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;

namespace CodeFirstServices.Services
{
    public class InwardItemFromSupplierService : IInwardItemFromSupplierService
    {
        private readonly IInwardItemsFromSupplierRepository _InwardItemFromSupplierRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IListOfItemCodeRepository _ListOfItemCodeRepository;
        public InwardItemFromSupplierService(IInwardItemsFromSupplierRepository InwardItemFromSupplierRepository, IUnitOfWork UnitOfWork, IListOfItemCodeRepository ListOfItemCodeRepository)
        {
            this._InwardItemFromSupplierRepository = InwardItemFromSupplierRepository;
            this._UnitOfWork = UnitOfWork;
            this._ListOfItemCodeRepository = ListOfItemCodeRepository;
        }

        public void CreateInwardItems(InwardItemsFromSupplier InwardItemsFromSupplier)
        {
            _InwardItemFromSupplierRepository.Add(InwardItemsFromSupplier);
            _UnitOfWork.Commit();
        }

        public IEnumerable<InwardItemsFromSupplier> GetItemsByPINo(string Pino)
        {
            var details = _InwardItemFromSupplierRepository.GetMany(Data=>Data.InwardNo == Pino);
            return details;
        }

        public IEnumerable<ListOfItemCode> GetItemCodesByPoNo(string ProcName, object[] id)
        {
            var data = _ListOfItemCodeRepository.ExecuteCustomStoredProcByParam(ProcName, id);
            return data;
        }

        public InwardItemsFromSupplier GetItemDetails(string itemCode, string PoNo)
        {
            var data = _InwardItemFromSupplierRepository.Get(m => m.itemCode == itemCode && m.PoNo == PoNo);
            return data;
        }

        public IEnumerable<InwardItemsFromSupplier> GetAllQuantity(string itemCode, string PoNo)
        {
            var data = _InwardItemFromSupplierRepository.GetMany(m => m.itemCode == itemCode && m.PoNo == PoNo);
            return data;
        }


        public void DeleteInwardItems(InwardItemsFromSupplier InwardItemsFromSupplier)
        {
            _InwardItemFromSupplierRepository.Delete(InwardItemsFromSupplier);
            _UnitOfWork.Commit();
        }

        public InwardItemsFromSupplier GetItemDetailsByInwardId(int InwardId)
        {
            var data = _InwardItemFromSupplierRepository.Get(m => m.InwardItemNo == InwardId);
            return data;
        }

        public void Update(InwardItemsFromSupplier InwardItemsFromSupplier)
        {
            _InwardItemFromSupplierRepository.Update(InwardItemsFromSupplier);
            _UnitOfWork.Commit();
        }

        public IEnumerable<InwardItemsFromSupplier> GetInvItemsByInwardNo(string Pino)
        {
            var details = _InwardItemFromSupplierRepository.GetMany(Data => Data.InwardNo == Pino && Data.ItemType=="Inventory");
            return details;
        }

        public InwardItemsFromSupplier GetItemDetailsByItemCode(string ItemCode)
        {
            var details = _InwardItemFromSupplierRepository.Get(i => i.itemCode == ItemCode);
            return details;
        }

        public InwardItemsFromSupplier GetItemByInwardNoAndItemCode(string inwardno, string itemcode)
        {
            var data = _InwardItemFromSupplierRepository.Get(i => i.InwardNo == inwardno && i.itemCode == itemcode);
            return data;
        }

        public IEnumerable<InwardItemsFromSupplier> GetInvItemsByInwardNoForNopO(string Pino)
        {
            var details = _InwardItemFromSupplierRepository.GetMany(Data => Data.InwardNo == Pino && Data.ItemType == "Inventory");
            return details;
        }
    }
}
