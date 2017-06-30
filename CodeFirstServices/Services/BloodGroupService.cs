using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System.Linq;
using System;


namespace CodeFirstServices.Services
{
    public class BloodGroupService : IBloodGroupService
    {
        private readonly IBloodGroupRepository _BloodGroupRepository;
        private readonly IUnitOfWork _unityOfWork;
        public BloodGroupService(IBloodGroupRepository BloodGroupRepository, IUnitOfWork unityOfWork)
        {
            this._BloodGroupRepository = BloodGroupRepository;
            this._unityOfWork = unityOfWork;
        }

        public IEnumerable<BloodGroup> GetBloodGroup()
        {
            var BloodGroup = _BloodGroupRepository.GetAll();
            return BloodGroup;
        }

        public string GetTypeOfSupplier(int id)
        {
            var BloodGroup = _BloodGroupRepository.Get(B => B.bloodGroupID == id);
            return BloodGroup.bloodGroupType;
        }

        public BloodGroup GetBloodGroupById(int id)
        {
            var BloodGroup = _BloodGroupRepository.GetById(id);
            return BloodGroup;
        }
    }
}
