#pragma strict

private var origin : Quaternion = Quaternion.identity;

function Start () {
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
  }
}

function ConvertRotation(q : Quaternion) {

}

function Update () {
  if (Input.touchCount > 0 || origin == Quaternion.identity) {
    origin = Input.gyro.attitude;
  }
  var gyroQuaternion : Quaternion = Quaternion.Inverse(Input.gyro.attitude);
  // var reversedData : Vector3 = new Vector3 (Input.gyro.attitude.eulerAngles.x, Input.gyro.attitude.eulerAngles.y, Input.gyro.attitude.eulerAngles.z);
  // print (reversedData);
  // print (Quaternion.Inverse(Input.gyro.attitude).eulerAngles);
  // print (Input.gyro.attitude.eulerAngles);
  // var multiplier : Quaternion = Quaternion(0, 0, .7071, -.7071);
  // print (gyroQuaternion);
  // print (multiplier);
  // print (multiplier * gyroQuaternion);
  // transform.eulerAngles = new Vector3( -gyroQuaternion[0], -gyroQuaternion[1], gyroQuaternion[2]);
  // transform.eulerAngles = reversedData;
  // transform.rotation = Quaternion.Euler(reversedData);
  // gyroQuaternion *= multiplier;
  // print(gyroQuaternion);
  // print(Input.gyro.attitude.eulerAngles.y);
  // print(Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.y, Vector3.up));
  
  var yaw : Quaternion = Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.y, Vector3.left);
  // var pitch : Quaternion = Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.x, Vector3.up);
  // var roll : Quaternion = Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.z, Vector3.forward);
  transform.rotation = Quaternion.Slerp(transform.rotation, yaw, .3);

  // transform.rotation = gyroQuaternion;
}