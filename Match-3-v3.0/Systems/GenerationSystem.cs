using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using System.Linq;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(GenerationZone))]
    [With(typeof(GenerationZone))]
    [With(typeof(Grid))]
    [With(typeof(Transform))]
    class GenerationSystem : AEntitySystem<float>
    {
        private CellPool _cellPool;
        private GameState _gameState;
        private World _world;

        public GenerationSystem(World world, CellPool cellPool, GameState initState)
            : base(world)
        {
            _gameState = initState;
            _cellPool = cellPool;
            _world = world;
            _world.Subscribe(this);
        }

        [Subscribe]
        private void On(in NewStateMessage newStateMessage)
        {
            _gameState = newStateMessage.Value;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_gameState == GameState.Generating)
            {
                var generationInfo = entity.Get<GenerationZone>();
                var grid = entity.Get<Grid>();
                Cell[][] newCells = Generate(ref grid, generationInfo);

                while (!FindMatchesSystem.IsGridValid(grid))
                {
                    newCells = Generate(ref grid, generationInfo);
                }

                var parentTransform = entity.Get<Transform>();
                foreach (var column in newCells)
                {
                    foreach (var cellComponent in column)
                    {
                        Entity cell = _cellPool.RequestCell(cellComponent, generationInfo.VerticalOffset, parentTransform);
                        entity.SetAsParentOf(cell);
                    }
                }
                entity.Set(grid);
                entity.Remove<GenerationZone>();
                _world.Publish(new NewStateMessage { Value = GameState.WaitForFalling });
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
            var height = 0;
            for (int i = 0; i < newCells.Length; ++i)
            {
                newCells[i] = new Cell[generationInfo.NewCellPositionsInGrid[i].Length];
                for(int j = 0; j <  newCells[i].Length; ++j)
                {
                    height = newCells[i].Length;
                    newCells[i][j] = new Cell
                    {
                        PositionInGrid = generationInfo.NewCellPositionsInGrid[i][j],
                        Color = CellColorGenerator.Get()
                    };
                }
            }
            if (!generationInfo.IsSecondaryGeneration)
            {
                //DebugSetup(newCells);
            }
            return newCells;
        }

        private void DebugSetup(Cell[][] cells)
        {
            cells[0][0].Color = CellColor.Green;
            cells[0][1].Color = CellColor.Purple;
            cells[0][2].Color = CellColor.Green;

            cells[1][0].Color = CellColor.Purple;
            cells[1][1].Color = CellColor.Brown;
            cells[1][2].Color = CellColor.Green;

            cells[2][0].Color = CellColor.Blue;
            cells[2][1].Color = CellColor.Gold;
            cells[2][2].Color = CellColor.Green;
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
    }
}
