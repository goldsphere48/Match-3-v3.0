using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class DebugSystem : InputSystem
    {
        private GameState _gameState;
        private World _world;
        private EntitySet _cells;

        public DebugSystem(World world, GameWindow window, GameState initState)
            : base(world.GetEntities().With<Grid>().AsSet(), window)
        {
            _gameState = initState;
            _world = world;
            _world.Subscribe(this);
            _cells = _world.GetEntities().With<Cell>().AsSet();
            new Thread(PrintDestroyersCount).Start();
        }

        [Subscribe]
        private void On(in NewStateMessage newState)
        {
            _gameState = newState.Value;
        }

        private void PrintDestroyersCount()
        {
            while (true)
            {
                Console.Clear();
                var lineBonuses = _world.Get<LineBonus>();
                Console.WriteLine("Line bonuses: " + lineBonuses.Length);
                var bombBonuses = _world.Get<BombBonus>();
                Console.WriteLine("Bomb bonuses: " + bombBonuses.Length);
                var dies = _world.GetEntities().With<Dying>().AsSet();
                Console.WriteLine("Dyings: " + dies.Count);
                foreach (var death in dies.GetEntities())
                {
                    if (death.Has<Cell>())
                    {
                        Console.WriteLine(death.Get<Cell>().PositionInGrid);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_gameState == GameState.WaitForUserInput)
            {
                var grid = entity.Get<Grid>();
                if (_state.RightButton == ButtonState.Released && _oldState.RightButton == ButtonState.Pressed)
                {
                    foreach (var cell in _cells.GetEntities())
                    {
                        cell.Set<Dying>();
                    }
                    _world.Publish(new NewStateMessage { Value = GameState.CellDestroying });
                }
            }
        }
    }
}
