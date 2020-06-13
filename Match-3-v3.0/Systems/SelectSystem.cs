using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.Data;
using Match_3_v3._0.Messages;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Match_3_v3._0.Systems
{
    internal class SelectSystem : InputSystem
    {
        private readonly World _world;
        private Entity? _firstSelected = null;
        private GameState _gameState;
        private Entity? _secondSelected = null;

        public SelectSystem(World world, GameWindow window, GameState initState)
            : base(world.GetEntities().With(typeof(FrameAnimation)).With(typeof(Cell)).AsSet(), window)
        {
            _world = world;
            _world.Subscribe(this);
            _gameState = initState;
        }

        protected override void Update(float state, in Entity cellEntity)
        {
            if (_gameState == GameState.WaitForUserInput)
            {
                if (_state.LeftButton == ButtonState.Released && _oldState.LeftButton == ButtonState.Pressed)
                {
                    var mousePosition = _state.Position.ToVector2();
                    var currentAnimation = cellEntity.Get<FrameAnimation>();
                    if (currentAnimation.Destination.Contains(mousePosition))
                    {
                        Select(cellEntity);
                    }
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
            _world.Publish(new NewStateMessage { Value = GameState.Swapping });
        }

        [Subscribe]
        private void On(in UnselectMessage _)
        {
            Unselect(ref _firstSelected);
            Unselect(ref _secondSelected);
        }

        [Subscribe]
        private void On(in NewStateMessage newState) => _gameState = newState.Value;

        private void Select(Entity cellEntity)
        {
            if (_firstSelected == null)
            {
                Select(cellEntity, ref _firstSelected);
            }
            else if (_secondSelected == null)
            {
                Select(cellEntity, ref _secondSelected);
            }
            if (_secondSelected.HasValue && _firstSelected.HasValue)
            {
                if (CanSwap(_firstSelected.Value, _secondSelected.Value))
                {
                    CreateSwap(_firstSelected.Value, _secondSelected.Value);
                }
                else
                {
                    Unselect(ref _firstSelected);
                    Unselect(ref _secondSelected);
                }
            }
        }

        private void Select(Entity cellEntity, ref Entity? contanier)
        {
            contanier = cellEntity;
            cellEntity.Get<FrameAnimation>().Play = true;
            cellEntity.Set<Selected>();
        }

        private void Unselect(ref Entity? cellEntity)
        {
            if (cellEntity.HasValue)
            {
                cellEntity.Value.Remove<Selected>();
                cellEntity.Value.Get<FrameAnimation>().Play = false;
                cellEntity = null;
            }
        }
    }
}