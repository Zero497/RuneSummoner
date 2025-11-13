using System;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class PopUpTextManager : MonoBehaviour
    {
        public TextMeshProUGUI titleActual;

        public TextMeshProUGUI mainTextActual;

        public void SetText(string title, string mainText)
        {
            titleActual.text = title;
            mainTextActual.text = mainText;
        }
    }
}
