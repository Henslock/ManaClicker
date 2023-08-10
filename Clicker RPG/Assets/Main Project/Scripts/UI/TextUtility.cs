using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class AbbrevationUtility
{
    public enum FormatType
    {
        TYPE_DEFAULT,
        TYPE_DECIMAL
    }

    private static readonly SortedDictionary<double, string> abbrevations = new SortedDictionary<double, string>
     {
         {Math.Pow(10, 6), " Million" },
         {Math.Pow(10, 9), " Billion" },
         {Math.Pow(10, 12), " Trillion" },
         {Math.Pow(10, 15), " Quadrillion" },
         {Math.Pow(10, 18), " Quintillion" },
         {Math.Pow(10, 21), " Sextillion" },
         {Math.Pow(10, 24), " Septillion" },
         {Math.Pow(10, 27), " Octillion" },
         {Math.Pow(10, 30), " Nonillion" },
         {Math.Pow(10, 33), " Decillion" },
         {Math.Pow(10, 36), " Undecillion" },
         {Math.Pow(10, 39), " Duodecillion" },
         {Math.Pow(10, 42), " Tredecillion" },
         {Math.Pow(10, 45), " Quattuordecillion" },
         {Math.Pow(10, 48), " Quindecillion" },
         {Math.Pow(10, 51), " Sexdecillion" },
         {Math.Pow(10, 54), " Septendecillion" },
         {Math.Pow(10, 57), " Octodecillion" },
         {Math.Pow(10, 60), " Novemdecillion" },
         {Math.Pow(10, 63), " Vigintillion" },
         {Math.Pow(10, 66), " Unvigintillion" },
         {Math.Pow(10, 69), " Duovigintillion" },
         {Math.Pow(10, 72), " Trevigintillion" },
         {Math.Pow(10, 75), " Quattuorvigintillion" },
         {Math.Pow(10, 78), " Quinvigintillion" },
         {Math.Pow(10, 81), " Sexvigintillion" },
         {Math.Pow(10, 84), " Septenvigintillion" },
         {Math.Pow(10, 87), " Octovigintillion" },
         {Math.Pow(10, 90), " Novemvigintillion" },
         {Math.Pow(10, 93), " Trigintillion" },
         {Math.Pow(10, 96), " Untrigintillion" },
         {Math.Pow(10, 99), " Duotrigintillion" },
         {Math.Pow(10, 100), " Googol" },
         {Math.Pow(10, 102), " Tretrigintillion" },
         {Math.Pow(10, 105), " Quattuortrigintillion" },
         {Math.Pow(10, 108), " Quintrigintillion" },
         {Math.Pow(10, 111), " Sextrigintillion" },
         {Math.Pow(10, 114), " Septentrigintillion" },
         {Math.Pow(10, 117), " Octotrigintillion" },
         {Math.Pow(10, 120), " Novemtrigintillion" },
         {Math.Pow(10, 123), " Absolute Shitton" }
     };

    public static string AbbreviateNumber(double number)
    {
        if(number == 0) { return "0"; }

        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<double, string> pair = abbrevations.ElementAt(i);
            if (number >= pair.Key)
            {
                double val = ((float)number / (float)pair.Key);
                return val.ToString("F3") + pair.Value;
            }
        }
        return String.Format("{0:#,#}", number);
    }

    public static string AbbreviateNumber(double number, FormatType type)
    {
        if (number == 0) { return "0"; }

        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<double, string> pair = abbrevations.ElementAt(i);
            if (number >= pair.Key)
            {
                double val = ((float)number / (float)pair.Key);
                return val.ToString("F3") + pair.Value;
            }
        }

        if (type == FormatType.TYPE_DEFAULT)
            return String.Format("{0:#,#}", number);

        if (type == FormatType.TYPE_DECIMAL)
            return String.Format("{0:#,#0.0}", number);

        return String.Format("{0:#,#}", number);
    }
}