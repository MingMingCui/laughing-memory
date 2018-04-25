using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class LuaMgr : Singleton<LuaMgr>, IUpdate {

    public LuaEnv luaEnv = null;

    public LuaMgr()
    {
        //只有HOTFIX_ENABLE模式下才启用热更新，免得麻烦
#if HOTFIX_ENABLE
        ProcessCtrl.Instance.AddUpdate(this);

        luaEnv = new LuaEnv();

        luaEnv.AddLoader((ref string filepath) => {
            return ResourceMgr.Instance.LoadLua(filepath + ".lua");
        });

        luaEnv.DoString("require 'main'");
#endif
    }

    public void Update()
    {
        if(luaEnv != null)
            luaEnv.Tick();
    }

    ~LuaMgr()
    {
        if(luaEnv != null)
            luaEnv.Dispose();
    }
}
