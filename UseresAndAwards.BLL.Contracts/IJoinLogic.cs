using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseresAndAwards.BLL.Contracts
{
    public interface IJoinLogic<LeftEntity, RightEntity> 
    {
        bool GetLeftJoin(out IEnumerable<KeyValuePair<LeftEntity, RightEntity>> collection, out string errorMessage);
    }
}
