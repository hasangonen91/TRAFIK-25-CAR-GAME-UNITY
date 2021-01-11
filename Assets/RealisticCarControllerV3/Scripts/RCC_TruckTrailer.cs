
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Truck trailer has additional wheelcolliders. This script handles center of mass of the trailer, wheelcolliders, and antiroll.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/RCC Truck Trailer")]
[RequireComponent (typeof(Rigidbody))]
public class RCC_TruckTrailer : MonoBehaviour {

	private RCC_CarControllerV3 carController;
	private Rigidbody rigid;
	private ConfigurableJoint joint;

	public Transform COM;
	private bool isSleeping = false;

	[System.Serializable]
	public class TrailerWheel{

		public WheelCollider wheelCollider;
		public Transform wheelModel;

		public float compression;
		public float wheelRotation = 0f;

		public void AddTorque(float torque){

			wheelCollider.motorTorque = torque;

		}

	}

	//Extra Wheels.
	public TrailerWheel[] trailerWheels;

	private WheelCollider[] allWheelColliders;
	private List<WheelCollider> leftWheelColliders = new List<WheelCollider>();
	private List<WheelCollider> rightWheelColliders = new List<WheelCollider>();

	public float antiRoll = 20000f;

	public bool attached = false;

	public class JointRestrictions{

		public ConfigurableJointMotion motionX;
		public ConfigurableJointMotion motionY;
		public ConfigurableJointMotion motionZ;

		public ConfigurableJointMotion angularMotionX;
		public ConfigurableJointMotion angularMotionY;
		public ConfigurableJointMotion angularMotionZ;

		public void Get(ConfigurableJoint configurableJoint){

			motionX = configurableJoint.xMotion;
			motionY = configurableJoint.yMotion;
			motionZ = configurableJoint.zMotion;

			angularMotionX = configurableJoint.angularXMotion;
			angularMotionY = configurableJoint.angularYMotion;
			angularMotionZ = configurableJoint.angularZMotion;

		}

		public void Set(ConfigurableJoint configurableJoint){

			configurableJoint.xMotion = motionX;
			configurableJoint.yMotion = motionY;
			configurableJoint.zMotion = motionZ;

			configurableJoint.angularXMotion = angularMotionX;
			configurableJoint.angularYMotion = angularMotionY;
			configurableJoint.angularZMotion = angularMotionZ;

		}

		public void Reset(ConfigurableJoint configurableJoint){

			configurableJoint.xMotion = ConfigurableJointMotion.Free;
			configurableJoint.yMotion = ConfigurableJointMotion.Free;
			configurableJoint.zMotion = ConfigurableJointMotion.Free;

			configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
			configurableJoint.angularZMotion = ConfigurableJointMotion.Free;

		}

	}

	public JointRestrictions jointRestrictions = new JointRestrictions();

	void Start () {

		rigid = GetComponent<Rigidbody>();
		joint = GetComponentInParent<ConfigurableJoint> ();
		jointRestrictions.Get (joint);

		rigid.interpolation = RigidbodyInterpolation.None;
		rigid.interpolation = RigidbodyInterpolation.Interpolate;
		joint.configuredInWorldSpace = true;

		allWheelColliders = GetComponentsInChildren<WheelCollider> ();

		for (int i = 0; i < allWheelColliders.Length; i++) {

			if(allWheelColliders[i].transform.localPosition.x < 0f)
				leftWheelColliders.Add(allWheelColliders[i]);
			else
				rightWheelColliders.Add(allWheelColliders[i]);

		}

		if (joint.connectedBody) {
			
			AttachTrailer (joint.connectedBody.gameObject.GetComponent<RCC_CarControllerV3> ());

		} else {
			
			carController = null;
			joint.connectedBody = null;
			jointRestrictions.Reset (joint);

		}

	}

	void FixedUpdate(){

		attached = joint.connectedBody;
		
		rigid.centerOfMass = transform.InverseTransformPoint(COM.transform.position);

		if (!carController)
			return;

		AntiRollBars();

		for (int i = 0; i < trailerWheels.Length; i++)
			trailerWheels [i].AddTorque (carController._gasInput * (attached ? 1f : 0f));

	}

	void Update(){

		if(rigid.velocity.magnitude < .01f && Mathf.Abs(rigid.angularVelocity.magnitude) < .01f)
			isSleeping = true;
		else
			isSleeping = false;

		WheelAlign ();

	}

