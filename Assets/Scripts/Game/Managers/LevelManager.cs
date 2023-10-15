using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Managers
{
    public class LevelManager:MonoBehaviour,IInitializable
    {
        [Inject] private GameManager _gameManager;
        [Inject] private UIManager _uiManager;
        [Inject] private InputManager _inputManager;
        
        [SerializeField] private Level[] levels;
        [SerializeField] private Box box;
        [SerializeField] private Camera mainCamera;
        [HideInInspector] public Level currentLevel;

        public void Initialize()
        {
            BuildLevel();
        }

        void BuildLevel()
        {
             var level = _gameManager.levelNumberToBuildLevel;
             var prefab = levels[level];
             currentLevel = Instantiate(prefab);
             currentLevel.SetupCells();
             box.transform.position =  new Vector3(currentLevel.transform.position.x, box.transform.position.y, box.transform.position.z);
             mainCamera.transform.position = new Vector3(currentLevel.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
             LevelStartAnimations();
             _uiManager.UpdateLevelText(_gameManager.levelNumberToDisplay +1);
        }

        void LevelStartAnimations()
        {
            var cachedCurrentLevelPosition = currentLevel.transform.localPosition;
            var cachedBoxPosition = box.transform.localPosition;
            
            currentLevel.transform.localPosition = currentLevel.transform.localPosition + Vector3.up * 15;
            box.transform.localPosition = box.transform.localPosition + Vector3.down * 15;

            currentLevel.transform.DOLocalMove(cachedCurrentLevelPosition, .5f).SetEase(Ease.Linear).OnComplete(() => _inputManager.SetSituation(true));
            box.transform.DOLocalMove(cachedBoxPosition, .5f).OnComplete(() =>
            {
                box.OpenBoxAnimation();
            }).SetEase(Ease.Linear);
        }
        
        async void LevelEndAnimations()
        {
            _inputManager.SetSituation(false);
            box.CloseBoxAnimation();
            await UniTask.Delay(1000);
            currentLevel.transform.DOLocalMove(currentLevel.transform.localPosition + Vector3.up * 15, .5f).SetEase(Ease.Linear).OnComplete(() =>_uiManager.OnLevelComplate());
            box.transform.DOLocalMove(box.transform.localPosition + Vector3.down * 15, .5f).SetEase(Ease.Linear);
        }

        #region LevelEnd

        public void LevelComplete()
        {
            _gameManager.LevelUp();
            LevelEndAnimations();
        }

        public void ReLoadScene()
        {
            SceneManager.LoadSceneAsync(sceneBuildIndex: SceneManager.GetActiveScene().buildIndex);
        }

        #endregion
    }
}