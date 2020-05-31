using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseresAndAwards.DAL.Contracts
{
    public interface IEntytyTextConverter<Entity>
    {
        string Delimiter { get; }

        string ToText(Entity entity);

        bool TryParseFromText(string source, out Entity entity);
    }
}
