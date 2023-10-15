using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Managers
{
    public class GridManager:MonoBehaviour,IInitializable
    {
        [Inject] private InputManager _inputManager;
        [Inject] private LevelManager _levelManager;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private float animationAngle = 5f;
        [SerializeField] private float animationDuration = .2f;
        
        private bool _isCameraNull;
        private bool _isDragging;
        private Fence _currentFence;
        private Direction _currentDirection;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private int _targetLayer = 1 << 6;
        
        public void Initialize()
        {
            Subscribe();
            _isCameraNull = _camera == null;
        }

        private void Update()
        {
            if (!_isDragging) return;
            HandleMouseDirection();
        }

        private void Subscribe()
        {
            _inputManager.Clicked += Clicked;
            _inputManager.ClickedUp += ClickedUp;
        }

        private void Unsubscribe()
        {
            _inputManager.Clicked -= Clicked;
            _inputManager.ClickedUp -= ClickedUp;
        }
        
        private void HandleMouseDirection()
        {
            _endPosition = Input.mousePosition;
            Vector3 movementDirection = _endPosition - _startPosition;
            if (movementDirection.magnitude > 5f) 
            {
                movementDirection.Normalize();

                float horizontal = movementDirection.x;
                float vertical = movementDirection.y;

                _currentDirection = GetDirectionFromInput(horizontal, vertical);
            }
            
        }
        
        private Direction GetDirectionFromInput(float horizontal, float vertical)
        {
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                return (horizontal > 0) ? Direction.Right : Direction.Left;
            }
            
            return (vertical > 0) ? Direction.Up : Direction.Down;
        }

        private void Clicked()
        {
            if (_isCameraNull) return;
            var rayMouse = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit,100,_targetLayer)) return;
            var fencePart = hit.transform.GetComponent<FencePart>();
            if (fencePart == null) return;
            var fence = fencePart.Fence;
            if (fence.fenceState == FenceState.Moving) return;
            _startPosition = Input.mousePosition;
            _currentFence = fence;
            _isDragging = true;
        }

        private void ClickedUp()
        {
            _isDragging = false;
            if (_currentFence == null) return;
            _currentFence.Move(_currentDirection);
            RotateAnimation(_levelManager.currentLevel.transform,_currentDirection);
            _currentFence = null;
        }

        public void RotateAnimation(Transform obj,Direction direction)
        {
            obj.DORotate(GetRotateAngle(direction, animationAngle), animationDuration/2).From(Vector3.zero).OnComplete(() => obj.DORotate(Vector3.zero, animationDuration/2));
        }

        private Vector3 GetRotateAngle(Direction direction,float angle)
        {
            return direction switch
            {
                Direction.Up => Vector3.right * angle,
                Direction.Down => Vector3.left * angle,
                Direction.Left => Vector3.up * angle,
                Direction.Right => Vector3.down * angle,
            };
        }
    }
}