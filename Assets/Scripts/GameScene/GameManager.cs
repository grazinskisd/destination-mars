using System;
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

        private void Start()
        {
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

            Ship.transform.position = _shipStartPosition;
            Ship.transform.rotation = Quaternion.Euler(_shipStartRotation);
            LoadMenu.gameObject.SetActive(true);
        }

        private void LaunchShip()
        {
            Ship.SetResources(
                LoadMenu.Fuel.Slider.value,
                LoadMenu.Oxygen.Slider.value,
                LoadMenu.Food.Slider.value);
            IssueEvent(OnRun);
            LoadMenu.gameObject.SetActive(false);
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
