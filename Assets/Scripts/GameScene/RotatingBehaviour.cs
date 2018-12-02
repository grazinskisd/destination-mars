using UnityEngine;

namespace Ldjam43
{
    public class RotatingBehaviour: MonoBehaviour
    {
        public float RotationSpeed;
        public Transform RotationCenter;

        private void Update()
        {
            transform.RotateAround(RotationCenter.position, Vector3.up, RotationSpeed * Time.deltaTime);
        }
    }
}
