using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StrengthenCtrl : UICtrlBase<StrengthenView>
{
    private EquipData equip;

    public override void SetParameters(object[] arg)
    {
        if (arg.Length > 0)
            equip = (EquipData)arg[0];
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        mView.Open(equip);
    }

    public override bool OnClose()
    {
        mView.Close();
        return base.OnClose();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
