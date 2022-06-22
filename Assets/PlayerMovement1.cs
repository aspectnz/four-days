using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{

  [Header("Movement")]
  float moveSpeed;
  public float walkSpeed;
  public float sprintSpeed;
  public float groundDrag;

  [Header("Jumping")]
  public float jumpForce;
  public float jumpCooldown;
  public float airMultiplier;
  bool readyToJump;

  [Header("Keybinds")]
  public KeyCode jumpKey = KeyCode.Space;
  public KeyCode sprintKey = KeyCode.LeftShift;


  [Header("Ground Check")]
  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask whatIsGround;
  bool grounded;

  public Transform orientation;

  float horizontalInput;
  float verticalInput;

  Vector3 moveDirection;

  Rigidbody rb;

  MovementState state;
  public enum MovementState
  {
    walking, sprinting, air
  }

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;

    ResetJump();
  }

  // Update is called once per frame
  void Update()
  {
    // Ground check 
    grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

    MyInput();
    SpeedControl();
    StateHandler();

    // Handle drag
    if (grounded)
      rb.drag = groundDrag;
    else rb.drag = 0;
  }

  void FixedUpdate()
  {
    MovePlayer();
  }

  void MyInput()
  {
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");

    // When to jump
    if (Input.GetKey(jumpKey) && readyToJump && grounded)
    {
      readyToJump = false;

      Jump();

      Invoke(nameof(ResetJump), jumpCooldown);
    }
  }

  void StateHandler()
  {
    // Mode - Sprinting
    if (grounded && Input.GetKey(sprintKey))
    {
      state = MovementState.sprinting;
      moveSpeed = sprintSpeed;
    }

    // Mode - Walking
    else if (grounded)
    {
      state = MovementState.walking;
      moveSpeed = walkSpeed;
    }

    // Mode - air
    else
    {
      state = MovementState.air;
    }
  }

  void MovePlayer()
  {
    // Calculate movement direction
    moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

    // On ground
    if (grounded)
      rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

    // In air
    else if (!grounded)
      rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
  }

  void SpeedControl()
  {
    Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    // Limit velocity if needed
    if (flatVel.magnitude > moveSpeed)
    {
      Vector3 limitedVel = flatVel.normalized * moveSpeed;
      rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

    }
  }

  void Jump()
  {
    // Reset y velocity
    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
  }

  void ResetJump()
  {
    readyToJump = true;
  }
}
