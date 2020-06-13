﻿using DefaultEcs;
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

    class Counter
    {
        private readonly Entity _entity;
        private int _value;
        private string _title;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _entity.Set(new CountPresenter(_title, _value));
            }
        }

        public Entity GetEntity()
        {
            return _entity;
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
    }
}
