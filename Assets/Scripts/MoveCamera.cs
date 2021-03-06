using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
  public Transform cameraPosition;

  Vector3 offset;


  // Start is called before the first frame update
  void Start()
  {
    //Calculate and store the offset value by getting the distance between the player's position and camera's position.
    offset = transform.position - cameraPosition.transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    transform.position = cameraPosition.position + offset;
  }
}
