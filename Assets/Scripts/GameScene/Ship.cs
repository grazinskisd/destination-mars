using UnityEngine;

namespace Ldjam43
{
    public class Ship : MonoBehaviour
    {
        public float MoveSpeed;
        public float RotateSpeed;

        private void Update()
        {
            if(Vertical > 0)
            {
                transform.position += Time.deltaTime * transform.forward * MoveSpeed * Vertical;
            }

            transform.Rotate(Vector3.up, Horizontal * Time.deltaTime * RotateSpeed);
        }

        private float Horizontal
        {
            get { return Input.GetAxis("Horizontal"); }
        }

        private float Vertical
        {
            get { return Input.GetAxis("Vertical"); }
        }
    }
}