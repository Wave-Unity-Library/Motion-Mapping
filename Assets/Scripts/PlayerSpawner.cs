using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkBehaviour {

//	public Transform target;
//	Vector3 offset;
//	public GameObject PlayerPrefab;
//	public GameObject prefabClone;
//	Vector3 Origin = new Vector3 (0, 0, 0);
//	Quaternion RotationOrigin = new Quaternion (0, 0, 0, 0);
//
//	void OnServerInitialized() {
//		Debug.Log("Server initialized and ready");
//		prefabClone = (GameObject) Network.Instantiate(PlayerPrefab, Origin, RotationOrigin, 0);
////		Camera.main.GetComponent<CameraController>().player = LocalPlayer;
//		target = LocalPlayer.transform;
//		offset = transform.position - target.position;
//		Debug.Log ("local " + LocalPlayer);
//		Debug.Log ("camera " + Camera.main.GetComponent<CameraController> ());
//	}
//
//	void LateUpdate() {
//		Vector3 targetCamPos = target.position + offset;
//		transform.position = Vector3.Lerp(transform.position, targetCamPos, 0.5f * Time.deltaTime);
//	}
}
