using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{

  [Header("Movement")]
  public float movespeed;

  public Transform orientation;

  float horizontalInput;
  float verticalInput;

  Vector3 moveDirection;

  Rigidbody rb;

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;

  }

  // Update is called once per frame
  void Update()
  {
    MyInput();
  }

  void FixedUpdate()
  {
    MovePlayer();
  }

  void MyInput()
  {
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");
  }

  void MovePlayer()
  {
    // Calculate movement direction
    moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Force);
  }


}
