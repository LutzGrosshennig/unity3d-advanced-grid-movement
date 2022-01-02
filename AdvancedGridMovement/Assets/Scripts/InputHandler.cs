using System;
using UnityEngine;
using UnityEngine.Events;

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

    void Update()
    {
        Action<EventMapping> action = new Action<EventMapping>(InputMapping);
        Array.ForEach(eventMappings, action);

        Action<EventMapping> actionKeyDown = new Action<EventMapping>(InputMappingKeyDown);
        Array.ForEach(eventMappingsKeyDown, actionKeyDown);
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
}
