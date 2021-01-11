
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene manager that contains current player vehicle, current player camera, current player UI, current player character, recording/playing mechanim, and other vehicles as well.
/// 
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Main/RCC Scene Manager")]
public class RCC_SceneManager : MonoBehaviour {

	#region singleton
	private static RCC_SceneManager instance;
	public static RCC_SceneManager Instance{
		
		get{
			
			if (instance == null) {

				instance = FindObjectOfType<RCC_SceneManager> ();

				if (instance == null) {
					
					GameObject sceneManager = new GameObject ("_RCCSceneManager");
					instance = sceneManager.AddComponent<RCC_SceneManager> ();

				}

			}
			
			return instance;

		}

	}

	#endregion

	public RCC_CarControllerV3 activePlayerVehicle;
	private RCC_CarControllerV3 lastActivePlayerVehicle;
	public RCC_Camera activePlayerCamera;
	public RCC_UIDashboardDisplay activePlayerCanvas;
	public Camera activeMainCamera;

	public bool registerFirstVehicleAsPlayer = true;
	public bool disableUIWhenNoPlayerVehicle = false;
	public bool loadCustomizationAtFirst = true;

	internal RCC_Recorder recorder;
	public enum RecordMode{Neutral, Play, Record}
	public RecordMode recordMode;

	// Default time scale of the game.
	private float orgTimeScale = 1f;

	public List <RCC_CarControllerV3> allVehicles = new List<RCC_CarControllerV3> ();

	#if BCG_ENTEREXIT
	public BCG_EnterExitPlayer activePlayerCharacter;
	#endif

	// Firing an event when main controller changed.
	public delegate void onMainControllerChanged();
	public static event onMainControllerChanged OnMainControllerChanged;

	// Firing an event when main behavior changed.
	public delegate void onBehaviorChanged();
	public static event onBehaviorChanged OnBehaviorChanged;

	// Firing an event when player vehicle changed.
	public delegate void onVehicleChanged();
	public static event onVehicleChanged OnVehicleChanged;

	void Awake(){

		RCC_Camera.OnBCGCameraSpawned += RCC_Camera_OnBCGCameraSpawned;

		RCC_CarControllerV3.OnRCCPlayerSpawned += RCC_CarControllerV3_OnRCCSpawned;
		RCC_AICarController.OnRCCAISpawned += RCC_AICarController_OnRCCAISpawned;
		RCC_CarControllerV3.OnRCCPlayerDestroyed += RCC_CarControllerV3_OnRCCPlayerDestroyed;
		RCC_AICarController.OnRCCAIDestroyed += RCC_AICarController_OnRCCAIDestroyed;
		activePlayerCanvas = GameObject.FindObjectOfType<RCC_UIDashboardDisplay> ();

		#if BCG_ENTEREXIT
		BCG_EnterExitPlayer.OnBCGPlayerSpawned += BCG_EnterExitPlayer_OnBCGPlayerSpawned;
		BCG_EnterExitPlayer.OnBCGPlayerDestroyed += BCG_EnterExitPlayer_OnBCGPlayerDestroyed;
		#endif

		// Getting default time scale of the game.
		orgTimeScale = Time.timeScale;
		recorder = gameObject.GetComponent<RCC_Recorder> ();

		if (!recorder)
			recorder = gameObject.AddComponent<RCC_Recorder> ();

		if(RCC_Settings.Instance.lockAndUnlockCursor)
			Cursor.lockState = CursorLockMode.Locked;

		#if ENABLE_VR
		UnityEngine.XR.XRSettings.enabled = RCC_Settings.Instance.useVR;
		#endif
		
	}

	#region ONSPAWNED

