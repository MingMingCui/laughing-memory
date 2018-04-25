using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightModule : ModuleBase {

    public override void Register()
    {
        RegistEvent((int)Msg.ServerMsgId.SCMD_RESP_BATTLE_BEGIN, OnBattleStart);
        RegistEvent((int)Msg.ServerMsgId.SCMD_RESP_BATTLE_END, OnBattleEnd);
    }

    public void OnBattleStart(ServerMsgObj msgObj)
    {
        Debug.LogFormat("OnBattleStart {0}", msgObj.Msg);
    }

    public void OnBattleEnd(ServerMsgObj msgObj)
    {
        Debug.LogFormat("OnBattleEnd {0}", msgObj.Msg);
    }
}
