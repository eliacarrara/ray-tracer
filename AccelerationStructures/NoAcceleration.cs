using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.AccelerationStructures
{
    class NoAcceleration : IAccelerationStructure
    {
        readonly List<Sphere> _spheres;

        public NoAcceleration(List<Sphere> spheres)
        {
            _spheres = new List<Sphere>(spheres);
        }

        public HitPoint GetHitPoint(Vector3 d, Vector3 OP)
        {
            var sphereIndex = HitPoint.NoHitPoint.SphereIndex;
            var shortestLambda = HitPoint.NoHitPoint.Lambda;

            for (int i = 0; i < _spheres.Count; i++)
            {
                var lambda = Helper.VectorSphereHitPoint(d, OP, _spheres[i]);
                if (lambda < shortestLambda)
                {
                    shortestLambda = lambda;
                    sphereIndex = i;
                }
            }

            if (HitPoint.Found(sphereIndex, shortestLambda))
                return new HitPoint(OP, shortestLambda, d, sphereIndex);
            else
                return HitPoint.NoHitPoint;
        }

        public Sphere this[int index]
        {
            get { return _spheres[index]; }
        }
    }
}
