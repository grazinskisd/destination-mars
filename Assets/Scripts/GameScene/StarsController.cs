using UnityEngine;

namespace Ldjam43
{
    public class StarsController: MonoBehaviour
    {
        public Transform Follow;
        public float Lag;

        private Vector3 _lastPosition;

        private void Update()
        {
            if(_lastPosition != Follow.position)
            {
                var diff = Follow.position - _lastPosition;
                transform.position += diff * Lag;
                _lastPosition = Follow.position;
            }
        }
    }
}
