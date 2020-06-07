﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3_v3._0.ResourceManagers
{
    interface IResourceLoader<TInfo, TResource>
    {
        TResource Load(GraphicsDevice device, TInfo info);
    }
}