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
        private readonly EntitySet _cells;
        private readonly World _world;
        private GameState _gameState;

        public FallSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _cells = _world.GetEntities().With<Cell>().AsSet();
            _gameState = initState;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_gameState == GameState.Falling)
            {
                var maxColumnHeight = 0;
                var grid = entity.Get<Grid>();
                var cells = GridUtil.CellsSetToDictionary(_cells, grid.Width, grid.Height);
                Point[][] newCells = new Point[grid.Width][];
                for (int i = 0; i < grid.Width; ++i)
                {
                    var emptyCellsCount = CalculateEmptyCellsCount(grid, cells, i);
                    for (int j = grid.Height - 1; j >= 0; --j)
                    {
                        var currentPosition = new Point(i, j);
                        ComeUpEmptyCell(currentPosition, cells, grid);
                    }
                    maxColumnHeight = Math.Max(maxColumnHeight, emptyCellsCount);
                    newCells[i] = GenerateColumnNewCellPositions(emptyCellsCount, i);
                }
                GridUtil.Generate(_world, newCells, -maxColumnHeight * PlayerPrefs.Get<int>("CellSize"));
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

        private void ComeUpEmptyCell(Point currentPosition, Dictionary<Point, Entity> cells, Grid grid)
        {
            if (!cells.ContainsKey(currentPosition))
            {
                var notEmptyPosition = GetNextNotEmptyPosition(currentPosition, cells);
                if (cells.TryGetValue(notEmptyPosition, out var fallingCellEntity))
                {
                    MoveDown(grid, fallingCellEntity, currentPosition);
                    cells.Remove(notEmptyPosition);
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

        private Point GetNextNotEmptyPosition(Point position, Dictionary<Point, Entity> cells)
        {
            do
            {
                position.Y--;
            }
            while (position.Y > 0 && !cells.TryGetValue(position, out var _));
            return position;
        }

        private void MoveDown(Grid grid, Entity cellEntity, Point newPosition)
        {
            var cell = cellEntity.Get<Cell>();
            Swap(grid, cell.PositionInGrid, newPosition);
            cell.PositionInGrid = newPosition;
            cellEntity.Set(cell);
            var position = newPosition.ToVector2() * PlayerPrefs.Get<int>("CellSize");
            cellEntity.Set(new TargetPosition { Position = position, UseLocalPosition = true });
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