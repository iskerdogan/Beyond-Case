using System;
using Game.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game
{
    public class Cell:MonoBehaviour
    {
        [Inject] private ScoreManager _scoreManager;

        [FormerlySerializedAs("ball")] [SerializeField] private BallsCell ballsCell;
        [SerializeField] private GameObject empty;

        public FencePart CurrentFencePart { get; private set; }
        public int id;
        public int x;
        public int y;
        public Action FenceEmpty;
        public Level level;

        private int _targetLayer = 1 << 6;

        public void Initialize(int id, int x, int y,Level level)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.level = level;
            gameObject.name = "Cell(" + x + "," + y + ")" + " id_" + id;
            
        }

        public void SetFencePart(FencePart fencePart)
        {
            CurrentFencePart = fencePart;

            if (fencePart == null)
            {
                FenceEmpty?.Invoke();
            }
        }

        public Cell GetNeighbour(Direction direction)
        {
            var neighbourPoint = GetNeighbourPoint(direction);
            return level.GetCellModel(neighbourPoint.Item1, neighbourPoint.Item2);
        }
        
        private (int, int) GetNeighbourPoint(Direction direction)
        {
            return direction switch
            {
                Direction.Up => (x, y + 1),
                Direction.Down => (x, y - 1),
                Direction.Left => (x - 1, y),
                Direction.Right => (x + 1, y),
            };
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.localPosition, Vector3.back *2);
        }

        public void Setup()
        {
            ballsCell.gameObject.SetActive(false);
            empty.SetActive(true);
            
            var position = transform.position;
            RaycastHit hit;
            if (!Physics.Raycast(position, Vector3.back, out hit,2, _targetLayer)) return;
            var fencePart = hit.transform.GetComponent<FencePart>();
            if (fencePart != null)
            {
                ballsCell.Initialize(this);
                fencePart.CurrentCell = this;
                CurrentFencePart = fencePart;
                ballsCell.gameObject.SetActive(true);
                empty.SetActive(false);
                level.ballCellCount++;
                //_scoreManager.IncreaseDestinationScore();
            }
        }
        
    }
}