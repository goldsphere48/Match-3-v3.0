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
        private readonly EntitySet _cellSet;
        private readonly World _world;
        private GameState _gameState;

        public CombinationSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _cellSet = _world.GetEntities().With<Cell>().AsSet();
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
                    ProceedCombination(combination, GridUtil.CellsSetToDictionary(_cellSet, width, height));
                }
                entity.Remove<CombinationsArray>();
                _world.Publish(new UnselectMessage());
                _world.Publish(new NewStateMessage { Value = GameState.CellDestroying });
            }
        }

        private static void ModifyWithLine(Entity cellEntity, Direction firstDirection, Direction secondDirection, string textureName)
        {
            cellEntity.Set(new LineBonus(firstDirection, secondDirection, cellEntity));
            SetModificationSprite(cellEntity, textureName);
        }

        private static void SetModificationSprite(Entity cellEntity, string textureName)
        {
            cellEntity.Set(new SpriteRenderer());
            cellEntity.Set(new ManagedResource<string, Texture2D>(textureName));
        }

        private void Destroy(Combination combination, Dictionary<Point, Entity> cellDictionary)
        {
            foreach (var cellPosition in combination)
            {
                if (cellDictionary.TryGetValue(cellPosition, out Entity cellEntity))
                {
                    if (!(cellEntity.Has<DontDestroy>() || cellEntity.Has<Dying>()))
                    {
                        cellEntity.Set<Dying>();
                    }
                    else
                    {
                        cellEntity.Remove<DontDestroy>();
                    }
                }
            }
        }

        private Entity GetModifiable(Combination combination, Dictionary<Point, Entity> cellDictionary)
        {
            Entity? modifiable = null;
            foreach (var cellPosition in combination)
            {
                if (cellDictionary.TryGetValue(cellPosition, out Entity cellEntity))
                {
                    modifiable = modifiable ?? cellEntity;
                    if (cellEntity.Has<Selected>())
                    {
                        return cellEntity;
                    }
                }
            }
            return modifiable.Value;
        }

        [Subscribe]
        private void On(in NewStateMessage newState) => _gameState = newState.Value;

        private void ProceedCombination(Combination combination, Dictionary<Point, Entity> cellDictionary)
        {
            if (combination.Count > 3)
            {
                var modifiable = GetModifiable(combination, cellDictionary);
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
            Destroy(combination, cellDictionary);
        }
    }
}