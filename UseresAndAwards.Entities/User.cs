using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseresAndAwards.Entities
{
    public class User
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var now = DateTime.Now;
                return (now.Year - this.DateOfBirth.Year) -
                    ((this.DateOfBirth.Month > now.Month) ||
                    (this.DateOfBirth.Month == now.Month &&
                    this.DateOfBirth.Day > now.Day) ? 1 : 0);
            }
        }
    }
}
