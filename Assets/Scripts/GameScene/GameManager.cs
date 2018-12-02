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
            Debug.Log("CLICK");
            // TODO: make it work, god dam it!
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
