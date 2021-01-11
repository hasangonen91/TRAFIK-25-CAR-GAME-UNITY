﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_FuelStation : MonoBehaviour {

	private RCC_CarControllerV3 targetVehicle;
	public float refillSpeed = 1f;

	void OnTriggerStay (Collider col) {

		if (targetVehicle == null) {

			if (col.gameObject.GetComponentInParent<RCC_CarControllerV3> ())
				targetVehicle = col.gameObject.GetComponentInParent<RCC_CarControllerV3> ();

		}

		if(targetVehicle)
			targetVehicle.fuelTank += refillSpeed * Time.deltaTime;
		
	}

	void OnTriggerExit (Collider col) {

		if (col.gameObject.GetComponentInParent<RCC_CarControllerV3> ())
			targetVehicle = null;

	}

}
