using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool.Interface;
using UnityEngine;

namespace Gameplay
{
    public class GridBoard : MonoBehaviour
    {
        [SerializeField] private Card[] cardPrefabs;
        [SerializeField] private Transform cardsHolder;
        [SerializeField] private List<Card> cards;

        public int totalTargetPairs;
        public Action<List<Card>> OnCardsGenerated;

        private IObjectPooler objectPooler;

        public void Initialize(IObjectPooler objectPooler)
        {
            this.objectPooler = objectPooler;
        }

        [ContextMenu("GenerateLevel")]
        public void GenerateLevel()
        {
            DestroyCards();

            for (int i = 0; i < totalTargetPairs; i++)
            {
                int randomCardIndex = UnityEngine.Random.Range(0, cardPrefabs.Length);
                Card card1 = objectPooler.Pool<Card>(cardPrefabs[randomCardIndex], cardsHolder);
                card1.transform.localScale = new Vector3(1, 1, 1);
                Card card2 = objectPooler.Pool<Card>(cardPrefabs[randomCardIndex], cardsHolder);
                card2.transform.localScale = new Vector3(1, 1, 1);
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

        public void DestroyCards()
        {
            // cards.ForEach(card => Destroy(card.gameObject));
            cards.ForEach(card => objectPooler.Remove(card));
            cards.Clear();
        }



    }
}