	// Aligning wheel model position and rotation.
	public void WheelAlign (){
		
		if (isSleeping)
			return;

		for (int i = 0; i < trailerWheels.Length; i++) {

			// Return if no wheel model selected.
			if(!trailerWheels[i].wheelModel){

				Debug.LogError(transform.name + " wheel of the " + transform.name + " is missing wheel model. This wheel is disabled");
				enabled = false;
				return;

			}

			WheelHit GroundHit;
			bool grounded = trailerWheels[i].wheelCollider.GetGroundHit(out GroundHit );

			float newCompression = trailerWheels[i].compression;

			if (grounded)
				newCompression = 1f - ((Vector3.Dot(trailerWheels[i].wheelCollider.transform.position - GroundHit.point, trailerWheels[i].wheelCollider.transform.up) - (trailerWheels[i].wheelCollider.radius * trailerWheels[i].wheelCollider.transform.lossyScale.y)) / trailerWheels[i].wheelCollider.suspensionDistance);
			else
				newCompression = trailerWheels[i].wheelCollider.suspensionDistance;

			trailerWheels[i].compression = Mathf.Lerp (trailerWheels[i].compression, newCompression, Time.deltaTime * 50f);

			// Set the position of the wheel model.
			trailerWheels[i].wheelModel.position = trailerWheels[i].wheelCollider.transform.position;
			trailerWheels[i].wheelModel.position += (trailerWheels[i].wheelCollider.transform.up * (trailerWheels[i].compression - 1.0f) * trailerWheels[i].wheelCollider.suspensionDistance);

			// X axis rotation of the wheel.
			trailerWheels[i].wheelRotation += trailerWheels[i].wheelCollider.rpm * 6f * Time.deltaTime;
			trailerWheels[i].wheelModel.rotation = trailerWheels[i].wheelCollider.transform.rotation * Quaternion.Euler(trailerWheels[i].wheelRotation, trailerWheels[i].wheelCollider.steerAngle, trailerWheels[i].wheelCollider.transform.rotation.z);

			// Gizmos for wheel forces and slips.
			float extension = (-trailerWheels[i].wheelCollider.transform.InverseTransformPoint(GroundHit.point).y - (trailerWheels[i].wheelCollider.radius * trailerWheels[i].wheelCollider.transform.lossyScale.y)) / trailerWheels[i].wheelCollider.suspensionDistance;

			Debug.DrawLine(GroundHit.point, GroundHit.point + trailerWheels[i].wheelCollider.transform.up * (GroundHit.force / rigid.mass), extension <= 0.0 ? Color.magenta : Color.white);
			Debug.DrawLine(GroundHit.point, GroundHit.point - trailerWheels[i].wheelCollider.transform.forward * GroundHit.forwardSlip * 2f, Color.green);
			Debug.DrawLine(GroundHit.point, GroundHit.point - trailerWheels[i].wheelCollider.transform.right * GroundHit.sidewaysSlip * 2f, Color.red);

		}

	}

	public void DetachTrailer(){

		carController = null;
		joint.connectedBody = null;
		jointRestrictions.Reset (joint);

		if (RCC_SceneManager.Instance.activePlayerCamera)
			StartCoroutine(RCC_SceneManager.Instance.activePlayerCamera.AutoFocus ());

	}

	public void AttachTrailer(RCC_CarControllerV3 vehicle){

		carController = vehicle;

		antiRoll = vehicle.antiRollRearHorizontal;

		joint.connectedBody = vehicle.rigid;
		jointRestrictions.Set (joint);

		vehicle.attachedTrailer = this;

		if (RCC_SceneManager.Instance.activePlayerCamera)
			StartCoroutine(RCC_SceneManager.Instance.activePlayerCamera.AutoFocus (transform, carController.transform));

	}

	public void AntiRollBars (){

		for (int i = 0; i < leftWheelColliders.Count; i++) {

			WheelHit FrontWheelHit;

			float travelFL = 1.0f;
			float travelFR = 1.0f;

			bool groundedFL= leftWheelColliders[i].GetGroundHit(out FrontWheelHit);

			if (groundedFL)
				travelFL = (-leftWheelColliders[i].transform.InverseTransformPoint(FrontWheelHit.point).y - leftWheelColliders[i].radius) / leftWheelColliders[i].suspensionDistance;

			bool groundedFR= rightWheelColliders[i].GetGroundHit(out FrontWheelHit);

			if (groundedFR)
				travelFR = (-rightWheelColliders[i].transform.InverseTransformPoint(FrontWheelHit.point).y - rightWheelColliders[i].radius) / rightWheelColliders[i].suspensionDistance;

			float antiRollForceFrontHorizontal= (travelFL - travelFR) * antiRoll;

			if (groundedFL)
				rigid.AddForceAtPosition(leftWheelColliders[i].transform.up * -antiRollForceFrontHorizontal, leftWheelColliders[i].transform.position); 
			if (groundedFR)
				rigid.AddForceAtPosition(rightWheelColliders[i].transform.up * antiRollForceFrontHorizontal, rightWheelColliders[i].transform.position); 

			WheelHit RearWheelHit;

			float travelRL = 1.0f;
			float travelRR = 1.0f;

			bool groundedRL= leftWheelColliders[i].GetGroundHit(out RearWheelHit);

			if (groundedRL)
				travelRL = (-leftWheelColliders[i].transform.InverseTransformPoint(RearWheelHit.point).y - leftWheelColliders[i].radius) / leftWheelColliders[i].suspensionDistance;

			bool groundedRR= rightWheelColliders[i].GetGroundHit(out RearWheelHit);

			if (groundedRR)
				travelRR = (-rightWheelColliders[i].transform.InverseTransformPoint(RearWheelHit.point).y - rightWheelColliders[i].radius) / rightWheelColliders[i].suspensionDistance;

			float antiRollForceRearHorizontal= (travelRL - travelRR) * antiRoll;

			if (groundedRL)
				rigid.AddForceAtPosition(leftWheelColliders[i].transform.up * -antiRollForceRearHorizontal, leftWheelColliders[i].transform.position); 
			if (groundedRR)
				rigid.AddForceAtPosition(rightWheelColliders[i].transform.up * antiRollForceRearHorizontal, rightWheelColliders[i].transform.position);

		}

	}

	void OnTriggerEnter(Collider col){

//		RCC_TrailerAttachPoint attacher = col.gameObject.GetComponent<RCC_TrailerAttachPoint> ();
//
//		if (!attacher)
//			return;
//
//		RCC_CarControllerV3 vehicle = attacher.gameObject.GetComponentInParent<RCC_CarControllerV3> ();
//
//		if (!vehicle || !attacher)
//			return;
//		
//		AttachTrailer (vehicle, attacher);

	}

}
