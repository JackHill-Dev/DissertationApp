using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    public void AppStart()
    {
        // Called from menu, transitions user to main environment
        SceneManager.LoadScene(1);
    }

    public void AppRestart()
    {
        SceneManager.LoadScene(1);
    }
    public void AppExit()
    {
        // Only work on full build not in editor
        Application.Quit();
    }
}
