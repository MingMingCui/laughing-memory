using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MsgFactory : Singleton<MsgFactory> {

    private List<ModuleBase> _modules = new List<ModuleBase>() {
        new LoginModule(),
        new StubModule(),
        new ShopModule()
    };

    public void OnRecvMsg(ServerMsgObj serverMsg)
    {
        int msgId = serverMsg.MsgId;
        bool msgHandled = false;
        for (int idx = 0; idx < _modules.Count; ++idx)
        {
            if (_modules[idx].OnMsg(serverMsg) && !msgHandled)
                msgHandled = true;
        }
        if (!msgHandled)
        {
            EDebug.LogWarningFormat("MsgFactory处理消息失败，未处理的消息id:{0} sub_id:{1} msg:\"{2}\"",
                serverMsg.MsgId, serverMsg.SubId, serverMsg.Msg);
        }
    }
}
