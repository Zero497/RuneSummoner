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

    public InputActionReference scroll;

    public float yMod;

    public float xMod;

    public float minCamSize;

    public float maxCamSize;

    public float scrollSpeed;

    private float zPos;

    private Camera self;

    private void Awake()
    {
        self = GetComponent<Camera>();
        zPos = transform.position.z;
    }

    void FixedUpdate()
    {
        self.orthographicSize -= scrollSpeed * scroll.action.ReadValue<Vector2>().y * Time.fixedDeltaTime;
        if (self.orthographicSize < minCamSize)
            self.orthographicSize = minCamSize;
        else if (self.orthographicSize > maxCamSize)
            self.orthographicSize = maxCamSize;
        
        Vector2 axis = move.action.ReadValue<Vector2>();
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
