using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Swap))]
    [With(typeof(Swap))]
    [With(typeof(Grid))]
    class SwapSystem : AEntitySystem<float>
    {
        public SwapSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, in Entity entity)
        {
            var swap = entity.Get<Swap>();
            CheckMatch(entity, swap);
            entity.Remove<Swap>();
        }

        private void CheckMatch(Entity entity, Swap swap)
        {
            var grid = entity.Get<Grid>();
            swap.Deconstruct(out var first, out var second);
            var firstCell = first.Get<Cell>();
            var secondCell = second.Get<Cell>();
            grid = ApplySwapToGridAndGet(grid, firstCell, secondCell);
            SwapCells(first, second);
            var matches = FindMatchesSystem.FindMatches(grid).Count();
            if (matches == 0)
            {
                first.Set(new SwapSuccess { Value = SwapResult.Fail });
                SwapBack(first, second);
            } else
            {
                first.Set(new SwapSuccess { Value = SwapResult.Success});
                entity.Set(grid);
                entity.Remove<Swap>();
            }
        }

        private Grid ApplySwapToGridAndGet(Grid grid, Cell first, Cell second)
        {
            first.PositionInGrid.Deconstruct(out var x1, out var y1);
            second.PositionInGrid.Deconstruct(out var x2, out var y2);
            var tmp = grid.Cells[x1, y1].Color;
            grid.Cells[x1, y1].Color = grid.Cells[x2, y2].Color;
            grid.Cells[x2, y2].Color = tmp;
            return grid;
        }

        private void SwapCells(Entity first, Entity second)
        {
            first.Set(new TargetPosition { Position = second.Get<Transform>().Position });
            second.Set(new TargetPosition { Position = first.Get<Transform>().Position });
        }

        private void SwapBack(Entity first, Entity second)
        {
            first.Set(new OriginalPosition { Value = first.Get<Transform>().Position });
            second.Set(new OriginalPosition { Value = second.Get<Transform>().Position });
        }
    }
}
