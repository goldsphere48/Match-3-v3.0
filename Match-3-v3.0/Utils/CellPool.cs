﻿using DefaultEcs;
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
            Entity? cell = _cells.Find(e => !e.Value.IsAlive && e.Value.Get<Cell>().Color == cellInfo.Color);
            if (cell.HasValue == false)
            {
                cell = _cellFactory.Create(cellInfo, parent);
                _cells.Add(cell.Value);
            }
            else
            {
                var component = cell?.Get<Cell>();
                component.PositionInGrid = cellInfo.PositionInGrid;
                cell.Value.Set(component);
            }
            PlaceCell(cell, cellInfo.PositionInGrid, PlayerPrefs.Get<int>("CellSize"), verticalOffset);
            return cell.Value;
        }

        internal void Disable(Entity entity)
        {
            var cell = _cells.Find(e => e.Value.Get<Cell>().PositionInGrid == entity.Get<Cell>().PositionInGrid);
            if (cell.HasValue)
            {
                cell.Value.Disable();
            }
        }

        private void PlaceCell(Entity? cell, Point positionInGrid, int cellSize, float verticalOffset)
        {
            var transform = cell.Value.Get<Transform>();
            transform.LocalPosition =
                Vector2.Add(
                    Vector2.Multiply(
                        positionInGrid.ToVector2(),
                        cellSize
                    ),
                    new Vector2(0, verticalOffset)
                );
            cell.Value.Set(transform);
        }
    }
}
