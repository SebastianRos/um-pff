using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Main Menu tutorial https://www.youtube.com/watch?v=zc8ac_qUXQY
public class MainMenu : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
