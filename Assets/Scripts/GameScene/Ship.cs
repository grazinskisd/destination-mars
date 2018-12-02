using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public delegate void ShipEventHandler();

    public class Ship : MonoBehaviour
    {
        public event ShipEventHandler OnDie;
        public event ShipEventHandler OnWin;
        public event ShipEventHandler OnCollision;
        public event ShipEventHandler OnThurst;
        public event ShipEventHandler OnThurstStop;

        private const string SHIPWRECK = "Shipwreck";
        private const string MARS = "Mars";

        public float MoveSpeed;
        public float RotateSpeed;
        public float FuelUsage;
        public float OxygenUsage;
        public float FoodUsage;

        public ShipResources Resources;
        public GameManager GameManager;
        public Renderer Renderer;
        public Rigidbody Rigidbody;
        public ParticleSystem ThrustEffect;

        private Resource Fuel;
        private Resource Oxygen;
        private Resource Food;

        public float FuelLeft { get { return Fuel.Value; } }
        public float OxygenLeft { get { return Oxygen.Value; } }
        public float FoodLeft { get { return Food.Value; } }

        private State _state;
        private bool _isMoving;

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
                    Rigidbody.velocity = transform.forward * MoveSpeed * Vertical;
                    Fuel.Update();

                    if (!_isMoving)
                    {
                        ThrustEffect.Play();
                        _isMoving = true;
                        IssueEvent(OnThurst);
                        Rigidbody.velocity = Vector3.zero;
                    }
                }
                else
                {
                    if (_isMoving)
                    {
                        ThrustEffect.Stop();
                        _isMoving = false;
                        IssueEvent(OnThurstStop);
                    }
                }

                transform.Rotate(Vector3.up, Horizontal * Time.deltaTime * RotateSpeed);

                if (Input.GetKeyDown(KeyCode.K))
                {
                    Die();
                }
            }
            else
            {
                Rigidbody.velocity = Vector3.zero;
                ThrustEffect.Stop();
            }
        }

        public void SetVisible(bool isVisible)
        {
            Renderer.enabled = isVisible;
        }

        public void SetResources(float fuel, float oxygen, float food)
        {
            Fuel.Value = fuel;
            Oxygen.Value = oxygen;
            Food.Value = food;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(SHIPWRECK))
            {
                // Can interact
                var wreck = other.GetComponentInParent<Shipwreck>();
                wreck.Interactable = true;
            }
            else if (other.CompareTag(MARS))
            {
                // Win
                IssueEvent(OnWin);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(SHIPWRECK))
            {
                // Can no longer interact
                var wreck = other.GetComponentInParent<Shipwreck>();
                wreck.Interactable = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            IssueEvent(OnCollision);
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