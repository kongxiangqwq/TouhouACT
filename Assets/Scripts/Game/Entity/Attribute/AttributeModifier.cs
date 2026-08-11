using UnityEngine;

public enum ModifierOP
{
    ADD, MULTIPLED_BASE,MULTIPLED_TOTAL
}

public class AttributeModifier
{
    private readonly string id;
    private readonly ModifierOP op;
    private readonly float amount;

    public AttributeModifier(string id, ModifierOP op, float amount)
    {
        this.id = id;
        this.op = op;
        this.amount = amount;
    }

    public string GetId() { return id; }

    public ModifierOP GetOP() {  return op; }

    public float GetAmount() { return amount; }
}
