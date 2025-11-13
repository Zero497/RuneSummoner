
using System;
using LoadGUIFolder;
using UnityEngine;

namespace Misc
{
    public class PopUpOnClick : MonoBehaviour
    {
        [NonSerialized] public int index;
        public void OnClick()
        {
            LoadGUIManager.loadGUIManager.ClosePopUp(index);
        }
    }
}
