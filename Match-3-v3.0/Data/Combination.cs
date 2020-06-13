using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Match_3_v3._0.Data
{
    internal enum LineOrientation
    {
        Vertical,
        Horizontal
    }

    internal class Combination : IEnumerable<Point>
    {
        private List<Point> _cellPositions = new List<Point>();

        public int Count => _cellPositions.Count;
        public LineOrientation Orientation { get; set; }

        public void Add(Point p)
        {
            _cellPositions.Add(p);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _cellPositions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cellPositions.GetEnumerator();
        }
    }
}