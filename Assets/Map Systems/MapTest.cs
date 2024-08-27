using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTest : MonoBehaviour
{
    public Tilemap map;

    public List<DataTile> tiles;

    public List<MapGenerator.GenerationType> genTypes;

    public List<float> spawnFrequency;

    public int sizeX;

    public int sizeY;
    private void Awake()
    {
        List<MapGenerator.GenDetails> detailsList = new List<MapGenerator.GenDetails>();
        for (int i = 0; i < genTypes.Count; i++)
        {
            detailsList.Add(new MapGenerator.GenDetails(genTypes[i], spawnFrequency[i]));
        }
        MapGenerator.GenerateMap(map, tiles, detailsList, sizeX, sizeY);
    }
}
