using UnityEditor;
using UnityEngine;

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
}
