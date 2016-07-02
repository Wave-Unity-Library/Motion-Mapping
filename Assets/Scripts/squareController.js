#pragma strict

private var origin : Quaternion = Quaternion.identity;

function Start () {
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
  }
}

function Update () {
  if (Input.touchCount > 0 || origin == Quaternion.identity) {
    origin = Input.gyro.attitude;
  }
  var gyroQuaternion : Quaternion = Quaternion.Inverse(Input.gyro.attitude);
  transform.rotation = gyroQuaternion;
}