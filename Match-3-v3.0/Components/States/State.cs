using Match_3_v3._0.Data;
using Match_3_v3._0.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components.States
{
    abstract class State
    {
        public abstract void Handle(GameStateContext system, GameState newState);
    }
}
