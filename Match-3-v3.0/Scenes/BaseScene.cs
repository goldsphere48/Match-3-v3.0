using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.ResourceManagers;
using Match_3_v3._0.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Scenes
{
    abstract class BaseScene : Scene
    {
        private World _world;
        private ISystem<float> _systems;
        private TextureResourceManager _textureManager;
        private SpriteFontResourceManager _fontManagaer;

        public override void Setup()
        {
            _textureManager = new TextureResourceManager(null, new PipelineResourceLoader<Texture2D>(_game.Content));
            _fontManagaer = new SpriteFontResourceManager(null, new PipelineResourceLoader<SpriteFont>(_game.Content));
            _world = new World();
            _textureManager.Manage(_world);
            _fontManagaer.Manage(_world);
            Setup(_world, out _systems);
        }

        public abstract void Setup(World world, out ISystem<float> systems);

        public override void Update(float elapsedTime)
        {
            _systems.Update(elapsedTime);
        }
    }
}
