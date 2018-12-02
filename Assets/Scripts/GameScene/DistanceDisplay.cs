using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public class DistanceDisplay: MonoBehaviour
    {
        public string Format;
        public Text Label;
        public Transform Object1;
        public Transform Object2;

        private void Update()
        {
            Label.text = string.Format(Format, Vector3.Distance(Object1.position, Object2.position));
        }
    }
}
