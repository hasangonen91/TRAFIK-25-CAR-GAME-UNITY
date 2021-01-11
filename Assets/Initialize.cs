//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    void Awake()
    {
        int speed = 60;
        int price = 400;
        int chk = PlayerPrefs.GetInt("chk", 0);
        if (chk == 0)
        {
            PlayerPrefs.SetInt("chk", 1);
            for (int i = 0; i < transform.childCount; i++)
            {

                string temp = (transform.GetChild(i)).ToString();
                carClass obj = new carClass();
                obj.topSpeed = speed;
                speed += 10;
                obj.price = price;
                if (i != 0)
                {
                    price += 100;
                }
                obj.CarID = i;
                if (i != 0)
                {
                    obj.isLocked = true;
                }
                else
                {
                    obj.isLocked = false;
                }
                string json = JsonUtility.ToJson(obj);
                PlayerPrefs.SetString(temp, json);

            }
            PlayerPrefs.SetInt("coins", 10);

        }
    }
}
