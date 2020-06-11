using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    class Transform
    {
        public float Angle { get; set; }
        public Vector2 Origin { get; set; }
        public List<Transform> Childrens { get; } = new List<Transform>();
        public Vector2 LocalPosition
        {
            get
            {
                if (Parent != null)
                {
                    return _position - Parent.Position;
                }
                return _position;
            }
            set => HandleLocalPositionChange(value);
        }
        public Transform Parent
        {
            get => _parent;
            set
            {
                if (value != null && value.Childrens.Contains(this) == false)
                {
                    value.Childrens.Add(this);

                }
                else if (value == null && _parent == null)
                {
                    _parent.Childrens.Remove(this);
                }
                _parent = value;
            }
        }
        public Vector2 Position { get => _position; set => HandleAbsolutePositionChange(value); }
        private Transform _parent;
        private Vector2 _position = Vector2.Zero;

        public void MoveTowards(Vector2 dir, Vector2 speed)
        {
            var newVector = new Vector2(dir.X - Position.X, dir.Y - Position.Y);
            newVector.Normalize();
            newVector *= speed;
            Position += newVector;
        }

        private void HandleAbsolutePositionChange(Vector2 newAbsolutePosition)
        {
            if (newAbsolutePosition != _position)
            {
                var dv = newAbsolutePosition - _position;
                _position = newAbsolutePosition;
                foreach (var child in Childrens)
                {
                    child.Position += dv;
                }
            }
        }

        private void HandleLocalPositionChange(Vector2 newLocalPosition)
        {
            if (Parent != null)
            {
                Position = newLocalPosition + Parent.Position;
            }
            else
            {
                Position = newLocalPosition;
            }
        }
    }
}
