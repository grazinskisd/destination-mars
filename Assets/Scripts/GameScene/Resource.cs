using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public delegate void ResourceEventHandler();
    public class Resource
    {
        public event ResourceEventHandler OnDepleted;

        public float Usage;
        public float Value
        {
            set
            {
                _value = value;
                _slider.value = value;
            }
            get
            {
                return _value;
            }
        }

        private float _value;

        private Slider _slider;

        public Resource(float usage, Slider slider)
        {
            Usage = usage;
            _slider = slider;
        }

        public void Update()
        {
            Value -= Usage * Time.deltaTime;
            if(Value <= 0 && OnDepleted != null)
            {
                OnDepleted();
            }
        }
    }
}
