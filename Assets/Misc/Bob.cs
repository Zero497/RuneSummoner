using System;
using UnityEngine;

public class Bob : MonoBehaviour
{
    public bool moveX;

    public bool moveY;

    public float accel;

    public float timeToBob;

    private float velX = 0;

    private float velY = 0;

    private float accelX = 0;

    private float accelY = 0;

    private float sinceLastAccelUpdate = 0;

    private void Start()
    {
        if (moveX) accelX = accel;
        if (moveY) accelY = accel;
        sinceLastAccelUpdate = timeToBob / 2;
    }

    private void Update()
    {
        transform.Translate(velX*Time.deltaTime, velY*Time.deltaTime, 0);
        sinceLastAccelUpdate += Time.deltaTime;
        if (sinceLastAccelUpdate >= timeToBob)
        {
            sinceLastAccelUpdate -= timeToBob;
            accelX = -accelX;
            accelY = -accelY;
        }

        velX += accelX * Time.deltaTime;
        velY += accelY * Time.deltaTime;
    }
}
