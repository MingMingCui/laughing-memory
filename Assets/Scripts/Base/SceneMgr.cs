using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : Singleton<SceneMgr>
{

    private string sceneToLoad = "";
    private AsyncOperation _async;

    public SceneMgr()
    {
        ZEventSystem.Register(EventConst.OnScenePreload, this, "OnLoadSceneOver");
        ZEventSystem.Register(EventConst.OnPreload, this, "OnPreload");
        ZEventSystem.Register(EventConst.SceneLoaded, this, "OnSceneLoaded");
    }

    public void LoadScene(string sceneName)
    {
        ProcessCtrl.Instance.KillAllCoroutine();
        UIFace.GetSingleton().DestroyUI();
        SoundMgr.Instance.Clear();
        //每次换场景的时候调用Lua的GC一下，防止资源被引用导致无法释放
#if HOTFIX_ENABLE
        LuaMgr.Instance.luaEnv.DoString(@"collectgarbage('collect')");
#endif
        ResourceMgr.Instance.Clear();
        Resources.UnloadUnusedAssets();
        ResourceMgr.Instance.ClearAssetBundle();
        GC.Collect();
        sceneToLoad = sceneName;
        SceneManager.LoadScene("Loading");
#if USE_ASSETBUNDLE
        CanvasView.Instance.OpenLoading();
        ResourceMgr.Instance.PreloadScene(sceneName);
#else
        SoundMgr.Instance.Clear();
        SceneManager.LoadScene(sceneToLoad);
        ZEventSystem.Dispatch(EventConst.SceneLoaded, sceneToLoad);
#endif
    }

    public IEnumerator _load()
    {
        if (sceneToLoad == "")
            yield break;
        _async = SceneManager.LoadSceneAsync(sceneToLoad);
        yield return _async;
        //ResourceMgr2.Instance.ClearAssetBundle();
        CanvasView.Instance.CloseLoading();
        ResourceMgr.FixScene();
        ZEventSystem.Dispatch(EventConst.SceneLoaded, sceneToLoad);
    }


    /// <summary>
    /// 场景加载完成，打开初始需要的界面
    /// </summary>
    /// <param name="name"></param>
    public void OnSceneLoaded(string name)
    {
        if (name == "Main")
        {
            ProcessCtrl.Instance.GoCoroutine("PlayBackgroundSound", CoroutinPlayMusic(100002));
            UIFace.GetSingleton().Open(UIID.Main);
        }
        else if (name == "Login")
        {
            ProcessCtrl.Instance.GoCoroutine("PlayBackgroundSound", CoroutinPlayMusic(100001));
            ProcessCtrl.Instance.GoCoroutine("CoroutineOpenLogin", CoroutineOpenLogin());
            
        }
        else if (name == "Game")
        {
            ProcessCtrl.Instance.GoCoroutine("PlayBackgroundSound", CoroutinPlayMusic(100003));
            UIFace.GetSingleton().Open(UIID.Match);
        }
    }

    public IEnumerator CoroutinPlayMusic(int musicid)
    {
        yield return ProcessCtrl.Instance.WaitForMoment;
        SoundMgr.Instance.PlayMusic(musicid);
    }

    public IEnumerator CoroutineOpenLogin()
    {
        yield return null;
        UIFace.GetSingleton().Open(UIID.Login);
    }

    public void OnLoadSceneOver()
    {
        Debug.Log("OnLoadSceneOver");
        SoundMgr.Instance.Clear();
        ProcessCtrl.Instance.GoCoroutine("SceneMgrLoad", _load());
    }

    public void OnPreload()
    {
        ResourceMgr.Instance.PreloadScene(sceneToLoad);
    }

    public float Progress
    {
        get { return _async != null ? _async.progress *100 : 0; }
    }
}
