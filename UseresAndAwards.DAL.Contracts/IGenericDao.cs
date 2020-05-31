using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseresAndAwards.DAL.Contracts
{
    public interface IGenericDao<Entity>
    {
        bool Add(Guid id, Entity entity, out string errorMessage);

        bool Delete(Guid id, out string errorMessage);

        bool GetAll(out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage);

        bool GetById(Guid id, out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage);
    }
}
