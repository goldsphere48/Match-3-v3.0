using SceneSystem;
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
using Match_3_v3._0.Entities;

namespace Match_3_v3._0.Scenes
{
    class GameScene : BaseScene
    {
        private BackgroundFactory _backgroundFactory;
        private GridFactory _gridFactory;
        private TextFactory _textFactory;
        private Counter _score;
        private Counter _timer;

        public override void Setup(World world, out ISystem<float> systems)
        {
            _backgroundFactory = new BackgroundFactory(world);
            _textFactory = new TextFactory(world);
            _gridFactory = new GridFactory(world, _game.GraphicsDevice, 8, 8);
            InitializeSystems(world, out systems);
            SetupWorld(world);
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
                new SpriteRenderSystem(_batch, world),
                new TransformSystem(world, _runner),
                new TextRenderSystem(_batch, world),
                new CounterSystem(world),
                new TimerSystem(world)
            );
        }

        private void SetupWorld(World world)
        {
            _backgroundFactory.Create("background", Color.White);
            _gridFactory.Create();
            CreateScoreBoard(world);
            CreateTimer(world);
        }

        private void CreateScoreBoard(World world)
        {
            _score = new Counter(new CounterArgs
            {
                Title = "Score",
                World = world,
                Position = new Vector2(30, 30),
                InitialValue = 0
            });
        }

        private void CreateTimerBoard(World world)
        {
            _timer = new Counter(new CounterArgs
            {
                Title = "Time",
                World = world,
                Position = new Vector2(510, 30),
                InitialValue = 3
            });
        }

        private void CreateTimer(World world)
        {
            CreateTimerBoard(world);
            var entity = world.CreateEntity();
            entity.Set(new Timer { Interval = 1, TimerTick = OnTimerTick});
        }

        private void OnTimerTick()
        {
            _timer.Value -= 1;
            if (_timer.Value <= 0)
            {
                PlayerPrefs.Set("Score", _score.Value);
                SceneManager.Instance.SetActiveScene<GameOverScene>();
            }
        }
    }
}
