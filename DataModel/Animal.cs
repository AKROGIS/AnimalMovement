using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public partial class Animal
    {
        public override string ToString()
        {
            return _ProjectId + "/" + _AnimalId;
        }
    }
}
