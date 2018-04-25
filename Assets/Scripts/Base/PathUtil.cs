using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class PathUtil : Singleton<PathUtil>
{
    public string DataPath;
    public string AssetBundlePath;
    public PathUtil()
    {
        DataPath = "Data/{0}";
        AssetBundlePath = Application.streamingAssetsPath + "/{0}";
    }

    private static StringBuilder sb = new StringBuilder();
    /// <summary>
    /// 检测路径 先到Application.persistentDataPath查看   没有则使用首包的路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string CheckPath(string path)
    {
        sb.Length = 0;
        sb.Append(Application.persistentDataPath).Append("/").Append(path);
        string newPath = sb.ToString();
        if (!File.Exists(newPath))
        {
            sb.Length = 0;
            sb.Append(Application.streamingAssetsPath).Append("/").Append(path);
#if UNITY_EDITOR||UNITY_STANDALONE
            //sb.Insert(0, "file:///");
#elif UNITY_ANDROID
            //sb.Insert(0, "file://");
#elif UNITY_IPHONE
            sb.Insert(0, "file://");
#endif
        }
        else
        {
#if UNITY_EDITOR||UNITY_STANDALONE
            //sb.Insert(0, "file:///");
#elif UNITY_ANDROID
            //sb.Insert(0, "file://");
#elif UNITY_IPHONE
            sb.Insert(0, "file://");
#endif
        }
        newPath = sb.ToString();
        return newPath;
    }

    public static string GetStreammingAssetsPath(string fileName)
    {
#if UNITY_EDITOR
        return string.Format("{0}/{1}", Application.streamingAssetsPath, fileName);
#elif UNITY_ANDROID
        return string.Format(@"jar:file://{0}!/assets/{1}", Application.dataPath, fileName);
#endif
    }

    public static string GetName(string path)
    {
        int splitIdx = path.LastIndexOf('/');
        if (splitIdx >= 0)
        {
            return path.Substring(splitIdx + 1);
        }
        else
        {
            return path;
        }
    }
}
