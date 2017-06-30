using CodeFirstEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstServices.Interfaces
{
    public interface IStateService
    {
        IEnumerable<State> getAllStates();
        State GetStatebyId(int id);
        IEnumerable<State> GetStateByCountry(int id);
        string GetStateNamebyId(int id);
        int GetStateIdByName(string name);
        IEnumerable<State> GetAll();
    }
}
