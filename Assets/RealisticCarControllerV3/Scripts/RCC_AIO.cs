
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// RCC All In One playable demo scene manager.
/// </summary>
public class RCC_AIO : MonoBehaviour {

	static RCC_AIO instance;

	public GameObject levels;
	public GameObject back;

	private AsyncOperation async;
	public Slider slider;

	void Start () {

		if (instance) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

	}

	void Update(){

		if (async != null && !async.isDone) {

			if(!slider.gameObject.activeSelf)
				slider.gameObject.SetActive (true);
			
			slider.value = async.progress;

		} else {

			if(slider.gameObject.activeSelf)
				slider.gameObject.SetActive (false);

		}

	}

	public void LoadLevel (string levelName) {

		async = SceneManager.LoadSceneAsync (levelName);

	}

	public void ToggleMenu (GameObject menu) {

		levels.SetActive (false);
		back.SetActive (false);

		menu.SetActive (true);

	}

	public void Quit () {

		Application.Quit ();

	}

}