	void RCC_CarControllerV3_OnRCCSpawned (RCC_CarControllerV3 RCC){

		if (!allVehicles.Contains (RCC))
			allVehicles.Add (RCC);

		if (registerFirstVehicleAsPlayer)
			RegisterPlayer (RCC);

		#if BCG_ENTEREXIT
		if (RCC.gameObject.GetComponent<BCG_EnterExitVehicle> ())
			RCC.gameObject.GetComponent<BCG_EnterExitVehicle> ().correspondingCamera = activePlayerCamera.gameObject;
		#endif

	}

	void RCC_AICarController_OnRCCAISpawned (RCC_AICarController RCCAI){

		if (!allVehicles.Contains (RCCAI.carController))
			allVehicles.Add (RCCAI.carController);

	}

	void RCC_Camera_OnBCGCameraSpawned (GameObject BCGCamera){

		activePlayerCamera = BCGCamera.GetComponent<RCC_Camera>();

	}

	#if BCG_ENTEREXIT
	void BCG_EnterExitPlayer_OnBCGPlayerSpawned (BCG_EnterExitPlayer player){

		activePlayerCharacter = player;

	}
	#endif

	#endregion

	#region ONDESTROYED

	void RCC_CarControllerV3_OnRCCPlayerDestroyed (RCC_CarControllerV3 RCC){

		if (allVehicles.Contains (RCC))
			allVehicles.Remove (RCC);

	}

	void RCC_AICarController_OnRCCAIDestroyed (RCC_AICarController RCCAI){

		if (allVehicles.Contains (RCCAI.carController))
			allVehicles.Remove (RCCAI.carController);

	}

	#if BCG_ENTEREXIT
	void BCG_EnterExitPlayer_OnBCGPlayerDestroyed (BCG_EnterExitPlayer player){

	}
	#endif

	#endregion

	void Update(){

		if (activePlayerVehicle) {

			if (activePlayerVehicle != lastActivePlayerVehicle) {
				
				if (OnVehicleChanged != null)
					OnVehicleChanged ();

			}

			lastActivePlayerVehicle = activePlayerVehicle;

		}

		if(disableUIWhenNoPlayerVehicle && activePlayerCanvas)
			CheckCanvas ();

		if (Input.GetKeyDown (RCC_Settings.Instance.recordKB))
			RCC.StartStopRecord ();

		if (Input.GetKeyDown (RCC_Settings.Instance.playbackKB))
			RCC.StartStopReplay ();

		if (Input.GetKey (RCC_Settings.Instance.slowMotionKB))
			Time.timeScale = .2f;

		if (Input.GetKeyUp (RCC_Settings.Instance.slowMotionKB))
			Time.timeScale = orgTimeScale;

		if(Input.GetButtonDown("Cancel"))
			Cursor.lockState = CursorLockMode.None;

		activeMainCamera = Camera.main;

		switch (recorder.mode) {

		case RCC_Recorder.Mode.Neutral:

			recordMode = RecordMode.Neutral;

			break;

		case RCC_Recorder.Mode.Play:

			recordMode = RecordMode.Play;

			break;

		case RCC_Recorder.Mode.Record:

			recordMode = RecordMode.Record;

			break;

		}

	}

	public void RegisterPlayer(RCC_CarControllerV3 playerVehicle){

		activePlayerVehicle = playerVehicle;

		if(activePlayerCamera)
			activePlayerCamera.SetTarget (activePlayerVehicle.gameObject);

		if (loadCustomizationAtFirst)
			RCC_Customization.LoadStats (RCC_SceneManager.Instance.activePlayerVehicle);

		if (GameObject.FindObjectOfType<RCC_CustomizerExample> ()) 
			GameObject.FindObjectOfType<RCC_CustomizerExample> ().CheckUIs ();

	}

	public void RegisterPlayer(RCC_CarControllerV3 playerVehicle, bool isControllable){

		activePlayerVehicle = playerVehicle;
		activePlayerVehicle.SetCanControl(isControllable);

		if(activePlayerCamera)
			activePlayerCamera.SetTarget (activePlayerVehicle.gameObject);

		if (GameObject.FindObjectOfType<RCC_CustomizerExample> ()) 
			GameObject.FindObjectOfType<RCC_CustomizerExample> ().CheckUIs ();

	}

