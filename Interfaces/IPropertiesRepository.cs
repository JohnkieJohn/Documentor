using System.Data.SqlClient;

namespace Documentor.Interfaces
{
    public interface IPropertiesRepository<T> where T : class
    {
        T GetById(int id, SqlConnection activeCon);
        void Create(T entity, SqlConnection activeCon);
        void Update(T entity, SqlConnection activeCon);
        void Delete(T entity, SqlConnection activeCon);
    }
}
