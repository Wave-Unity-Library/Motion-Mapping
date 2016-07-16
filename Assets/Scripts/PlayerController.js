#pragma strict

public var speed : float;
public var AccelerometerUpdateInterval : float = 1.0 / 60.0;
public var LowPassKernelWidthInSeconds : float = 1.0;
public var LowPassFilterFactor : float = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
private var movement : Vector3;
private var rb : Rigidbody;
private var lowPassValue : Vector3;
private var pastAccelerationMagnitude : float;
private var currentAccelerationMagnitude : float;
private var accelerationDifference : float; 

function Start() {
  // accessing rigid body component of player game object
  rb = GetComponent.<Rigidbody>();
  lowPassValue = Input.acceleration;
  pastAccelerationMagnitude = 9.9;
  movement = Vector3.zero;
}

function LowPassFilterAccelerometer() : Vector3 {
  // Smooths out noise from accelerometer data
  lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
  return lowPassValue;
}

function FixedUpdate () {
  currentAccelerationMagnitude = LowPassFilterAccelerometer().magnitude;
  accelerationDifference = Mathf.Abs(pastAccelerationMagnitude - currentAccelerationMagnitude);
  
  // movement of player in the direction of the camera
  if ( accelerationDifference > .0015 && .004 > accelerationDifference ) {
    movement = Camera.main.transform.forward;
  } 

  pastAccelerationMagnitude = Mathf.Abs(LowPassFilterAccelerometer().magnitude);

  // Maps the player's real-world steps into in-game movement
  rb.AddForce(movement * speed);
}
