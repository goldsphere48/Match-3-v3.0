using DefaultEcs;
using Match_3_v3._0.Components;
using Match_3_v3._0.EntityFactories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Entities
{
    class CounterArgs
    {
        public World World { get; set; }
        public string Title { get; set; }
        public Vector2 Position { get; set; }
        public int InitialValue { get; set; }
    }

    class CounterEventArgs : EventArgs
    {
        public int Value { get; }
        public CounterEventArgs(int value)
        {
            Value = value;
        }
    }

    class Counter
    {
        public event EventHandler<CounterEventArgs> CounterChanged;
        private Entity _entity;
        private int _value;

        private int Value
        {
            get => _value;
            set
            {
                _value = value;
                CounterChanged?.Invoke(this, new CounterEventArgs(_value));
                _entity.Set(new Count(_value));
            }
        }

        public Counter(CounterArgs args)
        {
            _entity = new TextFactory(args.World).Create("font", $"{args.Title}: ", args.Position);
            _entity.Set(new Count(args.InitialValue));
            _value = args.InitialValue;
        }
    }
}
