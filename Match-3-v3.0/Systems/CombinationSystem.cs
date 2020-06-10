using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(CombinationsArray))]
    [With(typeof(CombinationsArray))]
    class CombinationSystem : AEntitySystem<float>
    {
        private EntitySet _cells;
        private World _world;
        private GameState _gameState;

        public CombinationSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _cells = _world.GetEntities().With<Cell>().AsSet();
            _gameState = initState;
        }

        [Subscribe]
        private void On(in NewStateMessage newState)
        {
            _gameState = newState.Value;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_gameState == GameState.CombinationChecking)
            {
                var combinations = entity.Get<CombinationsArray>();
                var width = PlayerPrefs.Get<int>("Width");
                var height = PlayerPrefs.Get<int>("Height");
                foreach (var combination in combinations.Value)
                {
                    ProceedCombination(combination, GetCells(width, height));
                }
                entity.Remove<CombinationsArray>();
                _world.Publish(new NewStateMessage { Value = GameState.CellDestroying });
            }
        }

        private Dictionary<Point, Entity> GetCells(int width, int height)
        {
            var result = new Dictionary<Point, Entity>(width * height);
            foreach (var cell in _cells.GetEntities())
            {
                result.Add(cell.Get<Cell>().PositionInGrid, cell);
            }
            return result;
        }

        private void ProceedCombination(Combination combination, Dictionary<Point, Entity> cells)
        {
            ///if (combination.Count == 3)
            //{
            ProceedSimple(combination, cells);
            /*} else
            {
                var modifiable = GetModifiable(combination, cells);
                modifiable.Set<DontDestroy>();
                Destroy(combination, cells);
                modifiable.Remove<DontDestroy>();
                if (combination.Count == 4)
                {
                    ProceedLine(modifiable);
                } else
                {
                    ProceedBomb(modifiable);
                }
            }*/
        }

        private void ProceedBomb(Entity cell)
        {

        }

        private void ProceedLine(Entity cell)
        {

        }

        private Entity GetModifiable(Combination combination, Dictionary<Point, Entity> cells)
        {
            Entity? modifiable = null;
            foreach (var position in combination)
            {
                if (cells.TryGetValue(position, out Entity entity))
                {
                    modifiable = modifiable ?? entity;
                    if (entity.Has<Selected>())
                    {
                        modifiable = entity;
                    }
                }
            }
            return modifiable.Value;
        }

        private void Destroy(Combination combination, Dictionary<Point, Entity> cells)
        {
            foreach (var position in combination)
            {
                if (cells.TryGetValue(position, out Entity entity))
                {
                    if (!entity.Has<DontDestroy>())
                    {
                        entity.Set<Dying>();
                    }
                }
            }
        }

        private void ProceedSimple(Combination combination, Dictionary<Point, Entity> cells)
        {
            Destroy(combination, cells);
        }
    }
}
