using System.Collections.Generic;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;
using System;

namespace CodeFirstServices.Services
{
    public class StateService: IStateService
    {
        private readonly IStateRepository _stateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StateService(IStateRepository stateRepository, IUnitOfWork unitOfWork)
        {
            this._stateRepository = stateRepository;
            this._unitOfWork = unitOfWork;
        }
        public IEnumerable<State> getAllStates()
        {
            var state = _stateRepository.GetAll();
            return state;
        }

        public State GetStatebyId(int id)
        {
            var state = _stateRepository.Get(type => type.Stateid == id);
            return state;
        }

        public string GetStateNamebyId(int id)
        {
            var state = _stateRepository.Get(type => type.Stateid == id);
            return state.StateName;
        }
        public IEnumerable<State> GetStateByCountry(int id)
        {
            var statelist = _stateRepository.GetMany(state => state.Countryid == id);
            return statelist;
        }
        public int GetStateIdByName(string name)
        {
            var id = _stateRepository.Get(m => m.StateName== name);
            return id.Stateid;
        }

        public IEnumerable<State> GetAll()
        {
            var data = _stateRepository.GetAll();
            return data;
        }
    }
}
