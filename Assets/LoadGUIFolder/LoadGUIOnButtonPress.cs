using UnityEngine.InputSystem;

namespace LoadGUIFolder
{
    public class LoadGUIOnButtonPress : LoadGUIFolder.LoadGUI
    {
        //action to open gui
        public InputActionReference openGUI;

        private void OnEnable()
        {
            openGUI.action.performed += OnButtonPressed;
        }

        private void OnDisable()
        {
            openGUI.action.performed -= OnButtonPressed;
        }

        //if the gui is open, reenable player movement and close it, otherwise do the opposite
        protected virtual void OnButtonPressed(InputAction.CallbackContext callbackContext)
        {
            ONOpenTrigger();
        }
    }
}
