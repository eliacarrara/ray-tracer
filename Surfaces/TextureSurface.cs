using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Surfaces
{
    class TextureSurface : ISurface
    {
        readonly Vector3[] _texture = null;
        readonly int _width = 0;
        readonly int _height = 0;
        public TextureSurface(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                _width = bitmap.Width;
                _height = bitmap.Height;
                _texture = Helper.LoadBitmap(bitmap);
            }
        }

        public Vector3 GetTextureColor(Vector3 n)
        {
            if (_texture == null)
                return Vector3.Zero;

            float s = (float)((Math.Atan2(n.X, n.Z) + Math.PI) / (2 * Math.PI));
            float t = (float)(Math.Acos(n.Y) / Math.PI);

            Debug.Assert(s >= 0.0f && s <= 1.0f);
            Debug.Assert(t >= 0.0f && t <= 1.0f);

            int x = (int)((_width - 1) * s);
            int y = (int)((_height - 1) * t);

            return _texture[y * _width + x];
        }
    }
}
