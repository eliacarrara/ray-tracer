using System;
using System.Numerics;

namespace RayTracer
{
    public class Camera
    {
        readonly Vector3 _speedupX;
        readonly Vector3 _speedupY;

        public Camera(Vector3 up, Vector3 eye, Vector3 lookAt, float fov)
        {
            Up = Vector3.Normalize(up);
            F = Vector3.Normalize(lookAt - eye);
            Eye = eye;
            FOV = fov;
            
            R = Vector3.Normalize(Vector3.Cross(F, Up));
            U = Vector3.Normalize(Vector3.Cross(F, R));

            _speedupX = (float)Math.Tan(FOV / 2) * R;
            _speedupY = (float)Math.Tan(FOV / 2) * U;
        }
        
        public float FOV { get; private set; }
        public Vector3 Up { get; private set; }
        public Vector3 U { get; private set; }
        public Vector3 R { get; private set; }
        public Vector3 Eye { get; private set; }
        public Vector3 F { get; private set; }

        public Vector3 CreateEyeRay(float x, float y)
        {
            return Vector3.Normalize(F + (x * _speedupX) + (y * _speedupY));
        }
    }
}