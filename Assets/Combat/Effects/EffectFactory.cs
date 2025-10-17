using UnityEngine;

public static class EffectFactory
{
    public static Effect GetEffect(string effectName, UnitBase target, int stacks)
    {
        SendData data = new SendData(effectName);
        data.AddUnit(target);
        data.AddFloat(stacks);
        return GetEffect(data);
    }
    
    public static Effect GetEffect(SendData data)
    {
        if (data.strData[0] == null) return null;
        string effectName = data.strData[0];
        Effect effect;
        effectName = effectName.ToLower();
        switch (effectName)
        {
            case "resistant":
                effect = new Resistant();
                break;
            case "physicalattackup":
            case "physicalattackdown":
            case "physicaldefenseup":
            case "physicaldefensedown":
            case "physicaldefenceup":
            case "physicaldefencedown":
            case "magicalattackup":
            case "magicalattackdown":
            case "magicaldefenseup":
            case "magicaldefensedown":
            case "magicaldefenceup":
            case "magicaldefencedown":
            case "physmagstatchange":
                effect = new PhysMagStatChange();
                data.strData[0] = "physmagstatchange";
                break;
            case "spikes":
                effect = new Spikes();
                break;
            default:
                return null;
        }
        
        effect.Initialize(data);
        return effect;
    }
}