	public void RegisterPlayer(RCC_CarControllerV3 playerVehicle, bool isControllable, bool engineState){

		activePlayerVehicle = playerVehicle;
		activePlayerVehicle.SetCanControl(isControllable);
		activePlayerVehicle.SetEngine (engineState);

		if(activePlayerCamera)
			activePlayerCamera.SetTarget (activePlayerVehicle.gameObject);

		if (GameObject.FindObjectOfType<RCC_CustomizerExample> ()) 
			GameObject.FindObjectOfType<RCC_CustomizerExample> ().CheckUIs ();

	}

	public void DeRegisterPlayer(){

		if (activePlayerVehicle)
			activePlayerVehicle.SetCanControl (false);
		
		activePlayerVehicle = null;

		if (activePlayerCamera)
			activePlayerCamera.RemoveTarget ();

	}

	public void CheckCanvas(){

		if (!activePlayerVehicle || !activePlayerVehicle.gameObject.activeInHierarchy || !activePlayerVehicle.enabled) {

//			if (activePlayerCanvas.displayType == RCC_UIDashboardDisplay.DisplayType.Full)
//				activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Off);

			activePlayerCanvas.SetDisplayType(RCC_UIDashboardDisplay.DisplayType.Off);

			return;

		}

//		if(!activePlayerCanvas.gameObject.activeInHierarchy)
//			activePlayerCanvas.displayType = RCC_UIDashboardDisplay.DisplayType.Full;

		if(activePlayerCanvas.displayType != RCC_UIDashboardDisplay.DisplayType.Customization)
			activePlayerCanvas.displayType = RCC_UIDashboardDisplay.DisplayType.Full;

	}

	///<summary>
	/// Sets new behavior.
	///</summary>
	public static void SetBehavior(int behaviorIndex){

		RCC_Settings.Instance.overrideBehavior = true;
		RCC_Settings.Instance.behaviorSelectedIndex = behaviorIndex;

		if (OnBehaviorChanged != null)
			OnBehaviorChanged ();

	}

	///<summary>
	/// Sets the main controller type.
	///</summary>
	public static void SetController(int controllerIndex){

		RCC_Settings.Instance.controllerSelectedIndex = controllerIndex;

		switch (controllerIndex) {

		case 0:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Keyboard;
			break;

		case 1:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Mobile;
			break;

		case 2:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.XBox360One;
			break;

		case 3:
			RCC_Settings.Instance.controllerType = RCC_Settings.ControllerType.Custom;
			break;

		}

		if(OnMainControllerChanged != null)
			OnMainControllerChanged ();

	}

	// Changes current camera mode.
	public void ChangeCamera () {

		if(RCC_SceneManager.Instance.activePlayerCamera)
			RCC_SceneManager.Instance.activePlayerCamera.ChangeCamera();

	}

	void OnDisable(){

		RCC_Camera.OnBCGCameraSpawned -= RCC_Camera_OnBCGCameraSpawned;

		RCC_CarControllerV3.OnRCCPlayerSpawned -= RCC_CarControllerV3_OnRCCSpawned;
		RCC_AICarController.OnRCCAISpawned -= RCC_AICarController_OnRCCAISpawned;
		RCC_CarControllerV3.OnRCCPlayerDestroyed -= RCC_CarControllerV3_OnRCCPlayerDestroyed;
		RCC_AICarController.OnRCCAIDestroyed -= RCC_AICarController_OnRCCAIDestroyed;
		#if BCG_ENTEREXIT
		BCG_EnterExitPlayer.OnBCGPlayerSpawned -= BCG_EnterExitPlayer_OnBCGPlayerSpawned;
		BCG_EnterExitPlayer.OnBCGPlayerDestroyed -= BCG_EnterExitPlayer_OnBCGPlayerDestroyed;
		#endif

	}

}
