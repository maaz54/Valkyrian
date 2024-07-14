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
        [SerializeField] SaveLoadController saveLoadController;

        private List<Card> cards;

        private int cardsCheckingIndex;
        private Card[] cardsChecking = new Card[2];

        [SerializeField] int turn;
        [SerializeField] int score;

        private void Start()
        {
            gridBoard.OnCardsGenerated += OnCardsGenerated;
            gridBoard.OnCardsGeneratedFromJson += OnCardsGeneratedFromJson;
            menuUi.OnPlayButton += OnPlayButton;
            menuUi.OnHomeButton += OnHomeButton;
            menuUi.OnRestartButton += OnRestartButton;
            menuUi.OnNextButton += OnPlayButton;
            menuUi.OnSaveCurrentLevelButton += SaveData;
            menuUi.OnLoadPreviousLevelButton += LoadData;

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

        private void OnCardsGeneratedFromJson(List<Card> cards)
        {
            this.cards = cards;
            cards.ForEach(card => card.OnCardClick += OnCardClick);
            cardsCheckingIndex = 0;
            cardsChecking = new Card[2];
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
                cardsChecking.ToList().ForEach(card => { _ = card.HideCard(); });
                sFXPlayer.PlayAudioClip("Unmatch");
            }
            menuUi.UpdateScore(score, turn);
        }

        public void SaveData()
        {
            List<SaveLoadController.CardData> cardData = new List<SaveLoadController.CardData>();

            cards.ForEach(card => cardData.Add(new SaveLoadController.CardData
            {
                Index = card.Index,
                IsRevealed = card.IsRevealed,
            }));

            SaveLoadController.LevelData levelData = new SaveLoadController.LevelData
            {
                Score = this.score,
                Turn = this.turn,
                TotalTargetPair = gridBoard.totalTargetPairs,
                Cards = cardData
            };

            saveLoadController.SaveData(levelData, "LevelData");
        }

        public void LoadData()
        {
            SaveLoadController.LevelData levelData = saveLoadController.LoadData("LevelData");
            this.score = levelData.Score;
            this.turn = levelData.Turn;
            gridBoard.LoadSavedLevel(levelData);
        }

        private void OnLevelComplete()
        {
            menuUi.OnLevelComplete();
            sFXPlayer.PlayAudioClip("Win");
        }
    }
}