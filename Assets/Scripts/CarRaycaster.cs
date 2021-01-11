//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarRaycaster : MonoBehaviour
{
	float radius = 1f;
	bool DidRightHit = false, DidLeftHit = false;
	RaycastHit raycastHit;
	public float score = 0;
	public Text scoreText;
	public AudioClip swoosh;
	public GameObject game;

	public CarController RR;

	string currentTrafficCarNameLeft, currentTrafficCarNameRight;
	public int nearMisses;
	internal float timeLeft = 100f;
	internal int combo;
	internal int maxCombo;
	private float comboTime;

	public float speed;
	public int carSelected;
	private void Start()
	{
		carSelected = PlayerPrefs.GetInt("currentCar");

		RR = gameObject.GetComponent<CarController>();
		scoreText = GameObject.Find("GamePlayUI").transform.GetChild(0).gameObject.GetComponentsInChildren<Text>()[1];
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		speed = RR.KPH;
		CheckNearMiss();
	}

	void CheckNearMiss()
	{
		RaycastHit hit;

		Debug.DrawRay(transform.position, (-transform.right * 1f), Color.white);
		Debug.DrawRay(transform.position, (transform.right * 1f), Color.white);
		Debug.DrawRay(transform.position, (transform.forward * 5f), Color.white);

		if (Physics.Raycast(transform.position, (-transform.right), out hit, 1f) && !hit.collider.isTrigger)
		{
			currentTrafficCarNameLeft = hit.transform.name;
		}
		else
		{

			if (currentTrafficCarNameLeft != null && speed > 30)
			{

				nearMisses++;
				AudioSource.PlayClipAtPoint(swoosh, transform.position, 1.0F);

				combo++;
				comboTime = 0;
				if (maxCombo <= combo)
					maxCombo = combo;
				if (carSelected == 0)
				{
					score += 10f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				else if (carSelected == 1)
				{
					score += 15f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				else if (carSelected == 2)
				{
					score += 25f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				else if (carSelected == 3)
				{
					score += 30f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				scoreText.text = score.ToString();
				currentTrafficCarNameLeft = null;

			}
			else
			{

				currentTrafficCarNameLeft = null;

			}

		}


		if (Physics.Raycast(transform.position, (transform.right), out hit, 1f) && !hit.collider.isTrigger)
		{
			currentTrafficCarNameRight = hit.transform.name;
		}
		else
		{

			if (currentTrafficCarNameRight != null && speed > 30)
			{

				nearMisses++;
				AudioSource.PlayClipAtPoint(swoosh, transform.position, 1.0F);
				combo++;
				comboTime = 0;
				if (maxCombo <= combo)
					maxCombo = combo;

				if (carSelected == 0)
				{
					score += 10f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				else if (carSelected == 1)
				{
					score += 15f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				else if (carSelected == 2)
				{
					score += 25f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				else if (carSelected == 3)
				{
					score += 30f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
				}
				scoreText.text = score.ToString();

				currentTrafficCarNameRight = null;

			}
			else
			{

				currentTrafficCarNameRight = null;

			}

		}
	}

}
