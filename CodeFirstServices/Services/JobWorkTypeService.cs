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
    public class JobWorkTypeService:IJobWorkTypeService
    {
        private readonly IJobWorkTypeRepository _JobWorkTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JobWorkTypeService(IJobWorkTypeRepository JobWorkTypeRepository, IUnitOfWork unitOfWork)
        {
            this._JobWorkTypeRepository = JobWorkTypeRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(JobWorkType type)
        {
            _JobWorkTypeRepository.Add(type);
            _unitOfWork.Commit();
        }

        public JobWorkType GetLastRow()
        {
            var data = _JobWorkTypeRepository.GetAll().LastOrDefault();
            return data;
        }

        public IEnumerable<JobWorkType> GetJobWorkTypeList(string name)
        {
            var list = _JobWorkTypeRepository.GetMany(l1 => l1.Type.ToLower().StartsWith(name.ToString().ToLower())).OrderBy(s => s.Type);
            return list;
        }

        public IEnumerable<JobWorkType> GetJobWorkType(string type)
        {
            var list = _JobWorkTypeRepository.GetMany(n => n.Type == type);
            return list;
        }

        public JobWorkType GetJobWorkTypeById(int id)
        {
            var data = _JobWorkTypeRepository.Get(n => n.Id == id);
            return data;
        }

        public IEnumerable<JobWorkType> GetAll()
        {
            var list = _JobWorkTypeRepository.GetAll();
            return list;
        }
    }
}
