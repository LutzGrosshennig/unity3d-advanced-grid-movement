/* Copyright 2021-2022 Lutz Groﬂhennig

Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
*/

using System;
using UnityEngine;
using UnityEngine.Events;

/*
 * This script adds a simple input handler for the old input system, that allows you to map key presses to events rather then using GetKeyXYZ() all over the place.
 * However I advice to use the new Unity Inputsystem which does something similar out-of-the-box, it is sadly not the default input system at the time of writing.
 */

public class InputHandler : MonoBehaviour
{
    [System.Serializable]
    public class EventMapping
    {
        public KeyCode key;
        public UnityEvent callback;
    }

    [SerializeField] private EventMapping[] eventMappings;
    [SerializeField] private EventMapping[] eventMappingsKeyDown;
    [SerializeField] private EventMapping[] eventMappingsKeyUp;

    void Update()
    {
        Action<EventMapping> actionKeyDown = new Action<EventMapping>(InputMappingKeyDown);
        Array.ForEach(eventMappingsKeyDown, actionKeyDown);

        Action<EventMapping> actionKeyUp = new Action<EventMapping>(InputMappingKeyUp);
        Array.ForEach(eventMappingsKeyUp, actionKeyUp);

        Action<EventMapping> action = new Action<EventMapping>(InputMapping);
        Array.ForEach(eventMappings, action);
    }

    private static void InputMapping(EventMapping eventMapping)
    {
        if (Input.GetKey(eventMapping.key))
        {
            eventMapping.callback.Invoke();
        }
    }

    private static void InputMappingKeyDown(EventMapping eventMapping)
    {
        if (Input.GetKeyDown(eventMapping.key))
        {
            eventMapping.callback.Invoke();
        }
    }

    private static void InputMappingKeyUp(EventMapping eventMapping)
    {
        if (Input.GetKeyUp(eventMapping.key))
        {
            eventMapping.callback.Invoke();
        }
    }
}
