﻿using DefaultEcs;
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
    [With(typeof(Grid))]
    class FallSystem : AEntitySystem<float>
    {
        private World _world;
        private EntitySet _cells;
        private GameState _gameState;

        public FallSystem(World world, GameState initState)
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
            if (_gameState == GameState.Falling)
            {
                var maxColumnHeight = 0;
                var grid = entity.Get<Grid>();
                var cells = GetCells(grid.Width, grid.Height);
                Point[][] newCells = new Point[grid.Width][];
                for (int i = 0; i < grid.Width; ++i)
                {
                    var emptyCellsCount = CalculateEmptyCellsCount(grid, cells, i);
                    for (int j = grid.Height - 1 ; j >= 0; --j)
                    {
                        var currentPosition = new Point(i, j);
                        if (!cells.ContainsKey(currentPosition))
                        {
                            var notEmptyPosition = GetNextNotEmptyPosition(currentPosition, cells);
                            if (cells.TryGetValue(notEmptyPosition, out var fallingCellEntity))
                            {
                                MoveDown(fallingCellEntity, currentPosition);
                                cells.Remove(notEmptyPosition);
                            }
                        }
                    }                    
                    maxColumnHeight = Math.Max(maxColumnHeight, emptyCellsCount);
                    newCells[i] = GenerateColumnNewCellPositions(emptyCellsCount, i);
                }
                CreateNewGenerationZone(newCells, maxColumnHeight);
            }
        }

        private int CalculateEmptyCellsCount(Grid grid, Dictionary<Point, Entity> cells, int column)
        {
            var emptyCellsCount = 0;
            for (int j = grid.Height - 1; j >= 0; --j)
            {
                var currentPosition = new Point(column, j);
                if (!cells.ContainsKey(currentPosition))
                {
                    emptyCellsCount++;
                }
            }
            return emptyCellsCount;
        }

        private void CreateNewGenerationZone(Point[][] positions, int maxColumnHeight)
        {
            _world.First(e => e.Has<Grid>()).Set(
                new GenerationZone 
                {
                    NewCellPositionsInGrid = positions, 
                    VerticalOffset = -maxColumnHeight * PlayerPrefs.Get<int>("CellSize"),
                    IsSecondaryGeneration = true
                }
            );
            _world.Publish(new NewStateMessage { Value = GameState.Generating });
        }

        private Point[] GenerateColumnNewCellPositions(int count, int column)
        {
            Point[] newCells = new Point[count];
            for (int k = 0; k < newCells.Length; ++k)
            {
                newCells[k] = new Point(column, k);
            }
            return newCells;
        }

        private void MoveDown(Entity cellEntity, Point newPosition)
        {
            var transform = cellEntity.Get<Transform>();
            var cell = cellEntity.Get<Cell>();
            cell.PositionInGrid = newPosition;
            cellEntity.Set(cell);
            var position = newPosition.ToVector2() * PlayerPrefs.Get<int>("CellSize");
            cellEntity.Set(new TargetPosition { Position = position, UseLocalPosition = true });
        }

        private Point GetNextNotEmptyPosition(Point position, Dictionary<Point, Entity> cells)
        {
            do
            {
                position.Y--;
            }
            while (position.Y > 0 && !cells.TryGetValue(position, out var cell));
            return position;
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
    }
}