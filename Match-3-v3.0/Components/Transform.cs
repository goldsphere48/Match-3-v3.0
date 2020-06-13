using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Match_3_v3._0.Components
{
    internal class Transform
    {
        private Transform _parent;
        private Vector2 _position = Vector2.Zero;
        public float Angle { get; set; }
        public List<Transform> Children { get; } = new List<Transform>();

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

        public Vector2 Origin { get; set; }

        public Transform Parent
        {
            get => _parent;
            set
            {
                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                }
                else if (value == null && _parent == null)
                {
                    _parent.Children.Remove(this);
                }
                _parent = value;
            }
        }

        public Vector2 Position { get => _position; set => HandleAbsolutePositionChange(value); }

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
                foreach (var child in Children)
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