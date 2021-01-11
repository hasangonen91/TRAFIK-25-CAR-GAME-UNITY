
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCC_XRToggle : MonoBehaviour {

	public bool XREnabled = false;

	void Start () {

	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.V))
			ToggleXR ();

	}

	void ToggleXR(){

		UnityEngine.XR.XRSettings.enabled = !UnityEngine.XR.XRSettings.enabled;
		XREnabled = UnityEngine.XR.XRSettings.enabled;

	}

}
