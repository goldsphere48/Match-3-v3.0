using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Messages
{
    struct NewStateMessage
    {
        public GameState Value { get; set; }
    }
}
