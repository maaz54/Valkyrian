using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class GridBoard : MonoBehaviour
    {
        [SerializeField] private Card[] cardPrefabs;
        [SerializeField] private int totalTargetPairs;
        [SerializeField] private Transform cardsHolder;
        [SerializeField] private List<Card> cards;

        public Action<List<Card>> OnCardsGenerated;

        void Start()
        {

        }

        [ContextMenu("GenerateLevel")]
        public void GenerateLevel()
        {
            DestroyCards();

            for (int i = 0; i < totalTargetPairs; i++)
            {
                int randomCardIndex = UnityEngine.Random.Range(0, cardPrefabs.Length);
                Card card1 = Instantiate(cardPrefabs[randomCardIndex], cardsHolder);
                Card card2 = Instantiate(cardPrefabs[randomCardIndex], cardsHolder);

                cards.Add(card1);
                cards.Add(card2);
            }

            SuffleCards();

            OnCardsGenerated?.Invoke(cards);
        }

        private void SuffleCards()
        {
            int cardCount = cardsHolder.childCount;
            for (int i = 0; i < cardCount; i++)
            {
                Transform cardTransform = cardsHolder.GetChild(i);
                int randomIndex = UnityEngine.Random.Range(i, cardCount);
                cardTransform.SetSiblingIndex(randomIndex);
            }
        }

        private void DestroyCards()
        {
            cards.ForEach(card => Destroy(card.gameObject));
            cards.Clear();
        }



    }
}