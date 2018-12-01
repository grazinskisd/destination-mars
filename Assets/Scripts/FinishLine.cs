using UnityEngine;

namespace Ldjam43
{
    public delegate void FinishEventHandler();

    public class FinishLine: MonoBehaviour
    {
        public event FinishEventHandler OnFinish;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && OnFinish != null)
            {
                OnFinish();
            }
        }
    }
}
