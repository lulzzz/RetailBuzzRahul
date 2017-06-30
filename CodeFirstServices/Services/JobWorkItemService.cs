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
    public class JobWorkItemService:IJobWorkItemService
    {
        private readonly IJobWorkItemRepository _JobWorkItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JobWorkItemService(IJobWorkItemRepository JobWorkItemRepository, IUnitOfWork unitOfWork)
        {
            this._JobWorkItemRepository = JobWorkItemRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(JobWorkItem item)
        {
            _JobWorkItemRepository.Add(item);
            _unitOfWork.Commit();
        }

        public IEnumerable<JobWorkItem> GetAllItems()
        {
            var list = _JobWorkItemRepository.GetAll();
            return list;
        }

        public JobWorkItem GetLastItem()
        {
            var itemData = _JobWorkItemRepository.GetAll().LastOrDefault();
            return itemData;
        }

        public IEnumerable<JobWorkItem> GetInsertedRow(int LastItemBefore, int LastItemAfter)
        {
            var list = _JobWorkItemRepository.GetMany(l => l.itemId >= LastItemBefore && l.itemId <= LastItemAfter);
            return list;
        }

        public JobWorkItem GetItem(int LastItemBefore)
        {
            var list = _JobWorkItemRepository.Get(l => l.itemId == LastItemBefore);
            return list;
        }
    }
}
