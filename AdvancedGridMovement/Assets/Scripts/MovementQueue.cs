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
    private Action currentAction;

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
                currentAction = movementQueue.Dequeue();
                currentAction.Invoke();
            }
        }
        else
        {
            if (movementQueue.Count > 0)
            {
                Action nextAction = movementQueue.Peek();
                if (nextAction == currentAction)
                {
                    advancedGridMovement.SwitchToRunning();
                    return;
                }
            }
            advancedGridMovement.SwitchToWalking();
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
        if ((movementQueue.Count < QueueDepth) && (movementQueue.Count > 0))
        {
            movementQueue.Enqueue(() => { advancedGridMovement.MoveForward(); });
        }
    }
}
