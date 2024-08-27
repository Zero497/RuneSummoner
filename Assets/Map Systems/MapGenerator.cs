using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public static class MapGenerator
{
    public enum GenerationType
    {
        //filler will be the default tile in the generated map
        filler,
        //cluster tiles will generate in groups of 6 to 16
        cluster,
        //solitary tiles will generate in groups of 1 to 3
        solitary
    }
    
    public class GenDetails
    {
        //how this tile should be generated in this map
        public GenerationType genType;
        /*how common this tile should be in this map.
        only applies for non-filler tiles
        frequency of cluster and solitary should be low. 
        for these two, frequency indicates the likelihood a
        cluster/solitary tile set spawns at each coordinate*/
        public float spawnFrequency;

        public GenDetails(GenerationType genType, float spawnFrequency)
        {
            this.genType = genType;
            this.spawnFrequency = spawnFrequency;
        }
    }
    
    
    public static void GenerateMap(Tilemap tilemap, List<DataTile> validTiles, List<GenDetails> tileGen, int sizeX, int sizeY)
    {
        List<int> fillerTiles;
        List<int> clusterTiles;
        List<int> solitaryTiles;
        ParseGenTypes(validTiles, tileGen, out fillerTiles, out clusterTiles, out solitaryTiles);
        for(int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                int rand = Random.Range(0, fillerTiles.Count);
                tilemap.SetTile(new Vector3Int(x,y,0), validTiles[fillerTiles[rand]]);
            }
        }
        foreach (int i in clusterTiles)
        {
            for(int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    float rand = Random.value;
                    if (rand <= tileGen[i].spawnFrequency)
                    {
                        GenerateClusterTile(tilemap, validTiles[i], new Vector3Int(x,y,0));
                    }
                }
            }
        }
        foreach (int i in solitaryTiles)
        {
            for(int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    float rand = Random.value;
                    if (rand <= tileGen[i].spawnFrequency)
                    {
                        GenerateSolitaryTile(tilemap, validTiles[i], new Vector3Int(x,y,0), sizeX, sizeY);
                    }
                }
            }
        }
    }

    private static void GenerateSolitaryTile(Tilemap tilemap, DataTile tile, Vector3Int coord, int sizeX, int sizeY)
    {
        tilemap.SetTile(coord, tile);
        int rand = Random.Range(0, 2);
        while (rand > 0)
        {
            tilemap.SetTile(GetRandomAdjacentTile(coord, sizeX, sizeY), tile);
            rand--;
        }
    }

    private static void GenerateClusterTile(Tilemap tilemap, DataTile tile, Vector3Int coord)
    {
        //TODO
    }

    //collate the passed tiles into lists of indices for tiles of matching type
    private static void ParseGenTypes(List<DataTile> validTiles, List<GenDetails> details, out List<int> fillerTileList, out List<int> clusterTileList,
        out List<int> solitaryTileList)
    {
        List<List<int>> tileTypeLists = new List<List<int>> {new List<int>(), new List<int>(), new List<int>()};
        for(int i = 0; i<validTiles.Count; i++)
        {
            tileTypeLists[(int) details[i].genType].Add(i);
        }
        fillerTileList = tileTypeLists[0];
        clusterTileList = tileTypeLists[1];
        solitaryTileList = tileTypeLists[2];
    }

    //get a random coordinate adjacent to the target coordinate
    private static Vector3Int GetRandomAdjacentTile(Vector3Int target, int xbound, int ybound)
    {
        int randXChange;
        do
        {
            randXChange = target.x + Random.Range(-1, 1);
        } while (randXChange < 0 || randXChange >= xbound);
        int randYChange;
        do
        {
            randYChange = target.y + Random.Range(-1, 1);
        } while (randYChange < 0 || randYChange >= ybound);
        return new Vector3Int(randXChange, randYChange, 0);
    }
}
