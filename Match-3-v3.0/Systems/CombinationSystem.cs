using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(CombinationsArray))]
    [With(typeof(CombinationsArray))]
    internal class CombinationSystem : AEntitySystem<float>
    {
        private readonly EntitySet _cells;
        private readonly World _world;
        private GameState _gameState;

        public CombinationSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _cells = _world.GetEntities().With<Cell>().AsSet();
            _gameState = initState;
        }

        public static void ModifyWithBomb(Entity cellEntity)
        {
            SetModificationSprite(cellEntity, "Bomb");
            cellEntity.Set(new BombBonus());
        }

        public static void ModifyWithLine(Entity cellEntity, LineOrientation orientation)
        {
            if (orientation == LineOrientation.Horizontal)
            {
                ModifyWithLine(cellEntity, Direction.Left, Direction.Right, "LineHorizontal");
            }
            else
            {
                ModifyWithLine(cellEntity, Direction.Up, Direction.Down, "LineVertical");
            }
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
                    ProceedCombination(combination, GridUtil.CellsSetToDictionary(_cells, width, height));
                }
                entity.Remove<CombinationsArray>();
                _world.Publish(new UnselectMessage());
                _world.Publish(new NewStateMessage { Value = GameState.CellDestroying });
            }
        }

        private static void ModifyWithLine(Entity cell, Direction firstDirection, Direction secondDirection, string textureName)
        {
            cell.Set(new LineBonus(firstDirection, secondDirection, cell));
            SetModificationSprite(cell, textureName);
        }

        private static void SetModificationSprite(Entity cell, string textureName)
        {
            cell.Set(new SpriteRenderer());
            cell.Set(new ManagedResource<string, Texture2D>(textureName));
        }

        private void Destroy(Combination combination, Dictionary<Point, Entity> cells)
        {
            foreach (var position in combination)
            {
                if (cells.TryGetValue(position, out Entity entity))
                {
                    if (!(entity.Has<DontDestroy>() || entity.Has<Dying>()))
                    {
                        entity.Set<Dying>();
                    }
                    else
                    {
                        entity.Remove<DontDestroy>();
                    }
                }
            }
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

        [Subscribe]
        private void On(in NewStateMessage newState) => _gameState = newState.Value;

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
    }
}