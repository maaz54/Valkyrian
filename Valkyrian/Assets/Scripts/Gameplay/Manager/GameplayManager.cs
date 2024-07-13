using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] GridBoard gridBoard;
        private List<Card> cards;

        private int cardsCheckingIndex;
        private Card[] cardsChecking = new Card[2];

        [SerializeField] int turn;
        [SerializeField] int score;

        private void Start()
        {
            gridBoard.OnCardsGenerated += OnCardsGenerated;
            gridBoard.GenerateLevel();
        }

        private void OnCardsGenerated(List<Card> cards)
        {
            this.cards = cards;

            cards.ForEach(card => card.OnCardClick += OnCardClick);
            cardsCheckingIndex = 0;
            score = 0;
            turn = 0;

        }


        private void OnCardClick(Card card)
        {
            cardsChecking[cardsCheckingIndex] = card;
            cardsCheckingIndex++;
            card.RevealCard();

            if (cardsCheckingIndex >= 2)
            {
                cardsCheckingIndex = 0;
                _ = CheckMatchingCards();
            }
        }

        private async Task CheckMatchingCards()
        {
            turn++;
            if (cardsChecking[0].Index == cardsChecking[1].Index)
            {
                score++;
            }
            else
            {
                cardsChecking.ToList().ForEach(card => card.HideCard());
            }
        }


    }
}