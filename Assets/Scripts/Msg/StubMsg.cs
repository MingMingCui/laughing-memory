using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Msg.StubMsg
{
    /// <summary>
    /// 布阵消息
    /// </summary>
    [System.Serializable]
    public class StubMsg
    {
        public StubNode[] pve;
        public StubNode[] pvpattack;
        public StubNode[] pvpdefend;
        public StubNode[] march;
    }

    [System.Serializable]
    public class StubNode
    {
        public int heroid;
        public int pos;
    }
}
