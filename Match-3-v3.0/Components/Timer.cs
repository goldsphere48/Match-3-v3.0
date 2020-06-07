using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    struct Timer
    {
        public Action TimerTick;
        public float Interval { get; set; }
        public float CurrentTime { get; set; }
    }
}
