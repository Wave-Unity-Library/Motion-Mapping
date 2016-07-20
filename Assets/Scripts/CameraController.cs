using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

	public GameObject player;
	public float slerpValue = 0.2f;
	public int verticalOffsetAngle = -90;
	public int horizontalOffsetAngle = 0;

	Vector3 cameraOffset;
	Quaternion phoneOrientation;
	Quaternion correctedPhoneOrientation;
	Quaternion horizontalRotationCorrection;
	Quaternion verticalRotationCorrection;
	Quaternion inGameOrientation;

	public Transform target;
	public GameObject PlayerPrefab;
	GameObject prefabClone;
	Vector3 Origin = new Vector3 (0, 0, 0);
	Quaternion RotationOrigin = new Quaternion (0, 0, 0, 0);

	// Use this for initialization
	void Start () {
		// Checking if device has a gyroscope to enable
		if (SystemInfo.supportsGyroscope) {
			Input.gyro.enabled = true;
		} 

		// Used in LateUpdate function to set camera to follow player object
		//cameraOffset = transform.position - player.transform.position;
	}
	
	void Update () {
		// Retrieves gyroscopic information from phone
		phoneOrientation = Input.gyro.attitude;
		correctedPhoneOrientation = new Quaternion(phoneOrientation.x, phoneOrientation.y, -phoneOrientation.z, -phoneOrientation.w);
		verticalRotationCorrection = Quaternion.AngleAxis(verticalOffsetAngle, Vector3.left);
		horizontalRotationCorrection = Quaternion.AngleAxis(horizontalOffsetAngle, Vector3.up);
		inGameOrientation = horizontalRotationCorrection * verticalRotationCorrection * correctedPhoneOrientation;

		// Changes orientation of in-game camera to reflect orientation of phone
  		transform.rotation = Quaternion.Slerp(transform.rotation, inGameOrientation, slerpValue);
	}

	void LateUpdate () {
//		Vector3 targetCamPos = target.position + cameraOffset;
//		transform.position = Vector3.Lerp(transform.position, targetCamPos, 0.5f * Time.deltaTime);
		// Forces camera to follow player
		transform.position = player.transform.position + cameraOffset;
	}
		
	void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
		prefabClone = (GameObject) Network.Instantiate(PlayerPrefab, Origin, RotationOrigin, 0);
		Debug.Log (prefabClone);
//		Camera.main.GetComponent<CameraController>().player = LocalPlayer;
		player = prefabClone;
		target = prefabClone.transform;
		cameraOffset = transform.position - target.position;
		Debug.Log ("camera " + Camera.main.GetComponent<CameraController> ());
	}
}