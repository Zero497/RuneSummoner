using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSeedSetter : MonoBehaviour
{
    public int seed;

    public static bool set = false;
    private void Awake()
    {
        if (!set)
        {
            set = true;
            if (seed > 0)
            {
                UnityEngine.Random.InitState(seed);
                Debug.Log("Seed is: "+seed);
            }
            else
            {
                System.Random rand = new System.Random();
                int randomSeed = rand.Next(1, 1000000);
                UnityEngine.Random.InitState(randomSeed);
                Debug.Log("Seed is: "+randomSeed);
            }
            
        }
        
    }
}