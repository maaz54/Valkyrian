using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class ClickDetector : MonoBehaviour
    {
        public Action OnClick;

        void OnMouseDown()
        {
            OnClick?.Invoke();
        }
    }
}
