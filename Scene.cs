using RayTracer.AccelerationStructures;
using RayTracer.Surfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Scene
    {
        public const float SHADOW_FACTOR = 0.2f;
        const float K = 40;
        const int SHADOW_FEELER_COUNT = 10;
        const int REFLECTION_LIMIT = 5;
        const float REFLECTION_DECAY = 1.0f / 5.0f;

        readonly Light[] _lights;
        readonly IAccelerationStructure _spheres;
        readonly Camera _camera;
        readonly Vector3 _baseColor;

        public Scene(Camera camera, Vector3 baseColor, IAccelerationStructure spheres, Light[] lights)
        {
            _lights = lights;
            _camera = camera;
            _baseColor = baseColor;
            _spheres = spheres;
        }

        public Vector3 Sample(float u, float v, RenderSettings settings)
        {
            var d = _camera.CreateEyeRay(u, v);
            var hp = _spheres.GetHitPoint(d, _camera.Eye);

            var color = CalcColor(hp, settings);
            if (settings.Reflection)
                color += CalcReflection(hp, _camera.Eye, settings, 0, REFLECTION_DECAY);

            return color;
        }

        private Vector3 CalcColor(HitPoint hp, RenderSettings settings)
        {
            if (!hp.IsHit())
                return _baseColor;

            var sphere = _spheres[hp.SphereIndex];
            var position = hp.Position;
            var n = Vector3.Normalize(position - sphere.Center);

            var colorDiffuse = Vector3.Zero;
            var colorSpecular = Vector3.Zero;

            foreach (var light in _lights)
            {
                var l = Vector3.Normalize(light.Position - position);
                var r = 2 * Vector3.Dot(l, n) * n - l;
                var cosTheta = Vector3.Dot(n, l);

                if (cosTheta >= 0)
                {
                    // Diffuse lighting
                    if (settings.Diffuse)
                    {
                        Vector3 diff = light.Color * cosTheta * sphere.GetColor(n);
                        if (settings.Shadow)
                            diff *= CalcShadowFactor(position + n * 0.01f, light);
                        colorDiffuse += diff;
                    }

                    // Specular lighting
                    if (settings.Specular)
                    {
                        var alpha = Vector3.Dot(r, Vector3.Normalize(position - hp.Origin));
                        if (alpha < 0)
                            colorSpecular += light.Color * (float)Math.Abs(Math.Pow(alpha, K));
                    }
                }
            }

            return sphere.Emission + colorDiffuse + colorSpecular;
        }

        private Vector3 CalcShadowFactor(Vector3 H, Light light)
        {
            var count = 0;
            for (int i = 0; i < SHADOW_FEELER_COUNT; i++)
            {
                var L = Helper.CalcRndPointOnCircle(light.Position, light.Position - H, light.Radius);
                var hl = L - H;
                var hp = _spheres.GetHitPoint(Vector3.Normalize(hl), H);
                if (hp.IsHit() && (hp.Position - hp.Origin).Length() < hl.Length())
                    count++;
            }

            var ratio = ((float)count) / SHADOW_FEELER_COUNT;
            return Vector3.Lerp(Vector3.One, light.ShadowFactor, ratio);
        }

        private Vector3 CalcReflection(HitPoint hp, Vector3 OP, RenderSettings settings, int depth, float reflectionDecay)
        {
            if (!hp.IsHit())
                return _baseColor;

            if (depth >= REFLECTION_LIMIT)
                return Vector3.Zero;

            if (_spheres[hp.SphereIndex].ReflectionFactor <= 0.0f)
                return Vector3.Zero;

            var position = hp.Position;
            var n = Vector3.Normalize(position - _spheres[hp.SphereIndex].Center);
            var r = Vector3.Reflect(hp.Direction, n);
            var H = position + n * 0.01f;

            var hp2 = _spheres.GetHitPoint(r, H);

            if (!hp2.IsHit())
                return _baseColor;

            return _spheres[hp.SphereIndex].ReflectionFactor * reflectionDecay * CalcColor(hp2, settings) +
                CalcReflection(hp2, H, settings, depth + 1, reflectionDecay * reflectionDecay);
        }
    }
}