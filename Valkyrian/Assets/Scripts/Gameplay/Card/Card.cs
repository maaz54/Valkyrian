using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private ClickDetector clickDetector;
        [SerializeField] private int index;
        public int Index => index;

        public Action<Card> OnCardClick;

        bool isRevealed = false;

        private void OnEnable()
        {
            clickDetector.OnClick += OnClick;
        }

        private void OnDisable()
        {
            clickDetector.OnClick -= OnClick;
            OnCardClick = null;
        }

        private void OnClick()
        {
            if (!isRevealed)
                OnCardClick?.Invoke(this);
        }

        public void RevealCard()
        {
            transform.localEulerAngles = new(0, 180, 0);
            isRevealed = true;
        }

        public async Task HideCard()
        {
            await Task.Delay(TimeSpan.FromSeconds(.5f));
            isRevealed = false;
            transform.localEulerAngles = Vector3.zero;
        }

    }
}
