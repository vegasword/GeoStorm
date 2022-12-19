using System.Numerics;
using static System.Math;

namespace MyMathLib
{
    // ---------- Arithmetic ---------- //

    public static class Arithmetic
    {
        // Rounds the given value to the nearest int.
        public static int RoundInt(float val) { return (int)Round(val);  }

        // Rounds down the given value.
        public static int FloorInt(float val) { return (int)Floor(val);  }

        // Rounds up the given value.
        public static int CeilInt(float val) { return (int)Ceiling(val); }

        // Returns the sqare power of the given value.
        public static float SqPow(float val) { return val * val;  }

        // Returns 1 if the given value is positive or null, and -1 if it is negative.
        public static int SignOf(float val) { if ((int)val == 0) return 1; return (int)val / Abs((int)val); }

        // Converts the given angle from degrees to radians.
        public static float DegToRad(float val) { return val * ((float)PI / 180f); }

        // Converts the given angle from radians to degrees.
        public static float RadToDeg(float rad) { return rad * (180f / (float)PI); }
        
        // Clamps the given value between the maximum and minimum values.
        public static float Clamp(float val, float min, float max) { if (val > max) val = max; if (val < min) val = min; return val; }

        // Clamps the given value to be inferior or equal to the maximum value.
        public static float ClampUnder(float val, float max) { if (val > max) val = max; return val; }

        // Clamps the given value to be superior or equal to the minimum value.
        public static float ClampAbove(float val, float min) { if (val < min) val = min; return val; }

        // Compute linear interpolation between start and end for the parameter val (if 0 <= val <= 1: start <= return <= end).
        public static float Lerp(float val, float start, float end) { return start + val* (end - start); }

        // Compute the linear interpolation factor that returns val when lerping between start and end.
        public static float GetLerp(float val, float start, float end)
        {
            if (end - start != 0) return (val - start) / (end - start);
            return 0;
        }

        // Remaps the given value from one range to another.
        public static float Remap(float val, float inputStart, float inputEnd, float outputStart, float outputEnd)
        {
            return outputStart + (val - inputStart) * (outputEnd - outputStart) / (inputEnd - inputStart);
        }

        // Returns true if the given number is a power of 2.
        public static bool IsPowerOf2(int val) { return val == (int)Pow(2, (int)(Log(val) / Log(2))); }

        // Returns the closest power of 2 that is inferior or equal to val.
        public static int GetPowerOf2Under(int val) {  return (int)Pow(2, (int)(Log(val) / Log(2))); }

        // Returns the closest power of 2 that is superior or equal to val.
        public static int GetPowerOf2Above(int val)
        {
            if (IsPowerOf2(val)) return (int)Pow(2, (int)(Log(val) / Log(2)));
            else                 return (int)Pow(2, (int)(Log(val) / Log(2)) + 1);
        }
    }
}
