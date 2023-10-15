using Game.Managers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Box : MonoBehaviour
    {
        [Inject] private ScoreManager _scoreManager;
        
        [SerializeField] private Animator animator;
        
        private static readonly int OpenBox = Animator.StringToHash("OpenBox");
        private static readonly int CloseBox = Animator.StringToHash("CloseBox");

        public void OpenBoxAnimation()
        {
            animator.SetTrigger(OpenBox);
        }        
        
        public void CloseBoxAnimation()
        {
            animator.SetTrigger(CloseBox);
        }

        public void CloseBalls()
        {
            _scoreManager.CloseAllCollectedBalls();
        }
    }
}