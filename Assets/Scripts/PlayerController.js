#pragma strict

public class playerController extends Networking.NetworkBehaviour {
	var speed : float;

	private var rb : Rigidbody;
	private var distance : int;

	// Variables for used in the LowPassFilter function
	var AccelerometerUpdateInterval : float = 1.0 / 60.0;
	var LowPassKernelWidthInSeconds : float = 1.0;
	var LowPassFilterFactor : float = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
	private var lowPassValue : Vector3 = Vector3.zero;
	private var temp : float = 9.9;

	function Start() {
	  // accessing rigid body component of player game object
	  rb = GetComponent.<Rigidbody>();
	  lowPassValue = Input.acceleration;
	}

	// Smooths out noise from accelerometer data
	function LowPassFilterAccelerometer() : Vector3 {
	    lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
	    return lowPassValue;
	}

	// Maps the player's real-world steps into in-game movement
	function FixedUpdate () {
	  if (!isLocalPlayer) return;

	  var movement : Vector3 = Vector3.zero;
	  var accelerationDifference : float = Mathf.Abs(temp - LowPassFilterAccelerometer().magnitude);
	  
	  // movement of player in the direction of the camera
	  if ( accelerationDifference > .0015 && .004 > accelerationDifference ) {
	    movement = Camera.main.transform.forward;
	  } 
	  // else if (Input.GetKeyDown(KeyCode.DownArrow)) {
	  //   movement = Camera.main.transform.forward * -1;
	  // } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
	  //   movement = Camera.main.transform.right;
	  // } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
	  //   movement = Camera.main.transform.right * -1;
	  // }

	  temp = Mathf.Abs(LowPassFilterAccelerometer().magnitude);

	  // Movement vector applied to player gameObject
	  rb.AddForce(movement * speed);
	}

	// Handles interaction of player and orange cubes
	function OnTriggerEnter(other : Collider) {
	    if (other.gameObject.CompareTag('Pick Up')) {
	      other.gameObject.SetActive (false);
	    }
	}
}