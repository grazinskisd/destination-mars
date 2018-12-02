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

        private void Start()
        {
            LoadMenu.OnStart += LaunchShip;
            Ship.OnDie += Pause;
        }

        private void Pause()
        {
            IssueEvent(OnPause);
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
