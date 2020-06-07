using DefaultEcs;
using DefaultEcs.Resource;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Systems;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Scenes
{
    class GameOverScene : BaseScene
    {
        private ButtonFactory _buttonFactory;
        private BackgroundFactory _backgroundFactory;

        public override void Setup(World world, out ISystem<float> systems)
        {
            _backgroundFactory = new BackgroundFactory(world);
            _buttonFactory = new ButtonFactory(world, _game.GraphicsDevice);
            InitializeSystems(world, out systems);
            SetupWorld(world);
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
                new SpriteRenderSystem(_batch, world),
                new ButtonSystem(world, _game.Window),
                new TransformSystem(world, _runner)
            );
        }

        private void SetupWorld(World world)
        {
            _backgroundFactory.Create("background");
            CreateGameOver(world);
            CreateOkButton();
        }

        public void GoToMenu()
        {
            SceneManager.Instance.SetActiveScene<MainMenuScene>();
        }

        private void CreateGameOver(World world)
        {
            var entity = world.CreateEntity();
            entity.Set(new SpriteRenderer());
            entity.Set(new ManagedResource<string, Texture2D>("gameOver"));
            var position = SceneUtil.GetCenterFor(entity, _game.GraphicsDevice);
            entity.Set(new Transform
            {
                Position = Vector2.Subtract(position, new Vector2(0, 100))
            });
        }

        private void CreateOkButton()
        {
            var button = _buttonFactory.CreateAtCenter("okButton", GoToMenu);
            var transform = button.Get<Transform>();
            transform.Position = Vector2.Add(transform.Position, new Vector2(0, 100));
        }
    }
}
