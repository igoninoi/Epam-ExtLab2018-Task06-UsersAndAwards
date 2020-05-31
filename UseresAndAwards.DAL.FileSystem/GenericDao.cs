using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.DAL.Contracts;

using static System.IO.Path;

namespace UseresAndAwards.DAL.FileSystem
{
    public class GenericDao<Entity> : IGenericDao<Entity>
    {
        private IEntytyTextConverter<Entity> converter;

        public GenericDao(string path, IEntytyTextConverter<Entity> converter)
        {
            if (path == null || converter == null)
            {
                throw new ArgumentNullException();
            }

            if (!IsPathRooted(path))
            {
                throw new ArgumentException($"Needed full (rooted) path to storage dir for {nameof(Entity)}.", "path");
            }

            Directory.CreateDirectory(path);

            this.Path = path;
            this.converter = converter;
        }

        public string Path { get; private set; }

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

            var path = Combine(this.Path, id.ToString());

            if (File.Exists(path))
            {
                errorMessage = $"{nameof(Entity)} object with this ID is exists.";
                return false;
            }

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(path);
                sw.WriteLine(this.converter.ToText(entity));
            }
            catch (Exception ex)
            {
                Log.WriteToLog(ex);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }
            finally
            {
                try
                {
                    sw.Close();
                }
                catch
                {
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool Delete(Guid id, out string errorMessage)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentException("Null or empty GUID", "id");
            }

            var path = Combine(this.Path, id.ToString());

            if (!File.Exists(path))
            {
                errorMessage = $"{nameof(Entity)} object with this ID is not exists.";
                return false;
            }

            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Log.WriteToLog(ex);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool GetAll(out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage)
        {
            var dir = new DirectoryInfo(this.Path);
            if (!dir.Exists)
            {
                collection = new List<KeyValuePair<Guid, Entity>>(0);
                errorMessage = "Sorry. Data access error. Сontact the developer.";
                return false;
            }

            var files = dir.GetFileSystemInfos().OrderBy(file => file.CreationTime);

            var list = new List<KeyValuePair<Guid, Entity>>(0);
            KeyValuePair<Guid, Entity> pair;
            try
            {
                foreach (var file in files)
                {
                    if (!this.TryParseFile(file, out pair, out errorMessage))
                    {
                        collection = new List<KeyValuePair<Guid, Entity>>(0);
                        errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                        return false;
                    }

                    list.Add(pair);
                }
            }
            catch (Exception ex)
            {
                Log.WriteToLog(ex);
                collection = new List<KeyValuePair<Guid, Entity>>(0);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }

            collection = list;
            errorMessage = string.Empty;
            return true;
        }

        public bool GetById(Guid id, out IEnumerable<KeyValuePair<Guid, Entity>> collection, out string errorMessage)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentException("Null or empty GUID", "id");
            }

            var path = Combine(this.Path, id.ToString());

            var list = new List<KeyValuePair<Guid, Entity>>(1);
            KeyValuePair<Guid, Entity> pair;

            var file = new FileInfo(path);
            if (file.Exists)
            {
                if (!this.TryParseFile(file, out pair, out errorMessage))
                {
                    collection = new List<KeyValuePair<Guid, Entity>>(0);
                    errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                    return false;
                }

                list.Add(pair);
            }

            collection = list;
            errorMessage = string.Empty;
            return true;
        }

        private bool TryParseFile(FileSystemInfo file, out KeyValuePair<Guid, Entity> pair, out string errorMessage)
        {
            Guid id;
            if (!Guid.TryParse(file.Name, out id))
            {
                pair = new KeyValuePair<Guid, Entity>();
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }

            Entity entity;
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                if (!this.converter.TryParseFromText(sr.ReadToEnd(), out entity))
                {
                    pair = new KeyValuePair<Guid, Entity>();
                    errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                    return false;
                }
            }

            pair = new KeyValuePair<Guid, Entity>(id, entity);
            errorMessage = string.Empty;
            return true;
        }
    }
}
