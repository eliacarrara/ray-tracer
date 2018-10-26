using System.Diagnostics;
using System.Numerics;

namespace RayTracer
{
    public class Light
    {
        public Light(Vector3 position, Vector3 color, float radius, float intensity, float shadowFactor)
        {
            Position = position;
            Color = color * intensity;
            Radius = radius;
            ShadowFactor = new Vector3(shadowFactor);
        }
        
        public Vector3 Position { get; private set; }
        public Vector3 Color { get; private set; }
        public Vector3 ShadowFactor { get; private set; }
        public float Radius { get; private set; }
    }
}