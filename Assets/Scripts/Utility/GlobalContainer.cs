using System;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalContainer
{
    private static readonly Dictionary<Type, List<object>> entries = new Dictionary<Type, List<object>>();


    public static void Add<T>(T entry) where T : class
    {
        Type type = typeof(T);

        if (!entries.ContainsKey(type))
        {
            entries.Add(type, new List<object>());
        }

        entries[type].Add(entry);
    }

    public static void Remove(object entry)
    {
        Type type = entry.GetType();

        if (entries.ContainsKey(type))
        {
            if (entries[type].Contains(entry))
            {
                entries[type].Remove(entry);
            }
            else
            {
                Debug.LogError("Entry not found: " + entry);
            }
        }
        else
        {
            Debug.LogError("There're no entries for type: " + type);
        }
    }

    public static bool Exists(Type type)
    {
        if (entries.ContainsKey(type))
        {
            if (entries[type].Count > 0)
            {
                return true;
            }
        }

        return false;
    }

    public static T Get<T>() where T : class
    {
        T returnValue = null;

        Type type = typeof(T);

        if (entries.ContainsKey(type))
        {
            if (entries[type].Count > 0)
            {
                if (entries[type].Count > 1)
                {
                    Debug.LogWarningFormat("There's more than one entry of type '{0}', first one will be returned", type);
                }

                returnValue = entries[type][0] as T;

                if (returnValue == null)
                {
                    Debug.LogErrorFormat("Cannot cast entry '{0}' to type '{1}'", entries[type][0], type);
                }
            }
            else
            {
                Debug.LogError("Entry not found, type: " + type);
            }
        }
        else
        {
            Debug.LogErrorFormat("There're no entries for type '{0}'", type);
        }

        if (returnValue == null)
        {
            Type[] interfaces = type.GetInterfaces();
            foreach (Type iface in interfaces)
            {
                if (Exists(iface))
                {
                    Debug.LogWarningFormat("... but there's an entry for interface '{0}'. Are you sure you are requesting the right type?", iface.ToString());
                }
            }
        }

        return returnValue;
    }
}