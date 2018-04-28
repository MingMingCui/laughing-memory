using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSelectCtrl : UICtrlBase<FlagSelectView> {
    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.CreateFlagNode();
    }
}
