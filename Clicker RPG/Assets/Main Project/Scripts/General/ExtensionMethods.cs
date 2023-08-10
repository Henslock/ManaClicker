using UnityEngine;
using System.Collections;
using System;

public static class ExtensionMethods
{
    public static bool IsApproximately(double arg1, double arg2, double tolerance)
    {
        return Math.Abs(arg1 - arg2) < tolerance;
    }
    public static bool IsApproximately(double arg1, double arg2)
    {
        return Math.Abs(arg1 - arg2) < 0.02d;
    }
}