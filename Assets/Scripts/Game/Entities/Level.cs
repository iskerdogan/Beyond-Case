using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class Level: MonoBehaviour
    {
        public int width;
        public int height;
        public Cell[] cells;
        public int ballCellCount;
        
        public Cell GetCellModel(int x, int y)
        {
            if (!(x >= 0 && x < width) || !(y >= 0 && y < height)) return null;
            return cells[width * y + x];
        }

        public void SetupCells()
        {
            foreach (var cell in cells)
            {
                cell.Setup();
            }
        }
    }
}