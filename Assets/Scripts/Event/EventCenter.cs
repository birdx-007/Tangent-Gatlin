using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    PlayerWeaponTryShoot,
    PlayerWeaponStartRecoverOneBullet,
    PlayerWeaponEndRecoverOneBullet,
    FightStart
}

public class EventCenter
{
    private static Dictionary<EventType, Delegate> _dictionary = new Dictionary<EventType, Delegate>();

    private static void OnListenerAdding(EventType type, Delegate callback)
    {
        _dictionary.TryAdd(type, null);
        Delegate d = _dictionary[type];
        if (d != null && d.GetType() != callback.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件所对应的委托是{1}，要添加的委托类型为{2}", type, d.GetType(), callback.GetType()));
        }
    }

    private static void OnListenerRemoving(EventType type, Delegate callback)
    {
        if (_dictionary.TryGetValue(type, out var d))
        {
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", type));
            }
            else if (d.GetType() != callback.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", type, d.GetType(), callback.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误：没有事件码{0}", type));
        }
    }
    
    private static void OnListenerRemoved(EventType type)
    {
        if (_dictionary[type] == null)
        {
            _dictionary.Remove(type);
        }
    }

    #region AddListener

    public static void AddListener(EventType type, Callback callback)
    {
        OnListenerAdding(type,callback);
        _dictionary[type] = (Callback)_dictionary[type] + callback;
    }

    public static void AddListener<T>(EventType type, Callback<T> callback)
    {
        OnListenerAdding(type,callback);
        _dictionary[type] = (Callback<T>)_dictionary[type] + callback;
    }
    
    public static void AddListener<T1,T2>(EventType type, Callback<T1,T2> callback)
    {
        OnListenerAdding(type,callback);
        _dictionary[type] = (Callback<T1,T2>)_dictionary[type] + callback;
    }

    #endregion

    #region RemoveListener

    public static void RemoveListener(EventType eventType, Callback callBack)
    {
        OnListenerRemoving(eventType, callBack);
        _dictionary[eventType] = (Callback)_dictionary[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    
    public static void RemoveListener<T>(EventType eventType, Callback<T> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        _dictionary[eventType] = (Callback<T>)_dictionary[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    
    public static void RemoveListener<T1,T2>(EventType eventType, Callback<T1,T2> callBack)
    {
        OnListenerRemoving(eventType, callBack);
        _dictionary[eventType] = (Callback<T1,T2>)_dictionary[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    #endregion

    #region BroadCast

    public static void Broadcast(EventType eventType)
    {
        if (_dictionary.TryGetValue(eventType, out var d))
        {
            if (d is Callback callBack)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
    
    public static void Broadcast<T>(EventType eventType, T arg)
    {
        if (_dictionary.TryGetValue(eventType, out var d))
        {
            if (d is Callback<T> callBack)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
    
    public static void Broadcast<T1,T2>(EventType eventType, T1 arg1, T2 arg2)
    {
        if (_dictionary.TryGetValue(eventType, out var d))
        {
            if (d is Callback<T1,T2> callBack)
            {
                callBack(arg1,arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    #endregion
}
