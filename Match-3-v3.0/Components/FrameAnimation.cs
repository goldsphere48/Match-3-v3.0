using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match_3_v3._0.Components
{
    internal class FrameAnimation : RendererComponent
    {
        private bool _play = false;
        private Texture2D _textute;
        public float AnimationSpeed { get; set; }
        public int CurrentFrame { get; set; }
        public float CurrentState { get; set; }
        public int FrameCount { get; set; }
        public int FrameHeight => Texture.Height;
        public int FrameWidth => Texture.Width / FrameCount;
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

        public Texture2D Texture
        {
            get => _textute;
            set
            {
                _textute = value;
                Destination = new Rectangle(0, 0, (int)FrameWidth, (int)FrameHeight);
            }
        }
    }
}