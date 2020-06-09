using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class SelectSystem : InputSystem
    {
        private Entity? _firstSelected = null;
        private Entity? _secondSelected = null;
        private World _world;

        public SelectSystem(World world, GameWindow window)
            : base(world.GetEntities().With(typeof(FrameAnimation)).With(typeof(Cell)).AsSet(), window)
        {
            _world = world;
        }

        private void On(SwapFinishedMessage _)
        {
            Unselect(ref _firstSelected);
            Unselect(ref _secondSelected);
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_state.LeftButton == ButtonState.Released && _oldState.LeftButton == ButtonState.Pressed)
            {
                var position = _state.Position.ToVector2();
                var currentAnimation = entity.Get<FrameAnimation>();
                if (currentAnimation.Destination.Contains(position))
                {
                    Select(entity);
                }
            }
        }

        private void Select(Entity entity)
        {
            if (_firstSelected == null)
            {
                Select(entity, ref _firstSelected);
            } else if (_secondSelected == null)
            {
                Select(entity, ref _secondSelected);
            }
            if (TryGetSelectedCells(out var firstCell, out var secondCell))
            {
                var isNeighbours = GridUtil.IsNeighbours(firstCell, secondCell);
                if (_firstSelected == _secondSelected || isNeighbours == false)
                {
                    Unselect(ref _firstSelected);
                    Unselect(ref _secondSelected);
                } else
                {
                    var grid = _world.First(e => e.Has<Grid>());
                    grid.Set(new Swap(_firstSelected.Value, _secondSelected.Value));
                }
            }
        }

        private bool TryGetSelectedCells(out Cell first, out Cell second)
        {
            if (_secondSelected.HasValue && _firstSelected.HasValue)
            {
                first = _firstSelected.Value.Get<Cell>();
                second = _secondSelected.Value.Get<Cell>();
                return true;
            }
            first = null;
            second = null;
            return false;
        }

        private void Select(Entity entity, ref Entity? contanier)
        {
            contanier = entity;
            entity.Get<FrameAnimation>().Play = true;
            entity.Set<Selected>();
        }

        private void Unselect(ref Entity? entity)
        {
            if (entity.HasValue)
            {
                entity.Value.Get<FrameAnimation>().Play = false;
                entity.Value.Remove<Selected>();
                entity = null;
            }
        }
    }
}
