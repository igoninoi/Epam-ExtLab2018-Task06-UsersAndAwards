using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseresAndAwards.BLL.Contracts
{
    public interface IEntityLogic<Entity>
    {
        bool Add(Entity entity, out string errorMessage);

        bool Delete(Guid id, out string errorMessage);

        bool GetAll(out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage);
    }
}
