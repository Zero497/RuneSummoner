using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class VisionTest : MonoBehaviour
{
    public InputActionReference lclick;

    public InputActionReference rclick;
    
    private Vector3Int lastPosition;
    
    public Tilemap map;
    
    public Camera cam;

    public int sightRadius;

    private void Awake()
    {
        lastPosition = map.cellBounds.min;
    }

    private void OnEnable()
    {
        lclick.action.performed += OnClickTile;
        rclick.action.performed += OnClickTileR;
    }

    private void OnDisable()
    {
        lclick.action.performed -= OnClickTile;
        rclick.action.performed -= OnClickTileR;
    }

    private void OnClickTile(InputAction.CallbackContext context)
    {
        Vector2 position = Mouse.current.position.ReadValue();
        Vector3 positionActual = new Vector3(position.x, position.y, 0);
        Vector3 positionWorld = cam.ScreenToWorldPoint(positionActual);
        VisionManager.visionManager.ConcealInRadius("test", sightRadius, lastPosition);
        lastPosition = HexTileUtility.GetNearestTile(positionWorld, map);
        Debug.Log(lastPosition);
        VisionManager.visionManager.RevealInRadius("test",sightRadius, lastPosition);
    }
    
    private void OnClickTileR(InputAction.CallbackContext context)
    {
        Vector2 position = Mouse.current.position.ReadValue();
        Vector3 positionActual = new Vector3(position.x, position.y, 0);
        Vector3 positionWorld = cam.ScreenToWorldPoint(positionActual);
        VisionManager.visionManager.ConcealPosition(HexTileUtility.GetNearestTile(positionWorld, map));
    }
}
