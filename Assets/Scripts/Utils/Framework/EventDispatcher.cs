using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private static Dictionary<EventsID, Action<object>> _listeners = new Dictionary<EventsID, Action<object>>();


    #region Add Listeners, Post events, Remove listener

    /// <summary>
    /// Register to listen for eventID
    /// </summary>
    /// <param name="eventID">EventID that object want to listen</param>
    /// <param name="callback">Callback will be invoked when this eventID be raised</para	m>
    public static void RegisterListener(EventsID eventID, Action<object> callback)
    {
        // check if listener exist in distionary
        if (_listeners.ContainsKey(eventID))
        {
            // add callback to our collection
            _listeners[eventID] += callback;
        }
        else
        {
            // add new key-value pair
            _listeners.Add(eventID, null);
            _listeners[eventID] += callback;
        }
    }

    /// <summary>
    /// Posts the event. This will notify all listener that register for this event
    /// </summary>
    /// <param name="eventID">EventID.</param>
    /// <param name="sender">Sender, in some case, the Listener will need to know who send this message.</param>
    /// <param name="param">Parameter. Can be anything (struct, class ...), Listener will make a cast to get the data</param>
    public static void PostEvent(EventsID eventID, object param = null)
    {

        // posting event
        var callbacks = _listeners[eventID];
        // if there's no listener remain, then do nothing
        if (callbacks != null)
        {
            callbacks(param);
        }
        else
        {
            _listeners.Remove(eventID);
        }
    }

    /// <summary>
    /// Removes the listener. Use to Unregister listener
    /// </summary>
    /// <param name="eventID">EventID.</param>
    /// <param name="callback">Callback.</param>
    public static void RemoveListener(EventsID eventID, Action<object> callback)
    {
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] -= callback;
        }
    }
    #endregion
}
