using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductBLL.Repositories
{
   public interface IDataRepository <TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task <TEntity> Get(int id);
        void Add(TEntity entity);
        void Update(TEntity dbEntity, int id);
        void Delete(int id);
        string GetOldePhotoPath(int id);
        Task<IEnumerable<TEntity>> Search(string value);

    }
}
