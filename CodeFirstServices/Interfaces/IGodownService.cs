using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirstEntities;
using CodeFirstData.DBInteractions;

namespace CodeFirstServices.Interfaces
{
    public interface IGodownService
    {
        GodownMaster getGodownLast();
        void CreateGodown(GodownMaster godownmaster);
        GodownMaster GetGodownDetailsById(int id);
        IEnumerable<GodownMaster> GetEmailList(string email);
        IEnumerable<GodownMaster> GetGodownNames();
        GodownMaster GetGodownByCode(string code);
        void UpdateGodown(GodownMaster godown);
        void DeleteGodown(GodownMaster godown);
        IEnumerable<GodownMaster> GetGodownList(string name);
        IEnumerable<GetUsersEmail> GetUserEmails(string procname);
        IEnumerable<GodownMaster> GetGodownNamesExcludingOne(string name);
        GodownMaster GetGodownDetailsByName(string name);
        GodownMaster CheckShortCode(string Code);
        IEnumerable<GodownMaster> GetAll();
    }
}
