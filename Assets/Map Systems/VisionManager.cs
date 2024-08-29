using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisionManager : MonoBehaviour
{
    public static VisionManager visionManager;

    public Tilemap hiddenMap;

    public Tile hiddenTile;

    private Dictionary<Vector3Int, List<String>> revealedPositions = new Dictionary<Vector3Int, List<String>>();

    private void Awake()
    {
        if(visionManager != null) Destroy(visionManager.gameObject);
        visionManager = this;
    }

    //reveal the specified position
    public void RevealPosition(Vector3Int position)
    {
        hiddenMap.SetTile(position, null);
    }

    //conceal the specified position
    public void ConcealPosition(Vector3Int position)
    {
        revealedPositions.Remove(position);
        hiddenMap.SetTile(position, hiddenTile);
    }

    /*
     * pass a vector with negative values to oldposition for no original position
     */
    public void ChangeViewerPosition(String viewerID, int sightRadius, Vector3Int oldPosition, Vector3Int newPosition)
    {
        if (oldPosition is {x : > 0, y: > 0, z: > 0 })
        {
            for (int x = -sightRadius; x < sightRadius; x++)
            {
                for (int y = -sightRadius; y < sightRadius; y++)
                {
                    int val = Mathf.Abs(x)+Mathf.Abs(y)/2;
                    if (y % 2 != 0)
                    {
                        val++;
                    }

                    if (val < sightRadius)
                    {
                        
                    }
                }
            }
        }
    }
}
