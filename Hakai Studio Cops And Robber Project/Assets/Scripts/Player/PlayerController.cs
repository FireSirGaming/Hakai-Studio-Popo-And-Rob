using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEditor;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody rb_player;
    [SerializeField] private Transform platform;

    [Header("Enabled Mechanics")]
    [SerializeField] private bool freeze = false;
    [SerializeField] private bool enableGravity = true; public bool EnableGravity { set { enableGravity = value; } }
    [SerializeField] private bool enableMove = true; public bool EnableMove { set {  enableMove = value; } }
    [SerializeField] private bool enableCameraLook = true;
    [SerializeField] private bool enableSprint = true;
    [SerializeField] private bool enableAirAccel = false;
    [SerializeField] private bool enableJump = true; public bool EnableJump { set { enableJump = value; } }
    [SerializeField] private bool enableJumpTime = false;
    [SerializeField] private bool enableStepSlope = true;

    [Header("Movement Mechanics")]
    [SerializeField] private float gravity = 25f;
    private Vector3 temp;
    private Vector3 gravityVel;
    [SerializeField] private bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }
    [SerializeField] private float playerSpeed; // Overall move speed
    [SerializeField] private float diagonalSpeed = Mathf.Sqrt(50f); // [walkSpeed / sqrt(walkSpeed * walkSpeed x 2) * walkSpeed]
    [SerializeField] private float jumpForce = 15f;

    [Header("Step Slope Mechanics")]
    [SerializeField] private Transform stepRayUpper;
    [SerializeField] private Transform stepRayLower;
    [SerializeField] private float stepWidth = 1.2f;
    [SerializeField] private float stepHeight = 0.6f;
    [SerializeField] private float stepSmooth = 0.05f;

    [Header("Jump Mechanics")]
    [SerializeField] private Transform groundDectector; // *Required, "Base/Feet"
    [SerializeField] private LayerMask layerMask; // Standable Layer
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDelay = 0.2f;
    private float groundCooldown = 0f;
    private bool isJumping;
    [SerializeField] private float maxJumpTime = 0.2f;
    private float jumpTime;

    //public int maxJump; // Default = 1, Double = 2, etc.
    //private int jumpCount;

    [Header("Sprint Feature")]
    [SerializeField] private float sprintMultiplier = 6f;
    [SerializeField] private float walkSpeed = 5f;

    [Header("Air Acceleration Modifiers")]
    [SerializeField] private float defaultAccel = 1; // Default 1
    [SerializeField] private float lightAccel = 0.8f; // Proper strafe
    [SerializeField] private float heavyAccel = 0.3f; // Restricted strafe
    private float currentAccel = 1f;
    private bool breakLightAccel;

    [Header("Mouse Mechanics")]
    private Vector2 mouseInput = Vector2.zero;
    [SerializeField] private float clampUp = -70f;
    [SerializeField] private float clampDown = 30f;
    [SerializeField] private float mouseSensitivity = 1f;

    [Header("Teleport Features")]
    public float teleportDelay = 1f;
    public float teleportCooldown = 0f;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        CheckDeclare();
    }

    //void Update()
    //{
    //    if (!HasStateAuthority) return;
 
    //    if (freeze) return;

    //    CheckMoving();
    //    if (enableGravity) Gravity();
    //    if (enableMove) Movement();
    //    if (enableSprint) Sprint();
    //    if (enableJump) Jump();
    //    if (enableCameraLook) CameraLook();
    //    if (enableStepSlope) StepSlope();
    //}

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) { Debug.Log($"This {Runner.LocalPlayer} does not have authority!"); return; }

        if (freeze) return;

        CheckMoving();
        if (enableGravity) Gravity();
        if (enableMove) Movement();
        if (enableSprint) Sprint();
        if (enableJump) Jump();
        if (enableCameraLook) CameraLook();
        if (enableStepSlope) StepSlope();
    }

    private void Gravity()
    {
        // Ground Check
        if (groundCooldown <= 0f)
        {
            isGrounded = Physics.CheckSphere(groundDectector.position, 0.1f, layerMask);
        }
        else if (groundCooldown >= 0f)
        {
            groundCooldown -= Runner.DeltaTime;
        }

        // Reset variables on land
        if (isGrounded)
        {
            isJumping = false;
        }

        // Gravity
        if (!isGrounded)
        {
            isJumping = true;
            gravityVel += gravity * -player.up * Runner.DeltaTime;
        }
        else
        {
            gravityVel = Vector3.zero;//(gravity / 5) * -player.up;
        }

        Vector3 velocity = temp * playerSpeed * sprintMultiplier * currentAccel;

        rb_player.velocity = velocity + (gravityVel);
    }

    private void Movement()
    {
        // Air Accel Calculation
        if (enableAirAccel) AirAccel();

        // WASD Velocity
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        temp = Vector3.zero;
        if (inputZ > 0)
        {
            temp += player.forward;
        }
        if (inputZ < 0)
        {
            temp += -player.forward;
        }
        if (inputX > 0)
        {
            temp += player.right;
        }
        if (inputX < 0)
        {
            temp += -player.right;
        }

        if (inputX != 0 && inputZ != 0)
        {
            diagonalSpeed = (walkSpeed / Mathf.Sqrt(walkSpeed * walkSpeed * 2)) * walkSpeed;
            playerSpeed = diagonalSpeed;
        }
        else
        {
            playerSpeed = walkSpeed;
        }
    }

    private void StepSlope()
    {
        if (isMoving)
        {
            Vector3 moveDir = new Vector3(rb_player.velocity.x, 0f, rb_player.velocity.z).normalized;          

            RaycastHit hitLower;
            if (Physics.Raycast(stepRayLower.position, moveDir, out hitLower, 0.55f))
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(stepRayUpper.position, moveDir, out hitUpper, stepWidth))
                {
                    rb_player.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }

            Vector3 moveDir45 = moveDir + new Vector3(1.5f, 0f, 0f).normalized;

            RaycastHit hitLower45;
            if (Physics.Raycast(stepRayLower.position, moveDir45, out hitLower45, 0.6f))
            {
                RaycastHit hitUpper45;
                if (!Physics.Raycast(stepRayUpper.position, moveDir45, out hitUpper45, stepWidth + 0.1f))
                {
                    rb_player.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }

            Vector3 moveDirMinus45 = moveDir - new Vector3(1.5f, 0f, 0f).normalized;

            RaycastHit hitLowerMinus45;
            if (Physics.Raycast(stepRayLower.position, moveDirMinus45, out hitLowerMinus45, 0.6f))
            {
                RaycastHit hitUpperMinus45;
                if (!Physics.Raycast(stepRayUpper.position, moveDirMinus45, out hitUpperMinus45, stepWidth + 0.1f))
                {
                    rb_player.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }
        }
    }

    private void CheckMoving() 
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        if (inputZ > 0 || inputZ < 0 || inputX > 0 || inputX < 0 || isJumping)
        {
            isMoving = true;
        }

        else
        {
            isMoving = false;
        }
    }

    private void CameraLook()
    {
        // Camera
        mouseInput.x = Input.GetAxis("Mouse X") * mouseSensitivity;
        if (mouseInput.y < clampUp) mouseInput.y = clampUp;
        if (mouseInput.y > clampDown) mouseInput.y = clampDown;
        mouseInput.y += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        player.Rotate(0f, mouseInput.x, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(mouseInput.y, 0f, 0f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Sprint()
    {
        // Sprint
        if (enableSprint)
        {
            if (isGrounded && Input.GetKey(KeyCode.LeftShift))
            {
                sprintMultiplier = 1.3f;
            }
            else
            {
                sprintMultiplier = 1f;
            }
        }
    }

    private void Jump()
    {
        // Jump
        if (enableJump)
        {
            // Jump Time
            if (enableJumpTime)
            {
                if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                {
                    isJumping = true;
                    jumpTime = 0f;
                }
                if (isJumping)
                {
                    rb_player.AddForce(rb_player.transform.up * jumpForce * Runner.DeltaTime, ForceMode.Acceleration);
                    jumpTime += Runner.DeltaTime;
                }
                if (Input.GetKeyUp(KeyCode.Space) || jumpTime >= maxJumpTime)
                {
                    isJumping = false;
                }
            }

            else if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                groundCooldown = groundDelay;
                gravityVel += player.up * jumpForce * Runner.DeltaTime;
            }
        }
    }

    private void AirAccel()
    {
        if (Input.GetAxis("Vertical") != 0) breakLightAccel = true; // Whatever condition - e.g. If forward/back input, restricted accel until reset

        if (!isGrounded)
        {
            if (breakLightAccel)
            {
                currentAccel = heavyAccel;
            }
            else
            {
                currentAccel = lightAccel;
            }
        }
        else
        {
            currentAccel = defaultAccel;
            breakLightAccel = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            platform = collision.transform;
            gravityVel = Vector3.zero;
        }
    }

    private void Initialize()
    {
        player = gameObject.transform;
        rb_player = gameObject.GetComponent<Rigidbody>();
        stepRayUpper.localPosition = new Vector3(stepRayUpper.position.x, stepHeight, stepRayUpper.position.z);
    }

    private void CheckDeclare()
    {
        if (rb_player == null)
        {
            rb_player = GetComponent<Rigidbody>();
            Debug.Log("Character controller not assigned. Defaulted to player, errors may persist.");
        }

        if (groundDectector == null)
        {
            groundDectector = gameObject.transform;
            Debug.Log("Ground detector not assigned. Defaulted to player, errors may persist.");
        }

        if (enableJumpTime) enableJump = true;
        if (!enableAirAccel) currentAccel = defaultAccel;

        if (enableSprint)
        {
            enableMove = true;
            sprintMultiplier = 1.3f;
        }

        if (enableMove)
        {
            playerSpeed = walkSpeed;
            diagonalSpeed = (walkSpeed / Mathf.Sqrt(walkSpeed * walkSpeed * 2)) * walkSpeed;
        }
    }
}

//sfx needed
//walking
//running
//jumping
//crouching
//sliding
//gunshot 
//reloading
//melee attack
//knock down 
