using DefaultEcs;
using DefaultEcs.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Scenes
{
    class GameOverScene : BaseScene
    {
        public override void Setup(World world, out ISystem<float> systems)
        {
            systems = new SequentialSystem<float>();
        }
    }
}
