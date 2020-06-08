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
    class FallSystem : AEntitySystem<float>
    {
        private int _fallSpeed = 100;

        public FallSystem(World world)
            : base(world)
        {
            
        }

        protected override void Update(float state, in Entity entity)
        {
            var cell = entity.Get<Cell>();
            var transform = entity.Get<Transform>();
            var targetPosition = GetTargetPosition(cell);
            if (transform.LocalPosition.Y < targetPosition.Y)
            {
                transform.LocalPosition += new Vector2(0, _fallSpeed * state);
                entity.Set(transform);
            } else if (transform.LocalPosition.Y > targetPosition.Y)
            {
                transform.LocalPosition = targetPosition;
                entity.Set(transform);
            }
        }

        private Vector2 GetTargetPosition(Cell cell)
        {
            return Vector2.Multiply(cell.PositionInGrid, PlayerPrefs.Get<int>("CellSize"));
        }
    }
}
