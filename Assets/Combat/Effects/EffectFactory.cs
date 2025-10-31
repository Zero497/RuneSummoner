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
                effect.isBuff = true;
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
                effect.isBuff = true;
                break;
            /*
        Expects:
            Unit 0: unit to apply to
            Unit 1: unit applying effect
            String 0: effect name
            Float 0: stacks to apply
     */
            case "marked":
                effect = new Marked();
                effect.isBuff = false;
                break;
            /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
    */
            case "evade":
                effect = new Evade();
                effect.isBuff = true;
                break;
            /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
    */
            case "vulnerable":
                effect = new Vulnerable();
                effect.isBuff = false;
                break;
            /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
    */
            case "shocked":
                effect = new Shocked();
                effect.isBuff = false;
                break;
            /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
    */
            case "invulnerable":
                effect = new Invulnerable();
                effect.isBuff = true;
                break;
            /*
       Expects:
           Unit 0: unit to apply to
           String 0: effect name
           Float 0: stacks to apply
           Float 1: 1 for mod up, -1 for mod down
    */
            case "apup":
            case "apdown":
            case "abilitypowerup":
            case "abilitypowerdown":
                effect = new APUp();
                break;
            default:
                return null;
        }
        
        effect.Initialize(data);
        return effect;
    }
}
