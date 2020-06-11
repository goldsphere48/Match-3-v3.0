﻿using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{

    struct GridVisitorArgs
    { 
        public Grid Grid;
        public List<Cell> Solutions;
        public Dictionary<Point, bool> Visited;

        public GridVisitorArgs(Grid grid, ref Dictionary<Point, bool> visited)
        {
            Grid = grid;
            Solutions = null;
            Visited = visited;
        }
    }

    [With(typeof(Grid))]
    class FindMatchesSystem : AEntitySystem<float>
    {
        private GameState _gameState;
        private World _world;

        public FindMatchesSystem(World world, GameState initState)
            : base(world)
        { 
            _world = world;
            _world.Subscribe(this);
            _gameState = initState;
        }

        [Subscribe]
        private void On(in NewStateMessage newState)
        {
            _gameState = newState.Value;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_gameState == GameState.Matching)
            {
                var grid = entity.Get<Grid>();
                var combinations = FindMatches(grid);
                if (combinations.Count() > 0)
                {
                    CheckCombinations(combinations);
                } else
                {
                    _world.Publish(new NewStateMessage { Value = GameState.WaitForUserInput });
                }
            }
        }

        private void CheckCombinations(IEnumerable<Combination> combinations)
        {
            _world.CreateEntity().Set(new CombinationsArray { Value = combinations });
            _world.Publish(new NewStateMessage { Value = GameState.CombinationChecking });

        }

        public static IEnumerable<Combination> FindMatches(Grid grid)
        {
            var visited = new Dictionary<Point, bool>(grid.Width * grid.Height);
            var args = new GridVisitorArgs(grid, ref visited);
            List<Combination> combinations = new List<Combination>();
            for (int i = 0; i < grid.Width; ++i)
            {
                for (int j = 0; j < grid.Height; ++j)
                {
                    args.Solutions = new List<Cell>();
                    Find(ref args, grid.Cells[i, j]);
                    if (args.Solutions.Count >= 3)
                    {
                        foreach (var combination in GetCombinations(args.Solutions, grid.Width, grid.Height))
                        {
                            combinations.Add(combination);
                        }
                    }
                }
            }
            return combinations;
        }

        private static List<Combination> InitAmountList(int size)
        {
            var list = new List<Combination>(size);
            for (int i = 0; i < size; ++i)
            {
                list.Add(new Combination());
            }
            return list;
        }

        private static IEnumerable<Combination> GetCombinations(List<Cell> solution, int width, int height)
        {
            var xAmount = InitAmountList(width);
            var yAmount = InitAmountList(height);

            for (int i = 0; i < solution.Count; ++i)
            {
                var position = solution[i].PositionInGrid;
                xAmount[position.X].Add(position);
                yAmount[position.Y].Add(position);
            }

            for (int i = 0; i < yAmount.Count; i++)
            {
                if (yAmount[i].Count >= 3)
                {
                    yAmount[i].Orientation = Orientation.Horizontal;
                    yield return yAmount[i];
                }
            }

            for (int i = 0; i < xAmount.Count; i++)
            {
                if (xAmount[i].Count >= 3)
                {
                    xAmount[i].Orientation = Orientation.Vertical;
                    yield return xAmount[i];
                }
            }

        }

        public static void Find(ref GridVisitorArgs args, Cell currentCell)
        {
            args.Visited.TryGetValue(currentCell.PositionInGrid, out bool status);
            if (status == false)
            {
                Visit(ref args, currentCell);
            }
        }

        internal static IEnumerable<Combination> FindPossibleMatches(Grid grid)
        {
            /*           
                /(TT..{number_of_columns - 1}T)/
                /(T.T.{number_of_columns - 2}T)/
                /(.TT.{number_of_columns - 3}T)/
             */
            /*           
                /(T.{number_of_columns - 1}T.{number_of_columns}T)/
                /(T.{number_of_columns}T.{number_of_columns - 2}T)/
                /(.T.{number_of_columns - 2}T.{number_of_columns - 1}T)/
            */
            return Enumerable.Empty<Combination>();
        }

        private static void Visit(ref GridVisitorArgs args, Cell currentCell)
        {
            ref var visited = ref args.Visited;
            ref var solutions = ref args.Solutions;
            var grid = args.Grid;

            visited.TryGetValue(currentCell.PositionInGrid, out bool status);
            if (status == true)
            {
                return;
            }
            visited.Add(currentCell.PositionInGrid, true);
            solutions.Add(currentCell);
            var neighbours = GridUtil.GetNeighbours(currentCell.PositionInGrid, grid.Width, grid.Height);

            CheckNeighbour(currentCell, Neighbours.Top, neighbours, ref args);
            CheckNeighbour(currentCell, Neighbours.Right, neighbours, ref args);
            CheckNeighbour(currentCell, Neighbours.Bottom, neighbours, ref args);
            CheckNeighbour(currentCell, Neighbours.Left, neighbours, ref args);
        }

        private static void CheckNeighbour(Cell currentCell, Neighbours target, Neighbours neighbours, ref GridVisitorArgs args)
        {
            var grid = args.Grid;
            if (IsSameColor(target, neighbours, currentCell, grid))
            {
                var position = currentCell.PositionInGrid + GridUtil.NeighbourToVector2(target);
                currentCell = grid.Cells[position.X, position.Y];
                Visit(ref args, currentCell);
            }
        }

        private static bool IsSameColor(Neighbours target, Neighbours neighbours, Cell cell, Grid grid)
        {
            if ((target & neighbours) == 0)
            {
                return false;
            }
            var position = cell.PositionInGrid + GridUtil.NeighbourToVector2(target);
            if (cell.Color == grid.Cells[position.X, position.Y].Color)
            {
                return true;
            }
            return false;
        }
    }
}
