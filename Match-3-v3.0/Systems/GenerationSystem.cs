using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(GenerationZone))]
    [With(typeof(GenerationZone))]
    [With(typeof(Grid))]
    [With(typeof(Transform))]
    internal class GenerationSystem : AEntitySystem<float>
    {
        private readonly CellPool _cellPool;
        private readonly World _world;
        private GameState _gameState;

        public GenerationSystem(World world, CellPool cellPool, GameState initState)
            : base(world)
        {
            _gameState = initState;
            _cellPool = cellPool;
            _world = world;
            _world.Subscribe(this);
        }

        protected override void Update(float state, in Entity gridEntity)
        {
            if (_gameState == GameState.Generating)
            {
                var generationZone = gridEntity.Get<GenerationZone>();
                var grid = gridEntity.Get<Grid>();
                Cell[][] newCells = Generate(ref grid, generationZone);
                while (!FindMatchesSystem.IsGridValid(grid))
                {
                    newCells = Generate(ref grid, generationZone);
                }
                InstatiateNewCells(newCells, gridEntity, generationZone.VerticalOffset);
                gridEntity.Set(grid);
                gridEntity.Remove<GenerationZone>();
                _world.Publish(new NewStateMessage { Value = GameState.WaitForFalling });
            }
        }

        private void ApplyNewCells(Grid grid, Cell[][] newCells)
        {
            foreach (var column in newCells)
            {
                foreach (var cell in column)
                {
                    grid.Cells[cell.PositionInGrid.X, cell.PositionInGrid.Y] = cell;
                }
            }
        }

        private Cell[][] Generate(ref Grid grid, GenerationZone generationZone)
        {
            var newCells = GenerateNewCells(generationZone);
            ApplyNewCells(grid, newCells);
            return newCells;
        }

        private Cell[][] GenerateNewCells(GenerationZone generationInfo)
        {
            var newCells = new Cell[generationInfo.NewCellPositionsInGrid.Length][];
            for (int i = 0; i < newCells.Length; ++i)
            {
                newCells[i] = new Cell[generationInfo.NewCellPositionsInGrid[i].Length];
                for (int j = 0; j < newCells[i].Length; ++j)
                {
                    newCells[i][j] = new Cell
                    {
                        PositionInGrid = generationInfo.NewCellPositionsInGrid[i][j],
                        Color = CellColorGenerator.Get()
                    };
                }
            }
            return newCells;
        }

        private void InstatiateNewCells(Cell[][] newCells, Entity gridEntity, float verticalOffset)
        {
            var parentTransform = gridEntity.Get<Transform>();
            foreach (var column in newCells)
            {
                foreach (var cell in column)
                {
                    Entity cellEntity = _cellPool.RequestCell(cell, verticalOffset, parentTransform);
                    gridEntity.SetAsParentOf(cellEntity);
                }
            }
        }

        [Subscribe]
        private void On(in NewStateMessage newStateMessage) => _gameState = newStateMessage.Value;
    }
}