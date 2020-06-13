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
        private int _speed = PlayerPrefs.Get<int>("Speed");

        public TargetPositionSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, ReadOnlySpan<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var targetPosition = entity.Get<TargetPosition>();
                var transform = entity.Get<Transform>();
                if (targetPosition.UseLocalPosition)
                {
                    transform.LocalPosition = GetNewPosition(entity, transform.LocalPosition, targetPosition.Position, state);
                }
                else
                {
                    transform.Position = GetNewPosition(entity, transform.Position, targetPosition.Position, state);
                }
                entity.Set(transform);
            }
        }

        private Vector2 GetNewPosition(Entity entity, Vector2 currentPosition, Vector2 targetPosition, float state)
        {
            Vector2 newPosition = currentPosition;
            var offset = _speed * state;
            var step = GetStep(GetDirection(targetPosition, currentPosition), offset);
            if (!IsOnThePlace(newPosition, targetPosition, offset))
            {
                newPosition += step;
            }
            else
            {
                newPosition = targetPosition;
                entity.Remove<TargetPosition>();
            }
            return newPosition;
        }

        private Vector2 GetStep(Vector2 direction, float offset) => direction * offset;

        private Vector2 GetDirection(Vector2 target, Vector2 transform) 
            => Vector2.Normalize(Vector2.Subtract(target, transform));

        private bool IsOnThePlace(Vector2 position, Vector2 target, float step)
        {
            var length = Vector2.Subtract(position, target).Length();
            return length <= step;
        }
    }
}
