using UnityEngine;

namespace Ldjam43
{
    public delegate void ShipwreckEventHandler(Shipwreck sender);

    public class Shipwreck: MonoBehaviour
    {
        public event ShipwreckEventHandler OnClicked;
        public event ShipwreckEventHandler OnCollision;

        public float Fuel;
        public float Oxygen;
        public float Food;
        public Renderer Renderer;

        public Color InteractableColor;
        private Color _defaultColor;

        private bool _interactable;

        private void Start()
        {
            _defaultColor = Renderer.material.color;
        }

        public bool Interactable
        {
            set
            {
                _interactable = value;
                Renderer.material.color = _interactable ? InteractableColor : _defaultColor;
            }
        }

        private void OnMouseDown()
        {
            if (_interactable)
            {
                IssueEvent(OnClicked);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            IssueEvent(OnCollision);
        }

        private void IssueEvent(ShipwreckEventHandler eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue(this);
            }
        }
    }
}
