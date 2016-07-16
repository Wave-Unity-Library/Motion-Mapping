#pragma strict

public var speed : float;
public var accelerometerUpdateInterval : float = 1.0 / 60.0;
public var lowPassKernelWidthInSeconds : float = 1.0;
public var lowPassFilterFactor : float = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
private var movement : Vector3;
private var rb : Rigidbody;
private var lowPassValue : Vector3;
private var pastAccelerationMagnitude : float;
private var currentAccelerationMagnitude : float;
private var accelerationDifference : float; 

function Start() {
  rb = GetComponent.<Rigidbody>();
  lowPassValue = Input.acceleration;
  pastAccelerationMagnitude = 9.9;
}

function LowPassFilterAccelerometer() : Vector3 {
  // Smooths out noise from accelerometer data
  lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, lowPassFilterFactor);
  return lowPassValue;
}

function FixedUpdate () {
  movement = Vector3.zero;
  currentAccelerationMagnitude = LowPassFilterAccelerometer().magnitude;
  accelerationDifference = Mathf.Abs(pastAccelerationMagnitude - currentAccelerationMagnitude);
  
  // Moves player in the direction of the camera
  if ( accelerationDifference > .0015 && .004 > accelerationDifference ) {
    movement = Camera.main.transform.forward;
  } 

  pastAccelerationMagnitude = Mathf.Abs(currentAccelerationMagnitude);

  // Maps the player's real-world steps into in-game movement
  rb.AddForce(movement * speed);
}
