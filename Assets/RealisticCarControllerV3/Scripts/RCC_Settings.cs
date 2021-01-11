
using UnityEngine;
using System.Collections;

/// <summary>
/// Stored all general shared RCC settings here.
/// </summary>
[System.Serializable]
public class RCC_Settings : ScriptableObject {

	public string RCCVersion = "3.3";
	
	#region singleton
	private static RCC_Settings instance;
	public static RCC_Settings Instance{	get{if(instance == null) instance = Resources.Load("RCC Assets/RCC_Settings") as RCC_Settings; return instance;}}
	#endregion

	public int controllerSelectedIndex;
	public int behaviorSelectedIndex;

	public BehaviorType selectedBehaviorType{
		get{
			if (overrideBehavior)
				return behaviorTypes [behaviorSelectedIndex];
			else
				return null;
		}
	}

	public bool overrideFixedTimeStep = true;
	[Range(.005f, .06f)]public float fixedTimeStep = .02f;
	[Range(.5f, 20f)]public float maxAngularVelocity = 6;

	public bool overrideBehavior = true;

	// Behavior Types
	[System.Serializable]
	public class BehaviorType{

		public string behaviorName = "New Behavior";

		[Header("Steering Helpers")]
		public bool steeringHelper = true;
		public bool tractionHelper = true;
		public bool ABS = false;
		public bool ESP = false;
		public bool TCS = false;
		public bool applyExternalWheelFrictions = false;
		public bool applyRelativeTorque = false;

		public float highSpeedSteerAngleMinimum = 40f;
		public float highSpeedSteerAngleMaximum = 40f;

		public float highSpeedSteerAngleAtspeedMinimum = 100f;
		public float highSpeedSteerAngleAtspeedMaximum = 100f;

		[Space()]
		[Range(0f, 1f)]public float steerHelperAngularVelStrengthMinimum = .1f;
		[Range(0f, 1f)]public float steerHelperAngularVelStrengthMaximum = .1f;

		[Range(0f, 1f)]public float steerHelperLinearVelStrengthMinimum = .1f;
		[Range(0f, 1f)]public float steerHelperLinearVelStrengthMaximum = .1f;

		[Range(0f, 1f)]public float tractionHelperStrengthMinimum = .1f;
		[Range(0f, 1f)]public float tractionHelperStrengthMaximum = .1f;

		[Space()]
		public float antiRollFrontHorizontalMinimum = 1000f;
		public float antiRollRearHorizontalMinimum = 1000f;

		[Space()]
		[Range(0f, 1f)]public float gearShiftingDelayMaximum = .15f;

		[Range(0f, 10f)]public float angularDrag = .1f;

		[Header("Wheel Frictions Forward")]
		public float forwardExtremumSlip = .4f;
		public float forwardExtremumValue = 1f;
		public float forwardAsymptoteSlip = .8f;
		public float forwardAsymptoteValue = .5f;

		[Header("Wheel Frictions Sideways")]
		public float sidewaysExtremumSlip = .2f;
		public float sidewaysExtremumValue = 1f;
		public float sidewaysAsymptoteSlip = .5f;
		public float sidewaysAsymptoteValue = .75f;
	
	}

	// Behavior Types
	public BehaviorType[] behaviorTypes;

	public bool useFixedWheelColliders = true;
	public bool lockAndUnlockCursor = true;

	// Controller Type
	public ControllerType controllerType;
	public enum ControllerType{Keyboard, Mobile, XBox360One, Custom}

	// Keyboard Inputs
	public string verticalInput = "Vertical";
	public string horizontalInput = "Horizontal";
	public string mouseXInput = "Mouse X";
	public string mouseYInput = "Mouse Y";

	public KeyCode handbrakeKB = KeyCode.Space;
	public KeyCode startEngineKB = KeyCode.I;
	public KeyCode lowBeamHeadlightsKB = KeyCode.L;
	public KeyCode highBeamHeadlightsKB = KeyCode.K;
	public KeyCode rightIndicatorKB = KeyCode.E;
	public KeyCode leftIndicatorKB = KeyCode.Q;
	public KeyCode hazardIndicatorKB = KeyCode.Z;
	public KeyCode shiftGearUp = KeyCode.LeftShift;
	public KeyCode shiftGearDown = KeyCode.LeftControl;
	public KeyCode NGear = KeyCode.N;
	public KeyCode boostKB = KeyCode.F;
	public KeyCode slowMotionKB = KeyCode.G;
	public KeyCode changeCameraKB = KeyCode.C;
	public KeyCode recordKB = KeyCode.R;
	public KeyCode playbackKB = KeyCode.P;
	public KeyCode lookBackKB = KeyCode.B;
	public KeyCode trailerAttachDetach = KeyCode.T;

