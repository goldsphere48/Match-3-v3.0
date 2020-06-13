using DefaultEcs;
using DefaultEcs.System;
using DefaultEcs.Threading;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Entities;
using Match_3_v3._0.EntityFactories;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Systems;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using SceneSystem;
using System;

namespace Match_3_v3._0.Scenes
{
    internal class GameScene : BaseScene
    {
        private ImageFactory _imageFactory;
        private EntitySet _backgroundLayer;
        private EntitySet _borderLayer;
        private CellPool _cellPool;
        private EntitySet _cellsLayer;
        private EntitySet _destroyersLayer;
        private GameState _gameState = GameState.Generating;
        private GridFactory _gridFactory;
        private Counter _scoreCounter;
        private Counter _timerCounter;

        public override void Setup(World world, out ISystem<float> systems)
        {
            _imageFactory = new ImageFactory(world);
            _gridFactory = new GridFactory(
                world,
                _game.GraphicsDevice,
                PlayerPrefs.Get<int>("Width"),
                PlayerPrefs.Get<int>("Height"),
                PlayerPrefs.Get<int>("CellSize")
            );
            _cellPool = new CellPool(new CellFactory(world, PlayerPrefs.Get<int>("CellSize")));
            InitLayers(world);
            InitializeSystems(world, out systems);
            SetupWorld(world);
            world.Subscribe(this);
        }

        private void CreateBorders()
        {
            var entity = _imageFactory.Create("borders");
            entity.Set<Borders>();
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
            _scoreCounter.GetEntity().Set<Score>();
        }

        private void CreateTimer(World world)
        {
            CreateTimerBoard(world);
            var entity = world.CreateEntity();
            entity.Set(new Timer { Interval = 1, TimerTick = OnTimerTick });
        }

        private void CreateTimerBoard(World world)
        {
            _timerCounter = new Counter(new CounterArgs
            {
                Title = "Time",
                World = world,
                Position = new Vector2(510, 30),
                InitialValue = PlayerPrefs.Get<int>("RoundTime")
            });
        }

        private void InitializeSystems(World world, out ISystem<float> systems)
        {
            var _runner = new DefaultParallelRunner(Environment.ProcessorCount);
            systems = new SequentialSystem<float>(
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
                new WaitFallingSystem(world, _gameState),
                new RotationSystem(world),
                new TransformSystem(world, _runner),
                new FrameAnimationUpdateSystem(world),
                new SpriteRenderSystem(_batch, _backgroundLayer),
                new FrameAnimationDrawSystem(_batch, world),
                new SpriteRenderSystem(_batch, _cellsLayer),
                new SpriteRenderSystem(_batch, _destroyersLayer),
                new SpriteRenderSystem(_batch, _borderLayer),
                new TextRenderSystem(_batch, world),
                new DestroyersDyingSystem(world),
                new DyingSystem(world, _gameState),
                new DelayedDyingSystem(world)
            );
        }

        private void InitLayers(World world)
        {
            _cellsLayer = world.GetEntities().With<SpriteRenderer>().With<Cell>().AsSet();
            _backgroundLayer = world.GetEntities().With<SpriteRenderer>().Without<Cell>().AsSet();
            _destroyersLayer = world.GetEntities().With<SpriteRenderer>().With<Destroyer>().AsSet();
            _borderLayer = world.GetEntities().With<SpriteRenderer>().With<Borders>().AsSet();
        }

        [Subscribe]
        private void On(in AddScoreMessage newScore)
        {
            _scoreCounter.Value += newScore.Value;
            PlayerPrefs.Set("Score", _scoreCounter.Value);
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

        private void SetupWorld(World world)
        {
            _imageFactory.Create("background", Color.White);
            _gridFactory.Create();
            CreateScoreBoard(world);
            CreateTimer(world);
            CreateBorders();
        }
    }
}