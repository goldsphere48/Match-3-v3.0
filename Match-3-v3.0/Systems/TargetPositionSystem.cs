using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    [With(typeof(Cell))]
    [With(typeof(Transform))]
    [With(typeof(TargetPosition))]
    [With(typeof(FrameAnimation))]
    class TargetPositionSystem : AEntitySystem<float>
    {
        private int _speed = 200;

        public TargetPositionSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, in Entity entity)
        {
            var step = _speed * state;
            var cell = entity.Get<Cell>();
            var transform = entity.Get<Transform>();
            var targetPosition = entity.Get<TargetPosition>();
            var direction = Vector2.Subtract(targetPosition.Position, cell.PositionInGrid);
            var targetLocalPosition = GetLocalPosition(targetPosition.Position);
            if (IsOnThePlace(transform.LocalPosition, targetLocalPosition, step) == false)
            {
                transform.LocalPosition += Vector2.Multiply(direction, step);
                entity.Set(transform);
            } else
            {
                transform.LocalPosition = targetLocalPosition;
                entity.Set(transform);
                entity.Remove<TargetPosition>();
            }
        }

        private Vector2 GetLocalPosition(Vector2 positionInGrid)
        {
            return Vector2.Multiply(positionInGrid, PlayerPrefs.Get<int>("CellSize"));
        }

        private bool IsOnThePlace(Vector2 position, Vector2 target, float step)
        {
            return Vector2.Subtract(position, target).Length() <= step;
        }
    }
}
