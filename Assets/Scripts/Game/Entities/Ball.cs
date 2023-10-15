using UnityEngine;

namespace Game
{
    public class Ball: MonoBehaviour
    {
        public bool IsCounted { get; private set; }

        public void Counted()
        {
            IsCounted = true;
        }
    }
}