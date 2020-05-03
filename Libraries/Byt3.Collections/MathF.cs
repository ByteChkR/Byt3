using System;

namespace Byt3.Collections
{
    public static class MathF
    {
        public const float PI = (float) Math.PI;
        public const float E = (float) Math.E;

        public static float Clamp(float a, float min, float max)
        {
            return a < min ? min : a > max ? max : a;
        }

        public static float Clamp01(float a)
        {
            return Clamp(a, 0, 1);
        }

        public static float Acos(float d)
        {
            return (float) Math.Acos((double) d);
        }

        public static float Asin(float d)
        {
            return (float) Math.Asin((double) d);
        }

        public static float Atan(float d)
        {
            return (float) Math.Atan((double) d);
        }

        public static float Atan2(float y, float x)
        {
            return (float) Math.Atan2((double) y, (double) x);
        }

        public static float Ceiling(float a)
        {
            return (float) Math.Ceiling((double) a);
        }

        public static float Cos(float d)
        {
            return (float) Math.Cos((double) d);
        }

        public static float Cosh(float value)
        {
            return (float) Math.Cosh((double) value);
        }

        public static float Floor(float d)
        {
            return (float) Math.Floor((double) d);
        }

        public static float Sin(float a)
        {
            return (float) Math.Sin((double) a);
        }

        public static float Tan(float a)
        {
            return (float) Math.Tan((double) a);
        }

        public static float Sinh(float value)
        {
            return (float) Math.Sinh((double) value);
        }

        public static float Tanh(float value)
        {
            return (float) Math.Tanh((double) value);
        }

        public static float Round(float a)
        {
            return (float) Math.Round((double) a);
        }

        public static float Round(float a, int decimals)
        {
            return (float) Math.Round(a, decimals);
        }

        public static float Truncate(float d)
        {
            return (float) Math.Truncate((double) d);
        }

        public static float Sqrt(float d)
        {
            return (float) Math.Sqrt((double) d);
        }

        public static float Log(float d)
        {
            return (float) Math.Log((double) d);
        }

        public static float Log10(float d)
        {
            return (float) Math.Log10((double) d);
        }

        public static float Exp(float d)
        {
            return (float) Math.Exp((double) d);
        }

        public static float Pow(float x, float y)
        {
            return (float) Math.Pow((double) x, (double) y);
        }

        public static float IEEERemainder(float x, float y)
        {
            return (float) Math.IEEERemainder((double) x, (double) y);
        }

        public static float Abs(float value)
        {
            return Math.Abs(value);
        }

        public static float Max(float val1, float val2)
        {
            return Math.Max(val1, val2);
        }

        public static float Min(float val1, float val2)
        {
            return Math.Min(val1, val2);
        }

        public static float Log(float a, float newBase)
        {
            return (float) Math.Log((double) a, (double) newBase);
        }

        public static int IntPow(int basis, int exp)
        {
            if (exp == 0)
            {
                return 1;
            }

            int ret = basis;
            for (int i = 1; i < exp; i++)
            {
                ret *= basis;
            }

            return ret;
        }
        public static float IntPow(float basis, int exp)
        {
            if (exp == 0)
            {
                return 1;
            }

            float ret = basis;
            for (int i = 1; i < exp; i++)
            {
                ret *= basis;
            }

            return ret;
        }
    }
}