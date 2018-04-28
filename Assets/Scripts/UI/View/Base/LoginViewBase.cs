using UnityEngine;
using UnityEngine.UI;

public class LoginViewBase : UIViewBase 
{   
    [HideInInspector]
    public Transform mTransform;
    [HideInInspector]
    public Image logintitle_img;
    [HideInInspector]
    public Image account_img;
    [HideInInspector]
    public InputField account_input;
    [HideInInspector]
    public Image password_img;
    [HideInInspector]
    public InputField password_input;
    [HideInInspector]
    public Button login_btn;
    [HideInInspector]
    public Button signin_btn;
    void Awake ()
    {
        this.go = this.gameObject;
        this.mTransform = this.transform;
       logintitle_img = mTransform.Find("logintitle_img").GetComponent<Image>();
       account_img = mTransform.Find("account_img").GetComponent<Image>();
       account_input = mTransform.Find("account_img/account_input").GetComponent<InputField>();
       password_img = mTransform.Find("password_img").GetComponent<Image>();
       password_input = mTransform.Find("password_img/password_input").GetComponent<InputField>();
       login_btn = mTransform.Find("login_btn").GetComponent<Button>();
       signin_btn = mTransform.Find("signin_btn").GetComponent<Button>();
    }
}