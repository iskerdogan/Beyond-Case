using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Managers
{
    public class BallsCell : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] balls;
        private Cell _cell;

        public void Initialize(Cell cell)
        {
            _cell = cell;
            cell.FenceEmpty += OnFenceEmpty;
        }

        private void OnFenceEmpty()
        {
            _cell.FenceEmpty -= OnFenceEmpty;
            BallAddForce();
        }

        private async void BallAddForce()
        {
            var temp = 0;
            for (int i = 0; i < balls.Length; i++)
            {
                temp++;
                balls[i].isKinematic = false;
                balls[i].transform.SetParent(null);
                //var force = 100 - i *2;
                balls[i].AddForce(Vector3.back * 100);
                if (temp != 3) continue;
                await UniTask.Delay(30);
                temp = 0;
            }
        }
    }
}