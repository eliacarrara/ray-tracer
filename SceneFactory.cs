using RayTracer.AccelerationStructures;
using RayTracer.Surfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    class SceneFactory
    {
        public static Scene CreateThisThing()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(0, 0, 0), 0.5f, 0, new TextureSurface(Properties.Resources.cmbr)),
                new Sphere(new Vector3(0, 1000.5f, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(0.7f, 0.7f, 0.7f))), // bottom
                new Sphere(new Vector3(0, 0, 1001), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))) // back
            };

            var lights = new List<Light>
            {
                new Light(new Vector3(5, -5, -5), new Vector3(1, 1, 1), 0.5f, 0.8f, Scene.SHADOW_FACTOR)
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(-2f, -2f, -4);
            var lookAt = new Vector3(0, 0, 0);
            var fov = Helper.DegToRad(40);
            var baseColor = Vector3.Zero;
            return new Scene(new Camera(up, eye, lookAt, fov), baseColor, new NoAcceleration(spheres), lights.ToArray());
        }

        public static Scene CreateCornellBoxWithTextures()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(-1001, 0, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 0, 0))), // left
                new Sphere(new Vector3(1001, 0, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(0, 0, 1))), // right
                new Sphere(new Vector3(0, 0, 1001), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // back
                new Sphere(new Vector3(0, -1001, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // top
                new Sphere(new Vector3(0, 1001, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // bottom
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, 0.4f, new TextureSurface(Properties.Resources.moon)),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, 0.1f, new TextureSurface(Properties.Resources.cmbr), 0.1f)
            };

            var lights = new List<Light>
            {
                new Light(new Vector3(0, -0.9f, 0), new Vector3(1, 1, 1), 0.1f, 0.7f, Scene.SHADOW_FACTOR)
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);
            var baseColor = Vector3.Zero;
            return new Scene(new Camera(up, eye, lookAt, fov), baseColor, new NoAcceleration(spheres), lights.ToArray());
        }

        public static Scene CreateCornellBox()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(-1001, 0, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 0, 0))), // left
                new Sphere(new Vector3(1001, 0, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(0, 0, 1))), // right
                new Sphere(new Vector3(0, 0, 1001), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // back
                new Sphere(new Vector3(0, -1001, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // top
                new Sphere(new Vector3(0, 1001, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // bottom
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, 0.4f, new SingleColorSurface(new Vector3(1, 1, 0))),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, 0.1f, new SingleColorSurface(new Vector3(0, 1, 1)))
            };

            var lights = new List<Light>
            {
                new Light(new Vector3(0, -0.8f, 0), new Vector3(1, 1, 1), 0.2f, 0.7f, Scene.SHADOW_FACTOR)
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);
            var baseColor = Vector3.Zero;
            return new Scene(new Camera(up, eye, lookAt, fov), baseColor, new NoAcceleration(spheres), lights.ToArray());
        }

        public static Scene CreateCornellBoxTriLights()
        {
            var spheres = new List<Sphere>
            {
                new Sphere(new Vector3(-1001, 0, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 0, 0)), 0.1f), // left
                new Sphere(new Vector3(1001, 0, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(0, 0, 1))), // right
                new Sphere(new Vector3(0, 0, 1001), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // back
                new Sphere(new Vector3(0, -1001, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // top
                new Sphere(new Vector3(0, 1001, 0), 1000, 0.0f, new SingleColorSurface(new Vector3(1, 1, 1))), // bottom
                new Sphere(new Vector3(-0.6f, 0.7f, -0.6f), 0.3f, 0.4f, new SingleColorSurface(new Vector3(1, 1, 0))),
                new Sphere(new Vector3(0.3f, 0.4f, 0.3f), 0.6f, 0.1f, new SingleColorSurface(new Vector3(0, 1, 1)))
            };

            var lights = new List<Light>
            {
                new Light(new Vector3(-0.5f, -0.8f, 0.3f), new Vector3(1, 1, 0), 0.2f, 0.5f, Scene.SHADOW_FACTOR),
                new Light(new Vector3(0, -0.8f, -0.8f), new Vector3(0, 1, 1), 0.2f, 0.5f, Scene.SHADOW_FACTOR),
                new Light(new Vector3(0.5f, -0.8f, 0.3f), new Vector3(1, 0, 1), 0.2f, 0.5f, Scene.SHADOW_FACTOR)
            };

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);
            var baseColor = Vector3.Zero;

            return new Scene(new Camera(up, eye, lookAt, fov), baseColor, new NoAcceleration(spheres), lights.ToArray());
        }

        public static Scene CreateRandomSphereScene(int count, float size)
        {
            Random r = new Random();
            var spheres = new List<Sphere>();
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3((float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1, (float)r.NextDouble() * 2 - 1);
                Vector3 color = new Vector3((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
                spheres.Add(new Sphere(pos, size, 0, new SingleColorSurface(color)));
            }

            var lights = new List<Light>();
            lights.Add(new Light(new Vector3(0, 0, -2), Vector3.One, 0.2f, 0.5f, 0.2f));

            var up = new Vector3(0, 1, 0);
            var eye = new Vector3(0, 0, -4.5f);
            var lookAt = new Vector3(0, 0, 6);
            var fov = Helper.DegToRad(36);
            var baseColor = Vector3.Zero;
            return new Scene(new Camera(up, eye, lookAt, fov), baseColor, new BVHAccelerationStructure(spheres), lights.ToArray());
        }
    }
}
