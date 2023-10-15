using System;
using UnityEngine;

namespace Game
{
    public class FencePart : MonoBehaviour
    {
        public Fence Fence { get; private set; }
        public Cell CurrentCell { get; set; }

        public void SetFence(Fence fence)
        {
            Fence = fence;
        }

        public Cell GetDestinationCell(Direction direction)
        {
            return CurrentCell.GetNeighbour(direction);
        }
        
        public void SetCurrentCell(Cell cell)
        {
            CurrentCell = cell;
            CurrentCell.SetFencePart(this);
        }

        public Cell GetOppositeCell(Direction direction)
        {
            return direction switch
            {
                Direction.Up => CurrentCell.GetNeighbour(Direction.Down),
                Direction.Down => CurrentCell.GetNeighbour(Direction.Up),
                Direction.Left => CurrentCell.GetNeighbour(Direction.Right),
                Direction.Right => CurrentCell.GetNeighbour(Direction.Left),
                _ => null
            };
        }
        
        public bool CheckDestinationCell(Direction direction)
        {
            var destinationCell = CurrentCell.GetNeighbour(direction);
            if (!destinationCell) return false;
            if(destinationCell.CurrentFencePart != null && destinationCell.CurrentFencePart.Fence != Fence) return false;
            
            return true;
        }
    }
}