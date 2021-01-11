
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_Spawner : MonoBehaviour {

	// Use this for initialization
	void Start () {

		int selectedIndex = PlayerPrefs.GetInt ("SelectedRCCVehicle", 0);

		RCC.SpawnRCC (RCC_Vehicles.Instance.vehicles [selectedIndex], transform.position, transform.rotation, true, true, true);
		
	}

}
