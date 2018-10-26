using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Surfaces
{
    class SingleColorSurface : ISurface
    {
        readonly Vector3 _color;

        public SingleColorSurface(Vector3 color)
        {
            this._color = color;
        }

        public Vector3 GetTextureColor(Vector3 n)
        {
            return _color;
        }
    }
}
