//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public void onPlayClicked()
    {
        SceneManager.LoadScene(1);

    }
    public void onExitClicked()
    {
        Application.Quit();
    }
}
