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

    private Queue<Action> movementQueue;
    private AdvancedGridMovement advancedGridMovement;

    void Start()
    {
        if (QueueDepth < 1)
        {
            QueueDepth = 1;
        }

        movementQueue = new Queue<Action>(QueueDepth);
        advancedGridMovement = GetComponent<AdvancedGridMovement>();
    }

    void Update()
    {
        if (advancedGridMovement.IsStationary())
        {
            if (movementQueue.Count > 0)
            {
                var nextAction = movementQueue.Dequeue();
                nextAction.Invoke();
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
}
