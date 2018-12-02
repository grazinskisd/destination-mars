using UnityEngine;

namespace UnityEngine
{
    [RequireComponent(typeof(RectTransform))]
    public class ObjectiveArrow: MonoBehaviour
    {
        public Transform Object;
        public Transform Objective;
        public Transform MinimapCenter;
        public float maxDistance;

        private RectTransform _rect;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            LookAtObjective();
        }

        private void Update()
        {
            var dir = (Objective.position - Object.position).normalized;
            _rect.anchoredPosition = new Vector2(dir.x * maxDistance, dir.z * maxDistance);

            LookAtObjective();
        }

        private void LookAtObjective()
        {
            transform.LookAt(MinimapCenter.position);
            transform.Rotate(0, 90, -90);
        }
    }
}
