using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisionManager : MonoBehaviour
{
    public static VisionManager visionManager;

    public Tilemap tilemap;

    private HashSet<Vector3Int> revealedPositions = new HashSet<Vector3Int>();

    private void Awake()
    {
        if(visionManager != null) Destroy(visionManager.gameObject);
        visionManager = this;
    }

    public void RevealPosition(Vector3Int position)
    {
        if (revealedPositions.Add(position))
        {
            DataTile tileAtPosition = tilemap.GetTile<DataTile>(position);
            DataTile ptr = tileAtPosition.data.hiddenTilePointer;
            if (ptr != null)
            {
                tilemap.SetTile(position, ptr);
            }
        }
    }

    public void ConcealPosition(Vector3Int position)
    {
        if (revealedPositions.Remove(position))
        {
            DataTile tileAtPosition = tilemap.GetTile<DataTile>(position);
            
        }
    }
}
