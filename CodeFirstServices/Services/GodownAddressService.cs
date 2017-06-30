using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstData.DBInteractions;
using CodeFirstData.EntityRepositories;
using CodeFirstEntities;
using CodeFirstServices.Interfaces;

namespace CodeFirstServices.Services
{
    public class GodownAddressService : IGodownAddressService
    {
        private readonly IGodownAddressRepository _godownaddressRepository;
        private readonly IUnitOfWork _unitofwork;

        public GodownAddressService(IGodownAddressRepository godownaddressRepository, IUnitOfWork unitofwork)
        {
            this._godownaddressRepository = godownaddressRepository;
            this._unitofwork = unitofwork;
        }

        public void CreateGoDown(GodownAddress godownAddrData)
        {
            _godownaddressRepository.Add(godownAddrData);
            _unitofwork.Commit();
        }

        public void DeleteGoDownAddr(string gdCode)
        {
            var godownAddr = _godownaddressRepository.GetMany(gd => gd.GdCode == gdCode);
            foreach (var item in godownAddr)
            {
                _godownaddressRepository.Delete(item);
                _unitofwork.Commit();
            }
        }
        public GodownAddress GetAddressDetailsById(int id)
        {
            var details = _godownaddressRepository.Get(d => d.AddressId == id);
            return details;
        }
        public IEnumerable<GodownAddress> GetAddressList(string GdCode)
        {
            var list = _godownaddressRepository.GetMany(l => l.GdCode == GdCode);
            return list;
        }

        public GodownAddress GetLastAddress()
        {
            var row = _godownaddressRepository.GetAll().LastOrDefault();
            return row;
        }

        public GodownAddress GetAddressByArea(string area)
        {
            var data = _godownaddressRepository.Get(gd => gd.AreaName == area);
            return data;
        }

        public void UpdateGodown(GodownAddress godown)
        {
            _godownaddressRepository.Update(godown);
            _unitofwork.Commit();
        }

        public IEnumerable<GodownAddress> GetAllAddressesByCode(string code)
        {
            var details = _godownaddressRepository.GetMany(m => m.GdCode == code);
            return details;
        }

        public GodownAddress GetAddressByAreaByCode(string GdCode, string area)
        {
            var details = _godownaddressRepository.Get(m => m.GdCode == GdCode && m.AreaName == area);
            return details;
        }

        public IEnumerable<GodownAddress> GetAllGodownAddresses()
        {
            var list = _godownaddressRepository.GetAll();
            return list;
        }
    }
}
