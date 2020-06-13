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

                var lines = new Dictionary<Point, LineOrientation> 
                {
                    //{ new Point(2, 4), LineOrientation.Horizontal },
                    //{ new Point(2, 5), LineOrientation.Vertical},
                    //{ new Point(2, 3), LineOrientation.Horizontal },
                    //{ new Point(0, 4), LineOrientation.Vertical},
                };

                var bomb = new List<Point>
                {
                    //new Point(0, 4)
                };

                var parentTransform = entity.Get<Transform>();
                foreach (var column in newCells)
                {
                    foreach (var cellComponent in column)
                    {
                        Entity cell = _cellPool.RequestCell(cellComponent, generationInfo.VerticalOffset, parentTransform);
                        entity.SetAsParentOf(cell);
                        if (!generationInfo.IsSecondaryGeneration)
                        {
                            if (lines.TryGetValue(cellComponent.PositionInGrid, out var orientation))
                            {
                                CombinationSystem.ModifyWithLine(cell, orientation);
                            }
                            else if (bomb.Contains(cellComponent.PositionInGrid))
                            {
                                CombinationSystem.ModifyWithBomb(cell);
                            }
                        }
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
            if (!generationInfo.IsSecondaryGeneration)
            {
                DebugGeneration(newCells);
            }
            return newCells;
        }

        private void DebugGeneration(Cell[][] newCells)
        {
            newCells[0][0].Color = CellColor.Brown;
            newCells[0][1].Color = CellColor.Purple;
            newCells[0][2].Color = CellColor.Blue;
            newCells[0][3].Color = CellColor.Brown;
            newCells[0][4].Color = CellColor.Green;
            newCells[0][6].Color = CellColor.Green;

            newCells[1][0].Color = CellColor.Gold;
            newCells[1][1].Color = CellColor.Brown;
            newCells[1][2].Color = CellColor.Green;
            newCells[1][3].Color = CellColor.Green;
            newCells[1][4].Color = CellColor.Blue;
            newCells[1][5].Color = CellColor.Green;

            newCells[2][0].Color = CellColor.Purple;
            newCells[2][1].Color = CellColor.Gold;
            newCells[2][2].Color = CellColor.Blue;
            newCells[2][3].Color = CellColor.Gold;
            newCells[2][4].Color = CellColor.Brown;
            newCells[2][5].Color = CellColor.Gold;
            newCells[2][6].Color = CellColor.Green;
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
