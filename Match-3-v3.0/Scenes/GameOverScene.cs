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

namespace Match_3_v3._0.Scenes
{
    internal class GameOverScene : BaseScene
    {
        private ImageFactory _imageFactory;
        private ButtonFactory _buttonFactory;
        private TextFactory _textFactory;

        public void GoToMenu()
        {
            SceneManager.Instance.SetActiveScene<MainMenuScene>();
        }

        public override void Setup(World world, out ISystem<float> systems)
        {
            _imageFactory = new ImageFactory(world);
            _buttonFactory = new ButtonFactory(world, _game.GraphicsDevice);
            _textFactory = new TextFactory(world);
            InitializeSystems(world, out systems);
            SetupWorld(world);
        }

        private void CreateGameOver(World world)
        {
            var entity = world.CreateEntity();
            entity.Set(new SpriteRenderer());
            entity.Set(new ManagedResource<string, Texture2D>("gameOver"));
            var position = SceneUtil.GetCenterFor(entity, _game.GraphicsDevice);
            entity.Set(new Transform
            {
                Position = Vector2.Subtract(position, new Vector2(0, 200))
            });
        }

        private void CreateOkButton()
        {
            var button = _buttonFactory.CreateAtCenter("okButton", GoToMenu);
            var transform = button.Get<Transform>();
            transform.Position = Vector2.Add(transform.Position, new Vector2(0, 180));
        }

        private void CreateScoreText()
        {
            var entity = _textFactory.Create(
                new TextArgs
                {
                    FontName = "font",
                    Text = $"Score: {PlayerPrefs.Get<int>("Score")}",
                    Color = new Color(161, 63, 16)
                }
            );
            var position = SceneUtil.GetCenterFor(entity, _game.GraphicsDevice);
            entity.Set(new Transform { Position = position });
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
                new SpriteRenderSystem(_batch, world.GetEntities().With<SpriteRenderer>().AsSet()),
                new ButtonSystem(world, _game.Window),
                new TransformSystem(world, _runner),
                new TextRenderSystem(_batch, world),
                new TextSystem(world)
            );
        }

        private void SetupWorld(World world)
        {
            _imageFactory.Create("background");
            CreateGameOver(world);
            CreateScoreText();
            CreateOkButton();
        }
    }
}