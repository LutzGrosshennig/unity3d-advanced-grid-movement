/* Copyright 2021-2022 Lutz Groﬂhennig

Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * This script lets you queue up movement commands in advance and replays them.
 */

public class MovementQueue : MonoBehaviour
{
    [Header("Queue depth setting")]
    [SerializeField] [Range(1,5)] private int QueueDepth = 1;

    [Header("Event if the command can not be queued")]
    [SerializeField] private UnityEvent EventIfTheCommandIsNotQueable;

    [Header("Key press delay to switch into running mode")]
    [SerializeField] private float keyPressThresholdTime = 1.0f;

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
