using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }

    private static EventManager instance = null;

    private Dictionary<Event_Type, List<IListener>> Listeners = new Dictionary<Event_Type, List<IListener>>();


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void AddListener(Event_Type type, IListener Listener)
    {
        List<IListener> ListenerList = null;
        if (Listeners.TryGetValue(type, out ListenerList))
        {
            ListenerList.Add(Listener);
            return;
        }

        ListenerList = new List<IListener>();
        ListenerList.Add(Listener);
        Listeners.Add(type, ListenerList);
    }

    public void PostNotification(Event_Type type, Component sender, object param = null)
    {
        List<IListener> ListenList = null;
        if (!Listeners.TryGetValue(type, out ListenList))
        {
            return;
        }
        for (int i = 0; i < ListenList.Count; i++)
        {
            if (!ListenList[i].Equals(null))
            {
                ListenList[i].OnNotify(type, sender, param);
            }
        }
    }

    public void RemoveEvent(Event_Type type)
    {
        Listeners.Remove(type);
    }

    public void RemoveRedundancies()
    {
        Dictionary<Event_Type, List<IListener>> TmpListeners = new Dictionary<Event_Type, List<IListener>>();
        foreach (KeyValuePair<Event_Type, List<IListener>> item in Listeners)
        {
            for (int i = item.Value.Count - 1; i >= 0; i--)
            {
                if (item.Value[i].Equals(null))
                {
                    item.Value.RemoveAt(i);
                }
            }
            if (item.Value.Count > 0)
                TmpListeners.Add(item.Key, item.Value);
        }
        Listeners = TmpListeners;
    }

    private void OnLevelWasLoaded(int level)
    {
        RemoveRedundancies();
    }
}
