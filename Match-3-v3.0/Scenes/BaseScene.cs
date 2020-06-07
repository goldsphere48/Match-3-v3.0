using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.TextureManager;
using Match_3_v3._0.Utils;
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

        public override void Setup()
        {
            _textureManager = new TextureResourceManager(null, new PipelineTextureLoader(_game.Content));
            _world = new World();
            _textureManager.Manage(_world);
            Setup(_world, out _systems);
        }

        public abstract void Setup(World world, out ISystem<float> systems);

        public override void Update(float elapsedTime)
        {
            _systems.Update(elapsedTime);
        }
    }
}
