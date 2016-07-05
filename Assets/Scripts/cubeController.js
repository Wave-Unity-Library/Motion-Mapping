#pragma strict

private var phoneOrientation : Quaternion;
private var orientationCorrection : Quaternion;
private var inGameOrientation : Quaternion;

function Start () {
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
  }
}

function Update () {
  phoneOrientation = Input.gyro.attitude;
  orientationCorrection = Quaternion.AngleAxis(-90, Vector3.left);
  inGameOrientation = new Quaternion(phoneOrientation.x, phoneOrientation.y, -phoneOrientation.z, -phoneOrientation.w);

  transform.rotation = Quaternion.Slerp (transform.rotation, orientationCorrection * inGameOrientation, .2);
}