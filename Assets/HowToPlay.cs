using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    public GameObject howToPlayPanel;

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
}
