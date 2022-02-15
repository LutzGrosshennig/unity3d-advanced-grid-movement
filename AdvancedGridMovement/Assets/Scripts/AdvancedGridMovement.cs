﻿using UnityEngine;
using UnityEngine.Events;

public class AdvancedGridMovement : MonoBehaviour
{
    private const float RightHand = 90.0f;
    private const float LeftHand = -RightHand;

    [SerializeField] private float gridSize = 3.0f;

    [Header("Walk speed settings")]
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float turnSpeed = 5.0f;

    [Header("Movement animation curve")]
    [SerializeField] private AnimationCurve walkSpeedCurve;

    [Header("Walking head bob curve")]
    [SerializeField] private AnimationCurve walkHeadBobCurve;

    [Header("Run speed settings")]
    [SerializeField] private float runningSpeed = 1.5f;

    [Header("Running animation curve")]
    [SerializeField] private AnimationCurve runningSpeedCurve;

    [Header("Running head bob curve")]
    [SerializeField] private AnimationCurve runningHeadBobCurve;

    [Header("Step height")]
    [SerializeField] private float maximumStepHeight = 2.0f;


    [Header("Event when the path is blocked")]
    [SerializeField] private UnityEvent blockedEvent;

    // Animation target values.
    private Vector3 moveTowardsPosition;
    private Quaternion rotateFromDirection;

    // Animation source values.
    private Vector3 moveFromPosition;
    private Quaternion rotateTowardsDirection;

    // Animation progress
    private float rotationTime = 0.0f;
    private float curveTime = 0.0f;

    //Current settings
    private AnimationCurve currentAnimationCurve;
    private AnimationCurve currentHeadBobCurve;
    private float currentSpeed;

    void Start()
    {
        moveTowardsPosition = transform.position;
        rotateTowardsDirection = transform.rotation;
        currentAnimationCurve = walkSpeedCurve;
        currentHeadBobCurve = walkHeadBobCurve;
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (IsMoving())
        {
            AnimateMovement();
        }

        if (IsRotating())
        {
            AnimateRotation();
        }
    }

    public void SwitchToWalking()
    {
        currentSpeed = walkSpeed;
        currentAnimationCurve = walkSpeedCurve;
        currentHeadBobCurve = walkHeadBobCurve;
    }

    public void SwitchToRunning()
    {
        currentSpeed = runningSpeed;
        currentAnimationCurve = runningSpeedCurve;
        currentHeadBobCurve = runningHeadBobCurve;
    }

    private void AnimateRotation()
    {
        rotationTime += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(rotateFromDirection, rotateTowardsDirection, rotationTime * turnSpeed);
        CompensateRoundingErrors();
    }

    private void AnimateMovement()
    {
        curveTime += Time.deltaTime * currentSpeed;
        var currentPositionValue = currentAnimationCurve.Evaluate(curveTime);
        var currentHeadBobValue = currentHeadBobCurve.Evaluate(curveTime * gridSize);
        var targetHeading = Vector3.Normalize(HeightInvariantVector(moveTowardsPosition) - HeightInvariantVector(moveFromPosition));
        var newPosition = moveFromPosition + (targetHeading * (currentPositionValue * gridSize));
        newPosition.y = maximumStepHeight;
        
        RaycastHit hit;
        Ray downRay = new Ray(newPosition, -Vector3.up);

        // Cast a ray straight downwards.
        if (Physics.Raycast(downRay, out hit))
        {
            newPosition.y = (maximumStepHeight - hit.distance) + currentHeadBobValue;
        }
        else
        {
            newPosition.y = currentHeadBobValue;
        }

        transform.position = newPosition;
        CompensateRoundingErrors();
    }

    private void CompensateRoundingErrors()
    {
        // Bear in mind that floating point numbers are inaccurate by design. 
        // The == operator performs a fuzy compare which means that we are only approximatly near the target rotation.
        // We may not entirely reached the rotateTowardsViewAngle or we may have slightly overshot it already (both within the margin of error).
        if (transform.rotation == rotateTowardsDirection)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            transform.rotation = rotateTowardsDirection;
        }

        //mask out the head bobbing
        var currentPosition = HeightInvariantVector(transform.position);
        var target = HeightInvariantVector(moveTowardsPosition);

        if (currentPosition == target)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            currentPosition = HeightInvariantVector(moveTowardsPosition);
            currentPosition.y = transform.position.y;

            transform.position = currentPosition;
            curveTime = 0.0f;
        }

    }

    public void MoveForward()
    {
        CollisonCheckedMovement(CalculateForwardPosition());
    }

    public void MoveBackward()
    {
        CollisonCheckedMovement(-CalculateForwardPosition());
    }

    public void StrafeRight()
    {
        CollisonCheckedMovement(CalculateStrafePosition());
    }

    public void StrafeLeft()
    {
        CollisonCheckedMovement(-CalculateStrafePosition());
    }

    private void CollisonCheckedMovement(Vector3 movementDirection)
    {
        if (IsStationary())
        {
            Vector3 targetPosition = moveTowardsPosition + movementDirection;
            if (FreeSpace(targetPosition))
            {
                moveFromPosition = transform.position;
                moveTowardsPosition = targetPosition;
            }
            else
            {
                blockedEvent?.Invoke();
            }
        }
    }

    // should be refactored into an new class
    private bool FreeSpace(Vector3 targetPosition)
    {
        // this is pretty lousy way to perform collision checks, its just here for demonstration purposes.
        // Hint: layers are much faster then tags ;-)
        Vector3 delta = targetPosition - moveTowardsPosition;
        delta *= .6f;
        Collider[] intersectingColliders = Physics.OverlapBox(moveTowardsPosition + delta, new Vector3((gridSize / 2.0f) - .1f, 1.0f, (gridSize / 2.0f) - .1f), gameObject.transform.rotation);
        Collider[] filteredColliders = System.Array.FindAll(intersectingColliders, collider => collider.CompareTag("Enemy") || collider.CompareTag("Level"));
        return filteredColliders.Length == 0;
    }

    public void TurnRight()
    {
        TurnEulerDegrees(RightHand);
    }

    public void TurnLeft()
    {
        TurnEulerDegrees(LeftHand);
    }

    private void TurnEulerDegrees(in float eulerDirectionDelta)
    {
        if (!IsRotating())
        {
            rotateFromDirection = transform.rotation;
            rotateTowardsDirection *= Quaternion.Euler(0.0f, eulerDirectionDelta, 0.0f);
            rotationTime = 0.0f;
        }
    }

    public bool IsStationary()
    {
        return !(IsMoving() || IsRotating());
    }

    private bool IsMoving()
    {
        var current = HeightInvariantVector(transform.position);
        var target = HeightInvariantVector(moveTowardsPosition);
        return current != target;
    }

    private bool IsRotating()
    {
        return transform.rotation != rotateTowardsDirection;
    }

    private Vector3 HeightInvariantVector(Vector3 inVector)
    {
        return new Vector3(inVector.x, 0.0f, inVector.z);
    }

    private Vector3 CalculateForwardPosition()
    {
        return transform.forward * gridSize;
    }

    private Vector3 CalculateStrafePosition()
    {
        return transform.right * gridSize;
    }
}
