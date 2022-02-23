using System;
// using UnityEngine;

namespace ET
{
  public class CMath
  {
    public const float EPSILON = 0.0001f;
    public const int INT_MAX = 2147483647;
    public const uint UINT_MAX = 4294967295;

    public static bool IsZero(float fValue)
    {
      return (double)fValue <= 9.99999974737875E-05 && (double)fValue >= -9.99999974737875E-05;
    }

    // public static bool IsZero(Vector3 sVector)
    // {
    //   return CMath.IsZero(sVector.x) && CMath.IsZero(sVector.y) && CMath.IsZero(sVector.z);
    // }
    //
    // public static bool Equals(float fLhs, float fRhs)
    // {
    //   return (double)fLhs + 9.99999974737875E-05 >= (double)fRhs && (double)fLhs - 9.99999974737875E-05 <= (double)fRhs;
    // }
    //
    // public static float SqrMagnitude(Vector3 sVector)
    // {
    //   return (float)((double)sVector.x * (double)sVector.x + (double)sVector.y * (double)sVector.y +
    //                  (double)sVector.z * (double)sVector.z);
    // }
    //
    // public static float SqrMagnitudeXz(Vector3 vec1, Vector3 vec2)
    // {
    //   float num1 = vec1.x - vec2.x;
    //   float num2 = vec1.z - vec2.z;
    //   return (float)((double)num1 * (double)num1 + (double)num2 * (double)num2);
    // }
    //
    // public static float SqrMagnitudeXz(float fDeltaX, float fDeltaZ)
    // {
    //   return (float)((double)fDeltaX * (double)fDeltaX + (double)fDeltaZ * (double)fDeltaZ);
    // }
    //
    // public static float MagnitudeXz(Vector3 vec1, Vector3 vec2)
    // {
    //   float num1 = vec1.x - vec2.x;
    //   float num2 = vec1.z - vec2.z;
    //   return Mathf.Sqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2));
    // }
    //
    // public static float MagnitudeXz(float fDeltaX, float fDeltaZ)
    // {
    //   return Mathf.Sqrt((float)((double)fDeltaX * (double)fDeltaX + (double)fDeltaZ * (double)fDeltaZ));
    // }
    //
    // public static float SqrMagnitudeXy(Vector3 vec1, Vector3 vec2)
    // {
    //   float num1 = vec1.x - vec2.x;
    //   float num2 = vec1.y - vec2.y;
    //   return (float)((double)num1 * (double)num1 + (double)num2 * (double)num2);
    // }

    public static float SqrMagnitudeXy(float fDeltaX, float fDeltaY)
    {
      return (float)((double)fDeltaX * (double)fDeltaX + (double)fDeltaY * (double)fDeltaY);
    }

    public static int Sqrt(int value)
    {
      int num1 = 15;
      int num2 = 0;
      int num3 = 32768;
      if (value <= 1)
        return value;
      do
      {
        int num4 = (num2 << 1) + num3 << num1--;
        if (value >= num4)
        {
          num2 += num3;
          value -= num4;
        }
      } while ((num3 >>= 1) != 0);

      return num2;
    }

    public static int Sqrt(long value)
    {
      int num1 = 31;
      long num2 = 0;
      long num3 = 2147483648;
      if (value <= 1L)
        return (int)value;
      do
      {
        long num4 = (num2 << 1) + num3 << num1--;
        if (value >= num4)
        {
          num2 += num3;
          value -= num4;
        }
      } while ((num3 >>= 1) != 0L);

      return (int)num2;
    }

    public static int Abs(int value)
    {
      return value < 0 ? -value : value;
    }

    public static long Abs(long value)
    {
      return value < 0L ? -value : value;
    }

    public static int Min(int a, int b)
    {
      return Math.Min(a, b);
    }

    public static long Min(long a, long b)
    {
      return Math.Min(a, b);
    }

    public static int Max(int a, int b)
    {
      return Math.Max(a, b);
    }

    public static long Max(long a, long b)
    {
      return Math.Max(a, b);
    }

    public static int Climp(int value, int min, int max)
    {
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    public static long Climp(long value, long min, long max)
    {
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    public static int Lerp(int nBegin, int nDest, int t)
    {
      if (t < 0)
        t = 0;
      else if (t > 1000)
        t = 1000;
      return nBegin + (nDest - nBegin) * t / 1000;
    }

    public static int Sin(int value)
    {
      return CTrigonometric.IntSin(value);
    }

    public static int Cos(int value)
    {
      return CTrigonometric.IntCos(value);
    }

    public static int ASin(int value)
    {
      return CTrigonometric.IntASin(value);
    }

    public static int ACos(int value)
    {
      return CTrigonometric.IntACos(value);
    }

    public static bool IsOverstepInt(long value)
    {
      return value > (long)int.MaxValue || value < (long)int.MinValue;
    }

    public static int Sign(int value)
    {
      if (value < 0)
        return -1;
      return value == 0 ? 0 : 1;
    }

    public static int Clamp(int nValue, int nMin, int nMax)
    {
      if (nValue < nMin)
        return nMin;
      return nValue > nMax ? nMax : nValue;
    }

    public static long Clamp(long nValue, long nMin, long nMax)
    {
      if (nValue < nMin)
        return nMin;
      return nValue > nMax ? nMax : nValue;
    }

    public static SVector3 GetBezierPoint(int t, SVector3 start, SVector3 center, SVector3 end)
    {
      return (1000 - t) * (1000 - t) / 1000 * start / 1000 + 2 * t * (1000 - t) / 1000 * center / 1000 +
             t * t / 1000 * end / 1000;
    }

    public static SVector3 SMin(SVector3 a, SVector3 b)
    {
      return new SVector3(CMath.Min(a.x, b.x), CMath.Min(a.y, b.y), CMath.Min(a.z, b.z));
    }

    public static SVector3 SMax(SVector3 a, SVector3 b)
    {
      return new SVector3(CMath.Max(a.x, b.x), CMath.Max(a.y, b.y), CMath.Max(a.z, b.z));
    }

    public static SVector3 SAbs(SVector3 a)
    {
      return new SVector3(CMath.Abs(a.x), CMath.Abs(a.y), CMath.Abs(a.z));
    }

    public static void Swap<T>(ref T a, ref T b)
    {
      T tmp = a;
      a = b;
      b = tmp;
    }
  }
}