using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ldjam43
{
    public class GameManager: MonoBehaviour
    {
        public Player Player;
        public Corpse CorpsePrototype;
        public CorpsePopup CorpsePopup;
        public FinishLine FinishLine;
        public KeyCode RestartKey;

        private Vector3 _playerStart;
        private int _corpseCount;

        private void Start()
        {
            Player.OnDie += ProcessDeath;
            _playerStart = Player.transform.position;
            _corpseCount = 0;

            CorpsePopup.CloseButton.onClick.AddListener(() => CorpsePopup.gameObject.SetActive(false));
            CorpsePopup.gameObject.SetActive(false);

            FinishLine.OnFinish += DisplayEnd;
        }

        private void Update()
        {
            if (Input.GetKeyDown(RestartKey))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void DisplayEnd()
        {
            PlayerPrefs.SetInt("Sacrifices", _corpseCount);
            SceneManager.LoadScene("EndScene");
        }

        private void ProcessDeath(Player sender)
        {
            PlaceCorpse(sender);
            sender.transform.position = _playerStart;
            sender.ResetPlayer();
        }

        private void PlaceCorpse(Player sender)
        {
            var corpse = Instantiate(CorpsePrototype);
            var position = corpse.transform.position;
            position.x = sender.transform.position.x;
            position.z = sender.transform.position.z;
            corpse.transform.position = position;

            corpse.Water = sender.RemainingWater;
            corpse.OnClick += ShowPopup;
            corpse.OnPlayerLeft += (_) => CorpsePopup.gameObject.SetActive(false);
            _corpseCount++;
        }

        private void ShowPopup(Corpse sender)
        {
            CorpsePopup.gameObject.SetActive(true);
            CorpsePopup.Water.SetLoot(sender.Water);
            CorpsePopup.Water.TakeButton.onClick.RemoveAllListeners();
            CorpsePopup.Water.TakeButton.onClick.AddListener(() =>
            {
                float waterUsed = (Player.WaterCapacity - Player.RemainingWater);
                Player.RemainingWater = Mathf.Min(Player.RemainingWater + CorpsePopup.Water.LootAmount, Player.WaterCapacity);
                var leftWater = Mathf.Max(0, CorpsePopup.Water.LootAmount - waterUsed);
                CorpsePopup.Water.SetLoot(leftWater);
                sender.Water = leftWater;
            });
        }
    }
}
