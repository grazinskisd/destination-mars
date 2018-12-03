using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ldjam43
{
    public class StartController: MonoBehaviour
    {
        public Button StartButton;

        private void Start()
        {
            StartButton.onClick.AddListener(() =>
                SceneManager.LoadScene("GameScene")
            );
        }
    }
}
