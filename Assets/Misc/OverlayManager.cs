using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager instance;
    
    public Tilemap overlayMap;

    [Tooltip("This should map by index to overlay tiles")]
    public List<string> overlayOptions;
    
    public List<Tile> overlayTiles;
    
    private Dictionary<string, Tile> overlayTilesDictionary = new Dictionary<string, Tile>();
    
    private List<Vector3Int> overlayTilesPositions = new List<Vector3Int>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < overlayOptions.Count; i++)
        {
            overlayTilesDictionary.Add(overlayOptions[i], overlayTiles[i]);
        }
    }

    public void CreateOverlay(List<HexTileUtility.DjikstrasNode> positions, string overlayName)
    {
        Tile tile  = overlayTilesDictionary[overlayName];
        if (tile == null) return;
        foreach (HexTileUtility.DjikstrasNode pos in positions)
        {
            overlayMap.SetTile(pos.location, tile);
            overlayTilesPositions.Add(pos.location);
        }
    }

    public void CreateOverlay(List<Vector3Int> positions, string overlayName)
    {
        Tile tile  = overlayTilesDictionary[overlayName];
        if (tile == null) return;
        foreach (Vector3Int pos in positions)
        {
            overlayMap.SetTile(pos, tile);
            overlayTilesPositions.Add(pos);
        }
    }
    
    public void CreateOverlay(List<Vector3Int> positions, Tile tile)
    {
        foreach (Vector3Int pos in positions)
        {
            overlayMap.SetTile(pos, tile);
            overlayTilesPositions.Add(pos);
        }
    }
    
    public void CreateOverlay(List<HexTileUtility.DjikstrasNode> positions, Tile tile)
    {
        foreach (HexTileUtility.DjikstrasNode pos in positions)
        {
            overlayMap.SetTile(pos.location, tile);
            overlayTilesPositions.Add(pos.location);
        }
    }
    
    public void ClearOverlays()
    {
        foreach (Vector3Int pos in overlayTilesPositions) 
        {
            overlayMap.SetTile(pos, null);
        }
        overlayTilesPositions.Clear();
    }
}
