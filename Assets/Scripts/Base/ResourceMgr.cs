using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.Profiling;

public class ResourceMgr : Singleton<ResourceMgr>
{
    public delegate void AsyncLoadBundleCallback(AssetBundle bundle);

    //public const string extension = ".uab";
    public const string md5Name = "md5file.uab";
    public const string verBundleName = "ver.uab";
    public const string shaderBundleName = "shader.uab";
    public const string dataBundleName = "textasset.uab";
    public const string materialBundleName = "material.uab";
    public const string luaBundleName = "xlua.uab";

    private Dictionary<string, Object> _allRes = new Dictionary<string, Object>();
    private Dictionary<string, AssetBundle> _allAssetBundles = new Dictionary<string, AssetBundle>();
    private AssetBundleManifest _mainFeast = null;
    private Dictionary<string, string> _allData = new Dictionary<string, string>();
    private Dictionary<string, TextAsset> _allLua = new Dictionary<string, TextAsset>();
    private Dictionary<string, AssetBundle> _constAssetBundle = new Dictionary<string, AssetBundle>();
    private Dictionary<string, Sprite> _allSprite = new Dictionary<string, Sprite>();
    private Dictionary<string, AudioClip> _allAudioClip = new Dictionary<string, AudioClip>();
    private List<string> _unpackedBundle = new List<string>();
    private WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();

    private List<string> _loadList = new List<string>();
    private List<string> _loadingList = new List<string>();

    private int count = 0;
    private int cur = 0;

    private float _progress = 0;
    public float Progress
    {
        get { return _progress; }
        set
        {
            if (_progress != value)
            {
                _progress = value;
                CanvasView.Instance.SetLoading(_progress);
            }
        }
    }

    public ResourceMgr()
    {
#if USE_ASSETBUNDLE
        _init();
#endif
    }

    private void _init()
    {
        AssetBundle mainfestBundle = AssetBundle.LoadFromFile(PathUtil.CheckPath("StreamingAssets"));
        _mainFeast = mainfestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    /// <summary>
    /// 整个游戏最早的预加载，只添加永不卸载的资源
    /// </summary>
    public void GamePreload()
    {
#if USE_ASSETBUNDLE
        _loadConstBundle(shaderBundleName);
        //_loadConstBundle(materialBundleName);
        AssetBundle dataBundle = _loadConstBundle(dataBundleName);
        Object[] datas = dataBundle.LoadAllAssets();
        for (int idx = 0; idx < datas.Length; ++idx)
        {
            TextAsset ta = datas[idx] as TextAsset;
            _allData[ta.name] = ta.text;
        }
        //load lua
#if HOTFIX_ENABLE
        AssetBundle luaBundle = _loadConstBundle(luaBundleName);
        Object[] luafiles = luaBundle.LoadAllAssets();
        for (int idx = 0; idx < luafiles.Length; ++idx)
        {
            TextAsset ta = luafiles[idx] as TextAsset;
            _allLua.Add(ta.name, ta);
        }
#endif
#endif
        ZEventSystem.Dispatch(EventConst.OnGamePreloaded);
    }

    private AssetBundle _loadConstBundle(string bundleName)
    {
        AssetBundle bundle = _loadAssetBundleWithoutDependence(bundleName);
        if (bundle == null)
            Debug.LogErrorFormat("Load {0} failed", bundleName);
        else
            _constAssetBundle.Add(bundleName, bundle);
        return bundle;
    }

    public void PreloadScene(string sceneName)
    {
        Debug.Log("PreloadScene " + sceneName);
        //ProcessCtrl.Instance.GoCoroutine("CoroutinePreloadScene", _coroutinePreloadScene(sceneName));
        _preloadScene(sceneName);
    }

    public void Clear()
    {
        _allRes.Clear();
        _allSprite.Clear();
        _allAudioClip.Clear();
        _loadingList.Clear();
        _loadList.Clear();
    }

    public void ClearAssetBundle()
    {
        foreach (var p in _allAssetBundles)
        {
            if(p.Value != null)
                p.Value.Unload(true);
        }
        _allAssetBundles.Clear();
    }

    public Object LoadResource(int id)
    {
        var jRes = JsonMgr.GetSingleton().GetResArrayByID(id);
        if (jRes == null)
        {
            Debug.LogErrorFormat("LoadResource failed, no such resid:{0}", id);
            return null;
        }
#if USE_ASSETBUNDLE
        string assetBundleName = jRes.ResourceName.ToString();
        if (_allRes.ContainsKey(assetBundleName))
            return _allRes[assetBundleName];
        AssetBundle bundle = _loadAssetBundle(assetBundleName);
        if (bundle == null)
        {
            Debug.LogErrorFormat("No AssetBundle Names:{0}. Please check res json or StreammingAssets");
            return null;
        }
        string path = jRes.ResourcePath.ToString();
        string fileName = PathUtil.GetName(path);
        Object obj = bundle.LoadAsset(fileName);
        if (obj == null)
        {
            Debug.LogErrorFormat("AssetBundle.LoadAsset failed, no name:{0}. ", fileName);
            return null;
        }
        FixShader(obj as GameObject);
        _allRes[assetBundleName] = obj;
        return obj;
#else
        string path = jRes.ResourcePath.ToString();
        Object res = Resources.Load(path);
        if (res == null)
        {
            Debug.LogErrorFormat("Load Resource {0} failed, error path:", path);
            return null;
        }
        else
            return res;
#endif
    }

    public static void FixScene()
    {
#if UNITY_EDITOR
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in gos)
            FixShader(go);
#endif
    }

