#pragma strict

var player : GameObject;

private var offset : Vector3;
private var origin : Quaternion = Quaternion.identity;
private var pitch : Quaternion;
private var yaw : Quaternion;
private var target : Quaternion = Quaternion.identity;

function Start () {
  // Checking if device has a gyroscope to enable
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
  } 

  // Used in LateUpdate function to set camera to follow player object
  offset = transform.position - player.transform.position;
}

// Changes rotation of camera to reflect orientation of a phone
function Update () {
  if (Input.touchCount > 0 || origin == Quaternion.identity) {
    origin = Input.gyro.attitude;
  }

  var eulerAngles : Vector3 = Input.gyro.attitude.eulerAngles;

  pitch = Quaternion.AngleAxis(eulerAngles.y - 90, Vector3.left);
  yaw = Quaternion.AngleAxis(eulerAngles.x, Vector3.up);
  transform.localRotation = Quaternion.Slerp(Quaternion.identity * transform.rotation, yaw * pitch, .2);
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

  // if (eulerAngles.x > 45 || eulerAngles.x > 315) {
  //   roll = Quaternion.AngleAxis(0, Vector3.forward);
  // } else if (eulerAngles.x <= 45) {
  //   roll = Quaternion.AngleAxis(0, Vector3.forward);
  //   pitch = Quaternion.AngleAxis(eulerAngles.y, Vector3.left);
  //   yaw = Quaternion.AngleAxis(eulerAngles.x, Vector3.up);
  // }
    // target = yaw * target;
    // target = target * pitch;
    // transform.localRotation = yaw * pitch;
  // transform.Rotate (pitch, Space.Self);
  // transform.Rotate (yaw, Space.World);
  // var roll : Quaternion = Quaternion.AngleAxis(Input.gyro.attitude.eulerAngles.z, Vector3.forward);
  // transform.localRotation = pitch; 
}

// Forces camera to follow player
function LateUpdate () {
  transform.position = player.transform.position + offset;
}

// Displays sensor information on the player screen
function OnGUI(){
    GUI.Label(Rect(0,0,Screen.width,Screen.height), Input.gyro.attitude.eulerAngles + '\n' 
    + Input.gyro.attitude + '\n' 
    + Input.acceleration + '\n'
    + Input.compass.magneticHeading);
 }