using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3d 
{
    public double x;
    public double y;
    public double z;

    public Vector3d()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public Vector3d(double _x, double _y, double _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }

    public static Vector3d operator+(Vector3d v1, Vector3d v2)
    {
        return new Vector3d(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static Vector3d operator-(Vector3d v1, Vector3d v2)
    {
        return new Vector3d(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static Vector3d operator*(Vector3d v1, Vector3 v2)
    {
        return new Vector3d(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static Vector3d operator*(Vector3d v, double scalar)
    {
        return new Vector3d(v.x * scalar, v.y * scalar, v.z * scalar);
    }

    public static Vector3d operator*(double scalar, Vector3d v)
    {
        return new Vector3d(v.x * scalar, v.y * scalar, v.z * scalar);
    }

    public static Vector3d operator/(Vector3d v, double scalar)
    {
        return new Vector3d(v.x / scalar, v.y / scalar, v.z / scalar);
    }

    public static Vector3d operator/(Vector3d v1, Vector3d v2)
    {
        return new Vector3d(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }

    public double Magnitude()
    {
        return System.Math.Sqrt(x * x + y * y + z * z);
    }

    public double SqrMagnitude()
    {
        return (x * x + y * y + z * z);
    }

    public double Dot(Vector3d v)
    {
        return (x * v.x + y * v.y + z * v.z);
    }

    public static double Dot(Vector3d v1, Vector3d v2)
    {
        return (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
    }

    public void Normalize()
    {
        double invLength = 1.0d / System.Math.Sqrt(x * x + y * y + z * z);
        x *= invLength;
        y *= invLength;
        z *= invLength;
    }

    public Vector3d Normalized()
    {
        double invLength = 1.0d / System.Math.Sqrt(x*x + y*y + z*z);
        return new Vector3d(x * invLength, y * invLength, z * invLength);
    }

    public Vector3d Normalized(double l)
    {
        double length = System.Math.Sqrt(x*x + y*y + z*z);
        double invLength = l / length;
        return new Vector3d(x * invLength, y * invLength, z * invLength);
    }

    public Vector3d Normalized(ref double previousLength)
    {
        previousLength = System.Math.Sqrt(x*x + y*y + z*z);
        double invLength = 1.0 / previousLength;
        return new Vector3d(x * invLength, y * invLength, z * invLength);
    }

    public Vector3d Cross(Vector3d v)
    {
        return new Vector3d(y*v.z - z*v.y, z*v.x - x*v.z, x*v.y - y*v.x);
    }

    public static Vector3d Cross(Vector3d v1, Vector3d v2)
    {
        return new Vector3d(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }

    public override string ToString()
    {
        return "(" + x + "," + y + "," + z + ")";
    }

    public static Vector3d Zero() 
    {
        return new Vector3d(0,0,0);
    }
}
