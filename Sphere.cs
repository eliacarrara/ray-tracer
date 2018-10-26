using System.Numerics;
using RayTracer.Surfaces;

namespace RayTracer
{
    public class Sphere
    {
        public Sphere(Vector3 center, float radius, float reflectionFactor, ISurface surface, float emission = 0)
        {
            Center = center;
            Radius = radius;
            ReflectionFactor = reflectionFactor;
            Surface = surface;
            Emission = new Vector3(emission);
        }
        
        public Vector3 Center { get; private set; }
        public float Radius { get; private set; }
        public float ReflectionFactor { get; private set; }
        public Vector3 Emission { get; set; }
        public Vector3 GetColor(Vector3 n) { return Surface.GetTextureColor(n); }
        private ISurface Surface { get; set; }
    }
}