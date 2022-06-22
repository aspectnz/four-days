using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
  public float sensX;
  public float sensY;

  public Transform orientation;

  float xRotation;
  float yRotation;

  // Start is called before the first frame update
  void Start()
  {
    // Hide cursor and fix to center
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;


  }

  // Update is called once per frame
  void Update()
  {
    // Get mouse input
    float mouseX =
        Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
    float mouseY =
        Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

    yRotation += mouseX;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    // Rotate cam and orientation
    transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    orientation.rotation = Quaternion.Euler(0, yRotation, 0);
  }
}
