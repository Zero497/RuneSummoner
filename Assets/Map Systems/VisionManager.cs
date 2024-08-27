using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisionManager : MonoBehaviour
{
    public static VisionManager visionManager;

    public Tilemap tilemap;

    public HiddenTile hiddenTile;

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
            HiddenTile tileAt = tilemap.GetTile<HiddenTile>(position);
            if (tileAt != null)
            {
                tilemap.SetTile(position, tileAt.tileref);
            }
        }
    }

    public void ConcealPosition(Vector3Int position)
    {
        if (tilemap.GetTile(position).GetType() != typeof(HiddenTile))
        {
            revealedPositions.Remove(position);
            DataTile tileAtPosition = tilemap.GetTile<DataTile>(position);
            tilemap.SetTile(position, hiddenTile);
            tilemap.GetTile<HiddenTile>(position).tileref = tileAtPosition;
        }
    }
}
