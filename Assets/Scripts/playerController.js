#pragma strict

var speed : float;
var countText : UnityEngine.UI.Text;

private var rb : Rigidbody;
private var count: int;
private var distance: int;

// low pass filter variables
var AccelerometerUpdateInterval : float = 1.0 / 60.0;
var LowPassKernelWidthInSeconds : float = 1.0;
private var LowPassFilterFactor : float = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
private var lowPassValue : Vector3 = Vector3.zero;

function Start() {
  // accessing rigid body component of player game object
  rb = GetComponent.<Rigidbody>();

  lowPassValue = Input.acceleration;
  
  SetCountText();
}

function LowPassFilterAccelerometer() : Vector3 {
    lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
    return lowPassValue;
}

function FixedUpdate () {
  var moveHorizontal : float = Input.GetAxis ('Horizontal');
  var moveVertical : float = Input.GetAxis ('Vertical');
  var movement : Vector3 = Vector3.zero;

  print(LowPassFilterAccelerometer().magnitude);

  // movement is in the direction of the camera
  if (Input.GetKeyDown(KeyCode.UpArrow)) {
    movement = Camera.main.transform.forward;
  } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
    movement = Camera.main.transform.forward * -1;
  } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
    movement = Camera.main.transform.right;
  } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
    movement = Camera.main.transform.right * -1;
  }

  // movement vector applied to player gameObject
  rb.AddForce(movement * speed);
  SetCountText();
}

// function Update() {
//   var moveHorizontal : float = Input.GetAxis ('Horizontal');
//   var moveVertical : float = Input.GetAxis ('Vertical');

//   var movement : Vector3 = new Vector3 (0, 0, 0);

//   if (Input.GetKeyDown(KeyCode.UpArrow)) {
//     movement = Camera.main.transform.forward;
//   } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
//     movement = Camera.main.transform.forward * -1;
//   }

//   transform.position = transform.position + movement;
// }

function OnTriggerEnter(other : Collider) {
    if (other.gameObject.CompareTag('Pick Up')) {
      other.gameObject.SetActive (false);
    }
}

function SetCountText() {
  // countText.text = rb.velocity.ToString('F3');
  countText.text = Input.gyro.attitude.eulerAngles.ToString('F3');
}
