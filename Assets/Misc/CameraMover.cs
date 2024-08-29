using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMover : MonoBehaviour
{
    public float speed;

    public Tilemap map;

    private Camera self;

    private void Awake()
    {
        self = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        float xMod = 1.66f;
        float yMod = 1f;
        float newX = transform.position.x + Input.GetAxis("Horizontal")*Time.deltaTime * speed;
        if (newX < self.orthographicSize*xMod)
        {
            newX = self.orthographicSize*xMod;
        }
        if (newX > map.localBounds.max.x-self.orthographicSize*xMod-0.25f)
        {
            newX = map.localBounds.max.x-self.orthographicSize*xMod-0.25f;
        }
        float newY = transform.position.y + Input.GetAxis("Vertical")*Time.deltaTime * speed;
        if (newY < self.orthographicSize*yMod-0.5f)
        {
            newY = self.orthographicSize*yMod-0.5f;
        }
        if (newY > map.localBounds.max.y-self.orthographicSize*yMod)
        {
            newY = map.localBounds.max.y-self.orthographicSize*yMod;
        }
        transform.position = new Vector3(newX, newY, -1);
    }
}
