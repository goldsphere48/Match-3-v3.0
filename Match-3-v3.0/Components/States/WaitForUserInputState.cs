using Match_3_v3._0.Data;
using Match_3_v3._0.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components.States
{
    class WaitForUserInputState : State
    {
        public override void Handle(GameStateContext context, GameState newState)
        {
            switch (newState)
            {
                case GameState.Swaping:
                    context.SetState(new Swaping());
                    break;
            }
        }
    }
}
