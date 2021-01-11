using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarDriver : MonoBehaviour
{

    public WheelCollider frontRightW, frontLeftW;     //left is Driver  right is Passengert 
    public WheelCollider rearRightW, rearLeftW;
    public Transform frontRightT, frontLeftT;
    public Transform rearRightT, rearLeftT;
    public float motorForce = 60;
    public GameObject COM;
    public Rigidbody rigidbody;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        COM = GameObject.Find("COM");
        rigidbody.centerOfMass = COM.transform.localPosition;


    }
    public void Accelerate()
    {
        rearRightW.motorTorque = 1f * motorForce;
        rearLeftW.motorTorque = 1f * motorForce;
    }

    private void FixedUpdate()
    {
        Accelerate();
    }
    public void isAnotherCar()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, (transform.forward * 20f), Color.white);
        if (Physics.Raycast(transform.position, (transform.forward), out hit, 20f) && !hit.collider.isTrigger)
        {
            frontLeftW.brakeTorque = frontRightW.brakeTorque = 500f;
            rearRightW.brakeTorque = rearLeftW.brakeTorque = 500f;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Opponent")
        {
            gameObject.SetActive(false);
        }
    }
}
