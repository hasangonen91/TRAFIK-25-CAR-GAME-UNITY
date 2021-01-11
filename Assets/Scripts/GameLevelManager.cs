//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    public GameObject gameMode;
    public GameObject Location;

    public void Start()
    {
        gameMode.gameObject.SetActive(true);
        Location.gameObject.SetActive(false);
    }

    public void onMode1Selected()
    {
        PlayerPrefs.SetInt("level", 1);
        // SceneManager.LoadScene(2);
        gameMode.gameObject.SetActive(false);
        Location.gameObject.SetActive(true);
    }
    public void onMode2Selected()
    {
        PlayerPrefs.SetInt("level", 2);
        // SceneManager.LoadScene(2);
        gameMode.gameObject.SetActive(false);
        Location.gameObject.SetActive(true);
    }
    public void onMode3Selected()
    {

        PlayerPrefs.SetInt("level", 3);
        // SceneManager.LoadScene(2);
        gameMode.gameObject.SetActive(false);
        Location.gameObject.SetActive(true);
    }
    public void onDaySelected()
    {
        PlayerPrefs.SetInt("daySelected", 1);
        SceneManager.LoadScene(3);

    }
    public void onNightSelected()
    {
        PlayerPrefs.SetInt("daySelected", 0);

        SceneManager.LoadScene(3);

    }

}
