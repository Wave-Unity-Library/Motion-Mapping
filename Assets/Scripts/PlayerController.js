#pragma strict

public var speed : float;
public var AccelerometerUpdateInterval : float = 1.0 / 60.0;
public var LowPassKernelWidthInSeconds : float = 1.0;
public var LowPassFilterFactor : float = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
private var rb : Rigidbody;
private var lowPassValue : Vector3;
private var temp : float;

function Start() {
  // accessing rigid body component of player game object
  rb = GetComponent.<Rigidbody>();
  lowPassValue = Input.acceleration;
  temp = 9.9;
}

function LowPassFilterAccelerometer() : Vector3 {
  // Smooths out noise from accelerometer data
  lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
  return lowPassValue;
}

function FixedUpdate () {
  var movement : Vector3 = Vector3.zero;
  var accelerationDifference : float = Mathf.Abs(temp - LowPassFilterAccelerometer().magnitude);
  
  // movement of player in the direction of the camera
  if ( accelerationDifference > .0015 && .004 > accelerationDifference ) {
    movement = Camera.main.transform.forward;
  } 

  temp = Mathf.Abs(LowPassFilterAccelerometer().magnitude);

  // Maps the player's real-world steps into in-game movement
  rb.AddForce(movement * speed);
}
