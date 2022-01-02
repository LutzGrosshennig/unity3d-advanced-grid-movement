using UnityEngine;

public class AdvancedGridMovement : MonoBehaviour
{
    private const float LeftHand = -90.0f;
    private const float RightHand = +90.0f;

    public float gridSize = 4.0f;
    public float walkspeed = 1.0f;
    public float turnSpeed = 5.0f;

    private Vector3 moveTowardsPosition;
    private Quaternion rotateFromDirection;
    private Quaternion rotateTowardsDirection;

    private float rotationTime = 0.0f;

    void Start()
    {
        moveTowardsPosition = transform.position;
        rotateTowardsDirection = transform.rotation;
    }

    void Update()
    {
        if (IsMoving())
        {
            var step = Time.deltaTime * gridSize * walkspeed;
            AnimateMovement(step);
        }

        if (IsRotating())
        {
            AnimateRotation();
        }
    }

    private void AnimateRotation()
    {
        rotationTime += Time.deltaTime;
        transform.rotation = Quaternion.Slerp(rotateFromDirection, rotateTowardsDirection, rotationTime * turnSpeed);
      //  transform.rotation = Quaternion.RotateTowards(rotateFromDirection, rotateTowardsDirection, rotationTime * turnSpeed);
        CompensateRoundingErrors();
    }

    private void AnimateMovement(float step)
    {
        transform.position = Vector3.MoveTowards(transform.position, moveTowardsPosition, step);
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

        if (transform.position == moveTowardsPosition)
        {
            // To compensate rounding errors we explictly set the transform to our desired rotation.
            transform.position = moveTowardsPosition;
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
                moveTowardsPosition = targetPosition;
            }
            else
            {
            }
        }
    }

    private bool FreeSpace(Vector3 targetPosition)
    {
        Vector3 delta = targetPosition - moveTowardsPosition;
        delta *= .6f;
        Collider[] intersectingColliders = Physics.OverlapBox(moveTowardsPosition + delta, new Vector3((gridSize / 2.0f) - .1f, 1.0f, (gridSize / 2.0f) - .1f), gameObject.transform.rotation);
        Collider[] filteredColliders = System.Array.FindAll(intersectingColliders, collider => collider.CompareTag("Respawn") || collider.CompareTag("Level"));
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
            rotationTime = 0.0f;
            rotateTowardsDirection *= Quaternion.Euler(0.0f, eulerDirectionDelta, 0.0f);
        }
    }

    private bool IsStationary()
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
