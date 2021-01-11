//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCarGenerator : MonoBehaviour
{
    public GameObject oppoCar;

    public Transform objectToFollow;
    public Vector3 offset = new Vector3(0f, 0f, 20f);
    int gameMode;
    // Start is called before the first frame update
    void Start()
    {

        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;
        gameMode = PlayerPrefs.GetInt("level");
        if (gameMode == 1)
        {
            InvokeRepeating("GenerateTraffic", 2f, 1.3f);

        }
        else if (gameMode == 2)
        {
            InvokeRepeating("GenerateTraffic", 2f, 1.3f);
            InvokeRepeating("GenerateWrongWayTraffic", 2f, 1.3f);

        }
        else
        {
            Debug.Log("Error");
        }
    }

    // Update is called once per frame
    private void GenerateTraffic()
    {
        float number = Random.Range(2, 3);
        Instantiate(oppoCar, new Vector3(number, 0f, transform.position.z), Quaternion.Euler(0f, 180f, 0f));

    }
    private void GenerateWrongWayTraffic()
    {

    }
    void FixedUpdate()
    {

        //      Vector3 _targetPos = objectToFollow.position + 
        //					 objectToFollow.forward * offset.z + 
        //					 //objectToFollow.right * offset.x + 
        //					 objectToFollow.up * offset.y;
        //transform.position = Vector3.Lerp(transform.position, _targetPos, 10f * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, 0f, objectToFollow.position.z + offset.z);
    }
}
