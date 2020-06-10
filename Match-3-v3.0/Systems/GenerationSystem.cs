using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
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

        public GenerationSystem(World world, CellPool cellPool)
            : base(world)
        {
            _cellPool = cellPool;
        }

        protected override void Update(float state, in Entity entity)
        {
            ref var generationInfo = ref entity.Get<GenerationZone>();
            ref var grid = ref entity.Get<Grid>();
            Cell[][] newCells;
            do
            {
                newCells = GenerateNewCells(generationInfo);
                ApplyNewCells(grid, newCells);
            } 
            while (MatchesExist(grid) || PossibleMatchesDoesntExist(grid));

            var parentTransform = entity.Get<Transform>();
            foreach (var column in newCells)
            {
                foreach (var cellComponent in column)
                {
                    Entity cell = _cellPool.RequestCell(cellComponent, generationInfo.VerticalOffset, parentTransform);
                    entity.SetAsParentOf(cell);
                }
            }
            entity.Remove<GenerationZone>();
        }

        private Cell[][] GenerateNewCells(GenerationZone generationInfo)
        {
            var newCells = new Cell[generationInfo.NewCellPositionsInGrid.Length][];
            for (int i = 0; i < newCells.Length; ++i)
            {
                newCells[i] = new Cell[generationInfo.NewCellPositionsInGrid[i].Length];
                for(int j = 0; j <  newCells[i].Length; ++j)
                {
                    newCells[i][j] = new Cell
                    {
                        PositionInGrid = generationInfo.NewCellPositionsInGrid[i][j],
                        Color = CellColorGenerator.Get()
                    };
                }
            }
            DebugSetup(newCells);
            return newCells;
        }

        private void DebugSetup(Cell[][] cells)
        {
            cells[0][0].Color = CellColor.Blue;
            cells[0][1].Color = CellColor.Blue;
            cells[0][2].Color = CellColor.Green;
            cells[0][3].Color = CellColor.Blue;
        }

        private void ApplyNewCells(Grid grid, Cell[][] newCells)
        {
            foreach (var column in newCells)
            {
                foreach (var cell in column)
                {
                    grid.Cells[(int)cell.PositionInGrid.X, (int)cell.PositionInGrid.Y] = cell; 
                }
            }
        }

        private bool MatchesExist(Grid grid)
        {
            return FindMatchesSystem.FindMatches(grid).Count() > 0;
        }

        private bool PossibleMatchesDoesntExist(Grid grid)
        {
            return false;
        }
    }
}
