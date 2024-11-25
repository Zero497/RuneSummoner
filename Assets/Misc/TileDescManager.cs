using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileDescManager : MonoBehaviour
{
    public TextMeshProUGUI TileName;
    
    public Image tileImage;

    public TextMeshProUGUI moveCost;

    public TextMeshProUGUI impassability;

    public TextMeshProUGUI losblocking;
    
    public TextMeshProUGUI Description;
    
    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        DataTile target = TurnController.controller.mainMap.GetTile<DataTile>(DescriptionViewLoader.targetTile);
        TileName.text = target.data.name;
        tileImage.sprite = target.sprite;
        moveCost.text = "Move Cost: "+target.data.moveCost;
        impassability.text = "Is Impassable?: "+target.data.isImpassable;
        losblocking.text = "Blocks Line of Sight?: " + target.data.lineOfSightBlocking;
        Description.text = target.data.description;
    }
}
