//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCarDestory : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    private void Start()
    {
        objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void OnTriggerEnter(Collider other)
    {
        //  Destory(other);   
        if (other.tag == "Opponent")
        {
            Destroy(other.gameObject);
        }
    }
    void FixedUpdate()
    {
        Vector3 _targetPos = objectToFollow.position +
                             objectToFollow.forward * offset.z +
                             objectToFollow.right * offset.x +
                             objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _targetPos, 10f * Time.deltaTime);

    }
}
