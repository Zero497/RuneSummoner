using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListDifference 
{
    public static List<T> diff<T>(this List<T> initial, List<T> diffL) where T : IEquatable<T>
    {
        List<T> result = new List<T>();
        foreach (T item in initial)
        {
            if(!diffL.Contains(item))
                result.Add(item);
        }
        return result;
    }
}
