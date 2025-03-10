using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private LayerMask whatIsWall;
    private Rigidbody rb;
    private CharacterMovementController cmc;


    [Header("Climbing")]
    [SerializeField] private float climbSpeed;
    [SerializeField] private float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("ClimbJumping")]
    [SerializeField] private float climbJumpUpForce;
    [SerializeField] private float climbJumpBackForce;
    [SerializeField] private int climbJumps;
    private int climbJumpsLeft;

    [Header("Detection")]
    [SerializeField] private float detectionLength;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    [SerializeField] private float minWallNormalAngleChange;

    [Header("Exiting")]
    [SerializeField] private float exitWallTime;
    private float exitWallTimer;
    private bool exitingWall;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cmc = GetComponent<CharacterMovementController>();
    }

    private bool climbingInput;
    private InputDetector jumpInput;

    public void HandlePlayerInputs(Vector2 moveInput, bool jump)
    {
        climbingInput = moveInput.y > 0f;
        jumpInput.inputState = jump;
    }

    private void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing && !exitingWall)
        { ClimbingMovement(); }
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && climbingInput && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if (!climbing & climbTimer > 0)
            { StartClimbing(); }

            // timer
            if (climbTimer > 0)
            { climbTimer -= Time.deltaTime; }

            if (climbTimer < 0)
            { StopClimbing(); }
        }
        // State 2 - Exiting
        else if (exitingWall)
        {
            if (climbing) { StopClimbing(); }
            if (exitWallTimer > 0) { exitWallTimer -= Time.deltaTime; }
            if (exitWallTimer < 0) 
            { 
                exitingWall = false;
                cmc.isExitingWall = false;
            }
        }
        // State 3 - None
        else
        {
            if (climbing) { StopClimbing(); }
        }

        if (wallFront && jumpInput.HasStateChanged() == 0 && climbJumpsLeft > 0) { ClimbJump(); }
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(
            transform.position, 
            sphereCastRadius, 
            orientation.forward, 
            out frontWallHit, 
            detectionLength, 
            whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || 
            Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || cmc.grounded) 
        { 
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void ClimbingMovement()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, climbSpeed, rb.linearVelocity.z);

        // sound effect
    }

    private void StartClimbing()
    {
        climbing = true;
        cmc.isClimbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;

        // change camera view
    }

    private void StopClimbing()
    {
        climbing = false;
        cmc.isClimbing = false;
        // revert camera view
    }
    private void ClimbJump()
    {
        if (cmc.grounded) { return; }

        exitingWall = true;
        cmc.isExitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = (transform.up * climbJumpUpForce) + (frontWallHit.normal * climbJumpBackForce);

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply * rb.mass, ForceMode.Impulse);

        climbJumpsLeft--;
    }
}
