using System.Collections.Generic;
using JsonData;
using UnityEngine;

public class UIMgr 
{
    private Dictionary<UIID, IUICtrl> _register;
    protected List<UIID> openList;
    private Dictionary<string, int> layerOrder;

    protected UIMgr()
    {
        _register = new Dictionary<UIID, IUICtrl>();
        openList = new List<UIID>();
        layerOrder = new Dictionary<string, int>
        {
            {"Default",0 },
            {"UI1",0 },
            {"UI2",0 },
            {"UI3",0 }
        };       
    }

    protected IUICtrl GetCtrl(UIID id)
    {
        IUICtrl ctrl;
        _register.TryGetValue(id, out ctrl);
        return ctrl;
    }
    protected void Registration(UIID id, IUICtrl ctrl)
    {
        _register.Add(id, ctrl);
    }
    /// <summary>
    /// 打开一个UI
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected UIConfig OpenUI(UIID id)
    {
        IUICtrl ctrl = _register[id];
        UIConfig uiJson = JsonMgr.GetSingleton().GetUIConfigByID((int)id);
        if (uiJson == null)
            return null;
        UIViewBase vb = ctrl.GetView();
        if (vb == null)
        {
            GameObject uigo = GameObject.Instantiate(ResourceMgr.Instance.LoadResource(uiJson.Resid) as GameObject);
            if (uigo == null)
                throw new System.Exception("加载UI资源出错 -------- " + uiJson.Resid);
            vb = uigo.GetComponent<UIViewBase>();

            ctrl.SetView(vb);
            ctrl.OnInit();
            vb.AddComponent(uiJson);
        }
        vb.SetOrder(++layerOrder[uiJson.Layer]);

        if (!ctrl.IsOpen())
        {
            vb.SetView(true);
            ctrl.OnOpen();
        }
        if (!openList.Contains(id))
        {
            if ((uiJson.Layer.Equals("UI1") || uiJson.Layer.Equals("Default")))
            {
                Debug.LogFormat("Add into OpenList {0}", id);
                openList.Add(id);
            }
        }
        else
        {
            //换到队尾
            openList.Remove(id);
            openList.Add(id);
        }
        return uiJson;
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected void CloseUI(UIID id)
    {
        if (!_register.ContainsKey(id))
            return;
        if (!_register[id].OnClose())
            return;
        _register[id].isOpen = false;
        UIViewBase closeView = _register[id].GetView();
        closeView.SetView(false);
        --layerOrder[closeView.config.Layer];
    }

    public void DestroyUI()
    {
        foreach (var p in _register)
        {
            if (p.Value.IsOpen())
            {
                CloseUI(p.Key);
            }
            if (p.Value.GetView() != null)
                p.Value.OnDestroy();
        }
        openList.Clear();       
    }
}

