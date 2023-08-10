using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public static class LevelingTable
{
    private static readonly Dictionary<int, int> LevelListExp = new Dictionary<int, int>
    {
        { 1, 25},
        { 2, 50 },
        { 3, 75 },
        { 4, 100 },
        { 5, 150 },
        { 6, 200 },
        { 7, 250 },
        { 8, 300 },
        { 9, 350 },
        { 10, 400 },
        { 11, 400 },
        { 12, 400 },
        { 13, 400 }
    };

    public static double GetRequiredExpForLevel(int level)
    {
        if(!LevelListExp.ContainsKey(level)) { return 10; }
        
        return LevelListExp[level];
    }
}
