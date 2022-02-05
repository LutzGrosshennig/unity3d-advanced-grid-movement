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
    [SerializeField] private EventMapping[] eventMappingsKeyUp;

    void Update()
    {
        Action<EventMapping> actionKeyDown = new Action<EventMapping>(InputMappingKeyDown);
        Array.ForEach(eventMappingsKeyDown, actionKeyDown);

        Action<EventMapping> actionKeyUp = new Action<EventMapping>(InputMappingKeyUp);
        Array.ForEach(eventMappingsKeyUp, actionKeyDown);

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
