//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour
{
	public int currentCamera;

	private void Start()
	{
		currentCamera = 1;
		offset = new Vector3(0f, 2f, -2.5f);

		objectToFollow = GameObject.FindGameObjectWithTag("Player").transform;

	}
	public void LookAtTarget()
	{
		Vector3 _lookDirection = objectToFollow.position - transform.position;
		Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
		// transform.position = Vector3.Lerp(transform.position, _lookDirection, followSpeed * Time.deltaTime);

	}

	public void MoveToTarget()
	{
		Vector3 _targetPos = objectToFollow.position +
							 objectToFollow.forward * offset.z +
							 objectToFollow.right * offset.x +
							 objectToFollow.up * offset.y;
		transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);

	}
	public void changeCarAngle()
	{
		currentCamera++;
		if (currentCamera > 2)
			currentCamera = 1;

		if (currentCamera == 1)
		{
			offset = new Vector3(0f, 2f, -2.5f);
		}
		else if (currentCamera == 2)
		{
			offset = new Vector3(0f, 0.5f, -0.5f);

		}
	}


	private void FixedUpdate()
	{
		LookAtTarget();
		MoveToTarget();
		// TOP();
		transform.LookAt(objectToFollow);

	}






	public Transform objectToFollow;
	public Vector3 offset;
	public float followSpeed = 10;
	public float lookSpeed = 10;
}