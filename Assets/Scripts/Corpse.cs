using UnityEngine;

namespace Ldjam43
{
    public delegate void CorpseEventHandler(Corpse sender);

    public class Corpse: MonoBehaviour
    {
        public event CorpseEventHandler OnClick;
        public event CorpseEventHandler OnPlayerLeft;

        public float Water;

        private bool _isInteractive = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isInteractive = true;
                Debug.Log("PLAYER HIT ME!");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isInteractive = false;
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
