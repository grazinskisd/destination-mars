using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public delegate void LoadMenuEventHandler();
    public class LoadMenu: MonoBehaviour
    {
        private const string CAPACITY = "Capacity: {0}";
        private const string FUEL = "Fuel: {0}";
        private const string OXYGEN = "Oxygen: {0}";
        private const string FOOD = "Food: {0}";

        public event LoadMenuEventHandler OnStart;
        public event LoadMenuEventHandler OnSliderChanged;

        public int StartCapacity;
        public Text CapacityLabel;
        public Load Fuel;
        public Load Oxygen;
        public Load Food;
        public Button StartButton;

        private bool _canStart;
        private float _currentCapacity;

        [HideInInspector]
        public bool IsLoading;

        public void SetCapacity(float newCapacity)
        {
            _currentCapacity = newCapacity;
        }

        public void SetSliderValues(float fuel, float oxygen, float food)
        {
            Fuel.Slider.value = fuel;
            Oxygen.Slider.value = oxygen;
            Food.Slider.value = food;
        }

        public void SetMinValuesForSliders(float fuel, float oxygen, float food)
        {
            Fuel.Slider.minValue = fuel;
            Oxygen.Slider.minValue = oxygen;
            Food.Slider.minValue = food;
        }

        public void SetMaxValuesForSliders(float fuel, float oxygen, float food)
        {
            Fuel.Slider.maxValue = fuel;
            Oxygen.Slider.maxValue = oxygen;
            Food.Slider.maxValue = food;
        }

        public void ResetMenu()
        {
            ResetSlider(Fuel.Slider);
            ResetSlider(Oxygen.Slider);
            ResetSlider(Food.Slider);

            _currentCapacity = StartCapacity;
            SetSliderValues(33, 33, 33);
            UpdateCapacity();
        }

        private void ResetSlider(Slider slider)
        {
            slider.minValue = 0;
            slider.maxValue = 100;
            IsLoading = true;
        }

        private void Start()
        {
            ResetMenu();

            Fuel.Slider.onValueChanged.AddListener((val) =>
            {
                Fuel.Label.text = string.Format(FUEL, Mathf.Round(val));
                UpdateCapacity();
                IssueEvent(OnSliderChanged);
            });

            Oxygen.Slider.onValueChanged.AddListener((val) =>
            {
                Oxygen.Label.text = string.Format(OXYGEN, Mathf.Round(val));
                UpdateCapacity();
                IssueEvent(OnSliderChanged);
            });

            Food.Slider.onValueChanged.AddListener((val) =>
            {
                Food.Label.text = string.Format(FOOD, Mathf.Round(val));
                UpdateCapacity();
                IssueEvent(OnSliderChanged);
            });

            SetSliderValues(33, 33, 33);
            StartButton.onClick.AddListener(() => IssueEvent(OnStart));
            UpdateCapacity();
        }

        private void UpdateCapacity()
        {
            float leftCap = _currentCapacity - (IsLoading ? GetSum() : GetDiffSum());
            CapacityLabel.text = string.Format(CAPACITY, Mathf.Round(leftCap));
            CapacityLabel.color = leftCap < 0 ? Color.red : Color.white;
            StartButton.interactable = leftCap >= 0;
        }

        private float GetSum()
        {
            return (Fuel.Slider.value + Oxygen.Slider.value + Food.Slider.value);
        }

        private float GetDiffSum()
        {
            return GetDiff(Fuel.Slider) +
                    GetDiff(Oxygen.Slider) +
                    GetDiff(Food.Slider);
        }

        private float GetDiff(Slider slider)
        {
            return slider.maxValue - slider.value;
        }

        private void IssueEvent(LoadMenuEventHandler eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue();
            }
        }
    }
}
