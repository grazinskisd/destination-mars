using UnityEngine;

namespace Ldjam43
{
    public class CameraFollow: MonoBehaviour
    {
        public Transform ObjectToFollow;

        private Vector3 _offset;

        private void Start()
        {
            _offset = transform.position;
        }

        private void Update()
        {
            transform.position = ObjectToFollow.position + _offset;
        }
    }
}
