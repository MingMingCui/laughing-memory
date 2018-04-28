using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnMsgDelegate(ServerMsgObj msg);

public class ModuleBase
{
    public Dictionary<int, OnMsgDelegate> ModuleEvents = new Dictionary<int, OnMsgDelegate>();

    public ModuleBase()
    {
        Register();
    }

    public virtual void Register()
    {

    }

    public void RegistEvent(int msgId, OnMsgDelegate onMsg)
    {
        ModuleEvents.Add(msgId, onMsg);
    }

    public virtual bool OnMsg(ServerMsgObj msg)
    {
        if (ModuleEvents.ContainsKey(msg.MsgId))
        {
            ModuleEvents[msg.MsgId](msg);
            return true;
        }
        return false;
    }
    

}
