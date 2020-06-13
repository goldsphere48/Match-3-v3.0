using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private List<Type> _bonusList;

        public CombinationSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _cells = _world.GetEntities().With<Cell>().AsSet();
            _gameState = initState;
            _bonusList = new List<Type>() { typeof(LineBonus), typeof(BombBonus) };
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
                _world.Publish(new UnselectMessage());
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
            if (combination.Count > 3)
            {
                var modifiable = GetModifiable(combination, cells);
                if (!CellUtil.IsBonus(modifiable))
                {
                    modifiable.Set<DontDestroy>();
                    if (combination.Count == 4)
                    {
                        ModifyWithLine(modifiable, combination.Orientation);
                    }
                    else
                    {
                        ModifyWithBomb(modifiable);
                    }
                }
            }
            Destroy(combination, cells);
        }

        private void ModifyWithBomb(Entity cellEntity)
        {
            SetModificationSprite(cellEntity, "Bomb");
            cellEntity.Set(new BombBonus());
        }

        public void ModifyWithLine(Entity cellEntity, LineOrientation orientation)
        {
            var grid = _world.First(e => e.Has<Grid>());
            var textureName = "";
            var cell = cellEntity.Get<Cell>();
            Direction firstDirection;
            Direction secondDirection;
            if (orientation == LineOrientation.Horizontal)
            {
                firstDirection = Direction.Left;
                secondDirection = Direction.Right;
                textureName = "LineHorizontal";
            } else
            {
                firstDirection = Direction.Up;
                secondDirection = Direction.Down;
                textureName = "LineVertical";
            }
            SetModificationSprite(cellEntity, textureName);
            cellEntity.Set(new LineBonus(firstDirection, secondDirection, cellEntity));
        }

        private void SetModificationSprite(Entity cell, string textureName)
        {
            cell.Set(new SpriteRenderer());
            cell.Set(new ManagedResource<string, Texture2D>(textureName));
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
                        return entity;
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
                    } else
                    {
                        entity.Remove<DontDestroy>();
                    }
                }
            }
        }
    }
}
