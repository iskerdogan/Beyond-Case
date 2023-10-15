using System;
using UnityEngine;
using Zenject;

namespace Game.Managers
{
    public class InputManager: MonoBehaviour
    {
        public event Action Clicked;
        public event Action ClickedUp;

        private bool _isActive;
        
        private const string ButtonName = "Fire1";
        
        void Update()
        {
            if (!_isActive) return;
            CheckInput();
        }
        
        void CheckInput()
        {
            if (Input.GetButtonDown(ButtonName))
            {
                OnClicked();
            }
            if (Input.GetButtonUp(ButtonName))
            {
                OnClickedUp();
            }
        }
        private void OnClicked()
        {
            Clicked?.Invoke();
        }
        private void OnClickedUp()
        {
            ClickedUp?.Invoke();
        }

        public void SetSituation(bool situation)
        {
            _isActive = situation;
        }
    }
}