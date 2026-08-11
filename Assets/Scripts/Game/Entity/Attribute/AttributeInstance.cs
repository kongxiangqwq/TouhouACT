using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AttributeInstance
{
    private readonly Attribute attribute;
    private float minValue;
    private float maxValue;
    private float baseValue;

    private List<AttributeModifier> modifiers;

    private float value;

    public AttributeInstance(Attribute attribute)
    {
        this.attribute = attribute;
        this.minValue = attribute.defaultMinValue;
        this.maxValue = attribute.defaultMaxValue;
        this.baseValue = attribute.defaultBaseValue;

    }

    public AttributeInstance(Attribute attribute,float baseValue)
    {
        this.attribute = attribute;
        this.minValue = attribute.defaultMinValue;
        this.maxValue = attribute.defaultMaxValue;
        this.baseValue = baseValue;

    }

    public AttributeInstance(Attribute attribute, float minValue, float maxValue, float baseValue)
    {
        this.attribute = attribute;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.baseValue = baseValue;
        
    }

    public void ComputeModifiers()
    {

        float addVal = 0.0f;
        float multipleVal = 0.0f;
        float independMultipleVal = 1.0f;
        foreach (var modifier in modifiers)
        {
            switch (modifier.GetOP())
            {
                case ModifierOP.ADD:
                    addVal += modifier.GetAmount();
                    break;
                case ModifierOP.MULTIPLED_BASE:
                    multipleVal += modifier.GetAmount();
                    break;
                case ModifierOP.MULTIPLED_TOTAL:
                    independMultipleVal *= modifier.GetAmount();
                    break;
            }
        }
        value = math.clamp((baseValue + addVal) * multipleVal * independMultipleVal, minValue, maxValue);
    }

    public float GetValue()
    {
        return value;
    }
}
