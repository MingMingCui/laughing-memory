using Msg;
using UnityEngine;


public enum States
{
    wei =1,
    shu,
    wu
}

public class SelectStateView : SelectStateViewBase
{
    public States State = States.wu;

    public void Init()
    {
        wei_a_img.alphaHitTestMinimumThreshold = 0.1f;
        wei_b_img.alphaHitTestMinimumThreshold = 0.1f;
        shu_a_img.alphaHitTestMinimumThreshold = 0.1f;
        shu_b_img.alphaHitTestMinimumThreshold = 0.1f;
        wu_a_img.alphaHitTestMinimumThreshold = 0.1f;
        wu_b_img.alphaHitTestMinimumThreshold = 0.1f;
    }
    /// <summary>
    ///国家选择
    /// </summary>
    /// <param name="_state">国家</param>
    public void StateNum(States _state)
    {
        State = _state;
    }
    /// <summary>
    /// 随机获取国家
    /// </summary>
    public void RandomState()
    {
        zhezhao_obj.SetActive(true);
        string state = "";
        float time = 0;
        int idx = 0;
        while (true)
        {
          
            time += Time.deltaTime;
            idx = Random.Range(1, 4);
            switch (idx)
            {
                case 1:
                    State = States.wei;
                    state = "系统提示：恭喜主公选择了<color=#00FF01FF>魏国</color>，即将进入游戏！";
                    break;
                case 2:
                    State = States.shu;
                    state = "系统提示：恭喜主公选择了<color=#00FF01FF>蜀国</color>，即将进入游戏！";
                    break;
                case 3:
                    State = States.wu;
                    state = "系统提示：恭喜主公选择了<color=#00FF01FF>吴国</color>，即将进入游戏！";
                    break;
            }
            
            if (time >= 3)
                break;
        }
        CanvasView.Instance.AddNotice(state);
        Client.Instance.Send(ServerMsgId.CCMD_ROLE_STATE, (int)State, 0, Role.Instance.RoleId);
        Invoke("OpenGame", 2);
    }
    /// <summary>
    /// 显示提示信息
    /// </summary>
    public void Hint()
    {
        zhezhao_obj.SetActive(true);
        string state = "";
        switch (State)
        {
            case States.wei:
                state = "系统提示：恭喜主公选择了<color=#00FF01FF>魏国</color>，即将进入游戏！";
                break;
            case States.shu:
                state = "系统提示：恭喜主公选择了<color=#00FF01FF>蜀国</color>，即将进入游戏！"; 
                break;
            case States.wu:
                state = "系统提示：恭喜主公选择了<color=#00FF01FF>吴国</color>，即将进入游戏！"; 
                break;
        }
        CanvasView.Instance.AddNotice(state);
        Invoke("OpenGame" ,2);
    }
    public void OpenGame()
    {
        UIFace.GetSingleton().Close(UIID.SelectCountry);
      //  UIFace.GetSingleton().OpenUI(1003);
    }
}
