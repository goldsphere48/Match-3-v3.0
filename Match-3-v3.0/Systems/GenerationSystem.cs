using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Utils;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(GenerationZone))]
    [WhenChanged(typeof(GenerationZone))]
    [With(typeof(GenerationZone))]
    [With(typeof(Transform))]
    class GenerationSystem : AEntitySystem<float>
    {
        private CellPool _cellPool;

        public GenerationSystem(World world)
            : base(world)
        {
            _cellPool = new CellPool(new CellFactory(world, PlayerPrefs.Get<int>("CellSize")));
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
            while (IsMatchExist(grid) == false);

            var parentTransform = entity.Get<Transform>();
            foreach (var column in newCells)
            {
                foreach (var cellComponent in column)
                {
                    Entity cell = _cellPool.RequestCell(cellComponent, generationInfo.VerticalOffset, parentTransform);
                    entity.SetAsParentOf(cell);
                }
            }
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
            return newCells;
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

        private int attempts = 0;

        private bool IsMatchExist(Grid grid)
        {
            attempts++;
            if (attempts == 4)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
