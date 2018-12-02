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
        public event LoadMenuEventHandler OnExit;

        public int StartCapacity;
        public Text CapacityLabel;
        public Load Fuel;
        public Load Oxygen;
        public Load Food;
        public Button StartButton;
        public Button ExitButton;

        private bool _canStart;

        private void Start()
        {
            Fuel.Slider.onValueChanged.AddListener((val) =>
            {
                Fuel.Label.text = string.Format(FUEL, val);
                UpdateCapacity();
            });

            Oxygen.Slider.onValueChanged.AddListener((val) =>
            {
                Oxygen.Label.text = string.Format(OXYGEN, val);
                UpdateCapacity();
            });

            Food.Slider.onValueChanged.AddListener((val) =>
            {
                Food.Label.text = string.Format(FOOD, val);
                UpdateCapacity();
            });

            Fuel.Slider.value = 33;
            Oxygen.Slider.value = 33;
            Food.Slider.value = 33;

            StartButton.onClick.AddListener(() => IssueEvent(OnStart));
            ExitButton.onClick.AddListener(() => IssueEvent(OnExit));

            UpdateCapacity();
        }

        private void UpdateCapacity()
        {
            var leftCap = StartCapacity - (Fuel.Slider.value + Oxygen.Slider.value + Food.Slider.value);
            CapacityLabel.text = string.Format(CAPACITY, leftCap);
            CapacityLabel.color = leftCap < 0 ? Color.red : Color.white;
            StartButton.interactable = leftCap >= 0;
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
