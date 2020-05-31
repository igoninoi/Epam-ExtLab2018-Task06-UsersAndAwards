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
    public class LinkUserAwardTextConverter : IEntytyTextConverter<LinkUserAward>
    {
        public LinkUserAwardTextConverter()
        {
        }

        public LinkUserAwardTextConverter(string delimiter)
        {
            this.Delimiter = delimiter;
        }

        public string Delimiter { get; private set; } = System.IO.StreamWriter.Null.NewLine;

        public string ToText(LinkUserAward entity)
        {
            var sb = new StringBuilder();
            sb.AppendLine(entity.UserId.ToString());
            sb.AppendLine(entity.AwardId.ToString());
            return sb.ToString();
        }

        public bool TryParseFromText(string source, out LinkUserAward entity)
        {
            entity = new LinkUserAward();
            var sr = new StringReader(source);

            Guid id;
            if (!Guid.TryParse(sr.ReadLine(), out id))
            {
                return false;
            }

            entity.UserId = id;

            if (!Guid.TryParse(sr.ReadLine(), out id))
            {
                return false;
            }

            entity.AwardId = id;
            return true;
        }
    }
}
