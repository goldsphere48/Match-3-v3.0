using System;

namespace Match_3_v3._0.Components
{
    internal struct Timer
    {
        public Action TimerTick;
        public float CurrentTime { get; set; }
        public float Interval { get; set; }
    }
}