    public static void FixShader(GameObject go)
    {
#if UNITY_EDITOR
        if (go == null) return;
        Shader shader;
        Shader shader2;
        Material[] materials;
        Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            materials = componentsInChildren[i].sharedMaterials;
            for (int k = 0; k < materials.Length; k++)
            {
                if (materials[k] == null) continue;
                shader = materials[k].shader;
                if (shader == null) continue;
                shader2 = Shader.Find(shader.name);
                if (shader2 == null) continue;
                materials[k].shader = shader2;
            }
        }
        PostEffectBase[] posteffects = go.GetComponentsInChildren<PostEffectBase>(true);
        for (int idx = 0; idx < posteffects.Length; ++idx)
        {
            PostEffectBase effect = posteffects[idx];
            if (effect.material != null)
            {
                Shader effectShader = effect.material.shader;
                if (effectShader != null)
                {
                    Shader effectShader2 = Shader.Find(effectShader.name);
                    if(effectShader2 != null)
                        effect.material.shader = effectShader2;
                }
            }
        }
#endif
    }

    public Sprite LoadSprite(int id)
    {
        var jSprite = JsonMgr.GetSingleton().GetSpriteArrayByID(id);
        if (jSprite == null)
        {
            Debug.LogErrorFormat("LoadSprite failed, no such spriteid:{0}", id);
            return null;
        }
        string name = jSprite.ResourcePath.ToString();
#if USE_ASSETBUNDLE
        //如果没有就尝试load一下
        if (!_allSprite.ContainsKey(name))
        {
            string assetBundleName = jSprite.ResourceName.ToString();
            if (_allAssetBundles.ContainsKey(assetBundleName))
            {
                Sprite[] sprites = _allAssetBundles[assetBundleName].LoadAllAssets<Sprite>();
                for (int idx = 0; idx < sprites.Length; ++idx)
                {
                    Sprite sp = sprites[idx];
                    _allSprite.Add(sp.name, sp);
                }
            }
        }
        if (_allSprite.ContainsKey(name))
            return _allSprite[name];
        Debug.LogErrorFormat("LoadSprite failed，no such sprite name:{0} or the bundle has not been loaded.", name);
        return null;
#else
		try
		{
			return (Resources.Load(string.Format("Sprite/{0}", name)) as GameObject).GetComponent<SpriteRenderer>().sprite;
		}catch(System.Exception e)
		{
			throw new System.Exception (name.ToString());
		}
#endif
    }

    /// <summary>
    /// 加载音乐、音效
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AudioClip LoadSound(int id)
    {
        var jResObj = JsonMgr.GetSingleton().GetSoundArrayByID(id);
        if (jResObj == null)
            return null;
#if USE_ASSETBUNDLE
        //音效和资源不同，音效的AssetBundle中包含多个资源，每一个是以资源名命名的，所以这里用ResourcePath
        string resName = jResObj.ResourcePath.ToString();
        if (_allAudioClip.ContainsKey(resName))
        {
            return _allAudioClip[resName];
        }
        else
        {
            string assetBundleName = jResObj.ResourceName.ToString();
            if (!_allAssetBundles.ContainsKey(assetBundleName))
            {
                Debug.LogErrorFormat("Load Resource {0} failed, assetbundle {1} not loaded", resName, assetBundleName);
                return null;
            }
            AssetBundle bundle = _allAssetBundles[assetBundleName];
            AudioClip[] audioClips = bundle.LoadAllAssets<AudioClip>();
            for (int idx = 0; idx < audioClips.Length; ++idx)
            {
                _allAudioClip.Add(audioClips[idx].name, audioClips[idx]);
            }
            if (!_allAudioClip.ContainsKey(resName))
            {
                Debug.LogErrorFormat("Load Resource {0} failed, assetbundle {1} loaded but no such res", resName, assetBundleName);
                return null;
            }
            return _allAudioClip[resName];
        }
#else
        string path = "Sound/" + jResObj.ResourcePath.ToString();
        Object obj = Resources.Load(path);
        if (obj == null)
        {
            Debug.LogFormat("Load Resource failed，invalid path:{0}", path);
            return null;
        }
        return (obj as GameObject).GetComponent<AudioSource>().clip;
#endif
    }

    public string LoadData(string name)
    {
#if USE_ASSETBUNDLE
        if (_allData.ContainsKey(name))
        {
            return _allData[name];
        }
        else
        {
            EDebug.LogErrorFormat("Load {0} from assetbundle failed", name);
            return "";
        }
#else
        return (Resources.Load(string.Format(PathUtil.Instance.DataPath, name), typeof(TextAsset)) as TextAsset).text;
#endif
    }

    public byte[] LoadLua(string name)
    {
#if USE_ASSETBUNDLE
        if (_allLua.ContainsKey(name))
        {
            return _allLua[name].bytes;
        }
        else
        {
            EDebug.LogErrorFormat("Load {0} from assetbundle failed", name);
            return null;
        }
#else
        return (Resources.Load(name, typeof(TextAsset)) as TextAsset).bytes;
#endif
    }

    /// <summary>
    /// 协程预加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private void _preloadScene(string sceneName)
    {
        List<int> preloadList = PreloadMgr.Instance.GetPreloadList(sceneName);
        List<string> preloadAssetBundleNames = new List<string>();
        preloadAssetBundleNames.Add(string.Format("scene_{0}.uab", sceneName.ToLower()));
        for (int idx = 0; idx < preloadList.Count; ++idx)
        {
            var jRes = JsonMgr.GetSingleton().GetResArrayByID(preloadList[idx]);
            if (jRes == null)
            {
                Debug.LogErrorFormat("_coroutinePreLoad GetResJson failed, resId:{0}", preloadList[idx]);
                continue;
            }
            string assetBundleName = jRes.ResourceName.ToString();
            if (preloadAssetBundleNames.Contains(assetBundleName))
                continue;
            preloadAssetBundleNames.Add(assetBundleName);
        }
        List<string> _allDeps = new List<string>();
        for (int idx = 0; idx < preloadAssetBundleNames.Count; ++idx)
        {
            string[] deps = _mainFeast.GetAllDependencies(preloadAssetBundleNames[idx]);
            for (int did = 0; did < deps.Length; ++did)
            {
                string dep = deps[did];
                if (!_allDeps.Contains(dep))
                    _allDeps.Add(dep);
            }
        }
        for (int idx = 0; idx < _allDeps.Count; ++idx)
        {
            if (!preloadAssetBundleNames.Contains(_allDeps[idx]))
                preloadAssetBundleNames.Add(_allDeps[idx]);
        }

        _loadList = preloadAssetBundleNames;
        _loadingList.Clear();
        count = _loadList.Count;
        cur = 0;
        for (int cid = 0; cid < 10; ++cid)
        {
            ProcessCtrl.Instance.GoCoroutine(string.Format("StartLoadNextAssetBundle_{0}", cid), startLoadAssetBundle());
        }
    }

    private IEnumerator startLoadAssetBundle()
    {
        if (_loadList.Count == 0)
        {
            if (_loadingList.Count == 0)
            {
                ZEventSystem.Dispatch(EventConst.OnScenePreload);
            }
            yield break;
        }
        string curLoad = _loadList[0];
        _loadList.RemoveAt(0);
        if (_getAssetBundle(curLoad) != null)
        {
            ProcessCtrl.Instance.GoCoroutine(string.Format("StartLoadNextAssetBundle_{0}", curLoad), startLoadAssetBundle());
        }
        else
        {
            _loadingList.Add(curLoad);
            ProcessCtrl.Instance.GoCoroutine(string.Format("LoadAsssetBundle_{0}", curLoad), _cotoutineLoadAssetBundleWithoutDependence(curLoad));
        }
    }

    /// <summary>
    /// 同步加载一个AssetBundle, 考虑依赖
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    private AssetBundle _loadAssetBundle(string bundleName)
    {
        AssetBundle bundle = _getAssetBundle(bundleName);
        if (bundle != null)
            return bundle;
        string[] deps = _mainFeast.GetAllDependencies(bundleName);
        for (int idx = 0; idx < deps.Length; ++idx)
            _loadAssetBundle(deps[idx]);
        return _loadAssetBundleWithoutDependence(bundleName);
    }

    /// <summary>
    /// 同步加载单个AssetBundle，不考虑依赖
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    private AssetBundle _loadAssetBundleWithoutDependence(string bundleName)
    {
        if (_allAssetBundles.ContainsKey(bundleName))
            return _allAssetBundles[bundleName];
        string assetBundlePath = PathUtil.CheckPath(bundleName);
        if (!File.Exists(assetBundlePath))
        {
            Debug.LogErrorFormat("ResourceMgr._coroutineLoadAssetBundle failed, invalid path:{0}", assetBundlePath);
            return null;
        }
        byte[] bytes = File.ReadAllBytes(assetBundlePath);
        //bytes = CryptoService.CreateDescryptByte(bytes);
        AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
        _allAssetBundles.Add(bundleName, bundle);
        return bundle;
    }

    /// <summary>
    /// 协程加载单个AssetBundle，不考虑依赖
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    private IEnumerator _cotoutineLoadAssetBundleWithoutDependence(string bundleName)
    {
        string assetBundlePath = PathUtil.CheckPath(bundleName);
        if (!File.Exists(assetBundlePath))
        {
            Debug.LogErrorFormat("ResourceMgr._coroutineLoadAssetBundle failed, invalid path:{0}", assetBundlePath);
            _loadingList.Remove(bundleName);
            yield break;
        }
        byte[] bytes = File.ReadAllBytes(assetBundlePath);
        //bytes = CryptoService.CreateDescryptByte(bytes);
        //AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(bytes);
        yield return request;
        AssetBundle bundle = request.assetBundle;
        _allAssetBundles.Add(bundleName, bundle);

        _loadingList.Remove(bundleName);

        cur++;
        CanvasView.Instance.SetLoading((float)cur / count);

        yield return null;
        ProcessCtrl.Instance.GoCoroutine(string.Format("StartLoadNextAssetBundle_{0}", bundleName), startLoadAssetBundle());
    }

    private AssetBundle _getAssetBundle(string bundleName)
    {
        return (_allAssetBundles.ContainsKey(bundleName) ? _allAssetBundles[bundleName] : (_constAssetBundle.ContainsKey(bundleName) ?
            _constAssetBundle[bundleName] : null));
    }


}
