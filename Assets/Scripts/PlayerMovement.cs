using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

  [Header("Crouching")]
  public float crouchSpeed;
  public float crouchYScale;
  private float startYScale;

  [Header("Keybinds")]
  public KeyCode jumpKey = KeyCode.Space;
  public KeyCode sprintKey = KeyCode.C;
  public KeyCode crouchKey = KeyCode.LeftShift;


  [Header("Ground Check")]
  public float playerHeight;
  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask whatIsGround;
  bool grounded;

  [Header("Slope Handling")]
  public float maxSlopeAngle;
  private RaycastHit slopeHit;

  public Transform orientation;

  float horizontalInput;
  float verticalInput;

  Vector3 moveDirection;

  Rigidbody rb;

  public MovementState state;
  public enum MovementState
  {
    walking, sprinting, crouching, air
  }

  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;

    ResetJump();

    startYScale = transform.localScale.y;
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

    // Start crouch
    if (Input.GetKeyDown(crouchKey))
    {
      transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
      rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    // Stop crouch
    if (Input.GetKeyUp(crouchKey))
    {

      transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
    }
  }

  void StateHandler()
  {
    // Mode - Crouching
    if (Input.GetKey(crouchKey))
    {
      state = MovementState.crouching;
      moveSpeed = crouchSpeed;
    }

    // Mode - Sprinting
    else if (grounded && Input.GetKey(sprintKey))
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

    // On Slope
    if (OnSlope())
    {
      Debug.Log("On Slope!");
      rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
    }

    // On ground
    if (grounded)
      rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

    // In air
    else if (!grounded)
      rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

    // Turn gravity off while on slope
    // rb.useGravity = !OnSlope();
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
    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

    // rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
  }

  void ResetJump()
  {
    readyToJump = true;
  }

  private bool OnSlope()
  {
    if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
    {
      float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
      return angle < maxSlopeAngle && angle != 0;
    }

    return false;
  }

  private Vector3 GetSlopeMoveDirection()
  {
    return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
  }
}
