using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WinScreenUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject buttonsPanel;

    [Header("Audio Setup")]
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [Header("Dialogue Configuration")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float delayAfterLine = 1.5f;

    [Header("Story Lines")]
    [TextArea(2, 5)]
    [SerializeField]
    private string[] winLines = new string[]
    {
        "Signal found...",
        "Connection re-established.",
        "The darkness recedes.",
        "THANK YOU FOR PLAYING"
    };

    [TextArea(2, 5)]
    [SerializeField]
    private string[] loseLines = new string[]
    {
        "Signal lost...",
        "The connection has timed out.",
        "You remain in the dark.",
        "GAME OVER"
    };


    public static bool GameWon = true;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();


        if (GameWon && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
        else if (!GameWon && loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }


        if (buttonsPanel != null)
            buttonsPanel.SetActive(false);

        if (dialogueText != null)
        {
            StartCoroutine(PlayDialogueRoutine());
        }
    }

    private IEnumerator PlayDialogueRoutine()
    {

        string[] currentLines = GameWon ? winLines : loseLines;

 
        foreach (string line in currentLines)
        {
            dialogueText.text = "";

            foreach (char letter in line.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            yield return new WaitForSeconds(delayAfterLine);
        }

        if (buttonsPanel != null)
            buttonsPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}