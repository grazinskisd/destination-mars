using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public delegate void PlayerEventHandler(Player sender);

    public class Player : MonoBehaviour
    {
        public event PlayerEventHandler OnDie;

        public KeyCode KillKey;
        public float MoveSpeed;
        public float WaterCapacity;
        public float WaterUsage;
        public Slider WaterMeter;

        private Transform _transform;
        private float _remainingWater;

        private void Awake()
        {
            _transform = transform;
        }

        void Start()
        {
            ResetPlayer();
        }

        public float RemainingWater
        {
            get
            {
                return _remainingWater;
            }

            set
            {
                _remainingWater = value;
                WaterMeter.value = _remainingWater / WaterCapacity;
            }
        }

        void Update()
        {
            if (Horizontal != 0 || Vertical != 0)
            {
                _transform.position += Time.deltaTime * MoveSpeed * (Vector3.right * Horizontal + Vector3.forward * Vertical);
                RemainingWater -= Time.deltaTime * WaterUsage;

                if (RemainingWater <= 0)
                {
                    RemainingWater = 0;
                    Die();
                }
            }

            if (Input.GetKeyDown(KillKey))
            {
                Die();
            }
        }

        private void Die()
        {
            IssuePlayerEvent(OnDie);
        }

        public void ResetPlayer()
        {
            RemainingWater = WaterCapacity;
        }

        private void IssuePlayerEvent(PlayerEventHandler eventToIssue)
        {
            if (eventToIssue != null)
            {
                eventToIssue(this);
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
    }
}