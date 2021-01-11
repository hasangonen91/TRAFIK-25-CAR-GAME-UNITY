//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCarGenerator1 : MonoBehaviour
{
    float[] Lanes = new float[4];

    public GameObject PlayerCar;
    public GameObject AICar;
    public CarController RR;
    public GameObject[] opponentCar;

    public Transform objectToFollow;
    public float number;
    public Vector3 offset = new Vector3(0f, 0f, 30f);
    int gameMode;
    public float speedF = 1f;

    void Start()
    {
        PlayerCar = GameObject.FindGameObjectWithTag("Player");
        RR = PlayerCar.GetComponent<CarController>();

        objectToFollow = PlayerCar.transform;

        gameMode = PlayerPrefs.GetInt("level");
        Lanes[0] = -1.8f;
        Lanes[1] = -0.6f;
        Lanes[2] = 0.6f;
        Lanes[3] = 1.8f;

        StartCoroutine(TrafficDensityCheck());

    }


    IEnumerator TrafficDensityCheck()
    {
        while (true)
        {
            if (RR.KPH > 20)
            {
                if (gameMode == 1)
                {
                    GenerateTraffic();
                    GenerateTraffic2();
                }
                else if (gameMode == 2)
                {
                    GenerateTraffic();
                    GenerateWrongWayTraffic();
                }
                else if (gameMode == 3)
                {
                    GenerateWrongWayTraffic();
                    GenerateWrongWayTraffic2();

                }
            }
            yield return new WaitForSeconds(speedF);

        }

    }


    private void GenerateTraffic()
    {
        int number = Random.Range(2, 4);

        int ramdomcar = Random.Range(0, 4);
        GameObject oppo = Instantiate(opponentCar[ramdomcar], new Vector3(Lanes[number], 0f, transform.position.z), Quaternion.Euler(0f, 0f, 0f));
        EnemyCarDriver CD = oppo.GetComponent<EnemyCarDriver>();
        // CD.motorSpeed = ;
    }
    private void GenerateWrongWayTraffic()
    {
        int number1 = Random.Range(0, 2);
        int ramdomcar = Random.Range(0, 4);

        Instantiate(opponentCar[ramdomcar], new Vector3(Lanes[number1], 0f, transform.position.z), Quaternion.Euler(0f, 180f, 0f));

    }
    private void GenerateTraffic2()
    {
        int number = Random.Range(0, 2);

        int ramdomcar = Random.Range(0, 4);
        GameObject oppo = Instantiate(opponentCar[ramdomcar], new Vector3(Lanes[number], 0f, transform.position.z), Quaternion.Euler(0f, 0f, 0f));
        EnemyCarDriver CD = oppo.GetComponent<EnemyCarDriver>();
    }
    private void GenerateWrongWayTraffic2()
    {
        int number1 = Random.Range(2, 4);
        int ramdomcar = Random.Range(0, 4);

        Instantiate(opponentCar[ramdomcar], new Vector3(Lanes[number1], 0f, transform.position.z), Quaternion.Euler(0f, 180f, 0f));

    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0f, objectToFollow.position.z + offset.z);
        if (PlayerCar.gameObject.active == false)
        {
            StopCoroutine(TrafficDensityCheck());
        }
    }
}
