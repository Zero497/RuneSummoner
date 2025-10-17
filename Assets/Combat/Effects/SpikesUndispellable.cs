using UnityEngine;

public class SpikesUndispellable : Spikes
{
    /*
        Expects:
            String 0: effect name
            String 1: whether this effect is undispellable
            Float 1: damage type (enum as int)
            Float 2: damage element (enum as int)
     */
    public override bool Equals(SendData data)
    {
        if (base.Equals(data))
        {
            if (data.strData.Count < 1 && data.strData[1].ToLower().Equals("undispellable"))
                return true;
        }
        return false;
    }

    public override bool Equals(Effect effect)
    {
        if (effect is SpikesUndispellable)
        {
            return base.Equals(effect);
        }
        return false;
    }
}
