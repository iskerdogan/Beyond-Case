using Game.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Injection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private GameManager gameManager;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().FromInstance(inputManager).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().FromInstance(levelManager).AsSingle();
            Container.BindInterfacesAndSelfTo<GridManager>().FromInstance(gridManager).AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(gameManager).AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreManager>().FromInstance(scoreManager).AsSingle();
            Container.BindInterfacesAndSelfTo<UIManager>().FromInstance(uiManager).AsSingle();
        }
    }
}
