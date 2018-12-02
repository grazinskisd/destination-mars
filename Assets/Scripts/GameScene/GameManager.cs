using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ldjam43
{
    public delegate void GameEventManager();

    public class GameManager: MonoBehaviour
    {
        public event GameEventManager OnRun;
        public event GameEventManager OnPause;

        public LoadMenu LoadMenu;
        public Ship Ship;
        public Shipwreck ShipwreckPrefab;

        private Vector3 _shipStartPosition;
        private Vector3 _shipStartRotation;

        private List<Shipwreck> _allWrecks;

        private void Start()
        {
            _allWrecks = new List<Shipwreck>();
            SetLoadMenuActive(true);
            LoadMenu.OnStart += LaunchShip;
            Ship.OnDie += RestartMission;
            _shipStartPosition = Ship.transform.position;
            _shipStartRotation = Ship.transform.rotation.eulerAngles;
        }

        private void RestartMission()
        {
            IssueEvent(OnPause);
            var wreck = Instantiate(ShipwreckPrefab, Ship.transform.position, Ship.transform.rotation);
            wreck.Fuel = Ship.FuelLeft;
            wreck.Oxygen = Ship.OxygenLeft;
            wreck.Food = Ship.FoodLeft;
            wreck.OnClicked += ShopLoadMenu;
            _allWrecks.Add(wreck);

            Ship.transform.position = _shipStartPosition;
            Ship.transform.rotation = Quaternion.Euler(_shipStartRotation);
            SetLoadMenuActive(true);
        }

        private void SetLoadMenuActive(bool isActive)
        {
            LoadMenu.gameObject.SetActive(isActive);
        }

        private void ShopLoadMenu(Shipwreck sender)
        {
            SetLoadMenuActive(true);
            LoadMenu.SetSliderValues(sender.Fuel, sender.Oxygen, sender.Food);
            var shipCap = LoadMenu.StartCapacity - (Ship.FoodLeft + Ship.FuelLeft + Ship.OxygenLeft);
            //var wreckCap = LoadMenu.StartCapacity - (sender.Fuel + sender.Oxygen + sender.Food);

            Debug.Log("shipCap: " + shipCap);

            //var minFuel = GetMin(Ship.FuelLeft, sender.Fuel, shipCap);
            //var minOxygen = GetMin(Ship.OxygenLeft, sender.Oxygen, shipCap);
            //var minFood = GetMin(Ship.FoodLeft, sender.Food, shipCap);
            //LoadMenu.SetMinValuesForSliders(minFuel, minOxygen, minFood);

            //var maxFuel = GetMax(Ship.FuelLeft, sender.Fuel, wreckCap);
            //var maxOxygen = GetMax(Ship.OxygenLeft, sender.Oxygen, wreckCap);
            //var maxFood = GetMax(Ship.FoodLeft, sender.Food, wreckCap);
            LoadMenu.IsLoading = false;
            LoadMenu.SetCapacity(Mathf.Round(shipCap));
            LoadMenu.SetMaxValuesForSliders(sender.Fuel, sender.Oxygen, sender.Food);

            LoadMenu.OnSliderChanged += UpdateShipResources;

            IssueEvent(OnPause);
            Debug.Log("CLICK");
            // TODO: make it work, god dam it!
        }

        private void UpdateShipResources()
        {
            var fuel = Ship.FuelLeft + (LoadMenu.Fuel.Slider.maxValue - LoadMenu.Fuel.Slider.value);
            var oxygen = Ship.OxygenLeft + (LoadMenu.Oxygen.Slider.maxValue - LoadMenu.Oxygen.Slider.value);
            var food = Ship.FoodLeft + (LoadMenu.Food.Slider.maxValue - LoadMenu.Food.Slider.value);

            Ship.SetResources(fuel, oxygen, food);
        }

        private float GetMin(float current, float left, float capacity)
        {
            return Mathf.Clamp((current + left) - capacity, 0, capacity);
        }

        private float GetMax(float current, float left, float capacity)
        {
            return Mathf.Clamp((current + left), 0, capacity);
        }

        private void LaunchShip()
        {
            Ship.SetResources(
                LoadMenu.Fuel.Slider.value,
                LoadMenu.Oxygen.Slider.value,
                LoadMenu.Food.Slider.value);
            IssueEvent(OnRun);
            SetLoadMenuActive(false);
        }

        private void IssueEvent(GameEventManager eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue();
            }
        }
    }
}
