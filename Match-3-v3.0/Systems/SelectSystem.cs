﻿using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
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
        private Entity? _selected = null;
        private World _world;

        public SelectSystem(World world, GameWindow window)
            : base(world.GetEntities().With(typeof(FrameAnimation)).With(typeof(Cell)).AsSet(), window)
        {
            _world = world;
        }

        protected override void Update(float state, in Entity entity)
        {
            if (_state.LeftButton == ButtonState.Released && _oldState.LeftButton == ButtonState.Pressed)
            {
                var position = _state.Position.ToVector2();
                var currentAnimation = entity.Get<FrameAnimation>();
                var currentCell = entity.Get<Cell>();
                if (currentAnimation.Destination.Contains(position))
                {
                    if (_selected == null)
                    {
                        _selected = entity;
                        currentAnimation.Play = true;
                    } else if (_selected == entity)
                    {
                        _selected = null;
                        currentAnimation.Play = false;
                    } else
                    {
                        currentAnimation.Play = true;
                        var selectedCell = _selected.Value.Get<Cell>();
                        if (GridUtil.IsNeighbours(selectedCell, currentCell))
                        {
                            _selected.Value.Set(new TargetPosition { Position = entity.Get<Transform>().Position });
                            entity.Set(new TargetPosition { Position = _selected.Value.Get<Transform>().Position });
                            var grid = _world.First(e => e.Has<Grid>());
                            grid.Set(new Swap(_selected.Value, entity));
                        } 
                        else
                        {
                            _selected.Value.Get<FrameAnimation>().Play = false;
                            currentAnimation.Play = false;
                            _selected = null;
                        }
                    }
                }
            }
        }
    }
}
