using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HotUpdate : MonoBehaviour {

    public static string RemoteRes = "192.168.0.254/Res/";
    public delegate void OnLoadData(string data, string err = "");

    public class ResData
    {
        public string ABName;
        public string MD5;
        public uint Size;
    }

    // Use this for initialization
    void Start ()
    {
        ZEventSystem.Register(EventConst.OnGamePreloaded, this, "OnGamePreloaded");
        _clientMd5Path = Application.persistentDataPath + "/md5file.uab";
        _serverMd5Path = getRemotePath("md5file.uab");
        StartCoroutine(CheckUpdate());
    }

    private string _clientMd5Data;
    private string _serverMd5Data;
    private Dictionary<string, ResData> _clientList = new Dictionary<string, ResData>();
    private Dictionary<string, ResData> _serverList = new Dictionary<string, ResData>();
    private List<string> _updateList = new List<string>();
    private List<string> _removeList = new List<string>();
    private uint totalByte = 0;
    private int downloadByte = 0;
    private int curByte = 0;
    private float lastUpdateTime = 0;
    private int lastCurTotalByte = 0;
    private string _clientMd5Path;
    private string _serverMd5Path;

    private WWW downloadAsset = null;

    private string[] filesToUncompress;
    private int uncompressCount = 0;
    private int uncompressIdx = 0;
    private bool isDownloading = false;

    void Update()
    {
        if (isDownloading)
        {
            if (_updateList.Count > 0)
            {
                if (downloadAsset == null)
                {
                    downloadAsset = new WWW(getRemotePath(_updateList[0]));
                }
                else
                {
                    if (!string.IsNullOrEmpty(downloadAsset.error))
                    {
                        Debug.LogErrorFormat("HotUpdate, download failed, error:{0}", downloadAsset.error);
                        downloadAsset.Dispose();
                        downloadAsset = null;
                    }
                    else if (downloadAsset.isDone)
                    {
                        downloadByte += downloadAsset.bytesDownloaded;
                        curByte = 0;
                        string fileName = _updateList[0];
                        writeFile(fileName, downloadAsset.bytes, downloadAsset.bytesDownloaded);
                        downloadAsset.Dispose();
                        downloadAsset = null;
                        string clientLine = _clientList.ContainsKey(fileName) ? resData2Line(_clientList[fileName]) : "";
                        string serverLine = resData2Line(_serverList[fileName]);
                        if (string.IsNullOrEmpty(clientLine))
                            _clientMd5Data += clientLine;
                        else
                            _clientMd5Data = _clientMd5Data.Replace(clientLine, serverLine);

                        _updateList.RemoveAt(0);
                    }
                    else
                    {
                        curByte = downloadAsset.bytesDownloaded;
                    }
                }

                if (Time.time - lastUpdateTime >= 1)
                {
                    int curTotalByte = downloadByte + curByte;
                    CanvasView.Instance.SetDownloading(curTotalByte, totalByte, Mathf.Max(0, (curTotalByte - lastCurTotalByte) / (Time.time - lastUpdateTime)));
                    lastUpdateTime = Time.time;
                    lastCurTotalByte = curTotalByte;
                }

            }
            else
            {
                OnUpdateOver();
            }
        }
    }

    private IEnumerator CheckUpdate()
    {
#if USE_ASSETBUNDLE
        yield return checkUncompressAsset();
#endif

#if UPDATE
        yield return LoadUpdateConfig(_clientMd5Path, true, (string data, string err) =>
        {
            if (err == "")
            {
                _clientMd5Data = data;
                _clientList = getResList(data);
            }
            else
            {
                Debug.LogErrorFormat("CheckUpdate:Load local md5file.uab failed, err:" + err);
            }
        });

        yield return LoadUpdateConfig(_serverMd5Path, false, (string data, string err) =>
        {
            if (err == "")
            {
                _serverMd5Data = data;
                _serverList = getResList(data);
            }
            else
            {
                Debug.LogErrorFormat("CheckUpdate:Load remote md5file.uab failed, err:" + err);
            }
        });

        CheckUpdateList();

#else
        OnUpdateOver();
#endif
        yield return null;
    }

    public void OnGamePreloaded() { 
    
        JsonMgr.GetSingleton();
        SceneMgr.Instance.LoadScene("Login");
    }

    public void OnUpdateOver()
    {
#if UPDATE
        WriteMd5(true);
#endif
        isDownloading = false;
        ResourceMgr.Instance.GamePreload();
    }

    private void CheckUpdateList()
    {
        foreach (var fileData in _serverList)
        {
            string fileName = fileData.Key;

            if (!_clientList.ContainsKey(fileName))
            {
                _updateList.Add(fileName);
            }
            else
            {
                string serverMD5 = fileData.Value.MD5;
                ResData clientData = _clientList[fileName];
                string clientMD5 = clientData.MD5;
                if (serverMD5 != clientMD5)
                {
                    _updateList.Add(fileName);
                }
            }
        }

        foreach (var fileData in _clientList)
        {
            string fileName = fileData.Key;
            if (!_serverList.ContainsKey(fileName))
            {
                _removeList.Add(fileName);
            }
        }

        //删除本地多余文件
        if (_removeList.Count > 0)
        {
            string datePath = string.Empty;
            for (int i = 0; i < _removeList.Count; i++)
            {
                string dataName = _removeList[i];
                datePath = Application.persistentDataPath + "/" + dataName;
                if (File.Exists(datePath))
                    File.Delete(datePath);
                if (_clientList.ContainsKey(dataName))
                {
                    string md5Line = resData2Line(_clientList[dataName]);
                    _clientMd5Data = _clientMd5Data.Replace(md5Line, "");
                }
            }
            WriteMd5(false);
        }

        for (int idx = 0; idx < _updateList.Count; ++idx)
        {
            totalByte += _serverList[_updateList[idx]].Size;
        }

        if (totalByte > 0)
        {
            CanvasView.Instance.OpenDownloading();
            isDownloading = true;
        }
        else
        {
            OnUpdateOver();
        }
    }

    private IEnumerator LoadUpdateConfig(string fullPath, bool isLocal, OnLoadData ret)
    {
        Debug.Log("LoadUpdateConfig " + fullPath);
        byte[] bytes = new byte[0];
        bool loadErr = false;
        if (isLocal)
        {
            if (File.Exists(fullPath))
                bytes = File.ReadAllBytes(fullPath);
            else
                loadErr = true;
        }
        else
        {
            WWW www = new WWW(fullPath);
            yield return www;
            if (www != null)
            {
                bytes = www.bytes;
                www.Dispose();
            }
            else
                loadErr = true;
        }
        
        if (loadErr)
        {
            ret("", string.Format("Load client www error, path:{0}", fullPath));
            yield break;
        }
        else
        {
            ret(System.Text.Encoding.ASCII.GetString(bytes));
        }
    }

    private void writeFile(string fileName, byte[] content, int length)
    {
        Stream s = null;
        FileInfo fileInfo = new FileInfo(string.Format("{0}/{1}", Application.persistentDataPath, fileName));
        if (fileInfo.Exists)
        {
            fileInfo.Delete();
        }
        s = fileInfo.Create();
        s.Write(content, 0, length);
        s.Flush();
        s.Close();
        s.Dispose();
    }

    void WriteMd5(bool isLoaded)
    {
        string content = isLoaded ? _serverMd5Data : _clientMd5Data;
        byte[] bytes = System.Text.Encoding.Default.GetBytes(content);
        File.WriteAllBytes(_clientMd5Path, bytes);
    }

    //检查是否需要解压资源
    private IEnumerator checkUncompressAsset()
    {
        string md5Path = Path.Combine(Application.persistentDataPath, ResourceMgr.md5Name);
        if (File.Exists(md5Path))
        {
            yield break;
        }
        string mainfeastPath = PathUtil.CheckPath("StreamingAssets");
        AssetBundleManifest mainfeast;
        WWW www = new WWW(mainfeastPath);
        yield return www;
        if (www.error != null)
        {
            throw new System.Exception("加载Manifest失败");
        }
        AssetBundle ab = www.assetBundle;
        mainfeast = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        ab.Unload(false);
        www.Dispose();
        filesToUncompress = mainfeast.GetAllAssetBundles();
        yield return startUncompress();
    }

    private IEnumerator startUncompress()
    {
        Debug.Log("startUncompress");
        CanvasView.Instance.OpenUncompress();
        for (int idx = 0; idx < filesToUncompress.Length; ++idx)
        {
            string fileName = filesToUncompress[idx];
            yield return uncompressAsset(fileName);
            float progress = (float)idx / filesToUncompress.Length;
            CanvasView.Instance.SetUncompress(progress);
        }
        yield return uncompressAsset(ResourceMgr.md5Name);
    }

    private IEnumerator uncompressAsset(string name)
    {
        string path = Application.streamingAssetsPath + "/" + name;
        string filePath = Application.persistentDataPath + "/" + name;
#if UNITY_EDITOR || UNITY_IOS
        path = "file://" + path;
#endif
        using (WWW www = new WWW(path))
        {
            yield return www;
            if (www.error != null)
                throw new System.Exception(string.Format("解压资源{0}失败 ", path));
            File.WriteAllBytes(filePath, www.bytes);
        }
    }

    private string getRemotePath(string fileName)
    {
#if UNITY_EDITOR
        return PathUtil.GetStreammingAssetsPath(fileName);
#else
        return RemoteRes + fileName;
#endif
    }

    private Dictionary<string, ResData> getResList(string data)
    {
        Dictionary<string, ResData> ret = new Dictionary<string, ResData>();
        string[] lines = data.Split('\n');
        for (int idx = 0; idx < lines.Length; ++idx)
        {
            string line = lines[idx];
            if (string.IsNullOrEmpty(line))
                continue;
            ResData resData = new ResData();
            string[] info = line.Split('|');
            resData.ABName = info[0];
            resData.MD5 = info[1];
            resData.Size = uint.Parse(info[2]);
            ret.Add(info[0], resData);
        }
        return ret;
    }

    private string resData2Line(ResData data)
    {
        return string.Format("{0}|{1}|{2}\n", data.ABName, data.MD5, data.Size);
    }
}
