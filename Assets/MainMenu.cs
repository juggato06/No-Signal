using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); 
    }

   
    public void QuitGame()
    {
        Debug.Log("Quit!");

        
        Application.Quit();

        
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
