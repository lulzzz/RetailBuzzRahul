using System;
using System.Collections.Generic;

namespace CodeFirstData.DBInteractions
{
public interface IEntityRepository<T> where T : class
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Delete(Func<T, Boolean> predicate);
    T GetById(long Id);
    T Get(Func<T, Boolean> where);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetMany(Func<T, bool> where);
    T GetByName(string name);
    List<T> ExecuteCustomStoredProcByParam(string procName, object[] parameter);
    List<T> ExecuteCustomStoredProc(string procName);
    
}
}
