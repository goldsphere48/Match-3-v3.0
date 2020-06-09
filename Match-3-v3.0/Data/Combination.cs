using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Data
{
    enum Orientation
    {
        Vertical,
        Horizontal
    }

    class Combination : IEnumerable<Point>
    {
        private List<Point> _cellPositions = new List<Point>();

        public Orientation Orientation { get; set; }
        public int Count => _cellPositions.Count;

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
