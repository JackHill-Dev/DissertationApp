using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    private bool isWireframe;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // SwitchViewMode();
    }

    void AppStart()
    {
        // Called from menu, transitions user to main environment
    }

    void SwitchViewMode()
    {
        if (isWireframe)
        {
            // Set all models to wireframe mode
        }
        else
        {
            // Otherwise set to normal rendering mode
        }
    }

    public void AppRestart()
    {
        SceneManager.LoadScene(0);
    }
    void AppExit()
    {
        Application.Quit();
    }
}
