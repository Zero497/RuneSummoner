using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public int duration;
    
    public static Effect GetEffect(string effectName)
    {
        effectName = effectName.ToLower();
        switch (effectName)
        {
            default:
                return null;
        }
    }
}
