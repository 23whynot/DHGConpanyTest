using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.UI.WinMenu
{
    public class WinMenuInputController : MonoBehaviour
    {
        [SerializeField] private Button restartButton;

        private void Start() => restartButton.onClick.AddListener(Restart);

        private void OnDestroy() => restartButton.onClick.RemoveAllListeners();

        private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}