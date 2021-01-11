//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

	// Raycast distances used for detecting obstacles at front of our AI vehicle.
	public int wideRayLength = 20;
	public int tightRayLength = 20;
	public int sideRayLength = 3;
	public GameObject PlayerCar;
	public CarController carController;
	private bool raycasting = false;        // Raycasts hits an obstacle now?
	private float rayInput = 0f;                // Total ray input affected by raycast distances.

	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		PlayerCar = GameObject.FindGameObjectWithTag("Player");
		carController = PlayerCar.GetComponent<CarController>();
		rb = GetComponent<Rigidbody>();

	}

	// Update is called once per frame
	void Update()
	{
		// Vector3 center =new Vector3(transform.position.x,transform.position.y,transform.position.z);
		// Vector3 center = gameObject.transform.position;
		FindPlayer(transform.position, 10f);
	}
	void OnDrawGizmosSelected()
	{
		// Draw a yellow sphere at the transform's position
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 5);
	}
	void FindPlayer(Vector3 center, float radius)
	{
		Collider[] hitColliders = Physics.OverlapSphere(center, radius);
		int i = 0;
		while (i < hitColliders.Length)
		{
			if (hitColliders[i].tag == "Player")
			{
				Debug.Log("helloworld");
				rb.velocity = (PlayerCar.transform.position);
			}
			i++;
		}
	}
	void FixedRaycasts()
	{

		// Ray pivot position.
		Vector3 pivotPos = transform.position;
		pivotPos += transform.forward * carController.wheels[1].transform.localPosition.z;

		RaycastHit hit;

		// New bools effected by fixed raycasts.
		bool tightTurn = false;
		bool wideTurn = false;
		bool sideTurn = false;
		bool tightTurn1 = false;
		bool wideTurn1 = false;
		bool sideTurn1 = false;

		// New input steers effected by fixed raycasts.
		float newinputSteer1 = 0f;
		float newinputSteer2 = 0f;
		float newinputSteer3 = 0f;
		float newinputSteer4 = 0f;
		float newinputSteer5 = 0f;
		float newinputSteer6 = 0f;

		// Drawing Rays.
		Debug.DrawRay(pivotPos, Quaternion.AngleAxis(25, transform.up) * transform.forward * wideRayLength, Color.white);
		Debug.DrawRay(pivotPos, Quaternion.AngleAxis(-25, transform.up) * transform.forward * wideRayLength, Color.white);

		Debug.DrawRay(pivotPos, Quaternion.AngleAxis(7, transform.up) * transform.forward * tightRayLength, Color.white);
		Debug.DrawRay(pivotPos, Quaternion.AngleAxis(-7, transform.up) * transform.forward * tightRayLength, Color.white);

		Debug.DrawRay(pivotPos, Quaternion.AngleAxis(90, transform.up) * transform.forward * sideRayLength, Color.white);
		Debug.DrawRay(pivotPos, Quaternion.AngleAxis(-90, transform.up) * transform.forward * sideRayLength, Color.white);

		// Wide Raycasts.
		if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(25, transform.up) * transform.forward, out hit, wideRayLength) && !hit.collider.isTrigger && hit.transform.root != transform)
		{

			Debug.DrawRay(pivotPos, Quaternion.AngleAxis(25, transform.up) * transform.forward * wideRayLength, Color.red);
			newinputSteer1 = Mathf.Lerp(-.5f, 0f, (hit.distance / wideRayLength));
			wideTurn = true;

		}

		else
		{

			newinputSteer1 = 0f;
			wideTurn = false;

		}

		if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(-25, transform.up) * transform.forward, out hit, wideRayLength) && !hit.collider.isTrigger && hit.transform.root != transform)
		{

			Debug.DrawRay(pivotPos, Quaternion.AngleAxis(-25, transform.up) * transform.forward * wideRayLength, Color.red);
			newinputSteer4 = Mathf.Lerp(.5f, 0f, (hit.distance / wideRayLength));
			wideTurn1 = true;

		}
		else
		{

			newinputSteer4 = 0f;
			wideTurn1 = false;

		}

		// Tight Raycasts.
		if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(7, transform.up) * transform.forward, out hit, tightRayLength) && !hit.collider.isTrigger && hit.transform.root != transform)
		{

			Debug.DrawRay(pivotPos, Quaternion.AngleAxis(7, transform.up) * transform.forward * tightRayLength, Color.red);
			newinputSteer3 = Mathf.Lerp(-1f, 0f, (hit.distance / tightRayLength));
			tightTurn = true;

		}
		else
		{

			newinputSteer3 = 0f;
			tightTurn = false;

		}

		if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(-7, transform.up) * transform.forward, out hit, tightRayLength) && !hit.collider.isTrigger && hit.transform.root != transform)
		{

			Debug.DrawRay(pivotPos, Quaternion.AngleAxis(-7, transform.up) * transform.forward * tightRayLength, Color.red);
			newinputSteer2 = Mathf.Lerp(1f, 0f, (hit.distance / tightRayLength));
			tightTurn1 = true;

		}
		else
		{

			newinputSteer2 = 0f;
			tightTurn1 = false;

		}

		// Side Raycasts.
		if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(90, transform.up) * transform.forward, out hit, sideRayLength) && !hit.collider.isTrigger && hit.transform.root != transform)
		{

			Debug.DrawRay(pivotPos, Quaternion.AngleAxis(90, transform.up) * transform.forward * sideRayLength, Color.red);
			newinputSteer5 = Mathf.Lerp(-1f, 0f, (hit.distance / sideRayLength));
			sideTurn = true;

		}
		else
		{

			newinputSteer5 = 0f;
			sideTurn = false;

		}

		if (Physics.Raycast(pivotPos, Quaternion.AngleAxis(-90, transform.up) * transform.forward, out hit, sideRayLength) && !hit.collider.isTrigger && hit.transform.root != transform)
		{

			Debug.DrawRay(pivotPos, Quaternion.AngleAxis(-90, transform.up) * transform.forward * sideRayLength, Color.red);
			newinputSteer6 = Mathf.Lerp(1f, 0f, (hit.distance / sideRayLength));
			sideTurn1 = true;

		}
		else
		{

			newinputSteer6 = 0f;
			sideTurn1 = false;

		}

		// Raycasts hits an obstacle now?
		if (wideTurn || wideTurn1 || tightTurn || tightTurn1 || sideTurn || sideTurn1)
			raycasting = true;
		else
			raycasting = false;

		// If raycast hits a collider, feed rayInput.
		if (raycasting)
			rayInput = (newinputSteer1 + newinputSteer2 + newinputSteer3 + newinputSteer4 + newinputSteer5 + newinputSteer6);
		else
			rayInput = 0f;

		// // If rayInput is too much, ignore navigator input.
		// if(raycasting && Mathf.Abs(rayInput) > .5f)
		// 	ignoreWaypointNow = true;
		// else
		// 	ignoreWaypointNow = false;

	}
}
