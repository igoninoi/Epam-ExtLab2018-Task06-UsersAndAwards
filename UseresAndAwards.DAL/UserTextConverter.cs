using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.DAL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.DAL
{
    public class UserTextConverter : IEntytyTextConverter<User>
    {
        public UserTextConverter()
        {
        }

        public UserTextConverter(string delimiter)
        {
            this.Delimiter = delimiter;
        }

        public string Delimiter { get; private set; } = System.IO.StreamWriter.Null.NewLine;

        public string ToText(User entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine(entity.Name);
            sb.AppendLine(entity.DateOfBirth.ToShortDateString());
            return sb.ToString();
        }

        public bool TryParseFromText(string source, out User entity)
        {
            entity = new User();
            var sr = new StringReader(source);

            entity.Name = sr.ReadLine();

            DateTime date;
            bool isOk = DateTime.TryParse(sr.ReadLine(), out date);
            if (isOk)
            {
                entity.DateOfBirth = date;
            }

            return isOk;
        }
    }
}
