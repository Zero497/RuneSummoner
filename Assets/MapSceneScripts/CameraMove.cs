using System;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float xlim;

    public float ylim;

    public float speed;
    
    public void Update()
    {
        transform.Translate(speed*Input.GetAxis("Horizontal")*Time.deltaTime, speed*Input.GetAxis("Vertical")*Time.deltaTime, -10);
        Vector3 pos = transform.position;
        float x = pos.x;
        float y = pos.y;
        if (x > xlim)
        {
            x = xlim;
        } else if (x < -xlim)
        {
            x = -xlim;
        }

        if (y > ylim)
        {
            y = ylim;
        }else if (y < -ylim)
        {
            y = -ylim;
        }

        transform.position = new Vector3(x, y, -10);
    }
}
