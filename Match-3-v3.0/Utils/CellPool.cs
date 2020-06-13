using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Match_3_v3._0.Utils
{
    internal class CellPool
    {
        private readonly CellFactory _cellFactory;
        private readonly List<Entity?> data;

        public CellPool(CellFactory cellFactory)
        {
            _cellFactory = cellFactory;
            data = new List<Entity?>();
        }

        public Entity RequestCell(Cell cell, float verticalOffset, Transform parent)
        {
            Entity? cellEntity = data.Find(e => !e.Value.IsEnabled() && e.Value.Get<Cell>().Color == cell.Color);
            if (!cellEntity.HasValue)
            {
                cellEntity = _cellFactory.Create(cell, parent);
                data.Add(cellEntity.Value);
            }
            else
            {
                Reset(cellEntity, cell);
            }
            var localPosition = GetLocalPosition(cell);
            PlaceCell(cellEntity, localPosition, verticalOffset);
            if (verticalOffset != 0)
            {
                cellEntity.Value.Set(new TargetPosition { Position = localPosition, UseLocalPosition = true });
            }
            return cellEntity.Value;
        }

        private Vector2 GetLocalPosition(Cell cell) => cell.PositionInGrid.ToVector2() * PlayerPrefs.Get<int>("CellSize");

        private void PlaceCell(Entity? cellEntity, Vector2 localPosition, float verticalOffset)
        {
            var transform = cellEntity.Value.Get<Transform>();
            transform.LocalPosition =
                Vector2.Add(
                    localPosition,
                    new Vector2(0, verticalOffset)
                );
            cellEntity.Value.Set(transform);
        }

        private void Reset(Entity? cellEntity, Cell cell)
        {
            cellEntity.Value.Enable();
            var newCell = cellEntity.Value.Get<Cell>();
            newCell.PositionInGrid = cell.PositionInGrid;
            cellEntity.Value.Set(newCell);
        }
    }
}