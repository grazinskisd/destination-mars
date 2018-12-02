using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ldjam43
{
    public delegate void GameEventManager();

    public class GameManager: MonoBehaviour
    {
        public event GameEventManager OnRun;
        public event GameEventManager OnPause;
        public event GameEventManager OnWreckDestroyed;

        public LoadMenu LoadMenu;
        public Ship Ship;
        public Shipwreck ShipwreckPrefab;

        public EndMenu EndMenu;
        public ParticleSystem ExplosionEffect;

        private Vector3 _shipStartPosition;
        private Vector3 _shipStartRotation;

        private List<Shipwreck> _allWrecks;
        private float _tmpFuel;
        private float _tmpOxygen;
        private float _tmpFood;
        private Shipwreck _tmpWreck;

        private void Start()
        {
            _allWrecks = new List<Shipwreck>();
            SetLoadMenuActive(true);
            LoadMenu.OnStart += LaunchShip;
            Ship.OnDie += RestartMission;
            Ship.OnWin += ShowEndMenu;
            Ship.OnCollision += ShipExplosion;

            _shipStartPosition = Ship.transform.position;
            _shipStartRotation = Ship.transform.rotation.eulerAngles;
            EndMenu.gameObject.SetActive(false);
            EndMenu.RestartButton.onClick.AddListener(RestartGame);
        }

        private static void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                RestartGame();
            }
        }

        private void ShipExplosion()
        {
            Instantiate(ExplosionEffect, Ship.transform.position, Quaternion.identity);
            IssueEvent(OnPause);
            StartCoroutine(ProcessExplossion());
        }

        private void ShowEndMenu()
        {
            EndMenu.gameObject.SetActive(true);
            EndMenu.Label.text = string.Format("Sacrifices: {0}", _allWrecks.Count);
            IssueEvent(OnPause);
        }

        private IEnumerator ProcessExplossion()
        {
            Ship.SetVisible(false);
            yield return new WaitForSeconds(4);
            ResetShipTransform();
            Ship.SetVisible(true);
            SetLoadMenuActive(true);
        }

        private void RestartMission()
        {
            IssueEvent(OnPause);
            var wreck = Instantiate(ShipwreckPrefab, Ship.transform.position, Ship.transform.rotation);
            wreck.Fuel = Ship.FuelLeft;
            wreck.Oxygen = Ship.OxygenLeft;
            wreck.Food = Ship.FoodLeft;
            wreck.OnClicked += ShopLoadMenu;
            wreck.OnCollision += DestroyWreck;
            _allWrecks.Add(wreck);

            ResetShipTransform();
            SetLoadMenuActive(true);
        }

        private void DestroyWreck(Shipwreck sender)
        {
            Instantiate(ExplosionEffect, sender.transform.position, Quaternion.identity);
            sender.OnClicked -= ShopLoadMenu;
            sender.OnCollision -= DestroyWreck;
            Destroy(sender.gameObject);
            IssueEvent(OnWreckDestroyed);
        }

        private void ResetShipTransform()
        {
            Ship.transform.position = _shipStartPosition;
            Ship.transform.rotation = Quaternion.Euler(_shipStartRotation);
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

            LoadMenu.IsLoading = false;
            LoadMenu.SetCapacity(Mathf.Round(shipCap));
            LoadMenu.SetMaxValuesForSliders(sender.Fuel, sender.Oxygen, sender.Food);
            _tmpFuel = Ship.FuelLeft;
            _tmpOxygen = Ship.OxygenLeft;
            _tmpFood = Ship.FoodLeft;
            _tmpWreck = sender;
            LoadMenu.OnSliderChanged += UpdateShipResources;
            LoadMenu.OnStart -= LaunchShip;
            LoadMenu.OnStart += ResumeMission;

            IssueEvent(OnPause);
        }

        private void ResumeMission()
        {
            LoadMenu.OnSliderChanged -= UpdateShipResources;

            _tmpWreck.Fuel = LoadMenu.Fuel.Slider.value;
            _tmpWreck.Oxygen = LoadMenu.Oxygen.Slider.value;
            _tmpWreck.Food = LoadMenu.Food.Slider.value;

            LoadMenu.ResetMenu();
            SetLoadMenuActive(false);
            LoadMenu.OnStart -= ResumeMission;
            LoadMenu.OnStart += LaunchShip;
            IssueEvent(OnRun);
        }

        private void UpdateShipResources()
        {
            var fuel = _tmpFuel + (LoadMenu.Fuel.Slider.maxValue - LoadMenu.Fuel.Slider.value);
            var oxygen = _tmpOxygen + (LoadMenu.Oxygen.Slider.maxValue - LoadMenu.Oxygen.Slider.value);
            var food = _tmpFood + (LoadMenu.Food.Slider.maxValue - LoadMenu.Food.Slider.value);

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
