using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Match_3_v3._0.Systems
{
    internal struct GridVisitorArgs
    {
        public IEnumerable<Neighbours> CheckingNeighbours;
        public Grid Grid;
        public List<Cell> Solution;
        public Dictionary<Point, bool> Visited;

        public GridVisitorArgs(Grid grid, Dictionary<Point, bool> visited, IEnumerable<Neighbours> neighbours)
        {
            Grid = grid;
            Solution = null;
            Visited = visited;
            CheckingNeighbours = neighbours;
        }
    }

    [With(typeof(Grid))]
    internal class FindMatchesSystem : AEntitySystem<float>
    {
        private readonly World _world;
        private GameState _gameState;

        private static IEnumerable<Neighbours> AllNeighbours =>
            Enum.GetValues(typeof(Neighbours)) as IEnumerable<Neighbours>;

        private static IEnumerable<Neighbours> NotCornerNeighbours =>
            new Neighbours[] { Neighbours.Top, Neighbours.Right, Neighbours.Bottom, Neighbours.Left };

        public FindMatchesSystem(World world, GameState initState)
            : base(world)
        {
            _world = world;
            _world.Subscribe(this);
            _gameState = initState;
        }

        public static void Find(GridVisitorArgs args, Cell startCell)
        {
            args.Visited.TryGetValue(startCell.PositionInGrid, out bool status);
            if (!status)
            {
                Visit(args, startCell);
            }
        }

        public static IEnumerable<List<Cell>> FindCombinations(Grid grid, IEnumerable<Neighbours> checkingNeighbours)
        {
            var visited = new Dictionary<Point, bool>(grid.Width * grid.Height);
            var args = new GridVisitorArgs(grid, visited, checkingNeighbours);
            for (int i = 0; i < grid.Width; ++i)
            {
                for (int j = 0; j < grid.Height; ++j)
                {
                    args.Solution = new List<Cell>();
                    Find(args, grid.Cells[i, j]);
                    if (args.Solution.Count >= 3)
                    {
                        yield return args.Solution;
                    }
                }
            }
        }

        public static IEnumerable<Combination> FindMatches(Grid grid)
        {
            var combinations = FindCombinations(grid, NotCornerNeighbours);
            foreach (var combination in combinations)
            {
                foreach (var match in GetMatches(combination, grid.Width, grid.Height))
                {
                    yield return match;
                }
            }
        }

        public static void InitAmountLists(List<Cell> solution, int width, int height, out List<Combination> xAmount, out List<Combination> yAmount)
        {
            xAmount = InitAmountList(width);
            yAmount = InitAmountList(height);

            for (int i = 0; i < solution.Count; ++i)
            {
                var cellPosition = solution[i].PositionInGrid;
                xAmount[cellPosition.X].Add(cellPosition);
                yAmount[cellPosition.Y].Add(cellPosition);
            }
        }

        public static bool IsGridValid(Grid grid)
        {
            var combinations = FindCombinations(grid, AllNeighbours);
            foreach (var combination in combinations)
            {
                var matchesCount = GetMatches(combination, grid.Width, grid.Height).Count();
                if (matchesCount > 0 || IsPossibleMatchExist(combination, grid.Width, grid.Height))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void Update(float state, in Entity gridEntity)
        {
            if (_gameState == GameState.Matching)
            {
                var grid = gridEntity.Get<Grid>();
                var combinations = FindMatches(grid);
                if (combinations.Count() > 0)
                {
                    CheckCombinations(combinations);
                }
                else
                {
                    _world.Publish(new NewStateMessage { Value = GameState.WaitForUserInput });
                }
            }
        }

        private static void CheckNeighbour(Cell visitedCell, Neighbours neighbour, Neighbours neighbours, GridVisitorArgs args)
        {
            var grid = args.Grid;
            if (IsSameColor(neighbour, neighbours, visitedCell, grid))
            {
                var neighbourPositionInGrid = GetNeighbourPositionInGrid(visitedCell, neighbour);
                visitedCell = grid.Cells[neighbourPositionInGrid.X, neighbourPositionInGrid.Y];
                Visit(args, visitedCell);
            }
        }

        private static IEnumerable<Combination> GetMatches(List<Cell> solution, int width, int height)
        {
            InitAmountLists(solution, width, height, out var xAmount, out var yAmount);

            for (int i = 0; i < yAmount.Count; i++)
            {
                if (yAmount[i].Count >= 3)
                {
                    yAmount[i].Orientation = LineOrientation.Horizontal;
                    yield return yAmount[i];
                }
            }

            for (int i = 0; i < xAmount.Count; i++)
            {
                if (xAmount[i].Count >= 3)
                {
                    xAmount[i].Orientation = LineOrientation.Vertical;
                    yield return xAmount[i];
                }
            }
        }

        private static Point GetNeighbourPositionInGrid(Cell cell, Neighbours neghbour)
        {
            return cell.PositionInGrid + GridUtil.NeighbourToVector2(neghbour);
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

        private static bool IsPossibleMatchExist(List<Cell> solution, int width, int height)
        {
            InitAmountLists(solution, width, height, out var xAmount, out var yAmount);
            var xSize = xAmount.FindAll(e => e.Count > 0).Count();
            var ySize = yAmount.FindAll(e => e.Count > 0).Count();
            return IsPossibleMatchExist(xSize, yAmount) || IsPossibleMatchExist(ySize, xAmount);
        }

        private static bool IsPossibleMatchExist(int size, List<Combination> amount)
        {
            if (size > 2)
            {
                for (int i = 0; i < amount.Count; ++i)
                {
                    if (amount[i].Count > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsSameColor(Neighbours neighbour, Neighbours neighbours, Cell cell, Grid grid)
        {
            if ((neighbour & neighbours) == 0)
            {
                return false;
            }
            var neighbourPositionInGrid = GetNeighbourPositionInGrid(cell, neighbour);
            if (cell.Color == grid.Cells[neighbourPositionInGrid.X, neighbourPositionInGrid.Y].Color)
            {
                return true;
            }
            return false;
        }

        private static void Visit(GridVisitorArgs args, Cell visitedCell)
        {
            args.Visited.TryGetValue(visitedCell.PositionInGrid, out bool status);
            if (status == true)
            {
                return;
            }
            args.Visited.Add(visitedCell.PositionInGrid, true);
            args.Solution.Add(visitedCell);
            var neighbours = GridUtil.GetNeighbours(visitedCell.PositionInGrid, args.Grid.Width, args.Grid.Height);

            foreach (var checkedNeighbour in args.CheckingNeighbours)
            {
                CheckNeighbour(visitedCell, checkedNeighbour, neighbours, args);
            }
        }

        private void CheckCombinations(IEnumerable<Combination> combinations)
        {
            _world.CreateEntity().Set(new CombinationsArray { Value = combinations });
            _world.Publish(new NewStateMessage { Value = GameState.CombinationChecking });
        }

        [Subscribe]
        private void On(in NewStateMessage newState) => _gameState = newState.Value;
    }
}