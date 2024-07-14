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
                Card card1 = objectPooler.Pool<Card>(cardPrefabs[randomCardIndex]);
                card1.transform.localScale = Vector3.one * .01f;
                Card card2 = objectPooler.Pool<Card>(cardPrefabs[randomCardIndex]);
                card2.transform.localScale = Vector3.one * .01f;
                cards.Add(card1);
                cards.Add(card2);
            }

            Shuffle(cards);

            PlaceObjects();
            OnCardsGenerated?.Invoke(cards);

        }

        void Shuffle<T>(List<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void DestroyCards()
        {
            cards.ForEach(card => objectPooler.Remove(card));
            cards.Clear();
        }


        [SerializeField] float xSpacing = 1f;
        [SerializeField] float ySpacing = 1f;
        public int noOfRow;

        void PlaceObjects()
        {
            // Calculate the number of rows and columns based on the square root of the total number of objects
            int numPerRow = Mathf.CeilToInt(Mathf.Sqrt(cards.Count));
            int numRows = Mathf.CeilToInt((float)cards.Count / numPerRow);
            noOfRow = numRows;

            int i = 0;
            // Loop through each row and column to place objects
            for (int row = numRows - 1; row >= 0; row--)
            {
                for (int col = numPerRow - 1; col >= 0; col--)
                {
                    // Calculate the position for the current object
                    Vector3 position = new Vector3(col * xSpacing, row * ySpacing, 0);

                    if (i < cards.Count)
                        cards[i].transform.position = position;
                    i++;
                }
            }

            cardsHolder.transform.position = CenterOfCards(cards);
            cards.ForEach(card => card.transform.parent = cardsHolder);
            cardsHolder.transform.position = new Vector3(2.5f, .5f, 0);
        }

        Vector3 CenterOfCards(List<Card> cards)
        {
            var bound = new Bounds(cards[0].transform.position, Vector3.zero);
            for (int i = 1; i < cards.Count; i++)
            {
                bound.Encapsulate(cards[i].transform.position);
            }
            return bound.center;

        }

    }
}