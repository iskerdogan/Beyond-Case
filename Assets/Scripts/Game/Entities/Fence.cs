using System;
using System.Collections.Generic;
using DG.Tweening;
using Game.Managers;
using UnityEngine;
using Zenject;

namespace Game
{
    public enum FenceState
    {
        Empty,
        Moving
    }
    
    enum FenceColor
    {
        Blue,
        Green,
        Red,
        Purple    
    }
    public class Fence : MonoBehaviour
    {
        [SerializeField] private FencePart[] fenceParts;
        [SerializeField] private Material[] fenceMaterials;
        [SerializeField] private FenceColor fenceColor;

        [HideInInspector] public FenceState fenceState;
        private Material _currentMaterial;
        private void Start()
        {
            InitFenceParts();
        }

        private void InitFenceParts()
        {
            foreach (var fence in fenceParts)
            {
                var meshRenderer = fence.GetComponent<MeshRenderer>();
                meshRenderer.material = fenceMaterials[(int)fenceColor];
                fence.SetFence(this);
            }
        }

        public void Move(Direction direction)
        {
            fenceState = FenceState.Moving;
            if (!CanMove(direction))
            {
                FreezeAnimation(direction);
                return;
            }
            MoveAnimation(direction);
            SetCurrentCellForAllFenceParts(direction);
            
        }

        private void MoveAnimation(Direction direction)
        {
            transform.DOLocalMove(GetDestinationPosition(direction),.3f).SetRelative().OnComplete(() =>
            {
                fenceState = FenceState.Empty;
            });
        }
        
        private void FreezeAnimation(Direction direction)
        {
            transform.DOLocalMove((GetDestinationPosition(direction) * 0.05f),.1f).SetRelative().SetLoops(2,LoopType.Yoyo).OnComplete(() =>
            {
                fenceState = FenceState.Empty;
            });
        }

        private void SetCurrentCellForAllFenceParts(Direction direction)
        {
            var oldOppositeCells = new List<Cell>();
            for (int i = 0; i < fenceParts.Length; i++)
            {
                if (fenceParts[i].GetOppositeCell(direction) == null || fenceParts[i].GetOppositeCell(direction).CurrentFencePart == null || fenceParts[i].Fence != fenceParts[i].GetOppositeCell(direction).CurrentFencePart.Fence)
                {
                    oldOppositeCells.Add(fenceParts[i].CurrentCell);
                }
                
                var destinationCell = fenceParts[i].GetDestinationCell(direction);
                fenceParts[i].SetCurrentCell(destinationCell);
            }

            foreach (var oldCell in oldOppositeCells)
            {
                oldCell.SetFencePart(null);
            }
        }

        private bool CanMove(Direction direction)
        {
            for (int i = 0; i < fenceParts.Length; i++)
            {
                var temp = i;
                if (!fenceParts[temp].CheckDestinationCell(direction)) return false;
            }
            return true;
        }
        
        private Vector3 GetDestinationPosition(Direction currentDirection)
        {
            Vector3 directionVector = Vector3.zero;

            switch (currentDirection)
            {
                case Direction.Up:
                    directionVector = Vector3.up;
                    break;
                case Direction.Down:
                    directionVector = Vector3.down;
                    break;
                case Direction.Left:
                    directionVector = Vector3.left;
                    break;
                case Direction.Right:
                    directionVector = Vector3.right;
                    break;
            }
            
            return directionVector;
        }
        
    }
}