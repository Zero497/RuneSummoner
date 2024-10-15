using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class CameraMover : MonoBehaviour
{
    public float speed;

    public Tilemap map;

    public InputActionReference move;

    private float zPos;

    private Camera self;

    private void Awake()
    {
        self = GetComponent<Camera>();
        zPos = transform.position.z;
    }

    void FixedUpdate()
    {
        Vector2 axis = move.action.ReadValue<Vector2>();
        float xMod = 1.66f;
        float yMod = 1f;
        float newX = transform.position.x +axis.x *Time.deltaTime * speed;
        if (newX < map.LocalToWorld(map.localBounds.min).x+self.orthographicSize*xMod)
        {
            newX = map.LocalToWorld(map.localBounds.min).x+self.orthographicSize*xMod;
        }
        if (newX > map.LocalToWorld(map.localBounds.max).x-self.orthographicSize*xMod-0.25f)
        {
            newX = map.LocalToWorld(map.localBounds.max).x-self.orthographicSize*xMod-0.25f;
        }
        float newY = transform.position.y + axis.y*Time.deltaTime * speed;
        if (newY < map.LocalToWorld(map.localBounds.min).y+self.orthographicSize*yMod+0.5f)
        {
            newY = map.LocalToWorld(map.localBounds.min).y + self.orthographicSize * yMod + 0.5f;
        }
        if (newY > map.LocalToWorld(map.localBounds.max).y-self.orthographicSize*yMod)
        {
            newY = map.LocalToWorld(map.localBounds.max).y-self.orthographicSize*yMod;
        }
        transform.position = new Vector3(newX, newY, zPos);
    }
}
