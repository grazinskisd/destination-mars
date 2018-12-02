using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public delegate void ResourceEventHandler();
    public class Resource
    {
        public event ResourceEventHandler OnDepleted;

        public float Usage;
        public float Value;

        private Slider _slider;

        public Resource(float usage, Slider slider)
        {
            Usage = usage;
            _slider = slider;
        }

        public void Update()
        {
            Value -= Usage * Time.deltaTime;
            _slider.value = Value;
            if(Value <= 0 && OnDepleted != null)
            {
                OnDepleted();
            }
        }
    }
}
