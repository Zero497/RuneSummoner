using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ClickManager : MonoBehaviour
{
    public static ClickManager clickManager;
    
    public InputActionReference lclick;

    public InputActionReference rclick;

    public Camera mainCam;

    public Tilemap mainMap;

    public Tilemap overlayMap;

    public Tilemap fogMap;

    private UnitAction currentAction;

    private DateTime sinceLastClick;

    private void Awake()
    {
        clickManager = this;
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
        Vector3Int clickPosition = GetClickPosition();
        Debug.Log(clickPosition);
        if (currentAction is Move && clickPosition == TurnController.controller.currentActor.currentPosition)
        {
            MoveController.mControl.InitMovement(TurnController.controller.currentActor, false);
            currentAction = null;
        }
        if (currentAction is { inProgress: false })
        {
            if (!currentAction.RunAction(clickPosition))
            {
                Debug.Log("Action failed!");
            }
            return;
        }

        if (isDoubleClick())
        {
            OnDoubleClickTile(clickPosition);
            return;
        }
        if(currentAction == null)
            currentAction = InferAction(1, clickPosition);
        if(currentAction != null && currentAction.inProgress == false)
            currentAction.PrepAction();
        sinceLastClick = DateTime.Now;
    }

    private void OnDoubleClickTile(Vector3Int clickPosition)
    {
        if (currentAction is { inProgress: true })
        {
            currentAction.RushCompletion();
            return;
        }
        InferAction(3, clickPosition);
    }
    
    private void OnClickTileR(InputAction.CallbackContext context)
    {
        
    }

    private Vector3Int GetClickPosition()
    {
        Vector2 position = Mouse.current.position.ReadValue();
        Vector3 positionActual = new Vector3(position.x, position.y, 0);
        Vector3 positionWorld = mainCam.ScreenToWorldPoint(positionActual);
        return HexTileUtility.GetNearestTile(positionWorld, mainMap);
    }

    private UnitAction InferAction(int clickType, Vector3Int clickPosition)
    {
        if (fogMap.HasTile(clickPosition)) return null;
        UnitBase curActor = TurnController.controller.currentActor;
        if (clickType == 1 )
        {
            if (curActor.isFriendly)
            {
                if (clickPosition == curActor.currentPosition)
                {
                    return new Move();
                }
            }
        }
        if (clickType == 3)
        {
            if (curActor.isFriendly)
            {
                MoveController.mControl.InitMovement(curActor, false);
                if (MoveController.mControl.IsValidMove(clickPosition))
                {
                    currentAction = new Move();
                    currentAction.RunAction(clickPosition);
                }
            }
        }

        return null;
    }

    private bool isDoubleClick()
    {
        if (DateTime.Now - sinceLastClick < TimeSpan.FromSeconds(1))
            return true;
        return false;
    }
}
