#pragma strict

var player : GameObject;

private var offset : Vector3;
private var phoneOrientation : Quaternion;
private var orientationCorrection : Quaternion;
private var inGameOrientation : Quaternion;

function Start () {
  // Checking if device has a gyroscope to enable
  if (SystemInfo.supportsGyroscope) {
    Input.gyro.enabled = true;
  } 

  // Used in LateUpdate function to set camera to follow player object
  offset = transform.position - player.transform.position;
}

// Changes rotation of camera to reflect orientation of phone
function Update () {
  phoneOrientation = Input.gyro.attitude;
  // orientationCorrection = Quaternion.AngleAxis(-90, Vector3.left);
  inGameOrientation = new Quaternion(phoneOrientation.x, phoneOrientation.y, -phoneOrientation.z, -phoneOrientation.w);

  transform.rotation = Quaternion.Slerp (transform.rotation, orientationCorrection * inGameOrientation, .2);
}

// Forces camera to follow player
function LateUpdate () {
  transform.position = player.transform.position + offset;
}

// Displays sensor information on the player screen
function OnGUI(){
    GUI.Label(Rect(0,0,Screen.width,Screen.height), Input.gyro.attitude.eulerAngles + '\n' 
    + Input.gyro.attitude + '\n' 
    + Input.acceleration + '\n');
 }