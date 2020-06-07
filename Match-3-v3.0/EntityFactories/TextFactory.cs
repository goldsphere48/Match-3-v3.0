using DefaultEcs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.EntityFactories
{
    class TextFactory
    {
        private World _world;

        public TextFactory(World world)
        {
            _world = world;
        }

        public void Create(string fontName, string text, Vector2 position)
        {

        }

        public void Create(string fontName, string text)
        {

        }
    }
}
