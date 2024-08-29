using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTestScript : MonoBehaviour
{
    public InputActionReference click;

    public InputActionReference press;

    private void OnEnable()
    {
        click.action.performed += OnClick;
        press.action.performed += OnPress;
    }

    private void OnDisable()
    {
        click.action.performed -= OnClick;
        press.action.performed -= OnPress;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("Clicky");
    }

    private void OnPress(InputAction.CallbackContext context)
    {
        Debug.Log("E");
    }
}
