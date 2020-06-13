using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Components
{
    class FrameAnimation : RendererComponent
    {
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public float AnimationSpeed { get; set; }
        public bool IsLooping { get; set; }
        public bool Play 
        { 
            get => _play; 
            set
            {
                _play = value;
                if (!_play)
                {
                    CurrentFrame = 0;
                    CurrentState = 0;
                }
            } 
        }
        public int FrameHeight => Texture.Height;
        private Texture2D _textute;
        private bool _play = false;
        public Texture2D Texture 
        { 
            get => _textute;
            set 
            {
                _textute = value;
                Destination = new Rectangle(0, 0, (int)FrameWidth, (int)FrameHeight);
            }
        }
        public int FrameWidth => Texture.Width / FrameCount;
        public float CurrentState { get; set; }
    }
}
