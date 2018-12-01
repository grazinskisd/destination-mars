using UnityEngine;

namespace Ldjam43
{
    public delegate void CorpseEventHandler(Corpse sender);

    public class Corpse: MonoBehaviour
    {
        public event CorpseEventHandler OnClick;
        public event CorpseEventHandler OnPlayerLeft;

        public float Water;

        [SerializeField]
        private Color _interactColor;

        [SerializeField]
        private MeshRenderer _renderer;

        private bool _isInteractive = false;
        private Color _defaultColor;

        private void Start()
        {
            _defaultColor = _renderer.material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isInteractive = true;
                _renderer.material.color = _interactColor;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isInteractive = false;
                _renderer.material.color = _defaultColor;
                IssueEvent(OnPlayerLeft);
            }
        }

        private void OnMouseDown()
        {
            if (_isInteractive)
            {
                IssueEvent(OnClick);
            }
        }

        private void IssueEvent(CorpseEventHandler eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue(this);
            }
        }
    }
}
