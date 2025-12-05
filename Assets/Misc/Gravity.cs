using System;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float accel;

    public float drag;

    public Vector2 initVel;

    private Vector2 vel;

    private void Start()
    {
        vel = initVel;
    }

    private void Update()
    {
        var position = transform.position;
        position = new Vector3(position.x + vel.x*Time.deltaTime, position.y + vel.y*Time.deltaTime, position.z);
        transform.position = position;
        if (vel.x > 0)
            vel.x -= drag*Time.deltaTime;
        else
        {
            vel.x += drag*Time.deltaTime;
        }

        if (vel.x < 0.01f && vel.x > -0.01f)
            vel.x = 0;
        vel.y -= accel*Time.deltaTime;
        if(position.y < -2000 || position.y > 2000)
            Destroy(gameObject);
    }
}
