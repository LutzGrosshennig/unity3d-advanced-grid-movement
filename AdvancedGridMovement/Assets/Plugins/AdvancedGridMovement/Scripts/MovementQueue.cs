using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementQueue : MonoBehaviour
{
    [Header("Queue settings")]
    [SerializeField] [Range(1,5)] private int QueueDepth = 1;

    [Header("Event section")]
    [SerializeField] private UnityEvent EventIfTheCommandIsNotQueable;

    [Header("Key press threshold to enable running")]
    [SerializeField] private float keyPressThresholdTime = 0.5f;

    private AdvancedGridMovement advancedGridMovement;
    private Queue<Action> movementQueue;
    private Action currentAction;
    private float forwardKeyPressedTime;

    void Start()
    {
        movementQueue = new Queue<Action>(QueueDepth);
        advancedGridMovement = GetComponent<AdvancedGridMovement>();
        ResetKeyPressTimer();
    }

    void Update()
    {
        if (advancedGridMovement.IsStationary())
        {
            if (movementQueue.Count > 0)
            {
                currentAction = movementQueue.Dequeue();
                currentAction.Invoke();
            }
        }
    }

    private void queueCommand(Action action)
    {
        if (movementQueue.Count >= QueueDepth)
        {
            EventIfTheCommandIsNotQueable?.Invoke();
        }
        else
        {
            movementQueue.Enqueue(action);
        }
    }

    public void FlushQueue()
    {
        ResetKeyPressTimer();
        movementQueue.Clear();
        advancedGridMovement.SwitchToWalking();
    }

    private void ResetKeyPressTimer()
    {
        forwardKeyPressedTime = 0.0f;
    }

    public void Forward()
    {
        queueCommand(() => { advancedGridMovement.MoveForward(); });
    }

    public void Backward()
    {
        queueCommand(() => { advancedGridMovement.MoveBackward(); });
    }

    public void StrafeLeft()
    {
        queueCommand(() => { advancedGridMovement.StrafeLeft(); });
    }

    public void StrafeRight()
    {
        queueCommand(() => { advancedGridMovement.StrafeRight(); });
    }

    public void TurnLeft()
    {
        queueCommand(() => { advancedGridMovement.TurnLeft(); });
    }

    public void TurnRight()
    {
        queueCommand(() => { advancedGridMovement.TurnRight(); });
    }

    public void RunForward()
    {
        forwardKeyPressedTime += Time.deltaTime;

        if (forwardKeyPressedTime >= keyPressThresholdTime)
        {
            if ((movementQueue.Count < QueueDepth))
            {
                advancedGridMovement.SwitchToRunning();
                movementQueue.Enqueue(() => { advancedGridMovement.MoveForward(); });
            }
        }
    }

    public void StopRunForward()
    {
        if (forwardKeyPressedTime >= keyPressThresholdTime)
        {
            FlushQueue();
        }
    }
}
