using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ZEventSystem : Singleton<ZEventSystem>
{

    private struct EventPair
    {
        public object obj;
        public MethodInfo method;
    }

    //public delegate void EventFunc(params object[] objs);
    private static Dictionary<string, List<EventPair>> _events = new Dictionary<string, List<EventPair>>();

	public ZEventSystem()
	{
	}

    public static void Register(string eventName, object register, string funcName)
    {
        List<EventPair> lst = _events.ContainsKey(eventName) ? _events[eventName] : new List<EventPair>();
        for (int i = 0; i < lst.Count; i++)
        {
            if (register.Equals(lst[i].obj) && lst[i].method.Name.Equals(funcName))
                return;
        }
        EventPair ep = new EventPair();
        ep.obj = register;
        ep.method = register.GetType().GetMethod(funcName);
        if (ep.method == null)
        {
            EDebug.LogErrorFormat("ZEventSystem register failed, obj {0} get method:{1} failed, no this method", register, funcName);
            return;
        }
        lst.Add(ep);
        _events[eventName] = lst;
    }

    public static void DeRegister(string eventName, object register = null)
    {
        if (!_events.ContainsKey(eventName))
            return;
        if (register == null)
        {
            _events.Remove(eventName);
            return;
        }
        List<EventPair> lst = _events[eventName];
        for (int idx = 0; idx < lst.Count; ++idx)
        {
            if (register == lst[idx].obj)
            {
                lst.RemoveAt(idx);
            }
        }
    }

    public static void Dispatch(string eventName, params object[] args)
    {
        if (_events.ContainsKey(eventName))
        {
            List<EventPair> lst = _events[eventName];
            for (int idx = 0; idx < lst.Count; ++idx)
            {
                EventPair ep = lst[idx];
                ep.method.Invoke(ep.obj, args);
            }
        }
    }

    public static void ClearEvent()
    {
        _events.Clear();
    }
}
