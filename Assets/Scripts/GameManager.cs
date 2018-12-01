using System;
using UnityEngine;

namespace Ldjam43
{
    public class GameManager: MonoBehaviour
    {
        public Player Player;
        public Corpse CorpsePrototype;
        public CorpsePopup CorpsePopup;

        private Vector3 _playerStart;

        private void Start()
        {
            Player.OnDie += ProcessDeath;
            _playerStart = Player.transform.position;

            CorpsePopup.CloseButton.onClick.AddListener(() => CorpsePopup.gameObject.SetActive(false));
            CorpsePopup.gameObject.SetActive(false);
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
                CorpsePopup.Water.SetLoot(Mathf.Max(0, CorpsePopup.Water.LootAmount - waterUsed));
                sender.Water = Mathf.Max(0, CorpsePopup.Water.LootAmount - waterUsed);
            });
        }
    }
}
