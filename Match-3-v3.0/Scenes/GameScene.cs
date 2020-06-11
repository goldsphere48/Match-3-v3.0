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
using Match_3_v3._0.Utils;
using Match_3_v3._0.Data;

namespace Match_3_v3._0.Scenes
{
    class GameScene : BaseScene
    {
        private BackgroundFactory _backgroundFactory;
        private GridFactory _gridFactory;
        private Counter _scoreCounter;
        private Counter _timerCounter;
        private CellPool _cellPool;
        private GameState _gameState = GameState.Generating;

        public override void Setup(World world, out ISystem<float> systems)
        {
            _backgroundFactory = new BackgroundFactory(world);
            _gridFactory = new GridFactory(
                world, 
                _game.GraphicsDevice,
                PlayerPrefs.Get<int>("Width"),
                PlayerPrefs.Get<int>("Height"),
                PlayerPrefs.Get<int>("CellSize")
            );
            _cellPool = new CellPool(new CellFactory(world, PlayerPrefs.Get<int>("CellSize")));
            InitializeSystems(world, out systems);
            SetupWorld(world);
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
                new DebugSystem(world, _game.Window, _gameState),
                new CounterSystem(world),
                new TimerSystem(world),
                new GenerationSystem(world, _cellPool, _gameState),
                new SelectSystem(world, _game.Window, _gameState),
                new SwapSystem(world),
                new CancelSwapSystem(world),
                new SwapFinishedSystem(world),
                new FindMatchesSystem(world, _gameState),
                new CombinationSystem(world, _gameState),
                new DestroyersSystem(world, _gameState),
                new LineBonusSystem(world),
                new BombSystem(world),
                new FallSystem(world, _gameState),
                new TargetPositionSystem(world),
                new DestroyersDyingSystem(world),
                new WaitFallingSystem(world, _gameState),
                new RotationSystem(world),
                new TransformSystem(world, _runner),
                new FrameAnimationUpdateSystem(world),
                new SpriteRenderSystem(_batch, world.GetEntities().With<SpriteRenderer>().Without<Cell>().AsSet()),
                new FrameAnimationDrawSystem(_batch, world),
                new SpriteRenderSystem(_batch, world.GetEntities().With<SpriteRenderer>().With<Cell>().AsSet()),
                new SpriteRenderSystem(_batch, world.GetEntities().With<SpriteRenderer>().With<Destroyer>().AsSet(), 228),
                new TextRenderSystem(_batch, world),
                new DyingSystem(world, _gameState),
                new DelayedDyingSystem(world)
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
            _scoreCounter = new Counter(new CounterArgs
            {
                Title = "Score",
                World = world,
                Position = new Vector2(30, 30),
                InitialValue = 0
            });
        }

        private void CreateTimerBoard(World world)
        {
            _timerCounter = new Counter(new CounterArgs
            {
                Title = "Time",
                World = world,
                Position = new Vector2(510, 30),
                InitialValue = 6000
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
            _timerCounter.Value -= 1;
            if (_timerCounter.Value <= 0)
            {
                PlayerPrefs.Set("Score", _scoreCounter.Value);
                SceneManager.Instance.SetActiveScene<GameOverScene>();
            }
        }
    }
}
