using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstEntities;

namespace CodeFirstServices.Interfaces
{
    public interface IGodownAddressService
    {
        void CreateGoDown(GodownAddress godownAddrData);
        void DeleteGoDownAddr(string gdCode);
        GodownAddress GetAddressDetailsById(int id);
        IEnumerable<GodownAddress> GetAddressList(string GdCode);
        GodownAddress GetLastAddress();
        GodownAddress GetAddressByArea(string area);
        GodownAddress GetAddressByAreaByCode(string GdCode, string area);
        void UpdateGodown(GodownAddress godown);
        IEnumerable<GodownAddress> GetAllAddressesByCode(string code);
        IEnumerable<GodownAddress> GetAllGodownAddresses();
    }
}
