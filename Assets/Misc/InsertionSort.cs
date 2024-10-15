using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InsertionSort
{
    //extends List to perform insertion sort in place
    //T must implement IComparable
    public static void insertionSort<T>(this List<T> list) where T : IComparable
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = i;
            while (j > 0 && list[j-1].CompareTo(list[j]) > 0)
            {
                (list[j], list[j - 1]) = (list[j - 1], list[j]);
                j -= 1;
            }
        }
    }

    public delegate int Compator<in T>(T x, T y);
    
    public static void insertionSort<T>(this List<T> list, Compator<T> comp) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = i;
            while (j > 0 && comp(list[j-1],list[j]) > 0)
            {
                (list[j], list[j - 1]) = (list[j - 1], list[j]);
                j -= 1;
            }
        }
    }
}
