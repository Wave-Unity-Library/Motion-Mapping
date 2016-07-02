#pragma strict

function Update () {
  transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
}