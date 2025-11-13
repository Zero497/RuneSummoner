using System;
using UnityEngine;

public class InitSaves : MonoBehaviour
{
    private void Awake()
    {
        SaveHandler.Init();
    }
}
