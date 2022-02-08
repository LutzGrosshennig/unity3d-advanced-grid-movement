using UnityEngine;
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
        var targetHeading = Vector3.Normalize(moveTowardsPosition - moveFromPosition);
        var newPosition = moveFromPosition + (targetHeading * (currentPositionValue * gridSize));
        newPosition.y = currentHeadBobValue;
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
        var currentPosition = transform.position;
        currentPosition.y = 0.0f;

        if (currentPosition == moveTowardsPosition)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            transform.position = moveTowardsPosition;
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
        return transform.position != moveTowardsPosition;
    }

    private bool IsRotating()
    {
        return transform.rotation != rotateTowardsDirection;
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
