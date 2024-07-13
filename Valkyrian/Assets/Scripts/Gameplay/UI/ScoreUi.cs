using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Gameplay.UI
{
    public class ScoreUi : MonoBehaviour
    {
        /// <summary>
        /// Score Text
        /// </summary>
        [SerializeField] TextMeshProUGUI scoreText;

        /// <summary>
        /// No of turns Text
        /// </summary>
        [SerializeField] TextMeshProUGUI noOfTurnsText;

        /// <summary>
        /// Updating Score text
        /// </summary>
        public void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }

        /// <summary>
        /// Updating no turns text
        /// </summary>
        public void UpdateNoOfTurnsText(int noOfTurns)
        {
            noOfTurnsText.text = noOfTurns.ToString();
        }
    }
}
