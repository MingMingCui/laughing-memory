using UnityEngine;
using Msg;
using Msg.StubMsg;

public class StubModule : ModuleBase {

    public override void Register()
    {
        base.Register();
        RegistEvent((int)ServerMsgId.SCMD_RESP_SAVE_FORMATION, OnStubSaveOver);
        RegistEvent((int)ServerMsgId.ECMD_SAVE_FORMATION, OnStubSaveFailed);
    }

    public void OnStubSaveOver(ServerMsgObj msg)
    {
        Debug.LogFormat("OnStubSaveOver {0}", msg.Msg);
        ZEventSystem.Dispatch(EventConst.OnStubSaveOver);
    }

    public void OnStubSaveFailed(ServerMsgObj msg)
    {
        EDebug.LogErrorFormat("Stub Save failed");
    }
}
