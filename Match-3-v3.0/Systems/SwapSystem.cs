using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using System.Linq;

namespace Match_3_v3._0.Systems
{
    [WhenAdded(typeof(Swap))]
    [With(typeof(Swap))]
    [With(typeof(Grid))]
    internal class SwapSystem : AEntitySystem<float>
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
            Swap(grid, firstCell, secondCell);
            SetNewPostions(first, second);
            var matches = FindMatchesSystem.FindMatches(grid).Count();
            if (matches == 0)
            {
                Swap(grid, secondCell, firstCell);
                first.Set(new SwapSuccess { Value = SwapResult.Fail });
                SetOriginPositions(first, second);
            }
            else
            {
                SwapPostions(ref firstCell, ref secondCell);
                first.Set(firstCell);
                second.Set(secondCell);
                first.Set(new SwapSuccess { Value = SwapResult.Success });
                entity.Set(grid);
                entity.Remove<Swap>();
            }
        }

        private void SetNewPostions(Entity first, Entity second)
        {
            first.Set(new TargetPosition { Position = second.Get<Transform>().Position });
            second.Set(new TargetPosition { Position = first.Get<Transform>().Position });
        }

        private void SetOriginPositions(Entity first, Entity second)
        {
            first.Set(new OriginalPosition { Value = first.Get<Transform>().Position });
            second.Set(new OriginalPosition { Value = second.Get<Transform>().Position });
        }

        private void Swap(Grid grid, Cell first, Cell second)
        {
            first.PositionInGrid.Deconstruct(out var x1, out var y1);
            second.PositionInGrid.Deconstruct(out var x2, out var y2);
            SwapColor(ref grid.Cells[x1, y1], ref grid.Cells[x2, y2]);
        }

        private void SwapColor(ref Cell first, ref Cell second)
        {
            var tmp = first.Color;
            first.Color = second.Color;
            second.Color = tmp;
        }

        private void SwapPostions(ref Cell first, ref Cell second)
        {
            var tmp = first.PositionInGrid;
            first.PositionInGrid = second.PositionInGrid;
            second.PositionInGrid = tmp;
        }
    }
}