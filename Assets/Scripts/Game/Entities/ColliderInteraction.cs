using System;
using Game.Managers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class ColliderInteraction : MonoBehaviour
    {
        [Inject] private ScoreManager _scoreManager;

        private void OnTriggerEnter(Collider other)
        {
            var ball = other.GetComponent<Ball>();
            if (ball != null)
            {
                if (ball.IsCounted) return;
                ball.Counted(); 
                _scoreManager.IncreaseCurrentScore(ball);
            }
        }
    }
}