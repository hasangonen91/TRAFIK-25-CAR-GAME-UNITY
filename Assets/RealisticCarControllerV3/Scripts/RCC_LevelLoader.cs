
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RCC_LevelLoader : MonoBehaviour {

	public void LoadLevel (string levelName) {

		SceneManager.LoadScene (levelName);
		
	}

}
