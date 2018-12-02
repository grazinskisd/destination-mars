using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public delegate void ShipEventHandler();

    public class Ship : MonoBehaviour
    {
        public event ShipEventHandler OnDie;

        public float MoveSpeed;
        public float RotateSpeed;
        public float FuelUsage;
        public float OxygenUsage;
        public float FoodUsage;

        public ShipResources Resources;
        public GameManager GameManager;

        private Resource Fuel;
        private Resource Oxygen;
        private Resource Food;

        public float FuelLeft { get { return Fuel.Value; } }
        public float OxygenLeft { get { return Oxygen.Value; } }
        public float FoodLeft { get { return Food.Value; } }

        private State _state;

        private void Start()
        {
            Fuel = SetupResource(FuelUsage, Resources.Fuel);
            Oxygen = SetupResource(OxygenUsage, Resources.Oxygen);
            Food = SetupResource(FoodUsage, Resources.Food);
            _state = State.Paused;

            GameManager.OnPause += () => _state = State.Paused;
            GameManager.OnRun += () => _state = State.Running;
        }

        private Resource SetupResource(float usage, Slider slider)
        {
            var resource = new Resource(usage, slider);
            resource.OnDepleted += Die;
            return resource;
        }

        private void Update()
        {
            if (_state == State.Running)
            {
                Food.Update();
                Oxygen.Update();

                if (Vertical > 0)
                {
                    transform.position += Time.deltaTime * transform.forward * MoveSpeed * Vertical;
                    Fuel.Update();
                }

                transform.Rotate(Vector3.up, Horizontal * Time.deltaTime * RotateSpeed);
            }
        }

        public void SetResources(float fuel, float oxygen, float food)
        {
            Fuel.Value = fuel;
            Oxygen.Value = oxygen;
            Food.Value = food;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Shipwreck"))
            {
                // Can interact
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Shipwreck"))
            {
                // Can no longer interact
            }
        }

        private float Horizontal
        {
            get { return Input.GetAxis("Horizontal"); }
        }

        private float Vertical
        {
            get { return Input.GetAxis("Vertical"); }
        }

        private void Die()
        {
            IssueEvent(OnDie);
        }

        private void IssueEvent(ShipEventHandler eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue();
            }
        }
    }
}