	// XBox Inputs
	public string Xbox_verticalInput = "Xbox_Vertical";
	public string Xbox_horizontalInput = "Xbox_Horizontal";
	public string Xbox_triggerLeftInput = "Xbox_TriggerLeft";
	public string Xbox_triggerRightInput = "Xbox_TriggerRight";
	public string Xbox_mouseXInput = "Xbox_MouseX";
	public string Xbox_mouseYInput = "Xbox_MouseY";

	public string Xbox_handbrakeKB = "Xbox_B";
	public string Xbox_startEngineKB = "Xbox_Y";
	public string Xbox_lowBeamHeadlightsKB = "Xbox_LB";
	public string Xbox_highBeamHeadlightsKB = "Xbox_RB";
	public string Xbox_indicatorKB = "Xbox_DPadHorizontal";
	public string Xbox_hazardIndicatorKB = "Xbox_DPadVertical";
	public string Xbox_shiftGearUp = "Xbox_RB";
	public string Xbox_shiftGearDown = "Xbox_LB";
//	public KeyCode Xbox_NGear = KeyCode.N;
	public string Xbox_boostKB = "Xbox_A";
//	public KeyCode Xbox_slowMotionKB = KeyCode.G;
	public string Xbox_changeCameraKB = "Xbox_Back";
//	public KeyCode Xbox_recordKB = KeyCode.R;
//	public KeyCode Xbox_playbackKB = KeyCode.P;
	public string Xbox_lookBackKB = "Xbox_ClickRight";
	public string Xbox_trailerAttachDetach = "Xbox_ClickLeft";

	// Main Controller Settings
	public bool useVR = false;
	public bool useAutomaticGear = true;
	public bool runEngineAtAwake = true;
	public bool autoReverse = true;
	public bool autoReset = true;
	public GameObject contactParticles;

	public Units units;
	public enum Units {KMH, MPH}

	// UI Dashboard Type
	public UIType uiType;
	public enum UIType{UI, NGUI, None}

	// Information telemetry about current vehicle
	public bool useTelemetry = false;

	// For mobile usement
	public enum MobileController{TouchScreen, Gyro, SteeringWheel, Joystick}
	public MobileController mobileController;

	// Mobile controller buttons and accelerometer sensitivity
	public float UIButtonSensitivity = 3f;
	public float UIButtonGravity = 5f;
	public float gyroSensitivity = 2f;

	// Used for using the lights more efficent and realistic
	public bool useLightsAsVertexLights = true;
	public bool useLightProjectorForLightingEffect = false;

	// Other stuff
	public bool setTagsAndLayers = false;
	public string RCCLayer;
	public string RCCTag;
	public bool tagAllChildrenGameobjects = false;

	public GameObject chassisJoint;
	public GameObject exhaustGas;
	public RCC_SkidmarksManager skidmarksManager;
	public GameObject projector;
	public LayerMask projectorIgnoreLayer;

	public GameObject headLights;
	public GameObject brakeLights;
	public GameObject reverseLights;
	public GameObject indicatorLights;
	public GameObject mirrors;

	public RCC_Camera RCCMainCamera;
	public GameObject hoodCamera;
	public GameObject cinematicCamera;
	public GameObject RCCCanvas;

	public bool dontUseAnyParticleEffects = false;
	public bool dontUseChassisJoint = false;
	public bool dontUseSkidmarks = false;

	// Sound FX
	public AudioClip[] gearShiftingClips;
	public AudioClip[] crashClips;
	public AudioClip reversingClip;
	public AudioClip windClip;
	public AudioClip brakeClip;
	public AudioClip indicatorClip;
	public AudioClip NOSClip;
	public AudioClip turboClip;
	public AudioClip[] blowoutClip;
	public AudioClip[] exhaustFlameClips;
	public bool useSharedAudioSources = true;

	[Range(0f, 1f)]public float maxGearShiftingSoundVolume = .25f;
	[Range(0f, 1f)]public float maxCrashSoundVolume = 1f;
	[Range(0f, 1f)]public float maxWindSoundVolume = .1f;
	[Range(0f, 1f)]public float maxBrakeSoundVolume = .1f;

	// Used for folding sections of RCC Settings
	public bool foldGeneralSettings = false;
	public bool foldBehaviorSettings = false;
	public bool foldControllerSettings = false;
	public bool foldUISettings = false;
	public bool foldWheelPhysics = false;
	public bool foldSFX = false;
	public bool foldOptimization = false;
	public bool foldTagsAndLayers = false;

}
