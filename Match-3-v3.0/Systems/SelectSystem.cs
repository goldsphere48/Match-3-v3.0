﻿using DefaultEcs;
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
            _world.Subscribe(this);
        }

        [Subscribe]
        private void On(in SwapFinishedMessage _)
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
            if (_secondSelected.HasValue && _firstSelected.HasValue)
            {
                if (CanSwap(_firstSelected.Value, _secondSelected.Value))
                {
                    CreateSwap(_firstSelected.Value, _secondSelected.Value);
                } else
                {

                    Unselect(ref _firstSelected);
                    Unselect(ref _secondSelected);
                }
            }
        }

        private bool CanSwap(Entity first, Entity second)
        {
            var isNeighbours = GridUtil.IsNeighbours(first.Get<Cell>(), second.Get<Cell>());
            return _firstSelected != _secondSelected && isNeighbours == true;
        }

        private void CreateSwap(Entity first, Entity second)
        {
            var grid = _world.First(e => e.Has<Grid>());
            grid.Set(new Swap(first, second));
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
