using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
//Credit to Kryzarel for this Character Stat modifier system.

[System.Serializable]
public class Stat
{
    public double baseValue;
    public virtual double value { 
        get 
        {
            if (isDirty || baseValue != lastBaseValue)
            {
                lastBaseValue = baseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    protected bool isDirty = true;
    protected double _value;
    protected double lastBaseValue = double.MinValue;

    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> publicStatModifiers;

    public Stat()
    {
        baseValue = 0;
        statModifiers = new List<StatModifier>();
        publicStatModifiers = statModifiers.AsReadOnly();
    }

    public Stat(double Value) : this()
    {
        baseValue = Value;
    }

    public virtual void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.order < b.order)
            return -1;
        else if (a.order > b.order)
            return 1;
        return 0;
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if( statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object _source)
    {
        bool didRemove = false;
        for(int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if(statModifiers[i].source == _source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected virtual double CalculateFinalValue()
    {
        double finalValue = baseValue;
        double sumPercentAdd = 0;

        for(int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            if (mod.type == StatModType.FLAT)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModType.ADDITIVEPERCENT)
            {
                sumPercentAdd += mod.value;
                if(i + 1 >= statModifiers.Count || statModifiers[i+1].type != StatModType.ADDITIVEPERCENT)
                {
                    finalValue *= 1 + (sumPercentAdd/100);
                    sumPercentAdd = 0;
                }
            }
            else if (mod.type == StatModType.MULTIPLYPERCENTAGE)
            {
                finalValue *= 1 + (mod.value/100);
            }
        }

        return (double)Math.Round(finalValue, 4);

    }

    public virtual double CalculatePercentShiftFromStatModifiers()
    {
        double finalValue = 1;
        double sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            if (mod.type == StatModType.ADDITIVEPERCENT)
            {
                sumPercentAdd += mod.value;
                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].type != StatModType.ADDITIVEPERCENT)
                {
                    finalValue *= 1 + (sumPercentAdd / 100);
                    sumPercentAdd = 0;
                }
            }
            else if (mod.type == StatModType.MULTIPLYPERCENTAGE)
            {
                finalValue *= 1 + (mod.value / 100);
            }
        }

        return (double)Math.Round(finalValue, 4);
    }

}
