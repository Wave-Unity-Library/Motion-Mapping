#pragma strict

var player : GameObject;

private var offset : Vector3;
private var origin : Quaternion = Quaternion.identity;

function Start () {
  // checking if device has a gyroscope
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
    // transform.rotation = Quaternion.identity;
  }
  Input.compass.enabled = true;
  offset = transform.position - player.transform.position;
}

function Update () {
  if (Input.touchCount > 0 || origin == Quaternion.identity) {
    origin = Input.gyro.attitude;
  }

  var eulerAngles : Vector3 = Input.gyro.attitude.eulerAngles;
    // var gyroQuaternion : Vector3 = Quaternion.Inverse(Input.gyro.attitude).eulerAngles;
    // transform.eulerAngles = new Vector3( -gyroQuaternion[0], -gyroQuaternion[1], gyroQuaternion[2]);
  // transform.rotation = Quaternion.Inverse(Input.gyro.attitude);
  // transform.rotation = Quaternion.Inverse(origin) * Input.gyro.attitude;
  // transform.forward = transform.forward * -1;
  // print(Input.gyro.attitude);
  // print(Input.gyro.attitude.eulerAngles);
  // print(Input.acceleration);
  // var roll : Quaternion = Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.z, Vector3.forward);
  // var target : Quaternion = Quaternion.identity;
  var pitch : Quaternion;
  var yaw : Quaternion;
  var roll : Quaternion;

  // if (eulerAngles.x > 45 || eulerAngles.x > 315) {
  //   roll = Quaternion.AngleAxis(0, Vector3.forward);
    pitch = Quaternion.AngleAxis(eulerAngles.y - 90, Vector3.left);
    yaw = Quaternion.AngleAxis(eulerAngles.x, Vector3.up);
  // } else if (eulerAngles.x <= 45) {
  //   roll = Quaternion.AngleAxis(0, Vector3.forward);
  //   pitch = Quaternion.AngleAxis(eulerAngles.y, Vector3.left);
  //   yaw = Quaternion.AngleAxis(eulerAngles.x, Vector3.up);
  // }

    var target : Quaternion = Quaternion.identity;
    target = yaw * target;
    target = target * pitch;
    // transform.localRotation = yaw * pitch;
  // transform.Rotate (pitch, Space.Self);
  // transform.Rotate (yaw, Space.World);
  // var roll : Quaternion = Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.z, Vector3.forward);
  transform.localRotation = Quaternion.Slerp(transform.rotation, target, .2);

  // transform.localRotation = pitch; 
}

function LateUpdate () {
  transform.position = player.transform.position + offset;
}

function OnGUI(){
     GUI.Label(Rect(0,0,Screen.width,Screen.height), Input.gyro.attitude.eulerAngles + '\n' 
      + Input.gyro.attitude + '\n' 
      + Input.acceleration + '\n'
      + Input.compass.magneticHeading);
 }