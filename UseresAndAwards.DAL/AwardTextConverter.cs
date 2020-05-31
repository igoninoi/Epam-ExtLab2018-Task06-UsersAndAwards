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
    public class AwardTextConverter : IEntytyTextConverter<Award>
    {
        public AwardTextConverter()
        {
        }

        public AwardTextConverter(string delimiter)
        {
            this.Delimiter = delimiter;
        }

        public string Delimiter { get; private set; } = System.IO.StreamWriter.Null.NewLine;

        public string ToText(Award entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine(entity.Title);
            return sb.ToString();
        }

        public bool TryParseFromText(string source, out Award entity)
        {
            entity = new Award();
            var sr = new StringReader(source);
            entity.Title = sr.ReadLine();
            return true;
        }
    }
}
