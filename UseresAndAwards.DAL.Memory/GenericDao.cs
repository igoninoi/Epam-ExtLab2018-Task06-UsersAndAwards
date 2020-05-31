using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.DAL.Contracts;

namespace UseresAndAwards.DAL.Memory
{
    public class GenericDao<Entity> : IGenericDao<Entity>
    {
        private Dictionary<Guid, Entity> storage;

        public GenericDao()
        {
            this.storage = new Dictionary<Guid, Entity>();
        }

        public bool Add(Guid id, Entity entity, out string errorMessage)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentException("Null or empty GUID", "id");
            }

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (this.storage.ContainsKey(id))
            {
                errorMessage = $"{nameof(Entity)} with this ID is alredy exists.";
                return false;
            }

            this.storage.Add(id, entity);

            errorMessage = string.Empty;
            return true;
        }

        public bool Delete(Guid id, out string errorMessage)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentException("Null or empty GUID", "id");
            }

            if (!this.storage.ContainsKey(id))
            {
                errorMessage = $"{nameof(Entity)} with this ID is not exists.";
                return false;
            }

            this.storage.Remove(id);

            errorMessage = string.Empty;
            return true;
        }

        public bool GetAll(out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage)
        {
            collection = this.storage.ToArray();

            errorMessage = string.Empty;
            return true;
        }

        public bool GetById(Guid id, out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentException("Null or empty GUID", "id");
            }

            errorMessage = string.Empty;
            collection = this.storage.Where(x => x.Key == id);
            return true;
        }
    }
}
