using System;
using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [Inject] private LevelManager _levelManager;
        [Inject] private UIManager _uiManager;
        
        public int DestinationScore { get; private set; }
        public int CurrentScore { get; private set; }

        private Ball[] collectedBalls;

        private void Start()
        {
            Invoke(nameof(Initialize),.15f);
        }

        public void Initialize()
        {
            DestinationScore = _levelManager.currentLevel.ballCellCount * 9;
            collectedBalls = new Ball[DestinationScore];
            _uiManager.OnScoreChange(CurrentScore,DestinationScore);
        }

        public void IncreaseCurrentScore(Ball collectBall)
        {
            collectedBalls[CurrentScore] = collectBall;
            CurrentScore++;
            _uiManager.OnScoreChange(CurrentScore,DestinationScore);
            if (CurrentScore >= DestinationScore) _levelManager.LevelComplete();
        }

        public void CloseAllCollectedBalls()
        {
            foreach (var ball in collectedBalls)
            {
                ball.gameObject.SetActive(false);
            }
        }
    }
}