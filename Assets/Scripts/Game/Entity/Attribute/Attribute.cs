using UnityEngine;

public class Attribute
{
    public readonly string name;
    public readonly float defaultMinValue;
    public readonly float defaultMaxValue;
    public readonly float defaultBaseValue;
    public Attribute(string name, float defaultMinValue, float defaultMaxValue,float defaultBaseValue)
    {
        this.name = name;
        this.defaultMinValue = defaultMinValue;
        this.defaultMaxValue = defaultMaxValue;
        this.defaultBaseValue = defaultBaseValue;
    }

    
}
