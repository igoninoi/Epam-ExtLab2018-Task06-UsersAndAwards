using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.BLL.Contracts;
using UseresAndAwards.DAL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.BLL.Core
{
    public class UserLogic : IEntityLogic<User>
    {
        private static UserLogic self;

        private static IGenericDao<User> dao;

        static UserLogic()
        {
            self = new UserLogic();

            var key = System.Configuration.ConfigurationManager.AppSettings.Get("DAL");
            dao = DAL.UserDaos.GetInstance(key);
        }

        private UserLogic()
        {
        }

        public static IEntityLogic<User> GetInstance => self;

        public bool Add(User entity, out string errorMessage)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            string name = entity.Name;
            if (string.IsNullOrEmpty(name) || !char.IsLetterOrDigit(name[0]))
            {
                errorMessage = "User name is empty or do not starts with letter or digit.";
                return false;
            }

            if (name.Length > 100)
            {
                errorMessage = "Lenth of user name is more than 100.";
                return false;
            }

            if (entity.DateOfBirth > DateTime.Now || entity.DateOfBirth.Date < DateTime.Now.AddYears(-100))
            {
                errorMessage = "User's date of birth out of range (age had to be from 0 to 100).";
                return false;
            }

            var id = Guid.NewGuid();

            try
            {
                return dao.Add(id, entity, out errorMessage);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }
        }

        public bool Delete(Guid id, out string errorMessage)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (id == Guid.Empty)
            {
                errorMessage = "GUID has empty value.";
                return false;
            }

            try
            {
                return dao.Delete(id, out errorMessage);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Data access error. Сontact the developer.";
                return false;
            }
        }

        public bool GetAll(out IEnumerable<KeyValuePair<Guid, User>> collection, out string errorMessage)
        {
            bool result;
            try
            {
                result = dao.GetAll(out collection, out errorMessage);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Data access error. Сontact the developer.";
                collection = new List<KeyValuePair<Guid, User>>(0);
                return false;
            }

            collection = collection.ToList();
            return result;
        }

        private static void WriteToLog(Exception exception)
        {
            //// TODO
        }
    }
}
