#pragma strict

public var player : GameObject;
public var slerpValue : float = .2;
public var verticalOffsetAngle : int = -90;
public var horizontalOffsetAngle : int = 0;
private var cameraOffset : Vector3;
private var phoneOrientation : Quaternion;
private var correctedPhoneOrientation : Quaternion;
private var horizontalRotationCorrection : Quaternion;
private var verticalRotationCorrection : Quaternion;
private var inGameOrientation : Quaternion;

function Start() {
  // Checks if device has a gyroscope to enable
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
  } 

  // Exists in LateUpdate function to force camera to follow player object
  cameraOffset = transform.position - player.transform.position;
}

function Update() {
  // Retrieves gyroscopic information from phone
  phoneOrientation = Input.gyro.attitude;
  correctedPhoneOrientation = new Quaternion(phoneOrientation.x, phoneOrientation.y, -phoneOrientation.z, -phoneOrientation.w);
  verticalRotationCorrection = Quaternion.AngleAxis(verticalOffsetAngle, Vector3.left);
  horizontalRotationCorrection = Quaternion.AngleAxis(horizontalOffsetAngle, Vector3.up);
  inGameOrientation = horizontalRotationCorrection * verticalRotationCorrection * correctedPhoneOrientation;

  // Changes orientation of in-game camera to reflect orientation of phone
  transform.rotation = Quaternion.Slerp(transform.rotation, inGameOrientation, slerpValue);
}

function LateUpdate () {
  // Forces camera to follow player
  transform.position = player.transform.position + cameraOffset;
}