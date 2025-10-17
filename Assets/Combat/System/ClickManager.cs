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

    public void SetAction(UnitAction action)
    {
        currentAction = action;
    }

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
        if (currentAction is Move && clickPosition == TurnController.controller.currentActor.currentPosition)
        {
            MoveController.mControl.InitMovement(TurnController.controller.currentActor, false);
            currentAction = null;
        }
        if (currentAction is { inProgress: false })
        {
            if (!currentAction.RunAction(new SendData(clickPosition)))
            {
                OverlayManager.instance.ClearOverlays();
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
        if (currentAction is { inProgress: false })
        {
            currentAction.PrepAction();
        }
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
        Vector3Int clickPosition = GetClickPosition();
        if (DateTime.Now - sinceLastClick > TimeSpan.FromSeconds(2))
            DescriptionViewLoader.LoadDescriptionView(clickPosition);
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
            if (curActor != null && curActor.isFriendly)
            {
                if (clickPosition == curActor.currentPosition)
                {
                    return curActor.myMovement;
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
                    currentAction = curActor.myMovement;
                    currentAction.RunAction(new SendData(clickPosition));
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
