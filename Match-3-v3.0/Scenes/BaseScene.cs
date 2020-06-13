using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.ResourceManagers;
using Microsoft.Xna.Framework.Graphics;
using SceneSystem;

namespace Match_3_v3._0.Scenes
{
    internal abstract class BaseScene : Scene
    {
        private SpriteFontResourceManager _fontManager;
        private ISystem<float> _systems;
        private TextureResourceManager _textureManager;
        private World _world;

        public override void Setup()
        {
            _textureManager = new TextureResourceManager(null, new PipelineResourceLoader<Texture2D>(_game.Content));
            _fontManager = new SpriteFontResourceManager(null, new PipelineResourceLoader<SpriteFont>(_game.Content));
            _world = new World();
            _textureManager.Manage(_world);
            _fontManager.Manage(_world);
            Setup(_world, out _systems);
        }

        public abstract void Setup(World world, out ISystem<float> systems);

        public override void Update(float elapsedTime)
        {
            _systems.Update(elapsedTime);
        }
    }
}