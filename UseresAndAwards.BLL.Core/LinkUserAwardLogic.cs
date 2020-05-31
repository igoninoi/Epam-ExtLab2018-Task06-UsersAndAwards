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
    public class LinkUserAwardLogic : IEntityLogic<LinkUserAward>, IJoinLogic<User, Award>
    {
        private static LinkUserAwardLogic self;

        private static IGenericDao<LinkUserAward> linkDao;

        private static IGenericDao<User> leftDao;

        private static IGenericDao<Award> rightDao;

        static LinkUserAwardLogic()
        {
            self = new LinkUserAwardLogic();

            var key = System.Configuration.ConfigurationManager.AppSettings.Get("DAL");
            linkDao = DAL.LinkUserAwardDaos.GetInstance(key);
            leftDao = DAL.UserDaos.GetInstance(key);
            rightDao = DAL.AwardDaos.GetInstance(key);
        }

        private LinkUserAwardLogic()
        {
        }

        public static IEntityLogic<LinkUserAward> GetInstance => self;

        public static IJoinLogic<User, Award> GetJounInstance => self;

        public bool Add(LinkUserAward entity, out string errorMessage)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            Guid id = entity.UserId;
            if (id == null || id == Guid.Empty)
            {
                errorMessage = "User ID is null null empty.";
                return false;
            }

            IEnumerable<KeyValuePair<Guid, User>> leftResult;
            try
            {
                if (!leftDao.GetById(id, out leftResult, out errorMessage))
                {
                    errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }

            if (leftResult.Count() == 0)
            {
                errorMessage = "Тhere is no User with this ID.";
                return false;
            }

            id = entity.AwardId;
            if (id == null || id == Guid.Empty)
            {
                errorMessage = "Award ID is null null empty.";
                return false;
            }

            IEnumerable<KeyValuePair<Guid, Award>> rightResult;
            try
            {
                if (!rightDao.GetById(id, out rightResult, out errorMessage))
                {
                    errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                return false;
            }

            if (rightResult.Count() == 0)
            {
                errorMessage = "Тhere is no Award with this ID.";
                return false;
            }

            id = Guid.NewGuid();

            try
            {
                return linkDao.Add(id, entity, out errorMessage);
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
                return linkDao.Delete(id, out errorMessage);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Data access error. Сontact the developer.";
                return false;
            }
        }

        public bool GetAll(out IEnumerable<KeyValuePair<Guid, LinkUserAward>> collection, out string errorMessage)
        {
            bool result;
            try
            {
                result = linkDao.GetAll(out collection, out errorMessage);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Data access error. Сontact the developer.";
                collection = new List<KeyValuePair<Guid, LinkUserAward>>(0);
                return false;
            }

            collection = collection.ToList();
            return result;
        }

        public bool GetLeftJoin(out IEnumerable<KeyValuePair<User, Award>> collection, out string errorMessage)
        {
            var result = new List<KeyValuePair<User, Award>>(0);

            IEnumerable<KeyValuePair<Guid, User>> leftSet;
            IEnumerable<KeyValuePair<Guid, LinkUserAward>> linkSet;
            IEnumerable<KeyValuePair<Guid, Award>> rightSet;

            if (!leftDao.GetAll(out leftSet, out errorMessage) ||
                !linkDao.GetAll(out linkSet, out errorMessage) ||
                !rightDao.GetAll(out rightSet, out errorMessage))
            {
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                collection = result;
                return false;
            }

            try
            {
                result = leftSet
                    .Join(
                        linkSet,
                        x => x.Key,
                        y => y.Value.UserId,
                        (x, y) => new KeyValuePair<User, Guid>(x.Value, y.Value.AwardId))
                            .Join(
                                rightSet,
                                a => a.Value,
                                b => b.Key,
                                (x, y) => new KeyValuePair<User, Award>(x.Key, y.Value))
                                    .OrderBy(x => x.Key.Name).ToList();
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                errorMessage = "Sorry. Error in data access layer. Сontact the developer.";
                collection = new List<KeyValuePair<User, Award>>(0);
                return false;
            }

            errorMessage = string.Empty;
            collection = result;
            return true;
        }

        private static void WriteToLog(Exception exception)
        {
            //// TODO
        }
    }
}
