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
        private readonly List<Entity?> _cells;

        public CellPool(CellFactory cellFactory)
        {
            _cellFactory = cellFactory;
            _cells = new List<Entity?>();
        }

        public Entity RequestCell(Cell cellInfo, float verticalOffset, Transform parent)
        {
            Entity? cellEntity = _cells.Find(e => !e.Value.IsEnabled() && e.Value.Get<Cell>().Color == cellInfo.Color);
            if (!cellEntity.HasValue)
            {
                cellEntity = _cellFactory.Create(cellInfo, parent);
                _cells.Add(cellEntity.Value);
            }
            else
            {
                Reset(cellEntity, cellInfo);
            }
            var localPosition = GetLocalPosition(cellInfo);
            PlaceCell(cellEntity, localPosition, verticalOffset);
            if (verticalOffset != 0)
            {
                cellEntity.Value.Set(new TargetPosition { Position = localPosition, UseLocalPosition = true });
            }
            return cellEntity.Value;
        }

        private Vector2 GetLocalPosition(Cell cellInfo) => cellInfo.PositionInGrid.ToVector2() * PlayerPrefs.Get<int>("CellSize");

        private void PlaceCell(Entity? cell, Vector2 localPosition, float verticalOffset)
        {
            var transform = cell.Value.Get<Transform>();
            transform.LocalPosition =
                Vector2.Add(
                    localPosition,
                    new Vector2(0, verticalOffset)
                );
            cell.Value.Set(transform);
        }

        private void Reset(Entity? entity, Cell cellInfo)
        {
            entity.Value.Enable();
            var component = entity.Value.Get<Cell>();
            component.PositionInGrid = cellInfo.PositionInGrid;
            entity.Value.Set(component);
        }
    }
}