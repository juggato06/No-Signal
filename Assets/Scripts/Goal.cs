using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] private string endSceneName = "WinScene";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WinScreenUI.GameWon = true;
            SceneManager.LoadScene(endSceneName);
        }
    }
}