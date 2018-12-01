using UnityEngine;
using UnityEngine.UI;

namespace Ldjam43
{
    public class Loot : MonoBehaviour
    {
        public string Format;
        public Text Label;
        public Button TakeButton;
        public float LootAmount;

        public void SetLoot(float loot)
        {
            LootAmount = loot;
            UpdateLabel();
        }

        public void UpdateLabel()
        {
            Label.text = string.Format(Format, LootAmount);
        }
    }
}
