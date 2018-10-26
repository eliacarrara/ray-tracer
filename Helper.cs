using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace RayTracer
{
    public class Helper
    {
        private const float PI2 = (float)(2 * Math.PI);
        private const float GAMMA = 2.2f;
        private const float GAMMA_INV = 1 / GAMMA;
        private static readonly Random rnd = new Random();

        public static byte Clamp8(float value)
        {
            if (value >= 256)
                return 0xFF;
            if (value < 0)
                return 0;

            return (byte)value;
        }

        public static float GammaCorrectionToSRGB(float value)
        {
            return (float)Math.Pow(value, GAMMA_INV);
        }

        public static float GammaCorrectionToRGB(float value)
        {
            return (float)Math.Pow(value, GAMMA);
        }

        public static float DegToRad(float value)
        {
            return ((float)Math.PI / 180.0f) * value;
        }

        public static int CalcStride(int width, int bpp)
        {
            var raw = width * bpp / 8;
            return (raw % 4 == 0) ? raw : raw + (4 - raw % 4);
        }

        public static float RandomGaussValue(float mean, float sigma)
        {
            // https://stackoverflow.com/questions/218060/random-gaussian-variables
            var r1 = 1.0 - rnd.NextDouble();
            var r2 = 1.0 - rnd.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(r1)) * Math.Sin(PI2 * r2);
            return (float)(mean + sigma * randStdNormal);
        }

        public static float VectorSphereHitPoint(Vector3 d, Vector3 OP, Sphere s)
        {
            var sphereIndex = HitPoint.NoHitPoint.SphereIndex;
            var shortestLambda = HitPoint.NoHitPoint.Lambda;

            var ce = OP - s.Center;

            var b = Vector3.Dot(2 * ce, d);
            var c = ce.LengthSquared() - s.Radius * s.Radius;

            var bPow2 = b * b;
            var ac4 = 4 * c;
            var discriminant = bPow2 - ac4;
            if (discriminant < 0)
                return HitPoint.NoHitPoint.Lambda;

            var lambda1 = (float)((-b + Math.Sqrt(discriminant)) / 2);
            var lambda2 = (float)((-b - Math.Sqrt(discriminant)) / 2);

            if (lambda2 > 0 && lambda2 < shortestLambda)
                shortestLambda = lambda2;

            if (lambda1 > 0 && lambda1 < shortestLambda)
                shortestLambda = lambda1;

            return shortestLambda;
        }

        public static Vector3 CalcRndPointOnCircle(Vector3 center, Vector3 normal, float radius)
        {
            var x = (float)(Math.Sqrt(rnd.NextDouble()) * Math.Sin(PI2 * rnd.NextDouble()));
            var y = (float)(Math.Sqrt(rnd.NextDouble()) * Math.Cos(PI2 * rnd.NextDouble()));

            Vector3 normalX = Vector3.Normalize(Vector3.Cross(normal, center));
            while(normalX == Vector3.Zero)
            {
                Vector3 other = new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
                normalX = Vector3.Normalize(Vector3.Cross(normal, other));
            }

            Vector3 normalY = Vector3.Normalize(Vector3.Cross(normal, normalX));
            return center + radius * (x * normalX + y * normalY);
        }

        public static Vector3[] LoadBitmap(Bitmap bitmap)
        {
            var texture = new Vector3[bitmap.Width * bitmap.Height];
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat);
            int length = bitmapData.Stride * bitmapData.Height;
            var data = new byte[bitmapData.Stride * bitmapData.Height];
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, data, 0, length);
            bitmap.UnlockBits(bitmapData);

            int j = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    int pos = y * bitmapData.Stride + (x * 3);
                    float b = GammaCorrectionToRGB(data[pos++] / 255.0f);
                    float g = GammaCorrectionToRGB(data[pos++] / 255.0f);
                    float r = GammaCorrectionToRGB(data[pos++] / 255.0f);

                    texture[j++] = new Vector3(r, g, b);
                }
            }

            return texture;
        }

        public static Vector3 CalcRndHemisphereDirection(Vector3 normal)
        {
            
            float x = (float)(2 * (1 - rnd.NextDouble()) - 1);
            float y = (float)(2 * (1 - rnd.NextDouble()) - 1);
            float z = (float)(2 * (1 - rnd.NextDouble()) - 1);
            Vector3 vec = Vector3.Normalize(new Vector3(x, y, z));

            if (Vector3.Dot(vec, normal) >= 0)
                return vec;
            else
                return -vec;
        }
    }
}