using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.DAL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.DAL
{
    public class AwardDaos
    {
        private static IGenericDao<Award> memoryDao;

        private static IGenericDao<Award> fileSystemDao;

        public static IGenericDao<Award> GetInstance(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException();
            }

            switch (key.ToLower())
            {
                case "filesystem":
                    var path = ConfigurationManager.AppSettings.Get("DAL.FileSystem.Award.Path");

                    if (fileSystemDao == null)
                    {
                        fileSystemDao = new FileSystem.GenericDao<Award>(path, new AwardTextConverter());
                    }

                    return fileSystemDao;

                case "memory":
                    if (memoryDao == null)
                    {
                        memoryDao = new Memory.GenericDao<Award>();
                    }

                    return memoryDao;

                default:
                    throw new ArgumentException($"Unknown key '{key}'");
            }
        }
    }
}
