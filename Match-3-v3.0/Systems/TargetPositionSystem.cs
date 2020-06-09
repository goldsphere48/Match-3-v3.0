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
    [With(typeof(Transform))]
    [With(typeof(TargetPosition))]
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
            var transform = entity.Get<Transform>();
            var targetPosition = entity.Get<TargetPosition>();
            var direction = Vector2.Normalize(Vector2.Subtract(targetPosition.Position, transform.Position));
            if (IsOnThePlace(transform.Position, targetPosition.Position, step) == false)
            {
                transform.Position += Vector2.Multiply(direction, step);
                entity.Set(transform);
            } else
            {
                transform.Position = targetPosition.Position;
                entity.Set(transform);
                entity.Remove<TargetPosition>();
            }
        }

        private bool IsOnThePlace(Vector2 position, Vector2 target, float step)
        {
            var length = Vector2.Subtract(position, target).Length();
            return length <= step;
        }
    }
}
