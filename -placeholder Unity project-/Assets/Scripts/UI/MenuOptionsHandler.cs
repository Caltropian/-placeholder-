using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptionsHandler : MonoBehaviour
{
    public void QuitGame()
    {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
