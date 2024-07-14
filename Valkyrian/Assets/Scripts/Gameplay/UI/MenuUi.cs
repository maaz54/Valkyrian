using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class MenuUi : MonoBehaviour
    {
        /// <summary>
        /// Restart button in the menu UI
        /// </summary>
        [SerializeField] Button restartButton;

        /// <summary>
        /// Home button in the menu UI.
        /// </summary>
        [SerializeField] Button homeButton;

        /// <summary>
        /// Play button in the menu UI.
        /// </summary>
        [SerializeField] Button playButton;

        /// <summary>
        /// Next button in the menu UI.
        /// </summary>
        [SerializeField] Button nextButton;

        /// <summary>
        /// Restart button in the menu UI
        /// </summary>
        [SerializeField] Button SaveButton;

        /// <summary>
        /// Restart button in the menu UI
        /// </summary>
        [SerializeField] Button LoadButton;

        /// <summary>
        /// gameplay UI panel
        /// </summary>
        [SerializeField] GameObject gameplay;

        /// <summary>
        /// menu UI panel
        /// </summary>
        [SerializeField] GameObject menu;

        /// <summary>
        /// Level Complete panel
        /// </summary>
        [SerializeField] GameObject LevelComplete;

        /// <summary>
        /// Score UI Panel
        /// </summary>
        [SerializeField] ScoreUi scoreUi;

        /// <summary>
        /// event when the "Restart" button is clicked
        /// </summary>
        public Action OnRestartButton;

        /// <summary>
        /// event when the Home button is clicked
        /// </summary>
        public Action OnHomeButton;

        /// <summary>
        /// event when the Play button is clicked
        /// </summary>
        public Action OnPlayButton;

        /// <summary>
        /// event when the Play button is clicked
        /// </summary>
        public Action OnNextButton;

        /// <summary>
        /// event when the Play button is clicked
        /// </summary>
        public Action OnLoadPreviousLevelButton;

        /// <summary>
        /// event when the Play button is clicked
        /// </summary>
        public Action OnSaveCurrentLevelButton;

        private void Start()
        {
            ButtonEvents();
            OpenMenu();
        }

        /// <summary>
        /// sets up button click event 
        /// </summary>
        private void ButtonEvents()
        {
            restartButton.onClick.AddListener(() => OnRestartButton?.Invoke());
            homeButton.onClick.AddListener(() =>
            {
                OpenMenu();
                OnHomeButton?.Invoke();
            });
            playButton.onClick.AddListener(() =>
            {
                OpenGameplay();
                OnPlayButton?.Invoke();
            });

            nextButton.onClick.AddListener(() =>
            {
                OpenGameplay();
                OnNextButton?.Invoke();
            });

            LoadButton.onClick.AddListener(() =>
            {
                OnLoadPreviousLevelButton?.Invoke();
            });

            SaveButton.onClick.AddListener(() =>
            {
                OnSaveCurrentLevelButton?.Invoke();
            });
        }

        /// <summary>
        /// Enabling Gameplay Panel
        /// </summary>
        private void OpenGameplay()
        {
            gameplay.SetActive(true);
            menu.SetActive(false);
            LevelComplete.SetActive(false);

            UpdateScore(0, 0);
        }

        /// <summary>
        /// Enabling Menu Panel
        /// </summary>
        private void OpenMenu()
        {
            menu.SetActive(true);
            gameplay.SetActive(false);
            LevelComplete.SetActive(false);
        }

        public void OnLevelComplete()
        {
            menu.SetActive(false);
            gameplay.SetActive(false);
            LevelComplete.SetActive(true);
        }

        public void UpdateScore(int score, int turns)
        {
            scoreUi.UpdateScoreText(score);
            scoreUi.UpdateNoOfTurnsText(turns);
        }

    }
}
