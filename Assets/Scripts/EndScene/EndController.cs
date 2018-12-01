using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ldjam43
{
    public class EndController: MonoBehaviour
    {
        public Text SacrificesText;
        public Button RestartButton;
        public Button EndButton;

        private void Start()
        {
            EndButton.onClick.AddListener(() => Application.Quit());
            RestartButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
            SacrificesText.text = string.Format("Sacrifices: {0}", PlayerPrefs.GetInt("Sacrifices"));
        }
    }
}
