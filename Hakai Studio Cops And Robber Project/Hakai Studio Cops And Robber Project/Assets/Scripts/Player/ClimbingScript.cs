using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingScript : MonoBehaviour
{
    [SerializeField] private bool canClimb;
    [SerializeField] private bool isClimbing;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private float climbForce;
    [SerializeField] private float checkClimbMultiple;
    [SerializeField] private float climbLengthMultiple;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private Rigidbody rb;

    private Ray wallRay;
    private Ray forwardRay;
    private Ray leftRay;
    private Ray rightRay;
    private Ray upRay;
    private Ray downRay;

    private RaycastHit hit;

    [SerializeField] private Transform topPosition;
    [SerializeField] private Transform bottomPosition;
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform rightPosition;

    private Vector3 topDir;
    private Vector3 bottomDir;
    private Vector3 leftDir;
    private Vector3 rightDir;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canClimb && !isClimbing) CheckWall();
        if (isClimbing) { CheckWallSurrounding(); Climbing(); }
    }

    void CheckWall()
    {
        wallRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(wallRay.origin, wallRay.direction * checkClimbMultiple, Color.red);

        if (Physics.Raycast(wallRay, out hit, checkClimbMultiple))
        {
            if (hit.transform.CompareTag("Wall"))
            {
                playerController.EnableJump = false;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartClimb();
                }
            }

            else
            {
                playerController.EnableJump = true;
            }
        }
    }

    void StartClimb()
    {
        isClimbing = true;
        playerController.EnableMove = false;
        playerController.EnableGravity = false;
    }

    void EndClimb()
    {
        isClimbing = false;
        playerController.EnableMove = true;
        playerController.EnableGravity = false;
    }

    void Climbing()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        if (inputZ > 0)
        {
            transform.Translate(0f, 1f * climbForce * Time.deltaTime, 0f);
            //rb.AddForce(transform.up * climbForce * Time.deltaTime, ForceMode.Acceleration);
        }
        if (inputZ < 0)
        {
            transform.Translate(0f, -1f * climbForce * Time.deltaTime, 0f);
        }
        if (inputX > 0)
        {
            transform.Translate(1f * climbForce * Time.deltaTime, 0f, 0f);
        }
        if (inputX < 0)
        {
            transform.Translate(-1f * climbForce * Time.deltaTime, 0f, 0f);
        }
    }

    void CheckWallSurrounding()
    {
        topDir = (topPosition.transform.position - transform.position).normalized;
        bottomDir = (bottomPosition.transform.position - transform.position).normalized;
        leftDir = (leftPosition.transform.position - transform.position).normalized;
        rightDir = (rightPosition.transform.position - transform.position).normalized;

        forwardRay = new Ray(transform.position, transform.forward);
        upRay = new Ray(transform.position, topDir);
        downRay = new Ray(transform.position, bottomDir);
        leftRay = new Ray(transform.position, leftDir);
        rightRay = new Ray(transform.position, rightDir);

        Debug.DrawRay(forwardRay.origin, forwardRay.direction * climbLengthMultiple, Color.red);
        Debug.DrawRay(upRay.origin, upRay.direction * climbLengthMultiple, Color.red);
        Debug.DrawRay(downRay.origin, downRay.direction * climbLengthMultiple, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction * climbLengthMultiple, Color.red);
        Debug.DrawRay(rightRay.origin, rightRay.direction * climbLengthMultiple, Color.red);
    }
}
