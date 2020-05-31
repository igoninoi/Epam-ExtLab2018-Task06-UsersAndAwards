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
    public class AwardLogic : IEntityLogic<Award>
    {
        private static AwardLogic self;

        private static IGenericDao<Award> dao;

        static AwardLogic()
        {
            self = new AwardLogic();

            var key = System.Configuration.ConfigurationManager.AppSettings.Get("DAL");
            dao = DAL.AwardDaos.GetInstance(key);
        }

        private AwardLogic()
        {
        }

        public static IEntityLogic<Award> GetInstance => self;

        public bool Add(Award entity, out string errorMessage)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            string title = entity.Title;
            if (string.IsNullOrEmpty(title) || !char.IsLetterOrDigit(title[0]))
            {
                errorMessage = "Award title is empty or do not starts with letter or digit.";
                return false;
            }

            if (title.Length > 200)
            {
                errorMessage = "Lenth of award title is more than 200.";
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

        public bool GetAll(out IEnumerable<KeyValuePair<Guid, Award>> collection, out string errorMessage)
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
                collection = new List<KeyValuePair<Guid, Award>>(0);
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
