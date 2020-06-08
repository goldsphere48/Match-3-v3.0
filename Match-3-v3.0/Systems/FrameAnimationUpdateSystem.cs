using DefaultEcs;
using DefaultEcs.System;
using Match_3_v3._0.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.Systems
{
    class FrameAnimationUpdateSystem : AComponentSystem<float, FrameAnimation>
    {
        public FrameAnimationUpdateSystem(World world)
            : base(world)
        {

        }

        protected override void Update(float state, ref FrameAnimation component)
        {
            if (component.Play)
            {
                component.CurrentState += state;
                if (component.CurrentState >= component.AnimationSpeed)
                {
                    component.CurrentState = 0;
                    if (component.CurrentFrame + 1 == component.FrameCount)
                    {
                        component.CurrentFrame = 0;
                        if (component.IsLooping == false)
                        {
                            component.Play = false;
                        }
                    }
                    else
                    {
                        component.CurrentFrame++;
                    }
                }
            }
        }
    }
}
