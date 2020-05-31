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
    public class UserDaos
    {
        private static IGenericDao<User> memoryDao;

        private static IGenericDao<User> fileSystemDao;

        public static IGenericDao<User> GetInstance(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException();
            }

            switch (key.ToLower())
            {
                case "filesystem":
                    var path = ConfigurationManager.AppSettings.Get("DAL.FileSystem.User.Path");

                    if (fileSystemDao == null)
                    {
                        fileSystemDao = new FileSystem.GenericDao<User>(path, new UserTextConverter());
                    }

                    return fileSystemDao;

                case "memory":
                    if (memoryDao == null)
                    {
                        memoryDao = new Memory.GenericDao<User>();
                    }

                    return memoryDao;

                default:
                    throw new ArgumentException($"Unknown key '{key}'");
            }
        }
    }
}
