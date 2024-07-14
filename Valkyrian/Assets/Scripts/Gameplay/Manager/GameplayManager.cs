using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gameplay.UI;
using ObjectPool;
using SoundsPlayer;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] GridBoard gridBoard;
        [SerializeField] MenuUi menuUi;
        [SerializeField] SFXPlayer sFXPlayer;
        [SerializeField] ObjectPooler objectPooler;
        [SerializeField] CameraSize cameraSize;

        private List<Card> cards;

        private int cardsCheckingIndex;
        private Card[] cardsChecking = new Card[2];

        [SerializeField] int turn;
        [SerializeField] int score;

        private void Start()
        {
            gridBoard.OnCardsGenerated += OnCardsGenerated;
            menuUi.OnPlayButton += OnPlayButton;
            menuUi.OnHomeButton += OnHomeButton;
            menuUi.OnRestartButton += OnRestartButton;
            menuUi.OnNextButton += OnPlayButton;

            gridBoard.Initialize(objectPooler);
        }

        private void OnPlayButton()
        {
            gridBoard.GenerateLevel();
        }

        private void OnHomeButton()
        {
            gridBoard.DestroyCards();
        }

        private void OnRestartButton()
        {
            gridBoard.GenerateLevel();
        }

        private void OnCardsGenerated(List<Card> cards)
        {
            this.cards = cards;

            cards.ForEach(card => card.OnCardClick += OnCardClick);
            cardsCheckingIndex = 0;
            score = 0;
            turn = 0;
            cameraSize.OnCardsGenerate(gridBoard.noOfRow);
        }


        private void OnCardClick(Card card)
        {
            cardsChecking[cardsCheckingIndex] = card;
            cardsCheckingIndex++;
            card.RevealCard();
            sFXPlayer.PlayAudioClip("Rotate");

            if (cardsCheckingIndex >= 2)
            {
                cardsCheckingIndex = 0;
                CheckMatchingCards();
            }
        }

        private void CheckMatchingCards()
        {
            turn++;
            if (cardsChecking[0].Index == cardsChecking[1].Index)
            {
                score++;
                if (score >= gridBoard.totalTargetPairs)
                {
                    OnLevelComplete();
                }
                sFXPlayer.PlayAudioClip("Match");
            }
            else
            {
                cardsChecking.ToList().ForEach(card => {_ = card.HideCard();});
                sFXPlayer.PlayAudioClip("Unmatch");
            }
            menuUi.UpdateScore(score, turn);
        }

        private void OnLevelComplete()
        {
            menuUi.OnLevelComplete();
            sFXPlayer.PlayAudioClip("Win");

        }
    }
}