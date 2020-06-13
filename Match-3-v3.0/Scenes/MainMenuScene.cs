using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Systems;
using Microsoft.Xna.Framework;
using SceneSystem;
using System;

namespace Match_3_v3._0.Scenes
{
    internal class MainMenuScene : BaseScene
    {
        private BackgroundFactory _backgroundFactory;
        private ButtonFactory _buttonFactory;

        public override void Setup(World world, out ISystem<float> systems)
        {
            _backgroundFactory = new BackgroundFactory(world);
            _buttonFactory = new ButtonFactory(world, _game.GraphicsDevice);
            InitializeSystems(world, out systems);
            SetupWorld();
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
                new SpriteRenderSystem(_batch, world.GetEntities().With<SpriteRenderer>().AsSet()),
                new TransformSystem(world, _runner),
                new ButtonSystem(world, _game.Window)
            );
        }

        private void Play()
        {
            SceneManager.Instance.SetActiveScene<GameScene>();
        }

        private void SetupWorld()
        {
            _backgroundFactory.Create("background", Color.White);
            _buttonFactory.CreateAtCenter("playButton", Play);
        }
    }
}