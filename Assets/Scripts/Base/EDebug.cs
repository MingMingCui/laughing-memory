using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDebug {
#if DEBUG
    public static bool EnableDebug = true;
#else
    public static bool EnableDebug = false;
#endif

    public static void Log(object message)
    {
        if (EnableDebug)
            Debug.Log(message);
    }

    public static void LogFormat(string info, params object[] args)
    {
        if (EnableDebug)
            Debug.LogFormat(info, args);
    }

    public static void LogWarning(string info)
    {
        if (EnableDebug)
            Debug.LogWarning(info);
    }

    public static void LogWarningFormat(string info, params object[] args)
    {
        if (EnableDebug)
            Debug.LogWarningFormat(info, args);
    }

    public static void LogError(string info)
    {
        if (EnableDebug)
            Debug.LogError(info);
    }

    public static void LogErrorFormat(string info, params object[] args)
    {
        if (EnableDebug)
            Debug.LogErrorFormat(info, args);
    }
}
