//91GMS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject Car;
    public CarController RR;
    public CarRaycaster CR;
    public GameObject[] Cars;
    private string currentTime;
    public float vehicleSpeed;

    public GameObject TiltSteering;
    public GameObject ButtonSteering;


    public GameObject GamePlayUI;
    public Text TimeText;
    public Text ScoreText;
    public Text SpeedText;
    public Text DistanceText;
    public GameObject WrongWay;
    public Text t;

    public GameObject GameOverUI;
    public Text TotalDistanceText;
    public Text TotalDistanceMoney;
    public Text NearMissText;
    public Text NearMissMoney;
    public Text WrongTrackText;
    public Text WrongTrackMoney;
    public Text TotalCashText;


    public GameObject PauseMenu;
    public Sprite PauseSprite;
    public Sprite PlaySprite;

    public Material Day;
    public Material Night;

    public Text TotalScore;

    private bool isDaySelected;
    private int TScore;
    private float WrongTrackTime;
    private int gameMode;
    private bool isWrong;
    private bool foo;
    private float secondsCount;

  
    void Start()
    {
        foo = true;
        isDaySelected = (PlayerPrefs.GetInt("daySelected") == 1) ? true : false;
        if (isDaySelected)
        {
            RenderSettings.skybox = Day;
        }
        else
        {
            RenderSettings.skybox = Night;
        }

        if (PlayerPrefs.GetInt("tilt") == 1)
        {
            TiltSteering.gameObject.SetActive(true);
            ButtonSteering.gameObject.SetActive(false);
        }
        else
        {
            TiltSteering.gameObject.SetActive(false);
            ButtonSteering.gameObject.SetActive(true);
        }

        int temp = PlayerPrefs.GetInt("currentCar");
        // int temp = 3;
        Car = Instantiate(Cars[temp], new Vector3(0f, 0.5f, -23f), Quaternion.Euler(0f, 0f, 0f));
        RR = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        CR = GameObject.FindGameObjectWithTag("Player").GetComponent<CarRaycaster>();

        GameOverUI.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);

        gameMode = PlayerPrefs.GetInt("level");


    }

    
    void FixedUpdate()
    {
        vehicleSpeed = RR.KPH;
        if (RR.gameObject.active == false)
        {
            GameOverUI.gameObject.SetActive(true);
            GamePlayUI.gameObject.SetActive(false);
            ButtonSteering.gameObject.SetActive(false);
            TiltSteering.gameObject.SetActive(false);

            TotalDistanceText.text = (RR.distanceTravelled / 1500).ToString("f2");
            TotalDistanceMoney.text = "$" + (RR.distanceTravelled / 1500) * 2;

            NearMissText.text = CR.nearMisses.ToString();
            NearMissMoney.text = "$" + (CR.nearMisses * 2).ToString();

            TotalScore.text = CR.score.ToString("f1");

            WrongTrackText.text = WrongTrackTime.ToString("f1");
            WrongTrackMoney.text = "$" + (WrongTrackTime * 2).ToString();

            TScore = Mathf.RoundToInt((RR.distanceTravelled / 200) + (CR.nearMisses * 3) + ((float)(WrongTrackTime * 2.5)));
            TotalCashText.text = "$" + TScore.ToString();
            if (foo)
            {
                int c = PlayerPrefs.GetInt("coins");
                PlayerPrefs.SetInt("coins", c + TScore);
                foo = false;
            }
            return;
        }
        if ((Car.transform.position.x < -0.5f && gameMode == 2) || gameMode == 3)
        {
            WrongWay.gameObject.SetActive(true);
            WrongTrackTime += Time.deltaTime;
            t.text = WrongTrackTime.ToString("f1");
            isWrong = true;
        }

        
        secondsCount += Time.deltaTime;
        currentTime = secondsCount.ToString("f1");

        TimeText.text = currentTime + " SAN";

        SpeedText.text = vehicleSpeed.ToString("f1") + "KM/S";
        DistanceText.text = (RR.distanceTravelled / 1000).ToString("f2") + "Km";


    }
    

    public void PToggle(Button button)
    {
        if (Time.timeScale != 0)
        {
            button.GetComponent<Image>().sprite = PlaySprite;
            Time.timeScale = 0;
            PauseMenu.gameObject.SetActive(true);
            return;
        }
        else
        {
            button.GetComponent<Image>().sprite = PauseSprite;
            PauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;

        }
    }
    public void OnRestarButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }
    public void OnMainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void onChnageController()
    {
        if (PlayerPrefs.GetInt("tilt") == 1)
        {
            PlayerPrefs.SetInt("tilt", 0);
            TiltSteering.gameObject.SetActive(false);
            ButtonSteering.gameObject.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("tilt", 1);
            ButtonSteering.gameObject.SetActive(false);
            TiltSteering.gameObject.SetActive(true);

        }
    }

    
}
