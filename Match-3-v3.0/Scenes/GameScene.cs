﻿using SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.ResourceManagers;
using Match_3_v3._0.Systems;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;
using DefaultEcs.Resource;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Match_3_v3._0.EntityFactories;

namespace Match_3_v3._0.Scenes
{
    class GameScene : BaseScene
    {
        private BackgroundFactory _backgroundFactory;
        private GridFactory _gridFactory;
        private TextFactory _textFactory;

        public override void Setup(World world, out ISystem<float> systems)
        {
            _backgroundFactory = new BackgroundFactory(world);
            _textFactory = new TextFactory(world);
            _gridFactory = new GridFactory(world, _game.GraphicsDevice, 8, 8);
            InitializeSystems(world, out systems);
            SetupWorld();
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
                new SpriteRenderSystem(_batch, world),
                new TransformSystem(world, _runner),
                new TextRenderSystem(_batch, world)
            );
        }

        private void SetupWorld()
        {
            _backgroundFactory.Create("background", Color.White);
            _gridFactory.Create();
            _textFactory.Create("font", "Score: ", new Vector2(30, 30));
        }
    }
}