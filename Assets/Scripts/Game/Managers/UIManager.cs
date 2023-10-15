using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class UIManager: MonoBehaviour,IInitializable
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private GameObject successPanel;

        public void Initialize()
        {
            successPanel.SetActive(false);
            inGamePanel.SetActive(true);
        }

        public void OnScoreChange(int currentScore,int destinationScore)
        {
            scoreText.SetText(currentScore + "/" + destinationScore);
        }

        public void UpdateLevelText(int level)
        {
            levelText.SetText("Level " + level);
        }

        public void OnLevelComplate()
        {
            successPanel.SetActive(true);
            inGamePanel.SetActive(false);
        }
    }
}