using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Microsoft.Xna.Framework;

namespace Match_3_v3._0.Entities
{
    internal class Counter
    {
        private readonly Entity _entity;
        private string _title;
        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _entity.Set(new CountPresenter(_title, _value));
            }
        }

        public Counter(CounterArgs args)
        {
            _entity = new TextFactory(args.World).Create(
                new TextArgs
                {
                    FontName = "font",
                    Text = $"{args.Title}: {args.InitialValue}",
                    Position = args.Position,
                    Color = new Color(161, 63, 16)
                }
            );
            _title = args.Title;
            _value = args.InitialValue;
        }

        public Entity GetEntity()
        {
            return _entity;
        }
    }

    internal class CounterArgs
    {
        public int InitialValue { get; set; }
        public Vector2 Position { get; set; }
        public string Title { get; set; }
        public World World { get; set; }
    }
}