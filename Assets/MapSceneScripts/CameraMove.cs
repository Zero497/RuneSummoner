using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    public float xlim;

    public float ylim;

    public float speed;

    public InputActionReference moveRef;
    
    public void Update()
    {
        Vector2 move = moveRef.action.ReadValue<Vector2>();
        transform.Translate(speed*move.x*Time.deltaTime, speed*move.y*Time.deltaTime,0);
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

        transform.position = new Vector3(x, y, -50);
    }
}
