using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate
{
    void Update();
}

public class ProcessCtrl : MonoBehaviour
{
    public static ProcessCtrl Instance;
    public bool Pause = false;

    public WaitForSeconds WaitForMoment = new WaitForSeconds(0.1f);

    public WaitForSeconds WatiForOneSecond = new WaitForSeconds(1.0f);

    public WaitForSeconds StubDragDelay = null;

    // Use this for initialization
    List<string> _logs = new List<string>(); 
	void Awake () {
		DontDestroyOnLoad(this.gameObject);
	    Instance = this;
	    Application.logMessageReceived += LogTrace;
        AppMgr.Instance.Init();
	}

    private void LogTrace(string log, string traceback, LogType type)
    {
        _logs.Add(log);
        if(_logs.Count>20)
            _logs.RemoveAt(0);
    }

    private List<IUpdate> _updateList = new List<IUpdate>(); 
	
	// Update is called once per frame
	void Update ()
	{
	    if (Pause)
	        return;
	    for (int idx = 0; idx < _updateList.Count; ++idx)
	    {
            _updateList[idx].Update();
	    }
	}

    public bool AddUpdate(IUpdate obj)
    {
        if (obj == null)
            return false;
        if (_updateList.Contains(obj))
            return false;
        _updateList.Add(obj);
        return true;
    }

    public bool RemoveUpdate(IUpdate obj)
    {
        if (obj == null)
            return false;
        if (!_updateList.Contains(obj))
            return false;
        _updateList.Remove(obj);
        return true;
    }

    public void ClearUpdate()
    {
        _updateList.Clear();
    }

    private Dictionary<string, Coroutine> _allCoroutine =  new Dictionary<string, Coroutine>();

    public bool GoCoroutine(string name, IEnumerator routine)
    {
        if (_allCoroutine.ContainsKey(name))
        {
            Debug.LogWarningFormat("Coroutine {0} is running", name);
            return false;
        }
        else
        {
            _allCoroutine[name] = StartCoroutine(CoroutineWrapper(name, routine));
            return true;
        }
    }

    IEnumerator CoroutineWrapper(string name, IEnumerator routine)
    {
        yield return routine;

        //coroutine结束
        if (_allCoroutine.ContainsKey(name))
        {
            _allCoroutine.Remove(name);
        }
    }

    public bool KillCoroutine(string name)
    {
        if (!_allCoroutine.ContainsKey(name))
        {
            return false;
        }
        else
        {
            StopCoroutine(_allCoroutine[name]);
            _allCoroutine.Remove(name);
            return true;
        }
    }

    public void KillAllCoroutine()
    {
        foreach (var c in _allCoroutine)
        {
            StopCoroutine(c.Value);
        }
        _allCoroutine.Clear();
        StopAllCoroutines();
    }

    //void OnGUI()
    //{
    //    for (int idx = 0; idx < _logs.Count; ++idx)
    //    {
    //        GUI.Label(new Rect(100, idx * 20 + 100, 2000, 20), _logs[idx]);
    //    }
    //}
}
