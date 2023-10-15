using UnityEngine;

namespace Game
{
    public class LevelCreator: MonoBehaviour
    {
        public int width;
        public int height;
        [SerializeField] private Cell cellViewPrefab;
        
#if UNITY_EDITOR
        [ContextMenu("CreateGrid")]
        
        public void CreateGrid()
        {
            GameObject parent = new GameObject
            {
                name = "Level"
            };
            var level = parent.AddComponent<Level>();
            level.width = width;
            level.height = height;
            level.cells = new Cell[width * height];
            Vector3 cellParentPosition = new Vector3((width -1) * .5f, (height - 1) *.5f, 0);
            level.transform.position = cellParentPosition;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 cellPosition = new Vector3(x, y, 0);
                    Cell cellView = Instantiate(cellViewPrefab, cellPosition,Quaternion.identity, level.transform);
                    var id = width * y + x;
                    cellView.Initialize(id,x,y,level);
                    level.cells[id] = cellView;
                }
            }
        }
#endif
    }
}