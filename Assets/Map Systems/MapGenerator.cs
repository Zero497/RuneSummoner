using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public static class MapGenerator
{
    //chance for tiles adjacent to cluster center to be added to the cluster
    //chance is halved for each tile further out
    private static float CLUSTERCHANCE = 0.6f;
    
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
        List<float> weights = ParseWeights(GetFrequencyList(fillerTiles, tileGen));
        for(int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                float rand = Random.Range(0, 1.0f);
                int index = weights.Count - 1;
                float cur = 0;
                for (int i = 0; i < weights.Count - 1; i++)
                {
                    cur += weights[i];
                    if (rand < cur)
                    {
                        index = i;
                        break;
                    }
                }
                SetTileAtAndHide(tilemap, new Vector3Int(x,y,0), validTiles[fillerTiles[index]]);
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

    private static List<float> GetFrequencyList(List<int> indices, List<GenDetails> genList)
    {
        List<float> retval = new List<float>();
        foreach (int index in indices)
        {
            retval.Add(genList[index].spawnFrequency);
        }
        return retval;
    }

    private static List<float> ParseWeights(List<float> frequencies)
    {
        float total = frequencies.Sum();
        List<float> weights = new List<float>();
        foreach (float freq in frequencies)
        {
            weights.Add(freq/total);
        }

        return weights;
    }

    private static void GenerateSolitaryTile(Tilemap tilemap, DataTile tile, Vector3Int coord, int sizeX, int sizeY)
    {
        SetTileAtAndHide(tilemap, coord, tile);
        int rand = Random.Range(0, 2);
        while (rand > 0)
        {
            SetTileAtAndHide(tilemap, GetRandomAdjacentTile(coord, sizeX, sizeY), tile);
            rand--;
        }
    }

    private static void GenerateClusterTile(Tilemap tilemap, DataTile tile, Vector3Int coord)
    {
        Queue<Vector3Int> openList = new Queue<Vector3Int>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();
        List<float> chances = new List<float> { CLUSTERCHANCE };
        openList.Enqueue(coord);
        while (openList.Count != 0)
        {
            Vector3Int cur = openList.Dequeue();
            List<Vector3Int> adjs = HexTileUtility.GetAdjacentTiles(coord, tilemap);
            SetTileAtAndHide(tilemap, cur, tile);
            closedList.Add(cur);
            foreach (Vector3Int adj in adjs)
            {
                if(closedList.Contains(adj)) continue;
                float rand = Random.Range(0, 1.0f);
                int dist = HexTileUtility.GetTileDistance(coord, adj);
                while (dist > chances.Count)
                {
                    chances.Add(chances[^1]/2);
                }
                if (chances[dist-1] > rand) openList.Enqueue(adj);
                else closedList.Add(adj);
            }
            
        }
        
    }

    private static void SetTileAtAndHide(Tilemap tilemap, Vector3Int position, DataTile tile)
    {
        tilemap.SetTile(position, tile);
        VisionManager.visionManager.ConcealPosition(position);
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
