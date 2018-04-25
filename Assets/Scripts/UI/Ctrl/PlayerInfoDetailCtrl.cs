using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoDetailCtrl : UICtrlBase<PlayerInfoDetailView> {

    private bool isPvp = false;

    public override void SetParameters(object[] arg)
    {
        base.SetParameters(arg);
        if (arg.Length > 0)
        {
            isPvp = (bool)arg[0];
        }
        else
            isPvp = false;
    }

    public override void OnOpen()
    {
        base.OnOpen();
        initEvent(true);
        List<Vector2Int> stubdata = Role.Instance.GetStubData(StubType.PVE);
        Dictionary<int, int> stubDic = new Dictionary<int, int>();
        for (int idx = 0; idx < stubdata.Count; ++idx)
        {
            stubDic.Add(stubdata[idx].x, stubdata[idx].y);
        }
        this.mView.SetInfo(0, "董小雨", 13, "我的军团", stubDic, isPvp, 1234, 4321);
    }

    public override bool OnClose()
    {
        initEvent(false);
        return base.OnClose();
    }

    private void initEvent(bool open)
    {
        if (open)
        {
            this.mView.close_btn.onClick.AddListener(() => {
                UIFace.GetSingleton().Close(UIID.PlayerInfoDetail);
            });
        }
        else
        {
            this.mView.close_btn.onClick.RemoveAllListeners();
        }
    }
}
