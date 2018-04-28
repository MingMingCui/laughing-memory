using UnityEngine;

public class LoginCtrl : UICtrlBase<LoginView>
{

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        this.mView.CreateLoginBg();
        string username = PlayerPrefs.GetString("username");
        mView.account_input.text = username;
        string passwd = PlayerPrefs.GetString("passwd");
        mView.password_input.text = passwd;

        InitEvent(true);
    }
    public void InitEvent(bool open)
    {
        if (open)
        {
            ZEventSystem.Register(EventConst.OnMsgLogin, this, "OnLogin");
            ZEventSystem.Register(EventConst.OnMsgLoginFailed, this, "OnLoginFailed");
            ZEventSystem.Register(EventConst.OnMsgRegFailed, this, "OnRegFailed");
            mView.login_btn.onClick.AddListener(()=>{ LoginOrRegist(true); });
            mView.signin_btn.onClick.AddListener(() => { LoginOrRegist(false); });
        }
        else
        {
            ZEventSystem.DeRegister(EventConst.OnMsgLogin, this);
            ZEventSystem.DeRegister(EventConst.OnMsgLoginFailed, this);
            ZEventSystem.DeRegister(EventConst.OnMsgRegFailed, this);
            mView.login_btn.onClick.RemoveAllListeners();
            mView.signin_btn.onClick.RemoveAllListeners();
        }
    }

    public override bool OnClose()
    {
        base.OnClose();
        InitEvent(false);
        return true;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void LoginOrRegist(bool login)
    {
        if (string.IsNullOrEmpty(mView.account_input.text) || string.IsNullOrEmpty(mView.password_input.text))
            return;
        Client.Instance.SetLoginData(mView.account_input.text, mView.password_input.text, login);
        Client.Instance.Connect();
    }

    public void OnLogin(ServerMsgObj msgObj)
    {
        if(Role.Instance.RoleName == "")
        {
            UIFace.GetSingleton().Close(UIID.Login);
            UIFace.GetSingleton().Open(UIID.CreateRole);
        }
        else
        {
            PlayerPrefs.SetString("username", mView.account_input.text);
            PlayerPrefs.SetString("passwd", mView.password_input.text);
            SceneMgr.Instance.LoadScene("Main");
        }
    }

    public void OnLoginFailed(ServerMsgObj msgObj)
    {
        Debug.Log("OnLoginFailed " + msgObj.Msg);
    }

    public void OnRegFailed(ServerMsgObj msgObj)
    {
        Debug.Log("OnRegFailed " + msgObj.Msg);
    }
}
