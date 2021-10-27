using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathd
{
	public static readonly double Rad2Deg = 180.0 / Math.PI;
	public static readonly double Deg2Rad = Math.PI / 180.0;

        public static double Clamp01(double value) 
        {
            if (value < 0.0)
                return 0.0d;
            if (value > 1.0)
                return 1d;
            else
                return value;
        }

        public static double Lerp(double from, double to, double t) 
        {
            return from + (to - from) * Mathd.Clamp01(t);
        }

    public static double Min(double a, double b)
    {
        return a < b ? a : b;
    }

    public static double Max(double a, double b)
    {
        return a > b ? a : b;
    }

    public static double Abs(double a)
    {
        return Mathd.Max(-a, a);
    }

    public static int Abs(int value) 
    {
        return Math.Abs(value);
    }    

    public static double Ceil(double d) 
    {
        return Math.Ceiling(d);
    }

    public static double Floor(double d) 
    {
        return Math.Floor(d);
    }    

    public static double Sqrt(double d) 
    {
        return Math.Sqrt(d);
    }

	public static double Safe_Acos(double r)
	{
		return Math.Acos(Math.Min(1.0,Math.Max(-1.0,r)));	
	}    

    public static double Round(double d) 
    {
        return Math.Round(d);
    }    

    public static bool Approximately(double a, double b) 
    {
        return Mathd.Abs(b - a) < Mathd.Max(1E-06d * Mathd.Max(Mathd.Abs(a), Mathd.Abs(b)), 1.121039E-44d);
    }
}
