using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Utils
{
    class CellPool
    {
        private CellFactory _cellFactory;
        private List<Entity?> _cells;

        public CellPool(CellFactory cellFactory)
        {
            _cellFactory = cellFactory;
            _cells = new List<Entity?>();
        }

        public Entity RequestCell(Cell cellInfo, float verticalOffset, Transform parent)
        {
            Entity? cellEntity = _cells.Find(e => !e.Value.IsEnabled() && e.Value.Get<Cell>().Color == cellInfo.Color);
            if (cellEntity.HasValue == false)
            {
                cellEntity = _cellFactory.Create(cellInfo, parent);
                _cells.Add(cellEntity.Value);
            }
            else
            {
                cellEntity.Value.Enable();
                var component = cellEntity.Value.Get<Cell>();
                component.PositionInGrid = cellInfo.PositionInGrid;
                cellEntity.Value.Set(component);
            }
            PlaceCell(cellEntity, cellInfo.PositionInGrid, PlayerPrefs.Get<int>("CellSize"), verticalOffset);
            return cellEntity.Value;
        }

        private void PlaceCell(Entity? cell, Point positionInGrid, int cellSize, float verticalOffset)
        {
            var transform = cell.Value.Get<Transform>();
            var localPosition = positionInGrid.ToVector2() * cellSize;
            transform.LocalPosition =
                Vector2.Add(
                    localPosition,
                    new Vector2(0, verticalOffset)
                );
            if (verticalOffset != 0)
            {
                cell.Value.Set(new TargetPosition { Position = localPosition, UseLocalPosition = true });
            }
            cell.Value.Set(transform);
        }
    }
}
