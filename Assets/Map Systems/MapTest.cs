using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MapTest : MonoBehaviour
{
    public Tilemap map;

    public List<DataTile> tiles;

    public List<MapGenerator.GenerationType> genTypes;

    public List<float> spawnFrequency;

    public int sizeX;

    public int sizeY;

    public InputActionReference click;

    public Camera cam;
    private void Start()
    {
        List<MapGenerator.GenDetails> detailsList = new List<MapGenerator.GenDetails>();
        for (int i = 0; i < genTypes.Count; i++)
        {
            detailsList.Add(new MapGenerator.GenDetails(genTypes[i], spawnFrequency[i]));
        }
        MapGenerator.GenerateMap(map, tiles, detailsList, sizeX, sizeY);
    }

    private void OnEnable()
    {
        click.action.started += OnClickTile;
    }

    private void OnDisable()
    {
        click.action.started -= OnClickTile;
    }

    private void OnClickTile(InputAction.CallbackContext context)
    {
        Vector2 position = context.ReadValue<Vector2>();
        Vector3 positionActual = new Vector3(position.x, position.y, 0);
        Vector3 positionWorld = cam.ScreenToWorldPoint(positionActual);
        Vector3 closestTilePosition = positionWorld - map.GetCellCenterWorld(new Vector3Int(0,0,0));
        Vector3Int closestTile = new Vector3Int(0,0,0);
        for (int x = 0; x < map.size.x; x++)
        {
            for (int y = 0; y < map.size.y; y++)
            {
                Vector3 tempTilePosition = positionWorld - map.GetCellCenterWorld(new Vector3Int(x,y,0));
                if (tempTilePosition.magnitude < closestTilePosition.magnitude)
                {
                    closestTilePosition = tempTilePosition;
                    closestTile = new Vector3Int(x, y, 0);
                }
            }
        }
        VisionManager.visionManager.RevealPosition(closestTile);
    }
}
