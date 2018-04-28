using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Msg.StubMsg;

public class StubCtrl : UICtrlBase<StubView>
{
    private int _openType = 0;      //打开第几个页签
    private StubType _stubType = StubType.PVE;      //打开何种类型布阵
    public override void SetParameters(object[] arg)
    {
        base.SetParameters(arg);
        if (arg.Length > 0)
            _openType = (int)arg[0];
        else
            _openType = 0;
        if (arg.Length > 1)
            _stubType = (StubType)arg[1];
        else
            _stubType = StubType.PVE;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        _initEvent(true);

        this.mView.InitStubData(_stubType);
        this.mView.InitHero();
        this.mView.Tabs_obj.GetComponent<UITabs>().OpenTab(_openType);
    }

    public override bool OnClose()
    {
        if (_checkStubSave())
            return false;
        base.OnClose();
        _initEvent(false);
        this.mView.Clear();
        return true;
    }

    private bool _checkStubSave()
    {
        Dictionary<int, int> uiStubData = this.mView.StubData;
        List<Vector2Int> roleStubData = Role.Instance.GetStubData(_stubType);
        bool hasChanged = false;
        if (uiStubData.Count != roleStubData.Count)
            hasChanged = true;
        else
        {
            for (int idx = 0; idx < roleStubData.Count; ++idx)
            {
                if (!uiStubData.ContainsKey(roleStubData[idx].x) || uiStubData[roleStubData[idx].x] != roleStubData[idx].y)
                {
                    hasChanged = true;
                    break;
                }
            }
        }
        if (hasChanged)
        {
            TipCtrl ctrl = (TipCtrl)UIFace.GetSingleton().Open(UIID.Tip, 2);
            ctrl.SetHandler(
                delegate () {
                    this.mView.InitStubData(_stubType);
                    UIFace.GetSingleton().Close(UIID.Tip);
                    UIFace.GetSingleton().Close(UIID.Stub);
                },
                delegate ()
                {
                    SaveStub();
                    UIFace.GetSingleton().Close(UIID.Tip);
                });
        }
        return hasChanged;
    }

    private void _initEvent(bool open)
    {
        if (open)
        {
            ZEventSystem.Register(EventConst.OnStubSaveOver, this, "OnStubSaveOver");
            this.mView.StubCommit_btn.onClick.AddListener(delegate ()
            {
                SaveStub();
            });
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnStubSaveOver);
            this.mView.StubCommit_btn.onClick.RemoveAllListeners();
        }
    }

    public void OnStubSaveOver()
    {
        CanvasView.Instance.AddNotice(JsonMgr.GetSingleton().GetGlobalStringArrayByID(2003).desc);
        //同步给Role
        var stubData = Role.Instance.GetStubData(_stubType);
        stubData.Clear();
        foreach (var p in mView.StubData)
        {
            stubData.Add(new Vector2Int(p.Key, p.Value));
        }
        ZEventSystem.Dispatch(EventConst.OnStubChange);
        UIFace.GetSingleton().Close(UIID.Stub);
    }

    public void SaveStub()
    {
        StubMsg msg = new StubMsg();

        var StubDatas = Role.Instance.GetStubDatas();
        foreach (var p in StubDatas)
        {
            StubNode[] stubList = null;
            int stubCnt = p.Key == _stubType ? mView.StubData.Count : p.Value.Count;
            switch (p.Key)
            {
                case StubType.PVE:
                    stubList = msg.pve = new StubNode[stubCnt];
                    break;
                case StubType.PVPAttack:
                    stubList = msg.pvpattack = new StubNode[stubCnt];
                    break;
                case StubType.PVPDefend:
                    stubList = msg.pvpdefend = new StubNode[stubCnt];
                    break;
                case StubType.March:
                    stubList = msg.march = new StubNode[stubCnt];
                    break;
                default:
                    break;
            }
            if (p.Key == _stubType)
            {
                int stubidx = 0;
                foreach (var s in mView.StubData)
                {
                    StubNode node = new StubNode();
                    node.pos = s.Key;
                    node.heroid = s.Value;
                    stubList[stubidx++] = node;
                }
            }
            else
            {
                int stubidx = 0;
                foreach (var s in p.Value)
                {
                    StubNode node = new StubNode();
                    node.pos = s.x;
                    node.heroid = s.y;
                    stubList[stubidx++] = node;
                }
            }
        }
        
        Debug.LogFormat("SaveStub {0}", JsonUtility.ToJson(msg));
        Client.Instance.Send(Msg.ServerMsgId.CCMD_SAVE_FORMATION, msg, 0, Role.Instance.RoleId);
    }
}
