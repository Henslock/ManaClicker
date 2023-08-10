public enum StatModType
{
    FLAT,
    ADDITIVEPERCENT,
    MULTIPLYPERCENTAGE,
}

public class StatModifier
{
    public readonly double value;
    public readonly StatModType type;
    public readonly int order;
    public readonly object source;

    public StatModifier(double _value, StatModType _type, int _order, object _source)
    {
        value = _value;
        type = _type;
        order = _order;
        source = _source;
    }

    public StatModifier(double _value, StatModType _type) : this(_value, _type, (int)_type, null) { }
    public StatModifier(double _value, StatModType _type, int _order) : this(_value, _type, _order, null) { }
    public StatModifier(double _value, StatModType _type, object _source) : this(_value, _type, (int)_type, _source) { }
}
