using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
    public class CostCodeCreationService : ICostCodeCreationService
    {
        private readonly ICostCodeCreationRepository _CostCodeCreationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CostCodeCreationService(ICostCodeCreationRepository CostCodeCreationRepository, IUnitOfWork unitOfWork)
        {
            this._CostCodeCreationRepository = CostCodeCreationRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Create(CostCodeCreation code)
        {
            _CostCodeCreationRepository.Add(code);
            _unitOfWork.Commit();
        }

        public void Update(CostCodeCreation code)
        {
            _CostCodeCreationRepository.Update(code);
            _unitOfWork.Commit();
        }

        public IEnumerable<CostCodeCreation> GetActiveData()
        {
            var data = _CostCodeCreationRepository.GetMany(d => d.Status == "Active");
            return data;
        }

        public string GetDetailsByNumber(string no)
        {
            if (no == "1")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_1;
            }
            else if (no == "2")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_2;
            }
            else if (no == "3")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_3;
            }
            else if (no == "4")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_4;
            }
            else if (no == "5")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_5;
            }
            else if (no == "6")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_6;
            }
            else if (no == "7")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_7;
            }
            else if (no == "8")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_8;
            }
            else if (no == "9")
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_9;
            }
            else
            {
                var data = _CostCodeCreationRepository.Get(d => d.Status == "Active");
                return data.Value_0;
            }
        }

    }
}
