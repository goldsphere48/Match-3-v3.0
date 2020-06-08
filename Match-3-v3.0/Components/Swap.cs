using DefaultEcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    struct Swap
    {
        public Entity First;
        public Entity Second;
        public Swap(Entity first, Entity second)
        {
            First = first;
            Second = second;
        }
    }
}
