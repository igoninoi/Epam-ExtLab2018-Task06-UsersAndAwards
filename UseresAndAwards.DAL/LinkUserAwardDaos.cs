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
    public class LinkUserAwardDaos
    {
        private static IGenericDao<LinkUserAward> memoryDao;

        private static IGenericDao<LinkUserAward> fileSystemDao;

        public static IGenericDao<LinkUserAward> GetInstance(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException();
            }

            switch (key.ToLower())
            {
                case "filesystem":
                    var path = ConfigurationManager.AppSettings.Get("DAL.FileSystem.LinkUserAward.Path");

                    if (fileSystemDao == null)
                    {
                        fileSystemDao = new FileSystem.GenericDao<LinkUserAward>(path, new LinkUserAwardTextConverter());
                    }

                    return fileSystemDao;

                case "memory":
                    if (memoryDao == null)
                    {
                        memoryDao = new Memory.GenericDao<LinkUserAward>();
                    }

                    return memoryDao;

                default:
                    throw new ArgumentException($"Unknown key '{key}'");
            }
        }
    }
}
