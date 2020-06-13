using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Match_3_v3._0.Systems
{
    [With(typeof(Grid))]
    internal class FallSystem : AEntitySystem<float>
    {
        private readonly EntitySet _cellSet;
        private readonly World _world;
        private GameState _gameState;

        public FallSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _cellSet = _world.GetEntities().With<Cell>().AsSet();
            _gameState = initState;
        }

        protected override void Update(float state, in Entity gridEntity)
        {
            if (_gameState == GameState.Falling)
            {
                var maxColumnHeight = 0;
                var grid = gridEntity.Get<Grid>();
                var cellDictionary = GridUtil.CellsSetToDictionary(_cellSet, grid.Width, grid.Height);
                Point[][] newCells = new Point[grid.Width][];
                for (int i = 0; i < grid.Width; ++i)
                {
                    var emptyCellsCount = CalculateEmptyCellsCount(grid, cellDictionary, i);
                    for (int j = grid.Height - 1; j >= 0; --j)
                    {
                        var currentPosition = new Point(i, j);
                        ComeUpEmptyCell(currentPosition, cellDictionary, grid);
                    }
                    maxColumnHeight = Math.Max(maxColumnHeight, emptyCellsCount);
                    newCells[i] = GenerateColumnNewCellPositions(emptyCellsCount, i);
                }
                GridUtil.Generate(_world, newCells, -maxColumnHeight * PlayerPrefs.Get<int>("CellSize"));
            }
        }

        private int CalculateEmptyCellsCount(Grid grid, Dictionary<Point, Entity> cellDictionary, int column)
        {
            var emptyCellsCount = 0;
            for (int j = grid.Height - 1; j >= 0; --j)
            {
                var currentPosition = new Point(column, j);
                if (!cellDictionary.ContainsKey(currentPosition))
                {
                    emptyCellsCount++;
                }
            }
            return emptyCellsCount;
        }

        private void ComeUpEmptyCell(Point currentPosition, Dictionary<Point, Entity> cellDictionary, Grid grid)
        {
            if (!cellDictionary.ContainsKey(currentPosition))
            {
                var notEmptyPosition = GetNextNotEmptyPosition(currentPosition, cellDictionary);
                if (cellDictionary.TryGetValue(notEmptyPosition, out var fallingCellEntity))
                {
                    MoveDown(grid, fallingCellEntity, currentPosition);
                    cellDictionary.Remove(notEmptyPosition);
                }
            }
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

        private Point GetNextNotEmptyPosition(Point position, Dictionary<Point, Entity> cellDictionary)
        {
            do
            {
                position.Y--;
            }
            while (position.Y > 0 && !cellDictionary.TryGetValue(position, out var _));
            return position;
        }

        private void MoveDown(Grid grid, Entity cellEntity, Point newPositioninGrid)
        {
            var cell = cellEntity.Get<Cell>();
            Swap(grid, cell.PositionInGrid, newPositioninGrid);
            cell.PositionInGrid = newPositioninGrid;
            cellEntity.Set(cell);
            var newPosition = newPositioninGrid.ToVector2() * PlayerPrefs.Get<int>("CellSize");
            cellEntity.Set(new TargetPosition { Position = newPosition, UseLocalPosition = true });
        }

        [Subscribe]
        private void On(in NewStateMessage newState) => _gameState = newState.Value;

        private void Swap(Grid grid, Point first, Point second)
        {
            var tmp = grid.Cells[first.X, first.Y].Color;
            grid.Cells[first.X, first.Y].Color = grid.Cells[second.X, second.Y].Color;
            grid.Cells[second.X, second.Y].Color = tmp;
        }
    }
}