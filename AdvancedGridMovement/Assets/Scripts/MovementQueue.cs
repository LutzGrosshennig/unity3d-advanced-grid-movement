using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementQueue : MonoBehaviour
{
    [Header("Queue settings")]
    [SerializeField] private int QueueDepth = 5;

    [Header("Event section")]
    [SerializeField] private UnityEvent EventIfTheCommandIsNotQueable;

    [Header("Key press threshold to enable running")]
    [SerializeField] private float keyPressThresholdTime = 0.5f;

    private Queue<Action> movementQueue;
    private AdvancedGridMovement advancedGridMovement;
    private Action currentAction;

    private float forwardKeyPressedTime;

    void Start()
    {
        if (QueueDepth < 1)
        {
            QueueDepth = 1;
        }

        movementQueue = new Queue<Action>(QueueDepth);
        advancedGridMovement = GetComponent<AdvancedGridMovement>();
        forwardKeyPressedTime = 0.0f;
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
        advancedGridMovement.SwitchToWalking();
        movementQueue.Clear();
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
            forwardKeyPressedTime = 0.0f;
            advancedGridMovement.SwitchToWalking();
            FlushQueue();
        }
    }
}
